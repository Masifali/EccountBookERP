package com.mst.repositories;

import com.mst.models.SaleActivitiesReportFilter;
import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import com.mst.repositories.support.SaleActivitiesQuery;
import java.sql.Date;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/** Native BLL 0574 InventoryStockEvalautionDetail:1496/2862 and BLL 0017 branch allocations:85. */
@Repository
public class SaleActivitiesReportRepository {
    private final JdbcTemplate jdbc;
    public SaleActivitiesReportRepository(JdbcTemplate jdbc) { this.jdbc=jdbc; }
    public int screenId() {
        // Seeder name differs from form class: screen 481, Sales Reports module 53.
        var rows=jdbc.queryForList("SELECT Id FROM dbo.ScreenDefinition WHERE ModuleId=53 AND ScreenName=? AND TargetUrl=?",
                "SaleInvoiceRegisterNew","Architecture.WinApp.Inventory_Reports.frmEvaulationDetailSalesReports");
        if(rows.size()!=1)throw new IllegalStateException("Sale Invoice Report With Activities screen definition was not found uniquely");
        return ((Number)rows.get(0).get("Id")).intValue();
    }
    public List<Map<String,Object>> branches(UserAccount u) {
        var p=tenant(u);p.put("UserId",u.getId());return DesktopProc.rows(jdbc,"USP_GetBranchsAllocatedToUser",p);
    }
    public List<Map<String,Object>> lookups(UserAccount u,String branches) {
        var p=tenant(u);p.put("AppId",Objects.requireNonNullElse(u.getAppId(),0));p.put("UserId",u.getId());p.put("DocType","Sale");p.put("BranchesIds",branches);
        // Native Activity is unset; ADO.NET omits the null parameter and SQL returns every lookup category.
        return SaleActivitiesQuery.lookups(jdbc,p);
    }
    public String yearStart(UserAccount u,int year) {
        return DesktopProc.rows(jdbc,"Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId",tenant(u)).stream()
                .filter(r->((Number)r.get("Id")).intValue()==year).map(r->Objects.toString(r.get("Start_Period"),""))
                .filter(s->s.length()>=10).map(s->s.substring(0,10)).findFirst().orElse("");
    }
    public boolean costingByJobOrder(UserAccount u) {
        var p=tenant(u);p.put("ConfigDescription","SaleCostingJobOrderWise");p.put("Activity","GetConfigurationByOrgCompandConfigDescription");
        var rows=DesktopProc.rows(jdbc,"Sp_ConfigrationsAllocation_GetAllMethod",p);
        String value=rows.isEmpty()?"":Objects.toString(rows.get(0).get("ConfigKey"),"").trim();
        return "true".equalsIgnoreCase(value)||"1".equals(value);
    }
    public List<Map<String,Object>> rows(UserAccount u,SaleActivitiesReportFilter f,String branches,String groups) {
        var p=tenant(u);p.put("AppId",Objects.requireNonNullElse(u.getAppId(),0));p.put("UserId",u.getId());
        p.put("FromDate",Date.valueOf(f.fromDate()));p.put("ToDate",Date.valueOf(f.toDate()));
        p.put("ActivityName",f.activity());p.put("BranchesIds",branches);
        if(groups!=null&&!groups.isEmpty())p.put("CustomGroupIds",groups);
        number(p,"DocNoFrom",f.fromNo());number(p,"DocNoTo",f.toNo());
        number(p,"WarehouseId",f.warehouseId());number(p,"ItemId",f.itemId());
        number(p,"InventoryParentCategories",f.parentCategoryId());number(p,"ItemCategoryId",f.categoryId());
        number(p,"ItemTypeId",f.itemTypeId());number(p,"ItemClassGroupId",f.itemClassId());
        number(p,"SupplierCustomerId",f.customerId());number(p,"PackUom",f.packUom());
        number(p,"JobLotId",f.jobLotId());number(p,"RefPartyId",f.referencePartyId());
        number(p,"DistrictId",f.districtId());number(p,"CityId",f.cityId());
        number(p,"OtherCategoryId",f.otherCategoryId());number(p,"SaleAccountId",f.saleAccountId());
        number(p,"StockAccountId",f.stockAccountId());number(p,"CGSAccountId",f.cgsAccountId());
        if(f.cropYear()!=null&&!f.cropYear().isEmpty())p.put("CropYear",f.cropYear());
        // The form does not assign CostCenterId, AccountId, single CustomGroupId or FinancialYearId.
        return SaleActivitiesQuery.rows(jdbc,p);
    }
    private static void number(Map<String,Object> p,String key,int value) { if(value!=0)p.put(key,value); }
    private static Map<String,Object> tenant(UserAccount u) { return DesktopProc.params("OrganizationId",u.getOrganizationId(),"CompanyId",u.getCompanyId()); }
}
