package com.mst.services;

import com.mst.repositories.ProductionAgainstJobOrderRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.ProductionAgainstJobOrderRepository.col;
import static com.mst.repositories.ProductionAgainstJobOrderRepository.project;

/**
 * Screen 280 "Production Against (Job Order)" — the shell.
 *
 * frmFoodProduction_Load:452 reads seven switches and the rights object before the screen shows
 * anything, and their effects are structural: a panel disappears, a button disappears, two whole
 * tabs are removed. This service reproduces that decision, from the database, so the page can
 * render the same shape the desktop would.
 *
 * Six of the eight tabs host a separate desktop form, each ported as its own page and framed
 * into the tab (see {@link #tabs}). The Consumption and Transaction History tab pages belong to
 * FoodProductionWithValues itself; their data and save path are ProductionConsumptionService.
 */
@Service
public class ProductionAgainstJobOrderService {

    /** ScreenDefinition.Id — confirmed in migration/user-rights/reconciliation-input.json. */
    public static final int SCREEN_ID = 280;

    /** WagesRefDocumentsStatusList row ids the shell looks up (:459, :461). */
    private static final int REF_DOC_TYPE_OUTPUT      = 112;
    private static final int REF_DOC_TYPE_CONSUMPTION = 181;

    /** CommonServices.GetERPFeatureById(5) -> FIFOCGSFlag (:516). */
    private static final int ERP_FEATURE_FIFO_CGS = 5;

    /** GetActiveWareHouseByWareHouseType(2) — the Production warehouse type. */
    private static final int WAREHOUSE_TYPE_PRODUCTION = 2;

    /** The four names sent as ONE comma-separated request (:465), spelled as the desktop spells them. */
    private static final String CONFIG_REQUEST =
            "WagesCompulsoryOnProduction,IssuanceByLoader,IsManualEntryOnInputNotAllowed,"
          + "ByProductRateEditableIsAllow";

    @Autowired private ProductionAgainstJobOrderRepository repo;
    @Autowired private CurrentUserContext currentUserContext;

    /* -------------------------------------------------------------------------- the switches */

    /**
     * Everything frmFoodProduction_Load decides before the screen is usable.
     *
     * A configuration that cannot be read is reported as FALSE, which is what
     * `Conversion.ToBool(null)` yields on the desktop — and false is the restrictive answer for
     * every one of these except `IsManualEntryOnInputNotAllowed`, where false means the manual
     * panel stays visible, again matching the desktop.
     */
    public Map<String, Object> shellState() {
        int org  = currentUserContext.currentOrganizationId();
        int comp = currentUserContext.currentCompanyId();

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("screenId", SCREEN_ID);
        out.put("screenName", ProductionAgainstJobOrderRepository.DESKTOP_SCREEN_NAME);
        out.put("title", "Production Against (Job Order)");

        /* :458-462 — two rows picked out of the wages status list by RefDocumentTypeId. */
        boolean wagesOutput = false, wagesConsumption = false;
        try {
            for (Map<String, Object> r : repo.wagesRefDocumentStatuses()) {
                int id = asInt(col(r, "RefDocumentTypeId"));
                boolean active = asBool(col(r, "IsActive"));
                if (id == REF_DOC_TYPE_OUTPUT)      wagesOutput = active;
                if (id == REF_DOC_TYPE_CONSUMPTION) wagesConsumption = active;
            }
        } catch (Exception ignored) {
            /* The desktop's Find() returns null and both flags stay false. */
        }
        out.put("wagesActiveForOutput", wagesOutput);
        out.put("wagesActiveForConsumption", wagesConsumption);

        /* :463-464 — two single-value configurations. */
        out.put("saleCostingJobOrderWise",
                asBool(safeConfig(org, comp, "SaleCostingJobOrderWise")));
        out.put("outputItemsByJobOrderRateSchedule",
                asBool(safeConfig(org, comp, "OutputItemsByJobOrderRateSchedule")));

        /* :465-495 — four configurations in one call, matched on ConfigDescription. */
        boolean wagesStatus = false, loaderStatus = false;
        boolean manualEntryNotAllowed = false, byProductRateEditable = false;
        try {
            for (Map<String, Object> r : repo.multipleConfigurations(org, comp, CONFIG_REQUEST)) {
                String name = str(col(r, "ConfigDescription"));
                boolean v = asBool(col(r, "ConfigKey"));
                if ("WagesCompulsoryOnProduction".equals(name))    wagesStatus = v;
                if ("IssuanceByLoader".equals(name))               loaderStatus = v;
                if ("IsManualEntryOnInputNotAllowed".equals(name)) manualEntryNotAllowed = v;
                if ("ByProductRateEditableIsAllow".equals(name))   byProductRateEditable = v;
            }
        } catch (Exception ignored) { }
        out.put("wagesCompulsoryOnProduction", wagesStatus);
        out.put("issuanceByLoader", loaderStatus);
        out.put("isManualEntryOnInputNotAllowed", manualEntryNotAllowed);
        out.put("byProductRateEditable", byProductRateEditable);

        /* :516 — FIFO/CGS hides the "Pending For Rates" button and disables Rate + its UOM. */
        boolean fifo = false;
        try { fifo = repo.erpFeature(org, comp, ERP_FEATURE_FIFO_CGS); } catch (Exception ignored) { }
        out.put("fifoCgs", fifo);

        /* :490-514 — the rights object. */
        Map<String, Boolean> rights = rights();
        out.put("rights", rights);

        out.put("tabs", tabs(rights));
        return out;
    }

