package com.mst.repositories;

import com.mst.repositories.support.ProcExec;
import com.mst.services.PurchaseInvoiceFinancialRules;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Repository;
import java.sql.Types;
import java.util.*;
import static com.mst.services.PurchaseInvoiceFinancialRules.*;

/** Original invoice DAL contracts and posting order. Caller owns the transaction and access checks. */
@Repository
public class PurchaseInvoiceWriteRepository {
    private final JdbcTemplate jdbc;
    public PurchaseInvoiceWriteRepository(JdbcTemplate jdbc){this.jdbc=jdbc;}
    public Map<String,Object> defaults(String procedure){var values=copy(null);for(var p:contract(procedure))values.put(p.name(),p.defaultValue());return values;}
    private List<PurchaseInvoiceContracts.Parameter> contract(String procedure){var c=PurchaseInvoiceContracts.ALL.get(procedure);if(c==null)throw new IllegalArgumentException("Unknown invoice procedure");return c;}
    public int execute(String procedure,Map<String,Object> supplied){
        var values=copy(supplied);List<String> names=new ArrayList<>();List<Object> args=new ArrayList<>();
        for(var p:contract(procedure)){
            Object value=values.get(p.name());if(value==null)value=p.defaultValue();
            if(value instanceof String&&(p.type()==Types.DATE||p.type()==Types.TIMESTAMP)){
                String v=value.toString().trim().replace('T',' ');value=v.isEmpty()?null:p.type()==Types.DATE?java.sql.Date.valueOf(v.substring(0,10)):java.sql.Timestamp.valueOf(v.length()==10?v+" 00:00:00":v);
            }
            if(value instanceof java.util.Date)value=new java.sql.Timestamp(((java.util.Date)value).getTime());
            names.add("@"+p.name()+"=?");args.add(new SqlParameterValue(p.type(),value));
        }
        Integer id=ProcExec.call(jdbc,"EXEC dbo."+procedure+" "+String.join(",",names),args.toArray());return id==null?0:id;
    }
    private Map<Integer,Map<String,Object>> index(List<Map<String,Object>> rows){var result=new LinkedHashMap<Integer,Map<String,Object>>();for(var row:rows){var c=copy(row);result.put(i(c,"Id"),c);}return result;}
    public String configuration(int org,int company,String name){var rows=jdbc.queryForList("EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?,@CompanyId=?,@ConfigDescription=?,@Activity='GetConfigurationByOrgCompandConfigDescription'",org,company,name);return rows.isEmpty()?"":s(copy(rows.get(0)),"ConfigKey");}
    /** InvfrmPurchaseInvoice:3222-3250, CommonServices.GetUomScheduleByItemId. */
    public void resolveRateUoms(Map<String,Object> header,List<Map<String,Object>> details){
        if(!Set.of(2,3,4).contains(i(header,"InvoiceTypeId")))return;
        var byItem=new HashMap<Integer,List<Map<String,Object>>>();
        for(var detail:details){
            // Stored DAL rows omit the display equivalent; an unchanged record keeps its original UOM.
            if(!detail.containsKey("EquivalentPoRate"))continue;
            int item=i(detail,"ItemId");
            var schedules=byItem.computeIfAbsent(item,key->jdbc.queryForList("EXEC dbo.Sp_UOMSchedule_GetAllMethod @OrganizationId=?,@CompanyId=?,@ItemId=?,@Activity='ReadByItemID'",i(header,"OrganizationId"),i(header,"CompanyId"),key));
            if(schedules.isEmpty())continue;
            double equivalent=Math.rint(n(detail,"EquivalentPoRate"));
            var match=schedules.stream().map(PurchaseInvoiceFinancialRules::copy).filter(row->Double.compare(n(row,"Equivalent"),equivalent)==0).findFirst().orElseThrow(()->new IllegalArgumentException("Rate UOM is not defined for item "+s(detail,"ItemName")));
            detail.put("UomScheduleIdRate",i(match,"Id"));
        }
    }
    public PurchaseInvoiceFinancialRules.Accounts accounts(Map<String,Object> h,List<Map<String,Object>> details){
        int org=i(h,"OrganizationId"),company=i(h,"CompanyId");
        var items=index(jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='GetItemGlIdsandItemName'",org,company));
        var parties=index(jdbc.queryForList("EXEC dbo.Sp_SupplierCustomer_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='GetGlAccountIdandCompanyNameBySupplierCustomerId'",org,company));
        var lots=new HashMap<Integer,Integer>();for(var d:details)if(i(d,"JobLotId")>0&&!lots.containsKey(i(d,"JobLotId"))){
            var scoped=jdbc.queryForList("SELECT Id FROM dbo.JobLot WHERE Id=? AND OrganizationId=? AND CompanyId=?",i(d,"JobLotId"),org,company);if(scoped.isEmpty())throw new IllegalArgumentException("Job lot is outside the current company");
            var row=copy(jdbc.queryForMap("EXEC dbo.SP_JobLot_ReadMethod @Id=?,@Activity='GetById'",i(d,"JobLotId")));lots.put(i(d,"JobLotId"),i(row,"AccountId"));
        }
        String offset=configuration(org,company,"StockAgainstAccount");
        return new PurchaseInvoiceFinancialRules.Accounts(items,parties,lots,offset.isBlank()?0:Integer.parseInt(offset),Boolean.parseBoolean(configuration(org,company,"AutoRemarksFormatII")));
    }
    public List<Map<String,Object>> collection(int id,String name){
        String table=switch(name){case "details"->"InvPurchaseInvoiceDetail";case "freight"->"InvPurchaseInvoiceFreight";case "journal"->"InvPurchaseInvoiceJournal";case "expenses"->"InvPurchaseInvoiceExpense";case "emptyBags"->"InvPurchaseInvoiceEmptyBags";case "paymentTerms"->"InvPurchaseInvoicePaymentTermsDetail";default->throw new IllegalArgumentException("Unknown invoice collection");};
        return jdbc.queryForList("SELECT * FROM dbo."+table+" WHERE InvPurchaseInvoiceId=? ORDER BY Id",id);
    }
    public void post(Map<String,Object> h,PurchaseInvoiceFinancialRules.Voucher voucher){
        int org=i(h,"OrganizationId"),company=i(h,"CompanyId"),type=i(h,"DocumentTypeId"),id=i(h,"Id");
        ProcExec.run(jdbc,"EXEC dbo.usp_StockInTransit_EvaluationAndVoucherDelete_ByPurchaseInvoiceId @Id=?",id);
        ProcExec.run(jdbc,"EXEC dbo.Sp_InventoryStockEvalautionDetail_Update @OrganizationId=?,@CompanyId=?,@RefDocumentTypeId=?,@RefDocIdNo=?",org,company,type,id);
        var vh=voucher.header();var ids=jdbc.queryForList("EXEC dbo.Sp_Vouchers_GetMethods @Activity='GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId',@OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@DocumentTypeSrNo=?",org,company,type,id);
        int previous=ids.isEmpty()?0:i(copy(ids.get(0)),"Id");vh.put("Id",previous);vh.put("DocumentTypeSrNo",id);vh.put("RefDocNoId",id);vh.put("VoucherCode",h.get("DocNo"));
        int voucherId=execute(previous>0?"Sp_VoucherHead_Update":"Sp_VoucherHead_Insert",vh);if(voucherId<=0)voucherId=previous;if(voucherId<=0)throw new IllegalStateException("Voucher procedure did not return an ID");vh.put("Id",voucherId);
        for(var row:voucher.details()){row.put("VoucherHeadId",voucherId);row.put("BranchesId",h.get("BranchesId"));execute("Sp_VoucherDetail_Insert",row);}
        ProcExec.run(jdbc,"EXEC dbo.USP_VoucherBalanceCheck @OrganizationId=?,@CompanyId=?,@Id=?",org,company,voucherId);
        int historyId=execute("Sp_VoucherHead_H_Insert",vh);
        for(var row:voucher.details()){row.put("DocumentTypeIdRef",historyId);execute("Sp_VoucherDetail_H_Insert",row);}
        if(type==57)ProcExec.run(jdbc,"EXEC dbo.Sp_InventoryTransactions_GetALLMethod @OrganizationId=?,@CompanyId=?,@RefDocumentTypeId=?,@RefDocIdNo=?",org,company,type,id);
        ProcExec.run(jdbc,"EXEC DAW.USp_DocumentApprovalDetail_Insert @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@Id=?,@LimitAmount=?",org,company,type,id,n(h,"BillAmount"));
    }
}
