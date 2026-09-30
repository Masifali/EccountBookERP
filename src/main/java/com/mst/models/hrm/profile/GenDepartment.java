package com.mst.models.hrm.profile;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.ProfileManagement.genDepartment (model 0657) - every property, as
 * GenericProvider.SetProc sends it to Sp_genDepartment_Insert / Sp_genDepartment_Update (17 params, all declared).
 */
public class GenDepartment extends DesktopModel {
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public int ActionTypeId;
    public int AlteredById;
    public int ExpenseAccountId;
    public int PayableAcId;
    public int LoanAcId;
    public int BranchId;
    public int CompanyId;
    public int CreatedById;
    public int DepartmentId;
    public int DepartmentTypeProfileId;
    public int OrganizationId;
    public int UserLogId;
    public String DepartmentCode;
    public String DepartmentName;
    public String ShortName;
}
