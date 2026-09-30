package com.mst.models.hrm.dto;

/** One frmHoliday grid row: Id (HolidayId, 0 = new), Date (dd-MMM-yyyy), Day, IsOff, IsGazette, Description. */
public class ShiftHolidayRowDto {
    public int id;
    public String date;
    public String day;
    public boolean isOff;
    public boolean isGazette;
    public String description;
}
