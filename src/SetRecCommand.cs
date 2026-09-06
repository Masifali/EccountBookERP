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

internal class SetRecCommand : Command
{
	private uint _status;

	public SetRecCommand(ref CameraModel model, uint status)
		: base(ref model)
	{
		_status = status;
	}

	public override bool Execute()
	{
		if (_model.FixedMovie == 1)
		{
			EDSDK.EdsSetPropertyData(_model.Camera, 1296u, 0, 4, _status);
		}
		return true;
	}
}

}
