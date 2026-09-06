using System;
using System.Data;
using System.Drawing;
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
namespace Architecture.WinApp.Helper
{

public class CardCellModel
{
	public string Text { get; set; }

	public string ColumnNameId { get; set; }

	public string ColumnNameText { get; set; }

	public CardCellType CellType { get; set; }

	public EventHandler OnClick { get; set; }

	public Font Font { get; set; } = new Font("Segoe UI", 9f);


	public bool Bold { get; set; }

	public Color ForeColor { get; set; } = Color.Black;


	public Color BackColor { get; set; } = Color.White;


	public ContentAlignment TextAlign { get; set; } = ContentAlignment.MiddleLeft;


	public Padding Padding { get; set; } = new Padding(3, 2, 3, 2);


	public int Height { get; set; } = 22;


	public int Width { get; set; } = 100;


	public bool Visible { get; set; } = true;


	public int ColumnSpan { get; set; } = 1;


	public DataTable DataSource { get; set; }

	public FilterType FilterType { get; set; }
}

}
