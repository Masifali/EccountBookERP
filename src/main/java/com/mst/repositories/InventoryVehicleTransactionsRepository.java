package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryVehicleTransactionsRequest;
import java.sql.Date;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/** TransactionReportVehicleWise -> InventoryStockEvalautionDetail.GetStockEvalautionDetailByRefIds. */
@Repository
public class InventoryVehicleTransactionsRepository {
    private final JdbcTemplate jdbc;
    public InventoryVehicleTransactionsRepository(JdbcTemplate jdbc){this.jdbc=jdbc;}
    public Map<String,Object> lookups(UserAccount user){
        Map<String,Object> result=new LinkedHashMap<>();
        result.put("stock",jdbc.queryForList("EXEC dbo.Sp_Inventory_InventoryTransactions_DropDownAndLists @OrganizationId=?, @CompanyId=?",user.getOrganizationId(),user.getCompanyId()));
        result.put("packingTypes",jdbc.queryForList("EXEC dbo.Sp_InvPackingType_GetAllMethod @Activity=?","ReadAll"));
        result.put("documentTypes",jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",user.getOrganizationId(),user.getCompanyId(),"GetDocumentTypeFromInventoryStocksEvaluations"));
        result.put("financialYears",jdbc.queryForList("EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",user.getOrganizationId(),user.getCompanyId()));
        return result;
    }
    public List<Map<String,Object>> uoms(UserAccount user,int itemId){
        return ReportValueSupport.decimalStrings(jdbc.queryForList("EXEC dbo.Sp_UOMSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemId=?, @Activity=?",user.getOrganizationId(),user.getCompanyId(),itemId,"ReadByItemID"));
    }
    public List<Map<String,Object>> load(UserAccount user,InventoryVehicleTransactionsRequest r){
        // Omit optional parameters exactly as the BLL does. Never ask JDBC to infer a NULL's type
        // from positional procedure metadata: the desktop passes these parameters by name.
        StringBuilder sql=new StringBuilder("EXEC dbo.USP_GetStockEvalautionDetailByRefRefIds @OrganizationId=?, @CompanyId=?, @DateFrom=?, @DateTo=?");
        List<Object> args=new ArrayList<>(List.of(user.getOrganizationId(),user.getCompanyId(),Date.valueOf(r.getFromDate()),Date.valueOf(r.getToDate())));
        add(sql,args,"InventoryParentCategories",optional(r.getParentCategoryId()));add(sql,args,"ItemId",optional(r.getItemId()));
        add(sql,args,"SupplierCustomerId",optional(r.getSupplierCustomerId()));add(sql,args,"WarehouseId",optional(r.getWarehouseId()));
        add(sql,args,"JobLotId",optional(r.getJobLotId()));add(sql,args,"CropYear",blank(r.getCropYear()));
        add(sql,args,"ReferenceDocumentTypeId",desktopDocumentType(r.getDocumentTypeIds()));add(sql,args,"PackingTypeId",optional(r.getPackingTypeId()));add(sql,args,"ItemUomId",optional(r.getItemUomId()));
        return ReportValueSupport.decimalStrings(jdbc.queryForList(sql.toString(),args.toArray()));
    }
    private static void add(StringBuilder sql,List<Object> args,String name,Object value){if(value!=null){sql.append(", @").append(name).append("=?");args.add(value);}}
    private static Integer optional(Integer value){return value==null||value==0?null:value;}
    private static String blank(String value){return value==null||value.isEmpty()?null:value;}
    /** C# Conversion.ToInt returns zero when the checked-list value cannot convert to Int32. */
    public static Integer desktopDocumentType(String value){
        if(value==null||value.isBlank())return null;
        try{return optional(Integer.valueOf(value));}catch(NumberFormatException ex){return null;}
    }
}
