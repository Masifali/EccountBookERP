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

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Pcc822 - screen 822 "Conversion" (Architecture.WinApp.pcc.frmConversion, DocumentTypeId 1867): every procedure call of the form, one method per
 * BLL / CommonServices call. BLL: Architecture.BLL.pcc.Production.ConversionHeader (0362), DAL 0297. Parameter lists are taken from procdure.utf8.sql
 * ([pcc] schema). A null value is left out of the EXEC (ADO.NET AddWithValue(null)), as in {@link DesktopProc}.
 */
@Repository
public class Pcc822ConversionRepository {

    private final JdbcTemplate jdbc;

    public Pcc822ConversionRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private List<Map<String, Object>> rows(String proc, Map<String, Object> p) { return DesktopProc.rows(jdbc, proc, p); }

    private static Map<String, Object> orgComp(UserAccount u) { return params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()); }

    // ================================================================================== lookups (frmConversion_Load)

    /** ProductionTypeFill:840 - InvLookUp.GetLookupsByTypeIdDt(InvLookupTypeId 6) -> Sp_InvLookup_GetAllMethod 'ReadByInvlookTypeId'. */
    public List<Map<String, Object>> productionTypes(UserAccount u) {
        Map<String, Object> p = orgComp(u);
        p.put("InvLookupTypeId", 6);
        p.put("Activity", "ReadByInvlookTypeId");
        return rows("Sp_InvLookup_GetAllMethod", p);
    }

    /** PlantFeader:903 - InvProductionPlant.GetAll -> Sp_InvProductionPlant_GetAllMethod 'GetALL'. */
    public List<Map<String, Object>> plants(UserAccount u) {
        Map<String, Object> p = orgComp(u);
        p.put("Activity", "GetALL");
        return rows("Sp_InvProductionPlant_GetAllMethod", p);
    }

    /** CommonServices.GetItemByItemTypeId(ids) -> Sp_Item_GetAllMethod 'GetItemByItemTypeId' (Id, ItemName, ItemCodeNew, InventoryParentCategoriesId). */
    public List<Map<String, Object>> itemsByType(UserAccount u, String lookupTypeIds) {
        Map<String, Object> p = orgComp(u);
        p.put("LookupTypeIds", lookupTypeIds);
        p.put("Activity", "GetItemByItemTypeId");
        return rows("Sp_Item_GetAllMethod", p);
    }

    /** ItemAttributeVarient.GetAllForCombo(org, company, itemId) -> USP_ItemAttributeVarient_GetAllMethod 'GetAllForCombo'. */
    public List<Map<String, Object>> attributes(UserAccount u, int itemId) {
        Map<String, Object> p = orgComp(u);
        p.put("ItemId", itemId);
        p.put("Activity", "GetAllForCombo");
        return rows("USP_ItemAttributeVarient_GetAllMethod", p);
    }

    /** CommonServices.WareHouseAllocationToPlantsByProductionPlantId(PlantId) -> Sp_InvWareHouse_GetAllMethod (@PlantId only when non-zero). */
    public List<Map<String, Object>> plantWarehouses(UserAccount u, int plantId) {
        Map<String, Object> p = orgComp(u);
        if (plantId != 0) p.put("PlantId", plantId);
        p.put("Activity", "WareHouseAllocationToPlantsByProductionPlantId");
        return rows("Sp_InvWareHouse_GetAllMethod", p);
    }

    /** CommonServices.getActiveWareHouse -> InvWareHouse.GetActiveWareHouse -> Sp_InvWareHouse_GetAllMethod 'GetActiveWareHouse'. */
    public List<Map<String, Object>> activeWarehouses(UserAccount u) {
        Map<String, Object> p = orgComp(u);
        p.put("Activity", "GetActiveWareHouse");
        return rows("Sp_InvWareHouse_GetAllMethod", p);
    }

    /** SupplierCustomer.ReadByOrganizationCompanyIdForContractorWages. */
    public List<Map<String, Object>> contractors(UserAccount u) {
        Map<String, Object> p = orgComp(u);
        p.put("Activity", "ReadByOrganizationCompanyIdForContractorWages");
        return rows("Sp_SupplierCustomer_GetAllMethod", p);
    }

    /** CommonServies.GetSupplierCustomerListForFinancialEffects -> Id, GlAccountId. */
    public List<Map<String, Object>> supplierGl(UserAccount u) {
        Map<String, Object> p = orgComp(u);
        p.put("Activity", "GetGlAccountIdandCompanyNameBySupplierCustomerId");
        return rows("Sp_SupplierCustomer_GetAllMethod", p);
    }

    /** ConversionHeader.GetDataForDropDownFromConversionHeader(Activity 'ConversionNo') - Id, ReferenceName, Activity. */
    public List<Map<String, Object>> conversionNoDropDown(UserAccount u) {
        Map<String, Object> p = orgComp(u);
        p.put("Activity", "ConversionNo");
        return rows("[pcc].[USP_GetDataForDropDownFromConversionHeader]", p);
    }

    /** ConversionHeader.GenerateCode -> [pcc].[USP_ConversionHeader_GetAllMethod] 'GenerateCode' (DocNo). */
    public int generateCode(UserAccount u, int branchId, int docType, int financialYearId) {
        Map<String, Object> p = orgComp(u);
        p.put("BranchesId", branchId);
        p.put("FinancialYearId", financialYearId);
        p.put("DocumentTypeId", docType);
        p.put("Activity", "GenerateCode");
        List<Map<String, Object>> r = rows("[pcc].[USP_ConversionHeader_GetAllMethod]", p);
        return r.isEmpty() ? 0 : toInt(ci(r.get(0), "DocNo"));
    }

    // ================================================================================== stock / rates

    /** GetAvgRatesAndStockInHand.GetStockInHandAndAvgRateFromEvaluationConcrete(org, company, item, docDate, warehouse, 0, 0) -> ItemId, QtyInHand, StockInHand, AvgRate. */
    public List<Map<String, Object>> stockInHand(UserAccount u, int itemId, Timestamp docDate, int warehouseId) {
        Map<String, Object> p = orgComp(u);
        p.put("ItemId", itemId);
        p.put("DocDate", docDate);
        if (warehouseId != 0) p.put("WarehouseId", warehouseId);
        p.put("Activity", "GetStockInHandAndAvgRateFromEvaluation");
        return rows("Sp_GetAvgRatesAndStockInHand_GetAllMethod", p);
    }

    /**
     * [pcc].[USP_GetStockByFifoMethod] as GetStockByFifoMethodConcrete / FIFOImplementionForConcrete call it: @VarientId, @WarehouseId, @DocumentTypeId, @Id only when
     * non-zero, @FIFOXML only when earlier rows of the same save reserved layers.
     */
    public List<Map<String, Object>> fifoLayers(UserAccount u, int itemId, Timestamp docDate, int varientId, int warehouseId, int documentTypeId, int id, String fifoXml) {
        Map<String, Object> p = orgComp(u);
        p.put("ItemId", itemId);
        p.put("DocDate", docDate);
        if (varientId != 0) p.put("VarientId", varientId);
        if (warehouseId != 0) p.put("WarehouseId", warehouseId);
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        if (id != 0) p.put("Id", id);
        if (fifoXml != null) p.put("FIFOXML", fifoXml);
        return rows("[pcc].[USP_GetStockByFifoMethod]", p);
    }

    /** ContractorWagesRateSchedule.GetServiceActivityByItemId(1, item, effectedDate, activityId, contractorId) - ContractorWagesRateScheduleId, WagesActivity, Rate, Qty. */
    public List<Map<String, Object>> wagesActivities(UserAccount u, int itemId, Timestamp effectedDate, int activityId, int contractorId) {
        Map<String, Object> p = params();
        if (activityId != 0) p.put("ContractorWagesActivityId", activityId);
        if (contractorId != 0) p.put("ContractorId", contractorId);
        p.put("ItemId", itemId);
        p.put("RefDocumentTypeId", 1);
        p.put("EffectedDATE", effectedDate);
        p.put("Activity", "GetServiceActivityByItemId");
        return rows("[pcc].[USP_ContractorWagesRateSchedule_GetAllMethod]", p);
    }

    /** WareHouseAllocationToPlants.ValidateWareHouseAgainstPlantFromWareHouseAllocationToPlants - true when the procedure returns a row. */
    public boolean warehouseAllocatedToPlant(UserAccount u, int warehouseId, int plantId) {
        Map<String, Object> p = orgComp(u);
        p.put("WarehouseId", warehouseId);
        p.put("PlantId", plantId);
        return !rows("[dbo].[USP_ValidateWareHouseAgainstPlantFromWareHouseAllocationToPlants]", p).isEmpty();
    }

    // ================================================================================== history / read / delete

    /** ConversionHeader.FormHistory -> [pcc].[USP_ConversionHeader_GetAllMethod] 'FormHistory'. */
    public List<Map<String, Object>> formHistory(UserAccount u, int branchId, int docType, int financialYearId, Timestamp fromDate, Timestamp toDate,
                                                 int fromDocNo, int toDocNo, int id, boolean canViewAll, int entryUserId) {
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
        p.put("Activity", "FormHistory");
        return rows("[pcc].[USP_ConversionHeader_GetAllMethod]", p);
    }

    /** One 'ReadById' / 'ReadByHeaderId_*' read of [pcc].[USP_ConversionHeader_GetAllMethod] (DAL GetData). */
    public List<Map<String, Object>> read(String activity, int id) {
        Map<String, Object> p = params("Id", id, "Activity", activity);
        return rows("[pcc].[USP_ConversionHeader_GetAllMethod]", p);
    }

    /** ConversionHeader.DeleteByID(EntryUserId, Id) -> 'DeleteById'. */
    public void deleteById(int entryUserId, int id) {
        rows("[pcc].[USP_ConversionHeader_GetAllMethod]", params("EntryUserId", entryUserId, "Id", id, "Activity", "DeleteById"));
    }

    // ================================================================================== writes (DAL SetData, inside the caller's transaction)

    public int setHeader(boolean insert, Map<String, Object> p) {
        return DesktopProc.setProc(jdbc, insert ? "[pcc].[USP_ConversionHeader_Insert]" : "[pcc].[USP_ConversionHeader_Update]", p);
    }

    public int setOutput(Map<String, Object> p) { return DesktopProc.setProc(jdbc, "[pcc].[USP_ConversionOutPutDetail_Insert]", p); }

    public int setInput(Map<String, Object> p) { return DesktopProc.setProc(jdbc, "[pcc].[USP_ConversionInputDetail_Insert]", p); }

    public int setOverHead(Map<String, Object> p) { return DesktopProc.setProc(jdbc, "[pcc].[USP_ConversionOverHeadsDetail_Insert]", p); }

    public int setWages(Map<String, Object> p) { return DesktopProc.setProc(jdbc, "[pcc].[USP_ConversionWagesDetail_Insert]", p); }

    public JdbcTemplate jdbc() { return jdbc; }

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

    public static List<Map<String, Object>> copy(List<Map<String, Object>> in) { return new ArrayList<>(in); }

    public static Map<String, Object> newMap() { return new LinkedHashMap<>(); }
}
