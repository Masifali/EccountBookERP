using System;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Configurations
{

public class SMSParameters
{
	public string AccountCode { get; set; }

	public string AccountTitle { get; set; }

	public string DocumentNo { get; set; }

	public DateTime DocumentDate { get; set; }

	public string OtherDocNo { get; set; }

	public string Remarks { get; set; }

	public double Amount { get; set; }

	public double Quantity { get; set; }

	public string ChequeNo { get; set; }

	public DateTime ChequeDate { get; set; }

	public string CompanyName { get; set; }

	public string CellNo { get; set; }

	public string SCS { get; set; }

	public string GpNo { get; set; }

	public DateTime GpDate { get; set; }

	public string VehicleNo { get; set; }

	public string BilityNo { get; set; }

	public string ContainerNo { get; set; }

	public string SealNo { get; set; }

	public string DriverName { get; set; }

	public string DriverCellNo { get; set; }

	public double NetWeight { get; set; }
}

}
