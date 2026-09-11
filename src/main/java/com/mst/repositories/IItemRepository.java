package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

import com.mst.models.Item;

public interface IItemRepository extends JpaRepository<Item, Integer> {

	List<Item> findAllByOrderByItemName();

	@Query("SELECT COALESCE(MAX(i.id), 0) FROM Item i")
	Integer findMaxId();
}
