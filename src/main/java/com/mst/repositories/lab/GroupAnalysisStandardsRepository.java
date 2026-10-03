package com.mst.repositories.lab;

import com.mst.repositories.support.ProcExec;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Map;

/**
 * Procedure calls of screen 158 "Group Analysis Standards" (desktop
 * Architecture.WinApp.Lab.InvLabGroupAnalysisStandards). Every statement carries exactly the
 * parameters the desktop BLL adds; "bll:n" = projects/architecture.bll file line.
 */
@Repository
public class GroupAnalysisStandardsRepository {

    private final JdbcTemplate jdbc;

    public GroupAnalysisStandardsRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    /**
     * BLL.Lab.InvLabGroupAnalysisStandards.GetAllOrById (0406 bll:30-58):
     * Sp_InvLabGroupAnalysisStandards_GetAllMethod, @Id only when non-zero, @OrganizationId, @CompanyId,
     * @Activity 'ReadAll'.
     */
    public List<Map<String, Object>> getAllOrById(int organizationId, int companyId, int id) {
        if (id != 0) {
            return jdbc.queryForList(
                    "EXEC dbo.Sp_InvLabGroupAnalysisStandards_GetAllMethod @Id=?, @OrganizationId=?, @CompanyId=?, @Activity=?",
                    id, organizationId, companyId, "ReadAll");
        }
        return jdbc.queryForList(
                "EXEC dbo.Sp_InvLabGroupAnalysisStandards_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                organizationId, companyId, "ReadAll");
    }

    /**
     * BLL.Lab.InvLabAnalysisGroup.GetAllOrById (0400 bll:51-87): Sp_InvLabAnalysisGroup_GetAllMethod,
     * @Id / @ParentCategoryId only when non-zero (the form sends neither, GroupFill :185), @OrganizationId,
     * @CompanyId, @Activity 'ReadAll'. {@code id} is used only by the server-side reference check.
     */
    public List<Map<String, Object>> analysisGroups(int organizationId, int companyId, int id) {
        if (id != 0) {
            return jdbc.queryForList(
                    "EXEC dbo.Sp_InvLabAnalysisGroup_GetAllMethod @Id=?, @OrganizationId=?, @CompanyId=?, @Activity=?",
                    id, organizationId, companyId, "ReadAll");
        }
        return jdbc.queryForList(
                "EXEC dbo.Sp_InvLabAnalysisGroup_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                organizationId, companyId, "ReadAll");
    }

    /**
     * BLL.Lab.InvLabAnalysisItems.GetAllOrById (0402 bll:29-57): Sp_InvLabAnalysisItems_GetAllMethod,
     * @Id only when non-zero, @OrganizationId, @CompanyId, @Activity 'ReadAll'.
     */
    public List<Map<String, Object>> analysisItems(int organizationId, int companyId, int id) {
        if (id != 0) {
            return jdbc.queryForList(
                    "EXEC dbo.Sp_InvLabAnalysisItems_GetAllMethod @Id=?, @OrganizationId=?, @CompanyId=?, @Activity=?",
                    id, organizationId, companyId, "ReadAll");
        }
        return jdbc.queryForList(
                "EXEC dbo.Sp_InvLabAnalysisItems_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                organizationId, companyId, "ReadAll");
    }

    /**
     * BLL Save (0406 bll:14-22) -> DAL SetDate (0361) -> GenericProvider.SetProc (0207 dal:283): every
     * property of Model.Lab.InvLabGroupAnalysisStandards (0624) is sent as a parameter - MaxValue,
     * MinValue, CompanyId, Id, InvLabAnalysisGroup, InvLabAnalysisItems, OrganizationId,
     * IsEditableAfterApproval. Id == 0 -> Sp_InvLabGroupAnalysisStandards_Insert (ends with SELECT @Id),
     * otherwise Sp_InvLabGroupAnalysisStandards_Update (returns no rows; the DAL then keeps obj.Id).
     */
    public Integer save(int id, int groupId, int itemId, double minValue, double maxValue,
                        int organizationId, int companyId, boolean editableAfterApproval) {
        String proc = id == 0 ? "Sp_InvLabGroupAnalysisStandards_Insert" : "Sp_InvLabGroupAnalysisStandards_Update";
        Integer scalar = ProcExec.call(jdbc,
                "EXEC dbo." + proc + " @MaxValue=?, @MinValue=?, @CompanyId=?, @Id=?, @InvLabAnalysisGroup=?, "
                        + "@InvLabAnalysisItems=?, @OrganizationId=?, @IsEditableAfterApproval=?",
                maxValue, minValue, companyId, id, groupId, itemId, organizationId, editableAfterApproval);
        return scalar != null && scalar > 0 ? scalar : Integer.valueOf(id);      // DAL 0361: num > 0 ? num : obj.Id
    }
}
