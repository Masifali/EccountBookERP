package com.mst.repositories;

import com.mst.models.UserAccount;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

/**
 * Stock Conversion (invfrmStockConversionProduction.cs, DocTypeId 66) - the sources of the
 * thirteen dropdowns the page did not have. Entry Type, the fourteenth, stays in
 * StockConversionService.entryTypes().
 *
 * Every call below was traced three ways before it was written:
 *   1. the desktop fill method and what it puts in ReportsParameters / the model;
 *   2. the BLL method's IL (legacy-app-analysis/extracted_disasm/Architecture.bll), which decides
 *      which of those values actually become parameters, and under which guard;
 *   3. the procedure's declaration and the branch that answers, in procdure.sql.
 *
 * Where the desktop fills a model field the BLL never sends, that field is NOT sent here either.
 * The two cases that look wrong and are not:
 *   - InventoryParentCategories: the form passes Org/Company, the BLL sends only @Activity (and
 *     @Ids when set). The list is global.
 *   - InvPackingType.Getall: @Activity='ReadAll' only - no tenancy at all.
 *
 * Nothing here writes. A failing list is logged with its procedure and comes back empty, so the
 * page still opens; the log says which list failed rather than leaving an unexplained blank.
 */
@Repository
public class StockConversionLookupsRepository {

    private static final Logger LOG = LoggerFactory.getLogger(StockConversionLookupsRepository.class);

    private final JdbcTemplate jdbc;
    public StockConversionLookupsRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private List<Map<String, Object>> q(String label, String sql, Object... args) {
        try {
            return jdbc.queryForList(sql, args);
        } catch (Exception e) {
            LOG.warn("Stock Conversion lookup '{}' failed: {}", label, sql, e);
            return new ArrayList<>();
        }
    }

    /**
     * cmbParentCategory - ParentCategoryFill:1142 -> Item.InventoryParentCategories.
     * BLL: Sp_InventoryItemsOther_GetAllMethod, @Activity='InventoryParentCategories' only
     * (@Ids is guarded by IsNullOrEmpty and the form leaves it unset).
     * Columns: Id, InvParentCateDescription.
     */
    public List<Map<String, Object>> parentCategories() {
        return q("parentCategories",
                "EXEC dbo.Sp_InventoryItemsOther_GetAllMethod @Activity=?",
                "InventoryParentCategories");
    }

    /**
     * CmbProductionDepartment - BindProductionDepartment:831 ->
     * InvWareHouse.GetActiveWareHouseByWareHouseType with ActivityId=2 and
     * BranchesIds = UserAccount.BranchesId.ToString().
     * BLL: @WarehouseType only when ActivityId != 0; @WarehouseTypeIds from Activity (unset here);
     * @BranchesIds when not IsNullOrEmpty - the string of an int is never empty, so it is always
     * sent, "0" included.
     * Columns: Id, WareHouseName.
     */
    public List<Map<String, Object>> productionDepartments(UserAccount u, int branchId) {
        return q("productionDepartments",
                "EXEC dbo.Sp_InvWareHouse_GetAllMethod @OrganizationId=?, @CompanyId=?, "
              + "@WarehouseType=?, @BranchesIds=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), 2, String.valueOf(branchId),
                "GetActiveWareHouseByWareHouseType");
    }

    /**
     * CmbConversionType - BindProductionType:871 -> CommonServices.StaticColumnsService
     * ("ConversionType") -> GeneralReprots.StaticColumnNames -> SpStaticColumnNames @Activity.
     * Five literal rows (Id, type). Row 5 is removed by the SERVICE, not here, because that
     * depends on ERP feature 24.
     */
    public List<Map<String, Object>> conversionTypes() {
        return q("conversionTypes", "EXEC dbo.SpStaticColumnNames @Activity=?", "ConversionType");
    }

    /**
     * cmbGodown - Warehouse():894 -> WarehousesAllocationToBranch
     * .GetWarehousesAllocatedToBranchByBranchId. BLL sends @BranchId only when BranchesId != 0.
     * Columns: Id, WareHouseName (plus branch/plant columns the combo does not show). The
     * procedure LEFT JOINs plants, so a warehouse on two plants appears twice - on the desktop too.
     */
    public List<Map<String, Object>> warehousesForBranch(UserAccount u, int branchId) {
        if (branchId != 0) {
            return q("warehousesForBranch",
                    "EXEC dbo.USP_GetWarehousesAllocatedToBranch @OrganizationId=?, @CompanyId=?, @BranchId=?",
                    u.getOrganizationId(), u.getCompanyId(), branchId);
        }
        return q("warehousesForBranch",
                "EXEC dbo.USP_GetWarehousesAllocatedToBranch @OrganizationId=?, @CompanyId=?",
                u.getOrganizationId(), u.getCompanyId());
    }

