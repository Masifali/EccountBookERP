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

internal class MovieQualityComboBox : PropertyComboBox, IObserver
{
	private ActionSource _actionSource;

	private EDSDK.EdsPropertyDesc _desc;

	public void SetActionSource(ref ActionSource actionSource)
	{
		_actionSource = actionSource;
	}

	public MovieQualityComboBox()
	{
		map.Add(0u, "");
		map.Add(512u, "1920x1080_2398");
		map.Add(528u, "1920x1080_2398_alli_editing");
		map.Add(560u, "1920x1080_2398_ipb_standard");
		map.Add(768u, "1920x1080_2400");
		map.Add(784u, "1920x1080_2400_alli_editing");
		map.Add(816u, "1920x1080_2400_ipb_standard");
		map.Add(1024u, "1920x1080_2500");
		map.Add(1040u, "1920x1080_2500_alli_editing");
		map.Add(1072u, "1920x1080_2500_ipb_standard");
		map.Add(1280u, "1920x1080_2997");
		map.Add(1296u, "1920x1080_2997_alli_editing");
		map.Add(1328u, "1920x1080_2997_ipb_standard");
		map.Add(1552u, "1920x1080_5000_alli_editing");
		map.Add(1584u, "1920x1080_5000_ipb_standard");
		map.Add(1808u, "1920x1080_5994_alli_editing");
		map.Add(1840u, "1920x1080_5994_ipb_standard");
		map.Add(4624u, "1920x1080_2398_alli_editing");
		map.Add(4656u, "1920x1080_2398_ipb_standard");
		map.Add(4657u, "1920x1080_2398_ipb_light");
		map.Add(4880u, "1920x1080_2400_alli_editing");
		map.Add(4912u, "1920x1080_2400_ipb_standard");
		map.Add(4913u, "1920x1080_2400_ipb_light");
		map.Add(5136u, "1920x1080_2500_alli_editing");
		map.Add(5168u, "1920x1080_2500_ipb_standard");
		map.Add(5169u, "1920x1080_2500_ipb_light");
		map.Add(5392u, "1920x1080_2997_alli_editing");
		map.Add(5424u, "1920x1080_2997_ipb_standard");
		map.Add(5425u, "1920x1080_2997_ipb_light");
		map.Add(5648u, "1920x1080_5000_alli_editing");
		map.Add(5680u, "1920x1080_5000_ipb_standard");
		map.Add(5681u, "1920x1080_5000_ipb_light");
		map.Add(5904u, "1920x1080_5994_alli_editing");
		map.Add(5936u, "1920x1080_5994_ipb_standard");
		map.Add(5937u, "1920x1080_5994_ipb_light");
		map.Add(6160u, "1920x1080_10000_alli_editing");
		map.Add(6416u, "1920x1080_11988_alli_editing");
		map.Add(67072u, "1280x720_5000");
		map.Add(67328u, "1280x720_5994");
		map.Add(67600u, "1280x720_1000_alli_editing");
		map.Add(67856u, "1280x720_1199_alli_editing");
		map.Add(70704u, "1280x720_2500_ipb_standard");
		map.Add(70705u, "1280x720_5000_ipb_standard");
		map.Add(70960u, "1280x720_2997_ipb_standard");
		map.Add(70961u, "1280x720_2997_ipb_light");
		map.Add(71184u, "1280x720_5000_alli_editing");
		map.Add(71216u, "1280x720_5000_ipb_standard");
		map.Add(71440u, "1280x720_5994_alli_editing");
		map.Add(71472u, "1280x720_5994_ipb_standard");
		map.Add(71696u, "1280x720_10000_alli_editing");
		map.Add(71728u, "1280x720_10000_ipb_standard");
		map.Add(71952u, "1280x720_11988_alli_editing");
		map.Add(71984u, "1280x720_11988_ipb_standard");
		map.Add(132096u, "640x480_2500");
		map.Add(132352u, "640x480_2397");
		map.Add(197184u, "4096x2160_2398_motion_jpg");
		map.Add(197440u, "4096x2160_2400_motion_jpg");
		map.Add(197696u, "4096x2160_2500_motion_jpg");
		map.Add(197952u, "4096x2160_2997_motion_jpg");
		map.Add(201232u, "4096x2160_2398_alli_editing");
		map.Add(201264u, "4096x2160_2398_ipb_standard");
		map.Add(201265u, "4096x2160_2398_ipb_light");
		map.Add(201488u, "4096x2160_2400_alli_editing");
		map.Add(201520u, "4096x2160_2400_ipb_standard");
		map.Add(201521u, "4096x2160_2400_ipb_light");
		map.Add(201744u, "4096x2160_2500_alli_editing");
		map.Add(201776u, "4096x2160_2500_ipb_standard");
		map.Add(201777u, "4096x2160_2500_ipb_light");
		map.Add(202000u, "4096x2160_2997_alli_editing");
		map.Add(202032u, "4096x2160_2997_ipb_standard");
		map.Add(202033u, "4096x2160_2997_ipb_light");
		map.Add(202256u, "4096x2160_5000_alli_editing");
		map.Add(202288u, "4096x2160_5000_ipb_standard");
		map.Add(202289u, "4096x2160_5000_ipb_light");
		map.Add(202512u, "4096x2160_5994_alli_editing");
		map.Add(202544u, "4096x2160_5994_ipb_standard");
		map.Add(202545u, "4096x2160_5994_ipb_light");
		map.Add(202768u, "4096x2160_10000_alli_editing");
		map.Add(203024u, "4096x2160_11988_alli_editing");
		map.Add(332304u, "3840x2160_2398_alli_editing");
		map.Add(332336u, "3840x2160_2398_ipb_standard");
		map.Add(332337u, "3840x2160_2398_ipb_light");
		map.Add(332560u, "3840x2160_2400_alli_editing");
		map.Add(332592u, "3840x2160_2400_ipb_standard");
		map.Add(332593u, "3840x2160_2400_ipb_light");
		map.Add(332816u, "3840x2160_2500_alli_editing");
		map.Add(332848u, "3840x2160_2500_ipb_standard");
		map.Add(332849u, "3840x2160_2500_ipb_light");
		map.Add(333072u, "3840x2160_2997_alli_editing");
		map.Add(333104u, "3840x2160_2997_ipb_standard");
		map.Add(333105u, "3840x2160_2997_ipb_light");
		map.Add(333328u, "3840x2160_5000_alli_editing");
		map.Add(333360u, "3840x2160_5000_ipb_standard");
		map.Add(333361u, "3840x2160_5000_ipb_light");
		map.Add(333584u, "3840x2160_5994_alli_editing");
		map.Add(333616u, "3840x2160_5994_ipb_standard");
		map.Add(333617u, "3840x2160_5994_ipb_light");
		map.Add(333840u, "3840x2160_10000_alli_editing");
		map.Add(334096u, "3840x2160_11988_alli_editing");
		map.Add(528912u, "8192x4320_2398_alli_editing");
		map.Add(528944u, "8192x4320_2398_ipb_standard");
		map.Add(528945u, "8192x4320_2398_ipb_light");
		map.Add(529168u, "8192x4320_2400_alli_editing");
		map.Add(529200u, "8192x4320_2400_ipb_standard");
		map.Add(529201u, "8192x4320_2400_ipb_light");
		map.Add(529424u, "8192x4320_2500_alli_editing");
		map.Add(529456u, "8192x4320_2500_ipb_standard");
		map.Add(529457u, "8192x4320_2500_ipb_light");
		map.Add(529680u, "8192x4320_2997_alli_editing");
		map.Add(529712u, "8192x4320_2997_ipb_standard");
		map.Add(529713u, "8192x4320_2997_ipb_light");
		map.Add(594448u, "7680x4320_2398_alli_editing");
		map.Add(594480u, "7680x4320_2398_ipb_standard");
		map.Add(594481u, "7680x4320_2398_ipb_light");
		map.Add(594736u, "7680x4320_2400_ipb_standard");
		map.Add(594737u, "7680x4320_2400_ipb_light");
		map.Add(594960u, "7680x4320_2500_alli_editing");
		map.Add(594992u, "7680x4320_2500_ipb_standard");
		map.Add(594993u, "7680x4320_2500_ipb_light");
		map.Add(595216u, "7680x4320_2997_alli_editing");
		map.Add(595248u, "7680x4320_2997_ipb_standard");
		map.Add(595249u, "7680x4320_2997_ipb_light");
		map.Add(668272u, "2398_raw");
		map.Add(668273u, "2398_raw_light");
		map.Add(668528u, "2400_raw");
		map.Add(668529u, "2400_raw_light");
		map.Add(668784u, "2500_raw");
		map.Add(668785u, "2500_raw_light");
		map.Add(669040u, "2997_raw");
		map.Add(669041u, "2997_raw_light");
		map.Add(669296u, "5000_raw");
		map.Add(669552u, "5994_raw");
		map.Add(134222352u, "1920x1080_2398_alli_editing_crop");
		map.Add(134222384u, "1920x1080_2398_ipb_standard_crop");
		map.Add(134222385u, "1920x1080_2398_ipb_light_crop");
		map.Add(134222864u, "1920x1080_2500_alli_editing_crop");
		map.Add(134222896u, "1920x1080_2500_ipb_standard_crop");
		map.Add(134222897u, "1920x1080_2500_ipb_light_crop");
		map.Add(134223120u, "1920x1080_2997_alli_editing_crop");
		map.Add(134223152u, "1920x1080_2997_ipb_standard_crop");
		map.Add(134223153u, "1920x1080_2997_ipb_light_crop");
		map.Add(134223376u, "1920x1080_5000_alli_editing_crop");
		map.Add(134223408u, "1920x1080_5000_ipb_standard_crop");
		map.Add(134223409u, "1920x1080_5000_ipb_light_crop");
		map.Add(134223632u, "1920x1080_5994_alli_editing_crop");
		map.Add(134223664u, "1920x1080_5994_ipb_standard_crop");
		map.Add(134223665u, "1920x1080_5994_ipb_light_crop");
		map.Add(134223888u, "1920x1080_10000_alli_editing_crop");
		map.Add(134224144u, "1920x1080_11988_alli_editing_crop");
		map.Add(134418960u, "4096x2160_2398_alli_editing_crop");
		map.Add(134418992u, "4096x2160_2398_ipb_standard_crop");
		map.Add(134418993u, "4096x2160_2398_ipb_light_crop");
		map.Add(134419216u, "4096x2160_2400_alli_editing_crop");
		map.Add(134419248u, "4096x2160_2400_ipb_standard_crop");
		map.Add(134419249u, "4096x2160_2400_ipb_light_crop");
		map.Add(134419472u, "4096x2160_2500_alli_editing_crop");
		map.Add(134419504u, "4096x2160_2500_ipb_standard_crop");
		map.Add(134419505u, "4096x2160_2500_ipb_light_crop");
		map.Add(134419728u, "4096x2160_2997_alli_editing_crop");
		map.Add(134419760u, "4096x2160_2997_ipb_standard_crop");
		map.Add(134419761u, "4096x2160_2997_ipb_light_crop");
		map.Add(134419984u, "4096x2160_5000_alli_editing_crop");
		map.Add(134420016u, "4096x2160_5000_ipb_standard_crop");
		map.Add(134420017u, "4096x2160_5000_ipb_light_crop");
		map.Add(134420240u, "4096x2160_5994_alli_editing_crop");
		map.Add(134420272u, "4096x2160_5994_ipb_standard_crop");
		map.Add(134420273u, "4096x2160_5994_ipb_light_crop");
		map.Add(134550032u, "3840x2160_2398_alli_editing_crop");
		map.Add(134550064u, "3840x2160_2398_ipb_standard_crop");
		map.Add(134550065u, "3840x2160_2398_ipb_light_crop");
		map.Add(134550544u, "3840x2160_2500_alli_editing_crop");
		map.Add(134550576u, "3840x2160_2500_ipb_standard_crop");
		map.Add(134550577u, "3840x2160_2500_ipb_light_crop");
		map.Add(134550800u, "3840x2160_2997_alli_editing_crop");
		map.Add(134550832u, "3840x2160_2997_ipb_standard_crop");
		map.Add(134550833u, "3840x2160_2997_ipb_light_crop");
		map.Add(134551056u, "3840x2160_5000_alli_editing_crop");
		map.Add(134551088u, "3840x2160_5000_ipb_standard_crop");
		map.Add(134551089u, "3840x2160_5000_ipb_light_crop");
		map.Add(134551312u, "3840x2160_5994_alli_editing_crop");
		map.Add(134551344u, "3840x2160_5994_ipb_standard_crop");
		map.Add(134551345u, "3840x2160_5994_ipb_light_crop");
		map.Add(uint.MaxValue, "unknown");
	}

	protected override void OnSelectionChangeCommitted(EventArgs e)
	{
		if (base.SelectedItem != null)
		{
			uint key = (uint)_desc.PropDesc[SelectedIndex];
			_actionSource.FireEvent(ActionEvent.Command.SET_MOVIEQUALITY, (IntPtr)key);
		}
	}

	public void Update(Observable from, CameraEvent e)
	{
		CameraModel model = (CameraModel)from;
		CameraEvent.Type eventType = CameraEvent.Type.NONE;
		if (((eventType = e.GetEventType()) != CameraEvent.Type.PROPERTY_CHANGED && eventType != CameraEvent.Type.PROPERTY_DESC_CHANGED) || (int)e.GetArg() != 16778275)
		{
			return;
		}
		uint property = model.MovieQuality;
		switch (eventType)
		{
		case CameraEvent.Type.PROPERTY_CHANGED:
			UpdateProperty(property);
			break;
		case CameraEvent.Type.PROPERTY_DESC_CHANGED:
			_desc = model.MovieQualityDesc;
			if (model.FixedMovie == 0)
			{
				_desc.NumElements = 0;
			}
			UpdatePropertyDesc(ref _desc);
			UpdateProperty(property);
			break;
		}
	}
}

}
