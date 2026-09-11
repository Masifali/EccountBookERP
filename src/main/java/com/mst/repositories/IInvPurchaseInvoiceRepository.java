package com.mst.repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.mst.models.InvPurchaseInvoice;

import java.util.List;

@Repository
public interface IInvPurchaseInvoiceRepository extends JpaRepository<InvPurchaseInvoice, Integer> {
    List<InvPurchaseInvoice> findByCompanyId(Integer companyId);
    List<InvPurchaseInvoice> findBySupplierCustomerId(Integer supplierCustomerId);
}
