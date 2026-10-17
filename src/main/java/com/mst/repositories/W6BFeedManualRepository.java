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
 * Screen 929 "Production Manual Feed" - Architecture.WinApp.FeedMill.Production.frmProductionManualFeed (DocumentTypeId 1122, rights key base.Name
 * "frmProductionManualFeed", schema [fed]): every procedure call of the form, one method per BLL / CommonServices call.
 * Parameter lists come from procdure.utf8.sql.
 *
 * <pre>
 *   BLL 0431 Architecture.BLL.FeedMill.Production.ProductionHeader  Save / ReadById / FormHistory / GenerateCode / DeleteByID / GetDataForDropDownFromProductionHeader
 *   DAL 0488 Architecture.DAL.FeedMill.Production.ProductionHeader  SetData (header, output, input, over head, consumption, stock, validation, voucher) and GetData (4 result sets)
 *   BLL 0428 Architecture.BLL.FeedMill.Production.BomHeader         GetAllBomItems / GetBomNoByItemId / GetAllBomNoAndItems / GetBomdataByIds / getBomdataBySingleBomId
 *   BLL 0327 Production.ProductionType.GetAll, BLL 0203 genShift.Getall, BLL 0336 InvProductionPlant.GetAll
 *   BLL 0056 GetAvgRatesAndStockInHand                              getAvgRateByItemId / GetStockInHandAndAvgRateFromEvaluation
 * </pre>
 * A null value is left out of the EXEC, as in {@link DesktopProc}.
 */
@Repository
public class W6BFeedManualRepository {

    private final JdbcTemplate jdbc;

    public W6BFeedManualRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private List<Map<String, Object>> rows(String proc, Map<String, Object> p) { return DesktopProc.rows(jdbc, proc, p); }

