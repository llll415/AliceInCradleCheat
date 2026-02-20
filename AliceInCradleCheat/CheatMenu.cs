using System;
using System.Collections.Generic;
using UnityEngine;
using MelonLoader;

namespace AliceInCradleCheat
{
    public static class CheatMenu
    {
        private static bool isVisible = false;
        private static Rect windowRect = new Rect(50, 50, 480, 600);
        private static Vector2 scrollPos = Vector2.zero;
        private static Dictionary<string, bool> sectionFolds = new();
        private static GUIStyle windowStyle;
        private static GUIStyle sectionStyle;
        private static GUIStyle labelStyle;
        private static GUIStyle buttonStyle;
        private static GUIStyle toggleStyle;
        private static GUIStyle sliderStyle;
        private static GUIStyle sliderThumbStyle;
        private static GUIStyle textFieldStyle;
        private static GUIStyle descStyle;
        private static GUIStyle headerStyle;
        private static GUIStyle foldStyle;
        private static Texture2D bgTex;
        private static Texture2D sectionBgTex;
        private static Texture2D btnTex;
        private static Texture2D btnHoverTex;
        private static Texture2D fieldBgTex;
        private static bool stylesInitialized = false;

        public static bool IsVisible => isVisible;

        public static void Toggle()
        {
            isVisible = !isVisible;
        }

        private static Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;
            Texture2D tex = new Texture2D(width, height);
            tex.SetPixels(pix);
            tex.Apply();
            return tex;
        }

        private static void InitStyles()
        {
            if (stylesInitialized) return;

            bgTex = MakeTex(2, 2, new Color(0.08f, 0.08f, 0.12f, 0.95f));
            sectionBgTex = MakeTex(2, 2, new Color(0.14f, 0.14f, 0.20f, 0.9f));
            btnTex = MakeTex(2, 2, new Color(0.25f, 0.25f, 0.35f, 1f));
            btnHoverTex = MakeTex(2, 2, new Color(0.35f, 0.35f, 0.50f, 1f));
            fieldBgTex = MakeTex(2, 2, new Color(0.12f, 0.12f, 0.18f, 1f));

            windowStyle = new GUIStyle(GUI.skin.window);
            windowStyle.normal.background = bgTex;
            windowStyle.onNormal.background = bgTex;
            windowStyle.normal.textColor = new Color(0.9f, 0.85f, 1f);
            windowStyle.fontSize = 16;
            windowStyle.fontStyle = FontStyle.Bold;
            windowStyle.padding = new RectOffset(10, 10, 25, 10);

            sectionStyle = new GUIStyle(GUI.skin.box);
            sectionStyle.normal.background = sectionBgTex;
            sectionStyle.padding = new RectOffset(8, 8, 6, 6);
            sectionStyle.margin = new RectOffset(0, 0, 4, 4);

            headerStyle = new GUIStyle(GUI.skin.label);
            headerStyle.fontSize = 14;
            headerStyle.fontStyle = FontStyle.Bold;
            headerStyle.normal.textColor = new Color(0.7f, 0.8f, 1f);

            foldStyle = new GUIStyle(GUI.skin.button);
            foldStyle.fontSize = 13;
            foldStyle.fontStyle = FontStyle.Bold;
            foldStyle.alignment = TextAnchor.MiddleLeft;
            foldStyle.normal.textColor = new Color(0.8f, 0.85f, 1f);
            foldStyle.normal.background = sectionBgTex;
            foldStyle.hover.background = btnHoverTex;
            foldStyle.hover.textColor = Color.white;
            foldStyle.padding = new RectOffset(10, 10, 6, 6);
            foldStyle.margin = new RectOffset(0, 0, 2, 0);

            labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.normal.textColor = new Color(0.85f, 0.85f, 0.9f);
            labelStyle.fontSize = 13;

            descStyle = new GUIStyle(GUI.skin.label);
            descStyle.normal.textColor = new Color(0.55f, 0.55f, 0.65f);
            descStyle.fontSize = 11;
            descStyle.wordWrap = true;

            buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.normal.background = btnTex;
            buttonStyle.hover.background = btnHoverTex;
            buttonStyle.normal.textColor = new Color(0.95f, 0.85f, 0.5f);
            buttonStyle.hover.textColor = Color.white;
            buttonStyle.fontSize = 13;
            buttonStyle.fontStyle = FontStyle.Bold;
            buttonStyle.padding = new RectOffset(12, 12, 5, 5);

            toggleStyle = new GUIStyle(GUI.skin.toggle);
            toggleStyle.normal.textColor = new Color(0.85f, 0.85f, 0.9f);
            toggleStyle.onNormal.textColor = new Color(0.5f, 1f, 0.5f);
            toggleStyle.fontSize = 13;

            sliderStyle = new GUIStyle(GUI.skin.horizontalSlider);
            sliderThumbStyle = new GUIStyle(GUI.skin.horizontalSliderThumb);

            textFieldStyle = new GUIStyle(GUI.skin.textField);
            textFieldStyle.normal.background = fieldBgTex;
            textFieldStyle.normal.textColor = new Color(0.9f, 0.9f, 0.95f);
            textFieldStyle.fontSize = 13;
            textFieldStyle.padding = new RectOffset(6, 6, 4, 4);

            stylesInitialized = true;
        }

