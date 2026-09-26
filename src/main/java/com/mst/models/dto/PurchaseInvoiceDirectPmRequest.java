package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * Screen 495 "Purchase Invoice Direct PM" - what {@code frmPurchaseInvoiceDirectPM.Insert()} (:2356) reads.
 * Amounts, taxes, freight shares and the bill are recomputed on the server with the form's formulas.
 */
public class PurchaseInvoiceDirectPmRequest {

    public int Id;
    public String DocDate;
    public int SupplierId;
    public String ManualBillNo = "";
    public String RemarksHeader = "";
    public int TaxAccountId;
    public int CurrencyId;
    public double ExchangeRate;

    public List<Line> lines = new ArrayList<>();
    public List<Freight> freight = new ArrayList<>();
    public List<Journal> journal = new ArrayList<>();
    public List<InventoryPosItemRequest.Upload> files = new ArrayList<>();
    public List<Integer> removeAttachmentIds = new ArrayList<>();

    /** One dtGrid row (btnAdd_Click :2024 / btnUpdateDetail_Click :2202). */
    public static class Line {
        public int Id;
        public int ItemId;
        public int WarehouseId;
        public int RackId;
        public int ItemConditionId;
        public int CropYearId;
        public int JobLotId;
        public String PackingDate;
        public String ExpiryDate;
        public int UOMId;
        public double ItemQty;
        public double Rate;
        public int RateUOMId;
        public int TaxNameId;
        public double TaxPercent;
        public String RemarksDetail = "";
        public String GpNo = "";
        public String VehicleNo = "";
        public int RefDocumentTypeId;
        public int RefDocId;
        public int RefDocInvoiceId;
    }

    /** grdFreight - Transporter is the combo value (dtAccountlst.SuppliercustomerId). */
    public static class Freight {
        public int Transporter;
        public double Freight;
        public String Remarks = "";
    }

    /** grdGLedger - AccountId is the combo value, as for freight. */
    public static class Journal {
        public int AccountId;
        public String Remarks = "";
        public double Percentage;
        public double Qty;
        public double Rate;
        public double Debit;
        public double Credit;
    }
}
