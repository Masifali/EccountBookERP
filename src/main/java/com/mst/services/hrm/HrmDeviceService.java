package com.mst.services.hrm;

import com.mst.models.UserAccount;
import com.mst.models.hrm.device.MmProductDevice;
import com.mst.models.hrm.device.MmProductDeviceJob;
import com.mst.models.hrm.dto.DevicePullJobDto;
import com.mst.models.hrm.dto.DeviceSaveDto;
import com.mst.repositories.hrm.HrmDeviceRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.*;

/**
 * BLL of the HRM "Device Management" screens (AppModules 2021). Each method names the desktop form
 * method it reproduces; validation wording/order and the messages are the form's.
 *
 * The ZKTeco SDK is not used by either form: frmDeviceManagement only keeps the device list, and
 * PullAttendanceByMachine only QUEUES jobs in mmProductDeviceJob (JobStatus 'Pending') that the attendance
 * service on the device network executes - so both forms are fully ported.
 */
@Service
public class HrmDeviceService {

    public static final int SCREEN_DEVICE = 649;
    public static final int SCREEN_PULL = 650;
    public static final int SCREEN_PULL_BY_MACHINE = 651;

    @Autowired private HrmDeviceRepository repo;
    @Autowired private HrmSupport hrm;

    // ================================================================== 649 frmDeviceManagement

    /** frmDeviceManagement_Load: LocationFill + GridFill. */
    public Map<String, Object> deviceSetup() {
        UserAccount u = hrm.user(SCREEN_DEVICE);
        return map("rights", hrm.rights(u, SCREEN_DEVICE), "locations", locationCombo(u), "rows", deviceGrid(u));
    }

    public List<Map<String, Object>> devices() { return deviceGrid(hrm.user(SCREEN_DEVICE)); }

