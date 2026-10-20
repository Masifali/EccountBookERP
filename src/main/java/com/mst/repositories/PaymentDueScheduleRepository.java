package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.PaymentDueSchedule;

/**
 * Sp_PaymentDueSchedule_Insert → {@code save(entity)}.
 * Pre-insert logic of the proc (caller must replicate): if DueDays is null or 0 then DueDays = 1 and DuePercentage = 100.
 * The proc returns SCOPE_IDENTITY(); with save() the caller reads {@code getPaymentDueScheduleId()} from the saved entity.
 */
@Repository
public interface PaymentDueScheduleRepository extends JpaRepository<PaymentDueSchedule, Integer> {

    /** Sp_PaymentDueSchedule_ReadAll (single branch). Output = all entity columns in table order. */
    @Query("SELECT p FROM PaymentDueSchedule p "
         + "WHERE p.refDocumentTypeId = :documentTypeId AND p.refdocNoId = :refDocNoId")
    List<PaymentDueSchedule> readAll(@Param("documentTypeId") Integer documentTypeId,
                                     @Param("refDocNoId") Integer refDocNoId);
}
