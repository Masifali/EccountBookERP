package com.mst.repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.PurchaseOrderLabDeduction;

/**
 * Sp_PurchaseOrderLabDeduction_Insert → {@code save(entity)} with {@code id = maxId() + 1}.
 * Pre-insert logic of the proc: Id = ISNULL(MAX(CONVERT(int,Id)),0)+1 (the @Id parameter is ignored); the proc returns the new Id.
 */
@Repository
public interface PurchaseOrderLabDeductionRepository extends JpaRepository<PurchaseOrderLabDeduction, Integer> {

    /** Sp_PurchaseOrderLabDeduction_Insert: SELECT ISNULL(MAX(CONVERT(int,Id)),0) FROM PurchaseOrderLabDeduction (caller adds 1). */
    @Query("select coalesce(max(e.id),0) from PurchaseOrderLabDeduction e")
    Integer maxId();

    /** Sp_PurchaseOrder_Update: DELETE PurchaseOrderLabDeduction WHERE PurchaseOrderId = @Id */
    @Modifying
    @Transactional
    @Query("DELETE FROM PurchaseOrderLabDeduction e WHERE e.purchaseOrderId = :purchaseOrderId")
    int deleteByPurchaseOrderId(@Param("purchaseOrderId") Integer purchaseOrderId);
}
