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
 * Screen 551 "GoodsDispatchNotesConcrete" = Architecture.WinApp.pcc.Sale.GoodsDispatchNotesConcrete (Sale Pcc, module 85, document type 1855).
 *
 * Desktop map (GoodsDispatchNotesConcrete.cs, logic before InitializeComponent, 4869 lines):
 *   Load :246, BindPendingCustomer :616, BindPendingOrders :667, DeliveryTerm :712, TransporterAcFill :724, bindWareHouse :760, item :794,
 *   bindvarientunit :848, JobLotBind :894, BinCity :917, dtForGridComboFill :943, ConfigurationDefault :953, BindVehicles :999, btnAdd :1115,
 *   grdSettings :1187, grd_CellUpdated :1240, grd_ColumnButtonClick :1350, DeleteCustomerDetailRow :1380, AddRowInContractorWagesGrid :1417,
 *   WagesItemFillFromDetail :1422, GenerateRowsInWagesGrid :1457, grdContractorWagesGridSetting :1528, grdContractorWages_CellUpdated :1623,
 *   grdContractorWages_ColumnButtonClick :1700, WagesValidationWithDetail :1743, BtnGenerateWagesRows :1783, FillWagesGridFilterCombo :1796,
 *   BtnUpdateConctratorInWages :1861, UpdateWagesRateForAllRows :2060, FillGrdPendingOrders :2139, grdPendingOrdersSetting :2174,
 *   grdPendingOrders_ColumnButtonClick :2209, FormValidation :2225, Reset :2381, Insert :2517, ReadById_Update :2823, btnDelete_Click :2907,
 *   History :2983-3340, GenerateCustomerSlip :3362, CalculateWeight :3490, combdeliverytrm_TextChanged :3617, CmbCustomerOrderNos_Leave :3735,
 *   LoadDataDetailfromDeliveryOrder :3789, LoadExpData :3842, KeyDown :3893, AutoUpdatedRecords :4582, btnRecordsUpdate_Click :4828.
 * BLL/DAL: Architecture.BLL.pcc.InvGdn / DAL.pcc.InvGdn.SetData (header [pcc].[USP_InvGdn_InsertAndUpdate], [pcc].[USP_InvGdnDetail_Insert],
 *   [pcc].[USP_InvGDNWagesDetail_Insert], DMS attachments, Sp_InventoryTransactions_GetALLMethod, FIFO stock evaluation (ERP feature 5) or
 *   Sp_InventoryStockEvalautionDetail_Update, USP_StockValidationConcrete, [pcc].[USP_SaleOrderAutoComplete]) in ONE transaction.
 */
@Service
public class SalePccGdnConcreteService {
    public static final String SCREEN = "GoodsDispatchNotesConcrete";
    public static final int SCREEN_ID = 551;
    public static final int DOC_TYPE = 1855;
    public static final int DO_DOC_TYPE = 1853;
    private static final String GET = "[pcc].[USP_InvGdn_GetAllMethod]";

    private final SaleEngrSupport sup;
    private final SalePccLookups lk;
    private final SalePccP2Support p2;
    private final SalePccAttachments attachments;
    private final Map<String, List<Object[]>> paramCache = new java.util.concurrent.ConcurrentHashMap<>();

    public SalePccGdnConcreteService(SaleEngrSupport sup, SalePccLookups lk, SalePccP2Support p2, SalePccAttachments attachments) {
        this.sup = sup; this.lk = lk; this.p2 = p2; this.attachments = attachments;
    }

    // ------------------------------------------------------------------ Load

    public Map<String, Object> initial() {
        lk.requireView(SCREEN_ID);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("rights", sup.rights(SCREEN));
        m.put("nextNo", nextNo());
        m.put("freightAccountId", freightAccountId());                 // config "FreightOutwardAc" (> 0 shows the carriage controls)
        m.put("subsidiary", sup.erpFeature(4));
        m.putAll(lists());
        m.put("pending", pendingOrders());
        m.put("history", historyCombos());
        m.put("dateTypes", SalePccLookups.dateTypes());
        m.put("fyStart", lk.fyStart());
        m.put("defaults", p2.defaults());
        m.put("fmt", p2.fmt());
        return m;
    }

