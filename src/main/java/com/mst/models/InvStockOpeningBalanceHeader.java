package com.mst.models;

import java.io.Serializable;
import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/**
 * Mapped 1:1 onto dbo.InvStockOpeningBalanceHeader
 * (source: Sp_InvStockOpeningBalanceHeader_Insert / _Update / _GetAllMethod,
 * USP_GetDataForDropDownFromInvStockOpeningBalanceHeader).
 * Id is app-assigned: the Insert proc computes ISNULL(MAX(Id),0)+1 (see repository maxId()).
 */
@Entity
@Setter
@Getter
@Table(name = "InvStockOpeningBalanceHeader")
public class InvStockOpeningBalanceHeader implements Serializable {

    private static final long serialVersionUID = 1L;

    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "DocumentTypeId")
    private Integer documentTypeId;

    @Column(name = "TransactionType")
    private String transactionType;

    @Column(name = "DocDate")
    private LocalDate docDate;

    @Column(name = "DocNo")
    private Integer docNo;

    @Column(name = "WarehouseId")
    private Integer warehouseId;

    @Column(name = "StockCrGLAcId")
    private Integer stockCrGLAcId;

    @Column(name = "ItemId")
    private Integer itemId;

    @Column(name = "CropYear")
    private String cropYear;

    @Column(name = "JobLotId")
    private Integer jobLotId;

    @Column(name = "ItemUomSch")
    private Integer itemUomSch;

    @Column(name = "Qty")
    private Double qty;

    @Column(name = "WeightKgs")
    private Double weightKgs;

    @Column(name = "ItemRate")
    private Double itemRate;

    @Column(name = "RateUomSch")
    private Integer rateUomSch;

    @Column(name = "ItemAmount")
    private Double itemAmount;

    @Column(name = "Remarks")
    private String remarks;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "BranchesId")
    private Integer branchesId;

    @Column(name = "ProjectsId")
    private Integer projectsId;

    @Column(name = "EntryUserId")
    private Integer entryUserId;

    @Column(name = "EntryDate")
    private LocalDateTime entryDate;

    @Column(name = "ModifyUserId")
    private Integer modifyUserId;

    @Column(name = "ModifyDate")
    private LocalDateTime modifyDate;

    @Column(name = "IsApproved")
    private Boolean isApproved;

    @Column(name = "ApprovedUserId")
    private Integer approvedUserId;

    @Column(name = "ApprovedDate")
    private LocalDateTime approvedDate;

    @Column(name = "PackingTypeId")
    private Integer packingTypeId;

    @Column(name = "FinancialYearId")
    private Integer financialYearId;

    @Column(name = "ItemVariantId")
    private Integer itemVariantId;

    @Column(name = "AttachmentsValues")
    private String attachmentsValues;

    @Column(name = "CustomAttachmentsValues")
    private String customAttachmentsValues;

    @Column(name = "WorkOrderId")
    private Integer workOrderId;

    @Column(name = "BatchNo")
    private String batchNo;

    @Column(name = "ProductionStageId")
    private Integer productionStageId;

    @Column(name = "CastingTypeId")
    private Integer castingTypeId;

    @Column(name = "BaseDocumentTypeId")
    private Integer baseDocumentTypeId;

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

    @Column(name = "UserLogId")
    private Long userLogId;
}
