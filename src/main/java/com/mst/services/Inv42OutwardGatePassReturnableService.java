package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;
import static com.mst.services.Inv42Support.day;
import static com.mst.services.Inv42Support.dateTime;
import static com.mst.services.Inv42Support.str;
import static com.mst.services.Inv42Support.toDouble;
import static com.mst.services.Inv42Support.toInt;
import static com.mst.services.Inv42Support.truthy;

/**
 * Inventory hub, module 42 — screen 928 "Outward GatePass", desktop
 * {@code Architecture.WinApp.ReturnableInventoryManagement.OutwardGatePassReturnable} (base.Name "OutwardGatePassReturnable"),
 * DocumentTypeId 1201, RefDocumentTypeId 1200. Line refs ":NNN" are OutwardGatePassReturnable.cs.
 *
 * <pre>
 *   InitializeComponentMethod :327   rights, feature 4, GetDataForDropDownFromGPO("Customer"), GenerategpCode(1201),
 *                                    GatePassTypeFill("OutwardReturnable"), GetOrderTypeForOutwardGP_Returnable, ReferenceParties.GetAll,
 *                                    VehicleType.GetAll, GetHistoryonlystatusOpen(1201), DeliveryOrderDetailForGatePassOutward_Engr("1200")
 *   cmbgptype_Leave_1 :430           GenerateGPTypeCode
 *   CmbOrderno_Leave :490            DeliveryOrderNoCheckinGatePass (1200) → GetTotalWeightFromDeliveryOrder
 *   Insert :765                      GatePassOutward.Save → Sp_GatePassOutward_Insert|Update
 *   ReadById :855                    Sp_GatePassOutward_GetAllMethod 'ReadById'
 *   gridhistory :994                 GatepassHistory → 'ReadByGatePassHistory'
 *   btnPrint_Click :1580             OutwardGatePassReturnable_Slip1201 → 1201_OutwardGatePassReturnable_Slip.rpt (existing route)
 * </pre>
 *
 * DESKTOP BEHAVIOUR REPRODUCED: btnDelete_Click is empty (:906) — no Delete; a NEW pass must have Status "Open" (:775);
 * OtherSupCust is the Order Type text and SupplierContractCode the typed Delivery Order number (:797-799);
 * NoOfPackages is Conversion.ToInt of the Qty text (:809); IsApproved is the stored flag (Approve, :792).
 *
 * DEVIATIONS: Gp No from the generator / stored header; tenancy on open / update; FormHelper.ValidateControls messages are
 * rendered as "<caption> Field Required" (the helper's source is not in the recovered tree).
 */
@Service
public class Inv42OutwardGatePassReturnableService {

    public static final String SCREEN = "OutwardGatePassReturnable";
    public static final int DOC_TYPE = 1201;
    public static final int REF_DOC_TYPE = 1200;

    private final Inv42Support db;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public Inv42OutwardGatePassReturnableService(Inv42Support db, StoreScreenRights rights, CurrentUserContext ctx) {
        this.db = db;
        this.rights = rights;
        this.ctx = ctx;
    }

    // ================================================================================== load

