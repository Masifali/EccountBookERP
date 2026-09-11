package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

import com.mst.models.CustomerGroup;

public interface ICustomerGroupRepository extends JpaRepository<CustomerGroup, Integer> {

	List<CustomerGroup> findAllByOrderByDescription();

	@Query("select coalesce(max(g.id), 0) from CustomerGroup g")
	int findMaxId();
}
