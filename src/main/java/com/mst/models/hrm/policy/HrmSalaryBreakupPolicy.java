package com.mst.models.hrm.policy;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;
import java.util.List;

/**
 * Architecture.Model.HRM.PolicyManagment.hrmSalaryBreakupPolicy (model 0682) -> Sp_hrmSalaryBreakupPolicy_Insert /
 * _Update (12 params). The virtual SalaryType and SalaryBreakupPolicieslist are not parameters.
 */
public class HrmSalaryBreakupPolicy extends DesktopModel {
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public double SalaryTypePercent;
    public int ActionTypeId;
    public int AlteredById;
    public int CompanyId;
    public int CreatedById;
    public int LocationId;
    public int OrganizationId;
    public int SalaryBreakupPolicyId;
    public int SalaryTypeProfileId;
    public int UserLogId;
    public transient String SalaryType;
    public transient List<HrmSalaryBreakupPolicy> SalaryBreakupPolicieslist;
}
