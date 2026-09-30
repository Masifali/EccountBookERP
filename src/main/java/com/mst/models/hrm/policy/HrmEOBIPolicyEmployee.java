package com.mst.models.hrm.policy;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;

/**
 * Architecture.Model.HRM.PolicyManagment.hrmEOBIPolicyEmployee (model 0671) -> Sp_hrmEOBIPolicyEmployee_Insert (5 params).
 * The virtual EmployeeName is not a parameter.
 */
public class HrmEOBIPolicyEmployee extends DesktopModel {
    public BigDecimal CompanyShare = BigDecimal.ZERO;
    public BigDecimal EmployeeShare = BigDecimal.ZERO;
    public long EmployeeId;
    public long hrmEOBIPolicyEmployeeId;
    public long hrmEOBIPolicyId;
    public transient String EmployeeName;
}
