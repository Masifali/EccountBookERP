package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 795 "Production (Casting)" - Architecture.WinApp.Mfg.frmProductionEngr (DocumentTypeId 1670, rights key base.Name "frmProductionEngr", schema [Mfg]):
 * every procedure call of the form, one method per BLL / CommonServices call. Parameter lists come from procdure.utf8.sql.
 *
 * <pre>
 *   BLL 0366 Architecture.BLL.Mfg.ProductionHeader   Save / ReadById / FormHistory / GenerateCode / DeleteByID / WorkOrderSemiFinishForProductionOutput
 *   DAL 0319 Architecture.DAL.Mfg.ProductionHeader   SetData (header, output, input, overhead, inventory, voucher) and GetData (4 result sets)
 *   BLL 0369 Architecture.BLL.Mfg.BomHeader          ReadById (input detail list) and BOMNameForProduction
 *   BLL 0583 Item.AllItemsBindForEngrWithModalOnly   Sp_Item_GetAllMethod (loaded by the desktop into dtitemOutput, which nothing reads - not called here)
 *   BLL 0379 GlobalServicesMethods                   GetGlobalAllItemsWithCustomGroup / GetGlobalAllAccountsWithCustomGroup
 *   BLL 0019 JobLotsAllocationToBranch               [dbo].[USP_GetJobLotsAllocatedToBranch]
 *   CommonServices.StaticColumnsService              dbo.SpStaticColumnNames 'ProductionHeatCodeEngr'
 * </pre>
 * A null value is left out of the EXEC, as in {@link DesktopProc}. The voucher / inventory writes of DAL SetData that dbo procedures take are
 * issued by the service through {@link ProductionInputRepository} (same declared-parameter sets the other voucher ports use).
 */
@Repository
public class Engr795ProductionRepository {

    private final JdbcTemplate jdbc;

    public Engr795ProductionRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private List<Map<String, Object>> rows(String proc, Map<String, Object> p) { return DesktopProc.rows(jdbc, proc, p); }

