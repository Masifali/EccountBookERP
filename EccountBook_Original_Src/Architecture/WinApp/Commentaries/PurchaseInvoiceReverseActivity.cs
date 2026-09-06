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

public class PurchaseInvoiceReverseActivity : Form
{
	private UserAccount UserAccount = new UserAccount();

	private int InvoiceId;

	private int SupplierCustomerId;

	private int CommissionagentId;

	private int BrokerAgentId;

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private Label label1;

	private Label label2;

	private Label InvoiceAmount;

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

	internal GridEX grdGrn;

	internal GridEX GrdGatePass;

	internal GridEX grdWeighBridge;

	internal GridEX grdLabInfo;

	internal GridEX grdWagesInfo;

	private Panel OrderInfoPanel;

	internal GridEX GrdOrderInfo;

	private Label label14;

	private Panel WagesInfoPanel;

	private Label label13;

	private Panel LabInfoPanel;

	private Label label12;

	private Panel WeighBrdgeDetailPanel;

	private Label label11;

	private Panel GatePassesDetailPanel;

	private Label label10;

	private Panel GrnDetailPanel;

	private Label label9;

	private Panel InvoiceInfoPanel;

	internal GridEX GrdInvoiceInfo;

	private Label label8;

	private Panel panel4;

	private Label txtOrderType;

	private Label SupplierName;

	private Label label15;

	private Label BrokerName;

	private Label lblBroker;

	private Label CommissionAgentName;

	private Label lblCommAgent;

	private Label BrokeryAmount;

	private Label lblBrokeryAmount;

	private Label TotalCommAmount;

	private Label lblCommAmount;

	private Label label7;

	private Label label16;

	private Label label49;

	private Label label51;

	private Label label48;

	private Label label17;

	private Label label19;

	private Label label20;

	private Label EntryDate;

	private Label ModifyDate;

	private Label label21;

	private Label label22;

	private Label label23;

	private Label label25;

	private Label label26;

	private Label PaymentTerm;

	private Label DeliveryTerm;

	private Label DueDays;

	private Label DueDate;

	private Label IsApproved;

	private Label Add;

	private Label ItemAmount;

	private Label ManualBillNo;

	private Label BillAmount;

	private Label ReferencePartyName;

	private Label label3;

	private Label Less;

	private Label label27;

