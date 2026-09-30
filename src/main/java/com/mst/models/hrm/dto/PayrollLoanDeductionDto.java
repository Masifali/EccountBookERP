package com.mst.models.hrm.dto;

import java.util.List;

/** EmployeeLoanDeduction.Insert (673): cmbMonth / cmbYear and every grdPendingForDeduction row. */
public class PayrollLoanDeductionDto {
    public Object month;
    public Object year;
    public List<Row> rows;

    /** dtPending row: Id, EmployeeId, InstallmentAmount, NoOfInstallment, Amount. */
    public static class Row {
        public Object id;
        public Object employeeId;
        public Object installmentAmount;
        public Object noOfInstallment;
        public Object amount;
    }
}
