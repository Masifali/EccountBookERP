package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

import com.mst.models.Brand;

public interface IBrandRepository extends JpaRepository<Brand, Integer> {

	List<Brand> findAllByOrderByBrandName();

	@Query("SELECT COALESCE(MAX(b.id), 0) FROM Brand b")
	Integer findMaxId();
}
