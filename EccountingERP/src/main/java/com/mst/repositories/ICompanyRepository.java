package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;

import com.mst.models.Company;

public interface ICompanyRepository extends JpaRepository<Company, Integer> {

	List<Company> findAllByOrderByCompName();
}
