package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.saleinvoice.SaleInvoiceModels;
import com.mst.repositories.ExportOpeningRepository;
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
import java.util.Set;

import static com.mst.services.ExportMSupport.*;

/**
 * BLL side of 189 ExportOpening "Export Opening Balance" (Architecture.WinApp.Export.ExportOpening - the
 * "Commercial Invoice Opening" form, ExImInvoice DocumentTypeId 212) and its popup LoadSalesContractForExportOpening.
 *
 * Rights: the menu entry is ScreenDefinition 189 (View), but the form reads its rights with
 * SetRightsValueInRightsObject("EximInvoice") - ScreenName ExImInvoice, ScreenDefinition 211 "Export Commercial
 * Invoice" - so Save / Update / Print follow screen 211 (desktop quirk, kept). The form has no Delete.
 * Rows come only from "Load Sales Contract" (contracts of document types 202, 223 with balance); the detail
 * panel can only update a loaded row ("Sorry Cannot Add New Record Only Update Detail Record : Thank You").
 */
@Service
public class ExportOpeningService {

    public static final int SCREEN_ID = 189;
    public static final int RIGHTS_SCREEN_ID = 211;
    public static final int DOCUMENT_TYPE_ID = 212;

    @Autowired private ExportOpeningRepository repo;
    @Autowired private CurrentUserContext ctx;
    @Autowired private DesktopReportRights rights;

    private UserAccount viewer() {
        UserAccount u = ctx.requireAccountingUser();
        rights.require(u, SCREEN_ID, "View");
        return u;
    }
    private int fy() { return ctx.currentFinancialYearId(); }

    // ================================================================== load

    /** ImProformaInvoice_Load: ConfigureRights, ConfigureControls, BindData. */
    public Map<String, Object> setup() {
        UserAccount u = viewer();
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", right(ctx, rights, u, RIGHTS_SCREEN_ID, "Save"));
        perm.put("Update", right(ctx, rights, u, RIGHTS_SCREEN_ID, "Update"));
        perm.put("Print", right(ctx, rights, u, RIGHTS_SCREEN_ID, "Print"));
        out.put("permissions", perm);
        Map<String, Object> cfg = config(u);
        out.put("config", cfg);
        out.put("docNo", repo.generateDocNo(u, DOCUMENT_TYPE_ID, fy()));
        if (asBool(cfg.get("autoGenInvoiceNo"))) out.put("invoiceNo", repo.generateInvoiceNo(u, DOCUMENT_TYPE_ID, fy()));
        out.putAll(combos(u));
        out.put("branches", idName(repo.branches(u), "Id", "BranchName"));
        out.put("projects", idName(repo.projects(u), "Id", "ProjectName"));
        out.put("historyCustomers", idName(repo.historyCustomers(u, "212"), "Id", "name"));
        if (asBool(cfg.get("allowExportMultiCompanies"))) out.put("exportCompanies", idName(repo.exportCompanies(u), "Id", "CompName"));
        out.put("documentTypeId", DOCUMENT_TYPE_ID);
        return out;
    }

