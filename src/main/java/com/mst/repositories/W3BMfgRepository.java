package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * W3B - Mfg Production hub (module 103), desktop namespace Architecture.WinApp.Mfg. The [Mfg] schema procedures the
 * small master screens call (BLL: Architecture.BLL.Mfg.*; the BLL assembly is not in the recovered source, so each BLL
 * call is mapped to the procedure of the same name in procdure_index.csv):
 *
 * <pre>
 *   770 ProductionOperation       [Mfg].[USP_ProductionOperation_GetAllMethod] 'FormHistory' | 'ReadById'
 *                                 [Mfg].[USP_ProductionOperation_Insert] | _Update
 *   771 ProductionWorkStation     [Mfg].[USP_ProductionWorkStation_GetAllMethod] 'FormHistory' | 'ReadById'
 *                                 [Mfg].[USP_ProductionWorkStation_Insert] | _Update ; dbo.Sp_Department_GetAllMethod 'ReadAll'
 *   595 WorkStationOperation      [Mfg].[USP_WorkStationOperation_GetAllMethod] 'FormHistory' | 'ReadById'
 *                                 [Mfg].[USP_WorkStationOperation_Insert] | _Update
 *   773 frmProductionRouting      [Mfg].[USP_ProductionRoutingName_GetAllMethod] 'FormHistory',
 *                                 [Mfg].[USP_GetDataForDropDownFromProductionRouting] 'RoutingName',
 *                                 [Mfg].[USP_WorkStationOperation_GetAllMethod] 'GetWorkStationOpertaionByWSOId',
 *                                 [Mfg].[USP_ProductionRouting_GetAllMethod] 'ReadByRoutingId' | 'FormHistory' | 'DeleteByRoutingNameId',
 *                                 [Mfg].[USP_ProductionRouting_InsertAndUpdate]
 * </pre>
 */
@Repository
public class W3BMfgRepository {

    private final JdbcTemplate jdbc;
    public W3BMfgRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public List<Map<String, Object>> rows(String proc, Map<String, Object> p) { return DesktopProc.rows(jdbc, proc, p); }
    public int setProc(String proc, Map<String, Object> p) { return DesktopProc.setProc(jdbc, proc, p); }

    /** BLL 0067 Department.GetAll(org, comp) -> Sp_Department_GetAllMethod 'ReadAll'. */
    public List<Map<String, Object>> departments(UserAccount u) {
        return rows("Sp_Department_GetAllMethod", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    // ------------------------------------------------------------------------------ 770

    public List<Map<String, Object>> operationHistory(UserAccount u) {
        return rows("[Mfg].[USP_ProductionOperation_GetAllMethod]", params("OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "BranchesId", u.getBranchesId(), "Activity", "FormHistory"));
    }

    public Map<String, Object> operationById(int id) {
        List<Map<String, Object>> r = rows("[Mfg].[USP_ProductionOperation_GetAllMethod]", params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    public int operationSave(Map<String, Object> p, boolean update) {
        return setProc(update ? "[Mfg].[USP_ProductionOperation_Update]" : "[Mfg].[USP_ProductionOperation_Insert]", p);
    }

    // ------------------------------------------------------------------------------ 771

    public List<Map<String, Object>> workStationHistory(UserAccount u) {
        return rows("[Mfg].[USP_ProductionWorkStation_GetAllMethod]", params("OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "BranchesId", u.getBranchesId(), "Activity", "FormHistory"));
    }

    public Map<String, Object> workStationById(int id) {
        List<Map<String, Object>> r = rows("[Mfg].[USP_ProductionWorkStation_GetAllMethod]", params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    public int workStationSave(Map<String, Object> p, boolean update) {
        return setProc(update ? "[Mfg].[USP_ProductionWorkStation_Update]" : "[Mfg].[USP_ProductionWorkStation_Insert]", p);
    }

    // ------------------------------------------------------------------------------ 595

    public List<Map<String, Object>> wsoHistory(UserAccount u) {
        return rows("[Mfg].[USP_WorkStationOperation_GetAllMethod]", params("OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "BranchesId", u.getBranchesId(), "Activity", "FormHistory"));
    }

    public int wsoSave(Map<String, Object> p, boolean update) {
        return setProc(update ? "[Mfg].[USP_WorkStationOperation_Update]" : "[Mfg].[USP_WorkStationOperation_Insert]", p);
    }

    /** WorkStationOperation.GetWorkStationOpertaionByWSOId(org, comp, id) - @Id sent only when non-zero. */
    public List<Map<String, Object>> wsoByWsoId(UserAccount u, int id) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (id != 0) p.put("Id", id);
        p.put("Activity", "GetWorkStationOpertaionByWSOId");
        return rows("[Mfg].[USP_WorkStationOperation_GetAllMethod]", p);
    }

    // ------------------------------------------------------------------------------ 773

    public List<Map<String, Object>> routingNames(UserAccount u) {
        return rows("[Mfg].[USP_ProductionRoutingName_GetAllMethod]", params("OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "Activity", "FormHistory"));
    }

    public List<Map<String, Object>> routingNamesInUse(UserAccount u) {
        return rows("[Mfg].[USP_GetDataForDropDownFromProductionRouting]", params("OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "Activity", "RoutingName"));
    }

    public List<Map<String, Object>> routingByNameId(int routingNameId) {
        return rows("[Mfg].[USP_ProductionRouting_GetAllMethod]", params("Id", routingNameId, "Activity", "ReadByRoutingId"));
    }

    public List<Map<String, Object>> routingHistory(UserAccount u, int routingNameId, boolean canViewAll, int entryUserId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", u.getBranchesId());
        if (routingNameId != 0) p.put("Id", routingNameId);
        p.put("CanViewAllRecord", canViewAll ? 1 : 0);
        p.put("EntryUserId", entryUserId);
        p.put("Activity", "FormHistory");
        return rows("[Mfg].[USP_ProductionRouting_GetAllMethod]", p);
    }

    public void routingDeleteByName(UserAccount u, int userId, int routingNameId) {
        rows("[Mfg].[USP_ProductionRouting_GetAllMethod]", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Id", routingNameId, "EntryUserId", userId, "Activity", "DeleteByRoutingNameId"));
    }

    public int routingRowSave(Map<String, Object> p) { return setProc("[Mfg].[USP_ProductionRouting_InsertAndUpdate]", p); }
}
