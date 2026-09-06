using System;
using EDSDKLib;
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

internal class StartEvfCommand : Command
{
	public StartEvfCommand(ref CameraModel model)
		: base(ref model)
	{
	}

	public override bool Execute()
	{
		uint err = 0u;
		if (_model.EvfMode == 0)
		{
			uint evfMode = 1u;
			err = EDSDK.EdsSetPropertyData(_model.Camera, 1281u, 0, 4, evfMode);
		}
		if (err == 0)
		{
			uint device = _model.EvfOutputDevice;
			device |= 2u;
			err = EDSDK.EdsSetPropertyData(_model.Camera, 1280u, 0, 4, device);
		}
		switch (err)
		{
		case 129u:
		{
			CameraEvent e = new CameraEvent(CameraEvent.Type.DEVICE_BUSY, IntPtr.Zero);
			_model.NotifyObservers(e);
			return false;
		}
		default:
		{
			CameraEvent e2 = new CameraEvent(CameraEvent.Type.ERROR, (IntPtr)err);
			_model.NotifyObservers(e2);
			break;
		}
		case 0u:
			break;
		}
		return true;
	}
}

}
