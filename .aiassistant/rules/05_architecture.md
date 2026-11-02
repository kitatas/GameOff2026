---
apply: always
---

# Rule: Project Architecture & Dependency

このプロジェクトは以下のアーキテクチャを採用しています。
- **クリーンアーキテクチャ** + **ヘキサゴナルアーキテクチャ（Ports & Adapters）** の組み合わせ
- **VContainer** による DI、**UniTask / R3** によるリアクティブプログラミング
- 被依存専用の基盤コンテキスト（`Common`）と、シーン単位のコンテキスト（`Boot` / `Game`）という **水平 × 垂直** の二次元分割構造

## 1. フォルダ構造と役割

「スクリプトのルート」配下は **コンテキスト（モジュール）** と、その中に共通の **レイヤー構造** を持つ。

### 1-1. コンテキスト（水平分割）

コンテキスト名と役割は **`.aiassistant/rules/00_global.md`「プロジェクト固有設定」** を参照する。
名前空間プレフィックスは `{Root}.{Context}.*`。

### 1-2. レイヤー（垂直分割）

各コンテキストは以下の共通レイヤー構造を持つ。

```
{Context}/
 ├── Application/   # Enum・Const・ValueObject など定数・値オブジェクト定義。依存なし
 ├── Installer/     # DIコンテナへの登録（VContainer）。全レイヤーを参照可能
 ├── Data/
 │ ├── Entity/      # インメモリ状態を保持するエンティティ（ReactiveProperty等）
 │ └── DataStore/   # 外部データ取得・永続化の実装（マスタDB・PlayFab・LocalStorage等）
 │   └── Generated/ # MasterMemory / MessagePack の自動生成コード（手動編集禁止）
 ├── Domain/
 │ ├── Ports/       # Domain層が公開するイベント・通知のインターフェース（PubSub）
 │ ├── Repository/  # DataStoreを抽象化したリポジトリ実装
 │ └── UseCase/     # ビジネスロジック。Entity・Repository・Portsを操作する
 └── Presentation/
   ├── State/       # 画面遷移ステートマシン（StateMachine の各State）
   ├── Presenter/   # UseCaseを購読しViewを更新するPresenter
   ├── Facade/      # Presenterへの命令窓口（ViewからPresenterへの橋渡し）
   └── View/        # MonoBehaviour。UI描画・入力受付のみ担当
     ├── Button/    # ボタン専用Viewの基底・実装
     └── Modal/     # モーダルUI専用Viewの基底・実装
```

### 1-3. 各レイヤーの責務詳細

| レイヤー | アセンブリ例 | 主な責務 |
|---|---|---|
| `Application` | `Common.Application` | 定数・Enum・ValueObject の定義。ロジックを持たない |
| `Installer` | `Game.Installer` | VContainer の `LifetimeScope` でDI登録。シーン起動時のみ実行 |
| `Data.Entity` | `Game.Data.Entity` | `R3` の Subject や ReactiveProperty でゲーム状態を保持 |
| `Data.DataStore` | `Game.Data.DataStore` | MasterMemory テーブル・DTO・LocalStorage の読み書き実装 |
| `Domain.Ports` | `Game.Domain.Ports` | Domain から Presentation への通知チャンネル（インターフェース） |
| `Domain.Repository` | `Game.Domain.Repository` | DataStore を注入してデータアクセスを提供 |
| `Domain.UseCase` | `Game.Domain.UseCase` | Entity・Repository・Ports を組み合わせたビジネスロジック |
| `Presentation.State` | `Game.Presentation.State` | ステートマシンの各ステート。画面遷移の制御 |
| `Presentation.Presenter` | `Game.Presentation.Presenter` | UseCase を購読し、View への反映を担う |
| `Presentation.Facade` | `Game.Presentation.Facade` | View から Presenter へのコマンド窓口 |
| `Presentation.View` | `Game.Presentation.View` | MonoBehaviour。UIの描画と入力検知のみ。ロジックを持たない |
| `Utility` | `Game.Utility` | コンテキスト内で使う汎用ヘルパー（拡張メソッド等） |

## 2. 依存関係のルール

### 2-1. コンテキスト間の依存方向

```
Boot ──→ Common
Game ──→ Common
Boot ✗ Game （Boot は Game を参照してはならない）
Game ✗ Boot （Game は Boot を参照してはならない）
```

- `Common` は **被依存専用**。`Boot` や `Game` を参照してはならない。
- `Boot` と `Game` は **互いに参照禁止**。シーン間の連携は `Common` 経由で行う。

