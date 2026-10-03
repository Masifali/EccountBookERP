package com.mst.services.lab;

import com.mst.models.UserAccount;
import com.mst.repositories.PurchaseReportsRepository;
import com.mst.repositories.lab.LabPurchaseReportsRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import com.mst.services.StoreScreenRights;
import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.Set;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

/**
 * Lab Report (module 1011) - the three purchase-analysis report screens, ported from the desktop forms:
 *
 *   626 ScreenName LabDataVehicleWiseByParent   Architecture.WinApp.Lab/LabDataVehicleWiseByParent.cs   ("V:nnn")
 *   633 ScreenName InvLabPurchaseRegister       Architecture.WinApp.Lab/InvLabPurchaseRegister.cs       ("R:nnn")
 *   630 ScreenName LabPurchaseAnalyticPeriodic  Architecture.WinApp.Lab/LabPurchaseAnalyticPeriodic.cs  ("P:nnn")
 *       + the dialogs it opens: LabPurchaseAnalyticPeriodicItemWise.cs ("I:nnn") and
 *         LabPurchaseAnalyticPeriodicPartyAndItemWiseWithoutCards.cs ("W:nnn"); card = DynamicCards/LabPurchaseAnalyticCard.cs
 *
 * All three are read-only (no Save / Update / Delete on the desktop, none here). Organization and company always
 * come from the session. None of the forms checks a right of its own (no SetRightsValueInRightsObject call);
 * the desktop only lets a user open them through the menu, i.e. with the screen's View grant - that grant is what
 * every call here requires.
 */
@Service
public class LabPurchaseReportsService {

    /** seed_screendef.txt: 626 (module 1011) and 934 (module 55) both carry ScreenName LabDataVehicleWiseByParent. */
    private static final int[] VEHICLE_SCREENS = {626, 934};
    private static final int[] REGISTER_SCREENS = {633};
    private static final int[] PERIODIC_SCREENS = {630};
    private static final DateTimeFormatter STAMP = DateTimeFormatter.ofPattern("yyyy-MM-dd'T'HH:mm:ss");

    private final LabPurchaseReportsRepository repo;
    private final PurchaseReportsRepository common;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;
    private final StoreScreenRights screenRights;

    public LabPurchaseReportsService(LabPurchaseReportsRepository repo, PurchaseReportsRepository common, CurrentUserContext context,
                                     DesktopReportRights rights, StoreScreenRights screenRights) {
        this.repo = repo; this.common = common; this.context = context; this.rights = rights; this.screenRights = screenRights;
    }

    /** The signed-in user, holding the View grant of (one of) the screen's ScreenDefinition rows. */
    private UserAccount user(int[] screens) {
        UserAccount u = context.requireAccountingUser();
        AccessDeniedException last = null;
        for (int s : screens) {
            try { rights.require(u, s, "View"); return u; } catch (AccessDeniedException e) { last = e; }
        }
        throw last != null ? last : new AccessDeniedException("User session required");
    }

    // =====================================================================================================
    // 626 LabDataVehicleWiseByParent - "Purchase Analysis By Vehicle"
    // =====================================================================================================

    /** LabDataVehicleWiseByParent_Load V:144 - datFromDate = ActiveYr.Start_Period, ParentCategoryFill. */
    public Map<String, Object> vehicleInit() {
        UserAccount u = user(VEHICLE_SCREENS);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("yearStart", common.yearStart(u, context.currentFinancialYearId()));      // V:155
        out.put("now", STAMP.format(LocalDateTime.now()));                                 // datToDate keeps DateTime.Now
        // ParentCategoryFill V:175-208: Activity "ParentCategories", DocumentTypeIds "303"; Id / ReferenceName.
        out.put("parentCategories", idNames(repo.dropDownData(context.currentOrganizationId(), context.currentCompanyId(), "ParentCategories", "303", "", 0), null));
        // grdLabDetail_LinkClicked V:800-826: the OrderNo link opens ActivityDetails for RoleName "Admin", otherwise
        // only with the View right of screen "ActivityDetails".
        out.put("admin", "Admin".equals(context.currentRoleName()));
        out.put("activityDetailsView", screenRights.has("ActivityDetails", "view"));
        return out;
    }

