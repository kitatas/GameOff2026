# Skill: Unit Testing Implementation

このファイルはテストコードを生成する際の**具体的な実装パターン**を定義します。
ルール定義（優先度・命名規則・配置・テンプレート・プロジェクト固有設定等）は **`.aiassistant/rules/10_unit_testing.md`** を参照してください。

## 1. 依存の差し替えパターン（手書きフェイク）

モックライブラリを使わない前提（**`10_unit_testing.md`「プロジェクト固有設定」** 参照）で、手書きのフェイクを使う。
既存のフェイクが共通フェイクの置き場にある場合は、それを再利用する。

```csharp
// 呼び出しの記録・戻り値の制御ができる最小限のフェイク
private sealed class FakeRepository : I{Repository}
{
    public int SaveCount { get; private set; }
    public {Type} LastSaved { get; private set; }
    public Exception ExceptionToThrow { get; set; }

    public UniTask SaveAsync({Type} data, CancellationToken token)
    {
        if (ExceptionToThrow != null)
        {
            // 非同期メソッドの失敗は、例外を投げず失敗した UniTask として返す
            return UniTask.FromException(ExceptionToThrow);
        }

        SaveCount++;
        LastSaved = data;
        return UniTask.CompletedTask;
    }
}
```

- 検証は、フェイクに記録された値・回数（`SaveCount` / `LastSaved`）に対して `Assert.That` で行う
- 呼び出し検証は、副作用が仕様である場合のみ行う
- 実装が増えるメンバは、テストで使うものだけを実装し、それ以外は既定値を返す

## 2. 非同期テストパターン（UniTask）

- `UniTask` を返すメソッドのテストは `.AsTask()` で変換して `await` する
- 例外は `Assert.ThrowsAsync<T>(async () => await _useCase.Method().AsTask())` で検証する
- `CancellationToken` が必要な場合は `CancellationToken.None` を渡す
- **EditMode では PlayerLoop が動作しない**ため、`UniTask.Delay` / `UniTask.Yield` / `UniTask.NextFrame` などを待つ実装は完了しない。
  該当する場合はテスト対象外とするか、待機部分を差し替え可能にする設計をユーザーに提案する

```csharp
[Test]
public async Task {MethodName}_{Condition}_{ExpectedResult}()
{
    // Arrange
    var token = CancellationToken.None;

    // Act
    await _useCase.{MethodName}(token).AsTask();

    // Assert
    Assert.That(_entity.{Property}, Is.EqualTo(expected));
}
```

## 3. R3 / リアクティブプログラミングのテストパターン

- **テスト対象（Entity / UseCase / Presenter）が公開するストリームを購読**し、発行された値をリストに記録して検証する
- `ReactiveProperty` は購読時に **現在値を即時発行**する。初期値を検証対象から外す場合は `Skip(1)` を使う
- 購読は `TearDown` で `Dispose()` する（`CompositeDisposable` の使用を推奨）
- 時間依存の演算子（`Debounce` 等）をテストする場合は `FakeTimeProvider` を使う（実装が `TimeProvider` を受け取れる場合のみ）

```csharp
private CompositeDisposable _disposables;

[SetUp]
public void SetUp()
{
    _disposables = new CompositeDisposable();
    _entity = new {EntityType}();
}

[TearDown]
public void TearDown()
{
    _disposables.Dispose();
}

[Test]
public void Set_WhenValueChanges_PublishesNewValue()
{
    // Arrange
    List<int> published = new();
    _entity.{Stream}.Skip(1).Subscribe(published.Add).AddTo(_disposables);

    // Act
    _entity.Set(42);

    // Assert
    Assert.That(published, Is.EqualTo(new[] { 42 }));
}
```

## 4. AAA パターン（必須）

```csharp
[Test]
public void {MethodName}_{Condition}_{ExpectedResult}()
{
    // Arrange（前提条件の構築）
    var input = ...;

    // Act（テスト対象の実行）
    var result = _sut.{MethodName}(input);

    // Assert（結果の検証）
    Assert.That(result, Is.EqualTo(expected));
}
```

## 5. MonoBehaviour のテスト制約

- `EditMode` では Unity ライフサイクル（`Start`・`Update` 等）は動作しない
- テスト可能な部分（純粋なロジック・`public` メソッド）のみを対象とする
- `UnityEngine` 依存部分はインターフェース経由でフェイクに差し替える
- Unity の `Object` 派生型を `new` で生成しない（`ScriptableObject.CreateInstance` / `GameObject` を使う場合は、テスト後に `Object.DestroyImmediate` で破棄する）
- PlayMode テストが必要な場合はユーザーに確認する
