package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

import com.mst.models.SupplierCustomerType;

public interface ISupplierCustomerTypeRepository extends JpaRepository<SupplierCustomerType, Integer> {

	List<SupplierCustomerType> findAllByOrderBySupplierCustomerType();

	@Query("select coalesce(max(t.id), 0) from SupplierCustomerType t")
	int findMaxId();
}
