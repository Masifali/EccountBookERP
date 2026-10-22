package com.mst.repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.ItemAllocationToStoreRack;

/**
 * dbo.ItemAllocationToStoreRack — USP_ItemAllocationToStoreRack_InsertAndUpdate (single branch).
 *
 * Proc logic (upsert keyed on itemId + storeRackId):
 *  - no row for (itemId, storeRackId) (existsByItemIdAndStoreRackId = false) → INSERT with Id = ISNULL(MAX(Id),0)+1
 *    (maxId() + 1), itemId, storeRackId, isActive, EntryDate = GETDATE(), EntryUserId; returns SELECT @Id. Use save().
 *  - otherwise → UPDATE itemId, storeRackId, isActive, ModifyDate = GETDATE(), ModifyUserId WHERE itemId AND storeRackId
 *    (updateByItemIdAndStoreRackId). @EntryDate/@ModifyDate parameters are ignored (GETDATE() is used).
 */
@Repository
public interface ItemAllocationToStoreRackRepository extends JpaRepository<ItemAllocationToStoreRack, Integer> {

    /** USP_ItemAllocationToStoreRack_InsertAndUpdate — next app-assigned Id: ISNULL(MAX(Id),0) (+1 by the caller). */
    @Query("select coalesce(max(e.id),0) from ItemAllocationToStoreRack e")
    Integer maxId();

    /** USP_ItemAllocationToStoreRack_InsertAndUpdate — the IF NOT EXISTS guard (false → insert, true → update). */
    boolean existsByItemIdAndStoreRackId(Integer itemId, Integer storeRackId);

    /** USP_ItemAllocationToStoreRack_InsertAndUpdate — the UPDATE part (every SET column; ModifyDate = GETDATE()). */
    @Modifying
    @Transactional
    @Query("UPDATE ItemAllocationToStoreRack a SET a.itemId = :itemId, a.storeRackId = :storeRackId, a.isActive = :isActive, "
         + "a.modifyDate = CURRENT_TIMESTAMP, a.modifyUserId = :modifyUserId "
         + "WHERE a.itemId = :itemId AND a.storeRackId = :storeRackId")
    int updateByItemIdAndStoreRackId(@Param("itemId") Integer itemId,
                                     @Param("storeRackId") Integer storeRackId,
                                     @Param("isActive") Boolean isActive,
                                     @Param("modifyUserId") Integer modifyUserId);
}
