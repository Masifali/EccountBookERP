package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 273 "Production Job Order (Pre Costing)" — Architecture.WinApp.Production.
 * ProductionPreCostingAndJobOrder (DocumentTypeId 401). The procedures its BLL/DAL calls, with the
 * desktop's own parameters (checked against procdure_index.csv):
 *
 * <pre>
 *   BLL 0335 InvProductionJobOrder (DAL 0273)
 *     GetGenerateCode                 Sp_InvProductionJobOrder_GetAllMethod 'GenerateCode'           (:1401)
 *     GetLastWipAccount               Sp_InvProductionJobOrder_GetAllMethod 'GetLastWipAccount'      (:1009)
 *     GetByID                         ... 'GetById' + Sp_InvProductionJobOrderInput_GetAllMethod @Id,
 *                                     Sp_InvProductionJobOrderOutput_GetAllMethod @Id,
 *                                     USP_InvProductionJobOrderOverHeads_GetAllMethod 'ReadByProductionJobOrderId',
 *                                     USP_InvProductionJobOrderPackingMaterial_GetAllMethod 'ReadByJobOrderId',
 *                                     ... 'GetPlantDetailByHeaderId', ... 'ReadById_InvProductionJobOrderLabStandardDetail'
 *     FormHistoryNew                  [dbo].[USP_ProductionJobOrder_FormHistory] (@DocumentTypeIds='401', :4688)
 *     JobOrderNo                      [dbo].[USP_GetDataForDropDownFromJobOrder]  (Activity column 'JobOrderNo')
 *     CheckJobOrderIdRefToInvFoodProduction  Sp_InvProductionJobOrderPartyProcessing_GetAllMethod (80 input / 112 output)
 *     CheckPlantExistinProductionagaintJobOrder  Sp_InvProductionJobOrder_GetAllMethod 'CheckPlantExistinProductionagaintJobOrder'
 *     Save (DAL 0273 SetData, one transaction)
 *                                     Sp_InvProductionJobOrder_Insert | _Update, then per row
 *                                     Sp_InvProductionJobOrderInput_Insert, Sp_InvProductionJobOrderOutput_Insert,
 *                                     USP_InvProductionJobOrderPackingMaterial_Insert, USP_InvProductionJobOrderOverHeads_Insert,
 *                                     Sp_InvProductionJobOrderPlant_Insert, USP_InvProductionJobOrderLabStandardDetail_Insert,
 *                                     Proc_JobLot_Insert when AutoJobLotCreateOnJobOrder and ActionId 1 (DAL 0273:138)
 *   BLL 0020 PlantsAllocationToBranch.GetPlantsAllocatedToBranch   [dbo].[USP_GetPlantsAllocatedToBranch]
 *   JobLotsAllocationToBranch.GetJobLotsAllocatedToBranchByBranchId [dbo].[USP_GetJobLotsAllocatedToBranch]
 *   clsGlobalVariables.globalWarehousesWithBranches                 USP_GetWarehousesAllocatedToBranch
 *   CommonServices.CropYearGetAllService                            Sp_InvCropYear_GetAllMethod 'ReadAll'
 *   InvPackingType.Getall()                                          Sp_InvPackingType_GetAllMethod 'ReadAll' (no tenancy)
 *   ProductionType.GetAll                                            Sp_ProductionType_GetAllMethod
 *   CommonServices.ItemGetForComboServiceBind(6)                     Sp_Item_GetAllMethod @InventoryParentCategoriesId=6 'ReadAllForComboTwoColumns'
 *   CommonServices.CoaAllocationAccountTitleByAccountTypeIds("4")    Sp_COAAllocation_GetAllMethod 'GetAccountTitleByAccountTypeIds'
 *   SaleOrder.GetSaleOrderNo / ReadByPurchaseOrderIDNOrderItemId     Sp_SaleOrder_GetAllMethod 'ReadSaleOrderNo' / 'ReadBySaleOrderIDNOrderItemId'
 *   CommonServices.GetLcOrderNo / ExImLcOrder.ReadByExportSaleOrderIDNOrderItemId  Sp_ExImLcOrder_GetAllMethod
 *   ItemType.Getall / ItemCategory.Getall                            Sp_ItemType_GetAllMethod / Sp_ItemCategory_GetAllMethod 'ReadByOrganizationCompanyId'
 *   LabAnalysisScheduleItemAndPartyWise  _GetSupplierCustomer, ComboFill, ReadByMainId → USP_LabAnalysisScheduleItemAndPartyWise_*
 *   CommonServices.GetAvailableStockFromTransactionOrEvaluation(..., 2, 1)  usp_getAvailableStock @WithouStockLogic=1 @ActionId=2
 *   LoadavailableTransactionsForIssuance (loader form, source not recovered)
 *                                     USP_Inventory_StockEvalautionDetail_DropDownAndLists,
 *                                     SpInventoryTransactionEvaluation_GetAvailableTransactionsForIssuance
 * </pre>
 */
