package com.mst.repositories;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.FedItemPricingSchedule;
import com.mst.projections.LastPricingRow;
import com.mst.projections.MinMaxRateWithUomRow;

/**
 * fed.usp_ItemPricingSchedule_Insert (fed.ItemPricingSchedule, app-assigned Id).
 *
 * Pre-insert logic in the proc:
 *  1. Id = ISNULL(MAX(Id),0)+1 ({@link #maxId()}).
 *  2. ISNULL(@RateUomId,0)=0 -> RateUomId = UOMSchedule.Id of the item's base unit (UOMScheduleRepository.baseRateUomIdByItemId).
 *  3. When @ItemRate &lt;&gt; 1: latest ItemMinMaxRateSchedule on/before @EffectiveDate ({@link #lastMinMaxRate}) is scaled to the
 *     RateUom equivalent (UOMScheduleRepository.equivalentByItemIdAndId); @ItemRate below min / above max -> RAISERROR.
 *  4. @ItemRate null or 0 -> RAISERROR "Item Price not equal to zero".
 *  5. Last EffectiveDate for Item/PricingCustomGroupId ({@link #lastByItemAndGroup}); when @PricingCustomGroupId &gt; 0 and
 *     @EffectiveDate earlier -> RAISERROR "cannot define Prices in Previous Date".
 *  6. Duplicate (EffectiveDate, ItemId, RateUomId, PricingCustomGroupId) -> RAISERROR ({@link #existsByOrganizationIdAndCompanyIdAndEffectiveDateAndItemIdAndRateUomIdAndPricingCustomGroupId}).
 *  7. INSERT Id, EffectiveDate, ItemId, ItemRate, RateUomId, EntryDate, EntryUserId, OrganizationId, CompanyId, FinancialYearId,
 *     PricingCustomGroupId, DocumentTypeId, IsActive (ModifyDate/ModifyUserId/IsApproved/ApprovedUserId/ApprovedDate are NOT inserted); SELECT @Id.
 * Full behaviour: {@link #uspItemPricingScheduleInsert}; simple insert: save().
 */
@Repository
public interface FedItemPricingScheduleRepository extends JpaRepository<FedItemPricingSchedule, Integer> {

    /** fed.usp_ItemPricingSchedule_Insert step 1: ISNULL(Max(CONVERT(int,Id)),0) — caller adds 1. */
    @Query("select coalesce(max(e.id),0) from FedItemPricingSchedule e")
    Integer maxId();

    /**
     * fed.usp_ItemPricingSchedule_Insert — full procedural behaviour. Defaults: @Id, @EntryDate, @EntryUserId, @ModifyDate, @ModifyUserId,
     * @ApprovedUserId, @ApprovedDate = NULL; the rest are mandatory. Returns the new Id (SELECT @Id).
     * // KEEP-EXEC: min/max rate validation + several RAISERROR guards before the insert.
     */
    @Query(value = "EXEC fed.usp_ItemPricingSchedule_Insert @Id = :id, @EffectiveDate = :effectiveDate, @ItemId = :itemId, "
                 + "@ItemRate = :itemRate, @RateUomId = :rateUomId, @EntryDate = :entryDate, @EntryUserId = :entryUserId, "
                 + "@ModifyDate = :modifyDate, @ModifyUserId = :modifyUserId, @OrganizationId = :organizationId, @CompanyId = :companyId, "
                 + "@FinancialYearId = :financialYearId, @IsApproved = :isApproved, @IsActive = :isActive, @ApprovedUserId = :approvedUserId, "
                 + "@ApprovedDate = :approvedDate, @PricingCustomGroupId = :pricingCustomGroupId, @DocumentTypeId = :documentTypeId",
           nativeQuery = true)
    Integer uspItemPricingScheduleInsert(@Param("id") Integer id,
                                         @Param("effectiveDate") LocalDateTime effectiveDate,
                                         @Param("itemId") Integer itemId,
                                         @Param("itemRate") Double itemRate,
                                         @Param("rateUomId") Integer rateUomId,
                                         @Param("entryDate") LocalDateTime entryDate,
                                         @Param("entryUserId") Integer entryUserId,
                                         @Param("modifyDate") LocalDateTime modifyDate,
                                         @Param("modifyUserId") Integer modifyUserId,
                                         @Param("organizationId") Integer organizationId,
                                         @Param("companyId") Integer companyId,
                                         @Param("financialYearId") Integer financialYearId,
                                         @Param("isApproved") Boolean isApproved,
                                         @Param("isActive") Boolean isActive,
                                         @Param("approvedUserId") Integer approvedUserId,
                                         @Param("approvedDate") LocalDateTime approvedDate,
                                         @Param("pricingCustomGroupId") Integer pricingCustomGroupId,
                                         @Param("documentTypeId") Integer documentTypeId);

