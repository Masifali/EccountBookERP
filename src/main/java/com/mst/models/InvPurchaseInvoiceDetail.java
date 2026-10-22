package com.mst.models;

import javax.persistence.*;
import lombok.Data;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;

/** Mapped 1:1 onto dbo.InvPurchaseInvoiceDetail (source: Sp_InvPurchaseInvoiceDetail_Insert, usp_PurchaseInvoiceDetailRemoveByDetailIdsAndValidations, Sp_InvoicesVouchersandStocksDelete). Id is SCOPE_IDENTITY(). */
@Entity
@Table(name = "InvPurchaseInvoiceDetail")
@Data
public class InvPurchaseInvoiceDetail {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "InvPurchaseInvoiceId", nullable = false)
    private Integer invPurchaseInvoiceId;

    @Column(name = "InvGrnId")
    private Integer invGrnId;

    @Column(name = "PurchaseOrderId")
    private Integer purchaseOrderId;

    @Column(name = "ItemId", nullable = false)
    private Integer itemId;

    @Column(name = "PackingTypeId")
    private Integer packingTypeId;

    @Column(name = "CropYear", length = 50)
    private String cropYear;

    @Column(name = "JobLotId")
    private Integer jobLotId;

    @Column(name = "ItemQty")
    private Double itemQty;

    @Column(name = "GrossWeight")
    private Double grossWeight;

    @Column(name = "NetBillWeight")
    private Double netBillWeight;

    @Column(name = "ItemRate")
    private Double itemRate;

    @Column(name = "ItemAmount")
    private Double itemAmount;

    @Column(name = "WarehouseId")
    private Integer warehouseId;

    @Column(name = "RemarksDetail", length = 250)
    private String remarksDetail;

    // ---- columns added by batch G2_purchase_invoice_write (registry + procedures) ----

    @Column(name = "ActionTypeId")
    private Integer actionTypeId;

    @Column(name = "FreightVoucherId")
    private Integer freightVoucherId;

    @Column(name = "ShortageAmount")
    private BigDecimal shortageAmount;

    @Column(name = "AddLessAmount")
    private BigDecimal addLessAmount;

    @Column(name = "ChargeToParty")
    private String chargeToParty;

    @Column(name = "OtherDeduction")
    private BigDecimal otherDeduction;

    @Column(name = "DeductedAmount")
    private BigDecimal deductedAmount;

    @Column(name = "TotalPayableAmount")
    private BigDecimal totalPayableAmount;

    @Column(name = "DiscountAmount")
    private Double discountAmount;

    @Column(name = "ChargeToPartyAmount")
    private BigDecimal chargeToPartyAmount;

    @Column(name = "PaidAmount")
    private BigDecimal paidAmount;

    @Column(name = "GatePassId")
    private Integer gatePassId;

    @Column(name = "RefDocumentTypeId")
    private Integer refDocumentTypeId;

    @Column(name = "RefDocumentNo")
    private String refDocumentNo;

    @Column(name = "ReferenceDocumentTypeId")
    private Integer referenceDocumentTypeId;

    @Column(name = "ReferenceDocumentId")
    private Integer referenceDocumentId;

    @Column(name = "WbTicketId")
    private Integer wbTicketId;

    @Column(name = "InvPurchaseDemandHeaderId")
    private Integer invPurchaseDemandHeaderId;

    @Column(name = "ReceivedWeight")
    private BigDecimal receivedWeight;

    @Column(name = "InvGrnDetailId")
    private Integer invGrnDetailId;

    @Column(name = "EBWeight")
    private Double ebWeight;

    @Column(name = "EBTotalWt")
    private Double ebTotalWt;

    @Column(name = "AdLsWeight")
    private Double adLsWeight;

    @Column(name = "NetStockWeight")
    private Double netStockWeight;

    @Column(name = "UomScheduleIdRate")
    private Integer uomScheduleIdRate;

    @Column(name = "LabAnalisysNo")
    private String labAnalisysNo;

    @Column(name = "GpDate")
    private LocalDate gpDate;

    @Column(name = "IsTaxable")
    private String isTaxable;

    @Column(name = "TaxDescriptions")
    private String taxDescriptions;

    @Column(name = "TaxAmount")
    private Double taxAmount;

    @Column(name = "ItemCgsRate")
    private Double itemCgsRate;

    @Column(name = "AvgCgsRate")
    private Double avgCgsRate;

    @Column(name = "RateCut")
    private Double rateCut;

    @Column(name = "RateCutAmount")
    private Double rateCutAmount;

    @Column(name = "BillAmount")
    private Double billAmount;

    @Column(name = "FreightAmount")
    private Double freightAmount;

    @Column(name = "ExpenseAmount")
    private Double expenseAmount;

    @Column(name = "JournalAmount")
    private Double journalAmount;

    @Column(name = "CommissionAmount")
    private Double commissionAmount;

    @Column(name = "WeightCut")
    private Double weightCut;

    @Column(name = "WeightCutTotal")
    private Double weightCutTotal;

    @Column(name = "GpNo")
    private Integer gpNo;

    @Column(name = "VehicleNo")
    private String vehicleNo;

    @Column(name = "TaxNameId")
    private Integer taxNameId;

    @Column(name = "TaxPercent")
    private Double taxPercent;

    @Column(name = "Brokery")
    private Double brokery;

    @Column(name = "ScaleKart")
    private Double scaleKart;

    @Column(name = "PurchaseOrderDetailId")
    private Integer purchaseOrderDetailId;

    @Column(name = "OtherRate")
    private Double otherRate;

    @Column(name = "OtherRateUomId")
    private Integer otherRateUomId;

    @Column(name = "LineId")
    private Integer lineId;

    @Column(name = "CurrencyId")
    private Integer currencyId;

    @Column(name = "ExchangeRate")
    private BigDecimal exchangeRate;

    @Column(name = "FcyAmount")
    private BigDecimal fcyAmount;

    @Column(name = "EntryDate")
    private LocalDateTime entryDate;

    @Column(name = "EntryUserId")
    private Integer entryUserId;

    @Column(name = "RevisionNo")
    private Integer revisionNo;

    @Column(name = "WagesAmount")
    private Double wagesAmount;

    @Column(name = "AttributeVariantId")
    private Integer attributeVariantId;

    @Column(name = "EbPurAgainstWeight")
    private Double ebPurAgainstWeight;

    @Column(name = "BranchId")
    private Integer branchId;

    @Column(name = "PackingDate")
    private LocalDateTime packingDate;

    @Column(name = "ExpiryDate")
    private LocalDateTime expiryDate;

    @Column(name = "CropYearId")
    private Integer cropYearId;

    @Column(name = "RefDocId")
    private Integer refDocId;

    @Column(name = "RefDocInvoiceId")
    private Integer refDocInvoiceId;

    @Column(name = "RefDocSubId")
    private Integer refDocSubId;

    @Column(name = "DiscountTypeId")
    private Integer discountTypeId;

    @Column(name = "DiscountPercent")
    private Double discountPercent;

    @Column(name = "CityId")
    private Integer cityId;

    @Column(name = "CastingTypeId")
    private Integer castingTypeId;

    @Column(name = "ProductionStageId")
    private Integer productionStageId;

    @Column(name = "GdnDocumentTypeId")
    private Integer gdnDocumentTypeId;

    @Column(name = "GdnId")
    private Integer gdnId;

    @Column(name = "GdnDetailId")
    private Integer gdnDetailId;

    @Column(name = "LocationTypeId")
    private Integer locationTypeId;

    @Column(name = "EbPurAgainstWeightAmount")
    private Double ebPurAgainstWeightAmount;

    @Column(name = "FreightDeduction")
    private Double freightDeduction;

    @Column(name = "RateCutForExcessWeight")
    private Double rateCutForExcessWeight;

    @Column(name = "RateCutAmountForExcessWeight")
    private Double rateCutAmountForExcessWeight;

    @Column(name = "ExcessWeight")
    private Double excessWeight;

    @Column(name = "ItemConditionId")
    private Integer itemConditionId;

    @Column(name = "RackId")
    private Integer rackId;

    @Column(name = "ItemAmountWithoutDiscount")
    private BigDecimal itemAmountWithoutDiscount;

    @Column(name = "ModifyUserId")
    private Integer modifyUserId;

    @Column(name = "LabPurchaseId")
    private Integer labPurchaseId;

    @Column(name = "RefDocNo")
    private String refDocNo;

    @Column(name = "RefDocInvoiceNo")
    private String refDocInvoiceNo;

    @Column(name = "RefInvoiceQty")
    private BigDecimal refInvoiceQty;

    @Column(name = "RefInvoiceWeight")
    private BigDecimal refInvoiceWeight;

    @Column(name = "DiscountType")
    private String discountType;

    @Column(name = "PurchaseOrderDocumentTypeId")
    private Integer purchaseOrderDocumentTypeId;

    @Column(name = "PurchaseOrderQty")
    private BigDecimal purchaseOrderQty;

    @Column(name = "PurchaseDemandDocumentTypeId")
    private Integer purchaseDemandDocumentTypeId;

    @Column(name = "PurchaseDemandId")
    private Integer purchaseDemandId;

    @Column(name = "PurchaseDemandDetailId")
    private Integer purchaseDemandDetailId;

    @Column(name = "PurchaseDemandNo")
    private Integer purchaseDemandNo;

    @Column(name = "PurchaseDemandQty")
    private BigDecimal purchaseDemandQty;

    @Column(name = "PreBillDocumentTypeId")
    private Integer preBillDocumentTypeId;

    @Column(name = "PreBillId")
    private Integer preBillId;

    @Column(name = "PreBillDetailId")
    private Integer preBillDetailId;

    @Column(name = "PreBillNo")
    private Integer preBillNo;

    @Column(name = "PreBillQty")
    private BigDecimal preBillQty;

    @Column(name = "DeliveryChallanDocumentTypeId")
    private Integer deliveryChallanDocumentTypeId;

    @Column(name = "DeliveryChallanId")
    private Integer deliveryChallanId;

    @Column(name = "DeliveryChallanDetailId")
    private Integer deliveryChallanDetailId;

    @Column(name = "DeliveryChallanNo")
    private Integer deliveryChallanNo;

    @Column(name = "DeliveryChallanQty")
    private BigDecimal deliveryChallanQty;

    @Column(name = "GrnDocumentTypeId")
    private Integer grnDocumentTypeId;

    @Column(name = "GrnQty")
    private BigDecimal grnQty;

    @Column(name = "NoOfAttachmentsPO")
    private Integer noOfAttachmentsPO;

    @Column(name = "NoOfAttachmentsPD")
    private Integer noOfAttachmentsPD;

    @Column(name = "NoOfAttachmentsPreBill")
    private Integer noOfAttachmentsPreBill;

    @Column(name = "NoOfAttachmentsDC")
    private Integer noOfAttachmentsDC;

    @Column(name = "NoOfAttachmentsGrn")
    private Integer noOfAttachmentsGrn;

    @Column(name = "InvSaleInvoiceId")
    private Integer invSaleInvoiceId;

    @Column(name = "InvGdnId")
    private Integer invGdnId;

    @Column(name = "VoucherHeadId")
    private Integer voucherHeadId;

    @Column(name = "TaxTypeId")
    private Integer taxTypeId;

    @Column(name = "EBWPerUnit")
    private BigDecimal ebwPerUnit;

    @Column(name = "EBWTotal")
    private BigDecimal ebwTotal;

    @Column(name = "WtCut")
    private BigDecimal wtCut;

    @Column(name = "WtCutTotal")
    private BigDecimal wtCutTotal;

    @Column(name = "StockWeight")
    private BigDecimal stockWeight;

    @Column(name = "AreaCity")
    private String areaCity;

    @Column(name = "PurchaseOrder")
    private String purchaseOrder;

    @Column(name = "OrderDate")
    private LocalDateTime orderDate;

    @Column(name = "GrnNo")
    private Integer grnNo;

    @Column(name = "GrnDate")
    private LocalDateTime grnDate;

    @Column(name = "DocumentTypeId")
    private Integer documentTypeId;

    @Column(name = "DetailId")
    private Integer detailId;

    @Column(name = "ItemUOMId")
    private Integer itemUomId;

    @Column(name = "ModifyDate")
    private LocalDateTime modifyDate;
}
