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

    @Override
    public ItemCategory getItemCategoryById(int itemCategoryId) {
        return itemCategoryRepository.findById(itemCategoryId).orElse(null);
    }

    @Override
    public List<ItemCategory> getAllItemCategories() {
        return itemCategoryRepository.findAllByOrderByCategoryDescription();
    }

    @Override
    public ItemCategory save(ItemCategory itemCategory) {
        if (itemCategory.getId() == null || itemCategory.getId() == 0) {
            itemCategory.setId(itemCategoryRepository.findMaxId() + 1);
            if (itemCategory.getCategoryStatus() == null) {
                itemCategory.setCategoryStatus(true);
            }
        }
        return itemCategoryRepository.save(itemCategory);
    }

    @Override
    public void delete(int id) {
        itemCategoryRepository.deleteById(id);
    }
}
