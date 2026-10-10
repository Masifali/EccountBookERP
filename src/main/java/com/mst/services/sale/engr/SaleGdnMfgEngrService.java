package com.mst.services.sale.engr;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.server.ResponseStatusException;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.*;
import java.util.concurrent.ConcurrentHashMap;

import static com.mst.services.sale.engr.SaleEngrSupport.*;

/**
 * Screen 844 "frmGoodsDispatchNotesEngr" = Architecture.WinApp.Mfg.frmGoodsDispatchNotesEngr (Sale Engr, module 134, document type 1659).
 *
 * Desktop map (Architecture.WinApp.Mfg/frmGoodsDispatchNotesEngr.cs, logic before InitializeComponent :3177):
 *   ctor :288, InitializeComponentCustom :319, InitializeComponentMethod :392, GenerateCode :467, BindTransporterCombo :498, TransporterAcFill :538,
 *   DeliveryTerm :571, BindVehicleTypeCombo :589, VehicleTypefill :601, BindVehicleNoCombo :633, FillVehiclesNo :653, dtForGridComboFill :698,
 *   BindPendingCustomer :719, BindPendingOrders :773, BindCustomerCity :800, grdSettings :821, grd_CellUpdated :909, AvailableStockUpdateInGrid :1026,
 *   grd_ColumnButtonClick :1090, DeleteDetailRow :1114, AddDetailRow :1168, FillGrdPendingOrders :1187, grdPendingOrdersSetting :1224,
 *   grdPendingOrders_ColumnButtonClick :1260, FormValidation :1283, Reset :1324, btnRefresh_Click :1366, Insert :1399, ReadById :1580,
 *   btnDelete_Click :1635, btnUpdate_Click :1674, HistoryComboBind :1694, cmbDateTypeHistory_ValueChanged :1765, FillHistory :1814,
 *   HistoryGridSettings :1928, GetDetailGrdByHeadId :2084, GenerateCustomerSlip :2238, combdeliverytrm_TextChanged :2356, comsupplier_ValueChanged :2468,
 *   BindGridData :2481, LoadDataDetailfromDeliveryOrder :2538, KeyDown :2582, grd_KeyDown :2748, AutoUpdatedRecords :2933, btnRecordsUpdate_Click :3111.
 *
 * BLL/DAL chain (Architecture.BLL.Inventory.InvGdn.Save (0575) -> Architecture.DAL.Inventory.InvGdn.SetData (0428), one transaction):
 *   Sp_InvGdn_Insert / Sp_InvGdn_Update (EntryUser = 0 on update, ModifyUser = 0 on insert, ActionId 1 / 2)
 *   Sp_InvGdnDetail_Insert per row (live rows first, then the removed rows with ActionTypeId 3; LineId 1..n)
 *   DMS attachments ; Sp_InventoryTransactions_GetALLMethod ;
 *   ERP feature 5 on (document type 1659 is a "valid" type, no stock reference on the rows): CommonServices.FIFOImplemention per row
 *   (USP_GetStockByFifoMethod, [USP_InventoryQtyReverseAndDeleteByReferenceId on update], USP_InventoryStockEvalautionDetail_Insert);
 *   feature 5 off: Sp_InventoryStockEvalautionDetail_Update ; USP_InventoryValidation per row ; usp_StockInTransitUpdate_VoucherInsertFromGdnOrForwarding
 *   (document type 1659 is not an order-complete type and carries no reserved posting).
 *
 * Differences from the module-83 GoodsDispatchNotes_Engr (screen 539, document type 1612): this form dispatches a Delivery Order (type 1657) of
 * one customer, rows carry Modal Description / Casting Type / Production Stage / City, the quantity is saved as qty and weight alike, and stock
 * is costed by FIFO when ERP feature 5 is on.
 */
@Service
public class SaleGdnMfgEngrService {
    public static final String SCREEN = "frmGoodsDispatchNotesEngr";
    public static final String DO_SCREEN = "frmDeliveryOrderEngr";
    public static final int DOC_TYPE = 1659;
    public static final int DO_DOC_TYPE = 1657;

    private final SaleEngrSupport sup;
    private final SaleEngrAttachments attachments;
    private final Map<String, List<Object[]>> paramCache = new ConcurrentHashMap<>();

    public SaleGdnMfgEngrService(SaleEngrSupport sup, SaleEngrAttachments attachments) {
        this.sup = sup; this.attachments = attachments;
    }

    // ------------------------------------------------------------------ Load (InitializeComponentMethod)

