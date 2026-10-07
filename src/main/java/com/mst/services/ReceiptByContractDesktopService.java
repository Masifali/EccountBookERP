package com.mst.services;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.Set;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.dto.ContraVoucherDto;
import com.mst.repositories.AccountsGroupDSupport;
import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;

/**
 * Screen 18 "Receipt By Contract" - Architecture.WinApp.Account_Definition.ReceiptByContract (form Name "ReceiptByContract",
 * DocumentTypeId 29), call for call. BLL/DAL read from recovered_source/projects: BLL 0651 ReceiptByInvoiceHeader (MakeVoucher, Save,
 * GetByID, GenerateCode), DAL 0582 ReceiptByInvoiceHeader (SetDate / GetDate), models 1186 / 1187 / 1188, BLL 0654 VoucherHead
 * (balances, pending orders, history), BLL 0604 TaxesTypes, BLL 0608 TaxScheduleMain, GenericProvider.SetProc (every non-virtual
 * model property is a parameter, a CLR null is not sent).
 *
 * <ul>
 * <li>SupplierCustomer(): Usp_SupplierCustomerAgainstSaleOrder @OrganizationId @CompanyId (Id, CompanyName, GlAccountId, AccountTitle, AdvanceGlAcId, AdvanceAccount).</li>
 * <li>VoucherNofill: Sp_ReceiptByInvoiceHeader_GetAllMethod 'GenerateCode' (@DocumentTypeId 29, @FinancialYearId) -> DocNo, used when &gt; 0.</li>
 * <li>CoaAllocationGetAllServiceBind: Sp_COAAllocation_GetAllMethod 'COAAllocationSearch'; Tran Against Leave filters AccountTypeId 2 / 15 / 3,
 *     the tax accounts exclude 2 / 11 / 15.</li>
 * <li>BindTaxTypes: Sp_TaxesTypes_GetAllMethod 'ReadByCombo' @Type=1; ReadTaxSchedule: Sp_TaxSchedule_GetAllMehtod 'GetTaxPercentInTaxSchedule'.</li>
 * <li>Balances: Sp_Vouchers_GetMethods 'ReadByCurrentBalanceByDateAndAccountId' / USP_GetGLAndSubsidiaryCurrentBalance (feature 4).</li>
 * <li>GridContractFill: USP_GetPendingOrderForReceiptsVoucher. History: USp_ReceiptByContract_FromHistory.</li>
 * <li>Save (BLL.MakeVoucher then DAL.SetDate, one transaction): Sp_ReceiptByInvoiceHeader_Insert/_Update, Sp_ReceiptByInvoiceDetail_Insert,
 *     Sp_ReceiptByInvoiceOtherDetail_Insert, the voucher head / details, Sp_VoucherHead_H_Insert and Sp_VoucherDetail_H_Insert. The desktop DAL never
 *     persists AttachmentsList for this screen, so neither does this port.</li>
 * </ul>
 */
@Service
public class ReceiptByContractDesktopService {

    public static final String SCREEN_NAME = "ReceiptByContract";
    public static final int DOCUMENT_TYPE_ID = 29;

    private static final DateTimeFormatter DT = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss", Locale.ENGLISH);

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;
    private final AccountsGroupDSupport s;
    private final DesktopAttachmentStore store;

    public ReceiptByContractDesktopService(JdbcTemplate jdbc, CurrentUserContext ctx, AccountsGroupDSupport s, DesktopAttachmentStore store) {
        this.jdbc = jdbc;
        this.ctx = ctx;
        this.s = s;
        this.store = store;
    }

    /** A desktop validation or database message, shown as a plain MessageBox text. */
    public static class Refusal extends RuntimeException {
        private static final long serialVersionUID = 1L;
        public Refusal(String m) { super(m); }
    }

    // ------------------------------------------------------------------------------------------------- DDL

