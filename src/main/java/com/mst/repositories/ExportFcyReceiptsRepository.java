package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.Map;

import static com.mst.repositories.ExportGdBreakUpRepository.ci;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * 794 "Fcy Receipts" - Architecture.WinApp.Account_Definition.Acfrmfcbankreceipt (DocumentTypeId 203).
 * Data layer: BLL 0464 / DAL 0515 ExImFCBankReceipts, BLL 0467 ExImInvoice, BLL 0469 ExImLcOrder,
 * SupplierCustomer, MultiCurrency, ExImProceedsChargesType, COAAllocation, Branches, Projects, VoucherHead.
 * Every call is the desktop's own procedure with the desktop's own parameters (procdure.utf8.sql):
 *
 *   Sp_ExImFCBankReceipts_GetAllMethod   @OrganizationId @CompanyId @DocumentTypeId @FinancialYearId @Activity='GenerateDocNo'   txtdocNumber
 *                                        @Id @Activity='ReadById' | 'ReadByHeaderId' | 'ReadBreakupByHeaderId' | 'ReadPartyBreakupByHeaderId'
 *                                            | 'ReadExImShipmentPartyAgainstAdvancesPaymentByHeaderId'                            GetByID (DAL GetDate)
 *                                        @OrganizationId @CompanyId @Activity='GetReferenceNo'                                 cmbReferenceNo
 *   usp_getFcyReceiptsPaymentType        (none)                                                                               cmbPaymentTerm
 *   Sp_SupplierCustomer_GetAllMethod     @OrganizationId @CompanyId @Activity='ReadByOrganizationCompanyIdForExport'          Other / Multi customers; MakeVoucher GlAccountId
 *   Sp_ExImLcOrder_GetAllMethod          @OrganizationId @CompanyId @Activity='GetExportCustomerByAdvancePayment'             Advance customers / Consignee
 *                                        @OrganizationId @CompanyId [@FinancialYearId] [@SupCustId] @Activity='GetLcOrderNoBySupplierCustomerId'   cmbLCOrder
 *   Sp_ExImInvoice_GetAllMethod          @OrganizationId @CompanyId [@SupplierCustomerId] [@FinancialYearId] @Activity='GetInvoiceNoandPartiesForFcBankReceipts'
 *                                        @OrganizationId @CompanyId @Id @Activity='GetFcYAmount' | 'GetExchangeRateandCurrencyFromVoucher' | 'GetFcYAmountByContractId'
 *   USP_GetCommercialInvoicesaginstPreInvoices  @OrganizationId @CompanyId                                                    cmbInvoicenobreakup
 *   USP_GetFcyBankInvoicesBalance        @OrganizationId @CompanyId @Id
 *   USP_GetGDsAgainstAdvancePaymentUtilizeInInvoice  @OrganizationId @CompanyId [@Id] [@RecId] [@RefDocumentTypeId]            cmbGdNoBreakup
 *   usp_getPaymentTermDetailByInvoiceId  @InvoiceId [@FcyReceiptId]                                                           grdPaymentTerms
 *   Sp_MultiCurrency_GetAllMethod        @OrganizationId @CompanyId @Activity='ReadAll'
 *   Sp_ExImProceedsChargesType_GetAllMethod  @OrganizationId @CompanyId @Activity='ReadByOrganizationCompanyId'
 *   Sp_COAAllocation_GetAllMethod        @OrganizationId @CompanyId [@UserId] @Activity='COAAllocationSearch'                 dtaccounts (Account (Dr))
 *                                        @OrganizationId @CompanyId @AppId [@AccountTypeIds] [@AccountTypeIdsNot] [@UserId] @Activity='GetAccountTitleByAccountTypeIds'
 *   Sp_Branches_GetAllMethod             @OrganizationId @CompanyId @Activity='GetAll'                                         BrancheId (first row)
 *   Sp_Projects_GetAllMethod             @OrganizationId @CompanyId @MethodType='GetAll'                                       ProjectId (first row)
 *   USP_GetDataForDropDownFromFcyBankReceipts  @OrganizationId @CompanyId                                                      history combos
 *   USP_ExImFCBankReceipts_FormHistory   guarded, see history()
 *   Sp_Vouchers_GetMethods               @Activity='GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId' @OrganizationId @CompanyId @DocumentTypeId @DocumentTypeSrNo
 *   USP_GetFcyAndLcyBalance              @OrganizationId @CompanyId @FcyId @ToDate [@SupplierCustomerId] [@Id @DocumentTypeId]   MakeVoucher (auto gain/loss)
 *   USP_GetPendingPartyInvoices          @OrganizationId @CompanyId @CustomerId @FcyId [@FcyBankReceiptId]                     MakeVoucher FIFO
 *   USP_GetGainAndLossBreackup           @Id                                                                                  G&LBreakup
 *   Sp_ConfigrationsAllocation_GetAllMethod  'FCYReceiptAutoGainAndLoss'
 *   save (DAL SetDate, one transaction): Sp_ExImFCBankReceipts_Insert | _Update, Sp_ExImFcBankReceiptsDeductions_Insert,
 *     [dbo].[USP_FcyBankReceiptBreakUp_Insert], [dbo].[USP_FcyBankReceiptBreakUpofPartyInvoices_Insert], [dbo].[Sp_GainLossBreakup_Insert],
 *     USP_FcyPartyPaymentBreackUpForFinancials_Insert, USP_ExImShipmentPartyAgainstAdvancesPayment_Insert,
 *     usp_FcyBankReceiptPaymentTermDetail_Insert, USP_ExportAdvanceFinancialInstumentUtilizeValidation (@InvoiceId @GdId @RefDocumentTypeId),
 *     Sp_Vouchers_GetMethods, Sp_VoucherHead_Insert | Sp_VoucherHead_Update, Sp_VoucherDetail_Insert, USP_VoucherBalanceCheck,
 *     Sp_VoucherHead_H_Insert, Sp_VoucherDetail_H_Insert.
 *
 * No table, column or procedure is created or changed.
 */
