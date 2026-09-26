package com.mst.services;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.*;

@Service
public class SaleReturnGrnService {

    @Autowired private com.mst.repositories.GrnNumberingRepository grnNumbering;

    @Autowired private com.mst.repositories.PurchaseGrnRecordRepository records;
    @Autowired private PurchaseGrnPersistenceService persistence;

    @Autowired private com.mst.repositories.PurchaseGrnLookupRepository lookups;

    public Map<String,Object> getDropdowns(int org,int company) { return lookups.all(143); }

    public int generateNextDocNo(int orgId, int compId, int branchId, int yearId) {
        return grnNumbering.next(orgId,compId,branchId,yearId,143);
    }

    public List<Map<String, Object>> getHistory(int orgId, int compId, int branchId, int yearId,
                                                String fromDate, String toDate, Integer supplierId,
                                                Integer fromDocNo, Integer toDocNo, String dateType) {
        return records.history(143,fromDate,toDate,supplierId,fromDocNo,toDocNo,dateType);
    }

    public Map<String,Object> getById(int id) { return records.load(id,143); }

    @Transactional
    public Map<String,Object> saveSaleReturnGrn(Map<String,Object> payload) {
        try { return persistence.save(payload,143); }
        catch(Exception failure) {
            org.springframework.transaction.interceptor.TransactionAspectSupport.currentTransactionStatus().setRollbackOnly();
            return Map.of("success",false,"message","Error saving Sale Return GRN: "+failure.getMessage());
        }
    }

    @Transactional
    public boolean deleteSaleReturnGrn(int id) {
        records.delete(id,143);
        return true;
    }
}
