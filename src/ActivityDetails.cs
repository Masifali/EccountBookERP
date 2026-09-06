using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.ModernHistories;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.ModernHistoryModels;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.InventoryReports;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
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

public class ActivityDetails : Form
{
	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtOrderDetail = new DataTable();

	private DataTable dtGatePassessDetail = new DataTable();

	private DataTable dtweighBridgeDetail = new DataTable();

	private DataTable dtGRNDetail = new DataTable();

	private DataTable dtInvoiceInfo = new DataTable();

	private DataTable dtLabInfo = new DataTable();

	private DataTable dtWagesInfo = new DataTable();

	private DataTable dtEmptyBagsInfo = new DataTable();

	private int PurchaseOrderId;

	private bool ActionRemarksRequiredNotOnPurchaseOrderStatusUpdatation;

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

	private Label label7;

	private Label label6;

	private Label label4;

	private Label ExpiryDate;

	internal Label label8;

	private Label OrderDate;

	internal Label label5;

	private Label OrderNo;

	private Label ApprovalUserName;

	private Label EntryUserName;

	private Label lblApprovalUser;

	private Label label90;

	private Label label18;

	private Panel panel3;

	private Panel panel4;

	private Label label9;

	internal GridEX GrdInvoicesDetail;

	internal GridEX GrdGrnDetail;

	private Panel WeighBridgePanel;

	internal GridEX GrdWeighBridge;

	private Label label16;

	private Panel GatePassesDetailPanel;

	internal GridEX GrdGatePass;

	private Label label15;

	private Panel panelAllGridsCaption;

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

	private Panel WagesInfoPanel;

	private Panel LabInfoPanel;

	internal GridEX GrdLabInformation;

	private Label label20;

	private Label label22;

	internal GridEX GrdWagesInformation;

	private Panel GrnDetailPanel;

	private Panel InvoicesInfoPanel;

	private Label label21;

	private Label label17;

	private Panel panel7;

	private Panel PanelHeaderCaption;

	private Label label23;

	private Label label24;

	internal Label lblCommAgent;

	internal Label lblBroker;

	private Label label28;

	private Label label32;

	private Label label33;

	private Label label38;

	private Label lblModifyUser;

	private Label label47;

	private Label label48;

	private Label label51;

	private Label DeliveryTerm;

	private Label label49;

	private Label ModifyUserName;

	private Label PaymentTerm;

	private Label OrderItemQty;

	private Label EntryDate;

	private Label ApprovalStatus;

	private Label label34;

	private Label ReferenceParty;

	private Label ApprovedDate;

	private Label BrokerAgentName;

	private Label CommissionAgentName;

	private Label SupplierName;

	private Label DueDays;

	private Label DeliveryDays;

	private Label RecQty;

	private Label BalQty;

	private Label ModifyDate;

	private Button BtnCancelStatus;

	private Button BtnEditToRegister;

	private Button BtnCompleteStatus;

	private Label txtStatusChangedDate;

	private Label label40;

	private Label txtStatusChangedBy;

	private Label label26;

	private Button BtnEditToForm;

	private TextBox txtRemarks;

	private Label labelRemarks;

	private Label label27;

	internal GridEX grdOrderEmptyBag;

	internal UltraCombo CmbGatePassNo;

	private Label cmbsupplier;

	internal UltraCombo CmbOrderNo;

	private Label label42;

	private Button btnshow;

	private Button BtnRefresh;

	private Panel PanelAllGrids;

