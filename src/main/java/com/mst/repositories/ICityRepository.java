package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

import com.mst.models.City;

public interface ICityRepository extends JpaRepository<City, Integer> {

	// Desktop city lists expose Description as CityName; keep names added by the Java city dialog too.
	@Query(value = "SELECT Id, COALESCE(NULLIF(LTRIM(RTRIM(Description)), ''), LTRIM(RTRIM(CityName))) AS CityName, "
			+ "TehsilId, CompanyId, OrganizationId FROM dbo.City "
			+ "WHERE NULLIF(LTRIM(RTRIM(Description)), '') IS NOT NULL "
			+ "OR NULLIF(LTRIM(RTRIM(CityName)), '') IS NOT NULL ORDER BY CityName, Id", nativeQuery = true)
	List<City> findAllByOrderByCityName();

	City findByCityNameIgnoreCase(String cityName);

	@Query("select coalesce(max(c.id), 0) from City c")
	int findMaxId();
}
