package com.mst.services;

import com.mst.security.CurrentUserContext;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.mst.models.Warehouse;
import com.mst.repositories.IWarehouseRepository;
import com.mst.serviceInterface.IWarehouseService;

@Service("warehouseService")
public class WarehouseService implements IWarehouseService {

	@Autowired
	private IWarehouseRepository warehouseRepository;
	@Autowired
	private CurrentUserContext currentUserContext;

	@Override
	public List<Warehouse> getAll() {
		return warehouseRepository.findAllByOrderByWareHouseName();
	}

	@Override
	public Warehouse getById(int id) {
		return warehouseRepository.findById(id).orElse(null);
	}

	@Override
	public Warehouse addOrUpdate(Warehouse warehouse) {
		LocalDateTime now = LocalDateTime.now();
		if (warehouse.getId() == null || warehouse.getId() == 0) {
			warehouse.setId(null);
			warehouse.setEntryDate(now);
			warehouse.setEntryUser(currentUserContext.currentUserId());
			warehouse.setOrganizationId(currentUserContext.currentOrganizationId());
			warehouse.setCompanyId(currentUserContext.currentCompanyId());
		} else {
			warehouse.setModifyDate(now);
			warehouse.setModifyUser(currentUserContext.currentUserId());
		}
		return warehouseRepository.save(warehouse);
	}

	@Override
	public void delete(int id) {
		warehouseRepository.deleteById(id);
	}
}
