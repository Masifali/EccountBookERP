package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

import com.mst.models.ItemType;

public interface IItemTypeRepository extends JpaRepository<ItemType, Integer> {

	List<ItemType> findAllByOrderByTypeDescription();

	@Query("SELECT COALESCE(MAX(t.id), 0) FROM ItemType t")
	Integer findMaxId();
}
