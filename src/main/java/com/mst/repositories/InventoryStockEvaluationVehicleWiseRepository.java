package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryStockEvaluationVehicleWiseHoldRequest;
import com.mst.models.dto.InventoryStockEvaluationVehicleWiseRequest;
import java.sql.Date;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/**
 * Desktop StockEvaluationReportVehicleWise (screen 292).
 *   StockComboFill      StocksReport.StockReportComboByOrganizationAndCompanyId -> Sp_Inventory_InventoryTransactions_DropDownAndLists @OrganizationId @CompanyId
 *   PackingTypeFill     InvPackingType.Getall                                   -> Sp_InvPackingType_GetAllMethod @Activity='ReadAll'
 *   RefDocumentTypeFill Item.GetDocumentTypeFromInventoryStocksEvaluations      -> Sp_Item_GetAllMethod 'GetDocumentTypeFromInventoryStocksEvaluations'
 *   UOMFill             UOMSchedule.SearchByObject                              -> Sp_UOMSchedule_GetAllMethod 'ReadByItemID'
 *   Show                InventoryStockEvalautionDetail.StockEvalaution_VehicleWiseTransaction_Report -> USP_StockEvalaution_VehicleWiseTransaction_Report
 *   TranDetail          InventoryStockEvalautionDetail.GetTransactionDetailAgainstVehicle -> USP_GetTransactionDetailAgainstVehicle
 *   Stock Hold          labIPmActivityLog.StockHoldData                         -> usp_StockHoldForFumigation_UpdateStatus @StockHoldData (TVP_StockHoldForFumigation)
 *   Feature 24          CommonServices.GetERPFeatureById(24)                    -> USP_GetERPFeaturesByCompanyId
 *   Rights              CommonServices.SetRightsValueInRightsObject             -> Sp_tblUserRights_GetAllMethod 'GetByUserId'
 */
@Repository
public class InventoryStockEvaluationVehicleWiseRepository {
    public static final String SCREEN_NAME="StockEvaluationReportVehicleWise";
    private final JdbcTemplate jdbc;
    public InventoryStockEvaluationVehicleWiseRepository(JdbcTemplate jdbc){this.jdbc=jdbc;}

