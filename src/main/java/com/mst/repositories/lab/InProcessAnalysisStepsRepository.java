package com.mst.repositories.lab;

import com.mst.repositories.support.ProcExec;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.List;
import java.util.Map;

/**
 * Procedure calls of screen 163 "InProcess Analysis Steps Schedule" (desktop
 * Architecture.WinApp.Lab.LabInProcessAnalysisStepAndParameterSchedule) and of the helper form its
 * "+" button opens, Architecture.WinApp.DefineProcessStep. Every statement carries exactly the
 * parameters the desktop BLL adds; "bll:n" = projects/architecture.bll file line.
 */
@Repository
public class InProcessAnalysisStepsRepository {

    private static final String P_SCHEDULE_GET = "dbo.Sp_LabInProcessAnalysisStepAndParameterSchedule_GetAllMethod";
    private static final String P_STEP_GET = "dbo.Sp_LabInProcessAnalysisStep_GetAllMethod";

    private final JdbcTemplate jdbc;

    public InProcessAnalysisStepsRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ------------------------------------------------------------------ schedule (BLL 0396)

    /** InvProcessAnalysisStepSchedule.GetAllOrById (0396 bll:59-93): @Id only when non-zero, 'ReadAll'. */
    public List<Map<String, Object>> getAllOrById(int organizationId, int companyId, int id) {
        if (id != 0) {
            return jdbc.queryForList("EXEC " + P_SCHEDULE_GET + " @Id=?, @OrganizationId=?, @CompanyId=?, @Activity=?",
                    id, organizationId, companyId, "ReadAll");
        }
        return jdbc.queryForList("EXEC " + P_SCHEDULE_GET + " @OrganizationId=?, @CompanyId=?, @Activity=?",
                organizationId, companyId, "ReadAll");
    }

