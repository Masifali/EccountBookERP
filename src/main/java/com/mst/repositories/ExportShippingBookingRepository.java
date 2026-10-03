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
 * 214 "Export Shipping Booking Info" - Architecture.WinApp.Export.EximShippingBookingInfo. Data layer:
 * Architecture.BLL.Export.ExImExportShipingLineBooking (BLL 0463 / DAL, models 0847 + 0817), ExImInvoice (BLL 0467),
 * ExImLookUps, SeaPorts, GeneralReprots.StaticColumnNames, GlobalServicesMethods.getGlobalSupplierCustomer. Procedures:
 *
 *   Sp_ConfigrationsAllocation_GetAllMethod 'GetConfigurationByOrgCompandConfigDescription'   DefaultDaysToLessFromHistoryFromDate
 *   SpStaticColumnNames                                            @Activity='ExportContainerTypeForCRO'          (Id, type)
 *   Sp_SeaPorts_GetAllMethod 'ReadByCompanyNOrganizationId'        @OrganizationId @CompanyId @Activity           (Id, PortName)
 *   Sp_ExImInvoice_GetAllMethod 'GetInvoiceNoForBookingInfo'       @OrganizationId @CompanyId @Activity           (Id, InvoiceNo)
 *   Sp_ExImInvoice_GetAllMethod 'ReadByOrganizationCompanyId'      @OrganizationId @CompanyId @DocumentTypeIds='204,1816' @FinancialYearId @Id @Activity  (cmbExportInvoice_Leave)
 *   Sp_ExImLookUps_GetAllMethod 'ReadByOrganizationCompanyIdNExImLookUpTypeId'  @Activity @OrganizationId @CompanyId @ExImLookUptypesId=4   (Carrier Medium)
 *   USP_GetDataForDropDownFromShippingBookingInfo                  @OrganizationId @CompanyId                     (Activity / Id / ReferenceName)
 *   USP_GetVendorsAndCustomersWithCityName                         @OrganizationId @CompanyId                     (globalAllSupplierCustomer, group 10 kept)
 *   Sp_ExImExportShipingLineBooking_GetAllMethod 'ReadById' / 'ReadDetailByHeaderId'   @Id @Activity
 *   [dbo].[USP_ExImExportShipingLineBooking_FormHistory]           @OrganizationId @CompanyId [@Id] [dates] [@SupplierCustomerId @ShippingAgentId @ShippingLineId @TransporterId @DestinationPortId @ExImInvoiceId]
 *   Sp_ExImExportShipingLineBooking_Insert / _Update               the 30 non-virtual header properties (SetProc)
 *   USP_ExImExportShipingLineBookingDetail_Insert                  the 7 detail properties, one call per row (SetProc)
 */
@Repository
public class ExportShippingBookingRepository {

    private final JdbcTemplate jdbc;

    public ExportShippingBookingRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public String config(UserAccount u, String description) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (r.isEmpty()) return "";
        Object v = r.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    /** UserAccount.CompName of the desktop session (DeliveredAtBind row 1) - the Company row of the session's company. */
    public String companyName(UserAccount u) {
        List<Map<String, Object>> r = jdbc.queryForList("SELECT CompName FROM Company WHERE Id = ?", u.getCompanyId());
        return r.isEmpty() || r.get(0).get("CompName") == null ? "" : String.valueOf(r.get(0).get("CompName"));
    }

    public List<Map<String, Object>> containerTypes() {
        return DesktopProc.rows(jdbc, "SpStaticColumnNames", params("Activity", "ExportContainerTypeForCRO"));
    }

    public List<Map<String, Object>> ports(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_SeaPorts_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByCompanyNOrganizationId"));
    }

    public List<Map<String, Object>> invoicesForBookingInfo(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetInvoiceNoForBookingInfo"));
    }

    public List<Map<String, Object>> invoiceData(UserAccount u, int financialYearId, String documentTypeIds, int invoiceId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (documentTypeIds != null && !documentTypeIds.isEmpty()) p.put("DocumentTypeIds", documentTypeIds);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (invoiceId != 0) p.put("Id", invoiceId);
        p.put("Activity", "ReadByOrganizationCompanyId");
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", p);
    }

    public List<Map<String, Object>> carrierMediums(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ExImLookUps_GetAllMethod", params(
                "Activity", "ReadByOrganizationCompanyIdNExImLookUpTypeId",
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ExImLookUptypesId", 4));
    }

    public List<Map<String, Object>> historyDropDowns(UserAccount u) {
        return DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromShippingBookingInfo",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    public List<Map<String, Object>> globalAllSupplierCustomer(UserAccount u) {
        return DesktopProc.rows(jdbc, "USP_GetVendorsAndCustomersWithCityName",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    public List<Map<String, Object>> byId(int id) {
        return DesktopProc.rows(jdbc, "Sp_ExImExportShipingLineBooking_GetAllMethod", params("Id", id, "Activity", "ReadById"));
    }

    public List<Map<String, Object>> detailsByHeaderId(int id) {
        return DesktopProc.rows(jdbc, "Sp_ExImExportShipingLineBooking_GetAllMethod", params("Id", id, "Activity", "ReadDetailByHeaderId"));
    }

    public List<Map<String, Object>> formHistory(UserAccount u, Map<String, Object> filters) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        p.putAll(filters);
        return DesktopProc.rows(jdbc, "[dbo].[USP_ExImExportShipingLineBooking_FormHistory]", p);
    }

    /** DAL SetDate: header SetProc (0 back -> obj.Id), then every detail row through USP_ExImExportShipingLineBookingDetail_Insert; one transaction. */
    @Transactional(rollbackFor = Exception.class)
    public int save(Map<String, Object> header, List<Map<String, Object>> details) {
        int id = header.get("Id") == null ? 0 : ((Number) header.get("Id")).intValue();
        Integer n = DesktopProc.scalar(jdbc, id == 0 ? "Sp_ExImExportShipingLineBooking_Insert" : "Sp_ExImExportShipingLineBooking_Update", header);
        int headerId = (n == null || n == 0) ? id : n;
        for (Map<String, Object> d : details) {
            d.put("ExImExportShipingLineBookingId", headerId);
            DesktopProc.scalar(jdbc, "USP_ExImExportShipingLineBookingDetail_Insert", d);
        }
        return headerId;
    }
}
