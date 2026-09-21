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
 * Every dropdown on frmProductionJobOrder, from the procedure the desktop actually calls.
 *
 * ---------------------------------------------------------------------------------------------
 * WHERE EACH LIST COMES FROM  (traced form -> CommonServices -> BLL -> procedure + @Activity)
 * ---------------------------------------------------------------------------------------------
 *   Crop Year        CommonServices.CropYearGetAllService -> InvCropYear.Getall
 *                    Sp_InvCropYear_GetAllMethod     @OrganizationId @CompanyId @Activity=ReadAll
 *   WareHouse        CommonServices.getActiveWareHouse -> InvWareHouse.GetActiveWareHouse
 *                    Sp_InvWareHouse_GetAllMethod    @OrganizationId @CompanyId @Activity=GetActiveWareHouse
 *   Job Lot          CommonServices.JobLotGetAllService -> jobLot.GetAll
 *                    SP_JobLot_ReadMethod            @OrganizationId @CompanyId @Activity=GetAll
 *   Plant/Feader     InvProductionPlant.GetAll
 *                    Sp_InvProductionPlant_GetAllMethod @OrganizationId @CompanyId @Activity=GetALL
 *   Packing Type     InvPackingType.Getall
 *                    Sp_InvPackingType_GetAllMethod  @Activity=ReadAll        (no tenancy: the
 *                                                     desktop passes none - see note below)
 *   WorkInProcess /  CommonServices.CoaAllocationGetForComboServiceBind -> COAAllocation.GetForComboBind
 *   FinishGoods /    Sp_COAAllocation_GetAllMethod   @OrganizationId @CompanyId @UserId
 *   ByProduction                                     @Activity=COAForCombobindig
 *   WIP Item         CommonServices.ItemGetForComboServiceBind -> Item.GetAllbyCombobind
 *                    Sp_Item_GetAllMethod            @OrganizationId @CompanyId @Activity=ReadAllForComboTwoColumns
 *   Item (unfiltered, both grids)  ItemDetailFill -> Item.ReadAllItems
 *                    Sp_Item_GetAllMethod            @OrganizationId @CompanyId @Activity=ReadAllItems
 *   Inner/Outer UOM, PackSize      UOMSchedule.SearchByObject
 *                    Sp_UOMSchedule_GetAllMethod     @OrganizationId @CompanyId @ItemId @Activity=ReadByItemID
 *
 * Entry Type (Input/Output) and Filter Type (SaleOrder / Export Contract / Item Type / Item
 * Category) are built in the form as literal DataTables - they are NOT database lists on the
 * desktop either, so they are literals in the service rather than an invented table.
 *
 * ---------------------------------------------------------------------------------------------
 * THE PACKING TYPE PROCEDURE TAKES NO TENANCY - AND THAT IS NOT MINE TO ADD
 * ---------------------------------------------------------------------------------------------
 * InvPackingType.Getall() passes @Activity alone. Adding @OrganizationId/@CompanyId "for safety"
 * would change which rows the screen shows relative to the desktop, which is exactly the kind of
 * silent divergence this port exists to avoid. Recorded here as a finding, not patched.
 *
 * ---------------------------------------------------------------------------------------------
 * THE ITEM-TYPE / ITEM-CATEGORY FILTER IS DONE IN MEMORY, AS THE DESKTOP DOES IT
 * ---------------------------------------------------------------------------------------------
 * cmbDocNo_Leave reads the FULL item list and then keeps the rows whose ItemCategoryId (or
 * ItemTypeId) equals the chosen filter value - it does not ask the procedure to filter. The same
 * shape is reproduced here rather than appending a WHERE clause, so the two apps return the same
 * rows even where the procedure would have filtered differently.
 *
 * Note the two grids do not read the same list in that branch: the OUTPUT grid filters
 * Item.GetAll (@Activity=ReadByOrganizationCompanyId) and the INPUT grid filters Item.ReadAllItems
 * (@Activity=ReadAllItems). Different sources, so they are kept as two calls here.
 */
@Repository
public class ProductionJobOrderLookupsRepository {

    private static final Logger LOG = LoggerFactory.getLogger(ProductionJobOrderLookupsRepository.class);

