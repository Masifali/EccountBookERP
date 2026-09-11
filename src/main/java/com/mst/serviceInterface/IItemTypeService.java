package com.mst.serviceInterface;

import java.util.List;
import java.util.Map;
import com.mst.models.ItemType;

public interface IItemTypeService {
    List<ItemType> getAll();
    ItemType getById(int id);
    ItemType addOrUpdate(ItemType itemType);
    void delete(int id);

    List<Map<String, Object>> getParentCategoriesLookup();
    List<Map<String, Object>> getItemTypeLookups();
    List<Map<String, Object>> getItemTypeHistory();
    String generateItemTypeCode();
    Map<String, Object> getByIdSp(int id);
    Map<String, Object> saveSp(ItemType itemType);
}

