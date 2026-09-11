package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

import com.mst.models.ItemGroup;

public interface IItemGroupRepository extends JpaRepository<ItemGroup, Integer> {

	List<ItemGroup> findAllByOrderByItemGroupName();

	@Query("SELECT COALESCE(MAX(g.id), 0) FROM ItemGroup g")
	Integer findMaxId();
}