    /** Load / btnRefresh_Click fills. */
    public Map<String, Object> lists() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("refParties", lk.referenceParties());
        m.put("storeys", lk.buildingLookups(2));
        m.put("heights", lk.buildingLookups(1));
        m.put("transporters", p2.accountTitles("10,3"));              // TransporterAcFill: CoaAllocationAccountTitleByAccountTypeIds("10,3")
        m.put("vehicleTypes", lk.vehicleTypes());
        m.put("vehicles", lk.vehicles());
        m.put("warehouses", lk.warehouses());                          // F1 lookups of the detail grid (hidden entry group on the desktop)
        m.put("jobLots", lk.jobLots());
        m.put("cities", lk.cities());
        m.put("contractors", p2.contractors());                       // dtForGridComboFill: SupplierCustomer.ReadByOrganizationCompanyIdForContractorWages
        return m;
    }

    /** dtitem (pricing schedule items at the document date): Id, ItemName, ItemCode, ItemWeight - used by the wages Qty calculation. */
    public List<Map<String, Object>> items(String docDate) { return p2.pricedItems(docDate); }

    public List<Map<String, Object>> varients(int itemId) { return lk.varients(itemId); }

    public int freightAccountId() {
        int v = sup.configInt("FreightOutwardAc");
        return Math.max(v, 0);
    }

    /** InvGdn.GenerateCode. */
    public int nextNo() {
        List<Map<String, Object>> r = sup.rows(GET, "OrganizationId", sup.org(), "CompanyId", sup.company(), "BranchesId", idOrNull(sup.branch()),
                "FinancialYearId", idOrNull(sup.fy()), "DocumentTypeId", DOC_TYPE, "Activity", "GenerateCode");
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    // ------------------------------------------------------------------ pending delivery orders

    /** FillGrdPendingOrders: InvDeliveryOrder.OutstandingDeliveryorderForDispatch(org, company, branch, 1853); one row per Id, quantities summed. */
    public List<Map<String, Object>> pendingOrders() {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("BranchesId", sup.branch()); p.put("DocumentTypeId", DO_DOC_TYPE);
        List<Map<String, Object>> rows = com.mst.repositories.support.DesktopProc.rows(sup.jdbc(), "[pcc].[USP_OutstandingDeliveryorderForDispatch]", p);
        Map<Integer, Map<String, Object>> byId = new LinkedHashMap<>();
        Map<Integer, BigDecimal> qty = new HashMap<>(), wt = new HashMap<>();
        for (Map<String, Object> r : rows) {
            int id = toInt(r.get("Id"));
            qty.merge(id, toDecimal(ci(r, "DoQty")), BigDecimal::add);
            wt.merge(id, toDecimal(ci(r, "DoNetWeight")), BigDecimal::add);
            byId.putIfAbsent(id, r);
        }
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map.Entry<Integer, Map<String, Object>> e : byId.entrySet()) {
            Map<String, Object> r = e.getValue();
            out.add(row("Id", r.get("Id"), "SaleOrderId", r.get("SaleOrderId"), "OrderTypeId", r.get("OrderTypeId"), "OrderType", r.get("OrderType"), "DocNo", r.get("DocNo"),
                    "DocDate", r.get("DocDate"), "CustomerName", r.get("CustomerName"), "ReferenceNo", ci(r, "RefrenenceNo"), "Distance", r.get("Distance"),
                    "VehicleNo", r.get("VehicleNo"), "ItemQty", qty.get(e.getKey()), "ItemWeight", wt.get(e.getKey()), "RemarksHeader", r.get("RemarksHeader")));
        }
        return out;
    }

    private List<Map<String, Object>> pendingRaw(int doId, int recId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("DoId", doId);
        if (recId != 0) p.put("RecId", recId);
        return com.mst.repositories.support.DesktopProc.rows(sup.jdbc(), "[pcc].[USP_GetPendingCustomerAndOrderForGDN]", p);
    }

    /** BindPendingCustomer: one row per customer (SupplierCustomerId, CustomerName, InvDeliveryOrderId). */
    public List<Map<String, Object>> pendingCustomers(int doId) {
        List<Map<String, Object>> out = new ArrayList<>();
        Set<Integer> seen = new HashSet<>();
        for (Map<String, Object> r : pendingRaw(doId, 0))
            if (seen.add(toInt(r.get("SupplierCustomerId"))))
                out.add(row("SupplierCustomerId", toInt(r.get("SupplierCustomerId")), "CustomerName", str(r.get("CustomerName")), "InvDeliveryOrderId", toInt(r.get("InvDeliveryOrderId"))));
        return out;
    }

    /** BindPendingOrders: the customer's orders (SaleOrderId, SaleOrderNo, DoId, DeliveryNo, SupplierCustomerId). */
    public List<Map<String, Object>> pendingOrdersOf(int doId, int customerId, int recId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : pendingRaw(doId, recId))
            if (toInt(r.get("SupplierCustomerId")) == customerId)
                out.add(row("SaleOrderId", toInt(r.get("SaleOrderId")), "SaleOrderNo", toInt(r.get("OrderNo")), "DoId", toInt(r.get("InvDeliveryOrderId")),
                        "DeliveryNo", toInt(r.get("DoNo")), "SupplierCustomerId", toInt(r.get("SupplierCustomerId"))));
        return out;
    }

    /**
     * CmbCustomerOrderNos_Leave: InvDeliveryOrder.GetDeliveryOrderDataByPartyDoAndOrderId, then AvailableStockUpdateInGrid for every row.
     * The caller adds the rows to the grid (LoadDataDetailfromDeliveryOrder).
     */
    public Map<String, Object> deliveryOrderData(int customerId, int orderId, int doId, String docDate) {
        lk.requireView(SCREEN_ID);
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("SupplierCustomerId", customerId); p.put("OrderId", orderId); p.put("DoId", doId);
        List<Map<String, Object>> rows = com.mst.repositories.support.DesktopProc.rows(sup.jdbc(), "[pcc].[USP_GetDeliveryOrderDataByPartyDoAndOrderId]", p);
        List<Double> stock = new ArrayList<>();
        LocalDate d = docDateOrToday(docDate);
        for (Map<String, Object> r : rows) {
            int item = toInt(r.get("ItemId")), wh = toInt(r.get("WareHouseId"));
            stock.add(item > 0 || wh > 0 ? lk.stockInHand(item, d, wh, toInt(r.get("JobLotId")), toInt(r.get("ItemAttributeVarientId"))) : 0.0);
        }
        return row("rows", rows, "stock", stock);
    }

    /** AvailableStockUpdateInGrid / grd_CellUpdated (WareHouseId, JobLotId): QtyInHand per row (ItemId > 0 || WareHouseId > 0, else 0). */
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

    // ------------------------------------------------------------------ contractor wages helpers

    /** ContractorWagesRateSchedule.GetServiceActivityByItemId(2, itemId, date, 0, contractorId): zero contractor is not sent. */
    public List<Map<String, Object>> wageActivities(int itemId, String date, int contractorId) {
        Map<String, Object> p = new LinkedHashMap<>();
        if (contractorId != 0) p.put("ContractorId", contractorId);
        p.put("ItemId", itemId); p.put("RefDocumentTypeId", 2); p.put("EffectedDATE", docDateOrToday(date));
        p.put("Activity", "GetServiceActivityByItemId");
        return com.mst.repositories.support.DesktopProc.rows(sup.jdbc(), "[pcc].[USP_ContractorWagesRateSchedule_GetAllMethod]", p);
    }

    // ------------------------------------------------------------------ History

    /** HistoryComboBind: InvGdn.GetDataForDropDownFromGdn (org + company only), rows with Activity = Supplier_Customer (Id, name). */
    public Map<String, Object> historyCombos() {
        List<Map<String, Object>> cust = new ArrayList<>();
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company());
        for (Map<String, Object> r : com.mst.repositories.support.DesktopProc.rows(sup.jdbc(), "pcc.USP_GetDataForDropDownFromGdn", p))
            if ("Supplier_Customer".equals(str(ci(r, "Activity")))) cust.add(row("Id", r.get("Id"), "CustomerName", ci(r, "name")));
        return row("customers", cust);
    }

    /** FillHistory: InvGdn.FormHistory (USP_InvGdn_GetAllMethod 'FormHistory'); zero filters are not sent. */
    public List<Map<String, Object>> history(String fromDate, String toDate, int fromNo, int toNo, int customerId) {
        Map<String, Boolean> rt = sup.rights(SCREEN);
        boolean all = Boolean.TRUE.equals(rt.get("viewAll"));
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("BranchesId", sup.branch());
        p.put("FinancialYearId", sup.fy()); p.put("DocumentTypeId", DOC_TYPE);
        LocalDate f = SalePccLookups.parseDate(fromDate), t = SalePccLookups.parseDate(toDate);
        if (f != null) p.put("FromDate", f);
        if (t != null) p.put("ToDate", t);
        if (fromNo != 0) p.put("DocNoFrom", (double) fromNo);
        if (toNo != 0) p.put("DocNoTo", (double) toNo);
        if (customerId != 0) p.put("SupplierCustomerId", customerId);
        p.put("CanViewAllRecord", all);
        if (!all) p.put("EntryUserId", sup.userId());
        p.put("Activity", "FormHistory");
        return com.mst.repositories.support.DesktopProc.rows(sup.jdbc(), GET, p);
    }

    // ------------------------------------------------------------------ ReadById (ReadById_Update)

    private Map<String, Object> header(int id) {
        List<Map<String, Object>> r = sup.rows(GET, "Id", id, "Activity", "ReadById");
        UserAccount u = sup.user();
        if (r.isEmpty() || toInt(r.get(0).get("OrganizationId")) != u.getOrganizationId() || toInt(r.get(0).get("CompanyId")) != u.getCompanyId()
                || toInt(r.get(0).get("DocumentTypeId")) != DOC_TYPE)
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Goods dispatch note not found in this company");
        return r.get(0);
    }

    private List<Map<String, Object>> details(int id) { return sup.rows(GET, "Id", id, "Activity", "InvGdnDetail_ReadByInvGdnId"); }

    private List<Map<String, Object>> wages(int id) { return sup.rows(GET, "Id", id, "Activity", "InvGdnWagesDetail_ReadByInvGdnId"); }

    public Map<String, Object> record(int id) {
        Map<String, Object> h = header(id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("head", h);
        out.put("lines", details(id));
        out.put("wages", wages(id));
        // dtSupplier gets the saved customer with the delivery order id (OrderId), then BindPendingOrders(OrderId, customer, Id)
        int doId = toInt(h.get("OrderId")), cust = toInt(h.get("SupplierCustomerId"));
        out.put("customers", List.of(row("SupplierCustomerId", cust, "CustomerName", str(h.get("CustomerName")), "InvDeliveryOrderId", doId)));
        out.put("orders", pendingOrdersOf(doId, cust, id));
        out.put("attachments", attachments.list(SCREEN, id));
        return out;
    }

    /** grdHistory_SelectionChanged -> GetDetailGrdByHeadId (InvGdn.ReadById detail list). */
    public List<Map<String, Object>> detailOf(int id) { header(id); return details(id); }

    // ------------------------------------------------------------------ Save (Insert / AutoUpdatedRecords)

    public static class Line {
        public int id, orderId, orderDetailId, orderNo, doOrderDetailId, wareHouseId, itemId, attributeVarientId, jobLotId, cityId;
        public String wareHouseName, itemCode, itemName, attributeVarient, jobLot, cityName, remarksDetail;
        public double varientUnit, doQty, itemQty, confirmQty, itemWeight, netWeight;
        public boolean isFoc;
    }

    public static class Wage {
        public int id, orderId, orderWagesId, itemId, attributeVarientId, contractorId, contractorWagesRateScheduleId, parentUomId;
        public double varientEquivalent, qty, itemNetWeight, rate, addLess, netAmount, amount;
        public String remarks, serviceActivity, itemName, attributeVarient;
    }

    public static class Request {
        public int id, customerId, saleOrderId, deliveryOrderId, deliveryTermId, buildingHeightId, buildingStoreyId, referencePartyId, transporterId, vehicleTypeId;
        public String docDate, docNo, referenceNo, remarks, buildingArea, referencePartyName, referencePartyAddress, referencePartyCellNo, distance, carriageAmount;
        public String deliveryTerm, gpNo, gpDate, vehicleNo, driverName, driverCellNo, driverCnicNo;
        public boolean autoUpdate;
        public List<Line> lines = new ArrayList<>(), removed = new ArrayList<>();
        public List<Wage> wages = new ArrayList<>();
        public SaleEngrAttachments.Change attachments;
    }

    private static boolean blank(String s) { return s == null || s.trim().isEmpty(); }

    /** FormValidation(): the first failing message, word for word. */
    private void formValidation(Request r) {
        String no = text(r.docNo).trim();
        if (no.isEmpty() || "0".equals(no)) throw new Warning("DocNo Field is Required");
        if (r.customerId <= 0) throw new Warning("CustomerName Field is Required");
        if (r.saleOrderId <= 0) throw new Warning("Customer Order No Field is Required");
        if (blank(r.referencePartyName)) throw new Warning("Ref Party Name Field is Required");
        if (blank(r.referencePartyAddress)) throw new Warning("Ref Party Address Field is Required");
        if (blank(r.referencePartyCellNo)) throw new Warning("Ref Party CellNo Field is Required");
        double fr = toDecimal(r.carriageAmount).doubleValue();
        if (fr != 0 && r.transporterId <= 0) throw new Warning("Carriage Ac is Required when Carriage amount is greater than Zero");
        if (fr == 0 && r.transporterId > 0) throw new Warning("Carriage Amount is Required when Carriage Ac is Selected");
        if (r.deliveryTermId <= 0) throw new Warning("Delivery Term Field is Required");
        if (r.vehicleTypeId <= 0) throw new Warning("Vehicle Type Term Field is Required");
        if (blank(r.vehicleNo)) throw new Warning("Vehicle No Field is Required");
    }

    /** WagesValidationWithDetail(). */
    private void wagesValidation(Request r) {
        List<Line> lines = r.lines == null ? List.of() : r.lines;
        List<Wage> wg = r.wages == null ? List.of() : r.wages;
        if (lines.isEmpty() || wg.isEmpty()) return;
        // dtVarientForWages is filled from the grid whenever it has rows
        for (Wage w : wg) {
            if (w.itemId == 0) continue;
            boolean exists = false;
            for (Line l : lines) if (l.itemId == w.itemId) exists = true;
            if (!exists) throw new IllegalArgumentException("Item " + str(w.itemName) + " in Contractor Wages Grid  does not exist in Detail Grid ");
            BigDecimal detailQty = BigDecimal.ZERO, wageQty = BigDecimal.ZERO;
            for (Line l : lines) if (l.itemId == w.itemId && l.attributeVarientId == w.attributeVarientId) detailQty = detailQty.add(BigDecimal.valueOf(l.itemQty));
            for (Wage x : wg) if (x.itemId == w.itemId && x.attributeVarientId == w.attributeVarientId && x.contractorWagesRateScheduleId == w.contractorWagesRateScheduleId)
                wageQty = wageQty.add(BigDecimal.valueOf(x.qty));
            if (detailQty.compareTo(wageQty) != 0)
                throw new IllegalArgumentException("Qty is not equal to Detail Qty of Item " + str(w.itemName) + " and Varient " + str(w.attributeVarient) + " in Contractor Wages Grid ");
        }
    }

    @Transactional
    public Map<String, Object> save(Request r) {
        UserAccount u = sup.user();
        Map<String, Boolean> rt = sup.rights(SCREEN);
        if (!r.autoUpdate) lk.need(rt, r.id > 0 ? "update" : "save", r.id > 0 ? "Update" : "Save");
        List<Line> rows = r.lines == null ? List.of() : r.lines;
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        formValidation(r);
        double freight = toDecimal(r.carriageAmount).doubleValue();
        if (freight > 0) {
            if (r.transporterId == 0) throw new IllegalArgumentException("Transporter Account field Required");
            // txtSupplierGLId is never filled on the desktop (always 0), so the "same as Customer" check can never fire
        }
        Map<String, Object> old = null;
        if (r.id > 0) {
            old = header(r.id);
            if (!r.autoUpdate && toBool(old.get("IsApproved"))) throw new IllegalArgumentException("Record Not Update beacause Record has approved");
        }
        wagesValidation(r);

        // per-row validation, in the desktop's order
        Set<Integer> savedIds = new HashSet<>();
        if (r.id > 0) for (Map<String, Object> d : details(r.id)) savedIds.add(toInt(d.get("Id")));
        boolean notFoc = false;
        int n = 0;
        for (Line l : rows) {
            n++;
            if (l.id > 0 && !savedIds.contains(l.id)) throw new IllegalArgumentException("Invalid detail row for this goods dispatch note");
            if (l.wareHouseId == 0) throw new IllegalArgumentException("Warehouse Required in Detail Grid And row No: " + n);
            if (l.itemId == 0) throw new IllegalArgumentException("Item Required in Detail Grid And row No: " + n);
            if (l.attributeVarientId == 0) throw new IllegalArgumentException("AttributeVarient Required in Detail Grid And row No: " + n);
            if (l.varientUnit == 0) throw new IllegalArgumentException("VarientUnit Required in Detail Grid And row No: " + n);
            if (l.jobLotId == 0) throw new IllegalArgumentException("JobLot Required in Detail Grid And row No: " + n);
            if (l.itemQty == 0) throw new IllegalArgumentException("ItemQty Required in Detail Grid And row No: " + n);
            if (l.confirmQty == 0) throw new IllegalArgumentException("ConfirmQty Required in Detail Grid And row No: " + n);
            if (l.itemWeight == 0) throw new IllegalArgumentException("ItemWeight Required in Detail Grid And row No: " + n);
            if (l.netWeight == 0) throw new IllegalArgumentException("NetWeight Required in Detail Grid And row No: " + n);
            if (l.confirmQty > l.itemQty) throw new IllegalArgumentException("Confirm Qty can not be greater than Item Qty in Detail Grid And row No: " + n);
            if (!notFoc) notFoc = !l.isFoc;
        }
        List<Line> removed = r.removed == null ? List.of() : r.removed;
        for (Line l : removed)
            if (l.id <= 0 || !savedIds.contains(l.id)) throw new IllegalArgumentException("Deleted detail row does not belong to this goods dispatch note");
        if (!notFoc) throw new IllegalArgumentException("Not all records can be FreeOfCost in Detail Grid: ");

        List<Wage> wages = new ArrayList<>();
        n = 0;
        for (Wage w : r.wages == null ? List.<Wage>of() : r.wages) {
            n++;
            if (!(w.contractorWagesRateScheduleId > 0 || w.contractorId > 0 || w.itemId > 0 || w.attributeVarientId > 0)) continue;
            if (w.itemId == 0) throw new Warning("Item  Field Required in Grid Contractor Wages Row No: " + n);
            if (w.attributeVarientId == 0) throw new Warning("AttributeVarient  Field Required in Grid Contractor Wages Row No: " + n);
            if (w.contractorId == 0) throw new Warning("Contractor  Field Required in Grid Contractor Wages Row No: " + n);
            if (w.contractorWagesRateScheduleId == 0) throw new Warning("Service Activity Field Required in Grid Contractor Wages Row No: " + n);
            if (w.amount == 0) throw new Warning("Amount Field Required in Grid Contractor Wages Row No: " + n);
            wages.add(w);
        }

        LocalDate dd = SalePccLookups.parseDate(r.docDate);
        if (dd == null) throw new IllegalArgumentException("DocDate Field is Required");
        java.sql.Date docDate = java.sql.Date.valueOf(dd);
        LocalDate gp = SalePccLookups.parseDate(r.gpDate);
        Timestamp now = now();
        boolean update = r.id > 0;
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("Id", r.id);
        h.put("DocumentTypeId", DOC_TYPE);
        h.put("DocDate", docDate);
        h.put("DocNo", toInt(r.docNo));
        h.put("SupplierCustomerId", r.customerId);
        h.put("OrderTypeId", 0);
        h.put("OrderId", r.deliveryOrderId);
        h.put("ReferenceNo", text(r.referenceNo).trim());
        h.put("DeliveryTerm", text(r.deliveryTerm));
        h.put("ReferencPartyName", text(r.referencePartyName));
        h.put("ReferencPartyAddress", text(r.referencePartyAddress));
        h.put("ReferencPartyCellNo", text(r.referencePartyCellNo));
        h.put("GpNo", toInt(r.gpNo));
        h.put("GpDate", gp == null ? new java.sql.Date(System.currentTimeMillis()) : java.sql.Date.valueOf(gp));
        h.put("VehicleTypeId", r.vehicleTypeId);
        h.put("VehicleNo", text(r.vehicleNo));
        h.put("TransporterId", r.transporterId);
        h.put("CarriageAmount", BigDecimal.valueOf(freight));
        h.put("RemarksHeader", text(r.remarks).trim());
        h.put("DriverName", text(r.driverName));
        h.put("DriverCNICNo", text(r.driverCnicNo));
        h.put("DriverCellNo", text(r.driverCellNo));
        h.put("IsApproved", false);
        h.put("EntryDate", now);
        h.put("EntryUserId", update ? 0 : u.getId());                 // BLL.Save: EntryUserId = 0 on update
        h.put("ModifyDate", now);
        h.put("ModifyUserId", update ? u.getId() : 0);                // BLL.Save: ModifyUserId = 0 on insert
        h.put("ApprovedDate", now);
        h.put("ApprovedUserId", 0);
        h.put("OrganizationId", u.getOrganizationId());
        h.put("CompanyId", u.getCompanyId());
        h.put("BranchesId", u.getBranchesId());
        h.put("ProjectsId", u.getBranchesId());                       // obj.ProjectsId = UserAccount.BranchesId (desktop)
        h.put("FinancialYearId", sup.fy());
        h.put("ActionId", update ? 2 : 1);
        h.put("Distance", text(r.distance));
        h.put("AutoUpdate", r.autoUpdate ? 1 : 0);
        h.put("ReferencePartyId", r.referencePartyId);
        h.put("BuildingHeightId", r.buildingHeightId);
        h.put("BuildingStoreyId", r.buildingStoreyId);
        h.put("BuildingArea", text(r.buildingArea));
        List<Map<String, Object>> attBefore = attachments.remember(SCREEN, r.id);
        int num = sup.setProcMap("[pcc].[USP_InvGdn_InsertAndUpdate]", h);
        int id = num > 0 ? num : r.id;
        if (id <= 0) throw new IllegalStateException("The goods dispatch note could not be saved.");

        // details: the grid rows first, then the removed ones (ActionTypeId 3); LineId counts them all
        List<Line> all = new ArrayList<>(rows);
        all.addAll(removed);
        int lineId = 0;
        Map<Integer, Integer> detailIdByLine = new HashMap<>();
        for (Line l : all) {
            lineId++;
            boolean gone = lineId > rows.size();
            int rowId = r.id == 0 ? 0 : l.id;
            int action = gone ? 3 : (rowId <= 0 ? 1 : 2);
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("Id", rowId);
            d.put("InvGdnId", id);
            d.put("LineId", lineId);
            d.put("OrderTypeId", 1);
            d.put("OrderId", l.orderId);
            d.put("OrderDetailId", l.orderDetailId);
            d.put("DoOrderDetailId", l.doOrderDetailId);
            d.put("WarehouseId", l.wareHouseId);
            d.put("ItemId", l.itemId);
            d.put("ItemAttributeVarientId", l.attributeVarientId);
            d.put("VarientEquivalent", BigDecimal.valueOf(l.varientUnit));
            d.put("JobLotId", l.jobLotId);
            d.put("ItemQty", BigDecimal.valueOf(l.itemQty));
            d.put("ItemWeight", BigDecimal.valueOf(l.itemWeight));
            d.put("ItemNetWeight", BigDecimal.valueOf(l.netWeight));
            d.put("CityId", l.cityId);
            d.put("IsFOC", l.isFoc);
            d.put("RemarksDetail", text(l.remarksDetail));
            d.put("ActionTypeId", action);
            d.put("ItemUomId", 0);
            d.put("ScheduleId", 0);
            d.put("ConfirmQty", l.confirmQty);
            int did = sup.setProcMap("[pcc].[USP_InvGdnDetail_Insert]", d);
            detailIdByLine.put(lineId, did > 0 ? did : rowId);
        }
        for (Wage w : wages) {
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", w.id);
            x.put("InvGdnId", id);
            x.put("SaleOrderId", w.orderId);
            x.put("SaleOrderWagesId", w.orderWagesId);
            x.put("ContractorId", w.contractorId);
            x.put("ItemId", w.itemId);
            x.put("ItemAttributeVarientId", w.attributeVarientId);
            x.put("VarientEquivalent", BigDecimal.valueOf(w.varientEquivalent));
            x.put("ContractorWagesRateScheduleId", w.contractorWagesRateScheduleId);
            x.put("ParentUomId", w.parentUomId);
            x.put("Qty", BigDecimal.valueOf(w.qty));
            x.put("ItemNetWeight", BigDecimal.valueOf(w.itemNetWeight));
            x.put("Remarks", text(w.remarks));
            x.put("Rate", BigDecimal.valueOf(w.rate));
            x.put("AddLess", BigDecimal.valueOf(w.addLess));
            x.put("NetAmount", BigDecimal.valueOf(w.netAmount));
            x.put("Amount", BigDecimal.valueOf(w.amount));
            sup.setProcMap("[pcc].[USP_InvGDNWagesDetail_Insert]", x);
        }
        attachments.write(SCREEN, DOC_TYPE, id, r.customerId, attBefore, r.attachments);

        // InventoryTransactions model through Sp_InventoryTransactions_GetALLMethod
        sup.setProcMap("Sp_InventoryTransactions_GetALLMethod", full("Sp_InventoryTransactions_GetALLMethod",
                row("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "RefDocumentTypeId", DOC_TYPE, "RefDocIdNo", id)));
        if (sup.erpFeature(5)) fifo(u, id, update, r, docDate, rows, detailIdByLine);
        else sup.setProcMap("Sp_InventoryStockEvalautionDetail_Update", full("Sp_InventoryStockEvalautionDetail_Update",
                row("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "RefDocumentTypeId", DOC_TYPE, "RefDocIdNo", id)));
        for (Line l : rows) {
            Map<String, Object> v = new LinkedHashMap<>();
            v.put("OrganizationId", u.getOrganizationId()); v.put("CompanyId", u.getCompanyId()); v.put("DocumentTypeId", DOC_TYPE); v.put("DocDate", docDate);
            v.put("ItemId", l.itemId); v.put("WarehouseId", l.wareHouseId);
            if (l.attributeVarientId > 0) v.put("VarientId", l.attributeVarientId);
            v.put("Qty", BigDecimal.valueOf(l.itemQty));
            com.mst.repositories.support.DesktopProc.scalar(sup.jdbc(), "USP_StockValidationConcrete", v);
        }
        for (Line l : all) {
            Map<String, Object> v = new LinkedHashMap<>();
            v.put("OrganizationId", u.getOrganizationId()); v.put("CompanyId", u.getCompanyId()); v.put("SaleOrderId", l.orderId);
            com.mst.repositories.support.DesktopProc.scalar(sup.jdbc(), "[pcc].[USP_SaleOrderAutoComplete]", v);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id);
        out.put("docNo", toInt(r.docNo));
        out.put("updated", update);
        out.put("message", (update ? "Record Update Successfully [" : "Record Saved Successfully [") + toInt(r.docNo) + "] ");
        return out;
    }

    // ------------------------------------------------------------------ FIFO stock evaluation (DAL.pcc.InvGdn.SetData / CommonServices.FIFOImplementionForConcrete)

    private void fifo(UserAccount u, int id, boolean update, Request r, java.sql.Date docDate, List<Line> rows, Map<Integer, Integer> detailIdByLine) {
        Map<Integer, String> names = new HashMap<>();
        for (Map<String, Object> it : sup.rows("Sp_Item_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetItemGlIdsandItemName"))
            names.putIfAbsent(toInt(it.get("Id")), str(it.get("ItemName")));
        List<Map<String, Object>> allocations = new ArrayList<>();
        int lineId = 0;
        for (Line l : rows) {
            lineId++;
            String itemName = names.get(l.itemId);
            if (itemName == null) continue;                            // only items present in GetItemGlIdsandItemName are costed
            Map<String, Object> p = new LinkedHashMap<>();
            p.put("OrganizationId", u.getOrganizationId()); p.put("CompanyId", u.getCompanyId()); p.put("ItemId", l.itemId); p.put("DocDate", docDate);
            if (l.attributeVarientId != 0) p.put("VarientId", l.attributeVarientId);
            if (l.wareHouseId != 0) p.put("WarehouseId", l.wareHouseId);
            // JobLotId is never set on the ReportsParameters of the desktop call, so it is not sent either
            if (update) { p.put("DocumentTypeId", DOC_TYPE); p.put("Id", id); }
            if (!allocations.isEmpty()) p.put("FIFOXML", fifoXml(allocations));
            List<Map<String, Object>> stocks = com.mst.repositories.support.DesktopProc.rows(sup.jdbc(), "[pcc].[USP_GetStockByFifoMethod]", p);
            if (stocks == null || stocks.isEmpty()) throw new IllegalStateException("Stock Not Found this Item " + itemName + " against FIFO Method .....");
            double available = 0;
            for (Map<String, Object> s : stocks) available += toDouble(ci(s, "NetBalQty"));
            if (!(l.itemQty <= available))
                throw new IllegalStateException("Qty available is " + cs(available) + " and row Qty is " + cs(l.itemQty) + " this item " + itemName + " against FIFO....");
            double need = l.itemQty, used = 0;
            for (Map<String, Object> s : stocks) {
                double avg = toDouble(ci(s, "AvgRate"));
                if (avg <= 0) throw new IllegalStateException("Rate Not Found this Item " + itemName + " against FIFO Method");
                double balQty = toDouble(ci(s, "NetBalQty"));
                double qtyOut;
                if (balQty <= need - used) qtyOut = balQty;
                else if (balQty >= need - used) qtyOut = need - used;
                else continue;
                used += qtyOut;
                Map<String, Object> x = new LinkedHashMap<>();
                x.put("Id", toInt(ci(s, "Id")));
                x.put("LineId", lineId);
                x.put("ItemId", l.itemId);
                x.put("WarehouseId", l.wareHouseId);
                x.put("RateUom", toInt(ci(s, "RateUomId")));
                x.put("JobLotId", l.jobLotId);
                x.put("InvPackingTypeId", 0);
                x.put("ItemUom", 0);                                   // item5.ItemUom = InvGdnDetail.ItemUomId (never set on this screen)
                x.put("VarientId", l.attributeVarientId);
                x.put("RefRefDocumentTypeId", toInt(ci(s, "RefDocumentTypeId")));
                x.put("RefRefDocIdNo", toInt(ci(s, "RefDocIdNo")));
                x.put("RefRefDocSubIdNo", toInt(ci(s, "RefDocSubIdNo")));
                x.put("QtyOut", qtyOut);
                x.put("BillWeightOut", qtyOut);
                x.put("StockWeightOut", qtyOut);
                x.put("CgsRate", avg);
                x.put("CgsAmount", qtyOut * avg);
                allocations.add(x);
                if (l.itemQty == used) break;
            }
        }
        if (allocations.isEmpty()) return;
        if (update)
            sup.setProc("[dbo].[USP_InventoryQtyReverseAndDeleteByReferenceId]", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                    "RefDocumentTypeId", DOC_TYPE, "RefDocIdNo", id);
        for (Map<String, Object> x : allocations) {
            int ln = toInt(x.get("LineId"));
            Map<String, Object> m = new LinkedHashMap<>(x);
            m.put("OrganizationId", u.getOrganizationId()); m.put("CompanyId", u.getCompanyId());
            m.put("DocDate", docDate); m.put("DocCodeNo", toInt(r.docNo)); m.put("SupplierCustomerId", r.customerId); m.put("BranchesId", u.getBranchesId());
            m.put("OtherDocumentTypeId", DOC_TYPE);
            m.put("EntryUser", update ? 0 : u.getId());                // obj.EntryUserId (0 on update)
            m.put("ModifyUser", update ? u.getId() : 0);               // obj.ModifyUserId (0 on insert)
            m.put("CalcType", "Qty");
            m.put("OtherDocNoId", id);
            m.put("OtherSubDocNoId", detailIdByLine.getOrDefault(ln, 0));
            sup.setProcMap("USP_InventoryStockEvalautionDetail_Insert", full("USP_InventoryStockEvalautionDetail_Insert", m));
        }
        sup.setProc("pcc.usp_GdnDetailInsertintoStockTable", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "RefDocumentTypeId", DOC_TYPE, "InvGdnId", id);
    }

    /** CommonServices.ConvertListToXmlSerializer(List&lt;FIFOStockEvaluation&gt;). */
    private static String fifoXml(List<Map<String, Object>> reserved) {
        StringBuilder xml = new StringBuilder("<ArrayOfFIFOStockEvaluation>");
        for (Map<String, Object> a : reserved)
            xml.append("<FIFOStockEvaluation><RefDocumentTypeId>").append(toInt(a.get("RefRefDocumentTypeId"))).append("</RefDocumentTypeId><RefDocIdNo>").append(toInt(a.get("RefRefDocIdNo")))
                    .append("</RefDocIdNo><RefDocSubIdNo>").append(toInt(a.get("RefRefDocSubIdNo"))).append("</RefDocSubIdNo><ReserveQty>").append(cs(toDouble(a.get("QtyOut"))))
                    .append("</ReserveQty><ReserveWeight>").append(cs(toDouble(a.get("StockWeightOut")))).append("</ReserveWeight></FIFOStockEvaluation>");
        return xml.append("</ArrayOfFIFOStockEvaluation>").toString();
    }

    /** .NET double.ToString(). */
    private static String cs(double v) {
        if (v == Math.rint(v) && Math.abs(v) < 1e15) return String.valueOf((long) v);
        return BigDecimal.valueOf(v).stripTrailingZeros().toPlainString();
    }

    /** GenericProvider.SetProc of a model: every procedure parameter the caller did not give receives the property default (0 / false). */
    private Map<String, Object> full(String proc, Map<String, Object> given) {
        List<Object[]> params = paramCache.computeIfAbsent(proc, p -> {
            List<Object[]> l = new ArrayList<>();
            for (Map<String, Object> r : sup.jdbc().queryForList("SELECT p.name, t.name AS tname FROM sys.parameters p JOIN sys.types t ON p.user_type_id = t.user_type_id "
                    + "WHERE p.object_id = OBJECT_ID(?) ORDER BY p.parameter_id", p.contains(".") ? p : "dbo." + p))
                l.add(new Object[]{String.valueOf(r.get("name")).replace("@", ""), String.valueOf(r.get("tname")).toLowerCase()});
            return l;
        });
        Map<String, Object> out = new LinkedHashMap<>(given);
        for (Object[] p : params) {
            String nm = (String) p[0], t = (String) p[1];
            boolean has = false;
            for (String k : out.keySet()) if (k.equalsIgnoreCase(nm)) { has = true; break; }
            if (has) continue;
            if (t.equals("bit")) out.put(nm, false);
            else if (t.equals("tinyint") || t.equals("smallint") || t.equals("int") || t.equals("bigint")) out.put(nm, 0);
            else if (t.equals("float") || t.equals("real") || t.equals("decimal") || t.equals("numeric") || t.equals("money") || t.equals("smallmoney")) out.put(nm, 0.0);
        }
        return out;
    }

    // ------------------------------------------------------------------ Delete (btnDelete_Click)

    @Transactional
    public Map<String, Object> delete(int id) {
        lk.need(sup.rights(SCREEN), "delete", "Delete");
        if (id <= 0) throw new IllegalArgumentException("Record Id Not Found");
        Map<String, Object> h = header(id);
        if (toBool(h.get("IsApproved"))) throw new IllegalArgumentException("Record Not Delete beacause Record has approved");
        sup.rows(GET, "OrganizationId", sup.org(), "CompanyId", sup.company(), "DocumentTypeId", DOC_TYPE, "EntryUserId", sup.userId(), "Id", id, "Activity", "DeleteById");
        return row("message", "Deleted Successfully");
    }

    /** GetGdnIdsForAutoUpdation (btnRecordsUpdate_Click). */
    public List<Integer> autoUpdateIds() {
        lk.need(sup.rights(SCREEN), "update", "Update");
        List<Map<String, Object>> r = sup.rows("[pcc].[USP_GetGdnIdsForAutoUpdation]", "OrganizationId", sup.org(), "CompanyId", sup.company(), "DocumentTypeId", DOC_TYPE);
        if (r.isEmpty()) throw new IllegalArgumentException("Ids Not Found For Update");
        List<Integer> ids = new ArrayList<>();
        for (Map<String, Object> x : r) ids.add(toInt(x.get("Id")));
        return ids;
    }

    // ------------------------------------------------------------------ Prints (GenerateCustomerSlip)

    /** InvGdn.GdnConcrete_Slip: "Not Record Found For Display" when empty. */
    public List<Map<String, Object>> slipRows(int id) {
        lk.need(sup.rights(SCREEN), "print", "Print");
        if (id > 0) header(id);
        List<Map<String, Object>> r = id > 0 ? sup.rows("[pcc].[USP_InvGdn_Slip]", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Id", id) : List.of();
        if (r.isEmpty()) throw new IllegalArgumentException("Not Record Found For Display");
        return r;
    }

    /** InvGdn.GdnWagesDetail_SubReport(id) feeding InvGdn_SubReport.rpt. */
    public List<Map<String, Object>> slipWages(int id) { return sup.rows("[pcc].[USP_InvGdnWagesDetail_SubReport]", "InvGdnId", id); }

    public List<Map<String, Object>> attachmentList(int id) { header(id); return attachments.list(SCREEN, id); }

    public SaleEngrAttachments.Download download(int id, int attachmentId) { header(id); return attachments.download(SCREEN, id, attachmentId); }
}
