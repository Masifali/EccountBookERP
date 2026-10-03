package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportCommercialInvoiceRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.ExportCommercialInvoiceSupport.*;

/**
 * BLL side of screen 211 "Export Commercial Invoice" - Architecture.WinApp.Export.ExImCommercialInvoice
 * (ClientSize 1184 x 711, form Text "Commercial Invoice", DocumentTypeId 204, rights name "EximInvoice").
 *
 * tabControl1 Form | History. Form: toolStrip4 New / Save / Update(hidden) / Refresh / Attachment / Print-521 /
 * Print-529 / Load Sales Contract / Generate Invoice Nos(hidden) / ItemDetail_LookUp / ShortCut Keys /
 * Contract Print-501B / Export Charges + "Preview"; HeaderGropBox "Main Header" (1158 x 273); tabControl2 Invoice
 * Detail | Other Items | Payment Detail | Commission Info (ERP feature 15) | Other Charges.
 *
 * Detail rows come only from "Load Sales Contract" (LoadSalesContractForInvoice, DocumentTypeId 202); the "+" of the
 * detail panel only answers "Sorry Cannot Add New Record Only Update Detail Record : Thank You".
 * Rights by ScreenDefinition.Id 211: View, Save (new), Update (RecId > 0), Print. Tenancy, branch and financial year
 * come from the session. Attachments (Attachment / AddAttachment) are not part of this web port.
 */
@Service
public class ExportCommercialInvoiceService {

    public static final int SCREEN_ID = 211;
    public static final int DOCUMENT_TYPE_ID = 204;
    /** BtnLoad_Click: Loader.DocumentTypeId = 202. */
    public static final int LOADER_DOCUMENT_TYPE_ID = 202;

    @Autowired private ExportCommercialInvoiceRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights rights;

    private UserAccount user(String action) {
        UserAccount u = currentUserContext.requireAccountingUser();
        rights.require(u, SCREEN_ID, action);
        return u;
    }

    private boolean allowed(UserAccount u, String action) {
        try { rights.require(u, SCREEN_ID, action); return true; } catch (AccessDeniedException e) { return false; }
    }

    private int fy() { try { return currentUserContext.currentFinancialYearId(); } catch (RuntimeException e) { return 0; } }

    private interface Loader { Object load() throws Exception; }
    private static void put(Map<String, Object> out, String key, Loader l) {
        try { out.put(key, l.load()); } catch (Exception e) { out.put(key, new ArrayList<>()); out.put(key + "Error", msg(e)); }
    }

    // ================================================================== load

