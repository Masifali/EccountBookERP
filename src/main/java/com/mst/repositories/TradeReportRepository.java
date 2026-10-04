package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.TradeReportRequest;
import java.sql.Date;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/** Desktop VoucherReports.cs:2044-2233. Parameter spelling is part of the SQL contract. */
@Repository
public class TradeReportRepository {
    private final JdbcTemplate jdbc;
    public TradeReportRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public List<Map<String,Object>> load(UserAccount u, TradeReportRequest r) {
        LinkedHashMap<String,Object> p = context(u);
        p.put("AppId", u.getAppId()); p.put("CompanyIdCoa", u.getCompanyId());
        optional(p, "AccouuntClassId", r.isBalanceClassification() ? r.getBalanceClass() : r.getAccountClass());
        p.put("FromDate", Date.valueOf(r.getFromDate())); p.put("ToDate", Date.valueOf(r.getToDate()));
        optional(p,"BalanceFrom",r.getBalanceFrom()); optional(p,"BalanceTo",r.getBalanceTo());
        optional(p,"CityId",r.getCityId()); optional(p,"ParentAccountCode",r.getControlAccounts());
        optional(p,"LanguageId",r.getLanguageId()); optional(p,"CustomGroupId",r.getCustomGroupId());
        optional(p,"CustomerGroupIds",r.getInventoryGroups()); optional(p,"BranchesIds",r.getBranches());
        optional(p,"ActionId",r.isCredit() == r.isDebit() ? 0 : r.isCredit() ? 1 : 2);
        if(r.isApproved()) p.put("IsApproved",true);
        if(r.isTradeOnly()) p.put("TradeTypeId",r.getAccountClass()==3 ? 1 : 2);
        if(r.isBalanceClassification()) { p.put("ReprotTypeId",1); p.put("ReportType",r.getAccountClass()==3 ? "PAYABLES" : "RECEIVABLES"); }
        optional(p,"CoaDetailAccountId",r.getAccountId()); optional(p,"SubsidoryAccountId",r.getSubsidiaryId());
        p.put("UserId",u.getId()); optional(p,"CostCenterId",r.getCostCenterId());
        return ReportValueSupport.decimalStrings(execute("SpAccounts_TradeDebtorsAndCreditors_Report",p));
    }

