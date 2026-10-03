package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.saleinvoice.SaleInvoiceModels;
import com.mst.repositories.ExportReturnRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.ExportMSupport.*;

/**
 * BLL side of 191 ExportReturnInvoice "Export Return Invoice" (Architecture.WinApp.Export.ExportReturnInvoice,
 * ScreenName "ExportReturnInvoice", DocumentTypeId 242) and its popup frmLoadCommercialInvoiceForReturn.
 *
 * Rights from ScreenDefinition.Id 191 (CompanyRights + ScreenRights + tblUserRights; RoleName "Admin" turns
 * Save/Update/Print/Delete/CanViewAll on, as SetRightsValueInRightsObject does). Organisation, company, branch,
 * user and financial year come from the session only. The form's validations are repeated here with the
 * desktop's texts so a procedure is never reached with what the form would have refused.
 *
 * Configuration (GlobalVariables_Helper.GetConfigValueFromGlobal): FinancialisActiveonCommercialInvoice,
 * EnableExportReturnReverseFlow (true: "Load Invoices" popup, no pending-GRN grid, Qty editable + X column),
 * DefaultDaysToLessFromHistoryFromDate.
 */
@Service
public class ExportReturnInvoiceService {

    public static final int SCREEN_ID = 191;
    public static final int DOCUMENT_TYPE_ID = 242;

    @Autowired private ExportReturnRepository repo;
    @Autowired private CurrentUserContext ctx;
    @Autowired private DesktopReportRights rights;

    private UserAccount user(String action) {
        UserAccount u = ctx.requireAccountingUser();
        rights.require(u, SCREEN_ID, action);
        return u;
    }
    private int fy() { return ctx.currentFinancialYearId(); }

    // ================================================================== load

