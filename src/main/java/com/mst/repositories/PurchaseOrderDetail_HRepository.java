package com.mst.repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.PurchaseOrderDetail_H;

/**
 * History rows of PurchaseOrderDetail. Written by Sp_PurchaseOrderDetail_Insert (update branch, ActionTypeId = 2,
 * ModifyDate = GETDATE(), ModifyUserId = PurchaseOrder.ModifyUser, RevisionNo = maxRevisionNo(orderDetailId, 2) + 1)
 * and by USP_DeletePurchaseOrderDetailIfNotExistInGrn (ActionTypeId = 3, one row per deleted detail,
 * Id = ROW_NUMBER() + maxId()). Inserts via {@code save(entity)} with an app-assigned Id.
 */
@Repository
public interface PurchaseOrderDetail_HRepository extends JpaRepository<PurchaseOrderDetail_H, Integer> {

    /** Sp_PurchaseOrderDetail_Insert / USP_DeletePurchaseOrderDetailIfNotExistInGrn: SELECT ISNULL(MAX(CONVERT(INT,Id)),0) FROM PurchaseOrderDetail_H (caller adds 1). */
    @Query("select coalesce(max(e.id),0) from PurchaseOrderDetail_H e")
    Integer maxId();

    /** Sp_PurchaseOrderDetail_Insert: SELECT ISNULL(MAX(RevisionNo),0) FROM PurchaseOrderDetail_H WHERE OrderDetailId = @Id AND ActionTypeId = 2 (caller adds 1; proc passes actionTypeId = 2). */
    @Query("select coalesce(max(e.revisionNo),0) from PurchaseOrderDetail_H e "
         + "where e.orderDetailId = :orderDetailId and e.actionTypeId = :actionTypeId")
    Integer maxRevisionNo(@Param("orderDetailId") Integer orderDetailId, @Param("actionTypeId") Integer actionTypeId);
}
