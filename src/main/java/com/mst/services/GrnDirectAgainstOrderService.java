package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.GrnDirectAgainstOrderRepository;
import com.mst.repositories.PurchaseGrnWriteRepository;
import com.mst.repositories.partyprocessing.PpBLookupRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.*;

import static com.mst.repositories.GrnDirectAgainstOrderRepository.*;

/**
 * BLL of Architecture.WinApp.Purchase.GRNDirectAgainstOrder - "GRN Direct Against Order", ScreenDefinition 133
 * (ScreenName GRNDirectAgainstOrder, Purchase module 5), DocumentTypeId 169 (GRNDirectAgainstOrder.cs :391, :945, :1755).
 *
 * Load    InvFrmGRN_Load :1582 - rights (SetRightsValueInRightsObject(base.Name)), GetERPFeatureById(4), EmptyBagsTypeFill, BranchFill,
 *         ProjectFill, GenerateCode, SupplierNameFilll, CropYearfill, TransportFill, VehicleTypesFill, DeliveryTerm, WareHouseFill,
 *         JobLotFill, PackingTypeFill, CityFill, ItemNameFill. GetConfiguration() :884 and defaultConfiquration() :843 exist in the
 *         form but are never called, so no configuration is read.
 * Order   BtnLoadOrder_Click :2499 -> LoadPurchaseOrder (DocumentTypeId 41) -> LoadInGridDetail :2515 (PurchaseOrderLoadForPurchaseInvoice
 *         + GetPurchaseOrderEmptyBagsDetailByOrderId of the first picked order); cmbOrderNo_Leave :2680 (SupplierByPurchaseOrderNo).
 * Save    Insert() :359 - FormValidation :701, "Grid Record Not Found", confirm, the delivery-term weight checks, the empty bags checks;
 *         InvGrn.Save (BLL 0576:60: insert ActionId 1 / update ActionId 2) -> DAL 0429 SetData, one transaction: Sp_InvGrn_Insert /
 *         Sp_InvGrn_Update -> Sp_InvGrnDetail_Insert per row (LineId 1..n) -> Sp_InvGrnDetailEmptyBags_Insert ->
 *         Sp_InventoryTransactions_GetALLMethod -> usp_StockInTransitUpdate_StockEvaluationAndVoucherInsertFromGrn (the 46-only steps
 *         are not run for 169). Date lock: FunCheckValidFinancialYear inside both header procedures.
 * Read    ReadById :637 - InvGrn.GetByID ('ReadByID' + 'ReadByInvGrnID' + 'ReadByInvGrnIdEmptyBagsDetail' for 169).
 * History HistoryGridFill :1743 - InvGrn.GetHisoty 'GRNFormHistory' (NoOfRecords 50 on the tab change, all on LoadAll).
 * Delete  none: btnDelete is Visible = false and has no Click handler (Designer :5313-5316).
 */
@Service
public class GrnDirectAgainstOrderService {

    private final GrnDirectAgainstOrderRepository repo;
    private final PpBLookupRepository lookups;
    private final PurchaseGrnWriteRepository writes;
    private final CurrentUserContext context;

    public GrnDirectAgainstOrderService(GrnDirectAgainstOrderRepository repo, PpBLookupRepository lookups, PurchaseGrnWriteRepository writes,
                                        CurrentUserContext context) {
        this.repo = repo; this.lookups = lookups; this.writes = writes; this.context = context;
    }

    private UserAccount u() { return context.requireAccountingUser(); }

    /** The page itself: View on screen 133 (the hub tile's right). */
    public void requireView() { repo.requireRight("View"); }

    // ================================================================== load / refresh

