package com.mst.repositories.cmagt;

import com.mst.models.cmagt.dto.TradeBillAgainstGdnCmagtDto;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.namedparam.MapSqlParameterSource;
import org.springframework.jdbc.core.simple.SimpleJdbcCall;
import org.springframework.stereotype.Repository;
import org.springframework.util.LinkedCaseInsensitiveMap;

import java.time.LocalDate;
import java.util.*;

@Repository
public class TradeBillAgainstGdnCmagtRepository {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    /**
     * DISABLED — this save wrote a Trade Bill with no accounting voucher.
     *
     * ---------------------------------------------------------------------------------------
     * WHAT THE DESKTOP DOES
     * ---------------------------------------------------------------------------------------
     * BLL InvCommAgentTradeBill.Save begins with obj.VoucherHeadInvoices = MakeVoucher(obj),
     * then DAL SetData runs SIXTEEN procedures inside ONE SqlTransaction:
     *
     *   master      USP_InvCommAgentTradeBill_InsertAndUpdate
     *   details     Sp_InvCommAgentTradeBillDetail_Insert
     *   children    Sp_InvCommisionAgentBillPurchaseExpense_Insert
     *               Sp_InvCommisionAgentBillSaleExpense_Insert
     *               Sp_CommisionAgentBillSaleExpenseCreditToReleventAc_Insert
     *               USP_CommisionAgentBillPurchaseFreightExpense_Insert
     *               USP_CommisionAgentBillPaymentDetail_Insert
     *               USP_CommisionAgentBillCommissionDetail_Insert
     *               USP_CommisionAgentBillFreightDetail_Insert
     *               USP_CommisionAgentBillTaxDetail_Insert
     *   attachments Proc_DMSAttachments_Insert
     *   ACCOUNTING  Sp_VoucherHead_Insert / Sp_VoucherHead_Update
     *               Sp_VoucherDetail_Insert
     *               USP_VoucherBalanceCheck
     *               Sp_VoucherHead_H_Insert
     *               Sp_VoucherDetail_H_Insert
     *
     * MakeVoucher composes roughly 30 voucher-detail lines across 12 account sources with
     * debit/credit sign rules. None of that is traced yet.
     *
     * ---------------------------------------------------------------------------------------
     * WHAT THIS CODE DID
     * ---------------------------------------------------------------------------------------
     * Two procedures: the master (15 of its parameters) and the detail rows. No child
     * collections, no attachments, and NO VOUCHER — a grep for VoucherHead / VoucherDetail /
     * VoucherBalance in this file returned zero. Every web-saved Trade Bill therefore existed
     * in the commission ledger with no matching accounting entry, and reported "saved".
     *
     * Refusing is the safer failure, and it is the same position taken for Stock Conversion
     * (STOCK-CONVERSION-READ-SIDE-BUILT-SAVE-REFUSED). Reads on this screen are unaffected.
     */
    public Map<String, Object> saveOrUpdate(TradeBillAgainstGdnCmagtDto dto) {
        throw new UnsupportedOperationException(
                "Trade Bill save is disabled. The desktop posts an accounting voucher in the same "
              + "transaction (MakeVoucher -> Sp_VoucherHead_Insert/_Update, Sp_VoucherDetail_Insert, "
              + "USP_VoucherBalanceCheck, Sp_VoucherHead_H_Insert, Sp_VoucherDetail_H_Insert) plus "
              + "eight child collections. This implementation wrote only the master and its detail "
              + "rows, producing a Trade Bill with no accounting entry. It will be re-enabled once "
              + "MakeVoucher is traced line by line.");
    }


    /* ------------------------------------------------------------------------------------------
     * READ SIDE - BLL 0547_Architecture.BLL.Inventory.InvCommAgentTradeBill.cs and DAL
     * 0402_Architecture.DAL.Inventory.InvCommAgentTradeBill.cs, procedure
     * [dbo].[USP_InvCommAgentTradeBill_GetAllMethods] (23 params, every one defaults to NULL).
     *
     * The previous body sent @Activity='ReadBySearch_InvCommAgentTradeBill' (history) and
     * 'ReadById_InvCommAgentTradeBill' (open). Neither string is an IF branch of the procedure -
     * its branches are GenerateCode, GenerateBranchSrCode, ReadById, nine ReadByHeaderId_*,
     * FormHistory and DeleteById - so the procedure matched nothing and returned no result set:
     * History was always empty and every open said "Record not found".
     * ------------------------------------------------------------------------------------------ */

