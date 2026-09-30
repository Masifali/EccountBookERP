package com.mst.repositories;

import com.mst.repositories.support.ProcExec;
import com.mst.security.CurrentUserContext;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Repository;

import java.sql.Types;
import java.util.*;

/**
 * Sale Invoice Return (InvfrmSaleInvoiceReturn, DocumentTypeId 98) - every read the desktop form makes,
 * with the same procedures and parameters. Line references are to
 * Architecture.WinApp.Sale/InvfrmSaleInvoiceReturn.cs unless marked L: (LoadSaleInvoiceForReturn.cs).
 * The form saves through the PURCHASE invoice tables (InvPurchaseInvoice*), discriminated by type 98.
 */
@Repository
public class SaleInvoiceReturnRepository {

    public static final int TYPE = 98;
    public static final String SCREEN = "InvfrmSaleInvoiceReturn";

    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;

    public SaleInvoiceReturnRepository(JdbcTemplate jdbc, CurrentUserContext context) {
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
    public static boolean truthy(Object o) {
        String s = str(o).toLowerCase(Locale.ROOT);
        return s.equals("true") || s.equals("1");
    }
    private int org() { return context.currentOrganizationId(); }
    private int comp() { return context.currentCompanyId(); }
    private int user() { return context.currentUserId(); }
    private int branch() { return context.currentBranchId(); }
    private int fy() { return context.currentFinancialYearId(); }

    /** Sp_ConfigrationsAllocation_GetAllMethod 'GetConfigurationByOrgCompandConfigDescription' (BLL 0621). */
    public String config(String description) {
        var rows = jdbc.queryForList("EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?,@CompanyId=?,@ConfigDescription=?,@Activity='GetConfigurationByOrgCompandConfigDescription'",
                org(), comp(), description);
        return rows.isEmpty() ? "" : str(rows.get(0).get("ConfigKey"));
    }

    /** USP_GetERPFeaturesByCompanyId - the global ErpFeaturesList. */
    public Set<Integer> features() {
        Set<Integer> out = new HashSet<>();
        for (var r : jdbc.queryForList("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?,@CompanyId=?", org(), comp())) out.add(num(r.get("Id")));
        return out;
    }

    // ------------------------------------------------------------------ rights (:510-525)
    public boolean hasRight(String right) {
        String role = Objects.toString(context.currentRoleName(), "");
        if (("Admin".equalsIgnoreCase(role) || "Administrator".equalsIgnoreCase(role)) && !"Delete".equals(right)) return true;
        return jdbc.queryForList("EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?,@ScreenName=?,@RightName=?,@CompanyId=?,@Activity='GetByUserId'",
                        user(), SCREEN, role, comp()).stream()
                .anyMatch(row -> right.equalsIgnoreCase(str(row.get("RightName")))
                        && (Boolean.TRUE.equals(row.get("Value")) || "1".equals(str(row.get("Value")))));
    }

    /** Load-event flags (:526-553), all from the desktop's own sources. */
    public Map<String, Object> flags() {
        Map<String, Object> f = new LinkedHashMap<>();
        Set<Integer> features = features();
        f.put("wagesStatus", truthy(config("WagesCompulsoryOnSaleInvoiceReturn")));
        boolean wagesActive = jdbc.queryForList("EXEC dbo.USP_GetRefDocumentsForWages @RefDocumentTypeId=?", TYPE).stream()
                .anyMatch(r -> num(r.get("RefDocumentTypeId")) == TYPE && truthy(r.get("IsActive")));
        f.put("wagesActiveOrInActive", wagesActive);
        f.put("subsidiaryAccountAllownOnVouchers", features.contains(4));
        f.put("branchFeature", features.contains(11));
        f.put("branchImplemented", truthy(config("SaleInvoiceReturnBranchWise")));
        String v = config("WagesAmountCalculateOnQty");
        f.put("wagesAmountCalculateOnQty", !v.isEmpty() && truthy(v));
        v = config("ContractWagesChargetoProduct");
        f.put("contractWagesChargetoProduct", !v.isEmpty() && truthy(v));
        v = config("DebitAmountChargetoExpenseAcFreightGridPurchase");
        f.put("freightDebitToExpenses", !v.isEmpty() && truthy(v));
        f.put("commissionDebitToExpenses", truthy(config("DebitAmountChargetoExpenseAcOfCommission")));
        v = config("Default NoofDecimal Points For Amount");
        f.put("amountDecimals", v.isEmpty() ? 0 : Math.max(0, Math.min(4, num(v))));
        v = config("Default NoofDecimal Points For Rate");
        f.put("rateDecimals", v.isEmpty() ? 2 : Math.max(0, Math.min(6, num(v))));
        f.put("rights", Map.of("save", hasRight("Save"), "update", hasRight("Update"), "delete", hasRight("Delete"), "print", hasRight("Print"),
                "canViewAllRecord", hasRight("CanView AllRecord")));
        f.put("userBranchId", branch());
        f.put("userBranchName", userBranchName());
        return f;
    }

    private String userBranchName() {
        for (var r : jdbc.queryForList("EXEC dbo.Sp_Branches_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='GetAll'", org(), comp()))
            if (num(r.get("Id")) == branch()) return str(r.get("BranchName"));
        return "";
    }

    // ------------------------------------------------------------------ lookups (Load :554-679)
    public Map<String, Object> lookups(Map<String, Object> flags) {
        boolean subsidiary = Boolean.TRUE.equals(flags.get("subsidiaryAccountAllownOnVouchers"));
        boolean branchFeature = Boolean.TRUE.equals(flags.get("branchFeature"));
        boolean branchImplemented = Boolean.TRUE.equals(flags.get("branchImplemented"));
        boolean freightDebit = Boolean.TRUE.equals(flags.get("freightDebitToExpenses"));
        Map<String, Object> out = new LinkedHashMap<>();
        // OtherItemsBind :1409
        out.put("otherItems", jdbc.queryForList("EXEC dbo.Sp_InventoryItemsOther_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadAll'", org(), comp()));
        // CommissionDebitAccountFill :766 -> (GlAccountId, AccountTitle, SupplierCustomerId)
        List<Map<String, Object>> commDr = new ArrayList<>();
        if (subsidiary) {
            for (var r : jdbc.queryForList("EXEC dbo.USP_GetVendorsAndCustomersForTransporter @OrganizationId=?,@CompanyId=?", org(), comp()))
                commDr.add(acc(r.get("GlAccountId"), r.get("CompanyName"), r.get("Id")));
        } else {
            for (var r : coa("11,12,13,14,20,21", null)) commDr.add(acc(r.get("Id"), r.get("AccountTitle"), 0));
        }
        out.put("commissionDebitAccounts", commDr);
        // AccountsFill :1257 -> dtAccountlst / dtAccountlstForFreight (Id, SupplierCustomerId, AccountTitle)
        List<Map<String, Object>> accounts = new ArrayList<>(), freightAccounts = new ArrayList<>();
        if (subsidiary) {
            /* Q-A1: the desktop fills dtAccountlstForFreight then assigns dtAccountlstForFreight = dtAccountlst (empty) -
               both lists end up EMPTY with ERP feature 4. Reproduced. */
        } else {
            for (var r : coa(null, "4,10,2,11,12,13,14,15,20,21,22")) accounts.add(acc(r.get("Id"), r.get("AccountTitle"), 0));
            if (!freightDebit) freightAccounts = accounts;
            else for (var r : coa(null, "4,10,2,15,22")) freightAccounts.add(acc(r.get("Id"), r.get("AccountTitle"), 0));
        }
        out.put("journalAccounts", accounts);
        out.put("freightAccounts", freightAccounts);
        // CreditAccountForEmptyBag :699 -> global AllAccountsWithCustomGroupId (USP_GETAllAccountsFromCustomGroups), types 8,10
        List<Map<String, Object>> ebAcc = new ArrayList<>();
        for (var r : jdbc.queryForList("EXEC dbo.USP_GETAllAccountsFromCustomGroups @OrganizationId=?,@CompanyId=?", org(), comp())) {
            int t = num(r.get("AccountTypeId"));
            if (t == 8 || t == 10) ebAcc.add(acc(r.get("Id"), r.get("AccountTitle"), 0));
        }
        out.put("emptyBagCreditAccounts", ebAcc);
        // PaymentTerms :1127 (Rows[0] activated - first term)
        out.put("paymentTerms", jdbc.queryForList("EXEC dbo.Sp_InvDueTerms_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='GetAll'", org(), comp()));
        // DeliveryTerm :1144 / CommissionTypeFill :1163 - static in the desktop
        out.put("deliveryTerms", List.of(idv(1, "Load"), idv(2, "Ponch"), idv(3, "Load & FactoryWeight")));
        out.put("commissionTypes", List.of(idv(1, "Flat"), idv(2, "Percent"), idv(3, "Comm Weight")));
        // CommissionUOMFill :1182
        out.put("commissionUoms", jdbc.queryForList("EXEC dbo.SpStaticColumnNames @Activity='GetCommissionUom'"));
        // bindWareHouse :1011 / Branchlot :1215 - @BranchId only when BranchFeature && BranchImplemented
        Integer branchArg = (branchFeature && branchImplemented) ? branch() : null;
        out.put("warehouses", branchArg == null
                ? jdbc.queryForList("EXEC dbo.USP_GetWarehousesAllocatedToBranch @OrganizationId=?,@CompanyId=?", org(), comp())
                : jdbc.queryForList("EXEC dbo.USP_GetWarehousesAllocatedToBranch @OrganizationId=?,@CompanyId=?,@BranchId=?", org(), comp(), branchArg));
        out.put("jobLots", branchArg == null
                ? jdbc.queryForList("EXEC dbo.USP_GetJobLotsAllocatedToBranch @OrganizationId=?,@CompanyId=?", org(), comp())
                : jdbc.queryForList("EXEC dbo.USP_GetJobLotsAllocatedToBranch @OrganizationId=?,@CompanyId=?,@BranchId=?", org(), comp(), branchArg));
        // suppliercustomer :1054 - customers AND commission agents of sale invoices 95,99,186
        out.put("parties", parties());
        // PackingType :1199, cropyear :1357
        out.put("packingTypes", jdbc.queryForList("EXEC dbo.Sp_InvPackingType_GetAllMethod @Activity='ReadAll'"));
        out.put("cropYears", jdbc.queryForList("EXEC dbo.Sp_InvCropYear_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadAll'", org(), comp()));
        out.put("numbers", numbers());
        out.put("historyBranches", historyBranches(branchImplemented));
        return out;
    }

    private List<Map<String, Object>> coa(String typeIds, String typeIdsNot) {
        String sql = "EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId=?,@CompanyId=?,@AppId=?,@UserId=?,@Activity='GetAccountTitleByAccountTypeIds'"
                + (typeIds != null ? ",@AccountTypeIds=?" : "") + (typeIdsNot != null ? ",@AccountTypeIdsNot=?" : "");
        List<Object> args = new ArrayList<>(List.of(org(), comp(), context.currentAppId(), user()));
        if (typeIds != null) args.add(typeIds);
        if (typeIdsNot != null) args.add(typeIdsNot);
        return jdbc.queryForList(sql, args.toArray());
    }
    private static Map<String, Object> acc(Object id, Object title, Object party) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", num(id)); m.put("AccountTitle", str(title)); m.put("SupplierCustomerId", num(party));
        return m;
    }
    private static Map<String, Object> idv(int id, String v) { Map<String, Object> m = new LinkedHashMap<>(); m.put("Id", id); m.put("Value", v); return m; }

