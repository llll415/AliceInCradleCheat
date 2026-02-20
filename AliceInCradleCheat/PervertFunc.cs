using System;
using MelonLoader;
using HarmonyLib;
using nel;
using XX;

namespace AliceInCradleCheat
{
    // ##############################
    // Pervert mode
    // ##############################
    public class PervertFuncs
    {
        public PervertFuncs ()
        {
            _ = new EPDamageMultiplier();
            _ = new EnableMultipleOrgasmForAll();
            _ = new EasierOrgasm();
            _ = new AdditionalPleasure();
            //_ = new EroBow(); // v0.29: disabled
        }
    }
    public class EPDamageMultiplier : BasePatchClass
    {
        private static MelonPreferences_Entry<int> ep_dm_def;
        public EPDamageMultiplier()
        {
            ep_dm_def = TrackBindConfig("PervertFunctions", "EPDamageMultiplier", 1, 1, 100);
            TryPatch(GetType());
        }
        [HarmonyPostfix, HarmonyPatch(typeof(EpManager), "calcNormalEpErection")]
        private static void PatchContent(ref float __result)
        {
            __result *= ep_dm_def.Value;
        }
    }
    public class EnableMultipleOrgasmForAll : BasePatchClass
    {
        private static MelonPreferences_Entry<bool> switch_def;
        public EnableMultipleOrgasmForAll()
        {
            switch_def = TrackBindConfig("PervertFunctions", "EnableMultipleOrgasmForAll", false);
            TryPatch(GetType());
        }
        [HarmonyPrefix, HarmonyPatch(typeof(EpManager), "checkMultipleOrgasm")]
        private static bool PatchContent(ref EpManager __instance, ref bool __result, ref EpAtk Atk)
        {
            if (switch_def.Value)
            {
                float set_mo = 0.4f;
                int cur_mo = Traverse.Create(__instance).Field("multiple_orgasm").GetValue<int>();
                if (Atk.situation_key != "masturbate" && Atk.multiple_orgasm < set_mo)
                {
                    __result = X.XORSP() < set_mo * X.Pow(1f - X.ZLINE(cur_mo - 1, 9f) * 0.5f 
                        - X.ZLINE(cur_mo - 10, 40f) * 0.48f, 2);
                }
                return false;
            }
            return true;
        }
    }
    public class EasierOrgasm : BasePatchClass
    {
        private static MelonPreferences_Entry<bool> switch_def;
        public EasierOrgasm()
        {
            switch_def = TrackBindConfig("PervertFunctions", "EasierOrgasmWithHighEP", false);
            TryPatch(GetType());
        }
        [HarmonyPrefix, HarmonyPatch(typeof(EpManager), "getLeadToOrgasmRatio")]
        private static bool PatchContent(ref float __result)
        {
            if (switch_def.Value)
            {
                __result = 1;
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    public class AdditionalPleasure
    {
        internal static MelonPreferences_Entry<bool> s_def;
        internal static MelonPreferences_Entry<bool> m_def;
        public AdditionalPleasure()
        {
            s_def = BasePatchClass.BindConfig("PervertFunctions", "Sadism", false);
            m_def = BasePatchClass.BindConfig("PervertFunctions", "Masochism", false);
            _ = new SadismPatch();
            _ = new MasochismPatch();
            _ = new PleasureRecordPatch();
        }
    }
    public class SadismPatch : BasePatchClass
    {
        public SadismPatch()
        {
            TryPatch(GetType());
        }
        [HarmonyPrefix, HarmonyPatch(typeof(NelEnemy), "applyDamage", new Type[] { typeof(NelAttackInfo), typeof(bool) })]
        private static bool SadismMode(ref NelAttackInfo Atk)
        {
            PRNoel noel = MainReference.GetNoel();
            if (AdditionalPleasure.s_def != null && AdditionalPleasure.s_def.Value && Atk != null && Atk.AttackFrom is PR)
            {
                int value = (int)(Atk.hpdmg0 * 1.5f);
                value = value > 0 ? value : 1;
                noel.EpCon.applyEpDamage(new EpAtk(value, "sadism"), noel, EPCATEG_BITS.OTHER);
            }
            return true;
        }
    }
    public class MasochismPatch : BasePatchClass
    {
        public MasochismPatch()
        {
            TryPatch(GetType());
        }
        [HarmonyPrefix, HarmonyPatch(typeof(M2PrADmg), "applyHpDamageSimple")]
        private static bool MasochismMode(ref NelAttackInfoBase Atk)
        {
            if (Atk == null)
            {
                return true;
            }
            if (AdditionalPleasure.m_def != null && AdditionalPleasure.m_def.Value)
            {
                if (Atk.EpDmg == null)
                {
                    int value = (int)(Atk.hpdmg0 * 1.5f);
                    value = value > 0 ? value : 1;
                    Atk.EpDmg = new EpAtk(value, "masochism");
                }
            }
            else if (Atk.EpDmg != null && Atk.EpDmg.situation_key == "masochism")
            {
                Atk.EpDmg = null;
            }
            return true;
        }
    }
    public class PleasureRecordPatch : BasePatchClass
    {
        public PleasureRecordPatch()
        {
            TryPatch(GetType());
        }
        [HarmonyPrefix, HarmonyPatch(typeof(EpManager), "addMasturbateCountImmediate")]
        private static bool RemoveRecord(ref EpManager __instance, ref EPCATEG __result, ref string situation_key, ref int multiple_orgasm)
        {
            if (situation_key == "sadism" || situation_key == "masochism")
            {
                __result = EPCATEG.OTHER;
                Traverse lmc = Traverse.Create(__instance).Field("last_ex_multi_count_temp");
                if (lmc.GetValue<int>() < multiple_orgasm)
                {
                    lmc.SetValue(multiple_orgasm);
                }
                return false;
            }
            return true;
        }
    }
    // v0.29: NelNGolemToyBow.decideAttr was removed. This patch is disabled.
    /*
    public class EroBow : BasePatchClass
    {
        private static MelonPreferences_Entry<bool> switch_def;
        public EroBow()
        {
            switch_def = TrackBindConfig("PervertFunctions", "EroBow", false);
            TryPatch(GetType());
        }
        [HarmonyPrefix, HarmonyPatch(typeof(NelNGolemToyBow), "decideAttr")]
        private static bool PatchContent(ref NelNGolemToyBow __instance)
        {
            if (switch_def.Value)
            {
                MagicItem Mg = Traverse.Create(__instance).Field("Mg").GetValue<MagicItem>();
                if (Mg != null)
                {
                    Mg.Atk0 = Traverse.Create(__instance).Field("AtkAcme").GetValue<NelAttackInfo>();
                    Mg.Ray.HitLock(13f, null);
                    Traverse.Create(__instance).Field("Cattr").SetValue(new UnityEngine.Color32(243, 91, 169, 255));
                    Traverse.Create(__instance).Field("Cattr2").SetValue(new UnityEngine.Color(179, 10, 145, 170));
                }
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    */
}
