package com.mst.models.hrm.dto;

/**
 * Request body of Employee Loan (663, frmEmployeeLoan.Insert): the form's RecId and its controls.
 * Text boxes travel as typed (strings) so the service reads them with the desktop's Conversion.*.
 */
public class HrmLoanDto {
    public int id;                  // RecId (0 = btnsave, > 0 = btnupdate)
    public int employeeId;          // cmbEmployeeName.Value (0 = no ActiveRow)
    public String appliedOn;        // datAppliedOn.Value (yyyy-MM-dd)
    public String loanAmount;       // txtLoanAmount.Text
    public String noOfInstallments; // txtNoofInstallments.Text
    public String reason;           // txtReason.Text
}
