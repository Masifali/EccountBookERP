package com.mst.services;

import com.mst.repositories.InwardGatePassRecordRepository;
import com.mst.repositories.PaddyGrnRepository;
import com.mst.repositories.PaddyPurchaseRepository;
import com.mst.repositories.PurchaseGrnWriteRepository;
import com.mst.security.CurrentUserContext;
import java.util.*;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

/**
 * Screen 465 "Goods Receiving Notes For Gate Purchase" - Architecture.WinApp.Paddy_Purchase.frmGrnForGate
 * (Paddy Purchase module 48, InvGrn DocumentTypeId 165, gate pass DocumentTypeId 51).
 * Line references in the comments are to frmGrnForGate.cs; the save chain is BLL InvGrn.Save -> DAL InvGrn.SetData.
 */
@Service
public class PaddyGrnGateService {
    public static final String SCREEN = "frmGrnForGate";
    public static final int DOCUMENT_TYPE_ID = 165, GATE_PASS_DOCUMENT_TYPE_ID = 51;

    @Autowired private PaddyGrnRepository grn;
    @Autowired private PaddyPurchaseRepository paddy;
    @Autowired private PurchaseGrnWriteRepository writes;
    @Autowired private InwardGatePassRecordRepository rights;
    @Autowired private CurrentUserContext context;
    @Autowired private DesktopAttachmentStore attachmentStore;
    @Autowired private JdbcTemplate jdbc;

    private static int num(Object v) { return v instanceof Number ? ((Number) v).intValue() : (v == null || v.toString().isBlank() ? 0 : (int) Double.parseDouble(v.toString().trim())); }
    private static double dbl(Object v) {
        if (v instanceof Number) return ((Number) v).doubleValue();
        try { return v == null || v.toString().isBlank() ? 0 : Double.parseDouble(v.toString().replace(",", "").trim()); } catch (NumberFormatException e) { return 0; }
    }
    private static String text(Object v) { return v == null ? "" : v.toString().trim(); }
    private static Object pick(Map<String, Object> row, String key) {
        if (row == null) return null;
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }
    private static Map<String, Object> option(Object id, Object name) { Map<String, Object> m = new LinkedHashMap<>(); m.put("id", id); m.put("name", name); return m; }
    private static boolean flag(Object v) { String s = text(v); return s.equalsIgnoreCase("true") || s.equals("1") || s.equalsIgnoreCase("yes"); }
    private int org() { return context.currentOrganizationId(); }
    private int company() { return context.currentCompanyId(); }

