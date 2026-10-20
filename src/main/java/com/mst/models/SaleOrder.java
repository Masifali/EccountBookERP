package com.mst.models;

import java.io.Serializable;
import java.math.BigDecimal;
import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/**
 * Mapped 1:1 onto dbo.SaleOrder (source: Sp_SaleOrder_GetAllMethod, Sp_SalesSaleOrder_RiceAndPaddyRegister_Rpt).
 * No Insert proc in this batch; Id kept app-assigned (plain @Id) - see repository maxId().
 * Column types were not given by the registry and are inferred from column names
 * (SupplierRefNo / ExportInvoiceNo / ExportOrderNo kept as String because they are reference numbers, not ints).
 */
@Entity
@Setter
@Getter
@Table(name = "SaleOrder")
public class SaleOrder implements Serializable {

    private static final long serialVersionUID = 1L;

    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "DocNo")
    private Integer docNo;

    @Column(name = "OrderCatagoryId")
    private Integer orderCatagoryId;

    @Column(name = "DocDate")
    private LocalDateTime docDate;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "DocumentTypeId")
    private Integer documentTypeId;

    @Column(name = "ItemId")
    private Integer itemId;

    @Column(name = "ItemRate")
    private BigDecimal itemRate;

    @Column(name = "RateUom")
    private String rateUom;

    @Column(name = "BillWeightIn")
    private String billWeightIn;

    @Column(name = "ModifyUser")
    private Integer modifyUser;

    @Column(name = "PaymentTermsId")
    private Integer paymentTermsId;

    @Column(name = "OrderSupCustId")
    private Integer orderSupCustId;

    @Column(name = "IsAproved")
    private Boolean isAproved;

    @Column(name = "BranchSrNo")
    private Integer branchSrNo;

    @Column(name = "BranchesId")
    private Integer branchesId;

    @Column(name = "CommissionType")
    private String commissionType;

    @Column(name = "CommRate")
    private BigDecimal commRate;

    @Column(name = "CommAmount")
    private BigDecimal commAmount;

    @Column(name = "ItemAmountHeader")
    private BigDecimal itemAmountHeader;

    @Column(name = "DiscountAmountHeader")
    private BigDecimal discountAmountHeader;

    @Column(name = "TaxAmountHeader")
    private BigDecimal taxAmountHeader;

    @Column(name = "OrderAmount")
    private BigDecimal orderAmount;

    @Column(name = "SupplierRefNo")
    private String supplierRefNo;

    @Column(name = "OrderQty")
    private BigDecimal orderQty;

    @Column(name = "OrderWeight")
    private BigDecimal orderWeight;

    @Column(name = "CommissionRemarks")
    private String commissionRemarks;

    @Column(name = "OrderDueDays")
    private Integer orderDueDays;

    @Column(name = "OrderDueDate")
    private LocalDateTime orderDueDate;

    @Column(name = "OrderExpiryDate")
    private LocalDateTime orderExpiryDate;

    @Column(name = "DeliveryTerm")
    private String deliveryTerm;

    @Column(name = "DeliveryDays")
    private Integer deliveryDays;

    @Column(name = "RemarksHeader")
    private String remarksHeader;

    @Column(name = "OrderStatus")
    private String orderStatus;

    @Column(name = "EntryUser")
    private Integer entryUser;

    @Column(name = "EntryDate")
    private LocalDateTime entryDate;

    @Column(name = "ModifyDate")
    private LocalDateTime modifyDate;

    @Column(name = "PostUser")
    private Integer postUser;

    @Column(name = "PostDate")
    private LocalDateTime postDate;

    @Column(name = "OtherCommissionAgentId")
    private Integer otherCommissionAgentId;

    @Column(name = "OtherCommissionType")
    private String otherCommissionType;

    @Column(name = "OtherCommissionUom")
    private String otherCommissionUom;

    @Column(name = "OtherCommissionRate")
    private BigDecimal otherCommissionRate;

    @Column(name = "OtherCommissionAmount")
    private BigDecimal otherCommissionAmount;

    @Column(name = "OtherCommissionRemarks")
    private String otherCommissionRemarks;

    @Column(name = "OrderTypeId")
    private Integer orderTypeId;

    @Column(name = "OtherCategoryId")
    private Integer otherCategoryId;

    @Column(name = "BookingPersonId")
    private Integer bookingPersonId;

    @Column(name = "FinancialYearId")
    private Integer financialYearId;

    @Column(name = "RecordNo")
    private Integer recordNo;

    @Column(name = "DueDays")
    private Integer dueDays;

    @Column(name = "DueDate")
    private LocalDateTime dueDate;

    @Column(name = "NoOfAttachments")
    private Integer noOfAttachments;

    @Column(name = "EntryUserId")
    private Integer entryUserId;

    @Column(name = "BrokerAgentSupCustId")
    private Integer brokerAgentSupCustId;

    @Column(name = "UomScheduleIdCmRate")
    private Integer uomScheduleIdCmRate;

    @Column(name = "ActionId")
    private Integer actionId;

    @Column(name = "OrderType")
    private String orderType;

    @Column(name = "CatagorySrNo")
    private Integer catagorySrNo;

    @Column(name = "RefrenenceParty")
    private Integer refrenenceParty;

    @Column(name = "SupplierCustomerIdStockParty")
    private Integer supplierCustomerIdStockParty;

    @Column(name = "DeliveryStartDate")
    private LocalDateTime deliveryStartDate;

    @Column(name = "DeliveryRemarks")
    private String deliveryRemarks;

    @Column(name = "OrderTaxable")
    private Boolean orderTaxable;

    @Column(name = "PostState")
    private Boolean postState;

    @Column(name = "ProjectsId")
    private Integer projectsId;

    @Column(name = "ExportInvoiceNo")
    private String exportInvoiceNo;

    @Column(name = "ExportOrderNo")
    private String exportOrderNo;

    @Column(name = "ExchangeRate")
    private BigDecimal exchangeRate;

    @Column(name = "CurrencyId")
    private Integer currencyId;

    @Column(name = "FcyAmount")
    private BigDecimal fcyAmount;

    @Column(name = "NoOfCntnr")
    private Integer noOfCntnr;

    @Column(name = "ExportLotRef")
    private String exportLotRef;

    @Column(name = "PfiScId")
    private Integer pfiScId;

    @Column(name = "IsValidate")
    private Boolean isValidate;

    @Column(name = "BillCalculateTypeId")
    private Integer billCalculateTypeId;

    @Column(name = "OrderStatusRemarks")
    private String orderStatusRemarks;

    @Column(name = "AttachmentsValues")
    private String attachmentsValues;

    @Column(name = "CustomAttachmentsValues")
    private String customAttachmentsValues;

    @Column(name = "OrderId")
    private Integer orderId;
}
