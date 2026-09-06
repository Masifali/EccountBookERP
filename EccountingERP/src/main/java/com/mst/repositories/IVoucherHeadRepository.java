package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;

import com.mst.models.VoucherHead;

public interface IVoucherHeadRepository extends JpaRepository<VoucherHead, Integer> {

	List<VoucherHead> findByDocumentType_IdOrderByVoucherDateDescIdDesc(int documentTypeId);
}
