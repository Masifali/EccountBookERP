package com.mst.services;

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

    public ItemCategory getItemCategoryById(int itemCategoryId) {
        return itemCategoryRepository.findById(itemCategoryId).orElse(null);
    }

    public List<ItemCategory> getAllItemCategories() {
        return itemCategoryRepository.findAll();
    }

    public ItemCategory save(ItemCategory itemCategory) {
        return itemCategoryRepository.save(itemCategory);
    }
}
