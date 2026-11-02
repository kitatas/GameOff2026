---
apply: always
---

# Rule: Unit Test Implementation

Unityプロジェクトのユニットテストを生成・更新する際のルールです。
具体的な実装方法は **`.aiassistant/skills/unit_testing.md`** を参照してください。

## プロジェクト固有設定

ルート namespace・スクリプトのルート・テストのルート・コンテキストは、
**`.aiassistant/rules/00_global.md`「プロジェクト固有設定」** を参照する。
以下には、**テスト固有**のプロジェクト値のみを記載する。
他のドキュメント（agents / skills / 本ファイルの他の節）では、これらを直接書かず、この節を参照する。

| 項目 | 値 |
|---|---|
| テスト用アセンブリ | `Tests.EditMode`（`Tests/EditMode/Tests.EditMode.asmdef`・単一） |
| コンテキスト | `Common` / `Boot` / `Game` |
| 共通フェイク・ヘルパーの置き場 | `{テストのルート}/TestHelpers/`（例: `FakeMessagePipe.cs`） |
| 非同期 / リアクティブ / メッセージング | UniTask / R3 / MessagePipe |
| テストフレームワーク | Unity Test Framework（NUnit） |
| モックライブラリ | **なし**（手書きのフェイクで差し替える） |

## 基本方針

- テスト数ではなく、バグ検出力を優先する
- 実装詳細ではなく、仕様上重要な振る舞いをテストする
- 同じ振る舞いを重複してテストしない
- 適用できないテストケースを無理に作成しない（スキップした場合は理由を報告する）
- 既存テストがある場合は、可能な限りそのスタイルを踏襲する

## ルール確認

- 必ず最初に **`.aiassistant/rules/05_architecture.md`** で、対象クラスの属するレイヤーを特定する
- レイヤー特定後、テスト戦略の決定に必要な情報のみ参照する
- 具体的なテスト実装方法が必要な場合のみ **`.aiassistant/skills/unit_testing.md`** を参照する
- 読んでよいファイルの範囲は **`.aiassistant/agents/test_engineer.md`** に従う

## テスト対象分類

### 優先度 High

- Entity クラス (`Data/Entity` 配下)
- UseCase クラス (`Domain/UseCase` 配下)
- ValueObject クラス (`Application` 配下。ロジック・検証を持つもののみ)

### 優先度 Medium

- Ports の実装クラス (`Domain/Ports` 配下。インターフェース宣言のみのファイルは対象外)
- Presenter クラス (`Presentation/Presenter` 配下)
- Facade クラス (`Presentation/Facade` 配下)
- Utility クラス（拡張メソッド等の純粋ロジック）

### 優先度 Low

- View クラス (`Presentation/View` 配下) ※MonoBehaviour除く
- MonoBehaviour継承クラス ※テスト可能部分のみ

### 対象外

- ロジックを持たない `Const` / `Enum`（`EnumExtension` 等のロジックがある場合は対象）
- インターフェース宣言のみのファイル
- `Generated/` 配下の自動生成コード
- `Installer`（DI 登録のみ）

## ファイル命名・配置・namespace

テストは「プロジェクト固有設定」のテスト用アセンブリに配置する。
配置パスは、元クラスの「スクリプトのルート」以下の相対パスに、**コンテキストを含めて** 対応させる。

| 項目 | 規則 | 例 |
|---|---|---|
| ファイル名 | `{ClassName}Tests.cs` | `SampleEntityTests.cs` |
| 配置パス | `{テストのルート}/{Context}/{元と同じ相対パス}/` | `{テストのルート}/Common/Data/Entity/` |
| namespace | `{Root}.Tests.EditMode.{Context}.{Layer}` | `{Root}.Tests.EditMode.Common.Data.Entity` |

- 新しいアセンブリの参照が必要な場合、テスト用 `.asmdef` の `references` への追加が必要になる。
  `.asmdef` は直接編集せず、不足している場合はユーザーに報告する。
- 同じ型のフェイクが共通フェイクの置き場に既にある場合は、新規作成せず再利用する。
- モックライブラリが「なし」の場合、依存の差し替えは手書きのフェイクで行う。
  複数テストで使うものは共通フェイクの置き場に、1ファイルでのみ使うものはテストファイル内の `private sealed class` として定義する。
  モックライブラリが必要と判断した場合は、導入せずユーザーに提案する。

## テストクラス共通テンプレート

```csharp
[TestFixture]
public sealed class {ClassName}Tests
{
    private {ClassName} _{fieldName};

    [SetUp]
    public void SetUp()
    {
        // インスタンス生成・依存関係のフェイク設定
    }

    [TearDown]
    public void TearDown()
    {
        // IDisposable なリソースがある場合のみ破棄
    }
}
```

## クラス種別ごとのテスト生成指示

