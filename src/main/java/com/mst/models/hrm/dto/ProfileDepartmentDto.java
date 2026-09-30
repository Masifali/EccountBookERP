package com.mst.models.hrm.dto;

/**
 * genDepartment (638): RecId, cmbDepartmentType, txtShortName, txtDepartmentName,
 * CmbSalariesExpensesAc, CmbSalaryPayableAc, CmbSalaryLoanAc.
 */
public class ProfileDepartmentDto {
    public int id;
    public int typeId;
    public String shortName;
    public String name;
    public int expenseAccountId;
    public int payableAcId;
    public int loanAcId;
}
