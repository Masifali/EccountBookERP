package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.CmtrBrokerScheduleRepository;
import com.mst.services.cmagt.CommissionDropdownService;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.sql.Date;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/** Business rules and DDL parameter contract from CommissionBrokerySchedule.cs / CMTr BLL. */
@Service
public class CmtrBrokerScheduleService {
    public static final int DOCUMENT_TYPE_ID = 1700;
    private static final String FORM = "CommissionBrokerySchedule";
    private final CmtrBrokerScheduleRepository repo;
    private final CommissionDropdownService dropdowns;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;

    public CmtrBrokerScheduleService(CmtrBrokerScheduleRepository repo, CommissionDropdownService dropdowns,
                                    CurrentUserContext context,
                                    DesktopReportRights rights) {
        this.repo = repo; this.dropdowns = dropdowns; this.context = context; this.rights = rights;
    }

    public Map<String,Object> initialize(int screenId) {
        UserAccount u=user(screenId,"View");
        Map<String,Object> r=new LinkedHashMap<>();
        List<List<Map<String,Object>>> lookups=new ArrayList<>();
        for(int type=0;type<=5;type++) lookups.add(new ArrayList<>());
        for(Map<String,Object> row:repo.lookups()){
            int type=number(row.get("InvLookupTypeId"));
            if(type>=0&&type<lookups.size())lookups.get(type).add(row);
        }
        r.put("lookups",lookups); r.put("today",LocalDate.now().toString());
        r.put("parties",dropdowns.parties(true));
        r.put("permissions",Map.of("save",allowed(u,screenId,"Save"),"update",allowed(u,screenId,"Update"),
                "delete",allowed(u,screenId,"Delete"),"print",allowed(u,screenId,"Print"),
                "gridPrint",allowed(u,screenId,"Grid Print"),"gridExport",allowed(u,screenId,"Grid Export")));
        r.put("screenId",screenId); r.put("documentTypeId",DOCUMENT_TYPE_ID);
        return r;
    }

    public List<Map<String,Object>> history(int screenId) {
        UserAccount u=user(screenId,"View");
        LinkedHashMap<String,Object> p=new LinkedHashMap<>();
        p.put("OrganizationId",u.getOrganizationId());p.put("CompanyId",u.getCompanyId());
        p.put("BranchesId",u.getBranchesId());p.put("DocumentTypeId",DOCUMENT_TYPE_ID);
        p.put("FinancialYearId",context.currentFinancialYearId());p.put("NoOfRecords",0);
        p.put("CanViewAllRecord",allowed(u,screenId,"CanView AllRecord"));
        if(!Boolean.TRUE.equals(p.get("CanViewAllRecord")))p.put("EntryUserId",u.getId());
        p.put("Activity","FormHistory");
        return repo.history(p);
    }

    public Map<String,Object> record(int screenId,int id) {
        user(screenId,"View");
        List<Map<String,Object>> h=repo.readHeader(id);
        if(h.isEmpty())throw new IllegalArgumentException("The selected schedule was not found.");
        Map<String,Object> r=new LinkedHashMap<>();r.put("header",h.get(0));r.put("details",repo.readDetails(id));return r;
    }

    public List<Map<String,Object>> rate(int screenId,Map<String,String> q) {
        user(screenId,"View");
        LinkedHashMap<String,Object> p=new LinkedHashMap<>();
        p.put("EffectiveDate",date(q.get("effectiveDate"),"Effective Date"));
        p.put("CalculationParameterId",positive(q,"calculationParameterId"));
        p.put("RevenueExpenseId",positive(q,"revenueExpenseId"));
        p.put("SupplierCustomerId",positive(q,"supplierCustomerId"));
        p.put("ChargesIncentiveId",positive(q,"chargesIncentiveId"));
        p.put("PaymentTermId",positive(q,"paymentTermId"));
        p.put("DueDays",nonNegative(q.get("dueDays"),"Due Days"));
        p.put("Activity","GetCommScheduleRateByDateCalParamAndRevExpId");
        return repo.exec("[CmTr].[USP_CommTradeRevenueExpenseSchedule_GetAllMethod]",p);
    }

