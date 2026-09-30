package com.mst.models.partyprocessing;

import com.mst.models.hrm.DesktopModel;

/**
 * Architecture.Model.Inventory.ItemAllocation (model 0997): IsActive, BranchId, CompanyId, OrganizationId,
 * Id, ItemId -> Sp_ItemAllocation_Insert (6 params). BranchName / ItemAllocationlist are virtual.
 */
public class PpAItemAllocationModel extends DesktopModel {
    public boolean IsActive;
    public int BranchId;
    public int CompanyId;
    public int OrganizationId;
    public int Id;
    public int ItemId;
}
