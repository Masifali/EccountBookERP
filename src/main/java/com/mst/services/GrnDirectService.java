package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.GrnDirectRepository;
import com.mst.repositories.PurchaseGrnWriteRepository;
import com.mst.repositories.partyprocessing.PpBLookupRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.*;

import static com.mst.repositories.GrnDirectRepository.*;

/**
 * BLL of Architecture.WinApp.Purchase.InvFrmGRNDirect - "Goods Receiving Notes (without Gatepass)", ScreenDefinition 129
 * (frmGrnDirect), DocumentTypeId 137.
 *
 * Load   InvFrmGRN_Load :418 - GetConfiguration :566, rights :440, GetConfigurations :542, EmptyBagsTypeFill, GenerateCode,
 *        SupplierNameFilll, TransportFill, VehicleTypesFill, DeliveryTerm, WareHouseFill, JobLotFill, ItemNameFill,
 *        PackingTypeFill, CityFill, CropYear, HistoryComboFill, DefaultDaysToLessFromHistoryFromDate.
 * Save   Insert :1763 - FormValidation :1168, "Grid Record Not Found", the transporter check, the detail row checks, the
 *        delivery-term / deduction-policy weight checks, the empty bags checks; InvGrn.Save (BLL 0576:60: insert ActionId 1 and
 *        "Record cannot be inserted because detailId greater than zero", update ActionId 2) -> DAL 0429 SetData, one transaction:
 *        Sp_InvGrn_Insert / Sp_InvGrn_Update -> Sp_InvGrnDetail_Insert per row (LineId 1..n) -> Sp_InvGrnDetailEmptyBags_Insert ->
 *        Sp_InventoryTransactions_GetALLMethod -> usp_StockInTransitUpdate_StockEvaluationAndVoucherInsertFromGrn (the 46-only
 *        validations and special-approval log are not run for 137). Date lock: FunCheckValidFinancialYear inside the procedures.
 * Delete btnDelete_Click :2186 - InvPurchaseInvoice.RemoveByID -> Sp_InvoicesVouchersandStocksDelete (DocumentTypeId 137).
 * Read   ReadById :2111 / GrdHistory_SelectionChanged :3191 - InvGrn.GetByID ('ReadByID' + 'ReadByInvGrnIdDirect' + empty bags).
 * History HistoryGridFill :2911 - InvGrn.GetHisoty 'GRNFormHistory'.
 */
@Service
public class GrnDirectService {

    /** GetConfiguration :570 - the one multi-config read the form makes. */
    private static final String CONFIGS = "WeightCutForJuteBags,WeightCutForPPBags,WeightCutForOpenBulk,EmptyBagsWeightCutEditableOnGRN,"
            + "AddLessWeightCutEditableOnGRN,ContractorWagesCompulsoryBeforeInvoices,EmptyBagsInofrmationCompulsoryOnGRN,"
            + "LabCompulsoryForWeighBridgeAgainstGatePurchase,BillWeightAndStockWeightDifferenceTolerance";

    private final GrnDirectRepository repo;
    private final PpBLookupRepository lookups;
    private final PurchaseGrnWriteRepository writes;
    private final CurrentUserContext context;
    private final PurchaseDocAttachmentService attachments;

    public GrnDirectService(GrnDirectRepository repo, PpBLookupRepository lookups, PurchaseGrnWriteRepository writes, CurrentUserContext context,
                            PurchaseDocAttachmentService attachments) {
        this.repo = repo; this.lookups = lookups; this.writes = writes; this.context = context; this.attachments = attachments;
    }

    private UserAccount u() { return context.requireAccountingUser(); }

    /** The page itself: View on screen 129 (the menu tile's right). */
    public void requireView() { repo.requireRight("View"); }

    // ================================================================== load / refresh

