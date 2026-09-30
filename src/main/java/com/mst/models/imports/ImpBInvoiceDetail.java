package com.mst.models.imports;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.Import.invoiceDetail (model 0636) - non-virtual properties bound to
 * ImEx.usp_Set_invoiceDetail (@Activity from CommonProvider.GetActivity(appActionId): INSERT / UPDATE / DELETE).
 * locOrderDate is virtual on the desktop model and is never sent.
 */
public class ImpBInvoiceDetail extends DesktopModel {
    public boolean isApproved;
    public LocalDateTime approvedOn;
    public LocalDateTime createdOn;
    public LocalDateTime lastModifiedOn;
    public double FcAmount;
    public double ItemRate;
    public double netWeightInner;
    public double netWeightOuter;
    public double qtyInner;
    public double qtyOuter;
    public int appActionId;
    public int approvedUserId;
    public int createdUserId;
    public int ItemId;
    public int locationBranchId;
    public int LocOrderDetailId;
    public int LocOrderMasterId;
    public int lotJobId;
    public double NetWeightKgs;
    public int packingTypeIdInner;
    public int packingTypeIdOuter;
    public int packSizeIdinner;
    public int packSizeIdOuter;
    public int uomIdRate;
    public long invoiceDetailId;
    public long invoiceMasterId;
    public long RowVersionLong;
    public long lastModifiedUserId;
    public String ItemDescription;
    public String RemarksDetail;
}
