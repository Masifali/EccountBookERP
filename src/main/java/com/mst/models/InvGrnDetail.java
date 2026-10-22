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
 * Mapped 1:1 onto dbo.InvGrnDetail (GRN detail lines).
 * Source: Sp_InvGrnDetail_Insert / Sp_InvGrnDetail_GetAllMethod / Sp_InvGRN_GetAllMethod / USp_InvGrnStore_Register.
 * Id is app-assigned (Sp_InvGrnDetail_Insert computes ISNULL(MAX(Id),0)+1), so no @GeneratedValue.
 *
 * NOTE: the registry also lists OrderItemQty, ActionTypeId, DocNo, OrderQty, ApprovedQty, InvPurchaseDemandHeaderId,
 * DocDate, DemandQty, Qty, DeliveryChallanHeaderId, ChallanQty, PurchasePreBillDetailId, PurchasePreBillHeaderId,
 * PurchasePreBillQty, PurchaseDemandHeaderId, PurchaseDemandDetailId, PreBillQty, GrnQty, PurchaseOrderHeaderId,
 * PurchaseOrder, PurchaseDemandNo, PurchaseDemandDocDate, PurchaseDemandQty, UsedDemandQtyInPreBill,
 * UsedDemandQtyInGrn, PurchaseOrderDate, PurchaseOrderQty, UsedOrderQtyInGrn, UsedOrderQtyInPreBill,
 * PurchasePreBillId, PurchasePreBillNo, DeliveryChallanNo, DeliveryChallanDate, DeliveryChallanQty,
 * UsedDeliveryChallanQtyGrn, InvPurchaseInvoiceId, InvGrnDetailId, InvGdnId, ReceivedQty, TypeId for "InvGrnDetail".
 * In the procedures those are columns of the temp table #PurchasePreBillDetail (ReadByInvGrnIDStore) or of
 * PurchaseOrderDetail / InvPurchaseDemandDetail / DeliveryChallanDetail / InvPurchaseInvoiceDetail, NOT real columns
 * of dbo.InvGrnDetail (the INSERT in Sp_InvGrnDetail_Insert is the authoritative column list), so they are
 * intentionally not mapped. They are exposed through the InvGrnDetail*Row projections instead.
 */
@Entity
@Setter
@Getter
@Table(name = "InvGrnDetail")
public class InvGrnDetail implements Serializable {

    private static final long serialVersionUID = 1L;

    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "InvGrnId")
    private Integer invGrnId;

    @Column(name = "PurchaseOrderId")
    private Integer purchaseOrderId;

    @Column(name = "ItemId")
    private Integer itemId;

    @Column(name = "PackingTypeId")
    private Integer packingTypeId;

    @Column(name = "CropYear")
    private String cropYear;

    @Column(name = "JobLotId")
    private Integer jobLotId;

    @Column(name = "ItemQty")
    private Double itemQty;

    @Column(name = "ItemUomId")
    private Integer itemUomId;

    @Column(name = "GrossWeight")
    private Double grossWeight;

    @Column(name = "EBWPerUnit")
    private Double ebwPerUnit;

    @Column(name = "EBWTotal")
    private Double ebwTotal;

    @Column(name = "WtCut")
    private Double wtCut;

    @Column(name = "WtCutTotal")
    private Double wtCutTotal;

    @Column(name = "AdLsWeight")
    private Double adLsWeight;

    @Column(name = "NetBillWeight")
    private Double netBillWeight;

    @Column(name = "StockWeight")
    private Double stockWeight;

    @Column(name = "WarehouseId")
    private Integer warehouseId;

    @Column(name = "LabReportRef")
    private String labReportRef;

    @Column(name = "AreaCity")
    private String areaCity;

    @Column(name = "CommentsDetail")
    private String commentsDetail;

    @Column(name = "SupplySchedulId")
    private Integer supplySchedulId;

    @Column(name = "GatePassInwarDetailId")
    private Integer gatePassInwarDetailId;

    @Column(name = "InvPurchasedemondId")
    private Integer invPurchasedemondId;

    @Column(name = "PurchaseOrderDetailId")
    private Integer purchaseOrderDetailId;

    @Column(name = "PurchaserOrderNo")
    private Integer purchaserOrderNo;

    @Column(name = "ContractorId")
    private Integer contractorId;

    @Column(name = "ScaleKart")
    private Double scaleKart;

    @Column(name = "PurchaseDemondDetailId")
    private Integer purchaseDemondDetailId;

    @Column(name = "EBCutApprovedUserId")
    private Integer ebCutApprovedUserId;

    @Column(name = "EBCutDate")
    private LocalDateTime ebCutDate;

    @Column(name = "EBCutApprovedUserRemarks")
    private String ebCutApprovedUserRemarks;

    @Column(name = "CityId")
    private Integer cityId;

    @Column(name = "GdnId")
    private Integer gdnId;

    @Column(name = "GdnDetailId")
    private Integer gdnDetailId;

    @Column(name = "AssetId")
    private Integer assetId;

    @Column(name = "ConditionId")
    private Integer conditionId;

    @Column(name = "WareHouseFromId")
    private Integer wareHouseFromId;

    @Column(name = "EbPurAgainstWeight")
    private Double ebPurAgainstWeight;

    @Column(name = "WbTicketId")
    private Integer wbTicketId;

    @Column(name = "RefDocumentTypeId")
    private Integer refDocumentTypeId;

    @Column(name = "RefDocNoId")
    private Integer refDocNoId;

    @Column(name = "RefDocSubIdNo")
    private Integer refDocSubIdNo;

    @Column(name = "LineId")
    private Integer lineId;

    @Column(name = "ScaleShortWeight")
    private Double scaleShortWeight;

    @Column(name = "SupplierShortWeight")
    private Double supplierShortWeight;

    @Column(name = "WeightCutOnId")
    private Integer weightCutOnId;

    @Column(name = "FreightAmount")
    private Double freightAmount;

    @Column(name = "LabId")
    private Integer labId;

    @Column(name = "QtyForWtCut")
    private Double qtyForWtCut;

    @Column(name = "PackingDate")
    private LocalDateTime packingDate;

    @Column(name = "ExpiryDate")
    private LocalDateTime expiryDate;

    @Column(name = "CropYearId")
    private Integer cropYearId;

    @Column(name = "GdnDocumentTypeId")
    private Integer gdnDocumentTypeId;

    @Column(name = "SupplierQty")
    private Double supplierQty;

    @Column(name = "StockEbUnit")
    private Double stockEbUnit;

    @Column(name = "StockEbTotal")
    private Double stockEbTotal;

    @Column(name = "DeliveryChallanId")
    private Integer deliveryChallanId;

    @Column(name = "DeliveryChallanDetailId")
    private Integer deliveryChallanDetailId;

    @Column(name = "ItemConditionId")
    private Integer itemConditionId;

    @Column(name = "RackId")
    private Integer rackId;

    @Column(name = "GainLossWeightDetail")
    private BigDecimal gainLossWeightDetail;
}
