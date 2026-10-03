package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportPreInvoiceRepository;
import com.mst.repositories.ExportProformaInvoiceRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.ExportPreInvoiceService.asBool;
import static com.mst.services.ExportPreInvoiceService.asDate;
import static com.mst.services.ExportPreInvoiceService.asDouble;
import static com.mst.services.ExportPreInvoiceService.asInt;
import static com.mst.services.ExportPreInvoiceService.cDbl;
import static com.mst.services.ExportPreInvoiceService.cTextInt;
import static com.mst.services.ExportPreInvoiceService.ci;
import static com.mst.services.ExportPreInvoiceService.dec;
import static com.mst.services.ExportPreInvoiceService.iso;
import static com.mst.services.ExportPreInvoiceService.list;
import static com.mst.services.ExportPreInvoiceService.map;
import static com.mst.services.ExportPreInvoiceService.pick;
import static com.mst.services.ExportPreInvoiceService.raw;
import static com.mst.services.ExportPreInvoiceService.row;
import static com.mst.services.ExportPreInvoiceService.text;
import static com.mst.services.ExportPreInvoiceService.ts;

/**
 * BLL side of 215 "Proforma Invoice" - Architecture.WinApp.Export.ProformaInvoice (ExImLcOrder, DocumentTypeId 151;
 * the form's Text is "Export Contract"). Header + "Contract Detail" entry panel and grid + "Other Items" tab;
 * History with Print (501), Print523, Edit and SaveAs buttons.
 *
 * Desktop behaviour kept: Save asks first, regenerates the Doc No (new records only), then FormValidation; the
 * "Status Should Be Open In Save Mode" check only fires when the Update button is neither visible nor enabled
 * (i.e. a user without the Update right); NetWeightKgs stores the header M.Ton; TotalAmountWithAdls = Fcy
 * Amount; the grid's "Packing Detail" is saved into CommodityDetail (PackingDetail stays null) and read back from
 * it; ContractType 1 needs FlagFin, which the form never sets, so it is never written. Rights: View / Save /
 * Update / Print and "CanView AllRecord" (history EntryUser filter) of ScreenDefinition 215; tenancy from the session.
 */
@Service
public class ExportProformaInvoiceService {

    public static final int SCREEN_ID = 215;
    public static final int DOCUMENT_TYPE_ID = 151;

    @Autowired private ExportProformaInvoiceRepository repo;
    @Autowired private ExportPreInvoiceRepository shared;
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

    private int fy() { return currentUserContext.currentFinancialYearId(); }

    // ================================================================= load

