package com.mst.repositories;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.ConsumptionItems;
import com.mst.projections.ConsumptionItemsFormHistoryRow;
import com.mst.projections.ItemIdNameRow;

/**
 * USP_ConsumptionItems_Insert + USP_ConsumptionItems_FormHistory + USP_GetItemsAllocationForConsumption.
 *
 * USP_ConsumptionItems_Insert, insert branch (ISNULL(@Id,0)=0): Id = ISNULL(MAX(Id),0)+1 ({@link #maxId()}), INSERT Id, ItemId, IsActive,
 * Remarks, OrganizationId, CompanyId, BranchesId, EntryDate, EntryUserId (ModifyDate/ModifyUserId not inserted); SELECT @Id. Callers use save().
 * Update branch (@Id &gt; 0): {@link #updateById}.
 */
@Repository
public interface ConsumptionItemsRepository extends JpaRepository<ConsumptionItems, Integer> {

    /** USP_ConsumptionItems_Insert: ISNULL(MAX(CONVERT(INT,Id)),0) — caller adds 1. */
    @Query("select coalesce(max(e.id),0) from ConsumptionItems e")
    Integer maxId();

    /** USP_ConsumptionItems_Insert update branch (ISNULL(@Id,0) &lt;&gt; 0) — every SET column of the proc. */
    @Modifying
    @Transactional
    @Query("UPDATE ConsumptionItems e SET e.itemId = :itemId, e.isActive = :isActive, e.remarks = :remarks, e.organizationId = :organizationId, "
         + "e.companyId = :companyId, e.branchesId = :branchesId, e.modifyDate = :modifyDate, e.modifyUserId = :modifyUserId WHERE e.id = :id")
    int updateById(@Param("id") Integer id,
                   @Param("itemId") Integer itemId,
                   @Param("isActive") Boolean isActive,
                   @Param("remarks") String remarks,
                   @Param("organizationId") Integer organizationId,
                   @Param("companyId") Integer companyId,
                   @Param("branchesId") Integer branchesId,
                   @Param("modifyDate") LocalDateTime modifyDate,
                   @Param("modifyUserId") Integer modifyUserId);

    /** USP_ConsumptionItems_FormHistory — UserName = FirstName + ' ' + LastName of the entry user. */
    @Query("SELECT d.id AS id, d.itemId AS itemId, i.itemName AS itemName, d.isActive AS isActive, d.remarks AS remarks, d.entryDate AS entryDate, "
         + "CONCAT(ur.firstName, ' ', ur.lastName) AS userName "
         + "FROM ConsumptionItems d JOIN Item i ON d.itemId = i.id JOIN UserAccount ur ON d.entryUserId = ur.id "
         + "WHERE d.organizationId = :organizationId AND d.companyId = :companyId")
    List<ConsumptionItemsFormHistoryRow> formHistory(@Param("organizationId") Integer organizationId,
                                                     @Param("companyId") Integer companyId);

    /** USP_GetItemsAllocationForConsumption — items of the company not yet in ConsumptionItems; @ItemCategoryId / @ItemTypeId default NULL. */
    @Query("SELECT i.id AS id, i.itemName AS itemName FROM Item i "
         + "WHERE i.organizationId = :organizationId AND i.companyId = :companyId "
         + "AND (:itemCategoryId IS NULL OR i.itemCategory.id = :itemCategoryId) "
         + "AND (:itemTypeId IS NULL OR i.itemType.id = :itemTypeId) "
         + "AND i.id NOT IN (SELECT c.itemId FROM ConsumptionItems c WHERE c.organizationId = :organizationId AND c.companyId = :companyId)")
    List<ItemIdNameRow> getItemsAllocationForConsumption(@Param("organizationId") Integer organizationId,
                                                         @Param("companyId") Integer companyId,
                                                         @Param("itemCategoryId") Integer itemCategoryId,
                                                         @Param("itemTypeId") Integer itemTypeId);
}