        public static void Draw()
        {
            if (!isVisible) return;
            InitStyles();
            windowRect = GUILayout.Window(98765, windowRect, DrawWindow, "AliceInCradle Cheat Menu", windowStyle,
                GUILayout.MinWidth(400), GUILayout.MinHeight(300));
        }

        private static void DrawWindow(int id)
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            // Iterate through sections in order defined by LocNames
            foreach (LocConfigSection loc_section in LocNames.config_loc_array)
            {
                string section = loc_section.name;
                if (section == "" || !BasePatchClass.menuEntries.ContainsKey(section))
                    continue;

                List<CheatMenuEntry> entries = BasePatchClass.menuEntries[section];
                if (entries.Count == 0) continue;

                // Section fold toggle
                if (!sectionFolds.ContainsKey(section))
                    sectionFolds[section] = true;

                string sectionDisplayName = loc_section.Loc_name();
                string foldArrow = sectionFolds[section] ? "▼" : "►";
                if (GUILayout.Button($"{foldArrow}  {sectionDisplayName}", foldStyle))
                {
                    sectionFolds[section] = !sectionFolds[section];
                }

                if (!sectionFolds[section]) continue;

                GUILayout.BeginVertical(sectionStyle);

                foreach (CheatMenuEntry entry in entries)
                {
                    DrawEntry(entry);
                }

                GUILayout.EndVertical();
            }

            GUILayout.EndScrollView();
            GUI.DragWindow(new Rect(0, 0, 10000, 30));
        }

        private static void DrawEntry(CheatMenuEntry entry)
        {
            string displayName = LocNames.GetEntryLocName(entry.Section, entry.Key);
            string desc = LocNames.GetLocDesc(entry.Section, entry.Key);

            if (entry.IsButton && entry.EntryType == typeof(bool))
            {
                // Button-style entry (TimedFlag triggers)
                GUILayout.BeginHorizontal();
                var boolEntry = entry.EntryBase as MelonPreferences_Entry<bool>;
                if (GUILayout.Button(displayName, buttonStyle, GUILayout.Height(28)))
                {
                    boolEntry.Value = true;
                }
                GUILayout.EndHorizontal();
                if (!string.IsNullOrEmpty(desc))
                {
                    GUILayout.Label(desc, descStyle);
                }
            }
            else if (entry.EntryType == typeof(bool))
            {
                var boolEntry = entry.EntryBase as MelonPreferences_Entry<bool>;
                boolEntry.Value = GUILayout.Toggle(boolEntry.Value, "  " + displayName, toggleStyle);
                if (!string.IsNullOrEmpty(desc))
                {
                    GUILayout.Label("    " + desc, descStyle);
                }
            }
            else if (entry.EntryType == typeof(int))
            {
                var intEntry = entry.EntryBase as MelonPreferences_Entry<int>;
                if (entry.MinInt != entry.MaxInt && entry.MaxInt > entry.MinInt)
                {
                    // Slider with range
                    GUILayout.BeginHorizontal();
                    GUILayout.Label($"{displayName}: ", labelStyle, GUILayout.Width(180));
                    float val = GUILayout.HorizontalSlider(intEntry.Value, entry.MinInt, entry.MaxInt,
                        sliderStyle, sliderThumbStyle, GUILayout.MinWidth(150));
                    intEntry.Value = Mathf.RoundToInt(val);
                    GUILayout.Label(intEntry.Value.ToString(), labelStyle, GUILayout.Width(50));
                    GUILayout.EndHorizontal();
                }
                else
                {
                    // Text field for int
                    GUILayout.BeginHorizontal();
                    GUILayout.Label($"{displayName}: ", labelStyle, GUILayout.Width(180));
                    string txt = GUILayout.TextField(intEntry.Value.ToString(), textFieldStyle, GUILayout.Width(80));
                    if (int.TryParse(txt, out int parsed))
                    {
                        intEntry.Value = parsed;
                    }
                    GUILayout.EndHorizontal();
                }
                if (!string.IsNullOrEmpty(desc))
                {
                    GUILayout.Label("    " + desc, descStyle);
                }
            }
            else if (entry.EntryType == typeof(string))
            {
                var strEntry = entry.EntryBase as MelonPreferences_Entry<string>;
                GUILayout.BeginHorizontal();
                GUILayout.Label($"{displayName}: ", labelStyle, GUILayout.Width(180));
                strEntry.Value = GUILayout.TextField(strEntry.Value ?? "", textFieldStyle, GUILayout.MinWidth(150));
                GUILayout.EndHorizontal();
                if (!string.IsNullOrEmpty(desc))
                {
                    GUILayout.Label("    " + desc, descStyle);
                }
            }

            GUILayout.Space(2);
        }

        // Called when language changes to reset fold display names
        public static void RefreshLocalization()
        {
            // Styles will use LocNames dynamically, no action needed
            // But we can force re-read of display names if needed in future
        }
    }
}
