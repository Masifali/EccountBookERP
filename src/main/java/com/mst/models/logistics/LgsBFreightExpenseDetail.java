package com.mst.models.logistics;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Architecture.Model.lgstcm.FreightVoucherOutwardExpenseDetail - every NON-virtual property, spelled as the C# property, as GenericProvider.SetProc
 * binds it to [lgstcm].[USP_FreightVoucherOutwardExpenseDetail_Insert]. Virtual properties (OtherChargesItemDescription) are not
 * sent and are not declared here.
 */
public class LgsBFreightExpenseDetail extends DesktopModel {
    public double AddAmount;
    public double LessAmount;
    public double Qty;
    public double Rate;
    public int FreightVoucherOutwardExpenseDetailId;
    public int FreightVoucherOutwardId;
    public int LogiticOtherChargesItemsId;
    public String Remarks;
}