    public Map<String, Object> initial() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("rights", sup.rights(SCREEN));
        m.put("nextNo", nextNo());                                        // GenerateCode -> InvGdn.GenerateInvGdnCode (1659)
        m.put("vehicleTypes", vehicleTypes());                            // BindVehicleTypeCombo
        m.put("vehicleNos", vehicleNos());                                // BindVehicleNoCombo
        m.put("transporters", transporters());                            // BindTransporterCombo
        m.putAll(gridLists());                                            // dtForGridComboFill
        m.put("pending", pendingOrders());                                // FillGrdPendingOrders
        int ac = sup.configInt("FreightOutwardAc");
        m.put("outwardFreightAccountId", ac);
        int days = sup.configInt("DefaultDaysToLessFromHistoryFromDate");
        m.put("historyDays", days > 0 ? days : 3);
        m.put("fyStart", fyStart());
        return m;
    }

    /** btnRefresh_Click: vehicle types, vehicle numbers, transporters, then dtForGridComboFill. */
    public Map<String, Object> lists() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("vehicleTypes", vehicleTypes());
        m.put("vehicleNos", vehicleNos());
        m.put("transporters", transporters());
        m.putAll(gridLists());
        return m;
    }

    /** Reset tail: FillGrdPendingOrders + GenerateCode. */
    public Map<String, Object> resetData() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("pending", pendingOrders());
        m.put("nextNo", nextNo());
        return m;
    }

    public int nextNo() {
        List<Map<String, Object>> r = sup.rows("Sp_InvGdn_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "DocumentTypeId", DOC_TYPE,
                "FinancialYearId", idOrNull(sup.fy()), "BranchesId", idOrNull(sup.branch()), "Activity", "GenerateInvGdnCode");
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    /** VehicleType.GetAll: Sp_VehicleType_GetAllMethod (the desktop builds an Activity parameter but never sends it). */
    public List<Map<String, Object>> vehicleTypes() { return sup.rows("Sp_VehicleType_GetAllMethod"); }

    /** DefineVehicleWeight.ReadAll: USp_DefineVehicleWeight_FormHistory (FinancialYearId is not set by the form, so it is not sent). */
    public List<Map<String, Object>> vehicleNos() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("USp_DefineVehicleWeight_FormHistory", "OrganizationId", sup.org(), "CompanyId", sup.company()))
            out.add(row("Id", toInt(r.get("Id")), "VehicleNo", r.get("VehicleNo")));
        return out;
    }

    /**
     * BindTransporterCombo: with sub-ledger accounting (ERP feature 4) the vendors of USP_GetVendorsAndCustomersForTransporter
     * (Id = GlAccountId, AccountTitle = CompanyName, AccountCode = PartyCode, SupplierCustomerId = the party id), otherwise the COA allocation list
     * without account types 2, 15 and 11.
     */
    public List<Map<String, Object>> transporters() {
        List<Map<String, Object>> out = new ArrayList<>();
        if (sup.erpFeature(4)) {
            for (Map<String, Object> r : sup.rows("USP_GetVendorsAndCustomersForTransporter", "OrganizationId", sup.org(), "CompanyId", sup.company()))
                out.add(row("Id", toInt(r.get("GlAccountId")), "AccountTitle", r.get("CompanyName"), "AccountCode", r.get("PartyCode"), "SupplierCustomerId", toInt(r.get("Id"))));
            return out;
        }
        for (Map<String, Object> r : sup.rows("Sp_COAAllocation_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "UserId", idOrNull(sup.userId()), "Activity", "COAAllocationSearch")) {
            int t = toInt(r.get("AccountTypeId"));
            if (t != 2 && t != 15 && t != 11) out.add(row("Id", toInt(r.get("Id")), "AccountTitle", r.get("AccountTitle"), "AccountCode", r.get("AccountCode"), "SupplierCustomerId", 0));
        }
        return out;
    }

    /** dtForGridComboFill: warehouses (branch only with ERP feature 11), casting types (lookup type 1), production stages (type 3), cities. */
    public Map<String, Object> gridLists() {
        Map<String, Object> m = new LinkedHashMap<>();
        int branch = sup.erpFeature(11) ? sup.branch() : 0;
        m.put("warehouses", sup.rows("USP_GetWarehousesAllocatedToBranch", "OrganizationId", sup.org(), "CompanyId", sup.company(), "BranchId", idOrNull(branch)));
        List<Map<String, Object>> lk = sup.rows("[Mfg].[USP_LookUps_GetAllMethod]", "Activity", "FormHistory");
        List<Map<String, Object>> cast = new ArrayList<>(), stage = new ArrayList<>();
        for (Map<String, Object> r : lk) {
            int t = toInt(r.get("LookUpTypeId"));
            if (t == 1) cast.add(row("Id", toInt(r.get("Id")), "LookupName", r.get("LookupName")));
            if (t == 3) stage.add(row("Id", toInt(r.get("Id")), "LookupName", r.get("LookupName")));
        }
        m.put("castingTypes", cast);
        m.put("stages", stage);
        m.put("cities", sup.rows("SP_City_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "MethodType", "GetAll"));
        return m;
    }

    /**
     * FillGrdPendingOrders: InvDeliveryOrder.OutstandingDeliveryorderForDispatch(org, company, branch, 1657); one grid row per delivery order,
     * ItemQty = the sum of DoQty over that order's rows.
     */
    public List<Map<String, Object>> pendingOrders() {
        List<Map<String, Object>> src = sup.rows("USP_OutstandingDeliveryorderForDispatch", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "BranchesId", sup.branch(), "DocumentTypeId", DO_DOC_TYPE);
        Map<Integer, Double> qty = new LinkedHashMap<>();
        for (Map<String, Object> r : src) qty.merge(toInt(r.get("Id")), toDouble(r.get("DoQty")), Double::sum);
        Set<Integer> seen = new HashSet<>();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : src) {
            int id = toInt(r.get("Id"));
            if (!seen.add(id)) continue;
            out.add(row("Id", id, "DocumentTypeId", r.get("DocumentTypeId"), "SaleOrderId", r.get("SaleOrderId"), "OrderTypeId", r.get("OrderTypeId"), "OrderType", r.get("OrderType"),
                    "DocNo", r.get("DocNo"), "DocDate", r.get("DocDate"), "CustomerName", r.get("CustomerName"), "VehicleNo", r.get("VehicleNo"), "ItemQty", qty.get(id),
                    "ItemWeight", r.get("ItemWeight"), "CastingWeight", r.get("CastingWeight"), "RemarksHeader", r.get("RemarksHeader"), "NoOfAttachments", r.get("NoOfAttachments")));
        }
        return out;
    }

    // ------------------------------------------------------------------ Load of a delivery order into the form

    /** BindPendingCustomer: InvDeliveryOrder.GetPendingCustomerAndOrderForGDN(org, company, DoId, 0), one row per customer. */
    public List<Map<String, Object>> pendingCustomers(int doId) {
        List<Map<String, Object>> out = new ArrayList<>();
        Set<Integer> seen = new HashSet<>();
        for (Map<String, Object> r : sup.rows("USP_GetPendingCustomerAndOrderForGDN", "OrganizationId", sup.org(), "CompanyId", sup.company(), "DoId", doId)) {
            int c = toInt(r.get("SupplierCustomerId"));
            if (!seen.add(c)) continue;
            out.add(row("SupplierCustomerId", c, "CustomerName", str(r.get("CustomerName")), "InvDeliveryOrderId", toInt(r.get("InvDeliveryOrderId")), "DoNo", toInt(r.get("DoNo")),
                    "GlAccountId", toInt(r.get("GlAccountId")), "CityName", str(r.get("CityName")), "MobileNo", str(r.get("MobilePersonal"))));
        }
        return out;
    }

    /** InvDeliveryOrder.GetDeliveryOrderDataByPartyDoAndOrderId -> USP_GetDeliveryOrderDataByPartyDoAndOrderId. */
    public List<Map<String, Object>> deliveryOrderData(int customerId, int doId) {
        return sup.rows("USP_GetDeliveryOrderDataByPartyDoAndOrderId", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "SupplierCustomerId", customerId, "DoId", doId);
    }

    public static class StockReq {
        public int itemId, warehouseId, castingTypeId, variantId, packUomId;
    }

    /**
     * grd_CellUpdated / AvailableStockUpdateInGrid: InventoryStockEvalautionDetail.InventoryTransactions_GetCurrStockByItem_Engr
     * ([Mfg].[USP_InventoryTransactions_GetCurrStockByItem], AvailableStock); a stock above zero is shown, anything else is 0.
     */
    public List<Double> stocks(String docDate, List<StockReq> rows) {
        LocalDate d = parseDate(docDate);
        List<Double> out = new ArrayList<>();
        for (StockReq q : (rows == null ? List.<StockReq>of() : rows)) {
            if (q.itemId > 0 || q.warehouseId > 0) {
                double cur = 0.0;
                List<Map<String, Object>> r = sup.rows("[Mfg].[USP_InventoryTransactions_GetCurrStockByItem]", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                        "ItemId", q.itemId, "DocDate", d, "WareHouseId", q.warehouseId, "CastingTypeId", idOrNull(q.castingTypeId), "VariantId", idOrNull(q.variantId),
                        "ItemUomId", idOrNull(q.packUomId));
                if (!r.isEmpty()) cur = toDouble(r.get(0).get("AvailableStock"));
                out.add(cur > 0.0 ? cur : 0.0);
            } else out.add(0.0);
        }
        return out;
    }

    /** ItemAttributeVarient.GetAllForCombo (the F1 pop-up of the Modal Description cell). */
    public List<Map<String, Object>> variants(int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("USP_ItemAttributeVarient_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "ItemId", itemId, "Activity", "GetAllForCombo"))
            out.add(row("VariantId", r.get("ItemAttributeVarientId"), "VariantDescription", r.get("ItemAttribute") != null ? r.get("ItemAttribute") : r.get("VarientDescription")));
        return out;
    }

    // ------------------------------------------------------------------ History (HistoryComboBind / FillHistory)

    /** HistoryComboBind: InvGdn.GetDataForDropDownFromGdn(org, company, DocumentTypeIds "1659", Activity "Supplier"). */
    public List<Map<String, Object>> historyCustomers() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("USP_GetDataForDropDownFromGdn", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "Supplier", "DocumentTypeIds", String.valueOf(DOC_TYPE)))
            out.add(row("Id", r.get("Id"), "CustomerName", r.get("ReferenceName")));
        return out;
    }

    /** InvGdn.GetHisoty -> Sp_InvGdn_GetAllMethod 'GDNFormHistory'. The date pair follows the Doc / Entry / Modify / Approved radio. */
    public List<Map<String, Object>> history(String dateType, String fromDate, String toDate, int fromNo, int toNo, int customerId) {
        boolean all = Boolean.TRUE.equals(sup.rights(SCREEN).get("viewAll"));
        LocalDate from = parseDate(fromDate), to = parseDate(toDate);
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("DocumentTypeId", DOC_TYPE);
        p.put("FinancialYearId", idOrNull(sup.fy()));
        p.put("BranchesId", idOrNull(sup.branch()));
        String d = dateType == null ? "document" : dateType;
        String fk, tk;
        switch (d) {
            case "entry" -> { fk = "EntryFromDate"; tk = "EntryToDate"; }
            case "modify" -> { fk = "ModifyFromDate"; tk = "ModifyToDate"; }
            case "approved" -> { fk = "ApprovedFromDate"; tk = "ApprovedToDate"; }
            default -> { fk = "FromDate"; tk = "ToDate"; }
        }
        if (from != null) p.put(fk, from);
        if (to != null) p.put(tk, to);
        if (fromNo != 0) p.put("DocNoFrom", (double) fromNo);
        if (toNo != 0) p.put("DocNoTo", (double) toNo);
        if (customerId != 0) p.put("SupplierCustomerId", customerId);
        p.put("CanViewAllRecord", all);
        if (!all) p.put("EntryUser", sup.userId());
        p.put("Activity", "GDNFormHistory");
        return DesktopProc.rows(sup.jdbc(), "Sp_InvGdn_GetAllMethod", p);
    }

    // ------------------------------------------------------------------ ReadById (InvGdn.GetByID)

    private Map<String, Object> header(int id) {
        List<Map<String, Object>> r = sup.rows("Sp_InvGdn_GetAllMethod", "Id", id, "Activity", "GetById");
        UserAccount u = sup.user();
        if (r.isEmpty() || toInt(r.get(0).get("OrganizationId")) != u.getOrganizationId() || toInt(r.get(0).get("CompanyId")) != u.getCompanyId()
                || toInt(r.get(0).get("DocumentTypeId")) != DOC_TYPE)
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Goods dispatch note not found in this company");
        return r.get(0);
    }

    /** DAL.InvGdn.GetDate: document type 1659 is an "included" type, so the detail is Sp_InvGdn_GetAllMethod 'GetGDNDetailByGdnId' (InvGdnMainId). */
    private List<Map<String, Object>> details(int id) {
        return sup.rows("Sp_InvGdn_GetAllMethod", "InvGdnMainId", id, "Activity", "GetGDNDetailByGdnId");
    }

    public Map<String, Object> record(int id) {
        Map<String, Object> h = header(id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("head", h);
        out.put("lines", details(id));
        out.put("attachments", attachments.list(SCREEN, id));
        return out;
    }

    /** History selection (GetDetailGrdByHeadId): the detail of the highlighted row. */
    public List<Map<String, Object>> detailOf(int id) { header(id); return details(id); }

    // ------------------------------------------------------------------ Save (Insert :1399)

    public static class Line {
        public int id;
        public int orderId;
        public int orderDetailId;
        public int doOrderDetailId;
        public int warehouseId;
        public int itemId;
        public int packUomId;
        public int variantId;
        public int castingTypeId;
        public int productionStageId;
        public String remarks;
        public double itemQty;
        public int cityId;
    }

    public static class Request {
        public int id;
        public String docDate;
        public String docNo;
        public int customerId;
        public int deliveryOrderId;
        public String referenceNo;
        public String deliveryTerm;
        public int transporterId;
        public String carriage;
        public String gpNo;
        public String gpDate;
        public int vehicleTypeId;
        public String vehicleType;
        public String vehicleNo;
        public String driverName;
        public String driverCellNo;
        public String driverCnic;
        public String remarks;
        public List<Line> lines = new ArrayList<>();
        public List<Line> removed = new ArrayList<>();
        public SaleEngrAttachments.Change attachments;
    }

    @Transactional
    public Map<String, Object> save(Request r) {
        UserAccount u = sup.user();
        List<Line> lines = r.lines == null ? List.of() : r.lines;
        if (lines.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        formValidation(r);
        double carriage = toDouble(r.carriage);
        if (carriage > 0.0) {
            if (r.transporterId == 0) throw new IllegalArgumentException("Transporter Account field Required");
            if (r.transporterId == customerGlAccount(r.customerId)) throw new IllegalArgumentException("Transporter Account can not be same as Customer Please check");
        }
        final boolean update = r.id > 0;
        Map<String, Object> old = null;
        Set<Integer> savedIds = new HashSet<>();
        if (update) {
            old = header(r.id);
            for (Map<String, Object> d : details(r.id)) savedIds.add(toInt(d.get("Id")));
        }
        // per-row checks in the desktop's order (raised while the detail list is built)
        int rowNo = 0;
        for (Line l : lines) {
            rowNo++;
            if (l.orderId == 0) throw new IllegalArgumentException("Order Required in Detail Grid And row No: " + rowNo);
            if (l.warehouseId == 0) throw new IllegalArgumentException("Warehouse Required in Detail Grid And row No: " + rowNo);
            if (l.itemId == 0) throw new IllegalArgumentException("Item Required in Detail Grid And row No: " + rowNo);
            if (l.packUomId == 0) throw new IllegalArgumentException("Uom Required in Detail Grid And row No: " + rowNo);
            if (l.castingTypeId == 0) throw new IllegalArgumentException("CastingType Required in Detail Grid And row No: " + rowNo);
            if (l.productionStageId == 0) throw new IllegalArgumentException("ProductionStage Required in Detail Grid And row No: " + rowNo);
            if (l.itemQty == 0.0) throw new IllegalArgumentException("Qty Required in Detail Grid And row No: " + rowNo);
            if (update && l.id > 0 && !savedIds.contains(l.id)) throw new IllegalArgumentException("Invalid detail row for this goods dispatch note");
        }
        List<Line> removed = update && r.removed != null ? r.removed : List.of();
        for (Line l : removed)
            if (l.id <= 0 || !savedIds.contains(l.id)) throw new IllegalArgumentException("Deleted detail row does not belong to this goods dispatch note");

        Timestamp now = now();
        java.sql.Date docDate = java.sql.Date.valueOf(parseDate(r.docDate) == null ? LocalDate.now() : parseDate(r.docDate));
        LocalDate gp = parseDate(r.gpDate);
        Map<String, Object> h = new LinkedHashMap<>();
        for (String k : List.of("IsApproved", "AddWages", "ShortWeightDeductionApply", "IsStockReserved")) h.put(k, false);
        h.put("Id", update ? r.id : 0);
        h.put("DocumentTypeId", DOC_TYPE);
        h.put("DocDate", docDate);
        h.put("DocNo", toInt(r.docNo));
        h.put("SupplierCustomerId", r.customerId);
        h.put("RemarksHeader", text(r.remarks));
        h.put("OrderTypeId", 1);
        h.put("OrderId", r.deliveryOrderId);
        h.put("ReferenceDocNo", text(r.referenceNo));
        h.put("DeliveryTerm", text(r.deliveryTerm));
        h.put("TransporterId", r.transporterId);
        h.put("CarriageAmount", carriage);
        h.put("GpNo", toInt(r.gpNo));
        h.put("GpDate", java.sql.Date.valueOf(gp == null ? LocalDate.now() : gp));
        h.put("VehicleTypeId", r.vehicleTypeId);
        h.put("VehicleType", text(r.vehicleType));
        h.put("VehicleNo", text(r.vehicleNo));
        h.put("DriverName", text(r.driverName));
        h.put("DriverCellNo", text(r.driverCellNo));
        h.put("DriverCNIC", text(r.driverCnic));
        h.put("EntryDate", now);
        h.put("ModifyDate", now);
        h.put("PostDate", now);
        h.put("ReturnableDate", now);
        h.put("EntryUser", update ? 0 : u.getId());            // BLL.Save: EntryUser = 0 on update
        h.put("ModifyUser", update ? u.getId() : 0);           // BLL.Save: ModifyUser = 0 on insert
        h.put("OrganizationId", u.getOrganizationId());
        h.put("CompanyId", u.getCompanyId());
        h.put("BranchesId", u.getBranchesId());
        h.put("ProjectsId", u.getBranchesId());                // the desktop stores the branch in ProjectsId
        h.put("FinancialYearId", sup.fy());
        h.put("ActionId", update ? 2 : 1);
        h.put("ScreenName", SCREEN);
        for (String k : List.of("PartyWeight", "FactoryWeight", "OtherCharges")) h.put(k, 0.0);
        for (String k : List.of("PostUser", "OutwardGatePassId", "ReferencePartyId", "StockPartyId", "BillCalculateTypeId", "TransporterSupCustId", "RequestedByLookUpId",
                "AutoUpdateId", "AdvanceDeliveryOrderId")) h.put(k, 0);
        SaleEngrAttachments.Change ch = r.attachments;
        List<String> names = new ArrayList<>();
        if (update) {
            Set<Integer> gone = new HashSet<>(ch != null && ch.removeAttachmentIds != null ? ch.removeAttachmentIds : List.of());
            for (Map<String, Object> a : attachments.list(SCREEN, r.id)) if (!gone.contains(toInt(a.get("Id")))) names.add(str(a.get("Attachment")));
        }
        if (ch != null && ch.files != null) for (var f : ch.files) names.add(f.name);
        h.put("AttachmentsValues", String.join(",", names));
        h.put("CustomAttachmentsValues", String.join(",", names));

        int num = sup.setProcMap(update ? "Sp_InvGdn_Update" : "Sp_InvGdn_Insert", h);
        int id = num > 0 ? num : r.id;
        if (id <= 0) throw new IllegalStateException("The goods dispatch note could not be saved.");

        // DAL SetData: the live rows, then the removed rows (ActionTypeId 3); LineId runs over both; the new detail id is read back
        List<Map<String, Object>> dets = new ArrayList<>();
        for (Line l : lines) dets.add(detail(id, r.deliveryOrderId, l, update ? l.id : 0, (update ? l.id : 0) <= 0 ? 1 : 2));
        for (Line l : removed) dets.add(detail(id, r.deliveryOrderId, l, l.id, 3));
        int lineId = 0;
        for (Map<String, Object> d : dets) {
            d.put("LineId", ++lineId);
            d.put("Id", sup.setProcMap("Sp_InvGdnDetail_Insert", d));
        }
        attachments.apply(SCREEN, DOC_TYPE, id, r.customerId, ch);

        // InventoryTransactions model through Sp_InventoryTransactions_GetALLMethod
        sup.setProcMap("Sp_InventoryTransactions_GetALLMethod", full("Sp_InventoryTransactions_GetALLMethod",
                row("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "RefDocumentTypeId", DOC_TYPE, "RefDocIdNo", id)));
        if (sup.erpFeature(5)) fifo(u, id, update, h, docDate, dets);
        else sup.setProcMap("Sp_InventoryStockEvalautionDetail_Update", full("Sp_InventoryStockEvalautionDetail_Update",
                row("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "RefDocumentTypeId", DOC_TYPE, "RefDocIdNo", id)));
        for (Map<String, Object> d : dets) {
            Map<String, Object> v = new LinkedHashMap<>();
            v.put("OrganizationId", u.getOrganizationId()); v.put("CompanyId", u.getCompanyId()); v.put("DocumentTypeId", DOC_TYPE); v.put("DocDate", docDate);
            v.put("ItemId", d.get("ItemId")); v.put("WarehouseId", d.get("WarehouseId")); v.put("JobLotId", 0); v.put("InvPackingTypeId", 0); v.put("PackUomId", d.get("ItemUomId"));
            v.put("NetWeight", d.get("StockWeight")); v.put("RefDocumentTypeId", 0); v.put("RefDocNoId", 0); v.put("RefDocSubIdNo", 0); v.put("ItemConditionId", 0);
            DesktopProc.scalar(sup.jdbc(), "USP_InventoryValidation", v);
        }
        DesktopProc.scalar(sup.jdbc(), "usp_StockInTransitUpdate_VoucherInsertFromGdnOrForwarding",
                row("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", DOC_TYPE, "Id", id));

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id);
        out.put("docNo", toInt(r.docNo));
        out.put("updated", update);
        out.put("message", update ? "Record Update Successfully [" + toInt(r.docNo) + "] " : "Record Saved Successfully [" + toInt(r.docNo) + "] ");
        return out;
    }

    /** FormValidation (:1283): the first failing message, word for word. */
    private void formValidation(Request r) {
        String no = text(r.docNo);
        if (no.isEmpty() || "0".equals(no)) throw new Warning("DocNo Field is Required");
        if (r.customerId <= 0) throw new Warning("CustomerName Field is Required");
        if (r.deliveryOrderId <= 0) throw new Warning("Delivery Order No Field is Required");
        double f = toDouble(r.carriage);
        if (f != 0.0 && r.transporterId == 0) throw new Warning("Carriage Ac is Required when Carriage amount is greater than Zero");
        if (f == 0.0 && r.transporterId != 0) throw new Warning("Carriage Amount is Required when Carriage Ac is Selected");
        if (text(r.deliveryTerm).isEmpty()) throw new Warning("Delivery Term Field is Required");
    }

    /** The Gl account of the customer (the combo's GlAccountId cell). */
    private int customerGlAccount(int customerId) {
        if (customerId <= 0) return 0;
        List<Map<String, Object>> r = sup.jdbc().queryForList("SELECT GlAccountId FROM dbo.SupplierCustomer WHERE Id = ? AND OrganizationId = ? AND CompanyId = ?",
                customerId, sup.org(), sup.company());
        return r.isEmpty() ? 0 : toInt(r.get(0).get("GlAccountId"));
    }

    /** InvGdnDetail as the desktop fills it (:1477): GrossWeight = NetBillWeight = StockWeight = ItemQty = the grid's Qty. */
    private Map<String, Object> detail(int headId, int doId, Line l, int detailId, int action) {
        Map<String, Object> d = new LinkedHashMap<>();
        for (String k : List.of("AdLsWeight", "EBWPerUnit", "EBWTotal", "EbUnitStock", "EbTotalStock", "WtCut", "WtCutTotal", "ShortWeight", "ItemRate", "ItemAmount")) d.put(k, 0.0);
        d.put("GrossWeight", l.itemQty); d.put("NetBillWeight", l.itemQty); d.put("StockWeight", l.itemQty); d.put("ItemQty", l.itemQty);
        d.put("IsAssetItem", false);
        for (String k : List.of("DeliveryScheduleId", "PackingTypeId", "RefPartyId", "RefDocumentTypeId", "RefDocIdNo", "RefDocSubIdNo", "ItemConditionId", "GrnId", "GrnDetailId",
                "GrnDocumentTypeId", "DepartmentId", "WbTicketId", "RateUomId", "CropYearId", "BrandItemId", "RackId", "SecondaryUomId", "JobLotId", "DeliveryTypeId", "AssetId",
                "WareHouseToId", "CostCenterId", "JobOrderId", "JobOrderDocumentTypeId")) d.put(k, 0);
        d.put("SecondaryUomQty", BigDecimal.ZERO);
        d.put("Id", detailId);
        d.put("InvGdnId", headId);
        d.put("SaleOrderId", l.orderId);
        d.put("SaleOrderDetailId", l.orderDetailId);
        d.put("InvDeliveryOrderId", doId);                   // the DO combo value, sent on every row
        d.put("InvDeliveryOrderDetailId", l.doOrderDetailId);
        d.put("WarehouseId", l.warehouseId);
        d.put("ItemId", l.itemId);
        d.put("ItemUomId", l.packUomId);
        d.put("ItemVariantId", l.variantId);
        d.put("CastingTypeId", l.castingTypeId);
        d.put("ProductionStageId", l.productionStageId);
        d.put("CityId", l.cityId);
        d.put("OrderTypeId", 1);
        d.put("ActionTypeId", action);
        d.put("CommentsDetail", l.remarks == null ? "" : l.remarks);
        return d;
    }

    // ------------------------------------------------------------------ DAL SetData: FIFO costing (CommonServices.FIFOImplemention)

    private static String cs(double v) {
        if (v == Math.rint(v) && Math.abs(v) < 1e15) return String.valueOf((long) v);
        return BigDecimal.valueOf(v).stripTrailingZeros().toPlainString();
    }

    private double equivalent(int itemId, int scheduleId) {
        List<Map<String, Object>> r = sup.rows("Sp_Item_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "ItemId", itemId,
                "ScheduleId", scheduleId, "Activity", "GetEqvilentByItemIdAndUomScheduleId");
        return r.isEmpty() ? 0.0 : toDouble(r.get(0).get("Equivalent"));
    }

    private void fifo(UserAccount u, int id, boolean update, Map<String, Object> h, java.sql.Date docDate, List<Map<String, Object>> dets) {
        Map<Integer, String> glItems = new HashMap<>();
        for (Map<String, Object> r : sup.rows("Sp_Item_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetItemGlIdsandItemName"))
            glItems.putIfAbsent(toInt(r.get("Id")), str(r.get("ItemName")));
        List<Map<String, Object>> all = new ArrayList<>();
        for (Map<String, Object> d : dets) {
            int itemId = toInt(d.get("ItemId"));
            if (!glItems.containsKey(itemId)) continue;                 // only items listed by GetItemGlIdsandItemName are costed
            String itemName = glItems.get(itemId);
            double needQty = toDouble(d.get("ItemQty")), needWeight = toDouble(d.get("StockWeight"));
            // CommonServices.FIFOImplemention: @FIFOXML carries what the earlier rows of this save already took
            Map<String, Object> p = new LinkedHashMap<>();
            p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("ItemId", itemId); p.put("DocDate", docDate);
            if (toInt(d.get("ItemUomId")) != 0) p.put("PackUomId", toInt(d.get("ItemUomId")));
            if (toInt(d.get("WarehouseId")) != 0) p.put("WarehouseId", toInt(d.get("WarehouseId")));
            if (update) { p.put("DocumentTypeId", DOC_TYPE); p.put("Id", id); }
            if (!all.isEmpty()) p.put("FIFOXML", fifoXml(all));
            List<Map<String, Object>> stocks = DesktopProc.rows(sup.jdbc(), "USP_GetStockByFifoMethod", p);
            if (stocks.isEmpty()) throw new IllegalStateException("Stock Not Found this Item " + itemName + " against FIFO Method .....");
            double available = 0.0;
            for (Map<String, Object> s : stocks) available += toDouble(s.get("NetBalWeight"));
            double rounded = Math.abs(available) < 1e16 ? Math.rint(available * 100d) / 100d : available;   // .NET Math.Round(x, 2): midpoint to even
            if (!(needWeight <= rounded))
                throw new IllegalStateException("Weight available is " + cs(available) + " and row Weight is " + cs(needWeight) + " this item " + itemName + " against FIFO....");
            double usedQty = 0.0, usedWeight = 0.0;
            for (Map<String, Object> s : stocks) {
                double avg = toDouble(s.get("AvgRate"));
                int rateUom = toInt(s.get("RateUomId"));
                if (avg <= 0.0) throw new IllegalStateException("Rate Not Found this Item " + itemName + " against FIFO Method");
                if (rateUom == 0) throw new IllegalStateException("RateUomId not found  this " + itemName + " against FIFO Method");
                double layerWeight = toDouble(s.get("NetBalWeight")), layerQty = toDouble(s.get("NetBalQty"));
                double eq = equivalent(itemId, rateUom);
                if (eq == 0.0) throw new IllegalStateException("RateUom Not Found");
                double qtyOut, wOut;
                if (layerWeight <= needWeight - usedWeight) { qtyOut = layerQty; wOut = layerWeight; }
                else { qtyOut = needQty - usedQty; wOut = needWeight - usedWeight; }
                usedQty += qtyOut; usedWeight += wOut;
                Map<String, Object> x = new LinkedHashMap<>();
                x.put("Id", toInt(s.get("Id")));
                x.put("LineId", toInt(d.get("LineId")));
                x.put("ItemId", itemId);
                x.put("WarehouseId", toInt(d.get("WarehouseId")));
                x.put("RateUom", rateUom);
                x.put("JobLotId", 0);
                x.put("InvPackingTypeId", 0);
                x.put("ItemUom", toInt(d.get("ItemUomId")));
                x.put("RefRefDocumentTypeId", toInt(s.get("RefDocumentTypeId")));
                x.put("RefRefDocIdNo", toInt(s.get("RefDocIdNo")));
                x.put("RefRefDocSubIdNo", toInt(s.get("RefDocSubIdNo")));
                x.put("QtyOut", qtyOut);
                x.put("BillWeightOut", wOut);
                x.put("StockWeightOut", wOut);
                x.put("CgsRate", avg * eq);
                x.put("CgsAmount", wOut / eq * (avg * eq));
                x.put("CityId", toInt(d.get("CityId")));
                all.add(x);
                if (needWeight == usedWeight) break;
            }
        }
        if (all.isEmpty()) return;
        if (update) sup.setProcMap("[dbo].[USP_InventoryQtyReverseAndDeleteByReferenceId]", row("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "RefDocumentTypeId", DOC_TYPE, "RefDocIdNo", id));
        for (Map<String, Object> x : all) {
            x.put("OrganizationId", u.getOrganizationId());
            x.put("CompanyId", u.getCompanyId());
            x.put("DocDate", docDate);
            x.put("DocCodeNo", h.get("DocNo"));
            x.put("VehicleNo", h.get("VehicleNo"));
            x.put("GpNoDcNo", h.get("GpNo"));
            x.put("SupplierCustomerId", h.get("SupplierCustomerId"));
            x.put("BranchesId", h.get("BranchesId"));
            x.put("OtherDocumentTypeId", DOC_TYPE);
            x.put("EntryUser", h.get("EntryUser"));
            x.put("ModifyUser", h.get("ModifyUser"));
            x.put("CalcType", "Weight");
            for (Map<String, Object> d : dets)
                if (toInt(d.get("LineId")) == toInt(x.get("LineId")) && toInt(x.get("LineId")) > 0) { x.put("OtherDocNoId", id); x.put("OtherSubDocNoId", toInt(d.get("Id"))); break; }
            sup.setProcMap("USP_InventoryStockEvalautionDetail_Insert", full("USP_InventoryStockEvalautionDetail_Insert", x));
        }
    }

    private static String fifoXml(List<Map<String, Object>> reserved) {
        StringBuilder xml = new StringBuilder("<ArrayOfFIFOStockEvaluation>");
        for (Map<String, Object> a : reserved)
            xml.append("<FIFOStockEvaluation>")
                    .append("<RefDocumentTypeId>").append(toInt(a.get("RefRefDocumentTypeId"))).append("</RefDocumentTypeId>")
                    .append("<RefDocIdNo>").append(toInt(a.get("RefRefDocIdNo"))).append("</RefDocIdNo>")
                    .append("<RefDocSubIdNo>").append(toInt(a.get("RefRefDocSubIdNo"))).append("</RefDocSubIdNo>")
                    .append("<ReserveQty>").append(cs(toDouble(a.get("QtyOut")))).append("</ReserveQty>")
                    .append("<ReserveWeight>").append(cs(toDouble(a.get("StockWeightOut")))).append("</ReserveWeight>")
                    .append("</FIFOStockEvaluation>");
        return xml.append("</ArrayOfFIFOStockEvaluation>").toString();
    }

    // ------------------------------------------------------------------ Delete (btnDelete_Click :1635)

    /** InvPurchaseInvoice.RemoveByID -> DAL.AccountandInventoryRemoveById -> Sp_InvoicesVouchersandStocksDelete (org, company, id, document type). */
    @Transactional
    public Map<String, Object> delete(int id) {
        if (id == 0) throw new IllegalArgumentException("Record Id not found for deletion...");
        header(id);
        sup.setProc("Sp_InvoicesVouchersandStocksDelete", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Id", id, "DocumentTypeId", DOC_TYPE);
        return row("message", "Deleted Successfully");
    }

    // ------------------------------------------------------------------ slip (GenerateCustomerSlip :2238) and attachments

    /** InvGdn.GdnRegister_Eng (USP_GdnRegister_Eng, org, company, branch, year, 1659, Id): rows behind the 1659_GdnSlip_Engr report. */
    public Map<String, Object> slipRows(int id) {
        List<Map<String, Object>> r = sup.rows("USP_GdnRegister_Eng", "OrganizationId", sup.org(), "CompanyId", sup.company(), "BranchesId", sup.branch(),
                "FinancialYearId", sup.fy(), "DocumentTypeId", DOC_TYPE, "Id", idOrNull(id));
        return row("rows", r.size());
    }

    private static String screenOf(int docType) { return docType == DO_DOC_TYPE ? DO_SCREEN : SCREEN; }

    /** grdHistory / grdPendingOrders NoOfAttachments link: CommonServices.GetNoofAttachmentsByRefDocumentTypeID(id, document type). */
    public List<Map<String, Object>> attachmentList(int docType, int id) {
        if (docType == DOC_TYPE) header(id);
        return attachments.list(screenOf(docType), id);
    }

    public SaleEngrAttachments.Download download(int docType, int id, int attachmentId) {
        if (docType == DOC_TYPE) header(id);
        return attachments.download(screenOf(docType), id, attachmentId);
    }

    // ------------------------------------------------------------------ helpers

    /**
     * GenericProvider.SetProc of a model with many properties: every procedure parameter that the caller did not give receives the
     * property default (0 / false; an unset string or date is left out).
     */
    private Map<String, Object> full(String proc, Map<String, Object> given) {
        String bare = proc.replace("[dbo].", "").replace("[", "").replace("]", "");
        List<Object[]> params = paramCache.computeIfAbsent(bare, p -> {
            List<Object[]> l = new ArrayList<>();
            for (Map<String, Object> r : sup.jdbc().queryForList("SELECT p.name, t.name AS tname FROM sys.parameters p JOIN sys.types t ON p.user_type_id = t.user_type_id "
                    + "WHERE p.object_id = OBJECT_ID(?) ORDER BY p.parameter_id", "dbo." + p))
                l.add(new Object[]{String.valueOf(r.get("name")).replace("@", ""), String.valueOf(r.get("tname")).toLowerCase()});
            return l;
        });
        Map<String, Object> out = new LinkedHashMap<>(given);
        for (Object[] p : params) {
            String n = (String) p[0], t = (String) p[1];
            boolean has = false;
            for (String k : out.keySet()) if (k.equalsIgnoreCase(n)) { has = true; break; }
            if (has) continue;
            switch (t) {
                case "bit" -> out.put(n, false);
                case "tinyint", "smallint", "int", "bigint" -> out.put(n, 0);
                case "float", "real", "decimal", "numeric", "money", "smallmoney" -> out.put(n, 0.0);
                default -> { }
            }
        }
        return out;
    }

    public String fyStart() {
        for (Map<String, Object> r : sup.rows("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", "OrganizationId", sup.org(), "CompanyId", sup.company()))
            if (toInt(r.get("Id")) == sup.fy()) return str(r.get("Start_Period"));
        return "";
    }

    private static LocalDate parseDate(String s) {
        if (s == null || s.isBlank()) return null;
        try { return LocalDate.parse(s.trim().substring(0, Math.min(10, s.trim().length()))); } catch (RuntimeException e) { return null; }
    }
}
