package com.mst.repositories.pbi;

import com.mst.models.UserAccount;
import com.mst.repositories.hrm.HrmProcRepository;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * DAL of screen 41 "Payment By Invoice Voucher New"
 * (Architecture.WinApp.Account_Definition.PaymentByInvoiceVoucherNew + its popup LoadPendingInvoicesByPayment).
 * Every method is the desktop BLL call it names, with the procedure, @Activity and parameter set that BLL
 * builds (a parameter the BLL only adds under a condition is only added here under the same condition).
 * Tenancy comes from the signed-in {@link UserAccount}; nothing here takes it from a request.
 * A null value is omitted from the EXEC (ADO.NET AddWithValue(null)).
 */
@Repository
public class PbiVoucherRepository {

    private final HrmProcRepository db;

    public PbiVoucherRepository(HrmProcRepository db) { this.db = db; }

    // ------------------------------------------------------------------ combos (form Load / Refresh)

    /** CommonServices.CompanyServiceBind -> Company.GetAlldt(OrgCompanyTypeId = OrganizationId): Sp_Company_GetAllMethod 'ReadByOrganizationId'. */
    public List<Map<String, Object>> companies(UserAccount u) {
        return db.rows("Sp_Company_GetAllMethod", "OrgCompanyTypeId", u.getOrganizationId(), "Activity", "ReadByOrganizationId");
    }

    /** CommonServices.BrancheServiceBind -> Branches.GetAll (BLL 0058:29): Sp_Branches_GetAllMethod @OrganizationId @CompanyId 'GetAll'. */
    public List<Map<String, Object>> branches(UserAccount u) {
        return db.rows("Sp_Branches_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetAll");
    }

