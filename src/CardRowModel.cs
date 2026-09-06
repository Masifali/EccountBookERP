using System.Collections.Generic;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Helper
{

public class CardRowModel
{
	public List<CardCellModel> ColList { get; set; }

	public CardCellModel Col1 { get; set; }

	public CardCellModel Col2 { get; set; }

	public CardCellModel Col3 { get; set; }

	public CardCellModel Col4 { get; set; }

	public int RowHeight { get; set; } = 22;

}

}
