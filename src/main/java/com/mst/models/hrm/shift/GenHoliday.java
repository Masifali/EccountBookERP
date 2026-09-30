package com.mst.models.hrm.shift;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.AttendanceManagement.genHoliday (model 0732) - one grid row, SetProc to
 * Sp_genHoliday_Insert (14 parameters, all declared; the procedure inserts when @HolidayId is 0 and
 * IsOff/IsGazette is ticked, updates otherwise, then deletes every row with neither ticked).
 * C# ActionTypeId is a byte (TINYINT); an int is sent - same value. genHolidaysList (virtual) is not sent.
 */
public class GenHoliday extends DesktopModel {
    public int ActionTypeId;
    public boolean IsGazetted;
    public boolean IsOffDay;
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public LocalDateTime HolidayDate;
    public int CompanyId;
    public int HolidayId;
    public long AlteredById;
    public long CreatedById;
    public long OrginizationId;
    public long UserLogId;
    public String HolidayDetail;
    public String Name;
}
