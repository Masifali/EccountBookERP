using System.Collections.Generic;
using System.Data;
using Architecture.BLL;
using Architecture.Model;
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

public class Rights
{
	public DataTable getUserRights(int UserId, string ScreenName, string Role)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		DataTable dt = new DataTable();
		dt.Columns.Add("Right");
		dt.Columns.Add("Value");
		foreach (tblUserRights item in tblUserRights.GetByUserId(new tblUserRights
		{
			UserId = UserId,
			ScreenName = ScreenName,
			RightName = Role
		}))
		{
			dt.Rows.Add(item.RightName, item.Value);
		}
		return dt;
	}

	public bool getViewUserRights(ReportsParameters obj)
	{
		new List<tblUserRights>();
		using (List<tblUserRights>.Enumerator enumerator = tblUserRights.GetViewByUserId(obj).GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				return enumerator.Current.Value;
			}
		}
		return false;
	}
}

}
