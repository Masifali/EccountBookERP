package com.mst.services;

import com.mst.repositories.SupplierDispatchRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.*;

@Service
public class SupplierDispatchService {

    @Autowired
    private SupplierDispatchRepository repository;

    public Map<String, Object> generateNextNumbers(int orgId, int compId, int branchId, int yearId, int docTypeId) {
        Map<String, Object> map = new HashMap<>();
        map.put("docNo", repository.generateDocNo(orgId, compId, yearId, docTypeId));
        map.put("branchDocNo", repository.generateBranchDocNo(orgId, compId, branchId, yearId, docTypeId));
        return map;
    }

    public List<Map<String, Object>> getCities(int orgId, int compId) {
        return repository.getCities(orgId, compId);
    }

    public List<Map<String, Object>> getHistory(int orgId, int compId, int docTypeId, int yearId,
                                                String fromDate, String toDate, Double fromDocNo, Double toDocNo) {
        return repository.getHistory(orgId, compId, docTypeId, yearId, fromDate, toDate, fromDocNo, toDocNo);
    }

    public Map<String, Object> getById(int id) {
        Map<String, Object> result = new HashMap<>();
        Map<String, Object> header = repository.getById(id);
        if (header != null) {
            result.put("success", true);
            result.put("header", header);
            result.put("details", repository.getDetailsByHeaderId(id));
        } else {
            result.put("success", false);
            result.put("message", "Supplier Dispatch record not found");
        }
        return result;
    }

    public Map<String, Object> deleteRecord(int id, int userId) {
        Map<String, Object> result = new HashMap<>();
        try {
            repository.deleteRecord(id, userId);
            result.put("success", true);
            result.put("message", "Supplier Dispatch deleted successfully");
        } catch (Exception e) {
            result.put("success", false);
            result.put("message", "Error deleting Supplier Dispatch: " + e.getMessage());
        }
        return result;
    }
}
