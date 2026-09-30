package com.mst.repositories.hrm;

import com.mst.models.UserAccount;
import com.mst.models.hrm.leave.CPLAttendance;
import com.mst.models.hrm.leave.CPLEmployeeLeaveDetail;
import com.mst.models.hrm.leave.EmployeeLeaveOpening;
import com.mst.models.hrm.leave.EmployeeLeaveQuota;
import com.mst.models.hrm.leave.GenEmployeeLeave;
import com.mst.models.hrm.leave.GenEmployeeLeaveDetail;
import org.springframework.stereotype.Repository;

import java.time.LocalDateTime;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * DAL of the HRM "Leave Management" screens (AppModules 2023 / 47). Every call is the desktop BLL / DAL's own
 * procedure with the parameters it sends (recovered_source/projects/architecture.bll|dal, procdure.utf8.sql).
 *
 *   660 frmEmployeeLeaveOpening        BLL 0193 / DAL 0159, hrmLeaveQuota BLL 0177
 *   661 frmEmployeeLeaveRequest        BLL 0194 / DAL 0160
 *   662/463 frmEmployeeCPLLeaveOpening BLL 0217 / DAL 0187 (CPLAttendance, ReqTypeId 1)
 *   461 frmEmployeeCPLAttendance       BLL 0217 / DAL 0187 (CPLAttendance, ReqTypeId 2)
 *   462 frmEmployeeCPLRequest          BLL 0194 / DAL 0160 (LeaveTypeProfileId 13)
 *
 * The [hrm] schema procedures are written "[hrm].[...]" because DesktopProc prefixes "dbo." to a bare name.
 */
@Repository
public class HrmLeaveRepository {

    private static final String OPENING_GET = "[hrm].[Sp_EmployeeLeaveOpening_GetAllMethod]";

    private final HrmProcRepository db;

    public HrmLeaveRepository(HrmProcRepository db) { this.db = db; }

    // ------------------------------------------------------------------ shared lookups

