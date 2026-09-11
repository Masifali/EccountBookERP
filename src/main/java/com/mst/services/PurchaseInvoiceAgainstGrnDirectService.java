package com.mst.services;

import com.mst.repositories.PurchaseInvoiceAgainstGrnDirectRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.*;

@Service
public class PurchaseInvoiceAgainstGrnDirectService {

    @Autowired
    private PurchaseInvoiceAgainstGrnDirectRepository repository;

    public Map<String, Object> getDropdowns(int orgId, int compId) {
        Map<String, Object> map = new HashMap<>();
        map.put("brokerageAccounts", repository.getBrokerageAccounts());
        map.put("freightAccounts", repository.getFreightAccounts());
        map.put("creditAccountsForEmptyBags", repository.getCreditAccountsForEmptyBags());
        return map;
    }

    public List<Map<String, Object>> loadGrnDirectForInvoice(String gdnIds) {
        return repository.loadGrnDirectForInvoice(gdnIds);
    }

    public List<Map<String, Object>> getEmptyBagsFromGrn(String gdnIds, Integer orderId) {
        return repository.getEmptyBagsFromGrn(gdnIds, orderId);
    }

    public List<Map<String, Object>> getHistory(int orgId, int compId, Integer docTypeId, String fromDate, String toDate,
                                                Double fromDocNo, Double toDocNo, Integer supplierId) {
        return repository.getHistory(orgId, compId, docTypeId, fromDate, toDate, fromDocNo, toDocNo, supplierId);
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
            result.put("message", "Record not found");
        }
        return result;
    }

    public Map<String, Object> deleteRecord(int id, int orgId, int compId) {
        Map<String, Object> result = new HashMap<>();
        try {
            repository.deleteRecord(id, orgId, compId);
            result.put("success", true);
            result.put("message", "Purchase Invoice deleted successfully");
        } catch (Exception e) {
            result.put("success", false);
            result.put("message", "Error deleting Purchase Invoice: " + e.getMessage());
        }
        return result;
    }
}
