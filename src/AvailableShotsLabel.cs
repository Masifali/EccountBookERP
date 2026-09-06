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

internal class AvailableShotsLabel : InfoLabel, IObserver
{
	public void Update(Observable from, CameraEvent e)
	{
		CameraEvent.Type eventType = CameraEvent.Type.NONE;
		if ((eventType = e.GetEventType()) == CameraEvent.Type.PROPERTY_CHANGED && (int)e.GetArg() == 1034 && eventType == CameraEvent.Type.PROPERTY_CHANGED)
		{
			CameraModel obj = (CameraModel)from;
			string infoText = "";
			infoText = obj.AvailableShot.ToString();
			UpdateProperty(infoText);
		}
	}
}

}
