package com.mst.models.hrm.dto;

/**
 * Request body of General Policy (644, GeneralPolicy.cs btnsave_Click): RecId + updatemode and every
 * text box / check box the form reads (text boxes as typed; the BLL converts them with Conversion.ToInt).
 */
public class PolicyGeneralDto {
    public int id;
    public boolean updateMode;
    public String dateFrom;
    public String dateTo;
    public String payrollCutOffDay;
    public boolean isMonthEndPayroll;
    public String advanceAfterDay;
    public String advanceLimitPercent;
    public String otFactorRate;
    public boolean isSeparteOTPayroll;
    public String monthlyShortHoursLimit;
    public String noOfShortLeaveInMonth;
    public String noOfShortHoursInDay;
    public String deductionLeaveNoOfLate;
    public String deductionLeaveNoOfShortLeave;
    public String deductionLeaveNoOfHalfDay;
    public String cplApplyMinutesHours;
    public String cplCalculateHours;
    public String overtimeApplyAfterMin;
    public boolean isAllowMultiShiftInOneDay;
    public boolean showLateArrivalWithGraceTime;
}
