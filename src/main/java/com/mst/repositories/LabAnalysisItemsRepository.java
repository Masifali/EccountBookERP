package com.mst.repositories;

import com.mst.repositories.support.ProcExec;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Map;

/**
 * Screen 156 - Item Analysis Parameter (desktop <code>InvLabAnalysisItems.cs</code>, module 7 Lab).
 *
 * Every statement here is one the desktop form issues, with the parameters the desktop sends and
 * no others. Three procedures are involved and nothing else touches the table:
 *
 *   Sp_InvLabAnalysisItems_GetAllMethod  - the grid, the by-id read behind the row double-click,
 *                                          and (unused by this form) BindMasterParameter
 *   Sp_InvLabAnalysisItems_Insert        - btnsave
 *   Sp_InvLabAnalysisItems_Update        - btnupdate
 *   usp_getLabMasterParms                - the Master Parameter combo
 *
 * ---------------------------------------------------------------------------------------------
 * TWO THINGS THAT ARE EASY TO GET WRONG HERE
 * ---------------------------------------------------------------------------------------------
 *
 * 1. @Activity has NO DEFAULT. The procedure declares
 *
 *        @OrganizationId int=null, @CompanyId int=null, @Id int=null, @Activity nvarchar(50)
 *
 *    - note the missing "=null" on the last one. Omitting it is not "send NULL", it is a hard
 *      "Procedure expects parameter '@Activity'" failure. It is always sent.
 *
 * 2. @Id is GUARDED, not always-sent. The ReadAll branch filters with
 *
 *        and (@Id is null or d.Id=@Id)
 *
 *    so passing the C# model's default Id of 0 would match no row and the grid would come back
 *    empty. The desktop's gridfill() builds its model with OrganizationId and CompanyId only,
 *    leaving Id at 0, and the BLL omits a zero id - which is what makes ReadAll return everything.
 *    grdfrm_DoubleClick sets Id first and the same Activity then returns the single row. So one
 *    procedure serves both reads, and the ONLY difference is whether @Id is included.
 *
 * ---------------------------------------------------------------------------------------------
 * A MISSPELLED TABLE THAT DOES NOT MATTER HERE, BUT WOULD IF THE COMBO WERE MOVED
 * ---------------------------------------------------------------------------------------------
 * The GetAllMethod's 'BindMasterParameter' branch reads InvLabMasterParamenter (sic), while its
 * own ReadAll branch joins LabMasterParameters, and usp_getLabMasterParms reads
 * LabMasterParameters. Those are two different tables. The desktop's MasterParameters() calls
 * getLabMasterParms(), so this port calls usp_getLabMasterParms too - the one whose ids actually
 * match the MasterParId values ReadAll resolves names from. Switching to the BindMasterParameter
 * branch because it lives in the "same" procedure would silently populate the combo from the
 * wrong table.
 */
@Repository
public class LabAnalysisItemsRepository {

    private static final String SQL_READ =
            "EXEC dbo.Sp_InvLabAnalysisItems_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?";
    private static final String SQL_READ_BY_ID =
            "EXEC dbo.Sp_InvLabAnalysisItems_GetAllMethod @OrganizationId=?, @CompanyId=?, @Id=?, @Activity=?";

    /* Declaration order of the procedure's parameter block. Named parameters make the order
       irrelevant to SQL Server, but keeping it lets the signature be diffed against the dump. */
    private static final String SQL_INSERT =
            "EXEC dbo.Sp_InvLabAnalysisItems_Insert "
          + "@AnalysisParameterCode=?, @AnalysisParameterDescription=?, @CompanyId=?, "
          + "@OrganizationId=?, @ParentParameterId=?, @IsSub=?, @MasterParId=?, "
          + "@MinValue=?, @MaxValue=?";

    private static final String SQL_UPDATE =
            "EXEC dbo.Sp_InvLabAnalysisItems_Update "
          + "@Id=?, @AnalysisParameterCode=?, @AnalysisParameterDescription=?, @CompanyId=?, "
          + "@OrganizationId=?, @ParentParameterId=?, @IsSub=?, @MasterParId=?, "
          + "@MinValue=?, @MaxValue=?";

    private final JdbcTemplate jdbc;

    public LabAnalysisItemsRepository(JdbcTemplate jdbc) {
        this.jdbc = jdbc;
    }

    /** gridfill() - InvLabAnalysisItems.GetAllOrById with Id left at 0, so @Id is omitted. */
    public List<Map<String, Object>> readAll(int organizationId, int companyId) {
        return jdbc.queryForList(SQL_READ, organizationId, companyId, "ReadAll");
    }

    /** grdfrm_DoubleClick - the same Activity, with @Id supplied. */
    public List<Map<String, Object>> readById(int organizationId, int companyId, int id) {
        return jdbc.queryForList(SQL_READ_BY_ID, organizationId, companyId, id, "ReadAll");
    }

    /** MasterParameters() - InvLabAnalysisItems.getLabMasterParms(). The procedure takes none. */
    public List<Map<String, Object>> masterParameters() {
        return jdbc.queryForList("EXEC dbo.usp_getLabMasterParms");
    }

    /**
     * The insert ends in <code>SELECT @Id</code>, so it produces a result set and must not go
     * through JdbcTemplate.update() ("A result set was generated for update"). The id it returns
     * is Max(Id)+1 computed inside the procedure - this table has no identity column.
     */
    public Integer insert(String code, String description, int companyId, int organizationId,
                          int parentParameterId, boolean isSub, int masterParId,
                          double minValue, double maxValue) {
        return ProcExec.call(jdbc, SQL_INSERT,
                code, description, companyId, organizationId,
                parentParameterId, isSub, masterParId, minValue, maxValue);
    }

    /**
     * The update returns nothing at all, which is the opposite hazard - queryForList() would fail
     * with "The statement did not return a result set". ProcExec tolerates both shapes.
     */
    public void update(int id, String code, String description, int companyId, int organizationId,
                       int parentParameterId, boolean isSub, int masterParId,
                       double minValue, double maxValue) {
        ProcExec.call(jdbc, SQL_UPDATE,
                id, code, description, companyId, organizationId,
                parentParameterId, isSub, masterParId, minValue, maxValue);
    }
}
