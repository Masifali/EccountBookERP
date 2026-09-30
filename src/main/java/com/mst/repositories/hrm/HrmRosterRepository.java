package com.mst.repositories.hrm;

import com.mst.models.UserAccount;
import com.mst.models.hrm.roster.DutyRoasterGroups;
import com.mst.models.hrm.roster.GenDutyRoaster;
import com.mst.models.hrm.roster.GenDutyRoasterDetail;
import org.springframework.jdbc.core.PreparedStatementCallback;
import org.springframework.stereotype.Repository;

import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * DAL of the HRM duty-roster screens (AppModules 2022 "Attendance Management"):
 *
 *   656 frmDutyRoasterNew   657 frmEmployeeRoaster
 *
 * Every call is the desktop BLL/DAL's own procedure with the parameters it sends
 * (architecture.bll 0216 genDutyRoaster, architecture.dal 0186 genDutyRoaster, the ProfileManagement /
 * EmployeeManagement BLLs for the combos). Nothing is created or changed in the database.
 */
@Repository
public class HrmRosterRepository {

    private final HrmProcRepository db;

    public HrmRosterRepository(HrmProcRepository db) { this.db = db; }

    // ------------------------------------------------------------------ combos

    /** genEmployeeCategory.Getall: Sp_genEmployeeCategory_GetAllMethod @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> employeeCategories(UserAccount u) {
        return db.rows("Sp_genEmployeeCategory_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** genDepartment.Getall: Sp_genDepartment_GetAllMethod @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> departments(UserAccount u) {
        return db.rows("Sp_genDepartment_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** genSection.Getall: Sp_genSection_GetAllMethod @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> sections(UserAccount u) {
        return db.rows("Sp_genSection_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** genShift.Getall: Sp_genShift_GetAllMethod @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> shifts(UserAccount u) {
        return db.rows("Sp_genShift_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** genLocation.Getall: Sp_genLocation_GetAllMethod @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> locations(UserAccount u) {
        return db.rows("Sp_genLocation_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /**
     * WeekDaysFill -> genProfile.GetByProfileTypeId (ProfileTypeId 60, IsActive true - the BLL does not send
     * @IsActive): Sp_genProfile_GetAllMethod @OrganizationId, @CompanyId, @ProfileTypeId 60, @Activity 'ReadByProfileTypeId'.
     */
    public List<Map<String, Object>> weekProfiles(UserAccount u) {
        return db.rows("Sp_genProfile_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ProfileTypeId", 60, "Activity", "ReadByProfileTypeId");
    }

