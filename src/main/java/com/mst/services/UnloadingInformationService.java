package com.mst.services;

import com.mst.repositories.UnloadingInformationRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.*;

@Service
public class UnloadingInformationService {

    @Autowired
    private UnloadingInformationRepository repository;

    public List<Map<String, Object>> getItemsForUnloading(int orgId, int compId, Integer gpId, Integer labId) {
        return repository.getItemsForUnloading(orgId, compId, gpId, labId);
    }

    public List<Map<String, Object>> getContractors(int orgId, int compId, Integer branchId) {
        return repository.getContractors(orgId, compId, branchId);
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
            result.put("message", "Unloading Information record not found");
        }
        return result;
    }

    public List<Map<String, Object>> getHistory(int orgId, int compId, int yearId, String fromDate, String toDate, Double fromDocNo, Double toDocNo) {
        return repository.getHistory(orgId, compId, yearId, fromDate, toDate, fromDocNo, toDocNo);
    }

    public Map<String, Object> deleteRecord(int id, int userId) {
        Map<String, Object> result = new HashMap<>();
        try {
            repository.deleteRecord(id, userId);
            result.put("success", true);
            result.put("message", "Unloading Information deleted successfully");
        } catch (Exception e) {
            result.put("success", false);
            result.put("message", "Error deleting Unloading Information: " + e.getMessage());
        }
        return result;
    }
}