    /** InvFrmGRN_Load (:1620): GetConfiguration, rights, BranchFill, ProjectFill, GenerateCode, SupplierNameFilll, ContractorBind, TransportFill, VehicleTypesFill, CropYear, Status, JobLotFillWithoutOrder, PackingTypeFill, WarehouseFill, CityFill, Gatepass, defaultConfiquration. */
    public Map<String, Object> init() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("canSave", rights.hasScreenRight(SCREEN, "Save"));
        out.put("canUpdate", rights.hasScreenRight(SCREEN, "Update"));
        out.put("canPrint", rights.hasScreenRight(SCREEN, "Print"));
        out.put("config", config());
        out.putAll(lookups());
        out.put("docNo", nextDocNo());
        out.put("gatePasses", pendingGatePasses());
        return out;
    }

    /** GetConfiguration (:1124) + defaultConfiquration (:1069): the five keys read by the form, and the Job/Lot, Paking Type and FreightInwardAc defaults of clsGlobalVariables.configrationsAllocation. */
    public Map<String, Object> config() {
        Map<String, String> values = new LinkedHashMap<>();
        for (String k : List.of("WeightCutForJuteBags", "WeightCutForPPBags", "WeightCutForOpenBulk", "EmptyBagsWeightCutEditableOnGRN", "LabCompulsoryForWeighBridgeAgainstGatePurchase", "Job/Lot", "Paking Type", "FreightInwardAc")) values.put(k, "");
        for (Map<String, Object> r : grn.configurations(org(), company(), String.join(",", values.keySet()))) {
            String name = text(pick(r, "ConfigDescription"));
            for (String k : values.keySet()) if (k.equalsIgnoreCase(name)) values.put(k, text(pick(r, "ConfigKey")));
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("juteBagsCut", dbl(values.get("WeightCutForJuteBags")));
        out.put("ppBagsCut", dbl(values.get("WeightCutForPPBags")));
        out.put("openBulkCut", dbl(values.get("WeightCutForOpenBulk")));
        out.put("emptyBagsWeightCutEditable", flag(values.get("EmptyBagsWeightCutEditableOnGRN")));
        out.put("labCompulsory", flag(values.get("LabCompulsoryForWeighBridgeAgainstGatePurchase")));
        out.put("defaultJobLot", num(values.get("Job/Lot")));
        out.put("defaultPackingType", num(values.get("Paking Type")));
        out.put("defaultTransporter", num(values.get("FreightInwardAc")));
        return out;
    }

    /** The combo fills of InvFrmGRN_Load and toolStripButton1_Click (:2210). */
    public Map<String, Object> lookups() {
        int org = org(), company = company();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> list = new ArrayList<>();
        for (Map<String, Object> r : grn.branches(org, company)) list.add(option(num(pick(r, "Id")), text(pick(r, "BranchName"))));
        out.put("branches", list);
        list = new ArrayList<>();
        for (Map<String, Object> r : grn.projects(org, company)) list.add(option(num(pick(r, "Id")), text(pick(r, "ProjectName"))));
        out.put("projects", list);
        list = new ArrayList<>();
        for (Map<String, Object> r : paddy.suppliersByGroupIds(org, company, "1,2,3", 0)) list.add(option(num(pick(r, "Id")), text(pick(r, "CompanyName"))));
        out.put("suppliers", list);
        list = new ArrayList<>();
        for (Map<String, Object> r : paddy.suppliersByGroupIds(org, company, "8", 0)) list.add(option(num(pick(r, "Id")), text(pick(r, "CompanyName"))));
        out.put("contractors", list);
        list = new ArrayList<>();
        for (Map<String, Object> r : grn.accountsByTypeIds(org, company, context.currentAppId(), context.currentUserId(), "3")) list.add(option(num(pick(r, "Id")), text(pick(r, "AccountTitle"))));
        out.put("transporters", list);
        list = new ArrayList<>();
        for (Map<String, Object> r : paddy.vehicleTypes()) list.add(option(num(pick(r, "Id")), text(pick(r, "VehicleDescription"))));
        out.put("vehicleTypes", list);
        list = new ArrayList<>();
        for (Map<String, Object> r : grn.cropYears(org, company)) list.add(option(num(pick(r, "Id")), text(pick(r, "CropYear"))));
        out.put("cropYears", list);
        list = new ArrayList<>();
        for (Map<String, Object> r : grn.jobLots(org, company)) list.add(option(num(pick(r, "Id")), text(pick(r, "JobLotDescription"))));
        out.put("jobLots", list);
        list = new ArrayList<>();
        for (Map<String, Object> r : grn.packingTypes()) { int id = num(pick(r, "Id")); if (id == 1 || id == 2 || id == 5) list.add(option(id, text(pick(r, "PackTypeDesc")))); }   // PackingTypeFill (:1126): only Id 1, 2, 5
        out.put("packingTypes", list);
        list = new ArrayList<>();
        for (Map<String, Object> r : grn.activeWarehouses(org, company)) list.add(option(num(pick(r, "Id")), text(pick(r, "WareHouseName"))));
        out.put("warehouses", list);
        list = new ArrayList<>();
        for (Map<String, Object> r : paddy.cities(org, company)) list.add(option(num(pick(r, "Id")), text(pick(r, "CityName"))));
        out.put("cities", list);
        out.put("statuses", List.of(option(1, "Accepted"), option(2, "Rejected")));          // Status (:1268)
        list = new ArrayList<>();
        for (Map<String, Object> r : grn.itemsByItemTypeIds(org, company, "14")) list.add(option(num(pick(r, "Id")), text(pick(r, "ItemName"))));
        out.put("emptyBagItems", list);                                                        // EmptyBagsGridRefresh (:1586)
        list = new ArrayList<>();
        for (Map<String, Object> r : grn.staticColumns("PurchaseOrderEmptyBagsType")) list.add(option(num(pick(r, "Id")), text(pick(r, "Type"))));
        out.put("emptyBagTypes", list);                                                        // EmptyBagsTypeFill (:1459)
        list = new ArrayList<>();
        for (Map<String, Object> r : grn.staticColumns("EmptyBagsCondition")) list.add(option(num(pick(r, "Id")), text(pick(r, "Type"))));
        out.put("bagConditions", list);
        return out;
    }

    /** GenerateCode (:1108): InvGrn.GenerateInvGrnCode(165). */
    public int nextDocNo() { return grn.generateInvGrnCode(org(), company(), DOCUMENT_TYPE_ID, context.currentFinancialYearId()); }

    /** Gatepass (:1213): GatePassInward.GetPandingGatePassforInvGrnForGatePurchase(DocumentTypeId 51); grdGp columns. */
    public List<Map<String, Object>> pendingGatePasses() {
        return grn.pendingGatePassesForGate(org(), company(), GATE_PASS_DOCUMENT_TYPE_ID, context.currentFinancialYearId());
    }

    /** GatePassRecordFill (:1331): GatePassInward.GetByGpNoForGRN, then the lab rows when LabCompulsoryForWeighBridgeAgainstGatePurchase. */
    public Map<String, Object> gatePassRecord(int gpId) {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> rows = grn.gatePassForGrn(org(), company(), gpId, GATE_PASS_DOCUMENT_TYPE_ID, context.currentFinancialYearId());
        out.put("found", !rows.isEmpty());
        if (rows.isEmpty()) return out;
        out.put("gp", rows.get(0));
        out.put("lab", labRows(gpId));
        return out;
    }

    private List<Map<String, Object>> labRows(int gpId) {
        if (!flag(config().get("labCompulsory"))) return new ArrayList<>();
        return grn.labDataForGate(org(), company(), gpId);
    }

    /** PackUOMFillWithoutOrder (:1170): CommonServices.GetUomScheduleByItemId (Id, UOMCode, Equivalent, QtyEquivalent, BaseRateUom). */
    public List<Map<String, Object>> uoms(int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : grn.uomScheduleByItem(org(), company(), itemId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", num(pick(r, "Id"))); m.put("UOMCode", text(pick(r, "UOMCode"))); m.put("Equivalent", dbl(pick(r, "Equivalent")));
            m.put("QtyEquivalent", dbl(pick(r, "QtyEquivalent"))); m.put("BaseRateUom", flag(pick(r, "BaseRateUom")));
            out.add(m);
        }
        return out;
    }

    /** HistoryGridFill (:1788): InvGrn.GetHisoty(DocumentTypeId 165, CanViewAllRecord, ActiveYr); the grid takes 18 named columns. */
    public List<Map<String, Object>> history(int noOfRecords) {
        boolean canViewAll = rights.hasScreenRight(SCREEN, "CanView AllRecord");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : grn.grnHistory(org(), company(), DOCUMENT_TYPE_ID, canViewAll, context.currentFinancialYearId(), context.currentUserId(), noOfRecords)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", num(pick(r, "Id"))); m.put("DocDate", pick(r, "DocDate")); m.put("DocNo", pick(r, "DocNo")); m.put("SupplierName", pick(r, "SupplierName"));
            m.put("GpNo", pick(r, "GpNo")); m.put("InwardGatePassId", num(pick(r, "InwardGatePassId"))); m.put("VehicleNo", pick(r, "VehicleNo")); m.put("BiltyNo", pick(r, "BiltyNo"));
            m.put("FactoryWeight", dbl(pick(r, "FactoryWeight"))); m.put("PartyWeight", dbl(pick(r, "PartyWeight"))); m.put("TransporterName", pick(r, "Transporter"));
            m.put("Freight", dbl(pick(r, "CarriageAmount"))); m.put("Remarks", pick(r, "RemarksHeader")); m.put("EntryDate", pick(r, "EntryDate")); m.put("EntryUser", pick(r, "EntryUser"));
            m.put("ModifyDate", pick(r, "ModifyDate")); m.put("ModifyUser", pick(r, "ModifyUser")); m.put("NoOfAttachments", pick(r, "NoOfAttachments"));
            out.add(m);
        }
        return out;
    }

    private Map<String, Object> requireRecord(int id) {
        Map<String, Object> header = grn.grnHeader(id);
        if (header == null || num(pick(header, "OrganizationId")) != org() || num(pick(header, "CompanyId")) != company() || num(pick(header, "DocumentTypeId")) != DOCUMENT_TYPE_ID)
            throw new IllegalArgumentException("Record not found");
        return header;
    }

    /** GrdHistory_SelectionChanged (:2445): InvGrn.GetByID rows into the 14 columns of GrdHistoryDetail. */
    public List<Map<String, Object>> historyDetail(int id) {
        requireRecord(id);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : grn.grnDetails(id)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Item", pick(r, "Item")); m.put("CropYear", pick(r, "CropYear")); m.put("Job", pick(r, "JobLot")); m.put("PackingType", pick(r, "PackingType")); m.put("UOM", pick(r, "UOM"));
            m.put("Qty", dbl(pick(r, "ItemQty"))); m.put("GrossWeight", dbl(pick(r, "GrossWeight"))); m.put("EbUnit", dbl(pick(r, "EBWPerUnit"))); m.put("EbTotal", dbl(pick(r, "EBWTotal")));
            m.put("NetWeight", dbl(pick(r, "NetBillWeight"))); m.put("WareHouse", pick(r, "WareHouseName")); m.put("LabNo", pick(r, "LabReportRef")); m.put("City", pick(r, "AreaCity")); m.put("Contractor", pick(r, "Contractor"));
            out.add(m);
        }
        return out;
    }

    /** ReadById_Update (:565): InvGrn.GetByID(Id); the grid rows keep the dtdetail / dtEmptyBag column names of the form. */
    public Map<String, Object> getById(int id) {
        Map<String, Object> h = requireRecord(id);
        Map<String, Object> header = new LinkedHashMap<>();
        for (String k : List.of("BranchesId", "ProjectsId", "DocNo", "DocDate", "SupplierCustomerId", "TransporterId", "CarriageAmount", "PartyWeight", "FactoryWeight", "ScaleKart", "GpNo", "VehicleType", "VehicleNo", "BiltyNo", "RemarksHeader", "InwardGatePassId", "Status"))
            header.put(k, pick(h, k));
        List<Map<String, Object>> details = new ArrayList<>();
        for (Map<String, Object> r : grn.grnDetails(id)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ItemId", num(pick(r, "ItemId"))); m.put("Item", text(pick(r, "Item"))); m.put("CropYear", text(pick(r, "CropYear")));
            m.put("JobId", num(pick(r, "JobLotId"))); m.put("Job", text(pick(r, "JobLot")));
            m.put("PackingTypeId", num(pick(r, "PackingTypeId"))); m.put("PackingType", text(pick(r, "PackingType")));
            m.put("UOM", text(pick(r, "UOM"))); m.put("UOMId", num(pick(r, "ItemUomId")));
            m.put("Qty", dbl(pick(r, "ItemQty"))); m.put("GrossWeight", dbl(pick(r, "GrossWeight"))); m.put("ScaleKart", dbl(pick(r, "ScaleKart")));
            m.put("EbUnit", dbl(pick(r, "EBWPerUnit"))); m.put("EbTotal", dbl(pick(r, "EBWTotal"))); m.put("NetWeight", dbl(pick(r, "NetBillWeight")));
            m.put("WareHouse", num(pick(r, "WarehouseId"))); m.put("LabNo", text(pick(r, "LabReportRef"))); m.put("City", text(pick(r, "AreaCity")));
            m.put("ContractorId", num(pick(r, "ContractorId"))); m.put("Contractor", text(pick(r, "Contractor")));
            details.add(m);
        }
        List<Map<String, Object>> bags = new ArrayList<>();
        for (Map<String, Object> r : grn.grnEmptyBags(id)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("OrderId", num(pick(r, "PurchaseOrderId"))); m.put("Type", num(pick(r, "TypeId"))); m.put("ItemId", num(pick(r, "ItemId"))); m.put("Condition", num(pick(r, "BagsCondition")));
            m.put("RecQty", dbl(pick(r, "ReceivedQty"))); m.put("PurQty", dbl(pick(r, "PurchaseQty"))); m.put("Remarks", text(pick(r, "Remarks")));
            bags.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", header);
        out.put("details", details);
        out.put("emptyBags", bags);
        out.put("lab", labRows(num(pick(h, "InwardGatePassId"))));
        out.put("canUpdate", rights.hasScreenRight(SCREEN, "Update"));
        return out;
    }

    /** Insert (:300) .. btnSave_Click / btnUpdate_Click: BLL InvGrn.Save -> DAL SetData (header, details, empty bags, inventory transactions, stock in transit). */
    @Transactional
    public Map<String, Object> save(Map<String, Object> body) {
        int id = num(body.get("id"));
        boolean updating = id > 0;
        if (!rights.hasScreenRight(SCREEN, updating ? "Update" : "Save")) throw new SecurityException("You do not have the " + (updating ? "Update" : "Save") + " right on this screen");
        int org = org(), company = company(), year = context.currentFinancialYearId(), user = context.currentUserId();
        if (updating) requireRecord(id);

        // FormValidation (:707), same order and text
        int docNo = num(body.get("docNo"));
        if (docNo <= 0) throw new IllegalArgumentException("DocNo Field is Required");
        int branchId = num(body.get("branchId")), projectId = num(body.get("projectId")), supplierId = num(body.get("supplierId"));
        if (branchId <= 0) throw new IllegalArgumentException("Branch Field is Required");
        if (projectId <= 0) throw new IllegalArgumentException("Project Field is Required");
        if (supplierId <= 0) throw new IllegalArgumentException("Supplier Field is Required");
        int gpNo = num(body.get("gpNo"));
        if (gpNo == 0) throw new IllegalArgumentException("Gatepass Field is Required");
        double suppWt = dbl(body.get("suppWeight")), factWt = dbl(body.get("factWeight"));
        if (suppWt == 0) throw new IllegalArgumentException("Supplier Weight Field is Required");
        if (factWt == 0) throw new IllegalArgumentException("Factory Weight Field is Required");
        String status = text(body.get("status"));
        if (status.isEmpty()) throw new IllegalArgumentException("Status Field is Required");
        List<Map<String, Object>> rows = asRows(body.get("details"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");

        // the web has to keep each id inside the signed-in company (the desktop combos only list these rows)
        if (!ids(grn.branches(org, company), "Id").contains(branchId)) throw new IllegalArgumentException("Branch Field is Required");
        if (!ids(grn.projects(org, company), "Id").contains(projectId)) throw new IllegalArgumentException("Project Field is Required");
        if (!ids(paddy.suppliersByGroupIds(org, company, "1,2,3", 0), "Id").contains(supplierId)) throw new IllegalArgumentException("Supplier Field is Required");
        int gpId = num(body.get("gpId"));
        if (gpId > 0 && jdbc.queryForObject("SELECT COUNT(*) FROM dbo.GatePassInward WHERE Id=? AND OrganizationId=? AND CompanyId=? AND DocumentTypeId=?", Integer.class, gpId, org, company, GATE_PASS_DOCUMENT_TYPE_ID) != 1)
            throw new IllegalArgumentException("The gate pass is outside the current company");

        Map<String, Object> header = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
        Date now = new Date();
        header.put("Id", updating ? id : 0);
        header.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        header.put("DocDate", text(body.get("docDate")));
        header.put("DocNo", docNo);
        header.put("SupplierCustomerId", supplierId);
        header.put("GpNo", gpNo);
        header.put("PartyWeight", suppWt);
        header.put("FactoryWeight", factWt);
        header.put("ScaleKart", dbl(body.get("scaleKart")));
        header.put("VehicleType", text(body.get("vehicleType")));
        header.put("VehicleNo", text(body.get("vehicleNo")));
        header.put("DeliveryTerm", "Ponch & FactoryWeight");
        header.put("BiltyNo", text(body.get("biltyNo")));
        header.put("Status", status);
        double carriage = dbl(body.get("carriage"));
        if (carriage > 0) {
            int transporterId = num(body.get("transporterId"));
            if (transporterId <= 0) throw new IllegalArgumentException("Transporter Account field required");
            header.put("TransporterId", transporterId);
            header.put("CarriageAmount", carriage);
        }
        header.put("RemarksHeader", text(body.get("remarks")));
        header.put("BranchesId", branchId);
        header.put("ProjectsId", projectId);
        header.put("InwardGatePassId", gpId);
        header.put("OrganizationId", org);
        header.put("CompanyId", company);
        header.put("FinancialYearId", year);
        java.sql.Timestamp stamp = new java.sql.Timestamp(now.getTime());
        header.put("EntryDate", stamp); header.put("ModifyDate", stamp);
        header.put("EntryUser", user); header.put("ModifyUser", user);
        header.put("ActionId", updating ? 2 : 1);                      // BLL InvGrn.Save :60-70
        header.put("AttachmentsValues", updating ? text(pick(grn.grnHeader(id), "AttachmentsValues")) : "");
        header.put("CustomAttachmentsValues", updating ? text(pick(grn.grnHeader(id), "CustomAttachmentsValues")) : "");

        // details loop (:340)
        List<Map<String, Object>> details = new ArrayList<>();
        double grnQty = 0, grossWeight = 0;
        for (Map<String, Object> r : rows) {
            Map<String, Object> d = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
            d.put("ContractorId", num(r.get("ContractorId")));
            d.put("ItemId", num(r.get("ItemId")));
            d.put("CropYear", text(r.get("CropYear")));
            d.put("JobLotId", num(r.get("JobId")));
            d.put("PackingTypeId", num(r.get("PackingTypeId")));
            d.put("ItemQty", dbl(r.get("Qty")));
            grnQty += dbl(r.get("Qty"));
            d.put("ItemUomId", num(r.get("UOMId")));
            d.put("GrossWeight", dbl(r.get("GrossWeight")));
            grossWeight += dbl(r.get("GrossWeight"));
            d.put("ScaleKart", dbl(r.get("ScaleKart")));
            d.put("EBWPerUnit", dbl(r.get("EbUnit")));
            d.put("EBWTotal", dbl(r.get("EbTotal")));
            d.put("NetBillWeight", dbl(r.get("NetWeight")));
            d.put("StockWeight", dbl(r.get("NetWeight")));
            d.put("WarehouseId", num(r.get("WareHouse")));
            d.put("LabReportRef", text(r.get("LabNo")));
            d.put("AreaCity", text(r.get("City")));
            details.add(d);
        }
        if (grossWeight != factWt) throw new IllegalArgumentException("GrossWeight and Factory Weight Not Match");

        // empty bags (:384): totals run over every filled row, the type switch runs per filled row
        List<Map<String, Object>> bagRows = asRows(body.get("emptyBags"));
        if (bagRows.isEmpty()) bagRows = List.of(new HashMap<String, Object>());   // grdEmptyBags always holds at least one (blank) row, so RowCount > 0 on the desktop
        List<Map<String, Object>> bags = new ArrayList<>();
        {
            double recQty = 0, purQty = 0;
            for (Map<String, Object> b : bagRows) {
                if (num(b.get("Type")) > 0 && num(b.get("ItemId")) > 0) {
                    if (num(b.get("Condition")) == 0) throw new IllegalArgumentException("Please Select Bags_Condition First");
                    recQty += dbl(b.get("RecQty"));
                    purQty += dbl(b.get("PurQty"));
                }
            }
            if (grnQty != recQty + purQty) throw new IllegalArgumentException("Empty Bags Quantity must be equal to GrnQty");
            int ebStatus = 0;
            for (Map<String, Object> b : bagRows) {
                if (num(b.get("ItemId")) <= 0 || num(b.get("Type")) <= 0) continue;
                Map<String, Object> o = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
                o.put("Id", 0); o.put("ItemUomId", 0); o.put("WarehouseId", 0); o.put("RackId", 0);
                o.put("PurchaseOrderId", num(b.get("OrderId")));
                o.put("TypeId", num(b.get("Type")));
                o.put("ItemId", num(b.get("ItemId")));
                o.put("BagsCondition", num(b.get("Condition")));
                o.put("ReceivedQty", dbl(b.get("RecQty")));
                o.put("PurchaseQty", dbl(b.get("PurQty")));
                o.put("Remarks", text(b.get("Remarks")));
                ebStatus = num(o.get("TypeId"));
                if (dbl(o.get("ReceivedQty")) > 0 || dbl(o.get("PurchaseQty")) > 0) bags.add(o);
                switch (ebStatus) {                                         // the desktop runs the switch inside the row loop (frmGrnForGate :490)
                    case 1: if (recQty == 0 && purQty == 0) throw new IllegalArgumentException("Received Qty Or Purchase Qty Required in EmptyBags grid"); break;
                    case 2: case 3: if (purQty == 0) throw new IllegalArgumentException("Purchase Qty Required in EmptyBags grid"); break;
                    case 4: case 5: if (recQty == 0) throw new IllegalArgumentException("Received Qty Required in EmptyBags grid"); break;
                    default: break;
                }
                if (ebStatus > 0 && grnQty != recQty + purQty) throw new IllegalArgumentException("Empty Bags Quantity must be equal to GrnQty");
            }
        }

        // DAL InvGrn.SetData: header, every detail row, every empty-bag row, then the inventory / stock-in-transit procedures
        Map<String, Object> values = writes.headerValues(null, header);
        int savedId = writes.saveHeader(values, updating);
        int line = 1;
        for (Map<String, Object> d : details) writes.saveDetail(d, null, savedId, line++);
        for (Map<String, Object> b : bags) writes.saveEmptyBag(b, savedId);
        writes.validateAndPost(org, company, DOCUMENT_TYPE_ID, savedId, gpId, user);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", savedId);
        out.put("docNo", docNo);
        out.put("message", (updating ? "Record Update Successfully [" : "Record Save Successfully [") + docNo + "]");
        return out;
    }

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> asRows(Object v) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (v instanceof List) for (Object o : (List<Object>) v) if (o instanceof Map) out.add((Map<String, Object>) o);
        return out;
    }
    private static Set<Integer> ids(List<Map<String, Object>> rows, String key) {
        Set<Integer> s = new HashSet<>();
        for (Map<String, Object> r : rows) s.add(num(pick(r, key)));
        return s;
    }

    /** GrdHistory_LinkClicked (:1999): CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, 165). */
    public List<Map<String, Object>> attachments(int id) {
        requireRecord(id);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> row : grn.attachmentsByRefDocumentType(id, DOCUMENT_TYPE_ID)) {
            if (pick(row, "OrganizationId") != null && num(pick(row, "OrganizationId")) != org()) continue;
            if (pick(row, "CompanyId") != null && num(pick(row, "CompanyId")) != company()) continue;
            Map<String, Object> item = new LinkedHashMap<>();
            item.put("Id", num(pick(row, "Id"))); item.put("Attachment", text(pick(row, "Attachment"))); item.put("UploadedFileCustomName", text(pick(row, "UploadedFileCustomName")));
            item.put("EntryDate", pick(row, "EntryDate"));
            out.add(item);
        }
        return out;
    }

    public record AttachmentFile(String name, byte[] bytes) {}
    public AttachmentFile attachmentFile(int id, int attachmentId) {
        for (Map<String, Object> row : attachments(id)) {
            if (num(row.get("Id")) != attachmentId) continue;
            String original = baseName(text(row.get("Attachment"))), custom = text(row.get("UploadedFileCustomName"));
            String stored = custom.isBlank() ? original : baseName(custom);
            return new AttachmentFile(original.isBlank() ? stored : original, attachmentStore.read(context.requireAccountingUser(), stored));
        }
        throw new IllegalArgumentException("Attachment not found");
    }
    private static String baseName(String value) {
        String name = value == null ? "" : value.replace('\\', '/'); name = name.substring(name.lastIndexOf('/') + 1);
        if (!name.isEmpty()) DesktopAttachmentStore.validateName(name);
        return name;
    }
}
