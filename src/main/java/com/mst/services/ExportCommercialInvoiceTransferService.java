package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.saleinvoice.SaleInvoiceModels;
import com.mst.repositories.ExportCommercialInvoiceTransferRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;

import static com.mst.services.ExportInvoiceTransferSupport.*;

/**
 * BLL side of 202 "Commercial Invoice" - Architecture.WinApp.Export.CommiercialInvoiceAgainstPreInvoiceTransfer
 * (caption "Commercial Invoice", ExImInvoice DocumentTypeId 211, rights by the form Name = ScreenDefinition 202).
 * tabControl1: Form | History | GD BreakUp | GD History | Invoices Allocation To GD (the last three are the desktop's
 * copies of the GD break up / advance utilize / multi invoices to GD forms; the page shows the existing ported
 * screens 881 and 907 there).
 *
 * Desktop behaviour kept (Form + History):
 *  - Detail rows only come from "Load Forwarding" (LoadForwardingForCommercialInvoice); "+" answers "Sorry Cannot Add
 *    New Record Only Update Detail Record : Thank You"; a loaded row has Bank Amount 0 and Bank HS Code "0".
 *  - btnsave_Click order: "Grid Record not found", confirmation, Net Weight = SUM(M.Ton) * 1000, formvalidation (texts and
 *    order below), per row "Bank Amount Can't Be Zero in Row#n" then "Packing ExpiryDate field is required in Row#n"
 *    (the "Production No" check can never fire - it tests ProductionNo == null || PackingExpiryDate == ""), other items
 *    with Qty > 0 only, TotalAmount = SUM(Amount) + Add/Less + other items, "UtilizeAmount cannot be greater than
 *    TotalBankAmount" (payment FcyAmount total against the Bank Amount total, only when payment rows exist).
 *  - RemarksHeader is saved from the Other Items "Remarks" box (txtoRemarks), not from the header Remarks box (which the
 *    form never reads); Certificate1 = the hidden Special Remarks box (empty), Certificate2 = "Health Permit No".
 *  - CommissionDebitAcId only when the Commission Amount > 0. BranchesId / ProjectsId from the hidden branch / project
 *    combos (first row). ApprovedDate = now, IsApproved false.
 *  - ExImInvoice.Save always builds the voucher for 211 (MakeVoucherForExImInvoice): Customer GL from the customer,
 *    CreditAccountId 0 -> the first item's SaleGLAC (and saved on the header), commission pair when Commission Amount
 *    > 0, a Commission Debit Ac and an agent GL; other-item fallback looks the item up by oItemAmount (sic); the credit
 *    line of an other item has no DocumentTypeIdRef / RefInvoiceNo.
 *  - ReadById: the HSCode column is filled with OtherHsCode, ContractType of row 0 decides InvoiceUpdateFlag (no rows ->
 *    "Index was out of range" and the rest of ReadById is skipped), BindOtherItems is called with @Id = 0.
 * Attachments (DMS) are not part of this web screen: the AttachmentsValues of a loaded record are written back.
 * Tenancy, user, branch and financial year come from the session only.
 */
@Service
public class ExportCommercialInvoiceTransferService {

    public static final int SCREEN_ID = 202;
    public static final int DOCUMENT_TYPE_ID = 211;

    @Autowired private ExportCommercialInvoiceTransferRepository repo;
    @Autowired private CurrentUserContext ctx;
    @Autowired private DesktopReportRights rights;

    private UserAccount user(String action) {
        UserAccount u = ctx.requireAccountingUser();
        rights.require(u, SCREEN_ID, action);
        return u;
    }

    private boolean allowed(UserAccount u, String action) {
        try { rights.require(u, SCREEN_ID, action); return true; } catch (AccessDeniedException e) { return false; }
    }

    private int fy() { return ctx.currentFinancialYearId(); }

    // ============================================================================ formats / combos

    private static String digits(int n, boolean zeroIsTwo) {
        switch (n) {
            case 1: return "0";
            case 2: return "00";
            case 3: return "000";
            case 4: return "0000";
            case 0: return zeroIsTwo ? "00" : "";
            default: return "";
        }
    }

    /** CommonServices.SetDecimalFormats: DecimalRateFormate, DecimalFCYRateFormate, stringFormatsingleForFcy. */
    private Map<String, Object> formats(UserAccount u) {
        Map<String, Object> f = new LinkedHashMap<>();
        f.put("rate", "#,#0." + digits(asInt(repo.config(u, "Default NoofDecimal Points For Rate")), true));
        f.put("fcyRate", "#,#0." + digits(asInt(repo.config(u, "DefaultNoOfDecimalPointsForFcyRate")), false));
        f.put("fcyAmount", "#,##0." + digits(asInt(repo.config(u, "DefaultNoOfDecimalPointsForFcyAmount")), false));
        return f;
    }

    private Map<String, Object> banks(UserAccount u) {
        List<Map<String, Object>> home = new ArrayList<>(), foreign = new ArrayList<>();
        for (Map<String, Object> b : repo.banks(u)) {
            String h = text(ci(b, "IsHomeland"));
            Map<String, Object> r = row("Id", asInt(ci(b, "Id")), "BranchName", text(ci(b, "BranchName")));
            if ("Foreign Country".equals(h)) foreign.add(r);
            if ("Home Country".equals(h)) home.add(r);
        }
        return row("exporter", home, "importer", foreign);
    }