    /** FrmExportSalesContract_Load. */
    public Map<String, Object> setup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, "Save"));
        perm.put("Update", allowed(u, "Update"));
        perm.put("Print", allowed(u, "Print"));
        out.put("permissions", perm);
        out.put("docNo", docNo(u));
        out.putAll(lookups(u, true));
        int days;
        try { days = (int) Double.parseDouble(shared.config(u, "DefaultDaysToLessFromHistoryFromDate")); } catch (Exception e) { days = 0; }
        out.put("defaultDaysToLessFromHistoryFromDate", days);
        out.put("historyCustomers", historyCustomers(u));
        return out;
    }

    /** GenerateCode: the DocNo when > 0, else the box keeps its text (empty string here). */
    private String docNo(UserAccount u) {
        int c = repo.generateCode(u, fy());
        return c > 0 ? String.valueOf(c) : "";
    }

    public Map<String, Object> newCode() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("docNo", docNo(user("View")));
        return m;
    }

    /**
     * Combos: SupplierFill (customer / notify party / commission agent), salesPersonFill (group "9"), DeliveryTermFill,
     * PaymentTermsFill, LoadingPortFill, ImporterandExportBankFill, MultiCurrencyfill (CurrencyCode), ItemDetailFill
     * (config AllItemsBindonExportContract: ItemGetForComboServiceBind() else Item.ReadAllForExportCombo),
     * ItemPackingTypeFill, and at load also CropYearFill and BindOtherItems (GetItemByItemTypeId("14")).
     * The toolbar Refresh (toolStripButton1_Click) re-binds the first nine only.
     */
    private Map<String, Object> lookups(UserAccount u, boolean full) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("customers", pick(shared.exportCustomers(u), "Id", "CompanyName"));
        m.put("salesPersons", pick(shared.customersByGroup(u, "9"), "Id", "CompanyName"));
        m.put("deliveryTerms", pick(shared.deliveryTerms(), "Id", "Code"));
        m.put("paymentTerms", pick(shared.paymentTerms(u), "Id", "LcOrderTerm"));
        m.put("ports", pick(shared.seaPorts(u), "Id", "PortName"));
        List<Map<String, Object>> foreign = new ArrayList<>(), home = new ArrayList<>();
        for (Map<String, Object> b : shared.banks(u)) {
            String h = text(ci(b, "IsHomeland"));
            Map<String, Object> r = row("Id", asInt(ci(b, "Id")), "BranchName", text(ci(b, "BranchName")));
            if ("Foreign Country".equals(h)) foreign.add(r);
            else if ("Home Country".equals(h)) home.add(r);
        }
        m.put("importerBanks", foreign);
        m.put("exporterBanks", home);
        m.put("currencies", pick(shared.currencies(u), "Id", "CurrencyCode"));
        boolean all;
        try { all = asBool(shared.config(u, "AllItemsBindonExportContract")); } catch (Exception e) { all = false; }
        m.put("items", pick(all ? shared.itemsComboTwoColumns(u, 0) : shared.itemsForExportCombo(u), "Id", "ItemName"));
        m.put("packTypes", pick(shared.packTypes(u), "Id", "Description", "PackingWeight"));
        if (full) {
            m.put("cropYears", pick(shared.cropYears(u), "Id", "CropYear"));
            m.put("otherItemList", pick(shared.itemsByType(u, "14"), "Id", "ItemName"));
        }
        return m;
    }

    public Map<String, Object> refresh() { return lookups(user("View"), false); }

    private List<Map<String, Object>> historyCustomers(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : shared.dropDownsFromExportContract(u)) {
            if ("Customer".equals(text(ci(r, "ActivityType")))) out.add(row("Id", asInt(ci(r, "Id")), "Customer", text(ci(r, "name"))));
        }
        return out;
    }

    public List<Map<String, Object>> historyCombos() { return historyCustomers(user("View")); }

    public List<Map<String, Object>> uoms(int itemId) {
        UserAccount u = user("View");
        return ExportPreInvoiceService.uomRows(shared.uomsForItem(u, itemId));
    }

    // ================================================================= ReadById

    public Map<String, Object> readById(int id) {
        UserAccount u = user("View");
        Map<String, Object> h = repo.header(id);
        if (asInt(ci(h, "CompanyId")) != u.getCompanyId() || asInt(ci(h, "OrganizationId")) != u.getOrganizationId())
            throw new AccessDeniedException("Record belongs to another company");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> hd = new LinkedHashMap<>();
        hd.put("Id", asInt(ci(h, "Id")));
        hd.put("Status", text(ci(h, "Status")));
        hd.put("LcOrderDocNo", text(ci(h, "LcOrderDocNo")));
        hd.put("LcOrderDate", iso(ci(h, "LcOrderDate")));
        for (String k : new String[] { "SalesPersonId", "SupCustId", "NotifyPartyId", "CommissionAgentId", "DeliveryTermId", "PaymentTermId",
                "LoadingPortId", "DestinationPortId", "ImporterBankId", "ExporterBankId", "FcurrencyId", "ContractType" }) hd.put(k, asInt(ci(h, k)));
        hd.put("LotReference", text(ci(h, "LotReference")));
        hd.put("CommRate", ExportPreInvoiceService.clr(asDouble(ci(h, "CommRate"))));
        hd.put("CommAmount", ExportPreInvoiceService.clr(asDouble(ci(h, "CommAmount"))));
        hd.put("LcOrderNo", text(ci(h, "LcOrderNo")));
        hd.put("SalesContratDate", iso(ci(h, "SalesContratDate")));
        hd.put("LastShipmentDate", iso(ci(h, "LastShipmentDate")));
        hd.put("NoOfContainers", ExportPreInvoiceService.clr(asDouble(ci(h, "NoOfContainers"))));
        hd.put("InsuranceRemarks", text(ci(h, "InsuranceRemarks")));
        hd.put("FCurrencyAmount", ExportPreInvoiceService.clr(asDouble(ci(h, "FCurrencyAmount"))));
        hd.put("NetWeightKgs", ExportPreInvoiceService.clr(asDouble(ci(h, "NetWeightKgs"))));
        hd.put("CommodityDetial", text(ci(h, "CommodityDetial")));
        hd.put("RemarksHeader", text(ci(h, "RemarksHeader")));
        hd.put("ProductSpecification", text(ci(h, "ProductSpecification")));
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : repo.detail(id, asInt(ci(h, "DocumentTypeId")))) rows.add(detailRow(d));
        /* txtcontainersizedetail.Text = Obj.LcOrderDetail[0].ContainerSize - throws when the contract has no detail rows. */
        if (rows.isEmpty()) throw new IllegalStateException("Index was out of range. Must be non-negative and less than the size of the collection.");
        hd.put("ContainerSize", text(rows.get(0).get("ContainerSize")));
        out.put("header", hd);
        out.put("rows", rows);
        List<Map<String, Object>> other = new ArrayList<>();
        for (Map<String, Object> r : repo.otherItems(id)) {
            other.add(row("ItemId", asInt(ci(r, "otherItemId")), "ItemName", text(ci(r, "ItemName")), "Qty", asDouble(ci(r, "oItemQty")),
                    "Rate", asDouble(ci(r, "oItemRate")), "Amount", asDouble(ci(r, "oItemAmount")), "Remarks", text(ci(r, "OtherItemRemarks"))));
        }
        out.put("otherItems", other);
        return out;
    }

    /** One dtdetail row (ReadById's Rows.Add): NetWeight / 1000 into "Qty/M.Ton", CommodityDetail into "PackingDetail". */
    private static Map<String, Object> detailRow(Map<String, Object> d) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(ci(d, "Id")));
        m.put("ItemId", asInt(ci(d, "ExImItemId")));
        m.put("ItemName", text(ci(d, "ItemName")));
        m.put("PackTypeId", asInt(ci(d, "InvPackingMaterialTypeId")));
        m.put("PackType", text(ci(d, "ExImPackMaterialTypeName")));
        m.put("PackingWeight", asDouble(ci(d, "PackingWeight")));
        m.put("CropYearId", asInt(ci(d, "CropYearId")));
        m.put("CropYear", text(ci(d, "CropYear")));
        m.put("NoOfContainers", asDouble(ci(d, "NoofContainer")));
        m.put("NoOfBagsCntnr", asDouble(ci(d, "NoofBagsPerContainer")));
        m.put("PackSizeId", asInt(ci(d, "UOMScheduleIdOuter")));
        m.put("PackSize", text(ci(d, "PackUom")));
        m.put("QtyMTon", asDouble(ci(d, "NetWeight")) / 1000.0);
        m.put("NoOfBags", asDouble(ci(d, "NoOfUnit")));
        m.put("PackingTotalWeight", asDouble(ci(d, "PackingTotalWeight")));
        m.put("GrossWeight", asDouble(ci(d, "GrossWeight")));
        m.put("Rate", asDouble(ci(d, "RatePrice")));
        m.put("RateUOMId", asInt(ci(d, "UOMScheduleIdRate")));
        m.put("RateUOM", text(ci(d, "RateUom")));
        m.put("Amount", asDouble(ci(d, "TotalAmount")));
        m.put("PackingExpiryDate", text(ci(d, "PackingExpiryDate")));
        m.put("ContainerSize", text(ci(d, "ContainerSize")));
        m.put("Remarks", text(ci(d, "Other_Description")));
        m.put("PackingDetail", text(ci(d, "CommodityDetail")));
        return m;
    }

    /** GetDetailGrdByHeadId - the lower history grid. */
    public List<Map<String, Object>> historyDetail(int id) {
        user("View");
        Map<String, Object> h = repo.header(id);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : repo.detail(id, asInt(ci(h, "DocumentTypeId")))) out.add(detailRow(d));
        return out;
    }

    // ================================================================= history

    public List<Map<String, Object>> history(Map<String, Object> f) {
        UserAccount u = user("View");
        boolean viewAll = allowed(u, "CanView AllRecord");
        Map<String, Object> g = new LinkedHashMap<>();
        String by = text(f.get("dateBy"));
        String fromKey = "FromDate", toKey = "ToDate";
        if ("entry".equals(by)) { fromKey = "EntryFromDate"; toKey = "EntryToDate"; }
        else if ("modify".equals(by)) { fromKey = "ModifyFromDate"; toKey = "ModifyToDate"; }
        else if ("approved".equals(by)) { fromKey = "ApprovedFromDate"; toKey = "ApprovedToDate"; }
        LocalDate from = asBool(f.get("fromChecked")) ? asDate(f.get("fromDate")) : null;
        LocalDate to = asBool(f.get("toChecked")) ? asDate(f.get("toDate")) : null;
        if (from != null) g.put(fromKey, Timestamp.valueOf(LocalDateTime.of(from, LocalTime.now().withNano(0))));
        if (to != null) g.put(toKey, Timestamp.valueOf(LocalDateTime.of(to, LocalTime.now().withNano(0))));
        int fromDoc = cTextInt(f.get("fromDocNo")), toDoc = cTextInt(f.get("toDocNo"));
        if (fromDoc != 0) g.put("DocNoFrom", fromDoc);
        if (toDoc != 0) g.put("DocNoTo", toDoc);
        int cust = asInt(f.get("customerId"));
        if (cust != 0) g.put("SupplierCustomerId", cust);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.history(u, viewAll, fy(), g)) {
            out.add(row("Id", asInt(ci(r, "Id")), "DocNo", asInt(ci(r, "LcOrderDocNo")), "DocDate", iso(ci(r, "LcOrderDate")),
                    "PriRef", text(ci(r, "LcOrderNo")), "PriRefDate", iso(ci(r, "SalesContratDate")), "Customer", text(ci(r, "CustomerName")),
                    "NotifyParty", text(ci(r, "NotifyParty")), "LotRefNo", text(ci(r, "LotReference")), "PaymentTerm", text(ci(r, "PaymentTerm")),
                    "DeliveryTerm", text(ci(r, "DeliveryTerm")), "NetMTon", asDouble(ci(r, "NetKgs")), "FcyCode", text(ci(r, "CurrencyCode")),
                    "FcyAmount", asDouble(ci(r, "FCurrencyAmount")), "NoOfContainers", asDouble(ci(r, "NoOfContr")),
                    "LoadingPort", text(ci(r, "LoadingPort")), "DestinationPort", text(ci(r, "PortName")),
                    "InsuranceRemarks", text(ci(r, "InsuranceRemarks")), "NoOfAttachments", text(ci(r, "NoOfAttachments"))));
        }
        return out;
    }

    // ================================================================= save

    /**
     * btnsave_Click (Update and SaveAs call it; SaveAs first sets RecId = 0). New records regenerate the Doc No;
     * FormValidation in the desktop's order; the Status check; "Detail Grid record not found" when the grid is
     * empty; removed rows (update only) first with ActionTypeId 3, then the grid (Id kept only when updating;
     * 1 / 2), then the other items; ExImLcOrder.Save; "Record Save Successfully" / "Record Update Successfully".
     */
    public Map<String, Object> save(Map<String, Object> body) {
        int recId = asInt(body.get("recId"));
        UserAccount u = user(recId > 0 ? "Update" : "Save");
        Map<String, Object> h = map(body.get("header"));
        List<Map<String, Object>> rows = list(body.get("rows"));
        List<Map<String, Object>> removed = list(body.get("removed"));
        List<Map<String, Object>> others = list(body.get("otherItems"));
        String docNo = raw(h.get("txtExportSoNo"));
        if (recId == 0) docNo = docNo(u);
        else {
            Map<String, Object> old = repo.header(recId);
            if (asInt(ci(old, "CompanyId")) != u.getCompanyId() || asInt(ci(old, "OrganizationId")) != u.getOrganizationId()
                    || asInt(ci(old, "DocumentTypeId")) != DOCUMENT_TYPE_ID)
                throw new IllegalArgumentException("Record not found");
        }
        h.put("txtExportSoNo", docNo);
        validate(h);
        boolean updateVisible = asBool(body.get("updateVisible"));
        boolean updateEnabled = allowed(u, "Update");
        if (!updateVisible && !updateEnabled && !"Open".equals(raw(h.get("cmbStatusText")).trim()))
            throw new IllegalArgumentException("Status Should Be Open In Save Mode");

        Map<String, Object> m = lcOrderModel();
        if (recId > 0) m.put("Id", recId);
        m.put("Status", raw(h.get("cmbStatusText")));
        m.put("LcOrderDocNo", cTextInt(docNo.trim()));
        m.put("LcOrderDate", ts(h.get("txtDocDate")));
        m.put("SalesPersonId", asInt(h.get("CmbSalePerson")));
        m.put("SupCustId", asInt(h.get("CmbCustomer")));
        m.put("NotifyPartyId", asInt(h.get("CmbNotifyParty")));
        m.put("LotReference", text(h.get("txtLotRefNo")));
        m.put("LcOrderNo", text(h.get("txtBuyerOrderNo")));
        m.put("SalesContratDate", ts(h.get("datOrderDate")));
        m.put("LastShipmentDate", ts(h.get("txtShipmentDate")));
        m.put("DeliveryTermId", asInt(h.get("CmbDeliveryTerm")));
        m.put("PaymentTermId", asInt(h.get("CmbPaymentTerms")));
        m.put("LoadingPortId", asInt(h.get("CmbLoadingPort")));
        m.put("DestinationPortId", asInt(h.get("CmbDestinationPort")));
        m.put("ImporterBankId", asInt(h.get("CmbImporterBank")));
        m.put("ExporterBankId", asInt(h.get("CmbExporterbank")));
        m.put("NoOfContainers", cDbl(h.get("txtNoOfContainers")));
        m.put("FcurrencyId", asInt(h.get("CmbFCYCode")));
        m.put("FCurrencyAmount", cDbl(h.get("txtFCYAmount")));
        m.put("CommissionAgentId", asInt(h.get("cmbCommAgent")));
        m.put("CommRate", cDbl(h.get("txtCommPercent")));
        m.put("CommAmount", cDbl(h.get("txtCommAmount")));
        m.put("TotalAmountWithAdls", cDbl(h.get("txtFCYAmount")));
        m.put("NetWeightKgs", cDbl(h.get("txtMTon")));
        m.put("CommodityDetial", text(h.get("txtCommodityDetail")));
        m.put("RemarksHeader", text(h.get("txtSpecialInstructions")));
        m.put("InsuranceRemarks", text(h.get("txtInsuraneRemarks")));
        m.put("ProductSpecification", text(h.get("txtProductSpecification")));
        m.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        m.put("OrganizationId", u.getOrganizationId());
        m.put("CompanyId", u.getCompanyId());
        m.put("BranchesId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        Timestamp now = Timestamp.valueOf(LocalDateTime.now().withNano(0));
        m.put("EntryDate", now);
        m.put("ModifyDate", now);
        m.put("EntryUser", u.getId());
        m.put("ModifyUser", u.getId());
        m.put("FinancialYearId", fy());
        m.put("ActionId", recId == 0 ? 1 : 2);                                    // BLL ExImLcOrder.Save

        List<Map<String, Object>> details = new ArrayList<>();
        if (recId > 0) for (Map<String, Object> r : removed) {
            Map<String, Object> d = detailModel(r, recId);
            d.put("ActionTypeId", 3);
            details.add(d);
        }
        if (rows.isEmpty()) throw new IllegalArgumentException("Detail Grid record not found");
        for (Map<String, Object> r : rows) {
            Map<String, Object> d = detailModel(r, recId);
            d.put("ActionTypeId", asInt(d.get("Id")) <= 0 ? 1 : 2);
            details.add(d);
        }
        List<Map<String, Object>> otherModels = new ArrayList<>();
        for (Map<String, Object> o : others) {
            Map<String, Object> om = new LinkedHashMap<>();
            om.put("oItemAmount", dec(o.get("Amount")));
            om.put("oItemQty", dec(o.get("Qty")));
            om.put("oItemWeightKgs", BigDecimal.ZERO);
            om.put("oItemRate", asDouble(o.get("Rate")));
            om.put("ExImLcOrderId", 0);
            om.put("Id", 0);
            om.put("otherItemId", asInt(o.get("ItemId")));
            om.put("OtherItemRemarks", text(o.get("Remarks")));
            otherModels.add(om);
        }
        int id = repo.save(m, recId == 0 ? "Sp_ExImLcOrder_Insert" : "Sp_ExImLcOrder_Update", details, otherModels);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", id);
        out.put("docNo", docNo);
        out.put("message", recId == 0 ? "Record Save Successfully" : "Record Update Successfully");
        return out;
    }

    /** FormValidation() - texts and order of the desktop (combos are checked by their text, as there). */
    private static void validate(Map<String, Object> h) {
        String doc = raw(h.get("txtExportSoNo")).trim();
        if (doc.isEmpty() || doc.equals("0")) throw new IllegalArgumentException("DocNo Is required");
        if (raw(h.get("CmbSalePersonText")).trim().isEmpty()) throw new IllegalArgumentException("Sale Person Is required");
        if (raw(h.get("CmbCustomerText")).trim().isEmpty()) throw new IllegalArgumentException("Customer/Importer Is required");
        if (raw(h.get("txtBuyerOrderNo")).trim().isEmpty()) throw new IllegalArgumentException("Proforma Ref # Is required");
        if (raw(h.get("CmbDeliveryTermText")).trim().isEmpty()) throw new IllegalArgumentException("DeliveryTerms Is  required");
        if (raw(h.get("CmbPaymentTermsText")).trim().isEmpty()) throw new IllegalArgumentException("PaymentTerms Is required");
        String lp = raw(h.get("CmbLoadingPortText")).trim(), dp = raw(h.get("CmbDestinationPortText")).trim();
        if (lp.isEmpty()) throw new IllegalArgumentException("LoadingPort Is required");
        if (dp.isEmpty()) throw new IllegalArgumentException("Destination Port  Is required");
        if (dp.equals(lp)) throw new IllegalArgumentException("Destination Port and Loaing Port cannot be same...");
        boolean agent = asInt(h.get("cmbCommAgent")) != 0;
        if (cDbl(h.get("txtCommPercent")) > 0.0 && !agent)
            throw new IllegalArgumentException("As you have entered 'Commission %', the 'Commission Agent' field is required. ");
        if (cDbl(h.get("txtCommPercent")) == 0.0 && agent)
            throw new IllegalArgumentException("As you have selected 'Commission agent', please enter the commission percentage.");
        if (raw(h.get("CmbFCYCodeText")).trim().isEmpty()) throw new IllegalArgumentException("Fcy Code Is  required");
        if (!(cDbl(h.get("txtFCYAmount")) > 0.0) || raw(h.get("txtFCYAmount")).trim().isEmpty())
            throw new IllegalArgumentException("FCY Amount Must Be Greater Than 0 ThankYou");
        if (!(cDbl(h.get("txtMTon")) > 0.0)) throw new IllegalArgumentException("M.Ton Must Be Greater Than 0 ThankYou");
    }

    /** Architecture.Model.Export.ExImLcOrder - the 89 non-virtual properties with their CLR defaults. */
    private static Map<String, Object> lcOrderModel() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("IsApproved", false);
        m.put("PostState", false);
        m.put("EntryDate", null);
        for (String k : new String[] { "ExpiryDate", "LastShipmentDate", "LcOrderDate" }) m.put(k, null);
        m.put("ModifyDate", null);
        for (String k : new String[] { "PostDate", "SalesContratDate", "ShipmentStartDate", "CustomerApprovalDate", "FactoryLoadingDate" }) m.put(k, null);
        for (String k : new String[] { "ConversionRate", "EquivalentAmount", "FCurrencyAmount", "GrossWeightKgs", "NetWeightKgs", "NoOfContainers" }) m.put(k, 0d);
        for (String k : new String[] { "BranchesId", "CompanyId", "DeliveryTermId", "DestinationPortId", "EntryUser", "ExImProformaInvoice",
                "ExporterBankId", "ExportCompanyId", "FcurrencyId", "CareierTypeId", "Id", "ImporterBankId", "LcOrderDocNo", "LcOrderStatusId",
                "LoadingPortId", "ModifyUser", "NoOfShipments", "NotifyPartyId", "NotifyParty2Id", "NotifyParty3Id", "OrganizationId",
                "PaymentTermId", "PostUser", "ProductionSchDaysBefore", "ProjectsId", "SalesPersonId", "SampleSchDaysBefore",
                "ShipmentIntervalDays", "SupCustId", "ConsigneeId", "ContractType", "CommissionAgentId", "DeliveryDays", "ActionId" }) m.put(k, 0);
        m.put("CommRate", 0d);
        m.put("CommAmount", 0d);
        for (String k : new String[] { "CommodityDetial", "PaymentRemarks", "DeliveryRemarks", "CustomerCode", "ExpiryPlace", "FcurrencySymbol",
                "InquiryReference", "InsepctionDescription", "InsepctionRequired", "LcDocAttachment", "LcOrderNo", "CustomerOrderNo",
                "LegalizationDescription", "LegalizationRequired", "LotReference", "PartialShipment", "QuotReference", "RemarksHeader",
                "SalesContractRef", "ShippingMarks", "Status", "TransShipment", "InsuranceRemarks", "CustomerApprovalRemarks", "ContactPerson" }) m.put(k, null);
        for (String k : new String[] { "DocumentTypeId", "ShipedToId", "FinancialYearId", "CustomGroupId" }) m.put(k, 0);
        for (String k : new String[] { "ProductSpecification", "AddLessComments", "AttachmentsValues", "CustomAttachmentsValues" }) m.put(k, null);
        m.put("AddLessAmount", 0d);
        m.put("TotalAmountWithAdls", 0d);
        return m;
    }

    /** ExImLcOrderPackingDetail (34 properties) as btnsave_Click / grdDetails_ColumnButtonClick fill it. */
    private static Map<String, Object> detailModel(Map<String, Object> r, int recId) {
        Map<String, Object> d = new LinkedHashMap<>();
        for (String k : new String[] { "InnerQty", "NetWeight", "NoOfUnit", "OuterQty", "RatePrice", "TotalAmount", "NoofContainer",
                "PackingWeight", "NoofBagsPerContainer", "PackingTotalWeight", "GrossWeight", "PalletQty" }) d.put(k, 0d);
        for (String k : new String[] { "ExImItemId", "PmItemId", "ExImLcOrderId", "CropYearId", "Id", "AmountCalulationId",
                "InvPackingMaterialTypeId", "ItemSpecificationId", "ItemBrandId", "UOMScheduleIdInner", "UOMScheduleIdOuter",
                "UOMScheduleIdRate", "ActionTypeId", "ExImFarmingNTradeId", "ExImTradeTypeId", "ExImFarmingTypeId" }) d.put(k, 0);
        for (String k : new String[] { "Other_Description", "OuterPackDescription", "ContainerSize", "PackingExpiryDate", "CommodityDetail",
                "PackingDetail" }) d.put(k, null);
        double mton = asDouble(r.get("QtyMTon"));
        d.put("Id", recId > 0 ? asInt(r.get("Id")) : 0);
        d.put("ExImItemId", asInt(r.get("ItemId")));
        d.put("InvPackingMaterialTypeId", asInt(r.get("PackTypeId")));
        d.put("PackingWeight", asDouble(r.get("PackingWeight")));
        d.put("CropYearId", asInt(r.get("CropYearId")));
        d.put("NoofContainer", asDouble(r.get("NoOfContainers")));
        d.put("NoofBagsPerContainer", asDouble(r.get("NoOfBagsCntnr")));
        d.put("UOMScheduleIdOuter", asInt(r.get("PackSizeId")));
        d.put("UOMScheduleIdInner", asInt(r.get("PackSizeId")));
        d.put("NetWeight", mton * 1000.0);
        d.put("NoOfUnit", asDouble(r.get("NoOfBags")));
        d.put("OuterQty", mton);
        d.put("InnerQty", mton);
        d.put("PackingTotalWeight", asDouble(r.get("PackingTotalWeight")));
        d.put("GrossWeight", asDouble(r.get("GrossWeight")));
        d.put("UOMScheduleIdRate", asInt(r.get("RateUOMId")));
        d.put("RatePrice", asDouble(r.get("Rate")));
        d.put("TotalAmount", asDouble(r.get("Amount")));
        d.put("ContainerSize", raw(r.get("ContainerSize")));
        d.put("PackingExpiryDate", raw(r.get("PackingExpiryDate")));
        d.put("Other_Description", raw(r.get("Remarks")));
        d.put("CommodityDetail", raw(r.get("PackingDetail")));
        return d;
    }
}
