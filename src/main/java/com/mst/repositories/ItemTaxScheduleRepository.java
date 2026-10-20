package com.mst.repositories;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.ItemTaxSchedule;
import com.mst.projections.ItemTaxForItemRow;
import com.mst.projections.ItemTaxScheduleByItemIdsRow;
import com.mst.projections.ItemTaxScheduleComboRow;
import com.mst.projections.ItemTaxScheduleReadRow;
import com.mst.projections.ItemTaxScheduleRow;
import com.mst.projections.ItemsFromItemTaxScheduleRow;
import com.mst.projections.TaxScheduleMainRow;

/**
 * Sp_ItemTaxSchedule_GetAllMethod + Sp_ItemTaxSchedule_Insert + Sp_ItemTaxSchedule_Update + Usp_AllComboAgainstItemTaxSchedule.
 *
 * Sp_ItemTaxSchedule_Insert pre-insert logic: Id = ISNULL(MAX(Id),0)+1 ({@link #maxId()}); RAISERROR when CAST(@EffectedDate AS date) is
 * earlier than TaxScheduleMain.EffectedDate for Org/Company/TaxNameId=@TaxTypeId ({@link #taxScheduleMainEffectedDate}); RAISERROR when a row
 * already exists for Org/Company/Item/TaxTypeId on the same calendar day ({@link #countSameDay}); then INSERT Id, ItemId, TaxTypeId,
 * EffectedDate, TaxName, IsActive, EntryDate, EntryUser, OrganizationId, CompanyId (ModifyDate/ModifyUser NOT inserted); SELECT @Id.
 * Callers use save() or {@link #spItemTaxScheduleInsert}.
 *
 * Sp_ItemTaxSchedule_Update pre-update logic: same TaxScheduleMain date guard; RAISERROR when another row (Id &lt;&gt; @Id) exists for
 * Org/Company/Item on the same day ({@link #countSameDayOtherId}); when the ItemId changes, RAISERROR when a row exists for the new
 * Item/TaxTypeId/day; RAISERROR when the TaxTypeId is referenced in VoucherDetail, PurchaseOrderDetail (TaxNameId) or
 * InvPurchaseInvoiceDetail (TaxNameId) of the Org/Company. Then UPDATE ({@link #updateById}). Full behaviour: {@link #spItemTaxScheduleUpdate}.
 */
@Repository
public interface ItemTaxScheduleRepository extends JpaRepository<ItemTaxSchedule, Integer> {

    /** Sp_ItemTaxSchedule_Insert: ISNULL(Max(CONVERT(int,Id)),0) — caller adds 1. */
    @Query("select coalesce(max(e.id),0) from ItemTaxSchedule e")
    Integer maxId();

    // ------------------------------------------------------------------ Sp_ItemTaxSchedule_GetAllMethod

    /** Sp_ItemTaxSchedule_GetAllMethod @Activity='ReadById' */
    @Query("SELECT h.id AS id, h.itemId AS itemId, h.taxTypeId AS taxTypeId, h.effectedDate AS effectedDate, h.isActive AS isActive, "
         + "h.entryDate AS entryDate, h.entryUser AS entryUser, h.modifyDate AS modifyDate, h.modifyUser AS modifyUser, "
         + "h.organizationId AS organizationId, h.companyId AS companyId "
         + "FROM ItemTaxSchedule h WHERE h.id = :id")
    List<ItemTaxScheduleRow> readById(@Param("id") Integer id);

    /**
     * Sp_ItemTaxSchedule_GetAllMethod @Activity='ReadByOrganizationCompanyId' — all optional filters default NULL
     * (@ItemCategoryId, @ItemTypeId, @ItemId, @TaxTypeId, @EffectedDate (date), @Active).
     */
    @Query("SELECT h.id AS id, h.itemId AS itemId, h.taxTypeId AS taxTypeId, h.effectedDate AS effectedDate, h.isActive AS isActive, "
         + "h.entryDate AS entryDate, h.entryUser AS entryUser, h.modifyDate AS modifyDate, h.modifyUser AS modifyUser, "
         + "h.organizationId AS organizationId, h.companyId AS companyId, tt.taxName AS taxName, i.itemName AS itemName "
         + "FROM ItemTaxSchedule h "
         + "JOIN TaxesTypes tt ON h.taxTypeId = tt.id "
         + "JOIN Item i ON h.itemId = i.id "
         + "WHERE h.organizationId = :organizationId AND h.companyId = :companyId "
         + "AND (:itemCategoryId IS NULL OR i.itemCategory.id = :itemCategoryId) "
         + "AND (:itemTypeId IS NULL OR i.itemType.id = :itemTypeId) "
         + "AND (:itemId IS NULL OR h.itemId = :itemId) "
         + "AND (:taxTypeId IS NULL OR h.taxTypeId = :taxTypeId) "
         + "AND (:effectedDate IS NULL OR CAST(h.effectedDate AS date) = :effectedDate) "
         + "AND (:active IS NULL OR COALESCE(h.isActive, false) = COALESCE(:active, false))")
    List<ItemTaxScheduleReadRow> readByOrganizationCompanyId(@Param("organizationId") Integer organizationId,
                                                             @Param("companyId") Integer companyId,
                                                             @Param("itemCategoryId") Integer itemCategoryId,
                                                             @Param("itemTypeId") Integer itemTypeId,
                                                             @Param("itemId") Integer itemId,
                                                             @Param("taxTypeId") Integer taxTypeId,
                                                             @Param("effectedDate") LocalDate effectedDate,
                                                             @Param("active") Boolean active);