    public Map<String, Object> setup() {
        repo.requireRight("View");
        UserAccount u = u();
        Map<String, Object> out = new LinkedHashMap<>(lists(u));
        Map<String, Object> rights = new LinkedHashMap<>();
        rights.put("save", repo.hasRight("Save"));
        rights.put("update", repo.hasRight("Update"));
        rights.put("delete", repo.hasRight("Delete"));
        rights.put("print", repo.hasRight("Print"));
        rights.put("viewAll", repo.hasRight("CanView AllRecord"));
        // btnGrnFormHistory_Click :3690 / GrdHistoryDetail_LinkClicked :3291 read the View right of these two screens.
        rights.put("grnHistoryView", repo.hasRight("frmGRNHistory", "View"));
        rights.put("purchaseOrderView", repo.hasRight("PurchsaeOrder", "View"));
        out.put("rights", rights);
        Map<String, String> cfg = repo.configs(CONFIGS);
        out.put("juteBagsCut", dbl(cfg.get("WeightCutForJuteBags")));
        out.put("ppBagsCut", dbl(cfg.get("WeightCutForPPBags")));
        out.put("openBulkCut", dbl(cfg.get("WeightCutForOpenBulk")));
        out.put("emptyBagsWeightCutEditable", flag(cfg.get("EmptyBagsWeightCutEditableOnGRN")));
        out.put("addLessWeightEditable", flag(cfg.get("AddLessWeightCutEditableOnGRN")));
        out.put("defaultDays", toInt(lookups.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        out.put("emptyBagTypes", pick(lookups.staticColumns("PurchaseOrderEmptyBagsType"), "Id", "Type"));
        out.put("bagsConditions", pick(lookups.staticColumns("EmptyBagsCondition"), "Id", "Type"));
        out.put("emptyBagItems", pick(repo.emptyBagItems(), "Id=ItemId", "ItemName"));
        out.put("docNo", repo.nextDocNo());
        out.put("historySuppliers", historySuppliers());
        return out;
    }

    /** BtnRefresh_Click :2323 - the combos and GetConfigurations(). */
    public Map<String, Object> refresh() {
        repo.requireRight("View");
        return lists(u());
    }

    private Map<String, Object> lists(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        boolean subsidiary = lookups.feature(u, 4);                                        // GetConfigurations :546
        out.put("subsidiary", subsidiary);
        out.put("deductionPolicyOn", flag(lookups.config(u, "DeductionPolicyForGrnIsOn")));
        out.put("termDifference", dbl(lookups.config(u, "BillWeightAndStockWeightDifferenceTolerance")));
        out.put("termOnlyLessWeight", flag(lookups.config(u, "GRNAgainstPartyWeightWithTolerance")));
        out.put("freightInwardAc", subsidiary ? 0 : toInt(lookups.config(u, "FreightInwardAc")));
        out.put("suppliers", suppliers(u, subsidiary));
        out.put("transporters", transporters(u, subsidiary));
        out.put("vehicleTypes", pick(lookups.vehicleTypes(), "Id", "VehicleDescription"));
        out.put("warehouses", pick(lookups.activeWarehouses(u), "Id", "WareHouseName"));
        out.put("jobLots", pick(lookups.jobLotsAll(u), "Id", "JobLotDescription"));
        out.put("items", pick(lookups.readAllItems(u), "Id", "ItemName", "ItemCode=ItemCodeNew"));
        List<Map<String, Object>> packing = new ArrayList<>();                            // PackingTypeFill :851 - ids 1 and 2 only
        for (Map<String, Object> r : lookups.packingTypes()) { int id = toInt(col(r, "Id")); if (id == 1 || id == 2) packing.add(r); }
        out.put("packingTypes", pick(packing, "Id", "PackTypeDesc"));
        out.put("cities", pick(lookups.cities(u), "Id", "CityName"));
        out.put("cropYears", pick(lookups.cropYears(u), "Id", "CropYear"));
        return out;
    }

    /** SupplierNameFilll :642 - feature 4 -> GetVendorsAndCustomers(1), else SupplierCustomerGetforComboServiceBind. */
    private List<Map<String, Object>> suppliers(UserAccount u, boolean subsidiary) {
        List<Map<String, Object>> src = subsidiary ? lookups.vendorsAndCustomers(u, 1) : lookups.supplierCustomerCombo(u);
        return pick(src, "Id", "SupplierName=CompanyName", "GLAccountId=GlAccountId");
    }

    /** TransportFill :700 - feature 4 -> GetVendorsAndCustomersForTransporter (Id = GLAccountId, SupplierCustomerId = Id), else
     *  CoaAllocationGetAllServiceBind without AccountTypeId 2, 4, 10, 11, 12, 15 (SupplierCustomerId 0). */
    private List<Map<String, Object>> transporters(UserAccount u, boolean subsidiary) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (subsidiary) {
            for (Map<String, Object> r : lookups.vendorsForTransporter(u))
                out.add(row("Id", col(r, "GLAccountId"), "AccountTitle", col(r, "CompanyName"), "SupplierCustomerId", col(r, "Id")));
        } else {
            for (Map<String, Object> r : lookups.coaAllocations(u)) {
                int t = toInt(col(r, "AccountTypeId"));
                if (t != 2 && t != 4 && t != 10 && t != 11 && t != 12 && t != 15)
                    out.add(row("Id", col(r, "Id"), "AccountTitle", col(r, "AccountTitle"), "SupplierCustomerId", 0));
            }
        }
        return out;
    }

    public Map<String, Object> code() { repo.requireRight("View"); return row("docNo", repo.nextDocNo()); }

    /** PackUOMFillWithoutOrder :885 - UOMSchedule.SearchByObject(ItemId) -> Id / UOMCode / Equivalent. */
    public List<Map<String, Object>> uoms(int itemId) {
        repo.requireRight("View");
        return pick(lookups.uoms(u(), itemId), "Id", "UOMCode", "Equivalent");
    }

    /** DeductionPolicyForGrn :2356 - the policy row for the positive factory - supplier difference (PolicyTypeId 0 when none). */
    public Map<String, Object> deductionPolicy(String date, double difference) {
        repo.requireRight("View");
        Map<String, Object> p = difference > 0 ? repo.deductionPolicy(day(date), difference) : null;
        return row("PolicyTypeId", p == null ? 0 : toInt(col(p, "PolicyTypeId")), "ConditionDescription", p == null ? "" : Objects.toString(col(p, "ConditionDescription"), ""));
    }

    // ================================================================== history

    /** HistoryComboFill :2843 - rows whose Activity is "Supplier". */
    public List<Map<String, Object>> historySuppliers() {
        repo.requireRight("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.historyDropDown())
            if ("Supplier".equals(Objects.toString(col(r, "Activity"), ""))) out.add(row("Id", col(r, "Id"), "name", col(r, "ReferenceName")));
        return out;
    }

    /** HistoryGridFill :2911 -> InvGrn.GetHisoty (BLL 0576:334): every "!= 0" / CheckDateTimeNull guard of the BLL is kept. */
    public List<Map<String, Object>> history(Map<String, Object> b) {
        repo.requireRight("View");
        boolean all = repo.hasRight("CanView AllRecord");
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", repo.org());
        p.put("CompanyId", repo.company());
        p.put("DocumentTypeId", DOC);
        p.put("CanViewAllRecord", all);
        p.put("FinancialYearId", nz0(repo.year()));
        p.put("BranchesId", nz0(repo.branch()));
        if (!all) p.put("EntryUser", repo.user());
        String kind = Objects.toString(b.get("dateType"), "doc");
        Timestamp f = flag(b.get("fromChecked")) ? day(b.get("fromDate")) : null;
        Timestamp t = flag(b.get("toChecked")) ? day(b.get("toDate")) : null;
        if ("entry".equals(kind)) { p.put("EntryFromDate", f); p.put("EntryToDate", t); }
        else if ("modify".equals(kind)) { p.put("ModifyFromDate", f); p.put("ModifyToDate", t); }
        else if ("approved".equals(kind)) { p.put("ApprovedFromDate", f); p.put("ApprovedToDate", t); }
        else { p.put("fromDate", f); p.put("toDate", t); }
        p.put("GrnNoF", nz0(toInt(b.get("fromDocNo"))));
        p.put("GrnNoT", nz0(toInt(b.get("toDocNo"))));
        p.put("SupplierCustomerId", nz0(toInt(b.get("supplierId"))));
        p.put("ActionId", nz0(toInt(b.get("actionId"))));                                // RadReffered 1 / RadNotReffered 2 / All omitted
        p.put("Activity", "GRNFormHistory");
        return pick(repo.history(p), "RecordNo", "Id", "InvoiceNo", "DocNo", "DocDate", "DocumentTypeId", "DeliveryTerm", "SupplierName",
                "GpNo", "VehicleNo", "BiltyNo", "WagesId", "WagesNo", "FactoryWeight", "SuppWeight=PartyWeight", "Transporter",
                "FrieghtAmount=CarriageAmount", "RemarksHeader", "EntryDate", "EntryUser", "ModifyDate", "ModifyUser", "ApprovedDate=PostDate",
                "ApprovedUser", "NoOfAttachments", "AttachmentsCount", "DetailIdsCount");
    }

    // ================================================================== read

    /** The stored 137 GRN of this company / branch / year that the user may see (own records unless CanView AllRecord). */
    private Map<String, Object> require(int id) {
        Map<String, Object> h = id > 0 ? repo.scoped(id) : null;
        if (h == null) throw new IllegalArgumentException("Record Not Found");
        if (toInt(col(h, "EntryUser")) != repo.user() && !repo.hasRight("CanView AllRecord")) throw new IllegalArgumentException("Record Not Found");
        return h;
    }

    // ================================================================== Load Order (BtnLoadOrder_Click :4027)

    /** The LoadPurchaseOrder dialog list (DocumentTypeId 41; GetPurchaseOrderForPurchaseInvoice). */
    public List<Map<String, Object>> orderLoader(int supplierId, String fromDate, String toDate, int fromDocNo, int toDocNo) {
        repo.requireRight("View");
        return repo.purchaseOrdersForLoader(supplierId, day(fromDate), day(toDate), fromDocNo, toDocNo);
    }

    /**
     * LoadInGridDetail :4055 - the picked orders' balance lines (PurchaseOrderLoadForPurchaseInvoice, @POIds ",id,id") and
     * GetEmptyBagsInformationFromOrder(first picked order) :4137. The empty-bag read filters by Id alone, so it is answered
     * only for an order the (organization / company scoped) line read returned.
     */
    public Map<String, Object> orderLines(String orderIds) {
        repo.requireRight("View");
        List<Integer> ids = new ArrayList<>();
        for (String part : Objects.toString(orderIds, "").split(",")) {
            String t = part.trim();
            if (t.matches("\\d{1,9}") && Integer.parseInt(t) > 0) ids.add(Integer.parseInt(t));
        }
        Map<String, Object> out = new LinkedHashMap<>();
        if (ids.isEmpty()) { out.put("lines", List.of()); out.put("emptyBags", List.of()); return out; }
        StringBuilder csv = new StringBuilder();
        for (Integer id : ids) csv.append(',').append(id);                                // POIds = POIds + "," + id
        List<Map<String, Object>> lines = repo.purchaseOrderLines(csv.toString());
        int first = ids.get(0);                                                            // LoadPO.dtSupply.Rows[0][0]
        boolean own = false;
        for (Map<String, Object> l : lines) if (toInt(col(l, "Id")) == first) own = true;
        out.put("lines", lines);
        out.put("emptyBags", own ? repo.purchaseOrderEmptyBags(first) : List.of());
        return out;
    }

    /** The attachment list / download of a record (PurchaseDocAttachmentController): View + the same scope as ReadById. */
    public void requireReadable(int id) {
        repo.requireRight("View");
        require(id);
    }

    public Map<String, Object> byId(int id) {
        repo.requireRight("View");
        require(id);
        Map<String, Object> h = repo.header(id);
        if (h == null) throw new IllegalArgumentException("Record Not Found");
        Map<String, Object> out = pick(List.of(h), "Id", "DocNo", "DocDate", "SupplierCustomerId", "DeliveryTerm", "TransporterId", "CarriageAmount",
                "RemarksHeader", "GpNo", "VehicleType", "VehicleNo", "BiltyNo", "PartyWeight", "FactoryWeight").get(0);
        List<Map<String, Object>> det = new ArrayList<>();
        for (Map<String, Object> d : repo.details(id)) {                                  // ReadById :2146 - the dtdetail row
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", col(d, "Id"));
            x.put("OrderId", col(d, "PurchaseOrderId"));
            x.put("OrderDetailId", col(d, "PurchaseOrderDetailId"));
            x.put("OrderNo", col(d, "PurchaserOrderNo"));
            x.put("WarehouseId", col(d, "WarehouseId"));
            x.put("Warehouse", col(d, "WareHouseCode"));                                  // WareHouseCode, as the desktop reads it
            x.put("ItemId", col(d, "ItemId"));
            x.put("ItemCode", col(d, "ItemCode"));
            x.put("ItemName", col(d, "Item"));
            x.put("CropYear", col(d, "CropYear"));
            x.put("JobLotId", col(d, "JobLotId"));
            x.put("JobLot", col(d, "JobLot"));
            x.put("PackingTypeId", col(d, "PackingTypeId"));
            x.put("PackingType", col(d, "PackingType"));
            x.put("UOMId", col(d, "ItemUomId"));
            x.put("UOM", col(d, "UOMCode"));
            x.put("UOMEquivalent", col(d, "UOM"));
            x.put("Qty", col(d, "ItemQty"));
            x.put("GrossWight", col(d, "GrossWeight"));
            x.put("EbUnit", col(d, "EBWPerUnit"));
            x.put("EbTotal", col(d, "EBWTotal"));
            x.put("AddLesswt", col(d, "AdLsWeight"));
            x.put("NetWeight", col(d, "NetBillWeight"));
            x.put("StockWeight", col(d, "StockWeight"));
            x.put("CityId", col(d, "CityId"));
            x.put("City", col(d, "AreaCity"));
            x.put("PurchaseOrder", col(d, "PurchaseOrder"));                              // history detail OrderNo (GrdHistory_SelectionChanged)
            x.put("UOMEquivalentText", col(d, "UOM"));
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
        repo.requireRight(recId > 0 ? "Update" : "Save");                                  // btnSave / btnUpdate .Enabled
        Map<String, Object> stored = recId > 0 ? require(recId) : null;

        // FormValidation() :1168
        String docNoText = trim(b.get("docNo"));
        if (docNoText.isEmpty() || "0".equals(docNoText)) throw invalid("DocNo Field is Required");
        boolean subsidiary = lookups.feature(u, 4);
        int supplierId = toInt(b.get("supplierId"));
        if (supplierId == 0 || !has(suppliers(u, subsidiary), supplierId)) throw invalid("Supplier Field is Required");
        String term = trim(b.get("deliveryTerm"));
        if (!"Load".equals(term) && !"Ponch".equals(term)) throw invalid("DeliveryTerm Field is Required");     // DeliveryTerm() :1095 - the only two rows
        String gpNo = trim(b.get("gpNo"));
        if (gpNo.isEmpty() || "0".equals(gpNo)) throw invalid("Gp No Field is Required");
        int vehicleTypeId = toInt(b.get("vehicleTypeId"));
        Map<String, Object> vehicle = null;
        for (Map<String, Object> r : lookups.vehicleTypes()) if (toInt(col(r, "Id")) == vehicleTypeId) vehicle = r;
        String vehicleText = vehicle == null ? "" : Objects.toString(col(vehicle, "VehicleDescription"), "").trim();
        if (vehicleTypeId == 0 || vehicle == null || vehicleText.isEmpty() || "0".equals(vehicleText)) throw invalid("Vehicle Type Field is Required");
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

        // Transporter :1795 - only read when the freight amount is positive.
        int transporterId = 0, transporterSupCustId = 0;
        double carriage = dbl(trim(b.get("carriageAmount")));
        if (carriage > 0.0) {
            int picked = toInt(b.get("transporterId"));
            Map<String, Object> t = null;
            for (Map<String, Object> r : transporters(u, subsidiary)) if (toInt(r.get("Id")) == picked) t = r;
            if (picked == 0 || t == null) throw invalid("Transporter Account field required");
            transporterId = picked;
            transporterSupCustId = subsidiary ? toInt(t.get("SupplierCustomerId")) : 0;
        } else {
            carriage = 0.0;
        }

        Timestamp now = new Timestamp(System.currentTimeMillis());
        Timestamp docDate = day(b.get("docDate"));
        if (docDate == null) docDate = Timestamp.valueOf(LocalDate.now().atStartOfDay());
        int docNo = recId > 0 ? toInt(col(stored, "DocNo")) : toInt(docNoText);
        Map<String, Object> header = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
        header.put("Id", recId);
        header.put("OrganizationId", repo.org());
        header.put("CompanyId", repo.company());
        header.put("BranchesId", repo.branch());
        header.put("ProjectsId", repo.branch());                                           // GRN.ProjectsId = UserAccount.BranchesId
        header.put("FinancialYearId", repo.year());
        header.put("DocumentTypeId", DOC);
        header.put("ScreenName", "InvFrmGRNDirect");
        header.put("EntryDate", now);
        header.put("ModifyDate", now);
        header.put("PostDate", now);
        header.put("EntryUser", repo.user());
        header.put("ModifyUser", repo.user());
        header.put("PostUser", repo.user());
        header.put("IsApproved", false);
        header.put("DocNo", docNo);
        header.put("DocDate", docDate);
        header.put("SupplierCustomerId", supplierId);
        header.put("DeliveryTerm", term);
        header.put("RemarksHeader", trim(b.get("remarks")));
        header.put("GpNo", toInt(gpNo));
        header.put("VehicleType", Objects.toString(col(vehicle, "VehicleDescription"), ""));
        header.put("VehicleNo", vehicleNo);
        header.put("BiltyNo", biltyNo);
        header.put("PartyWeight", supplierWeightBox);
        header.put("FactoryWeight", factoryWeightBox);
        header.put("TransporterId", transporterId);
        header.put("CarriageAmount", carriage);
        header.put("TransporterSupCustId", transporterSupCustId);
        header.put("InwardGatePassId", 0);
        header.put("ActionId", recId > 0 ? 2 : 1);                                         // InvGrn.Save (BLL 0576:60)
        // AttachmentsValues: "" on a new record, the values ReadById loaded on an update; when the Attachment form was
        // changed, the joined names of the kept + new files (DAL 0429 SetData :84-85).
        header.put("AttachmentsValues", recId > 0 ? col(stored, "AttachmentsValues") : "");
        header.put("CustomAttachmentsValues", recId > 0 ? col(stored, "CustomAttachmentsValues") : "");

        // Detail rows :1848
        List<Integer> ownDetailIds = recId > 0 ? repo.storedDetailIds(recId) : List.of();
        List<Map<String, Object>> details = new ArrayList<>();
        double grossWeight = 0.0, grnQty = 0.0;
        boolean otherThanOpenBulkExist = false;
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> r = rows.get(i);
            String rn = " in Detail in row No : " + (i + 1);
            if (toInt(r.get("WarehouseId")) == 0) throw invalid("Warehouse Required" + rn);
            if (toInt(r.get("ItemId")) == 0) throw invalid("Item Required" + rn);
            if (Objects.toString(r.get("CropYear"), "").isEmpty()) throw invalid("CropYear Required" + rn);
            if (toInt(r.get("JobLotId")) == 0) throw invalid("JobLot Required" + rn);
            if (toInt(r.get("PackingTypeId")) == 0) throw invalid("PackingType Required" + rn);
            if (toInt(r.get("UOMId")) == 0) throw invalid("UOM Required" + rn);
            if (dbl(r.get("Qty")) == 0.0) throw invalid("Qty Required" + rn);
            if (dbl(r.get("GrossWight")) == 0.0) throw invalid("GrossWight Required" + rn);
            if (dbl(r.get("NetWeight")) == 0.0) throw invalid("NetWeight Required" + rn);
            if (dbl(r.get("StockWeight")) == 0.0) throw invalid("StockWeight Required" + rn);
            Map<String, Object> x = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
            int detailId = toInt(r.get("Id"));
            if (recId > 0 && detailId > 0 && !ownDetailIds.contains(detailId)) detailId = 0;   // an id of another record is never kept
            x.put("Id", detailId);
            x.put("PurchaseOrderId", toInt(r.get("OrderId")));
            x.put("PurchaseOrderDetailId", toInt(r.get("OrderDetailId")));
            x.put("WarehouseId", toInt(r.get("WarehouseId")));
            x.put("ItemId", toInt(r.get("ItemId")));
            x.put("CropYear", Objects.toString(r.get("CropYear"), ""));
            x.put("JobLotId", toInt(r.get("JobLotId")));
            x.put("PackingTypeId", toInt(r.get("PackingTypeId")));
            x.put("ItemUomId", toInt(r.get("UOMId")));
            x.put("ItemQty", dbl(r.get("Qty")));
            if (toInt(r.get("PackingTypeId")) != 5) { otherThanOpenBulkExist = true; grnQty += dbl(r.get("Qty")); }
            x.put("GrossWeight", dbl(r.get("GrossWight")));
            grossWeight += dbl(r.get("GrossWight"));
            x.put("EBWPerUnit", dbl(r.get("EbUnit")));
            x.put("EBWTotal", dbl(r.get("EbTotal")));
            x.put("AdLsWeight", dbl(r.get("AddLesswt")));
            x.put("NetBillWeight", dbl(r.get("NetWeight")));
            x.put("StockWeight", dbl(r.get("StockWeight")));
            x.put("CityId", toInt(r.get("CityId")));
            x.put("AreaCity", Objects.toString(r.get("City"), ""));
            details.add(x);
        }

        // Weights against the delivery term :1917
        boolean deductionOn = flag(lookups.config(u, "DeductionPolicyForGrnIsOn"));
        if (deductionOn && "Ponch".equals(term)) {
            deductionPolicyCheck(grossWeight, supplierWeightBox, factoryWeightBox, docDate);
        } else {
            if ("Load".equals(term) || "Load & PartyWeight".equals(term) || "Ponch & PartyWeight".equals(term)) {
                if (grossWeight != supplierWeightBox) throw invalid("GrossWeight and Supplier Weight Must be Equal Due to Delivery term is " + term);
            }
            if ("Load & FactoryWeight".equals(term) || "Ponch & FactoryWeight".equals(term)) {
                if (grossWeight != factoryWeightBox)
                    throw invalid("GrossWeight must be equal to GRN Weight ( here FactoryWeight = " + net(factoryWeightBox) + "), due to Delivery Term " + term);
            }
            if ("Ponch".equals(term)) {
                String m = "GrossWeight must be equal to GRN Weight = which is lesser (Supplier Weight or Factory Weight), due to Delivery Term " + term;
                if (supplierWeightBox > factoryWeightBox) { if (grossWeight != factoryWeightBox) throw invalid(m); }
                else if (grossWeight != supplierWeightBox) throw invalid(m);
            }
        }

        // Empty bags :1958 (the grid always holds at least its blank row, so RowCount > 0)
        List<Map<String, Object>> bagRows = list(b.get("emptyBags"));
        List<Map<String, Object>> bags = new ArrayList<>();
        Map<String, String> cfg = repo.configs(CONFIGS);
        boolean emptyBagsCompulsory = flag(cfg.get("EmptyBagsInofrmationCompulsoryOnGRN"));
        double recQty = 0.0, purQty = 0.0;
        for (Map<String, Object> e : bagRows) {
            if (toInt(e.get("Type")) > 0 && toInt(e.get("ItemId")) > 0) {
                if (toInt(e.get("Condition")) == 0) throw invalid("Please Select Bags_Condition First");
                recQty += dbl(e.get("RecQty"));
                purQty += dbl(e.get("PurQty"));
            }
        }
        if (emptyBagsCompulsory && otherThanOpenBulkExist) {
            double q = recQty + purQty;
            if (grnQty != q) throw invalid("Empty Bags Qty must be equal to GrnQty\nGrn Qty is " + net(grnQty) + " and Empty Bags Qty is " + net(q));
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

        // InvGrn.Save (BLL 0576:60)
        if (recId == 0) for (Map<String, Object> x : details) if (toInt(x.get("Id")) > 0) throw invalid("Record cannot be inserted because detailId greater than zero");

        // Attachment form changes (files are stored now and removed again if this transaction rolls back).
        PurchaseDocAttachmentService.Prepared attached = attachments.prepare(recId, DOC, b.get("attachments"));
        if (attached != null && attached.changed()) {
            header.put("AttachmentsValues", attached.names());
            header.put("CustomAttachmentsValues", attached.storedNames());
        }

        // DAL 0429 SetData - one transaction (this method's).
        var values = writes.headerValues(null, header);
        int id = writes.saveHeader(values, recId > 0);
        int line = 1;
        for (Map<String, Object> x : details) writes.saveDetail(x, null, id, line++);
        for (Map<String, Object> o : bags) writes.saveEmptyBag(o, id);
        attachments.persist(id, DOC, supplierId, attached);                                // DAL 0429 :143-177 (RefAccountId = SupplierCustomerId)
        writes.validateAndPost(repo.org(), repo.company(), DOC, id, 0, repo.user());      // Sp_InventoryTransactions + usp_StockInTransit (137)

        Map<String, Object> res = new LinkedHashMap<>();
        res.put("success", true);
        res.put("id", id);
        res.put("docNo", docNo);
        res.put("message", (recId > 0 ? "Record Update Successfully [" : "Record Save Successfully [") + docNo + "]");
        // :2061 - WagesStatus (ContractorWagesCompulsoryBeforeInvoices) && WagesActiveOrInActive -> frmwagesBillHeader for this GRN.
        res.put("openWages", flag(cfg.get("ContractorWagesCompulsoryBeforeInvoices")) && repo.wagesRefDocumentActive());
        res.put("grossWeight", grossWeight);
        return res;
    }

    /** DeductionPolicyForGrn(GrossWeight) :2356 for the Ponch delivery term with the policy on. */
    private void deductionPolicyCheck(double grossWeight, double supplierWeight, double factoryWeight, Timestamp docDate) {
        int policyId = 0;
        String policyName = "";
        double diffWeight = factoryWeight - supplierWeight;
        if (diffWeight > 0.0) {
            Map<String, Object> p = repo.deductionPolicy(docDate, diffWeight);
            if (p != null) {
                policyId = toInt(col(p, "PolicyTypeId"));
                policyName = " Policy is [" + Objects.toString(col(p, "ConditionDescription"), "") + "]";
            }
        }
        double finalBillWt = 0.0;
        switch (policyId) {
            case 0: case 1: finalBillWt = factoryWeight; break;
            case 2: finalBillWt = factoryWeight + diffWeight / 2.0; break;
            case 3: finalBillWt = supplierWeight; break;
            default: break;
        }
        if (grossWeight > 0.0 && grossWeight != finalBillWt)
            throw invalid("GrossWeight " + net(grossWeight) + " Should Equal To FinalWeight " + net(finalBillWt) + "..." + policyName);
    }

    // ================================================================== delete

    @Transactional(rollbackFor = Exception.class)
    public Map<String, Object> delete(int id) {
        if (id <= 0) throw invalid("Record Not Found");
        repo.requireRight("Delete");
        require(id);
        repo.delete(id);
        return row("success", true, "id", id, "message", "Delete Record Successfully");
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
    /** .NET double.ToString(): shortest text without a trailing ".0". */
    static String net(double v) {
        if (v == Math.rint(v) && Math.abs(v) < 1e15) return String.valueOf((long) v);
        return BigDecimal.valueOf(v).stripTrailingZeros().toPlainString();
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
