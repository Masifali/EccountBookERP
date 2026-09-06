package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;

import com.mst.models.VoucherDetail;

public interface IVoucherDetailRepository extends JpaRepository<VoucherDetail, Integer> {

	List<VoucherDetail> findByVoucherHead_IdOrderByIdAsc(int voucherHeadId);

	void deleteByVoucherHead_Id(int voucherHeadId);
}
