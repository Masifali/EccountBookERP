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

    public void requireView(){records.requireRight(56,"View");}

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
        // grdHistory Edit / double-click -> ReadById(id): GetByID has no branch filter, so an invoice of another branch listed in the
        // history (the Branch Name combo's allocation list) opens too; it is read-only here (Update would move it to this branch, :3590).
        var historyBranches=new HashSet<Integer>();for(var b:lookups.historyBranches())historyBranches.add(PurchaseInvoiceFinancialRules.i(PurchaseInvoiceFinancialRules.copy(b),"BranchId"));
        var data=records.loadViewable(id,56,historyBranches::contains);data.put("rateUoms",lookups.rateUoms((List<Map<String,Object>>)data.get("details")));
        // ReadById:4131 VoucherHeadIdGet -> CommonServices.GetVoucherHeadId (DAL 0243:244) for the 103/104 prints.
        var voucher=jdbcTemplate.queryForList("EXEC dbo.Sp_Vouchers_GetMethods @Activity='GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId',@OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@DocumentTypeSrNo=?",context.currentOrganizationId(),context.currentCompanyId(),56,id);
        data.put("VoucherHeadId",voucher.isEmpty()?0:PurchaseInvoiceFinancialRules.i(PurchaseInvoiceFinancialRules.copy(voucher.get(0)),"Id"));
        return data;
    }

    /** GetAll():4932-5069 - FormHistory over the branches picked in the history Branch combo (validated against the allocation list). */
    public List<Map<String,Object>> history(String branchIds,String fromDate,String toDate,Integer supplierId,Integer fromDocNo,Integer toDocNo,String dateType){
        records.requireRight(56,"View");
        String branches=allowedBranches(branchIds);
        boolean all=records.hasRight(56,"CanView AllRecord");
        String mode=Objects.toString(dateType,"").toLowerCase(Locale.ROOT);
        String prefix="entrydate".equals(mode)?"Entry":"modifydate".equals(mode)?"Modify":"approveddate".equals(mode)?"Approved":"";
        return jdbcTemplate.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@FinancialYearId=?,@CanViewAllRecord=?,@EntryUser=?,@"+prefix+"FromDate=?,@"+prefix+"ToDate=?,@FromDocNo=?,@ToDocNo=?,@SupplierCustomerId=?,@BranchesIds=?,@Activity='FormHistory' WITH RECOMPILE",
                context.currentOrganizationId(),context.currentCompanyId(),56,context.currentFinancialYearId(),all,
                new org.springframework.jdbc.core.SqlParameterValue(java.sql.Types.INTEGER,all?null:context.currentUserId()),
                date(fromDate),date(toDate),positive(fromDocNo),positive(toDocNo),positive(supplierId),branches);
    }

    /** grdHistory_SelectionChanged:5224 -> DetailGridBind:5246 (GetByID detail list with JobLot and PackingType). */
    public List<Map<String,Object>> historyDetail(int id,String branchIds){
        records.requireRight(56,"View");
        var allowed=new HashSet<>(Arrays.asList(allowedBranches(branchIds).split(",")));
        var rows=jdbcTemplate.queryForList("SELECT BranchesId,EntryUser FROM dbo.InvPurchaseInvoice WHERE Id=? AND DocumentTypeId=56 AND OrganizationId=? AND CompanyId=?",id,context.currentOrganizationId(),context.currentCompanyId());
        if(rows.isEmpty()||!allowed.contains(String.valueOf(PurchaseInvoiceFinancialRules.i(PurchaseInvoiceFinancialRules.copy(rows.get(0)),"BranchesId"))))throw new IllegalArgumentException("Invoice not found in the selected branches");
        if(PurchaseInvoiceFinancialRules.i(PurchaseInvoiceFinancialRules.copy(rows.get(0)),"EntryUser")!=context.currentUserId()&&!records.hasRight(56,"CanView AllRecord"))throw new IllegalArgumentException("Invoice not found in your accessible records");
        return jdbcTemplate.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @Id=?,@Activity='PurchaseDetailReadByInvPurchaseInvoiceId' WITH RECOMPILE",id);
    }

    /** cmbBranchName_Leave:4908 -> HistoryComboBind(HistoryComboDbCall()). */
    public List<Map<String,Object>> historySuppliers(String branchIds){records.requireRight(56,"View");return lookups.historySuppliers(allowedBranches(branchIds));}

    /** HistoryComboDbCall/GetAll: "Select branch first" when the combo is empty; only allocated branches are accepted. */
    private String allowedBranches(String input){
        if(input==null||input.isBlank())throw new IllegalArgumentException("Select branch first");
        var allowed=new HashSet<Integer>();for(var b:lookups.historyBranches())allowed.add(PurchaseInvoiceFinancialRules.i(PurchaseInvoiceFinancialRules.copy(b),"BranchId"));
        var picked=new ArrayList<String>();
        for(String token:input.split(",")){if(token.isBlank())continue;int b;try{b=Integer.parseInt(token.trim());}catch(NumberFormatException bad){throw new IllegalArgumentException("Select branch first");}if(!allowed.contains(b))throw new IllegalArgumentException("Branch is not allocated to this user");picked.add(String.valueOf(b));}
        if(picked.isEmpty())throw new IllegalArgumentException("Select branch first");
        return String.join(",",picked);
    }
    private static org.springframework.jdbc.core.SqlParameterValue positive(Integer v){return new org.springframework.jdbc.core.SqlParameterValue(java.sql.Types.INTEGER,v!=null&&v>0?v:null);}
    private static org.springframework.jdbc.core.SqlParameterValue date(String v){try{return new org.springframework.jdbc.core.SqlParameterValue(java.sql.Types.DATE,v==null||v.isBlank()?null:java.sql.Date.valueOf(v.trim()));}catch(IllegalArgumentException bad){throw new IllegalArgumentException("Use a valid invoice filter date");}}

    @Autowired private PurchaseInvoicePersistenceService persistence;

    @Autowired private PurchaseInvoiceSaveRules saveRules;

    /** btnSave_Click/btnUpdate_Click -> Insert(): form checks first, then the original DAL chain; desktop success text (:3946-3953). */
    public Map<String, Object> savePurchaseInvoice(Map<String, Object> payload) {
        boolean update=PurchaseInvoiceFinancialRules.i(PurchaseInvoiceFinancialRules.copy(payload),"Id")>0;
        var result=new LinkedHashMap<String,Object>(persistence.save(saveRules.prepare(payload),56));
        result.put("message",(update?"Record Update Successfully [":"Record Saved Successfully [")+result.get("docNo")+"] ");
        return result;
    }

    @Transactional
    public boolean deletePurchaseInvoice(int id) {
        saveRules.requireNotApproved(id);
        records.delete(id,56);
        return true;
    }
}
