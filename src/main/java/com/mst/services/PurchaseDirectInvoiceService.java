package com.mst.services;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.*;

@Service
public class PurchaseDirectInvoiceService {

    @Autowired private com.mst.repositories.PurchaseInvoiceRecordRepository records;

    @Autowired
    private JdbcTemplate jdbcTemplate;

    @Autowired
    private com.mst.repositories.PurchaseInvoiceNumberingRepository numbering;

    public Map<String, Object> getDropdowns(int orgId, int compId) { return desktopDropdowns(); }

    public int generateNextDocNo(int orgId, int compId, int branchId, int yearId) {
        return numbering.next(orgId, compId, yearId, 57);
    }
    public int generateNextBranchNo(int orgId,int compId,int branchId,int yearId){return numbering.nextBranch(orgId,compId,yearId,branchId,57);}

    public List<Map<String, Object>> getHistory(int orgId, int compId, int branchId, int yearId,
                                                String fromDate, String toDate, Integer supplierId,
                                                Integer fromDocNo, Integer toDocNo, String dateType) {
        return records.history(57,fromDate,toDate,supplierId,fromDocNo,toDocNo,dateType);
    }

    public Map<String, Object> getById(int id) {
        return records.load(id,57);
    }

    @Autowired private PurchaseInvoicePersistenceService persistence;

    @Autowired private com.mst.repositories.PurchaseDirectInvoiceLookupRepository directLookups;
    @Autowired private com.mst.repositories.PurchaseInvoiceWriteRepository invoiceWrites;

    public Map<String,Object> desktopDropdowns(){return directLookups.all();}

    @SuppressWarnings("unchecked")
    public Map<String,Object> calculateBill(Map<String,Object> input){
        records.requireRight(57,"View");
        var payload=PurchaseInvoiceFinancialRules.copy(input);int id=PurchaseInvoiceFinancialRules.i(payload,"Id");
        var header=PurchaseInvoiceFinancialRules.copy(id>0?records.require(id,57):null);header.putAll(payload);
        List<Map<String,Object>> details=((List<Map<String,Object>>)payload.getOrDefault("details",List.of())).stream().map(PurchaseInvoiceFinancialRules::copy).toList();
        var collections=new HashMap<String,List<Map<String,Object>>>();
        for(String key:List.of("expenses","freight","journal","emptyBags"))collections.put(key,payload.containsKey(key)?((List<Map<String,Object>>)payload.get(key)).stream().map(PurchaseInvoiceFinancialRules::copy).toList():id>0?invoiceWrites.collection(id,key):List.of());
        int supplier=PurchaseInvoiceFinancialRules.i(header,"SupplierCustomerId");
        var party=jdbcTemplate.queryForList("SELECT GlAccountId FROM dbo.SupplierCustomer WHERE Id=? AND OrganizationId=? AND CompanyId=?",supplier,header.get("OrganizationId"),header.get("CompanyId"));
        // New documents obtain context in the controller; existing records are scoped above.
        int gl=party.isEmpty()?0:PurchaseInvoiceFinancialRules.i(PurchaseInvoiceFinancialRules.copy(party.get(0)),"GlAccountId");
        PurchaseDirectInvoiceCalculations.bill(header,details,collections.get("expenses"),collections.get("freight"),collections.get("journal"),collections.get("emptyBags"),gl,Boolean.parseBoolean(directLookups.configuration("DebitAmountChargetoExpenseAcFreightGridPurchase")),directLookups.amountDigits());
        return Map.of("billAmount",header.get("BillAmount"),"commAmount",header.get("CommAmount"),"brokeryAmount",header.get("BrokeryAmount"),"details",details);
    }

    @SuppressWarnings("unchecked")
    public Map<String,Object> calculateLine(Map<String,Object> input){
        records.requireRight(57,"View");
        var row=PurchaseInvoiceFinancialRules.copy((Map<String,Object>)input.get("line"));
        int item=PurchaseInvoiceFinancialRules.i(row,"ItemId");
        return PurchaseDirectInvoiceCalculations.line(row,Objects.toString(input.get("changed"),""),Boolean.TRUE.equals(input.get("newInvoice")),Boolean.TRUE.equals(input.get("bagsAgainstWeight")),directLookups.equivalent(PurchaseInvoiceFinancialRules.i(row,"ItemUOMId"),item),directLookups.equivalent(PurchaseInvoiceFinancialRules.i(row,"UomScheduleIdRate"),item),directLookups.amountDigits());
    }

    public Map<String, Object> saveDirectInvoice(Map<String, Object> payload) {
        return persistence.save(payload,57);
    }

    @Transactional
    public boolean deleteDirectInvoice(int id) {
        records.delete(id,57);
        return true;
    }
}
