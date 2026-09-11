package com.mst.serviceInterface;

import java.util.List;
import com.mst.models.ProductType;

public interface IProductTypeService {
    List<ProductType> getAll();
    ProductType getById(int id);
    default ProductType getById(Integer id) { return id == null ? null : getById(id.intValue()); }
    ProductType addOrUpdate(ProductType productType);
    void delete(int id);
}
