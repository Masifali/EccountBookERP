package com.mst.models.imports;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Architecture.Model.Import.Transaction.ShipmentBooking (model 0656) - every non-virtual property,
 * spelled as the C# property, in declaration order. DAL ShipmentBooking.SetData binds them all through
 * GenericProvider.SetProc to [ImEx].[usp_Set_ShipmentBooking] (+ @Activity INSERT / UPDATE).
 * AttachmentsList / DeleteAttachmentsList are virtual on the desktop and are not bound.
 */
public class ImpBShipmentBooking extends DesktopModel {
    public boolean isApproved;
    public LocalDateTime approvedOn;
    public LocalDateTime bookingDate;
    public LocalDateTime createdOn;
    public LocalDateTime DeletedOn;
    public LocalDateTime docDate;
    public LocalDateTime etaDestinationPort;
    public LocalDateTime etaLoadingport;
    public LocalDateTime etdLoadingPort;
    public LocalDateTime lastModifiedOn;
    public BigDecimal exchangeRate = BigDecimal.ZERO;
    public int appActionId;
    public int approvedUserId;
    public int BranchesId;
    public int CompanyId;
    public int containerTypeId;
    public int createdUserId;
    public int currencyId;
    public int dischargePortId;
    public int docNo;
    public int FinancialYearId;
    public int freeDays;
    public int loadingPortId;
    public int locationBranchId;
    public int OrganizationId;
    public int ProjectsId;
    public long ShipmentBookingId;
    public int shippingLIneId;
    public int totalContainer;
    public int transitDays;
    public long DeletedUserId;
    public long invoiceMasterId;
    public long lastModifiedUserId;
    public long LocOrderMasterId;
    public String address;
    public String AttachmentsValues;
    public String bookingNo;
    public String containerCollectionLocation;
    public String CustomAttachmentsValues;
    public String vessel;
    public String voyage;
    public long RowVersionLong;
}
