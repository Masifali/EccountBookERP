using System;
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

internal class ActionRadioButton : RadioButton
{
	private ActionSource _actionSource;

	public ActionEvent.Command Command { get; set; }

	public void SetActionSource(ref ActionSource actionSource)
	{
		_actionSource = actionSource;
	}

	protected override void OnClick(EventArgs e)
	{
		_actionSource.FireEvent(Command, IntPtr.Zero);
		base.OnCheckedChanged(e);
	}
}

}
