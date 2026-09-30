package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.PurchaseReportsRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.*;
import java.util.stream.Collectors;
import org.springframework.dao.DataAccessException;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

/**
 * Purchase Reports (App 3, module 52). One section per desktop form; the page receives the rows the
 * desktop grid is built from (desktop column order and names) and the raw procedure rows (the "dt" the
 * desktop hands to ShowReportWithDataTable), so its prints go through /reports/print/grid with exactly
 * the desktop's data.
 *
 *   477 Inventory_Reports.frmGRNHistory            "Grn Report"
 *   478 Inventory_Reports.frmGatePassReport        "Gate Pass Report"
 *   479 InventoryReports.PurchaseOrderHistory      "Purchase Order Report"
 *   480 Inventory_Reports.PurchaseRegisterNew      "Purchase Report (With Activites)"
 *   869 SupplierPortal.Reports.frmSupplierDispatchPreBillReport "Stock In Transit Report"
 *
 * Tenancy (organization / company / user / financial year) always comes from the session. The report
 * forms themselves check no rights beyond opening the screen; the web requires the screen's View grant.
 */
@Service
public class PurchaseReportService {
    public static final int GRN_SCREEN = 477, GATE_PASS_SCREEN = 478, PO_SCREEN = 479, REGISTER_SCREEN = 480, TRANSIT_SCREEN = 869;
    private static final int PURCHASE_ORDER_SCREEN = 120;   // "PurchsaeOrder" (frmGRNHistory.cs:574 PoNo link)
    private static final DateTimeFormatter STAMP = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss");

    private final PurchaseReportsRepository repo;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;
    private final SaleReportApprovalService approvals;
    private final DesktopAttachmentStore attachmentStore;

    public PurchaseReportService(PurchaseReportsRepository repo, CurrentUserContext context, DesktopReportRights rights,
                                 SaleReportApprovalService approvals, DesktopAttachmentStore attachmentStore) {
        this.repo = repo; this.context = context; this.rights = rights; this.approvals = approvals; this.attachmentStore = attachmentStore;
    }

    /** CommonServices.DateType() - the fixed five rows (CommonServices.cs:15850). */
    private static final List<Map<String, Object>> DATE_TYPES = List.of(
            Map.of("Id", 1, "Parameters", "This Day"), Map.of("Id", 2, "Parameters", "This Week"),
            Map.of("Id", 3, "Parameters", "This Month"), Map.of("Id", 4, "Parameters", "This Year"),
            Map.of("Id", 5, "Parameters", "Financial Year"));

    private UserAccount user(int screen) {
        UserAccount u = context.requireAccountingUser();
        rights.require(u, screen, "View");
        return u;
    }

    // =====================================================================================
    // 477 frmGRNHistory
    // =====================================================================================

    private static final String BRANCH_GRN = "[dbo].[USP_GetBranchsAllocatedToUserFromGrn]";

    /** frmGRNHistory_Load:162 - AllDropDownBind, ParameterFill, BranchesFill (GetBranchesAllocatedToUserFromGrn, 46). */
    public Map<String, Object> grnInit() {
        UserAccount u = user(GRN_SCREEN);
        var branches = repo.branches(BRANCH_GRN, u, 46);
        Map<String, Object> out = init(u, branches);
        // Load calls AllDropDownBind BEFORE BranchesFill (:170-172): the first lists are for all branches.
        out.put("lookups", plainRows(repo.grnLookups(u, "")));
        // PoNo becomes a link only with the view right on "PurchsaeOrder" (frmGRNHistory.cs:572-579).
        out.put("poLink", hasView(u, PURCHASE_ORDER_SCREEN));
        return out;
    }

    public List<Map<String, Object>> grnLookups(List<Integer> branchIds) {
        UserAccount u = user(GRN_SCREEN);
        return plainRows(repo.grnLookups(u, branchIds(u, repo.branches(BRANCH_GRN, u, 46), branchIds)));
    }

    /** gridHisory:341. */
    public Map<String, Object> grnRows(Map<String, Object> body) {
        UserAccount u = user(GRN_SCREEN);
        Map<String, Object> f = grnFilter(u, body);
        List<Map<String, Object>> raw = repo.grnHistory(u, f);
        List<Map<String, Object>> rows = new ArrayList<>();
        for (var r : raw) {
            int id = num(r.get("Id"));
            // Freight is shared across a GRN's lines in proportion to NetBillWeight (:437-451).
            double totalFreight = raw.stream().filter(x -> num(x.get("Id")) == id).findFirst().map(x -> dbl(x.get("FreightAmount"))).orElse(0.0);
            double totalWeight = raw.stream().filter(x -> num(x.get("Id")) == id).mapToDouble(x -> dbl(x.get("NetBillWeight"))).sum();
            double freight = totalWeight != 0 ? dbl(r.get("NetBillWeight")) / totalWeight * totalFreight : 0.0;
            Map<String, Object> row = project(r, GRN_COLUMNS);
            row.put("Freight", freight);
            rows.add(row);
        }
        return result(rows, raw);
    }

    /** 335_01 Register (toolStripButton1_Click:1082) prints usp_GrnRegisterSummaryItemWise with the same filters. */
    public Map<String, Object> grnPrintArgs(Map<String, Object> body) {
        UserAccount u = user(GRN_SCREEN);
        Map<String, Object> f = grnFilter(u, body);
        Map<String, Object> args = new LinkedHashMap<>();
        args.put("fromDate", iso(f.get("fromDate")));
        args.put("toDate", iso(f.get("toDate")));
        for (String[] m : new String[][]{{"supplierId", "supplierCustomerId"}, {"gpFrom", "gpSrNoF"}, {"gpTo", "gpSrNoT"},
                {"fromNo", "fromDocNo"}, {"toNo", "toDocNo"}, {"warehouseId", "warehouseId"}, {"jobLotId", "jobLotId"},
                {"itemId", "itemId"}, {"parentCategoryId", "inventoryParentCategories"}}) {
            if (num(f.get(m[0])) != 0) args.put(m[1], num(f.get(m[0])));
        }
        for (String[] m : new String[][]{{"areaCity", "areaCity"}, {"branchesIds", "branchesIds"}, {"vehicleNo", "vehicleNo"}}) {
            if (!str(f.get(m[0])).isEmpty()) args.put(m[1], str(f.get(m[0])));
        }
        return args;
    }

    private Map<String, Object> grnFilter(UserAccount u, Map<String, Object> b) {
        Map<String, Object> f = new LinkedHashMap<>();
        f.put("branchesIds", branchIds(u, repo.branches(BRANCH_GRN, u, 46), ints(b.get("branchIds"))));
        f.put("fromDate", required(b.get("fromDate")));
        f.put("toDate", required(b.get("toDate")));
        for (String k : List.of("fromNo", "toNo", "gpFrom", "gpTo", "supplierId", "warehouseId", "parentCategoryId", "itemId", "jobLotId"))
            f.put(k, num(b.get(k)));
        f.put("vehicleNo", str(b.get("vehicleNo")));
        // AreaCity is CmbCityName.Text, sent only when a city is selected (:363-366).
        if (num(b.get("cityId")) > 0) f.put("areaCity", str(b.get("cityName")));
        return f;
    }

