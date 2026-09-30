package com.mst.repositories.hrm;

import com.mst.models.UserAccount;
import com.mst.models.hrm.overtime.EmployeeOverTime;
import com.mst.models.hrm.overtime.OverTimeRequest;
import com.mst.models.hrm.overtime.OverTimeRequestDetail;
import org.springframework.stereotype.Repository;

import java.time.LocalDateTime;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * DAL of the HRM "Over Time Management" screens (AppModules 2025),
 * Architecture.BLL/DAL.HRM.OverTimeManagement.* and the lookups the two forms call.
 * Every call is the desktop's own procedure with the parameters its BLL sends (the BLL's
 * "if (x != 0)" / CheckDateTimeNull guards are kept: an unset value is not sent).
 *
 * hrm-schema procedures are passed as [hrm].[name] - DesktopProc prefixes a bare name with dbo.
 *
 *   665 OverTimeRequest            BLL 0190 / DAL 0156   [hrm].[Sp_OverTimeRequest_GetAllMethod] ReadAll|ReadById|ReadDetailByHeaderId,
 *                                                        [hrm].[Sp_OverTimeRequest_Insert|Update] (20), [hrm].[Sp_OverTimeRequestDetail_Insert] (16),
 *                                                        [hrm].[USP_OverTimeRequest_SlipAndRegister]
 *       EmpOverTimeLoadForRequest  BLL 0189              dbo.usp_ActualOverTimeLoaderForRequest, Sp_genEmployeeHistory_GetAllMethod ReadAllActiveEmployee
 *   666 frmEmployeeOverTime        BLL 0189 / DAL 0155   [hrm].[usp_getOTEmployees], [hrm].[usp_GetEmployeeAttendanceMonthWise],
 *                                                        [hrm].[Sp_EmployeeOverTime_Insert] (25), [hrm].[Sp_EmployeeOverTime_GetAllMethod] ReadAll,
 *                                                        dbo.USP_GetDataForDropDownFromEmployeeOverTime, [hrm].[USP_EmployeeOverTime_SlipAndRegister]
 */
@Repository
public class HrmOvertimeRepository {

    private final HrmProcRepository db;

    public HrmOvertimeRepository(HrmProcRepository db) { this.db = db; }

    public HrmProcRepository db() { return db; }

    // ------------------------------------------------------------------ lookups (DropDown fills)

