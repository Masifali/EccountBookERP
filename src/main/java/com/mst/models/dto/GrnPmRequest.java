package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * Screen 500 "Goods Receipt Notes PM" - the form as {@code GrnPackingMaterial.Insert()} (:534) reads it.
 *
 * Only what the operator can actually change on the desktop form is taken from here. Supplier,
 * delivery term, gate pass, vehicle, bilty and the two weights are disabled controls filled from
 * the gate pass / the order loader / the saved record, so the server re-derives them from the
 * same sources instead of trusting the request.
 */
public class GrnPmRequest {

    public int Id;
    public String DocDate;
    /** Gate pass the form was loaded from (txtGpId). Ignored on update - the saved one is kept. */
    public int InwardGatePassId;
    /** CmbTransport.Value - a ChartOfAccount id, or a SupplierCustomer id when feature 4 is on. */
    public int TransporterValue;
    public String CarriageAmount = "";
    public String RemarksHeader = "";

    public List<Line> lines = new ArrayList<>();
    public List<InventoryPosItemRequest.Upload> files = new ArrayList<>();
    public List<Integer> removeAttachmentIds = new ArrayList<>();

    /** One row of {@code dtdetail} (:346-369). */
    public static class Line {
        public int Id;
        public int OrderId;
        public int OrderDetailId;
        public int CropYearId;
        public String PackingDate;
        public String ExpiryDate;
        public double ItemQty;
        public double GrossWeight;
        public double SupplierQty;
        public String CityName = "";
        public int WarehouseId;
        public int RackId;
        public int ItemConditionId;
        public String RemarksDetail = "";
    }
}
