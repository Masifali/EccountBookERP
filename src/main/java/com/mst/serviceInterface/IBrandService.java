package com.mst.serviceInterface;

import java.util.List;
import com.mst.models.Brand;

public interface IBrandService {
    List<Brand> getAll();
    Brand getById(int id);
    default Brand getById(Integer id) { return id == null ? null : getById(id.intValue()); }
    Brand addOrUpdate(Brand brand);
    void delete(int id);
}
