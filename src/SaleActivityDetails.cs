using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.ModernHistories;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.ModernHistoryModels;
using Architecture.WinApp.Common;
using Janus.Windows.GridEX;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Commentaries
{

public class SaleActivityDetails : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DataTable dtOrderDetail = new DataTable();

	private DataTable dtGatePassessDetail = new DataTable();

	private DataTable dtweighBridgeDetail = new DataTable();

	private DataTable dtGRNDetail = new DataTable();

	private DataTable dtInvoiceInfo = new DataTable();

	private DataTable dtDoInfo = new DataTable();

	private DataTable dtWagesInfo = new DataTable();

	private int SaleOrderId;

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel1;

	private Panel panel2;

	private Label label2;

	private Label label1;

	private TabPage tabPage2;

	internal Label label3;

	private Label BalanceWeight;

	private Label RecievedWeight;

	private Label OrderWeight;

	private Label label14;

	private Label label13;

	private Label label12;

	private Label ExpiryStatus;

	private Label OrderStatus;

	private Label DeliveryTerm;

	private Label label7;

	private Label label6;

	private Label label4;

	private Label ExpiryDate;

	internal Label label8;

	private Label OrderDate;

	internal Label label5;

	private Label OrderNo;

	private Label ApprovalUserName;

	private Label ModifyUserName;

	private Label EntryUserName;

	private Label lblApprovalUser;

	private Label lblModifyUser;

	private Label label18;

	private Panel OrderDetailIInfoPanel;

	private Panel panel4;

	private Label label9;

	private Panel panel5;

	private Panel GdnDetailPanel;

	private Label label21;

	private Panel WeighBridgePanel;

	private Label label17;

	private Panel GatePassDetailPanel;

	private Label label16;

	private Panel DeliveryOrderPanel;

	internal GridEX GrdDeliveryOrder;

	private Panel panel8;

	private Label label15;

	private Panel panel6;

	private Label label11;

	private Label label10;

	internal GridEX GrdOrderDetail;

	private Panel panel12;

	private Panel panel13;

	private Panel panel14;

	private Label ApprovalUser1;

	private Label ModifyUser1;

	private Label EntryUser1;

	private Label lblApprovalUser1;

	private Label lblModifyUser1;

	private Label label25;

	private Label BalanceWeight1;

	private Label RecievedWeight1;

	private Label OrderWeight1;

	private Label label29;

	private Label label30;

	private Label label31;

	private Label ExpiryStatus1;

	private Label OrderStatus1;

	private Label DeliveryTerm1;

	private Label label35;

	private Label label36;

	private Label label37;

	private Label ExpiryDate1;

	internal Label label39;

	private Label OrderDate1;

	internal Label label41;

	private Label OrderNo1;

	internal Label label43;

	private Label label44;

	private Label label45;

	private Panel panel15;

	internal GridEX gridEX1;

	private Label label19;

	internal GridEX GrdInvoicesDetail;

	internal GridEX GrdGdnDetail;

	internal GridEX GrdWeighBridge;

	internal GridEX GrdGatePass;

	private Panel InvoiceDetailPanel;

	private Label label23;

	private Panel WagesDetailPanel;

	internal GridEX GrdWagesInfo;

	private Label label22;

	private Label txtPartyName;

	private Label label24;

	public SaleActivityDetails(UserAccount pUser, int OrderId)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUser;
		SaleOrderId = OrderId;
	}

	private void ActivityDetails_Load(object sender, EventArgs e)
	{
		try
		{
			GetDataAgainstOrder();
			tabControl1.TabPages.Remove(tabPage2);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDataAgainstOrder()
	{
		try
		{
			int organizationId = UserAccount.OrganizationId;
			int CompanyId = UserAccount.CompanyId;
			int FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			SaleOrderActivityDetail Order = ModernHistory.SaleOrder_Activity(organizationId, CompanyId, FinancialYearId, SaleOrderId);
			if (Order.SaleOrderInfoList.Count > 0)
			{
				txtPartyName.Text = Order.SaleOrderInfoList[0].CustomerName.ToString();
				OrderNo.Text = Order.SaleOrderInfoList[0].OrderNo.ToString();
				OrderNo1.Text = Order.SaleOrderInfoList[0].OrderNo.ToString();
				OrderDate.Text = Order.SaleOrderInfoList[0].OrderDate.ToShortDateString();
				OrderDate1.Text = Order.SaleOrderInfoList[0].OrderDate.ToShortDateString();
				ExpiryDate.Text = Order.SaleOrderInfoList[0].OrderExpiryDate.ToShortDateString();
				ExpiryDate1.Text = Order.SaleOrderInfoList[0].OrderExpiryDate.ToShortDateString();
				DeliveryTerm.Text = Order.SaleOrderInfoList[0].DeliveryTerm.ToString();
				DeliveryTerm1.Text = Order.SaleOrderInfoList[0].DeliveryTerm.ToString();
				ExpiryStatus.Text = Order.SaleOrderInfoList[0].ExpiryStatus.ToString();
				ExpiryStatus1.Text = Order.SaleOrderInfoList[0].ExpiryStatus.ToString();
				if (Order.SaleOrderInfoList[0].ExpiryStatus.ToString() != "Expired")
				{
					ExpiryStatus.BackColor = Color.Teal;
					ExpiryStatus1.BackColor = Color.Teal;
				}
				else
				{
					ExpiryStatus.BackColor = Color.Red;
					ExpiryStatus1.BackColor = Color.Red;
				}
				OrderStatus.Text = Order.SaleOrderInfoList[0].OrderStatus.ToString();
				OrderStatus1.Text = Order.SaleOrderInfoList[0].OrderStatus.ToString();
				if (OrderStatus.Text == "Open")
				{
					OrderStatus.BackColor = Color.Goldenrod;
					OrderStatus1.BackColor = Color.Goldenrod;
				}
				else if (OrderStatus.Text == "Complete")
				{
					OrderStatus.BackColor = Color.Teal;
					OrderStatus1.BackColor = Color.Teal;
				}
				else
				{
					OrderStatus.BackColor = Color.Red;
					OrderStatus1.BackColor = Color.Red;
				}
				OrderWeight.Text = Order.OrderTotalWeight.ToString("#,#;(#,#);0");
				OrderWeight1.Text = Order.OrderTotalWeight.ToString("#,#;(#,#);0");
				RecievedWeight.Text = Order.OrderReceivedWeight.ToString("#,#;(#,#);0");
				RecievedWeight1.Text = Order.OrderReceivedWeight.ToString("#,#;(#,#);0");
				BalanceWeight.Text = Order.OrderBalanceWeight.ToString("#,#;(#,#);0");
				BalanceWeight1.Text = Order.OrderBalanceWeight.ToString("#,#;(#,#);0");
				EntryUserName.Text = Order.SaleOrderInfoList[0].EntryUserName.ToString();
				EntryUser1.Text = Order.SaleOrderInfoList[0].EntryUserName.ToString();
				if (Order.SaleOrderInfoList[0].ModifyUser > 0)
				{
					lblModifyUser.Visible = true;
					ModifyUserName.Visible = true;
					ModifyUserName.Text = Order.SaleOrderInfoList[0].ModifyUserName.ToString();
					lblModifyUser1.Visible = true;
					ModifyUser1.Visible = true;
					ModifyUser1.Text = Order.SaleOrderInfoList[0].ModifyUserName.ToString();
				}
				else
				{
					lblModifyUser.Visible = false;
					ModifyUserName.Visible = false;
					lblModifyUser1.Visible = false;
					ModifyUser1.Visible = false;
				}
				if (Order.SaleOrderInfoList[0].PostUser > 0)
				{
					lblApprovalUser.Visible = true;
					ApprovalUserName.Visible = true;
					ApprovalUserName.Text = Order.SaleOrderInfoList[0].EntryUserName.ToString();
					lblApprovalUser1.Visible = true;
					ApprovalUser1.Visible = true;
					ApprovalUser1.Text = Order.SaleOrderInfoList[0].EntryUserName.ToString();
				}
				else
				{
					lblApprovalUser.Visible = false;
					ApprovalUserName.Visible = false;
					lblApprovalUser1.Visible = false;
					ApprovalUser1.Visible = false;
				}
				GrdOrderDetail.ClearStructure();
				OrderDetailIInfoPanel.Show();
				dtOrderDetail.Columns.Add("SaleOrderId");
				dtOrderDetail.Columns.Add("SupCustId");
				dtOrderDetail.Columns.Add("CustomerName");
				dtOrderDetail.Columns.Add("ItemId");
				dtOrderDetail.Columns.Add("StockAcId");
				dtOrderDetail.Columns.Add("ItemName");
				dtOrderDetail.Columns.Add("Crop");
				dtOrderDetail.Columns.Add("OrderItemQty", typeof(double));
				dtOrderDetail.Columns.Add("PackUom");
				dtOrderDetail.Columns.Add("NetWeight", typeof(double));
				dtOrderDetail.Columns.Add("OrderItemRate");
				dtOrderDetail.Columns.Add("RateUom");
				dtOrderDetail.Columns.Add("Amount", typeof(double));
				dtOrderDetail.Columns.Add("LabSampleNo");
				dtOrderDetail.Columns.Add("DetailRemarks");
				for (int m = 0; m < Order.SaleOrderInfoList.Count; m++)
				{
					dtOrderDetail.Rows.Add(Order.SaleOrderInfoList[m].SaleOrderId, Order.SaleOrderInfoList[m].OrderSupCustId, Order.SaleOrderInfoList[m].CustomerName, Order.SaleOrderInfoList[m].OrderItemId, Order.SaleOrderInfoList[m].StockAcId, Order.SaleOrderInfoList[m].ItemName, Order.SaleOrderInfoList[m].Crop, Order.SaleOrderInfoList[m].OrderItemQty, Order.SaleOrderInfoList[m].PackUom, Order.SaleOrderInfoList[m].NetWeight, Order.SaleOrderInfoList[m].OrderItemRate, Order.SaleOrderInfoList[m].RateUom, Order.SaleOrderInfoList[m].Amount, Order.SaleOrderInfoList[m].LabSampleNo, Order.SaleOrderInfoList[m].DetailRemarks);
				}
				GrdOrderDetail.DataSource = dtOrderDetail;
				GrdOrderDetail.RetrieveStructure();
				GridSettingForOrderDetail();
			}
			else
			{
				OrderDetailIInfoPanel.Hide();
			}
			if (Order.DeliveryOrderInfoList != null && Order.DeliveryOrderInfoList.Count > 0)
			{
				DeliveryOrderPanel.Show();
				GrdDeliveryOrder.ClearStructure();
				if (dtDoInfo.Columns.Count == 0)
				{
					dtDoInfo.Columns.Add("DeliveryOrderId");
					dtDoInfo.Columns.Add("DocNo");
					dtDoInfo.Columns.Add("DocDate");
					dtDoInfo.Columns.Add("DeliveryOrderType");
					dtDoInfo.Columns.Add("LoadingQty", typeof(double));
					dtDoInfo.Columns.Add("LoadingWeight", typeof(double));
					dtDoInfo.Columns.Add("PackingWeight", typeof(double));
					dtDoInfo.Columns.Add("GrossWeight", typeof(double));
					dtDoInfo.Columns.Add("EntryUserName");
					dtDoInfo.Columns.Add("ModifyUserName");
					dtDoInfo.Columns.Add("ApprovalUserName");
				}
				for (int l = 0; l < Order.DeliveryOrderInfoList.Count; l++)
				{
					dtDoInfo.Rows.Add(Order.DeliveryOrderInfoList[l].DeliveryOrderId, Order.DeliveryOrderInfoList[l].DocNo, Order.DeliveryOrderInfoList[l].DocDate.ToString("dd-MMM-yyyy"), Order.DeliveryOrderInfoList[l].DeliveryOrderType, Order.DeliveryOrderInfoList[l].LoadingQty, Order.DeliveryOrderInfoList[l].LoadingWeight, Order.DeliveryOrderInfoList[l].PackingWeight, Order.DeliveryOrderInfoList[l].GrossWeight, Order.DeliveryOrderInfoList[l].EntryUserName, Order.DeliveryOrderInfoList[l].ModifyUserName, Order.DeliveryOrderInfoList[l].ApprovalUserName);
				}
				if (dtDoInfo.Rows.Count > 0)
				{
					DeliveryOrderPanel.Show();
					GrdDeliveryOrder.DataSource = dtDoInfo;
					GrdDeliveryOrder.RetrieveStructure();
					GridSettingForDeliveryOrder();
				}
				else
				{
					GrdDeliveryOrder.ClearStructure();
					DeliveryOrderPanel.Hide();
				}
			}
			else
			{
				DeliveryOrderPanel.Hide();
			}
			if (Order.GatePassInfoList != null && Order.GatePassInfoList.Count > 0)
			{
				GrdGatePass.ClearStructure();
				GatePassDetailPanel.Show();
				dtGatePassessDetail.Columns.Add("GatePassId");
				dtGatePassessDetail.Columns.Add("GpSrNo");
				dtGatePassessDetail.Columns.Add("GpDate");
				dtGatePassessDetail.Columns.Add("VehicleNo");
				dtGatePassessDetail.Columns.Add("BiltyNo");
				dtGatePassessDetail.Columns.Add("VehicleInDate");
				dtGatePassessDetail.Columns.Add("VehicleOutDate");
				dtGatePassessDetail.Columns.Add("DoWeight", typeof(double));
				dtGatePassessDetail.Columns.Add("FactoryWeight", typeof(double));
				dtGatePassessDetail.Columns.Add("DifferenceWeight", typeof(double));
				dtGatePassessDetail.Columns.Add("EntryUserName");
				dtGatePassessDetail.Columns.Add("ModifyUserName");
				dtGatePassessDetail.Columns.Add("ApprovalUserName");
				for (int k = 0; k < Order.GatePassInfoList.Count; k++)
				{
					dtGatePassessDetail.Rows.Add(Order.GatePassInfoList[k].GatePassId, Order.GatePassInfoList[k].GpSrNo, Order.GatePassInfoList[k].GpDate.ToString("dd-MMM-yyyy"), Order.GatePassInfoList[k].VehicleNo, Order.GatePassInfoList[k].BiltyNo, Order.GatePassInfoList[k].VehicleInDate, Order.GatePassInfoList[k].VehicleOutDate, Order.GatePassInfoList[k].SupplierWeight, Order.GatePassInfoList[k].FactoryWeight, Order.GatePassInfoList[k].DifferenceWeight, Order.GatePassInfoList[k].EntryUserName, Order.GatePassInfoList[k].ModifyUserName, Order.GatePassInfoList[k].ApprovalUserName);
				}
				GrdGatePass.DataSource = dtGatePassessDetail;
				GrdGatePass.RetrieveStructure();
				GridSettingsForGatePass();
			}
			else
			{
				GatePassDetailPanel.Hide();
				GrdGatePass.ClearStructure();
			}
			if (Order.WeighBridgeInfoList != null && Order.WeighBridgeInfoList.Count > 0)
			{
				WeighBridgePanel.Show();
				if (dtweighBridgeDetail.Columns.Count == 0)
				{
					dtweighBridgeDetail.Columns.Add("GatePassId");
					dtweighBridgeDetail.Columns.Add("WbId");
					dtweighBridgeDetail.Columns.Add("GpSrNo");
					dtweighBridgeDetail.Columns.Add("TicketNo");
					dtweighBridgeDetail.Columns.Add("TicketDate");
					dtweighBridgeDetail.Columns.Add("FirstWeight", typeof(double));
					dtweighBridgeDetail.Columns.Add("SecondWeight", typeof(double));
					dtweighBridgeDetail.Columns.Add("NetWbWeight", typeof(double));
					dtweighBridgeDetail.Columns.Add("WbRemarks");
					dtweighBridgeDetail.Columns.Add("EntryUserName");
					dtweighBridgeDetail.Columns.Add("ModifyUserName");
				}
				for (int j2 = 0; j2 < Order.WeighBridgeInfoList.Count; j2++)
				{
					dtweighBridgeDetail.Rows.Add(Order.WeighBridgeInfoList[j2].GatePassId, Order.WeighBridgeInfoList[j2].WbId, Order.WeighBridgeInfoList[j2].GpSrNo, Order.WeighBridgeInfoList[j2].TicketNo, Order.WeighBridgeInfoList[j2].TicketDate.ToString("dd-MMM-yyyy"), Order.WeighBridgeInfoList[j2].FirstWeight, Order.WeighBridgeInfoList[j2].SecondWeight, Order.WeighBridgeInfoList[j2].NetWbWeight, Order.WeighBridgeInfoList[j2].WbRemarks, Order.WeighBridgeInfoList[j2].EntryUserName, Order.WeighBridgeInfoList[j2].ModifyUserName);
				}
				GrdWeighBridge.DataSource = dtweighBridgeDetail;
				GrdWeighBridge.RetrieveStructure();
				GridSettingsForWeighBridge();
			}
			else
			{
				WeighBridgePanel.Hide();
				GrdWeighBridge.ClearStructure();
			}
			if (Order.GoodsDispatchtNotesInfoList != null && Order.GoodsDispatchtNotesInfoList.Count > 0)
			{
				GdnDetailPanel.Show();
				if (dtGRNDetail.Columns.Count == 0)
				{
					dtGRNDetail.Columns.Add("GdnId");
					dtGRNDetail.Columns.Add("GpSrNo");
					dtGRNDetail.Columns.Add("GdnNo");
					dtGRNDetail.Columns.Add("GdnDate");
					dtGRNDetail.Columns.Add("FreightAmount", typeof(double));
					dtGRNDetail.Columns.Add("TotalQuantity", typeof(double));
					dtGRNDetail.Columns.Add("TotalGrossWeight", typeof(double));
					dtGRNDetail.Columns.Add("TotalNetWeight", typeof(double));
					dtGRNDetail.Columns.Add("TotalStockWeight", typeof(double));
					dtGRNDetail.Columns.Add("EntryUserName");
					dtGRNDetail.Columns.Add("ModifyUserName");
				}
				for (int n = 0; n < Order.GoodsDispatchtNotesInfoList.Count; n++)
				{
					dtGRNDetail.Rows.Add(Order.GoodsDispatchtNotesInfoList[n].GdnId, Order.GoodsDispatchtNotesInfoList[n].GpSrNo, Order.GoodsDispatchtNotesInfoList[n].DocNo, Order.GoodsDispatchtNotesInfoList[n].DocDate.ToString("dd-MMM-yyyy"), Order.GoodsDispatchtNotesInfoList[n].FreightAmount, Order.GoodsDispatchtNotesInfoList[n].ItemQty, Order.GoodsDispatchtNotesInfoList[n].GrossWeight, Order.GoodsDispatchtNotesInfoList[n].NetBillWeight, Order.GoodsDispatchtNotesInfoList[n].StockWeight, Order.GoodsDispatchtNotesInfoList[n].EntryUserName, Order.GoodsDispatchtNotesInfoList[n].ModifyUserName);
				}
				GrdGdnDetail.DataSource = dtGRNDetail;
				GrdGdnDetail.RetrieveStructure();
				GridSettingsForGRN();
			}
			else
			{
				GrdGdnDetail.ClearStructure();
				GdnDetailPanel.Hide();
			}
			if (Order.InvoiceInfoList != null && Order.InvoiceInfoList.Count > 0)
			{
				InvoiceDetailPanel.Show();
				if (dtInvoiceInfo.Columns.Count == 0)
				{
					dtInvoiceInfo.Columns.Add("InvoiceId");
					dtInvoiceInfo.Columns.Add("InvoiceNumber");
					dtInvoiceInfo.Columns.Add("InvoiceDate");
					dtInvoiceInfo.Columns.Add("GpNo");
					dtInvoiceInfo.Columns.Add("GdnNo");
					dtInvoiceInfo.Columns.Add("ItemAmount", typeof(double));
					dtInvoiceInfo.Columns.Add("CommAmount", typeof(double));
					dtInvoiceInfo.Columns.Add("Expenses");
					dtInvoiceInfo.Columns.Add("ChargetoProduct");
					dtInvoiceInfo.Columns.Add("NetItemAmount", typeof(double));
					dtInvoiceInfo.Columns.Add("InvoiceAmount", typeof(double));
					dtInvoiceInfo.Columns.Add("EntryUser");
					dtInvoiceInfo.Columns.Add("ModifyUser");
					dtInvoiceInfo.Columns.Add("ApprovalUser");
				}
				for (int j = 0; j < Order.InvoiceInfoList.Count; j++)
				{
					dtInvoiceInfo.Rows.Add(Order.InvoiceInfoList[j].SaleInvoiceId, Order.InvoiceInfoList[j].DocNo, Order.InvoiceInfoList[j].DocDate.ToString("dd-MMM-yyyy"), Order.InvoiceInfoList[j].GpSrNo, Order.InvoiceInfoList[j].GdnNo, Order.InvoiceInfoList[j].ItemAmount, Order.InvoiceInfoList[j].CommissionAmount, Order.InvoiceInfoList[j].OtherExpenses, Order.InvoiceInfoList[j].ChargeToProduct, Order.InvoiceInfoList[j].BillAmount, Order.InvoiceInfoList[j].InvoiceAmount, Order.InvoiceInfoList[j].EntryUserName, Order.InvoiceInfoList[j].ModifyUserName, Order.InvoiceInfoList[j].ApprovalUserName);
				}
				GrdInvoicesDetail.DataSource = dtInvoiceInfo;
				GrdInvoicesDetail.RetrieveStructure();
				GridSettingsForInvoices();
			}
			else
			{
				GrdInvoicesDetail.ClearStructure();
				InvoiceDetailPanel.Hide();
			}
			if (Order.WagesInfoList != null && Order.WagesInfoList.Count > 0)
			{
				WagesDetailPanel.Show();
				GrdWagesInfo.ClearStructure();
				if (dtWagesInfo.Columns.Count == 0)
				{
					dtWagesInfo.Columns.Add("Id");
					dtWagesInfo.Columns.Add("GdnId");
					dtWagesInfo.Columns.Add("GdnNumber");
					dtWagesInfo.Columns.Add("DocNo");
					dtWagesInfo.Columns.Add("DocDate");
					dtWagesInfo.Columns.Add("ContractorId");
					dtWagesInfo.Columns.Add("ContractorName");
					dtWagesInfo.Columns.Add("WagesAccountName");
					dtWagesInfo.Columns.Add("Quantity", typeof(double));
					dtWagesInfo.Columns.Add("PackSize");
					dtWagesInfo.Columns.Add("Rate");
					dtWagesInfo.Columns.Add("Weight", typeof(double));
					dtWagesInfo.Columns.Add("Amount", typeof(double));
					dtWagesInfo.Columns.Add("EntryUserName");
					dtWagesInfo.Columns.Add("ModifyUserName");
				}
				for (int i = 0; i < Order.WagesInfoList.Count; i++)
				{
					dtWagesInfo.Rows.Add(Order.WagesInfoList[i].Id, Order.WagesInfoList[i].GdnId, Order.WagesInfoList[i].GdnNumber, Order.WagesInfoList[i].DocNo, Order.WagesInfoList[i].DocDate.ToShortDateString(), Order.WagesInfoList[i].ContractorId, Order.WagesInfoList[i].ContractorName, Order.WagesInfoList[i].WagesAccountName, Order.WagesInfoList[i].Quantity, Order.WagesInfoList[i].PackSize, Order.WagesInfoList[i].Rate, Order.WagesInfoList[i].Weight, Order.WagesInfoList[i].Amount, Order.WagesInfoList[i].EntryUserName, Order.WagesInfoList[i].ModifyUserName);
				}
				GrdWagesInfo.DataSource = dtWagesInfo;
				GrdWagesInfo.RetrieveStructure();
				GridSettingsForWagesInformation();
			}
			else
			{
				WagesDetailPanel.Hide();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingForDeliveryOrder()
	{
		try
		{
			GrdDeliveryOrder.RootTable.Columns["LoadingQty"].AggregateFunction = (AggregateFunction)2;
			GrdDeliveryOrder.RootTable.Columns["LoadingWeight"].AggregateFunction = (AggregateFunction)2;
			GrdDeliveryOrder.RootTable.Columns["PackingWeight"].AggregateFunction = (AggregateFunction)2;
			GrdDeliveryOrder.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			GrdDeliveryOrder.RootTable.Columns["LoadingQty"].TextAlignment = (TextAlignment)3;
			GrdDeliveryOrder.RootTable.Columns["LoadingWeight"].TextAlignment = (TextAlignment)3;
			GrdDeliveryOrder.RootTable.Columns["PackingWeight"].TextAlignment = (TextAlignment)3;
			GrdDeliveryOrder.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			GrdDeliveryOrder.RootTable.Columns["LoadingQty"].FormatString = "#,#";
			GrdDeliveryOrder.RootTable.Columns["LoadingWeight"].FormatString = "#,#";
			GrdDeliveryOrder.RootTable.Columns["PackingWeight"].FormatString = "#,#";
			GrdDeliveryOrder.RootTable.Columns["GrossWeight"].FormatString = "#,#";
			GrdDeliveryOrder.RootTable.Columns["LoadingQty"].TotalFormatString = "#,##0.##";
			GrdDeliveryOrder.RootTable.Columns["LoadingWeight"].TotalFormatString = "#,##0.##";
			GrdDeliveryOrder.RootTable.Columns["PackingWeight"].TotalFormatString = "#,##0.##";
			GrdDeliveryOrder.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
			GrdDeliveryOrder.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			GrdDeliveryOrder.RootTable.Columns["DeliveryOrderId"].Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingForOrderDetail()
	{
		try
		{
			GrdOrderDetail.RootTable.Columns["OrderItemQty"].AggregateFunction = (AggregateFunction)2;
			GrdOrderDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			GrdOrderDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			GrdOrderDetail.RootTable.Columns["OrderItemQty"].TextAlignment = (TextAlignment)3;
			GrdOrderDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			GrdOrderDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			GrdOrderDetail.RootTable.Columns["OrderItemQty"].FormatString = "#,#";
			GrdOrderDetail.RootTable.Columns["NetWeight"].FormatString = "#,#";
			GrdOrderDetail.RootTable.Columns["Amount"].FormatString = "#,#";
			GrdOrderDetail.RootTable.Columns["OrderItemQty"].TotalFormatString = "#,##0.##";
			GrdOrderDetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			GrdOrderDetail.RootTable.Columns["Amount"].TotalFormatString = "#,##0.##";
			GrdOrderDetail.RootTable.Columns["SaleOrderId"].Visible = false;
			GrdOrderDetail.RootTable.Columns["SupCustId"].Visible = false;
			GrdOrderDetail.RootTable.Columns["ItemId"].Visible = false;
			GrdOrderDetail.RootTable.Columns["StockAcId"].Visible = false;
			GrdOrderDetail.RootTable.Columns["ItemName"].ColumnType = (ColumnType)5;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingsForGatePass()
	{
		try
		{
			GrdGatePass.RootTable.Columns["GatePassId"].Visible = false;
			GrdGatePass.RootTable.Columns["GpSrNo"].ColumnType = (ColumnType)5;
			GrdGatePass.RootTable.Columns["DoWeight"].AggregateFunction = (AggregateFunction)2;
			GrdGatePass.RootTable.Columns["FactoryWeight"].AggregateFunction = (AggregateFunction)2;
			GrdGatePass.RootTable.Columns["DifferenceWeight"].AggregateFunction = (AggregateFunction)2;
			GrdGatePass.RootTable.Columns["DoWeight"].TextAlignment = (TextAlignment)3;
			GrdGatePass.RootTable.Columns["FactoryWeight"].TextAlignment = (TextAlignment)3;
			GrdGatePass.RootTable.Columns["DifferenceWeight"].TextAlignment = (TextAlignment)3;
			GrdGatePass.RootTable.Columns["DoWeight"].FormatString = "#,#";
			GrdGatePass.RootTable.Columns["FactoryWeight"].FormatString = "#,#";
			GrdGatePass.RootTable.Columns["DifferenceWeight"].FormatString = "#,#;(#,#);0";
			GrdGatePass.RootTable.Columns["DoWeight"].TotalFormatString = "#,##0.##";
			GrdGatePass.RootTable.Columns["FactoryWeight"].TotalFormatString = "#,##0.##";
			GrdGatePass.RootTable.Columns["DifferenceWeight"].TotalFormatString = "#,##0.##";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingsForWeighBridge()
	{
		try
		{
			GrdWeighBridge.RootTable.Columns["GatePassId"].Visible = false;
			GrdWeighBridge.RootTable.Columns["WbId"].Visible = false;
			GrdWeighBridge.RootTable.Columns["TicketNo"].ColumnType = (ColumnType)5;
			GrdWeighBridge.RootTable.Columns["FirstWeight"].AggregateFunction = (AggregateFunction)2;
			GrdWeighBridge.RootTable.Columns["SecondWeight"].AggregateFunction = (AggregateFunction)2;
			GrdWeighBridge.RootTable.Columns["NetWbWeight"].AggregateFunction = (AggregateFunction)2;
			GrdWeighBridge.RootTable.Columns["FirstWeight"].TextAlignment = (TextAlignment)3;
			GrdWeighBridge.RootTable.Columns["SecondWeight"].TextAlignment = (TextAlignment)3;
			GrdWeighBridge.RootTable.Columns["NetWbWeight"].TextAlignment = (TextAlignment)3;
			GrdWeighBridge.RootTable.Columns["FirstWeight"].FormatString = "#,#";
			GrdWeighBridge.RootTable.Columns["SecondWeight"].FormatString = "#,#";
			GrdWeighBridge.RootTable.Columns["NetWbWeight"].FormatString = "#,#";
			GrdWeighBridge.RootTable.Columns["FirstWeight"].TotalFormatString = "#,##0.##";
			GrdWeighBridge.RootTable.Columns["SecondWeight"].TotalFormatString = "#,##0.##";
			GrdWeighBridge.RootTable.Columns["NetWbWeight"].TotalFormatString = "#,##0.##";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingsForGRN()
	{
		try
		{
			GrdGdnDetail.RootTable.Columns["GdnId"].Visible = false;
			GrdGdnDetail.RootTable.Columns["GdnNo"].ColumnType = (ColumnType)5;
			GrdGdnDetail.RootTable.Columns["FreightAmount"].AggregateFunction = (AggregateFunction)2;
			GrdGdnDetail.RootTable.Columns["TotalQuantity"].AggregateFunction = (AggregateFunction)2;
			GrdGdnDetail.RootTable.Columns["TotalGrossWeight"].AggregateFunction = (AggregateFunction)2;
			GrdGdnDetail.RootTable.Columns["TotalNetWeight"].AggregateFunction = (AggregateFunction)2;
			GrdGdnDetail.RootTable.Columns["TotalStockWeight"].AggregateFunction = (AggregateFunction)2;
			GrdGdnDetail.RootTable.Columns["FreightAmount"].TextAlignment = (TextAlignment)3;
			GrdGdnDetail.RootTable.Columns["TotalQuantity"].TextAlignment = (TextAlignment)3;
			GrdGdnDetail.RootTable.Columns["TotalGrossWeight"].TextAlignment = (TextAlignment)3;
			GrdGdnDetail.RootTable.Columns["TotalNetWeight"].TextAlignment = (TextAlignment)3;
			GrdGdnDetail.RootTable.Columns["TotalStockWeight"].TextAlignment = (TextAlignment)3;
			GrdGdnDetail.RootTable.Columns["FreightAmount"].FormatString = "#,#";
			GrdGdnDetail.RootTable.Columns["TotalQuantity"].FormatString = "#,#";
			GrdGdnDetail.RootTable.Columns["TotalGrossWeight"].FormatString = "#,#";
			GrdGdnDetail.RootTable.Columns["TotalNetWeight"].FormatString = "#,#";
			GrdGdnDetail.RootTable.Columns["TotalStockWeight"].FormatString = "#,#";
			GrdGdnDetail.RootTable.Columns["FreightAmount"].TotalFormatString = "#,##0.##";
			GrdGdnDetail.RootTable.Columns["TotalQuantity"].TotalFormatString = "#,##0.##";
			GrdGdnDetail.RootTable.Columns["TotalGrossWeight"].TotalFormatString = "#,##0.##";
			GrdGdnDetail.RootTable.Columns["TotalNetWeight"].TotalFormatString = "#,##0.##";
			GrdGdnDetail.RootTable.Columns["TotalStockWeight"].TotalFormatString = "#,##0.##";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingsForWagesInformation()
	{
		try
		{
			GrdWagesInfo.RootTable.Columns["Id"].Visible = false;
			GrdWagesInfo.RootTable.Columns["GdnId"].Visible = false;
			GrdWagesInfo.RootTable.Columns["ContractorId"].Visible = false;
			GrdWagesInfo.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			GrdWagesInfo.RootTable.Columns["ContractorName"].ColumnType = (ColumnType)5;
			GrdWagesInfo.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			GrdWagesInfo.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			GrdWagesInfo.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			GrdWagesInfo.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			GrdWagesInfo.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			GrdWagesInfo.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			GrdWagesInfo.RootTable.Columns["Amount"].FormatString = "0,0";
			GrdWagesInfo.RootTable.Columns["Quantity"].FormatString = "0,0";
			GrdWagesInfo.RootTable.Columns["Weight"].FormatString = "0,0";
			GrdWagesInfo.RootTable.Columns["Amount"].TotalFormatString = "#,##0.##";
			GrdWagesInfo.RootTable.Columns["Quantity"].TotalFormatString = "#,##0.##";
			GrdWagesInfo.RootTable.Columns["Weight"].TotalFormatString = "#,##0.##";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingsForInvoices()
	{
		try
		{
			GrdInvoicesDetail.RootTable.Columns["InvoiceId"].Visible = false;
			GrdInvoicesDetail.RootTable.Columns["InvoiceNumber"].ColumnType = (ColumnType)5;
			GrdInvoicesDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			GrdInvoicesDetail.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			GrdInvoicesDetail.RootTable.Columns["NetItemAmount"].AggregateFunction = (AggregateFunction)2;
			GrdInvoicesDetail.RootTable.Columns["InvoiceAmount"].AggregateFunction = (AggregateFunction)2;
			GrdInvoicesDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			GrdInvoicesDetail.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			GrdInvoicesDetail.RootTable.Columns["NetItemAmount"].TextAlignment = (TextAlignment)3;
			GrdInvoicesDetail.RootTable.Columns["InvoiceAmount"].TextAlignment = (TextAlignment)3;
			GrdInvoicesDetail.RootTable.Columns["ItemAmount"].FormatString = "#,#";
			GrdInvoicesDetail.RootTable.Columns["CommAmount"].FormatString = "#,#";
			GrdInvoicesDetail.RootTable.Columns["NetItemAmount"].FormatString = "#,#";
			GrdInvoicesDetail.RootTable.Columns["InvoiceAmount"].FormatString = "#,#";
			GrdInvoicesDetail.RootTable.Columns["ItemAmount"].TotalFormatString = "#,##0.##";
			GrdInvoicesDetail.RootTable.Columns["CommAmount"].TotalFormatString = "#,##0.##";
			GrdInvoicesDetail.RootTable.Columns["NetItemAmount"].TotalFormatString = "#,##0.##";
			GrdInvoicesDetail.RootTable.Columns["InvoiceAmount"].TotalFormatString = "#,##0.##";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OrderNo_Click(object sender, EventArgs e)
	{
		try
		{
			if (GrdOrderDetail.GetRows().Length != 0)
			{
				int PrintId = Conversion.ToInt(GrdOrderDetail.CurrentRow.Cells["SaleOrderId"].Value);
				if (PrintId > 0)
				{
					CommonServices.SaleOrderReports273(PrintId);
					return;
				}
				throw new Exception("Record Not Found For Display");
			}
			throw new Exception("Record Not Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdOrderDetail_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow currentRow = GrdOrderDetail.CurrentRow;
			Conversion.ToInt(currentRow.Cells["StockAcId"].Value);
			int ItemId = Conversion.ToInt(currentRow.Cells["ItemId"].Value);
			if (e.Column.Key == "ItemName")
			{
				CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(ItemId, clsGlobalVariables.ActiveYr.Start_Period, DateTime.Now);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdDeliveryOrder_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int DeliveryOrderId = Conversion.ToInt(GrdDeliveryOrder.CurrentRow.Cells["DeliveryOrderId"].Value);
			if (e.Column.Key == "DocNo")
			{
				CommonServices.InvDeliveryOrderSlip(DeliveryOrderId, 84);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdGatePass_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int GatePassId = Conversion.ToInt(GrdGatePass.CurrentRow.Cells["GatePassId"].Value);
			if (e.Column.Key == "GpSrNo")
			{
				CommonServices.GatePassOutwardSlipandRegister(GatePassId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdWeighBridge_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int WbId = Conversion.ToInt(GrdWeighBridge.CurrentRow.Cells["WbId"].Value);
			if (e.Column.Key == "TicketNo")
			{
				CommonServices.WbTransactionSlip280(WbId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdGrnDetail_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int GdnId = Conversion.ToInt(GrdGdnDetail.CurrentRow.Cells["GdnId"].Value);
			if (e.Column.Key == "GdnNo")
			{
				CommonServices.InvGdnSlip260(GdnId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdWagesInfo_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow currentRow = GrdWagesInfo.CurrentRow;
			int Id = Conversion.ToInt(currentRow.Cells["Id"].Value);
			int ContractorId = Conversion.ToInt(currentRow.Cells["ContractorId"].Value);
			if (e.Column.Key == "DocNo")
			{
				CommonServices.ContractorWagesBill_SlipandRegister_002(Id);
			}
			if (e.Column.Key == "ContractorName")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(ContractorId, clsGlobalVariables.ActiveYr.Start_Period, DateTime.Now);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdInvoicesDetail_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int InvoiceId = Conversion.ToInt(GrdInvoicesDetail.CurrentRow.Cells["InvoiceId"].Value);
			if (e.Column.Key == "InvoiceNumber")
			{
				new SaleInvoiceReverseActivity(UserAccount, InvoiceId).Show();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdOrderDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = GrdOrderDetail.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				Conversion.ToInt(item.Cells["StockAcId"].Value);
				int ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
				if (GrdOrderDetail.CurrentColumn != null && GrdOrderDetail.CurrentColumn.Key == "ItemName")
				{
					CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(ItemId, clsGlobalVariables.ActiveYr.Start_Period, DateTime.Now);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdDeliveryOrder_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = GrdDeliveryOrder.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int DeliveryOrderId = Conversion.ToInt(item.Cells["DeliveryOrderId"].Value);
				if (GrdDeliveryOrder.CurrentColumn != null && GrdDeliveryOrder.CurrentColumn.Key == "DocNo")
				{
					CommonServices.InvDeliveryOrderSlip(DeliveryOrderId, 84);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdGatePass_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = GrdGatePass.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int GatePassId = Conversion.ToInt(item.Cells["GatePassId"].Value);
				if (GrdGatePass.CurrentColumn != null && GrdGatePass.CurrentColumn.Key == "GpSrNo")
				{
					CommonServices.GatePassOutwardSlipandRegister(GatePassId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdWeighBridge_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = GrdWeighBridge.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int WbId = Conversion.ToInt(item.Cells["WbId"].Value);
				if (GrdWeighBridge.CurrentColumn != null && GrdWeighBridge.CurrentColumn.Key == "TicketNo")
				{
					CommonServices.WbTransactionSlip280(WbId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdGdnDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = GrdGdnDetail.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int GdnId = Conversion.ToInt(item.Cells["GdnId"].Value);
				if (GrdGdnDetail.CurrentColumn != null && GrdGdnDetail.CurrentColumn.Key == "GdnNo")
				{
					CommonServices.InvGdnSlip260(GdnId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdWagesInfo_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = GrdWagesInfo.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int Id = Conversion.ToInt(item.Cells["Id"].Value);
			int ContractorId = Conversion.ToInt(item.Cells["ContractorId"].Value);
			if (GrdWagesInfo.CurrentColumn != null)
			{
				if (GrdWagesInfo.CurrentColumn.Key == "DocNo")
				{
					CommonServices.ContractorWagesBill_SlipandRegister_002(Id);
				}
				else if (GrdWagesInfo.CurrentColumn.Key == "ContractorName")
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(ContractorId, clsGlobalVariables.ActiveYr.Start_Period, DateTime.Now);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdInvoicesDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = GrdInvoicesDetail.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int InvoiceId = Conversion.ToInt(item.Cells["InvoiceId"].Value);
				if (GrdInvoicesDetail.CurrentColumn != null && GrdInvoicesDetail.CurrentColumn.Key == "InvoiceNumber")
				{
					new SaleInvoiceReverseActivity(UserAccount, InvoiceId).Show();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Expected O, but got Unknown
		//IL_095a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0964: Expected O, but got Unknown
		//IL_0bd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bdc: Expected O, but got Unknown
		//IL_0e49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e53: Expected O, but got Unknown
		//IL_10c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ca: Expected O, but got Unknown
		//IL_1339: Unknown result type (might be due to invalid IL or missing references)
		//IL_1343: Expected O, but got Unknown
		//IL_15af: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b9: Expected O, but got Unknown
		//IL_1a05: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a0f: Expected O, but got Unknown
		GridEXLayout GrdInvoicesDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdWagesInfo_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdGdnDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdWeighBridge_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdGatePass_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdDeliveryOrder_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdOrderDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout gridEX1_DesignTimeLayout = new GridEXLayout();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.InvoiceDetailPanel = new System.Windows.Forms.Panel();
		this.GrdInvoicesDetail = new GridEX();
		this.label23 = new System.Windows.Forms.Label();
		this.WagesDetailPanel = new System.Windows.Forms.Panel();
		this.GrdWagesInfo = new GridEX();
		this.label22 = new System.Windows.Forms.Label();
		this.GdnDetailPanel = new System.Windows.Forms.Panel();
		this.GrdGdnDetail = new GridEX();
		this.label21 = new System.Windows.Forms.Label();
		this.WeighBridgePanel = new System.Windows.Forms.Panel();
		this.GrdWeighBridge = new GridEX();
		this.label17 = new System.Windows.Forms.Label();
		this.GatePassDetailPanel = new System.Windows.Forms.Panel();
		this.GrdGatePass = new GridEX();
		this.label16 = new System.Windows.Forms.Label();
		this.DeliveryOrderPanel = new System.Windows.Forms.Panel();
		this.GrdDeliveryOrder = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label11 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.OrderDetailIInfoPanel = new System.Windows.Forms.Panel();
		this.GrdOrderDetail = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.txtPartyName = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ApprovalUserName = new System.Windows.Forms.Label();
		this.ModifyUserName = new System.Windows.Forms.Label();
		this.EntryUserName = new System.Windows.Forms.Label();
		this.lblApprovalUser = new System.Windows.Forms.Label();
		this.lblModifyUser = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.BalanceWeight = new System.Windows.Forms.Label();
		this.RecievedWeight = new System.Windows.Forms.Label();
		this.OrderWeight = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.ExpiryStatus = new System.Windows.Forms.Label();
		this.OrderStatus = new System.Windows.Forms.Label();
		this.DeliveryTerm = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.ExpiryDate = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.OrderDate = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.OrderNo = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel15 = new System.Windows.Forms.Panel();
		this.gridEX1 = new GridEX();
		this.label19 = new System.Windows.Forms.Label();
		this.panel12 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.panel14 = new System.Windows.Forms.Panel();
		this.ApprovalUser1 = new System.Windows.Forms.Label();
		this.ModifyUser1 = new System.Windows.Forms.Label();
		this.EntryUser1 = new System.Windows.Forms.Label();
		this.lblApprovalUser1 = new System.Windows.Forms.Label();
		this.lblModifyUser1 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.BalanceWeight1 = new System.Windows.Forms.Label();
		this.RecievedWeight1 = new System.Windows.Forms.Label();
		this.OrderWeight1 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.ExpiryStatus1 = new System.Windows.Forms.Label();
		this.OrderStatus1 = new System.Windows.Forms.Label();
		this.DeliveryTerm1 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.ExpiryDate1 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.OrderDate1 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.OrderNo1 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel5.SuspendLayout();
		this.InvoiceDetailPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdInvoicesDetail).BeginInit();
		this.WagesDetailPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdWagesInfo).BeginInit();
		this.GdnDetailPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdGdnDetail).BeginInit();
		this.WeighBridgePanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdWeighBridge).BeginInit();
		this.GatePassDetailPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdGatePass).BeginInit();
		this.DeliveryOrderPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdDeliveryOrder).BeginInit();
		this.panel8.SuspendLayout();
		this.panel6.SuspendLayout();
		this.OrderDetailIInfoPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdOrderDetail).BeginInit();
		this.panel4.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel15.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.gridEX1).BeginInit();
		this.panel12.SuspendLayout();
		this.panel13.SuspendLayout();
		this.panel14.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1370, 749);
		this.tabControl1.TabIndex = 0;
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.OrderDetailIInfoPanel);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1362, 723);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Grid View";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel5.AutoScroll = true;
		this.panel5.Controls.Add(this.InvoiceDetailPanel);
		this.panel5.Controls.Add(this.WagesDetailPanel);
		this.panel5.Controls.Add(this.GdnDetailPanel);
		this.panel5.Controls.Add(this.WeighBridgePanel);
		this.panel5.Controls.Add(this.GatePassDetailPanel);
		this.panel5.Controls.Add(this.DeliveryOrderPanel);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 262);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1356, 458);
		this.panel5.TabIndex = 2;
		this.InvoiceDetailPanel.Controls.Add((System.Windows.Forms.Control)(object)this.GrdInvoicesDetail);
		this.InvoiceDetailPanel.Controls.Add(this.label23);
		this.InvoiceDetailPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.InvoiceDetailPanel.Location = new System.Drawing.Point(0, 938);
		this.InvoiceDetailPanel.Name = "InvoiceDetailPanel";
		this.InvoiceDetailPanel.Size = new System.Drawing.Size(1339, 180);
		this.InvoiceDetailPanel.TabIndex = 7;
		this.GrdInvoicesDetail.AllowEdit = (InheritableBoolean)2;
		this.GrdInvoicesDetail.AlternatingColors = true;
		((ScrollableBase)this.GrdInvoicesDetail).BackColor = System.Drawing.Color.White;
		this.GrdInvoicesDetail.ColumnAutoResize = true;
		GrdInvoicesDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdInvoicesDetail.DesignTimeLayout = GrdInvoicesDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdInvoicesDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdInvoicesDetail.DynamicFiltering = true;
		this.GrdInvoicesDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdInvoicesDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdInvoicesDetail.GroupByBoxVisible = false;
		this.GrdInvoicesDetail.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.GrdInvoicesDetail).Location = new System.Drawing.Point(0, 33);
		((System.Windows.Forms.Control)(object)this.GrdInvoicesDetail).Name = "GrdInvoicesDetail";
		((ScrollableBase)this.GrdInvoicesDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdInvoicesDetail).Size = new System.Drawing.Size(1339, 147);
		((System.Windows.Forms.Control)(object)this.GrdInvoicesDetail).TabIndex = 8;
		this.GrdInvoicesDetail.TotalRow = (InheritableBoolean)1;
		this.GrdInvoicesDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdInvoicesDetail).VisualStyle = (VisualStyle)4;
		this.GrdInvoicesDetail.LinkClicked += new ColumnActionEventHandler(GrdInvoicesDetail_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdInvoicesDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdInvoicesDetail_KeyDown);
		this.label23.BackColor = System.Drawing.Color.Teal;
		this.label23.Dock = System.Windows.Forms.DockStyle.Top;
		this.label23.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(0, 0);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(1339, 33);
		this.label23.TabIndex = 6;
		this.label23.Text = "Invoices Detail";
		this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.WagesDetailPanel.Controls.Add((System.Windows.Forms.Control)(object)this.GrdWagesInfo);
		this.WagesDetailPanel.Controls.Add(this.label22);
		this.WagesDetailPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.WagesDetailPanel.Location = new System.Drawing.Point(0, 758);
		this.WagesDetailPanel.Name = "WagesDetailPanel";
		this.WagesDetailPanel.Size = new System.Drawing.Size(1339, 180);
		this.WagesDetailPanel.TabIndex = 6;
		this.GrdWagesInfo.AllowEdit = (InheritableBoolean)2;
		this.GrdWagesInfo.AlternatingColors = true;
		((ScrollableBase)this.GrdWagesInfo).BackColor = System.Drawing.Color.White;
		this.GrdWagesInfo.ColumnAutoResize = true;
		GrdWagesInfo_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdWagesInfo.DesignTimeLayout = GrdWagesInfo_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdWagesInfo).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdWagesInfo.DynamicFiltering = true;
		this.GrdWagesInfo.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdWagesInfo).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdWagesInfo.GroupByBoxVisible = false;
		this.GrdWagesInfo.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.GrdWagesInfo).Location = new System.Drawing.Point(0, 33);
		((System.Windows.Forms.Control)(object)this.GrdWagesInfo).Name = "GrdWagesInfo";
		((ScrollableBase)this.GrdWagesInfo).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdWagesInfo).Size = new System.Drawing.Size(1339, 147);
		((System.Windows.Forms.Control)(object)this.GrdWagesInfo).TabIndex = 9;
		this.GrdWagesInfo.TotalRow = (InheritableBoolean)1;
		this.GrdWagesInfo.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdWagesInfo).VisualStyle = (VisualStyle)4;
		this.GrdWagesInfo.LinkClicked += new ColumnActionEventHandler(GrdWagesInfo_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdWagesInfo).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdWagesInfo_KeyDown);
		this.label22.BackColor = System.Drawing.Color.Teal;
		this.label22.Dock = System.Windows.Forms.DockStyle.Top;
		this.label22.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.White;
		this.label22.Location = new System.Drawing.Point(0, 0);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(1339, 33);
		this.label22.TabIndex = 5;
		this.label22.Text = "Wages Information";
		this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.GdnDetailPanel.Controls.Add((System.Windows.Forms.Control)(object)this.GrdGdnDetail);
		this.GdnDetailPanel.Controls.Add(this.label21);
		this.GdnDetailPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.GdnDetailPanel.Location = new System.Drawing.Point(0, 578);
		this.GdnDetailPanel.Name = "GdnDetailPanel";
		this.GdnDetailPanel.Size = new System.Drawing.Size(1339, 180);
		this.GdnDetailPanel.TabIndex = 4;
		this.GrdGdnDetail.AllowEdit = (InheritableBoolean)2;
		this.GrdGdnDetail.AlternatingColors = true;
		((ScrollableBase)this.GrdGdnDetail).BackColor = System.Drawing.Color.White;
		this.GrdGdnDetail.ColumnAutoResize = true;
		GrdGdnDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdGdnDetail.DesignTimeLayout = GrdGdnDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdGdnDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdGdnDetail.DynamicFiltering = true;
		this.GrdGdnDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdGdnDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdGdnDetail.GroupByBoxVisible = false;
		this.GrdGdnDetail.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.GrdGdnDetail).Location = new System.Drawing.Point(0, 33);
		((System.Windows.Forms.Control)(object)this.GrdGdnDetail).Name = "GrdGdnDetail";
		((ScrollableBase)this.GrdGdnDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdGdnDetail).Size = new System.Drawing.Size(1339, 147);
		((System.Windows.Forms.Control)(object)this.GrdGdnDetail).TabIndex = 7;
		this.GrdGdnDetail.TotalRow = (InheritableBoolean)1;
		this.GrdGdnDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdGdnDetail).VisualStyle = (VisualStyle)4;
		this.GrdGdnDetail.LinkClicked += new ColumnActionEventHandler(GrdGrnDetail_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdGdnDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdGdnDetail_KeyDown);
		this.label21.BackColor = System.Drawing.Color.Teal;
		this.label21.Dock = System.Windows.Forms.DockStyle.Top;
		this.label21.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.White;
		this.label21.Location = new System.Drawing.Point(0, 0);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(1339, 33);
		this.label21.TabIndex = 4;
		this.label21.Text = "GDN Detail";
		this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.WeighBridgePanel.Controls.Add((System.Windows.Forms.Control)(object)this.GrdWeighBridge);
		this.WeighBridgePanel.Controls.Add(this.label17);
		this.WeighBridgePanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.WeighBridgePanel.Location = new System.Drawing.Point(0, 398);
		this.WeighBridgePanel.Name = "WeighBridgePanel";
		this.WeighBridgePanel.Size = new System.Drawing.Size(1339, 180);
		this.WeighBridgePanel.TabIndex = 3;
		this.GrdWeighBridge.AllowEdit = (InheritableBoolean)2;
		this.GrdWeighBridge.AlternatingColors = true;
		((ScrollableBase)this.GrdWeighBridge).BackColor = System.Drawing.Color.White;
		this.GrdWeighBridge.ColumnAutoResize = true;
		GrdWeighBridge_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdWeighBridge.DesignTimeLayout = GrdWeighBridge_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdWeighBridge).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdWeighBridge.DynamicFiltering = true;
		this.GrdWeighBridge.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdWeighBridge).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdWeighBridge.GroupByBoxVisible = false;
		this.GrdWeighBridge.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.GrdWeighBridge).Location = new System.Drawing.Point(0, 36);
		((System.Windows.Forms.Control)(object)this.GrdWeighBridge).Name = "GrdWeighBridge";
		((ScrollableBase)this.GrdWeighBridge).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdWeighBridge).Size = new System.Drawing.Size(1339, 144);
		((System.Windows.Forms.Control)(object)this.GrdWeighBridge).TabIndex = 6;
		this.GrdWeighBridge.TotalRow = (InheritableBoolean)1;
		this.GrdWeighBridge.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdWeighBridge).VisualStyle = (VisualStyle)4;
		this.GrdWeighBridge.LinkClicked += new ColumnActionEventHandler(GrdWeighBridge_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdWeighBridge).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdWeighBridge_KeyDown);
		this.label17.BackColor = System.Drawing.Color.Teal;
		this.label17.Dock = System.Windows.Forms.DockStyle.Top;
		this.label17.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(0, 0);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(1339, 36);
		this.label17.TabIndex = 3;
		this.label17.Text = "Weigh Bridge Detail";
		this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.GatePassDetailPanel.Controls.Add((System.Windows.Forms.Control)(object)this.GrdGatePass);
		this.GatePassDetailPanel.Controls.Add(this.label16);
		this.GatePassDetailPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.GatePassDetailPanel.Location = new System.Drawing.Point(0, 218);
		this.GatePassDetailPanel.Name = "GatePassDetailPanel";
		this.GatePassDetailPanel.Size = new System.Drawing.Size(1339, 180);
		this.GatePassDetailPanel.TabIndex = 2;
		this.GrdGatePass.AllowEdit = (InheritableBoolean)2;
		this.GrdGatePass.AlternatingColors = true;
		((ScrollableBase)this.GrdGatePass).BackColor = System.Drawing.Color.White;
		this.GrdGatePass.ColumnAutoResize = true;
		GrdGatePass_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdGatePass.DesignTimeLayout = GrdGatePass_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdGatePass).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdGatePass.DynamicFiltering = true;
		this.GrdGatePass.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdGatePass).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdGatePass.GroupByBoxVisible = false;
		this.GrdGatePass.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.GrdGatePass).Location = new System.Drawing.Point(0, 33);
		((System.Windows.Forms.Control)(object)this.GrdGatePass).Name = "GrdGatePass";
		((ScrollableBase)this.GrdGatePass).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdGatePass).Size = new System.Drawing.Size(1339, 147);
		((System.Windows.Forms.Control)(object)this.GrdGatePass).TabIndex = 5;
		this.GrdGatePass.TotalRow = (InheritableBoolean)1;
		this.GrdGatePass.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdGatePass).VisualStyle = (VisualStyle)4;
		this.GrdGatePass.LinkClicked += new ColumnActionEventHandler(GrdGatePass_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdGatePass).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdGatePass_KeyDown);
		this.label16.BackColor = System.Drawing.Color.Teal;
		this.label16.Dock = System.Windows.Forms.DockStyle.Top;
		this.label16.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(0, 0);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(1339, 33);
		this.label16.TabIndex = 2;
		this.label16.Text = "Gate Passes Detail";
		this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.DeliveryOrderPanel.Controls.Add((System.Windows.Forms.Control)(object)this.GrdDeliveryOrder);
		this.DeliveryOrderPanel.Controls.Add(this.panel8);
		this.DeliveryOrderPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.DeliveryOrderPanel.Location = new System.Drawing.Point(0, 38);
		this.DeliveryOrderPanel.Name = "DeliveryOrderPanel";
		this.DeliveryOrderPanel.Size = new System.Drawing.Size(1339, 180);
		this.DeliveryOrderPanel.TabIndex = 1;
		this.GrdDeliveryOrder.AllowEdit = (InheritableBoolean)2;
		this.GrdDeliveryOrder.AlternatingColors = true;
		((ScrollableBase)this.GrdDeliveryOrder).BackColor = System.Drawing.Color.White;
		this.GrdDeliveryOrder.ColumnAutoResize = true;
		GrdDeliveryOrder_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdDeliveryOrder.DesignTimeLayout = GrdDeliveryOrder_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdDeliveryOrder).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdDeliveryOrder.DynamicFiltering = true;
		this.GrdDeliveryOrder.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdDeliveryOrder).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdDeliveryOrder.GroupByBoxVisible = false;
		this.GrdDeliveryOrder.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.GrdDeliveryOrder).Location = new System.Drawing.Point(0, 36);
		((System.Windows.Forms.Control)(object)this.GrdDeliveryOrder).Name = "GrdDeliveryOrder";
		((ScrollableBase)this.GrdDeliveryOrder).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdDeliveryOrder).Size = new System.Drawing.Size(1339, 144);
		((System.Windows.Forms.Control)(object)this.GrdDeliveryOrder).TabIndex = 4;
		this.GrdDeliveryOrder.TotalRow = (InheritableBoolean)1;
		this.GrdDeliveryOrder.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdDeliveryOrder).VisualStyle = (VisualStyle)4;
		this.GrdDeliveryOrder.LinkClicked += new ColumnActionEventHandler(GrdDeliveryOrder_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdDeliveryOrder).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdDeliveryOrder_KeyDown);
		this.panel8.Controls.Add(this.label15);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1339, 36);
		this.panel8.TabIndex = 0;
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Dock = System.Windows.Forms.DockStyle.Top;
		this.label15.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(0, 0);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(1339, 33);
		this.label15.TabIndex = 1;
		this.label15.Text = "Delivery Order Detail";
		this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel6.Controls.Add(this.label11);
		this.panel6.Controls.Add(this.label10);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1339, 38);
		this.panel6.TabIndex = 0;
		this.label11.BackColor = System.Drawing.Color.Teal;
		this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label11.ForeColor = System.Drawing.Color.Black;
		this.label11.Location = new System.Drawing.Point(5, 32);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(1220, 2);
		this.label11.TabIndex = 2;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Calibri", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.Teal;
		this.label10.Location = new System.Drawing.Point(4, 5);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(259, 26);
		this.label10.TabIndex = 1;
		this.label10.Text = "Order Recieving Information";
		this.OrderDetailIInfoPanel.Controls.Add((System.Windows.Forms.Control)(object)this.GrdOrderDetail);
		this.OrderDetailIInfoPanel.Controls.Add(this.panel4);
		this.OrderDetailIInfoPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.OrderDetailIInfoPanel.Location = new System.Drawing.Point(3, 122);
		this.OrderDetailIInfoPanel.Name = "OrderDetailIInfoPanel";
		this.OrderDetailIInfoPanel.Size = new System.Drawing.Size(1356, 140);
		this.OrderDetailIInfoPanel.TabIndex = 1;
		this.GrdOrderDetail.AllowEdit = (InheritableBoolean)2;
		this.GrdOrderDetail.AlternatingColors = true;
		((ScrollableBase)this.GrdOrderDetail).BackColor = System.Drawing.Color.White;
		this.GrdOrderDetail.ColumnAutoResize = true;
		GrdOrderDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdOrderDetail.DesignTimeLayout = GrdOrderDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdOrderDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdOrderDetail.DynamicFiltering = true;
		this.GrdOrderDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdOrderDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdOrderDetail.GroupByBoxVisible = false;
		this.GrdOrderDetail.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.GrdOrderDetail).Location = new System.Drawing.Point(0, 31);
		((System.Windows.Forms.Control)(object)this.GrdOrderDetail).Name = "GrdOrderDetail";
		((ScrollableBase)this.GrdOrderDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdOrderDetail).Size = new System.Drawing.Size(1356, 109);
		((System.Windows.Forms.Control)(object)this.GrdOrderDetail).TabIndex = 3;
		this.GrdOrderDetail.TotalRow = (InheritableBoolean)1;
		this.GrdOrderDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdOrderDetail).VisualStyle = (VisualStyle)4;
		this.GrdOrderDetail.LinkClicked += new ColumnActionEventHandler(GrdOrderDetail_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdOrderDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdOrderDetail_KeyDown);
		this.panel4.Controls.Add(this.label9);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1356, 31);
		this.panel4.TabIndex = 0;
		this.label9.BackColor = System.Drawing.Color.Teal;
		this.label9.Dock = System.Windows.Forms.DockStyle.Top;
		this.label9.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(0, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(1356, 34);
		this.label9.TabIndex = 0;
		this.label9.Text = "Order Detail Information";
		this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel1.Controls.Add(this.txtPartyName);
		this.panel1.Controls.Add(this.label24);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Controls.Add(this.label2);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1356, 119);
		this.panel1.TabIndex = 0;
		this.txtPartyName.AutoSize = true;
		this.txtPartyName.BackColor = System.Drawing.Color.Teal;
		this.txtPartyName.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPartyName.ForeColor = System.Drawing.Color.White;
		this.txtPartyName.Location = new System.Drawing.Point(341, 3);
		this.txtPartyName.Name = "txtPartyName";
		this.txtPartyName.Size = new System.Drawing.Size(174, 23);
		this.txtPartyName.TabIndex = 25;
		this.txtPartyName.Text = "Load & Factory Weight";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(229, 3);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(112, 23);
		this.label24.TabIndex = 24;
		this.label24.Text = "Party Name: ";
		this.panel2.Controls.Add(this.ApprovalUserName);
		this.panel2.Controls.Add(this.ModifyUserName);
		this.panel2.Controls.Add(this.EntryUserName);
		this.panel2.Controls.Add(this.lblApprovalUser);
		this.panel2.Controls.Add(this.lblModifyUser);
		this.panel2.Controls.Add(this.label18);
		this.panel2.Controls.Add(this.BalanceWeight);
		this.panel2.Controls.Add(this.RecievedWeight);
		this.panel2.Controls.Add(this.OrderWeight);
		this.panel2.Controls.Add(this.label14);
		this.panel2.Controls.Add(this.label13);
		this.panel2.Controls.Add(this.label12);
		this.panel2.Controls.Add(this.ExpiryStatus);
		this.panel2.Controls.Add(this.OrderStatus);
		this.panel2.Controls.Add(this.DeliveryTerm);
		this.panel2.Controls.Add(this.label7);
		this.panel2.Controls.Add(this.label6);
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add(this.ExpiryDate);
		this.panel2.Controls.Add(this.label8);
		this.panel2.Controls.Add(this.OrderDate);
		this.panel2.Controls.Add(this.label5);
		this.panel2.Controls.Add(this.OrderNo);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel2.Location = new System.Drawing.Point(0, 33);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1356, 86);
		this.panel2.TabIndex = 2;
		this.ApprovalUserName.AutoSize = true;
		this.ApprovalUserName.BackColor = System.Drawing.Color.Teal;
		this.ApprovalUserName.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ApprovalUserName.ForeColor = System.Drawing.Color.White;
		this.ApprovalUserName.Location = new System.Drawing.Point(1118, 58);
		this.ApprovalUserName.Name = "ApprovalUserName";
		this.ApprovalUserName.Size = new System.Drawing.Size(40, 15);
		this.ApprovalUserName.TabIndex = 23;
		this.ApprovalUserName.Text = "Name";
		this.ModifyUserName.AutoSize = true;
		this.ModifyUserName.BackColor = System.Drawing.Color.Teal;
		this.ModifyUserName.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ModifyUserName.ForeColor = System.Drawing.Color.White;
		this.ModifyUserName.Location = new System.Drawing.Point(563, 58);
		this.ModifyUserName.Name = "ModifyUserName";
		this.ModifyUserName.Size = new System.Drawing.Size(122, 15);
		this.ModifyUserName.TabIndex = 22;
		this.ModifyUserName.Text = "Load & Factory Weight";
		this.EntryUserName.AutoSize = true;
		this.EntryUserName.BackColor = System.Drawing.Color.Teal;
		this.EntryUserName.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.EntryUserName.ForeColor = System.Drawing.Color.White;
		this.EntryUserName.Location = new System.Drawing.Point(81, 58);
		this.EntryUserName.Name = "EntryUserName";
		this.EntryUserName.Size = new System.Drawing.Size(122, 15);
		this.EntryUserName.TabIndex = 21;
		this.EntryUserName.Text = "Load & Factory Weight";
		this.lblApprovalUser.AutoSize = true;
		this.lblApprovalUser.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblApprovalUser.Location = new System.Drawing.Point(1031, 58);
		this.lblApprovalUser.Name = "lblApprovalUser";
		this.lblApprovalUser.Size = new System.Drawing.Size(91, 15);
		this.lblApprovalUser.TabIndex = 20;
		this.lblApprovalUser.Text = "Approval User: ";
		this.lblModifyUser.AutoSize = true;
		this.lblModifyUser.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblModifyUser.Location = new System.Drawing.Point(482, 58);
		this.lblModifyUser.Name = "lblModifyUser";
		this.lblModifyUser.Size = new System.Drawing.Size(80, 15);
		this.lblModifyUser.TabIndex = 19;
		this.lblModifyUser.Text = "Modify User: ";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(7, 58);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(71, 15);
		this.label18.TabIndex = 18;
		this.label18.Text = "Entry User: ";
		this.BalanceWeight.AutoSize = true;
		this.BalanceWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BalanceWeight.Location = new System.Drawing.Point(1125, 41);
		this.BalanceWeight.Name = "BalanceWeight";
		this.BalanceWeight.Size = new System.Drawing.Size(56, 15);
		this.BalanceWeight.TabIndex = 17;
		this.BalanceWeight.Text = "1200000";
		this.RecievedWeight.AutoSize = true;
		this.RecievedWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RecievedWeight.Location = new System.Drawing.Point(590, 41);
		this.RecievedWeight.Name = "RecievedWeight";
		this.RecievedWeight.Size = new System.Drawing.Size(56, 15);
		this.RecievedWeight.TabIndex = 16;
		this.RecievedWeight.Text = "1200000";
		this.OrderWeight.AutoSize = true;
		this.OrderWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.OrderWeight.Location = new System.Drawing.Point(97, 41);
		this.OrderWeight.Name = "OrderWeight";
		this.OrderWeight.Size = new System.Drawing.Size(56, 15);
		this.OrderWeight.TabIndex = 15;
		this.OrderWeight.Text = "1200000";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(1030, 41);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(95, 15);
		this.label14.TabIndex = 14;
		this.label14.Text = "Balance Weight:";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(481, 41);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(100, 15);
		this.label13.TabIndex = 13;
		this.label13.Text = "Dispatch Weight:";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(6, 41);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(87, 15);
		this.label12.TabIndex = 12;
		this.label12.Text = "Order Weight:";
		this.ExpiryStatus.AutoSize = true;
		this.ExpiryStatus.BackColor = System.Drawing.Color.Red;
		this.ExpiryStatus.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ExpiryStatus.ForeColor = System.Drawing.Color.White;
		this.ExpiryStatus.Location = new System.Drawing.Point(568, 24);
		this.ExpiryStatus.Name = "ExpiryStatus";
		this.ExpiryStatus.Size = new System.Drawing.Size(48, 15);
		this.ExpiryStatus.TabIndex = 11;
		this.ExpiryStatus.Text = "Expired";
		this.OrderStatus.AutoSize = true;
		this.OrderStatus.BackColor = System.Drawing.Color.Goldenrod;
		this.OrderStatus.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.OrderStatus.ForeColor = System.Drawing.Color.White;
		this.OrderStatus.Location = new System.Drawing.Point(1117, 24);
		this.OrderStatus.Name = "OrderStatus";
		this.OrderStatus.Size = new System.Drawing.Size(35, 15);
		this.OrderStatus.TabIndex = 10;
		this.OrderStatus.Text = "open";
		this.DeliveryTerm.AutoSize = true;
		this.DeliveryTerm.BackColor = System.Drawing.Color.Teal;
		this.DeliveryTerm.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.DeliveryTerm.ForeColor = System.Drawing.Color.White;
		this.DeliveryTerm.Location = new System.Drawing.Point(97, 24);
		this.DeliveryTerm.Name = "DeliveryTerm";
		this.DeliveryTerm.Size = new System.Drawing.Size(122, 15);
		this.DeliveryTerm.TabIndex = 9;
		this.DeliveryTerm.Text = "Load & Factory Weight";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(1030, 24);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(81, 15);
		this.label7.TabIndex = 8;
		this.label7.Text = "Order Status:";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(481, 24);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(81, 15);
		this.label6.TabIndex = 7;
		this.label6.Text = "Expiry Status:";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(7, 24);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(87, 15);
		this.label4.TabIndex = 6;
		this.label4.Text = "Delivery Term:";
		this.ExpiryDate.AutoSize = true;
		this.ExpiryDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ExpiryDate.Location = new System.Drawing.Point(1106, 7);
		this.ExpiryDate.Name = "ExpiryDate";
		this.ExpiryDate.Size = new System.Drawing.Size(64, 15);
		this.ExpiryDate.TabIndex = 5;
		this.ExpiryDate.Text = "9-12-2022";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.Teal;
		this.label8.Location = new System.Drawing.Point(1030, 7);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(73, 15);
		this.label8.TabIndex = 4;
		this.label8.Text = "Expiry Date:";
		this.OrderDate.AutoSize = true;
		this.OrderDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.OrderDate.Location = new System.Drawing.Point(553, 7);
		this.OrderDate.Name = "OrderDate";
		this.OrderDate.Size = new System.Drawing.Size(64, 15);
		this.OrderDate.TabIndex = 3;
		this.OrderDate.Text = "9-12-2022";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.Teal;
		this.label5.Location = new System.Drawing.Point(481, 7);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(73, 15);
		this.label5.TabIndex = 2;
		this.label5.Text = "Order Date:";
		this.OrderNo.AutoSize = true;
		this.OrderNo.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.OrderNo.ForeColor = System.Drawing.Color.RoyalBlue;
		this.OrderNo.Location = new System.Drawing.Point(60, 7);
		this.OrderNo.Name = "OrderNo";
		this.OrderNo.Size = new System.Drawing.Size(21, 15);
		this.OrderNo.TabIndex = 1;
		this.OrderNo.Text = "29";
		this.OrderNo.Click += new System.EventHandler(OrderNo_Click);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Teal;
		this.label3.Location = new System.Drawing.Point(6, 7);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(53, 15);
		this.label3.TabIndex = 0;
		this.label3.Text = "Order #:";
		this.label2.BackColor = System.Drawing.Color.Teal;
		this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label2.ForeColor = System.Drawing.Color.Black;
		this.label2.Location = new System.Drawing.Point(6, 29);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(1220, 2);
		this.label2.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Calibri", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Teal;
		this.label1.Location = new System.Drawing.Point(4, 3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(174, 26);
		this.label1.TabIndex = 0;
		this.label1.Text = "Sale Order Activity";
		this.tabPage2.Controls.Add(this.panel15);
		this.tabPage2.Controls.Add(this.panel12);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1362, 723);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Cards View";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.gridEX1);
		this.panel15.Controls.Add(this.label19);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(3, 118);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1356, 100);
		this.panel15.TabIndex = 1;
		this.gridEX1.AllowEdit = (InheritableBoolean)2;
		this.gridEX1.AlternatingColors = true;
		((ScrollableBase)this.gridEX1).BackColor = System.Drawing.Color.White;
		this.gridEX1.ColumnAutoResize = true;
		gridEX1_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.gridEX1.DesignTimeLayout = gridEX1_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.gridEX1).Dock = System.Windows.Forms.DockStyle.Fill;
		this.gridEX1.DynamicFiltering = true;
		this.gridEX1.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.gridEX1).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.gridEX1.GroupByBoxVisible = false;
		this.gridEX1.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.gridEX1).Location = new System.Drawing.Point(0, 33);
		((System.Windows.Forms.Control)(object)this.gridEX1).Name = "gridEX1";
		((ScrollableBase)this.gridEX1).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.gridEX1).Size = new System.Drawing.Size(1356, 67);
		((System.Windows.Forms.Control)(object)this.gridEX1).TabIndex = 4;
		this.gridEX1.TotalRow = (InheritableBoolean)1;
		this.gridEX1.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.gridEX1).VisualStyle = (VisualStyle)4;
		this.label19.BackColor = System.Drawing.Color.Teal;
		this.label19.Dock = System.Windows.Forms.DockStyle.Top;
		this.label19.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.White;
		this.label19.Location = new System.Drawing.Point(0, 0);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(1356, 33);
		this.label19.TabIndex = 1;
		this.label19.Text = "Order Detail Information";
		this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel12.Controls.Add(this.panel13);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 3);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1356, 115);
		this.panel12.TabIndex = 0;
		this.panel13.Controls.Add(this.panel14);
		this.panel13.Controls.Add(this.label44);
		this.panel13.Controls.Add(this.label45);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1356, 110);
		this.panel13.TabIndex = 1;
		this.panel14.Controls.Add(this.ApprovalUser1);
		this.panel14.Controls.Add(this.ModifyUser1);
		this.panel14.Controls.Add(this.EntryUser1);
		this.panel14.Controls.Add(this.lblApprovalUser1);
		this.panel14.Controls.Add(this.lblModifyUser1);
		this.panel14.Controls.Add(this.label25);
		this.panel14.Controls.Add(this.BalanceWeight1);
		this.panel14.Controls.Add(this.RecievedWeight1);
		this.panel14.Controls.Add(this.OrderWeight1);
		this.panel14.Controls.Add(this.label29);
		this.panel14.Controls.Add(this.label30);
		this.panel14.Controls.Add(this.label31);
		this.panel14.Controls.Add(this.ExpiryStatus1);
		this.panel14.Controls.Add(this.OrderStatus1);
		this.panel14.Controls.Add(this.DeliveryTerm1);
		this.panel14.Controls.Add(this.label35);
		this.panel14.Controls.Add(this.label36);
		this.panel14.Controls.Add(this.label37);
		this.panel14.Controls.Add(this.ExpiryDate1);
		this.panel14.Controls.Add(this.label39);
		this.panel14.Controls.Add(this.OrderDate1);
		this.panel14.Controls.Add(this.label41);
		this.panel14.Controls.Add(this.OrderNo1);
		this.panel14.Controls.Add(this.label43);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel14.Location = new System.Drawing.Point(0, 34);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1356, 76);
		this.panel14.TabIndex = 2;
		this.ApprovalUser1.AutoSize = true;
		this.ApprovalUser1.BackColor = System.Drawing.Color.Teal;
		this.ApprovalUser1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ApprovalUser1.ForeColor = System.Drawing.Color.White;
		this.ApprovalUser1.Location = new System.Drawing.Point(1118, 58);
		this.ApprovalUser1.Name = "ApprovalUser1";
		this.ApprovalUser1.Size = new System.Drawing.Size(40, 15);
		this.ApprovalUser1.TabIndex = 23;
		this.ApprovalUser1.Text = "Name";
		this.ModifyUser1.AutoSize = true;
		this.ModifyUser1.BackColor = System.Drawing.Color.Teal;
		this.ModifyUser1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ModifyUser1.ForeColor = System.Drawing.Color.White;
		this.ModifyUser1.Location = new System.Drawing.Point(563, 58);
		this.ModifyUser1.Name = "ModifyUser1";
		this.ModifyUser1.Size = new System.Drawing.Size(122, 15);
		this.ModifyUser1.TabIndex = 22;
		this.ModifyUser1.Text = "Load & Factory Weight";
		this.EntryUser1.AutoSize = true;
		this.EntryUser1.BackColor = System.Drawing.Color.Teal;
		this.EntryUser1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.EntryUser1.ForeColor = System.Drawing.Color.White;
		this.EntryUser1.Location = new System.Drawing.Point(81, 58);
		this.EntryUser1.Name = "EntryUser1";
		this.EntryUser1.Size = new System.Drawing.Size(122, 15);
		this.EntryUser1.TabIndex = 21;
		this.EntryUser1.Text = "Load & Factory Weight";
		this.lblApprovalUser1.AutoSize = true;
		this.lblApprovalUser1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblApprovalUser1.Location = new System.Drawing.Point(1031, 58);
		this.lblApprovalUser1.Name = "lblApprovalUser1";
		this.lblApprovalUser1.Size = new System.Drawing.Size(91, 15);
		this.lblApprovalUser1.TabIndex = 20;
		this.lblApprovalUser1.Text = "Approval User: ";
		this.lblModifyUser1.AutoSize = true;
		this.lblModifyUser1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblModifyUser1.Location = new System.Drawing.Point(482, 58);
		this.lblModifyUser1.Name = "lblModifyUser1";
		this.lblModifyUser1.Size = new System.Drawing.Size(80, 15);
		this.lblModifyUser1.TabIndex = 19;
		this.lblModifyUser1.Text = "Modify User: ";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(7, 58);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(71, 15);
		this.label25.TabIndex = 18;
		this.label25.Text = "Entry User: ";
		this.BalanceWeight1.AutoSize = true;
		this.BalanceWeight1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BalanceWeight1.Location = new System.Drawing.Point(1125, 41);
		this.BalanceWeight1.Name = "BalanceWeight1";
		this.BalanceWeight1.Size = new System.Drawing.Size(56, 15);
		this.BalanceWeight1.TabIndex = 17;
		this.BalanceWeight1.Text = "1200000";
		this.RecievedWeight1.AutoSize = true;
		this.RecievedWeight1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RecievedWeight1.Location = new System.Drawing.Point(590, 41);
		this.RecievedWeight1.Name = "RecievedWeight1";
		this.RecievedWeight1.Size = new System.Drawing.Size(56, 15);
		this.RecievedWeight1.TabIndex = 16;
		this.RecievedWeight1.Text = "1200000";
		this.OrderWeight1.AutoSize = true;
		this.OrderWeight1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.OrderWeight1.Location = new System.Drawing.Point(97, 41);
		this.OrderWeight1.Name = "OrderWeight1";
		this.OrderWeight1.Size = new System.Drawing.Size(56, 15);
		this.OrderWeight1.TabIndex = 15;
		this.OrderWeight1.Text = "1200000";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(1030, 41);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(95, 15);
		this.label29.TabIndex = 14;
		this.label29.Text = "Balance Weight:";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(481, 41);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(103, 15);
		this.label30.TabIndex = 13;
		this.label30.Text = "Recieved Weight:";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(6, 41);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(87, 15);
		this.label31.TabIndex = 12;
		this.label31.Text = "Order Weight:";
		this.ExpiryStatus1.AutoSize = true;
		this.ExpiryStatus1.BackColor = System.Drawing.Color.Red;
		this.ExpiryStatus1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ExpiryStatus1.ForeColor = System.Drawing.Color.White;
		this.ExpiryStatus1.Location = new System.Drawing.Point(568, 24);
		this.ExpiryStatus1.Name = "ExpiryStatus1";
		this.ExpiryStatus1.Size = new System.Drawing.Size(48, 15);
		this.ExpiryStatus1.TabIndex = 11;
		this.ExpiryStatus1.Text = "Expired";
		this.OrderStatus1.AutoSize = true;
		this.OrderStatus1.BackColor = System.Drawing.Color.Goldenrod;
		this.OrderStatus1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.OrderStatus1.ForeColor = System.Drawing.Color.White;
		this.OrderStatus1.Location = new System.Drawing.Point(1117, 24);
		this.OrderStatus1.Name = "OrderStatus1";
		this.OrderStatus1.Size = new System.Drawing.Size(35, 15);
		this.OrderStatus1.TabIndex = 10;
		this.OrderStatus1.Text = "open";
		this.DeliveryTerm1.AutoSize = true;
		this.DeliveryTerm1.BackColor = System.Drawing.Color.Teal;
		this.DeliveryTerm1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.DeliveryTerm1.ForeColor = System.Drawing.Color.White;
		this.DeliveryTerm1.Location = new System.Drawing.Point(97, 24);
		this.DeliveryTerm1.Name = "DeliveryTerm1";
		this.DeliveryTerm1.Size = new System.Drawing.Size(122, 15);
		this.DeliveryTerm1.TabIndex = 9;
		this.DeliveryTerm1.Text = "Load & Factory Weight";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(1030, 24);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(81, 15);
		this.label35.TabIndex = 8;
		this.label35.Text = "Order Status:";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(481, 24);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(81, 15);
		this.label36.TabIndex = 7;
		this.label36.Text = "Expiry Status:";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(7, 24);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(87, 15);
		this.label37.TabIndex = 6;
		this.label37.Text = "Delivery Term:";
		this.ExpiryDate1.AutoSize = true;
		this.ExpiryDate1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ExpiryDate1.Location = new System.Drawing.Point(1106, 7);
		this.ExpiryDate1.Name = "ExpiryDate1";
		this.ExpiryDate1.Size = new System.Drawing.Size(64, 15);
		this.ExpiryDate1.TabIndex = 5;
		this.ExpiryDate1.Text = "9-12-2022";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.ForeColor = System.Drawing.Color.Teal;
		this.label39.Location = new System.Drawing.Point(1030, 7);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(73, 15);
		this.label39.TabIndex = 4;
		this.label39.Text = "Expiry Date:";
		this.OrderDate1.AutoSize = true;
		this.OrderDate1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.OrderDate1.Location = new System.Drawing.Point(553, 7);
		this.OrderDate1.Name = "OrderDate1";
		this.OrderDate1.Size = new System.Drawing.Size(64, 15);
		this.OrderDate1.TabIndex = 3;
		this.OrderDate1.Text = "9-12-2022";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.ForeColor = System.Drawing.Color.Teal;
		this.label41.Location = new System.Drawing.Point(481, 7);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(73, 15);
		this.label41.TabIndex = 2;
		this.label41.Text = "Order Date:";
		this.OrderNo1.AutoSize = true;
		this.OrderNo1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.OrderNo1.Location = new System.Drawing.Point(60, 7);
		this.OrderNo1.Name = "OrderNo1";
		this.OrderNo1.Size = new System.Drawing.Size(21, 15);
		this.OrderNo1.TabIndex = 1;
		this.OrderNo1.Text = "29";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.Color.Teal;
		this.label43.Location = new System.Drawing.Point(6, 7);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(53, 15);
		this.label43.TabIndex = 0;
		this.label43.Text = "Order #:";
		this.label44.BackColor = System.Drawing.Color.Teal;
		this.label44.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label44.ForeColor = System.Drawing.Color.Black;
		this.label44.Location = new System.Drawing.Point(6, 29);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(1220, 2);
		this.label44.TabIndex = 1;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Calibri", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.ForeColor = System.Drawing.Color.Teal;
		this.label45.Location = new System.Drawing.Point(4, 3);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(216, 26);
		this.label45.TabIndex = 0;
		this.label45.Text = "Purchase Order Activity";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1370, 749);
		base.Controls.Add(this.tabControl1);
		base.Name = "SaleActivityDetails";
		this.Text = "Activity Details";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(ActivityDetails_Load);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.InvoiceDetailPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdInvoicesDetail).EndInit();
		this.WagesDetailPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdWagesInfo).EndInit();
		this.GdnDetailPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdGdnDetail).EndInit();
		this.WeighBridgePanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdWeighBridge).EndInit();
		this.GatePassDetailPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdGatePass).EndInit();
		this.DeliveryOrderPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdDeliveryOrder).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.OrderDetailIInfoPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdOrderDetail).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel15.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.gridEX1).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
