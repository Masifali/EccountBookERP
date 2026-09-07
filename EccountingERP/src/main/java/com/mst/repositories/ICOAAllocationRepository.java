package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;

import com.mst.models.COAAllocation;

public interface ICOAAllocationRepository extends JpaRepository<COAAllocation, Integer> {

	List<COAAllocation> findByChartofAccountId(Integer chartofAccountId);

	COAAllocation findByChartofAccountIdAndCompanyId(Integer chartofAccountId, Integer companyId);

	List<COAAllocation> findByCompanyId(Integer companyId);

	void deleteByCompanyId(Integer companyId);
}
