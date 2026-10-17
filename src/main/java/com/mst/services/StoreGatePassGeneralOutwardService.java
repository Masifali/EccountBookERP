package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.engr.SaleEngrSupport;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.server.ResponseStatusException;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.*;

import static com.mst.services.sale.engr.SaleEngrSupport.*;

/**
 * Screen 343 GatePassGeneralOutward (Architecture.WinApp.Inventory_Definition.GatePassGeneralOutward, Store Sale, module 109).
 * Rights/attachment screen name = the form name (base.Name) "GatePassGeneralOutward"; DocumentTypeId 92, table GatePassGeneral.
 *
 * Desktop map (GatePassGeneralOutward.cs): InwardGatePass_Load :269, DocumentNoFill :334, vehicleTypefill :369, GatePassType :398,
 * WarehouseFill :428, FactoryWeight :458, LookUpBind :483, WeighBridgeRoute :539, Status :569, formvalidation :~590, ReadById :667,
 * Insert :743, GetFormHistory :~850, GetWeighBridgeWeight :1097, Reset :1166, BalanceQty :1484, textBox1_Leave :1514, DifferenceWeight :1568.
 * BLL Architecture.BLL.Inventory.GatePassGeneral (0550): Save (Id==0 Sp_GatePassGeneral_Insert, else IsApproved=true + Sp_GatePassGeneral_Update),
 * GetByID, GenerateCode, ReadAll; DAL 0404 SetData = SetProc + attachments in one transaction.
 */
@Service
public class StoreGatePassGeneralOutwardService {
    public static final String SCREEN = "GatePassGeneralOutward";
    public static final int DOC_TYPE = 92;
    private static final String PROC = "Sp_GatePassGeneral_GetAllMethod";

    private final SaleEngrSupport sup;
    private final SaleEngrAttachments attachments;

    public StoreGatePassGeneralOutwardService(SaleEngrSupport sup, SaleEngrAttachments attachments) {
        this.sup = sup; this.attachments = attachments;
    }

    // ------------------------------------------------------------------ Load (InwardGatePass_Load)

