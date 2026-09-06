package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

import com.mst.models.ProductType;

public interface IProductTypeRepository extends JpaRepository<ProductType, Integer> {

	List<ProductType> findAllByOrderByProductTypeDescription();

	@Query("SELECT COALESCE(MAX(p.id), 0) FROM ProductType p")
	Integer findMaxId();
}
