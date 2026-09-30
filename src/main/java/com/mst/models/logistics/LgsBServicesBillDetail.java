package com.mst.models.logistics;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Architecture.Model.lgstcm.ServicesBillDetail - every NON-virtual property, spelled as the C# property, as GenericProvider.SetProc
 * binds it to [lgstcm].[USP_ServicesBillDetail_Insert]. Virtual properties (AgreementDocNo, PurchaseOrderDocNo, ItemName, ItemCode, TransactionCurrencyCode, RateBaseUom, RateBaseUomEquivalent, TaxName, DebitAccountCode, DebitAccountTitle, LocationType, FromCity, ToCity, LoadingPort, DestinationPort, GlcyCurrencyCode, FcyCurrencyCode) are not
 * sent and are not declared here.
 */
public class LgsBServicesBillDetail extends DesktopModel {
    public LocalDateTime partyRefDocDate;
    public double fcyAmount;
    public double lcyAmount;
    public double ServiceRate;
    public double TaxAmount;
    public double TaxPercent;
    public double tcyAmount;
    public double TotalAmount;
    public double transactionExchangeRate;
    public int ActionTypeId;
    public int AgreementDetailId;
    public int AgreementHeaderId;
    public int ChargesTypeId;
    public int DebitAccountId;
    public int PurchaseOrderDetailId;
    public int PurchaseOrderHeaderId;
    public int RateBaseUomId;
    public int serviceItemId;
    public int ServicesBillDetailId;
    public int ServicesBillHeaderId;
    public int TaxNameId;
    public int transactionCurrencyId;
    public String partyRefDocNo;
    public String Remarks;
    public int LocationTypeId;
    public int FromCityId;
    public int ToCityId;
    public int LoadingPortId;
    public int DestinationPortId;
    public double Qty;
    public double NetWeight;
    public double FcyExchangeRate;
    public int FcyCurrencyId;
    public double GlcyExchangeRate;
    public double GlcyAmount;
    public int GlcyCurrencyId;
}
