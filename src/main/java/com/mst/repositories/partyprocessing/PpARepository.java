package com.mst.repositories.partyprocessing;

import com.mst.models.UserAccount;
import com.mst.models.hrm.DesktopModel;
import com.mst.repositories.hrm.HrmProcRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Repository;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.function.Supplier;

/**
 * DAL of the Party Processing definition screens and reports owned by this group (PpA):
 * 683 Define Item, 684 Stock Party, 685 Item Category, 686 Item Type, 689 Reference Parties,
 * 678 Stock Opening Balance, 690-694 reports.
 *
 * Every method is one desktop BLL call: the procedure, @Activity and the parameters the BLL sends
 * (guarded ones only when the BLL would send them). Tenancy always comes from the signed-in user.
 * All calls go through the shared HrmProcRepository (DesktopProc: a null value is omitted).
 */
@Repository
public class PpARepository {

    @Autowired private HrmProcRepository db;

    private static Map<String, Object> p(Object... kv) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (int i = 0; i + 1 < kv.length; i += 2) m.put(String.valueOf(kv[i]), kv[i + 1]);
        return m;
    }

    private static Map<String, Object> oc(UserAccount u, Object... kv) {
        Map<String, Object> m = p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        m.putAll(p(kv));
        return m;
    }

    public List<Map<String, Object>> rows(String proc, Map<String, Object> params) { return db.rows(proc, params); }

    public int set(String proc, DesktopModel m) { return db.set(proc, m); }

    public <T> T tx(Supplier<T> work) { return db.tx(work); }

    // ================================================================== shared combos

    /** InvLookUp.GetLookupsByTypeIdDt: Sp_InvLookup_GetAllMethod @OrganizationId, @CompanyId, @InvLookupTypeId, @Activity='ReadByInvlookTypeId'. */
    public List<Map<String, Object>> invLookups(UserAccount u, int typeId) {
        return db.rows("Sp_InvLookup_GetAllMethod", oc(u, "InvLookupTypeId", typeId, "Activity", "ReadByInvlookTypeId"));
    }

    /** Item.InventoryParentCategories (Ids empty -> not sent): Sp_InventoryItemsOther_GetAllMethod @Activity='InventoryParentCategories'. */
    public List<Map<String, Object>> inventoryParentCategories() {
        return db.rows("Sp_InventoryItemsOther_GetAllMethod", p("Activity", "InventoryParentCategories"));
    }

    /** UOM.Getall: Sp_UOM_GetAllMethod @OrganizationId, @CompanyId, @Activity='ReadByOrganizationCompanyId'. */
    public List<Map<String, Object>> uoms(UserAccount u) {
        return db.rows("Sp_UOM_GetAllMethod", oc(u, "Activity", "ReadByOrganizationCompanyId"));
    }

    /** ItemGroup.GetAll: Sp_ItemGroup_GetAllMethod @OrganizationId, @CompanyId, @Activity='ReadAll'. */
    public List<Map<String, Object>> itemGroups(UserAccount u) {
        return db.rows("Sp_ItemGroup_GetAllMethod", oc(u, "Activity", "ReadAll"));
    }

    /** CommonServices.CompanyServiceBind -> Company.GetAlldt(OrgCompanyTypeId = OrganizationId): Sp_Company_GetAllMethod 'ReadByOrganizationId'. */
    public List<Map<String, Object>> companies(UserAccount u) {
        return db.rows("Sp_Company_GetAllMethod", p("OrgCompanyTypeId", u.getOrganizationId(), "Activity", "ReadByOrganizationId"));
    }

    /** CommonServices.WareHouseGetAllService -> InvWareHouse.Getall: Sp_InvWareHouse_GetAllMethod 'ReadByOrganizationCompanyId'. */
    public List<Map<String, Object>> warehouses(UserAccount u) {
        return db.rows("Sp_InvWareHouse_GetAllMethod", oc(u, "Activity", "ReadByOrganizationCompanyId"));
    }

    /** Item.ReadAllForItemsForPartyProcessing(org, comp, "") - @ParentIds not sent: Sp_Item_GetAllMethod. */
    public List<Map<String, Object>> itemsForPartyProcessing(UserAccount u) {
        return db.rows("Sp_Item_GetAllMethod", oc(u, "Activity", "ReadAllForItemsForPartyProcessing"));
    }

    /** CommonServices.GetUomScheduleByItemId: Sp_UOMSchedule_GetAllMethod @OrganizationId, @CompanyId, @ItemId, @Activity='ReadByItemID'. */
    public List<Map<String, Object>> uomScheduleByItem(UserAccount u, int itemId) {
        return db.rows("Sp_UOMSchedule_GetAllMethod", oc(u, "ItemId", itemId, "Activity", "ReadByItemID"));
    }

    /** InvPackingType.Getall(): Sp_InvPackingType_GetAllMethod @Activity='ReadAll'. */
    public List<Map<String, Object>> packingTypes() {
        return db.rows("Sp_InvPackingType_GetAllMethod", p("Activity", "ReadAll"));
    }

    /** CommonServices.GetSupplierustomerForPartyProcessing -> SupplierCustomer.GetSupplierustomerForPartyProcessing. */
    public List<Map<String, Object>> stockParties(UserAccount u) {
        return db.rows("Sp_SupplierCustomer_GetAllMethod", oc(u, "Activity", "GetSupplierustomerForPartyProcessing"));
    }

    /** ReferenceParties.GetAll: [Sp_ReferenceParties_GetAllMethod] @OrganizationId, @CompanyId, @Activity='ReadAll'. */
    public List<Map<String, Object>> referencePartiesAll(UserAccount u) {
        return db.rows("Sp_ReferenceParties_GetAllMethod", oc(u, "Activity", "ReadAll"));
    }

    /** CommonServices.CropYearGetAllService -> InvCropYear.Getall: Sp_InvCropYear_GetAllMethod 'ReadAll'. */
    public List<Map<String, Object>> cropYears(UserAccount u) {
        return db.rows("Sp_InvCropYear_GetAllMethod", oc(u, "Activity", "ReadAll"));
    }

    /** jobLot.AllJobLotForPartyProcessing: SP_JobLot_ReadMethod @OrganizationId, @CompanyId, @Activity='AllJobLotForPartyProcessing'. */
    public List<Map<String, Object>> jobLotsForPartyProcessing(UserAccount u) {
        return db.rows("SP_JobLot_ReadMethod", oc(u, "Activity", "AllJobLotForPartyProcessing"));
    }

    /** SupplierCustomer.GetDataForDropDownFromGRNGDN (no DocumentTypeIds / Activity): USP_GetDataForDropDownFromGrnGdnStorePartyProcessing. */
    public List<Map<String, Object>> dropDownFromGrnGdnStore(UserAccount u) {
        return db.rows("USP_GetDataForDropDownFromGrnGdnStorePartyProcessing", oc(u));
    }

    /** clsGlobalVariables.ActiveYr source: Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId. */
    public List<Map<String, Object>> activeFinancialYears(UserAccount u) {
        return db.rows("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", oc(u));
    }

    /** CommonServices.GetERPFeatureById: the id among USP_GetERPFeaturesByCompanyId's rows. */
    public List<Map<String, Object>> erpFeatures(UserAccount u) {
        return db.rows("USP_GetERPFeaturesByCompanyId", oc(u));
    }

    // ================================================================== 686 Item Type

    public List<Map<String, Object>> itemTypes(UserAccount u) {
        return db.rows("Sp_ItemTypePartyProcessing_GetAllMethod", oc(u, "Activity", "ReadAll"));
    }

    public List<Map<String, Object>> itemType(int id) {
        return db.rows("Sp_ItemTypePartyProcessing_GetAllMethod", p("Id", id, "Activity", "ReadById"));
    }

    // ================================================================== 685 Item Category

    public List<Map<String, Object>> itemCategories(UserAccount u) {
        return db.rows("Sp_ItemCategoryPartyProcessing_GetAllMethod", oc(u, "Activity", "ReadAll"));
    }

    public List<Map<String, Object>> itemCategory(int id) {
        return db.rows("Sp_ItemCategoryPartyProcessing_GetAllMethod", p("Id", id, "Activity", "ReadById"));
    }

    // ================================================================== 683 Define Item

    /** ItemPartyProcessing.Getall: @ItemCategoryId not set by the form; @EntryUser only when !CanViewAllRecord; @NoOfRecords when != 0. */
    public List<Map<String, Object>> items(UserAccount u, boolean canViewAll, int noOfRecords) {
        Map<String, Object> m = oc(u, "CanViewAllRecord", canViewAll);
        if (!canViewAll) m.put("EntryUser", u.getId());
        if (noOfRecords != 0) m.put("NoOfRecords", noOfRecords);
        m.put("Activity", "ReadAll");
        return db.rows("Sp_ItemPartyProcessing_GetAllMethod", m);
    }

    public List<Map<String, Object>> item(int id) {
        return db.rows("Sp_ItemPartyProcessing_GetAllMethod", p("Id", id, "Activity", "ReadById"));
    }

    /** ItemPartyProcessing.GenerateCode: @OrganizationId, @CompanyId, @ItemCategoryId, @Activity='GenerateItemCodeByCategoryId'. */
    public List<Map<String, Object>> itemCode(UserAccount u, int categoryId) {
        return db.rows("Sp_ItemPartyProcessing_GetAllMethod", oc(u, "ItemCategoryId", categoryId, "Activity", "GenerateItemCodeByCategoryId"));
    }

    /** DAL: Sp_UOMSchedule_GetAllMethod @ItemGroupId, @Activity='ReadByItemGroupId' (no tenancy, as the DAL). */
    public List<Map<String, Object>> uomScheduleByGroup(int groupId) {
        return db.rows("Sp_UOMSchedule_GetAllMethod", p("ItemGroupId", groupId, "Activity", "ReadByItemGroupId"));
    }

    // ================================================================== 689 Reference Parties

    public List<Map<String, Object>> referencePartyTypes(UserAccount u) {
        return db.rows("Sp_ReferenceParties_GetAllMethod", oc(u, "Activity", "ReadAllReferencePartyType"));
    }

    /** ReferenceParties.FormHistory: @ReferencePartyTypeId only when != 0. */
    public List<Map<String, Object>> referencePartyHistory(UserAccount u, int typeId) {
        Map<String, Object> m = oc(u);
        if (typeId != 0) m.put("ReferencePartyTypeId", typeId);
        m.put("Activity", "FormHistory");
        return db.rows("Sp_ReferenceParties_GetAllMethod", m);
    }

    public List<Map<String, Object>> referenceParty(int id) {
        return db.rows("Sp_ReferenceParties_GetAllMethod", p("Activity", "ReadById", "Id", id));
    }

    /** SupplierCustomer.GetVendorsAndCustomers(org, comp, PartyTypeId) - @PartyTypeId only when != 0. */
    public List<Map<String, Object>> vendorsAndCustomers(UserAccount u, int partyTypeId) {
        Map<String, Object> m = oc(u);
        if (partyTypeId != 0) m.put("PartyTypeId", partyTypeId);
        return db.rows("USP_GetVendorsAndCustomers", m);
    }

    /** SupplierCustomer.GetforComboBinding: Sp_SupplierCustomer_GetAllMethod 'ReadByOrganizationIdCompanyIdForBinding'. */
    public List<Map<String, Object>> supplierCustomersForCombo(UserAccount u) {
        return db.rows("Sp_SupplierCustomer_GetAllMethod", oc(u, "Activity", "ReadByOrganizationIdCompanyIdForBinding"));
    }

    // ================================================================== 684 Stock Party (SupplierCustomer)

    /** CoaAllocationGetForComboServiceBind -> COAAllocation.GetForComboBind: @UserId only when != 0. */
    public List<Map<String, Object>> coaForCombo(UserAccount u) {
        Map<String, Object> m = oc(u);
        if (u.getId() != null && u.getId() != 0) m.put("UserId", u.getId());
        m.put("Activity", "COAForCombobindig");
        return db.rows("Sp_COAAllocation_GetAllMethod", m);
    }

    /** country.GetAll: SP_Country_ReadMethod @OrganizationId, @CompanyId, @MethodType='GetAll'. */
    public List<Map<String, Object>> countries(UserAccount u) {
        return db.rows("SP_Country_ReadMethod", oc(u, "MethodType", "GetAll"));
    }

    /** City.GetAll: SP_City_GetAllMethod @OrganizationId, @CompanyId, @MethodType='GetAll'. */
    public List<Map<String, Object>> cities(UserAccount u) {
        return db.rows("SP_City_GetAllMethod", oc(u, "MethodType", "GetAll"));
    }

    /** SupplierCustomer.Getall: 'ReadByOrganizationCompanyId' (cmbglac_Leave scans it for the GL account). */
    public List<Map<String, Object>> supplierCustomersAll(UserAccount u) {
        return db.rows("Sp_SupplierCustomer_GetAllMethod", oc(u, "Activity", "ReadByOrganizationCompanyId"));
    }

    public List<Map<String, Object>> supplierCustomer(int id) {
        return db.rows("Sp_SupplierCustomer_GetAllMethod", p("Id", id, "Activity", "ReadById"));
    }

    public List<Map<String, Object>> stockPartyHistory(UserAccount u) {
        return db.rows("Sp_SupplierCustomer_GetAllMethod", oc(u, "Activity", "FormHistoryForPartyProcessing"));
    }

    /** SupplierCustomer DAL SetData, after the SetProc: [dbo].[USP_SupplierCustomerTaxSchedule_SyncFromMapping] @OrganizationId, @CompanyId. */
    public void syncTaxSchedule(UserAccount u) {
        db.rows("[dbo].[USP_SupplierCustomerTaxSchedule_SyncFromMapping]", oc(u));
    }

    /** GeneralReprots.SupplierCustomerRegister (row check for 293 / 292_01). */
    public List<Map<String, Object>> supplierCustomerRegister(Map<String, Object> params) {
        return db.rows("Sp_SupplierCustomerHistory_rpt", params);
    }

    // ================================================================== 678 Stock Opening Balance

    /** InvStockOpeningBalancePartyProcessing.GenerateCode: @Activity, @DocumentTypeId, @OrganizationId, @CompanyId, @FinancialYearId. */
    public List<Map<String, Object>> stockOpeningCode(UserAccount u, int documentTypeId, int financialYearId) {
        return db.rows("Sp_InvStockOpeningBalancePartyProcessing_GetAllMethod", p("Activity", "GenerateCode", "DocumentTypeId", documentTypeId,
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "FinancialYearId", financialYearId));
    }

    public List<Map<String, Object>> stockOpening(int id) {
        return db.rows("Sp_InvStockOpeningBalancePartyProcessing_GetAllMethod", p("Id", id, "Activity", "ReadById"));
    }

    /** InvStockOpeningBalancePartyProcessing.DeleteById: @Id, @EntryUserId, @Activity='DeleteById' (GetDataTableProc). */
    public void deleteStockOpening(int id, int userId) {
        db.rows("Sp_InvStockOpeningBalancePartyProcessing_GetAllMethod", p("Id", id, "EntryUserId", userId, "Activity", "DeleteById"));
    }

    /** InvStockOpeningBalancePartyProcessing.GetAll: [dbo].[USP_StockOpeningBalancePartyProcessing_FormHistory] (params built by the service). */
    public List<Map<String, Object>> stockOpeningHistory(Map<String, Object> params) {
        return db.rows("[dbo].[USP_StockOpeningBalancePartyProcessing_FormHistory]", params);
    }

    /** InvStockOpeningBalancePartyProcessing_SlipandRegister (row check for the 416_01 print). */
    public List<Map<String, Object>> stockOpeningSlip(Map<String, Object> params) {
        return db.rows("[dbo].[Sp_InvStockOpeningBalancePartyProcessing_SlipandRegister]", params);
    }

    // ================================================================== reports 690-694

    /** InvGrnPartyProcessing.GetDataForDropDownFromGrn: @DocumentTypeIds / @Activity only when not empty. */
    public List<Map<String, Object>> dropDownFromGrn(UserAccount u, String documentTypeIds) {
        Map<String, Object> m = oc(u);
        if (documentTypeIds != null && !documentTypeIds.isEmpty()) m.put("DocumentTypeIds", documentTypeIds);
        return db.rows("[dbo].[USP_GetDataForDropDownFromGrnPartyProcessing]", m);
    }

    /** PartyProcessingGatePassReports.GetDataForDropDownFromGPPrtyProcessing: @Activity / @DocumentTypeIds null -> not sent. */
    public List<Map<String, Object>> dropDownFromGatePass(UserAccount u) {
        return db.rows("[dbo].[USP_GetDataForDropDownFromGatePassPartyProcessing]", oc(u));
    }

    /** StocksReport.InventoryTransactionsPartyProcessing_DropDownAndList (@ActivityType not set by the form). */
    public List<Map<String, Object>> stockDropDowns(UserAccount u) {
        return db.rows("USP_InventoryTransactionsPartyProcessing_DropDownAndLists", oc(u));
    }
}
