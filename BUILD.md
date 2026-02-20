# AliceInCradleCheat

A MelonLoader based cheat plugin for AliceInCradle.

## Building

Note: path to the project folder should not contain any non-ASCII characters.

### Obtaining dependencies

1. Install MelonLoader v0.7.x to the game directory.
2. Copy `MelonLoader.dll` from the game's `MelonLoader/net35/` folder to the `lib` folder in this project. If `0Harmony.dll` exists separately, copy it too.
3. Copy the following DLLs from the game's `AliceInCradle_Data\Managed` folder to the `lib` folder:
   - `Assembly-CSharp.dll`
   - `better.dll`
   - `netstandard.dll`
   - `unsafeAssem.dll`
   - `UnityEngine.dll`
   - `UnityEngine.CoreModule.dll`
   - `UnityEngine.IMGUIModule.dll`
   - `UnityEngine.InputLegacyModule.dll`
   - `UnityEngine.TextRenderingModule.dll`
   - `UnityEngine.UI.dll`

### Building Binaries

1. Open `AliceInCradleCheat.sln` and let Visual Studio load everything.
2. In the toolbar, select the desired Configuration (`Debug` or `Release`).
3. From the Build menu, select `Build Solution` (or use the keybind; default is `F7`).
4. Copy the `AliceInCradleCheat.dll` from within the `bin` folder to the `Mods` folder in the game directory.

### Notes

- Target framework: .NET Framework 4.6, platform x64.
- All `lib` references have `Private = False` so they are not copied to the output.
- When game versions update, some Harmony patches may break if the target method signatures change. Check MelonLoader logs for "Patch ... failed!" messages.
