using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace RemoveIndustrialStuff;

[HarmonyPatch(typeof(TradeDeal), "AddToTradeables")]
public static class TradeDeal_AddToTradeables
{
    public static bool Prefix(Thing t)
    {
        return !RemoveIndustrialStuff.Things.Contains(t.def);
    }
}