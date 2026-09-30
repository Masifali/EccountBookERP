package com.mst.repositories;

import com.mst.security.CurrentUserContext;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Repository;

import java.sql.Types;
import java.util.*;

import static com.mst.services.PurchaseInvoiceFinancialRules.copy;
import static com.mst.services.PurchaseInvoiceFinancialRules.i;
import static com.mst.services.PurchaseInvoiceFinancialRules.s;

/**
 * Read side of frmPurchaseInvoiceAgaintGrnDirect (screen 131, DocumentTypeId 138). Every query is the
 * one the desktop form reaches through its BLL; tenancy always comes from the session. No catch-and-
 * return-empty: a failing procedure surfaces as an error.
 */
@Repository
public class PurchaseInvoiceAgainstGrnDirectRepository {

    public static final int TYPE = 138;
    /** frmLoadGRN.DocumentTypeId = 137 (toolStripButton3_Click_1 :2775) - the GRN Direct document. */
    public static final int GRN_TYPE = 137;

    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;

    public PurchaseInvoiceAgainstGrnDirectRepository(JdbcTemplate jdbc, CurrentUserContext context) {
        this.jdbc = jdbc;
        this.context = context;
    }

    private int org() { return context.currentOrganizationId(); }
    private int company() { return context.currentCompanyId(); }

    /** SupplierNameFilll :1226 - CommonServices.SupplierCustomerGetAllServiceBind() -> SupplierCustomer.Getall
     *  (BLL 0600:110) -> Sp_SupplierCustomer_GetAllMethod @Activity='ReadByOrganizationCompanyId'. The same
     *  table feeds Supplier, Commission Agent and Broker (:1229-1231) and the supplier GL lookup (:2928). */
    public List<Map<String, Object>> suppliers() {
        return jdbc.queryForList("EXEC dbo.Sp_SupplierCustomer_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadByOrganizationCompanyId'", org(), company());
    }

    /** bindPaymentTerm :1247 - GetDueTermServiceBind -> Sp_InvDueTerms_GetAllMethod @Activity='GetAll' (Id / TermsDescription). */
    public List<Map<String, Object>> paymentTerms() {
        return jdbc.queryForList("EXEC dbo.Sp_InvDueTerms_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='GetAll'", org(), company());
    }

    /** COAAllocation.GetAccountTitleByAccountTypeIds (BLL 0648:423): @AccountTypeIds / @AccountTypeIdsNot only when
     *  non-empty, @UserId when non-zero. */
    private List<Map<String, Object>> coa(String typeIds, String typeIdsNot) {
        return jdbc.queryForList("EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId=?,@CompanyId=?,@AppId=?,@AccountTypeIds=?,@AccountTypeIdsNot=?,@UserId=?,@Activity='GetAccountTitleByAccountTypeIds'",
                org(), company(), context.currentAppId(),
                new SqlParameterValue(Types.NVARCHAR, typeIds), new SqlParameterValue(Types.NVARCHAR, typeIdsNot), context.currentUserId());
    }

    /** AccountsFill :1264 - CoaAllocationAccountTitleByAccountTypeIds(null, "2,11,12,15") -> every allocated account
     *  EXCEPT types 2, 11, 12, 15; bound Id / AccountTitle to CmbFreightAc. */
    public List<Map<String, Object>> freightAccounts() { return coa(null, "2,11,12,15"); }

    /** CreditAccountForEmptyBag :733 - CoaAllocationAccountTitleByAccountTypeIds("10"); Id / AccountTitle. */
    public List<Map<String, Object>> bagCreditAccounts() { return coa("10", null); }

    /** LocationTypeFill :493 - VoucherHead.GetLocationType -> usp_getLocationType (no parameters). */
    public List<Map<String, Object>> locationTypes() { return jdbc.queryForList("EXEC dbo.usp_getLocationType"); }

    /** CurrencyFill :515 - MultiCurrency.GetAll -> Sp_MultiCurrency_GetAllMethod @Activity='ReadAll'; Id / CurrencyCode. */
    public List<Map<String, Object>> currencies() {
        return jdbc.queryForList("EXEC dbo.Sp_MultiCurrency_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadAll'", org(), company());
    }

    /** OtherItemsBind :1284 - InventoryItemsOther.GetAll (BLL 0573) -> Sp_InventoryItemsOther_GetAllMethod 'ReadAll'. */
    public List<Map<String, Object>> otherItems() {
        return jdbc.queryForList("EXEC dbo.Sp_InventoryItemsOther_GetAllMethod @Activity='ReadAll',@organizationId=?,@CompanyId=?", org(), company());
    }

    /** HistoryComboFill :1312-1343 - Usp_AllComboAgainstPurchaseInvoice @DocumentTypeIds='138', rows whose Activity is
     *  "Supplier" (Id / ReferenceName). */
    public List<Map<String, Object>> historySuppliers() {
        var out = new ArrayList<Map<String, Object>>();
        for (var raw : jdbc.queryForList("EXEC dbo.Usp_AllComboAgainstPurchaseInvoice @OrganizationId=?,@CompanyId=?,@DocumentTypeIds=?", org(), company(), String.valueOf(TYPE))) {
            var r = copy(raw);
            if (!"Supplier".equals(s(r, "Activity"))) continue;
            var m = new LinkedHashMap<String, Object>();
            m.put("Id", i(r, "Id"));
            m.put("Supplier", r.get("ReferenceName"));
            out.add(m);
        }
        return out;
    }

