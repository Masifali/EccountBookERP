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
        return load(report, u, r, null);
    }
    /** R4 2026-09-30: financialYearId = clsGlobalVariables.ActiveYr.Id (the session's year, CurrentUserContext). */
    public List<Map<String,Object>> load(String report, UserAccount u, DesktopReceivablesRequest r, Integer financialYearId) {
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
                if(financialYearId!=null) p.put("FinancialYearId",financialYearId); else {
                List<Map<String,Object>> years=jdbc.queryForList("EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",u.getOrganizationId(),u.getCompanyId());
                if(years.size()!=1) throw new IllegalStateException("Select a single active financial year");
                p.put("FinancialYearId",years.get(0).get("Id")); } if(!report.equals("payables-report")) p.put("AccountType",3);
                add(p,report.equals("payables-report")?"ControlAccountIds":"ParentAccountCode",r.getControls()); add(p,"CustomerGroupIds",r.getGroups());
                add(p,"BranchesIds",r.getBranches()); add(p,"CityId",r.getCityId());
                if(report.equals("payables-report")) {
                    /* Payables.cs btnshow_Click: BalanceFrom/To = Conversion.ToDouble(text); "if (SkipZero.Checked) vh.ActionId = 1"
                       and "if (ChkTradeParties.Checked) vh.ActionId = 1" - PayablesReport sends ActionId as @ShowOnlyTrade;
                       vh.SkipZero is never set, so @SkipZero is never sent. */
                    add(p,"BalanceFrom",r.getBalanceFrom()); add(p,"BalanceTo",r.getBalanceTo());
                    add(p,"ShowOnlyTrade",(r.isTradeOnly()||r.isSkipZero())?1:0);
                } else {
                    /* Receivables.cs DataFill: FromDocNo/ToDocNo = Conversion.ToInt(txtBalanceFrom/To.Text) - Convert.ToInt32 of a
                       text with a decimal point throws and becomes 0 (not sent); ChkShowOnlyTrade -> ActionId -> @ShowOnlyTrade,
                       SkipZero -> @SkipZero. */
                    add(p,"BalanceFrom",wholeOrZero(r.getBalanceFrom())); add(p,"BalanceTo",wholeOrZero(r.getBalanceTo()));
                    add(p,"ShowOnlyTrade",r.isTradeOnly()?1:0); add(p,"SkipZero",r.isSkipZero()?1:0);
                }
                add(p,"ShowAssetLiability",r.getShowAssetLiability());
                break;
            default: throw new IllegalArgumentException("Unknown receivables report");
        }
        StringJoiner sql=new StringJoiner(", ","EXEC dbo."+procedure+" ","");
        p.keySet().forEach(key->sql.add("@"+key+"=?"));
        return ReportValueSupport.decimalStrings(jdbc.queryForList(sql.toString(),p.values().toArray()));
    }
    /** Conversion.ToInt(text): Convert.ToInt32 of a whole number, 0 when the text carries a decimal point or overflows. */
    private static int wholeOrZero(java.math.BigDecimal v) {
        if(v==null || v.scale()>0) return 0;
        try { return v.intValueExact(); } catch(ArithmeticException e) { return 0; }
    }
    private static void add(Map<String,Object> p,String key,Object value) {
        if(value==null || value.toString().isBlank()) return;
        if(value instanceof Number && new java.math.BigDecimal(value.toString()).signum()==0) return;
        p.put(key,value instanceof LocalDate ? Date.valueOf((LocalDate)value) : value);
    }
}