    /** dtgrid of gridHisory (:385-452), in the desktop's column order. */
    private static final String[][] GRN_COLUMNS = {
            {"Id", "Id"}, {"DocumentTypeId", "DocumentTypeId"}, {"BranchName", "BranchName"}, {"DocDate", "DocDate"},
            {"DocNo", "DocNo"}, {"InvoiceNo", "InvoiceNo"}, {"SupplierName", "CompanyName"}, {"GpDate", "GpDate"},
            {"GpNo", "GpNo"}, {"WareHouse", "WareHouseName"}, {"ParentCategory", "ParentCategoryDescription"},
            {"Item", "ItemName"}, {"CropYear", "CropYear"}, {"JobLot", "JobLotDescription"}, {"UOM", "UOMCode"},
            {"ItemQty", "ItemQty"}, {"VehicleNo", "VehicleNo"}, {"BiltyNo", "BiltyNo"}, {"InwardGatePassId", "InwardGatePassId"},
            {"WagesId", "WagesId"}, {"WagesNo", "WagesNo"}, {"OrderId", "OrderId"}, {"PoDate", "PoDate"}, {"PoNo", "PoNo"},
            {"PackingType", "PackTypeDesc"}, {"GrossWeight", "GrossWeight"}, {"EB/Unit", "EBWPerUnit"}, {"EBWTotal", "EBWTotal"},
            {"EbPurAgainstWeight", "EbPurAgainstWeight"}, {"AdLsWeight", "AdLsWeight"}, {"WtCut", "WtCut"}, {"WtCutTotal", "WtCutTotal"},
            // :452 fills ScaleShortWt/ScaleShortWtApply from the SUPPLIER columns and SuppShortWt/SuppShortWtApply
            // from the SCALE columns - reproduced as the desktop shows them.
            {"ScaleShortWt", "SupplierShortWeight"}, {"ScaleShortWtApply", "SupplierShortWeightApply", "bool"},
            {"SuppShortWt", "ScaleShortWeight"}, {"SuppShortWtApply", "ScaleShortWeightApply", "bool"},
            {"NetBillWeight", "NetBillWeight"}, {"StockEbUnit", "StockEbUnit"}, {"StockEbTotal", "StockEbTotal"},
            {"StockWeight", "StockWeight"}, {"CityName", "AreaCity"}, {"Transporter", "TransporterName"}, {"Freight", "FreightAmount"},
            {"EntryDate", "EntryDate"}, {"EntryUser", "UserNameEusr"}, {"ModifyDate", "ModifyDate"}, {"ModifyUser", "UserNameMusr"},
            {"NoOfAttachments", "NoOfAttachments"}};

    // =====================================================================================
    // 478 frmGatePassReport
    // =====================================================================================

    private static final String BRANCH_GPI = "[dbo].[USP_GetBranchsAllocatedToUserFromGatePassInward]";

    /** frmGatePassReport_Load:122 - ALlDropDown, IsAcceptFill, BranchesFill (51), Datetypefill. */
    public Map<String, Object> gatePassInit() {
        UserAccount u = user(GATE_PASS_SCREEN);
        var branches = repo.branches(BRANCH_GPI, u, 51);
        Map<String, Object> out = init(u, branches);
        // Load calls ALlDropDown BEFORE BranchesFill (:131-133): the first lists are for all branches.
        out.put("lookups", plainRows(repo.gatePassLookups(u, "")));
        out.put("statuses", List.of(Map.of("Id", 1, "Status", "Open"), Map.of("Id", 2, "Status", "Accepted"), Map.of("Id", 3, "Status", "Rejected")));
        return out;
    }

    public List<Map<String, Object>> gatePassLookups(List<Integer> branchIds) {
        UserAccount u = user(GATE_PASS_SCREEN);
        return plainRows(repo.gatePassLookups(u, branchIds(u, repo.branches(BRANCH_GPI, u, 51), branchIds)));
    }

    /** GridFill:320. */
    public Map<String, Object> gatePassRows(Map<String, Object> b) {
        UserAccount u = user(GATE_PASS_SCREEN);
        Map<String, Object> f = new LinkedHashMap<>();
        f.put("branchesIds", branchIds(u, repo.branches(BRANCH_GPI, u, 51), ints(b.get("branchIds"))));
        f.put("fromDate", required(b.get("fromDate")));
        f.put("toDate", required(b.get("toDate")));
        f.put("fromNo", num(b.get("fromNo")));
        f.put("toNo", num(b.get("toNo")));
        f.put("supplierId", num(b.get("supplierId")));
        f.put("status", str(b.get("status")));                          // cmbIsAccept.Text, always (:334)
        if (num(b.get("gatePassTypeId")) > 0) f.put("gatePassType", str(b.get("gatePassType")));   // cmbgptype.Text (:335-338)
        var raw = repo.gatePassHistory(u, f);
        return result(raw.stream().map(r -> project(r, GATE_PASS_COLUMNS)).toList(), raw);
    }

    /** dtTable of GridFill (:355-386). */
    private static final String[][] GATE_PASS_COLUMNS = {
            {"Id", "Id"}, {"BranchName", "BranchName"}, {"GpSrNo", "GpSrNo"}, {"GpDate", "GpDate", "date"},
            {"GatePassType", "GatepassType"}, {"OrderType", "OrderType"}, {"OrderNo", "OrderNo"}, {"SupplierName", "CompanyName"},
            {"ItemName", "VarietyName"}, {"ItemQty", "ItemQty"}, {"InTime", "InTime"}, {"OutTime", "OutTime"},
            {"Freight", "Freight"}, {"NetPaid", "NetPaid"}, {"SupplierFirstWeight", "SupplierFirstWeight"},
            {"SupplierSecondWeight", "SupplierSecondWeight"}, {"SupplierWeight", "SupplierWeight"}, {"FactoryWeight", "FactoryWeight"},
            {"DifferenceWeight", "DifferenceWeight"}, {"EntryUser", "UserName"}, {"Status", "Status"}, {"CityName", "Description"},
            {"VehicleType", "VehicleType"}, {"VehicleNo", "VehicleNo"}, {"BiltyNo", "BiltyNo"}, {"Remarks", "OtherRemarks"},
            {"NoOfAttachments", "NoOfAttachments"}};

    // =====================================================================================
    // 479 PurchaseOrderHistory
    // =====================================================================================

    private static final String BRANCH_PO = "[dbo].[USP_GetBranchsAllocatedToUserFromPurchaseOrder]";
    private static final Map<String, String> PO_ACTION_RIGHTS = Map.of(
            "Complete", "CanChangeOrderStatusToComplete", "Cancel", "CanChangeOrderStatusToCancel",
            "Open", "CanChangeOrderStatusToOpen", "UpdateExpiryDate", "CanChangeOrderExpiryDate");

