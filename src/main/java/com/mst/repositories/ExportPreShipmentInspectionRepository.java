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
 * 188 "Pre-Shipment Inspection" - Architecture.WinApp.Export.PreShipmentInspection. Data layer: Architecture.BLL.Export
 * .InvLabPreProductionExportLotInspectionHeader (BLL 0472 / DAL 0524, models 0866 + 0839), ExImLcOrder (BLL 0469),
 * ExImLookUps (BLL 0473), CommonServices.GetJobLotGlIdsandName (clsGlobalVariables.globalJobLot). Procedures:
 *
 *   Sp_ConfigrationsAllocation_GetAllMethod 'GetConfigurationByOrgCompandConfigDescription'   DefaultDaysToLessFromHistoryFromDate
 *   Sp_ExImLookUps_GetAllMethod 'ReadByOrganizationCompanyIdNExImLookUpTypeId'   @ExImLookUptypesId=3  (Inspection Status)
 *   Sp_InvLabPreProductionExportLotInspectionHeader_GetAllMethod 'GenerateCode'     @OrganizationId @CompanyId @Activity  (DocNo)
 *   Sp_InvLabPreProductionExportLotInspectionHeader_GetAllMethod 'ReadById' / 'ReadByHeaderId'   @Id @Activity
 *   [dbo].[USP_GetDataForDropDownFrom_LabPreExportLotInspection]  @OrganizationId @CompanyId @Activity='Items'   (history Item combo)
 *   [dbo].[USP_GetDataForDropDownFromExportContract]              @OrganizationId @CompanyId [@SupplierCustomerId] [@Activity='ContractNo']  (Items / Customer / ContractNo)
 *   SP_JobLot_ReadMethod 'GetJobLotGlIdsandName'                  @OrganizationId @CompanyId @Activity
 *   [dbo].[usp_InvLabPreProductionExportLotInspectionHeader_FormHistory]   @OrganizationId @CompanyId @ScreenName @FinancialYearId @CanViewAllRecord [@EntryUser] [dates] [@ItemId @FromDocNo @ToDocNo]
 *   USP_InvLabPreProductionExportLotInspectionHeader_InsertAndUpdate   the 73 non-virtual header properties (SetProc)
 *   Sp_InvLabPreProductionExportLotInspectionDetail_Insert        the 12 detail properties, one call per grid row (SetProc)
 * The form never passes RemoveDetailIds, so usp_InvLabPreProductionExportLotInspectionDetailDeletebyIds never runs (desktop behaviour kept).
 */
@Repository
public class ExportPreShipmentInspectionRepository {

    private final JdbcTemplate jdbc;

    public ExportPreShipmentInspectionRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public String config(UserAccount u, String description) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (r.isEmpty()) return "";
        Object v = r.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    public List<Map<String, Object>> inspectionStatuses(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ExImLookUps_GetAllMethod", params(
                "Activity", "ReadByOrganizationCompanyIdNExImLookUpTypeId",
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ExImLookUptypesId", 3));
    }

    public List<Map<String, Object>> generateCode(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_InvLabPreProductionExportLotInspectionHeader_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GenerateCode"));
    }

    public List<Map<String, Object>> historyItems(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFrom_LabPreExportLotInspection]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "Items"));
    }

    /** ExImLcOrder.GetDataForDropDownFromExportContract - all activities (Items / Customer) or 'ContractNo' for one customer. */
    public List<Map<String, Object>> contractDropDowns(UserAccount u, int supplierCustomerId, String activity) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (activity != null && !activity.isEmpty()) p.put("Activity", activity);
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromExportContract]", p);
    }

    public List<Map<String, Object>> jobLots(UserAccount u) {
        return DesktopProc.rows(jdbc, "SP_JobLot_ReadMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetJobLotGlIdsandName"));
    }

    public List<Map<String, Object>> byId(int id) {
        return DesktopProc.rows(jdbc, "Sp_InvLabPreProductionExportLotInspectionHeader_GetAllMethod", params("Id", id, "Activity", "ReadById"));
    }

    public List<Map<String, Object>> detailsByHeaderId(int id) {
        return DesktopProc.rows(jdbc, "Sp_InvLabPreProductionExportLotInspectionHeader_GetAllMethod", params("Id", id, "Activity", "ReadByHeaderId"));
    }

    public List<Map<String, Object>> formHistory(UserAccount u, Map<String, Object> filters) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        p.putAll(filters);
        return DesktopProc.rows(jdbc, "[dbo].[usp_InvLabPreProductionExportLotInspectionHeader_FormHistory]", p);
    }

    /** DAL SetData: header SetProc (0 back -> obj.Id), then every detail row; one transaction. */
    @Transactional(rollbackFor = Exception.class)
    public int save(Map<String, Object> header, List<Map<String, Object>> details) {
        int id = header.get("Id") == null ? 0 : ((Number) header.get("Id")).intValue();
        Integer n = DesktopProc.scalar(jdbc, "USP_InvLabPreProductionExportLotInspectionHeader_InsertAndUpdate", header);
        int headerId = (n == null || n == 0) ? id : n;
        for (Map<String, Object> d : details) {
            d.put("InvLabPreProductionExportLotInspectionHeaderId", headerId);
            DesktopProc.scalar(jdbc, "Sp_InvLabPreProductionExportLotInspectionDetail_Insert", d);
        }
        return headerId;
    }
}