### 2-2. レイヤー間の依存方向（同一コンテキスト内）

依存は **下位レイヤーへの単方向** のみ許可する。

```
Installer
 └──→ Presentation.* / Domain.* / Data.* （全レイヤー参照可。DI登録専用）

Presentation.State
 └──→ Domain.UseCase / Application

Presentation.Presenter
 └──→ Domain.UseCase / Presentation.State / Presentation.Facade / Application

Presentation.Facade
 └──→ Presentation.View / Application

Presentation.View
 └──→ Application （ロジック層への参照禁止）

Domain.UseCase
 └──→ Domain.Repository / Domain.Ports / Data.Entity / Application

Domain.Repository
 └──→ Data.DataStore / Application

Domain.Ports
 └──→ Application （最下位インターフェース層）

Data.Entity
 └──→ Application

Data.DataStore
 └──→ Application （外部ライブラリのみ参照可）
```

#### 禁止事項

| 禁止参照 | 理由 |
|---|---|
| `Presentation.*` → `Data.*` | Presentation は Domain 経由でのみデータにアクセスする |
| `Domain.*` → `Presentation.*` | Domain はUIに依存してはならない |
| `Data.*` → `Domain.*` | 下位レイヤーが上位を参照してはならない |
| `View` → `UseCase` / `Repository` | View はロジックを持たず Facade 経由でのみ操作する |
| `Common.*` → `Boot.*` / `Game.*` | Common は上位コンテキストを知ってはならない |

### 2-3. asmdef による強制ルールのまとめ

`.asmdef` の `references` により、上記ルールはコンパイル時に強制される。

| アセンブリ | 参照先アセンブリ（同コンテキスト内） |
|---|---|
| `*.Application` | ―（外部ライブラリのみ） |
| `*.Data.Entity` | `*.Application` |
| `*.Data.DataStore` | `*.Application`、外部ライブラリ |
| `*.Domain.Ports` | `*.Application`、`Common.Domain.Ports` |
| `*.Domain.Repository` | `*.Data.DataStore`、`*.Data.Entity`、`*.Application` |
| `*.Domain.UseCase` | `*.Domain.Repository`、`*.Domain.Ports`、`*.Data.Entity`、`*.Application` |
| `*.Presentation.State` | `*.Domain.Ports`、`*.Application` |
| `*.Presentation.Presenter` | `*.Domain.UseCase`、`*.Domain.Ports`、`*.Presentation.View`、`*.Application` |
| `*.Presentation.Facade` | `*.Domain.UseCase`、`*.Presentation.View`、`*.Application` |
| `*.Presentation.View` | `*.Application`、外部UIライブラリ |
| `*.Installer` | 全アセンブリ（DI登録のため） |

### 2-4. 外部ライブラリの利用ルール

| ライブラリ | 利用可能レイヤー |
|---|---|
| **VContainer** | `Installer` のみ（DI登録） |
| **UniRx / R3** | `Domain.*`・`Presentation.*` |
| **UniTask** | `Domain.*`・`Presentation.*`・`Utility` |
| **MasterMemory** | `Data.DataStore` のみ |
| **MessagePack** | `Data.DataStore` のみ（`Generated/` 配下） |
| **PlayFab SDK** | `Data.DataStore` のみ |
| **UnityEngine.UI / TMPro** | `Presentation.View` のみ |

## 3. ファイル命名規則

| 種別 | 命名パターン | 例 |
|---|---|---|
| Entity | `{概念}Entity.cs` | `SampleEntity.cs` |
| DataStore（テーブル） | `{概念}Table.cs` / `{概念}Data.cs` | `SampleTable.cs` |
| Repository | `{概念}Repository.cs` | `SampleRepository.cs` |
| UseCase | `{概念}UseCase.cs` | `SampleUseCase.cs` |
| Ports | `{概念}Ports.cs` | `SamplePorts.cs` |
| Presenter | `{概念}Presenter.cs` | `SamplePresenter.cs` |
| Facade | `{概念}Facade.cs` | `SampleFacade.cs` |
| State | `{概念}State.cs` | `SampleState.cs` |
| View | `{概念}View.cs` | `SampleView.cs` |
| Installer | `{Context}Installer.cs` | `GameInstaller.cs` |
| 定数・Enum・VO | `Enum.cs` / `Const.cs` / `ValueObject.cs` | （各Applicationフォルダ内に1ファイル） |
| 自動生成コード | `Generated/` フォルダ配下 | 手動編集禁止 |
