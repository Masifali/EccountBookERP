package com.mst.serviceInterface;

import java.util.List;
import com.mst.models.ItemCategory;

public interface IItemCategoryService {
    List<ItemCategory> getAllItemCategories();
    ItemCategory getItemCategoryById(int itemCategoryId);
    ItemCategory save(ItemCategory itemCategory);
    void delete(int id);
}
