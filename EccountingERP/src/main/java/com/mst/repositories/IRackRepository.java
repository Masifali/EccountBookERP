package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

import com.mst.models.Rack;

public interface IRackRepository extends JpaRepository<Rack, Integer> {

	List<Rack> findAllByOrderBySortNo();

	@Query("SELECT COALESCE(MAX(r.id), 0) FROM Rack r")
	Integer findMaxId();
}
