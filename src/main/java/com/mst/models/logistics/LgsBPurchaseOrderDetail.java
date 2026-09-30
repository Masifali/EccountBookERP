package com.mst.models.logistics;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Architecture.Model.lgstcm.PurchaseOrderDetail - every NON-virtual property, spelled as the C# property, as GenericProvider.SetProc
 * binds it to [lgstcm].[USP_PurchaseOrderDetail_Insert]. Virtual properties (ItemName, TaxName, ItemCode, TransactionCurrencyCode, GlcyCurrencyCode, FcyCurrencyCode, RateBaseUom, AgreementDocNo, RateBaseUomEquivalent, LocationType, FromCity, ToCity, LoadingPort, DestinationPort) are not
 * sent and are not declared here.
 */
public class LgsBPurchaseOrderDetail extends DesktopModel {
    public LocalDateTime partyRefDocDate;
    public double fcyAmount;
    public double lcyAmount;
    public double OrderRate;
    public double tcyAmount;
    public double transactionExchangeRate;
    public int ActionTypeId;
    public int AgreementDetailId;
    public int AgreementHeaderId;
    public int PurchaseOrderDetailId;
    public int PurchaseOrderHeaderId;
    public int RateBaseUomId;
    public int serviceItemId;
    public int transactionCurrencyId;
    public String partyRefDocNo;
    public String Remarks;
    public int TaxNameId;
    public int LocationTypeId;
    public int FromCityId;
    public int ToCityId;
    public int LoadingPortId;
    public int DestinationPortId;
    public double TaxPercent;
    public double TaxAmount;
    public double TotalAmount;
    public double Qty;
    public double NetWeight;
    public double FcyExchangeRate;
    public int FcyCurrencyId;
    public double GlcyExchangeRate;
    public double GlcyAmount;
    public int GlcyCurrencyId;
}