    public Map<String, Object> initial() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("rights", sup.rights(SCREEN));
        m.put("warehouses", warehouses());                          // WarehouseFill: CommonServices.getActiveWareHouse
        m.put("vehicleTypes", sup.rows("Sp_VehicleType_GetAllMethod"));   // vehicleTypefill: VehicleType.GetAll
        m.put("nextNo", nextNo());                                  // DocumentNoFill
        m.putAll(lookups());                                        // LookUpBind
        return m;
    }

    public List<Map<String, Object>> warehouses() {
        return sup.rows("Sp_InvWareHouse_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetActiveWareHouse");
    }

    public List<Map<String, Object>> vehicleTypes() { return sup.rows("Sp_VehicleType_GetAllMethod"); }

    /** DocumentNoFill: GatePassGeneral.GenerateCode - 0 (text left unchanged on the desktop) when no row. */
    public int nextNo() {
        UserAccount u = sup.user();
        return sup.firstInt(sup.rows(PROC, "Activity", "GenerateCode", "DocumentTypeId", DOC_TYPE, "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "FinancialYearId", idOrNull(sup.fy()), "BranchesId", idOrNull(u.getBranchesId())), "GpSrNo");
    }

    /** LookUpBind: WbPartyandItemsDefine.GetAll split on WbType ("PartyName" => party list, else item list). */
    public Map<String, Object> lookups() {
        List<Map<String, Object>> parties = new ArrayList<>(), items = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("SP_WbPartyandItemsDefine_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "ReadAll")) {
            if ("PartyName".equals(str(r.get("WbType")))) parties.add(row("Id", r.get("Id"), "PartyName", r.get("PartyName")));
            else items.add(row("Id", r.get("Id"), "ItemName", r.get("ItemName")));
        }
        return row("parties", parties, "items", items);
    }

    // ------------------------------------------------------------------ Delivery order (textBox1_Leave / ReadById / BalanceQty)

    /** InvDeliveryOrder.ReadIdByDeliveryOrderId (DocumentTypeId 85); null when there is no row. */
    private Map<String, Object> deliveryOrder(int docNo) {
        List<Map<String, Object>> dt = sup.rows("Sp_InvDeliveryOrder_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "DocNo", docNo, "DocumentTypeId", 85, "Activity", "ReadIdByDeliveryOrderId");
        return dt.isEmpty() ? null : dt.get(0);
    }

    /** BalanceQty: InvDeliveryOrder.TotalReceivedQtyByDeliveryOrderStockTransfer; "0" when no row. */
    private String balQty(int deliveryOrderId) {
        List<Map<String, Object>> dt = sup.rows("Sp_InvDeliveryOrder_GetAllMethod", "PurchaseOrderId", deliveryOrderId, "Organizationid", sup.org(),
                "CompanyId", sup.company(), "Activity", "TotalReceivedQtyByDeliveryOrderStockTransfer");
        return dt.isEmpty() ? "0" : str(dt.get(0).get("BalQty"));
    }

    /** textBox1_Leave for gate pass type Store. */
    public Map<String, Object> deliveryOrderLookup(int docNo) {
        Map<String, Object> d = deliveryOrder(docNo);
        if (d == null) return row("found", false);
        int id = toInt(d.get("Id"));
        return row("found", true, "deliveryOrderId", id, "loadingInstructions", str(d.get("LoadingInstructions")), "netWeight", str(d.get("NetWeight")),
                "vehicleType", str(d.get("VehicleType")), "vehicleNo", str(d.get("VehicleNo")), "balQty", balQty(id));
    }

    // ------------------------------------------------------------------ History (GetFormHistory)

    public static class HistoryFilter {
        public String status;          // "Open" for grdfrmfill, empty for the History tab
        public String dateMode;        // doc | entry | modify | approved
        public String fromDate, toDate;
        public int fromDocNo, toDocNo, partyId;
    }

    public List<Map<String, Object>> history(HistoryFilter f) {
        UserAccount u = sup.user();
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("Activity", "ReadAll");
        p.put("DocumentTypeId", DOC_TYPE);
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        if (sup.fy() != 0) p.put("FinancialYearId", sup.fy());
        if (u.getBranchesId() != 0) p.put("BranchesId", u.getBranchesId());
        if (f.status != null && !f.status.isEmpty()) p.put("GpStatus", f.status);
        if (f.fromDocNo != 0) p.put("FromDocNo", f.fromDocNo);
        if (f.toDocNo != 0) p.put("ToDocNo", f.toDocNo);
        String fk = "FromDate", tk = "ToDate";
        String mode = f.dateMode == null ? "doc" : f.dateMode;
        if ("entry".equals(mode)) { fk = "EntryFromDate"; tk = "EntryToDate"; }
        else if ("modify".equals(mode)) { fk = "ModifyFromDate"; tk = "ModifyToDate"; }
        else if ("approved".equals(mode)) { fk = "ApprovedFromDate"; tk = "ApprovedToDate"; }
        LocalDate from = date(f.fromDate), to = date(f.toDate);
        if (from != null) p.put(fk, from);
        if (to != null) p.put(tk, to);
        if (f.partyId != 0) p.put("SupplierCustomerId", f.partyId);
        List<Map<String, Object>> src = DesktopProc.rows(sup.jdbc(), PROC, p);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : src) {
            out.add(row("Id", r.get("Id"), "GpType", r.get("gatePassType"), "GpDate", r.get("GpDate"), "GpNo", r.get("GpSrNo"),
                    "DeliverOrderNo", r.get("DeliveryOrderNo"), "ManualNo", r.get("ManualNo"), "VehicleType", r.get("VehicleType"), "VehicleNo", r.get("VehicleNo"),
                    "PartyName", r.get("PartyName"), "ItemName", r.get("VarietyName"), "ItemQty", r.get("ItemQty"), "Warehouse", r.get("WarehouseName"),
                    "WBStatus", r.get("Status"), "FactoryWeight", r.get("FactoryWeight"), "EntryUser", r.get("EntryUser"), "EntryDate", r.get("EntryDate"),
                    "ModifyUser", r.get("ModifyUser"), "ModifyDate", r.get("ModifyDate"), "ApprovedUser", r.get("ApprovedUser"), "ApprovedDate", r.get("PostDate"),
                    "NoOfAttachments", r.get("NoOfAttachments"), "Remarks", r.get("OtherRemarks")));
        }
        return out;
    }

    // ------------------------------------------------------------------ ReadById (+ the store delivery-order re-read) and GetWeighBridgeWeight

    private Map<String, Object> load(int id) {
        List<Map<String, Object>> rows = sup.rows(PROC, "Id", id, "Activity", "ReadById");
        if (rows.isEmpty() || toInt(rows.get(0).get("OrganizationId")) != sup.org() || toInt(rows.get(0).get("CompanyId")) != sup.company()
                || toInt(rows.get(0).get("DocumentTypeId")) != DOC_TYPE)
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Gate pass not found in this company");
        return rows.get(0);
    }

    public Map<String, Object> record(int id) {
        Map<String, Object> rec = load(id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rec", rec);
        int doId = toInt(rec.get("PurchaseOrderId"));
        if ("Store".equals(str(rec.get("GatepassType")))) {
            Map<String, Object> d = deliveryOrder(toInt(str(rec.get("SupplierContractCode")).trim()));
            if (d == null) { doId = 0; out.put("doNotFound", true); }
            else {
                doId = toInt(d.get("Id"));
                out.put("loadingInstructions", str(d.get("LoadingInstructions")));
                out.put("doWeight", str(d.get("NetWeight")));
            }
        }
        out.put("deliveryOrderId", doId);
        out.put("attachments", attachments.list(SCREEN, id));
        // GetWeighBridgeWeight (runs after every ReadById call site): WbTransactions.GetNetWeightFromWbTransactions
        List<Map<String, Object>> lst = sup.rows("Sp_WbTransation_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "Id", id, "RefDocumentTypeId", DOC_TYPE, "Activity", "GetNetWeightFromWbTransactions");
        double net = 0;
        for (Map<String, Object> r : lst) net += toDouble(r.get("NetWbWeight"));
        out.put("wbFound", !lst.isEmpty());
        out.put("wbNetWeight", net);
        out.put("balQty", balQty(doId));
        return out;
    }

    // ------------------------------------------------------------------ Save (formvalidation + Insert)

    public static class Request {
        public int id;
        public String gpDate;
        public String gpNo, qty, vehicleNo, deliveryOrderNo, remarks, manualNo, factoryWeight;
        public String gatePassTypeText, vehicleTypeText, statusText, weighBridgeText, partyName, varietyName;
        public int gatePassTypeId, vehicleTypeId, statusId, weighBridgeId, warehouseId;
        public SaleEngrAttachments.Change attachments;
    }

    private static final Map<Integer, String> GP_TYPES = Map.of(1, "General", 2, "Store");
    private static final Map<Integer, String> STATUS = Map.of(1, "Open", 2, "Accepted", 3, "Rejected");
    private static final Map<Integer, String> WB = Map.of(1, "Weighable", 2, "Non-Weighable");

    private static int parseInt(String s) {
        try { return Integer.parseInt(s.trim()); } catch (RuntimeException e) { throw new Warning("Input string was not in a correct format."); }
    }

    @Transactional
    public Map<String, Object> save(Request r) {
        UserAccount u = sup.user();
        String type = GP_TYPES.get(r.gatePassTypeId) == null ? "" : GP_TYPES.get(r.gatePassTypeId);
        String vehicleType = "";
        if (r.vehicleTypeId > 0) for (Map<String, Object> v : vehicleTypes()) if (toInt(v.get("Id")) == r.vehicleTypeId) vehicleType = str(v.get("VehicleDescription"));
        String status = STATUS.get(r.statusId) == null ? "" : STATUS.get(r.statusId);
        String wb = WB.get(r.weighBridgeId) == null ? "" : WB.get(r.weighBridgeId);
        // formvalidation()
        if (type.isEmpty()) throw new Warning("Please Select CmbGatePassType");
        if (text(r.gpNo).isEmpty() || "0".equals(text(r.gpNo))) throw new Warning("Gp No Filed Required");
        if (text(r.vehicleNo).isEmpty()) throw new Warning("Please Select Vehicle No");
        if (vehicleType.isEmpty()) throw new Warning("Please Select Vehicle Type");
        if (!"General".equals(type) && (text(r.deliveryOrderNo).isEmpty() || "0".equals(text(r.deliveryOrderNo)))) throw new Warning("DeliveryOrder No Field Required");
        if (status.isEmpty()) throw new Warning("Please Select Status");
        if (wb.isEmpty()) throw new Warning("Please Select WeighBridge");
        if (text(r.qty).isEmpty() || "0".equals(text(r.qty))) throw new Warning("Qty Field Required");
        // Insert()
        double factory = 0;
        String fw = text(r.factoryWeight);
        if (!fw.isEmpty() && !"0".equals(fw)) { try { factory = Double.parseDouble(fw); } catch (RuntimeException e) { factory = 0; } }
        int gpSrNo = parseInt(r.gpNo);
        int qty = parseInt(r.qty);
        int deliveryOrderId = 0;                                          // the desktop keeps it in a field; the server re-derives it
        if ("Store".equals(type)) {
            Map<String, Object> d = deliveryOrder(toInt(text(r.deliveryOrderNo)));
            if (d != null) deliveryOrderId = toInt(d.get("Id"));
        }
        Timestamp now = now();
        Timestamp today = Timestamp.valueOf(LocalDate.now().atStartOfDay());
        Map<String, Object> p = new LinkedHashMap<>();                    // GenericProvider.SetProc: every non-null model property
        if (r.id > 0) p.put("Id", r.id); else p.put("Id", 0);
        p.put("DocumentTypeId", DOC_TYPE);
        p.put("GpSrNo", gpSrNo);
        p.put("GpDate", date(r.gpDate));
        p.put("GatepassType", type);
        p.put("GpTypeSrNo", 0);
        p.put("SupplierCustomerId", 0);
        p.put("OtherSupCust", text(r.partyName));
        p.put("VehicleType", vehicleType);
        p.put("VehicleNo", text(r.vehicleNo));
        p.put("NoOfPackages", qty);
        p.put("OtherRemarks", text(r.remarks));
        p.put("RefDocumentTypeId", DOC_TYPE);
        p.put("RefDocumentEntryNo", 0);
        p.put("InDateTimeStamp", now);
        p.put("OutDateTimeStamp", now);
        p.put("DocAttachment", status);
        p.put("IsApproved", r.id > 0);                                    // BLL Save: obj.IsApproved = true on update
        p.put("EntryDate", today);
        p.put("EntryUser", u.getId());
        p.put("ModifyDate", today);
        p.put("ModifyUser", u.getId());
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("PostUser", 0);
        p.put("PostState", false);
        p.put("CityId", 0);
        p.put("Freight", 0.0);
        p.put("SupplierContractCode", text(r.deliveryOrderNo));
        p.put("Status", wb);
        p.put("NetPaid", 0.0);
        p.put("SupplierWeight", 0.0);
        p.put("FactoryWeight", factory);
        p.put("DifferenceWeight", 0.0);
        p.put("WeighBridgeId", 0);
        p.put("WarehouseId", r.warehouseId);
        p.put("VarietyName", text(r.varietyName));
        p.put("PurchaseOrderId", deliveryOrderId);
        p.put("FinancialYearId", sup.fy());
        p.put("ManualNo", text(r.manualNo));
        p.put("BranchesId", u.getBranchesId());
        p.put("DeliveryChallanHeaderId", 0);
        p.put("PurchaseDemandId", 0);
        String proc = r.id > 0 ? "Sp_GatePassGeneral_Update" : "Sp_GatePassGeneral_Insert";
        int num = DesktopProc.setProc(sup.jdbc(), proc, p);
        int id = num > 0 ? num : r.id;                                    // DAL SetData: if (num > 0) obj.Id = num else num = obj.Id
        if (id <= 0) throw new IllegalStateException("The gate pass could not be saved.");
        attachments.apply(SCREEN, DOC_TYPE, id, 0, r.attachments);
        return row("id", id, "gpSrNo", gpSrNo, "updated", r.id > 0,
                "message", (r.id > 0 ? "Record Update Successfully[" : "Record Save Successfully[") + gpSrNo + "]");
    }

    public List<Map<String, Object>> attachmentList(int id) { load(id); return attachments.list(SCREEN, id); }

    public SaleEngrAttachments.Download download(int id, int attachmentId) { load(id); return attachments.download(SCREEN, id, attachmentId); }

    private static LocalDate date(String s) {
        if (s == null || s.isBlank()) return null;
        try { return LocalDate.parse(s.trim().substring(0, Math.min(10, s.trim().length()))); } catch (RuntimeException e) { return null; }
    }
}
