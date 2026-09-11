package com.mst.services;

import com.mst.repositories.GrnDirectAgainstOrderRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.*;

@Service
public class GrnDirectAgainstOrderService {

    @Autowired
    private GrnDirectAgainstOrderRepository repository;

    public Map<String, Object> generateNextNumbers(int orgId, int compId, int yearId, int docTypeId) {
        Map<String, Object> map = new HashMap<>();
        map.put("docNo", repository.generateDocNo(orgId, compId, yearId, docTypeId));
        return map;
    }

    public List<Map<String, Object>> getTransporters(int orgId, int compId) {
        return repository.getTransporters(orgId, compId);
    }

    public List<Map<String, Object>> getOrdersBySupplier(int orgId, int compId, int supplierId) {
        return repository.getOrdersBySupplier(orgId, compId, supplierId);
    }

    public List<Map<String, Object>> getHistory(int orgId, int compId, int branchId, int yearId,
                                                int docTypeId, String fromDate, String toDate,
                                                Double fromDocNo, Double toDocNo, Integer supplierId) {
        return repository.getHistory(orgId, compId, branchId, yearId, docTypeId, fromDate, toDate, fromDocNo, toDocNo, supplierId);
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
            result.put("message", "GRN Direct record not found");
        }
        return result;
    }

    public Map<String, Object> deleteRecord(int id, int orgId, int compId) {
        Map<String, Object> result = new HashMap<>();
        try {
            repository.deleteRecord(id, orgId, compId);
            result.put("success", true);
            result.put("message", "GRN Direct deleted successfully");
        } catch (Exception e) {
            result.put("success", false);
            result.put("message", "Error deleting GRN Direct: " + e.getMessage());
        }
        return result;
    }
}
