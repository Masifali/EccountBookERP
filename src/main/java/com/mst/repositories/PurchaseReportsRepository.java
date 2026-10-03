package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/**
 * Purchase Reports (module 52) - the desktop BLL calls of the five report forms, parameter for
 * parameter. Every numeric / text parameter the BLL guards with "!= 0" / "!= empty" is omitted the
 * same way (DesktopProc drops null values, as ADO.NET AddWithValue(null) does).
 *
 *   477 frmGRNHistory               BLL 0576 InvGrn           Sp_InvGrn_History, usp_GrnRegisterSummaryItemWise (print)
 *   478 frmGatePassReport           BLL 0567 GatePassInward   Sp_GatePassInward_History
 *   479 PurchaseOrderHistory        BLL 0595 PurchaseOrder    Sp_PurchaseOrder_History_Rpt, USP_PurchaseOrderSummaryRegister,
 *                                                             USP_GetOrdersStatus, Sp_PurchaseOrder_GetAllMethod (status)
 *   480 PurchaseRegisterNew         BLL 0574 / 0581           Usp_InvPurchaseInvoice_PurchasingReports,
 *                                                             USP_PurchaseReportWithActivitiesFromEvaulations
 *   869 frmSupplierDispatchPreBillReport BLL 0517 SupplierDispatch USP_SupplierDispatch_ActivityReport
 */
@Repository
public class PurchaseReportsRepository {
    private final JdbcTemplate jdbc;

    public PurchaseReportsRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ================================================================= shared

    public List<Map<String, Object>> branches(String proc, UserAccount u, int documentTypeId) {
        Map<String, Object> p = tenant(u);
        p.put("UserId", u.getId());
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        return DesktopProc.rows(jdbc, proc, p);
    }

    /** clsGlobalVariables.ActiveYr.Start_Period - the signed-in financial year's start. */
    public String yearStart(UserAccount u, int financialYearId) {
        var rows = DesktopProc.rows(jdbc, "Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", tenant(u));
        for (var r : rows) if (num(r.get("Id")) == financialYearId && r.get("Start_Period") != null)
            return String.valueOf(r.get("Start_Period")).substring(0, 10);
        return "";
    }

    /** CommonServices.GetConfigurationByOrgCompandConfigDescription(name) - the ConfigKey text. */
    public String configuration(UserAccount u, String description) {
        var p = tenant(u);
        p.put("ConfigDescription", description);
        p.put("Activity", "GetConfigurationByOrgCompandConfigDescription");
        var rows = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", p);
        return rows.isEmpty() ? "" : Objects.toString(rows.get(0).get("ConfigKey"), "");
    }

    /** Right names granted to the user on a screen (tblUserRights, company-enabled screens only). */
    public Set<String> grantedRights(UserAccount u, int screenId) {
        return new LinkedHashSet<>(jdbc.queryForList("SELECT DISTINCT r.RightName FROM dbo.ScreenRights r "
                + "INNER JOIN dbo.tblUserRights g ON g.RightId=r.Id AND g.ScreenId=r.ScreenID "
                + "WHERE r.ScreenID=? AND g.CompanyId=? AND g.UserId=? AND g.Value=1 "
                + "AND EXISTS (SELECT 1 FROM dbo.CompanyRights c WHERE c.ScreenId=r.ScreenID "
                + "AND c.CompanyId=g.CompanyId AND c.IsActive=1)", String.class,
                screenId, u.getCompanyId(), u.getId()));
    }

    /** DAW.usp_getDocumentApprovalPolicy (DocumentApprovalPolicy.GetDocumentsInPolicy) - DocumentId list. */
    public Set<Integer> documentsInApprovalPolicy(UserAccount u) {
        Set<Integer> out = new HashSet<>();
        for (var r : DesktopProc.rows(jdbc, "[DAW].[usp_getDocumentApprovalPolicy]", tenant(u))) out.add(num(r.get("DocumentId")));
        return out;
    }

    /** CommonServices.GetGlAccountIdBySupplierCustomerId. */
    public int glAccountOfParty(UserAccount u, int supplierCustomerId) {
        var p = tenant(u);
        p.put("SupplierCustomerId", supplierCustomerId);
        p.put("Activity", "GetGlAccountIdBySupplierCustomerId");
        var rows = DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", p);
        if (rows.isEmpty() || rows.get(0).isEmpty()) return 0;
        return num(rows.get(0).values().iterator().next());
    }

