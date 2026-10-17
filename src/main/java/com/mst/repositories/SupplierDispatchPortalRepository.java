package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 788 "Supplier Dispatch" - frmSupplierDispatch.cs (Architecture.WinApp.Purchase), DocumentTypeId 246.
 *
 * The form shares BLL 0517 / DAL 0383 Architecture.*.Inventory.SupplierDispatch with the Stock In Transit
 * form (868, DocumentTypeId 251): numbering, ReadById, history, delete and the four-table save are already in
 * {@link StockInTransitRepository} and are called from the service with DocumentTypeId 246. This class only
 * holds what frmSupplierDispatch calls and that class does not:
 *
 *   cmbCityFill                 City.GetAll                          -> SP_City_GetAllMethod 'GetAll'  (BLL City :GetAll)
 *   SupplierNameFill            CommonServices.GetVendorsAndCustomers(1) -> USP_GetVendorsAndCustomers (PartyTypeId 1)
 *                               CommonServices.SupplierCustomerGetforComboServiceBind
 *                                                                    -> Sp_SupplierCustomer_GetAllMethod 'ReadByOrganizationIdCompanyIdForBinding'
 *   ItemWithoutOrderFill        CommonServices.ReadAllItems          -> Sp_Item_GetAllMethod 'ReadAllItems'
 *   bindRateUomAndItemPackUom   CommonServices.GetUomScheduleByItemId-> Sp_UOMSchedule_GetAllMethod 'ReadByItemID'
 *   OrderBind                   SupplierDispatch.PurchaseOrder_ForSupplierDispatch -> USP_PurchaseOrder_ForSupplierDispatch
 */
@Repository
public class SupplierDispatchPortalRepository {

    private final JdbcTemplate jdbc;
    public SupplierDispatchPortalRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    /** City.GetAll(new City{Org, Comp}) - BLL City: @OrganizationId, @CompanyId, @MethodType 'GetAll'. */
    public List<Map<String, Object>> cities(UserAccount u) {
        return DesktopProc.rows(jdbc, "SP_City_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "MethodType", "GetAll"));
    }

    /** SupplierCustomer.GetVendorsAndCustomers(org, comp, PartyTypeId) - the SubsidiaryAccountAllownOnVouchers branch (ERP feature 4). */
    public List<Map<String, Object>> vendorsAndCustomers(UserAccount u, int partyTypeId) {
        return DesktopProc.rows(jdbc, "USP_GetVendorsAndCustomers", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "PartyTypeId", partyTypeId));
    }

    /** SupplierCustomer.GetforComboBinding - the other branch of SupplierNameFill. */
    public List<Map<String, Object>> supplierCustomersForCombo(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Activity", "ReadByOrganizationIdCompanyIdForBinding"));
    }

    /** Item.ReadAllItems(new Item{Org, Comp}) - ItemWithoutOrderFill. */
    public List<Map<String, Object>> allItems(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAllItems"));
    }

    /** UOMSchedule.SearchByObject(new UOMSchedule{Org, Comp, ItemId}) - 'ReadByItemID'. */
    public List<Map<String, Object>> uomScheduleByItem(UserAccount u, int itemId) {
        return DesktopProc.rows(jdbc, "Sp_UOMSchedule_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemId", itemId, "Activity", "ReadByItemID"));
    }

    /**
     * SupplierDispatch.PurchaseOrder_ForSupplierDispatch (BLL 0517) - org, comp, FinancialYearId always;
     * @BranchesId only when BranchFeature && BranchImplemented (OrderBind); @SupplierCustomerId always.
     */
    public List<Map<String, Object>> purchaseOrdersForSupplierDispatch(UserAccount u, int financialYearId, int supplierId, Integer branchesId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", financialYearId);
        if (branchesId != null) p.put("BranchesId", branchesId);
        p.put("SupplierCustomerId", supplierId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_PurchaseOrder_ForSupplierDispatch]", p);
    }
}
