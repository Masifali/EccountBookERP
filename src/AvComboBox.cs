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

internal class AvComboBox : PropertyComboBox, IObserver
{
	private ActionSource _actionSource;

	private EDSDK.EdsPropertyDesc _desc;

	public void SetActionSource(ref ActionSource actionSource)
	{
		_actionSource = actionSource;
	}

	public AvComboBox()
	{
		map.Add(0u, "");
		map.Add(8u, "1.0");
		map.Add(11u, "1.1");
		map.Add(12u, "1.2");
		map.Add(13u, "1.2");
		map.Add(16u, "1.4");
		map.Add(19u, "1.6");
		map.Add(20u, "1.8");
		map.Add(21u, "1.8");
		map.Add(24u, "2.0");
		map.Add(27u, "2.2");
		map.Add(28u, "2.5");
		map.Add(29u, "2.5");
		map.Add(32u, "2.8");
		map.Add(35u, "3.2");
		map.Add(128u, "3.3");
		map.Add(133u, "3.4");
		map.Add(36u, "3.5");
		map.Add(37u, "3.5");
		map.Add(40u, "4.0");
		map.Add(43u, "4.5");
		map.Add(44u, "4.5");
		map.Add(45u, "5.0");
		map.Add(48u, "5.6");
		map.Add(51u, "6.3");
		map.Add(52u, "6.7");
		map.Add(53u, "7.1");
		map.Add(56u, "8.0");
		map.Add(59u, "9.0");
		map.Add(60u, "9.5");
		map.Add(61u, "10");
		map.Add(64u, "11");
		map.Add(67u, "13");
		map.Add(68u, "13");
		map.Add(69u, "14");
		map.Add(72u, "16");
		map.Add(75u, "18");
		map.Add(76u, "19");
		map.Add(77u, "20");
		map.Add(80u, "22");
		map.Add(83u, "25");
		map.Add(84u, "27");
		map.Add(85u, "29");
		map.Add(88u, "32");
		map.Add(91u, "36");
		map.Add(92u, "38");
		map.Add(93u, "40");
		map.Add(96u, "45");
		map.Add(99u, "51");
		map.Add(100u, "54");
		map.Add(101u, "57");
		map.Add(104u, "64");
		map.Add(107u, "72");
		map.Add(108u, "76");
		map.Add(109u, "80");
		map.Add(112u, "91");
		map.Add(255u, "Auto");
		map.Add(uint.MaxValue, "unknown");
	}

	protected override void OnSelectionChangeCommitted(EventArgs e)
	{
		if (base.SelectedItem != null)
		{
			uint key = (uint)_desc.PropDesc[SelectedIndex];
			_actionSource.FireEvent(ActionEvent.Command.SET_AV, (IntPtr)key);
		}
	}

	public void Update(Observable from, CameraEvent e)
	{
		CameraModel model = (CameraModel)from;
		CameraEvent.Type eventType = CameraEvent.Type.NONE;
		if (((eventType = e.GetEventType()) == CameraEvent.Type.PROPERTY_CHANGED || eventType == CameraEvent.Type.PROPERTY_DESC_CHANGED) && (int)e.GetArg() == 1029)
		{
			uint property = model.Av;
			switch (eventType)
			{
			case CameraEvent.Type.PROPERTY_CHANGED:
				UpdateProperty(property);
				break;
			case CameraEvent.Type.PROPERTY_DESC_CHANGED:
				_desc = model.AvDesc;
				UpdatePropertyDesc(ref _desc);
				UpdateProperty(property);
				break;
			}
		}
	}
}

}
