# AliceInCradleCheat

基于 MelonLoader 的 AliceInCradle 作弊插件。

基于 [K3nny567/AliceInCradleCheat](https://github.com/K3nny567/AliceInCradleCheat)（BepInEx 版本，原仓库：[availlizard/AliceInCradleCheat](https://github.com/availlizard/AliceInCradleCheat)）分支修改，迁移至 MelonLoader 0.7.x 并适配 v0.29 游戏版本。

## 安装与使用

注意：游戏目录路径不应包含非 ASCII 字符。

### 安装

1. 安装 [MelonLoader v0.7.x](https://github.com/LavaGang/MelonLoader/releases)：使用自动安装器或手动解压到 `AliceInCradle.exe` 所在的游戏根目录。
2. 安装 `AliceInCradleCheat.dll`：下载并放入游戏根目录下的 `Mods` 文件夹。

### 使用方法

在游戏中按 `BackQuote`（`` ` ``）键打开作弊菜单，再按一次关闭。

设置保存在 `UserData/AliceInCradleCheat.cfg`。

## 功能列表

1. 锁定 HP 和 MP（可分别设置）。
2. 伤害倍率调节。
3. 免疫魔物攻击（拦截 PR 及 M2PrADmg 伤害）。
4. 无限跳跃、解锁仓库使用。
5. 环境伤害免疫（荆棘、酸液/熔岩）。
6. 异常状态免疫：睡眠、混乱、麻痹、着火、冻结、杂念、**石化**（v0.29 新增）。
7. 兴奋度锁定与 EP 伤害无效化。
8. 施虐 / 受虐模式。
9. 关闭部分马赛克。
10. 额外物品掉落。

## 已知问题

以下功能因 v0.29 游戏代码变更（方法被移除/重构）已**禁用**：

- **跳过战败演出**（SkipGameOverPlay）— `UiGO.runGiveup` 已被移除，游戏战败流程重构为 `UiGOContinuer`。
- **人偶弩紫射线**（EroBow）— `NelNGolemToyBow.decideAttr` 已被移除。
- **无限制快速传送**（Unlimited Fast Travel）— 原仓库中标注为已移除，本分支中不包含此功能，不确定是否曾实现。

---

本分支由 Claude Opus 4 协助修改（BepInEx → MelonLoader 迁移 + v0.29 适配）。
