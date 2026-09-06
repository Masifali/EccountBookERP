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

internal class GetPropertyCommand : Command
{
	private uint _propertyID;

	public GetPropertyCommand(ref CameraModel model, uint propertyID)
		: base(ref model)
	{
		_propertyID = propertyID;
	}

	public override bool Execute()
	{
		uint err = 0u;
		err = GetProperty(_propertyID);
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

	private uint GetProperty(uint propertyID)
	{
		uint err = 0u;
		EDSDK.EdsDataType dataType = EDSDK.EdsDataType.Unknown;
		int dataSize = 0;
		if (propertyID == 65535)
		{
			if (err == 0)
			{
				err = GetProperty(1078u);
			}
			if (err == 0)
			{
				err = GetProperty(1025u);
			}
			if (err == 0)
			{
				err = GetProperty(262u);
			}
			if (err == 0)
			{
				err = GetProperty(1030u);
			}
			if (err == 0)
			{
				err = GetProperty(1029u);
			}
			if (err == 0)
			{
				err = GetProperty(1026u);
			}
			if (err == 0)
			{
				err = GetProperty(1027u);
			}
			if (err == 0)
			{
				err = GetProperty(1031u);
			}
			if (err == 0)
			{
				err = GetProperty(256u);
			}
			if (err == 0)
			{
				err = GetProperty(1034u);
			}
			if (err == 0)
			{
				err = GetProperty(8u);
			}
			if (err == 0)
			{
				err = GetProperty(16778261u);
			}
			if (err == 0)
			{
				err = GetProperty(16778275u);
			}
			if (err == 0)
			{
				err = GetProperty(276u);
			}
			if (err == 0)
			{
				err = GetProperty(16778333u);
			}
			if (err == 0)
			{
				err = GetProperty(16778502u);
			}
			if (err == 0)
			{
				err = GetProperty(16778274u);
			}
			if (err == 0)
			{
				err = GetProperty(16778296u);
			}
			if (err == 0)
			{
				err = GetProperty(16778273u);
			}
			return err;
		}
		err = EDSDK.EdsGetPropertySize(_model.Camera, propertyID, 0, out dataType, out dataSize);
		if (err == 0)
		{
			switch (dataType)
			{
			case EDSDK.EdsDataType.UInt32:
			{
				err = EDSDK.EdsGetPropertyData(_model.Camera, propertyID, 0, out uint data);
				if (err == 0)
				{
					_model.SetPropertyUInt32(propertyID, data);
				}
				break;
			}
			case EDSDK.EdsDataType.Int32:
			{
				err = EDSDK.EdsGetPropertyData(_model.Camera, propertyID, 0, out uint data2);
				if (err == 0)
				{
					_model.SetPropertyInt32(propertyID, data2);
				}
				break;
			}
			case EDSDK.EdsDataType.String:
			{
				err = EDSDK.EdsGetPropertyData(_model.Camera, propertyID, 0, out string str);
				if (err == 0)
				{
					_model.SetPropertyString(propertyID, str);
				}
				break;
			}
			case EDSDK.EdsDataType.FocusInfo:
			{
				err = EDSDK.EdsGetPropertyData(_model.Camera, propertyID, 0, out EDSDK.EdsFocusInfo focusInfo);
				if (err == 0)
				{
					_model.SetPropertyFocusInfo(propertyID, focusInfo);
				}
				break;
			}
			case EDSDK.EdsDataType.ByteBlock:
			{
				byte[] data3 = new byte[dataSize];
				err = EDSDK.EdsGetPropertyData(_model.Camera, propertyID, 0, out data3);
				if (err == 0)
				{
					_model.SetPropertyByteBlock(propertyID, data3);
				}
				break;
			}
			}
		}
		else if (dataType == EDSDK.EdsDataType.FocusInfo || err == 80)
		{
			_model.FocusInfo = default(EDSDK.EdsFocusInfo);
			err = 0u;
		}
		if (err == 0)
		{
			CameraEvent e = new CameraEvent(CameraEvent.Type.PROPERTY_CHANGED, (IntPtr)propertyID);
			_model.NotifyObservers(e);
		}
		return err;
	}
}

}
