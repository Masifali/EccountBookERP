package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

import com.mst.models.SupplierCustomer;

public interface ISupplierCustomerRepository extends JpaRepository<SupplierCustomer, Integer> {

	List<SupplierCustomer> findAllByOrderByCompanyName();

	List<SupplierCustomer> findByCustomerTypeIdOrderByCompanyName(Integer customerTypeId);

	@Query("select coalesce(max(s.id), 0) from SupplierCustomer s")
	int findMaxId();
}
