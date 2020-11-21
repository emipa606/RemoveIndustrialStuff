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
            var harmony = new Harmony(id: "Mlie.RemoveIndustrialStuff");
            harmony.Patch(original: AccessTools.Method(type: typeof(ThingSetMaker), name: "Generate", parameters: new[] { typeof(ThingSetMakerParams) }), prefix: new HarmonyMethod(typeof(RemoveIndustrialStuffHarmony), nameof(ItemCollectionGeneratorGeneratePrefix)), postfix: null);
            //Log.Message("AddToTradeables");
            harmony.Patch(AccessTools.Method(typeof(TradeDeal), "AddToTradeables"), new HarmonyMethod(typeof(RemoveIndustrialStuffHarmony), nameof(PostCacheTradeables)), null);
            //Log.Message("CanGenerate");
            harmony.Patch(AccessTools.Method(typeof(ThingSetMakerUtility), nameof(ThingSetMakerUtility.CanGenerate)), null, new HarmonyMethod(typeof(RemoveIndustrialStuffHarmony), nameof(ThingSetCleaner)));
            
        }

        public static void ThingSetCleaner(ThingDef thingDef, ref bool __result)
        {
            __result &= !RemoveIndustrialStuff.things.Contains(thingDef);
        }

        public static bool PostCacheTradeables(Thing t)
        {
            return !RemoveIndustrialStuff.things.Contains(t.def);
        }

        public static void ItemCollectionGeneratorGeneratePrefix(ref ThingSetMakerParams parms)
        {
            if (!parms.techLevel.HasValue || parms.techLevel > RemoveIndustrialStuff.MAX_TECHLEVEL)
            {
                parms.techLevel = RemoveIndustrialStuff.MAX_TECHLEVEL;
            }
        }

    }
}