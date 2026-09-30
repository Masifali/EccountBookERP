package com.mst.repositories.partyprocessing;

import com.mst.models.UserAccount;
import com.mst.repositories.hrm.HrmProcRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Map;

/**
 * Combo sources and configuration reads shared by the Party Processing transaction screens of this
 * port. Each method names the desktop call it reproduces (WinApp CommonServices -> BLL -> procedure);
 * parameters and guards are the BLL's. Tenancy always comes from the signed-in user.
 * Reads only - nothing here writes.
 */
@Repository
public class PpBLookupRepository {

    private final HrmProcRepository db;

    public PpBLookupRepository(HrmProcRepository db) { this.db = db; }

    public HrmProcRepository db() { return db; }

    private static int org(UserAccount u) { return u.getOrganizationId(); }
    private static int co(UserAccount u) { return u.getCompanyId(); }

    // ------------------------------------------------------------------ configuration / features

    /**
     * CommonServices.GetConfigurationByOrgCompandConfigDescription / GlobalVariables_Helper.GetConfigValueFromGlobal:
     * Sp_ConfigrationsAllocation_GetAllMethod @Activity='GetConfigurationByOrgCompandConfigDescription' -> ConfigKey,
     * "" when there is no row (the desktop helpers return "" and Conversion.ToBool / ToInt read it as false / 0).
     */
    public String config(UserAccount u, String description) {
        List<Map<String, Object>> r = db.rows("Sp_ConfigrationsAllocation_GetAllMethod", "OrganizationId", org(u), "CompanyId", co(u),
                "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription");
        if (r.isEmpty() || r.get(0).get("ConfigKey") == null) return "";
        return String.valueOf(r.get(0).get("ConfigKey")).trim();
    }

    /** CommonServices.GetERPFeatureById(id) / GetERPFeaturesByCompanyId: USP_GetERPFeaturesByCompanyId contains that Id. */
    public boolean feature(UserAccount u, int featureId) {
        for (Map<String, Object> r : db.rows("USP_GetERPFeaturesByCompanyId", "OrganizationId", org(u), "CompanyId", co(u))) {
            Object id = r.get("Id");
            if (id instanceof Number && ((Number) id).intValue() == featureId) return true;
        }
        return false;
    }

    /** CommonServices.StaticColumnsService(activity): SpStaticColumnNames @Activity. */
    public List<Map<String, Object>> staticColumns(String activity) {
        return db.rows("SpStaticColumnNames", "Activity", activity);
    }

    // ------------------------------------------------------------------ parties

    /** CommonServices.GetSupplierustomerForPartyProcessing -> BLL 0600 -> Sp_SupplierCustomer_GetAllMethod 'GetSupplierustomerForPartyProcessing'. */
    public List<Map<String, Object>> stockParties(UserAccount u) {
        return db.rows("Sp_SupplierCustomer_GetAllMethod", "OrganizationId", org(u), "CompanyId", co(u),
                "Activity", "GetSupplierustomerForPartyProcessing");
    }

    /** CommonServices.ReferencePartyServiceBind / ReferenceParties.GetAll (BLL 0074) -> [Sp_ReferenceParties_GetAllMethod] 'ReadAll'. */
    public List<Map<String, Object>> referenceParties(UserAccount u) {
        return db.rows("Sp_ReferenceParties_GetAllMethod", "OrganizationId", org(u), "CompanyId", co(u), "Activity", "ReadAll");
    }

    /** CommonServices.GetVendorsAndCustomers(partyTypeId): USP_GetVendorsAndCustomers @PartyTypeId. */
    public List<Map<String, Object>> vendorsAndCustomers(UserAccount u, int partyTypeId) {
        return db.rows("USP_GetVendorsAndCustomers", "OrganizationId", org(u), "CompanyId", co(u), "PartyTypeId", partyTypeId);
    }

    /** CommonServices.SupplierCustomerGetforComboServiceBind -> SupplierCustomer.GetforComboBinding (BLL 0600). */
    public List<Map<String, Object>> supplierCustomerCombo(UserAccount u) {
        return db.rows("Sp_SupplierCustomer_GetAllMethod", "OrganizationId", org(u), "CompanyId", co(u),
                "Activity", "ReadByOrganizationIdCompanyIdForBinding");
    }

