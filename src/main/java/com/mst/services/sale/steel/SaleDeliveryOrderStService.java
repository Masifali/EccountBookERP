package com.mst.services.sale.steel;

import com.mst.models.UserAccount;
import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.engr.SaleEngrSupport;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.server.ResponseStatusException;

import java.math.BigDecimal;
import java.math.MathContext;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.*;

import static com.mst.services.sale.engr.SaleEngrSupport.*;

/**
 * Screen 541 "DeliveryOrder_St" = Architecture.WinApp.Steel.Sale.DeliveryOrder_St (Sale Steel, module 84, document type 1506)
 * and its loader frmLoadSaleOrder_St.
 *
 * Desktop map (DeliveryOrder_St.cs): PurchsaeOrder_Load :318, FormValidation :408, FormValidationDetail :437, btnplus_Click :502, grdSettings :528,
 * grd_ColumnButtonClick :625, grd_CellUpdated :702, btnUpdateDetail_Click :783, grd_DoubleClick :828, Insert :912, vehicleTypefill :1162,
 * SupplierNameFill :1179, WareHouseFill :1196, JobLotFill :1213, DocumentNoFill :1229, ItemDetailFillWithoutOrderId :1245, bindRateUomAndItemPackUom :1262,
 * CmbPackingTypeFill :1296, CmbDeliveryOrderTypeFill :1313, CmbReferencePartiesFill :1330, btnRefresh_Click :1371, Reset :1390, ResetDetail :1433,
 * KeyDown :1468, ReadById :1564, gridhistoryfill :1603, HistoryGridSettings :1716, grdhistory_* :1768-1900, CalculateOrderBalanceQtyandWeight :1965,
 * CalculateWeight :2004, LoadInGridDetail :2072, btnLoadSaleOrder_Click :2182, SaleOrderBind :2250, ItemBindbyOrderId :2295, DocDate_ValueChanged :2343,
 * AvailableStockGetByItem :2470.   Loader: frmLoadSaleOrder_St.cs (PendingSaleOrderLoad, GridDetailBind, GridSecondViewFill).
 *
 * Procedures: [ST].[USP_DeliveryOrder_GetAllMethod], ST.USP_InvDeliveryOrder_Insert / _Update, ST.USP_InvDeliveryOrderDetail_Insert,
 * [ST].[USP_GetAvailableStockForDO], [ST].[USP-SaleOrderIdandNoGetBySupplierCustomerId], [ST].[USP_ReadBySaleOrderIdAndOrderItemId],
 * [ST].[USP_LoadSOForDOInDetail], [ST].[USP_PendingSaleOrderForDeliveryOrder], [ST].[USP_PendingSaleOrderDetailForDeliveryOrder].
 */
@Service
public class SaleDeliveryOrderStService {
    public static final String SCREEN = "DeliveryOrder_St";
    public static final int DOC_TYPE = 1506;
    private static final String P_DO = "[ST].[USP_DeliveryOrder_GetAllMethod]";

    private final SaleEngrSupport sup;
    private final SaleSteelSupport db;
    private final SaleSteelLookups look;
    private final SaleEngrAttachments attachments;

    public SaleDeliveryOrderStService(SaleEngrSupport sup, SaleSteelSupport db, SaleSteelLookups look, SaleEngrAttachments attachments) {
        this.sup = sup; this.db = db; this.look = look; this.attachments = attachments;
    }

    // ------------------------------------------------------------------ Load (PurchsaeOrder_Load :318)

