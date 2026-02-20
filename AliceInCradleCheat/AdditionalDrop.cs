using System.Collections.Generic;
using MelonLoader;
using HarmonyLib;
using nel;
using XX;

namespace AliceInCradleCheat
{
    // ##############################
    // Additional items when drop from bag
    // ##############################
    public class AdditionalDrop : BasePatchClass
    {
        public static string section = "GenerateItemWhenDrop";
        private static Dictionary<string, string> NameToKey;
        private static Dictionary<string, string> KeyToName;
        private static MelonPreferences_Entry<string> item_name_def;
        private static MelonPreferences_Entry<int> count_def;
        private static MelonPreferences_Entry<string> grade_def;
        public AdditionalDrop()
        {
            item_name_def = TrackBindConfig(section, "ItemName", "");
            count_def = TrackBindConfig(section, "Count", 0, 0, 20);
            grade_def = TrackBindConfig(section, "Grade",
                LocNames.GetEntryLocName("", "option_SameGrade"));
            TryPatch(GetType());
        }

        public static void ResetItemNames()
        {
            // Re-initialize the name<->key mappings when language changes
            if (NelItem.OData != null)
            {
                LoadItemNameMappings();
            }
        }

        public static void LoadItemNameMappings()
        {
            NameToKey = new();
            KeyToName = new();
            if (NelItem.OData == null)
            {
                return;
            }
            foreach (NelItem itm in NelItem.OData.Values)
            {
                if (itm == null || itm.key == null)
                {
                    continue;
                }
                string key = itm.key;
                if ((itm.is_precious && key != "enhancer_slot" && key != "oc_slot") ||
                    itm.is_cache_item || itm.is_enhancer || itm.is_reelmbox)
                {
                    continue;
                }
                TX tx;
                try
                {
                    tx = TX.getTX("_NelItem_name_" + key, true, true, null);
                }
                catch
                {
                    tx = null;
                }
                string localized_name = tx == null ? key : tx.text;
                NameToKey[localized_name] = key;
                KeyToName[key] = localized_name;
            }
        }

        [HarmonyPrefix, HarmonyPatch(typeof(UiItemManageBox), "fnClickItemCmd")]
        private static bool PatchContent(ref aBtn B, ref UiItemManageBox __instance)
        {
            int count = count_def.Value;
            if ((B.title != "drop" && B.title != "discard_row" && B.title != "discard_water") || count == 0) { return true; }
            string item_name = item_name_def.Value;
            NelItem Itm;

            // Initialize name mappings if not done
            if (NameToKey == null || KeyToName == null)
            {
                LoadItemNameMappings();
            }

            if (NelItem.OData.ContainsKey(item_name))
            {
                // Direct key input
                Itm = NelItem.OData[item_name];
            }
            else if (NameToKey != null && NameToKey.ContainsKey(item_name) &&
                NelItem.OData.ContainsKey(NameToKey[item_name]))
            {
                // Localized name input
                Itm = NelItem.OData[NameToKey[item_name]];
            }
            else
            {
                // Fallback to current selected item
                Itm = __instance.UsingTarget;
            }
            if ((Itm.is_precious && Itm.key != "enhancer_slot" && Itm.key != "oc_slot") || Itm.is_cache_item || Itm.is_enhancer)
            {
                Itm = __instance.UsingTarget;
            }
            int grade;
            int new_grade = 5;
            bool retain_grade_flag;
            string grade_set = grade_def.Value;
            if (grade_set == LocNames.GetEntryLocName("", "option_SameGrade"))
            {
                retain_grade_flag = true;
            }
            else if (int.TryParse(grade_set, out new_grade))
            {
                retain_grade_flag = false;
            }
            else
            {
                retain_grade_flag = true;
            }
            grade = Itm.individual_grade ? 0 : retain_grade_flag ? __instance.get_grade_cursor() : new_grade - 1;
            if (count > 0)
            {
                NelItemManager.NelItemDrop nelItemDrop = __instance.IMNG.dropManual(Itm, count, grade,
                    __instance.Pr.x, __instance.Pr.y, X.NIXP(-0.003f, -0.07f) * (float)CAim._XD(__instance.Pr.aim, 1),
                    X.NIXP(-0.01f, -0.04f), null, false);
                nelItemDrop.discarded = true;
                nelItemDrop.finePrActive(__instance.Pr); // Possibly not needed
            }
            return true;
        }
    }
}
