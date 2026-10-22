package com.mst.repositories;

import java.util.List;
import java.util.Map;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.City;
import com.mst.projections.CityNameRow;
import com.mst.projections.CityReadByIdRow;
import com.mst.projections.CityReadByOrganizationCompanyIdRow;
import com.mst.projections.CityRow;
import com.mst.projections.DistrictTehsilRow;

/**
 * dbo.City - SP_City_GetAllMethod (@MethodType branches). Tehsil and District have no entity, so every branch that
 * joins them is native. @CompanyId is accepted by the proc but never used in a filter.
 */
@Repository
public interface CityRepository extends JpaRepository<City, Integer> {

    /** SP_City_GetAllMethod @MethodType='GetAll' (native: Tehsil). */
    @Query(value = com.mst.repositories.support.CityQueries.ALL, nativeQuery = true)
    List<CityRow> getAll(@Param("organizationId") Integer organizationId);

    /** SP_City_GetAllMethod @MethodType='ReadByOrganizationCompanyId' (native: Tehsil). @CountryId optional, default NULL. */
    @Query(value = com.mst.repositories.support.CityQueries.BY_ORGANIZATION, nativeQuery = true)
    List<CityReadByOrganizationCompanyIdRow> readByOrganizationCompanyId(@Param("organizationId") Integer organizationId,
            @Param("countryId") Integer countryId);

    /** SP_City_GetAllMethod @MethodType='ReadById' (native: Tehsil; the city name comes back as plain "Description" in this branch). */
    @Query(value = com.mst.repositories.support.CityQueries.BY_ID, nativeQuery = true)
    List<CityReadByIdRow> readById(@Param("id") Integer id);

    /** SP_City_GetAllMethod @MethodType='ReadCityByTehsilId' (the proc's LEFT JOIN Tehsil contributes no column and cannot change the row set, so it is dropped). @TehsilId optional. */
    @Query("SELECT DISTINCT c.id AS id, c.description AS cityName FROM City c "
         + "WHERE c.organizationId = :organizationId AND (:tehsilId IS NULL OR c.tehsilId = :tehsilId)")
    List<CityNameRow> readCityByTehsilId(@Param("organizationId") Integer organizationId, @Param("tehsilId") Integer tehsilId);

    /** SP_City_GetAllMethod @MethodType='GetDistrictAndTehsilByCityId' (native: District, Tehsil). */
    @Query(value = com.mst.repositories.support.CityQueries.DISTRICT_TEHSIL, nativeQuery = true)
    List<DistrictTehsilRow> getDistrictAndTehsilByCityId(@Param("organizationId") Integer organizationId, @Param("id") Integer id);

    @Query(value = com.mst.repositories.support.CityQueries.ALL, nativeQuery = true)
    List<Map<String, Object>> getAllRows(@Param("organizationId") Integer organizationId);

    @Query(value = com.mst.repositories.support.CityQueries.BY_ORGANIZATION, nativeQuery = true)
    List<Map<String, Object>> readOrganizationRows(@Param("organizationId") Integer organizationId, @Param("countryId") Integer countryId);

    @Query(value = com.mst.repositories.support.CityQueries.BY_ID, nativeQuery = true)
    List<Map<String, Object>> readIdRows(@Param("id") Integer id);

    @Query(value = "SELECT DISTINCT c.Id, c.Description AS CityName FROM City c WHERE c.OrganizationId = :organizationId AND (:tehsilId IS NULL OR c.TehsilId = :tehsilId)", nativeQuery = true)
    List<Map<String, Object>> readTehsilRows(@Param("organizationId") Integer organizationId, @Param("tehsilId") Integer tehsilId);

    @Query(value = com.mst.repositories.support.CityQueries.DISTRICT_TEHSIL, nativeQuery = true)
    List<Map<String, Object>> readDistrictRows(@Param("organizationId") Integer organizationId, @Param("id") Integer id);
}
