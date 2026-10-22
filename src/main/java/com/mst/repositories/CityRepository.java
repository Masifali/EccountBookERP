package com.mst.repositories;

import java.util.List;

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
    @Query(value = "SELECT City.Id, City.Code, City.Description AS CityName, City.CityNameOtherLingo, City.EntryDate, City.EntryUser, City.ModifyDate, "
         + "City.ModifyUser, City.PostDate, City.PostUser, City.PostState, City.OrganizationId, City.CompanyId, City.CountryId, City.TehsilId, "
         + "Country.Description AS CountryName, t.Description AS Tehsil "
         + "FROM City LEFT JOIN Country ON City.CountryId = Country.Id LEFT JOIN Tehsil T ON City.TehsilId = t.Id "
         + "WHERE City.OrganizationId = :organizationId", nativeQuery = true)
    List<CityRow> getAll(@Param("organizationId") Integer organizationId);

    /** SP_City_GetAllMethod @MethodType='ReadByOrganizationCompanyId' (native: Tehsil). @CountryId optional, default NULL. */
    @Query(value = "SELECT City.Id, City.Code, City.Description AS CityName, CityNameOtherLingo, City.EntryDate, City.EntryUser, City.ModifyDate, "
         + "City.ModifyUser, City.PostDate, City.PostUser, City.PostState, City.OrganizationId, City.CompanyId, City.CountryId, "
         + "Country.Description AS CountryName, t.Description AS Tehsil "
         + "FROM City LEFT JOIN Country ON City.CountryId = Country.Id LEFT JOIN Tehsil T ON City.TehsilId = t.Id "
         + "WHERE City.OrganizationId = :organizationId AND (:countryId IS NULL OR City.CountryId = :countryId)", nativeQuery = true)
    List<CityReadByOrganizationCompanyIdRow> readByOrganizationCompanyId(@Param("organizationId") Integer organizationId,
            @Param("countryId") Integer countryId);

    /** SP_City_GetAllMethod @MethodType='ReadById' (native: Tehsil; the city name comes back as plain "Description" in this branch). */
    @Query(value = "SELECT City.Id, City.Code, City.Description, City.CityNameOtherLingo, City.EntryDate, City.EntryUser, City.ModifyDate, City.ModifyUser, "
         + "City.PostDate, City.PostUser, City.PostState, City.OrganizationId, City.CompanyId, City.CountryId, City.TehsilId, "
         + "Country.Description AS CountryName, t.Description AS Tehsil "
         + "FROM City LEFT JOIN Country ON City.CountryId = Country.Id LEFT JOIN Tehsil T ON City.TehsilId = t.Id "
         + "WHERE City.Id = :id", nativeQuery = true)
    List<CityReadByIdRow> readById(@Param("id") Integer id);

    /** SP_City_GetAllMethod @MethodType='ReadCityByTehsilId' (the proc's LEFT JOIN Tehsil contributes no column and cannot change the row set, so it is dropped). @TehsilId optional. */
    @Query("SELECT DISTINCT c.id AS id, c.description AS cityName FROM City c "
         + "WHERE c.organizationId = :organizationId AND (:tehsilId IS NULL OR c.tehsilId = :tehsilId)")
    List<CityNameRow> readCityByTehsilId(@Param("organizationId") Integer organizationId, @Param("tehsilId") Integer tehsilId);

    /** SP_City_GetAllMethod @MethodType='GetDistrictAndTehsilByCityId' (native: District, Tehsil). */
    @Query(value = "SELECT DISTINCT t.DistrictId, d.Description AS District, c.TehsilId, c.Description AS Tehsil "
         + "FROM District d INNER JOIN Tehsil t ON d.Id = t.DistrictId INNER JOIN City c ON t.Id = c.TehsilId "
         + "WHERE d.OrganizationId = :organizationId AND c.Id = :id", nativeQuery = true)
    List<DistrictTehsilRow> getDistrictAndTehsilByCityId(@Param("organizationId") Integer organizationId, @Param("id") Integer id);
}
