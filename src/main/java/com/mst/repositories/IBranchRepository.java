package com.mst.repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.mst.models.Branch;

import java.util.List;

@Repository
public interface IBranchRepository extends JpaRepository<Branch, Integer> {
    List<Branch> findByCompanyId(Integer companyId);
    List<Branch> findByIsActiveTrue();
}
