package com.mst.models.hrm.dto;

/** One row of Loan Approval's installment grid (GenerateApprovedLoan's DataTable). */
public class HrmLoanInstallmentRowDto {
    public int employeeLoanInstallmentId;  // EmployeeLoanInstallmentId (always 0)
    public int installments;               // Installments
    public String month;                   // Month (Cells["Month"].Text, "MMM")
    public int year;                       // Year
    public double amount;                  // Amount (typeof(double))
}
