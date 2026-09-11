package com.mst.services;

import com.mst.security.CurrentUserContext;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.mst.models.Item;
import com.mst.repositories.IItemRepository;
import com.mst.serviceInterface.IItemService;

@Service("itemService")
public class ItemService implements IItemService {

	@Autowired
	private IItemRepository itemRepository;
	@Autowired
	private CurrentUserContext currentUserContext;

	@Override
	public List<Item> getAll() {
		return itemRepository.findAllByOrderByItemName();
	}

	@Override
	public Item getById(int id) {
		return itemRepository.findById(id).orElse(null);
	}

	@Override
	public Item addOrUpdate(Item item) {
		LocalDateTime now = LocalDateTime.now();
		if (item.getId() == null || item.getId() == 0) {
			item.setId(itemRepository.findMaxId() + 1);
			item.setEntryDate(now);
			item.setEntryUser(currentUserContext.currentUserId());
			item.setOrganizationId(currentUserContext.currentOrganizationId());
			item.setCompanyId(currentUserContext.currentCompanyId());
			if (item.getItemStatus() == null) {
				item.setItemStatus(true);
			}
		} else {
			item.setModifyDate(now);
			item.setModifyUser(currentUserContext.currentUserId());
		}
		if (item.getItemCategory() != null && item.getItemCategory().getId() != null && item.getItemCategory().getId() == 0) {
			item.setItemCategory(null);
		}
		if (item.getItemType() != null && item.getItemType().getId() != null && item.getItemType().getId() == 0) {
			item.setItemType(null);
		}
		if (item.getRack() != null && item.getRack().getId() != null && item.getRack().getId() == 0) {
			item.setRack(null);
		}
		return itemRepository.save(item);
	}

	@Override
	public void delete(int id) {
		itemRepository.deleteById(id);
	}
}
