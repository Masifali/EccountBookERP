package com.mst.services;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.mst.models.Branch;
import com.mst.repositories.IBranchRepository;

import java.util.List;
import java.util.Optional;

@Service
public class BranchService {

    @Autowired
    private IBranchRepository branchRepository;

    public List<Branch> getAllBranches() {
        return branchRepository.findAll();
    }

    public List<Branch> getActiveBranches() {
        return branchRepository.findByIsActiveTrue();
    }

    public List<Branch> getBranchesByCompany(Integer companyId) {
        return branchRepository.findByCompanyId(companyId);
    }

    public Optional<Branch> getBranchById(Integer id) {
        return branchRepository.findById(id);
    }

    public Branch saveBranch(Branch branch) {
        return branchRepository.save(branch);
    }

    public void deleteBranch(Integer id) {
        branchRepository.deleteById(id);
    }
}
