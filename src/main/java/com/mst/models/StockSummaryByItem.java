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
 * Mapped 1:1 onto dbo.StockSummaryByItem (source: USP_StockSummaryByItem_InsertAndUpdate).
 * Id is app-assigned: the proc computes ISNULL(MAX(Id),0)+1 (see StockSummaryByItemRepository.maxId()).
 * ItemQty/ItemRate/NetWeight/RateUOM/Amount are declared float in the proc, hence Double.
 */
@Entity
@Setter
@Getter
@Table(name = "StockSummaryByItem")
public class StockSummaryByItem implements Serializable {

    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "ItemId")
    private Integer itemId;

    @Column(name = "ItemUOMId")
    private Integer itemUOMId;

    @Column(name = "PackingTypeId")
    private Integer packingTypeId;

    @Column(name = "ItemQty")
    private Double itemQty;

    @Column(name = "ItemRate")
    private Double itemRate;

    @Column(name = "NetWeight")
    private Double netWeight;

    @Column(name = "RateUOM")
    private Double rateUOM;

    @Column(name = "Amount")
    private Double amount;

    @Column(name = "Remarks")
    private String remarks;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "FinancialYearId")
    private Integer financialYearId;

    @Column(name = "ActionId")
    private Integer actionId;

    @Column(name = "EntryDate")
    private LocalDateTime entryDate;

    @Column(name = "EntryUserId")
    private Integer entryUserId;

    @Column(name = "ModifyDate")
    private LocalDateTime modifyDate;

    @Column(name = "ModifyUserId")
    private Integer modifyUserId;

    @Column(name = "DocDate")
    private LocalDateTime docDate;

    // Listed for dbo.StockSummaryByItem in the batch registry but not referenced by any procedure in this
    // batch (most likely a ROW_NUMBER() alias from another proc). Verify it exists before relying on it.
    @Column(name = "row_num")
    private Long rowNum;
}