    /** CommonServices.GetNoofAttachmentsByRefDocumentTypeID -> DMSAttachments.ReadAttachmentsbyRefDocumentTypeId. */
    public List<Map<String, Object>> attachments(int id, int refDocumentTypeId) {
        return DesktopProc.rows(jdbc, "Sp_DMSAttachments_GetAllMethod", DesktopProc.params(
                "RefDocumentTypeId", refDocumentTypeId, "Id", id, "Activity", "ReadAttachmentsbyRefDocumentTypeId"));
    }

    // ================================================================= 477 frmGRNHistory

    /** InvGrn.GetDataForDropDownFromGrn(org, comp, null, null, BranchIds, 0) - frmGRNHistory.cs:215. */
    public List<Map<String, Object>> grnLookups(UserAccount u, String branchIds) {
        var p = tenant(u);
        text(p, "BranchesIds", branchIds);
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromGrn]", p);
    }

    /** InvGrn.InvGrn_History (BLL 0576:699) - first table; the second is the Crystal logo. */
    public List<Map<String, Object>> grnHistory(UserAccount u, Map<String, Object> f) {
        return DesktopProc.rows(jdbc, "Sp_InvGrn_History", grnParams(u, f));
    }

    private Map<String, Object> grnParams(UserAccount u, Map<String, Object> f) {
        var p = tenant(u);
        date(p, "GrnDateF", f.get("fromDate"));
        date(p, "GrnDateT", f.get("toDate"));
        number(p, "SupplierCustomerId", f.get("supplierId"));
        number(p, "GpSrNoF", f.get("gpFrom"));
        number(p, "GpSrNoT", f.get("gpTo"));
        number(p, "GrnNoF", f.get("fromNo"));
        number(p, "GrnNoT", f.get("toNo"));
        number(p, "WarehouseId", f.get("warehouseId"));
        number(p, "JobLotId", f.get("jobLotId"));
        number(p, "ItemId", f.get("itemId"));
        text(p, "AreaCity", f.get("areaCity"));
        text(p, "BranchesIds", f.get("branchesIds"));
        text(p, "VehicleNo", f.get("vehicleNo"));
        number(p, "InventoryParentCategorId", f.get("parentCategoryId"));
        return p;
    }

    // ================================================================= 478 frmGatePassReport

    /**
     * GatePassInward.GetDataForDropDownFromGPI(int CompanyId, int OrganizationId, ...) is called by
     * frmGatePassReport.cs:171 as (OrganizationId, CompanyId, ...): the two values land swapped in
     * @OrganizationId / @CompanyId. Reproduced (desktop quirk).
     */
    public List<Map<String, Object>> gatePassLookups(UserAccount u, String branchIds) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getCompanyId());
        p.put("CompanyId", u.getOrganizationId());
        text(p, "BranchesIds", branchIds);
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromGPI]", p);
    }

    /** GatePassInward.GatepassInwardHistory (BLL 0567:1311). */
    public List<Map<String, Object>> gatePassHistory(UserAccount u, Map<String, Object> f) {
        var p = tenant(u);
        number(p, "SupplierCustomerId", f.get("supplierId"));
        date(p, "FromDate", f.get("fromDate"));
        date(p, "ToDate", f.get("toDate"));
        number(p, "FromDocNo", f.get("fromNo"));
        number(p, "ToDocNo", f.get("toNo"));
        text(p, "Status", f.get("status"));
        text(p, "GatepassType", f.get("gatePassType"));
        text(p, "BranchesIds", f.get("branchesIds"));
        return DesktopProc.rows(jdbc, "Sp_GatePassInward_History", p);
    }

    // ================================================================= 479 PurchaseOrderHistory

    /** PurchaseOrder.GetDataForDropDownFromPurchaseOrder, DocumentTypeIds "41" (PurchaseOrderHistory.cs:591). */
    public List<Map<String, Object>> purchaseOrderLookups(UserAccount u, String branchIds) {
        var p = tenant(u);
        p.put("DocumentTypeIds", "41");
        text(p, "BranchesIds", branchIds);
        return DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromPurchaseOrder", p);
    }

    /** PurchaseOrder.GetParentCategoriesForPurchaseOrder - no dates are set by the form (:631). */
    public List<Map<String, Object>> purchaseOrderParentCategories(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[usp_getParentCategoriesForPurchaseOrder]", tenant(u));
    }

    /** PurchaseOrder.GetOrderStatus (BLL 0595:458), DocumentTypeId 41, IntervalDays 0 (not sent). */
    public List<Map<String, Object>> orderStatus(UserAccount u) {
        var p = tenant(u);
        p.put("DocumentTypeId", 41);
        return DesktopProc.rows(jdbc, "USP_GetOrdersStatus", p);
    }

    /** PurchaseOrder.PurchaseOrderHistory (BLL 0595:494) as GetDetailData (:822) fills it. */
    public List<Map<String, Object>> purchaseOrderHistory(UserAccount u, Map<String, Object> f) {
        var p = tenant(u);
        p.put("DocumentTypeId", 41);
        date(p, "StartOrderDate", f.get("fromDate"));
        date(p, "EndOrderDate", f.get("toDate"));
        number(p, "PoSrFrom", f.get("fromNo"));
        number(p, "PoSrTo", f.get("toNo"));
        number(p, "SupplierCustomerId", f.get("supplierId"));
        number(p, "ItemId", f.get("itemId"));
        approval(p, "IsApproved", f.get("approval"));
        text(p, "Status", status(f.get("status")));
        text(p, "BranchesIds", f.get("branchesIds"));
        number(p, "ActivitiyId", f.get("activityId"));
        number(p, "ParentCategoryId", f.get("parentCategoryId"));
        number(p, "BookingPersonId", f.get("bookingPersonId"));
        return DesktopProc.rows(jdbc, "Sp_PurchaseOrder_History_Rpt", p);
    }

    /** PurchaseOrder.PurchaseOrderSummaryRegister (BLL 0595:2494) as GridSummaryFill (:1549) fills it. */
    public List<Map<String, Object>> purchaseOrderSummary(UserAccount u, Map<String, Object> f) {
        var p = tenant(u);
        date(p, "FromDate", f.get("fromDate"));
        date(p, "ToDate", f.get("toDate"));
        number(p, "DocNoFrom", f.get("fromNo"));
        number(p, "DocNoTo", f.get("toNo"));
        number(p, "InventoryParentCategories", f.get("parentCategoryId"));
        number(p, "ItemCategoryId", f.get("itemCategoryId"));
        number(p, "ItemTypeId", f.get("itemTypeId"));
        number(p, "ItemId", f.get("itemId"));
        text(p, "CropYear", f.get("cropYear"));
        number(p, "JobLotId", f.get("jobLotId"));
        number(p, "DistrictId", f.get("districtId"));
        number(p, "OrderSupCustId", f.get("supplierId"));
        if (bool(f.get("packUom"))) p.put("IsPackSize", 1);
        number(p, "CityId", f.get("cityId"));
        if (bool(f.get("cityWise"))) p.put("IsCity", 1);
        if (bool(f.get("skipZero"))) p.put("SkipZero", 1);
        approval(p, "IsApproved", f.get("approval"));
        text(p, "OrderStatus", status(f.get("status")));
        text(p, "ActivityName", f.get("reportType"));
        text(p, "BranchesIds", f.get("branchesIds"));
        number(p, "ActivityId", f.get("activityId"));
        number(p, "BookingPersonId", f.get("bookingPersonId"));
        return DesktopProc.rows(jdbc, "[dbo].[USP_PurchaseOrderSummaryRegister]", p);
    }

    /** PurchaseOrder.UpdateStatusandIsApprovedbyOrderId (BLL 0595:678): one call per order, no transaction. */
    public void updateOrderStatus(UserAccount u, int id, String reqType, String remarks, LocalDateTime expiryDate) {
        var p = tenant(u);
        p.put("PostUser", u.getId());
        if (reqType != null && !reqType.isEmpty()) p.put("ReqType", reqType);
        if (id != 0) p.put("Id", id);
        if (remarks != null && !remarks.isEmpty()) p.put("OrderStatusRemarks", remarks);
        if (expiryDate != null) p.put("OrderExpiryDate", Timestamp.valueOf(expiryDate));
        p.put("Activity", "UpdateStatusandIsapprovedByOrderId");
        DesktopProc.rows(jdbc, "Sp_PurchaseOrder_GetAllMethod", p);
    }

    // ================================================================= 480 PurchaseRegisterNew

    /** InvPurchaseInvoice.GetBranchsAllocatedToUserFromPurchaseInvoice(org, comp, user, 0) - :686. */
    public List<Map<String, Object>> purchaseInvoiceBranches(UserAccount u) {
        return branches("[dbo].[USP_GetBranchsAllocatedToUserFromPurchaseInvoice]", u, 0);
    }

    /** InvPurchaseInvoice.AllComboBindAgainstPurchaseInvoice - AllcomboFillFromPurchaseInvoice (:787). */
    public List<Map<String, Object>> purchaseInvoiceCombos(UserAccount u, String branchIds) {
        var p = tenant(u);
        text(p, "BranchesIds", branchIds);
        return DesktopProc.rows(jdbc, "[dbo].[Usp_AllComboAgainstPurchaseInvoice]", p);
    }

    /** StocksReport.Inventory_StockEvalautionDetail_DropDownAndLists, DocumentTypeIds "56,57" - (:974). */
    public List<Map<String, Object>> evaluationCombos(UserAccount u, String branchIds) {
        var p = tenant(u);
        p.put("DocumentTypeIds", "56,57");
        text(p, "BranchesIds", branchIds);
        return DesktopProc.rows(jdbc, "USP_Inventory_StockEvalautionDetail_DropDownAndLists", p);
    }

    /** CommonServices.CustomeGroupsDefine(2) -> AcLookUps.GetAll: Sp_AcLookUps_GetAllMethod 'ReadAll', TypeId 2. */
    public List<Map<String, Object>> customGroups(UserAccount u) {
        var p = tenant(u);
        p.put("AcLookUpTypesId", 2);
        p.put("Activity", "ReadAll");
        return DesktopProc.rows(jdbc, "Sp_AcLookUps_GetAllMethod", p);
    }

    /** InventoryStockEvalautionDetail.PurchasesingRegister (BLL 0574:2945) as GridFill (:1226) fills it. */
    public List<Map<String, Object>> purchasingRegister(UserAccount u, Map<String, Object> f) {
        var p = tenant(u);
        date(p, "datefrom", f.get("fromDate"));
        date(p, "DateTo", f.get("toDate"));
        number(p, "BillNoFrom", f.get("fromNo"));
        number(p, "BillNoTo", f.get("toNo"));
        number(p, "ItemClassId", f.get("itemClassId"));
        number(p, "ItemCategoryId", f.get("itemCategoryId"));
        number(p, "ItemTypeId", f.get("itemTypeId"));
        text(p, "CropYear", f.get("cropYear"));
        text(p, "BranchesIds", f.get("branchesIds"));
        text(p, "CustomGroupIds", f.get("customGroupIds"));
        text(p, "ParentCategoryIds", f.get("parentCategoryIds"));
        number(p, "PackingTypeId", f.get("packingTypeId"));
        number(p, "JobLotId", f.get("jobLotId"));
        number(p, "WarehouseId", f.get("warehouseId"));
        number(p, "ItemId", f.get("itemId"));
        number(p, "SupplierCustomerId", f.get("supplierId"));
        number(p, "CommissionAgentId", f.get("commissionAgentId"));
        number(p, "StockAccountId", f.get("stockAccountId"));
        number(p, "DocumentTypeId", f.get("documentTypeId"));
        p.put("ReportType", Objects.toString(f.get("reportType"), ""));
        return DesktopProc.rows(jdbc, "Usp_InvPurchaseInvoice_PurchasingReports", p);
    }

    /** InvPurchaseInvoice.PurchaseReportWithActivitiesFromEvaulations (BLL 0581:2532) as GridSummaryFill (:1702). */
    public List<Map<String, Object>> purchaseWithActivities(UserAccount u, Map<String, Object> f) {
        var p = tenant(u);
        date(p, "FromDate", f.get("fromDate"));
        date(p, "ToDate", f.get("toDate"));
        number(p, "DocNoFrom", f.get("fromNo"));
        number(p, "DocNoTo", f.get("toNo"));
        number(p, "ItemId", f.get("itemId"));
        number(p, "ItemCategoryId", f.get("itemCategoryId"));
        number(p, "ItemClassGroupId", f.get("itemClassId"));
        number(p, "ItemTypeId", f.get("itemTypeId"));
        number(p, "SupplierCustomerId", f.get("supplierId"));
        number(p, "PackingTypeId", f.get("packingTypeId"));
        number(p, "JobLotId", f.get("jobLotId"));
        number(p, "WarehouseId", f.get("warehouseId"));
        number(p, "CityId", f.get("cityId"));
        // BLL 0581:2659 sends @DistrictId when != 0; USP_PurchaseReportWithActivitiesFromEvaulations has no
        // such parameter, so a chosen District fails on the desktop too (reproduced, see report).
        number(p, "DistrictId", f.get("districtId"));
        if (bool(f.get("cityWise"))) p.put("IsCityAdd", 1);
        if (bool(f.get("packUom"))) p.put("IsPackSizeAdd", 1.0);
        text(p, "CropYear", f.get("cropYear"));
        if (!Objects.toString(f.get("reportType"), "").isEmpty()) p.put("ActivityName", f.get("reportType"));
        number(p, "PurchaseStockAccountId", f.get("stockAccountId"));
        text(p, "BranchesIds", f.get("branchesIds"));
        text(p, "ParentCategoryIds", f.get("parentCategoryIds"));
        text(p, "CustomGroupIds", f.get("customGroupIds"));
        return DesktopProc.rows(jdbc, "[dbo].[USP_PurchaseReportWithActivitiesFromEvaulations]", p);
    }

    // ================================================================= 869 frmSupplierDispatchPreBillReport

    /** SupplierDispatch.GetDataForDropDownFromSupplierDispatch, DocumentTypeIds "251" (:263). */
    public List<Map<String, Object>> dispatchLookups(UserAccount u, String branchIds) {
        var p = tenant(u);
        text(p, "BranchesIds", branchIds);
        p.put("DocumentTypeIds", "251");
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromSupplierDispatch]", p);
    }

    /** SupplierDispatch.GetMaxETA_DateForPreBill (BLL 0517:549): ETA_Destination of row 0, else DateTime.Now. */
    public Object maxEtaForPreBill(UserAccount u) {
        var p = tenant(u);
        p.put("Activity", "GetMaxETA_DateForPreBill");
        var rows = DesktopProc.rows(jdbc, "[dbo].[USP_SupplierDispatch_GetAllMethod]", p);
        return rows.isEmpty() ? null : rows.get(0).get("ETA_Destination");
    }

    /** SupplierDispatch.SupplierDispatch_ActivityReport (BLL 0517:712) as GridFill (:471) fills it. */
    public List<Map<String, Object>> dispatchActivityReport(UserAccount u, Map<String, Object> f) {
        var p = tenant(u);
        text(p, "BranchesIds", f.get("branchesIds"));
        date(p, "FromDate", f.get("fromDate"));
        date(p, "ToDate", f.get("toDate"));
        number(p, "DocNoFrom", f.get("fromNo"));
        number(p, "DocNoTo", f.get("toNo"));
        number(p, "OrderNoFrom", f.get("orderNoFrom"));
        number(p, "OrderNoTo", f.get("orderNoTo"));
        text(p, "ParentCategoryIds", f.get("parentCategoryIds"));
        number(p, "ItemId", f.get("itemId"));
        number(p, "SupplierCustomerId", f.get("supplierId"));
        number(p, "CommissionAgentId", f.get("commissionAgentId"));
        number(p, "ReferencePartyId", f.get("referencePartyId"));
        text(p, "DriverName", f.get("driverName"));
        text(p, "VehicleNo", f.get("vehicleNo"));
        number(p, "CityId", f.get("cityId"));
        number(p, "ActionId", f.get("actionId"));
        text(p, "ActivityName", f.get("reportType"));
        return DesktopProc.rows(jdbc, "[dbo].[USP_SupplierDispatch_ActivityReport]", p);
    }

    // ================================================================= helpers

    private static Map<String, Object> tenant(UserAccount u) {
        return DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    /** "Approved" -> 1, "UnApproved" -> 0, "All" -> not sent (ApprovedFilter "All"). */
    private static void approval(Map<String, Object> p, String key, Object value) {
        String v = Objects.toString(value, "");
        if ("Approved".equals(v)) p.put(key, true);
        else if ("UnApproved".equals(v)) p.put(key, false);
    }

    private static String status(Object value) {
        String v = Objects.toString(value, "");
        return "Open".equals(v) || "Complete".equals(v) || "Cancel".equals(v) ? v : null;
    }

    private static void number(Map<String, Object> p, String key, Object value) {
        int n = num(value);
        if (n != 0) p.put(key, n);
    }

    private static void text(Map<String, Object> p, String key, Object value) {
        if (value == null) return;
        String v = value.toString();
        if (!v.isEmpty()) p.put(key, v);
    }

    private static void date(Map<String, Object> p, String key, Object value) {
        if (value instanceof LocalDateTime d) p.put(key, Timestamp.valueOf(d));
    }

    private static boolean bool(Object value) {
        return value instanceof Boolean b ? b : "true".equalsIgnoreCase(Objects.toString(value, ""));
    }

    public static int num(Object value) {
        if (value instanceof Number n) return n.intValue();
        if (value == null) return 0;
        try { return (int) Double.parseDouble(value.toString().trim()); } catch (NumberFormatException e) { return 0; }
    }
}