    /** InitializeComponentMethod:390 - rights, parent categories, drop-downs, branches (41), config flag. */
    public Map<String, Object> purchaseOrderInit() {
        UserAccount u = user(PO_SCREEN);
        var branches = repo.branches(BRANCH_PO, u, 41);
        Map<String, Object> out = init(u, branches);
        // GetDropDownData runs before BranchesFill has set the branch text (:398-423): all branches.
        out.put("lookups", plainRows(repo.purchaseOrderLookups(u, "")));
        out.put("parentCategories", plainRows(repo.purchaseOrderParentCategories(u)));
        out.put("rights", poRights(u));
        out.put("remarksNotRequired", remarksNotRequired(u));
        out.put("approvalPolicy", repo.documentsInApprovalPolicy(u).contains(41));
        out.put("reportTypes", List.of("Order Register", "Order Summary By Item", "Order Summary By Supplier", "Order Summary By Supplier & Item"));
        return out;
    }

    public List<Map<String, Object>> purchaseOrderLookups(List<Integer> branchIds) {
        UserAccount u = user(PO_SCREEN);
        return plainRows(repo.purchaseOrderLookups(u, branchIds(u, repo.branches(BRANCH_PO, u, 41), branchIds)));
    }

    /** FilterData:989 / FilterDataSummary:1491 - the five status panels (USP_GetOrdersStatus rows 0-4). */
    public List<Double> purchaseOrderStatus() {
        UserAccount u = user(PO_SCREEN);
        var rows = repo.orderStatus(u);
        if (rows.isEmpty()) return List.of();
        List<Double> out = new ArrayList<>();
        for (int i = 0; i < 5; i++) {
            if (i >= rows.size()) throw new IllegalArgumentException("There is no row at position " + i + ".");
            out.add(dbl(rows.get(i).get("OrderQty")));
        }
        return out;
    }

    /** GetDetailData:822 + BindDetailData:918. */
    public Map<String, Object> purchaseOrderDetail(Map<String, Object> b) {
        UserAccount u = user(PO_SCREEN);
        var raw = repo.purchaseOrderHistory(u, poDetailFilter(u, b));
        return result(raw.stream().map(r -> project(r, PO_DETAIL_COLUMNS)).toList(), raw);
    }

    private Map<String, Object> poDetailFilter(UserAccount u, Map<String, Object> b) {
        Map<String, Object> f = new LinkedHashMap<>();
        f.put("branchesIds", branchIds(u, repo.branches(BRANCH_PO, u, 41), ints(b.get("branchIds"))));
        f.put("fromDate", date(b.get("fromDate")));                     // only when the picker is checked (:835-842)
        f.put("toDate", date(b.get("toDate")));
        for (String k : List.of("fromNo", "toNo", "supplierId", "bookingPersonId", "parentCategoryId", "itemId", "activityId"))
            f.put(k, num(b.get(k)));
        f.put("status", status(b.get("status")));
        f.put("approval", approval(b.get("approval")));
        return f;
    }

    /** GridSummaryFill:1549. */
    public Map<String, Object> purchaseOrderSummary(Map<String, Object> b) {
        UserAccount u = user(PO_SCREEN);
        String type = str(b.get("reportType"));
        if (!List.of("Order Register", "Order Summary By Item", "Order Summary By Supplier", "Order Summary By Supplier & Item").contains(type))
            throw new IllegalArgumentException("Please Select Activity First...");
        Map<String, Object> f = new LinkedHashMap<>();
        f.put("branchesIds", branchIds(u, repo.branches(BRANCH_PO, u, 41), ints(b.get("branchIds"))));
        f.put("fromDate", date(b.get("fromDate")));
        f.put("toDate", date(b.get("toDate")));
        for (String k : List.of("fromNo", "toNo", "parentCategoryId", "itemCategoryId", "itemTypeId", "supplierId", "bookingPersonId",
                "itemId", "jobLotId", "cityId", "districtId", "activityId")) f.put(k, num(b.get(k)));
        // CropYear is the combo's text, only when a row is active and its value > 0 (:1568).
        if (num(b.get("cropYearId")) > 0) f.put("cropYear", str(b.get("cropYear")));
        f.put("reportType", type);
        f.put("status", status(b.get("status")));
        f.put("approval", approval(b.get("approval")));
        f.put("packUom", bool(b.get("packUom")));
        f.put("cityWise", bool(b.get("cityWise")));
        f.put("skipZero", bool(b.get("skipZero")));
        var raw = repo.purchaseOrderSummary(u, f);
        List<Map<String, Object>> rows = new ArrayList<>();
        for (var r : raw) {
            Map<String, Object> row = project(r, PO_SUMMARY_COLUMNS);
            if (!"Order Register".equals(type)) {
                // the three summaries fill Id/DocumentTypeId/DocType/DocDate/DocNo with 0,0,"",Now,0 (:1675-1697)
                row.put("Id", 0); row.put("DocumentTypeId", 0); row.put("DocType", ""); row.put("DocDate", STAMP.format(LocalDateTime.now())); row.put("DocNo", 0);
                row.put("BookingPerson", ""); row.put("CropYear", ""); row.put("JobLot", "");
                if (!"Order Summary By Supplier".equals(type) && !"Order Summary By Supplier & Item".equals(type)) row.put("Supplier", "");
                if ("Order Summary By Supplier".equals(type)) { row.put("ItemCode", ""); row.put("ItemName", ""); }
            }
            rows.add(row);
        }
        return result(rows, raw);
    }

    /**
     * Complete / Open / Cancel / UpdateExpiryDate - the row buttons (CompleteStatus:1993, OpenStatus:2044,
     * CancelStatus:2095, UpdateExpiryStatus:2142) and the toolbar's multi-row Open / Complete / Cancel
     * (btnOpenOrders_Click:2780 ...). The selection is checked against a fresh run of the same report.
     */
    public Map<String, Object> purchaseOrderAction(Map<String, Object> b) {
        UserAccount u = user(PO_SCREEN);
        String action = str(b.get("action"));
        if (!PO_ACTION_RIGHTS.containsKey(action)) throw new IllegalArgumentException("Unknown order action");
        boolean bulk = bool(b.get("bulk"));
        if (bulk && "UpdateExpiryDate".equals(action)) throw new IllegalArgumentException("Unknown order action");
        @SuppressWarnings("unchecked")
        Map<String, Object> filterBody = b.get("filter") instanceof Map<?, ?> m ? (Map<String, Object>) m : Map.of();
        Map<String, Object> f = poDetailFilter(u, filterBody);
        Set<String> granted = repo.grantedRights(u, PO_SCREEN);
        String status = str(f.get("status"));
        boolean approved = "Approved".equals(f.get("approval"));
        // GridSettings:1280-1319 - which buttons exist for the filters of the last Show.
        boolean visible = "Open".equals(action)
                ? approved && !"Open".equals(status) && !status.isEmpty()
                : approved && "Open".equals(status);
        if (!visible || !granted.contains(PO_ACTION_RIGHTS.get(action)))
            throw new AccessDeniedException("The user does not have rights for this order action");

        @SuppressWarnings("unchecked")
        List<Map<String, Object>> selections = b.get("selections") instanceof List<?> l ? (List<Map<String, Object>>) l : List.of();
        if (selections.isEmpty()) {
            if (bulk) throw new IllegalArgumentException("Please Check Rows To Proceed!");
            throw new IllegalArgumentException("Record Not Found");
        }
        if (!bulk && selections.size() != 1) throw new IllegalArgumentException("Record Not Found");
        String bulkRemarks = str(b.get("remarks"));
        if (bulk && bulkRemarks.isEmpty()) throw new IllegalArgumentException("Action Remarks Required");   // RemarksPopUp (:2804)

        Set<Integer> reported = repo.purchaseOrderHistory(u, f).stream().map(r -> num(r.get("Id"))).collect(Collectors.toSet());
        boolean remarksOptional = remarksNotRequired(u);
        List<Object[]> work = new ArrayList<>();
        for (var s : selections) {
            int id = num(s.get("id"));
            if (id <= 0 || !reported.contains(id)) throw new IllegalArgumentException("The report changed. Press Show before updating an order");
            String remarks = bulk ? bulkRemarks : str(s.get("remarks"));
            if (!bulk && remarks.isEmpty() && !remarksOptional) throw new IllegalArgumentException("Action Remarks Required");
            LocalDateTime expiry = null;
            if ("UpdateExpiryDate".equals(action)) {
                expiry = date(s.get("expiryDate"));
                if (expiry == null) throw new IllegalArgumentException("Expiry Date is required");
            }
            work.add(new Object[]{id, remarks, expiry});
        }
        String reqType = "Complete".equals(action) ? "Status" : action;
        int done = 0;
        try {
            for (Object[] w : work) {
                repo.updateOrderStatus(u, (Integer) w[0], reqType, (String) w[1], (LocalDateTime) w[2]);
                done++;
            }
        } catch (DataAccessException e) {
            // UpdateStatusandIsApprovedbyOrderId runs one procedure call per order without a transaction.
            throw new IllegalStateException("The order action failed after " + done + " completed orders. Press Show before trying again.", e);
        }
        String message;
        if (bulk) message = switch (action) {
            case "Open" -> "Orders Open Successfully";
            case "Complete" -> "Orders Completed Successfully";
            default -> "Orders Cancel Successfully";
        };
        else message = switch (action) {
            case "Open" -> "Record Open Successfully";
            case "Complete" -> "Record Complete Successfully";
            case "Cancel" -> "Record Cancel Successfully";
            default -> "Update ExpiryDate Successfully";
        };
        return Map.of("message", message, "processed", done);
    }

