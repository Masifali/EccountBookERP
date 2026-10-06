package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryStockEvaluationVehicleWiseHoldRequest;
import com.mst.models.dto.InventoryStockEvaluationVehicleWiseRequest;
import com.mst.repositories.InventoryStockEvaluationVehicleWiseRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import java.util.*;
import org.springframework.stereotype.Service;

/** Desktop Architecture.WinApp.Inventory_Stocks_Report.StockEvaluationReportVehicleWise (ScreenDefinition 292). */
@Service
public class InventoryStockEvaluationVehicleWiseService {
    public static final int SCREEN_ID=292;
    /** dt4 columns added only when !formright.RateandAmountFieldAccessDenied. */
    private static final List<String> RATE_COLUMNS=List.of("RateUom","AVgRate","AmountIn","AmountOut","BalAmount");
    private final InventoryStockEvaluationVehicleWiseRepository repository;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;
    public InventoryStockEvaluationVehicleWiseService(InventoryStockEvaluationVehicleWiseRepository repository,CurrentUserContext context,DesktopReportRights rights){this.repository=repository;this.context=context;this.rights=rights;}
    private UserAccount user(){UserAccount user=context.requireAccountingUser();rights.require(user,SCREEN_ID,"View");return user;}

    /** Form Load / btnRefresh: StockComboFill, RefDocumentTypeFill, PackingTypeFill + the Load-time settings. */
    public Map<String,Object> lookups(){
        UserAccount u=user();
        Map<String,Object> result=repository.lookups(u);
        Object start=null;
        try{start=repository.financialYearStart(u,context.currentFinancialYearId());}catch(RuntimeException ex){start=null;}
        result.put("financialYearStart",start==null?null:String.valueOf(start).substring(0,Math.min(10,String.valueOf(start).length())));
        result.put("stockReleaseFromFumigation",repository.feature(u,24));
        result.put("rateAndAmountDenied",rateAndAmountDenied(u));
        result.put("amountDecimals",repository.amountDecimals(u));
        result.put("rateDecimals",repository.rateDecimals(u));
        return result;
    }

    public List<Map<String,Object>> uoms(int itemId){return repository.uoms(user(),itemId);}

    /** BtnSearch_Click -> StockEvalaution_VehicleWise(). */
    public Map<String,Object> load(InventoryStockEvaluationVehicleWiseRequest r){
        UserAccount u=user();
        if(r.getFromDate()==null||r.getToDate()==null)throw new IllegalArgumentException("Choose a valid From Date and To Date");
        for(String ids:new String[]{r.getRefDocumentTypeIds(),r.getParentCategoryIds(),r.getItemCategoryIds(),r.getItemTypeIds()}){
            // The desktop builds these as "id1,id2," (trailing comma kept); only digits and commas are accepted.
            if(ids!=null&&!ids.isEmpty()&&!ids.matches("[0-9,]+"))throw new IllegalArgumentException("Choose valid filter values");
        }
        boolean denied=rateAndAmountDenied(u);
        List<Map<String,Object>> rows=repository.load(u,r);
        Map<String,Object> result=new LinkedHashMap<>();
        result.put("rows",project(rows,denied));
        result.put("rateAndAmountDenied",denied);
        return result;
    }

    /** grd_ColumnButtonClick "TranDetail" -> StockTransactionDetail.StockEvalaution_VehicleWise(). */
    public List<Map<String,Object>> transactionDetail(int refDocumentTypeId,int refDocIdNo,int refDocSubIdNo){
        return repository.transactionDetail(user(),refDocumentTypeId,refDocIdNo,refDocSubIdNo);
    }

    /** btnStockHold_Click -> StockHold(); the button only exists when ERP feature 24 is on. */
    public Map<String,Object> stockHold(InventoryStockEvaluationVehicleWiseHoldRequest request){
        UserAccount u=user();
        if(!repository.feature(u,24))throw new IllegalStateException("Stock Hold For Fumigation is not enabled for this company");
        if(request==null||request.getRows()==null||request.getRows().isEmpty())throw new IllegalArgumentException("Please Check Rows first");
        String remarks=request.getRemarks();
        if(remarks==null||remarks.isEmpty())throw new IllegalArgumentException("Action Remarks Required");
        try{repository.stockHold(u,request.getRows(),remarks);}
        catch(org.springframework.dao.DataAccessException ex){
            Throwable cause=ex.getMostSpecificCause();
            throw new IllegalStateException(cause==null||cause.getMessage()==null?"Stock hold failed":cause.getMessage());
        }
        Map<String,Object> result=new LinkedHashMap<>();
        result.put("success",true);result.put("message","Record's Stock hold Successfully");
        return result;
    }

    /** formright.RateandAmountFieldAccessDenied: false for the Admin role, else the "RateandAmountFieldAccessDenied" row's Value. */
    private boolean rateAndAmountDenied(UserAccount u){
        String role=context.currentRoleName();
        if("Admin".equals(role))return false;
        boolean denied=false;
        try{
            for(Map<String,Object> row:repository.userRights(u,role)){
                Object name=row.get("RightName");
                if(name!=null&&"RateandAmountFieldAccessDenied".equals(name.toString()))denied=truthy(row.get("Value"));
            }
        }catch(RuntimeException ex){denied=false;}
        return denied;
    }
    private static boolean truthy(Object v){
        if(v instanceof Boolean)return (Boolean)v;
        if(v instanceof Number)return ((Number)v).intValue()!=0;
        return v!=null&&("true".equalsIgnoreCase(v.toString())||"1".equals(v.toString()));
    }

    /** dt4: the desktop copies these columns (Warehouse <- WareHouseCode, JobLot <- JobLotCode, AvgRate <- AVgRate). */
    public static List<Map<String,Object>> project(List<Map<String,Object>> source,boolean denied){
        String[] keys={"Id","RefDocumentTypeId","RefDocIdNo","RefDocSubIdNo","RefDocumentType","DocDate","DocNo","ManualNo","JobOrderNo","SupplierCustomerId","PartyName","VehicleNo","GpNo","Aflatoxins","WarehouseId","WareHouseCode","ItemId","ItemName","CropBatch","JobLotId","JobLotCode","InvPackingTypeId","PackingType","ItemUomId","PackUom","QtyIn","QtyOut","BalQty","WeightIn","WeightOut","BalWeight","RateUomId","RateUom","AVgRate","AmountIn","AmountOut","BalAmount","BiltyNo","StepDescription"};
        List<Map<String,Object>> result=new ArrayList<>();
        for(Map<String,Object> row:source){
            Map<String,Object> mapped=new LinkedHashMap<>();
            for(String key:keys){
                if(denied&&RATE_COLUMNS.contains(key))continue;
                String target="WareHouseCode".equals(key)?"Warehouse":"JobLotCode".equals(key)?"JobLot":"AVgRate".equals(key)?"AvgRate":key;
                mapped.put(target,row.get(key));
            }
            result.add(mapped);
        }
        return result;
    }
}