    @Transactional
    public Map<String,Object> save(int screenId,Map<String,Object> body) {
        UserAccount u=user(screenId,number(body.get("id"))>0?"Update":"Save");
        int id=number(body.get("id")); int party=number(body.get("supplierCustomerId"));
        if(party<=0)throw new IllegalArgumentException("Customer/Supplier Field is Required");
        String policy=text(body.get("policyCode")); String remarks=text(body.get("remarksHeader"));
        if(policy.isBlank()||"0".equals(policy))throw new IllegalArgumentException("Policy Code Field is Required");
        Object raw=body.get("details"); if(!(raw instanceof List<?> list)||list.isEmpty())throw new IllegalArgumentException("Grid Record Not Found");
        LocalDateTimeNow now=new LocalDateTimeNow();
        LinkedHashMap<String,Object> header=new LinkedHashMap<>();
        header.put("Id",id==0?null:id);header.put("DocumentTypeId",DOCUMENT_TYPE_ID);header.put("SupplierCustomerId",party);
        header.put("PolicyCode",policy);header.put("RemarksHeader",remarks);header.put("OrganizationId",u.getOrganizationId());
        header.put("CompanyId",u.getCompanyId());header.put("BranchId",u.getBranchesId());header.put("FinancialYearId",context.currentFinancialYearId());
        header.put("EntryUserId",u.getId());header.put("EntryDate",now.timestamp);header.put("ModifyUserId",u.getId());
        header.put("ModifyDate",now.timestamp);header.put("IsApproved",false);header.put("ApprovedUserId",null);
        header.put("ApprovedDate",null);header.put("ActionId",id==0?1:2);header.put("RowVersion",null);header.put("ScreenName",FORM);
        Object saved=repo.saveHeader(id==0?"[CmTr].[USP_CommTradeRevenueExpenseSchedule_Insert]":"[CmTr].[USP_CommTradeRevenueExpenseSchedule_Update]",header,id==0);
        int savedId=number(saved); if(savedId<=0)throw new IllegalStateException("The schedule id was not returned.");
        int sort=1;
        for(Object item:list){
            if(!(item instanceof Map<?,?> m))continue;
            int detailId=number(m.get("id"));
            required(m.get("daysFrom"),"Days From");required(m.get("daysTo"),"Days To");
            required(m.get("rate"),"Rate");required(m.get("rateUom"),"Rate UOM");
            int from=nonNegative(m.get("daysFrom"),"Days From"),to=nonNegative(m.get("daysTo"),"Days To");
            if(from>to)throw new IllegalArgumentException("DaysFrom Can not be Greater than DaysTo...");
            int action=number(m.get("actionTypeId"));if(action==0)action=detailId>0?2:1;
            LinkedHashMap<String,Object> d=new LinkedHashMap<>();d.put("Id",detailId>0?detailId:null);d.put("CommTradeRevenueExpenseScheduleId",savedId);
            d.put("IsRevenueExpense",positive(m,"isRevenueExpense"));d.put("EffectiveDate",date(text(m.get("effectiveDate")),"Effective Date"));
            d.put("TransactionTypeId",positive(m,"transactionTypeId"));d.put("PaymentTermId",positive(m,"paymentTermId"));
            d.put("ChargesIncentiveId",positive(m,"chargesIncentiveId"));d.put("CalculationParameterId",positive(m,"calculationParameterId"));
            d.put("DaysFrom",from);d.put("DaysTo",to);d.put("Rate",decimal(m.get("rate"),"Rate"));
            d.put("RateUom",decimal(m.get("rateUom"),"Rate UOM"));d.put("SortNo",number(m.containsKey("sortNo")?m.get("sortNo"):sort));
            d.put("ActionTypeId",action);d.put("RowVersion",null);repo.saveDetail(d);sort++;
        }
        Object removed=body.get("removedDetailIds");if(removed instanceof List<?> removedIds)for(Object value:removedIds){
            int detailId=number(value);if(detailId<=0)continue;
            LinkedHashMap<String,Object>d=new LinkedHashMap<>();d.put("Id",detailId);d.put("CommTradeRevenueExpenseScheduleId",savedId);
            d.put("IsRevenueExpense",0);d.put("EffectiveDate",Date.valueOf(LocalDate.now()));d.put("TransactionTypeId",0);
            d.put("PaymentTermId",0);d.put("ChargesIncentiveId",0);d.put("CalculationParameterId",0);d.put("DaysFrom",0);d.put("DaysTo",0);
            d.put("Rate",BigDecimal.ZERO);d.put("RateUom",BigDecimal.ZERO);d.put("SortNo",sort++);d.put("ActionTypeId",3);d.put("RowVersion",null);repo.saveDetail(d);
        }
        return Map.of("success",true,"id",savedId,"message",id==0?"Data Save Successfully.... ":"Data Update Successfully.... ");
    }

    public Map<String,Object> delete(int screenId,int id) {
        UserAccount u=user(screenId,"Delete");if(id<=0)throw new IllegalArgumentException("Select a schedule first.");
        LinkedHashMap<String,Object> p=new LinkedHashMap<>();p.put("EntryUserId",u.getId());p.put("Id",id);p.put("Activity","DeleteById");
        repo.exec("[CmTr].[USP_CommTradeRevenueExpenseSchedule_GetAllMethod]",p);return Map.of("success",true,"message","Data Deleted Successfully.... ");
    }

    private UserAccount user(int screenId,String action){if(screenId!=516&&screenId!=521)throw new IllegalArgumentException("Invalid screen id.");UserAccount u=context.requireAccountingUser();rights.require(u,screenId,action);return u;}
    private boolean allowed(UserAccount u,int screenId,String action){try{rights.require(u,screenId,action);return true;}catch(AccessDeniedException e){return false;}}
    private static int positive(Map<?,?> m,String k){int v=number(m.get(k));if(v<=0)throw new IllegalArgumentException(k+" is required.");return v;}
    private static int nonNegative(Map<String,?> m,String k){return nonNegative(m.get(k),k);}
    private static int nonNegative(Object x,String k){int v=number(x);if(v<0)throw new IllegalArgumentException(k+" cannot be negative.");return v;}
    private static void required(Object x,String label){if(x==null||String.valueOf(x).isBlank())throw new IllegalArgumentException(label+" Field Required");}
    private static BigDecimal decimal(Object x,String label){try{return new BigDecimal(String.valueOf(x==null||String.valueOf(x).isBlank()?"0":x));}catch(Exception e){throw new IllegalArgumentException(label+" is invalid.");}}
    private static Date date(String s,String label){if(s==null||s.isBlank())throw new IllegalArgumentException(label+" Field Required");try{return Date.valueOf(s);}catch(Exception e){throw new IllegalArgumentException(label+" is invalid.");}}
    private static int number(Object x){if(x instanceof Number n)return n.intValue();try{return x==null?0:Integer.parseInt(String.valueOf(x));}catch(Exception e){return 0;}}
    private static String text(Object x){return x==null?"":String.valueOf(x).trim();}
    private static final class LocalDateTimeNow { final Timestamp timestamp=new Timestamp(System.currentTimeMillis()); }
}
