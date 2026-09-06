// using Attachment = global::Architecture.Model.Attachment;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using SupplierCustomer = global::Architecture.Model.Inventory.SupplierCustomer;
using ChartofAccount = global::Architecture.Model.Accounts.ChartofAccount;
using BankReconciliation = global::Architecture.Model.Accounts.BankReconciliation;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp
{
using DropDownBind = global::Architecture.WinApp.Common.DropDownBind;
using Rightsobjects = global::Architecture.WinApp.Common.Rightsobjects;


using System.Collections.Generic;
using System.Windows.Forms;

public static class HistoryStack
{
	public static List<Form> history = new List<Form>();

	private static int currentIndex = -1;

	public static void Push(Form form)
	{
		history.Add(form);
		currentIndex = history.Count - 1;
	}

	public static Form GetPrevious()
	{
		if (currentIndex > 0)
		{
			currentIndex--;
			return history[currentIndex];
		}
		return null;
	}

	public static Form GetNext()
	{
		if (currentIndex < history.Count - 1)
		{
			currentIndex++;
			return history[currentIndex];
		}
		return null;
	}

	public static bool CanNavigateForward()
	{
		return currentIndex < history.Count - 1;
	}

	public static bool CanNavigateBackward()
	{
		return currentIndex > 0;
	}

	public static void Remove(Form form)
	{
		history.Remove(form);
		if (currentIndex >= history.Count)
		{
			currentIndex = history.Count - 1;
		}
	}

	public static Form GetCurrentForm()
	{
		return history[currentIndex];
	}
}

}
