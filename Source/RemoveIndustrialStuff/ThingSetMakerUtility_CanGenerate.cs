﻿using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace RemoveIndustrialStuff;

[HarmonyPatch(typeof(ThingSetMakerUtility), nameof(ThingSetMakerUtility.CanGenerate))]
public static class ThingSetMakerUtility_CanGenerate
{
    public static void Prefix(ThingDef thingDef, ref bool __result)
    {
        __result &= !RemoveIndustrialStuff.Things.Contains(thingDef);
    }
}