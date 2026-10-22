package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.ItemClass;

@Repository
public interface ItemClassRepository extends JpaRepository<ItemClass, Integer> {

    /** Sp_ItemClass_GetAllMethod @Activity='ReadAll' (no filter; @OrganizationId/@CompanyId unused). Output = all entity columns. */
    @Query("SELECT c FROM ItemClass c")
    List<ItemClass> readAll();

    /** Sp_ItemClass_GetAllMethod @Activity='ReadByClassId'. Output = all entity columns. */
    @Query("SELECT c FROM ItemClass c WHERE c.classId = :classId")
    List<ItemClass> readByClassId(@Param("classId") Integer classId);
}
