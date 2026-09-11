package com.mst.services;

import com.mst.security.CurrentUserContext;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.mst.models.ProductType;
import com.mst.repositories.IProductTypeRepository;
import com.mst.serviceInterface.IProductTypeService;

@Service("productTypeService")
public class ProductTypeService implements IProductTypeService {

	@Autowired
	private IProductTypeRepository productTypeRepository;
	@Autowired
	private CurrentUserContext currentUserContext;

	@Override
	public List<ProductType> getAll() {
		return productTypeRepository.findAllByOrderByProductTypeDescription();
	}

	@Override
	public ProductType getById(int id) {
		return productTypeRepository.findById(id).orElse(null);
	}

	@Override
	public ProductType addOrUpdate(ProductType productType) {
		LocalDateTime now = LocalDateTime.now();
		if (productType.getId() == null || productType.getId() == 0) {
			productType.setId(productTypeRepository.findMaxId() + 1);
			productType.setEntryDate(now);
			productType.setEntryUserId(currentUserContext.currentUserId());
			productType.setOrganizationId(currentUserContext.currentOrganizationId());
			productType.setCompanyId(currentUserContext.currentCompanyId());
		} else {
			productType.setModifyDate(now);
			productType.setModifyUserId(currentUserContext.currentUserId());
		}
		return productTypeRepository.save(productType);
	}

	@Override
	public void delete(int id) {
		productTypeRepository.deleteById(id);
	}
}