    /** Sp_ItemAllocation_Insert guard: IF NOT EXISTS(fed.ItemPricingSchedule WHERE OrganizationId AND CompanyId AND ItemId). */
    boolean existsByOrganizationIdAndCompanyIdAndItemId(Integer organizationId, Integer companyId, Integer itemId);

    /** Sp_ItemAllocation_Insert guard: IF NOT EXISTS(fed.ItemPricingSchedule WHERE ... AND RateUomId = @getRateUomId). */
    boolean existsByOrganizationIdAndCompanyIdAndItemIdAndRateUomId(Integer organizationId, Integer companyId, Integer itemId, Integer rateUomId);

    /** fed.usp_ItemPricingSchedule_Insert step 6: duplicate check on EffectiveDate / ItemId / RateUomId / PricingCustomGroupId. */
    boolean existsByOrganizationIdAndCompanyIdAndEffectiveDateAndItemIdAndRateUomIdAndPricingCustomGroupId(
            Integer organizationId, Integer companyId, LocalDateTime effectiveDate, Integer itemId, Integer rateUomId, Integer pricingCustomGroupId);

    /** fed.usp_ItemPricingSchedule_Insert step 3: TOP 1 min/max rate (+ its UOMSchedule.Equivalent) effective on/before @EffectiveDate. */
    @Query(value = "SELECT TOP 1 R.MinRate AS MinRate, R.MaxRate AS MaxRate, UOM.Equivalent AS MinMaxRateUom "
                 + "FROM ItemMinMaxRateSchedule R INNER JOIN UOMSchedule UOM ON R.RateUomId = UOM.Id AND R.ItemId = UOM.ItemId "
                 + "WHERE R.OrganizationId = :organizationId AND R.CompanyId = :companyId AND R.ItemId = :itemId "
                 + "AND CAST(R.EffectedDate AS DATE) <= CAST(:effectiveDate AS DATE) ORDER BY R.EffectedDate DESC", nativeQuery = true)
    List<MinMaxRateWithUomRow> lastMinMaxRate(@Param("organizationId") Integer organizationId,
                                              @Param("companyId") Integer companyId,
                                              @Param("itemId") Integer itemId,
                                              @Param("effectiveDate") LocalDateTime effectiveDate);

    /** fed.usp_ItemPricingSchedule_Insert step 5: TOP 1 last EffectiveDate, ItemName and PolicyCustomGroups.GroupName for Item/PricingCustomGroupId (PolicyCustomGroups has no entity). */
    @Query(value = "SELECT TOP 1 ps.EffectiveDate AS LastEffectedDate, i.ItemName AS ItemName, pg.GroupName AS GroupName "
                 + "FROM fed.ItemPricingSchedule ps INNER JOIN Item i on i.Id=ps.ItemId INNER JOIN PolicyCustomGroups PG ON PS.PricingCustomGroupId = PG.Id "
                 + "WHERE ps.OrganizationId = :organizationId AND ps.CompanyId = :companyId AND ps.ItemId = :itemId "
                 + "AND ps.PricingCustomGroupId = :pricingCustomGroupId ORDER BY EffectiveDate DESC", nativeQuery = true)
    List<LastPricingRow> lastByItemAndGroup(@Param("organizationId") Integer organizationId,
                                            @Param("companyId") Integer companyId,
                                            @Param("itemId") Integer itemId,
                                            @Param("pricingCustomGroupId") Integer pricingCustomGroupId);
}