    /** CommonServices.GetVendorsAndCustomersForTransporter: USP_GetVendorsAndCustomersForTransporter (no paging arguments). */
    public List<Map<String, Object>> vendorsForTransporter(UserAccount u) {
        return db.rows("USP_GetVendorsAndCustomersForTransporter", "OrganizationId", org(u), "CompanyId", co(u));
    }

    /** CommonServices.CoaAllocationGetAllServiceBind -> COAAllocation.GetAll: Sp_COAAllocation_GetAllMethod 'COAAllocationSearch' (@UserId when set). */
    public List<Map<String, Object>> coaAllocations(UserAccount u) {
        Integer uid = u.getId();
        return db.rows("Sp_COAAllocation_GetAllMethod", "OrganizationId", org(u), "CompanyId", co(u),
                "UserId", uid != null && uid != 0 ? uid : null, "Activity", "COAAllocationSearch");
    }

    // ------------------------------------------------------------------ inventory masters

    /** CommonServices.GetLookupsByTypeIdDt(typeId) -> InvLookUp.GetLookupsByTypeIdDt (BLL 0577): Sp_InvLookup_GetAllMethod 'ReadByInvlookTypeId'. */
    public List<Map<String, Object>> lookupsByType(UserAccount u, int typeId) {
        return db.rows("Sp_InvLookup_GetAllMethod", "OrganizationId", org(u), "CompanyId", co(u), "InvLookupTypeId", typeId,
                "Activity", "ReadByInvlookTypeId");
    }

    /** VehicleType.GetAll (BLL 0611): Sp_VehicleType_GetAllMethod with NO parameter (the built @Activity list is never passed). */
    public List<Map<String, Object>> vehicleTypes() {
        return db.rows("Sp_VehicleType_GetAllMethod");
    }

    /** CommonServices.CropYearGetAllService -> InvCropYear.Getall (BLL 0571): Sp_InvCropYear_GetAllMethod 'ReadAll'. */
    public List<Map<String, Object>> cropYears(UserAccount u) {
        return db.rows("Sp_InvCropYear_GetAllMethod", "OrganizationId", org(u), "CompanyId", co(u), "Activity", "ReadAll");
    }

    /** InvPackingType.Getall (BLL 0579): Sp_InvPackingType_GetAllMethod 'ReadAll' (no tenancy parameters on the proc). */
    public List<Map<String, Object>> packingTypes() {
        return db.rows("Sp_InvPackingType_GetAllMethod", "Activity", "ReadAll");
    }

    /** UOMSchedule.SearchByObject (BLL 0610): Sp_UOMSchedule_GetAllMethod 'ReadByItemID'. */
    public List<Map<String, Object>> uoms(UserAccount u, int itemId) {
        return db.rows("Sp_UOMSchedule_GetAllMethod", "OrganizationId", org(u), "CompanyId", co(u), "ItemId", itemId,
                "Activity", "ReadByItemID");
    }

    /** jobLot.AllJobLotForPartyProcessing (BLL 0594): SP_JobLot_ReadMethod 'AllJobLotForPartyProcessing'. */
    public List<Map<String, Object>> jobLotsPartyProcessing(UserAccount u) {
        return db.rows("SP_JobLot_ReadMethod", "OrganizationId", org(u), "CompanyId", co(u), "Activity", "AllJobLotForPartyProcessing");
    }

    /** CommonServices.JobLotGetAllService -> jobLot.GetAll (BLL 0594): SP_JobLot_ReadMethod 'GetAll'. */
    public List<Map<String, Object>> jobLotsAll(UserAccount u) {
        return db.rows("SP_JobLot_ReadMethod", "OrganizationId", org(u), "CompanyId", co(u), "Activity", "GetAll");
    }

    /** Item.ReadAllForItemsForPartyProcessing(org, co, parentIds) (BLL 0583): Sp_Item_GetAllMethod, @ParentIds when not blank. */
    public List<Map<String, Object>> itemsForPartyProcessing(UserAccount u, String parentIds) {
        return db.rows("Sp_Item_GetAllMethod", "OrganizationId", org(u), "CompanyId", co(u),
                "ParentIds", parentIds == null || parentIds.isEmpty() ? null : parentIds, "Activity", "ReadAllForItemsForPartyProcessing");
    }