    /** InvProcessAnalysisStepSchedule.GenerateCode (0396 bll:31-57): 'GenerateSortNo', Rows[0]["SortNo"]. */
    public int generateSortNo(int organizationId, int companyId) {
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC " + P_SCHEDULE_GET + " @OrganizationId=?, @CompanyId=?, @Activity=?",
                organizationId, companyId, "GenerateSortNo");
        return rows.isEmpty() ? 0 : asInt(rows.get(0).get("SortNo"));
    }

    /**
     * InvProcessAnalysisStepSchedule.BindProcessStepName (0396 bll:95-129): @Id only when non-zero,
     * 'BindProcessStepName' -> Id, ProcessStepName of LabInProcessAnalysisStep.
     */
    public List<Map<String, Object>> processStepNames(int organizationId, int companyId, int id) {
        if (id != 0) {
            return jdbc.queryForList("EXEC " + P_SCHEDULE_GET + " @Id=?, @OrganizationId=?, @CompanyId=?, @Activity=?",
                    id, organizationId, companyId, "BindProcessStepName");
        }
        return jdbc.queryForList("EXEC " + P_SCHEDULE_GET + " @OrganizationId=?, @CompanyId=?, @Activity=?",
                organizationId, companyId, "BindProcessStepName");
    }

    /**
     * BLL.Production.InvProductionPlant.GetAll (0336 bll:30-50): Sp_InvProductionPlant_GetAllMethod
     * @OrganizationId, @CompanyId, @Activity 'GetALL'.
     */
    public List<Map<String, Object>> plants(int organizationId, int companyId) {
        return jdbc.queryForList(
                "EXEC dbo.Sp_InvProductionPlant_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                organizationId, companyId, "GetALL");
    }

    /** BLL.Lab.InvLabAnalysisItems.GetAllOrById (0402 bll:29-57): @Id only when non-zero, 'ReadAll'. */
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
     * InvProcessAnalysisStepSchedule.Save (0396 bll:15-29) -> DAL SetDate (0354) -> GenericProvider.SetProc:
     * every property of Model.Lab.InvProcessAnalysisStepSchedule (0612) is a parameter - EnteryDate,
     * AnalysisParameterId, CompanyId, EnteryUserId, Id, LabInProcessAnalysisStepId, OrganizationId,
     * SortNo, PlantId. Id == 0 -> ..._Insert (SELECT @Id), else ..._Update (no rows).
     */
    public Integer saveSchedule(int id, int plantId, int stepId, int parameterId, int sortNo,
                                int organizationId, int companyId, int entryUserId, Timestamp entryDate) {
        String proc = id == 0 ? "Sp_LabInProcessAnalysisStepAndParameterSchedule_Insert"
                              : "Sp_LabInProcessAnalysisStepAndParameterSchedule_Update";
        Integer scalar = ProcExec.call(jdbc,
                "EXEC dbo." + proc + " @EnteryDate=?, @AnalysisParameterId=?, @CompanyId=?, @EnteryUserId=?, @Id=?, "
                        + "@LabInProcessAnalysisStepId=?, @OrganizationId=?, @SortNo=?, @PlantId=?",
                entryDate, parameterId, companyId, entryUserId, id, stepId, organizationId, sortNo, plantId);
        return scalar != null && scalar > 0 ? scalar : Integer.valueOf(id);      // DAL 0354: num > 0 ? num : obj.Id
    }

    // ------------------------------------------------------------------ process step (BLL 0395)

    /** ProcessStep.FormHistory (0395 bll:54-80): @OrganizationId, @CompanyId, 'FormHistory'. */
    public List<Map<String, Object>> processStepHistory(int organizationId, int companyId) {
        return jdbc.queryForList("EXEC " + P_STEP_GET + " @OrganizationId=?, @CompanyId=?, @Activity=?",
                organizationId, companyId, "FormHistory");
    }

    /** ProcessStep.GenerateCode (0395 bll:82-108): 'GenerateSortNo', Rows[0]["SortNo"]. */
    public int processStepSortNo(int organizationId, int companyId) {
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC " + P_STEP_GET + " @OrganizationId=?, @CompanyId=?, @Activity=?",
                organizationId, companyId, "GenerateSortNo");
        return rows.isEmpty() ? 0 : asInt(rows.get(0).get("SortNo"));
    }

    /** ProcessStep.GetByID (0395 bll:30-52): @Id, @Activity 'ReadById'. */
    public List<Map<String, Object>> processStepById(int id) {
        return jdbc.queryForList("EXEC " + P_STEP_GET + " @Id=?, @Activity=?", id, "ReadById");
    }

    /**
     * ProcessStep.Save (0395 bll:14-28) -> DAL SetData (0352) -> GenericProvider.SetProc: every property
     * of Model.Inventory.ProcessStep (0988) - EntryDate, CompanyId, EntryUserId, Id, OrganizationId,
     * SortNo, ProcessStepName. Id == 0 -> Sp_LabInProcessAnalysisStep_Insert (SELECT SCOPE_IDENTITY()),
     * else Sp_LabInProcessAnalysisStep_Update (no rows -> 0).
     */
    public Integer saveProcessStep(int id, String processStepName, int sortNo, int organizationId, int companyId,
                                   int entryUserId, Timestamp entryDate) {
        String proc = id == 0 ? "Sp_LabInProcessAnalysisStep_Insert" : "Sp_LabInProcessAnalysisStep_Update";
        return ProcExec.call(jdbc,
                "EXEC dbo." + proc + " @EntryDate=?, @CompanyId=?, @EntryUserId=?, @Id=?, @OrganizationId=?, "
                        + "@SortNo=?, @ProcessStepName=?",
                entryDate, companyId, entryUserId, id, organizationId, sortNo, processStepName);
    }

    private static int asInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null) return 0;
        try { return Integer.parseInt(String.valueOf(o).trim()); }
        catch (NumberFormatException e) { return 0; }
    }
}
