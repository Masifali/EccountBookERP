using System;
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

internal class FormatVolumeCommand : Command
{
	private IntPtr _volume;

	public FormatVolumeCommand(ref CameraModel model, ref IntPtr volume)
		: base(ref model)
	{
		_volume = volume;
	}

	public override bool Execute()
	{
		_ = _model.Camera;
		uint err = 0u;
		if (err == 0)
		{
			if (MessageBox.Show("Format the memory card?", "CameraControl", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
			{
				return true;
			}
			if (err == 0)
			{
				err = EDSDK.EdsFormatVolume(_volume);
			}
		}
		return true;
	}
}

}
