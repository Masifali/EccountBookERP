package com.mst.services;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Types;
import java.util.*;

import static com.mst.services.PurchaseInvoiceFinancialRules.*;

/**
 * Purchase Invoice Direct (InvfrmPurchasedirectInvoice, screen 117, DocumentTypeId 57).
 * Save/Update/Delete go through the shared DAL 0434 port (PurchaseInvoicePersistenceService, type 57); the desktop's own
 * refusals and grid mapping run first (PurchaseDirectInvoiceDesktopRules). History, the Purchase Order loader
 * (btnPurchaseOrderLoader_Click :5246) and the detail-row delete check (DeleteDetailrow :2240) are 57-only here.
 */
@Service
public class PurchaseDirectInvoiceService {
    private static final int TYPE = 57;

    @Autowired private com.mst.repositories.PurchaseInvoiceRecordRepository records;
    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private com.mst.repositories.PurchaseInvoiceNumberingRepository numbering;
    @Autowired private PurchaseInvoicePersistenceService persistence;
    @Autowired private com.mst.repositories.PurchaseDirectInvoiceLookupRepository directLookups;
    @Autowired private com.mst.repositories.PurchaseInvoiceWriteRepository invoiceWrites;
    @Autowired private com.mst.security.CurrentUserContext currentUser;

    public Map<String, Object> getDropdowns(int orgId, int compId) { return desktopDropdowns(); }
    public Map<String,Object> desktopDropdowns(){return directLookups.all();}

    public int generateNextDocNo(int orgId, int compId, int branchId, int yearId) { return numbering.next(orgId, compId, yearId, TYPE); }
    public int generateNextBranchNo(int orgId,int compId,int branchId,int yearId){return numbering.nextBranch(orgId,compId,yearId,branchId,TYPE);}

    /** ReadById :3546 - GetByID plus VoucherHeadIdGet :1236 (CommonServices.VoucherHeadIdGet(RecId, 57)). */
    public Map<String, Object> getById(int id) {
        // ReadById has no branch filter: History Edit opens an invoice of any branch the History branch list offers
        // (shown read-only on the web when it is not of the current branch - "OtherBranch").
        var result=records.loadViewable(id,TYPE,historyBranchIds()::contains);
        var v=jdbcTemplate.queryForList("EXEC dbo.Sp_Vouchers_GetMethods @Activity='GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId',@OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@DocumentTypeSrNo=?",currentUser.currentOrganizationId(),currentUser.currentCompanyId(),TYPE,id);
        result.put("voucherHeadId",v.isEmpty()?0:i(copy(v.get(0)),"Id"));
        result.put("approved",PurchaseDirectInvoiceDesktopRules.approved(copy(result)));
        return result;
    }

