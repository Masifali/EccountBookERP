package com.mst.models.hrm.policy;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;
import java.util.List;

/**
 * Architecture.Model.HRM.PolicyManagment.hrmPolicyHeader (model 0681) -> Sp_hrmPolicyHeader_Insert / _Update (11 params).
 */
public class HrmPolicyHeader extends DesktopModel {
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public LocalDateTime DateFrom;
    public LocalDateTime DateTo;
    public int ActionType;
    public int AlteredBy;
    public int BranchesId;
    public int CompanyId;
    public int CreatedBy;
    public int OrganizationId;
    public int PolicyHeaderId;
    /** virtual List&lt;hrmPolicyDetail&gt; HrmPolicyDetailList - not a parameter. */
    public transient List<HrmPolicyDetail> HrmPolicyDetailList;
}
