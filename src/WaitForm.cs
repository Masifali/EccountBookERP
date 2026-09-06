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


using System.Threading;
using System.Windows.Forms;

public class WaitForm
{
	private frmWaitForm waitform;

	private Thread loadthread;

	public void Show()
	{
		loadthread = new Thread((ThreadStart)LoadingProcess);
		loadthread.IsBackground = true;
		loadthread.Start();
	}

	public void Show(Form parent)
	{
		loadthread = new Thread((ParameterizedThreadStart)LoadingProcess);
		loadthread.IsBackground = true;
		loadthread.Start(parent);
	}

	public void Close()
	{
		if (waitform != null && !waitform.IsDisposed)
		{
			try
			{
				waitform.Invoke(new ThreadStart(waitform.CloseWaitForm));
			}
			catch
			{
			}
			finally
			{
				waitform = null;
				loadthread = null;
			}
		}
		if (loadthread != null && loadthread.IsAlive)
		{
			loadthread.Join(500);
		}
		loadthread = null;
	}

	private void LoadingProcess()
	{
		waitform = new frmWaitForm();
		Application.Run(waitform);
	}

	private void LoadingProcess(object parent)
	{
		Form parent2 = parent as Form;
		waitform = new frmWaitForm(parent2);
		Application.Run(waitform);
	}
}

}