@Repository
public class Prod273PreCostingJobOrderRepository {

    private static final Logger LOG = LoggerFactory.getLogger(Prod273PreCostingJobOrderRepository.class);

    public static final String P_GET = "Sp_InvProductionJobOrder_GetAllMethod";

    private final JdbcTemplate jdbc;
    public Prod273PreCostingJobOrderRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private List<Map<String, Object>> rows(String proc, Map<String, Object> p) { return DesktopProc.rows(jdbc, proc, p); }

    private List<Map<String, Object>> safe(String label, String proc, Map<String, Object> p) {
        try {
            return DesktopProc.rows(jdbc, proc, p);
        } catch (RuntimeException e) {
            LOG.warn("Screen 273 lookup '{}' ({}) failed", label, proc, e);
            return new ArrayList<>();
        }
    }

    // ================================================================================ header reads

    /** GeneratePlanCode:1401 — GetGenerateCode(401, ActiveYr) → PlanCode column of row 0 (@FinancialYearId when non-zero). */
    public int nextCode(UserAccount u, int documentTypeId, int financialYearId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", documentTypeId);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        p.put("Activity", "GenerateCode");
        List<Map<String, Object>> r = rows(P_GET, p);
        return r.isEmpty() ? 0 : firstInt(r.get(0), "PlanCode");
    }

