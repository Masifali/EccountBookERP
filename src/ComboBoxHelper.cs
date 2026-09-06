using System;
using System.Data;
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

public static class ComboBoxHelper
{
	public static DataTable GridComboBoxData(string RequestFor, DataGridViewComboBoxColumn cmb)
	{
		DataTable dt = null;
		try
		{
			if (RequestFor == "AccountCategory")
			{
				GetAccountCategory(cmb);
			}
		}
		catch (Exception)
		{
			throw;
		}
		return dt;
	}

	public static void GetAccountCategory(DataGridViewComboBoxColumn cmb)
	{
	}

	public static void AllMaterial(ComboBox cmb)
	{
	}

	public static void AllUnit(ComboBox cmb)
	{
	}

	public static void AllVoucherType(ComboBox cmb)
	{
	}
}

}
