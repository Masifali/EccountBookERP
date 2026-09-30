package com.mst.models.hrm.profile;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.ProfileManagement.genEmployeeCategory (model 0659) - every property, as
 * GenericProvider.SetProc sends it to Sp_genEmployeeCategory_Insert / _Update (11 params, all declared).
 */
public class GenEmployeeCategory extends DesktopModel {
    public int ActionTypeId;                 // byte
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public int EmployeeCategoryId;
    public int CompanyId;
    public long AlteredById;
    public long CreatedById;
    public long OrginizationId;
    public long UserLogId;
    public String EmployeeCategoryName;
    public String EmployeeCategoryPrefix;
}