    /** InitializeComponentMethod + InitializeComponentCustom. */
    public Map<String, Object> setup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", right(ctx, rights, u, SCREEN_ID, "Save"));
        perm.put("Update", right(ctx, rights, u, SCREEN_ID, "Update"));
        perm.put("Print", right(ctx, rights, u, SCREEN_ID, "Print"));
        perm.put("Delete", right(ctx, rights, u, SCREEN_ID, "Delete"));
        out.put("permissions", perm);
        Map<String, Object> cfg = config(u);
        out.put("config", cfg);
        out.put("docNo", repo.riGenerateCode(u, fy(), DOCUMENT_TYPE_ID));
        out.putAll(globals(u));
        if (!asBool(cfg.get("enableExportReturnReverseFlow"))) out.put("pending", pendingRows(u));
        out.put("history", historyCombos(u));
        out.put("documentTypeId", DOCUMENT_TYPE_ID);
        return out;
    }

    private Map<String, Object> config(UserAccount u) {
        Map<String, Object> c = new LinkedHashMap<>();
        c.put("financialisActiveonCommercialInvoice", asBool(repo.config(u, "FinancialisActiveonCommercialInvoice")));
        c.put("enableExportReturnReverseFlow", asBool(repo.config(u, "EnableExportReturnReverseFlow")));
        c.put("defaultDaysToLessFromHistoryFromDate", asInt(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        c.put("decAmount", digits(repo.config(u, "Default NoofDecimal Points For Amount")));
        c.put("decFcy", digits(repo.config(u, "DefaultNoOfDecimalPointsForFcyAmount")));
        return c;
    }

    /** SupplierDtFillFromGlobal, CurrencyBindFromGlobal, CreditGlAccountBindFromGlobal, DetailComboDtFillFromGlobal. */
    private Map<String, Object> globals(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        /* CustomerGroupId == 7 && !IsSubSupCust; the 4th value written is GlAccountId into the CityName column (desktop quirk). */
        List<Map<String, Object>> customers = new ArrayList<>();
        for (Map<String, Object> r : repo.globalSupplierCustomers(u)) {
            if (asInt(ci(r, "CustomerGroupId")) != 7 || asBool(ci(r, "IsSubSupCust"))) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("CompanyName", text(ci(r, "CompanyName")));
            m.put("PartyCode", text(ci(r, "PartyCode")));
            m.put("CityName", String.valueOf(asInt(ci(r, "GlAccountId"))));
            m.put("MobileNo", "");
            customers.add(m);
        }
        out.put("customers", customers);
        List<Map<String, Object>> currencies = new ArrayList<>();
        for (Map<String, Object> r : repo.currencies(u)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("CurrencyCode", text(ci(r, "CurrencyCode")));
            currencies.add(m);
        }
        out.put("currencies", currencies);
        out.put("creditAccounts", accountsByType(repo.accountsWithCustomGroup(u), new int[]{3, 6, 8, 10}));
        List<Map<String, Object>> wh = new ArrayList<>();
        for (Map<String, Object> r : repo.warehouses(u)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("WareHouseName", text(ci(r, "WarehouseName")));
            wh.add(m);
        }
        out.put("warehouses", wh);
        return out;
    }

    /** DatatableHelper.GetAccountsFromGlobalByTypeIds(withTypeIds) - distinct ChartOfAccountId, first occurrence. */
    static List<Map<String, Object>> accountsByType(List<Map<String, Object>> all, int[] typeIds) {
        Set<Integer> types = new HashSet<>();
        for (int t : typeIds) types.add(t);
        Set<Integer> seen = new HashSet<>();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : all) {
            if (!types.contains(asInt(ci(r, "AccountTypeId")))) continue;
            int id = asInt(ci(r, "ChartOfAccountId"));
            if (!seen.add(id)) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", id);
            m.put("AccountTitle", text(ci(r, "AccountTitle")));
            m.put("AccountCode", text(ci(r, "AccountCode")));
            m.put("ParentAccountTitle", text(ci(r, "ParentAccountTitle")));
            m.put("AccountClass", text(ci(r, "AccountClassName")));
            out.add(m);
        }
        return out;
    }

    /** btnRefresh_Click: configuration and the global combos again (pending GRN grid re-bound from the cached rows on the desktop). */
    public Map<String, Object> refresh() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("config", config(u));
        out.putAll(globals(u));
        return out;
    }

    /** Reset(): GetDocumentCode + PendingGrnDbCall when the reverse flow is off. */
    public Map<String, Object> reset() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("docNo", repo.riGenerateCode(u, fy(), DOCUMENT_TYPE_ID));
        if (!asBool(repo.config(u, "EnableExportReturnReverseFlow"))) out.put("pending", pendingRows(u));
        return out;
    }

    /** PendingGrnDbCall - the raw procedure rows (the page projects grnDataTable and reads the rest on Load). */
    private List<Map<String, Object>> pendingRows(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.riPendingGrn(u, fy())) out.add(plain(r));
        return out;
    }

    /** ExImInvoice.InvoicePaymentTermsDetailByInvoiceId - for BindPaymentDetailByInvoiceId. */
    public List<Map<String, Object>> invoicePaymentTerms(int invoiceId) {
        user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.invoicePaymentTerms(invoiceId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("ExImInvoiceId", asInt(ci(r, "ExImInvoiceId")));
            m.put("PaymentTermId", asInt(ci(r, "PaymentTermId")));
            m.put("PaymentTerm", text(ci(r, "PaymentTerm")));
            m.put("PrcntOfTotal", asDouble(ci(r, "PrcntOfTotal")));
            m.put("FcyAmount", asDouble(ci(r, "FcyAmount")));
            m.put("DueDays", text(ci(r, "DueDays")));
            m.put("PaymentRemarks", text(ci(r, "PaymentRemarks")));
            out.add(m);
        }
        return out;
    }

    // ================================================================== history

    /** GetDataForDropDownFromExportReturnInvoice split by Activity: Customer / DebitAccount / InvoiceNo. */
    public Map<String, Object> historyCombos() { return historyCombos(user("View")); }

    private Map<String, Object> historyCombos(UserAccount u) {
        List<Map<String, Object>> cust = new ArrayList<>(), debit = new ArrayList<>(), inv = new ArrayList<>(), all = new ArrayList<>();
        for (Map<String, Object> r : repo.riDropDown(u)) {
            String a = text(ci(r, "Activity"));
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("Name", text(ci(r, "ReferenceName")));
            m.put("Activity", a);
            all.add(m);
            if ("Customer".equals(a)) cust.add(m);
            else if ("DebitAccount".equals(a)) debit.add(m);
            else if ("InvoiceNo".equals(a)) inv.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("customers", cust);
        out.put("debitAccounts", debit);
        out.put("invoices", inv);
        out.put("all", all);
        return out;
    }

    /** Gridhistoryfill: dtHistoryGrid's columns in their order. */
    public List<Map<String, Object>> history(Map<String, Object> b) {
        UserAccount u = user("View");
        boolean canViewAll = isAdmin(ctx) || repo.canViewAllRecordRight(u, SCREEN_ID);
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("BranchesId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        p.put("FinancialYearId", fy());
        p.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        p.put("CanViewAllRecord", canViewAll);
        /* BLL quirk: the parameter is named @EntryUser (the procedure declares @EntryUserId) and ReportsParameters.EntryUser
           is never set - SQL Server refuses the call for a user without "CanView AllRecord", exactly as on the desktop. */
        if (!canViewAll) p.put("EntryUser", 0);
        java.sql.Date from = asBool(b.get("fromChecked")) ? sqlDate(b.get("fromDate")) : null;
        java.sql.Date to = asBool(b.get("toChecked")) ? sqlDate(b.get("toDate")) : null;
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        int fromNo = asInt(b.get("fromDocNo")), toNo = asInt(b.get("toDocNo"));
        if (fromNo != 0) p.put("DocNoFrom", fromNo);
        if (toNo != 0) p.put("DocNoTo", toNo);
        int cust = asInt(b.get("supplierCustomerId")), inv = asInt(b.get("exImInvoiceId")), debit = asInt(b.get("debitAccountId"));
        if (cust != 0) p.put("SupplierCustomerId", cust);
        if (inv != 0) p.put("ExImInvoiceId", inv);
        if (debit != 0) p.put("DebitAccountId", debit);
        p.put("Activity", "FormHistory");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.riHistory(p)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("DocNo", text(ci(r, "DocNo")));
            m.put("DocDate", iso(ci(r, "DocDate")));
            m.put("DocumentTypeId", asInt(ci(r, "DocumentTypeId")));
            m.put("SupplierCustomerId", asInt(ci(r, "SupplierCustomerId")));
            m.put("CustomerName", text(ci(r, "CustomerName")));
            m.put("DebitAccountId", asInt(ci(r, "DebitAccountId")));
            m.put("DebitAccountTitle", text(ci(r, "DebitAccountTitle")));
            m.put("CurrencyId", asInt(ci(r, "CurrencyId")));
            m.put("CurrencyName", text(ci(r, "CurrencyName")));
            m.put("FcyAmountWithOutAddLess", asDouble(ci(r, "FcyAmountWithOutAddLess")));
            m.put("AddLessAmount", asDouble(ci(r, "AddLessAmount")));
            m.put("FcyAmount", asDouble(ci(r, "FcyAmount")));
            m.put("ExchangeRate", asDouble(ci(r, "ExchangeRate")));
            m.put("LcyAmount", asDouble(ci(r, "LcyAmount")));
            m.put("NetWeight", asDouble(ci(r, "NetWeight")));
            m.put("RemarksHeader", text(ci(r, "RemarksHeader")));
            m.put("EntryDate", isoDateTime(ci(r, "EntryDate")));
            m.put("EntryUser", text(ci(r, "EntryUser")));
            m.put("ModifyUser", text(ci(r, "ModifyUser")));
            m.put("ModifyDate", isoDateTime(ci(r, "ModifyDate")));
            m.put("ApprovedUser", text(ci(r, "ApprovedUser")));
            m.put("ApprovedDate", isoDateTime(ci(r, "ApprovedDate")));
            m.put("IsApproved", asBool(ci(r, "IsApproved")));
            m.put("NoOfAttachments", asInt(ci(r, "NoOfAttachments")));
            out.add(m);
        }
        return out;
    }

    // ================================================================== read

    /**
     * ReadById (Edit / double-click; the desktop does nothing at all without DoHaveUpdateRights) and
     * GetDetailGrdByHeadId (history selection). The BLL indexes [0] of the result: no row is the ADO
     * "Index was out of range" error the desktop shows.
     */
    public Map<String, Object> byId(int id, boolean forEdit) {
        UserAccount u = user("View");
        if (forEdit && !right(ctx, rights, u, SCREEN_ID, "Update")) {
            Map<String, Object> none = new LinkedHashMap<>();
            none.put("noRight", true);
            return none;
        }
        List<Map<String, Object>> hs = repo.riHeader(id);
        if (hs.isEmpty()) throw new IllegalStateException("Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index");
        Map<String, Object> h = hs.get(0);
        Map<String, Object> head = new LinkedHashMap<>();
        head.put("Id", asInt(ci(h, "Id")));
        head.put("DocNo", text(ci(h, "DocNo")));
        head.put("DocDate", iso(ci(h, "DocDate")));
        head.put("ExImInvoiceId", asInt(ci(h, "ExImInvoiceId")));
        head.put("InvoiceNo", text(ci(h, "InvoiceNo")));
        head.put("InvoiceDate", iso(ci(h, "InvoiceDate")));
        head.put("FcyAmountWithOutAddLess", asDouble(ci(h, "FcyAmountWithOutAddLess")));
        head.put("AddLessAmount", asDouble(ci(h, "AddLessAmount")));
        head.put("FcyAmount", asDouble(ci(h, "FcyAmount")));
        head.put("CurrencyId", asInt(ci(h, "CurrencyId")));
        head.put("ExchangeRate", asDouble(ci(h, "ExchangeRate")));
        head.put("LcyAmount", asDouble(ci(h, "LcyAmount")));
        head.put("RemarksHeader", text(ci(h, "RemarksHeader")));
        head.put("NetWeight", asDouble(ci(h, "NetWeight")));
        head.put("SupplierCustomerId", asInt(ci(h, "SupplierCustomerId")));
        head.put("DebitAccounId", asInt(ci(h, "DebitAccounId")));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", head);
        out.put("detail", detailRows(repo.riDetails(id)));
        out.put("payments", paymentRows(repo.riPayments(id)));
        return out;
    }

    /** FillDetailFromListCommonForReadById - dtDetail's columns in order. */
    private static List<Map<String, Object>> detailRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(d, "Id")));
            m.put("ExImInvoiceId", asInt(ci(d, "ExImInvoiceId")));
            m.put("ExImInvoiceDetailId", asInt(ci(d, "ExImInvoiceDetailId")));
            m.put("GrnId", asInt(ci(d, "GrnId")));
            m.put("GrnDetailId", asInt(ci(d, "GrnDetailId")));
            m.put("GrnNo", asInt(ci(d, "GrnNo")));
            m.put("WarehouseId", asInt(ci(d, "WarehouseId")));
            m.put("Warehouse", text(ci(d, "WareHouseName")));
            m.put("ItemId", asInt(ci(d, "ItemId")));
            m.put("ItemName", text(ci(d, "ItemName")));
            m.put("ItemCode", text(ci(d, "ItemCode")));
            m.put("CropYearId", asInt(ci(d, "CropYearId")));
            m.put("CropYear", text(ci(d, "CropYear")));
            m.put("JobLotId", asInt(ci(d, "JobLotId")));
            m.put("JobLot", text(ci(d, "JobLot")));
            m.put("PackingTypeId", asInt(ci(d, "PackingTypeId")));
            m.put("PackingType", text(ci(d, "PackingType")));
            m.put("PackUomId", asInt(ci(d, "PackUomId")));
            m.put("PackUomCode", text(ci(d, "PackUomCode")));
            m.put("PackUomEquivalent", asDouble(ci(d, "PackUomEquivalent")));
            m.put("Qty", asDouble(ci(d, "Qty")));
            m.put("NetWeight", asDouble(ci(d, "NetWeight")));
            m.put("StockWeight", asDouble(ci(d, "StockWeight")));
            m.put("Mton", asDouble(ci(d, "Mton")));
            m.put("RatePrice", asDouble(ci(d, "RatePrice")));
            m.put("RateUomId", asInt(ci(d, "RateUomId")));
            m.put("RateUomCode", text(ci(d, "RateUomCode")));
            m.put("RateUomEquivalent", asDouble(ci(d, "RateUomEquivalent")));
            m.put("FcyAmountWithOutAddLess", asDouble(ci(d, "FcyAmountWithOutAddLess")));
            m.put("AddLessAmount", asDouble(ci(d, "AddLessAmount")));
            m.put("FcyAmount", asDouble(ci(d, "FcyAmount")));
            m.put("LcyAmount", asDouble(ci(d, "LcyAmount")));
            m.put("RemarksDetail", text(ci(d, "RemarksDetail")));
            out.add(m);
        }
        return out;
    }

    /** FillPaymentDetailFromListCommonForReadById - dtPaymentTerm's columns. */
    private static List<Map<String, Object>> paymentRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(d, "Id")));
            m.put("InvoiceId", asInt(ci(d, "ExImInvoiceId")));
            m.put("InvoiceDetailId", asInt(ci(d, "ExImInvoicePaymentDetailId")));
            m.put("PaymentTermId", asInt(ci(d, "PaymentTermId")));
            m.put("PaymentTerm", text(ci(d, "PaymentTerm")));
            m.put("PrcntOfTotal", asDouble(ci(d, "PrcntOfTotal")));
            m.put("FcyAmount", asDouble(ci(d, "FcyAmount")));
            m.put("LcyAmount", asDouble(ci(d, "LcyAmount")));
            m.put("DueDays", text(ci(d, "DueDays")));
            m.put("AdjustmentAmount", asDouble(ci(d, "AdjustmentAmount")));
            m.put("Remarks", text(ci(d, "PaymentRemarks")));
            out.add(m);
        }
        return out;
    }

    /** ExportReturnVoucherSlip: CommonServices.VoucherHeadIdGet(PrintId, 242) for the 102 slip. */
    public Map<String, Object> voucherHead(int id) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("voucherHeadId", repo.voucherHeadId(u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, id));
        return out;
    }

    // ================================================================== delete

    /** btnDelete_Click. */
    public Map<String, Object> delete(int recId) {
        UserAccount u = ctx.requireAccountingUser();
        rights.require(u, SCREEN_ID, "View");
        if (!right(ctx, rights, u, SCREEN_ID, "Delete")) throw new org.springframework.security.access.AccessDeniedException("The user does not have Delete rights for this screen");
        if (recId == 0) throw new IllegalArgumentException("Record Not Delete because RecId No Found");
        repo.riDelete(u, DOCUMENT_TYPE_ID, recId);
        return ok("Deleted Successfully");
    }

    // ================================================================== save

    /**
     * Insert(): FormValidation, the per-row FormHelper.ValidateField checks, the payment-grid total check, then
     * BLL ExportReturnInvoice.Save (MakeVoucher, ActionId 1/2) -> DAL SetData.
     */
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = ctx.requireAccountingUser();
        rights.require(u, SCREEN_ID, "View");
        int recId = asInt(b.get("recId"));
        if (!right(ctx, rights, u, SCREEN_ID, recId > 0 ? "Update" : "Save"))
            throw new org.springframework.security.access.AccessDeniedException("The user does not have " + (recId > 0 ? "Update" : "Save") + " rights for this screen");
        boolean reverse = asBool(repo.config(u, "EnableExportReturnReverseFlow"));
        Map<String, Object> h = map(b.get("header"));
        List<Map<String, Object>> rows = list(b.get("rows"));
        List<Map<String, Object>> removed = list(b.get("removed"));
        List<Map<String, Object>> pays = list(b.get("payments"));

        if (rows.isEmpty()) throw new IllegalArgumentException("Please Check Detail Grid");
        /* FormValidation */
        if (trim(h.get("InvoiceNo")).isEmpty()) throw new IllegalArgumentException("Invoice No Field is Required");
        if (asInt(h.get("CurrencyId")) == 0) throw new IllegalArgumentException("Currency Code Field is Required");
        if (dec(h.get("ExchangeRate")).signum() == 0) throw new IllegalArgumentException("ExchangeRate Field is Required");
        if (dec(h.get("FcyAmount")).signum() == 0) throw new IllegalArgumentException("FcyAmount Field is Required");
        if (dec(h.get("LcyAmount")).signum() == 0) throw new IllegalArgumentException("RsAmount Field is Required");
        if (asInt(h.get("SupplierCustomerId")) == 0) throw new IllegalArgumentException("Customer Field is Required");

        int fy = fy();
        Timestamp now = now();
        int currencyId = asInt(h.get("CurrencyId"));
        BigDecimal exchangeRate = dec(h.get("ExchangeRate"));
        Map<String, Object> head = new LinkedHashMap<>();
        head.put("IsApproved", false);
        head.put("ApprovedDate", now);
        head.put("DocDate", pickerValue(h.get("DocDate")));
        head.put("EntryDate", now);
        head.put("ModifyDate", now);
        head.put("ExchangeRate", exchangeRate);
        head.put("FcyAmount", dec(h.get("FcyAmount")));
        head.put("LcyAmount", dec(h.get("LcyAmount")));
        head.put("AddLessAmount", dec(h.get("AddLessAmount")));
        head.put("FcyAmountWithOutAddLess", dec(h.get("FcyAmountWithOutAddLess")));
        head.put("NetWeight", dec(h.get("NetWeight")));
        head.put("ActionId", recId == 0 ? 1 : 2);
        head.put("ApprovedUserId", u.getId());
        head.put("BranchesId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        head.put("CompanyId", u.getCompanyId());
        head.put("ContractId", 0);
        head.put("CurrencyId", currencyId);
        head.put("DebitAccounId", asInt(h.get("DebitAccounId")));
        head.put("DocNo", asInt(h.get("DocNo")));
        head.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        head.put("EntryUserId", u.getId());
        head.put("ExImInvoiceId", asInt(h.get("ExImInvoiceId")));
        head.put("FinancialYearId", fy);
        head.put("Id", recId);
        head.put("ModifyUserId", u.getId());
        head.put("OrganizationId", u.getOrganizationId());
        head.put("PendingForView", 0);
        head.put("ProjectsId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        head.put("SupplierCustomerId", asInt(h.get("SupplierCustomerId")));
        head.put("RemarksHeader", text(h.get("RemarksHeader")));
        String invoiceNo = text(h.get("InvoiceNo"));
        String currencyName = trim(h.get("CurrencyName"));

        List<Map<String, Object>> details = new ArrayList<>();
        List<Integer> grnIdsOfRows = new ArrayList<>();
        List<String> itemNames = new ArrayList<>();
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> r = rows.get(i);
            int rowId = recId != 0 ? asInt(r.get("Id")) : 0;
            Map<String, Object> vd = detailModel(r, rowId <= 0 ? 1 : 2, rowId, currencyId, exchangeRate);
            validateField(vd.get("ExImInvoiceDetailId"), "Invoice Detail ID", i, "Detail Grid");
            validateField(vd.get("ExImInvoiceId"), "Invoice ID", i, "Detail Grid");
            if (!reverse) {
                validateField(vd.get("GrnId"), "GRN ID", i, "Detail Grid");
                validateField(vd.get("GrnDetailId"), "GRN Detail ID", i, "Detail Grid");
            }
            validateField(vd.get("WarehouseId"), "Warehouse", i, "Detail Grid");
            validateField(vd.get("ItemId"), "Item Name", i, "Detail Grid");
            validateField(vd.get("CropYearId"), "Crop Year", i, "Detail Grid");
            validateField(vd.get("JobLotId"), "Job Lot", i, "Detail Grid");
            validateField(vd.get("PackingTypeId"), "Packing Type", i, "Detail Grid");
            validateField(vd.get("PackUomId"), "Pack UOM", i, "Detail Grid");
            validateField(vd.get("Qty"), "Qty", i, "Detail Grid");
            validateField(vd.get("NetWeight"), "Net Weight", i, "Detail Grid");
            validateField(vd.get("Mton"), "M.Ton", i, "Detail Grid");
            validateField(vd.get("StockWeight"), "Stock Weight", i, "Detail Grid");
            validateField(vd.get("RatePrice"), "Rate Price", i, "Detail Grid");
            validateField(vd.get("RateUomId"), "Rate UOM", i, "Detail Grid");
            validateField(vd.get("FcyAmount"), "FCY Amount", i, "Detail Grid");
            details.add(vd);
        }
        if (recId > 0 && !removed.isEmpty()) {
            for (Map<String, Object> r : removed) details.add(detailModel(r, 3, asInt(r.get("Id")), currencyId, exchangeRate));
        }

        List<Map<String, Object>> payments = new ArrayList<>();
        if (!pays.isEmpty()) {
            double totalAdj = 0;
            for (Map<String, Object> p : pays) totalAdj += asDouble(p.get("AdjustmentAmount"));
            double adj = roundEven(totalAdj);
            double inv = roundEven(dec(h.get("FcyAmount")).doubleValue());
            if (adj != inv) throw new IllegalArgumentException("Total InvoiceAmount " + num(inv) + " and Total Payment UtilizeAmount " + num(adj) + " Not equal Please Check");
            for (int i = 0; i < pays.size(); i++) {
                Map<String, Object> r = pays.get(i);
                Map<String, Object> p = new LinkedHashMap<>();
                p.put("FcyAmount", asDouble(r.get("FcyAmount")));
                p.put("LcyAmount", asDouble(r.get("LcyAmount")));
                p.put("AdjustmentAmount", asDouble(r.get("AdjustmentAmount")));
                p.put("PrcntOfTotal", asDouble(r.get("PrcntOfTotal")));
                p.put("ExportReturnInvoiceId", 0);
                p.put("ExImInvoicePaymentDetailId", asInt(r.get("InvoiceDetailId")));
                p.put("ExImInvoiceId", asInt(r.get("InvoiceId")));
                p.put("Id", asInt(r.get("Id")));
                p.put("SortNo", i + 1);
                p.put("PaymentTermId", asInt(r.get("PaymentTermId")));
                p.put("PaymentRemarks", text(r.get("Remarks")));
                p.put("DueDays", asInt(r.get("DueDays")));
                p.put("EntryUserId", u.getId());
                p.put("ModifyUserId", u.getId());
                validateField(p.get("PaymentTermId"), "PaymentTerm", i, "PaymentTerm Detail");
                validateField(p.get("FcyAmount"), "FcyAmount", i, "PaymentTerm Detail");
                payments.add(p);
            }
        }

        /* BLL Save: MakeVoucher first (EntryUserId / ModifyUserId still both the user), then ActionId and the user reset. */
        SaleInvoiceModels.VoucherHead vh = new SaleInvoiceModels.VoucherHead();
        List<SaleInvoiceModels.VoucherDetail> vds = makeVoucher(u, head, invoiceNo, currencyName, details, vh);
        if (recId == 0) head.put("ModifyUserId", 0); else head.put("EntryUserId", 0);

        int id = repo.riSave(head, details, payments, vh, vds);
        Map<String, Object> out = ok(recId > 0 ? "Record Update SuccessFully" : "Record Save SuccessFully");
        out.put("id", id);
        return out;
    }

    /** FillDetailListCommonForInsertAndDelete - ExportReturnInvoiceDetail non-virtual properties. */
    private static Map<String, Object> detailModel(Map<String, Object> r, int actionTypeId, int id, int currencyId, BigDecimal exchangeRate) {
        Map<String, Object> vd = new LinkedHashMap<>();
        vd.put("IsApproved", false);
        vd.put("ExchangeRate", exchangeRate);
        vd.put("FcyAmount", dec(r.get("FcyAmount")));
        vd.put("LcyAmount", dec(r.get("LcyAmount")));
        vd.put("AddLessAmount", dec(r.get("AddLessAmount")));
        vd.put("FcyAmountWithOutAddLess", dec(r.get("FcyAmountWithOutAddLess")));
        vd.put("Mton", dec(r.get("Mton")));
        vd.put("NetWeight", dec(r.get("NetWeight")));
        vd.put("Qty", dec(r.get("Qty")));
        vd.put("RatePrice", dec(r.get("RatePrice")));
        vd.put("StockWeight", dec(r.get("StockWeight")));
        vd.put("CostRate", BigDecimal.ZERO);
        vd.put("CostAmount", BigDecimal.ZERO);
        vd.put("ActionTypeId", actionTypeId);
        vd.put("CurrencyId", currencyId);
        vd.put("Id", id);
        vd.put("LineId", 0);
        vd.put("ExportReturnInvoiceId", 0);
        vd.put("ExImInvoiceDetailId", asInt(r.get("ExImInvoiceDetailId")));
        vd.put("ExImInvoiceId", asInt(r.get("ExImInvoiceId")));
        vd.put("GrnId", asInt(r.get("GrnId")));
        vd.put("GrnDetailId", asInt(r.get("GrnDetailId")));
        vd.put("WarehouseId", asInt(r.get("WarehouseId")));
        vd.put("ItemId", asInt(r.get("ItemId")));
        vd.put("CropYearId", asInt(r.get("CropYearId")));
        vd.put("JobLotId", asInt(r.get("JobLotId")));
        vd.put("PackingTypeId", asInt(r.get("PackingTypeId")));
        vd.put("PackUomId", asInt(r.get("PackUomId")));
        vd.put("RateUomId", asInt(r.get("RateUomId")));
        vd.put("RemarksDetail", trim(r.get("RemarksDetail")));
        return vd;
    }

    /**
     * BLL ExportReturnInvoice.MakeVoucher (0451:20). One sale-return pair per detail row that is not removed
     * (ActionTypeId 3), plus the COGS pair: rows without a GRN use ExportReturnStockInTransitAccount and a cost rate
     * only when SaleCostingJobOrderWise is on; rows with a GRN always need a cost rate ("CGS Rate not found against ..").
     * Each kept row gets LineId 1..n, CostRate and CostAmount, which the detail insert then carries.
     */
    private List<SaleInvoiceModels.VoucherDetail> makeVoucher(UserAccount u, Map<String, Object> obj, String invoiceNo, String currencyName,
                                                              List<Map<String, Object>> details, SaleInvoiceModels.VoucherHead vh) {
        int customerId = asInt(obj.get("SupplierCustomerId"));
        List<Map<String, Object>> customers = repo.exportCustomers(u);
        if (!customers.isEmpty()) {
            Map<String, Object> c = null;
            for (Map<String, Object> r : customers) if (asInt(ci(r, "Id")) == customerId) { c = r; break; }
            if (c == null) throw new IllegalStateException("Customer GLAccountId not Found");
            vh.RefAccountId = asInt(ci(c, "GlAccountId"));
        }
        double exRate = asDouble(obj.get("ExchangeRate"));
        vh.DocumentTypeId = DOCUMENT_TYPE_ID;
        vh.DocumentTypeSrNo = asInt(obj.get("Id"));
        vh.RefDocNoId = asInt(obj.get("ExImInvoiceId"));
        vh.VoucherCode = asInt(obj.get("DocNo"));
        vh.VoucherDate = ((Timestamp) obj.get("DocDate")).toLocalDateTime();
        vh.Remarks = text(obj.get("RemarksHeader"));
        vh.RemarksOtherLingo = "";
        vh.VoucherAmount = asDouble(obj.get("LcyAmount"));
        vh.BillAmount = asDouble(obj.get("LcyAmount"));
        vh.ChequeDate = LocalDate.now().atStartOfDay();
        vh.IncludeWHT = false;
        vh.BranchId = asInt(obj.get("BranchesId"));
        vh.ProjectId = asInt(obj.get("ProjectsId"));
        vh.ManualBillNo = invoiceNo;
        vh.DueDate = LocalDateTime.now().withNano(0);
        vh.DueDays = 0;
        vh.ExchangeCurrencyRate = exRate;
        vh.FcAmount = asDouble(obj.get("FcyAmount"));
        vh.MultiCurrencyId = asInt(obj.get("CurrencyId"));
        vh.OrganizationId = u.getOrganizationId();
        vh.CompanyId = u.getCompanyId();
        vh.FinancialYearId = asInt(obj.get("FinancialYearId"));
        vh.EntryUser = asInt(obj.get("EntryUserId"));
        vh.EntryDate = LocalDateTime.now().withNano(0);
        vh.ModifyDate = LocalDateTime.now().withNano(0);
        vh.ModifyUser = asInt(obj.get("ModifyUserId"));
        vh.PostUser = 0;

        List<Map<String, Object>> itemGl = repo.itemGl(u);
        boolean anyWithoutGrn = false;
        for (Map<String, Object> d : details) if (asInt(d.get("GrnId")) == 0) { anyWithoutGrn = true; break; }
        int transitAccount = 0;
        boolean jobOrderWise = false;
        if (anyWithoutGrn) {
            transitAccount = asInt(repo.config(u, "ExportReturnStockInTransitAccount"));
            jobOrderWise = asBool(repo.config(u, "SaleCostingJobOrderWise"));
        }
        String text = text(obj.get("RemarksHeader"));                       // RemarksHeader != "" ? RemarksHeader : ""
        text = text + " InvoiceNo " + invoiceNo;
        int debitAccount = asInt(obj.get("DebitAccounId"));
        List<SaleInvoiceModels.VoucherDetail> out = new ArrayList<>();
        int line = 1;
        for (Map<String, Object> item : details) {
            if (asInt(item.get("ActionTypeId")) == 3) continue;
            if (itemGl.isEmpty()) throw new IllegalStateException("ItemGlAccount Not found");
            Map<String, Object> g = null;
            for (Map<String, Object> r : itemGl) if (asInt(ci(r, "Id")) == asInt(item.get("ItemId"))) { g = r; break; }
            if (g == null) throw new IllegalStateException("ItemGlAccount Not found");
            String itemName = text(ci(g, "ItemName"));
            item.put("LineId", line);
            double lcy = asDouble(item.get("LcyAmount")), fcy = asDouble(item.get("FcyAmount"));
            double qty = asDouble(item.get("Qty")), net = asDouble(item.get("NetWeight")), rate = asDouble(item.get("RatePrice"));
            int saleGl = asInt(ci(g, "SaleGLAC")), cogsGl = asInt(ci(g, "COGSGLAC")), purchaseGl = asInt(ci(g, "PurchaseGLAC"));
            text = text + " Item: " + itemName + " Qty: " + num(item.get("Qty")) + " Weight: " + num(item.get("NetWeight")) + " " + currencyName
                    + " @Rate: " + num(item.get("RatePrice")) + " ExchangeRate " + num(obj.get("ExchangeRate"));
            int saleSide = debitAccount > 0 ? debitAccount : saleGl;

            SaleInvoiceModels.VoucherDetail v1 = saleLine(saleSide, vh.RefAccountId, text, lcy, 0, item, qty, rate * exRate, net, customerId, asInt(obj.get("CurrencyId")), fcy, exRate, line);
            out.add(v1);
            SaleInvoiceModels.VoucherDetail v2 = saleLine(vh.RefAccountId, saleSide, text, 0, lcy, item, qty, rate * exRate, net, customerId, asInt(obj.get("CurrencyId")), fcy, exRate, line);
            out.add(v2);

            double stock = asDouble(item.get("StockWeight"));
            if (asInt(item.get("GrnId")) == 0) {
                if (transitAccount <= 0) throw new IllegalStateException("Export Return Stock InTransit Account not map in configuration please check...");
                double costRate = 0;
                if (jobOrderWise) costRate = repo.costRate(u, asInt(item.get("ItemId")), asInt(item.get("JobLotId")), asInt(item.get("ExImInvoiceId")),
                        asInt(item.get("ExImInvoiceDetailId")), 0, 0, 0, 0);
                item.put("CostRate", BigDecimal.valueOf(costRate));
                if (costRate > 0) {
                    double amt = stock * costRate;
                    item.put("CostAmount", BigDecimal.valueOf(amt));
                    String t2 = "Item: " + itemName + ",   Qty: " + num(item.get("Qty")) + ",   Weight: " + num(item.get("StockWeight")) + ",   Rate:" + num(costRate) + "  Item Amount: " + num(amt);
                    out.add(cogsLine(transitAccount, cogsGl, t2, amt, 0, item, qty, stock, costRate, customerId, line));
                    out.add(cogsLine(cogsGl, transitAccount, t2, 0, amt, item, qty, stock, costRate, customerId, line));
                }
            } else {
                double costRate = repo.costRate(u, asInt(item.get("ItemId")), asInt(item.get("JobLotId")), asInt(item.get("ExImInvoiceId")),
                        asInt(item.get("ExImInvoiceDetailId")), asInt(item.get("CropYearId")), asInt(item.get("WarehouseId")),
                        asInt(item.get("PackUomId")), asInt(item.get("PackingTypeId")));
                item.put("CostRate", BigDecimal.valueOf(costRate));
                if (costRate <= 0) throw new IllegalStateException("CGS Rate not found against " + itemName);
                double amt = stock * costRate;
                item.put("CostAmount", BigDecimal.valueOf(amt));
                String t3 = "Item: " + itemName + ",   Qty: " + num(item.get("Qty")) + ",   Weight: " + num(item.get("StockWeight")) + ",   Rate:" + num(costRate) + "  Item Amount: " + num(amt);
                int cogsSide = debitAccount > 0 ? debitAccount : cogsGl;
                out.add(cogsLine(purchaseGl, cogsSide, t3, amt, 0, item, qty, stock, costRate, customerId, line));
                out.add(cogsLine(cogsSide, purchaseGl, t3, 0, amt, item, qty, stock, costRate, customerId, line));
            }
            line++;
        }
        return out;
    }

    private static SaleInvoiceModels.VoucherDetail saleLine(int account, int against, String comments, double debit, double credit, Map<String, Object> item,
                                                           double qty, double itemRate, double weight, int customerId, int currencyId, double fcy, double exRate, int line) {
        SaleInvoiceModels.VoucherDetail d = new SaleInvoiceModels.VoucherDetail();
        d.AccountId = account;
        d.AgainstAccountId = against;
        d.Comments = comments;
        d.DebitAmount = debit;
        d.CreditAmount = credit;
        d.ItemId = asInt(item.get("ItemId"));
        d.QtyIn = qty;
        d.ItemRate = itemRate;
        d.WeightIn = weight;
        d.ItemAmount = debit != 0 ? debit : credit;
        d.SupplierCustomerId = customerId;
        d.DMultiCurrencyId = currencyId;
        d.DCurrencyAmount = fcy;
        d.DExchangeCurrencyRate = exRate;
        d.LineId = line;
        return d;
    }

    private static SaleInvoiceModels.VoucherDetail cogsLine(int account, int against, String comments, double debit, double credit, Map<String, Object> item,
                                                           double qty, double stock, double costRate, int customerId, int line) {
        SaleInvoiceModels.VoucherDetail d = new SaleInvoiceModels.VoucherDetail();
        d.AccountId = account;
        d.AgainstAccountId = against;
        d.Comments = comments;
        d.DebitAmount = debit;
        d.CreditAmount = credit;
        d.ItemId = asInt(item.get("ItemId"));
        d.JobLotId = asInt(item.get("JobLotId"));
        d.QtyIn = qty;
        d.WeightIn = stock;
        d.ItemRate = costRate;
        d.ItemCgsRate = costRate;
        d.ItemAmount = debit != 0 ? debit : credit;
        d.SupplierCustomerId = customerId;
        d.RefDocSubIdNo = asInt(item.get("Id"));
        d.IsCGS = 1;
        d.LineId = line;
        return d;
    }

    // ================================================================== frmLoadCommercialInvoiceForReturn

    /** InitializeComponentCustom (From = ActiveYr.Start_Period) + AllComboDBCall + GridRecordsDBCall. */
    public Map<String, Object> loaderSetup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("fromDate", iso(repo.financialYearStart(fy())));
        Map<String, List<Map<String, Object>>> combos = new LinkedHashMap<>();
        for (String k : new String[]{"Invoice", "ContractNo", "Customer", "Items", "JobLot", "Crop"}) combos.put(k, new ArrayList<>());
        for (Map<String, Object> r : repo.exportInvoiceDropDown(u)) {
            String a = text(ci(r, "ActivityType"));
            for (String k : combos.keySet()) {
                if (k.equalsIgnoreCase(a)) {
                    Map<String, Object> m = new LinkedHashMap<>();
                    m.put("Id", asInt(ci(r, "Id")));
                    m.put("name", text(ci(r, "name")));
                    combos.get(k).add(m);
                }
            }
        }
        out.put("combos", combos);
        return out;
    }

    /** GridRecordsDBCall: ExImInvoice.ExImInvoice_GetDataForReturn - FromDate / ToDate always set by the pickers. */
    public List<Map<String, Object>> loaderRows(Map<String, Object> b) {
        UserAccount u = user("View");
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        java.sql.Date from = sqlDate(b.get("fromDate")), to = sqlDate(b.get("toDate"));
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("Todate", to);
        int inv = asInt(b.get("exImInvoiceId")), contract = asInt(b.get("contractId")), cust = asInt(b.get("supplierCustomerId"));
        int item = asInt(b.get("itemId")), job = asInt(b.get("jobLotId")), crop = asInt(b.get("cropYearId"));
        if (inv != 0) p.put("ExImInvoiceId", inv);
        if (contract != 0) p.put("ContractId", contract);
        if (cust != 0) p.put("SupplierCustomerId", cust);
        if (item != 0) p.put("ItemId", item);
        if (job != 0) p.put("JobLotId", job);
        if (crop != 0) p.put("CropYearId", crop);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.invoiceDataForReturn(p)) out.add(plain(r));
        return out;
    }

    /** A procedure row with JSON-friendly values (dates as yyyy-MM-dd, numbers kept). */
    static Map<String, Object> plain(Map<String, Object> r) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (Map.Entry<String, Object> e : r.entrySet()) {
            Object v = e.getValue();
            if (v instanceof java.util.Date) v = iso(v);
            else if (v instanceof BigDecimal) v = ((BigDecimal) v).doubleValue();
            m.put(e.getKey(), v);
        }
        return m;
    }
}
