---
apply: always
---

# Rule: Global Implementation

このドキュメントは、プロジェクト全体の共通コーディング標準、Unity/C# のベストプラクティス、および AI が出力すべきフォーマットを定義します。
AI はコード生成時にこれらのルールを最優先で遵守してください。

## 0. プロジェクト固有設定

プロジェクト名・パス・コンテキスト・環境など、**プロジェクト固有の値はこの節にのみ記載する**。
`.aiassistant/` 配下の他のドキュメントでは、これらを直接書かず、この節を参照する（`{...}` はこの表の値に読み替える）。
別プロジェクトで再利用する場合は、この節（および `10_unit_testing.md` の「プロジェクト固有設定」）を書き換える。

| 項目 | 値 |
|---|---|
| 環境 | Unity 6000.3.17f1 / URP (Universal Render Pipeline) / C# 9.0 / .NET Framework 4.7.1 |
| ルート namespace（`{Root}`） | `GameOff2026` |
| スクリプトのルート | `Assets/GameOff2026/Scripts/` |
| テストのルート | `Assets/GameOff2026/Tests/EditMode/` |
| コンテキスト | `Common` / `Boot` / `Game` |

### コンテキストの役割

| コンテキスト | 役割 |
|---|---|
| `Common` | 全シーン共通の基盤（被依存専用）。BGM/SE・ローディング・例外処理・PlayFab連携・セーブなど横断的関心事 |
| `Boot` | 起動シーン専用。スプラッシュ・ログイン・表示名設定など起動フローを管理 |
| `Game` | ゲームシーン専用。ボード・駒・ジェム・スコア・ラウンド・レリックなどゲームロジックを管理 |

## 1. 言語仕様と基本方針

- **C# バージョン:** C# 9.0+ の標準構文を使用すること
- **最新構文の推奨:** パターンマッチング (`is`, `switch` 式)、パターン型キャスト、ターゲット型の `new` 式 (`List<int> list = new();`) などを積極的に使用してください。
- **名前空間 (Namespace):** すべての新規スクリプトには適切な名前空間を設定してください。
    - 形式: `{Root}.{Context}.{Layer}` （例: `{Root}.Game.Domain.UseCase`）
    - `{Context}` は「プロジェクト固有設定」のコンテキストのいずれか

## 2. アーキテクチャ

詳細なレイヤー構造・依存関係・ファイル命名規則は **`.aiassistant/rules/05_architecture.md`** を必ず参照してください。

### 概要

- **クリーンアーキテクチャ + ヘキサゴナルアーキテクチャ（Ports & Adapters）** を採用
- スクリプトは「スクリプトのルート」配下に配置
- コンテキスト（水平）× レイヤー（垂直）の二次元構造

### 依存方向の原則（必守）

- 依存は **下位レイヤーへの単方向のみ**
- `Common` は `Boot` / `Game` を参照禁止
- `Boot` と `Game` は互いに参照禁止
- `.asmdef` によりコンパイル時に強制される

### 自動生成コードの禁止

- `Generated/` フォルダ配下（MasterMemory / MessagePack）は **手動編集禁止**

## 3. 命名規則 (Naming Conventions)

- **PascalCase:** クラス、構造体、インターフェース、メソッド、プロパティ、Enum の要素
    - インターフェースの接頭辞には `I` を付与する (例: `ICharacterService`)
- **camelCase:** メソッドの引数、ローカル変数
- **_camelCase（アンダースコア開始）:** プライベートなメンバ変数（フィールド）
- **UPPER_CASE:** 定数 (`const`)、読み取り専用の静的変数 (`static readonly`)
- **`[SerializeField]` を付与したプライベート変数:** `camelCase` で統一

### レイヤー別ファイル命名（抜粋）

| 種別 | 命名パターン | 例 |
|---|---|---|
| Entity | `{概念}Entity.cs` | `SampleEntity.cs` |
| UseCase | `{概念}UseCase.cs` | `SampleUseCase.cs` |
| Ports | `{概念}Ports.cs` | `SamplePorts.cs` |
| Repository | `{概念}Repository.cs` | `SampleRepository.cs` |
| Presenter | `{概念}Presenter.cs` | `SamplePresenter.cs` |
| Facade | `{概念}Facade.cs` | `SampleFacade.cs` |
| View | `{概念}View.cs` | `SampleView.cs` |
| Installer | `{Context}Installer.cs` | `GameInstaller.cs` |

詳細は **`.aiassistant/rules/05_architecture.md`** のセクション 3 を参照してください。

## 4. Unity ベストプラクティス（パフォーマンス & 安全性）

- **文字列による参照の禁止:** `GameObject.Find()` や文字列によるコンポーネント・アニメーション指定を避けてください。`nameof()`・シリアライズ参照・`Animator.StringToHash()` を使用してください。
- **Null 条件演算子の制限:** Unity の `Object`（MonoBehaviour・ScriptableObject 等）派生クラスに対して `?.` / `??` を使用しないでください。明示的に `== null` でチェックすること（Unity 独自の偽 null チェック機構との競合を避けるため）。
- **`Update()` 内での `GetComponent` 呼び出し禁止:** `Awake()` / `Start()` でキャッシュしてください。
- **`FindObjectOfType` の多用禁止:** VContainer の DI 経由で参照を解決してください。

## 5. 外部ライブラリの利用方針

### DI（依存関係注入）

- **VContainer** を使用。`Installer` レイヤーの `LifetimeScope` でのみ登録してください。
- **コンストラクタ注入を優先**し、フィールドインジェクションは避けてください。

### 非同期処理

- `async/await` には **UniTask** を全面的に使用してください。
- 戻り値は `UniTask` または `UniTask<T>`
- 適切な `CancellationToken`（例: `destroyCancellationToken`）を伝播させてください。
- 標準の `Task` は使用しないでください。

### リアクティブプログラミング

- **R3** を使用してください。
- `Domain.UseCase` では `ReactiveProperty` / `Subject` の状態を保持します。
- `IDisposable` は必ず `TearDown` または `OnDestroy` で破棄してください。

### 利用可能レイヤーの制約

| ライブラリ | 利用可能レイヤー |
|---|---|
| VContainer | `Installer` のみ |
| R3 | `Domain.*`・`Presentation.*` |
| UniTask | `Domain.*`・`Presentation.*`・`Utility` |
| MasterMemory | `Data.DataStore` のみ |
| MessagePack | `Data.DataStore` のみ（`Generated/` 配下） |
| UnityEngine.UI / TMPro | `Presentation.View` のみ |

## 6. テスト

配置・命名・namespace・テストパターンは **`.aiassistant/rules/10_unit_testing.md`** を参照してください。

- テストは「テストのルート」配下に配置（コンテキストを含めた相対パス）
- テストフレームワーク: **Unity Test Framework（NUnit）**
- 依存の差し替えは手書きのフェイク（モックライブラリは未導入）

## 7. 生成コードの出力フォーマット

- **インデント:** スペース 4 つ（タブは使用しない）
- **中括弧 (`{}`):** オールマンスタイル（改行して配置）
- **コメント:** 複雑なロジックや Unity 固有の仕様を利用している箇所には、日本語で簡潔に理由（Why）を説明するコメントを記述してください。
