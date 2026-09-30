package com.mst.models.hrm.dto;

/**
 * One dtdetail row of OverTimeRequest (Id, EmployeeId, EmployeeName, FromDateTime, ToDateTime,
 * OTHours, OTRate, IsAllowMeal, EntryByLoader). fromDateTime / toDateTime are "HH:mm" (a row typed
 * or loaded on the page) or the ISO date-time the server returned for a saved row.
 */
public class OvertimeRequestRowDto {
    public long id;
    public long employeeId;
    public String employeeName;
    public String fromDateTime;
    public String toDateTime;
    public double otHours;
    public double otRate;
    public boolean isAllowMeal;
    public boolean entryByLoader;
}
