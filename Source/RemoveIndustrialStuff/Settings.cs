using UnityEngine;
using Verse;

namespace RemoveIndustrialStuff;

public class Settings : ModSettings
{
    public bool LimitFactions = true;

    public bool LimitItems = true;

    public bool LimitPawns = true;

    public bool LimitResearch = true;

    public bool LogRemovals;

    public void DoWindowContents(Rect canvas)
    {
        const float gap = 8f;
        var listingStandard = new Listing_Standard { ColumnWidth = canvas.width };
        listingStandard.Begin(canvas);
        listingStandard.Gap(gap);
        listingStandard.CheckboxLabeled("RIS.LimitItems".Translate(), ref LimitItems,
            "RIS.LimitItems.Desc".Translate());
        listingStandard.CheckboxLabeled("RIS.LimitResearch".Translate(), ref LimitResearch,
            "RIS.LimitResearch.Desc".Translate());
        listingStandard.CheckboxLabeled("RIS.LimitFactions".Translate(), ref LimitFactions,
            "RIS.LimitFactions.Desc".Translate());
        listingStandard.CheckboxLabeled("RIS.LimitPawnKinds".Translate(), ref LimitPawns,
            "RIS.LimitPawnKinds.Desc".Translate());
        listingStandard.Gap(gap);
        listingStandard.CheckboxLabeled("RIS.LogRemovals".Translate(), ref LogRemovals,
            "RIS.LogRemovals.Desc".Translate());
        listingStandard.Gap(gap);
        listingStandard.Label("RIS.RestartInfo".Translate());
        if (ModStuff.currentVersion != null)
        {
            listingStandard.Gap();
            GUI.contentColor = Color.gray;
            listingStandard.Label("RIS.CurrentModVersion".Translate(ModStuff.currentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref LimitItems, "LimitItems", true);
        Scribe_Values.Look(ref LimitResearch, "LimitResearch", true);
        Scribe_Values.Look(ref LimitFactions, "LimitFactions", true);
        Scribe_Values.Look(ref LimitPawns, "LimitPawns", true);
        Scribe_Values.Look(ref LogRemovals, "LogRemovals");
    }
}