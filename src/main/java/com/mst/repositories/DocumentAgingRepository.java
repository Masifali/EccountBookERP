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
        return lookups(u,"3,8","3");
    }
    public Map<String,Object> customerLookups(UserAccount u) {
        return lookups(u,"3","2");
    }
    private Map<String,Object> lookups(UserAccount u,String types,String classes) {
        Map<String,Object> data=new LinkedHashMap<>();
        data.put("amountDecimals",ReportValueSupport.amountDecimals(jdbc,u));
        data.put("accounts",jdbc.queryForList("EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @AppId=?, @UserId=?, @AccountTypeIds=?, @AccountClassIds=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),u.getAppId(),u.getId(),types,classes,"GetAccountTitleByAccountTypeIds"));
        data.put("customGroups",jdbc.queryForList("EXEC dbo.Sp_AcLookUps_GetAllMethod @OrganizationId=?, @CompanyId=?, @AcLookUpTypesId=1, @Activity='ReadAll'",u.getOrganizationId(),u.getCompanyId()));
        data.put("partyGroups",jdbc.queryForList("EXEC dbo.Sp_CustomerGroup_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity='GetSupplierCustomerGroupFromInventoryStockEvaluation'",u.getOrganizationId(),u.getCompanyId()));
        List<Map<String,Object>> years=jdbc.queryForList("EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",u.getOrganizationId(),u.getCompanyId());
        if(years.size()!=1) throw new IllegalStateException("Select one active financial year for ledger navigation");
        data.put("year",years.get(0));
        return data;
    }
    private static void add(StringBuilder sql,List<Object> p,String name,int value) { if(value!=0) {sql.append(", @").append(name).append("=?");p.add(value);} }
}
