package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportSalesContractRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.ExportGSupport.*;

/**
 * The BLL side of 209 FrmExportSalesContract "Export Contract" (Architecture.WinApp.Export),
 * DocumentTypeId 202, ClientSize 1234 x 711. Form | History. The form: a "Main" header, the
 * tabControl2 pages Contract Detail / PaymentDetail / Packing Material (PMRP) / Other Items /
 * Other Charges with an entry panel and a grid each, Save / Save As / Update / 501-Print / 501A-Print.
 *
 * Rights: View to load, Save (btnsave, BtnSaveAs), Update (btnUpdate), Delete (BtnDelete - the
 * desktop handler is empty), Print, CanViewAllRecord (history EntryUser filter). Organisation,
 * company, financial year and the audit user come from the session, never from the request.
 */
@Service
public class ExportSalesContractService {

    public static final int SCREEN_ID = 209;
    public static final int DOCUMENT_TYPE_ID = 202;

    @Autowired private ExportSalesContractRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights rights;

    private UserAccount user(String action) {
        UserAccount u = currentUserContext.requireAccountingUser();
        rights.require(u, SCREEN_ID, action);
        return u;
    }

    private boolean allowed(UserAccount u, String action) {
        try { rights.require(u, SCREEN_ID, action); return true; }
        catch (AccessDeniedException ex) { return false; }
    }

    private int financialYearId() { return currentUserContext.currentFinancialYearId(); }

    // ================================================================= load / refresh

