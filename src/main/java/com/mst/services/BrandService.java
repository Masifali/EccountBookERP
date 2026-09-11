package com.mst.services;

import com.mst.security.CurrentUserContext;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.mst.models.Brand;
import com.mst.repositories.IBrandRepository;
import com.mst.serviceInterface.IBrandService;

@Service("brandService")
public class BrandService implements IBrandService {

	@Autowired
	private IBrandRepository brandRepository;
	@Autowired
	private CurrentUserContext currentUserContext;

	@Override
	public List<Brand> getAll() {
		return brandRepository.findAllByOrderByBrandName();
	}

	@Override
	public Brand getById(int id) {
		return brandRepository.findById(id).orElse(null);
	}

	@Override
	public Brand addOrUpdate(Brand brand) {
		LocalDateTime now = LocalDateTime.now();
		if (brand.getId() == null || brand.getId() == 0) {
			brand.setId(brandRepository.findMaxId() + 1);
			brand.setEntryDate(now);
			brand.setEntryUserId(currentUserContext.currentUserId());
			brand.setOrganizationId(currentUserContext.currentOrganizationId());
			brand.setCompanyId(currentUserContext.currentCompanyId());
		} else {
			brand.setModifyDate(now);
			brand.setModifyUserId(currentUserContext.currentUserId());
		}
		return brandRepository.save(brand);
	}

	@Override
	public void delete(int id) {
		brandRepository.deleteById(id);
	}
}
