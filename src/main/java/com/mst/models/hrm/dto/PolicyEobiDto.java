package com.mst.models.hrm.dto;

import java.util.List;

/** Request body of E.O.B.I Policy (647, EOBIPolicy.cs Insert()): RecId, the text boxes as typed and the grdfrm rows. */
public class PolicyEobiDto {
    public int id;
    public String policyDescription;
    public String fromDate;
    public String toDate;
    public String ageLimit;
    public String employeeShare;
    public String companyShare;
    public List<PolicyEobiRowDto> details;
}
