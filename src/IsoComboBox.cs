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

internal class IsoComboBox : PropertyComboBox, IObserver
{
	private ActionSource _actionSource;

	private EDSDK.EdsPropertyDesc _desc;

	public void SetActionSource(ref ActionSource actionSource)
	{
		_actionSource = actionSource;
	}

	public IsoComboBox()
	{
		map.Add(0u, "Auto");
		map.Add(40u, "6");
		map.Add(48u, "12");
		map.Add(56u, "25");
		map.Add(64u, "50");
		map.Add(72u, "100");
		map.Add(75u, "125");
		map.Add(77u, "160");
		map.Add(80u, "200");
		map.Add(83u, "250");
		map.Add(85u, "320");
		map.Add(88u, "400");
		map.Add(91u, "500");
		map.Add(93u, "640");
		map.Add(96u, "800");
		map.Add(99u, "1000");
		map.Add(101u, "1250");
		map.Add(104u, "1600");
		map.Add(107u, "2000");
		map.Add(109u, "2500");
		map.Add(112u, "3200");
		map.Add(115u, "4000");
		map.Add(117u, "5000");
		map.Add(120u, "6400");
		map.Add(123u, "8000");
		map.Add(125u, "10000");
		map.Add(128u, "12800");
		map.Add(131u, "16000");
		map.Add(133u, "20000");
		map.Add(136u, "25600");
		map.Add(139u, "32000");
		map.Add(141u, "40000");
		map.Add(144u, "51200");
		map.Add(147u, "64000");
		map.Add(149u, "80000");
		map.Add(152u, "102400");
		map.Add(160u, "204800");
		map.Add(168u, "409600");
		map.Add(176u, "819200");
		map.Add(uint.MaxValue, "unknown");
	}

	protected override void OnSelectionChangeCommitted(EventArgs e)
	{
		if (base.SelectedItem != null)
		{
			uint key = (uint)_desc.PropDesc[SelectedIndex];
			_actionSource.FireEvent(ActionEvent.Command.SET_ISO_SPEED, (IntPtr)key);
		}
	}

	public void Update(Observable from, CameraEvent e)
	{
		CameraModel model = (CameraModel)from;
		CameraEvent.Type eventType = CameraEvent.Type.NONE;
		if (((eventType = e.GetEventType()) == CameraEvent.Type.PROPERTY_CHANGED || eventType == CameraEvent.Type.PROPERTY_DESC_CHANGED) && (int)e.GetArg() == 1026)
		{
			uint property = model.Iso;
			switch (eventType)
			{
			case CameraEvent.Type.PROPERTY_CHANGED:
				UpdateProperty(property);
				break;
			case CameraEvent.Type.PROPERTY_DESC_CHANGED:
				_desc = model.IsoDesc;
				UpdatePropertyDesc(ref _desc);
				UpdateProperty(property);
				break;
			}
		}
	}
}

}