@Repository
public class ExportFcyReceiptsRepository {

    private final JdbcTemplate jdbc;

    public ExportFcyReceiptsRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private Map<String, Object> oc(UserAccount u) { return params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()); }

    public String config(UserAccount u, String description) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (r.isEmpty()) return "";
        Object v = r.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    /** ExImFCBankReceipts.GenerateCode - Rows[0]["DocNo"]. */
    public int generateCode(UserAccount u, int financialYearId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ExImFCBankReceipts_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", 203,
                "FinancialYearId", financialYearId, "Activity", "GenerateDocNo"));
        if (r.isEmpty()) throw new IllegalStateException("There is no row at position 0.");
        Object v = ci(r.get(0), "DocNo");
        return v instanceof Number ? ((Number) v).intValue() : 0;
    }

    public List<Map<String, Object>> paymentTypes() { return DesktopProc.rows(jdbc, "usp_getFcyReceiptsPaymentType", params()); }

    public List<Map<String, Object>> referenceNos(UserAccount u) {
        Map<String, Object> p = oc(u); p.put("Activity", "GetReferenceNo");
        return DesktopProc.rows(jdbc, "Sp_ExImFCBankReceipts_GetAllMethod", p);
    }

    public List<Map<String, Object>> exportCustomers(UserAccount u) {
        Map<String, Object> p = oc(u); p.put("Activity", "ReadByOrganizationCompanyIdForExport");
        return DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", p);
    }

    public List<Map<String, Object>> advanceCustomers(UserAccount u) {
        Map<String, Object> p = oc(u); p.put("Activity", "GetExportCustomerByAdvancePayment");
        return DesktopProc.rows(jdbc, "Sp_ExImLcOrder_GetAllMethod", p);
    }

    /** CommonServices.GetLcOrderNoBySupplierCustomerId(customer) - FinancialYearId = active year. */
    public List<Map<String, Object>> lcOrders(UserAccount u, int financialYearId, int customerId) {
        Map<String, Object> p = oc(u);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (customerId != 0) p.put("SupCustId", customerId);
        p.put("Activity", "GetLcOrderNoBySupplierCustomerId");
        return DesktopProc.rows(jdbc, "Sp_ExImLcOrder_GetAllMethod", p);
    }

    /** CommonServices.GetInvoiceNoandPartiesForFcBankReceipts(SupplierCustomerId) - Id, RefName, FilterType. */
    public List<Map<String, Object>> invoicesAndParties(UserAccount u, int financialYearId, int customerId) {
        Map<String, Object> p = oc(u);
        if (customerId != 0) p.put("SupplierCustomerId", customerId);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        p.put("Activity", "GetInvoiceNoandPartiesForFcBankReceipts");
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", p);
    }

    /** ExImInvoice.GetCommercialInvoicesaginstPreInvoices - the form passes OrganizationId as CompanyId too (kept). */
    public List<Map<String, Object>> breakupInvoices(int organizationId, int companyIdAsSent) {
        return DesktopProc.rows(jdbc, "USP_GetCommercialInvoicesaginstPreInvoices",
                params("OrganizationId", organizationId, "CompanyId", companyIdAsSent));
    }

    public List<Map<String, Object>> fcyBankInvoicesBalance(UserAccount u, int invoiceId) {
        Map<String, Object> p = oc(u); p.put("Id", invoiceId);
        return DesktopProc.rows(jdbc, "USP_GetFcyBankInvoicesBalance", p);
    }

    public List<Map<String, Object>> invoiceActivity(UserAccount u, int id, String activity) {
        Map<String, Object> p = oc(u); p.put("Id", id); p.put("Activity", activity);
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", p);
    }

    public List<Map<String, Object>> gdsForInvoice(UserAccount u, int invoiceId, int recId, int refDocumentTypeId) {
        Map<String, Object> p = oc(u);
        if (invoiceId > 0) p.put("Id", invoiceId);
        if (recId > 0) p.put("RecId", recId);
        if (refDocumentTypeId > 0) p.put("RefDocumentTypeId", refDocumentTypeId);
        return DesktopProc.rows(jdbc, "USP_GetGDsAgainstAdvancePaymentUtilizeInInvoice", p);
    }

    public List<Map<String, Object>> paymentTermDetail(int invoiceId, int recId) {
        Map<String, Object> p = params("InvoiceId", invoiceId);
        if (recId > 0) p.put("FcyReceiptId", recId);
        return DesktopProc.rows(jdbc, "usp_getPaymentTermDetailByInvoiceId", p);
    }

    public List<Map<String, Object>> currencies(UserAccount u) {
        Map<String, Object> p = oc(u); p.put("Activity", "ReadAll");
        return DesktopProc.rows(jdbc, "Sp_MultiCurrency_GetAllMethod", p);
    }

    public List<Map<String, Object>> chargesTypes(UserAccount u) {
        Map<String, Object> p = oc(u); p.put("Activity", "ReadByOrganizationCompanyId");
        return DesktopProc.rows(jdbc, "Sp_ExImProceedsChargesType_GetAllMethod", p);
    }

    /** COAAllocation.GetAll (COAAllocationSearch) - dtaccounts. */
    public List<Map<String, Object>> allAccounts(UserAccount u) {
        Map<String, Object> p = oc(u);
        if (u.getId() != null && u.getId() != 0) p.put("UserId", u.getId());
        p.put("Activity", "COAAllocationSearch");
        return DesktopProc.rows(jdbc, "Sp_COAAllocation_GetAllMethod", p);
    }

    /** COAAllocation.GetAccountTitleByAccountTypeIds(AccountTypeIds, AccountTypeIdsNot). */
    public List<Map<String, Object>> accountsByTypes(UserAccount u, int appId, String typeIds, String typeIdsNot) {
        Map<String, Object> p = oc(u);
        p.put("AppId", appId);
        if (typeIds != null && !typeIds.isEmpty()) p.put("AccountTypeIds", typeIds);
        if (typeIdsNot != null && !typeIdsNot.isEmpty()) p.put("AccountTypeIdsNot", typeIdsNot);
        if (u.getId() != null && u.getId() != 0) p.put("UserId", u.getId());
        p.put("Activity", "GetAccountTitleByAccountTypeIds");
        return DesktopProc.rows(jdbc, "Sp_COAAllocation_GetAllMethod", p);
    }

    public int firstBranchId(UserAccount u) {
        Map<String, Object> p = oc(u); p.put("Activity", "GetAll");
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_Branches_GetAllMethod", p);
        return r.isEmpty() ? 0 : asInt(ci(r.get(0), "Id"));
    }

    public int firstProjectId(UserAccount u) {
        Map<String, Object> p = oc(u); p.put("MethodType", "GetAll");
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_Projects_GetAllMethod", p);
        return r.isEmpty() ? 0 : asInt(ci(r.get(0), "Id"));
    }

    public List<Map<String, Object>> historyDropDowns(UserAccount u) {
        return DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromFcyBankReceipts", oc(u));
    }

    public List<Map<String, Object>> history(Map<String, Object> p) {
        return DesktopProc.rows(jdbc, "USP_ExImFCBankReceipts_FormHistory", p);
    }

    /** VoucherHead.GetVoucherHeadIdByDocumentTypeIdandRefDocNoId. */
    public int voucherHeadId(UserAccount u, int documentTypeId, int refId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods", params(
                "Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId",
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId, "DocumentTypeSrNo", refId));
        return r.isEmpty() ? 0 : asInt(ci(r.get(0), "Id"));
    }

    public List<Map<String, Object>> fcyAndLcyBalance(Map<String, Object> p) { return DesktopProc.rows(jdbc, "USP_GetFcyAndLcyBalance", p); }

    public List<Map<String, Object>> pendingPartyInvoices(Map<String, Object> p) { return DesktopProc.rows(jdbc, "USP_GetPendingPartyInvoices", p); }

    public List<Map<String, Object>> gainAndLossBreakup(int id) { return DesktopProc.rows(jdbc, "USP_GetGainAndLossBreackup", params("Id", id)); }

    // ------------------------------------------------------------------ GetByID

    public List<Map<String, Object>> byIdActivity(int id, String activity) {
        return DesktopProc.rows(jdbc, "Sp_ExImFCBankReceipts_GetAllMethod", params("Id", id, "Activity", activity));
    }

    // ------------------------------------------------------------------ SetDate (one transaction)

    /**
     * DAL ExImFCBankReceipts.SetDate, statement for statement. Every map is the model's non-virtual
     * properties in declaration order (a null value is not sent, as ADO.NET omits a CLR null).
     */
    @Transactional(rollbackFor = Exception.class)
    public int save(boolean insert, Map<String, Object> header, List<Map<String, Object>> deductions,
                    List<Map<String, Object>> breakups, List<Map<String, Object>> partyInvoices,
                    List<Map<String, Object>> gainLoss, List<Map<String, Object>> partyPayments,
                    List<Map<String, Object>> shipmentAdvances, List<Map<String, Object>> paymentTerms,
                    Map<String, Object> voucherHead, List<Map<String, Object>> voucherDetails,
                    String[] voucherHeadFields, String[] voucherDetailFields) {
        int num3 = DesktopProc.setProc(jdbc, insert ? "Sp_ExImFCBankReceipts_Insert" : "Sp_ExImFCBankReceipts_Update", header);
        int id;
        if (num3 > 0) { id = num3; header.put("Id", id); }
        else { id = asInt(header.get("Id")); num3 = id; }
        for (Map<String, Object> d : deductions) {
            d.put("ExImFCBankReceiptsId", id);
            DesktopProc.setProc(jdbc, "Sp_ExImFcBankReceiptsDeductions_Insert", d);
        }
        for (Map<String, Object> b : breakups) {
            b.put("FcyBankReceiptId", id);
            DesktopProc.setProc(jdbc, "[dbo].[USP_FcyBankReceiptBreakUp_Insert]", b);
        }
        int sort = 1;
        for (Map<String, Object> pi : partyInvoices) {
            pi.put("SortNo", sort++);
            pi.put("FcyBankReceiptId", id);
            pi.put("RefDocumentTypeId", 4);
            DesktopProc.setProc(jdbc, "[dbo].[USP_FcyBankReceiptBreakUpofPartyInvoices_Insert]", pi);
        }
        for (Map<String, Object> g : gainLoss) {
            g.put("FcyBankReceiptId", id);
            g.put("OrganizationId", header.get("OrganizationId"));
            g.put("CompanyId", header.get("CompanyId"));
            DesktopProc.setProc(jdbc, "[dbo].[Sp_GainLossBreakup_Insert]", g);
        }
        for (Map<String, Object> pp : partyPayments) {
            pp.put("FcyReceiptId", id);
            DesktopProc.setProc(jdbc, "USP_FcyPartyPaymentBreackUpForFinancials_Insert", pp);
        }
        for (Map<String, Object> sa : shipmentAdvances) {
            sa.put("FcyReceiptId", id);
            DesktopProc.setProc(jdbc, "USP_ExImShipmentPartyAgainstAdvancesPayment_Insert", sa);
        }
        for (Map<String, Object> pt : paymentTerms) {
            pt.put("FcyBankReceiptId", id);
            DesktopProc.setProc(jdbc, "usp_FcyBankReceiptPaymentTermDetail_Insert", pt);
        }
        for (Map<String, Object> b : breakups) {
            DesktopProc.rows(jdbc, "USP_ExportAdvanceFinancialInstumentUtilizeValidation", params(
                    "InvoiceId", b.get("RefDocRecordId"), "GdId", b.get("GDId"), "RefDocumentTypeId", b.get("GdRefDocTypeId")));
        }
        /* voucher */
        int existing = 0;
        List<Map<String, Object>> vr = DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods", params(
                "Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId",
                "OrganizationId", header.get("OrganizationId"), "CompanyId", header.get("CompanyId"),
                "DocumentTypeId", header.get("DocumentTypeId"), "DocumentTypeSrNo", id));
        if (!vr.isEmpty()) { existing = asInt(ci(vr.get(0), "Id")); voucherHead.put("Id", existing); }
        voucherHead.put("DocumentTypeSrNo", id);
        voucherHead.put("RefDocNoId", id);
        int num2 = DesktopProc.setProc(jdbc, existing == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", pick(voucherHead, voucherHeadFields));
        if (num2 > 0) voucherHead.put("Id", num2);
        else num2 = asInt(voucherHead.get("Id"));
        for (Map<String, Object> vd : voucherDetails) {
            vd.put("BranchesId", header.get("BranchId"));
            vd.put("VoucherHeadId", voucherHead.get("Id"));
            DesktopProc.setProc(jdbc, "Sp_VoucherDetail_Insert", pick(vd, voucherDetailFields));
        }
        DesktopProc.scalar(jdbc, "USP_VoucherBalanceCheck", params(
                "OrganizationId", header.get("OrganizationId"), "CompanyId", header.get("CompanyId"), "Id", voucherHead.get("Id")));
        voucherHead.put("RefDocNoId", voucherHead.get("Id"));
        int documentTypeIdRef = DesktopProc.setProc(jdbc, "Sp_VoucherHead_H_Insert", pick(voucherHead, voucherHeadFields));
        for (Map<String, Object> vd : voucherDetails) {
            vd.put("VoucherHeadId", voucherHead.get("Id"));
            vd.put("DocumentTypeIdRef", documentTypeIdRef);
            DesktopProc.setProc(jdbc, "Sp_VoucherDetail_H_Insert", pick(vd, voucherDetailFields));
        }
        return num3;
    }

    private static Map<String, Object> pick(Map<String, Object> src, String[] names) {
        Map<String, Object> p = params();
        for (String n : names) { Object v = src.get(n); if (v != null) p.put(n, v); }
        return p;
    }

    static int asInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        try { return (int) Double.parseDouble(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0; }
    }
}
