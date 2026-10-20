package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.InvPackingType;
import com.mst.projections.InvPackingTypeHistoryRow;
import com.mst.projections.InvPackingTypeReadAllRow;
import com.mst.projections.InvPackingTypeRow;

@Repository
public interface InvPackingTypeRepository extends JpaRepository<InvPackingType, Integer> {

    /** Sp_InvPackingType_GetAllMethod @Activity='ReadById' */
    @Query("SELECT p.id AS id, p.packTypeCode AS packTypeCode, p.packTypeDesc AS packTypeDesc, p.isActive AS isActive, "
         + "p.minEbWeight AS minEbWeight, p.maxEbWeight AS maxEbWeight, p.minEbStockWeight AS minEbStockWeight, "
         + "p.maxEbStockWeight AS maxEbStockWeight "
         + "FROM InvPackingType p WHERE p.id = :id")
    List<InvPackingTypeRow> readById(@Param("id") Integer id);

    /** Sp_InvPackingType_GetAllMethod @Activity='ReadAll' (only active rows: ISNULL(IsActive,0)=1) */
    @Query("SELECT p.id AS id, p.packTypeCode AS packTypeCode, p.packTypeDesc AS packTypeDesc, "
         + "p.minEbWeight AS minEbWeight, p.maxEbWeight AS maxEbWeight, p.minEbStockWeight AS minEbStockWeight, "
         + "p.maxEbStockWeight AS maxEbStockWeight "
         + "FROM InvPackingType p WHERE COALESCE(p.isActive, false) = true")
    List<InvPackingTypeReadAllRow> readAll();

    /** Sp_InvPackingType_GetAllMethod @Activity='FormHistory' (all rows) */
    @Query("SELECT p.id AS id, p.packTypeCode AS packTypeCode, p.packTypeDesc AS packTypeDesc, p.isActive AS isActive "
         + "FROM InvPackingType p")
    List<InvPackingTypeHistoryRow> formHistory();
}
