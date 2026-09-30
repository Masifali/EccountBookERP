package com.mst.models.hrm.profile;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.ProfileManagement.hrmBenefit (model 0663) - every non-virtual property, as
 * GenericProvider.SetProc sends it to Sp_hrmBenefit_Insert / Sp_hrmBenefit_Update (15 params, all declared).
 * ProfileName is `virtual` on the desktop (not sent) -> transient.
 */
public class HrmBenefit extends DesktopModel {
    public boolean IsDirect;
    public boolean PayrolEffect;
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public int ActionTypeId;
    public int AlteredById;
    public int BenefitId;
    public int BenefitTypeProfileId;
    public int BranchesId;
    public int CompanyId;
    public int CreatedById;
    public int OrganizationId;
    public int UserLogId;
    public String BenefitName;
    public String BenefitPrefix;
    public transient String ProfileName;
}
