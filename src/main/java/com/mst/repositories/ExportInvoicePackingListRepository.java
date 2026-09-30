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
 * 882 "Export Invoice Packing List" - Architecture.WinApp.Export.frmExportInvoicePackingList (App 8
 * "Export", ModuleId 11). Data layer: Architecture.BLL.Export.ExImInvoice (BLL 0467) / DAL 0519,
 * ExImLcOrder.GetDataForDropDownFromExportInvoice, CommodityDetailItemCustomerWise.GetRemarks and
 * CommonServices.GetERPFeatureById(9). Every call is the desktop's own procedure with the desktop's
 * own parameters (procdure.utf8.sql, 29-Sep-2026):
 *
 *   USP_ExImInvoice_GetInvoicesRefferedInDo        @OrganizationId @CompanyId @FinancialYearId     cmbInvoiceNo (Id, InvoiceNo)
 *   USP_GetDataForDropDownFromExportInvoice        @OrganizationId @CompanyId                      history combos (Id, name, ActivityType)
 *   USP_ExImInvoice_ReadByIdForPackingList         @ExImInvoiceId                                  header (one row)
 *   Sp_ExImInvoice_GetAllMethod                    @Id @Activity='ReadExImInvoicePackingDetailByHeaderId'     dtdetail (invoice detail - weights)
 *                                                  @Id @Activity='ExImInvoicePackingListDetail_ReadByHeaderId' saved packing-list rows
 *   USP_InvDeliveryOrder_ReadByInvoiceForPackingList @ExImInvoiceId                                DO rows that seed the grid for a new list
 *   USP_ExImInvoicePackingListDetail_Insert        the 27 non-virtual model properties               SetDataForPackingListDetail, one call per row
 *   USP_ExImInvoicePackingListDetail_DeleteByInvoiceId @DeletedByUserId @ExImInvoiceId              BtnDelete
 *   USP_ExImInvoice_FormHistoryForPackingList      see history()                                   History grid
 *   USP_CommodityDetailItemCustomerWise_GetAllMethod @OrganizationId @CompanyId @ItemId [@SupplierCustomerId] @Activity='GetRemarks'   F1 popups
 *   USP_GetERPFeaturesByCompanyId                  @OrganizationId @CompanyId                      ERP feature 9 AllowExportMultiCompanies
 *   Sp_ConfigrationsAllocation_GetAllMethod        GetConfigurationByOrgCompandConfigDescription    DefaultDaysToLessFromHistoryFromDate, ItemSearchByCode
 *
 * No table, column or procedure is created or changed.
 */
@Repository
public class ExportInvoicePackingListRepository {

    private final JdbcTemplate jdbc;

    public ExportInvoicePackingListRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

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
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetERPFeaturesByCompanyId",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            Object id = ExportGdBreakUpRepository.ci(r, "Id");
            if (id instanceof Number && ((Number) id).intValue() == featureId) return true;
            if (id != null && String.valueOf(id).trim().equals(String.valueOf(featureId))) return true;
        }
        return false;
    }

    /** ExImInvoice.ExImInvoice_GetInvoicesRefferedInDo(org, comp, ActiveYr.Id). */
    public List<Map<String, Object>> invoicesReferredInDo(UserAccount u, int financialYearId) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_ExImInvoice_GetInvoicesRefferedInDo]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "FinancialYearId", financialYearId));
    }

    /** ExImLcOrder.GetDataForDropDownFromExportInvoice(org, comp) - no Activity, no DocumentTypeIds. */
    public List<Map<String, Object>> historyDropDowns(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromExportInvoice]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** ExImInvoice.ExImInvoice_ReadByIdForPackingList. */
    public List<Map<String, Object>> headerById(int invoiceId) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_ExImInvoice_ReadByIdForPackingList]", params("ExImInvoiceId", invoiceId));
    }

    /** ExImInvoice.ExImInvoicePackingDetail_ReadByHeaderId - dtdetail. */
    public List<Map<String, Object>> invoiceDetail(int invoiceId) {
        return DesktopProc.rows(jdbc, "[dbo].[Sp_ExImInvoice_GetAllMethod]",
                params("Id", invoiceId, "Activity", "ReadExImInvoicePackingDetailByHeaderId"));
    }

    /** ExImInvoice.ExImInvoicePackingListDetail_ReadByInvoiceId. */
    public List<Map<String, Object>> packingListDetail(int invoiceId) {
        return DesktopProc.rows(jdbc, "[dbo].[Sp_ExImInvoice_GetAllMethod]",
                params("Id", invoiceId, "Activity", "ExImInvoicePackingListDetail_ReadByHeaderId"));
    }

    /** ExImInvoice.InvDeliveryOrder_ReadByInvoiceForPackingList. */
    public List<Map<String, Object>> deliveryOrderRows(int invoiceId) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_InvDeliveryOrder_ReadByInvoiceForPackingList]", params("ExImInvoiceId", invoiceId));
    }

    /** CommodityDetailItemCustomerWise.GetRemarks(org, comp, ItemId, SupplierCustomerId) - @SupplierCustomerId only when != 0. */
    public List<Map<String, Object>> commodityRemarks(UserAccount u, int itemId, int supplierCustomerId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ItemId", itemId);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        p.put("Activity", "GetRemarks");
        return DesktopProc.rows(jdbc, "[dbo].[USP_CommodityDetailItemCustomerWise_GetAllMethod]", p);
    }

    /**
     * ExImInvoice.FormHistoryForPackingList(ReportsParameters) - GUARDED parameters exactly as the BLL
     * adds them: the date pair of the chosen radio only when ticked, @SupplierCustomerId / @Id /
     * @DocNoFrom / @DocNoTo / @ActionId only when non-zero, @DocumentTypeIds = "204"; ApprovedFilter is
     * "All" so @IsApproved is never sent; NoOfRecords / PageSize / PageNumber are never set.
     */
    public List<Map<String, Object>> history(UserAccount u, int financialYearId, Map<String, Object> guarded) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", financialYearId);
        p.putAll(guarded);
        return DesktopProc.rows(jdbc, "[USP_ExImInvoice_FormHistoryForPackingList]", p);
    }

    /**
     * DAL ExImInvoice.SetDataForPackingListDetail: every list item (removed rows first, then the grid
     * rows) through USP_ExImInvoicePackingListDetail_Insert with ExImInvoiceId = the header id; one
     * transaction; returns the header id.
     */
    @Transactional(rollbackFor = Exception.class)
    public int savePackingList(int invoiceId, List<Map<String, Object>> items) {
        for (Map<String, Object> item : items) {
            item.put("ExImInvoiceId", invoiceId);
            DesktopProc.scalar(jdbc, "USP_ExImInvoicePackingListDetail_Insert", item);
        }
        return invoiceId;
    }

    /** ExImInvoice.PackingListDeleteByInvoiceId(InvoiceId, EntryUserId). */
    @Transactional(rollbackFor = Exception.class)
    public void deleteByInvoiceId(int invoiceId, int userId) {
        DesktopProc.rows(jdbc, "[dbo].[USP_ExImInvoicePackingListDetail_DeleteByInvoiceId]",
                params("DeletedByUserId", userId, "ExImInvoiceId", invoiceId));
    }
}
