package com.mst.repositories;

import com.mst.repositories.support.DesktopProc;
import com.mst.repositories.support.ProcExec;
import com.mst.security.CurrentUserContext;
import org.springframework.jdbc.core.ConnectionCallback;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Repository;

import java.sql.CallableStatement;
import java.sql.ResultSet;
import java.sql.Timestamp;
import java.sql.Types;
import java.util.*;
import java.util.concurrent.ConcurrentHashMap;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Purchase Invoice Return (InvfrmInvPurchaseInvoiceReturn, screen 125, DocumentTypeId 59) - every read the desktop
 * form and its loader make, with the same procedures and parameters. ":NNN" = Architecture.WinApp.Purchase/
 * InvfrmInvPurchaseInvoiceReturn.cs, "L:NNN" = frmLoadPurchaseInvoiceForReturn.cs, "D:NNN" = DAL 0434, "B:NNN" = BLL 0581.
 * Tenancy always comes from the session.
 */
@Repository
public class PurchaseInvoiceReturnRepository {

    public static final int TYPE = 59;
    /** tblUserRights / ScreenDefinition 125 ScreenName (SetRightsValueInRightsObject("InvfrmPurchaseReturn") :508). */
    public static final String RIGHTS_SCREEN = "InvfrmPurchaseReturn";
    /** purchaseInvoice.ScreenName :2502. */
    public static final String SAVE_SCREEN = "InvfrmPurchaseReturn";

    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    private final Map<String, List<String[]>> procParams = new ConcurrentHashMap<>();

    public PurchaseInvoiceReturnRepository(JdbcTemplate jdbc, CurrentUserContext context) {
        this.jdbc = jdbc;
        this.context = context;
    }

