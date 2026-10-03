package com.mst.repositories;

import com.mst.models.UserAccount;
import java.sql.Date;
import java.time.LocalDate;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/** VoucherReports.cs:5780. Calculations and interval captions come from SQL Server. */
@Repository
public class DocumentAgingRepository {
    private final JdbcTemplate jdbc;
    public DocumentAgingRepository(JdbcTemplate jdbc) { this.jdbc=jdbc; }
    public List<Map<String,Object>> supplier(UserAccount user,LocalDate date,int days,int account,int group,int custom) {
        return load("usp_SupplierAgingReport_DocumentWise",user,date,days,account,group,custom);
    }
    public List<Map<String,Object>> customer(UserAccount user,LocalDate date,int days,int account,int group,int custom) {
        return load("usp_CustomerAgingReport_DocumentWise",user,date,days,account,group,custom);
    }
    private List<Map<String,Object>> load(String procedure,UserAccount user,LocalDate date,int days,int account,int group,int custom) {
        List<Object> p=new ArrayList<>(Arrays.asList(user.getOrganizationId(),user.getCompanyId(),Date.valueOf(date)));
        StringBuilder sql=new StringBuilder("EXEC dbo."+procedure+" @OrganizationId=?, @CompanyId=?, @AsOnDate=?");
        add(sql,p,"AgingDays",days); add(sql,p,"AccountId",account); add(sql,p,"PartyGroupId",group); add(sql,p,"CustomGruopId",custom);
        return ReportValueSupport.decimalStrings(jdbc.queryForList(sql.toString(),p.toArray()));
    }
    public Map<String,Object> supplierLookups(UserAccount u) {
        return lookups(u,"3,8","3",0);
    }
    public Map<String,Object> customerLookups(UserAccount u) {
        return lookups(u,"3","2",0);
    }
    /** frmPayablesAgingDocumentWise (screen 873): the same three lists; the GL link's From date is
     *  clsGlobalVariables.ActiveYr.Start_Period = the session's chosen year (yearId), not "exactly one active year". */
    public Map<String,Object> supplierLookups(UserAccount u,int yearId) {
        return lookups(u,"3,8","3",yearId);
    }
    /** frmPayablesAgingDocumentWise.btnRefresh_Click: BindCustomerAccountCombo(CustomerAccountData()) only -
     *  CommonServices.CoaAllocationAccountTitleByAccountTypeIds("3,8", "", 0, "3"). */
    public List<Map<String,Object>> supplierAccounts(UserAccount u) {
        return accounts(u,"3,8","3");
    }
    /** frmReceivableAgingDocumentWise (screen 872): CustomerAccountData() = CoaAllocationAccountTitleByAccountTypeIds("3", "", 0, "2"),
     *  CustomeGroupsDefine(1), CustomerGroup.GetSupplierCustomerGroupFromInventoryStockEvaluation (the same activity as 873);
     *  the GL link's From date = clsGlobalVariables.ActiveYr.Start_Period = the session's chosen year. */
    public Map<String,Object> customerLookups(UserAccount u,int yearId) {
        return lookups(u,"3","2",yearId);
    }
    /** frmReceivableAgingDocumentWise.btnRefresh_Click: BindCustomerAccountCombo(CustomerAccountData()) only. */
    public List<Map<String,Object>> customerAccounts(UserAccount u) {
        return accounts(u,"3","2");
    }
    private List<Map<String,Object>> accounts(UserAccount u,String types,String classes) {
        return jdbc.queryForList("EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @AppId=?, @UserId=?, @AccountTypeIds=?, @AccountClassIds=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),u.getAppId(),u.getId(),types,classes,"GetAccountTitleByAccountTypeIds");
    }
    private Map<String,Object> lookups(UserAccount u,String types,String classes,int yearId) {
        Map<String,Object> data=new LinkedHashMap<>();
        data.put("amountDecimals",ReportValueSupport.amountDecimals(jdbc,u));
        data.put("accounts",accounts(u,types,classes));
        data.put("customGroups",jdbc.queryForList("EXEC dbo.Sp_AcLookUps_GetAllMethod @OrganizationId=?, @CompanyId=?, @AcLookUpTypesId=1, @Activity='ReadAll'",u.getOrganizationId(),u.getCompanyId()));
        data.put("partyGroups",jdbc.queryForList("EXEC dbo.Sp_CustomerGroup_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity='GetSupplierCustomerGroupFromInventoryStockEvaluation'",u.getOrganizationId(),u.getCompanyId()));
        List<Map<String,Object>> years=jdbc.queryForList("EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",u.getOrganizationId(),u.getCompanyId());
        Map<String,Object> year=null;
        if(yearId>0) for(Map<String,Object> y:years) { Object id=y.get("Id"); if(id instanceof Number && ((Number)id).intValue()==yearId) { year=y; break; } }
        if(yearId>0 && year==null) year=new LinkedHashMap<>(Map.of("Id",yearId));   // the lists still bind; the GL link then opens without a From date
        if(yearId<=0) {
            if(years.size()!=1) throw new IllegalStateException("Select one active financial year for ledger navigation");
            year=years.get(0);
        }
        if(yearId>0) {   // Start_Period as the local yyyy-MM-dd (a datetime would serialize as a UTC instant and can shift a day)
            Object start=null;
            for(Map.Entry<String,Object> e:year.entrySet()) if("Start_Period".equalsIgnoreCase(e.getKey())) start=e.getValue();
            year=new LinkedHashMap<>(year);
            year.keySet().removeIf(k->"Start_Period".equalsIgnoreCase(k));
            if(start instanceof java.sql.Timestamp) year.put("Start_Period",((java.sql.Timestamp)start).toLocalDateTime().toLocalDate().toString());
            else if(start instanceof java.util.Date) year.put("Start_Period",new java.sql.Date(((java.util.Date)start).getTime()).toLocalDate().toString());
            else if(start instanceof java.time.temporal.TemporalAccessor) year.put("Start_Period",String.valueOf(start).substring(0,10));
        }
        data.put("year",year);
        return data;
    }
    private static void add(StringBuilder sql,List<Object> p,String name,int value) { if(value!=0) {sql.append(", @").append(name).append("=?");p.add(value);} }
}