    public Map<String,Object> lookups(UserAccount user){
        Map<String,Object> result=new LinkedHashMap<>();
        result.put("stock",jdbc.queryForList("EXEC dbo.Sp_Inventory_InventoryTransactions_DropDownAndLists @OrganizationId=?, @CompanyId=?",user.getOrganizationId(),user.getCompanyId()));
        result.put("packingTypes",jdbc.queryForList("EXEC dbo.Sp_InvPackingType_GetAllMethod @Activity=?","ReadAll"));
        result.put("documentTypes",jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",user.getOrganizationId(),user.getCompanyId(),"GetDocumentTypeFromInventoryStocksEvaluations"));
        return result;
    }

    public List<Map<String,Object>> uoms(UserAccount user,int itemId){
        return ReportValueSupport.decimalStrings(jdbc.queryForList("EXEC dbo.Sp_UOMSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemId=?, @Activity=?",user.getOrganizationId(),user.getCompanyId(),itemId,"ReadByItemID"));
    }

    /** clsGlobalVariables.ActiveYr.Start_Period: the login year, else the first active year. */
    public Object financialYearStart(UserAccount user,int financialYearId){
        List<Map<String,Object>> years=jdbc.queryForList("EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",user.getOrganizationId(),user.getCompanyId());
        Map<String,Object> row=null;
        for(Map<String,Object> r:years){Object id=r.get("Id");if(id instanceof Number&&((Number)id).intValue()==financialYearId){row=r;break;}}
        if(row==null&&!years.isEmpty())row=years.get(0);
        return row==null?null:row.get("Start_Period");
    }

    /** CommonServices.GetERPFeatureById(24) - StockReleaseFromFumigation. */
    public boolean feature(UserAccount user,int featureId){
        for(Map<String,Object> r:jdbc.queryForList("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?, @CompanyId=?",user.getOrganizationId(),user.getCompanyId())){
            Object id=r.get("Id");if(id instanceof Number&&((Number)id).intValue()==featureId)return true;
        }
        return false;
    }

    /** tblUserRights.GetByUserId(UserId, ScreenName=base.Name, RightName=RoleName). */
    public List<Map<String,Object>> userRights(UserAccount user,String roleName){
        return jdbc.queryForList("EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?, @ScreenName=?, @RightName=?, @CompanyId=?, @Activity=?",
                user.getId(),SCREEN_NAME,roleName==null?"":roleName,user.getCompanyId(),"GetByUserId");
    }

    /** Desktop default numeric formats: clsGlobalVariables.stringFormatsingle / DecimalRateFormate. */
    public int amountDecimals(UserAccount user){return ReportValueSupport.amountDecimals(jdbc,user);}
    public int rateDecimals(UserAccount user){
        List<Map<String,Object>> rows=jdbc.queryForList("EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @ConfigDescription=?, @Activity=?",
                user.getOrganizationId(),user.getCompanyId(),"Default NoofDecimal Points For Rate","GetConfigurationByOrgCompandConfigDescription");
        int places=0;
        if(!rows.isEmpty()&&rows.get(0).get("ConfigKey")!=null){
            try{places=Integer.parseInt(rows.get(0).get("ConfigKey").toString().trim());}catch(NumberFormatException ex){places=0;}
        }
        return places>=1&&places<=4?places:2;
    }

    public List<Map<String,Object>> load(UserAccount user,InventoryStockEvaluationVehicleWiseRequest r){
        // Parameters are omitted exactly when the BLL omits them (zero ids, empty strings).
        StringBuilder sql=new StringBuilder("EXEC dbo.USP_StockEvalaution_VehicleWiseTransaction_Report @OrganizationId=?, @CompanyId=?");
        List<Object> args=new ArrayList<>();args.add(user.getOrganizationId());args.add(user.getCompanyId());
        add(sql,args,"DateFrom",r.getFromDate()==null?null:Date.valueOf(r.getFromDate()));
        add(sql,args,"DateTo",r.getToDate()==null?null:Date.valueOf(r.getToDate()));
        add(sql,args,"ClassGroupId",optional(r.getClassGroupId()));
        add(sql,args,"ItemId",optional(r.getItemId()));
        add(sql,args,"SupplierCustomerId",optional(r.getSupplierCustomerId()));
        add(sql,args,"WarehouseId",optional(r.getWarehouseId()));
        add(sql,args,"JobLotId",optional(r.getJobLotId()));
        add(sql,args,"CropYear",blank(r.getCropYear()));
        add(sql,args,"PackingTypeId",optional(r.getPackingTypeId()));
        add(sql,args,"ItemUomId",optional(r.getItemUomId()));
        add(sql,args,"WeightFrom",Boolean.TRUE.equals(r.getSkipZero())?Integer.valueOf(1):null);
        add(sql,args,"RefDocumentTypeIds",blank(r.getRefDocumentTypeIds()));
        add(sql,args,"ParentCategoryIds",blank(r.getParentCategoryIds()));
        add(sql,args,"ItemCategoryIds",blank(r.getItemCategoryIds()));
        add(sql,args,"ItemTypeIds",blank(r.getItemTypeIds()));
        return ReportValueSupport.decimalStrings(jdbc.queryForList(sql.toString(),args.toArray()));
    }

    public List<Map<String,Object>> transactionDetail(UserAccount user,int refDocumentTypeId,int refDocIdNo,int refDocSubIdNo){
        return ReportValueSupport.decimalStrings(jdbc.queryForList("EXEC dbo.USP_GetTransactionDetailAgainstVehicle @OrganizationId=?, @CompanyId=?, @RefRefDocumentTypeId=?, @RefRefDocIdNo=?, @RefRefDocSubIdNo=?",
                user.getOrganizationId(),user.getCompanyId(),refDocumentTypeId,refDocIdNo,refDocSubIdNo));
    }

    /**
     * labIPmActivityLog.StockHoldData: one TVP row per checked grid row, columns in the desktop DataTable's
     * order (OrganizationId, CompanyId, DocumentTypeId, Id, DetailId, UserId, ReqType, DateForUpdate, Remarks),
     * inside one transaction that rolls back on any error.
     */
    public void stockHold(UserAccount user,List<InventoryStockEvaluationVehicleWiseHoldRequest.Row> rows,String remarks){
        // Ids are typed integers written as literals (no SQL text from the client), so any number of
        // checked rows stays under the 2100-parameter limit; the remarks text is the only parameter.
        StringBuilder sql=new StringBuilder("SET NOCOUNT ON; SET XACT_ABORT ON; DECLARE @Remarks nvarchar(max)=?; BEGIN TRAN; DECLARE @StockHoldData dbo.TVP_StockHoldForFumigation; ");
        int org=zero(user.getOrganizationId()),company=zero(user.getCompanyId()),userId=zero(user.getId());
        for(InventoryStockEvaluationVehicleWiseHoldRequest.Row row:rows){
            sql.append("INSERT INTO @StockHoldData VALUES (").append(org).append(',').append(company).append(',')
               .append(zero(row.getRefDocumentTypeId())).append(',').append(zero(row.getRefDocIdNo())).append(',').append(zero(row.getRefDocSubIdNo())).append(',')
               .append(userId).append(",N'StockHold',GETDATE(),@Remarks); ");
        }
        sql.append("EXEC dbo.usp_StockHoldForFumigation_UpdateStatus @StockHoldData=@StockHoldData; COMMIT TRAN;");
        jdbc.update(sql.toString(),remarks);
    }

    private static void add(StringBuilder sql,List<Object> args,String name,Object value){if(value!=null){sql.append(", @").append(name).append("=?");args.add(value);}}
    private static Integer optional(Integer value){return value==null||value==0?null:value;}
    private static int zero(Integer value){return value==null?0:value;}
    private static String blank(String value){return value==null||value.isEmpty()?null:value;}
}