    /**
     * HistoryComboFill V:210-260 (CmbParentCategory_Leave): Activity "" (not sent), DocumentTypeIds "303",
     * ParentId = the parent category (sent when non-zero), BranchesIds = UserAccount.BranchesId.ToString().
     * Rows with Activity "Supplier" fill CmbSupplier, "Item" fill CmbItemName.
     */
    public Map<String, Object> vehicleLookups(int parentCategoryId) {
        UserAccount u = user(VEHICLE_SCREENS);
        List<Map<String, Object>> dt = repo.dropDownData(context.currentOrganizationId(), context.currentCompanyId(), "", "303",
                String.valueOf(num(u.getBranchesId())), parentCategoryId);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("suppliers", idNames(dt, "Supplier"));
        out.put("items", idNames(dt, "Item"));
        return out;
    }

    /** dtLabDetail of gridHistory V:355-381, in the desktop's column order: {name, type}. */
    private static final String[][] VEHICLE_DETAIL = {
            {"Id", "int"}, {"GatePassInwardId", "int"}, {"PurchaseType", "string"}, {"LabDate", "date"}, {"LabNo", "int"},
            {"OrderId", "int"}, {"OrderNo", "string"}, {"SupplierCustomerId", "int"}, {"Supplier", "string"}, {"VehicleNo", "string"},
            {"ItemId", "int"}, {"ItemName", "string"}, {"AnalysisQty", "double"}, {"NetWeight", "double"}, {"QtyForWtCut", "double"},
            {"WeightCut", "double"}, {"ItemRate", "double"}, {"RateCut", "double"}, {"NetRate", "double"}, {"Moisture", "double"},
            {"Empty Shell / Trash", "double"}, {"Dust/Stone", "double"}, {"Broken", "double"}, {"AGL", "double"}, {"Damage", "double"},
            {"Chooba", "double"}};

    /** dtSummary of gridHistory V:395-413: {grid column, procedure column, type}. */
    private static final String[][] VEHICLE_SUMMARY = {
            {"ItemId", "ItemId", "int"}, {"ItemName", "ItemName", "string"}, {"TotalAnalysisQty", "TotalAnalysisQty", "double"},
            {"TotalNetWeight", "TotalNetWeight", "double"}, {"AvgBroken", "AvgBroken", "double"}, {"AvgMoisture", "AvgMoisture", "double"},
            {"WeightedAvgBroken", "WeightedAvgBroken", "double"}, {"WeightedAvgMoisture", "WeightedAvgMoisture", "double"},
            {"MinMoisture", "MinMoisture", "double"}, {"MaxMoisture", "MaxMoisture", "double"}, {"MinBroken", "MinBroken", "double"},
            {"MaxBroken", "MaxBroken", "double"}, {"Slab1", "WeightedAvgSlab1", "double"}, {"Slab2", "WeightedAvgSlab2", "double"},
            {"Slab3", "WeightedAvgSlab3", "double"}};