    private final JdbcTemplate jdbc;
    public ProductionJobOrderLookupsRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    /* A lookup that fails is reported and returns empty - it never pretends the list is empty
       without saying so in the log. The screen still opens; the log names the procedure. */
    private List<Map<String, Object>> q(String label, String sql, Object... args) {
        try {
            return jdbc.queryForList(sql, args);
        } catch (Exception e) {
            LOG.warn("Production Job Order lookup '{}' failed: {}", label, sql, e);
            return new ArrayList<>();
        }
    }

    // ----------------------------------------------------------------- header / both grids

    public List<Map<String, Object>> cropYears(UserAccount u) {
        return q("cropYears",
                "EXEC dbo.Sp_InvCropYear_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "ReadAll");
    }

    public List<Map<String, Object>> warehouses(UserAccount u) {
        return q("warehouses",
                "EXEC dbo.Sp_InvWareHouse_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "GetActiveWareHouse");
    }

    public List<Map<String, Object>> jobLots(UserAccount u) {
        return q("jobLots",
                "EXEC dbo.SP_JobLot_ReadMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "GetAll");
    }

    public List<Map<String, Object>> plants(UserAccount u) {
        return q("plants",
                "EXEC dbo.Sp_InvProductionPlant_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "GetALL");
    }

    /** @Activity only - the desktop sends no tenancy here. See the class note. */
    public List<Map<String, Object>> packingTypes() {
        return q("packingTypes",
                "EXEC dbo.Sp_InvPackingType_GetAllMethod @Activity=?", "ReadAll");
    }

    public List<Map<String, Object>> coaAccounts(UserAccount u) {
        return q("coaAccounts",
                "EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @UserId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), u.getId(), "COAForCombobindig");
    }

    /** CmbWipItem - Item.GetAllbyCombobind with no parent category (the form passes none). */
    public List<Map<String, Object>> wipItems(UserAccount u) {
        return q("wipItems",
                "EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "ReadAllForComboTwoColumns");
    }

    /** ItemDetailFill - the unfiltered list both item pickers start from. */
    public List<Map<String, Object>> allItems(UserAccount u) {
        return q("allItems",
                "EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "ReadAllItems");
    }

    /** Item.GetAll - the list the OUTPUT grid filters by Item Type / Item Category. */
    public List<Map<String, Object>> itemsForOutputFilter(UserAccount u) {
        return q("itemsForOutputFilter",
                "EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "ReadByOrganizationCompanyId");
    }

    public List<Map<String, Object>> uomSchedule(UserAccount u, int itemId) {
        return q("uomSchedule",
                "EXEC dbo.Sp_UOMSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), itemId, "ReadByItemID");
    }

    // ----------------------------------------------------------------- cmbDocNo cascade

    public List<Map<String, Object>> saleOrderNumbers(UserAccount u) {
        return q("saleOrderNumbers",
                "EXEC dbo.Sp_SaleOrder_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "ReadSaleOrderNo");
    }

    public List<Map<String, Object>> lcOrderNumbers(UserAccount u) {
        return q("lcOrderNumbers",
                "EXEC dbo.Sp_ExImLcOrder_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "GetLcOrderNo");
    }

    public List<Map<String, Object>> itemTypes(UserAccount u) {
        return q("itemTypes",
                "EXEC dbo.Sp_ItemType_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "ReadByOrganizationCompanyId");
    }

    public List<Map<String, Object>> itemCategories(UserAccount u) {
        return q("itemCategories",
                "EXEC dbo.Sp_ItemCategory_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "ReadByOrganizationCompanyId");
    }

    /** SaleOrder.ReadByPurchaseOrderIDNOrderItemId - key is OrderItemId, display ItemName. */
    public List<Map<String, Object>> itemsBySaleOrder(int saleOrderId) {
        return q("itemsBySaleOrder",
                "EXEC dbo.Sp_SaleOrder_GetAllMethod @SaleOrderId=?, @Activity=?",
                saleOrderId, "ReadBySaleOrderIDNOrderItemId");
    }

    /** ExImLcOrder.ReadByExportSaleOrderIDNOrderItemId - key is ExImItemId, display ItemName. */
    public List<Map<String, Object>> itemsByExportContract(UserAccount u, int lcOrderId) {
        return q("itemsByExportContract",
                "EXEC dbo.Sp_ExImLcOrder_GetAllMethod @OrganizationId=?, @CompanyId=?, @Id=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), lcOrderId, "ReadByExportSaleOrderIDNOrderItemId");
    }
}
