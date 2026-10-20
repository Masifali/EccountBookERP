package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.ItemAllocateToManufactures;
import com.mst.projections.ItemAllocateToManufacturesRow;

/**
 * Sp_ItemAllocateToManufactures_GetAllMethod + USP_ItemAllocateToManufactures_Insert.
 *
 * Insert branch of USP_ItemAllocateToManufactures_Insert (ISNULL(@Id,0)=0): proc sets Id = ISNULL(MAX(Id),0)+1 (use {@link #maxId()})
 * then inserts Id, ItemId, ManufactureId, IsActive and returns the new Id (SELECT @Id). Callers use save().
 * Update branch (@Id &gt; 0): {@link #updateById}.
 */
@Repository
public interface ItemAllocateToManufacturesRepository extends JpaRepository<ItemAllocateToManufactures, Integer> {

    /** USP_ItemAllocateToManufactures_Insert pre-insert: ISNULL(MAX(CONVERT(INT,Id)),0) — caller adds 1. */
    @Query("select coalesce(max(e.id),0) from ItemAllocateToManufactures e")
    Integer maxId();

    /** Sp_ItemAllocateToManufactures_GetAllMethod @Activity='ReadAll' (InvLookUp filtered on InvLookupTypeId = 14 = manufacturer lookup). */
    @Query("SELECT d.id AS id, d.itemId AS itemId, d.manufactureId AS manufactureId, d.isActive AS isActive, "
         + "i.itemName AS itemName, lp.lookupName AS manufacture "
         + "FROM ItemAllocateToManufactures d "
         + "JOIN Item i ON d.itemId = i.id "
         + "JOIN InvLookUp lp ON d.manufactureId = lp.id AND lp.invLookupTypeId = 14 "
         + "WHERE i.organizationId = :organizationId AND i.companyId = :companyId")
    List<ItemAllocateToManufacturesRow> readAll(@Param("organizationId") Integer organizationId,
                                                @Param("companyId") Integer companyId);

    /** USP_ItemAllocateToManufactures_Insert update branch (@Id &gt; 0). */
    @Modifying
    @Transactional
    @Query("UPDATE ItemAllocateToManufactures e SET e.itemId = :itemId, e.manufactureId = :manufactureId, e.isActive = :isActive "
         + "WHERE e.id = :id")
    int updateById(@Param("id") Integer id,
                   @Param("itemId") Integer itemId,
                   @Param("manufactureId") Integer manufactureId,
                   @Param("isActive") Boolean isActive);
}
