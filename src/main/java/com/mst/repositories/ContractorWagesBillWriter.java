package com.mst.repositories;

import com.mst.repositories.support.ProcExec;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCallback;
import org.springframework.stereotype.Repository;

import java.sql.ResultSet;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Objects;

/**
 * Labour Wages (Contractor Wages Bill) writer.
 *
 * This reproduces, step for step, two pieces of desktop code:
 *
 *   Architecture.DAL.ContractorWages.InvContractorWagesBillHeader.SetData(obj, ProcName)
 *   Architecture.BLL.ContractorWages.InvContractorWagesBillHeader.MakeVoucher(obj)
 *
 * The caller owns the transaction. Nothing here touches a table directly - every write goes through
 * the same stored procedure the desktop calls, in the same order:
 *
 *   1. Sp_InvContractorWagesBillHeader_Insert | _Update              -> header id
 *   2. Sp_InvContractorWagesBillDetail_Insert  per detail row        (always Insert, exactly as the
 *      desktop does - the Update procedure owns clearing the old children)
 *   3. voucher branch:
 *        rows present -> Sp_Vouchers_GetMethods
 *                        @Activity='GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId'
 *                        Sp_VoucherHead_Insert | _Update
 *                        Sp_VoucherDetail_Insert per line
 *                        USP_VoucherBalanceCheck
 *                        Sp_VoucherHead_H_Insert + Sp_VoucherDetail_H_Insert (audit copies)
 *        no rows      -> usp_ContractorWagesVoucherDeleteByWagesId
 *   4. RefDocumentTypeId 806 or 68 -> [dbo].[usp_WagesProportionateToStockEvaluation]
 *
 * The parameter lists are not guesses: GenericProvider.SetProc binds one @PropertyName per
 * non-virtual property of the matching model, and those models declare exactly the properties
 * listed in headerParams()/detailParams() below.
 */
@Repository
public class ContractorWagesBillWriter {

    private final JdbcTemplate jdbc;

