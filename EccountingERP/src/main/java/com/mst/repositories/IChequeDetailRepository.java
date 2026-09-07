package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

import com.mst.models.ChequeDetail;

public interface IChequeDetailRepository extends JpaRepository<ChequeDetail, Long> {

	List<ChequeDetail> findByActiveTrueAndChequeInHandAccountOrderByChequeDate(String chequeInHandAccount);

	List<ChequeDetail> findByChequeInHandAccount(String chequeInHandAccount);

	@Query("select coalesce(max(c.id), 0) from ChequeDetail c")
	Long findMaxId();
}