    /** Sp_ItemTaxSchedule_GetAllMethod @Activity='GetItemTaxScheduleForItemId' — TOP 1 over view V_TaxScheduleForTax (native). */
    @Query(value = "select top 1 TaxNameId, TaxName,TaxScheduleId,tax.EffectedDate,TaxPercent,TaxGLAccountId "
                 + "from Item i inner join V_TaxScheduleForTax Tax on i.Id = Tax.ItemId "
                 + "Where CAST(tax.EffectedDate AS date) <= CAST(:effectedDate AS date) and tax.ItemId = :itemId "
                 + "and Tax.OrganizationId = :organizationId and tax.CompanyId = :companyId "
                 + "order by tax.EffectedDate desc", nativeQuery = true)
    List<ItemTaxForItemRow> getItemTaxScheduleForItemId(@Param("organizationId") Integer organizationId,
                                                        @Param("companyId") Integer companyId,
                                                        @Param("effectedDate") LocalDate effectedDate,
                                                        @Param("itemId") Integer itemId);

    /** Sp_ItemTaxSchedule_GetAllMethod @Activity='GetAllTaxSchedule' — TaxScheduleMain has no entity (native). @TaxTypeId default NULL (filters TaxesTypes.Type). */
    @Query(value = "SELECT t.TaxNameId,tp.TaxName,t.TaxPercent FROM TaxScheduleMain t INNER JOIN TaxesTypes tp on t.TaxNameId = tp.Id "
                 + "Where CAST(t.EffectedDate AS date) <= CAST(:effectedDate AS date) and t.OrganizationId = :organizationId "
                 + "and t.CompanyId = :companyId and (:taxTypeId is null or tp.Type = :taxTypeId)", nativeQuery = true)
    List<TaxScheduleMainRow> getAllTaxSchedule(@Param("organizationId") Integer organizationId,
                                               @Param("companyId") Integer companyId,
                                               @Param("effectedDate") LocalDate effectedDate,
                                               @Param("taxTypeId") Integer taxTypeId);

    /** Sp_ItemTaxSchedule_GetAllMethod @Activity='GetTaxScheduleDetailbyItemIds' — latest schedule per item of the comma-separated @ItemIds (native: ROW_NUMBER + fnSplitString + view). */
    @Query(value = "SELECT * FROM( select ROW_NUMBER() OVER (PARTITION BY tax.ItemId ORDER BY Tax.EffectedDate DESC) RecordNo, "
                 + "Tax.ItemId,TaxNameId, TaxName,TaxScheduleId,Tax.EffectedDate,TaxPercent,TaxGLAccountId "
                 + "from Item i inner join V_TaxScheduleForTax Tax on i.Id = Tax.ItemId "
                 + "Where CAST(tax.EffectedDate AS date) <= CAST(:effectedDate AS date) and Tax.OrganizationId = :organizationId "
                 + "and tax.CompanyId = :companyId AND (Tax.ItemId in (select data from dbo.fnSplitString(:itemIds,','))) "
                 + ")a WHERE a.RecordNo = 1", nativeQuery = true)
    List<ItemTaxScheduleByItemIdsRow> getTaxScheduleDetailbyItemIds(@Param("organizationId") Integer organizationId,
                                                                    @Param("companyId") Integer companyId,
                                                                    @Param("effectedDate") LocalDate effectedDate,
                                                                    @Param("itemIds") String itemIds);

