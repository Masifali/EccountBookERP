package com.mst.models.hrm.dto;

/**
 * Request body of frmEmployeeLateAdjustment.Insert (672) and EmployeeShortAdjustment.Insert (674): the
 * form's RecId and its controls as typed (text boxes stay text - the desktop converts them with
 * Conversion.ToInt of the string). Late uses totalLates / totalLateDays / deductedDays; Short uses
 * totalHours / totalMinutes / deductedShortHours.
 */
public class PayrollAdjustmentDto {
    public int id;
    public Object employeeId;
    public Object month;
    public Object year;
    public String totalLates;
    public String totalLateDays;
    public String deductedDays;
    public String totalHours;
    public String totalMinutes;
    public String deductedShortHours;
    public String deductionRate;
    public boolean isDeduction;
    public boolean isLeaveAdjust;
}
