using HarmonyLib;
using RimWorld;

namespace RemoveIndustrialStuff;

[HarmonyPatch(typeof(ThingSetMaker), nameof(ThingSetMaker.Generate), typeof(ThingSetMakerParams))]
public static class ThingSetMaker_Generate
{
    public static void Prefix(ref ThingSetMakerParams parms)
    {
        if (parms.techLevel is null or > RemoveIndustrialStuff.MaxTechlevel)
        {
            parms.techLevel = RemoveIndustrialStuff.MaxTechlevel;
        }
    }
}