    /** InitializeComponentMethod :327-388. */
    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN));
        out.put("defaultDaysToLessFromHistoryFromDate", toInt(db.config(u, "DefaultDaysToLessFromHistoryFromDate")));   // :366
        out.put("gpUpdateWithoutApproval", truthy(db.config(u, "GatePassOutwardUpdateWithoutApproval")));          // :367
        out.put("gpNo", gpNo(u));
        out.put("orderTypes", orderTypes(u));                                                                        // :466
        out.putAll(refresh(u));
        out.put("statuses", statuses());                                                                             // :715
        out.put("openGatePasses", openGatePasses(u));                                                                // :1192
        out.put("pendingDeliveryOrders", pendingDeliveryOrders(u));                                                  // :1327
        out.put("historyCustomers", historyCustomers(u));                                                            // :333
        return out;
    }

    /** btnRefresh_Click :1498. */
    public Map<String, Object> refresh() { return refresh(ctx.requireAccountingUser()); }

    private Map<String, Object> refresh(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("gatePassTypes", gatePassTypes(u));                                  // :335 / :1504
        out.put("referenceParties", db.referenceParties(u));                         // :624
        out.put("transporters", db.transporters(u, db.feature(u, 4)));               // :656
        out.put("vehicleTypes", db.vehicleTypes());                                  // :703
        return out;
    }

    /** GpNoDbCall :391 — GatePassOutward.GenerategpCode (1201). */
    public int gpNo() { return gpNo(ctx.requireAccountingUser()); }

    private int gpNo(UserAccount u) {
        List<Map<String, Object>> r = db.rows("Sp_GatePassOutward_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", DOC_TYPE,
                "FinancialYearId", ctx.currentFinancialYearId(), "BranchesId", u.getBranchesId(), "Activity", "GenerategpCode"));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("GpSrNo"));
    }

    /** cmbgptype_Leave_1 :430 — GenerateGPTypeCode for the selected type text; 0 → "Please GatePass Type Select". */
    public int typeCode(String gatepassType) {
        UserAccount u = ctx.requireAccountingUser();
        if (str(gatepassType).trim().isEmpty()) return 0;
        List<Map<String, Object>> r = db.rows("Sp_GatePassOutward_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", DOC_TYPE,
                "FinancialYearId", ctx.currentFinancialYearId(), "GatepassType", gatepassType, "BranchesId", u.getBranchesId(),
                "Activity", "GenerateGPTypeCode"));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("GpTypeSrNo"));
    }

    /** CommonServices.GatePassTypeFill("OutwardReturnable") → Sp_GatePassType_GetAllMethod (Id, GpTypeDescription). */
    private List<Map<String, Object>> gatePassTypes(UserAccount u) {
        return Inv42Support.project(db.rows("Sp_GatePassType_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "OutwardReturnable")), "Id", "GpTypeDescription");
    }

    /** GpBasedOnDbCall :466 — USP_GetOrderTypeForOutwardGP_Returnable (Id 1200, OrderType 'DeliverOrder'). */
    private List<Map<String, Object>> orderTypes(UserAccount u) {
        return Inv42Support.project(db.rows("USP_GetOrderTypeForOutwardGP_Returnable", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId())), "Id", "OrderType");
    }

    /** Status :715 — 1 Open, 2 Accepted, 4 Cancel. */
    private static List<Map<String, Object>> statuses() {
        List<Map<String, Object>> out = new ArrayList<>();
        int[] ids = { 1, 2, 4 };
        String[] names = { "Open", "Accepted", "Cancel" };
        for (int i = 0; i < ids.length; i++) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ids[i]);
            o.put("Name", names[i]);
            out.add(o);
        }
        return out;
    }

    /** HistoryComboBind :910 — GatePassOutward.GetDataForDropDownFromGPO(org, comp, "Customer", null). */
    private List<Map<String, Object>> historyCustomers(UserAccount u) {
        return Inv42Support.project(db.rows("USP_GetDataForDropDownFromOutWardGP", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "Customer")), "Id", "ReferenceName");
    }

    /** OpenGatePassDbCall :1192 — 'GetHistoryonlystatusOpen' (1201); dtOpenGatePass columns :1225-1237. */
    public List<Map<String, Object>> openGatePasses() { return openGatePasses(ctx.requireAccountingUser()); }

    private List<Map<String, Object>> openGatePasses(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : db.rows("Sp_GatePassOutward_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", DOC_TYPE,
                "FinancialYearId", ctx.currentFinancialYearId(), "BranchesId", u.getBranchesId(), "Activity", "GetHistoryonlystatusOpen"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("GpDate", day(r.get("GpDate")));
            o.put("GpNo", toInt(r.get("GpSrNo")));
            o.put("GatepassType", str(r.get("GatepassType")));
            o.put("OrderType", str(r.get("OrderType")));
            o.put("OrderNo", str(r.get("OrderNo")));
            o.put("Customer", str(r.get("CustomerName")));
            o.put("Qty", toDouble(r.get("NoOfBags")));
            o.put("VehicleType", str(r.get("VehicleType")));
            o.put("VehicleNo", str(r.get("VehicleNo")));
            o.put("BiltyNo", str(r.get("BiltyNo")));
            o.put("Status", str(r.get("Status")));
            o.put("Remaks", str(r.get("OtherRemarks")));
            out.add(o);
        }
        return out;
    }

    /** PendingDeliveryOrderDbCall :1327 — USP_DeliveryOrderDetailForGatePassOutward_Engr (DocumentTypeIds "1200"); dt :1360-1372. */
    public List<Map<String, Object>> pendingDeliveryOrders() { return pendingDeliveryOrders(ctx.requireAccountingUser()); }

    private List<Map<String, Object>> pendingDeliveryOrders(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : db.rows("USP_DeliveryOrderDetailForGatePassOutward_Engr", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", ctx.currentFinancialYearId(), "BranchesId", u.getBranchesId(), "DocumentTypeIds", String.valueOf(REF_DOC_TYPE)))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("DocumentTypeId", toInt(r.get("DocumentTypeId")));
            o.put("DocDate", day(r.get("DocDate")));
            o.put("DoNo", toInt(r.get("DoNo")));
            o.put("DeliveryOrderType", str(r.get("DeliveryOrderType")));
            o.put("CustomerName", str(r.get("CustomerName")));
            o.put("DoQty", toDouble(r.get("DoQty")));
            o.put("RequestedBy", str(r.get("RequestedBy")));
            o.put("ApprovedBy", str(r.get("ApprovedBy")));
            o.put("Remarks", str(r.get("LoadingInstructions")));
            o.put("AccountRemarks", str(r.get("AccountRemarks")));
            o.put("EntryDate", dateTime(r.get("EntryDate")));
            o.put("EntryUserName", str(r.get("EntryUserName")));
            out.add(o);
        }
        return out;
    }

    /**
     * CmbOrderno_Leave :490 — InvDeliveryOrder.DeliveryOrderNoCheckinGatePass (RAISERRORs: "DeliveryOrder you entered has
     * no Existence!", "DeliveryOrder is Not Approved", "Record already exist in GataPass Please Check!") then
     * GetTotalWeightFromDeliveryOrder: DoTotalQty, VehicleNo, VehicleType and the customer rows (Id, CompanyName).
     */
    public Map<String, Object> deliveryOrder(int docNo) {
        UserAccount u = ctx.requireAccountingUser();
        List<Map<String, Object>> ids = db.rows("Sp_InvDeliveryOrder_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", REF_DOC_TYPE,
                "DocNo", docNo, "FinancialYearId", ctx.currentFinancialYearId(), "BranchesId", u.getBranchesId(),
                "Activity", "DeliveryOrderNoCheckinGatePass"));
        int id = ids.isEmpty() ? 0 : toInt(ids.get(0).get("Id"));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("saleOrderId", id);
        out.put("rows", id > 0 ? weightRows(u, id) : new ArrayList<Map<String, Object>>());
        return out;
    }

    /** DeliveryOrderDataFromDoDBCall :558 — 'GetTotalWeightFromDeliveryOrder' (1200, Id). */
    private List<Map<String, Object>> weightRows(UserAccount u, int id) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : db.rows("Sp_InvDeliveryOrder_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", REF_DOC_TYPE,
                "Id", id, "Activity", "GetTotalWeightFromDeliveryOrder"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("CompanyName", str(r.get("CompanyName")));
            o.put("DoTotalQty", toDouble(r.get("DoTotalQty")));
            o.put("VehicleNo", str(r.get("VehicleNo")));
            o.put("VehicleType", str(r.get("VehicleType")));
            o.put("TotalWeight", toDouble(r.get("TotalWeight")));
            out.add(o);
        }
        return out;
    }

    // ================================================================================= history

    /** gridhistory :994 — GatePassOutward.GatepassHistory → 'ReadByGatePassHistory'. */
    public List<Map<String, Object>> history(Map<String, Object> f) {
        UserAccount u = ctx.requireAccountingUser();
        boolean viewAll = rights.has(SCREEN, "viewAll");
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", u.getBranchesId(), "FinancialYearId", ctx.currentFinancialYearId(), "DocumentTypeId", DOC_TYPE,
                "CanViewAllRecord", viewAll);
        if (!viewAll) p.put("EntryUser", u.getId());
        String dateType = str(f.get("dateType"));
        Timestamp from = Inv42Support.ts(f.get("fromDate")), to = Inv42Support.ts(f.get("toDate"));
        String fk = "DateFrom", tk = "DateTo";
        if ("entry".equals(dateType)) { fk = "EntryFromDate"; tk = "EntryToDate"; }
        else if ("modify".equals(dateType)) { fk = "ModifyFromDate"; tk = "ModifyToDate"; }
        if (from != null) p.put(fk, from);
        if (to != null) p.put(tk, to);
        if (toInt(f.get("fromNo")) != 0) p.put("GpSrNoFrom", toInt(f.get("fromNo")));
        if (toInt(f.get("toNo")) != 0) p.put("GpSrNoTo", toInt(f.get("toNo")));
        if (toInt(f.get("supplierCustomerId")) != 0) p.put("SupplierCustomerId", toInt(f.get("supplierCustomerId")));
        p.put("Activity", "ReadByGatePassHistory");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : db.rows("Sp_GatePassOutward_GetAllMethod", p)) {
            Map<String, Object> o = new LinkedHashMap<>();        // dt :1051-1075
            o.put("Id", toInt(r.get("Id")));
            o.put("GatepassType", str(r.get("GatepassType")));
            o.put("OrderType", str(r.get("OrderType")));
            o.put("GpSrNo", toInt(r.get("GpSrNo")));
            o.put("GpDate", day(r.get("GpDate")));
            o.put("DeliveryOrderNo", str(r.get("DeliveryOrderNo")));
            o.put("SupplierCustomerId", toInt(r.get("SupplierCustomerId")));
            o.put("CustomerName", str(r.get("CompanyName")));
            o.put("ReferencePartyName", str(r.get("ReferencePartyName")));
            o.put("TransporterName", str(r.get("Transporter")));
            o.put("Freight", toDouble(r.get("Freight")));
            o.put("NetPaid", toDouble(r.get("NetPaid")));
            o.put("VehicleType", str(r.get("VehicleType")));
            o.put("VehicleNo", str(r.get("VehicleNo")));
            o.put("BiltyNo", str(r.get("BiltyNo")));
            o.put("ItemQty", toDouble(r.get("ItemQty")));
            o.put("InTime", dateTime(r.get("InTime")));
            o.put("OutTime", dateTime(r.get("OutTime")));
            o.put("Status", str(r.get("Status")));
            o.put("EntryUser", str(r.get("UserName")));
            o.put("EntryDate", dateTime(r.get("EntryDate")));
            o.put("ModifyUser", str(r.get("ModifyUserName")));
            o.put("ModifyDate", dateTime(r.get("ModifyDate")));
            o.put("OtherRemarks", str(r.get("OtherRemarks")));
            o.put("NoOfAttachments", toInt(r.get("NoOfAttachments")));
            out.add(o);
        }
        return out;
    }

    // ==================================================================================== read

    /** ReadById :855 — GatePassOutward.GetByID + the customer rows of its delivery order (:877). */
    public Map<String, Object> load(int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> g = ownedHeader(u, id);
        if (g == null) return null;
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", toInt(g.get("Id")));
        out.put("GpSrNo", toInt(g.get("GpSrNo")));
        out.put("GpDate", day(g.get("GpDate")));
        out.put("GatepassType", str(g.get("GatepassType")));
        out.put("GpTypeSrNo", toInt(g.get("GpTypeSrNo")));
        out.put("OtherSupCust", str(g.get("OtherSupCust")));
        out.put("SupplierContractCode", str(g.get("SupplierContractCode")));
        out.put("SaleOrderId", toInt(g.get("SaleOrderId")));
        out.put("SupplierCustomerId", toInt(g.get("SupplierCustomerId")));
        out.put("ReferencePartyId", toInt(g.get("ReferencePartyId")));
        out.put("OtherRemarks", str(g.get("OtherRemarks")));
        out.put("TransporterId", toInt(g.get("TransporterId")));
        out.put("Freight", toDouble(g.get("Freight")));
        out.put("NetPaid", toDouble(g.get("NetPaid")));
        out.put("VehicleType", str(g.get("VehicleType")));
        out.put("VehicleNo", str(g.get("VehicleNo")));
        out.put("BiltyNo", str(g.get("BiltyNo")));
        out.put("NoOfPackages", toInt(g.get("NoOfPackages")));
        out.put("InDateTimeStamp", dateTime(g.get("InDateTimeStamp")));
        out.put("IsApproved", truthy(g.get("IsApproved")));
        out.put("Status", str(g.get("Status")));
        int soId = toInt(g.get("SaleOrderId"));
        out.put("customers", soId > 0 ? weightRows(u, soId) : new ArrayList<Map<String, Object>>());
        return out;
    }

    private Map<String, Object> ownedHeader(UserAccount u, int id) {
        if (id <= 0) return null;
        List<Map<String, Object>> r = db.rows("Sp_GatePassOutward_GetAllMethod", params("Id", id, "Activity", "ReadById"));
        if (r.size() != 1) return null;
        Map<String, Object> g = r.get(0);
        if (toInt(g.get("DocumentTypeId")) != DOC_TYPE) return null;
        if (toInt(g.get("CompanyId")) != toInt(u.getCompanyId())) return null;
        if (toInt(g.get("OrganizationId")) != toInt(u.getOrganizationId())) return null;
        return g;
    }

    // ==================================================================================== save

    /** Insert :765 — formvalidation, "Status should be Open in Case of Save", the GatePassOutward object, GatePassOutward.Save. */
    @Transactional
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = ctx.requireAccountingUser();
        int id = toInt(b.get("Id"));
        if (id == 0 && !rights.has(SCREEN, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (id != 0 && !rights.has(SCREEN, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");
        Map<String, Object> existing = null;
        if (id > 0) {
            existing = ownedHeader(u, id);
            if (existing == null) throw new IllegalArgumentException("Record not update because Id not found");           // :845
        }
        int gpSrNo = existing != null ? toInt(existing.get("GpSrNo")) : gpNo(u);
        String status = str(b.get("Status")).trim();
        /* formvalidation :733 — FormHelper.ValidateControls(txtgpno INT, cmbgptype INT, CmbOrderno INT, CmbCustomer INT,
           cmbvehicletype INT, txtvehicleno String, txtqty String, CmbStatus INT). */
        if (gpSrNo <= 0) throw new IllegalArgumentException("Gate Pass No Field Required");
        if (toInt(b.get("GatePassTypeId")) <= 0 || str(b.get("GatepassType")).trim().isEmpty()) throw new IllegalArgumentException("GatePass Type Field Required");
        if (toInt(b.get("OrderNo")) <= 0) throw new IllegalArgumentException("Delivery Order # Field Required");
        if (toInt(b.get("SupplierCustomerId")) <= 0) throw new IllegalArgumentException("Customer Name Field Required");
        if (toInt(b.get("VehicleTypeId")) <= 0) throw new IllegalArgumentException("Vehicle Type Field Required");
        if (str(b.get("VehicleNo")).trim().isEmpty()) throw new IllegalArgumentException("Vehicle No Field Required");
        if (str(b.get("Qty")).trim().isEmpty()) throw new IllegalArgumentException("Qty Field Required");
        if (status.isEmpty()) throw new IllegalArgumentException("Status Field Required");
        if (!"Open".equals(status) && id == 0) throw new IllegalArgumentException("Status should be Open in Case of Save");  // :777
        if (str(b.get("OtherSupCust")).trim().isEmpty()) throw new IllegalArgumentException("Order Type Field Required");     // :504
        int soId = toInt(b.get("SaleOrderId"));
        if (soId <= 0) throw new IllegalArgumentException("DeliveryOrder you entered has no Existence!");

        Map<String, Object> g = new LinkedHashMap<>();                                                                   // :782-812
        g.put("OrganizationId", u.getOrganizationId());
        g.put("CompanyId", u.getCompanyId());
        g.put("BranchesId", u.getBranchesId());
        g.put("FinancialYearId", ctx.currentFinancialYearId());
        g.put("DocumentTypeId", DOC_TYPE);
        g.put("RefDocumentTypeId", REF_DOC_TYPE);
        g.put("Id", id);
        g.put("EntryUser", u.getId());
        g.put("ModifyUser", u.getId());
        g.put("EntryDate", existing == null ? Inv42Support.now() : existing.get("EntryDate"));
        g.put("ModifyDate", Inv42Support.now());
        g.put("IsApproved", existing != null && truthy(existing.get("IsApproved")));
        g.put("GpSrNo", gpSrNo);
        g.put("GpDate", Inv42Support.docDate(b.get("GpDate")));
        g.put("GatepassType", str(b.get("GatepassType")));
        g.put("GpTypeSrNo", toInt(b.get("GpTypeSrNo")));
        g.put("OtherSupCust", str(b.get("OtherSupCust")).trim());
        g.put("SaleOrderId", soId);
        g.put("SupplierContractCode", str(b.get("OrderNo")).trim());
        g.put("SupplierCustomerId", toInt(b.get("SupplierCustomerId")));
        g.put("ReferencePartyId", toInt(b.get("ReferencePartyId")));
        g.put("OtherRemarks", str(b.get("OtherRemarks")).trim());
        g.put("TransporterId", toInt(b.get("TransporterId")));
        g.put("Freight", toDouble(b.get("Freight")));
        g.put("NetPaid", toDouble(b.get("NetPaid")));
        g.put("VehicleType", str(b.get("VehicleType")).trim());
        g.put("VehicleNo", str(b.get("VehicleNo")).trim());
        g.put("BiltyNo", str(b.get("BiltyNo")).trim());
        g.put("NoOfPackages", (int) Math.rint(toDouble(b.get("Qty"))));
        Timestamp in = Inv42Support.ts(b.get("InDateTimeStamp")), outT = Inv42Support.ts(b.get("OutDateTimeStamp"));
        g.put("InDateTimeStamp", in == null ? Inv42Support.now() : in);
        g.put("OutDateTimeStamp", outT == null ? Inv42Support.now() : outT);
        g.put("Status", status);
        g.put("AttachmentsValues", existing == null ? "" : str(existing.get("AttachmentsValues")));
        g.put("CustomAttachmentsValues", existing == null ? "" : str(existing.get("CustomAttachmentsValues")));
        for (String z : new String[] { "RefDocumentEntryNo", "PostUser", "PostState", "CityId", "WeighBridgeId", "WarehouseId", "ItemId",
                "ExportReturnGrnId", "GdnId" }) g.put(z, 0);
        for (String z : new String[] { "SupplierWeight", "FactoryWeight", "DifferenceWeight", "PackUnit", "WeightCommapredToSoWt" }) g.put(z, 0d);
        for (String z : new String[] { "DocAttachment", "VarietyName", "Container", "Container1", "SealNo", "SealNo1", "DriverName", "DriverCellNo",
                "DriverCNIC", "WeightDiffRemarks" }) g.put(z, "");

        int num = db.setProc(id == 0 ? "Sp_GatePassOutward_Insert" : "Sp_GatePassOutward_Update", g);
        if (num <= 0) num = id;
        if (num <= 0) throw new RuntimeException("Save returned no record id.");
        String msg = (id > 0 ? "Data Update Successfully....  " : "Data Save Successfully....  ") + gpSrNo;          // :819 / :824
        return Inv42Support.ok(msg, num, gpSrNo);
    }
}
