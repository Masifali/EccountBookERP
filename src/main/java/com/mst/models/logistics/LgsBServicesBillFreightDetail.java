package com.mst.models.logistics;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Architecture.Model.lgstcm.ServicesBillFreightVoucherDetail - every NON-virtual property, spelled as the C# property, as GenericProvider.SetProc
 * binds it to [lgstcm].[USP_ServicesBillFreightVoucherDetail_Insert]. Virtual properties (RateBaseUom, RateBaseUomEquivalent) are not
 * sent and are not declared here.
 */
public class LgsBServicesBillFreightDetail extends DesktopModel {
    public int ServicesBillFreightVoucherDetailId;
    public int ServicesBillHeaderId;
    public int ExImInvoiceId;
    public int FreightVoucherOutwardId;
    public int GatePassOutwardId;
    public int DeliveryOrderId;
    public double Qty;
    public double Weight;
    public double FreightRate;
    public int RateBaseUomId;
    public double QtyForRate;
    public double BiltyFreight;
    public double OtherCharges;
    public double TotalBiltyFreight;
    public double ThisRowFreightAmount;
    public String Remarks;
}