    /** Sp_ItemTaxSchedule_GetAllMethod @Activity='GetItemsFromItemTaxSchedule' — InventoryParentCategories has no entity (native). */
    @Query(value = "SELECT distinct it.ItemId AS Id,i.ItemName,i.ItemCodeNew,p.Id As ParentCategoryId,p.InvParentCateDescription As ParentCategory "
                 + "FROM ItemTaxSchedule it INNER JOIN Item i on it.ItemId = i.Id INNER JOIN ItemCategory cat ON cat.Id = i.ItemCategoryId "
                 + "INNER JOIN InventoryParentCategories p ON p.Id = cat.InventoryParentCategoriesId "
                 + "WHERE it.OrganizationId = :organizationId AND it.CompanyId = :companyId", nativeQuery = true)
    List<ItemsFromItemTaxScheduleRow> getItemsFromItemTaxSchedule(@Param("organizationId") Integer organizationId,
                                                                  @Param("companyId") Integer companyId);

    // ------------------------------------------------------------------ Usp_AllComboAgainstItemTaxSchedule

    /**
     * Usp_AllComboAgainstItemTaxSchedule — @Activity default NULL (all groups), or one of TaxType / ItemName / ItemCategory /
     * ItemParentCategory / ItemClassGroup / ItemType / EffectedDate. Temp-table pipeline rewritten as CTE + UNION ALL
     * (the proc only INSERTs into #tmpResult, no updates). The EffectedDate group is CAST to nvarchar explicitly because the
     * proc stored it into an nvarchar(max) column (implicit conversion, default style).
     */
    @Query(value = "WITH TmpFinalResult AS ( SELECT distinct h.TaxTypeId, h.ItemId, CAST(h.EffectedDate as Date) EffectedDate "
                 + "FROM ItemTaxSchedule h WHERE h.OrganizationId = :organizationId and h.CompanyId = :companyId ), "
                 + "TmpResult AS ( "
                 + "select Distinct 'TaxType' as Activity, h.TaxTypeId as Id, TaxesTypes.TaxDescription as ReferenceName "
                 + "from TmpFinalResult h INNER JOIN TaxesTypes ON h.TaxTypeId = TaxesTypes.Id where (:activity is null or 'TaxType' = :activity) "
                 + "UNION ALL "
                 + "select Distinct 'ItemName' as Activity, Pid.ItemId as Id, i.ItemName as ReferenceName "
                 + "from TmpFinalResult Pid inner join Item i on Pid.ItemId = i.Id where (:activity is null or 'ItemName' = :activity) "
                 + "UNION ALL "
                 + "select Distinct 'ItemCategory' as Activity, i.ItemCategoryId as Id, ic.CategoryDescription as ReferenceName "
                 + "from TmpFinalResult PId inner join Item i on Pid.ItemId = i.Id inner join ItemCategory ic on i.ItemCategoryId = ic.Id "
                 + "where (:activity is null or 'ItemCategory' = :activity) "
                 + "UNION ALL "
                 + "select Distinct 'ItemParentCategory' as Activity, ic.InventoryParentCategoriesId as Id, pc.InvParentCateDescription as ReferenceName "
                 + "from TmpFinalResult Pid inner join Item i on Pid.ItemId = i.Id inner join ItemCategory ic on i.ItemCategoryId = ic.Id "
                 + "Inner join InventoryParentCategories pc on pc.Id=ic.InventoryParentCategoriesId where (:activity is null or 'ItemParentCategory' = :activity) "
                 + "UNION ALL "
                 + "select Distinct 'ItemClassGroup' as Activity, ic.ItemClassGroupId as Id, c.ClassGroupName as ReferenceName "
                 + "from TmpFinalResult Pid inner join Item i on Pid.ItemId = i.Id inner join ItemCategory ic on i.ItemCategoryId = ic.Id "
                 + "Inner join ItemClassGroup c on c.Id=ic.ItemClassGroupId where (:activity is null or 'ItemClassGroup' = :activity) "
                 + "UNION ALL "
                 + "select Distinct 'ItemType' as Activity, i.ItemTypeId as Id, it.TypeDescription as ReferenceName "
                 + "from TmpFinalResult Pid inner join Item i on Pid.ItemId = i.Id inner join ItemType it on i.ItemTypeId = it.Id "
                 + "where (:activity is null or 'ItemType' = :activity) "
                 + "UNION ALL "
                 + "select Distinct 'EffectedDate' as Activity, 0 as Id, CONVERT(nvarchar(max), pid.EffectedDate) as ReferenceName "
                 + "from TmpFinalResult pid where (:activity is null or 'EffectedDate' = :activity) ) "
                 + "SELECT R.Id, R.ReferenceName, R.Activity FROM TmpResult R", nativeQuery = true)
    List<ItemTaxScheduleComboRow> allComboAgainstItemTaxSchedule(@Param("organizationId") Integer organizationId,
                                                                 @Param("companyId") Integer companyId,
                                                                 @Param("activity") String activity);

