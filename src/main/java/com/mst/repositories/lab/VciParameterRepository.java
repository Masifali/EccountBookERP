package com.mst.repositories.lab;

import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 809 "VCI Parameter" — desktop Architecture.WinApp.Lookups/ExImVCIParameter.cs,
 * BLL 0037 (ExImVCIParameter) + 0036 (ExImVCICategory), DAL 0036, Model 0049.
 *
 * Every statement is one the desktop form issues, with the parameters the BLL sends, in its order:
 *
 *   USP_ExImVCICategory_GetAllMethod   @OrganizationId, @CompanyId, @Activity='FormHistory'
 *        ParameterCategoryfill (form :94) -> BLL 0036 GetAll. Columns used: Id, VciCategoryDescription.
 *   USP_ExImVCIParameter_GetAllMethod  @OrganizationId, @CompanyId, [@ExImVCICartegoryId], @Activity='GetAll'
 *        FormHistory (form :251) -> BLL 0037 GetAll. @Id is added only when != 0 (the form never sets it)
 *        and @ExImVCICartegoryId only when != 0. Columns used: VCIParameterId, VciCategoryDescription,
 *        VciParameterDescription.
 *   USP_ExImVCIParameter_Insert / _Update
 *        BLL 0037 save -> DAL 0036 SetData (own transaction) -> GenericProvider.SetProc (0207:283), which
 *        sends ONE PARAMETER PER MODEL PROPERTY in declaration order (Model 0049): EntryDate, ModifyDate,
 *        CompanyId, EntryUserId, ExImVCICartegoryId, Id, ModifyUserId, OrganizationId, SortNo,
 *        VciParameterDescription. The form never sets SortNo, so it is 0. Both procedures write the
 *        USP_UserAudit_Insert row themselves.
 */
@Repository
public class VciParameterRepository {

    private final JdbcTemplate jdbc;

    public VciParameterRepository(JdbcTemplate jdbc) {
        this.jdbc = jdbc;
    }

    public List<Map<String, Object>> categories(int organizationId, int companyId) {
        return DesktopProc.rows(jdbc, "USP_ExImVCICategory_GetAllMethod", params(
                "OrganizationId", organizationId, "CompanyId", companyId, "Activity", "FormHistory"));
    }

    /** categoryId 0 = the parameter is not sent (BLL 0037: "if (obj.ExImVCICartegoryId != 0)"). */
    public List<Map<String, Object>> getAll(int organizationId, int companyId, int categoryId) {
        return DesktopProc.rows(jdbc, "USP_ExImVCIParameter_GetAllMethod", params(
                "OrganizationId", organizationId, "CompanyId", companyId,
                "ExImVCICartegoryId", categoryId != 0 ? categoryId : null,
                "Activity", "GetAll"));
    }

    private static Map<String, Object> model(Timestamp now, int companyId, int userId, int categoryId, int id,
                                             int organizationId, String description) {
        return params("EntryDate", now, "ModifyDate", now, "CompanyId", companyId, "EntryUserId", userId,
                "ExImVCICartegoryId", categoryId, "Id", id, "ModifyUserId", userId,
                "OrganizationId", organizationId, "SortNo", 0, "VciParameterDescription", description);
    }

    /** Ends in SELECT @Id (Max(Id)+1) — ExecuteScalar on the desktop. */
    public Integer insert(Timestamp now, int companyId, int userId, int categoryId, int organizationId, String description) {
        return DesktopProc.scalar(jdbc, "USP_ExImVCIParameter_Insert",
                model(now, companyId, userId, categoryId, 0, organizationId, description));
    }

    /** Returns no result set; the DAL then answers obj.Id (0036: "num = obj.Id"). */
    public void update(Timestamp now, int companyId, int userId, int categoryId, int id, int organizationId, String description) {
        DesktopProc.setProc(jdbc, "USP_ExImVCIParameter_Update",
                model(now, companyId, userId, categoryId, id, organizationId, description));
    }
}
