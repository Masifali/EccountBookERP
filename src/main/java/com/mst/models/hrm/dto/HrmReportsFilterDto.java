package com.mst.models.hrm.dto;

/**
 * Filter panel of the HRM report screens (371-378), posted by Show / print checks. Public camelCase
 * fields (Jackson binds them by name). Every id is the combo's Value (0 / absent = the combo is empty,
 * Conversion.ToInt(null) = 0); dates are "yyyy-MM-dd".
 *
 *   departmentId, sectionId, shiftId, designationId, employeeId   UltraCombo values
 *   partyLocationId  cmbPartyLocation (372: genPartyLocation; 374: genLocation "Location")
 *   status           371 cmbIsActive: 1 Active, 2 InActive, 0 = no active row ("All")
 *   date             DocDate (373 / 374 txtDate, 375 txtDate "MMM" - its month and year count)
 *   fromDate, toDate 372 register tab / 376
 *   month, year      372 txtDutyMonth / txtDutyYear, 377 datMonth / datYear, 378 cmbMonth / cmbYear values
 *   monthName        378 cmbMonth.Text ("January" ...) -> @Month
 *   debitAccountId   378 CmbDebitAccount
 *   posted           378 RadPosted.Checked (false = RadUnPosted)
 *   tab              372 tabControl1.SelectedIndex (0 Detail Register, 1 Short Register)
 */
public class HrmReportsFilterDto {
    public int departmentId;
    public int sectionId;
    public int shiftId;
    public int designationId;
    public int employeeId;
    public int partyLocationId;
    public int status;
    public String date;
    public String fromDate;
    public String toDate;
    public int month;
    public int year;
    public String monthName;
    public int debitAccountId;
    public boolean posted;
    public int tab;
}
