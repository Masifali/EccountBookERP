package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.Country;

/** dbo.Country - SP_Country_ReadMethod (@MethodType branches). Both branches select exactly the entity's columns, so the entity is the row. */
@Repository
public interface CountryRepository extends JpaRepository<Country, Integer> {

    /** SP_Country_ReadMethod @MethodType='GetAll' (no filter at all - @OrganizationId/@CompanyId are ignored by the proc). */
    @Query("SELECT c FROM Country c")
    List<Country> getAll();

    /** SP_Country_ReadMethod @MethodType='GetById' (named getCountryById because JpaRepository already declares getById(ID) with a different return type). */
    @Query("SELECT c FROM Country c WHERE c.id = :id")
    List<Country> getCountryById(@Param("id") Integer id);
}
