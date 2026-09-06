package com.mst.services;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.mst.models.ItemType;
import com.mst.repositories.IItemTypeRepository;
import com.mst.security.CurrentUserContext;
import com.mst.serviceInterface.IItemTypeService;

@Service("itemTypeService")
public class ItemTypeService implements IItemTypeService {

	@Autowired
	private IItemTypeRepository itemTypeRepository;
	@Autowired
	private CurrentUserContext currentUserContext;

	@Override
	public List<ItemType> getAll() {
		return itemTypeRepository.findAllByOrderByTypeDescription();
	}

	@Override
	public ItemType getById(int id) {
		return itemTypeRepository.findById(id).orElse(null);
	}

	@Override
	public ItemType addOrUpdate(ItemType itemType) {
		LocalDateTime now = LocalDateTime.now();
		if (itemType.getId() == null || itemType.getId() == 0) {
			itemType.setId(itemTypeRepository.findMaxId() + 1);
			itemType.setEntryDate(now);
			itemType.setEntryUser(currentUserContext.currentUserId());
			itemType.setOrganizationId(currentUserContext.currentOrganizationId());
			itemType.setCompanyId(currentUserContext.currentCompanyId());
			itemType.setPostState(false);
		} else {
			itemType.setModifyDate(now);
			itemType.setModifyUser(currentUserContext.currentUserId());
		}
		if (itemType.getParentCategory() != null && itemType.getParentCategory().getId() != null
				&& itemType.getParentCategory().getId() == 0) {
			itemType.setParentCategory(null);
		}
		return itemTypeRepository.save(itemType);
	}

	@Override
	public void delete(int id) {
		itemTypeRepository.deleteById(id);
	}
}
