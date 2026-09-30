package com.mst.models.hrm.dto;

/** grdEmployeeDirectBenefits / grdEmployeeAssetsBenefit row: BenefitId, BenefitValue, ApprovalPersonId, Select. */
public class EmployeeBenefitRowDto {
    public int benefitId;
    public String benefitValue;
    public int approvalPersonId;
    public boolean select;
}
