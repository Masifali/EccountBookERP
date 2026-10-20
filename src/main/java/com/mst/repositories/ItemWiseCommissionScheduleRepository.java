package com.mst.repositories;

import java.time.LocalDate;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.ItemWiseCommissionSchedule;

/**
 * USP_ItemWiseCommission_InsertOrUpdate. Branch "update" (EXISTS row with Id = @Id): {@link #updateById}.
 * Branch "insert" (otherwise): Id = ISNULL(MAX(Id),0)+1 ({@link #maxId()}), INSERT Id, OnPurchase, OnSale, EffectedFromDate, EntryDate,
 * BranchesId, CompanyId, EntryUserId, ItemId, OrgnizationId (ModifyDate/ModifyUserId not inserted); SELECT @Id — callers use
 * existsById() + save(). All params default NULL.
 */
@Repository
public interface ItemWiseCommissionScheduleRepository extends JpaRepository<ItemWiseCommissionSchedule, Integer> {

    /** USP_ItemWiseCommission_InsertOrUpdate: ISNULL(Max(CONVERT(int,Id)),0) — caller adds 1. */
    @Query("select coalesce(max(e.id),0) from ItemWiseCommissionSchedule e")
    Integer maxId();

    /** USP_ItemWiseCommission_InsertOrUpdate update branch — every SET column of the proc (EntryDate/EntryUserId untouched). */
    @Modifying
    @Transactional
    @Query("UPDATE ItemWiseCommissionSchedule e SET e.onPurchase = :onPurchase, e.onSale = :onSale, e.effectedFromDate = :effectedFromDate, "
         + "e.modifyDate = :modifyDate, e.branchesId = :branchesId, e.companyId = :companyId, e.itemId = :itemId, e.modifyUserId = :modifyUserId, "
         + "e.orgnizationId = :orgnizationId WHERE e.id = :id")
    int updateById(@Param("id") Integer id,
                   @Param("onPurchase") Boolean onPurchase,
                   @Param("onSale") Boolean onSale,
                   @Param("effectedFromDate") LocalDate effectedFromDate,
                   @Param("modifyDate") LocalDate modifyDate,
                   @Param("branchesId") Integer branchesId,
                   @Param("companyId") Integer companyId,
                   @Param("itemId") Integer itemId,
                   @Param("modifyUserId") Integer modifyUserId,
                   @Param("orgnizationId") Integer orgnizationId);
}
