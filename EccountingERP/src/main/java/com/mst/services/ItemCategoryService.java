package com.mst.services;

import com.mst.constants.MSTConstants;
import com.mst.models.ItemCategory;
import com.mst.repositories.IItemCategoryRepository;
import com.mst.serviceInterface.IItemCategoryService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;

@Service("itemCategoryService")
public class ItemCategoryService implements IItemCategoryService {

    @Autowired
    private IItemCategoryRepository itemCategoryRepository;

    @Override
    public ItemCategory getItemCategoryById(long itemCategoryId) {
        return itemCategoryRepository.findById(itemCategoryId).orElse(null);
    }

    @Override
    public ItemCategory getItemCategoryByCode(long itemCategoryCode) {
        return itemCategoryRepository.findByCode(itemCategoryCode);
    }

    @Override
    public List<ItemCategory> getAllItemCategories() {
        return itemCategoryRepository.findAll();
    }

    @Override
    public ItemCategory addOrUpdateItemCategory(ItemCategory itemCategory) {

        ItemCategory itemCategoryExists = itemCategoryRepository.findById(itemCategory.getId()).orElse(null);
        if (itemCategoryExists != null) {
            itemCategoryExists.setName(itemCategory.getName());
            itemCategory = itemCategoryExists;
        } else {
            itemCategory.setCode(getNextCode());
            itemCategory.setFormattedCode(String.valueOf(itemCategory.getCode()));
        }

        itemCategory.setName(itemCategory.getName().toUpperCase());

        itemCategoryRepository.save(itemCategory);
        return itemCategory;
    }

    @Override
    public long getNextCode() {
        ItemCategory itemCategory = itemCategoryRepository.findTopByOrderByCodeDesc();
        if (itemCategory != null) {
            return itemCategory.getCode() + 1;
        } else {
            return MSTConstants.FIRST_LEVEL_ITEM_STARTING_VALUE;
        }
    }
}
