package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import com.mst.models.VehicleType;

/** dbo.VehicleType - Sp_VehicleType_GetAllMethod (single branch: SELECT * FROM VehicleType, no parameters). */
@Repository
public interface VehicleTypeRepository extends JpaRepository<VehicleType, Integer> {

    /** Sp_VehicleType_GetAllMethod (SELECT * - the entity carries the columns the project reads: Id, VehicleDescription). */
    @Query("SELECT v FROM VehicleType v")
    List<VehicleType> getAll();
}
