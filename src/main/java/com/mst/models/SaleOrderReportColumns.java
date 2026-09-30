package com.mst.models;

import java.util.*;

/** Column projections copied from frmSaleOrderHistory gridHisory, GrdMain_SelectionChanged and GridSummaryFill.
 * Hidden identifiers remain available for opening the original document. SQL values are not fabricated.
 */
public final class SaleOrderReportColumns {
    private SaleOrderReportColumns() { }
    public static final String DETAIL = "Id:Id,DocumentTypeId:DocumentTypeId,DocumentType:DocumentTypeDescription,DocDate:DocDate,DocNo:DocNo,CustomerName:SupplierName,BookingPerson:BookingPerson,ReferencePartyDetail:ReferencePartyNameDetail,CommissionAgent:CompanyNameSpCAgent,CommRate:CommRate,CommAmount:CommAmount,OrderSupCustId:OrderSupCustId,DueDays:OrderDueDays,DueDate:OrderDueDate,OrderItemId:OrderItemId,SaleGLAC:SaleGLAC,PaymentTerm:DeliveryTerm,DeliveryTerm:TermsDescription,ItemName:ItemName,CropYear:Crop,PackUom:UOMCodeItm,ItemQty:OrderItemQty,DispatchQty:DispatchQty,BalQty:BalQty,Weight:NetWeight,DispatchWeight:DispatchWeight,BalWeight:BalWeight,ItemRate:OrderItemRate,ItemAmount:Amount,OrderStatus:OrderStatus,BagPrice:BagPrice,BagWeight:BagWeight,DeliveryDays:DeliveryDays,ExpiryDate:OrderExpiryDate,EntryDate:EntryDate,EntryUser:UserNameEusr,ModifyDate:ModifyDate,ModifyUser:UserNameMusr,ApprovedDate:PostDate,ApprovedUser:UserNameAusr,ReferenceParty:ReferencePartyNameDetail,Remarks:RemarksHeader,ActionRemarks:ActionRemarks";
    public static final String HEADER = "Id:Id,DocumentTypeId:DocumentTypeId,DocumentType:DocumentTypeDescription,DocDate:DocDate,DocNo:DocNo,SupplierCustomerId:OrderSupCustId,CustomerName:SupplierName,BookingPerson:BookingPerson,CommissionAgent:CompanyNameSpCAgent,CommRate:CommRate,CommAmount:CommAmount,OrderStatus:OrderStatus,DueDays:OrderDueDays,DueDate:OrderDueDate,PaymentTerm:TermsDescription,DeliveryTerm:DeliveryTerm,DispatchQty:DispatchQty,OrderAmount:Amount,BalWeight:BalWeight,DeliveryDays:DeliveryDays,ExpiryDate:OrderExpiryDate,EntryDate:EntryDate,EntryUser:UserNameEusr,ModifyDate:ModifyDate,ModifyUser:UserNameMusr,ApprovedDate:PostDate,ApprovedUser:UserNameAusr,Remarks:RemarksHeader,ActionRemarks:ActionRemarks";
    public static final String LINES = "ReferenceParty:ReferencePartyNameDetail,ItemId:OrderItemId,SaleGLAC:SaleGLAC,ItemName:ItemName,CropYear:Crop,PackingType:PackTypeDesc,PackUom:UOMCodeItm,ItemQty:OrderItemQty,DispatchQty:DispatchQty,BalQty:BalQty,Weight:NetWeight,DispatchWeight:DispatchWeight,BalWeight:BalWeight,ItemRate:OrderItemRate,ItemAmount:Amount,BagPrice:BagPrice,BagWeight:BagWeight,CityName:CityArea";
    public static final List<String> ACTIVITIES = List.of(
        "Order Register",
        "Order Summary By Item",
        "Order Summary By Item & Pack Size",
        "Order Summary By Item & City",
        "Order Summary By Item,Pack Size & City",
        "Order Summary By Customer",
        "Order Summary By Customer & Item",
        "Order Summary By Customer & Pack Size",
        "Order Summary By Customer & City",
        "Order Summary By Customer,Item & City",
        "Order Summary By Customer and ReferenceParty",
        "Order Summary By Customer & Item & ReferenceParty",
        "Order Summary By ReferenceParty",
        "Order Summary By ReferenceParty & City",
        "Order Summary By ReferenceParty & Item"
    );
    private static final Map<String, String> SUMMARY = Map.ofEntries(
        Map.entry("Order Register", "Id:Id,DocumentTypeId:DocumentTypeId,BranchName:BranchName,DocType:DocumentTypeCode,DocDate:DocDate,DocNo:DocNo,PartyName:Customer,BookingPerson:BookingPerson,ReferenceParty:ReferencePartyDetail,ItemCode:ItemCode,ItemName:ItemName,PackUom:UOMCode,CropYear:CropBatch,JobLot:JobLotCode,PackingType:PackTypeCode,OrderQty:OrderQty,DispatchedQty:DispatchQty,BalQty:BalQty,OrderWeight:OrderWeight,DispatchedWeight:DispatchWeight,BalWeight:BalWeight,OrderAmount:OrderAmount,DispatchedAmount:DispatchAmount,BalAmount:BalAmount,AvgRate:AvgRate,PercentOfTotal:PrcntOfTotal,CityName:CityName"),
        Map.entry("Order Summary By Item", "Id:Id,ItemCode:ItemCode,ItemName:ItemName,OrderQty:OrderQty,DispatchedQty:DispatchQty,BalQty:BalQty,OrderWeight:OrderWeight,DispatchedWeight:DispatchWeight,BalWeight:BalWeight,OrderAmount:OrderAmount,DispatchedAmount:DispatchAmount,BalAmount:BalAmount,AvgRate:AvgRate,PercentOfTotal:PrcntOfTotal"),
        Map.entry("Order Summary By Item & Pack Size", "Id:Id,ItemCode:ItemCode,ItemName:ItemName,PackUom:UOMCode,OrderQty:OrderQty,DispatchedQty:DispatchQty,BalQty:BalQty,OrderWeight:OrderWeight,DispatchedWeight:DispatchWeight,BalWeight:BalWeight,OrderAmount:OrderAmount,DispatchedAmount:DispatchAmount,BalAmount:BalAmount,AvgRate:AvgRate,PercentOfTotal:PrcntOfTotal"),
        Map.entry("Order Summary By Item & City", "Id:Id,ItemCode:ItemCode,ItemName:ItemName,OrderQty:OrderQty,DispatchedQty:DispatchQty,BalQty:BalQty,OrderWeight:OrderWeight,DispatchedWeight:DispatchWeight,BalWeight:BalWeight,OrderAmount:OrderAmount,DispatchedAmount:DispatchAmount,BalAmount:BalAmount,AvgRate:AvgRate,PercentOfTotal:PrcntOfTotal,CityName:CityName"),
        Map.entry("Order Summary By Item,Pack Size & City", "Id:Id,ItemCode:ItemCode,ItemName:ItemName,PackUom:UOMCode,OrderQty:OrderQty,DispatchedQty:DispatchQty,BalQty:BalQty,OrderWeight:OrderWeight,DispatchedWeight:DispatchWeight,BalWeight:BalWeight,OrderAmount:OrderAmount,DispatchedAmount:DispatchAmount,BalAmount:BalAmount,AvgRate:AvgRate,PercentOfTotal:PrcntOfTotal,CityName:CityName"),
        Map.entry("Order Summary By Customer", "Id:Id,PartyName:Customer,OrderQty:OrderQty,DispatchedQty:DispatchQty,BalQty:BalQty,OrderWeight:OrderWeight,DispatchedWeight:DispatchWeight,BalWeight:BalWeight,OrderAmount:OrderAmount,DispatchedAmount:DispatchAmount,BalAmount:BalAmount,AvgRate:AvgRate,PercentOfTotal:PrcntOfTotal"),
        Map.entry("Order Summary By Customer & Item", "Id:Id,PartyName:Customer,ItemCode:ItemCode,ItemName:ItemName,OrderQty:OrderQty,DispatchedQty:DispatchQty,BalQty:BalQty,OrderWeight:OrderWeight,DispatchedWeight:DispatchWeight,BalWeight:BalWeight,OrderAmount:OrderAmount,DispatchedAmount:DispatchAmount,BalAmount:BalAmount,AvgRate:AvgRate,PercentOfTotal:PrcntOfTotal"),
        Map.entry("Order Summary By Customer & City", "Id:Id,PartyName:Customer,OrderQty:OrderQty,DispatchedQty:DispatchQty,BalQty:BalQty,OrderWeight:OrderWeight,DispatchedWeight:DispatchWeight,BalWeight:BalWeight,OrderAmount:OrderAmount,DispatchedAmount:DispatchAmount,BalAmount:BalAmount,AvgRate:AvgRate,PercentOfTotal:PrcntOfTotal,CityName:CityName"),
        Map.entry("Order Summary By Customer,Item & City", "Id:Id,PartyName:Customer,ItemCode:ItemCode,ItemName:ItemName,OrderQty:OrderQty,DispatchedQty:DispatchQty,BalQty:BalQty,OrderWeight:OrderWeight,DispatchedWeight:DispatchWeight,BalWeight:BalWeight,OrderAmount:OrderAmount,DispatchedAmount:DispatchAmount,BalAmount:BalAmount,AvgRate:AvgRate,PercentOfTotal:PrcntOfTotal,CityName:CityName"),
        Map.entry("Order Summary By Customer & Pack Size", "Id:Id,PartyName:Customer,PackUom:UOMCode,OrderQty:OrderQty,DispatchedQty:DispatchQty,BalQty:BalQty,OrderWeight:OrderWeight,DispatchedWeight:DispatchWeight,BalWeight:BalWeight,OrderAmount:OrderAmount,DispatchedAmount:DispatchAmount,BalAmount:BalAmount,AvgRate:AvgRate,PercentOfTotal:PrcntOfTotal"),
        Map.entry("Order Summary By Customer and ReferenceParty", "Id:Id,PartyName:Customer,ReferenceParty:ReferencePartyDetail,PackUom:UOMCode,OrderQty:OrderQty,DispatchedQty:DispatchQty,BalQty:BalQty,OrderWeight:OrderWeight,DispatchedWeight:DispatchWeight,BalWeight:BalWeight,OrderAmount:OrderAmount,DispatchedAmount:DispatchAmount,BalAmount:BalAmount,AvgRate:AvgRate,PercentOfTotal:PrcntOfTotal"),
        Map.entry("Order Summary By Customer & Item & ReferenceParty", "Id:Id,PartyName:Customer,ReferenceParty:ReferencePartyDetail,Itemcode:itemcode,ItemName:ItemName,PackUom:UOMCode,OrderQty:OrderQty,DispatchedQty:DispatchQty,BalQty:BalQty,OrderWeight:OrderWeight,DispatchedWeight:DispatchWeight,BalWeight:BalWeight,OrderAmount:OrderAmount,DispatchedAmount:DispatchAmount,BalAmount:BalAmount,AvgRate:AvgRate,PercentOfTotal:PrcntOfTotal"),
        Map.entry("Order Summary By ReferenceParty", "Id:Id,ReferenceParty:ReferencePartyDetail,PackUom:UOMCode,OrderQty:OrderQty,DispatchedQty:DispatchQty,BalQty:BalQty,OrderWeight:OrderWeight,DispatchedWeight:DispatchWeight,BalWeight:BalWeight,OrderAmount:OrderAmount,DispatchedAmount:DispatchAmount,BalAmount:BalAmount,AvgRate:AvgRate,PercentOfTotal:PrcntOfTotal"),
        Map.entry("Order Summary By ReferenceParty & City", "Id:Id,CityName:CityName,ReferenceParty:ReferencePartyDetail,PackUom:UOMCode,OrderQty:OrderQty,DispatchedQty:DispatchQty,BalQty:BalQty,OrderWeight:OrderWeight,DispatchedWeight:DispatchWeight,BalWeight:BalWeight,OrderAmount:OrderAmount,DispatchedAmount:DispatchAmount,BalAmount:BalAmount,AvgRate:AvgRate,PercentOfTotal:PrcntOfTotal"),
        Map.entry("Order Summary By ReferenceParty & Item", "Id:Id,ReferenceParty:ReferencePartyDetail,ItemCode:itemcode,ItemName:ItemName,PackUom:UOMCode,OrderQty:OrderQty,DispatchedQty:DispatchQty,BalQty:BalQty,OrderWeight:OrderWeight,DispatchedWeight:DispatchWeight,BalWeight:BalWeight,OrderAmount:OrderAmount,DispatchedAmount:DispatchAmount,BalAmount:BalAmount,AvgRate:AvgRate,PercentOfTotal:PrcntOfTotal")
    );
    public static String summary(String activity) {
        if (activity == null) throw new IllegalArgumentException("Please Select Activity First...");
        String spec = SUMMARY.get(activity);
        if (spec == null) throw new IllegalArgumentException("Please Select Activity First...");
        return spec;
    }
    public static List<String> names(String spec) {
        return Arrays.stream(spec.split(",")).map(field -> field.split(":")[0]).toList();
    }
    public static Map<String, Object> project(Map<String, Object> source, String spec) {
        Map<String, Object> row = new LinkedHashMap<>();
        for (String field : spec.split(",")) {
            String[] pair = field.split(":");
            if (!source.containsKey(pair[1])) throw new IllegalStateException("Sale Order Report is missing column " + pair[1]);
            row.put(pair[0], source.get(pair[1]));
        }
        return row;
    }
}
