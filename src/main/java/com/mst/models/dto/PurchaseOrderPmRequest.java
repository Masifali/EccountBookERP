package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * Screen 498 "1002 Purchsae Order" (Packing Material) - {@code PurchsaeOrderPmNew.Insert()} :1722.
 *
 * Only the controls Insert() reads. Everything Insert() hard-codes (DocumentTypeId 700,
 * OrderCatagoryId 2, BaseDocumentTypeId 2, OrderStatus "Open", ProjectsId = BranchesId, the user
 * and year fields, the three totals) is set on the server, never taken from the request.
 */
public class PurchaseOrderPmRequest {
    /** 0 = Save / Save As, &gt;0 = Update. */
    public int Id;

    /** yyyy-MM-dd */
    public String DocDate;
    public int DocNo;
    public int BranchSrNo;

    public int OrderSupCustId;
    public String SupplierRefNo = "";
    public String RemarksHeader = "";
    public int PaymentTermsId;
    public String OrderDueDays = "";
    /** 1 = Load, 2 = Ponch (DeliveryTermBind :909). The TEXT is what is stored. */
    public int DeliveryTermId;
    /** yyyy-MM-dd */
    public String DeliveryStartDate;
    public String DeliveryDays = "";
    /** 1 = On Weight, 2 = On Qty (BillTypeBind :928). */
    public int BillCalculateTypeId;

    public int CurrencyId;
    public String ExchangeRate = "";

    /** OrderDetailRemoveIds - the saved detail ids the operator deleted (grd_ColumnButtonClick). */
    public List<Integer> removedDetailIds = new ArrayList<>();

    public List<Line> lines = new ArrayList<>();

    public List<InventoryPosItemRequest.Upload> files = new ArrayList<>();
    public List<Integer> removeAttachmentIds = new ArrayList<>();

    /** One row of {@code table} (InitializeComponentCustom :432-465). */
    public static class Line {
        public int Id;
        public int ItemId;
        public int CropYearId;
        public String WeightCapacity = "";
        /** yyyy-MM-dd (the desktop shows MMM-yyyy but stores the picker's full value). */
        public String PackingDate;
        public String ExpiryDate;
        public int PackUomId;
        public double ItemQty;
        public double WeightPerQty;
        public int RateUomId;
        public double ItemRate;
        public int TaxNameId;
        public double TaxPercent;
        public int LeadTime;
        public int RefDocumentTypeId;
        public int RefDocId;
        public int RefDocInvoiceId;
        public String Remarks = "";
    }
}