    /** Approval Detail (frmApprovalCommentory) of a row of the last Show - Detail grid or the Order Register summary. */
    @SuppressWarnings("unchecked")
    public Map<String, Object> purchaseOrderApproval(Map<String, Object> b) {
        UserAccount u = user(PO_SCREEN);
        Map<String, Object> filterBody = b.get("filter") instanceof Map<?, ?> m ? (Map<String, Object>) m : Map.of();
        List<Map<String, Object>> rows = bool(b.get("summary"))
                ? (List<Map<String, Object>>) purchaseOrderSummary(filterBody).get("raw")
                : repo.purchaseOrderHistory(u, poDetailFilter(u, filterBody));
        return approvals.history(Map.of("rows", rows), "rows", num(b.get("documentTypeId")), num(b.get("id")));
    }

    private Map<String, Boolean> poRights(UserAccount u) {
        Set<String> granted = repo.grantedRights(u, PO_SCREEN);
        Map<String, Boolean> out = new LinkedHashMap<>();
        for (String r : PO_ACTION_RIGHTS.values()) out.put(r, granted.contains(r));
        return out;
    }

    private boolean remarksNotRequired(UserAccount u) {
        String v = repo.configuration(u, "ActionRemarksRequiredNotOnPurchaseOrderStatusUpdatation").trim();
        return "true".equalsIgnoreCase(v) || "1".equals(v);
    }

    /** data of BindDetailData (:926-971). */
    private static final String[][] PO_DETAIL_COLUMNS = {
            {"Id", "Id"}, {"DocumentTypeId", "DocumentTypeId"}, {"BranchName", "BranchName"}, {"ItemCategory", "ItemCategory"},
            {"ItemType", "TypeDescription"}, {"DocDate", "DocDate", "date"}, {"DocNo", "DocNo"}, {"OrderSupCustId", "OrderSupCustId"},
            {"SupplierName", "SupplierName"}, {"BookingPerson", "BookingPerson"}, {"CommissionAgent", "CompanyNameSpCAgent"},
            {"DeliveryTerm", "DeliveryTerm"}, {"DeliveryDays", "DeliveryDays"}, {"DueDate", "OrderDueDate"},
            {"PurchaseGLAC", "PurchaseGLAC"}, {"ItemId", "OrderItemId"}, {"ItemName", "ItemName"}, {"Crop", "Crop"},
            {"PackUom", "UOMCodeItm"}, {"Qty", "OrderItemQty"}, {"RcvdQty", "ReceivedQty"}, {"RejQty", "RejQty"},
            {"BalQty", "BalQty"}, {"Weight", "NetWeight"}, {"RcvdWeight", "ReceivedWeight"}, {"BalWeight", "BalWeight"},
            {"Rate", "OrderItemRate"}, {"OrderAmount", "Amount"}, {"ReceivedAmount", "ReceivedAmount"}, {"BalAmount", "BalAmount"},
            {"PPBagRate", "PPBagRate"}, {"JuteBagRate", "JuteBagRate"}, {"PPBagWeight", "PPBagWeight"}, {"JuteBagWeight", "JuteBagWeight"},
            {"ApprovedBy", "UserNameAusr"}, {"ApprovedDate", "PostDate", "date"}, {"ExpiryDate", "OrderExpiryDate", "date"},
            {"ActionRemarks", ""}};

    /** dtOrderSummaryRegister (InitializeComponentCustom:354) filled per report type (:1655-1699). */
    private static final String[][] PO_SUMMARY_COLUMNS = {
            {"Id", "Id"}, {"DocumentTypeId", "DocumentTypeId"}, {"DocType", "DocumentTypeCode"}, {"DocDate", "DocDate", "date"},
            {"DocNo", "DocNo"}, {"BranchName", "BranchName"}, {"ItemCode", "ItemCode"}, {"ItemName", "ItemName"},
            {"PackUom", "UOMCode"}, {"OrderQty", "OrderQty"}, {"RcvdQty", "ReceivedQty"}, {"BalQty", "BalQty"},
            {"OrderWeight", "OrderWeight"}, {"RcvdWeight", "ReceivedWeight"}, {"BalWeight", "BalWeight"},
            {"OrderAmount", "OrderAmount"}, {"RcvdAmount", "ReceivedAmount"}, {"BalAmount", "BalAmount"}, {"AvgRate", "AvgRate"},
            {"PercentOfTotal", "PrcntOfTotal"}, {"CityName", "CityName"}, {"Supplier", "Supplier"}, {"BookingPerson", "BookingPerson"},
            {"CropYear", "CropBatch"}, {"JobLot", "JobLotCode"}};

    // =====================================================================================
    // 480 PurchaseRegisterNew
    // =====================================================================================

