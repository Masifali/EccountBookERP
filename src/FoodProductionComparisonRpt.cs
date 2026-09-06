// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Production;
using Architecture.BLL.Reports.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Common;
using Architecture.Model.Production;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
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
namespace Architecture.WinApp.Production_Reports
{

public class FoodProductionComparisonRpt : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtReg = new DataTable();

	private DataTable dtInput = new DataTable();

	private DataTable dtGainLoss = new DataTable();

	private DataTable dtBranch = new DataTable();

	private bool DoHaveCanRateandAmount;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel6;

	private Panel panel5;

	internal GridEX DataGridHistory;

	private Panel panel4;

	internal UltraCombo cmbPlantComparison;

	private Label label1;

	internal UltraCombo cmbProductionTypeComparison;

	private Label label4;

	private Button btnshowComparison;

	private TextBox GpsNoToComparsion;

	private TextBox GpsNoFromComparison;

	private Label label3;

	private Panel panel3;

	private Label label23;

	private Panel panel2;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton toolStripButton1;

	private TabPage tabPage2;

	private Panel panel7;

	private Panel panel10;

	internal GridEX grdInput;

	private Panel panel9;

	internal UltraCombo cmbJobOrderSummary;

	private Label label5;

	private Panel panel8;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNewSummary;

	private ToolStripButton btnPrint607Register;

	private Button btnShowSummary;

	private Label label6;

	private DateTimePicker datToDateSummary;

	private DateTimePicker datFromDateSummary;

	private RadioButton RdGainLoss;

	private RadioButton RdInputDetail;

	private Label lblGainLossKg;

	private Label lblProdTotalWeight;

	private Label lblGainLossStatus;

	private Label lblPercent;

	internal UltraCombo CmbPackUomSummary;

	private Label label9;

	internal UltraCombo CmbItemNameSummary;

	private Label label8;

	private ToolStripDropDownButton tsDropDownGainLoss;

	internal UltraCombo CmbPlantSummary;

	private Label label10;

	private Panel panel11;

	private Label label11;

	internal UltraCombo cmbBranchNameComparison;

	private Label label2;

	private Label label12;

	private Panel PanelValues;

	private Panel panel52;

	private TextBox lblSumofTotalIssue;

	private TextBox lblGainLossDiff;

	private Label lblTotalIssueWeight;

	private TextBox lblSumOfOutPut;

	private Label label17;

	internal UltraCombo CmbBranchNameSummary;

	private Label label7;

	internal UltraCombo CmbJobOrderNoComprison;

	private Label label13;

	private ToolStripButton BtnRefreshSummary;

	private ToolStripButton BtnRefreshComparison;

