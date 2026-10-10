package com.mst.services.sale.pcc;

import com.mst.models.UserAccount;
import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.engr.SaleEngrSupport;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.server.ResponseStatusException;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.*;

import static com.mst.services.sale.engr.SaleEngrSupport.*;

/**
 * Screen 549 "DeliveryOrderConcrete" = Architecture.WinApp.pcc.Sale.DeliveryOrderConcrete (Sale Pcc, module 85, document type 1853).
 *
 * Desktop map (DeliveryOrderConcrete.cs, logic lines before InitializeComponent, 3228 lines):
 *   PurchsaeOrder_Load :214, DocumentNoFill :352, vehicleTypefill :374, BinCity :406, BindVehicles :435, BuildingHeightFill :484,
 *   BuildingStoreysFill :517, ReferencePartyFill :550, SupplierNameFill :588, SaleOrderBind :635, ItemBindbyOrderId :659, WareHouseFill :702,
 *   JobLotFill :734, bindvarientunit :766, rdbtnItemName_CheckedChanged :804, CmbItemName_Leave :832, CalculateWeight :859,
 *   AvailableStockGetForLabel :907, AvailableStockUpdateInGrid :939, DocDate_ValueChanged :981, CmbPackUom_TextChanged :1044,
 *   FormValidation :1104, FormValidationDetail :1126, btnRefresh_Click :1216, Reset :1235, ResetDetail :1283, btnplus_Click :1324,
 *   grd_DoubleClick :1343, btnUpdateDetail_Click :1399, GridComboFill :1448, grdSettings :1486, grd_ColumnButtonClick :1595,
 *   grd_CellUpdated :1666, Insert :1780, getUpdate :2064, btnDelete_Click :2111, LoadInGridDetail :2136, LoadDataDetailfromOrder :2151,
 *   GetSaleOrderIds :2192, cmbDateTypeHistory_ValueChanged :2251, HistoryComboBind :2287, FillHistory :2445, HistoryGridSettings :2517,
 *   grdhistory_SelectionChanged :2614, GridDetailSetting :2664, prints :2702-2722, KeyDown :2802, MakeShortCutKeys :2915.
 * Dialog LoadSaleOrderForDeliveryOrderConcrete: StockComboFill, PendingPurchaseOrderRegularForLoad (USP_LoadPendingSaleOrderOnInvoice, 1852),
 *   DetailGridBind, btnLoadOnInvoice_Click_1.
 * BLL/DAL: Architecture.BLL.pcc.InvDeliveryOrder (USP_InvDeliveryOrder_GetAllMethod / _InsertAndUpdate / USP_InvDeliveryOrderDetail_Insert /
 *   USP_DeliveryOrderStackWarningMessage / USP_GetDataForDropDownFromDeliveryOrder / USP_InvDeliveryOrder_Slip), DAL.pcc.InvDeliveryOrder.SetData
 *   (header, every detail, attachments in ONE transaction).
 */
@Service
public class SalePccDeliveryOrderService {
    public static final String SCREEN = "DeliveryOrderConcrete";
    public static final int SCREEN_ID = 549;
    public static final int DOC_TYPE = 1853;
    public static final int ORDER_DOC_TYPE = 1852;
    private static final String GET = "[pcc].[USP_InvDeliveryOrder_GetAllMethod]";

    private final SaleEngrSupport sup;
    private final SalePccLookups lk;
    private final SalePccAttachments attachments;

    public SalePccDeliveryOrderService(SaleEngrSupport sup, SalePccLookups lk, SalePccAttachments attachments) {
        this.sup = sup; this.lk = lk; this.attachments = attachments;
    }

    // ------------------------------------------------------------------ Load / Refresh

    public Map<String, Object> initial() {
        lk.requireView(SCREEN_ID);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("rights", sup.rights(SCREEN));
        m.put("nextNo", nextNo());
        m.putAll(lists());
        m.put("history", historyCombos());
        m.put("dateTypes", SalePccLookups.dateTypes());
        m.put("fyStart", lk.fyStart());
        return m;
    }

