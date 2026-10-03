package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Data layer shared by the Export contract screens 209 "Export Contract" (FrmExportSalesContract,
 * DocumentTypeId 202), 879 "Export Contract (III)" (frmExportContractIII, DocumentTypeId 223) and
 * 240 "Packing Detail By Export Contract" (frmExportSalesContractPmDetail). Every call is one of the
 * desktop BLL calls traced to its procedure in procdure.utf8.sql (01-Oct-2026):
 *
 *   Sp_ExImLcOrder_GetAllMethod 'GenerateDocNoCompanyIdOrganizationId'  @OrganizationId @CompanyId @DocumentTypeId [@FinancialYearId]   ExImLcOrder.GenerateCode -> DocNo
 *   Sp_ExImLcOrder_GetAllMethod 'ReadById'                               @Id                                       ExImLcOrder.GetByID header
 *   Sp_ExImLcOrder_GetAllMethod 'ReadByLcOrderHeaderId' | 'ReadByLcOrderHeaderIdForSaleContracttExport' @Id   DAL ExImLcOrder.GetDate detail (201 -> first, else second)
 *   Sp_ExImLcOrder_GetAllMethod 'ReadExImLcOrderOtherItemsByHeaderId' | 'ReadExImLcOrderPaymentTermsDetailByHeaderId' |
 *                               'ReadExImLcContractPackingMaterialDetailByHeaderId' | 'ExImLcOrderOtherChargesDetail_ReadByHeaderId' @Id
 *   Sp_ExImLcOrder_GetAllMethod 'GetIdByDocNo'          @OrganizationId @CompanyId @LcOrderNo [@FinancialYearId]   txtLcOrderNo_Leave
 *   Sp_ExImLcOrder_GetAllMethod 'GetLcOrderNo'          @OrganizationId @CompanyId [@FinancialYearId]              CommonServices.GetLcOrderNo (240 contract combo)
 *   Sp_ExImLcOrder_GetAllMethod 'GetAllLcOrderSchedulingPolicy'                                                     txtFactoryLoadingDate_ValueChanged
 *   Sp_ExImLcOrder_Insert / Sp_ExImLcOrder_Update        the 90 non-virtual ExImLcOrder properties                   DAL ExImLcOrder.SetDate
 *   Sp_ExImLcOrderPackingDetail_Insert                   the 34 non-virtual ExImLcOrderPackingDetail properties      (ActionTypeId 1/2/3)
 *   Sp_ExImLcOrderOtherItems_Insert                      @Id @ExImLcOrderId @otherItemId @oItemQty @oItemWeightKgs @oItemRate @oItemAmount @OtherItemRemarks
 *   USP_ExImLcOrderPaymentTermsDetail_Insert             @Id @LcOrderId @PaymentTermId @PrcntOfTotal @FcyAmount @ExportDueTypeId @DueDays @SortNo @Remarks
 *   USP_ExImLcContractPackingMaterialDetail_Insert       @Id @ExImLcOrderId @ItemId @BrandId @PackingTypeId @Qty @Rate @Amount @pmRemarks @SortNo @BrandUomId @BrandOuterQty @BrandInnerQty @ItemUomId @InnerQty
 *   USP_ExImLcOrderOtherChargesDetail_Insert             @Id @ExImLcOrderId @ChargesItemId @AddAmount @LessAmount @Remarks
 *   USP_ExImLcContractPackingMaterialDetail_DeletePreviousData @ExImLcOrderId                                       DAL SetDateForPmDetail (240)
 *   [dbo].[USP_ExImLcOrder_FormHistory]                  @OrganizationId @CompanyId @DocumentTypeIds @CanViewAllRecord [@FinancialYearId] [@EntryUser] [dates] [@DocNoFrom @DocNoTo] [@SupplierCustomerId]
 *   [dbo].[USP_GetDataForDropDownFromExportContract]     @OrganizationId @CompanyId [@Activity]                     history Customer combo
 *   USP_ExImLcOrder_LastSavedRecord                      @OrganizationId @CompanyId [@DocumentTypeId]
 *   USP_ExImLcOrder_LastSavedRecordByItemId              @OrganizationId @CompanyId @ItemId
 *   Sp_SupplierCustomer_GetAllMethod 'ReadByOrganizationCompanyIdForExport' | 'GetSupplierustomerByCustomerGroupId' (@CustomerGroupIds)
 *   USP_GetVendorsAndCustomersWithCityName               @OrganizationId @CompanyId                                clsGlobalVariables.globalAllSupplierCustomer (879)
 *   Sp_ExImDeliveryTerm_GetAllMethod 'ReadAll' | Sp_ExImLcPaymentTerm_GetAllMethod 'ReadAll' | Sp_SeaPorts_GetAllMethod 'ReadByCompanyNOrganizationId'
 *   Sp_MultiCurrency_GetAllMethod 'ReadAll' | usp_getMultiCurrencywithExchangeRate @CompanyId | Sp_InvCropYear_GetAllMethod 'ReadAll'
 *   Sp_Bank_GetAllMethod 'ReadAll' | Sp_ExImPackMaterilaType_GetAllMethod 'ReadAll' | USP_ExportCharges_GetAllMethod 'ComboBind'
 *   Sp_Item_GetAllMethod 'ReadAllForExportCombo' | 'ReadAllForComboTwoColumnsWithParentId' | 'GetItemByItemTypeId' (@LookupTypeIds='14')
 *   [dbo].[USP_Item_AllItemsWithModal] | [dbo].[usp_getBrands] | usp_getAllUomsByCompanyId [@ItemId]              clsGlobalVariables.* (879, 240)
 *   GetUomScheduleByItemIdForExport @OrganizationId @CompanyId @ItemId | Sp_UOMSchedule_GetAllMethod 'ReadByItemID' @ItemId
 *   [dbo].[USP_CommodityDetailItemCustomerWise_GetAllMethod] 'GetRemarks' @ItemId [@SupplierCustomerId]
 *   [sdt].[USP_GetCustomGroupAllocatedToCustomerBySupplierCustomerId] @OrganizationId @CompanyId @SupplierCustomerId
 *   usp_getFarmingNTrade | usp_getExportCompaniesByCompanyId | usp_getLastRateByItemId @ItemId
 *   [mrp].[USP_ItemAndPMItemMap_GetByItemIds]            @OrganizationId @CompanyId (ItemIds null -> omitted)
 *   [dbo].[USP_ExportInvoiceNos_GetAllMethod] 'GetFinalInvoiceNosForExportContract' @OrganizationId @CompanyId @DocumentTypeId [@InvoiceNo]
 *   Sp_ConfigrationsAllocation_GetAllMethod 'GetConfigurationByOrgCompandConfigDescription' | USP_GetERPFeaturesByCompanyId
 *
 * GenericProvider.SetProc sends EVERY non-virtual property of the model, so the save methods take
 * the full property maps the services build (unset ints 0, unset strings omitted). The saves run in
 * one transaction and roll back on any error, as the DAL does. No table, column or procedure is
 * created or changed; attachments (DMS file copy) are not part of this port.
 */
