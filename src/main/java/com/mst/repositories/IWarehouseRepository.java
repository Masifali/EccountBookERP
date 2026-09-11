package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;

import com.mst.models.Warehouse;

public interface IWarehouseRepository extends JpaRepository<Warehouse, Integer> {

	List<Warehouse> findAllByOrderByWareHouseName();
}
