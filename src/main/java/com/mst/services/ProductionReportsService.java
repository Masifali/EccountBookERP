package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ProductionReportsRepository;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * The five Production Reports. Read-only — nothing here writes, so none of the posting risk that
 * gates Stock Conversion's Save applies.
 *
 * Organization and company come from the signed-in user on every call. They are not parameters:
 * a report that accepted a company id would hand one company's production figures to another.
 *
 * Reports 975 (Job Order Summary) and 309 (Production Summary) are built. The other three land
 * here as they are built, sharing this service and its repository.
 */
@Service
public class ProductionReportsService {

    private static final Logger LOG = LoggerFactory.getLogger(ProductionReportsService.class);

    @Autowired private ProductionReportsRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private JdbcTemplate jdbcTemplate;

    /** ProductionSummaryReport.cs:271 — the form fixes this; it is not a caller's choice. */
    private static final int JOB_ORDER_DOCUMENT_TYPE_ID = 403;

    /** base.Name, and the ScreenName dbo.ScreenDefinition 309 carries. */
    private static final String SCREEN_PRODUCTION_SUMMARY = "ProductionSummaryReport";

    /**
     * CommonServices.cs:17661 — {@code DoHaveCanRateandAmount} is the row whose RightName is
     * "Rate". Note it is one of the rights the desktop does NOT grant Admin blanket access to, so
     * there is no role short-circuit here either.
     */
    private static final String RIGHT_RATE = "Rate";

    private static final String SQL_USER_RIGHTS_FOR_SCREEN =
            "EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?, @ScreenName=?, @RightName=?, "
            + "@CompanyId=?, @Activity=?";

    // ============================================================ 975 — Job Order Summary