    /** frmEvaulationDetailSalesReports_Load:320. */
    public Map<String, Object> registerInit() {
        UserAccount u = user(REGISTER_SCREEN);
        var branches = repo.purchaseInvoiceBranches(u);
        Map<String, Object> out = init(u, branches);
        String ids = defaultBranchIds(u, branches);
        out.put("detailLookups", plainRows(repo.purchaseInvoiceCombos(u, ids)));
        // AllcomboFillFromEvaluation reads the DETAIL tab's branch combo, also for the summary tab (:987).
        out.put("summaryLookups", plainRows(repo.evaluationCombos(u, ids)));
        out.put("customGroups", plainRows(repo.customGroups(u)));
        out.put("activities", List.of("Detail", "Comparison By Item", "Comparison By Item and Supplier"));
        out.put("reportTypes", REGISTER_SUMMARY_TYPES);
        return out;
    }

    public List<Map<String, Object>> registerDetailLookups(List<Integer> branchIds) {
        UserAccount u = user(REGISTER_SCREEN);
        return plainRows(repo.purchaseInvoiceCombos(u, branchIds(u, repo.purchaseInvoiceBranches(u), branchIds)));
    }

    public List<Map<String, Object>> registerSummaryLookups(List<Integer> branchIds) {
        UserAccount u = user(REGISTER_SCREEN);
        return plainRows(repo.evaluationCombos(u, branchIds(u, repo.purchaseInvoiceBranches(u), branchIds)));
    }

    /** GridFill:1226. */
    public Map<String, Object> registerDetail(Map<String, Object> b) {
        UserAccount u = user(REGISTER_SCREEN);
        String activity = str(b.get("reportType"));
        Map<String, Object> f = new LinkedHashMap<>();
        f.put("fromDate", required(b.get("fromDate")));
        f.put("toDate", required(b.get("toDate")));
        for (String k : List.of("fromNo", "toNo", "itemClassId", "itemCategoryId", "itemTypeId", "packingTypeId", "jobLotId",
                "warehouseId", "itemId", "supplierId", "commissionAgentId", "documentTypeId", "stockAccountId")) f.put(k, num(b.get(k)));
        if (num(b.get("cropYearId")) > 0) f.put("cropYear", str(b.get("cropYear")));
        f.put("reportType", activity);
        f.put("parentCategoryIds", idList(b.get("parentCategoryIds")));
        f.put("branchesIds", branchIds(u, repo.purchaseInvoiceBranches(u), ints(b.get("branchIds"))));
        f.put("customGroupIds", idList(b.get("customGroupIds")));
        var raw = repo.purchasingRegister(u, f);
        if (raw.isEmpty()) return result(List.of(), raw);
        if (activity.isEmpty()) throw new IllegalArgumentException("Report Type field required");
        List<Map<String, Object>> rows = new ArrayList<>();
        for (var r : raw) {
            switch (activity) {
                case "Detail" -> {
                    Map<String, Object> row = project(r, REGISTER_DETAIL_COLUMNS);
                    if (num(r.get("DocumentTypeId")) == 59)   // returns are shown negative (:1310)
                        for (String k : List.of("ItemQty", "QtyByParentCategory", "BillWeight", "StockWeight", "BillAmount", "Amount+Exp"))
                            row.put(k, -dbl(row.get(k)));
                    rows.add(row);
                }
                case "Comparison By Item" -> rows.add(project(r, REGISTER_COMPARISON_ITEM));
                case "Comparison By Item and Supplier" -> rows.add(project(r, REGISTER_COMPARISON_ITEM_SUPPLIER));
                default -> { return result(List.of(), raw); }
            }
        }
        return result(rows, raw);
    }

    /** GridSummaryFill:1702. */
    public Map<String, Object> registerSummary(Map<String, Object> b) {
        UserAccount u = user(REGISTER_SCREEN);
        String type = str(b.get("reportType"));
        if (!REGISTER_SUMMARY_TYPES.contains(type)) throw new IllegalArgumentException("Please Select Activity First...");
        Map<String, Object> f = new LinkedHashMap<>();
        f.put("fromDate", required(b.get("fromDate")));
        f.put("toDate", required(b.get("toDate")));
        for (String k : List.of("fromNo", "toNo", "itemClassId", "itemCategoryId", "itemTypeId", "itemId", "jobLotId", "warehouseId",
                "packingTypeId", "districtId", "stockAccountId", "cityId", "supplierId")) f.put(k, num(b.get(k)));
        if (num(b.get("cropYearId")) > 0) f.put("cropYear", str(b.get("cropYear")));
        f.put("cityWise", bool(b.get("cityWise")));
        f.put("packUom", bool(b.get("packUom")));
        f.put("reportType", type);
        f.put("parentCategoryIds", idList(b.get("parentCategoryIds")));
        f.put("branchesIds", branchIds(u, repo.purchaseInvoiceBranches(u), ints(b.get("branchIds"))));
        f.put("customGroupIds", idList(b.get("customGroupIds")));
        var raw = repo.purchaseWithActivities(u, f);
        String[][] spec = REGISTER_SUMMARY_COLUMNS.get(type);
        return result(raw.stream().map(r -> project(r, spec)).toList(), raw);
    }

    private static final List<String> REGISTER_SUMMARY_TYPES = List.of("Purchase Register", "Purchase Summary By Item",
            "Purchase Summary By Item & Warehouse", "Purchase Summary By Supplier", "Purchase Summary By Item & Supplier",
            "Purchase Summary By Parent Category", "Purchase Summary By Parent Category & Supplier", "Purchase Summary By HsCode");

    /** dtDetail (:328-383) filled at :1310. */
    private static final String[][] REGISTER_DETAIL_COLUMNS = {
            {"ParentCategory", "ParentCategory"}, {"TransactionType", "TransactionType"}, {"PurchaseType", "PurchaseType"},
            {"SupplierName", "SupplierName"}, {"Id", "Id"}, {"DocumentTypeId", "DocumentTypeId"}, {"PurchaseOrderId", "PurchaseOrderId"},
            {"PoNo", "PoNo"}, {"BillDate", "BillDate", "date"}, {"BillNo", "BillNo"}, {"BranchSrNo", "BranchSrNo"}, {"ManualNo", "ManualNo"},
            {"WareHouseName", "WareHouseName"}, {"ItemName", "ItemName"}, {"PackUom", "UOMCode"}, {"CropYear", "CropYear"},
            {"JobLot", "JobLotCode"}, {"PackingType", "PackTypeCode"}, {"ItemQty", "ItemQty"}, {"QtyByParentCategory", "QtyByParentCategory"},
            {"BillWeight", "BillWeight"}, {"StockWeight", "StockWeight"}, {"ItemRate", "ItemRate"}, {"RateCut", "RateCut"},
            {"RateCutAmount", "RateCutAmount"}, {"BillAmount", "BillAmount"}, {"Commission", "Commission"}, {"Freight", "Freight"},
            {"Expense", "OExpense"}, {"Wages", "WagesAmount"}, {"EmptyBagsAmount", "EmptyBagsAmount"}, {"EmptyBagsAmount40KG", "EmptyBagsAmount40KG"},
            {"FreightDeduction", "FreightDeduction"}, {"FreightDeduction40KG", "FreightDeduction40KG"}, {"ItemAddLess(JV)", "ItemAddLess_JV"},
            {"TotalExpense", "TotalExpense"}, {"Exp/40kg", "TotalExp40Kg"}, {"Amount+Exp", "TotalAmount"}, {"PartyBillAmount", "PartyBillAmount"},
            {"PartyAddLess_JV", "PartyAddLess_JV"}, {"RateWithExp", "RateWithExp"}, {"StockAvgRateKg", "StockAvgRateKg"},
            {"AvgRatePerKgOnBillWeight", "AvgRatePerKgOnBillWeight"}, {"GpNo", "GpNo"}, {"GpDate", "GpDate", "date"},
            {"GrnDate", "GrnDate", "date"}, {"GrnNo", "GrnNo"}, {"VehicleNo", "VehicleNo"}, {"TicketNos", "TicketNos"},
            {"CommissionAgent", "CommissionAgent"}, {"CommissionType", "CommissionType"}, {"CommRate", "CommRate"},
            {"BrokerName", "BrokerName"}, {"BrokeryType", "BrokeryType"}, {"BrokeryRate", "BrokeryRate"}, {"CityName", "CityName"}};

