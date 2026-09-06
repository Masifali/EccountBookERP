using System.Windows.Forms;
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

internal class ProgressBar : System.Windows.Forms.ProgressBar
{
	private delegate void _UpdateProperty(uint value);

	public ActionEvent.Command Command { get; set; }

	protected void UpdateProperty(uint value)
	{
		if (base.InvokeRequired)
		{
			Invoke(new _UpdateProperty(UpdateProperty), value);
		}
		else
		{
			base.Value = (int)value;
		}
	}
}

}