    /**
     * Job Order Summary — frmProductionJobOrderSummaryRpt.
     *
     * @param approvalFilter "All", "Approved" or "Unapproved". "All" omits &#64;IsApproved
     *                       entirely, which is what the desktop's rdAll radio does; the other two
     *                       send true/false. Anything else is refused rather than quietly treated
     *                       as "All", because silently widening a filter changes what the operator
     *                       is looking at.
     * @param fromDate       optional — the desktop's From picker has its own checkbox and is
     *                       unticked by default, so an absent from-date is normal, not missing.
     */
    public List<Map<String, Object>> jobOrderSummary(String fromDate, String toDate,
                                                     String approvalFilter) {
        UserAccount u = currentUserContext.requireAccountingUser();
        Boolean isApproved;
        String f = approvalFilter == null ? "All" : approvalFilter.trim();
        if (f.isEmpty() || "All".equalsIgnoreCase(f))        isApproved = null;
        else if ("Approved".equalsIgnoreCase(f))             isApproved = Boolean.TRUE;
        else if ("Unapproved".equalsIgnoreCase(f)
              || "UnApproved".equalsIgnoreCase(f))           isApproved = Boolean.FALSE;
        else throw new IllegalArgumentException(
                "Approval filter must be All, Approved or Unapproved");
        /* gridHisory:151-176 copies twelve columns of the result into dtGrid, three of them
           renamed (BalWeight, BalAmount, LedgerBalAmount); the procedure's other columns
           (ProductionStartDate, InputQty, ...) are not on the grid. The raw rows still go to the
           672 print, which the page asks for through /api/reports/jos-672. */
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.jobOrderSummary(u, fromDate, toDate, isApproved)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id",               civ(r, "Id"));
            o.put("DocDate",          civ(r, "DocDate"));
            o.put("JobOrderNo",       civ(r, "JobOrderNo"));
            o.put("InputWeight",      civ(r, "InputWeight"));
            o.put("InputAmount",      civ(r, "InputAmount"));
            o.put("OutPutWeight",     civ(r, "OutPutWeight"));
            o.put("OutPutAmount",     civ(r, "OutPutAmount"));
            o.put("BalanceWeight",    civ(r, "BalWeight"));
            o.put("BalanceAmount",    civ(r, "BalAmount"));
            o.put("LedgerBalance",    civ(r, "LedgerBalAmount"));
            o.put("SettlementStatus", civ(r, "SettlementStatus"));
            o.put("ApprovalStatus",   civ(r, "ApprovalStatus"));
            o.put("JobOrderStatus",   civ(r, "JobOrderStatus"));
            out.add(o);
        }
        return out;
    }

    /**
     * InitializeComponentMethod:122 - From = today minus DefaultDaysToLessFromHistoryFromDate
     * (3 when that is not above 0), unticked; To = today. Plus the amount format behind
     * stringFormatboth (CommonServices.GetDecimalConfiguration:4914).
     */
    public Map<String, Object> jobOrderSummaryDefaults() {
        UserAccount u = currentUserContext.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        int days = intConfig(u, "DefaultDaysToLessFromHistoryFromDate");
        out.put("fromDaysBack", days > 0 ? days : 3);
        out.put("amountDecimals", amountDecimals(u));
        return out;
    }

    /** "Default NoofDecimal Points For Amount": 1-4 decimals; anything else gives "#,##0." (none). */
    public int amountDecimals(UserAccount u) {
        int n = intConfig(u, "Default NoofDecimal Points For Amount");
        return (n >= 1 && n <= 4) ? n : 0;
    }

    private int intConfig(UserAccount u, String name) {
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    "EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, "
                  + "@ConfigDescription=?, @Activity=?",
                    u.getOrganizationId(), u.getCompanyId(), name,
                    "GetConfigurationByOrgCompandConfigDescription");
            if (rows.isEmpty()) return 0;
            Object v = civ(rows.get(0), "ConfigKey");
            return v == null ? 0 : (int) Math.floor(Double.parseDouble(String.valueOf(v).trim()));
        } catch (Exception e) {
            LOG.warn("Configuration '{}' could not be read; treating as 0", name, e);
            return 0;
        }
    }

    private static Object civ(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    /**
     * The drill-down behind a Job Order Summary line - DataGridHistory_LinkClicked:210 always
     * sends actionId = 1, and the procedure returns rows only when ISNULL(@ActionId,0) = 1.
     * It is fixed here, not taken from the caller.
     */
    public List<Map<String, Object>> productionSettlementDetail(int jobOrderId, int actionId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        if (jobOrderId <= 0) throw new IllegalArgumentException("Select a job order");
        return repo.productionSettlementDetail(u, jobOrderId, 1);
    }

    // ============================================================ 309 — Production Summary Report

    /** The branch multi-select. Also the whitelist every posted branch id is checked against. */
    public Map<String, Object> productionSummaryLookups() {
        UserAccount u = currentUserContext.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> branches = repo.productionBranchesForUser(u);
        out.put("branches", branches);
        /* The desktop preselects the signed-in user's own branch:
           cmbBranchName.Text = UserAccount.BranchName (BranchesFill:222). */
        out.put("defaultBranchId", u.getBranchesId());
        out.put("canSeeRateAndAmount", canSeeRateAndAmount());
        return out;
    }

    /**
     * The job-order picker. Cascades off the branch multi-select exactly as cmbBranchName_Leave
     * does — change the branches, the job-order list is rebuilt.
     */
    public List<Map<String, Object>> productionSummaryJobOrders(List<Integer> branchIds) {
        UserAccount u = currentUserContext.requireAccountingUser();
        return repo.jobOrderNoWithInfo(u, financialYearId(), JOB_ORDER_DOCUMENT_TYPE_ID,
                branchIdsCsv(u, branchIds));
    }

    /**
     * The plant picker, cascading off the chosen job order — cmbsummeryJobOrderNo_TextChanged:291.
     * The branch it filters by is the user's own, not the ticked ones, as in the desktop.
     */
    public List<Map<String, Object>> productionSummaryPlants(int jobOrderId,
                                                             List<Integer> branchIds) {
        UserAccount u = currentUserContext.requireAccountingUser();
        if (jobOrderId <= 0) return new ArrayList<>();
        requireOwnJobOrder(u, jobOrderId, branchIds);
        return repo.plantByJobOrderId(jobOrderId, currentUserContext.currentBranchId());
    }

    /**
     * btnShow_Click:477 — the desktop fills all five panels from one click, so one call returns
     * all five. Splitting them would let the panels drift apart between clicks.
     *
     * Every value column is removed SERVER-SIDE when the user lacks the "Rate" right. The desktop
     * only hides the columns, which is enough for a WinForms grid; on the web, a hidden column is
     * still in the response, so withholding it is what actually honours the same right.
     */
    public Map<String, Object> productionSummary(int jobOrderId, int plantId, String fromDate,
                                                 String toDate, List<Integer> branchIds) {
        UserAccount u = currentUserContext.requireAccountingUser();
        if (jobOrderId <= 0) throw new IllegalArgumentException("Select a job order");
        requireOwnJobOrder(u, jobOrderId, branchIds);

        String csv = branchIdsCsv(u, branchIds);
        boolean rate = canSeeRateAndAmount();

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("canSeeRateAndAmount", rate);

        /* grdSummery — GrdSummaryValuesFill:527. The desktop copies ten named columns into a new
           table, renaming two of them, and shows nothing else the procedure returns. */
        out.put("values", project(
                repo.productionSummaryValues(u, jobOrderId, plantId, fromDate, toDate, csv),
                new String[][] {
                        { "SortNo",                "SortNo" },
                        { "TranType",              "TranType" },
                        { "WeightPrcnt",           "WeightPrcnt" },
                        { "NetQty",                "NetQty" },
                        { "NetWeight",             "NetWeight" },
                        { "Amount",                "Amount" },
                        { "AvgRate",               "AvgRate" },
                        { "AvgRateWoExp",          "AvgRateWoExp" },
                        { "OverHeadAmount",        "ByProductOverHeads" },
                        { "PackingMaterialAmount", "ByProductPackingMaterial" },
                },
                /* GrdSummarySetting:565 hides exactly these two without the right. The other
                   three money-ish columns stay, because the desktop leaves them visible. */
                rate ? null : new String[] { "AvgRate", "Amount" }));

        /* grdSummaryExportScheduleInfo — GridSummaryJobOrderScheduleDetail:632. The three id
           columns are added and then hidden (ScheduleGridSetting:660); they are kept in the
           payload because the row needs them to stay identifiable, and the page does not draw
           them. */
        out.put("schedule", project(
                repo.jobOrderScheduleSummary(jobOrderId),
                new String[][] {
                        { "ContractId",   "OrderId" },
                        { "SaleContract", "ContractNo" },
                        { "ScheduleId",   "ScheduleId" },
                        { "ScheduleNo",   "ScheduleNo" },
                        { "SupCustId",    "SupCustId" },
                        { "CustomerName", "CustomerName" },
                },
                null));

        /* grdSummaryDetail — GrdSummaryReportFill:711, grouped by TransactionType. */
        out.put("recovery", project(
                repo.productionRecoverySummary(u, jobOrderId, plantId, fromDate, toDate, csv),
                new String[][] {
                        { "TransactionType", "TranType" },
                        { "PlantName",       "PlantName" },
                        { "ItemName",        "ItemName" },
                        { "CropYear",        "CropBatch" },
                        { "PackSize",        "UOMCode" },
                        { "JobLot",          "JobLotCode" },
                        { "QtyTotal",        "NetQty" },
                        { "NetWeightTotal",  "NetWeight" },
                        { "Amount",          "Amount" },
                        { "TransactionRate", "TranRate" },
                        { "WeightPrcnt",     "WeightPrcnt" },
                        { "NetRate",         "NetRate" },
                },
                /* SummeryHistoryGridSetting:747. */
                rate ? null : new String[] { "TransactionRate", "Amount", "NetRate" }));

        /* grdSummaryDocWiseDetail — grdDocWiseSummeryFill:803, grouped by TransactionType.
           Note DocNo comes from DocCode, and Remarks from the lowercase "remarks". */
        out.put("docWise", project(
                repo.productionDocWiseSummary(u, jobOrderId, plantId, fromDate, toDate, csv),
                new String[][] {
                        { "SortNo",          "SortNo" },
                        { "DocNo",           "DocCode" },
                        { "DocDate",         "DocDate" },
                        { "TransactionType", "TranType" },
                        { "ItemName",        "ItemName" },
                        { "PackSize",        "UOMCode" },
                        { "CropYear",        "CropBatch" },
                        { "Remarks",         "remarks" },
                        { "JobLot",          "JobLotCode" },
                        { "NetQty",          "NetQty" },
                        { "NetWeight",       "NetWeight" },
                        { "Amount",          "Amount" },
                        { "TranRate",        "TranRate" },
                        { "WeightPrcnt",     "WeightPrcnt" },
                        { "NetRate",         "NetRate" },
                },
                /* grdDocWiseSummerySetting:839. TotalAmountBySortNo is named there but is not one
                   of the fifteen columns the form copies across, so it never exists to hide. */
                rate ? null : new String[] { "TranRate", "Amount", "NetRate" }));

        return out;
    }

    // ------------------------------------------------------------ helpers

    /**
     * The branch string the three 309 procedures expect: the ticked branch ids, comma-separated
     * with a LEADING comma, exactly as the form builds it (GrdSummaryValuesFill:510).
     *
     * Ids a browser sends are not trusted. Each is matched against this user's own allocation from
     * USP_GetBranchsAllocatedToUserFromProduction; anything else is dropped, so a caller cannot
     * widen the report to a branch they were never allocated by editing the request.
     */
    /**
     * The @BranchesIds the three report dropdown loaders send (308 ComboBindComparison:277 and
     * ComboBindSummary:355, 306 ComboBind:143).
     *
     * Each one first sets {@code BranchesIds = UserAccount.BranchesId.ToString()} and only
     * replaces it when the branch box has text. So with nothing ticked the desktop lists the
     * signed-in user's OWN branch - never every branch.
     *
     * Pass 2 correction: with nothing ticked the web sent "", which the repository omits, so the
     * procedure received NULL and listed job orders, plants and items across ALL branches. Note the fallback is the bare id ("5"), without the
     * leading comma the ticked form builds - also as the desktop sends it.
     */
    private String dropdownBranches(UserAccount u, List<Integer> requested) {
        String csv = branchIdsCsv(u, requested);
        if (!csv.isEmpty()) return csv;
        Integer own = u.getBranchesId();
        return own == null ? "" : String.valueOf(own);
    }

    private String branchIdsCsv(UserAccount u, List<Integer> requested) {
        if (requested == null || requested.isEmpty()) return "";
        List<Integer> allowed = new ArrayList<>();
        for (Map<String, Object> r : repo.productionBranchesForUser(u)) {
            Object v = ci(r, "BranchId");
            if (v instanceof Number) allowed.add(((Number) v).intValue());
            else if (v != null) {
                try { allowed.add(Integer.parseInt(v.toString().trim())); } catch (Exception ignored) { }
            }
        }
        StringBuilder sb = new StringBuilder();
        for (Integer id : requested) {
            if (id != null && allowed.contains(id)) sb.append(',').append(id);
        }
        return sb.toString();
    }

    /**
     * USP_InvProductionJobOrderAndOrderAllocation_SummaryByJobOrderId takes ONLY &#64;JobOrderId —
     * no organization, no company — so it would happily return another company's contracts and
     * customer names for a job order id typed into the URL. The desktop cannot hit that, because
     * the only way to set the id there is to pick a row from a dropdown that was already filtered.
     * This reproduces that boundary explicitly: the id has to be in the list this user's own
     * filters produce.
     */
    private void requireOwnJobOrder(UserAccount u, int jobOrderId, List<Integer> branchIds) {
        List<Map<String, Object>> list = repo.jobOrderNoWithInfo(
                u, financialYearId(), JOB_ORDER_DOCUMENT_TYPE_ID, branchIdsCsv(u, branchIds));
        for (Map<String, Object> r : list) {
            Object v = ci(r, "Id");
            if (v instanceof Number && ((Number) v).intValue() == jobOrderId) return;
            if (v != null && v.toString().trim().equals(String.valueOf(jobOrderId))) return;
        }
        throw new IllegalArgumentException("That job order is not available on this screen");
    }

    /**
     * CommonServices.cs:17661 — the "Rate" right. Computed per call, never cached in a field: this
     * is a singleton bean, and a cached answer would carry one user's permission into the next
     * request. A failed read denies rather than grants.
     */
    private boolean canSeeRateAndAmount() {
        String role = currentRoleName();
        try {
            List<Map<String, Object>> rights = jdbcTemplate.queryForList(
                    SQL_USER_RIGHTS_FOR_SCREEN,
                    currentUserContext.currentUserId(),
                    SCREEN_PRODUCTION_SUMMARY,
                    role == null ? "" : role,
                    currentUserContext.currentCompanyId(),
                    "GetByUserId");
            for (Map<String, Object> r : rights) {
                Object name = ci(r, "RightName");
                if (name != null && RIGHT_RATE.equalsIgnoreCase(name.toString().trim())) {
                    return toBool(ci(r, "Value"));
                }
            }
        } catch (Exception e) {
            LOG.warn("Could not read the Rate right for {}; hiding rate and amount",
                    SCREEN_PRODUCTION_SUMMARY, e);
        }
        return false;
    }

    private String currentRoleName() {
        try { return currentUserContext.currentRoleName(); }
        catch (Exception e) { return null; }
    }

    private int financialYearId() {
        try { return currentUserContext.currentFinancialYearId(); }
        catch (Exception e) { return 0; }
    }

    /**
     * Copies the named source columns into the desktop's display names, in the desktop's order,
     * dropping any the caller is not allowed to see. A column the procedure did not return comes
     * back as null rather than throwing, which is what DataRow indexing into a DataTable built
     * this way effectively gives the desktop.
     */
    private static List<Map<String, Object>> project(List<Map<String, Object>> rows,
                                                     String[][] mapping, String[] drop) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (rows == null) return out;
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (String[] m : mapping) {
                if (drop != null && contains(drop, m[0])) continue;
                o.put(m[0], ci(r, m[1]));
            }
            out.add(o);
        }
        return out;
    }

    private static boolean contains(String[] a, String s) {
        for (String x : a) if (x.equalsIgnoreCase(s)) return true;
        return false;
    }

    /** SQL Server column names come back with the procedure's own casing; match without it. */
    private static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    private static boolean toBool(Object v) {
        if (v == null) return false;
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        String s = v.toString().trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s) || "Y".equalsIgnoreCase(s);
    }

    // ============================================================ 310 — Production Register

    /**
     * Activityfill:472 — three fixed rows, hard-coded in the desktop form itself. They are not a
     * dropdown this port invented: the value goes to the procedure's &#64;Activity and selects
     * which of three result shapes comes back, so it may only ever be one of these three.
     */
    private static final List<String> REGISTER_ACTIVITIES =
            java.util.Arrays.asList("Production Register", "OutPut By Packing Material",
                                    "Production_Summary");

    /**
     * EntryTypeFill:449 — also hard-coded in the form. The Id here is a STRING key, not a number:
     * "Issue", "ByProduct", "FinishGoods" are what go to &#64;EntryTypeDetail, while the caption
     * is what the operator ticks.
     */
    private static final String[][] REGISTER_ENTRY_TYPES = {
            { "Issue",       "Material Input" },
            { "ByProduct",   "OutPut By Product" },
            { "FinishGoods", "OutPut Finish Goods" },
    };

    /**
     * AllCombobind:288 — ONE procedure feeds six pickers. Its rows carry an {@code Activity}
     * discriminator and the form sorts them into six tables by it, taking Id and ReferenceName
     * from each. Splitting server-side keeps that mapping in one place.
     *
     * The dropdown names the form gives each list are kept ("ParentCategory", "Warehouse",
     * "ItemName", "Plant", "WipAccount", "StockAccount") because they are the discriminator
     * values, not labels.
     */
    public Map<String, Object> productionRegisterLookups() {
        UserAccount u = currentUserContext.requireAccountingUser();

        Map<String, List<Map<String, Object>>> buckets = new LinkedHashMap<>();
        for (String key : new String[] { "ParentCategory", "Warehouse", "ItemName", "Plant",
                                         "WipAccount", "StockAccount" }) {
            buckets.put(key, new ArrayList<>());
        }
        for (Map<String, Object> r : repo.productionDropdownSource(u)) {
            Object activity = ci(r, "Activity");
            if (activity == null) continue;
            List<Map<String, Object>> bucket = buckets.get(activity.toString().trim());
            if (bucket == null) continue;          /* a discriminator this form does not use */
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(r, "Id"));
            o.put("Name", ci(r, "ReferenceName"));
            bucket.add(o);
        }

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("parentCategories", buckets.get("ParentCategory"));
        out.put("warehouses",       buckets.get("Warehouse"));
        out.put("items",            buckets.get("ItemName"));
        out.put("plants",           buckets.get("Plant"));
        out.put("wipAccounts",      buckets.get("WipAccount"));
        out.put("stockAccounts",    buckets.get("StockAccount"));
        out.put("jobOrders",        repo.jobOrdersFromProduction(u));
        out.put("branches",         repo.productionBranchesForUser(u));
        out.put("defaultBranchId",  u.getBranchesId());

        List<Map<String, Object>> entryTypes = new ArrayList<>();
        for (String[] e : REGISTER_ENTRY_TYPES) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", e[0]);
            o.put("Activity", e[1]);
            entryTypes.add(o);
        }
        out.put("entryTypes", entryTypes);
        out.put("activities", REGISTER_ACTIVITIES);
        return out;
    }

    /**
     * Gridfill:495. One procedure, three shapes — the Activity chooses which, and each shape gets
     * its own projection because the desktop builds a different DataTable for each.
     *
     * @param activity   one of REGISTER_ACTIVITIES. Refused otherwise rather than passed through:
     *                   it goes straight to &#64;Activity, and a value the procedure does not know
     *                   would return something this screen cannot render.
     * @param dateMode   "doc" or "entry" — the rdDocDate / rdEntryDate radio. The chosen mode
     *                   decides which PAIR of parameters the dates land in; the other pair is not
     *                   sent at all.
     * @param entryTypes the ticked entry-type keys ("Issue", "ByProduct", "FinishGoods").
     */
    public Map<String, Object> productionRegister(
            String activity, String dateMode, String fromDate, String toDate,
            List<String> entryTypes, int jobOrderId, int parentCategoryId, int warehouseId,
            int itemId, int plantId, int wipAccountId, int stockAccountId,
            List<Integer> branchIds) {

        UserAccount u = currentUserContext.requireAccountingUser();

        String act = activity == null ? "" : activity.trim();
        if (!REGISTER_ACTIVITIES.contains(act)) {
            throw new IllegalArgumentException("Select an activity type");
        }

        /* Gridfill:668 — with no branch chosen the desktop focuses the branch box and throws
           before any query runs. The same message, and the same refusal to query. */
        String csv = branchIdsCsv(u, branchIds);
        if (csv.isEmpty()) throw new IllegalArgumentException("Select Branch First");

        /* The ticked keys, comma-joined with a leading comma, as Gridfill:511 builds them. Any
           key that is not one of the form's own three is dropped. */
        StringBuilder et = new StringBuilder();
        if (entryTypes != null) {
            for (String k : entryTypes) {
                for (String[] e : REGISTER_ENTRY_TYPES) {
                    if (e[0].equals(k)) { et.append(',').append(e[0]); break; }
                }
            }
        }

        boolean entryMode = "entry".equalsIgnoreCase(dateMode);
        String docFrom   = entryMode ? null : fromDate;
        String docTo     = entryMode ? null : toDate;
        String entryFrom = entryMode ? fromDate : null;
        String entryTo   = entryMode ? toDate   : null;

        List<Map<String, Object>> rows = repo.productionRegister(
                u, act, jobOrderId, docFrom, docTo, entryFrom, entryTo, plantId, itemId,
                stockAccountId, warehouseId, parentCategoryId, wipAccountId,
                et.toString(), csv);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("activity", act);

        if ("Production Register".equals(act)) {
            out.put("rows", projectRegister(rows));
        } else if ("OutPut By Packing Material".equals(act)) {
            out.put("rows", project(rows, new String[][] {
                    { "JobOrder",    "JobOrder" },
                    { "Item",        "ItemName" },
                    { "UOM",         "PackUom" },
                    { "PmItem",      "PmItem" },
                    { "BrandQty",    "BrandQty" },
                    { "BrandWeight", "BrandWeight" },
                    { "PmQty",       "PmQty" },
            }, null));
        } else {
            List<Map<String, Object>> summary = project(rows, new String[][] {
                    { "EntryType",  "EntryType" },
                    { "JobOrderId", "InvJobOrderId" },
                    { "JobOrder",   "JobOrder" },
                    { "PlantId",    "PlantId" },
                    { "Plant",      "PlantName" },
                    { "ItemId",     "ItemId" },
                    { "ItemName",   "ItemName" },
                    { "Qty",        "Qty" },
                    { "Weight",     "Weight" },
            }, null);
            out.put("rows", summary);
            out.put("totals", summaryTotals(summary));
        }
        return out;
    }

    /**
     * Gridfill:597 — 33 columns, and eleven of them are renamed on the way in. Two details that
     * cannot be read off the column names:
     *
     *   DocNo     comes from DocCode
     *   RatePerKg is not a column at all — it is {@code Amount / Weight}, computed here
     *
     * The division is reproduced as the desktop writes it, with no guard added: a zero weight
     * gives the same non-finite result a C# double division gives, and inventing a 0 or a 1 in
     * its place would quietly change a figure an operator reads as real.
     */
    private static List<Map<String, Object>> projectRegister(List<Map<String, Object>> rows) {
        String[][] mapping = {
                { "InvFoodProductionId", "InvFoodProductionId" },
                { "BranchName",          "BranchName" },
                { "DocDate",             "DocDate" },
                { "DocNo",               "DocCode" },
                { "JobOrder",            "JobOrder" },
                { "ReUseJobOrder",       "ReUseJobOrder" },
                { "PlantName",           "PlantName" },
                { "RefDocType",          "RefDocType" },
                { "RefDocNo",            "RefDocNo" },
                { "RefDocDate",          "RefDocDate" },
                { "GrnNo",               "GrnNo" },
                { "GrnDate",             "GrnDate" },
                { "VehicleNo",           "VehicleNo" },
                { "BiltyNo",             "BiltyNo" },
                { "SupplierOtherRef",    "SupplierOtherRef" },
                { "Warehouse",           "WareHouseName" },
                { "Item",                "ItemName" },
                { "CropYear",            "CropBatch" },
                { "JobLot",              "JobLotDescription" },
                { "PackingType",         "PackTypeDesc" },
                { "UOM",                 "PackUom" },
                { "ItemQty",             "Qty" },
                { "Weight",              "Weight" },
                { "Rate",                "Rate" },
                { "Amount",              "Amount" },
                { "EntryType",           "EntryType" },
                { "WipAccount",          "WipAccount" },
                { "EntryDate",           "EntryDate" },
                { "EntryUser",           "EntryUser" },
                { "ModifyDate",          "ModifyDate" },
                { "ModifyUser",          "ModifyUser" },
                { "MainRemarks",         "MainRemarks" },
        };
        List<Map<String, Object>> out = new ArrayList<>();
        if (rows == null) return out;
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (String[] m : mapping) {
                o.put(m[0], ci(r, m[1]));
                /* The desktop's column order puts RatePerKg between Rate and Amount. */
                if ("Rate".equals(m[0])) {
                    o.put("RatePerKg", toDouble(ci(r, "Amount")) / toDouble(ci(r, "Weight")));
                }
            }
            out.add(o);
        }
        return out;
    }

    /**
     * Gridfill:815 — the six boxes above the Production_Summary grid. Issuance, by-product and
     * finished goods are each a Sum(Weight) filtered on the EntryType CAPTION, not the key.
     */
    private static Map<String, Object> summaryTotals(List<Map<String, Object>> rows) {
        double issuance = 0, byProduct = 0, finishGoods = 0;
        for (Map<String, Object> r : rows) {
            Object t = r.get("EntryType");
            String s = t == null ? "" : t.toString().trim();
            double w = toDouble(r.get("Weight"));
            if ("Material Input".equals(s))            issuance += w;
            else if ("OutPut By Product".equals(s))    byProduct += w;
            else if ("OutPut Finish Goods".equals(s))  finishGoods += w;
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("totalIssuance", issuance);
        out.put("totalByProduct", byProduct);
        out.put("totalFinishGoods", finishGoods);
        out.put("totalInput", issuance);
        out.put("totalOutPut", byProduct + finishGoods);
        out.put("totalDifference", issuance - (byProduct + finishGoods));
        return out;
    }

    private static double toDouble(Object v) {
        if (v == null) return 0d;
        if (v instanceof Number) return ((Number) v).doubleValue();
        try { return Double.parseDouble(v.toString().trim()); }
        catch (Exception e) { return 0d; }
    }

    // ============================================================ 308 — Production Comparison

    /** base.Name, and the ScreenName dbo.ScreenDefinition 308 carries. */
    private static final String SCREEN_PRODUCTION_COMPARISON = "FoodProductionComparisonRpt";

    /**
     * ComboBindComparison:282 — the Comparison tab's two pickers, rebuilt whenever the branch
     * tick-list changes (cmbBranchNameComparison_Leave:483).
     */
    public Map<String, Object> comparisonLookups(List<Integer> branchIds) {
        UserAccount u = currentUserContext.requireAccountingUser();
        List<Map<String, Object>> src = repo.productionDropdownSource(
                u, financialYearId(), dropdownBranches(u, branchIds));

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("jobOrders", bucket(src, "JobOrder"));
        out.put("plants",    bucket(src, "Plant"));
        return out;
    }

    /**
     * ComboBindSummary:362 — the Summary tab's pickers.
     *
     * CORRECTED 2026-09-24: an earlier note here claimed the splitting loop never matches
     * "JobOrder". It does - resolved-source :385-388 adds Activity == "JobOrder" rows to dataTable,
     * which :398 binds to cmbJobOrderSummary (confirmed in the compiled IL too). The Summary tab's
     * Job Order picker is therefore filled, with a blank first row, like the other two.
     */
    public Map<String, Object> comparisonSummaryLookups(List<Integer> branchIds) {
        UserAccount u = currentUserContext.requireAccountingUser();
        List<Map<String, Object>> src = repo.productionDropdownSource(
                u, financialYearId(), dropdownBranches(u, branchIds));

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("jobOrders", bucket(src, "JobOrder"));
        out.put("plants",    bucket(src, "Plant"));
        out.put("items",     bucket(src, "ItemName"));
        return out;
    }

    /** Shared by both tabs: the branch tick-list, the production types and the rate right. */
    public Map<String, Object> comparisonFormLookups() {
        UserAccount u = currentUserContext.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("branches", repo.productionBranchesForUser(u));
        out.put("defaultBranchId", u.getBranchesId());
        out.put("productionTypes", repo.productionTypes());
        out.put("canSeeRateAndAmount", hasRight(SCREEN_PRODUCTION_COMPARISON, RIGHT_RATE));
        /* frmGPOutward_Load:216 — the From date opens at the active financial year's start. */
        out.put("financialYearStart", financialYearStart());
        return out;
    }

    /** CmbItemName_Leave:465 — the pack UOM list cascades off the chosen item. */
    public List<Map<String, Object>> comparisonPackUoms(int itemId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        if (itemId <= 0) return new ArrayList<>();
        return repo.uomScheduleByItem(u, itemId);
    }

    /** gridHisory:637 — the Comparison tab's only grid. */
    public List<Map<String, Object>> productionComparison(String productionType, int docNoFrom,
                                                          int docNoTo, int plantId,
                                                          int jobOrderId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        if (productionType == null || productionType.trim().isEmpty()) {
            throw new IllegalArgumentException("Select a production type");
        }
        return project(repo.productionComparison(u, productionType.trim(), docNoFrom, docNoTo,
                                                 plantId, jobOrderId),
                new String[][] {
                        { "Id",             "Id" },
                        { "PlanCode",       "PlanCode" },
                        { "PlanDate",       "PlanDate" },
                        { "JobOrderNo",     "JobOrderNo" },
                        { "PlanType",       "PlanType" },
                        { "Plant",          "Plant" },
                        { "Input",          "Input" },
                        { "BP_Output",      "BP_Output" },
                        { "FG_Output",      "FG_Output" },
                        { "Short_Gain",     "Short_Gain" },
                        { "BP_Recovery",    "BP_Recovery" },
                        { "FG_Recovery",    "FG_Recovery" },
                        { "Total_Recovery", "Total_Recovery" },
                }, null);
    }

    /**
     * btnShowInput_Click:801 — the Summary tab's radio picks one of two reports into the same
     * grid, so one endpoint serves both and says which shape it returned.
     *
     * @param mode "detail" (RdInputDetail, the default) or "gainloss".
     */
    public Map<String, Object> comparisonSummary(String mode, int jobOrderId, int plantId,
                                                 int itemId, int packUomId, String fromDate,
                                                 String toDate) {
        UserAccount u = currentUserContext.requireAccountingUser();
        boolean gainLoss = "gainloss".equalsIgnoreCase(mode);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("mode", gainLoss ? "gainloss" : "detail");

        if (!gainLoss) {
            /* grdInputSetting:1057 — ItemAmount and ItemNetAmount are hidden unless the screen's
               "Rate" right is granted, so they are withheld rather than merely not drawn. */
            boolean rate = hasRight(SCREEN_PRODUCTION_COMPARISON, RIGHT_RATE);
            out.put("canSeeRateAndAmount", rate);
            out.put("rows", project(
                    repo.productionIssuanceGrnWise(u, jobOrderId, plantId, itemId, packUomId,
                                                   fromDate, toDate),
                    new String[][] {
                            { "OrderNo",              "OrderNo" },
                            { "GpNo",                 "GpNo" },
                            { "GpDate",               "GpDate" },
                            { "GrnNo",                "GrnNo" },
                            { "GrnDate",              "GrnDate" },
                            { "InvoiceNo",            "InvoiceNo" },
                            { "JobOrderNo",           "JobOrderNo" },
                            /* The desktop's own spelling in the result set. */
                            { "ProductionInstruction", "ProductionInsturction" },
                            { "SupplierName",         "SupplierName" },
                            { "ItemName",             "ItemName" },
                            { "VehicleNo",            "VehicleNo" },
                            { "BiltyNo",              "BiltyNo" },
                            { "CropYear",             "CropYear" },
                            { "PackingType",          "PackTypeDesc" },
                            { "PackUom",              "UOMCode" },
                            { "IssueQty",             "IssueQty" },
                            { "IssueWeight",          "IssueWeight" },
                            { "ItemQty",              "ItemQty" },
                            { "GrossWeight",          "GrossWeight" },
                            { "EBWPerUnit",           "EBWPerUnit" },
                            { "EBWTotal",             "EBWTotal" },
                            { "AdLsWeight",           "AdLsWeight" },
                            { "StockWeight",          "StockWeight" },
                            { "NetBillWeight",        "NetBillWeight" },
                            { "ItemAmount",           "ItemAmount" },
                            { "ItemNetAmount",        "ItemNetAmount" },
                            { "Transporter",          "TransporterName" },
                            { "CarriageAmount",       "CarriageAmount" },
                            { "CityName",             "AreaCity" },
                    },
                    rate ? null : new String[] { "ItemAmount", "ItemNetAmount" }));
            return out;
        }

        List<Map<String, Object>> rows = project(
                repo.productionGainLossSummary(u, jobOrderId, plantId, itemId, packUomId,
                                               fromDate, toDate),
                new String[][] {
                        { "JobOrderNo",       "JobOrderNo" },
                        { "EntryType",        "TypeDescription" },
                        { "ItemCode",         "ItemCode" },
                        { "ItemName",         "ItemName" },
                        { "PackUom",          "PackSize" },
                        { "IssueQty",         "IssueQty" },
                        { "IssueWeight",      "IssueWeight" },
                        { "ProductionQty",    "ProductionQty" },
                        { "ProductionWeight", "ProductionWeight" },
                }, null);
        out.put("rows", rows);
        out.put("totals", gainLossTotals(rows));
        return out;
    }

    /**
     * Production_GainLossSummary:926 — the panel under the gain/loss grid.
     *
     * {@code Diff = Sum(IssueWeight) - Sum(ProductionWeight)}, and its SIGN decides the caption:
     * positive is "Loss" in red, negative is "Gain" in green, zero is "Nill" in black. The
     * percentage is {@code |Diff / IssueWeight * 100|} rounded to 2 — computed only when Diff is
     * not zero, exactly as the desktop guards it.
     */
    private static Map<String, Object> gainLossTotals(List<Map<String, Object>> rows) {
        double issue = 0, prod = 0;
        for (Map<String, Object> r : rows) {
            issue += toDouble(r.get("IssueWeight"));
            prod  += toDouble(r.get("ProductionWeight"));
        }
        double diff = issue - prod;
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("totalIssueWeight", issue);
        out.put("totalProductionWeight", prod);
        out.put("difference", diff);
        out.put("status", diff > 0 ? "Loss" : (diff < 0 ? "Gain" : "Nill"));
        if (diff != 0) {
            double pct = diff / issue * 100d;
            out.put("percent", Math.round(Math.abs(pct) * 100d) / 100d);
        }
        return out;
    }

    /** One bucket out of the shared dropdown source, keyed on its Activity discriminator. */
    private static List<Map<String, Object>> bucket(List<Map<String, Object>> src, String key) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (src == null) return out;
        for (Map<String, Object> r : src) {
            Object a = ci(r, "Activity");
            if (a == null || !key.equals(a.toString().trim())) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(r, "Id"));
            o.put("Name", ci(r, "ReferenceName"));
            out.add(o);
        }
        return out;
    }

    /**
     * The same grant-grid read canSeeRateAndAmount does, taking the screen and right by name so
     * a second screen does not need a second copy. No Admin short-circuit: the desktop grants
     * Admin a blanket Save/Update/Delete/Print, but not "Rate".
     */
    private boolean hasRight(String desktopScreenName, String rightName) {
        String role = currentRoleName();
        try {
            List<Map<String, Object>> rights = jdbcTemplate.queryForList(
                    SQL_USER_RIGHTS_FOR_SCREEN,
                    currentUserContext.currentUserId(),
                    desktopScreenName,
                    role == null ? "" : role,
                    currentUserContext.currentCompanyId(),
                    "GetByUserId");
            for (Map<String, Object> r : rights) {
                Object name = ci(r, "RightName");
                if (name != null && rightName.equalsIgnoreCase(name.toString().trim())) {
                    return toBool(ci(r, "Value"));
                }
            }
        } catch (Exception e) {
            LOG.warn("Could not read the {} right for {}; denying", rightName, desktopScreenName, e);
        }
        return false;
    }

    /**
     * frmGPOutward_Load:216 sets the Summary tab's From date to
     * {@code clsGlobalVariables.ActiveYr.Start_Period}.
     *
     * The active year comes from the same procedure CurrentUserContext already uses —
     * Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId — rather than a hand-written
     * SELECT. If the row does not carry Start_Period, null comes back and the page simply leaves
     * the date unset instead of inventing one.
     */
    // ============================================ 306 — Production PackingMaterial Consumption

    /** ComboBind:144 — the two item pickers, plus the branch tick-list and the year's start. */
    public Map<String, Object> packingMaterialLookups(List<Integer> branchIds) {
        UserAccount u = currentUserContext.requireAccountingUser();
        List<Map<String, Object>> src = repo.packingMaterialDropdownSource(
                u, financialYearId(), dropdownBranches(u, branchIds));

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("items",   bucket(src, "BrandItem"));
        out.put("pmItems", bucket(src, "PMItem"));
        out.put("branches", repo.productionBranchesForUser(u));
        out.put("defaultBranchId", u.getBranchesId());
        /* frmProductionPendingPurchaseInvoice_Load:105 — From opens at the year's start. */
        out.put("financialYearStart", financialYearStart());
        return out;
    }

    /**
     * GridBind:272. Branch is mandatory here as it is on 310: with nothing ticked the desktop
     * focuses the branch box and throws before any query runs.
     *
     * The rows come back unprojected, because the desktop binds the result set directly.
     */
    public List<Map<String, Object>> packingMaterialConsumption(String fromDate, String toDate,
                                                                int itemId, int pmItemId,
                                                                List<Integer> branchIds) {
        UserAccount u = currentUserContext.requireAccountingUser();
        String csv = branchIdsCsv(u, branchIds);
        if (csv.isEmpty()) throw new IllegalArgumentException("Select Branch First");
        /* GridBind:262-263 always sends both dates, and the procedure compares DocDate against
           them without allowing for NULL - an omitted date returns zero rows, silently. */
        if (fromDate == null || fromDate.trim().isEmpty() || toDate == null || toDate.trim().isEmpty()) {
            throw new IllegalArgumentException("From Date and To Date are required");
        }
        return repo.packingMaterialConsumptionRegister(u, fromDate, toDate, itemId, pmItemId, csv);
    }

    private String financialYearStart() {
        UserAccount u = currentUserContext.requireAccountingUser();
        int yearId = financialYearId();
        try {
            List<Map<String, Object>> years = jdbcTemplate.queryForList(
                    "EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId "
                    + "@OrganizationId=?, @CompanyId=?",
                    u.getOrganizationId(), u.getCompanyId());
            Map<String, Object> row = null;
            for (Map<String, Object> r : years) {
                Object id = ci(r, "Id");
                if (id instanceof Number && ((Number) id).intValue() == yearId) { row = r; break; }
            }
            if (row == null && !years.isEmpty()) row = years.get(0);
            if (row != null) {
                Object v = ci(row, "Start_Period");
                return v == null ? null : v.toString();
            }
        } catch (Exception e) {
            LOG.warn("Could not read the active financial year's start period", e);
        }
        return null;
    }
}
