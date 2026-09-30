package com.mst.models.hrm.policy;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;
import java.util.List;

/**
 * Architecture.Model.HRM.PolicyManagment.hrmEOBIPolicy (model 0670) -> Sp_hrmEOBIPolicy_Insert / _Update (15 params).
 */
public class HrmEOBIPolicy extends DesktopModel {
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public LocalDateTime FromDate;
    public LocalDateTime ToDate;
    public double CompanyShare;
    public double EmployeeShare;
    public int ActionTypeId;
    public int AgeLimit;
    public int AlteredById;
    public int CompanyId;
    public int CreatedById;
    public int EOBIPolicyId;
    public int OrganizationId;
    public int UserLogId;
    public String PolicyDescription;
    public transient List<HrmEOBIPolicyEmployee> EOBIPolicyEmployeesList;
}
