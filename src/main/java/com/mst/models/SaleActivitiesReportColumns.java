package com.mst.models;

import java.util.*;

/** Projections from frmEvaulationDetailSalesReports.GridFill; financial calculations remain in SQL. */
public final class SaleActivitiesReportColumns {
    private SaleActivitiesReportColumns() { }
    public static final List<String> ACTIVITIES = List.of(
        "Sales Register",
        "Sales Summary By Item",
        "Sales Summary By Item & City",
        "Sales Summary By Item & Pack Size",
        "Sales Summary By Item & Warehouse",
        "Sales Summary By Item,Pack Size & City",
        "Sales Summary By Customer",
        "Sales Summary By Customer & Item",
        "Sales Summary By Customer & City",
        "Sales Summary By Customer,Item & City",
        "Sales Summary By Customer & ReferenceParty",
        "Sales Summary By Customer,Item & ReferenceParty",
        "Sales Summary By Customer & Pack Size",
        "Sales Summary By Parent Category",
        "Sales Summary By Parent Category & Item",
        "Sales Summary By Parent Category & Customer",
        "Sales Summary By ReferenceParty",
        "Sales Summary By ReferenceParty & City",
        "Sales Summary By ReferenceParty & Item",
        "Sales Summary By ReferenceParty,Item & Pack Size",
        "Sales Summary By ReferenceParty & Pack Size",
        "INVOICE WISE PROFITABLITY",
        "Sales Summary HsCode"
    );
    private static final Map<String,String> SPECS = Map.ofEntries(
        Map.entry("Sales Register", "Id:RefDocIdNo,DocumentTypeId:RefDocumentTypeId,BranchName:BranchName,DocType:DocumentTypeCode,SoNo:SoNo,GpNo:GpNoDcNo,GdnNo:GdnNo,BillDate:DocDate,BillNo:DocCodeNo,BranchSrNo:BranchSrNo,ManualBillNo:ReferenceNo,PartyName:Customer,WareHouseName:WarehouseName,ItemCode:ItemCode,ItemName:ItemName,HsCode:HsCode,PackUom:UOMCode,CropYear:CropBatch,JobLot:JobLotCode,PackingType:PackTypeCode,VehicleNo:VehicleNo,ItemQty:QtyOut,BillWeight:BillWeightOut,StockWeight:StockWeight,ItemRate:ItemRate,RateCut:RateCut,NetRate:$NetRate,ItemAmountWithoutExpense:ItemAmountWithoutExpense,AvgRate:AvgRate,AvgRate40Kg:AvgRate40Kg,Expenses:Expenses,Freight:Freight,Commission:Commission,Amount:AmountOut,PrctByAmount:PrcntOfTotal,PrctByWeight:PrcntOfTotalWeight,CityName:CityName,TicketNos:TicketNos"),
        Map.entry("Sales Summary By Item", "ItemCode:ItemCode,ItemName:ItemName,HsCode:HsCode,ItemQty:QtyOut,BillWeight:BillWeightOut,Amount:AmountOut,AvgRate:AvgRate40Kg,PrctByAmount:PrcntOfTotal,PrctByWeight:PrcntOfTotalWeight,CityName:CityName"),
        Map.entry("Sales Summary By Item & Pack Size", "ItemCode:ItemCode,ItemName:ItemName,PackUom:UOMCode,ItemQty:QtyOut,BillWeight:BillWeightOut,Amount:AmountOut,AvgRate:AvgRate,AvgRate40Kg:AvgRate40Kg,PrctByAmount:PrcntOfTotal,PrctByWeight:PrcntOfTotalWeight,CityName:CityName"),
        Map.entry("Sales Summary By Item & Warehouse", "WarehouseName:WarehouseName,ItemCode:ItemCode,ItemName:ItemName,ItemQty:QtyOut,BillWeight:BillWeightOut,Amount:AmountOut,AvgRate:AvgRate40Kg,PrctByAmount:PrcntOfTotal,PrctByWeight:PrcntOfTotalWeight,CityName:CityName"),
        Map.entry("Sales Summary By Item & City", "ItemCode:ItemCode,ItemName:ItemName,ItemQty:QtyOut,BillWeight:BillWeightOut,Amount:AmountOut,AvgRate:AvgRate40Kg,PrctByAmount:PrcntOfTotal,PrctByWeight:PrcntOfTotalWeight,CityName:CityName"),
        Map.entry("Sales Summary By Item,Pack Size & City", "ItemCode:ItemCode,ItemName:ItemName,PackUom:UOMCode,ItemQty:QtyOut,BillWeight:BillWeightOut,Amount:AmountOut,AvgRate:AvgRate,AvgRate40Kg:AvgRate40Kg,PrctByAmount:PrcntOfTotal,PrctByWeight:PrcntOfTotalWeight,CityName:CityName"),
        Map.entry("Sales Summary By Customer", "PartyName:Customer,ItemQty:QtyOut,BillWeight:BillWeightOut,Amount:AmountOut,AvgRate:AvgRate40Kg,PrctByAmount:PrcntOfTotal,PrctByWeight:PrcntOfTotalWeight,CityName:CityName"),
        Map.entry("Sales Summary By Customer & Item", "PartyName:Customer,ItemCode:ItemCode,ItemName:ItemName,PackUom:UOMCode,ItemQty:QtyOut,BillWeight:BillWeightOut,Amount:AmountOut,AvgRate:AvgRate,AvgRate40Kg:AvgRate40Kg,PrctByAmount:PrcntOfTotal,PrctByWeight:PrcntOfTotalWeight,CityName:CityName"),
        Map.entry("Sales Summary By Customer & City", "PartyName:Customer,PackUom:UOMCode,ItemQty:QtyOut,BillWeight:BillWeightOut,Amount:AmountOut,AvgRate:AvgRate,AvgRate40Kg:AvgRate40Kg,PrctByAmount:PrcntOfTotal,PrctByWeight:PrcntOfTotalWeight,CityName:CityName"),
        Map.entry("Sales Summary By Customer,Item & City", "PartyName:Customer,ItemCode:ItemCode,ItemName:ItemName,PackUom:UOMCode,ItemQty:QtyOut,BillWeight:BillWeightOut,Amount:AmountOut,AvgRate:AvgRate,AvgRate40Kg:AvgRate40Kg,PrctByAmount:PrcntOfTotal,PrctByWeight:PrcntOfTotalWeight,CityName:CityName"),
        Map.entry("Sales Summary By Customer & ReferenceParty", "PartyName:Customer,ReferenceParty:ReferencePartyName,ItemQty:QtyOut,BillWeight:BillWeightOut,Amount:AmountOut,AvgRate:AvgRate40Kg,PrctByAmount:PrcntOfTotal,PrctByWeight:PrcntOfTotalWeight"),
        Map.entry("Sales Summary By Customer,Item & ReferenceParty", "PartyName:Customer,ItemCode:ItemCode,ItemName:ItemName,ReferenceParty:ReferencePartyName,PackUom:UOMCode,ItemQty:QtyOut,BillWeight:BillWeightOut,Amount:AmountOut,AvgRate:AvgRate,AvgRate40Kg:AvgRate40Kg,PrctByAmount:PrcntOfTotal,PrctByWeight:PrcntOfTotalWeight"),
        Map.entry("Sales Summary By Customer & Pack Size", "PartyName:Customer,PackUom:UOMCode,ItemQty:QtyOut,BillWeight:BillWeightOut,Amount:AmountOut,AvgRate:AvgRate,AvgRate40Kg:AvgRate40Kg,PrctByAmount:PrcntOfTotal,PrctByWeight:PrcntOfTotalWeight"),
        Map.entry("Sales Summary By Parent Category", "ParentCategory:ParentCategory,ItemQty:QtyOut,BillWeight:BillWeightOut,Amount:AmountOut,AvgRate:AvgRate40Kg,PrctByAmount:PrcntOfTotal,PrctByWeight:PrcntOfTotalWeight"),
        Map.entry("Sales Summary By Parent Category & Item", "ItemName:ItemName,ParentCategory:ParentCategory,ItemQty:QtyOut,BillWeight:BillWeightOut,Amount:AmountOut,AvgRate:AvgRate40Kg,PrctByAmount:PrcntOfTotal,PrctByWeight:PrcntOfTotalWeight"),
        Map.entry("Sales Summary By Parent Category & Customer", "PartyName:Customer,ParentCategory:ParentCategory,ItemQty:QtyOut,BillWeight:BillWeightOut,Amount:AmountOut,AvgRate:AvgRate40Kg,PrctByAmount:PrcntOfTotal,PrctByWeight:PrcntOfTotalWeight"),
        Map.entry("Sales Summary By ReferenceParty", "ReferenceParty:ReferencePartyName,ItemQty:QtyOut,BillWeight:BillWeightOut,Amount:AmountOut,AvgRate:AvgRate40Kg,PrctByAmount:PrcntOfTotal,PrctByWeight:PrcntOfTotalWeight"),
        Map.entry("Sales Summary By ReferenceParty & City", "ReferenceParty:ReferencePartyName,CityName:CityName,ItemQty:QtyOut,BillWeight:BillWeightOut,Amount:AmountOut,AvgRate:AvgRate40Kg,PrctByAmount:PrcntOfTotal,PrctByWeight:PrcntOfTotalWeight"),
        Map.entry("Sales Summary By ReferenceParty & Item", "ReferenceParty:ReferencePartyName,ItemCode:ItemCode,ItemName:ItemName,ItemQty:QtyOut,BillWeight:BillWeightOut,Amount:AmountOut,AvgRate:AvgRate40Kg,PrctByAmount:PrcntOfTotal,PrctByWeight:PrcntOfTotalWeight"),
        Map.entry("Sales Summary By ReferenceParty,Item & Pack Size", "ReferenceParty:ReferencePartyName,ItemCode:ItemCode,ItemName:ItemName,PackUom:UOMCode,ItemQty:QtyOut,BillWeight:BillWeightOut,Amount:AmountOut,AvgRate:AvgRate,AvgRate40Kg:AvgRate40Kg,PrctByAmount:PrcntOfTotal,PrctByWeight:PrcntOfTotalWeight"),
        Map.entry("Sales Summary By ReferenceParty & Pack Size", "ReferenceParty:ReferencePartyName,PackUom:UOMCode,ItemQty:QtyOut,BillWeight:BillWeightOut,Amount:AmountOut,AvgRate:AvgRate,AvgRate40Kg:AvgRate40Kg,PrctByAmount:PrcntOfTotal,PrctByWeight:PrcntOfTotalWeight"),
        Map.entry("INVOICE WISE PROFITABLITY", "Id:RefDocIdNo,DocumentTypeCode:DocumentTypeCode,DocDate:DocDate,DocNo:DocCodeNo,GdnNo:GdnNo,ItemName:ItemName,HsCode:HsCode,SaleQty:QtyOut,SaleWeight:BillWeightOut,SaleRate:AvgRate,SaleAmount:ItemAmountWithoutExpense,JobOrderNo:JobOrderNo,RateOfJobNo:CgsRate,ProfitLossPerRate:ProfitLossPerRate,ProfitLoss:ProfitLoss,ProfitLossPercent:ProfitLossPercent"),
        Map.entry("Sales Summary HsCode", "BranchesId:BranchesId,BranchName:BranchName,HsCode:HsCode,ItemQty:QtyOut,BillWeight:BillWeightOut,Amount:AmountOut,AvgRate:AvgRate40Kg,PrctByAmount:PrcntOfTotal,PrctByWeight:PrcntOfTotalWeight")
    );
    public static String spec(String activity) {
        if(activity==null||!SPECS.containsKey(activity))throw new IllegalArgumentException("Please Select Activity First...");
        return SPECS.get(activity);
    }
    public static List<String> names(String spec) { return Arrays.stream(spec.split(",")).map(f->f.split(":")[0]).toList(); }
    public static Map<String,Object> project(Map<String,Object> source,String spec) {
        Map<String,Object> row=new LinkedHashMap<>();
        for(String field:spec.split(",")) {
            String[] pair=field.split(":");
            if("$NetRate".equals(pair[1])) {
                row.put(pair[0],number(value(source,"ItemRate"))-number(value(source,"RateCut")));
            } else row.put(pair[0],value(source,pair[1]));
        }
        if(source.containsKey("RefDocumentTypeId")&&source.containsKey("RefDocIdNo")) {
            row.put("LinkDocumentTypeId",source.get("RefDocumentTypeId"));
            row.put("LinkRecordId",source.get("RefDocIdNo"));
        }
        return row;
    }
    private static Object value(Map<String,Object> row,String key) {
        if(!row.containsKey(key))throw new IllegalStateException("Sale Invoice Activities Report is missing column "+key);
        return row.get(key);
    }
    private static double number(Object value) { return value==null?0:((Number)value).doubleValue(); }
}
