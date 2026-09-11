package com.mst.services;

import com.mst.security.CurrentUserContext;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.mst.models.ItemGroup;
import com.mst.repositories.IItemGroupRepository;
import com.mst.serviceInterface.IItemGroupService;

@Service("itemGroupService")
public class ItemGroupService implements IItemGroupService {

	@Autowired
	private IItemGroupRepository itemGroupRepository;
	@Autowired
	private CurrentUserContext currentUserContext;

	@Override
	public List<ItemGroup> getAll() {
		return itemGroupRepository.findAllByOrderByItemGroupName();
	}

	@Override
	public ItemGroup getById(int id) {
		return itemGroupRepository.findById(id).orElse(null);
	}

	@Override
	public ItemGroup addOrUpdate(ItemGroup itemGroup) {
		if (itemGroup.getId() == null || itemGroup.getId() == 0) {
			itemGroup.setId(itemGroupRepository.findMaxId() + 1);
			itemGroup.setOrganizationId(currentUserContext.currentOrganizationId());
			itemGroup.setCompanyId(currentUserContext.currentCompanyId());
		}
		return itemGroupRepository.save(itemGroup);
	}

	@Override
	public void delete(int id) {
		itemGroupRepository.deleteById(id);
	}
}
