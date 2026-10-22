package com.mst.repositories;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.ItemPricingSchedule;
import com.mst.projections.LastPricingRow;

/**
 * Sp_ItemPricingSchedule_Insert (dbo.ItemPricingSchedule, identity Id, returns SCOPE_IDENTITY()).
 *
 * Pre-insert logic in the proc (all raise 16/1 errors and return without inserting):
 *  1. @ItemPrice null or 0 -> "Item Price not equal to zero".
 *  2. Last EffectedDate for Org/Company/Item/PriceTypeId ({@link #lastByItemAndPriceType}); @EffectedDate earlier -> "cannot define Prices in Previous Date".
 *  3. A row already on the same date for Item/PriceTypeId/CropYearId ({@link #countSameDate}) -> "cannot insert 2 prices".
 *  4. When @PriceTypeId not null and @PackingUom &gt; 0: previous unit price = ItemPrice/PackingUom of the latest row; the new unit price
 *     (@ItemPrice/@PackingUom) must be within +/- ConfigrationsAllocation.ConfigKey % (ConfigrationsDefinitionId = 55, default 5000000 %).
 *  5. @RateUomId = UOMSchedule.Id where ItemId/CompanyId/Equivalent = @PackingUom; when none, a UOMSchedule row is created
 *     (Id = max+1, ScheduleUnitId = UOM.Id whose UOMDescription = CAST(@PackingUom AS nvarchar) for the company, Equivalent = @PackingUom).
 *  6. SmsHistory "Price list published" row inserted when none exists for that day (see SmsHistoryRepository).
 *  7. INSERT ItemPricingSchedule (all columns) ; SELECT SCOPE_IDENTITY().
 * Full behaviour: {@link #spItemPricingScheduleInsert}; simple insert: save() (id comes back on the entity).
 */
@Repository
public interface ItemPricingScheduleRepository extends JpaRepository<ItemPricingSchedule, Integer> {

    /**
     * Sp_ItemPricingSchedule_Insert — full procedural behaviour. All params default NULL. Returns SCOPE_IDENTITY().
     * // KEEP-EXEC: validations with RAISERROR, UOMSchedule + SmsHistory side inserts, ConfigrationsAllocation percentage check.
     */
    @Query(value = "EXEC dbo.Sp_ItemPricingSchedule_Insert @Id = :id, @ScheduleDesc = :scheduleDesc, @ItemId = :itemId, "
                 + "@EffectedDate = :effectedDate, @EntryDate = :entryDate, @EntryUser = :entryUser, @ModifyDate = :modifyDate, "
                 + "@ModifyUser = :modifyUser, @OrganizationId = :organizationId, @CompanyId = :companyId, @BranchesId = :branchesId, "
                 + "@ChangedPriceLookUpId = :changedPriceLookUpId, @PriceTypeId = :priceTypeId, @ItemPrice = :itemPrice, @Margin = :margin, "
                 + "@PackingUom = :packingUom, @RateUomId = :rateUomId, @PackingRate = :packingRate, @CropYearId = :cropYearId, "
                 + "@IsActive = :isActive", nativeQuery = true)
    Object spItemPricingScheduleInsert(@Param("id") Integer id,
                                       @Param("scheduleDesc") String scheduleDesc,
                                       @Param("itemId") Integer itemId,
                                       @Param("effectedDate") LocalDateTime effectedDate,
                                       @Param("entryDate") LocalDateTime entryDate,
                                       @Param("entryUser") Integer entryUser,
                                       @Param("modifyDate") LocalDateTime modifyDate,
                                       @Param("modifyUser") Integer modifyUser,
                                       @Param("organizationId") Integer organizationId,
                                       @Param("companyId") Integer companyId,
                                       @Param("branchesId") Integer branchesId,
                                       @Param("changedPriceLookUpId") Integer changedPriceLookUpId,
                                       @Param("priceTypeId") Integer priceTypeId,
                                       @Param("itemPrice") Double itemPrice,
                                       @Param("margin") Double margin,
                                       @Param("packingUom") Integer packingUom,
                                       @Param("rateUomId") Integer rateUomId,
                                       @Param("packingRate") Double packingRate,
                                       @Param("cropYearId") Integer cropYearId,
                                       @Param("isActive") Boolean isActive);

