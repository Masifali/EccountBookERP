package com.mst.repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import com.mst.models.InwardGatePassPurchaseBreakUp;

/**
 * USP_GatePassInwardPurchaseBreakUp_Insert → callers use {@code save()} on {@link InwardGatePassPurchaseBreakUp}.
 * <p>
 * Pre-insert logic in the proc: {@code @Id = ISNULL(MAX(CONVERT(int,Id)),0)+1} over the whole table (no Organization /
 * Company filter), then a plain single-row INSERT of Id, InwardGatePassId, Qty, UOM, GrossWeight, EbWeight, EBTotal,
 * NetWeight; it finishes with {@code SELECT @Id} (the new id). The existing entity keeps its IDENTITY strategy
 * (not changed per conventions); {@link #maxId()} is provided so a caller can reproduce the proc's app-assigned
 * numbering if the column is not an identity in the target DB. With {@code save()} the caller gets the id from the entity.
 */
@Repository
public interface InwardGatePassPurchaseBreakUpRepository extends JpaRepository<InwardGatePassPurchaseBreakUp, Integer> {

    /** USP_GatePassInwardPurchaseBreakUp_Insert — pre-insert {@code ISNULL(MAX(CONVERT(int,Id)),0)} (caller adds 1). */
    @Query("select coalesce(max(e.id),0) from InwardGatePassPurchaseBreakUp e")
    Integer maxId();
}
