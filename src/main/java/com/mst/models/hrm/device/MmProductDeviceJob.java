package com.mst.models.hrm.device;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.DeviceManagement.mmProductDeviceJob (model 0727) - one queued device job
 * (PullLog / Download / Upload / Delete) as PullAttendanceByMachine.MakeProductDeviceData builds it and
 * DAL mmProductDeviceJob.SetData sends it to Sp_mmProductDeviceJob_Insert (19 parameters, all declared).
 * The device work itself is done later by the attendance service that reads mmProductDeviceJob.
 * MmProductDeviceJobsList (virtual) is not a parameter and is not declared here.
 */
public class MmProductDeviceJob extends DesktopModel {
    public int ActionTypeId;
    public boolean IsAllEmployee;
    public boolean JobPriority;
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public LocalDateTime RequestFromDate;
    public LocalDateTime RequestToDate;
    public int BiometricTypeProfileId;
    public int ProductDeviceId;
    public int OrganizationId;
    public int CompanyId;
    public long AlteredById;
    public long CreatedById;
    public long ProductDeviceJobId;
    public long UserLogId;
    public String EmployeeNo;
    public String JobMessage;
    public String JobStatus;
    public String JobTask;
}