    /** LocationFill: DataTable { Id = LocationId, Name = LocationName }, BindDDL(ZeroIndex false). */
    private List<Map<String, Object>> locationCombo(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.locations(u)) out.add(map("Id", r.get("LocationId"), "Name", r.get("LocationName")));
        return out;
    }

    /** GridFill: Id, LocationId, LocationName(DeviceLocation), DeviceName, NetworkPort(PortNo), NetworkIP(IpAddress), IsActive, Status(ActiveStatus). */
    private List<Map<String, Object>> deviceGrid(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.devices(u)) {
            out.add(map("Id", r.get("Id"), "LocationId", r.get("LocationId"), "LocationName", r.get("DeviceLocation"),
                    "DeviceName", r.get("DeviceName"), "NetworkPort", r.get("PortNo"), "NetworkIP", r.get("IpAddress"),
                    "IsActive", str(r.get("IsActive")), "Status", r.get("ActiveStatus")));
        }
        return out;
    }

    /** datagrid_DoubleClick -> RetrivedData(Id) -> mmProductDevice.GetByID (only an id of the form's own list). */
    public Map<String, Object> device(int id) {
        UserAccount u = hrm.user(SCREEN_DEVICE);
        if (!owns(repo.devices(u), "Id", id)) throw invalid("Record not found.");
        Map<String, Object> r = one(repo.device(id));
        return map("ProductDeviceId", r.get("ProductDeviceId"), "LocationId", toInt(r.get("LocationId")), "DeviceName", r.get("DeviceName"),
                "NetworkPort", r.get("NetworkPort"), "NetworkIP", r.get("NetworkIP"), "IsActive", toBool(r.get("IsActive")));
    }

    /**
     * Insert(): FormValidation (Location / Device Name / Network Port / Network IP - Text == string.Empty,
     * not trimmed), then the model exactly as the form fills it and mmProductDevice.Save.
     */
    public Map<String, Object> saveDevice(DeviceSaveDto b) {
        UserAccount u = hrm.user(SCREEN_DEVICE);
        List<Map<String, Object>> locations = repo.locations(u);
        Map<String, Object> loc = null;
        for (Map<String, Object> r : locations) if (toInt(r.get("LocationId")) == b.locationId && b.locationId != 0) loc = r;
        if (loc == null) throw invalid("Location Required");
        if (str(b.deviceName).isEmpty()) throw invalid("Device Name Required");
        if (str(b.networkPort).isEmpty()) throw invalid("Network Port Required");
        if (str(b.networkIP).isEmpty()) throw invalid("Network IP Required");
        int id = Math.max(0, b.id);
        if (id > 0 && !owns(repo.devices(u), "Id", id)) throw invalid("Record not found.");
        LocalDateTime now = LocalDateTime.now();
        MmProductDevice m = new MmProductDevice();
        m.ProductDeviceId = id;
        m.LocationId = b.locationId;
        m.DeviceLocationText = str(loc.get("LocationName"));
        m.DeviceName = b.deviceName;
        m.NetworkPort = b.networkPort;
        m.NetworkIP = b.networkIP;
        m.OrganizationId = u.getOrganizationId();
        m.CompanyId = u.getCompanyId();
        m.ActionTypeId = id > 0 ? 2 : 1;
        m.AlteredById = u.getId();
        m.AlteredOn = now;
        m.CreatedById = u.getId();
        m.CreatedOn = now;
        m.IsActive = b.isActive;
        m.UserLogId = u.getId();
        int n = repo.saveDevice(m);
        return saved(n, id > 0 ? "Update Successfully" : "Save Successfully");
    }

    // ================================================================== 650 / 651 PullAttendanceByMachine

    /** Both menu rows (650 PullAttendance, 651 PullAttendanceByMachine) open this form: View on either one. */
    private UserAccount pullUser() {
        UserAccount u = hrm.user();
        if (!hrm.can(u, SCREEN_PULL_BY_MACHINE, "View") && !hrm.can(u, SCREEN_PULL, "View"))
            throw new AccessDeniedException("You don't have the right to view this form...");
        return u;
    }

    /** PullAttendanceByMachine_Load: MachineDataGridBind + EmployeeGridbind. */
    public Map<String, Object> pullSetup() {
        UserAccount u = pullUser();
        return map("machines", machineGrid(u), "employees", employeeGrid(u));
    }

    /** btnnew_Click -> Reset(): EmployeeGridbind + MachineDataGridBind. */
    public Map<String, Object> pullReload() { return pullSetup(); }

    /** MachineDataGridBind: Id, LocationId, BranchName, DeviceName, DeviceLocation, IPAddress, PortNo, IsActive (all text columns). */
    private List<Map<String, Object>> machineGrid(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.devices(u)) {
            out.add(map("Id", r.get("Id"), "LocationId", r.get("LocationId"), "BranchName", r.get("BranchName"), "DeviceName", r.get("DeviceName"),
                    "DeviceLocation", r.get("DeviceLocation"), "IPAddress", r.get("IpAddress"), "PortNo", r.get("PortNo"), "IsActive", str(r.get("IsActive"))));
        }
        return out;
    }

    /** EmployeeGridbind: EmployeeHistoryId, EmployeeId, EmployeeNo, EmployeeName, Designation, Department, ShiftName, Category, Section, FingerTemplate, FaceTemplate. */
    private List<Map<String, Object>> employeeGrid(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.employeesForDutyRoaster(u)) {
            out.add(map("EmployeeHistoryId", r.get("EmployeeHistoryId"), "EmployeeId", r.get("EmployeeId"), "EmployeeNo", r.get("EmployeeNo"),
                    "EmployeeName", r.get("EmployeeName"), "Designation", r.get("Designation"), "Department", r.get("Department"),
                    "ShiftName", r.get("ShiftName"), "Category", r.get("EmployeeCategory"), "Section", r.get("SectionName"),
                    "FingerTemplate", r.get("FingerTemplate"), "FaceTemplate", r.get("FaceTemplate")));
        }
        return out;
    }

    /**
     * MakeProductDeviceData(JobTask) - btnPullAttendance "PullLog", btnDownloadTemplate "Download",
     * button2 "Upload", button1 "Delete": exactly one machine ticked ("Only one row select"), then one
     * Pending job per ticked employee (or one job without EmployeeNo when none is ticked), all in one
     * transaction; "Request successfuly submit".
     */
    public Map<String, Object> queueJob(DevicePullJobDto b) {
        UserAccount u = pullUser();
        String task = str(b.jobTask);
        if (!task.equals("PullLog") && !task.equals("Download") && !task.equals("Upload") && !task.equals("Delete")) throw invalid("Unknown job.");
        List<Integer> devices = b.devices == null ? new ArrayList<>() : b.devices;
        if (devices.size() != 1) throw invalid("Only one row select");
        int deviceId = devices.get(0) == null ? 0 : devices.get(0);
        if (!owns(repo.devices(u), "Id", deviceId)) throw invalid("Record not found.");
        List<String> nos = b.employeeNos == null ? new ArrayList<>() : b.employeeNos;
        List<Map<String, Object>> employees = nos.isEmpty() ? null : repo.employeesForDutyRoaster(u);
        LocalDateTime now = LocalDateTime.now();
        List<MmProductDeviceJob> jobs = new ArrayList<>();
        if (!nos.isEmpty()) {
            for (String no : nos) {
                boolean mine = false;
                for (Map<String, Object> e : employees) if (same(e.get("EmployeeNo"), no)) { mine = true; break; }
                if (!mine) throw invalid("Record not found.");
                MmProductDeviceJob j = job(u, deviceId, task, now);
                j.EmployeeNo = str(no);
                jobs.add(j);
            }
        } else {
            jobs.add(job(u, deviceId, task, now));
        }
        repo.saveJobs(jobs);
        return saved(deviceId, "Request successfuly submit");
    }

    private static MmProductDeviceJob job(UserAccount u, int deviceId, String task, LocalDateTime now) {
        MmProductDeviceJob j = new MmProductDeviceJob();
        j.ProductDeviceId = deviceId;
        j.JobTask = task;
        j.JobStatus = "Pending";
        j.JobPriority = true;
        j.BiometricTypeProfileId = 1;
        j.CreatedById = u.getId();
        j.AlteredById = u.getId();
        j.CreatedOn = now;
        j.AlteredOn = now;
        j.RequestFromDate = now;
        j.RequestToDate = now;
        j.ActionTypeId = 0;
        j.OrganizationId = u.getOrganizationId();
        j.CompanyId = u.getCompanyId();
        return j;
    }
}