    /**
     * CommonServices.SetRightsValueInRightsObject(base.Name) for this screen, rule for rule
     * (CommonServices.cs:14738).
     *
     *  - Only the role spelled exactly "Admin" is special (C# `==`, case-sensitive). It starts
     *    with Save, Update, Delete and Print set, and the grant rows still run afterwards:
     *    Save/Update/Print stay true for Admin, but a "Delete" row overrides even for Admin.
     *  - "Rate" -> DoHaveCanRateandAmount and "ProductionSettlement" -> DoHaveCanSettlementtab
     *    come ONLY from grant rows. Admin gets neither by default, so an Admin without those rows
     *    sees no rate columns and no Settlement/Overhead tabs, exactly as on the desktop.
     *
     * Pass 2 corrected three things here: the settlement right is named "ProductionSettlement"
     * (the previous "Settlement"/"CanSettlementtab" never matched a row, so no non-Admin user
     * could ever see those tabs); Admin no longer short-circuits Rate and Settlement to true;
     * and "Administrator" is no longer treated as Admin.
     *
     * An unreadable grid is NOT an implicit grant: everything not set above stays false.
     */
    public Map<String, Boolean> rights() {
        Map<String, Boolean> r = new LinkedHashMap<>();
        String role = currentUserContext.currentRoleName();
        boolean admin = "Admin".equals(role);
        r.put("save", admin);
        r.put("update", admin);
        r.put("delete", admin);
        r.put("print", admin);
        r.put("rateAndAmount", false);
        r.put("settlementTab", false);
        try {
            for (Map<String, Object> row : repo.userRightsForScreen(
                    currentUserContext.currentUserId(),
                    ProductionAgainstJobOrderRepository.DESKTOP_SCREEN_NAME,
                    role, currentUserContext.currentCompanyId())) {
                String name = str(col(row, "RightName"));
                boolean v = asBool(col(row, "Value"));
                switch (name) {
                    case "Save":   r.put("save",   admin || v); break;
                    case "Update": r.put("update", admin || v); break;
                    case "Print":  r.put("print",  admin || v); break;
                    case "Delete": r.put("delete", v);          break;
                    /* hides Rate, Rate UOM, Amount, their labels, the rate buttons and the
                       voucher-print button, and widens Remarks to 640 (:505-512). */
                    case "Rate":   r.put("rateAndAmount", v);   break;
                    /* REMOVES the Settlement and Overhead tabs when false (:476). */
                    case "ProductionSettlement": r.put("settlementTab", v); break;
                    default: break;
                }
            }
        } catch (Exception ignored) {
            /* leave the defaults above in place */
        }
        return r;
    }

    /**
     * The tab strip in the order the desktop ends up with after frmFoodProduction_Load.
     *
     * The designer adds the pages as Input, OutPut, Consumption, PackingMaterial, OverHead,
     * TransctionHistory, Summary, Settlement (:3719-3726). Load then removes pages and re-inserts
     * runtime pages by index (FormHelper.AddTabPageToTabControl: Insert when the index is within
     * the count, otherwise Add). Replaying those steps gives:
     *
     *   with ProductionSettlement:    Input, Output, Consumption, PackingMaterial, Overhead,
     *                                 Transaction History, Summary, Settlement
     *   without it (:476 removes two): Input, Output, Consumption, PackingMaterial,
     *                                 Transaction History, Summary
     *
     * Pass 2 corrected the order: Overhead had been placed before PackingMaterial and
     * Transaction History at the end.
     *
     * The Summary tab is literally `new ProductionSummaryReport(UserAccount)` (:859) - the same
     * form already built as report 309, so it points there instead of being rebuilt.
     */
    public List<Map<String, Object>> tabs(Map<String, Boolean> rights) {
        boolean settlementAllowed = Boolean.TRUE.equals(rights.get("settlementTab"));
        List<Map<String, Object>> t = new ArrayList<>();
        /* Each hosted form is its own page (PanelOtherForms <- new frmX(UserAccount), :839) and is
           framed into the tab, exactly as the desktop drops a separate form into the panel. */
        String b = "/production/production-against-job-order/";
        t.add(tab("Input",           "frmProductionInput.cs",           "5,790 lines", false, b + "input"));
        t.add(tab("Output",          "frmProductionOutput.cs",          "5,981 lines", false, b + "output"));
        t.add(tab("Consumption",     "FoodProductionWithValues.cs", "a tab page of this form - :1246-3303 + designer :3755-5365", true, null));
        t.add(tab("PackingMaterial", "frmProductionPackingMaterial.cs", "4,213 lines", false, b + "packing-material"));
        if (settlementAllowed) {
            t.add(tab("Overhead",    "frmProductionOverhead.cs",        "3,390 lines", false, b + "overhead"));
        }
        t.add(tab("Transaction History", "FoodProductionWithValues.cs", "a tab page of this form - :890-1245 + designer :5418-5810", true, null));
        t.add(tab("Summary",         "ProductionSummaryReport.cs",  "1,772 lines", false,
                  "/production/reports/production-summary"));
        if (settlementAllowed) {
            t.add(tab("Settlement",  "frmProductionSettlement.cs",      "3,962 lines", false, b + "settlement"));
        }
        return t;
    }

