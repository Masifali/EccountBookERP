package com.mst.models.imports;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.Import.ProformaDetail (model 0631) - non-virtual properties bound to
 * [ImEx].[usp_Set_ProformaDetail] with @Activity INSERT / UPDATE / DELETE (appActionId 1 / 2 / 3).
 */
public class ImpBProformaDetail extends DesktopModel {
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
    public double NetWeightKgs;
    public int appActionId;
    public int approvedUserId;
    public int ItemId;
    public int packingTypeIdInner;
    public int packingTypeIdOuter;
    public int packSizeIdinner;
    public int packSizeIdOuter;
    public long proformaMasterId;
    public int uomIdRate;
    public long appUserLogId;
    public long createdUserId;
    public long lastModifiedUserId;
    public long proformaDetailId;
    public long RowVersionLong;
    public String ItemDescription;
    public String RemarksDetail;
    public int locationBranchId;
    public int lotJobId;
}