    /** DocumentNo :1373 + DocumentBranchNo :1385. */
    public Map<String, Object> numbers() {
        Map<String, Object> m = new LinkedHashMap<>();
        var d = jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@FinancialYearId=?,@Activity='GenerateCode'", org(), comp(), TYPE, fy());
        m.put("docNo", d.isEmpty() ? 0 : num(d.get(0).get("DocNo")));
        var b = jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@FinancialYearId=?,@BranchesId=?,@Activity='GenerateBranchSrNo'", org(), comp(), TYPE, fy(), branch());
        m.put("branchSrNo", b.isEmpty() ? 0 : num(b.get(0).values().iterator().next()));
        return m;
    }

    /** suppliercustomer :1054 - the party combo rows (Id, CompanyName, PartyCode, GlAccountId, ...). */
    public List<Map<String, Object>> parties() {
        return jdbc.queryForList("EXEC dbo.USP_GetPartiesFromSaleInvoiceWithGlAccount @OrganizationId=?,@CompanyId=?,@DocumentTypeIds=?", org(), comp(), "95,99,186");
    }
    /** ItemsBindByParty :1071 - items ever sold to this customer. */
    public List<Map<String, Object>> itemsByParty(int partyId) {
        return jdbc.queryForList("EXEC dbo.USP_GetItemsFromSaleInvoiceAgainstPartyId @OrganizationId=?,@CompanyId=?,@SupplierCustomerId=?,@DocumentTypeIds=?", org(), comp(), partyId, "95,99,186");
    }
    /** item() :1090 (Refresh only) - all items. */
    public List<Map<String, Object>> allItems() {
        return jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadAllForComboTwoColumns'", org(), comp());
    }
    /** PackUOM :1309 - CommonServices.GetUomScheduleByItemId. */
    public List<Map<String, Object>> uoms(int itemId) {
        return jdbc.queryForList("EXEC dbo.Sp_UOMSchedule_GetAllMethod @OrganizationId=?,@CompanyId=?,@ItemId=?,@Activity='ReadByItemID'", org(), comp(), itemId);
    }

