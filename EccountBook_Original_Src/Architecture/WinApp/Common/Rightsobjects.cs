namespace Architecture.WinApp.Common;

public class Rightsobjects
{
	public bool DoHaveViewRight { get; set; }

	public bool DoHaveSaveRight { get; set; }

	public bool DoHavePrintRights { get; set; }

	public bool DoHaveUpdateRights { get; set; }

	public bool DoHaveFieldChooserRights { get; set; }

	public bool DoHaveSaveLayoutRights { get; set; }

	public bool DoHaveGridPrintRights { get; set; }

	public bool DoHaveGridExportRights { get; set; }

	public bool DoHaveGridGroupExpandRights { get; set; }

	public bool DoHaveGridGroupCollapseRights { get; set; }

	public bool DoHaveReportExportRights { get; set; }

	public bool DoHaveReportPrintRights { get; set; }

	public bool DoHaveCanViewAllRecordRights { get; set; }

	public bool DoHaveCanDelete { get; set; }

	public bool DoHaveApproveRights { get; set; }

	public bool DoHaveCanRateandAmount { get; set; }

	public bool RateandAmountFieldAccessDenied { get; set; }

	public bool DoHaveCanSettlementtab { get; set; }

	public bool CanChangeOrderStatusToComplete { get; set; }

	public bool CanChangeOrderStatusToOpen { get; set; }

	public bool CanChangeOrderStatusToCancel { get; set; }

	public bool CanChangeOrderExpiryDate { get; set; }

	public bool PurchaseOrder { get; set; }

	public bool PurchaseInvoice { get; set; }

	public bool PurchaseInvoiceDirect { get; set; }

	public bool SaleInvoiceReturn { get; set; }

	public bool SaleOrder { get; set; }

	public bool DeliveryOrder { get; set; }

	public bool DeliveryOrderPackingMaterial { get; set; }

	public bool SaleInvoice { get; set; }

	public bool SaleInvoiceDirect { get; set; }

	public bool GatePassOutward { get; set; }

	public bool GatePassInward { get; set; }

	public bool GatePassOutwardGeneral { get; set; }

	public bool CashPaymentVoucher { get; set; }

	public bool BankPaymentVoucher { get; set; }

	public bool CashReceiptsVoucher { get; set; }

	public bool BankReceiptsVoucher { get; set; }

	public bool ContraVoucher { get; set; }

	public bool ExpenseVoucher { get; set; }

	public bool BillPayables { get; set; }

	public bool BillReceivables { get; set; }

	public bool JournalVoucher { get; set; }

	public bool LabPurchaseAnalysis { get; set; }
}
