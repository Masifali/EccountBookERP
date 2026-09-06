// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Production;
using Architecture.BLL.Reports.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Production;
using Architecture.WinApp.Properties;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
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

public class PendingJobOrderForApproval : Form
{
	private DataTable dtgrid = new DataTable();

	private DataTable dtgrdPendingforView = new DataTable();

	public bool UnApprove;

	public int SpecialApprovalId;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel PanelMain;

	private Panel panel1;

	private Label label1;

	private Panel panel3;

	private Panel panel2;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel4;

	private Panel panel6;

	private Panel panel5;

	private TabPage tabPage2;

	private Panel panel7;

	private Panel panel8;

	private ToolStrip toolStrip1;

	private ToolStripButton BtnNew;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton BtnApprove;

	internal GridEX grdForApproval;

	internal GridEX grdPendingforView;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStripButton bntPendingforView;

	private ToolStripButton BtnShortcutkeys;

	public PendingJobOrderForApproval(UserAccount User)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = User;
	}

	private void PendingJobOrderForApproval_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 680);
			if (UnApprove)
			{
				tabControl1.TabPages.Remove(tabPage2);
				bntPendingforView.Visible = false;
				BtnApprove.Text = "UnApprove";
				BtnApprove.Image = Resources.close_line;
			}
			dtgrid.Columns.Add("Id");
			dtgrid.Columns.Add("JobOrderNo");
			dtgrid.Columns.Add("JobStatus");
			dtgrid.Columns.Add("IssueQty", typeof(double));
			dtgrid.Columns.Add("IssueWeight", typeof(double));
			dtgrid.Columns.Add("IssueAmount", typeof(double));
			dtgrid.Columns.Add("ByProductQty", typeof(double));
			dtgrid.Columns.Add("ByProductWeight", typeof(double));
			dtgrid.Columns.Add("ByProductAmount", typeof(double));
			dtgrid.Columns.Add("FinishGoodQty", typeof(double));
			dtgrid.Columns.Add("FinishGoodWeight", typeof(double));
			dtgrid.Columns.Add("FinishGoodAmount", typeof(double));
			dtgrid.Columns.Add("BalQty", typeof(double));
			dtgrid.Columns.Add("BalWeight", typeof(double));
			dtgrid.Columns.Add("BalAmount", typeof(double));
			dtgrid.Columns.Add("DifferenceAmount", typeof(double));
			grdForApproval.DataSource = dtgrid;
			grdForApproval.RetrieveStructure();
			GrdApprovalsettings();
			dtgrdPendingforView.Columns.Add("Id");
			dtgrdPendingforView.Columns.Add("JobOrderNo");
			dtgrdPendingforView.Columns.Add("JobStatus");
			dtgrdPendingforView.Columns.Add("IssueQty", typeof(double));
			dtgrdPendingforView.Columns.Add("IssueWeight", typeof(double));
			dtgrdPendingforView.Columns.Add("IssueAmount", typeof(double));
			dtgrdPendingforView.Columns.Add("ByProductQty", typeof(double));
			dtgrdPendingforView.Columns.Add("ByProductWeight", typeof(double));
			dtgrdPendingforView.Columns.Add("ByProductAmount", typeof(double));
			dtgrdPendingforView.Columns.Add("FinishGoodQty", typeof(double));
			dtgrdPendingforView.Columns.Add("FinishGoodWeight", typeof(double));
			dtgrdPendingforView.Columns.Add("FinishGoodAmount", typeof(double));
			dtgrdPendingforView.Columns.Add("BalQty", typeof(double));
			dtgrdPendingforView.Columns.Add("BalWeight", typeof(double));
			dtgrdPendingforView.Columns.Add("BalAmount", typeof(double));
			dtgrdPendingforView.Columns.Add("DifferenceAmount", typeof(double));
			grdPendingforView.DataSource = dtgrdPendingforView;
			grdPendingforView.RetrieveStructure();
			grdPendingforViewsettings();
			DetailgridFill();
			grdPendingforViewFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 0)
			{
				if (grdForApproval.RowCount == 0)
				{
					DetailgridFill();
				}
			}
			else if (grdPendingforView.RowCount == 0)
			{
				grdPendingforViewFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailgridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		try
		{
			DataTable dt = InvProductionJobOrder.GetPendingJobOrderForApproval(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				PendingForView = 0,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				SpecialApprovalDocs = SpecialApprovalId,
				IsApproved = UnApprove
			});
			if (dt.Rows.Count > 0)
			{
				dtgrid.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtgrid.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["JobOrderNo"], dt.Rows[i]["JobOrderStatus"], Conversion.ToDouble(dt.Rows[i]["IssueQty"]), Conversion.ToDouble(dt.Rows[i]["IssueWeight"]), Conversion.ToDouble(dt.Rows[i]["IssueAmount"]), Conversion.ToDouble(dt.Rows[i]["ByProductQty"]), Conversion.ToDouble(dt.Rows[i]["ByProductWeight"]), Conversion.ToDouble(dt.Rows[i]["ByProductAmount"]), Conversion.ToDouble(dt.Rows[i]["FinishQty"]), Conversion.ToDouble(dt.Rows[i]["FinishWeight"]), Conversion.ToDouble(dt.Rows[i]["FinishAmount"]), Conversion.ToDouble(dt.Rows[i]["BalQty"]), Conversion.ToDouble(dt.Rows[i]["BalWeight"]), Conversion.ToDouble(dt.Rows[i]["BalAmount"]), Conversion.ToDouble(dt.Rows[i]["DifferenceAmount"]));
				}
				grdForApproval.DataSource = dtgrid;
				grdForApproval.RetrieveStructure();
				GrdApprovalsettings();
			}
			else
			{
				grdForApproval.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdApprovalsettings()
	{
		//IL_0506: Unknown result type (might be due to invalid IL or missing references)
		//IL_050c: Expected O, but got Unknown
		//IL_052f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Expected O, but got Unknown
		//IL_0591: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Expected O, but got Unknown
		try
		{
			grdForApproval.RootTable.Columns["Id"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdForApproval, 2, 3);
			grdForApproval.RootTable.Columns["JobOrderNo"].Width = 150;
			grdForApproval.RootTable.Columns["JobStatus"].Width = Constants.InventoryConstants.StatusConstant;
			grdForApproval.RootTable.Columns["IssueQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdForApproval.RootTable.Columns["IssueWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdForApproval.RootTable.Columns["IssueAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdForApproval.RootTable.Columns["ByProductQty"].Width = Constants.InventoryConstants.QtyConstant + 10;
			grdForApproval.RootTable.Columns["ByProductWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdForApproval.RootTable.Columns["ByProductAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdForApproval.RootTable.Columns["FinishGoodQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdForApproval.RootTable.Columns["FinishGoodWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdForApproval.RootTable.Columns["FinishGoodAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdForApproval.RootTable.Columns["BalQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdForApproval.RootTable.Columns["BalWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdForApproval.RootTable.Columns["BalAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdForApproval.RootTable.Columns["DifferenceAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdForApproval.RootTable.Columns.Add("Select");
			grdForApproval.RootTable.Columns["Select"].ActAsSelector = true;
			grdForApproval.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdForApproval.RootTable.Columns["Select"].Width = 40;
			grdForApproval.RootTable.Columns["Select"].Position = 0;
			grdForApproval.RootTable.Columns.Add("Print");
			grdForApproval.RootTable.Columns["Print"].Caption = "Print";
			grdForApproval.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdForApproval.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdForApproval.RootTable.Columns["Print"].ButtonText = "Print";
			grdForApproval.RootTable.Columns["Print"].Width = 40;
			grdForApproval.RootTable.Columns["Print"].Position = 1;
			grdForApproval.RootTable.Columns.Add("Approve");
			grdForApproval.RootTable.Columns["Approve"].Caption = (UnApprove ? "UnApprove" : "Approve");
			grdForApproval.RootTable.Columns["Approve"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdForApproval.RootTable.Columns["Approve"].ButtonStyle = (ButtonStyle)5;
			grdForApproval.RootTable.Columns["Approve"].ButtonText = (UnApprove ? "UnApprove" : "Approve");
			grdForApproval.RootTable.Columns["Approve"].Width = (UnApprove ? 75 : 60);
			grdForApproval.RootTable.Columns["Approve"].Position = 2;
			GridEX_Helper.AddButton(grdForApproval, "Settlement", "Settlement", 75, 3);
			grdForApproval.FrozenColumns = 4;
			GridEXColumn column = new GridEXColumn();
			column = grdForApproval.RootTable.Columns["BalWeight"];
			GridEXFormatCondition fc = new GridEXFormatCondition(column, (ConditionOperator)2, (object)0);
			fc.TargetColumn = column;
			fc.FormatStyle.FontBold = (TriState)2;
			fc.FormatStyle.ForeColor = Color.Red;
			grdForApproval.RootTable.FormatConditions.Add(fc);
			column = grdForApproval.RootTable.Columns["BalWeight"];
			fc = new GridEXFormatCondition(column, (ConditionOperator)3, (object)0);
			fc.TargetColumn = column;
			fc.FormatStyle.FontBold = (TriState)2;
			fc.FormatStyle.ForeColor = Color.Green;
			grdForApproval.RootTable.FormatConditions.Add(fc);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdForApproval_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdForApproval.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Approve")
				{
					ApprovedRowRecord(CurrId);
				}
				else if (e.Column.Key == "Print")
				{
					GeneratePrint(CurrId);
				}
				else if (e.Column.Key == "Settlement")
				{
					frmProductionSettlement frmProductionSettlement = new frmProductionSettlement(UserAccount);
					frmProductionSettlement.RequestedByOtherDocument = true;
					frmProductionSettlement.JobOrderId = CurrId;
					frmProductionSettlement.BringToFront();
					frmProductionSettlement.ShowDialog();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingforViewFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		try
		{
			DataTable dt = InvProductionJobOrder.GetPendingJobOrderForApproval(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				PendingForView = 1,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				SpecialApprovalDocs = SpecialApprovalId,
				IsApproved = false
			});
			if (dt.Rows.Count > 0)
			{
				dtgrdPendingforView.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtgrdPendingforView.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["JobOrderNo"], dt.Rows[i]["JobOrderStatus"], Conversion.ToDouble(dt.Rows[i]["IssueQty"]), Conversion.ToDouble(dt.Rows[i]["IssueWeight"]), Conversion.ToDouble(dt.Rows[i]["IssueAmount"]), Conversion.ToDouble(dt.Rows[i]["ByProductQty"]), Conversion.ToDouble(dt.Rows[i]["ByProductWeight"]), Conversion.ToDouble(dt.Rows[i]["ByProductAmount"]), Conversion.ToDouble(dt.Rows[i]["FinishQty"]), Conversion.ToDouble(dt.Rows[i]["FinishWeight"]), Conversion.ToDouble(dt.Rows[i]["FinishAmount"]), Conversion.ToDouble(dt.Rows[i]["BalQty"]), Conversion.ToDouble(dt.Rows[i]["BalWeight"]), Conversion.ToDouble(dt.Rows[i]["BalAmount"]));
				}
				grdPendingforView.DataSource = dtgrdPendingforView;
				grdPendingforView.RetrieveStructure();
				grdPendingforViewsettings();
			}
			else
			{
				grdPendingforView.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingforViewsettings()
	{
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Expected O, but got Unknown
		//IL_04e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Expected O, but got Unknown
		//IL_054b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Expected O, but got Unknown
		try
		{
			grdPendingforView.RootTable.Columns["Id"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdPendingforView, 2, 3);
			grdPendingforView.RootTable.Columns["JobStatus"].Width = Constants.InventoryConstants.StatusConstant;
			grdPendingforView.RootTable.Columns["JobOrderNo"].Width = Constants.InventoryConstants.JobLotConstant;
			grdPendingforView.RootTable.Columns["IssueQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdPendingforView.RootTable.Columns["IssueWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdPendingforView.RootTable.Columns["IssueAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdPendingforView.RootTable.Columns["ByProductQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdPendingforView.RootTable.Columns["ByProductWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdPendingforView.RootTable.Columns["ByProductAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdPendingforView.RootTable.Columns["FinishGoodQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdPendingforView.RootTable.Columns["FinishGoodWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdPendingforView.RootTable.Columns["FinishGoodAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdPendingforView.RootTable.Columns["BalQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdPendingforView.RootTable.Columns["BalWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdPendingforView.RootTable.Columns["BalAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdPendingforView.RootTable.Columns["DifferenceAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdPendingforView.RootTable.Columns.Add("Select");
			grdPendingforView.RootTable.Columns["Select"].ActAsSelector = true;
			grdPendingforView.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdPendingforView.RootTable.Columns["Select"].Width = 40;
			grdPendingforView.RootTable.Columns["Select"].Position = 0;
			grdPendingforView.RootTable.Columns.Add("Print");
			grdPendingforView.RootTable.Columns["Print"].Caption = "Print";
			grdPendingforView.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPendingforView.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdPendingforView.RootTable.Columns["Print"].ButtonText = "Print";
			grdPendingforView.RootTable.Columns["Print"].Width = 50;
			grdPendingforView.RootTable.Columns["Print"].Position = 1;
			grdPendingforView.RootTable.Columns.Add("Approve");
			grdPendingforView.RootTable.Columns["Approve"].Caption = "Approve";
			grdPendingforView.RootTable.Columns["Approve"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPendingforView.RootTable.Columns["Approve"].ButtonStyle = (ButtonStyle)5;
			grdPendingforView.RootTable.Columns["Approve"].ButtonText = "Approve";
			grdPendingforView.RootTable.Columns["Approve"].Width = 60;
			grdPendingforView.RootTable.Columns["Approve"].Position = 2;
			grdPendingforView.FrozenColumns = 3;
			GridEXColumn column = new GridEXColumn();
			column = grdPendingforView.RootTable.Columns["BalWeight"];
			GridEXFormatCondition fc = new GridEXFormatCondition(column, (ConditionOperator)2, (object)0);
			fc.TargetColumn = column;
			fc.FormatStyle.FontBold = (TriState)2;
			fc.FormatStyle.ForeColor = Color.Red;
			grdPendingforView.RootTable.FormatConditions.Add(fc);
			column = grdPendingforView.RootTable.Columns["BalWeight"];
			fc = new GridEXFormatCondition(column, (ConditionOperator)3, (object)0);
			fc.TargetColumn = column;
			fc.FormatStyle.FontBold = (TriState)2;
			fc.FormatStyle.ForeColor = Color.Green;
			grdPendingforView.RootTable.FormatConditions.Add(fc);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingforView_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPendingforView.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Approve")
				{
					ApprovedRowRecord(CurrId);
				}
				if (e.Column.Key == "Print")
				{
					GeneratePrint(CurrId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GeneratePrint(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvFoodProductionReports.InvFoodProduction_Summery3_Rpt(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				JobOrderId = PrintId
			});
			if (dt.Rows.Count > 0 && dt != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "615-InvRptProductionSummery.rpt");
			}
			else
			{
				MessageBox.Show("Record Not Found For DisPlay");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdForApproval).Name);
			if (fs != null)
			{
				grdForApproval.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdPendingforView).Name);
			if (fs != null)
			{
				grdPendingforView.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			DetailgridFill();
			grdPendingforViewFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton2_Click(object sender, EventArgs e)
	{
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		try
		{
			if (tabControl1.SelectedIndex == 0)
			{
				if (grdForApproval.GetCheckedRows().Count() == 0)
				{
					throw new Exception("Please select check box first");
				}
				string action = (UnApprove ? "UnApprove" : "Approve");
				if (MessageBox.Show("Are you sure to " + action + " Voucher?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					ApprovalList obj = new ApprovalList();
					obj.AllApprovalLists = new List<ApprovalList>();
					GridEXRow[] checkedRows = grdForApproval.GetCheckedRows();
					foreach (GridEXRow item in checkedRows)
					{
						ApprovalList detail = new ApprovalList();
						detail.OrganizationId = UserAccount.OrganizationId;
						detail.CompanyId = UserAccount.CompanyId;
						detail.Id = Conversion.ToInt(item.Cells["Id"].Value);
						detail.EntryUser = UserAccount.ID;
						detail.ReqType = (UnApprove ? "Un_Approve" : "AP");
						detail.ActionTypeId = false;
						obj.AllApprovalLists.Add(detail);
					}
					if (SpecialApprovalId == 0)
					{
						InvProductionJobOrder.JobOrderApproveList(obj);
					}
					else
					{
						InvProductionJobOrder.JobOrderSpecialApprovalList(obj);
					}
					MessageBox.Show("Record " + action + " Successfully");
					DetailgridFill();
				}
				return;
			}
			if (grdPendingforView.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (MessageBox.Show("Are you sure to Approve Voucher?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList obj2 = new ApprovalList();
				obj2.AllApprovalLists = new List<ApprovalList>();
				GridEXRow[] checkedRows = grdPendingforView.GetCheckedRows();
				foreach (GridEXRow item2 in checkedRows)
				{
					ApprovalList detail2 = new ApprovalList();
					detail2.OrganizationId = UserAccount.OrganizationId;
					detail2.CompanyId = UserAccount.CompanyId;
					detail2.Id = Conversion.ToInt(item2.Cells["Id"].Value);
					detail2.EntryUser = UserAccount.ID;
					detail2.ReqType = "AP";
					detail2.ActionTypeId = false;
					obj2.AllApprovalLists.Add(detail2);
				}
				if (SpecialApprovalId == 0)
				{
					InvProductionJobOrder.JobOrderApproveList(obj2);
				}
				else
				{
					InvProductionJobOrder.JobOrderSpecialApprovalList(obj2);
				}
				MessageBox.Show("Record Approved Successfully");
				grdPendingforViewFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ApprovedRowRecord(int CurrId)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		try
		{
			string action = (UnApprove ? "UnApprove" : "Approve");
			if (MessageBox.Show("Are you sure to " + action + " Settlement?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				ApprovalList obj = new ApprovalList();
				obj.AllApprovalLists = new List<ApprovalList>();
				ApprovalList detail = new ApprovalList();
				detail.OrganizationId = UserAccount.OrganizationId;
				detail.CompanyId = UserAccount.CompanyId;
				detail.Id = CurrId;
				detail.EntryUser = UserAccount.ID;
				detail.ReqType = (UnApprove ? "Un_Approve" : "AP");
				detail.ActionTypeId = false;
				obj.AllApprovalLists.Add(detail);
				if (SpecialApprovalId == 0)
				{
					InvProductionJobOrder.JobOrderApproveList(obj);
				}
				else
				{
					InvProductionJobOrder.JobOrderSpecialApprovalList(obj);
				}
				MessageBox.Show("Record " + action + " Successfully");
				DetailgridFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bntPendingforView_Click(object sender, EventArgs e)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		try
		{
			if (grdForApproval.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (MessageBox.Show("Are you sure to move record to pending for view?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList obj = new ApprovalList();
				obj.AllApprovalLists = new List<ApprovalList>();
				GridEXRow[] checkedRows = grdForApproval.GetCheckedRows();
				foreach (GridEXRow item in checkedRows)
				{
					ApprovalList detail = new ApprovalList();
					detail.OrganizationId = UserAccount.OrganizationId;
					detail.CompanyId = UserAccount.CompanyId;
					detail.Id = Conversion.ToInt(item.Cells["Id"].Value);
					detail.EntryUser = UserAccount.ID;
					detail.ReqType = "AP";
					detail.ActionTypeId = true;
					obj.AllApprovalLists.Add(detail);
				}
				if (SpecialApprovalId == 0)
				{
					InvProductionJobOrder.JobOrderApproveList(obj);
				}
				else
				{
					InvProductionJobOrder.JobOrderSpecialApprovalList(obj);
				}
				MessageBox.Show("Record moved Successfully");
				DetailgridFill();
				grdPendingforViewFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingforView_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPendingforView.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Space && grdPendingforView.CurrentColumn != null)
			{
				if (grdPendingforView.CurrentColumn.Key == "Approve")
				{
					ApprovedRowRecord(CurrId);
				}
				if (grdPendingforView.CurrentColumn.Key == "Print")
				{
					GeneratePrint(CurrId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdForApproval_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdForApproval.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Space && grdForApproval.CurrentColumn != null)
			{
				if (grdForApproval.CurrentColumn.Key == "Approve")
				{
					ApprovedRowRecord(CurrId);
				}
				if (grdForApproval.CurrentColumn.Key == "Print")
				{
					GeneratePrint(CurrId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveryOrderForApproval_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.KeyCode == Keys.N)
			{
				toolStripButton1_Click(null, null);
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 82;
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				toolStripButton1_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdForApproval).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				((Control)(object)grdForApproval).Focus();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				((Control)(object)grdForApproval).Focus();
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
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Date");
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

	private void BtnShortcutkeys_Click(object sender, EventArgs e)
	{
		try
		{
			MakeShortCutKeys();
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
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_0624: Unknown result type (might be due to invalid IL or missing references)
		//IL_062e: Expected O, but got Unknown
		//IL_0956: Unknown result type (might be due to invalid IL or missing references)
		//IL_0960: Expected O, but got Unknown
		GridEXLayout grdForApproval_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPendingforView_DesignTimeLayout = new GridEXLayout();
		this.PanelMain = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdForApproval = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdPendingforView = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.BtnNew = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnApprove = new System.Windows.Forms.ToolStripButton();
		this.bntPendingforView = new System.Windows.Forms.ToolStripButton();
		this.BtnShortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.PanelMain.SuspendLayout();
		this.panel2.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdForApproval).BeginInit();
		this.panel5.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPendingforView).BeginInit();
		this.panel8.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.PanelMain.BackColor = System.Drawing.Color.White;
		this.PanelMain.Controls.Add(this.panel2);
		this.PanelMain.Controls.Add(this.panel1);
		this.PanelMain.Controls.Add(this.panel3);
		this.PanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanelMain.Location = new System.Drawing.Point(0, 0);
		this.PanelMain.Name = "PanelMain";
		this.PanelMain.Size = new System.Drawing.Size(800, 450);
		this.PanelMain.TabIndex = 0;
		this.panel2.Controls.Add(this.tabControl1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 58);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(800, 392);
		this.panel2.TabIndex = 5;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(800, 392);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabPage1.Location = new System.Drawing.Point(4, 25);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(792, 363);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Detail";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(3, 3);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(786, 357);
		this.panel4.TabIndex = 0;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdForApproval);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 24);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(786, 333);
		this.panel6.TabIndex = 1;
		this.grdForApproval.AllowEdit = (InheritableBoolean)2;
		grdForApproval_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdForApproval.DesignTimeLayout = grdForApproval_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdForApproval).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdForApproval.DynamicFiltering = true;
		this.grdForApproval.FilterMode = (FilterMode)2;
		this.grdForApproval.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdForApproval).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdForApproval.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdForApproval).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdForApproval).Name = "grdForApproval";
		((ScrollableBase)this.grdForApproval).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdForApproval).Size = new System.Drawing.Size(786, 333);
		((System.Windows.Forms.Control)(object)this.grdForApproval).TabIndex = 49;
		((System.Windows.Forms.Control)(object)this.grdForApproval).TabStop = false;
		this.grdForApproval.TotalRow = (InheritableBoolean)1;
		this.grdForApproval.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdForApproval).VisualStyle = (VisualStyle)4;
		this.grdForApproval.ColumnButtonClick += new ColumnActionEventHandler(grdForApproval_ColumnButtonClick);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(786, 24);
		this.panel5.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(746, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grdForApproval;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 9;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 25);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(792, 363);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Pending for View";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdPendingforView);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(3, 28);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(786, 332);
		this.panel7.TabIndex = 3;
		this.grdPendingforView.AllowEdit = (InheritableBoolean)2;
		grdPendingforView_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPendingforView.DesignTimeLayout = grdPendingforView_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPendingforView).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPendingforView.DynamicFiltering = true;
		this.grdPendingforView.FilterMode = (FilterMode)2;
		this.grdPendingforView.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPendingforView).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPendingforView.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdPendingforView).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPendingforView).Name = "grdPendingforView";
		((ScrollableBase)this.grdPendingforView).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPendingforView).Size = new System.Drawing.Size(786, 332);
		((System.Windows.Forms.Control)(object)this.grdPendingforView).TabIndex = 49;
		((System.Windows.Forms.Control)(object)this.grdPendingforView).TabStop = false;
		this.grdPendingforView.TotalRow = (InheritableBoolean)1;
		this.grdPendingforView.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPendingforView).VisualStyle = (VisualStyle)4;
		this.grdPendingforView.ColumnButtonClick += new ColumnActionEventHandler(grdPendingforView_ColumnButtonClick);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.ctrlGrdBar2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(786, 25);
		this.panel8.TabIndex = 2;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(739, -1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.grdPendingforView;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 25);
		this.ctrlGrdBar2.TabIndex = 10;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.label1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 26);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(800, 32);
		this.panel1.TabIndex = 4;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(6, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(189, 19);
		this.label1.TabIndex = 0;
		this.label1.Text = "Production Job Orders";
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(800, 26);
		this.panel3.TabIndex = 3;
		this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.BtnNew, this.toolStripSeparator1, this.BtnApprove, this.bntPendingforView, this.BtnShortcutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(800, 26);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.BtnNew.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNew.Name = "BtnNew";
		this.BtnNew.Size = new System.Drawing.Size(55, 23);
		this.BtnNew.Text = "New";
		this.BtnNew.Click += new System.EventHandler(toolStripButton1_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 26);
		this.BtnApprove.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnApprove.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.BtnApprove.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnApprove.Name = "BtnApprove";
		this.BtnApprove.Size = new System.Drawing.Size(88, 23);
		this.BtnApprove.Text = "Approved";
		this.BtnApprove.Click += new System.EventHandler(toolStripButton2_Click);
		this.bntPendingforView.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.bntPendingforView.Image = Architecture.WinApp.Properties.Resources.eye_fill;
		this.bntPendingforView.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.bntPendingforView.Name = "bntPendingforView";
		this.bntPendingforView.Size = new System.Drawing.Size(126, 23);
		this.bntPendingforView.Text = "PendingForView";
		this.bntPendingforView.Click += new System.EventHandler(bntPendingforView_Click);
		this.BtnShortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortcutkeys.Name = "BtnShortcutkeys";
		this.BtnShortcutkeys.Size = new System.Drawing.Size(122, 23);
		this.BtnShortcutkeys.Text = "ShortCut Keys";
		this.BtnShortcutkeys.Click += new System.EventHandler(BtnShortcutkeys_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(800, 450);
		base.Controls.Add(this.PanelMain);
		base.Name = "PendingJobOrderForApproval";
		this.Text = "PendingJobOrderForApproval";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(PendingJobOrderForApproval_Load);
		this.PanelMain.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdForApproval).EndInit();
		this.panel5.ResumeLayout(false);
		this.tabPage2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPendingforView).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