@Repository
public class ExportSalesContractRepository {

    private final JdbcTemplate jdbc;

    public ExportSalesContractRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private static Map<String, Object> tenant(UserAccount u) {
        return params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    // ------------------------------------------------------------------ configuration / features

    /** CommonServices.GetConfigurationByOrgCompandConfigDescription(name) - ConfigKey, "" when absent. */
    public String config(UserAccount u, String description) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (r.isEmpty()) return "";
        Object v = r.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    /** CommonServices.GetERPFeatureById(id) - membership in USP_GetERPFeaturesByCompanyId. */
    public boolean erpFeature(UserAccount u, int featureId) {
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetERPFeaturesByCompanyId", tenant(u))) {
            Object id = r.get("Id");
            if (id instanceof Number && ((Number) id).intValue() == featureId) return true;
            if (id != null && String.valueOf(id).trim().equals(String.valueOf(featureId))) return true;
        }
        return false;
    }

    // ------------------------------------------------------------------ ExImLcOrder reads

    /** ExImLcOrder.GenerateCode - the next LcOrderDocNo. */
    public int generateCode(UserAccount u, int documentTypeId, int financialYearId) {
        Map<String, Object> p = tenant(u);
        p.put("DocumentTypeId", documentTypeId);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        p.put("Activity", "GenerateDocNoCompanyIdOrganizationId");
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ExImLcOrder_GetAllMethod", p);
        if (r.isEmpty()) return 0;
        Object v = r.get(0).get("DocNo");
        return v instanceof Number ? ((Number) v).intValue() : 0;
    }

