using System;
using System.Collections.Generic;
using UnityEngine;
using MelonLoader;
using HarmonyLib;
using nel;
using m2d;

namespace AliceInCradleCheat
{
    public abstract class BasePatchClass
    {
        // Registry of all categories and entries for the CheatMenu
        public static Dictionary<string, MelonPreferences_Category> categories = new();
        public static Dictionary<string, List<CheatMenuEntry>> menuEntries = new();

        public List<string> config_keys = new();

        public MelonPreferences_Entry<T> TrackBindConfig<T>(string section, string key, T val,
            int minVal = 0, int maxVal = 0, bool isButton = false)
        {
            MelonPreferences_Entry<T> entry = BindConfig(section, key, val, minVal, maxVal, isButton);
            config_keys.Add($"{section}.{key}");
            return entry;
        }

        public static MelonPreferences_Entry<T> BindConfig<T>(string section, string key, T val,
            int minVal = 0, int maxVal = 0, bool isButton = false)
        {
            if (!categories.ContainsKey(section))
            {
                string displayName = LocNames.GetSectionLocName(section);
                categories[section] = MelonPreferences.CreateCategory(section, displayName);
                categories[section].SetFilePath("UserData/AliceInCradleCheat.cfg");
                menuEntries[section] = new List<CheatMenuEntry>();
            }

            string entryDisplayName = LocNames.GetEntryLocName(section, key);
            string entryDesc = LocNames.GetLocDesc(section, key);
            MelonPreferences_Entry<T> entry = categories[section].CreateEntry(key, val,
                entryDisplayName, entryDesc);

            // Register for CheatMenu rendering
            menuEntries[section].Add(new CheatMenuEntry
            {
                Key = key,
                Section = section,
                EntryBase = entry,
                EntryType = typeof(T),
                MinInt = minVal,
                MaxInt = maxVal,
                IsButton = isButton,
            });

            return entry;
        }

        public void RemoveConfigs()
        {
            // MelonPreferences doesn't support dynamic removal
            // Entries will simply be ignored if patch fails
        }

        public void TryPatch(Type patch_type)
        {
            try
            {
                var harmony = new HarmonyLib.Harmony($"AliceInCradleCheat.{patch_type.Name}");
                harmony.PatchAll(patch_type);
            }
            catch //(Exception ex)
            {
                Melon<AICCheat>.Logger.Error($"Patch {patch_type} failed!");
                //Melon<AICCheat>.Logger.Msg(ex.ToString());
                RemoveConfigs();
            }
        }
    }

    public class CheatMenuEntry
    {
        public string Key;
        public string Section;
        public MelonPreferences_Entry EntryBase;
        public Type EntryType;
        public int MinInt;
        public int MaxInt;
        public bool IsButton;
    }

    public class TimedFlag
    {
        /*Attach a timer to a boolean flag,
        If the flag is switched to true, the timer will
        start the count down, once timer reached 0,
        the flag will be set to false;
        //*/
        private readonly MelonPreferences_Entry<bool> config_flag;
        private float timer;
        private const float max_time = 0.1f;
        public TimedFlag(MelonPreferences_Entry<bool> config_flag)
        {
            this.config_flag = config_flag;
            timer = 0;
        }
        public bool Check()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (timer < 0)
                {
                    config_flag.Value = false;
                    timer = 0;
                }
                else
                {
                    config_flag.Value = true;
                }
                return false;
            }
            else
            {
                if (config_flag.Value)
                {
                    timer = max_time;
                }
                return config_flag.Value;
            }
        }
        public void SetFlag(bool val)
        {
            if (val == false)
            {
                timer = 0;
            }
            config_flag.Value = val;
        }
    }

    internal class MainReference : BasePatchClass
    {
        private static NelM2DBase m2d;
        private static PRNoel noel;
        public MainReference()
        {
            TryPatch(typeof(MainReference));
        }

        [HarmonyPostfix, HarmonyPatch(typeof(PRNoel), "newGame")]
        private static void PatchContent()
        {
            FetchNoel();
        }
        internal static void FetchNoel()
        {
            m2d = M2DBase.Instance as NelM2DBase;
            noel = m2d.getPrNoel();
        }
        internal static NelM2DBase GetM2D()
        {
            if (m2d == null)
            {
                FetchNoel();
            }
            return m2d;
        }
        internal static PRNoel GetNoel()
        {
            if (noel == null)
            {
                FetchNoel();
            }
            return noel;
        }
    }

    public class BaseLocClass
    {
        public int order = 0;
        public string name;
        public readonly string[] loc_name_array;
        public BaseLocClass(string name, string[] loc_name_array)
        {
            this.name = name;
            this.loc_name_array = loc_name_array;
        }
        public static int GetLangIndex(string lang = "")
        {
            int i;
            if (lang == "" || lang == null)
            {
                i = Array.IndexOf(LocNames.lang_array, LocNames.menu_lang);
            }
            else
            {
                i = Array.IndexOf(LocNames.lang_array, lang);
            }
            return i == -1 ? 0 : i;
        }
        public string Loc_name(string lang = "")
        {
            int i = GetLangIndex(lang);
            return loc_name_array[i] == "" ? name : loc_name_array[i];
        }
    }

    public class LocConfigSection : BaseLocClass
    {
        // Create localization for config sections
        public Dictionary<string, LocConfigEntry> entry_dict = new();
        public LocConfigSection(string name, string[] loc_name_array, string[] entry_loc_array) :
            base(name, loc_name_array)
        {
            int n = LocNames.lang_array.Length + 1;
            int order = 0;
            for (int i = 0; i < entry_loc_array.Length; i += n)
            {
                string entry_key = entry_loc_array[i];
                if (entry_key.StartsWith("desc_") && name != "")
                {
                    continue;
                }
                List<string> loc_list = new();
                for (int j = 1; j < n; j++)
                {
                    loc_list.Add(entry_loc_array[i + j]);
                }
                entry_dict[entry_key] = new(entry_key, loc_list.ToArray(), order);
                order--;
            }
            for (int i = 0; i < entry_loc_array.Length; i += n)
            {
                string entry_key = entry_loc_array[i];
                if (!entry_key.StartsWith("desc_") || name == "")
                {
                    continue;
                }
                entry_key = entry_key.Remove(0, "desc_".Length);
                List<string> loc_list = new();
                for (int j = 1; j < n; j++)
                {
                    loc_list.Add(entry_loc_array[i + j]);
                }
                try
                {
                    entry_dict[entry_key].SetLocDesc(loc_list.ToArray());
                }
                catch
                {
                    Melon<AICCheat>.Logger.Error($"key not exist for 'desc_{entry_key}'");
                }
            }
        }
    }

    public class LocConfigEntry : BaseLocClass
    {
        // Create localization for config entries
        private string[] loc_desc_array;
        public LocConfigEntry(string name, string[] loc_name_array, int order) :
            base(name, loc_name_array)
        {
            this.order = order;
        }
        public string Loc_desc(string lang = "")
        {
            if (loc_desc_array == null) return "";
            int i = GetLangIndex(lang);
            return loc_desc_array[i];
        }
        public void SetLocDesc(string[] loc_desc_array)
        {
            this.loc_desc_array = loc_desc_array;
        }
    }
}
