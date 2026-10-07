package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * Screen 862 ExpenseVoucherNew ("Expense Voucher New") -
 * desktop class Architecture.WinApp.Account_Definition.VouchersWithTax.ExpenseVoucherNew (DocumentTypeId 26).
 *
 * What the page posts: the header controls, the grid ROWS (the desktop's DataTable "table") and the cost-centre
 * breakup rows (dtGridCostCenterDetail). The ledger line pairs and VoucherAmount are derived on the server from
 * the form's own Insert().
 */
public class ExpenseTaxVoucherDto {
    /** 0 = Save, the loaded voucher's Id = Update (RecId). */
    public Integer Id = 0;
    public Integer VoucherCode = 0;            // txtVoucherCode
    public String  VoucherDate;                // datVoucherDate, yyyy-MM-dd
    public Integer ProjectId = 0;              // CmbProjectId ("Cost Center")
    public Integer LocationTypeId = 0;         // cmbLocationType
    public Integer RefAccountId = 0;           // CmbRefAccountId ("Credit Account")
    public String  Remarks;                    // txtRemarks
    public Integer MultiCurrencyId = 0;        // cmbCurrency
    public Double  ExchangeCurrencyRate = 0d;  // txtExchangeRate
    public Double  FcAmount = 0d;              // txtFcyAmount
    public Boolean CustomAccounts = Boolean.FALSE;   // chkCustomAccounts
    public List<Row> rows = new ArrayList<>();
    public List<Break> costCenters = new ArrayList<>();
    /** The operator already answered Yes to the negative-balance / duplicate question. */
    public Boolean NegativeBalanceAcknowledged = Boolean.FALSE;
    public Boolean DuplicateAcknowledged = Boolean.FALSE;

    /** One row of grd (btnplus_Click -> table.Rows.Add(...)). */
    public static class Row {
        public Integer AccountId = 0;                 // CmbAccountId
        public Integer SubsidiaryAccountId = 0;       // CmbSubsidiaryAccount
        public Integer SubsidiaryAccountTypeId = 0;
        public Integer JobLotId = 0;                  // CmbJobLot
        public String  Remarks;                       // txtComments
        public Integer TcyCodeId = 0;                 // detail Tcy code
        public Double  TcyExchangeRate = 0d;          // txtTcyExchangeRateDetail
        public Double  FcyAmount = 0d;                // txtFcyAmountDetail
        public Double  Amount = 0d;                   // txtDebitAmount
        public Integer ReferenceAccountId = 0;        // CmbReferenceAccount
        public Integer LineId = 0;                    // max(LineId)+1 at Add
        public Integer CostCenterId = 0;              // 0 when more than one cost centre is checked
        public Integer BranchId = 0;                  // cmbBranch (0 when the Branch feature is off)
        public Integer ChequeId = 0;                  // credit account type 15 only
        public String  ChequeDate;
        public String  ChequeNo;
        public String  PayTitle;
    }

    /** One row of dtGridCostCenterDetail (Id, LineId, ParentId, DebitAccount, CostCenterId, CostCenter, Percent, Amount). */
    public static class Break {
        public Integer Id = 0;
        public Integer LineId = 0;
        public Integer CostCenterId = 0;
        public Double  Percent = 0d;
        public Double  Amount = 0d;
    }
}
