package com.mst.models;

import java.util.*;

/** Native frmGPOutward gridHisory:454-500. */
public final class SaleOutwardReportColumns {
    private SaleOutwardReportColumns() { }
    private static final String SPEC = "Id:id,BranchName:BranchName,GpSrNo:GpSrNo,GpDate:GpDate,GpTypeSrNo:GpTypeSrNo,GpType:GatepassType,OrderType:OtherSupCust,DeliveryOrderNo:DeliveryOrderNo,PartyName:CompanyName,VehicleType:VehicleType,VehicleNo:VehicleNo,BiltyNo:BiltyNo,NoOfPackages:NoOfPackages,InDateTime:InDateTimeStamp,OutDateTime:OutDateTimeStamp,IsApproved:IsApproved,InvoiceNo:InvoiceNo,DocAttachment:DocAttachment,Description:Description,Freight:Freight,SupplierContractCode:SupplierContractCode,Status:Status,NetPaid:NetPaid,SupplierWeight:SupplierWeight,FactoryWeight:FactoryWeight,DifferenceWeight:DifferenceWeight,WeighBridgeId:WeighBridgeId,GrossWeightWb:GrossWeightWb,TareWeightWb:TareWeightWb,WareHouseName:WareHouseName,GpVarietyName:VarietyName,ItemName:ItemName,JobLotDescription:JobLotDescription,PackingType:PackingType,PackUom:PackUom,DoQty:DoQty,GrossWeight:GrossWeight,EntryUser:UserNameEuser,EntryDate:EntryDate,ModifyUser:UserNameMuser,ModifyDate:ModifyDate,ApprovalUser:UserNameAuser,PostDate:PostDate,OtherRemarks:OtherRemarks";
    public static List<String> names() {
        return Arrays.stream(SPEC.split(",")).map(f -> f.split(":")[0]).toList();
    }
    public static Map<String,Object> project(Map<String,Object> source) {
        Map<String,Object> row=new LinkedHashMap<>();
        for(String field:SPEC.split(",")) {
            String[] pair=field.split(":");
            if(!source.containsKey(pair[1]))throw new IllegalStateException("Outward Gate Pass Report is missing column "+pair[1]);
            row.put(pair[0],source.get(pair[1]));
        }
        // Additional hidden link identity from the original procedure, not a displayed number used as an ID.
        row.put("DocumentTypeId",source.get("DocumentTypeId"));
        return row;
    }
}
