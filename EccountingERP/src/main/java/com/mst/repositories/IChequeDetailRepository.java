package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;

import com.mst.models.ChequeDetail;

public interface IChequeDetailRepository extends JpaRepository<ChequeDetail, Long> {

	List<ChequeDetail> findByActiveTrueAndChequeInHandAccountOrderByChequeDate(String chequeInHandAccount);
}
