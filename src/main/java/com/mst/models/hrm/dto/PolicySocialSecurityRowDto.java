package com.mst.models.hrm.dto;

/** One grdfrm row of Social Security: FromSalary / ToSalary (string columns), Percent% / Amount (double columns). */
public class PolicySocialSecurityRowDto {
    public String fromSalary;
    public String toSalary;
    public Double percent;
    public Double amount;
}