    private static final String PROC = "USP_InvCommAgentTradeBill_GetAllMethods";

    /**
     * The nine child reads DAL GetDate performs after ReadById (0402 lines 229-272), in order,
     * keyed by the model property the DAL fills. Each is @Id + @Activity only (GetDetail).
     */
    private static final String[][] CHILD_READS = {
            {"InvCommAgentTradeBillDetailslist",                      "ReadByHeaderId_InvCommAgentTradeBillDetail"},
            {"InvCommAgentTradePurchaseExpList",                      "ReadByHeaderId_CommisionAgentBillPurchaseExpense"},
            {"InvCommAgentTradeFreightExpList",                       "ReadByHeaderId_CommisionAgentBillPurchaseFreightExpense"},
            {"InvCommAgentTradeSaleExpList",                          "ReadByHeaderId_CommisionAgentBillSaleExpense"},
            {"CommisionAgentBillSaleExpenseCreditToReleventAcsList",  "ReadByHeaderId_CommisionAgentBillSaleExpenseCreditToReleventAc"},
            {"CommisionAgentBillCommissionDetailList",                "ReadByHeaderId_CommisionAgentBillCommissionDetail"},
            {"CommisionAgentBillPaymentDetailList",                   "ReadByHeaderId_CommisionAgentBillPaymentDetail"},
            {"CommisionAgentBillFreightDetailList",                   "ReadByHeaderId_CommisionAgentBillFreightDetail"},
            {"CommisionAgentBillTaxDetailList",                       "ReadByHeaderId_CommisionAgentBillTaxDetail"},
    };

    /**
     * BLL FormHistory (0547 lines 910-1087), called by frmCommissionAgentTradeBillAgainstGdn
     * HistoryGridFill (5943-6039).
     *
     * Always sent: @OrganizationId, @CompanyId, @DocumentTypeId, @FinancialYearId,
     * @CanViewAllRecord, @Activity. @BranchesId is sent when non-zero (it always is on the
     * desktop). @EntryUserId only when the user lacks "CanView AllRecord". The dates and the
     * doc-no / trading-account / supplier / customer filters only when set.
     *
     * @CanViewAllRecord is the load-bearing one: the procedure's WHERE ends with
     * (@CanViewAllRecord = 1 or (@CanViewAllRecord = 0 and h.EnteryUserId = @EntryUserId)),
     * so a NULL there returns zero rows for everybody. It is declared INT - bound as 1/0.
     */
    public List<Map<String, Object>> formHistory(int organizationId, int companyId, int branchId,
                                                 int financialYearId, int documentTypeId,
                                                 boolean canViewAllRecord, Integer entryUserId,
                                                 String fromDate, String toDate,
                                                 Integer docNoFrom, Integer docNoTo,
                                                 Integer tradingAccountId, Integer supplierId,
                                                 Integer customerId) {
        MapSqlParameterSource p = new MapSqlParameterSource();
        p.addValue("OrganizationId", organizationId);
        p.addValue("CompanyId", companyId);
        p.addValue("DocumentTypeId", documentTypeId);
        p.addValue("FinancialYearId", financialYearId);
        if (branchId != 0) p.addValue("BranchesId", branchId);
        p.addValue("CanViewAllRecord", canViewAllRecord ? 1 : 0);
        if (!canViewAllRecord) p.addValue("EntryUserId", entryUserId);
        java.sql.Date f = parseDate(fromDate);
        java.sql.Date t = parseDate(toDate);
        if (f != null) p.addValue("FromDate", f);
        if (t != null) p.addValue("ToDate", t);
        if (nz(docNoFrom))        p.addValue("DocNoFrom", docNoFrom);
        if (nz(docNoTo))          p.addValue("DocNoTo", docNoTo);
        if (nz(tradingAccountId)) p.addValue("TradingAccountId", tradingAccountId);
        if (nz(supplierId))       p.addValue("SupplierId", supplierId);
        if (nz(customerId))       p.addValue("CustomerId", customerId);
        p.addValue("Activity", "FormHistory");
        return call(p);
    }

