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
 * Mapped 1:1 onto dbo.InventoryTransactions (source: Sp_InventoryTransactions_GetALLMethod).
 * The proc never supplies Id (every section is INSERT ... SELECT without Id) -> IDENTITY.
 * Types follow the proc parameter declarations (FLOAT -> Double, DECIMAL(18,2) -> BigDecimal, BIT -> Boolean);
 * columns the proc does not declare are inferred from the column name / the value written into them.
 */
@Entity
@Setter
@Getter
@Table(name = "InventoryTransactions")
public class InventoryTransactions implements Serializable {

    private static final long serialVersionUID = 1L;

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "BranchesId")
    private Integer branchesId;

    @Column(name = "ProjectsId")
    private Integer projectsId;

    @Column(name = "RefDocumentTypeId")
    private Integer refDocumentTypeId;

    @Column(name = "RefDocIdNo")
    private Integer refDocIdNo;

    @Column(name = "DocCodeNo")
    private Integer docCodeNo;

    @Column(name = "DocDate")
    private LocalDateTime docDate;

    @Column(name = "SupplierCustomerId")
    private Integer supplierCustomerId;

    @Column(name = "IsApproved")
    private Boolean isApproved;

    @Column(name = "RefDocSubIdNo")
    private Integer refDocSubIdNo;

    @Column(name = "WarehouseId")
    private Integer warehouseId;

    @Column(name = "JobLotId")
    private Integer jobLotId;

    @Column(name = "ItemId")
    private Integer itemId;

    @Column(name = "ItemUom")
    private Integer itemUom;

    @Column(name = "CropBatch")
    private String cropBatch;

    @Column(name = "InvPackingTypeId")
    private Integer invPackingTypeId;

    @Column(name = "QtyIn")
    private Double qtyIn;

    @Column(name = "QtyOut")
    private Double qtyOut;

    @Column(name = "BillWeightIn")
    private Double billWeightIn;

    @Column(name = "BillWeightOut")
    private Double billWeightOut;

    @Column(name = "StockWeightIn")
    private Double stockWeightIn;

    @Column(name = "StockWeightOut")
    private Double stockWeightOut;

    /** 'Qty' / 'Weight' / 'QTY' / 'ConfirmQty' */
    @Column(name = "CalcType")
    private String calcType;

    @Column(name = "ItemRate")
    private Double itemRate;

    @Column(name = "RateUom")
    private Integer rateUom;

    @Column(name = "AmountIn")
    private Double amountIn;

    @Column(name = "ExpenseAmountIn")
    private Double expenseAmountIn;

    @Column(name = "AmountOut")
    private Double amountOut;

    /** Section label, e.g. 'Goods Receipts Notes', 'Sale Invoice Direct', 'Production OutPut' */
    @Column(name = "TranRemarks")
    private String tranRemarks;

    @Column(name = "EntryDate")
    private LocalDateTime entryDate;

    @Column(name = "EntryUserId")
    private Integer entryUserId;

    @Column(name = "ModifyDate")
    private LocalDateTime modifyDate;

    @Column(name = "ModifyUserId")
    private Integer modifyUserId;

    @Column(name = "GrossWeight")
    private Double grossWeight;

    @Column(name = "EbUnit")
    private Double ebUnit;

    @Column(name = "EbTotal")
    private Double ebTotal;

    @Column(name = "WtCut")
    private Double wtCut;

    @Column(name = "WtCutTotal")
    private Double wtCutTotal;

    @Column(name = "AddLess")
    private Double addLess;

    @Column(name = "GpNo")
    private Integer gpNo;

    @Column(name = "VehicleNo")
    private String vehicleNo;

    @Column(name = "VarientId")
    private Integer varientId;

    @Column(name = "ProductionStageId")
    private Integer productionStageId;

    @Column(name = "WorkOrderId")
    private Integer workOrderId;

    @Column(name = "WorkStationId")
    private Integer workStationId;

    @Column(name = "CastingTypeId")
    private Integer castingTypeId;

    @Column(name = "SecondaryUomId")
    private Integer secondaryUomId;

    @Column(name = "SecondaryUomQty")
    private BigDecimal secondaryUomQty;

    @Column(name = "SecondaryUomItemRate")
    private BigDecimal secondaryUomItemRate;

    @Column(name = "CityId")
    private Integer cityId;

    @Column(name = "EbPurAgainstWeight")
    private BigDecimal ebPurAgainstWeight;

    /** Copied from GatePassInward.GatepassType / GatePassOutward.GatepassType (String in InwardGatePass entity). */
    @Column(name = "GPType")
    private String gpType;

    @Column(name = "GpTypeSrNo")
    private Integer gpTypeSrNo;

    @Column(name = "IsOnHold")
    private Boolean isOnHold;

    /** 'ACTIVE' or '' */
    @Column(name = "HoldStatus")
    private String holdStatus;

    @Column(name = "StockHoldUserId")
    private Integer stockHoldUserId;

    @Column(name = "JobOrderId")
    private Integer jobOrderId;

    /** Written as 1 by the GDN / sale / export / production-output sections. */
    @Column(name = "ValidForSale")
    private Boolean validForSale;

    @Column(name = "labIPmActivityLogId")
    private Integer labIPmActivityLogId;

    /** Written from Mfg.ProductionHeader.batchCode / Mfg.InvStockTransferDetail.BatchDescription (text). */
    @Column(name = "BatchNo")
    private String batchNo;

    @Column(name = "CropYearId")
    private Integer cropYearId;

    @Column(name = "ParentCategoryId")
    private Integer parentCategoryId;

    @Column(name = "InvGrnId")
    private Integer invGrnId;

    @Column(name = "InvGdnId")
    private Integer invGdnId;

    @Column(name = "DocumentTypeId")
    private Integer documentTypeId;

    @Column(name = "ExImInvoiceId")
    private Integer exImInvoiceId;

    @Column(name = "TypeCode")
    private String typeCode;

    @Column(name = "TypeDescription")
    private String typeDescription;

    @Column(name = "Type")
    private String type;
}