    /** dtComparisonByItem (:384-409) filled at :1321. */
    private static final String[][] REGISTER_COMPARISON_ITEM = {
            {"ParentCategory", "ParentCategory"}, {"ItemName", "ItemName"}, {"CropYear", "CropYear"}, {"ItemQty", "ItemQty"},
            {"QtyByParentCategory", "QtyByParentCategory"}, {"BillWeight", "BillWeight"}, {"StockWeight", "StockWeight"},
            {"ItemRate", "ItemRate"}, {"BillAmount", "BillAmount"}, {"TotalExpense", "TotalExpense"}, {"Amount+Exp", "TotalAmount"},
            {"RateWithExp", "RateWithExp"}, {"TotalExp40Kg", "TotalExp40Kg"}, {"Freight40Kg", "Freight40Kg"}, {"Comm40Kg", "Comm40Kg"},
            {"Expense40Kg", "OExpense40Kg"}, {"Wages40Kg", "Wages40Kg"}, {"EmptyBagsAmount", "EmptyBagsAmount"},
            {"EmptyBagsAmount40KG", "EmptyBagsAmount40KG"}, {"FreightDeduction", "FreightDeduction"}, {"FreightDeduction40KG", "FreightDeduction40KG"},
            {"Freight", "Freight"}, {"Commission", "Commission"}, {"Expense", "OExpense"}, {"Wages", "WagesAmount"}, {"ItemAddLess(JV)", "ItemAddLess_JV"}};

    /** dtComparisonByItemSupplier (:410-436) filled at :1332. */
    private static final String[][] REGISTER_COMPARISON_ITEM_SUPPLIER = {
            {"ParentCategory", "ParentCategory"}, {"ItemName", "ItemName"}, {"CropYear", "CropYear"}, {"SupplierName", "SupplierName"},
            {"ItemQty", "ItemQty"}, {"QtyByParentCategory", "QtyByParentCategory"}, {"BillWeight", "BillWeight"}, {"StockWeight", "StockWeight"},
            {"ItemRate", "ItemRate"}, {"BillAmount", "BillAmount"}, {"TotalExpense", "TotalExpense"}, {"Amount+Exp", "TotalAmount"},
            {"RateWithExp", "RateWithExp"}, {"TotalExp40Kg", "TotalExp40Kg"}, {"Freight40Kg", "Freight40Kg"}, {"Comm40Kg", "Comm40Kg"},
            {"Expense40Kg", "OExpense40Kg"}, {"Wages40Kg", "Wages40Kg"}, {"EmptyBagsAmount", "EmptyBagsAmount"},
            {"EmptyBagsAmount40KG", "EmptyBagsAmount40KG"}, {"FreightDeduction", "FreightDeduction"}, {"FreightDeduction40KG", "FreightDeduction40KG"},
            {"Freight", "Freight"}, {"Commission", "Commission"}, {"Expense", "OExpense"}, {"Wages", "WagesAmount"}, {"ItemAddLess(JV)", "ItemAddLess_JV"}};

    private static final String[] SUMMARY_TAIL = {"Qty", "GrossWeight", "EbTotal", "AddLess", "BillWeight", "StockWeight", "AvgRate", "Amount",
            "TotalExpenses", "TotalAmountWithExp", "AvgRateExp", "Freight", "Commission", "Expenses", "Wages", "EmptyBagsAmount",
            "FreightDeduction", "PrcntOfTotal", "PrcntOfTotalWeight"};

    private static String[][] summarySpec(String[] head, String[] tail) {
        List<String[]> out = new ArrayList<>();
        for (String h : head) out.add(new String[]{h.split(":")[0], h.contains(":") ? h.split(":")[1] : h, h.equals("DocDate") ? "date" : ""});
        for (String t : SUMMARY_TAIL) out.add(new String[]{t, t});
        for (String t : tail) out.add(new String[]{t, t});
        return out.toArray(new String[0][]);
    }

    /** The eight DataTables of Load (:437-640), filled at :1793-1933. */
    private static final Map<String, String[][]> REGISTER_SUMMARY_COLUMNS = Map.of(
            "Purchase Register", summarySpec(new String[]{"DocDate", "RefDocumentTypeId", "DocumentType:DocumentTypeCode", "DocCodeNo",
                    "PartyName", "ItemCode", "ItemName", "UOMCode", "CropBatch", "JobLotCode", "PackTypeCode", "VehicleNo", "CityName"},
                    new String[]{"ParentCategory", "ItemType", "ItemCategory"}),
            "Purchase Summary By Item", summarySpec(new String[]{"CityName", "ItemCode", "ItemName", "UOMCode"},
                    new String[]{"ParentCategory", "ItemCategory", "ItemType"}),
            "Purchase Summary By Supplier", summarySpec(new String[]{"CityName", "PartyName", "UOMCode"}, new String[]{}),
            "Purchase Summary By Item & Supplier", summarySpec(new String[]{"CityName", "PartyName", "ItemCode", "ItemName", "UOMCode"},
                    new String[]{"ParentCategory", "ItemCategory", "ItemType"}),
            "Purchase Summary By Item & Warehouse", summarySpec(new String[]{"CityName", "ItemCode", "ItemName", "WarehouseName", "UOMCode"},
                    new String[]{"ParentCategory", "ItemCategory", "ItemType"}),
            "Purchase Summary By Parent Category", summarySpec(new String[]{"CityName", "ParentCategory", "UOMCode"}, new String[]{}),
            "Purchase Summary By Parent Category & Supplier", summarySpec(new String[]{"CityName", "PartyName", "ParentCategory", "UOMCode"}, new String[]{}),
            "Purchase Summary By HsCode", summarySpec(new String[]{"CityName", "HsCode", "UOMCode"}, new String[]{}));

    /** Approval Detail button of the Detail grid (GridEX_Helper.AddButton "ApprovalDetail", :1429). */
    public Map<String, Object> registerApproval(Map<String, Object> b) {
        @SuppressWarnings("unchecked")
        Map<String, Object> filterBody = b.get("filter") instanceof Map<?, ?> m ? (Map<String, Object>) m : Map.of();
        @SuppressWarnings("unchecked")
        List<Map<String, Object>> raw = (List<Map<String, Object>>) registerDetail(filterBody).get("raw");
        return approvals.history(Map.of("rows", raw), "rows", num(b.get("documentTypeId")), num(b.get("id")));
    }

