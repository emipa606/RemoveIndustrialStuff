using System.Text;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace RemoveIndustrialStuff
{
    [StaticConstructorOnStartup]
    public static class RemoveIndustrialStuff
    {
        public const TechLevel MAX_TECHLEVEL = TechLevel.Medieval;
        private static int removedDefs;
        private static readonly StringBuilder DebugString = new StringBuilder();

        public static IEnumerable<ThingDef> things = new List<ThingDef>();

        static RemoveIndustrialStuff()
        {
            DebugString.AppendLine("RemoveIndustrialStuff - Start Removal Log");
            DebugString.AppendLine("Tech Max Level = " + MAX_TECHLEVEL.ToString());

            removedDefs = 0;
            IEnumerable<ResearchProjectDef> projects = new List<ResearchProjectDef>();
            if (ModStuff.Settings.LimitResearch)
            {
                projects = DefDatabase<ResearchProjectDef>.AllDefs.Where(rpd => rpd.techLevel > MAX_TECHLEVEL);
            }
            var extraDefsToRemove = new List<string>();

            if (ModStuff.Settings.LimitItems)
            {
                things = new HashSet<ThingDef>(DefDatabase<ThingDef>.AllDefs.Where(td =>
                td.techLevel > MAX_TECHLEVEL ||
                extraDefsToRemove.Contains(td.defName) ||
                (td.researchPrerequisites?.Any(rpd => projects.Contains(rpd)) ?? false) || new string[]
                {

                }.Contains(td.defName)));
            }
            DebugString.AppendLine("RecipeDef Removal List");


            foreach (var thing in from thing in things where thing.tradeTags != null select thing)
            {
                var tags = thing.tradeTags.ToArray();
                foreach (var tag in tags)
                {
                    if (tag.StartsWith("CE_AutoEnableCrafting"))
                    {
                        thing.tradeTags.Remove(tag);
                    }
                }
            }

            DebugString.AppendLine("ResearchProjectDef Removal List");
            RemoveStuffFromDatabase(typeof(DefDatabase<ResearchProjectDef>), projects.Cast<Def>());

            DebugString.AppendLine("Scenario Part Removal List");
            FieldInfo getThingInfo =
                typeof(ScenPart_ThingCount).GetField("thingDef", BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (ScenarioDef def in DefDatabase<ScenarioDef>.AllDefs)
            {
                foreach (ScenPart sp in def.scenario.AllParts)
                {
                    if (sp is ScenPart_ThingCount && things.Contains((ThingDef)getThingInfo?.GetValue(sp)))
                    {
                        def.scenario.RemovePart(sp);
                        DebugString.AppendLine("- " + sp.Label + " " + ((ThingDef)getThingInfo?.GetValue(sp)).label +
                                               " from " + def.label);
                    }
                }
            }

            foreach (ThingCategoryDef thingCategoryDef in DefDatabase<ThingCategoryDef>.AllDefs)
            {
                thingCategoryDef.childThingDefs.RemoveAll(things.Contains);
            }

            DebugString.AppendLine("Stock Generator Part Cleanup");
            foreach (TraderKindDef tkd in DefDatabase<TraderKindDef>.AllDefs)
            {
                for (var i = tkd.stockGenerators.Count - 1; i >= 0; i--)
                {
                    StockGenerator stockGenerator = tkd.stockGenerators[i];

                    switch (stockGenerator)
                    {
                        case StockGenerator_SingleDef sd when things.Contains(Traverse.Create(sd).Field("thingDef")
                            .GetValue<ThingDef>()):
                            ThingDef def = Traverse.Create(sd).Field("thingDef")
                                .GetValue<ThingDef>();
                            tkd.stockGenerators.Remove(stockGenerator);
                            DebugString.AppendLine("- " + def.label + " from " + tkd.label +
                                                   "'s StockGenerator_SingleDef");
                            break;
                        case StockGenerator_MultiDef md:
                            Traverse thingListTraverse = Traverse.Create(md).Field("thingDefs");
                            List<ThingDef> thingList = thingListTraverse.GetValue<List<ThingDef>>();
                            var removeList = thingList.FindAll(things.Contains);
                            removeList?.ForEach(x =>
                                DebugString.AppendLine("- " + x.label + " from " + tkd.label +
                                                       "'s StockGenerator_MultiDef"));
                            thingList.RemoveAll(things.Contains);

                            if (thingList.NullOrEmpty())
                            {
                                tkd.stockGenerators.Remove(stockGenerator);
                            }
                            else
                            {
                                thingListTraverse.SetValue(thingList);
                            }

                            break;
                    }
                }
            }

            DebugString.AppendLine("ThingDef Removal List");
            RemoveStuffFromDatabase(typeof(DefDatabase<ThingDef>), things.ToArray());

            DebugString.AppendLine("ThingSetMaker Reset");
            ThingSetMakerUtility.Reset();

            DebugString.AppendLine("Designators Resolved Again");
            MethodInfo resolveDesignatorsAgain = typeof(DesignationCategoryDef).GetMethod("ResolveDesignators",
                BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (DesignationCategoryDef dcd in DefDatabase<DesignationCategoryDef>.AllDefs)
            {
                resolveDesignatorsAgain?.Invoke(dcd, null);
            }

            if (ModStuff.Settings.LimitPawns)
            {
                DebugString.AppendLine("PawnKindDef Removal List");
                RemoveStuffFromDatabase(typeof(DefDatabase<PawnKindDef>),
                DefDatabase<PawnKindDef>.AllDefs
                    .Where(pkd =>
                        (!pkd.defaultFactionType?.isPlayer ?? false) &&
                        (pkd.race.techLevel > MAX_TECHLEVEL))
                    .Cast<Def>());
            }

            if (ModStuff.Settings.LimitFactions)
            {
                DebugString.AppendLine("FactionDef Removal List");
                RemoveStuffFromDatabase(typeof(DefDatabase<FactionDef>),
                    DefDatabase<FactionDef>.AllDefs.Where(fd => !fd.isPlayer && fd.techLevel > MAX_TECHLEVEL).Cast<Def>());
            }
            Log.Message("Removed " + removedDefs + " industrial defs");

            PawnWeaponGenerator.Reset();
            PawnApparelGenerator.Reset();

            Debug.Log(DebugString.ToString());
            DebugString = new StringBuilder();
        }


        private static void RemoveStuffFromDatabase(Type databaseType, [NotNull] IEnumerable<Def> defs)
        {
            IEnumerable<Def> enumerable = defs as Def[] ?? defs.ToArray();
            if (!enumerable.Any())
            {
                return;
            }

            Traverse rm = Traverse.Create(databaseType).Method("Remove", enumerable.First());
            foreach (Def def in enumerable)
            {
                removedDefs++;
                DebugString.AppendLine("- " + def.label);
                rm.GetValue(def);
            }
        }
    }
}