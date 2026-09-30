package com.mst.models.hrm.dto;

/** Request body of Employee Advance (664, frmEmployeeAdvance.Insert): RecId and the form's controls. */
public class HrmAdvanceDto {
    public int id;                        // RecId
    public int employeeId;                // cmbEmployeeName.Value
    public String docNo;                  // txtDocNo.Text
    public String requestDate;            // datRequestDate.Value
    public String appliedOn;              // datAppliedOn.Value
    public String advanceAmount;          // txtAmount.Text
    public String previousAdvance;        // txtPreviousAdvance.Text
    public String previousApprovedAmount; // txtPreivousApprovedAmount.Text
    public String reason;                 // txtReason.Text
}
