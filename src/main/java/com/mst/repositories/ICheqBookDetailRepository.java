package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import com.mst.models.CheqBookDetail;

@Repository
public interface ICheqBookDetailRepository extends JpaRepository<CheqBookDetail, Long> {

	List<CheqBookDetail> findByCheqBookHeaderIdOrderByIdAsc(Long cheqBookHeaderId);

	@Query("select coalesce(max(c.id), 0) from CheqBookDetail c")
	Long findMaxId();
}
