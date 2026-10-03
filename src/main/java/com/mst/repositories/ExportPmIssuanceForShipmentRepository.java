package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * "Packing Material Issuance For Shipment" - Architecture.WinApp.Export.PackingMaterialIssuanceForShipment
 * (InvGsStoreIssuanceHeader with DocumentTypeId 213). The store-issuance data layer itself (BLL 0252 / DAL 0221:
 * GenerateCode, FormHistory, GetByID, Save procedures, stock posting, voucher) is the one StoreIssuanceRepository already
 * implements for 451 / 452 and is reused as is; this class holds only the calls the 213 form adds:
 *
 *   usp_getExportInvoicePendingAndAll        @OrganizationId @CompanyId @ActionId=1 [@RecId > 0]            InvoiceNoFill
 *   Sp_ExImInvoice_GetAllMethod              @OrganizationId @CompanyId @Id @Activity='GetInvoiceInformationByInvoiceIdForServices'   InvoiceInfo
 *   Sp_SeaPorts_GetAllMethod                 @OrganizationId @CompanyId @Activity='ReadByCompanyNOrganizationId'   PortFrom
 *   [dbo].[USP_Item_AllItemsWithModal]       @OrganizationId @CompanyId                                     clsGlobalVariables.getGlobalAllItems
 *   [dbo].[USP_GETAllAccountsFromCustomGroups] @OrganizationId @CompanyId                                   clsGlobalVariables.AllAccountsWithCustomGroupId
 *   Sp_InvGsStoreIssuanceHeader_SlipandRegister @OrganizationId @CompanyId @Id                              475 slip (StoreIssuanceHistory)
 */
@Repository
public class ExportPmIssuanceForShipmentRepository {

    private final JdbcTemplate jdbc;

    public ExportPmIssuanceForShipmentRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    /** ExImInvoice.getExportInvoicePendingAndAll(ActionId 1, RecId) - @RecId only when > 0. */
    public List<Map<String, Object>> invoicesPending(UserAccount u, int recId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ActionId", 1);
        if (recId > 0) p.put("RecId", recId);
        return DesktopProc.rows(jdbc, "usp_getExportInvoicePendingAndAll", p);
    }

    public List<Map<String, Object>> invoiceInformation(UserAccount u, int invoiceId) {
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", invoiceId,
                "Activity", "GetInvoiceInformationByInvoiceIdForServices"));
    }

    public List<Map<String, Object>> seaPorts(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_SeaPorts_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByCompanyNOrganizationId"));
    }

    /** GlobalServicesMethods.AllItemsWithModal(org, comp, 0, 0, "") - page size / number / keyword not sent. */
    public List<Map<String, Object>> allItems(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_Item_AllItemsWithModal]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** GlobalServicesMethods.GetGlobalAllAccountsWithCustomGroup(org, comp, 0, 0, ""). */
    public List<Map<String, Object>> allAccounts(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GETAllAccountsFromCustomGroups]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** CommonServices.StoreIssuanceHeader_Slip475 -> InvGsStoreIssuanceHeader.StoreIssuanceHistory with Id only. */
    public List<Map<String, Object>> slip475(UserAccount u, int id) {
        return DesktopProc.rows(jdbc, "Sp_InvGsStoreIssuanceHeader_SlipandRegister", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", id));
    }
}