    /** GetLastInvoiceTransportAndBrokerAgentId :1502 (BLL 0581:2049). */
    public Map<String, Object> lastTransportAndBroker() {
        var rows = jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@Activity='GetLastInvoiceTransportAndBrokerAgentId'", org(), company(), TYPE);
        return rows.isEmpty() ? Map.of() : rows.get(0);
    }

    /** cmbCurrency_Leave :560 - VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher (BLL 0654:2679), DocumentTypeIds "138". */
    public List<Map<String, Object>> lastExchangeRate(int currencyId) {
        return jdbc.queryForList("EXEC dbo.Sp_Vouchers_GetMethods @OrganizationId=?,@CompanyId=?,@DocumentTypeIds=?,@DMultiCurrencyIds=?,@Activity='GetMultiCurrencyAndLastRate'",
                org(), company(), String.valueOf(TYPE), String.valueOf(currencyId));
    }

    /** LoadDataDetailGridAgainstGP :1537 - InvPurchaseInvoice.DirectGrnLoadForPurchaseInvoiceDirect (BLL 0581:727). */
    public List<Map<String, Object>> grnRows(String grnIds) {
        return jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @GdnIds=?,@Activity='DirectGrnLoadForPurchaseInvoiceDirect'", grnIds);
    }

    /** LoadEmptyBagsData :1635 - GetEmptyBagsFromGrn(ids, 0) (BLL 0581:842). */
    public List<Map<String, Object>> grnEmptyBags(String grnIds) {
        return jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @GdnIds=?,@OrderId=?,@Activity='GetEmptyBagsFromGrn'", grnIds, 0);
    }

    /** LoadInGridDetail :1602 / ReadById :2177 - USP_GetWagesAmountByRefDocumentTypeIdandRefIds with RefDocumentTypeId 137. */
    public List<Map<String, Object>> wages(String grnIds) {
        return jdbc.queryForList("EXEC dbo.USP_GetWagesAmountByRefDocumentTypeIdandRefIds @OrganizationId=?,@CompanyId=?,@RefDocumentTypeId=?,@RefIds=?", org(), company(), GRN_TYPE, grnIds);
    }

    /** Insert() :2413 - CommonServices.GetUomScheduleByItemId -> Sp_UOMSchedule_GetAllMethod 'ReadByItemID'. */
    public List<Map<String, Object>> uomSchedules(int itemId) {
        return jdbc.queryForList("EXEC dbo.Sp_UOMSchedule_GetAllMethod @OrganizationId=?,@CompanyId=?,@ItemId=?,@Activity='ReadByItemID'", org(), company(), itemId);
    }

    /** VoucherHeadIdGet :1373 - CommonServices.VoucherHeadIdGet(RecId, 138). */
    public int voucherHeadId(int id) {
        var rows = jdbc.queryForList("EXEC dbo.Sp_Vouchers_GetMethods @Activity='GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId',@OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@DocumentTypeSrNo=?", org(), company(), TYPE, id);
        return rows.isEmpty() ? 0 : i(copy(rows.get(0)), "Id");
    }

    /** ReadById :2135 - GetByID -> Sp_InvPurchaseInvoice_GetAllMethod ReadById; DAL 0434 GetDate reads type 138 details with
     *  'DirectPurchaseDetailReadByInvPurchaseInvoiceId' and the expense / empty-bag collections. */
    public Map<String, Object> header(int id) {
        return jdbc.queryForMap("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @Id=?,@Activity='ReadById' WITH RECOMPILE", id);
    }
    public List<Map<String, Object>> read(int id, String activity) {
        return jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @Id=?,@Activity=? WITH RECOMPILE", id, activity);
    }

    /**
     * GetAll :1870-1933 -> InvPurchaseInvoice.FormHistory (BLL 0581:369). Sent exactly as the form sends it:
     * @CanViewAllRecord always, @FinancialYearId, @EntryUser only when the user may NOT view all records, the
     * date pair of the ticked radio (each end only when supplied), @FromDocNo/@ToDocNo/@SupplierCustomerId only
     * when non-zero. The form never sets BranchesIds or NoOfRecords, so neither is sent.
     */
    public List<Map<String, Object>> history(boolean canViewAll, String dateMode, java.sql.Date from, java.sql.Date to,
                                             int fromDocNo, int toDocNo, int supplierId) {
        var sql = new StringBuilder("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@CanViewAllRecord=?,@FinancialYearId=?");
        var args = new ArrayList<Object>(List.of(org(), company(), TYPE, canViewAll, context.currentFinancialYearId()));
        if (!canViewAll) { sql.append(",@EntryUser=?"); args.add(context.currentUserId()); }
        String prefix = switch (dateMode == null ? "" : dateMode) {
            case "entry" -> "Entry";
            case "modify" -> "Modify";
            case "approved" -> "Approved";
            default -> "";
        };
        if (from != null) { sql.append(",@").append(prefix).append("FromDate=?"); args.add(from); }
        if (to != null) { sql.append(",@").append(prefix).append("ToDate=?"); args.add(to); }
        if (fromDocNo != 0) { sql.append(",@FromDocNo=?"); args.add(fromDocNo); }
        if (toDocNo != 0) { sql.append(",@ToDocNo=?"); args.add(toDocNo); }
        if (supplierId != 0) { sql.append(",@SupplierCustomerId=?"); args.add(supplierId); }
        sql.append(",@Activity='FormHistory' WITH RECOMPILE");
        return jdbc.queryForList(sql.toString(), args.toArray());
    }
}
