package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;

import com.mst.models.RealUserRight;

public interface IRealUserRightRepository extends JpaRepository<RealUserRight, Integer> {

	List<RealUserRight> findByUserIdAndCompanyIdAndRightIdInAndValueTrue(Integer userId, Integer companyId, List<Integer> rightIds);
}
