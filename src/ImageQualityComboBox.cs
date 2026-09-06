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

internal class ImageQualityComboBox : PropertyComboBox, IObserver
{
	private ActionSource _actionSource;

	private EDSDK.EdsPropertyDesc _desc;

	public void SetActionSource(ref ActionSource actionSource)
	{
		_actionSource = actionSource;
	}

	public ImageQualityComboBox()
	{
		map.Add(6618895u, "RAW");
		map.Add(6553619u, "RAW + Large Fine Jpeg");
		map.Add(6553875u, "RAW + Middle Fine Jpeg");
		map.Add(6554131u, "RAW + Small Fine Jpeg");
		map.Add(6553618u, "RAW + Large Normal Jpeg");
		map.Add(6553874u, "RAW + Middle Normal Jpeg");
		map.Add(6554130u, "RAW + Small Normal Jpeg");
		map.Add(6557203u, "RAW + Small1 Fine Jpeg");
		map.Add(6557202u, "RAW + Small1 Normal Jpeg");
		map.Add(6557459u, "RAW + Small2 Jpeg");
		map.Add(6557715u, "RAW + Small3 Jpeg");
		map.Add(6553616u, "RAW + Large Jpeg");
		map.Add(6554896u, "RAW + Middle1 Jpeg");
		map.Add(6555152u, "RAW + Middle2 Jpeg");
		map.Add(6554128u, "RAW + Small Jpeg");
		map.Add(23396111u, "Middle RAW");
		map.Add(23330835u, "Middle RAW + Large Fine Jpeg");
		map.Add(23331091u, "Middle RAW + Middle Fine Jpeg");
		map.Add(23331347u, "Middle RAW + Small Fine Jpeg");
		map.Add(23330834u, "Middle RAW + Large Normal Jpeg");
		map.Add(23331090u, "Middle RAW + Middle Normal Jpeg");
		map.Add(23331346u, "Middle RAW + Small Normal Jpeg");
		map.Add(23334419u, "Middle RAW + Small1 Fine Jpeg");
		map.Add(23334418u, "Middle RAW + Small1 Normal Jpeg");
		map.Add(23334675u, "Middle RAW + Small2 Jpeg");
		map.Add(23334931u, "Middle RAW + Small3 Jpeg");
		map.Add(23330832u, "Middle RAW + Large Jpeg");
		map.Add(23332112u, "Middle RAW + Middle1 Jpeg");
		map.Add(23332368u, "Middle RAW + Middle2 Jpeg");
		map.Add(23331344u, "Middle RAW + Small Jpeg");
		map.Add(40173327u, "Small RAW");
		map.Add(40108051u, "Small RAW + Large Fine Jpeg");
		map.Add(40108307u, "Small RAW + Middle Fine Jpeg");
		map.Add(40108563u, "Small RAW + Small Fine Jpeg");
		map.Add(40108050u, "Small RAW + Large Normal Jpeg");
		map.Add(40108306u, "Small RAW + Middle Normal Jpeg");
		map.Add(40108562u, "Small RAW + Small Normal Jpeg");
		map.Add(40111635u, "Small RAW + Small1 Fine Jpeg");
		map.Add(40111634u, "Small RAW + Small1 Normal Jpeg");
		map.Add(40111891u, "Small RAW + Small2 Jpeg");
		map.Add(40112147u, "Small RAW + Small3 Jpeg");
		map.Add(40108048u, "Small RAW + Large Jpeg");
		map.Add(40109328u, "Small RAW + Middle1 Jpeg");
		map.Add(40109584u, "Small RAW + Middle2 Jpeg");
		map.Add(40108560u, "Small RAW + Small Jpeg");
		map.Add(1310479u, "Large Fine Jpeg");
		map.Add(1244943u, "Large Normal Jpeg");
		map.Add(18087695u, "Middle Fine Jpeg");
		map.Add(18022159u, "Middle Normal Jpeg");
		map.Add(34864911u, "Small Fine Jpeg");
		map.Add(34799375u, "Small Normal Jpeg");
		map.Add(236191503u, "Small1 Fine Jpeg");
		map.Add(236125967u, "Small1 Normal Jpeg");
		map.Add(252968719u, "Small2 Jpeg");
		map.Add(269745935u, "Small3 Jpeg");
		map.Add(1113871u, "Large Jpeg");
		map.Add(84999951u, "Middle1 Jpeg");
		map.Add(101777167u, "Middle2 Jpeg");
		map.Add(34668303u, "Small Jpeg");
		map.Add(6553359u, "CRAW");
		map.Add(6488083u, "CRAW + Large Fine Jpeg");
		map.Add(6488339u, "CRAW + Middle Fine Jpeg");
		map.Add(6489363u, "CRAW + Middle1 Fine Jpeg");
		map.Add(6489619u, "CRAW + Middle2 Fine Jpeg");
		map.Add(6488595u, "CRAW + Small Fine Jpeg");
		map.Add(6491667u, "CRAW + Small1 Fine Jpeg");
		map.Add(6491923u, "CRAW + Small2 Fine Jpeg");
		map.Add(6492179u, "CRAW + Small3 Fine Jpeg");
		map.Add(6488082u, "CRAW + Large Normal Jpeg");
		map.Add(6488338u, "CRAW + Middle Normal Jpeg");
		map.Add(6489362u, "CRAW + Middle1 Normal Jpeg");
		map.Add(6489618u, "CRAW + Middle2 Normal Jpeg");
		map.Add(6488594u, "CRAW + Small Normal Jpeg");
		map.Add(6491666u, "CRAW + Small1 Normal Jpeg");
		map.Add(6488080u, "CRAW + Large Jpeg");
		map.Add(6489360u, "CRAW + Middle1 Jpeg");
		map.Add(6489616u, "CRAW + Middle2 Jpeg");
		map.Add(6488592u, "CRAW + Small Jpeg");
		map.Add(8453903u, "HEIF Large");
		map.Add(6553728u, "RAW  + HEIF Large");
		map.Add(6488192u, "CRAW + HEIF Large");
		map.Add(8650511u, "HEIF Large Fine");
		map.Add(8584975u, "HEIF Large Normal");
		map.Add(25427727u, "HEIF Middle Fine");
		map.Add(25362191u, "HEIF Middle Normal");
		map.Add(243531535u, "HEIF Small1 Fine");
		map.Add(243465999u, "HEIF Small1 Normal");
		map.Add(260308751u, "HEIF Small2 Fine");
		map.Add(6553731u, "RAW + HEIF Large Fine");
		map.Add(6553730u, "RAW + HEIF Large Normal");
		map.Add(6553987u, "RAW + HEIF Middle Fine");
		map.Add(6553986u, "RAW + HEIF Middle Normal");
		map.Add(6557315u, "RAW + HEIF Small1 Fine");
		map.Add(6557314u, "RAW + HEIF Small1 Normal");
		map.Add(6557571u, "RAW + HEIF Small2 Fine");
		map.Add(6488195u, "CRAW + HEIF Large Fine");
		map.Add(6488194u, "CRAW + HEIF Large Normal");
		map.Add(6488451u, "CRAW + HEIF Middle Fine");
		map.Add(6488450u, "CRAW + HEIF Middle Normal");
		map.Add(6491779u, "CRAW + HEIF Small1 Fine");
		map.Add(6491778u, "CRAW + HEIF Small1 Normal");
		map.Add(6492035u, "CRAW + HEIF Small2 Fine");
	}

	protected override void OnSelectionChangeCommitted(EventArgs e)
	{
		if (base.SelectedItem != null)
		{
			uint key = (uint)_desc.PropDesc[SelectedIndex];
			_actionSource.FireEvent(ActionEvent.Command.SET_IMAGEQUALITY, (IntPtr)key);
		}
	}

	public void Update(Observable from, CameraEvent e)
	{
		CameraModel model = (CameraModel)from;
		CameraEvent.Type eventType = CameraEvent.Type.NONE;
		if (((eventType = e.GetEventType()) == CameraEvent.Type.PROPERTY_CHANGED || eventType == CameraEvent.Type.PROPERTY_DESC_CHANGED) && (int)e.GetArg() == 256)
		{
			uint property = model.ImageQuality;
			switch (eventType)
			{
			case CameraEvent.Type.PROPERTY_CHANGED:
				UpdateProperty(property);
				break;
			case CameraEvent.Type.PROPERTY_DESC_CHANGED:
				_desc = model.ImageQualityDesc;
				UpdatePropertyDesc(ref _desc);
				UpdateProperty(property);
				break;
			}
		}
	}
}

}
