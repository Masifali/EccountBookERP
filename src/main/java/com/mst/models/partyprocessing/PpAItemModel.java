package com.mst.models.partyprocessing;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

/**
 * Architecture.Model.PartyProcessing.ItemPartyProcessing (model 0429): the 14 non-virtual properties
 * SetProc sends to Sp_ItemPartyProcessing_Insert / _Update (14 params, all declared). The virtual
 * AttachmentsList / DeleteAttachmentsList / ItemAllocationlist are not parameters (transient).
 */
public class PpAItemModel extends DesktopModel {
    public boolean IsActive;
    public LocalDateTime EntryDate;
    public LocalDateTime ModifyDate;
    public int CompanyId;
    public int EntryUser;
    public int Id;
    public int ItemBaseUnitId;
    public int ItemCategoryId;
    public int ItemTypeId;
    public int ModifyUser;
    public int OrganizationId;
    public int UomGroupId;
    public String ItemCode;
    public String ItemName;

    /** virtual List&lt;ItemAllocation&gt; ItemAllocationlist - saved by the DAL after the header. */
    public transient List<PpAItemAllocationModel> ItemAllocationlist = new ArrayList<>();
}
