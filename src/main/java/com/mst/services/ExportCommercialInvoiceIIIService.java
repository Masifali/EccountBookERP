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
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.ExportCommercialInvoiceSupport.*;

/**
 * BLL side of screen 880 "Commercial Invoice III" - Architecture.WinApp.Export.frmCommercialInvoiceIII
 * (ClientSize 1184 x 761, ScreenName "frmCommercialInvoiceIII", DocumentTypeId 204).
 *
 * tabControl1 Form | History. Form: toolStrip4 New / Save / Update(hidden) / Refresh / Attachment / Print-521 /
 * Print-521-A..D / Load Sales Contract / Generate Invoice Nos(hidden) / Export Charges / ItemDetail_LookUp /
 * Generate Inv Nos / ShortCut Keys / Packing List + "Preview"; HeaderGropBox "Main Header" with groupBox3 "Custom
 * Values"; tabControl4 Invoice Detail (tabControl2: Item Detail | Payment Detail | Other Charges Detail | Other Items -
 * the Packing List page is removed by InitializeComponentCustom) | Custom Detail (tabControl5: Item Detail | Payment
 * Detail | Other Charges Detail). Detail rows come only from "Load Sales Contract" (LoadSalesContractForInvoice,
 * DocumentTypeId 223), into both the invoice and the custom detail grids.
 *
 * Rights by ScreenDefinition.Id 880: View, Save (new), Update (RecId > 0), Print. Tenancy, branch and financial year
 * come from the session. Attachments are not part of this web port.
 */
@Service
public class ExportCommercialInvoiceIIIService {

    public static final int SCREEN_ID = 880;
    public static final int DOCUMENT_TYPE_ID = 204;
    /** BtnLoad_Click: Loader.DocumentTypeId = 223. */
    public static final int LOADER_DOCUMENT_TYPE_ID = 223;

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

