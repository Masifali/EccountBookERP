package com.mst.services;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.*;

@Service
public class PurchaseInvoiceFullService {

    @Autowired private com.mst.repositories.PurchaseInvoiceRecordRepository records;

    @Autowired
    private JdbcTemplate jdbcTemplate;

    @Autowired
    private com.mst.repositories.PurchaseInvoiceNumberingRepository numbering;

    @Autowired private com.mst.repositories.PurchaseInvoiceLookupRepository lookups;
    @Autowired private com.mst.repositories.PurchaseInvoiceWriteRepository writes;
    @Autowired private com.mst.security.CurrentUserContext context;

    public Map<String, Object> getDropdowns(int orgId, int compId) { return lookups.all(); }

    public int generateNextBranchNo(){return numbering.nextBranch(context.currentOrganizationId(),context.currentCompanyId(),context.currentFinancialYearId(),context.currentBranchId(),56);}

    @SuppressWarnings("unchecked")
    public Map<String,Object> calculateLine(Map<String,Object> body){
        records.requireRight(56,"View");
        return PurchaseInvoiceCalculations.line((Map<String,Object>)body.get("line"),PurchaseInvoiceFinancialRules.i(PurchaseInvoiceFinancialRules.copy(body),"InvoiceTypeId"));
    }

    @SuppressWarnings("unchecked")
    public Map<String,Object> paymentRow(Map<String,Object> body){
        records.requireRight(56,"View");
        return PurchaseInvoicePaymentRules.edit((List<Map<String,Object>>)body.get("rows"),((Number)body.get("index")).intValue(),Objects.toString(body.get("changed")),java.time.LocalDate.parse(Objects.toString(body.get("docDate"))));
    }

    @SuppressWarnings("unchecked")
    public Map<String,Object> supplementRow(Map<String,Object> body){
        records.requireRight(56,"View");
        return PurchaseInvoiceCalculations.supplement(Objects.toString(body.get("grid")),(Map<String,Object>)body.get("row"),Objects.toString(body.get("changed")),PurchaseInvoiceFinancialRules.n(PurchaseInvoiceFinancialRules.copy(body),"itemTotal"),lookups.amountDigits(),lookups.enabled("DebitAmountChargetoExpenseAcFreightGridPurchase"));
    }

    @SuppressWarnings("unchecked")
    public Map<String,Object> calculateBill(Map<String,Object> body){
        records.requireRight(56,"View");var payload=PurchaseInvoiceFinancialRules.copy(body);int id=PurchaseInvoiceFinancialRules.i(payload,"Id");
        var h=PurchaseInvoiceFinancialRules.copy(id>0?records.require(id,56):null);h.putAll(payload);
        var rows=((List<Map<String,Object>>)payload.getOrDefault("details",List.of())).stream().map(PurchaseInvoiceFinancialRules::copy).toList();
        var collections=new HashMap<String,List<Map<String,Object>>>();
        for(String key:List.of("expenses","freight","journal","emptyBags"))collections.put(key,payload.containsKey(key)?((List<Map<String,Object>>)payload.get(key)).stream().map(PurchaseInvoiceFinancialRules::copy).toList():id>0?writes.collection(id,key):List.of());
        var party=jdbcTemplate.queryForList("SELECT GlAccountId FROM dbo.SupplierCustomer WHERE Id=? AND OrganizationId=? AND CompanyId=?",PurchaseInvoiceFinancialRules.i(h,"SupplierCustomerId"),context.currentOrganizationId(),context.currentCompanyId());
        int gl=party.isEmpty()?0:PurchaseInvoiceFinancialRules.i(PurchaseInvoiceFinancialRules.copy(party.get(0)),"GlAccountId");
        PurchaseInvoiceCalculations.bill(h,rows,collections.get("expenses"),collections.get("freight"),collections.get("journal"),collections.get("emptyBags"),gl,new PurchaseInvoiceCalculations.Configuration(lookups.amountDigits(),lookups.enabled("DebitAmountChargetoExpenseAcFreightGridPurchase"),lookups.enabled("WagesAmountCalculateOnQty"),lookups.enabled("ContractWagesChargetoProduct"),lookups.subsidiary()));
        var terms=payload.containsKey("paymentTerms")?(List<Map<String,Object>>)payload.get("paymentTerms"):id>0?writes.collection(id,"paymentTerms"):List.<Map<String,Object>>of();
        return Map.of("billAmount",h.get("BillAmount"),"commAmount",h.get("CommAmount"),"brokeryAmount",h.get("BrokeryAmount"),"details",rows,"paymentTerms",PurchaseInvoicePaymentRules.calculate(h,rows,terms,!Boolean.FALSE.equals(body.get("paymentByPercent")),false));
    }

    public int generateNextDocNo(int orgId, int compId, int branchId, int yearId, int docTypeId) {
        if(docTypeId!=0&&docTypeId!=56)throw new IllegalArgumentException("Use the original form for this invoice type");
        return numbering.next(orgId, compId, yearId, 56);
    }

    public List<Map<String, Object>> getHistory(int orgId, int compId, int branchId, int yearId, int docTypeId,
                                                String fromDate, String toDate, Integer supplierId,
                                                Integer fromDocNo, Integer toDocNo, String dateType) {
        if(docTypeId!=0&&docTypeId!=56)throw new IllegalArgumentException("Use the original form for this invoice type");
        return records.history(56,fromDate,toDate,supplierId,fromDocNo,toDocNo,dateType);
    }

    @SuppressWarnings("unchecked")
    public Map<String, Object> getById(int id) {
        var data=records.load(id,56);data.put("rateUoms",lookups.rateUoms((List<Map<String,Object>>)data.get("details")));return data;
    }

    @Autowired private PurchaseInvoicePersistenceService persistence;

    public Map<String, Object> savePurchaseInvoice(Map<String, Object> payload) {
        return persistence.save(payload,56);
    }

    @Transactional
    public boolean deletePurchaseInvoice(int id) {
        records.delete(id,56);
        return true;
    }
}
