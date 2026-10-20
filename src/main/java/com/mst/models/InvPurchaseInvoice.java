package com.mst.models;

import javax.persistence.*;
import lombok.Data;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;

import java.util.Date;
import java.util.List;
import java.util.ArrayList;

/** Mapped 1:1 onto dbo.InvPurchaseInvoice (source: Sp_InvPurchaseInvoice_Insert / _Update / Sp_InvoicesVouchersandStocksDelete). Id is SCOPE_IDENTITY() in the Insert proc. */
@Entity
@Table(name = "InvPurchaseInvoice")
@Data
public class InvPurchaseInvoice {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "DocumentTypeId")
    private Integer documentTypeId;

    @Column(name = "DocNo")
    private Integer docNo;

    @Column(name = "DocDate")
    @Temporal(TemporalType.DATE)
    private Date docDate;

    @Column(name = "FinancialYearId")
    private Integer financialYearId;

    @Column(name = "SupplierCustomerId")
    private Integer supplierCustomerId;

    @Column(name = "ManualBillNo", length = 50)
    private String manualBillNo;

    @Column(name = "BillAmount")
    private Double billAmount;

    @Column(name = "RemarksHeader")
    private String remarksHeader;

    @Column(name = "IsApproved")
    private Boolean isApproved = false;

    @Column(name = "EntryDate")
    @Temporal(TemporalType.TIMESTAMP)
    private Date entryDate;

    @Column(name = "EntryUser")
    private Integer entryUser;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "BranchesId")
    private Integer branchesId;

    @OneToMany(mappedBy = "invPurchaseInvoiceId", cascade = CascadeType.ALL, fetch = FetchType.LAZY)
    private List<InvPurchaseInvoiceDetail> details = new ArrayList<>();

    // ---- columns added by batch G2_purchase_invoice_write (registry + procedures) ----

    @Column(name = "VoucherDate")
    private LocalDateTime voucherDate;

    @Column(name = "VoucherCode")
    private String voucherCode;

    @Column(name = "BaseDocumentTypeId")
    private Integer baseDocumentTypeId;

    @Column(name = "DocumentTypeSrNo")
    private Integer documentTypeSrNo;

    @Column(name = "ChequeNo")
    private String chequeNo;

    @Column(name = "PayTitle")
    private String payTitle;

    @Column(name = "DueDate")
    private LocalDate dueDate;

    @Column(name = "DueDays")
    private Integer dueDays;

    @Column(name = "SupplierReferenceNo")
    private String supplierReferenceNo;

    @Column(name = "InvSaleInvoiceId")
    private Integer invSaleInvoiceId;

    @Column(name = "InvGdnId")
    private Integer invGdnId;

    @Column(name = "ReferencePartyName")
    private String referencePartyName;

    @Column(name = "SalesTaxNo")
    private Integer salesTaxNo;

    @Column(name = "SupplierInvoiceNo")
    private Integer supplierInvoiceNo;

    @Column(name = "SupplierInvoiceDate")
    private LocalDateTime supplierInvoiceDate;

    @Column(name = "StockPartyId")
    private Integer stockPartyId;

    @Column(name = "ReferencePartyId")
    private Integer referencePartyId;

    @Column(name = "CommissionAgentId")
    private Integer commissionAgentId;

    @Column(name = "CommissionType")
    private String commissionType;

    @Column(name = "CommRate")
    private Double commRate;

    @Column(name = "UomScheduleIdCmRate")
    private String uomScheduleIdCmRate;

    @Column(name = "CommAmount")
    private Double commAmount;

    @Column(name = "CommissionRemarks")
    private String commissionRemarks;

    @Column(name = "CashReceived")
    private Double cashReceived;

    @Column(name = "OtherRemarks")
    private String otherRemarks;

    @Column(name = "IsAttachments")
    private String isAttachments;

    @Column(name = "IsTaxable")
    private Boolean isTaxable;

    @Column(name = "ModifyDate")
    private LocalDateTime modifyDate;

    @Column(name = "ModifyUser")
    private Integer modifyUser;

    @Column(name = "PostDate")
    private LocalDateTime postDate;

    @Column(name = "PostUser")
    private Integer postUser;

    @Column(name = "ProjectsId")
    private Integer projectsId;

    @Column(name = "DeliveryTerm")
    private String deliveryTerm;

    @Column(name = "PaymentTermsId")
    private Integer paymentTermsId;

    @Column(name = "BrokerAgentId")
    private Integer brokerAgentId;

    @Column(name = "BrokeryType")
    private String brokeryType;

    @Column(name = "BrokeryUom")
    private Double brokeryUom;

    @Column(name = "BrokeryRate")
    private Double brokeryRate;

    @Column(name = "BrokeryAmount")
    private Double brokeryAmount;

    @Column(name = "InvoiceTypeId")
    private Integer invoiceTypeId;

    @Column(name = "TransportAccountId")
    private Integer transportAccountId;

    @Column(name = "FreightAmount")
    private Double freightAmount;

    @Column(name = "ActionId")
    private Integer actionId;

    @Column(name = "GhallaMandiId")
    private Integer ghallaMandiId;

    @Column(name = "DeliveryDays")
    private Integer deliveryDays;

    @Column(name = "DeliveryStartDate")
    private LocalDate deliveryStartDate;

    @Column(name = "BillCalculateTypeId")
    private Integer billCalculateTypeId;

    @Column(name = "InvoiceQty")
    private BigDecimal invoiceQty;

    @Column(name = "InvoiceWeight")
    private BigDecimal invoiceWeight;

    @Column(name = "CurrencyId")
    private Integer currencyId;

    @Column(name = "ExchangeRate")
    private BigDecimal exchangeRate;

    @Column(name = "FcyAmount")
    private BigDecimal fcyAmount;

    @Column(name = "ScreenName")
    private String screenName;

    @Column(name = "WagesAmount")
    private Double wagesAmount;

    @Column(name = "BranchSrNo")
    private Integer branchSrNo;

    @Column(name = "CommissionCreditAccountId")
    private Integer commissionCreditAccountId;

    @Column(name = "AttachmentsValues")
    private String attachmentsValues;

    @Column(name = "CustomAttachmentsValues")
    private String customAttachmentsValues;

    @Column(name = "DiscountAmount")
    private Double discountAmount;

    @Column(name = "DiscountAccountId")
    private Integer discountAccountId;

    @Column(name = "TransporterCreditPartyId")
    private Integer transporterCreditPartyId;

    @Column(name = "TransporterDebitGLId")
    private Integer transporterDebitGLId;

    @Column(name = "TransporterDebitPartyId")
    private Integer transporterDebitPartyId;

    @Column(name = "FreightRemark")
    private String freightRemark;

    @Column(name = "CustomAccounts")
    private Boolean customAccounts;

    @Column(name = "IsUploaded")
    private Boolean isUploaded;

    @Column(name = "UploadedById")
    private Integer uploadedById;

    @Column(name = "UploadedDate")
    private LocalDateTime uploadedDate;

    @Column(name = "RecordNo")
    private Integer recordNo;

    @Column(name = "VoucherHeadId")
    private Integer voucherHeadId;

    @Column(name = "PurchaseAgainst")
    private String purchaseAgainst;

    @Column(name = "BillType")
    private String billType;

    @Column(name = "ApprovedStatus")
    private String approvedStatus;

    @Column(name = "NoOfAttachments")
    private Integer noOfAttachments;

    @Column(name = "VehicleNos")
    private String vehicleNos;

    @Column(name = "EntryUserId")
    private Integer entryUserId;

    @Column(name = "ModifyUserId")
    private Integer modifyUserId;

    @Column(name = "ScheduleCode")
    private String scheduleCode;

    @Column(name = "ExImLcOrderId")
    private Integer exImLcOrderId;

    @Column(name = "DocCode")
    private String docCode;

    @Column(name = "TaxAccountId")
    private Integer taxAccountId;

    @Column(name = "InvoiceId")
    private Integer invoiceId;

    @Column(name = "UnApproveUserId")
    private Integer unApproveUserId;

    @Column(name = "RevisionNo")
    private Integer revisionNo;

    @Column(name = "UserLogId")
    private Long userLogId;

    @Column(name = "TransporterId")
    private Integer transporterId;

    @Column(name = "GpNo")
    private Integer gpNo;

    @Column(name = "VehicleNo")
    private String vehicleNo;

    @Column(name = "FactoryWeight")
    private BigDecimal factoryWeight;

    @Column(name = "PartyWeight")
    private BigDecimal partyWeight;

    @Column(name = "RateCut")
    private BigDecimal rateCut;

    @Column(name = "RateCutRemarks")
    private String rateCutRemarks;

    @Column(name = "InwardGatePassId")
    private Integer inwardGatePassId;

    @Column(name = "PurchaseOrderId")
    private Integer purchaseOrderId;
}
