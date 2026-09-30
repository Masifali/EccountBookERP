package com.mst.models.imports;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.Import.masterDocumentSerial (model 0629) - non-virtual properties bound by
 * masterDocumentSerialProvider.Set -> GenericProvider.Set(sqlTrn, activity, obj) to
 * ImEx.usp_Set_masterDocumentSerial (RowVersionLong is virtual on this model and is not sent).
 */
public class ImpBMasterDocumentSerial extends DesktopModel {
    public boolean isApproved;
    public LocalDateTime approvedOn;
    public LocalDateTime createdOn;
    public LocalDateTime invoiceDate;
    public LocalDateTime lastModifiedOn;
    public LocalDateTime locOrderDate;
    public LocalDateTime proformaDate;
    public int AppActionId;
    public int approvedUserId;
    public int BranchesId;
    public int CompanyId;
    public int createdUserId;
    public int FinancialYearId;
    public int locationBranchId;
    public int OrganizationId;
    public int refDocumentTypeId;
    public long invoiceMasterId;
    public long lastModifiedUserId;
    public long LocOrderMasterId;
    public long masterDocumentSerialId;
    public long ProformaMasterId;
    public long ShipmentBookingId;
    public String invoiceNo;
    public String locOrderNo;
    public String proformaNo;
}