    /**
     * CommonServices.GetItemPartyProcessing(ids) -> ItemPartyProcessing.GetItemsForPartyProcessing (BLL 0308):
     * Sp_ItemPartyProcessing_GetAllMethod 'GetItemsForPartyProcessing', @ParentCategoriesIds when not empty.
     */
    public List<Map<String, Object>> itemPartyProcessing(UserAccount u, String ids) {
        return db.rows("Sp_ItemPartyProcessing_GetAllMethod", "OrganizationId", org(u), "CompanyId", co(u),
                "ParentCategoriesIds", ids == null || ids.isEmpty() ? null : ids, "Activity", "GetItemsForPartyProcessing");
    }

    /** CommonServices.ReadAllItems -> Item.ReadAllItems (BLL 0583): Sp_Item_GetAllMethod 'ReadAllItems'. */
    public List<Map<String, Object>> readAllItems(UserAccount u) {
        return db.rows("Sp_Item_GetAllMethod", "OrganizationId", org(u), "CompanyId", co(u), "Activity", "ReadAllItems");
    }

    /** CommonServices.GetItemByItemTypeId(ids) -> Item.GetItemByItemTypeId: Sp_Item_GetAllMethod @LookupTypeIds, 'GetItemByItemTypeId'. */
    public List<Map<String, Object>> itemsByItemType(UserAccount u, String ids) {
        return db.rows("Sp_Item_GetAllMethod", "OrganizationId", org(u), "CompanyId", co(u), "LookupTypeIds", ids,
                "Activity", "GetItemByItemTypeId");
    }

    /** CommonServices.WareHouseGetAllService / InvWareHouse.Getall (BLL 0582): Sp_InvWareHouse_GetAllMethod 'ReadByOrganizationCompanyId'. */
    public List<Map<String, Object>> warehouses(UserAccount u) {
        return db.rows("Sp_InvWareHouse_GetAllMethod", "OrganizationId", org(u), "CompanyId", co(u), "Activity", "ReadByOrganizationCompanyId");
    }

    /** CommonServices.getActiveWareHouse -> InvWareHouse.GetActiveWareHouse (BLL 0582): 'GetActiveWareHouse'. */
    public List<Map<String, Object>> activeWarehouses(UserAccount u) {
        return db.rows("Sp_InvWareHouse_GetAllMethod", "OrganizationId", org(u), "CompanyId", co(u), "Activity", "GetActiveWareHouse");
    }

    /** CommonServices.CityGetAllService / City.GetAll (BLL 0060): SP_City_GetAllMethod @MethodType='GetAll'. */
    public List<Map<String, Object>> cities(UserAccount u) {
        return db.rows("SP_City_GetAllMethod", "OrganizationId", org(u), "CompanyId", co(u), "MethodType", "GetAll");
    }

    /** CommonServices.GetItemGlIdsandItemName (DAL 0205): Sp_Item_GetAllMethod 'GetItemGlIdsandItemName' (Id, ItemName). */
    public List<Map<String, Object>> itemGlIdsAndNames(UserAccount u) {
        return db.rows("Sp_Item_GetAllMethod", "OrganizationId", org(u), "CompanyId", co(u), "Activity", "GetItemGlIdsandItemName");
    }

    /** CommonServices.GetEqvilentByItemIdAndUomScheduleId (DAL 0205): Sp_Item_GetAllMethod @ScheduleId -> Equivalent (0 when no row). */
    public double equivalentByItemAndSchedule(UserAccount u, int itemId, int scheduleId) {
        List<Map<String, Object>> r = db.rows("Sp_Item_GetAllMethod", "OrganizationId", org(u), "CompanyId", co(u), "ItemId", itemId,
                "ScheduleId", scheduleId, "Activity", "GetEqvilentByItemIdAndUomScheduleId");
        if (r.isEmpty()) return 0d;
        Object v = r.get(0).get("Equivalent");
        return v instanceof Number ? ((Number) v).doubleValue() : 0d;
    }
}
