package com.mst.services;

import com.mst.repositories.*;
import com.mst.repositories.support.ProcExec;
import com.mst.security.CurrentUserContext;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import java.util.*;
import static com.mst.services.PurchaseInvoiceFinancialRules.*;

/** Atomic original DAL 0434 save for weighted Purchase Invoice and Direct Purchase Invoice. */
@Service
public class PurchaseInvoicePersistenceService {
    private final PurchaseInvoiceRecordRepository records;private final PurchaseInvoiceWriteRepository writes;
    private final CurrentUserContext context;private final JdbcTemplate jdbc;
    private final PurchaseInvoiceAttachmentService attachments;
    private static final Map<String,String> PROCEDURES=Map.of("freight","Sp_InvPurchaseInvoiceFreight_Insert","journal","Sp_InvPurchaseInvoiceJournal_Insert","expenses","Sp_InvPurchaseInvoiceExpense_Insert","emptyBags","Sp_InvPurchaseInvoiceEmptyBags_Insert","paymentTerms","USP_InvPurchaseInvoicePaymentTermsDetail_Insert");
    public PurchaseInvoicePersistenceService(PurchaseInvoiceRecordRepository records,PurchaseInvoiceWriteRepository writes,CurrentUserContext context,JdbcTemplate jdbc,PurchaseInvoiceAttachmentService attachments){this.records=records;this.writes=writes;this.context=context;this.jdbc=jdbc;this.attachments=attachments;}
    @SuppressWarnings("unchecked")
    private List<Map<String,Object>> rows(Map<String,Object> payload,String key,List<Map<String,Object>> stored){
        Object supplied=payload.get(key);if(supplied==null)return stored.stream().map(PurchaseInvoiceFinancialRules::copy).toList();
        if(!(supplied instanceof List<?>))throw new IllegalArgumentException(key+" must be a list");
        var originals=new HashMap<Integer,Map<String,Object>>();for(var row:stored)originals.put(i(copy(row),"Id"),row);
        var result=new ArrayList<Map<String,Object>>();var seen=new HashSet<Integer>();
        for(Object value:(List<?>)supplied){if(!(value instanceof Map<?,?>))throw new IllegalArgumentException("Invalid "+key+" row");var row=copy((Map<String,Object>)value);int id=i(row,"Id");
            if(id>0&&(!originals.containsKey(id)||!seen.add(id)))throw new IllegalArgumentException("A "+key+" row belongs to another invoice or is repeated");
            var merged=copy(originals.get(id));merged.putAll(row);result.add(merged);
        }return result;
    }
    @Transactional
    public Map<String,Object> save(Map<String,Object> supplied,int type){
        if(type!=56&&type!=57)throw new IllegalArgumentException("Unsupported weighted purchase invoice type");
        var payload=copy(supplied);int id=i(payload,"Id");boolean update=id>0;records.requireRight(type,update?"Update":"Save");
        var old=update?records.require(id,type):copy(null);var h=writes.defaults("Sp_InvPurchaseInvoice_Insert");h.putAll(old);h.putAll(payload);
        // The form edits OtherRemarks; RemarksHeader contains the generated financial narrative.
        if(!payload.containsKey("RemarksHeader"))h.put("RemarksHeader",old.get("OtherRemarks"));
        h.put("OtherRemarks",h.get("RemarksHeader"));
        h.put("OrganizationId",context.currentOrganizationId());h.put("CompanyId",context.currentCompanyId());h.put("BranchesId",context.currentBranchId());h.put("ProjectsId",context.currentBranchId());h.put("FinancialYearId",context.currentFinancialYearId());h.put("DocumentTypeId",type);
        h.put("EntryUser",update?old.get("EntryUser"):context.currentUserId());h.put("EntryDate",update?old.get("EntryDate"):new java.sql.Timestamp(System.currentTimeMillis()));h.put("ModifyUser",update?context.currentUserId():0);h.put("ModifyDate",new java.sql.Timestamp(System.currentTimeMillis()));h.put("ActionId",update?2:1);h.put("ScreenName",type==56?"InvfrmPurchaseInvoice":"InvfrmPurchasedirectInvoice");
        for(String key:List.of("IsApproved","PostDate","PostUser","AttachmentsValues","CustomAttachmentsValues","RowVersion","IsUploaded","UploadedDate","UploadedById"))h.put(key,update?old.get(key):null);
        if(!update){if(!payload.containsKey("DocDate"))h.put("DocDate",java.sql.Date.valueOf(java.time.LocalDate.now()));if(!payload.containsKey("DueDate"))h.put("DueDate",h.get("DocDate"));if(!payload.containsKey("SupplierInvoiceDate"))h.put("SupplierInvoiceDate",h.get("DocDate"));}
        if(i(h,"SupplierCustomerId")<=0)throw new IllegalArgumentException("Select a supplier");
        for(String key:List.of("AttachmentsList","DeleteAttachmentsList"))if(payload.get(key) instanceof Collection<?> c&&!c.isEmpty())throw new IllegalArgumentException("Use the invoice attachment dialog");
        var details=rows(payload,"details",update?writes.collection(id,"details"):List.of());if(details.isEmpty())throw new IllegalArgumentException("Detail list not found");
        var collections=new LinkedHashMap<String,List<Map<String,Object>>>();
        for(String name:List.of("freight","journal","expenses","emptyBags","paymentTerms"))collections.put(name,rows(payload,name,update?writes.collection(id,name):List.of()));
        var storedDues=update?jdbc.queryForList("SELECT *,PaymentDueScheduleId AS Id FROM dbo.PaymentDueSchedule WHERE RefdocNoId=? AND refDocumentTypeId=?",id,type):List.<Map<String,Object>>of();
        var dues=rows(payload,"paymentDues",storedDues);
        var warehouses=new HashSet<Integer>();for(var w:jdbc.queryForList("EXEC dbo.USP_GetWarehousesAllocatedToBranch @OrganizationId=?,@CompanyId=?,@BranchId=?",h.get("OrganizationId"),h.get("CompanyId"),h.get("BranchesId")))warehouses.add(i(copy(w),"Id"));
        for(var d:details){
            if(type==56&&payload.containsKey("LocationTypeId"))d.put("LocationTypeId",i(payload,"LocationTypeId"));
            if(i(d,"ItemId")<=0||!warehouses.contains(i(d,"WarehouseId")))throw new IllegalArgumentException("Select an item and an allocated warehouse for every line");
            for(String field:List.of("PackingTypeId","JobLotId","ItemUOMId","ItemQty","GrossWeight","NetBillWeight","NetStockWeight","ItemRate","ItemAmount"))if(n(d,field)<=0)throw new IllegalArgumentException(field+" is required for every invoice line");
            if(s(d,"CropYear").isBlank())throw new IllegalArgumentException("CropYear is required for every invoice line");
            if(type==57&&i(d,"UomScheduleIdRate")<=0)throw new IllegalArgumentException("Rate UOM is required");
            if(type==56&&(i(d,"InvGrnId")<=0||i(d,"InvGrnDetailId")<=0))throw new IllegalArgumentException("Select an original GRN and GRN detail for every purchase invoice line");
            if(i(d,"InvGrnId")>0&&jdbc.queryForObject("SELECT COUNT(*) FROM dbo.InvGrn g JOIN dbo.InvGrnDetail d ON d.InvGrnId=g.Id WHERE g.Id=? AND d.Id=? AND g.OrganizationId=? AND g.CompanyId=? AND g.BranchesId=? AND d.ItemId=?",Integer.class,i(d,"InvGrnId"),i(d,"InvGrnDetailId"),h.get("OrganizationId"),h.get("CompanyId"),h.get("BranchesId"),i(d,"ItemId"))!=1)throw new IllegalArgumentException("GRN reference is outside this company/branch or does not match the item");
        }
        if(type==56)writes.resolveRateUoms(h,details);
        h.put("InvoiceQty",details.stream().mapToDouble(d->n(d,"ItemQty")).sum());h.put("InvoiceWeight",details.stream().mapToDouble(d->n(d,"NetBillWeight")).sum());
        var accounts=writes.accounts(h,details);
        if(type==57&&payload.containsKey("details")){
            String precision=writes.configuration(i(h,"OrganizationId"),i(h,"CompanyId"),"Default NoofDecimal Points For Amount");int digits=precision.isBlank()?0:Integer.parseInt(precision);
            boolean freightToExpense=Boolean.parseBoolean(writes.configuration(i(h,"OrganizationId"),i(h,"CompanyId"),"DebitAmountChargetoExpenseAcFreightGridPurchase"));
            PurchaseDirectInvoiceCalculations.bill(h,details,collections.get("expenses"),collections.get("freight"),collections.get("journal"),collections.get("emptyBags"),i(copy(accounts.parties().get(i(h,"SupplierCustomerId"))),"GlAccountId"),freightToExpense,digits);
        }
        if(type==56&&payload.containsKey("details")){
            int org=i(h,"OrganizationId"),company=i(h,"CompanyId");String precision=writes.configuration(org,company,"Default NoofDecimal Points For Amount");
            boolean subsidiary=jdbc.queryForList("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?,@CompanyId=?",org,company).stream().anyMatch(r->i(copy(r),"Id")==4);
            var config=new PurchaseInvoiceCalculations.Configuration(precision.isBlank()?0:Integer.parseInt(precision),Boolean.parseBoolean(writes.configuration(org,company,"DebitAmountChargetoExpenseAcFreightGridPurchase")),Boolean.parseBoolean(writes.configuration(org,company,"WagesAmountCalculateOnQty")),Boolean.parseBoolean(writes.configuration(org,company,"ContractWagesChargetoProduct")),subsidiary);
            PurchaseInvoiceCalculations.bill(h,details,collections.get("expenses"),collections.get("freight"),collections.get("journal"),collections.get("emptyBags"),i(copy(accounts.parties().get(i(h,"SupplierCustomerId"))),"GlAccountId"),config);
            collections.put("paymentTerms",PurchaseInvoicePaymentRules.calculate(h,details,collections.get("paymentTerms"),!Boolean.FALSE.equals(payload.get("paymentByPercent")),true));
        }
        var financial=PurchaseInvoiceFinancialRules.calculate(h,details,collections.get("freight"),collections.get("journal"),collections.get("emptyBags"),accounts);
        // The desktop performs this check after posting. Fail before writes too; the database check is retained.
        double difference=financial.details().stream().mapToDouble(d->n(d,"DebitAmount")-n(d,"CreditAmount")).sum();
        if(Math.abs(difference)>0.01)throw new IllegalArgumentException("Invoice voucher does not balance; check allocated freight, brokery and empty-bag charges (difference "+difference+")");
        var attachmentChange=payload.get("attachments")==null?null:attachments.prepare(id,type,payload.get("attachments"));
        if(attachmentChange!=null){h.put("AttachmentsValues",attachmentChange.names());h.put("CustomAttachmentsValues",attachmentChange.storedNames());}
        int newId=writes.execute(update?"Sp_InvPurchaseInvoice_Update":"Sp_InvPurchaseInvoice_Insert",h);if(!update)id=newId;if(id<=0)throw new IllegalStateException("Invoice procedure did not return an ID");h.put("Id",id);
        h.put("DocNo",jdbc.queryForObject("SELECT DocNo FROM dbo.InvPurchaseInvoice WHERE Id=?",Integer.class,id));
        boolean wages=Boolean.parseBoolean(writes.configuration(i(h,"OrganizationId"),i(h,"CompanyId"),"ContractWagesChargetoProduct"));
        int line=0;var ids=new ArrayList<String>();for(var d:details){d.put("InvPurchaseInvoiceId",id);d.put("LineId",++line);d.put("BillAmount",n(d,"ItemAmount")+n(d,"FreightAmount")+n(d,"ExpenseAmount")+n(d,"CommissionAmount")+n(d,"Brokery")+(wages?n(d,"WagesAmount"):0)-n(d,"EbPurAgainstWeightAmount")-n(d,"FreightDeduction"));int detailId=writes.execute("Sp_InvPurchaseInvoiceDetail_Insert",d);if(detailId<=0)throw new IllegalStateException("Invoice detail procedure did not return an ID");ids.add(Integer.toString(detailId));}
        for(var c:collections.entrySet()){int sort=0;for(var row:c.getValue()){row.put("InvPurchaseInvoiceId",id);if(c.getKey().equals("paymentTerms"))row.put("SortNo",++sort);writes.execute(PROCEDURES.get(c.getKey()),row);}}
        for(var row:dues){row.put("RefdocNoId",id);row.put("refDocumentTypeId",type);writes.execute("Sp_PaymentDueSchedule_Insert",row);}
        ProcExec.run(jdbc,"EXEC dbo.usp_PurchaseInvoiceDetailRemoveByDetailIdsAndValidations @OrganizationId=?,@CompanyId=?,@Id=?,@DetailIds=?",h.get("OrganizationId"),h.get("CompanyId"),id,String.join(",",ids)+",");
        if(attachmentChange!=null)attachments.persist(id,type,i(h,"SupplierCustomerId"),attachmentChange);
        writes.post(h,financial);
        return Map.of("success",true,"id",id,"docNo",h.get("DocNo"),"message","Purchase invoice saved successfully");
    }
}
