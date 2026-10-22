package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.Projects;

/** dbo.Projects - Sp_Projects_GetAllMethod (@MethodType branches). Both branches select exactly the entity's columns. */
@Repository
public interface ProjectsRepository extends JpaRepository<Projects, Integer> {

    /** Sp_Projects_GetAllMethod @MethodType='GetAll' */
    @Query("SELECT p FROM Projects p WHERE p.organizationId = :organizationId AND p.companyId = :companyId")
    List<Projects> getAll(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /** Sp_Projects_GetAllMethod @MethodType='GetByID' */
    @Query("SELECT p FROM Projects p WHERE p.id = :id")
    List<Projects> getByID(@Param("id") Integer id);
}