    // ------------------------------------------------------------------ Sp_ItemTaxSchedule_Insert

    /**
     * Sp_ItemTaxSchedule_Insert — full procedural behaviour (date guard + duplicate guard + insert). All params default NULL.
     * Returns the new Id (SELECT @Id).
     * // KEEP-EXEC: RAISERROR validations against TaxScheduleMain and duplicate day.
     */
    @Query(value = "EXEC dbo.Sp_ItemTaxSchedule_Insert @CompanyId = :companyId, @EffectedDate = :effectedDate, @TaxName = :taxName, "
                 + "@EntryDate = :entryDate, @EntryUser = :entryUser, @Id = :id, @IsActive = :isActive, @ItemId = :itemId, "
                 + "@ModifyDate = :modifyDate, @ModifyUser = :modifyUser, @OrganizationId = :organizationId, @TaxTypeId = :taxTypeId",
           nativeQuery = true)
    Integer spItemTaxScheduleInsert(@Param("companyId") Integer companyId,
                                    @Param("effectedDate") LocalDateTime effectedDate,
                                    @Param("taxName") String taxName,
                                    @Param("entryDate") LocalDateTime entryDate,
                                    @Param("entryUser") Integer entryUser,
                                    @Param("id") Integer id,
                                    @Param("isActive") Boolean isActive,
                                    @Param("itemId") Integer itemId,
                                    @Param("modifyDate") LocalDateTime modifyDate,
                                    @Param("modifyUser") Integer modifyUser,
                                    @Param("organizationId") Integer organizationId,
                                    @Param("taxTypeId") Integer taxTypeId);

    /** Sp_ItemTaxSchedule_Insert / _Update guard: TaxScheduleMain.EffectedDate for Org/Company/TaxNameId (TaxScheduleMain has no entity; take first). */
    @Query(value = "SELECT EffectedDate FROM TaxScheduleMain WHERE OrganizationId = :organizationId AND CompanyId = :companyId AND TaxNameId = :taxTypeId",
           nativeQuery = true)
    List<java.sql.Timestamp> taxScheduleMainEffectedDate(@Param("organizationId") Integer organizationId,
                                                          @Param("companyId") Integer companyId,
                                                          @Param("taxTypeId") Integer taxTypeId);

    /** Sp_ItemTaxSchedule_Insert guard (and _Update when ItemId changes): rows for Org/Company/Item/TaxTypeId on the same calendar day. */
    @Query(value = "SELECT COUNT(1) FROM dbo.ItemTaxSchedule WHERE OrganizationId = :organizationId AND CompanyId = :companyId "
                 + "AND ItemId = :itemId AND TaxTypeId = :taxTypeId AND CAST(EffectedDate AS date) = CAST(:effectedDate AS date)", nativeQuery = true)
    Long countSameDay(@Param("organizationId") Integer organizationId,
                      @Param("companyId") Integer companyId,
                      @Param("itemId") Integer itemId,
                      @Param("taxTypeId") Integer taxTypeId,
                      @Param("effectedDate") LocalDateTime effectedDate);

    /** Sp_ItemTaxSchedule_Insert error text helper: REPLACE(TaxName,'%','') of the tax type (TaxesTypes planned entity, read via JPQL). */
    @Query("select tt.taxName from TaxesTypes tt where tt.id = :taxTypeId")
    List<String> taxNameByTaxTypeId(@Param("taxTypeId") Integer taxTypeId);

    // ------------------------------------------------------------------ Sp_ItemTaxSchedule_Update

