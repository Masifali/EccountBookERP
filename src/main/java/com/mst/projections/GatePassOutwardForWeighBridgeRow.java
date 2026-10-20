package com.mst.projections;

import java.util.Date;

/** Row of Sp_GatePassInward_GetAllMethod @Activity='GetRefDocNoidByDocumentTypeId' (both @CheckConfig sub-branches; native). */
public interface GatePassOutwardForWeighBridgeRow {
    Integer getId();
    Integer getDocumentTypeId();
    Integer getGpSrNo();
    Date getGpDate();
    String getVehicleNo();
    Integer getNoOfPackages();
    String getOtherRemarks();
    String getSupplierContractCode();
    Integer getReferenceDocTypeId();
    Integer getReferenceDocNoId();
    String getGatepassType();
}
