package com.mst.services.sale.steel;

import com.mst.models.UserAccount;
import com.mst.repositories.support.ProcExec;
import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.engr.SaleEngrSupport;
import org.springframework.dao.DataAccessException;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.server.ResponseStatusException;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.*;

import static com.mst.services.sale.engr.SaleEngrSupport.*;

/**
 * Screen 545 "OutwardGatePass_St" = Architecture.WinApp.Steel.Sale.OutwardGatePass_St (Sale Steel, module 84).
 *
 * Desktop map (OutwardGatePass_St.cs, 4623 lines; InitializeComponent 2313):
 *   btnupdate_Click :323, gpnofill :453, Status :480, WeighBridgeStatus :499, GpBasedOn :517, CityFill :536, vehicleTypefill :560,
 *   CustomerFill :576, WarehouseFill :592, ItemNameFill :608, gatepasstype :625, CmbOrderno_Leave :642, DifferenceWeight :791,
 *   CmbGPBasedOn_Leave :809, formvalidation :841, ReadById :930, btnsave_Click :1062, OutwardGatePass_Load :1193, gridhistory :1253,
 *   grdhistorysetting :1359, CustomerHistoryFill :1393, grdhistory_* :1482-1575, grdfrmfill :1599, grdfrmsetting :1635, grd_* :1660-1700,
 *   GetTicketNoandWeighBridgeWeight :1701, DeliveryOrderGridFill :1756, GridSetting :1792, SaleOrderGridFill :1832,
 *   GridSettingForSaleOrder :1868, Reset :1921, btnRefresh_Click :2000, KeyDown :2077, btnPrint_Click :2160, btnPrint291_Click :2176,
 *   WeightCalculation :2258.
 * Differences from the Sale Engr gate pass (screen 534): document type 1507 (reference types 1505 sale order / 1506 delivery order /
 * 1507 gate sale), the three fixed order types DeliveryOrder / SaleOrder / GateSale, the Steel (ST schema) delivery order and
 * sale order procedures, warehouse / item / pack unit / compared weight / container weight / difference weight / weight difference
 * remarks fields, no type serial number (cmbgptype_Leave_1 is empty), the two information grids come from
 * [ST].[USp_DeliveryOrderDetailForGatePassOutward], print is 1512-GatePassOutwardSlipAndRegisterSteel.rpt.
 */
@Service
public class SaleOutwardGatePassStService {
    public static final String SCREEN = "OutwardGatePass_St";
    public static final int DOC_TYPE = 1507;
    public static final int SO_DOC = 1505, DO_DOC = 1506;
    private static final String P_DO = "[ST].[USP_DeliveryOrder_GetAllMethod]";
    private static final String P_SUPP_SO = "[ST].[USP_GetSupplierBySaleOrderNo]";
    private static final String P_INFO = "[ST].[USp_DeliveryOrderDetailForGatePassOutward]";

    private final SaleEngrSupport sup;
    private final SaleSteelSupport db;
    private final SaleEngrAttachments attachments;

    public SaleOutwardGatePassStService(SaleEngrSupport sup, SaleSteelSupport db, SaleEngrAttachments attachments) {
        this.sup = sup; this.db = db; this.attachments = attachments;
    }

    // ------------------------------------------------------------------ Load (OutwardGatePass_Load :1193)

