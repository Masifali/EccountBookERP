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
 * "Goods Receipts At Port" - Architecture.WinApp.Export.ExImGoodsReceiptsAtPort and its loader
 * GetForwardingDataForGoodsReceiptsAsPort. Data layer: BLL 0455 / DAL 0507 / model 0830 ExImGoodsReceiptsAtPort,
 * ExImForwarding.GetForwardingDataForGoodsReceiptsAsPort (BLL 0465:773), CommonServices.GetSupplierustomerByCustomerGroupId
 * (SupplierCustomer BLL), MultiCurrency.GetAll, CommonServices.ItemGetForComboServiceBind (Item.GetAllbyCombobind, BLL 0583).
 * Procedures and parameters as read from procdure.utf8.sql (03-Oct-2026):
 *
 *   Sp_ExImGoodsReceiptsAtPort_GetAllMetohd  @OrganizationId @CompanyId @BranchId @Activity='FormHistory'       BindHistory
 *   USP_ExImGoodsReceiptsAtPort_Insert       the 15 model properties (SetProc), one per grid row              Save, obj.Id == 0
 *   USP_ExImGoodsReceiptsAtPort_Update       the 15 model properties (SetProc), one per grid row              Save, obj.Id == 1 (UpdateMode)
 *   USP_GetForwardingDataForGoodsReceiptsAsPort @OrganizationId @CompanyId @FinancialYearId @BranchesId
 *                                            [@SupplierCustomerId >0] [@ItemId >0] [@FcurrencyId >0] @SkipZero=1   loader grid
 *   Sp_SupplierCustomer_GetAllMethod         @OrganizationId @CompanyId @CustomerGroupIds='7' @Activity='GetSupplierustomerByCustomerGroupId'
 *   Sp_MultiCurrency_GetAllMethod            @OrganizationId @CompanyId @Activity='ReadAll'
 *   Sp_Item_GetAllMethod                     @OrganizationId @CompanyId @Activity='ReadAllForComboTwoColumns'
 */
@Repository
public class ExportGoodsReceiptsAtPortRepository {

    private final JdbcTemplate jdbc;

    public ExportGoodsReceiptsAtPortRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    /** BLL Getall - BranchId is the user's BranchesId. */
    public List<Map<String, Object>> history(UserAccount u, int branchId) {
        return DesktopProc.rows(jdbc, "Sp_ExImGoodsReceiptsAtPort_GetAllMetohd", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchId", branchId, "Activity", "FormHistory"));
    }

    /** ExImForwarding.GetForwardingDataForGoodsReceiptsAsPort - ZeroBalanceType 1 is always set by the loader. */
    public List<Map<String, Object>> forwardingData(UserAccount u, int financialYearId, int branchId,
                                                    int partyId, int itemId, int currencyId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", financialYearId, "BranchesId", branchId);
        if (partyId > 0) p.put("SupplierCustomerId", partyId);
        if (itemId > 0) p.put("ItemId", itemId);
        if (currencyId > 0) p.put("FcurrencyId", currencyId);
        p.put("SkipZero", 1);
        return DesktopProc.rows(jdbc, "USP_GetForwardingDataForGoodsReceiptsAsPort", p);
    }

    /** CommonServices.GetSupplierustomerByCustomerGroupId(ids) - ParentId 0 is not sent. */
    public List<Map<String, Object>> partiesByGroup(UserAccount u, String groupIds) {
        return DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "CustomerGroupIds", groupIds, "Activity", "GetSupplierustomerByCustomerGroupId"));
    }

    public List<Map<String, Object>> currencies(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_MultiCurrency_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    /** ItemGetForComboServiceBind() - ItemCategoryId 0, so @InventoryParentCategoriesId is not sent. */
    public List<Map<String, Object>> itemsCombo(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAllForComboTwoColumns"));
    }

    /**
     * DAL ExImGoodsReceiptsAtPort.SetData: every list item through the procedure the BLL chose, one transaction,
     * rolled back on any error. Returns the last ExecuteScalar (0 when nothing came back), as the DAL does.
     */
    @Transactional(rollbackFor = Exception.class)
    public int save(String proc, List<Map<String, Object>> items) {
        int num = 0;
        for (Map<String, Object> item : items) {
            Integer v = DesktopProc.scalar(jdbc, proc, item);
            num = v == null ? 0 : v;
        }
        return num;
    }
}
