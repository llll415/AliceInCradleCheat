using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(AliceInCradleCheat.AICCheat), "AliceInCradleCheat", "0.27.0", "AliceInCradleCheat")]
[assembly: MelonGame]

namespace AliceInCradleCheat
{
    public class AICCheat : MelonMod
    {
        public override void OnInitializeMelon()
        {
            LocNames.InitializeLocNames();

            // Core reference
            _ = new MainReference();

            // Localization hooks
            _ = new MenuLocalization();
            _ = new AddCustomText();

            // Feature modules
            _ = new LockStatus();
            _ = new SuperNoel();
            _ = new SetGameValue();
            _ = new RestrictionLift();
            _ = new NonHModeEnhance();
            _ = new PervertFuncs();
            _ = new SpecialItemEffect();
            _ = new AdditionalDrop();
            _ = new OtherFuncs();

            LoggerInstance.Msg($"AliceInCradleCheat v0.27.0 loaded!");
        }

        public override void OnUpdate()
        {
            // Toggle cheat menu with BackQuote key (same as original)
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                CheatMenu.Toggle();
            }
        }

        public override void OnGUI()
        {
            CheatMenu.Draw();
        }
    }
}
