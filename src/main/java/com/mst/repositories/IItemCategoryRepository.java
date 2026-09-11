package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

import com.mst.models.ItemCategory;

public interface IItemCategoryRepository extends JpaRepository<ItemCategory, Integer> {

	List<ItemCategory> findAllByOrderByCategoryDescription();

	@Query("SELECT COALESCE(MAX(c.id), 0) FROM ItemCategory c")
	Integer findMaxId();
}