    /**
     * FrmExportSalesContract_Load: rights, ERP features 9 (Export Company) and 12 (Document Custom
     * Group), the four configurations, GenerateCode, every combo, LastSavedRecord defaults,
     * ItemAndPMItemMap, the history Customer combo and From date.
     */
    public Map<String, Object> setup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, "Save"));
        perm.put("Update", allowed(u, "Update"));
        perm.put("Delete", allowed(u, "Delete"));
        perm.put("Print", allowed(u, "Print"));
        out.put("permissions", perm);
        boolean multi = false, sdf = false;
        try { multi = repo.erpFeature(u, 9); sdf = repo.erpFeature(u, 12); } catch (Exception e) { /* feature list unavailable -> both off */ }
        out.put("allowExportMultiCompanies", multi);
        out.put("shipmentDocumentsFeature", sdf);
        out.put("config", config(u));
        out.put("docNo", generateCode(u));
        out.putAll(combos(u, multi, sdf, true));
        try { out.put("lastSaved", lastSaved(u)); } catch (Exception e) { out.put("lastSaved", null); }
        try { out.put("itemPmMap", itemPmMap(u)); } catch (Exception e) { out.put("itemPmMap", new ArrayList<>()); }
        try { out.put("historyCustomers", historyCustomers(u)); } catch (Exception e) { out.put("historyCustomers", new ArrayList<>()); }
        return out;
    }

    /** AllItemsBindonExportContract, PMGridInfoShouldNotCompulsoryOnContract, DefaultDaysToLessFromHistoryFromDate, ItemSearchByCode, DefaultNoOfDecimalPointsForFcyAmount. */
    private Map<String, Object> config(UserAccount u) {
        Map<String, Object> c = new LinkedHashMap<>();
        c.put("allItemsBindonExportContract", asBool(repo.config(u, "AllItemsBindonExportContract")));
        c.put("pmGridInfoShouldNotCompulsoryOnContract", asBool(repo.config(u, "PMGridInfoShouldNotCompulsoryOnContract")));
        c.put("defaultDaysToLessFromHistoryFromDate", asInt(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        c.put("itemSearchByCode", asBool(repo.config(u, "ItemSearchByCode")));
        c.put("fcyDecimals", asInt(repo.config(u, "DefaultNoOfDecimalPointsForFcyAmount")));
        return c;
    }

    /** GenerateCode() - the next LcOrderDocNo for 202 in the active year. */
    public int generateCode(UserAccount u) {
        try { return repo.generateCode(u, DOCUMENT_TYPE_ID, financialYearId()); } catch (Exception e) { return 0; }
    }

    public Map<String, Object> generateCodeOnly() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("docNo", generateCode(user("View")));
        return out;
    }

    /**
     * toolStripButton1_Click (the toolbar Refresh / Ctrl+R): every combo again, the PM configuration,
     * ItemAndPMItemMap and the LastSavedRecord defaults. btnRefresh_Click (not wired to a button) is
     * the same set without the packing items / farming re-read - the page applies the full set.
     */
    public Map<String, Object> refresh() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        boolean multi = false, sdf = false;
        try { multi = repo.erpFeature(u, 9); sdf = repo.erpFeature(u, 12); } catch (Exception e) { /* off */ }
        out.put("allowExportMultiCompanies", multi);
        out.put("shipmentDocumentsFeature", sdf);
        out.put("config", config(u));
        out.putAll(combos(u, multi, sdf, false));
        try { out.put("lastSaved", lastSaved(u)); } catch (Exception e) { out.put("lastSaved", null); }
        try { out.put("itemPmMap", itemPmMap(u)); } catch (Exception e) { out.put("itemPmMap", new ArrayList<>()); }
        return out;
    }

    private Map<String, Object> combos(UserAccount u, boolean multi, boolean sdf, boolean initial) {
        Map<String, Object> out = new LinkedHashMap<>();
        boolean allItems = asBool(repo.config(u, "AllItemsBindonExportContract"));
        put(out, "customers", () -> pairsOf(repo.customersForExport(u), "Id", "CompanyName"));
        put(out, "salesPersons", () -> pairsOf(repo.supplierCustomerByGroup(u, "9"), "Id", "CompanyName"));
        put(out, "currencies", () -> pairsOf(repo.currencies(u), "Id", "CurrencyCode"));
        put(out, "deliveryTerms", () -> rows(repo.deliveryTerms(), "Id", "Code", "Description"));
        put(out, "paymentTerms", () -> rows(repo.paymentTerms(u), "Id", "lcOrderTerm", "lcOrderTermDesc"));
        put(out, "ports", () -> pairsOf(repo.seaPorts(u), "Id", "PortName"));
        put(out, "banks", () -> banks(u));
        put(out, "items", () -> items(u, allItems));
        put(out, "packTypes", () -> pairsOf(repo.packTypes(u), "Id", "Description"));
        put(out, "cropYears", () -> pairsOf(repo.cropYears(u), "Id", "CropYear"));
        put(out, "exportCharges", () -> pairsOf(repo.exportCharges(u), "ExportChargesID", "ExportChargesName"));
        put(out, "otherItems", () -> items14(u));
        put(out, "pmItems", () -> items14(u));
        put(out, "farmingNTrade", () -> farming(u));
        if (multi) put(out, "exportCompanies", () -> pairsOf(repo.exportCompanies(u), "Id", "CompName"));
        if (sdf) put(out, "customGroups", () -> customGroups(u, 0));
        /* ItemSpecificationsBind at load runs with ItemId 0 / Customer 0 - the procedure's answer for that is kept. */
        put(out, "commodityRemarks", () -> commodityRemarks(u, 0, 0));
        return out;
    }

    private interface Src { Object get(); }
    private static void put(Map<String, Object> out, String key, Src s) {
        try { out.put(key, s.get()); } catch (Exception e) { out.put(key, new ArrayList<>()); out.put(key + "Error", msg(e)); }
    }

    private static List<Map<String, Object>> pairsOf(List<Map<String, Object>> src, String idCol, String nameCol) {
        return pairs(src, idCol, nameCol);
    }

    private static List<Map<String, Object>> rows(List<Map<String, Object>> src, String idCol, String nameCol, String descCol) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : src) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, idCol)));
            m.put("name", text(ci(r, nameCol)));
            m.put("desc", text(ci(r, descCol)));
            out.add(m);
        }
        return out;
    }

    /** ImporterandExportBankFill: Bank.GetAll split by IsHomeland "Foreign Country" / "Home Country". */
    private Map<String, Object> banks(UserAccount u) {
        List<Map<String, Object>> foreign = new ArrayList<>(), home = new ArrayList<>();
        for (Map<String, Object> r : repo.banks(u)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("name", text(ci(r, "BranchName")));
            String h = text(ci(r, "IsHomeland"));
            if ("Foreign Country".equals(h)) foreign.add(m);
            else if ("Home Country".equals(h)) home.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("importer", foreign);
        out.put("exporter", home);
        return out;
    }

    /** ItemDetailFill: Item.ReadAllForExportCombo, or every allocated item when AllItemsBindonExportContract. */
    private List<Map<String, Object>> items(UserAccount u, boolean allItems) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : allItems ? repo.itemsTwoColumnsWithParent(u) : repo.itemsForExportCombo(u)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("ItemName", text(ci(r, "ItemName")));
            m.put("ItemCode", text(ci(r, "ItemCode")));
            out.add(m);
        }
        return out;
    }

    /** CommonServices.GetItemByItemTypeId("14") - Id, ItemName, ItemCodeNew. */
    private List<Map<String, Object>> items14(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.itemsByType14(u)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("ItemName", text(ci(r, "ItemName")));
            m.put("ItemCode", text(ci(r, "ItemCodeNew")));
            out.add(m);
        }
        return out;
    }

    private List<Map<String, Object>> farming(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.farmingNTrade()) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("name", text(ci(r, "FarmingNTrade")));
            m.put("ExImFarmingTypeId", asInt(ci(r, "ExImFarmingTypeId")));
            m.put("ExImTradeTypeId", asInt(ci(r, "ExImTradeTypeId")));
            out.add(m);
        }
        return out;
    }

    /** DocumentcustomGroupBind(customer) - CustomGroupId, CustomGroup. */
    public List<Map<String, Object>> customGroups(UserAccount u, int customerId) {
        return pairs(repo.customGroupsByCustomer(u, customerId), "CustomGroupId", "CustomGroup");
    }

    public List<Map<String, Object>> customGroupsApi(int customerId) { return customGroups(user("View"), customerId); }

    /** ItemSpecificationsBind: CommodityDetailItemCustomerWise.GetRemarks(item, customer) - Id, Remarks. */
    public List<Map<String, Object>> commodityRemarks(UserAccount u, int itemId, int customerId) {
        return pairs(repo.commodityRemarks(u, itemId, customerId), "Id", "Remarks");
    }

    public List<Map<String, Object>> commodityRemarksApi(int itemId, int customerId) { return commodityRemarks(user("View"), itemId, customerId); }

    /** LastSavedRecord(org, comp, 0): SalesPersonId, FcurrencyId, CropYearId, InvPackingMaterialTypeId of the first row. */
    private Map<String, Object> lastSaved(UserAccount u) {
        List<Map<String, Object>> r = repo.lastSavedRecord(u, 0);
        if (r.isEmpty()) return null;
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("SalesPersonId", asInt(ci(r.get(0), "SalesPersonId")));
        m.put("FcurrencyId", asInt(ci(r.get(0), "FcurrencyId")));
        m.put("CropYearId", asInt(ci(r.get(0), "CropYearId")));
        m.put("InvPackingMaterialTypeId", asInt(ci(r.get(0), "InvPackingMaterialTypeId")));
        return m;
    }

    public Map<String, Object> lastSavedApi() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("lastSaved", lastSaved(user("View")));
        return out;
    }

    /** ItemAndPMItemMap_GetByItemIds: ItemId, PmItemId, PmItemCode, PmItemName, PmBaseUomId, PmBaseUomCode (AutoFillGridFromMapping). */
    private List<Map<String, Object>> itemPmMap(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.itemAndPmItemMap(u)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ItemId", asInt(ci(r, "ItemId")));
            m.put("PmItemId", asInt(ci(r, "PmItemId")));
            m.put("PmItemCode", text(ci(r, "PmItemCode")));
            m.put("PmItemName", text(ci(r, "PmItemName")));
            m.put("PmBaseUomId", asInt(ci(r, "PmBaseUomId")));
            m.put("PmBaseUomCode", text(ci(r, "PmBaseUomCode")));
            out.add(m);
        }
        return out;
    }

    /** HistoryCombosFill: GetDataForDropDownFromExportContract(Activity "Customer") -> Id, name. */
    private List<Map<String, Object>> historyCustomers(UserAccount u) {
        return pairs(repo.historyDropDowns(u, "Customer"), "Id", "name");
    }

    public Map<String, Object> historyCombos() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("historyCustomers", historyCustomers(user("View")));
        return out;
    }

    // ================================================================= item side lookups

    /** bindRateUomAndItemPackUom: GetUomScheduleByItemIdForExport - Id, UOMCode, Equivalent, QtyEquivalent, BaseRateUom. */
    public List<Map<String, Object>> uomForExport(int itemId) {
        UserAccount u = user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.uomForExport(u, itemId)) out.add(uomRow(r));
        return out;
    }

    /** BindPmItemPackUom: GetUomScheduleByItemId (Sp_UOMSchedule_GetAllMethod 'ReadByItemID'). */
    public List<Map<String, Object>> uomByItem(int itemId) {
        UserAccount u = user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.uomByItem(u, itemId)) out.add(uomRow(r));
        return out;
    }

    static Map<String, Object> uomRow(Map<String, Object> r) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(ci(r, "Id")));
        m.put("UOMCode", text(ci(r, "UOMCode")));
        m.put("Equivalent", asDouble(ci(r, "Equivalent")));
        m.put("QtyEquivalent", asDouble(ci(r, "QtyEquivalent")));
        m.put("BaseRateUom", asBool(ci(r, "BaseRateUom")));
        return m;
    }

    /** GetPmItemRate: InventoryStockEvalautionDetail.GetLastRateByItemId. */
    public Map<String, Object> lastRate(int itemId) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rate", repo.lastRateByItemId(u, itemId));
        return out;
    }

    /** txtFactoryLoadingDate_ValueChanged: the scheduling policy intervals. */
    public Map<String, Object> schedulingPolicy() {
        user("View");
        List<Map<String, Object>> r = repo.schedulingPolicy();
        Map<String, Object> out = new LinkedHashMap<>();
        if (!r.isEmpty()) {
            out.put("ProductionInterval", asInt(ci(r.get(0), "ProductionInterval")));
            out.put("InspectionInterval", asInt(ci(r.get(0), "InspectionInterval")));
            out.put("PackMaterialInterval", asInt(ci(r.get(0), "PackMaterialInterval")));
        }
        return out;
    }

    /** txtLcOrderNo_Leave: GetIdByDocNo(OrderNo = the typed Export So No) -> the Id of the first row or 0. */
    public Map<String, Object> idByDocNo(String docNo) {
        UserAccount u = user("View");
        List<Map<String, Object>> r = repo.idByDocNo(u, text(docNo), financialYearId());
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", r.isEmpty() ? 0 : asInt(ci(r.get(0), "Id")));
        return out;
    }

    // ================================================================= ReadById

    /** ReadById(ID): the header and the five grids of ExImLcOrder.GetByID. */
    public Map<String, Object> readById(int id) {
        user("View");
        List<Map<String, Object>> hdr = repo.header(id);
        if (hdr.isEmpty()) throw new IllegalArgumentException("Record not found");
        Map<String, Object> h = hdr.get(0);
        int docType = asInt(ci(h, "DocumentTypeId"));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", header(h));
        out.put("details", detailRows(repo.detail(id, docType)));
        out.put("otherItems", otherItemRows(repo.otherItems(id)));
        out.put("paymentTerms", paymentRows(repo.paymentTermsDetail(id)));
        out.put("pmRows", pmRows(repo.packingMaterialDetail(id)));
        out.put("otherCharges", chargeRows(repo.otherChargesDetail(id)));
        return out;
    }

    static Map<String, Object> header(Map<String, Object> r) {
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("Id", asInt(ci(r, "Id")));
        h.put("Status", text(ci(r, "Status")));
        h.put("LcOrderDocNo", asInt(ci(r, "LcOrderDocNo")));
        h.put("LcOrderDate", iso(ci(r, "LcOrderDate")));
        h.put("SalesPersonId", asInt(ci(r, "SalesPersonId")));
        h.put("SupCustId", asInt(ci(r, "SupCustId")));
        h.put("ConsigneeId", asInt(ci(r, "ConsigneeId")));
        h.put("NotifyPartyId", asInt(ci(r, "NotifyPartyId")));
        h.put("NotifyParty2Id", asInt(ci(r, "NotifyParty2Id")));
        h.put("NotifyParty3Id", asInt(ci(r, "NotifyParty3Id")));
        h.put("LotReference", text(ci(r, "LotReference")));
        h.put("LcOrderNo", text(ci(r, "LcOrderNo")));
        h.put("CustomerOrderNo", text(ci(r, "CustomerOrderNo")));
        h.put("SalesContratDate", iso(ci(r, "SalesContratDate")));
        h.put("ShipmentStartDate", iso(ci(r, "ShipmentStartDate")));
        h.put("LastShipmentDate", iso(ci(r, "LastShipmentDate")));
        h.put("DeliveryTermId", asInt(ci(r, "DeliveryTermId")));
        h.put("PaymentTermId", asInt(ci(r, "PaymentTermId")));
        h.put("LoadingPortId", asInt(ci(r, "LoadingPortId")));
        h.put("DestinationPortId", asInt(ci(r, "DestinationPortId")));
        h.put("ImporterBankId", asInt(ci(r, "ImporterBankId")));
        h.put("ExporterBankId", asInt(ci(r, "ExporterBankId")));
        h.put("ExportCompanyId", asInt(ci(r, "ExportCompanyId")));
        h.put("NoOfContainers", asDouble(ci(r, "NoOfContainers")));
        h.put("FcurrencyId", asInt(ci(r, "FcurrencyId")));
        h.put("FCurrencyAmount", asDouble(ci(r, "FCurrencyAmount")));
        h.put("NetWeightKgs", asDouble(ci(r, "NetWeightKgs")));
        h.put("DeliveryRemarks", text(ci(r, "DeliveryRemarks")));
        h.put("PaymentRemarks", text(ci(r, "PaymentRemarks")));
        h.put("CommodityDetial", text(ci(r, "CommodityDetial")));
        h.put("RemarksHeader", text(ci(r, "RemarksHeader")));
        h.put("ProductSpecification", text(ci(r, "ProductSpecification")));
        h.put("ConversionRate", asDouble(ci(r, "ConversionRate")));
        h.put("DeliveryDays", asInt(ci(r, "DeliveryDays")));
        h.put("CustomerApprovalDate", iso(ci(r, "CustomerApprovalDate")));
        h.put("CustomerApprovalRemarks", text(ci(r, "CustomerApprovalRemarks")));
        h.put("FactoryLoadingDate", iso(ci(r, "FactoryLoadingDate")));
        h.put("CustomGroupId", asInt(ci(r, "CustomGroupId")));
        h.put("ContractType", asInt(ci(r, "ContractType")));
        h.put("DocumentTypeId", asInt(ci(r, "DocumentTypeId")));
        h.put("AttachmentsValues", text(ci(r, "AttachmentsValues")));
        h.put("CustomAttachmentsValues", text(ci(r, "CustomAttachmentsValues")));
        return h;
    }

    /** dtdetail rows of ReadById (desktop column names, "Qty/M.Ton" as QtyMTon). */
    static List<Map<String, Object>> detailRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(d, "Id")));
            m.put("ItemCode", text(ci(d, "ItemCode")));
            m.put("ItemId", asInt(ci(d, "ExImItemId")));
            m.put("ItemName", text(ci(d, "ItemName")));
            m.put("PackTypeId", asInt(ci(d, "InvPackingMaterialTypeId")));
            m.put("PackType", text(ci(d, "ExImPackMaterialTypeName")));
            m.put("CropYearId", asInt(ci(d, "CropYearId")));
            m.put("CropYear", text(ci(d, "CropYear")));
            m.put("QtyMTon", asDouble(ci(d, "NetWeight")) / 1000.0);
            m.put("PackSizeId", asInt(ci(d, "UOMScheduleIdOuter")));
            m.put("PackSize", text(ci(d, "PackUom")));
            m.put("QtyEquivalent", asDouble(ci(d, "QtyEquivalent")));
            m.put("NoOfBags", asDouble(ci(d, "NoOfUnit")));
            m.put("Rate", asDouble(ci(d, "RatePrice")));
            m.put("RateUOMId", asInt(ci(d, "UOMScheduleIdRate")));
            m.put("RateUOM", text(ci(d, "RateUom")));
            m.put("Amount", asDouble(ci(d, "TotalAmount")));
            m.put("OtherItemAmount", 0.0);
            m.put("CommodityDetail", text(ci(d, "CommodityDetail")));
            m.put("ExImFarmingNTradeId", asInt(ci(d, "ExImFarmingNTradeId")));
            m.put("FarmingNTrade", text(ci(d, "FarmingNTrade")));
            m.put("ExImFarmingTypeId", asInt(ci(d, "ExImFarmingTypeId")));
            m.put("ExImTradeTypeId", asInt(ci(d, "ExImTradeTypeId")));
            m.put("Remarks", text(ci(d, "Other_Description")));
            /* the extra columns the III form carries */
            m.put("PmItemId", asInt(ci(d, "PmItemId")));
            m.put("PmItemCode", text(ci(d, "PmItemCode")));
            m.put("PmItemName", text(ci(d, "PmItemName")));
            m.put("BrandId", asInt(ci(d, "ItemBrandId")));
            m.put("BrandCode", text(ci(d, "BrandCode")));
            m.put("BrandName", text(ci(d, "BrandName")));
            m.put("OuterEquivalent", asDouble(ci(d, "UOMScheduleIdOuterName")));
            m.put("InnerQty", asDouble(ci(d, "InnerQty")));
            m.put("InnerUomId", asInt(ci(d, "UOMScheduleIdInner")));
            m.put("InnerUom", text(ci(d, "InnerUom")));
            m.put("InnerEquivalent", asDouble(ci(d, "UOMScheduleIdInnerName")));
            m.put("PalletQty", asDouble(ci(d, "PalletQty")));
            m.put("RateEquivalent", asDouble(ci(d, "UOMScheduleIdRateName")));
            m.put("PackingDetail", text(ci(d, "PackingDetail")));
            out.add(m);
        }
        return out;
    }

    static List<Map<String, Object>> otherItemRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ItemId", asInt(ci(d, "otherItemId")));
            m.put("ItemName", text(ci(d, "ItemName")));
            m.put("Qty", asDouble(ci(d, "oItemQty")));
            m.put("Rate", asDouble(ci(d, "oItemRate")));
            m.put("Amount", asDouble(ci(d, "oItemAmount")));
            m.put("Remarks", text(ci(d, "OtherItemRemarks")));
            out.add(m);
        }
        return out;
    }

    static List<Map<String, Object>> paymentRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("PaymentTermId", asInt(ci(d, "PaymentTermId")));
            m.put("PaymentTerm", text(ci(d, "LcOrderTerm")));
            m.put("PrcntOfTotal", asDouble(ci(d, "PrcntOfTotal")));
            m.put("FcyAmount", asDouble(ci(d, "FcyAmount")));
            m.put("Remarks", text(ci(d, "Remarks")));
            out.add(m);
        }
        return out;
    }

    static List<Map<String, Object>> pmRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(d, "Id")));
            m.put("BrandId", asInt(ci(d, "BrandId")));
            m.put("BrandCode", text(ci(d, "BrandCode")));
            m.put("BrandName", text(ci(d, "BrandName")));
            m.put("BrandUomId", asInt(ci(d, "BrandUomId")));
            m.put("BrandUom", text(ci(d, "BrandUomCode")));
            m.put("PackingTypeId", asInt(ci(d, "PackingTypeId")));
            m.put("PackingType", text(ci(d, "PackingType")));
            m.put("BrandOuterQty", asDouble(ci(d, "BrandOuterQty")));
            m.put("BrandInnerQty", asDouble(ci(d, "BrandInnerQty")));
            m.put("PmItemId", asInt(ci(d, "ItemId")));
            m.put("PmItemCode", text(ci(d, "ItemCode")));
            m.put("PmItemName", text(ci(d, "ItemName")));
            m.put("PmItemUomId", asInt(ci(d, "ItemUomId")));
            m.put("PmItemUom", text(ci(d, "ItemUomCode")));
            m.put("PmItemOuterQty", asDouble(ci(d, "Qty")));
            m.put("PmItemInnerQty", asDouble(ci(d, "InnerQty")));
            m.put("Rate", asDouble(ci(d, "Rate")));
            m.put("Amount", asDouble(ci(d, "Amount")));
            m.put("Remarks", text(ci(d, "pmRemarks")));
            out.add(m);
        }
        return out;
    }

    static List<Map<String, Object>> chargeRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(d, "Id")));
            m.put("ChargesItemId", asInt(ci(d, "ChargesItemId")));
            m.put("ChargesItem", text(ci(d, "ChargesItemName")));
            m.put("AddAmount", asDouble(ci(d, "AddAmount")));
            m.put("LessAmount", asDouble(ci(d, "LessAmount")));
            m.put("Remarks", text(ci(d, "Remarks")));
            out.add(m);
        }
        return out;
    }

    // ================================================================= save

    /**
     * Insert(): FormValidation (the desktop texts, in order), the FcAmount / NetWeight cross-check,
     * "Payment Percent Not Equal To 100%", CheckBomHeaderIds, the header model, the removed rows
     * (ActionTypeId 3, update only), the detail rows (1 / 2), other items, payment terms (PaymentTermId
     * = the first row's), the PM rows with their row validations, PmGridOuter/InnerQtyValidation against
     * dtBrandForPMItem rebuilt from the detail grid, other charges; then ExImLcOrder.Save.
     * "Record Save Successfully" / "Record Update Successfully".
     */
    public Map<String, Object> save(Map<String, Object> b) {
        int recId = asInt(b.get("recId"));
        UserAccount u = user(recId > 0 ? "Update" : "Save");
        Map<String, Object> h = map(b.get("header"));
        boolean multi = asBool(b.get("allowExportMultiCompanies"));
        boolean pmNotCompulsory = asBool(repo.config(u, "PMGridInfoShouldNotCompulsoryOnContract"));

        /* FormValidation() */
        if (multi && asInt(h.get("ExportCompanyId")) == 0) throw new IllegalArgumentException("Export Company Is required");
        String soNo = text(h.get("LcOrderDocNo"));
        if (soNo.isEmpty() || "0".equals(soNo)) throw new IllegalArgumentException("Export So No Is required");
        if (asInt(h.get("SalesPersonId")) == 0) throw new IllegalArgumentException("Sale Person Is required");
        if (asInt(h.get("SupCustId")) == 0) throw new IllegalArgumentException("Customer Is required");
        if (text(h.get("LcOrderNo")).isEmpty()) throw new IllegalArgumentException("OrderNo Is required");
        if (text(h.get("SalesContratDate")).isEmpty()) throw new IllegalArgumentException("OrderDate Is required");
        if (!(asDouble(h.get("NetWeightKgs")) > 0)) throw new IllegalArgumentException("M.Ton Must Be Greater Than 0.");
        if (asInt(h.get("NoOfContainers")) <= 0) throw new IllegalArgumentException("No. of Containers Must Be Greater Than 0.");
        if (asInt(h.get("FcurrencyId")) == 0) throw new IllegalArgumentException("Fcy Code Is required");
        if (asDouble(h.get("ConversionRate")) == 0) throw new IllegalArgumentException("Exchange Rate Is required");
        if (!(asDouble(h.get("FCurrencyAmount")) > 0)) throw new IllegalArgumentException("Fcy Amount Must Be Greater Than 0.");
        if (asInt(h.get("DeliveryDays")) == 0) throw new IllegalArgumentException("Delivery Days Is required");
        if (asInt(h.get("DeliveryTermId")) == 0) throw new IllegalArgumentException("Delivery Term Is required");
        if (asInt(h.get("LoadingPortId")) == 0) throw new IllegalArgumentException("Loading Port Is required");
        if (asInt(h.get("DestinationPortId")) == 0) throw new IllegalArgumentException("Destination Port  Is required");

        List<Map<String, Object>> details = list(b.get("details"));
        List<Map<String, Object>> removed = list(b.get("removedDetails"));
        List<Map<String, Object>> otherItems = list(b.get("otherItems"));
        List<Map<String, Object>> payments = list(b.get("paymentTerms"));
        List<Map<String, Object>> pm = list(b.get("pmRows"));
        List<Map<String, Object>> charges = list(b.get("otherCharges"));

        double total = 0, qty = 0, otherAmount = 0, pct = 0;
        for (Map<String, Object> r : details) { total += asDouble(r.get("Amount")); qty += asDouble(r.get("QtyMTon")); }
        for (Map<String, Object> r : otherItems) otherAmount += asDouble(r.get("Amount"));
        for (Map<String, Object> r : payments) pct += asDouble(r.get("PrcntOfTotal"));
        if (total + otherAmount != asDouble(h.get("FCurrencyAmount")) || qty != asDouble(h.get("NetWeightKgs")))
            throw new IllegalArgumentException("Please check FcAmount or NetWeight");
        if (pct < 100.0) throw new IllegalArgumentException("Payment Percent Not Equal To 100%");
        checkBomHeaderIds(details, pm, "Detail Grid");

        Map<String, Object> header = headerModel(u, h, recId, DOCUMENT_TYPE_ID);
        header.put("FinancialYearId", financialYearId());
        if (asBool(h.get("OnCommission"))) header.put("ContractType", 1);   /* ChkOnCommission (Visible = false) && FlagFin */

        List<Map<String, Object>> detailModels = new ArrayList<>();
        if (recId > 0) for (Map<String, Object> r : removed) { Map<String, Object> vd = detailModel(r, asInt(r.get("Id"))); vd.put("ActionTypeId", 3); detailModels.add(vd); }
        if (details.isEmpty()) throw new IllegalArgumentException("Detail Grid record not found");
        String remarksFromDetail = "";
        for (Map<String, Object> r : details) {
            int id = recId != 0 ? asInt(r.get("Id")) : 0;
            Map<String, Object> vd = detailModel(r, id);
            vd.put("ActionTypeId", id <= 0 ? 1 : 2);
            if (remarksFromDetail.isEmpty()) remarksFromDetail = text(vd.get("CommodityDetail"));
            detailModels.add(vd);
        }
        if (text(header.get("RemarksHeader")).isEmpty()) header.put("RemarksHeader", remarksFromDetail.isEmpty() ? null : remarksFromDetail);

        List<Map<String, Object>> otherModels = new ArrayList<>();
        for (Map<String, Object> r : otherItems) otherModels.add(otherItemModel(r));
        if (payments.isEmpty()) throw new IllegalArgumentException("Payment Term Grid record not found");
        List<Map<String, Object>> payModels = new ArrayList<>();
        for (Map<String, Object> r : payments) payModels.add(paymentModel(r));
        header.put("PaymentTermId", asInt(payments.get(0).get("PaymentTermId")));

        List<Map<String, Object>> pmModels = new ArrayList<>();
        if (!pm.isEmpty()) {
            int i = 0;
            for (Map<String, Object> r : pm) {
                pmModels.add(pmModel(r, recId != 0 ? asInt(r.get("Id")) : 0, i++, false));
            }
        } else if (!pmNotCompulsory) {
            throw new IllegalArgumentException("Packing Material Grid record not found");
        }
        pmGridValidation(brandForPmItem(details, "PackSizeId", "NoOfBags"), pm);

        List<Map<String, Object>> chargeModels = new ArrayList<>();
        int ci = 0;
        for (Map<String, Object> r : charges) { chargeModels.add(chargeModel(r, ci)); ci++; }

        int id = repo.saveContract(header, recId > 0, detailModels, otherModels, payModels, pmModels, chargeModels);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", id);
        out.put("message", recId == 0 ? "Record Save Successfully" : "Record Update Successfully");
        return out;
    }

    /** CheckBomHeaderIds: every PM Brand must be an item of the detail grid. */
    static void checkBomHeaderIds(List<Map<String, Object>> details, List<Map<String, Object>> pm, String gridName) {
        Set<Integer> detailIds = new HashSet<>();
        for (Map<String, Object> r : details) { int v = asInt(r.get("ItemId")); if (v > 0) detailIds.add(v); }
        Set<Integer> pmIds = new LinkedHashSet<>();
        for (Map<String, Object> r : pm) { int v = asInt(r.get("BrandId")); if (v > 0) pmIds.add(v); }
        pmIds.removeAll(detailIds);
        if (!pmIds.isEmpty()) {
            List<String> names = new ArrayList<>();
            for (Map<String, Object> r : pm) if (pmIds.contains(asInt(r.get("BrandId")))) names.add(text(r.get("BrandName")));
            throw new IllegalArgumentException("Brand Items '" + String.join(", ", names) + "' found in Packing Material Grid but not in " + gridName + ".");
        }
    }

    /**
     * FilldtBrandForPMItem: the detail grid grouped by (ItemId, PackTypeId, pack size id) - OuterQty =
     * sum of bags, InnerQty = sum of bags * the first row's QtyEquivalent, M_Ton = sum of Qty/M.Ton.
     * Returned as "brandId|uomId|packTypeId" -> {OuterQty, InnerQty}.
     */
    static Map<String, double[]> brandForPmItem(List<Map<String, Object>> details, String packSizeCol, String bagsCol) {
        Map<String, double[]> acc = new LinkedHashMap<>();
        Map<String, Double> firstEq = new HashMap<>();
        for (Map<String, Object> r : details) {
            String key = asInt(r.get("ItemId")) + "|" + asInt(r.get(packSizeCol)) + "|" + asInt(r.get("PackTypeId"));
            double bags = asDouble(r.get(bagsCol)), mton = asDouble(r.get("QtyMTon"));
            if (!acc.containsKey(key)) { acc.put(key, new double[] { bags, 0, mton }); firstEq.put(key, asDouble(r.get("QtyEquivalent"))); }
            else { double[] a = acc.get(key); a[0] += bags; a[2] += mton; }
        }
        for (Map.Entry<String, double[]> e : acc.entrySet()) e.getValue()[1] = e.getValue()[0] * firstEq.get(e.getKey());
        return acc;
    }

    /** PmGridOuterQtyValidation + PmGridInnerQtyValidation with the desktop's texts. */
    static void pmGridValidation(Map<String, double[]> brand, List<Map<String, Object>> pm) {
        if (pm.isEmpty()) return;
        for (int pass = 0; pass < 2; pass++) {
            boolean outer = pass == 0;
            Set<String> seen = new HashSet<>();
            for (Map<String, Object> row : pm) {
                String key = asInt(row.get("BrandId")) + "|" + asInt(row.get("BrandUomId")) + "|" + asInt(row.get("PackingTypeId"));
                if (seen.contains(key)) continue;
                double[] b = brand.get(key);
                String nm = text(row.get("BrandName")), uom = text(row.get("BrandUom")), pt = text(row.get("PackingType"));
                if (b == null)
                    throw new IllegalArgumentException("Brand " + (outer ? "outer" : "Inner") + " quantity for the selected combination (Brand Item '" + nm + "' with UOM '" + uom + "' and PackingType '" + pt + "') could not be found.");
                double brandQty = outer ? b[0] : b[1];
                double gridQty = 0;
                for (Map<String, Object> r : pm)
                    if ((asInt(r.get("BrandId")) + "|" + asInt(r.get("BrandUomId")) + "|" + asInt(r.get("PackingTypeId"))).equals(key))
                        gridQty += asDouble(r.get(outer ? "PmItemOuterQty" : "PmItemInnerQty"));
                if (gridQty > brandQty) {
                    double diff = gridQty - brandQty;
                    String w = outer ? "Outer" : "Inner";
                    throw new IllegalArgumentException("Pm" + w + "Qty can't be greater than Brand " + w + "Qty.\n" + "Brand " + w + "Qty is " + num(brandQty) + "\n"
                            + "Pm" + w + "Qty is " + num(gridQty) + "\n" + "Difference is " + num(diff) + "\n"
                            + "For Brand Item '" + nm + "' with UOM '" + uom + "' and PackingType '" + pt + "'");
                }
                seen.add(key);
            }
        }
    }

    /**
     * The ExImLcOrder model in property order (GenericProvider.SetProc sends every non-virtual
     * property): the form's values, the session tenancy, EntryDate/ModifyDate = now, Entry/ModifyUser
     * = the session user, FinancialYearId = the active year. Unset strings stay null (not sent).
     */
    static Map<String, Object> headerModel(UserAccount u, Map<String, Object> h, int recId, int documentTypeId) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("IsApproved", false);
        m.put("PostState", false);
        m.put("EntryDate", now());
        m.put("ExpiryDate", null);
        m.put("LastShipmentDate", ts(h.get("LastShipmentDate")));
        m.put("LcOrderDate", ts(h.get("LcOrderDate")));
        m.put("ModifyDate", now());
        m.put("PostDate", null);
        m.put("SalesContratDate", ts(h.get("SalesContratDate")));
        m.put("ShipmentStartDate", ts(h.get("ShipmentStartDate")));
        m.put("CustomerApprovalDate", ts(h.get("CustomerApprovalDate")));
        m.put("FactoryLoadingDate", ts(h.get("FactoryLoadingDate")));
        m.put("ConversionRate", asDouble(h.get("ConversionRate")));
        m.put("EquivalentAmount", 0.0);
        m.put("FCurrencyAmount", asDouble(h.get("FCurrencyAmount")));
        m.put("GrossWeightKgs", 0.0);
        m.put("NetWeightKgs", asDouble(h.get("NetWeightKgs")));
        m.put("NoOfContainers", asDouble(h.get("NoOfContainers")));
        m.put("BranchesId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        m.put("CompanyId", u.getCompanyId());
        m.put("DeliveryTermId", asInt(h.get("DeliveryTermId")));
        m.put("DestinationPortId", asInt(h.get("DestinationPortId")));
        m.put("EntryUser", u.getId());
        m.put("ExImProformaInvoice", 0);
        m.put("ExporterBankId", asInt(h.get("ExporterBankId")));
        m.put("ExportCompanyId", asInt(h.get("ExportCompanyId")));
        m.put("FcurrencyId", asInt(h.get("FcurrencyId")));
        m.put("CareierTypeId", 0);
        m.put("Id", recId > 0 ? recId : 0);
        m.put("ImporterBankId", asInt(h.get("ImporterBankId")));
        m.put("LcOrderDocNo", asInt(h.get("LcOrderDocNo")));
        m.put("LcOrderStatusId", 0);
        m.put("LoadingPortId", asInt(h.get("LoadingPortId")));
        m.put("ModifyUser", u.getId());
        m.put("NoOfShipments", 0);
        m.put("NotifyPartyId", asInt(h.get("NotifyPartyId")));
        m.put("NotifyParty2Id", asInt(h.get("NotifyParty2Id")));
        m.put("NotifyParty3Id", asInt(h.get("NotifyParty3Id")));
        m.put("OrganizationId", u.getOrganizationId());
        m.put("PaymentTermId", 0);
        m.put("PostUser", 0);
        m.put("ProductionSchDaysBefore", 0);
        m.put("ProjectsId", 0);
        m.put("SalesPersonId", asInt(h.get("SalesPersonId")));
        m.put("SampleSchDaysBefore", 0);
        m.put("ShipmentIntervalDays", 0);
        m.put("SupCustId", asInt(h.get("SupCustId")));
        m.put("ConsigneeId", asInt(h.get("ConsigneeId")));
        m.put("ContractType", 0);
        m.put("CommissionAgentId", 0);
        m.put("DeliveryDays", asInt(h.get("DeliveryDays")));
        m.put("ActionId", recId > 0 ? 2 : 1);
        m.put("CommRate", 0.0);
        m.put("CommAmount", 0.0);
        m.put("CommodityDetial", text(h.get("CommodityDetial")));
        m.put("PaymentRemarks", text(h.get("PaymentRemarks")));
        m.put("DeliveryRemarks", text(h.get("DeliveryRemarks")));
        m.put("CustomerCode", null);
        m.put("ExpiryPlace", null);
        m.put("FcurrencySymbol", null);
        m.put("InquiryReference", null);
        m.put("InsepctionDescription", null);
        m.put("InsepctionRequired", null);
        m.put("LcDocAttachment", null);
        m.put("LcOrderNo", text(h.get("LcOrderNo")));
        m.put("CustomerOrderNo", h.containsKey("CustomerOrderNo") ? text(h.get("CustomerOrderNo")) : null);
        m.put("LegalizationDescription", null);
        m.put("LegalizationRequired", null);
        m.put("LotReference", text(h.get("LotReference")));
        m.put("PartialShipment", null);
        m.put("QuotReference", null);
        m.put("RemarksHeader", text(h.get("RemarksHeader")));
        m.put("SalesContractRef", null);
        m.put("ShippingMarks", null);
        m.put("Status", text(h.get("Status")));
        m.put("TransShipment", null);
        m.put("InsuranceRemarks", null);
        m.put("CustomerApprovalRemarks", text(h.get("CustomerApprovalRemarks")));
        m.put("ContactPerson", null);
        m.put("DocumentTypeId", documentTypeId);
        m.put("ShipedToId", 0);
        m.put("FinancialYearId", 0);
        m.put("CustomGroupId", asInt(h.get("CustomGroupId")));
        m.put("ProductSpecification", text(h.get("ProductSpecification")));
        m.put("AddLessComments", null);
        m.put("AttachmentsValues", text(h.get("AttachmentsValues")).isEmpty() ? "" : text(h.get("AttachmentsValues")));
        m.put("CustomAttachmentsValues", text(h.get("CustomAttachmentsValues")).isEmpty() ? "" : text(h.get("CustomAttachmentsValues")));
        m.put("AddLessAmount", 0.0);
        m.put("TotalAmountWithAdls", asDouble(h.get("FCurrencyAmount")));
        return m;
    }

    /** The ExImLcOrderPackingDetail model (34 non-virtual properties) from a 209 grid row. */
    static Map<String, Object> detailModel(Map<String, Object> r, int id) {
        Map<String, Object> m = new LinkedHashMap<>();
        double mton = asDouble(r.get("QtyMTon"));
        m.put("InnerQty", mton);
        m.put("NetWeight", mton * 1000.0);
        m.put("NoOfUnit", asDouble(r.get("NoOfBags")));
        m.put("OuterQty", mton);
        m.put("RatePrice", asDouble(r.get("Rate")));
        m.put("TotalAmount", asDouble(r.get("Amount")));
        m.put("NoofContainer", 0.0);
        m.put("PackingWeight", 0.0);
        m.put("NoofBagsPerContainer", 0.0);
        m.put("PackingTotalWeight", 0.0);
        m.put("GrossWeight", 0.0);
        m.put("PalletQty", 0.0);
        m.put("ExImItemId", asInt(r.get("ItemId")));
        m.put("PmItemId", 0);
        m.put("ExImLcOrderId", 0);
        m.put("CropYearId", asInt(r.get("CropYearId")));
        m.put("Id", id);
        m.put("AmountCalulationId", 0);
        m.put("InvPackingMaterialTypeId", asInt(r.get("PackTypeId")));
        m.put("ItemSpecificationId", 0);
        m.put("ItemBrandId", 0);
        m.put("UOMScheduleIdInner", asInt(r.get("PackSizeId")));
        m.put("UOMScheduleIdOuter", asInt(r.get("PackSizeId")));
        m.put("UOMScheduleIdRate", asInt(r.get("RateUOMId")));
        m.put("ActionTypeId", 0);
        m.put("ExImFarmingNTradeId", asInt(r.get("ExImFarmingNTradeId")));
        m.put("ExImTradeTypeId", asInt(r.get("ExImTradeTypeId")));
        m.put("ExImFarmingTypeId", asInt(r.get("ExImFarmingTypeId")));
        m.put("Other_Description", text(r.get("Remarks")));
        m.put("OuterPackDescription", null);
        m.put("ContainerSize", null);
        m.put("PackingExpiryDate", null);
        m.put("CommodityDetail", text(r.get("CommodityDetail")));
        m.put("PackingDetail", null);
        return m;
    }

    static Map<String, Object> otherItemModel(Map<String, Object> r) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("oItemAmount", dec(r.get("Amount")));
        m.put("oItemQty", dec(r.get("Qty")));
        m.put("oItemWeightKgs", java.math.BigDecimal.ZERO);
        m.put("oItemRate", asDouble(r.get("Rate")));
        m.put("ExImLcOrderId", 0);
        m.put("Id", 0);
        m.put("otherItemId", asInt(r.get("ItemId")));
        m.put("OtherItemRemarks", text(r.get("Remarks")));
        return m;
    }

    static Map<String, Object> paymentModel(Map<String, Object> r) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("FcyAmount", asDouble(r.get("FcyAmount")));
        m.put("PrcntOfTotal", asDouble(r.get("PrcntOfTotal")));
        m.put("DueDays", 0);
        m.put("ExportDueTypeId", 0);
        m.put("Id", 0);
        m.put("LcOrderId", 0);
        m.put("PaymentTermId", asInt(r.get("PaymentTermId")));
        m.put("SortNo", 0);
        m.put("Remarks", text(r.get("Remarks")));
        return m;
    }

    /**
     * ExImLcContractPackingMaterialDetail from a PM grid row, with the desktop's per-row checks
     * ("... Is Required In Packing Material Grid in Row No n"). The III form validates after the
     * values are read (FormHelper.ValidateField texts) - the same checks, so one builder serves both.
     */
    static Map<String, Object> pmModel(Map<String, Object> r, int id, int rowIndex, boolean iii) {
        int n = rowIndex + 1;
        if (asInt(r.get("BrandId")) == 0) throw new IllegalArgumentException(iii ? "Brand Item is required in Packing Material Grid at row No: " + n : "Brand Item Is Required In Packing Material Grid in Row No " + n);
        if (asInt(r.get("BrandUomId")) == 0) throw new IllegalArgumentException(iii ? "Brand Uom is required in Packing Material Grid at row No: " + n : "Brand Uom Is Required In Packing Material Grid in Row No " + n);
        if (asInt(r.get("PackingTypeId")) == 0) throw new IllegalArgumentException(iii ? "Brand PackingType is required in Packing Material Grid at row No: " + n : "Brand PackingType Is Required In Packing Material Grid in Row No " + n);
        if (asDouble(r.get("BrandOuterQty")) == 0) throw new IllegalArgumentException(iii ? "BrandOuterQty is required in Packing Material Grid at row No: " + n : "BrandOuterQty Is Required In Packing Material Grid in Row No " + n);
        if (asInt(r.get("PmItemId")) == 0) throw new IllegalArgumentException(iii ? "PmItem is required in Packing Material Grid at row No: " + n : "PmItem Is Required In Packing Material Grid in Row No " + n);
        if (asInt(r.get("PmItemUomId")) == 0) throw new IllegalArgumentException(iii ? "PmItemUom is required in Packing Material Grid at row No: " + n : "PmItemUom Is Required In Packing Material Grid in Row No " + n);
        if (iii ? (dec(r.get("PmItemOuterQty")).signum() == 0 && asDouble(r.get("PmItemInnerQty")) == 0)
                : (asInt(r.get("PmItemOuterQty")) == 0 && asInt(r.get("PmItemInnerQty")) == 0))
            throw new IllegalArgumentException(iii ? "PmItemOuterQty or PmItemInnerQty is required in Packing Material Grid in Row No " + n : "PmItemOuterQty or PmItemInnerQty Is Required In Packing Material Grid in Row No " + n);
        if (asDouble(r.get("Rate")) == 0) throw new IllegalArgumentException(iii ? "Rate is required in Packing Material Grid at row No: " + n : "Rate Is Required In Packing Material Grid in Row No " + n);
        if (asDouble(r.get("Amount")) == 0) throw new IllegalArgumentException(iii ? "Amount is required in Packing Material Grid at row No: " + n : "Amount Is Required In Packing Material Grid in Row No " + n);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Amount", dec(r.get("Amount")));
        m.put("Qty", dec(r.get("PmItemOuterQty")));
        m.put("Rate", dec(r.get("Rate")));
        m.put("ExImLcOrderId", 0);
        m.put("Id", id);
        m.put("BrandId", asInt(r.get("BrandId")));
        m.put("ItemId", asInt(r.get("PmItemId")));
        m.put("PackingTypeId", asInt(r.get("PackingTypeId")));
        m.put("SortNo", 0);
        m.put("pmRemarks", text(r.get("Remarks")));
        m.put("BrandUomId", asInt(r.get("BrandUomId")));
        m.put("BrandOuterQty", asDouble(r.get("BrandOuterQty")));
        m.put("BrandInnerQty", asDouble(r.get("BrandInnerQty")));
        m.put("ItemUomId", asInt(r.get("PmItemUomId")));
        m.put("InnerQty", asDouble(r.get("PmItemInnerQty")));
        return m;
    }

    static Map<String, Object> chargeModel(Map<String, Object> r, int rowIndex) {
        int chargesItemId = asInt(r.get("ChargesItemId"));
        double add = asDouble(r.get("AddAmount")), less = asDouble(r.get("LessAmount"));
        if (chargesItemId == 0) throw new IllegalArgumentException("Charges Item is required in Other Charges at row No: " + (rowIndex + 1));
        if (add == 0 && less == 0) throw new IllegalArgumentException("AddAmount Or Less Amount is required in Other Charges at row No: " + (rowIndex + 1));
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("AddAmount", add);
        m.put("LessAmount", less);
        m.put("ChargesItemId", chargesItemId);
        m.put("ExImLcOrderId", 0);
        m.put("Id", 0);
        m.put("Remarks", text(r.get("Remarks")));
        return m;
    }

    // ================================================================= history

    /**
     * HistoryFill: DocumentTypeIds "202", CanViewAllRecord right (EntryUser when absent), the active
     * year, the ticked From/To on the checked radio's pair, FromDocNo / ToDocNo / Customer when
     * non-zero, ApprovedFilter "All" (no @IsApproved); the dtcol projection.
     */
    public List<Map<String, Object>> history(Map<String, Object> f) {
        return historyRows(repo.formHistory(historyParams(user("View"), f, String.valueOf(DOCUMENT_TYPE_ID))));
    }

    Map<String, Object> historyParams(UserAccount u, Map<String, Object> f, String documentTypeIds) {
        boolean canViewAll = allowed(u, "CanViewAllRecord");
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("DocumentTypeIds", documentTypeIds);
        p.put("CanViewAllRecord", canViewAll);
        int fy = financialYearId();
        if (fy != 0) p.put("FinancialYearId", fy);
        if (!canViewAll) p.put("EntryUser", u.getId());
        LocalDate from = asBool(f.get("fromChecked")) ? asDate(f.get("fromDate")) : null;
        LocalDate to = asBool(f.get("toChecked")) ? asDate(f.get("toDate")) : null;
        String by = text(f.get("dateBy"));
        String fromKey = "FromDate", toKey = "ToDate";
        if ("entry".equals(by)) { fromKey = "EntryFromDate"; toKey = "EntryToDate"; }
        else if ("modify".equals(by)) { fromKey = "ModifyFromDate"; toKey = "ModifyToDate"; }
        else if ("approved".equals(by)) { fromKey = "ApprovedFromDate"; toKey = "ApprovedToDate"; }
        if (from != null) p.put(fromKey, sqlDate(from));
        if (to != null) p.put(toKey, sqlDate(to));
        int fromDoc = asInt(f.get("fromDocNo")), toDoc = asInt(f.get("toDocNo"));
        if (fromDoc != 0) p.put("DocNoFrom", (double) fromDoc);
        if (toDoc != 0) p.put("DocNoTo", (double) toDoc);
        int cust = asInt(f.get("customerId"));
        if (cust != 0) p.put("SupplierCustomerId", cust);
        return p;
    }

    static List<Map<String, Object>> historyRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("DocNo", asInt(ci(r, "LcOrderDocNo")));
            m.put("DocDate", iso(ci(r, "SalesContratDate")));
            m.put("OrderNo", text(ci(r, "LcOrderNo")));
            m.put("OrderDate", iso(ci(r, "LcOrderDate")));
            m.put("CustomerName", text(ci(r, "CustomerName")));
            m.put("CustomerOrderNo", text(ci(r, "CustomerOrderNo")));
            m.put("LotReference", text(ci(r, "LotReference")));
            m.put("MTons", asDouble(ci(r, "NetKgs")));
            m.put("NoOfContainers", asInt(ci(r, "NoOfContr")));
            m.put("FcyCode", text(ci(r, "CurrencyCode")));
            m.put("FcyAmount", asDouble(ci(r, "FCurrencyAmount")));
            m.put("LastShipmentDate", iso(ci(r, "LastShipmentDate")));
            m.put("NotifyParty", text(ci(r, "NotifyParty")));
            m.put("DeliveryTerm", text(ci(r, "DeliveryTerm")));
            m.put("PaymentTerm", text(ci(r, "PaymentTerm")));
            m.put("LoadingPort", text(ci(r, "LoadingPort")));
            m.put("DestinationPort", text(ci(r, "PortName")));
            m.put("Commodity_Description", text(ci(r, "Commodity_Description")));
            m.put("EntryUser", text(ci(r, "EntryUserName")));
            m.put("EntryDate", isoDateTime(ci(r, "EntryDate")));
            m.put("ModifyUser", text(ci(r, "ModifyUserName")));
            m.put("ModifyDate", isoDateTime(ci(r, "ModifyDate")));
            m.put("NoOfAttachments", asInt(ci(r, "NoOfAttachments")));
            out.add(m);
        }
        return out;
    }
}
