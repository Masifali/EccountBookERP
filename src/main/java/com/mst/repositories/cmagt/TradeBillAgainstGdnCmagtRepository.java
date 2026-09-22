package com.mst.repositories.cmagt;

import com.mst.models.cmagt.dto.TradeBillAgainstGdnCmagtDto;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.namedparam.MapSqlParameterSource;
import org.springframework.jdbc.core.simple.SimpleJdbcCall;
import org.springframework.stereotype.Repository;

import java.math.BigDecimal;
import java.text.SimpleDateFormat;
import java.util.*;

@Repository
public class TradeBillAgainstGdnCmagtRepository {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    private static final SimpleDateFormat DATE_FORMAT = new SimpleDateFormat("yyyy-MM-dd");

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


    public List<Map<String, Object>> getHistory(Integer companyId, Integer organizationId, String fromDate, String toDate) {
        SimpleJdbcCall call = new SimpleJdbcCall(jdbcTemplate)
                .withProcedureName("USP_InvCommAgentTradeBill_GetAllMethods");

        MapSqlParameterSource params = new MapSqlParameterSource();
        params.addValue("Activity", "ReadBySearch_InvCommAgentTradeBill");
        /* The controller passes the session's own values. The ': 1' fallbacks that
           used to sit here would have quietly widened a read to company 1 if one
           ever arrived null, hiding the fault instead of surfacing it. */
        params.addValue("CompanyId", companyId);
        params.addValue("OrganizationId", organizationId);
        params.addValue("DocumentTypeId", 1056);
        params.addValue("FromDate", parseDate(fromDate));
        params.addValue("ToDate", parseDate(toDate));

        Map<String, Object> out = call.execute(params);
        return extractList(out);
    }

    public Map<String, Object> getById(Integer id) {
        Map<String, Object> result = new HashMap<>();

        SimpleJdbcCall headerCall = new SimpleJdbcCall(jdbcTemplate)
                .withProcedureName("USP_InvCommAgentTradeBill_GetAllMethods");

        MapSqlParameterSource headerParams = new MapSqlParameterSource();
        headerParams.addValue("Activity", "ReadById_InvCommAgentTradeBill");
        headerParams.addValue("Id", id);

        Map<String, Object> headerOut = headerCall.execute(headerParams);
        List<Map<String, Object>> headers = extractList(headerOut);

        if (headers != null && !headers.isEmpty()) {
            Map<String, Object> header = new HashMap<>(headers.get(0));

            SimpleJdbcCall detCall = new SimpleJdbcCall(jdbcTemplate)
                    .withProcedureName("USP_InvCommAgentTradeBill_GetAllMethods");
            MapSqlParameterSource detParams = new MapSqlParameterSource();
            detParams.addValue("Activity", "ReadByHeaderId_InvCommAgentTradeBillDetail");
            detParams.addValue("Id", id);
            header.put("invCommAgentTradeBillDetailslist", extractList(detCall.execute(detParams)));

            result.put("status", "SUCCESS");
            result.put("data", header);
        } else {
            result.put("status", "ERROR");
            result.put("message", "Record not found");
        }
        return result;
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

    private Integer extractReturnedId(Map<String, Object> out) {
        for (Object val : out.values()) {
            if (val instanceof Integer) return (Integer) val;
            if (val instanceof Number) return ((Number) val).intValue();
        }
        return null;
    }

    private Date parseDate(String dateStr) {
        if (dateStr == null || dateStr.trim().isEmpty()) return new Date();
        try {
            return DATE_FORMAT.parse(dateStr);
        } catch (Exception e) {
            return new Date();
        }
    }
}
