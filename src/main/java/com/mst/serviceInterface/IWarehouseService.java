package com.mst.serviceInterface;

import java.util.List;
import com.mst.models.Warehouse;

public interface IWarehouseService {
    List<Warehouse> getAll();
    Warehouse getById(int id);
    default Warehouse getById(Integer id) { return id == null ? null : getById(id.intValue()); }
    Warehouse addOrUpdate(Warehouse warehouse);
    void delete(int id);
}