    /**
     * BLL GetByID (0547 lines 761-783): @Id + @Activity='ReadById', then DAL GetDate reads the
     * nine child collections. Returns null when the procedure yields no row (deleted - the
     * branch filters ActionId <> 3 - or not found).
     *
     * The ReadById SELECT carries placeholder columns ('' AS InvCommAgentTradeBillDetailslist,
     * ... ) for every child list; the header is kept case-insensitive so the real lists replace
     * those placeholders instead of sitting beside them under a second casing.
     */
    public Map<String, Object> readById(int id) {
        MapSqlParameterSource p = new MapSqlParameterSource();
        p.addValue("Id", id);
        p.addValue("Activity", "ReadById");
        List<Map<String, Object>> rows = call(p);
        if (rows.isEmpty()) return null;

        Map<String, Object> header = new LinkedCaseInsensitiveMap<>();
        header.putAll(rows.get(0));
        for (String[] child : CHILD_READS) {
            MapSqlParameterSource cp = new MapSqlParameterSource();
            cp.addValue("Id", id);
            cp.addValue("Activity", child[1]);
            header.put(child[0], call(cp));
        }
        return header;
    }

    /**
     * BLL GenerateCode / GenerateBranchSrCode (0547 lines 810-908). Both read column DocNo.
     * DocumentNoDbCall / BranchSrNoDbCall in the form (1149-1199).
     */
    public int generateCode(String activity, int organizationId, int companyId,
                            int documentTypeId, int financialYearId, int branchId) {
        MapSqlParameterSource p = new MapSqlParameterSource();
        p.addValue("OrganizationId", organizationId);
        p.addValue("CompanyId", companyId);
        p.addValue("DocumentTypeId", documentTypeId);
        p.addValue("FinancialYearId", financialYearId);
        p.addValue("BranchesId", branchId);
        p.addValue("Activity", activity);
        List<Map<String, Object>> rows = call(p);
        if (rows.isEmpty()) return 0;
        Object v = rows.get(0).get("DocNo");
        return v instanceof Number ? ((Number) v).intValue() : 0;
    }

    /**
     * BLL DeleteByID (0547 lines 785-808): @EntryUserId, @Id, @Activity='DeleteById', one
     * transaction. The procedure itself refuses an approved bill (RAISERROR), soft-deletes the
     * header and detail (ActionId / ActionTypeId = 3), removes attachments, inventory rows and
     * the VoucherHead/VoucherDetail for this document, and writes the user-audit row.
     */
    public void deleteById(int entryUserId, int id) {
        MapSqlParameterSource p = new MapSqlParameterSource();
        p.addValue("EntryUserId", entryUserId);
        p.addValue("Id", id);
        p.addValue("Activity", "DeleteById");
        new SimpleJdbcCall(jdbcTemplate).withProcedureName(PROC).execute(p);
    }

    private List<Map<String, Object>> call(MapSqlParameterSource p) {
        return extractList(new SimpleJdbcCall(jdbcTemplate).withProcedureName(PROC).execute(p));
    }

    @SuppressWarnings("unchecked")
    private List<Map<String, Object>> extractList(Map<String, Object> out) {
        for (Object val : out.values()) {
            if (val instanceof List) {
                return (List<Map<String, Object>>) val;
            }
        }
        return Collections.emptyList();
    }

    private static boolean nz(Integer v) { return v != null && v != 0; }

    /**
     * A blank date is OMITTED, as the desktop omits an unchecked date picker. It used to become
     * "today", silently narrowing the history to one day; an unparseable value did the same.
     */
    private static java.sql.Date parseDate(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        try {
            return java.sql.Date.valueOf(LocalDate.parse(s.trim().substring(0, Math.min(10, s.trim().length()))));
        } catch (Exception e) {
            throw new IllegalArgumentException("Invalid date '" + s + "' - expected yyyy-MM-dd");
        }
    }
}
