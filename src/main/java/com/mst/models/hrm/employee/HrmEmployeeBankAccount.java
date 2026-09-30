package com.mst.models.hrm.employee;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.EmployeeManagement.hrmEmployeeBankAccount (model 0718) - 17 properties, as
 * SetProc sends them to Sp_hrmEmployeeBankAccount_Insert (17 parameters, all declared). The form
 * sets BankAccountId (the Bank row id) and never BankProfileId, which therefore goes as 0.
 */
public class HrmEmployeeBankAccount extends DesktopModel {
    public boolean IsPayrollAccount;
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public int ActionTypeId;
    public int AlteredById;
    public int BankAccountId;
    public int BankProfileId;
    public int BankRequisitionId;
    public int CompanyId;
    public int CreatedById;
    public int EmployeeBankAccountId;
    public int EmployeeBankAccountLineId;
    public int EmployeeId;
    public int OrganizationId;
    public int UserLogId;
    public String AccountNo;
    public String AccountTitle;
}
