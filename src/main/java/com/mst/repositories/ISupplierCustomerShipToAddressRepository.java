package com.mst.repositories;

import com.mst.models.SupplierCustomerShipToAddress;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface ISupplierCustomerShipToAddressRepository extends JpaRepository<SupplierCustomerShipToAddress, Integer> {
    List<SupplierCustomerShipToAddress> findBySupplierCustomerId(Integer supplierCustomerId);
    void deleteBySupplierCustomerId(Integer supplierCustomerId);
}
