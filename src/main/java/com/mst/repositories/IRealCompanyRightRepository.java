package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;

import com.mst.models.RealCompanyRight;

public interface IRealCompanyRightRepository extends JpaRepository<RealCompanyRight, Integer> {

	List<RealCompanyRight> findByCompanyIdAndScreenIdInAndIsActiveTrue(Integer companyId, List<Integer> screenIds);
}
