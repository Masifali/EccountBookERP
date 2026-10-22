package com.mst.models;

import javax.persistence.*;
import lombok.Data;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.Date;
import java.util.List;
import java.util.ArrayList;

/**
 * Mapped 1:1 onto dbo.PurchaseOrder.
 * Existing entity (copied from SRC_REF) extended by batch E_purchase_order with every column seen in
 * Sp_PurchaseOrder_GetAllMethod / Sp_PurchaseOrder_Update / Sp_PurchaseOrderSlip_Rpt / Sp_PurchaseOrder_PackingMaterial_Rpt.
 * Existing fields/annotations are untouched; new columns are appended at the end.
 */
@Entity
@Table(name = "PurchaseOrder")
@Data
public class PurchaseOrder {

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

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "BranchesId")
    private Integer branchesId;

    @Column(name = "IsApproved")
    private Boolean isApproved = false;

    @Column(name = "RemarksHeader")
    private String remarksHeader;

    @Column(name = "EntryDate")
    @Temporal(TemporalType.TIMESTAMP)
    private Date entryDate;

    @Column(name = "EntryUser")
    private Integer entryUser;

    @OneToMany(mappedBy = "purchaseOrderId", cascade = CascadeType.ALL, fetch = FetchType.LAZY)
    private List<PurchaseOrderDetail> details = new ArrayList<>();

    // ---------------------------------------------------------------------------------------------
    // Columns added by batch E_purchase_order (from Sp_PurchaseOrder_GetAllMethod 'ReadById',
    // Sp_PurchaseOrder_Update SET list and 'UpdateStatusandIsapprovedByOrderId').
    // ---------------------------------------------------------------------------------------------

    @Column(name = "BranchSrNo")
    private Integer branchSrNo;

    @Column(name = "OrderCatagoryId")
    private Integer orderCatagoryId;

    @Column(name = "CatagorySrNo")
    private Integer catagorySrNo;

    @Column(name = "OrderSupCustId")
    private Integer orderSupCustId;

    @Column(name = "SupplierRefNo")
    private String supplierRefNo;

    @Column(name = "RefrenenceParty")
    private Integer refrenenceParty;

    @Column(name = "SupplierCustomerIdStockParty")
    private Integer supplierCustomerIdStockParty;

    @Column(name = "BrokerAgentSupCustId")
    private Integer brokerAgentSupCustId;

    @Column(name = "CommissionType")
    private String commissionType;

    @Column(name = "CommRate")
    private Double commRate;

    @Column(name = "UomScheduleIdCmRate")
    private Integer uomScheduleIdCmRate;

    @Column(name = "CommAmount")
    private Double commAmount;

    @Column(name = "CommissionRemarks")
    private String commissionRemarks;

    @Column(name = "PaymentTermsId")
    private Integer paymentTermsId;

    @Column(name = "OrderDueDays")
    private Integer orderDueDays;

    @Column(name = "OrderDueDate")
    private LocalDateTime orderDueDate;

    @Column(name = "OrderExpiryDate")
    private LocalDate orderExpiryDate;

    @Column(name = "DeliveryTerm")
    private String deliveryTerm;

    @Column(name = "DeliveryStartDate")
    private LocalDateTime deliveryStartDate;

    @Column(name = "DeliveryDays")
    private Integer deliveryDays;

    @Column(name = "DeliveryRemarks")
    private String deliveryRemarks;

    @Column(name = "OrderTaxable")
    private Boolean orderTaxable;

    @Column(name = "OrderStatus")
    private String orderStatus;

    @Column(name = "IsAproved")
    private Boolean isAproved;

    @Column(name = "ModifyDate")
    private LocalDateTime modifyDate;

    @Column(name = "ModifyUser")
    private Integer modifyUser;

    @Column(name = "PostDate")
    private LocalDateTime postDate;

    @Column(name = "PostUser")
    private Integer postUser;

    @Column(name = "PostState")
    private Boolean postState;

    @Column(name = "ProjectsId")
    private Integer projectsId;

    @Column(name = "OrderType")
    private String orderType;

    @Column(name = "CityId")
    private Integer cityId;

    @Column(name = "BrokerAgentId")
    private Integer brokerAgentId;

    @Column(name = "BrokeryType")
    private String brokeryType;

    @Column(name = "BrokeryRate")
    private Double brokeryRate;

    @Column(name = "BrokeryUom")
    private Double brokeryUom;

    @Column(name = "BrokeryAmount")
    private Double brokeryAmount;

    @Column(name = "CashFreight")
    private Boolean cashFreight;

    @Column(name = "CreditFreight")
    private Boolean creditFreight;

    @Column(name = "BillCalculateTypeId")
    private Integer billCalculateTypeId;

    @Column(name = "OrderQty")
    private Double orderQty;

    @Column(name = "OrderWeight")
    private Double orderWeight;

    @Column(name = "OrderAmount")
    private Double orderAmount;

    @Column(name = "CurrencyId")
    private Integer currencyId;

    @Column(name = "ExchangeRate")
    private Double exchangeRate;

    @Column(name = "FcyAmount")
    private Double fcyAmount;

    @Column(name = "OrderStatusRemarks")
    private String orderStatusRemarks;

    @Column(name = "BaseDocumentTypeId")
    private Integer baseDocumentTypeId;

    @Column(name = "AttachmentsValues")
    private String attachmentsValues;

    @Column(name = "CustomAttachmentsValues")
    private String customAttachmentsValues;

    @Column(name = "DeliveryTermId")
    private Integer deliveryTermId;

    @Column(name = "LocationTypeId")
    private Integer locationTypeId;

    @Column(name = "BookingPersonId")
    private Integer bookingPersonId;

    @Column(name = "PermitNo")
    private String permitNo;

    @Column(name = "PermitIssueDate")
    private LocalDateTime permitIssueDate;

    @Column(name = "PermitValidityUpto")
    private LocalDateTime permitValidityUpto;

    @Column(name = "UserLogId")
    private Long userLogId;

    @Column(name = "StatusChangedDate")
    private LocalDateTime statusChangedDate;

    @Column(name = "StatusChangedById")
    private Integer statusChangedById;
}
