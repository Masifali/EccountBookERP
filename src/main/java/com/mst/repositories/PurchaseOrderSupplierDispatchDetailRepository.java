package com.mst.repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.PurchaseOrderSupplierDispatchDetail;

/**
 * USP_PurchaseOrderSupplierDispatchDetail_Insert → {@code save(entity)} with {@code id = maxId() + 1}.
 * Pre-insert logic of the proc: Id = ISNULL(MAX(Id),0)+1 (the @Id parameter is ignored); the proc returns the new Id.
 */
@Repository
public interface PurchaseOrderSupplierDispatchDetailRepository extends JpaRepository<PurchaseOrderSupplierDispatchDetail, Integer> {

    /** USP_PurchaseOrderSupplierDispatchDetail_Insert: SELECT ISNULL(MAX(Id),0) FROM PurchaseOrderSupplierDispatchDetail (caller adds 1). */
    @Query("select coalesce(max(e.id),0) from PurchaseOrderSupplierDispatchDetail e")
    Integer maxId();

    /** Sp_PurchaseOrder_Update: Delete PurchaseOrderSupplierDispatchDetail Where PurchaseOrderId = @Id */
    @Modifying
    @Transactional
    @Query("DELETE FROM PurchaseOrderSupplierDispatchDetail e WHERE e.purchaseOrderId = :purchaseOrderId")
    int deleteByPurchaseOrderId(@Param("purchaseOrderId") Integer purchaseOrderId);
}
