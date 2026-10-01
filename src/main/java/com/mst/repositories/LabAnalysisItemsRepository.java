package com.mst.repositories;

import com.mst.repositories.support.ProcExec;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Map;

/**
 * Screen 156 "Item Analysis Parameter" — desktop Architecture.WinApp.Lab/InvLabAnalysisItems.cs,
 * BLL 0402_Architecture.BLL.Lab.InvLabAnalysisItems.cs, DAL 0358, Model 0618.
 *
 * Every statement is one the desktop form issues, with the parameters the BLL sends:
 *
 *   Sp_InvLabAnalysisItems_GetAllMethod  [@Id], @OrganizationId, @CompanyId, @Activity='ReadAll'
 *        gridfill (InvLabAnalysisItems.cs:214-251) and grdfrm_DoubleClick (:273-303) -> BLL
 *        GetAllOrById (0402:29-63). @Id is added only when != 0 (0402:34): the ReadAll branch filters
 *        "(@Id is null or d.Id=@Id)", so sending 0 would return nothing. @Activity has no default
 *        in the procedure and is always sent.
 *   usp_getLabMasterParms                 no parameters — MasterParameters (:305-319), BLL 0402:65-76.
 *        (The 'BindMasterParameter' branch of GetAllMethod reads a differently named table,
 *        InvLabMasterParamenter, and is NOT what the form calls.)
 *   Sp_InvLabAnalysisItems_Insert / _Update
 *        BLL Save (0402:13-27) -> DAL SetDate (0358:10-34) -> GenericProvider.SetProc (0207:283-311),
 *        which sends ONE PARAMETER PER MODEL PROPERTY, named after the property, in declaration
 *        order (Model 0618): CompanyId, Id, OrganizationId, ParentParameterId, MasterParId,
 *        AnalysisParameterCode, AnalysisParameterDescription, IsSub, MinValue, MaxValue.
 *        So the insert receives @Id=0 as well; the procedure overwrites it with Max(Id)+1 and ends in
 *        SELECT @Id. The update returns no result set. ProcExec tolerates both shapes.
 */
@Repository
public class LabAnalysisItemsRepository {

    private static final String SQL_READ =
            "EXEC dbo.Sp_InvLabAnalysisItems_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?";
    private static final String SQL_READ_BY_ID =
            "EXEC dbo.Sp_InvLabAnalysisItems_GetAllMethod @Id=?, @OrganizationId=?, @CompanyId=?, @Activity=?";

    private static final String PARAMS =
            "@CompanyId=?, @Id=?, @OrganizationId=?, @ParentParameterId=?, @MasterParId=?, "
          + "@AnalysisParameterCode=?, @AnalysisParameterDescription=?, @IsSub=?, @MinValue=?, @MaxValue=?";
    private static final String SQL_INSERT = "EXEC dbo.Sp_InvLabAnalysisItems_Insert " + PARAMS;
    private static final String SQL_UPDATE = "EXEC dbo.Sp_InvLabAnalysisItems_Update " + PARAMS;

    private final JdbcTemplate jdbc;

    public LabAnalysisItemsRepository(JdbcTemplate jdbc) {
        this.jdbc = jdbc;
    }

    /** gridfill() — GetAllOrById with Id left at 0, so @Id is omitted. */
    public List<Map<String, Object>> readAll(int organizationId, int companyId) {
        return jdbc.queryForList(SQL_READ, organizationId, companyId, "ReadAll");
    }

    /** grdfrm_DoubleClick — the same Activity, with @Id supplied. */
    public List<Map<String, Object>> readById(int organizationId, int companyId, int id) {
        return jdbc.queryForList(SQL_READ_BY_ID, id, organizationId, companyId, "ReadAll");
    }

    /** MasterParameters() — columns Id, MasterParameterName. */
    public List<Map<String, Object>> masterParameters() {
        return jdbc.queryForList("EXEC dbo.usp_getLabMasterParms");
    }

    public Integer insert(String code, String description, int companyId, int organizationId,
                          int parentParameterId, boolean isSub, int masterParId,
                          double minValue, double maxValue) {
        return ProcExec.call(jdbc, SQL_INSERT,
                companyId, 0, organizationId, parentParameterId, masterParId,
                code, description, isSub, minValue, maxValue);
    }

    public void update(int id, String code, String description, int companyId, int organizationId,
                       int parentParameterId, boolean isSub, int masterParId,
                       double minValue, double maxValue) {
        ProcExec.call(jdbc, SQL_UPDATE,
                companyId, id, organizationId, parentParameterId, masterParId,
                code, description, isSub, minValue, maxValue);
    }
}
