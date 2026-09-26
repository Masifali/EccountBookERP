package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 331 "Delivery Order (Stock Transfer)" — Architecture.WinApp.Sale.DeliveryOrder (DeliveryOrder.cs)
 * opened as ScreenDefinition 331 (ScreenName / Tag "DeliveryOrderTransfer", DocumentTypeId 84).
 *
 * Every read and write the form, its BLL and its DAL make, with the BLL's parameter names and the BLL's
 * "only when non-zero" conditions:
 *
 *   BLL 0558 Architecture.BLL.Inventory.InvDeliveryOrder   GenerateCode, GetByID, FormHistoryNew,
 *            GetDataForDropDownFromDeliveryOrder, GetAvailableStockForDeliveryOrder,
 *            DeliveryOrderStackWarningMessage, Save
 *   DAL 0411 Architecture.DAL.Inventory.InvDeliveryOrder   SetData, GetData (ReadById → ReadByIdDetailId,
 *            ReadInvDeliveryOrderExpensesByHeaderId)
 *   BLL 0574 InventoryStockEvalautionDetail.GetCurrentStockByItemId (grid AvailableStock)
 *   BLL 0131 InvGrnandGdnReports.InvDeliveryOrderSlip       (262 and 264 slips — Sp_InvDeliveryOrder_Slip)
 *   BLL 0058 Branches.GetAll (CommonServices.BrancheServiceBind), BLL 0611 VehicleType.GetAll,
 *   BLL 0074 ReferenceParties.GetAll, BLL 0573 InventoryItemsOther.GetAll,
 *   BLL 0379 GlobalServicesMethods — the clsGlobalVariables lists the form binds from
 *            (SupplierCustomerLists, WarehousesWithBranches, AllItems, UomSchedule, CropYear, InvPackingType),
 *   DAL 0205 GetJobLotGlIdsandName (globalJobLot), GetERPFeaturesByCompanyId (ErpFeaturesList),
 *   BLL 0269 CommonRepository.RemoveByID → USP_RecoredRemoveByOrgCompDocAndByID.
 */
@Repository
public class DeliveryOrderTransferRepository {

    public static final int DOCUMENT_TYPE_ID = 84;
    public static final String P_GETALL = "Sp_InvDeliveryOrder_GetAllMethod";

    private final JdbcTemplate jdbc;
    public DeliveryOrderTransferRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ============================================================================ numbering