    /**
     * cmbItem - ItemFill():917 caches Item.ReadAllItems in dtitem; BindItemCombo filters that
     * cache IN MEMORY on InventoryParentCategoriesId. BLL: Sp_Item_GetAllMethod
     * @OrganizationId, @CompanyId, @Activity='ReadAllItems'.
     * Columns used: Id, ItemName, InventoryParentCategoriesId.
     */
    public List<Map<String, Object>> allItems(UserAccount u) {
        return q("allItems",
                "EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "ReadAllItems");
    }

    /**
     * cmbLot and CmbJobLotForGrid - combojoblotfill():966, one call bound to both ->
     * JobLotsAllocationToBranch.GetJobLotsAllocatedToBranchByBranchId. @BranchId only when != 0.
     * Columns: Id, JobLotDescription.
     */
    public List<Map<String, Object>> jobLotsForBranch(UserAccount u, int branchId) {
        if (branchId != 0) {
            return q("jobLotsForBranch",
                    "EXEC dbo.USP_GetJobLotsAllocatedToBranch @OrganizationId=?, @CompanyId=?, @BranchId=?",
                    u.getOrganizationId(), u.getCompanyId(), branchId);
        }
        return q("jobLotsForBranch",
                "EXEC dbo.USP_GetJobLotsAllocatedToBranch @OrganizationId=?, @CompanyId=?",
                u.getOrganizationId(), u.getCompanyId());
    }

    /**
     * CmbCropyr - CropYear():990 -> CommonServices.CropYearGetAllService -> InvCropYear.Getall:
     * Sp_InvCropYear_GetAllMethod @OrganizationId, @CompanyId, @Activity='ReadAll'.
     * Columns: Id, CropYear.
     */
    public List<Map<String, Object>> cropYears(UserAccount u) {
        return q("cropYears",
                "EXEC dbo.Sp_InvCropYear_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "ReadAll");
    }

    /**
     * cmbBagType - bagType():1008 -> InvPackingType.Getall(): Sp_InvPackingType_GetAllMethod
     * @Activity='ReadAll' and nothing else. Columns: Id, PackTypeDesc.
     */
    public List<Map<String, Object>> packingTypes() {
        return q("packingTypes", "EXEC dbo.Sp_InvPackingType_GetAllMethod @Activity=?", "ReadAll");
    }

    /**
     * cmbMoistureSlab - MoistureSlabFill():1387 -> MoistureSlab.MoistureSlabGetAll:
     * Sp_MoistureSlabGetAll @OrganizationId, @CompanyId (no activity).
     * Columns: Id, MoistureSlabCode, MoistureSlabDescription, MinValue, MaxValue.
     */
    public List<Map<String, Object>> moistureSlabs(UserAccount u) {
        return q("moistureSlabs",
                "EXEC dbo.Sp_MoistureSlabGetAll @OrganizationId=?, @CompanyId=?",
                u.getOrganizationId(), u.getCompanyId());
    }

    /**
     * CmbDifferenceAccount - AccountFills():1025 ->
     * CommonServices.CoaAllocationAccountTitleByAccountTypeIds("4,12,10,9") ->
     * COAAllocation.GetAccountTitleByAccountTypeIds. BLL always sends @OrganizationId,
     * @CompanyId, @AppId; @AccountTypeIds when non-empty; @UserId when != 0; CostCenterId,
     * NotReferred, RecId and the two class lists are 0 / "" here, so they are not sent.
     * Note the FOUR account types - not the "4" screen 281 uses.
     * Columns: Id, AccountTitle.
     */
    public List<Map<String, Object>> differenceAccounts(UserAccount u, int appId) {
        return q("differenceAccounts",
                "EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @AppId=?, "
              + "@AccountTypeIds=?, @UserId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), appId, "4,12,10,9", u.getId(),
                "GetAccountTitleByAccountTypeIds");
    }

    /**
     * cmbUOM and cmbRateUom - bindRateUomAndItemPackUom:1041 ->
     * CommonServices.GetUomScheduleByItemId(item) -> UOMSchedule.SearchByObject:
     * Sp_UOMSchedule_GetAllMethod @OrganizationId, @CompanyId, @ItemId, @Activity='ReadByItemID'.
     * CommonServices then copies five columns into a new table - Id, UOMCode, Equivalent,
     * QtyEquivalent, BaseRateUom - which the service reproduces.
     */
    public List<Map<String, Object>> uomsForItem(UserAccount u, int itemId) {
        return q("uomsForItem",
                "EXEC dbo.Sp_UOMSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), itemId, "ReadByItemID");
    }

    /**
     * CommonServices.GetERPFeatureById(24) -> StockReleaseFromFumigation (Load:576). The global
     * ErpFeaturesList is USP_GetERPFeaturesByCompanyId; the flag is "is feature 24 among them".
     */
    public boolean erpFeature(UserAccount u, int featureId) {
        for (Map<String, Object> r : q("erpFeatures",
                "EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?, @CompanyId=?",
                u.getOrganizationId(), u.getCompanyId())) {
            Object id = ci(r, "Id");
            if (id instanceof Number && ((Number) id).intValue() == featureId) return true;
        }
        return false;
    }

    /**
     * The raw ConfigKey of one configuration row, or null when the company has no such row.
     * Same activity as StockConversionRepository.config, but it keeps "absent" distinct from
     * "false", which CmbEntryTypeFill needs (see StockConversionService.entryTypes).
     */
    public String configKey(UserAccount u, String configDescription) {
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, "
              + "@ConfigDescription=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), configDescription,
                "GetConfigurationByOrgCompandConfigDescription");
        if (rows.isEmpty()) return null;
        Object v = ci(rows.get(0), "ConfigKey");
        return v == null ? "" : String.valueOf(v);
    }

    public static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }
}
