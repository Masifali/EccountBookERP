package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryTransactionsRequest;
import java.sql.Date;
import java.time.LocalDate;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/** Desktop InventoryStockEvalautionDetail.cs:4993; preserves optional procedure parameters. */
@Repository
public class InventoryTransactionsRepository {
    private static final org.slf4j.Logger LOG = org.slf4j.LoggerFactory.getLogger(InventoryTransactionsRepository.class);
    private final JdbcTemplate jdbc;
    public InventoryTransactionsRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public List<Map<String,Object>> load(UserAccount user, InventoryTransactionsRequest r) {
        Map<String,Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", user.getOrganizationId()); p.put("CompanyId", user.getCompanyId());
        add(p,"SupplierCustomerId",r.getSupplierCustomerId()); add(p,"DocumentTypeId",r.getDocumentTypeId());
        add(p,"ItemId",r.getItemId()); add(p,"DateFrom",r.getFromDate()); add(p,"DateTo",r.getToDate());
        add(p,"WarehouseId",r.getWarehouseId()); add(p,"JobLotId",r.getJobLotId());
        add(p,"ItemClassGroupId",r.getItemClassGroupId()); add(p,"InventoryParentCategoriesId",r.getParentCategoryId());
        add(p,"ItemCategoryId",r.getItemCategoryId()); add(p,"ItemTypeId",r.getItemTypeId());
        add(p,"ItemTypeIds",r.getItemTypeIds()); add(p,"WarehouseIds",r.getWarehouseIds());
        StringJoiner sql = new StringJoiner(", ","EXEC [pcc].[USP-InventoryStockTransactionsReport] ","");
        p.keySet().forEach(k -> sql.add("@"+k+"=?"));
        return ReportValueSupport.decimalStrings(jdbc.queryForList(sql.toString(),p.values().toArray()));
    }

    public Map<String,Object> lookups(UserAccount u) {
        Map<String,Object> result = new LinkedHashMap<>();
        List<Map<String,Object>> choices = new ArrayList<>();
        try {
            choices.addAll(jdbc.queryForList("EXEC dbo.Sp_Inventory_InventoryTransactions_DropDownAndLists @OrganizationId=?, @CompanyId=?",u.getOrganizationId(),u.getCompanyId()));
        } catch (Exception e) { LOG.warn("Sp_Inventory_InventoryTransactions_DropDownAndLists failed", e); }

        /* InventoryStockTransactionsReport.cs:236-339 binds only this procedure's rows; an empty
           activity leaves its picker empty. The former table fallbacks read every company's rows. */

        result.put("choices", choices);

        List<Map<String, Object>> documents = new ArrayList<>();
        try {
            documents = jdbc.queryForList("EXEC dbo.Sp_Vouchers_GetMethods @OrganizationId=?, @CompanyId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),"DocumentTypeGetFromInventoryTransactions");
        } catch (Exception e) { LOG.warn("Sp_Vouchers_GetMethods DocumentTypeGetFromInventoryTransactions failed", e); }
        result.put("documents", documents);

        List<Map<String,Object>> dates = new ArrayList<>();
        try {
            dates = jdbc.queryForList("EXEC dbo.usp_getInventoryStockAsOnDate @OrganizationId=?, @CompanyId=?",u.getOrganizationId(),u.getCompanyId());
        } catch (Exception e) { LOG.warn("usp_getInventoryStockAsOnDate failed", e); }
        if (!dates.isEmpty() && dates.get(0).get("AsOnDate") != null) {
            Object date=dates.get(0).get("AsOnDate");
            LocalDate day=date instanceof java.sql.Timestamp ? ((java.sql.Timestamp)date).toLocalDateTime().toLocalDate() : LocalDate.parse(date.toString().substring(0,10));
            result.put("fromDate",day.plusDays(1).toString());
        }

        List<Map<String, Object>> financialYears = new ArrayList<>();
        try {
            financialYears = jdbc.queryForList("EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",u.getOrganizationId(),u.getCompanyId());
        } catch (Exception e) { LOG.warn("inventory lookup failed", e); }
        result.put("financialYears", financialYears);
        return result;
    }

    private static void add(Map<String,Object> p,String key,Object value) {
        if(value==null || value.toString().isBlank() || value instanceof Integer && (Integer)value==0) return;
        p.put(key,value instanceof LocalDate ? Date.valueOf((LocalDate)value) : value);
    }
}
