using System;
using System.Data;
using System.Windows.Forms;
using Architecture.BLL.Lab;
using Architecture.BLL.WeightBridge;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.WeightBridge;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.WeightBridge
{

public class WeightBridge_Helper
{
	public static void CameraInitilization(HVUtility cam, string ip, short port, string user, string pass, string camName)
	{
		if (string.IsNullOrWhiteSpace(ip) || port == 0 || string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
		{
			MessageBox.Show(camName + " configuration missing!", "Camera Error");
			return;
		}
		try
		{
			cam.ConnectCamera(ip, port, user, pass);
			cam.Preview();
		}
		catch (Exception ex)
		{
			MessageBox.Show(camName + " connection failed.\n" + ex.Message, "Camera Error");
		}
	}

	public static void CameraDisconnect(HVUtility cam)
	{
		try
		{
			cam?.Disconnect();
		}
		catch
		{
		}
	}

	public static DataTable GetGpNoFromLab(UserAccount userAccount, string WbStatus = "Auto")
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		try
		{
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("Id", typeof(int));
			dataTable.Columns.Add("GpNo", typeof(int));
			dataTable.Columns.Add("VehicleNo");
			dataTable.Columns.Add("GatepassType");
			DataTable dt = InvLabAnalysisPurchaseHeader.GetGatePassIdandNoforWeighBridge(new ReportsParameters
			{
				OrganizationId = userAccount.OrganizationId,
				CompanyId = userAccount.CompanyId,
				BranchesId = userAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				WeighBridgeStatus = WbStatus
			});
			if (dt != null && dt.Rows.Count > 0)
			{
				foreach (DataRow row in dt.Rows)
				{
					DataRow dr = dataTable.NewRow();
					dr["Id"] = Conversion.ToInt(row["Id"]);
					dr["GpNo"] = Conversion.ToInt(row["GpSrNo"]);
					dataTable.Rows.Add(dr);
				}
			}
			return dataTable;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void WbPartyandItemsDtFillDbCall(UserAccount userAccount, DataTable dtParty, DataTable dtItems)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		try
		{
			dtParty.Clear();
			dtItems.Clear();
			DataTable dt = WbPartyandItemsDefine.GetAll(new WbPartyandItemsDefine
			{
				OrganizationId = userAccount.OrganizationId,
				CompanyId = userAccount.CompanyId
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToString(dt.Rows[i]["WbType"]) == "PartyName")
				{
					dtParty.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["PartyName"]);
				}
				else
				{
					dtItems.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ItemName"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static string GetTicketNoDbCall(UserAccount userAccount)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		try
		{
			return Conversion.ToString((object)WbTransactions.GenerateCode(new WbTransactions
			{
				OrganizationId = userAccount.OrganizationId,
				CompanyId = userAccount.CompanyId,
				BranchId = userAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			}));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}

}
