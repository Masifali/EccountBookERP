package com.mst.models.hrm.dto;

import java.util.List;

/**
 * 654 frmGenShiftTiming Insert(): RecId (0 = Save / SaveAs), the combos, dates (yyyy-MM-dd), every time
 * picker (HH:mm - the desktop joins each TimeOfDay to the Start Date), the hour text boxes as typed,
 * Next Day Closed and the ticked week-day rows.
 */
public class ShiftTimingSaveDto {
    public int id;
    public int shiftId;
    public int shiftLocationId;
    public String description;
    public String startDate;
    public String endDate;
    public String startTime;
    public String endTime;
    public String startBreakTime;
    public String endBreakTime;
    public String lateStartTime;
    public String lateEndTime;
    public String departureStartTime;
    public String departureEndTime;
    public String shortLeaveInStartTime;
    public String shortLeaveInEndTime;
    public String shortLeaveOutStartTime;
    public String shortLeaveOutEndTime;
    public String halfDayInStartTime;
    public String halfDayInEndTime;
    public String halfDayOutStartTime;
    public String halfDayOutEndTime;
    public String absentAfterInTime;
    public String absentBeforeOutTime;
    public String inHours;
    public String outHours;
    public String totalDaysHours;
    public boolean isNextDayClosed;
    public List<ShiftWeekDayDto> weekDays;
}