	public ActivityDetails(UserAccount pUser, int purchaseOrderId)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUser;
		PurchaseOrderId = purchaseOrderId;
	}

	private void ActivityDetails_Load(object sender, EventArgs e)
	{
		try
		{
			formright = CommonServices.SetRightsValueInRightsObject("PurchaseOrderHistory");
			ActionRemarksRequiredNotOnPurchaseOrderStatusUpdatation = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ActionRemarksRequiredNotOnPurchaseOrderStatusUpdatation"));
			GetDataAgainstOrder();
			tabControl1.TabPages.Remove(tabPage2);
			GpNoBind();
			OrderNoBind();
			if (PurchaseOrderId > 0)
			{
				CmbOrderNo.Value = PurchaseOrderId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OrderNoBind()
	{
		try
		{
			DataTable dt = PurchaseOrder.PurchaseOrder_AllPoNosForCombo(UserAccount.OrganizationId, UserAccount.CompanyId);
			InfragisticsHelper.BindAndRetainSelection(CmbOrderNo, dt, "Id", "DocNo", "OrderNo", AllColumns: true, Conversion.ToInt(CmbOrderNo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GpNoBind()
	{
		try
		{
			DataTable dt = GatePassInward.GatePassInward_AllGpNoAgainstPO(UserAccount.OrganizationId, UserAccount.CompanyId);
			InfragisticsHelper.BindAndRetainSelection(CmbGatePassNo, dt, "Id", "GpSrNo", "Gp SrNo", AllColumns: true, Conversion.ToInt(CmbGatePassNo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt != null && dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbGatePassNo).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				CmbGatePassNo_Leave(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbGatePassNo_Leave(object sender, EventArgs e)
	{
		try
		{
			int gpId = Conversion.ToInt(CmbGatePassNo.Value);
			if (((UltraGridBase)CmbGatePassNo).ActiveRow != null && gpId > 0)
			{
				int Id = Conversion.ToInt(((UltraDropDownBase)CmbGatePassNo).SelectedRow.Cells["PurchaseOrderId"].Value);
				CmbOrderNo.Value = Id;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			PurchaseOrderId = Conversion.ToInt(CmbOrderNo.Value);
			GetDataAgainstOrder();
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
			dtOrderDetail.Clear();
			dtGatePassessDetail.Clear();
			dtweighBridgeDetail.Clear();
			dtGRNDetail.Clear();
			dtInvoiceInfo.Clear();
			dtLabInfo.Clear();
			dtWagesInfo.Clear();
			dtEmptyBagsInfo.Clear();
			GrdOrderDetail.DataSource = null;
			GrdGatePass.DataSource = null;
			GrdLabInformation.DataSource = null;
			GrdWagesInformation.DataSource = null;
			grdOrderEmptyBag.DataSource = null;
			GrdWeighBridge.DataSource = null;
			GrdGrnDetail.DataSource = null;
			GrdInvoicesDetail.DataSource = null;
			int organizationId = UserAccount.OrganizationId;
			int CompanyId = UserAccount.CompanyId;
			int FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			PurchaseOrderActivityDetail Order = ModernHistory.PurchaseOrder_Activity(organizationId, CompanyId, FinancialYearId, PurchaseOrderId);
			if (Order != null && Order.PurchaseOrderInfoList?.Count > 0)
			{
				OrderNo.Text = Order.PurchaseOrderInfoList[0].OrderNo.ToString();
				OrderNo1.Text = Order.PurchaseOrderInfoList[0].OrderNo.ToString();
				OrderDate.Text = Order.PurchaseOrderInfoList[0].OrderDate.ToShortDateString();
				OrderDate1.Text = Order.PurchaseOrderInfoList[0].OrderDate.ToShortDateString();
				ExpiryDate.Text = Order.PurchaseOrderInfoList[0].OrderExpiryDate.ToShortDateString();
				ExpiryDate1.Text = Order.PurchaseOrderInfoList[0].OrderExpiryDate.ToShortDateString();
				EntryDate.Text = Order.PurchaseOrderInfoList[0].EntryDate.ToShortDateString();
				DeliveryTerm.Text = Order.PurchaseOrderInfoList[0].DeliveryTerm.ToString();
				DeliveryTerm1.Text = Order.PurchaseOrderInfoList[0].DeliveryTerm.ToString();
				PaymentTerm.Text = Order.PurchaseOrderInfoList[0].PaymentTerm.ToString();
				SupplierName.Text = Order.PurchaseOrderInfoList[0].SupplierName.ToString();
				ReferenceParty.Text = Conversion.ToString((object)Order.PurchaseOrderInfoList[0].ReferenceParty);
				CommissionAgentName.Text = Conversion.ToString((object)Order.PurchaseOrderInfoList[0].CommissionAgentName);
				BrokerAgentName.Text = Conversion.ToString((object)Order.PurchaseOrderInfoList[0].BrokerAgentName);
				DueDays.Text = Conversion.ToString((object)Order.PurchaseOrderInfoList[0].OrderDueDays);
				DeliveryDays.Text = Conversion.ToString((object)Order.PurchaseOrderInfoList[0].DeliveryDays);
				ApprovedDate.Text = Order.PurchaseOrderInfoList[0].PostDate.ToShortDateString();
				ExpiryStatus.Text = Order.PurchaseOrderInfoList[0].ExpiryStatus.ToString();
				ExpiryStatus1.Text = Order.PurchaseOrderInfoList[0].ExpiryStatus.ToString();
				if (Order.PurchaseOrderInfoList[0].ExpiryStatus.ToString() != "Expired")
				{
					ExpiryStatus.BackColor = Color.Teal;
					ExpiryStatus1.BackColor = Color.Teal;
				}
				else
				{
					ExpiryStatus.BackColor = Color.Red;
					ExpiryStatus1.BackColor = Color.Red;
				}
				OrderStatus.Text = Order.PurchaseOrderInfoList[0].OrderStatus.ToString();
				OrderStatus1.Text = Order.PurchaseOrderInfoList[0].OrderStatus.ToString();
				OrderItemQty.Text = Order.PurchaseOrderInfoList[0].OrderItemQty.ToString();
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
				BtnCancelStatus.Visible = OrderStatus.Text == "Open";
				BtnCompleteStatus.Visible = OrderStatus.Text == "Open";
				label27.Visible = OrderStatus.Text == "Open";
				labelRemarks.Visible = OrderStatus.Text == "Open";
				txtRemarks.Visible = OrderStatus.Text == "Open";
				OrderWeight.Text = Order.OrderTotalWeight.ToString("#,#;(#,#);0");
				OrderWeight1.Text = Order.OrderTotalWeight.ToString("#,#;(#,#);0");
				RecievedWeight.Text = Order.OrderReceivedWeight.ToString("#,#;(#,#);0");
				BalanceWeight.Text = Order.OrderBalanceWeight.ToString("#,#;(#,#);0");
				BalanceWeight1.Text = Order.OrderBalanceWeight.ToString("#,#;(#,#);0");
				OrderItemQty.Text = Order.OrderQty.ToString("#,#;(#,#);0");
				RecQty.Text = Order.OrderReceivedQty.ToString("#,#;(#,#);0");
				BalQty.Text = Order.OrderBalanceQty.ToString("#,#;(#,#);0");
				EntryUserName.Text = Order.PurchaseOrderInfoList[0].EntryUserName.ToString();
				EntryDate.Text = Order.PurchaseOrderInfoList[0].EntryDate.ToString();
				EntryUser1.Text = Order.PurchaseOrderInfoList[0].EntryUserName.ToString();
				ModifyUserName.Text = Conversion.ToString((object)Order.PurchaseOrderInfoList[0].ModifyUserName);
				ModifyDate.Text = (Conversion.CheckDateTimeNull(Order.PurchaseOrderInfoList[0].ModifyDate) ? "" : Order.PurchaseOrderInfoList[0].ModifyDate.ToString());
				txtStatusChangedBy.Text = Conversion.ToString((object)Order.PurchaseOrderInfoList[0].StatusChangedByName);
				txtStatusChangedDate.Text = (Conversion.CheckDateTimeNull(Order.PurchaseOrderInfoList[0].StatusChangedDate) ? "" : Order.PurchaseOrderInfoList[0].StatusChangedDate.ToString());
				ModifyUser1.Text = Conversion.ToString((object)Order.PurchaseOrderInfoList[0].ModifyUserName);
				ApprovalUserName.Text = Conversion.ToString((object)Order.PurchaseOrderInfoList[0].ApprovalUserName);
				ApprovedDate.Text = (Conversion.CheckDateTimeNull(Order.PurchaseOrderInfoList[0].PostDate) ? "" : Order.PurchaseOrderInfoList[0].PostDate.ToString());
				ApprovalStatus.Text = Conversion.ToString((object)Order.PurchaseOrderInfoList[0].ApprovalStatus);
				lblApprovalUser1.Visible = true;
				ApprovalUser1.Visible = true;
				ApprovalUser1.Text = Conversion.ToString((object)Order.PurchaseOrderInfoList[0].ApprovalUserName);
				if (dtOrderDetail.Columns.Count == 0)
				{
					dtOrderDetail.Columns.Add("PurchaeOrderId");
					dtOrderDetail.Columns.Add("ItemId");
					dtOrderDetail.Columns.Add("StockAcId");
					dtOrderDetail.Columns.Add("ItemName");
					dtOrderDetail.Columns.Add("Crop");
					dtOrderDetail.Columns.Add("Qty", typeof(double));
					dtOrderDetail.Columns.Add("PackUom");
					dtOrderDetail.Columns.Add("NetWeight", typeof(double));
					dtOrderDetail.Columns.Add("OrderItemRate", typeof(double));
					dtOrderDetail.Columns.Add("RateUom");
					dtOrderDetail.Columns.Add("Amount", typeof(double));
					dtOrderDetail.Columns.Add("LabSampleNo");
					dtOrderDetail.Columns.Add("Moisture");
					dtOrderDetail.Columns.Add("DetailRemarks");
				}
				for (int i2 = 0; i2 < Order.PurchaseOrderInfoList.Count; i2++)
				{
					dtOrderDetail.Rows.Add(Order.PurchaseOrderInfoList[i2].PurchaeOrderId, Order.PurchaseOrderInfoList[i2].OrderItemId, Order.PurchaseOrderInfoList[i2].StockAcId, Order.PurchaseOrderInfoList[i2].ItemName, Order.PurchaseOrderInfoList[i2].Crop, Order.PurchaseOrderInfoList[i2].OrderItemQty, Order.PurchaseOrderInfoList[i2].PackUom, Order.PurchaseOrderInfoList[i2].NetWeight, Order.PurchaseOrderInfoList[i2].OrderItemRate, Order.PurchaseOrderInfoList[i2].RateUom, Order.PurchaseOrderInfoList[i2].Amount, Order.PurchaseOrderInfoList[i2].LabSampleNo, Order.PurchaseOrderInfoList[i2].Moisture, Order.PurchaseOrderInfoList[i2].DetailRemarks);
				}
				GrdOrderDetail.DataSource = dtOrderDetail;
				GridSettingForOrderDetail();
				if (Order.OrderEmptyBagsInfoList != null && Order.OrderEmptyBagsInfoList.Count > 0)
				{
					if (dtEmptyBagsInfo.Columns.Count == 0)
					{
						dtEmptyBagsInfo.Columns.Add("Id");
						dtEmptyBagsInfo.Columns.Add("PurchaseOrderId");
						dtEmptyBagsInfo.Columns.Add("EmptyBagsType");
						dtEmptyBagsInfo.Columns.Add("ItemId");
						dtEmptyBagsInfo.Columns.Add("ItemName");
						dtEmptyBagsInfo.Columns.Add("PackingType");
						dtEmptyBagsInfo.Columns.Add("WeightCut", typeof(double));
						dtEmptyBagsInfo.Columns.Add("Rate", typeof(double));
					}
					for (int n = 0; n < Order.OrderEmptyBagsInfoList.Count; n++)
					{
						dtEmptyBagsInfo.Rows.Add(Order.OrderEmptyBagsInfoList[n].Id, Order.OrderEmptyBagsInfoList[n].PurchaseOrderId, Order.OrderEmptyBagsInfoList[n].EmptyBagsType, Order.OrderEmptyBagsInfoList[n].ItemId, Order.OrderEmptyBagsInfoList[n].ItemName, Order.OrderEmptyBagsInfoList[n].PackTypeDesc, Order.OrderEmptyBagsInfoList[n].WeightCut, Order.OrderEmptyBagsInfoList[n].Rate);
					}
					((Control)(object)grdOrderEmptyBag).Visible = true;
					grdOrderEmptyBag.DataSource = dtEmptyBagsInfo;
					grdOrderEmptyBag.RetrieveStructure();
					GridSettingsForEmptyBags();
				}
				else
				{
					((Control)(object)grdOrderEmptyBag).Visible = false;
					grdOrderEmptyBag.DataSource = null;
				}
				if (Order.GatePassInfoList != null && Order.GatePassInfoList.Count > 0)
				{
					GatePassesDetailPanel.Show();
					if (dtGatePassessDetail.Columns.Count == 0)
					{
						dtGatePassessDetail.Columns.Add("GatePassId");
						dtGatePassessDetail.Columns.Add("GpSrNo", typeof(int));
						dtGatePassessDetail.Columns.Add("GpDate", typeof(DateTime));
						dtGatePassessDetail.Columns.Add("VehicleNo", typeof(string));
						dtGatePassessDetail.Columns.Add("BiltyNo", typeof(string));
						dtGatePassessDetail.Columns.Add("VehicleInDate", typeof(DateTime));
						dtGatePassessDetail.Columns.Add("VehicleOutDate", typeof(DateTime));
						dtGatePassessDetail.Columns.Add("SupplierWeight", typeof(double));
						dtGatePassessDetail.Columns.Add("FactoryWeight", typeof(double));
						dtGatePassessDetail.Columns.Add("DifferenceWeight", typeof(double));
						dtGatePassessDetail.Columns.Add("Short/Excess", typeof(string));
						dtGatePassessDetail.Columns.Add("EntryUserName");
						dtGatePassessDetail.Columns.Add("ModifyUserName");
						dtGatePassessDetail.Columns.Add("ApprovalUserName");
					}
					for (int m = 0; m < Order.GatePassInfoList.Count; m++)
					{
						dtGatePassessDetail.Rows.Add(Order.GatePassInfoList[m].GatePassId, Order.GatePassInfoList[m].GpSrNo, Order.GatePassInfoList[m].GpDate.ToString("dd-MMM-yy"), Order.GatePassInfoList[m].VehicleNo, Order.GatePassInfoList[m].BiltyNo, Order.GatePassInfoList[m].VehicleInDate, Order.GatePassInfoList[m].VehicleOutDate, Order.GatePassInfoList[m].SupplierWeight, Order.GatePassInfoList[m].FactoryWeight, Order.GatePassInfoList[m].DifferenceWeight, Order.GatePassInfoList[m].ShortExcess, Order.GatePassInfoList[m].EntryUserName, Order.GatePassInfoList[m].ModifyUserName, Order.GatePassInfoList[m].ApprovalUserName);
					}
					GrdGatePass.DataSource = dtGatePassessDetail;
					GridSettingsForGatePass();
				}
				else
				{
					GatePassesDetailPanel.Hide();
					GrdGatePass.DataSource = null;
				}
				if (Order.WeighBridgeInfoList != null && Order.WeighBridgeInfoList.Count > 0)
				{
					WeighBridgePanel.Show();
					if (dtweighBridgeDetail.Columns.Count == 0)
					{
						dtweighBridgeDetail.Columns.Add("GatePassId");
						dtweighBridgeDetail.Columns.Add("WbId");
						dtweighBridgeDetail.Columns.Add("GpSrNo", typeof(int));
						dtweighBridgeDetail.Columns.Add("TicketNo", typeof(int));
						dtweighBridgeDetail.Columns.Add("TicketDate", typeof(DateTime));
						dtweighBridgeDetail.Columns.Add("FirstWeight", typeof(double));
						dtweighBridgeDetail.Columns.Add("SecondWeight", typeof(double));
						dtweighBridgeDetail.Columns.Add("NetWbWeight", typeof(double));
						dtweighBridgeDetail.Columns.Add("WbRemarks");
						dtweighBridgeDetail.Columns.Add("EntryUserName");
						dtweighBridgeDetail.Columns.Add("ModifyUserName");
					}
					for (int j2 = 0; j2 < Order.WeighBridgeInfoList.Count; j2++)
					{
						dtweighBridgeDetail.Rows.Add(Order.WeighBridgeInfoList[j2].GatePassId, Order.WeighBridgeInfoList[j2].WbId, Order.WeighBridgeInfoList[j2].GpSrNo, Order.WeighBridgeInfoList[j2].TicketNo, Order.WeighBridgeInfoList[j2].TicketDate.ToString("dd-MMM-yy"), Order.WeighBridgeInfoList[j2].FirstWeight, Order.WeighBridgeInfoList[j2].SecondWeight, Order.WeighBridgeInfoList[j2].NetWbWeight, Order.WeighBridgeInfoList[j2].WbRemarks, Order.WeighBridgeInfoList[j2].EntryUserName, Order.WeighBridgeInfoList[j2].ModifyUserName);
					}
					GrdWeighBridge.DataSource = dtweighBridgeDetail;
					GridSettingsForWeighBridge();
				}
				else
				{
					WeighBridgePanel.Hide();
					GrdWeighBridge.DataSource = null;
				}
				if (Order.GoodsReceiptNote != null && Order.GoodsReceiptNote.Count > 0)
				{
					GrnDetailPanel.Show();
					if (dtGRNDetail.Columns.Count == 0)
					{
						dtGRNDetail.Columns.Add("GrnId");
						dtGRNDetail.Columns.Add("GpSrNo", typeof(int));
						dtGRNDetail.Columns.Add("GrnNo", typeof(int));
						dtGRNDetail.Columns.Add("GrnDate", typeof(DateTime));
						dtGRNDetail.Columns.Add("PurchaseType");
						dtGRNDetail.Columns.Add("DeliveryTerm");
						dtGRNDetail.Columns.Add("SupplierWeight", typeof(double));
						dtGRNDetail.Columns.Add("FactoryWeight", typeof(double));
						dtGRNDetail.Columns.Add("DifferenceWeight", typeof(double));
						dtGRNDetail.Columns.Add("Short/Excess", typeof(string));
						dtGRNDetail.Columns.Add("Qty", typeof(double));
						dtGRNDetail.Columns.Add("GrossWeight", typeof(double));
						dtGRNDetail.Columns.Add("EbUnit", typeof(double));
						dtGRNDetail.Columns.Add("EbTotal", typeof(double));
						dtGRNDetail.Columns.Add("WeightCutTotal", typeof(double));
						dtGRNDetail.Columns.Add("OtherLess", typeof(double));
						dtGRNDetail.Columns.Add("BillWeight", typeof(double));
						dtGRNDetail.Columns.Add("StockEbUnit", typeof(double));
						dtGRNDetail.Columns.Add("StockEbTotal", typeof(double));
						dtGRNDetail.Columns.Add("StockWeight", typeof(double));
						dtGRNDetail.Columns.Add("AreaCity");
						dtGRNDetail.Columns.Add("FreightAmount", typeof(double));
						dtGRNDetail.Columns.Add("EntryUser");
						dtGRNDetail.Columns.Add("ModifyUser");
					}
					for (int l = 0; l < Order.GoodsReceiptNote.Count; l++)
					{
						double DiffWeight = Order.GoodsReceiptNote[l].PartyWeight - Order.GoodsReceiptNote[l].FactoryWeight;
						dtGRNDetail.Rows.Add(Order.GoodsReceiptNote[l].GrnId, Order.GoodsReceiptNote[l].GpSrNo, Order.GoodsReceiptNote[l].DocNo, Order.GoodsReceiptNote[l].DocDate.ToString("dd-MMM-yy"), Order.GoodsReceiptNote[l].PurchaseType, Order.GoodsReceiptNote[l].DeliveryTerm, Order.GoodsReceiptNote[l].PartyWeight, Order.GoodsReceiptNote[l].FactoryWeight, DiffWeight, (DiffWeight > 0.0) ? "Short" : ((DiffWeight < 0.0) ? "Excess" : "Nill"), Order.GoodsReceiptNote[l].TotalQuantity, Order.GoodsReceiptNote[l].TotalGrossWeight, Order.GoodsReceiptNote[l].AvgEbUnit, Order.GoodsReceiptNote[l].TotalEbTotal, Order.GoodsReceiptNote[l].TotalWtCutTotal, Order.GoodsReceiptNote[l].TotalAdLsWeight, Order.GoodsReceiptNote[l].TotalNetWeight, Order.GoodsReceiptNote[l].AvgStockEbUnit, Order.GoodsReceiptNote[l].TotalStockEbTotal, Order.GoodsReceiptNote[l].TotalStockWeight, Order.GoodsReceiptNote[l].AreaCity, Order.GoodsReceiptNote[l].FreightAmount, Order.GoodsReceiptNote[l].EntryUserName, Order.GoodsReceiptNote[l].ModifyUserName);
					}
					GrdGrnDetail.DataSource = dtGRNDetail;
					GrdGrnDetail.RetrieveStructure();
					GridSettingsForGRN();
				}
				else
				{
					GrnDetailPanel.Hide();
					GrdGrnDetail.DataSource = null;
				}
				if (Order.PurchaseInvoiceInfo != null && Order.PurchaseInvoiceInfo.Count > 0)
				{
					InvoicesInfoPanel.Show();
					if (dtInvoiceInfo.Columns.Count == 0)
					{
						dtInvoiceInfo.Columns.Add("GpNo", typeof(int));
						dtInvoiceInfo.Columns.Add("GrnId", typeof(int));
						dtInvoiceInfo.Columns.Add("GrnNo", typeof(string));
						dtInvoiceInfo.Columns.Add("InvoiceId");
						dtInvoiceInfo.Columns.Add("InvoiceNumber", typeof(int));
						dtInvoiceInfo.Columns.Add("InvoiceDate", typeof(DateTime));
						dtInvoiceInfo.Columns.Add("BillWeight", typeof(double));
						dtInvoiceInfo.Columns.Add("ItemAmount", typeof(double));
						dtInvoiceInfo.Columns.Add("SupplierExpenses", typeof(double));
						dtInvoiceInfo.Columns.Add("SupplierAddLess", typeof(double));
						dtInvoiceInfo.Columns.Add("SupplierBillAmount", typeof(double));
						dtInvoiceInfo.Columns.Add("CommissionAmount", typeof(double));
						dtInvoiceInfo.Columns.Add("FreightAmount", typeof(double));
						dtInvoiceInfo.Columns.Add("DueDays", typeof(int));
						dtInvoiceInfo.Columns.Add("DueDate", typeof(DateTime));
						dtInvoiceInfo.Columns.Add("Wages", typeof(double));
						dtInvoiceInfo.Columns.Add("ChargetoProduct", typeof(double));
						dtInvoiceInfo.Columns.Add("AvgRate", typeof(double));
						dtInvoiceInfo.Columns.Add("AvgRateCut", typeof(double));
						dtInvoiceInfo.Columns.Add("NetItemAmount", typeof(double));
						dtInvoiceInfo.Columns.Add("NetAvgRate", typeof(double));
						dtInvoiceInfo.Columns.Add("EntryDate");
						dtInvoiceInfo.Columns.Add("EntryUser");
						dtInvoiceInfo.Columns.Add("ApprovedStatus");
						dtInvoiceInfo.Columns.Add("ApprovedBy");
						dtInvoiceInfo.Columns.Add("ApprovedDate");
					}
					for (int k = 0; k < Order.PurchaseInvoiceInfo.Count; k++)
					{
						string PostDate = ((!Conversion.CheckDateTimeNull(Order.PurchaseInvoiceInfo[k].PostDate)) ? Order.PurchaseInvoiceInfo[k].PostDate.ToString() : "");
						dtInvoiceInfo.Rows.Add(Order.PurchaseInvoiceInfo[k].GpSrNo, Order.PurchaseInvoiceInfo[k].GrnId, Order.PurchaseInvoiceInfo[k].GrnNo, Order.PurchaseInvoiceInfo[k].PurchaseInvoiceId, Order.PurchaseInvoiceInfo[k].DocNo, Order.PurchaseInvoiceInfo[k].DocDate.ToString("dd-MMM-yy"), Order.PurchaseInvoiceInfo[k].BillWeight, Order.PurchaseInvoiceInfo[k].ItemAmount, Order.PurchaseInvoiceInfo[k].OtherExpenses, Order.PurchaseInvoiceInfo[k].PartyAddLess, Order.PurchaseInvoiceInfo[k].InvoiceAmount, Order.PurchaseInvoiceInfo[k].CommissionAmount, Order.PurchaseInvoiceInfo[k].FreightAmount, Order.PurchaseInvoiceInfo[k].DueDays, Order.PurchaseInvoiceInfo[k].DueDate, Order.PurchaseInvoiceInfo[k].WagesAmount, Order.PurchaseInvoiceInfo[k].ChargeToProduct, Order.PurchaseInvoiceInfo[k].AvgRate, Order.PurchaseInvoiceInfo[k].AvgRateCut, Order.PurchaseInvoiceInfo[k].BillAmount, Order.PurchaseInvoiceInfo[k].NetAvgRate, Order.PurchaseInvoiceInfo[k].EntryDate, Order.PurchaseInvoiceInfo[k].EntryUserName, Order.PurchaseInvoiceInfo[k].ApprovedStatus, Order.PurchaseInvoiceInfo[k].ApprovalUserName, PostDate);
					}
					GrdInvoicesDetail.DataSource = dtInvoiceInfo;
					GrdInvoicesDetail.RetrieveStructure();
					GridSettingsForInvoices();
				}
				else
				{
					InvoicesInfoPanel.Hide();
					GrdInvoicesDetail.DataSource = null;
				}
				if (Order.LabInfoList != null && Order.LabInfoList.Count > 0)
				{
					LabInfoPanel.Show();
					GrdLabInformation.DataSource = null;
					if (dtLabInfo.Columns.Count == 0)
					{
						dtLabInfo.Columns.Add("Id");
						dtLabInfo.Columns.Add("StockAcId");
						dtLabInfo.Columns.Add("GatePassId");
						dtLabInfo.Columns.Add("GpNo", typeof(int));
						dtLabInfo.Columns.Add("DocNo", typeof(int));
						dtLabInfo.Columns.Add("DocDate", typeof(DateTime));
						dtLabInfo.Columns.Add("ItemId");
						dtLabInfo.Columns.Add("ItemName");
						dtLabInfo.Columns.Add("Crop");
						dtLabInfo.Columns.Add("Moisture", typeof(double));
						dtLabInfo.Columns.Add("QtyForCut/Touch", typeof(double));
						dtLabInfo.Columns.Add("WeightCut/Touch", typeof(double));
						dtLabInfo.Columns.Add("RateCut/Touch", typeof(double));
						dtLabInfo.Columns.Add("ItemRate", typeof(double));
						dtLabInfo.Columns.Add("LabStatus");
						dtLabInfo.Columns.Add("Remarks");
						dtLabInfo.Columns.Add("AnalystName");
						dtLabInfo.Columns.Add("ApprovedBy");
						dtLabInfo.Columns.Add("EntryDate", typeof(DateTime));
						dtLabInfo.Columns.Add("EntryUser");
					}
					for (int j = 0; j < Order.LabInfoList.Count; j++)
					{
						dtLabInfo.Rows.Add(Order.LabInfoList[j].Id, Order.LabInfoList[j].StockAcId, Order.LabInfoList[j].GatePassId, Order.LabInfoList[j].GatePassNumber, Order.LabInfoList[j].DocNo, Order.LabInfoList[j].DocDate.ToShortDateString(), Order.LabInfoList[j].ItemId, Order.LabInfoList[j].ItemName, Order.LabInfoList[j].Crop, Order.LabInfoList[j].Moisture, Order.LabInfoList[j].QtyForWtCut, Order.LabInfoList[j].DeductionWeight, Order.LabInfoList[j].DeductionRate, Order.LabInfoList[j].ItemRate, Order.LabInfoList[j].LabStatus, Order.LabInfoList[j].RemarksHeader, Order.LabInfoList[j].AnalystName, Order.LabInfoList[j].ApprovalUserName, Order.LabInfoList[j].EntryDate, Order.LabInfoList[j].EntryUserName);
					}
					GrdLabInformation.DataSource = dtLabInfo;
					GrdLabInformation.RetrieveStructure();
					GridSettingsForLabInformation();
				}
				else
				{
					LabInfoPanel.Hide();
				}
				if (Order.WagesInfoList != null && Order.WagesInfoList.Count > 0)
				{
					WagesInfoPanel.Show();
					GrdWagesInformation.DataSource = null;
					for (int i = 0; i < Order.WagesInfoList.Count; i++)
					{
						if (dtWagesInfo.Columns.Count == 0)
						{
							dtWagesInfo.Columns.Add("Id");
							dtWagesInfo.Columns.Add("GrnId");
							dtWagesInfo.Columns.Add("GrnNumber", typeof(int));
							dtWagesInfo.Columns.Add("DocNo", typeof(int));
							dtWagesInfo.Columns.Add("DocDate", typeof(DateTime));
							dtWagesInfo.Columns.Add("ContractorId");
							dtWagesInfo.Columns.Add("ContractorName");
							dtWagesInfo.Columns.Add("WagesAccountName");
							dtWagesInfo.Columns.Add("Qty", typeof(double));
							dtWagesInfo.Columns.Add("PackSize");
							dtWagesInfo.Columns.Add("Rate", typeof(double));
							dtWagesInfo.Columns.Add("Weight", typeof(double));
							dtWagesInfo.Columns.Add("Amount", typeof(double));
							dtWagesInfo.Columns.Add("EntryUserName");
							dtWagesInfo.Columns.Add("ModifyUserName");
						}
						dtWagesInfo.Rows.Add(Order.WagesInfoList[i].Id, Order.WagesInfoList[i].GrnId, Order.WagesInfoList[i].GrnNumber, Order.WagesInfoList[i].DocNo, Order.WagesInfoList[i].DocDate.ToShortDateString(), Order.WagesInfoList[i].ContractorId, Order.WagesInfoList[i].ContractorName, Order.WagesInfoList[i].WagesAccountName, Order.WagesInfoList[i].Quantity, Order.WagesInfoList[i].PackSize, Order.WagesInfoList[i].Rate, Order.WagesInfoList[i].Weight, Order.WagesInfoList[i].Amount, Order.WagesInfoList[i].EntryUserName, Order.WagesInfoList[i].ModifyUserName);
					}
					GrdWagesInformation.DataSource = dtWagesInfo;
					GridSettingsForWagesInformation();
				}
				else
				{
					WagesInfoPanel.Hide();
				}
				return;
			}
			OrderNo.Text = "";
			OrderNo1.Text = "";
			OrderDate.Text = "";
			OrderDate1.Text = "";
			ExpiryDate.Text = "";
			ExpiryDate1.Text = "";
			EntryDate.Text = "";
			DeliveryTerm.Text = "";
			DeliveryTerm1.Text = "";
			PaymentTerm.Text = "";
			SupplierName.Text = "";
			ReferenceParty.Text = "";
			CommissionAgentName.Text = "";
			BrokerAgentName.Text = "";
			DueDays.Text = "0";
			DeliveryDays.Text = "0";
			ApprovedDate.Text = "";
			ExpiryStatus.Text = "";
			ExpiryStatus1.Text = "";
			ExpiryStatus.BackColor = Color.Black;
			ExpiryStatus1.BackColor = Color.Black;
			OrderStatus.Text = "";
			OrderStatus1.Text = "";
			OrderItemQty.Text = "0";
			OrderStatus.BackColor = Color.Black;
			OrderStatus1.BackColor = Color.Black;
			BtnCancelStatus.Visible = false;
			BtnCompleteStatus.Visible = false;
			label27.Visible = false;
			labelRemarks.Visible = false;
			txtRemarks.Visible = false;
			OrderWeight.Text = "0";
			OrderWeight1.Text = "0";
			RecievedWeight.Text = "0";
			BalanceWeight.Text = "0";
			BalanceWeight1.Text = "0";
			OrderItemQty.Text = "0";
			RecQty.Text = "0";
			BalQty.Text = "0";
			EntryUserName.Text = "";
			EntryDate.Text = "";
			EntryUser1.Text = "";
			ModifyUserName.Text = "";
			ModifyDate.Text = "";
			txtStatusChangedBy.Text = "";
			txtStatusChangedDate.Text = "";
			ModifyUser1.Text = "";
			ApprovalUserName.Text = "";
			ApprovedDate.Text = "";
			ApprovalStatus.Text = "";
			lblApprovalUser1.Visible = false;
			ApprovalUser1.Visible = false;
			ApprovalUser1.Text = "";
			throw new Exception("Record Not Found");
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
			GrdOrderDetail.ColumnAutoResize = false;
			GrdOrderDetail.RootTable.Columns["ItemName"].Width = 150;
			GrdOrderDetail.RootTable.Columns["Crop"].Width = 75;
			GrdOrderDetail.RootTable.Columns["Qty"].Width = 60;
			GrdOrderDetail.RootTable.Columns["PackUom"].Width = 50;
			GrdOrderDetail.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			GrdOrderDetail.RootTable.Columns["OrderItemRate"].Width = 80;
			GrdOrderDetail.RootTable.Columns["RateUom"].Width = 50;
			GrdOrderDetail.RootTable.Columns["Amount"].Width = 90;
			GrdOrderDetail.RootTable.Columns["LabSampleNo"].Width = 80;
			GrdOrderDetail.RootTable.Columns["Moisture"].Width = 80;
			GrdOrderDetail.RootTable.Columns["DetailRemarks"].Width = 120;
			GrdOrderDetail.RootTable.Columns["PurchaeOrderId"].Visible = false;
			GrdOrderDetail.RootTable.Columns["ItemId"].Visible = false;
			GrdOrderDetail.RootTable.Columns["StockAcId"].Visible = false;
			GrdOrderDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			GrdOrderDetail.RootTable.Columns["Moisture"].AggregateFunction = (AggregateFunction)2;
			GrdOrderDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			GrdOrderDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			GrdOrderDetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			GrdOrderDetail.RootTable.Columns["Moisture"].TextAlignment = (TextAlignment)3;
			GrdOrderDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			GrdOrderDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			GrdOrderDetail.RootTable.Columns["OrderItemRate"].TextAlignment = (TextAlignment)3;
			GrdOrderDetail.RootTable.Columns["Qty"].FormatString = "#,#";
			GrdOrderDetail.RootTable.Columns["NetWeight"].FormatString = "#,#";
			GrdOrderDetail.RootTable.Columns["Amount"].FormatString = "#,#";
			GrdOrderDetail.RootTable.Columns["Moisture"].FormatString = "#,##0.##";
			GrdOrderDetail.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			GrdOrderDetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			GrdOrderDetail.RootTable.Columns["Amount"].TotalFormatString = "#,##0.##";
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
			GrdGatePass.ColumnAutoResize = false;
			GrdGatePass.RootTable.Columns["GpDate"].FormatString = "dd-MMM-yy";
			GrdGatePass.RootTable.Columns["VehicleInDate"].FormatString = "HH:mm:ss dd-MMM-yy";
			GrdGatePass.RootTable.Columns["VehicleOutDate"].FormatString = "HH:mm:ss dd-MMM-yy";
			GrdGatePass.RootTable.Columns["GpSrNo"].Width = 50;
			GrdGatePass.RootTable.Columns["GpDate"].Width = 80;
			GrdGatePass.RootTable.Columns["VehicleNo"].Width = 80;
			GrdGatePass.RootTable.Columns["BiltyNo"].Width = 60;
			GrdGatePass.RootTable.Columns["VehicleInDate"].Width = 120;
			GrdGatePass.RootTable.Columns["VehicleOutDate"].Width = 120;
			GrdGatePass.RootTable.Columns["SupplierWeight"].Width = 70;
			GrdGatePass.RootTable.Columns["FactoryWeight"].Width = 90;
			GrdGatePass.RootTable.Columns["DifferenceWeight"].Width = 70;
			GrdGatePass.RootTable.Columns["Short/Excess"].Width = 90;
			GrdGatePass.RootTable.Columns["EntryUserName"].Width = 100;
			GrdGatePass.RootTable.Columns["ModifyUserName"].Width = 100;
			GrdGatePass.RootTable.Columns["ApprovalUserName"].Width = 100;
			GrdGatePass.RootTable.Columns["GatePassId"].Visible = false;
			GrdGatePass.RootTable.Columns["GpSrNo"].ColumnType = (ColumnType)5;
			GrdGatePass.RootTable.Columns["SupplierWeight"].AggregateFunction = (AggregateFunction)2;
			GrdGatePass.RootTable.Columns["FactoryWeight"].AggregateFunction = (AggregateFunction)2;
			GrdGatePass.RootTable.Columns["DifferenceWeight"].AggregateFunction = (AggregateFunction)2;
			GrdGatePass.RootTable.Columns["SupplierWeight"].TextAlignment = (TextAlignment)3;
			GrdGatePass.RootTable.Columns["FactoryWeight"].TextAlignment = (TextAlignment)3;
			GrdGatePass.RootTable.Columns["DifferenceWeight"].TextAlignment = (TextAlignment)3;
			GrdGatePass.RootTable.Columns["SupplierWeight"].FormatString = "#,#";
			GrdGatePass.RootTable.Columns["FactoryWeight"].FormatString = "#,#";
			GrdGatePass.RootTable.Columns["DifferenceWeight"].FormatString = "#,##0.##";
			GrdGatePass.RootTable.Columns["SupplierWeight"].TotalFormatString = "#,##0.##";
			GrdGatePass.RootTable.Columns["FactoryWeight"].TotalFormatString = "#,##0.##";
			GrdGatePass.RootTable.Columns["DifferenceWeight"].TotalFormatString = "#,##0.##";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingsForLabInformation()
	{
		try
		{
			GrdLabInformation.ColumnAutoResize = false;
			GrdLabInformation.RootTable.Columns["Id"].Visible = false;
			GrdLabInformation.RootTable.Columns["GatePassId"].Visible = false;
			GrdLabInformation.RootTable.Columns["ItemId"].Visible = false;
			GrdLabInformation.RootTable.Columns["StockAcId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(GrdLabInformation, 3, 2);
			GrdLabInformation.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yy";
			GrdLabInformation.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			GrdLabInformation.RootTable.Columns["GpNo"].Width = 50;
			GrdLabInformation.RootTable.Columns["DocNo"].Width = 50;
			GrdLabInformation.RootTable.Columns["DocDate"].Width = 80;
			GrdLabInformation.RootTable.Columns["ItemName"].Width = 150;
			GrdLabInformation.RootTable.Columns["Crop"].Width = 70;
			GrdLabInformation.RootTable.Columns["Moisture"].Width = 70;
			GrdLabInformation.RootTable.Columns["QtyForCut/Touch"].Width = 90;
			GrdLabInformation.RootTable.Columns["WeightCut/Touch"].Width = 90;
			GrdLabInformation.RootTable.Columns["RateCut/Touch"].Width = 90;
			GrdLabInformation.RootTable.Columns["ItemRate"].Width = 70;
			GrdLabInformation.RootTable.Columns["Remarks"].Width = 90;
			GrdLabInformation.RootTable.Columns["AnalystName"].Width = 90;
			GrdLabInformation.RootTable.Columns["LabStatus"].Width = 70;
			GrdLabInformation.RootTable.Columns["ApprovedBy"].Width = 100;
			GrdLabInformation.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			GrdLabInformation.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 40;
			GrdLabInformation.RootTable.Columns["GpNo"].ColumnType = (ColumnType)5;
			GrdLabInformation.RootTable.Columns["ItemName"].ColumnType = (ColumnType)5;
			GrdLabInformation.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
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
			GrdWagesInformation.ColumnAutoResize = false;
			GrdWagesInformation.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yy";
			GrdWagesInformation.RootTable.Columns["GrnNumber"].Width = 50;
			GrdWagesInformation.RootTable.Columns["DocNo"].Width = 50;
			GrdWagesInformation.RootTable.Columns["DocDate"].Width = 80;
			GrdWagesInformation.RootTable.Columns["ContractorName"].Width = 150;
			GrdWagesInformation.RootTable.Columns["WagesAccountName"].Width = 150;
			GrdWagesInformation.RootTable.Columns["Qty"].Width = 70;
			GrdWagesInformation.RootTable.Columns["PackSize"].Width = 60;
			GrdWagesInformation.RootTable.Columns["Rate"].Width = 60;
			GrdWagesInformation.RootTable.Columns["Weight"].Width = 70;
			GrdWagesInformation.RootTable.Columns["Amount"].Width = 70;
			GrdWagesInformation.RootTable.Columns["EntryUserName"].Width = 90;
			GrdWagesInformation.RootTable.Columns["ModifyUserName"].Width = 90;
			GrdWagesInformation.RootTable.Columns["Id"].Visible = false;
			GrdWagesInformation.RootTable.Columns["GrnId"].Visible = false;
			GrdWagesInformation.RootTable.Columns["ContractorId"].Visible = false;
			GrdWagesInformation.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			GrdWagesInformation.RootTable.Columns["ContractorName"].ColumnType = (ColumnType)5;
			GrdWagesInformation.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			GrdWagesInformation.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			GrdWagesInformation.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			GrdWagesInformation.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			GrdWagesInformation.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			GrdWagesInformation.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			GrdWagesInformation.RootTable.Columns["Amount"].FormatString = "0,0";
			GrdWagesInformation.RootTable.Columns["Qty"].FormatString = "0,0";
			GrdWagesInformation.RootTable.Columns["Weight"].FormatString = "0,0";
			GrdWagesInformation.RootTable.Columns["Amount"].TotalFormatString = "#,##0.##";
			GrdWagesInformation.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			GrdWagesInformation.RootTable.Columns["Weight"].TotalFormatString = "#,##0.##";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingsForEmptyBags()
	{
		try
		{
			grdOrderEmptyBag.ColumnAutoResize = false;
			grdOrderEmptyBag.RootTable.Columns["Id"].Visible = false;
			grdOrderEmptyBag.RootTable.Columns["PurchaseOrderId"].Visible = false;
			grdOrderEmptyBag.RootTable.Columns["ItemId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdOrderEmptyBag, 2, 2);
			grdOrderEmptyBag.RootTable.Columns["EmptyBagsType"].Width = 90;
			grdOrderEmptyBag.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdOrderEmptyBag.RootTable.Columns["PackingType"].Width = Constants.InventoryConstants.PackingType;
			grdOrderEmptyBag.RootTable.Columns["WeightCut"].Width = 50;
			grdOrderEmptyBag.RootTable.Columns["Rate"].Width = 60;
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
			GrdWeighBridge.ColumnAutoResize = false;
			GrdWeighBridge.RootTable.Columns["TicketDate"].FormatString = "dd-MMM-yy";
			GrdWeighBridge.RootTable.Columns["GpSrNo"].Width = 50;
			GrdWeighBridge.RootTable.Columns["TicketNo"].Width = 80;
			GrdWeighBridge.RootTable.Columns["TicketDate"].Width = 80;
			GrdWeighBridge.RootTable.Columns["FirstWeight"].Width = Constants.InventoryConstants.WeightConstant;
			GrdWeighBridge.RootTable.Columns["SecondWeight"].Width = Constants.InventoryConstants.WeightConstant;
			GrdWeighBridge.RootTable.Columns["NetWbWeight"].Width = Constants.InventoryConstants.WeightConstant;
			GrdWeighBridge.RootTable.Columns["WbRemarks"].Width = 100;
			GrdWeighBridge.RootTable.Columns["EntryUserName"].Width = 125;
			GrdWeighBridge.RootTable.Columns["ModifyUserName"].Width = 125;
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
			GrdGrnDetail.ColumnAutoResize = false;
			GrdGrnDetail.RootTable.Columns["GrnId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(GrdGrnDetail, 2, 2);
			GrdGrnDetail.RootTable.Columns["GrnDate"].FormatString = "dd-MMM-yy";
			GrdGrnDetail.RootTable.Columns["StockEbUnit"].Caption = "Eb Unit";
			GrdGrnDetail.RootTable.Columns["StockEbTotal"].Caption = "Eb Total";
			GrdGrnDetail.RootTable.Columns["WeightCutTotal"].Caption = "Cut / Touch";
			GrdGrnDetail.RootTable.Columns["Short/Excess"].Caption = "Short / Excess";
			GrdGrnDetail.RootTable.Columns["GrnNo"].ColumnType = (ColumnType)5;
			GrdGrnDetail.RootTable.Columns["GpSrNo"].Width = 50;
			GrdGrnDetail.RootTable.Columns["GrnNo"].Width = 50;
			GrdGrnDetail.RootTable.Columns["GrnDate"].Width = 80;
			GrdGrnDetail.RootTable.Columns["PurchaseType"].Width = 110;
			GrdGrnDetail.RootTable.Columns["DeliveryTerm"].Width = 75;
			GrdGrnDetail.RootTable.Columns["SupplierWeight"].Width = Constants.InventoryConstants.WeightConstant;
			GrdGrnDetail.RootTable.Columns["FactoryWeight"].Width = Constants.InventoryConstants.WeightConstant;
			GrdGrnDetail.RootTable.Columns["DifferenceWeight"].Width = Constants.InventoryConstants.WeightConstant;
			GrdGrnDetail.RootTable.Columns["Short/Excess"].Width = 75;
			GrdGrnDetail.RootTable.Columns["Qty"].Width = 75;
			GrdGrnDetail.RootTable.Columns["GrossWeight"].Width = Constants.InventoryConstants.WeightConstant;
			GrdGrnDetail.RootTable.Columns["EbUnit"].Width = 55;
			GrdGrnDetail.RootTable.Columns["EbTotal"].Width = 60;
			GrdGrnDetail.RootTable.Columns["WeightCutTotal"].Width = 60;
			GrdGrnDetail.RootTable.Columns["OtherLess"].Width = 60;
			GrdGrnDetail.RootTable.Columns["BillWeight"].Width = Constants.InventoryConstants.WeightConstant;
			GrdGrnDetail.RootTable.Columns["StockEbUnit"].Width = 70;
			GrdGrnDetail.RootTable.Columns["StockEbTotal"].Width = 80;
			GrdGrnDetail.RootTable.Columns["StockWeight"].Width = 80;
			GrdGrnDetail.RootTable.Columns["AreaCity"].Width = 100;
			GrdGrnDetail.RootTable.Columns["FreightAmount"].Width = 75;
			GrdGrnDetail.RootTable.Columns["EntryUser"].Width = 100;
			GrdGrnDetail.RootTable.Columns["ModifyUser"].Width = 100;
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
			GrdInvoicesDetail.ColumnAutoResize = false;
			GrdInvoicesDetail.RootTable.Columns["GrnId"].Visible = false;
			GrdInvoicesDetail.RootTable.Columns["InvoiceId"].Visible = false;
			GrdInvoicesDetail.RootTable.Columns["BillWeight"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(GrdInvoicesDetail, 2, 2);
			GrdInvoicesDetail.RootTable.Columns["InvoiceDate"].FormatString = "dd-MMM-yy";
			GrdInvoicesDetail.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			GrdInvoicesDetail.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			GrdInvoicesDetail.RootTable.Columns["GpNo"].Width = 50;
			GrdInvoicesDetail.RootTable.Columns["GrnNo"].Width = 50;
			GrdInvoicesDetail.RootTable.Columns["BillWeight"].Width = 60;
			GrdInvoicesDetail.RootTable.Columns["InvoiceNumber"].Width = 60;
			GrdInvoicesDetail.RootTable.Columns["InvoiceDate"].Width = 80;
			GrdInvoicesDetail.RootTable.Columns["ItemAmount"].Width = 80;
			GrdInvoicesDetail.RootTable.Columns["CommissionAmount"].Width = 85;
			GrdInvoicesDetail.RootTable.Columns["SupplierExpenses"].Width = 80;
			GrdInvoicesDetail.RootTable.Columns["ChargetoProduct"].Width = 80;
			GrdInvoicesDetail.RootTable.Columns["NetItemAmount"].Width = 80;
			GrdInvoicesDetail.RootTable.Columns["SupplierBillAmount"].Width = 100;
			GrdInvoicesDetail.RootTable.Columns["DueDays"].Width = 70;
			GrdInvoicesDetail.RootTable.Columns["DueDate"].Width = 80;
			GrdInvoicesDetail.RootTable.Columns["Wages"].Width = 70;
			GrdInvoicesDetail.RootTable.Columns["AvgRate"].Width = 70;
			GrdInvoicesDetail.RootTable.Columns["AvgRateCut"].Width = 70;
			GrdInvoicesDetail.RootTable.Columns["NetAvgRate"].Width = 80;
			GrdInvoicesDetail.RootTable.Columns["EntryDate"].Width = 145;
			GrdInvoicesDetail.RootTable.Columns["EntryUser"].Width = 100;
			GrdInvoicesDetail.RootTable.Columns["ApprovedStatus"].Width = 80;
			GrdInvoicesDetail.RootTable.Columns["ApprovedBy"].Width = 100;
			GrdInvoicesDetail.RootTable.Columns["ApprovedDate"].Width = 145;
			GrdInvoicesDetail.RootTable.Columns["NetAvgRate"].Caption = "NetAvgRate 40KG";
			GrdInvoicesDetail.RootTable.Columns["GrnNo"].ColumnType = (ColumnType)5;
			GrdInvoicesDetail.RootTable.Columns["InvoiceNumber"].ColumnType = (ColumnType)5;
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
				CommonServices.PurchaseOrderSlipReport203(Conversion.ToInt(GrdOrderDetail.CurrentRow.Cells["PurchaeOrderId"].Value));
				return;
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

	private void GrdLabInformation_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow currentRow = GrdLabInformation.CurrentRow;
			Conversion.ToInt(currentRow.Cells["StockAcId"].Value);
			int Id = Conversion.ToInt(currentRow.Cells["Id"].Value);
			int ItemId = Conversion.ToInt(currentRow.Cells["ItemId"].Value);
			int GatePassId = Conversion.ToInt(currentRow.Cells["GatePassId"].Value);
			if (e.Column.Key == "ItemName")
			{
				CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(ItemId, clsGlobalVariables.ActiveYr.Start_Period, DateTime.Now);
			}
			if (e.Column.Key == "DocNo")
			{
				CommonServices.LabAnalysisReport653(Id);
			}
			if (e.Column.Key == "GpNo")
			{
				CommonServices.GatePassInwardSlipAndRegisterReport(GatePassId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdWagesInformation_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow currentRow = GrdWagesInformation.CurrentRow;
			int Id = Conversion.ToInt(currentRow.Cells["Id"].Value);
			int ContractorId = Conversion.ToInt(currentRow.Cells["ContractorId"].Value);
			if (e.Column.Key == "ContractorName")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(ContractorId, clsGlobalVariables.ActiveYr.Start_Period, DateTime.Now);
			}
			if (e.Column.Key == "DocNo")
			{
				CommonServices.ContractorWagesBill_SlipandRegister_002(Id);
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
			int GrnId = Conversion.ToInt(GrdGrnDetail.CurrentRow.Cells["GrnId"].Value);
			if (e.Column.Key == "GrnNo")
			{
				CommonServices.GrnSlipWithSubReports(GrnId);
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
			GridEXRow currentRow = GrdInvoicesDetail.CurrentRow;
			int InvoiceId = Conversion.ToInt(currentRow.Cells["InvoiceId"].Value);
			int GrnId = Conversion.ToInt(currentRow.Cells["GrnId"].Value);
			if (e.Column.Key == "InvoiceNumber")
			{
				new PurchaseInvoiceReverseActivity(UserAccount, InvoiceId).Show();
			}
			if (e.Column.Key == "GrnNo")
			{
				CommonServices.GrnSlipWithSubReports(GrnId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ActivityDetails_FormClosing(object sender, FormClosingEventArgs e)
	{
		try
		{
			HistoryStack.Remove(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			GpNoBind();
			OrderNoBind();
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

	private void GrdLabInformation_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = GrdLabInformation.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			Conversion.ToInt(item.Cells["StockAcId"].Value);
			int Id = Conversion.ToInt(item.Cells["Id"].Value);
			int ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
			int GatePassId = Conversion.ToInt(item.Cells["GatePassId"].Value);
			if (GrdLabInformation.CurrentColumn != null)
			{
				if (GrdLabInformation.CurrentColumn.Key == "ItemName")
				{
					CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(ItemId, clsGlobalVariables.ActiveYr.Start_Period, DateTime.Now);
				}
				else if (GrdLabInformation.CurrentColumn.Key == "DocNo")
				{
					CommonServices.LabAnalysisReport653(Id);
				}
				else if (GrdLabInformation.CurrentColumn.Key == "GpNo")
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

	private void GrdWagesInformation_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = GrdWagesInformation.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int Id = Conversion.ToInt(item.Cells["Id"].Value);
			int ContractorId = Conversion.ToInt(item.Cells["ContractorId"].Value);
			if (GrdWagesInformation.CurrentColumn != null)
			{
				if (GrdWagesInformation.CurrentColumn.Key == "DocNo")
				{
					CommonServices.ContractorWagesBill_SlipandRegister_002(Id);
				}
				else if (GrdWagesInformation.CurrentColumn.Key == "ContractorName")
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

	private void GrdGrnDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = GrdGrnDetail.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int GrnId = Conversion.ToInt(item.Cells["GrnId"].Value);
				if (GrdGrnDetail.CurrentColumn != null && GrdGrnDetail.CurrentColumn.Key == "GrnNo")
				{
					CommonServices.GrnSlipWithSubReports(GrnId);
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
					new PurchaseInvoiceReverseActivity(UserAccount, InvoiceId).Show();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCompleteStatus_Click(object sender, EventArgs e)
	{
		try
		{
			CompleteStatus(PurchaseOrderId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCancelStatus_Click(object sender, EventArgs e)
	{
		try
		{
			CancelStatus(PurchaseOrderId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CompleteStatus(int Id)
	{
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Expected O, but got Unknown
		try
		{
			if (Conversion.ToString((object)ApprovalStatus.Text) != "Approved")
			{
				MessageBox.Show("You cannot complete the status because the order is not approved yet.");
				return;
			}
			if (!formright.CanChangeOrderStatusToComplete)
			{
				MessageBox.Show("You don't have the right to change the order status to complete.");
				return;
			}
			string Remarks = Conversion.ToString((object)txtRemarks.Text);
			if (Remarks == string.Empty && !ActionRemarksRequiredNotOnPurchaseOrderStatusUpdatation)
			{
				txtRemarks.Focus();
				MessageBox.Show("Action Remarks Required");
			}
			else if (MessageBox.Show("Are you sure to Complete Status?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.Id = Id;
				item.ReqType = "Status";
				item.Comments = Remarks;
				item.EntryUser = UserAccount.ID;
				val.AllApprovalLists.Add(item);
				PurchaseOrder.UpdateStatusandIsApprovedbyOrderId(val);
				MessageBox.Show("Record Complete Successfully");
				GetDataAgainstOrder();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CancelStatus(int Id)
	{
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Expected O, but got Unknown
		try
		{
			if (!formright.CanChangeOrderStatusToCancel)
			{
				MessageBox.Show("You don't have the right to change the order status to cancel.");
				return;
			}
			if (Conversion.ToString((object)ApprovalStatus.Text) != "Approved")
			{
				MessageBox.Show("You cannot cancel the status because the order is not approved yet.");
				return;
			}
			string Remarks = Conversion.ToString((object)txtRemarks.Text);
			if (Remarks == string.Empty && !ActionRemarksRequiredNotOnPurchaseOrderStatusUpdatation)
			{
				txtRemarks.Focus();
				MessageBox.Show("Action Remarks Required");
			}
			else if (MessageBox.Show("Are you sure to Cancel Status?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.Id = Id;
				item.ReqType = "Cancel";
				item.Comments = Remarks;
				item.EntryUser = UserAccount.ID;
				val.AllApprovalLists.Add(item);
				PurchaseOrder.UpdateStatusandIsApprovedbyOrderId(val);
				MessageBox.Show("Record Cancel Successfully");
				GetDataAgainstOrder();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnEditToForm_Click(object sender, EventArgs e)
	{
		try
		{
			FormHelper.CanOpenForm("PurchsaeOrder", delegate
			{
				CommonServices.EditMethodFromLinked(41, PurchaseOrderId);
			}, UserAccount);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnEditToRegister_Click(object sender, EventArgs e)
	{
		try
		{
			FormHelper.CanOpenForm("PurchaseOrderHistory", delegate
			{
				PurchaseOrderHistory purchaseOrderHistory = new PurchaseOrderHistory(UserAccount)
				{
					txtSrFrom = 
					{
						Text = OrderNo.Text
					},
					txtSrTo = 
					{
						Text = OrderNo.Text
					}
				};
				if (OrderStatus.Text == "Open")
				{
					purchaseOrderHistory.RadOpenDetail.Checked = true;
				}
				else if (OrderStatus.Text == "Cancel")
				{
					purchaseOrderHistory.RadCancelDetail.Checked = true;
				}
				else if (OrderStatus.Text == "Complete")
				{
					purchaseOrderHistory.RadCompleteDetail.Checked = true;
				}
				if (ApprovalStatus.Text == "Approved")
				{
					purchaseOrderHistory.RadApprovedOnlyDetail.Checked = true;
				}
				else
				{
					purchaseOrderHistory.RadUnApprovedOnlyDetail.Checked = true;
				}
				purchaseOrderHistory.Show();
				purchaseOrderHistory.BringToFront();
			}, UserAccount);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ActivityDetails_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
				HistoryStack.Remove(this);
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 78;
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				BtnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)GrdOrderDetail).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				CmbGatePassNo.Focus();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				CmbGatePassNo.Focus();
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 80;
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For show data");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+F5", "For Focus on Date Type");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Date Type");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
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
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Expected O, but got Unknown
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Expected O, but got Unknown
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Expected O, but got Unknown
		//IL_057c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0586: Expected O, but got Unknown
		//IL_0c31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3b: Expected O, but got Unknown
		//IL_0eab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb5: Expected O, but got Unknown
		//IL_1128: Unknown result type (might be due to invalid IL or missing references)
		//IL_1132: Expected O, but got Unknown
		//IL_13a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b1: Expected O, but got Unknown
		//IL_1626: Unknown result type (might be due to invalid IL or missing references)
		//IL_1630: Expected O, but got Unknown
		//IL_18a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_18ac: Expected O, but got Unknown
		//IL_1db8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dc2: Expected O, but got Unknown
		GridEXLayout GrdWagesInformation_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Commentaries.ActivityDetails));
		GridEXLayout GrdWeighBridge_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdGatePass_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdLabInformation_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdGrnDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdInvoicesDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdOrderDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		GridEXLayout gridEX1_DesignTimeLayout = new GridEXLayout();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.PanelAllGrids = new System.Windows.Forms.Panel();
		this.WagesInfoPanel = new System.Windows.Forms.Panel();
		this.GrdWagesInformation = new GridEX();
		this.label22 = new System.Windows.Forms.Label();
		this.WeighBridgePanel = new System.Windows.Forms.Panel();
		this.GrdWeighBridge = new GridEX();
		this.label16 = new System.Windows.Forms.Label();
		this.GatePassesDetailPanel = new System.Windows.Forms.Panel();
		this.GrdGatePass = new GridEX();
		this.label15 = new System.Windows.Forms.Label();
		this.LabInfoPanel = new System.Windows.Forms.Panel();
		this.GrdLabInformation = new GridEX();
		this.label20 = new System.Windows.Forms.Label();
		this.GrnDetailPanel = new System.Windows.Forms.Panel();
		this.GrdGrnDetail = new GridEX();
		this.label17 = new System.Windows.Forms.Label();
		this.InvoicesInfoPanel = new System.Windows.Forms.Panel();
		this.GrdInvoicesDetail = new GridEX();
		this.label21 = new System.Windows.Forms.Label();
		this.panelAllGridsCaption = new System.Windows.Forms.Panel();
		this.label11 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdOrderEmptyBag = new GridEX();
		this.GrdOrderDetail = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.PanelHeaderCaption = new System.Windows.Forms.Panel();
		this.BtnRefresh = new System.Windows.Forms.Button();
		this.btnshow = new System.Windows.Forms.Button();
		this.CmbOrderNo = new UltraCombo();
		this.label42 = new System.Windows.Forms.Label();
		this.CmbGatePassNo = new UltraCombo();
		this.cmbsupplier = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label27 = new System.Windows.Forms.Label();
		this.labelRemarks = new System.Windows.Forms.Label();
		this.BtnCancelStatus = new System.Windows.Forms.Button();
		this.BtnEditToForm = new System.Windows.Forms.Button();
		this.BtnEditToRegister = new System.Windows.Forms.Button();
		this.BtnCompleteStatus = new System.Windows.Forms.Button();
		this.ModifyDate = new System.Windows.Forms.Label();
		this.RecQty = new System.Windows.Forms.Label();
		this.BalQty = new System.Windows.Forms.Label();
		this.DueDays = new System.Windows.Forms.Label();
		this.DeliveryDays = new System.Windows.Forms.Label();
		this.BrokerAgentName = new System.Windows.Forms.Label();
		this.CommissionAgentName = new System.Windows.Forms.Label();
		this.SupplierName = new System.Windows.Forms.Label();
		this.txtStatusChangedDate = new System.Windows.Forms.Label();
		this.ApprovedDate = new System.Windows.Forms.Label();
		this.ReferenceParty = new System.Windows.Forms.Label();
		this.ApprovalStatus = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.EntryDate = new System.Windows.Forms.Label();
		this.OrderItemQty = new System.Windows.Forms.Label();
		this.PaymentTerm = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.DeliveryTerm = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.lblModifyUser = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.lblBroker = new System.Windows.Forms.Label();
		this.lblCommAgent = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.txtStatusChangedBy = new System.Windows.Forms.Label();
		this.ApprovalUserName = new System.Windows.Forms.Label();
		this.ModifyUserName = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.EntryUserName = new System.Windows.Forms.Label();
		this.lblApprovalUser = new System.Windows.Forms.Label();
		this.label90 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.BalanceWeight = new System.Windows.Forms.Label();
		this.RecievedWeight = new System.Windows.Forms.Label();
		this.OrderWeight = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.ExpiryStatus = new System.Windows.Forms.Label();
		this.OrderStatus = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.ExpiryDate = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.OrderDate = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.OrderNo = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
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
		this.PanelAllGrids.SuspendLayout();
		this.WagesInfoPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdWagesInformation).BeginInit();
		this.WeighBridgePanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdWeighBridge).BeginInit();
		this.GatePassesDetailPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdGatePass).BeginInit();
		this.LabInfoPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdLabInformation).BeginInit();
		this.GrnDetailPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdGrnDetail).BeginInit();
		this.InvoicesInfoPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdInvoicesDetail).BeginInit();
		this.panelAllGridsCaption.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOrderEmptyBag).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.GrdOrderDetail).BeginInit();
		this.panel4.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel7.SuspendLayout();
		this.PanelHeaderCaption.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGatePassNo).BeginInit();
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
		this.tabPage1.Controls.Add(this.PanelAllGrids);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1362, 723);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Grid View";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.PanelAllGrids.AutoScroll = true;
		this.PanelAllGrids.Controls.Add(this.WagesInfoPanel);
		this.PanelAllGrids.Controls.Add(this.WeighBridgePanel);
		this.PanelAllGrids.Controls.Add(this.GatePassesDetailPanel);
		this.PanelAllGrids.Controls.Add(this.LabInfoPanel);
		this.PanelAllGrids.Controls.Add(this.GrnDetailPanel);
		this.PanelAllGrids.Controls.Add(this.InvoicesInfoPanel);
		this.PanelAllGrids.Controls.Add(this.panelAllGridsCaption);
		this.PanelAllGrids.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanelAllGrids.Location = new System.Drawing.Point(3, 354);
		this.PanelAllGrids.Name = "PanelAllGrids";
		this.PanelAllGrids.Size = new System.Drawing.Size(1356, 366);
		this.PanelAllGrids.TabIndex = 3;
		this.WagesInfoPanel.Controls.Add((System.Windows.Forms.Control)(object)this.GrdWagesInformation);
		this.WagesInfoPanel.Controls.Add(this.label22);
		this.WagesInfoPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.WagesInfoPanel.Location = new System.Drawing.Point(0, 865);
		this.WagesInfoPanel.Name = "WagesInfoPanel";
		this.WagesInfoPanel.Size = new System.Drawing.Size(1339, 150);
		this.WagesInfoPanel.TabIndex = 6;
		this.GrdWagesInformation.AllowEdit = (InheritableBoolean)2;
		this.GrdWagesInformation.AlternatingColors = true;
		((ScrollableBase)this.GrdWagesInformation).BackColor = System.Drawing.Color.White;
		this.GrdWagesInformation.ColumnAutoResize = true;
		GrdWagesInformation_DesignTimeLayout.LayoutString = resources.GetString("GrdWagesInformation_DesignTimeLayout.LayoutString");
		this.GrdWagesInformation.DesignTimeLayout = GrdWagesInformation_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdWagesInformation).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdWagesInformation.DynamicFiltering = true;
		this.GrdWagesInformation.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdWagesInformation).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdWagesInformation.GroupByBoxVisible = false;
		this.GrdWagesInformation.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.GrdWagesInformation).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.GrdWagesInformation).Name = "GrdWagesInformation";
		((ScrollableBase)this.GrdWagesInformation).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdWagesInformation).Size = new System.Drawing.Size(1339, 125);
		((System.Windows.Forms.Control)(object)this.GrdWagesInformation).TabIndex = 7;
		this.GrdWagesInformation.TotalRow = (InheritableBoolean)1;
		this.GrdWagesInformation.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdWagesInformation).VisualStyle = (VisualStyle)4;
		this.GrdWagesInformation.LinkClicked += new ColumnActionEventHandler(GrdWagesInformation_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdWagesInformation).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdWagesInformation_KeyDown);
		this.label22.BackColor = System.Drawing.Color.Teal;
		this.label22.Dock = System.Windows.Forms.DockStyle.Top;
		this.label22.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.White;
		this.label22.Location = new System.Drawing.Point(0, 0);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(1339, 25);
		this.label22.TabIndex = 4;
		this.label22.Text = "Wages Information";
		this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.WeighBridgePanel.Controls.Add((System.Windows.Forms.Control)(object)this.GrdWeighBridge);
		this.WeighBridgePanel.Controls.Add(this.label16);
		this.WeighBridgePanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.WeighBridgePanel.Location = new System.Drawing.Point(0, 715);
		this.WeighBridgePanel.Name = "WeighBridgePanel";
		this.WeighBridgePanel.Size = new System.Drawing.Size(1339, 150);
		this.WeighBridgePanel.TabIndex = 2;
		this.GrdWeighBridge.AllowEdit = (InheritableBoolean)2;
		this.GrdWeighBridge.AlternatingColors = true;
		((ScrollableBase)this.GrdWeighBridge).BackColor = System.Drawing.Color.White;
		this.GrdWeighBridge.ColumnAutoResize = true;
		GrdWeighBridge_DesignTimeLayout.LayoutString = resources.GetString("GrdWeighBridge_DesignTimeLayout.LayoutString");
		this.GrdWeighBridge.DesignTimeLayout = GrdWeighBridge_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdWeighBridge).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdWeighBridge.DynamicFiltering = true;
		this.GrdWeighBridge.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdWeighBridge).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdWeighBridge.GroupByBoxVisible = false;
		this.GrdWeighBridge.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.GrdWeighBridge).Location = new System.Drawing.Point(0, 24);
		((System.Windows.Forms.Control)(object)this.GrdWeighBridge).Name = "GrdWeighBridge";
		((ScrollableBase)this.GrdWeighBridge).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdWeighBridge).Size = new System.Drawing.Size(1339, 126);
		((System.Windows.Forms.Control)(object)this.GrdWeighBridge).TabIndex = 5;
		this.GrdWeighBridge.TotalRow = (InheritableBoolean)1;
		this.GrdWeighBridge.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdWeighBridge).VisualStyle = (VisualStyle)4;
		this.GrdWeighBridge.LinkClicked += new ColumnActionEventHandler(GrdWeighBridge_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdWeighBridge).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdWeighBridge_KeyDown);
		this.label16.BackColor = System.Drawing.Color.Teal;
		this.label16.Dock = System.Windows.Forms.DockStyle.Top;
		this.label16.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(0, 0);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(1339, 24);
		this.label16.TabIndex = 2;
		this.label16.Text = "Weigh Bridge Detail";
		this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.GatePassesDetailPanel.Controls.Add((System.Windows.Forms.Control)(object)this.GrdGatePass);
		this.GatePassesDetailPanel.Controls.Add(this.label15);
		this.GatePassesDetailPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.GatePassesDetailPanel.Location = new System.Drawing.Point(0, 545);
		this.GatePassesDetailPanel.Name = "GatePassesDetailPanel";
		this.GatePassesDetailPanel.Size = new System.Drawing.Size(1339, 170);
		this.GatePassesDetailPanel.TabIndex = 1;
		this.GrdGatePass.AllowEdit = (InheritableBoolean)2;
		this.GrdGatePass.AlternatingColors = true;
		((ScrollableBase)this.GrdGatePass).BackColor = System.Drawing.Color.White;
		this.GrdGatePass.ColumnAutoResize = true;
		GrdGatePass_DesignTimeLayout.LayoutString = resources.GetString("GrdGatePass_DesignTimeLayout.LayoutString");
		this.GrdGatePass.DesignTimeLayout = GrdGatePass_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdGatePass).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdGatePass.DynamicFiltering = true;
		this.GrdGatePass.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdGatePass).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdGatePass.GroupByBoxVisible = false;
		this.GrdGatePass.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.GrdGatePass).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.GrdGatePass).Name = "GrdGatePass";
		((ScrollableBase)this.GrdGatePass).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdGatePass).Size = new System.Drawing.Size(1339, 149);
		((System.Windows.Forms.Control)(object)this.GrdGatePass).TabIndex = 4;
		this.GrdGatePass.TotalRow = (InheritableBoolean)1;
		this.GrdGatePass.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdGatePass).VisualStyle = (VisualStyle)4;
		this.GrdGatePass.LinkClicked += new ColumnActionEventHandler(GrdGatePass_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdGatePass).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdGatePass_KeyDown);
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Dock = System.Windows.Forms.DockStyle.Top;
		this.label15.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(0, 0);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(1339, 21);
		this.label15.TabIndex = 1;
		this.label15.Text = "Gate Passes Detail";
		this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.LabInfoPanel.Controls.Add((System.Windows.Forms.Control)(object)this.GrdLabInformation);
		this.LabInfoPanel.Controls.Add(this.label20);
		this.LabInfoPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.LabInfoPanel.Location = new System.Drawing.Point(0, 375);
		this.LabInfoPanel.Name = "LabInfoPanel";
		this.LabInfoPanel.Size = new System.Drawing.Size(1339, 170);
		this.LabInfoPanel.TabIndex = 5;
		this.GrdLabInformation.AllowEdit = (InheritableBoolean)2;
		this.GrdLabInformation.AlternatingColors = true;
		((ScrollableBase)this.GrdLabInformation).BackColor = System.Drawing.Color.White;
		this.GrdLabInformation.ColumnAutoResize = true;
		GrdLabInformation_DesignTimeLayout.LayoutString = resources.GetString("GrdLabInformation_DesignTimeLayout.LayoutString");
		this.GrdLabInformation.DesignTimeLayout = GrdLabInformation_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdLabInformation).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdLabInformation.DynamicFiltering = true;
		this.GrdLabInformation.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdLabInformation).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdLabInformation.GroupByBoxVisible = false;
		this.GrdLabInformation.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.GrdLabInformation).Location = new System.Drawing.Point(0, 24);
		((System.Windows.Forms.Control)(object)this.GrdLabInformation).Name = "GrdLabInformation";
		((ScrollableBase)this.GrdLabInformation).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdLabInformation).Size = new System.Drawing.Size(1339, 146);
		((System.Windows.Forms.Control)(object)this.GrdLabInformation).TabIndex = 6;
		this.GrdLabInformation.TotalRow = (InheritableBoolean)1;
		this.GrdLabInformation.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdLabInformation).VisualStyle = (VisualStyle)4;
		this.GrdLabInformation.LinkClicked += new ColumnActionEventHandler(GrdLabInformation_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdLabInformation).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdLabInformation_KeyDown);
		this.label20.BackColor = System.Drawing.Color.Teal;
		this.label20.Dock = System.Windows.Forms.DockStyle.Top;
		this.label20.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.Color.White;
		this.label20.Location = new System.Drawing.Point(0, 0);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(1339, 24);
		this.label20.TabIndex = 3;
		this.label20.Text = "Lab Information";
		this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.GrnDetailPanel.Controls.Add((System.Windows.Forms.Control)(object)this.GrdGrnDetail);
		this.GrnDetailPanel.Controls.Add(this.label17);
		this.GrnDetailPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.GrnDetailPanel.Location = new System.Drawing.Point(0, 205);
		this.GrnDetailPanel.Name = "GrnDetailPanel";
		this.GrnDetailPanel.Size = new System.Drawing.Size(1339, 170);
		this.GrnDetailPanel.TabIndex = 7;
		this.GrdGrnDetail.AllowEdit = (InheritableBoolean)2;
		this.GrdGrnDetail.AlternatingColors = true;
		((ScrollableBase)this.GrdGrnDetail).BackColor = System.Drawing.Color.White;
		this.GrdGrnDetail.ColumnAutoResize = true;
		GrdGrnDetail_DesignTimeLayout.LayoutString = resources.GetString("GrdGrnDetail_DesignTimeLayout.LayoutString");
		this.GrdGrnDetail.DesignTimeLayout = GrdGrnDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdGrnDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdGrnDetail.DynamicFiltering = true;
		this.GrdGrnDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdGrnDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdGrnDetail.GroupByBoxVisible = false;
		this.GrdGrnDetail.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.GrdGrnDetail).Location = new System.Drawing.Point(0, 24);
		((System.Windows.Forms.Control)(object)this.GrdGrnDetail).Name = "GrdGrnDetail";
		((ScrollableBase)this.GrdGrnDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdGrnDetail).Size = new System.Drawing.Size(1339, 146);
		((System.Windows.Forms.Control)(object)this.GrdGrnDetail).TabIndex = 6;
		this.GrdGrnDetail.TotalRow = (InheritableBoolean)1;
		this.GrdGrnDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdGrnDetail).VisualStyle = (VisualStyle)4;
		this.GrdGrnDetail.LinkClicked += new ColumnActionEventHandler(GrdGrnDetail_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdGrnDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdGrnDetail_KeyDown);
		this.label17.BackColor = System.Drawing.Color.Teal;
		this.label17.Dock = System.Windows.Forms.DockStyle.Top;
		this.label17.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(0, 0);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(1339, 24);
		this.label17.TabIndex = 5;
		this.label17.Text = "GRN Detail";
		this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.InvoicesInfoPanel.Controls.Add((System.Windows.Forms.Control)(object)this.GrdInvoicesDetail);
		this.InvoicesInfoPanel.Controls.Add(this.label21);
		this.InvoicesInfoPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.InvoicesInfoPanel.Location = new System.Drawing.Point(0, 35);
		this.InvoicesInfoPanel.Name = "InvoicesInfoPanel";
		this.InvoicesInfoPanel.Size = new System.Drawing.Size(1339, 170);
		this.InvoicesInfoPanel.TabIndex = 8;
		this.GrdInvoicesDetail.AllowEdit = (InheritableBoolean)2;
		this.GrdInvoicesDetail.AlternatingColors = true;
		((ScrollableBase)this.GrdInvoicesDetail).BackColor = System.Drawing.Color.White;
		this.GrdInvoicesDetail.ColumnAutoResize = true;
		GrdInvoicesDetail_DesignTimeLayout.LayoutString = resources.GetString("GrdInvoicesDetail_DesignTimeLayout.LayoutString");
		this.GrdInvoicesDetail.DesignTimeLayout = GrdInvoicesDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdInvoicesDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdInvoicesDetail.DynamicFiltering = true;
		this.GrdInvoicesDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdInvoicesDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdInvoicesDetail.GroupByBoxVisible = false;
		this.GrdInvoicesDetail.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.GrdInvoicesDetail).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.GrdInvoicesDetail).Name = "GrdInvoicesDetail";
		((ScrollableBase)this.GrdInvoicesDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdInvoicesDetail).Size = new System.Drawing.Size(1339, 148);
		((System.Windows.Forms.Control)(object)this.GrdInvoicesDetail).TabIndex = 7;
		this.GrdInvoicesDetail.TotalRow = (InheritableBoolean)1;
		this.GrdInvoicesDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdInvoicesDetail).VisualStyle = (VisualStyle)4;
		this.GrdInvoicesDetail.LinkClicked += new ColumnActionEventHandler(GrdInvoicesDetail_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdInvoicesDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdInvoicesDetail_KeyDown);
		this.label21.BackColor = System.Drawing.Color.Teal;
		this.label21.Dock = System.Windows.Forms.DockStyle.Top;
		this.label21.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.White;
		this.label21.Location = new System.Drawing.Point(0, 0);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(1339, 22);
		this.label21.TabIndex = 6;
		this.label21.Text = "Invoices Detail";
		this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panelAllGridsCaption.Controls.Add(this.label11);
		this.panelAllGridsCaption.Controls.Add(this.label10);
		this.panelAllGridsCaption.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelAllGridsCaption.Location = new System.Drawing.Point(0, 0);
		this.panelAllGridsCaption.Name = "panelAllGridsCaption";
		this.panelAllGridsCaption.Size = new System.Drawing.Size(1339, 35);
		this.panelAllGridsCaption.TabIndex = 0;
		this.label11.BackColor = System.Drawing.Color.Teal;
		this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label11.ForeColor = System.Drawing.Color.Black;
		this.label11.Location = new System.Drawing.Point(5, 29);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(1220, 2);
		this.label11.TabIndex = 2;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Calibri", 15f, System.Drawing.FontStyle.Bold);
		this.label10.ForeColor = System.Drawing.Color.Teal;
		this.label10.Location = new System.Drawing.Point(4, 4);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(247, 24);
		this.label10.TabIndex = 1;
		this.label10.Text = "Order Recieving Information";
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdOrderEmptyBag);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.GrdOrderDetail);
		this.panel3.Controls.Add(this.panel4);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 174);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1356, 180);
		this.panel3.TabIndex = 1;
		this.grdOrderEmptyBag.AllowEdit = (InheritableBoolean)2;
		this.grdOrderEmptyBag.AlternatingColors = true;
		((ScrollableBase)this.grdOrderEmptyBag).BackColor = System.Drawing.Color.White;
		this.grdOrderEmptyBag.ColumnAutoResize = true;
		((System.Windows.Forms.Control)(object)this.grdOrderEmptyBag).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdOrderEmptyBag.DynamicFiltering = true;
		this.grdOrderEmptyBag.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdOrderEmptyBag).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOrderEmptyBag.GroupByBoxVisible = false;
		this.grdOrderEmptyBag.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.grdOrderEmptyBag).Location = new System.Drawing.Point(936, 24);
		((System.Windows.Forms.Control)(object)this.grdOrderEmptyBag).Name = "grdOrderEmptyBag";
		((ScrollableBase)this.grdOrderEmptyBag).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOrderEmptyBag).Size = new System.Drawing.Size(420, 156);
		((System.Windows.Forms.Control)(object)this.grdOrderEmptyBag).TabIndex = 4;
		this.grdOrderEmptyBag.TotalRow = (InheritableBoolean)1;
		this.grdOrderEmptyBag.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOrderEmptyBag).VisualStyle = (VisualStyle)4;
		this.GrdOrderDetail.AllowEdit = (InheritableBoolean)2;
		this.GrdOrderDetail.AlternatingColors = true;
		((ScrollableBase)this.GrdOrderDetail).BackColor = System.Drawing.Color.White;
		this.GrdOrderDetail.ColumnAutoResize = true;
		GrdOrderDetail_DesignTimeLayout.LayoutString = resources.GetString("GrdOrderDetail_DesignTimeLayout.LayoutString");
		this.GrdOrderDetail.DesignTimeLayout = GrdOrderDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdOrderDetail).Dock = System.Windows.Forms.DockStyle.Left;
		this.GrdOrderDetail.DynamicFiltering = true;
		this.GrdOrderDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdOrderDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdOrderDetail.GroupByBoxVisible = false;
		this.GrdOrderDetail.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.GrdOrderDetail).Location = new System.Drawing.Point(0, 24);
		((System.Windows.Forms.Control)(object)this.GrdOrderDetail).Name = "GrdOrderDetail";
		((ScrollableBase)this.GrdOrderDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdOrderDetail).Size = new System.Drawing.Size(936, 156);
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
		this.panel4.Size = new System.Drawing.Size(1356, 24);
		this.panel4.TabIndex = 0;
		this.label9.BackColor = System.Drawing.Color.Teal;
		this.label9.Dock = System.Windows.Forms.DockStyle.Top;
		this.label9.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(0, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(1356, 23);
		this.label9.TabIndex = 0;
		this.label9.Text = "Order Detail Information";
		this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel1.Controls.Add(this.panel7);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1356, 171);
		this.panel1.TabIndex = 0;
		this.panel7.Controls.Add(this.PanelHeaderCaption);
		this.panel7.Controls.Add(this.panel2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1356, 171);
		this.panel7.TabIndex = 3;
		this.PanelHeaderCaption.Controls.Add(this.BtnRefresh);
		this.PanelHeaderCaption.Controls.Add(this.btnshow);
		this.PanelHeaderCaption.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOrderNo);
		this.PanelHeaderCaption.Controls.Add(this.label42);
		this.PanelHeaderCaption.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGatePassNo);
		this.PanelHeaderCaption.Controls.Add(this.cmbsupplier);
		this.PanelHeaderCaption.Controls.Add(this.label1);
		this.PanelHeaderCaption.Controls.Add(this.label2);
		this.PanelHeaderCaption.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanelHeaderCaption.Location = new System.Drawing.Point(0, 0);
		this.PanelHeaderCaption.Name = "PanelHeaderCaption";
		this.PanelHeaderCaption.Size = new System.Drawing.Size(1356, 39);
		this.PanelHeaderCaption.TabIndex = 0;
		this.BtnRefresh.BackColor = System.Drawing.Color.Transparent;
		this.BtnRefresh.FlatAppearance.BorderSize = 0;
		this.BtnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnRefresh.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnRefresh.ForeColor = System.Drawing.Color.White;
		this.BtnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnRefresh.Location = new System.Drawing.Point(227, 7);
		this.BtnRefresh.Margin = new System.Windows.Forms.Padding(2);
		this.BtnRefresh.Name = "BtnRefresh";
		this.BtnRefresh.Size = new System.Drawing.Size(36, 24);
		this.BtnRefresh.TabIndex = 2;
		this.BtnRefresh.UseVisualStyleBackColor = false;
		this.BtnRefresh.Click += new System.EventHandler(BtnRefresh_Click);
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(625, 6);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(55, 24);
		this.btnshow.TabIndex = 2;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.CmbOrderNo.AllowNull = (DefaultableBoolean)1;
		this.CmbOrderNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOrderNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOrderNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbOrderNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Location = new System.Drawing.Point(521, 5);
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Name = "CmbOrderNo";
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Size = new System.Drawing.Size(99, 26);
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).TabIndex = 1;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(470, 11);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(47, 15);
		this.label42.TabIndex = 3;
		this.label42.Text = "Order #";
		this.CmbGatePassNo.AllowNull = (DefaultableBoolean)1;
		this.CmbGatePassNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGatePassNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGatePassNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGatePassNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbGatePassNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGatePassNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbGatePassNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbGatePassNo).Location = new System.Drawing.Point(388, 5);
		((System.Windows.Forms.Control)(object)this.CmbGatePassNo).Name = "CmbGatePassNo";
		((System.Windows.Forms.Control)(object)this.CmbGatePassNo).Size = new System.Drawing.Size(79, 26);
		((System.Windows.Forms.Control)(object)this.CmbGatePassNo).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbGatePassNo).Leave += new System.EventHandler(CmbGatePassNo_Leave);
		this.cmbsupplier.AutoSize = true;
		this.cmbsupplier.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbsupplier.Location = new System.Drawing.Point(315, 11);
		this.cmbsupplier.Name = "cmbsupplier";
		this.cmbsupplier.Size = new System.Drawing.Size(67, 15);
		this.cmbsupplier.TabIndex = 3;
		this.cmbsupplier.Text = "Gate Pass #";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Calibri", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Teal;
		this.label1.Location = new System.Drawing.Point(6, 5);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(216, 26);
		this.label1.TabIndex = 0;
		this.label1.Text = "Purchase Order Activity";
		this.label2.BackColor = System.Drawing.Color.Teal;
		this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label2.ForeColor = System.Drawing.Color.Black;
		this.label2.Location = new System.Drawing.Point(6, 34);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(1241, 2);
		this.label2.TabIndex = 1;
		this.panel2.Controls.Add(this.txtRemarks);
		this.panel2.Controls.Add(this.label27);
		this.panel2.Controls.Add(this.labelRemarks);
		this.panel2.Controls.Add(this.BtnCancelStatus);
		this.panel2.Controls.Add(this.BtnEditToForm);
		this.panel2.Controls.Add(this.BtnEditToRegister);
		this.panel2.Controls.Add(this.BtnCompleteStatus);
		this.panel2.Controls.Add(this.ModifyDate);
		this.panel2.Controls.Add(this.RecQty);
		this.panel2.Controls.Add(this.BalQty);
		this.panel2.Controls.Add(this.DueDays);
		this.panel2.Controls.Add(this.DeliveryDays);
		this.panel2.Controls.Add(this.BrokerAgentName);
		this.panel2.Controls.Add(this.CommissionAgentName);
		this.panel2.Controls.Add(this.SupplierName);
		this.panel2.Controls.Add(this.txtStatusChangedDate);
		this.panel2.Controls.Add(this.ApprovedDate);
		this.panel2.Controls.Add(this.ReferenceParty);
		this.panel2.Controls.Add(this.ApprovalStatus);
		this.panel2.Controls.Add(this.label34);
		this.panel2.Controls.Add(this.EntryDate);
		this.panel2.Controls.Add(this.OrderItemQty);
		this.panel2.Controls.Add(this.PaymentTerm);
		this.panel2.Controls.Add(this.label49);
		this.panel2.Controls.Add(this.DeliveryTerm);
		this.panel2.Controls.Add(this.label51);
		this.panel2.Controls.Add(this.label48);
		this.panel2.Controls.Add(this.label40);
		this.panel2.Controls.Add(this.label47);
		this.panel2.Controls.Add(this.lblModifyUser);
		this.panel2.Controls.Add(this.label38);
		this.panel2.Controls.Add(this.label33);
		this.panel2.Controls.Add(this.label32);
		this.panel2.Controls.Add(this.label28);
		this.panel2.Controls.Add(this.lblBroker);
		this.panel2.Controls.Add(this.lblCommAgent);
		this.panel2.Controls.Add(this.label24);
		this.panel2.Controls.Add(this.label23);
		this.panel2.Controls.Add(this.txtStatusChangedBy);
		this.panel2.Controls.Add(this.ApprovalUserName);
		this.panel2.Controls.Add(this.ModifyUserName);
		this.panel2.Controls.Add(this.label26);
		this.panel2.Controls.Add(this.EntryUserName);
		this.panel2.Controls.Add(this.lblApprovalUser);
		this.panel2.Controls.Add(this.label90);
		this.panel2.Controls.Add(this.label18);
		this.panel2.Controls.Add(this.BalanceWeight);
		this.panel2.Controls.Add(this.RecievedWeight);
		this.panel2.Controls.Add(this.OrderWeight);
		this.panel2.Controls.Add(this.label14);
		this.panel2.Controls.Add(this.label13);
		this.panel2.Controls.Add(this.label12);
		this.panel2.Controls.Add(this.ExpiryStatus);
		this.panel2.Controls.Add(this.OrderStatus);
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
		this.panel2.Location = new System.Drawing.Point(0, 39);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1356, 132);
		this.panel2.TabIndex = 2;
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(476, 104);
		this.txtRemarks.Margin = new System.Windows.Forms.Padding(2);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(342, 23);
		this.txtRemarks.TabIndex = 107;
		this.label27.BackColor = System.Drawing.Color.White;
		this.label27.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label27.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label27.Location = new System.Drawing.Point(326, 83);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(144, 19);
		this.label27.TabIndex = 108;
		this.label27.Text = "Mark As";
		this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.labelRemarks.BackColor = System.Drawing.Color.White;
		this.labelRemarks.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.labelRemarks.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.labelRemarks.Location = new System.Drawing.Point(476, 83);
		this.labelRemarks.Name = "labelRemarks";
		this.labelRemarks.Size = new System.Drawing.Size(342, 19);
		this.labelRemarks.TabIndex = 108;
		this.labelRemarks.Text = "ActionRemarks";
		this.labelRemarks.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.BtnCancelStatus.BackColor = System.Drawing.Color.Teal;
		this.BtnCancelStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelStatus.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.BtnCancelStatus.ForeColor = System.Drawing.Color.White;
		this.BtnCancelStatus.Location = new System.Drawing.Point(406, 104);
		this.BtnCancelStatus.Name = "BtnCancelStatus";
		this.BtnCancelStatus.Size = new System.Drawing.Size(65, 23);
		this.BtnCancelStatus.TabIndex = 57;
		this.BtnCancelStatus.Text = "Cancel";
		this.BtnCancelStatus.UseVisualStyleBackColor = false;
		this.BtnCancelStatus.Click += new System.EventHandler(BtnCancelStatus_Click);
		this.BtnEditToForm.BackColor = System.Drawing.Color.White;
		this.BtnEditToForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnEditToForm.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnEditToForm.ForeColor = System.Drawing.Color.White;
		this.BtnEditToForm.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.BtnEditToForm.Location = new System.Drawing.Point(85, 4);
		this.BtnEditToForm.Name = "BtnEditToForm";
		this.BtnEditToForm.Size = new System.Drawing.Size(23, 18);
		this.BtnEditToForm.TabIndex = 57;
		this.BtnEditToForm.UseVisualStyleBackColor = false;
		this.BtnEditToForm.Click += new System.EventHandler(BtnEditToForm_Click);
		this.BtnEditToRegister.BackColor = System.Drawing.Color.White;
		this.BtnEditToRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnEditToRegister.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnEditToRegister.ForeColor = System.Drawing.Color.White;
		this.BtnEditToRegister.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.BtnEditToRegister.Location = new System.Drawing.Point(162, 75);
		this.BtnEditToRegister.Name = "BtnEditToRegister";
		this.BtnEditToRegister.Size = new System.Drawing.Size(23, 18);
		this.BtnEditToRegister.TabIndex = 57;
		this.BtnEditToRegister.UseVisualStyleBackColor = false;
		this.BtnEditToRegister.Click += new System.EventHandler(BtnEditToRegister_Click);
		this.BtnCompleteStatus.BackColor = System.Drawing.Color.Teal;
		this.BtnCompleteStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCompleteStatus.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.BtnCompleteStatus.ForeColor = System.Drawing.Color.White;
		this.BtnCompleteStatus.Location = new System.Drawing.Point(326, 104);
		this.BtnCompleteStatus.Name = "BtnCompleteStatus";
		this.BtnCompleteStatus.Size = new System.Drawing.Size(80, 23);
		this.BtnCompleteStatus.TabIndex = 57;
		this.BtnCompleteStatus.Text = "Complete";
		this.BtnCompleteStatus.UseVisualStyleBackColor = false;
		this.BtnCompleteStatus.Click += new System.EventHandler(BtnCompleteStatus_Click);
		this.ModifyDate.AutoSize = true;
		this.ModifyDate.BackColor = System.Drawing.Color.Transparent;
		this.ModifyDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ModifyDate.ForeColor = System.Drawing.SystemColors.ControlText;
		this.ModifyDate.Location = new System.Drawing.Point(1173, 58);
		this.ModifyDate.Name = "ModifyDate";
		this.ModifyDate.Size = new System.Drawing.Size(74, 15);
		this.ModifyDate.TabIndex = 56;
		this.ModifyDate.Text = "Modify Date";
		this.RecQty.AutoSize = true;
		this.RecQty.BackColor = System.Drawing.Color.Transparent;
		this.RecQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RecQty.ForeColor = System.Drawing.SystemColors.ControlText;
		this.RecQty.Location = new System.Drawing.Point(936, 23);
		this.RecQty.Name = "RecQty";
		this.RecQty.Size = new System.Drawing.Size(14, 15);
		this.RecQty.TabIndex = 55;
		this.RecQty.Text = "0";
		this.BalQty.AutoSize = true;
		this.BalQty.BackColor = System.Drawing.Color.Transparent;
		this.BalQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BalQty.ForeColor = System.Drawing.Color.Crimson;
		this.BalQty.Location = new System.Drawing.Point(936, 40);
		this.BalQty.Name = "BalQty";
		this.BalQty.Size = new System.Drawing.Size(16, 16);
		this.BalQty.TabIndex = 54;
		this.BalQty.Text = "0";
		this.DueDays.AutoSize = true;
		this.DueDays.BackColor = System.Drawing.Color.Transparent;
		this.DueDays.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.DueDays.ForeColor = System.Drawing.SystemColors.ControlText;
		this.DueDays.Location = new System.Drawing.Point(697, 23);
		this.DueDays.Name = "DueDays";
		this.DueDays.Size = new System.Drawing.Size(57, 15);
		this.DueDays.TabIndex = 53;
		this.DueDays.Text = "Due Days";
		this.DeliveryDays.AutoSize = true;
		this.DeliveryDays.BackColor = System.Drawing.Color.Transparent;
		this.DeliveryDays.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.DeliveryDays.ForeColor = System.Drawing.SystemColors.ControlText;
		this.DeliveryDays.Location = new System.Drawing.Point(696, 59);
		this.DeliveryDays.Name = "DeliveryDays";
		this.DeliveryDays.Size = new System.Drawing.Size(77, 15);
		this.DeliveryDays.TabIndex = 52;
		this.DeliveryDays.Text = "DeliveryDays";
		this.BrokerAgentName.AutoSize = true;
		this.BrokerAgentName.BackColor = System.Drawing.Color.Transparent;
		this.BrokerAgentName.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BrokerAgentName.ForeColor = System.Drawing.SystemColors.ControlText;
		this.BrokerAgentName.Location = new System.Drawing.Point(326, 58);
		this.BrokerAgentName.Name = "BrokerAgentName";
		this.BrokerAgentName.Size = new System.Drawing.Size(40, 15);
		this.BrokerAgentName.TabIndex = 51;
		this.BrokerAgentName.Text = "Name";
		this.CommissionAgentName.AutoSize = true;
		this.CommissionAgentName.BackColor = System.Drawing.Color.Transparent;
		this.CommissionAgentName.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.CommissionAgentName.ForeColor = System.Drawing.SystemColors.ControlText;
		this.CommissionAgentName.Location = new System.Drawing.Point(326, 40);
		this.CommissionAgentName.Name = "CommissionAgentName";
		this.CommissionAgentName.Size = new System.Drawing.Size(109, 15);
		this.CommissionAgentName.TabIndex = 50;
		this.CommissionAgentName.Text = "CommAgentName";
		this.SupplierName.AutoSize = true;
		this.SupplierName.BackColor = System.Drawing.Color.Transparent;
		this.SupplierName.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.SupplierName.ForeColor = System.Drawing.SystemColors.ControlText;
		this.SupplierName.Location = new System.Drawing.Point(326, 6);
		this.SupplierName.Name = "SupplierName";
		this.SupplierName.Size = new System.Drawing.Size(88, 15);
		this.SupplierName.TabIndex = 49;
		this.SupplierName.Text = "Supplier Name";
		this.txtStatusChangedDate.AutoSize = true;
		this.txtStatusChangedDate.BackColor = System.Drawing.Color.Transparent;
		this.txtStatusChangedDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtStatusChangedDate.ForeColor = System.Drawing.SystemColors.ControlText;
		this.txtStatusChangedDate.Location = new System.Drawing.Point(130, 113);
		this.txtStatusChangedDate.Name = "txtStatusChangedDate";
		this.txtStatusChangedDate.Size = new System.Drawing.Size(120, 15);
		this.txtStatusChangedDate.TabIndex = 48;
		this.txtStatusChangedDate.Text = "Status Changed Date";
		this.ApprovedDate.AutoSize = true;
		this.ApprovedDate.BackColor = System.Drawing.Color.Transparent;
		this.ApprovedDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ApprovedDate.ForeColor = System.Drawing.SystemColors.ControlText;
		this.ApprovedDate.Location = new System.Drawing.Point(1173, 95);
		this.ApprovedDate.Name = "ApprovedDate";
		this.ApprovedDate.Size = new System.Drawing.Size(90, 15);
		this.ApprovedDate.TabIndex = 48;
		this.ApprovedDate.Text = "Approved Date";
		this.ReferenceParty.AutoSize = true;
		this.ReferenceParty.BackColor = System.Drawing.Color.Transparent;
		this.ReferenceParty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ReferenceParty.ForeColor = System.Drawing.SystemColors.ControlText;
		this.ReferenceParty.Location = new System.Drawing.Point(326, 23);
		this.ReferenceParty.Name = "ReferenceParty";
		this.ReferenceParty.Size = new System.Drawing.Size(95, 15);
		this.ReferenceParty.TabIndex = 47;
		this.ReferenceParty.Text = "Reference Party";
		this.ApprovalStatus.AutoSize = true;
		this.ApprovalStatus.BackColor = System.Drawing.Color.Transparent;
		this.ApprovalStatus.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ApprovalStatus.ForeColor = System.Drawing.SystemColors.ControlText;
		this.ApprovalStatus.Location = new System.Drawing.Point(1173, 114);
		this.ApprovalStatus.Name = "ApprovalStatus";
		this.ApprovalStatus.Size = new System.Drawing.Size(93, 15);
		this.ApprovalStatus.TabIndex = 46;
		this.ApprovalStatus.Text = "Approval Status";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(1069, 114);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(93, 15);
		this.label34.TabIndex = 45;
		this.label34.Text = "Approval Status";
		this.EntryDate.AutoSize = true;
		this.EntryDate.BackColor = System.Drawing.Color.Transparent;
		this.EntryDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.EntryDate.ForeColor = System.Drawing.SystemColors.ControlText;
		this.EntryDate.Location = new System.Drawing.Point(1173, 23);
		this.EntryDate.Name = "EntryDate";
		this.EntryDate.Size = new System.Drawing.Size(65, 15);
		this.EntryDate.TabIndex = 44;
		this.EntryDate.Text = "Entry Date";
		this.OrderItemQty.AutoSize = true;
		this.OrderItemQty.BackColor = System.Drawing.Color.Transparent;
		this.OrderItemQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.OrderItemQty.ForeColor = System.Drawing.SystemColors.ControlText;
		this.OrderItemQty.Location = new System.Drawing.Point(936, 6);
		this.OrderItemQty.Name = "OrderItemQty";
		this.OrderItemQty.Size = new System.Drawing.Size(14, 15);
		this.OrderItemQty.TabIndex = 43;
		this.OrderItemQty.Text = "0";
		this.PaymentTerm.AutoSize = true;
		this.PaymentTerm.BackColor = System.Drawing.Color.Transparent;
		this.PaymentTerm.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.PaymentTerm.ForeColor = System.Drawing.SystemColors.ControlText;
		this.PaymentTerm.Location = new System.Drawing.Point(697, 6);
		this.PaymentTerm.Name = "PaymentTerm";
		this.PaymentTerm.Size = new System.Drawing.Size(87, 15);
		this.PaymentTerm.TabIndex = 42;
		this.PaymentTerm.Text = "Payment Term";
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(602, 58);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(84, 15);
		this.label49.TabIndex = 41;
		this.label49.Text = "Delivery Days:";
		this.DeliveryTerm.AutoSize = true;
		this.DeliveryTerm.BackColor = System.Drawing.Color.Transparent;
		this.DeliveryTerm.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.DeliveryTerm.ForeColor = System.Drawing.SystemColors.ControlText;
		this.DeliveryTerm.Location = new System.Drawing.Point(697, 40);
		this.DeliveryTerm.Name = "DeliveryTerm";
		this.DeliveryTerm.Size = new System.Drawing.Size(83, 15);
		this.DeliveryTerm.TabIndex = 40;
		this.DeliveryTerm.Text = "Delivery Term";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(602, 40);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(87, 15);
		this.label51.TabIndex = 39;
		this.label51.Text = "Delivery Term:";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(602, 6);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(91, 15);
		this.label48.TabIndex = 37;
		this.label48.Text = "Payment Term:";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(6, 113);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(127, 15);
		this.label40.TabIndex = 35;
		this.label40.Text = "Status Changed Date: ";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(1069, 95);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(97, 15);
		this.label47.TabIndex = 35;
		this.label47.Text = "Approved Date: ";
		this.lblModifyUser.AutoSize = true;
		this.lblModifyUser.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblModifyUser.Location = new System.Drawing.Point(1069, 58);
		this.lblModifyUser.Name = "lblModifyUser";
		this.lblModifyUser.Size = new System.Drawing.Size(88, 15);
		this.lblModifyUser.TabIndex = 33;
		this.lblModifyUser.Text = "Last Modified: ";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(1069, 23);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(72, 15);
		this.label38.TabIndex = 31;
		this.label38.Text = "Entry Date: ";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.ForeColor = System.Drawing.Color.Crimson;
		this.label33.Location = new System.Drawing.Point(823, 40);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(75, 15);
		this.label33.TabIndex = 30;
		this.label33.Text = "Balance Qty:";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(823, 23);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(83, 15);
		this.label32.TabIndex = 29;
		this.label32.Text = "Received Qty:";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(823, 6);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(67, 15);
		this.label28.TabIndex = 28;
		this.label28.Text = "Order Qty:";
		this.lblBroker.AutoSize = true;
		this.lblBroker.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBroker.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
		this.lblBroker.Location = new System.Drawing.Point(210, 58);
		this.lblBroker.Name = "lblBroker";
		this.lblBroker.Size = new System.Drawing.Size(84, 15);
		this.lblBroker.TabIndex = 27;
		this.lblBroker.Text = "Broker Name:";
		this.lblCommAgent.AutoSize = true;
		this.lblCommAgent.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCommAgent.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
		this.lblCommAgent.Location = new System.Drawing.Point(210, 40);
		this.lblCommAgent.Name = "lblCommAgent";
		this.lblCommAgent.Size = new System.Drawing.Size(113, 15);
		this.lblCommAgent.TabIndex = 26;
		this.lblCommAgent.Text = "Commission Agent:";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(210, 23);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(61, 15);
		this.label24.TabIndex = 25;
		this.label24.Text = "Ref Party:";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(210, 6);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(92, 15);
		this.label23.TabIndex = 24;
		this.label23.Text = "Supplier Name:";
		this.txtStatusChangedBy.AutoSize = true;
		this.txtStatusChangedBy.BackColor = System.Drawing.Color.Transparent;
		this.txtStatusChangedBy.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtStatusChangedBy.ForeColor = System.Drawing.SystemColors.ControlText;
		this.txtStatusChangedBy.Location = new System.Drawing.Point(130, 95);
		this.txtStatusChangedBy.Name = "txtStatusChangedBy";
		this.txtStatusChangedBy.Size = new System.Drawing.Size(107, 15);
		this.txtStatusChangedBy.TabIndex = 23;
		this.txtStatusChangedBy.Text = "Status Changed By";
		this.ApprovalUserName.AutoSize = true;
		this.ApprovalUserName.BackColor = System.Drawing.Color.Transparent;
		this.ApprovalUserName.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ApprovalUserName.ForeColor = System.Drawing.SystemColors.ControlText;
		this.ApprovalUserName.Location = new System.Drawing.Point(1173, 77);
		this.ApprovalUserName.Name = "ApprovalUserName";
		this.ApprovalUserName.Size = new System.Drawing.Size(77, 15);
		this.ApprovalUserName.TabIndex = 23;
		this.ApprovalUserName.Text = "Approved By";
		this.ModifyUserName.AutoSize = true;
		this.ModifyUserName.BackColor = System.Drawing.Color.Transparent;
		this.ModifyUserName.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ModifyUserName.ForeColor = System.Drawing.SystemColors.ControlText;
		this.ModifyUserName.Location = new System.Drawing.Point(1173, 40);
		this.ModifyUserName.Name = "ModifyUserName";
		this.ModifyUserName.Size = new System.Drawing.Size(122, 15);
		this.ModifyUserName.TabIndex = 22;
		this.ModifyUserName.Text = "Load & Factory Weight";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(6, 95);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(114, 15);
		this.label26.TabIndex = 20;
		this.label26.Text = "Status Changed By: ";
		this.EntryUserName.AutoSize = true;
		this.EntryUserName.BackColor = System.Drawing.Color.Transparent;
		this.EntryUserName.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.EntryUserName.ForeColor = System.Drawing.SystemColors.ControlText;
		this.EntryUserName.Location = new System.Drawing.Point(1173, 6);
		this.EntryUserName.Name = "EntryUserName";
		this.EntryUserName.Size = new System.Drawing.Size(122, 15);
		this.EntryUserName.TabIndex = 21;
		this.EntryUserName.Text = "Load & Factory Weight";
		this.lblApprovalUser.AutoSize = true;
		this.lblApprovalUser.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblApprovalUser.Location = new System.Drawing.Point(1069, 77);
		this.lblApprovalUser.Name = "lblApprovalUser";
		this.lblApprovalUser.Size = new System.Drawing.Size(84, 15);
		this.lblApprovalUser.TabIndex = 20;
		this.lblApprovalUser.Text = "Approved By: ";
		this.label90.AutoSize = true;
		this.label90.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label90.Location = new System.Drawing.Point(1069, 40);
		this.label90.Name = "label90";
		this.label90.Size = new System.Drawing.Size(104, 15);
		this.label90.TabIndex = 19;
		this.label90.Text = "Last Modified By: ";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(1069, 6);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(71, 15);
		this.label18.TabIndex = 18;
		this.label18.Text = "Entry User: ";
		this.BalanceWeight.AutoSize = true;
		this.BalanceWeight.BackColor = System.Drawing.Color.Transparent;
		this.BalanceWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BalanceWeight.ForeColor = System.Drawing.Color.Crimson;
		this.BalanceWeight.Location = new System.Drawing.Point(936, 95);
		this.BalanceWeight.Name = "BalanceWeight";
		this.BalanceWeight.Size = new System.Drawing.Size(16, 16);
		this.BalanceWeight.TabIndex = 17;
		this.BalanceWeight.Text = "0";
		this.RecievedWeight.AutoSize = true;
		this.RecievedWeight.BackColor = System.Drawing.Color.Transparent;
		this.RecievedWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RecievedWeight.ForeColor = System.Drawing.SystemColors.ControlText;
		this.RecievedWeight.Location = new System.Drawing.Point(936, 77);
		this.RecievedWeight.Name = "RecievedWeight";
		this.RecievedWeight.Size = new System.Drawing.Size(14, 15);
		this.RecievedWeight.TabIndex = 16;
		this.RecievedWeight.Text = "0";
		this.OrderWeight.AutoSize = true;
		this.OrderWeight.BackColor = System.Drawing.Color.Transparent;
		this.OrderWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.OrderWeight.ForeColor = System.Drawing.SystemColors.ControlText;
		this.OrderWeight.Location = new System.Drawing.Point(936, 58);
		this.OrderWeight.Name = "OrderWeight";
		this.OrderWeight.Size = new System.Drawing.Size(14, 15);
		this.OrderWeight.TabIndex = 15;
		this.OrderWeight.Text = "0";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.Crimson;
		this.label14.Location = new System.Drawing.Point(823, 95);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(95, 15);
		this.label14.TabIndex = 14;
		this.label14.Text = "Balance Weight:";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(823, 77);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(103, 15);
		this.label13.TabIndex = 13;
		this.label13.Text = "Recieved Weight:";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(823, 58);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(87, 15);
		this.label12.TabIndex = 12;
		this.label12.Text = "Order Weight:";
		this.ExpiryStatus.AutoSize = true;
		this.ExpiryStatus.BackColor = System.Drawing.Color.Red;
		this.ExpiryStatus.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ExpiryStatus.ForeColor = System.Drawing.Color.White;
		this.ExpiryStatus.Location = new System.Drawing.Point(85, 58);
		this.ExpiryStatus.Name = "ExpiryStatus";
		this.ExpiryStatus.Size = new System.Drawing.Size(48, 15);
		this.ExpiryStatus.TabIndex = 11;
		this.ExpiryStatus.Text = "Expired";
		this.OrderStatus.AutoSize = true;
		this.OrderStatus.BackColor = System.Drawing.Color.Goldenrod;
		this.OrderStatus.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.OrderStatus.ForeColor = System.Drawing.Color.White;
		this.OrderStatus.Location = new System.Drawing.Point(85, 77);
		this.OrderStatus.Name = "OrderStatus";
		this.OrderStatus.Size = new System.Drawing.Size(35, 15);
		this.OrderStatus.TabIndex = 10;
		this.OrderStatus.Text = "open";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(6, 77);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(81, 15);
		this.label7.TabIndex = 8;
		this.label7.Text = "Order Status:";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(6, 58);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(81, 15);
		this.label6.TabIndex = 7;
		this.label6.Text = "Expiry Status:";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(602, 23);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(61, 15);
		this.label4.TabIndex = 6;
		this.label4.Text = "Due Days:";
		this.ExpiryDate.AutoSize = true;
		this.ExpiryDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ExpiryDate.Location = new System.Drawing.Point(85, 40);
		this.ExpiryDate.Name = "ExpiryDate";
		this.ExpiryDate.Size = new System.Drawing.Size(64, 15);
		this.ExpiryDate.TabIndex = 5;
		this.ExpiryDate.Text = "9-12-2022";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
		this.label8.Location = new System.Drawing.Point(6, 40);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(73, 15);
		this.label8.TabIndex = 4;
		this.label8.Text = "Expiry Date:";
		this.OrderDate.AutoSize = true;
		this.OrderDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.OrderDate.Location = new System.Drawing.Point(85, 23);
		this.OrderDate.Name = "OrderDate";
		this.OrderDate.Size = new System.Drawing.Size(64, 15);
		this.OrderDate.TabIndex = 3;
		this.OrderDate.Text = "9-12-2022";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
		this.label5.Location = new System.Drawing.Point(6, 23);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(73, 15);
		this.label5.TabIndex = 2;
		this.label5.Text = "Order Date:";
		this.OrderNo.AutoSize = true;
		this.OrderNo.Cursor = System.Windows.Forms.Cursors.Hand;
		this.OrderNo.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.OrderNo.ForeColor = System.Drawing.Color.RoyalBlue;
		this.OrderNo.Location = new System.Drawing.Point(60, 6);
		this.OrderNo.Name = "OrderNo";
		this.OrderNo.Size = new System.Drawing.Size(21, 15);
		this.OrderNo.TabIndex = 1;
		this.OrderNo.Text = "29";
		this.OrderNo.Click += new System.EventHandler(OrderNo_Click);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
		this.label3.Location = new System.Drawing.Point(6, 6);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(53, 15);
		this.label3.TabIndex = 0;
		this.label3.Text = "Order #:";
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
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "ActivityDetails";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ActivityDetails_FormClosing);
		base.Load += new System.EventHandler(ActivityDetails_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ActivityDetails_KeyDown);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.PanelAllGrids.ResumeLayout(false);
		this.WagesInfoPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdWagesInformation).EndInit();
		this.WeighBridgePanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdWeighBridge).EndInit();
		this.GatePassesDetailPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdGatePass).EndInit();
		this.LabInfoPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdLabInformation).EndInit();
		this.GrnDetailPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdGrnDetail).EndInit();
		this.InvoicesInfoPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdInvoicesDetail).EndInit();
		this.panelAllGridsCaption.ResumeLayout(false);
		this.panelAllGridsCaption.PerformLayout();
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOrderEmptyBag).EndInit();
		((System.ComponentModel.ISupportInitialize)this.GrdOrderDetail).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.PanelHeaderCaption.ResumeLayout(false);
		this.PanelHeaderCaption.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGatePassNo).EndInit();
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
