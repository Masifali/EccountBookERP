using System;
using System.Runtime.InteropServices;
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

internal class WhiteBalanceComboBox : PropertyComboBox, IObserver
{
	private ActionSource _actionSource;

	private EDSDK.EdsPropertyDesc _desc;

	public void SetActionSource(ref ActionSource actionSource)
	{
		_actionSource = actionSource;
	}

	public WhiteBalanceComboBox()
	{
		map.Add(0u, "Auto: Ambience priority");
		map.Add(1u, "Daylight");
		map.Add(2u, "Cloudy");
		map.Add(3u, "Tungsten light");
		map.Add(4u, "White fluorescent light");
		map.Add(5u, "Flash");
		map.Add(6u, "Custom1");
		map.Add(8u, "Shade");
		map.Add(9u, "Color temp.");
		map.Add(10u, "Custom white balance: PC-1");
		map.Add(11u, "Custom white balance: PC-2");
		map.Add(12u, "Custom white balance: PC-3");
		map.Add(15u, "Custom2");
		map.Add(16u, "Custom3");
		map.Add(17u, "Underwater");
		map.Add(18u, "Custom4");
		map.Add(19u, "Custom5");
		map.Add(20u, "Custom white balance: PC-4");
		map.Add(21u, "Custom white balance: PC-5");
		map.Add(23u, "Auto: White priority");
	}

	protected override void OnSelectionChangeCommitted(EventArgs e)
	{
		if (base.SelectedItem != null)
		{
			uint key = (uint)_desc.PropDesc[SelectedIndex];
			_actionSource.FireEvent(ActionEvent.Command.SET_WHITE_BALANCE, (IntPtr)key);
		}
	}

	public void Update(Observable from, CameraEvent e)
	{
		CameraModel model = (CameraModel)from;
		CameraEvent.Type eventType = CameraEvent.Type.NONE;
		if ((eventType = e.GetEventType()) != CameraEvent.Type.PROPERTY_CHANGED && eventType != CameraEvent.Type.PROPERTY_DESC_CHANGED)
		{
			return;
		}
		switch ((uint)(int)e.GetArg())
		{
		case 262u:
		{
			uint property = model.WhiteBalance;
			switch (eventType)
			{
			case CameraEvent.Type.PROPERTY_CHANGED:
				UpdateProperty(property);
				break;
			case CameraEvent.Type.PROPERTY_DESC_CHANGED:
				_desc = model.WhiteBalanceDesc;
				UpdatePropertyDesc(ref _desc);
				UpdateProperty(property);
				break;
			}
			break;
		}
		case 16778502u:
		{
			if (eventType != CameraEvent.Type.PROPERTY_CHANGED || EDSDK.EdsGetPropertySize(model.Camera, 16778502u, 0, out var _, out var size) != 0)
			{
				break;
			}
			IntPtr ptr = Marshal.AllocHGlobal(size);
			if (EDSDK.EdsGetPropertyData(model.Camera, 16778502u, 0, size, ptr) != 0)
			{
				Marshal.FreeHGlobal(ptr);
				break;
			}
			byte[] mwb = EDSDK.ConvertMWB(EDSDK.MarshalPtrToManualWBData(ptr));
			if (EDSDK.EdsSetPropertyData(model.Camera, 16777732u, 0, mwb.Length, mwb) != 0)
			{
				Marshal.FreeHGlobal(ptr);
				break;
			}
			EDSDK.EdsSetPropertyData(model.Camera, 262u, 0, 4, 6);
			Marshal.FreeHGlobal(ptr);
			break;
		}
		}
	}
}

}
