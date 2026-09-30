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
        var result=records.load(id,57);
        // VoucherHeadIdGet :1240 -> CommonServices.VoucherHeadIdGet(RecId,57), used by btnPrint (118)
        var v=jdbcTemplate.queryForList("EXEC dbo.Sp_Vouchers_GetMethods @Activity='GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId',@OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@DocumentTypeSrNo=?",currentUser.currentOrganizationId(),currentUser.currentCompanyId(),57,id);
        result.put("voucherHeadId",v.isEmpty()?0:PurchaseInvoiceFinancialRules.i(PurchaseInvoiceFinancialRules.copy(v.get(0)),"Id"));
        return result;
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
        PurchaseDirectInvoiceDesktopRules.journalSupplier(collections.get("journal"),gl);
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

    /**
     * Insert (:3075) / btnUpdate_Click (:3530): the desktop refusals in its order and words (PurchaseDirectInvoiceDesktopRules),
     * then the shared 56/57 save. Messages as :3472/:3480.
     */
    @SuppressWarnings("unchecked")
    @Transactional
    public Map<String, Object> saveDirectInvoice(Map<String, Object> payload) {
        int id=PurchaseInvoiceFinancialRules.i(payload,"Id");
        var stored=id>0?records.require(id,57):null;
        if(stored!=null&&PurchaseDirectInvoiceDesktopRules.approved(stored))throw new IllegalArgumentException("Record Not Update because Record has approved");
        var h=PurchaseInvoiceFinancialRules.copy(stored);h.putAll(payload);
        var lists=new HashMap<String,List<Map<String,Object>>>();
        for(String key:List.of("details","freight","journal","expenses","emptyBags"))
            lists.put(key,payload.get(key) instanceof List<?> l?((List<Map<String,Object>>)l).stream().map(PurchaseInvoiceFinancialRules::copy).toList():id>0?invoiceWrites.collection(id,key):List.of());
        boolean freightToExpenses=Boolean.parseBoolean(directLookups.configuration("DebitAmountChargetoExpenseAcFreightGridPurchase"));
        PurchaseDirectInvoiceDesktopRules.validate(h,lists.get("details"),lists.get("freight"),lists.get("journal"),lists.get("expenses"),lists.get("emptyBags"),freightToExpenses);
        PurchaseDirectInvoiceDesktopRules.journalSupplier(lists.get("journal"),supplierGl(PurchaseInvoiceFinancialRules.i(h,"SupplierCustomerId")));
        // :3197 SupplierInvoiceDate = DateTime.Now on save and on update
        payload.put("SupplierInvoiceDate",new java.sql.Timestamp(System.currentTimeMillis()));
        var result=new LinkedHashMap<String,Object>(persistence.save(payload,57));
        result.put("message",id>0?"Record Update Successfully":"Record Saved Successfully");
        return result;
    }

    private int supplierGl(int supplier){
        var party=jdbcTemplate.queryForList("SELECT GlAccountId FROM dbo.SupplierCustomer WHERE Id=? AND OrganizationId=? AND CompanyId=?",supplier,currentUser.currentOrganizationId(),currentUser.currentCompanyId());
        return party.isEmpty()?0:PurchaseInvoiceFinancialRules.i(PurchaseInvoiceFinancialRules.copy(party.get(0)),"GlAccountId");
    }

    @Autowired private com.mst.security.CurrentUserContext currentUser;

    /** btnDelete_Click :3699 - approved records are refused; RemoveByID -> Sp_InvoicesVouchersandStocksDelete. */
    @Transactional
    public boolean deleteDirectInvoice(int id) {
        if(PurchaseDirectInvoiceDesktopRules.approved(records.require(id,57)))throw new IllegalArgumentException("Record Not Delete because Record has approved");
        records.delete(id,57);
        return true;
    }
}
