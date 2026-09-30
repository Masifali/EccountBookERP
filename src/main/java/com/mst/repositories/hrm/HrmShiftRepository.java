package com.mst.repositories.hrm;

import com.mst.models.UserAccount;
import com.mst.models.hrm.shift.GenHoliday;
import com.mst.models.hrm.shift.GenShift;
import com.mst.models.hrm.shift.GenShiftLocation;
import com.mst.models.hrm.shift.GenShiftTiming;
import com.mst.models.hrm.shift.GenShiftTimingWeekDay;
import org.springframework.stereotype.Repository;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * DAL of the HRM shift screens (AppModules 2022). Every call is the desktop DAL's own procedure with the
 * parameters its BLL sends:
 *
 *   652 frmGenShift          BLL 0203 / DAL 0174 genShift          Sp_genShift_GetAllMethod ReadAll|ReadById, Sp_genShift_Insert|Update (14)
 *   653 frmShiftLocation     BLL 0204 / DAL 0175 genShiftLocation  Sp_genShiftLocation_GetAllMethod ReadAll|ReadById, Sp_genShiftLocation_Insert|Update (14)
 *   654 frmGenShiftTiming    BLL 0219 / DAL 0189 genShiftTiming    Sp_genShiftTiming_GetAllMethod ReadAll|ReadById, Sp_genShiftTiming_Insert|Update (42)
 *                                                                  + sP_genShiftTimingWeekDay_Insert (10) per week day, one transaction;
 *                            genProfile.GetByProfileTypeId          Sp_genProfile_GetAllMethod ReadByProfileTypeId (60 = week days)
 *   655 frmHoliday           BLL 0218 / DAL 0188 genHoliday        Sp_genHoliday_GetAllMethod ReadByMonthAndYear|ReadAll, Sp_genHoliday_Insert (14) per row,
 *                                                                  then Sp_SysUpdateAttendanceFromPull / Sp_SysUpdateAttendanceStatus
 *   shared combos            CommonServices.BrancheServiceBind      Sp_Branches_GetAllMethod GetAll
 *                            CommonServices.ProjectServiceBind      Sp_Projects_GetAllMethod @MethodType GetAll
 *                            genLocation.Getall                     Sp_genLocation_GetAllMethod ReadAll
 *                            CommonServices.GetYears                ActiveYr.Start_Period (Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId, the login's proc)
 */
@Repository
public class HrmShiftRepository {

    private final HrmProcRepository db;

    public HrmShiftRepository(HrmProcRepository db) { this.db = db; }

    // ------------------------------------------------------------------ shared combos

    /** Branches.GetAll: @OrganizationId, @CompanyId, @Activity 'GetAll' (columns Id, BranchName ...). */
    public List<Map<String, Object>> branches(UserAccount u) {
        return db.rows("Sp_Branches_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetAll");
    }

    /** Projects.GetAlldt: @OrganizationId, @CompanyId, @MethodType 'GetAll' (columns Id, ProjectName ...). */
    public List<Map<String, Object>> projects(UserAccount u) {
        return db.rows("Sp_Projects_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "MethodType", "GetAll");
    }

    /** genLocation.Getall: @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> locations(UserAccount u) {
        return db.rows("Sp_genLocation_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** The login's financial-year list (Id, Start_Period ...) - clsGlobalVariables.ActiveYr is one of its rows. */
    public List<Map<String, Object>> financialYears(UserAccount u) {
        return db.rows("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    // ------------------------------------------------------------------ 652 genShift

    /** genShift.Getall: @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> shifts(UserAccount u) {
        return db.rows("Sp_genShift_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** genShift.GetByID: @ShiftId, @Activity 'ReadById'. */
    public List<Map<String, Object>> shift(int id) {
        return db.rows("Sp_genShift_GetAllMethod", "ShiftId", id, "Activity", "ReadById");
    }

    /** BLL genShift.Save -> DAL SetData: Insert when ShiftId == 0 (SCOPE_IDENTITY), else Update (no row -> obj.ShiftId). */
    public int saveShift(GenShift m) {
        int n = db.set(m.ShiftId == 0 ? "Sp_genShift_Insert" : "Sp_genShift_Update", m);
        return n > 0 ? n : m.ShiftId;
    }

    // ------------------------------------------------------------------ 653 genShiftLocation

    /**
     * genShiftLocation.Getall: @OrganizationId, @CompanyId, @ShiftId only when != 0, @IsActive only when
     * true, @Activity 'ReadAll'.
     */
    public List<Map<String, Object>> shiftLocations(UserAccount u, int shiftId, boolean isActive) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        if (shiftId != 0) p.put("ShiftId", shiftId);
        if (isActive) p.put("IsActive", true);
        p.put("Activity", "ReadAll");
        return db.rows("Sp_genShiftLocation_GetAllMethod", p);
    }

    /** genShiftLocation.GetByID: @ShiftLocationId, @Activity 'ReadById'. */
    public List<Map<String, Object>> shiftLocation(int id) {
        return db.rows("Sp_genShiftLocation_GetAllMethod", "ShiftLocationId", id, "Activity", "ReadById");
    }

    /** BLL genShiftLocation.Save -> DAL SetData: Insert when ShiftLocationId == 0, else Update. */
    public int saveShiftLocation(GenShiftLocation m) {
        int n = db.set(m.ShiftLocationId == 0 ? "Sp_genShiftLocation_Insert" : "Sp_genShiftLocation_Update", m);
        return n > 0 ? n : m.ShiftLocationId;
    }

    // ------------------------------------------------------------------ 654 genShiftTiming

    /** genShiftTiming.Getall: @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> shiftTimings(UserAccount u) {
        return db.rows("Sp_genShiftTiming_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /**
     * genShiftTiming.GetByID: @ShiftTimingId, @Activity 'ReadById'. (The DAL then asks the same proc for
     * 'ReadByShiftTimingIdWeekofDays', an activity the procedure does not have - it returns nothing and the
     * form never uses it, so that call is not repeated.)
     */
    public List<Map<String, Object>> shiftTiming(int id) {
        return db.rows("Sp_genShiftTiming_GetAllMethod", "ShiftTimingId", id, "Activity", "ReadById");
    }

    /**
     * BLL genShiftTiming.Save -> DAL SetData, one transaction: Sp_genShiftTiming_Insert (ShiftTimingId == 0)
     * or Sp_genShiftTiming_Update (which deletes the old genShiftTimingWeekDay rows), then
     * sP_genShiftTimingWeekDay_Insert for every ticked week day with the header id.
     */
    public int saveShiftTiming(GenShiftTiming m) {
        return db.tx(() -> {
            int n = db.set(m.ShiftTimingId == 0 ? "Sp_genShiftTiming_Insert" : "Sp_genShiftTiming_Update", m);
            if (n > 0) m.ShiftTimingId = n; else n = m.ShiftTimingId;
            for (GenShiftTimingWeekDay d : m.GenShiftTimingWeekDaysDetailList) {
                d.ShiftTimingId = m.ShiftTimingId;
                db.set("sP_genShiftTimingWeekDay_Insert", d);
            }
            return n;
        });
    }

    /** genProfile.GetByProfileTypeId: @OrganizationId, @CompanyId, @ProfileTypeId, @Activity 'ReadByProfileTypeId' (IsActive is not sent by the BLL). */
    public List<Map<String, Object>> profilesByType(UserAccount u, int profileTypeId) {
        return db.rows("Sp_genProfile_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ProfileTypeId", profileTypeId, "Activity", "ReadByProfileTypeId");
    }

    // ------------------------------------------------------------------ 655 genHoliday

    /** genHoliday.GetByMonthAndYear: @Month, @Year, @Activity 'ReadByMonthAndYear'. */
    public List<Map<String, Object>> holidaysByMonth(int month, int year) {
        return db.rows("Sp_genHoliday_GetAllMethod", "Month", month, "Year", year, "Activity", "ReadByMonthAndYear");
    }

    /** genHoliday.Getall: @OrganizationId, @CompanyId, @Activity 'ReadAll' - the company's own holidays (tenancy guard). */
    public List<Map<String, Object>> holidays(UserAccount u) {
        return db.rows("Sp_genHoliday_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /**
     * BLL genHoliday.Save -> DAL SetData: every row through Sp_genHoliday_Insert (ActionTypeId 1 when
     * HolidayId == 0, else 2) in one transaction; after the commit (outside it, as CommonProvider.ExecuteProcedure
     * opens its own connection) [dbo].[Sp_SysUpdateAttendanceFromPull] and [dbo].[Sp_SysUpdateAttendanceStatus]
     * with @OrganizationId / @CompanyId.
     */
    public int saveHolidays(List<GenHoliday> rows, UserAccount u) {
        int result = db.tx(() -> {
            int r = 0;
            for (GenHoliday h : rows) {
                h.ActionTypeId = h.HolidayId == 0 ? 1 : 2;
                r = db.set("Sp_genHoliday_Insert", h);
            }
            return r;
        });
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        db.scalar("Sp_SysUpdateAttendanceFromPull", p);
        db.scalar("Sp_SysUpdateAttendanceStatus", new LinkedHashMap<>(p));
        return result;
    }
}