    /** genEmployee.Getall(ReportsParameters): Sp_genEmployee_GetAllMethod @OrganizationId, @CompanyId, @Activity 'ReadAll' (DepartmentId 0 -> not sent). */
    public List<Map<String, Object>> employees(UserAccount u) {
        return db.rows("Sp_genEmployee_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /**
     * genEmployeeHistory.GetAllActiveEmployee / GetAllEmployeesActive (same call when only Org/Company are set):
     * Sp_genEmployeeHistory_GetAllMethod @OrganizationId, @CompanyId, @Activity 'ReadAllActiveEmployee'.
     */
    public List<Map<String, Object>> activeEmployees(UserAccount u) {
        return db.rows("Sp_genEmployeeHistory_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAllActiveEmployee");
    }

    /** genProfile.GetByProfileTypeId: Sp_genProfile_GetAllMethod @OrganizationId, @CompanyId, @ProfileTypeId 3, @Activity 'ReadByProfileTypeId'. */
    public List<Map<String, Object>> leaveTypes(UserAccount u) {
        return db.rows("Sp_genProfile_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ProfileTypeId", 3, "Activity", "ReadByProfileTypeId");
    }

    /**
     * hrmLeaveQuota.GetDataForLeaveQuotaDropDown: Sp_hrmLeaveQuota_GetAllMethod @OrganizationId, @CompanyId,
     * @FromDate (clsGlobalVariables.ActiveYr.Start_Period), @Activity 'GetDataForLeaveQuotaDropDown'.
     */
    public List<Map<String, Object>> leaveQuotas(UserAccount u, LocalDateTime fromDate) {
        return db.rows("Sp_hrmLeaveQuota_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FromDate", fromDate, "Activity", "GetDataForLeaveQuotaDropDown");
    }

    /** clsGlobalVariables.ActiveYr: the active financial years of the company (Id, Start_Period ...). */
    public List<Map<String, Object>> activeYears(UserAccount u) {
        return db.rows("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    // ------------------------------------------------------------------ 660 Leave Opening

    /**
     * hrmLeaveQuota.GetDataForLeaveOpening: Sp_hrmLeaveQuota_GetAllMethod @OrganizationId, @CompanyId, @LeaveQuotaId,
     * @LeaveTypeProfileId, @EmployeeId (only when != 0), @Activity 'GetDataForLeaveOpening'.
     */
    public List<Map<String, Object>> leaveOpeningRows(UserAccount u, int leaveQuotaId, int leaveTypeProfileId, long employeeId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("LeaveQuotaId", leaveQuotaId);
        p.put("LeaveTypeProfileId", leaveTypeProfileId);
        if (employeeId != 0L) p.put("EmployeeId", employeeId);
        p.put("Activity", "GetDataForLeaveOpening");
        return db.rows("Sp_hrmLeaveQuota_GetAllMethod", p);
    }

    /**
     * BLL EmployeeLeaveOpening.Save -> DAL SetData in one transaction: [hrm].[Sp_EmployeeLeaveOpening_Insert]
     * (ActionTypeId 1) or _Update (ActionTypeId 2), then [hrm].[Sp_EmployeeLeaveQuota_Insert] per row with the
     * header id. Returns the Insert's id, or the header id when the Update returned nothing.
     */
    public long saveLeaveOpening(EmployeeLeaveOpening m) {
        return db.tx(() -> {
            long num;
            if (m.EmployeeLeaveOpeningId == 0L) {
                m.ActionTypeId = 1;
                num = db.set("[hrm].[Sp_EmployeeLeaveOpening_Insert]", m);
            } else {
                m.ActionTypeId = 2;
                num = db.set("[hrm].[Sp_EmployeeLeaveOpening_Update]", m);
            }
            if (num > 0) m.EmployeeLeaveOpeningId = num; else num = m.EmployeeLeaveOpeningId;
            for (EmployeeLeaveQuota d : m.EmployeeLeaveQuotasList) {
                d.EmployeeLeaveOpeningId = m.EmployeeLeaveOpeningId;
                db.set("[hrm].[Sp_EmployeeLeaveQuota_Insert]", d);
            }
            return num;
        });
    }

    // ------------------------------------------------------------------ 661 Leave Request / 462 CPL Request

    /**
     * EmployeeLeaveOpening.GetLeavesBalanceByEmployeeId: [hrm].[Sp_EmployeeLeaveOpening_GetAllMethod] @OrganizationId,
     * @CompanyId, @EmployeeId, @LeaveQuotaId, @LeaveTypeProfileId, @Activity 'GetLeavesBalanceByEmployeeId'.
     */
    public List<Map<String, Object>> leaveBalance(UserAccount u, long employeeId, int leaveQuotaId, int leaveTypeProfileId) {
        return db.rows(OPENING_GET, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "EmployeeId", employeeId,
                "LeaveQuotaId", leaveQuotaId, "LeaveTypeProfileId", leaveTypeProfileId, "Activity", "GetLeavesBalanceByEmployeeId");
    }

    /** genEmployeeLeave.ReadAll: Sp_genEmployeeLeave_ReadAll @OrganizationId, @CompanyId, @LeaveTypeProfileId (only when != 0), @Activity 'ReadAll'. */
    public List<Map<String, Object>> leaves(UserAccount u, int leaveTypeProfileId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        if (leaveTypeProfileId != 0) p.put("LeaveTypeProfileId", leaveTypeProfileId);
        p.put("Activity", "ReadAll");
        return db.rows("Sp_genEmployeeLeave_ReadAll", p);
    }

    /** genEmployeeLeave.GetByID -> DAL GetData: @Activity 'ReadById', @EmployeeLeaveId. */
    public List<Map<String, Object>> leave(long id) {
        return db.rows("Sp_genEmployeeLeave_ReadAll", "Activity", "ReadById", "EmployeeLeaveId", id);
    }

    /** DAL GetData, first child list: @EmployeeLeaveId, @Activity 'ReadDetailByHeaderId'. */
    public List<Map<String, Object>> leaveDetails(long id) {
        return db.rows("Sp_genEmployeeLeave_ReadAll", "EmployeeLeaveId", id, "Activity", "ReadDetailByHeaderId");
    }

    /** DAL GetData, second child list: @EmployeeLeaveId, @Activity 'ReadCPLLeaveDetailByHeaderId'. */
    public List<Map<String, Object>> leaveCplDetails(long id) {
        return db.rows("Sp_genEmployeeLeave_ReadAll", "EmployeeLeaveId", id, "Activity", "ReadCPLLeaveDetailByHeaderId");
    }

    /**
     * BLL genEmployeeLeave.Save -> DAL SetData: in one transaction Sp_genEmployeeLeave_Insert (EmployeeLeaveId 0)
     * or Sp_genEmployeeLeave_Update, every genEmployeeLeaveDetail through Sp_genEmployeeLeaveDetail_Insert and
     * every CPLEmployeeLeaveDetail through Sp_CPLEmployeeLeaveDetail_Insert with the header id; after the commit
     * CommonProvider.ExecuteProcedure [dbo].[Sp_SysUpdateAttendanceFromPull] and [dbo].[Sp_SysUpdateAttendanceStatus]
     * with @OrganizationId, @CompanyId (each on its own connection, outside the transaction, as the DAL does).
     */
    public long saveLeave(GenEmployeeLeave m) {
        long num = db.tx(() -> {
            long n = db.set(m.EmployeeLeaveId == 0L ? "Sp_genEmployeeLeave_Insert" : "Sp_genEmployeeLeave_Update", m);
            if (n > 0) m.EmployeeLeaveId = n; else n = m.EmployeeLeaveId;
            for (GenEmployeeLeaveDetail d : m.genEmployeeLeaveDetailList) {
                d.EmployeeLeaveId = m.EmployeeLeaveId;
                db.set("Sp_genEmployeeLeaveDetail_Insert", d);
            }
            for (CPLEmployeeLeaveDetail c : m.CPLEmployeeLeaveDetailslist) {
                c.EmployeeLeaveId = m.EmployeeLeaveId;
                db.set("Sp_CPLEmployeeLeaveDetail_Insert", c);
            }
            return n;
        });
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", m.OrganizationId);
        p.put("CompanyId", m.CompanyId);
        db.scalar("[dbo].[Sp_SysUpdateAttendanceFromPull]", p);
        db.scalar("[dbo].[Sp_SysUpdateAttendanceStatus]", p);
        return num;
    }

    /**
     * CPLAttendance.GetCPLRecordForEmployeeLeaveRequest: Sp_CPLAttendance_GetAllMethod @OrganizationId, @CompanyId,
     * @EmployeeId, @Activity 'GetCPLRecordForEmployeeLeaveRequest'.
     */
    public List<Map<String, Object>> cplForLeaveRequest(UserAccount u, long employeeId) {
        return db.rows("Sp_CPLAttendance_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "EmployeeId", employeeId, "Activity", "GetCPLRecordForEmployeeLeaveRequest");
    }

    // ------------------------------------------------------------------ 662/463 CPL Opening, 461 CPL Attendance

    /** CPLAttendance.ReadAll: Sp_CPLAttendance_GetAllMethod @OrganizationId, @CompanyId, @ReqTypeId (when != 0), @Activity 'ReadAll'. */
    public List<Map<String, Object>> cplList(UserAccount u, int reqTypeId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        if (reqTypeId != 0) p.put("ReqTypeId", reqTypeId);
        p.put("Activity", "ReadAll");
        return db.rows("Sp_CPLAttendance_GetAllMethod", p);
    }

    /** CPLAttendance.ReadById: @Id, @Activity 'ReadById'. */
    public List<Map<String, Object>> cpl(long id) {
        return db.rows("Sp_CPLAttendance_GetAllMethod", "Id", id, "Activity", "ReadById");
    }

    /**
     * CPLAttendance.GetEmployeeAttendanceRecordForCPLAttendance: @OrganizationId, @CompanyId, @EmployeeId, @Month,
     * @Year, @Activity 'GetEmployeeAttendanceRecordForCPLAttendance'.
     */
    public List<Map<String, Object>> cplAttendanceRecords(UserAccount u, long employeeId, int month, int year) {
        return db.rows("Sp_CPLAttendance_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "EmployeeId", employeeId, "Month", month, "Year", year, "Activity", "GetEmployeeAttendanceRecordForCPLAttendance");
    }

    /** BLL CPLAttendance.Save -> DAL SetData: Sp_CPLAttendance_Insert per row in one transaction; the last row's result. */
    public int saveCpl(List<CPLAttendance> rows) {
        return db.tx(() -> {
            int result = 0;
            for (CPLAttendance r : rows) result = db.set("Sp_CPLAttendance_Insert", r);
            return result;
        });
    }
}
