package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;

import com.mst.models.UserGroup;

public interface IUserGroupRepository extends JpaRepository<UserGroup, Integer> {

	List<UserGroup> findAllByOrderByUserGroupName();

	UserGroup findByUserGroupRole(String userGroupRole);
}
