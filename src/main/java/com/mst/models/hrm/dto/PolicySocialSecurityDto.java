package com.mst.models.hrm.dto;

import java.util.List;

/**
 * Request body of Social Security (643, SocialSecurity.cs Insert()): RecId, the Main group and the
 * detail grid (grdfrm rows) exactly as the form reads them.
 */
public class PolicySocialSecurityDto {
    /** RecId (0 = Save, &gt; 0 = Update). */
    public int id;
    /** cmbSalaryFactor.Value */
    public int salaryFactorProfileId;
    /** cmbSalaryFactor.Text (formvalidation checks the text). */
    public String salaryFactorText;
    public String fromDate;
    public String toDate;
    /** txtminyearlimit.Text as typed. */
    public String minYearLimit;
    public String policyDescription;
    public List<PolicySocialSecurityRowDto> details;
}
