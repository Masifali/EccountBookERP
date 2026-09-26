package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.DeliveryOrderPmDto;
import com.mst.repositories.DeliveryOrderPmRepository;
import com.mst.repositories.DeliveryOrderTransferRepository;
import com.mst.repositories.StockAdjustmentRepository;
import com.mst.repositories.StockTransferStoreRepository;
import com.mst.repositories.StoreIssuanceRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.DeliveryOrderPmRepository.DOC;
import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * Packing Material (ModuleId 54) - screen 506 "Delivery Order Packing Material".
 *
 *   Desktop form : Architecture.WinApp.StoreManagement.DeliveryOrderPackingMaterial (ScreenName "DeliveryOrderPackingMaterial")
 *   DocumentTypeId 85, route /packing-material/delivery-order, API /api/packing-material/delivery-order
 *   BLL 0558 / DAL 0411 InvDeliveryOrder (as 331), Model 1006 / 1000, CommonServices.InvDeliveryOrderSlip (262-DeliveryOrderSlip.rpt).
 *
 * SAVE (btnsave / btnupdate → Insert():582 → BLL Save:16 → DAL SetData:16), one transaction:
 * Sp_InvDeliveryOrder_Insert | _Update → Sp_InvDeliveryOrderDetail_Insert per row (grid rows 1 new / 2 existing, then
 * removed rows 3) → [DAW].[USp_DocumentApprovalDetail_Insert] (LimitAmount 0). No stock and no voucher.
 *
 * DESKTOP BEHAVIOUR REPRODUCED (not corrected)
 *  1. Detail validation checks cmbType's TEXT but CmbItemName's VALUE for "Bag Type field is required".
 *  2. Warehouse is not required on a row (never validated); ModifyUser is 0 on insert, and IsApproved is never
 *     set by Insert() - an update always sends false.
 *  3. The Retain check (qty > balance retain qty) runs only when a row is added or changed on the entry bar.
 *  4. Typing a Doc No that exists opens that document (txtdocno Leave → ReadIdByDocNo).
 *  5. The order combo lists PurchaseOrder rows of DocumentTypeId 41 for the customer; when there are none the combo
 *     accepts free text and the row keeps OrderId 0 with OrderNo = the typed number.
 *  6. Update refuses only "Approved Record Not Update" from the value read when the document was opened.
 *
 * DEVIATIONS (web-only)
 *  A. DocNo is the generator value on insert and the stored header's on update.
 *  B. A document is opened / updated / printed only when it is DocumentTypeId 85 of the user's organization and company.
 *  C. Removed rows are rebuilt from the stored detail (the page sends their ids).
 *  D. Attachments, grid layouts and the shortcut popup are not ported.
 */
@Service
public class DeliveryOrderPmService {

    public static final String SCREEN = "DeliveryOrderPackingMaterial";

    private final DeliveryOrderPmRepository own;
    private final DeliveryOrderTransferRepository dor;
    private final StockTransferStoreRepository store;
    private final StockAdjustmentRepository adj;
    private final StoreIssuanceRepository shared;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public DeliveryOrderPmService(DeliveryOrderPmRepository own, DeliveryOrderTransferRepository dor,
                                  StockTransferStoreRepository store, StockAdjustmentRepository adj,
                                  StoreIssuanceRepository shared, StoreScreenRights rights, CurrentUserContext ctx) {
        this.own = own; this.dor = dor; this.store = store; this.adj = adj; this.shared = shared;
        this.rights = rights; this.ctx = ctx;
    }

    // ================================================================================= load

