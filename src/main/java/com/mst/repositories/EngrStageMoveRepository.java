package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screens 807 / 846 "Production Move (Stage Shift)" - Architecture.WinApp.Mfg.frmStockTransferManual_Engr (DocumentTypeId 1663, rights key
 * base.Name "frmStockTransferManual_Engr"): every procedure call of the form, one method per BLL / CommonServices call.
 *
 * This is NOT the Store form frmStockTransferManual (806, DAL 0412 dbo.Sp_InvStockTransfer*): it uses the [Mfg] schema through
 *   BLL 0363 Architecture.BLL.Mfg.InvStockTransferHeader (Save / ReadById / FormHistory / GenerateCode / DeleteByID / StockTransferRegister),
 *   DAL 0316 Architecture.DAL.Mfg.InvStockTransferHeader.SetData, Models 0535 / 0536,
 *   BLL 0364 WarehouseAndProductionStagesAllocateToStockTransferType, BLL 0370 Mfg.LookUps, BLL 0373 ProductionWorkStation, BLL 0368 WorkOrder,
 *   BLL 0574 InventoryStockEvalautionDetail.GetAvailableQtyByWarehouseAndProductionStage, BLL 0511 ItemModel.ItemModel_GetByItemIds.
 * Parameter lists come from procdure.utf8.sql. A null value is left out of the EXEC, as in {@link DesktopProc}.
 */
@Repository
public class EngrStageMoveRepository {

    private final JdbcTemplate jdbc;

    public EngrStageMoveRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private List<Map<String, Object>> rows(String proc, Map<String, Object> p) { return DesktopProc.rows(jdbc, proc, p); }

