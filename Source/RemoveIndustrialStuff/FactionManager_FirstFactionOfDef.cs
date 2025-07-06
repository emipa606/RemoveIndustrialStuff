﻿using HarmonyLib;
using RimWorld;

namespace RemoveIndustrialStuff;

[HarmonyPatch(typeof(FactionManager), nameof(FactionManager.FirstFactionOfDef), typeof(FactionDef))]
public static class FactionManager_FirstFactionOfDef
{
    public static bool Prefix(ref FactionDef facDef)
    {
        return !ModStuff.Settings.LimitFactions || facDef is not { techLevel: > RemoveIndustrialStuff.MaxTechlevel };
    }
}