    private Map<String, Object> config(UserAccount u) {
        Map<String, Object> c = new LinkedHashMap<>();
        c.put("defaultDaysToLessFromHistoryFromDate", asInt(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        c.put("flagFin", asBool(repo.config(u, "FinancialisActiveonCommercialInvoice")));
        c.put("autoGenInvoiceNo", asBool(repo.config(u, "CommercialInvoicePrefix")));
        c.put("rateAddLessOnCommercialInvoice", asBool(repo.config(u, "RateAddLessOnCommercialInvoice")));
        c.put("allowExportMultiCompanies", repo.erpFeature(u, 9));
        return c;
    }

    /** BindData / btnRefresh_Click combos. */
    private Map<String, Object> combos(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("creditAccounts", idName(repo.creditAccounts(u), "Id", "AccountTitle"));
        out.put("customers", idName(repo.exportContractDropDown(u, "Customer"), "Id", "name"));
        out.put("notifyParties", idName(repo.exportCustomers(u), "Id", "CompanyName"));
        List<Map<String, Object>> dts = new ArrayList<>();
        for (Map<String, Object> r : repo.deliveryTerms()) dts.add(row3(r, "Id", "Code", "Description"));
        out.put("deliveryTerms", dts);
        List<Map<String, Object>> pts = new ArrayList<>();
        for (Map<String, Object> r : repo.paymentTerms(u)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("lcOrderTerm", text(ci(r, "lcOrderTerm")));
            m.put("Description", text(ci(r, "lcOrderTermDesc")));
            pts.add(m);
        }
        out.put("paymentTerms", pts);
        List<Map<String, Object>> loading = new ArrayList<>(), destination = new ArrayList<>();
        for (Map<String, Object> r : repo.seaPorts(u)) {
            String t = text(ci(r, "PortType"));
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("PortName", text(ci(r, "PortName")));
            if ("Loading".equals(t)) loading.add(m); else if ("Destination".equals(t)) destination.add(m);
        }
        out.put("loadingPorts", loading);
        out.put("destinationPorts", destination);
        List<Map<String, Object>> home = new ArrayList<>(), foreign = new ArrayList<>();
        for (Map<String, Object> r : repo.banks(u)) {
            String t = text(ci(r, "IsHomeland"));
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("BranchName", text(ci(r, "BranchName")));
            if ("Home Country".equals(t)) home.add(m);
            if ("Foreign Country".equals(t)) foreign.add(m);
        }
        out.put("exporterBanks", home);
        out.put("importerBanks", foreign);
        out.put("currencies", idName(repo.currencies(u), "Id", "CurrencyName"));
        out.put("cropYears", idName(repo.cropYears(u), "Id", "CropYear"));
        out.put("jobLots", idName(repo.jobLots(u), "Id", "JobLotDescription"));
        out.put("packTypes", idName(repo.packTypes(u), "Id", "Description"));
        List<Map<String, Object>> fnt = new ArrayList<>();
        for (Map<String, Object> r : repo.farmingNTrade()) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("FarmingNTrade", text(ci(r, "FarmingNTrade")));
            m.put("ExImFarmingTypeId", asInt(ci(r, "ExImFarmingTypeId")));
            m.put("ExImTradeTypeId", asInt(ci(r, "ExImTradeTypeId")));
            fnt.add(m);
        }
        out.put("farmingNTrade", fnt);
        return out;
    }

    private static Map<String, Object> row3(Map<String, Object> r, String a, String b, String c) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put(a, asInt(ci(r, a)));
        m.put(b, text(ci(r, b)));
        m.put(c, text(ci(r, c)));
        return m;
    }