    private List<Map<String, Object>> invoiceNos(UserAccount u, String invoiceNo) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.finalInvoiceNos(u, invoiceNo))
            out.add(row("Id", asInt(ci(r, "Id")), "MainId", asInt(ci(r, "MainId")), "PrefixTypeId", asInt(ci(r, "PrefixTypeId")), "InvoiceNo", text(ci(r, "FinalInvoiceNo"))));
        return out;
    }

    /** btnrefersh_Click list (branches, invoice nos, prefix types, projects, customers, terms, ports, currency, banks, packing types, crop years). */
    private Map<String, Object> lookups(UserAccount u) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("branches", pick(repo.branches(u), "Id", "BranchName"));
        m.put("invoiceNos", invoiceNos(u, null));
        m.put("prefixTypes", pick(repo.prefixTypes(), "Id", "PrefixDescription"));
        m.put("projects", pick(repo.projects(u), "Id", "ProjectName"));
        m.put("customers", pick(repo.customers(u), "Id", "CompanyName"));
        m.put("deliveryTerms", pick(repo.deliveryTerms(), "Id", "Code"));
        m.put("paymentTerms", pick(repo.paymentTerms(u), "Id", "LcOrderTerm"));
        m.put("ports", pick(repo.seaPorts(u), "Id", "PortName"));
        m.put("currencies", pick(repo.currencies(u), "Id", "CurrencyName"));
        m.put("banks", banks(u));
        m.put("packTypes", pick(repo.packingTypes(), "Id", "PackTypeDesc"));
        m.put("cropYears", pick(repo.cropYears(u), "Id", "CropYear"));
        return m;
    }

    /** ImProformaInvoice_Load. */
    public Map<String, Object> setup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, "Save"));
        perm.put("Update", allowed(u, "Update"));
        perm.put("Print", allowed(u, "Print"));
        out.put("permissions", perm);
        out.put("formats", formats(u));
        int code = repo.generateDocNo(u, fy());
        out.put("docNo", code > 0 ? String.valueOf(code) : "");
        out.put("preInvoices", pick(repo.preInvoices(u, 0), "Id", "InvoiceNo"));
        out.putAll(lookups(u));
        out.put("commissionDebitAccounts", pick(repo.coaByAccountTypes(u, "11,12,13,14,20,21"), "Id", "AccountTitle"));
        out.put("lastCommissionDebitAcId", repo.lastCommissionDebitAccountId(u));
        out.put("defaultDays", asInt(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        out.put("historyCustomers", historyCustomers(u));
        return out;
    }

    public Map<String, Object> refresh() { return lookups(user("View")); }

    public Map<String, Object> newCode() {
        UserAccount u = user("View");
        int code = repo.generateDocNo(u, fy());
        return row("docNo", code > 0 ? String.valueOf(code) : "");
    }

    /** BindCommercialInvoiceNofromlookup(InvoiceNo). */
    public List<Map<String, Object>> finalInvoiceNos(String invoiceNo) { return invoiceNos(user("View"), invoiceNo); }

    /**
     * cmbSupCust_Leave_1 (also CmbPreInvoice Leave and after the loader): BindPreInvoice (pre invoices of the customer),
     * BindFinancialInstrument (FI list) and BindHeaderInformPreInvoiceByPreInvoiceId(pre invoice).
     */
    public Map<String, Object> customerLeave(int customerId, int preInvoiceId) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("preInvoices", pick(repo.preInvoices(u, customerId), "Id", "InvoiceNo"));
        out.put("fis", fiRows(u));
        List<Map<String, Object>> h = repo.preInvoiceHeader(preInvoiceId);
        out.put("preInvoiceHeader", h.isEmpty() ? null : plain(h.get(0)));
        return out;
    }

    private List<Map<String, Object>> fiRows(UserAccount u) {
        return pick(repo.fiNos(u), "Id", "EFormNo", "PaymenttermId", "DocumentTypeId", "BalFIAmount");
    }

    public List<Map<String, Object>> fis() { return fiRows(user("View")); }

    /** bindRateUomAndItemPackUom (CommonServices.GetUomScheduleByItemId): Id, UOMCode, Equivalent. */
    public List<Map<String, Object>> uoms(int itemId) {
        UserAccount u = user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.uomSchedule(u, itemId)) out.add(row("Id", asInt(ci(r, "Id")), "UOMCode", text(ci(r, "UOMCode")), "Equivalent", asDouble(ci(r, "Equivalent"))));
        return out;
    }

    // ============================================================================ loader popup

    /** LoadInvoices_Load: Start_Period of the year is only shown (FromDate is never sent), combos, currency, expiry dates, production nos. */
    public Map<String, Object> loaderSetup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> prof = new ArrayList<>(), party = new ArrayList<>(), item = new ArrayList<>();
        for (Map<String, Object> r : repo.forwardingDropDowns(u)) {
            String a = text(ci(r, "Activity"));
            Map<String, Object> m = row("Id", text(ci(r, "Id")), "Name", text(ci(r, "ReferenceName")));
            if ("ContractNo".equals(a)) prof.add(m);
            if ("Customer".equals(a)) party.add(m);
            if ("Item".equals(a)) item.add(m);
        }
        out.put("proformas", prof);
        out.put("parties", party);
        out.put("items", item);
        out.put("currencies", pick(repo.currencies(u), "Id", "CurrencyName"));
        List<Map<String, Object>> exp = new ArrayList<>(), prod = new ArrayList<>();
        for (Map<String, Object> r : repo.packingExpiryDates(u)) exp.add(row("Id", text(ci(r, "PackingExpiryDate")), "ExpiryDate", text(ci(r, "PackingExpiryDate"))));
        for (Map<String, Object> r : repo.productionNos(u)) prod.add(row("Id", text(ci(r, "ProductionNo")), "ProductionNo", text(ci(r, "ProductionNo"))));
        out.put("expiryDates", exp);
        out.put("productionNos", prod);
        return out;
    }

    /** ExportPreInvoicesLoad (btngrnlod_Click): the raw rows (the page projects the grid and hands the checked raw rows back). */
    public List<Map<String, Object>> loaderSearch(Map<String, Object> f) {
        UserAccount u = user("View");
        return plain(repo.loaderData(u, fy(), asInt(f.get("proformaId")), asInt(f.get("customerId")), asInt(f.get("fcyId")), asInt(f.get("itemId")),
                text(f.get("packingExpiryDate")), text(f.get("productionNo")), asBool(f.get("skipZero")) ? 1 : 0));
    }

    // ============================================================================ read / history

    private Map<String, Object> owned(UserAccount u, int id) {
        if (id <= 0) return null;
        Map<String, Object> h = repo.header(id);
        if (h == null) return null;
        if (asInt(ci(h, "OrganizationId")) != u.getOrganizationId() || asInt(ci(h, "CompanyId")) != u.getCompanyId()) return null;
        return h;
    }

    /** dtdetail row from an ExImInvoicePackingDetail (ReadById column order). */
    private static Map<String, Object> detailRow(Map<String, Object> d) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(ci(d, "Id")));
        m.put("PerformaId", asInt(ci(d, "ContractId")));
        m.put("PerformaDetailId", asInt(ci(d, "ContractDetailId")));
        m.put("PerformaDate", iso(ci(d, "ProformaDocDate")));
        m.put("PriRefNo", text(ci(d, "PriRefNo")));
        m.put("ItemId", asInt(ci(d, "ItemId")));
        m.put("ItemName", text(ci(d, "ItemName")));
        m.put("HSCode", text(ci(d, "OtherHsCode")));                 /* desktop quirk: HSCode column <- OtherHsCode */
        m.put("PackTypeId", asInt(ci(d, "PackingMaterialTypeId")));
        m.put("PackType", text(ci(d, "PackMaterilaType")));
        m.put("PackingWeight", asDouble(ci(d, "PackingWeight")));
        m.put("CropYearId", asInt(ci(d, "CropYearId")));
        m.put("CropYear", text(ci(d, "CropYear")));
        m.put("NoOfContainers", asDouble(ci(d, "NoofContainers")));
        m.put("NoOfBagsCntnr", asDouble(ci(d, "NoofBagsPerContainer")));
        m.put("PackSizeId", asInt(ci(d, "OuterQtyUomId")));
        m.put("PackSize", text(ci(d, "OuterUOM")));
        m.put("QtyMTon", asDouble(ci(d, "MTon")));
        m.put("NoOfBags", asDouble(ci(d, "OuterQty")));
        m.put("ValidateBags", asDouble(ci(d, "OuterQty")));
        m.put("NetWeight", asDouble(ci(d, "NetWeight")));
        m.put("ValidateNetWeight", asDouble(ci(d, "NetWeight")));
        m.put("TotalPackingWeight", asDouble(ci(d, "TotalPackingWeight")));
        m.put("GrossWeight", asDouble(ci(d, "GrossWeight")));
        m.put("RateWithoutAddLess", asDouble(ci(d, "RateWithoutAddLess")));
        m.put("AddLessRate", asDouble(ci(d, "RateAddLess")));
        m.put("CostMTon", asDouble(ci(d, "RatePrice")));
        m.put("ContractRate", asDouble(ci(d, "ContractRate")));
        m.put("RateUOMId", asInt(ci(d, "RateUomId")));
        m.put("RateUOM", text(ci(d, "RateUOM")));
        m.put("Amount", asDouble(ci(d, "FcAmount")));
        m.put("OtherRate", asDouble(ci(d, "OtherRate")));
        m.put("OtherAmount", asDouble(ci(d, "OtherAmount")));
        m.put("OtherHSCode", text(ci(d, "OtherHsCode")));
        m.put("PackingExpiryDate", text(ci(d, "PackingExpiryDate")));
        m.put("ProductionNo", text(ci(d, "ProductionNo")));
        m.put("ItemCommodityDetail", text(ci(d, "ItemCommodityDetail")));
        m.put("HealthPermitNoDetail", text(ci(d, "HealthPermitNoDetail")));
        return m;
    }

    /** ReadById (history double-click). */
    public Map<String, Object> readById(int id) {
        UserAccount u = user("View");
        Map<String, Object> h = owned(u, id);
        if (h == null) throw new IllegalStateException("Index was out of range. Must be non-negative and less than the size of the collection.");
        Map<String, Object> hd = new LinkedHashMap<>();
        for (String k : new String[] { "Id", "BranchesId", "ProjectsId", "DocCode", "PrefixTypeId", "PreInvoiceId", "SupplierCustomerId", "CommissionAgentId",
                "CommissionDebitAcId", "DestinationPortId", "OtherCustomerId", "OtherDestinationPortId", "LcOrderNoId", "NotifyParty1", "NotifyParty2",
                "ImporterBankId", "ExporteBankId", "PaymentTermId", "DeliveryTermId", "LoadingPortId", "FcurrencyId" }) hd.put(k, asInt(ci(h, k)));
        hd.put("DocDate", iso(ci(h, "DocDate")));
        hd.put("InvoiceNo", text(ci(h, "InvoiceNo")));
        hd.put("LotNoRef", text(ci(h, "LotNoRef")));
        hd.put("CarierType", text(ci(h, "CarierType")));
        hd.put("EFormNo", text(ci(h, "EFormNo")));
        hd.put("EFormDate", iso(ci(h, "EFormDate")));
        hd.put("Certificate1", text(ci(h, "Certificate1")));
        hd.put("Certificate2", text(ci(h, "Certificate2")));
        hd.put("AddLessComments", text(ci(h, "AddLessComments")));
        /* double.ToString() of the stored amounts */
        for (String k : new String[] { "CommissionPercentage", "CommissionAmount", "FCurrencyAmount", "ConversionRate", "EquivalentAmount", "GrossWeight",
                "NetWeight", "NoOfContainers", "AddLessAmount", "TotalAmount" }) hd.put(k, clr(asDouble(ci(h, k))));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", hd);
        List<Map<String, Object>> rows = new ArrayList<>();
        int contractType = 0;
        List<Map<String, Object>> det = repo.child(id, "ReadExImInvoicePackingDetailByHeaderId");
        for (Map<String, Object> d : det) rows.add(detailRow(d));
        if (!det.isEmpty()) contractType = asInt(ci(det.get(0), "ContractType"));
        out.put("rows", rows);
        out.put("invoiceUpdateFlag", contractType);
        List<Map<String, Object>> other = new ArrayList<>();
        for (Map<String, Object> d : repo.child(id, "ReadExImInvoiceOtherItemsByHeaderId"))
            other.add(row("ItemId", asInt(ci(d, "otherItemId")), "ItemName", text(ci(d, "ItemName")), "Qty", asDouble(ci(d, "oItemQty")),
                    "Rate", asDouble(ci(d, "oItemRate")), "Amount", asDouble(ci(d, "oItemAmount")), "Remarks", text(ci(d, "OtherItemRemarks"))));
        out.put("otherItems", other);
        List<Map<String, Object>> pay = new ArrayList<>();
        for (Map<String, Object> d : repo.child(id, "ReadExImInvoicePaymentTermsDetailByHeaderId"))
            pay.add(row("Id", asInt(ci(d, "Id")), "DocumentTypeId", asInt(ci(d, "DocumentTypeId")), "ExImEFormRegistrationId", asInt(ci(d, "ExImEFormRegistrationId")),
                    "FinancialInstrumentNo", text(ci(d, "FinancialInstrumentNo")), "PaymentTermId", asInt(ci(d, "PaymentTermId")),
                    "PaymentTerm", text(ci(d, "PaymentTerm")), "PrcntOfTotal", asDouble(ci(d, "PrcntOfTotal")), "FcyAmount", asDouble(ci(d, "FcyAmount")),
                    "DueDays", text(ci(d, "DueDays"))));
        out.put("paymentTerms", pay);
        out.put("otherItemList", pick(repo.otherItemsByContract(u), "ItemId", "ItemName", "oItemRate"));
        out.put("fis", fiRows(u));
        return out;
    }

    private List<Map<String, Object>> historyCustomers(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.historyDropDowns(u))
            if ("Customer".equals(text(ci(r, "ActivityType")))) out.add(row("Id", asInt(ci(r, "Id")), "Customer", text(ci(r, "name"))));
        return out;
    }

    /** btnRefreshHistory_Click -> HistoryCombosFill. */
    public List<Map<String, Object>> historyCombos() { return historyCustomers(user("View")); }

    private static String netDate(Object o) {
        String s = iso(o);
        if (s.isEmpty()) return "1/1/0001";                       /* Conversion.ToDateTime(DBNull) = DateTime.MinValue */
        LocalDate d = LocalDate.parse(s);
        return d.getMonthValue() + "/" + d.getDayOfMonth() + "/" + d.getYear();
    }

    /** HistoryGridFill: ExImInvoice.FormHistory with Ids "211", projected to the desktop columns. */
    public List<Map<String, Object>> history(Map<String, Object> f) {
        UserAccount u = user("View");
        Timestamp from = asBool(f.get("fromChecked")) ? pickerDate(f.get("fromDate")) : null;
        Timestamp to = asBool(f.get("toChecked")) ? pickerDate(f.get("toDate")) : null;
        String mode = text(f.get("dateMode"));
        Map<String, Timestamp> dates = new LinkedHashMap<>();
        if ("doc".equals(mode)) { dates.put("FromDate", from); dates.put("ToDate", to); }
        else if ("entry".equals(mode)) { dates.put("EntryFromDate", from); dates.put("EntryToDate", to); }
        else if ("modify".equals(mode)) { dates.put("ModifyFromDate", from); dates.put("ModifyToDate", to); }
        else if ("approved".equals(mode)) { dates.put("ApprovedFromDate", from); dates.put("ApprovedToDate", to); }
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.formHistory(u, fy(), dates, asInt(f.get("customerId")), asInt(f.get("fromDocNo")), asInt(f.get("toDocNo")))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("VoucherHeadId", asInt(ci(r, "VoucherHeadId")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("DocCode", asInt(ci(r, "DocCode")));
            m.put("DocDate", netDate(ci(r, "DocDate")));
            m.put("CustomerName", text(ci(r, "Customer")));
            m.put("NoOfContainers", asDouble(ci(r, "NoOfContainers")));
            m.put("GrossWeight", asDouble(ci(r, "GrossWeight")));
            m.put("NetWeight", asDouble(ci(r, "NetWeight")));
            m.put("FcyAmount", asDouble(ci(r, "FCurrencyAmount")));
            m.put("AddLess", asDouble(ci(r, "AddLessAmount")));
            m.put("TotalAmount", asDouble(ci(r, "TotalAmount")));
            m.put("LoadingPort", text(ci(r, "LoadingPort")));
            m.put("DestinationPort", text(ci(r, "DestinationPort")));
            m.put("EntryUser", text(ci(r, "EntryUserName")));
            m.put("EntryDate", netDate(ci(r, "EntryDate")));
            m.put("ModifyUser", text(ci(r, "ModifyUserName")));
            m.put("ModifyDate", netDate(ci(r, "ModifyDate")));
            m.put("ApprovedStatus", text(ci(r, "ApprovedStatus")));
            m.put("ApprovedUser", text(ci(r, "ApprovedUserName")));
            m.put("ApprovedDate", netDate(ci(r, "ApprovedDate")));
            m.put("NoOfAttachments", text(ci(r, "NoOfAttachments")));
            out.add(m);
        }
        return out;
    }

    /** GetDetailByHeaderId (DataGridHistory_SelectionChanged). */
    public List<Map<String, Object>> historyDetail(int id) {
        UserAccount u = user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        if (owned(u, id) == null) return out;
        for (Map<String, Object> d : repo.child(id, "ReadExImInvoicePackingDetailByHeaderId")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ItemName", text(ci(d, "ItemName")));
            m.put("HSCode", text(ci(d, "HsCode")));
            m.put("PackingType", text(ci(d, "PackMaterilaType")));
            m.put("PackingWeight", asDouble(ci(d, "PackingWeight")));
            m.put("CropYear", text(ci(d, "CropYear")));
            m.put("NoOfContainers", asDouble(ci(d, "NoofContainers")));
            m.put("NoOfBagsCntnr", asDouble(ci(d, "NoofBagsPerContainer")));
            m.put("PackUom", text(ci(d, "OuterUOM")));
            m.put("MTon", asDouble(ci(d, "MTon")));
            m.put("NoOfBags", asDouble(ci(d, "OuterQty")));
            m.put("NetWeight", asDouble(ci(d, "NetWeight")));
            m.put("TotalPackingWeight", asDouble(ci(d, "TotalPackingWeight")));
            m.put("GrossWeight", asDouble(ci(d, "GrossWeight")));
            m.put("RateMTon", asDouble(ci(d, "RatePrice")));
            m.put("RateUOM", text(ci(d, "RateUOM")));
            m.put("Amount", asDouble(ci(d, "FcAmount")));
            m.put("BankRate", asDouble(ci(d, "OtherRate")));
            m.put("BankAmount", asDouble(ci(d, "OtherAmount")));
            m.put("BankHSCode", text(ci(d, "OtherHsCode")));
            m.put("PackingExpiryDate", text(ci(d, "PackingExpiryDate")));
            m.put("ItemCommodityDetail", text(ci(d, "ItemCommodityDetail")));
            m.put("HealthPermitNoDetail", text(ci(d, "HealthPermitNoDetail")));
            out.add(m);
        }
        return out;
    }

    // ============================================================================ save

    /** formvalidation - the page sends the values; "active row" checks are the combo values / texts. */
    private void validate(Map<String, Object> h) {
        if (text(h.get("txtdocno")).isEmpty() || asInt(h.get("txtdocno")) == 0) throw new IllegalArgumentException("Doc No Is Required");
        if (asInt(h.get("CmbPreInvoice")) == 0) throw new IllegalArgumentException("PreInvoice Is Required");
        if (asInt(h.get("cmbPrefixType")) <= 0) throw new IllegalArgumentException("Prefix Type Is Required");
        String inv = text(h.get("CmbinvoicenoText"));
        if (inv.isEmpty() || "0".equals(inv.trim())) throw new IllegalArgumentException("Invoice No Is Required");
        if (asInt(h.get("cmbSupCust")) == 0) throw new IllegalArgumentException("Customer Is Required");
        if (asInt(h.get("cmbPaymentTermsNew")) == 0) throw new IllegalArgumentException("Payment Term Is Required");
        if (asInt(h.get("cmbdeliverytermnew")) == 0) throw new IllegalArgumentException("Delivery Term Is Required");
        if (asInt(h.get("cmbLoadingPort")) == 0) throw new IllegalArgumentException("Loading Port Is Required");
        if (asInt(h.get("cmbDestinationPort")) == 0) throw new IllegalArgumentException("Destination Port  Is Required");
        if (text(h.get("cmbDestinationPortText")).trim().equals(text(h.get("cmbLoadingPortText")).trim()))
            throw new IllegalArgumentException("Destination Port and Loaing Port cannot be same...");
        if (text(h.get("cmbcareiertypeText")).isEmpty()) throw new IllegalArgumentException("Carier Type  Is Required");
        String gd = text(h.get("txtGdNo"));
        if (gd.isEmpty() || "0".equals(gd)) throw new IllegalArgumentException("GD_number Is Required");
        if (asInt(h.get("cmbfcycode")) == 0) throw new IllegalArgumentException("Fcy Code Is Required");
        if (asDouble(h.get("txtfcyAmount")) == 0) throw new IllegalArgumentException("Fcy Amount  Is Required");
        if (!(asDouble(h.get("txtNetWeight")) > 0)) throw new IllegalArgumentException("Net Weight  Is Required");
        if (!(asDouble(h.get("txtGrossWeight")) >= asDouble(h.get("txtNetWeight"))))
            throw new IllegalArgumentException("Gross Weight Must Be Equal Or Greater Than Net Weight Thank You");
        boolean agent = asInt(h.get("cmbCommissionAgent")) != 0;
        if (asDouble(h.get("txtCommPercent")) > 0 && !agent)
            throw new IllegalArgumentException("As you have entered 'Commission %', the 'Commission Agent' field is required. ");
        if (asDouble(h.get("txtCommPercent")) == 0 && agent)
            throw new IllegalArgumentException("As you have selected 'Commission agent', please enter the commission percentage.");
        if (asDouble(h.get("txtCommAmount")) > 0 && agent && asInt(h.get("CmbCommissionDebitAccount")) == 0)
            throw new IllegalArgumentException("Commission DebitAccount Is Required");
        if (!(asDouble(h.get("txtExRate")) > 0)) throw new IllegalArgumentException("ExchangeRate Field is Required");
        if (!(asDouble(h.get("txtTotalNetAmount")) > 0)) throw new IllegalArgumentException("Total Amount  Is Required");
    }

    /** ",a,b,a".Split(',').Distinct(): one element -> that element, else string.Join(",", ...).TrimStart(','). */
    private static String distinctJoin(String csv) {
        if (csv == null || csv.isEmpty()) return null;
        LinkedHashSet<String> set = new LinkedHashSet<>();
        for (String s : csv.split(",", -1)) set.add(s);
        if (set.size() == 1) return set.iterator().next();
        String j = String.join(",", set);
        int i = 0;
        while (i < j.length() && j.charAt(i) == ',') i++;
        return j.substring(i);
    }

    private static String shortDate(Object v) {
        String s = iso(v);
        if (s.isEmpty()) return "1/1/0001";
        LocalDate d = LocalDate.parse(s);
        return d.getMonthValue() + "/" + d.getDayOfMonth() + "/" + d.getYear();
    }

    /** ExImInvoicePackingDetail: every int/double property at its default; strings left out (null = not sent). */
    private static Map<String, Object> blankPacking() {
        Map<String, Object> d = new LinkedHashMap<>();
        for (String k : new String[] { "FcAmount", "NetWeight", "OuterQty", "RatePrice", "RateAddLess", "RateWithoutAddLess", "PackingWeight",
                "TotalPackingWeight", "GrossWeight", "NoofContainers", "NoofBagsPerContainer", "MTon", "OtherRate", "OtherAmount", "AddLessAmount",
                "InnerQty", "PalletQty" }) d.put(k, 0d);
        for (String k : new String[] { "ExImFarmingNTradeId", "ExImTradeTypeId", "ExImFarmingTypeId", "LabInspectionId", "PmItemId", "BrandId",
                "ExImInvoiceId", "ContractId", "ContractDetailId", "Id", "InnerQtyUomId", "ItemId", "ItemUOMId", "CropYearId", "OuterQtyUomId",
                "PackingMaterialTypeId", "RateUomId", "JobLotId", "ItemSpicificationId", "PreInvoiceId", "PreInvoiceDetailId", "LineId",
                "ContractScheduleId", "ContractScheduleDetailId", "ActionTypeId", "AmountCalulationId" }) d.put(k, 0);
        return d;
    }

    /** grdDetail_ColumnButtonClick "Delete" on a saved row (ActionTypeId 3): NetWeight through Conversion.ToInt. */
    private static Map<String, Object> removedModel(Map<String, Object> r) {
        Map<String, Object> vd = blankPacking();
        vd.put("Id", asInt(r.get("Id")));
        vd.put("ContractId", asInt(r.get("PerformaId")));
        vd.put("ItemId", asInt(r.get("ItemId")));
        vd.put("HsCode", text(r.get("HSCode")));
        vd.put("PackingMaterialTypeId", asInt(r.get("PackTypeId")));
        vd.put("PackingWeight", asDouble(r.get("PackingWeight")));
        vd.put("CropYearId", asInt(r.get("CropYearId")));
        vd.put("NoofContainers", asDouble(r.get("NoOfContainers")));
        vd.put("NoofBagsPerContainer", asDouble(r.get("NoOfBagsCntnr")));
        vd.put("OuterQtyUomId", asInt(r.get("PackSizeId")));
        vd.put("InnerQtyUomId", asInt(r.get("PackSizeId")));
        vd.put("MTon", asDouble(r.get("QtyMTon")));
        vd.put("OuterQty", asDouble(r.get("NoOfBags")));
        vd.put("InnerQty", (double) asInt(r.get("NoOfBags")));
        vd.put("NetWeight", (double) asInt(r.get("NetWeight")));
        vd.put("TotalPackingWeight", asDouble(r.get("TotalPackingWeight")));
        vd.put("GrossWeight", asDouble(r.get("GrossWeight")));
        vd.put("RateWithoutAddLess", asDouble(r.get("RateWithoutAddLess")));
        vd.put("RateAddLess", asDouble(r.get("AddLessRate")));
        vd.put("RatePrice", asDouble(r.get("CostMTon")));
        vd.put("RateUomId", asInt(r.get("RateUOMId")));
        vd.put("FcAmount", asDouble(r.get("Amount")));
        vd.put("OtherRate", asDouble(r.get("OtherRate")));
        vd.put("OtherAmount", asDouble(r.get("OtherAmount")));
        vd.put("OtherHsCode", text(r.get("OtherHSCode")));
        vd.put("PackingExpiryDate", text(r.get("PackingExpiryDate")));
        vd.put("ProductionNo", text(r.get("ProductionNo")));
        vd.put("ItemCommodityDetail", text(r.get("ItemCommodityDetail")));
        vd.put("HealthPermitNoDetail", text(r.get("HealthPermitNoDetail")));
        vd.put("ActionTypeId", 3);
        return vd;
    }

    /** Every non-virtual ExImInvoice value property at its CLR default (strings null = not sent). */
    private static Map<String, Object> blankHeader() {
        Map<String, Object> p = new LinkedHashMap<>();
        for (String k : new String[] { "Id", "CommercialInvoiceId", "ApprovedUser", "BranchesId", "CompanyId", "PrefixTypeId", "DeliveryTermId", "ContinentId",
                "OriginCountryId", "ImporterCountryId", "PlaceOfDeliveryId", "DestinationPortId", "DocCode", "DocumentTypeId", "EFormId", "EntryUser",
                "ExporteBankId", "ExportCompanyId", "FcurrencyId", "ImporterBankId", "LcOrderNoId", "LoadingPortId", "ModifyUser", "OrganizationId",
                "PaymentTermId", "ProjectsId", "SupplierCustomerId", "CommissionAgentId", "FinancialYearId", "PreInvoiceId", "CommissionDebitAcId",
                "OtherCustomerId", "OtherDestinationPortId", "ConsigneeId", "ActionId", "NotifyParty1", "NotifyParty2", "NotifyParty3",
                "CreditAccountId", "SubContractExpiryDays" }) p.put(k, 0);
        for (String k : new String[] { "BuyerGLValueRs", "ConversionRate", "EquivalentAmount", "FCurrencyAmount", "FobValue", "GrossWeight", "InvoiceCommercialValue",
                "NetWeight", "NoOfContainers", "AddLessAmount", "CommissionPercentage", "CommissionAmount", "TotalAmount" }) p.put(k, 0d);
        p.put("GrossMton", BigDecimal.ZERO);
        p.put("NetMton", BigDecimal.ZERO);
        p.put("IsApproved", false);
        return p;
    }

    /** btnsave_Click / btnupdate_Click (the confirmation is asked on the page first). */
    public Map<String, Object> save(Map<String, Object> body) {
        int recId = asInt(body.get("recId"));
        UserAccount u = user(recId == 0 ? "Save" : "Update");
        Map<String, Object> existing = null;
        if (recId > 0) { existing = owned(u, recId); if (existing == null) throw new IllegalArgumentException("Record Not Found"); }
        Map<String, Object> h = map(body.get("header"));
        List<Map<String, Object>> rows = list(body.get("rows"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record not found");
        double mton = 0;
        for (Map<String, Object> r : rows) mton += asDouble(r.get("QtyMTon"));
        double netWeightKg = mton * 1000.0;
        h.put("txtNetWeight", clr(netWeightKg));                   /* txtNetWeight.Text = NetWeightkg.ToString() */
        validate(h);

        Timestamp now = now();
        Map<String, Object> pih = blankHeader();
        pih.put("Id", recId > 0 ? recId : 0);
        pih.put("OrganizationId", u.getOrganizationId());
        pih.put("CompanyId", u.getCompanyId());
        pih.put("IsApproved", false);
        pih.put("EntryDate", now);
        pih.put("EntryUser", u.getId());
        pih.put("ModifyDate", now);
        pih.put("ModifyUser", u.getId());
        pih.put("ApprovedDate", now);
        pih.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        pih.put("FinancialYearId", fy());
        pih.put("BranchesId", asInt(h.get("cmbbranches")));
        pih.put("ProjectsId", asInt(h.get("cmbproject")));
        pih.put("DocCode", asInt(h.get("txtdocno")));
        pih.put("DocDate", pickerDate(h.get("txtDocDate")));
        pih.put("PrefixTypeId", asInt(h.get("cmbPrefixType")));
        pih.put("InvoiceNo", text(h.get("CmbinvoicenoText")).trim());
        pih.put("PreInvoiceId", asInt(h.get("CmbPreInvoice")));
        pih.put("SupplierCustomerId", asInt(h.get("cmbSupCust")));
        pih.put("NotifyParty1", asInt(h.get("cmbNotifyParty1")));
        pih.put("NotifyParty2", asInt(h.get("cmbNotifyParty2")));
        pih.put("LotNoRef", text(h.get("txtLotRef")));
        pih.put("PaymentTermId", asInt(h.get("cmbPaymentTermsNew")));
        pih.put("ImporterBankId", asInt(h.get("cmbimporterBankNew")));
        pih.put("ExporteBankId", asInt(h.get("cmbexporterBankNew")));
        pih.put("DeliveryTermId", asInt(h.get("cmbdeliverytermnew")));
        pih.put("LoadingPortId", asInt(h.get("cmbLoadingPort")));
        pih.put("DestinationPortId", asInt(h.get("cmbDestinationPort")));
        pih.put("OtherDestinationPortId", asInt(h.get("cmbOtherDestinationPort")));
        pih.put("OtherCustomerId", asInt(h.get("cmbOtherCustomer")));
        pih.put("CarierType", text(h.get("cmbcareiertypeText")));
        double commAmount = asDouble(text(h.get("txtCommAmount")).trim());
        pih.put("CommissionAmount", commAmount);
        pih.put("CommissionAgentId", asInt(h.get("cmbCommissionAgent")));
        pih.put("CommissionPercentage", asDouble(text(h.get("txtCommPercent")).trim()));
        if (commAmount > 0) pih.put("CommissionDebitAcId", asInt(h.get("CmbCommissionDebitAccount")));
        pih.put("FcurrencyId", asInt(h.get("cmbfcycode")));
        pih.put("FCurrencyAmount", asDouble(text(h.get("txtfcyAmount")).trim()));
        pih.put("ConversionRate", asDouble(text(h.get("txtExRate")).trim()));
        pih.put("EquivalentAmount", asDouble(text(h.get("txtLocalAmt")).trim()));
        pih.put("GrossWeight", asDouble(text(h.get("txtGrossWeight")).trim()));
        pih.put("NetWeight", netWeightKg);
        pih.put("NoOfContainers", (double) asInt(text(h.get("txtNoOfContainer")).trim()));
        pih.put("EFormNo", text(h.get("txtGdNo")).trim());
        pih.put("EFormDate", pickerDate(h.get("txtIformDate")));
        pih.put("Certificate2", text(h.get("txtcertificateOfOrigion")).trim());
        pih.put("Certificate1", text(h.get("txtcertificate")).trim());
        pih.put("AddLessComments", text(h.get("txtaddlesscommnets")));
        double addLess = asDouble(text(h.get("txtaddlessamount")).trim());
        pih.put("AddLessAmount", addLess);
        pih.put("TotalAmount", asDouble(text(h.get("txtTotalNetAmount")).trim()));
        pih.put("RemarksHeader", text(h.get("txtoRemarks")).trim());
        pih.put("AttachmentsValues", existing == null ? null : (ci(existing, "AttachmentsValues") == null ? null : text(ci(existing, "AttachmentsValues"))));
        pih.put("CustomAttachmentsValues", existing == null ? null : (ci(existing, "CustomAttachmentsValues") == null ? null : text(ci(existing, "CustomAttachmentsValues"))));

        List<Map<String, Object>> packing = new ArrayList<>();
        if (recId > 0) for (Map<String, Object> r : list(body.get("removed"))) packing.add(removedModel(r));
        StringBuilder contractIds = new StringBuilder(), contractDetailIds = new StringBuilder(), contractNos = new StringBuilder(), contractDates = new StringBuilder();
        double totalFc = 0, totalBank = 0;
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> r = rows.get(i);
            Map<String, Object> vd = blankPacking();
            vd.put("Id", asInt(r.get("Id")));
            vd.put("ContractId", asInt(r.get("PerformaId")));
            contractIds.append(',').append(text(r.get("PerformaId")));
            contractDetailIds.append(',').append(text(r.get("PerformaDetailId")));
            contractNos.append(',').append(text(r.get("PriRefNo")));
            contractDates.append(',').append(shortDate(r.get("PerformaDate")));
            vd.put("ItemId", asInt(r.get("ItemId")));
            vd.put("HsCode", text(r.get("HSCode")));
            vd.put("PackingMaterialTypeId", asInt(r.get("PackTypeId")));
            vd.put("PackingWeight", asDouble(r.get("PackingWeight")));
            vd.put("CropYearId", asInt(r.get("CropYearId")));
            vd.put("NoofContainers", asDouble(r.get("NoOfContainers")));
            vd.put("NoofBagsPerContainer", asDouble(r.get("NoOfBagsCntnr")));
            vd.put("OuterQtyUomId", asInt(r.get("PackSizeId")));
            vd.put("InnerQtyUomId", asInt(r.get("PackSizeId")));
            vd.put("MTon", asDouble(r.get("QtyMTon")));
            vd.put("OuterQty", asDouble(r.get("NoOfBags")));
            vd.put("InnerQty", (double) asInt(r.get("NoOfBags")));
            vd.put("NetWeight", asDouble(r.get("NetWeight")));
            vd.put("TotalPackingWeight", asDouble(r.get("TotalPackingWeight")));
            vd.put("GrossWeight", asDouble(r.get("GrossWeight")));
            vd.put("RateWithoutAddLess", asDouble(r.get("RateWithoutAddLess")));
            vd.put("RateAddLess", asDouble(r.get("AddLessRate")));
            vd.put("RatePrice", asDouble(r.get("CostMTon")));
            vd.put("RateUomId", asInt(r.get("RateUOMId")));
            vd.put("FcAmount", asDouble(r.get("Amount")));
            double otherRate = asDouble(r.get("OtherRate")), otherAmount = asDouble(r.get("OtherAmount"));
            vd.put("OtherRate", otherRate);
            vd.put("OtherAmount", otherAmount);
            totalBank += otherAmount;
            if (otherRate > 0 && otherAmount == 0) throw new IllegalArgumentException("Bank Amount Can't Be Zero in Row#" + (i + 1));
            totalFc += asDouble(vd.get("FcAmount"));
            vd.put("OtherHsCode", text(r.get("OtherHSCode")));
            String expiry = text(r.get("PackingExpiryDate"));
            vd.put("PackingExpiryDate", expiry);
            vd.put("ProductionNo", text(r.get("ProductionNo")));
            if (expiry.isEmpty()) throw new IllegalArgumentException("Packing ExpiryDate field is required in Row#" + (i + 1));
            /* "Production No field is required" - unreachable on the desktop (ProductionNo == null || PackingExpiryDate == "") */
            vd.put("ItemCommodityDetail", text(r.get("ItemCommodityDetail")));
            vd.put("HealthPermitNoDetail", text(r.get("HealthPermitNoDetail")));
            vd.put("ActionTypeId", asInt(vd.get("Id")) <= 0 ? 1 : 2);
            packing.add(vd);
        }
        pih.put("ContractIds", distinctJoin(contractIds.toString()));
        pih.put("ContractDetailIds", distinctJoin(contractDetailIds.toString()));
        pih.put("ContractNos", distinctJoin(contractNos.toString()));
        pih.put("ContractDates", distinctJoin(contractDates.toString()));

        double otherAmountTotal = 0, utilize = 0;
        List<Map<String, Object>> otherItems = new ArrayList<>();
        for (Map<String, Object> r : list(body.get("otherItems"))) {
            BigDecimal qty = BigDecimal.valueOf(asDouble(r.get("Qty")));
            if (qty.signum() <= 0) continue;
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("oItemAmount", BigDecimal.valueOf(asDouble(r.get("Amount"))));
            d.put("oItemQty", qty);
            d.put("oItemWeightKgs", BigDecimal.ZERO);
            d.put("oItemRate", asDouble(r.get("Rate")));
            d.put("ExImInvoiceId", 0);
            d.put("Id", 0);
            d.put("otherItemId", asInt(r.get("ItemId")));
            d.put("OtherItemRemarks", text(r.get("Remarks")));
            otherAmountTotal += asDouble(r.get("Amount"));
            otherItems.add(d);
        }
        List<Map<String, Object>> payments = new ArrayList<>();
        List<Map<String, Object>> payRows = list(body.get("paymentTerms"));
        for (int i = 0; i < payRows.size(); i++) {
            Map<String, Object> r = payRows.get(i);
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("FcyAmount", asDouble(r.get("FcyAmount")));
            d.put("PrcntOfTotal", asDouble(r.get("PrcntOfTotal")));
            d.put("DueDays", asInt(r.get("DueDays")));
            d.put("EntryUserId", 0);
            d.put("ModifyUserId", 0);
            d.put("ExImEFormRegistrationId", asInt(r.get("ExImEFormRegistrationId")));
            d.put("ExImInvoiceId", 0);
            d.put("RefDocumentTypeId", 0);
            d.put("Id", asInt(r.get("Id")));
            d.put("PaymentTermId", asInt(r.get("PaymentTermId")));
            d.put("DocumentTypeId", asInt(r.get("DocumentTypeId")));
            d.put("SortNo", i + 1);
            d.put("FinancialInstrumentNo", text(r.get("FinancialInstrumentNo")));
            utilize += asDouble(r.get("FcyAmount"));
            payments.add(d);
        }
        double totalAmount = totalFc + addLess + otherAmountTotal;
        pih.put("TotalAmount", totalAmount);
        if (!payRows.isEmpty() && utilize > totalBank) throw new IllegalArgumentException("UtilizeAmount cannot be greater than TotalBankAmount");

        pih.put("ActionId", recId == 0 ? 1 : 2);
        Voucher v = makeVoucher(pih, packing, otherItems, repo.supplierCustomerGl(u), repo.itemGl(u));
        pih.put("CreditAccountId", v.creditAccountId);
        int id = repo.save(pih, packing, payments, otherItems, v.head, v.details);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", id);
        out.put("message", recId == 0 ? "Save SuccessFully" : "Update SuccessFully");
        return out;
    }

    // ============================================================================ BLL ExImInvoice.MakeVoucherForExImInvoice (211)

    static final class Voucher {
        SaleInvoiceModels.VoucherHead head;
        List<SaleInvoiceModels.VoucherDetail> details = new ArrayList<>();
        int creditAccountId;
    }

    /** double.ToString() for the comment texts. */
    private static String cs(double v) { return clr(v); }

    private Voucher makeVoucher(Map<String, Object> h, List<Map<String, Object>> packing, List<Map<String, Object>> otherItems,
                                List<Map<String, Object>> supplierGl, List<Map<String, Object>> itemGl) {
        Voucher out = new Voucher();
        SaleInvoiceModels.VoucherHead vh = new SaleInvoiceModels.VoucherHead();
        int headerId = asInt(h.get("Id"));
        double rate = asDouble(h.get("ConversionRate"));
        int fcyId = asInt(h.get("FcurrencyId"));
        int customerId = asInt(h.get("SupplierCustomerId"));
        int agentId = asInt(h.get("CommissionAgentId"));
        int creditAccountId = asInt(h.get("CreditAccountId"));
        int agentGl = 0;
        vh.DocumentTypeId = DOCUMENT_TYPE_ID;
        vh.DocumentTypeSrNo = headerId;
        vh.RefDocNoId = headerId;
        vh.VoucherCode = asInt(h.get("DocCode"));
        vh.VoucherDate = ((Timestamp) h.get("DocDate")).toLocalDateTime();
        vh.Remarks = text(h.get("RemarksHeader"));
        vh.RemarksOtherLingo = "";
        vh.VoucherAmount = asDouble(h.get("EquivalentAmount"));
        vh.FcAmount = asDouble(h.get("TotalAmount"));
        vh.ExchangeCurrencyRate = rate;
        vh.MultiCurrencyId = fcyId;
        if (!supplierGl.isEmpty()) {
            Map<String, Object> c = null;
            for (Map<String, Object> r : supplierGl) if (asInt(ci(r, "Id")) == customerId) { c = r; break; }
            if (c == null) throw new IllegalStateException("Customer GLAccountId not Found");
            vh.RefAccountId = asInt(ci(c, "GlAccountId"));
            if (agentId > 0) {
                Map<String, Object> a = null;
                for (Map<String, Object> r : supplierGl) if (asInt(ci(r, "Id")) == agentId) { a = r; break; }
                if (a == null) throw new IllegalStateException("Commission Agent GLAccountId not Found");
                agentGl = asInt(ci(a, "GlAccountId"));
            }
        }
        vh.ChequeDate = LocalDate.now().atStartOfDay();
        vh.IncludeWHT = false;
        vh.BranchId = asInt(h.get("BranchesId"));
        vh.ProjectId = asInt(h.get("ProjectsId"));
        vh.BillAmount = asDouble(h.get("TotalAmount"));
        vh.ManualBillNo = text(h.get("InvoiceNo"));
        vh.DueDate = ((Timestamp) h.get("EFormDate")).toLocalDateTime();
        vh.DueDays = 0;
        vh.OrganizationId = asInt(h.get("OrganizationId"));
        vh.CompanyId = asInt(h.get("CompanyId"));
        vh.FinancialYearId = asInt(h.get("FinancialYearId"));
        vh.EntryUser = asInt(h.get("EntryUser"));
        vh.EntryDate = LocalDateTime.now();
        vh.ModifyDate = LocalDateTime.now();
        vh.ModifyUser = asInt(h.get("ModifyUser"));
        vh.ActionId = 1;
        out.head = vh;
        double sumMton = 0, sumWeight = 0;
        for (Map<String, Object> it : packing) {
            if (asInt(it.get("ActionTypeId")) == 3) continue;
            double outerQty = asDouble(it.get("OuterQty")), netWeight = asDouble(it.get("NetWeight"));
            double ratePrice = asDouble(it.get("RatePrice")), fcAmount = asDouble(it.get("FcAmount"));
            int itemId = asInt(it.get("ItemId"));
            String comments = "   NoOfBags: " + cs(outerQty) + "   M.Ton: " + cs(netWeight / 1000.0) + "  @Rate: " + cs(ratePrice) + "  FcAmount:" + cs(fcAmount);
            if (creditAccountId == 0) {
                if (itemGl.isEmpty()) throw new IllegalStateException("Item Record Not found");
                Map<String, Object> g = null;
                for (Map<String, Object> r : itemGl) if (asInt(ci(r, "Id")) == itemId) { g = r; break; }
                if (g == null) throw new IllegalStateException("Item Record Not found");
                creditAccountId = asInt(ci(g, "SaleGLAC"));
            }
            if (creditAccountId == 0) throw new IllegalStateException("CreditAccount Not Found please Check");
            double amount = fcAmount * rate;
            out.details.add(itemLine(vh.RefAccountId, creditAccountId, comments, amount, 0, itemId, outerQty, ratePrice, netWeight, fcAmount, rate, fcyId, customerId));
            sumMton += asDouble(it.get("MTon"));
            sumWeight += netWeight;
            out.details.add(itemLine(creditAccountId, vh.RefAccountId, comments, 0, amount, itemId, outerQty, ratePrice, netWeight, fcAmount, rate, fcyId, customerId));
        }
        double commAmount = asDouble(h.get("CommissionAmount"));
        int commDebit = asInt(h.get("CommissionDebitAcId"));
        if (commAmount > 0 && commDebit > 0 && agentGl > 0) {
            String text3 = "Mton  " + cs(sumMton) + "  TotalWeight " + cs(sumWeight) + "  Comm% " + cs(asDouble(h.get("CommissionPercentage")));
            double amount = commAmount * rate;
            out.details.add(commissionLine(commDebit, agentGl, text3, amount, 0, commAmount, rate, fcyId));
            out.details.add(commissionLine(agentGl, commDebit, text3, 0, amount, commAmount, rate, fcyId));
        }
        for (Map<String, Object> oi : otherItems) {
            double oAmount = asDouble(oi.get("oItemAmount"));
            if (creditAccountId == 0) {
                if (itemGl.isEmpty()) throw new IllegalStateException("OtherItem Record Not found");
                Map<String, Object> g = null;
                int byAmount = asInt(oi.get("oItemAmount"));                 /* BLL: x.Id == Conversion.ToInt(item.oItemAmount) */
                for (Map<String, Object> r : itemGl) if (asInt(ci(r, "Id")) == byAmount) { g = r; break; }
                if (g == null) throw new IllegalStateException("OtherItem Record Not found");
                creditAccountId = asInt(ci(g, "SaleGLAC"));
            }
            if (creditAccountId == 0) throw new IllegalStateException("CreditAccount Not Found against Other Item please Check");
            double amount = oAmount * rate;
            double qty = asDouble(oi.get("oItemQty"));
            SaleInvoiceModels.VoucherDetail dr = otherLine(vh.RefAccountId, creditAccountId, text(oi.get("OtherItemRemarks")), amount, 0,
                    asInt(oi.get("otherItemId")), qty, asDouble(oi.get("oItemRate")), oAmount, rate, fcyId);
            dr.DocumentTypeIdRef = 1;
            dr.RefInvoiceNo = "SALE";
            out.details.add(dr);
            out.details.add(otherLine(creditAccountId, vh.RefAccountId, text(oi.get("OtherItemRemarks")), 0, amount,
                    asInt(oi.get("otherItemId")), qty, asDouble(oi.get("oItemRate")), oAmount, rate, fcyId));
        }
        out.creditAccountId = creditAccountId;
        return out;
    }

    private static SaleInvoiceModels.VoucherDetail itemLine(int account, int against, String comments, double debit, double credit, int itemId,
                                                           double qtyOut, double rate, double weight, double fcAmount, double exRate, int fcyId, int customerId) {
        SaleInvoiceModels.VoucherDetail d = new SaleInvoiceModels.VoucherDetail();
        d.AccountId = account;
        d.AgainstAccountId = against;
        d.Comments = comments;
        d.DebitAmount = debit;
        d.CreditAmount = credit;
        d.ItemId = itemId;
        d.QtyOut = qtyOut;
        d.ItemRate = rate;
        d.WeightOut = weight;
        d.RateCut = 0;
        d.RateCutAmount = 0;
        d.ItemAmount = debit != 0 ? debit : credit;
        d.DCurrencyAmount = fcAmount;
        d.DExchangeCurrencyRate = exRate;
        d.DMultiCurrencyId = fcyId;
        d.Freight = 0; d.Expenses = 0; d.Journal = 0; d.JobLotId = 0; d.Commission = 0; d.OrderNo = 0; d.GpNo = 0;
        d.VehicleNo = "";
        d.SupplierCustomerId = customerId;
        d.DocumentTypeIdRef = 1;
        d.RefInvoiceNo = "SALE";
        return d;
    }

    private static SaleInvoiceModels.VoucherDetail commissionLine(int account, int against, String comments, double debit, double credit,
                                                                 double fcAmount, double exRate, int fcyId) {
        SaleInvoiceModels.VoucherDetail d = new SaleInvoiceModels.VoucherDetail();
        d.AccountId = account;
        d.AgainstAccountId = against;
        d.Comments = comments;
        d.DebitAmount = debit;
        d.CreditAmount = credit;
        d.ItemAmount = debit != 0 ? debit : credit;
        d.DCurrencyAmount = fcAmount;
        d.DExchangeCurrencyRate = exRate;
        d.DMultiCurrencyId = fcyId;
        d.DocumentTypeIdRef = 1;
        d.RefInvoiceNo = "SALE";
        return d;
    }

    private static SaleInvoiceModels.VoucherDetail otherLine(int account, int against, String comments, double debit, double credit, int itemId,
                                                            double qty, double rate, double fcAmount, double exRate, int fcyId) {
        SaleInvoiceModels.VoucherDetail d = new SaleInvoiceModels.VoucherDetail();
        d.AccountId = account;
        d.AgainstAccountId = against;
        d.Comments = comments;
        d.DebitAmount = debit;
        d.CreditAmount = credit;
        d.ItemId = itemId;
        d.QtyOut = qty;
        d.ItemRate = rate;
        d.WeightOut = qty;
        d.ItemAmount = debit != 0 ? debit : credit;
        d.DCurrencyAmount = fcAmount;
        d.DExchangeCurrencyRate = exRate;
        d.DMultiCurrencyId = fcyId;
        return d;
    }
}
