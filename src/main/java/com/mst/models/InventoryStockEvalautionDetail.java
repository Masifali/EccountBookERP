package com.mst.models;

import java.io.Serializable;
import java.math.BigDecimal;
import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/**
 * Mapped 1:1 onto dbo.InventoryStockEvalautionDetail (source: Sp_InventoryStockEvalautionDetail_Update).
 * This is the per-document-line stock evaluation ledger: every approved/unapproved inventory document
 * (opening balance, purchase/sale invoices, GRN/GDN, production, transfers, adjustments, returns ...)
 * is exploded by the procedure into IN/OUT rows keyed by RefDocumentTypeId + RefDocIdNo (+ RefDocSubIdNo).
 * <p>
 * Column types: where the procedure declares a parameter of the same name its T-SQL type is used
 * (float(53) -> Double, decimal -> BigDecimal, int -> Integer, nvarchar -> String, bit -> Boolean,
 * datetime -> LocalDateTime); the remaining columns follow the name-inference rule of CONVENTIONS.md,
 * except the monetary columns Freight / Commission / OtherExpense / EbTotal / WagesAmount which the
 * procedure only ever fills with numeric expressions and are therefore BigDecimal.
 * Id is never supplied by the procedure (every INSERT omits it) -> IDENTITY.
 */
@Entity
@Setter
@Getter
@Table(name = "InventoryStockEvalautionDetail")
public class InventoryStockEvalautionDetail implements Serializable {

