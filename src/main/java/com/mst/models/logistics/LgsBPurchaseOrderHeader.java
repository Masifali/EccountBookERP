package com.mst.models.logistics;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Architecture.Model.lgstcm.PurchaseOrderHeader - every NON-virtual property, spelled as the C# property, as GenericProvider.SetProc
 * binds it to [lgstcm].[USP_PurchaseOrderHeader_InsertAndUpdate]. Virtual properties (PaymentTerm, ServiceType, BrokerAgentName, ServiceProviderName, TransactionCurrencyCode, GlcyCurrencyCode, ExportInvoiceNo, PurchaseOrderDetailList, AttachmentsList, DeleteAttachmentsList) are not
 * sent and are not declared here.
 */
public class LgsBPurchaseOrderHeader extends DesktopModel {
    public boolean IsApproved;
    public LocalDateTime ApprovedDate;
    public LocalDateTime DocumentDate;
    public LocalDateTime EntryDate;
    public LocalDateTime ModifyDate;
    public LocalDateTime OrderValidityDate;
    public double fcyExchangeRate;
    public double GlcyExchangeRate;
    public double GlcyAmount;
    public double transactionExchangeRate;
    public int ActionId;
    public int ApprovedUserId;
    public int BranchesId;
    public int brokerAgentId;
    public int CompanyId;
    public int DocumentNo;
    public int DocumentTypeId;
    public int EntryUserId;
    public int fcyCurrencyId;
    public int GlcyCurrencyId;
    public int FinancialYearId;
    public int ModifyUserId;
    public int OrganizationId;
    public int ProjectsId;
    public int PurchaseOrderHeaderId;
    public int serviceProviderId;
    public int serviceTypeId;
    public int transactionCurrencyId;
    public String AttachmentsValues;
    public String CustomAttachmentsValues;
    public String RemarksHeader;
    public int ExportInvoiceId;
    public int paymentTermId;
    public int DueDays;
}
