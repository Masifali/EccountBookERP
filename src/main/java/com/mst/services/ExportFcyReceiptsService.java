package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportFcyReceiptsRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.math.MathContext;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.format.DateTimeFormatter;
import java.time.format.DateTimeParseException;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;

import static com.mst.repositories.ExportGdBreakUpRepository.ci;

/**
 * The BLL side of 794 Acfrmfcbankreceipt "Fcy Receipts" (Architecture.WinApp.Account_Definition), DocumentTypeId 203.
 *
 * Insert() (form) -> ExImFCBankReceipts.Save (BLL 0464) -> MakeVoucher -> DAL SetDate. Every validation of
 * FormValidation / Insert is repeated here with the desktop's text and order; MakeVoucher is ported branch for
 * branch (manual gain/loss, auto gain/loss with Invoice Payment, Multi Invoices Payment with FIFO party invoices
 * and ledger-balance average rate, and the generic branch). The voucher is saved through the same procedures
 * and in the same transaction as the receipt.
 *
 * Desktop quirks kept (documented, not fixed):
 *   - InvoiceNoForBreakUp sends CompanyId = OrganizationId to USP_GetCommercialInvoicesaginstPreInvoices.
 *   - BranchId / ProjectId: the first Sp_Branches / Sp_Projects row, but RESETMAIN zeroes BrancheId / ProjectId and
 *     the form never reloads them, so every save after the first New / save / history Edit of a session sends 0
 *     (the page tells the service through "branchReset").
 *   - ReadById fills an empty charges FDBCNo with the receipt's Bank FBP # (saved back that way on update).
 *   - The invoice-wise breakup duplicate check compares each FDBC # only with the previous grid row.
 *   - Model.VoucherHeadId carries the Gain/Loss account (CmbGainLossAc), as the form assigns it.
 *   - double / decimal values inside voucher comments print as .NET Framework ToString() does (15 significant digits).
 *   - ExImInvoice.GetInvoiecHeaderByID / VoucherInvoicesAdjustment (TotalInvoiceAmount > 0) are never reached from
 *     this form (Save calls MakeVoucher(obj) without them); that branch is not ported.
 *
 * Rights: DesktopReportRights with screen 794 - View, Save, Update, Print, CanViewAllRecord. Tenancy, financial
 * year, app and user come from the session only.
 */
@Service
public class ExportFcyReceiptsService {

    public static final int SCREEN_ID = 794;
    public static final int DOCUMENT_TYPE_ID = 203;

    @Autowired private ExportFcyReceiptsRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights rights;

    /**
     * Rights of the other menu entries of the same desktop form (screens 707 "FCY Bank Receipt" and 43 "FCY Receipt",
     * FcyBankReceiptService, 2026-10-02): while set for the current request, every right is answered by this check
     * instead of DesktopReportRights(794). Unset (the /export/fcy-receipts page), nothing changes.
     */
    private static final ThreadLocal<java.util.function.BiPredicate<UserAccount, String>> RIGHTS_OVERRIDE = new ThreadLocal<>();

    public <T> T withRights(java.util.function.BiPredicate<UserAccount, String> check, java.util.function.Supplier<T> body) {
        java.util.function.BiPredicate<UserAccount, String> previous = RIGHTS_OVERRIDE.get();
        RIGHTS_OVERRIDE.set(check);
        try { return body.get(); }
        finally { if (previous == null) RIGHTS_OVERRIDE.remove(); else RIGHTS_OVERRIDE.set(previous); }
    }

    private UserAccount user(String action) {
        UserAccount u = currentUserContext.requireAccountingUser();
        java.util.function.BiPredicate<UserAccount, String> o = RIGHTS_OVERRIDE.get();
        if (o != null) {
            if (!o.test(u, action)) throw new AccessDeniedException("The user does not have " + action + " rights for this screen");
            return u;
        }
        rights.require(u, SCREEN_ID, action);
        return u;
    }

    private boolean allowed(UserAccount u, String action) {
        java.util.function.BiPredicate<UserAccount, String> o = RIGHTS_OVERRIDE.get();
        if (o != null) return o.test(u, action);
        try { rights.require(u, SCREEN_ID, action); return true; }
        catch (AccessDeniedException ex) { return false; }
    }

    private int fy() { return currentUserContext.currentFinancialYearId(); }

    // ================================================================= load

