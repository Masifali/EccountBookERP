package com.mst.repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.mst.models.RealScreenDefinition;

@Repository
public interface IRealScreenDefinitionRepository extends JpaRepository<RealScreenDefinition, Integer> {
	RealScreenDefinition findByScreenName(String screenName);
	RealScreenDefinition findByTargetUrl(String targetUrl);
}
