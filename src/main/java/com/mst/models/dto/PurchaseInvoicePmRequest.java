package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * Screen 501 "Purchase Invoice PM" - the controls {@code PurchaseInvoicePackingMaterial.Insert()} (:1916)
 * reads that the operator can change. Everything else (supplier, delivery term, bill type, quantities,
 * weights, rates) is re-read on the server from the GRNs or the saved invoice.
 */
public class PurchaseInvoicePmRequest {

    public int Id;
    public String DocDate;
    public String ManualBillNo = "";
    public String RemarksHeader = "";
    public int PaymentTermsId;
    public String DueDays = "";
    public int TaxAccountId;

    public List<Line> lines = new ArrayList<>();
    public List<Freight> freight = new ArrayList<>();
    public List<Journal> journal = new ArrayList<>();
    public List<InventoryPosItemRequest.Upload> files = new ArrayList<>();
    public List<Integer> removeAttachmentIds = new ArrayList<>();

    /** One grid row. {@code Edited} = grd_CellUpdated / the F1 tax popup ran on this row. */
    public static class Line {
        public int Id;
        public int InvGrnId;
        public int InvGrnDetailId;
        public boolean Edited;
        public double AddLsAmount;
        public int TaxNameId;
        public double TaxPercent;
    }

    /** grdFreight - Transporter is the combo value (SupplierCustomerId with feature 4, else account id). */
    public static class Freight {
        public int InvGrnId;
        public int Transporter;
        public double Freight;
        public double Debit;
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
