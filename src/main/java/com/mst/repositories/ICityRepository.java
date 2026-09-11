package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

import com.mst.models.City;

public interface ICityRepository extends JpaRepository<City, Integer> {

	List<City> findAllByOrderByCityName();

	City findByCityNameIgnoreCase(String cityName);

	@Query("select coalesce(max(c.id), 0) from City c")
	int findMaxId();
}
