using System.Windows.Forms;
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

public class PropertyTrackBar : TrackBar
{
	private delegate void _UpdateProperty(uint value);

	private delegate void _UpdatePropertyDesc(ref EDSDK.EdsPropertyDesc desc);

	protected void UpdateProperty(uint value)
	{
		if (base.InvokeRequired)
		{
			Invoke(new _UpdateProperty(UpdateProperty), value);
		}
		else if (base.CanSelect)
		{
			base.Value = (int)value;
		}
	}

	protected void UpdatePropertyDesc(ref EDSDK.EdsPropertyDesc desc)
	{
		if (base.InvokeRequired)
		{
			Invoke(new _UpdatePropertyDesc(UpdatePropertyDesc), desc);
			return;
		}
		base.Enabled = desc.NumElements != 0;
		if (desc.NumElements != 0)
		{
			base.Maximum = desc.PropDesc[0] - 1;
		}
	}
}

}