    public List<Map<String, Object>> header(int id) {
        return DesktopProc.rows(jdbc, "Sp_ExImLcOrder_GetAllMethod", params("Id", id, "Activity", "ReadById"));
    }

    /** DAL ExImLcOrder.GetDate: DocumentTypeId 201 reads 'ReadByLcOrderHeaderId', every other type the Export variant. */
    public List<Map<String, Object>> detail(int id, int documentTypeId) {
        return DesktopProc.rows(jdbc, "Sp_ExImLcOrder_GetAllMethod", params("Id", id,
                "Activity", documentTypeId == 201 ? "ReadByLcOrderHeaderId" : "ReadByLcOrderHeaderIdForSaleContracttExport"));
    }

    public List<Map<String, Object>> otherItems(int id) {
        return DesktopProc.rows(jdbc, "Sp_ExImLcOrder_GetAllMethod", params("Id", id, "Activity", "ReadExImLcOrderOtherItemsByHeaderId"));
    }

    public List<Map<String, Object>> paymentTermsDetail(int id) {
        return DesktopProc.rows(jdbc, "Sp_ExImLcOrder_GetAllMethod", params("Id", id, "Activity", "ReadExImLcOrderPaymentTermsDetailByHeaderId"));
    }

    public List<Map<String, Object>> packingMaterialDetail(int id) {
        return DesktopProc.rows(jdbc, "Sp_ExImLcOrder_GetAllMethod", params("Id", id, "Activity", "ReadExImLcContractPackingMaterialDetailByHeaderId"));
    }

    public List<Map<String, Object>> otherChargesDetail(int id) {
        return DesktopProc.rows(jdbc, "Sp_ExImLcOrder_GetAllMethod", params("Id", id, "Activity", "ExImLcOrderOtherChargesDetail_ReadByHeaderId"));
    }

    /** ExImLcOrder.GetIdByDocNo - the desktop passes the typed Doc No as @LcOrderNo (its own quirk). */
    public List<Map<String, Object>> idByDocNo(UserAccount u, String orderNo, int financialYearId) {
        Map<String, Object> p = tenant(u);
        p.put("LcOrderNo", orderNo);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        p.put("Activity", "GetIdByDocNo");
        return DesktopProc.rows(jdbc, "Sp_ExImLcOrder_GetAllMethod", p);
    }

    /** CommonServices.GetLcOrderNo(FinancialYearId = 0) - Id, LcOrderNo, SupCustId, PartyName. */
    public List<Map<String, Object>> lcOrderNos(UserAccount u, int financialYearId) {
        Map<String, Object> p = tenant(u);
        if (financialYearId > 0) p.put("FinancialYearId", financialYearId);
        p.put("Activity", "GetLcOrderNo");
        return DesktopProc.rows(jdbc, "Sp_ExImLcOrder_GetAllMethod", p);
    }

    /** CommonServices.GetAllLcOrderSchedulingPolicy - ProductionInterval, InspectionInterval, PackMaterialInterval. */
    public List<Map<String, Object>> schedulingPolicy() {
        return DesktopProc.rows(jdbc, "Sp_ExImLcOrder_GetAllMethod", params("Activity", "GetAllLcOrderSchedulingPolicy"));
    }

