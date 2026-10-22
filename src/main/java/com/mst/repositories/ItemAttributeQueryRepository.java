package com.mst.repositories;

import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

/**
 * item.USP_ItemAttribute_InsertAndUpdateByCategory — copies every item.CategoryAttribute of a category onto every Item of that category
 * (as item.ItemAttribute rows) that does not have the attribute yet. The tables live in the "item" schema and have no entity, so this is a
 * native INSERT ... SELECT. The proc's #Data temp table is only a staging SELECT, so the two statements collapse into one.
 */
@Repository
public interface ItemAttributeQueryRepository extends org.springframework.data.repository.Repository<com.mst.models.ItemAllocation, Integer> {

    /**
     * item.USP_ItemAttribute_InsertAndUpdateByCategory — @ItemId default NULL (all items of the category).
     * itemAttributeId = CONVERT(bigint, ItemId + '00' + attributeId).
     */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO item.ItemAttribute ([itemAttributeId],[itemId],[attributeId],[seqNo],[isActive],[EntryDate],[EntryUserId],[organizationId],[companyId]) "
                 + "Select R.[itemAttributeId], R.[itemId], R.[attributeId], R.[seqNo], R.[isActive], R.[EntryDate], R.[EntryUserId], R.[organizationId], R.[companyId] "
                 + "from ( Select CONVERT(bigint,ISNULL(CONVERT(nvarchar(max),i.Id),'') + '00' + ISNULL(CONVERT(nvarchar(max),c.attributeId),''),0) itemAttributeId, "
                 + "i.Id ItemId,c.attributeId,c.seqNo,c.isActive,c.EntryDate,c.EntryUserId,c.ModifyDate,c.ModifyUserId,c.organizationId,c.companyId "
                 + "From item.CategoryAttribute c Inner join item.Attribute A on A.attributeid=c.attributeId "
                 + "Left Join dbo.Item i on i.ItemCategoryId=c.itemCategoryId "
                 + "Where c.organizationId=:organizationId and c.companyId=:companyId and c.itemCategoryId=:itemCategoryId "
                 + "and (:itemId is null or i.Id=:itemId) "
                 + "and Not Exists (Select 1 From item.ItemAttribute IA Where IA.organizationId=:organizationId and IA.companyId=:companyId "
                 + "and IA.attributeId=c.attributeId and IA.itemId=i.Id) "
                 + "and Exists (Select 1 From item AA Where AA.organizationId=:organizationId and AA.companyId=:companyId "
                 + "and AA.ItemCategoryId=c.itemCategoryId) ) R", nativeQuery = true)
    int insertAndUpdateByCategory(@Param("organizationId") Integer organizationId,
                                  @Param("companyId") Integer companyId,
                                  @Param("itemCategoryId") Integer itemCategoryId,
                                  @Param("itemId") Integer itemId);
}
