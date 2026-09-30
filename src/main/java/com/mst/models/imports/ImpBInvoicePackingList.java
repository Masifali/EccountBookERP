package com.mst.models.imports;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.Import.invoicePackingList (model 0634) - the non-virtual properties that
 * GenericProvider.Set(sqlTrn, activity, item) binds to ImEx.usp_Set_invoicePackingList (schema from
 * usp_Sys_GetSchema 'invoicePackingList'). ItemCode / ItemName / PackUom / PackEquivalent / JobLot and the
 * detail list are virtual and never sent.
 */
public class ImpBInvoicePackingList extends DesktopModel {
    public boolean isApproved;
    public LocalDateTime approvedOn;
    public LocalDateTime createdOn;
    public LocalDateTime lastModifiedOn;
    public double netWeightInner;
    public double netWeightOuter;
    public double qtyInner;
    public double qtyOuter;
    public double NetWeightKgs;
    public int appActionId;
    public int JobLotId;
    public int approvedUserId;
    public int createdUserId;
    public int ItemId;
    public int locationBranchId;
    public int locOrderMasterId;
    public int packingTypeIdInner;
    public int packingTypeIdOuter;
    public int packSizeIdinner;
    public int packSizeIdOuter;
    public long invoiceDetailId;
    public long invoiceMasterId;
    public long invoicePackingListId;
    public long lastModifiedUserId;
    public long locOrderDetailId;
    public long rowVersionLong;
    public String containerNo;
    public String ItemDescription;
    public String RemarksDetail;
}