    /** GetLastSaveAccount:1009 — GetLastWipAccount(401): AccountId, WipItemId, WipWareHouseId of row 0. */
    public Map<String, Object> lastWipAccount(UserAccount u, int documentTypeId) {
        List<Map<String, Object>> r = safe("lastWipAccount", P_GET, params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId, "Activity", "GetLastWipAccount"));
        return r.isEmpty() ? null : r.get(0);
    }

    /** BLL 0335 GetByID → 'GetById'. */
    public Map<String, Object> header(int id) {
        List<Map<String, Object>> r = rows(P_GET, params("Id", id, "Activity", "GetById"));
        return r.isEmpty() ? null : r.get(0);
    }

    public List<Map<String, Object>> inputRows(int headerId) {
        return rows("Sp_InvProductionJobOrderInput_GetAllMethod", params("Id", headerId));
    }

    public List<Map<String, Object>> outputRows(int headerId) {
        return rows("Sp_InvProductionJobOrderOutput_GetAllMethod", params("Id", headerId));
    }

    public List<Map<String, Object>> overheadRows(UserAccount u, int headerId) {
        return rows("USP_InvProductionJobOrderOverHeads_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", headerId, "Activity", "ReadByProductionJobOrderId"));
    }

    public List<Map<String, Object>> packingRows(UserAccount u, int headerId) {
        return rows("USP_InvProductionJobOrderPackingMaterial_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", headerId, "Activity", "ReadByJobOrderId"));
    }

    public List<Map<String, Object>> plantRows(int headerId) {
        return rows(P_GET, params("Id", headerId, "Activity", "GetPlantDetailByHeaderId"));
    }

    public List<Map<String, Object>> labRows(int headerId) {
        return rows(P_GET, params("Id", headerId, "Activity", "ReadById_InvProductionJobOrderLabStandardDetail"));
    }

    // ================================================================================ guards

    /** grdInputDetail_DoubleClick:2549 (80) / grdPlannedOutput_DoubleClick:3339 (112) — first column of row 0. */
    public int referencedByProduction(UserAccount u, int jobOrderId, int productionDocumentTypeId) {
        List<Map<String, Object>> r = rows("Sp_InvProductionJobOrderPartyProcessing_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "InvJobOrderId", jobOrderId, "DocumentTypeId", productionDocumentTypeId,
                "Activity", "CheckJobOrderIdRefToInvFoodProduction"));
        return r.isEmpty() ? 0 : firstInt(r.get(0), null);
    }

    /** grdPlantdetail_ColumnButtonClick:6652 — CheckPlantExistinProductionagaintJobOrder → bool of row 0. */
    public boolean plantUsedInProduction(UserAccount u, int jobOrderId, int plantId) {
        List<Map<String, Object>> r = rows(P_GET, params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", jobOrderId,
                "PlantId", plantId, "Activity", "CheckPlantExistinProductionagaintJobOrder"));
        if (r.isEmpty() || r.get(0).isEmpty()) return false;
        Object v = r.get(0).values().iterator().next();
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        return v != null && ("1".equals(String.valueOf(v).trim()) || "true".equalsIgnoreCase(String.valueOf(v).trim()));
    }

    // ================================================================================ history

    /**
     * BindHistoryGrid:4679 — FormHistoryNew with DocumentTypeIds "401", CanViewAllRecord, EntryUser,
     * FinancialYearId, the date pair of the checked radio (both pickers have no ShowCheckBox → always
     * Checked), FromDocNo / ToDocNo / InvJobOrderId (the BLL adds each only when non-zero / non-null).
     */
    public List<Map<String, Object>> formHistory(UserAccount u, int financialYearId, boolean canViewAll, int entryUser,
                                                 String dateMode, Timestamp from, Timestamp to,
                                                 int docNoFrom, int docNoTo, int invJobOrderId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        p.put("DocumentTypeIds", "401");
        String fromName, toName;
        String mode = dateMode == null ? "doc" : dateMode.trim().toLowerCase();
        if ("entry".equals(mode))         { fromName = "EntryFromDate";    toName = "EntryToDate"; }
        else if ("modify".equals(mode))   { fromName = "ModifyFromDate";   toName = "ModifyToDate"; }
        else if ("approved".equals(mode)) { fromName = "ApprovedFromDate"; toName = "ApprovedToDate"; }
        else                              { fromName = "FromDate";         toName = "ToDate"; }
        if (from != null) p.put(fromName, from);
        if (to != null) p.put(toName, to);
        if (docNoFrom != 0) p.put("DocNoFrom", docNoFrom);
        if (docNoTo != 0) p.put("DocNoTo", docNoTo);
        p.put("CanViewAllRecord", canViewAll);
        if (!canViewAll) p.put("EntryUser", entryUser);
        if (invJobOrderId != 0) p.put("InvJobOrderId", invJobOrderId);
        return rows("[dbo].[USP_ProductionJobOrder_FormHistory]", p);
    }

    /** HistoryJobOrderNofill:4575 — InvProductionJobOrder.JobOrderNo → rows whose Activity = "JobOrderNo". */
    public List<Map<String, Object>> jobOrderNoDropDown(UserAccount u) {
        return safe("jobOrderNoDropDown", "[dbo].[USP_GetDataForDropDownFromJobOrder]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    // ================================================================================ pickers

    public List<Map<String, Object>> plantsAllocatedToBranch(UserAccount u) {
        return safe("plants", "[dbo].[USP_GetPlantsAllocatedToBranch]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    public List<Map<String, Object>> jobLotsAllocatedToBranch(UserAccount u, int branchId) {
        return safe("jobLots", "[dbo].[USP_GetJobLotsAllocatedToBranch]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BranchId", branchId));
    }

    public List<Map<String, Object>> warehousesAllocatedToBranch(UserAccount u, int branchId) {
        return safe("warehouses", "USP_GetWarehousesAllocatedToBranch", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BranchId", branchId));
    }

    public List<Map<String, Object>> cropYears(UserAccount u) {
        return safe("cropYears", "Sp_InvCropYear_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    /** InvPackingType.Getall() — @Activity alone; the desktop passes no tenancy. */
    public List<Map<String, Object>> packingTypes() {
        return safe("packingTypes", "Sp_InvPackingType_GetAllMethod", params("Activity", "ReadAll"));
    }

    /** ProductionType.GetAll(new ProductionType()) — Id 0 → no parameter. */
    public List<Map<String, Object>> productionTypes() {
        return safe("productionTypes", "Sp_ProductionType_GetAllMethod", params());
    }

    /** ItemFill:1423 — ItemGetForComboServiceBind(6) → parent inventory category 6. */
    public List<Map<String, Object>> wipItems(UserAccount u) {
        return safe("wipItems", "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "InventoryParentCategoriesId", 6, "Activity", "ReadAllForComboTwoColumns"));
    }

    /** AccountFills:1379 — CoaAllocationAccountTitleByAccountTypeIds("4"). */
    public List<Map<String, Object>> accountsByType(UserAccount u, int appId, String accountTypeIds) {
        return safe("accountsByType", "Sp_COAAllocation_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "AppId", appId,
                "AccountTypeIds", accountTypeIds, "UserId", u.getId(), "Activity", "GetAccountTitleByAccountTypeIds"));
    }

    /** GetLocalSaleOrderNo:1307 — SaleOrder.GetSaleOrderNo (Id, DocNo). */
    public List<Map<String, Object>> saleOrderNumbers(UserAccount u) {
        return safe("saleOrderNumbers", "Sp_SaleOrder_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadSaleOrderNo"));
    }

    /** GetExportContractNo:1283 — CommonServices.GetLcOrderNo (Id, LcOrderNo). */
    public List<Map<String, Object>> lcOrderNumbers(UserAccount u) {
        return safe("lcOrderNumbers", "Sp_ExImLcOrder_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetLcOrderNo"));
    }

    /** GetItemTypes:1335 — ItemType.Getall (Id, TypeDescription). */
    public List<Map<String, Object>> itemTypes(UserAccount u) {
        return safe("itemTypes", "Sp_ItemType_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByOrganizationCompanyId"));
    }

    /** GetItemCategory:1359 — ItemCategory.Getall (Id, CategoryDescription). */
    public List<Map<String, Object>> itemCategories(UserAccount u) {
        return safe("itemCategories", "Sp_ItemCategory_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByOrganizationCompanyId"));
    }

    /** ItemDetailFillBySaleOrder:5315 — SaleOrder.ReadByPurchaseOrderIDNOrderItemId (OrderItemId, ItemName). */
    public List<Map<String, Object>> itemsBySaleOrder(int saleOrderId) {
        return safe("itemsBySaleOrder", "Sp_SaleOrder_GetAllMethod", params("SaleOrderId", saleOrderId, "Activity", "ReadBySaleOrderIDNOrderItemId"));
    }

    /** ItemDetailFillByExportSaleContractId:5346 — ExImLcOrder.ReadByExportSaleOrderIDNOrderItemId (ExImItemId, ItemName). */
    public List<Map<String, Object>> itemsByExportContract(UserAccount u, int lcOrderId) {
        return safe("itemsByExportContract", "Sp_ExImLcOrder_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", lcOrderId, "Activity", "ReadByExportSaleOrderIDNOrderItemId"));
    }

    // ================================================================================ lab standards

    /** dtPartiesFromStandardDbCall:6850 — LabAnalysisScheduleItemAndPartyWise_GetSupplierCustomer. */
    public List<Map<String, Object>> partiesFromStandard(UserAccount u) {
        return safe("partiesFromStandard", "USP_LabAnalysisScheduleItemAndPartyWise_GetSupplierCustomer", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** AnalysisGroupDbCall:6954 — ComboFill(BrandId, SupplierCustomerId) → MainId, ScheduleName. */
    public List<Map<String, Object>> analysisGroups(UserAccount u, int brandItemId, int supplierCustomerId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BrandItemId", brandItemId);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        p.put("Activity", "ComboFill");
        return safe("analysisGroups", "USP_LabAnalysisScheduleItemAndPartyWise_GetAllMethod", p);
    }

    /** GridFillByAnalysisGroup:7019 — ReadByMainId → the schedule rows (filtered on Active by the form). */
    public List<Map<String, Object>> analysisGroupRows(UserAccount u, int mainId) {
        return safe("analysisGroupRows", "USP_LabAnalysisScheduleItemAndPartyWise_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "MainId", mainId, "Activity", "ReadById"));
    }

    // ================================================================================ stock / loader

    /** cmbItemInput_Leave:2275 — GetAvailableStockFromTransactionOrEvaluation(id, PlanDate, 0,0,null,0,0, 2, 1).BalWeight. */
    public double availableStockWeight(UserAccount u, int itemId, Timestamp docDate) {
        List<Map<String, Object>> r = rows("usp_getAvailableStock", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ItemId", itemId, "DateTo", docDate,
                "WithouStockLogic", 1, "ActionId", 2));
        if (r.isEmpty()) return 0d;
        Object w = r.get(0).get("BalWeight");
        return w instanceof Number ? ((Number) w).doubleValue() : 0d;
    }

    /** LoadavailableTransactionsForIssuance.StockComboFill → USP_Inventory_StockEvalautionDetail_DropDownAndLists. */
    public List<Map<String, Object>> issuanceDropDowns(UserAccount u, int branchId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (branchId != 0) p.put("BranchesIds", String.valueOf(branchId));
        return safe("issuanceDropDowns", "USP_Inventory_StockEvalautionDetail_DropDownAndLists", p);
    }

    /** PendingInventoryTransactionsForIssuanceLoad → every filter guarded (non-zero / non-empty). */
    public List<Map<String, Object>> availableTransactionsForIssuance(UserAccount u, int branchId, Timestamp from, Timestamp to,
                                                                      int warehouseId, int itemId, int jobLotId, String cropYear,
                                                                      int refDocumentTypeId, int supplierCustomerId,
                                                                      int parentCategory, int itemCategoryId, int itemTypeId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (branchId != 0) p.put("BranchesId", branchId);
        if (from != null) p.put("DateFrom", from);
        if (to != null) p.put("DateTo", to);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (warehouseId != 0) p.put("WarehouseId", warehouseId);
        if (itemId != 0) p.put("ItemId", itemId);
        if (refDocumentTypeId != 0) p.put("ReferenceDocumentTypeId", refDocumentTypeId);
        if (jobLotId != 0) p.put("JobLotId", jobLotId);
        if (parentCategory != 0) p.put("InventoryParentCategories", parentCategory);
        if (itemCategoryId != 0) p.put("ItemCategoryId", itemCategoryId);
        if (itemTypeId != 0) p.put("ItemTypeId", itemTypeId);
        if (cropYear != null && !cropYear.trim().isEmpty()) p.put("CropYear", cropYear.trim());
        return rows("SpInventoryTransactionEvaluation_GetAvailableTransactionsForIssuance", p);
    }

    // ================================================================================ save

    public int saveHeader(boolean insert, Map<String, Object> head) {
        return DesktopProc.setProc(jdbc, insert ? "Sp_InvProductionJobOrder_Insert" : "Sp_InvProductionJobOrder_Update", head);
    }

    public void insertInput(Map<String, Object> p)    { DesktopProc.setProc(jdbc, "Sp_InvProductionJobOrderInput_Insert", p); }
    public void insertOutput(Map<String, Object> p)   { DesktopProc.setProc(jdbc, "Sp_InvProductionJobOrderOutput_Insert", p); }
    public void insertPacking(Map<String, Object> p)  { DesktopProc.setProc(jdbc, "USP_InvProductionJobOrderPackingMaterial_Insert", p); }
    public void insertOverhead(Map<String, Object> p) { DesktopProc.setProc(jdbc, "USP_InvProductionJobOrderOverHeads_Insert", p); }
    public void insertPlant(Map<String, Object> p)    { DesktopProc.setProc(jdbc, "Sp_InvProductionJobOrderPlant_Insert", p); }
    public void insertLab(Map<String, Object> p)      { DesktopProc.setProc(jdbc, "USP_InvProductionJobOrderLabStandardDetail_Insert", p); }
    public void insertJobLot(Map<String, Object> p)   { DesktopProc.setProc(jdbc, "Proc_JobLot_Insert", p); }

    // ================================================================================ helpers

    private static int firstInt(Map<String, Object> r, String column) {
        if (r == null || r.isEmpty()) return 0;
        Object v = null;
        if (column != null) {
            for (Map.Entry<String, Object> e : r.entrySet()) if (e.getKey().equalsIgnoreCase(column)) { v = e.getValue(); break; }
        }
        if (v == null && (column == null || !r.containsKey(column))) v = r.values().iterator().next();
        if (v instanceof Number) return ((Number) v).intValue();
        if (v == null) return 0;
        try { return Integer.parseInt(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0; }
    }
}
