package com.mst.models.partyprocessing;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.PartyProcessing.InvStockOpeningBalancePartyProcessing (model 0426): the 30 non-virtual
 * properties SetProc sends to Sp_InvStockOpeningBalancePartyProcessing_Insert / _Update (31 params; @RowVersion
 * keeps its default). AttachmentsList / DeleteAttachmentsList are virtual.
 */
public class PpAStockOpeningModel extends DesktopModel {
    public boolean IsApproved;
    public LocalDateTime ApprovedDate;
    public LocalDateTime DocDate;
    public LocalDateTime EntryDate;
    public LocalDateTime ModifyDate;
    public double Qty;
    public double WeightKgs;
    public int ApprovedUserId;
    public int BranchesId;
    public int CompanyId;
    public int FinancialYearId;
    public int DocNo;
    public int DocumentTypeId;
    public int EntryUserId;
    public int Id;
    public int ActionId;
    public int ItemId;
    public int ItemUomSch;
    public int JobLotId;
    public int ModifyUserId;
    public int OrganizationId;
    public int PackingTypeId;
    public int ProjectsId;
    public int WarehouseId;
    public int StockPartyId;
    public int SupplierCustomerId;
    public int CropYearId;
    public String CropYear;
    public String Remarks;
    public String ScreenName;
}