    /** ReferencePartyFill, BuildingStoreysFill, BuildingHeightFill, WareHouseFill, JobLotFill, vehicleTypefill, SupplierNameFill, BinCity, BindVehicles. */
    public Map<String, Object> lists() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("refParties", lk.referenceParties());
        m.put("storeys", lk.buildingLookups(2));
        m.put("heights", lk.buildingLookups(1));
        m.put("warehouses", lk.warehouses());
        m.put("jobLots", lk.jobLots());
        m.put("vehicleTypes", lk.vehicleTypes());
        m.put("customers", lk.customers());
        m.put("cities", lk.cities());
        m.put("vehicles", lk.vehicles());
        return m;
    }

    /** InvDeliveryOrder.GenerateCode. */
    public int nextNo() {
        List<Map<String, Object>> r = sup.rows(GET, "OrganizationId", sup.org(), "CompanyId", sup.company(), "DocumentTypeId", DOC_TYPE,
                "BranchesId", idOrNull(sup.branch()), "FinancialYearId", idOrNull(sup.fy()), "Activity", "GenerateCode");
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    public List<Map<String, Object>> orderNos(int customerId) { return lk.orderNos(customerId, ORDER_DOC_TYPE); }

    public List<Map<String, Object>> orderItems(int orderId) { return lk.orderItems(orderId); }

    public List<Map<String, Object>> varients(int itemId) { return lk.varients(itemId); }

    /** AvailableStockGetForLabel. */
    public Map<String, Object> stock(int itemId, String docDate, int warehouseId, int jobLotId, int varientId) {
        double q = itemId > 0 ? lk.stockInHand(itemId, docDateOrToday(docDate), warehouseId, jobLotId, varientId) : 0.0;
        return row("qty", q);
    }

    /** AvailableStockUpdateInGrid: one QtyInHand per grid row (ItemId > 0 || WareHouseId > 0, else 0). */
    public List<Double> stockLines(String docDate, List<Map<String, Object>> lines) {
        LocalDate d = docDateOrToday(docDate);
        List<Double> out = new ArrayList<>();
        for (Map<String, Object> l : lines == null ? List.<Map<String, Object>>of() : lines) {
            int item = toInt(l.get("itemId")), wh = toInt(l.get("wareHouseId"));
            out.add(item > 0 || wh > 0 ? lk.stockInHand(item, d, wh, toInt(l.get("jobLotId")), toInt(l.get("varientId"))) : 0.0);
        }
        return out;
    }

    private static LocalDate docDateOrToday(String s) { LocalDate d = SalePccLookups.parseDate(s); return d == null ? LocalDate.now() : d; }

    // ------------------------------------------------------------------ History

    /** HistoryComboBind: InvDeliveryOrder.GetDataForDropDownFromDeliveryOrder (org + company only), split by Activity. */
    public Map<String, Object> historyCombos() {
        List<Map<String, Object>> cust = new ArrayList<>(), item = new ArrayList<>(), veh = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("[pcc].[USP_GetDataForDropDownFromDeliveryOrder]", "OrganizationId", sup.org(), "CompanyId", sup.company())) {
            Map<String, Object> o = row("Id", r.get("Id"), "Name", r.get("ReferenceName"));
            switch (str(r.get("Activity"))) {
                case "Customer" -> cust.add(o);
                case "Item" -> item.add(o);
                case "VehicleNo" -> veh.add(o);
                default -> { }
            }
        }
        return row("customers", cust, "items", item, "vehicles", veh);
    }

    /** FillHistory. ItemId is Conversion.ToInt(CmbItemHistory.Text) on the desktop (the item NAME), so an item filter is never sent. */
    public List<Map<String, Object>> history(String fromDate, String toDate, int fromNo, int toNo, String vehicleNo, int customerId) {
        Map<String, Boolean> rt = sup.rights(SCREEN);
        boolean all = Boolean.TRUE.equals(rt.get("viewAll"));
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("BranchesId", sup.branch());
        p.put("FinancialYearId", sup.fy()); p.put("DocumentTypeId", DOC_TYPE);
        LocalDate f = SalePccLookups.parseDate(fromDate), t = SalePccLookups.parseDate(toDate);
        if (f != null) p.put("FromDate", f);
        if (t != null) p.put("ToDate", t);
        if (fromNo != 0) p.put("FromDocNo", (double) fromNo);
        if (toNo != 0) p.put("ToDocNo", (double) toNo);
        if (customerId != 0) p.put("SupplierCustomerId", customerId);
        if (vehicleNo != null && !vehicleNo.isEmpty()) p.put("VehicleNo", vehicleNo);
        p.put("IsApproved", false);                                   // ApprovedFilter is never set, so the BLL always sends @IsApproved = false (the proc ignores it)
        p.put("CanViewAllRecord", all);
        if (!all) p.put("EntryUserId", sup.userId());
        p.put("Activity", "FormHistory");
        return com.mst.repositories.support.DesktopProc.rows(sup.jdbc(), GET, p);
    }

    // ------------------------------------------------------------------ ReadById (getUpdate)

    private Map<String, Object> header(int id) {
        List<Map<String, Object>> r = sup.rows(GET, "Id", id, "Activity", "ReadById");
        UserAccount u = sup.user();
        if (r.isEmpty() || toInt(r.get(0).get("OrganizationId")) != u.getOrganizationId() || toInt(r.get(0).get("CompanyId")) != u.getCompanyId()
                || toInt(r.get(0).get("DocumentTypeId")) != DOC_TYPE)
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Delivery order not found in this company");
        return r.get(0);
    }

    private List<Map<String, Object>> details(int id) {
        return sup.rows(GET, "Id", id, "Activity", "ReadByDeliveryOrderHeaderId");
    }

    public Map<String, Object> record(int id) {
        Map<String, Object> h = header(id);
        List<Map<String, Object>> lines = details(id);
        LocalDate d = SalePccLookups.parseDate(str(h.get("DocDate")));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("head", h);
        out.put("lines", lines);
        List<Double> stock = new ArrayList<>();                       // AvailableStockUpdateInGrid after the grid is filled
        for (Map<String, Object> l : lines) {
            int item = toInt(l.get("ItemId")), wh = toInt(l.get("WarehouseId"));
            stock.add(item > 0 || wh > 0 ? lk.stockInHand(item, d == null ? LocalDate.now() : d, wh, toInt(l.get("JobLotId")), toInt(l.get("ItemAttributeVarientId"))) : 0.0);
        }
        out.put("stock", stock);
        out.put("attachments", attachments.list(SCREEN, id));
        return out;
    }

    // ------------------------------------------------------------------ Loader (LoadSaleOrderForDeliveryOrderConcrete)

    public Map<String, Object> loaderCombos() {
        Map<String, Object> m = new LinkedHashMap<>(lk.loaderCombos());
        m.put("fromDate", lk.fyStart());
        return m;
    }

    /** PendingPurchaseOrderRegularForLoad: SaleOrder.LoadSaleOrderOnInvoiceForConcrete (zero filters are not sent). */
    public List<Map<String, Object>> loaderOrders(Map<String, String> q) {
        lk.requireView(SCREEN_ID);
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("BranchesId", sup.branch()); p.put("FinancialYearId", sup.fy());
        p.put("DocumentTypeId", ORDER_DOC_TYPE);
        LocalDate f = SalePccLookups.parseDate(q.get("fromDate")), t = SalePccLookups.parseDate(q.get("toDate"));
        if (f != null) p.put("FromDate", f);
        if (t != null) p.put("Todate", t);
        int fn = toInt(q.get("fromNo")), tn = toInt(q.get("toNo"));
        if (fn != 0) p.put("FromDocNo", (double) fn);
        if (tn != 0) p.put("ToDocNo", (double) tn);
        int c = toInt(q.get("customerId")), i = toInt(q.get("itemId")), cat = toInt(q.get("categoryId")), ty = toInt(q.get("itemTypeId")), par = toInt(q.get("parentCategoryId"));
        if (c != 0) p.put("SupplierCustomerId", c);
        if (i != 0) p.put("ItemId", i);
        if (cat != 0) p.put("ItemCategoryId", cat);
        if (ty != 0) p.put("ItemTypeId", ty);
        if (par != 0) p.put("InventoryParentCategoriesId", par);
        return com.mst.repositories.support.DesktopProc.rows(sup.jdbc(), "[pcc].[USP_LoadPendingSaleOrderOnInvoice]", p);
    }

    // ------------------------------------------------------------------ Save (Insert)

    public static class Line {
        public int id;
        public int supplierCustomerId;
        public int orderId;
        public int orderDetailId;
        public int referencePartyId;
        public String referencePartyName;
        public String referencePartyCellNo;
        public String referencePartyAddress;
        public int buildingStoreyId;
        public int buildingHeightId;
        public String buildingArea;
        public int wareHouseId;
        public int itemId;
        public int attributeVarientId;
        public double varientUnit;
        public int jobLotId;
        public double qty;
        public double itemWeight;
        public double netWeight;
        public int cityId;
        public String remarksDetail;
        public boolean isFoc;
    }

    public static class Request {
        public int id;
        public String docDate;
        public String docNo;
        public int vehicleTypeId;
        public String vehicleNo;
        public String referenceNo;
        public String distance;
        public List<Line> lines = new ArrayList<>();
        public List<Line> removed = new ArrayList<>();
        public boolean ignoreWarning;
        public SaleEngrAttachments.Change attachments;
    }

    @Transactional
    public Map<String, Object> save(Request r) {
        UserAccount u = sup.user();
        Map<String, Boolean> rt = sup.rights(SCREEN);
        lk.need(rt, r.id > 0 ? "update" : "save", r.id > 0 ? "Update" : "Save");
        List<Line> lines = r.lines == null ? List.of() : r.lines;
        if (lines.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        // FormValidation()
        String no = text(r.docNo);
        if (no.isEmpty() || "0".equals(no)) throw new Warning("DocNo Field is Required");
        boolean veh = false;
        for (Map<String, Object> v : lk.vehicleTypes()) if (toInt(v.get("Id")) == r.vehicleTypeId && r.vehicleTypeId > 0) veh = true;
        if (!veh) throw new Warning("VehicleType Field is Required");
        if (text(r.vehicleNo).isEmpty() || "0".equals(text(r.vehicleNo))) throw new Warning("VehicleNo Field is Required");

        boolean approved = false;
        Set<Integer> savedIds = new HashSet<>();
        if (r.id > 0) {
            Map<String, Object> old = header(r.id);
            approved = toBool(old.get("IsApproved"));
            for (Map<String, Object> d : details(r.id)) savedIds.add(toInt(d.get("Id")));
        }

        BigDecimal doQty = BigDecimal.ZERO;
        boolean otherThanFoc = false;
        int rowNo = 0;
        for (Line l : lines) {
            rowNo++;
            doQty = doQty.add(BigDecimal.valueOf(l.qty));
            if (l.orderId == 0) throw new IllegalArgumentException("SaleOrder No Not Found in Detail Grd Row No: " + rowNo);
            if (!l.isFoc) otherThanFoc = true;
        }
        if (!otherThanFoc) throw new IllegalArgumentException("Atleast one Record other than FreeOFCost in grd ");

        // the per-row validation chain of Insert(), in the desktop's nesting order
        rowNo = 0;
        for (Line l : lines) {
            rowNo++;
            if (l.orderId <= 0) throw new IllegalArgumentException("OrderNo Field Required in Detail Grid Row No: " + rowNo);
            if (l.orderDetailId <= 0) throw new IllegalArgumentException("OrderDetailId Field Required in Detail Grid Row No: " + rowNo);
            if (l.supplierCustomerId <= 0) throw new IllegalArgumentException("Customer Field Required in Detail Grid Row No: " + rowNo);
            if (text(l.referencePartyName).isEmpty()) throw new IllegalArgumentException("ReferencePartyName Field Required in Detail Grid Row No: " + rowNo);
            if (text(l.referencePartyCellNo).isEmpty()) throw new IllegalArgumentException("ReferencePartyCellNo Field Required in Detail Grid Row No: " + rowNo);
            if (text(l.referencePartyAddress).isEmpty()) throw new IllegalArgumentException("ReferencePartyAddress Field Required in Detail Grid Row No: " + rowNo);
            if (l.wareHouseId <= 0) throw new IllegalArgumentException("WareHouseName Field Required in Detail Grid Row No: " + rowNo);
            if (l.itemId <= 0) throw new IllegalArgumentException("Item Name Field Required in Detail Grid Row No: " + rowNo);
            if (l.attributeVarientId == 0) throw new IllegalArgumentException("AttributeVarient Required in Detail Grid And row No: " + rowNo);
            if (l.varientUnit == 0) throw new IllegalArgumentException("VarientUnit Required in Detail Grid And row No: " + rowNo);
            if (l.jobLotId <= 0) throw new IllegalArgumentException("JobLot Field Required in Detail Grid Row No: " + rowNo);
            if (!(l.qty > 0)) throw new IllegalArgumentException("ItemQty Field Required in Detail Grid Row No: " + rowNo);
            if (!(l.itemWeight > 0)) throw new IllegalArgumentException("ItemWeight Field Required in Detail Grid Row No: " + rowNo);
            if (!(l.netWeight > 0)) throw new IllegalArgumentException("DoNetWeight Field Required in Detail Grid Row No: " + rowNo);
            if (l.cityId <= 0) throw new IllegalArgumentException("City Field Required in Detail Grid Row No: " + rowNo);
            if (l.id > 0 && !savedIds.contains(l.id)) throw new IllegalArgumentException("Invalid detail row for this delivery order");
        }
        List<Line> removed = r.removed == null ? List.of() : r.removed;
        for (Line l : removed)
            if (l.id <= 0 || !savedIds.contains(l.id)) throw new IllegalArgumentException("Deleted detail row does not belong to this delivery order");

        LocalDate docDate = SalePccLookups.parseDate(r.docDate);
        if (docDate == null) throw new IllegalArgumentException("DocDate Field is Required");

        // InvDeliveryOrder.DeliveryOrderStackWarningMessage(po): the first row that has a warning stops the loop; the user answers Yes / No
        if (!r.ignoreWarning) {
            String w = stackWarning(docDate, lines);
            if (!w.isEmpty()) return row("warning", w);
        }

        Timestamp now = now();
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("Id", r.id);
        h.put("DocumentTypeId", DOC_TYPE);
        h.put("DocNo", toInt(r.docNo));
        h.put("DocDate", docDate);
        h.put("DeliveryOrderTypeId", 1);
        h.put("RefrenenceNo", text(r.referenceNo));
        h.put("VehicleNo", text(r.vehicleNo));
        h.put("VehicleTypeId", r.vehicleTypeId);
        h.put("DoTotalQty", doQty);
        h.put("IsApproved", false);
        h.put("EntryDate", now);
        h.put("EntryUserId", u.getId());
        h.put("ModifyDate", now);
        h.put("ModifyUserId", u.getId());
        h.put("ApprovedDate", now);
        h.put("ApprovedUserId", 0);
        h.put("OrganizationId", u.getOrganizationId());
        h.put("CompanyId", u.getCompanyId());
        h.put("BranchesId", u.getBranchesId());
        h.put("ProjectsId", u.getBranchesId());                       // po.ProjectsId = UserAccount.BranchesId (desktop)
        h.put("FinancialYearId", sup.fy());
        h.put("ActionId", r.id > 0 ? 2 : 1);
        h.put("Distance", text(r.distance));
        List<Map<String, Object>> attBefore = attachments.remember(SCREEN, r.id);
        int num = sup.setProcMap("[pcc].[USP_InvDeliveryOrder_InsertAndUpdate]", h);
        int id = num > 0 ? num : r.id;
        if (id <= 0) throw new IllegalStateException("The delivery order could not be saved.");

        for (Line l : lines) {
            int action = r.id == 0 ? 1 : (l.id > 0 ? 2 : 1);
            sup.setProcMap("[pcc].[USP_InvDeliveryOrderDetail_Insert]", detail(id, l, action, r.id == 0 ? 0 : l.id));
        }
        for (Line l : removed) sup.setProcMap("[pcc].[USP_InvDeliveryOrderDetail_Insert]", detail(id, l, 3, l.id));
        attachments.write(SCREEN, DOC_TYPE, id, id, attBefore, r.attachments);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id);
        out.put("docNo", toInt(r.docNo));
        out.put("updated", r.id > 0);
        out.put("approved", approved);
        out.put("message", (r.id > 0 ? "Data Update Successfully....  " : "Data Save Successfully....  ") + toInt(r.docNo));
        return out;
    }

    private String stackWarning(LocalDate docDate, List<Line> lines) {
        for (Line l : lines) {
            Map<String, Object> p = new LinkedHashMap<>();
            p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company());
            if (l.itemId != 0) p.put("ItemId", l.itemId);
            p.put("DoDate", docDate);
            if (l.wareHouseId != 0) p.put("WarehouseId", l.wareHouseId);
            if (l.jobLotId != 0) p.put("JobLotId", l.jobLotId);
            if (l.attributeVarientId != 0) p.put("ItemAttributeVarientId", l.attributeVarientId);
            p.put("ItemQty", BigDecimal.valueOf(l.qty));
            List<Map<String, Object>> w = com.mst.repositories.support.DesktopProc.rows(sup.jdbc(), "[pcc].[USP_DeliveryOrderStackWarningMessage]", p);
            if (!w.isEmpty()) return str(w.get(0).get("WarningMessage"));
        }
        return "";
    }

    /** GenericProvider.SetProc(InvDeliveryOrderdetail): every non-virtual property; an unset (null) string property is not sent. */
    private Map<String, Object> detail(int headId, Line l, int action, int rowId) {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("IsFOC", l.isFoc);
        d.put("DoNetWeight", BigDecimal.valueOf(l.netWeight));
        d.put("DoQty", BigDecimal.valueOf(l.qty));
        d.put("ItemWeight", BigDecimal.valueOf(l.itemWeight));
        d.put("VarientEquivalent", BigDecimal.valueOf(l.varientUnit));
        d.put("ActionTypeId", action);
        d.put("CityId", l.cityId);
        d.put("Id", rowId);
        d.put("InvDeliveryOrderId", headId);
        d.put("ItemAttributeVarientId", l.attributeVarientId);
        d.put("ItemId", l.itemId);
        d.put("JobLotId", l.jobLotId);
        d.put("SaleOrderDetailId", l.orderDetailId);
        d.put("SaleOrderId", l.orderId);
        d.put("SupplierCustomerId", l.supplierCustomerId);
        d.put("WarehouseId", l.wareHouseId);
        d.put("ReferencePartyAddress", l.referencePartyAddress);
        d.put("ReferencePartyCellNo", l.referencePartyCellNo);
        d.put("ReferencePartyName", l.referencePartyName);
        d.put("RemarksDetail", l.remarksDetail);
        d.put("ReferencePartyId", l.referencePartyId);
        d.put("BuildingHeightId", l.buildingHeightId);
        d.put("BuildingStoreyId", l.buildingStoreyId);
        d.put("BuildingArea", l.buildingArea);
        return d;
    }

    // ------------------------------------------------------------------ Delete (btnDelete_Click)

    @Transactional
    public Map<String, Object> delete(int id) {
        lk.need(sup.rights(SCREEN), "delete", "Delete");
        if (id == 0) throw new IllegalArgumentException("Record Id Not Found");
        Map<String, Object> h = header(id);
        if (toBool(h.get("IsApproved"))) throw new IllegalArgumentException("Record has been approved");
        sup.rows(GET, "EntryUserId", sup.userId(), "Id", id, "Activity", "DeleteById");
        return row("message", "Delete Record Seccessfully");
    }

    // ------------------------------------------------------------------ Prints (CommonServices.DeliveryOrderConcreteSlip / ...CustomerWise)

    /** InvDeliveryOrder.DeliveryOrder_SlipConcrete: "No Record Found For Display" when empty. */
    public List<Map<String, Object>> slipRows(int id) {
        if (id <= 0) throw new IllegalArgumentException("No Record Found For Display");
        lk.need(sup.rights(SCREEN), "print", "Print");
        header(id);
        List<Map<String, Object>> r = sup.rows("[pcc].[USP_InvDeliveryOrder_Slip]", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Id", id);
        if (r.isEmpty()) throw new IllegalArgumentException("No Record Found For Display");
        return r;
    }

    public List<Map<String, Object>> attachmentList(int id) { header(id); return attachments.list(SCREEN, id); }

    public SaleEngrAttachments.Download download(int id, int attachmentId) { header(id); return attachments.download(SCREEN, id, attachmentId); }
}