    public Map<String,Object> lookups(UserAccount u) {
        Map<String,Object> result = new LinkedHashMap<>();
        List<Map<String,Object>> decimalConfig = execute("Sp_ConfigrationsAllocation_GetAllMethod",
                params(u,"ConfigDescription","Default NoofDecimal Points For Amount",
                        "Activity","GetConfigurationByOrgCompandConfigDescription"));
        int amountDecimals = 0;
        if (!decimalConfig.isEmpty() && decimalConfig.get(0).get("ConfigKey") != null) {
            String configured = decimalConfig.get(0).get("ConfigKey").toString().trim();
            if (!configured.isEmpty()) amountDecimals = Integer.parseInt(configured);
        }
        // CommonServices.GetDecimalConfiguration uses fixed places only for 1 through 4.
        result.put("amountDecimals", amountDecimals >= 1 && amountDecimals <= 4 ? amountDecimals : 0);
        List<Map<String,Object>> years=execute("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId",context(u));
        if(years.size()!=1) throw new IllegalStateException("Select a single active financial year before opening this report");
        Map<String,Object> year = new LinkedHashMap<>(years.get(0));
        // These are calendar dates. Serializing a midnight SQL timestamp in UTC shifts
        // 1-Aug to 31-Jul before the browser takes the date portion of the value.
        for (String column : List.of("Start_Period", "End_Period")) {
            Object date = year.get(column);
            if (date instanceof java.sql.Timestamp timestamp) year.put(column, timestamp.toLocalDateTime().toLocalDate().toString());
            else if (date instanceof java.sql.Date sqlDate) year.put(column, sqlDate.toLocalDate().toString());
        }
        result.put("year",year);
        result.put("branchId",u.getBranchesId()); result.put("appId",u.getAppId());
        result.put("features",execute("USP_GetERPFeaturesByCompanyId",context(u)));
        result.put("cities",execute("SP_City_GetAllMethod",params(u,"MethodType","GetAll")));
        result.put("customGroups",execute("Sp_AcLookUps_GetAllMethod",params(u,"AcLookUpTypesId",1,"Activity","ReadAll")));
        result.put("inventoryGroups",execute("Sp_CustomerGroup_GetAllMethod",params(u,"Activity","GetSupplierCustomerGroupFromInventoryStockEvaluation")));
        result.put("partyGroups",execute("Sp_CustomerGroup_GetAllMethod",params(u,"Activity","ReadAll")));
        List<Map<String,Object>> branches = new ArrayList<>();
        try { branches = execute("USP_GetBranchesFromVouchersByAccountId",context(u)); } catch (Exception ignored) {}
        /* BranchesFill(): VoucherHead.GetBranchesFromVouchersByAccountId(org, company, "", 0) only.
           The desktop has no second or third source; an empty result leaves the combo empty. */
        for (Map<String, Object> map : branches) {
            Object id = map.get("Id") != null ? map.get("Id") : (map.get("id") != null ? map.get("id") : (map.get("BranchId") != null ? map.get("BranchId") : map.get("branchId")));
            Object name = map.get("BranchName") != null ? map.get("BranchName") : (map.get("branchName") != null ? map.get("branchName") : (map.get("Name") != null ? map.get("Name") : map.get("name")));
            map.put("Id", id); map.put("id", id); map.put("ID", id); map.put("BranchId", id); map.put("branchId", id);
            map.put("BranchName", name); map.put("branchName", name); map.put("Name", name); map.put("name", name);
            /* R4 2026-09-30: map is a LinkedCaseInsensitiveMap - every alias put above REPLACED the stored key spelling
               (the JSON carried "ID"/"branchId"/"branchName"/"name" only, so the page's r.Id / r.BranchName were undefined and the Branches
               combo showed blank rows). Re-put the proc's own spelling (USP_GetBranchesFromVouchersByAccountId: Id, BranchName). */
            map.put("Id", id); map.put("BranchName", name);
        }
        result.put("branches", branches);
        result.put("controls",execute("Sp_ChartofAccount_GetAllMethodFromCOA",params(u,"FinancialYearId",years.get(0).get("Id"),"Account_Level",3,"AccountTypeId",3,"AccountClassIds","2,3","CoaType","ReadAllAccountGroup")));
        result.put("accounts",accounts(u,0));
        result.put("languages",execute("Sp_MultiLanguages_GetAll",params(u,"MethodType","ReadAll")));
        result.put("costCenters",execute("usp_getCostCenters",params(u,"UserId",u.getId(),"AppId",u.getAppId())));
        return result;
    }

    public List<Map<String,Object>> accounts(UserAccount u,int costCenter) {
        LinkedHashMap<String,Object> p=params(u,"AppId",u.getAppId(),"UserId",u.getId(),"AccountTypeIds","3,6,8,16,17,18","AccountClassIds","2,3","Activity","GetAccountTitleByAccountTypeIds");
        optional(p,"CostCenterId",costCenter);
        return execute("Sp_COAAllocation_GetAllMethod",p);
    }
    private static LinkedHashMap<String,Object> context(UserAccount u) { return params(u,new Object[0]); }
    private static LinkedHashMap<String,Object> params(UserAccount u,Object... pairs) {
        LinkedHashMap<String,Object> p=new LinkedHashMap<>();
        p.put("OrganizationId",u.getOrganizationId()); p.put("CompanyId",u.getCompanyId());
        for(int i=0;i<pairs.length;i+=2) p.put((String)pairs[i],pairs[i+1]);
        return p;
    }
    private static void optional(Map<String,Object> p,String key,Object value) {
        if(value==null || value.toString().isBlank()) return;
        if(value instanceof Number && new java.math.BigDecimal(value.toString()).signum()==0) return;
        p.put(key,value);
    }
    private List<Map<String,Object>> execute(String procedure,LinkedHashMap<String,Object> p) {
        StringJoiner sql=new StringJoiner(", ","EXEC dbo."+procedure+" ","");
        p.keySet().forEach(k->sql.add("@"+k+"=?"));
        return jdbc.queryForList(sql.toString(),p.values().toArray());
    }
}