    // =====================================================================================
    // 869 frmSupplierDispatchPreBillReport
    // =====================================================================================

    private static final String BRANCH_DISPATCH = "[dbo].[USP_GetBranchsAllocatedToUserFromSupplierDispatch]";

    /** frmSupplierDispatchPreBillReport_Load:183. */
    public Map<String, Object> transitInit() {
        UserAccount u = user(TRANSIT_SCREEN);
        var branches = repo.branches(BRANCH_DISPATCH, u, 251);
        Map<String, Object> out = init(u, branches);
        // BranchesFill (:244-255): the user's branch, else the first row.
        if (num(out.get("branchId")) == 0 && !branches.isEmpty()) out.put("branchId", num(branches.get(0).get("BranchId")));
        String ids = num(out.get("branchId")) == 0 ? "" : "," + out.get("branchId");
        out.put("lookups", plainRows(repo.dispatchLookups(u, ids)));
        Object eta = repo.maxEtaForPreBill(u);
        out.put("maxEta", eta == null ? STAMP.format(LocalDateTime.now()) : plain(eta));
        out.put("activities", List.of("Detail Report", "Customized Detail Report", "DocWise Report", "Summary By Party"));
        return out;
    }

    public List<Map<String, Object>> transitLookups(List<Integer> branchIds) {
        UserAccount u = user(TRANSIT_SCREEN);
        return plainRows(repo.dispatchLookups(u, branchIds(u, repo.branches(BRANCH_DISPATCH, u, 251), branchIds)));
    }

    /** GridFill:471. */
    public Map<String, Object> transitRows(Map<String, Object> b) {
        UserAccount u = user(TRANSIT_SCREEN);
        Map<String, Object> f = new LinkedHashMap<>();
        f.put("branchesIds", branchIds(u, repo.branches(BRANCH_DISPATCH, u, 251), ints(b.get("branchIds"))));
        String type = str(b.get("reportType"));
        if (type.isEmpty()) throw new IllegalArgumentException("Select Report Type First");
        f.put("fromDate", date(b.get("fromDate")));
        f.put("toDate", date(b.get("toDate")));
        for (String k : List.of("fromNo", "toNo", "orderNoFrom", "orderNoTo", "itemId", "supplierId", "commissionAgentId",
                "referencePartyId", "cityId")) f.put(k, num(b.get(k)));
        f.put("parentCategoryIds", idList(b.get("parentCategoryIds")));
        f.put("driverName", str(b.get("driverName")));
        f.put("vehicleNo", str(b.get("vehicleNo")));
        f.put("reportType", type);
        int actionId = switch (str(b.get("receiving"))) { case "Pending" -> 1; case "Received" -> 2; default -> 0; };
        f.put("actionId", actionId);
        if (actionId > 0 && f.get("toDate") == null)
            throw new IllegalArgumentException("Please Select ToDate For Filtering Pending Or Received Records");
        var raw = repo.dispatchActivityReport(u, f);
        return result(raw.stream().map(r -> project(r, TRANSIT_COLUMNS)).toList(), raw);
    }

    /** dtcol of GridFill (:546-611). */
    private static final String[][] TRANSIT_COLUMNS = {
            {"Id", "Id"}, {"DocumentTypeId", "DocumentTypeId"}, {"DocumentType", "DocumentType"}, {"BranchName", "BranchName"},
            {"DocNo", "DocNo"}, {"BranchSrNo", "BranchSrNo"}, {"DocDate", "DocDate"}, {"VehicleNo", "VehicleNo"}, {"BiltyNo", "BiltyNo"},
            {"DepartureDate", "DepartureDate"}, {"TransitDays", "TransitDays"}, {"ETAdestination", "ETAdestination"},
            {"TotalQty", "TotalQty"}, {"SupplierWbWeight", "SupplierWbWeight"}, {"TotalAmount", "TotalAmount"}, {"FcyAmount", "FcyAmount"},
            {"Currency", "Currency"}, {"ExchangeRate", "ExchangeRate"}, {"Freight", "Freight"}, {"OtherExpense", "OtherExpense"},
            {"AdvanceFreight", "AdvanceFreight"}, {"CityName", "CityName"}, {"Remarks", "Remarks"}, {"EntryUser", "EntryUser"},
            {"EntryDate", "EntryDate"}, {"ModifyUser", "ModifyUser"}, {"ModifyDate", "ModifyDate"}, {"ApprovedUser", "ApprovedUser"},
            {"ApprovedDate", "ApprovedDate"}, {"ApprovalStatus", "IsApproved"}, {"DetailId", "DetailId"}, {"SupplierId", "SupplierId"},
            {"SupplierGLId", "SupplierGLId"}, {"SupplierName", "SupplierName"}, {"OrderId", "OrderId"}, {"OrderNo", "OrderNo"},
            {"OrderDate", "OrderDate"}, {"CommissionAgentId", "CommissionAgentId"}, {"CommissionAgentName", "CommissionAgentName"},
            {"CommAmount", "CommAmount"}, {"ReferencePartyId", "ReferencePartyId"}, {"ReferencePartyName", "ReferencePartyName"},
            {"ItemId", "ItemId"}, {"ItemCode", "ItemCode"}, {"ItemName", "ItemName"}, {"CropYear", "CropYear"},
            {"PackingType", "PackingType"}, {"PackUom", "PackUom"}, {"Qty", "Qty"}, {"NetWeight", "NetWeight"}, {"Rate", "Rate"},
            {"RateUom", "RateUom"}, {"Amount", "Amount"}, {"NoOfAttachments", "NoOfAttachments"}, {"TotalVehicles", "TotalVehicles"},
            {"EnRouteVehicles", "EnRouteVehicles"}, {"PreviouslyReachedVehicles", "PreviouslyReachedVehicles"},
            {"TodayReachedVehicle", "TodayReachedVehicle"}, {"ExpectedTodayReachedVehicle", "ExpectedTodayReachedVehicle"},
            {"LedgerAmount", "LedgerAmount"}, {"LedgerIncludingBillAmount", "LedgerIncludingBillAmount"}};

    // =====================================================================================
    // shared links: GL account of a party, attachments
    // =====================================================================================

    /** CommonServices.GetGlAccountIdBySupplierCustomerId for the SupplierName links (479 / 869). */
    public Map<String, Object> glAccount(int screen, int supplierCustomerId) {
        UserAccount u = user(allowedScreen(screen));
        return Map.of("accountId", supplierCustomerId <= 0 ? 0 : repo.glAccountOfParty(u, supplierCustomerId));
    }

    /** CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId) - rows of another tenant are dropped. */
    public List<Map<String, Object>> attachments(int screen, int id, int documentTypeId) {
        UserAccount u = user(allowedScreen(screen));
        List<Map<String, Object>> out = new ArrayList<>();
        if (id <= 0 || documentTypeId <= 0) return out;
        for (var r : repo.attachments(id, documentTypeId)) {
            if (num(r.get("OrganizationId")) != num(u.getOrganizationId()) || num(r.get("CompanyId")) != num(u.getCompanyId())) continue;
            Map<String, Object> a = new LinkedHashMap<>();
            a.put("Id", num(r.get("Id")));
            a.put("AttachmentName", str(r.get("Attachment")));
            a.put("CustomName", str(r.get("UploadedFileCustomName")));
            a.put("EntryDate", plain(r.get("EntryDate")));
            out.add(a);
        }
        return out;
    }