    public ContractorWagesBillWriter(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ================================================================= parameter maps

    /** Architecture.Model.ContractorWages.InvContractorWagesBillHeader - 26 non-virtual properties. */
    public static Map<String, Object> headerParams() {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("IsAproved", false);
        p.put("ApprovedDate", null);
        p.put("DocDate", null);
        p.put("EntryDate", null);
        p.put("ModifyDate", null);
        p.put("WeightTotal", 0d);
        p.put("QtyTotal", 0d);
        p.put("ApprovedUserId", 0);
        p.put("BranchesId", 0);
        p.put("CompanyId", 0);
        p.put("FinancialYearId", 0);
        p.put("DocNo", 0);
        p.put("DocumentTypeId", 0);
        p.put("EntryUser", 0);
        p.put("Id", 0);
        p.put("ModifyUser", 0);
        p.put("OrganizationId", 0);
        p.put("ProjectsId", 0);
        p.put("RefDocNo", 0);
        p.put("RefDocNoId", 0);
        p.put("RefDocumentTypeId", 0);
        p.put("StockPartyId", 0);
        p.put("JobOrderId", 0);
        p.put("ScaleSlipNo", 0);
        p.put("OtherRemarks", "");
        p.put("RefDocument", "");
        return p;
    }

    /** Architecture.Model.ContractorWages.InvContractorWagesBillDetail - 31 non-virtual properties. */
    public static Map<String, Object> detailParams() {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("RefDocDate", null);
        p.put("BillWeight", 0d);
        p.put("WageRate", 0d);
        p.put("RateAddLess", 0d);
        p.put("WagesAmount", 0d);
        p.put("Weight", 0d);
        p.put("WeightCut", 0d);
        p.put("BillQty", 0d);
        p.put("Qty", 0d);
        p.put("RefDocQty", 0d);
        p.put("RefDocWeight", 0d);
        p.put("FreeOfCost", false);
        p.put("IsCompany", false);
        p.put("RefLineId", 0);
        p.put("ContractorId", 0);
        p.put("JobOrderId", 0);
        p.put("RefDocumentTypeId", 0);
        p.put("Id", 0);
        p.put("InvConractorWagesAccountsId", 0);
        p.put("InvContractorWagesBillHeaderId", 0);
        p.put("InvPackingTypeId", 0);
        p.put("ItemId", 0);
        p.put("JobLotId", 0);
        p.put("PackSize", 0d);
        p.put("WagesTypeId", 0);
        p.put("WareHouseFromId", 0);
        p.put("WareHouseToId", 0);
        p.put("WbTransactionsIdDt", 0);
        p.put("InvContractorWagesScheduleId", 0);
        p.put("Crop", "");
        p.put("RemarksDetail", "");
        return p;
    }

    // ================================================================= save

    /**
     * @param header     header parameter map, already populated by the service
     * @param details    detail parameter maps, in grid order
     * @param isUpdate   true -> _Update, false -> _Insert
     * @return the header id
     */
    public int save(Map<String, Object> header, List<Map<String, Object>> details, boolean isUpdate) {
        int orgId = i(header.get("OrganizationId"));
        int compId = i(header.get("CompanyId"));
        int documentTypeId = i(header.get("DocumentTypeId"));
        int refDocumentTypeId = i(header.get("RefDocumentTypeId"));

        int headerId = scalar("Sp_InvContractorWagesBillHeader_" + (isUpdate ? "Update" : "Insert"),
                              header, i(header.get("Id")));
        if (headerId <= 0) {
            throw new IllegalStateException("The wages bill did not return a record ID.");
        }
        header.put("Id", headerId);

        for (Map<String, Object> d : details) {
            d.put("InvContractorWagesBillHeaderId", headerId);
            /* WagesAccountName and ItemName travel on the row for MakeVoucher's comment text
               (BLL :224) but are NOT parameters of Sp_InvContractorWagesBillDetail_Insert - the
               model declares them virtual - so they are dropped for the procedure call only. */
            Map<String, Object> forProc = new LinkedHashMap<>(d);
            forProc.remove("WagesAccountName");
            forProc.remove("ItemName");
            scalar("Sp_InvContractorWagesBillDetail_Insert", forProc, 0);
        }

        List<Map<String, Object>> voucherLines = buildVoucherLines(header, details);

        if (!voucherLines.isEmpty()) {
            Map<String, Object> v = buildVoucherHead(header);

            int voucherId = 0;
            List<Map<String, Object>> existing = jdbc.queryForList(
                    "EXEC dbo.Sp_Vouchers_GetMethods @Activity=?, @OrganizationId=?, @CompanyId=?, "
                  + "@DocumentTypeId=?, @DocumentTypeSrNo=?",
                    "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId",
                    orgId, compId, documentTypeId, headerId);
            if (!existing.isEmpty() && existing.get(0).get("Id") instanceof Number) {
                voucherId = ((Number) existing.get(0).get("Id")).intValue();
            }

            v.put("Id", voucherId);
            v.put("DocumentTypeSrNo", headerId);
            voucherId = scalar("Sp_VoucherHead_" + (voucherId == 0 ? "Insert" : "Update"), v, voucherId);
            v.put("Id", voucherId);

            for (Map<String, Object> line : voucherLines) {
                line.put("VoucherHeadId", voucherId);
                scalar("Sp_VoucherDetail_Insert", line, 0);
            }

            ProcExec.call(jdbc, "EXEC dbo.USP_VoucherBalanceCheck @OrganizationId=?, @CompanyId=?, @Id=?",
                        orgId, compId, voucherId);

            int audit = scalar("Sp_VoucherHead_H_Insert", v, 0);
            for (Map<String, Object> line : voucherLines) {
                line.put("VoucherHeadId", voucherId);
                line.put("DocumentTypeIdRef", audit);
                scalar("Sp_VoucherDetail_H_Insert", line, 0);
            }
        } else {
            ProcExec.call(jdbc, "EXEC dbo.usp_ContractorWagesVoucherDeleteByWagesId "
                      + "@OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @Id=?",
                        orgId, compId, documentTypeId, headerId);
        }

        if (refDocumentTypeId == 806 || refDocumentTypeId == 68) {
            ProcExec.call(jdbc, "EXEC [dbo].[usp_WagesProportionateToStockEvaluation] "
                      + "@OrganizationId=?, @CompanyId=?, @RefDocumentTypeId=?, @RefDocNoId=?",
                        orgId, compId, refDocumentTypeId, i(header.get("RefDocNoId")));
        }

        return headerId;
    }

    // ================================================================= MakeVoucher

    /** MakeVoucher(), BLL :20-44 - the voucher header fields it sets, over the shared defaults. */
    private Map<String, Object> buildVoucherHead(Map<String, Object> h) {
        Map<String, Object> v = InventoryOpeningDefaults.voucher();
        Timestamp now = new Timestamp(System.currentTimeMillis());
        v.put("DocumentTypeId", h.get("DocumentTypeId"));
        v.put("DocumentTypeSrNo", h.get("Id"));
        v.put("RefDocNoId", h.get("Id"));
        v.put("VoucherCode", i(h.get("DocNo")));
        v.put("VoucherDate", h.get("DocDate"));
        v.put("Remarks", Objects.toString(h.get("OtherRemarks"), ""));
        v.put("RemarksOtherLingo", "");
        v.put("ChequeDate", new java.sql.Date(System.currentTimeMillis()));
        v.put("IncludeWHT", false);
        v.put("BranchId", h.get("BranchesId"));
        v.put("ProjectId", h.get("ProjectsId"));
        v.put("BillAmount", 0d);
        v.put("ManualBillNo", String.valueOf(i(h.get("RefDocNo"))));
        v.put("DueDate", h.get("DocDate"));
        v.put("DueDays", 0);
        v.put("OrganizationId", h.get("OrganizationId"));
        v.put("CompanyId", h.get("CompanyId"));
        v.put("FinancialYearId", h.get("FinancialYearId"));
        v.put("EntryUser", h.get("EntryUser"));
        v.put("EntryDate", now);
        v.put("ModifyDate", now);
        v.put("ModifyUser", h.get("ModifyUser"));
        return v;
    }

    /**
     * MakeVoucher(), BLL :50-355. Two branches, reproduced as written:
     *
     *   DocumentTypeId 101 or 810 - debit account resolved through four alternatives in the
     *     desktop's own order (WIP account, item purchase GL, forwarding contract GL, wages account
     *     GL), then one debit line and one credit line per non-free-of-cost detail row. A detail
     *     whose contractor has no GL account is SKIPPED, not rejected (BLL :211-214).
     *
     *   DocumentTypeId 219 - debit is the wages account GL when IsCompany, otherwise the stock
     *     party's GL; a missing contractor GL here DOES throw (BLL :337).
     *
     * Free-of-cost rows are skipped entirely in the first branch (BLL :54-57).
     */
    private List<Map<String, Object>> buildVoucherLines(Map<String, Object> h, List<Map<String, Object>> details) {
        List<Map<String, Object>> lines = new ArrayList<>();
        if (details == null || details.isEmpty()) return lines;

        int orgId = i(h.get("OrganizationId"));
        int compId = i(h.get("CompanyId"));
        int documentTypeId = i(h.get("DocumentTypeId"));
        int refDocumentTypeId = i(h.get("RefDocumentTypeId"));
        int jobOrderId = i(h.get("JobOrderId"));
        int stockPartyId = i(h.get("StockPartyId"));
        String refDocument = Objects.toString(h.get("RefDocument"), "");

        Map<Integer, Integer> partyGl = supplierCustomerGlAccounts(orgId, compId);

        if (documentTypeId == 101 || documentTypeId == 810) {
            boolean chargeToWip = false, chargeToProduct = false, chargeToContract = false;
            if (documentTypeId != 219 && (refDocumentTypeId == 80 || refDocumentTypeId == 112)) {
                chargeToWip = configBool(orgId, compId, "ProductionWagesChargeToWIPAccount");
            } else if (documentTypeId == 101 && refDocumentTypeId == 205) {
                chargeToContract = configBool(orgId, compId, "LabourWagesChargedtoContractAccountonForwarding");
            } else if (documentTypeId == 101
                    && (refDocumentTypeId == 46 || refDocumentTypeId == 68 || refDocumentTypeId == 806)) {
                chargeToProduct = configBool(orgId, compId, "ContractWagesChargetoProduct");
            }

            Map<Integer, Integer> itemPurchaseGl = chargeToProduct ? itemPurchaseGlAccounts(orgId, compId) : null;
            Map<Integer, Integer> wagesAccountGl = null;

            for (Map<String, Object> d : details) {
                if (b(d.get("FreeOfCost"))) continue;               // BLL :54-57

                int debit = 0;

                if (chargeToWip && (refDocumentTypeId == 80 || refDocumentTypeId == 112 || refDocumentTypeId == 181)) {
                    if (documentTypeId == 101) {
                        debit = jobOrderAccount(orgId, compId, i(h.get("RefDocNoId")),
                                                "GetWipAccountsByProductionId", "WIPAccountId");
                    } else {
                        debit = jobOrderAccount(orgId, compId, jobOrderId,
                                                "GetGlAccountsByJobOrderId", "WorkInProccessAcId");
                    }
                }

                if (chargeToProduct) {
                    Integer gl = itemPurchaseGl.get(i(d.get("ItemId")));
                    if (gl == null) throw new IllegalStateException("ItemId Not Found");
                    debit = gl;
                }

                if (chargeToContract) {
                    List<Map<String, Object>> rows = jdbc.queryForList(
                            "EXEC dbo.usp_getExportInvoiceDataByForwardingId @OrganizationId=?, @CompanyId=?, @Id=?",
                            orgId, compId, i(h.get("RefDocNoId")));
                    if (rows.isEmpty()) {
                        throw new IllegalStateException("Contract Credit Account Not found against Forwarding");
                    }
                    debit = i(rows.get(0).get("ContractGLAcId"));
                }

                if (debit == 0) {
                    if (wagesAccountGl == null) wagesAccountGl = wagesAccountGlAccounts(orgId, compId);
                    if (wagesAccountGl.isEmpty()) {
                        throw new IllegalStateException("Contactor Wages Account Id Not Found");
                    }
                    Integer gl = wagesAccountGl.get(i(d.get("InvConractorWagesAccountsId")));
                    if (gl == null) throw new IllegalStateException("Contactor Wages Account Id Not Found");
                    debit = gl;
                }

                if (debit == 0) throw new IllegalStateException("Debit AccountId Not Found");

                Integer contractorGl = partyGl.get(i(d.get("ContractorId")));
                if (contractorGl == null) continue;                 // BLL :211-214 - skipped, not rejected

                if (documentTypeId == 810 && !b(d.get("IsCompany"))) {
                    Integer spGl = partyGl.get(stockPartyId);
                    if (spGl == null) {
                        throw new IllegalStateException("StockParty GlAccountId not found against " + stockPartyId);
                    }
                    debit = spGl;
                }

                addPair(lines, d, debit, contractorGl, refDocument, jobOrderId, b(d.get("FreeOfCost")));
            }
            return lines;
        }

        if (documentTypeId == 219) {
            if (stockPartyId == 0) throw new IllegalStateException("StockPartyId cannot be equal to zero");
            Map<Integer, Integer> wagesAccountGl = wagesAccountGlAccounts(orgId, compId);

            for (Map<String, Object> d : details) {
                Integer contractorGl = partyGl.get(i(d.get("ContractorId")));
                if (contractorGl == null) {
                    throw new IllegalStateException("Contactor GlAccountId not found against " + i(d.get("ContractorId")));
                }
                int debit;
                if (b(d.get("IsCompany"))) {
                    if (wagesAccountGl.isEmpty()) {
                        throw new IllegalStateException("Contactor Wages Accounts data is empty");
                    }
                    Integer gl = wagesAccountGl.get(i(d.get("InvConractorWagesAccountsId")));
                    if (gl == null) {
                        throw new IllegalStateException("Activity GlAccountId not found against "
                                                       + i(d.get("InvConractorWagesAccountsId")));
                    }
                    debit = gl;
                } else {
                    Integer spGl = partyGl.get(stockPartyId);
                    if (spGl == null) {
                        throw new IllegalStateException("StockParty GlAccountId not found against " + stockPartyId);
                    }
                    debit = spGl;
                }
                if (debit == 0) {
                    throw new IllegalStateException("DebitAccountId cannot be equal to zero for Party Processing");
                }
                /* This branch posts the full amount even for a free-of-cost row - the desktop's 219
                   branch has no FreeOfCost test (BLL :318-340). */
                addPair(lines, d, debit, contractorGl, refDocument, jobOrderId, false);
            }
        }
        return lines;
    }

    /** The debit/credit pair and the comment string, byte for byte as the desktop composes it. */
    private void addPair(List<Map<String, Object>> lines, Map<String, Object> d,
                         int debitAccount, int creditAccount, String refDocument,
                         int jobOrderId, boolean freeOfCost) {
        double amount = dbl(d.get("WagesAmount"));
        double rate = dbl(d.get("WageRate"));
        String comments = refDocument + "  :" + s(d.get("WagesAccountName"))
                + " Item Name :" + s(d.get("ItemName"))
                + "  Wages Qty :  " + num(d.get("Qty"))
                + "  Wages Rate :  " + num(d.get("WageRate"))
                + "  Wages Amount :  " + num(d.get("WagesAmount"))
                + "  BillWeight :  " + num(d.get("BillWeight"));

        Map<String, Object> dr = InventoryOpeningDefaults.detail();
        dr.put("AccountId", debitAccount);
        dr.put("AgainstAccountId", creditAccount);
        dr.put("Comments", comments);
        dr.put("DebitAmount", freeOfCost ? 0d : amount);
        dr.put("ItemAmount", freeOfCost ? 0d : amount);
        dr.put("CreditAmount", 0d);
        dr.put("ItemRate", rate);
        dr.put("OrderNo", jobOrderId);
        lines.add(dr);

        Map<String, Object> cr = InventoryOpeningDefaults.detail();
        cr.put("AccountId", creditAccount);
        cr.put("AgainstAccountId", debitAccount);
        cr.put("Comments", comments);
        cr.put("DebitAmount", 0d);
        cr.put("CreditAmount", freeOfCost ? 0d : amount);
        cr.put("ItemAmount", freeOfCost ? 0d : amount);
        cr.put("ItemRate", rate);
        cr.put("OrderNo", jobOrderId);
        lines.add(cr);
    }

    // ================================================================= lookups used by MakeVoucher

    /** CommonServies.GetSupplierCustomerListForFinancialEffects - Sp_SupplierCustomer_GetAllMethod. */
    private Map<Integer, Integer> supplierCustomerGlAccounts(int orgId, int compId) {
        Map<Integer, Integer> m = new LinkedHashMap<>();
        for (Map<String, Object> r : jdbc.queryForList(
                "EXEC dbo.Sp_SupplierCustomer_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                orgId, compId, "GetGlAccountIdandCompanyNameBySupplierCustomerId")) {
            m.put(i(r.get("Id")), i(r.get("GlAccountId")));
        }
        return m;
    }

    /** CommonServies.GetItemListForFinancialEffects - Sp_Item_GetAllMethod. */
    private Map<Integer, Integer> itemPurchaseGlAccounts(int orgId, int compId) {
        Map<Integer, Integer> m = new LinkedHashMap<>();
        for (Map<String, Object> r : jdbc.queryForList(
                "EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                orgId, compId, "GetItemGlIdsandItemName")) {
            m.put(i(r.get("Id")), i(r.get("PurchaseGLAC")));
        }
        return m;
    }

    /** Sp_InvConractorWagesAccounts_GetAllMethod @Activity='ReadAll'. */
    private Map<Integer, Integer> wagesAccountGlAccounts(int orgId, int compId) {
        Map<Integer, Integer> m = new LinkedHashMap<>();
        for (Map<String, Object> r : jdbc.queryForList(
                "EXEC dbo.Sp_InvConractorWagesAccounts_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                orgId, compId, "ReadAll")) {
            m.put(i(r.get("Id")), i(r.get("GlAccountId")));
        }
        return m;
    }

    private int jobOrderAccount(int orgId, int compId, int id, String activity, String column) {
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC dbo.Sp_InvProductionJobOrder_GetAllMethod @OrganizationId=?, @CompanyId=?, @Id=?, @Activity=?",
                orgId, compId, id, activity);
        if (rows.isEmpty()) {
            throw new IllegalStateException("Work in process Account not found against JobOrder");
        }
        return i(rows.get(0).get(column));
    }

    /** CommonServices.GetConfigurationFromAllocation - the project's established config read. */
    private boolean configBool(int orgId, int compId, String configDescription) {
        try {
            List<Map<String, Object>> rows = jdbc.queryForList(
                    "EXEC Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, "
                  + "@ConfigDescription=?, @DefinitionIds=?, @Activity=?",
                    orgId, compId, configDescription, null,
                    "GetConfigurationByOrgCompandConfigDescription");
            if (rows.isEmpty()) return false;
            Object v = rows.get(0).get("ConfigKey");
            if (v == null) return false;
            String s = String.valueOf(v).trim();
            return "1".equals(s) || "true".equalsIgnoreCase(s) || "yes".equalsIgnoreCase(s);
        } catch (Exception e) {
            return false;
        }
    }

    // ================================================================= helpers

    private int scalar(String procedure, Map<String, Object> values, int fallback) {
        String sql = "EXEC dbo." + procedure + " "
                   + String.join(", ", values.keySet().stream().map(k -> "@" + k + "=?").toList());
        return jdbc.execute(sql, (PreparedStatementCallback<Integer>) statement -> {
            int i = 1;
            for (Object value : values.values()) statement.setObject(i++, value);
            boolean result = statement.execute();
            Integer first = null;
            // Drain every result so an error after SELECT @Id cannot accidentally be missed.
            while (true) {
                if (result) {
                    try (ResultSet rs = statement.getResultSet()) {
                        if (first == null && rs.next() && rs.getObject(1) instanceof Number) {
                            first = ((Number) rs.getObject(1)).intValue();
                        }
                    }
                } else if (statement.getUpdateCount() == -1) break;
                result = statement.getMoreResults();
            }
            return first != null && first > 0 ? first : fallback;
        });
    }

    private static int i(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).intValue();
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0; }
    }

    private static double dbl(Object o) {
        if (o == null) return 0d;
        if (o instanceof Number) return ((Number) o).doubleValue();
        try { return Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0d; }
    }

    private static boolean b(Object o) {
        if (o == null) return false;
        if (o instanceof Boolean) return (Boolean) o;
        String s = String.valueOf(o).trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s);
    }

    private static String s(Object o) { return o == null ? "" : String.valueOf(o); }

    /** C# double.ToString() drops a trailing ".0"; match that so the comment text is identical. */
    private static String num(Object o) {
        double d = dbl(o);
        return (d == Math.rint(d) && !Double.isInfinite(d))
                ? String.valueOf((long) d)
                : String.valueOf(d);
    }
}
