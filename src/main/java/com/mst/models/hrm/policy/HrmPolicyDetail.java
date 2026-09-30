package com.mst.models.hrm.policy;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;

/**
 * Architecture.Model.HRM.PolicyManagment.hrmPolicyDetail (model 0680) -> Sp_hrmPolicyDetail_Insert (24 params,
 * every one declared). Description is a string the form never sets: null, so it is not sent.
 */
public class HrmPolicyDetail extends DesktopModel {
    public int AdvanceAfterDay;
    public boolean AllowMultiShiftToEmployeeOnDay;
    public int CPLApplyMinHours;
    public boolean FoodEqually;
    public boolean FoodOnSalary;
    public boolean IsMonthEndPayroll;
    public boolean IsSeparteOTPayroll;
    public boolean LateArrivalWithGraceTime;
    public int LateNoOfDay;
    public int NoOfHalfDay;
    public int NoOfShortHoursInDay;
    public int NoOfShortLeave;
    public int NoOfShortLeaveInMonth;
    public int OTApplyAfterMin;
    public int PayrollCutOffDay;
    public int ShortNoOfHours;
    public BigDecimal AdvanceLimitPercent = BigDecimal.ZERO;
    public BigDecimal FoodShareAmount = BigDecimal.ZERO;
    public BigDecimal FoodSharePercent = BigDecimal.ZERO;
    public BigDecimal OTFactorRate = BigDecimal.ZERO;
    public int HeaderId;
    public int PolicyId;
    public int EmployeeDutyHour;
    public String Description;
}
