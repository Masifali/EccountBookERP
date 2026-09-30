package com.mst.models.hrm.policy;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;
import java.util.List;

/**
 * Architecture.Model.HRM.PolicyManagment.hrmSocialPolicy (model 0683) - every non-virtual property, as
 * GenericProvider.SetProc sends it to Sp_hrmSocialPolicy_Insert / Sp_hrmSocialPolicy_Update
 * (16 params, all declared by both procedures).
 */
public class HrmSocialPolicy extends DesktopModel {
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public LocalDateTime FromDate;
    public LocalDateTime ToDate;
    public int ActionTypeId;
    public int AlteredById;
    public int BranchId;
    public int CompanyId;
    public int CreatedById;
    public int MinYearLimit;
    public int OrganizationId;
    public int ProjectId;
    public int SalaryFactorProfileId;
    public int SocialPolicyId;
    public int UserLogId;
    public String PolicyDescription;
    /** virtual List&lt;hrmSocialPolicySlab&gt; SocialSecurityDetail - not a parameter. */
    public transient List<HrmSocialPolicySlab> SocialSecurityDetail;
}