    /** Sp_ItemPricingSchedule_Insert step 2: TOP 1 last EffectedDate + ItemName for Org/Company/Item/PriceTypeId ORDER BY EffectedDate DESC (GroupName is always NULL here). */
    @Query(value = "SELECT TOP 1 ps.EffectedDate AS LastEffectedDate, i.ItemName AS ItemName, NULL AS GroupName "
                 + "FROM dbo.ItemPricingSchedule ps INNER JOIN Item i on i.Id=ps.ItemId "
                 + "WHERE ps.OrganizationId = :organizationId AND ps.CompanyId = :companyId AND ps.ItemId = :itemId and PriceTypeId=:priceTypeId "
                 + "ORDER BY ps.EffectedDate DESC", nativeQuery = true)
    List<LastPricingRow> lastByItemAndPriceType(@Param("organizationId") Integer organizationId,
                                                @Param("companyId") Integer companyId,
                                                @Param("itemId") Integer itemId,
                                                @Param("priceTypeId") Integer priceTypeId);

    /** Sp_ItemPricingSchedule_Insert step 3: rows on the same calendar day for Item/PriceTypeId/CropYearId. */
    @Query(value = "SELECT COUNT(1) FROM ItemPricingSchedule WHERE OrganizationId = :organizationId AND CompanyId = :companyId "
                 + "AND CAST(EffectedDate AS DATE) = CAST(:effectedDate AS DATE) AND ItemId = :itemId AND PriceTypeId=:priceTypeId "
                 + "AND CropYearId = :cropYearId", nativeQuery = true)
    Long countSameDate(@Param("organizationId") Integer organizationId,
                       @Param("companyId") Integer companyId,
                       @Param("effectedDate") LocalDateTime effectedDate,
                       @Param("itemId") Integer itemId,
                       @Param("priceTypeId") Integer priceTypeId,
                       @Param("cropYearId") Integer cropYearId);

    /** Sp_ItemPricingSchedule_Insert step 4: TOP 1 previous unit price (ItemPrice / PackingUom when both &gt; 0) for Org/Company/Item/PriceTypeId. */
    @Query(value = "SELECT TOP 1 CASE WHEN ItemPrice > 0 AND PackingUom > 0 THEN ItemPrice / PackingUom ELSE ItemPrice END "
                 + "FROM dbo.ItemPricingSchedule WHERE OrganizationId = :organizationId AND CompanyId = :companyId AND ItemId = :itemId "
                 + "and PriceTypeId=:priceTypeId ORDER BY EffectedDate DESC", nativeQuery = true)
    Double previousUnitPrice(@Param("organizationId") Integer organizationId,
                             @Param("companyId") Integer companyId,
                             @Param("itemId") Integer itemId,
                             @Param("priceTypeId") Integer priceTypeId);

    /** Sp_ItemPricingSchedule_Insert step 4: pricing percentage from ConfigrationsAllocation (ConfigrationsDefinitionId = 55), convert(FLOAT,ConfigKey). */
    @Query(value = "select convert(FLOAT,ConfigKey,0) from ConfigrationsAllocation where OrganizationId = :organizationId "
                 + "and CompanyId = :companyId and ConfigrationsDefinitionId = 55", nativeQuery = true)
    List<Double> pricePercentageConfig(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /** Sp_ItemPricingSchedule_Insert step 5: UOM.Id whose UOMDescription = CAST(@PackingUom AS nvarchar(20)) for the company (UOM has no entity). */
    @Query(value = "SELECT Id FROM UOM WHERE UOMDescription = CAST(:packingUom AS nvarchar(20)) AND CompanyId = :companyId", nativeQuery = true)
    List<Integer> uomIdByDescription(@Param("packingUom") Integer packingUom, @Param("companyId") Integer companyId);

    /** Sp_ItemAllocation_Insert / Sp_ItemPricingSchedule_Insert read ItemName from vItemAllocation / Item for error messages. */
    @Query("select i.itemName from Item i where i.id = :itemId")
    String itemNameById(@Param("itemId") Integer itemId);
}
