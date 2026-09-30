package com.mst.models.hrm.profile;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.ProfileManagement.genSection (model 0662) - every property, as
 * GenericProvider.SetProc sends it to Sp_genSection_Insert / Sp_genSection_Update (14 params, all declared).
 * CreatedOn / AlteredOn are DateTime? and DefineSection never sets them -> null -> not sent (proc default NULL).
 */
public class GenSection extends DesktopModel {
    public int SectionId;
    public int DepartmentId;
    public String ShortName;
    public String SectionName;
    public String SectionCode;
    public int SectionSeqNo;
    public int CreatedById;
    public LocalDateTime CreatedOn;
    public int AlteredById;
    public LocalDateTime AlteredOn;
    public int ActionTypeId;
    public int UserLogId;
    public int OrganizationId;
    public int CompanyId;
}
