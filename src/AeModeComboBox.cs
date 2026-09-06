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

internal class AeModeComboBox : PropertyComboBox, IObserver
{
	private ActionSource _actionSource;

	private EDSDK.EdsPropertyDesc _desc;

	public void SetActionSource(ref ActionSource actionSource)
	{
		_actionSource = actionSource;
	}

	public AeModeComboBox()
	{
		map.Add(0u, "P");
		map.Add(1u, "Tv");
		map.Add(2u, "Av");
		map.Add(3u, "M");
		map.Add(55u, "Fv");
		map.Add(4u, "Bulb");
		map.Add(5u, "A-DEP");
		map.Add(6u, "DEP");
		map.Add(7u, "C1");
		map.Add(8u, "Lock");
		map.Add(9u, "GreenMode");
		map.Add(10u, "Night Portrait");
		map.Add(11u, "Sports");
		map.Add(12u, "Portrait");
		map.Add(13u, "Landscape");
		map.Add(14u, "Close-up");
		map.Add(15u, "No Strobo");
		map.Add(16u, "C2");
		map.Add(17u, "C3");
		map.Add(19u, "Creative Auto");
		map.Add(20u, "Movies");
		map.Add(21u, "Photo In Movie");
		map.Add(22u, "Scene Intelligent Auto");
		map.Add(23u, "Handheld Night Scene");
		map.Add(24u, "HDR Backlight Control");
		map.Add(25u, "SCN");
		map.Add(26u, "Kids");
		map.Add(27u, "Food");
		map.Add(28u, "Candlelight");
		map.Add(29u, "Creative filters");
		map.Add(30u, "Grainy B/W");
		map.Add(31u, "Soft focus");
		map.Add(32u, "Toy camera effect");
		map.Add(33u, "Fish-eye effect");
		map.Add(34u, "Water painting effect");
		map.Add(35u, "Miniature effect");
		map.Add(36u, "HDR art standard");
		map.Add(37u, "HDR art vivid");
		map.Add(38u, "HDR art bold");
		map.Add(39u, "HDR art embossed");
		map.Add(40u, "Dream");
		map.Add(41u, "Old Movies");
		map.Add(42u, "Memory");
		map.Add(43u, "Dramatic B&W");
		map.Add(44u, "Miniature effect movie");
		map.Add(45u, "Panning");
		map.Add(46u, "Group Photo");
		map.Add(50u, "Self Portrait");
		map.Add(51u, "Plus Movie Auto");
		map.Add(52u, "Smooth skin");
		map.Add(53u, "Panorama");
		map.Add(54u, "Silent Mode");
		map.Add(56u, "Art bold effect");
		map.Add(57u, "Fireworks");
		map.Add(58u, "Star portrait");
		map.Add(59u, "Star nightscape");
		map.Add(60u, "Star trails");
		map.Add(61u, "Star time-lapse movie");
		map.Add(62u, "Background blur");
		map.Add(uint.MaxValue, "unknown");
	}

	protected override void OnSelectionChangeCommitted(EventArgs e)
	{
		if (base.SelectedItem != null)
		{
			uint key = (uint)_desc.PropDesc[SelectedIndex];
			_actionSource.FireEvent(ActionEvent.Command.SET_AE_MODE, (IntPtr)key);
		}
	}

	public void Update(Observable from, CameraEvent e)
	{
		CameraModel model = (CameraModel)from;
		CameraEvent.Type eventType = CameraEvent.Type.NONE;
		if (((eventType = e.GetEventType()) == CameraEvent.Type.PROPERTY_CHANGED || eventType == CameraEvent.Type.PROPERTY_DESC_CHANGED) && (int)e.GetArg() == 1078)
		{
			uint property = model.AEMode;
			switch (eventType)
			{
			case CameraEvent.Type.PROPERTY_CHANGED:
				UpdateProperty(property);
				break;
			case CameraEvent.Type.PROPERTY_DESC_CHANGED:
				_desc = model.AEModeDesc;
				UpdatePropertyDesc(ref _desc);
				UpdateProperty(property);
				break;
			}
		}
	}
}

}
