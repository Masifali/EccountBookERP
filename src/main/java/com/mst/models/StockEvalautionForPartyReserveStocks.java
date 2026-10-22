package com.mst.models;

import java.io.Serializable;
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
 * Mapped 1:1 onto dbo.StockEvalautionForPartyReserveStocks (source: Sp_InventoryStockEvalautionDetail_Update,
 * section RefDocumentTypeId = 99 "SaleInvoiceDirect" when InvSaleInvoice.IsReserve = 1).
 * Holds the "stock in" side of a reserved-stock sale (party reserve warehouse = InvSaleInvoiceDetail.ReserveWareHouse).
 * <p>
 * Column types follow the same rule as {@link InventoryStockEvalautionDetail} (shared column names, same
 * procedure parameters). The registry lists no key column for this table; an identity {@code Id} column
 * is assumed (the procedure never inserts it).
 */
@Entity
@Setter
@Getter
@Table(name = "StockEvalautionForPartyReserveStocks")
public class StockEvalautionForPartyReserveStocks implements Serializable {

    private static final long serialVersionUID = 1L;

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)   // not in the registry column list; assumed identity key, the proc never inserts it
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

    @Column(name = "DocCodeNo")
    private Integer docCodeNo;

    @Column(name = "DocDate")
    private LocalDateTime docDate;

    @Column(name = "SupplierCustomerId")
    private Integer supplierCustomerId;

    @Column(name = "RefDocSubIdNo")
    private Integer refDocSubIdNo;

    @Column(name = "OrderNo")
    private Integer orderNo;

    @Column(name = "WarehouseId")
    private Integer warehouseId;

    @Column(name = "PrdJobOrderNo")
    private Integer prdJobOrderNo;

    @Column(name = "VehicleNo")
    private String vehicleNo;

    @Column(name = "GpNoDcNo")
    private Integer gpNoDcNo;

    @Column(name = "TranRemarks")
    private String tranRemarks;

    @Column(name = "ItemId")
    private Integer itemId;

    @Column(name = "ItemUom")
    private Integer itemUom;

    @Column(name = "CropBatch")
    private String cropBatch;

    @Column(name = "JobLotId")
    private Integer jobLotId;

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

    @Column(name = "CgsRate")
    private Double cgsRate;

    @Column(name = "CgsAmount")
    private Double cgsAmount;

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

}
