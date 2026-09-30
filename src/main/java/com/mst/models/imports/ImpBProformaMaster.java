package com.mst.models.imports;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Architecture.Model.Import.ProformaMaster (model 0632) - non-virtual properties bound by DAL
 * ProformaMaster.SetDate -> GenericProvider.SetProc(sqlTrn, obj, "[ImEx].[usp_Set_ProformaMaster]", activity).
 */
public class ImpBProformaMaster extends DesktopModel {
    public int appActionId;
    public boolean isApproved;
    public LocalDateTime approvedOn;
    public LocalDateTime createdOn;
    public LocalDateTime dateOfIssue;
    public LocalDateTime docDate;
    public LocalDateTime lastModifiedOn;
    public LocalDateTime lastShipmentDate;
    public LocalDateTime validityUpto;
    public BigDecimal fclTotal = BigDecimal.ZERO;
    public BigDecimal fcyAmountTotal = BigDecimal.ZERO;
    public BigDecimal grossWeightTotal = BigDecimal.ZERO;
    public BigDecimal netWeightTotal = BigDecimal.ZERO;
    public int ApprovedUserId;
    public int BranchesId;
    public int CompanyId;
    public int currencyId;
    public int destinationPortId;
    public int docNo;
    public int documentTypeId;
    public int FinancialYearId;
    public int incoTermId;
    public int loadingPortId;
    public int OrganizationId;
    public int placeOfIssue;
    public int ProjectsId;
    public int paymentTermId;
    public int GoodsOriginId;
    public long appUserLogId;
    public long bankIdExporter;
    public long bankIdImporter;
    public long clientLocationId;
    public long createdUserId;
    public long exporterId;
    public long importerId;
    public long lastModifiedUserId;
    public long masterDocumentSerialId;
    public long proformaMasterId;
    public long RowVersionLong;
    public long salePersonId;
    public long stationBranchId;
    public String TermsConditions;
    public String AttachmentsValues;
    public String CustomAttachmentsValues;
    public String docStatus;
    public String exporterRefNo;
    public String importerRefNo;
    public String proformaNo;
    public String remarksHeader;
}