    private static Map<String, Object> tab(String name, String form, String size, boolean ownTabPage,
                                           String route) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("name", name);
        m.put("desktopForm", form);
        m.put("desktopSize", size);
        /* true: an ordinary tab page of FoodProductionWithValues itself (:853-857 hides
           PanelOtherForms); false: a separate form constructed into PanelOtherForms. */
        m.put("ownTabPage", ownTabPage);
        m.put("route", route);
        /* Consumption and Transaction History are this form's own tab pages and are built into the
           shell page itself (countx_p280_consumption.js); the others are framed from their route. */
        m.put("built", route != null || ownTabPage);
        return m;
    }

    /* --------------------------------------------------------------------------- the pickers */

    /** Production Department — GetActiveWareHouseByWareHouseType(2). */
    public List<Map<String, Object>> productionDepartments() {
        return project(repo.productionDepartments(currentUserContext.currentOrganizationId(),
                                                  currentUserContext.currentCompanyId(),
                                                  WAREHOUSE_TYPE_PRODUCTION),
                       "Id", "WareHouseName");
    }

    /**
     * Job Order picker — usp_getJobOrdersForProduction.
     *
     * BranchesFill() on this form is an EMPTY METHOD BODY: it is declared and does nothing, so
     * the desktop applies no branch filter of its own here. The branch still travels to the
     * procedure, because the procedure takes it — it comes from the session, never the request.
     */
    public List<Map<String, Object>> jobOrders() {
        return repo.jobOrdersForProduction(currentUserContext.currentOrganizationId(),
                                           currentUserContext.currentCompanyId(),
                                           currentUserContext.currentBranchId());
    }

    /** Plant picker — usp_getPlantsForProductionByJobOrderId, scoped to the chosen job order. */
    public List<Map<String, Object>> plants(int jobOrderId) {
        if (jobOrderId <= 0) return new ArrayList<>();
        return repo.plantsForJobOrder(currentUserContext.currentOrganizationId(),
                                      currentUserContext.currentCompanyId(),
                                      currentUserContext.currentFinancialYearId(),
                                      currentUserContext.currentBranchId(),
                                      jobOrderId);
    }

    /** Consumption's WIP Account / WIP Item - cmbJobOrderConsumption_Leave:1433. */
    public List<Map<String, Object>> glAccounts(int jobOrderId) {
        if (jobOrderId <= 0) return new ArrayList<>();
        return repo.glAccountsByJobOrderId(currentUserContext.currentOrganizationId(),
                                           currentUserContext.currentCompanyId(), jobOrderId);
    }

    /** UOMSchedule.Getall — the whole-company list the shell caches and every tab reads. */
    public List<Map<String, Object>> uomSchedules() {
        return repo.uomSchedules(currentUserContext.currentOrganizationId(),
                                 currentUserContext.currentCompanyId());
    }

    /* --------------------------------------------------------------------------- helpers */

    private String safeConfig(int org, int comp, String name) {
        try { return repo.configValue(org, comp, name); } catch (Exception e) { return ""; }
    }

    private static boolean equalsAny(String v, String... options) {
        for (String o : options) if (o.equalsIgnoreCase(v)) return true;
        return false;
    }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }

    private static int asInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        try { return Integer.parseInt(str(o)); } catch (Exception e) { return 0; }
    }

    /** Conversion.ToBool — the desktop accepts true/false, 1/0 and "1"/"0" alike. */
    private static boolean asBool(Object o) {
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number)  return ((Number) o).intValue() != 0;
        String s = str(o).toLowerCase();
        return "1".equals(s) || "true".equals(s) || "yes".equals(s);
    }
}
