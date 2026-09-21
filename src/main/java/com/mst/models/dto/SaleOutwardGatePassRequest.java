package com.mst.models.dto;
import java.time.*;
public class SaleOutwardGatePassRequest {
 public int id,gpSrNo,gpTypeSrNo,supplierCustomerId,noOfPackages,refDocumentTypeId,refDocumentEntryNo,cityId,weighBridgeId,warehouseId,saleOrderId,itemId,transporterId,referencePartyId;
 public LocalDate gpDate,biltyDate; public LocalDateTime inDateTimeStamp,outDateTimeStamp;
 public String gatepassType,otherSupCust,vehicleType,vehicleNo,biltyNo,otherRemarks,supplierContractCode,status,varietyName,container,container1,sealNo,sealNo1,docAttachment,weightDiffRemarks;
 public double freight,netPaid,supplierWeight,factoryWeight,differenceWeight,packUnit,weightCommapredToSoWt;
 public boolean approved;
}
