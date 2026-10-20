package com.mst.repositories;

import java.time.LocalDateTime;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.StockSummaryByItem;

/**
 * dbo.StockSummaryByItem — USP_StockSummaryByItem_InsertAndUpdate (single branch).
 *
 * Proc logic (upsert keyed on ItemId + ItemUOMId + PackingTypeId + calendar day of DocDate):
 *  - no matching row (countForDay = 0) → ActionId forced to 1, Id = ISNULL(MAX(Id),0)+1 (maxId() + 1), INSERT of
 *    Id, ItemId, ItemUOMId, PackingTypeId, ItemQty, ItemRate, NetWeight, RateUOM, Amount, Remarks, OrganizationId,
 *    CompanyId, FinancialYearId, ActionId, EntryDate = GETDATE(), EntryUserId, DocDate; returns SELECT @Id. Use save().
 *  - otherwise → ActionId forced to 2 and UPDATE (updateForDay). @EntryDate/@ModifyDate/@ActionId/@Id parameters are
 *    effectively ignored. All parameters default to NULL.
 */
@Repository
public interface StockSummaryByItemRepository extends JpaRepository<StockSummaryByItem, Integer> {

    /** USP_StockSummaryByItem_InsertAndUpdate — next app-assigned Id: ISNULL(MAX(Id),0) (+1 by the caller). */
    @Query("select coalesce(max(e.id),0) from StockSummaryByItem e")
    Integer maxId();

    /**
     * USP_StockSummaryByItem_InsertAndUpdate — the IF NOT EXISTS guard (native: CAST(DocDate AS DATE) day comparison).
     * 0 → insert, otherwise → update.
     */
    @Query(value = "SELECT COUNT(1) FROM StockSummaryByItem WHERE ItemId = :itemId AND ItemUOMId = :itemUOMId "
         + "AND PackingTypeId = :packingTypeId AND (CAST(DocDate as Date) = Cast(:docDate as Date))", nativeQuery = true)
    Long countForDay(@Param("itemId") Integer itemId,
                     @Param("itemUOMId") Integer itemUOMId,
                     @Param("packingTypeId") Integer packingTypeId,
                     @Param("docDate") LocalDateTime docDate);

    /**
     * USP_StockSummaryByItem_InsertAndUpdate — the UPDATE part with every SET column of the proc
     * (ActionId = 2 and ModifyDate = GETDATE() exactly as the proc sets them).
     */
    @Modifying
    @Transactional
    @Query(value = "UPDATE [dbo].StockSummaryByItem SET ItemQty = :itemQty, ItemRate = :itemRate, NetWeight = :netWeight, "
         + "RateUOM = :rateUOM, Amount = :amount, Remarks = :remarks, OrganizationId = :organizationId, CompanyId = :companyId, "
         + "FinancialYearId = :financialYearId, ActionId = 2, ModifyDate = GETDATE(), ModifyUserId = :modifyUserId "
         + "WHERE ItemId = :itemId and ItemUOMId = :itemUOMId and PackingTypeId = :packingTypeId "
         + "and (CAST(DocDate as Date) = Cast(:docDate as Date))", nativeQuery = true)
    int updateForDay(@Param("itemId") Integer itemId,
                     @Param("itemUOMId") Integer itemUOMId,
                     @Param("packingTypeId") Integer packingTypeId,
                     @Param("docDate") LocalDateTime docDate,
                     @Param("itemQty") Double itemQty,
                     @Param("itemRate") Double itemRate,
                     @Param("netWeight") Double netWeight,
                     @Param("rateUOM") Double rateUOM,
                     @Param("amount") Double amount,
                     @Param("remarks") String remarks,
                     @Param("organizationId") Integer organizationId,
                     @Param("companyId") Integer companyId,
                     @Param("financialYearId") Integer financialYearId,
                     @Param("modifyUserId") Integer modifyUserId);
}
