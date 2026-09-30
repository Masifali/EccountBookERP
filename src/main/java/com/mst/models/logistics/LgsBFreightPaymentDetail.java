package com.mst.models.logistics;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Architecture.Model.lgstcm.FreightVoucherOutwardPaymentDetail - every NON-virtual property, spelled as the C# property, as GenericProvider.SetProc
 * binds it to [lgstcm].[USP_FreightVoucherOutwardPaymentDetail_Insert]. Virtual properties (AccountTitle, InstrumentType, TransactionType) are not
 * sent and are not declared here.
 */
public class LgsBFreightPaymentDetail extends DesktopModel {
    public LocalDateTime CheqDate;
    public BigDecimal CreditAmount = BigDecimal.ZERO;
    public BigDecimal DebitAmount = BigDecimal.ZERO;
    public int AccountId;
    public int ActionTypeId;
    public int CheqId;
    public int FreightVoucherOutwardId;
    public int FreightVoucherOutwardPaymentDetailId;
    public int InstrumentTypeId;
    public int TransTypeId;
    public int LineId;
    public String CheqNo;
    public String PayeeTitle;
    public String Remarks;
}