    /** btnshow_Click V:299 + gridHistory V:318-433. */
    public Map<String, Object> vehicleRows(Map<String, Object> b) {
        UserAccount u = user(VEHICLE_SCREENS);
        int parentCategoryId = toInt(b.get("parentCategoryId"));
        // V:303 - CmbParentCategory.ActiveRow == null
        if (parentCategoryId == 0) throw new IllegalArgumentException("Please Select Parent Category First...");
        String orderNo = str(b.get("orderNo")).trim();                                   // V:331
        // V:340-347: every number box goes through Conversion.ToInt (a non-integer text is 0, i.e. not sent).
        List<List<Map<String, Object>>> ds = repo.labDataVehicleWiseByParent(context.currentOrganizationId(), context.currentCompanyId(), parentCategoryId,
                stamp(b.get("fromDate")), stamp(b.get("toDate")),                        // V:332-339 only while the picker is checked
                toInt(b.get("itemId")), toInt(b.get("supplierId")),
                toInt(b.get("moistureTo")), toInt(b.get("moistureFrom")),
                orderNo, toInt(orderNo) != 0,
                toInt(b.get("slab1From")), toInt(b.get("slab2From")), toInt(b.get("slab3From")),
                toInt(b.get("slab1To")), toInt(b.get("slab2To")), toInt(b.get("slab3To")));
        List<Map<String, Object>> table00 = ds.size() > 0 ? ds.get(0) : new ArrayList<>();     // V:349-352
        List<Map<String, Object>> table01 = ds.size() > 1 ? ds.get(1) : new ArrayList<>();

        List<Map<String, Object>> detail = new ArrayList<>();
        for (Map<String, Object> r : table00) {
            Map<String, Object> row = new LinkedHashMap<>();
            for (String[] c : VEHICLE_DETAIL) row.put(c[0], convert(column(r, c[0]), c[1]));
            detail.add(row);
        }
        List<Map<String, Object>> summary = new ArrayList<>();
        for (Map<String, Object> r : table01) {
            Map<String, Object> row = new LinkedHashMap<>();
            for (String[] c : VEHICLE_SUMMARY) row.put(c[0], convert(column(r, c[1]), c[2]));
            summary.add(row);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("detail", detail);
        out.put("summary", summary);
        // DS_Table00 / DS_Table01 as the procedure returned them - what Print_Click hands to the 665 report (V:766-777).
        out.put("rawDetail", table00);
        out.put("rawSummary", table01);
        return out;
    }

    // =====================================================================================================
    // 633 InvLabPurchaseRegister - "Purchase Analylsis Report"
    // =====================================================================================================

    /** VoucherValidation_Load R:72 - txtdatef = ActiveYr.Start_Period, AllComboBinds, StatusBind. */
    public Map<String, Object> registerInit() {
        UserAccount u = user(REGISTER_SCREENS);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("yearStart", common.yearStart(u, context.currentFinancialYearId()));      // R:77
        out.put("now", STAMP.format(LocalDateTime.now()));
        out.put("lookups", registerLookups(u));
        return out;
    }

    public Map<String, Object> registerLookups() { return registerLookups(user(REGISTER_SCREENS)); }

    /**
     * AllComboBinds R:112-165: Activity "" and DocumentTypeIds "" - neither is sent, only organization and company.
     * "bound" is false when the procedure returned no row: the desktop then returns before binding (R:123-126).
     */
    private Map<String, Object> registerLookups(UserAccount u) {
        List<Map<String, Object>> dt = repo.dropDownData(context.currentOrganizationId(), context.currentCompanyId(), "", "", "", 0);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("bound", !dt.isEmpty());
        out.put("suppliers", idNames(dt, "Supplier"));
        out.put("parentCategories", idNames(dt, "ParentCategories"));
        out.put("items", idNames(dt, "Item"));
        out.put("orders", idNames(dt, "PurchaseOrderNo"));
        return out;
    }

    /** The fifteen fixed columns of dtG (R:309-323): {grid column, procedure column}. */
    private static final String[][] REGISTER_FIXED = {
            {"Id", "Id"}, {"DocDate", "DocDate"}, {"DocNo", "DocNo"}, {"ItemId", "ItemId"}, {"ItemName", "ItemName"},
            {"GatePassInwardId", "GatePassInwardId"}, {"GpNo", "GpSrNo"}, {"VehicleNo", "VehicleNo"}, {"CropYear", "CropYear"},
            {"PartyName", "PartyName"}, {"OrderNo", "OrderNo"}, {"ItemQty", "ItemQty"}, {"PartyLot", "PartyLotRefNo"},
            {"WareHouseName", "WareHouseName"}, {"Status", "Status"}};

    /**
     * BindGrid R:268-376. The analysis-parameter columns are built exactly as the form builds them, by ORDINAL:
     * a column is added for every non-empty cell of the first row from procedure column 46 on (R:324-330), and the
     * j-th added column is filled from procedure column 31 + j (R:366).
     */
    public Map<String, Object> registerRows(Map<String, Object> b) {
        UserAccount u = user(REGISTER_SCREENS);
        Timestamp from = stamp(b.get("fromDate")), to = stamp(b.get("toDate"));
        // txtdatef / txtdatet have no check box: FromDate and ToDate are always sent (R:276-277).
        if (from == null) throw new IllegalArgumentException("Date From is required");
        if (to == null) throw new IllegalArgumentException("Date To is required");
        // R:280-287: cmbstatus.Text == "" -> ApprovedFilter "All" (no @IsAccepted); else Conversion.ToBool(cmbstatus.Value).
        String status = str(b.get("status")).trim();
        Boolean isAccepted = status.isEmpty() ? null : Boolean.valueOf(toBool(status));
        List<Map<String, Object>> dt = repo.labPurchaseAnalysisRegister(context.currentOrganizationId(), context.currentCompanyId(), from, to,
                toInt(b.get("gpNoFrom")), toInt(b.get("gpNoTo")), toInt(b.get("itemId")), toInt(b.get("supplierId")),
                toInt(b.get("orderId")), isAccepted, toInt(b.get("parentCategoryId")));

        Map<String, Object> out = new LinkedHashMap<>();
        List<String> dynamic = new ArrayList<>();
        List<Map<String, Object>> rows = new ArrayList<>();
        if (!dt.isEmpty()) {
            List<String> columnNames = new ArrayList<>(dt.get(0).keySet());
            int lengthOfColumns = columnNames.size() - 46;                                 // R:307
            Set<String> used = new HashSet<>();
            for (String[] c : REGISTER_FIXED) used.add(c[0].toLowerCase(Locale.ROOT));
            for (int i = 0; i < lengthOfColumns; i++) {                                    // R:324-330
                String name = text(dt.get(0).get(columnNames.get(i + 46)));
                if (name.isEmpty()) continue;
                if (!used.add(name.toLowerCase(Locale.ROOT)))
                    throw new IllegalArgumentException("A column named '" + name + "' already belongs to this DataTable.");
                dynamic.add(name);
            }
            for (Map<String, Object> r : dt) {                                             // R:333-368
                Map<String, Object> row = new LinkedHashMap<>();
                for (String[] c : REGISTER_FIXED) {
                    Object v = column(r, c[1]);
                    // R:337 Conversion.ToDateTime(DocDate).ToShortDateString() - the date without its time
                    row.put(c[0], "DocDate".equals(c[0]) ? dateOnly(v) : v);
                }
                List<Object> cells = new ArrayList<>(r.values());
                for (int j = 0; j < dynamic.size(); j++) {
                    if (31 + j >= cells.size()) throw new IllegalArgumentException("Cannot find column " + (31 + j) + ".");
                    Object v = cells.get(31 + j);
                    row.put(dynamic.get(j), v == null ? null : (Object) dbl(v));           // typeof(double) columns
                }
                rows.add(row);
            }
        }
        out.put("dynamicColumns", dynamic);
        out.put("rows", rows);
        // dtGroupAnalysis - what print_Click_1 hands to 661-LabPurchaseAnalysisRegitser.rpt (R:88-97).
        out.put("raw", dt);
        return out;
    }

    // =====================================================================================================
    // 630 LabPurchaseAnalyticPeriodic - "Lab Purchase Analysis Periodic Report"
    // =====================================================================================================

    /** PurchaseAnalyticPeriodic_Load P:53 - DateFrom = Now.AddDays(-30), DateTo = Now, GetSeasonScheduleDates. */
    public Map<String, Object> periodicInit() {
        UserAccount u = user(PERIODIC_SCREENS);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("fromDate", LocalDate.now().minusDays(30).toString());                     // P:60
        out.put("toDate", LocalDate.now().toString());
        out.put("season", season(u));
        // clsGlobalVariables.DecimalRateFormate (AvgRate of the drill-down grids) - "Default NoofDecimal Points For Rate",
        // read the way PurchaseReportService.init does.
        int rate = toInt(common.configuration(u, "Default NoofDecimal Points For Rate").trim());
        out.put("rateDecimals", rate >= 1 && rate <= 4 ? rate : rate == 0 ? 2 : 0);
        return out;
    }

    /** GetSeasonScheduleDates P:67-98 (re-read after the Season Year Schedule dialog closes, P:204). */
    public Map<String, Object> periodicSeason() { return season(user(PERIODIC_SCREENS)); }

    private Map<String, Object> season(UserAccount u) {
        List<Map<String, Object>> dt = repo.seasonYearSchedule(context.currentOrganizationId(), context.currentCompanyId());
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("found", !dt.isEmpty());                                                   // isScheduleFound
        if (!dt.isEmpty()) {
            out.put("seasonStartDate", dateOnly(column(dt.get(0), "SeasonStartDate")));    // P:79
            out.put("seasonEndDate", dateOnly(column(dt.get(0), "SeasonEndDate")));        // P:80
        } else {
            out.put("message", "No Schedule found. Please Make a season year schedule first!");   // P:87
        }
        return out;
    }

    /** The season bounds the form holds in its two disabled pickers; null when there is no schedule (Show does nothing, P:112). */
    private java.sql.Date[] seasonDates(UserAccount u) {
        List<Map<String, Object>> dt = repo.seasonYearSchedule(context.currentOrganizationId(), context.currentCompanyId());
        if (dt.isEmpty()) return null;
        return new java.sql.Date[]{date(dateOnly(column(dt.get(0), "SeasonStartDate")), "Season Start Date"),
                date(dateOnly(column(dt.get(0), "SeasonEndDate")), "Season End Date")};
    }

    private static final String[] CARD_VALUES = {"NoofVehicles", "TotalQty", "Moisture", "GreenGrain", "EmptyShell", "Damage",
            "Sherivalled", "Fungus", "AGL", "Broken"};

    /**
     * btnshow_Click P:106-165: usp_LabPurchaseAnalyticsA, one LabPurchaseAnalyticCard per distinct ParentId (first-seen
     * order), captioned with that group's ParentCategory; card table = Description + the ten value columns.
     */
    public Map<String, Object> periodicCards(Map<String, Object> b) {
        UserAccount u = user(PERIODIC_SCREENS);
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> cards = new ArrayList<>();
        out.put("cards", cards);
        java.sql.Date[] season = seasonDates(u);
        out.put("scheduleFound", season != null);
        if (season == null) return out;                                                    // P:112 if (!isScheduleFound) return;
        List<Map<String, Object>> dt = repo.labPurchaseAnalyticsA(context.currentOrganizationId(), context.currentCompanyId(),
                date(b.get("fromDate"), "From Date"), date(b.get("toDate"), "To Date"), season[0], season[1]);
        Map<Integer, Map<String, Object>> byParent = new LinkedHashMap<>();
        for (Map<String, Object> r : dt) {
            int parentId = toInt(column(r, "ParentId"));
            Map<String, Object> card = byParent.get(parentId);
            if (card == null) {
                card = new LinkedHashMap<>();
                card.put("id", parentId);                                                  // P:136
                card.put("caption", text(column(r, "ParentCategory")));                    // P:135
                card.put("rows", new ArrayList<Map<String, Object>>());
                byParent.put(parentId, card);
                cards.add(card);
            }
            Map<String, Object> row = new LinkedHashMap<>();
            row.put("Description", column(r, "Description"));
            for (String k : CARD_VALUES) row.put(k, numeric(column(r, k)));
            rowsOf(card).add(row);
        }
        return out;
    }

    /**
     * Card title click (P:191-200) -> LabPurchaseAnalyticPeriodicItemWise with RequestedByOtherDocument, FromDate,
     * ToDate and ParentCategoryId; its btnshow_Click (I:113-178): usp_LabPurchaseAnalytics_ItemWise, one card per
     * distinct (ItemId, CropYear), captioned "ItemName CropYear", RequestFor "Product_Wise".
     */
    public Map<String, Object> periodicItemWise(Map<String, Object> b) {
        UserAccount u = user(PERIODIC_SCREENS);
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> cards = new ArrayList<>();
        out.put("cards", cards);
        java.sql.Date[] season = seasonDates(u);
        out.put("scheduleFound", season != null);
        if (season == null) return out;                                                    // I:117
        List<Map<String, Object>> dt = repo.labPurchaseAnalyticsItemWise(context.currentOrganizationId(), context.currentCompanyId(),
                toInt(b.get("parentCategoryId")),                                          // I:127 RequestedByOtherDocument ? ParentCategoryId : 0
                date(b.get("fromDate"), "From Date"), date(b.get("toDate"), "To Date"), season[0], season[1]);
        Map<String, Map<String, Object>> byItem = new LinkedHashMap<>();
        for (Map<String, Object> r : dt) {
            int itemId = toInt(column(r, "ItemId"));
            String cropYear = text(column(r, "CropYear"));
            String key = itemId + "\u0000" + cropYear;
            Map<String, Object> card = byItem.get(key);
            if (card == null) {
                card = new LinkedHashMap<>();
                card.put("itemId", itemId);                                                // I:147
                card.put("caption", text(column(r, "ItemName")) + " " + cropYear);         // I:146
                card.put("rows", new ArrayList<Map<String, Object>>());
                byItem.put(key, card);
                cards.add(card);
            }
            Map<String, Object> row = new LinkedHashMap<>();                               // I:152-170
            row.put("SortNo", toInt(column(r, "SortNo")));
            row.put("Description", column(r, "Description"));
            for (String k : CARD_VALUES) row.put(k, numeric(column(r, k)));
            row.put("AvgRate", numeric(column(r, "AvgRate")));
            rowsOf(card).add(row);
        }
        return out;
    }

    /**
     * LabPurchaseAnalyticPeriodicPartyAndItemWiseWithoutCards.GetData W:45-79: usp_LabPurchaseAnalyticsPartyWiseByItemId
     * with ItemId always and SortNo when non-zero. lblItemName = "( ItemName CropYear )" of the last row (W:77).
     */
    public Map<String, Object> periodicPartyWise(Map<String, Object> b) {
        UserAccount u = user(PERIODIC_SCREENS);
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> rows = new ArrayList<>();
        out.put("rows", rows);
        out.put("itemName", "");
        java.sql.Date[] season = seasonDates(u);
        if (season == null) return out;
        List<Map<String, Object>> dt = repo.labPurchaseAnalyticsPartyWiseByItemId(context.currentOrganizationId(), context.currentCompanyId(),
                date(b.get("fromDate"), "From Date"), date(b.get("toDate"), "To Date"), season[0], season[1],
                toInt(b.get("itemId")), toInt(b.get("sortNo")));
        for (Map<String, Object> r : dt) {
            out.put("itemName", "( " + text(column(r, "ItemName")) + " " + text(column(r, "CropYear")) + " )");
            Map<String, Object> row = new LinkedHashMap<>();
            row.put("Description", column(r, "Description"));
            row.put("Supplier", column(r, "SupplierName"));
            for (String k : CARD_VALUES) row.put(k, numeric(column(r, k)));
            row.put("AvgRate", numeric(column(r, "AvgRate")));
            rows.add(row);
        }
        return out;
    }

    // =====================================================================================================
    // helpers
    // =====================================================================================================

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> rowsOf(Map<String, Object> card) { return (List<Map<String, Object>>) card.get("rows"); }

    /** {Id, ReferenceName} of the rows whose Activity matches (all rows when activity is null). */
    private static List<Map<String, Object>> idNames(List<Map<String, Object>> dt, String activity) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : dt) {
            if (activity != null && !activity.equals(text(find(r, "Activity")))) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", find(r, "Id"));
            m.put("ReferenceName", find(r, "ReferenceName"));
            out.add(m);
        }
        return out;
    }

    /** DataRow["name"] - case-insensitive; null when the column is not there. */
    private static Object find(Map<String, Object> row, String name) {
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    /** DataRow["name"] - a missing column is the desktop's ArgumentException, not a silent default. */
    private static Object column(Map<String, Object> row, String name) {
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        throw new IllegalArgumentException("Column '" + name + "' does not belong to table .");
    }

    /** Conversion.ToInt / ToString / ToDouble / ToDateTime by dtLabDetail column type. */
    private static Object convert(Object v, String type) {
        switch (type) {
            case "int": return toInt(v);
            case "double": return dbl(v);
            case "date": return v == null ? "" : String.valueOf(v);
            default: return text(v);
        }
    }

    /** Conversion.ToInt: Convert.ToInt32, 0 for null / empty / anything that does not convert (e.g. "12.5" text). */
    private static int toInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Boolean) return (Boolean) v ? 1 : 0;
        if (v instanceof Number) {
            double d = ((Number) v).doubleValue();
            if (Double.isNaN(d) || d > Integer.MAX_VALUE || d < Integer.MIN_VALUE) return 0;
            return (int) Math.rint(d);                                                     // Convert.ToInt32(double): banker's rounding
        }
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return 0;
        try { return Integer.parseInt(s.startsWith("+") ? s.substring(1) : s); } catch (NumberFormatException e) { return 0; }
    }

    private static int num(Integer v) { return v == null ? 0 : v; }

    /** Conversion.ToDouble: 0 for null / empty / unparsable / infinity. */
    private static double dbl(Object v) {
        if (v == null) return 0.0;
        double d;
        if (v instanceof Number) d = ((Number) v).doubleValue();
        else if (v instanceof Boolean) d = (Boolean) v ? 1 : 0;
        else {
            String s = String.valueOf(v).trim().replace(",", "");
            if (s.isEmpty()) return 0.0;
            try { d = Double.parseDouble(s); } catch (NumberFormatException e) { return 0.0; }
        }
        return Double.isInfinite(d) || Double.isNaN(d) ? 0.0 : d;
    }

    /**
     * A procedure cell going into a typeof(double) / typeof(int) DataTable column (the analytics procedures return
     * Moisture ... Broken as NVARCHAR): the number when it is one, NULL stays empty, other text is passed through.
     */
    private static Object numeric(Object v) {
        if (v == null || v instanceof Number) return v;
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        try { return Double.parseDouble(s); } catch (NumberFormatException e) { return s; }
    }

    /** Conversion.ToBool: Convert.ToBoolean, then Convert.ToBoolean(Convert.ToInt32(value)) - "1" is true. */
    private static boolean toBool(Object v) {
        if (v == null) return false;
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).doubleValue() != 0;
        String s = String.valueOf(v).trim();
        if (s.equalsIgnoreCase("true")) return true;
        if (s.equalsIgnoreCase("false") || s.isEmpty()) return false;
        try { return Integer.parseInt(s) != 0; } catch (NumberFormatException e) { return false; }
    }

    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    /** Conversion.ToString of a cell - .NET renders 0.0 as "0" and 12.50 as "12.5". */
    private static String text(Object v) {
        if (v == null) return "";
        if (v instanceof Double || v instanceof Float) {
            double d = ((Number) v).doubleValue();
            if (Double.isNaN(d) || Double.isInfinite(d)) return String.valueOf(v);
            return BigDecimal.valueOf(d).stripTrailingZeros().toPlainString();
        }
        if (v instanceof BigDecimal) return ((BigDecimal) v).toPlainString();
        return String.valueOf(v);
    }

    /** "yyyy-MM-dd" of an ISO date / date-time text ("" when there is none). */
    private static String dateOnly(Object v) {
        String s = str(v).trim();
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }

    /** A DateTimePicker value from the page ("yyyy-MM-dd" or "yyyy-MM-ddTHH:mm:ss"); null = unchecked (not sent). */
    private static Timestamp stamp(Object v) {
        String s = str(v).trim();
        if (s.isEmpty()) return null;
        try {
            return s.length() > 10 ? Timestamp.valueOf(LocalDateTime.parse(s.replace(' ', 'T'))) : Timestamp.valueOf(LocalDate.parse(s).atStartOfDay());
        } catch (RuntimeException e) {
            throw new IllegalArgumentException("Invalid date: " + s);
        }
    }

    private static java.sql.Date date(Object v, String label) {
        String s = dateOnly(v);
        if (s.isEmpty()) throw new IllegalArgumentException(label + " is required");
        try { return java.sql.Date.valueOf(LocalDate.parse(s)); } catch (RuntimeException e) { throw new IllegalArgumentException("Invalid date: " + s); }
    }
}
