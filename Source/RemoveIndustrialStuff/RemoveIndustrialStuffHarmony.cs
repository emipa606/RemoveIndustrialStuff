using System;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace RemoveIndustrialStuff
{
    [StaticConstructorOnStartup]
    public static class RemoveIndustrialStuffHarmony
    {
        static RemoveIndustrialStuffHarmony()
        {
            var harmony = new Harmony("Mlie.RemoveIndustrialStuff");
            harmony.Patch(AccessTools.Method(typeof(ThingSetMaker), "Generate", new[] {typeof(ThingSetMakerParams)}),
                new HarmonyMethod(typeof(RemoveIndustrialStuffHarmony), nameof(ItemCollectionGeneratorGeneratePrefix)));

            // Log.Message("AddToTradeables");
            harmony.Patch(AccessTools.Method(typeof(TradeDeal), "AddToTradeables"),
                new HarmonyMethod(typeof(RemoveIndustrialStuffHarmony), nameof(PostCacheTradeables)));

            // Log.Message("CanGenerate");
            harmony.Patch(AccessTools.Method(typeof(ThingSetMakerUtility), nameof(ThingSetMakerUtility.CanGenerate)),
                null, new HarmonyMethod(typeof(RemoveIndustrialStuffHarmony), nameof(ThingSetCleaner)));
            harmony.Patch(AccessTools.Method(typeof(FactionManager), "FirstFactionOfDef", new[] {typeof(FactionDef)}),
                new HarmonyMethod(typeof(RemoveIndustrialStuffHarmony), nameof(FactionManagerFirstFactionOfDefPrefix)));

            harmony.Patch(
                AccessTools.Method(typeof(BackCompatibility), "FactionManagerPostLoadInit", Array.Empty<Type>()),
                new HarmonyMethod(typeof(RemoveIndustrialStuffHarmony),
                    nameof(BackCompatibilityFactionManagerPostLoadInitPrefix)));
        }

        public static bool BackCompatibilityFactionManagerPostLoadInitPrefix()
        {
            return !ModStuff.Settings.LimitFactions;
        }

        public static bool FactionManagerFirstFactionOfDefPrefix(ref FactionDef facDef)
        {
            return !ModStuff.Settings.LimitFactions || facDef == null ||
                   facDef.techLevel <= RemoveIndustrialStuff.MAX_TECHLEVEL;
        }

        public static void ItemCollectionGeneratorGeneratePrefix(ref ThingSetMakerParams parms)
        {
            if (!parms.techLevel.HasValue || parms.techLevel > RemoveIndustrialStuff.MAX_TECHLEVEL)
            {
                parms.techLevel = RemoveIndustrialStuff.MAX_TECHLEVEL;
            }
        }

        public static bool PostCacheTradeables(Thing t)
        {
            return !RemoveIndustrialStuff.things.Contains(t.def);
        }

        public static void ThingSetCleaner(ThingDef thingDef, ref bool __result)
        {
            __result &= !RemoveIndustrialStuff.things.Contains(thingDef);
        }
    }
}