    public Map<String, Object> initial() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("rights", sup.rights(SCREEN));
        m.put("nextNo", nextGpNo());                                                   // gpnofill
        m.put("gatePassTypes", sup.rows("Sp_GatePassType_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "Outward"));   // gatepasstype
        m.put("customers", customers());                                               // CustomerFill
        m.put("vehicleTypes", sup.rows("Sp_VehicleType_GetAllMethod"));                // vehicleTypefill
        m.put("warehouses", warehouses());                                             // WarehouseFill
        m.put("cities", sup.rows("SP_City_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "MethodType", "GetAll"));   // CityFill
        m.put("openRecords", openRecords());                                           // grdfrmfill
        m.put("doGrid", infoGrid(DO_DOC));                                             // DeliveryOrderGridFill
        m.put("soGrid", infoGrid(SO_DOC));                                             // SaleOrderGridFill
        String cityArea = sup.config("City Area");
        m.put("cityArea", cityArea.isEmpty() ? 0 : toInt(cityArea));
        int days = toInt(sup.config("DefaultDaysToLessFromHistoryFromDate"));
        m.put("historyDays", days > 0 ? days : 3);
        m.put("historyCustomers", sup.rows("Usp_SupplierCustomerAgainstOutwarGP", "OrganizationId", sup.org(), "CompanyId", sup.company(), "BranchesId", idOrNull(sup.branch())));   // CustomerHistoryFill
        return m;
    }

    /** Refresh button (btnRefresh_Click :2000): ItemNameFill, gatepasstype, vehicleTypefill, CityFill, WarehouseFill, grdfrmfill. */
    public Map<String, Object> refresh() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("items", items());
        m.put("gatePassTypes", sup.rows("Sp_GatePassType_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "Outward"));
        m.put("vehicleTypes", sup.rows("Sp_VehicleType_GetAllMethod"));
        m.put("cities", sup.rows("SP_City_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "MethodType", "GetAll"));
        m.put("warehouses", warehouses());
        m.put("openRecords", openRecords());
        return m;
    }

    /** Reset (:1921) re-reads gpnofill, grdfrmfill and the two information grids. */
    public Map<String, Object> resetData() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("nextNo", nextGpNo());
        m.put("openRecords", openRecords());
        m.put("doGrid", infoGrid(DO_DOC));
        m.put("soGrid", infoGrid(SO_DOC));
        return m;
    }

    /** gpnofill: GatePassOutward.GenerategpCode (DocumentTypeId 1507) - 0 when the procedure returns no row. */
    public int nextGpNo() {
        return sup.firstInt(sup.rows("Sp_GatePassOutward_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "DocumentTypeId", DOC_TYPE, "FinancialYearId", sup.fy(), "BranchesId", idOrNull(sup.branch()), "Activity", "GenerategpCode"), "GpSrNo");
    }

    /** CustomerFill / CommonServices.SupplierCustomerGetforComboServiceBind = SupplierCustomer.GetforComboBinding. */
    public List<Map<String, Object>> customers() {
        return sup.rows("Sp_SupplierCustomer_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "ReadByOrganizationIdCompanyIdForBinding");
    }

    /** WarehouseFill: CommonServices.getActiveWareHouse = InvWareHouse.GetActiveWareHouse. */
    public List<Map<String, Object>> warehouses() {
        return sup.rows("Sp_InvWareHouse_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetActiveWareHouse");
    }

    /** ItemNameFill: CommonServices.AllItemsBindForSteel = Item.AllItemsBindForSteel. */
    public List<Map<String, Object>> items() {
        return sup.rows("Sp_Item_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "AllItemsBindForSteel");
    }

    // ------------------------------------------------------------------ CmbOrderno_Leave :642

    public Map<String, Object> orderLeave(String basedOn, String orderNo, String gpDate) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("soId", 0);
        if ("SaleOrder".equals(basedOn)) {
            Dt d = parseDate(gpDate);
            SaleSteelSupport.Table lst = db.table(P_SUPP_SO, "OrganizationId", sup.org(), "CompanyId", sup.company(), "DocumentTypeId", SO_DOC,
                    "OrderNo", toInt(text(orderNo)), "FinancialYearId", sup.fy(), "GpDate", d == null ? null : d.ts);                 // GetSaleOrderDataForOutward
            if (!lst.rows.isEmpty()) {
                out.put("soId", toInt(SaleSteelSupport.get(lst.rows.get(0), "SaleOrderId")));
                out.put("items", lst.rows);                                      // BindDDLNew(lst, CmbVariety, "OrderItemId", "ItemName")
                out.put("customers", lst.rows);                                  // BindDDLNew(lst, cmbsupp, "Id", "CompanyName")
            } else {
                out.put("clearCustomer", true);
                out.put("clearOrderNo", true);
            }
        } else if ("DeliverOrder".equals(basedOn)) {
            int code = sup.firstInt(db.table(P_DO, "OrganizationId", sup.org(), "CompanyId", sup.company(), "DocNo", toInt(text(orderNo)),
                    "DocumentTypeId", DO_DOC, "FinancialYearId", sup.fy(), "BranchesId", idOrNull(sup.branch()), "Activity", "DeliveryOrderNoCheckinGatePass").rows, "Id");
            if (code > 0) {
                out.put("soId", code);
                List<Map<String, Object>> w = db.table(P_DO, "OrganizationId", sup.org(), "CompanyId", sup.company(), "Id", code,
                        "DocumentTypeId", DO_DOC, "Activity", "GetTotalWeightFromDeliveryOrder").rows;
                if (!w.isEmpty()) {
                    Map<String, Object> w0 = w.get(0);
                    out.put("items", w);                                         // BindDDLNew(Weight, CmbVariety, "ItemId", "ItemName")
                    out.put("itemId", toInt(SaleSteelSupport.get(w0, "ItemId")));
                    out.put("warehouseId", SaleSteelSupport.s(SaleSteelSupport.get(w0, "WarehouseId")));
                    out.put("containerWeight", SaleSteelSupport.s(SaleSteelSupport.get(w0, "TotalWeight")));
                    out.put("qty", SaleSteelSupport.s(SaleSteelSupport.get(w0, "DoTotalQty")));
                    out.put("vehicleNo", SaleSteelSupport.s(SaleSteelSupport.get(w0, "VehicleNo")));
                    out.put("vehicleTypeId", toInt(SaleSteelSupport.get(w0, "VehicleTypeId")));
                    out.put("deliveryOrderType", SaleSteelSupport.s(SaleSteelSupport.get(w0, "DeliveryOrderType")));
                    out.put("customers", w);                                     // BindDDLNew(Weight, cmbsupp, "Id", "CompanyName")
                } else {
                    out.put("soId", 0);
                    out.put("clearContainerWeight", true);
                    out.put("clearFactoryWeight", true);
                    out.put("clearCustomer", true);
                }
            } else {
                out.put("clearOrderNo", true);
                out.put("clearCustomer", true);
            }
        }
        return out;
    }

    // ------------------------------------------------------------------ grids

    /** grdfrmfill :1599 - GatePassOutward.GetHistoryonlystatusOpen; the grid shows the procedure's columns. */
    public Map<String, Object> openRecords() {
        return SaleSteelSupport.grid(db.table("Sp_GatePassOutward_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "DocumentTypeId", DOC_TYPE, "FinancialYearId", sup.fy(), "BranchesId", idOrNull(sup.branch()), "Activity", "GetHistoryonlystatusOpen"));
    }

    /** DeliveryOrderGridFill :1756 / SaleOrderGridFill :1832 - SaleOrder.OrderDetailForGatePassOutwardSteel. */
    public Map<String, Object> infoGrid(int docType) {
        return SaleSteelSupport.grid(db.table(P_INFO, "OrganizationId", sup.org(), "CompanyId", sup.company(), "FinancialYearId", sup.fy(),
                "BranchesId", idOrNull(sup.branch()), "DocumentTypeId", docType));
    }

    // ------------------------------------------------------------------ History (gridhistory :1253)

    public Map<String, Object> history(String dateType, String fromDate, String toDate, int fromNo, int toNo, int customerId) {
        boolean viewAll = sup.rights(SCREEN).get("viewAll");
        UserAccount u = sup.user();
        String df = "DateFrom", dt = "DateTo";
        if ("entry".equals(dateType)) { df = "EntryFromDate"; dt = "EntryToDate"; }
        else if ("modify".equals(dateType)) { df = "ModifyFromDate"; dt = "ModifyToDate"; }
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("DocumentTypeId", DOC_TYPE);
        p.put("FinancialYearId", sup.fy());
        p.put("BranchesId", idOrNull(u.getBranchesId()));
        if (customerId > 0) p.put("SupplierCustomerId", customerId);
        Dt f = parseDate(fromDate), t = parseDate(toDate);
        if (t != null) p.put(dt, t.ts);
        if (f != null) p.put(df, f.ts);
        if (fromNo > 0) p.put("GpSrNoFrom", fromNo);
        if (toNo > 0) p.put("GpSrNoTo", toNo);
        p.put("CanViewAllRecord", viewAll);
        if (!viewAll) p.put("EntryUser", u.getId());
        p.put("Activity", "ReadByGatePassHistory");
        return SaleSteelSupport.grid(db.table("Sp_GatePassOutward_GetAllMethod", p));
    }

    // ------------------------------------------------------------------ ReadById :930 + GetTicketNoandWeighBridgeWeight :1701

    public Map<String, Object> record(int id) {
        Map<String, Object> rec = load(id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rec", rec);
        String based = str(rec.get("OtherSupCust")).trim();
        out.put("basedOn", based);
        int soId = 0;
        if ("SaleOrder".equals(based)) {
            // GetSupplierBySaleOrderNoForSteel passes the order number as @SaleOrderId, which [ST].[USP_GetSupplierBySaleOrderNo] does not use
            // (it looks the order up by @OrderNo), so on the desktop the customer list stays empty. The web also sends @OrderNo so the
            // customer of the stored order is listed; an order that can no longer be read (expired, completed) leaves the list empty.
            try {
                List<Map<String, Object>> lst = db.table(P_SUPP_SO, "SaleOrderId", toInt(str(rec.get("SupplierContractCode")).trim()), "OrderNo", toInt(str(rec.get("SupplierContractCode")).trim()),
                        "DocumentTypeId", SO_DOC, "OrganizationId", sup.org(), "CompanyId", sup.company(), "FinancialYearId", sup.fy()).rows;
                if (!lst.isEmpty()) { soId = toInt(SaleSteelSupport.get(lst.get(0), "SaleOrderId")); out.put("customers", lst); }
            } catch (DataAccessException ignored) { /* desktop shows nothing here */ }
            if (soId == 0) soId = toInt(rec.get("SaleOrderId"));
        } else if ("DeliveryOrder".equals(based)) {
            soId = toInt(rec.get("SaleOrderId"));
            // InvDeliveryOrder.GetTotalWeightFromDeliveryOrder (the non-Steel procedure Sp_InvDeliveryOrder_GetAllMethod)
            List<Map<String, Object>> w = sup.rows("Sp_InvDeliveryOrder_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                    "Id", soId, "DocumentTypeId", DO_DOC, "Activity", "GetTotalWeightFromDeliveryOrder");
            if (!w.isEmpty()) out.put("customers", w);
        } else if ("GateSale".equals(based)) {
            soId = toInt(rec.get("SaleOrderId"));
            out.put("customers", customers());
        }
        out.put("soId", soId);
        out.put("wb", wbInfo(id));
        out.put("attachments", attachments.list(SCREEN, id));
        return out;
    }

    private Map<String, Object> load(int id) {
        List<Map<String, Object>> rows = sup.rows("Sp_GatePassOutward_GetAllMethod", "Id", id, "Activity", "ReadById");
        if (rows.size() != 1 || toInt(rows.get(0).get("OrganizationId")) != sup.org() || toInt(rows.get(0).get("CompanyId")) != sup.company())
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Outward gate pass not found in this company");
        return rows.get(0);
    }

    /** GetTicketNoandWeighBridgeWeight: WbTransactions.GetNetWeightFromWbTransactions (ReferenceDocTypeId 1507, ReferenceDocNoId = gate pass id). */
    public Map<String, Object> wbInfo(int gpId) {
        List<Map<String, Object>> lst = sup.rows("Sp_WbTransation_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "Id", gpId, "RefDocumentTypeId", DOC_TYPE, "Activity", "GetNetWeightFromWbTransactions", "BranchesId", idOrNull(sup.branch()));
        Map<String, Object> m = new LinkedHashMap<>();
        if (!lst.isEmpty()) {
            StringBuilder ticket = new StringBuilder();
            double net = 0;
            for (Map<String, Object> r : lst) { ticket.append(',').append(str(r.get("TicketNo"))); net += toDouble(r.get("NetWbWeight")); }
            m.put("ticketNo", ticket.toString());
            m.put("netWeight", net);
        } else {
            m.put("ticketNo", "0");
            m.put("netWeight", 0);
        }
        return m;
    }

    // ------------------------------------------------------------------ Save (btnsave_Click :1062 / btnupdate_Click :323)

    public static class Request {
        public int id;
        public String gpDate;
        public int gpSrNo;
        public String gatepassType;
        public int gpTypeSrNo;
        public String basedOn;               // DeliverOrder / SaleOrder / GateSale (the combo VALUE); basedOnText below is the displayed text
        public String basedOnText;           // DeliveryOrder / SaleOrder / GateSale
        public String orderNo;
        public int supplierCustomerId;
        public String remarks;
        public int noOfPackages;
        public int vehicleTypeId;
        public String vehicleNo;
        public String biltyNo;
        public double freight;
        public double netPaid;
        public String inTime;
        public String outTime;
        public int cityId;
        public String status;                // Open / Accepted / Rejected
        public String wbStatus;              // Auto / Manual (the combo TEXT)
        public String weighBridgeSlipNo;
        public double factoryWeight;
        public String factoryWeightText;
        public int warehouseId;
        public String varietyName;
        public double packUnit;
        public double compareWeight;
        public double containerWeight;
        public String containerWeightText;
        public double differenceWeight;
        public String differenceWeightText;
        public String diffWeightRemarks;
        public SaleEngrAttachments.Change attachments;
    }

    @Transactional
    public Map<String, Object> save(Request r) {
        UserAccount u = sup.user();
        boolean upd = r.id > 0;
        String basedText = text(r.basedOnText);
        validate(r, basedText);
        // btnsave_Click / btnupdate_Click
        if (!upd && !"Open".equals(text(r.status))) throw new IllegalArgumentException("Status Not Change");
        double factory = 0;
        if (upd) {
            if (!text(r.factoryWeightText).isEmpty() && !"0".equals(text(r.factoryWeightText))) factory = toDouble(r.factoryWeightText);
            else if ("Accepted".equals(text(r.status))) throw new IllegalArgumentException("Factory Weight field required");
        } else if (!text(r.factoryWeightText).isEmpty()) {
            factory = (float) toDouble(r.factoryWeightText);
        }
        double container = text(r.containerWeightText).isEmpty() ? 0 : toDouble(r.containerWeightText);
        double diff = text(r.differenceWeightText).isEmpty() ? 0 : toDouble(r.differenceWeightText);
        double packUnit = 0, cmp = 0;
        if ("SaleOrder".equals(basedText)) { packUnit = toDouble(r.packUnit); cmp = toDouble(r.compareWeight); }

        Map<String, Object> old = upd ? load(r.id) : null;
        // SOId is kept in a form field on the desktop (set by CmbOrderno_Leave / ReadById); the web re-derives it from the same calls
        String orderNo = text(r.orderNo);
        int soId = 0;
        if ("SaleOrder".equals(basedText)) {
            List<Map<String, Object>> lst = db.table(P_SUPP_SO, "OrganizationId", sup.org(), "CompanyId", sup.company(), "DocumentTypeId", SO_DOC,
                    "OrderNo", toInt(orderNo), "FinancialYearId", sup.fy(), "GpDate", parseDate(r.gpDate) == null ? null : parseDate(r.gpDate).ts).rows;
            if (!lst.isEmpty()) soId = toInt(SaleSteelSupport.get(lst.get(0), "SaleOrderId"));
            else if (old != null) soId = toInt(old.get("SaleOrderId"));
        } else if ("DeliveryOrder".equals(basedText)) {
            soId = sup.firstInt(db.table(P_DO, "OrganizationId", sup.org(), "CompanyId", sup.company(), "DocNo", toInt(orderNo),
                    "DocumentTypeId", DO_DOC, "FinancialYearId", sup.fy(), "BranchesId", idOrNull(sup.branch()), "Activity", "DeliveryOrderNoCheckinGatePass").rows, "Id");
            if (soId == 0 && old != null) soId = toInt(old.get("SaleOrderId"));
        } else if (old != null) {
            soId = toInt(old.get("SaleOrderId"));
        }
        if (!upd && soId == 0 && !"GateSale".equals(basedText)) throw new IllegalArgumentException("Order No Not Found");

        int refDoc = "GateSale".equals(basedText) ? 1507 : ("SaleOrder".equals(basedText) ? 1505 : 1506);
        String vehicleType = "";
        if (r.vehicleTypeId > 0) for (Map<String, Object> v : sup.rows("Sp_VehicleType_GetAllMethod")) if (toInt(v.get("Id")) == r.vehicleTypeId) vehicleType = str(v.get("VehicleDescription"));

        Timestamp now = now();
        Dt inT = parseDateTime(r.inTime), outT = parseDateTime(r.outTime);
        Timestamp inStamp = upd ? (inT == null ? null : inT.ts) : now;                       // insert: InDateTimeStamp = DateTime.Now
        Timestamp outStamp = upd ? now : (outT == null ? null : outT.ts);                    // update: OutDateTimeStamp = DateTime.Now
        String proc = upd ? "Sp_GatePassOutward_Update" : "Sp_GatePassOutward_Insert";
        String sql = "EXEC dbo." + proc + " @Id=?,@DocumentTypeId=1507,@GpSrNo=?,@GpDate=?,@GatepassType=?,@GpTypeSrNo=?,@SupplierCustomerId=?,@OtherSupCust=?,@VehicleType=?,@VehicleNo=?,"
                + "@BiltyNo=?,@NoOfPackages=?,@OtherRemarks=?,@RefDocumentTypeId=?,@RefDocumentEntryNo=?,@InDateTimeStamp=?,@OutDateTimeStamp=?,@DocAttachment=?,@IsApproved=?,"
                + "@EntryDate=?,@EntryUser=?,@ModifyDate=?,@ModifyUser=?,@OrganizationId=?,@CompanyId=?,@PostUser=0,@PostState=0,@PostDate=NULL,@CityId=?,@Freight=?,"
                + "@SupplierContractCode=?,@Status=?,@NetPaid=?,@SupplierWeight=?,@FactoryWeight=?,@DifferenceWeight=?,@WeighBridgeId=?,@WarehouseId=?,@VarietyName=?,@SaleOrderId=?,"
                + "@Container=?,@Container1=?,@SealNo=?,@SealNo1=?,@DriverName=NULL,@DriverCellNo=NULL,@DriverCNIC=NULL,@WeightDiffRemarks=?,@FinancialYearId=?,@PackUnit=?,"
                + "@WeightCommapredToSoWt=?,@BranchesId=?,@ItemId=?,@ExportReturnGrnId=0,@GdnId=0,@TransporterId=?,@ReferencePartyId=?,@BiltyDate=?,@UserLogId=NULL";
        Object[] a = {r.id, r.gpSrNo, parseDate(r.gpDate) == null ? null : parseDate(r.gpDate).ts, text(r.gatepassType), r.gpTypeSrNo, idOrNull(r.supplierCustomerId), basedText, vehicleType, text(r.vehicleNo),
                text(r.biltyNo), r.noOfPackages, text(r.remarks), refDoc, 0, inStamp, outStamp, text(r.wbStatus),
                old == null ? Boolean.FALSE : toBool(old.get("IsApproved")), old == null ? now : old.get("EntryDate"), old == null ? u.getId() : old.get("EntryUser"), now, u.getId(),
                u.getOrganizationId(), u.getCompanyId(), idOrNull(r.cityId), r.freight, orderNo, text(r.status), r.netPaid, container, factory, diff,
                idOrNull(toInt(r.weighBridgeSlipNo)), idOrNull(r.warehouseId), text(r.varietyName), idOrNull(soId), null, null, null, null,
                upd ? text(r.diffWeightRemarks) : null, sup.fy(), packUnit, cmp, u.getBranchesId(), null, null, null, null};
        Integer got = ProcExec.call(sup.jdbc(), sql, a);
        int id = upd ? r.id : (got == null ? 0 : got);
        if (id <= 0) throw new IllegalStateException("The gate pass could not be saved.");
        attachments.apply(SCREEN, DOC_TYPE, id, r.supplierCustomerId, r.attachments);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id);
        out.put("gpSrNo", r.gpSrNo);
        out.put("updated", upd);
        out.put("message", (upd ? "Record Update Successfully" : "Record Save Successfully") + r.gpSrNo);
        return out;
    }

    /** formvalidation :841 - word for word, same order. */
    private void validate(Request r, String basedText) {
        String t = text(r.gatepassType);
        if (t.isEmpty() || "0".equals(t)) throw new Warning("Please select GatePass Type");
        if (r.gpSrNo == 0) throw new Warning("Please select Gate Pass No");
        if (r.supplierCustomerId == 0) throw new Warning("Please Select Customer");
        if (r.vehicleTypeId == 0) throw new Warning("Please Select Vehicle Type");
        if (text(r.vehicleNo).isEmpty()) throw new Warning("Please Select Vehicle No");
        if (!Set.of("Open", "Accepted", "Rejected").contains(text(r.status))) throw new Warning("Please Select Status");
        if (!Set.of("Auto", "Manual").contains(text(r.wbStatus))) throw new Warning("Please Select WeighBridgeStatus");
        String on = text(r.orderNo);
        if ("SaleOrder".equals(basedText)) {
            if (on.isEmpty() || "0".equals(on)) throw new Warning("Please Enter Order#");
            if (r.supplierCustomerId == 0) throw new Warning("Please Select Customer Name");
            if (toDouble(r.packUnit) <= 0.0) throw new Warning("PackUnit Field Required");
            if (toDouble(r.compareWeight) <= 0.0) throw new Warning("Weight Field Required");
        } else if ("DeliveryOrder".equals(basedText)) {
            if (on.isEmpty() || "0".equals(on)) throw new Warning("Please Enter Delivery Order #");
            String cw = text(r.containerWeightText);
            if (cw.isEmpty() || "0".equals(cw)) throw new Warning("Container Weight field required");
        }
    }

    public List<Map<String, Object>> attachmentList(int id) { load(id); return attachments.list(SCREEN, id); }

    public SaleEngrAttachments.Download download(int id, int attachmentId) { load(id); return attachments.download(SCREEN, id, attachmentId); }

    // ------------------------------------------------------------------ Define Vehicle (VehicleType form: Sp_VehicleType_Insert / Update @id @vehicledescription)

    @Transactional
    public Map<String, Object> saveVehicleType(int id, String description) {
        String d = text(description);
        if (!d.isEmpty()) {
            String proc = id > 0 ? "Sp_VehicleType_Update" : "Sp_VehicleType_Insert";
            sup.setProc(proc, "id", id, "vehicledescription", d);
        }
        return SaleEngrSupport.row("rows", sup.rows("Sp_VehicleType_GetAllMethod"));
    }

    // ------------------------------------------------------------------ helpers

    private static final class Dt { final Timestamp ts; Dt(Timestamp t) { ts = t; } }

    private static Dt parseDate(String s) {
        if (s == null || s.isBlank()) return null;
        try { return new Dt(Timestamp.valueOf(LocalDate.parse(s.trim().substring(0, Math.min(10, s.trim().length()))).atStartOfDay())); } catch (RuntimeException e) { return null; }
    }

    private static Dt parseDateTime(String s) {
        if (s == null || s.isBlank()) return null;
        try {
            String t = s.trim().replace(' ', 'T');
            if (t.length() == 10) return new Dt(Timestamp.valueOf(LocalDate.parse(t).atStartOfDay()));
            if (t.length() == 5) return new Dt(Timestamp.valueOf(LocalDate.now().atTime(java.time.LocalTime.parse(t))));
            return new Dt(Timestamp.valueOf(LocalDateTime.parse(t.length() == 16 ? t + ":00" : t)));
        } catch (RuntimeException e) { return null; }
    }
}
