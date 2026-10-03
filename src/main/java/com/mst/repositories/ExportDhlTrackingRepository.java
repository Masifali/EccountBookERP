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
 * "DHL Tracking" - Architecture.WinApp.Export.frmDhlTracking (DocumentTypeId 248). Data layer: DHLTracking BLL 0442 /
 * DAL 0493 / model 0788, CommonServices.GetExportLookUpsDataByTypeIdDt(6) (ExImLookUps.GetByLookTypeId).
 * The desktop form never calls a DHL web service - it is a plain register of air-way bills kept in dbo.DHLTracking.
 *
 *   Sp_ExImLookUps_GetAllMethod           @Activity='ReadByOrganizationCompanyIdNExImLookUpTypeId' @OrganizationId @CompanyId @ExImLookUptypesId=6
 *   USP_GetDataForDropDownFromDHLTracking @OrganizationId @CompanyId                (Activity not set -> not sent)   ComboBindHistory
 *   USP_DHLTracking_GetAllMethod          @OrganizationId @CompanyId @DocumentTypeId=248 @CanViewAllRecord [@EntryUserId]
 *                                         [@DispatchedFromDate] [@DispatchedToDate] [@FinalETAFromDate] [@FinalETAToDate]
 *                                         [@AWBBillNumber] [@CurrentStatusId] @Activity='FormHistory'                BindGrid
 *   USP_DHLTracking_GetAllMethod          @DHLTrackingId @Activity='ReadById'                                         GetByID
 *   USP_DHLTracking_GetAllMethod          @DHLTrackingId @EntryUserId @Activity='DeleteById'  (own transaction)      DeleteById
 *   USP_DHLTracking_InsertAndUpdate       the 20 non-virtual model properties (SetProc)                              Save
 */
@Repository
public class ExportDhlTrackingRepository {

    private final JdbcTemplate jdbc;

    public ExportDhlTrackingRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public List<Map<String, Object>> currentStatuses(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ExImLookUps_GetAllMethod", params(
                "Activity", "ReadByOrganizationCompanyIdNExImLookUpTypeId",
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ExImLookUptypesId", 6));
    }

    public List<Map<String, Object>> dropDowns(UserAccount u) {
        return DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromDHLTracking", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** BLL FormHistory - the first table of the DataSet (the second is the company logo for the register print). */
    public List<Map<String, Object>> history(Map<String, Object> p) {
        return DesktopProc.rows(jdbc, "USP_DHLTracking_GetAllMethod", p);
    }

    public List<Map<String, Object>> readById(long id) {
        return DesktopProc.rows(jdbc, "USP_DHLTracking_GetAllMethod", params("DHLTrackingId", id, "Activity", "ReadById"));
    }

    @Transactional(rollbackFor = Exception.class)
    public void deleteById(long id, int entryUserId) {
        DesktopProc.rows(jdbc, "[dbo].[USP_DHLTracking_GetAllMethod]", params("DHLTrackingId", id, "EntryUserId", entryUserId, "Activity", "DeleteById"));
    }

    /** DAL SetData: Convert.ToInt32(SetProc(...)); 0 when the procedure selected nothing. */
    @Transactional(rollbackFor = Exception.class)
    public long save(Map<String, Object> model) {
        Integer v = DesktopProc.scalar(jdbc, "USP_DHLTracking_InsertAndUpdate", model);
        return v == null ? 0L : v;
    }
}
