package com.mst.models.hrm.employee;

import com.mst.models.hrm.DesktopModel;

/**
 * Architecture.Model.USERINFO (model 0103) - the attendance-device user row the genEmployee DAL writes
 * with Sp_USERINFO_Insert (5 parameters, all declared) after every employee save.
 */
public class USERINFO extends DesktopModel {
    public int USERID;
    public int BADGENUMBER;
    public int OrganizationId;
    public int CompanyId;
    public String NAME;
}
