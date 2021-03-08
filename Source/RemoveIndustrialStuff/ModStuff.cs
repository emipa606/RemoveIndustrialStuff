using UnityEngine;
using Verse;

namespace RemoveIndustrialStuff
{
    public class ModStuff : Mod
    {
        public static Settings Settings;

        public ModStuff(ModContentPack content) : base(content)
        {
            Settings = GetSettings<Settings>();
        }

        public override string SettingsCategory()
        {
            return "Remove Industrial Stuff";
        }

        public override void DoSettingsWindowContents(Rect canvas)
        {
            Settings.DoWindowContents(canvas);
        }
    }
}