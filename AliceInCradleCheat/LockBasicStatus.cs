using MelonLoader;
using HarmonyLib;
using nel;

namespace AliceInCradleCheat
{
    // ##############################
    // Lock basic status
    // ##############################
    public class LockStatus : BasePatchClass
    {
        private static MelonPreferences_Entry<bool> hp_switch_def;
        private static MelonPreferences_Entry<int> hp_def;
        private static MelonPreferences_Entry<bool> mp_switch_def;
        private static MelonPreferences_Entry<int> mp_def;
        private static MelonPreferences_Entry<bool> ep_switch_def;
        private static MelonPreferences_Entry<int> ep_def;
        public LockStatus()
        {
            string section = "BasicStatus";
            hp_switch_def = TrackBindConfig(section, "HPLockSwitch", false);
            hp_def = TrackBindConfig(section, "HP", 100, 0, 100);
            mp_switch_def = TrackBindConfig(section, "MPLockSwitch", false);
            mp_def = TrackBindConfig(section, "MP", 100, 0, 100);
            section = "PervertFunctions";
            ep_switch_def = TrackBindConfig(section, "EPLockSwitch", false);
            ep_def = TrackBindConfig(section, "EP", 0, 0, 1000);
            TryPatch(GetType());
        }
        [HarmonyPostfix, HarmonyPatch(typeof(SceneGame), "runIRD")]
        private static void PatchContent()
        {
            PRNoel noel = MainReference.GetNoel();
            if (noel == null)
            {
                return;
            }
            if (hp_switch_def.Value)
            {
                int max_hp = (int)noel.get_maxhp();
                int set_hp = hp_def.Value * max_hp / 100;
                Traverse.Create(noel).Field("hp").SetValue(set_hp);
                if (noel.UP != null && noel.UP.isActive())
                {
                    UIStatus.Instance.fineHpRatio(true, false);
                }
            }
            if (mp_switch_def.Value)
            {
                int max_mp = (int)noel.get_maxmp();
                int set_mp = mp_def.Value * max_mp / 100;
                max_mp -= noel.EggCon.total;
                set_mp = set_mp < max_mp ? set_mp : max_mp;
                Traverse.Create(noel).Field("mp").SetValue(set_mp);
                if (noel.UP != null && noel.UP.isActive())
                {
                    UIStatus.Instance.fineMpRatio(true, false);
                }
            }
            if (ep_switch_def.Value)
            {
                noel.ep = ep_def.Value;
                noel.EpCon.fineCounter();
            }
        }
    }
}
