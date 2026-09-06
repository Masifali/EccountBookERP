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

internal class TempStatusLabel : InfoLabel, IObserver
{
	public void Update(Observable from, CameraEvent e)
	{
		CameraEvent.Type eventType = CameraEvent.Type.NONE;
		if ((eventType = e.GetEventType()) == CameraEvent.Type.PROPERTY_CHANGED && (int)e.GetArg() == 16778261 && eventType == CameraEvent.Type.PROPERTY_CHANGED)
		{
			CameraModel model = (CameraModel)from;
			string[] infoText = new string[8] { "Normal", "Warning", "FramerateDown", "DisableLiveview", "DisableRelease", "StillQualityWarning", "RestrictionMovieRecording", "unknown" };
			if ((model.TempStatus & 0xFFFF0000u) == 131072)
			{
				UpdateProperty(infoText[6]);
			}
			else if (infoText.Length > model.TempStatus)
			{
				UpdateProperty(infoText[model.TempStatus]);
			}
			else
			{
				UpdateProperty(infoText[infoText.Length - 1]);
			}
		}
	}
}

}
