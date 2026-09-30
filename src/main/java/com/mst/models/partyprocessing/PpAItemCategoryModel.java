package com.mst.models.partyprocessing;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.PartyProcessing.ItemCategoryPartyProcessing (model 0428): the 13 properties SetProc
 * sends to Sp_ItemCategoryPartyProcessing_Insert / _Update (13 params, all declared).
 */
public class PpAItemCategoryModel extends DesktopModel {
    public boolean CategoryStatus;
    public LocalDateTime EntryDate;
    public LocalDateTime ModifyDate;
    public int CompanyId;
    public int EntryUser;
    public int Id;
    public int ModifyUser;
    public int OrganizationId;
    public int ParentCategoriesId;
    public int SerialFrom;
    public int SerialTo;
    public String CategoryCode;
    public String CategoryDescription;
}
