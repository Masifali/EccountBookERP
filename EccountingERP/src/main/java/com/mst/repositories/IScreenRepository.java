package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;

import com.mst.models.Screen;

public interface IScreenRepository extends JpaRepository<Screen, Integer> {

	List<Screen> findAllByOrderByModuleDescriptionAscScreenNameAsc();

	boolean existsByTargetUrl(String targetUrl);

	Screen findByTargetUrl(String targetUrl);

	/** This port's own built screens under a given real Accounts sub-module (see Screen.realModuleId). */
	List<Screen> findByRealModuleIdOrderByScreenNameAsc(Integer realModuleId);

	long countByRealModuleId(Integer realModuleId);
}