    /**
     * Sp_ItemTaxSchedule_Update — full procedural behaviour. All params default NULL.
     * // KEEP-EXEC: five RAISERROR guards (TaxScheduleMain date, same-day duplicate, item change duplicate, Voucher / PurchaseOrder / PurchaseInvoice references).
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Sp_ItemTaxSchedule_Update @CompanyId = :companyId, @EffectedDate = :effectedDate, @TaxName = :taxName, "
                 + "@EntryDate = :entryDate, @EntryUser = :entryUser, @Id = :id, @IsActive = :isActive, @ItemId = :itemId, "
                 + "@ModifyDate = :modifyDate, @ModifyUser = :modifyUser, @OrganizationId = :organizationId, @TaxTypeId = :taxTypeId",
           nativeQuery = true)
    void spItemTaxScheduleUpdate(@Param("companyId") Integer companyId,
                                 @Param("effectedDate") LocalDateTime effectedDate,
                                 @Param("taxName") String taxName,
                                 @Param("entryDate") LocalDateTime entryDate,
                                 @Param("entryUser") Integer entryUser,
                                 @Param("id") Integer id,
                                 @Param("isActive") Boolean isActive,
                                 @Param("itemId") Integer itemId,
                                 @Param("modifyDate") LocalDateTime modifyDate,
                                 @Param("modifyUser") Integer modifyUser,
                                 @Param("organizationId") Integer organizationId,
                                 @Param("taxTypeId") Integer taxTypeId);

    /** Sp_ItemTaxSchedule_Update — the UPDATE statement itself (every SET column of the proc; EntryDate/EntryUser are not updated). */
    @Modifying
    @Transactional
    @Query("UPDATE ItemTaxSchedule e SET e.itemId = :itemId, e.taxTypeId = :taxTypeId, e.effectedDate = :effectedDate, e.taxName = :taxName, "
         + "e.isActive = :isActive, e.modifyDate = :modifyDate, e.modifyUser = :modifyUser, e.organizationId = :organizationId, "
         + "e.companyId = :companyId WHERE e.id = :id")
    int updateById(@Param("id") Integer id,
                   @Param("itemId") Integer itemId,
                   @Param("taxTypeId") Integer taxTypeId,
                   @Param("effectedDate") LocalDateTime effectedDate,
                   @Param("taxName") String taxName,
                   @Param("isActive") Boolean isActive,
                   @Param("modifyDate") LocalDateTime modifyDate,
                   @Param("modifyUser") Integer modifyUser,
                   @Param("organizationId") Integer organizationId,
                   @Param("companyId") Integer companyId);

    /** Sp_ItemTaxSchedule_Update guard: SELECT ItemId FROM ItemTaxSchedule WHERE Org/Company/Id (the "old" item id). */
    @Query("select e.itemId from ItemTaxSchedule e where e.organizationId = :organizationId and e.companyId = :companyId and e.id = :id")
    Integer oldItemId(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("id") Integer id);

    /** Sp_ItemTaxSchedule_Update guard: other rows (Id &lt;&gt; @Id) for Org/Company/Item on the same calendar day. */
    @Query(value = "SELECT COUNT(1) FROM dbo.ItemTaxSchedule WHERE OrganizationId = :organizationId AND CompanyId = :companyId "
                 + "AND ItemId = :itemId AND CAST(EffectedDate AS date) = CAST(:effectedDate AS date) and Id <> :id", nativeQuery = true)
    Long countSameDayOtherId(@Param("organizationId") Integer organizationId,
                             @Param("companyId") Integer companyId,
                             @Param("itemId") Integer itemId,
                             @Param("effectedDate") LocalDateTime effectedDate,
                             @Param("id") Integer id);

    /** Sp_ItemTaxSchedule_Update guard: VoucherHead/VoucherDetail rows using the TaxTypeId (OrganizationId/CompanyId are unqualified in the proc = VoucherHead's via join resolution). */
    @Query("select count(d) from VoucherHead h join VoucherDetail d on h.id = d.voucherHead.id "
         + "where h.organizationId = :organizationId and h.companyId = :companyId and d.taxTypeId = :taxTypeId")
    Long countVoucherReferences(@Param("organizationId") Integer organizationId,
                                @Param("companyId") Integer companyId,
                                @Param("taxTypeId") Integer taxTypeId);

    /** Sp_ItemTaxSchedule_Update guard: PurchaseOrderDetail.TaxNameId references (TaxNameId not in the PurchaseOrderDetail entity → native). */
    @Query(value = "SELECT COUNT(1) FROM PurchaseOrder h inner join PurchaseOrderDetail d on h.Id = d.PurchaseOrderId "
                 + "where h.OrganizationId = :organizationId and h.CompanyId = :companyId and d.TaxNameId = :taxTypeId", nativeQuery = true)
    Long countPurchaseOrderReferences(@Param("organizationId") Integer organizationId,
                                      @Param("companyId") Integer companyId,
                                      @Param("taxTypeId") Integer taxTypeId);

    /** Sp_ItemTaxSchedule_Update guard: InvPurchaseInvoiceDetail.TaxNameId references (native). */
    @Query(value = "SELECT COUNT(1) FROM InvPurchaseInvoice h inner join InvPurchaseInvoiceDetail d on h.Id = d.InvPurchaseInvoiceId "
                 + "where h.OrganizationId = :organizationId and h.CompanyId = :companyId and d.TaxNameId = :taxTypeId", nativeQuery = true)
    Long countPurchaseInvoiceReferences(@Param("organizationId") Integer organizationId,
                                        @Param("companyId") Integer companyId,
                                        @Param("taxTypeId") Integer taxTypeId);
}
