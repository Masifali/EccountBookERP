package com.mst.models.imports;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Architecture.Model.Import.invoiceOtherItem (model 0633) - non-virtual properties bound to
 * ImEx.usp_Set_invoiceOtherItem. ItemName / ItemCode are virtual and not sent.
 */
public class ImpBInvoiceOtherItem extends DesktopModel {
    public boolean isApproved;
    public LocalDateTime approvedOn;
    public LocalDateTime createdOn;
    public LocalDateTime lastModifiedOn;
    public BigDecimal ItemAmount = BigDecimal.ZERO;
    public BigDecimal ItemQty = BigDecimal.ZERO;
    public BigDecimal NetWeightKgs = BigDecimal.ZERO;
    public double ItemRate;
    public int appActionId;
    public int approvedUserId;
    public int createdUserId;
    public long invoiceOtherItemId;
    public int ItemId;
    public int locationBranchId;
    public int WareHouseFromId;
    public long invoiceMasterId;
    public long lastModifiedUserId;
    public long RowVersionLong;
    public String ContainerNo;
    public String Remarks;
}