	public PurchaseInvoiceReverseActivity(UserAccount user, int Id)
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
			PurchaseInvoiceReverseActivityDetail obj = ModernHistory.PurchaseInvoiceReverseActivityDetail(UserAccount.OrganizationId, UserAccount.CompanyId, InvoiceId);
			if (obj.InvoiceInfo.Count > 0)
			{
				GrdInvoiceInfo.DataSource = null;
				InvoiceInfoPanel.Show();
				DocNo.Text = Conversion.ToString((object)obj.InvoiceInfo[0].DocNo);
				ManualBillNo.Text = Conversion.ToString((object)obj.InvoiceInfo[0].ManualBillNo);
				DocDate.Text = Conversion.ToString((object)obj.InvoiceInfo[0].DocDate.Date.ToShortDateString());
				InvoiceAmount.Text = obj.InvoiceInfo[0].InvoiceAmount.ToString("#,#;(#,#);0");
				Add.Text = obj.InvoiceInfo[0].OtherAddLess.ToString("#,#;(#,#);0");
				ItemAmount.Text = obj.InvoiceInfo[0].ItemAmount.ToString("#,#;(#,#);0");
				BillAmount.Text = obj.InvoiceInfo[0].BillAmount.ToString("#,#;(#,#);0");
				SupplierName.Text = obj.InvoiceInfo[0].SupplierName;
				ReferencePartyName.Text = obj.InvoiceInfo[0].ReferencePartyName;
				DeliveryTerm.Text = obj.InvoiceInfo[0].DeliveryTerm;
				PaymentTerm.Text = obj.InvoiceInfo[0].PaymentTerm;
				DueDate.Text = Conversion.ToString((object)obj.InvoiceInfo[0].DueDate.Date.ToShortDateString());
				DueDays.Text = Conversion.ToString((object)obj.InvoiceInfo[0].DueDays);
				SupplierCustomerId = obj.InvoiceInfo[0].SupplierCustomerId;
				CommissionagentId = obj.InvoiceInfo[0].CommissionAgentId;
				TotalCommAmount.Text = obj.InvoiceInfo[0].TotalCommAmount.ToString("0,0");
				CommissionAgentName.Text = obj.InvoiceInfo[0].CommissionAgent;
				BrokerName.Text = obj.InvoiceInfo[0].BrokerAgent;
				double brokerAmount = 0.0 - obj.InvoiceInfo[0].BrokeryAmount;
				BrokeryAmount.Text = brokerAmount.ToString("#,#;(#,#);0");
				EntryUserName.Text = Conversion.ToString((object)obj.InvoiceInfo[0].EntryUserName);
				EntryDate.Text = obj.InvoiceInfo[0].EntryDate.ToString("dd-MMM-yy hh:mm tt");
				ModifyUserName.Text = Conversion.ToString((object)obj.InvoiceInfo[0].ModifyUserName);
				ModifyDate.Text = (Conversion.CheckDateTimeNull(obj.InvoiceInfo[0].ModifyDate) ? "" : obj.InvoiceInfo[0].ModifyDate.ToString("dd-MMM-yy hh:mm tt"));
				ApprovalUserName.Text = Conversion.ToString((object)obj.InvoiceInfo[0].ApprovalUserName);
				IsApproved.Text = Conversion.ToString((object)obj.InvoiceInfo[0].IsApproved);
				DataTable dtInvoiceInfo = new DataTable();
				dtInvoiceInfo.Columns.Add("GpNo", typeof(int));
				dtInvoiceInfo.Columns.Add("OrderNo");
				dtInvoiceInfo.Columns.Add("VehicleNo", typeof(string));
				dtInvoiceInfo.Columns.Add("ItemName", typeof(string));
				dtInvoiceInfo.Columns.Add("CropYear", typeof(string));
				dtInvoiceInfo.Columns.Add("PackSize");
				dtInvoiceInfo.Columns.Add("PackingType");
				dtInvoiceInfo.Columns.Add("Qty", typeof(double));
				dtInvoiceInfo.Columns.Add("GrossWeight", typeof(double));
				dtInvoiceInfo.Columns.Add("EBTotalWt", typeof(double));
				dtInvoiceInfo.Columns.Add("WeightCutTotal", typeof(double));
				dtInvoiceInfo.Columns.Add("NetBillWeight", typeof(double));
				dtInvoiceInfo.Columns.Add("ItemRate", typeof(double));
				dtInvoiceInfo.Columns.Add("RateCut");
				dtInvoiceInfo.Columns.Add("NetRate", typeof(double));
				dtInvoiceInfo.Columns.Add("RateUom", typeof(string));
				dtInvoiceInfo.Columns.Add("ExpenseAmount", typeof(double));
				dtInvoiceInfo.Columns.Add("CommissionAmount", typeof(double));
				dtInvoiceInfo.Columns.Add("ItemNetAmount", typeof(double));
				for (int i2 = 0; i2 < obj.InvoiceInfo.Count; i2++)
				{
					dtInvoiceInfo.Rows.Add(obj.InvoiceInfo[i2].GpNo, obj.InvoiceInfo[i2].OrderNo, obj.InvoiceInfo[i2].VehicleNo, obj.InvoiceInfo[i2].ItemName, obj.InvoiceInfo[i2].CropYear, obj.InvoiceInfo[i2].PackSize, obj.InvoiceInfo[i2].PackingType, obj.InvoiceInfo[i2].ItemQty, obj.InvoiceInfo[i2].GrossWeight, obj.InvoiceInfo[i2].EBTotalWt, obj.InvoiceInfo[i2].WeightCutTotal, obj.InvoiceInfo[i2].NetBillWeight, obj.InvoiceInfo[i2].ItemRate, obj.InvoiceInfo[i2].RateCut, obj.InvoiceInfo[i2].NetRate, obj.InvoiceInfo[i2].RateUom, obj.InvoiceInfo[i2].ExpenseAmount, obj.InvoiceInfo[i2].CommissionAmount, obj.InvoiceInfo[i2].ItemNetAmount);
				}
				GrdInvoiceInfo.DataSource = dtInvoiceInfo;
				GridSettingForInvoiceInfo();
			}
			else
			{
				InvoiceInfoPanel.Hide();
			}
			if (obj.GrnInfoList.Count > 0)
			{
				DataTable dtGrnInfo = new DataTable();
				dtGrnInfo.Columns.Add("Id", typeof(int));
				dtGrnInfo.Columns.Add("GatePassId", typeof(int));
				dtGrnInfo.Columns.Add("SupplierCustomerId", typeof(int));
				dtGrnInfo.Columns.Add("DocNo", typeof(int));
				dtGrnInfo.Columns.Add("DocDate", typeof(DateTime));
				dtGrnInfo.Columns.Add("GatePassType", typeof(string));
				dtGrnInfo.Columns.Add("GatePassNumber", typeof(int));
				dtGrnInfo.Columns.Add("DeliveryTerm", typeof(string));
				dtGrnInfo.Columns.Add("VehicleNo", typeof(string));
				dtGrnInfo.Columns.Add("SupplierName", typeof(string));
				dtGrnInfo.Columns.Add("PartyWeight", typeof(double));
				dtGrnInfo.Columns.Add("FactoryWeight", typeof(double));
				dtGrnInfo.Columns.Add("GrossWeight", typeof(double));
				dtGrnInfo.Columns.Add("WeightCut", typeof(double));
				dtGrnInfo.Columns.Add("Ad_Less_Weight", typeof(double));
				dtGrnInfo.Columns.Add("NetWeight", typeof(double));
				dtGrnInfo.Columns.Add("StockWeight", typeof(double));
				dtGrnInfo.Columns.Add("EntryUser", typeof(string));
				dtGrnInfo.Columns.Add("EntryDate", typeof(DateTime));
				dtGrnInfo.Columns.Add("ModifyUser", typeof(string));
				dtGrnInfo.Columns.Add("ModifyDate", typeof(DateTime));
				for (int n = 0; n < obj.GrnInfoList.Count; n++)
				{
					dtGrnInfo.Rows.Add(obj.GrnInfoList[n].Id, obj.GrnInfoList[n].GatePassId, obj.GrnInfoList[n].SupplierCustomerId, obj.GrnInfoList[n].DocNo, obj.GrnInfoList[n].DocDate, obj.GrnInfoList[n].GatePassType, obj.GrnInfoList[n].GatePassNumber, obj.GrnInfoList[n].DeliveryTerm, obj.GrnInfoList[n].VehicleNo, obj.GrnInfoList[n].SupplierName, obj.GrnInfoList[n].PartyWeight, obj.GrnInfoList[n].FactoryWeight, obj.GrnInfoList[n].GrossWeight, obj.GrnInfoList[n].WeightCut, obj.GrnInfoList[n].Ad_Less_Weight, obj.GrnInfoList[n].NetWeight, obj.GrnInfoList[n].StockWeight, Conversion.ToString((object)obj.GrnInfoList[n].EntryUserName), Conversion.ToDateTime((object)obj.GrnInfoList[n].EntryDate), Conversion.ToString((object)obj.GrnInfoList[n].ModifyUserName), Conversion.ToDateTime((object)obj.GrnInfoList[n].ModifyDate));
				}
				grdGrn.DataSource = dtGrnInfo;
				GridSettingForGrn();
				txtOrderType.Text = obj.GrnInfoList[0].GatePassType;
			}
			else
			{
				GrnDetailPanel.Hide();
			}
			if (obj.GpInfoList.Count > 0)
			{
				DataTable dtGpInfo = new DataTable();
				dtGpInfo.Columns.Add("GatePassId", typeof(int));
				dtGpInfo.Columns.Add("GrnNo", typeof(int));
				dtGpInfo.Columns.Add("GpSrNo", typeof(int));
				dtGpInfo.Columns.Add("GpDate", typeof(DateTime));
				dtGpInfo.Columns.Add("VehicleNo", typeof(string));
				dtGpInfo.Columns.Add("BiltyNo", typeof(string));
				dtGpInfo.Columns.Add("SupplierWeight", typeof(double));
				dtGpInfo.Columns.Add("FactoryWeight", typeof(double));
				dtGpInfo.Columns.Add("DifferenceWeight", typeof(double));
				dtGpInfo.Columns.Add("VehicleInDate", typeof(DateTime));
				dtGpInfo.Columns.Add("VehicleOutDate", typeof(DateTime));
				dtGpInfo.Columns.Add("EntryUser", typeof(string));
				dtGpInfo.Columns.Add("EntryDate", typeof(DateTime));
				dtGpInfo.Columns.Add("ModifyUser", typeof(string));
				dtGpInfo.Columns.Add("ModifyDate", typeof(DateTime));
				dtGpInfo.Columns.Add("ApprovalUser", typeof(string));
				dtGpInfo.Columns.Add("ApprovalDate", typeof(DateTime));
				for (int m = 0; m < obj.GpInfoList.Count; m++)
				{
					dtGpInfo.Rows.Add(obj.GpInfoList[m].GatePassId, obj.GpInfoList[m].GrnNo, obj.GpInfoList[m].GpSrNo, obj.GpInfoList[m].GpDate, obj.GpInfoList[m].VehicleNo, obj.GpInfoList[m].BiltyNo, obj.GpInfoList[m].SupplierWeight, obj.GpInfoList[m].FactoryWeight, obj.GpInfoList[m].DifferenceWeight, obj.GpInfoList[m].VehicleInDate, obj.GpInfoList[m].VehicleOutDate, Conversion.ToString((object)obj.GpInfoList[m].EntryUserName), obj.GpInfoList[m].EntryDate, Conversion.ToString((object)obj.GpInfoList[m].ModifyUserName), obj.GpInfoList[m].ModifyDate, Conversion.ToString((object)obj.GpInfoList[m].ApprovalUserName), obj.GpInfoList[m].PostDate);
				}
				GrdGatePass.DataSource = dtGpInfo;
				GridSettingForGatePass();
			}
			else
			{
				GatePassesDetailPanel.Hide();
			}
			if (obj.WbInfoList.Count > 0)
			{
				DataTable dtWbInfoList = new DataTable();
				dtWbInfoList.Columns.Add("WbId", typeof(int));
				dtWbInfoList.Columns.Add("GatePassId", typeof(int));
				dtWbInfoList.Columns.Add("GatePassNumber", typeof(int));
				dtWbInfoList.Columns.Add("TicketNo", typeof(int));
				dtWbInfoList.Columns.Add("TicketDate", typeof(DateTime));
				dtWbInfoList.Columns.Add("FirstWeight", typeof(double));
				dtWbInfoList.Columns.Add("SecondWeight", typeof(double));
				dtWbInfoList.Columns.Add("NetWbWeight", typeof(double));
				dtWbInfoList.Columns.Add("WbRemarks", typeof(string));
				dtWbInfoList.Columns.Add("EntryUser", typeof(string));
				dtWbInfoList.Columns.Add("EntryDate", typeof(DateTime));
				dtWbInfoList.Columns.Add("ModifyUser", typeof(string));
				dtWbInfoList.Columns.Add("ModifyDate", typeof(DateTime));
				for (int l = 0; l < obj.WbInfoList.Count; l++)
				{
					dtWbInfoList.Rows.Add(obj.WbInfoList[l].WbId, obj.WbInfoList[l].GatePassId, obj.WbInfoList[l].GatePassNumber, obj.WbInfoList[l].TicketNo, Conversion.ToDateTime((object)obj.WbInfoList[l].TicketDate), obj.WbInfoList[l].FirstWeight, obj.WbInfoList[l].SecondWeight, obj.WbInfoList[l].NetWbWeight, obj.WbInfoList[l].WbRemarks, Conversion.ToString((object)obj.WbInfoList[l].EntryUserName), obj.WbInfoList[l].EntryDate, Conversion.ToString((object)obj.WbInfoList[l].ModifyUserName), obj.WbInfoList[l].ModifyDate);
				}
				grdWeighBridge.DataSource = dtWbInfoList;
				GridSettingForWeighBridge();
			}
			else
			{
				WeighBrdgeDetailPanel.Hide();
			}
			if (obj.LabInfoList.Count > 0)
			{
				DataTable dtLabInfoList = new DataTable();
				dtLabInfoList.Columns.Add("Id", typeof(int));
				dtLabInfoList.Columns.Add("GatePassId", typeof(int));
				dtLabInfoList.Columns.Add("ItemId", typeof(int));
				dtLabInfoList.Columns.Add("StockAcId", typeof(int));
				dtLabInfoList.Columns.Add("DocDate", typeof(DateTime));
				dtLabInfoList.Columns.Add("DocNo", typeof(int));
				dtLabInfoList.Columns.Add("GatePassNumber", typeof(int));
				dtLabInfoList.Columns.Add("ItemName", typeof(string));
				dtLabInfoList.Columns.Add("DeductionWeight", typeof(double));
				dtLabInfoList.Columns.Add("DeductionRate", typeof(double));
				dtLabInfoList.Columns.Add("ItemRate", typeof(double));
				dtLabInfoList.Columns.Add("AnalystName", typeof(string));
				dtLabInfoList.Columns.Add("RemarksHeader", typeof(string));
				dtLabInfoList.Columns.Add("Crop", typeof(string));
				dtLabInfoList.Columns.Add("LabStatus", typeof(string));
				dtLabInfoList.Columns.Add("EntryUserName", typeof(string));
				dtLabInfoList.Columns.Add("EntryDate", typeof(DateTime));
				dtLabInfoList.Columns.Add("ModifyUserName", typeof(string));
				dtLabInfoList.Columns.Add("ModifyDate", typeof(DateTime));
				dtLabInfoList.Columns.Add("ApprovedUser", typeof(string));
				dtLabInfoList.Columns.Add("ApprovalDate", typeof(DateTime));
				for (int k = 0; k < obj.LabInfoList.Count; k++)
				{
					dtLabInfoList.Rows.Add(obj.LabInfoList[k].Id, obj.LabInfoList[k].GatePassId, obj.LabInfoList[k].ItemId, obj.LabInfoList[k].StockAcId, obj.LabInfoList[k].DocDate, obj.LabInfoList[k].DocNo, obj.LabInfoList[k].GatePassNumber, obj.LabInfoList[k].ItemName, obj.LabInfoList[k].DeductionWeight, obj.LabInfoList[k].DeductionRate, obj.LabInfoList[k].ItemRate, obj.LabInfoList[k].AnalystName, obj.LabInfoList[k].RemarksHeader, obj.LabInfoList[k].Crop, obj.LabInfoList[k].LabStatus, Conversion.ToString((object)obj.LabInfoList[k].EntryUserName), obj.LabInfoList[k].EntryDate, Conversion.ToString((object)obj.LabInfoList[k].ModifyUserName), obj.LabInfoList[k].ModifyDate, Conversion.ToString((object)obj.LabInfoList[k].ApprovalUserName), obj.LabInfoList[k].PostDate);
				}
				grdLabInfo.DataSource = dtLabInfoList;
				GridSettingForLabInfo();
			}
			else
			{
				LabInfoPanel.Hide();
			}
			if (obj.WagesInfoList.Count > 0)
			{
				DataTable dtWagesInfoList = new DataTable();
				dtWagesInfoList.Columns.Add("Id", typeof(int));
				dtWagesInfoList.Columns.Add("GrnId", typeof(int));
				dtWagesInfoList.Columns.Add("ContractorId", typeof(int));
				dtWagesInfoList.Columns.Add("DocNo", typeof(int));
				dtWagesInfoList.Columns.Add("DocDate", typeof(DateTime));
				dtWagesInfoList.Columns.Add("GrnNumber", typeof(int));
				dtWagesInfoList.Columns.Add("ContractorName", typeof(string));
				dtWagesInfoList.Columns.Add("WagesAccountName", typeof(string));
				dtWagesInfoList.Columns.Add("Quantity", typeof(double));
				dtWagesInfoList.Columns.Add("Weight", typeof(double));
				dtWagesInfoList.Columns.Add("PackSize", typeof(string));
				dtWagesInfoList.Columns.Add("Rate", typeof(double));
				dtWagesInfoList.Columns.Add("Amount", typeof(double));
				dtWagesInfoList.Columns.Add("EntryUserName", typeof(string));
				dtWagesInfoList.Columns.Add("EntryDate", typeof(DateTime));
				dtWagesInfoList.Columns.Add("ModifyUserName", typeof(string));
				dtWagesInfoList.Columns.Add("ModifyDate", typeof(DateTime));
				for (int j = 0; j < obj.WagesInfoList.Count; j++)
				{
					dtWagesInfoList.Rows.Add(obj.WagesInfoList[j].Id, obj.WagesInfoList[j].GrnId, obj.WagesInfoList[j].ContractorId, obj.WagesInfoList[j].DocNo, Conversion.ToDateTime((object)obj.WagesInfoList[j].DocDate), obj.WagesInfoList[j].GrnNumber, obj.WagesInfoList[j].ContractorName, obj.WagesInfoList[j].WagesAccountName, obj.WagesInfoList[j].Quantity, obj.WagesInfoList[j].Weight, obj.WagesInfoList[j].PackSize, obj.WagesInfoList[j].Rate, obj.WagesInfoList[j].Amount, Conversion.ToString((object)obj.WagesInfoList[j].EntryUserName), obj.WagesInfoList[j].EntryDate, Conversion.ToString((object)obj.WagesInfoList[j].ModifyUserName), obj.WagesInfoList[j].ModifyDate);
				}
				grdWagesInfo.DataSource = dtWagesInfoList;
				GridSettingForWagesInfo();
			}
			else
			{
				WagesInfoPanel.Hide();
			}
			if (obj.PoInfoList.Count > 0)
			{
				DataTable dtPoInfoList = new DataTable();
				dtPoInfoList.Columns.Add("PurchaeOrderId", typeof(int));
				dtPoInfoList.Columns.Add("OrderItemId", typeof(int));
				dtPoInfoList.Columns.Add("StockAcId", typeof(int));
				dtPoInfoList.Columns.Add("OrderNo", typeof(int));
				dtPoInfoList.Columns.Add("OrderDate", typeof(DateTime));
				dtPoInfoList.Columns.Add("OrderExpiryDate", typeof(DateTime));
				dtPoInfoList.Columns.Add("PaymentTerm", typeof(string));
				dtPoInfoList.Columns.Add("DeliveryTerm", typeof(string));
				dtPoInfoList.Columns.Add("ExpiryStatus", typeof(string));
				dtPoInfoList.Columns.Add("OrderStatus", typeof(string));
				dtPoInfoList.Columns.Add("ItemName", typeof(string));
				dtPoInfoList.Columns.Add("PackUom", typeof(string));
				dtPoInfoList.Columns.Add("OrderItemQty", typeof(double));
				dtPoInfoList.Columns.Add("NetWeight", typeof(double));
				dtPoInfoList.Columns.Add("OrderItemRate", typeof(double));
				dtPoInfoList.Columns.Add("RateUom", typeof(string));
				dtPoInfoList.Columns.Add("Amount", typeof(double));
				dtPoInfoList.Columns.Add("Crop", typeof(string));
				dtPoInfoList.Columns.Add("LabSampleNo", typeof(string));
				dtPoInfoList.Columns.Add("DetailRemarks", typeof(string));
				dtPoInfoList.Columns.Add("EntryUser", typeof(string));
				dtPoInfoList.Columns.Add("EntryDate", typeof(DateTime));
				dtPoInfoList.Columns.Add("ModifyUser", typeof(string));
				dtPoInfoList.Columns.Add("ModifyDate", typeof(DateTime));
				dtPoInfoList.Columns.Add("ApprovalUser", typeof(string));
				dtPoInfoList.Columns.Add("PostDate", typeof(DateTime));
				for (int i = 0; i < obj.PoInfoList.Count; i++)
				{
					dtPoInfoList.Rows.Add(obj.PoInfoList[i].PurchaeOrderId, obj.PoInfoList[i].OrderItemId, obj.PoInfoList[i].StockAcId, obj.PoInfoList[i].OrderNo, Conversion.ToDateTime((object)obj.PoInfoList[i].OrderDate), Conversion.ToDateTime((object)obj.PoInfoList[i].OrderExpiryDate), obj.PoInfoList[i].PaymentTerm, obj.PoInfoList[i].DeliveryTerm, obj.PoInfoList[i].ExpiryStatus, obj.PoInfoList[i].OrderStatus, obj.PoInfoList[i].ItemName, obj.PoInfoList[i].PackUom, obj.PoInfoList[i].OrderItemQty, obj.PoInfoList[i].NetWeight, obj.PoInfoList[i].OrderItemRate, obj.PoInfoList[i].RateUom, obj.PoInfoList[i].Amount, obj.PoInfoList[i].Crop, obj.PoInfoList[i].LabSampleNo, obj.PoInfoList[i].DetailRemarks, obj.PoInfoList[i].EntryUserName, obj.PoInfoList[i].EntryDate, obj.PoInfoList[i].ModifyUserName, obj.PoInfoList[i].ModifyDate, obj.PoInfoList[i].ApprovalUserName, obj.PoInfoList[i].PostDate);
				}
				GrdOrderInfo.DataSource = dtPoInfoList;
				GridSettingsForOrder();
			}
			else
			{
				OrderInfoPanel.Hide();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingForInvoiceInfo()
	{
		try
		{
			GrdInvoiceInfo.RootTable.Columns["OrderNo"].Width = 60;
			GrdInvoiceInfo.RootTable.Columns["VehicleNo"].Width = 70;
			GrdInvoiceInfo.RootTable.Columns["ItemName"].Width = 150;
			GrdInvoiceInfo.RootTable.Columns["CropYear"].Width = 70;
			GrdInvoiceInfo.RootTable.Columns["PackSize"].Width = 60;
			GrdInvoiceInfo.RootTable.Columns["PackingType"].Width = 60;
			GrdInvoiceInfo.RootTable.Columns["Qty"].Width = 60;
			GrdInvoiceInfo.RootTable.Columns["GrossWeight"].Width = 80;
			GrdInvoiceInfo.RootTable.Columns["EBTotalWt"].Width = 80;
			GrdInvoiceInfo.RootTable.Columns["WeightCutTotal"].Width = 80;
			GrdInvoiceInfo.RootTable.Columns["NetBillWeight"].Width = 80;
			GrdInvoiceInfo.RootTable.Columns["ItemRate"].Width = 70;
			GrdInvoiceInfo.RootTable.Columns["RateCut"].Width = 70;
			GrdInvoiceInfo.RootTable.Columns["NetRate"].Width = 70;
			GrdInvoiceInfo.RootTable.Columns["RateUOM"].Width = 60;
			GrdInvoiceInfo.RootTable.Columns["ExpenseAmount"].Width = 80;
			GrdInvoiceInfo.RootTable.Columns["CommissionAmount"].Width = 80;
			GrdInvoiceInfo.RootTable.Columns["ItemNetAmount"].Width = 80;
			GrdInvoiceInfo.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			GrdInvoiceInfo.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			GrdInvoiceInfo.RootTable.Columns["EBTotalWt"].AggregateFunction = (AggregateFunction)2;
			GrdInvoiceInfo.RootTable.Columns["WeightCutTotal"].AggregateFunction = (AggregateFunction)2;
			GrdInvoiceInfo.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			GrdInvoiceInfo.RootTable.Columns["ExpenseAmount"].AggregateFunction = (AggregateFunction)2;
			GrdInvoiceInfo.RootTable.Columns["CommissionAmount"].AggregateFunction = (AggregateFunction)2;
			GrdInvoiceInfo.RootTable.Columns["ItemNetAmount"].AggregateFunction = (AggregateFunction)2;
			GrdInvoiceInfo.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			GrdInvoiceInfo.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			GrdInvoiceInfo.RootTable.Columns["EBTotalWt"].TextAlignment = (TextAlignment)3;
			GrdInvoiceInfo.RootTable.Columns["WeightCutTotal"].TextAlignment = (TextAlignment)3;
			GrdInvoiceInfo.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			GrdInvoiceInfo.RootTable.Columns["ExpenseAmount"].TextAlignment = (TextAlignment)3;
			GrdInvoiceInfo.RootTable.Columns["CommissionAmount"].TextAlignment = (TextAlignment)3;
			GrdInvoiceInfo.RootTable.Columns["ItemNetAmount"].TextAlignment = (TextAlignment)3;
			GrdInvoiceInfo.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			GrdInvoiceInfo.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
			GrdInvoiceInfo.RootTable.Columns["NetRate"].TextAlignment = (TextAlignment)3;
			GrdInvoiceInfo.RootTable.Columns["Qty"].FormatString = "0,0";
			GrdInvoiceInfo.RootTable.Columns["GrossWeight"].FormatString = "0,0";
			GrdInvoiceInfo.RootTable.Columns["EBTotalWt"].FormatString = "0,0";
			GrdInvoiceInfo.RootTable.Columns["WeightCutTotal"].FormatString = "0,0";
			GrdInvoiceInfo.RootTable.Columns["NetBillWeight"].FormatString = "0,0";
			GrdInvoiceInfo.RootTable.Columns["ExpenseAmount"].FormatString = "0,0";
			GrdInvoiceInfo.RootTable.Columns["CommissionAmount"].FormatString = "0,0";
			GrdInvoiceInfo.RootTable.Columns["ItemNetAmount"].FormatString = "0,0";
			GrdInvoiceInfo.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			GrdInvoiceInfo.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
			GrdInvoiceInfo.RootTable.Columns["EBTotalWt"].TotalFormatString = "#,##0.##";
			GrdInvoiceInfo.RootTable.Columns["WeightCutTotal"].TotalFormatString = "#,##0.##";
			GrdInvoiceInfo.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.##";
			GrdInvoiceInfo.RootTable.Columns["ExpenseAmount"].TotalFormatString = "#,##0.##";
			GrdInvoiceInfo.RootTable.Columns["CommissionAmount"].TotalFormatString = "#,##0.##";
			GrdInvoiceInfo.RootTable.Columns["ItemNetAmount"].TotalFormatString = "#,##0.##";
			GrdInvoiceInfo.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			GrdInvoiceInfo.RootTable.Columns["ItemRate"].FormatString = "#,##0.###";
			GrdInvoiceInfo.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
			GrdInvoiceInfo.RootTable.Columns["RateCut"].FormatString = "#,##0.###";
			GrdInvoiceInfo.RootTable.Columns["NetRate"].TextAlignment = (TextAlignment)3;
			GrdInvoiceInfo.RootTable.Columns["NetRate"].FormatString = "#,##0.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingForGrn()
	{
		try
		{
			grdGrn.RootTable.Columns["Id"].Visible = false;
			grdGrn.RootTable.Columns["GatePassId"].Visible = false;
			grdGrn.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdGrn.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yy";
			grdGrn.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdGrn.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdGrn.RootTable.Columns["DocNo"].Width = 60;
			grdGrn.RootTable.Columns["DocDate"].Width = 70;
			grdGrn.RootTable.Columns["GatePassType"].Width = 100;
			grdGrn.RootTable.Columns["GatePassNumber"].Width = 50;
			grdGrn.RootTable.Columns["DeliveryTerm"].Width = 100;
			grdGrn.RootTable.Columns["VehicleNo"].Width = 100;
			grdGrn.RootTable.Columns["SupplierName"].Width = 100;
			grdGrn.RootTable.Columns["PartyWeight"].Width = 75;
			grdGrn.RootTable.Columns["FactoryWeight"].Width = 75;
			grdGrn.RootTable.Columns["GrossWeight"].Width = 75;
			grdGrn.RootTable.Columns["WeightCut"].Width = 75;
			grdGrn.RootTable.Columns["AddLessWeight"].Width = 75;
			grdGrn.RootTable.Columns["NetWeight"].Width = 75;
			grdGrn.RootTable.Columns["StockWeight"].Width = 75;
			grdGrn.RootTable.Columns["EntryUser"].Width = 100;
			grdGrn.RootTable.Columns["EntryDate"].Width = 135;
			grdGrn.RootTable.Columns["ModifyUser"].Width = 100;
			grdGrn.RootTable.Columns["ModifyDate"].Width = 135;
			grdGrn.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			grdGrn.RootTable.Columns["GatePassNumber"].ColumnType = (ColumnType)5;
			grdGrn.RootTable.Columns["SupplierName"].ColumnType = (ColumnType)5;
			grdGrn.RootTable.Columns["PartyWeight"].AggregateFunction = (AggregateFunction)2;
			grdGrn.RootTable.Columns["FactoryWeight"].AggregateFunction = (AggregateFunction)2;
			grdGrn.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdGrn.RootTable.Columns["WeightCut"].AggregateFunction = (AggregateFunction)2;
			grdGrn.RootTable.Columns["AddLessWeight"].AggregateFunction = (AggregateFunction)2;
			grdGrn.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdGrn.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			grdGrn.RootTable.Columns["PartyWeight"].TextAlignment = (TextAlignment)3;
			grdGrn.RootTable.Columns["FactoryWeight"].TextAlignment = (TextAlignment)3;
			grdGrn.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grdGrn.RootTable.Columns["WeightCut"].TextAlignment = (TextAlignment)3;
			grdGrn.RootTable.Columns["AddLessWeight"].TextAlignment = (TextAlignment)3;
			grdGrn.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdGrn.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			grdGrn.RootTable.Columns["PartyWeight"].FormatString = "0,0";
			grdGrn.RootTable.Columns["FactoryWeight"].FormatString = "0,0";
			grdGrn.RootTable.Columns["GrossWeight"].FormatString = "0,0";
			grdGrn.RootTable.Columns["WeightCut"].FormatString = "0,0";
			grdGrn.RootTable.Columns["AddLessWeight"].FormatString = "0,0";
			grdGrn.RootTable.Columns["NetWeight"].FormatString = "0,0";
			grdGrn.RootTable.Columns["StockWeight"].FormatString = "0,0";
			grdGrn.RootTable.Columns["PartyWeight"].TotalFormatString = "#,##0.##";
			grdGrn.RootTable.Columns["FactoryWeight"].TotalFormatString = "#,##0.##";
			grdGrn.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
			grdGrn.RootTable.Columns["WeightCut"].TotalFormatString = "#,##0.##";
			grdGrn.RootTable.Columns["AddLessWeight"].TotalFormatString = "#,##0.##";
			grdGrn.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			grdGrn.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.##";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingForGatePass()
	{
		try
		{
			GrdGatePass.RootTable.Columns["GatePassId"].Visible = false;
			GrdGatePass.RootTable.Columns["GpDate"].FormatString = "dd-MMM-yy";
			GrdGatePass.RootTable.Columns["VehicleInDate"].FormatString = "dd-MMM-yy hh:mm tt";
			GrdGatePass.RootTable.Columns["VehicleOutDate"].FormatString = "dd-MMM-yy hh:mm tt";
			GrdGatePass.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			GrdGatePass.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			GrdGatePass.RootTable.Columns["ApprovalDate"].FormatString = "dd-MMM-yy hh:mm tt";
			GrdGatePass.RootTable.Columns["GrnNo"].Width = 50;
			GrdGatePass.RootTable.Columns["GpSrNo"].Width = 50;
			GrdGatePass.RootTable.Columns["GpDate"].Width = 70;
			GrdGatePass.RootTable.Columns["VehicleNo"].Width = 100;
			GrdGatePass.RootTable.Columns["BiltyNo"].Width = 100;
			GrdGatePass.RootTable.Columns["SupplierWeight"].Width = 75;
			GrdGatePass.RootTable.Columns["FactoryWeight"].Width = 75;
			GrdGatePass.RootTable.Columns["DifferenceWeight"].Width = 75;
			GrdGatePass.RootTable.Columns["VehicleInDate"].Width = 135;
			GrdGatePass.RootTable.Columns["VehicleOutDate"].Width = 135;
			GrdGatePass.RootTable.Columns["EntryUser"].Width = 100;
			GrdGatePass.RootTable.Columns["EntryDate"].Width = 135;
			GrdGatePass.RootTable.Columns["ModifyUser"].Width = 100;
			GrdGatePass.RootTable.Columns["ModifyDate"].Width = 135;
			GrdGatePass.RootTable.Columns["ApprovalUser"].Width = 100;
			GrdGatePass.RootTable.Columns["ApprovalDate"].Width = 135;
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
		try
		{
			grdWeighBridge.RootTable.Columns["WbId"].Visible = false;
			grdWeighBridge.RootTable.Columns["GatePassId"].Visible = false;
			grdWeighBridge.RootTable.Columns["TicketDate"].FormatString = "dd-MMM-yy";
			grdWeighBridge.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdWeighBridge.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdWeighBridge.RootTable.Columns["GatePassNumber"].Width = 50;
			grdWeighBridge.RootTable.Columns["TicketNo"].Width = 50;
			grdWeighBridge.RootTable.Columns["TicketDate"].Width = 70;
			grdWeighBridge.RootTable.Columns["FirstWeight"].Width = 75;
			grdWeighBridge.RootTable.Columns["SecondWeight"].Width = 75;
			grdWeighBridge.RootTable.Columns["NetWbWeight"].Width = 75;
			grdWeighBridge.RootTable.Columns["WbRemarks"].Width = 150;
			grdWeighBridge.RootTable.Columns["EntryUser"].Width = 100;
			grdWeighBridge.RootTable.Columns["EntryDate"].Width = 135;
			grdWeighBridge.RootTable.Columns["ModifyUser"].Width = 100;
			grdWeighBridge.RootTable.Columns["ModifyDate"].Width = 135;
			grdWeighBridge.RootTable.Columns["GatePassNumber"].ColumnType = (ColumnType)5;
			grdWeighBridge.RootTable.Columns["TicketNo"].ColumnType = (ColumnType)5;
			grdWeighBridge.RootTable.Columns["FirstWeight"].AggregateFunction = (AggregateFunction)2;
			grdWeighBridge.RootTable.Columns["SecondWeight"].AggregateFunction = (AggregateFunction)2;
			grdWeighBridge.RootTable.Columns["NetWbWeight"].AggregateFunction = (AggregateFunction)2;
			grdWeighBridge.RootTable.Columns["FirstWeight"].TextAlignment = (TextAlignment)3;
			grdWeighBridge.RootTable.Columns["SecondWeight"].TextAlignment = (TextAlignment)3;
			grdWeighBridge.RootTable.Columns["NetWbWeight"].TextAlignment = (TextAlignment)3;
			grdWeighBridge.RootTable.Columns["FirstWeight"].FormatString = "0,0";
			grdWeighBridge.RootTable.Columns["SecondWeight"].FormatString = "0,0";
			grdWeighBridge.RootTable.Columns["NetWbWeight"].FormatString = "0,0";
			grdWeighBridge.RootTable.Columns["FirstWeight"].TotalFormatString = "#,##0.##";
			grdWeighBridge.RootTable.Columns["SecondWeight"].TotalFormatString = "#,##0.##";
			grdWeighBridge.RootTable.Columns["NetWbWeight"].TotalFormatString = "#,##0.##";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingForLabInfo()
	{
		try
		{
			grdLabInfo.RootTable.Columns["Id"].Visible = false;
			grdLabInfo.RootTable.Columns["GatePassId"].Visible = false;
			grdLabInfo.RootTable.Columns["ItemId"].Visible = false;
			grdLabInfo.RootTable.Columns["StockAcId"].Visible = false;
			grdLabInfo.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yy";
			grdLabInfo.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdLabInfo.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdLabInfo.RootTable.Columns["ApprovalDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdLabInfo.RootTable.Columns["DocDate"].Width = 70;
			grdLabInfo.RootTable.Columns["DocNo"].Width = 50;
			grdLabInfo.RootTable.Columns["GatePassNumber"].Width = 50;
			grdLabInfo.RootTable.Columns["ItemName"].Width = 150;
			grdLabInfo.RootTable.Columns["DeductionWeight"].Width = 75;
			grdLabInfo.RootTable.Columns["DeductionRate"].Width = 75;
			grdLabInfo.RootTable.Columns["ItemRate"].Width = 75;
			grdLabInfo.RootTable.Columns["AnalystName"].Width = 100;
			grdLabInfo.RootTable.Columns["RemarksHeader"].Width = 200;
			grdLabInfo.RootTable.Columns["Crop"].Width = 100;
			grdLabInfo.RootTable.Columns["LabStatus"].Width = 100;
			grdLabInfo.RootTable.Columns["EntryUser"].Width = 80;
			grdLabInfo.RootTable.Columns["EntryDate"].Width = 135;
			grdLabInfo.RootTable.Columns["ModifyUser"].Width = 80;
			grdLabInfo.RootTable.Columns["ModifyDate"].Width = 135;
			grdLabInfo.RootTable.Columns["ApprovedUser"].Width = 80;
			grdLabInfo.RootTable.Columns["ApprovalDate"].Width = 135;
			grdLabInfo.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			grdLabInfo.RootTable.Columns["GatePassNumber"].ColumnType = (ColumnType)5;
			grdLabInfo.RootTable.Columns["ItemName"].ColumnType = (ColumnType)5;
			grdLabInfo.RootTable.Columns["DeductionRate"].TextAlignment = (TextAlignment)3;
			grdLabInfo.RootTable.Columns["DeductionRate"].FormatString = "#,##0.###";
			grdLabInfo.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdLabInfo.RootTable.Columns["ItemRate"].FormatString = "#,##0.###";
			grdLabInfo.RootTable.Columns["DeductionWeight"].AggregateFunction = (AggregateFunction)2;
			grdLabInfo.RootTable.Columns["DeductionWeight"].TextAlignment = (TextAlignment)3;
			grdLabInfo.RootTable.Columns["DeductionWeight"].FormatString = "#,##0.##";
			grdLabInfo.RootTable.Columns["DeductionWeight"].TotalFormatString = "#,##0.##";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingForWagesInfo()
	{
		try
		{
			grdWagesInfo.RootTable.Columns["Id"].Visible = false;
			grdWagesInfo.RootTable.Columns["GrnId"].Visible = false;
			grdWagesInfo.RootTable.Columns["ContractorId"].Visible = false;
			grdWagesInfo.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yy";
			grdWagesInfo.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdWagesInfo.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdWagesInfo.RootTable.Columns["DocNo"].Width = 50;
			grdWagesInfo.RootTable.Columns["DocDate"].Width = 70;
			grdWagesInfo.RootTable.Columns["GrnNumber"].Width = 50;
			grdWagesInfo.RootTable.Columns["ContractorName"].Width = 150;
			grdWagesInfo.RootTable.Columns["WagesAccountName"].Width = 150;
			grdWagesInfo.RootTable.Columns["Quantity"].Width = 60;
			grdWagesInfo.RootTable.Columns["Weight"].Width = 75;
			grdWagesInfo.RootTable.Columns["PackSize"].Width = 60;
			grdWagesInfo.RootTable.Columns["Rate"].Width = 60;
			grdWagesInfo.RootTable.Columns["Amount"].Width = 100;
			grdWagesInfo.RootTable.Columns["EntryUser"].Width = 80;
			grdWagesInfo.RootTable.Columns["EntryDate"].Width = 135;
			grdWagesInfo.RootTable.Columns["ModifyUser"].Width = 80;
			grdWagesInfo.RootTable.Columns["ModifyDate"].Width = 135;
			grdWagesInfo.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			grdWagesInfo.RootTable.Columns["ContractorName"].ColumnType = (ColumnType)5;
			grdWagesInfo.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdWagesInfo.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			grdWagesInfo.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdWagesInfo.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdWagesInfo.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			grdWagesInfo.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdWagesInfo.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdWagesInfo.RootTable.Columns["Rate"].FormatString = "#,##0.###";
			grdWagesInfo.RootTable.Columns["Amount"].FormatString = "#,##0.##";
			grdWagesInfo.RootTable.Columns["Quantity"].FormatString = "#,##0.##";
			grdWagesInfo.RootTable.Columns["Weight"].FormatString = "#,##0.##";
			grdWagesInfo.RootTable.Columns["Amount"].TotalFormatString = "#,##0.##";
			grdWagesInfo.RootTable.Columns["Quantity"].TotalFormatString = "#,##0.##";
			grdWagesInfo.RootTable.Columns["Weight"].TotalFormatString = "#,##0.##";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingsForOrder()
	{
		try
		{
			GrdOrderInfo.RootTable.Columns["PurchaeOrderId"].Visible = false;
			GrdOrderInfo.RootTable.Columns["OrderItemId"].Visible = false;
			GrdOrderInfo.RootTable.Columns["StockAcId"].Visible = false;
			GrdOrderInfo.RootTable.Columns["OrderDate"].FormatString = "dd-MMM-yy";
			GrdOrderInfo.RootTable.Columns["OrderExpiryDate"].FormatString = "dd-MMM-yy";
			GrdOrderInfo.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			GrdOrderInfo.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			GrdOrderInfo.RootTable.Columns["ApprovalDate"].FormatString = "dd-MMM-yy hh:mm tt";
			GrdOrderInfo.RootTable.Columns["OrderNo"].Width = 50;
			GrdOrderInfo.RootTable.Columns["OrderDate"].Width = 70;
			GrdOrderInfo.RootTable.Columns["OrderExpiryDate"].Width = 80;
			GrdOrderInfo.RootTable.Columns["PaymentTerm"].Width = 70;
			GrdOrderInfo.RootTable.Columns["DeliveryTerm"].Width = 70;
			GrdOrderInfo.RootTable.Columns["ExpiryStatus"].Width = 70;
			GrdOrderInfo.RootTable.Columns["OrderStatus"].Width = 70;
			GrdOrderInfo.RootTable.Columns["ItemName"].Width = 150;
			GrdOrderInfo.RootTable.Columns["PackUom"].Width = 60;
			GrdOrderInfo.RootTable.Columns["OrderItemQty"].Width = 60;
			GrdOrderInfo.RootTable.Columns["NetWeight"].Width = 75;
			GrdOrderInfo.RootTable.Columns["OrderItemRate"].Width = 100;
			GrdOrderInfo.RootTable.Columns["RateUom"].Width = 75;
			GrdOrderInfo.RootTable.Columns["Amount"].Width = 100;
			GrdOrderInfo.RootTable.Columns["Crop"].Width = 60;
			GrdOrderInfo.RootTable.Columns["LabSampleNo"].Width = 80;
			GrdOrderInfo.RootTable.Columns["DetailRemarks"].Width = 150;
			GrdOrderInfo.RootTable.Columns["EntryUser"].Width = 80;
			GrdOrderInfo.RootTable.Columns["EntryDate"].Width = 135;
			GrdOrderInfo.RootTable.Columns["ModifyUser"].Width = 80;
			GrdOrderInfo.RootTable.Columns["ModifyDate"].Width = 135;
			GrdOrderInfo.RootTable.Columns["ApprovedUser"].Width = 80;
			GrdOrderInfo.RootTable.Columns["ApprovalDate"].Width = 135;
			GrdOrderInfo.RootTable.Columns["OrderItemQty"].AggregateFunction = (AggregateFunction)2;
			GrdOrderInfo.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			GrdOrderInfo.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			GrdOrderInfo.RootTable.Columns["OrderItemRate"].TextAlignment = (TextAlignment)3;
			GrdOrderInfo.RootTable.Columns["OrderItemRate"].FormatString = "#,##0.###";
			GrdOrderInfo.RootTable.Columns["OrderItemQty"].TextAlignment = (TextAlignment)3;
			GrdOrderInfo.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			GrdOrderInfo.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			GrdOrderInfo.RootTable.Columns["OrderItemQty"].FormatString = "0,0";
			GrdOrderInfo.RootTable.Columns["NetWeight"].FormatString = "0,0";
			GrdOrderInfo.RootTable.Columns["Amount"].FormatString = "0,0";
			GrdOrderInfo.RootTable.Columns["OrderItemQty"].TotalFormatString = "#,##0.##";
			GrdOrderInfo.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			GrdOrderInfo.RootTable.Columns["Amount"].TotalFormatString = "#,##0.##";
			GrdOrderInfo.RootTable.Columns["OrderNo"].ColumnType = (ColumnType)5;
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
			CommonServices.PurchaseInvoicettSlip_220A(InvoiceId);
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
			CommonServices.GoToCustomerLedgerFromLinkedEvent(SupplierCustomerId, clsGlobalVariables.ActiveYr.Start_Period, DateTime.Now);
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
			CommonServices.GoToCustomerLedgerFromLinkedEvent(CommissionagentId, clsGlobalVariables.ActiveYr.Start_Period, DateTime.Now);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BrokerName_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.GoToCustomerLedgerFromLinkedEvent(BrokerAgentId, clsGlobalVariables.ActiveYr.Start_Period, DateTime.Now);
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
			GridEXRow currentRow = grdGrn.CurrentRow;
			int GrnId = Conversion.ToInt(currentRow.Cells["Id"].Value);
			int GatePassId = Conversion.ToInt(currentRow.Cells["GatePassId"].Value);
			int SupplierCustomerId = Conversion.ToInt(currentRow.Cells["SupplierCustomerId"].Value);
			if (e.Column.Key == "DocNo")
			{
				CommonServices.GrnSlipWithSubReports(GrnId);
			}
			if (e.Column.Key == "GatePassNumber")
			{
				CommonServices.GatePassInwardSlipAndRegisterReport(GatePassId);
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

	private void GrdGatePass_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int GatePassId = Conversion.ToInt(GrdGatePass.CurrentRow.Cells["GatePassId"].Value);
			if (e.Column.Key == "GpSrNo")
			{
				CommonServices.GatePassInwardSlipAndRegisterReport(GatePassId);
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
				CommonServices.GatePassInwardSlipAndRegisterReport(GatePassId);
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

	private void grdLabInfo_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = grdLabInfo.CurrentRow;
			int GatePassId = Conversion.ToInt(item.Cells["GatePassId"].Value);
			int Id = Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Column.Key == "GatePassNumber")
			{
				CommonServices.GatePassInwardSlipAndRegisterReport(GatePassId);
			}
			if (e.Column.Key == "DocNo")
			{
				CommonServices.LabAnalysisReport653(Id);
			}
			if (e.Column.Key == "ItemName")
			{
				Conversion.ToInt(item.Cells["StockAcId"].Value);
				CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["ItemId"].Value), clsGlobalVariables.ActiveYr.Start_Period, DateTime.Now);
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

	private void GrdOrderInfo_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(GrdOrderInfo.CurrentRow.Cells["PurchaeOrderId"].Value);
			if (e.Column.Key == "OrderNo")
			{
				new ActivityDetails(UserAccount, Id).Show();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGrn_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = grdGrn.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int GrnId = Conversion.ToInt(item.Cells["Id"].Value);
			int GatePassId = Conversion.ToInt(item.Cells["GatePassId"].Value);
			int SupplierCustomerId = Conversion.ToInt(item.Cells["SupplierCustomerId"].Value);
			if (grdGrn.CurrentColumn != null)
			{
				if (grdGrn.CurrentColumn.Key == "DocNo")
				{
					CommonServices.GrnSlipWithSubReports(GrnId);
				}
				else if (grdGrn.CurrentColumn.Key == "GatePassNumber")
				{
					CommonServices.GatePassInwardSlipAndRegisterReport(GatePassId);
				}
				else if (grdGrn.CurrentColumn.Key == "SupplierName")
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
					CommonServices.GatePassInwardSlipAndRegisterReport(GatePassId);
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
					CommonServices.GatePassInwardSlipAndRegisterReport(GatePassId);
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

	private void grdLabInfo_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = grdLabInfo.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int GatePassId = Conversion.ToInt(item.Cells["GatePassId"].Value);
			int Id = Conversion.ToInt(item.Cells["Id"].Value);
			if (grdLabInfo.CurrentColumn != null)
			{
				if (grdLabInfo.CurrentColumn.Key == "GatePassNumber")
				{
					CommonServices.GatePassInwardSlipAndRegisterReport(GatePassId);
				}
				else if (grdLabInfo.CurrentColumn.Key == "DocNo")
				{
					CommonServices.LabAnalysisReport653(Id);
				}
				else if (grdLabInfo.CurrentColumn.Key == "ItemName")
				{
					Conversion.ToInt(item.Cells["StockAcId"].Value);
					CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["ItemId"].Value), clsGlobalVariables.ActiveYr.Start_Period, DateTime.Now);
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

	private void GrdOrderInfo_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = GrdOrderInfo.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int Id = Conversion.ToInt(item.Cells["PurchaeOrderId"].Value);
				if (GrdOrderInfo.CurrentColumn != null && GrdOrderInfo.CurrentColumn.Key == "OrderNo")
				{
					new ActivityDetails(UserAccount, Id).Show();
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
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		try
		{
			if (GrdInvoiceInfo.RowCount <= 0)
			{
				return;
			}
			GridEXColumn RateCut = GrdInvoiceInfo.RootTable.Columns["RateCut"];
			GridEXColumn EbWeight = GrdInvoiceInfo.RootTable.Columns["EBTotalWt"];
			if (RateCut != null)
			{
				GridEXCell obj = e.Row.Cells[((JanusCollectionElement)RateCut).Index];
				obj.FormatStyle = new GridEXFormatStyle();
				obj.FormatStyle.ForeColor = Color.Red;
				obj.FormatStyle.FontBold = (TriState)2;
			}
			if (EbWeight != null)
			{
				GridEXCell EbCell = e.Row.Cells[((JanusCollectionElement)EbWeight).Index];
				if (Conversion.ToInt(EbCell.Value) != 0)
				{
					EbCell.FormatStyle = new GridEXFormatStyle();
					EbCell.FormatStyle.ForeColor = Color.Red;
					EbCell.FormatStyle.FontBold = (TriState)2;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGrn_FormattingRow(object sender, RowLoadEventArgs e)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		try
		{
			if (grdGrn.RowCount <= 0)
			{
				return;
			}
			GridEXColumn WeightCut = grdGrn.RootTable.Columns["WeightCut"];
			if (WeightCut != null)
			{
				GridEXCell WtCutCell = e.Row.Cells[((JanusCollectionElement)WeightCut).Index];
				if (Conversion.ToInt(WtCutCell.Value) != 0)
				{
					WtCutCell.FormatStyle = new GridEXFormatStyle();
					WtCutCell.FormatStyle.ForeColor = Color.Red;
					WtCutCell.FormatStyle.FontBold = (TriState)2;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdGatePass_FormattingRow(object sender, RowLoadEventArgs e)
	{
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
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_07cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Expected O, but got Unknown
		//IL_0a41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4b: Expected O, but got Unknown
		//IL_0cb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cbd: Expected O, but got Unknown
		//IL_0f27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f31: Expected O, but got Unknown
		//IL_119b: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a5: Expected O, but got Unknown
		//IL_11b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_11bc: Expected O, but got Unknown
		//IL_1426: Unknown result type (might be due to invalid IL or missing references)
		//IL_1430: Expected O, but got Unknown
		//IL_143d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1447: Expected O, but got Unknown
		//IL_16ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b5: Expected O, but got Unknown
		GridEXLayout GrdOrderInfo_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Commentaries.PurchaseInvoiceReverseActivity));
		GridEXLayout grdWagesInfo_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdLabInfo_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdWeighBridge_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdGatePass_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdGrn_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdInvoiceInfo_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.OrderInfoPanel = new System.Windows.Forms.Panel();
		this.GrdOrderInfo = new GridEX();
		this.label14 = new System.Windows.Forms.Label();
		this.WagesInfoPanel = new System.Windows.Forms.Panel();
		this.grdWagesInfo = new GridEX();
		this.label13 = new System.Windows.Forms.Label();
		this.LabInfoPanel = new System.Windows.Forms.Panel();
		this.grdLabInfo = new GridEX();
		this.label12 = new System.Windows.Forms.Label();
		this.WeighBrdgeDetailPanel = new System.Windows.Forms.Panel();
		this.grdWeighBridge = new GridEX();
		this.label11 = new System.Windows.Forms.Label();
		this.GatePassesDetailPanel = new System.Windows.Forms.Panel();
		this.GrdGatePass = new GridEX();
		this.label10 = new System.Windows.Forms.Label();
		this.GrnDetailPanel = new System.Windows.Forms.Panel();
		this.grdGrn = new GridEX();
		this.label9 = new System.Windows.Forms.Label();
		this.InvoiceInfoPanel = new System.Windows.Forms.Panel();
		this.GrdInvoiceInfo = new GridEX();
		this.label8 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.Less = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.ReferencePartyName = new System.Windows.Forms.Label();
		this.BillAmount = new System.Windows.Forms.Label();
		this.ManualBillNo = new System.Windows.Forms.Label();
		this.ItemAmount = new System.Windows.Forms.Label();
		this.Add = new System.Windows.Forms.Label();
		this.IsApproved = new System.Windows.Forms.Label();
		this.DueDays = new System.Windows.Forms.Label();
		this.DueDate = new System.Windows.Forms.Label();
		this.PaymentTerm = new System.Windows.Forms.Label();
		this.DeliveryTerm = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.ModifyDate = new System.Windows.Forms.Label();
		this.EntryDate = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.BrokeryAmount = new System.Windows.Forms.Label();
		this.lblBrokeryAmount = new System.Windows.Forms.Label();
		this.TotalCommAmount = new System.Windows.Forms.Label();
		this.lblCommAmount = new System.Windows.Forms.Label();
		this.BrokerName = new System.Windows.Forms.Label();
		this.lblBroker = new System.Windows.Forms.Label();
		this.CommissionAgentName = new System.Windows.Forms.Label();
		this.lblCommAgent = new System.Windows.Forms.Label();
		this.SupplierName = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.txtOrderType = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.ApprovalUserName = new System.Windows.Forms.Label();
		this.ModifyUserName = new System.Windows.Forms.Label();
		this.EntryUserName = new System.Windows.Forms.Label();
		this.lblApprovalUser = new System.Windows.Forms.Label();
		this.lblModifyUser = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.InvoiceAmount = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.DocNo = new System.Windows.Forms.Label();
		this.lbl = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.OrderInfoPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdOrderInfo).BeginInit();
		this.WagesInfoPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdWagesInfo).BeginInit();
		this.LabInfoPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdLabInfo).BeginInit();
		this.WeighBrdgeDetailPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdWeighBridge).BeginInit();
		this.GatePassesDetailPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdGatePass).BeginInit();
		this.GrnDetailPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGrn).BeginInit();
		this.InvoiceInfoPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdInvoiceInfo).BeginInit();
		this.panel2.SuspendLayout();
		this.panel4.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1370, 749);
		this.panel1.TabIndex = 0;
		this.panel3.AutoScroll = true;
		this.panel3.Controls.Add(this.OrderInfoPanel);
		this.panel3.Controls.Add(this.WagesInfoPanel);
		this.panel3.Controls.Add(this.LabInfoPanel);
		this.panel3.Controls.Add(this.WeighBrdgeDetailPanel);
		this.panel3.Controls.Add(this.GatePassesDetailPanel);
		this.panel3.Controls.Add(this.GrnDetailPanel);
		this.panel3.Controls.Add(this.InvoiceInfoPanel);
		this.panel3.Controls.Add(this.label6);
		this.panel3.Controls.Add(this.label5);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 213);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1370, 536);
		this.panel3.TabIndex = 1;
		this.OrderInfoPanel.Controls.Add((System.Windows.Forms.Control)(object)this.GrdOrderInfo);
		this.OrderInfoPanel.Controls.Add(this.label14);
		this.OrderInfoPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.OrderInfoPanel.Location = new System.Drawing.Point(0, 1086);
		this.OrderInfoPanel.Name = "OrderInfoPanel";
		this.OrderInfoPanel.Size = new System.Drawing.Size(1353, 180);
		this.OrderInfoPanel.TabIndex = 15;
		this.GrdOrderInfo.AllowEdit = (InheritableBoolean)2;
		this.GrdOrderInfo.AlternatingColors = true;
		((ScrollableBase)this.GrdOrderInfo).BackColor = System.Drawing.Color.White;
		GrdOrderInfo_DesignTimeLayout.LayoutString = resources.GetString("GrdOrderInfo_DesignTimeLayout.LayoutString");
		this.GrdOrderInfo.DesignTimeLayout = GrdOrderInfo_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdOrderInfo).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdOrderInfo.DynamicFiltering = true;
		this.GrdOrderInfo.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdOrderInfo).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdOrderInfo.GroupByBoxVisible = false;
		this.GrdOrderInfo.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.GrdOrderInfo).Location = new System.Drawing.Point(0, 30);
		((System.Windows.Forms.Control)(object)this.GrdOrderInfo).Name = "GrdOrderInfo";
		((ScrollableBase)this.GrdOrderInfo).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdOrderInfo).Size = new System.Drawing.Size(1353, 150);
		((System.Windows.Forms.Control)(object)this.GrdOrderInfo).TabIndex = 7;
		this.GrdOrderInfo.TotalRow = (InheritableBoolean)1;
		this.GrdOrderInfo.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdOrderInfo).VisualStyle = (VisualStyle)4;
		this.GrdOrderInfo.LinkClicked += new ColumnActionEventHandler(GrdOrderInfo_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdOrderInfo).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdOrderInfo_KeyDown);
		this.label14.BackColor = System.Drawing.Color.Teal;
		this.label14.Dock = System.Windows.Forms.DockStyle.Top;
		this.label14.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(0, 0);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(1353, 30);
		this.label14.TabIndex = 5;
		this.label14.Text = "Order Information";
		this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.WagesInfoPanel.Controls.Add((System.Windows.Forms.Control)(object)this.grdWagesInfo);
		this.WagesInfoPanel.Controls.Add(this.label13);
		this.WagesInfoPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.WagesInfoPanel.Location = new System.Drawing.Point(0, 906);
		this.WagesInfoPanel.Name = "WagesInfoPanel";
		this.WagesInfoPanel.Size = new System.Drawing.Size(1353, 180);
		this.WagesInfoPanel.TabIndex = 14;
		this.grdWagesInfo.AllowEdit = (InheritableBoolean)2;
		this.grdWagesInfo.AlternatingColors = true;
		((ScrollableBase)this.grdWagesInfo).BackColor = System.Drawing.Color.White;
		grdWagesInfo_DesignTimeLayout.LayoutString = resources.GetString("grdWagesInfo_DesignTimeLayout.LayoutString");
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
		((System.Windows.Forms.Control)(object)this.grdWagesInfo).Size = new System.Drawing.Size(1353, 150);
		((System.Windows.Forms.Control)(object)this.grdWagesInfo).TabIndex = 7;
		this.grdWagesInfo.TotalRow = (InheritableBoolean)1;
		this.grdWagesInfo.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdWagesInfo).VisualStyle = (VisualStyle)4;
		this.grdWagesInfo.LinkClicked += new ColumnActionEventHandler(grdWagesInfo_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdWagesInfo).KeyDown += new System.Windows.Forms.KeyEventHandler(grdWagesInfo_KeyDown);
		this.label13.BackColor = System.Drawing.Color.Teal;
		this.label13.Dock = System.Windows.Forms.DockStyle.Top;
		this.label13.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(0, 0);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(1353, 30);
		this.label13.TabIndex = 5;
		this.label13.Text = "Wages Information";
		this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.LabInfoPanel.Controls.Add((System.Windows.Forms.Control)(object)this.grdLabInfo);
		this.LabInfoPanel.Controls.Add(this.label12);
		this.LabInfoPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.LabInfoPanel.Location = new System.Drawing.Point(0, 726);
		this.LabInfoPanel.Name = "LabInfoPanel";
		this.LabInfoPanel.Size = new System.Drawing.Size(1353, 180);
		this.LabInfoPanel.TabIndex = 13;
		this.grdLabInfo.AllowEdit = (InheritableBoolean)2;
		this.grdLabInfo.AlternatingColors = true;
		((ScrollableBase)this.grdLabInfo).BackColor = System.Drawing.Color.White;
		grdLabInfo_DesignTimeLayout.LayoutString = resources.GetString("grdLabInfo_DesignTimeLayout.LayoutString");
		this.grdLabInfo.DesignTimeLayout = grdLabInfo_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdLabInfo).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdLabInfo.DynamicFiltering = true;
		this.grdLabInfo.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdLabInfo).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdLabInfo.GroupByBoxVisible = false;
		this.grdLabInfo.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.grdLabInfo).Location = new System.Drawing.Point(0, 30);
		((System.Windows.Forms.Control)(object)this.grdLabInfo).Name = "grdLabInfo";
		((ScrollableBase)this.grdLabInfo).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdLabInfo).Size = new System.Drawing.Size(1353, 150);
		((System.Windows.Forms.Control)(object)this.grdLabInfo).TabIndex = 6;
		this.grdLabInfo.TotalRow = (InheritableBoolean)1;
		this.grdLabInfo.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdLabInfo).VisualStyle = (VisualStyle)4;
		this.grdLabInfo.LinkClicked += new ColumnActionEventHandler(grdLabInfo_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdLabInfo).KeyDown += new System.Windows.Forms.KeyEventHandler(grdLabInfo_KeyDown);
		this.label12.BackColor = System.Drawing.Color.Teal;
		this.label12.Dock = System.Windows.Forms.DockStyle.Top;
		this.label12.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(0, 0);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(1353, 30);
		this.label12.TabIndex = 5;
		this.label12.Text = "Lab Information";
		this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.WeighBrdgeDetailPanel.Controls.Add((System.Windows.Forms.Control)(object)this.grdWeighBridge);
		this.WeighBrdgeDetailPanel.Controls.Add(this.label11);
		this.WeighBrdgeDetailPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.WeighBrdgeDetailPanel.Location = new System.Drawing.Point(0, 546);
		this.WeighBrdgeDetailPanel.Name = "WeighBrdgeDetailPanel";
		this.WeighBrdgeDetailPanel.Size = new System.Drawing.Size(1353, 180);
		this.WeighBrdgeDetailPanel.TabIndex = 12;
		this.grdWeighBridge.AllowEdit = (InheritableBoolean)2;
		this.grdWeighBridge.AlternatingColors = true;
		((ScrollableBase)this.grdWeighBridge).BackColor = System.Drawing.Color.White;
		grdWeighBridge_DesignTimeLayout.LayoutString = resources.GetString("grdWeighBridge_DesignTimeLayout.LayoutString");
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
		((System.Windows.Forms.Control)(object)this.grdWeighBridge).Size = new System.Drawing.Size(1353, 150);
		((System.Windows.Forms.Control)(object)this.grdWeighBridge).TabIndex = 4;
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
		this.label11.Size = new System.Drawing.Size(1353, 30);
		this.label11.TabIndex = 5;
		this.label11.Text = "Weigh Bridge Detail";
		this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.GatePassesDetailPanel.Controls.Add((System.Windows.Forms.Control)(object)this.GrdGatePass);
		this.GatePassesDetailPanel.Controls.Add(this.label10);
		this.GatePassesDetailPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.GatePassesDetailPanel.Location = new System.Drawing.Point(0, 366);
		this.GatePassesDetailPanel.Name = "GatePassesDetailPanel";
		this.GatePassesDetailPanel.Size = new System.Drawing.Size(1353, 180);
		this.GatePassesDetailPanel.TabIndex = 11;
		this.GrdGatePass.AllowEdit = (InheritableBoolean)2;
		this.GrdGatePass.AlternatingColors = true;
		((ScrollableBase)this.GrdGatePass).BackColor = System.Drawing.Color.White;
		GrdGatePass_DesignTimeLayout.LayoutString = resources.GetString("GrdGatePass_DesignTimeLayout.LayoutString");
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
		((System.Windows.Forms.Control)(object)this.GrdGatePass).Size = new System.Drawing.Size(1353, 150);
		((System.Windows.Forms.Control)(object)this.GrdGatePass).TabIndex = 5;
		this.GrdGatePass.TotalRow = (InheritableBoolean)1;
		this.GrdGatePass.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdGatePass).VisualStyle = (VisualStyle)4;
		this.GrdGatePass.FormattingRow += new RowLoadEventHandler(GrdGatePass_FormattingRow);
		this.GrdGatePass.LinkClicked += new ColumnActionEventHandler(GrdGatePass_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdGatePass).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdGatePass_KeyDown);
		this.label10.BackColor = System.Drawing.Color.Teal;
		this.label10.Dock = System.Windows.Forms.DockStyle.Top;
		this.label10.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.White;
		this.label10.Location = new System.Drawing.Point(0, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(1353, 30);
		this.label10.TabIndex = 5;
		this.label10.Text = "Gate Passes Detail";
		this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.GrnDetailPanel.Controls.Add((System.Windows.Forms.Control)(object)this.grdGrn);
		this.GrnDetailPanel.Controls.Add(this.label9);
		this.GrnDetailPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.GrnDetailPanel.Location = new System.Drawing.Point(0, 186);
		this.GrnDetailPanel.Name = "GrnDetailPanel";
		this.GrnDetailPanel.Size = new System.Drawing.Size(1353, 180);
		this.GrnDetailPanel.TabIndex = 10;
		this.grdGrn.AllowEdit = (InheritableBoolean)2;
		this.grdGrn.AlternatingColors = true;
		((ScrollableBase)this.grdGrn).BackColor = System.Drawing.Color.White;
		grdGrn_DesignTimeLayout.LayoutString = resources.GetString("grdGrn_DesignTimeLayout.LayoutString");
		this.grdGrn.DesignTimeLayout = grdGrn_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGrn).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdGrn.DynamicFiltering = true;
		this.grdGrn.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdGrn).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGrn.GroupByBoxVisible = false;
		this.grdGrn.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.grdGrn).Location = new System.Drawing.Point(0, 30);
		((System.Windows.Forms.Control)(object)this.grdGrn).Name = "grdGrn";
		((ScrollableBase)this.grdGrn).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGrn).Size = new System.Drawing.Size(1353, 150);
		((System.Windows.Forms.Control)(object)this.grdGrn).TabIndex = 5;
		this.grdGrn.TotalRow = (InheritableBoolean)1;
		this.grdGrn.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGrn).VisualStyle = (VisualStyle)4;
		this.grdGrn.FormattingRow += new RowLoadEventHandler(grdGrn_FormattingRow);
		this.grdGrn.LinkClicked += new ColumnActionEventHandler(grdGrn_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdGrn).KeyDown += new System.Windows.Forms.KeyEventHandler(grdGrn_KeyDown);
		this.label9.BackColor = System.Drawing.Color.Teal;
		this.label9.Dock = System.Windows.Forms.DockStyle.Top;
		this.label9.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(0, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(1353, 30);
		this.label9.TabIndex = 5;
		this.label9.Text = "Grn Detail";
		this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.InvoiceInfoPanel.Controls.Add((System.Windows.Forms.Control)(object)this.GrdInvoiceInfo);
		this.InvoiceInfoPanel.Controls.Add(this.label8);
		this.InvoiceInfoPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.InvoiceInfoPanel.Location = new System.Drawing.Point(0, 36);
		this.InvoiceInfoPanel.Name = "InvoiceInfoPanel";
		this.InvoiceInfoPanel.Size = new System.Drawing.Size(1353, 150);
		this.InvoiceInfoPanel.TabIndex = 9;
		this.GrdInvoiceInfo.AllowEdit = (InheritableBoolean)2;
		this.GrdInvoiceInfo.AlternatingColors = true;
		((ScrollableBase)this.GrdInvoiceInfo).BackColor = System.Drawing.Color.White;
		GrdInvoiceInfo_DesignTimeLayout.LayoutString = resources.GetString("GrdInvoiceInfo_DesignTimeLayout.LayoutString");
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
		((System.Windows.Forms.Control)(object)this.GrdInvoiceInfo).Size = new System.Drawing.Size(1353, 120);
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
		this.label8.Size = new System.Drawing.Size(1353, 30);
		this.label8.TabIndex = 5;
		this.label8.Text = "Invoice Info";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label6.BackColor = System.Drawing.Color.Teal;
		this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label6.Dock = System.Windows.Forms.DockStyle.Top;
		this.label6.ForeColor = System.Drawing.Color.Black;
		this.label6.Location = new System.Drawing.Point(0, 34);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(1353, 2);
		this.label6.TabIndex = 3;
		this.label5.Dock = System.Windows.Forms.DockStyle.Top;
		this.label5.Font = new System.Drawing.Font("Calibri", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.Teal;
		this.label5.Location = new System.Drawing.Point(0, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(1353, 34);
		this.label5.TabIndex = 2;
		this.label5.Text = "Purchase Invoice Information";
		this.label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
		this.panel2.Controls.Add(this.Less);
		this.panel2.Controls.Add(this.label27);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.ReferencePartyName);
		this.panel2.Controls.Add(this.BillAmount);
		this.panel2.Controls.Add(this.ManualBillNo);
		this.panel2.Controls.Add(this.ItemAmount);
		this.panel2.Controls.Add(this.Add);
		this.panel2.Controls.Add(this.IsApproved);
		this.panel2.Controls.Add(this.DueDays);
		this.panel2.Controls.Add(this.DueDate);
		this.panel2.Controls.Add(this.PaymentTerm);
		this.panel2.Controls.Add(this.DeliveryTerm);
		this.panel2.Controls.Add(this.label26);
		this.panel2.Controls.Add(this.label21);
		this.panel2.Controls.Add(this.label22);
		this.panel2.Controls.Add(this.label23);
		this.panel2.Controls.Add(this.label25);
		this.panel2.Controls.Add(this.ModifyDate);
		this.panel2.Controls.Add(this.EntryDate);
		this.panel2.Controls.Add(this.label20);
		this.panel2.Controls.Add(this.label19);
		this.panel2.Controls.Add(this.label49);
		this.panel2.Controls.Add(this.label51);
		this.panel2.Controls.Add(this.label48);
		this.panel2.Controls.Add(this.label17);
		this.panel2.Controls.Add(this.label16);
		this.panel2.Controls.Add(this.label7);
		this.panel2.Controls.Add(this.BrokeryAmount);
		this.panel2.Controls.Add(this.lblBrokeryAmount);
		this.panel2.Controls.Add(this.TotalCommAmount);
		this.panel2.Controls.Add(this.lblCommAmount);
		this.panel2.Controls.Add(this.BrokerName);
		this.panel2.Controls.Add(this.lblBroker);
		this.panel2.Controls.Add(this.CommissionAgentName);
		this.panel2.Controls.Add(this.lblCommAgent);
		this.panel2.Controls.Add(this.SupplierName);
		this.panel2.Controls.Add(this.label15);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.ApprovalUserName);
		this.panel2.Controls.Add(this.ModifyUserName);
		this.panel2.Controls.Add(this.EntryUserName);
		this.panel2.Controls.Add(this.lblApprovalUser);
		this.panel2.Controls.Add(this.lblModifyUser);
		this.panel2.Controls.Add(this.label18);
		this.panel2.Controls.Add(this.InvoiceAmount);
		this.panel2.Controls.Add(this.DocDate);
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add(this.DocNo);
		this.panel2.Controls.Add(this.lbl);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1370, 213);
		this.panel2.TabIndex = 0;
		this.Less.AutoSize = true;
		this.Less.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Less.ForeColor = System.Drawing.Color.Black;
		this.Less.Location = new System.Drawing.Point(977, 112);
		this.Less.Name = "Less";
		this.Less.Size = new System.Drawing.Size(30, 15);
		this.Less.TabIndex = 70;
		this.Less.Text = "Less";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(847, 112);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(30, 15);
		this.label27.TabIndex = 69;
		this.label27.Text = "Less";
		this.label3.BackColor = System.Drawing.Color.Teal;
		this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label3.ForeColor = System.Drawing.Color.Black;
		this.label3.Location = new System.Drawing.Point(2, 45);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(1170, 2);
		this.label3.TabIndex = 68;
		this.ReferencePartyName.AutoSize = true;
		this.ReferencePartyName.Cursor = System.Windows.Forms.Cursors.Hand;
		this.ReferencePartyName.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ReferencePartyName.ForeColor = System.Drawing.Color.Blue;
		this.ReferencePartyName.Location = new System.Drawing.Point(138, 54);
		this.ReferencePartyName.Name = "ReferencePartyName";
		this.ReferencePartyName.Size = new System.Drawing.Size(40, 15);
		this.ReferencePartyName.TabIndex = 67;
		this.ReferencePartyName.Text = "Name";
		this.BillAmount.AutoSize = true;
		this.BillAmount.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BillAmount.ForeColor = System.Drawing.Color.Blue;
		this.BillAmount.Location = new System.Drawing.Point(1274, 109);
		this.BillAmount.Name = "BillAmount";
		this.BillAmount.Size = new System.Drawing.Size(93, 18);
		this.BillAmount.TabIndex = 66;
		this.BillAmount.Text = "Bill Amount";
		this.BillAmount.Visible = false;
		this.ManualBillNo.AutoSize = true;
		this.ManualBillNo.Cursor = System.Windows.Forms.Cursors.Hand;
		this.ManualBillNo.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ManualBillNo.ForeColor = System.Drawing.Color.Blue;
		this.ManualBillNo.Location = new System.Drawing.Point(138, 112);
		this.ManualBillNo.Name = "ManualBillNo";
		this.ManualBillNo.Size = new System.Drawing.Size(14, 15);
		this.ManualBillNo.TabIndex = 65;
		this.ManualBillNo.Text = "1";
		this.ItemAmount.AutoSize = true;
		this.ItemAmount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ItemAmount.ForeColor = System.Drawing.Color.Black;
		this.ItemAmount.Location = new System.Drawing.Point(977, 74);
		this.ItemAmount.Name = "ItemAmount";
		this.ItemAmount.Size = new System.Drawing.Size(81, 15);
		this.ItemAmount.TabIndex = 63;
		this.ItemAmount.Text = "Item Amount";
		this.Add.AutoSize = true;
		this.Add.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Add.ForeColor = System.Drawing.Color.Black;
		this.Add.Location = new System.Drawing.Point(977, 93);
		this.Add.Name = "Add";
		this.Add.Size = new System.Drawing.Size(85, 15);
		this.Add.TabIndex = 62;
		this.Add.Text = "OtherAddLess";
		this.IsApproved.AutoSize = true;
		this.IsApproved.BackColor = System.Drawing.Color.Transparent;
		this.IsApproved.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.IsApproved.ForeColor = System.Drawing.SystemColors.ControlText;
		this.IsApproved.Location = new System.Drawing.Point(648, 151);
		this.IsApproved.Name = "IsApproved";
		this.IsApproved.Size = new System.Drawing.Size(69, 15);
		this.IsApproved.TabIndex = 61;
		this.IsApproved.Text = "IsApproved";
		this.DueDays.AutoSize = true;
		this.DueDays.BackColor = System.Drawing.Color.Transparent;
		this.DueDays.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.DueDays.ForeColor = System.Drawing.SystemColors.ControlText;
		this.DueDays.Location = new System.Drawing.Point(402, 74);
		this.DueDays.Name = "DueDays";
		this.DueDays.Size = new System.Drawing.Size(57, 15);
		this.DueDays.TabIndex = 60;
		this.DueDays.Text = "Due Days";
		this.DueDate.AutoSize = true;
		this.DueDate.BackColor = System.Drawing.Color.Transparent;
		this.DueDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.DueDate.ForeColor = System.Drawing.SystemColors.ControlText;
		this.DueDate.Location = new System.Drawing.Point(402, 93);
		this.DueDate.Name = "DueDate";
		this.DueDate.Size = new System.Drawing.Size(58, 15);
		this.DueDate.TabIndex = 59;
		this.DueDate.Text = "Due Date";
		this.PaymentTerm.AutoSize = true;
		this.PaymentTerm.BackColor = System.Drawing.Color.Transparent;
		this.PaymentTerm.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.PaymentTerm.ForeColor = System.Drawing.SystemColors.ControlText;
		this.PaymentTerm.Location = new System.Drawing.Point(402, 54);
		this.PaymentTerm.Name = "PaymentTerm";
		this.PaymentTerm.Size = new System.Drawing.Size(87, 15);
		this.PaymentTerm.TabIndex = 58;
		this.PaymentTerm.Text = "Payment Term";
		this.DeliveryTerm.AutoSize = true;
		this.DeliveryTerm.BackColor = System.Drawing.Color.Transparent;
		this.DeliveryTerm.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.DeliveryTerm.ForeColor = System.Drawing.SystemColors.ControlText;
		this.DeliveryTerm.Location = new System.Drawing.Point(402, 112);
		this.DeliveryTerm.Name = "DeliveryTerm";
		this.DeliveryTerm.Size = new System.Drawing.Size(83, 15);
		this.DeliveryTerm.TabIndex = 57;
		this.DeliveryTerm.Text = "Delivery Term";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(557, 151);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(69, 15);
		this.label26.TabIndex = 56;
		this.label26.Text = "IsApproved";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(847, 93);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(29, 15);
		this.label21.TabIndex = 55;
		this.label21.Text = "Add";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(1144, 111);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(128, 15);
		this.label22.TabIndex = 54;
		this.label22.Text = "Customer Bill Amount";
		this.label22.Visible = false;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(847, 131);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(100, 15);
		this.label23.TabIndex = 53;
		this.label23.Text = "Total Bill Amount";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(847, 74);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(85, 15);
		this.label25.TabIndex = 51;
		this.label25.Text = "Item Amount:";
		this.ModifyDate.AutoSize = true;
		this.ModifyDate.BackColor = System.Drawing.Color.Transparent;
		this.ModifyDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ModifyDate.ForeColor = System.Drawing.SystemColors.ControlText;
		this.ModifyDate.Location = new System.Drawing.Point(648, 112);
		this.ModifyDate.Name = "ModifyDate";
		this.ModifyDate.Size = new System.Drawing.Size(74, 15);
		this.ModifyDate.TabIndex = 50;
		this.ModifyDate.Text = "Modify Date";
		this.EntryDate.AutoSize = true;
		this.EntryDate.BackColor = System.Drawing.Color.Transparent;
		this.EntryDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.EntryDate.ForeColor = System.Drawing.SystemColors.ControlText;
		this.EntryDate.Location = new System.Drawing.Point(648, 74);
		this.EntryDate.Name = "EntryDate";
		this.EntryDate.Size = new System.Drawing.Size(65, 15);
		this.EntryDate.TabIndex = 49;
		this.EntryDate.Text = "Entry Date";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(557, 74);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(65, 15);
		this.label20.TabIndex = 48;
		this.label20.Text = "Entry Date";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(557, 112);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(74, 15);
		this.label19.TabIndex = 47;
		this.label19.Text = "Modify Date";
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(304, 112);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(87, 15);
		this.label49.TabIndex = 46;
		this.label49.Text = "Delivery Term:";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(304, 93);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(62, 15);
		this.label51.TabIndex = 45;
		this.label51.Text = "Due Date:";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(304, 54);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(91, 15);
		this.label48.TabIndex = 44;
		this.label48.Text = "Payment Term:";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(304, 74);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(61, 15);
		this.label17.TabIndex = 43;
		this.label17.Text = "Due Days:";
		this.label16.AutoSize = true;
		this.label16.BackColor = System.Drawing.Color.Transparent;
		this.label16.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label16.Location = new System.Drawing.Point(12, 112);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(89, 15);
		this.label16.TabIndex = 42;
		this.label16.Text = "Manual Bill No:";
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.Transparent;
		this.label7.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label7.Location = new System.Drawing.Point(12, 54);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(95, 15);
		this.label7.TabIndex = 41;
		this.label7.Text = "Reference Party";
		this.BrokeryAmount.AutoSize = true;
		this.BrokeryAmount.Cursor = System.Windows.Forms.Cursors.Default;
		this.BrokeryAmount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BrokeryAmount.ForeColor = System.Drawing.Color.Black;
		this.BrokeryAmount.Location = new System.Drawing.Point(138, 190);
		this.BrokeryAmount.Name = "BrokeryAmount";
		this.BrokeryAmount.Size = new System.Drawing.Size(14, 15);
		this.BrokeryAmount.TabIndex = 40;
		this.BrokeryAmount.Text = "0";
		this.lblBrokeryAmount.AutoSize = true;
		this.lblBrokeryAmount.BackColor = System.Drawing.Color.Transparent;
		this.lblBrokeryAmount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBrokeryAmount.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lblBrokeryAmount.Location = new System.Drawing.Point(12, 190);
		this.lblBrokeryAmount.Name = "lblBrokeryAmount";
		this.lblBrokeryAmount.Size = new System.Drawing.Size(99, 15);
		this.lblBrokeryAmount.TabIndex = 39;
		this.lblBrokeryAmount.Text = "Broker Amount: ";
		this.TotalCommAmount.AutoSize = true;
		this.TotalCommAmount.Cursor = System.Windows.Forms.Cursors.Default;
		this.TotalCommAmount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.TotalCommAmount.ForeColor = System.Drawing.Color.Black;
		this.TotalCommAmount.Location = new System.Drawing.Point(138, 151);
		this.TotalCommAmount.Name = "TotalCommAmount";
		this.TotalCommAmount.Size = new System.Drawing.Size(14, 15);
		this.TotalCommAmount.TabIndex = 38;
		this.TotalCommAmount.Text = "0";
		this.lblCommAmount.AutoSize = true;
		this.lblCommAmount.BackColor = System.Drawing.Color.Transparent;
		this.lblCommAmount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCommAmount.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lblCommAmount.Location = new System.Drawing.Point(12, 151);
		this.lblCommAmount.Name = "lblCommAmount";
		this.lblCommAmount.Size = new System.Drawing.Size(128, 15);
		this.lblCommAmount.TabIndex = 37;
		this.lblCommAmount.Text = "Commission Amount: ";
		this.BrokerName.AutoSize = true;
		this.BrokerName.Cursor = System.Windows.Forms.Cursors.Hand;
		this.BrokerName.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BrokerName.ForeColor = System.Drawing.Color.Blue;
		this.BrokerName.Location = new System.Drawing.Point(138, 171);
		this.BrokerName.Name = "BrokerName";
		this.BrokerName.Size = new System.Drawing.Size(40, 15);
		this.BrokerName.TabIndex = 36;
		this.BrokerName.Text = "Name";
		this.BrokerName.Click += new System.EventHandler(BrokerName_Click);
		this.lblBroker.AutoSize = true;
		this.lblBroker.BackColor = System.Drawing.Color.Transparent;
		this.lblBroker.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBroker.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lblBroker.Location = new System.Drawing.Point(12, 171);
		this.lblBroker.Name = "lblBroker";
		this.lblBroker.Size = new System.Drawing.Size(87, 15);
		this.lblBroker.TabIndex = 35;
		this.lblBroker.Text = "Broker Name: ";
		this.CommissionAgentName.AutoSize = true;
		this.CommissionAgentName.Cursor = System.Windows.Forms.Cursors.Hand;
		this.CommissionAgentName.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.CommissionAgentName.ForeColor = System.Drawing.Color.Blue;
		this.CommissionAgentName.Location = new System.Drawing.Point(138, 131);
		this.CommissionAgentName.Name = "CommissionAgentName";
		this.CommissionAgentName.Size = new System.Drawing.Size(40, 15);
		this.CommissionAgentName.TabIndex = 34;
		this.CommissionAgentName.Text = "Name";
		this.CommissionAgentName.Click += new System.EventHandler(CommissionAgentName_Click);
		this.lblCommAgent.AutoSize = true;
		this.lblCommAgent.BackColor = System.Drawing.Color.Transparent;
		this.lblCommAgent.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCommAgent.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lblCommAgent.Location = new System.Drawing.Point(12, 131);
		this.lblCommAgent.Name = "lblCommAgent";
		this.lblCommAgent.Size = new System.Drawing.Size(116, 15);
		this.lblCommAgent.TabIndex = 33;
		this.lblCommAgent.Text = "Commission Agent: ";
		this.SupplierName.AutoSize = true;
		this.SupplierName.Cursor = System.Windows.Forms.Cursors.Hand;
		this.SupplierName.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.SupplierName.ForeColor = System.Drawing.Color.Blue;
		this.SupplierName.Location = new System.Drawing.Point(977, 53);
		this.SupplierName.Name = "SupplierName";
		this.SupplierName.Size = new System.Drawing.Size(42, 16);
		this.SupplierName.TabIndex = 32;
		this.SupplierName.Text = "Name";
		this.SupplierName.Click += new System.EventHandler(SupplierName_Click);
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.Transparent;
		this.label15.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label15.Location = new System.Drawing.Point(847, 54);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(95, 15);
		this.label15.TabIndex = 31;
		this.label15.Text = "Supplier Name: ";
		this.panel4.Controls.Add(this.txtOrderType);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1370, 39);
		this.panel4.TabIndex = 30;
		this.txtOrderType.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtOrderType.Font = new System.Drawing.Font("Calibri", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderType.ForeColor = System.Drawing.Color.Black;
		this.txtOrderType.Location = new System.Drawing.Point(583, 0);
		this.txtOrderType.Name = "txtOrderType";
		this.txtOrderType.Size = new System.Drawing.Size(787, 39);
		this.txtOrderType.TabIndex = 2;
		this.txtOrderType.Text = "OrderType";
		this.txtOrderType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label1.Dock = System.Windows.Forms.DockStyle.Left;
		this.label1.Font = new System.Drawing.Font("Calibri", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Teal;
		this.label1.Location = new System.Drawing.Point(0, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(583, 39);
		this.label1.TabIndex = 1;
		this.label1.Text = "Purchase Invoice Reverse Activity";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.ApprovalUserName.AutoSize = true;
		this.ApprovalUserName.BackColor = System.Drawing.Color.Transparent;
		this.ApprovalUserName.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ApprovalUserName.ForeColor = System.Drawing.SystemColors.ControlText;
		this.ApprovalUserName.Location = new System.Drawing.Point(648, 131);
		this.ApprovalUserName.Name = "ApprovalUserName";
		this.ApprovalUserName.Size = new System.Drawing.Size(40, 15);
		this.ApprovalUserName.TabIndex = 29;
		this.ApprovalUserName.Text = "Name";
		this.ModifyUserName.AutoSize = true;
		this.ModifyUserName.BackColor = System.Drawing.Color.Transparent;
		this.ModifyUserName.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ModifyUserName.ForeColor = System.Drawing.SystemColors.ControlText;
		this.ModifyUserName.Location = new System.Drawing.Point(648, 93);
		this.ModifyUserName.Name = "ModifyUserName";
		this.ModifyUserName.Size = new System.Drawing.Size(103, 15);
		this.ModifyUserName.TabIndex = 28;
		this.ModifyUserName.Text = "ModifyUserName";
		this.EntryUserName.AutoSize = true;
		this.EntryUserName.BackColor = System.Drawing.Color.Transparent;
		this.EntryUserName.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.EntryUserName.ForeColor = System.Drawing.SystemColors.ControlText;
		this.EntryUserName.Location = new System.Drawing.Point(648, 54);
		this.EntryUserName.Name = "EntryUserName";
		this.EntryUserName.Size = new System.Drawing.Size(122, 15);
		this.EntryUserName.TabIndex = 27;
		this.EntryUserName.Text = "Load & Factory Weight";
		this.lblApprovalUser.AutoSize = true;
		this.lblApprovalUser.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblApprovalUser.Location = new System.Drawing.Point(557, 131);
		this.lblApprovalUser.Name = "lblApprovalUser";
		this.lblApprovalUser.Size = new System.Drawing.Size(84, 15);
		this.lblApprovalUser.TabIndex = 26;
		this.lblApprovalUser.Text = "Approval User";
		this.lblModifyUser.AutoSize = true;
		this.lblModifyUser.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblModifyUser.Location = new System.Drawing.Point(557, 93);
		this.lblModifyUser.Name = "lblModifyUser";
		this.lblModifyUser.Size = new System.Drawing.Size(73, 15);
		this.lblModifyUser.TabIndex = 25;
		this.lblModifyUser.Text = "Modify User";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(557, 54);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(64, 15);
		this.label18.TabIndex = 24;
		this.label18.Text = "Entry User";
		this.InvoiceAmount.AutoSize = true;
		this.InvoiceAmount.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.InvoiceAmount.ForeColor = System.Drawing.Color.Teal;
		this.InvoiceAmount.Location = new System.Drawing.Point(977, 129);
		this.InvoiceAmount.Name = "InvoiceAmount";
		this.InvoiceAmount.Size = new System.Drawing.Size(124, 18);
		this.InvoiceAmount.TabIndex = 11;
		this.InvoiceAmount.Text = "Invoice Amount";
		this.DocDate.AutoSize = true;
		this.DocDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.ForeColor = System.Drawing.Color.Black;
		this.DocDate.Location = new System.Drawing.Point(138, 93);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(56, 15);
		this.DocDate.TabIndex = 9;
		this.DocDate.Text = "Doc Date";
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.Transparent;
		this.label4.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label4.Location = new System.Drawing.Point(12, 93);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(63, 15);
		this.label4.TabIndex = 8;
		this.label4.Text = "Doc Date: ";
		this.DocNo.AutoSize = true;
		this.DocNo.Cursor = System.Windows.Forms.Cursors.Hand;
		this.DocNo.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.DocNo.ForeColor = System.Drawing.Color.Blue;
		this.DocNo.Location = new System.Drawing.Point(138, 74);
		this.DocNo.Name = "DocNo";
		this.DocNo.Size = new System.Drawing.Size(14, 15);
		this.DocNo.TabIndex = 7;
		this.DocNo.Text = "1";
		this.DocNo.Click += new System.EventHandler(DocNo_Click);
		this.lbl.AutoSize = true;
		this.lbl.BackColor = System.Drawing.Color.Transparent;
		this.lbl.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbl.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbl.Location = new System.Drawing.Point(12, 74);
		this.lbl.Name = "lbl";
		this.lbl.Size = new System.Drawing.Size(80, 15);
		this.lbl.TabIndex = 6;
		this.lbl.Text = "Document #: ";
		this.label2.BackColor = System.Drawing.Color.Teal;
		this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label2.Dock = System.Windows.Forms.DockStyle.Top;
		this.label2.ForeColor = System.Drawing.Color.Black;
		this.label2.Location = new System.Drawing.Point(0, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(1370, 2);
		this.label2.TabIndex = 2;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1370, 749);
		base.Controls.Add(this.panel1);
		base.Name = "PurchaseInvoiceReverseActivity";
		this.Text = "Purchase Invoice Reverse Activity";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(PurchaseInvoiceReverseActivity_Load);
		this.panel1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.OrderInfoPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdOrderInfo).EndInit();
		this.WagesInfoPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdWagesInfo).EndInit();
		this.LabInfoPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdLabInfo).EndInit();
		this.WeighBrdgeDetailPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdWeighBridge).EndInit();
		this.GatePassesDetailPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdGatePass).EndInit();
		this.GrnDetailPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGrn).EndInit();
		this.InvoiceInfoPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdInvoiceInfo).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel4.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