    /** SupplierCustomer(): the customers that have sale orders. */
    public List<Map<String, Object>> suppliers() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Usp_SupplierCustomerAgainstSaleOrder", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId()))) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (String k : new String[] { "Id", "CompanyName", "GlAccountId", "AccountTitle", "AdvanceGlAcId", "AdvanceAccount" }) o.put(k, norm(r.get(k)));
            out.add(o);
        }
        return out;
    }

    private Map<String, Object> supplierRow(int id) {
        for (Map<String, Object> r : suppliers()) if (netInt(r.get("Id")) == id) return r;
        return null;
    }

    /** BindTaxTypes: Sp_TaxesTypes_GetAllMethod 'ReadByCombo' @Type = 1. */
    public List<Map<String, Object>> taxTypes() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_TaxesTypes_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(), "Type", 1, "Activity", "ReadByCombo"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", r.get("Id"));
            o.put("TaxName", r.get("TaxName"));
            out.add(o);
        }
        return out;
    }

    /** CommonServices.CoaAllocationGetAllServiceBind: Sp_COAAllocation_GetAllMethod 'COAAllocationSearch'. */
    private List<Map<String, Object>> coaAll() {
        Map<String, Object> p = DesktopProc.params("OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId());
        int uid = ctx.currentUserId();
        if (uid != 0) p.put("UserId", uid);
        p.put("Activity", "COAAllocationSearch");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_COAAllocation_GetAllMethod", p)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", r.get("Id"));
            o.put("AccountTitle", r.get("AccountTitle"));
            o.put("AccountTypeId", r.get("AccountTypeId"));
            out.add(o);
        }
        return out;
    }

    /** AccountsComboBind: the tax accounts (not cash 2, 11, bank 15). */
    public List<Map<String, Object>> taxAccounts() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : coaAll()) {
            int t = netInt(r.get("AccountTypeId"));
            if (t == 2 || t == 11 || t == 15) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", r.get("Id"));
            o.put("AccountTitle", r.get("AccountTitle"));
            out.add(o);
        }
        return out;
    }

    /** CmbTranType_Leave: the credit accounts of one account type (Cash 2, Bank 15, Party 3). */
    public List<Map<String, Object>> accountsByType(int type) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : coaAll()) {
            if (netInt(r.get("AccountTypeId")) != type) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", r.get("Id"));
            o.put("AccountTitle", r.get("AccountTitle"));
            out.add(o);
        }
        return out;
    }

    /** VoucherNofill: 0 when the procedure returns no row (the page then keeps the box as it is). */
    public int generateCode() {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_ReceiptByInvoiceHeader_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "FinancialYearId", ctx.currentFinancialYearId(), "DocumentTypeId", DOCUMENT_TYPE_ID, "Activity", "GenerateCode"));
        return rows.isEmpty() ? 0 : netInt(rows.get(0).get("DocNo"));
    }

    /** ReceiptByInvoice_Load / btnRefresh_Click: SupplierCustomer, BindTaxTypes (and, on load, the rest). */
    public Map<String, Object> load() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("suppliers", suppliers());
        out.put("taxTypes", taxTypes());
        out.put("taxAccounts", taxAccounts());
        out.put("docNo", generateCode());
        out.put("hasSubsidiary", s.feature(4));
        out.put("rights", s.rights(SCREEN_NAME));
        return out;
    }

    // ------------------------------------------------------------------------------------------------- balances

    private Map<String, Object> glBalance(int accountId) {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods", DesktopProc.params(
                "Activity", "ReadByCurrentBalanceByDateAndAccountId",
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "FinancialYearId", ctx.currentFinancialYearId(), "RefAccountId", accountId,
                "VoucherDate", Timestamp.valueOf(LocalDateTime.now())));
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("has", !rows.isEmpty());
        o.put("balance", rows.isEmpty() ? 0d : netDouble(rows.get(0).get("Balance")));
        return o;
    }

    /** AccountCurrentBalance. */
    public Map<String, Object> accountBalance(int accountId) { return glBalance(accountId); }

    /** CustomerCurrentBalance: the GL balance, or the GL + subsidiary balance with ERP feature 4. */
    public Map<String, Object> customerBalance(int supplierId) {
        int gl = 0;
        Map<String, Object> sup = supplierRow(supplierId);
        if (sup != null) gl = netInt(sup.get("GlAccountId"));
        if (!s.feature(4)) return glBalance(gl);
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "USP_GetGLAndSubsidiaryCurrentBalance", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "ToDate", Timestamp.valueOf(LocalDateTime.now()), "GlAccountId", gl, "SubSaidiaryId", supplierId));
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("has", !rows.isEmpty());
        o.put("balance", rows.isEmpty() ? 0d : netDouble(rows.get(0).get("BalanceAmount")));
        return o;
    }

    /** AdvanceAccountCurrentBalance. */
    public Map<String, Object> advanceBalance(int supplierId) {
        int gl = 0;
        Map<String, Object> sup = supplierRow(supplierId);
        if (sup != null) gl = netInt(str(sup.get("AdvanceGlAcId")));
        return glBalance(gl);
    }

    /** ChkBoxWthHolding_CheckedChanged: ReadTaxSchedule(date, tax type). */
    public List<Map<String, Object>> taxSchedule(int taxTypeId, String date) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_TaxSchedule_GetAllMehtod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "EffectedDate", ts(date), "TaxNameId", taxTypeId, "Activity", "GetTaxPercentInTaxSchedule"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("TaxPercent", norm(r.get("TaxPercent")));
            o.put("TaxGLAccountId", norm(r.get("TaxGLAccountId")));
            o.put("AccountTitle", r.get("AccountTitle"));
            out.add(o);
        }
        return out;
    }

    // ------------------------------------------------------------------------------------------------- grids

    /** GridContractFill: the pending contracts of one customer (TaxAmount is added by the page). */
    public List<Map<String, Object>> pending(int supplierId) {
        if (supplierId == 0) throw new Refusal("Please select Customer Name first");
        Map<String, Object> p = DesktopProc.params("OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "SupplierCustomerId", supplierId);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetPendingOrderForReceiptsVoucher", p)) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (String k : new String[] { "Id", "DocumentTypeId", "PaymentTermsId", "OrderDate", "OrderNo", "DueDate", "PaymentTerm", "OrderAmount", "ReceivedAmount", "BalanceAmount" })
                o.put(k, norm(r.get(k)));
            out.add(o);
        }
        return out;
    }

    /** HistoryGridFill: USp_ReceiptByContract_FromHistory. */
    public List<Map<String, Object>> history(int supplierId, String from, String to, int fromDocNo, int toDocNo) {
        Map<String, Object> p = DesktopProc.params("OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId());
        if (supplierId != 0) p.put("SupplierCustomerId", supplierId);
        if (from != null && !from.trim().isEmpty()) p.put("FromDate", ts(from));
        if (to != null && !to.trim().isEmpty()) p.put("ToDate", ts(to));
        if (fromDocNo != 0) p.put("FromDocNo", fromDocNo);
        if (toDocNo != 0) p.put("ToDocNo", toDocNo);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USp_ReceiptByContract_FromHistory", p)) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (String k : new String[] { "Id", "DocumentTypeId", "DocDate", "DocNo", "CustomerName", "TaxDebitAccount", "TaxCreditAccount", "TaxName", "TaxPrct", "TaxAmount", "ReceivedAmount", "NoOfAttachemtns" })
                o.put(k, norm(r.get(k)));
            out.add(o);
        }
        return out;
    }

    // ------------------------------------------------------------------------------------------------- read

    private Map<String, Object> header(int id) {
        List<Map<String, Object>> own = jdbc.queryForList(
                "SELECT Id FROM dbo.ReceiptByInvoiceHeader WHERE Id = ? AND OrganizationId = ? AND CompanyId = ? AND DocumentTypeId = ?",
                id, ctx.currentOrganizationId(), ctx.currentCompanyId(), DOCUMENT_TYPE_ID);
        if (own.isEmpty()) throw new Refusal("Index was out of range. Must be non-negative and less than the size of the collection.");
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_ReceiptByInvoiceHeader_GetAllMethod", DesktopProc.params("Id", id, "Activity", "ReadByID"));
        if (rows.isEmpty()) throw new Refusal("Index was out of range. Must be non-negative and less than the size of the collection.");
        return rows.get(0);
    }

    private List<Map<String, Object>> detailRows(int id) {
        return DesktopProc.rows(jdbc, "Sp_ReceiptByInvoiceHeader_GetAllMethod", DesktopProc.params("Id", id, "Activity", "ReadByIdDetail"));
    }

    private List<Map<String, Object>> otherRows(int id) {
        return DesktopProc.rows(jdbc, "Sp_ReceiptByInvoiceHeader_GetAllMethod", DesktopProc.params("Id", id, "Activity", "ReadByIdOtherDetail"));
    }

    /** CommonServices.VoucherHeadIdGet(id, 29). */
    public int voucherHeadId(int id) {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods", DesktopProc.params(
                "Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId",
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "DocumentTypeId", DOCUMENT_TYPE_ID, "DocumentTypeSrNo", id));
        return rows.isEmpty() ? 0 : netInt(rows.get(0).get("Id"));
    }

    /** DMSAttachments.GetByID(id, base.Name). */
    public List<Map<String, Object>> attachments(int id) {
        return attachmentRows(DesktopProc.params("ScreenName", SCREEN_NAME, "Id", id, "Activity", "ReadById"));
    }

    /** CommonServices.GetNoofAttachmentsByRefDocumentTypeID(id, 29): 'ReadAttachmentsbyRefDocumentTypeId'. */
    public List<Map<String, Object>> attachmentsByRef(int id) {
        return attachmentRows(DesktopProc.params("RefDocumentTypeId", DOCUMENT_TYPE_ID, "Id", id, "Activity", "ReadAttachmentsbyRefDocumentTypeId"));
    }

    private List<Map<String, Object>> attachmentRows(Map<String, Object> p) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> a : DesktopProc.rows(jdbc, "Sp_DMSAttachments_GetAllMethod", p)) {
            if (netInt(a.get("OrganizationId")) != ctx.currentOrganizationId() || netInt(a.get("CompanyId")) != ctx.currentCompanyId()) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", a.get("Id"));
            o.put("Attachment", a.get("Attachment"));
            o.put("UploadedFileCustomName", a.get("UploadedFileCustomName"));
            o.put("UploadedFileSizeMb", norm(a.get("UploadedFileSizeMb")));
            o.put("EntryDate", norm(a.get("EntryDate")));
            o.put("EntryUserName", a.get("EntryUserName"));
            out.add(o);
        }
        return out;
    }

    /** ReadById: GetByID(RecId) - header, contract rows, other detail rows, voucher head id, attachments. */
    public Map<String, Object> getById(int id) {
        Map<String, Object> h = header(id);
        Map<String, Object> head = new LinkedHashMap<>();
        for (String k : new String[] { "Id", "DocDate", "DocNo", "SupplierCustomerId", "TaxTypeId", "TaxPrct", "TaxAmount", "TaxDebitAcId", "TaxCreditAcId" }) head.put(k, norm(h.get(k)));
        List<Map<String, Object>> details = new ArrayList<>();
        for (Map<String, Object> d : detailRows(id)) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (String k : new String[] { "RefDocNoId", "RefDocumentTypeId", "PaymentTermsId", "InvoiceDate", "InvoiceNo", "DueDate", "PaymentTerm", "BillAmount", "TotalReceivedAmount", "BalanceAmount" })
                o.put(k, norm(d.get(k)));
            details.add(o);
        }
        List<Map<String, Object>> others = new ArrayList<>();
        for (Map<String, Object> d : otherRows(id)) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (String k : new String[] { "TransType", "TransAgainst", "GLAccountId", "AccountTitle", "Amount", "CheqNo", "CheqDate", "PayTitle", "Remarks" }) o.put(k, norm(d.get(k)));
            others.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", head);
        out.put("details", details);
        out.put("others", others);
        out.put("voucherHeadId", voucherHeadId(id));
        out.put("attachments", attachments(id));
        return out;
    }

    // ------------------------------------------------------------------------------------------------- save

    public static class Upload {
        public String name;
        public String base64;
    }

    public static class SaveRequest {
        public int id;
        public String docDate;
        public String docNo;
        public Object supplierId;
        public boolean wht;
        public Object taxTypeId;
        public Object taxDrId;
        public Object taxCrId;
        public String taxPercent;
        public String taxAmount;
        /** grdDetail rows: TransTypeId, TransAgainstId, GLAccountId, Amount, CheqNo, CheqDate (yyyy-MM-dd), Paytitle, Remarks */
        public List<Map<String, Object>> others;
        /** grdContract rows: Id, DocumentTypeId, ReceivedAmount, TaxAmount, checked */
        public List<Map<String, Object>> contracts;
        public List<Integer> keepAttachments;
        public List<Upload> addAttachments;
    }

    private static String against(int id) { return id == 1 ? "Cash" : id == 2 ? "Bank" : id == 3 ? "Party" : ""; }

    /**
     * Insert() after the confirm: Formvalidation, GrandTotal and the three amount checks are repeated with the desktop's texts and order,
     * then ReceiptByInvoiceHeader.Save (MakeVoucher + DAL.SetDate). Returns {id, docNo}.
     */
    @Transactional
    public Map<String, Object> save(SaveRequest r) {
        int org = ctx.currentOrganizationId(), company = ctx.currentCompanyId(), user = ctx.currentUserId();
        int fy = ctx.currentFinancialYearId();
        if (r.id < 0) throw new Refusal("Invalid record");
        s.requireRight(SCREEN_NAME, r.id > 0 ? "canUpdate" : "canSave", "You do not have the right to " + (r.id > 0 ? "update" : "save") + " this record");

        /* Formvalidation */
        int supplierId = netInt(r.supplierId);
        Map<String, Object> supplier = supplierId == 0 ? null : supplierRow(supplierId);
        if (supplier == null) throw new Refusal("Please Select Supplier");
        Set<Integer> taxAcIds = new HashSet<>();
        for (Map<String, Object> a : taxAccounts()) taxAcIds.add(netInt(a.get("Id")));
        int taxCr = netInt(r.taxCrId), taxDr = netInt(r.taxDrId), taxType = netInt(r.taxTypeId);
        if (r.wht) {
            if (!taxAcIds.contains(taxCr)) throw new Refusal("Tax Credit Ac field required");
            if (!taxAcIds.contains(taxDr)) throw new Refusal("Tax Debit Ac field required");
            String pct = r.taxPercent == null ? "" : r.taxPercent;
            if (pct.isEmpty() || pct.equals("0")) throw new Refusal("Tax Percent field required");
            String amt = r.taxAmount == null ? "" : r.taxAmount;
            if (amt.isEmpty() || amt.equals("0")) throw new Refusal("Tax Amount field required");
        }
        if (taxCr != 0 && !taxAcIds.contains(taxCr)) taxCr = 0;
        if (taxDr != 0 && !taxAcIds.contains(taxDr)) taxDr = 0;
        String docNoText = r.docNo == null ? "" : r.docNo;
        if (docNoText.isEmpty() || docNoText.equals("0")) throw new Refusal("DocNo field required");

        List<Map<String, Object>> others = r.others == null ? new ArrayList<Map<String, Object>>() : r.others;
        List<Map<String, Object>> contracts = r.contracts == null ? new ArrayList<Map<String, Object>>() : r.contracts;
        List<Map<String, Object>> checked = new ArrayList<>();
        for (Map<String, Object> c : contracts) if (truthy(c.get("checked"))) checked.add(c);

        /* GrandTotal */
        double totalReceived = 0d, detailAmount = 0d;
        for (Map<String, Object> c : checked) totalReceived += netDouble(c.get("ReceivedAmount"));
        for (Map<String, Object> o : others) if (netInt(o.get("TransTypeId")) != 2) detailAmount += netDouble(o.get("Amount"));
        double grandText = round(totalReceived, 6);                       // txtgrandtotal.Text = ToString("#,##0.######")
        double diffText = round(totalReceived - detailAmount, 5);         // txtdifference.Text = ToString("#,##0.#####")
        if (diffText > 0.0 || diffText < 0.0) throw new Refusal("Please Clear The Difference Amount");
        double amount = 0d, totalReceivedAmount = 0d;
        if (!others.isEmpty()) {
            amount = detailAmount;
            if (amount != grandText) throw new Refusal("Please Check... Received & Total Amount Is Not Equal");
        }
        if (!checked.isEmpty()) {
            for (Map<String, Object> c : checked) totalReceivedAmount += netDouble(c.get("ReceivedAmount"));
        }
        if (totalReceivedAmount != amount) throw new Refusal("Please Check... Received & Total Amount Is Not Equal");

        if (others.isEmpty()) throw new Refusal("Outstanding Order Grid Can not be Empty");
        if (contracts.isEmpty()) throw new Refusal("Outstanding Order Grid Can not be Empty");
        if (checked.isEmpty()) throw new Refusal("Please Check any row in OutStanding Contract Grid First...");

        int recId = r.id;
        List<Map<String, Object>> stored = new ArrayList<>();
        if (recId > 0) {
            header(recId);                                              // belongs to the signed-in company
            stored = detailRows(recId);
        }
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        Timestamp docDate = ts(r.docDate);
        int docNo = netInt(docNoText.trim());
        double taxPrct = netDouble(r.taxPercent);
        double taxAmt = netDouble(r.taxAmount);

        /* the contract rows: the order data comes from the pending list (or the stored receipt), the form only picks and amounts them */
        Map<String, Map<String, Object>> allowed = new LinkedHashMap<>();
        for (Map<String, Object> p : stored) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("OrderDate", p.get("InvoiceDate")); o.put("OrderNo", p.get("InvoiceNo")); o.put("DueDate", p.get("DueDate"));
            o.put("PaymentTermsId", p.get("PaymentTermsId")); o.put("OrderAmount", p.get("BillAmount")); o.put("BalanceAmount", p.get("BalanceAmount"));
            allowed.put(netInt(p.get("RefDocNoId")) + "|" + netInt(p.get("RefDocumentTypeId")), o);
        }
        for (Map<String, Object> p : pending(supplierId)) allowed.put(netInt(p.get("Id")) + "|" + netInt(p.get("DocumentTypeId")), p);

        Set<Integer> accountIds = new HashSet<>();
        for (Map<String, Object> a : coaAll()) accountIds.add(netInt(a.get("Id")));
        int advanceAc = netInt(str(supplier.get("AdvanceGlAcId")));
        if (advanceAc != 0) accountIds.add(advanceAc);

        /* ReceiptByInvoiceOtherDetail rows */
        List<Map<String, Object>> otherList = new ArrayList<>();
        for (Map<String, Object> o : others) {
            int tt = netInt(o.get("TransTypeId")), ta = netInt(o.get("TransAgainstId")), gl = netInt(o.get("GLAccountId"));
            if (tt < 1 || tt > 3) throw new Refusal("TransType field required");
            if (ta < 1 || ta > 3) throw new Refusal("Trans Against field required");
            if (!accountIds.contains(gl)) throw new Refusal("Credit Account field required");
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("TransType", tt);
            d.put("TransAgainst", against(ta));
            d.put("GLAccountId", gl);
            d.put("Remarks", str(o.get("Remarks")));
            d.put("Amount", netDouble(o.get("Amount")));
            d.put("CheqNo", str(o.get("CheqNo")));
            d.put("CheqDate", ts(str(o.get("CheqDate"))));
            d.put("PayTitle", str(o.get("Paytitle")));
            otherList.add(d);
        }
        /* ReceiptByInvoiceDetail rows (checked contract rows only) */
        List<Map<String, Object>> detailList = new ArrayList<>();
        for (Map<String, Object> c : checked) {
            int cid = netInt(c.get("Id")), cdt = netInt(c.get("DocumentTypeId"));
            Map<String, Object> src = allowed.get(cid + "|" + cdt);
            if (src == null) throw new Refusal("Please Check any row in OutStanding Contract Grid First...");
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("RefDocNoId", cid);
            d.put("RefDocumentTypeId", cdt);
            d.put("PaymentTermsId", netInt(src.get("PaymentTermsId")));
            d.put("InvoiceDate", ts(str(src.get("OrderDate"))));
            d.put("InvoiceNo", netInt(src.get("OrderNo")));
            d.put("DueDate", ts(str(src.get("DueDate"))));
            d.put("BillAmount", (float) netDouble(src.get("OrderAmount")));
            d.put("Amount", (float) netDouble(c.get("ReceivedAmount")));
            d.put("BalanceAmount", (float) netDouble(src.get("BalanceAmount")));
            d.put("DWhtAmount", (float) netDouble(c.get("TaxAmount")));
            detailList.add(d);
        }

        /* BLL.MakeVoucher (before anything is written) */
        List<Map<String, Object>> sl = DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", DesktopProc.params(
                "Activity", "GetGlAccountIdandCompanyNameBySupplierCustomerId", "OrganizationId", org, "CompanyId", company));
        int refAccount = 0, advance = 0;
        if (!sl.isEmpty()) {
            Map<String, Object> mine = null;
            for (Map<String, Object> x : sl) if (netInt(x.get("Id")) == supplierId) { mine = x; break; }
            if (mine == null) throw new Refusal("Supplier GLAccountId not Found");
            refAccount = netInt(mine.get("GlAccountId"));
            advance = netInt(mine.get("AdvanceGlAcId"));
        }
        ContraVoucherDto.Head head = new ContraVoucherDto.Head();
        head.FinancialYearId = fy;
        head.EntryDate = fmt(now); head.EntryUser = user; head.ModifyDate = fmt(now); head.PostDate = fmt(now); head.DueDate = fmt(now);
        head.ModifyUser = user; head.OrganizationId = org; head.CompanyId = company;
        head.VoucherDate = fmt(docDate); head.VoucherCode = docNo; head.DocumentTypeId = DOCUMENT_TYPE_ID;
        head.RefAccountId = refAccount; head.AgainstAccountId = refAccount;
        List<ContraVoucherDto.Detail> vd = new ArrayList<>();
        double voucherAmount = 0d;
        for (Map<String, Object> o : otherList) {
            double a = (Double) o.get("Amount");
            if ((int) Math.rint(a) <= 0) continue;
            int tt = (Integer) o.get("TransType"), gl = (Integer) o.get("GLAccountId");
            String rem = str(o.get("Remarks"));
            if (tt == 2) {
                if (advance == 0) throw new Refusal("Advance GlAccountId Not Found");
                ContraVoucherDto.Detail d1 = new ContraVoucherDto.Detail();
                d1.AccountId = gl; d1.AgainstAccountId = advance; d1.Comments = rem; d1.CreditAmount = 0d; d1.DebitAmount = a;
                voucherAmount += d1.DebitAmount;
                vd.add(d1);
                ContraVoucherDto.Detail d2 = new ContraVoucherDto.Detail();
                d2.AccountId = advance; d2.AgainstAccountId = gl; d2.Comments = rem; d2.DebitAmount = 0d; d2.CreditAmount = a;
                d2.SubsidiaryTypeId = 1; d2.SubsidiaryAccountId = supplierId; d2.SupplierCustomerId = supplierId;
                vd.add(d2);
            } else if (tt != 3 && head.RefAccountId != gl) {
                ContraVoucherDto.Detail d3 = new ContraVoucherDto.Detail();
                d3.AccountId = gl; d3.AgainstAccountId = head.RefAccountId; d3.Comments = rem; d3.CreditAmount = 0d; d3.DebitAmount = a;
                voucherAmount += d3.DebitAmount;
                vd.add(d3);
                ContraVoucherDto.Detail d4 = new ContraVoucherDto.Detail();
                d4.AccountId = head.RefAccountId; d4.AgainstAccountId = gl; d4.Comments = rem; d4.DebitAmount = 0d; d4.CreditAmount = a;
                d4.SubsidiaryTypeId = 1; d4.SubsidiaryAccountId = supplierId; d4.SupplierCustomerId = supplierId;
                vd.add(d4);
            }
        }
        if (taxAmt > 0.0) {
            String tc = cs(taxPrct) + cs(taxAmt);
            ContraVoucherDto.Detail d5 = new ContraVoucherDto.Detail();
            d5.AccountId = taxDr; d5.AgainstAccountId = taxCr; d5.Comments = tc; d5.CreditAmount = 0d; d5.DebitAmount = taxAmt;
            vd.add(d5);
            ContraVoucherDto.Detail d6 = new ContraVoucherDto.Detail();
            d6.AccountId = taxCr; d6.AgainstAccountId = taxDr; d6.Comments = tc; d6.DebitAmount = 0d; d6.CreditAmount = taxAmt;
            vd.add(d6);
            voucherAmount += d5.DebitAmount;
        }
        head.VoucherAmount = voucherAmount;

        /* DAL.SetDate */
        int num = DesktopProc.setProc(jdbc, recId == 0 ? "Sp_ReceiptByInvoiceHeader_Insert" : "Sp_ReceiptByInvoiceHeader_Update", DesktopProc.params(
                "Id", recId,
                "DocDate", docDate,
                "DocumentTypeId", DOCUMENT_TYPE_ID,
                "DocNo", docNo,
                "SupplierCustomerId", supplierId,
                "OrganizationId", org,
                "CompanyId", company,
                "EntryUser", user,
                "EntryDate", now,
                "ModifyUser", user,
                "ModifyDate", now,
                "TaxTypeId", taxType,
                "TaxCreditAcId", taxCr,
                "TaxDebitAcId", taxDr,
                "TaxPrct", taxPrct,
                "TaxAmount", taxAmt,
                "FinancialYearId", fy));
        int hid = num > 0 ? num : recId;
        for (Map<String, Object> d : detailList) {
            DesktopProc.setProc(jdbc, "Sp_ReceiptByInvoiceDetail_Insert", DesktopProc.params(
                    "Id", 0,
                    "ReceiptByInvoiceHeaderId", hid,
                    "InvoiceDate", d.get("InvoiceDate"),
                    "InvoiceNo", d.get("InvoiceNo"),
                    "RefDocumentTypeId", d.get("RefDocumentTypeId"),
                    "RefDocNoId", d.get("RefDocNoId"),
                    "DueDate", d.get("DueDate"),
                    "AdvanceAmount", 0d,
                    "BillAmount", d.get("BillAmount"),
                    "TotalReceivedAmount", 0d,
                    "BalanceAmount", d.get("BalanceAmount"),
                    "Amount", d.get("Amount"),
                    "DWhtAmount", d.get("DWhtAmount"),
                    "DAdjustmentAmount", 0d,
                    "PaymentTermsId", d.get("PaymentTermsId")));
        }
        for (Map<String, Object> o : otherList) {
            DesktopProc.setProc(jdbc, "Sp_ReceiptByInvoiceOtherDetail_Insert", DesktopProc.params(
                    "Id", 0,
                    "ReceiptByInvoiceHeaderId", hid,
                    "TransType", o.get("TransType"),
                    "GLAccountId", o.get("GLAccountId"),
                    "Remarks", o.get("Remarks"),
                    "CheqId", 0,
                    "CheqNo", o.get("CheqNo"),
                    "CheqDate", o.get("CheqDate"),
                    "PayTitle", o.get("PayTitle"),
                    "Amount", o.get("Amount"),
                    "TransAgainst", o.get("TransAgainst")));
        }
        int existing = voucherHeadId(hid);
        head.DocumentTypeSrNo = hid;
        if (existing != 0) head.Id = existing;
        int vnum = DesktopProc.setProc(jdbc, existing == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", PdcReceiptDesktopService.fields(head));
        if (vnum > 0) head.Id = vnum;
        for (ContraVoucherDto.Detail d : vd) {
            d.VoucherHeadId = head.Id;
            DesktopProc.setProc(jdbc, "Sp_VoucherDetail_Insert", PdcReceiptDesktopService.fields(d));
        }
        int ref = DesktopProc.setProc(jdbc, "Sp_VoucherHead_H_Insert", PdcReceiptDesktopService.fields(head));
        for (ContraVoucherDto.Detail d : vd) {
            d.VoucherHeadId = head.Id;
            d.DocumentTypeIdRef = ref;
            DesktopProc.setProc(jdbc, "Sp_VoucherDetail_H_Insert", PdcReceiptDesktopService.fields(d));
        }

        /* the DAL never reads AttachmentsList; Insert() only runs RemoveByIdAndNames after an update that removed every attachment */
        if (recId > 0) {
            boolean keepAny = r.keepAttachments != null && !r.keepAttachments.isEmpty();
            boolean addAny = r.addAttachments != null && !r.addAttachments.isEmpty();
            if (!keepAny && !addAny && !attachments(recId).isEmpty()) {
                DesktopProc.rows(jdbc, "Sp_DMSAttachments_GetAllMethod", DesktopProc.params(
                        "Id", recId, "ScreenName", SCREEN_NAME, "RefDocumentTypeId", DOCUMENT_TYPE_ID, "Activity", "RemoveByIdAndName"));
            }
        }

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", hid);
        out.put("docNo", docNo);
        return out;
    }

    public static class Download {
        public final String name;
        public final byte[] bytes;
        public Download(String name, byte[] bytes) { this.name = name; this.bytes = bytes; }
    }

    /** Opening one attached file of a receipt. */
    public Download download(int docId, int attachmentId) {
        header(docId);
        List<Map<String, Object>> all = new ArrayList<>(attachments(docId));
        all.addAll(attachmentsByRef(docId));
        for (Map<String, Object> a : all) {
            if (netInt(a.get("Id")) != attachmentId) continue;
            String original = base(str(a.get("Attachment")));
            String storedName = str(a.get("UploadedFileCustomName"));
            return new Download(original, store.read(ctx.requireAccountingUser(), base(storedName.isEmpty() ? original : storedName)));
        }
        throw new Refusal("Attachment not found");
    }

    private static String base(String v) {
        String n = v.replace('\\', '/');
        n = n.substring(n.lastIndexOf('/') + 1);
        DesktopAttachmentStore.validateName(n);
        return n;
    }

    // ------------------------------------------------------------------------------------------------- helpers

    private static boolean truthy(Object v) { return v instanceof Boolean ? (Boolean) v : v != null && "true".equalsIgnoreCase(String.valueOf(v)); }

    /** Math.Round-like to n decimals as the "#,##0.###" format shows it. */
    private static double round(double v, int n) { return BigDecimal.valueOf(v).setScale(n, java.math.RoundingMode.HALF_UP).doubleValue(); }

    /** Conversion.ToString(double). */
    private static String cs(double v) {
        if (v == Math.rint(v) && Math.abs(v) < 1e15) return String.valueOf((long) v);
        return BigDecimal.valueOf(v).stripTrailingZeros().toPlainString();
    }

    private static String fmt(Timestamp t) { return t.toLocalDateTime().format(DT); }

    /** Conversion.ToInt: null / "" / a non-integer string -> 0. */
    static int netInt(Object v) {
        if (v == null) return 0;
        try {
            if (v instanceof Number) {
                double d = ((Number) v).doubleValue();
                if (Double.isNaN(d) || Double.isInfinite(d)) return 0;
                double x = Math.rint(d);
                if (x > Integer.MAX_VALUE || x < Integer.MIN_VALUE) return 0;
                return (int) x;
            }
            String t = String.valueOf(v).trim();
            if (!t.matches("[+-]?\\d+")) return 0;
            return Integer.parseInt(t);
        } catch (RuntimeException e) {
            return 0;
        }
    }

    static double netDouble(Object v) {
        if (v == null) return 0.0;
        try {
            double d = v instanceof Number ? ((Number) v).doubleValue() : Double.parseDouble(String.valueOf(v).trim().replace(",", ""));
            return Double.isNaN(d) || Double.isInfinite(d) ? 0.0 : d;
        } catch (RuntimeException e) {
            return 0.0;
        }
    }

    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    /** 'yyyy-MM-dd' or 'yyyy-MM-dd HH:mm:ss' / 'T' form -> Timestamp (a date alone gets 00:00:00). */
    static Timestamp ts(String v) {
        if (v == null || v.trim().isEmpty()) throw new Refusal("Invalid date");
        try {
            String t = v.trim().replace('T', ' ');
            if (t.length() == 10) return Timestamp.valueOf(LocalDate.parse(t).atTime(LocalTime.MIDNIGHT));
            if (t.length() == 16) t = t + ":00";
            return Timestamp.valueOf(LocalDateTime.parse(t.substring(0, 19), DT));
        } catch (RuntimeException e) {
            throw new Refusal("Invalid date");
        }
    }

    /** JSON friendly value: Timestamp -> yyyy-MM-dd HH:mm:ss, BigDecimal -> plain number string. */
    static Object norm(Object v) {
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().format(DT);
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().atStartOfDay().format(DT);
        if (v instanceof BigDecimal) return ((BigDecimal) v).stripTrailingZeros().toPlainString();
        return v;
    }
}
