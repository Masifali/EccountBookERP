package com.mst.models.hrm.profile;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.ProfileManagement.genLocation (model 0661) - every property, as
 * GenericProvider.SetProc sends it to Sp_genLocation_Insert / Sp_genLocation_Update (25 params, all declared).
 * frmgenLocation leaves ActionTypeId / UserLogId / AddressId / BranchId / ProjectId / LocationSettingId at 0,
 * IsDefaultLocation false and FooterDetail null - reproduced as is.
 */
public class GenLocation extends DesktopModel {
    public boolean IsDefaultLocation;
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public int ActionTypeId;
    public int AddressId;
    public int AlteredById;
    public int BranchId;
    public int CompanyId;
    public int CreatedById;
    public int LocationId;
    public int LocationSettingId;
    public int LocationTypeProfileId;
    public int OrganizationId;
    public int ProjectId;
    public int UserLogId;
    public String Email;
    public String FooterDetail;
    public String HeaderDetail;
    public String LicenseNo;
    public String LocationName;
    public String LocationShortName;
    public String Mobile1;
    public String Mobile2;
    public String PortalURL;
    public String SubLocationName;
}