    /** ImProformaInvoice_Load: ConfigureRights + ConfigureControls + BindData. */
    public Map<String, Object> setup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, "Save"));
        perm.put("Update", allowed(u, "Update"));
        perm.put("Print", allowed(u, "Print"));
        out.put("permissions", perm);
        out.put("config", config(u));
        boolean f9 = repo.erpFeature(u, 9), f15 = repo.erpFeature(u, 15);
        out.put("allowExportMultiCompanies", f9);
        out.put("saleCommissionTabFeatureWise", f15);
        put(out, "projects", () -> idName(repo.projects(u), "Id", "ProjectName"));
        put(out, "docNo", () -> repo.generateDocNo(u, DOCUMENT_TYPE_ID, fy()));
        put(out, "invoiceNo", () -> asBool(repo.config(u, "CommercialInvoicePrefix")) ? repo.generateInvoiceNo(u, DOCUMENT_TYPE_ID, fy()) : "");
        out.putAll(combos(u, f9, f15));
        put(out, "historyCombos", () -> historyCombos(u));
        return out;
    }

    /** DefaultDaysToLessFromHistoryFromDate, FinancialisActiveonCommercialInvoice, CommercialInvoicePrefix, RateAddLessOnCommercialInvoice. */
    private Map<String, Object> config(UserAccount u) {
        Map<String, Object> c = new LinkedHashMap<>();
        c.put("defaultDaysToLessFromHistoryFromDate", asInt(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        c.put("flagFin", asBool(repo.config(u, "FinancialisActiveonCommercialInvoice")));
        c.put("autoGenInvoiceNo", asBool(repo.config(u, "CommercialInvoicePrefix")));
        c.put("rateAddLessOnCommercialInvoice", asBool(repo.config(u, "RateAddLessOnCommercialInvoice")));
        return c;
    }

    /** BindData / btnRefresh_Click: every combo source. */
    private Map<String, Object> combos(UserAccount u, boolean f9, boolean f15) {
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "creditAccounts", () -> idName(repo.creditAccounts(u), "Id", "AccountTitle"));
        put(out, "customers", () -> idName(repo.exportContractDropDown(u, "Customer"), "Id", "name"));   // SupplierCustomerFromContracts
        put(out, "notifyParties", () -> idName(repo.exportCustomers(u), "Id", "CompanyName"));          // CustomerGetAll
        put(out, "deliveryTerms", () -> pickAll(repo.deliveryTerms(), "Id", "Code", "Description"));
        put(out, "paymentTerms", () -> paymentTermRows(repo.paymentTerms(u)));
        put(out, "ports", () -> pickAll(repo.seaPorts(u), "Id", "PortName", "PortType"));
        put(out, "banks", () -> pickAll(repo.banks(u), "Id", "BranchName", "IsHomeland", "BankIBANNo"));
        put(out, "otherItems", () -> idName(repo.otherItems(u), "Id", "ItemName"));
        put(out, "currencies", () -> idName(repo.currencies(u), "Id", "CurrencyName"));
        put(out, "cropYears", () -> idName(repo.cropYears(u), "Id", "CropYear"));
        put(out, "jobLots", () -> idName(repo.jobLots(u), "Id", "JobLotDescription"));
        put(out, "packTypes", () -> idName(repo.packTypes(u), "Id", "Description"));
        put(out, "farmingNTrade", () -> pickAll(repo.farmingNTrade(), "Id", "FarmingNTrade", "ExImFarmingTypeId", "ExImTradeTypeId"));
        put(out, "exportCharges", () -> idName(repo.exportCharges(u), "ExportChargesID", "ExportChargesName"));
        if (f9) put(out, "exportCompanies", () -> idName(repo.exportCompanies(u), "Id", "CompName"));
        if (f15) put(out, "salesPersons", () -> idName(repo.supplierCustomersByGroup(u, "9", 0), "Id", "CompanyName"));
        return out;
    }

    private static List<Map<String, Object>> pickAll(List<Map<String, Object>> rows, String... cols) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) out.add(pick(r, cols));
        return out;
    }

    private static List<Map<String, Object>> paymentTermRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("lcOrderTerm", text(ci(r, "lcOrderTerm")));
            m.put("Description", text(ci(r, "lcOrderTermDesc")));
            out.add(m);
        }
        return out;
    }

    /** btnRefresh_Click: the combos again + CommercialInvoicePrefix re-read (GenerateInvoiceNo when on). */
    public Map<String, Object> refresh() {
        UserAccount u = user("View");
        boolean f9 = repo.erpFeature(u, 9), f15 = repo.erpFeature(u, 15);
        Map<String, Object> out = combos(u, f9, f15);
        boolean auto = asBool(repo.config(u, "CommercialInvoicePrefix"));
        out.put("autoGenInvoiceNo", auto);
        if (auto) put(out, "invoiceNo", () -> repo.generateInvoiceNo(u, DOCUMENT_TYPE_ID, fy()));
        return out;
    }

    /** FormReset: generateCode + GenerateInvoiceNo when AutoGenInvoiceNo. */
    public Map<String, Object> newForm() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("docNo", repo.generateDocNo(u, DOCUMENT_TYPE_ID, fy()));
        boolean auto = asBool(repo.config(u, "CommercialInvoicePrefix"));
        out.put("autoGenInvoiceNo", auto);
        out.put("invoiceNo", auto ? repo.generateInvoiceNo(u, DOCUMENT_TYPE_ID, fy()) : "");
        return out;
    }

    /**
     * cmbSupCust_Leave: BindFinancialInstrument (GetFINoForInvoice with the customer, only when one is chosen),
     * bindConsigneeAgainstCustomer (GetSupplierustomerByCustomerGroupId(null, customer)), HSCodeBind (item + customer).
     */
    public Map<String, Object> customerLeave(int customerId, int itemId) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> fis = new ArrayList<>();
        if (customerId > 0) for (Map<String, Object> r : repo.fiNosForInvoice(u, customerId)) fis.add(fiRow(r));
        out.put("fis", fis);
        out.put("consignees", customerId > 0 ? idName(repo.supplierCustomersByGroup(u, null, customerId), "Id", "CompanyName") : new ArrayList<>());
        out.put("hsCodes", hsCodes(u, itemId, customerId));
        return out;
    }

    private static Map<String, Object> fiRow(Map<String, Object> r) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(ci(r, "Id")));
        m.put("EFormNo", text(ci(r, "EFormNo")));
        m.put("PaymenttermId", asInt(ci(r, "PaymenttermId")));
        m.put("DocumnetTypeId", asInt(ci(r, "DocumentTypeId")));
        return m;
    }

    private List<Map<String, Object>> hsCodes(UserAccount u, int itemId, int customerId) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (itemId <= 0) return out;
        for (Map<String, Object> r : repo.commodityRemarks(u, itemId, customerId)) out.add(pick(r, "Id", "HSCode"));
        return out;
    }

    /** combitem_Leave: bindRateUomAndItemPackUom + HSCodeBind. */
    public Map<String, Object> itemLeave(int itemId, int customerId) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> uoms = new ArrayList<>();
        for (Map<String, Object> r : repo.uomSchedule(u, itemId)) uoms.add(pick(r, "Id", "UOMCode", "Equivalent"));
        out.put("uoms", uoms);
        out.put("hsCodes", hsCodes(u, itemId, customerId));
        return out;
    }

    /** GetContractScheduleByContractId(ContractId) with ExImInvoiceId = RecId: Id, AttentiveLoadingDate, FCL, ScheduleCode. */
    public List<Map<String, Object>> contractSchedules(int contractId, int recId) {
        UserAccount u = user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.contractSchedules(u, contractId, recId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            List<Object> vals = new ArrayList<>(r.values());
            m.put("Id", asInt(ci(r, "Id")));
            m.put("AttentiveLoadingDate", iso(ci(r, "AttentiveLoadingDate")));
            m.put("Cell2", vals.size() > 2 ? asDouble(vals.get(2)) : 0d);         // SelectedRow.Cells[2] = ScheduleFCL
            m.put("Cell3", vals.size() > 3 ? text(vals.get(3)) : "");             // SelectedRow.Cells[3] = ContractNoInvoiceWise
            out.add(m);
        }
        return out;
    }

    /** dcmbfino_ValueChanged -> GetFinancialInstrumentsBalance ("#,#0.###"). */
    public Map<String, Object> fiBalance(int id, int documentTypeId) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("balance", asDouble(repo.fiBalance(u, documentTypeId, id)));
        return out;
    }

    // ================================================================== history

    /** HistoryCombosFill: ActivityType Customer / Invoice. */
    public Map<String, Object> historyCombos(UserAccount u) {
        List<Map<String, Object>> customers = new ArrayList<>(), invoices = new ArrayList<>();
        for (Map<String, Object> r : repo.historyDropDowns(u)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("Name", text(ci(r, "name")));
            String t = text(ci(r, "ActivityType"));
            if ("Customer".equals(t)) customers.add(m); else if ("Invoice".equals(t)) invoices.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("customers", customers);
        out.put("invoices", invoices);
        return out;
    }

    public Map<String, Object> historyCombos() { return historyCombos(user("View")); }

    /** HistoryGridFill: Ids "204", the radio's date pair when its box is ticked, ActionId 2 / 1 / 0. */
    public List<Map<String, Object>> history(Map<String, Object> body) {
        UserAccount u = user("View");
        java.sql.Date from = asBool(body.get("fromChecked")) ? sqlDate(body.get("fromDate")) : null;
        java.sql.Date to = asBool(body.get("toChecked")) ? sqlDate(body.get("toDate")) : null;
        String kind = text(body.get("dateKind"));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.formHistory(u, fy(), String.valueOf(DOCUMENT_TYPE_ID), kind.isEmpty() ? "doc" : kind, from, to,
                asInt(body.get("customerId")), asInt(body.get("invoiceId")), asInt(body.get("fromDocNo")), asInt(body.get("toDocNo")), asInt(body.get("actionId")))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("DocCode", asInt(ci(r, "DocCode")));
            m.put("DocDate", iso(ci(r, "DocDate")));
            m.put("CustomerName", text(ci(r, "Customer")));
            m.put("ContractNos", text(ci(r, "ContractNos")));
            m.put("NoOfContainers", asDouble(ci(r, "NoOfContainers")));
            m.put("GrossWeight", asDouble(ci(r, "GrossWeight")));
            m.put("NetWeight", asDouble(ci(r, "NetWeight")));
            m.put("FcyAmount", asDouble(ci(r, "FCurrencyAmount")));
            m.put("AddLess", asDouble(ci(r, "AddLessAmount")));
            m.put("TotalAmount", asDouble(ci(r, "TotalAmount")));
            m.put("JobLots", text(ci(r, "JobLots")));
            m.put("LoadingPort", text(ci(r, "LoadingPort")));
            m.put("DestinationPort", text(ci(r, "DestinationPort")));
            m.put("EntryUser", text(ci(r, "EntryUserName")));
            m.put("EntryDate", isoDateTime(ci(r, "EntryDate")));
            m.put("ModifyUser", text(ci(r, "ModifyUserName")));
            m.put("ModifyDate", isoDateTime(ci(r, "ModifyDate")));
            m.put("NoOfAttachments", asInt(ci(r, "NoOfAttachments")));
            out.add(m);
        }
        return out;
    }

    /** DataGridHistory_SelectionChanged -> GetDetailGrdByHeadId: the dtdetail rows of the selected invoice. */
    public List<Map<String, Object>> historyDetail(int id) {
        user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : repo.child(id, "ReadExImInvoicePackingDetailByHeaderId")) out.add(detailRow(d));
        return out;
    }

    // ================================================================== ReadById

    /** FillDetailFromListCommonForReadById - one dtdetail row from an ExImInvoicePackingDetail. */
    static Map<String, Object> detailRow(Map<String, Object> d) {
        Map<String, Object> m = new LinkedHashMap<>();
        double rwal = asDouble(ci(d, "RateWithoutAddLess")), ratePrice = asDouble(ci(d, "RatePrice"));
        m.put("Id", asInt(ci(d, "Id")));
        m.put("ContractId", asInt(ci(d, "ContractId")));
        m.put("ContractDetailId", asInt(ci(d, "ContractDetailId")));
        m.put("ContractNo", text(ci(d, "PriRefNo")));
        m.put("ContractDate", iso(ci(d, "ProformaDocDate")));
        m.put("ContractNoInvoiceWise", text(ci(d, "ContractNoShipmentWise")));
        m.put("ItemId", asInt(ci(d, "ItemId")));
        m.put("ItemName", text(ci(d, "ItemName")));
        m.put("PackTypeId", asInt(ci(d, "PackingMaterialTypeId")));
        m.put("PackType", text(ci(d, "PackMaterilaType")));
        m.put("CropYearId", asInt(ci(d, "CropYearId")));
        m.put("CropYear", text(ci(d, "CropYear")));
        m.put("JobLotId", asInt(ci(d, "JobLotId")));
        m.put("JobLot", text(ci(d, "JobLotDescription")));
        m.put("QtyMTon", asDouble(ci(d, "MTon")));
        m.put("PackSizeId", asInt(ci(d, "OuterQtyUomId")));
        m.put("PackSize", text(ci(d, "OuterUOM")));
        m.put("NoOfBags", asDouble(ci(d, "OuterQty")));
        m.put("RateWithoutAddLess", rwal > 0 ? rwal : ratePrice);
        m.put("RateAddLess", asDouble(ci(d, "RateAddLess")));
        m.put("CostMTon", ratePrice);
        m.put("ContractRate", asDouble(ci(d, "ContractRate")));
        m.put("RateUOMId", asInt(ci(d, "RateUomId")));
        m.put("RateUOM", text(ci(d, "RateUOM")));
        m.put("RateEquivalent", asDouble(ci(d, "RateEquivalent")));
        m.put("Amount", asDouble(ci(d, "FcAmount")));
        m.put("ContractScheduleId", asInt(ci(d, "ContractScheduleId")));
        m.put("ContractSchedule", shortDate(ci(d, "AttentiveLoadingDate")));
        m.put("HSCode", text(ci(d, "HsCode")));
        m.put("ItemDetail", text(ci(d, "ItemDescriptionManual")));
        m.put("PackingDetail", text(ci(d, "OuterPackDescription")));
        m.put("OtherItemAmount", asDouble(ci(d, "OtherAmount")));
        m.put("AddLessAmount", asDouble(ci(d, "AddLessAmount")));
        m.put("FlagToUpdateScheduleCombo", true);
        m.put("ExImFarmingNTradeId", asInt(ci(d, "ExImFarmingNTradeId")));
        m.put("FarmingNTrade", text(ci(d, "FarmingNTrade")));
        m.put("ExImFarmingTypeId", asInt(ci(d, "ExImFarmingTypeId")));
        m.put("ExImTradeTypeId", asInt(ci(d, "ExImTradeTypeId")));
        m.put("ContractScheduleDetailId", asInt(ci(d, "ContractScheduleDetailId")));
        m.put("ScheduleFCL", asDouble(ci(d, "ScheduleFCL")));
        m.put("ContractType", asInt(ci(d, "ContractType")));
        return m;
    }

    /** ReadById(Id): ExImInvoice.GetByID - header and the child lists the form shows. */
    public Map<String, Object> readById(int id) {
        UserAccount u = user("View");
        List<Map<String, Object>> h = repo.headerById(id);
        if (h.isEmpty()) throw new IllegalStateException("Index was out of range. Must be non-negative and less than the size of the collection.");
        Map<String, Object> r = h.get(0);
        Map<String, Object> o = new LinkedHashMap<>();
        for (String c : new String[]{"Id", "BranchesId", "ProjectsId", "DocCode", "SupplierCustomerId", "ConsigneeId", "NotifyParty1", "NotifyParty2",
                "ImporterBankId", "ExporteBankId", "DeliveryTermId", "LoadingPortId", "DestinationPortId", "FcurrencyId", "ExportCompanyId",
                "CreditAccountId", "SubContractExpiryDays", "ExportVoucherId", "ForwardingId"}) o.put(c, asInt(ci(r, c)));
        for (String c : new String[]{"InvoiceNo", "LotNoRef", "CarierType", "EFormNo", "Certificate1", "Certificate2", "PaymentRemarks",
                "DeliveryRemarks", "AddLessComments", "CustomerContractNos", "AttachmentsValues", "CustomAttachmentsValues"}) o.put(c, text(ci(r, c)));
        for (String c : new String[]{"FCurrencyAmount", "EquivalentAmount", "GrossWeight", "NetWeight", "NoOfContainers", "AddLessAmount",
                "TotalAmount", "ConversionRate", "GrossMton", "NetMton"}) o.put(c, asDouble(ci(r, c)));
        o.put("DocDate", iso(ci(r, "DocDate")));
        o.put("EFormDate", iso(ci(r, "EFormDate")));
        List<Map<String, Object>> detail = new ArrayList<>();
        for (Map<String, Object> d : repo.child(id, "ReadExImInvoicePackingDetailByHeaderId")) detail.add(detailRow(d));
        o.put("detail", detail);
        o.put("invoiceUpdateFlag", detail.isEmpty() ? 0 : detail.get(0).get("ContractType"));   // InvoiceUpdateFlag = Detail[0].ContractType
        List<Map<String, Object>> oi = new ArrayList<>();
        for (Map<String, Object> d : repo.child(id, "ReadExImInvoiceOtherItemsByHeaderId")) oi.add(otherItemRow(d));
        o.put("otherItems", oi);
        List<Map<String, Object>> pt = new ArrayList<>();
        for (Map<String, Object> d : repo.child(id, "ReadExImInvoicePaymentTermsDetailByHeaderId")) pt.add(paymentTermRow(d));
        o.put("paymentTerms", pt);
        List<Map<String, Object>> cm = new ArrayList<>();
        for (Map<String, Object> d : repo.child(id, "EximInvoiceCommissionInfoByReadByHeaderId")) cm.add(commissionRow(d));
        o.put("commissions", cm);
        List<Map<String, Object>> oc = new ArrayList<>();
        for (Map<String, Object> d : repo.child(id, "ExImInvoiceOtherChargesDetail_ReadByHeaderId")) oc.add(otherChargeRow(d, false));
        o.put("otherCharges", oc);
        /* ReadById ends with BindFinancialInstrument() and cmbSupCust_Leave (consignees, FIs, HS codes). */
        o.put("customer", customerLeave(asInt(o.get("SupplierCustomerId")), 0));
        return o;
    }

    // ================================================================== loader (LoadSalesContractForInvoice, DocumentTypeId 202)

    /** LoadInvoices_Load: FromDate = ActiveYr.Start_Period, AllComboBind (Customer / Items / Currency), ExportSalesContractApprovalMandatory. */
    public Map<String, Object> loaderSetup() {
        UserAccount u = user("View");
        return loaderSetup(u, repo, fy());
    }

    static Map<String, Object> loaderSetup(UserAccount u, ExportCommercialInvoiceRepository repo, int fy) {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> customers = new ArrayList<>(), items = new ArrayList<>(), currencies = new ArrayList<>();
        for (Map<String, Object> r : repo.exportContractDropDown(u, null)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("Name", text(ci(r, "name")));
            String t = text(ci(r, "ActivityType"));
            if ("Customer".equals(t)) customers.add(m); else if ("Items".equals(t)) items.add(m); else if ("Currency".equals(t)) currencies.add(m);
        }
        out.put("customers", customers);
        out.put("items", items);
        out.put("currencies", currencies);
        out.put("fromDate", iso(repo.financialYearStart(fy)));
        out.put("approvalMandatory", asBool(repo.config(u, "ExportSalesContractApprovalMandatory")));
        return out;
    }

    /** ExportInvoicesLoad (schedule wise): GetProformaDataForInvoices - all rows; the page builds the header grid (one row per ScheduleId). */
    public List<Map<String, Object>> loader(Map<String, Object> body) {
        UserAccount u = user("View");
        return loaderRows(u, repo, fy(), LOADER_DOCUMENT_TYPE_ID, body);
    }

    static List<Map<String, Object>> loaderRows(UserAccount u, ExportCommercialInvoiceRepository repo, int fy, int documentTypeId, Map<String, Object> body) {
        Timestamp from = body.get("fromDate") == null || text(body.get("fromDate")).isEmpty() ? null : ts(body.get("fromDate"));
        Timestamp to = body.get("toDate") == null || text(body.get("toDate")).isEmpty() ? null : ts(body.get("toDate"));
        String scheduleIds = asBool(body.get("loadSaved")) ? text(body.get("scheduleIds")) : null;
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.proformaDataForInvoices(u, fy, documentTypeId, from, to, asInt(body.get("customerId")),
                asInt(body.get("currencyId")), asInt(body.get("itemId")), scheduleIds)) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) {
                Object v = e.getValue();
                if (v instanceof Timestamp || v instanceof java.sql.Date) v = iso(v);
                else if (v instanceof BigDecimal) v = ((BigDecimal) v).doubleValue();
                m.put(e.getKey(), v);
            }
            out.add(m);
        }
        return out;
    }

    /** BindLoaderData: GetOtherItemsFromLcOrder, LcOrderPaymentTermsDetailByLcOrderIds, ExImLcOrderOtherChargesDetailByLcOrderIds (distinct contract ids). */
    public Map<String, Object> loaderApply(String ids) {
        UserAccount u = user("View");
        return loaderApply(u, repo, ids);
    }

    static Map<String, Object> loaderApply(UserAccount u, ExportCommercialInvoiceRepository repo, String ids) {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> oi = new ArrayList<>();
        for (Map<String, Object> d : repo.otherItemsFromLcOrder(u, ids)) oi.add(otherItemRow(d));
        out.put("otherItems", oi);
        List<Map<String, Object>> pt = new ArrayList<>();
        for (Map<String, Object> d : repo.lcOrderPaymentTerms(ids)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", 0);
            m.put("PaymentTermId", asInt(ci(d, "PaymentTermId")));
            m.put("PaymentTerm", text(ci(d, "LcOrderTerm")));
            m.put("DocumentTypeId", 0);
            m.put("ExImEFormRegistrationId", 0);
            m.put("FinancialInstrumentNo", "");
            m.put("PrcntOfTotal", asDouble(ci(d, "PrcntOfTotal")));
            m.put("FcyAmount", asDouble(ci(d, "FcyAmount")));
            m.put("DueDays", asInt(ci(d, "DueDays")));
            m.put("Remarks", text(ci(d, "Remarks")));
            pt.add(m);
        }
        out.put("paymentTerms", pt);
        List<Map<String, Object>> oc = new ArrayList<>();
        for (Map<String, Object> d : repo.lcOrderOtherCharges(ids)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", 0);
            m.put("ContractId", asInt(ci(d, "ExImLcOrderId")));
            m.put("ContractOtherChargesDetailId", asInt(ci(d, "Id")));
            m.put("ChargesItemId", asInt(ci(d, "ChargesItemId")));
            m.put("ChargesItem", text(ci(d, "ChargesItemName")));
            m.put("AddAmount", asDouble(ci(d, "AddAmount")));
            m.put("LessAmount", asDouble(ci(d, "LessAmount")));
            m.put("Remarks", text(ci(d, "Remarks")));
            oc.add(m);
        }
        out.put("otherCharges", oc);
        return out;
    }

    // ================================================================== save

    private static double round3(double v) { return BigDecimal.valueOf(v).setScale(3, RoundingMode.HALF_UP).doubleValue(); }

    /**
     * btnsave_Click after the page's confirmations (Save/Update, the add-less-without-charges question and the
     * "Credit Value charge to Sale Account" question are asked on the page, in the desktop's order). Every
     * rule that decides what is written is checked again here, with the desktop text.
     */
    public Map<String, Object> save(Map<String, Object> body) {
        int recId = asInt(body.get("recId"));
        UserAccount u = user(recId == 0 ? "Save" : "Update");
        Map<String, Object> h = map(body.get("header"));
        List<Map<String, Object>> detail = list(body.get("detail"));
        List<Map<String, Object>> removed = list(body.get("removed"));
        List<Map<String, Object>> otherItems = list(body.get("otherItems"));
        List<Map<String, Object>> paymentTerms = list(body.get("paymentTerms"));
        List<Map<String, Object>> commissions = list(body.get("commissions"));
        List<Map<String, Object>> otherCharges = list(body.get("otherCharges"));
        boolean flagFin = asBool(repo.config(u, "FinancialisActiveonCommercialInvoice"));
        boolean f9 = repo.erpFeature(u, 9);

        if (detail.isEmpty()) throw new IllegalArgumentException("Grid Record not found");
        double mtonTotal = 0;
        for (Map<String, Object> r : detail) mtonTotal += asDouble(r.get("QtyMTon"));
        double netWeightBox = round3(mtonTotal);                       // txtNetWeight.Text = total.ToString("#,##0.###")

        /* formvalidation */
        if (f9 && asInt(h.get("ExportCompanyId")) == 0) throw new IllegalArgumentException("Export Company Is required");
        if (asInt(h.get("DocCode")) == 0) throw new IllegalArgumentException("Doc No Is Required");
        if (text(h.get("InvoiceNo")).isEmpty()) throw new IllegalArgumentException("Invoice No Is Required");
        if (asInt(h.get("SupplierCustomerId")) == 0) throw new IllegalArgumentException("Customer Is Required");
        if (flagFin && asInt(h.get("CreditAccountId")) == 0 && !asBool(h.get("creditToSaleConfirmed"))) throw new IllegalArgumentException("Please select CreditAc");
        if (asInt(h.get("DeliveryTermId")) == 0) throw new IllegalArgumentException("Delivery Term Is Required");
        if (asInt(h.get("LoadingPortId")) == 0) throw new IllegalArgumentException("Loading Port Is Required");
        if (asInt(h.get("DestinationPortId")) == 0) throw new IllegalArgumentException("Destination Port  Is Required");
        if (asInt(h.get("CarierTypeId")) == 0) throw new IllegalArgumentException("Carrier Type  Is Required");
        if (asInt(h.get("FcurrencyId")) == 0) throw new IllegalArgumentException("Fcy Code Is Required");
        if (asDouble(h.get("FCurrencyAmount")) == 0) throw new IllegalArgumentException("Fcy Amount  Is Required");
        if (asInt(h.get("NoOfContainers")) == 0) throw new IllegalArgumentException("No.of Containers Field Is Required");
        if (!(netWeightBox > 0)) throw new IllegalArgumentException("Net Weight  Is Required");
        if (!(asDouble(h.get("GrossMton")) >= netWeightBox)) throw new IllegalArgumentException("Gross Weight Must Be Equal Or Greater Than Net Weight Thank You");
        if (!(asDouble(h.get("ConversionRate")) > 0)) throw new IllegalArgumentException("ExchangeRate Field is Required");
        if (!(asDouble(h.get("TotalAmount")) > 0)) throw new IllegalArgumentException("Total Amount  Is Required");

        Timestamp now = new Timestamp(System.currentTimeMillis());
        Map<String, Object> pih = header(u, recId, h, now, fy());
        ExportCommercialInvoiceRepository.SaveSet set = new ExportCommercialInvoiceRepository.SaveSet();
        if (recId > 0) for (Map<String, Object> r : removed) set.packing.add(removedModel(r));

        double mtonPerContainer = netWeightBox / asDouble(h.get("NoOfContainers"));
        StringBuilder contractIds = new StringBuilder(), contractDetailIds = new StringBuilder(), contractNos = new StringBuilder(),
                contractDates = new StringBuilder(), scheduleIds = new StringBuilder(), scheduleNos = new StringBuilder();
        double totalFc = 0, netWeight = 0;
        BigDecimal netMton = BigDecimal.ZERO;
        int i = 0;
        for (Map<String, Object> r : detail) {
            Map<String, Object> vd = blankPacking(false);
            vd.put("Id", asInt(r.get("Id")));
            vd.put("ContractId", asInt(r.get("ContractId")));
            if (asInt(vd.get("ContractId")) == 0) throw new IllegalArgumentException("ContractId required in Row#" + (i + 1));
            vd.put("ContractDetailId", asInt(r.get("ContractDetailId")));
            contractIds.append(',').append(text(r.get("ContractId")));
            contractDetailIds.append(',').append(text(r.get("ContractDetailId")));
            contractNos.append(',').append(text(r.get("ContractNo")));
            contractDates.append(',').append(shortDate(r.get("ContractDate")));
            scheduleIds.append(',').append(text(r.get("ContractScheduleId")));
            scheduleNos.append(',').append(text(r.get("ContractNoInvoiceWise")));
            if (asInt(vd.get("ContractDetailId")) == 0) throw new IllegalArgumentException("ContractDetailId required in Row#" + (i + 1));
            vd.put("ContractNoShipmentWise", text(r.get("ContractNoInvoiceWise")));
            vd.put("ContractScheduleDetailId", asInt(r.get("ContractScheduleDetailId")));
            vd.put("ItemId", asInt(r.get("ItemId")));
            vd.put("CropYearId", asInt(r.get("CropYearId")));
            vd.put("JobLotId", asInt(r.get("JobLotId")));
            vd.put("PackingMaterialTypeId", asInt(r.get("PackTypeId")));
            double mton = asDouble(r.get("QtyMTon"));
            vd.put("MTon", mton);
            vd.put("NetWeight", mton * 1000.0);
            vd.put("OuterQtyUomId", asInt(r.get("PackSizeId")));
            vd.put("OuterQty", asDouble(r.get("NoOfBags")));
            vd.put("InnerQtyUomId", asInt(r.get("PackSizeId")));
            vd.put("InnerQty", (double) asInt(r.get("NoOfBags")));
            vd.put("RateWithoutAddLess", asDouble(r.get("RateWithoutAddLess")));
            vd.put("RateAddLess", asDouble(r.get("RateAddLess")));
            vd.put("RatePrice", asDouble(r.get("CostMTon")));
            vd.put("RateUomId", asInt(r.get("RateUOMId")));
            vd.put("AddLessAmount", asDouble(r.get("AddLessAmount")));
            vd.put("FcAmount", asDouble(r.get("Amount")));
            vd.put("OtherAmount", asDouble(r.get("OtherItemAmount")));
            vd.put("ContractScheduleId", asInt(r.get("ContractScheduleId")));
            if (asInt(vd.get("ContractScheduleId")) == 0) throw new IllegalArgumentException("Contract Schedule required in Row#" + (i + 1));
            totalFc += asDouble(vd.get("FcAmount"));
            vd.put("HsCode", text(r.get("HSCode")));
            vd.put("ItemDescriptionManual", text(r.get("ItemDetail")));
            vd.put("OuterPackDescription", text(r.get("PackingDetail")));
            vd.put("ExImFarmingNTradeId", asInt(r.get("ExImFarmingNTradeId")));
            vd.put("ExImTradeTypeId", asInt(r.get("ExImTradeTypeId")));
            vd.put("ExImFarmingTypeId", asInt(r.get("ExImFarmingTypeId")));
            vd.put("ActionTypeId", asInt(vd.get("Id")) <= 0 ? 1 : 2);
            netWeight += mton * 1000.0;
            netMton = netMton.add(BigDecimal.valueOf(mton));
            vd.put("NoofContainers", mton / mtonPerContainer);
            set.packing.add(vd);
            i++;
        }
        pih.put("NetWeight", netWeight);
        pih.put("NetMton", netMton);
        pih.put("ContractIds", distinctJoin(contractIds.toString()));
        pih.put("ContractDetailIds", distinctJoin(contractDetailIds.toString()));
        pih.put("ContractNos", distinctJoin(contractNos.toString()));
        pih.put("ContractDates", distinctJoin(contractDates.toString()));
        pih.put("ContractScheduleIds", distinctJoin(scheduleIds.toString()));
        pih.put("ContractScheduleNos", distinctJoin(scheduleNos.toString()));

        double otherAmount = 0, utilize = 0;
        for (Map<String, Object> r : otherItems) {
            if (!(dec(r.get("Qty")).signum() > 0)) continue;
            Map<String, Object> d = otherItemModel(r);
            otherAmount += asDouble(d.get("oItemAmount"));
            set.otherItems.add(d);
        }
        if (paymentTerms.isEmpty()) throw new IllegalArgumentException("Payment Detail Not Found. Please Check! ");
        int sort = 0;
        for (Map<String, Object> r : paymentTerms) {
            Map<String, Object> d = paymentTermModel(r, ++sort);
            if (asInt(d.get("PaymentTermId")) == 1 && asInt(d.get("ExImEFormRegistrationId")) == 0)
                throw new IllegalArgumentException("Financial Instrument number field is required against advance in payment term detail...");
            utilize += asDouble(d.get("FcyAmount"));
            set.paymentTerms.add(d);
        }
        pih.put("PaymentTermId", asInt(paymentTerms.get(0).get("PaymentTermId")));
        for (Map<String, Object> r : commissions) {
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("exImInvoiceSaleManCommissionId", 0);
            d.put("exImInvoiceId", 0);
            d.put("SalesPersonId", asInt(r.get("SalesPersonId")));
            d.put("commissionTypeId", asInt(r.get("CommissionTypeId")));
            d.put("ActionTypeId", 1);
            d.put("commissionRate", asDouble(r.get("Rate")));
            d.put("rateUom", text(r.get("RateUom")));
            d.put("fcyAmount", asDouble(r.get("FcyAmount")));
            d.put("exchangeRate", asDouble(pih.get("ConversionRate")));
            d.put("lcyAmount", asDouble(r.get("LcyAmount")));
            d.put("Remarks", text(r.get("Remarks")));
            set.commissions.add(d);
        }
        i = 0;
        for (Map<String, Object> r : otherCharges) {
            Map<String, Object> d = otherChargeModel(r, false);
            validateField(asInt(d.get("ChargesItemId")), "Charges Item", i, "Other Charges");
            if (asDouble(d.get("AddAmount")) == 0 && asDouble(d.get("LessAmount")) == 0)
                throw new IllegalArgumentException("AddAmount Or Less Amount is required in Other Charges at row No: " + (i + 1));
            set.otherCharges.add(d);
            i++;
        }
        double totalAmount = totalFc + asDouble(pih.get("AddLessAmount")) + otherAmount;
        pih.put("TotalAmount", totalAmount);
        if (Math.abs(netRound(totalAmount) - netRound(utilize)) >= 1.0)
            throw new IllegalArgumentException("Total Invoice Amount and Total Payment Utilize Amount Not equal Please Check");

        /* ExImInvoice.Save: DocumentTypeId 204 builds the voucher only when FinancialisActiveonCommercialInvoice is on. */
        pih.put("ActionId", recId == 0 ? 1 : 2);
        if (flagFin) {
            Voucher v = makeVoucher(pih, set.packing, set.otherItems, repo.supplierCustomerGl(u), repo.itemGl(u));
            pih.put("CreditAccountId", v.creditAccountId);
            set.voucherHead = v.head;
            set.voucherDetails = v.details;
        }
        int id = repo.save(pih, set);
        Map<String, Object> out = saved(id, recId == 0 ? "Save SuccessFully" : "Update SuccessFully");
        out.put("printId", id);
        return out;
    }

    /** ExImInvoice non-virtual properties in btnsave_Click's assignment; the ones the form never sets keep their defaults. */
    private Map<String, Object> header(UserAccount u, int recId, Map<String, Object> h, Timestamp now, int fy) {
        Map<String, Object> p = blankHeader();
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
        p.put("DocDate", ts(h.get("DocDate")));
        p.put("InvoiceNo", trim(h.get("InvoiceNo")));
        p.put("SupplierCustomerId", asInt(h.get("SupplierCustomerId")));
        p.put("ConsigneeId", asInt(h.get("ConsigneeId")));
        p.put("NotifyParty1", asInt(h.get("NotifyParty1")));
        p.put("NotifyParty2", asInt(h.get("NotifyParty2")));
        p.put("LotNoRef", text(h.get("LotNoRef")));
        p.put("EFormNo", trim(h.get("EFormNo")));
        p.put("EFormDate", ts(h.get("EFormDate")));
        p.put("FcurrencyId", asInt(h.get("FcurrencyId")));
        p.put("ConversionRate", asDouble(h.get("ConversionRate")));
        p.put("FCurrencyAmount", asDouble(h.get("FCurrencyAmount")));
        p.put("EquivalentAmount", asDouble(h.get("EquivalentAmount")));
        p.put("AddLessAmount", asDouble(h.get("AddLessAmount")));
        p.put("TotalAmount", asDouble(h.get("TotalAmount")));
        p.put("AddLessComments", text(h.get("AddLessComments")));
        p.put("DeliveryTermId", asInt(h.get("DeliveryTermId")));
        p.put("PaymentRemarks", text(h.get("PaymentRemarks")));
        p.put("DeliveryRemarks", text(h.get("DeliveryRemarks")));
        p.put("ImporterBankId", asInt(h.get("ImporterBankId")));
        p.put("ExporteBankId", asInt(h.get("ExporteBankId")));
        p.put("LoadingPortId", asInt(h.get("LoadingPortId")));
        p.put("DestinationPortId", asInt(h.get("DestinationPortId")));
        p.put("CarierType", text(h.get("CarierType")));
        BigDecimal grossMton = dec(h.get("GrossMton"));
        p.put("GrossMton", grossMton);
        p.put("GrossWeight", grossMton.multiply(BigDecimal.valueOf(1000)).doubleValue());
        p.put("ExportCompanyId", asInt(h.get("ExportCompanyId")));
        p.put("NoOfContainers", (double) asInt(h.get("NoOfContainers")));
        p.put("Certificate1", trim(h.get("Certificate1")));
        p.put("Certificate2", trim(h.get("Certificate2")));
        p.put("CreditAccountId", asInt(h.get("CreditAccountId")));
        p.put("CustomerContractNos", text(h.get("CustomerContractNos")));
        p.put("SubContractExpiryDays", asInt(h.get("SubContractExpiryDays")));
        String av = text(h.get("AttachmentsValues"));
        p.put("AttachmentsValues", av.isEmpty() ? null : av);                // AttachmentsValues != "" ? AttachmentsValues : PIH.AttachmentsValues (null)
        p.put("CustomAttachmentsValues", text(h.get("CustomAttachmentsValues")));
        return p;
    }

    /** Every non-virtual ExImInvoice value property at its CLR default (strings null = not sent). */
    static Map<String, Object> blankHeader() {
        Map<String, Object> p = new LinkedHashMap<>();
        String[] ints = {"Id", "CommercialInvoiceId", "ApprovedUser", "BranchesId", "CompanyId", "PrefixTypeId", "DeliveryTermId", "ContinentId",
                "OriginCountryId", "ImporterCountryId", "PlaceOfDeliveryId", "DestinationPortId", "DocCode", "DocumentTypeId", "EFormId", "EntryUser",
                "ExporteBankId", "ExportCompanyId", "FcurrencyId", "ImporterBankId", "LcOrderNoId", "LoadingPortId", "ModifyUser", "OrganizationId",
                "PaymentTermId", "ProjectsId", "SupplierCustomerId", "CommissionAgentId", "FinancialYearId", "PreInvoiceId", "CommissionDebitAcId",
                "OtherCustomerId", "OtherDestinationPortId", "ConsigneeId", "ActionId", "NotifyParty1", "NotifyParty2", "NotifyParty3",
                "CreditAccountId", "SubContractExpiryDays"};
        String[] dbls = {"BuyerGLValueRs", "ConversionRate", "EquivalentAmount", "FCurrencyAmount", "FobValue", "GrossWeight", "InvoiceCommercialValue",
                "NetWeight", "NoOfContainers", "AddLessAmount", "CommissionPercentage", "CommissionAmount", "TotalAmount"};
        for (String k : ints) p.put(k, 0);
        for (String k : dbls) p.put(k, 0d);
        p.put("GrossMton", BigDecimal.ZERO);
        p.put("NetMton", BigDecimal.ZERO);
        p.put("IsApproved", false);
        return p;
    }

    /** grdDetail_ColumnButtonClick "Delete" on a saved row - the ExImInvoicePackingDetail the desktop keeps with ActionTypeId 3. */
    private static Map<String, Object> removedModel(Map<String, Object> r) {
        Map<String, Object> vd = blankPacking(false);
        vd.put("Id", asInt(r.get("Id")));
        vd.put("ContractId", asInt(r.get("ContractId")));
        vd.put("ContractDetailId", asInt(r.get("ContractDetailId")));
        vd.put("ContractNoShipmentWise", text(r.get("ContractNoInvoiceWise")));
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
        vd.put("ContractScheduleId", asInt(r.get("ContractScheduleId")));
        vd.put("ContractScheduleDetailId", asInt(r.get("ContractScheduleDetailId")));
        vd.put("HsCode", text(r.get("HSCode")));
        vd.put("ItemDescriptionManual", text(r.get("ItemDetail")));
        vd.put("ExImFarmingNTradeId", asInt(r.get("ExImFarmingNTradeId")));
        vd.put("ExImTradeTypeId", asInt(r.get("ExImTradeTypeId")));
        vd.put("ExImFarmingTypeId", asInt(r.get("ExImFarmingTypeId")));
        vd.put("OuterPackDescription", text(r.get("PackingDetail")));
        vd.put("ActionTypeId", 3);
        return vd;
    }

    /** Today, for the history To Date default. */
    static String today() { return LocalDate.now().toString(); }
}
