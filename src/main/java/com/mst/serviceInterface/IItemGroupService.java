package com.mst.serviceInterface;

import java.util.List;
import com.mst.models.ItemGroup;

public interface IItemGroupService {
    List<ItemGroup> getAll();
    ItemGroup getById(int id);
    ItemGroup addOrUpdate(ItemGroup itemGroup);
    void delete(int id);
}
