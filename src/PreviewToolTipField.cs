using System.Windows.Forms;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.ModernHistories
{

public class PreviewToolTipField
{
	public string Caption { get; set; }

	public Control ValueControl { get; set; }

	public int CaptionWidth { get; set; } = 160;


	public int ValueWidth { get; set; } = 230;


	public int RowHeight { get; set; } = 28;


	public bool IsHighlight { get; set; }

	public Padding CaptionPadding { get; set; } = new Padding(1);


	public Padding ValuePadding { get; set; } = new Padding(1);

}

}
