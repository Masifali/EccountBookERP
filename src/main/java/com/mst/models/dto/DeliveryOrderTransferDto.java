package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * Screen 331 "Delivery Order (Stock Transfer)" — Architecture.WinApp.Sale.DeliveryOrder opened with
 * ScreenName / Tag "DeliveryOrderTransfer" (DocumentTypeId 84, DO Type 2 "StockTransfer").
 * What the page posts to /api/store/delivery-order-transfer/save.
 *
 * Row fields are the desktop grid's DataTable columns (InitializeComponentCustom, DeliveryOrder.cs:463-500)
 * that FillDetailListCommonForInsertAndDelete (:2085) reads. Everything the server owns — organization,
 * company, financial year, users, entry dates, DocNo, ScreenName, DeliveryOrderType — is filled by the
 * service from the signed-in user and from the stored header, never from here. Every posted id is
 * re-checked against the company's own lists before it is saved. Public fields keep Jackson's JSON names
 * exactly as spelled here (the page posts these keys).
 */
public class DeliveryOrderTransferDto {

    public Integer Id = 0;                 // RecId (0 = Save / SaveAs, > 0 = Update)
    public Integer SaveAsFromId = 0;       // the history row "SaveAs" was pressed on (grdhistory_Saveas:2626), else 0
    public String  DocDate;                // DocDate.Value, "yyyy-MM-dd"
    public Integer DoTypeId = 0;           // CmbDeliveryOrderType.Value (locked to 2 in this mode)
    public Integer SaleTypeId = 0;         // CmbSaleType.Value
    public String  VehicleType;            // CmbVehicleType.Text
    public String  VehicleNo;              // txtVehicleNo.Text
    public Integer BranchFromId = 0;       // cmbBranchFrom.Value
    public Integer BranchToId = 0;         // cmbBranchTo.Value
    public String  Remarks;                // txtremarks.Text -> LoadingInstructions
    public Boolean IsStockReserved = false;// ChkIsStockReservedPerParty.Checked (used only while it is visible)
    public Boolean ConfirmWarning = false; // the operator answered Yes to DeliveryOrderStackWarningMessage

    public List<Row> rows = new ArrayList<>();
    /** lstRemoveRecord — ids of saved detail rows removed from the grid (Update only). */
    public List<Integer> removedIds = new ArrayList<>();
    public List<Expense> expenses = new ArrayList<>();

    public static class Row {
        public Integer Id = 0;
        public Integer SupplierCustomerId = 0;
        public Integer RefPartyId = 0;
        public Integer ItemId = 0;
        public Integer ItemUOMId = 0;
        public Integer CropYearId = 0;
        public Integer PackingTypeId = 0;
        public Double  LoadQty = 0d;
        public Double  LoadWeight = 0d;
        public Double  PackingUnit = 0d;
        public Double  PackingWeight = 0d;
        public Double  GrossWeight = 0d;
        public Integer WareHouseId = 0;
        public Integer JobLotId = 0;
        public String  Remarks;
    }

    /** GridExpense row (dtExpense, :501-507). */
    public static class Expense {
        public Integer Id = 0;
        public Integer SaleOrderId = 0;
        public Integer SaleOrderCustomerExpId = 0;
        public Integer ItemId = 0;
        public Double  Qty = 0d;
        public String  Remarks;
    }
}
