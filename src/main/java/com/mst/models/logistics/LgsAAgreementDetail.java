package com.mst.models.logistics;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/** Generated from 0594_Architecture.Model.lgstcm.AgreementDetail.cs - every non-virtual property, as GenericProvider.SetProc binds it. */
public class LgsAAgreementDetail extends DesktopModel {
    public boolean isActive;
    public LocalDateTime effectiveDateFrom;
    public LocalDateTime effectiveDateTo;
    public LocalDateTime partyRefDocDate;
    public double fcyAmount;
    public double lcyAmount;
    public double serviceRate;
    public double tcyAmount;
    public double transactionExchangeRate;
    public int ActionTypeId;
    public int agreementDetailId;
    public int AgreementHeaderId;
    public int serviceItemId;
    public int serviceRateBaseUomId;
    public int transactionCurrencyId;
    public String partyRefDocNo;
    public String Remarks;
}
