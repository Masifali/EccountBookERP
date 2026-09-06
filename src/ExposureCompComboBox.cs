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

internal class ExposureCompComboBox : PropertyComboBox, IObserver
{
	private ActionSource _actionSource;

	private EDSDK.EdsPropertyDesc _desc;

	public void SetActionSource(ref ActionSource actionSource)
	{
		_actionSource = actionSource;
	}

	public ExposureCompComboBox()
	{
		map.Add(40u, "+5");
		map.Add(37u, "+4 2/3");
		map.Add(36u, "+4 1/2");
		map.Add(35u, "+4 1/3");
		map.Add(32u, "+4");
		map.Add(29u, "+3 2/3");
		map.Add(28u, "+3 1/2");
		map.Add(27u, "+3 1/3");
		map.Add(24u, "+3");
		map.Add(21u, "+2 2/3");
		map.Add(20u, "+2 1/2");
		map.Add(19u, "+2 1/3");
		map.Add(16u, "+2");
		map.Add(13u, "+1 2/3");
		map.Add(12u, "+1 1/2");
		map.Add(11u, "+1 1/3");
		map.Add(8u, "+1");
		map.Add(5u, "+2/3");
		map.Add(4u, "+1/2");
		map.Add(3u, "+1/3");
		map.Add(0u, "0");
		map.Add(253u, "-1/3");
		map.Add(252u, "-1/2");
		map.Add(251u, "-2/3");
		map.Add(248u, "-1");
		map.Add(245u, "-1 1/3");
		map.Add(244u, "-1 1/2");
		map.Add(243u, "-1 2/3");
		map.Add(240u, "-2");
		map.Add(237u, "-2 1/3");
		map.Add(236u, "-2 1/2");
		map.Add(235u, "-2 2/3");
		map.Add(232u, "-3");
		map.Add(229u, "-3 1/3");
		map.Add(228u, "-3 1/2");
		map.Add(227u, "-3 2/3");
		map.Add(224u, "-4");
		map.Add(221u, "-4 1/3");
		map.Add(220u, "-4 1/2");
		map.Add(219u, "-4 2/3");
		map.Add(216u, "-5");
		map.Add(uint.MaxValue, "unknown");
	}

	protected override void OnSelectionChangeCommitted(EventArgs e)
	{
		if (base.SelectedItem != null)
		{
			uint key = (uint)_desc.PropDesc[SelectedIndex];
			_actionSource.FireEvent(ActionEvent.Command.SET_EXPOSURE_COMPENSATION, (IntPtr)key);
		}
	}

	public void Update(Observable from, CameraEvent e)
	{
		CameraModel model = (CameraModel)from;
		CameraEvent.Type eventType = CameraEvent.Type.NONE;
		if (((eventType = e.GetEventType()) == CameraEvent.Type.PROPERTY_CHANGED || eventType == CameraEvent.Type.PROPERTY_DESC_CHANGED) && (int)e.GetArg() == 1031)
		{
			uint property = model.ExposureCompensation;
			switch (eventType)
			{
			case CameraEvent.Type.PROPERTY_CHANGED:
				UpdateProperty(property);
				break;
			case CameraEvent.Type.PROPERTY_DESC_CHANGED:
				_desc = model.ExposureCompensationDesc;
				UpdatePropertyDesc(ref _desc);
				UpdateProperty(property);
				break;
			}
		}
	}
}

}
