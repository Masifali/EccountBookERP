package com.mst.repositories.hrm;

import com.mst.models.UserAccount;
import com.mst.models.hrm.device.MmProductDevice;
import com.mst.models.hrm.device.MmProductDeviceJob;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Map;

/**
 * DAL of the HRM "Device Management" screens (AppModules 2021). Every call is the desktop DAL's own
 * procedure with the parameters its BLL sends:
 *
 *   649 frmDeviceManagement       BLL 0213 / DAL 0184 mmProductDevice   Sp_mmProductDevice_GetAllMethod ReadAll|ReadById,
 *                                                                         Sp_mmProductDevice_Insert|Update (58);
 *                                 BLL 0170 genLocation                  Sp_genLocation_GetAllMethod ReadAll
 *   650/651 PullAttendanceByMachine  BLL 0214 / DAL 0185 mmProductDeviceJob  Sp_mmProductDeviceJob_Insert (19), one transaction;
 *                                 BLL genEmployeeHistory.EmployeeLoadForDutyRoaster  Sp_genEmployeeHistory_GetAllMethod
 */
@Repository
public class HrmDeviceRepository {

    private final HrmProcRepository db;

    public HrmDeviceRepository(HrmProcRepository db) { this.db = db; }

    /**
     * mmProductDevice.Getall: @OrganizationId, @CompanyId, @Activity 'ReadAll'.
     * (The procedure's own WHERE on Organization/Company is commented out - it returns every device.)
     */
    public List<Map<String, Object>> devices(UserAccount u) {
        return db.rows("Sp_mmProductDevice_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** mmProductDevice.GetByID: @ProductDeviceId, @Activity 'ReadById' (the desktop takes [0]). */
    public List<Map<String, Object>> device(int id) {
        return db.rows("Sp_mmProductDevice_GetAllMethod", "ProductDeviceId", id, "Activity", "ReadById");
    }

    /**
     * BLL mmProductDevice.Save -> DAL SetData: Sp_mmProductDevice_Insert when ProductDeviceId == 0, else
     * Sp_mmProductDevice_Update; Convert.ToInt32(SetProc) or, when nothing came back (the Update), obj.ProductDeviceId.
     */
    public int saveDevice(MmProductDevice m) {
        int n = db.set(m.ProductDeviceId == 0 ? "Sp_mmProductDevice_Insert" : "Sp_mmProductDevice_Update", m);
        return n > 0 ? n : m.ProductDeviceId;
    }

    /** genLocation.Getall (ProfileManagement): @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> locations(UserAccount u) {
        return db.rows("Sp_genLocation_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /**
     * genEmployeeHistory.EmployeeLoadForDutyRoaster with only Organization / Company set (the form leaves
     * Shift / Section / Department / Location / Designation at 0, so the BLL adds none of them):
     * @OrganizationId, @CompanyId, @Activity 'EmployeeLoadForDutyRoaster'.
     */
    public List<Map<String, Object>> employeesForDutyRoaster(UserAccount u) {
        return db.rows("Sp_genEmployeeHistory_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "EmployeeLoadForDutyRoaster");
    }

    /**
     * BLL mmProductDeviceJob.Save(obj): the container's ProductDeviceId is always 0, so it is always
     * DAL SetData(obj, "Sp_mmProductDeviceJob_Insert"): every job of MmProductDeviceJobsList in ONE
     * transaction; returns the last SCOPE_IDENTITY.
     */
    public int saveJobs(List<MmProductDeviceJob> jobs) {
        return db.tx(() -> {
            int result = 0;
            for (MmProductDeviceJob j : jobs) result = db.set("Sp_mmProductDeviceJob_Insert", j);
            return result;
        });
    }
}
