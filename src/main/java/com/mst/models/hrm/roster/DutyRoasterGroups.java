package com.mst.models.hrm.roster;

import com.mst.models.hrm.DesktopModel;

/**
 * Architecture.Model.HRM.AttendanceManagement.DutyRoasterGroups (model 0728) - the 5 properties
 * GenericProvider.SetProc sends to Sp_DutyRoasterGroups_Insert (@Id, @DutyRoasterId, @EmployeeGroupId,
 * @ShiftId, @WeekDayId - all declared).
 */
public class DutyRoasterGroups extends DesktopModel {
    public int Id;
    public int EmployeeGroupId;
    public int ShiftId;
    public long DutyRoasterId;
    public int WeekDayId;
}