    /** PurchsaeOrder_Load:275. */
    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        int fy = ctx.currentFinancialYearId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN));
        out.put("branches", StockAdjustmentRepository.project(dor.branches(u), "Id", "BranchName"));
        out.put("projects", StockAdjustmentRepository.project(own.projects(u), "Id", "ProjectName"));
        out.put("docNo", own.generateCode(u, fy, branch(u)));
        out.put("customers", customers(u));
        out.put("items", items(u));
        out.put("vehicleTypes", StockAdjustmentRepository.project(dor.vehicleTypes(), "Id", "VehicleDescription"));
        out.put("racks", store.racks(u, branch(u)));
        out.put("uoms", dor.uomSchedule(u));
        out.put("packingMaterialDefaultWarehouse", toInt(shared.config(u, "PackingMaterialDefaultWarehouse")));
        out.put("historyCustomers", historyCustomers());
        return out;
    }

    public int docNo() {
        UserAccount u = ctx.requireAccountingUser();
        return own.generateCode(u, ctx.currentFinancialYearId(), branch(u));
    }

    /** SupplierDtFillFromGlobal:845 - with ERP feature 4 only PartyTypeId 2. */
    private List<Map<String, Object>> customers(UserAccount u) {
        boolean f4 = dor.erpFeature(u, 4);
        int showBoth = toInt(shared.config(u, "ShowBothVendorAndCustomerOnSalesPurchase"));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> s : dor.suppliers(u, showBoth)) if (!f4 || toInt(s.get("PartyTypeId")) == 2) out.add(s);
        return out;
    }

    /** ItemDetailFill:921 - getGlobalAllItems with ItemTypeOfTypeId 14. */
    private List<Map<String, Object>> items(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : dor.allItems(u)) if (toInt(r.get("ItemTypeOfTypeId")) == 14) out.add(r);
        return out;
    }

    public List<Map<String, Object>> historyCustomers() {
        UserAccount u = ctx.requireAccountingUser();
        return StockAdjustmentRepository.project(own.historyCustomers(u, ctx.currentFinancialYearId(), String.valueOf(branch(u))),
                "Id", "ReferenceName");
    }

    /** CmbPurchaseOrderNo:989 - {PurchaseOrderId, PurchaseOrderNo}. */
    public List<Map<String, Object>> orders(int customerId) {
        return StockAdjustmentRepository.project(own.orders(ctx.requireAccountingUser(), customerId), "PurchaseOrderId", "PurchaseOrderNo");
    }

    /** BalRetainQty:1126 (bag type 2) and BalanceStock:1168 (item and date only, "#,##0.##"). */
    public Map<String, Object> balances(int bagTypeId, int customerId, int itemId, String docDate) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("retain", bagTypeId == 2 ? own.balRetainQty(u, customerId, itemId) : null);
        out.put("stock", adj.stockInHand(u, itemId, StoreIssuanceService.pickerDate(docDate), 0, 0, ""));
        return out;
    }

    /** txtdocno Leave → ReadIdByDocNo; 0 when there is none. */
    public int idByDocNo(int docNo) {
        UserAccount u = ctx.requireAccountingUser();
        int id = own.idByDocNo(u, docNo);
        return id > 0 && owned(u, id) != null ? id : 0;
    }

    // ================================================================================= history

    /** gridhistoryfill:1561 - doc / entry / modify / approved date radio; rows kept once per Id. */
    public List<Map<String, Object>> history(String dateType, String from, String to, int fromDocNo, int toDocNo, int customerId) {
        UserAccount u = ctx.requireAccountingUser();
        boolean viewAll = rights.has(SCREEN, "viewAll");
        Timestamp f = blank(from) ? null : StoreIssuanceService.pickerDate(from), t = blank(to) ? null : StoreIssuanceService.pickerDate(to);
        String k = dateType == null ? "doc" : dateType;
        List<Map<String, Object>> out = new ArrayList<>();
        Set<Integer> seen = new HashSet<>();
        for (Map<String, Object> r : own.history(u, ctx.currentFinancialYearId(), viewAll, u.getId(),
                "doc".equals(k) ? f : null, "doc".equals(k) ? t : null, "entry".equals(k) ? f : null, "entry".equals(k) ? t : null,
                "modify".equals(k) ? f : null, "modify".equals(k) ? t : null, "approved".equals(k) ? f : null, "approved".equals(k) ? t : null,
                fromDocNo, toDocNo, customerId)) {
            int id = toInt(ci(r, "Id"));
            if (!seen.add(id)) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", id);
            o.put("DocNo", toInt(ci(r, "DocNo")));
            o.put("DocDate", ci(r, "DocDate"));
            o.put("VehicleType", str(ci(r, "VehicleType")));
            o.put("VehicleNo", str(ci(r, "VehicleNo")));
            o.put("Remarks", str(ci(r, "LoadingInstructions")));
            o.put("EntryDate", ci(r, "EntryDate"));
            o.put("EntryUser", str(ci(r, "EntryUser")));
            o.put("ModifyDate", ci(r, "ModifyDate"));
            o.put("ModifyUser", str(ci(r, "ModifyUser")));
            o.put("NoOfAttachments", toInt(ci(r, "NoOfAttachments")));
            out.add(o);
        }
        return out;
    }

    // ==================================================================================== read

    /** ReadById:724. */
    public Map<String, Object> load(int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> h = owned(u, id);
        if (h == null) return null;
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : own.details(id)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(d, "Id")));
            o.put("BagTypeId", toInt(ci(d, "BagTypeId")));
            o.put("BagType", str(ci(d, "BagType")));
            o.put("SupplierCustomerId", toInt(ci(d, "SupplierCustomerId")));
            o.put("SupplierCustomer", str(ci(d, "SupplierCustomer")));
            o.put("OrderId", toInt(ci(d, "SaleOrderId")));
            o.put("OrderNo", toInt(ci(d, "OrderNo")));
            o.put("ItemId", toInt(ci(d, "ItemId")));
            o.put("ItemCode", str(ci(d, "ItemCode")));
            o.put("Item", str(ci(d, "ItemName")));
            o.put("WareHouseId", toInt(ci(d, "WarehouseId")));
            o.put("WareHouseName", str(ci(d, "WareHouseName")));
            o.put("ItemUOMId", toInt(ci(d, "PackUomId")));
            o.put("ItemUOM", str(ci(d, "PackUOM")));
            o.put("QTY", toDouble(ci(d, "DoQty")));
            o.put("Remarks", str(ci(d, "LoadingRemarks")));
            rows.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", toInt(ci(h, "Id")));
        out.put("BranchesId", toInt(ci(h, "BranchesId")));
        out.put("ProjectsId", toInt(ci(h, "ProjectsId")));
        out.put("DocDate", ci(h, "DocDate"));
        out.put("DocNo", toInt(ci(h, "DocNo")));
        out.put("VehicleType", str(ci(h, "VehicleType")));
        out.put("VehicleNo", str(ci(h, "VehicleNo")));
        out.put("Remarks", str(ci(h, "LoadingInstructions")));
        out.put("IsApproved", bool(ci(h, "IsApproved")));
        out.put("rows", rows);
        return out;
    }

    public boolean owns(int id) { return owned(ctx.requireAccountingUser(), id) != null; }

    // ==================================================================================== save

    /** Insert():582 → BLL 0558 Save:16 → DAL 0411 SetData:16. */
    @Transactional
    public Map<String, Object> save(DeliveryOrderPmDto dto) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = dto.Id;
        if (recId == 0 && !rights.has(SCREEN, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (recId != 0 && !rights.has(SCREEN, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");
        Map<String, Object> existing = null;
        if (recId != 0) {
            existing = owned(u, recId);
            if (existing == null) throw new IllegalArgumentException("Record not found");
            if (bool(ci(existing, "IsApproved"))) throw new IllegalArgumentException("Approved Record Not Update");
        }
        /* FormValidation:343. */
        if (dto.BranchesId == 0 || ids(dor.branches(u), "Id").stream().noneMatch(x -> x == dto.BranchesId))
            throw new IllegalArgumentException("branch field is required");
        if (dto.ProjectsId == 0 || ids(own.projects(u), "Id").stream().noneMatch(x -> x == dto.ProjectsId))
            throw new IllegalArgumentException("Project Field is Required");
        int fy = ctx.currentFinancialYearId();
        int docNo = existing != null ? toInt(ci(existing, "DocNo")) : own.generateCode(u, fy, branch(u));
        if (docNo == 0) throw new IllegalArgumentException("DocNo Field is Required");
        List<DeliveryOrderPmDto.Row> rows = dto.rows == null ? new ArrayList<>() : dto.rows;
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");

        Set<Integer> customers = new HashSet<>(), items = new HashSet<>();
        for (Map<String, Object> c : customers(u)) customers.add(toInt(c.get("Id")));
        for (Map<String, Object> i : items(u)) items.add(toInt(i.get("Id")));
        Map<Integer, Map<String, Object>> stored = new LinkedHashMap<>();
        if (existing != null) for (Map<String, Object> d : own.details(recId)) stored.put(toInt(ci(d, "Id")), d);

        List<Map<String, Object>> detailModels = new ArrayList<>();
        double total = 0d;
        Set<Integer> postedIds = new HashSet<>();
        for (int k = 0; k < rows.size(); k++) {
            DeliveryOrderPmDto.Row r = rows.get(k);
            int n = k + 1;
            if (r.Id > 0 && !stored.containsKey(r.Id)) throw new IllegalArgumentException("Row " + n + " does not belong to this document. Open the document again.");
            if (r.Id > 0) postedIds.add(r.Id);
            if (r.BagTypeId != 1 && r.BagTypeId != 2) throw new IllegalArgumentException("Bag Type field is required");
            if (r.SupplierCustomerId == 0 || !customers.contains(r.SupplierCustomerId)) throw new IllegalArgumentException("Customer field is required");
            if (r.ItemId == 0 || !items.contains(r.ItemId)) throw new IllegalArgumentException("ItemName field is required");
            if (r.ItemUOMId == 0) throw new IllegalArgumentException("PackUOM field is required");
            if (!Double.isFinite(r.QTY)) throw new IllegalArgumentException("Invalid number");
            if (r.Id == 0 && r.BagTypeId == 2 && r.QTY > own.balRetainQty(u, r.SupplierCustomerId, r.ItemId))
                throw new IllegalArgumentException("Qty can Not be Greater than Bal retain Qty When Bag Type Is Retain....");
            total += r.QTY;
            Map<String, Object> vd = detailModel();
            vd.put("DoQty", r.QTY);
            vd.put("DoWeight", r.QTY);
            vd.put("LoadingQty", r.QTY);
            vd.put("LoadingWeight", r.QTY);
            vd.put("Id", r.Id);
            vd.put("ItemId", r.ItemId);
            vd.put("PackUomId", r.ItemUOMId);
            vd.put("SaleOrderId", r.OrderId);
            vd.put("SupplierCustomerId", r.SupplierCustomerId);
            vd.put("WarehouseId", r.WareHouseId);
            vd.put("ActionTypeId", r.Id > 0 ? 2 : 1);
            vd.put("BagTypeId", r.BagTypeId);
            vd.put("LoadingRemarks", r.Remarks == null ? "" : r.Remarks);
            detailModels.add(vd);
        }
        if (recId > 0 && dto.removedIds != null) {                                                        // :661
            for (Integer rid : dto.removedIds) {
                if (rid == null || rid <= 0) continue;
                Map<String, Object> s = stored.get(rid);
                if (s == null || postedIds.contains(rid)) throw new IllegalArgumentException("A removed row does not belong to this document. Open the document again.");
                Map<String, Object> vd = detailModel();
                double q = toDouble(ci(s, "DoQty"));
                vd.put("DoQty", q); vd.put("DoWeight", q); vd.put("LoadingQty", q); vd.put("LoadingWeight", q);
                vd.put("Id", rid);
                vd.put("ItemId", toInt(ci(s, "ItemId")));
                vd.put("PackUomId", toInt(ci(s, "PackUomId")));
                vd.put("SaleOrderId", toInt(ci(s, "SaleOrderId")));
                vd.put("SupplierCustomerId", toInt(ci(s, "SupplierCustomerId")));
                vd.put("WarehouseId", toInt(ci(s, "WarehouseId")));
                vd.put("ActionTypeId", 3);
                vd.put("BagTypeId", toInt(ci(s, "BagTypeId")));
                vd.put("LoadingRemarks", str(ci(s, "LoadingRemarks")));
                detailModels.add(vd);
            }
        }

        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        Map<String, Object> po = new LinkedHashMap<>();                         // Model 1006, declaration order
        po.put("IsApproved", false);
        po.put("IsStockReserved", false);
        po.put("ApprovedDate", null);
        po.put("ExpiryDate", null);
        po.put("ReturnableDate", null);
        po.put("DocDate", StoreIssuanceService.formDate(dto.DocDate, recId));
        po.put("EntryDate", now);
        po.put("ModifyDate", now);
        po.put("DoTotalQty", BigDecimal.valueOf(total));
        po.put("ApprovedUser", 0);
        po.put("TransporterId", 0);
        po.put("BranchesId", dto.BranchesId);
        po.put("ToBranchId", 0);
        po.put("FromBranchId", 0);
        po.put("CompanyId", u.getCompanyId());
        po.put("DocNo", docNo);
        po.put("DocumentTypeId", DOC);
        po.put("EntryUser", u.getId());
        po.put("Id", recId);
        po.put("ModifyUser", recId > 0 ? u.getId() : 0);                         // :607 only on update
        po.put("OrganizationId", u.getOrganizationId());
        po.put("ProjectsId", dto.ProjectsId);
        po.put("EximInvoiceId", 0);
        po.put("FinancialYearId", fy);
        po.put("ActionId", recId == 0 ? 1 : 2);
        po.put("LoadingPortId", 0);
        po.put("SaleTypeId", 0);
        po.put("DepartmentFromId", 0);
        po.put("DepartmentToId", 0);
        po.put("RequestedByLookUpId", 0);
        po.put("ApprovedByLookUpId", 0);
        po.put("LoadingInstructions", dto.Remarks == null ? "" : dto.Remarks);
        po.put("DeliveryOrderType", null);
        po.put("OtherWeightRemarks", null);
        po.put("AccountRemarks", null);
        po.put("VehicleNo", dto.VehicleNo == null ? "" : dto.VehicleNo.trim());
        po.put("ScreenName", SCREEN);
        po.put("VehicleType", dto.VehicleType == null ? "" : dto.VehicleType);
        po.put("GrossWeight", 0d);
        po.put("NetWeight", 0d);
        po.put("PackingWeight", 0d);
        po.put("OtherWeight", 0d);
        po.put("AttachmentsValues", existing == null ? "" : ci(existing, "AttachmentsValues"));
        po.put("CustomAttachmentsValues", existing == null ? "" : ci(existing, "CustomAttachmentsValues"));

        int num = dor.setProc(recId == 0 ? "Sp_InvDeliveryOrder_Insert" : "Sp_InvDeliveryOrder_Update", po);
        int headerId = num > 0 ? num : recId;
        if (headerId <= 0) throw new IllegalStateException("Save returned no document id.");
        for (Map<String, Object> d : detailModels) {
            d.put("InvDeliveryOrderId", headerId);
            dor.setProc("Sp_InvDeliveryOrderDetail_Insert", d);
        }
        dor.setProc("[DAW].[USp_DocumentApprovalDetail_Insert]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOC, "Id", headerId, "LimitAmount", BigDecimal.ZERO));
        Map<String, Object> ok = new LinkedHashMap<>();
        ok.put("success", true);
        ok.put("id", num > 0 ? num : recId);
        ok.put("message", (recId > 0 ? "Data Update Successfully....  " : "Data Save Successfully....  ") + docNo);
        return ok;
    }

    /** Model 1000 InvDeliveryOrderdetail - every non-virtual property at its default (as the 331 port sends it). */
    private static Map<String, Object> detailModel() {
        Map<String, Object> d = new LinkedHashMap<>();
        for (String k : new String[] { "DoQty", "DoWeight", "LoadingQty", "LoadingWeight", "PackingWeight", "TotalPackingWeight",
                "GrossWeight", "StockWeight", "OtherWeight", "InnerQty", "InnerUomId", "InnerEbUnit", "InnerEbTotal", "AccessWtSet", "OuterEbTotal" })
            d.put(k, 0d);
        for (String k : new String[] { "Id", "InvDeliveryOrderId", "InvPackingTypeId", "ItemId", "PackUomId", "CastingTypeId",
                "ItemVariantId", "SaleOrderId", "SaleOrderDetailId", "InvoiceDetailId", "SupplierCustomerId", "WarehouseId",
                "WareHouseToId", "JobLotId", "ToJobLotId", "RefPartyId", "RefDocumentTypeId", "RefDocIdNo", "RefDocSubIdNo",
                "CropYearId", "ExImInvoiceId", "ActionTypeId", "BagTypeId", "ContainerId", "DeliveryTypeId", "AssetId" })
            d.put(k, 0);
        d.put("LoadingRemarks", null);
        d.put("ContainerRemarks", null);
        d.put("InspectionRemarks", null);
        d.put("ItemDiscription", null);
        d.put("DeliveryScheduleId", 0);
        d.put("DeliveryScheduleDetailId", 0);
        d.put("ThirdPartyAnalysisSubId", 0);
        d.put("ThirdPartyAnalysisId", 0);
        d.put("IsAssetItem", false);
        return d;
    }

    // ================================================================================= helpers

    private Map<String, Object> owned(UserAccount u, int id) {
        if (id <= 0) return null;
        Map<String, Object> h = dor.header(id);
        if (h == null) return null;
        if (toInt(ci(h, "DocumentTypeId")) != DOC) return null;
        if (toInt(ci(h, "CompanyId")) != u.getCompanyId()) return null;
        if (toInt(ci(h, "OrganizationId")) != u.getOrganizationId()) return null;
        return h;
    }

    private static List<Integer> ids(List<Map<String, Object>> rows, String key) {
        List<Integer> out = new ArrayList<>();
        for (Map<String, Object> r : rows) out.add(toInt(ci(r, key)));
        return out;
    }

    private static boolean bool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        String s = v == null ? "" : String.valueOf(v).trim();
        return "true".equalsIgnoreCase(s) || "1".equals(s);
    }

    private static int branch(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }
    private static boolean blank(String s) { return s == null || s.trim().isEmpty(); }
}