    /**
     * frmDutyRoasterNew.EmployeeName -> genEmployeeHistory.GetAllEmployeesActive(ReportsParameters):
     * Sp_genEmployeeHistory_GetAllMethod @OrganizationId, @CompanyId, [@DepartmentIds], [@SectionId],
     * [@LocationId = PartyLocationId], @Activity 'ReadAllActiveEmployee'. (The form also fills
     * EmployeeCategoryId, but this BLL method never sends it.)
     */
    public List<Map<String, Object>> activeEmployees(UserAccount u, int sectionId, int locationId, String departmentIds) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        if (departmentIds != null && !departmentIds.isEmpty()) p.put("DepartmentIds", departmentIds);
        if (sectionId != 0) p.put("SectionId", sectionId);
        if (locationId != 0) p.put("LocationId", locationId);
        p.put("Activity", "ReadAllActiveEmployee");
        return db.rows("Sp_genEmployeeHistory_GetAllMethod", p);
    }

    /**
     * frmEmployeeRoaster.EmployeeName -> genEmployeeHistory.GetAllActiveEmployee(genEmployeeHistory):
     * Sp_genEmployeeHistory_GetAllMethod @OrganizationId, @CompanyId, @Activity 'ReadAllActiveEmployee'.
     */
    public List<Map<String, Object>> allActiveEmployees(UserAccount u) {
        return db.rows("Sp_genEmployeeHistory_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Activity", "ReadAllActiveEmployee");
    }

    // ------------------------------------------------------------------ genDutyRoaster reads

    /** Sp_genDutyRoaster_GetAllMethod @Activity 'ReadAll', @OrganizationId, @CompanyId - the company's rosters (tenancy guard). */
    public List<Map<String, Object>> rosters(UserAccount u) {
        return db.rows("Sp_genDutyRoaster_GetAllMethod", "Activity", "ReadAll", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    /** genDutyRoaster.GetByID: Sp_genDutyRoaster_GetAllMethod @DutyRoasterId, @Activity 'ReadById' (GetProc, [0]). */
    public List<Map<String, Object>> roster(long id) {
        return db.rows("Sp_genDutyRoaster_GetAllMethod", "DutyRoasterId", id, "Activity", "ReadById");
    }

    /**
     * DAL genDutyRoaster.GetAll's child read, verbatim: @Id = DutyRoasterId, @Activity 'ReadDutyRoasterGroupById'.
     * (The procedure filters that activity on @DutyRoasterId, which the DAL never sends, so on the real
     * database this returns no row - the desktop's DutyRoasterGroupsList is always empty.)
     */
    public List<Map<String, Object>> rosterGroups(long id) {
        return db.rows("Sp_genDutyRoaster_GetAllMethod", "Id", id, "Activity", "ReadDutyRoasterGroupById");
    }

    /** genDutyRoaster.GetHistoryHeaderData: Sp_genDutyRoaster_GetAllMethod @OrganizationId, @CompanyId, @Activity 'DutyRoasterFormHistoryHeader'. */
    public List<Map<String, Object>> historyHeader(UserAccount u) {
        return db.rows("Sp_genDutyRoaster_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Activity", "DutyRoasterFormHistoryHeader");
    }

    /** genDutyRoaster.DutyRoaster_FormHistory: [dbo].[USP_DutyRoaster_FormHistory] with the parameter list the BLL builds. */
    public List<Map<String, Object>> formHistory(Map<String, Object> params) {
        return db.rows("[dbo].[USP_DutyRoaster_FormHistory]", params);
    }

    /** genDutyRoaster.GetHistoryDetailData: Sp_genDutyRoaster_GetAllMethod @Id, @Activity 'DutyRoasterFormHistoryDetail'. */
    public List<Map<String, Object>> historyDetail(int id) {
        return db.rows("Sp_genDutyRoaster_GetAllMethod", "Id", id, "Activity", "DutyRoasterFormHistoryDetail");
    }

    /** genDutyRoaster.GetoffdaysfromDutyRoasterDetailByHeaderId: @DutyRoasterId, @Activity 'GetoffdaysfromDutyRoasterDetailByHeaderId'. */
    public List<Map<String, Object>> offDays(long dutyRoasterId) {
        return db.rows("Sp_genDutyRoaster_GetAllMethod", "DutyRoasterId", dutyRoasterId, "Activity", "GetoffdaysfromDutyRoasterDetailByHeaderId");
    }

    /** genDutyRoaster.GetEmployeesByDutyRosterId: @DutyRoasterId, @Activity 'GetEmployeesByDutyRosterId'. */
    public List<Map<String, Object>> employeesByRoster(int dutyRoasterId) {
        return db.rows("Sp_genDutyRoaster_GetAllMethod", "DutyRoasterId", dutyRoasterId, "Activity", "GetEmployeesByDutyRosterId");
    }

    /**
     * genDutyRoaster.GetDutyDatesForDutyRoaster: GenericProvider.GetDataSetProc("SP_GetDutyDatesForDutyRoaster")
     * with @OrganizationId, @CompanyId, @FromDate, @ToDate and, when not zero / empty, @DutyRoasterId,
     * @DepartmentIds, @SectionId, @EmployeeCategoryId, @EmployeeId, @LocationId (= PartyLocationId).
     * Every result set is returned (Tables[0] dates, [1] employee x shift x date, [2] employee grid, [3] groups).
     */
    public List<Table> dutyDates(Map<String, Object> params) {
        return dataSet("SP_GetDutyDatesForDutyRoaster", params);
    }

    // ------------------------------------------------------------------ genDutyRoaster.Save (DAL SetData)

    /** SetProc(sqlTrn, obj, "Sp_genDutyRoaster_Insert" | "Sp_genDutyRoaster_Update") - Convert.ToInt64(ExecuteScalar). */
    public long saveHeader(GenDutyRoaster m, boolean insert) {
        return db.set(insert ? "Sp_genDutyRoaster_Insert" : "Sp_genDutyRoaster_Update", m);
    }

    /** SetProc(sqlTrn, genDutyRoasterDetails, "Sp_genDutyRoasterDetail_Insert") - every detail row, insert and edit alike. */
    public long saveDetail(GenDutyRoasterDetail d) {
        return db.set("Sp_genDutyRoasterDetail_Insert", d);
    }

    /** SetProc(sqlTrn, dutyRoasterGroups, "Sp_DutyRoasterGroups_Insert"). */
    public long saveGroup(DutyRoasterGroups g) {
        return db.set("Sp_DutyRoasterGroups_Insert", g);
    }

    /** One SqlTransaction around header + details + groups (DAL SetData's BeginTransaction ... Commit / Rollback). */
    public <T> T tx(java.util.function.Supplier<T> work) { return db.tx(work); }

    /**
     * After the commit the DAL runs, on their own connections, CommonProvider.ExecuteProcedure
     * "[dbo].[Sp_SysUpdateAttendanceFromPull]" and "[dbo].[Sp_SysUpdateAttendanceStatus]" with
     * @OrganizationId, @CompanyId (ExecuteScalar, result ignored).
     */
    public void updateAttendance(UserAccount u) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        db.scalar("[dbo].[Sp_SysUpdateAttendanceFromPull]", p);
        db.scalar("[dbo].[Sp_SysUpdateAttendanceStatus]", new LinkedHashMap<>(p));
    }

    // ------------------------------------------------------------------ GetDataSetProc

    /** A DataTable of a DataSet: column names in the procedure's order and the raw rows. */
    public static final class Table {
        public final List<String> cols = new ArrayList<>();
        public final List<Object[]> rows = new ArrayList<>();

        public int idx(String name) {
            for (int i = 0; i < cols.size(); i++) if (cols.get(i).equalsIgnoreCase(name)) return i;
            return -1;
        }

        public Object get(Object[] row, String name) {
            int i = idx(name);
            return i < 0 ? null : row[i];
        }

        /** Row as an ordered map (column order kept, for grids built with RetrieveStructure). */
        public Map<String, Object> map(Object[] row) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (int i = 0; i < cols.size(); i++) m.put(cols.get(i), row[i]);
            return m;
        }
    }

    /**
     * GenericProvider.GetDataSetProc: SqlDataAdapter.Fill(DataSet) - every result set the procedure
     * returns, in order. Null parameters are left out (AddWithValue(null)), as DesktopProc does.
     */
    private List<Table> dataSet(String proc, Map<String, Object> params) {
        List<Object> args = new ArrayList<>();
        StringBuilder sb = new StringBuilder("EXEC ").append(proc.startsWith("[") ? proc : "dbo." + proc);
        boolean first = true;
        for (Map.Entry<String, Object> e : params.entrySet()) {
            if (e.getValue() == null) continue;
            String k = e.getKey().startsWith("@") ? e.getKey().substring(1) : e.getKey();
            sb.append(first ? " " : ", ").append('@').append(k).append("=?");
            args.add(e.getValue());
            first = false;
        }
        return db.jdbc().execute(sb.toString(), (PreparedStatementCallback<List<Table>>) ps -> {
            for (int i = 0; i < args.size(); i++) ps.setObject(i + 1, args.get(i));
            List<Table> out = new ArrayList<>();
            boolean isRs = ps.execute();
            while (true) {
                if (isRs) {
                    try (ResultSet rs = ps.getResultSet()) {
                        ResultSetMetaData md = rs.getMetaData();
                        int n = md.getColumnCount();
                        Table t = new Table();
                        for (int c = 1; c <= n; c++) {
                            String label = md.getColumnLabel(c);
                            t.cols.add(label == null || label.isEmpty() ? "Column" + c : label);
                        }
                        while (rs.next()) {
                            Object[] r = new Object[n];
                            for (int c = 1; c <= n; c++) r[c - 1] = rs.getObject(c);
                            t.rows.add(r);
                        }
                        out.add(t);
                    }
                } else if (ps.getUpdateCount() == -1) {
                    break;
                }
                isRs = ps.getMoreResults();
            }
            return out;
        });
    }
}
