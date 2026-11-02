# Agent Guidelines

## 共通ルール

以下のファイルに定義された共通ルールをすべて遵守してください。
- `.aiassistant/rules/00_global.md`

## Agent 固有の指示

### 操作上の制約

- `ProjectSettings/` 以下のファイルは変更しない
- `Packages/packages-lock.json` は変更しない
- `.unity` シーンファイル・`.meta` ファイルは直接編集しない
- 新規ファイル作成時は、対応する `.meta` ファイルが Unity により自動生成されることを前提とする
- エディタ拡張コードは `Assets/Editor/` に配置し、必ず `#if UNITY_EDITOR` で保護する

### 回答スタイル

- コード修正時は差分が明確になるよう、変更箇所にコメントを添える
- 説明は日本語で記述する
