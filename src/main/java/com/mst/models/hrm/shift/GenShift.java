package com.mst.models.hrm.shift;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.EmployeeManagement.genShift (model 0716) - every property, as SetProc sends it
 * to Sp_genShift_Insert / Sp_genShift_Update (14 parameters, all declared by both procedures).
 */
public class GenShift extends DesktopModel {
    public int ActionTypeId;
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public int BranchId;
    public int CompanyId;
    public int OrganizationId;
    public int ProjectId;
    public int ShiftId;
    public int AlteredById;
    public int CreatedById;
    public int UserLogId;
    public String ShiftName;
    public String ShiftShortName;
    public String ShiftUrduName;
}
