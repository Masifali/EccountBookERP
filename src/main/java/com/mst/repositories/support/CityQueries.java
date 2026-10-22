package com.mst.repositories.support;

import com.mst.repositories.CityRepository;
import org.springframework.stereotype.Component;
import java.util.*;

/** Query-backed city reads, preserving the legacy result columns and organization scope. */
@Component
public class CityQueries {
    public static final String ALL = "SELECT City.Id, City.Code, City.Description AS CityName, City.CityNameOtherLingo, City.EntryDate, City.EntryUser, City.ModifyDate, "
         + "City.ModifyUser, City.PostDate, City.PostUser, City.PostState, City.OrganizationId, City.CompanyId, City.CountryId, City.TehsilId, "
         + "Country.Description AS CountryName, t.Description AS Tehsil "
         + "FROM City LEFT JOIN Country ON City.CountryId = Country.Id LEFT JOIN Tehsil T ON City.TehsilId = t.Id "
         + "WHERE City.OrganizationId = :organizationId";
    public static final String BY_ORGANIZATION = "SELECT City.Id, City.Code, City.Description AS CityName, CityNameOtherLingo, City.EntryDate, City.EntryUser, City.ModifyDate, "
         + "City.ModifyUser, City.PostDate, City.PostUser, City.PostState, City.OrganizationId, City.CompanyId, City.CountryId, "
         + "Country.Description AS CountryName, t.Description AS Tehsil "
         + "FROM City LEFT JOIN Country ON City.CountryId = Country.Id LEFT JOIN Tehsil T ON City.TehsilId = t.Id "
         + "WHERE City.OrganizationId = :organizationId AND (:countryId IS NULL OR City.CountryId = :countryId)";
    public static final String BY_ID = "SELECT City.Id, City.Code, City.Description, City.CityNameOtherLingo, City.EntryDate, City.EntryUser, City.ModifyDate, City.ModifyUser, "
         + "City.PostDate, City.PostUser, City.PostState, City.OrganizationId, City.CompanyId, City.CountryId, City.TehsilId, "
         + "Country.Description AS CountryName, t.Description AS Tehsil "
         + "FROM City LEFT JOIN Country ON City.CountryId = Country.Id LEFT JOIN Tehsil T ON City.TehsilId = t.Id "
         + "WHERE City.Id = :id";
    public static final String DISTRICT_TEHSIL = "SELECT DISTINCT t.DistrictId, d.Description AS District, c.TehsilId, c.Description AS Tehsil "
         + "FROM District d INNER JOIN Tehsil t ON d.Id = t.DistrictId INNER JOIN City c ON t.Id = c.TehsilId "
         + "WHERE d.OrganizationId = :organizationId AND c.Id = :id";
    private final CityRepository repository;

    public CityQueries(CityRepository repository) { this.repository = repository; }

    public List<Map<String, Object>> rows(Object... parameters) {
        return rows(DesktopProc.params(parameters));
    }

    public List<Map<String, Object>> rows(Map<String, Object> parameters) {
        Map<String, Object> p = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
        parameters.forEach((key, value) -> p.put(key.replace("@", ""), value));
        Integer org = number(p.get("OrganizationId"));
        Object method = p.get("MethodType");
        List<Map<String, Object>> result;
        if (method == null) return new ArrayList<>();
        switch (method.toString().toLowerCase(Locale.ROOT)) {
            case "getall": result = repository.getAllRows(org); break;
            case "readbyorganizationcompanyid": result = repository.readOrganizationRows(org, number(p.get("CountryId"))); break;
            case "readbyid": result = repository.readIdRows(number(p.get("Id"))); break;
            case "readcitybytehsilid": result = repository.readTehsilRows(org, number(p.get("TehsilId"))); break;
            case "getdistrictandtehsilbycityid": result = repository.readDistrictRows(org, number(p.get("Id"))); break;
            default: return new ArrayList<>();
        }
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> row : result) {
            Map<String, Object> copy = new org.springframework.util.LinkedCaseInsensitiveMap<>();
            copy.putAll(row);
            out.add(copy);
        }
        return out;
    }

    private static Integer number(Object value) {
        return value == null ? null : value instanceof Number ? ((Number) value).intValue() : Integer.valueOf(value.toString());
    }
}
