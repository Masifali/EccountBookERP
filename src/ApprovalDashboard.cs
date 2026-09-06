using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Main;
using Architecture.WinApp.Cmagt.Approvals;
using Architecture.WinApp.Common;
using Architecture.WinApp.DynamicCards;
using Architecture.WinApp.FeedMill.Approval;
using Architecture.WinApp.HRM.PayRoll;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Architecture.WinApp.SpecialApprovalDashboard;
using Architecture.WinApp.lgstcm.Approvals;
using Architecture.WinApp.pcc.Approvals;
using Infragistics.Win;
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
namespace Architecture.WinApp.ApprovalDashboard
{

public class ApprovalDashboard : Form
{
	private bool BranchFeature;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private GroupBox GroupBoxInventory;

	private FlowLayoutPanel inventoryFlowLayout;

	private GroupBox GroupBoxAccounts;

	private FlowLayoutPanel accountsFlowLayout;

	private Panel panel2;

	private Label label2;

	private Panel panel5;

	private Label label1;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton BtnSpecialApprovalDashBoard;

	private Panel FilterPanel;

	private GroupBox groupBox3;

	private Button BtnShow;

	internal UltraCombo CmbBranch;

	private Label lblCompany;

	private GroupBox GroupBoxExport;

	private FlowLayoutPanel ExportFlowLayout;

	private Panel panel3;

	private Label label3;

	private Label label4;

	private DateTimePicker datFromDate;

	private Label label33;

	private DateTimePicker datToDate;

	private Panel PanelExportAndCommissionAgent;

	private GroupBox GroupBoxCommissionAgent;

	private FlowLayoutPanel flowLayoutPanel1;

	private Panel panel4;

	private Label label5;

	public ApprovalDashboard(UserAccount pUser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUser;
	}

