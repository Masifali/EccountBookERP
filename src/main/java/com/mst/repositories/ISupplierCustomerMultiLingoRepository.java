package com.mst.repositories;

import com.mst.models.SupplierCustomerMultiLingo;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface ISupplierCustomerMultiLingoRepository extends JpaRepository<SupplierCustomerMultiLingo, Integer> {
    List<SupplierCustomerMultiLingo> findBySupplierCustomerId(Integer supplierCustomerId);
    void deleteBySupplierCustomerId(Integer supplierCustomerId);
}
