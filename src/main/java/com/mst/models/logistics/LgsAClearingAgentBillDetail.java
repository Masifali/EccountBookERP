package com.mst.models.logistics;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/** Generated from 0124_Architecture.Model.Service.ExImClearingAgentBillDetail.cs - every non-virtual property, as GenericProvider.SetProc binds it. */
public class LgsAClearingAgentBillDetail extends DesktopModel {
    public LocalDateTime DocDate;
    public LocalDateTime EffectedDateD;
    public LocalDateTime ValidUpTo;
    public double Amount;
    public double OtherChargesAmount;
    public double FcyAmount;
    public double ChargesRate;
    public double ExhangeRate;
    public double ItemRate;
    public double Qty;
    public double TaxAmount;
    public double TaxPercent;
    public double TotalAmount;
    public int BillHeaderId;
    public int CurrencyId;
    public int DebitAccountId;
    public int Id;
    public int ItemId;
    public int TaxNameId;
    public int UomId;
    public String Description;
    public String SupplierRefNo;
}
