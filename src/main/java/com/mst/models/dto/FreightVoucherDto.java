package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * Screen "Freight Payment Voucher" — Architecture.WinApp.Account_Definition.FreightVoucher,
 * DocumentTypeId 28. The body the page posts to {@code /accounts/api/freight/save}.
 *
 * Every field is what {@code FreightVoucher.Insert()} reads from a control. The header text
 * fields travel as the TEXT the control shows (not a parsed number), because the desktop builds
 * the detail remarks from {@code txtSupplierWeight.Text}, {@code txtShortWeight.Text} and so on
 * verbatim — "12500" after a pending-record load, "12500.00" after an edit load.
 */
public class FreightVoucherDto {

    /** RecId — 0 on Save, the voucher id on Update. */
    public Integer id = 0;

    public String  documentNo;          // txtvoucherno
    public String  docDate;             // voucherdatetime, yyyy-MM-dd
    public Boolean creditAccount = false; // rdCreditAccount.Checked -> FreightOn 1
    public Integer cashAccountId = 0;   // CmbCashAccount.Value
    public String  manualNo;            // txtManualNumber
    public String  remarksHeader;       // txtremarksmain

    public String  gpId;                // txtGpId (hidden)
    public String  gpNo;                // txtGPNo
    public String  gpDate;              // txtGPDate, yyyy-MM-dd
    public String  vehicleNo;           // txtVehicleNo
    public String  biltyNo;             // txtBiltyNo
    public String  biltyFreight;        // txtBiltyFreight
    public String  partyName;           // txtPartyName
    public String  supplierWeight;      // txtSupplierWeight
    public String  factoryWeight;       // txtFactoryWeight
    public String  shortWeight;         // txtShortWeight
    public String  allowShortage;       // txtAllowShortage
    public String  shortageApply;       // txtShortageApply
    public String  toleranceWeight;     // txtToleranceWeight (header "Weigh Bridge Tolerance")
    public Integer cityId = 0;          // CmbCity.Value

    public Integer driverBiodataId = 0; // DriverBioId
    public String  cnicNo;              // txtCNIC.Text
    public String  driverCellNo;        // txtDriverCellNo.Text
    public Boolean driverCellMaskFull = false; // txtDriverCellNo.MaskFull
    public String  whatsappNo;          // txtWhatsAppNo.Text
    public String  alternateCellNo;     // txtAlternateCellNo.Text
    public String  driverName;          // txtDriverName.Text
    public String  fatherName;          // txtFatherName.Text
    public String  fatherCnicNo;        // txtFatherCNIC.Text

    public List<Row> rows = new ArrayList<>();
    public List<BreakUp> breakUps = new ArrayList<>();

    /** One row of {@code table} (grd). Column names are the desktop DataTable's. */
    public static class Row {
        public Integer id = 0;
        public Integer freightCustomAccountsGroupId = 0;
        public Integer debitAccountId = 0;
        public String  accountTitle;
        public Double  biltyFreight = 0d;
        public Double  freightPer100Kg = 0d;
        public Double  freightPerMton = 0d;
        public Double  wbCharges = 0d;
        public Double  advanceByParty = 0d;
        public Double  advanceByFactory = 0d;
        public Double  otherDeduction = 0d;
        public Double  totalFreight = 0d;
        public Double  shortageAmount = 0d;
        public Double  discountAmount = 0d;
        public Double  remainingAmount = 0d;
        public Double  deductionAmount = 0d;
        public Double  chargeToParty = 0d;
        public Double  paidAmount = 0d;
        public String  remarks;
        public String  varietyName;
        public String  itemQty;          // the cell's text, as "Freight of {ItemQty} Bags" uses it
        public Double  rateForShortage = 0d;
    }

    /** One row of {@code dtFreightPaymentBreakUp} (FreightVoucherPaymentBreakUp). */
    public static class BreakUp {
        public Integer id = 0;
        public Integer transactionTypeId = 0;
        public String  transactionType;
        public Integer instrumentTypeId = 0;
        public String  instrumentType;
        public Integer accountTitleId = 0;
        public String  accountTitle;
        public Integer chequeId = 0;
        public String  chequeNo;
        public String  chequeDate;       // yyyy-MM-dd
        public Double  amount = 0d;
        public String  payeeTitle;
        public String  remarks;
    }

    /** The Register tab's filter group (groupBox3). */
    public static class RegisterFilter {
        public String  fromDate;
        public String  toDate;
        public Integer gpNoFrom = 0;
        public Integer gpNoTo = 0;
        public String  vehicleNo;
        public Integer supplierCustomerId = 0;
        public Integer creditAccountId = 0;
        public Integer debitAccountId = 0;
        public Boolean onlyDiscountedRows = false;
        public Boolean freightAuditByCity = false;
    }
}
