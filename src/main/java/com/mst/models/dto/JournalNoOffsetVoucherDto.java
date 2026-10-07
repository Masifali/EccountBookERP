package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * Screen 914 JournalVoucher_New ("Journal Voucher (without Offset)") -
 * desktop class Architecture.WinApp.Account_Definition.VouchersWithTax.JournalVoucher_New (DocumentTypeId 17).
 *
 * What the page posts: the header controls and the grid ROWS (the desktop's DataTable "table"). The ledger
 * lines, the cost-centre rows and VoucherAmount are derived on the server from the form's own Insert().
 */
public class JournalNoOffsetVoucherDto {
    /** 0 = Save, the loaded voucher's Id = Update (RecId). */
    public Integer Id = 0;
    public Integer VoucherCode = 0;            // txtdocno
    public String  VoucherDate;                // DocDate, yyyy-MM-dd
    public Integer ProjectId = 0;              // CmbProjectId ("Cost Center")
    public Integer LocationTypeId = 0;         // cmbLocationType
    public String  Remarks;                    // txtVoucherremarks (not trimmed)
    /** txtCheq.Text at the time of Save (the detail box, not a grid value). */
    public String  ChequeNo;
    public Integer MultiCurrencyId = 0;        // cmbCurrency
    public Double  ExchangeCurrencyRate = 0d;  // txtExchangeRate
    public Double  FcAmount = 0d;              // txtFcyAmount
    public Boolean CustomAccounts = Boolean.FALSE;   // chkCustomAccounts
    public List<Row> rows = new ArrayList<>();
    /** Debit accounts whose "same amount on the same date" question the operator already answered Yes to. */
    public List<Integer> duplicateAcknowledgedAccounts = new ArrayList<>();

    /** One row of grd (btnplus_Click -> table.Rows.Add(...)). */
    public static class Row {
        public Integer AccountId = 0;                 // CmbDr
        public Integer SubsidiaryAccountId = 0;       // CmbDrSubsidiaryAccount
        public Integer SubsidiaryAccountTypeId = 0;
        public String  Remarks;                       // txtDrComments
        public Integer JobLotId = 0;                  // CmbJobLot
        public String  CheqNo;                        // txtCheq
        public Double  FcyAmountDr = 0d;
        public Double  AmountDr = 0d;                 // txtAmount
        public Double  FcyAmountCr = 0d;
        public Double  AmountCr = 0d;                 // txtCredit
        public Integer BranchId = 0;                  // cmbBranch (0 when the Branch feature is off)
        public Integer CostCenterId = 0;              // CmbCostCenter (booking office only)
    }
}