    /** CommonServices.DeliveryOrderGenerateCode(84):11142 → BLL 0558 GenerateCode (FY / branch only when non-zero). */
    public int generateCode(UserAccount u, int financialYearId, int branchesId) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOCUMENT_TYPE_ID);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (branchesId != 0) p.put("BranchesId", branchesId);
        p.put("Activity", "GenerateCode");
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, p);
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    // ============================================================================ combos

    /** VehicleTypesBind(VehicleType.GetAll()) — BLL 0611, Sp_VehicleType_GetAllMethod has no parameter. */
    public List<Map<String, Object>> vehicleTypes() {
        return DesktopProc.rows(jdbc, "Sp_VehicleType_GetAllMethod", params());
    }

    /** CommonServices.BrancheServiceBind:824 → BLL 0058 Branches.GetAll (Activity 'GetAll'). */
    public List<Map<String, Object>> branches(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Branches_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Activity", "GetAll"));
    }

    /**
     * clsGlobalVariables.globalSupplierCustomer — DatatableHelper.GlobalSupplierCustomerListsFillDbCall(org, comp)
     * (partyTypeId 0) over BLL 0379 getGlobalSupplierCustomer (USP_GetVendorsAndCustomersWithCityName, no optional
     * parameter sent): customer groups 7, 9, 10 always dropped; with ShowBothVendorAndCustomerOnSalesPurchase = 1
     * groups 7, 8, 9, 10, 13 dropped. PartyTypeId is kept for SupplierDtFillFromGlobal's feature-4 filter.
     */
    public List<Map<String, Object>> suppliers(UserAccount u, int showBothVendorAndCustomer) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetVendorsAndCustomersWithCityName", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            int g = toInt(r.get("CustomerGroupId"));
            if (g == 7 || g == 9 || g == 10) continue;
            boolean excluded = g == 8 || g == 13;
            if (showBothVendorAndCustomer == 1 && excluded) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("CompanyName", str(r.get("CompanyName")));
            o.put("PartyCode", str(r.get("PartyCode")));
            o.put("GlAccountId", toInt(r.get("GlAccountId")));
            o.put("CityId", toInt(r.get("CityId")));
            o.put("CityName", str(r.get("CityName")));
            o.put("MobileNo", str(r.get("MobilePersonal")));
            o.put("PartyTypeId", toInt(r.get("PartyTypeId")));
            out.add(o);
        }
        return out;
    }

    /**
     * clsGlobalVariables.globalWarehousesWithBranches — BLL 0379 getGlobalActiveWarehouse(org, comp, BranchesId):
     * USP_GetWarehousesAllocatedToBranch with @BranchId = the user's branch (always sent).
     */
    public List<Map<String, Object>> warehousesWithBranches(UserAccount u, int branchId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetWarehousesAllocatedToBranch", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BranchId", branchId))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("Name", str(r.get("WarehouseName")));
            out.add(o);
        }
        return out;
    }

    /** clsGlobalVariables.getGlobalAllItems — BLL 0379 AllItemsWithModal(org, comp, 0, 0, "") (paging / keyword not sent). */
    public List<Map<String, Object>> allItems(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "[dbo].[USP_Item_AllItemsWithModal]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("ItemName", str(r.get("ItemName")));
            o.put("ItemCode", str(r.get("ItemCode")));
            o.put("ItemTypeOfTypeId", toInt(r.get("ItemTypeOfTypeId")));
            out.add(o);
        }
        return out;
    }

    /**
     * clsGlobalVariables.globalUomSchedule — BLL 0379 getAllUomsByCompanyId(org, comp, 0, 1): @ItemId not sent,
     * @Active 1. CommonServices.dtUomFromGloablUomScheduleByItemId:2159 filters it by ItemId.
     */
    public List<Map<String, Object>> uomSchedule(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "usp_getAllUomsByCompanyId", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Active", 1))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("UOMCode", str(r.get("UOMCode")));
            o.put("Equivalent", toDouble(r.get("Equivalent")));
            o.put("BaseRateUom", bool(r.get("BaseRateUom")));
            o.put("BasePackUom", bool(r.get("BasePackUom")));
            o.put("ItemId", toInt(r.get("ItemId")));
            out.add(o);
        }
        return out;
    }

    /** clsGlobalVariables.globalCropYear — BLL 0379 getGlobalAllCropYear: Sp_InvCropYear_GetAllMethod 'ReadAll'. */
    public List<Map<String, Object>> cropYears(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "[dbo].[Sp_InvCropYear_GetAllMethod]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("Name", str(r.get("CropYear")));
            out.add(o);
        }
        return out;
    }

    /** clsGlobalVariables.globalJobLot — DAL 0205 GetJobLotGlIdsandName: SP_JobLot_ReadMethod 'GetJobLotGlIdsandName'. */
    public List<Map<String, Object>> jobLots(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "[dbo].[SP_JobLot_ReadMethod]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetJobLotGlIdsandName"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("Name", str(r.get("JobLotDescription")));
            out.add(o);
        }
        return out;
    }

    /** clsGlobalVariables.globalInvPackingType — BLL 0379 getGlobalAllPackingType: @Activity 'ReadAll' only. */
    public List<Map<String, Object>> packingTypes() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "[dbo].[Sp_InvPackingType_GetAllMethod]", params("Activity", "ReadAll"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("Name", str(r.get("PackTypeDesc")));
            out.add(o);
        }
        return out;
    }

    /** ReferencePartiesDBCall:894 → BLL 0074 ReferenceParties.GetAll: [Sp_ReferenceParties_GetAllMethod] 'ReadAll'. */
    public List<Map<String, Object>> referenceParties(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "[Sp_ReferenceParties_GetAllMethod]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("Name", str(r.get("ReferencePartyName")));
            out.add(o);
        }
        return out;
    }

    /** OtherItemdtDbCall:1051 → BLL 0573 InventoryItemsOther.GetAll: @Activity 'ReadAll', @organizationId, @CompanyId. */
    public List<Map<String, Object>> otherItems(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_InventoryItemsOther_GetAllMethod", params(
                "Activity", "ReadAll", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("Name", str(r.get("OtherItemName")));
            out.add(o);
        }
        return out;
    }

    /** CommonServices.GetERPFeatureById — clsGlobalVariables.ErpFeaturesList (USP_GetERPFeaturesByCompanyId). */
    public boolean erpFeature(UserAccount u, int featureId) {
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetERPFeaturesByCompanyId", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            if (toInt(r.get("Id")) == featureId) return true;
        }
        return false;
    }

    /**
     * GetConfigurationsFromGlobal:712 — clsGlobalVariables.GetApplicationsAllocateToCompanyList.Find(AppId 4, this
     * company).IsActive. The list is read here from USP_GetApplicationsAllocateToCompany (no parameters); the BLL
     * that fills the desktop list is not in the recovered source.
     */
    public boolean customerPortal(UserAccount u) {
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetApplicationsAllocateToCompany", params())) {
            if (toInt(r.get("AppId")) == 4 && toInt(r.get("CompanyId")) == toInt(u.getCompanyId())) return bool(r.get("IsActive"));
        }
        return false;
    }

    /**
     * HistoryComboDBCall:665 → BLL 0558 GetDataForDropDownFromDeliveryOrder → dbo.USP_GetDataForDropDownFromDeliveryOrder.
     * The form sets FinancialYearId, BranchesIds (the user's branch as text), Activity 'Customer' and — Tag
     * "DeliveryOrderTransfer" — ReqType 'StockTransfer' (sent as @DeliveryOrderType). DocumentTypeIds is never set.
     */
    public List<Map<String, Object>> historyCustomers(UserAccount u, int financialYearId, String branchesIds) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (branchesIds != null && !branchesIds.isEmpty()) p.put("BranchesIds", branchesIds);
        p.put("Activity", "Customer");
        p.put("DeliveryOrderType", "StockTransfer");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromDeliveryOrder]", p)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("Name", str(r.get("ReferenceName")));
            out.add(o);
        }
        return out;
    }

    // ============================================================================ stock

    /**
     * AvailableStockGetByItem:3608 / GetStockBytableRow:4205 → BLL 0558 GetAvailableStockForDeliveryOrder →
     * USP_GetAvailableStockForDeliveryOrder. Every parameter is always added (no conditions in the BLL).
     */
    public double availableStockForDeliveryOrder(UserAccount u, int itemId, Timestamp docDate, int warehouseId, int jobLotId,
                                                 String cropYear, int packingTypeId, int itemUomId) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "ItemId", itemId,
                "DocDateTo", docDate,
                "WareHouseId", warehouseId,
                "JobLotId", jobLotId,
                "CropYear", cropYear,
                "InvPackingTypeId", packingTypeId,
                "ItemUomId", itemUomId);
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "USP_GetAvailableStockForDeliveryOrder", p);
        return r.isEmpty() ? 0d : toDouble(r.get(0).get("AvailableStock"));
    }

    /**
     * UpdateAvailableStockInGridDetailRow:3477 → BLL 0574 GetCurrentStockByItemId → Sp_SaleOrder_GetAllMethod
     * 'GetCurrentStockByItemId'. Every parameter is always added.
     */
    public double currentStockByItemId(UserAccount u, int itemId, Timestamp docDate, int warehouseId, int jobLotId,
                                       String cropYear, int packingTypeId, int itemUomId) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "ItemId", itemId,
                "DocDateTo", docDate,
                "WareHouseId", warehouseId,
                "JobLotId", jobLotId,
                "CropYear", cropYear,
                "InvPackingTypeId", packingTypeId,
                "ItemUomId", itemUomId,
                "Activity", "GetCurrentStockByItemId");
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_SaleOrder_GetAllMethod", p);
        return r.isEmpty() ? 0d : toDouble(r.get(0).get("AvailableStock"));
    }

    // ============================================================================ history

    /**
     * gridhistoryfill:2388 → BLL 0558 FormHistoryNew → SP_DeliveryOrderFormHistory. Parameters in the BLL's order
     * and under the BLL's conditions; Activity "StockTransfer" (Tag "DeliveryOrderTransfer") travels as
     * @DeliveryOrderType. The form also sets BranchesId, which FormHistoryNew never sends. NoOfRecords, ActionId,
     * RequestedById and ApprovedById are never set (0), so never sent.
     */
    public List<Map<String, Object>> formHistory(UserAccount u, int financialYearId, boolean canViewAll, int entryUser,
                                                 Timestamp fromDate, Timestamp toDate,
                                                 Timestamp entryFrom, Timestamp entryTo,
                                                 Timestamp modifyFrom, Timestamp modifyTo,
                                                 Timestamp approvedFrom, Timestamp approvedTo,
                                                 int docNoFrom, int docNoTo, int supplierCustomerId) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOCUMENT_TYPE_ID,
                "CanViewAllRecord", canViewAll);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (!canViewAll) p.put("EntryUser", entryUser);
        p.put("DeliveryOrderType", "StockTransfer");
        if (fromDate != null) p.put("FromDate", fromDate);
        if (toDate != null) p.put("ToDate", toDate);
        if (entryFrom != null) p.put("EntryFromDate", entryFrom);
        if (entryTo != null) p.put("EntryToDate", entryTo);
        if (modifyFrom != null) p.put("ModifyFromDate", modifyFrom);
        if (modifyTo != null) p.put("ModifyToDate", modifyTo);
        if (approvedFrom != null) p.put("ApprovedFromDate", approvedFrom);
        if (approvedTo != null) p.put("ApprovedToDate", approvedTo);
        if (docNoFrom != 0) p.put("DocNoFrom", docNoFrom);
        if (docNoTo != 0) p.put("DocNoTo", docNoTo);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        return DesktopProc.rows(jdbc, "SP_DeliveryOrderFormHistory", p);
    }

    // ============================================================================ read (DAL 0411 GetData)

    /** BLL 0558 GetByID → @Id, @Activity 'ReadById'. The header row, or null. */
    public Map<String, Object> header(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    /** DAL 0411 GetData — DocumentTypeId 84 and not "Export": @Activity 'ReadByIdDetailId'. */
    public List<Map<String, Object>> details(int id) {
        return DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadByIdDetailId"));
    }

    /** DAL 0411 GetData — DocumentTypeId 84 and not "Export": @Activity 'ReadInvDeliveryOrderExpensesByHeaderId'. */
    public List<Map<String, Object>> expenses(int id) {
        return DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadInvDeliveryOrderExpensesByHeaderId"));
    }

    // ============================================================================ save support

    /**
     * BLL 0558 DeliveryOrderStackWarningMessage:1879 for one detail of the list. dbo.USP_DeliveryOrderStackWarningMessage
     * always answers one row (WarningMessage defaults to ''), so the BLL's loop always stops at the first detail.
     */
    public String stackWarning(UserAccount u, int itemId, int warehouseId, int jobLotId, int packingTypeId,
                               int packUomId, Timestamp docDate, String cropYear, double doWeight) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (itemId != 0) p.put("ItemId", itemId);
        if (warehouseId != 0) p.put("WarehouseId", warehouseId);
        if (jobLotId != 0) p.put("JobLotId", jobLotId);
        if (packingTypeId != 0) p.put("InvPackingTypeId", packingTypeId);
        if (packUomId != 0) p.put("PackUomId", packUomId);
        if (docDate != null) p.put("DoDate", docDate);
        if (cropYear != null && !cropYear.isEmpty()) p.put("CropYear", cropYear);
        p.put("NetWeight", doWeight);
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "[dbo].[USP_DeliveryOrderStackWarningMessage]", p);
        if (r.isEmpty()) return null;                                   // no row: the BLL's loop goes on
        Object v = r.get(0).get("WarningMessage");
        return v == null ? "" : String.valueOf(v);
    }

    /** GenericProvider.SetProc — Convert.ToInt32(ExecuteScalar()). */
    public int setProc(String proc, Map<String, Object> model) { return DesktopProc.setProc(jdbc, proc, model); }

    // ============================================================================ delete / print

    /** CommonServices.RemoveByID(84, Id):16361 → BLL 0269 → USP_RecoredRemoveByOrgCompDocAndByID. */
    public void removeById(UserAccount u, int id) {
        DesktopProc.scalar(jdbc, "USP_RecoredRemoveByOrgCompDocAndByID", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOCUMENT_TYPE_ID,
                "Id", id,
                "UserId", u.getId()));
    }

    /**
     * CommonServices.InvDeliveryOrderSlip(Id, 84):9381 and DeliveryOrderSlip264(Id, 84):9339 — both
     * BLL 0131 InvGrnandGdnReports.InvDeliveryOrderSlip (Sp_InvDeliveryOrder_Slip); only the .rpt differs
     * (262-DeliveryOrderSlip.rpt / 264-DeliveryChallanByDeliveryOrder.rpt).
     */
    public List<Map<String, Object>> slip(UserAccount u, int id) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOCUMENT_TYPE_ID,
                "Id", id);
        if (u.getId() != null && u.getId() != 0) p.put("UserId", u.getId());
        return DesktopProc.rows(jdbc, "Sp_InvDeliveryOrder_Slip", p);
    }

    static boolean bool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        String s = v == null ? "" : String.valueOf(v).trim();
        return "true".equalsIgnoreCase(s) || "1".equals(s);
    }
}
