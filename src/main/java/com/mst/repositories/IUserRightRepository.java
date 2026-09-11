package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

import com.mst.constants.RightType;
import com.mst.models.UserRight;

public interface IUserRightRepository extends JpaRepository<UserRight, Integer> {

	/**
	 * join fetch ur.screen here (same fix as IUserAccountRepository.findByUserName) -
	 * CustomUserDetailsService reads right.getScreen() for every row this returns,
	 * well after this call's own session would otherwise have closed, which throws
	 * LazyInitializationException without the fetch join.
	 */
	@Query("select ur from UserRight ur join fetch ur.screen where ur.user.id = ?1")
	List<UserRight> findByUserId(Integer userId);

	UserRight findByUserIdAndScreenIdAndRightType(Integer userId, Integer screenId, RightType rightType);

	boolean existsByUserIdAndScreenIdAndRightTypeAndValueTrue(Integer userId, Integer screenId, RightType rightType);
}