    public Map<String, Object> setup() {
        repo.requireRight("View");
        UserAccount u = u();
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> rights = new LinkedHashMap<>();                                    // InvFrmGRN_Load :1588
        rights.put("save", repo.hasRight("Save"));
        rights.put("update", repo.hasRight("Update"));
        rights.put("print", repo.hasRight("Print"));
        rights.put("viewAll", repo.hasRight("CanView AllRecord"));
        rights.put("grnHistoryView", repo.hasRight("frmGRNHistory", "View"));                   // btnGrnFormHistory_Click :2316
        out.put("rights", rights);
        out.put("branches", pick(repo.branches(), "Id", "BranchName"));                        // BranchFill :956
        out.put("projects", pick(repo.projects(), "Id", "ProjectName"));                       // ProjectFill :973
        out.put("docNo", repo.nextDocNo());                                                    // GenerateCode :931
        out.put("cropYears", pick(lookups.cropYears(u), "Id", "CropYear"));                    // CropYearfill :1193
        out.put("deliveryTerms", pick(lookups.staticColumns("DeliveryTerm"), "Id", "Type"));   // DeliveryTerm() :1568
        out.put("emptyBagTypes", pick(lookups.staticColumns("PurchaseOrderEmptyBagsType"), "Id", "Type"));   // EmptyBagsTypeFill :1730
        out.put("bagsConditions", pick(lookups.staticColumns("EmptyBagsCondition"), "Id", "Type"));
        out.put("emptyBagItems", pick(lookups.itemsByItemType(u, "14"), "Id", "ItemName"));   // EmptyBagsGridRefresh :1668
        out.putAll(lists(u, 0));
        return out;
    }

    /** toolStripButton1_Click (Refresh) :2210 - Supplier, Transport, VehicleTypes, PackingType, City, ItemName (of the current order no),
     *  WareHouse, JobLot. */
    public Map<String, Object> refresh(int orderId) {
        repo.requireRight("View");
        return lists(u(), orderId);
    }

    private Map<String, Object> lists(UserAccount u, int orderId) {
        Map<String, Object> out = new LinkedHashMap<>();
        boolean subsidiary = lookups.feature(u, 4);                                             // InvFrmGRN_Load :1598
        out.put("suppliers", suppliers(u, subsidiary));
        out.put("transporters", transporters(u));
        out.put("vehicleTypes", pick(lookups.vehicleTypes(), "Id", "VehicleDescription"));    // VehicleTypesFill :1048
        List<Map<String, Object>> packing = new ArrayList<>();                                  // PackingTypeFill :1094 - ids 1 and 2 only
        for (Map<String, Object> r : lookups.packingTypes()) { int id = toInt(col(r, "Id")); if (id == 1 || id == 2) packing.add(r); }
        out.put("packingTypes", pick(packing, "Id", "PackTypeDesc"));
        out.put("cities", pick(lookups.cities(u), "Id", "CityName"));                          // CityFill :1151 (City.GetAll)
        out.put("items", orderItemRows(orderId));                                               // ItemNameFill :1064
        out.put("warehouses", pick(lookups.activeWarehouses(u), "Id", "WareHouseName"));       // WareHouseFill :1209
        out.put("jobLots", pick(lookups.jobLotsAll(u), "Id", "JobLotDescription"));            // JobLotFill :1177
        return out;
    }

    /** SupplierNameFilll :990 - feature 4 -> GetVendorsAndCustomers(1), else SupplierCustomerGetforComboServiceBind; Id / CompanyName. */
    private List<Map<String, Object>> suppliers(UserAccount u, boolean subsidiary) {
        return pick(subsidiary ? lookups.vendorsAndCustomers(u, 1) : lookups.supplierCustomerCombo(u), "Id", "CompanyName");
    }

