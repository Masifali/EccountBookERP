package com.mst.models.hrm.dto;

/**
 * One genDutyRoasterDetail row of the form's lstDutyRoasterDetail that is still on duty at Save
 * (the form's RemoveAll(!IsOnDuty) already ran on the page). Every value is the one the form read from
 * SP_GetDutyDatesForDutyRoaster's second result set; line = DutyRoasterDetailLineId (row index + 1).
 */
public class RosterDetailRowDto {
    public long line;
    public long employeeId;
    public long employeeHistoryId;
    public int shiftId;
    public String dutyDate;
    public String weekDayName;
    public int employeeGroupId;
    public long dutyRoasterDetailId;
    public long dutyRoasterId;
}
