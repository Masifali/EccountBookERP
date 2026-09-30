package com.mst.models.hrm.employee;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.EmployeeManagement.hrmEmployeeReference (model 0722) - 21 properties, as
 * SetProc sends them to Sp_hrmEmployeeReference_Insert (21 parameters, all declared).
 */
public class HrmEmployeeReference extends DesktopModel {
    public boolean IsReferPerson;
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public int ActionTypeId;
    public int AddressId;
    public int AlteredById;
    public int BranchId;
    public int CompanyId;
    public int CreatedById;
    public int DesignationId;
    public int EmployeeId;
    public int EmployeeReferenceId;
    public int EmployeeReferenceLineId;
    public int OrganizationId;
    public int ProjectId;
    public int UserLogId;
    public String Description;
    public String Name;
    public String OrginizationName;
    public String Mobile;
    public String Email;
}
