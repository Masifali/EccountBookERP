package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

import com.mst.models.Bank;

public interface IBankRepository extends JpaRepository<Bank, Integer> {

	List<Bank> findAllByOrderByBranchName();

	@Query("select coalesce(max(b.id), 0) from Bank b")
	int findMaxId();
}
