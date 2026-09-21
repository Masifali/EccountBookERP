package com.mst.repositories;

import java.util.List;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import com.mst.models.BankCharges;

public interface IBankChargesRepository extends JpaRepository<BankCharges, Integer> {

	List<BankCharges> findAllByOrderByIdDesc();

	@Query("select coalesce(max(bc.id), 0) from BankCharges bc")
	int findMaxId();
}
