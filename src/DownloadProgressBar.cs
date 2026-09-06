using System.Threading;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace CameraControl
{

internal class DownloadProgressBar : ProgressBar, IObserver
{
	public void Update(Observable from, CameraEvent e)
	{
		_ = (CameraModel)from;
		if (e.GetEventType() == CameraEvent.Type.PROGRESS)
		{
			uint percentage2 = (uint)(int)e.GetArg();
			UpdateProperty(percentage2);
		}
		if (e.GetEventType() == CameraEvent.Type.DOWNLOAD_COMPLETE)
		{
			Thread.Sleep(1000);
			uint percentage = 0u;
			UpdateProperty(percentage);
		}
	}
}

}
