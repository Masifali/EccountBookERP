using System.Drawing;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Common
{

public class clsGeneral
{
	public static void ValidateTextControl(ref TextBox txt, string dMessage, ErrorProvider ep)
	{
		if (txt.Text.Trim().Length == 0)
		{
			ep.SetError(txt, "Please Enter the " + dMessage + " Value.");
		}
		else
		{
			ep.SetError(txt, "");
		}
	}

	public static void ValidInfragesticDDL(ref UltraCombo DDL, string dMessage, ErrorProvider ep)
	{
		if (int.Parse(((UltraGridBase)DDL).ActiveRow.Cells[0].Value.ToString()) == 0)
		{
			ep.SetError((Control)(object)DDL, "Please Select the " + dMessage + " Value.");
		}
		else
		{
			ep.SetError((Control)(object)DDL, "");
		}
	}

	public static void ValidateComboControl(ref ComboBox cbo, string dMessage, ErrorProvider ep)
	{
		if (cbo.Text.Trim().Length == 0)
		{
			ep.SetError(cbo, "Please Select the " + dMessage + " Value.");
		}
		else
		{
			ep.SetError(cbo, "");
		}
	}

	public static void txtBoxEnterColor(ref TextBox txtBox)
	{
		txtBox.BackColor = Color.Yellow;
	}

	public static void txtBoxLeaveColor(ref TextBox txtBox)
	{
		txtBox.BackColor = Color.White;
	}

	public static void cboBoxEnterColor(ref ComboBox cmbBox)
	{
		cmbBox.BackColor = Color.Yellow;
	}

	public static void cboBoxDefaultColor(ref ComboBox cmbBox)
	{
		cmbBox.BackColor = Color.Gainsboro;
	}

	public static void txtBoxDefaultColor(ref TextBox txtBox)
	{
		txtBox.BackColor = Color.Gainsboro;
	}
}

}