以下のパターンのうち、**対象クラスに該当するものを実装する**。該当しないパターンは作成しない。
（例：例外を投げない実装に対して `ShouldThrow` を作らない）

### Entity クラス

- コンストラクタ（初期状態の検証）
- public メソッドの正常系（状態変化・発行される値）
- public メソッドの異常系（実装が例外を投げる、または入力を拒否する場合のみ）
- 境界値（実装が範囲・上限・下限を扱う場合のみ）
- `ReactiveProperty` / `Subject` の通知（購読して発行値を検証する）

プロパティの単純な get/set のみを確認するテストは、バグ検出力が低いため作成しない。

### UseCase クラス

SetUp では **Entity は実インスタンスで生成し**、Repository・Ports は手書きのフェイク（共通フェイクの置き場を参照）に差し替えて UseCase を構築する。
コンストラクタ引数の順序・型は対象ファイルで確認すること。

```csharp
[TestFixture]
public sealed class {UseCaseName}Tests
{
    private {UseCaseName} _useCase;
    private {EntityType} _entity;
    private Fake{PortsType} _ports;
    private Fake{RepositoryType} _repository;

    [SetUp]
    public void SetUp()
    {
        _entity = new {EntityType}();
        _ports = new Fake{PortsType}();
        _repository = new Fake{RepositoryType}();
        _useCase = new {UseCaseName}(_entity, _ports, _repository);
    }
}
```

- 初期化メソッド（存在する場合）が Entity / 依存に与える結果
- ビジネスロジックの正常系（Entity の状態・戻り値で検証する）
- 分岐・異常系（例外、早期 return、境界値）
- 依存への呼び出し検証（Ports への通知、Repository への保存など、**副作用が仕様である場合のみ**。フェイクの `LastPublished` / `PublishCount` 等で検証する）

状態や戻り値で検証できる振る舞いは、呼び出し検証よりも状態検証を優先する。

### ValueObject クラス

- 無効値でのコンストラクタ・ファクトリ（実装が検証する場合のみ）
- 範囲外値・境界値（範囲が定義されている場合のみ）
- 正常値での生成と値の保持
- 等価性・変換など、型が持つ振る舞い

### Ports 実装クラス

インターフェース宣言のみのファイルはテストしない。実装クラスがある場合のみ対象とする。

- 依存（Publisher 等）を介した通知が、正しい引数で行われること
- Subscribe を提供する場合は、登録したハンドラが通知で呼ばれること

共通フェイクの置き場に Publisher / Subscriber のフェイクがある場合は、それを使用する。

### Presenter / Facade クラス

- 購読元の通知に応じて、View（インターフェース）や下位層が正しく呼ばれること
- Facade は、View からの入力が UseCase / Presenter の対応するメソッドへ渡ること
- Unity の `Object` 派生型に依存する部分はテスト対象外とし、報告する

## 品質チェックリスト

生成後に以下を満たしているか確認すること。
- [ ] テスト対象の public な振る舞いのうち、仕様上重要なものにテストが存在する
- [ ] 該当する範囲で、正常系・異常系・境界値が揃っている
- [ ] 依存のフェイク設定・検証が適切（過剰な呼び出し検証をしていない）
- [ ] 各テストが他のテストに依存していない（独立性）
- [ ] テストメソッド名が `{対象}_{条件}_{期待結果}` 形式になっている
- [ ] すべてのテストが Arrange-Act-Assert パターンに従っている
- [ ] `IDisposable` なリソース（購読を含む）が破棄されている
- [ ] 配置パス・namespace が上記規則に一致している

## テスト更新ルール

| 変更種別 | 対応アクション |
| --- | --- |
| 元クラスに新 `public` メソッド追加 | 対応テストメソッドを追加 |
| メソッドシグネチャ変更 | テストを修正 |
| ビジネスロジック変更 | テストケースを見直し |
| 依存関係変更 | `SetUp` のフェイク設定を更新 |

## 制約・ベストプラクティス

### 制約事項

- `MonoBehaviour` は EditMode では限定的なテストのみ実施する
- `UnityEngine` 依存部分はフェイクまたはスタブに置き換える
- 非同期処理は UniTask を使用している場合、`.AsTask()` で変換して `await` する（テストコードでは `async Task` の使用を許可する）

### ベストプラクティス

- 1テストで検証する振る舞いは 1 つにする（関連する複数の `Assert` は可）
- テストデータはテストメソッド内で生成し、外部状態に依存しない
- 外部リソース（ファイル・ネットワーク等）への依存を避ける
- テスト実行順序に依存しない設計にする
- `Assert` のメッセージ引数には、失敗時に原因が分かりにくい場合のみ説明を含める
- テストコードのコメントは、Why の説明が必要な箇所のみに留める（`CLAUDE.md` の「変更箇所へのコメント」はテストには適用しない）