    public Map<String, Object> initial() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("rights", sup.rights(SCREEN));
        m.put("decimals", look.amountDecimals());
        m.put("nextNo", nextNo());                                          // DocumentNoFill
        m.put("refParties", look.referenceParties());                       // CmbReferencePartiesFill
        m.put("packingTypes", look.packingTypes());                         // CmbPackingTypeFill
        m.put("warehouses", look.warehouses());                             // WareHouseFill
        m.put("jobLots", look.jobLots());                                   // JobLotFill
        m.put("vehicleTypes", look.vehicleTypes());                         // vehicleTypefill
        m.put("customers", look.customers());                               // SupplierNameFill
        m.put("items", look.items());                                       // ItemDetailFillWithoutOrderId
        m.put("types", List.of(row("Id", 1, "OrderType", "Local")));        // CmbDeliveryOrderTypeFill
        m.put("historyDays", look.historyDays());
        return m;
    }

    /** btnRefresh_Click :1371 */
    public Map<String, Object> refresh() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("refParties", look.referenceParties());
        m.put("packingTypes", look.packingTypes());
        m.put("warehouses", look.warehouses());
        m.put("jobLots", look.jobLots());
        return m;
    }

    /** DocumentNoFill: CommonServices.DeliveryOrderGenerateCodeForSteel(1506) -> InvDeliveryOrder.GenerateCodeForSteel (BranchesId is sent as 0, FinancialYearId when set). */
    public int nextNo() {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("DocumentTypeId", DOC_TYPE); p.put("BranchesId", 0);
        if (sup.fy() != 0) p.put("FinancialYearId", sup.fy());
        p.put("Activity", "GenerateCode");
        List<Map<String, Object>> r = com.mst.repositories.support.DesktopProc.rows(sup.jdbc(), P_DO, p);
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    // ------------------------------------------------------------------ combos that depend on other combos

    /** SaleOrderBind :2250 - SaleOrder.SaleOrderIdandNoBySupplierCustomerId (DocumentTypeId is not set by the desktop: 0). */
    public List<Map<String, Object>> orders(int customerId) {
        return sup.rows("[ST].[USP-SaleOrderIdandNoGetBySupplierCustomerId]", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "OrderSupCustId", customerId, "DocumentTypeId", 0);
    }

    /** ItemBindbyOrderId :2295 - SaleOrder.ReadBySaleOrderIdAndOrderItemId -> (Id = OrderItemId, ItemName, SaleOrderDetailId = Id). */
    public List<Map<String, Object>> orderItems(int orderId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("[ST].[USP_ReadBySaleOrderIdAndOrderItemId]", "SaleOrderId", orderId))
            out.add(row("Id", r.get("OrderItemId"), "ItemName", r.get("ItemName"), "SaleOrderDetailId", r.get("Id")));
        return out;
    }

    /** bindRateUomAndItemPackUom :1262 - CommonServices.GetUomScheduleByItemId. */
    public List<Map<String, Object>> uoms(int itemId) { return look.uoms(itemId); }

    /** InvDeliveryOrder.GetAvailableStockForDOForSteel -> [ST].[USP_GetAvailableStockForDO].AvailableStock. */
    public double availableStock(int warehouseId, int itemId, int jobLotId, String docDate, int packingTypeId, int uomId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("ItemId", itemId); p.put("DocDateTo", date(docDate));
        p.put("WareHouseId", warehouseId); p.put("JobLotId", jobLotId); p.put("InvPackingTypeId", packingTypeId); p.put("ItemUomId", uomId);
        List<Map<String, Object>> r = com.mst.repositories.support.DesktopProc.rows(sup.jdbc(), "[ST].[USP_GetAvailableStockForDO]", p);
        return r.isEmpty() ? 0.0 : toDouble(r.get(0).get("AvailableStock"));
    }

    /** CalculateOrderBalanceQtyandWeight :1965 - InvDeliveryOrder.OrderBalanceWeightforSteel ('GetDoBalWeightByOrder'; 0 when no row). */
    public double orderBalanceWeight(int orderId, int saleOrderDetailId, int itemId) {
        List<Map<String, Object>> r = sup.rows(P_DO, "OrganizationId", sup.org(), "CompanyId", sup.company(), "Id", orderId,
                "SaleOrderDetailId", saleOrderDetailId, "ItemId", itemId, "Activity", "GetDoBalWeightByOrder");
        return r.isEmpty() ? 0.0 : toDouble(r.get(0).get("BalWeight"));
    }

    public static class StockRow { public int itemId; public int warehouseId; public int jobLotId; }

    /** DocDate_ValueChanged :2343 - InventoryStockEvalautionDetail.GetCurrentStockByItemId for each grid row. */
    public List<Double> currentStocks(String docDate, List<StockRow> rows) {
        List<Double> out = new ArrayList<>();
        for (StockRow x : rows == null ? List.<StockRow>of() : rows) {
            Map<String, Object> p = new LinkedHashMap<>();
            p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("ItemId", x.itemId); p.put("DocDateTo", date(docDate));
            p.put("WareHouseId", x.warehouseId); p.put("JobLotId", x.jobLotId); p.put("CropYear", 0); p.put("InvPackingTypeId", 0); p.put("ItemUomId", 0);
            p.put("Activity", "GetCurrentStockByItemId");
            List<Map<String, Object>> r = com.mst.repositories.support.DesktopProc.rows(sup.jdbc(), "Sp_SaleOrder_GetAllMethod", p);
            out.add(r.isEmpty() ? 0.0 : toDouble(r.get(0).get("AvailableStock")));
        }
        return out;
    }

    // ------------------------------------------------------------------ Loader (frmLoadSaleOrder_St)

    public Map<String, Object> loaderCombos() {
        return row("items", look.items(), "customers", look.customers(), "fromDate", fyStart());
    }

    /** FromDate.Value = clsGlobalVariables.ActiveYr.Start_Period. */
    public String fyStart() {
        for (Map<String, Object> r : sup.rows("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", "OrganizationId", sup.org(), "CompanyId", sup.company()))
            if (toInt(r.get("Id")) == sup.fy()) return str(r.get("Start_Period"));
        return "";
    }

    /** PendingSaleOrderLoad: SaleOrder.PendingSaleOrderForDeliveryOrderForSteel (FinancialYearId = the active year). */
    public Map<String, Object> loaderPending(String fromDate, String toDate, int customerId, int itemId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company());
        if (sup.fy() != 0) p.put("FinancialYearId", sup.fy());
        if (customerId != 0) p.put("SupplierCustomerId", customerId);
        if (itemId != 0) p.put("ItemId", itemId);
        Timestamp f = ts(fromDate), t = ts(toDate);
        if (f != null) p.put("DocDateFrom", f);
        if (t != null) p.put("DocDateTo", t);
        return SaleSteelSupport.grid(db.table("[ST].[USP_PendingSaleOrderForDeliveryOrder]", p));
    }

    /** GridDetailBind(Ids, OrderDetailIds) and GridSecondViewFill (no ids): SaleOrder.PendingSaleOrderDetailForDeliveryOrderForSteel. */
    public Map<String, Object> loaderDetail(String ids, String orderDetailIds) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company());
        if (ids != null && !ids.isEmpty()) p.put("Ids", ids);
        if (orderDetailIds != null && !orderDetailIds.isEmpty()) p.put("OrderDetailIds", orderDetailIds);
        return SaleSteelSupport.grid(db.table("[ST].[USP_PendingSaleOrderDetailForDeliveryOrder]", p));
    }

    /** LoadInGridDetail :2072 - SaleOrder.LoadSOForDOInDetail ([ST].[USP_LoadSOForDOInDetail]); the columns arrive in the procedure's order. */
    public Map<String, Object> loadRows(String gdnIds) {
        return SaleSteelSupport.grid(db.table("[ST].[USP_LoadSOForDOInDetail]", "GdnIds", gdnIds, "OrganizationId", sup.org(), "CompanyId", sup.company()));
    }

    // ------------------------------------------------------------------ History (gridhistoryfill :1603)

    public List<Map<String, Object>> history(String dateType, String fromDate, String toDate, int fromNo, int toNo) {
        boolean viewAll = Boolean.TRUE.equals(sup.rights(SCREEN).get("viewAll"));
        UserAccount u = sup.user();
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId()); p.put("CompanyId", u.getCompanyId()); p.put("DocumentTypeId", DOC_TYPE);
        if (sup.fy() != 0) p.put("FinancialYearId", sup.fy());
        p.put("CanViewAllRecord", viewAll);
        if (!viewAll) p.put("EntryUserId", u.getId());
        String fk = "FromDate", tk = "ToDate";
        if ("entry".equals(dateType)) { fk = "EntryFromDate"; tk = "EntryToDate"; }
        else if ("modify".equals(dateType)) { fk = "ModifyFromDate"; tk = "ModifyToDate"; }
        else if ("approved".equals(dateType)) { fk = "ApprovedFromDate"; tk = "ApprovedToDate"; }
        Timestamp f = ts(fromDate), t = ts(toDate);
        if (f != null) p.put(fk, f);
        if (t != null) p.put(tk, t);
        if (fromNo != 0) p.put("DocNoFrom", (double) fromNo);
        if (toNo != 0) p.put("DocNoTo", (double) toNo);
        p.put("Activity", "FormHistory");
        List<Map<String, Object>> src = com.mst.repositories.support.DesktopProc.rows(sup.jdbc(), P_DO, p);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : src) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", r.get("Id")); o.put("DocNo", r.get("DocNo")); o.put("DocDate", r.get("DocDate"));
            o.put("OrderType", toInt(r.get("DeliveryOrderType")) == 1 ? "Local" : "Stock Transfer");
            o.put("ManualNo", r.get("MannualNo")); o.put("VehicleType", r.get("VehicleDescription")); o.put("VehicleNo", r.get("VehicleNo"));
            o.put("DoQty", r.get("DoTotalQty")); o.put("GrossWeight", r.get("TotalGrossWeight")); o.put("NetWeight", r.get("TotalNetWeight"));
            o.put("EntryUser", r.get("EntryUserName")); o.put("EntryDate", r.get("EntryDate"));
            o.put("ModifyUser", r.get("ModifyUserName")); o.put("ModifyDate", r.get("ModifyDate"));
            o.put("IsApproved", toBool(r.get("IsApproved")) ? "Approved" : "Not Approved");
            o.put("ApprovedUser", r.get("ApprovedUserName")); o.put("ApprovedDate", r.get("ApprovedDate"));
            o.put("Remarks", r.get("LoadingInstructions")); o.put("NoOfAttachments", r.get("NoOfAttachments"));
            out.add(o);
        }
        return out;
    }

    // ------------------------------------------------------------------ ReadById :1564 / InvDeliveryOrder.GetByID

    private Map<String, Object> header(int id) {
        List<Map<String, Object>> r = sup.rows(P_DO, "Id", id, "Activity", "ReadById");
        UserAccount u = sup.user();
        if (r.isEmpty() || toInt(r.get(0).get("OrganizationId")) != u.getOrganizationId() || toInt(r.get(0).get("CompanyId")) != u.getCompanyId()
                || toInt(r.get(0).get("DocumentTypeId")) != DOC_TYPE)
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Delivery order not found in this company");
        return r.get(0);
    }

    public List<Map<String, Object>> lines(int id) {
        header(id);
        return sup.rows(P_DO, "Id", id, "Activity", "ReadByIdDetailId");
    }

    public Map<String, Object> record(int id) {
        Map<String, Object> h = header(id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("head", h);
        out.put("lines", sup.rows(P_DO, "Id", id, "Activity", "ReadByIdDetailId"));
        out.put("attachments", attachments.list(SCREEN, id));
        return out;
    }

    // ------------------------------------------------------------------ Save (Insert :912)

    public static class Line {
        public int id, refDocumentTypeId, refDocIdNo, refDocSubIdNo, refPartyId, supplierCustomerId, saleOrderDetailId, orderId;
        public int wareHouseId, itemId, jobLotId, packingTypeId, packUomId;
        public double itemQty, loadQty, loadWeight, grossWeight;
        public String remarks;
    }

    public static class Request {
        public int id;
        public String docDate;
        public String docNo;
        public int deliveryOrderType;
        public String manualNo;
        public String remarks;
        public int vehicleTypeId;
        public String vehicleNo;
        public List<Line> lines = new ArrayList<>();
        public List<Line> removed = new ArrayList<>();
        public SaleEngrAttachments.Change attachments;
    }

    /** FormValidation :408 - the first failing message, word for word. */
    private void formValidation(Request r) {
        String no = text(r.docNo);
        if (no.isEmpty() || "0".equals(no)) throw new Warning("DocNo Field is Required");
        if (r.deliveryOrderType <= 0) throw new Warning("DeliveryOrderType Field is Required");
        if (r.vehicleTypeId <= 0) throw new Warning("Vehicle type Field is Required");
        String v = text(r.vehicleNo);
        if (v.isEmpty() || "0".equals(v)) throw new Warning("VehicleNo Field is Required");
    }

    @Transactional
    public Map<String, Object> save(Request r) {
        UserAccount u = sup.user();
        List<Line> lines = r.lines == null ? List.of() : r.lines;
        if (lines.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        formValidation(r);

        Set<Integer> savedIds = new HashSet<>();
        if (r.id > 0) {
            header(r.id);
            for (Map<String, Object> d : sup.rows(P_DO, "Id", r.id, "Activity", "ReadByIdDetailId")) savedIds.add(toInt(d.get("Id")));
        }

        double doQty = 0.0, netWeight = 0.0;
        for (Line l : lines) {
            doQty += l.itemQty;
            netWeight += l.loadWeight;
            if (l.orderId == 0) throw new IllegalArgumentException("SaleOrder No Not Found");        // DeliveryOrder type text is never StockTransfer
        }
        for (Line l : lines) {                                                                         // the nested checks of the detail loop, in the desktop's order
            if (l.supplierCustomerId <= 0) throw new IllegalArgumentException("Customer Field Required");
            if (l.orderId <= 0) throw new IllegalArgumentException("OrderNo Field Required");
            if (l.saleOrderDetailId <= 0) throw new IllegalArgumentException("OrderDetailId Field Required");
            if (l.itemId <= 0) throw new IllegalArgumentException("Item Name Field Required");
            if (l.packingTypeId <= 0) throw new IllegalArgumentException("PackigType Field Required");
            if (l.packUomId <= 0) throw new IllegalArgumentException("PackUom Field Required");
            if (!(l.loadQty > 0.0)) throw new IllegalArgumentException("LoadQty Field Required");
            if (!(l.loadWeight > 0.0)) throw new IllegalArgumentException("LoadWeight Field Required");
            if (!(l.grossWeight > 0.0)) throw new IllegalArgumentException("GrossWeight Field Required");
            if (l.wareHouseId <= 0) throw new IllegalArgumentException("WareHouseName Field Required");
            if (l.jobLotId <= 0) throw new IllegalArgumentException("JobLoat Field Required");
            if (r.id > 0 && l.id > 0 && !savedIds.contains(l.id)) throw new IllegalArgumentException("Invalid detail row for this delivery order");
        }
        List<Line> removed = r.removed == null ? List.of() : r.removed;
        if (r.id > 0) for (Line l : removed) if (l.id <= 0 || !savedIds.contains(l.id)) throw new IllegalArgumentException("Deleted detail row does not belong to this delivery order");

        Timestamp now = now();
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("Id", r.id);
        h.put("DocumentTypeId", DOC_TYPE);
        h.put("DocNo", toInt(r.docNo));
        h.put("DocDate", ts(r.docDate));
        h.put("DeliveryOrderType", r.deliveryOrderType);
        h.put("MannualNo", text(r.manualNo));
        h.put("LoadingInstructions", text(r.remarks));
        h.put("IsApproved", false);
        h.put("EntryDate", now);
        h.put("EntryUserId", u.getId());
        h.put("ModifyDate", now);
        h.put("ModifyUserId", u.getId());
        h.put("ApprovedDate", now);
        h.put("ApprovedUserId", u.getId());
        h.put("OrganizationId", u.getOrganizationId());
        h.put("FinancialYearId", sup.fy());
        h.put("CompanyId", u.getCompanyId());
        h.put("BranchesId", u.getBranchesId());
        h.put("ProjectsId", u.getBranchesId());
        h.put("DoTotalQty", dec(doQty));
        h.put("TotalGrossWeight", dec(netWeight));                  // the desktop stores the net weight in both (po.TotalGrossWeight = NetWeight)
        h.put("TotalNetWeight", dec(netWeight));
        h.put("VehicleNo", text(r.vehicleNo));
        h.put("VehicleTypeId", String.valueOf(r.vehicleTypeId));
        h.put("PendingForView", 0);
        h.put("ActionId", r.id > 0 ? 2 : 1);
        h.put("ToBranchId", 0);
        SaleEngrAttachments.Change ch = r.attachments;
        int num = sup.setProcMap(r.id > 0 ? "ST.USP_InvDeliveryOrder_Update" : "ST.USP_InvDeliveryOrder_Insert", h);
        int id = num > 0 ? num : r.id;
        if (id <= 0) throw new IllegalStateException("The delivery order could not be saved.");

        if (r.id > 0) for (Line l : removed) sup.setProcMap("ST.USP_InvDeliveryOrderDetail_Insert", detail(id, l, 3));
        for (Line l : lines) {
            int action = r.id == 0 ? 1 : (l.id > 0 ? 2 : 1);
            sup.setProcMap("ST.USP_InvDeliveryOrderDetail_Insert", detail(id, l, action));
        }
        attachments.apply(SCREEN, DOC_TYPE, id, 0, ch);
        sup.setProcMap("[DAW].[USp_DocumentApprovalDetail_Insert]", row("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOC_TYPE, "Id", id, "LimitAmount", BigDecimal.ZERO));

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id);
        out.put("docNo", toInt(r.docNo));
        out.put("updated", r.id > 0);
        out.put("message", (r.id > 0 ? "Data Update Successfully....  " : "Data Save Successfully....  ") + toInt(r.docNo));
        return out;
    }

    /** GenericProvider.SetProc(InvDeliveryOrderDetail): the procedure's 22 parameters (CityId is never set by the desktop: 0). */
    private Map<String, Object> detail(int headId, Line l, int action) {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("Id", action == 1 ? 0 : l.id);
        d.put("InvDeliveryOrderId", headId);
        d.put("SupplierCustomerId", l.supplierCustomerId);
        d.put("SaleOrderId", l.orderId);
        d.put("SaleOrderDetailId", l.saleOrderDetailId);
        d.put("ItemId", l.itemId);
        d.put("PackUomId", l.packUomId);
        d.put("PackingTypeId", l.packingTypeId);
        d.put("JobLotId", l.jobLotId);
        d.put("WarehouseId", l.wareHouseId);
        d.put("LoadingRemarks", text(l.remarks));
        d.put("DoQty", dec(l.loadQty));
        d.put("DoWeight", dec(l.loadWeight));
        d.put("LoadingQty", dec(l.loadQty));
        d.put("LoadingWeight", dec(l.loadWeight));
        d.put("GrossWeight", dec(l.grossWeight));
        d.put("CityId", 0);
        d.put("RefPartyId", l.refPartyId > 0 ? l.refPartyId : 0);
        d.put("RefDocumentTypeId", l.refDocumentTypeId);
        d.put("RefDocIdNo", l.refDocIdNo);
        d.put("RefDocSubIdNo", l.refDocSubIdNo);
        d.put("ActionTypeId", action);
        return d;
    }

    // ------------------------------------------------------------------ attachments

    public List<Map<String, Object>> attachmentList(int id) { header(id); return attachments.list(SCREEN, id); }

    public SaleEngrAttachments.Download download(int id, int attachmentId) { header(id); return attachments.download(SCREEN, id, attachmentId); }

    public Map<String, Object> saveVehicleType(int id, String description) {
        String d = text(description);
        if (!d.isEmpty()) sup.setProc(id > 0 ? "Sp_VehicleType_Update" : "Sp_VehicleType_Insert", "id", id, "vehicledescription", d);
        return row("rows", look.vehicleTypes());
    }

    // ------------------------------------------------------------------ helpers

    private static BigDecimal dec(double v) { return new BigDecimal(v, new MathContext(15, RoundingMode.HALF_EVEN)); }

    private static Timestamp ts(String s) {
        if (s == null || s.isBlank()) return null;
        try { return Timestamp.valueOf(LocalDate.parse(s.trim().substring(0, Math.min(10, s.trim().length()))).atStartOfDay()); } catch (RuntimeException e) { return null; }
    }

    private static Timestamp date(String s) {
        Timestamp t = ts(s);
        return t == null ? now() : t;
    }
}
