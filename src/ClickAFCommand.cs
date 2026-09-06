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

internal class ClickAFCommand : Command
{
	private uint _status;

	public ClickAFCommand(ref CameraModel model, uint status)
		: base(ref model)
	{
		_status = status;
	}

	public override bool Execute()
	{
		if (!_model.isEvfEnable)
		{
			return true;
		}
		if (_status == 1)
		{
			_model.NotifyObservers(new CameraEvent(CameraEvent.Type.MOUSE_CURSOR, (IntPtr)1));
		}
		uint err = 0u;
		if (_status == 2)
		{
			int param = _model.ClickPoint;
			EDSDK.EdsSize AfPoint = default(EDSDK.EdsSize);
			AfPoint.width = param >> 16;
			AfPoint.height = param & 0xFFFF;
			err = EDSDK.EdsSetFramePoint(_model.Camera, AfPoint, inLockAfFrame: true);
		}
		switch (err)
		{
		case 129u:
		{
			CameraEvent e = new CameraEvent(CameraEvent.Type.DEVICE_BUSY, IntPtr.Zero);
			_model.NotifyObservers(e);
			return true;
		}
		default:
		{
			CameraEvent e2 = new CameraEvent(CameraEvent.Type.ERROR, (IntPtr)err);
			_model.NotifyObservers(e2);
			return true;
		}
		case 0u:
			return true;
		}
	}
}

}
