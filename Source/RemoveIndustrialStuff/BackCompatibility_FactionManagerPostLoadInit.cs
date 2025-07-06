using HarmonyLib;
using Verse;

namespace RemoveIndustrialStuff;

[HarmonyPatch(typeof(BackCompatibility), nameof(BackCompatibility.FactionManagerPostLoadInit), [])]
public static class BackCompatibility_FactionManagerPostLoadInit
{
    public static bool Prefix()
    {
        return !ModStuff.Settings.LimitFactions;
    }
}