    private static final long serialVersionUID = 1L;

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)   // Id never appears in any INSERT column list of the proc -> DB identity
    @Column(name = "Id")
    private Integer id;

    @Column(name = "RefDocumentTypeId")
    private Integer refDocumentTypeId;

    @Column(name = "RefDocIdNo")
    private Integer refDocIdNo;

    @Column(name = "RefRefDocumentTypeId")
    private Integer refRefDocumentTypeId;

    @Column(name = "RefRefDocIdNo")
    private Integer refRefDocIdNo;

    @Column(name = "RefRefDocSubIdNo")
    private Integer refRefDocSubIdNo;

    @Column(name = "OtherDocumentTypeId")
    private Integer otherDocumentTypeId;

    @Column(name = "OtherDocNoId")
    private Integer otherDocNoId;

    @Column(name = "OtherSubDocNoId")
    private Integer otherSubDocNoId;

    @Column(name = "DocCodeNo")
    private Integer docCodeNo;

    @Column(name = "DocNo")
    private Integer docNo;

    @Column(name = "DocDate")
    private LocalDateTime docDate;

    @Column(name = "SupplierCustomerId")
    private Integer supplierCustomerId;

    @Column(name = "RefPartyId")
    private Integer refPartyId;

    @Column(name = "RefDocSubIdNo")
    private Integer refDocSubIdNo;

    @Column(name = "OrderNo")
    private Integer orderNo;

    @Column(name = "WarehouseId")
    private Integer warehouseId;

    @Column(name = "RefWarehouseId")
    private Integer refWarehouseId;

    @Column(name = "PrdJobOrderNo")
    private Integer prdJobOrderNo;

    @Column(name = "VehicleNo")
    private String vehicleNo;

    @Column(name = "GpNo")
    private Integer gpNo;

    @Column(name = "GpNoDcNo")
    private Integer gpNoDcNo;

    @Column(name = "GPType")
    private String gpType;

    @Column(name = "GpTypeSrNo")
    private Integer gpTypeSrNo;

    @Column(name = "BiltyNo")
    private String biltyNo;

    @Column(name = "TranRemarks")
    private String tranRemarks;

    @Column(name = "ItemId")
    private Integer itemId;

    @Column(name = "ItemName")
    private String itemName;

    @Column(name = "ItemUom")
    private Integer itemUom;

    @Column(name = "CropBatch")
    private String cropBatch;

    @Column(name = "CropYearId")
    private Integer cropYearId;

    @Column(name = "JobLotId")
    private Integer jobLotId;

    @Column(name = "InvPackingTypeId")
    private Integer invPackingTypeId;

    @Column(name = "QtyIn")
    private Double qtyIn;

    @Column(name = "QtyOut")
    private Double qtyOut;

    @Column(name = "WeightIn")
    private BigDecimal weightIn;

    @Column(name = "WeightOut")
    private BigDecimal weightOut;

    @Column(name = "BillWeightIn")
    private Double billWeightIn;

    @Column(name = "BillWeightOut")
    private Double billWeightOut;

    @Column(name = "StockWeightIn")
    private Double stockWeightIn;

    @Column(name = "StockWeightOut")
    private Double stockWeightOut;

    @Column(name = "CalcType")
    private String calcType;

    @Column(name = "ItemRate")
    private Double itemRate;

    @Column(name = "RateUom")
    private Integer rateUom;

    @Column(name = "RateUomId")
    private Integer rateUomId;

    @Column(name = "OtherRate")
    private BigDecimal otherRate;

    @Column(name = "OtherRateUomId")
    private Integer otherRateUomId;

    @Column(name = "AmountIn")
    private Double amountIn;

    @Column(name = "ExpenseAmountIn")
    private Double expenseAmountIn;

    @Column(name = "AmountOut")
    private Double amountOut;

    @Column(name = "CgsRate")
    private Double cgsRate;

    @Column(name = "CgsAmount")
    private Double cgsAmount;

    @Column(name = "CurrencyId")
    private Integer currencyId;

    @Column(name = "ExchangeRate")
    private BigDecimal exchangeRate;

    @Column(name = "FcyAmount")
    private BigDecimal fcyAmount;

    @Column(name = "GrossWeight")
    private BigDecimal grossWeight;

    @Column(name = "EbTotal")
    private BigDecimal ebTotal;

    @Column(name = "AddLessWeight")
    private BigDecimal addLessWeight;

    @Column(name = "EbPurAgainstWeight")
    private Double ebPurAgainstWeight;

    @Column(name = "EbPurAgainstWeightAmount")
    private BigDecimal ebPurAgainstWeightAmount;

    @Column(name = "Freight")
    private BigDecimal freight;

    @Column(name = "FreightDeduction")
    private BigDecimal freightDeduction;

    @Column(name = "Commission")
    private BigDecimal commission;

    @Column(name = "OtherExpense")
    private BigDecimal otherExpense;

    @Column(name = "WagesAmount")
    private BigDecimal wagesAmount;

    @Column(name = "GrnGdnId")
    private Integer grnGdnId;

    @Column(name = "GrnGdnNo")
    private Integer grnGdnNo;

    @Column(name = "GrnGdnDate")
    private LocalDateTime grnGdnDate;

    @Column(name = "GrnGdnDetailId")
    private Integer grnGdnDetailId;

    @Column(name = "IsOnHold")
    private Boolean isOnHold;

    @Column(name = "HoldStatus")
    private String holdStatus;

    @Column(name = "StockHoldUserId")
    private Integer stockHoldUserId;

    @Column(name = "ValidForSale")
    private Boolean validForSale;

    @Column(name = "IsApproved")
    private Boolean isApproved;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "BranchesId")
    private Integer branchesId;

    @Column(name = "ProjectsId")
    private Integer projectsId;

    @Column(name = "CityId")
    private Integer cityId;

    @Column(name = "WorkOrderId")
    private Integer workOrderId;

    @Column(name = "BatchNo")
    private String batchNo;

    @Column(name = "ProductionStageId")
    private Integer productionStageId;

    @Column(name = "WorkStationId")
    private Integer workStationId;

    @Column(name = "VarientId")
    private Integer varientId;

    @Column(name = "CastingTypeId")
    private Integer castingTypeId;

    @Column(name = "ItemConditionId")
    private Integer itemConditionId;

    @Column(name = "RackId")
    private Integer rackId;

    @Column(name = "SecondaryUomId")
    private Integer secondaryUomId;

    @Column(name = "SecondaryUomQty")
    private BigDecimal secondaryUomQty;

    @Column(name = "SecondaryUomItemRate")
    private BigDecimal secondaryUomItemRate;

    @Column(name = "labIPmActivityLogId")
    private Integer labIPmActivityLogId;

    @Column(name = "InventoryParentCategoriesId")
    private Integer inventoryParentCategoriesId;

    @Column(name = "Mno")
    private Integer mno;

    @Column(name = "RNo")
    private Integer rNo;

    @Column(name = "EntryDate")
    private LocalDateTime entryDate;

    @Column(name = "EntryUser")
    private Integer entryUser;

    @Column(name = "ModifyDate")
    private LocalDateTime modifyDate;

    @Column(name = "ModifyUser")
    private Integer modifyUser;

}
