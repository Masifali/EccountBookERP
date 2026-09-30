package com.mst.models.partyprocessing;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.PartyProcessing.ItemTypePartyProcessing (model 0430): every property, as
 * GenericProvider.SetProc sends it to Sp_ItemTypePartyProcessing_Insert / _Update (10 params, all declared).
 */
public class PpAItemTypeModel extends DesktopModel {
    public LocalDateTime EntryDate;
    public LocalDateTime ModifyDate;
    public int CompanyId;
    public int EntryUser;
    public int Id;
    public int LookUpId;
    public int ModifyUser;
    public int OrganizationId;
    public String TypeCode;
    public String TypeDescription;
}
