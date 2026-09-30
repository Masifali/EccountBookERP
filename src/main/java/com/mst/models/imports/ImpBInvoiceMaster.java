package com.mst.models.imports;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Architecture.Model.Import.invoiceMaster (model 0637) - non-virtual properties bound by
 * invoiceMasterProvider.Set -> GenericProvider.Set(sqlTrn, Activity, obj) to ImEx.usp_Set_invoiceMaster.
 * The three child lists, the voucher and the attachment lists are virtual and not sent.
 */
public class ImpBInvoiceMaster extends DesktopModel {
    public boolean isApproved;
    public LocalDateTime approvedOn;
    public LocalDateTime BillOfLadingDate;
    public LocalDateTime createdOn;
    public LocalDateTime docDate;
    public LocalDateTime GoodsDeclarationDate;
    public LocalDateTime lastModifiedOn;
    public LocalDateTime lcOrderMasterDate;
    public LocalDateTime proformaMasterDate;
    public BigDecimal exchangeRate = BigDecimal.ZERO;
    public double fcyAmountTotal;
    public double GoodsDeclarationValue;
    public double grossWeightTotal;
    public double netWeightTotal;
    public double NoOfPackages;
    public int AppActionId;
    public int ApprovedUserId;
    public int bankIdExporter;
    public int bankIdImporter;
    public int BranchesId;
    public int CompanyId;
    public int currencyId;
    public int customGroupIdDocument;
    public int DabitAccountId;
    public int destinationPortId;
    public int docNo;
    public int documentTypeId;
    public int fclTotal;
    public int FinancialYearId;
    public int incoTermId;
    public int legalEntityId;
    public int loadingPortId;
    public int OrganizationId;
    public int payementTermId;
    public int ProjectsId;
    public int RevisionNo;
    public int salePersonId;
    public long appUserLogId;
    public long ConsigneeId;
    public long createdUserId;
    public long exporterId;
    public long importerId;
    public long invoiceMasterId;
    public long lastModifiedUserId;
    public long lcOrderMasterId;
    public long masterDocumentSerialId;
    public long proformaMasterId;
    public long RowVersionLong;
    public String AttachmentsValues;
    public String BillOfLadingNo;
    public String CustomAttachmentsValues;
    public String docStatus;
    public String exporterRefNo;
    public String GoodsDeclarationNo;
    public String importerRefNo;
    public String invoiceMasterNo;
    public String remarksHeader;
}
