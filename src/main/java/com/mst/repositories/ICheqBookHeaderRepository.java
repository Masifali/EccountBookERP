package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import com.mst.models.CheqBookHeader;

@Repository
public interface ICheqBookHeaderRepository extends JpaRepository<CheqBookHeader, Long> {

	List<CheqBookHeader> findAllByOrderByIdDesc();

	@Query("select coalesce(max(c.docNo), 0) from CheqBookHeader c")
	Integer findMaxDocNo();

	@Query("select coalesce(max(c.id), 0) from CheqBookHeader c")
	Long findMaxId();
}
