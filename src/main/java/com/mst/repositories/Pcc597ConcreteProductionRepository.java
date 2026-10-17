package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Pcc597 - screen 597 "Concrete Production" (Architecture.WinApp.Pcc.frmConcreteProduction, DocumentTypeId 1850): every procedure call of the form, one
 * method per BLL / CommonServices call. BLL: Architecture.BLL.pcc.ProductionHeader (0350), DAL 0288. Parameter lists are taken from procdure.utf8.sql
 * ([pcc] schema). A null / zero optional value is left out of the EXEC exactly as the BLL does, see {@link DesktopProc}.
 */
@Repository
public class Pcc597ConcreteProductionRepository {

    private static final String PROC_HEADER = "[pcc].[USP_ProductionHeader_GetAllMethod]";
    private static final String PROC_BOM = "[pcc].[USP_BomHeader_GetAllMethod]";

    private final JdbcTemplate jdbc;

    public Pcc597ConcreteProductionRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private List<Map<String, Object>> rows(String proc, Map<String, Object> p) { return DesktopProc.rows(jdbc, proc, p); }

    private static Map<String, Object> orgComp(UserAccount u) { return params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()); }

    // ================================================================================== lookups (frmConcreteProduction_Load, .cs 560-600)

    /** ProductionTypeFill (.cs 1176) - InvLookUp.GetLookupsByTypeIdDt(6) -> Sp_InvLookup_GetAllMethod 'ReadByInvlookTypeId'. */
    public List<Map<String, Object>> productionTypes(UserAccount u) {
        Map<String, Object> p = orgComp(u);
        p.put("InvLookupTypeId", 6);
        p.put("Activity", "ReadByInvlookTypeId");
        return rows("Sp_InvLookup_GetAllMethod", p);
    }

    /** PlantFeaderDbCall (.cs 1199) - InvProductionPlant.GetAll -> Sp_InvProductionPlant_GetAllMethod 'GetALL'. */
    public List<Map<String, Object>> plants(UserAccount u) {
        Map<String, Object> p = orgComp(u);
        p.put("Activity", "GetALL");
        return rows("Sp_InvProductionPlant_GetAllMethod", p);
    }

    /** WIPWarehouseFill (.cs 1227) - CommonServices.GetActiveWareHouseByWareHouseType(5) -> Sp_InvWareHouse_GetAllMethod (@WarehouseType). */
    public List<Map<String, Object>> warehousesByType(UserAccount u, int warehouseType) {
        Map<String, Object> p = orgComp(u);
        if (warehouseType != 0) p.put("WarehouseType", warehouseType);
        p.put("Activity", "GetActiveWareHouseByWareHouseType");
        return rows("Sp_InvWareHouse_GetAllMethod", p);
    }

    /** CommonServices.WareHouseAllocationToPlantsByProductionPlantId(PlantId) (.cs 1362) -> Sp_InvWareHouse_GetAllMethod (@PlantId only when non-zero). */
    public List<Map<String, Object>> plantWarehouses(UserAccount u, int plantId) {
        Map<String, Object> p = orgComp(u);
        if (plantId != 0) p.put("PlantId", plantId);
        p.put("Activity", "WareHouseAllocationToPlantsByProductionPlantId");
        return rows("Sp_InvWareHouse_GetAllMethod", p);
    }

    /** WIPAccountFill (.cs 1259) / WagesExpenseAccountFill (.cs 1291) - CoaAllocationAccountTitleByAccountTypeIds(ids) -> Sp_COAAllocation_GetAllMethod. */
    public List<Map<String, Object>> accountsByType(UserAccount u, String accountTypeIds) {
        Map<String, Object> p = orgComp(u);
        if (u.getAppId() != null && u.getAppId() != 0) p.put("AppId", u.getAppId());
        if (u.getId() != null && u.getId() != 0) p.put("UserId", u.getId());
        p.put("AccountTypeIds", accountTypeIds);
        p.put("Activity", "GetAccountTitleByAccountTypeIds");
        return rows("Sp_COAAllocation_GetAllMethod", p);
    }

    /** dtFillsForGrids (.cs 715) - CommonServices.JobLotGetAllService -> SP_JobLot_ReadMethod 'GetAll'. */
    public List<Map<String, Object>> jobLots(UserAccount u) {
        Map<String, Object> p = orgComp(u);
        p.put("Activity", "GetAll");
        return rows("SP_JobLot_ReadMethod", p);
    }

    /** dtFillsForGrids - SupplierCustomer.ReadByOrganizationCompanyIdForContractorWages. */
    public List<Map<String, Object>> contractors(UserAccount u) {
        Map<String, Object> p = orgComp(u);
        p.put("Activity", "ReadByOrganizationCompanyIdForContractorWages");
        return rows("Sp_SupplierCustomer_GetAllMethod", p);
    }

    /** CommonServies.GetSupplierCustomerListForFinancialEffects (BLL MakeVoucher) -> Id, GlAccountId. */
    public List<Map<String, Object>> supplierGl(UserAccount u) {
        Map<String, Object> p = orgComp(u);
        p.put("Activity", "GetGlAccountIdandCompanyNameBySupplierCustomerId");
        return rows("Sp_SupplierCustomer_GetAllMethod", p);
    }

    /** RefPartydtDbCall (.cs 1019) - ReferenceParties.ReferencePArtyByReferencePartyTypeIdOrSupplierCustomerId(ReferencePartyTypeId 1). */
    public List<Map<String, Object>> refParties(UserAccount u) {
        Map<String, Object> p = orgComp(u);
        p.put("ReferencePartyTypeId", 1);
        p.put("Activity", "ReferencePArtyByReferencePartyTypeIdOrSupplierCustomerId");
        return rows("Sp_ReferenceParties_GetAllMethod", p);
    }

    /** SupplierDtFillFromGlobal (.cs 997) - clsGlobalVariables.globalAllSupplierCustomer (Id, CompanyName, PartyCode, GlAccountId, CityName, MobilePersonal). */
    public List<Map<String, Object>> customers(UserAccount u) {
        return rows("[dbo].[USP_GetVendorsAndCustomersWithCityName]", orgComp(u));
    }

    /** SaleOrder.GetSaleOrderNo (BLL 0596) - Sp_SaleOrder_GetAllMethod 'ReadSaleOrderNo' (@OrderSupCustId, @ItemId only when non-zero). */
    public List<Map<String, Object>> saleOrders(UserAccount u, int customerId, int itemId) {
        Map<String, Object> p = orgComp(u);
        if (customerId != 0) p.put("OrderSupCustId", customerId);
        if (itemId != 0) p.put("ItemId", itemId);
        p.put("Activity", "ReadSaleOrderNo");
        return rows("Sp_SaleOrder_GetAllMethod", p);
    }

    /** AllItemsWithModal (clsGlobalVariables.getGlobalAllItems) used by ValidateItemTypeOfType (.cs 5783): Id, ItemName, ItemTypeOfTypeId. */
    public List<Map<String, Object>> allItems(UserAccount u) {
        return rows("[dbo].[USP_Item_AllItemsWithModal]", orgComp(u));
    }

    /** ItemAttributeVarient.GetAllForCombo(org, company, itemId) -> USP_ItemAttributeVarient_GetAllMethod 'GetAllForCombo'. */
    public List<Map<String, Object>> attributes(UserAccount u, int itemId) {
        Map<String, Object> p = orgComp(u);
        p.put("ItemId", itemId);
        p.put("Activity", "GetAllForCombo");
        return rows("USP_ItemAttributeVarient_GetAllMethod", p);
    }

    /** ProductionHeader.GetDataForDropDownFromProductionHeader (BLL 0350) - the ProductionNo combo (AllCombobind). */
    public List<Map<String, Object>> productionNoDropDown(UserAccount u) {
        return rows("[pcc].[USP_GetDataForDropDownFromProductionHeader]", orgComp(u));
    }

    /** ProductionHeader.GenerateCode -> 'GenerateCode' (DocNo). */
    public int generateCode(UserAccount u, int branchId, int docType, int financialYearId) {
        Map<String, Object> p = orgComp(u);
        p.put("BranchesId", branchId);
        p.put("FinancialYearId", financialYearId);
        p.put("DocumentTypeId", docType);
        p.put("Activity", "GenerateCode");
        List<Map<String, Object>> r = rows(PROC_HEADER, p);
        return r.isEmpty() ? 0 : toInt(ci(r.get(0), "DocNo"));
    }

    // ================================================================================== BOM / work order

    /** ItemdtFillFromBomDbCall (.cs 803) - BomHeader.GetAllBomItems: DocumentTypeId 1851, @TypeOfTypeId = production type when non-zero. */
    public List<Map<String, Object>> bomItems(UserAccount u, int branchId, int financialYearId, int typeOfTypeId) {
        Map<String, Object> p = orgComp(u);
        p.put("BranchesId", branchId);
        p.put("FinancialYearId", financialYearId);
        p.put("DocumentTypeId", 1851);
        if (typeOfTypeId != 0) p.put("TypeOfTypeId", typeOfTypeId);
        p.put("Activity", "GetAllBomItems");
        return rows(PROC_BOM, p);
    }

    /** BOMNoAndItemFill (.cs 924) - BomHeader.GetAllBomNoAndItems (DocumentTypeId 1851). */
    public List<Map<String, Object>> bomNosAndItems(UserAccount u, int branchId, int financialYearId) {
        Map<String, Object> p = orgComp(u);
        p.put("BranchesId", branchId);
        p.put("FinancialYearId", financialYearId);
        p.put("DocumentTypeId", 1851);
        p.put("Activity", "GetAllBomNoAndItems");
        return rows(PROC_BOM, p);
    }

    /** BomHeader.ReadById(Id): header + the input and the factory-overhead lists (DAL GetData). */
    public List<Map<String, Object>> bomRead(String activity, int id) {
        return rows(PROC_BOM, params("Id", id, "Activity", activity));
    }

    /** BomHeader.BomHeader_BomWagesDetailWithUpdatedRatesByHeaderId(BOMId) -> [pcc].[USP_BomHeader_BomWagesDetailWithUpdatedRatesByHeaderId] @Id. */
    public List<Map<String, Object>> bomWagesWithUpdatedRates(int bomId) {
        return rows("[pcc].[USP_BomHeader_BomWagesDetailWithUpdatedRatesByHeaderId]", params("Id", bomId));
    }

    /** grdInputMaterial_KeyDown F1 on ItemName - 'GetAlternativeItemsByMainItemId' (@ItemId = main item, @BomHeaderId). */
    public List<Map<String, Object>> alternativeItems(UserAccount u, int mainItemId, int bomHeaderId) {
        Map<String, Object> p = orgComp(u);
        p.put("ItemId", mainItemId);
        p.put("BomHeaderId", bomHeaderId);
        p.put("Activity", "GetAlternativeItemsByMainItemId");
        return rows(PROC_BOM, p);
    }

    /** WorkOrderDbCall (.cs 1079) - WorkOrderHeader.PendingWorkOrderForProduction (@RecId only when non-zero, @OrderDetailIds when given). */
    public List<Map<String, Object>> pendingWorkOrders(UserAccount u, int branchId, int financialYearId, int recId, String orderDetailIds) {
        Map<String, Object> p = orgComp(u);
        p.put("BranchesId", branchId);
        p.put("FinancialYearId", financialYearId);
        if (recId != 0) p.put("RecId", recId);
        if (orderDetailIds != null && !orderDetailIds.trim().isEmpty()) p.put("OrderDetailIds", orderDetailIds.trim());
        return rows("[pcc].[USP_PendingWorkOrderForProduction]", p);
    }

    // ================================================================================== stock / rates

    /** GetAvgRatesAndStockInHand.GetStockInHandAndAvgRateFromEvaluationConcrete(org, company, item, docDate, warehouse, 0, 0) -> QtyInHand, AvgRate. */
    public List<Map<String, Object>> stockInHand(UserAccount u, int itemId, Timestamp docDate, int warehouseId) {
        Map<String, Object> p = orgComp(u);
        p.put("ItemId", itemId);
        p.put("DocDate", docDate);
        if (warehouseId != 0) p.put("WarehouseId", warehouseId);
        p.put("Activity", "GetStockInHandAndAvgRateFromEvaluation");
        return rows("Sp_GetAvgRatesAndStockInHand_GetAllMethod", p);
    }

    /**
     * [pcc].[USP_GetStockByFifoMethod] as GetStockByFifoMethodConcrete / FIFOImplementionForConcrete call it: @WarehouseId, @JobLotId, @DocumentTypeId, @Id only
     * when non-zero, @FIFOXML only when earlier rows of the same save reserved layers. No @VarientId (stockUOM is never set by this screen).
     */
    public List<Map<String, Object>> fifoLayers(UserAccount u, int itemId, Timestamp docDate, int warehouseId, int jobLotId, int documentTypeId, int id, String fifoXml) {
        Map<String, Object> p = orgComp(u);
        p.put("ItemId", itemId);
        p.put("DocDate", docDate);
        if (warehouseId != 0) p.put("WarehouseId", warehouseId);
        if (jobLotId != 0) p.put("JobLotId", jobLotId);
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        if (id != 0) p.put("Id", id);
        if (fifoXml != null) p.put("FIFOXML", fifoXml);
        return rows("[pcc].[USP_GetStockByFifoMethod]", p);
    }

    /** ContractorWagesRateSchedule.GetServiceActivityByItemId(1, item, effectedDate, activityId, contractorId) - ContractorWagesRateScheduleId, WagesActivity, Rate, Qty. */
    public List<Map<String, Object>> wagesActivities(int itemId, Timestamp effectedDate, int activityId, int contractorId) {
        Map<String, Object> p = params();
        if (activityId != 0) p.put("ContractorWagesActivityId", activityId);
        if (contractorId != 0) p.put("ContractorId", contractorId);
        p.put("ItemId", itemId);
        p.put("RefDocumentTypeId", 1);
        p.put("EffectedDATE", effectedDate);
        p.put("Activity", "GetServiceActivityByItemId");
        return rows("[pcc].[USP_ContractorWagesRateSchedule_GetAllMethod]", p);
    }

    // ================================================================================== history / read / delete

    /** ProductionHeader.FormHistory (BLL 0350) -> 'FormHistory'. Zero / null filters are left out, @ActionId 1 = referred, 2 = not referred. */
    public List<Map<String, Object>> formHistory(UserAccount u, int branchId, int docType, int financialYearId, Timestamp fromDate, Timestamp toDate,
                                                 int fromDocNo, int toDocNo, int id, boolean canViewAll, int entryUserId, int actionId) {
        Map<String, Object> p = orgComp(u);
        p.put("BranchesId", branchId);
        p.put("DocumentTypeId", docType);
        p.put("FinancialYearId", financialYearId);
        if (fromDate != null) p.put("FromDate", fromDate);
        if (toDate != null) p.put("ToDate", toDate);
        if (fromDocNo != 0) p.put("FromDocNo", fromDocNo);
        if (toDocNo != 0) p.put("ToDocNo", toDocNo);
        if (id != 0) p.put("Id", id);
        p.put("CanViewAllRecord", canViewAll ? 1 : 0);
        if (!canViewAll) p.put("EntryUserId", entryUserId);
        if (actionId != 0) p.put("ActionId", actionId);
        p.put("Activity", "FormHistory");
        return rows(PROC_HEADER, p);
    }

    /** One 'ReadById' / 'ReadByHeaderId_*' read of [pcc].[USP_ProductionHeader_GetAllMethod] (DAL GetData). */
    public List<Map<String, Object>> read(String activity, int id) {
        return rows(PROC_HEADER, params("Id", id, "Activity", activity));
    }

    /** ProductionHeader.DeleteByID(EntryUserId, Id) -> 'DeleteById'. The procedure itself refuses approved / released / FIFO-referred documents. */
    public void deleteById(int entryUserId, int id) {
        rows(PROC_HEADER, params("EntryUserId", entryUserId, "Id", id, "Activity", "DeleteById"));
    }

    // ================================================================================== writes (DAL SetData, inside the caller's transaction)

    public int setHeader(boolean insert, Map<String, Object> p) {
        return DesktopProc.setProc(jdbc, insert ? "[pcc].[USP_ProductionHeader_Insert]" : "[pcc].[USP_ProductionHeader_Update]", p);
    }

    public int setOutput(Map<String, Object> p) { return DesktopProc.setProc(jdbc, "[pcc].[USP_ProductionOutPutDetail_Insert]", p); }

    public int setInput(Map<String, Object> p) { return DesktopProc.setProc(jdbc, "[pcc].[USP_ProductionInputDetail_Insert]", p); }

    public int setOverHead(Map<String, Object> p) { return DesktopProc.setProc(jdbc, "[pcc].[USP_ProductionOverHeadsDetail_Insert]", p); }

    public int setWages(Map<String, Object> p) { return DesktopProc.setProc(jdbc, "[pcc].[USP_ProductionWagesDetail_Insert]", p); }

    // ================================================================================== small converters

    public static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    public static int toInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        if (v instanceof Boolean) return ((Boolean) v) ? 1 : 0;
        String s = String.valueOf(v).trim().replace(",", "");
        if (s.isEmpty()) return 0;
        try { return (int) Double.parseDouble(s); } catch (NumberFormatException e) { return 0; }
    }
}