    private static Map<String, Object> orgComp(UserAccount u) { return params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()); }

    // ================================================================================== lookups (frmBillOfMaterial_Manufacturing_Load:463)

    /** ProductionTypeFill:653 - ProductionType.GetAll (no @Id): Sp_ProductionType_GetAllMethod (Id, ProductionTypeDescription). */
    public List<Map<String, Object>> productionTypes() {
        return rows("Sp_ProductionType_GetAllMethod", params());
    }

    /** ShiftComboFill:685 - genShift.Getall: Sp_genShift_GetAllMethod 'ReadAll' (ShiftId, ShiftName). */
    public List<Map<String, Object>> shifts(UserAccount u) {
        Map<String, Object> p = orgComp(u);
        p.put("Activity", "ReadAll");
        return rows("Sp_genShift_GetAllMethod", p);
    }

    /** PlantFeader:730 - InvProductionPlant.GetAll: Sp_InvProductionPlant_GetAllMethod 'GetALL' (Id, Description). */
    public List<Map<String, Object>> plants(UserAccount u) {
        Map<String, Object> p = orgComp(u);
        p.put("Activity", "GetALL");
        return rows("Sp_InvProductionPlant_GetAllMethod", p);
    }

    /** dtFillsForGrids:771 - CommonServices.getActiveWareHouse: Sp_InvWareHouse_GetAllMethod 'GetActiveWareHouse' (Id, WareHouseName). */
    public List<Map<String, Object>> activeWarehouses(UserAccount u) {
        Map<String, Object> p = orgComp(u);
        p.put("Activity", "GetActiveWareHouse");
        return rows("dbo.Sp_InvWareHouse_GetAllMethod", p);
    }

    /** dtFillsForGrids:771 - CommonServices.GetItemByItemTypeId("14,17"): Sp_Item_GetAllMethod 'GetItemByItemTypeId' (Id, ItemName, ParentCategory ...). */
    public List<Map<String, Object>> consumptionItems(UserAccount u) {
        Map<String, Object> p = orgComp(u);
        p.put("LookupTypeIds", "14,17");
        p.put("Activity", "GetItemByItemTypeId");
        return rows("Sp_Item_GetAllMethod", p);
    }

    /** ItemFillFromBom:807 - BomHeader.GetAllBomItems (DocumentTypeId 1120; @TypeOfTypeId not sent): ItemId, ItemName, ItemCode. */
    public List<Map<String, Object>> bomItems(UserAccount u, int branchId, int financialYearId) {
        Map<String, Object> p = orgComp(u);
        p.put("BranchesId", branchId);
        p.put("DocumentTypeId", 1120);
        p.put("FinancialYearId", financialYearId);
        p.put("Activity", "GetAllBomItems");
        return rows("[fed].[USP_BomHeader_GetAllMethod]", p);
    }

    /** BOMNobyItemFill:923 - BomHeader.GetBomNoByItemId: Id, BomName, ItemUomId, PackUom, PackUomEquivalent, BomBatchQty, BomDate. */
    public List<Map<String, Object>> bomsByItem(UserAccount u, int branchId, int financialYearId, int itemId) {
        Map<String, Object> p = orgComp(u);
        p.put("BranchesId", branchId);
        p.put("DocumentTypeId", 1120);
        p.put("FinancialYearId", financialYearId);
        p.put("ItemId", itemId);
        p.put("Activity", "GetBomNoByItemId");
        return rows("[fed].[USP_BomHeader_GetAllMethod]", p);
    }

    /** BOMNoAndItemFill:995 - BomHeader.GetAllBomNoAndItems: Id, BomName, ItemId, ItemName, ItemCode, ItemUomId, PackUom, PackUomEquivalent, BomBatchQty. */
    public List<Map<String, Object>> bomNoAndItems(UserAccount u, int branchId, int financialYearId) {
        Map<String, Object> p = orgComp(u);
        p.put("BranchesId", branchId);
        p.put("DocumentTypeId", 1120);
        p.put("FinancialYearId", financialYearId);
        p.put("Activity", "GetAllBomNoAndItems");
        return rows("[fed].[USP_BomHeader_GetAllMethod]", p);
    }

    /** FillInputDetailForMultiBom:1185 - BomHeader.getBomdataBySingleBomId (RestrictSingleBomInOutPut). */
    public List<Map<String, Object>> bomDataBySingle(UserAccount u, int bomId) {
        Map<String, Object> p = orgComp(u);
        p.put("BomId", bomId);
        return rows("[fed].[usp_getBomdataBySingleBomId]", p);
    }

    /** FillInputDetailForMultiBom:1185 - BomHeader.GetBomdataByIds(org, comp, ",1,2"). */
    public List<Map<String, Object>> bomDataByIds(UserAccount u, String bomIds) {
        Map<String, Object> p = orgComp(u);
        p.put("BomIds", bomIds);
        return rows("[fed].[usp_getBomdataByIds]", p);
    }

    // ================================================================================== rate / stock (BLL 0056)

    /** CommonServices.getAvgRateByItemId(itemId, docDate, 1122, recId) -> fed.usp_getAvgRateByItemId (@DocumentTypeId / @RecId when non-zero; @JobLotId never). */
    public double avgRateByItemId(UserAccount u, int itemId, Timestamp docDate, int recId) {
        Map<String, Object> p = orgComp(u);
        p.put("ItemId", itemId);
        p.put("DocDate", docDate);
        p.put("DocumentTypeId", 1122);
        if (recId != 0) p.put("RecId", recId);
        List<Map<String, Object>> r = rows("[fed].[usp_getAvgRateByItemId]", p);
        if (r.isEmpty()) return 0d;
        Object v = r.get(0).get("AvgRate");
        if (v == null) for (Map.Entry<String, Object> e : r.get(0).entrySet()) if ("AvgRate".equalsIgnoreCase(e.getKey())) v = e.getValue();
        return v instanceof Number ? ((Number) v).doubleValue() : 0d;
    }

    /**
     * UpdateRowStockAndRate:5799 / grdInputMaterial_KeyDown:4600 - GetAvgRatesAndStockInHand.GetStockInHandAndAvgRateFromEvaluation.
     * The form builds ReportsParameters with OrganizationId = UserAccount.CompanyId and CompanyId = UserAccount.OrganizationId (swapped, kept here);
     * WarehouseId is on the object but the BLL never sends it (@OrganizationId @CompanyId @ItemId @DocDate 'GetAvgRateQtyAndStockInHand').
     */
    public double stockInHand(UserAccount u, int itemId, Timestamp docDate) {
        Map<String, Object> p = params("OrganizationId", u.getCompanyId(), "CompanyId", u.getOrganizationId());
        p.put("ItemId", itemId);
        p.put("DocDate", docDate);
        p.put("Activity", "GetAvgRateQtyAndStockInHand");
        List<Map<String, Object>> r = rows("Sp_GetAvgRatesAndStockInHand_GetAllMethod", p);
        if (r.isEmpty()) return 0d;
        for (Map.Entry<String, Object> e : r.get(0).entrySet()) {
            if ("StockInHand".equalsIgnoreCase(e.getKey())) return e.getValue() instanceof Number ? ((Number) e.getValue()).doubleValue() : 0d;
        }
        return 0d;
    }

    // ================================================================================== BLL 0431 reads

    /** DocumentNoFill:628 - BLL GenerateCode (@OrganizationId @CompanyId @FinancialYearId @DocumentTypeId 1122 'GenerateCode'; no @BranchesId). */
    public List<Map<String, Object>> generateCode(UserAccount u, int financialYearId) {
        Map<String, Object> p = orgComp(u);
        p.put("FinancialYearId", financialYearId);
        p.put("DocumentTypeId", 1122);
        p.put("Activity", "GenerateCode");
        return rows("[fed].[USP_ProductionHeader_GetAllMethod]", p);
    }

    /** DAL 0488 GetData: 'ReadById' header (ActionId <> 3). */
    public List<Map<String, Object>> readHeader(int id) {
        return rows("[fed].[USP_ProductionHeader_GetAllMethod]", params("Id", id, "Activity", "ReadById"));
    }

    /** DAL 0488 GetData: 'ReadByHeaderId_ProductionInputDetail'. */
    public List<Map<String, Object>> readInputs(int id) {
        return rows("[fed].[USP_ProductionHeader_GetAllMethod]", params("Id", id, "Activity", "ReadByHeaderId_ProductionInputDetail"));
    }

    /** DAL 0488 GetData: 'ReadByHeaderId_ProductionOutPutDetail'. */
    public List<Map<String, Object>> readOutputs(int id) {
        return rows("[fed].[USP_ProductionHeader_GetAllMethod]", params("Id", id, "Activity", "ReadByHeaderId_ProductionOutPutDetail"));
    }

    /** DAL 0488 GetData: 'ReadByHeaderId_ProductionOverHeadsDetail'. */
    public List<Map<String, Object>> readOverheads(int id) {
        return rows("[fed].[USP_ProductionHeader_GetAllMethod]", params("Id", id, "Activity", "ReadByHeaderId_ProductionOverHeadsDetail"));
    }

    /** DAL 0488 GetData: 'ReadByHeaderId_ProductionConsumptionDetail'. */
    public List<Map<String, Object>> readConsumptions(int id) {
        return rows("[fed].[USP_ProductionHeader_GetAllMethod]", params("Id", id, "Activity", "ReadByHeaderId_ProductionConsumptionDetail"));
    }

    /**
     * HistoryGridFill:3781 - BLL 0431 FormHistory. FromDate / ToDate only when the picker is checked; FromDocNo / ToDocNo / @Id (production-no combo) / @ProductTypeId when not 0;
     * @CanViewAllRecord always; @EntryUserId only when it is false and then obj.EntryUser, which the form never sets (0) - kept; @ActionId (radios, All = 0) never sent.
     */
    public List<Map<String, Object>> formHistory(UserAccount u, int branchId, int financialYearId, Object fromDate, Object toDate, int fromDocNo, int toDocNo,
                                                 int productionHeaderId, boolean canViewAll, int productTypeId) {
        Map<String, Object> p = orgComp(u);
        p.put("BranchesId", branchId);
        p.put("DocumentTypeId", 1122);
        p.put("FinancialYearId", financialYearId);
        if (fromDate != null) p.put("FromDate", fromDate);
        if (toDate != null) p.put("ToDate", toDate);
        if (fromDocNo != 0) p.put("FromDocNo", fromDocNo);
        if (toDocNo != 0) p.put("ToDocNo", toDocNo);
        if (productionHeaderId != 0) p.put("Id", productionHeaderId);
        p.put("CanViewAllRecord", canViewAll ? 1 : 0);
        if (!canViewAll) p.put("EntryUserId", 0);
        if (productTypeId != 0) p.put("ProductTypeId", productTypeId);
        p.put("Activity", "FormHistory");
        return rows("[fed].[USP_ProductionHeader_GetAllMethod]", p);
    }

    /** AllCombobind:3570 - GetDataForDropDownFromProductionHeader (ApprovedFilter "All": no @IsApproved; DocumentTypeId 0: not sent; no @Activity) -> Id, ReferenceName, Activity. */
    public List<Map<String, Object>> historyDropDowns(UserAccount u) {
        return rows("[fed].[USP_GetDataForDropDownFromProductionHeader]", orgComp(u));
    }

    /** btnDelete_Click:3452 - BLL 0431 DeleteByID(EntryUserId, Id) ('DeleteById'). */
    public void deleteById(int entryUserId, int id) {
        Map<String, Object> p = params("EntryUserId", entryUserId, "Id", id);
        p.put("Activity", "DeleteById");
        DesktopProc.scalar(jdbc, "[fed].[USP_ProductionHeader_GetAllMethod]", p);
    }

    // ================================================================================== DAL 0488 SetData (inside the caller's transaction)

    public int setHeader(boolean update, Map<String, Object> p) {
        return DesktopProc.setProc(jdbc, update ? "[fed].[USP_ProductionHeader_Update]" : "[fed].[USP_ProductionHeader_Insert]", p);
    }

    public int setOutput(Map<String, Object> p) { return DesktopProc.setProc(jdbc, "[fed].[USP_ProductionOutPutDetail_Insert]", p); }

    public int setInput(Map<String, Object> p) { return DesktopProc.setProc(jdbc, "[fed].[USP_ProductionInputDetail_Insert]", p); }

    public int setOverhead(Map<String, Object> p) { return DesktopProc.setProc(jdbc, "[fed].[USP_ProductionOverHeadsDetail_Insert]", p); }

    public int setConsumption(Map<String, Object> p) { return DesktopProc.setProc(jdbc, "[fed].[USP_ProductionConsumptionDetail_Insert]", p); }
}
