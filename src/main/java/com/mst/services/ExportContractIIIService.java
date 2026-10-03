package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportSalesContractRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.ExportGSupport.*;
import static com.mst.services.ExportSalesContractService.*;

/**
 * The BLL side of 879 frmExportContractIII "Export Contract (III)" (Architecture.WinApp.Export),
 * ScreenName frmExportContractIII, DocumentTypeId 223, ClientSize 1234 x 761. The newer rewrite of
 * the contract form: the Contract No comes from the generated contract numbers
 * (USP_ExportInvoiceNos_GetAllMethod), customers / sales persons / consignees / notify parties from
 * the global party list, items / packing items / brands / crop years from the global item lists, a
 * detail row carries Pm Item, Brand, Outer / Inner UOM, Pallet Qty and Pm Detail, the FCY combo
 * fills the exchange rate, and "Save" can open the Contract Schedule (chkOpenContractSchedule).
 *
 * Rights: View, Save (btnsave, BtnSaveAs), Update, Delete (empty handler), Print, CanViewAllRecord.
 */
@Service
public class ExportContractIIIService {

    public static final int SCREEN_ID = 879;
    public static final int DOCUMENT_TYPE_ID = 223;

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

    /** InitializeComponentMethod - every DB call of the Task.Run block plus the global lists the binders read. */
    public Map<String, Object> setup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, "Save"));
        perm.put("Update", allowed(u, "Update"));
        perm.put("Delete", allowed(u, "Delete"));
        perm.put("Print", allowed(u, "Print"));
        out.put("permissions", perm);
        out.putAll(all(u, DOCUMENT_TYPE_ID));
        try { out.put("historyCustomers", pairs(repo.historyDropDowns(u, "Customer"), "Id", "name")); } catch (Exception e) { out.put("historyCustomers", new ArrayList<>()); }
        return out;
    }

    /** btnRefresh_Click: the global lists are re-read and every combo re-bound; LastSavedRecord with DocumentTypeId 0 (Reset) / 223 (load). */
    public Map<String, Object> refresh() {
        UserAccount u = user("View");
        return all(u, 0);
    }

    private Map<String, Object> all(UserAccount u, int lastSavedDocType) {
        Map<String, Object> out = new LinkedHashMap<>();
        boolean multi = false, sdf = false;
        try { multi = repo.erpFeature(u, 9); sdf = repo.erpFeature(u, 12); } catch (Exception e) { /* off */ }
        out.put("allowExportMultiCompanies", multi);
        out.put("shipmentDocumentsFeature", sdf);
        Map<String, Object> c = new LinkedHashMap<>();
        boolean allItems = asBool(repo.config(u, "AllItemsBindonExportContract"));
        c.put("allItemsBindonExportContract", allItems);
        c.put("pmGridInfoShouldNotCompulsoryOnContract", asBool(repo.config(u, "PMGridInfoShouldNotCompulsoryOnContract")));
        c.put("defaultDaysToLessFromHistoryFromDate", asInt(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        c.put("itemSearchByCode", asBool(repo.config(u, "ItemSearchByCode")));
        c.put("specialInstructionsForExportContract", repo.config(u, "SpecialInstructionsForExportContract"));
        c.put("defaultCropYearId", asInt(repo.config(u, "Default Crop Year")));
        c.put("defaultPackingTypeId", asInt(repo.config(u, "Paking Type")));
        c.put("fcyDecimals", asInt(repo.config(u, "DefaultNoOfDecimalPointsForFcyAmount")));
        out.put("config", c);
        put(out, "contractNos", () -> contractNos(u, null));
        if (multi) put(out, "exportCompanies", () -> pairs(repo.exportCompanies(u), "Id", "CompName"));
        put(out, "parties", () -> parties(u));
        put(out, "banks", () -> banks(u));
        put(out, "ports", () -> pairs(repo.seaPorts(u), "Id", "PortName"));
        put(out, "deliveryTerms", () -> rows3(repo.deliveryTerms(), "Id", "Code", "Description"));
        put(out, "currencies", () -> currencies(u));
        if (sdf) put(out, "customGroups", () -> pairs(repo.customGroupsByCustomer(u, 0), "CustomGroupId", "CustomGroup"));
        put(out, "items", () -> items(u, allItems));
        put(out, "packTypes", () -> pairs(repo.packTypes(u), "Id", "Description"));
        put(out, "brands", () -> brands(u));
        put(out, "cropYears", () -> pairs(repo.cropYears(u), "Id", "CropYear"));
        put(out, "commodityRemarks", () -> commodityRemarks(u, 0));
        put(out, "farmingNTrade", () -> farming(u));
        put(out, "paymentTerms", () -> rows3(repo.paymentTerms(u), "Id", "lcOrderTerm", "lcOrderTermDesc"));
        put(out, "exportCharges", () -> pairs(repo.exportCharges(u), "ExportChargesID", "ExportChargesName"));
        try { out.put("lastSaved", lastSaved(u, lastSavedDocType)); } catch (Exception e) { out.put("lastSaved", null); }
        return out;
    }

    private interface Src { Object get(); }
    private static void put(Map<String, Object> out, String key, Src s) {
        try { out.put(key, s.get()); } catch (Exception e) { out.put(key, new ArrayList<>()); out.put(key + "Error", msg(e)); }
    }

    private static List<Map<String, Object>> rows3(List<Map<String, Object>> src, String idCol, String nameCol, String descCol) {
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

    /** CustomerOrderNoDbCall(ContractNo): GetFinalInvoiceNosForExportContract -> Id, CustomerOrderNo (FinalInvoiceNo). */
    private List<Map<String, Object>> contractNos(UserAccount u, String contractNo) {
        return pairs(repo.finalInvoiceNosForContract(u, DOCUMENT_TYPE_ID, contractNo), "Id", "FinalInvoiceNo");
    }

    public Map<String, Object> contractNosApi(String contractNo) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("contractNos", contractNos(user("View"), text(contractNo)));
        return out;
    }

    /**
     * SupplierDtFillFromGlobal / ConsigneeAgainstCustomerBind: the global party list. customers =
     * CustomerGroupId 7 and not a sub-party, salesPersons = CustomerGroupId 9, and every sub-party
     * (IsSubSupCust) with its ParentsSupCustId so the page can filter Consignee / Notify Party 1-3.
     */
    private Map<String, Object> parties(UserAccount u) {
        List<Map<String, Object>> customers = new ArrayList<>(), salesmen = new ArrayList<>(), subs = new ArrayList<>();
        for (Map<String, Object> r : repo.globalSupplierCustomers(u)) {
            int group = asInt(ci(r, "CustomerGroupId"));
            boolean sub = asBool(ci(r, "IsSubSupCust"));
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("name", text(ci(r, "CompanyName")));
            m.put("PartyCode", text(ci(r, "PartyCode")));
            m.put("CityName", text(ci(r, "CityName")));
            m.put("MobileNo", text(ci(r, "MobilePersonal")));
            if (group == 7 && !sub) customers.add(m);
            if (group == 9) salesmen.add(m);
            if (sub) {
                Map<String, Object> s = new LinkedHashMap<>(m);
                s.put("ParentsSupCustId", asInt(ci(r, "ParentsSupCustId")));
                subs.add(s);
            }
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("customers", customers);
        out.put("salesPersons", salesmen);
        out.put("subParties", subs);
        return out;
    }

    private Map<String, Object> banks(UserAccount u) {
        List<Map<String, Object>> foreign = new ArrayList<>(), home = new ArrayList<>();
        for (Map<String, Object> r : repo.banks(u)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("name", text(ci(r, "BranchName")));
            m.put("BankIBANNo", text(ci(r, "BankIBANNo")));
            String h = text(ci(r, "IsHomeland"));
            if ("Foreign Country".equals(h)) foreign.add(m);
            else if ("Home Country".equals(h)) home.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("importer", foreign);
        out.put("exporter", home);
        return out;
    }

    /** MultiCurrency.GetMultiCurrencywithExchangeRate - Id, CurrencyCode, CurrencyName, ExchangeRate (Cells[3]). */
    private List<Map<String, Object>> currencies(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.currenciesWithExchangeRate(u)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("name", text(ci(r, "CurrencyCode")));
            m.put("CurrencyName", text(ci(r, "CurrencyName")));
            m.put("ExchangeRate", asDouble(ci(r, "ExchangeRate")));
            out.add(m);
        }
        return out;
    }

    /**
     * ItemDtsFillFromGlobal: clsGlobalVariables.getGlobalAllItems (USP_Item_AllItemsWithModal) filtered to
     * ItemClassId 8, or to ItemTypeOfTypeId not in (14, 17) when AllItemsBindonExportContract; the page
     * groups them by Item Category / Item Type (RadCategory / RadType). pmItems = ItemTypeOfTypeId 14.
     */
    private Map<String, Object> items(UserAccount u, boolean allItems) {
        List<Map<String, Object>> items = new ArrayList<>(), pm = new ArrayList<>();
        for (Map<String, Object> r : repo.allItemsWithModal(u)) {
            int classId = asInt(ci(r, "ItemClassId")), typeOfType = asInt(ci(r, "ItemTypeOfTypeId"));
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("ItemName", text(ci(r, "ItemName")));
            m.put("ItemCode", text(ci(r, "ItemCode")));
            m.put("InventoryParentCategoriesId", asInt(ci(r, "InventoryParentCategoriesId")));
            m.put("ItemCategoryId", asInt(ci(r, "ItemCategoryId")));
            m.put("ItemCategory", text(ci(r, "ItemCategory")));
            m.put("ItemTypeId", asInt(ci(r, "ItemTypeId")));
            m.put("ItemType", text(ci(r, "ItemType")));
            m.put("ProductionStageId", asInt(ci(r, "ItemProductionStageId")));
            m.put("ProductionStage", text(ci(r, "productionStageName")));
            boolean keep = allItems ? (typeOfType != 14 && typeOfType != 17) : classId == 8;
            if (keep) items.add(m);
            if (typeOfType == 14) pm.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("items", items);
        out.put("pmItems", pm);
        return out;
    }

    private List<Map<String, Object>> brands(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.brands(u)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("BrandName", text(ci(r, "BrandName")));
            m.put("BrandCode", text(ci(r, "BrandCode")));
            out.add(m);
        }
        return out;
    }

    /** ItemCommodityDetailBind: GetRemarks(ItemId) split - DetailTypeId 2 -> pmDetail, else commodity. */
    private Map<String, Object> commodityRemarks(UserAccount u, int itemId) {
        List<Map<String, Object>> commo = new ArrayList<>(), pm = new ArrayList<>();
        for (Map<String, Object> r : repo.commodityRemarks(u, itemId, 0)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("name", text(ci(r, "Remarks")));
            if (asInt(ci(r, "DetailTypeId")) == 2) pm.add(m); else commo.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("commodity", commo);
        out.put("pmDetail", pm);
        return out;
    }

    public Map<String, Object> commodityRemarksApi(int itemId) { return commodityRemarks(user("View"), itemId); }

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

    private Map<String, Object> lastSaved(UserAccount u, int documentTypeId) {
        List<Map<String, Object>> r = repo.lastSavedRecord(u, documentTypeId);
        if (r.isEmpty()) return null;
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("SalesPersonId", asInt(ci(r.get(0), "SalesPersonId")));
        m.put("FcurrencyId", asInt(ci(r.get(0), "FcurrencyId")));
        m.put("CropYearId", asInt(ci(r.get(0), "CropYearId")));
        m.put("InvPackingMaterialTypeId", asInt(ci(r.get(0), "InvPackingMaterialTypeId")));
        return m;
    }

    public Map<String, Object> lastSavedApi(int documentTypeId) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("lastSaved", lastSaved(user("View"), documentTypeId));
        return out;
    }

    public List<Map<String, Object>> customGroupsApi(int customerId) {
        return pairs(repo.customGroupsByCustomer(user("View"), customerId), "CustomGroupId", "CustomGroup");
    }

    public Map<String, Object> historyCombos() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("historyCustomers", pairs(repo.historyDropDowns(user("View"), "Customer"), "Id", "name"));
        return out;
    }

    // ================================================================= item side lookups

    /** bindRateUomAndItemPackUom: GetUomScheduleByItemIdForExport. */
    public List<Map<String, Object>> uomForExport(int itemId) {
        UserAccount u = user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.uomForExport(u, itemId)) out.add(uomRow(r));
        return out;
    }

    /** PmItemUomFromGlobalBind: dtUomFromGloablUomScheduleByItemId - the global schedule for the item. */
    public List<Map<String, Object>> globalUoms(int itemId) {
        UserAccount u = user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.globalUoms(u, itemId)) out.add(uomRow(r));
        return out;
    }

    public Map<String, Object> lastRate(int itemId) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rate", repo.lastRateByItemId(u, itemId));
        return out;
    }

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

    // ================================================================= ReadById

    /** ReadById(ID) plus CustomerOrderNoBind(CustomerOrderNoDbCall(Obj.LcOrderNo)). */
    public Map<String, Object> readById(int id) {
        UserAccount u = user("View");
        List<Map<String, Object>> hdr = repo.header(id);
        if (hdr.isEmpty()) throw new IllegalArgumentException("Record not found");
        Map<String, Object> h = hdr.get(0);
        int docType = asInt(ci(h, "DocumentTypeId"));
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> header = header(h);
        out.put("header", header);
        out.put("details", detailRows(repo.detail(id, docType)));
        out.put("otherItems", otherItemRows(repo.otherItems(id)));
        out.put("paymentTerms", paymentRows(repo.paymentTermsDetail(id)));
        out.put("pmRows", pmRows(repo.packingMaterialDetail(id)));
        out.put("otherCharges", chargeRows(repo.otherChargesDetail(id)));
        try { out.put("contractNos", contractNos(u, text(header.get("LcOrderNo")))); } catch (Exception e) { out.put("contractNos", new ArrayList<>()); }
        return out;
    }

    // ================================================================= save

    /**
     * Insert(): "Grid Record Not Found" when the detail grid is empty, FormHelper.ValidateControls
     * (field + " field is required" / " must be a non-zero number" / " is not valid"), the FcAmount /
     * NetWeight check, "Payment Percent Not Equal To 100%", CheckBomHeaderIds, the header (containers
     * defaulting to NetWeightKgs / 25 rounded to 3), the detail rows through
     * FillDetailListCommonForInsertAndDelete + ValidateField, other items, payment terms, PM rows,
     * other charges; ExImLcOrder.Save.
     */
    public Map<String, Object> save(Map<String, Object> b) {
        int recId = asInt(b.get("recId"));
        UserAccount u = user(recId > 0 ? "Update" : "Save");
        Map<String, Object> h = map(b.get("header"));
        boolean multi = asBool(b.get("allowExportMultiCompanies"));
        boolean pmNotCompulsory = asBool(repo.config(u, "PMGridInfoShouldNotCompulsoryOnContract"));

        List<Map<String, Object>> details = list(b.get("details"));
        List<Map<String, Object>> removed = list(b.get("removedDetails"));
        List<Map<String, Object>> otherItems = list(b.get("otherItems"));
        List<Map<String, Object>> payments = list(b.get("paymentTerms"));
        List<Map<String, Object>> pm = list(b.get("pmRows"));
        List<Map<String, Object>> charges = list(b.get("otherCharges"));

        if (details.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        if (multi && asInt(h.get("ExportCompanyId")) == 0) throw new IllegalArgumentException("Export Company field is required");
        if (asInt(h.get("ContractNoId")) == 0) throw new IllegalArgumentException("Export Contract No field is required");
        if (asInt(h.get("SalesPersonId")) == 0) throw new IllegalArgumentException("Sale Person field is required");
        if (asInt(h.get("SupCustId")) == 0) throw new IllegalArgumentException("Customer field is required");
        if (dateNull(asDate(h.get("SalesContratDate")))) throw new IllegalArgumentException("Order Date is not valid");
        if (asDouble(h.get("NetWeightKgs")) == 0) throw new IllegalArgumentException("M.Ton must be a non-zero number");
        if (asDouble(h.get("NoOfContainers")) == 0) throw new IllegalArgumentException("No. of Containers must be a non-zero number");
        if (asInt(h.get("FcurrencyId")) == 0) throw new IllegalArgumentException("FCY Code field is required");
        if (asDouble(h.get("ConversionRate")) == 0) throw new IllegalArgumentException("Exchange Rate must be a non-zero number");
        if (asDouble(h.get("FCurrencyAmount")) == 0) throw new IllegalArgumentException("FCY Amount must be a non-zero number");
        if (asInt(h.get("DeliveryDays")) == 0) throw new IllegalArgumentException("Delivery Days must be a non-zero number");
        if (asInt(h.get("DeliveryTermId")) == 0) throw new IllegalArgumentException("Delivery Term field is required");
        if (asInt(h.get("LoadingPortId")) == 0) throw new IllegalArgumentException("Loading Port field is required");
        if (asInt(h.get("DestinationPortId")) == 0) throw new IllegalArgumentException("Destination Port field is required");

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
        header.put("LcOrderNo", text(h.get("LcOrderNo")));           /* the Contract No combo's text */
        header.put("CustomerOrderNo", text(h.get("CustomerOrderNo")));
        double netKgs = asDouble(h.get("NetWeightKgs")), containers = asDouble(h.get("NoOfContainers"));
        header.put("NoOfContainers", containers > 0 ? containers : (netKgs > 0 ? Math.round(netKgs / 25.0 * 1000.0) / 1000.0 : 0.0));
        header.put("LcOrderDocNo", 0);                                /* the III form has no Doc No box; the insert numbers it */

        List<Map<String, Object>> detailModels = new ArrayList<>();
        if (recId > 0) for (Map<String, Object> r : removed) { Map<String, Object> vd = detailModelIII(r, asInt(r.get("Id"))); vd.put("ActionTypeId", 3); detailModels.add(vd); }
        String remarksFromDetail = "";
        int ri = 0;
        for (Map<String, Object> r : details) {
            int id = recId != 0 ? asInt(r.get("Id")) : 0;
            Map<String, Object> vd = detailModelIII(r, id);
            vd.put("ActionTypeId", id <= 0 ? 1 : 2);
            validateField(vd.get("ExImItemId"), "Item", ri);
            validateField(vd.get("InvPackingMaterialTypeId"), "PackingType", ri);
            validateField(vd.get("PmItemId"), "Pm Item", ri);
            validateField(vd.get("ItemBrandId"), "Brand", ri);
            validateField(vd.get("CropYearId"), "CropYear", ri);
            validateField(vd.get("NetWeight"), "M.Ton", ri);
            validateField(vd.get("UOMScheduleIdOuter"), "Outer Uom", ri);
            validateField(vd.get("RatePrice"), "Rate", ri);
            validateField(vd.get("UOMScheduleIdRate"), "Rate Uom", ri);
            validateField(vd.get("TotalAmount"), "Amount", ri);
            if (text(header.get("RemarksHeader")).isEmpty()) remarksFromDetail = text(vd.get("Other_Description"));
            detailModels.add(vd);
            ri++;
        }
        if (text(header.get("RemarksHeader")).isEmpty()) header.put("RemarksHeader", remarksFromDetail);

        List<Map<String, Object>> otherModels = new ArrayList<>();
        for (Map<String, Object> r : otherItems) otherModels.add(otherItemModel(r));
        if (payments.isEmpty()) throw new IllegalArgumentException("Payment Term Grid record not found");
        List<Map<String, Object>> payModels = new ArrayList<>();
        for (Map<String, Object> r : payments) payModels.add(paymentModel(r));
        header.put("PaymentTermId", asInt(payments.get(0).get("PaymentTermId")));

        List<Map<String, Object>> pmModels = new ArrayList<>();
        if (!pm.isEmpty()) {
            int i = 0;
            for (Map<String, Object> r : pm) pmModels.add(pmModel(r, asInt(r.get("Id")), i++, true));
        } else if (!pmNotCompulsory) {
            throw new IllegalArgumentException("Packing Material Grid record not found");
        }
        pmGridValidation(brandForPmItem(details, "OuterUomId", "OuterQty"), pm);

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

    /** FormHelper.ValidateField(value, field, rowIndex) - "Detail Grid". */
    private static void validateField(Object value, String field, int rowIndex) {
        boolean bad = value == null
                || (value instanceof Integer && (Integer) value == 0)
                || (value instanceof Double && (Double) value <= 0)
                || (value instanceof String && ((String) value).trim().isEmpty());
        if (bad) throw new IllegalArgumentException(field + " is required in Detail Grid at row No: " + (rowIndex + 1));
    }

    /** FillDetailListCommonForInsertAndDelete - the ExImLcOrderPackingDetail model from a III grid row. */
    static Map<String, Object> detailModelIII(Map<String, Object> r, int id) {
        Map<String, Object> m = new LinkedHashMap<>();
        double mton = asDouble(r.get("QtyMTon"));
        m.put("InnerQty", asDouble(r.get("InnerQty")));
        m.put("NetWeight", mton * 1000.0);
        m.put("NoOfUnit", asDouble(r.get("OuterQty")));
        m.put("OuterQty", mton);
        m.put("RatePrice", asDouble(r.get("Rate")));
        m.put("TotalAmount", asDouble(r.get("Amount")));
        m.put("NoofContainer", 0.0);
        m.put("PackingWeight", 0.0);
        m.put("NoofBagsPerContainer", 0.0);
        m.put("PackingTotalWeight", 0.0);
        m.put("GrossWeight", 0.0);
        m.put("PalletQty", asDouble(r.get("PalletQty")));
        m.put("ExImItemId", asInt(r.get("ItemId")));
        m.put("PmItemId", asInt(r.get("PmItemId")));
        m.put("ExImLcOrderId", 0);
        m.put("CropYearId", asInt(r.get("CropYearId")));
        m.put("Id", id);
        m.put("AmountCalulationId", 0);
        m.put("InvPackingMaterialTypeId", asInt(r.get("PackTypeId")));
        m.put("ItemSpecificationId", 0);
        m.put("ItemBrandId", asInt(r.get("BrandId")));
        m.put("UOMScheduleIdInner", asInt(r.get("InnerUomId")));
        m.put("UOMScheduleIdOuter", asInt(r.get("OuterUomId")));
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
        m.put("PackingDetail", text(r.get("PackingDetail")));
        return m;
    }

    // ================================================================= history

    /** HistoryFill with DocumentTypeIds "223"; dtcol has Contract No / Date, Customer Order No / Date. */
    public List<Map<String, Object>> history(Map<String, Object> f) {
        UserAccount u = user("View");
        boolean canViewAll = allowed(u, "CanViewAllRecord");
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("DocumentTypeIds", String.valueOf(DOCUMENT_TYPE_ID));
        p.put("CanViewAllRecord", canViewAll);
        int fy = financialYearId();
        if (fy != 0) p.put("FinancialYearId", fy);
        if (!canViewAll) p.put("EntryUser", u.getId());
        java.time.LocalDate from = asBool(f.get("fromChecked")) ? asDate(f.get("fromDate")) : null;
        java.time.LocalDate to = asBool(f.get("toChecked")) ? asDate(f.get("toDate")) : null;
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
        return historyRows(repo.formHistory(p));
    }

    static Set<Integer> ids(List<Map<String, Object>> rows, String col) {
        Set<Integer> s = new LinkedHashSet<>();
        for (Map<String, Object> r : rows) s.add(asInt(r.get(col)));
        return s;
    }
}
