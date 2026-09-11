package com.mst.services;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.mst.models.UserGroup;
import com.mst.repositories.IUserGroupRepository;
import com.mst.serviceInterface.IUserGroupService;

@Service
public class UserGroupService implements IUserGroupService {

	@Autowired
	private IUserGroupRepository userGroupRepository;

	@Override
	public List<UserGroup> getAll() {
		return userGroupRepository.findAllByOrderByUserGroupName();
	}

	@Override
	public UserGroup getById(Integer id) {
		return id == null ? null : userGroupRepository.findById(id).orElse(null);
	}

	@Override
	public UserGroup addOrUpdate(UserGroup userGroup) {
		return userGroupRepository.save(userGroup);
	}

	@Override
	public void delete(Integer id) {
		userGroupRepository.deleteById(id);
	}
}