    static List<Map<String, Object>> idName(List<Map<String, Object>> rows, String idCol, String nameCol) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, idCol)));
            m.put(nameCol, text(ci(r, nameCol)));
            out.add(m);
        }
        return out;
    }

    /** btnRefresh_Click: the combos again, CommercialInvoicePrefix re-read and the invoice no regenerated when it is on. */
    public Map<String, Object> refresh() {
        UserAccount u = viewer();
        Map<String, Object> out = new LinkedHashMap<>(combos(u));
        boolean auto = asBool(repo.config(u, "CommercialInvoicePrefix"));
        out.put("autoGenInvoiceNo", auto);
        if (auto) out.put("invoiceNo", repo.generateInvoiceNo(u, DOCUMENT_TYPE_ID, fy()));
        return out;
    }

    /** FormReset: generateCode + GenerateInvoiceNo (when AutoGenInvoiceNo). */
    public Map<String, Object> reset() {
        UserAccount u = viewer();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("docNo", repo.generateDocNo(u, DOCUMENT_TYPE_ID, fy()));
        boolean auto = asBool(repo.config(u, "CommercialInvoicePrefix"));
        out.put("autoGenInvoiceNo", auto);
        if (auto) out.put("invoiceNo", repo.generateInvoiceNo(u, DOCUMENT_TYPE_ID, fy()));
        return out;
    }

    /** cmbSupCust_Leave: BindFinancialInstrument + bindConsigneeAgainstCustomer (+ HSCodeBind with the current item). */
    public Map<String, Object> customer(int supplierCustomerId, int itemId) {
        UserAccount u = viewer();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> fis = new ArrayList<>();
        if (supplierCustomerId > 0) {
            for (Map<String, Object> r : repo.fiNosForInvoice(u, supplierCustomerId)) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", asInt(ci(r, "Id")));
                m.put("EFormNo", text(ci(r, "EFormNo")));
                m.put("PaymenttermId", asInt(ci(r, "PaymenttermId")));
                m.put("DocumnetTypeId", asInt(ci(r, "DocumentTypeId")));
                fis.add(m);
            }
        }
        out.put("fis", fis);
        List<Map<String, Object>> cons = new ArrayList<>();
        if (supplierCustomerId > 0) cons = idName(repo.consignees(u, supplierCustomerId), "Id", "CompanyName");
        out.put("consignees", cons);
        out.put("hsCodes", hsCodes(u, itemId, supplierCustomerId));
        return out;
    }

    /** HSCodeBind: CommodityDetailItemCustomerWise.GetRemarks(ItemId, SupplierCustomerId) - Id, HSCode. */
    private List<Map<String, Object>> hsCodes(UserAccount u, int itemId, int supplierCustomerId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.commodityRemarks(u, itemId, supplierCustomerId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("HSCode", text(ci(r, "HSCode")));
            out.add(m);
        }
        return out;
    }

    /** combitem_Leave: bindRateUomAndItemPackUom (GetUomScheduleByItemId) + HSCodeBind. */
    public Map<String, Object> item(int itemId, int supplierCustomerId) {
        UserAccount u = viewer();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> uoms = new ArrayList<>();
        for (Map<String, Object> r : repo.uomSchedule(u, itemId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("UOMCode", text(ci(r, "UOMCode")));
            m.put("Equivalent", asDouble(ci(r, "Equivalent")));
            uoms.add(m);
        }
        out.put("uoms", uoms);
        out.put("hsCodes", hsCodes(u, itemId, supplierCustomerId));
        return out;
    }

    /** GetFinancialInstrumentsBalance - Balance.ToString("0,0"). */
    public Map<String, Object> fiBalance(int documentTypeId, int id) {
        UserAccount u = viewer();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("balance", asDouble(repo.fiBalance(u, documentTypeId, id)));
        return out;
    }

    // ================================================================== history

    public List<Map<String, Object>> historyCustomers() { return idName(repo.historyCustomers(viewer(), "212"), "Id", "name"); }

    /** HistoryGridFill: ExImInvoice.FormHistory with Ids "212", ApprovedFilter "All"; the 18 columns of the desktop DataTable. */
    public List<Map<String, Object>> history(Map<String, Object> b) {
        UserAccount u = viewer();
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("FinancialYearId", fy());
        String kind = text(b.get("dateKind"));
        java.sql.Date from = asBool(b.get("fromChecked")) ? sqlDate(b.get("fromDate")) : null;
        java.sql.Date to = asBool(b.get("toChecked")) ? sqlDate(b.get("toDate")) : null;
        String fk = "FromDate", tk = "ToDate";
        if ("entry".equals(kind)) { fk = "EntryFromDate"; tk = "EntryToDate"; }
        else if ("modify".equals(kind)) { fk = "ModifyFromDate"; tk = "ModifyToDate"; }
        else if ("approved".equals(kind)) { fk = "ApprovedFromDate"; tk = "ApprovedToDate"; }
        if (from != null) p.put(fk, from);
        if (to != null) p.put(tk, to);
        int cust = asInt(b.get("supplierCustomerId"));
        if (cust != 0) p.put("SupplierCustomerId", cust);
        p.put("DocumentTypeIds", "212");
        int fromNo = asInt(b.get("fromDocNo")), toNo = asInt(b.get("toDocNo"));
        if (fromNo != 0) p.put("DocNoFrom", fromNo);
        if (toNo != 0) p.put("DocNoTo", toNo);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.formHistory(p)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("DocCode", asInt(ci(r, "DocCode")));
            m.put("DocDate", iso(ci(r, "DocDate")));
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
            m.put("EntryDate", isoDateTime(ci(r, "EntryDate")));
            m.put("ModifyUser", text(ci(r, "ModifyUserName")));
            m.put("ModifyDate", isoDateTime(ci(r, "ModifyDate")));
            m.put("NoOfAttachments", text(ci(r, "NoOfAttachments")));
            out.add(m);
        }
        return out;
    }

    // ================================================================== read

    /** ExImInvoice.GetByID (ReadById + packing / payment terms / other items) for ReadById and GetDetailByHeaderId. */
    public Map<String, Object> byId(int id) {
        UserAccount u = viewer();
        List<Map<String, Object>> hs = repo.headerById(id);
        if (hs.isEmpty()) throw new IllegalStateException("Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index");
        Map<String, Object> h = ExportReturnInvoiceService.plain(hs.get(0));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", h);
        List<Map<String, Object>> det = new ArrayList<>();
        for (Map<String, Object> r : repo.child(id, "ReadExImInvoicePackingDetailByHeaderId")) det.add(ExportReturnInvoiceService.plain(r));
        out.put("detail", det);
        List<Map<String, Object>> pay = new ArrayList<>();
        for (Map<String, Object> r : repo.child(id, "ReadExImInvoicePaymentTermsDetailByHeaderId")) pay.add(ExportReturnInvoiceService.plain(r));
        out.put("payments", pay);
        List<Map<String, Object>> oi = new ArrayList<>();
        for (Map<String, Object> r : repo.child(id, "ReadExImInvoiceOtherItemsByHeaderId")) oi.add(ExportReturnInvoiceService.plain(r));
        out.put("otherItems", oi);
        /* ReadById ends with BindFinancialInstrument() for the loaded customer. */
        out.put("customer", customer(asInt(ci(hs.get(0), "SupplierCustomerId")), 0));
        return out;
    }

    // ================================================================== loader (LoadSalesContractForExportOpening)

    public Map<String, Object> loaderSetup() {
        UserAccount u = viewer();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("fromDate", iso(repo.financialYearStart(fy())));
        out.put("parties", idName(repo.exportContractDropDown(u, "Customer"), "Id", "name"));
        out.put("items", idName(repo.exportContractDropDown(u, "Items"), "Id", "name"));
        out.put("currencies", idName(repo.exportContractDropDown(u, "Currency"), "Id", "name"));
        return out;
    }

    /** ExportInvoicesLoad: GetProformaDataForPreInvoices(DocumentTypeIds "202,223", ZeroBalanceType 1). */
    public List<Map<String, Object>> loaderRows(Map<String, Object> b) {
        UserAccount u = viewer();
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("BranchesId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        p.put("FinancialYearId", fy());
        p.put("DocumentTypeIds", "202,223");
        java.sql.Date from = sqlDate(b.get("fromDate")), to = sqlDate(b.get("toDate"));
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        int cust = asInt(b.get("supplierCustomerId")), fcy = asInt(b.get("fcyId")), item = asInt(b.get("itemId"));
        if (cust != 0) p.put("SupplierCustomerId", cust);
        if (fcy != 0) p.put("FcurrencyId", fcy);
        if (item != 0) p.put("ItemId", item);
        p.put("SkipZero", 1);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.proformaDataForPreInvoices(p)) out.add(ExportReturnInvoiceService.plain(r));
        return out;
    }

    /** BindLoaderData: other items and payment terms of the loaded contracts. */
    public Map<String, Object> loaderBind(String ids) {
        UserAccount u = viewer();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> oi = new ArrayList<>();
        for (Map<String, Object> r : repo.otherItemsFromLcOrder(u, ids)) oi.add(ExportReturnInvoiceService.plain(r));
        out.put("otherItems", oi);
        List<Map<String, Object>> pt = new ArrayList<>();
        for (Map<String, Object> r : repo.lcOrderPaymentTerms(ids)) pt.add(ExportReturnInvoiceService.plain(r));
        out.put("paymentTerms", pt);
        return out;
    }

    // ================================================================== save

    /**
     * btnsave_Click (also btnupdate_Click): formvalidation, the per-row checks with "... required in Row#n", the
     * contract id / no / date strings, the payment-total check, then BLL ExImInvoice.Save (DocumentTypeId 212 always
     * builds MakeVoucherForExImInvoice) -> DAL ExImInvoice.SetData.
     */
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = viewer();
        int recId = asInt(b.get("recId"));
        if (!right(ctx, rights, u, RIGHTS_SCREEN_ID, recId > 0 ? "Update" : "Save"))
            throw new AccessDeniedException("The user does not have " + (recId > 0 ? "Update" : "Save") + " rights for this screen");
        Map<String, Object> h = map(b.get("header"));
        List<Map<String, Object>> rows = list(b.get("rows"));
        List<Map<String, Object>> removed = list(b.get("removed"));
        List<Map<String, Object>> others = list(b.get("otherItems"));
        List<Map<String, Object>> pays = list(b.get("payments"));
        boolean multi = repo.erpFeature(u, 9);

        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record not found");
        /* formvalidation */
        if (multi && asInt(h.get("ExportCompanyId")) == 0) throw new IllegalArgumentException("Export Company Is required");
        if (asInt(h.get("DocCode")) == 0) throw new IllegalArgumentException("Doc No Is Required");
        if (text(h.get("InvoiceNo")).isEmpty()) throw new IllegalArgumentException("Invoice No Is Required");
        if (asInt(h.get("SupplierCustomerId")) == 0) throw new IllegalArgumentException("Customer Is Required");
        if (asInt(h.get("PaymentTermId")) == 0) throw new IllegalArgumentException("Payment Term Is Required");
        if (asInt(h.get("DeliveryTermId")) == 0) throw new IllegalArgumentException("Delivery Term Is Required");
        if (asInt(h.get("LoadingPortId")) == 0) throw new IllegalArgumentException("Loading Port Is Required");
        if (asInt(h.get("DestinationPortId")) == 0) throw new IllegalArgumentException("Destination Port  Is Required");
        if (trim(h.get("CarierType")).isEmpty()) throw new IllegalArgumentException("Carrier Type  Is Required");
        if (asInt(h.get("FcurrencyId")) == 0) throw new IllegalArgumentException("Fcy Code Is Required");
        if (asDouble(h.get("FCurrencyAmount")) == 0) throw new IllegalArgumentException("Fcy Amount  Is Required");
        if (asInt(h.get("NoOfContainers")) == 0) throw new IllegalArgumentException("No.of Containers Field Is Required");
        if (!(asDouble(h.get("NetMton")) > 0)) throw new IllegalArgumentException("Net Weight  Is Required");
        if (!(asDouble(h.get("GrossMton")) >= asDouble(h.get("NetMton")))) throw new IllegalArgumentException("Gross Weight Must Be Equal Or Greater Than Net Weight Thank You");
        if (!(asDouble(h.get("ConversionRate")) > 0)) throw new IllegalArgumentException("ExchangeRate Field is Required");
        if (!(asDouble(h.get("TotalAmount")) > 0)) throw new IllegalArgumentException("Total Amount  Is Required");
        if (asInt(h.get("CreditAccountId")) == 0) throw new IllegalArgumentException("Please select CreditAc");

        int fy = fy();
        Timestamp now = now();
        Map<String, Object> p = ExportOpeningRepository.blankHeader();
        p.put("Id", recId > 0 ? recId : 0);
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("BranchesId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        p.put("IsApproved", false);
        p.put("EntryDate", now);
        p.put("EntryUser", u.getId());
        p.put("ModifyDate", now);
        p.put("ModifyUser", u.getId());
        p.put("ApprovedDate", now);
        p.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        p.put("FinancialYearId", fy);
        p.put("ProjectsId", asInt(h.get("ProjectsId")));
        p.put("DocCode", asInt(h.get("DocCode")));
        p.put("DocDate", pickerValue(h.get("DocDate")));
        p.put("InvoiceNo", trim(h.get("InvoiceNo")));
        p.put("SupplierCustomerId", asInt(h.get("SupplierCustomerId")));
        p.put("ConsigneeId", asInt(h.get("ConsigneeId")));
        p.put("NotifyParty1", asInt(h.get("NotifyParty1")));
        p.put("NotifyParty2", asInt(h.get("NotifyParty2")));
        p.put("LotNoRef", text(h.get("LotNoRef")));
        p.put("EFormNo", trim(h.get("EFormNo")));
        p.put("EFormDate", pickerValue(h.get("EFormDate")));
        p.put("FcurrencyId", asInt(h.get("FcurrencyId")));
        p.put("ConversionRate", asDouble(h.get("ConversionRate")));
        p.put("FCurrencyAmount", asDouble(h.get("FCurrencyAmount")));
        p.put("EquivalentAmount", asDouble(h.get("EquivalentAmount")));
        p.put("AddLessAmount", asDouble(h.get("AddLessAmount")));
        p.put("TotalAmount", asDouble(h.get("TotalAmount")));
        p.put("AddLessComments", text(h.get("AddLessComments")));
        p.put("PaymentTermId", asInt(h.get("PaymentTermId")));
        p.put("DeliveryTermId", asInt(h.get("DeliveryTermId")));
        p.put("PaymentRemarks", text(h.get("PaymentRemarks")));
        p.put("DeliveryRemarks", text(h.get("DeliveryRemarks")));
        p.put("ImporterBankId", asInt(h.get("ImporterBankId")));
        p.put("ExporteBankId", asInt(h.get("ExporteBankId")));
        p.put("LoadingPortId", asInt(h.get("LoadingPortId")));
        p.put("DestinationPortId", asInt(h.get("DestinationPortId")));
        p.put("CarierType", text(h.get("CarierType")));
        BigDecimal grossMton = dec(h.get("GrossMton")), netMton = dec(h.get("NetMton"));
        p.put("GrossMton", grossMton);
        p.put("NetMton", netMton);
        p.put("GrossWeight", grossMton.multiply(BigDecimal.valueOf(1000)).doubleValue());
        p.put("NetWeight", netMton.multiply(BigDecimal.valueOf(1000)).doubleValue());
        p.put("ExportCompanyId", asInt(h.get("ExportCompanyId")));
        p.put("NoOfContainers", (double) asInt(h.get("NoOfContainers")));
        p.put("Certificate1", trim(h.get("Certificate1")));
        p.put("Certificate2", trim(h.get("Certificate2")));
        p.put("CreditAccountId", asInt(h.get("CreditAccountId")));

        List<Map<String, Object>> packing = new ArrayList<>();
        if (recId > 0) for (Map<String, Object> r : removed) packing.add(removedModel(r));
        double totalFc = 0;
        List<String> cIds = new ArrayList<>(), cdIds = new ArrayList<>(), cNos = new ArrayList<>(), cDates = new ArrayList<>();
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> r = rows.get(i);
            String n = String.valueOf(i + 1);
            Map<String, Object> vd = ExportOpeningRepository.blankPacking();
            vd.put("Id", asInt(r.get("Id")));
            vd.put("ContractId", asInt(r.get("ContractId")));
            if (asInt(vd.get("ContractId")) == 0) throw new IllegalArgumentException("ContractId required in Row#" + n);
            vd.put("ContractDetailId", asInt(r.get("ContractDetailId")));
            cIds.add(text(r.get("ContractId")));
            cdIds.add(text(r.get("ContractDetailId")));
            cNos.add(text(r.get("ContractNo")));
            cDates.add(shortDate(r.get("ContractDate")));
            if (asInt(vd.get("ContractDetailId")) == 0) throw new IllegalArgumentException("ContractDetailId required in Row#" + n);
            vd.put("ItemId", asInt(r.get("ItemId")));
            if (asInt(vd.get("ItemId")) == 0) throw new IllegalArgumentException("Item required in Row#" + n);
            vd.put("CropYearId", asInt(r.get("CropYearId")));
            if (asInt(vd.get("CropYearId")) == 0) throw new IllegalArgumentException("Crop Year required in Row#" + n);
            vd.put("JobLotId", asInt(r.get("JobLotId")));
            if (asInt(vd.get("JobLotId")) == 0) throw new IllegalArgumentException("Job Lot required in Row#" + n);
            vd.put("PackingMaterialTypeId", asInt(r.get("PackTypeId")));
            if (asInt(vd.get("PackingMaterialTypeId")) == 0) throw new IllegalArgumentException("Packing Type required in Row#" + n);
            double mton = asDouble(r.get("QtyMTon"));
            vd.put("MTon", mton);
            if (mton == 0) throw new IllegalArgumentException("Mton required in Row#" + n);
            vd.put("NetWeight", mton * 1000.0);
            vd.put("OuterQtyUomId", asInt(r.get("PackSizeId")));
            if (asInt(vd.get("OuterQtyUomId")) == 0) throw new IllegalArgumentException("Pack Size required in Row#" + n);
            vd.put("OuterQty", asDouble(r.get("NoOfBags")));
            if (asDouble(r.get("NoOfBags")) == 0) throw new IllegalArgumentException("No. Of Bags required in Row#" + n);
            vd.put("InnerQtyUomId", asInt(r.get("PackSizeId")));
            vd.put("InnerQty", (double) asInt(r.get("NoOfBags")));
            vd.put("RateWithoutAddLess", asDouble(r.get("RateWithoutAddLess")));
            vd.put("RateAddLess", asDouble(r.get("RateAddLess")));
            vd.put("RatePrice", asDouble(r.get("CostMTon")));
            if (asDouble(r.get("CostMTon")) == 0) throw new IllegalArgumentException("Cost/M.Ton required in Row#" + n);
            vd.put("RateUomId", asInt(r.get("RateUOMId")));
            if (asInt(vd.get("RateUomId")) == 0) throw new IllegalArgumentException("Rate Uom required in Row#" + n);
            double fc = asDouble(r.get("Amount"));
            vd.put("FcAmount", fc);
            totalFc += fc;
            vd.put("HsCode", text(r.get("HSCode")));
            vd.put("ItemDescriptionManual", text(r.get("ItemDetail")));
            vd.put("OuterPackDescription", text(r.get("PackingDetail")));
            vd.put("ExImFarmingNTradeId", asInt(r.get("ExImFarmingNTradeId")));
            vd.put("ExImTradeTypeId", asInt(r.get("ExImTradeTypeId")));
            vd.put("ExImFarmingTypeId", asInt(r.get("ExImFarmingTypeId")));
            vd.put("ActionTypeId", asInt(vd.get("Id")) <= 0 ? 1 : 2);
            packing.add(vd);
        }
        p.put("ContractIds", distinct(cIds));
        p.put("ContractDetailIds", distinct(cdIds));
        p.put("ContractNos", distinct(cNos));
        p.put("ContractDates", distinct(cDates));

        double otherAmount = 0;
        List<Map<String, Object>> otherItems = new ArrayList<>();
        for (Map<String, Object> r : others) {
            if (dec(r.get("Qty")).signum() <= 0) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", 0);
            o.put("ExImInvoiceId", 0);
            o.put("otherItemId", asInt(r.get("ItemId")));
            o.put("oItemQty", dec(r.get("Qty")));
            o.put("oItemWeightKgs", BigDecimal.ZERO);
            o.put("oItemRate", asDouble(r.get("Rate")));
            o.put("oItemAmount", dec(r.get("Amount")));
            otherAmount += dec(r.get("Amount")).doubleValue();
            o.put("OtherItemRemarks", text(r.get("Remarks")));
            otherItems.add(o);
        }
        if (pays.isEmpty()) throw new IllegalArgumentException("Payment Detail Not Found. Please Check! ");
        double utilize = 0;
        List<Map<String, Object>> paymentTerms = new ArrayList<>();
        for (int i = 0; i < pays.size(); i++) {
            Map<String, Object> r = pays.get(i);
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("Id", asInt(r.get("Id")));
            d.put("ExImInvoiceId", 0);
            d.put("DocumentTypeId", asInt(r.get("DocumentTypeId")));
            d.put("ExImEFormRegistrationId", asInt(r.get("ExImEFormRegistrationId")));
            d.put("FinancialInstrumentNo", text(r.get("FinancialInstrumentNo")));
            d.put("PaymentTermId", asInt(r.get("PaymentTermId")));
            d.put("PrcntOfTotal", asDouble(r.get("PrcntOfTotal")));
            d.put("FcyAmount", asDouble(r.get("FcyAmount")));
            d.put("DueDays", asInt(r.get("DueDays")));
            d.put("SortNo", i + 1);
            d.put("PaymentRemarks", text(r.get("Remarks")));
            d.put("EntryUserId", 0);
            d.put("ModifyUserId", 0);
            d.put("RefDocumentTypeId", 0);
            utilize += asDouble(r.get("FcyAmount"));
            paymentTerms.add(d);
        }
        double total = totalFc + asDouble(p.get("AddLessAmount")) + otherAmount;
        p.put("TotalAmount", total);
        if (total != utilize) throw new IllegalArgumentException("Total Invoice Amount and Total Payment Utilize Amount Not equal Please Check");

        SaleInvoiceModels.VoucherHead vh = new SaleInvoiceModels.VoucherHead();
        List<SaleInvoiceModels.VoucherDetail> vds = makeVoucher(u, p, otherItems, vh);
        p.put("ActionId", recId == 0 ? 1 : 2);
        int id = repo.save(p, packing, paymentTerms, otherItems, vh, vds);
        Map<String, Object> out = ok(recId == 0 ? "Save SuccessFully" : "Update SuccessFully");
        out.put("id", id);
        return out;
    }

    /** "a,b,a" -> distinct values in first-seen order (Split(',').Distinct() then TrimStart(',')). */
    private static String distinct(List<String> vals) {
        Set<String> s = new LinkedHashSet<>(vals);
        return String.join(",", s);
    }

    /** DateTime.ToShortDateString (en-US M/d/yyyy). */
    private static String shortDate(Object v) {
        LocalDate d = asDate(v);
        if (d == null) d = LocalDate.of(1, 1, 1);
        return d.getMonthValue() + "/" + d.getDayOfMonth() + "/" + d.getYear();
    }

    /** grdDetail_ColumnButtonClick "Delete" on a saved row - the ExImInvoicePackingDetail kept with ActionTypeId 3. */
    private static Map<String, Object> removedModel(Map<String, Object> r) {
        Map<String, Object> vd = ExportOpeningRepository.blankPacking();
        vd.put("Id", asInt(r.get("Id")));
        vd.put("ContractId", asInt(r.get("ContractId")));
        vd.put("ContractDetailId", asInt(r.get("ContractDetailId")));
        vd.put("ItemId", asInt(r.get("ItemId")));
        vd.put("CropYearId", asInt(r.get("CropYearId")));
        vd.put("JobLotId", asInt(r.get("JobLotId")));
        vd.put("PackingMaterialTypeId", asInt(r.get("PackTypeId")));
        vd.put("MTon", asDouble(r.get("QtyMTon")));
        vd.put("NetWeight", asDouble(r.get("QtyMTon")) * 1000.0);
        vd.put("OuterQtyUomId", asInt(r.get("PackSizeId")));
        vd.put("OuterQty", asDouble(r.get("NoOfBags")));
        vd.put("InnerQtyUomId", asInt(r.get("PackSizeId")));
        vd.put("InnerQty", (double) asInt(r.get("NoOfBags")));
        vd.put("RatePrice", asDouble(r.get("CostMTon")));
        vd.put("RateUomId", asInt(r.get("RateUOMId")));
        vd.put("FcAmount", asDouble(r.get("Amount")));
        vd.put("HsCode", text(r.get("HSCode")));
        vd.put("ItemDescriptionManual", text(r.get("ItemDetail")));
        vd.put("ExImFarmingNTradeId", asInt(r.get("ExImFarmingNTradeId")));
        vd.put("ExImTradeTypeId", asInt(r.get("ExImTradeTypeId")));
        vd.put("ExImFarmingTypeId", asInt(r.get("ExImFarmingTypeId")));
        vd.put("OuterPackDescription", text(r.get("PackingDetail")));
        vd.put("ActionTypeId", 3);
        return vd;
    }

    /**
     * BLL MakeVoucherForExImInvoice for DocumentTypeId 212: no per-row lines; one customer / credit-account pair for
     * TotalAmount x ConversionRate, then one pair per other item (CreditAccountId is required by the form, so the
     * item-GL fallback never runs). The commission pair needs CommissionAmount, which this form never sets.
     */
    private List<SaleInvoiceModels.VoucherDetail> makeVoucher(UserAccount u, Map<String, Object> h, List<Map<String, Object>> otherItems,
                                                              SaleInvoiceModels.VoucherHead vh) {
        double rate = asDouble(h.get("ConversionRate"));
        int fcyId = asInt(h.get("FcurrencyId"));
        int customerId = asInt(h.get("SupplierCustomerId"));
        int creditAccountId = asInt(h.get("CreditAccountId"));
        vh.DocumentTypeId = DOCUMENT_TYPE_ID;
        vh.DocumentTypeSrNo = asInt(h.get("Id"));
        vh.RefDocNoId = asInt(h.get("Id"));
        vh.VoucherCode = asInt(h.get("DocCode"));
        vh.VoucherDate = ((Timestamp) h.get("DocDate")).toLocalDateTime();
        vh.Remarks = "";                                                    // Conversion.ToString(obj.RemarksHeader) - never set by this form
        vh.RemarksOtherLingo = "";
        vh.VoucherAmount = asDouble(h.get("EquivalentAmount"));
        vh.FcAmount = asDouble(h.get("TotalAmount"));
        vh.ExchangeCurrencyRate = rate;
        vh.MultiCurrencyId = fcyId;
        List<Map<String, Object>> supplierGl = repo.supplierCustomerGl(u);
        if (!supplierGl.isEmpty()) {
            Map<String, Object> c = null;
            for (Map<String, Object> r : supplierGl) if (asInt(ci(r, "Id")) == customerId) { c = r; break; }
            if (c == null) throw new IllegalStateException("Customer GLAccountId not Found");
            vh.RefAccountId = asInt(ci(c, "GlAccountId"));
        }
        List<Map<String, Object>> itemGl = repo.itemGl(u);
        vh.ChequeDate = LocalDate.now().atStartOfDay();
        vh.IncludeWHT = false;
        vh.BranchId = asInt(h.get("BranchesId"));
        vh.ProjectId = asInt(h.get("ProjectsId"));
        vh.BillAmount = asDouble(h.get("TotalAmount"));
        vh.ManualBillNo = text(h.get("InvoiceNo"));
        vh.DueDate = ((Timestamp) h.get("EFormDate")).toLocalDateTime();
        vh.DueDays = 0;
        vh.OrganizationId = u.getOrganizationId();
        vh.CompanyId = u.getCompanyId();
        vh.FinancialYearId = asInt(h.get("FinancialYearId"));
        vh.EntryUser = asInt(h.get("EntryUser"));
        vh.EntryDate = LocalDateTime.now().withNano(0);
        vh.ModifyDate = LocalDateTime.now().withNano(0);
        vh.ModifyUser = asInt(h.get("ModifyUser"));
        vh.ActionId = 1;

        List<SaleInvoiceModels.VoucherDetail> out = new ArrayList<>();
        double netWeight = asDouble(h.get("NetWeight")), totalAmount = asDouble(h.get("TotalAmount"));
        String text2 = " M.Ton: " + num(netWeight / 1000.0) + " NetWeight: " + num(netWeight) + " FcAmount: " + num(h.get("FCurrencyAmount"))
                + " AddLess: " + num(h.get("AddLessAmount")) + " TotalFcyAmount: " + num(totalAmount);
        if (creditAccountId == 0) throw new IllegalStateException("CreditAccount Not Found please Check");
        double num6 = totalAmount * rate;
        out.add(line(vh.RefAccountId, creditAccountId, text2, num6, 0, netWeight, totalAmount, rate, fcyId, customerId));
        out.add(line(creditAccountId, vh.RefAccountId, text2, 0, num6, netWeight, totalAmount, rate, fcyId, customerId));
        for (Map<String, Object> oi : otherItems) {
            double oAmount = asDouble(oi.get("oItemAmount"));
            if (creditAccountId == 0) {
                if (itemGl.isEmpty()) throw new IllegalStateException("OtherItem Record Not found");
                Map<String, Object> g = null;
                for (Map<String, Object> r : itemGl) if (asInt(ci(r, "Id")) == (int) oAmount) { g = r; break; }   // BLL looks up by oItemAmount (sic)
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
            out.add(dr);
            out.add(otherLine(creditAccountId, vh.RefAccountId, text(oi.get("OtherItemRemarks")), 0, amount,
                    asInt(oi.get("otherItemId")), qty, asDouble(oi.get("oItemRate")), oAmount, rate, fcyId));
        }
        return out;
    }

    private static SaleInvoiceModels.VoucherDetail line(int account, int against, String comments, double debit, double credit,
                                                       double netWeight, double totalAmount, double rate, int fcyId, int customerId) {
        SaleInvoiceModels.VoucherDetail d = new SaleInvoiceModels.VoucherDetail();
        d.AccountId = account;
        d.AgainstAccountId = against;
        d.Comments = comments;
        d.DebitAmount = debit;
        d.CreditAmount = credit;
        d.QtyOut = netWeight / 1000.0;
        d.WeightOut = netWeight;
        d.ItemAmount = debit != 0 ? debit : credit;
        d.DCurrencyAmount = totalAmount;
        d.DExchangeCurrencyRate = rate;
        d.DMultiCurrencyId = fcyId;
        d.SupplierCustomerId = customerId;
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
