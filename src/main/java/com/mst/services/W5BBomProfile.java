package com.mst.services;

import java.util.LinkedHashMap;
import java.util.Map;

/**
 * W5B - the three Bill Of Material screens share one engine; this class holds what differs between them.
 *
 * <pre>
 *   774 frmBillOfMaterial          Architecture.WinApp.Mfg   [Mfg] schema  DocumentTypeId 1617  1617_USP_BillOfMaterial_Slip.rpt
 *   845 frmBillOfMaterialCasting   Architecture.WinApp.Mfg   [Mfg] schema  DocumentTypeId 1664  1664_BillOfMaterialCasting_Slip.rpt
 *   598 frmBillOfMaterialConcrete  Architecture.WinApp.pcc   [pcc] schema  DocumentTypeId 1851  1851_USP_BillOfMaterial_Slip.rpt
 * </pre>
 */
public final class W5BBomProfile {

    public final String key;          // mfg | casting | concrete
    public final int screenId;
    public final String desktopClass;
    public final String screen;       // base.Name - the rights key
    public final int docType;
    public final String schema;       // Mfg | pcc
    public final String route;
    public final String title;        // desktop form title strip
    public final String historyTitle;
    public final String rpt;
    public final String pageTitle;

    /** Mfg + Casting: Mfg.BomHeader. Concrete: pcc.BomHeader. */
    public final boolean mfgSchema;
    /** 774 (and 598 for item / wages): Item combo, Contractor Wages grid, Operations grid, Routing combo, IsActive check. */
    public final boolean hasItem, hasWages, hasOperations, hasRouting, hasActive;
    /** 774 + 845: Casting Type combo, UOM combo from the UOM master. */
    public final boolean hasCastingType;
    /** 598 only: Production Type, Plant, Top / Bottom Force, Min/Max/Main item input columns. */
    public final boolean concrete;
    /** 845: "Qty Field Required". */
    public final boolean qtyRequired;

    private W5BBomProfile(String key, int screenId, String desktopClass, String screen, int docType, String schema, String route,
                          String title, String historyTitle, String rpt, String pageTitle, boolean mfgSchema, boolean hasItem,
                          boolean hasWages, boolean hasOperations, boolean hasRouting, boolean hasActive, boolean hasCastingType,
                          boolean concrete, boolean qtyRequired) {
        this.key = key; this.screenId = screenId; this.desktopClass = desktopClass; this.screen = screen; this.docType = docType;
        this.schema = schema; this.route = route; this.title = title; this.historyTitle = historyTitle; this.rpt = rpt;
        this.pageTitle = pageTitle; this.mfgSchema = mfgSchema; this.hasItem = hasItem; this.hasWages = hasWages;
        this.hasOperations = hasOperations; this.hasRouting = hasRouting; this.hasActive = hasActive;
        this.hasCastingType = hasCastingType; this.concrete = concrete; this.qtyRequired = qtyRequired;
    }

    public static final W5BBomProfile MFG = new W5BBomProfile("mfg", 774, "frmBillOfMaterial", "frmBillOfMaterial", 1617, "Mfg",
            "/production/mfg/bill-of-material", "Bill Of Material Manufacturing", "Bill Of Material Manufacturing History",
            "1617_USP_BillOfMaterial_Slip.rpt", "Bill Of Material", true, true, true, true, true, true, true, false, false);

    public static final W5BBomProfile CASTING = new W5BBomProfile("casting", 845, "frmBillOfMaterialCasting", "frmBillOfMaterialCasting", 1664, "Mfg",
            "/production/mfg/bill-of-material-casting", "Bill Of Material Casting", "Bill Of Material Casting History",
            "1664_BillOfMaterialCasting_Slip.rpt", "Bill Of Material Casting", true, false, false, false, false, false, true, false, true);

    public static final W5BBomProfile CONCRETE = new W5BBomProfile("concrete", 598, "frmBillOfMaterialConcrete", "frmBillOfMaterialConcrete", 1851, "pcc",
            "/production/pcc/bill-of-material", "Bill Of Material Manufacturing", "Bill Of Material Manufacturing History",
            "1851_USP_BillOfMaterial_Slip.rpt", "Bill Of Material", false, true, true, false, false, true, false, true, false);

    public String getProc() { return "[" + schema + "].[USP_BomHeader_GetAllMethod]"; }

    /** The flags the page script reads. */
    public Map<String, Object> config() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("variant", key);
        m.put("screenId", screenId);
        m.put("screen", screen);
        m.put("docType", docType);
        m.put("rpt", rpt);
        m.put("title", title);
        m.put("hasItem", hasItem);
        m.put("hasWages", hasWages);
        m.put("hasOperations", hasOperations);
        m.put("hasRouting", hasRouting);
        m.put("hasActive", hasActive);
        m.put("hasCastingType", hasCastingType);
        m.put("concrete", concrete);
        m.put("qtyRequired", qtyRequired);
        return m;
    }
}
