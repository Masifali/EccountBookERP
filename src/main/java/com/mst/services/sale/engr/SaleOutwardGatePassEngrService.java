package com.mst.services.sale.engr;

import com.mst.models.UserAccount;
import com.mst.repositories.support.ProcExec;
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
 * Screen 534 "OutwardGatePassTrading" = Architecture.WinApp.SaleTrading.OutwardGatePassTrading (Sale Engr, module 83).
 *
 * Desktop map (OutwardGatePassTrading.cs, 4478 lines; InitializeComponent 2882):
 *   OutwardGatePass_Load :322, gpnofill :430, Status :450, WeighBridgeStatus :468, GpBasedOn :486, gatepasstype :503,
 *   cmbgptype_Leave_1 :520, vehicleTypefill :550, CityFill :566, SupplierFill :585, CmbOrderno_Leave :610,
 *   CmbGPBasedOn_Leave :745, formvalidation :775, Insert :850, ReadById :1010, CustomerHistoryFill :1148, gridhistory :1215,
 *   grdhistorysetting :1370, grdfrmfill :1500, DeliveryOrderGridFill :1655, SaleOrderGridFill :1735, Reset :1815,
 *   GetTicketNoandWeighBridgeWeight :1985, btnPrint_Click :2030, KeyDown :2145.
 * Differences from the base OutwardGatePass (Sale module): no warehouse / item / container / seal / pack-unit / weight-compare /
 * difference-weight / bilty-date fields, RefDocumentTypeId is always 1606, the order types come from USP_GetOrderTypeForOutwardGP_Engr
 * (1605 Sale Order / 1606 Delivery Order / 59 / 3 General ...), and the Sale Order / Delivery Order information tabs are shown when
 * the configurations EnableSaleOrderFlow / EnableDeliveryOrderFlow are on.
 */
@Service
public class SaleOutwardGatePassEngrService {
    public static final String SCREEN = "OutwardGatePassTrading";
    public static final int DOC_TYPE = 91;

    private final SaleEngrSupport sup;
    private final SaleEngrAttachments attachments;

    public SaleOutwardGatePassEngrService(SaleEngrSupport sup, SaleEngrAttachments attachments) {
        this.sup = sup; this.attachments = attachments;
    }

    // ------------------------------------------------------------------ Load

