using System;
using System.Threading;
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

internal class EndEvfCommand : Command
{
	public EndEvfCommand(ref CameraModel model)
		: base(ref model)
	{
	}

	public override bool Execute()
	{
		uint err = 0u;
		uint device = _model.EvfOutputDevice;
		if ((device & 2) == 0)
		{
			return true;
		}
		if (_model.EvfDepthOfFieldPreview != 0)
		{
			err = EDSDK.EdsGetPropertySize(_model.Camera, 1284u, 0, out var _, out var size2);
			err = EDSDK.EdsSetPropertyData(_model.Camera, 1284u, 0, size2, 0u);
			if (err == 0)
			{
				Thread.Sleep(1000);
			}
		}
		if (err == 0)
		{
			device &= 0xFFFFFFFDu;
			err = EDSDK.EdsGetPropertySize(_model.Camera, 1280u, 0, out var _, out var size);
			err = EDSDK.EdsSetPropertyData(_model.Camera, 1280u, 0, size, device);
		}
		switch (err)
		{
		case 41218u:
			return false;
		case 129u:
		{
			CameraEvent e = new CameraEvent(CameraEvent.Type.DEVICE_BUSY, IntPtr.Zero);
			_model.NotifyObservers(e);
			return false;
		}
		default:
		{
			CameraEvent e = new CameraEvent(CameraEvent.Type.ERROR, (IntPtr)err);
			_model.NotifyObservers(e);
			return false;
		}
		case 0u:
			_model.RollPitch = 1u;
			_model.NotifyObservers(new CameraEvent(CameraEvent.Type.ANGLEINFO, IntPtr.Zero));
			return true;
		}
	}
}

}
