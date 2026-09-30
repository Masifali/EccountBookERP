package com.mst.models.logistics;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Architecture.Model.lgstcm.ServicesBillHeader - every NON-virtual property, spelled as the C# property, as GenericProvider.SetProc
 * binds it to [lgstcm].[USP_ServicesBillHeader_InsertAndUpdate]. Virtual properties (PaymentTerm, ServiceType, BilltoPartyName, ExportInvoiceNo, ExportInvoiceCustomerName, BrokerOrServiceProviderName, TransactionCurrencyCode, GlcyCurrencyCode, InvoiceAccountId, ServicesBillDetailList, ServicesBillFreightVoucherDetailList, AttachmentsList, DeleteAttachmentsList, VoucherHeadInvoices) are not
 * sent and are not declared here.
 */
public class LgsBServicesBillHeader extends DesktopModel {
    public boolean IsApproved;
    public LocalDateTime ApprovedDate;
    public LocalDateTime DocumentDate;
    public LocalDateTime EntryDate;
    public LocalDateTime ModifyDate;
    public double fcyExchangeRate;
    public double transactionExchangeRate;
    public int ActionId;
    public int ApprovedUserId;
    public int BilltoPartyId;
    public int BranchesId;
    public int BrokerOrServiceProviderId;
    public int CompanyId;
    public int DocumentNo;
    public int DocumentTypeId;
    public int DueDays;
    public int EntryUserId;
    public int ExportInvoiceId;
    public int fcyCurrencyId;
    public int FinancialYearId;
    public int ModifyUserId;
    public int OrganizationId;
    public int paymentTermId;
    public int ProjectsId;
    public int ServicesBillHeaderId;
    public int serviceTypeId;
    public int transactionCurrencyId;
    public String ApprovalRemarks;
    public String AttachmentsValues;
    public String CustomAttachmentsValues;
    public String ReferenceNo;
    public String RemarksHeader;
    public String FreightVoucherOutwardIds;
    public int GlcyCurrencyId;
    public double GlcyExchangeRate;
    public double GlcyAmount;
}