    private static Map<String, Object> orgComp(UserAccount u) { return params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()); }

    // ================================================================================== lookups

    /** clsGlobalVariables.GetAllLookUps_Engr = BLL 0370 LookUps.FormHistoryWithList -> [Mfg].[USP_LookUps_GetAllMethod] 'FormHistory' (Id, LookUpTypeId, Code, LookupName, GLAccountId). */
    public List<Map<String, Object>> allLookups() {
        return rows("[Mfg].[USP_LookUps_GetAllMethod]", params("Activity", "FormHistory"));
    }

    /** CommonServices.GetLookupsByTypeId_Engr(typeId) -> BLL 0370 LookUps.GetDataByTypeId -> 'GetDataByTypeId' (@Id = lookup type id). */
    public List<Map<String, Object>> lookupsByType(int typeId) {
        return rows("[Mfg].[USP_LookUps_GetAllMethod]", params("Id", typeId, "Activity", "GetDataByTypeId"));
    }

    /** WareHouseFill:988 - WarehousesAllocationToBranch.GetWarehousesAllocatedToBranchByBranchId (org / company only: BranchesId is 0 on the ReportsParameters). */
    public List<Map<String, Object>> warehouses(UserAccount u) {
        return rows("[dbo].[USP_GetWarehousesAllocatedToBranch]", orgComp(u));
    }

    /** WorkStationFill:1212 - BLL 0373 ProductionWorkStation.FormHistory (org, company, BranchesId = user's branch) 'FormHistory' (Id, WorkStationName, ...). */
    public List<Map<String, Object>> workStations(UserAccount u, int branchId) {
        Map<String, Object> p = orgComp(u);
        p.put("BranchesId", branchId);
        p.put("Activity", "FormHistory");
        return rows("[Mfg].[USP_ProductionWorkStation_GetAllMethod]", p);
    }

    /** WorkOrderFill:1267 - BLL 0368 WorkOrder.GetWorkOrdersForStockTransfer 'GetWorkOrderNoForStockTransfer' (Id, WorkOrderNo). */
    public List<Map<String, Object>> workOrders(UserAccount u) {
        Map<String, Object> p = orgComp(u);
        p.put("Activity", "GetWorkOrderNoForStockTransfer");
        return rows("[Mfg].[USP_WorkOrder_GetAllMethod]", p);
    }

    /** GetTransferTypeData:1356 - BLL 0364 GetWarehouseAndProductionStageDataByTransferTypeId (FromWarehouseId, ToWarehouseId, FromProductionStageId, ToProductionStageId, ...). */
    public List<Map<String, Object>> typeAllocation(UserAccount u, int transferTypeId) {
        Map<String, Object> p = orgComp(u);
        p.put("TransferTypeId", transferTypeId);
        return rows("[Mfg].[usp_getWarehouseAndProductionStageDataByTransferTypeId]", p);
    }

    /** CmbTransferType_Leave:1428 - BLL 0574 InventoryStockEvalautionDetail.GetAvailableQtyByWarehouseAndProductionStage -> dbo.usp_getAvailableQtyByWarehouseAndProductionStage. */
    public List<Map<String, Object>> availableStock(UserAccount u, int warehouseId, int productionStageId, Object asOnDate) {
        Map<String, Object> p = orgComp(u);
        p.put("WarehouseId", warehouseId);
        p.put("ProductionStageId", productionStageId);
        p.put("AsOnDate", asOnDate);
        return rows("[dbo].[usp_getAvailableQtyByWarehouseAndProductionStage]", p);
    }

    /** SampleModaldBCall:782 - ItemModel.ItemModel_GetByItemIds -> [item].[USP_ItemModel_GetByItemIds] (itemModalId, ItemModal, ItemSampleNo, ItemCategoryId, ItemId). */
    public List<Map<String, Object>> itemModels(UserAccount u, String itemIds) {
        Map<String, Object> p = orgComp(u);
        p.put("ItemIds", itemIds);
        return rows("[item].[USP_ItemModel_GetByItemIds]", p);
    }

    /** ItemUomBind:959 - ItemAttributeVarient.GetAllForCombo -> USP_ItemAttributeVarient_GetAllMethod 'GetAllForCombo' (ItemAttributeVarientId, ItemAttribute). */
    public List<Map<String, Object>> attributes(UserAccount u, int itemId) {
        Map<String, Object> p = orgComp(u);
        p.put("ItemId", itemId);
        p.put("Activity", "GetAllForCombo");
        return rows("USP_ItemAttributeVarient_GetAllMethod", p);
    }

    // ================================================================================== BLL 0363 reads

    /** DocumentNoDBCall:531 - BLL 0363 GenerateCode (@BranchesId, @FinancialYearId, @DocumentTypeId 1663). */
    public List<Map<String, Object>> generateCode(UserAccount u, int branchId, int documentTypeId, int financialYearId) {
        Map<String, Object> p = orgComp(u);
        p.put("BranchesId", branchId);
        p.put("FinancialYearId", financialYearId);
        p.put("DocumentTypeId", documentTypeId);
        p.put("Activity", "GenerateCode");
        return rows("[Mfg].[USP_InvStockTransferHeader_GetAllMethod]", p);
    }

    /** DAL 0316 GetData: 'ReadById' header. */
    public List<Map<String, Object>> readHeader(int id) {
        return rows("[Mfg].[USP_InvStockTransferHeader_GetAllMethod]", params("Id", id, "Activity", "ReadById"));
    }

    /** DAL 0316 GetData: 'ReadByHeaderId_InvStockTransferDetail' (the @Id is the header id). */
    public List<Map<String, Object>> readDetails(int headerId) {
        return rows("[Mfg].[USP_InvStockTransferHeader_GetAllMethod]", params("Id", headerId, "Activity", "ReadByHeaderId_InvStockTransferDetail"));
    }

    /** gridhistoryfill:3105 - BLL 0363 FormHistory. */
    public List<Map<String, Object>> formHistory(UserAccount u, int branchId, int documentTypeId, int financialYearId, Object fromDate, Object toDate,
                                                 int fromDocNo, int toDocNo, boolean canViewAll, int entryUserId) {
        Map<String, Object> p = orgComp(u);
        p.put("BranchesId", branchId);
        p.put("DocumentTypeId", documentTypeId);
        p.put("FinancialYearId", financialYearId);
        if (fromDate != null) p.put("FromDate", fromDate);
        if (toDate != null) p.put("ToDate", toDate);
        if (fromDocNo != 0) p.put("FromDocNo", fromDocNo);
        if (toDocNo != 0) p.put("ToDocNo", toDocNo);
        p.put("CanViewAllRecord", canViewAll ? 1 : 0);
        if (!canViewAll) p.put("EntryUserId", entryUserId);
        p.put("Activity", "FormHistory");
        return rows("[Mfg].[USP_InvStockTransferHeader_GetAllMethod]", p);
    }

    /** btnDelete_Click:2453 - BLL 0363 DeleteByID (@EntryUserId, @Id, 'DeleteById'). */
    public void deleteById(int entryUserId, int id) {
        DesktopProc.scalar(jdbc, "[Mfg].[USP_InvStockTransferHeader_GetAllMethod]", params("EntryUserId", entryUserId, "Id", id, "Activity", "DeleteById"));
    }

    // ================================================================================== DAL 0316 SetData (inside the caller's transaction)

    public int setHeader(Map<String, Object> p) { return DesktopProc.setProc(jdbc, "[Mfg].[USP_InvStockTransferHeader_InsertAndUpdate]", p); }

    public int setDetail(Map<String, Object> p) { return DesktopProc.setProc(jdbc, "[Mfg].[USP_InvStockTransferDetail_Insert]", p); }

    /** DAL 0316:70 - InventoryTransactions model (non-null defaults, as GenericProvider sends them) -> Sp_InventoryTransactions_GetALLMethod. */
    public void inventoryTransactions(UserAccount u, int documentTypeId, int headerId) {
        Map<String, Object> p = InventoryOpeningDefaults.transactions();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("RefDocumentTypeId", documentTypeId);
        p.put("RefDocIdNo", headerId);
        DesktopProc.setProc(jdbc, "Sp_InventoryTransactions_GetALLMethod", p);
    }

    /** DAL 0316:77 - InventoryStockEvalautionDetail model (non-null defaults) -> Sp_InventoryStockEvalautionDetail_Update. */
    public void stockEvaluationUpdate(UserAccount u, int documentTypeId, int headerId) {
        Map<String, Object> p = InventoryOpeningDefaults.evaluation();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("RefDocumentTypeId", documentTypeId);
        p.put("RefDocIdNo", headerId);
        DesktopProc.setProc(jdbc, "Sp_InventoryStockEvalautionDetail_Update", p);
    }

    /** DAL 0316:87 - USP_InventoryValidation (documentType 1663 / 1665 only): VarientId, ProductionStageId, WorkOrderId, WorkStationId when > 0, BatchNo when not empty. */
    public void inventoryValidation(UserAccount u, int documentTypeId, Object docDate, int itemId, int warehouseId, int variantId, int stageId,
                                    int workOrderId, int workStationId, String batchNo, double netWeight) {
        Map<String, Object> p = orgComp(u);
        p.put("DocumentTypeId", documentTypeId);
        p.put("DocDate", docDate);
        p.put("ItemId", itemId);
        p.put("WarehouseId", warehouseId);
        if (variantId > 0) p.put("VarientId", variantId);
        if (stageId > 0) p.put("ProductionStageId", stageId);
        if (workOrderId > 0) p.put("WorkOrderId", workOrderId);
        if (workStationId > 0) p.put("WorkStationId", workStationId);
        if (batchNo != null && !batchNo.isEmpty()) p.put("BatchNo", batchNo);
        p.put("NetWeight", netWeight);
        DesktopProc.scalar(jdbc, "USP_InventoryValidation", p);
    }

    public JdbcTemplate jdbc() { return jdbc; }
}
