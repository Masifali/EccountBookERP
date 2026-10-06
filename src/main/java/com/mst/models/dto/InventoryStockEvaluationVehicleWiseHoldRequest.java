package com.mst.models.dto;

import java.util.List;
import lombok.Data;

/** btnStockHold: the checked grid rows (RefDocumentTypeId, RefDocIdNo, RefDocSubIdNo) and the RemarksPopUp text. */
@Data
public class InventoryStockEvaluationVehicleWiseHoldRequest {
    private String remarks;
    private List<Row> rows;
    @Data
    public static class Row {
        private Integer refDocumentTypeId,refDocIdNo,refDocSubIdNo;
    }
}