    private static List<Map<String, Object>> pickAll(List<Map<String, Object>> rows, String... cols) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) out.add(pick(r, cols));
        return out;
    }

    // ================================================================== load

    /** InitializeComponentMethod: the background reads, then every Bind, in the desktop's order. */
    public Map<String, Object> setup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, "Save"));
        perm.put("Update", allowed(u, "Update"));
        perm.put("Print", allowed(u, "Print"));
        out.put("permissions", perm);
        Map<String, Object> cfg = new LinkedHashMap<>();
        cfg.put("rateAddLessOnCommercialInvoice", asBool(repo.config(u, "RateAddLessOnCommercialInvoice")));
        cfg.put("allItemsBindonExportContract", asBool(repo.config(u, "AllItemsBindonExportContract")));
        cfg.put("defaultDaysToLessFromHistoryFromDate", asInt(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        cfg.put("itemSearchByCode", asBool(repo.config(u, "ItemSearchByCode")));
        out.put("config", cfg);
        boolean f9 = repo.erpFeature(u, 9);
        out.put("allowExportMultiCompanies", f9);
        put(out, "historyCombos", () -> historyCombos(u));
        put(out, "docNo", () -> String.valueOf(repo.generateDocNo(u, DOCUMENT_TYPE_ID, fy())));
        out.putAll(combos(u, f9));
        return out;
    }

    private Map<String, Object> combos(UserAccount u, boolean f9) {
        Map<String, Object> out = new LinkedHashMap<>();
        if (f9) put(out, "exportCompanies", () -> idName(repo.exportCompanies(u), "Id", "CompName"));
        put(out, "invoiceNos", () -> idName(repo.finalInvoiceNos(u, null), "Id", "FinalInvoiceNo"));
        put(out, "customers", () -> customers(u));
        put(out, "banks", () -> pickAll(repo.banks(u), "Id", "BranchName", "BankIBANNo", "IsHomeland"));
        put(out, "ports", () -> idName(repo.seaPorts(u), "Id", "PortName"));
        put(out, "deliveryTerms", () -> pickAll(repo.deliveryTerms(), "Id", "Code", "Description"));
        put(out, "continents", () -> idName(repo.continents(), "ContinentID", "ContinentName"));
        put(out, "countries", () -> idName(repo.countries(u), "Id", "Description"));
        put(out, "cities", () -> idName(repo.cities(u), "Id", "CityName"));
        put(out, "currencies", () -> idName(repo.currenciesWithRate(u), "Id", "CurrencyCode"));
        put(out, "cropYears", () -> idName(repo.cropYears(u), "Id", "CropYear"));
        put(out, "jobLots", () -> idName(repo.jobLotsGlobal(u), "Id", "JobLotDescription"));
        put(out, "packTypes", () -> idName(repo.packTypes(u), "Id", "Description"));
        put(out, "brands", () -> pickAll(repo.brands(u), "Id", "BrandName", "BrandCode"));
        put(out, "pmItems", () -> pmItems(u));
        put(out, "paymentTerms", () -> paymentTermRows(repo.paymentTerms(u)));
        put(out, "exportCharges", () -> idName(repo.exportCharges(u), "ExportChargesID", "ExportChargesName"));
        return out;
    }

    /** SupplierDtFillFromGlobal: globalAllSupplierCustomer with CustomerGroupId 7 and not a sub party. */
    private List<Map<String, Object>> customers(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.globalSupplierCustomers(u)) {
            if (asInt(ci(r, "CustomerGroupId")) != 7 || asBool(ci(r, "IsSubSupCust"))) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("CompanyName", text(ci(r, "CompanyName")));
            m.put("PartyCode", text(ci(r, "PartyCode")));
            m.put("CityName", text(ci(r, "CityName")));
            m.put("MobileNo", text(ci(r, "MobilePersonal")));
            out.add(m);
        }
        return out;
    }

    /** ItemDtsFillFromGlobal: dtPMItem = getGlobalAllItems with ItemTypeOfTypeId 14 (Id, ItemName, ItemCode). */
    private List<Map<String, Object>> pmItems(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.allItems(u)) {
            if (asInt(ci(r, "ItemTypeOfTypeId")) != 14) continue;
            out.add(pick(r, "Id", "ItemName", "ItemCode"));
        }
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

    /** btnRefresh_Click (global services re-read, then every Bind) - also re-reads the containers of RecId. */
    public Map<String, Object> refresh(int recId) {
        UserAccount u = user("View");
        boolean f9 = repo.erpFeature(u, 9);
        Map<String, Object> out = combos(u, f9);
        out.put("allowExportMultiCompanies", f9);
        put(out, "containers", () -> containers(u, recId));
        return out;
    }

    private List<Map<String, Object>> containers(UserAccount u, int recId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.containers(u, recId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "ContainerId")));
            m.put("Name", text(ci(r, "Container")));
            out.add(m);
        }
        return out;
    }

    /** FormReset: GetDocumentCode + InvoiceNoBind(InvoiceNoDbCall()). */
    public Map<String, Object> newForm() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("docNo", String.valueOf(repo.generateDocNo(u, DOCUMENT_TYPE_ID, fy())));
        out.put("invoiceNos", idName(repo.finalInvoiceNos(u, null), "Id", "FinalInvoiceNo"));
        return out;
    }

    /** cmbSupCust_Leave: FinancialInstrumentDtFillDbCall(customer) + ConsigneeAgainstCustomerBind(customer). */
    public Map<String, Object> customerLeave(int customerId) {
        UserAccount u = user("View");
        return customerLeave(u, customerId);
    }

    private Map<String, Object> customerLeave(UserAccount u, int customerId) {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> fis = new ArrayList<>();
        if (customerId != 0) {
            for (Map<String, Object> r : repo.fiNosForInvoice(u, customerId)) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", asInt(ci(r, "Id")));
                m.put("EFormNo", text(ci(r, "EFormNo")));
                m.put("PaymenttermId", asInt(ci(r, "PaymenttermId")));
                m.put("DocumnetTypeId", asInt(ci(r, "DocumentTypeId")));
                fis.add(m);
            }
        }
        out.put("fis", fis);
        /* globalAllSupplierCustomer with ParentsSupCustId == customer and IsSubSupCust - consignee and the three notify parties. */
        List<Map<String, Object>> subs = new ArrayList<>();
        if (customerId > 0) {
            for (Map<String, Object> r : repo.globalSupplierCustomers(u)) {
                if (asInt(ci(r, "ParentsSupCustId")) != customerId || !asBool(ci(r, "IsSubSupCust"))) continue;
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", asInt(ci(r, "Id")));
                m.put("CompanyName", text(ci(r, "CompanyName")));
                m.put("PartyCode", text(ci(r, "PartyCode")));
                subs.add(m);
            }
        }
        out.put("subParties", subs);
        return out;
    }

    /** FinancialInstrumentsBalanceBindWithDbCall ("0,0"). */
    public Map<String, Object> fiBalance(int id, int documentTypeId) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("balance", asDouble(repo.fiBalance(u, documentTypeId, id)));
        return out;
    }

    /** CommonServices.dtUomFromGloablUomScheduleByItemId(ItemId): the global UOM schedule of the item (Id, UomCode, Equivalent). */
    public List<Map<String, Object>> uoms(int itemId) {
        UserAccount u = user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.allUoms(u)) {
            if (asInt(ci(r, "ItemId")) != itemId) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("UomCode", text(ci(r, "UOMCode")));
            m.put("Equivalent", asDouble(ci(r, "Equivalent")));
            out.add(m);
        }
        return out;
    }

    /** ItemCommodityDetailDbCall(ItemId) - GetRemarks without a customer; the page filters DetailTypeId 1 / 2. */
    public List<Map<String, Object>> commodity(int itemId) {
        UserAccount u = user("View");
        return pickAll(repo.commodityRemarks(u, itemId, 0), "Id", "Remarks", "DetailTypeId");
    }

    /** ThirdPartyNoDbCall(ItemId). */
    public List<Map<String, Object>> thirdParty(int itemId) {
        UserAccount u = user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.thirdPartyInspections(u, fy(), itemId)) out.add(pick(r, "Id", "InspectionNo"));
        return out;
    }

    // ================================================================== history

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

    /** HistoryGridFill: Ids = DocumentTypeId ("204"); dtHistoryGrid has no ContractNos / JobLots columns (unlike 211). */
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
            m.put("NoOfAttachments", asInt(ci(r, "NoOfAttachments")));
            out.add(m);
        }
        return out;
    }

    /** GetDetailGrdByHeadId: all eight history grids; empty when the invoice has no detail row. */
    public Map<String, Object> historyDetail(int id) {
        user("View");
        Map<String, Object> o = children(id);
        if (((List<?>) o.get("detail")).isEmpty()) {
            for (String k : o.keySet().toArray(new String[0])) o.put(k, new ArrayList<>());
        }
        return o;
    }

    private Map<String, Object> children(int id) {
        Map<String, Object> o = new LinkedHashMap<>();
        List<Map<String, Object>> d = new ArrayList<>(), dc = new ArrayList<>(), pt = new ArrayList<>(), ptc = new ArrayList<>(),
                oc = new ArrayList<>(), occ = new ArrayList<>(), pl = new ArrayList<>(), oi = new ArrayList<>();
        for (Map<String, Object> r : repo.child(id, "ReadExImInvoicePackingDetailByHeaderId")) d.add(detailRow(r));
        for (Map<String, Object> r : repo.child(id, "ExImInvoicePackingDetailCustom_ReadByHeaderId")) dc.add(detailRow(r));
        for (Map<String, Object> r : repo.child(id, "ReadExImInvoicePaymentTermsDetailByHeaderId")) pt.add(paymentTermRow(r));
        for (Map<String, Object> r : repo.child(id, "ExImInvoicePaymentTermsDetailCustom_ReadByHeaderId")) ptc.add(paymentTermRow(r));
        for (Map<String, Object> r : repo.child(id, "ExImInvoiceOtherChargesDetail_ReadByHeaderId")) oc.add(otherChargeRow(r, false));
        for (Map<String, Object> r : repo.child(id, "ExImInvoiceOtherChargesDetailCustom_ReadByHeaderId")) occ.add(otherChargeRow(r, true));
        for (Map<String, Object> r : repo.child(id, "ExImInvoicePackingListDetail_ReadByHeaderId"))
            pl.add(pick(r, "Id", "ContainerId", "ContainerNo", "ItemId", "ItemCode", "ItemName", "CommodityDetail", "PmDetail", "PackageDate",
                    "ExpiryDate", "LotNo", "ThirdPartyAnalysisId", "ThirdPartyAnalysisNo", "SubLotNo", "OuterQty", "NetWeight", "GrossWeight", "Remarks"));
        for (Map<String, Object> r : repo.child(id, "ReadExImInvoiceOtherItemsByHeaderId")) oi.add(otherItemRow(r));
        o.put("detail", d);
        o.put("paymentTerms", pt);
        o.put("otherCharges", oc);
        o.put("packingList", pl);
        o.put("otherItems", oi);
        o.put("detailCustom", dc);
        o.put("paymentTermsCustom", ptc);
        o.put("otherChargesCustom", occ);
        return o;
    }

    /** FillDetailFromListCommonForReadById (AddExtraColumns adds CropYear / JobLot / PackingType for the history grids). */
    static Map<String, Object> detailRow(Map<String, Object> d) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(ci(d, "Id")));
        m.put("ContractId", asInt(ci(d, "ContractId")));
        m.put("ContractDetailId", asInt(ci(d, "ContractDetailId")));
        m.put("ContractNo", text(ci(d, "PriRefNo")));
        m.put("ContractDate", iso(ci(d, "ProformaDocDate")));
        m.put("ContractNoInvoiceWise", text(ci(d, "ContractNoShipmentWise")));
        m.put("ContractScheduleId", asInt(ci(d, "ContractScheduleId")));
        m.put("ContractSchedule", text(ci(d, "ContractScheduleNo")));
        m.put("ContractScheduleDetailId", asInt(ci(d, "ContractScheduleDetailId")));
        m.put("ItemCategoryId", asInt(ci(d, "ItemCategoryId")));
        m.put("ItemCategory", text(ci(d, "ItemCategory")));
        m.put("ItemId", asInt(ci(d, "ItemId")));
        m.put("ItemCode", text(ci(d, "ItemCode")));
        m.put("ItemName", text(ci(d, "ItemName")));
        m.put("CropYearId", asInt(ci(d, "CropYearId")));
        m.put("JobLotId", asInt(ci(d, "JobLotId")));
        m.put("PackingTypeId", asInt(ci(d, "PackingMaterialTypeId")));
        m.put("PmItemId", asInt(ci(d, "PmItemId")));
        m.put("PmItemCode", text(ci(d, "PmItemCode")));
        m.put("PmItemName", text(ci(d, "PmItemName")));
        m.put("BrandId", asInt(ci(d, "BrandId")));
        m.put("BrandCode", text(ci(d, "BrandCode")));
        m.put("BrandName", text(ci(d, "BrandName")));
        m.put("QtyMTon", asDouble(ci(d, "NetWeight")) / 1000);
        m.put("OuterUomId", asInt(ci(d, "OuterQtyUomId")));
        m.put("OuterUom", text(ci(d, "OuterUOM")));
        m.put("OuterEquivalent", asDouble(ci(d, "OuterEquivalent")));
        m.put("OuterQtyEquivalent", asDouble(ci(d, "OuterQtyEquivalent")));
        m.put("OuterQty", asDouble(ci(d, "OuterQty")));
        m.put("InnerUomId", asInt(ci(d, "InnerQtyUomId")));
        m.put("InnerUom", text(ci(d, "InnerUOM")));
        m.put("InnerEquivalent", asDouble(ci(d, "InnerEquivalent")));
        m.put("InnerQty", asDouble(ci(d, "InnerQty")));
        m.put("PalletQty", asDouble(ci(d, "PalletQty")));
        m.put("GrossWeight", asDouble(ci(d, "GrossWeight")));
        m.put("RateWithoutAddLess", asDouble(ci(d, "RateWithoutAddLess")));
        m.put("RateAddLess", asDouble(ci(d, "RateAddLess")));
        m.put("NetRate", asDouble(ci(d, "RatePrice")));
        m.put("ContractRate", asDouble(ci(d, "ContractRate")));
        m.put("RateUOMId", asInt(ci(d, "RateUomId")));
        m.put("RateUOM", text(ci(d, "RateUOM")));
        m.put("RateEquivalent", asDouble(ci(d, "RateEquivalent")));
        m.put("Amount", asDouble(ci(d, "FcAmount")));
        m.put("LabInspectionId", asInt(ci(d, "LabInspectionId")));
        m.put("BuyerLotNo", text(ci(d, "BuyerLotNo")));
        m.put("ProductCode", text(ci(d, "ProductCode")));
        m.put("BuyerHSCode", text(ci(d, "BuyerHSCode")));
        m.put("HSCode", text(ci(d, "HsCode")));
        m.put("CommodityDetail", text(ci(d, "ItemCommodityDetail")));
        m.put("PackingDetail", text(ci(d, "OuterPackDescription")));
        m.put("Remarks", text(ci(d, "Remarks")));
        m.put("OtherItemAmount", asDouble(ci(d, "OtherAmount")));
        m.put("ScheduleFCL", asDouble(ci(d, "ScheduleFCL")));
        m.put("CropYear", text(ci(d, "CropYear")));
        m.put("JobLot", text(ci(d, "JobLotDescription")));
        m.put("PackingType", text(ci(d, "PackMaterilaType")));
        return m;
    }

    /** ReadById(ID): FormReset, GetByID, header boxes, the containers of the invoice, every grid. */
    public Map<String, Object> readById(int id) {
        UserAccount u = user("View");
        List<Map<String, Object>> h = repo.headerById(id);
        if (h.isEmpty()) throw new IllegalStateException("Index was out of range. Must be non-negative and less than the size of the collection.");
        Map<String, Object> r = h.get(0);
        Map<String, Object> o = new LinkedHashMap<>();
        for (String c : new String[]{"Id", "ExportCompanyId", "DocCode", "SupplierCustomerId", "ConsigneeId", "NotifyParty1", "NotifyParty2", "NotifyParty3",
                "ImporterBankId", "ExporteBankId", "LoadingPortId", "DestinationPortId", "DeliveryTermId", "ContinentId", "OriginCountryId",
                "ImporterCountryId", "PlaceOfDeliveryId", "FcurrencyId"}) o.put(c, asInt(ci(r, c)));
        for (String c : new String[]{"InvoiceNo", "DocumentaryCreditNo", "CustomerContractNos", "ContractDates", "LotNoRef", "GDNo", "CarierType",
                "DeliveryRemarks", "TCPRegNo", "AddLessComments", "Certificate1", "Certificate2", "Remarks1", "Remarks2", "OtherRemarks1",
                "OtherRemarks2", "AttachmentsValues", "CustomAttachmentsValues"}) o.put(c, text(ci(r, c)));
        for (String c : new String[]{"FCurrencyAmount", "ConversionRate", "EquivalentAmount", "AddLessAmount", "TotalAmount", "NoOfContainers",
                "GrossMton", "GrossWeight", "NetMton", "NetWeight"}) o.put(c, asDouble(ci(r, c)));
        o.put("DocDate", iso(ci(r, "DocDate")));
        o.put("DocumentaryCreditNoIssueDate", iso(ci(r, "DocumentaryCreditNoIssueDate")));
        o.put("GDDate", iso(ci(r, "GDDate")));
        o.put("invoiceNos", idName(repo.finalInvoiceNos(u, text(ci(r, "InvoiceNo"))), "Id", "FinalInvoiceNo"));   // InvoiceNoBind(InvoiceNoDbCall(Obj.InvoiceNo))
        o.put("customer", customerLeave(u, asInt(o.get("SupplierCustomerId"))));
        o.put("containers", containers(u, id));
        o.putAll(children(id));
        return o;
    }

    // ================================================================== loader (LoadSalesContractForInvoice, DocumentTypeId 223)

    public Map<String, Object> loaderSetup() {
        UserAccount u = user("View");
        return ExportCommercialInvoiceService.loaderSetup(u, repo, fy());
    }

    public List<Map<String, Object>> loader(Map<String, Object> body) {
        UserAccount u = user("View");
        return ExportCommercialInvoiceService.loaderRows(u, repo, fy(), LOADER_DOCUMENT_TYPE_ID, body);
    }

    public Map<String, Object> loaderApply(String ids) {
        UserAccount u = user("View");
        return ExportCommercialInvoiceService.loaderApply(u, repo, ids);
    }

    // ================================================================== save (Insert)

    private static double round(double v, int scale) { return BigDecimal.valueOf(v).setScale(scale, RoundingMode.HALF_UP).doubleValue(); }

    /** FormHelper.ValidateControls for a TextBox Double / INT: unparseable or 0 -> "{field} must be a non-zero number". */
    private static void requireNumber(Object v, String field) {
        String s = text(v).trim();
        double d;
        try { d = Double.parseDouble(s.replace(",", "")); } catch (NumberFormatException e) { d = 0; }
        if (s.isEmpty() || d == 0) throw new IllegalArgumentException(field + " must be a non-zero number");
    }

    private static void requireInt(Object v, String field) {
        String s = text(v).trim();
        int d;
        try { d = Integer.parseInt(s); } catch (NumberFormatException e) { d = 0; }
        if (d == 0) throw new IllegalArgumentException(field + " must be a non-zero number");
    }

    private static void requireCombo(Object v, String field) { if (asInt(v) == 0) throw new IllegalArgumentException(field + " field is required"); }
    private static void requireText(Object v, String field) { if (text(v).trim().isEmpty()) throw new IllegalArgumentException(field + " field is required"); }

    /**
     * Insert() after the page's own checks and the "Are you sure to Save?/Update?" question. btnsave_Click sets RecId = 0
     * first (a Save after an Edit inserts a new invoice - kept); btnupdate_Click passes the loaded RecId.
     */
    public Map<String, Object> save(Map<String, Object> body) {
        int recId = asInt(body.get("recId"));
        UserAccount u = user(recId == 0 ? "Save" : "Update");
        Map<String, Object> h = map(body.get("header"));
        List<Map<String, Object>> detail = list(body.get("detail"));
        List<Map<String, Object>> removed = list(body.get("removed"));
        List<Map<String, Object>> detailCustom = list(body.get("detailCustom"));
        List<Map<String, Object>> removedCustom = list(body.get("removedCustom"));
        List<Map<String, Object>> paymentTerms = list(body.get("paymentTerms"));
        List<Map<String, Object>> paymentTermsCustom = list(body.get("paymentTermsCustom"));
        List<Map<String, Object>> otherCharges = list(body.get("otherCharges"));
        List<Map<String, Object>> otherChargesCustom = list(body.get("otherChargesCustom"));
        List<Map<String, Object>> otherItems = list(body.get("otherItems"));
        boolean f9 = repo.erpFeature(u, 9);

        if (detail.isEmpty()) throw new IllegalArgumentException("Item Detail Record Not Found");
        if (paymentTerms.isEmpty()) throw new IllegalArgumentException("Payment Detail Not Found. Please Check! ");
        if (detailCustom.isEmpty()) throw new IllegalArgumentException("Custom Item Detail Record Not Found");
        if (paymentTermsCustom.isEmpty()) throw new IllegalArgumentException("Custom Payment Detail Not Found. Please Check! ");

        if (f9) requireCombo(h.get("ExportCompanyId"), "Export Company");
        requireInt(h.get("DocCode"), "Doc No");
        requireText(h.get("InvoiceNo"), "Invoice No");
        requireText(h.get("DocumentaryCreditNo"), "Documentary Credit No");
        requireCombo(h.get("SupplierCustomerId"), "Customer");
        requireCombo(h.get("ConsigneeId"), "Consignee");
        requireCombo(h.get("LoadingPortId"), "Loading Port");
        requireCombo(h.get("DestinationPortId"), "Destination Port");
        requireCombo(h.get("CarierTypeId"), "Carrier Type");
        requireCombo(h.get("DeliveryTermId"), "Delivery Term");
        requireCombo(h.get("ContinentId"), "Continent");
        requireCombo(h.get("OriginCountryId"), "Origin");
        requireCombo(h.get("ImporterCountryId"), "Importer Country");
        requireCombo(h.get("PlaceOfDeliveryId"), "Delivery Place");
        requireCombo(h.get("FcurrencyId"), "FCY Code");
        requireNumber(h.get("ConversionRate"), "Exchange Rate");
        requireNumber(h.get("FCurrencyAmount"), "FCY Amount");
        requireNumber(h.get("EquivalentAmount"), "Local Amount");
        requireNumber(h.get("TotalAmount"), "Total Amount");
        requireNumber(h.get("NoOfContainers"), "No Of Container");
        requireNumber(h.get("GrossMton"), "Gross MTon");
        requireNumber(h.get("NetMton"), "Net MTon");

        /* UpdateAddLessAmountInHeaderFromChargesGrid + UpdateHeaderWeightAndAmountFromDetailGrid(false) */
        double addLess = 0;
        for (Map<String, Object> r : otherCharges) addLess += asDouble(r.get("AddAmount")) - asDouble(r.get("LessAmount"));
        addLess = round(addLess, 3);
        double mTon = 0, grossKg = 0, amount = 0, otherAmountGrid = 0;
        for (Map<String, Object> r : detail) { mTon += asDouble(r.get("QtyMTon")); grossKg += asDouble(r.get("GrossWeight")); amount += asDouble(r.get("Amount")); }
        for (Map<String, Object> r : otherItems) otherAmountGrid += asDouble(r.get("Amount"));
        double netTotal = round(amount + addLess + otherAmountGrid, 3);
        double netMtonBox = round(mTon, 5), grossMtonBox = grossKg > 0 ? round(grossKg / 1000.0, 5) : 0, fcyBox = round(amount, 3);
        double rate = asDouble(h.get("ConversionRate"));
        double localBox = (netTotal > 0 && rate > 0) ? Math.round(netTotal * rate) : 0;          // ToString("0,0")
        if (netTotal > 0) for (Map<String, Object> r : paymentTerms) r.put("FcyAmount", asDouble(r.get("PrcntOfTotal")) * netTotal / 100.0);
        if (grossMtonBox < netMtonBox) throw new IllegalArgumentException("Gross Weight must be equal to or greater than Net Weight. Thank you.");
        double pct = 0, pctC = 0;
        for (Map<String, Object> r : paymentTerms) pct += asDouble(r.get("PrcntOfTotal"));
        for (Map<String, Object> r : paymentTermsCustom) pctC += asDouble(r.get("PrcntOfTotal"));
        if (pct != 100.0 || pctC != 100.0) throw new IllegalArgumentException("Payment Percent Not Equal To 100%");
        /* CheckItemIds(dtdetail, dtPackingListDetail): the Packing List page is removed from tabControl2, its table is always empty. */

        Timestamp now = new Timestamp(System.currentTimeMillis());
        Map<String, Object> pih = ExportCommercialInvoiceService.blankHeader();
        int branch = u.getBranchesId() == null ? 0 : u.getBranchesId();
        pih.put("OrganizationId", u.getOrganizationId());
        pih.put("CompanyId", u.getCompanyId());
        pih.put("BranchesId", branch);
        pih.put("ProjectsId", branch);                                   // PIH.ProjectsId = UserAccount.BranchesId (sic)
        pih.put("FinancialYearId", fy());
        pih.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        pih.put("EntryDate", now);
        pih.put("ModifyDate", now);
        pih.put("EntryUser", u.getId());
        pih.put("ModifyUser", u.getId());
        pih.put("ApprovedDate", now);
        pih.put("IsApproved", false);
        pih.put("ExportCompanyId", asInt(h.get("ExportCompanyId")));
        pih.put("Id", recId);
        pih.put("DocCode", asInt(h.get("DocCode")));
        pih.put("DocDate", ts(h.get("DocDate")));
        pih.put("InvoiceNo", text(h.get("InvoiceNo")));
        pih.put("DocumentaryCreditNo", trim(h.get("DocumentaryCreditNo")));
        pih.put("DocumentaryCreditNoIssueDate", ts(h.get("DocumentaryCreditNoIssueDate")));
        pih.put("SupplierCustomerId", asInt(h.get("SupplierCustomerId")));
        pih.put("ConsigneeId", asInt(h.get("ConsigneeId")));
        pih.put("CustomerContractNos", text(h.get("CustomerContractNos")));
        pih.put("ContractDates", text(h.get("ContractDates")));
        pih.put("NotifyParty1", asInt(h.get("NotifyParty1")));
        pih.put("NotifyParty2", asInt(h.get("NotifyParty2")));
        pih.put("NotifyParty3", asInt(h.get("NotifyParty3")));
        pih.put("ImporterBankId", asInt(h.get("ImporterBankId")));
        pih.put("ExporteBankId", asInt(h.get("ExporteBankId")));
        pih.put("LotNoRef", text(h.get("LotNoRef")));
        pih.put("EFormNo", trim(h.get("GDNo")));                           // txtGdNo
        pih.put("EFormDate", ts(h.get("GDDate")));                         // txtGdDate
        pih.put("LoadingPortId", asInt(h.get("LoadingPortId")));
        pih.put("DestinationPortId", asInt(h.get("DestinationPortId")));
        pih.put("CarierType", text(h.get("CarierType")));
        pih.put("DeliveryTermId", asInt(h.get("DeliveryTermId")));
        pih.put("DeliveryRemarks", text(h.get("DeliveryRemarks")));
        pih.put("TCPRegNo", text(h.get("TCPRegNo")));
        pih.put("ContinentId", asInt(h.get("ContinentId")));
        pih.put("OriginCountryId", asInt(h.get("OriginCountryId")));
        pih.put("ImporterCountryId", asInt(h.get("ImporterCountryId")));
        pih.put("PlaceOfDeliveryId", asInt(h.get("PlaceOfDeliveryId")));
        pih.put("FcurrencyId", asInt(h.get("FcurrencyId")));
        pih.put("ConversionRate", rate);
        pih.put("FCurrencyAmount", fcyBox);
        pih.put("EquivalentAmount", localBox);
        pih.put("AddLessAmount", addLess);
        pih.put("AddLessComments", text(h.get("AddLessComments")));
        pih.put("TotalAmount", netTotal);
        pih.put("NoOfContainers", (double) asInt(h.get("NoOfContainers")));
        BigDecimal grossMton = BigDecimal.valueOf(grossMtonBox), netMton = BigDecimal.valueOf(netMtonBox);
        pih.put("GrossMton", grossMton);
        pih.put("GrossWeight", grossMton.multiply(BigDecimal.valueOf(1000)).doubleValue());
        pih.put("NetMton", netMton);
        pih.put("NetWeight", netMton.multiply(BigDecimal.valueOf(1000)).doubleValue());
        pih.put("Certificate1", trim(h.get("Certificate1")));
        pih.put("Certificate2", trim(h.get("Certificate2")));
        pih.put("Remarks1", text(h.get("Remarks1")));
        pih.put("Remarks2", text(h.get("Remarks2")));
        pih.put("OtherRemarks1", text(h.get("OtherRemarks1")));
        pih.put("OtherRemarks2", text(h.get("OtherRemarks2")));

        ExportCommercialInvoiceRepository.SaveSet set = new ExportCommercialInvoiceRepository.SaveSet();
        double totalFc = 0, utilize = 0, utilizeC = 0;
        double mtonPerContainer = netMtonBox / asDouble(h.get("NoOfContainers"));
        StringBuilder contractIds = new StringBuilder(), contractDetailIds = new StringBuilder(), contractNos = new StringBuilder(),
                contractDates = new StringBuilder(), scheduleIds = new StringBuilder(), scheduleNos = new StringBuilder();
        if (recId > 0) for (Map<String, Object> r : removed) { Map<String, Object> vd = detailModel(r, false); vd.put("Id", asInt(r.get("Id"))); vd.put("ActionTypeId", 3); set.packing.add(vd); }
        int i = 0;
        for (Map<String, Object> r : detail) {
            Map<String, Object> vd = detailModel(r, false);
            int vid = recId != 0 ? asInt(r.get("Id")) : 0;
            vd.put("Id", vid);
            vd.put("ActionTypeId", vid <= 0 ? 1 : 2);
            rowChecks(vd, i, "Detail Grid", "Detail Grid Row No");
            contractIds.append(',').append(text(r.get("ContractId")));
            contractDetailIds.append(',').append(text(r.get("ContractDetailId")));
            contractNos.append(',').append(text(r.get("ContractNo")));
            contractDates.append(',').append(shortDate(r.get("ContractDate")));
            scheduleIds.append(',').append(text(r.get("ContractScheduleId")));
            scheduleNos.append(',').append(text(r.get("ContractNoInvoiceWise")));
            totalFc += asDouble(vd.get("FcAmount"));
            vd.put("NoofContainers", asDouble(vd.get("MTon")) / mtonPerContainer);
            set.packing.add(vd);
            i++;
        }
        if (recId > 0) for (Map<String, Object> r : removedCustom) { Map<String, Object> vd = detailModel(r, true); vd.put("Id", asInt(r.get("Id"))); vd.put("ActionTypeId", 3); set.packingCustom.add(vd); }
        i = 0;
        for (Map<String, Object> r : detailCustom) {
            Map<String, Object> vd = detailModel(r, true);
            int vid = recId != 0 ? asInt(r.get("Id")) : 0;
            vd.put("Id", vid);
            vd.put("ActionTypeId", vid <= 0 ? 1 : 2);
            rowChecks(vd, i, "Detail Custom Grid", "Custom Detail Grid Row No");
            set.packingCustom.add(vd);
            i++;
        }
        i = 0;
        for (Map<String, Object> r : paymentTerms) {
            Map<String, Object> d = paymentTermModel(r, i + 1);
            validateField(asInt(d.get("PaymentTermId")), "Payment Term", i, "Payment Detail");
            if (asInt(d.get("PaymentTermId")) == 1) validateField(asInt(d.get("ExImEFormRegistrationId")), "Financial Instrument number", i, "Payment Detail");
            validateField(asDouble(d.get("FcyAmount")), "FcyAmount", i, "Payment Detail");
            utilize += asDouble(d.get("FcyAmount"));
            set.paymentTerms.add(d);
            i++;
        }
        if (netRound(netTotal) != netRound(utilize)) throw new IllegalArgumentException("Total Invoice Amount and Total Payment Utilize Amount Not equal Please Check");
        i = 0;
        for (Map<String, Object> r : paymentTermsCustom) {
            Map<String, Object> d = paymentTermModel(r, i + 1);
            validateField(asInt(d.get("PaymentTermId")), "Payment Term", i, "Payment DetailCustom");
            if (asInt(d.get("PaymentTermId")) == 1) validateField(asInt(d.get("ExImEFormRegistrationId")), "Financial Instrument number", i, "Payment DetailCustom");
            validateField(asDouble(d.get("FcyAmount")), "FcyAmount", i, "Payment DetailCustom");
            utilizeC += asDouble(d.get("FcyAmount"));
            set.paymentTermsCustom.add(d);
            i++;
        }
        double customTotal = 0, customAmount = 0, addLessC = 0;
        for (Map<String, Object> r : detailCustom) customAmount += asDouble(r.get("Amount"));
        for (Map<String, Object> r : otherChargesCustom) addLessC += asDouble(r.get("AddAmount")) - asDouble(r.get("LessAmount"));
        customTotal = round(customAmount + round(addLessC, 3), 3);       // txtTotalFcyAmountHeaderCustom
        if (netRound(customTotal) != netRound(utilizeC)) throw new IllegalArgumentException("Total Invoice Amount and Total Payment Utilize Amount Not equal in Custom Please Check");
        i = 0;
        for (Map<String, Object> r : otherCharges) {
            Map<String, Object> d = otherChargeModel(r, false);
            validateField(asInt(d.get("ChargesItemId")), "Charges Item", i, "Other Charges");
            if (asDouble(d.get("AddAmount")) == 0 && asDouble(d.get("LessAmount")) == 0)
                throw new IllegalArgumentException("AddAmount Or Less Amount is required in Other Charges at row No: " + (i + 1));
            set.otherCharges.add(d);
            i++;
        }
        i = 0;
        for (Map<String, Object> r : otherChargesCustom) {
            Map<String, Object> d = otherChargeModel(r, true);
            validateField(asInt(d.get("ChargesItemId")), "Charges Item", i, "Other Charges Custom");
            if (asDouble(d.get("AddAmount")) == 0 && asDouble(d.get("LessAmount")) == 0)
                throw new IllegalArgumentException("AddAmount Or Less Amount is required in Other Charges Custom at row No: " + (i + 1));
            set.otherChargesCustom.add(d);
            i++;
        }
        i = 0;
        double otherAmount = 0;
        for (Map<String, Object> r : otherItems) {
            Map<String, Object> d = otherItemModel(r);
            validateField(asInt(d.get("otherItemId")), "Item Name", i, "Other Item");
            validateField(d.get("oItemQty"), "Qty", i, "Other Item");
            validateField(asDouble(d.get("oItemRate")), "Rate", i, "Other Item");
            validateField(d.get("oItemAmount"), "Amount", i, "Other Item");
            otherAmount += asDouble(d.get("oItemAmount"));
            set.otherItems.add(d);
            i++;
        }
        pih.put("ContractIds", distinctJoin(contractIds.toString()));
        pih.put("ContractDetailIds", distinctJoin(contractDetailIds.toString()));
        pih.put("ContractNos", distinctJoin(contractNos.toString()));
        pih.put("ContractDates", distinctJoin(contractDates.toString()));
        pih.put("ContractScheduleIds", distinctJoin(scheduleIds.toString()));
        pih.put("ContractScheduleNos", distinctJoin(scheduleNos.toString()));
        pih.put("PaymentTermId", asInt(set.paymentTerms.get(0).get("PaymentTermId")));
        pih.put("TotalAmount", totalFc + addLess + otherAmount);
        pih.put("AttachmentsValues", text(h.get("AttachmentsValues")));
        pih.put("CustomAttachmentsValues", text(h.get("CustomAttachmentsValues")));
        pih.put("ActionId", recId == 0 ? 1 : 2);

        if (asBool(repo.config(u, "FinancialisActiveonCommercialInvoice"))) {
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

    /** The FormHelper.ValidateField sequence and the 40 % gross-weight rule of Insert() for one (custom) detail row. */
    private static void rowChecks(Map<String, Object> vd, int i, String grid, String rowLabel) {
        boolean custom = grid.contains("Custom");
        validateField(asInt(vd.get("ContractId")), "Contract No", i, grid);
        validateField(asInt(vd.get("ContractScheduleId")), "Contract Schedule No", i, grid);
        validateField(asInt(vd.get("ItemId")), "Item", i, grid);
        validateField(asInt(vd.get("CropYearId")), "CropYear", i, grid);
        validateField(asInt(vd.get("PackingMaterialTypeId")), "PackingType", i, grid);
        validateField(asDouble(vd.get("MTon")), "Qty/M.Ton", i, grid);
        validateField(asInt(vd.get("OuterQtyUomId")), "Outer Uom", i, grid);
        validateField(asDouble(vd.get("OuterQty")), "OuterQty", i, grid);
        if (custom) grossCheck(vd, i, rowLabel);
        validateField(asDouble(vd.get("RatePrice")), "Rate", i, grid);
        validateField(asInt(vd.get("RateUomId")), "Rate Uom", i, grid);
        validateField(asDouble(vd.get("FcAmount")), "Amount", i, grid);
        if (!custom) grossCheck(vd, i, rowLabel);
    }

    private static void grossCheck(Map<String, Object> vd, int i, String rowLabel) {
        double gross = asDouble(vd.get("GrossWeight")), net = asDouble(vd.get("NetWeight"));
        if (gross > net * 1.4) {
            double allowed = net * 1.4;
            throw new IllegalArgumentException("Gross Weight '" + csNum(gross) + "' cannot exceed 40% above the Net Weight '" + csNum(net) + "'. "
                    + "Allowed maximum is '" + String.format("%.2f", allowed) + "'. (" + rowLabel + ": " + (i + 1) + ")");
        }
    }

    /** FillDetailListCommonForInsertAndDelete - the non-virtual ExImInvoicePackingDetail(Custom) properties it sets. */
    private static Map<String, Object> detailModel(Map<String, Object> r, boolean custom) {
        Map<String, Object> vd = blankPacking(custom);
        vd.put("ContractId", asInt(r.get("ContractId")));
        vd.put("ContractDetailId", asInt(r.get("ContractDetailId")));
        vd.put("ContractNoShipmentWise", text(r.get("ContractNoInvoiceWise")));
        vd.put("ContractScheduleId", asInt(r.get("ContractScheduleId")));
        vd.put("ContractScheduleDetailId", asInt(r.get("ContractScheduleDetailId")));
        vd.put("ItemId", asInt(r.get("ItemId")));
        vd.put("CropYearId", asInt(r.get("CropYearId")));
        vd.put("JobLotId", asInt(r.get("JobLotId")));
        vd.put("PackingMaterialTypeId", asInt(r.get("PackingTypeId")));
        vd.put("PmItemId", asInt(r.get("PmItemId")));
        vd.put("BrandId", asInt(r.get("BrandId")));
        double mton = asDouble(r.get("QtyMTon"));
        vd.put("MTon", mton);
        vd.put("NetWeight", mton * 1000);
        vd.put("OuterQtyUomId", asInt(r.get("OuterUomId")));
        vd.put("OuterQty", asDouble(r.get("OuterQty")));
        vd.put("GrossWeight", asDouble(r.get("GrossWeight")));
        vd.put("InnerQtyUomId", asInt(r.get("InnerUomId")));
        vd.put("InnerQty", asDouble(r.get("InnerQty")));
        vd.put("PalletQty", asDouble(r.get("PalletQty")));
        vd.put("RateWithoutAddLess", asDouble(r.get("RateWithoutAddLess")));
        vd.put("RateAddLess", asDouble(r.get("RateAddLess")));
        vd.put("RatePrice", asDouble(r.get("NetRate")));
        vd.put("RateUomId", asInt(r.get("RateUOMId")));
        vd.put("FcAmount", asDouble(r.get("Amount")));
        vd.put("LabInspectionId", asInt(r.get("LabInspectionId")));
        vd.put("BuyerLotNo", text(r.get("BuyerLotNo")));
        vd.put("ProductCode", text(r.get("ProductCode")));
        vd.put("BuyerHSCode", text(r.get("BuyerHSCode")));
        vd.put("HsCode", text(r.get("HSCode")));
        vd.put("ItemCommodityDetail", text(r.get("CommodityDetail")));
        vd.put("OuterPackDescription", text(r.get("PackingDetail")));
        vd.put("ItemDescriptionManual", text(r.get("PackingDetail")));      // sic: ItemDescriptionManual = PackingDetail
        vd.put("Remarks", text(r.get("Remarks")));
        vd.put("OtherAmount", asDouble(r.get("OtherItemAmount")));
        if (asDouble(vd.get("GrossWeight")) == 0) vd.put("GrossWeight", mton * 1000);
        return vd;
    }
}
