package com.mst.services;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.LinkedHashMap;
import java.util.Map;

/**
 * Screen 911 "Freight Voucher Report" (ScreenName/TargetUrl FreightVoucherRegister -
 * FreightVoucherRegister.cs). Its data calls are the ones the Freight Voucher screen's own register
 * already makes, with the same parameters, so the page uses FreightVoucherController for them:
 *   HistoryComboBind  InvFreightVoucher.GetDataForDropDownFromFreightVoucher(org, comp, null)  -> /accounts/api/freight/history-combos
 *   btnshow_Click     InvFreightVoucher.FreightVoucherSlipAndRegister (DocumentTypeId 28, FY, dates,
 *                     GpSrNoF/T, VehicleNo, SupplierCustomerId, CreditAccountId, DebitAccountId)  -> /accounts/api/freight/register
 *   Print / Voucher   CommonServices.FreightVoucherSlip241 / ANewAcRptPaymentReceiptsVoucherSlip_102 -> /slip241, /voucher102
 * This service only supplies what VoucherValidation_Load reads from globals: ActiveYr.Start_Period and
 * the amount decimal configuration used by GridEX_Helper.GridWrappingAndColumnSettings.
 */
@Service
public class FreightVoucherReportService {

    @Autowired
    private AccountReportsHSupport h;

    public Map<String, Object> init() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("financialYearStart", h.financialYearStart());
        out.put("amountDecimals", h.amountDecimals());
        return out;
    }
}
