package com.mst.services;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.*;

@Service
public class MarketGrnService {

    @Autowired private com.mst.repositories.GrnNumberingRepository grnNumbering;

    @Autowired private com.mst.repositories.PurchaseGrnRecordRepository records;
    @Autowired private PurchaseGrnPersistenceService persistence;

    @Autowired private com.mst.repositories.PurchaseGrnLookupRepository lookups;

    @Autowired private org.springframework.jdbc.core.JdbcTemplate jdbc;

    public Map<String,Object> getDropdowns(int org,int company) { return lookups.all(46); }

    public int generateNextDocNo(int orgId, int compId, int branchId, int yearId) {
        return grnNumbering.next(orgId,compId,branchId,yearId,46);
    }

    public List<Map<String, Object>> getHistory(int orgId, int compId, int branchId, int yearId,
                                                String fromDate, String toDate, Integer supplierId,
                                                Integer fromDocNo, Integer toDocNo, String dateType) {
        return getHistory(orgId,compId,branchId,yearId,fromDate,toDate,supplierId,fromDocNo,toDocNo,dateType,null);
    }

    /** HistoryGridFill :5804 — actionId 1 Reffered / 2 Not Reffered / null All (RadAll). */
    public List<Map<String, Object>> getHistory(int orgId, int compId, int branchId, int yearId,
                                                String fromDate, String toDate, Integer supplierId,
                                                Integer fromDocNo, Integer toDocNo, String dateType, Integer actionId) {
        return records.history(46,fromDate,toDate,supplierId,fromDocNo,toDocNo,dateType,actionId);
    }

    public Map<String,Object> getById(int id) { return records.load(id,46); }

    @Transactional
    public Map<String,Object> saveMarketGrn(Map<String,Object> payload) {
        try { return persistence.save(payload,46); }
        catch(Exception failure) {
            org.springframework.transaction.interceptor.TransactionAspectSupport.currentTransactionStatus().setRollbackOnly();
            // A desktop refusal is shown in the desktop's words, without a prefix.
            if(failure instanceof IllegalArgumentException)return Map.of("success",false,"message",Objects.toString(failure.getMessage(),"Request failed."));
            return Map.of("success",false,"message","Error saving Market GRN: "+failure.getMessage());
        }
    }

    @Transactional
    public boolean deleteMarketGrn(int id) {
        records.require(id,46);
        // btnDelete_Click :4241 — RefferedInInvoice (ReadById :4029, InvoiceId > 0) stops the delete first.
        var rows=jdbc.queryForList("EXEC dbo.Sp_InvGrn_GetAllMethod @Id=?,@Activity='ReadById'",id);
        if(!rows.isEmpty()&&com.mst.repositories.PurchaseGrnWriteRepository.number(rows.get(0).get("InvoiceId"))>0)
            throw new IllegalArgumentException("This Document is referred in invoice. So you can't delete this record.");
        records.delete(id,46);
        return true;
    }
}
