# AliceInCradleCheat

[中文说明](README_ZH.md)

A MelonLoader based cheat plugin for AliceInCradle.

Forked from [K3nny567/AliceInCradleCheat](https://github.com/K3nny567/AliceInCradleCheat) (BepInEx version, original repo: [availlizard/AliceInCradleCheat](https://github.com/availlizard/AliceInCradleCheat)). This branch migrates to MelonLoader 0.7.x and adds v0.29 game compatibility.

## Installation and usage

Note: path to the game folder should not contain any non-ASCII characters.

### Installation

1. Install [MelonLoader v0.7.x](https://github.com/LavaGang/MelonLoader/releases): use the automated installer or manually extract files to the game root folder where `AliceInCradle.exe` is located.
2. Install `AliceInCradleCheat.dll`: download and place `AliceInCradleCheat.dll` into the `Mods` folder within the game root directory.

### Usage

Press `BackQuote` (`` ` ``) in game to open the cheat menu, press again to close.

Settings are saved to `UserData/AliceInCradleCheat.cfg`.

## Implemented Functions

1. Lock HP and MP. (Now separated)
2. Damage modifier.
3. Invincible to monsters (blocks PR and M2PrADmg damage).
4. Infinite jump, unlimited storage access.
5. Environment damage immunity (thorns, lava/acid).
6. Debuff immunity: sleep, confuse, paralysis, burned, frozen, jamming, **stone/petrification** (new in v0.29).
7. EP lock and EP damage disable.
8. Sadism / Masochism mode.
9. Disable mosaics.
10. Additional item drops.

## Known Issues

The following features are **disabled** due to game changes in v0.29 (methods removed/refactored):

- **SkipGameOverPlay** — `UiGO.runGiveup` was removed; game over flow refactored to `UiGOContinuer`.
- **EroBow** (golem bow ero-shot) — `NelNGolemToyBow.decideAttr` was removed.
- **Unlimited Fast Travel** — Marked as removed in the original repo. This branch does not include it; unclear if it was ever implemented.

---

This branch was modified with assistance from Claude Opus 4 (BepInEx → MelonLoader migration + v0.29 adaptation).
