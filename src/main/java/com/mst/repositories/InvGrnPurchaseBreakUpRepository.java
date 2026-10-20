package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import com.mst.models.InvGrnPurchaseBreakUp;

/**
 * USP_InvGrnPurchaseBreakUp_Insert → entity + save().
 * Pre-insert logic from the proc: Id = ISNULL(MAX(CONVERT(int,Id)),0)+1 over the whole table
 * (no organization/company scope) — caller sets id = maxId()+1 then save(). The proc ends with
 * SELECT @Id; with save() the caller already holds the id on the entity.
 * Proc defaults: @Id/@InvGrnId/@InwardGatePassId/@InwardBreakupId/@NetPackSize/@SupplierRcvdWeight/
 * @ScaleShortage/@BillWeight default NULL. NetBillWeight is a table column not written by the proc.
 */
@Repository
public interface InvGrnPurchaseBreakUpRepository extends JpaRepository<InvGrnPurchaseBreakUp, Integer> {

    /** USP_InvGrnPurchaseBreakUp_Insert: SELECT ISNULL(Max(CONVERT(int,Id)),0) FROM InvGrnPurchaseBreakUp (add 1 for the new Id) */
    @Query("select coalesce(max(e.id),0) from InvGrnPurchaseBreakUp e")
    Integer maxId();

    List<InvGrnPurchaseBreakUp> findByInvGrnId(Integer invGrnId);

    List<InvGrnPurchaseBreakUp> findByInwardGatePassId(Integer inwardGatePassId);
}
