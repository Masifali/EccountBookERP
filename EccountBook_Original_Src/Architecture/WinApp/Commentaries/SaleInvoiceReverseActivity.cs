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
using Janus.Data;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.Commentaries;

public class SaleInvoiceReverseActivity : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DataTable dtInvoiceInfo = new DataTable();

	private int InvoiceId;

	private int CustomerId;

	private int CommissionAgentId;

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private Label label1;

	private Label label2;

	private Label InvoiceAmount;

	private Label label3;

	private Label DocDate;

	private Label label4;

	private Label DocNo;

	private Label lbl;

	private Label ApprovalUserName;

	private Label ModifyUserName;

	private Label EntryUserName;

	private Label lblApprovalUser;

	private Label lblModifyUser;

	private Label label18;

	private Panel panel3;

	private Label label5;

	private Label label6;

	internal GridEX grdWeighBridge;

	internal GridEX grdGdn;

	private Panel InvoiceInfoPanel;

	internal GridEX GrdInvoiceInfo;

	private Label label8;

	private Panel GdnDetailPanel;

	private Label label9;

	private Panel WagesInfoPanel;

	private Panel DeliveryOrderPanel;

	private Label label13;

	private Panel GatePassDetailPanel;

	private Label label12;

	private Panel WeighBridgeDetailPanel;

	private Label label11;

	private Panel SaleOrderInfoPanel;

	private Label label7;

	internal GridEX GrdDeliveryOrder;

	internal GridEX GrdSaleOrder;

	internal GridEX grdWagesInfo;

	private Label label10;

	internal GridEX GrdGatePass;

	private Label TotalCommAmount;

	private Label lblCommAmount;

	private Label CommissionAgentName;

	private Label lblCommAgent;

	private Label CustomerName;

	private Label label15;

	private Label datEntryDate;

	private Label label16;

	private Label datModifyDate;

	private Label label19;

	private Label datApprovalDate;

	private Label label23;

	private Label ManualBillNo;

	private Label label21;

	public SaleInvoiceReverseActivity(UserAccount user, int Id)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InvoiceId = Id;
	}

	private void PurchaseInvoiceReverseActivity_Load(object sender, EventArgs e)
	{
		try
		{
			GetData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetData()
	{
		try
		{
			SaleInvoiceReverseActivityDetail obj = ModernHistory.SaleInvoiceReverseActivityDetail(UserAccount.OrganizationId, UserAccount.CompanyId, InvoiceId);
			if (obj.InvoiceInfo.Count > 0)
			{
				GrdInvoiceInfo.ClearStructure();
				InvoiceInfoPanel.Show();
				DocNo.Text = Conversion.ToString((object)obj.InvoiceInfo[0].DocNo);
				DocNo.Tag = Conversion.ToString((object)obj.InvoiceInfo[0].Id);
				DocDate.Text = Conversion.ToString((object)obj.InvoiceInfo[0].DocDate.Date.ToShortDateString());
				InvoiceAmount.Text = obj.InvoiceInfo[0].InvoiceAmount.ToString("#,#;(#,#);0");
				CustomerId = obj.InvoiceInfo[0].SupplierCustomerId;
				CustomerName.Text = obj.InvoiceInfo[0].CustomerName;
				ManualBillNo.Text = obj.InvoiceInfo[0].ManualBillNo;
				EntryUserName.Text = Conversion.ToString((object)obj.InvoiceInfo[0].EntryUserName);
				datEntryDate.Text = Conversion.ToDateTime((object)obj.InvoiceInfo[0].EntryDate).ToString("dd-MMM-yy hh:mm tt");
				ModifyUserName.Text = Conversion.ToString((object)obj.InvoiceInfo[0].ModifyUserName);
				datModifyDate.Text = (Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)obj.InvoiceInfo[0].ModifyDate)) ? "" : Conversion.ToDateTime((object)obj.InvoiceInfo[0].ModifyDate).ToString("dd-MMM-yy hh:mm tt"));
				ApprovalUserName.Text = Conversion.ToString((object)obj.InvoiceInfo[0].ApprovalUserName);
				datApprovalDate.Text = (Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)obj.InvoiceInfo[0].PostDate)) ? "" : Conversion.ToDateTime((object)obj.InvoiceInfo[0].PostDate).ToString("dd-MMM-yy hh:mm tt"));
				dtInvoiceInfo.Columns.Add("GpNo");
				dtInvoiceInfo.Columns.Add("VehicleNo");
				dtInvoiceInfo.Columns.Add("ItemName");
				dtInvoiceInfo.Columns.Add("CropYear");
				dtInvoiceInfo.Columns.Add("ItemQty", typeof(double));
				dtInvoiceInfo.Columns.Add("GrossWeight", typeof(double));
				dtInvoiceInfo.Columns.Add("EBTotalWt", typeof(double));
				dtInvoiceInfo.Columns.Add("NetBillWeight", typeof(double));
				dtInvoiceInfo.Columns.Add("ItemRate", typeof(double));
				dtInvoiceInfo.Columns.Add("RateCut", typeof(double));
				dtInvoiceInfo.Columns.Add("RateUom");
				dtInvoiceInfo.Columns.Add("ItemAmount", typeof(double));
				dtInvoiceInfo.Columns.Add("ExpenseAmount", typeof(double));
				dtInvoiceInfo.Columns.Add("CommissionAmount", typeof(double));
				dtInvoiceInfo.Columns.Add("ItemNetAmount", typeof(double));
				for (int j = 0; j < obj.InvoiceInfo.Count; j++)
				{
					dtInvoiceInfo.Rows.Add(obj.InvoiceInfo[j].GpNo, obj.InvoiceInfo[j].VehicleNo, obj.InvoiceInfo[j].ItemName, obj.InvoiceInfo[j].CropYear, obj.InvoiceInfo[j].ItemQty, obj.InvoiceInfo[j].GrossWeight, obj.InvoiceInfo[j].EBTotalWt, obj.InvoiceInfo[j].NetBillWeight, obj.InvoiceInfo[j].ItemRate, obj.InvoiceInfo[j].RateCut, obj.InvoiceInfo[j].RateUom, obj.InvoiceInfo[j].ItemAmount, obj.InvoiceInfo[j].ExpenseAmount, obj.InvoiceInfo[j].CommissionAmount, obj.InvoiceInfo[j].ItemNetAmount);
				}
				GrdInvoiceInfo.DataSource = dtInvoiceInfo;
				GrdInvoiceInfo.RetrieveStructure();
				GridSettingForInvoiceInfo();
			}
			else
			{
				InvoiceInfoPanel.Hide();
			}
			if (obj.DoInfoList.Count > 0)
			{
				GrdDeliveryOrder.ClearStructure();
				DeliveryOrderPanel.Show();
				GrdDeliveryOrder.DataSource = obj.DoInfoList;
				GrdDeliveryOrder.RetrieveStructure();
				GridSettingForDeliveryOrderInfo();
			}
			else
			{
				DeliveryOrderPanel.Hide();
			}
			if (obj.GdnInfoList.Count > 0)
			{
				grdGdn.ClearStructure();
				GdnDetailPanel.Show();
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("GatePassId", typeof(int));
				dt.Columns.Add("GatePassType", typeof(string));
				dt.Columns.Add("GatePassNumber", typeof(int));
				dt.Columns.Add("DeliveryTerm", typeof(string));
				dt.Columns.Add("VehicleNo", typeof(string));
				dt.Columns.Add("SupplierCustomerId", typeof(int));
				dt.Columns.Add("SupplierName", typeof(string));
				dt.Columns.Add("PartyWeight", typeof(double));
				dt.Columns.Add("FactoryWeight", typeof(double));
				dt.Columns.Add("GrossWeight", typeof(double));
				dt.Columns.Add("BagWeight", typeof(double));
				dt.Columns.Add("WeightCut", typeof(double));
				dt.Columns.Add("Ad_Less_Weight", typeof(double));
				dt.Columns.Add("NetWeight", typeof(double));
				dt.Columns.Add("StockWeight", typeof(double));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				for (int i = 0; i < obj.GdnInfoList.Count; i++)
				{
					dt.Rows.Add(obj.GdnInfoList[i].Id, obj.GdnInfoList[i].DocNo, obj.GdnInfoList[i].DocDate, obj.GdnInfoList[i].GatePassId, obj.GdnInfoList[i].GatePassType, obj.GdnInfoList[i].GatePassNumber, obj.GdnInfoList[i].DeliveryTerm, obj.GdnInfoList[i].VehicleNo, obj.GdnInfoList[i].SupplierCustomerId, obj.GdnInfoList[i].SupplierName, obj.GdnInfoList[i].PartyWeight, obj.GdnInfoList[i].FactoryWeight, obj.GdnInfoList[i].GrossWeight, obj.GdnInfoList[i].BagWeight, obj.GdnInfoList[i].WeightCut, obj.GdnInfoList[i].Ad_Less_Weight, obj.GdnInfoList[i].NetWeight, obj.GdnInfoList[i].StockWeight, obj.GdnInfoList[i].EntryUserName, obj.GdnInfoList[i].EntryDate, obj.GdnInfoList[i].ModifyUserName, obj.GdnInfoList[i].ModifyDate);
				}
				grdGdn.DataSource = dt;
				grdGdn.RetrieveStructure();
				GridSettingForGdn();
			}
			else
			{
				GdnDetailPanel.Hide();
			}
			if (obj.GpInfoList.Count > 0)
			{
				GrdGatePass.ClearStructure();
				GatePassDetailPanel.Show();
				GrdGatePass.DataSource = obj.GpInfoList;
				GrdGatePass.RetrieveStructure();
				GridSettingForGatePass();
			}
			else
			{
				GatePassDetailPanel.Hide();
			}
			if (obj.WbInfoList.Count > 0)
			{
				grdWeighBridge.ClearStructure();
				WeighBridgeDetailPanel.Show();
				grdWeighBridge.DataSource = obj.WbInfoList;
				grdWeighBridge.RetrieveStructure();
				GridSettingForWeighBridge();
			}
			else
			{
				WeighBridgeDetailPanel.Hide();
			}
			if (obj.WagesInfoList.Count > 0)
			{
				grdWagesInfo.ClearStructure();
				WagesInfoPanel.Show();
				grdWagesInfo.DataSource = obj.WagesInfoList;
				grdWagesInfo.RetrieveStructure();
				GridSettingForWagesInfo();
			}
			else
			{
				WagesInfoPanel.Hide();
			}
			if (obj.SoInfoList.Count > 0)
			{
				GrdSaleOrder.ClearStructure();
				SaleOrderInfoPanel.Show();
				GrdSaleOrder.DataSource = obj.SoInfoList;
				GrdSaleOrder.RetrieveStructure();
				GridSettingForSaleOrderInfo();
			}
			else
			{
				SaleOrderInfoPanel.Hide();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingForInvoiceInfo()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GrdInvoiceInfo.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)GrdInvoiceInfo.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			GrdInvoiceInfo.RootTable.Columns["GpNo"].Width = 45;
			GrdInvoiceInfo.RootTable.Columns["VehicleNo"].Width = 75;
			GrdInvoiceInfo.RootTable.Columns["ItemName"].Width = 130;
			GrdInvoiceInfo.RootTable.Columns["CropYear"].Width = 70;
			GrdInvoiceInfo.RootTable.Columns["ItemQty"].Width = 80;
			GrdInvoiceInfo.RootTable.Columns["GrossWeight"].Width = 80;
			GrdInvoiceInfo.RootTable.Columns["EBTotalWt"].Width = 65;
			GrdInvoiceInfo.RootTable.Columns["NetBillWeight"].Width = 85;
			GrdInvoiceInfo.RootTable.Columns["ItemRate"].Width = 70;
			GrdInvoiceInfo.RootTable.Columns["RateCut"].Width = 70;
			GrdInvoiceInfo.RootTable.Columns["RateUom"].Width = 55;
			GrdInvoiceInfo.RootTable.Columns["ItemAmount"].Width = 95;
			GrdInvoiceInfo.RootTable.Columns["ExpenseAmount"].Width = 85;
			GrdInvoiceInfo.RootTable.Columns["CommissionAmount"].Width = 85;
			GrdInvoiceInfo.RootTable.Columns["ItemNetAmount"].Width = 95;
			GrdInvoiceInfo.RootTable.Columns["ItemRate"].Caption = "Net Rate";
			GrdInvoiceInfo.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			GrdInvoiceInfo.RootTable.Columns["ItemRate"].FormatString = "#,##0.###";
			GrdInvoiceInfo.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
			GrdInvoiceInfo.RootTable.Columns["RateCut"].FormatString = "#,##0.###";
			GrdInvoiceInfo.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			GrdInvoiceInfo.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			GrdInvoiceInfo.RootTable.Columns["EBTotalWt"].AggregateFunction = (AggregateFunction)2;
			GrdInvoiceInfo.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			GrdInvoiceInfo.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			GrdInvoiceInfo.RootTable.Columns["ExpenseAmount"].AggregateFunction = (AggregateFunction)2;
			GrdInvoiceInfo.RootTable.Columns["CommissionAmount"].AggregateFunction = (AggregateFunction)2;
			GrdInvoiceInfo.RootTable.Columns["ItemNetAmount"].AggregateFunction = (AggregateFunction)2;
			GrdInvoiceInfo.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			GrdInvoiceInfo.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			GrdInvoiceInfo.RootTable.Columns["EBTotalWt"].TextAlignment = (TextAlignment)3;
			GrdInvoiceInfo.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			GrdInvoiceInfo.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			GrdInvoiceInfo.RootTable.Columns["ExpenseAmount"].TextAlignment = (TextAlignment)3;
			GrdInvoiceInfo.RootTable.Columns["CommissionAmount"].TextAlignment = (TextAlignment)3;
			GrdInvoiceInfo.RootTable.Columns["ItemNetAmount"].TextAlignment = (TextAlignment)3;
			GrdInvoiceInfo.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			GrdInvoiceInfo.RootTable.Columns["GrossWeight"].FormatString = "#,##0.###";
			GrdInvoiceInfo.RootTable.Columns["EBTotalWt"].FormatString = "#,##0.###";
			GrdInvoiceInfo.RootTable.Columns["NetBillWeight"].FormatString = "#,##0.###";
			GrdInvoiceInfo.RootTable.Columns["ItemAmount"].FormatString = "#,##0.###";
			GrdInvoiceInfo.RootTable.Columns["ExpenseAmount"].FormatString = "#,##0.###";
			GrdInvoiceInfo.RootTable.Columns["CommissionAmount"].FormatString = "#,##0.###";
			GrdInvoiceInfo.RootTable.Columns["ItemNetAmount"].FormatString = "#,##0.###";
			GrdInvoiceInfo.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			GrdInvoiceInfo.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.###";
			GrdInvoiceInfo.RootTable.Columns["EBTotalWt"].TotalFormatString = "#,##0.###";
			GrdInvoiceInfo.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.###";
			GrdInvoiceInfo.RootTable.Columns["ItemAmount"].TotalFormatString = "#,##0.###";
			GrdInvoiceInfo.RootTable.Columns["ExpenseAmount"].TotalFormatString = "#,##0.###";
			GrdInvoiceInfo.RootTable.Columns["CommissionAmount"].TotalFormatString = "#,##0.###";
			GrdInvoiceInfo.RootTable.Columns["ItemNetAmount"].TotalFormatString = "#,##0.###";
			GrdInvoiceInfo.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			GrdInvoiceInfo.RootTable.Columns["ItemRate"].FormatString = "#,##0.###";
			GrdInvoiceInfo.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
			GrdInvoiceInfo.RootTable.Columns["RateCut"].FormatString = "#,##0.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingForDeliveryOrderInfo()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GrdDeliveryOrder.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)GrdDeliveryOrder.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			GrdDeliveryOrder.RootTable.Columns["DeliveryOrderType"].Caption = "Do Type";
			GrdDeliveryOrder.RootTable.Columns["GpSrNo"].Caption = "Gp No";
			GrdDeliveryOrder.RootTable.Columns["EntryUserName"].Caption = "Entry User";
			GrdDeliveryOrder.RootTable.Columns["ModifyUserName"].Caption = "Modify User";
			GrdDeliveryOrder.RootTable.Columns["ApprovalUserName"].Caption = "Approval User";
			GrdDeliveryOrder.RootTable.Columns["DocNo"].Width = 55;
			GrdDeliveryOrder.RootTable.Columns["DocDate"].Width = 75;
			GrdDeliveryOrder.RootTable.Columns["DeliveryOrderType"].Width = 80;
			GrdDeliveryOrder.RootTable.Columns["OrderNo"].Width = 65;
			GrdDeliveryOrder.RootTable.Columns["CustomerName"].Width = 130;
			GrdDeliveryOrder.RootTable.Columns["GpSrNo"].Width = 55;
			GrdDeliveryOrder.RootTable.Columns["LoadingQty"].Width = 80;
			GrdDeliveryOrder.RootTable.Columns["LoadingWeight"].Width = 80;
			GrdDeliveryOrder.RootTable.Columns["GrossWeight"].Width = 80;
			GrdDeliveryOrder.RootTable.Columns["PackingWeight"].Width = 75;
			GrdDeliveryOrder.RootTable.Columns["EntryUserName"].Width = 100;
			GrdDeliveryOrder.RootTable.Columns["ModifyUserName"].Width = 100;
			GrdDeliveryOrder.RootTable.Columns["ApprovalUserName"].Width = 100;
			GrdDeliveryOrder.RootTable.Columns["EntryDate"].Width = 135;
			GrdDeliveryOrder.RootTable.Columns["ModifyDate"].Width = 135;
			GrdDeliveryOrder.RootTable.Columns["PostDate"].Width = 135;
			GrdDeliveryOrder.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			GrdDeliveryOrder.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			GrdDeliveryOrder.RootTable.Columns["PostDate"].FormatString = "dd-MMM-yy hh:mm tt";
			GrdDeliveryOrder.RootTable.Columns["DeliveryOrderId"].Visible = false;
			GrdDeliveryOrder.RootTable.Columns["IsApproved"].Visible = false;
			GrdDeliveryOrder.RootTable.Columns["OrderSupCustId"].Visible = false;
			GrdDeliveryOrder.RootTable.Columns["EntryUser"].Visible = false;
			GrdDeliveryOrder.RootTable.Columns["ModifyUser"].Visible = false;
			GrdDeliveryOrder.RootTable.Columns["PostUser"].Visible = false;
			GrdDeliveryOrder.RootTable.Columns["SaleOrderId"].Visible = false;
			GrdDeliveryOrder.RootTable.Columns["EntryUserPrfilePicUrl"].Visible = false;
			GrdDeliveryOrder.RootTable.Columns["ModifyUserPrfilePicUrl"].Visible = false;
			GrdDeliveryOrder.RootTable.Columns["ApprovalUserPrfilePicUrl"].Visible = false;
			GrdDeliveryOrder.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			GrdDeliveryOrder.RootTable.Columns["LoadingQty"].AggregateFunction = (AggregateFunction)2;
			GrdDeliveryOrder.RootTable.Columns["LoadingWeight"].AggregateFunction = (AggregateFunction)2;
			GrdDeliveryOrder.RootTable.Columns["PackingWeight"].AggregateFunction = (AggregateFunction)2;
			GrdDeliveryOrder.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			GrdDeliveryOrder.RootTable.Columns["LoadingQty"].TextAlignment = (TextAlignment)3;
			GrdDeliveryOrder.RootTable.Columns["LoadingWeight"].TextAlignment = (TextAlignment)3;
			GrdDeliveryOrder.RootTable.Columns["PackingWeight"].TextAlignment = (TextAlignment)3;
			GrdDeliveryOrder.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			GrdDeliveryOrder.RootTable.Columns["LoadingQty"].FormatString = "#,##0.###";
			GrdDeliveryOrder.RootTable.Columns["LoadingWeight"].FormatString = "#,##0.###";
			GrdDeliveryOrder.RootTable.Columns["PackingWeight"].FormatString = "#,##0.###";
			GrdDeliveryOrder.RootTable.Columns["GrossWeight"].FormatString = "#,##0.###";
			GrdDeliveryOrder.RootTable.Columns["LoadingQty"].TotalFormatString = "#,##0.###";
			GrdDeliveryOrder.RootTable.Columns["LoadingWeight"].TotalFormatString = "#,##0.###";
			GrdDeliveryOrder.RootTable.Columns["PackingWeight"].TotalFormatString = "#,##0.###";
			GrdDeliveryOrder.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingForGdn()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdGdn.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdGdn.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdGdn.RootTable.Columns["Id"].Visible = false;
			grdGdn.RootTable.Columns["GatePassId"].Visible = false;
			grdGdn.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdGdn.RootTable.Columns["SupplierName"].Caption = "Customer Name";
			grdGdn.RootTable.Columns["GatePassNumber"].Caption = "Gp No";
			grdGdn.RootTable.Columns["Ad_Less_Weight"].Caption = "Add / Less";
			grdGdn.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdGdn.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdGdn.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			grdGdn.RootTable.Columns["GatePassNumber"].ColumnType = (ColumnType)5;
			grdGdn.RootTable.Columns["SupplierName"].ColumnType = (ColumnType)5;
			grdGdn.RootTable.Columns["DocNo"].Width = 55;
			grdGdn.RootTable.Columns["DocDate"].Width = 75;
			grdGdn.RootTable.Columns["GatePassType"].Width = 87;
			grdGdn.RootTable.Columns["GatePassNumber"].Width = 50;
			grdGdn.RootTable.Columns["DeliveryTerm"].Width = 87;
			grdGdn.RootTable.Columns["VehicleNo"].Width = 75;
			grdGdn.RootTable.Columns["SupplierName"].Width = 130;
			grdGdn.RootTable.Columns["PartyWeight"].Width = 70;
			grdGdn.RootTable.Columns["FactoryWeight"].Width = 70;
			grdGdn.RootTable.Columns["GrossWeight"].Width = 70;
			grdGdn.RootTable.Columns["BagWeight"].Width = 65;
			grdGdn.RootTable.Columns["WeightCut"].Width = 60;
			grdGdn.RootTable.Columns["Ad_Less_Weight"].Width = 60;
			grdGdn.RootTable.Columns["NetWeight"].Width = 80;
			grdGdn.RootTable.Columns["StockWeight"].Width = 80;
			grdGdn.RootTable.Columns["EntryUser"].Width = 100;
			grdGdn.RootTable.Columns["EntryDate"].Width = 135;
			grdGdn.RootTable.Columns["ModifyUser"].Width = 100;
			grdGdn.RootTable.Columns["ModifyDate"].Width = 135;
			grdGdn.RootTable.Columns["PartyWeight"].AggregateFunction = (AggregateFunction)2;
			grdGdn.RootTable.Columns["FactoryWeight"].AggregateFunction = (AggregateFunction)2;
			grdGdn.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdGdn.RootTable.Columns["BagWeight"].AggregateFunction = (AggregateFunction)2;
			grdGdn.RootTable.Columns["WeightCut"].AggregateFunction = (AggregateFunction)2;
			grdGdn.RootTable.Columns["Ad_Less_Weight"].AggregateFunction = (AggregateFunction)2;
			grdGdn.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdGdn.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			grdGdn.RootTable.Columns["PartyWeight"].TextAlignment = (TextAlignment)3;
			grdGdn.RootTable.Columns["FactoryWeight"].TextAlignment = (TextAlignment)3;
			grdGdn.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grdGdn.RootTable.Columns["BagWeight"].TextAlignment = (TextAlignment)3;
			grdGdn.RootTable.Columns["WeightCut"].TextAlignment = (TextAlignment)3;
			grdGdn.RootTable.Columns["Ad_Less_Weight"].TextAlignment = (TextAlignment)3;
			grdGdn.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdGdn.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			grdGdn.RootTable.Columns["PartyWeight"].FormatString = "#,##0.###";
			grdGdn.RootTable.Columns["FactoryWeight"].FormatString = "#,##0.###";
			grdGdn.RootTable.Columns["GrossWeight"].FormatString = "#,##0.###";
			grdGdn.RootTable.Columns["BagWeight"].FormatString = "#,##0.###";
			grdGdn.RootTable.Columns["WeightCut"].FormatString = "#,##0.###";
			grdGdn.RootTable.Columns["Ad_Less_Weight"].FormatString = "#,##0.###";
			grdGdn.RootTable.Columns["NetWeight"].FormatString = "#,##0.###";
			grdGdn.RootTable.Columns["StockWeight"].FormatString = "#,##0.###";
			grdGdn.RootTable.Columns["PartyWeight"].TotalFormatString = "#,##0.###";
			grdGdn.RootTable.Columns["FactoryWeight"].TotalFormatString = "#,##0.###";
			grdGdn.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.###";
			grdGdn.RootTable.Columns["BagWeight"].TotalFormatString = "#,##0.###";
			grdGdn.RootTable.Columns["WeightCut"].TotalFormatString = "#,##0.###";
			grdGdn.RootTable.Columns["Ad_Less_Weight"].TotalFormatString = "#,##0.###";
			grdGdn.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.###";
			grdGdn.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingForGatePass()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GrdGatePass.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)GrdGatePass.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			GrdGatePass.RootTable.Columns["GpSrNo"].Caption = "Gp No";
			GrdGatePass.RootTable.Columns["SupplierWeight"].Caption = "Do Weight";
			GrdGatePass.RootTable.Columns["DifferenceWeight"].Caption = "Diff Weight";
			GrdGatePass.RootTable.Columns["EntryUserName"].Caption = "Entry User";
			GrdGatePass.RootTable.Columns["ModifyUserName"].Caption = "Modify User";
			GrdGatePass.RootTable.Columns["ApprovalUserName"].Caption = "Approval User";
			GrdGatePass.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			GrdGatePass.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			GrdGatePass.RootTable.Columns["PostDate"].FormatString = "dd-MMM-yy hh:mm tt";
			GrdGatePass.RootTable.Columns["VehicleInDate"].FormatString = "dd-MMM-yy hh:mm tt";
			GrdGatePass.RootTable.Columns["VehicleOutDate"].FormatString = "dd-MMM-yy hh:mm tt";
			GrdGatePass.RootTable.Columns["GdnNo"].Width = 60;
			GrdGatePass.RootTable.Columns["GpSrNo"].Width = 55;
			GrdGatePass.RootTable.Columns["GpDate"].Width = 70;
			GrdGatePass.RootTable.Columns["VehicleNo"].Width = 70;
			GrdGatePass.RootTable.Columns["BiltyNo"].Width = 65;
			GrdGatePass.RootTable.Columns["SupplierWeight"].Width = 75;
			GrdGatePass.RootTable.Columns["FactoryWeight"].Width = 75;
			GrdGatePass.RootTable.Columns["DifferenceWeight"].Width = 70;
			GrdGatePass.RootTable.Columns["VehicleInDate"].Width = 135;
			GrdGatePass.RootTable.Columns["VehicleOutDate"].Width = 135;
			GrdGatePass.RootTable.Columns["EntryUserName"].Width = 100;
			GrdGatePass.RootTable.Columns["EntryDate"].Width = 135;
			GrdGatePass.RootTable.Columns["ModifyUserName"].Width = 100;
			GrdGatePass.RootTable.Columns["ModifyDate"].Width = 135;
			GrdGatePass.RootTable.Columns["ApprovalUserName"].Width = 100;
			GrdGatePass.RootTable.Columns["PostDate"].Width = 135;
			GrdGatePass.RootTable.Columns["EntryUser"].Visible = false;
			GrdGatePass.RootTable.Columns["ModifyUser"].Visible = false;
			GrdGatePass.RootTable.Columns["PostUser"].Visible = false;
			GrdGatePass.RootTable.Columns["GatePassId"].Visible = false;
			GrdGatePass.RootTable.Columns["ApprovalUserPrfilePicUrl"].Visible = false;
			GrdGatePass.RootTable.Columns["ModifyUserPrfilePicUrl"].Visible = false;
			GrdGatePass.RootTable.Columns["EntryUserPrfilePicUrl"].Visible = false;
			GrdGatePass.RootTable.Columns["GpSrNo"].ColumnType = (ColumnType)5;
			GrdGatePass.RootTable.Columns["SupplierWeight"].AggregateFunction = (AggregateFunction)2;
			GrdGatePass.RootTable.Columns["FactoryWeight"].AggregateFunction = (AggregateFunction)2;
			GrdGatePass.RootTable.Columns["DifferenceWeight"].AggregateFunction = (AggregateFunction)2;
			GrdGatePass.RootTable.Columns["SupplierWeight"].TextAlignment = (TextAlignment)3;
			GrdGatePass.RootTable.Columns["FactoryWeight"].TextAlignment = (TextAlignment)3;
			GrdGatePass.RootTable.Columns["DifferenceWeight"].TextAlignment = (TextAlignment)3;
			GrdGatePass.RootTable.Columns["SupplierWeight"].FormatString = "0,0";
			GrdGatePass.RootTable.Columns["FactoryWeight"].FormatString = "0,0";
			GrdGatePass.RootTable.Columns["DifferenceWeight"].FormatString = "0,0";
			GrdGatePass.RootTable.Columns["SupplierWeight"].TotalFormatString = "#,##0.##";
			GrdGatePass.RootTable.Columns["FactoryWeight"].TotalFormatString = "#,##0.##";
			GrdGatePass.RootTable.Columns["DifferenceWeight"].TotalFormatString = "#,##0.##";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingForWeighBridge()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdWeighBridge.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdWeighBridge.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdWeighBridge.RootTable.Columns["GatePassNumber"].Caption = "Gp No";
			grdWeighBridge.RootTable.Columns["TicketNo"].Caption = "Ticket #";
			grdWeighBridge.RootTable.Columns["EntryUserName"].Caption = "Entry User";
			grdWeighBridge.RootTable.Columns["ModifyUserName"].Caption = "Modify User";
			grdWeighBridge.RootTable.Columns["GatePassNumber"].Width = 55;
			grdWeighBridge.RootTable.Columns["TicketNo"].Width = 60;
			grdWeighBridge.RootTable.Columns["TicketDate"].Width = 75;
			grdWeighBridge.RootTable.Columns["FirstWeight"].Width = 80;
			grdWeighBridge.RootTable.Columns["SecondWeight"].Width = 80;
			grdWeighBridge.RootTable.Columns["NetWbWeight"].Width = 80;
			grdWeighBridge.RootTable.Columns["WbRemarks"].Width = 130;
			grdWeighBridge.RootTable.Columns["EntryUserName"].Width = 100;
			grdWeighBridge.RootTable.Columns["ModifyUserName"].Width = 100;
			grdWeighBridge.RootTable.Columns["EntryDate"].Width = 135;
			grdWeighBridge.RootTable.Columns["ModifyDate"].Width = 135;
			grdWeighBridge.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdWeighBridge.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdWeighBridge.RootTable.Columns["WbId"].Visible = false;
			grdWeighBridge.RootTable.Columns["EntryUser"].Visible = false;
			grdWeighBridge.RootTable.Columns["ModifyUser"].Visible = false;
			grdWeighBridge.RootTable.Columns["GatePassId"].Visible = false;
			grdWeighBridge.RootTable.Columns["EntryUserPrfilePicUrl"].Visible = false;
			grdWeighBridge.RootTable.Columns["ModifyUserPrfilePicUrl"].Visible = false;
			grdWeighBridge.RootTable.Columns["GatePassNumber"].ColumnType = (ColumnType)5;
			grdWeighBridge.RootTable.Columns["TicketNo"].ColumnType = (ColumnType)5;
			grdWeighBridge.RootTable.Columns["FirstWeight"].AggregateFunction = (AggregateFunction)2;
			grdWeighBridge.RootTable.Columns["SecondWeight"].AggregateFunction = (AggregateFunction)2;
			grdWeighBridge.RootTable.Columns["NetWbWeight"].AggregateFunction = (AggregateFunction)2;
			grdWeighBridge.RootTable.Columns["FirstWeight"].TextAlignment = (TextAlignment)3;
			grdWeighBridge.RootTable.Columns["SecondWeight"].TextAlignment = (TextAlignment)3;
			grdWeighBridge.RootTable.Columns["NetWbWeight"].TextAlignment = (TextAlignment)3;
			grdWeighBridge.RootTable.Columns["FirstWeight"].FormatString = "#,##0.###";
			grdWeighBridge.RootTable.Columns["SecondWeight"].FormatString = "#,##0.###";
			grdWeighBridge.RootTable.Columns["NetWbWeight"].FormatString = "#,##0.###";
			grdWeighBridge.RootTable.Columns["FirstWeight"].TotalFormatString = "#,##0.###";
			grdWeighBridge.RootTable.Columns["SecondWeight"].TotalFormatString = "#,##0.###";
			grdWeighBridge.RootTable.Columns["NetWbWeight"].TotalFormatString = "#,##0.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingForWagesInfo()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdWagesInfo.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdWagesInfo.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdWagesInfo.RootTable.Columns["GdnNumber"].Caption = "Gdn No";
			grdWagesInfo.RootTable.Columns["EntryUserName"].Caption = "Entry User";
			grdWagesInfo.RootTable.Columns["ModifyUserName"].Caption = "Modify User";
			grdWagesInfo.RootTable.Columns["DocNo"].Width = 55;
			grdWagesInfo.RootTable.Columns["DocDate"].Width = 75;
			grdWagesInfo.RootTable.Columns["GdnNumber"].Width = 60;
			grdWagesInfo.RootTable.Columns["ContractorName"].Width = 120;
			grdWagesInfo.RootTable.Columns["WagesAccountName"].Width = 120;
			grdWagesInfo.RootTable.Columns["Quantity"].Width = 70;
			grdWagesInfo.RootTable.Columns["Weight"].Width = 80;
			grdWagesInfo.RootTable.Columns["PackSize"].Width = 60;
			grdWagesInfo.RootTable.Columns["Rate"].Width = 65;
			grdWagesInfo.RootTable.Columns["Amount"].Width = 85;
			grdWagesInfo.RootTable.Columns["EntryUserName"].Width = 100;
			grdWagesInfo.RootTable.Columns["ModifyUserName"].Width = 100;
			grdWagesInfo.RootTable.Columns["EntryDate"].Width = 135;
			grdWagesInfo.RootTable.Columns["ModifyDate"].Width = 135;
			grdWagesInfo.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdWagesInfo.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdWagesInfo.RootTable.Columns["Id"].Visible = false;
			grdWagesInfo.RootTable.Columns["GdnId"].Visible = false;
			grdWagesInfo.RootTable.Columns["EntryUser"].Visible = false;
			grdWagesInfo.RootTable.Columns["ModifyUser"].Visible = false;
			grdWagesInfo.RootTable.Columns["ContractorId"].Visible = false;
			grdWagesInfo.RootTable.Columns["EntryUserPrfilePicUrl"].Visible = false;
			grdWagesInfo.RootTable.Columns["ModifyUserPrfilePicUrl"].Visible = false;
			grdWagesInfo.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			grdWagesInfo.RootTable.Columns["ContractorName"].ColumnType = (ColumnType)5;
			grdWagesInfo.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdWagesInfo.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			grdWagesInfo.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdWagesInfo.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdWagesInfo.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			grdWagesInfo.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdWagesInfo.RootTable.Columns["Amount"].FormatString = "#,##0.###";
			grdWagesInfo.RootTable.Columns["Quantity"].FormatString = "#,##0.###";
			grdWagesInfo.RootTable.Columns["Weight"].FormatString = "#,##0.###";
			grdWagesInfo.RootTable.Columns["Amount"].TotalFormatString = "#,##0.###";
			grdWagesInfo.RootTable.Columns["Quantity"].TotalFormatString = "#,##0.###";
			grdWagesInfo.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
			grdWagesInfo.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdWagesInfo.RootTable.Columns["Rate"].FormatString = "#,##0.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingForSaleOrderInfo()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GrdSaleOrder.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)GrdSaleOrder.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			GrdSaleOrder.RootTable.Columns["EntryUserName"].Caption = "Entry User";
			GrdSaleOrder.RootTable.Columns["ModifyUserName"].Caption = "Modify User";
			GrdSaleOrder.RootTable.Columns["ApprovalUserName"].Caption = "Approval User";
			GrdSaleOrder.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			GrdSaleOrder.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			GrdSaleOrder.RootTable.Columns["PostDate"].FormatString = "dd-MMM-yy hh:mm tt";
			GrdSaleOrder.RootTable.Columns["OrderNo"].Width = 55;
			GrdSaleOrder.RootTable.Columns["OrderDate"].Width = 75;
			GrdSaleOrder.RootTable.Columns["OrderExpiryDate"].Width = 80;
			GrdSaleOrder.RootTable.Columns["PaymentTerm"].Width = 70;
			GrdSaleOrder.RootTable.Columns["DeliveryTerm"].Width = 75;
			GrdSaleOrder.RootTable.Columns["ExpiryStatus"].Width = 75;
			GrdSaleOrder.RootTable.Columns["OrderStatus"].Width = 75;
			GrdSaleOrder.RootTable.Columns["ItemName"].Width = 130;
			GrdSaleOrder.RootTable.Columns["PackUom"].Width = 65;
			GrdSaleOrder.RootTable.Columns["OrderItemQty"].Width = 70;
			GrdSaleOrder.RootTable.Columns["NetWeight"].Width = 80;
			GrdSaleOrder.RootTable.Columns["OrderItemRate"].Width = 75;
			GrdSaleOrder.RootTable.Columns["RateUom"].Width = 65;
			GrdSaleOrder.RootTable.Columns["Amount"].Width = 90;
			GrdSaleOrder.RootTable.Columns["Crop"].Width = 75;
			GrdSaleOrder.RootTable.Columns["LabSampleNo"].Width = 80;
			GrdSaleOrder.RootTable.Columns["DetailRemarks"].Width = 130;
			GrdSaleOrder.RootTable.Columns["EntryUserName"].Width = 100;
			GrdSaleOrder.RootTable.Columns["EntryDate"].Width = 135;
			GrdSaleOrder.RootTable.Columns["ModifyUserName"].Width = 100;
			GrdSaleOrder.RootTable.Columns["ModifyDate"].Width = 135;
			GrdSaleOrder.RootTable.Columns["ApprovalUserName"].Width = 100;
			GrdSaleOrder.RootTable.Columns["PostDate"].Width = 135;
			GrdSaleOrder.RootTable.Columns["SaleOrderId"].Visible = false;
			GrdSaleOrder.RootTable.Columns["EntryUser"].Visible = false;
			GrdSaleOrder.RootTable.Columns["ModifyUser"].Visible = false;
			GrdSaleOrder.RootTable.Columns["PostUser"].Visible = false;
			GrdSaleOrder.RootTable.Columns["EntryUserPrfilePicUrl"].Visible = false;
			GrdSaleOrder.RootTable.Columns["ModifyUserPrfilePicUrl"].Visible = false;
			GrdSaleOrder.RootTable.Columns["ApprovalUserPrfilePicUrl"].Visible = false;
			GrdSaleOrder.RootTable.Columns["SaleOrderDetailId"].Visible = false;
			GrdSaleOrder.RootTable.Columns["StockAcId"].Visible = false;
			GrdSaleOrder.RootTable.Columns["OrderItemId"].Visible = false;
			GrdSaleOrder.RootTable.Columns["OrderNo"].ColumnType = (ColumnType)5;
			GrdSaleOrder.RootTable.Columns["ItemName"].ColumnType = (ColumnType)5;
			GrdSaleOrder.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			GrdSaleOrder.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			GrdSaleOrder.RootTable.Columns["OrderItemQty"].AggregateFunction = (AggregateFunction)2;
			GrdSaleOrder.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			GrdSaleOrder.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			GrdSaleOrder.RootTable.Columns["OrderItemQty"].TextAlignment = (TextAlignment)3;
			GrdSaleOrder.RootTable.Columns["Amount"].FormatString = "#,##0.###";
			GrdSaleOrder.RootTable.Columns["NetWeight"].FormatString = "#,##0.###";
			GrdSaleOrder.RootTable.Columns["OrderItemQty"].FormatString = "#,##0.###";
			GrdSaleOrder.RootTable.Columns["Amount"].TotalFormatString = "#,##0.###";
			GrdSaleOrder.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.###";
			GrdSaleOrder.RootTable.Columns["OrderItemQty"].TotalFormatString = "#,##0.###";
			GrdSaleOrder.RootTable.Columns["OrderItemRate"].TextAlignment = (TextAlignment)3;
			GrdSaleOrder.RootTable.Columns["OrderItemRate"].FormatString = "#,##0.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocNo_Click(object sender, EventArgs e)
	{
		try
		{
			if (DocNo.Tag != null)
			{
				CommonServices.SaleInvoicetSlip_301(Conversion.ToInt(DocNo.Tag), CustomerId, 0, Conversion.ToDateTime((object)DocDate.Text), Conversion.ToDateTime((object)DocDate.Text));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierName_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.GoToCustomerLedgerFromLinkedEvent(CustomerId, clsGlobalVariables.ActiveYr.Start_Period, DateTime.Now);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionAgentName_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.GoToCustomerLedgerFromLinkedEvent(CommissionAgentId, clsGlobalVariables.ActiveYr.Start_Period, DateTime.Now);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGrn_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow currentRow = grdGdn.CurrentRow;
			int GdnId = Conversion.ToInt(currentRow.Cells["Id"].Value);
			int GatePassId = Conversion.ToInt(currentRow.Cells["GatePassId"].Value);
			int SupplierCustomerId = Conversion.ToInt(currentRow.Cells["SupplierCustomerId"].Value);
			if (e.Column.Key == "DocNo")
			{
				CommonServices.InvGdnSlip260(GdnId);
			}
			if (e.Column.Key == "GatePassNumber")
			{
				CommonServices.GatePassOutwardSlipandRegister(GatePassId);
			}
			if (e.Column.Key == "SupplierName")
			{
				CommonServices.GoToCustomerLedgerFromLinkedEvent(SupplierCustomerId, clsGlobalVariables.ActiveYr.Start_Period, DateTime.Now);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdWagesInfo_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow currentRow = grdWagesInfo.CurrentRow;
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

	private void grdWeighBridge_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow currentRow = grdWeighBridge.CurrentRow;
			int GatePassId = Conversion.ToInt(currentRow.Cells["GatePassId"].Value);
			int WbId = Conversion.ToInt(currentRow.Cells["WbId"].Value);
			if (e.Column.Key == "GatePassNumber")
			{
				CommonServices.GatePassOutwardSlipandRegister(GatePassId);
			}
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

	private void GrdDeliveryOrder_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(GrdDeliveryOrder.CurrentRow.Cells["DeliveryOrderId"].Value);
			if (e.Column.Key == "DocNo")
			{
				CommonServices.InvDeliveryOrderSlip(Id, 84);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdSaleOrder_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow currentRow = GrdSaleOrder.CurrentRow;
			int SaleOrderId = Conversion.ToInt(currentRow.Cells["SaleOrderId"].Value);
			Conversion.ToInt(currentRow.Cells["StockAcId"].Value);
			int OrderItemId = Conversion.ToInt(currentRow.Cells["OrderItemId"].Value);
			DateTime OrderDate = Conversion.ToDateTime(currentRow.Cells["EntryDate"].Value);
			if (e.Column.Key == "OrderNo")
			{
				new SaleActivityDetails(UserAccount, SaleOrderId).Show();
			}
			if (e.Column.Key == "ItemName")
			{
				CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(OrderItemId, OrderDate, DateTime.Now);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGdn_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = grdGdn.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int GdnId = Conversion.ToInt(item.Cells["Id"].Value);
			int GatePassId = Conversion.ToInt(item.Cells["GatePassId"].Value);
			int SupplierCustomerId = Conversion.ToInt(item.Cells["SupplierCustomerId"].Value);
			if (grdGdn.CurrentColumn != null)
			{
				if (grdGdn.CurrentColumn.Key == "DocNo")
				{
					CommonServices.InvGdnSlip260(GdnId);
				}
				else if (grdGdn.CurrentColumn.Key == "GatePassNumber")
				{
					CommonServices.GatePassOutwardSlipandRegister(GatePassId);
				}
				else if (grdGdn.CurrentColumn.Key == "SupplierName")
				{
					CommonServices.GoToCustomerLedgerFromLinkedEvent(SupplierCustomerId, clsGlobalVariables.ActiveYr.Start_Period, DateTime.Now);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdWagesInfo_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = grdWagesInfo.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int Id = Conversion.ToInt(item.Cells["Id"].Value);
			int ContractorId = Conversion.ToInt(item.Cells["ContractorId"].Value);
			if (grdWagesInfo.CurrentColumn != null)
			{
				if (grdWagesInfo.CurrentColumn.Key == "DocNo")
				{
					CommonServices.ContractorWagesBill_SlipandRegister_002(Id);
				}
				else if (grdWagesInfo.CurrentColumn.Key == "ContractorName")
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

	private void grdWeighBridge_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = grdWeighBridge.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int GatePassId = Conversion.ToInt(item.Cells["GatePassId"].Value);
			int WbId = Conversion.ToInt(item.Cells["WbId"].Value);
			if (grdWeighBridge.CurrentColumn != null)
			{
				if (grdWeighBridge.CurrentColumn.Key == "GatePassNumber")
				{
					CommonServices.GatePassOutwardSlipandRegister(GatePassId);
				}
				else if (grdWeighBridge.CurrentColumn.Key == "TicketNo")
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
				int Id = Conversion.ToInt(item.Cells["DeliveryOrderId"].Value);
				if (GrdDeliveryOrder.CurrentColumn != null && GrdDeliveryOrder.CurrentColumn.Key == "DocNo")
				{
					CommonServices.InvDeliveryOrderSlip(Id, 84);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdSaleOrder_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = GrdSaleOrder.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int SaleOrderId = Conversion.ToInt(item.Cells["SaleOrderId"].Value);
			Conversion.ToInt(item.Cells["StockAcId"].Value);
			int OrderItemId = Conversion.ToInt(item.Cells["OrderItemId"].Value);
			Conversion.ToDateTime(item.Cells["EntryDate"].Value);
			if (GrdSaleOrder.CurrentColumn != null)
			{
				if (GrdSaleOrder.CurrentColumn.Key == "OrderNo")
				{
					new SaleActivityDetails(UserAccount, SaleOrderId);
				}
				else if (GrdSaleOrder.CurrentColumn.Key == "ItemName")
				{
					CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(OrderItemId, clsGlobalVariables.ActiveYr.Start_Period, DateTime.Now);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdInvoiceInfo_FormattingRow(object sender, RowLoadEventArgs e)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		try
		{
			if (GrdInvoiceInfo.RowCount <= 0)
			{
				return;
			}
			GridEXColumn RateCut = GrdInvoiceInfo.RootTable.Columns["RateCut"];
			if (RateCut != null)
			{
				GridEXCell CutCell = e.Row.Cells[((JanusCollectionElement)RateCut).Index];
				if (Conversion.ToInt(CutCell.Value) != 0)
				{
					CutCell.FormatStyle = new GridEXFormatStyle();
					CutCell.FormatStyle.ForeColor = Color.Red;
					CutCell.FormatStyle.FontBold = (TriState)2;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGdn_FormattingRow(object sender, RowLoadEventArgs e)
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		try
		{
			if (grdGdn.RowCount <= 0)
			{
				return;
			}
			GridEXColumn BagWeight = grdGdn.RootTable.Columns["BagWeight"];
			GridEXColumn WeightCut = grdGdn.RootTable.Columns["WeightCut"];
			if (BagWeight != null)
			{
				GridEXCell BagWeightCell = e.Row.Cells[((JanusCollectionElement)BagWeight).Index];
				if (Conversion.ToInt(BagWeightCell.Value) > 0)
				{
					BagWeightCell.FormatStyle = new GridEXFormatStyle();
					BagWeightCell.FormatStyle.ForeColor = Color.Red;
					BagWeightCell.FormatStyle.FontBold = (TriState)2;
				}
			}
			if (WeightCut != null)
			{
				GridEXCell WeightCutCell = e.Row.Cells[((JanusCollectionElement)WeightCut).Index];
				if (Conversion.ToInt(WeightCutCell.Value) > 0)
				{
					WeightCutCell.FormatStyle = new GridEXFormatStyle();
					WeightCutCell.FormatStyle.ForeColor = Color.Red;
					WeightCutCell.FormatStyle.FontBold = (TriState)2;
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
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Expected O, but got Unknown
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_0698: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a2: Expected O, but got Unknown
		//IL_0905: Unknown result type (might be due to invalid IL or missing references)
		//IL_090f: Expected O, but got Unknown
		//IL_0b71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7b: Expected O, but got Unknown
		//IL_0ddd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de7: Expected O, but got Unknown
		//IL_104c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1056: Expected O, but got Unknown
		//IL_12b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c3: Expected O, but got Unknown
		//IL_12d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_12da: Expected O, but got Unknown
		//IL_153b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1545: Expected O, but got Unknown
		GridEXLayout GrdSaleOrder_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdDeliveryOrder_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdGatePass_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdWeighBridge_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdWagesInfo_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdGdn_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdInvoiceInfo_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.SaleOrderInfoPanel = new System.Windows.Forms.Panel();
		this.GrdSaleOrder = new GridEX();
		this.label7 = new System.Windows.Forms.Label();
		this.DeliveryOrderPanel = new System.Windows.Forms.Panel();
		this.GrdDeliveryOrder = new GridEX();
		this.label13 = new System.Windows.Forms.Label();
		this.GatePassDetailPanel = new System.Windows.Forms.Panel();
		this.GrdGatePass = new GridEX();
		this.label12 = new System.Windows.Forms.Label();
		this.WeighBridgeDetailPanel = new System.Windows.Forms.Panel();
		this.grdWeighBridge = new GridEX();
		this.label11 = new System.Windows.Forms.Label();
		this.WagesInfoPanel = new System.Windows.Forms.Panel();
		this.grdWagesInfo = new GridEX();
		this.label10 = new System.Windows.Forms.Label();
		this.GdnDetailPanel = new System.Windows.Forms.Panel();
		this.grdGdn = new GridEX();
		this.label9 = new System.Windows.Forms.Label();
		this.InvoiceInfoPanel = new System.Windows.Forms.Panel();
		this.GrdInvoiceInfo = new GridEX();
		this.label8 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.TotalCommAmount = new System.Windows.Forms.Label();
		this.lblCommAmount = new System.Windows.Forms.Label();
		this.CommissionAgentName = new System.Windows.Forms.Label();
		this.lblCommAgent = new System.Windows.Forms.Label();
		this.CustomerName = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.ApprovalUserName = new System.Windows.Forms.Label();
		this.ModifyUserName = new System.Windows.Forms.Label();
		this.EntryUserName = new System.Windows.Forms.Label();
		this.lblApprovalUser = new System.Windows.Forms.Label();
		this.lblModifyUser = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.InvoiceAmount = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.DocNo = new System.Windows.Forms.Label();
		this.lbl = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.datEntryDate = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.datModifyDate = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.ManualBillNo = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.datApprovalDate = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.SaleOrderInfoPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdSaleOrder).BeginInit();
		this.DeliveryOrderPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdDeliveryOrder).BeginInit();
		this.GatePassDetailPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdGatePass).BeginInit();
		this.WeighBridgeDetailPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdWeighBridge).BeginInit();
		this.WagesInfoPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdWagesInfo).BeginInit();
		this.GdnDetailPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGdn).BeginInit();
		this.InvoiceInfoPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdInvoiceInfo).BeginInit();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1468, 967);
		this.panel1.TabIndex = 0;
		this.panel3.AutoScroll = true;
		this.panel3.Controls.Add(this.SaleOrderInfoPanel);
		this.panel3.Controls.Add(this.DeliveryOrderPanel);
		this.panel3.Controls.Add(this.GatePassDetailPanel);
		this.panel3.Controls.Add(this.WeighBridgeDetailPanel);
		this.panel3.Controls.Add(this.WagesInfoPanel);
		this.panel3.Controls.Add(this.GdnDetailPanel);
		this.panel3.Controls.Add(this.InvoiceInfoPanel);
		this.panel3.Controls.Add(this.label6);
		this.panel3.Controls.Add(this.label5);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 118);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1468, 849);
		this.panel3.TabIndex = 1;
		this.SaleOrderInfoPanel.Controls.Add((System.Windows.Forms.Control)(object)this.GrdSaleOrder);
		this.SaleOrderInfoPanel.Controls.Add(this.label7);
		this.SaleOrderInfoPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.SaleOrderInfoPanel.Location = new System.Drawing.Point(0, 1103);
		this.SaleOrderInfoPanel.Name = "SaleOrderInfoPanel";
		this.SaleOrderInfoPanel.Size = new System.Drawing.Size(1451, 180);
		this.SaleOrderInfoPanel.TabIndex = 15;
		this.GrdSaleOrder.AllowEdit = (InheritableBoolean)2;
		this.GrdSaleOrder.AlternatingColors = true;
		((ScrollableBase)this.GrdSaleOrder).BackColor = System.Drawing.Color.White;
		GrdSaleOrder_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdSaleOrder.DesignTimeLayout = GrdSaleOrder_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdSaleOrder).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdSaleOrder.DynamicFiltering = true;
		this.GrdSaleOrder.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdSaleOrder).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdSaleOrder.GroupByBoxVisible = false;
		this.GrdSaleOrder.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.GrdSaleOrder).Location = new System.Drawing.Point(0, 30);
		((System.Windows.Forms.Control)(object)this.GrdSaleOrder).Name = "GrdSaleOrder";
		((ScrollableBase)this.GrdSaleOrder).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdSaleOrder).Size = new System.Drawing.Size(1451, 150);
		((System.Windows.Forms.Control)(object)this.GrdSaleOrder).TabIndex = 11;
		this.GrdSaleOrder.TotalRow = (InheritableBoolean)1;
		this.GrdSaleOrder.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdSaleOrder).VisualStyle = (VisualStyle)4;
		this.GrdSaleOrder.LinkClicked += new ColumnActionEventHandler(GrdSaleOrder_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdSaleOrder).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdSaleOrder_KeyDown);
		this.label7.BackColor = System.Drawing.Color.Teal;
		this.label7.Dock = System.Windows.Forms.DockStyle.Top;
		this.label7.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(0, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(1451, 30);
		this.label7.TabIndex = 8;
		this.label7.Text = "Sale Order Information";
		this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.DeliveryOrderPanel.Controls.Add((System.Windows.Forms.Control)(object)this.GrdDeliveryOrder);
		this.DeliveryOrderPanel.Controls.Add(this.label13);
		this.DeliveryOrderPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.DeliveryOrderPanel.Location = new System.Drawing.Point(0, 923);
		this.DeliveryOrderPanel.Name = "DeliveryOrderPanel";
		this.DeliveryOrderPanel.Size = new System.Drawing.Size(1451, 180);
		this.DeliveryOrderPanel.TabIndex = 14;
		this.GrdDeliveryOrder.AllowEdit = (InheritableBoolean)2;
		this.GrdDeliveryOrder.AlternatingColors = true;
		((ScrollableBase)this.GrdDeliveryOrder).BackColor = System.Drawing.Color.White;
		GrdDeliveryOrder_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdDeliveryOrder.DesignTimeLayout = GrdDeliveryOrder_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdDeliveryOrder).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdDeliveryOrder.DynamicFiltering = true;
		this.GrdDeliveryOrder.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdDeliveryOrder).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdDeliveryOrder.GroupByBoxVisible = false;
		this.GrdDeliveryOrder.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.GrdDeliveryOrder).Location = new System.Drawing.Point(0, 30);
		((System.Windows.Forms.Control)(object)this.GrdDeliveryOrder).Name = "GrdDeliveryOrder";
		((ScrollableBase)this.GrdDeliveryOrder).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdDeliveryOrder).Size = new System.Drawing.Size(1451, 150);
		((System.Windows.Forms.Control)(object)this.GrdDeliveryOrder).TabIndex = 10;
		this.GrdDeliveryOrder.TotalRow = (InheritableBoolean)1;
		this.GrdDeliveryOrder.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdDeliveryOrder).VisualStyle = (VisualStyle)4;
		this.GrdDeliveryOrder.LinkClicked += new ColumnActionEventHandler(GrdDeliveryOrder_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdDeliveryOrder).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdDeliveryOrder_KeyDown);
		this.label13.BackColor = System.Drawing.Color.Teal;
		this.label13.Dock = System.Windows.Forms.DockStyle.Top;
		this.label13.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(0, 0);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(1451, 30);
		this.label13.TabIndex = 7;
		this.label13.Text = "Delivery Order  Detail";
		this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.GatePassDetailPanel.Controls.Add((System.Windows.Forms.Control)(object)this.GrdGatePass);
		this.GatePassDetailPanel.Controls.Add(this.label12);
		this.GatePassDetailPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.GatePassDetailPanel.Location = new System.Drawing.Point(0, 743);
		this.GatePassDetailPanel.Name = "GatePassDetailPanel";
		this.GatePassDetailPanel.Size = new System.Drawing.Size(1451, 180);
		this.GatePassDetailPanel.TabIndex = 13;
		this.GrdGatePass.AllowEdit = (InheritableBoolean)2;
		this.GrdGatePass.AlternatingColors = true;
		((ScrollableBase)this.GrdGatePass).BackColor = System.Drawing.Color.White;
		GrdGatePass_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdGatePass.DesignTimeLayout = GrdGatePass_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdGatePass).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdGatePass.DynamicFiltering = true;
		this.GrdGatePass.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdGatePass).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdGatePass.GroupByBoxVisible = false;
		this.GrdGatePass.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.GrdGatePass).Location = new System.Drawing.Point(0, 30);
		((System.Windows.Forms.Control)(object)this.GrdGatePass).Name = "GrdGatePass";
		((ScrollableBase)this.GrdGatePass).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdGatePass).Size = new System.Drawing.Size(1451, 150);
		((System.Windows.Forms.Control)(object)this.GrdGatePass).TabIndex = 7;
		this.GrdGatePass.TotalRow = (InheritableBoolean)1;
		this.GrdGatePass.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdGatePass).VisualStyle = (VisualStyle)4;
		this.GrdGatePass.LinkClicked += new ColumnActionEventHandler(GrdGatePass_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdGatePass).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdGatePass_KeyDown);
		this.label12.BackColor = System.Drawing.Color.Teal;
		this.label12.Dock = System.Windows.Forms.DockStyle.Top;
		this.label12.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(0, 0);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(1451, 30);
		this.label12.TabIndex = 6;
		this.label12.Text = "Gate Pass Detail";
		this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.WeighBridgeDetailPanel.Controls.Add((System.Windows.Forms.Control)(object)this.grdWeighBridge);
		this.WeighBridgeDetailPanel.Controls.Add(this.label11);
		this.WeighBridgeDetailPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.WeighBridgeDetailPanel.Location = new System.Drawing.Point(0, 563);
		this.WeighBridgeDetailPanel.Name = "WeighBridgeDetailPanel";
		this.WeighBridgeDetailPanel.Size = new System.Drawing.Size(1451, 180);
		this.WeighBridgeDetailPanel.TabIndex = 12;
		this.grdWeighBridge.AllowEdit = (InheritableBoolean)2;
		this.grdWeighBridge.AlternatingColors = true;
		((ScrollableBase)this.grdWeighBridge).BackColor = System.Drawing.Color.White;
		grdWeighBridge_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdWeighBridge.DesignTimeLayout = grdWeighBridge_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdWeighBridge).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdWeighBridge.DynamicFiltering = true;
		this.grdWeighBridge.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdWeighBridge).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdWeighBridge.GroupByBoxVisible = false;
		this.grdWeighBridge.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.grdWeighBridge).Location = new System.Drawing.Point(0, 30);
		((System.Windows.Forms.Control)(object)this.grdWeighBridge).Name = "grdWeighBridge";
		((ScrollableBase)this.grdWeighBridge).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdWeighBridge).Size = new System.Drawing.Size(1451, 150);
		((System.Windows.Forms.Control)(object)this.grdWeighBridge).TabIndex = 6;
		this.grdWeighBridge.TotalRow = (InheritableBoolean)1;
		this.grdWeighBridge.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdWeighBridge).VisualStyle = (VisualStyle)4;
		this.grdWeighBridge.LinkClicked += new ColumnActionEventHandler(grdWeighBridge_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdWeighBridge).KeyDown += new System.Windows.Forms.KeyEventHandler(grdWeighBridge_KeyDown);
		this.label11.BackColor = System.Drawing.Color.Teal;
		this.label11.Dock = System.Windows.Forms.DockStyle.Top;
		this.label11.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(0, 0);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(1451, 30);
		this.label11.TabIndex = 4;
		this.label11.Text = "Weigh Bridge Detail";
		this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.WagesInfoPanel.Controls.Add((System.Windows.Forms.Control)(object)this.grdWagesInfo);
		this.WagesInfoPanel.Controls.Add(this.label10);
		this.WagesInfoPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.WagesInfoPanel.Location = new System.Drawing.Point(0, 383);
		this.WagesInfoPanel.Name = "WagesInfoPanel";
		this.WagesInfoPanel.Size = new System.Drawing.Size(1451, 180);
		this.WagesInfoPanel.TabIndex = 11;
		this.grdWagesInfo.AllowEdit = (InheritableBoolean)2;
		this.grdWagesInfo.AlternatingColors = true;
		((ScrollableBase)this.grdWagesInfo).BackColor = System.Drawing.Color.White;
		grdWagesInfo_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdWagesInfo.DesignTimeLayout = grdWagesInfo_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdWagesInfo).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdWagesInfo.DynamicFiltering = true;
		this.grdWagesInfo.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdWagesInfo).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdWagesInfo.GroupByBoxVisible = false;
		this.grdWagesInfo.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.grdWagesInfo).Location = new System.Drawing.Point(0, 30);
		((System.Windows.Forms.Control)(object)this.grdWagesInfo).Name = "grdWagesInfo";
		((ScrollableBase)this.grdWagesInfo).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdWagesInfo).Size = new System.Drawing.Size(1451, 150);
		((System.Windows.Forms.Control)(object)this.grdWagesInfo).TabIndex = 11;
		this.grdWagesInfo.TotalRow = (InheritableBoolean)1;
		this.grdWagesInfo.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdWagesInfo).VisualStyle = (VisualStyle)4;
		this.grdWagesInfo.LinkClicked += new ColumnActionEventHandler(grdWagesInfo_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdWagesInfo).KeyDown += new System.Windows.Forms.KeyEventHandler(grdWagesInfo_KeyDown);
		this.label10.BackColor = System.Drawing.Color.Teal;
		this.label10.Dock = System.Windows.Forms.DockStyle.Top;
		this.label10.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold);
		this.label10.ForeColor = System.Drawing.Color.White;
		this.label10.Location = new System.Drawing.Point(0, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(1451, 30);
		this.label10.TabIndex = 10;
		this.label10.Text = "Wages Info";
		this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.GdnDetailPanel.Controls.Add((System.Windows.Forms.Control)(object)this.grdGdn);
		this.GdnDetailPanel.Controls.Add(this.label9);
		this.GdnDetailPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.GdnDetailPanel.Location = new System.Drawing.Point(0, 203);
		this.GdnDetailPanel.Name = "GdnDetailPanel";
		this.GdnDetailPanel.Size = new System.Drawing.Size(1451, 180);
		this.GdnDetailPanel.TabIndex = 10;
		this.grdGdn.AllowEdit = (InheritableBoolean)2;
		this.grdGdn.AlternatingColors = true;
		((ScrollableBase)this.grdGdn).BackColor = System.Drawing.Color.White;
		grdGdn_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGdn.DesignTimeLayout = grdGdn_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGdn).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdGdn.DynamicFiltering = true;
		this.grdGdn.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdGdn).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGdn.GroupByBoxVisible = false;
		this.grdGdn.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.grdGdn).Location = new System.Drawing.Point(0, 30);
		((System.Windows.Forms.Control)(object)this.grdGdn).Name = "grdGdn";
		((ScrollableBase)this.grdGdn).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGdn).Size = new System.Drawing.Size(1451, 150);
		((System.Windows.Forms.Control)(object)this.grdGdn).TabIndex = 6;
		this.grdGdn.TotalRow = (InheritableBoolean)1;
		this.grdGdn.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGdn).VisualStyle = (VisualStyle)4;
		this.grdGdn.FormattingRow += new RowLoadEventHandler(grdGdn_FormattingRow);
		this.grdGdn.LinkClicked += new ColumnActionEventHandler(grdGrn_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdGdn).KeyDown += new System.Windows.Forms.KeyEventHandler(grdGdn_KeyDown);
		this.label9.BackColor = System.Drawing.Color.Teal;
		this.label9.Dock = System.Windows.Forms.DockStyle.Top;
		this.label9.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(0, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(1451, 30);
		this.label9.TabIndex = 5;
		this.label9.Text = "GDN Detail";
		this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.InvoiceInfoPanel.Controls.Add((System.Windows.Forms.Control)(object)this.GrdInvoiceInfo);
		this.InvoiceInfoPanel.Controls.Add(this.label8);
		this.InvoiceInfoPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.InvoiceInfoPanel.Location = new System.Drawing.Point(0, 33);
		this.InvoiceInfoPanel.Name = "InvoiceInfoPanel";
		this.InvoiceInfoPanel.Size = new System.Drawing.Size(1451, 170);
		this.InvoiceInfoPanel.TabIndex = 9;
		this.GrdInvoiceInfo.AllowEdit = (InheritableBoolean)2;
		this.GrdInvoiceInfo.AlternatingColors = true;
		((ScrollableBase)this.GrdInvoiceInfo).BackColor = System.Drawing.Color.White;
		GrdInvoiceInfo_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdInvoiceInfo.DesignTimeLayout = GrdInvoiceInfo_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdInvoiceInfo).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdInvoiceInfo.DynamicFiltering = true;
		this.GrdInvoiceInfo.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdInvoiceInfo).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdInvoiceInfo.GroupByBoxVisible = false;
		this.GrdInvoiceInfo.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.GrdInvoiceInfo).Location = new System.Drawing.Point(0, 30);
		((System.Windows.Forms.Control)(object)this.GrdInvoiceInfo).Name = "GrdInvoiceInfo";
		((ScrollableBase)this.GrdInvoiceInfo).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdInvoiceInfo).Size = new System.Drawing.Size(1451, 140);
		((System.Windows.Forms.Control)(object)this.GrdInvoiceInfo).TabIndex = 6;
		this.GrdInvoiceInfo.TotalRow = (InheritableBoolean)1;
		this.GrdInvoiceInfo.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdInvoiceInfo).VisualStyle = (VisualStyle)4;
		this.GrdInvoiceInfo.FormattingRow += new RowLoadEventHandler(GrdInvoiceInfo_FormattingRow);
		this.label8.BackColor = System.Drawing.Color.Teal;
		this.label8.Dock = System.Windows.Forms.DockStyle.Top;
		this.label8.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(0, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(1451, 30);
		this.label8.TabIndex = 5;
		this.label8.Text = "Sale Invoice Info";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label6.BackColor = System.Drawing.Color.Teal;
		this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label6.Dock = System.Windows.Forms.DockStyle.Top;
		this.label6.ForeColor = System.Drawing.Color.Black;
		this.label6.Location = new System.Drawing.Point(0, 31);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(1451, 2);
		this.label6.TabIndex = 3;
		this.label5.Dock = System.Windows.Forms.DockStyle.Top;
		this.label5.Font = new System.Drawing.Font("Calibri", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.Teal;
		this.label5.Location = new System.Drawing.Point(0, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(1451, 31);
		this.label5.TabIndex = 2;
		this.label5.Text = "Sale Invoice Information";
		this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel2.Controls.Add(this.datApprovalDate);
		this.panel2.Controls.Add(this.label23);
		this.panel2.Controls.Add(this.ManualBillNo);
		this.panel2.Controls.Add(this.label21);
		this.panel2.Controls.Add(this.datModifyDate);
		this.panel2.Controls.Add(this.label19);
		this.panel2.Controls.Add(this.datEntryDate);
		this.panel2.Controls.Add(this.label16);
		this.panel2.Controls.Add(this.TotalCommAmount);
		this.panel2.Controls.Add(this.lblCommAmount);
		this.panel2.Controls.Add(this.CommissionAgentName);
		this.panel2.Controls.Add(this.lblCommAgent);
		this.panel2.Controls.Add(this.CustomerName);
		this.panel2.Controls.Add(this.label15);
		this.panel2.Controls.Add(this.ApprovalUserName);
		this.panel2.Controls.Add(this.ModifyUserName);
		this.panel2.Controls.Add(this.EntryUserName);
		this.panel2.Controls.Add(this.lblApprovalUser);
		this.panel2.Controls.Add(this.lblModifyUser);
		this.panel2.Controls.Add(this.label18);
		this.panel2.Controls.Add(this.InvoiceAmount);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.DocDate);
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add(this.DocNo);
		this.panel2.Controls.Add(this.lbl);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1468, 118);
		this.panel2.TabIndex = 0;
		this.TotalCommAmount.AutoSize = true;
		this.TotalCommAmount.Cursor = System.Windows.Forms.Cursors.Default;
		this.TotalCommAmount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.TotalCommAmount.ForeColor = System.Drawing.Color.Black;
		this.TotalCommAmount.Location = new System.Drawing.Point(1402, 80);
		this.TotalCommAmount.Name = "TotalCommAmount";
		this.TotalCommAmount.Size = new System.Drawing.Size(14, 15);
		this.TotalCommAmount.TabIndex = 48;
		this.TotalCommAmount.Text = "0";
		this.TotalCommAmount.Visible = false;
		this.lblCommAmount.AutoSize = true;
		this.lblCommAmount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCommAmount.ForeColor = System.Drawing.Color.Teal;
		this.lblCommAmount.Location = new System.Drawing.Point(1276, 80);
		this.lblCommAmount.Name = "lblCommAmount";
		this.lblCommAmount.Size = new System.Drawing.Size(128, 15);
		this.lblCommAmount.TabIndex = 47;
		this.lblCommAmount.Text = "Commission Amount: ";
		this.lblCommAmount.Visible = false;
		this.CommissionAgentName.AutoSize = true;
		this.CommissionAgentName.Cursor = System.Windows.Forms.Cursors.Hand;
		this.CommissionAgentName.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.CommissionAgentName.ForeColor = System.Drawing.Color.Blue;
		this.CommissionAgentName.Location = new System.Drawing.Point(1402, 63);
		this.CommissionAgentName.Name = "CommissionAgentName";
		this.CommissionAgentName.Size = new System.Drawing.Size(40, 15);
		this.CommissionAgentName.TabIndex = 44;
		this.CommissionAgentName.Text = "Name";
		this.CommissionAgentName.Visible = false;
		this.CommissionAgentName.Click += new System.EventHandler(CommissionAgentName_Click);
		this.lblCommAgent.AutoSize = true;
		this.lblCommAgent.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCommAgent.ForeColor = System.Drawing.Color.Teal;
		this.lblCommAgent.Location = new System.Drawing.Point(1276, 63);
		this.lblCommAgent.Name = "lblCommAgent";
		this.lblCommAgent.Size = new System.Drawing.Size(116, 15);
		this.lblCommAgent.TabIndex = 43;
		this.lblCommAgent.Text = "Commission Agent: ";
		this.lblCommAgent.Visible = false;
		this.CustomerName.AutoEllipsis = true;
		this.CustomerName.Cursor = System.Windows.Forms.Cursors.Hand;
		this.CustomerName.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.CustomerName.ForeColor = System.Drawing.Color.Blue;
		this.CustomerName.Location = new System.Drawing.Point(113, 78);
		this.CustomerName.Name = "CustomerName";
		this.CustomerName.Size = new System.Drawing.Size(282, 15);
		this.CustomerName.TabIndex = 42;
		this.CustomerName.Text = "Name";
		this.CustomerName.Click += new System.EventHandler(SupplierName_Click);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.Teal;
		this.label15.Location = new System.Drawing.Point(12, 78);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(104, 15);
		this.label15.TabIndex = 41;
		this.label15.Text = "Customer Name: ";
		this.ApprovalUserName.AutoSize = true;
		this.ApprovalUserName.BackColor = System.Drawing.Color.Teal;
		this.ApprovalUserName.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ApprovalUserName.ForeColor = System.Drawing.Color.White;
		this.ApprovalUserName.Location = new System.Drawing.Point(925, 41);
		this.ApprovalUserName.Name = "ApprovalUserName";
		this.ApprovalUserName.Size = new System.Drawing.Size(40, 15);
		this.ApprovalUserName.TabIndex = 29;
		this.ApprovalUserName.Text = "Name";
		this.ModifyUserName.AutoSize = true;
		this.ModifyUserName.BackColor = System.Drawing.Color.Teal;
		this.ModifyUserName.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ModifyUserName.ForeColor = System.Drawing.Color.White;
		this.ModifyUserName.Location = new System.Drawing.Point(486, 78);
		this.ModifyUserName.Name = "ModifyUserName";
		this.ModifyUserName.Size = new System.Drawing.Size(122, 15);
		this.ModifyUserName.TabIndex = 28;
		this.ModifyUserName.Text = "Load & Factory Weight";
		this.EntryUserName.AutoSize = true;
		this.EntryUserName.BackColor = System.Drawing.Color.Teal;
		this.EntryUserName.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.EntryUserName.ForeColor = System.Drawing.Color.White;
		this.EntryUserName.Location = new System.Drawing.Point(486, 41);
		this.EntryUserName.Name = "EntryUserName";
		this.EntryUserName.Size = new System.Drawing.Size(122, 15);
		this.EntryUserName.TabIndex = 27;
		this.EntryUserName.Text = "Load & Factory Weight";
		this.lblApprovalUser.AutoSize = true;
		this.lblApprovalUser.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblApprovalUser.Location = new System.Drawing.Point(828, 41);
		this.lblApprovalUser.Name = "lblApprovalUser";
		this.lblApprovalUser.Size = new System.Drawing.Size(91, 15);
		this.lblApprovalUser.TabIndex = 26;
		this.lblApprovalUser.Text = "Approval User: ";
		this.lblModifyUser.AutoSize = true;
		this.lblModifyUser.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblModifyUser.Location = new System.Drawing.Point(399, 78);
		this.lblModifyUser.Name = "lblModifyUser";
		this.lblModifyUser.Size = new System.Drawing.Size(80, 15);
		this.lblModifyUser.TabIndex = 25;
		this.lblModifyUser.Text = "Modify User: ";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(399, 41);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(71, 15);
		this.label18.TabIndex = 24;
		this.label18.Text = "Entry User: ";
		this.InvoiceAmount.AutoSize = true;
		this.InvoiceAmount.BackColor = System.Drawing.Color.Teal;
		this.InvoiceAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.InvoiceAmount.ForeColor = System.Drawing.Color.White;
		this.InvoiceAmount.Location = new System.Drawing.Point(925, 77);
		this.InvoiceAmount.Name = "InvoiceAmount";
		this.InvoiceAmount.Size = new System.Drawing.Size(106, 17);
		this.InvoiceAmount.TabIndex = 11;
		this.InvoiceAmount.Text = "Invoice Amount";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Black;
		this.label3.Location = new System.Drawing.Point(828, 78);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(100, 15);
		this.label3.TabIndex = 10;
		this.label3.Text = "Invoice Amount: ";
		this.DocDate.AutoSize = true;
		this.DocDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.ForeColor = System.Drawing.Color.Black;
		this.DocDate.Location = new System.Drawing.Point(113, 59);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(56, 15);
		this.DocDate.TabIndex = 9;
		this.DocDate.Text = "Doc Date";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Teal;
		this.label4.Location = new System.Drawing.Point(12, 59);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(100, 15);
		this.label4.TabIndex = 8;
		this.label4.Text = "Document Date: ";
		this.DocNo.AutoSize = true;
		this.DocNo.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.DocNo.ForeColor = System.Drawing.Color.Blue;
		this.DocNo.Location = new System.Drawing.Point(113, 41);
		this.DocNo.Name = "DocNo";
		this.DocNo.Size = new System.Drawing.Size(14, 15);
		this.DocNo.TabIndex = 7;
		this.DocNo.Text = "1";
		this.DocNo.Click += new System.EventHandler(DocNo_Click);
		this.lbl.AutoSize = true;
		this.lbl.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbl.ForeColor = System.Drawing.Color.Teal;
		this.lbl.Location = new System.Drawing.Point(12, 41);
		this.lbl.Name = "lbl";
		this.lbl.Size = new System.Drawing.Size(80, 15);
		this.lbl.TabIndex = 6;
		this.lbl.Text = "Document #: ";
		this.label2.BackColor = System.Drawing.Color.Teal;
		this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label2.Dock = System.Windows.Forms.DockStyle.Top;
		this.label2.ForeColor = System.Drawing.Color.Black;
		this.label2.Location = new System.Drawing.Point(0, 32);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(1468, 2);
		this.label2.TabIndex = 2;
		this.label1.Dock = System.Windows.Forms.DockStyle.Top;
		this.label1.Font = new System.Drawing.Font("Calibri", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Teal;
		this.label1.Location = new System.Drawing.Point(0, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(1468, 32);
		this.label1.TabIndex = 1;
		this.label1.Text = "Sale Invoice Reverse Activity";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.datEntryDate.AutoSize = true;
		this.datEntryDate.BackColor = System.Drawing.Color.Teal;
		this.datEntryDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.datEntryDate.ForeColor = System.Drawing.Color.White;
		this.datEntryDate.Location = new System.Drawing.Point(486, 59);
		this.datEntryDate.Name = "datEntryDate";
		this.datEntryDate.Size = new System.Drawing.Size(63, 15);
		this.datEntryDate.TabIndex = 50;
		this.datEntryDate.Text = "Date Time";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(399, 59);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(72, 15);
		this.label16.TabIndex = 49;
		this.label16.Text = "Entry Date: ";
		this.datModifyDate.AutoSize = true;
		this.datModifyDate.BackColor = System.Drawing.Color.Teal;
		this.datModifyDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.datModifyDate.ForeColor = System.Drawing.Color.White;
		this.datModifyDate.Location = new System.Drawing.Point(486, 97);
		this.datModifyDate.Name = "datModifyDate";
		this.datModifyDate.Size = new System.Drawing.Size(60, 15);
		this.datModifyDate.TabIndex = 52;
		this.datModifyDate.Text = "DateTime";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(399, 97);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(81, 15);
		this.label19.TabIndex = 51;
		this.label19.Text = "Modify Date: ";
		this.ManualBillNo.AutoSize = true;
		this.ManualBillNo.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ManualBillNo.ForeColor = System.Drawing.Color.Black;
		this.ManualBillNo.Location = new System.Drawing.Point(113, 97);
		this.ManualBillNo.Name = "ManualBillNo";
		this.ManualBillNo.Size = new System.Drawing.Size(23, 15);
		this.ManualBillNo.TabIndex = 54;
		this.ManualBillNo.Text = "No";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.Teal;
		this.label21.Location = new System.Drawing.Point(12, 97);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(82, 15);
		this.label21.TabIndex = 53;
		this.label21.Text = "Manual Bill #: ";
		this.datApprovalDate.AutoSize = true;
		this.datApprovalDate.BackColor = System.Drawing.Color.Teal;
		this.datApprovalDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.datApprovalDate.ForeColor = System.Drawing.Color.White;
		this.datApprovalDate.Location = new System.Drawing.Point(925, 59);
		this.datApprovalDate.Name = "datApprovalDate";
		this.datApprovalDate.Size = new System.Drawing.Size(60, 15);
		this.datApprovalDate.TabIndex = 56;
		this.datApprovalDate.Text = "DateTime";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(828, 59);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(92, 15);
		this.label23.TabIndex = 55;
		this.label23.Text = "Approval Date: ";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1468, 967);
		base.Controls.Add(this.panel1);
		base.Name = "SaleInvoiceReverseActivity";
		this.Text = "Sale Invoice Reverse Activity";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(PurchaseInvoiceReverseActivity_Load);
		this.panel1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.SaleOrderInfoPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdSaleOrder).EndInit();
		this.DeliveryOrderPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdDeliveryOrder).EndInit();
		this.GatePassDetailPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdGatePass).EndInit();
		this.WeighBridgeDetailPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdWeighBridge).EndInit();
		this.WagesInfoPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdWagesInfo).EndInit();
		this.GdnDetailPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGdn).EndInit();
		this.InvoiceInfoPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdInvoiceInfo).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		base.ResumeLayout(false);
	}
}
