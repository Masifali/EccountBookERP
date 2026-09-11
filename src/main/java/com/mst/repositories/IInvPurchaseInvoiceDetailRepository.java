package com.mst.repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.mst.models.InvPurchaseInvoiceDetail;

import java.util.List;

@Repository
public interface IInvPurchaseInvoiceDetailRepository extends JpaRepository<InvPurchaseInvoiceDetail, Integer> {
    List<InvPurchaseInvoiceDetail> findByInvPurchaseInvoiceId(Integer invPurchaseInvoiceId);
}
