package com.mst.models.hrm.profile;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.ProfileManagement.genEmployeeGroup (model 0660) - every property, as
 * GenericProvider.SetProc sends it to Sp_genEmployeeGroup_Insert / Sp_genEmployeeGroup_Update
 * (11 params, all declared by both procs; note the desktop spelling OrginizationId).
 */
public class GenEmployeeGroup extends DesktopModel {
    public int ActionTypeId;                 // byte
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public int EmployeeGroupId;
    public int CompanyId;
    public long AlteredById;
    public long CreatedById;
    public long OrginizationId;
    public long UserLogId;
    public String EmployeeGroupName;
    public String EmployeeGroupPrefix;
}