    // ------------------------------------------------------------------------------------------------ History
    /**
     * GetAll :4676 - InvPurchaseInvoice.FormHistory (BLL 0581 :369): @EntryUser only without CanViewAllRecord, the date pair of
     * the ticked radio only when its check box is ticked, doc numbers / supplier only when non-zero, @BranchesIds = the
     * ticked History branches ("Select branch first" when none).
     */
    public List<Map<String,Object>> history(String branchIds,String fromDate,String toDate,Integer supplierId,Integer fromDocNo,Integer toDocNo,String dateType){
        records.requireRight(TYPE,"View");
        String branches=allowedBranches(branchIds);
        boolean all=records.hasRight(TYPE,"CanView AllRecord");
        String mode=Objects.toString(dateType,"").toLowerCase(Locale.ROOT);
        String prefix="entrydate".equals(mode)?"Entry":"modifydate".equals(mode)?"Modify":"approveddate".equals(mode)?"Approved":"";
        return jdbcTemplate.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@FinancialYearId=?,@CanViewAllRecord=?,@EntryUser=?,@"+prefix+"FromDate=?,@"+prefix+"ToDate=?,@FromDocNo=?,@ToDocNo=?,@SupplierCustomerId=?,@BranchesIds=?,@Activity='FormHistory' WITH RECOMPILE",
                currentUser.currentOrganizationId(),currentUser.currentCompanyId(),TYPE,currentUser.currentFinancialYearId(),all,
                new SqlParameterValue(Types.INTEGER,all?null:currentUser.currentUserId()),
                date(fromDate),date(toDate),positive(fromDocNo),positive(toDocNo),positive(supplierId),branches);
    }
    /** grdHistory_SelectionChanged :5183 -> GetDetailGrdByHeadId :5000 (GetByID detail list: DirectPurchaseDetailReadByInvPurchaseInvoiceId). */
    public List<Map<String,Object>> historyDetail(int id,String branchIds){
        records.requireRight(TYPE,"View");
        var allowed=new HashSet<>(Arrays.asList(allowedBranches(branchIds).split(",")));
        var rows=jdbcTemplate.queryForList("SELECT BranchesId,EntryUser FROM dbo.InvPurchaseInvoice WHERE Id=? AND DocumentTypeId=? AND OrganizationId=? AND CompanyId=?",id,TYPE,currentUser.currentOrganizationId(),currentUser.currentCompanyId());
        if(rows.isEmpty()||!allowed.contains(String.valueOf(i(copy(rows.get(0)),"BranchesId"))))throw new IllegalArgumentException("Invoice not found in the selected branches");
        if(i(copy(rows.get(0)),"EntryUser")!=currentUser.currentUserId()&&!records.hasRight(TYPE,"CanView AllRecord"))throw new IllegalArgumentException("Invoice not found in your accessible records");
        return jdbcTemplate.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @Id=?,@Activity='DirectPurchaseDetailReadByInvPurchaseInvoiceId' WITH RECOMPILE",id);
    }
    /** cmbBranchName_Leave :1248 -> HistoryComboBind(HistoryComboDbCall()). */
    public List<Map<String,Object>> historySuppliers(String branchIds){records.requireRight(TYPE,"View");return directLookups.historySuppliers(allowedBranches(branchIds));}
    /** btnRefreshHistory_Click :5220 - HistoryBranchdtFillDBCall (now with BranchImplemented read), HistoryComboBranchBind. */
    public Map<String,Object> historyRefresh(){
        records.requireRight(TYPE,"View");
        return Map.of("historyBranches",directLookups.historyBranches(true),"currentBranchId",currentUser.currentBranchId());
    }
    private Set<Integer> historyBranchIds(){var ids=new HashSet<Integer>();for(var b:directLookups.historyBranches(false))ids.add(i(copy(b),"BranchId"));return ids;}
    private String allowedBranches(String input){
        if(input==null||input.isBlank())throw new IllegalArgumentException("Select branch first");
        var allowed=new HashSet<Integer>();for(var b:directLookups.historyBranches(false))allowed.add(i(copy(b),"BranchId"));allowed.add(currentUser.currentBranchId());
        var picked=new ArrayList<String>();
        for(String token:input.split(",")){if(token.isBlank())continue;int b;try{b=Integer.parseInt(token.trim());}catch(NumberFormatException bad){throw new IllegalArgumentException("Select branch first");}if(!allowed.contains(b))throw new IllegalArgumentException("Branch is not allocated to this user");picked.add(String.valueOf(b));}
        if(picked.isEmpty())throw new IllegalArgumentException("Select branch first");
        return String.join(",",picked);
    }
    private static SqlParameterValue positive(Integer v){return new SqlParameterValue(Types.INTEGER,v!=null&&v>0?v:null);}
    private static SqlParameterValue date(String v){try{return new SqlParameterValue(Types.DATE,v==null||v.isBlank()?null:java.sql.Date.valueOf(v.trim()));}catch(IllegalArgumentException bad){throw new IllegalArgumentException("Use a valid invoice filter date");}}

    // ------------------------------------------------------------------------------------------------ Purchase Order loader
    /**
     * LoadPurchaseOrder (DocumentTypeId 41, :5264). The dialog's own source is not in the workspace; its list is BLL 0595
     * GetPurchaseOrderForPurchaseInvoice (Sp_PurchaseOrder_GetAllMethod) with org/company always and the optional filters only
     * when set. {@code excludeIds} is GridIds (:5255, the orders already in the detail grid) sent as @Ids ("not in").
     */
    public List<Map<String,Object>> pendingOrders(Integer supplierId,String fromDate,String toDate,Integer fromDocNo,Integer toDocNo,String excludeIds){
        records.requireRight(TYPE,"View");
        var sql=new StringBuilder("EXEC dbo.Sp_PurchaseOrder_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?");
        var args=new ArrayList<Object>(List.of(currentUser.currentOrganizationId(),currentUser.currentCompanyId(),41));
        if(supplierId!=null&&supplierId>0){sql.append(",@OrderSupCustId=?");args.add(supplierId);}
        if(fromDate!=null&&!fromDate.isBlank()){sql.append(",@DocDateFrom=?");args.add(date(fromDate));}
        if(toDate!=null&&!toDate.isBlank()){sql.append(",@DocDateTo=?");args.add(date(toDate));}
        if(fromDocNo!=null&&fromDocNo>0){sql.append(",@FromDocNo=?");args.add(fromDocNo);}
        if(toDocNo!=null&&toDocNo>0){sql.append(",@ToDocNo=?");args.add(toDocNo);}
        String ids=cleanIds(excludeIds);if(!ids.isEmpty()){sql.append(",@Ids=?");args.add(ids);}
        sql.append(",@Activity='GetPurchaseOrderForPurchaseInvoice'");
        return jdbcTemplate.queryForList(sql.toString(),args.toArray());
    }
    /**
     * LoadInGridDetail :5277 for the checked orders: LoadPurchaseOrderDataForInvoice (PurchaseOrderLoadForPurchaseInvoice,
     * DocumentTypeId 41, @POIds), LoadPurchaseOrderExpensesChargToProductData (GetPurchaseOrderExpensesChargToProdut @POIds),
     * LoadExpData (PurchaseOrderSupplierExpenseByOrderIds @OrderIds); the page then applies the desktop's row rules.
     */
    public Map<String,Object> loadOrders(String orderIds){
        records.requireRight(TYPE,"View");
        String ids=cleanIds(orderIds);if(ids.isEmpty())throw new IllegalArgumentException("Select a purchase order first");
        String poIds=","+ids;
        var result=new LinkedHashMap<String,Object>();
        result.put("orders",jdbcTemplate.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@POIds=?,@Activity='PurchaseOrderLoadForPurchaseInvoice' WITH RECOMPILE",currentUser.currentOrganizationId(),currentUser.currentCompanyId(),41,poIds));
        result.put("freight",jdbcTemplate.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @POIds=?,@Activity='GetPurchaseOrderExpensesChargToProdut' WITH RECOMPILE",poIds));
        result.put("expenses",jdbcTemplate.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @OrderIds=?,@Activity='PurchaseOrderSupplierExpenseByOrderIds' WITH RECOMPILE",poIds));
        return result;
    }
    /** GetEmptyBagsInformationFromOrder :1607 - PurchaseOrder.GetPurchaseOrderEmptyBagsDetailByOrderId(0, Ids) (BLL 0595 :1336). */
    public List<Map<String,Object>> orderEmptyBags(String orderIds){
        records.requireRight(TYPE,"View");
        String ids=cleanIds(orderIds);if(ids.isEmpty())return List.of();
        return jdbcTemplate.queryForList("EXEC dbo.Sp_PurchaseOrder_GetAllMethod @Ids=?,@Activity='GetPurchaseOrderEmptyBagsDetailByOrderId'",ids);
    }
    private static String cleanIds(String ids){
        if(ids==null)return "";var out=new LinkedHashSet<String>();
        for(String t:ids.split(",")){t=t.trim();if(t.isEmpty())continue;try{int v=Integer.parseInt(t);if(v>0)out.add(String.valueOf(v));}catch(NumberFormatException bad){throw new IllegalArgumentException("Invalid purchase order id");}}
        return String.join(",",out);
    }

    /** DeleteDetailrow :2240 - a stored row (Id > 0) is checked with InvPurchaseInvoice.StockInReferenceValidationReferredOrNot. */
    @Transactional
    public void detailRowCheck(int invoiceId,int detailId){
        if(detailId<=0)return;
        records.require(invoiceId,TYPE);records.requireRight(TYPE,"Update");
        com.mst.repositories.support.ProcExec.run(jdbcTemplate,"EXEC dbo.usp_StockInReferenceValidationReferredOrNot @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@Id=?,@DetailId=?",currentUser.currentOrganizationId(),currentUser.currentCompanyId(),TYPE,invoiceId,detailId);
    }

    // ------------------------------------------------------------------------------------------------ calculations
    @SuppressWarnings("unchecked")
    private Map<String,List<Map<String,Object>>> gridRows(Map<String,Object> payload,int id){
        var ui=new LinkedHashMap<String,List<Map<String,Object>>>();
        for(String key:List.of("freight","journal","expenses","emptyBags"))
            ui.put(key,payload.get(key) instanceof List<?> l?((List<Map<String,Object>>)l).stream().map(PurchaseInvoiceFinancialRules::copy).toList():id>0?storedAsGrid(id,key):List.of());
        return ui;
    }
    /** Stored rows in the grid's shape (ReadById :3618-3656) when the page did not send the grid. */
    private List<Map<String,Object>> storedAsGrid(int id,String key){
        boolean subsidiary=directLookups.subsidiary();
        var rows=new ArrayList<Map<String,Object>>();
        for(var raw:invoiceWrites.collection(id,key)){var r=copy(raw);
            if(key.equals("journal")){r.put("AccountId",subsidiary?i(r,"SupplierCustomerId"):i(r,"ChartofAccountId"));r.put("GlAccountId",i(r,"ChartofAccountId"));}
            if(key.equals("expenses")||key.equals("emptyBags"))r.putIfAbsent("CustomRemarks",r.get("Remarks"));
            rows.add(r);}
        return rows;
    }
    @SuppressWarnings("unchecked")
    public Map<String,Object> calculateBill(Map<String,Object> input){
        records.requireRight(TYPE,"View");
        var payload=copy(input);int id=i(payload,"Id");
        var header=copy(id>0?records.requireViewable(id,TYPE,historyBranchIds()::contains):null);header.putAll(payload);
        List<Map<String,Object>> details=((List<Map<String,Object>>)payload.getOrDefault("details",List.of())).stream().map(PurchaseInvoiceFinancialRules::copy).toList();
        var collections=PurchaseDirectInvoiceDesktopRules.forCalculation(gridRows(payload,id),directLookups.subsidiary());
        int gl=supplierGl(i(header,"SupplierCustomerId"));
        PurchaseDirectInvoiceCalculations.bill(header,details,collections.get("expenses"),collections.get("freight"),collections.get("journal"),collections.get("emptyBags"),gl,directLookups.flag("DebitAmountChargetoExpenseAcFreightGridPurchase"),directLookups.amountDigits());
        return Map.of("billAmount",header.get("BillAmount"),"commAmount",header.get("CommAmount"),"brokeryAmount",header.get("BrokeryAmount"),"details",details);
    }
    @SuppressWarnings("unchecked")
    public Map<String,Object> calculateLine(Map<String,Object> input){
        records.requireRight(TYPE,"View");
        var row=copy((Map<String,Object>)input.get("line"));
        int item=i(row,"ItemId");
        return PurchaseDirectInvoiceCalculations.line(row,Objects.toString(input.get("changed"),""),Boolean.TRUE.equals(input.get("newInvoice"))&&records.hasRight(TYPE,"Save"),Boolean.TRUE.equals(input.get("bagsAgainstWeight")),directLookups.equivalent(i(row,"ItemUOMId"),item),directLookups.equivalent(i(row,"UomScheduleIdRate"),item),directLookups.amountDigits());
    }

    // ------------------------------------------------------------------------------------------------ save / delete
    /**
     * saveToolStripButton_Click :3517 / btnUpdate_Click :3530 -> Insert() :3074: the desktop refusals in its order and words,
     * the four grids mapped as Insert builds its lists, then the shared 56/57 save. Messages :3472 / :3480.
     */
    @SuppressWarnings("unchecked")
    @Transactional
    public Map<String, Object> saveDirectInvoice(Map<String, Object> payload) {
        int id=i(payload,"Id");
        var stored=id>0?records.require(id,TYPE):null;
        if(stored!=null&&PurchaseDirectInvoiceDesktopRules.approved(stored))throw new IllegalArgumentException("Record Not Update because Record has approved");
        var h=copy(stored);h.putAll(payload);
        List<Map<String,Object>> details=payload.get("details") instanceof List<?> l?((List<Map<String,Object>>)l).stream().map(PurchaseInvoiceFinancialRules::copy).toList():id>0?invoiceWrites.collection(id,"details"):List.of();
        details=PurchaseDirectInvoiceDesktopRules.collapseRepeatedIds(details);
        var ui=gridRows(payload,id);
        boolean subsidiary=directLookups.subsidiary();
        boolean freightToExpenses=directLookups.flag("DebitAmountChargetoExpenseAcFreightGridPurchase");
        int digits=directLookups.amountDigits();
        double comm=PurchaseDirectInvoiceCalculations.commissionAmount(h,details,digits);
        PurchaseDirectInvoiceDesktopRules.validate(h,details,ui.get("freight"),ui.get("journal"),ui.get("expenses"),ui.get("emptyBags"),freightToExpenses,comm);
        // Insert :3178 BillAmount() - the supplier's GL chosen in Supplier AddLess.
        PurchaseDirectInvoiceDesktopRules.journalSupplier(ui.get("journal"),supplierGl(i(h,"SupplierCustomerId")));
        var collections=PurchaseDirectInvoiceDesktopRules.forSave(ui,subsidiary,names(subsidiary));
        var body=new LinkedHashMap<String,Object>(payload);
        body.put("details",details);
        body.putAll(collections);
        // :3204 SupplierInvoiceDate = DateTime.Now on save and on update.
        body.put("SupplierInvoiceDate",new java.sql.Timestamp(System.currentTimeMillis()));
        // Insert never sets PaymentTermsId / InvoiceTypeId on its new InvPurchaseInvoice: the int defaults (0) are sent (quirk kept).
        body.put("PaymentTermsId",0);body.put("InvoiceTypeId",0);
        body.put("CustomAccounts",Boolean.TRUE.equals(payload.get("CustomAccounts")));
        // The direct form has no payment schedule: its model lists stay null (nothing inserted; the update procedure deletes).
        body.put("paymentTerms",List.of());body.put("paymentDues",List.of());
        var result=new LinkedHashMap<String,Object>(persistence.save(body,TYPE));
        result.put("message",id>0?"Record Update Successfully":"Record Saved Successfully");
        // :3482 - frmwagesBillHeader(RefDocTypeId 57, RefDocId, GrossWeightTotal) when WagesCompulsoryOnPurchaseInvoiceDirect and wages are active for 57.
        boolean wages=directLookups.flag("WagesCompulsoryOnPurchaseInvoiceDirect")&&directLookups.wagesActive();
        result.put("openWages",wages);
        result.put("grossWeightTotal",details.stream().mapToDouble(d->n(d,"GrossWeight")).sum());
        return result;
    }
    private PurchaseDirectInvoiceDesktopRules.Names names(boolean subsidiary){
        var other=new HashMap<Integer,String>();for(var r:directLookups.otherItems()){var c=copy(r);other.put(i(c,"Id"),s(c,"OtherItemName"));}
        var pm=new HashMap<Integer,String>();for(var r:directLookups.packingMaterialItems()){var c=copy(r);pm.put(i(c,"ItemId"),s(c,"ItemName"));}
        var accounts=new HashMap<Integer,String>();for(var r:directLookups.accounts(subsidiary)){var c=copy(r);accounts.putIfAbsent(subsidiary?i(c,"Id"):i(c,"ChartOfAccountId"),subsidiary?s(c,"CompanyName"):s(c,"AccountTitle"));}
        var conditions=new HashMap<Integer,String>();for(var r:directLookups.itemConditions()){var c=copy(r);conditions.put(i(c,"Id"),s(c,"ConditionStatus"));}
        return new PurchaseDirectInvoiceDesktopRules.Names(other,pm,accounts,conditions);
    }
    private int supplierGl(int supplier){
        var party=jdbcTemplate.queryForList("SELECT GlAccountId FROM dbo.SupplierCustomer WHERE Id=? AND OrganizationId=? AND CompanyId=?",supplier,currentUser.currentOrganizationId(),currentUser.currentCompanyId());
        return party.isEmpty()?0:i(copy(party.get(0)),"GlAccountId");
    }

    /** btnDelete_Click :3699 - approved records are refused; RemoveByID -> Sp_InvoicesVouchersandStocksDelete. */
    @Transactional
    public boolean deleteDirectInvoice(int id) {
        if(PurchaseDirectInvoiceDesktopRules.approved(records.require(id,TYPE)))throw new IllegalArgumentException("Record Not Delete because Record has approved");
        records.delete(id,TYPE);
        return true;
    }
}
