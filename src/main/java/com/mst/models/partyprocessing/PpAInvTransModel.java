package com.mst.models.partyprocessing;

import com.mst.models.hrm.DesktopModel;

/**
 * Architecture.Model.PartyProcessing.InventoryTransactionsPartyProcessing as the stock-opening DAL fills it
 * (OrganizationId, CompanyId, RefDocumentTypeId, RefDocIdNo) -> Sp_InventoryTransactionsPartyProcessing_Insert
 * (4 params).
 */
public class PpAInvTransModel extends DesktopModel {
    public int OrganizationId;
    public int CompanyId;
    public int RefDocumentTypeId;
    public int RefDocIdNo;
}