	public FoodProductionComparisonRpt(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmGPOutward_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1100, 600);
			Rightsobjects obj = CommonServices.SetRightsValueInRightsObject(base.Name);
			if (obj != null)
			{
				DoHaveCanRateandAmount = obj.DoHaveCanRateandAmount;
			}
			BranchesFill();
			ComboBindComparison();
			ProductionTypeBind();
			((UltraGridBase)cmbProductionTypeComparison).Rows[1].Activate();
			ReportsLoad();
			ComboBindSummary();
			RdInputDetail.Checked = true;
			datFromDateSummary.Value = clsGlobalVariables.ActiveYr.Start_Period;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchesFill()
	{
		try
		{
			dtBranch = InvFoodProduction.GetBranchsAllocatedToUserFromProduction(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, 0, "");
			if (dtBranch.Rows.Count > 0)
			{
				DDL.BindDDL(dtBranch, cmbBranchNameComparison, "BranchId", "BranchName", "Branch Name", ZeroIndex: false);
				DDL.BindDDL(dtBranch, CmbBranchNameSummary, "BranchId", "BranchName", "Branch Name", ZeroIndex: false);
				UltraGridColumn obj = ((UltraGridBase)cmbBranchNameComparison).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj).Key = "Selected";
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
				cmbBranchNameComparison.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)cmbBranchNameComparison.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)cmbBranchNameComparison.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)cmbBranchNameComparison.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)cmbBranchNameComparison).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((Control)(object)cmbBranchNameComparison).Text = UserAccount.BranchName.ToString();
				UltraGridColumn obj2 = ((UltraGridBase)CmbBranchNameSummary).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj2).Key = "Selected";
				((HeaderBase)obj2.Header).Caption = string.Empty;
				obj2.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj2.DataType = typeof(bool);
				((HeaderBase)obj2.Header).VisiblePosition = 0;
				CmbBranchNameSummary.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)CmbBranchNameSummary.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)CmbBranchNameSummary.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)CmbBranchNameSummary.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)CmbBranchNameSummary).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((Control)(object)CmbBranchNameSummary).Text = UserAccount.BranchName.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProductionTypeBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Expected O, but got Unknown
		try
		{
			DataTable dtprot = ProductionType.GetAll(new ProductionType());
			if (dtprot.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtprot, cmbProductionTypeComparison, "Id", "ProductionTypeDescription", "Production Type ", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboBindComparison()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesIds = UserAccount.BranchesId.ToString();
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)cmbBranchNameComparison).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbBranchNameComparison).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = BranchIds;
			}
			DataTable dtcombo = InvFoodProduction.GetDataForDropDownFromFoodProduction(obj);
			if (dtcombo.Rows.Count > 0)
			{
				DataTable dtjob = new DataTable();
				dtjob.Columns.Add("Id");
				dtjob.Columns.Add("name");
				DataTable dtPlant = new DataTable();
				dtPlant.Columns.Add("Id");
				dtPlant.Columns.Add("name");
				for (int i = 0; i < dtcombo.Rows.Count; i++)
				{
					if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "JobOrder")
					{
						dtjob.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
					}
					if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "Plant")
					{
						dtPlant.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
					}
				}
				if (dtjob.Rows.Count > 0)
				{
					DDL.BindDDL(dtjob, CmbJobOrderNoComprison, "Id", "name", "Job Order", ZeroIndex: true);
					((UltraGridBase)CmbJobOrderNoComprison).Rows[0].Activate();
				}
				else
				{
					((Control)(object)CmbJobOrderNoComprison).Text = string.Empty;
					((UltraGridBase)CmbJobOrderNoComprison).DataSource = null;
				}
				if (dtPlant.Rows.Count > 0)
				{
					DDL.BindDDL(dtPlant, cmbPlantComparison, "Id", "name", "Plant Name", ZeroIndex: true);
					((UltraGridBase)cmbPlantComparison).Rows[0].Activate();
				}
				else
				{
					((Control)(object)cmbPlantComparison).Text = string.Empty;
					((UltraGridBase)cmbPlantComparison).DataSource = null;
				}
			}
			else
			{
				((Control)(object)CmbJobOrderNoComprison).Text = string.Empty;
				((UltraGridBase)CmbJobOrderNoComprison).DataSource = null;
				((Control)(object)cmbPlantComparison).Text = string.Empty;
				((UltraGridBase)cmbPlantComparison).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboBindSummary()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesIds = UserAccount.BranchesId.ToString();
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)CmbBranchNameSummary).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)CmbBranchNameSummary).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = BranchIds;
			}
			DataTable dtcombo = InvFoodProduction.GetDataForDropDownFromFoodProduction(obj);
			if (dtcombo.Rows.Count > 0)
			{
				DataTable dtjob = new DataTable();
				dtjob.Columns.Add("Id");
				dtjob.Columns.Add("name");
				DataTable dtitem = new DataTable();
				dtitem.Columns.Add("Id");
				dtitem.Columns.Add("name");
				DataTable dtPlant = new DataTable();
				dtPlant.Columns.Add("Id");
				dtPlant.Columns.Add("name");
				for (int i = 0; i < dtcombo.Rows.Count; i++)
				{
					if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "JobOrder")
					{
						dtjob.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
					}
					if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "Plant")
					{
						dtPlant.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
					}
					if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ItemName")
					{
						dtitem.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
					}
				}
				if (dtjob.Rows.Count > 0)
				{
					DDL.BindDDL(dtjob, cmbJobOrderSummary, "Id", "name", "Job Order", ZeroIndex: true);
					((UltraGridBase)cmbJobOrderSummary).Rows[0].Activate();
				}
				else
				{
					((Control)(object)cmbJobOrderSummary).Text = string.Empty;
					((UltraGridBase)cmbJobOrderSummary).DataSource = null;
				}
				if (dtPlant.Rows.Count > 0)
				{
					DDL.BindDDL(dtPlant, CmbPlantSummary, "Id", "name", "Plant Name", ZeroIndex: true);
					((UltraGridBase)CmbPlantSummary).Rows[0].Activate();
				}
				else
				{
					((Control)(object)CmbPlantSummary).Text = string.Empty;
					((UltraGridBase)CmbPlantSummary).DataSource = null;
				}
				if (dtitem.Rows.Count > 0)
				{
					DDL.BindDDL(dtitem, CmbItemNameSummary, "Id", "name", "Item Name", ZeroIndex: true);
					((UltraGridBase)CmbItemNameSummary).Rows[0].Activate();
				}
				else
				{
					((Control)(object)CmbItemNameSummary).Text = string.Empty;
					((UltraGridBase)CmbItemNameSummary).DataSource = null;
				}
			}
			else
			{
				((Control)(object)cmbJobOrderSummary).Text = string.Empty;
				((UltraGridBase)cmbJobOrderSummary).DataSource = null;
				((Control)(object)CmbPlantSummary).Text = string.Empty;
				((UltraGridBase)CmbPlantSummary).DataSource = null;
				((Control)(object)CmbItemNameSummary).Text = string.Empty;
				((UltraGridBase)CmbItemNameSummary).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbItemName_Leave(object sender, EventArgs e)
	{
		try
		{
			DataTable dt = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(CmbItemNameSummary.Value));
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbPackUomSummary, "Id", "UOMCode", "PackUom", ZeroIndex: false);
				((UltraGridBase)CmbPackUomSummary).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			}
			else
			{
				((Control)(object)CmbPackUomSummary).Text = string.Empty;
				((UltraGridBase)CmbPackUomSummary).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbBranchNameComparison_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbBranchNameComparison).Text != string.Empty && ((UltraGridBase)cmbBranchNameComparison).ActiveRow != null)
			{
				ComboBindComparison();
				return;
			}
			((Control)(object)CmbJobOrderNoComprison).Text = string.Empty;
			((UltraGridBase)CmbJobOrderNoComprison).ActiveRow = null;
			((Control)(object)cmbPlantComparison).Text = string.Empty;
			((UltraGridBase)cmbPlantComparison).ActiveRow = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbBranchNameSummary_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)CmbBranchNameSummary).Text != string.Empty && ((UltraGridBase)CmbBranchNameSummary).ActiveRow != null)
			{
				ComboBindSummary();
				return;
			}
			((Control)(object)cmbJobOrderSummary).Text = string.Empty;
			((UltraGridBase)cmbJobOrderSummary).ActiveRow = null;
			((Control)(object)CmbPlantSummary).Text = string.Empty;
			((UltraGridBase)CmbPlantSummary).ActiveRow = null;
			((Control)(object)CmbItemNameSummary).Text = string.Empty;
			((UltraGridBase)CmbItemNameSummary).ActiveRow = null;
			((Control)(object)CmbPackUomSummary).Text = string.Empty;
			((UltraGridBase)CmbPackUomSummary).ActiveRow = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void reset()
	{
		try
		{
			GpsNoFromComparison.Text = string.Empty;
			GpsNoToComparsion.Text = string.Empty;
			((Control)(object)cmbPlantComparison).Text = string.Empty;
			((UltraGridBase)cmbProductionTypeComparison).Rows[1].Activate();
			gridHisory();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewInput_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)cmbJobOrderSummary).Text = string.Empty;
			grdInput.ClearStructure();
			RdInputDetail.Checked = true;
			LablesStatus(status: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FoodProductionComparisonRpt_FormClosing(object sender, FormClosingEventArgs e)
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

	private void BtnRefreshSummary_Click(object sender, EventArgs e)
	{
		try
		{
			BranchesFill();
			ComboBindSummary();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshComparison_Click(object sender, EventArgs e)
	{
		try
		{
			BranchesFill();
			ComboBindComparison();
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
			if (tabControl1.SelectedIndex == 1)
			{
				datFromDateSummary.Focus();
				RdInputDetail.Checked = true;
				LablesStatus(status: false);
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
			gridHisory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridHisory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDocNo = Conversion.ToInt((object)GpsNoFromComparison.Text);
			obj.ToDocNo = Conversion.ToInt((object)GpsNoToComparsion.Text);
			obj.ItemId = Conversion.ToInt(cmbPlantComparison.Value);
			obj.JobOrderId = Conversion.ToInt(CmbJobOrderNoComprison.Value);
			obj.ReportType = Conversion.ToString((object)((Control)(object)cmbProductionTypeComparison).Text);
			dtReg = InvFoodProduction.FoodProductionComparisons_Rpt(obj);
			if (dtReg.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("PlanCode", typeof(string));
				dtcol.Columns.Add("PlanDate", typeof(DateTime));
				dtcol.Columns.Add("JobOrderNo", typeof(string));
				dtcol.Columns.Add("PlanType", typeof(string));
				dtcol.Columns.Add("Plant", typeof(string));
				dtcol.Columns.Add("Input", typeof(decimal));
				dtcol.Columns.Add("BP_Output", typeof(decimal));
				dtcol.Columns.Add("FG_Output", typeof(decimal));
				dtcol.Columns.Add("Short_Gain", typeof(decimal));
				dtcol.Columns.Add("BP_Recovery", typeof(decimal));
				dtcol.Columns.Add("FG_Recovery", typeof(decimal));
				dtcol.Columns.Add("Total_Recovery", typeof(decimal));
				dtcol.Rows.Clear();
				for (int i = 0; i < dtReg.Rows.Count; i++)
				{
					dtcol.Rows.Add(dtReg.Rows[i]["Id"], dtReg.Rows[i]["PlanCode"], dtReg.Rows[i]["PlanDate"], dtReg.Rows[i]["JobOrderNo"], dtReg.Rows[i]["PlanType"], dtReg.Rows[i]["Plant"], dtReg.Rows[i]["Input"], dtReg.Rows[i]["BP_Output"], dtReg.Rows[i]["FG_Output"], dtReg.Rows[i]["Short_Gain"], dtReg.Rows[i]["BP_Recovery"], dtReg.Rows[i]["FG_Recovery"], dtReg.Rows[i]["Total_Recovery"]);
				}
				DataGridHistory.DataSource = dtcol;
				DataGridHistory.RetrieveStructure();
				GridSetting();
			}
			else
			{
				DataGridHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting()
	{
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["Id"].Width = 10;
			DataGridHistory.RootTable.Columns["PlanCode"].Width = 80;
			DataGridHistory.RootTable.Columns["PlanDate"].Width = 80;
			DataGridHistory.RootTable.Columns["JobOrderNo"].Width = 140;
			DataGridHistory.RootTable.Columns["PlanType"].Width = 100;
			DataGridHistory.RootTable.Columns["Plant"].Width = 110;
			DataGridHistory.RootTable.Columns["Input"].Width = 90;
			DataGridHistory.RootTable.Columns["BP_Output"].Width = 100;
			DataGridHistory.RootTable.Columns["FG_Output"].Width = 100;
			DataGridHistory.RootTable.Columns["Short_Gain"].Width = 100;
			DataGridHistory.RootTable.Columns["BP_Recovery"].Width = 100;
			DataGridHistory.RootTable.Columns["FG_Recovery"].Width = 100;
			DataGridHistory.RootTable.Columns["Total_Recovery"].Width = 110;
			DataGridHistory.RootTable.Columns["Input"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["Input"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["Input"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["Input"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["BP_Output"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["BP_Output"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["BP_Output"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["BP_Output"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["FG_Output"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FG_Output"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FG_Output"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["FG_Output"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["Short_Gain"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["Short_Gain"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["Short_Gain"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["Short_Gain"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["BP_Recovery"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["BP_Recovery"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["BP_Recovery"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["BP_Recovery"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["FG_Recovery"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FG_Recovery"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FG_Recovery"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["FG_Recovery"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["Total_Recovery"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["Total_Recovery"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["Total_Recovery"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["Total_Recovery"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 50;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Expected O, but got Unknown
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			int JobOrderId = Conversion.ToInt(r.Cells["Id"].Value);
			if (!(e.Column.Key == "Print"))
			{
				return;
			}
			DataTable dt = InvFoodProductionReports.InvFoodProductionRecoverySummeryReport(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				JobOrderId = JobOrderId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Record Not Found For Display");
				return;
			}
			List<SubReportObject> subReports = new List<SubReportObject>();
			DataTable dtSub = InvFoodProductionReports.DailyPlantConsumedHoursByJobOrderId_SubReport(JobOrderId, 0);
			if (dtSub != null && dtSub.Rows.Count > 0)
			{
				subReports.Add(new SubReportObject(dtSub, "DailyPlantConsumedHoursByJobOrderId_SubReport"));
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress);
			val.RptPerameter("@CompanyName", UserAccount.CompName);
			val.ShowReportWithDataTableSubReprt(dt, "602-InvRptProductionSummery.rpt", subReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShowInput_Click(object sender, EventArgs e)
	{
		try
		{
			if (RdInputDetail.Checked)
			{
				GridFillInput();
			}
			else
			{
				Production_GainLossSummary();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridFillInput()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.JobOrderId = Conversion.ToInt(cmbJobOrderSummary.Value);
			obj.FromDate = datFromDateSummary.Value;
			obj.ToDate = datToDateSummary.Value;
			obj.PlantId = Conversion.ToInt(CmbPlantSummary.Value);
			obj.ItemId = Conversion.ToInt(CmbItemNameSummary.Value);
			obj.ItemUomId = Conversion.ToInt(CmbPackUomSummary.Value);
			dtInput = InvFoodProductionReports.FoodProductionIssuanceGrnWiseByJobOrderId_607(obj);
			grdInput.ClearStructure();
			if (dtInput.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("OrderNo", typeof(int));
				dtcol.Columns.Add("GpNo", typeof(string));
				dtcol.Columns.Add("GpDate", typeof(DateTime));
				dtcol.Columns.Add("GrnNo", typeof(int));
				dtcol.Columns.Add("GrnDate", typeof(DateTime));
				dtcol.Columns.Add("InvoiceNo", typeof(int));
				dtcol.Columns.Add("JobOrderNo", typeof(string));
				dtcol.Columns.Add("ProductionInstruction", typeof(string));
				dtcol.Columns.Add("SupplierName", typeof(string));
				dtcol.Columns.Add("ItemName", typeof(string));
				dtcol.Columns.Add("VehicleNo", typeof(string));
				dtcol.Columns.Add("BiltyNo", typeof(string));
				dtcol.Columns.Add("CropYear", typeof(string));
				dtcol.Columns.Add("PackingType", typeof(string));
				dtcol.Columns.Add("PackUom", typeof(string));
				dtcol.Columns.Add("IssueQty", typeof(decimal));
				dtcol.Columns.Add("IssueWeight", typeof(decimal));
				dtcol.Columns.Add("ItemQty", typeof(decimal));
				dtcol.Columns.Add("GrossWeight", typeof(decimal));
				dtcol.Columns.Add("EBWPerUnit", typeof(decimal));
				dtcol.Columns.Add("EBWTotal", typeof(decimal));
				dtcol.Columns.Add("AdLsWeight", typeof(decimal));
				dtcol.Columns.Add("StockWeight", typeof(decimal));
				dtcol.Columns.Add("NetBillWeight", typeof(decimal));
				dtcol.Columns.Add("ItemAmount", typeof(decimal));
				dtcol.Columns.Add("ItemNetAmount", typeof(decimal));
				dtcol.Columns.Add("Transporter", typeof(string));
				dtcol.Columns.Add("CarriageAmount", typeof(decimal));
				dtcol.Columns.Add("CityName", typeof(string));
				dtcol.Rows.Clear();
				for (int i = 0; i < dtInput.Rows.Count; i++)
				{
					dtcol.Rows.Add(dtInput.Rows[i]["OrderNo"], dtInput.Rows[i]["GpNo"], dtInput.Rows[i]["GpDate"], dtInput.Rows[i]["GrnNo"], dtInput.Rows[i]["GrnDate"], dtInput.Rows[i]["InvoiceNo"], dtInput.Rows[i]["JobOrderNo"], dtInput.Rows[i]["ProductionInsturction"], dtInput.Rows[i]["SupplierName"], dtInput.Rows[i]["ItemName"], dtInput.Rows[i]["VehicleNo"], dtInput.Rows[i]["BiltyNo"], dtInput.Rows[i]["CropYear"], dtInput.Rows[i]["PackTypeDesc"], dtInput.Rows[i]["UOMCode"], dtInput.Rows[i]["IssueQty"], dtInput.Rows[i]["IssueWeight"], dtInput.Rows[i]["ItemQty"], dtInput.Rows[i]["GrossWeight"], dtInput.Rows[i]["EBWPerUnit"], dtInput.Rows[i]["EBWTotal"], dtInput.Rows[i]["AdLsWeight"], dtInput.Rows[i]["StockWeight"], dtInput.Rows[i]["NetBillWeight"], dtInput.Rows[i]["ItemAmount"], dtInput.Rows[i]["ItemNetAmount"], dtInput.Rows[i]["TransporterName"], dtInput.Rows[i]["CarriageAmount"], dtInput.Rows[i]["AreaCity"]);
				}
				grdInput.DataSource = dtcol;
				grdInput.RetrieveStructure();
				grdInputSetting();
			}
			else
			{
				grdInput.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Production_GainLossSummary()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = datFromDateSummary.Value;
			obj.ToDate = datToDateSummary.Value;
			obj.JobOrderId = Conversion.ToInt(cmbJobOrderSummary.Value);
			obj.ItemId = Conversion.ToInt(CmbItemNameSummary.Value);
			obj.stockUOM = Conversion.ToInt(CmbPackUomSummary.Value);
			obj.PlantId = Conversion.ToInt(CmbPlantSummary.Value);
			dtGainLoss = InvFoodProductionReports.Production_GainLossSummary_Rpt(obj);
			grdInput.ClearStructure();
			if (dtGainLoss.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("JobOrderNo", typeof(string));
				dtcol.Columns.Add("EntryType", typeof(string));
				dtcol.Columns.Add("ItemCode", typeof(string));
				dtcol.Columns.Add("ItemName", typeof(string));
				dtcol.Columns.Add("PackUom", typeof(string));
				dtcol.Columns.Add("IssueQty", typeof(decimal));
				dtcol.Columns.Add("IssueWeight", typeof(decimal));
				dtcol.Columns.Add("ProductionQty", typeof(decimal));
				dtcol.Columns.Add("ProductionWeight", typeof(decimal));
				dtcol.Rows.Clear();
				for (int i = 0; i < dtGainLoss.Rows.Count; i++)
				{
					dtcol.Rows.Add(dtGainLoss.Rows[i]["JobOrderNo"], dtGainLoss.Rows[i]["TypeDescription"], dtGainLoss.Rows[i]["ItemCode"], dtGainLoss.Rows[i]["ItemName"], dtGainLoss.Rows[i]["PackSize"], dtGainLoss.Rows[i]["IssueQty"], dtGainLoss.Rows[i]["IssueWeight"], dtGainLoss.Rows[i]["ProductionQty"], dtGainLoss.Rows[i]["ProductionWeight"]);
				}
				grdInput.DataSource = dtcol;
				grdInput.RetrieveStructure();
				grdGainLossSummarySetting();
				double IssueWeight = Conversion.ToDouble(grdInput.GetTotal(grdInput.RootTable.Columns["IssueWeight"], (AggregateFunction)2));
				double ProdWeight = Conversion.ToDouble(grdInput.GetTotal(grdInput.RootTable.Columns["ProductionWeight"], (AggregateFunction)2));
				double Diff = IssueWeight - ProdWeight;
				lblSumofTotalIssue.Text = IssueWeight.ToString("#,#");
				lblSumOfOutPut.Text = ProdWeight.ToString("#,#");
				lblGainLossDiff.Text = Diff.ToString("#,#;(#,#);0");
				if (Diff > 0.0)
				{
					lblGainLossStatus.Text = "Loss";
					lblPercent.ForeColor = Color.Red;
					lblGainLossStatus.ForeColor = Color.Red;
					lblGainLossStatus.Font = new Font(lblGainLossStatus.Font, FontStyle.Bold);
				}
				else if (Diff < 0.0)
				{
					lblGainLossStatus.Text = "Gain";
					lblGainLossStatus.ForeColor = Color.Green;
					lblPercent.ForeColor = Color.Green;
					lblGainLossStatus.Font = new Font(lblGainLossStatus.Font, FontStyle.Bold);
				}
				else if (Diff == 0.0)
				{
					lblGainLossStatus.Text = "Nill";
					lblPercent.ForeColor = Color.Black;
					lblGainLossStatus.ForeColor = Color.Black;
					lblGainLossStatus.Font = new Font(lblGainLossStatus.Font, FontStyle.Bold);
				}
				if (Diff != 0.0)
				{
					double Perct = Diff / IssueWeight * 100.0;
					lblPercent.Text = Math.Round(Math.Abs(Perct), 2) + " %";
				}
				LablesStatus(status: true);
			}
			else
			{
				LablesStatus(status: false);
				grdInput.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LablesStatus(bool status)
	{
		try
		{
			PanelValues.Visible = status;
			lblTotalIssueWeight.Visible = true;
			lblProdTotalWeight.Visible = true;
			lblSumofTotalIssue.Visible = true;
			lblSumOfOutPut.Visible = true;
			lblGainLossDiff.Visible = true;
			lblGainLossStatus.Visible = true;
			lblGainLossKg.Visible = true;
			lblPercent.Visible = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInputSetting()
	{
		try
		{
			grdInput.RootTable.Columns["OrderNo"].Width = 60;
			grdInput.RootTable.Columns["GpNo"].Width = 50;
			grdInput.RootTable.Columns["GpDate"].Width = 70;
			grdInput.RootTable.Columns["GrnNo"].Width = 60;
			grdInput.RootTable.Columns["GrnDate"].Width = 80;
			grdInput.RootTable.Columns["InvoiceNo"].Width = 90;
			grdInput.RootTable.Columns["JobOrderNo"].Width = 120;
			grdInput.RootTable.Columns["ProductionInstruction"].Width = 200;
			grdInput.RootTable.Columns["SupplierName"].Width = 250;
			grdInput.RootTable.Columns["ItemName"].Width = 250;
			grdInput.RootTable.Columns["VehicleNo"].Width = 100;
			grdInput.RootTable.Columns["BiltyNo"].Width = 100;
			grdInput.RootTable.Columns["CropYear"].Width = 80;
			grdInput.RootTable.Columns["PackingType"].Width = 100;
			grdInput.RootTable.Columns["PackUom"].Width = 70;
			grdInput.RootTable.Columns["IssueQty"].Width = 80;
			grdInput.RootTable.Columns["IssueWeight"].Width = 90;
			grdInput.RootTable.Columns["ItemQty"].Width = 80;
			grdInput.RootTable.Columns["GrossWeight"].Width = 90;
			grdInput.RootTable.Columns["EBWPerUnit"].Width = 110;
			grdInput.RootTable.Columns["EBWTotal"].Width = 100;
			grdInput.RootTable.Columns["AdLsWeight"].Width = 100;
			grdInput.RootTable.Columns["StockWeight"].Width = 100;
			grdInput.RootTable.Columns["NetBillWeight"].Width = 100;
			grdInput.RootTable.Columns["ItemAmount"].Width = 100;
			grdInput.RootTable.Columns["ItemNetAmount"].Width = 100;
			grdInput.RootTable.Columns["Transporter"].Width = 140;
			grdInput.RootTable.Columns["CarriageAmount"].Width = 110;
			grdInput.RootTable.Columns["CityName"].Width = 140;
			grdInput.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdInput.RootTable.Columns["ItemQty"].FormatString = "0,0";
			grdInput.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grdInput.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grdInput.RootTable.Columns["GrossWeight"].FormatString = "#,##0.##";
			grdInput.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
			grdInput.RootTable.Columns["EBWPerUnit"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["EBWPerUnit"].TextAlignment = (TextAlignment)3;
			grdInput.RootTable.Columns["EBWPerUnit"].FormatString = "#,##0.##";
			grdInput.RootTable.Columns["EBWPerUnit"].TotalFormatString = "#,##0.##";
			grdInput.RootTable.Columns["EBWTotal"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["EBWTotal"].TextAlignment = (TextAlignment)3;
			grdInput.RootTable.Columns["EBWTotal"].FormatString = "#,##0.##";
			grdInput.RootTable.Columns["EBWTotal"].TotalFormatString = "#,##0.##";
			grdInput.RootTable.Columns["AdLsWeight"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["AdLsWeight"].TextAlignment = (TextAlignment)3;
			grdInput.RootTable.Columns["AdLsWeight"].FormatString = "#,##0.##";
			grdInput.RootTable.Columns["AdLsWeight"].TotalFormatString = "#,##0.##";
			grdInput.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			grdInput.RootTable.Columns["StockWeight"].FormatString = "#,##0.##";
			grdInput.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.##";
			grdInput.RootTable.Columns["IssueWeight"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["IssueWeight"].TextAlignment = (TextAlignment)3;
			grdInput.RootTable.Columns["IssueWeight"].FormatString = "#,##0.##";
			grdInput.RootTable.Columns["IssueWeight"].TotalFormatString = "#,##0.##";
			grdInput.RootTable.Columns["IssueQty"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["IssueQty"].TextAlignment = (TextAlignment)3;
			grdInput.RootTable.Columns["IssueQty"].FormatString = "#,##0.##";
			grdInput.RootTable.Columns["IssueQty"].TotalFormatString = "#,##0.##";
			grdInput.RootTable.Columns["ItemAmount"].Visible = false;
			grdInput.RootTable.Columns["ItemNetAmount"].Visible = false;
			if (DoHaveCanRateandAmount)
			{
				grdInput.RootTable.Columns["ItemAmount"].Visible = true;
				grdInput.RootTable.Columns["ItemNetAmount"].Visible = true;
				grdInput.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
				grdInput.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
				grdInput.RootTable.Columns["ItemAmount"].FormatString = "#,##0.##";
				grdInput.RootTable.Columns["ItemAmount"].TotalFormatString = "#,##0.##";
				grdInput.RootTable.Columns["ItemNetAmount"].AggregateFunction = (AggregateFunction)2;
				grdInput.RootTable.Columns["ItemNetAmount"].TextAlignment = (TextAlignment)3;
				grdInput.RootTable.Columns["ItemNetAmount"].FormatString = "#,##0.##";
				grdInput.RootTable.Columns["ItemNetAmount"].TotalFormatString = "#,##0.##";
			}
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGainLossSummarySetting()
	{
		try
		{
			grdInput.RootTable.Columns["JobOrderNo"].Width = 120;
			grdInput.RootTable.Columns["EntryType"].Width = 120;
			grdInput.RootTable.Columns["ItemCode"].Width = 100;
			grdInput.RootTable.Columns["ItemName"].Width = 200;
			grdInput.RootTable.Columns["PackUom"].Width = 60;
			grdInput.RootTable.Columns["IssueQty"].Width = 80;
			grdInput.RootTable.Columns["IssueWeight"].Width = 90;
			grdInput.RootTable.Columns["ProductionQty"].Width = 100;
			grdInput.RootTable.Columns["ProductionWeight"].Width = 110;
			grdInput.RootTable.Columns["IssueQty"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["IssueWeight"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["ProductionQty"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["ProductionWeight"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["IssueQty"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["IssueQty"].TextAlignment = (TextAlignment)3;
			grdInput.RootTable.Columns["IssueQty"].FormatString = "0,0";
			grdInput.RootTable.Columns["IssueQty"].TotalFormatString = "#,##0.##";
			grdInput.RootTable.Columns["IssueWeight"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["IssueWeight"].TextAlignment = (TextAlignment)3;
			grdInput.RootTable.Columns["IssueWeight"].FormatString = "#,##0.##";
			grdInput.RootTable.Columns["IssueWeight"].TotalFormatString = "#,##0.##";
			grdInput.RootTable.Columns["ProductionQty"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["ProductionQty"].TextAlignment = (TextAlignment)3;
			grdInput.RootTable.Columns["ProductionQty"].FormatString = "#,##0.##";
			grdInput.RootTable.Columns["ProductionQty"].TotalFormatString = "#,##0.##";
			grdInput.RootTable.Columns["ProductionWeight"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["ProductionWeight"].TextAlignment = (TextAlignment)3;
			grdInput.RootTable.Columns["ProductionWeight"].FormatString = "#,##0.##";
			grdInput.RootTable.Columns["ProductionWeight"].TotalFormatString = "#,##0.##";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RdInputDetail_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (RdInputDetail.Checked)
			{
				LablesStatus(status: false);
				grdInput.ClearStructure();
			}
			else
			{
				grdInput.ClearStructure();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataGridHistory).Name);
			if (fs != null)
			{
				DataGridHistory.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdInput).Name);
			if (fs != null)
			{
				grdInput.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ReportsLoad()
	{
		try
		{
			string[] files = CommonServices.DynamicReportsLoad("ProductionGainLoss");
			if (files != null && files.Length != 0)
			{
				for (int i = 0; i < files.Length; i++)
				{
					tsDropDownGainLoss.DropDownItems.Add(Path.GetFileNameWithoutExtension(files[i]));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtReg.Rows.Count == 0 || dtReg == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dtReg, "604-FoodProductionComparisonRpt.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint607Register_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtInput.Rows.Count == 0 || dtInput == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dtInput, "607-FoodProductionIssuanceGrnWiseByJobOrderId.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tsDropDownGainLoss_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtGainLoss.Rows.Count == 0 || dtGainLoss == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dtGainLoss, e.ClickedItem.Text + ".rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GpsNoFrom_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GpsNoTo_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmGPOutward_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.Control && e.KeyCode == Keys.P)
		{
			toolStripButton1_Click(null, null);
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (e.Control && e.KeyCode == Keys.N)
		{
			btnNew_Click(null, null);
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
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected O, but got Unknown
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Expected O, but got Unknown
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Expected O, but got Unknown
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Expected O, but got Unknown
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Expected O, but got Unknown
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Expected O, but got Unknown
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Expected O, but got Unknown
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Expected O, but got Unknown
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Expected O, but got Unknown
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Expected O, but got Unknown
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Expected O, but got Unknown
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Expected O, but got Unknown
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Expected O, but got Unknown
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Expected O, but got Unknown
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Expected O, but got Unknown
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Expected O, but got Unknown
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Expected O, but got Unknown
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Expected O, but got Unknown
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Expected O, but got Unknown
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Expected O, but got Unknown
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Expected O, but got Unknown
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Expected O, but got Unknown
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Expected O, but got Unknown
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Expected O, but got Unknown
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Expected O, but got Unknown
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Expected O, but got Unknown
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected O, but got Unknown
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Expected O, but got Unknown
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Expected O, but got Unknown
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Expected O, but got Unknown
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Expected O, but got Unknown
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Expected O, but got Unknown
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Expected O, but got Unknown
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Expected O, but got Unknown
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Expected O, but got Unknown
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Expected O, but got Unknown
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Expected O, but got Unknown
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Expected O, but got Unknown
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Expected O, but got Unknown
		//IL_048b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Expected O, but got Unknown
		//IL_051a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0524: Expected O, but got Unknown
		//IL_0530: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Expected O, but got Unknown
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_0550: Expected O, but got Unknown
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Expected O, but got Unknown
		//IL_0acc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad6: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance5 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		GridEXLayout grdInput_DesignTimeLayout = new GridEXLayout();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance69 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label12 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.cmbBranchNameComparison = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbPlantComparison = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbJobOrderNoComprison = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.cmbProductionTypeComparison = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.btnshowComparison = new System.Windows.Forms.Button();
		this.GpsNoToComparsion = new System.Windows.Forms.TextBox();
		this.GpsNoFromComparison = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshComparison = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdInput = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label7 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label17 = new System.Windows.Forms.Label();
		this.CmbBranchNameSummary = new UltraCombo();
		this.PanelValues = new System.Windows.Forms.Panel();
		this.panel52 = new System.Windows.Forms.Panel();
		this.lblTotalIssueWeight = new System.Windows.Forms.Label();
		this.lblProdTotalWeight = new System.Windows.Forms.Label();
		this.lblGainLossKg = new System.Windows.Forms.Label();
		this.lblSumOfOutPut = new System.Windows.Forms.TextBox();
		this.lblSumofTotalIssue = new System.Windows.Forms.TextBox();
		this.lblGainLossDiff = new System.Windows.Forms.TextBox();
		this.lblGainLossStatus = new System.Windows.Forms.Label();
		this.lblPercent = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label11 = new System.Windows.Forms.Label();
		this.CmbPlantSummary = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.CmbPackUomSummary = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.CmbItemNameSummary = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.RdGainLoss = new System.Windows.Forms.RadioButton();
		this.RdInputDetail = new System.Windows.Forms.RadioButton();
		this.label6 = new System.Windows.Forms.Label();
		this.datToDateSummary = new System.Windows.Forms.DateTimePicker();
		this.datFromDateSummary = new System.Windows.Forms.DateTimePicker();
		this.btnShowSummary = new System.Windows.Forms.Button();
		this.cmbJobOrderSummary = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNewSummary = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshSummary = new System.Windows.Forms.ToolStripButton();
		this.btnPrint607Register = new System.Windows.Forms.ToolStripButton();
		this.tsDropDownGainLoss = new System.Windows.Forms.ToolStripDropDownButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel5.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchNameComparison).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPlantComparison).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNoComprison).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbProductionTypeComparison).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInput).BeginInit();
		this.panel10.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranchNameSummary).BeginInit();
		this.PanelValues.SuspendLayout();
		this.panel52.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantSummary).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUomSummary).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameSummary).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobOrderSummary).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1190, 561);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1190, 561);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1182, 532);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Production Comparison";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel6.Controls.Add(this.panel5);
		this.panel6.Controls.Add(this.panel4);
		this.panel6.Controls.Add(this.panel3);
		this.panel6.Controls.Add(this.panel2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1176, 526);
		this.panel6.TabIndex = 1;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 143);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1176, 383);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 47;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Controls.Add(this.label12);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 120);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1176, 23);
		this.panel5.TabIndex = 1;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1132, 1);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 20);
		this.ctrlGrdBar1.TabIndex = 9;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(3, 2);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(132, 18);
		this.label12.TabIndex = 4;
		this.label12.Text = "Filtered Records";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchNameComparison);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPlantComparison);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrderNoComprison);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbProductionTypeComparison);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.btnshowComparison);
		this.panel4.Controls.Add(this.GpsNoToComparsion);
		this.panel4.Controls.Add(this.GpsNoFromComparison);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 56);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1176, 64);
		this.panel4.TabIndex = 1;
		this.cmbBranchNameComparison.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchNameComparison.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchNameComparison.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchNameComparison.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchNameComparison).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbBranchNameComparison).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchNameComparison).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchNameComparison).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchNameComparison).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance19;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchNameComparison).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchNameComparison).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbBranchNameComparison).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchNameComparison).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchNameComparison).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchNameComparison).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbBranchNameComparison).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchNameComparison).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchNameComparison).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance25).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchNameComparison).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbBranchNameComparison).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchNameComparison).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchNameComparison).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchNameComparison).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbBranchNameComparison).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchNameComparison).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchNameComparison).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbBranchNameComparison).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchNameComparison).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbBranchNameComparison).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchNameComparison).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchNameComparison).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchNameComparison).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchNameComparison).Location = new System.Drawing.Point(95, 6);
		((System.Windows.Forms.Control)(object)this.cmbBranchNameComparison).Name = "cmbBranchNameComparison";
		((System.Windows.Forms.Control)(object)this.cmbBranchNameComparison).Size = new System.Drawing.Size(224, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchNameComparison).TabIndex = 591;
		((System.Windows.Forms.Control)(object)this.cmbBranchNameComparison).Leave += new System.EventHandler(cmbBranchNameComparison_Leave);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(9, 12);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(79, 15);
		this.label2.TabIndex = 592;
		this.label2.Text = "Branch Name";
		this.cmbPlantComparison.AllowNull = (DefaultableBoolean)1;
		this.cmbPlantComparison.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPlantComparison.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPlantComparison.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPlantComparison).DisplayLayout.Appearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbPlantComparison).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPlantComparison).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPlantComparison).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPlantComparison).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance72;
		((SpecialBoxBase)((UltraGridBase)this.cmbPlantComparison).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPlantComparison).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbPlantComparison).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPlantComparison).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPlantComparison).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPlantComparison).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbPlantComparison).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPlantComparison).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPlantComparison).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance77).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPlantComparison).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbPlantComparison).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPlantComparison).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance78).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPlantComparison).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance80).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPlantComparison).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbPlantComparison).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPlantComparison).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPlantComparison).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbPlantComparison).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPlantComparison).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbPlantComparison).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPlantComparison).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPlantComparison).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPlantComparison).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPlantComparison.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPlantComparison).Location = new System.Drawing.Point(717, 6);
		((System.Windows.Forms.Control)(object)this.cmbPlantComparison).Name = "cmbPlantComparison";
		((System.Windows.Forms.Control)(object)this.cmbPlantComparison).Size = new System.Drawing.Size(216, 26);
		((System.Windows.Forms.Control)(object)this.cmbPlantComparison).TabIndex = 3;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(647, 12);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(69, 15);
		this.label1.TabIndex = 177;
		this.label1.Text = "Plant Name";
		this.CmbJobOrderNoComprison.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrderNoComprison.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrderNoComprison.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrderNoComprison.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrderNoComprison).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbJobOrderNoComprison).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrderNoComprison).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNoComprison).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNoComprison).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance90;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNoComprison).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNoComprison).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbJobOrderNoComprison).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrderNoComprison).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance2).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrderNoComprison).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrderNoComprison).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbJobOrderNoComprison).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrderNoComprison).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNoComprison).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance38).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrderNoComprison).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbJobOrderNoComprison).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrderNoComprison).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNoComprison).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrderNoComprison).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbJobOrderNoComprison).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrderNoComprison).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrderNoComprison).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbJobOrderNoComprison).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrderNoComprison).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbJobOrderNoComprison).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrderNoComprison).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrderNoComprison).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoComprison).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrderNoComprison.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoComprison).Location = new System.Drawing.Point(426, 34);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoComprison).Name = "CmbJobOrderNoComprison";
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoComprison).Size = new System.Drawing.Size(216, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoComprison).TabIndex = 2;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(325, 40);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(75, 15);
		this.label13.TabIndex = 175;
		this.label13.Text = "JobOrder No";
		this.cmbProductionTypeComparison.AllowNull = (DefaultableBoolean)1;
		this.cmbProductionTypeComparison.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbProductionTypeComparison.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbProductionTypeComparison.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbProductionTypeComparison).DisplayLayout.Appearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbProductionTypeComparison).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbProductionTypeComparison).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance97).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance97).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbProductionTypeComparison).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbProductionTypeComparison).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance98;
		((SpecialBoxBase)((UltraGridBase)this.cmbProductionTypeComparison).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance99).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance99).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbProductionTypeComparison).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbProductionTypeComparison).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbProductionTypeComparison).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbProductionTypeComparison).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbProductionTypeComparison).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbProductionTypeComparison).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbProductionTypeComparison).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbProductionTypeComparison).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance104).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbProductionTypeComparison).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbProductionTypeComparison).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbProductionTypeComparison).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbProductionTypeComparison).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbProductionTypeComparison).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbProductionTypeComparison).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbProductionTypeComparison).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbProductionTypeComparison).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbProductionTypeComparison).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbProductionTypeComparison).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbProductionTypeComparison).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbProductionTypeComparison).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbProductionTypeComparison).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbProductionTypeComparison).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbProductionTypeComparison.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbProductionTypeComparison).Location = new System.Drawing.Point(426, 6);
		((System.Windows.Forms.Control)(object)this.cmbProductionTypeComparison).Name = "cmbProductionTypeComparison";
		((System.Windows.Forms.Control)(object)this.cmbProductionTypeComparison).Size = new System.Drawing.Size(216, 26);
		((System.Windows.Forms.Control)(object)this.cmbProductionTypeComparison).TabIndex = 2;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(325, 12);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(94, 15);
		this.label4.TabIndex = 175;
		this.label4.Text = "Production Type";
		this.btnshowComparison.BackColor = System.Drawing.Color.Teal;
		this.btnshowComparison.FlatAppearance.BorderSize = 0;
		this.btnshowComparison.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshowComparison.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshowComparison.ForeColor = System.Drawing.Color.White;
		this.btnshowComparison.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshowComparison.Location = new System.Drawing.Point(877, 34);
		this.btnshowComparison.Margin = new System.Windows.Forms.Padding(2);
		this.btnshowComparison.Name = "btnshowComparison";
		this.btnshowComparison.Size = new System.Drawing.Size(56, 26);
		this.btnshowComparison.TabIndex = 4;
		this.btnshowComparison.Text = "Show";
		this.btnshowComparison.UseVisualStyleBackColor = false;
		this.btnshowComparison.Click += new System.EventHandler(btnshow_Click);
		this.GpsNoToComparsion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.GpsNoToComparsion.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GpsNoToComparsion.Location = new System.Drawing.Point(208, 36);
		this.GpsNoToComparsion.Name = "GpsNoToComparsion";
		this.GpsNoToComparsion.ShortcutsEnabled = false;
		this.GpsNoToComparsion.Size = new System.Drawing.Size(111, 23);
		this.GpsNoToComparsion.TabIndex = 1;
		this.GpsNoToComparsion.KeyPress += new System.Windows.Forms.KeyPressEventHandler(GpsNoTo_KeyPress);
		this.GpsNoFromComparison.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.GpsNoFromComparison.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GpsNoFromComparison.Location = new System.Drawing.Point(95, 36);
		this.GpsNoFromComparison.Name = "GpsNoFromComparison";
		this.GpsNoFromComparison.ShortcutsEnabled = false;
		this.GpsNoFromComparison.Size = new System.Drawing.Size(111, 23);
		this.GpsNoFromComparison.TabIndex = 0;
		this.GpsNoFromComparison.KeyPress += new System.Windows.Forms.KeyPressEventHandler(GpsNoFrom_KeyPress);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(9, 40);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(88, 15);
		this.label3.TabIndex = 165;
		this.label3.Text = "Doc From && To";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 25);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1176, 31);
		this.panel3.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 6);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(198, 19);
		this.label23.TabIndex = 4;
		this.label23.Text = "Production Comparison";
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1176, 25);
		this.panel2.TabIndex = 0;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNew, this.BtnRefreshComparison, this.toolStripButton1 });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1176, 27);
		this.toolStrip3.TabIndex = 7;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(68, 24);
		this.btnNew.Text = "&Reset";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.BtnRefreshComparison.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshComparison.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshComparison.Name = "BtnRefreshComparison";
		this.BtnRefreshComparison.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshComparison.Text = "&Refresh";
		this.BtnRefreshComparison.Click += new System.EventHandler(BtnRefreshComparison_Click);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(152, 24);
		this.toolStripButton1.Text = "604-Print-Register";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1182, 532);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Production Summary & Detail ";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdInput);
		this.panel7.Controls.Add(this.panel10);
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(3, 3);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1176, 526);
		this.panel7.TabIndex = 0;
		this.grdInput.AllowEdit = (InheritableBoolean)2;
		this.grdInput.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdInput_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInput.DesignTimeLayout = grdInput_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInput).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdInput.DynamicFiltering = true;
		this.grdInput.FilterMode = (FilterMode)2;
		this.grdInput.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdInput).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInput.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.grdInput).Location = new System.Drawing.Point(0, 171);
		((System.Windows.Forms.Control)(object)this.grdInput).Name = "grdInput";
		((ScrollableBase)this.grdInput).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInput).Size = new System.Drawing.Size(1176, 355);
		((System.Windows.Forms.Control)(object)this.grdInput).TabIndex = 0;
		this.grdInput.TotalRow = (InheritableBoolean)1;
		this.grdInput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInput).VisualStyle = (VisualStyle)4;
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label7);
		this.panel10.Controls.Add(this.ctrlGrdBar2);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.ForeColor = System.Drawing.Color.Black;
		this.panel10.Location = new System.Drawing.Point(0, 147);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1176, 24);
		this.panel10.TabIndex = 2;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(4, 2);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(132, 18);
		this.label7.TabIndex = 4;
		this.label7.Text = "Filtered Records";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1508, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdInput;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(55, 27);
		this.ctrlGrdBar2.TabIndex = 11;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel9.Controls.Add(this.label17);
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranchNameSummary);
		this.panel9.Controls.Add(this.PanelValues);
		this.panel9.Controls.Add(this.panel11);
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPlantSummary);
		this.panel9.Controls.Add(this.label10);
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUomSummary);
		this.panel9.Controls.Add(this.label9);
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemNameSummary);
		this.panel9.Controls.Add(this.label8);
		this.panel9.Controls.Add(this.RdGainLoss);
		this.panel9.Controls.Add(this.RdInputDetail);
		this.panel9.Controls.Add(this.label6);
		this.panel9.Controls.Add(this.datToDateSummary);
		this.panel9.Controls.Add(this.datFromDateSummary);
		this.panel9.Controls.Add(this.btnShowSummary);
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.cmbJobOrderSummary);
		this.panel9.Controls.Add(this.label5);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 26);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1176, 121);
		this.panel9.TabIndex = 1;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(5, 40);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(79, 15);
		this.label17.TabIndex = 601;
		this.label17.Text = "Branch Name";
		this.CmbBranchNameSummary.AllowNull = (DefaultableBoolean)1;
		this.CmbBranchNameSummary.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranchNameSummary.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranchNameSummary.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranchNameSummary).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbBranchNameSummary).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranchNameSummary).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranchNameSummary).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranchNameSummary).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranchNameSummary).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranchNameSummary).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbBranchNameSummary).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranchNameSummary).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranchNameSummary).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranchNameSummary).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbBranchNameSummary).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranchNameSummary).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranchNameSummary).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance14).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranchNameSummary).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbBranchNameSummary).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranchNameSummary).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranchNameSummary).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranchNameSummary).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbBranchNameSummary).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranchNameSummary).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranchNameSummary).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbBranchNameSummary).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranchNameSummary).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbBranchNameSummary).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranchNameSummary).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranchNameSummary).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranchNameSummary).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbBranchNameSummary).Location = new System.Drawing.Point(86, 34);
		((System.Windows.Forms.Control)(object)this.CmbBranchNameSummary).Name = "CmbBranchNameSummary";
		((System.Windows.Forms.Control)(object)this.CmbBranchNameSummary).Size = new System.Drawing.Size(233, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranchNameSummary).TabIndex = 600;
		((System.Windows.Forms.Control)(object)this.CmbBranchNameSummary).Leave += new System.EventHandler(CmbBranchNameSummary_Leave);
		this.PanelValues.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PanelValues.Controls.Add(this.panel52);
		this.PanelValues.Controls.Add(this.lblSumOfOutPut);
		this.PanelValues.Controls.Add(this.lblSumofTotalIssue);
		this.PanelValues.Controls.Add(this.lblGainLossDiff);
		this.PanelValues.Controls.Add(this.lblGainLossStatus);
		this.PanelValues.Controls.Add(this.lblPercent);
		this.PanelValues.Location = new System.Drawing.Point(747, 38);
		this.PanelValues.Name = "PanelValues";
		this.PanelValues.Size = new System.Drawing.Size(424, 78);
		this.PanelValues.TabIndex = 196;
		this.panel52.BackColor = System.Drawing.Color.White;
		this.panel52.Controls.Add(this.lblTotalIssueWeight);
		this.panel52.Controls.Add(this.lblProdTotalWeight);
		this.panel52.Controls.Add(this.lblGainLossKg);
		this.panel52.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel52.Location = new System.Drawing.Point(0, 0);
		this.panel52.Name = "panel52";
		this.panel52.Size = new System.Drawing.Size(422, 24);
		this.panel52.TabIndex = 0;
		this.lblTotalIssueWeight.AutoSize = true;
		this.lblTotalIssueWeight.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalIssueWeight.ForeColor = System.Drawing.Color.Teal;
		this.lblTotalIssueWeight.Location = new System.Drawing.Point(8, 3);
		this.lblTotalIssueWeight.Name = "lblTotalIssueWeight";
		this.lblTotalIssueWeight.Size = new System.Drawing.Size(104, 17);
		this.lblTotalIssueWeight.TabIndex = 2;
		this.lblTotalIssueWeight.Text = "Total Issue (Kg)";
		this.lblProdTotalWeight.AutoSize = true;
		this.lblProdTotalWeight.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
		this.lblProdTotalWeight.ForeColor = System.Drawing.Color.Teal;
		this.lblProdTotalWeight.Location = new System.Drawing.Point(125, 3);
		this.lblProdTotalWeight.Name = "lblProdTotalWeight";
		this.lblProdTotalWeight.Size = new System.Drawing.Size(117, 17);
		this.lblProdTotalWeight.TabIndex = 182;
		this.lblProdTotalWeight.Text = "Total OutPut (Kg)";
		this.lblGainLossKg.AutoSize = true;
		this.lblGainLossKg.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
		this.lblGainLossKg.ForeColor = System.Drawing.Color.Teal;
		this.lblGainLossKg.Location = new System.Drawing.Point(251, 3);
		this.lblGainLossKg.Name = "lblGainLossKg";
		this.lblGainLossKg.Size = new System.Drawing.Size(99, 17);
		this.lblGainLossKg.TabIndex = 183;
		this.lblGainLossKg.Text = "Gain/Loss (Kg)";
		this.lblSumOfOutPut.BackColor = System.Drawing.Color.White;
		this.lblSumOfOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSumOfOutPut.Location = new System.Drawing.Point(125, 35);
		this.lblSumOfOutPut.Name = "lblSumOfOutPut";
		this.lblSumOfOutPut.ReadOnly = true;
		this.lblSumOfOutPut.Size = new System.Drawing.Size(116, 23);
		this.lblSumOfOutPut.TabIndex = 8;
		this.lblSumOfOutPut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.lblSumofTotalIssue.BackColor = System.Drawing.Color.White;
		this.lblSumofTotalIssue.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSumofTotalIssue.Location = new System.Drawing.Point(2, 35);
		this.lblSumofTotalIssue.Name = "lblSumofTotalIssue";
		this.lblSumofTotalIssue.ReadOnly = true;
		this.lblSumofTotalIssue.Size = new System.Drawing.Size(116, 23);
		this.lblSumofTotalIssue.TabIndex = 8;
		this.lblSumofTotalIssue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.lblGainLossDiff.BackColor = System.Drawing.Color.White;
		this.lblGainLossDiff.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblGainLossDiff.Location = new System.Drawing.Point(250, 35);
		this.lblGainLossDiff.Name = "lblGainLossDiff";
		this.lblGainLossDiff.ReadOnly = true;
		this.lblGainLossDiff.Size = new System.Drawing.Size(101, 23);
		this.lblGainLossDiff.TabIndex = 2;
		this.lblGainLossDiff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.lblGainLossStatus.AutoSize = true;
		this.lblGainLossStatus.Location = new System.Drawing.Point(360, 48);
		this.lblGainLossStatus.Name = "lblGainLossStatus";
		this.lblGainLossStatus.Size = new System.Drawing.Size(37, 16);
		this.lblGainLossStatus.TabIndex = 187;
		this.lblGainLossStatus.Text = "lable";
		this.lblPercent.AutoSize = true;
		this.lblPercent.Location = new System.Drawing.Point(360, 30);
		this.lblPercent.Name = "lblPercent";
		this.lblPercent.Size = new System.Drawing.Size(37, 16);
		this.lblPercent.TabIndex = 188;
		this.lblPercent.Text = "lable";
		this.panel11.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel11.Controls.Add(this.label11);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1176, 26);
		this.panel11.TabIndex = 195;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(4, 4);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(232, 19);
		this.label11.TabIndex = 4;
		this.label11.Text = "Production Summary Detail";
		this.CmbPlantSummary.AllowNull = (DefaultableBoolean)1;
		this.CmbPlantSummary.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPlantSummary.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPlantSummary.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPlantSummary).DisplayLayout.Appearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbPlantSummary).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPlantSummary).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantSummary).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantSummary).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance85;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantSummary).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantSummary).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbPlantSummary).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPlantSummary).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPlantSummary).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPlantSummary).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbPlantSummary).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPlantSummary).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantSummary).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance91).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPlantSummary).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbPlantSummary).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPlantSummary).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance92).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantSummary).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPlantSummary).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbPlantSummary).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPlantSummary).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPlantSummary).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbPlantSummary).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPlantSummary).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbPlantSummary).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPlantSummary).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPlantSummary).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPlantSummary).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPlantSummary.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPlantSummary).Location = new System.Drawing.Point(391, 34);
		((System.Windows.Forms.Control)(object)this.CmbPlantSummary).Name = "CmbPlantSummary";
		((System.Windows.Forms.Control)(object)this.CmbPlantSummary).Size = new System.Drawing.Size(223, 26);
		((System.Windows.Forms.Control)(object)this.CmbPlantSummary).TabIndex = 5;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(323, 40);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(69, 15);
		this.label10.TabIndex = 194;
		this.label10.Text = "Plant Name";
		this.CmbPackUomSummary.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUomSummary.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUomSummary.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUomSummary.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUomSummary).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbPackUomSummary).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUomSummary).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUomSummary).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUomSummary).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUomSummary).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUomSummary).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbPackUomSummary).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUomSummary).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUomSummary).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUomSummary).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbPackUomSummary).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUomSummary).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUomSummary).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance30).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUomSummary).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbPackUomSummary).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUomSummary).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUomSummary).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUomSummary).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbPackUomSummary).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUomSummary).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUomSummary).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbPackUomSummary).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUomSummary).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbPackUomSummary).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUomSummary).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUomSummary).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUomSummary).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUomSummary.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUomSummary).Location = new System.Drawing.Point(391, 90);
		((System.Windows.Forms.Control)(object)this.CmbPackUomSummary).Name = "CmbPackUomSummary";
		((System.Windows.Forms.Control)(object)this.CmbPackUomSummary).Size = new System.Drawing.Size(159, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUomSummary).TabIndex = 2;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(323, 96);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(61, 15);
		this.label9.TabIndex = 192;
		this.label9.Text = "Pack Uom";
		this.CmbItemNameSummary.AllowNull = (DefaultableBoolean)1;
		this.CmbItemNameSummary.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemNameSummary.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemNameSummary.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemNameSummary).DisplayLayout.Appearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbItemNameSummary).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemNameSummary).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameSummary).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameSummary).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance45;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameSummary).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameSummary).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbItemNameSummary).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemNameSummary).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemNameSummary).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemNameSummary).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbItemNameSummary).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemNameSummary).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameSummary).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance51).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemNameSummary).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbItemNameSummary).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemNameSummary).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameSummary).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemNameSummary).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbItemNameSummary).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemNameSummary).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemNameSummary).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbItemNameSummary).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemNameSummary).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbItemNameSummary).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemNameSummary).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemNameSummary).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemNameSummary).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemNameSummary.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemNameSummary).Location = new System.Drawing.Point(391, 62);
		((System.Windows.Forms.Control)(object)this.CmbItemNameSummary).Name = "CmbItemNameSummary";
		((System.Windows.Forms.Control)(object)this.CmbItemNameSummary).Size = new System.Drawing.Size(223, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemNameSummary).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbItemNameSummary).Leave += new System.EventHandler(CmbItemName_Leave);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(323, 68);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(67, 15);
		this.label8.TabIndex = 190;
		this.label8.Text = "Item Name";
		this.RdGainLoss.AutoSize = true;
		this.RdGainLoss.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdGainLoss.Location = new System.Drawing.Point(620, 66);
		this.RdGainLoss.Name = "RdGainLoss";
		this.RdGainLoss.Size = new System.Drawing.Size(132, 19);
		this.RdGainLoss.TabIndex = 7;
		this.RdGainLoss.TabStop = true;
		this.RdGainLoss.Text = "Gain/Loss Summary";
		this.RdGainLoss.UseVisualStyleBackColor = true;
		this.RdInputDetail.AutoSize = true;
		this.RdInputDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdInputDetail.Location = new System.Drawing.Point(620, 38);
		this.RdInputDetail.Name = "RdInputDetail";
		this.RdInputDetail.Size = new System.Drawing.Size(90, 19);
		this.RdInputDetail.TabIndex = 6;
		this.RdInputDetail.TabStop = true;
		this.RdInputDetail.Text = "InPut_Detail";
		this.RdInputDetail.UseVisualStyleBackColor = true;
		this.RdInputDetail.CheckedChanged += new System.EventHandler(RdInputDetail_CheckedChanged);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(5, 68);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(79, 15);
		this.label6.TabIndex = 179;
		this.label6.Text = "From,To Date";
		this.datToDateSummary.CustomFormat = "dd-MMM-yy";
		this.datToDateSummary.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDateSummary.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDateSummary.Location = new System.Drawing.Point(203, 64);
		this.datToDateSummary.Name = "datToDateSummary";
		this.datToDateSummary.Size = new System.Drawing.Size(116, 23);
		this.datToDateSummary.TabIndex = 1;
		this.datFromDateSummary.CustomFormat = "dd-MMM-yy";
		this.datFromDateSummary.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDateSummary.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDateSummary.Location = new System.Drawing.Point(86, 64);
		this.datFromDateSummary.Name = "datFromDateSummary";
		this.datFromDateSummary.Size = new System.Drawing.Size(116, 23);
		this.datFromDateSummary.TabIndex = 0;
		this.btnShowSummary.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnShowSummary.FlatAppearance.BorderSize = 0;
		this.btnShowSummary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowSummary.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowSummary.ForeColor = System.Drawing.Color.White;
		this.btnShowSummary.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnShowSummary.Location = new System.Drawing.Point(553, 90);
		this.btnShowSummary.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowSummary.Name = "btnShowSummary";
		this.btnShowSummary.Size = new System.Drawing.Size(61, 26);
		this.btnShowSummary.TabIndex = 8;
		this.btnShowSummary.Text = "Show";
		this.btnShowSummary.UseVisualStyleBackColor = false;
		this.btnShowSummary.Click += new System.EventHandler(btnShowInput_Click);
		this.cmbJobOrderSummary.AllowNull = (DefaultableBoolean)1;
		this.cmbJobOrderSummary.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJobOrderSummary.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJobOrderSummary.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJobOrderSummary).DisplayLayout.Appearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbJobOrderSummary).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJobOrderSummary).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobOrderSummary).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobOrderSummary).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance59;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobOrderSummary).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobOrderSummary).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbJobOrderSummary).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbJobOrderSummary).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbJobOrderSummary).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbJobOrderSummary).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbJobOrderSummary).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbJobOrderSummary).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobOrderSummary).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance64).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbJobOrderSummary).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbJobOrderSummary).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbJobOrderSummary).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance65).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobOrderSummary).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbJobOrderSummary).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbJobOrderSummary).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbJobOrderSummary).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbJobOrderSummary).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbJobOrderSummary).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbJobOrderSummary).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbJobOrderSummary).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbJobOrderSummary).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbJobOrderSummary).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbJobOrderSummary).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbJobOrderSummary.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbJobOrderSummary).Location = new System.Drawing.Point(86, 90);
		((System.Windows.Forms.Control)(object)this.cmbJobOrderSummary).Name = "cmbJobOrderSummary";
		((System.Windows.Forms.Control)(object)this.cmbJobOrderSummary).Size = new System.Drawing.Size(233, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobOrderSummary).TabIndex = 3;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(5, 96);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(59, 15);
		this.label5.TabIndex = 177;
		this.label5.Text = "Job Order";
		this.panel8.Controls.Add(this.toolStrip1);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1176, 26);
		this.panel8.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNewSummary, this.BtnRefreshSummary, this.btnPrint607Register, this.tsDropDownGainLoss });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1176, 27);
		this.toolStrip1.TabIndex = 10;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNewSummary.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewSummary.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewSummary.Name = "btnNewSummary";
		this.btnNewSummary.Size = new System.Drawing.Size(68, 24);
		this.btnNewSummary.Text = "&Reset";
		this.btnNewSummary.Click += new System.EventHandler(btnNewInput_Click);
		this.BtnRefreshSummary.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshSummary.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshSummary.Name = "BtnRefreshSummary";
		this.BtnRefreshSummary.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshSummary.Text = "&Refresh";
		this.BtnRefreshSummary.Click += new System.EventHandler(BtnRefreshSummary_Click);
		this.btnPrint607Register.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint607Register.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint607Register.Name = "btnPrint607Register";
		this.btnPrint607Register.Size = new System.Drawing.Size(195, 24);
		this.btnPrint607Register.Text = "607-InPutDetail_Register";
		this.btnPrint607Register.Click += new System.EventHandler(btnPrint607Register_Click);
		this.tsDropDownGainLoss.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.tsDropDownGainLoss.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsDropDownGainLoss.Name = "tsDropDownGainLoss";
		this.tsDropDownGainLoss.Size = new System.Drawing.Size(138, 24);
		this.tsDropDownGainLoss.Text = "Gain/Loss Print";
		this.tsDropDownGainLoss.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(tsDropDownGainLoss_DropDownItemClicked);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1190, 561);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "FoodProductionComparisonRpt";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(FoodProductionComparisonRpt_FormClosing);
		base.Load += new System.EventHandler(frmGPOutward_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmGPOutward_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchNameComparison).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPlantComparison).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNoComprison).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbProductionTypeComparison).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInput).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranchNameSummary).EndInit();
		this.PanelValues.ResumeLayout(false);
		this.PanelValues.PerformLayout();
		this.panel52.ResumeLayout(false);
		this.panel52.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantSummary).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUomSummary).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameSummary).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobOrderSummary).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
