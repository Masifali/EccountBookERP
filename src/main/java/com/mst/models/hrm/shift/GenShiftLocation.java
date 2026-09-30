package com.mst.models.hrm.shift;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.EmployeeManagement.genShiftLocation (model 0717) - SetProc to
 * Sp_genShiftLocation_Insert / Sp_genShiftLocation_Update (14 parameters, all declared).
 * LocationName / ShiftName are virtual on the desktop (not sent) and are left out.
 */
public class GenShiftLocation extends DesktopModel {
    public boolean IsActive;
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public int ActionTypeId;
    public int AlteredById;
    public int BranchId;
    public int CompanyId;
    public int CreatedById;
    public int LocationId;
    public int OrganizationId;
    public int ProjectId;
    public int ShiftId;
    public int ShiftLocationId;
    public int UserLogId;
}