	private void ApprovalDashboard_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1450, 650);
			BranchFeature = CommonServices.GetERPFeatureById(11);
			if (BranchFeature)
			{
				((Control)(object)CmbBranch).Enabled = true;
			}
			else
			{
				((Control)(object)CmbBranch).Enabled = false;
			}
			datFromDate.Value = DateTime.Now.AddDays(-7.0);
			datToDate.Value = DateTime.Now;
			BranchesBind();
			DynamicallyGenerateCards();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchesBind()
	{
		try
		{
			List<getGlobalBranchesAllocateToUser> lst = clsGlobalVariables.globalBranchesAllocateToUser;
			DataTable dtBranch = new DataTable();
			dtBranch.Columns.Add("BranchId", typeof(int));
			dtBranch.Columns.Add("BranchName", typeof(string));
			if (lst.Count > 0)
			{
				for (int i = 0; i < lst.Count; i++)
				{
					dtBranch.Rows.Add(lst[i].BranchId, lst[i].BranchName);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbBranch, dtBranch, "BranchId", "BranchName", "Branch Name", AllColumns: true, Conversion.ToInt(CmbBranch.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dtBranch.Rows.Count > 0)
			{
				CmbBranch.Value = UserAccount.BranchesId;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DynamicallyGenerateCards()
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		try
		{
			accountsFlowLayout.Controls.Clear();
			inventoryFlowLayout.Controls.Clear();
			ExportFlowLayout.Controls.Clear();
			flowLayoutPanel1.Controls.Clear();
			if (BranchFeature && Conversion.ToInt(CmbBranch.Value) == 0)
			{
				CmbBranch.Focus();
				throw new Exception("Please Select Branch First");
			}
			ReportsParameters obj = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				UserId = UserAccount.ID,
				BranchesId = (BranchFeature ? Conversion.ToInt(CmbBranch.Value) : UserAccount.BranchesId)
			};
			if (datFromDate.Checked)
			{
				obj.FromDate = datFromDate.Value;
			}
			if (datToDate.Checked)
			{
				obj.ToDate = datToDate.Value;
			}
			DataTable source = Dashboard.ReadAccountsDashboardPendingForApproval(obj);
			DataTable dtInventory = Dashboard.ReadInventoryDashboardPendingForApproval(obj);
			EnumerableRowCollection<DataRow> accountRows = from r in source.AsEnumerable()
				where Conversion.ToInt(r["ActionId"]) != 2
				select r;
			RenderCards(accountRows, accountsFlowLayout, (DataRow r) => r["VoucherType"]?.ToString());
			EnumerableRowCollection<DataRow> inventoryRows = from r in dtInventory.AsEnumerable()
				where Conversion.ToInt(r["ActionId"]) != 3 && Conversion.ToInt(r["ActionId"]) != 5
				select r;
			RenderCards(inventoryRows, inventoryFlowLayout, (DataRow r) => r["Description"]?.ToString());
			EnumerableRowCollection<DataRow> exportInventory = from r in dtInventory.AsEnumerable()
				where Conversion.ToInt(r["ActionId"]) == 3
				select r;
			EnumerableRowCollection<DataRow> exportAccounts = from r in source.AsEnumerable()
				where Conversion.ToInt(r["ActionId"]) == 2
				select r;
			RenderCards(exportInventory, ExportFlowLayout, (DataRow r) => r["Description"]?.ToString());
			RenderCards(exportAccounts, ExportFlowLayout, (DataRow r) => r["VoucherType"]?.ToString());
			bool hasExport = exportInventory.Any() || exportAccounts.Any();
			GroupBoxExport.Visible = hasExport;
			EnumerableRowCollection<DataRow> commissionRows = from r in dtInventory.AsEnumerable()
				where Conversion.ToInt(r["ActionId"]) == 5
				select r;
			RenderCards(commissionRows, flowLayoutPanel1, (DataRow r) => r["Description"]?.ToString());
			bool hasCommission = commissionRows.Any();
			GroupBoxCommissionAgent.Visible = hasCommission;
			PanelExportAndCommissionAgent.Visible = hasExport || hasCommission;
			GroupBoxExport.Dock = (hasCommission ? DockStyle.Top : DockStyle.Fill);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		void RenderCards(IEnumerable<DataRow> rows, FlowLayoutPanel panel, Func<DataRow, string> titleSelector)
		{
			foreach (DataRow row in rows)
			{
				DynamicallyGenerateCards card = new DynamicallyGenerateCards
				{
					Id = row["TypeID"]?.ToString(),
					Title = titleSelector(row),
					Count = row["Count"]?.ToString()
				};
				card.UserControlClicked += UserControl_Click;
				panel.Controls.Add(card);
			}
		}
	}

	private void UserControl_Click(object sender, EventArgs e)
	{
		int id = Conversion.ToInt((object)((DynamicallyGenerateCards)sender).Id);
		int BranchesId = ((Conversion.ToInt(CmbBranch.Value) > 0) ? Conversion.ToInt(CmbBranch.Value) : 0);
		if ((id >= 0 && id <= 9) || id == 18 || id == 34 || id == 35)
		{
			PendingApprovalVouchersHistory obj = new PendingApprovalVouchersHistory(UserAccount);
			PendingApprovalVouchersHistory.Id = id;
			if (datFromDate.Checked)
			{
				obj.RequestedFromDate = datFromDate.Value;
			}
			obj.RequestedToDate = datToDate.Value;
			obj.ShowDialog();
			obj.BringToFront();
		}
		else
		{
			switch (id)
			{
			case 1000:
			{
				frmPayrollPosting frmPayrollPosting = new frmPayrollPosting(UserAccount);
				frmPayrollPosting.ApprovedStatus = true;
				frmPayrollPosting.ShowDialog();
				frmPayrollPosting.BringToFront();
				break;
			}
			case 19:
			{
				CommercialInvoiceApproval obj2 = new CommercialInvoiceApproval(UserAccount);
				if (datFromDate.Checked)
				{
					obj2.RequestedFromDate = datFromDate.Value;
				}
				obj2.RequestedToDate = datToDate.Value;
				obj2.BringToFront();
				obj2.ShowDialog();
				break;
			}
			case 20:
			{
				PurchaseOrderForApproval obj3 = new PurchaseOrderForApproval(UserAccount);
				obj3.BranchesId = BranchesId;
				if (datFromDate.Checked)
				{
					obj3.RequestedFromDate = datFromDate.Value;
				}
				obj3.RequestedToDate = datToDate.Value;
				obj3.ShowDialog();
				obj3.BringToFront();
				break;
			}
			case 21:
			{
				PurchaseOrderStoreForApproval obj4 = new PurchaseOrderStoreForApproval(UserAccount);
				obj4.DocumentTypeId = 42;
				if (datFromDate.Checked)
				{
					obj4.RequestedFromDate = datFromDate.Value;
				}
				obj4.RequestedToDate = datToDate.Value;
				obj4.ShowDialog();
				obj4.BringToFront();
				break;
			}
			case 23:
			{
				PendingApprovalPurchaseInvoice obj5 = new PendingApprovalPurchaseInvoice(UserAccount);
				PendingApprovalPurchaseInvoice.DocumentTypeId = 56;
				obj5.BranchesId = BranchesId;
				if (datFromDate.Checked)
				{
					obj5.RequestedFromDate = datFromDate.Value;
				}
				obj5.RequestedToDate = datToDate.Value;
				obj5.ShowDialog();
				obj5.BringToFront();
				break;
			}
			case 24:
			{
				PendingApprovalPurchaseInvoice obj6 = new PendingApprovalPurchaseInvoice(UserAccount);
				PendingApprovalPurchaseInvoice.DocumentTypeId = 57;
				obj6.BranchesId = BranchesId;
				if (datFromDate.Checked)
				{
					obj6.RequestedFromDate = datFromDate.Value;
				}
				obj6.RequestedToDate = datToDate.Value;
				obj6.ShowDialog();
				obj6.BringToFront();
				break;
			}
			case 26:
			{
				PendingApprovalPurchaseInvoice obj7 = new PendingApprovalPurchaseInvoice(UserAccount);
				PendingApprovalPurchaseInvoice.DocumentTypeId = 59;
				obj7.BranchesId = BranchesId;
				if (datFromDate.Checked)
				{
					obj7.RequestedFromDate = datFromDate.Value;
				}
				obj7.RequestedToDate = datToDate.Value;
				obj7.ShowDialog();
				obj7.BringToFront();
				break;
			}
			case 28:
			{
				frmSaleOrderForApproval obj8 = new frmSaleOrderForApproval(UserAccount);
				obj8.DocumentTypeId = 81;
				obj8.BranchesId = BranchesId;
				if (datFromDate.Checked)
				{
					obj8.RequestedFromDate = datFromDate.Value;
				}
				obj8.RequestedToDate = datToDate.Value;
				obj8.ShowDialog();
				obj8.BringToFront();
				break;
			}
			case 90:
			{
				frmSaleOrderForApproval obj9 = new frmSaleOrderForApproval(UserAccount);
				obj9.DocumentTypeId = 90;
				obj9.BranchesId = BranchesId;
				if (datFromDate.Checked)
				{
					obj9.RequestedFromDate = datFromDate.Value;
				}
				obj9.RequestedToDate = datToDate.Value;
				obj9.ShowDialog();
				obj9.BringToFront();
				break;
			}
			case 54:
			{
				frmSaleOrderForApproval obj10 = new frmSaleOrderForApproval(UserAccount);
				obj10.DocumentTypeId = 127;
				obj10.BranchesId = BranchesId;
				if (datFromDate.Checked)
				{
					obj10.RequestedFromDate = datFromDate.Value;
				}
				obj10.RequestedToDate = datToDate.Value;
				obj10.ShowDialog();
				obj10.BringToFront();
				break;
			}
			case 70:
			case 1656:
			{
				SaleOrderForApproval_Engr obj11 = new SaleOrderForApproval_Engr(UserAccount);
				obj11.DocumentTypeId = id switch
				{
					1656 => 1656, 
					70 => 1605, 
					_ => 0, 
				};
				obj11.BranchesId = BranchesId;
				if (datFromDate.Checked)
				{
					obj11.RequestedFromDate = datFromDate.Value;
				}
				obj11.RequestedToDate = datToDate.Value;
				obj11.ShowDialog();
				obj11.BringToFront();
				break;
			}
			case 76:
			{
				PendingGRNForApproval obj12 = new PendingGRNForApproval(UserAccount);
				PendingGRNForApproval.DocumentTypeId = 46;
				obj12.BranchesId = BranchesId;
				if (datFromDate.Checked)
				{
					obj12.RequestedFromDate = datFromDate.Value;
				}
				obj12.RequestedToDate = datToDate.Value;
				obj12.ShowDialog();
				obj12.BringToFront();
				break;
			}
			case 73:
			{
				frmDayBookApproval obj76 = new frmDayBookApproval(UserAccount);
				obj76.IsApproved = false;
				obj76.ShowDialog();
				obj76.BringToFront();
				break;
			}
			case 700:
			{
				PurchaseOrderStoreForApproval obj13 = new PurchaseOrderStoreForApproval(UserAccount);
				obj13.DocumentTypeId = 700;
				if (datFromDate.Checked)
				{
					obj13.RequestedFromDate = datFromDate.Value;
				}
				obj13.RequestedToDate = datToDate.Value;
				obj13.ShowDialog();
				obj13.BringToFront();
				break;
			}
			case 1600:
			{
				PurchaseOrderStoreForApproval obj14 = new PurchaseOrderStoreForApproval(UserAccount);
				obj14.DocumentTypeId = 1600;
				if (datFromDate.Checked)
				{
					obj14.RequestedFromDate = datFromDate.Value;
				}
				obj14.RequestedToDate = datToDate.Value;
				obj14.ShowDialog();
				obj14.BringToFront();
				break;
			}
			case 138:
			{
				PendingApprovalPurchaseInvoice obj15 = new PendingApprovalPurchaseInvoice(UserAccount);
				PendingApprovalPurchaseInvoice.DocumentTypeId = 138;
				obj15.BranchesId = BranchesId;
				if (datFromDate.Checked)
				{
					obj15.RequestedFromDate = datFromDate.Value;
				}
				obj15.RequestedToDate = datToDate.Value;
				obj15.ShowDialog();
				obj15.BringToFront();
				break;
			}
			case 60:
			{
				PendingApprovalPurchaseInvoice obj16 = new PendingApprovalPurchaseInvoice(UserAccount);
				PendingApprovalPurchaseInvoice.DocumentTypeId = 172;
				obj16.BranchesId = BranchesId;
				if (datFromDate.Checked)
				{
					obj16.RequestedFromDate = datFromDate.Value;
				}
				obj16.RequestedToDate = datToDate.Value;
				obj16.ShowDialog();
				obj16.BringToFront();
				break;
			}
			case 36:
			{
				PendingApprovalPurchaseInvoice obj17 = new PendingApprovalPurchaseInvoice(UserAccount);
				PendingApprovalPurchaseInvoice.DocumentTypeId = 98;
				obj17.BranchesId = BranchesId;
				if (datFromDate.Checked)
				{
					obj17.RequestedFromDate = datFromDate.Value;
				}
				obj17.RequestedToDate = datToDate.Value;
				obj17.ShowDialog();
				obj17.BringToFront();
				break;
			}
			case 65:
			{
				PendingApprovalPurchaseInvoice obj18 = new PendingApprovalPurchaseInvoice(UserAccount);
				PendingApprovalPurchaseInvoice.DocumentTypeId = 64;
				obj18.BranchesId = BranchesId;
				if (datFromDate.Checked)
				{
					obj18.RequestedFromDate = datFromDate.Value;
				}
				obj18.RequestedToDate = datToDate.Value;
				obj18.ShowDialog();
				obj18.BringToFront();
				break;
			}
			case 32:
			{
				PendingApprovalSaleInvoice obj19 = new PendingApprovalSaleInvoice(UserAccount);
				PendingApprovalSaleInvoice.DocumentTypeId = 95;
				obj19.BranchesId = BranchesId;
				if (datFromDate.Checked)
				{
					obj19.RequestedFromDate = datFromDate.Value;
				}
				obj19.RequestedToDate = datToDate.Value;
				obj19.ShowDialog();
				obj19.BringToFront();
				break;
			}
			case 33:
			{
				PendingApprovalSaleInvoice obj20 = new PendingApprovalSaleInvoice(UserAccount);
				PendingApprovalSaleInvoice.DocumentTypeId = 99;
				obj20.BranchesId = BranchesId;
				if (datFromDate.Checked)
				{
					obj20.RequestedFromDate = datFromDate.Value;
				}
				obj20.RequestedToDate = datToDate.Value;
				obj20.ShowDialog();
				obj20.BringToFront();
				break;
			}
			case 126:
			{
				PendingApprovalSaleInvoice obj21 = new PendingApprovalSaleInvoice(UserAccount);
				PendingApprovalSaleInvoice.DocumentTypeId = 126;
				obj21.BaseDocumentTypeId = 1;
				obj21.BranchesId = BranchesId;
				if (datFromDate.Checked)
				{
					obj21.RequestedFromDate = datFromDate.Value;
				}
				obj21.RequestedToDate = datToDate.Value;
				obj21.ShowDialog();
				obj21.BringToFront();
				break;
			}
			case 127:
			{
				PendingApprovalSaleInvoice obj22 = new PendingApprovalSaleInvoice(UserAccount);
				PendingApprovalSaleInvoice.DocumentTypeId = 126;
				obj22.BaseDocumentTypeId = 2;
				obj22.BranchesId = BranchesId;
				if (datFromDate.Checked)
				{
					obj22.RequestedFromDate = datFromDate.Value;
				}
				obj22.RequestedToDate = datToDate.Value;
				obj22.ShowDialog();
				obj22.BringToFront();
				break;
			}
			case 145:
			{
				PendingApprovalSaleInvoice obj23 = new PendingApprovalSaleInvoice(UserAccount);
				PendingApprovalSaleInvoice.DocumentTypeId = 145;
				obj23.BaseDocumentTypeId = 1;
				obj23.BranchesId = BranchesId;
				if (datFromDate.Checked)
				{
					obj23.RequestedFromDate = datFromDate.Value;
				}
				obj23.RequestedToDate = datToDate.Value;
				obj23.ShowDialog();
				obj23.BringToFront();
				break;
			}
			case 184:
			{
				PendingApprovalSaleInvoice obj24 = new PendingApprovalSaleInvoice(UserAccount);
				PendingApprovalSaleInvoice.DocumentTypeId = 184;
				if (datFromDate.Checked)
				{
					obj24.RequestedFromDate = datFromDate.Value;
				}
				obj24.RequestedToDate = datToDate.Value;
				obj24.ShowDialog();
				obj24.BringToFront();
				break;
			}
			case 59:
			{
				PendingApprovalSaleInvoice obj25 = new PendingApprovalSaleInvoice(UserAccount);
				PendingApprovalSaleInvoice.DocumentTypeId = 171;
				if (datFromDate.Checked)
				{
					obj25.RequestedFromDate = datFromDate.Value;
				}
				obj25.RequestedToDate = datToDate.Value;
				obj25.ShowDialog();
				obj25.BringToFront();
				break;
			}
			case 185:
			{
				PendingApprovalSaleInvoice obj26 = new PendingApprovalSaleInvoice(UserAccount);
				PendingApprovalSaleInvoice.DocumentTypeId = 1609;
				if (datFromDate.Checked)
				{
					obj26.RequestedFromDate = datFromDate.Value;
				}
				obj26.RequestedToDate = datToDate.Value;
				obj26.ShowDialog();
				obj26.BringToFront();
				break;
			}
			case 186:
			{
				PendingApprovalSaleInvoice obj27 = new PendingApprovalSaleInvoice(UserAccount);
				PendingApprovalSaleInvoice.DocumentTypeId = 1608;
				if (datFromDate.Checked)
				{
					obj27.RequestedFromDate = datFromDate.Value;
				}
				obj27.RequestedToDate = datToDate.Value;
				obj27.ShowDialog();
				obj27.BringToFront();
				break;
			}
			case 187:
			{
				PendingApprovalSaleInvoice obj28 = new PendingApprovalSaleInvoice(UserAccount);
				PendingApprovalSaleInvoice.DocumentTypeId = 1611;
				if (datFromDate.Checked)
				{
					obj28.RequestedFromDate = datFromDate.Value;
				}
				obj28.RequestedToDate = datToDate.Value;
				obj28.ShowDialog();
				obj28.BringToFront();
				break;
			}
			case 1660:
			{
				PendingApprovalSaleInvoice obj29 = new PendingApprovalSaleInvoice(UserAccount);
				PendingApprovalSaleInvoice.DocumentTypeId = 1660;
				if (datFromDate.Checked)
				{
					obj29.RequestedFromDate = datFromDate.Value;
				}
				obj29.RequestedToDate = datToDate.Value;
				obj29.ShowDialog();
				obj29.BringToFront();
				break;
			}
			case 1661:
			{
				PendingApprovalSaleInvoice obj30 = new PendingApprovalSaleInvoice(UserAccount);
				PendingApprovalSaleInvoice.DocumentTypeId = 1661;
				if (datFromDate.Checked)
				{
					obj30.RequestedFromDate = datFromDate.Value;
				}
				obj30.RequestedToDate = datToDate.Value;
				obj30.ShowDialog();
				obj30.BringToFront();
				break;
			}
			case 37:
			{
				DeliveryOrderForApproval obj31 = new DeliveryOrderForApproval(UserAccount);
				obj31.DocumentTypeId = 84;
				obj31.DeliveryOrderTypeId = 37;
				if (datFromDate.Checked)
				{
					obj31.RequestedFromDate = datFromDate.Value;
				}
				obj31.RequestedToDate = datToDate.Value;
				obj31.ShowDialog();
				obj31.BringToFront();
				break;
			}
			case 63:
			{
				DeliveryOrderForApproval obj32 = new DeliveryOrderForApproval(UserAccount);
				obj32.DocumentTypeId = 84;
				obj32.DeliveryOrderTypeId = 63;
				if (datFromDate.Checked)
				{
					obj32.RequestedFromDate = datFromDate.Value;
				}
				obj32.RequestedToDate = datToDate.Value;
				obj32.ShowDialog();
				obj32.BringToFront();
				break;
			}
			case 64:
			{
				DeliveryOrderForApproval obj33 = new DeliveryOrderForApproval(UserAccount);
				obj33.DocumentTypeId = 84;
				obj33.DeliveryOrderTypeId = 64;
				if (datFromDate.Checked)
				{
					obj33.RequestedFromDate = datFromDate.Value;
				}
				obj33.RequestedToDate = datToDate.Value;
				obj33.ShowDialog();
				obj33.BringToFront();
				break;
			}
			case 38:
			{
				DeliveryOrderForApproval obj34 = new DeliveryOrderForApproval(UserAccount);
				obj34.DocumentTypeId = 85;
				obj34.DeliveryOrderTypeId = 38;
				if (datFromDate.Checked)
				{
					obj34.RequestedFromDate = datFromDate.Value;
				}
				obj34.RequestedToDate = datToDate.Value;
				obj34.ShowDialog();
				obj34.BringToFront();
				break;
			}
			case 62:
			{
				PendingJobOrderForApproval pendingJobOrderForApproval = new PendingJobOrderForApproval(UserAccount);
				pendingJobOrderForApproval.ShowDialog();
				pendingJobOrderForApproval.BringToFront();
				break;
			}
			case 112:
			{
				frmProductionOutPutApprovalOrUnApproval obj35 = new frmProductionOutPutApprovalOrUnApproval(UserAccount);
				if (datFromDate.Checked)
				{
					obj35.RequestedFromDate = datFromDate.Value;
				}
				obj35.RequestedToDate = datToDate.Value;
				obj35.ShowDialog();
				obj35.BringToFront();
				break;
			}
			case 39:
			{
				PendingApprovalGatePassOutward obj36 = new PendingApprovalGatePassOutward(UserAccount);
				obj36.ActionId = 2;
				obj36.BranchesId = BranchesId;
				if (datFromDate.Checked)
				{
					obj36.RequestedFromDate = datFromDate.Value;
				}
				obj36.RequestedToDate = datToDate.Value;
				obj36.ShowDialog();
				obj36.BringToFront();
				break;
			}
			case 56:
			{
				PendingApprovalGatePassOutward obj37 = new PendingApprovalGatePassOutward(UserAccount);
				obj37.ActionId = 1;
				obj37.BranchesId = BranchesId;
				if (datFromDate.Checked)
				{
					obj37.RequestedFromDate = datFromDate.Value;
				}
				obj37.RequestedToDate = datToDate.Value;
				obj37.ShowDialog();
				obj37.BringToFront();
				break;
			}
			case 40:
			{
				PendingApprovalGatePassOutwardGeneral obj38 = new PendingApprovalGatePassOutwardGeneral(UserAccount);
				obj38.DocumentTypeId = 92;
				obj38.BranchesId = BranchesId;
				if (datFromDate.Checked)
				{
					obj38.RequestedFromDate = datFromDate.Value;
				}
				obj38.RequestedToDate = datToDate.Value;
				obj38.ShowDialog();
				obj38.BringToFront();
				break;
			}
			case 189:
			{
				PendingApprovalGatePassOutwardGeneral obj39 = new PendingApprovalGatePassOutwardGeneral(UserAccount);
				obj39.DocumentTypeId = 52;
				obj39.BranchesId = BranchesId;
				if (datFromDate.Checked)
				{
					obj39.RequestedFromDate = datFromDate.Value;
				}
				obj39.RequestedToDate = datToDate.Value;
				obj39.ShowDialog();
				obj39.BringToFront();
				break;
			}
			case 57:
			{
				LabPurchaseAnalysisForApproval labPurchaseAnalysisForApproval2 = new LabPurchaseAnalysisForApproval(UserAccount);
				labPurchaseAnalysisForApproval2.BranchesId = BranchesId;
				labPurchaseAnalysisForApproval2.ShowDialog();
				labPurchaseAnalysisForApproval2.BringToFront();
				break;
			}
			case 303:
			{
				LabPurchaseAnalysisForApproval labPurchaseAnalysisForApproval3 = new LabPurchaseAnalysisForApproval(UserAccount);
				labPurchaseAnalysisForApproval3.BranchesId = BranchesId;
				labPurchaseAnalysisForApproval3.RateApprovalGatePurchase = true;
				labPurchaseAnalysisForApproval3.ShowDialog();
				labPurchaseAnalysisForApproval3.BringToFront();
				break;
			}
			case 301:
			{
				LabPurchaseAnalysisForApproval labPurchaseAnalysisForApproval = new LabPurchaseAnalysisForApproval(UserAccount);
				labPurchaseAnalysisForApproval.BranchesId = BranchesId;
				labPurchaseAnalysisForApproval.WeightApprovalPurchaseOrder = true;
				labPurchaseAnalysisForApproval.ShowDialog();
				labPurchaseAnalysisForApproval.BringToFront();
				break;
			}
			case 58:
			{
				DepartmentRequestApproval obj40 = new DepartmentRequestApproval(UserAccount);
				if (datFromDate.Checked)
				{
					obj40.RequestedFromDate = datFromDate.Value;
				}
				obj40.RequestedToDate = datToDate.Value;
				obj40.ShowDialog();
				obj40.BringToFront();
				break;
			}
			case 806:
			{
				PendingApprovalStockTransfer obj41 = new PendingApprovalStockTransfer(UserAccount);
				obj41.DocumentTypeId = 806;
				if (datFromDate.Checked)
				{
					obj41.RequestedFromDate = datFromDate.Value;
				}
				obj41.RequestedToDate = datToDate.Value;
				obj41.ShowDialog();
				obj41.BringToFront();
				break;
			}
			case 68:
			{
				PendingApprovalStockTransfer obj42 = new PendingApprovalStockTransfer(UserAccount);
				obj42.DocumentTypeId = 68;
				if (datFromDate.Checked)
				{
					obj42.RequestedFromDate = datFromDate.Value;
				}
				obj42.RequestedToDate = datToDate.Value;
				obj42.ShowDialog();
				obj42.BringToFront();
				break;
			}
			case 71:
			case 1657:
			{
				DeliveryOrderApproval_Engr obj43 = new DeliveryOrderApproval_Engr(UserAccount);
				obj43.DocumentTypeId = id switch
				{
					1657 => 1657, 
					71 => 1606, 
					_ => 0, 
				};
				obj43.DeliveryOrderTypeId = 37;
				if (datFromDate.Checked)
				{
					obj43.RequestedFromDate = datFromDate.Value;
				}
				obj43.RequestedToDate = datToDate.Value;
				obj43.ShowDialog();
				obj43.BringToFront();
				break;
			}
			case 1200:
			{
				DeliveryOrderApproval_Engr obj44 = new DeliveryOrderApproval_Engr(UserAccount);
				obj44.DocumentTypeId = 1200;
				obj44.DeliveryOrderTypeId = 37;
				if (datFromDate.Checked)
				{
					obj44.RequestedFromDate = datFromDate.Value;
				}
				obj44.RequestedToDate = datToDate.Value;
				obj44.ShowDialog();
				obj44.BringToFront();
				break;
			}
			case 72:
			{
				PendingApprovalPurchaseInvoice obj45 = new PendingApprovalPurchaseInvoice(UserAccount);
				PendingApprovalPurchaseInvoice.DocumentTypeId = 702;
				obj45.BranchesId = BranchesId;
				if (datFromDate.Checked)
				{
					obj45.RequestedFromDate = datFromDate.Value;
				}
				obj45.RequestedToDate = datToDate.Value;
				obj45.ShowDialog();
				obj45.BringToFront();
				break;
			}
			case 245:
			{
				PendingApprovalPurchaseInvoice obj46 = new PendingApprovalPurchaseInvoice(UserAccount);
				PendingApprovalPurchaseInvoice.DocumentTypeId = 245;
				obj46.BranchesId = BranchesId;
				if (datFromDate.Checked)
				{
					obj46.RequestedFromDate = datFromDate.Value;
				}
				obj46.RequestedToDate = datToDate.Value;
				obj46.ShowDialog();
				obj46.BringToFront();
				break;
			}
			case 74:
			{
				StoreIssuanceFinancialOnApproval storeIssuanceFinancialOnApproval = new StoreIssuanceFinancialOnApproval(UserAccount);
				storeIssuanceFinancialOnApproval.ShowDialog();
				storeIssuanceFinancialOnApproval.BringToFront();
				break;
			}
			case 77:
			{
				SaleOrderForApproval_Concrete obj47 = new SaleOrderForApproval_Concrete(UserAccount);
				obj47.DocumentTypeId = 1852;
				if (datFromDate.Checked)
				{
					obj47.RequestedFromDate = datFromDate.Value;
				}
				obj47.RequestedToDate = datToDate.Value;
				obj47.ShowDialog();
				obj47.BringToFront();
				break;
			}
			case 78:
			{
				DeliveryOrderForApproval_Concrete obj48 = new DeliveryOrderForApproval_Concrete(UserAccount);
				obj48.DocumentTypeId = 1853;
				if (datFromDate.Checked)
				{
					obj48.RequestedFromDate = datFromDate.Value;
				}
				obj48.RequestedToDate = datToDate.Value;
				obj48.ShowDialog();
				obj48.BringToFront();
				break;
			}
			case 79:
			{
				SaleInvoiceForApproval_Concrete obj49 = new SaleInvoiceForApproval_Concrete(UserAccount);
				obj49.DocumentTypeId = 1856;
				if (datFromDate.Checked)
				{
					obj49.RequestedFromDate = datFromDate.Value;
				}
				obj49.RequestedToDate = datToDate.Value;
				obj49.ShowDialog();
				obj49.BringToFront();
				break;
			}
			case 80:
			{
				SaleInvoiceForApproval_Concrete obj50 = new SaleInvoiceForApproval_Concrete(UserAccount);
				obj50.DocumentTypeId = 1861;
				if (datFromDate.Checked)
				{
					obj50.RequestedFromDate = datFromDate.Value;
				}
				obj50.RequestedToDate = datToDate.Value;
				obj50.ShowDialog();
				obj50.BringToFront();
				break;
			}
			case 81:
			{
				PurchaseOrderForApproval_Concrete obj51 = new PurchaseOrderForApproval_Concrete(UserAccount);
				obj51.DocumentTypeId = 1863;
				if (datFromDate.Checked)
				{
					obj51.RequestedFromDate = datFromDate.Value;
				}
				obj51.RequestedToDate = datToDate.Value;
				obj51.ShowDialog();
				obj51.BringToFront();
				break;
			}
			case 82:
			{
				PurchaseInvoiceForApproval_Concrete obj52 = new PurchaseInvoiceForApproval_Concrete(UserAccount);
				obj52.DocumentTypeId = 1859;
				if (datFromDate.Checked)
				{
					obj52.RequestedFromDate = datFromDate.Value;
				}
				obj52.RequestedToDate = datToDate.Value;
				obj52.ShowDialog();
				obj52.BringToFront();
				break;
			}
			case 83:
			{
				PurchaseInvoiceForApproval_Concrete obj53 = new PurchaseInvoiceForApproval_Concrete(UserAccount);
				obj53.DocumentTypeId = 1865;
				if (datFromDate.Checked)
				{
					obj53.RequestedFromDate = datFromDate.Value;
				}
				obj53.RequestedToDate = datToDate.Value;
				obj53.ShowDialog();
				obj53.BringToFront();
				break;
			}
			case 84:
			{
				SaleInvoiceForApproval_Concrete obj54 = new SaleInvoiceForApproval_Concrete(UserAccount);
				obj54.DocumentTypeId = 1862;
				if (datFromDate.Checked)
				{
					obj54.RequestedFromDate = datFromDate.Value;
				}
				obj54.RequestedToDate = datToDate.Value;
				obj54.ShowDialog();
				obj54.BringToFront();
				break;
			}
			case 1618:
			{
				PendingGRNForApproval_Engr obj55 = new PendingGRNForApproval_Engr(UserAccount);
				if (datFromDate.Checked)
				{
					obj55.RequestedFromDate = datFromDate.Value;
				}
				obj55.RequestedToDate = datToDate.Value;
				obj55.ShowDialog();
				obj55.BringToFront();
				break;
			}
			case 31:
			{
				PendingGDNForApproval_Engr obj56 = new PendingGDNForApproval_Engr(UserAccount);
				if (datFromDate.Checked)
				{
					obj56.RequestedFromDate = datFromDate.Value;
				}
				obj56.RequestedToDate = datToDate.Value;
				obj56.ShowDialog();
				obj56.BringToFront();
				break;
			}
			case 188:
			{
				PendingStorePurchaseDemandForApproval obj57 = new PendingStorePurchaseDemandForApproval(UserAccount);
				obj57.ParentCategoryId = 8;
				if (datFromDate.Checked)
				{
					obj57.RequestedFromDate = datFromDate.Value;
				}
				obj57.RequestedToDate = datToDate.Value;
				obj57.ShowDialog();
				obj57.BringToFront();
				break;
			}
			case 141:
			{
				PendingStorePurchaseDemandForApproval obj58 = new PendingStorePurchaseDemandForApproval(UserAccount);
				obj58.ParentCategoryId = 7;
				if (datFromDate.Checked)
				{
					obj58.RequestedFromDate = datFromDate.Value;
				}
				obj58.RequestedToDate = datToDate.Value;
				obj58.ShowDialog();
				obj58.BringToFront();
				break;
			}
			case 1100:
			{
				Architecture.WinApp.FeedMill.Approval.PurchaseOrderForApproval purchaseOrderForApproval = new Architecture.WinApp.FeedMill.Approval.PurchaseOrderForApproval(UserAccount);
				purchaseOrderForApproval.BringToFront();
				purchaseOrderForApproval.ShowDialog();
				break;
			}
			case 101:
			{
				PendingApprovalLabourWages obj59 = new PendingApprovalLabourWages(UserAccount);
				obj59.DocumentTypeId = 101;
				if (datFromDate.Checked)
				{
					obj59.RequestedFromDate = datFromDate.Value;
				}
				obj59.RequestedToDate = datToDate.Value;
				obj59.BringToFront();
				obj59.ShowDialog();
				break;
			}
			case 810:
			{
				PendingApprovalLabourWages obj60 = new PendingApprovalLabourWages(UserAccount);
				obj60.DocumentTypeId = 810;
				if (datFromDate.Checked)
				{
					obj60.RequestedFromDate = datFromDate.Value;
				}
				obj60.RequestedToDate = datToDate.Value;
				obj60.BringToFront();
				obj60.ShowDialog();
				break;
			}
			case 910:
			{
				DeliveryOrderForApproval obj61 = new DeliveryOrderForApproval(UserAccount);
				obj61.DocumentTypeId = 910;
				obj61.DeliveryOrderTypeId = 910;
				if (datFromDate.Checked)
				{
					obj61.RequestedFromDate = datFromDate.Value;
				}
				obj61.RequestedToDate = datToDate.Value;
				obj61.ShowDialog();
				obj61.BringToFront();
				break;
			}
			case 242:
			{
				frmExportReturnInvoiceApproval obj62 = new frmExportReturnInvoiceApproval(UserAccount);
				if (datFromDate.Checked)
				{
					obj62.RequestedFromDate = datFromDate.Value;
				}
				obj62.RequestedToDate = datToDate.Value;
				obj62.ShowDialog();
				obj62.BringToFront();
				break;
			}
			case 142:
			{
				PendingFreightVoucherForApproval obj63 = new PendingFreightVoucherForApproval(UserAccount);
				obj63.DocumentTypeId = 28;
				if (datFromDate.Checked)
				{
					obj63.RequestedFromDate = datFromDate.Value;
				}
				obj63.RequestedToDate = datToDate.Value;
				obj63.ShowDialog();
				obj63.BringToFront();
				break;
			}
			case 1870:
			{
				frmWorkOrderApproval obj64 = new frmWorkOrderApproval(UserAccount);
				obj64.DocumentTypeId = 1870;
				if (datFromDate.Checked)
				{
					obj64.RequestedFromDate = datFromDate.Value;
				}
				obj64.RequestedToDate = datToDate.Value;
				obj64.ShowDialog();
				obj64.BringToFront();
				break;
			}
			case 1300:
			{
				frmApprovalLogisticRateNegotiation obj65 = new frmApprovalLogisticRateNegotiation(UserAccount);
				obj65.DocumentTypeId = 1300;
				if (datFromDate.Checked)
				{
					obj65.RequestedFromDate = datFromDate.Value;
				}
				obj65.RequestedToDate = datToDate.Value;
				obj65.ShowDialog();
				obj65.BringToFront();
				break;
			}
			case 1302:
			{
				frmApprovalLogisticRateNegotiation obj66 = new frmApprovalLogisticRateNegotiation(UserAccount);
				obj66.DocumentTypeId = 1302;
				if (datFromDate.Checked)
				{
					obj66.RequestedFromDate = datFromDate.Value;
				}
				obj66.RequestedToDate = datToDate.Value;
				obj66.ShowDialog();
				obj66.BringToFront();
				break;
			}
			case 1301:
			{
				frmApprovalLogisticAgreement obj67 = new frmApprovalLogisticAgreement(UserAccount);
				obj67.DocumentTypeId = 1301;
				if (datFromDate.Checked)
				{
					obj67.RequestedFromDate = datFromDate.Value;
				}
				obj67.RequestedToDate = datToDate.Value;
				obj67.ShowDialog();
				obj67.BringToFront();
				break;
			}
			case 1303:
			{
				frmApprovalLogisticPurchaseOrder obj68 = new frmApprovalLogisticPurchaseOrder(UserAccount);
				obj68.DocumentTypeId = 1303;
				if (datFromDate.Checked)
				{
					obj68.RequestedFromDate = datFromDate.Value;
				}
				obj68.RequestedToDate = datToDate.Value;
				obj68.ShowDialog();
				obj68.BringToFront();
				break;
			}
			case 1304:
			{
				frmApprovalLogisticPurchaseServicesBill obj69 = new frmApprovalLogisticPurchaseServicesBill(UserAccount);
				obj69.DocumentTypeId = 1304;
				if (datFromDate.Checked)
				{
					obj69.RequestedFromDate = datFromDate.Value;
				}
				obj69.RequestedToDate = datToDate.Value;
				obj69.ShowDialog();
				obj69.BringToFront();
				break;
			}
			case 155:
			{
				frmApprovalLogisticPurchaseServicesBill obj70 = new frmApprovalLogisticPurchaseServicesBill(UserAccount);
				obj70.DocumentTypeId = 155;
				if (datFromDate.Checked)
				{
					obj70.RequestedFromDate = datFromDate.Value;
				}
				obj70.RequestedToDate = datToDate.Value;
				obj70.ShowDialog();
				obj70.BringToFront();
				break;
			}
			case 999:
			{
				frmThirdPartyLotInspectionApproval obj71 = new frmThirdPartyLotInspectionApproval(UserAccount);
				if (datFromDate.Checked)
				{
					obj71.RequestedFromDate = datFromDate.Value;
				}
				obj71.RequestedToDate = datToDate.Value;
				obj71.ShowDialog();
				obj71.BringToFront();
				break;
			}
			case 202:
			{
				frmExportSalesContractApproval obj72 = new frmExportSalesContractApproval(UserAccount);
				if (datFromDate.Checked)
				{
					obj72.RequestedFromDate = datFromDate.Value;
				}
				obj72.RequestedToDate = datToDate.Value;
				obj72.ShowDialog();
				obj72.BringToFront();
				break;
			}
			case 1053:
			{
				frmApprovalSaleOrderCmagt obj73 = new frmApprovalSaleOrderCmagt(UserAccount);
				obj73.DocumentTypeId = 1053;
				if (datFromDate.Checked)
				{
					obj73.RequestedFromDate = datFromDate.Value;
				}
				obj73.RequestedToDate = datToDate.Value;
				obj73.ShowDialog();
				obj73.BringToFront();
				break;
			}
			case 1052:
			{
				frmApprovalPurchaseOrderCmagt obj74 = new frmApprovalPurchaseOrderCmagt(UserAccount);
				obj74.DocumentTypeId = 1052;
				if (datFromDate.Checked)
				{
					obj74.RequestedFromDate = datFromDate.Value;
				}
				obj74.RequestedToDate = datToDate.Value;
				obj74.ShowDialog();
				obj74.BringToFront();
				break;
			}
			case 1056:
			{
				frmApprovalCommissionTradeBill obj75 = new frmApprovalCommissionTradeBill(UserAccount);
				obj75.DocumentTypeId = 1056;
				if (datFromDate.Checked)
				{
					obj75.RequestedFromDate = datFromDate.Value;
				}
				obj75.RequestedToDate = datToDate.Value;
				obj75.ShowDialog();
				obj75.BringToFront();
				break;
			}
			}
		}
		DynamicallyGenerateCards();
	}

	private void Refresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "BranchesAllocatedToUser" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			BranchesBind();
			DynamicallyGenerateCards();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ApprovalDashboard_FormClosing(object sender, FormClosingEventArgs e)
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

	private void BtnSpecialApprovalDashBoard_Click(object sender, EventArgs e)
	{
		try
		{
			new Architecture.WinApp.SpecialApprovalDashboard.SpecialApprovalDashboard(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error while closing the form: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnShow_Click(object sender, EventArgs e)
	{
		try
		{
			DynamicallyGenerateCards();
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
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Expected O, but got Unknown
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ApprovalDashboard.ApprovalDashboard));
		this.panel1 = new System.Windows.Forms.Panel();
		this.GroupBoxExport = new System.Windows.Forms.GroupBox();
		this.ExportFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.GroupBoxInventory = new System.Windows.Forms.GroupBox();
		this.inventoryFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.GroupBoxAccounts = new System.Windows.Forms.GroupBox();
		this.accountsFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.FilterPanel = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.label4 = new System.Windows.Forms.Label();
		this.datFromDate = new System.Windows.Forms.DateTimePicker();
		this.label33 = new System.Windows.Forms.Label();
		this.BtnShow = new System.Windows.Forms.Button();
		this.CmbBranch = new UltraCombo();
		this.lblCompany = new System.Windows.Forms.Label();
		this.datToDate = new System.Windows.Forms.DateTimePicker();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.BtnSpecialApprovalDashBoard = new System.Windows.Forms.ToolStripButton();
		this.PanelExportAndCommissionAgent = new System.Windows.Forms.Panel();
		this.GroupBoxCommissionAgent = new System.Windows.Forms.GroupBox();
		this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.GroupBoxExport.SuspendLayout();
		this.panel3.SuspendLayout();
		this.GroupBoxInventory.SuspendLayout();
		this.panel2.SuspendLayout();
		this.GroupBoxAccounts.SuspendLayout();
		this.panel5.SuspendLayout();
		this.FilterPanel.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranch).BeginInit();
		this.toolStrip3.SuspendLayout();
		this.PanelExportAndCommissionAgent.SuspendLayout();
		this.GroupBoxCommissionAgent.SuspendLayout();
		this.panel4.SuspendLayout();
		base.SuspendLayout();
		this.panel1.AutoScroll = true;
		this.panel1.Controls.Add(this.PanelExportAndCommissionAgent);
		this.panel1.Controls.Add(this.GroupBoxInventory);
		this.panel1.Controls.Add(this.GroupBoxAccounts);
		this.panel1.Controls.Add(this.FilterPanel);
		this.panel1.Controls.Add(this.toolStrip3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1434, 611);
		this.panel1.TabIndex = 1;
		this.GroupBoxExport.Controls.Add(this.ExportFlowLayout);
		this.GroupBoxExport.Controls.Add(this.panel3);
		this.GroupBoxExport.Dock = System.Windows.Forms.DockStyle.Top;
		this.GroupBoxExport.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.GroupBoxExport.Location = new System.Drawing.Point(0, 0);
		this.GroupBoxExport.Name = "GroupBoxExport";
		this.GroupBoxExport.Size = new System.Drawing.Size(341, 326);
		this.GroupBoxExport.TabIndex = 5;
		this.GroupBoxExport.TabStop = false;
		this.GroupBoxExport.Text = "Export";
		this.ExportFlowLayout.AutoScroll = true;
		this.ExportFlowLayout.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.ExportFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
		this.ExportFlowLayout.Location = new System.Drawing.Point(3, 51);
		this.ExportFlowLayout.Name = "ExportFlowLayout";
		this.ExportFlowLayout.Size = new System.Drawing.Size(335, 272);
		this.ExportFlowLayout.TabIndex = 0;
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label3);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 21);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(335, 30);
		this.panel3.TabIndex = 2;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(116, 3);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(181, 23);
		this.label3.TabIndex = 0;
		this.label3.Text = "Export Dashboard";
		this.GroupBoxInventory.Controls.Add(this.inventoryFlowLayout);
		this.GroupBoxInventory.Controls.Add(this.panel2);
		this.GroupBoxInventory.Dock = System.Windows.Forms.DockStyle.Left;
		this.GroupBoxInventory.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.GroupBoxInventory.Location = new System.Drawing.Point(547, 68);
		this.GroupBoxInventory.Name = "GroupBoxInventory";
		this.GroupBoxInventory.Size = new System.Drawing.Size(546, 543);
		this.GroupBoxInventory.TabIndex = 1;
		this.GroupBoxInventory.TabStop = false;
		this.GroupBoxInventory.Text = "Inventory";
		this.inventoryFlowLayout.AutoScroll = true;
		this.inventoryFlowLayout.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.inventoryFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
		this.inventoryFlowLayout.Location = new System.Drawing.Point(3, 51);
		this.inventoryFlowLayout.Name = "inventoryFlowLayout";
		this.inventoryFlowLayout.Size = new System.Drawing.Size(540, 489);
		this.inventoryFlowLayout.TabIndex = 0;
		this.panel2.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 21);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(540, 30);
		this.panel2.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(163, 2);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(212, 23);
		this.label2.TabIndex = 0;
		this.label2.Text = "Inventory Dashboard";
		this.GroupBoxAccounts.Controls.Add(this.accountsFlowLayout);
		this.GroupBoxAccounts.Controls.Add(this.panel5);
		this.GroupBoxAccounts.Dock = System.Windows.Forms.DockStyle.Left;
		this.GroupBoxAccounts.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.GroupBoxAccounts.Location = new System.Drawing.Point(0, 68);
		this.GroupBoxAccounts.Name = "GroupBoxAccounts";
		this.GroupBoxAccounts.Size = new System.Drawing.Size(547, 543);
		this.GroupBoxAccounts.TabIndex = 0;
		this.GroupBoxAccounts.TabStop = false;
		this.GroupBoxAccounts.Text = "Accounts";
		this.accountsFlowLayout.AutoScroll = true;
		this.accountsFlowLayout.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.accountsFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
		this.accountsFlowLayout.Location = new System.Drawing.Point(3, 51);
		this.accountsFlowLayout.Name = "accountsFlowLayout";
		this.accountsFlowLayout.Size = new System.Drawing.Size(541, 489);
		this.accountsFlowLayout.TabIndex = 0;
		this.panel5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 21);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(541, 30);
		this.panel5.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(144, 2);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(205, 23);
		this.label1.TabIndex = 0;
		this.label1.Text = "Accounts Dashboard";
		this.FilterPanel.BackColor = System.Drawing.Color.White;
		this.FilterPanel.Controls.Add(this.groupBox3);
		this.FilterPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.FilterPanel.Location = new System.Drawing.Point(0, 27);
		this.FilterPanel.Name = "FilterPanel";
		this.FilterPanel.Size = new System.Drawing.Size(1434, 41);
		this.FilterPanel.TabIndex = 4;
		this.groupBox3.Controls.Add(this.label4);
		this.groupBox3.Controls.Add(this.datFromDate);
		this.groupBox3.Controls.Add(this.label33);
		this.groupBox3.Controls.Add(this.BtnShow);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranch);
		this.groupBox3.Controls.Add(this.lblCompany);
		this.groupBox3.Controls.Add(this.datToDate);
		this.groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(0, 0);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(798, 41);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(210, 17);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(48, 15);
		this.label4.TabIndex = 137;
		this.label4.Text = "To Date";
		this.datFromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.CustomFormat = "dd-MMM-yy";
		this.datFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDate.Location = new System.Drawing.Point(71, 13);
		this.datFromDate.Name = "datFromDate";
		this.datFromDate.ShowCheckBox = true;
		this.datFromDate.Size = new System.Drawing.Size(134, 23);
		this.datFromDate.TabIndex = 134;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(5, 17);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(63, 15);
		this.label33.TabIndex = 136;
		this.label33.Text = "From Date";
		this.BtnShow.BackColor = System.Drawing.Color.Teal;
		this.BtnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShow.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShow.ForeColor = System.Drawing.Color.White;
		this.BtnShow.Location = new System.Drawing.Point(737, 13);
		this.BtnShow.Name = "BtnShow";
		this.BtnShow.Size = new System.Drawing.Size(55, 23);
		this.BtnShow.TabIndex = 1;
		this.BtnShow.Text = "Show";
		this.BtnShow.UseVisualStyleBackColor = false;
		this.BtnShow.Click += new System.EventHandler(BtnShow_Click);
		this.CmbBranch.AllowNull = (DefaultableBoolean)1;
		this.CmbBranch.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbBranch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbBranch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbBranch).Location = new System.Drawing.Point(452, 11);
		((System.Windows.Forms.Control)(object)this.CmbBranch).Name = "CmbBranch";
		((System.Windows.Forms.Control)(object)this.CmbBranch).Size = new System.Drawing.Size(282, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranch).TabIndex = 0;
		this.lblCompany.AutoSize = true;
		this.lblCompany.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCompany.ForeColor = System.Drawing.Color.Black;
		this.lblCompany.Location = new System.Drawing.Point(403, 17);
		this.lblCompany.Name = "lblCompany";
		this.lblCompany.Size = new System.Drawing.Size(46, 15);
		this.lblCompany.TabIndex = 45;
		this.lblCompany.Text = "Branch";
		this.datToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.CustomFormat = "dd-MMM-yy";
		this.datToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDate.Location = new System.Drawing.Point(264, 13);
		this.datToDate.Name = "datToDate";
		this.datToDate.ShowCheckBox = true;
		this.datToDate.Size = new System.Drawing.Size(134, 23);
		this.datToDate.TabIndex = 135;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNew, this.BtnSpecialApprovalDashBoard });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1434, 27);
		this.toolStrip3.TabIndex = 2;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = (System.Drawing.Image)resources.GetObject("btnNew.Image");
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(70, 24);
		this.btnNew.Text = "&Refresh";
		this.btnNew.Click += new System.EventHandler(Refresh_Click);
		this.BtnSpecialApprovalDashBoard.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.BtnSpecialApprovalDashBoard.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.BtnSpecialApprovalDashBoard.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSpecialApprovalDashBoard.Name = "BtnSpecialApprovalDashBoard";
		this.BtnSpecialApprovalDashBoard.Size = new System.Drawing.Size(214, 24);
		this.BtnSpecialApprovalDashBoard.Text = "Special Approval DashBoard";
		this.BtnSpecialApprovalDashBoard.Click += new System.EventHandler(BtnSpecialApprovalDashBoard_Click);
		this.PanelExportAndCommissionAgent.Controls.Add(this.GroupBoxCommissionAgent);
		this.PanelExportAndCommissionAgent.Controls.Add(this.GroupBoxExport);
		this.PanelExportAndCommissionAgent.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanelExportAndCommissionAgent.Location = new System.Drawing.Point(1093, 68);
		this.PanelExportAndCommissionAgent.Name = "PanelExportAndCommissionAgent";
		this.PanelExportAndCommissionAgent.Size = new System.Drawing.Size(341, 543);
		this.PanelExportAndCommissionAgent.TabIndex = 6;
		this.GroupBoxCommissionAgent.Controls.Add(this.flowLayoutPanel1);
		this.GroupBoxCommissionAgent.Controls.Add(this.panel4);
		this.GroupBoxCommissionAgent.Dock = System.Windows.Forms.DockStyle.Fill;
		this.GroupBoxCommissionAgent.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.GroupBoxCommissionAgent.Location = new System.Drawing.Point(0, 326);
		this.GroupBoxCommissionAgent.Name = "GroupBoxCommissionAgent";
		this.GroupBoxCommissionAgent.Size = new System.Drawing.Size(341, 217);
		this.GroupBoxCommissionAgent.TabIndex = 6;
		this.GroupBoxCommissionAgent.TabStop = false;
		this.GroupBoxCommissionAgent.Text = "Commission Agent";
		this.flowLayoutPanel1.AutoScroll = true;
		this.flowLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 51);
		this.flowLayoutPanel1.Name = "flowLayoutPanel1";
		this.flowLayoutPanel1.Size = new System.Drawing.Size(335, 163);
		this.flowLayoutPanel1.TabIndex = 0;
		this.panel4.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 21);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(335, 30);
		this.panel4.TabIndex = 2;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(116, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(291, 23);
		this.label5.TabIndex = 0;
		this.label5.Text = "CommissionAgent Dashboard";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1434, 611);
		base.Controls.Add(this.panel1);
		base.Name = "ApprovalDashboard";
		this.Text = "Approval Dashboard";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ApprovalDashboard_FormClosing);
		base.Load += new System.EventHandler(ApprovalDashboard_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.GroupBoxExport.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.GroupBoxInventory.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.GroupBoxAccounts.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.FilterPanel.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranch).EndInit();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.PanelExportAndCommissionAgent.ResumeLayout(false);
		this.GroupBoxCommissionAgent.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
