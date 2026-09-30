package com.mst.models.hrm.dto;

/**
 * frmgenLocation (640): RecId and every text box / combo of the form (txtLocationName, txtShortName,
 * txtSubLocationName, txtEmail, txtMobile1, txtMobile2, txtPortalURL, txtLicenseNo, txtAddressDetail,
 * cmbLocationType).
 */
public class ProfileLocationDto {
    public int id;
    public String name;
    public String shortName;
    public String subName;
    public String email;
    public String mobile1;
    public String mobile2;
    public String portalUrl;
    public String licenseNo;
    public String addressDetail;
    public int typeId;
}