    public Map<String, Object> initial() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("rights", sup.rights(SCREEN));
        m.put("nextNo", nextGpNo());                                                   // gpnofill
        m.put("orderTypes", sup.rows("USP_GetOrderTypeForOutwardGP_Engr", "OrganizationId", sup.org(), "CompanyId", sup.company()));   // GpBasedOn
        m.put("gatePassTypes", sup.rows("Sp_GatePassType_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "Outward"));   // gatepasstype
        m.put("vehicleTypes", sup.rows("Sp_VehicleType_GetAllMethod"));                // VehicleType.GetAll()
        m.put("cities", sup.rows("SP_City_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "MethodType", "GetAll"));   // City.GetAll
        m.put("customers", suppliers());                                               // SupplierFill
        m.put("historyCustomers", sup.rows("Usp_SupplierCustomerAgainstOutwarGP", "OrganizationId", sup.org(), "CompanyId", sup.company()));   // CustomerHistoryFill
        m.put("openRecords", openRecords());                                           // grdfrmfill
        String cityArea = sup.config("City Area");
        m.put("cityArea", cityArea.isEmpty() ? 0 : toInt(cityArea));
        boolean so = sup.configBool("EnableSaleOrderFlow"), dO = sup.configBool("EnableDeliveryOrderFlow");
        m.put("enableSO", so);
        m.put("enableDO", dO);
        m.put("doRows", dO ? deliveryOrderRows() : List.of());
        m.put("soRows", so ? saleOrderRows() : List.of());
        m.put("amountDecimals", amountDecimals());
        return m;
    }

    private int amountDecimals() {
        int n = toInt(sup.config("Default NoofDecimal Points For Amount"));
        return Math.max(0, Math.min(15, n));
    }

    /** gpnofill: GatePassOutward.GenerategpCode - 0 when the procedure returns no row. */
    public int nextGpNo() {
        return sup.firstInt(sup.rows("Sp_GatePassOutward_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "DocumentTypeId", DOC_TYPE, "FinancialYearId", sup.fy(), "BranchesId", idOrNull(sup.branch()), "Activity", "GenerategpCode"), "GpSrNo");
    }

    /** cmbgptype_Leave_1: GatePassOutward.GenerateGPTypeCode. */
    public Map<String, Object> typeCode(String type) {
        int code = sup.firstInt(sup.rows("Sp_GatePassOutward_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "DocumentTypeId", DOC_TYPE, "FinancialYearId", sup.fy(), "BranchesId", idOrNull(sup.branch()),
                "GatepassType", type == null ? "" : type, "Activity", "GenerateGPTypeCode"), "GpTypeSrNo");
        return SaleEngrSupport.row("code", code);
    }

    /** SupplierFill: ERP feature 4 -> USP_GetVendorsAndCustomers(1) else SupplierCustomerGetforComboServiceBind. */
    private List<Map<String, Object>> suppliers() {
        if (sup.erpFeature(4))
            return sup.rows("USP_GetVendorsAndCustomers", "OrganizationId", sup.org(), "CompanyId", sup.company(), "PartyTypeId", 1);
        return sup.rows("Sp_SupplierCustomer_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "Activity", "ReadByOrganizationIdCompanyIdForBinding");
    }

    /** CmbGPBasedOn_Leave: order type 3 (General) -> SupplierFill again. */
    public List<Map<String, Object>> customersFor(int orderTypeId) { return suppliers(); }

    // ------------------------------------------------------------------ CmbOrderno_Leave

    public Map<String, Object> orderLeave(int orderTypeId, String orderNo, String gpDate, String gatePassType) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("soId", 0);
        if (text(typeName(orderTypeId)).isEmpty()) throw new Warning("Order Type Field Required");
        if (text(gatePassType).isEmpty()) throw new Warning("GatePass Type Field Required");
        switch (orderTypeId) {
            case 1605 -> {
                List<Map<String, Object>> lst = sup.rows("Sp_SupplierCustomer_GetAllMethod", "SaleOrderId", toInt(text(orderNo)),
                        "DocumentTypeId", orderTypeId, "OrganizationId", sup.org(), "CompanyId", sup.company(),
                        "FinancialYearId", sup.fy(), "GpDate", parseDate(gpDate), "Activity", "GetSupplierBySaleOrderNo");
                if (!lst.isEmpty()) {
                    out.put("soId", toInt(lst.get(0).get("SaleOrderId")));
                    out.put("customers", lst);
                    out.put("selectFirst", true);
                } else {
                    out.put("clearCustomer", true);
                    out.put("clearOrderNo", true);
                }
            }
            case 1606 -> {
                int code = sup.firstInt(sup.rows("Sp_InvDeliveryOrder_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                        "DocNo", toInt(text(orderNo)), "DocumentTypeId", orderTypeId, "FinancialYearId", sup.fy(),
                        "BranchesId", sup.branch(), "Activity", "DeliveryOrderNoCheckinGatePass"), "Id");
                if (code > 0) {
                    out.put("soId", code);
                    List<Map<String, Object>> w = sup.rows("Sp_InvDeliveryOrder_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                            "Id", code, "DocumentTypeId", orderTypeId, "Activity", "GetTotalWeightFromDeliveryOrder");
                    if (!w.isEmpty()) {
                        Map<String, Object> w0 = w.get(0);
                        out.put("qty", str(w0.get("DoTotalQty")));
                        out.put("vehicleNo", str(w0.get("VehicleNo")));
                        out.put("vehicleTypeText", str(w0.get("VehicleType")));
                        if ("Export".equals(str(w0.get("DeliveryOrderType"))) && !"Export".equals(text(gatePassType))) {
                            out.put("message", "Please Select GatePass Type Export");
                            out.put("clearOrderNo", true);
                        } else {
                            out.put("customers", w);
                            out.put("selectFirst", true);
                        }
                    } else {
                        out.put("soId", 0);
                        out.put("clearFactoryWeight", true);
                        out.put("clearCustomer", true);
                    }
                } else {
                    out.put("clearOrderNo", true);
                    out.put("clearCustomer", true);
                }
            }
            case 59 -> {
                out.put("customers", sup.rows("USP_GetPartiesFromPurchaseInvoiceStoreWithGlAccount", "OrganizationId", sup.org(), "CompanyId", sup.company()));
            }
            default -> { }
        }
        return out;
    }

    private String typeName(int id) {
        for (Map<String, Object> r : sup.rows("USP_GetOrderTypeForOutwardGP_Engr", "OrganizationId", sup.org(), "CompanyId", sup.company()))
            if (toInt(r.get("Id")) == id) return str(r.get("OrderType"));
        return "";
    }

    // ------------------------------------------------------------------ grids

    /** grdfrmfill: GatePassOutward.GetHistoryonlystatusOpen. dt.Rows[i][Id] reads column 0 (the field Id is 0). */
    public List<Map<String, Object>> openRecords() {
        List<Map<String, Object>> src = sup.rows("Sp_GatePassOutward_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "DocumentTypeId", DOC_TYPE, "FinancialYearId", sup.fy(), "BranchesId", idOrNull(sup.branch()), "Activity", "GetHistoryonlystatusOpen");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : src) {
            out.add(SaleEngrSupport.row("Id", r.get("Id"), "GpDate", r.get("GpDate"), "GpNo", r.get("GpSrNo"), "GatepassType", r.get("GatepassType"),
                    "OrderType", r.get("OrderType"), "OrderNo", r.get("OrderNo"), "Customer", r.get("CustomerName"), "Qty", r.get("NoOfBags"),
                    "VehicleType", r.get("VehicleType"), "VehicleNo", r.get("VehicleNo"), "BiltyNo", r.get("BiltyNo"), "Status", r.get("Status"),
                    "CityName", r.get("CityName"), "FactoryWeight", r.get("FactoryWeight"), "ApprovedBy", r.get("ApprovedBy"), "Remaks", r.get("OtherRemarks")));
        }
        return out;
    }

    /** DeliveryOrderGridFill: InvDeliveryOrder.DeliveryOrderDetailForGatePassOutward_Engr (DocumentTypeIds "1606"). */
    public List<Map<String, Object>> deliveryOrderRows() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("USP_DeliveryOrderDetailForGatePassOutward_Engr", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "FinancialYearId", sup.fy(), "BranchesId", sup.branch(), "DocumentTypeIds", "1606")) {
            out.add(SaleEngrSupport.row("Id", r.get("Id"), "DocumentTypeId", r.get("DocumentTypeId"), "DocDate", r.get("DocDate"), "DoNo", r.get("DoNo"),
                    "SONo", r.get("SaleOrderNo"), "DeliveryOrderType", r.get("DeliveryOrderType"), "CustomerName", r.get("CustomerName"),
                    "DoQty", r.get("DoQty"), "RequestedBy", r.get("RequestedBy"), "ApprovedBy", r.get("ApprovedBy"), "Remarks", r.get("LoadingInstructions"),
                    "AccountRemarks", r.get("AccountRemarks"), "EntryDate", r.get("EntryDate"), "EntryUserName", r.get("EntryUserName")));
        }
        return out;
    }

    /** SaleOrderGridFill: InvDeliveryOrder.SaleOrderDetailForGatePassOutward_Engr (DocumentTypeIds "1605"). */
    public List<Map<String, Object>> saleOrderRows() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("USP_SaleOrderDetailForGatePassOutward_Engr", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "BranchesId", sup.branch(), "FinancialYearId", sup.fy(), "DocumentTypeIds", "1605")) {
            out.add(SaleEngrSupport.row("Id", r.get("Id"), "DocumentTypeId", r.get("DocumentTypeId"), "DocDate", r.get("DocDate"), "OrderNo", r.get("OrderNo"),
                    "CustomerName", r.get("CustomerName"), "ItemQty", r.get("ItemQty"), "DispatchQty", r.get("DispatchQty"), "BalQty", r.get("BalQty"),
                    "OrderStatus", r.get("OrderStatus"), "OrderRemarks", r.get("OrderRemarks"), "EntryDate", r.get("EntryDate"), "EntryUserName", r.get("EntryUserName")));
        }
        return out;
    }

    // ------------------------------------------------------------------ History (gridhistory)

    public List<Map<String, Object>> history(String dateType, String fromDate, String toDate, int fromNo, int toNo, int customerId) {
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
        LocalDate f = parseDate(fromDate), t = parseDate(toDate);
        if (t != null) p.put(dt, t);
        if (f != null) p.put(df, f);
        if (fromNo > 0) p.put("GpSrNoFrom", fromNo);
        if (toNo > 0) p.put("GpSrNoTo", toNo);
        p.put("CanViewAllRecord", viewAll);
        if (!viewAll) p.put("EntryUser", u.getId());
        p.put("Activity", "ReadByGatePassHistory");
        List<Map<String, Object>> src = com.mst.repositories.support.DesktopProc.rows(sup.jdbc(), "Sp_GatePassOutward_GetAllMethod", p);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : src) {
            out.add(SaleEngrSupport.row("Id", r.get("Id"), "GatepassType", r.get("GatepassType"), "OrderType", r.get("OrderType"), "GpSrNo", r.get("GpSrNo"),
                    "GpDate", r.get("GpDate"), "DeliveryOrderNo", r.get("DeliveryOrderNo"), "OrderNo", r.get("OrderNo"), "CustomerName", r.get("CompanyName"),
                    "ItemQty", r.get("ItemQty"), "VehicleType", r.get("VehicleType"), "VehicleNo", r.get("VehicleNo"), "BiltyNo", r.get("BiltyNo"),
                    "InTime", r.get("InTime"), "OutTime", r.get("OutTime"), "CityName", r.get("Description"), "Freight", r.get("Freight"),
                    "NetPaid", r.get("NetPaid"), "Status", r.get("Status"), "SupplierWeight", r.get("SupplierWeight"), "FactoryWeight", r.get("FactoryWeight"),
                    "DifferenceWeight", r.get("DifferenceWeight"), "WeighBridgeStatus", r.get("WeighBridgeStatus"), "EntryUser", r.get("UserName"),
                    "EntryDate", r.get("EntryDate"), "ModifyUser", r.get("ModifyUserName"), "ModifyDate", r.get("ModifyDate"), "IsApproved", r.get("IsApproved"),
                    "OtherRemarks", r.get("OtherRemarks"), "NoOfAttachments", r.get("NoOfAttachments")));
        }
        return out;
    }

    // ------------------------------------------------------------------ ReadById + GetTicketNoandWeighBridgeWeight

    public Map<String, Object> record(int id) {
        Map<String, Object> rec = load(id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rec", rec);
        int type = toInt(typeIdByName(str(rec.get("OtherSupCust"))));
        out.put("orderTypeId", type);
        int soId = 0;
        if (type == 1605) {
            List<Map<String, Object>> lst = sup.rows("Sp_SupplierCustomer_GetAllMethod", "SaleOrderId", toInt(str(rec.get("SupplierContractCode")).trim()),
                    "DocumentTypeId", 1605, "OrganizationId", sup.org(), "CompanyId", sup.company(), "FinancialYearId", sup.fy(), "Activity", "GetSupplierBySaleOrderNo");
            if (!lst.isEmpty()) { soId = toInt(lst.get(0).get("SaleOrderId")); out.put("customers", lst); }
        } else if (type == 1606) {
            soId = toInt(rec.get("SaleOrderId"));
            List<Map<String, Object>> w = sup.rows("Sp_InvDeliveryOrder_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                    "Id", soId, "DocumentTypeId", 1606, "Activity", "GetTotalWeightFromDeliveryOrder");
            if (!w.isEmpty()) out.put("customers", w);
        } else if (type == 3) {
            soId = toInt(rec.get("SaleOrderId"));
            out.put("customers", suppliers());
        }
        out.put("soId", soId);
        out.put("wb", wbInfo(id));
        out.put("attachments", attachments.list(SCREEN, id));
        return out;
    }

    private Object typeIdByName(String name) {
        for (Map<String, Object> r : sup.rows("USP_GetOrderTypeForOutwardGP_Engr", "OrganizationId", sup.org(), "CompanyId", sup.company()))
            if (str(r.get("OrderType")).equals(name)) return r.get("Id");
        return 0;
    }

    private Map<String, Object> load(int id) {
        List<Map<String, Object>> rows = sup.rows("Sp_GatePassOutward_GetAllMethod", "Id", id, "Activity", "ReadById");
        if (rows.size() != 1 || toInt(rows.get(0).get("OrganizationId")) != sup.org() || toInt(rows.get(0).get("CompanyId")) != sup.company())
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Outward gate pass not found in this company");
        return rows.get(0);
    }

    /** GetTicketNoandWeighBridgeWeight: WbTransactions.GetNetWeightFromWbTransactions (ReferenceDocTypeId 91, ReferenceDocNoId = gate pass id). */
    public Map<String, Object> wbInfo(int gpId) {
        List<Map<String, Object>> lst = sup.rows("Sp_WbTransation_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "Id", gpId, "RefDocumentTypeId", DOC_TYPE, "Activity", "GetNetWeightFromWbTransactions");
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

    // ------------------------------------------------------------------ Save (Insert / btnupdate_Click)

    public static class Request {
        public int id;
        public String gpDate;
        public int gpSrNo;
        public String gatepassType;
        public int gpTypeSrNo;
        public int orderTypeId;
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
        public int statusId;
        public int wbStatusId;
        public String weighBridgeSlipNo;
        public double factoryWeight;
        public SaleEngrAttachments.Change attachments;
    }

    private static final Map<Integer, String> STATUS = Map.of(1, "Open", 2, "Accepted", 3, "Rejected");
    private static final Map<Integer, String> WB = Map.of(1, "Auto", 2, "Mannual");

    @Transactional
    public Map<String, Object> save(Request r) {
        UserAccount u = sup.user();
        // formvalidation()
        String orderTypeText = typeName(r.orderTypeId);
        if (text(r.gatepassType).isEmpty() || "0".equals(text(r.gatepassType))) throw new Warning("Please select GatePass Type");
        if (r.gpSrNo == 0) throw new Warning("Please select Gate Pass No");
        if (text(r.vehicleNo).isEmpty()) throw new Warning("Please Select Vehicle No");
        String vehicleType = "";
        if (r.vehicleTypeId > 0) for (Map<String, Object> v : sup.rows("Sp_VehicleType_GetAllMethod")) if (toInt(v.get("Id")) == r.vehicleTypeId) vehicleType = str(v.get("VehicleDescription"));
        if (r.vehicleTypeId == 0 || vehicleType.isEmpty()) throw new Warning("Please Select Vehicle Type");
        if (r.statusId == 0 || !STATUS.containsKey(r.statusId)) throw new Warning("Please Select Status");
        if (r.wbStatusId == 0 || !WB.containsKey(r.wbStatusId)) throw new Warning("Please Select WeighBridgeStatus");
        String orderNo = text(r.orderNo);
        if ("SaleOrder".equals(orderTypeText)) {
            if (orderNo.isEmpty() || "0".equals(orderNo)) throw new Warning("Please Enter Order#");
            if (r.supplierCustomerId == 0) throw new Warning("Please Select Customer Name");
        } else if (r.orderTypeId != 3 && (orderNo.isEmpty() || "0".equals(orderNo))) {
            throw new Warning("Please Enter Delivery Order #");
        }
        // Insert()
        if (!"Open".equals(STATUS.get(r.statusId)) && r.id == 0) throw new Warning("Status should be Open in Case of Save");

        Map<String, Object> old = r.id > 0 ? load(r.id) : null;
        // SOId is re-derived on the server (the desktop keeps it in a field set by the Leave / ReadById code)
        int soId = 0;
        if (r.orderTypeId == 1605) {
            List<Map<String, Object>> lst = sup.rows("Sp_SupplierCustomer_GetAllMethod", "SaleOrderId", toInt(orderNo), "DocumentTypeId", 1605,
                    "OrganizationId", sup.org(), "CompanyId", sup.company(), "FinancialYearId", sup.fy(), "GpDate", parseDate(r.gpDate), "Activity", "GetSupplierBySaleOrderNo");
            if (!lst.isEmpty()) soId = toInt(lst.get(0).get("SaleOrderId"));
        } else if (r.orderTypeId == 1606) {
            soId = sup.firstInt(sup.rows("Sp_InvDeliveryOrder_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "DocNo", toInt(orderNo),
                    "DocumentTypeId", 1606, "FinancialYearId", sup.fy(), "BranchesId", sup.branch(), "Activity", "DeliveryOrderNoCheckinGatePass"), "Id");
            if (soId == 0 && old != null) soId = toInt(old.get("SaleOrderId"));
        } else if (r.orderTypeId == 3 && old != null) {
            soId = toInt(old.get("SaleOrderId"));
        }
        if (!"General".equals(orderTypeText) && !"Purchase Return".equals(orderTypeText) && soId == 0 && r.orderTypeId != 3)
            throw new Warning("Order No Not Found");

        Timestamp now = now();
        String proc = r.id > 0 ? "Sp_GatePassOutward_Update" : "Sp_GatePassOutward_Insert";
        String sql = "EXEC dbo." + proc + " @Id=?,@DocumentTypeId=91,@GpSrNo=?,@GpDate=?,@GatepassType=?,@GpTypeSrNo=?,@SupplierCustomerId=?,@OtherSupCust=?,@VehicleType=?,@VehicleNo=?,"
                + "@BiltyNo=?,@NoOfPackages=?,@OtherRemarks=?,@RefDocumentTypeId=?,@RefDocumentEntryNo=?,@InDateTimeStamp=?,@OutDateTimeStamp=?,@DocAttachment=?,@IsApproved=?,"
                + "@EntryDate=?,@EntryUser=?,@ModifyDate=?,@ModifyUser=?,@OrganizationId=?,@CompanyId=?,@PostUser=0,@PostState=0,@PostDate=NULL,@CityId=?,@Freight=?,"
                + "@SupplierContractCode=?,@Status=?,@NetPaid=?,@SupplierWeight=?,@FactoryWeight=?,@DifferenceWeight=?,@WeighBridgeId=?,@WarehouseId=?,@VarietyName=?,@SaleOrderId=?,"
                + "@Container=?,@Container1=?,@SealNo=?,@SealNo1=?,@DriverName=NULL,@DriverCellNo=NULL,@DriverCNIC=NULL,@WeightDiffRemarks=?,@FinancialYearId=?,@PackUnit=?,"
                + "@WeightCommapredToSoWt=?,@BranchesId=?,@ItemId=?,@ExportReturnGrnId=0,@GdnId=0,@TransporterId=?,@ReferencePartyId=?,@BiltyDate=?,@UserLogId=NULL";
        Object[] a = {r.id, r.gpSrNo, parseDate(r.gpDate), text(r.gatepassType), r.gpTypeSrNo, idOrNull(r.supplierCustomerId), orderTypeText, vehicleType, text(r.vehicleNo),
                text(r.biltyNo), r.noOfPackages, text(r.remarks), 1606, 0, parseDateTime(r.inTime), parseDateTime(r.outTime), WB.get(r.wbStatusId),
                old == null ? Boolean.FALSE : toBool(old.get("IsApproved")), old == null ? now : old.get("EntryDate"), old == null ? u.getId() : old.get("EntryUser"), now, u.getId(),
                u.getOrganizationId(), u.getCompanyId(), idOrNull(r.cityId), r.freight, orderNo, STATUS.get(r.statusId), r.netPaid, 0.0, r.factoryWeight, 0.0,
                idOrNull(toInt(r.weighBridgeSlipNo)), null, null, idOrNull(soId), null, null, null, null, null, sup.fy(), 0.0, 0.0, u.getBranchesId(), null, null, null, null};
        Integer got = ProcExec.call(sup.jdbc(), sql, a);
        int id = r.id > 0 ? r.id : (got == null ? 0 : got);
        if (id <= 0) throw new IllegalStateException("The gate pass could not be saved.");
        attachments.apply(SCREEN, DOC_TYPE, id, r.supplierCustomerId, r.attachments);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id);
        out.put("gpSrNo", r.gpSrNo);
        out.put("updated", r.id > 0);
        out.put("message", (r.id > 0 ? "Data Update Successfully...." : "Record Save Successfully....") + r.gpSrNo);
        return out;
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

    private static LocalDate parseDate(String s) {
        if (s == null || s.isBlank()) return null;
        try { return LocalDate.parse(s.trim().substring(0, Math.min(10, s.trim().length()))); } catch (RuntimeException e) { return null; }
    }

    private static Timestamp parseDateTime(String s) {
        if (s == null || s.isBlank()) return null;
        try {
            String t = s.trim().replace(' ', 'T');
            if (t.length() == 10) return Timestamp.valueOf(LocalDate.parse(t).atStartOfDay());
            return Timestamp.valueOf(LocalDateTime.parse(t.length() == 16 ? t + ":00" : t));
        } catch (RuntimeException e) { return null; }
    }
}
