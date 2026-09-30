package com.mst.models.hrm.employee;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.EmployeeManagement.genEmployeeWeekDay (model 0711) - 13 properties, as SetProc
 * sends them to Sp_genEmployeeWeekDay_Insert (13 parameters, all declared). The form never sets
 * UserLogId, so it goes as 0 like the desktop.
 */
public class GenEmployeeWeekDay extends DesktopModel {
    public int ActionTypeId;
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public int LineId;
    public int WeekDayProfileId;
    public long AlteredById;
    public long CompanyId;
    public long CreatedById;
    public long EmployeeId;
    public long EmployeeWeekDayId;
    public long OrganizationId;
    public long UserLogId;
    public String WeekDayName;
}
