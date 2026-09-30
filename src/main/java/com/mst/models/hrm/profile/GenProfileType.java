package com.mst.models.hrm.profile;

import com.mst.models.hrm.DesktopModel;

/**
 * Architecture.Model.HRM.PolicyManagment.genProfileType (model 0668) - every property, as
 * GenericProvider.SetProc sends it to Sp_genProfileType_Insert / Sp_genProfileType_Update (6 params, all declared).
 */
public class GenProfileType extends DesktopModel {
    public int BranchesId;
    public int CompanyId;
    public int OrganizationId;
    public int ProfileTypeId;
    public String Prefix;
    public String ProfileTypeName;
}
