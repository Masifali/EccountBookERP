package com.mst.repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.ItemsReorderSchedule;

/**
 * Sp_ItemsReorderSchedule_Insert — simple single-table insert. Pre-insert logic: SortNo = ISNULL(MAX(SortNo),0)+1 within ItemId
 * ({@link #maxSortNoByItemId}); Id = ISNULL(MAX(Id),0)+1 ({@link #maxId()}); then INSERT all columns; SELECT @Id. Callers use save().
 * All proc params default NULL.
 */
@Repository
public interface ItemsReorderScheduleRepository extends JpaRepository<ItemsReorderSchedule, Integer> {

    /** Sp_ItemsReorderSchedule_Insert: ISNULL(max(convert(int,Id)),0) — caller adds 1. */
    @Query("select coalesce(max(e.id),0) from ItemsReorderSchedule e")
    Integer maxId();

    /** Sp_ItemsReorderSchedule_Insert: isnull(max(convert(int,sortno)),0) within the ItemId — caller adds 1. */
    @Query("select coalesce(max(e.sortNo),0) from ItemsReorderSchedule e where e.itemId = :itemId")
    Integer maxSortNoByItemId(@Param("itemId") Integer itemId);
}