    public List<Map<String, Object>> lastSavedRecord(UserAccount u, int documentTypeId) {
        Map<String, Object> p = tenant(u);
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_ExImLcOrder_LastSavedRecord]", p);
    }

    public List<Map<String, Object>> lastSavedRecordByItemId(UserAccount u, int itemId) {
        Map<String, Object> p = tenant(u);
        p.put("ItemId", itemId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_ExImLcOrder_LastSavedRecordByItemId]", p);
    }

    /** ExImLcOrder.FormHistory(ReportsParameters): the guarded parameters are built by the service. */
    public List<Map<String, Object>> formHistory(Map<String, Object> p) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_ExImLcOrder_FormHistory]", p);
    }

    /** ExImLcOrder.GetDataForDropDownFromExportContract(org, comp, Activity) - Id, name, ActivityType. */
    public List<Map<String, Object>> historyDropDowns(UserAccount u, String activity) {
        Map<String, Object> p = tenant(u);
        if (activity != null && !activity.isEmpty()) p.put("Activity", activity);
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromExportContract]", p);
    }

    // ------------------------------------------------------------------ saves

    /**
     * DAL ExImLcOrder.SetDate: the header through Sp_ExImLcOrder_Insert (ActionId 1) or _Update
     * (ActionId 2) - the insert SELECTs the new Id, the update selects nothing so the id stays - then
     * every list row through its insert procedure with the header id; one transaction. The
     * attachment copy / Proc_DMSAttachments_Insert branch is not part of this port.
     */
    @Transactional(rollbackFor = Exception.class)
    public int saveContract(Map<String, Object> header, boolean update,
                            List<Map<String, Object>> details, List<Map<String, Object>> otherItems,
                            List<Map<String, Object>> paymentTerms, List<Map<String, Object>> pmRows,
                            List<Map<String, Object>> otherCharges) {
        int num = DesktopProc.setProc(jdbc, update ? "Sp_ExImLcOrder_Update" : "Sp_ExImLcOrder_Insert", header);
        int id = num > 0 ? num : (Integer) header.get("Id");
        for (Map<String, Object> d : details) { d.put("ExImLcOrderId", id); DesktopProc.setProc(jdbc, "Sp_ExImLcOrderPackingDetail_Insert", d); }
        for (Map<String, Object> d : otherItems) { d.put("ExImLcOrderId", id); DesktopProc.setProc(jdbc, "Sp_ExImLcOrderOtherItems_Insert", d); }
        for (Map<String, Object> d : paymentTerms) { d.put("LcOrderId", id); DesktopProc.setProc(jdbc, "USP_ExImLcOrderPaymentTermsDetail_Insert", d); }
        for (Map<String, Object> d : pmRows) { d.put("ExImLcOrderId", id); DesktopProc.setProc(jdbc, "USP_ExImLcContractPackingMaterialDetail_Insert", d); }
        for (Map<String, Object> d : otherCharges) { d.put("ExImLcOrderId", id); DesktopProc.setProc(jdbc, "USP_ExImLcOrderOtherChargesDetail_Insert", d); }
        return id;
    }

    /** DAL ExImLcOrder.SetDateForPmDetail: delete the contract's PM rows, re-insert the grid; returns the contract id. */
    @Transactional(rollbackFor = Exception.class)
    public int saveForPmDetail(int contractId, List<Map<String, Object>> pmRows) {
        DesktopProc.rows(jdbc, "[dbo].[USP_ExImLcContractPackingMaterialDetail_DeletePreviousData]", params("ExImLcOrderId", contractId));
        for (Map<String, Object> d : pmRows) { d.put("ExImLcOrderId", contractId); DesktopProc.setProc(jdbc, "USP_ExImLcContractPackingMaterialDetail_Insert", d); }
        return contractId;
    }

    // ------------------------------------------------------------------ drop-down sources

    public List<Map<String, Object>> customersForExport(UserAccount u) {
        Map<String, Object> p = tenant(u); p.put("Activity", "ReadByOrganizationCompanyIdForExport");
        return DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", p);
    }

    /** CommonServices.GetSupplierustomerByCustomerGroupId("9") - ParentId 0 is not sent. */
    public List<Map<String, Object>> supplierCustomerByGroup(UserAccount u, String groupIds) {
        Map<String, Object> p = tenant(u); p.put("CustomerGroupIds", groupIds); p.put("Activity", "GetSupplierustomerByCustomerGroupId");
        return DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", p);
    }

    /** GlobalServicesMethods.getGlobalSupplierCustomer(org, comp) - clsGlobalVariables.globalAllSupplierCustomer. */
    public List<Map<String, Object>> globalSupplierCustomers(UserAccount u) {
        return DesktopProc.rows(jdbc, "USP_GetVendorsAndCustomersWithCityName", tenant(u));
    }

    public List<Map<String, Object>> deliveryTerms() {
        return DesktopProc.rows(jdbc, "Sp_ExImDeliveryTerm_GetAllMethod", params("Activity", "ReadAll"));
    }

    public List<Map<String, Object>> paymentTerms(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ExImLcPaymentTerm_GetAllMethod", params("Activity", "ReadAll", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    public List<Map<String, Object>> seaPorts(UserAccount u) {
        Map<String, Object> p = tenant(u); p.put("Activity", "ReadByCompanyNOrganizationId");
        return DesktopProc.rows(jdbc, "Sp_SeaPorts_GetAllMethod", p);
    }

    public List<Map<String, Object>> currencies(UserAccount u) {
        Map<String, Object> p = tenant(u); p.put("Activity", "ReadAll");
        return DesktopProc.rows(jdbc, "Sp_MultiCurrency_GetAllMethod", p);
    }

    /** MultiCurrency.GetMultiCurrencywithExchangeRate(CompanyId) - Id, CurrencyCode, CurrencyName, ExchangeRate, BaseCurrencyId. */
    public List<Map<String, Object>> currenciesWithExchangeRate(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[usp_getMultiCurrencywithExchangeRate]", params("CompanyId", u.getCompanyId()));
    }

    public List<Map<String, Object>> cropYears(UserAccount u) {
        Map<String, Object> p = tenant(u); p.put("Activity", "ReadAll");
        return DesktopProc.rows(jdbc, "Sp_InvCropYear_GetAllMethod", p);
    }

    public List<Map<String, Object>> banks(UserAccount u) {
        Map<String, Object> p = tenant(u); p.put("Activity", "ReadAll");
        return DesktopProc.rows(jdbc, "Sp_Bank_GetAllMethod", p);
    }

    public List<Map<String, Object>> packTypes(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ExImPackMaterilaType_GetAllMethod", params("Activity", "ReadAll", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    public List<Map<String, Object>> exportCharges(UserAccount u) {
        Map<String, Object> p = tenant(u); p.put("Activity", "ComboBind");
        return DesktopProc.rows(jdbc, "USP_ExportCharges_GetAllMethod", p);
    }

    /** Item.ReadAllForExportCombo - Id, ItemName, ItemCode (ItemClassId 8). */
    public List<Map<String, Object>> itemsForExportCombo(UserAccount u) {
        Map<String, Object> p = tenant(u); p.put("Activity", "ReadAllForExportCombo");
        return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", p);
    }

    /** CommonServices.ReadAllForComboTwoColumnsWithParentId() - all allocated items (ParentCategoryId 0 not sent). */
    public List<Map<String, Object>> itemsTwoColumnsWithParent(UserAccount u) {
        Map<String, Object> p = tenant(u); p.put("Activity", "ReadAllForComboTwoColumnsWithParentId");
        return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", p);
    }

    /** CommonServices.GetItemByItemTypeId("14") - packing material items: Id, ItemName, ItemCodeNew. */
    public List<Map<String, Object>> itemsByType14(UserAccount u) {
        Map<String, Object> p = tenant(u); p.put("LookupTypeIds", "14"); p.put("Activity", "GetItemByItemTypeId");
        return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", p);
    }

    /** GlobalServicesMethods.AllItemsWithModal - clsGlobalVariables.getGlobalAllItems. */
    public List<Map<String, Object>> allItemsWithModal(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_Item_AllItemsWithModal]", tenant(u));
    }

    /** GlobalServicesMethods.AllBrands - clsGlobalVariables.getGlobalAllBrands (Id, BrandName, BrandCode). */
    public List<Map<String, Object>> brands(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[usp_getBrands]", tenant(u));
    }

    /** CommonServices.dtUomFromGloablUomScheduleByItemId - the global UOM schedule filtered to the item. */
    public List<Map<String, Object>> globalUoms(UserAccount u, int itemId) {
        Map<String, Object> p = tenant(u);
        if (itemId != 0) p.put("ItemId", itemId);
        return DesktopProc.rows(jdbc, "usp_getAllUomsByCompanyId", p);
    }

    /** CommonServices.GetUomScheduleByItemIdForExport - Id, UOMCode, Equivalent, QtyEquivalent, BaseRateUom. */
    public List<Map<String, Object>> uomForExport(UserAccount u, int itemId) {
        Map<String, Object> p = tenant(u); p.put("ItemId", itemId);
        return DesktopProc.rows(jdbc, "GetUomScheduleByItemIdForExport", p);
    }

    /** CommonServices.GetUomScheduleByItemId - Sp_UOMSchedule_GetAllMethod 'ReadByItemID'. */
    public List<Map<String, Object>> uomByItem(UserAccount u, int itemId) {
        Map<String, Object> p = tenant(u); p.put("ItemId", itemId); p.put("Activity", "ReadByItemID");
        return DesktopProc.rows(jdbc, "Sp_UOMSchedule_GetAllMethod", p);
    }

    /** CommodityDetailItemCustomerWise.GetRemarks(ItemId, SupplierCustomerId) - @SupplierCustomerId only when != 0. */
    public List<Map<String, Object>> commodityRemarks(UserAccount u, int itemId, int supplierCustomerId) {
        Map<String, Object> p = tenant(u);
        p.put("ItemId", itemId);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        p.put("Activity", "GetRemarks");
        return DesktopProc.rows(jdbc, "[dbo].[USP_CommodityDetailItemCustomerWise_GetAllMethod]", p);
    }

    /** ClientCustomGroup.GetCustomGroupAllocatedToCustomerBySupplierCustomerId - CustomGroupId, CustomGroup. */
    public List<Map<String, Object>> customGroupsByCustomer(UserAccount u, int supplierCustomerId) {
        Map<String, Object> p = tenant(u); p.put("SupplierCustomerId", supplierCustomerId);
        return DesktopProc.rows(jdbc, "[sdt].[USP_GetCustomGroupAllocatedToCustomerBySupplierCustomerId]", p);
    }

    /** ExImLcOrder.GetFarmingNTrade - Id, FarmingNTrade, ExImFarmingTypeId, ExImTradeTypeId. */
    public List<Map<String, Object>> farmingNTrade() {
        return DesktopProc.rows(jdbc, "usp_getFarmingNTrade", params());
    }

    /** ExImLcOrder.GetExportCompaniesByCompany - Id, CompName. */
    public List<Map<String, Object>> exportCompanies(UserAccount u) {
        return DesktopProc.rows(jdbc, "usp_getExportCompaniesByCompanyId", tenant(u));
    }

    /** InventoryStockEvalautionDetail.GetLastRateByItemId - ItemRate of the first row, 0 when none. */
    public double lastRateByItemId(UserAccount u, int itemId) {
        Map<String, Object> p = tenant(u); p.put("ItemId", itemId);
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "usp_getLastRateByItemId", p);
        if (r.isEmpty()) return 0;
        Object v = r.get(0).get("ItemRate");
        return v instanceof Number ? ((Number) v).doubleValue() : 0;
    }

    /** ItemAndPMItemMap.ItemAndPMItemMap_GetByItemIds(comp, org, null) - a null ItemIds is not sent. */
    public List<Map<String, Object>> itemAndPmItemMap(UserAccount u) {
        return DesktopProc.rows(jdbc, "[mrp].[USP_ItemAndPMItemMap_GetByItemIds]", tenant(u));
    }

    /** GenerateExportInvoiceNos.GetFinalInvoiceNosForExportContract - Id, FinalInvoiceNo; @InvoiceNo only when given. */
    public List<Map<String, Object>> finalInvoiceNosForContract(UserAccount u, int documentTypeId, String contractNo) {
        Map<String, Object> p = tenant(u);
        p.put("DocumentTypeId", documentTypeId);
        if (contractNo != null && !contractNo.isEmpty()) p.put("InvoiceNo", contractNo);
        p.put("Activity", "GetFinalInvoiceNosForExportContract");
        return DesktopProc.rows(jdbc, "[dbo].[USP_ExportInvoiceNos_GetAllMethod]", p);
    }
}