    private static Map<String, Object> orgComp(UserAccount u) { return params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()); }

    // ================================================================================== lookups (InitializeComponentMethod:454)

    /** clsGlobalVariables.GetAllLookUps_Engr = BLL 0370 LookUps.FormHistoryWithList -> [Mfg].[USP_LookUps_GetAllMethod] 'FormHistory' (Id, LookUpTypeId, LookupName ...). */
    public List<Map<String, Object>> allLookups() {
        return rows("[Mfg].[USP_LookUps_GetAllMethod]", params("Activity", "FormHistory"));
    }

    /** GetHeatCode:698 - CommonServices.StaticColumnsService("ProductionHeatCodeEngr") -> dbo.SpStaticColumnNames (Id, type). */
    public List<Map<String, Object>> heatCodes() {
        return rows("SpStaticColumnNames", params("Activity", "ProductionHeatCodeEngr"));
    }

    /** BindJobLot:567 - JobLotsAllocationToBranch.GetJobLotsAllocatedToBranchByBranchId (BranchId sent only when it is not 0). */
    public List<Map<String, Object>> jobLots(UserAccount u, int branchId) {
        Map<String, Object> p = orgComp(u);
        if (branchId != 0) p.put("BranchId", branchId);
        return rows("[dbo].[USP_GetJobLotsAllocatedToBranch]", p);
    }

    /** GetWarehouses:891 - WarehousesAllocationToBranch.GetWarehousesAllocatedToBranchByBranchId (BranchesId = BranchFeature ? user's branch : 0). */
    public List<Map<String, Object>> warehouses(UserAccount u, int branchesId) {
        Map<String, Object> p = orgComp(u);
        if (branchesId != 0) p.put("BranchesId", branchesId);
        return rows("[dbo].[USP_GetWarehousesAllocatedToBranch]", p);
    }

    /** CommonServices.GetERPFeatureById - clsGlobalVariables.ErpFeaturesList (USP_GetERPFeaturesByCompanyId); the feature is in the list. */
    public boolean erpFeature(UserAccount u, int featureId) {
        Set<Integer> out = new HashSet<>();
        for (Map<String, Object> r : rows("USP_GetERPFeaturesByCompanyId", orgComp(u))) {
            for (Map.Entry<String, Object> e : r.entrySet()) {
                if ("Id".equalsIgnoreCase(e.getKey()) && e.getValue() instanceof Number) out.add(((Number) e.getValue()).intValue());
            }
        }
        return out.contains(featureId);
    }

    /** GlobalServicesDbCall("ItemsWithCustomGroupId") - [dbo].[USP_GetAllItemFromCustomGroup] (ItemId, ItemName, ItemCode, CustomGroupId, GroupCode, GroupName). */
    public List<Map<String, Object>> itemsWithCustomGroup(UserAccount u, int branchId) {
        Map<String, Object> p = orgComp(u);
        p.put("BranchesId", branchId);
        return rows("[dbo].[USP_GetAllItemFromCustomGroup]", p);
    }

    /** GlobalServicesDbCall("AccountsWithCustomGroupId") - [dbo].[USP_GETAllAccountsFromCustomGroups] (PageSize / PageNumber 0 and Keyword "" are not sent). */
    public List<Map<String, Object>> accountsWithCustomGroup(UserAccount u) {
        return rows("[dbo].[USP_GETAllAccountsFromCustomGroups]", orgComp(u));
    }

    /** GetAllBomHeader:742 - BomHeader.BOMNameForProduction(org, comp, RecId, 0): RecId sent when not 0, CastingTypeId (0) never (Id, BomName, EffectedDate, CastingTypeId). */
    public List<Map<String, Object>> boms(UserAccount u, int recId) {
        Map<String, Object> p = orgComp(u);
        if (recId != 0) p.put("RecId", recId);
        return rows("[Mfg].[USP_BomHeader_BOMNameForProduction]", p);
    }

    /** FillInputDetailForMultiBom:1062 - BomHeader.ReadById(BOMId): its BomInputDetailList = 'ReadByHeaderId_BomInputDetail'. */
    public List<Map<String, Object>> bomInputs(int bomId) {
        return rows("[Mfg].[USP_BomHeader_GetAllMethod]", params("Id", bomId, "Activity", "ReadByHeaderId_BomInputDetail"));
    }

    /** BtnLoadOutPut_Click:3966 - ProductionHeader.WorkOrderSemiFinishForProductionOutput (RecId sent when not 0; CastingTypeId always). */
    public List<Map<String, Object>> workOrderSemiFinish(UserAccount u, int castingTypeId, int recId) {
        Map<String, Object> p = orgComp(u);
        if (recId != 0) p.put("RecId", recId);
        p.put("CastingTypeId", castingTypeId);
        return rows("[Mfg].[USP_WorkOrderSemiFinishForProductionOutput]", p);
    }

    // ================================================================================== BLL 0366 reads

    /** GenerateCode:667 - @BranchesId, @FinancialYearId, @DocumentTypeId 1670. */
    public List<Map<String, Object>> generateCode(UserAccount u, int branchId, int documentTypeId, int financialYearId) {
        Map<String, Object> p = orgComp(u);
        p.put("BranchesId", branchId);
        p.put("FinancialYearId", financialYearId);
        p.put("DocumentTypeId", documentTypeId);
        p.put("Activity", "GenerateCode");
        return rows("[Mfg].[USP_ProductionHeader_GetAllMethod]", p);
    }

    /** DAL 0319 GetData: 'ReadById' header. */
    public List<Map<String, Object>> readHeader(int id) {
        return rows("[Mfg].[USP_ProductionHeader_GetAllMethod]", params("Id", id, "Activity", "ReadById"));
    }

    /** DAL 0319 GetData: 'ReadDetailByHeaderId' (output + scrap rows: OutPutTypeId 1 / 2). */
    public List<Map<String, Object>> readOutputs(int id) {
        return rows("[Mfg].[USP_ProductionHeader_GetAllMethod]", params("Id", id, "Activity", "ReadDetailByHeaderId"));
    }

    /** DAL 0319 GetData: 'ReadInputDetailByHeaderId'. */
    public List<Map<String, Object>> readInputs(int id) {
        return rows("[Mfg].[USP_ProductionHeader_GetAllMethod]", params("Id", id, "Activity", "ReadInputDetailByHeaderId"));
    }

    /** DAL 0319 GetData: 'ReadOverHeadsDetailByHeaderId'. */
    public List<Map<String, Object>> readOverheads(int id) {
        return rows("[Mfg].[USP_ProductionHeader_GetAllMethod]", params("Id", id, "Activity", "ReadOverHeadsDetailByHeaderId"));
    }

    /** HistoryGridFill:2615 - BLL 0366 FormHistory (CanViewAllRecord always sent; EntryUserId only when it is false). */
    public List<Map<String, Object>> formHistory(UserAccount u, int branchId, int documentTypeId, int financialYearId, Object fromDate, Object toDate,
                                                 int fromDocNo, int toDocNo, boolean canViewAll, int entryUserId) {
        Map<String, Object> p = orgComp(u);
        p.put("BranchesId", branchId);
        p.put("FinancialYearId", financialYearId);
        p.put("DocumentTypeId", documentTypeId);
        if (fromDate != null) p.put("FromDate", fromDate);
        if (toDate != null) p.put("ToDate", toDate);
        if (fromDocNo != 0) p.put("FromDocNo", fromDocNo);
        if (toDocNo != 0) p.put("ToDocNo", toDocNo);
        p.put("CanViewAllRecord", canViewAll ? 1 : 0);
        if (!canViewAll) p.put("EntryUserId", entryUserId);
        p.put("Activity", "FormHistory");
        return rows("[Mfg].[USP_ProductionHeader_GetAllMethod]", p);
    }

    /** BtnDelete_Click:2267 - BLL 0366 DeleteByID(org, company, 1670, user, id). */
    public void deleteById(UserAccount u, int documentTypeId, int entryUserId, int id) {
        Map<String, Object> p = orgComp(u);
        p.put("DocumentTypeId", documentTypeId);
        p.put("EntryUserId", entryUserId);
        p.put("Id", id);
        p.put("Activity", "DeleteById");
        DesktopProc.scalar(jdbc, "[Mfg].[USP_ProductionHeader_GetAllMethod]", p);
    }

    // ================================================================================== DAL 0319 SetData (inside the caller's transaction)

    public int setHeader(Map<String, Object> p) { return DesktopProc.setProc(jdbc, "[Mfg].[USP_ProductionHeader_InsertAndUpdate]", p); }

    public int setOutput(Map<String, Object> p) { return DesktopProc.setProc(jdbc, "[Mfg].[USP_ProductionOutPutDetail_Insert]", p); }

    public int setInput(Map<String, Object> p) { return DesktopProc.setProc(jdbc, "[Mfg].[USP_productionInputDetail_Insert]", p); }

    public int setOverhead(Map<String, Object> p) { return DesktopProc.setProc(jdbc, "[Mfg].[USP_ProductionOverHeadsDetail_Insert]", p); }

    /** DAL 0319 (voucher branch, unsettled): usp_VoucherDeleteByVoucherId @Id (ExecuteNonQuery). */
    public void voucherDeleteByVoucherId(int voucherId) {
        DesktopProc.scalar(jdbc, "usp_VoucherDeleteByVoucherId", params("Id", voucherId));
    }

    public JdbcTemplate jdbc() { return jdbc; }

    /** Helper for callers that need a plain copy of a row list. */
    public static List<Map<String, Object>> copy(List<Map<String, Object>> src) { return new ArrayList<>(src); }
}
