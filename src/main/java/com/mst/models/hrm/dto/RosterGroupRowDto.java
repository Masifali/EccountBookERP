package com.mst.models.hrm.dto;

/** One grdGroup row: EmployeeGroupId, the "Shift" cell (ShiftId) and the "RestDay" cell (WeekDayName = ProfileId). */
public class RosterGroupRowDto {
    public int employeeGroupId;
    public int shiftId;
    public int weekDayId;
}
