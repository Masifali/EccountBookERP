package com.mst.repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.mst.models.PurchaseOrder;

import java.util.List;

@Repository
public interface IPurchaseOrderRepository extends JpaRepository<PurchaseOrder, Integer> {
    List<PurchaseOrder> findByCompanyId(Integer companyId);
    List<PurchaseOrder> findBySupplierCustomerId(Integer supplierCustomerId);
}