    /** Acfrmfcbankreceipt_Load. */
    public Map<String, Object> setup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, "Save"));
        perm.put("Update", allowed(u, "Update"));
        perm.put("Print", allowed(u, "Print"));
        out.put("permissions", perm);
        out.put("documentNo", docNo(u));
        List<Map<String, Object>> terms = new ArrayList<>();
        for (Map<String, Object> r : repo.paymentTypes()) terms.add(idName(asInt(ci(r, "Id")), text(ci(r, "PaymentType"))));
        out.put("paymentTerms", terms);
        out.putAll(refreshLists(u));
        out.put("gainLossAccounts", accountList(repo.accountsByTypes(u, appId(), null, "2,4,11,12,15")));
        out.put("fbcAccounts", accountList(repo.accountsByTypes(u, appId(), "11,13,14,20,21", null)));
        out.putAll(historyCombos(u));
        return out;
    }

    private int appId() { try { return currentUserContext.currentAppId(); } catch (RuntimeException e) { return 0; } }

    private int docNo(UserAccount u) {
        try { int c = repo.generateCode(u, fy()); return c > 0 ? c : 0; }
        catch (RuntimeException e) { return 0; }
    }

    /** GenerateDocumentNo (RESETMAIN). */
    public Map<String, Object> newDocumentNo() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("documentNo", docNo(u));
        return out;
    }

    /** btnRefresh_Click: GetAllAccounts, CurrencyCOde, ChargerType, ReferenceNoFill (also part of Load). */
    public Map<String, Object> refresh() { return refreshLists(user("View")); }

    private Map<String, Object> refreshLists(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> acc = new ArrayList<>();
        for (Map<String, Object> r : repo.allAccounts(u)) {
            Map<String, Object> m = idName(asInt(ci(r, "Id")), text(ci(r, "AccountTitle")));
            m.put("AccountTypeId", asInt(ci(r, "AccountTypeId")));
            acc.add(m);
        }
        out.put("accounts", acc);
        List<Map<String, Object>> cur = new ArrayList<>();
        for (Map<String, Object> r : repo.currencies(u)) cur.add(idName(asInt(ci(r, "Id")), text(ci(r, "CurrencyCode"))));
        out.put("currencies", cur);
        List<Map<String, Object>> ct = new ArrayList<>();
        for (Map<String, Object> r : repo.chargesTypes(u)) ct.add(idName(asInt(ci(r, "Id")), text(ci(r, "ProceedsChargesTypedescription"))));
        out.put("chargesAccountTypes", ct);
        List<Map<String, Object>> refs = new ArrayList<>();
        for (Map<String, Object> r : repo.referenceNos(u)) { String s = text(ci(r, "ReferenceNo")); Map<String, Object> m = new LinkedHashMap<>(); m.put("Id", s); m.put("name", s); refs.add(m); }
        out.put("referenceNos", refs);
        return out;
    }

    private static List<Map<String, Object>> accountList(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) out.add(idName(asInt(ci(r, "Id")), text(ci(r, "AccountTitle"))));
        return out;
    }

    // ================================================================= cascades

    /**
     * PaymentTerm(): the customer list by term - "Advanced Payment" GetExportCustomerByAdvancePayment (also Consignee),
     * "Invoice Payment" GetInvoiceNoandPartiesForFcBankReceipts() PartyName rows (+ Consignee from the advance list),
     * "Other Payment" / Value 4 SupplierCustomer.ReadByOrganizationCompanyIdForExport.
     */
    public Map<String, Object> customers(String term, int termId) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> cust = new ArrayList<>(), cons = new ArrayList<>();
        if ("Advanced Payment".equals(term)) {
            for (Map<String, Object> r : repo.advanceCustomers(u)) cust.add(idName(asInt(ci(r, "SupCustId")), text(ci(r, "CompanyName"))));
            cons.addAll(cust);
        } else if ("Invoice Payment".equals(term)) {
            for (Map<String, Object> r : repo.invoicesAndParties(u, fy(), 0))
                if ("PartyName".equals(text(ci(r, "FilterType")))) cust.add(idName(asInt(ci(r, "Id")), text(ci(r, "RefName"))));
            for (Map<String, Object> r : repo.advanceCustomers(u)) cons.add(idName(asInt(ci(r, "SupCustId")), text(ci(r, "CompanyName"))));
        } else if ("Other Payment".equals(term) || termId == 4) {
            for (Map<String, Object> r : repo.exportCustomers(u)) cust.add(idName(asInt(ci(r, "Id")), text(ci(r, "CompanyName"))));
        } else {
            for (Map<String, Object> r : repo.invoicesAndParties(u, fy(), 0))
                if ("PartyName".equals(text(ci(r, "FilterType")))) cust.add(idName(asInt(ci(r, "Id")), text(ci(r, "RefName"))));
        }
        out.put("customers", cust);
        out.put("consignees", cons);
        return out;
    }

    /** InvoiceNo(): GetInvoiceNoandPartiesForFcBankReceipts(customer) - FilterType "InvoiceNo". */
    public List<Map<String, Object>> invoices(int customerId) {
        UserAccount u = user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.invoicesAndParties(u, fy(), customerId))
            if ("InvoiceNo".equals(text(ci(r, "FilterType")))) out.add(idName(asInt(ci(r, "Id")), text(ci(r, "RefName"))));
        return out;
    }

    /** LcOrder(): GetLcOrderNoBySupplierCustomerId(customer). */
    public List<Map<String, Object>> lcOrders(int customerId) {
        UserAccount u = user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.lcOrders(u, fy(), customerId)) out.add(idName(asInt(ci(r, "Id")), text(ci(r, "LcOrderNo"))));
        return out;
    }

    /** InvoiceNoForBreakUp(): Id, InvoiceNo, EFormNo, InvoiceStatus, DocumentTypeId. */
    public List<Map<String, Object>> breakupInvoices() {
        UserAccount u = user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.breakupInvoices(u.getOrganizationId(), u.getOrganizationId())) {
            Map<String, Object> m = idName(asInt(ci(r, "Id")), text(ci(r, "InvoiceNo")));
            m.put("EFormNo", text(ci(r, "EFormNo")));
            m.put("InvoiceStatus", text(ci(r, "InvoiceStatus")));
            m.put("DocumentTypeId", asInt(ci(r, "DocumentTypeId")));
            out.add(m);
        }
        return out;
    }

    /**
     * InvoiceAmountGet(): breakup mode (term 4 + Bank) USP_GetFcyBankInvoicesBalance(breakup invoice); otherwise
     * GetFcYAmount(invoice) less PreviousBalance. Then GetExchangeRateandCurrencyFromVoucher.
     */
    public Map<String, Object> invoiceInfo(boolean breakupMode, int invoiceId, double previousBalance) {
        UserAccount u = user("View");
        double inv = 0, rec = 0, bal = 0;
        if (breakupMode) {
            List<Map<String, Object>> r = repo.fcyBankInvoicesBalance(u, invoiceId);
            if (!r.isEmpty()) { inv = asDouble(ci(r.get(0), "InvoiceAmount")); rec = asDouble(ci(r.get(0), "FcyReceivedAmount")); bal = inv - rec; }
        } else if (invoiceId > 0) {
            List<Map<String, Object>> r = repo.invoiceActivity(u, invoiceId, "GetFcYAmount");
            if (!r.isEmpty()) {
                inv = asDouble(ci(r.get(0), "InvoiceAmount"));
                rec = asDouble(ci(r.get(0), "FcyReceivedAmount")) - previousBalance;
                bal = inv - rec;
            }
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("InvoiceAmount", inv);
        out.put("ReceivedAmount", rec);
        out.put("BalanceAmount", bal);
        List<Map<String, Object>> x = repo.invoiceActivity(u, invoiceId, "GetExchangeRateandCurrencyFromVoucher");
        if (!x.isEmpty()) {
            out.put("VoucherRate", asDouble(ci(x.get(0), "ExchangeCurrencyRate")));
            out.put("MultiCurrencyId", asInt(ci(x.get(0), "MultiCurrencyId")));
            out.put("VoucherRateFound", true);
        } else {
            out.put("VoucherRate", 0d);
            out.put("VoucherRateFound", false);
        }
        return out;
    }

    /** getPaymentTermDetailByInvoiceId(InvoiceId, Id). */
    public List<Map<String, Object>> paymentTerms(int invoiceId, int recId) {
        user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.paymentTermDetail(invoiceId, recId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("InvoicePaymentDetailId", asInt(ci(r, "Id")));
            m.put("PaymentTermId", asInt(ci(r, "PaymentTermId")));
            m.put("PaymentTerm", text(ci(r, "PaymentTerm")));
            m.put("Amount", asDouble(ci(r, "FcyAmount")));
            m.put("Received", asDouble(ci(r, "Received")));
            m.put("Balance", asDouble(ci(r, "Balance")));
            m.put("ThisReceipt", asDouble(ci(r, "ThisReceipt")));
            out.add(m);
        }
        return out;
    }

    /** SalesContractAmountGet(): GetFcYAmountByContractId. */
    public Map<String, Object> contractAmount(int contractId) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> r = repo.invoiceActivity(u, contractId, "GetFcYAmountByContractId");
        out.put("found", !r.isEmpty());
        if (!r.isEmpty()) {
            double inv = asDouble(ci(r.get(0), "InvoiceAmount")), rec = asDouble(ci(r.get(0), "FcyReceivedAmount"));
            out.put("InvoiceAmount", inv);
            out.put("ReceivedAmount", rec);
            out.put("BalanceAmount", inv - rec);
        }
        return out;
    }

    /** cmbInvoicenobreakup_Leave -> GetGDsAgainstAdvancePaymentUtilizeInInvoice(org, comp, invoice, GdBreakUpId, RefDocumentTypeId). */
    public List<Map<String, Object>> gds(int invoiceId, int gdBreakUpId, int refDocumentTypeId) {
        UserAccount u = user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.gdsForInvoice(u, invoiceId, gdBreakUpId, refDocumentTypeId)) {
            Map<String, Object> m = idName(asInt(ci(r, "Id")), text(ci(r, "GDNO")));
            m.put("GDValue", asDouble(ci(r, "GDValue")));
            m.put("UtilizeAmount", asDouble(ci(r, "UtilizeAmount")));
            m.put("GDBalance", asDouble(ci(r, "GDBalance")));
            m.put("BankInvoiceNo", text(ci(r, "BankInvoiceNo")));
            m.put("DocumentTypeId", asInt(ci(r, "DocumentTypeId")));
            out.add(m);
        }
        return out;
    }

    /** VoucherHeadIdGet - CommonServices.VoucherHeadIdGet(Id, 203). */
    public Map<String, Object> voucherHead(int id) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("voucherHeadId", id > 0 ? repo.voucherHeadId(u, DOCUMENT_TYPE_ID, id) : 0);
        return out;
    }

    /** GainAndLossBreakUp pop-up (minimum): USP_GetGainAndLossBreackup(@Id) rows. */
    public List<Map<String, Object>> gainLossBreakup(int id) {
        user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.gainAndLossBreakup(id)) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) {
                Object v = e.getValue();
                if (v instanceof Timestamp) v = ((Timestamp) v).toLocalDateTime().toString();
                else if (v instanceof java.util.Date) v = new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().toString();
                else if (v instanceof BigDecimal) v = ((BigDecimal) v).doubleValue();
                m.put(e.getKey(), v);
            }
            out.add(m);
        }
        return out;
    }

    // ================================================================= ReadById

    /** ExImFCBankReceipts.GetByID(ID) - header + charges + invoice breakups + party breakups + shipment advances. */
    public Map<String, Object> readById(int id) {
        UserAccount u = user("View");
        List<Map<String, Object>> h = repo.byIdActivity(id, "ReadById");
        if (h.isEmpty()) throw new IllegalArgumentException("Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index");
        Map<String, Object> r = h.get(0);
        Map<String, Object> hd = new LinkedHashMap<>();
        for (String k : new String[] {"Id", "DocumentNo", "SupplierCustomerId", "ExImLcOrderId", "ExImInvoiceId", "DebitAccountTypeId",
                "BankGlDrAc", "MultiCurrencyId", "ThirdCurrencyId", "VoucherHeadId", "FBCDebitAccountId", "TransTypeId"})
            hd.put(k, asInt(ci(r, k)));
        for (String k : new String[] {"FcGrossAmount", "FcNetAmount", "ExchangeRate", "ThirdCurrencyHcyExchangeRate", "ThirdCurrencyFcyExchangeRate",
                "ThirdCurrencyAmount", "ThirdCurrencyReceiverExchangeRate", "ThirdCurrencyReceiverFcyAmount", "NetAmountRs", "BankDrAmount", "FcFBCharges"})
            hd.put(k, asDouble(ci(r, k)));
        for (String k : new String[] {"ExmLcPaymentTerm", "BankFbpNo", "Remarks", "ReferenceNo", "CompanyName", "InvoiceNo", "LcOrderNo", "ThirdCurrencyCode"})
            hd.put(k, text(ci(r, k)));
        hd.put("DocumentDate", iso(ci(r, "DocumentDate")));
        String fbp = text(ci(r, "BankFbpNo"));
        List<Map<String, Object>> ded = new ArrayList<>();
        for (Map<String, Object> d : repo.byIdActivity(id, "ReadByHeaderId")) {
            Map<String, Object> m = new LinkedHashMap<>();
            String f = text(ci(d, "FDBCNo"));
            m.put("FDBCNo", f.isEmpty() ? fbp : f);
            m.put("TypeId", 1);
            m.put("Type", text(ci(d, "Type")));
            m.put("ChargesTypeId", asInt(ci(d, "ExImProceedsChargesTypeId")));
            m.put("ChargesType", text(ci(d, "ProceedsChargesTypeCode")));
            m.put("Prcnt", asDouble(ci(d, "ChargesPercent")));
            m.put("Remarks", text(ci(d, "ProceedsRemarks")));
            m.put("FCAmount", asDouble(ci(d, "FcAmount")));
            m.put("RsAmount", asDouble(ci(d, "RsAmount")));
            ded.add(m);
        }
        List<Map<String, Object>> brk = new ArrayList<>();
        for (Map<String, Object> b : repo.byIdActivity(id, "ReadBreakupByHeaderId")) {
            Map<String, Object> m = new LinkedHashMap<>();
            double realized = asDouble(ci(b, "RealizedAmount")), comm = asDouble(ci(b, "AgencyComm"));
            m.put("Id", asInt(ci(b, "Id")));
            m.put("InvoiceId", asInt(ci(b, "RefDocRecordId")));
            m.put("InvoiceNo", text(ci(b, "InvoiceNo")));
            m.put("GDId", asInt(ci(b, "GDId")));
            m.put("GDNO", text(ci(b, "GDNo")));
            m.put("GdRefDocTypeId", asInt(ci(b, "GdRefDocTypeId")));
            m.put("InvoiceAmount", realized + comm);
            m.put("RealizedAmount", realized);
            m.put("ForiegnAgencyCommAmount", comm);
            m.put("FDBCNo", text(ci(b, "ReceiverRefNo")));
            m.put("Status", text(ci(b, "StepStatus")));
            brk.add(m);
        }
        List<Map<String, Object>> party = new ArrayList<>();
        for (Map<String, Object> p : repo.byIdActivity(id, "ReadPartyBreakupByHeaderId")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("RealizedAmount", asDouble(ci(p, "RealizedAmount")));
            m.put("FBC", asDouble(ci(p, "FBC")));
            m.put("TotalAmount", asDouble(ci(p, "TotalAmount")));
            party.add(m);
        }
        List<Map<String, Object>> ship = new ArrayList<>();
        for (Map<String, Object> s : repo.byIdActivity(id, "ReadExImShipmentPartyAgainstAdvancesPaymentByHeaderId")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ConsigneeId", asInt(ci(s, "ConsigneeId")));
            m.put("ContractId", asInt(ci(s, "ContractId")));
            m.put("FINo", text(ci(s, "FINo")));
            ship.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", hd);
        out.put("charges", ded);
        out.put("breakups", brk);
        out.put("partyBreakups", party);
        out.put("shipmentAdvances", ship);
        out.put("voucherHeadId", repo.voucherHeadId(u, DOCUMENT_TYPE_ID, id));
        return out;
    }

    // ================================================================= history

    private Map<String, Object> historyCombos(UserAccount u) {
        List<Map<String, Object>> c = new ArrayList<>(), t = new ArrayList<>(), a = new ArrayList<>(), i = new ArrayList<>();
        for (Map<String, Object> r : repo.historyDropDowns(u)) {
            String act = text(ci(r, "Activity"));
            Map<String, Object> m = idName(asInt(ci(r, "Id")), text(ci(r, "ReferenceName")));
            if ("Customer".equals(act)) c.add(m);
            else if ("PaymentTerm".equals(act)) t.add(m);
            else if ("ReceiverAccount".equals(act)) a.add(m);
            else if ("InvoiceNo".equals(act)) i.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("historyCustomers", c);
        out.put("historyPaymentTerms", t);
        out.put("historyReceiverAccounts", a);
        out.put("historyInvoices", i);
        return out;
    }

    /** btnRefreshHistory_Click -> HistoryCombosFill. */
    public Map<String, Object> historyComboRefresh() { return historyCombos(user("View")); }

    /** bindHistory -> ExImFCBankReceipts.HistoryForm. */
    public List<Map<String, Object>> history(Map<String, Object> f) {
        UserAccount u = user("View");
        boolean can = allowed(u, "CanViewAllRecord");
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        p.put("CanViewAllRecord", can);
        if (fy() != 0) p.put("FinancialYearId", fy());
        if (!can) p.put("EntryUser", u.getId());
        LocalDate from = asBool(f.get("fromChecked")) ? asDate(f.get("fromDate")) : null;
        LocalDate to = asBool(f.get("toChecked")) ? asDate(f.get("toDate")) : null;
        if (from != null) p.put("FromDate", Timestamp.valueOf(from.atTime(LocalTime.now().withNano(0))));
        if (to != null) p.put("ToDate", Timestamp.valueOf(to.atTime(LocalTime.now().withNano(0))));
        int cust = asInt(f.get("customerId")), fromNo = asInt(f.get("fromDocNo")), toNo = asInt(f.get("toDocNo"));
        int recv = asInt(f.get("receiverAccountId")), inv = asInt(f.get("invoiceId"));
        String term = str(f.get("paymentTerm"));
        if (cust != 0) p.put("SupplierCustomerId", cust);
        if (fromNo != 0) p.put("DocNoFrom", fromNo);
        if (toNo != 0) p.put("DocNoTo", toNo);
        if (recv != 0) p.put("ReceiverAccountId", recv);
        if (inv != 0) p.put("InvoiceId", inv);
        if (!term.isEmpty()) p.put("PaymentTerm", term);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.history(p)) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (String k : new String[] {"Id", "VoucherHeadId", "DocumentTypeId", "DocumentNo", "NoOfAttachments"}) m.put(k, asInt(ci(r, k)));
            m.put("DocumentDate", iso(ci(r, "DocumentDate")));
            for (String k : new String[] {"CustomerName", "InvoiceNo", "PaymentTerm", "AccountType", "BankGLDrAc", "BankFbpNo", "CurrencyCode",
                    "ReferenceNo", "FBCAccount", "UserName"}) m.put(k, text(ci(r, k)));
            for (String k : new String[] {"BankDrAmount", "FcGrossAmount", "FcFBCharges", "FcNetAmount", "ExchangeRate", "NetAmountRs"}) m.put(k, asDouble(ci(r, k)));
            m.put("EntryDate", isoDateTime(ci(r, "EntryDate")));
            out.add(m);
        }
        return out;
    }

    /** Print gate (Voucher102 / 516-Print / history Slip & Voucher); the PDFs go through ReportRegistry. */
    public Map<String, Object> printCheck(int id) {
        user("Print");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", id);
        return out;
    }

    // ================================================================= Insert

    @SuppressWarnings("unchecked")
    public Map<String, Object> save(Map<String, Object> b) {
        int recId = asInt(b.get("recId"));
        UserAccount u = user(recId > 0 ? "Update" : "Save");
        String term = str(b.get("paymentTerm"));
        int termId = asInt(b.get("paymentTermId"));
        int acctType = asInt(b.get("accountTypeId"));
        formValidation(b, term, termId);

        double gross = asDouble(b.get("FcGrossAmount")), fbc = asDouble(b.get("FcFBCharges"));
        List<Map<String, Object>> breakRows = list(b.get("breakups"));
        if ("Invoice Payment".equals(term)) {
            double balance = asDouble(b.get("BalanceAmount"));
            if (gross > balance) throw new IllegalArgumentException("FCY Amount cannot be greater than Balance Amount please check");
            double thisReceipt = 0;
            for (Map<String, Object> pt : list(b.get("paymentTerms"))) thisReceipt += asDouble(pt.get("ThisReceipt"));
            if (BigDecimal.valueOf(thisReceipt).compareTo(BigDecimal.valueOf(gross).add(BigDecimal.valueOf(fbc))) != 0)
                throw new IllegalArgumentException("Fcy Amount not equal to Payment term grid total. Fcy Amount is " + cs(gross) + " & Payment Grid Total is " + cs(thisReceipt) + ".");
        } else if (termId == 4 && acctType == 1) {
            double sum = 0;
            for (Map<String, Object> r : breakRows) sum += asDouble(r.get("RealizedAmount"));
            if (sum != gross) throw new IllegalArgumentException("Total Realized Amount Is Not Equal to Fc Amount. Please Check!");
        }
        double thirdAmt = asDouble(b.get("ThirdCurrencyAmount"));
        if (thirdAmt > 0) {
            if (asInt(b.get("ThirdCurrencyId")) == 0) throw new IllegalArgumentException("Third Currency field is required");
            if (asDouble(b.get("ThirdCurrencyFcyExchangeRate")) == 0) throw new IllegalArgumentException("Third Currency Customer ExchangeRate field is required");
        }

        int transType = asInt(b.get("transTypeId"));
        boolean branchReset = asBool(b.get("branchReset"));
        int branchId = branchReset ? 0 : repo.firstBranchId(u);
        int projectId = branchReset ? 0 : repo.firstProjectId(u);
        LocalDateTime now = LocalDateTime.now();
        LocalDate docDate = asDate(b.get("DocumentDate"));
        if (docDate == null) docDate = LocalDate.now();
        LocalDateTime docDateTime = docDate.atTime(LocalTime.now().withNano(0));
        double exRate = asDouble(b.get("ExchangeRate"));

        /* the model (ExImFCBankReceipts) in property order */
        Map<String, Object> fc = new LinkedHashMap<>();
        fc.put("IsApproved", false);
        fc.put("PostState", false);
        fc.put("DocumentDate", Timestamp.valueOf(docDateTime));
        fc.put("EntryDate", Timestamp.valueOf(now));
        fc.put("ModifyDate", Timestamp.valueOf(now));
        fc.put("PostDate", Timestamp.valueOf(now));
        fc.put("BankDrAmount", asDouble(b.get("BankDrAmount")));
        fc.put("ExchangeRate", exRate);
        fc.put("FcFBCharges", fbc);
        fc.put("FcGrossAmount", gross);
        fc.put("FcNetAmount", asDouble(b.get("FcNetAmount")));
        fc.put("NetAmountRs", asDouble(b.get("NetAmountRs")));
        fc.put("ThirdCurrencyReceiverExchangeRate", asDouble(b.get("ThirdCurrencyReceiverExchangeRate")));
        fc.put("ThirdCurrencyReceiverFcyAmount", asDouble(b.get("ThirdCurrencyReceiverFcyAmount")));
        fc.put("DebitAccountCurrencyId", 0);
        fc.put("DebitAccountCurrencyAmount", 0d);
        fc.put("DebitAccountCurrencyExchangeRate", 0d);
        fc.put("BankGlDrAc", asInt(b.get("BankGlDrAc")));
        fc.put("BranchId", branchId);
        fc.put("CompanyId", u.getCompanyId());
        fc.put("DocumentNo", asInt(b.get("DocumentNo")));
        fc.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        fc.put("EntryUser", u.getId());
        fc.put("ExImInvoiceId", asInt(b.get("ExImInvoiceId")));
        fc.put("DebitAccountTypeId", acctType);
        fc.put("ExImLcOrderId", asInt(b.get("ExImLcOrderId")));
        fc.put("FinancialYearId", fy());
        fc.put("Id", recId);
        fc.put("ModifyUser", u.getId());
        fc.put("MultiCurrencyId", asInt(b.get("MultiCurrencyId")));
        fc.put("OrganizationId", u.getOrganizationId());
        fc.put("PostUser", 0);
        fc.put("ProjectId", projectId);
        fc.put("SupplierCustomerId", asInt(b.get("SupplierCustomerId")));
        fc.put("VoucherHeadId", asInt(b.get("GainLossAccountId")));
        fc.put("ThirdCurrencyId", asInt(b.get("ThirdCurrencyId")));
        fc.put("FBCDebitAccountId", asInt(b.get("FBCDebitAccountId")));
        fc.put("TransTypeId", transType);
        fc.put("ThirdCurrencyHcyExchangeRate", asDouble(b.get("ThirdCurrencyHcyExchangeRate")));
        fc.put("ThirdCurrencyFcyExchangeRate", asDouble(b.get("ThirdCurrencyFcyExchangeRate")));
        fc.put("ThirdCurrencyAmount", thirdAmt);
        fc.put("BankFbpNo", str(b.get("BankFbpNo")));
        fc.put("ExmLcPaymentTerm", term);
        fc.put("FcBankChargesTypeAcId", 0);
        fc.put("FcBankChargesAmountRs", fbc * exRate);
        fc.put("Remarks", str(b.get("Remarks")));
        fc.put("ReferenceNo", str(b.get("ReferenceNo")));
        /* virtual values MakeVoucher reads */
        Map<String, Object> v = new LinkedHashMap<>();
        v.put("CompanyName", str(b.get("BankAccountText")));
        v.put("InvoiceNo", str(b.get("InvoiceNoText")));
        v.put("CurrencyCode", str(b.get("CurrencyCode")));
        v.put("ThirdCurrencyCode", str(b.get("ThirdCurrencyCode")));
        v.put("VoucherExchangeRate", asDouble(b.get("VoucherExchangeRate")));

        /* FcBankReceiptDetail */
        List<Map<String, Object>> ded = new ArrayList<>();
        for (Map<String, Object> r : list(b.get("charges"))) {
            String fdbc = str(r.get("FDBCNo"));
            if ((transType == 2 || transType == 3) && termId == 4 && acctType != 4 && fdbc.isEmpty())
                throw new IllegalArgumentException("FDBCNo Field is Required in Charges Grid ......");
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("FcAmount", asDouble(r.get("FCAmount")));
            d.put("RsAmount", asDouble(r.get("RsAmount")));
            d.put("ChargesPercent", asDouble(r.get("Prcnt")));
            d.put("ExImFCBankReceiptsId", 0);
            d.put("ExImProceedsChargesTypeId", asInt(r.get("ChargesTypeId")));
            d.put("Id", 0);
            d.put("FDBCNo", fdbc);
            d.put("ProceedsRemarks", str(r.get("Remarks")));
            d.put("Type", str(r.get("Type")));
            ded.add(d);
        }
        /* FcyBankReceiptBreakUpList */
        List<Map<String, Object>> brk = new ArrayList<>();
        String prevFdbc = "";
        int idx = 0;
        for (Map<String, Object> r : breakRows) {
            Map<String, Object> m = new LinkedHashMap<>();
            int bid = asInt(r.get("Id"));
            String recv = str(r.get("FDBCNo"));
            if (transType == 2 || transType == 3) {
                if (idx == 0) prevFdbc = recv;
                if (prevFdbc.equals(recv) && idx != 0) throw new IllegalArgumentException("FDBC # already add in Grid....");
                prevFdbc = recv;
            }
            m.put("Id", bid);
            m.put("FcyBankReceiptId", 0);
            m.put("SenderRefNo", "");
            m.put("ReceiverRefNo", recv);
            m.put("RefDocumentTypeId", 4);
            m.put("RefDocRecordId", asInt(r.get("InvoiceId")));
            m.put("RealizedAmount", dec(r.get("RealizedAmount")));
            m.put("AgencyComm", dec(r.get("ForiegnAgencyCommAmount")));
            m.put("StepStatus", str(r.get("Status")));
            m.put("SortNo", 1);
            m.put("ActionTypeId", bid == 0 ? 1 : 2);
            m.put("GDId", asInt(r.get("GDId")));
            m.put("GdRefDocTypeId", asInt(r.get("GdRefDocTypeId")));
            brk.add(m);
            idx++;
        }
        if (termId == 1 || termId == 2) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", 0);
            m.put("FcyBankReceiptId", 0);
            m.put("SenderRefNo", "");
            m.put("ReceiverRefNo", str(b.get("BankFbpNo")));
            m.put("RefDocumentTypeId", termId == 1 ? 1 : 2);
            m.put("RefDocRecordId", termId == 1 ? asInt(b.get("ExImInvoiceId")) : asInt(b.get("ExImLcOrderId")));
            m.put("RealizedAmount", BigDecimal.valueOf(gross));
            m.put("AgencyComm", BigDecimal.ZERO);
            m.put("StepStatus", "Final Part");
            m.put("SortNo", 1);
            m.put("ActionTypeId", 1);
            m.put("GDId", 0);
            m.put("GdRefDocTypeId", 0);
            brk.add(m);
        }
        /* FcyPartyPaymentBreackUpForFinancialslist */
        List<Map<String, Object>> party = new ArrayList<>();
        if (transType == 1 || transType == 3) {
            BigDecimal realizedSum = BigDecimal.ZERO;
            for (Map<String, Object> r : list(b.get("partyBreakups"))) {
                BigDecimal total = dec(r.get("TotalAmount"));
                if (total.compareTo(BigDecimal.ZERO) > 0) {
                    Map<String, Object> m = new LinkedHashMap<>();
                    m.put("FBC", dec(r.get("FBC")));
                    BigDecimal realized = dec(r.get("RealizedAmount"));
                    m.put("RealizedAmount", realized);
                    m.put("TotalAmount", total);
                    m.put("ExchangeRate", BigDecimal.ZERO);
                    m.put("LcyAmount", BigDecimal.ZERO);
                    m.put("FcyReceiptId", 0);
                    m.put("Id", 0);
                    if (realized.compareTo(BigDecimal.ZERO) == 0) throw new IllegalArgumentException("RealizedAmount Field is Required in PartyPaymentsBreackUp");
                    party.add(m);
                    realizedSum = realizedSum.add(realized);
                }
            }
            if (realizedSum.compareTo(BigDecimal.valueOf(gross)) != 0) throw new IllegalArgumentException("Total Party Realized Amount not equal to FCY Amount please Check");
        }
        if (termId == 4 && acctType == 1 && brk.isEmpty()) throw new IllegalArgumentException("InvoiceWise BreakUp Not Found in Grid please Check");
        /* FcyBankReceiptPaymentTermDetails */
        List<Map<String, Object>> terms = new ArrayList<>();
        if ("Invoice Payment".equals(term)) {
            int line = 1;
            for (Map<String, Object> r : list(b.get("paymentTerms"))) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("ThisReceipt", asDouble(r.get("ThisReceipt")));
                m.put("ExImInvoicePaymentTermsDetailId", asInt(r.get("InvoicePaymentDetailId")));
                m.put("FcyBankReceiptId", 0);
                m.put("Id", 0);
                m.put("LineId", line++);
                m.put("PaymentTermId", asInt(r.get("PaymentTermId")));
                m.put("ExImInvoiceId", asInt(b.get("ExImInvoiceId")));
                m.put("AdjustmentVoucherId", 0);
                terms.add(m);
            }
        }
        /* ExImShipmentPartyAgainstAdvancesPaymentslist */
        List<Map<String, Object>> ship = new ArrayList<>();
        int consignee = asInt(b.get("ConsigneeId"));
        String fiNo = str(b.get("FINo")).trim();
        if ((termId == 1 || termId == 2) && consignee > 0 && !fiNo.isEmpty()) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ConsigneeId", consignee);
            m.put("ContractId", asInt(b.get("ExImLcOrderId")));
            m.put("FcyReceiptId", 0);
            m.put("Id", 0);
            m.put("FINo", fiNo);
            ship.add(m);
        }

        /* ExImFCBankReceipts.Save -> MakeVoucher -> DAL SetDate */
        List<Map<String, Object>> partyInvoices = new ArrayList<>();
        List<Map<String, Object>> gainLoss = new ArrayList<>();
        Map<String, Object> vh = makeVoucher(u, fc, v, ded, brk, party, partyInvoices, gainLoss);
        List<Map<String, Object>> vds = (List<Map<String, Object>>) vh.remove("__details");
        int id = repo.save(recId == 0, fc, ded, brk, partyInvoices, gainLoss, party, ship, terms, vh, vds, VH, VD);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", id);
        out.put("documentNo", fc.get("DocumentNo"));
        out.put("voucherHeadId", repo.voucherHeadId(u, DOCUMENT_TYPE_ID, id));
        out.put("message", (recId == 0 ? "Record Save Successfully [" : "Record Update Successfully [") + fc.get("DocumentNo") + "]");
        return out;
    }

    /** FormValidation - the desktop's texts in its order. */
    private static void formValidation(Map<String, Object> b, String term, int termId) {
        String fbp = str(b.get("BankFbpNo")).trim();
        if (fbp.isEmpty() || "0".equals(fbp)) throw new IllegalArgumentException("FBP Bank Field Required");
        if (asDouble(b.get("FcGrossAmount")) == 0) throw new IllegalArgumentException("Fc Gross Field Required");
        if (asDouble(b.get("FcNetAmount")) == 0) throw new IllegalArgumentException("Fc Net Amount Field Required");
        if (asDouble(b.get("ExchangeRate")) == 0) throw new IllegalArgumentException("Exchange Rate Field Required");
        if (asDouble(b.get("NetAmountRs")) == 0) throw new IllegalArgumentException("Net Amount Field Required");
        if (asInt(b.get("SupplierCustomerId")) == 0) throw new IllegalArgumentException("Customer  Field Required");
        if (termId == 0 || term.isEmpty()) throw new IllegalArgumentException("Payment term Field Required");
        if ("Invoice Payment".equals(term) && asInt(b.get("GainLossAccountId")) == 0) throw new IllegalArgumentException("GainLossAc Field Required");
        if (termId == 2 && asInt(b.get("ConsigneeId")) == 0) throw new IllegalArgumentException("Consignee Field Required");
        if (termId == 2 && str(b.get("FINo")).isEmpty()) throw new IllegalArgumentException("FI No Field Required");
        if (termId == 1 && str(b.get("InvoiceNoText")).isEmpty()) throw new IllegalArgumentException("Invoice Field Required");
        if (asInt(b.get("accountTypeId")) == 0) throw new IllegalArgumentException(" Account Type Field Required");
        if (asDouble(b.get("FcFBCharges")) > 0 && asInt(b.get("FBCDebitAccountId")) == 0) throw new IllegalArgumentException("FBC Debit Account Field Required");
        if (asInt(b.get("BankGlDrAc")) == 0) throw new IllegalArgumentException(" Account (DR) Field Required");
        if (asInt(b.get("MultiCurrencyId")) == 0) throw new IllegalArgumentException("Currency Code Field Required");
    }

    // ================================================================= MakeVoucher (BLL 0464 :111-1292)

    private Map<String, Object> makeVoucher(UserAccount u, Map<String, Object> o, Map<String, Object> v,
                                            List<Map<String, Object>> ded, List<Map<String, Object>> brk,
                                            List<Map<String, Object>> party, List<Map<String, Object>> partyInvoices,
                                            List<Map<String, Object>> gainLoss) {
        List<Map<String, Object>> charges = repo.chargesTypes(u);
        double num = 0, num2 = 0;
        String text = "";
        int id = asInt(o.get("Id")), supplierId = asInt(o.get("SupplierCustomerId")), bank = asInt(o.get("BankGlDrAc"));
        int mc = asInt(o.get("MultiCurrencyId")), fbcAc = asInt(o.get("FBCDebitAccountId")), acctType = asInt(o.get("DebitAccountTypeId"));
        int transType = asInt(o.get("TransTypeId")), thirdId = asInt(o.get("ThirdCurrencyId"));
        int gainLossAc = asInt(o.get("VoucherHeadId"));
        double ex = asDouble(o.get("ExchangeRate")), gross = asDouble(o.get("FcGrossAmount")), netRs = asDouble(o.get("NetAmountRs"));
        double fbc = asDouble(o.get("FcFBCharges")), bankDr = asDouble(o.get("BankDrAmount"));
        double tRecvRate = asDouble(o.get("ThirdCurrencyReceiverExchangeRate")), tRecvAmt = asDouble(o.get("ThirdCurrencyReceiverFcyAmount"));
        double tAmt = asDouble(o.get("ThirdCurrencyAmount")), tFcyRate = asDouble(o.get("ThirdCurrencyFcyExchangeRate"));
        double dacAmt = 0, dacRate = 0;      /* DebitAccountCurrencyAmount / ExchangeRate - never set by the form */
        int dacId = 0;
        double vRate = asDouble(v.get("VoucherExchangeRate"));
        String remarks = str(o.get("Remarks")), cur = str(v.get("CurrencyCode")), tCode = str(v.get("ThirdCurrencyCode"));
        String term = str(o.get("ExmLcPaymentTerm"));

        Map<String, Object> vh = voucherHeadDefaults();
        List<Map<String, Object>> lines = new ArrayList<>();
        vh.put("DocumentTypeId", o.get("DocumentTypeId"));
        vh.put("DocumentTypeSrNo", id);
        vh.put("RefDocNoId", id);
        vh.put("VoucherCode", asInt(o.get("DocumentNo")));
        vh.put("VoucherDate", o.get("DocumentDate"));
        int refAccount = 0;
        List<Map<String, Object>> sups = repo.exportCustomers(u);
        if (!sups.isEmpty()) {
            Map<String, Object> s = null;
            for (Map<String, Object> r : sups) if (asInt(ci(r, "Id")) == supplierId) { s = r; break; }
            if (s == null) throw new IllegalStateException("Supplier GLAccountId not Found");
            refAccount = asInt(ci(s, "GlAccountId"));
            text = text(ci(s, "CompanyName"));
        }
        vh.put("RefAccountId", refAccount);
        double num3 = 0;
        for (Map<String, Object> d : ded) if (asDouble(d.get("FcAmount")) > 0 && ex > 0) num3 += asDouble(d.get("FcAmount"));
        String text2 = "";
        if ("Invoice Payment".equals(term)) {
            if (!remarks.isEmpty()) text2 = text2 + remarks + "  ";
            text2 = text2 + "INVOICE NO " + str(v.get("InvoiceNo")) + "  " + cur + "  " + cs(gross);
            if (num3 > 0) { double num4 = gross - num3; text2 = text2 + " - " + cs(num3) + " = " + cs(num4); }
            Timestamp dd = (Timestamp) o.get("DocumentDate");
            text2 = text2 + " @" + cs(ex) + " REALIZED IN " + str(v.get("CompanyName")) + "  AS DATED "
                    + dd.toLocalDateTime().format(DateTimeFormatter.ofPattern("dd-MMM-yyyy", Locale.ENGLISH));
        }
        vh.put("Remarks", text2);
        vh.put("RemarksOtherLingo", "");
        vh.put("ChequeDate", Timestamp.valueOf(LocalDate.now().atStartOfDay()));
        vh.put("IncludeWHT", false);
        vh.put("BranchId", o.get("BranchId"));
        vh.put("ProjectId", o.get("ProjectId"));
        vh.put("ManualBillNo", str(o.get("ReferenceNo")));
        vh.put("DueDate", Timestamp.valueOf(LocalDateTime.now().withNano(0)));
        vh.put("DueDays", 0);
        vh.put("ExchangeCurrencyRate", ex);
        vh.put("FcAmount", gross);
        vh.put("MultiCurrencyId", mc);
        vh.put("OrganizationId", o.get("OrganizationId"));
        vh.put("CompanyId", o.get("CompanyId"));
        vh.put("FinancialYearId", o.get("FinancialYearId"));
        vh.put("EntryUser", o.get("EntryUser"));
        vh.put("EntryDate", Timestamp.valueOf(LocalDateTime.now().withNano(0)));
        vh.put("ModifyDate", Timestamp.valueOf(LocalDateTime.now().withNano(0)));
        vh.put("ModifyUser", o.get("ModifyUser"));
        vh.put("PostUser", 0);

        boolean flag = asBool(repo.config(u, "FCYReceiptAutoGainAndLoss"));
        if (!flag) {
            BigDecimal num5 = BigDecimal.ZERO;
            for (Map<String, Object> b : brk) num5 = num5.add((BigDecimal) b.get("AgencyComm"));
            if (acctType == 2 && ex > 0 && tRecvRate > 0 && tRecvAmt > 0) {
                double num7 = gross - tRecvAmt, num6 = tRecvAmt * ex;
                Map<String, Object> d = vd();
                d.put("AccountId", bank); d.put("AgainstAccountId", refAccount);
                d.put("DebitAmount", num6); num += num6; d.put("CreditAmount", 0d);
                d.put("Comments", tCode + "  " + cs(tAmt) + " Received From " + text + " @ " + cs(tFcyRate));
                d.put("DMultiCurrencyId", mc); d.put("DExchangeCurrencyRate", ex); d.put("DCurrencyAmount", tRecvAmt);
                d.put("ThirdCurrencyId", dacId); d.put("ThirdCurrencyFcyExchangeRate", dacRate);
                d.put("ThirdCurrencyAmount", (dacAmt > 0 && dacRate > 0) ? tRecvAmt * dacRate : dacAmt);
                d.put("ThirdCurrencyReceiverExchangeRate", tRecvRate); d.put("ThirdCurrencyReceiverFcyAmount", tRecvAmt);
                lines.add(d);
                if (num7 != 0) {
                    Map<String, Object> d2 = vd();
                    d2.put("AccountId", gainLossAc); d2.put("AgainstAccountId", refAccount);
                    if (num7 > 0) d2.put("DebitAmount", num7 * ex); else d2.put("CreditAmount", Math.abs(num7) * ex);
                    d2.put("Comments", tCode + "  " + cs(tAmt) + " Ex.Rate " + cs(tRecvRate) + "  Party Ex.Rate " + cs(tFcyRate));
                    d2.put("DMultiCurrencyId", mc); d2.put("DExchangeCurrencyRate", ex); d2.put("DCurrencyAmount", num7);
                    lines.add(d2);
                }
            } else {
                Map<String, Object> d = vd();
                d.put("AccountId", bank); d.put("AgainstAccountId", refAccount);
                d.put("DebitAmount", netRs); num += netRs; d.put("CreditAmount", 0d);
                String c = remarks + "  " + cur + "  " + cs(gross);
                if (num5.compareTo(BigDecimal.ZERO) > 0) c = c + "  FTT " + csM(num5);
                c = c + " @ " + cs(ex);
                d.put("Comments", c);
                d.put("DMultiCurrencyId", mc); d.put("DExchangeCurrencyRate", ex);
                double dca = netRs / ex; d.put("DCurrencyAmount", dca);
                d.put("ThirdCurrencyId", dacId); d.put("ThirdCurrencyFcyExchangeRate", dacRate);
                d.put("ThirdCurrencyAmount", (dacAmt > 0 && dacRate > 0) ? dca * dacRate : dacAmt);
                lines.add(d);
            }
            Map<String, Object> d4 = vd();
            d4.put("AccountId", refAccount); d4.put("AgainstAccountId", bank);
            d4.put("Comments", remarks + "   " + cur + "  " + cs(gross) + "  @ " + cs(ex));
            d4.put("DebitAmount", 0d); d4.put("CreditAmount", netRs); num2 += netRs;
            d4.put("DMultiCurrencyId", mc); d4.put("DExchangeCurrencyRate", ex);
            double dca4 = netRs / ex; d4.put("DCurrencyAmount", dca4);
            d4.put("ThirdCurrencyId", thirdId); d4.put("ThirdCurrencyFcyExchangeRate", tFcyRate);
            d4.put("ThirdCurrencyAmount", (tAmt > 0 && tFcyRate > 0) ? dca4 * tFcyRate : tAmt);
            d4.put("ThirdCurrencyReceiverExchangeRate", tRecvRate); d4.put("ThirdCurrencyReceiverFcyAmount", tRecvAmt);
            lines.add(d4);
            if (fbc > 0) {
                if (fbcAc == 0) throw new IllegalStateException("FBCDebitAccountId not found");
                double rate = vRate == 0 ? ex : vRate;
                Map<String, Object> d5 = vd();
                d5.put("AccountId", fbcAc); d5.put("AgainstAccountId", refAccount);
                d5.put("Comments", "Foreign Bank Charges " + cs(fbc));
                d5.put("DebitAmount", fbc * rate); d5.put("DMultiCurrencyId", mc); d5.put("DExchangeCurrencyRate", rate); d5.put("DCurrencyAmount", fbc);
                lines.add(d5);
                Map<String, Object> d6 = vd();
                d6.put("AccountId", refAccount); d6.put("AgainstAccountId", fbcAc);
                d6.put("Comments", "Foreign Bank Charges " + cs(fbc));
                double cr6 = fbc * rate; d6.put("CreditAmount", cr6); num2 += cr6;
                d6.put("DMultiCurrencyId", mc); d6.put("DExchangeCurrencyRate", rate); d6.put("DCurrencyAmount", fbc);
                d6.put("ThirdCurrencyId", thirdId); d6.put("ThirdCurrencyFcyExchangeRate", tFcyRate);
                d6.put("ThirdCurrencyAmount", (tAmt > 0 && tFcyRate > 0) ? cr6 / tFcyRate : tAmt);
                lines.add(d6);
            }
            if (!ded.isEmpty()) {
                for (Map<String, Object> r4 : ded) {
                    if (!charges.isEmpty()) {
                        double[] nn = chargeLines(lines, charges, r4, bank, mc, ex, dacId, dacRate, dacAmt, true, true);
                        num += nn[0]; num2 += nn[1];
                    }
                }
                vh.put("VoucherAmount", num);
                vh.put("BillAmount", num2);
            }
        } else {
            double num16, num17 = 0, num18 = 0;
            String text3 = "", text4 = "";
            for (Map<String, Object> d : ded) num18 += asDouble(d.get("RsAmount"));
            if ("Invoice Payment".equals(term) && vRate > 0) {
                bankAndThirdLines(lines, acctType, ex, tRecvRate, tRecvAmt, gross, bank, refAccount, gainLossAc, mc, text, cur, tCode, tAmt, tFcyRate,
                        thirdId, dacId, dacRate, dacAmt, netRs, text2, true, new double[] {0}, null);
                num += asDouble(lines.get(0).get("DebitAmount"));   /* only the bank debit (voucherDetail9 / voucherDetail11) is added to num */
                num16 = gross * vRate;
                num17 = bankDr + num18 - num16;
                text2 = text2 + " INVOICE EXCHANGE RATE IS @ " + cs(vRate);
                Map<String, Object> d12 = vd();
                d12.put("AccountId", refAccount); d12.put("AgainstAccountId", bank);
                d12.put("DebitAmount", 0d); d12.put("CreditAmount", num16);
                d12.put("DMultiCurrencyId", mc); d12.put("DExchangeCurrencyRate", ex);
                double dca12 = num16 / vRate; d12.put("DCurrencyAmount", dca12);
                d12.put("Comments", !text3.isEmpty() ? text2 + "  " + text3 : text2);
                text4 += str(d12.get("Comments"));
                num2 += num16;
                d12.put("ThirdCurrencyId", thirdId); d12.put("ThirdCurrencyFcyExchangeRate", tFcyRate);
                d12.put("ThirdCurrencyAmount", (tAmt > 0 && tFcyRate > 0) ? dca12 * tFcyRate : tAmt);
                lines.add(d12);
                if (fbc > 0) {
                    if (fbcAc == 0) throw new IllegalStateException("FBCDebitAccountId not found");
                    Map<String, Object> d13 = vd();
                    d13.put("AccountId", fbcAc); d13.put("AgainstAccountId", refAccount);
                    d13.put("Comments", "Foreign Bank Charges " + cs(fbc));
                    d13.put("DebitAmount", fbc * vRate); d13.put("DMultiCurrencyId", mc); d13.put("DExchangeCurrencyRate", vRate); d13.put("DCurrencyAmount", fbc);
                    lines.add(d13);
                    Map<String, Object> d14 = vd();
                    d14.put("AccountId", refAccount); d14.put("AgainstAccountId", fbcAc);
                    d14.put("Comments", "Foreign Bank Charges " + cs(fbc));
                    double cr14 = fbc * vRate; d14.put("CreditAmount", cr14); num2 += cr14;
                    d14.put("DMultiCurrencyId", mc); d14.put("DExchangeCurrencyRate", vRate); d14.put("DCurrencyAmount", fbc);
                    d14.put("ThirdCurrencyId", thirdId); d14.put("ThirdCurrencyFcyExchangeRate", tFcyRate);
                    d14.put("ThirdCurrencyAmount", (tAmt > 0 && tFcyRate > 0) ? fbc * tFcyRate : tAmt);
                    lines.add(d14);
                }
                String text5 = " FcyCode " + cur + " @Rate " + cs(vRate) + " InvoiceAmount " + cs(gross);
                gainLoss.add(gl(gross, num16, vRate, "Invoice", !text3.isEmpty() ? text5 + "  " + text3 : text5));
                if (num17 != 0) {
                    if (gainLossAc == 0) throw new IllegalStateException("Gain & Loss Account Not found....");
                    Map<String, Object> d16 = vd();
                    d16.put("AccountId", gainLossAc); d16.put("AgainstAccountId", refAccount);
                    if (num17 > 0) { d16.put("DebitAmount", 0d); d16.put("CreditAmount", num17); }
                    else { d16.put("CreditAmount", 0d); d16.put("DebitAmount", Math.abs(num17)); }
                    d16.put("Comments", text2);
                    d16.put("DMultiCurrencyId", mc); d16.put("DExchangeCurrencyRate", ex);
                    d16.put("DCurrencyAmount", Math.abs(num17) / vRate);
                    d16.put("RateCutAmount", num17);
                    lines.add(d16);
                }
            } else if ("Multi Invoices Payment".equals(term)) {
                if ((transType == 2 || transType == 3) && acctType == 1) {
                    if (brk.isEmpty()) throw new IllegalStateException("FcyBankReceiptBreakUpList not found");
                    for (Map<String, Object> item : brk) {
                        BigDecimal realized = (BigDecimal) item.get("RealizedAmount"), comm = (BigDecimal) item.get("AgencyComm");
                        Map<String, Object> d17 = vd();
                        d17.put("AccountId", bank); d17.put("AgainstAccountId", refAccount);
                        double dr17 = realized.doubleValue() * ex; d17.put("DebitAmount", dr17); num += dr17; d17.put("CreditAmount", 0d);
                        String c = remarks + "  " + cur + "  " + csM(realized);
                        if (comm.compareTo(BigDecimal.ZERO) > 0) c = c + "  FTT " + csM(comm);
                        d17.put("Comments", c + " @ " + cs(ex));
                        d17.put("DMultiCurrencyId", mc); d17.put("DExchangeCurrencyRate", ex); d17.put("DCurrencyAmount", realized.doubleValue());
                        lines.add(d17);
                        if (ded.isEmpty()) continue;
                        String recv = str(item.get("ReceiverRefNo")).toUpperCase(Locale.ROOT);
                        List<Map<String, Object>> list7 = new ArrayList<>();
                        for (Map<String, Object> x : ded) if (str(x.get("FDBCNo")).toUpperCase(Locale.ROOT).equals(recv)) list7.add(x);
                        if (list7.isEmpty()) throw new IllegalStateException("FDBCNO NOT MATCH INVOICE WISE BREAKUP AND CHARGES GRID");
                        for (Map<String, Object> r3 : list7) {
                            if (charges.isEmpty()) throw new IllegalStateException("Charges data not Found");
                            num2 += chargeLines(lines, charges, r3, bank, mc, ex, dacId, dacRate, dacAmt, false, true)[1];
                        }
                        for (Map<String, Object> r2 : list7) {
                            if (charges.isEmpty()) throw new IllegalStateException("Charges data not Found");
                            num += chargeLines(lines, charges, r2, bank, mc, ex, dacId, dacRate, dacAmt, true, false)[0];
                        }
                        vh.put("VoucherAmount", num);
                        vh.put("BillAmount", num2);
                    }
                } else if (acctType == 2 && ex > 0 && tRecvRate > 0 && tRecvAmt > 0) {
                    double num35 = gross - tRecvAmt, num34 = tRecvAmt * ex;
                    Map<String, Object> d20 = vd();
                    d20.put("AccountId", bank); d20.put("AgainstAccountId", refAccount);
                    d20.put("DebitAmount", num34); num += num34; d20.put("CreditAmount", 0d);
                    d20.put("Comments", tCode + "  " + cs(tAmt) + " Received From " + text + " @ " + cs(tFcyRate) + "  " + cur + "  " + cs(gross) + " @ " + cs(ex) + " Receiver Ex.Rate " + cs(tRecvRate));
                    d20.put("DMultiCurrencyId", mc); d20.put("DExchangeCurrencyRate", ex); d20.put("DCurrencyAmount", tRecvAmt);
                    d20.put("ThirdCurrencyId", thirdId); d20.put("ThirdCurrencyFcyExchangeRate", tFcyRate); d20.put("ThirdCurrencyAmount", tAmt);
                    d20.put("ThirdCurrencyReceiverExchangeRate", tRecvRate); d20.put("ThirdCurrencyReceiverFcyAmount", tRecvAmt);
                    lines.add(d20);
                    if (num35 != 0) {
                        Map<String, Object> d21 = vd();
                        d21.put("AccountId", gainLossAc); d21.put("AgainstAccountId", refAccount);
                        if (num35 > 0) d21.put("DebitAmount", num35 * ex); else d21.put("CreditAmount", Math.abs(num35) * ex);
                        d21.put("Comments", tCode + "  " + cs(tAmt) + " Ex.Rate " + cs(tRecvRate) + "  Party Ex.Rate " + cs(tFcyRate));
                        d21.put("DMultiCurrencyId", mc); d21.put("DExchangeCurrencyRate", ex); d21.put("DCurrencyAmount", num35);
                        lines.add(d21);
                    }
                } else {
                    Map<String, Object> d22 = vd();
                    d22.put("AccountId", bank); d22.put("AgainstAccountId", refAccount);
                    d22.put("DebitAmount", netRs); num += netRs; d22.put("CreditAmount", 0d);
                    d22.put("Comments", remarks + "  " + cur + "  " + cs(gross) + " @ " + cs(ex));
                    d22.put("DMultiCurrencyId", mc); d22.put("DExchangeCurrencyRate", ex); d22.put("DCurrencyAmount", netRs / ex);
                    lines.add(d22);
                }
                double num38 = 0, num39 = 0, num40, num41 = 0, num42, num43, num44, num45;
                List<Object[]> fifo = partyInvoicesFifo(u, o, cur, partyInvoices);
                if (!fifo.isEmpty()) {
                    for (Object[] f : fifo) { num38 += ((BigDecimal) f[0]).doubleValue(); num39 += ((BigDecimal) f[1]).doubleValue(); }
                    if (num38 > 0 && num39 > 0) {
                        for (Object[] f : fifo) {
                            gainLoss.add(gl(((BigDecimal) f[0]).doubleValue(), ((BigDecimal) f[1]).doubleValue(), ((BigDecimal) f[2]).doubleValue(), "Invoice", (String) f[3]));
                            text4 += (String) f[3];
                        }
                    }
                }
                if (gross > 0 && num38 > 0) {
                    num40 = gross - num38;
                    if (num40 != 0) {
                        List<Map<String, Object>> bal = fcyAndLcyBalance(u, o);
                        if (!bal.isEmpty()) {
                            if (asDouble(ci(bal.get(0), "FcyAmount")) > 0) {
                                num43 = asDouble(ci(bal.get(0), "FcyAmount"));
                                num45 = asDouble(ci(bal.get(0), "AvgExchangeRate"));
                                num44 = num43 - num38;
                                if (num44 >= num40 && num45 > 0) {
                                    num41 = num40 * num45;
                                    text4 = text4 + " LedgerBalance " + cs(num44) + " AvgExchangeRate " + cs(num45);
                                    gainLoss.add(gl(num40, num41, num45, "LedgerBalance", text4));
                                } else if (num44 < num40) {
                                    double num46 = num40 - num44;
                                    num41 = num44 * num45 + num46 * ex;
                                    if (num44 > 0) {
                                        text4 = text4 + " LedgerBalance " + cs(num44) + " AvgExchangeRate " + cs(num45);
                                        gainLoss.add(gl(num44, num44 * num45, num45, "LedgerBalance", text4));
                                    }
                                    if (num46 > 0) {
                                        text4 = text4 + " ExtraAmount " + cs(num46) + " ExchangeRate " + cs(ex);
                                        gainLoss.add(gl(num46, num46 * ex, ex, "ExtraAmount", text4));
                                    }
                                } else {
                                    num41 = num40 * ex;
                                }
                            } else {
                                num41 = num40 * ex;
                                text4 = text4 + " ExtraAmount " + cs(num40) + " ExchangeRate " + cs(ex);
                                gainLoss.add(gl(num40, num41, ex, "ExtraAmount", text4));
                            }
                        } else {
                            num41 = num40 * ex;
                        }
                    }
                } else {
                    List<Map<String, Object>> bal = fcyAndLcyBalance(u, o);
                    if (!bal.isEmpty()) {
                        if (asDouble(ci(bal.get(0), "FcyAmount")) > 0) {
                            num43 = asDouble(ci(bal.get(0), "FcyAmount"));
                            num45 = asDouble(ci(bal.get(0), "AvgExchangeRate"));
                            if (num43 >= gross && num45 > 0) {
                                num41 = gross * num45;
                                text4 = text4 + " LedgerBalance " + cs(num43) + " AvgExchangeRate " + cs(num45);
                                gainLoss.add(gl(gross, num41, num45, "LedgerBalance", text4));
                            } else if (num43 < gross) {
                                double num47 = gross - num43;
                                num41 = num43 * num45 + num47 * ex;
                                if (num43 > 0) {
                                    text4 = text4 + " LedgerBalance " + cs(num43) + " AvgExchangeRate " + cs(num45);
                                    gainLoss.add(gl(num43, num43 * num45, num45, "LedgerBalance", text4));
                                }
                                if (num47 > 0) {
                                    /* the desktop adds this row BEFORE appending its text to text4 */
                                    gainLoss.add(gl(num47, num47 * ex, ex, "ExtraAmount", text4));
                                    text4 = text4 + " ExtraAmount " + cs(num47) + " ExchangeRate " + cs(ex);
                                }
                            } else {
                                num41 = gross * ex;
                            }
                        } else {
                            num41 = gross * ex;
                            text4 = text4 + " ExtraAmount " + cs(gross) + " ExchangeRate " + cs(ex);
                            gainLoss.add(gl(gross, num41, ex, "ExtraAmount", text4));
                        }
                    } else {
                        num41 = gross * ex;
                        text4 = text4 + " ExtraAmount " + cs(gross) + " ExchangeRate " + cs(ex);
                        gainLoss.add(gl(gross, num41, ex, "ExtraAmount", text4));
                    }
                }
                num42 = num39 + num41;
                num17 = bankDr + num18 - num42;
                double num48 = netRs - (num42 + num17);
                num17 += num48;
                if ((transType == 1 || transType == 3) && acctType == 1) {
                    for (Map<String, Object> item5 : party) {
                        BigDecimal realized = (BigDecimal) item5.get("RealizedAmount");
                        Map<String, Object> d23 = vd();
                        d23.put("AccountId", refAccount); d23.put("AgainstAccountId", bank);
                        d23.put("Comments", remarks + "   " + cur + "  " + csM(realized) + "  @ " + cs(ex) + " FBC " + csM((BigDecimal) item5.get("FBC")));
                        double cr = num42 / gross * realized.doubleValue();
                        d23.put("CreditAmount", cr); num2 += cr;
                        d23.put("DMultiCurrencyId", mc); d23.put("DExchangeCurrencyRate", num42 / gross); d23.put("DCurrencyAmount", realized.doubleValue());
                        lines.add(d23);
                        item5.put("ExchangeRate", toDecimal(num42 / gross));
                        item5.put("LcyAmount", toDecimal(cr));
                    }
                    if (party.isEmpty()) throw new IllegalStateException("FcyPartyPaymentBreackUpForFinancialslist not found");
                } else {
                    Map<String, Object> d24 = vd();
                    d24.put("AccountId", refAccount); d24.put("AgainstAccountId", bank);
                    d24.put("Comments", remarks + "   " + cur + "  " + cs(gross) + "  @ " + cs(ex));
                    d24.put("DebitAmount", 0d); d24.put("CreditAmount", num42); num2 += num42;
                    d24.put("DMultiCurrencyId", mc); d24.put("DExchangeCurrencyRate", ex); d24.put("DCurrencyAmount", gross);
                    lines.add(d24);
                }
                if (fbc > 0) {
                    if (fbcAc == 0) throw new IllegalStateException("FBCDebitAccountId not found");
                    Map<String, Object> d25 = vd();
                    d25.put("AccountId", fbcAc); d25.put("AgainstAccountId", refAccount);
                    d25.put("Comments", "Foreign Bank Charges " + cs(fbc));
                    d25.put("DebitAmount", fbc * ex); d25.put("DMultiCurrencyId", mc); d25.put("DExchangeCurrencyRate", ex); d25.put("DCurrencyAmount", fbc);
                    lines.add(d25);
                    Map<String, Object> d26 = vd();
                    d26.put("AccountId", refAccount); d26.put("AgainstAccountId", fbcAc);
                    d26.put("Comments", "Foreign Bank Charges " + cs(fbc));
                    d26.put("CreditAmount", fbc * ex); num2 += fbc * ex;
                    d26.put("DMultiCurrencyId", mc); d26.put("DExchangeCurrencyRate", ex); d26.put("DCurrencyAmount", fbc);
                    lines.add(d26);
                }
                if (num17 != 0) {
                    Map<String, Object> d27 = vd();
                    d27.put("AccountId", gainLossAc); d27.put("AgainstAccountId", refAccount);
                    if (num17 > 0) { d27.put("DebitAmount", 0d); d27.put("CreditAmount", num17); }
                    else { d27.put("CreditAmount", 0d); d27.put("DebitAmount", Math.abs(num17)); }
                    d27.put("Comments", text4);
                    d27.put("DMultiCurrencyId", mc); d27.put("DExchangeCurrencyRate", ex);
                    d27.put("DCurrencyAmount", !(vRate > 0) ? Math.abs(num17) / ex : Math.abs(num17) / vRate);
                    d27.put("RateCutAmount", num17);
                    lines.add(d27);
                }
            } else {
                if (acctType == 2 && ex > 0 && tRecvRate > 0 && tRecvAmt > 0) {
                    double num54 = gross - tRecvAmt, num53 = tRecvAmt * ex;
                    Map<String, Object> d28 = vd();
                    d28.put("AccountId", bank); d28.put("AgainstAccountId", refAccount);
                    d28.put("DebitAmount", num53); num += num53; d28.put("CreditAmount", 0d);
                    d28.put("Comments", tCode + "  " + cs(tAmt) + " Received From " + text + " @ " + cs(tFcyRate) + "  " + cur + "  " + cs(gross) + " @ " + cs(ex) + " Receiver Ex.Rate " + cs(tRecvRate));
                    d28.put("DMultiCurrencyId", mc); d28.put("DExchangeCurrencyRate", ex); d28.put("DCurrencyAmount", tRecvAmt);
                    d28.put("ThirdCurrencyId", thirdId); d28.put("ThirdCurrencyFcyExchangeRate", tFcyRate); d28.put("ThirdCurrencyAmount", tAmt);
                    d28.put("ThirdCurrencyReceiverExchangeRate", tRecvRate); d28.put("ThirdCurrencyReceiverFcyAmount", tRecvAmt);
                    lines.add(d28);
                    if (num54 != 0) {
                        Map<String, Object> d29 = vd();
                        d29.put("AccountId", gainLossAc); d29.put("AgainstAccountId", refAccount);
                        if (num54 > 0) d29.put("DebitAmount", num54 * ex); else d29.put("CreditAmount", Math.abs(num54) * ex);
                        d29.put("Comments", tCode + "  " + cs(tAmt) + " Ex.Rate " + cs(tRecvRate) + "  Party Ex.Rate " + cs(tFcyRate));
                        d29.put("DMultiCurrencyId", mc); d29.put("DExchangeCurrencyRate", ex); d29.put("DCurrencyAmount", num54);
                        lines.add(d29);
                    }
                } else {
                    Map<String, Object> d30 = vd();
                    d30.put("AccountId", bank); d30.put("AgainstAccountId", refAccount);
                    d30.put("DebitAmount", netRs); num += netRs; d30.put("CreditAmount", 0d);
                    d30.put("Comments", remarks + "  " + cur + "  " + cs(gross) + " @ " + cs(ex));
                    d30.put("DMultiCurrencyId", mc); d30.put("DExchangeCurrencyRate", ex);
                    double dca = netRs / ex; d30.put("DCurrencyAmount", dca);
                    d30.put("ThirdCurrencyId", dacId); d30.put("ThirdCurrencyFcyExchangeRate", dacRate);
                    d30.put("ThirdCurrencyAmount", (dacAmt > 0 && dacRate > 0) ? dca * dacRate : dacAmt);
                    d30.put("ThirdCurrencyReceiverExchangeRate", tRecvRate); d30.put("ThirdCurrencyReceiverFcyAmount", tRecvAmt);
                    lines.add(d30);
                }
                Map<String, Object> d31 = vd();
                d31.put("AccountId", refAccount); d31.put("AgainstAccountId", bank);
                d31.put("Comments", remarks + "   " + cur + "  " + cs(gross) + "  @ " + cs(ex));
                d31.put("DebitAmount", 0d); d31.put("CreditAmount", netRs); num2 += netRs;
                d31.put("DMultiCurrencyId", mc); d31.put("DExchangeCurrencyRate", ex);
                double dca31 = netRs / ex; d31.put("DCurrencyAmount", dca31);
                d31.put("ThirdCurrencyId", thirdId); d31.put("ThirdCurrencyFcyExchangeRate", tFcyRate);
                d31.put("ThirdCurrencyAmount", (tAmt > 0 && tFcyRate > 0) ? dca31 * tFcyRate : tAmt);
                d31.put("ThirdCurrencyReceiverExchangeRate", tRecvRate); d31.put("ThirdCurrencyReceiverFcyAmount", tRecvAmt);
                lines.add(d31);
                if (fbc > 0) {
                    if (fbcAc == 0) throw new IllegalStateException("FBCDebitAccountId not found");
                    Map<String, Object> d32 = vd();
                    d32.put("AccountId", fbcAc); d32.put("AgainstAccountId", refAccount);
                    d32.put("Comments", "Foreign Bank Charges " + cs(fbc));
                    d32.put("DebitAmount", fbc * ex); d32.put("DMultiCurrencyId", mc); d32.put("DExchangeCurrencyRate", ex); d32.put("DCurrencyAmount", fbc);
                    lines.add(d32);
                    Map<String, Object> d33 = vd();
                    d33.put("AccountId", refAccount); d33.put("AgainstAccountId", fbcAc);
                    d33.put("Comments", "Foreign Bank Charges " + cs(fbc));
                    d33.put("CreditAmount", fbc * ex); num2 += fbc * ex;
                    d33.put("DMultiCurrencyId", mc); d33.put("DExchangeCurrencyRate", ex); d33.put("DCurrencyAmount", fbc);
                    d33.put("ThirdCurrencyId", thirdId); d33.put("ThirdCurrencyFcyExchangeRate", tFcyRate);
                    d33.put("ThirdCurrencyAmount", (tAmt > 0 && tFcyRate > 0) ? fbc * tFcyRate : tAmt);
                    lines.add(d33);
                }
            }
            if (!ded.isEmpty() && (!"Multi Invoices Payment".equals(term) || (transType != 2 && transType != 3) || acctType != 1)) {
                for (Map<String, Object> r : ded) {
                    if (!charges.isEmpty()) {
                        double[] nn = chargeLines(lines, charges, r, bank, mc, ex, dacId, dacRate, dacAmt, true, true);
                        num += nn[0]; num2 += nn[1];
                    }
                }
            }
        }
        /* BLL :1282-1283 - both branches end with the running totals */
        vh.put("VoucherAmount", num);
        vh.put("BillAmount", num2);
        vh.put("__details", lines);
        return vh;
    }

    /**
     * The flag / Invoice Payment bank line(s) (BLL :428-486): third-currency receiver pair or the single bank debit
     * carrying text2. Debit totals are summed by the caller.
     */
    private static void bankAndThirdLines(List<Map<String, Object>> lines, int acctType, double ex, double tRecvRate, double tRecvAmt, double gross,
                                          int bank, int refAccount, int gainLossAc, int mc, String text, String cur, String tCode, double tAmt,
                                          double tFcyRate, int thirdId, int dacId, double dacRate, double dacAmt, double netRs, String text2,
                                          boolean unused, double[] unusedAcc, Object unusedObj) {
        if (acctType == 2 && ex > 0 && tRecvRate > 0 && tRecvAmt > 0) {
            double num20 = gross - tRecvAmt, num19 = tRecvAmt * ex;
            Map<String, Object> d9 = vd();
            d9.put("AccountId", bank); d9.put("AgainstAccountId", refAccount);
            d9.put("DebitAmount", num19); d9.put("CreditAmount", 0d);
            d9.put("Comments", tCode + "  " + cs(tAmt) + " Received From " + text + " @ " + cs(tFcyRate) + "  " + cur + "  " + cs(gross) + " @ " + cs(ex) + " Receiver Ex.Rate " + cs(tRecvRate));
            d9.put("DMultiCurrencyId", mc); d9.put("DExchangeCurrencyRate", ex); d9.put("DCurrencyAmount", tRecvAmt);
            d9.put("ThirdCurrencyId", dacId); d9.put("ThirdCurrencyFcyExchangeRate", dacRate);
            d9.put("ThirdCurrencyAmount", (dacAmt > 0 && dacRate > 0) ? tRecvAmt * dacRate : dacAmt);
            d9.put("ThirdCurrencyReceiverExchangeRate", tRecvRate); d9.put("ThirdCurrencyReceiverFcyAmount", tRecvAmt);
            lines.add(d9);
            if (num20 != 0) {
                Map<String, Object> d10 = vd();
                d10.put("AccountId", gainLossAc); d10.put("AgainstAccountId", refAccount);
                if (num20 > 0) d10.put("DebitAmount", num20 * ex); else d10.put("CreditAmount", Math.abs(num20) * ex);
                d10.put("Comments", tCode + "  " + cs(tAmt) + " Ex.Rate " + cs(tRecvRate) + "  Party Ex.Rate " + cs(tFcyRate));
                d10.put("DMultiCurrencyId", mc); d10.put("DExchangeCurrencyRate", ex); d10.put("DCurrencyAmount", num20);
                lines.add(d10);
            }
        } else {
            Map<String, Object> d11 = vd();
            d11.put("AccountId", bank); d11.put("AgainstAccountId", refAccount);
            d11.put("DebitAmount", netRs); d11.put("CreditAmount", 0d);
            d11.put("Comments", text2);
            d11.put("DMultiCurrencyId", mc); d11.put("DExchangeCurrencyRate", ex);
            double dca = netRs / ex; d11.put("DCurrencyAmount", dca);
            d11.put("ThirdCurrencyId", dacId); d11.put("ThirdCurrencyFcyExchangeRate", dacRate);
            d11.put("ThirdCurrencyAmount", (dacAmt > 0 && dacRate > 0) ? dca * dacRate : dacAmt);
            lines.add(d11);
        }
    }

    /**
     * One charges row -> debit charges GL / credit bank (BLL :343-409 and the copies). Returns {debit added, credit added}.
     * withDebit / withCredit pick which half (the Multi Invoices branch adds all credits first, then all debits).
     */
    private static double[] chargeLines(List<Map<String, Object>> lines, List<Map<String, Object>> charges, Map<String, Object> r,
                                        int bank, int mc, double ex, int dacId, double dacRate, double dacAmt, boolean withDebit, boolean withCredit) {
        Map<String, Object> ct = null;
        int ctId = asInt(r.get("ExImProceedsChargesTypeId"));
        for (Map<String, Object> c : charges) if (asInt(ci(c, "Id")) == ctId) { ct = c; break; }
        if (ct == null) throw new IllegalStateException("Charges AccountType GLAccountId not Found");
        int gl = asInt(ci(ct, "ProceedsChargesGlAccount"));
        String desc = text(ci(ct, "ProceedsChargesTypedescription"));
        String pr = str(r.get("ProceedsRemarks"));
        String comments = pr.isEmpty() ? pr + "  " + desc : pr;
        double rs = asDouble(r.get("RsAmount")), fcAmt = asDouble(r.get("FcAmount"));
        boolean local = "Local".equals(str(r.get("Type")));
        double dca = local ? rs / ex : fcAmt;
        double[] out = {0, 0};
        if (withDebit) {
            Map<String, Object> d = vd();
            d.put("AccountId", gl); d.put("AgainstAccountId", bank); d.put("Comments", comments);
            d.put("DebitAmount", rs); out[0] = rs;
            d.put("DCurrencyAmount", dca); d.put("DExchangeCurrencyRate", ex); d.put("DMultiCurrencyId", mc);
            d.put("CreditAmount", 0d);
            lines.add(d);
        }
        if (withCredit) {
            Map<String, Object> d = vd();
            d.put("AccountId", bank); d.put("AgainstAccountId", gl); d.put("Comments", comments);
            d.put("DebitAmount", 0d); d.put("CreditAmount", rs); out[1] = rs;
            d.put("DCurrencyAmount", dca); d.put("DExchangeCurrencyRate", ex); d.put("DMultiCurrencyId", mc);
            if (withDebit) {
                /* the single-pass copies set the third-currency fields on the bank credit; the Multi Invoices pass does not */
                d.put("ThirdCurrencyId", dacId); d.put("ThirdCurrencyFcyExchangeRate", dacRate);
                d.put("ThirdCurrencyAmount", (dacAmt > 0 && dacRate > 0) ? dca * dacRate : dacAmt);
            }
            lines.add(d);
        }
        return out;
    }

    /** GetPartyInvoicesAgainstFIFO - {InvoiceFcyAmount, ConversionAmount, ExchangeRate, Remarks} rows; fills the party-invoice list. */
    private List<Object[]> partyInvoicesFifo(UserAccount u, Map<String, Object> o, String cur, List<Map<String, Object>> partyInvoices) {
        List<Object[]> out = new ArrayList<>();
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", o.get("OrganizationId"));
        p.put("CompanyId", o.get("CompanyId"));
        p.put("CustomerId", o.get("SupplierCustomerId"));
        p.put("FcyId", o.get("MultiCurrencyId"));
        if (asInt(o.get("Id")) != 0) p.put("FcyBankReceiptId", o.get("Id"));
        List<Map<String, Object>> rows = repo.pendingPartyInvoices(p);
        if (rows.isEmpty()) return out;
        double total = 0;
        for (Map<String, Object> r : rows) total += asDouble(ci(r, "BalanceAmount"));
        double gross = asDouble(o.get("FcGrossAmount"));
        if (total > 0) {
            double num2 = gross, num4 = 0;
            for (Map<String, Object> r : rows) {
                double num3 = asDouble(ci(r, "BalanceAmount"));
                BigDecimal rate = dec(ci(r, "ExchangeRate"));
                String rateText = csAny(ci(r, "ExchangeRate"));
                if (num3 > 0 && num3 <= num2 - num4) {
                    num4 += num3;
                    BigDecimal realized = toDecimal(num3);
                    partyInvoices.add(partyInvoice(asInt(ci(r, "InvoiceId")), realized));
                    String t = " FcyCode " + cur + " @Rate " + rateText + " Invoice# " + text(ci(r, "InvoiceNo")) + " InvoiceAmount " + csM(realized);
                    out.add(new Object[] {realized, realized.multiply(rate), rate, t});
                } else if (num3 >= num2 - num4) {
                    BigDecimal realized = toDecimal(num2 - num4);
                    partyInvoices.add(partyInvoice(asInt(ci(r, "InvoiceId")), realized));
                    num4 += realized.doubleValue();
                    String t = " FcyCode " + cur + " @Rate " + rateText + " Invoice# " + text(ci(r, "InvoiceNo")) + " InvoiceAmount " + csM(realized);
                    out.add(new Object[] {realized, realized.multiply(rate), rate, t});
                }
                if (gross == num4) break;
            }
        }
        return out;
    }

    private static Map<String, Object> partyInvoice(int invoiceId, BigDecimal realized) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", 0);
        m.put("FcyBankReceiptId", 0);
        m.put("RefDocumentTypeId", 0);
        m.put("RefDocId", invoiceId);
        m.put("RealizedAmount", realized);
        m.put("SortNo", 0);
        return m;
    }

    /** GetFcyAndLcyBalance(ReportsParameters): FcyId, ToDate = DocumentDate, SupplierCustomerId, Id + DocumentTypeId when Id > 0. */
    private List<Map<String, Object>> fcyAndLcyBalance(UserAccount u, Map<String, Object> o) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", o.get("OrganizationId"));
        p.put("CompanyId", o.get("CompanyId"));
        p.put("FcyId", o.get("MultiCurrencyId"));
        p.put("ToDate", o.get("DocumentDate"));
        if (asInt(o.get("SupplierCustomerId")) != 0) p.put("SupplierCustomerId", o.get("SupplierCustomerId"));
        if (asInt(o.get("Id")) > 0) { p.put("Id", o.get("Id")); p.put("DocumentTypeId", o.get("DocumentTypeId")); }
        return repo.fcyAndLcyBalance(p);
    }

    private static Map<String, Object> gl(double fcy, double local, double rate, String type, String remarks) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", 0);
        m.put("FcyBankReceiptId", 0);
        m.put("OrganizationId", 0);
        m.put("CompanyId", 0);
        m.put("FcyAmount", fcy);
        m.put("LocalAmount", local);
        m.put("ExchangeRate", rate);
        m.put("Type", type);
        m.put("Remarks", remarks);
        return m;
    }

    // ================================================================= voucher models (Architecture.Model.Accounts)

    /** VoucherHead non-virtual properties as GenericProvider.SetProc sends them (InclusiveTax is bool? null -> not sent). */
    static final String[] VH = {"Id", "DocumentTypeId", "DocumentTypeSrNo", "RefDocNoId", "VoucherCode",
            "VoucherDate", "Remarks", "RemarksOtherLingo", "VoucherAmount", "FinancialYearId", "RefAccountId",
            "AgainstAccountId", "MultiCurrencyId", "ConversionFormula", "ExchangeCurrencyRate", "FcAmount", "CheqId",
            "ChequeNo", "ChequeDate", "PayTitle", "BankBranch", "ChequePrintId", "Source", "DrCrNoteType", "IsApproved",
            "EntryDate", "EntryUser", "ModifyDate", "ModifyUser", "PostDate", "PostUser", "PostState", "OrganizationId",
            "CompanyId", "IncludeWHT", "BranchId", "ProjectId", "ManualBillNo", "BillAmount", "DueDate", "DueDays",
            "ActionId", "CostCenterAmount", "AttachmentsValues", "RefDocumentTypeId", "CustomAttachmentsValues",
            "CustomAccounts", "BaseDocumentTypeId", "AdvanceTaxAccountId", "AdvanceTaxAmount", "OtherChargesAccountId",
            "OtherChargesAmount", "IsUploaded", "InclusiveTax", "FixedAssetEntryTypeId"};

    /** VoucherDetail non-virtual properties (IsTaxable is a string, null -> not sent). */
    static final String[] VD = {"Id", "VoucherHeadId", "AccountId", "AgainstAccountId", "Comments",
            "CommentsOtherLingo", "DebitAmount", "CreditAmount", "JobLotId", "RefInvoiceNo", "TaxesTotalAmount",
            "TaxesRemarks", "IsTaxable", "TaxTypeId", "TaxPrcnt", "DCheqDate", "CheqNoDetail", "DocumentTypeIdRef",
            "InvoiceNoRefId", "ItemId", "OrderNo", "GpNo", "VehicleNo", "GpDate", "QtyIn", "QtyOut", "WeightIn",
            "WeightOut", "SupplierCustomerId", "ItemRate", "RateCut", "RateCutAmount", "ItemAmount", "Expenses", "Freight",
            "Journal", "Commission", "DMultiCurrencyId", "DConversionFormula", "DExchangeCurrencyRate", "DCurrencyAmount",
            "PaymentType", "AdvanceAmount", "WhtHolding", "SaleTax", "ExTax", "Adjustment", "ActionId", "LineId",
            "RefDocumentTypeId", "RefDocNoId", "RefDocNoDetailId", "RefDocSubIdNo", "ItemCgsRate", "TotalCreditAmount",
            "TotalDebitAmount", "SubNo", "PayeeTitle", "SubsidiaryTypeId", "EmployeeId", "SubsidiaryAccountId", "IsCGS",
            "SubsidiaryAgainstTypeId", "SubsidiaryAgainstAccountId", "ThirdCurrencyId", "ThirdCurrencyFcyExchangeRate",
            "ThirdCurrencyHcyExchangeRate", "ThirdCurrencyAmount", "ThirdCurrencyReceiverExchangeRate",
            "ThirdCurrencyReceiverFcyAmount", "SortNo", "InstrumentTypeId", "ChequeTypeId", "BranchesId", "CostCenterId",
            "SBRTaxAmount", "DiscountPercent", "DiscountAmount", "ReferenceAccountId", "LocationTypeId", "PaymentTypeId",
            "TaxAmount", "BaseFcyId", "BaseFcyExchangeRate", "BaseFcyAmount"};

    private static Map<String, Object> voucherHeadDefaults() {
        Map<String, Object> v = new LinkedHashMap<>();
        for (String k : new String[] {"IncludeWHT", "IsApproved", "PostState", "IsUploaded", "CustomAccounts"}) v.put(k, false);
        for (String k : new String[] {"BillAmount", "ExchangeCurrencyRate", "FcAmount", "VoucherAmount", "CostCenterAmount",
                "AdvanceTaxAmount", "OtherChargesAmount"}) v.put(k, 0d);
        for (String k : new String[] {"AgainstAccountId", "BranchId", "CheqId", "ChequePrintId", "CompanyId", "DocumentTypeId",
                "DocumentTypeSrNo", "DueDays", "EntryUser", "FinancialYearId", "Id", "ModifyUser", "MultiCurrencyId",
                "OrganizationId", "PostUser", "ProjectId", "RefAccountId", "RefDocNoId", "VoucherCode", "ActionId",
                "RefDocumentTypeId", "FixedAssetEntryTypeId", "BaseDocumentTypeId", "AdvanceTaxAccountId",
                "OtherChargesAccountId"}) v.put(k, 0);
        return v;
    }

    private static Map<String, Object> vd() {
        Map<String, Object> v = new LinkedHashMap<>();
        for (String k : new String[] {"Adjustment", "AdvanceAmount", "Commission", "CreditAmount", "DCurrencyAmount",
                "DebitAmount", "TaxAmount", "DExchangeCurrencyRate", "Expenses", "ExTax", "Freight", "ItemAmount", "ItemRate",
                "Journal", "QtyIn", "QtyOut", "RateCut", "RateCutAmount", "SaleTax", "TaxesTotalAmount", "TaxPrcnt",
                "WeightIn", "WeightOut", "WhtHolding", "ItemCgsRate", "TotalDebitAmount", "TotalCreditAmount",
                "ThirdCurrencyFcyExchangeRate", "ThirdCurrencyHcyExchangeRate", "ThirdCurrencyAmount",
                "ThirdCurrencyReceiverExchangeRate", "ThirdCurrencyReceiverFcyAmount", "SBRTaxAmount", "DiscountPercent",
                "DiscountAmount", "BaseFcyExchangeRate", "BaseFcyAmount"}) v.put(k, 0d);
        for (String k : new String[] {"ThirdCurrencyId", "AccountId", "AgainstAccountId", "DMultiCurrencyId", "DocumentTypeIdRef",
                "GpNo", "Id", "InvoiceNoRefId", "ItemId", "JobLotId", "OrderNo", "SupplierCustomerId", "EmployeeId",
                "SubsidiaryTypeId", "SubsidiaryAccountId", "SubsidiaryAgainstTypeId", "SubsidiaryAgainstAccountId",
                "TaxTypeId", "ActionId", "VoucherHeadId", "RefDocumentTypeId", "RefDocNoId", "RefDocNoDetailId",
                "RefDocSubIdNo", "LineId", "InstrumentTypeId", "SubNo", "SortNo", "IsCGS", "PaymentTypeId", "ChequeTypeId",
                "BranchesId", "CostCenterId", "ReferenceAccountId", "LocationTypeId", "BaseFcyId"}) v.put(k, 0);
        return v;
    }

    // ================================================================= helpers

    private static Map<String, Object> idName(int id, String name) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", id);
        m.put("name", name);
        return m;
    }

    /** .NET Framework double.ToString() - "G" with 15 significant digits. */
    static String cs(double d) {
        if (Double.isNaN(d)) return "NaN";
        if (Double.isInfinite(d)) return d > 0 ? "Infinity" : "-Infinity";
        if (d == 0) return "0";
        BigDecimal bd = new BigDecimal(d).round(new MathContext(15)).stripTrailingZeros();
        double abs = Math.abs(d);
        if (abs >= 1e15 || abs < 1e-5) {
            String s = String.format(Locale.ROOT, "%.14E", bd.doubleValue());
            String[] parts = s.split("E");
            String mant = parts[0].contains(".") ? parts[0].replaceAll("0+$", "").replaceAll("\\.$", "") : parts[0];
            int exp = Integer.parseInt(parts[1]);
            return mant + "E" + (exp < 0 ? "-" : "+") + String.format(Locale.ROOT, "%02d", Math.abs(exp));
        }
        return bd.toPlainString();
    }

    /** decimal.ToString(). */
    static String csM(BigDecimal m) { return m == null ? "0" : m.stripTrailingZeros().toPlainString(); }

    private static String csAny(Object v) {
        if (v == null) return "";
        if (v instanceof BigDecimal) return ((BigDecimal) v).toPlainString();
        if (v instanceof Double || v instanceof Float) return cs(((Number) v).doubleValue());
        return String.valueOf(v);
    }

    /** Conversion.ToDecimal(double) - System.Convert.ToDecimal keeps 15 significant digits. */
    static BigDecimal toDecimal(double d) { return d == 0 ? BigDecimal.ZERO : new BigDecimal(d).round(new MathContext(15)).stripTrailingZeros(); }

    static BigDecimal dec(Object v) {
        if (v == null) return BigDecimal.ZERO;
        if (v instanceof BigDecimal) return (BigDecimal) v;
        if (v instanceof Number) return toDecimal(((Number) v).doubleValue());
        String s = String.valueOf(v).trim().replace(",", "");
        if (s.isEmpty()) return BigDecimal.ZERO;
        try { return new BigDecimal(s); } catch (NumberFormatException e) { return BigDecimal.ZERO; }
    }

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> list(Object v) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (v instanceof List) for (Object x : (List<Object>) v) if (x instanceof Map) out.add((Map<String, Object>) x);
        return out;
    }

    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }
    private static String text(Object v) { return v == null ? "" : String.valueOf(v).trim(); }

    static int asInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        if (v instanceof Boolean) return (Boolean) v ? 1 : 0;
        String s = String.valueOf(v).trim().replace(",", "");
        if (s.isEmpty()) return 0;
        try { return Integer.parseInt(s); }
        catch (NumberFormatException e) { try { return (int) Double.parseDouble(s); } catch (NumberFormatException e2) { return 0; } }
    }

    static double asDouble(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).doubleValue();
        try { return Double.parseDouble(String.valueOf(v).trim().replace(",", "")); } catch (NumberFormatException e) { return 0; }
    }

    static boolean asBool(Object v) {
        if (v == null) return false;
        if (v instanceof Boolean) return (Boolean) v;
        String s = String.valueOf(v).trim().toLowerCase(Locale.ROOT);
        return "true".equals(s) || "1".equals(s) || "on".equals(s) || "yes".equals(s);
    }

    static LocalDate asDate(Object v) {
        if (v == null) return null;
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        if (s.length() > 10) s = s.substring(0, 10);
        try { return LocalDate.parse(s); } catch (DateTimeParseException e) { return null; }
    }

    static String iso(Object v) {
        if (v == null) return "";
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().toLocalDate().toString();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().toLocalDate().toString();
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).toLocalDate().toString();
        String s = String.valueOf(v).trim();
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }

    static String isoDateTime(Object v) {
        if (v == null) return "";
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().withNano(0).toString();
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().withNano(0).toString();
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).withNano(0).toString();
        return String.valueOf(v).trim();
    }
}
