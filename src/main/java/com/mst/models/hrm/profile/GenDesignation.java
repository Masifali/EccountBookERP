package com.mst.models.hrm.profile;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.ProfileManagement.genDesignation (model 0658) - every property, as
 * GenericProvider.SetProc sends it to Sp_genDesignation_Insert / Sp_genDesignation_Update (10 params,
 * checked against procdure.utf8.sql).
 */
public class GenDesignation extends DesktopModel {
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public int ActionTypeId;
    public int AlteredById;
    public int CompanyId;
    public int CreatedById;
    public int DesignationId;
    public int OrganizationId;
    public int UserLogId;
    public String DesignationName;
}