    /** TransportFill :1018 - CoaAllocationGetAllServiceBind without AccountTypeId 2, 4, 10, 11, 12, 15 (no feature-4 branch on this form). */
    private List<Map<String, Object>> transporters(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : lookups.coaAllocations(u)) {
            int t = toInt(col(r, "AccountTypeId"));
            if (t != 2 && t != 4 && t != 10 && t != 11 && t != 12 && t != 15) out.add(row("Id", col(r, "Id"), "AccountTitle", col(r, "AccountTitle")));
        }
        return out;
    }

    /** ItemNameFill :1064 - OrderItemId / ItemName / PODetailId (= PurchaseOrderDetail.Id) and the row's CityArea. */
    private List<Map<String, Object>> orderItemRows(int orderId) {
        return pick(repo.orderItems(orderId), "OrderItemId", "ItemName", "PODetailId=Id", "CityArea");
    }

    public List<Map<String, Object>> orderItems(int orderId) { repo.requireRight("View"); return orderItemRows(orderId); }

    public Map<String, Object> code() { repo.requireRight("View"); return row("docNo", repo.nextDocNo()); }

    /** PackUOMFillWithoutOrder :1123 - UOMSchedule.SearchByObject(ItemId) -> Id / Equivalent (the combo shows Equivalent). */
    public List<Map<String, Object>> uoms(int itemId) {
        repo.requireRight("View");
        return pick(lookups.uoms(u(), itemId), "Id", "UOMCode", "Equivalent");
    }

    // ================================================================== purchase order

    /** cmbOrderNo_Leave :2680 - the order's DeliveryTerm and supplier CompanyName; found=false is "Supplier Name Not Found". */
    public Map<String, Object> orderLeave(int orderNo) {
        repo.requireRight("View");
        List<Map<String, Object>> r = repo.supplierByOrderNo(orderNo);
        if (r.isEmpty()) return row("found", false);
        return row("found", true, "DeliveryTerm", Objects.toString(col(r.get(0), "DeliveryTerm"), ""),
                "CompanyName", Objects.toString(col(r.get(0), "CompanyName"), ""));
    }

    /** The LoadPurchaseOrder list (see GrnDirectAgainstOrderRepository.pendingOrders - the dialog's source is not in the corpus). */
    public List<Map<String, Object>> pendingOrders(int supplierId, String fromDate, String toDate, String fromDocNo, String toDocNo) {
        repo.requireRight("View");
        return pick(repo.pendingOrders(supplierId, day(fromDate), day(toDate), toInt(fromDocNo), toInt(toDocNo)),
                "Id", "DocNo", "DocDate", "SupplierCustomerId", "SupplierName", "ItemName", "ItemCode", "ItemQty", "ItemWeight", "PackUom",
                "UOMCode", "OrderItemRate", "DeliveryTerm", "JobLotDescription", "Crop", "PODetailId");
    }

    /**
     * LoadInGridDetail :2515 - POIds = "," + id for every picked row (dtSupply.Rows[i][0]); PurchaseOrderId = the first picked row's id.
     * LoadPurchaseOrderDataForGrn :2586 - the dtdetail rows (PackingTypeId / PackingType 0, WarehouseId / Warehouse 0, AddLesswt 0,
     * WtCut 0, NetWeight = StockWeight = GrossWeight - EmptyBagsTotal) and the header's supplier / remarks / delivery term from row 0.
     * GetEmptyBagsInformationFromOrder(PurchaseOrderId) :2546 - that order's PurchaseOrderEmptyBags rows.
     */
    public Map<String, Object> loadOrders(List<Integer> ids) {
        repo.requireRight("View");
        if (ids == null || ids.isEmpty()) return row("rows", List.of(), "emptyBags", List.of());
        StringBuilder poIds = new StringBuilder();
        for (Integer id : ids) poIds.append(',').append(id == null ? 0 : id);
        int firstOrderId = ids.get(0) == null ? 0 : ids.get(0);
        List<Map<String, Object>> src = repo.orderLoad(poIds.toString());
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : src) {
            double gross = dbl(col(d, "GrossWeight")), ebTotal = dbl(col(d, "EmptyBagsTotal"));
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("OrderDetailId", col(d, "PODetailId"));
            x.put("OrderId", col(d, "Id"));
            x.put("OrderNo", col(d, "OrderNo"));
            x.put("ItemId", col(d, "ItemId"));
            x.put("Item", col(d, "ItemName"));
            x.put("PackingTypeId", 0);
            x.put("PackingType", "0");
            x.put("UOM", col(d, "UOMCodeItem"));                                               // the pack UOM's Equivalent
            x.put("UOMId", col(d, "OrderItemUOMId"));
            x.put("CropYear", col(d, "Crop"));
            x.put("JobLotId", col(d, "JobLotId"));
            x.put("JobLot", col(d, "JobLotDescription"));
            x.put("Qty", dbl(col(d, "ItemQty")));
            x.put("GrossWight", gross);
            x.put("AddLesswt", 0d);
            x.put("EBUnit", dbl(col(d, "EmptyBags")));
            x.put("EBTotal", ebTotal);
            x.put("WtCutUnit", 0d);
            x.put("WtCutTotal", 0d);
            x.put("NetWeight", gross - ebTotal);
            x.put("StockWeight", gross - ebTotal);
            x.put("City", col(d, "CityArea"));
            x.put("WarehouseId", 0);
            x.put("Warehouse", "0");
            x.put("RemarksDetail", col(d, "OrderRemarks"));
            rows.add(x);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows);
        if (!src.isEmpty()) {
            out.put("SupplierCustomerId", col(src.get(0), "SupplierCustomerId"));
            out.put("RemarksHeader", Objects.toString(col(src.get(0), "RemarksHeader"), ""));
            out.put("DeliveryTerm", Objects.toString(col(src.get(0), "DeliveryTerm"), ""));
        }
        out.put("emptyBags", pick(repo.orderEmptyBags(firstOrderId), "PurchaseOrderId", "Type", "ItemId", "PackingTypeId", "WeightCut"));
        return out;
    }

    // ================================================================== history

    /** HistoryGridFill(NoOfRecords) :1743 -> InvGrn.GetHisoty: org, company, DocumentTypeId 169, CanViewAllRecord always; FinancialYearId,
     *  NoOfRecords when != 0; EntryUser when not CanViewAllRecord. No branch, date, doc-no or supplier filter is set on this form. */
    public List<Map<String, Object>> history(int noOfRecords) {
        repo.requireRight("View");
        boolean all = repo.hasRight("CanView AllRecord");
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", repo.org());
        p.put("CompanyId", repo.company());
        p.put("DocumentTypeId", DOC);
        p.put("CanViewAllRecord", all);
        p.put("FinancialYearId", nz0(repo.year()));
        if (!all) p.put("EntryUser", repo.user());
        p.put("NoOfRecords", nz0(noOfRecords));
        p.put("Activity", "GRNFormHistory");
        return repo.history(p);
    }

    // ================================================================== read

    /** The stored 169 GRN of this organization / company / financial year that the user may see (own records unless CanView AllRecord). */
    private Map<String, Object> require(int id) {
        Map<String, Object> h = id > 0 ? repo.header(id) : null;
        if (h == null || toInt(col(h, "DocumentTypeId")) != DOC || toInt(col(h, "OrganizationId")) != repo.org()
                || toInt(col(h, "CompanyId")) != repo.company() || toInt(col(h, "FinancialYearId")) != repo.year())
            throw new IllegalArgumentException("Record Not Found");
        if (toInt(col(h, "EntryUser")) != repo.user() && !repo.hasRight("CanView AllRecord")) throw new IllegalArgumentException("Record Not Found");
        return h;
    }

    /** ReadById :637 - the header fields the form reads and the dtdetail / dtEmptyBag rows in the form's column order. */
    public Map<String, Object> byId(int id) {
        repo.requireRight("View");
        Map<String, Object> h = require(id);
        Map<String, Object> out = pick(List.of(h), "Id", "BranchesId", "ProjectsId", "DocNo", "DocDate", "SupplierCustomerId", "TransporterId",
                "CarriageAmount", "PartyWeight", "FactoryWeight", "VehicleType", "VehicleNo", "BiltyNo", "RemarksHeader", "DeliveryTerm").get(0);
        List<Map<String, Object>> det = new ArrayList<>();
        for (Map<String, Object> d : repo.details(id)) {                                        // ReadById :673
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("OrderDetailId", col(d, "PurchaseOrderDetailId"));
            x.put("OrderId", col(d, "PurchaseOrderId"));
            x.put("OrderNo", col(d, "PurchaserOrderNo"));
            x.put("ItemId", col(d, "ItemId"));
            x.put("Item", col(d, "Item"));
            x.put("PackingTypeId", col(d, "PackingTypeId"));
            x.put("PackingType", col(d, "PackingType"));
            x.put("UOM", col(d, "UOM"));                                                       // UOM.Equivalent (model InvGrnDetail.UOM is a double)
            x.put("UOMId", col(d, "ItemUomId"));
            x.put("CropYear", col(d, "CropYear"));
            x.put("JobLotId", col(d, "JobLotId"));
            x.put("JobLot", col(d, "JobLot"));
            x.put("Qty", col(d, "ItemQty"));
            x.put("GrossWight", col(d, "GrossWeight"));
            x.put("AddLesswt", col(d, "AdLsWeight"));
            x.put("EBUnit", col(d, "EBWPerUnit"));
            x.put("EBTotal", col(d, "EBWTotal"));
            x.put("WtCutUnit", col(d, "WtCut"));
            x.put("WtCutTotal", col(d, "WtCutTotal"));
            x.put("NetWeight", col(d, "NetBillWeight"));
            x.put("StockWeight", col(d, "StockWeight"));
            x.put("City", col(d, "AreaCity"));
            x.put("WarehouseId", col(d, "WarehouseId"));
            x.put("Warehouse", col(d, "WareHouseCode"));                                       // WareHouseCode, as the form reads it
            x.put("RemarksDetail", col(d, "CommentsDetail"));
            det.add(x);
        }
        out.put("details", det);
        out.put("emptyBags", pick(repo.emptyBags(id), "OrderId=PurchaseOrderId", "Type=TypeId", "ItemId", "Condition=BagsCondition",
                "RecQty=ReceivedQty", "PurQty=PurchaseQty", "Remarks"));
        return out;
    }

    // ================================================================== save

    @Transactional(rollbackFor = Exception.class)
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = u();
        int recId = Math.max(0, toInt(b.get("id")));
        repo.requireRight(recId > 0 ? "Update" : "Save");                                      // btnSave / btnUpdate .Enabled (:1589, :1591)
        Map<String, Object> stored = recId > 0 ? require(recId) : null;

        // FormValidation() :701
        String docNoText = trim(b.get("docNo"));
        if (docNoText.isEmpty() || "0".equals(docNoText)) throw invalid("DocNo Field is Required");
        int branchId = toInt(b.get("branchId"));
        if (branchId == 0 || !has(repo.branches(), branchId)) throw invalid("Branch Field is Required");
        int projectId = toInt(b.get("projectId"));
        if (projectId == 0 || !has(repo.projects(), projectId)) throw invalid("Project Field is Required");
        int supplierId = toInt(b.get("supplierId"));
        if (supplierId == 0 || !has(suppliers(u, lookups.feature(u, 4)), supplierId)) throw invalid("Supplier Field is Required");
        String term = trim(b.get("deliveryTerm"));
        if (term.isEmpty() || "0".equals(term)) throw invalid("DeliveryTerm Field is Required");
        int vehicleTypeId = toInt(b.get("vehicleTypeId"));
        Map<String, Object> vehicle = null;
        for (Map<String, Object> r : lookups.vehicleTypes()) if (toInt(col(r, "Id")) == vehicleTypeId) vehicle = r;
        if (vehicleTypeId == 0 || vehicle == null) throw invalid("Vehicle Type Field is Required");
        String vehicleNo = trim(b.get("vehicleNo"));
        if (vehicleNo.isEmpty() || "0".equals(vehicleNo)) throw invalid("Vehicle No Field is Required");
        String biltyNo = trim(b.get("biltyNo"));
        if (biltyNo.isEmpty() || "0".equals(biltyNo)) throw invalid("Bilty No Field is Required");
        double supplierWeightBox = dbl(trim(b.get("supplierWeight")));
        if (supplierWeightBox == 0.0) throw invalid("Supplier Weight Field is Required");
        double factoryWeightBox = dbl(trim(b.get("factoryWeight")));
        if (factoryWeightBox == 0.0) throw invalid("Factory Weight Field is Required");
        List<Map<String, Object>> rows = list(b.get("rows"));
        if (rows.isEmpty()) throw invalid("Grid Record Not Found");

        // GRN.TransporterId = ToInt(CmbTransport.Value) :401 - a text that is not a list row has no Value (0).
        int transporterId = toInt(b.get("transporterId"));
        if (transporterId != 0 && !has(transporters(u), transporterId)) transporterId = 0;

        Timestamp now = new Timestamp(System.currentTimeMillis());
        Timestamp docDate = day(b.get("docDate"));
        if (docDate == null) docDate = Timestamp.valueOf(LocalDate.now().atStartOfDay());
        int docNo = recId > 0 ? toInt(col(stored, "DocNo")) : toInt(docNoText);
        Map<String, Object> header = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);             // Insert() :391-413
        header.put("Id", recId);
        header.put("DocumentTypeId", DOC);
        header.put("DocDate", docDate);
        header.put("DocNo", docNo);
        header.put("SupplierCustomerId", supplierId);
        header.put("PartyWeight", supplierWeightBox);
        header.put("FactoryWeight", factoryWeightBox);
        header.put("VehicleType", Objects.toString(col(vehicle, "VehicleDescription"), ""));     // combvehtyp.Text
        header.put("VehicleNo", vehicleNo);
        header.put("DeliveryTerm", term);
        header.put("BiltyNo", biltyNo);
        header.put("TransporterId", transporterId);
        header.put("CarriageAmount", dbl(trim(b.get("carriageAmount"))));
        header.put("RemarksHeader", trim(b.get("remarks")));
        header.put("BranchesId", branchId);                                                     // combbrnch.Value
        header.put("ProjectsId", projectId);                                                    // combproject.Value
        header.put("OrganizationId", repo.org());
        header.put("CompanyId", repo.company());
        header.put("FinancialYearId", repo.year());
        header.put("EntryDate", now);
        header.put("ModifyDate", now);
        header.put("ScreenName", SCREEN_NAME);                                                  // base.Name
        header.put("EntryUser", repo.user());
        header.put("ModifyUser", repo.user());
        header.put("ActionId", recId > 0 ? 2 : 1);                                              // InvGrn.Save (BLL 0576:60)
        // DAL 0429: AttachmentsValues is only written from a non-empty attachments list - none is added on the web; an update keeps the
        // values ReadById loaded (the attachments the form loaded are its list).
        header.put("AttachmentsValues", recId > 0 ? col(stored, "AttachmentsValues") : null);
        header.put("CustomAttachmentsValues", recId > 0 ? col(stored, "CustomAttachmentsValues") : null);

        // Detail rows :422-449 (no per-row check on the desktop at save time)
        List<Map<String, Object>> details = new ArrayList<>();
        double grossWeight = 0.0, grnQty = 0.0;
        for (Map<String, Object> r : rows) {
            Map<String, Object> x = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
            x.put("PurchaseOrderId", toInt(r.get("OrderId")));
            x.put("PurchaseOrderDetailId", toInt(r.get("OrderDetailId")));
            x.put("ItemId", toInt(r.get("ItemId")));
            x.put("PackingTypeId", toInt(r.get("PackingTypeId")));
            x.put("ItemQty", dbl(r.get("Qty")));
            x.put("ItemUomId", toInt(r.get("UOMId")));
            x.put("CropYear", Objects.toString(r.get("CropYear"), ""));
            x.put("JobLotId", toInt(r.get("JobLotId")));
            x.put("WarehouseId", toInt(r.get("WarehouseId")));
            x.put("GrossWeight", dbl(r.get("GrossWight")));
            grossWeight += dbl(r.get("GrossWight"));
            x.put("AdLsWeight", dbl(r.get("AddLesswt")));
            grnQty += dbl(r.get("Qty"));
            x.put("EBWPerUnit", dbl(r.get("EBUnit")));
            x.put("EBWTotal", dbl(r.get("EBTotal")));
            x.put("WtCut", dbl(r.get("WtCutUnit")));
            x.put("WtCutTotal", dbl(r.get("WtCutTotal")));
            x.put("NetBillWeight", dbl(r.get("NetWeight")));
            x.put("StockWeight", dbl(r.get("StockWeight")));
            x.put("AreaCity", Objects.toString(r.get("City"), ""));
            x.put("CommentsDetail", Objects.toString(r.get("RemarksDetail"), ""));
            details.add(x);
        }

        // Weights against the delivery term :450-481
        if ("Load".equals(term) || "Load & PartyWeight".equals(term) || "Ponch & PartyWeight".equals(term)) {
            if (grossWeight != supplierWeightBox) throw invalid("GrossWeight and Supplier Weight Not Match");
        }
        if ("Load & FactoryWeight".equals(term) || "Ponch & FactoryWeight".equals(term)) {
            if (grossWeight != factoryWeightBox) throw invalid("GrossWeight and Factory Weight Not Match");
        }
        if ("Ponch".equals(term)) {
            if (supplierWeightBox > factoryWeightBox) { if (grossWeight != factoryWeightBox) throw invalid("GrossWeight and Factory Weight Not Match"); }
            else if (grossWeight != supplierWeightBox) throw invalid("GrossWeight and Supplier Weight Not Match");
        }

        // Empty bags :482-569 (EmptyBagsInformationComp stays false: GetConfiguration() is never called on this form)
        List<Map<String, Object>> bagRows = list(b.get("emptyBags"));
        List<Map<String, Object>> bags = new ArrayList<>();
        if (!bagRows.isEmpty()) {
            double recQty = 0.0, purQty = 0.0;
            for (Map<String, Object> e : bagRows) {
                if (toInt(e.get("Type")) > 0 && toInt(e.get("ItemId")) > 0) {
                    if (toInt(e.get("Condition")) == 0) throw invalid("Please Select Bags_Condition First");
                    recQty += dbl(e.get("RecQty"));
                    purQty += dbl(e.get("PurQty"));
                }
            }
            for (Map<String, Object> e : bagRows) {
                if (toInt(e.get("ItemId")) <= 0 || toInt(e.get("Type")) <= 0) continue;
                Map<String, Object> o = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
                o.put("PurchaseOrderId", toInt(e.get("OrderId")));
                o.put("TypeId", toInt(e.get("Type")));
                o.put("ItemId", toInt(e.get("ItemId")));
                o.put("BagsCondition", toInt(e.get("Condition")));
                o.put("ReceivedQty", dbl(e.get("RecQty")));
                o.put("PurchaseQty", dbl(e.get("PurQty")));
                o.put("Remarks", Objects.toString(e.get("Remarks"), ""));
                int status = toInt(e.get("Type"));
                if (dbl(e.get("RecQty")) > 0.0 || dbl(e.get("PurQty")) > 0.0) bags.add(o);
                switch (status) {
                    case 1: if (recQty == 0.0 && purQty == 0.0) throw invalid("Received Qty Or Purchase Qty Required in EmptyBags grid"); break;
                    case 2: case 3: if (purQty == 0.0) throw invalid("Purchase Qty Required in EmptyBags grid"); break;
                    case 4: case 5: if (recQty == 0.0) throw invalid("Received Qty Required in EmptyBags grid"); break;
                    default: break;
                }
                if (status > 0 && grnQty != recQty + purQty) throw invalid("Empty Bags Quantity must be equal to GrnQty");
            }
        }

        // InvGrn.Save (BLL 0576:60) - the form never sets a detail Id, so the insert guard always passes.
        // DAL 0429 SetData - one transaction (this method's).
        var values = writes.headerValues(null, header);
        int id = writes.saveHeader(values, recId > 0);
        int line = 1;
        for (Map<String, Object> x : details) writes.saveDetail(x, null, id, line++);
        for (Map<String, Object> o : bags) writes.saveEmptyBag(o, id);
        writes.validateAndPost(repo.org(), repo.company(), DOC, id, 0, repo.user());            // Sp_InventoryTransactions + usp_StockInTransit (169)

        Map<String, Object> res = new LinkedHashMap<>();
        res.put("success", true);
        res.put("id", id);
        res.put("docNo", docNo);
        res.put("message", (recId > 0 ? "Record Update Successfully [" : "Record Save Successfully [") + docNo + "]");
        return res;
    }

    // ================================================================== helpers

    private static IllegalArgumentException invalid(String m) { return new IllegalArgumentException(m); }
    private static String trim(Object v) { return v == null ? "" : v.toString().trim(); }
    private static double dbl(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).doubleValue();
        String s = v.toString().trim().replace(",", "");
        if (s.isEmpty()) return 0;
        try { return Double.parseDouble(s); } catch (NumberFormatException e) { return 0; }
    }
    private static Timestamp day(Object v) {
        String s = v == null ? "" : v.toString().trim();
        if (s.length() < 10 || !s.substring(0, 10).matches("\\d{4}-\\d{2}-\\d{2}")) return null;
        return Timestamp.valueOf(LocalDate.parse(s.substring(0, 10)).atStartOfDay());
    }
    private static boolean has(List<Map<String, Object>> rows, int id) {
        for (Map<String, Object> r : rows) if (toInt(col(r, "Id")) == id) return true;
        return false;
    }
    private static Map<String, Object> row(Object... kv) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (int i = 0; i + 1 < kv.length; i += 2) m.put(String.valueOf(kv[i]), kv[i + 1]);
        return m;
    }
    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> list(Object v) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (!(v instanceof List)) return out;
        for (Object o : (List<?>) v) if (o instanceof Map) out.add((Map<String, Object>) o);
        return out;
    }
    /** Rows re-shaped to the listed columns: "Target" or "Target=Source" (source read case-insensitively). */
    private static List<Map<String, Object>> pick(List<Map<String, Object>> rows, String... cols) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (String c : cols) {
                int eq = c.indexOf('=');
                m.put(eq < 0 ? c : c.substring(0, eq), col(r, eq < 0 ? c : c.substring(eq + 1)));
            }
            out.add(m);
        }
        return out;
    }
}