    // ------------------------------------------------------------------ history (:711-860, :3328)
    /** HistoryComboBranchFill :819 -> (Id, BranchName, Selected). */
    public List<Map<String, Object>> historyBranches(boolean branchImplemented) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (branchImplemented) {
            Map<String, Object> m = new LinkedHashMap<>(); m.put("Id", branch()); m.put("BranchName", userBranchName()); m.put("Selected", true); out.add(m);
            return out;
        }
        for (var r : jdbc.queryForList("EXEC dbo.USP_GetBranchsAllocatedToUserFromPurchaseInvoice @OrganizationId=?,@CompanyId=?,@UserId=?,@DocumentTypeId=?", org(), comp(), user(), TYPE)) {
            Map<String, Object> m = new LinkedHashMap<>(); m.put("Id", num(r.get("BranchId"))); m.put("BranchName", str(r.get("BranchName"))); m.put("Selected", num(r.get("BranchId")) == branch()); out.add(m);
        }
        return out;
    }
    /** HistoryComboFill :711 - Usp_AllComboAgainstPurchaseInvoice rows with Activity='Supplier'. */
    public List<Map<String, Object>> historyCustomers(String branchesIds) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("EXEC dbo.Usp_AllComboAgainstPurchaseInvoice @OrganizationId=?,@CompanyId=?,@DocumentTypeIds=?,@BranchesIds=?", org(), comp(), String.valueOf(TYPE), branchesIds))
            if ("Supplier".equalsIgnoreCase(str(r.get("Activity")))) { Map<String, Object> m = new LinkedHashMap<>(); m.put("Id", num(r.get("Id"))); m.put("Supplier", str(r.get("ReferenceName"))); out.add(m); }
        return out;
    }
    /** GetAll :3328 -> InvPurchaseInvoice.FormHistory. */
    public List<Map<String, Object>> history(String dateMode, String fromDate, String toDate, int fromDocNo, int toDocNo, int customerId, String branchesIds, boolean canViewAll) {
        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@CanViewAllRecord=?,@FinancialYearId=?,@Activity='FormHistory'");
        List<Object> args = new ArrayList<>(List.of(org(), comp(), TYPE, canViewAll, fy()));
        if (!canViewAll) { sql.append(",@EntryUser=?"); args.add(user()); }
        String prefix = switch (str(dateMode).toLowerCase(Locale.ROOT)) { case "entry" -> "Entry"; case "modify" -> "Modify"; case "approved" -> "Approved"; default -> ""; };
        if (!str(fromDate).isEmpty()) { sql.append(",@").append(prefix).append("FromDate=?"); args.add(new SqlParameterValue(Types.TIMESTAMP, java.sql.Timestamp.valueOf(fromDate.substring(0, 10) + " 00:00:00"))); }
        if (!str(toDate).isEmpty()) { sql.append(",@").append(prefix).append("ToDate=?"); args.add(new SqlParameterValue(Types.TIMESTAMP, java.sql.Timestamp.valueOf(toDate.substring(0, 10) + " 00:00:00"))); }
        if (fromDocNo != 0) { sql.append(",@FromDocNo=?"); args.add(fromDocNo); }
        if (toDocNo != 0) { sql.append(",@ToDocNo=?"); args.add(toDocNo); }
        if (customerId != 0) { sql.append(",@SupplierCustomerId=?"); args.add(customerId); }
        if (!str(branchesIds).isEmpty()) { sql.append(",@BranchesIds=?"); args.add(branchesIds); }
        return jdbc.queryForList(sql.toString(), args.toArray());
    }

    // ------------------------------------------------------------------ read (:2609) / DAL GetByID (D:580)
    public Map<String, Object> read(int id) {
        var rows = jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @Id=?,@Activity='ReadById' WITH RECOMPILE", id);
        if (rows.isEmpty()) return null;
        Map<String, Object> h = new LinkedHashMap<>(rows.get(0));
        if (num(h.get("DocumentTypeId")) != TYPE || num(h.get("OrganizationId")) != org() || num(h.get("CompanyId")) != comp()) return null;
        h.put("details", jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @Id=?,@Activity='DirectPurchaseDetailReadByInvPurchaseInvoiceId' WITH RECOMPILE", id));
        h.put("expenses", jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @Id=?,@Activity='InvPurchaseInvoiceExpense_ReadByPurchaseInvoiceID' WITH RECOMPILE", id));
        h.put("freight", jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @Id=?,@Activity='InvPurchaseInvoiceFreight_ReadByPurchaseInvoiceID' WITH RECOMPILE", id));
        h.put("journal", jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @Id=?,@Activity='InvPurchaseInvoiceJournal_ReadByPurchaseInvoiceID' WITH RECOMPILE", id));
        h.put("emptyBags", jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @Id=?,@Activity='InvPurchaseInvoiceEmptyBags_ReadByPurchaseInvoiceID' WITH RECOMPILE", id));
        h.put("voucherHeadId", voucherHeadId(id));
        // Q-W1: ReadById :2699 reads the wages of RefDocumentType 143 for the invoice's GRN ids (0 for non-GRN returns)
        Set<String> grnIds = new LinkedHashSet<>();
        for (var d : (List<Map<String, Object>>) h.get("details")) grnIds.add(String.valueOf(num(d.get("InvGrnId"))));
        h.put("wagesAmountOverride", wages(143, String.join(",", grnIds)));
        h.put("attachments", attachments(id));
        return h;
    }
    /** VoucherHeadIdGet - Sp_Vouchers_GetMethods 'GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId'. */
    public int voucherHeadId(int id) {
        var v = jdbc.queryForList("EXEC dbo.Sp_Vouchers_GetMethods @Activity='GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId',@OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@DocumentTypeSrNo=?", org(), comp(), TYPE, id);
        return v.isEmpty() ? 0 : num(v.get(0).get("Id"));
    }
    /** USP_GetWagesAmountByRefDocumentTypeIdandRefIds - always one row. */
    public double wages(int refType, String refIds) {
        if (str(refIds).isEmpty()) return 0;
        var w = jdbc.queryForList("EXEC dbo.USP_GetWagesAmountByRefDocumentTypeIdandRefIds @OrganizationId=?,@CompanyId=?,@RefDocumentTypeId=?,@RefIds=?", org(), comp(), refType, refIds);
        return w.isEmpty() ? 0 : dbl(w.get(0).get("WagesAmount"));
    }
    public List<Map<String, Object>> attachments(int id) {
        try {
            return jdbc.queryForList("EXEC dbo.Sp_DMSAttachments_GetAllMethod @RefDocumentTypeId=?,@Id=?,@Activity='ReadAttachmentsbyRefDocumentTypeId'", TYPE, id);
        } catch (RuntimeException e) { return List.of(); }
    }

    // ------------------------------------------------------------------ loader (LoadSaleInvoiceForReturn)
    /** L: BranchesDbCall - USP_GetBranchsAllocatedToUserFromSaleInvoice (no @DocumentTypeId). */
    public List<Map<String, Object>> loaderBranches(boolean branchImplemented) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (branchImplemented) { Map<String, Object> m = new LinkedHashMap<>(); m.put("BranchId", branch()); m.put("BranchName", userBranchName()); out.add(m); return out; }
        for (var r : jdbc.queryForList("EXEC dbo.USP_GetBranchsAllocatedToUserFromSaleInvoice @OrganizationId=?,@CompanyId=?,@UserId=?", org(), comp(), user())) {
            Map<String, Object> m = new LinkedHashMap<>(); m.put("BranchId", num(r.get("BranchId"))); m.put("BranchName", str(r.get("BranchName"))); out.add(m);
        }
        return out;
    }
    public String financialYearStart() {
        var r = jdbc.queryForList("SELECT Start_Period FROM dbo.FinancialYear WHERE Id=?", fy());
        return r.isEmpty() || r.get(0).get("Start_Period") == null ? java.time.LocalDate.now().toString() : String.valueOf(r.get(0).get("Start_Period")).substring(0, 10);
    }
    /** L: PendingDataDbCall - USP_PendingSaleInvoiceForReturnInvoice. */
    public List<Map<String, Object>> pendingSaleInvoices(String fromDate, String toDate, String branchesIds) {
        return jdbc.queryForList("EXEC dbo.USP_PendingSaleInvoiceForReturnInvoice @OrganizationId=?,@CompanyId=?,@FinancialYearId=?,@FromDate=?,@ToDate=?,@BranchesIds=?",
                org(), comp(), fy(), new SqlParameterValue(Types.DATE, java.sql.Date.valueOf(fromDate.substring(0, 10))), new SqlParameterValue(Types.DATE, java.sql.Date.valueOf(toDate.substring(0, 10))), branchesIds);
    }
    /** LoadFreightDataFromSaleInvoice :5639 - Sp_InvSaleInvoice_GetAllMethod 'SaleInvoiceFreight_ByIds'. */
    public List<Map<String, Object>> saleInvoiceFreight(String invoiceIds) {
        return jdbc.queryForList("EXEC dbo.Sp_InvSaleInvoice_GetAllMethod @GdnIds=?,@Activity='SaleInvoiceFreight_ByIds'", invoiceIds);
    }
    /** DeleteDetailrow :5221 - usp_StockInReferenceValidationReferredOrNot (raises when referenced). */
    public void validateDetailDelete(int id, int detailId) {
        ProcExec.run(jdbc, "EXEC dbo.usp_StockInReferenceValidationReferredOrNot @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@Id=?,@DetailId=?", org(), comp(), TYPE, id, detailId);
    }
    /** btnDelete :4030 - Sp_InvoicesVouchersandStocksDelete. */
    public void delete(int id) {
        ProcExec.run(jdbc, "EXEC dbo.Sp_InvoicesVouchersandStocksDelete @OrganizationId=?,@CompanyId=?,@Id=?,@DocumentTypeId=?,@UserId=?", org(), comp(), id, TYPE, user());
    }
    /** CGS reversal rate (GeneralFinancialMethods, 98 branch) - usp_getAvgRateOrlastPurchaseRate. */
    public Map<String, Object> cgsRate(int itemId, int rateUomId, Object docDate, int recId, int refType, int refDocId, int refDocSubId) {
        StringBuilder sql = new StringBuilder("EXEC dbo.usp_getAvgRateOrlastPurchaseRate @OrganizationId=?,@CompanyId=?,@ItemId=?,@RateUomId=?,@DocDate=?");
        List<Object> args = new ArrayList<>(List.of(org(), comp(), itemId, rateUomId, new SqlParameterValue(Types.DATE, java.sql.Date.valueOf(String.valueOf(docDate).substring(0, 10)))));
        if (recId > 0) { sql.append(",@DocumentTypeId=?,@RecId=?"); args.add(TYPE); args.add(recId); }
        if (refType != 0 && refDocId != 0 && refDocSubId != 0) { sql.append(",@RefDocumentTypeId=?,@RefDocIdNo=?,@RefDocSubIdNo=?"); args.add(refType); args.add(refDocId); args.add(refDocSubId); }
        var rows = jdbc.queryForList(sql.toString(), args.toArray());
        return rows.isEmpty() ? Map.of() : rows.get(0);
    }
    /** Header of any type-98 invoice for scope checks. */
    public Map<String, Object> header(int id) {
        var rows = jdbc.queryForList("SELECT * FROM dbo.InvPurchaseInvoice WHERE Id=? AND DocumentTypeId=? AND OrganizationId=? AND CompanyId=?", id, TYPE, org(), comp());
        return rows.isEmpty() ? null : rows.get(0);
    }
}