    /** CommonServices.ProjectServiceBind -> Projects.GetAlldt (BLL 0078:58): Sp_Projects_GetAllMethod @MethodType 'GetAll'. */
    public List<Map<String, Object>> projects(UserAccount u) {
        return db.rows("Sp_Projects_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "MethodType", "GetAll");
    }

    /** jobLot.GetList (BLL 0594:54): SP_JobLot_ReadMethod @OrganizationId @CompanyId 'GetAll'. */
    public List<Map<String, Object>> jobLots(UserAccount u) {
        return db.rows("SP_JobLot_ReadMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetAll");
    }

    /**
     * CommonServices.CoaAllocationGetAllServiceBind -> COAAllocation.GetAll (BLL 0648:154):
     * Sp_COAAllocation_GetAllMethod @OrganizationId @CompanyId (@UserId when != 0) 'COAAllocationSearch'.
     */
    public List<Map<String, Object>> coaAllocationSearch(UserAccount u) {
        Integer uid = u.getId();
        return db.rows("Sp_COAAllocation_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "UserId", uid != null && uid != 0 ? uid : null, "Activity", "COAAllocationSearch");
    }

    /** TaxesTypes.GetForComboBind(Type = 1) (BLL 0604:88): Sp_TaxesTypes_GetAllMethod @OrganizationId @CompanyId @Type 'ReadByCombo'. */
    public List<Map<String, Object>> taxTypes(UserAccount u) {
        return db.rows("Sp_TaxesTypes_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Type", 1, "Activity", "ReadByCombo");
    }

    /** clsGlobalVariables.configrationsAllocation (ConfigDescription) - Sp_ConfigrationsAllocation_GetAllMethod 'GetConfigurationByOrgCompandConfigDescription'. */
    public String config(UserAccount u, String description) {
        List<Map<String, Object>> rows = db.rows("Sp_ConfigrationsAllocation_GetAllMethod", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription");
        if (rows.isEmpty()) return null;
        Object v = rows.get(0).get("ConfigKey");
        return v == null ? null : String.valueOf(v);
    }

    /** clsGlobalVariables.ActiveYr rows (Start_Period for the popup's FromDate). */
    public List<Map<String, Object>> activeYears(UserAccount u) {
        return db.rows("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    // ------------------------------------------------------------------ header events

    /**
     * CommonServices.GenerateVoucherCode(DocumentTypeId) -> VoucherHead.GenerateVoucherCodeByDocumentTypeId (BLL 0654:334):
     * Sp_Vouchers_GetMethods @OrganizationId @CompanyId @DocumentTypeId @FinancialYearId @BranchesId 'GenerateVoucherCodeByDocumentTypeId'.
     */
    public List<Map<String, Object>> generateVoucherCode(UserAccount u, int yearId, int documentTypeId) {
        Integer b = u.getBranchesId();
        return db.rows("Sp_Vouchers_GetMethods", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId, "FinancialYearId", yearId, "BranchesId", b == null ? 0 : b,
                "Activity", "GenerateVoucherCodeByDocumentTypeId");
    }

    /** AccountTitleFill -> COAAllocation.GetDetailAccountByDocumentTypeId (BLL 0648:226): 'GetDetailAccountsByDocumentTypeId'. */
    public List<Map<String, Object>> detailAccountsByDocumentType(UserAccount u, int documentTypeId) {
        return db.rows("Sp_COAAllocation_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId, "Activity", "GetDetailAccountsByDocumentTypeId");
    }

    /** VoucherHead.ReadByCurrentBalanceByDateAndAccountId (BLL 0654:491). */
    public List<Map<String, Object>> currentBalance(UserAccount u, int yearId, int accountId, Timestamp voucherDate) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("Activity", "ReadByCurrentBalanceByDateAndAccountId");
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("FinancialYearId", yearId);
        p.put("RefAccountId", accountId);
        p.put("VoucherDate", voucherDate);
        return db.rows("Sp_Vouchers_GetMethods", p);
    }

    /**
     * CheqBookHeader.OutstandingCheqNo (BLL 0647:61): SP_CheqBookHeader_GetAllMethod @OrganizationId @CompanyId @BankId
     * (@RecId / @Id only when != 0 - the form sets neither) @MethodType 'OutstandingCheqNo'.
     */
    public List<Map<String, Object>> outstandingCheques(UserAccount u, int bankId) {
        return db.rows("SP_CheqBookHeader_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BankId", bankId, "MethodType", "OutstandingCheqNo");
    }

    // ------------------------------------------------------------------ detail events

    /** VoucherHead.GetInvoiceNoByPaymentByInvoice (BLL 0654:1551) - @Id is not set by the form, so it is not sent. */
    public List<Map<String, Object>> invoiceNos(UserAccount u, int yearId, int accountId) {
        return db.rows("Sp_Vouchers_GetMethods", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", yearId, "RefAccountId", accountId, "Activity", "GetInvoiceNoByPaymentByInvoice");
    }

    /** VoucherHead.GetInvoiceWiseBalanceAmount (BLL 0654:1597): @Id = the invoice (DocumentTypeSrNo), @RefAccountId = the account. */
    public List<Map<String, Object>> invoiceBalance(UserAccount u, int yearId, int accountId, int invoiceId) {
        return db.rows("Sp_Vouchers_GetMethods", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", yearId, "Id", invoiceId, "RefAccountId", accountId, "Activity", "GetInvoiceWiseBalanceAmount");
    }

    /** TaxScheduleMain.ReadTaxSchedule (BLL 0608:82): Sp_TaxSchedule_GetAllMehtod 'GetTaxPercentInTaxSchedule'. */
    public List<Map<String, Object>> taxSchedule(UserAccount u, Timestamp effectedDate, int taxNameId) {
        return db.rows("Sp_TaxSchedule_GetAllMehtod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "EffectedDate", effectedDate, "TaxNameId", taxNameId, "Activity", "GetTaxPercentInTaxSchedule");
    }

    // ------------------------------------------------------------------ Load Invoices popup

    /** LoadPendingInvoicesByPayment.AccountTitleFill -> COAAllocation.GetSupplierGlAccountExistInPurchaseInvoice (BLL 0648:395). */
    public List<Map<String, Object>> supplierGlAccounts(UserAccount u) {
        return db.rows("Sp_COAAllocation_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Activity", "GetSupplierGlAccountExistInPurchaseInvoice");
    }

    /**
     * VoucherHead.GetInvoicesForPaymentInvoiceVoucherLoader (BLL 0654:1777): @OrganizationId @CompanyId @FinancialYearId
     * @RefAccountId, @FromDate / @ToDate unless DateTime.MinValue, @InvoiceIds when PurchaseInvoiceIds != "" (a null
     * PurchaseInvoiceIds is added as a null value, i.e. not sent), @ReqType, 'GetInvoicesForPaymentInvoiceVoucherLoader'.
     */
    public List<Map<String, Object>> invoicesForLoader(UserAccount u, int yearId, int accountId, Timestamp from, Timestamp to,
                                                       String invoiceIds, String reqType) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("FinancialYearId", yearId);
        p.put("RefAccountId", accountId);
        p.put("FromDate", from);
        p.put("ToDate", to);
        p.put("InvoiceIds", invoiceIds);
        p.put("ReqType", reqType);
        p.put("Activity", "GetInvoicesForPaymentInvoiceVoucherLoader");
        return db.rows("Sp_Vouchers_GetMethods", p);
    }

    // ------------------------------------------------------------------ history / edit / print

    /**
     * VoucherHead.PaymentByInvoiceVoucherNewHistory (BLL 0654:1640) as HistoryFill sends it: DocumentTypeId 1 ->
     * @DocumentTypeName '1,2'; @FinancialYearId when != 0; @CanViewAllRecord; @EntryUser when not CanViewAllRecord
     * (the form never sets EntryUser, so the BLL sends 0); @NoOfRecords when != 0.
     */
    public List<Map<String, Object>> history(UserAccount u, int yearId, boolean canViewAllRecord, int noOfRecords) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("Activity", "PaymentByInvoiceVoucherNewHistory");
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        if (yearId != 0) p.put("FinancialYearId", yearId);
        p.put("DocumentTypeName", "1,2");
        p.put("CanViewAllRecord", canViewAllRecord);
        if (!canViewAllRecord) p.put("EntryUser", 0);
        if (noOfRecords != 0) p.put("NoOfRecords", noOfRecords);
        return db.rows("Sp_Vouchers_GetMethods", p);
    }

    /** VoucherHead.GetByID (BLL 0654:377) -> DAL 0586 GetData: the header ('ReadByID'). */
    public List<Map<String, Object>> head(int id) {
        return db.rows("Sp_Vouchers_GetMethods", "Id", id, "Activity", "ReadByID");
    }

    /** DAL 0586 GetData: voucherDetailList ('VoucherDetail_ReadByVoucherHeadID'). */
    public List<Map<String, Object>> details(int id) {
        return db.rows("Sp_Vouchers_GetMethods", "Id", id, "Activity", "VoucherDetail_ReadByVoucherHeadID");
    }

    /** DMSAttachments.GetByID(Id, base.Name) (BLL 0069:49): Sp_DMSAttachments_GetAllMethod @ScreenName @Id 'ReadById'. */
    public List<Map<String, Object>> attachments(int id, String screenName) {
        return db.rows("Sp_DMSAttachments_GetAllMethod", "ScreenName", screenName, "Id", id, "Activity", "ReadById");
    }

    /** VoucherReports.PaymentByInvoiceNewSlip (BLL 0141:337): the rows of 132-PaymentByInvoiceSlipNew_Report.rpt. */
    public List<Map<String, Object>> slip(UserAccount u, int id) {
        return db.rows("[dbo].[SpVouchers_Payment&ReceipteByInvoiceVoucherSlipNew_Rpt]", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "Id", id);
    }
}