    /** genDepartment.Getall: Sp_genDepartment_GetAllMethod @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> departments(UserAccount u) {
        return db.rows("Sp_genDepartment_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** genSection.Getall: Sp_genSection_GetAllMethod @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> sections(UserAccount u) {
        return db.rows("Sp_genSection_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** genEmployee.Getall(ReportsParameters): Sp_genEmployee_GetAllMethod @OrganizationId, @CompanyId, @Activity 'ReadAll' (DepartmentId 0 -> not sent). */
    public List<Map<String, Object>> employees(UserAccount u) {
        return db.rows("Sp_genEmployee_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** genEmployeeHistory.GetAllEmployeesActive: Sp_genEmployeeHistory_GetAllMethod @OrganizationId, @CompanyId, [@DepartmentId], @Activity 'ReadAllActiveEmployee'. */
    public List<Map<String, Object>> activeEmployees(UserAccount u, int departmentId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        if (departmentId != 0) p.put("DepartmentId", departmentId);
        p.put("Activity", "ReadAllActiveEmployee");
        return db.rows("Sp_genEmployeeHistory_GetAllMethod", p);
    }

    /**
     * CommonServices.GetConfigurationByOrgCompandConfigDescription -> ConfigrationsAllocation BLL:
     * Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId, @CompanyId, @ConfigDescription,
     * @Activity 'GetConfigurationByOrgCompandConfigDescription'; the value is ConfigKey of the first row ("" when none).
     */
    public String config(UserAccount u, String description) {
        List<Map<String, Object>> rows = db.rows("Sp_ConfigrationsAllocation_GetAllMethod", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription");
        if (rows.isEmpty()) return "";
        Object v = rows.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v);
    }

    /** clsGlobalVariables.ActiveYr (for CommonServices.GetYears): Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId. */
    public List<Map<String, Object>> financialYears(UserAccount u) {
        return db.rows("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    // ------------------------------------------------------------------ 665 OverTimeRequest

    /**
     * OverTimeRequest.ReadAll(ReportsParameters): [hrm].[Sp_OverTimeRequest_GetAllMethod] @OrganizationId,
     * @CompanyId, the date pair of the chosen radio (only when set), [@SectionId], [@DepartmentId],
     * [@RequestById], @Activity 'ReadAll'. {@code filters} holds only the parameters the BLL sends.
     */
    public List<Map<String, Object>> requestHistory(UserAccount u, Map<String, Object> filters) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.putAll(filters);
        p.put("Activity", "ReadAll");
        return db.rows("[hrm].[Sp_OverTimeRequest_GetAllMethod]", p);
    }

    /** OverTimeRequest.GetByID -> DAL GetData: @Activity 'ReadById', @OverTimeRequestId. */
    public List<Map<String, Object>> request(long id) {
        return db.rows("[hrm].[Sp_OverTimeRequest_GetAllMethod]", "Activity", "ReadById", "OverTimeRequestId", id);
    }

    /** DAL GetData's second call per header: @OverTimeRequestId, @Activity 'ReadDetailByHeaderId' (GetAllDetail). */
    public List<Map<String, Object>> requestDetails(long id) {
        return db.rows("[hrm].[Sp_OverTimeRequest_GetAllMethod]", "OverTimeRequestId", id, "Activity", "ReadDetailByHeaderId");
    }

    /**
     * BLL OverTimeRequest.Save -> DAL SetData, one SqlTransaction: the header through
     * Sp_OverTimeRequest_Insert (OverTimeRequestId 0, ActionTypeId 1) or Sp_OverTimeRequest_Update
     * (ActionTypeId 2); the Insert's SELECT is the new id, the Update returns none and the DAL keeps
     * obj.OverTimeRequestId. Then every OverTimeRequestDetailList entry (deleted rows first, then the
     * grid rows, as the form builds the list) through Sp_OverTimeRequestDetail_Insert with the header id.
     */
    public long saveRequest(OverTimeRequest h, List<OverTimeRequestDetail> details) {
        return db.tx(() -> {
            boolean insert = h.OverTimeRequestId == 0L;
            h.ActionTypeId = insert ? 1 : 2;
            long n = db.set(insert ? "[hrm].[Sp_OverTimeRequest_Insert]" : "[hrm].[Sp_OverTimeRequest_Update]", h);
            if (n > 0) h.OverTimeRequestId = n; else n = h.OverTimeRequestId;
            for (OverTimeRequestDetail d : details) {
                d.OverTimeRequestId = h.OverTimeRequestId;
                db.set("[hrm].[Sp_OverTimeRequestDetail_Insert]", d);
            }
            return n;
        });
    }

    /** CommonServices.OverTimeSlip -> OverTimeRequest.OTSlipandRegister: [hrm].[USP_OverTimeRequest_SlipAndRegister] @OrganizationId, @CompanyId, @Id. */
    public List<Map<String, Object>> requestSlip(UserAccount u, long id) {
        return db.rows("[hrm].[USP_OverTimeRequest_SlipAndRegister]", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", id);
    }

    /**
     * EmployeeOverTime.ActualOverTimeLoaderForRequest: [dbo].[usp_ActualOverTimeLoaderForRequest]
     * @OrganizationId, @CompanyId, @FromDate, @ToDate, [@EmployeeId], [@DepartmentId].
     */
    public List<Map<String, Object>> overtimeLoader(UserAccount u, LocalDateTime from, LocalDateTime to, long employeeId, long departmentId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("FromDate", java.sql.Timestamp.valueOf(from));
        p.put("ToDate", java.sql.Timestamp.valueOf(to));
        if (employeeId != 0) p.put("EmployeeId", employeeId);
        if (departmentId != 0) p.put("DepartmentId", departmentId);
        return db.rows("usp_ActualOverTimeLoaderForRequest", p);
    }

    // ------------------------------------------------------------------ 666 frmEmployeeOverTime

    /** EmployeeOverTime.getOTEmployees: [hrm].[usp_getOTEmployees] @OrganizationId, @CompanyId, @Month, @Year (always sent). */
    public List<Map<String, Object>> otEmployees(UserAccount u, int month, int year) {
        return db.rows("[hrm].[usp_getOTEmployees]", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Month", month, "Year", year);
    }

    /** EmployeeOverTime.GetEmployeeAttendanceMonthWise: [hrm].[usp_GetEmployeeAttendanceMonthWise] @OrganizationId, @CompanyId, [@EmployeeId], [@Month], [@Year]. */
    public List<Map<String, Object>> attendanceMonthWise(UserAccount u, long employeeId, int month, int year) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        if (employeeId != 0) p.put("EmployeeId", employeeId);
        if (month != 0) p.put("Month", month);
        if (year != 0) p.put("Year", year);
        return db.rows("[hrm].[usp_GetEmployeeAttendanceMonthWise]", p);
    }

    /**
     * BLL EmployeeOverTime.Save(obj): obj is a fresh wrapper, EmployeeOverTimeId 0, so the BLL always
     * picks [hrm].[Sp_EmployeeOverTime_Insert]; DAL SetData runs every EmployeeOverTimeList entry in
     * one SqlTransaction and returns the last Convert.ToInt32(SetProc).
     */
    public long saveEmployeeOverTime(List<EmployeeOverTime> list) {
        return db.tx(() -> {
            long r = 0;
            for (EmployeeOverTime m : list) r = db.set("[hrm].[Sp_EmployeeOverTime_Insert]", m);
            return r;
        });
    }

    /** EmployeeOverTime.FormHistory: [hrm].[Sp_EmployeeOverTime_GetAllMethod] @OrganizationId, @CompanyId, [@Month], [@Year], [@EmployeeId], @Activity 'ReadAll'. */
    public List<Map<String, Object>> employeeOverTimeHistory(UserAccount u, int month, int year, long employeeId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        if (month != 0) p.put("Month", month);
        if (year != 0) p.put("Year", year);
        if (employeeId != 0) p.put("EmployeeId", employeeId);
        p.put("Activity", "ReadAll");
        return db.rows("[hrm].[Sp_EmployeeOverTime_GetAllMethod]", p);
    }

    /** EmployeeOverTime.GetDataForDropDown(org, comp, null): dbo.USP_GetDataForDropDownFromEmployeeOverTime @OrganizationId, @CompanyId (no @Activity). */
    public List<Map<String, Object>> employeeOverTimeDropDowns(UserAccount u) {
        return db.rows("USP_GetDataForDropDownFromEmployeeOverTime", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    /**
     * EmployeeOverTime.EmployeeOverTime_SlipAndRegister: [hrm].[USP_EmployeeOverTime_SlipAndRegister]
     * @OrganizationId, @CompanyId, @Month, @Year, [@EmployeeId] (the form's value is always 0, see the service).
     */
    public List<Map<String, Object>> employeeOverTimeSlip(UserAccount u, int month, int year, long employeeId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("Month", month);
        p.put("Year", year);
        if (employeeId != 0) p.put("EmployeeId", employeeId);
        return db.rows("[hrm].[USP_EmployeeOverTime_SlipAndRegister]", p);
    }
}
