package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;

import com.mst.models.RealScreenRight;

public interface IRealScreenRightRepository extends JpaRepository<RealScreenRight, Integer> {

	List<RealScreenRight> findByScreenIdInAndRightName(List<Integer> screenIds, String rightName);
}