    // ------------------------------------------------------------------ helpers
    public static int num(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null || String.valueOf(o).isBlank()) return 0;
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (NumberFormatException e) { return 0; }
    }
    public static double dbl(Object o) {
        if (o instanceof Number) return ((Number) o).doubleValue();
        if (o == null || String.valueOf(o).isBlank()) return 0;
        try { return Double.parseDouble(String.valueOf(o).trim().replace(",", "")); } catch (NumberFormatException e) { return 0; }
    }
    public static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }
    public static boolean truthy(Object o) { String s = str(o).toLowerCase(Locale.ROOT); return s.equals("true") || s.equals("1"); }
    private int org() { return context.currentOrganizationId(); }
    private int comp() { return context.currentCompanyId(); }
    private int user() { return context.currentUserId(); }
    private int branch() { return context.currentBranchId(); }
    private int fy() { return context.currentFinancialYearId(); }
    private static Map<String, Object> ci(Map<String, Object> m) { var t = new TreeMap<String, Object>(String.CASE_INSENSITIVE_ORDER); if (m != null) t.putAll(m); return t; }

    /** CommonServices.GetConfigurationByOrgCompandConfigDescription / clsGlobalVariables.configrationsAllocation. */
    public String config(String description) {
        var rows = jdbc.queryForList("EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?,@CompanyId=?,@ConfigDescription=?,@Activity='GetConfigurationByOrgCompandConfigDescription'",
                org(), comp(), description);
        return rows.isEmpty() ? "" : str(ci(rows.get(0)).get("ConfigKey"));
    }
    /** true when the configuration row exists at all (clsGlobalVariables.configrationsAllocation.Where(..).Count > 0). */
    public boolean configExists(String description) {
        return !jdbc.queryForList("EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?,@CompanyId=?,@ConfigDescription=?,@Activity='GetConfigurationByOrgCompandConfigDescription'",
                org(), comp(), description).isEmpty();
    }
    /** CommonServices.GetERPFeatureById - USP_GetERPFeaturesByCompanyId. */
    public Set<Integer> features() {
        Set<Integer> out = new HashSet<>();
        for (var r : jdbc.queryForList("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?,@CompanyId=?", org(), comp())) out.add(num(ci(r).get("Id")));
        return out;
    }

    // ------------------------------------------------------------------ rights (:508-525, formright)
    public boolean hasRight(String right) {
        String role = Objects.toString(context.currentRoleName(), "");
        if (("Admin".equalsIgnoreCase(role) || "Administrator".equalsIgnoreCase(role)) && !"Delete".equals(right)) return true;
        return jdbc.queryForList("EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?,@ScreenName=?,@RightName=?,@CompanyId=?,@Activity='GetByUserId'",
                        user(), RIGHTS_SCREEN, role, comp()).stream()
                .anyMatch(row -> right.equalsIgnoreCase(str(ci(row).get("RightName")))
                        && (Boolean.TRUE.equals(ci(row).get("Value")) || "1".equals(str(ci(row).get("Value")))));
    }
    public void requireRight(String right) {
        if (!hasRight(right)) throw new IllegalArgumentException("You don't have " + right + " right on Purchase Invoice Return");
    }

    // ------------------------------------------------------------------ Load event flags (:501-570)
    public Map<String, Object> flags() {
        Map<String, Object> f = new LinkedHashMap<>();
        Set<Integer> features = features();
        f.put("wagesStatus", truthy(config("WagesCompulsoryOnPurchaseInvoiceReturn")));                 // :524
        f.put("branchImplemented", truthy(config("PurchaseInvoiceReturnBranchWise")));                    // :525
        f.put("branchFeature", features.contains(11));                                                    // :526
        f.put("wagesActiveOrInActive", jdbc.queryForList("EXEC dbo.USP_GetRefDocumentsForWages").stream()  // :527-528
                .anyMatch(r -> num(ci(r).get("RefDocumentTypeId")) == TYPE && truthy(ci(r).get("IsActive"))));
        f.put("hasMultiCurrencyFeature", features.contains(6));                                            // multiCurrencyFeature :1365
        f.put("feature5", features.contains(5));
        String v = config("Default NoofDecimal Points For Amount");
        f.put("amountDecimals", v.isEmpty() ? 0 : Math.max(0, Math.min(15, num(v))));
        v = config("Default NoofDecimal Points For Rate");
        f.put("rateDecimals", v.isEmpty() ? 2 : Math.max(0, Math.min(10, num(v))));
        v = config("DefaultNoOfDecimalPointsForFcyAmount");
        f.put("fcyDecimals", v.isEmpty() ? 0 : Math.max(0, Math.min(10, num(v))));
        // DefaultConfigurations :1483
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("jobLotId", configExists("Job/Lot") ? num(config("Job/Lot")) : null);
        d.put("baseCurrencyId", configExists("Base Currency") ? num(config("Base Currency")) : null);
        d.put("baseCurrencyRate", configExists("BaseCurrencyRate") ? num(config("BaseCurrencyRate")) : null);   // Conversion.ToInt :1504
        /* Q-C1: ItemSearchWithNameOrCode is read from the BaseCurrencyRate row, not ItemSearchByCode (:1507-1509). */
        d.put("itemSearchByCode", configExists("ItemSearchByCode") ? truthy(config("BaseCurrencyRate")) : null);
        f.put("defaults", d);
        Map<String, Object> rights = new LinkedHashMap<>();
        for (String r : List.of("View", "Save", "Update", "Delete", "Print", "CanView AllRecord")) rights.put(r.replace(" ", ""), hasRight(r));
        f.put("rights", rights);
        f.put("userBranchId", branch());
        f.put("userBranchName", userBranchName());
        return f;
    }

    public String userBranchName() {
        for (var r : jdbc.queryForList("EXEC dbo.Sp_Branches_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='GetAll'", org(), comp()))
            if (num(ci(r).get("Id")) == branch()) return str(ci(r).get("BranchName"));
        return "";
    }

    // ------------------------------------------------------------------ lookups (Load :576-596, Refresh :2995)
    public Map<String, Object> lookups(Map<String, Object> flags) {
        boolean branchFeature = Boolean.TRUE.equals(flags.get("branchFeature")), branchImplemented = Boolean.TRUE.equals(flags.get("branchImplemented"));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("accounts", accounts());                                                                    // AccountsFill :1115
        out.put("otherItems", DesktopProc.rows(jdbc, "Sp_InventoryItemsOther_GetAllMethod",           // OtherItemsBind :1271
                params("Activity", "ReadAll", "organizationId", org(), "CompanyId", comp())));
        int b = (branchFeature && branchImplemented) ? branch() : 0;                                        // bindWareHouse :785 / Branchlot :1073
        out.put("warehouses", DesktopProc.rows(jdbc, "[dbo].[USP_GetWarehousesAllocatedToBranch]", params("OrganizationId", org(), "CompanyId", comp(), "BranchId", b != 0 ? b : null)));
        out.put("jobLots", DesktopProc.rows(jdbc, "[dbo].[USP_GetJobLotsAllocatedToBranch]", params("OrganizationId", org(), "CompanyId", comp(), "BranchId", b != 0 ? b : null)));
        out.put("suppliers", suppliers());                                                                  // suppliercustomer :828
        out.put("paymentTerms", DesktopProc.rows(jdbc, "Sp_InvDueTerms_GetAllMethod",                    // PaymentTerms :920
                params("OrganizationId", org(), "CompanyId", comp(), "Activity", "GetAll")));
        out.put("deliveryTerms", List.of(idv(1, "Load"), idv(2, "Ponch")));                                 // DeliveryTerm :950
        out.put("commissionTypes", List.of(idv(1, "Flat"), idv(2, "Percent"), idv(3, "Comm Weight")));      // CommissionTypeFill :981
        out.put("commissionUoms", DesktopProc.rows(jdbc, "SpStaticColumnNames", params("Activity", "GetCommissionUom")));   // :1013
        out.put("items", items());                                                                          // item() :871
        out.put("packingTypes", DesktopProc.rows(jdbc, "Sp_InvPackingType_GetAllMethod", params("Activity", "ReadAll")));  // :1044
        out.put("cropYears", DesktopProc.rows(jdbc, "Sp_InvCropYear_GetAllMethod",                      // cropyear :1156
                params("OrganizationId", org(), "CompanyId", comp(), "Activity", "ReadAll")));
        out.put("currencies", DesktopProc.rows(jdbc, "Sp_MultiCurrency_GetAllMethod",                   // CurrencyFill :1458
                params("OrganizationId", org(), "CompanyId", comp(), "Activity", "ReadAll")));
        out.put("historyBranches", historyBranches(branchImplemented));
        return out;
    }
    private static Map<String, Object> idv(int id, String v) { Map<String, Object> m = new LinkedHashMap<>(); m.put("Id", id); m.put("Value", v); return m; }

    /** AccountsFill :1115 - clsGlobalVariables.AllAccountsWithCustomGroupId minus AccountTypeId 2,10,11,12,15,22, distinct ChartOfAccountId. */
    public List<Map<String, Object>> accounts() {
        Set<Integer> excluded = Set.of(2, 10, 11, 12, 15, 22), seen = new HashSet<>();
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : DesktopProc.rows(jdbc, "[dbo].[USP_GETAllAccountsFromCustomGroups]", params("OrganizationId", org(), "CompanyId", comp()))) {
            if (excluded.contains(num(r.get("AccountTypeId")))) continue;
            int id = num(r.get("ChartOfAccountId"));
            if (!seen.add(id)) continue;
            Map<String, Object> m = new LinkedHashMap<>(); m.put("Id", id); m.put("AccountTitle", str(r.get("AccountTitle"))); out.add(m);
        }
        return out;
    }
    /** suppliercustomer :828 - CommonServices.SupplierCustomerGetAllServiceBind = SupplierCustomer.Getall (BLL 0600:110),
     *  Sp_SupplierCustomer_GetAllMethod 'ReadByOrganizationCompanyId' (Id, CompanyName, GlAccountId are the columns read). */
    public List<Map<String, Object>> suppliers() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", params("OrganizationId", org(), "CompanyId", comp(), "Activity", "ReadByOrganizationCompanyId"))) {
            Map<String, Object> m = new LinkedHashMap<>(); m.put("Id", num(r.get("Id"))); m.put("CompanyName", str(r.get("CompanyName"))); m.put("GlAccountId", num(r.get("GlAccountId"))); out.add(m);
        }
        return out;
    }
    /** item() :871 - CommonServices.ReadAllItems = Item.ReadAllItems (BLL 0583:246), Sp_Item_GetAllMethod 'ReadAllItems' -> (Id, ItemName, ItemCodeNew). */
    public List<Map<String, Object>> items() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params("OrganizationId", org(), "CompanyId", comp(), "Activity", "ReadAllItems"))) {
            Map<String, Object> m = new LinkedHashMap<>(); m.put("Id", num(r.get("Id"))); m.put("ItemName", str(r.get("ItemName"))); m.put("ItemCode", str(r.get("ItemCodeNew"))); out.add(m);
        }
        return out;
    }
    /** UomFromGlobalBind :1137 - CommonServices.dtUomFromGloablUomScheduleByItemId (usp_getAllUomsByCompanyId, Active 1, the item). */
    public List<Map<String, Object>> uoms(int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (itemId <= 0) return out;
        for (var r : jdbc.queryForList("EXEC dbo.usp_getAllUomsByCompanyId @OrganizationId=?,@CompanyId=?,@ItemId=?,@Active=1", org(), comp(), itemId)) {
            var c = ci(r); Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", num(c.get("Id"))); m.put("UOMCode", str(c.get("UOMCode"))); m.put("Equivalent", dbl(c.get("Equivalent"))); out.add(m);
        }
        return out;
    }
    /** AvailableStockGetByItem :4494 - InventoryStockEvalautionDetail.GetCurrentStockByItemId (BLL 0574:870), every parameter sent. */
    public double currentStock(int warehouseId, int itemId, int jobLotId, String cropYear, String docDate, int packingTypeId, int packUomId) {
        var rows = jdbc.queryForList("EXEC dbo.Sp_SaleOrder_GetAllMethod @OrganizationId=?,@CompanyId=?,@ItemId=?,@DocDateTo=?,@WareHouseId=?,@JobLotId=?,@CropYear=?,@InvPackingTypeId=?,@ItemUomId=?,@Activity='GetCurrentStockByItemId'",
                org(), comp(), itemId, new SqlParameterValue(Types.TIMESTAMP, ts(docDate)), warehouseId, jobLotId, cropYear == null ? "" : cropYear, packingTypeId, packUomId);
        return rows.isEmpty() ? 0 : dbl(ci(rows.get(0)).get("AvailableStock"));
    }

    /** DocumentNo :1185 / DocumentNoBranch :1201. */
    public Map<String, Object> numbers(PurchaseInvoiceNumberingRepository numbering) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("docNo", numbering.next(org(), comp(), fy(), TYPE));
        m.put("branchSrNo", numbering.nextBranch(org(), comp(), fy(), branch(), TYPE));
        return m;
    }

    // ------------------------------------------------------------------ history (:3077, :3563, :3620)
    /** HistoryComboBranchFill :3563 -> (Id, BranchName). */
    public List<Map<String, Object>> historyBranches(boolean branchImplemented) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (branchImplemented) { Map<String, Object> m = new LinkedHashMap<>(); m.put("Id", branch()); m.put("BranchName", userBranchName()); out.add(m); return out; }
        for (var r : DesktopProc.rows(jdbc, "[dbo].[USP_GetBranchsAllocatedToUserFromPurchaseInvoice]", params("OrganizationId", org(), "CompanyId", comp(), "UserId", user(), "DocumentTypeId", TYPE))) {
            Map<String, Object> m = new LinkedHashMap<>(); m.put("Id", num(r.get("BranchId"))); m.put("BranchName", str(r.get("BranchName"))); out.add(m);
        }
        return out;
    }
    /** HistoryComboFill :3620 - Usp_AllComboAgainstPurchaseInvoice rows with Activity = 'Supplier'. */
    public List<Map<String, Object>> historySuppliers(String branchesIds) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : DesktopProc.rows(jdbc, "[dbo].[Usp_AllComboAgainstPurchaseInvoice]", params("OrganizationId", org(), "CompanyId", comp(), "DocumentTypeIds", String.valueOf(TYPE), "BranchesIds", str(branchesIds).isEmpty() ? null : branchesIds)))
            if ("Supplier".equals(str(r.get("Activity")))) { Map<String, Object> m = new LinkedHashMap<>(); m.put("Id", num(r.get("Id"))); m.put("Supplier", str(r.get("ReferenceName"))); out.add(m); }
        return out;
    }
    /** GetAll :3077 -> InvPurchaseInvoice.FormHistory (B:370). No @BranchesIds: the 59 history does not send it. */
    public List<Map<String, Object>> history(String dateMode, String fromDate, String toDate, int fromDocNo, int toDocNo, int supplierId, boolean canViewAll) {
        String prefix = switch (str(dateMode).toLowerCase(Locale.ROOT)) { case "entry" -> "Entry"; case "modify" -> "Modify"; case "approved" -> "Approved"; default -> ""; };
        Map<String, Object> p = params("OrganizationId", org(), "CompanyId", comp(), "DocumentTypeId", TYPE, "CanViewAllRecord", canViewAll, "FinancialYearId", fy());
        if (!canViewAll) p.put("EntryUser", user());
        if (!str(fromDate).isEmpty()) p.put(prefix.isEmpty() ? "FromDate" : prefix + "FromDate", ts(fromDate));
        if (!str(toDate).isEmpty()) p.put(prefix.isEmpty() ? "ToDate" : prefix + "ToDate", ts(toDate));
        if (fromDocNo != 0) p.put("FromDocNo", fromDocNo);
        if (toDocNo != 0) p.put("ToDocNo", toDocNo);
        if (supplierId != 0) p.put("SupplierCustomerId", supplierId);
        p.put("Activity", "FormHistory");
        return DesktopProc.rows(jdbc, "[Sp_InvPurchaseInvoice_GetAllMethod]", p);
    }
    private static Timestamp ts(String d) {
        String s = str(d); if (s.isEmpty()) return new Timestamp(System.currentTimeMillis());
        return Timestamp.valueOf(s.length() >= 10 ? s.substring(0, 10) + " 00:00:00" : s);
    }

    // ------------------------------------------------------------------ read (ReadById :2746 -> BLL GetByID -> D:580)
    public Map<String, Object> header(int id) {
        var rows = jdbc.queryForList("SELECT * FROM dbo.InvPurchaseInvoice WHERE Id=? AND DocumentTypeId=? AND OrganizationId=? AND CompanyId=?", id, TYPE, org(), comp());
        return rows.isEmpty() ? null : ci(rows.get(0));
    }
    public Map<String, Object> read(int id) {
        if (header(id) == null) return null;
        var rows = jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @Id=?,@Activity='ReadById' WITH RECOMPILE", id);
        if (rows.isEmpty()) return null;
        Map<String, Object> h = new LinkedHashMap<>(rows.get(0));
        h.put("details", readList(id, "DirectPurchaseDetailReadByInvPurchaseInvoiceId"));
        h.put("expenses", readList(id, "InvPurchaseInvoiceExpense_ReadByPurchaseInvoiceID"));
        h.put("freight", readList(id, "InvPurchaseInvoiceFreight_ReadByPurchaseInvoiceID"));
        h.put("journal", readList(id, "InvPurchaseInvoiceJournal_ReadByPurchaseInvoiceID"));
        h.put("voucherHeadId", voucherHeadId(id));
        return h;
    }
    private List<Map<String, Object>> readList(int id, String activity) {
        return jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @Id=?,@Activity=? WITH RECOMPILE", id, activity);
    }
    /** VoucherHeadIdGet :1252. */
    public int voucherHeadId(int id) {
        var v = jdbc.queryForList("EXEC dbo.Sp_Vouchers_GetMethods @Activity='GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId',@OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@DocumentTypeSrNo=?", org(), comp(), TYPE, id);
        return v.isEmpty() ? 0 : num(ci(v.get(0)).get("Id"));
    }

    // ------------------------------------------------------------------ loader (frmLoadPurchaseInvoiceForReturn)
    /** L:BranchesFill - USP_GetBranchsAllocatedToUserFromPurchaseInvoice with DocumentTypeId 0 (not sent). */
    public List<Map<String, Object>> loaderBranches(boolean branchImplemented) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (branchImplemented) { Map<String, Object> m = new LinkedHashMap<>(); m.put("BranchId", branch()); m.put("BranchName", userBranchName()); out.add(m); return out; }
        for (var r : DesktopProc.rows(jdbc, "[dbo].[USP_GetBranchsAllocatedToUserFromPurchaseInvoice]", params("OrganizationId", org(), "CompanyId", comp(), "UserId", user()))) {
            Map<String, Object> m = new LinkedHashMap<>(); m.put("BranchId", num(r.get("BranchId"))); m.put("BranchName", str(r.get("BranchName"))); out.add(m);
        }
        return out;
    }
    public String financialYearStart() {
        var r = jdbc.queryForList("SELECT Start_Period FROM dbo.FinancialYear WHERE Id=?", fy());
        return r.isEmpty() || r.get(0).get("Start_Period") == null ? java.time.LocalDate.now().toString() : String.valueOf(r.get(0).get("Start_Period")).substring(0, 10);
    }
    /** L:PendingGdnLoad - InvPurchaseInvoice.PendingPurchaseInvoiceForReturnInvoice (B:3190). */
    public List<Map<String, Object>> pendingInvoices(String fromDate, String toDate, String branchesIds) {
        return DesktopProc.rows(jdbc, "USP_PendingPurchaseInvoiceForReturnInvoice", params("OrganizationId", org(), "CompanyId", comp(), "FinancialYearId", fy() != 0 ? fy() : null,
                "FromDate", ts(fromDate), "ToDate", ts(toDate), "BranchesIds", str(branchesIds).isEmpty() ? null : branchesIds));
    }

    // ------------------------------------------------------------------ DAL 0434 SetData pieces not in PurchaseInvoiceContracts
    public List<Map<String, Object>> itemGlIds() {
        return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params("OrganizationId", org(), "CompanyId", comp(), "Activity", "GetItemGlIdsandItemName"));
    }
    public List<Map<String, Object>> jobLotGlIds() {
        return DesktopProc.rows(jdbc, "[dbo].[SP_JobLot_ReadMethod]", params("OrganizationId", org(), "CompanyId", comp(), "Activity", "GetJobLotGlIdsandName"));
    }
    /** CommonServices.GetEqvilentByItemIdAndUomScheduleId (DAL 0205:391). */
    public double equivalent(int itemId, int scheduleId) {
        var r = DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params("OrganizationId", org(), "CompanyId", comp(), "ItemId", itemId, "ScheduleId", scheduleId, "Activity", "GetEqvilentByItemIdAndUomScheduleId"));
        return r.isEmpty() ? 0 : dbl(r.get(0).get("Equivalent"));
    }
    /** CommonServices.FIFOImplemention (DAL 0205:725) - the stock layers. */
    public List<Map<String, Object>> stockByFifo(Map<String, Object> p) { return DesktopProc.rows(jdbc, "USP_GetStockByFifoMethod", p); }
    /** CommonServices.GetInventoryStockEvalautionDetailByOtherIds (DAL 0205:1331). */
    public List<Map<String, Object>> stockByOtherIds(int otherType, int otherId, int otherSubId) {
        return DesktopProc.rows(jdbc, "USP_GetInventoryStockEvalautionDetailByOtherIds", params("OrganizationId", org(), "CompanyId", comp(), "OtherDocumentTypeId", otherType, "OtherDocNoId", otherId, "OtherSubDocNoId", otherSubId));
    }
    public void run(String sql, Object... args) { ProcExec.run(jdbc, sql, args); }

    /**
     * GenericProvider.SetProc for a model the invoice contracts do not cover (InventoryStockEvalautionDetail): every
     * parameter of the procedure is bound, from the supplied values or the CLR default of its type.
     */
    public int setProc(String procedure, Map<String, Object> supplied) {
        var values = ci(supplied);
        List<String[]> ps = procParams.computeIfAbsent(procedure, p -> jdbc.queryForList(
                "SELECT p.name, t.name AS tname FROM sys.parameters p JOIN sys.types t ON p.user_type_id=t.user_type_id WHERE p.object_id=OBJECT_ID(?) ORDER BY p.parameter_id", "dbo." + p).stream()
                .map(r -> new String[]{String.valueOf(r.get("name")).substring(1), String.valueOf(r.get("tname")).toLowerCase(Locale.ROOT)}).toList());
        if (ps.isEmpty()) throw new IllegalStateException("Procedure " + procedure + " was not found");
        String marks = String.join(",", Collections.nCopies(ps.size(), "?"));
        return jdbc.execute((ConnectionCallback<Integer>) c -> {
            try (CallableStatement st = c.prepareCall("{call dbo." + procedure + "(" + marks + ")}")) {
                for (int i = 0; i < ps.size(); i++) {
                    String name = ps.get(i)[0], t = ps.get(i)[1];
                    Object v = values.get(name);
                    switch (t) {
                        case "int", "smallint", "tinyint" -> st.setInt(i + 1, v == null ? 0 : num(v));
                        case "bigint" -> st.setLong(i + 1, v == null ? 0L : (long) dbl(v));
                        case "bit" -> st.setBoolean(i + 1, v != null && truthy(v));
                        case "float", "real", "decimal", "numeric", "money" -> st.setDouble(i + 1, v == null ? 0d : dbl(v));
                        case "date", "datetime", "datetime2", "smalldatetime" -> {
                            if (v == null) st.setNull(i + 1, Types.TIMESTAMP);
                            else st.setTimestamp(i + 1, v instanceof Timestamp x ? x : v instanceof java.util.Date x ? new Timestamp(x.getTime()) : ts(String.valueOf(v)));
                        }
                        default -> { if (v == null) st.setNull(i + 1, Types.NVARCHAR); else st.setString(i + 1, String.valueOf(v)); }
                    }
                }
                boolean rs = st.execute(); int found = 0; boolean taken = false;
                while (true) {
                    if (rs) { try (ResultSet r = st.getResultSet()) { if (!taken && r.next()) { Object first = r.getObject(1); if (first instanceof Number n) found = n.intValue(); } taken = true; while (r.next()) { } } }
                    else if (st.getUpdateCount() == -1) break;
                    rs = st.getMoreResults();
                }
                return found;
            }
        });
    }
}
