package com.mst.models;

import java.io.Serializable;
import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/**
 * Mapped 1:1 onto dbo.PurchaseOrderDetail_H (source: Sp_PurchaseOrderDetail_Insert update-log branch,
 * USP_DeletePurchaseOrderDetailIfNotExistInGrn delete-log). Id is app-assigned: ISNULL(MAX(Id),0)+1.
 */
@Entity
@Setter
@Getter
@Table(name = "PurchaseOrderDetail_H")
public class PurchaseOrderDetail_H implements Serializable {

    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "OrderDetailId")
    private Integer orderDetailId;

    @Column(name = "PurchaseOrderId")
    private Integer purchaseOrderId;

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

    @Column(name = "JobLotId")
    private Integer jobLotId;

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

    @Column(name = "ActionTypeId")
    private Integer actionTypeId;

    @Column(name = "ModifyUserId")
    private Integer modifyUserId;

    @Column(name = "ModifyDate")
    private LocalDateTime modifyDate;

    @Column(name = "RevisionNo")
    private Integer revisionNo;

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

    @Column(name = "EntryDate")
    private LocalDateTime entryDate;

    @Column(name = "EntryUserId")
    private Integer entryUserId;
}
