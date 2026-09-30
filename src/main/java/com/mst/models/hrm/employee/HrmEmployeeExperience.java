package com.mst.models.hrm.employee;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.EmployeeManagement.hrmEmployeeExperience (model 0720) - 19 properties, as
 * SetProc sends them to Sp_hrmEmployeeExperience_Insert (19 parameters, all declared).
 */
public class HrmEmployeeExperience extends DesktopModel {
    public int EmployeeExperienceId;
    public int EmployeeExperienceLineId;
    public int EmployeeId;
    public int CityId;
    public int DesignationId;
    public String OrginizationName;
    public LocalDateTime FromDate;
    public LocalDateTime ToDate;
    public String Description;
    public int CreatedById;
    public LocalDateTime CreatedOn;
    public int AlteredById;
    public LocalDateTime AlteredOn;
    public int ActionTypeId;
    public int UserLogId;
    public int OrganizationId;
    public int CompanyId;
    public int BranchId;
    public int ProjectId;
}
