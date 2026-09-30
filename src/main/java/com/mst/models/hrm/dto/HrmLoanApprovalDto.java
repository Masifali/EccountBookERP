package com.mst.models.hrm.dto;

import java.util.List;

/**
 * Request body of Loan Approval (667, LoanApproval.Insert): the loaded pending loan (cmbEmployeeName's
 * single row: Id = EmployeeId, LoanId = EmployeeLoanId), the text boxes and the generated installment grid.
 */
public class HrmLoanApprovalDto {
    public int employeeId;          // cmbEmployeeName.Value
    public int loanId;              // SelectedRow.Cells[2] (LoanId) - 0 when the row has no third cell
    public String loanAmount;       // txtLoanAmount.Text
    public String approvedAmount;   // txtApprovedAmount.Text
    public String noOfInstallments; // txtNoofInstallments.Text
    public String appliedOn;        // datAppliedOn.Value
    public List<HrmLoanInstallmentRowDto> details;   // grd.GetRows()
}
