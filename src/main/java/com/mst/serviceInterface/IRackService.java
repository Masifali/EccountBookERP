package com.mst.serviceInterface;

import java.util.List;
import com.mst.models.Rack;

public interface IRackService {
    List<Rack> getAll();
    Rack getById(int id);
    default Rack getById(Integer id) { return id == null ? null : getById(id.intValue()); }
    Rack addOrUpdate(Rack rack);
    void delete(int id);
}
