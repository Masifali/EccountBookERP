package com.mst.repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.mst.models.ItemAllocateToWareHouse;

/**
 * dbo.ItemAllocateToWareHouse — Sp_ItemAllocateToWareHouse_Insert (single branch; callers use save()).
 *
 * Proc logic: the INSERT happens only if no row exists with the same OrganizationId/CompanyId/ItemId/WarehouseId
 * (existsByOrganizationIdAndCompanyIdAndItemIdAndWarehouseId); then it returns SELECT SCOPE_IDENTITY().
 * Inserted columns: ItemId, WarehouseId, EntryDate, EntryUser, OrganizationId, CompanyId, BrancheId, ProjectsId, ActionId
 * (@Id, @ModifyDate, @ModifyUser parameters are ignored). All parameters default to NULL. Id is IDENTITY.
 */
@Repository
public interface ItemAllocateToWareHouseRepository extends JpaRepository<ItemAllocateToWareHouse, Integer> {

    /** Sp_ItemAllocateToWareHouse_Insert — the "if not exists" guard before the insert. */
    boolean existsByOrganizationIdAndCompanyIdAndItemIdAndWarehouseId(Integer organizationId, Integer companyId, Integer itemId, Integer warehouseId);
}
