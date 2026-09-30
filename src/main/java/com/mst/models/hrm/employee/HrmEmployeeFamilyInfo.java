package com.mst.models.hrm.employee;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.EmployeeManagement.hrmEmployeeFamilyInfo (model 0721) - 16 properties, as
 * SetProc sends them to Sp_hrmEmployeeFamilyInfo_Insert (16 parameters, all declared).
 */
public class HrmEmployeeFamilyInfo extends DesktopModel {
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public int ActionTypeId;
    public int AlteredById;
    public int BranchId;
    public int CompanyId;
    public int CreatedById;
    public int EmployeeFamilyInfoId;
    public int EmployeeFamilyInfoLineId;
    public int EmployeeId;
    public int OrganizationId;
    public int ProjectId;
    public int RelationshipProfileId;
    public int UserLogId;
    public String Description;
    public String Name;
}
