package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.UOMSchedule;

/**
 * dbo.UOMSchedule helpers for the pre-insert logic of Sp_ItemAllocation_Insert, Sp_ItemPricingSchedule_Insert and
 * fed.usp_ItemPricingSchedule_Insert. Inserts are app-assigned Id = ISNULL(MAX(Id),0)+1 ({@link #maxId()}) then save().
 */
@Repository
public interface UOMScheduleRepository extends JpaRepository<UOMSchedule, Integer> {

    /** Sp_ItemAllocation_Insert / Sp_ItemPricingSchedule_Insert: ISNULL(Max(CONVERT(int,Id)),0) — caller adds 1. */
    @Query("select coalesce(max(e.id),0) from UOMSchedule e")
    Integer maxId();

    /** Sp_ItemAllocation_Insert: IF NOT EXISTS(UOMSchedule WHERE OrganizationId AND ItemId AND ScheduleUnitId). */
    boolean existsByOrganizationIdAndItemIdAndScheduleUnitId(Integer organizationId, Integer itemId, Integer scheduleUnitId);

    /** Sp_ItemAllocation_Insert ELSE branch: SELECT Id, Equivalent FROM UOMSchedule WHERE ScheduleUnitId AND ItemId (take first). */
    List<UOMSchedule> findByScheduleUnitIdAndItemId(Integer scheduleUnitId, Integer itemId);

    /** Sp_ItemPricingSchedule_Insert: select Id From UOMSchedule where ItemId AND CompanyId AND Equivalent = @PackingUom (take first). */
    List<UOMSchedule> findByItemIdAndCompanyIdAndEquivalent(Integer itemId, Integer companyId, Double equivalent);

    /** fed.usp_ItemPricingSchedule_Insert: SELECT Equivalent FROM UOMSchedule WHERE ItemId AND Id = @RateUomId. */
    @Query("select u.equivalent from UOMSchedule u where u.itemId = :itemId and u.id = :rateUomId")
    Double equivalentByItemIdAndId(@Param("itemId") Integer itemId, @Param("rateUomId") Integer rateUomId);

    /** fed.usp_ItemPricingSchedule_Insert (ISNULL(@RateUomId,0)=0): UOMSchedule.Id of the item's base unit (ScheduleUnitId = Item.BaseUnitId, same CompanyId). */
    @Query("select u.id from Item i join UOMSchedule u on i.id = u.itemId and i.companyId = u.companyId and u.scheduleUnitId = i.baseUnitId "
         + "where i.id = :itemId")
    List<Integer> baseRateUomIdByItemId(@Param("itemId") Integer itemId);
}
