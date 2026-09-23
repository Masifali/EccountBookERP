package com.mst.services;

import com.mst.repositories.PurchaseInvoiceAgainstGrnDirectRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.*;

@Service
public class PurchaseInvoiceAgainstGrnDirectService {

    @Autowired
    private PurchaseInvoiceAgainstGrnDirectRepository repository;
    @Autowired private com.mst.repositories.PurchaseInvoiceRecordRepository records;

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
        return records.history(138,fromDate,toDate,supplierId,fromDocNo==null?null:fromDocNo.intValue(),toDocNo==null?null:toDocNo.intValue(),"docdate");
    }

    public Map<String, Object> getById(int id) {
        Map<String,Object> header=records.load(id,138);
        Map<String,Object> result=new LinkedHashMap<>(header);
        result.put("success",true);result.put("header",header);
        return result;
    }

    public Map<String, Object> deleteRecord(int id, int orgId, int compId) {
        records.delete(id,138);
        return Map.of("success",true,"message","Purchase Invoice deleted successfully");
    }
}
