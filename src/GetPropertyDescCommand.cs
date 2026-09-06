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

internal class GetPropertyDescCommand : Command
{
	private uint _propertyID;

	public GetPropertyDescCommand(ref CameraModel model, uint propertyID)
		: base(ref model)
	{
		_propertyID = propertyID;
	}

	public override bool Execute()
	{
		uint err = 0u;
		err = GetPropertyDesc(_propertyID);
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
			CameraEvent e2 = new CameraEvent(CameraEvent.Type.ERROR, (IntPtr)err);
			_model.NotifyObservers(e2);
			break;
		}
		case 0u:
			break;
		}
		return true;
	}

	private uint GetPropertyDesc(uint propertyID)
	{
		uint err = 0u;
		EDSDK.EdsPropertyDesc propertyDesc = default(EDSDK.EdsPropertyDesc);
		if (propertyID == 65535)
		{
			if (err == 0)
			{
				err = GetPropertyDesc(1078u);
			}
			if (err == 0)
			{
				err = GetPropertyDesc(1025u);
			}
			if (err == 0)
			{
				err = GetPropertyDesc(262u);
			}
			if (err == 0)
			{
				err = GetPropertyDesc(1030u);
			}
			if (err == 0)
			{
				err = GetPropertyDesc(1029u);
			}
			if (err == 0)
			{
				err = GetPropertyDesc(1026u);
			}
			if (err == 0)
			{
				err = GetPropertyDesc(1027u);
			}
			if (err == 0)
			{
				err = GetPropertyDesc(1031u);
			}
			if (err == 0)
			{
				err = GetPropertyDesc(256u);
			}
			if (err == 0)
			{
				err = GetPropertyDesc(1294u);
			}
			if (err == 0)
			{
				err = GetPropertyDesc(16778275u);
			}
			if (err == 0)
			{
				err = GetPropertyDesc(276u);
			}
			if (err == 0)
			{
				err = GetPropertyDesc(16778333u);
			}
			return err;
		}
		if (err == 0)
		{
			err = EDSDK.EdsGetPropertyDesc(_model.Camera, propertyID, out propertyDesc);
		}
		if (err == 0)
		{
			_model.SetPropertyDesc(propertyID, ref propertyDesc);
		}
		if (err == 0)
		{
			CameraEvent e = new CameraEvent(CameraEvent.Type.PROPERTY_DESC_CHANGED, (IntPtr)propertyID);
			_model.NotifyObservers(e);
		}
		return err;
	}
}

}
