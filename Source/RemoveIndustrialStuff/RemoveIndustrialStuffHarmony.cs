using System.Reflection;
using HarmonyLib;
using Verse;

namespace RemoveIndustrialStuff;

[StaticConstructorOnStartup]
public static class RemoveIndustrialStuffHarmony
{
    static RemoveIndustrialStuffHarmony()
    {
        new Harmony("Mlie.RemoveIndustrialStuff").PatchAll(Assembly.GetExecutingAssembly());
    }
}