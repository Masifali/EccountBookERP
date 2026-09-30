package com.mst.models.hrm.dto;

import java.util.List;

/** Request body of Salary Breakup Policy (645, SalaryBreakupPolicy.cs Insert()): RecId, cmbLocation.Value and grdfrm rows. */
public class PolicySalaryBreakupDto {
    public int id;
    public int locationId;
    public List<PolicySalaryBreakupRowDto> details;
}
