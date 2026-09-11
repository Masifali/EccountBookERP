package com.mst.serviceInterface;

import java.util.List;
import com.mst.models.Item;

public interface IItemService {
    List<Item> getAll();
    Item getById(int id);
    default Item getById(Integer id) { return id == null ? null : getById(id.intValue()); }
    default Item getItemDefById(long id) { return getById((int) id); }
    Item addOrUpdate(Item item);
    void delete(int id);
}
