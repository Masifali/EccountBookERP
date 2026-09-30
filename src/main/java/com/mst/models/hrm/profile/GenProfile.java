package com.mst.models.hrm.profile;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.PolicyManagment.genProfile (model 0667) - every property, as
 * GenericProvider.SetProc sends it to Sp_genProfile_Insert / Sp_genProfile_Update (16 params, all declared).
 */
public class GenProfile extends DesktopModel {
    public int ActionTypeId;
    public boolean IsActive;
    public int ProfileTypeId;
    public LocalDateTime EntryDate;
    public LocalDateTime ModifyDate;
    public int CompanyId;
    public int ProfileId;
    public int ProfileSeqNo;
    public long EntryUser;
    public long ModifyUser;
    public long OrganizationId;
    public long UserLogId;
    public String ProfileCode;
    public String ProfileName;
    public String ProfilePrefix;
    public String ProfileUrduName;
}
