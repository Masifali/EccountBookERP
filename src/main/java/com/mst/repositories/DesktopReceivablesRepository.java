package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.DesktopReceivablesRequest;
import java.sql.Date;
import java.time.LocalDate;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/** Parameter contracts from desktop VoucherReports.cs:1707,1788,5523. */
@Repository
public class DesktopReceivablesRepository {
    private final JdbcTemplate jdbc;
    public DesktopReceivablesRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }
    public List<Map<String,Object>> load(String report, UserAccount u, DesktopReceivablesRequest r) {
        LinkedHashMap<String,Object> p = new LinkedHashMap<>();
        p.put("OrganizationId",u.getOrganizationId()); p.put("CompanyId",u.getCompanyId()); p.put("UserId",u.getId());
        add(p,"FromDate",r.getFromDate()); add(p,"ToDate",r.getToDate());
        add(p,"CustomGroupId",r.getCustomGroupId());
        String procedure;
        switch(report) {
            case "receivables-by-due-dates":
                procedure="Usp_ReceivablesByDueDates";
                p.put("AccouuntClassId",2); p.put("ActionId",1);
                add(p,"ParentAccountId",r.getParentId());
                break;
            case "receivables-receipt-schedule":
                procedure="usp_ReceivablesAndReceiptsSchedule";
                p.put("ParentCategoryId",r.getParentCategoryId());
                add(p,"DueDateFrom",r.getDueFrom()); add(p,"DueDateTo",r.getDueTo());
                add(p,"SaleFromDate",r.getSaleFrom()); add(p,"SaleToDate",r.getSaleTo());
                add(p,"PartyGroupId",r.getPartyGroupId()); add(p,"ControlAccountIds",r.getControls());
                add(p,"BalanceFrom",r.getBalanceFrom()); add(p,"BalanceTo",r.getBalanceTo());
                add(p,"BranchesIds",r.getBranches());
                break;
            case "payables-report":
            case "receivables-report":
                procedure=report.equals("payables-report")?"Sp_Accounts_Payables_Rpt":"Sp_Accounts_Receivables_Rpt";
                List<Map<String,Object>> years=jdbc.queryForList("EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",u.getOrganizationId(),u.getCompanyId());
                if(years.size()!=1) throw new IllegalStateException("Select a single active financial year");
                p.put("FinancialYearId",years.get(0).get("Id")); if(!report.equals("payables-report")) p.put("AccountType",3);
                add(p,report.equals("payables-report")?"ControlAccountIds":"ParentAccountCode",r.getControls()); add(p,"CustomerGroupIds",r.getGroups());
                add(p,"BranchesIds",r.getBranches()); add(p,"CityId",r.getCityId());
                add(p,"BalanceFrom",r.getBalanceFrom()); add(p,"BalanceTo",r.getBalanceTo());
                add(p,"ShowOnlyTrade",r.isTradeOnly()?1:0); add(p,"SkipZero",r.isSkipZero()?1:0);
                add(p,"ShowAssetLiability",r.getShowAssetLiability());
                break;
            default: throw new IllegalArgumentException("Unknown receivables report");
        }
        StringJoiner sql=new StringJoiner(", ","EXEC dbo."+procedure+" ","");
        p.keySet().forEach(key->sql.add("@"+key+"=?"));
        return ReportValueSupport.decimalStrings(jdbc.queryForList(sql.toString(),p.values().toArray()));
    }
    private static void add(Map<String,Object> p,String key,Object value) {
        if(value==null || value.toString().isBlank()) return;
        if(value instanceof Number && new java.math.BigDecimal(value.toString()).signum()==0) return;
        p.put(key,value instanceof LocalDate ? Date.valueOf((LocalDate)value) : value);
    }
}
