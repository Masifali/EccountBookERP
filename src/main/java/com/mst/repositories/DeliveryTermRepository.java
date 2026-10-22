package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.DeliveryTerm;

/** dbo.DeliveryTerm - USP_DeliveryTerm_GetAllMethod (@Activity branches). Both branches return exactly the entity's columns (Id, Description, ValueDescription). */
@Repository
public interface DeliveryTermRepository extends JpaRepository<DeliveryTerm, Integer> {

    /** USP_DeliveryTerm_GetAllMethod @Activity='ReadById' */
    @Query("SELECT h FROM DeliveryTerm h WHERE h.id = :id")
    List<DeliveryTerm> readById(@Param("id") Integer id);

    /** USP_DeliveryTerm_GetAllMethod @Activity='FormHistory' (all rows, no filter). */
    @Query("SELECT h FROM DeliveryTerm h")
    List<DeliveryTerm> formHistory();
}
