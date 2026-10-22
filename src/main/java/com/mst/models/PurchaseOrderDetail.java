package com.mst.models;

import javax.persistence.*;
import lombok.Data;

import java.time.LocalDateTime;

/**
 * Mapped 1:1 onto dbo.PurchaseOrderDetail.
 * Existing entity (copied from SRC_REF) extended by batch E_purchase_order with every column seen in
 * Sp_PurchaseOrderDetail_Insert / USP_DeletePurchaseOrderDetailIfNotExistInGrn / Sp_PurchaseOrder_GetAllMethod.
 * Existing fields/annotations are untouched; new columns are appended at the end.
 * NOTE: Sp_PurchaseOrderDetail_Insert assigns Id = ISNULL(MAX(Id),0)+1 (see PurchaseOrderDetailJpaRepository.maxId());
 * the existing @GeneratedValue(IDENTITY) strategy is kept unchanged as required by the conventions.
 */
@Entity
@Table(name = "PurchaseOrderDetail")
@Data
public class PurchaseOrderDetail {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "PurchaseOrderId", nullable = false)
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

    @Column(name = "ItemUomId")
    private Integer itemUomId;

    @Column(name = "GrossWeight")
    private Double grossWeight;

    @Column(name = "ItemRate")
    private Double itemRate;

    @Column(name = "ItemAmount")
    private Double itemAmount;

    @Column(name = "WarehouseId")
    private Integer warehouseId;

    @Column(name = "CommentsDetail", length = 250)
    private String commentsDetail;

    // ---------------------------------------------------------------------------------------------
    // Columns added by batch E_purchase_order (Sp_PurchaseOrderDetail_Insert INSERT/UPDATE lists,
    // USP_DeletePurchaseOrderDetailIfNotExistInGrn and GetAllMethod 'ReadByPurchaseOrderHeaderId').
    // ---------------------------------------------------------------------------------------------

    @Column(name = "OrderItemId")
    private Integer orderItemId;

    @Column(name = "OrderItemUOMId")
    private Integer orderItemUomId;

    @Column(name = "OrderItemQty")
    private Double orderItemQty;

    @Column(name = "NetWeight")
    private Double netWeight;

    @Column(name = "OrderItemRate")
    private Double orderItemRate;

    @Column(name = "OrderItemRateUOMId")
    private Integer orderItemRateUomId;

    @Column(name = "Amount")
    private Double amount;

    @Column(name = "CityArea")
    private String cityArea;

    @Column(name = "LabSampleNo")
    private String labSampleNo;

    @Column(name = "AmountCalcType")
    private String amountCalcType;

    @Column(name = "TaxableStatus")
    private String taxableStatus;

    @Column(name = "OrderRemarks")
    private String orderRemarks;

    @Column(name = "Crop")
    private String crop;

    @Column(name = "BagWeight")
    private Double bagWeight;

    @Column(name = "BagPrice")
    private Double bagPrice;

    @Column(name = "Moisture")
    private String moisture;

    @Column(name = "TaxAmount")
    private Double taxAmount;

    @Column(name = "LineId")
    private Integer lineId;

    @Column(name = "RetailRate")
    private Double retailRate;

    @Column(name = "TaxNameId")
    private Integer taxNameId;

    @Column(name = "TaxPercent")
    private Double taxPercent;

    @Column(name = "TotalAmount")
    private Double totalAmount;

    @Column(name = "ReviseRemarks")
    private String reviseRemarks;

    @Column(name = "InvLabSampleAnalysisHeaderId")
    private Integer invLabSampleAnalysisHeaderId;

    @Column(name = "WeightPerQty")
    private Double weightPerQty;

    @Column(name = "CurrencyId")
    private Integer currencyId;

    @Column(name = "ExchangeRate")
    private Double exchangeRate;

    @Column(name = "FcyAmount")
    private Double fcyAmount;

    @Column(name = "CityId")
    private Integer cityId;

    @Column(name = "PackingDate")
    private LocalDateTime packingDate;

    @Column(name = "ExpiryDate")
    private LocalDateTime expiryDate;

    @Column(name = "CropYearId")
    private Integer cropYearId;

    @Column(name = "LeadTime")
    private Integer leadTime;

    @Column(name = "WeightCapacity")
    private String weightCapacity;

    @Column(name = "RefDocumentTypeId")
    private Integer refDocumentTypeId;

    @Column(name = "RefDocId")
    private Integer refDocId;

    @Column(name = "RefDocInvoiceId")
    private Integer refDocInvoiceId;

    @Column(name = "LabAnalysisStandardScheduleId")
    private Integer labAnalysisStandardScheduleId;

    @Column(name = "ModifyDate")
    private LocalDateTime modifyDate;

    @Column(name = "ModifyUserId")
    private Integer modifyUserId;

    @Column(name = "ActionTypeId")
    private Integer actionTypeId;

    @Column(name = "EntryDate")
    private LocalDateTime entryDate;

    @Column(name = "EntryUserId")
    private Integer entryUserId;

    @Column(name = "RevisionNo")
    private Integer revisionNo;
}
