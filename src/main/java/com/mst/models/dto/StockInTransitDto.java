package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * Screen 868 "Stock In Transit" (frmSupplierDispatchPreBill.cs, DocumentTypeId 251) — what the page
 * posts to /api/purchase/stock-in-transit/save.
 *
 * Field names are the desktop control / grid column keys (public fields: Jackson uses the field name
 * as-is). Everything the server owns — organization, company, branch, financial year, users, Doc No,
 * Branch Sr No, entry dates, the header totals (TotalQty / TotalWeight / TotalAmount / FcyAmount), the
 * commission / brokery amounts and each row's FcyAmount — is filled by the service from the signed-in
 * user, the stored document or its own arithmetic (the desktop recomputes all of them at the start of
 * Insert(), :2034-2038), never taken from here.
 *
 * Text fields the desktop tests AS TEXT (TransitDays, ExchangeRate, SupplierWbWeight, the rates) are
 * posted as the text the operator typed, so the "empty or 0" checks see what the desktop saw.
 */
public class StockInTransitDto {

    public Integer Id = 0;                    // RecId (0 = new; btnsave_Click forces 0)
    public Object  attachments;               // Attachment form (AT) changes {files, removeAttachmentIds}; null = untouched
    public String  DocDate;                   // txtDocDateMain, "yyyy-MM-dd"
    public String  DepartureDate;             // txtDepartureDateMain (disabled; follows DocDate), "yyyy-MM-dd"
    public String  ETAdestination;            // txtETAdestinationMain, "yyyy-MM-dd"
    public String  TransitDays;               // txtTransitDaysMain (text)
    public String  VehicleNo;                 // txtVehicleNoMain
    public String  BiltyNo;                   // txtBiltyNoMain
    public String  ManualBillNo;              // txtManualBillNo
    public String  RemarksHeader;             // txtRemarksMain
    public String  Freight;                   // txtFreightMain (text)
    public String  OtherExpense;              // txtOtherExpenseMain (text)
    public String  AdvanceFreight;            // txtAdvanceFreight (text)
    public String  SupplierWbWeight;          // txtSupplierWbWtMain (text)
    public Integer SupplierId = 0;            // CmbSupplierDetail.Value
    public Integer ReferencePartyId = 0;      // CmbRefPartyDetail.Value
    public Integer CityId = 0;                // CmbCityNameMain.Value
    public String  DeliveryTerm;              // CmbDeliveryTerm.Text
    public Integer CurrencyId = 0;            // CmbCurrencyMain.Value
    public String  ExchangeRate;              // txtExchangeRateMain (text)

    public Integer CommissionAgentId = 0;     // CmbCommisionAgent.Value
    public String  CommTypeValue;             // CmbCommType.Value (the Id "1".."3", or the raw text of an unmatched entry)
    public String  CommTypeText;              // CmbCommType.Text
    public String  CommUomText;               // CmbCommUOM.Text
    public String  CommRate;                  // txtcommRate (text)
    public Integer BrokeryAcId = 0;           // CmbBrokeryAc.Value
    public String  BrokeryTypeValue;          // CmbBrokeryType.Value
    public String  BrokeryTypeText;           // CmbBrokeryType.Text
    public String  BrokeryUomValue;           // CmbBrokeryRateUom.Value (the Id "1".."4", or the raw text)
    public String  BrokeryUomText;            // CmbBrokeryRateUom.Text
    public String  BrokeryRate;               // txtBrokeryRate (text)
    /** txtBrokeryAmount as shown — TotalBrokeryAmount (:3136) leaves it unchanged when a rate is typed
     *  but the type is none of Flat / Percent / Comm Weight, so the previous text is an input. */
    public String  BrokeryAmountText;

    public List<Row> rows = new ArrayList<>();          // grd (table)
    public List<Expense> expenses = new ArrayList<>();  // grdInvExp (dtInvExp)

    /** InitializeComponentCustom (:572-600) — the detail table's columns that reach the save. */
    public static class Row {
        public Integer Id = 0;
        public Integer OrderId = 0;
        public Integer OrderDetailId = 0;
        public Integer ItemId = 0;
        public Integer CropYearId = 0;
        public Integer PackingTypeId = 0;
        public Integer PackUomId = 0;
        public Double  Qty = 0d;
        public Double  GrossWeight = 0d;
        public Double  EBUnit = 0d;
        public Double  EBTotal = 0d;
        public Double  AddLss = 0d;
        public Double  Weight = 0d;
        public Integer RateUomId = 0;
        public Double  Rate = 0d;
        public Double  Amount = 0d;
        public String  Remarks;
        public Integer ReceiverLocationId = 0;
    }

    /** dtInvExp (:601-608): Id, OrderId, OrderExpId, ItemId, Qty, Rate, Amount, Remarks. */
    public static class Expense {
        public Integer Id = 0;
        public Integer OrderId = 0;
        public Integer OrderExpId = 0;
        public Integer ItemId = 0;
        public Double  Qty = 0d;
        public Double  Rate = 0d;
        public Double  Amount = 0d;
        public String  Remarks;
    }
}