    public record Download(String name, byte[] bytes) {}

    public Download attachment(int screen, int id, int documentTypeId, int attachmentId) {
        UserAccount u = context.requireAccountingUser();
        for (var a : attachments(screen, id, documentTypeId)) {
            if (num(a.get("Id")) != attachmentId) continue;
            String stored = basename(str(a.get("CustomName")).isBlank() ? str(a.get("AttachmentName")) : str(a.get("CustomName")));
            String name = str(a.get("AttachmentName")).isBlank() ? stored : basename(str(a.get("AttachmentName")));
            return new Download(name, attachmentStore.read(u, stored));
        }
        throw new IllegalArgumentException("Attachment not found");
    }

    private static int allowedScreen(int screen) {
        if (!Set.of(GRN_SCREEN, GATE_PASS_SCREEN, PO_SCREEN, REGISTER_SCREEN, TRANSIT_SCREEN).contains(screen))
            throw new IllegalArgumentException("Unknown report");
        return screen;
    }

    private static String basename(String name) {
        String v = name == null ? "" : name.replace('\\', '/');
        v = v.substring(v.lastIndexOf('/') + 1);
        DesktopAttachmentStore.validateName(v);
        return v;
    }

    // =====================================================================================
    // helpers
    // =====================================================================================

    private Map<String, Object> init(UserAccount u, List<Map<String, Object>> branches) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("branches", plainRows(branches));
        int current = num(u.getBranchesId());
        out.put("branchId", branches.stream().anyMatch(r -> num(r.get("BranchId")) == current) ? current : 0);
        out.put("dateTypes", DATE_TYPES);
        out.put("yearStart", repo.yearStart(u, context.currentFinancialYearId()));
        out.put("now", STAMP.format(LocalDateTime.now()));
        // clsGlobalVariables.stringFormatsingle / DecimalRateFormate decimals (company configuration).
        int amount = num(repo.configuration(u, "Default NoofDecimal Points For Amount").trim());
        int rate = num(repo.configuration(u, "Default NoofDecimal Points For Rate").trim());
        out.put("amountDecimals", amount >= 1 && amount <= 4 ? amount : 2);
        out.put("rateDecimals", rate >= 1 && rate <= 4 ? rate : rate == 0 ? 2 : 0);
        return out;
    }

    private static String defaultBranchIds(UserAccount u, List<Map<String, Object>> branches) {
        int current = num(u.getBranchesId());
        return branches.stream().anyMatch(r -> num(r.get("BranchId")) == current) ? "," + current : "";
    }

    private boolean hasView(UserAccount u, int screen) {
        try { rights.require(u, screen, "View"); return true; } catch (AccessDeniedException e) { return false; }
    }

    /** The checked branches as the desktop builds them (",1,2"); every id must be allocated to the user. */
    private static String branchIds(UserAccount u, List<Map<String, Object>> allowedRows, List<Integer> ids) {
        if (ids == null || ids.isEmpty()) throw new IllegalArgumentException("Select Branch First");
        Set<Integer> allowed = allowedRows.stream().map(r -> num(r.get("BranchId"))).collect(Collectors.toSet());
        if (ids.stream().anyMatch(id -> id == null || !allowed.contains(id)))
            throw new AccessDeniedException("The selected branch is not allocated to this user");
        return ids.stream().distinct().map(String::valueOf).collect(Collectors.joining(",", ",", ""));
    }

    private static String idList(Object value) {
        List<Integer> ids = ints(value);
        return ids.isEmpty() ? "" : ids.stream().map(String::valueOf).collect(Collectors.joining(",", ",", ""));
    }

    private static List<Integer> ints(Object value) {
        List<Integer> out = new ArrayList<>();
        if (value instanceof Collection<?> c) for (Object o : c) { int n = num(o); if (n != 0) out.add(n); }
        return out;
    }

    private static Map<String, Object> result(List<Map<String, Object>> rows, List<Map<String, Object>> raw) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows);
        out.put("raw", plainRows(raw));
        return out;
    }

    private static Map<String, Object> project(Map<String, Object> source, String[][] spec) {
        Map<String, Object> row = new LinkedHashMap<>();
        for (String[] c : spec) {
            Object v = c[1].isEmpty() ? "" : source.get(c[1]);
            String kind = c.length > 2 ? c[2] : "";
            if ("bool".equals(kind)) v = dbl(v) != 0;
            else if ("date".equals(kind)) { String s = plain(v) == null ? "" : String.valueOf(plain(v)); v = s.length() >= 10 ? s.substring(0, 10) : s; }
            else v = plain(v);
            row.put(c[0], v);
        }
        return row;
    }

    private static List<Map<String, Object>> plainRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            r.forEach((k, v) -> m.put(k, plain(v)));
            out.add(m);
        }
        return out;
    }

    private static Object plain(Object v) {
        if (v instanceof Timestamp t) return STAMP.format(t.toLocalDateTime());
        if (v instanceof java.sql.Date d) return d.toLocalDate().toString();
        if (v instanceof java.util.Date d) return STAMP.format(new Timestamp(d.getTime()).toLocalDateTime());
        if (v instanceof LocalDateTime d) return STAMP.format(d);
        if (v instanceof java.time.OffsetDateTime d) return STAMP.format(d.toLocalDateTime());
        if (v instanceof BigDecimal b) return b.doubleValue();
        return v;
    }

    /** "yyyy-MM-dd" or "yyyy-MM-ddTHH:mm[:ss]" (the picker's date and its time of day) - null when absent. */
    private static LocalDateTime date(Object value) {
        String s = str(value).trim();
        if (s.isEmpty()) return null;
        try {
            if (s.length() == 10) return LocalDate.parse(s).atStartOfDay();
            return LocalDateTime.parse(s.replace(' ', 'T'));
        } catch (RuntimeException e) {
            throw new IllegalArgumentException("Invalid date: " + s);
        }
    }

    private static LocalDateTime required(Object value) {
        LocalDateTime d = date(value);
        if (d == null) throw new IllegalArgumentException("From Date and To Date are required");
        return d;
    }

    private static String iso(Object value) {
        return value instanceof LocalDateTime d ? d.toLocalDate().toString() : null;
    }

    private static String status(Object value) {
        String v = str(value);
        return Set.of("Open", "Complete", "Cancel").contains(v) ? v : "";
    }

    private static String approval(Object value) {
        String v = str(value);
        return Set.of("Approved", "UnApproved").contains(v) ? v : "All";
    }

    private static boolean bool(Object value) {
        return value instanceof Boolean b ? b : "true".equalsIgnoreCase(str(value));
    }

    private static String str(Object value) { return value == null ? "" : value.toString(); }

    private static int num(Object value) { return PurchaseReportsRepository.num(value); }

    private static double dbl(Object value) {
        if (value instanceof Number n) return n.doubleValue();
        if (value instanceof Boolean b) return b ? 1 : 0;
        if (value == null) return 0;
        try { return Double.parseDouble(value.toString().trim()); } catch (NumberFormatException e) { return 0; }
    }
}
