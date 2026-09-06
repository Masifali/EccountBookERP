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

internal class AfModeComboBox : PropertyComboBox, IObserver
{
	public AfModeComboBox()
	{
		map.Add(0u, "One-Shot AF");
		map.Add(1u, "AI Servo AF");
		map.Add(2u, "AI Focus AF");
		map.Add(3u, "Manual Focus");
		map.Add(uint.MaxValue, "unknown");
	}

	public void Update(Observable from, CameraEvent e)
	{
		CameraModel model = (CameraModel)from;
		CameraEvent.Type eventType = CameraEvent.Type.NONE;
		if (((eventType = e.GetEventType()) == CameraEvent.Type.PROPERTY_CHANGED || eventType == CameraEvent.Type.PROPERTY_DESC_CHANGED) && (int)e.GetArg() == 1028)
		{
			uint property = model.AFMode;
			if (eventType == CameraEvent.Type.PROPERTY_CHANGED)
			{
				UpdateProperty(property);
			}
		}
	}
}

}
