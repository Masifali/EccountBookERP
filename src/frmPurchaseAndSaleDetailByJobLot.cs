// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Documents.Utils;
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
namespace Architecture.WinApp.Inventory_Reports
{

public class frmPurchaseAndSaleDetailByJobLot : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtBindCombo = new DataTable();

	private DataTable dtBranch = new DataTable();

	private DataTable dtRegister = new DataTable();

	public int ParentId;

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private Panel panel2;

	private Label label27;

	private Panel panel3;

	private Label label23;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX DataGridHistory;

	private ToolStripButton btnRegister;

	private Label label6;

	private TextBox txtInvoiceNo;

	private Button btnshow;

	private Panel panel5;

	private Label label2;

	private ToolStripButton toolStripButton1;

	public DateTimePicker dateGdnToDate;

	public DateTimePicker datGdnFromDate;

	private Label label12;

	internal UltraCombo cmbjoblot;

	internal UltraCombo cmbperemeter;

	private Label label15;

	private GroupBox groupBox1;

	private ToolStripButton toolStripButton2;

	private Label label17;

	internal UltraCombo cmbBranchName;

	internal GridEX grdSummaryByJob;

	private Label label1;

	private Panel panel6;

	private Panel panel7;

	public frmPurchaseAndSaleDetailByJobLot(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmPurchaseAndSaleDetailByJobLot_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1100, 500);
			ctrlGrdBar1.mGridChooseFielder.Visible = false;
			ctrlGrdBar1.mGridSaveLayouts.Visible = false;
			ctrlGrdBar1.removeLayoutToolStripMenuItem.Visible = false;
			BranchesFill();
			ParameterFill();
			ComboFill();
			((UltraGridBase)cmbperemeter).Rows[1].Activate();
			cmbperemeter.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ParameterFill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbperemeter, "Id", "Parameters", "Date Type", ZeroIndex: false);
			}
			else
			{
				((Control)(object)cmbperemeter).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbperemeter_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbperemeter.Value) == 1)
			{
				datGdnFromDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 2)
			{
				datGdnFromDate.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				datGdnFromDate.Value = first;
				dateGdnToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				datGdnFromDate.Value = first2;
				dateGdnToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 5)
			{
				datGdnFromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
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
			((UltraGridBase)cmbBranchName).DataSource = null;
			dtBranch = BranchesAllocationToUser.GetBranchsAllocatedToUser(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID);
			if (dtBranch.Rows.Count > 0)
			{
				DDL.BindDDL(dtBranch, cmbBranchName, "BranchId", "BranchName", "Branch Name", ZeroIndex: false);
				UltraGridColumn obj = ((UltraGridBase)cmbBranchName).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj).Key = "Selected";
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
				cmbBranchName.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)cmbBranchName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((Control)(object)cmbBranchName).Text = UserAccount.BranchName.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboFill()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		try
		{
			DataTable dtJob = new DataTable();
			dtJob.Columns.Add("Id");
			dtJob.Columns.Add("name");
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeIds = "56,57,95,99";
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) + ",").Split(',');
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
			dtBindCombo = StocksReport.Inventory_StockEvalautionDetail_DropDownAndLists(obj);
			if (dtBindCombo.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtBindCombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtBindCombo.Rows[i]["ActivityType"]) == "JobLot")
				{
					dtJob.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
				}
			}
			DDL.BindDDL(dtJob, cmbjoblot, "Id", "name", "Job Lot", ZeroIndex: true);
			((UltraGridBase)cmbjoblot).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbjoblot).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void cmbBranchName_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbBranchName).Text != string.Empty && ((UltraGridBase)cmbBranchName).ActiveRow != null)
			{
				ComboFill();
				return;
			}
			((Control)(object)cmbjoblot).Text = string.Empty;
			((UltraGridBase)cmbjoblot).ActiveRow = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters gdn = new ReportsParameters();
			gdn.OrganizationId = UserAccount.OrganizationId;
			gdn.CompanyId = UserAccount.CompanyId;
			gdn.FromDate = Conversion.ToDateTime((object)datGdnFromDate.Value);
			gdn.ToDate = Conversion.ToDateTime((object)dateGdnToDate.Value);
			gdn.JobLotId = Conversion.ToInt(cmbjoblot.Value);
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				gdn.BranchesIds = BranchIds;
				dtRegister = StocksReport.PurchaseAndSaleDetailByJobLot(gdn);
				FillGridHistory();
				FillGridSummary();
				return;
			}
			cmbBranchName.Focus();
			throw new Exception("Select Branch First");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillGridHistory()
	{
		try
		{
			if (dtRegister.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("DocDate", typeof(DateTime));
				dtcol.Columns.Add("DocNo", typeof(int));
				dtcol.Columns.Add("DocumentTypeId", typeof(int));
				dtcol.Columns.Add("DocType", typeof(string));
				dtcol.Columns.Add("TransType", typeof(string));
				dtcol.Columns.Add("SupplierCustomerId", typeof(int));
				dtcol.Columns.Add("PartyName", typeof(string));
				dtcol.Columns.Add("WarehouseId", typeof(int));
				dtcol.Columns.Add("WareHouseName", typeof(string));
				dtcol.Columns.Add("ItemId", typeof(int));
				dtcol.Columns.Add("ItemName", typeof(string));
				dtcol.Columns.Add("ItemUOMId", typeof(int));
				dtcol.Columns.Add("PackUom", typeof(string));
				dtcol.Columns.Add("CropYear", typeof(string));
				dtcol.Columns.Add("JobLotId", typeof(int));
				dtcol.Columns.Add("JobLot", typeof(string));
				dtcol.Columns.Add("PackingTypeId", typeof(int));
				dtcol.Columns.Add("PackingType", typeof(string));
				dtcol.Columns.Add("InQty", typeof(decimal));
				dtcol.Columns.Add("InWeight", typeof(decimal));
				dtcol.Columns.Add("ItemRate", typeof(decimal));
				dtcol.Columns.Add("RateUom", typeof(string));
				dtcol.Columns.Add("InAmount", typeof(decimal));
				dtcol.Columns.Add("OutQty", typeof(decimal));
				dtcol.Columns.Add("OutWeight", typeof(decimal));
				dtcol.Columns.Add("OutAmount", typeof(decimal));
				dtcol.Rows.Clear();
				for (int i = 0; i < dtRegister.Rows.Count; i++)
				{
					dtcol.Rows.Add(dtRegister.Rows[i]["Id"], dtRegister.Rows[i]["DocDate"], dtRegister.Rows[i]["DocNo"], dtRegister.Rows[i]["DocumentTypeId"], dtRegister.Rows[i]["DocType"], dtRegister.Rows[i]["TransType"], dtRegister.Rows[i]["SupplierCustomerId"], dtRegister.Rows[i]["PartyName"], dtRegister.Rows[i]["WarehouseId"], dtRegister.Rows[i]["WareHouseName"], dtRegister.Rows[i]["ItemId"], dtRegister.Rows[i]["ItemName"], dtRegister.Rows[i]["ItemUOMId"], dtRegister.Rows[i]["PackUom"], dtRegister.Rows[i]["CropYear"], dtRegister.Rows[i]["JobLotId"], dtRegister.Rows[i]["JobLotDescription"], dtRegister.Rows[i]["PackingTypeId"], dtRegister.Rows[i]["PackTypeDesc"], dtRegister.Rows[i]["InQty"], dtRegister.Rows[i]["InWeight"], dtRegister.Rows[i]["ItemRate"], dtRegister.Rows[i]["RateUom"], dtRegister.Rows[i]["InAmount"], dtRegister.Rows[i]["OutQty"], dtRegister.Rows[i]["OutWeight"], dtRegister.Rows[i]["OutAmount"]);
				}
				DataGridHistory.DataSource = dtcol;
				DataGridHistory.RetrieveStructure();
				grdSetting();
			}
			else
			{
				DataGridHistory.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillGridSummary()
	{
		try
		{
			if (dtRegister.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("JobLotId", typeof(int));
				dtcol.Columns.Add("JobLot", typeof(string));
				dtcol.Columns.Add("InQty", typeof(decimal));
				dtcol.Columns.Add("InWeight", typeof(decimal));
				dtcol.Columns.Add("InAmount", typeof(decimal));
				dtcol.Columns.Add("OutQty", typeof(decimal));
				dtcol.Columns.Add("OutWeight", typeof(decimal));
				dtcol.Columns.Add("OutAmount", typeof(decimal));
				dtcol.Columns.Add("GrossProfitLoss", typeof(decimal));
				dtcol.Rows.Clear();
				HashSet<int> list = new HashSet<int>();
				for (int i = 0; i < dtRegister.Rows.Count; i++)
				{
					if (!list.Contains(Conversion.ToInt(dtRegister.Rows[i]["JobLotId"])))
					{
						dtcol.Rows.Add(dtRegister.Rows[i]["JobLotId"], dtRegister.Rows[i]["JobLotDescription"], dtRegister.Rows[i]["TotalInQtyByJob"], dtRegister.Rows[i]["TotalInWeightByJob"], dtRegister.Rows[i]["TotalInAmountByJob"], dtRegister.Rows[i]["TotalOutQtyByJob"], dtRegister.Rows[i]["TotalOutWeightByJob"], dtRegister.Rows[i]["TotalOutAmountByJob"], dtRegister.Rows[i]["GrossProfitLossByJob"]);
						list.Add(Conversion.ToInt(dtRegister.Rows[i]["JobLotId"]));
					}
				}
				grdSummaryByJob.DataSource = dtcol;
				grdSummaryByJob.RetrieveStructure();
				SummaryGridSetting();
			}
			else
			{
				grdSummaryByJob.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSetting()
	{
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["SupplierCustomerId"].Visible = false;
			DataGridHistory.RootTable.Columns["WarehouseId"].Visible = false;
			DataGridHistory.RootTable.Columns["ItemId"].Visible = false;
			DataGridHistory.RootTable.Columns["ItemUOMId"].Visible = false;
			DataGridHistory.RootTable.Columns["JobLotId"].Visible = false;
			DataGridHistory.RootTable.Columns["PackingTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["Id"].Width = 20;
			DataGridHistory.RootTable.Columns["DocDate"].Width = 70;
			DataGridHistory.RootTable.Columns["DocNo"].Width = 60;
			DataGridHistory.RootTable.Columns["DocumentTypeId"].Width = 120;
			DataGridHistory.RootTable.Columns["DocType"].Width = 70;
			DataGridHistory.RootTable.Columns["TransType"].Width = 80;
			DataGridHistory.RootTable.Columns["SupplierCustomerId"].Width = 120;
			DataGridHistory.RootTable.Columns["PartyName"].Width = 120;
			DataGridHistory.RootTable.Columns["WarehouseId"].Width = 120;
			DataGridHistory.RootTable.Columns["WareHouseName"].Width = 120;
			DataGridHistory.RootTable.Columns["ItemId"].Width = 60;
			DataGridHistory.RootTable.Columns["ItemName"].Width = 150;
			DataGridHistory.RootTable.Columns["ItemUOMId"].Width = 60;
			DataGridHistory.RootTable.Columns["PackUom"].Width = 60;
			DataGridHistory.RootTable.Columns["CropYear"].Width = 70;
			DataGridHistory.RootTable.Columns["JobLotId"].Width = 70;
			DataGridHistory.RootTable.Columns["JobLot"].Width = 140;
			DataGridHistory.RootTable.Columns["PackingTypeId"].Width = 120;
			DataGridHistory.RootTable.Columns["PackingType"].Width = 90;
			DataGridHistory.RootTable.Columns["InQty"].Width = 60;
			DataGridHistory.RootTable.Columns["InWeight"].Width = 70;
			DataGridHistory.RootTable.Columns["ItemRate"].Width = 70;
			DataGridHistory.RootTable.Columns["RateUom"].Width = 60;
			DataGridHistory.RootTable.Columns["InAmount"].Width = 90;
			DataGridHistory.RootTable.Columns["OutQty"].Width = 90;
			DataGridHistory.RootTable.Columns["OutWeight"].Width = 100;
			DataGridHistory.RootTable.Columns["OutAmount"].Width = 100;
			DataGridHistory.RootTable.Columns["InQty"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["InQty"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["InQty"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["InQty"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["InWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["InWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["InWeight"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["InWeight"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistory.RootTable.Columns["ItemRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistory.RootTable.Columns["InAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["InAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["InAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["InAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["OutQty"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["OutQty"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["OutQty"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["OutQty"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["OutWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["OutWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["OutWeight"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["OutWeight"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["OutAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["OutAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["OutAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["OutAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void SummaryGridSetting()
	{
		try
		{
			grdSummaryByJob.RootTable.Columns["JobLotId"].Visible = false;
			grdSummaryByJob.RootTable.Columns["JobLotId"].Width = 70;
			grdSummaryByJob.RootTable.Columns["JobLot"].Width = 140;
			grdSummaryByJob.RootTable.Columns["InQty"].Width = 70;
			grdSummaryByJob.RootTable.Columns["InWeight"].Width = 80;
			grdSummaryByJob.RootTable.Columns["InAmount"].Width = 110;
			grdSummaryByJob.RootTable.Columns["OutQty"].Width = 80;
			grdSummaryByJob.RootTable.Columns["OutWeight"].Width = 100;
			grdSummaryByJob.RootTable.Columns["OutAmount"].Width = 110;
			grdSummaryByJob.RootTable.Columns["GrossProfitLoss"].Width = 130;
			grdSummaryByJob.RootTable.Columns["InQty"].AggregateFunction = (AggregateFunction)2;
			grdSummaryByJob.RootTable.Columns["InQty"].TextAlignment = (TextAlignment)3;
			grdSummaryByJob.RootTable.Columns["InQty"].FormatString = "#,##0.##";
			grdSummaryByJob.RootTable.Columns["InQty"].TotalFormatString = "#,##0.##";
			grdSummaryByJob.RootTable.Columns["InWeight"].AggregateFunction = (AggregateFunction)2;
			grdSummaryByJob.RootTable.Columns["InWeight"].TextAlignment = (TextAlignment)3;
			grdSummaryByJob.RootTable.Columns["InWeight"].FormatString = "#,##0.##";
			grdSummaryByJob.RootTable.Columns["InWeight"].TotalFormatString = "#,##0.##";
			grdSummaryByJob.RootTable.Columns["InAmount"].AggregateFunction = (AggregateFunction)2;
			grdSummaryByJob.RootTable.Columns["InAmount"].TextAlignment = (TextAlignment)3;
			grdSummaryByJob.RootTable.Columns["InAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdSummaryByJob.RootTable.Columns["InAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdSummaryByJob.RootTable.Columns["OutQty"].AggregateFunction = (AggregateFunction)2;
			grdSummaryByJob.RootTable.Columns["OutQty"].TextAlignment = (TextAlignment)3;
			grdSummaryByJob.RootTable.Columns["OutQty"].FormatString = "#,##0.##";
			grdSummaryByJob.RootTable.Columns["OutQty"].TotalFormatString = "#,##0.##";
			grdSummaryByJob.RootTable.Columns["OutWeight"].AggregateFunction = (AggregateFunction)2;
			grdSummaryByJob.RootTable.Columns["OutWeight"].TextAlignment = (TextAlignment)3;
			grdSummaryByJob.RootTable.Columns["OutWeight"].FormatString = "#,##0.##";
			grdSummaryByJob.RootTable.Columns["OutWeight"].TotalFormatString = "#,##0.##";
			grdSummaryByJob.RootTable.Columns["OutAmount"].AggregateFunction = (AggregateFunction)2;
			grdSummaryByJob.RootTable.Columns["OutAmount"].TextAlignment = (TextAlignment)3;
			grdSummaryByJob.RootTable.Columns["OutAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdSummaryByJob.RootTable.Columns["OutAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdSummaryByJob.RootTable.Columns["GrossProfitLoss"].AggregateFunction = (AggregateFunction)2;
			grdSummaryByJob.RootTable.Columns["GrossProfitLoss"].TextAlignment = (TextAlignment)3;
			grdSummaryByJob.RootTable.Columns["GrossProfitLoss"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdSummaryByJob.RootTable.Columns["GrossProfitLoss"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
	}

	private void Reset()
	{
		try
		{
			((Control)(object)cmbjoblot).Text = string.Empty;
			cmbperemeter.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			BranchesFill();
			ComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		try
		{
			Close();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmPurchaseAndSaleDetailByJobLot_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnshow_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "DataGridHistory")
			{
				cmbperemeter.Focus();
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
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRegister_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtRegister.Rows.Count == 0 || dtRegister == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtRegister, "56_08_PurchaseAndSaleDetailByJobLot.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmPurchaseAndSaleDetailByJobLot_KeyDown(object sender, KeyEventArgs e)
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
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				toolStripButton1_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)DataGridHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				cmbperemeter.Focus();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbperemeter.Focus();
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnRegister_Click(null, null);
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
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
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

	private void toolStripButton2_Click(object sender, EventArgs e)
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
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Expected O, but got Unknown
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Expected O, but got Unknown
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Expected O, but got Unknown
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Expected O, but got Unknown
		//IL_057a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0584: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		GridEXLayout grdSummaryByJob_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Inventory_Reports.frmPurchaseAndSaleDetailByJobLot));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.dateGdnToDate = new System.Windows.Forms.DateTimePicker();
		this.datGdnFromDate = new System.Windows.Forms.DateTimePicker();
		this.cmbBranchName = new UltraCombo();
		this.cmbperemeter = new UltraCombo();
		this.label17 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.cmbjoblot = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.txtInvoiceNo = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.btnRegister = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.grdSummaryByJob = new GridEX();
		this.label1 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel5.SuspendLayout();
		this.panel2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbjoblot).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSummaryByJob).BeginInit();
		this.panel6.SuspendLayout();
		this.panel7.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.toolStrip3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(4);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(984, 461);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 205);
		this.panel4.Margin = new System.Windows.Forms.Padding(4);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(984, 256);
		this.panel4.TabIndex = 7;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(984, 231);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.ForeColor = System.Drawing.Color.White;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(4);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(984, 25);
		this.panel5.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(5, 3);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(142, 19);
		this.label2.TabIndex = 3;
		this.label2.Text = "Filtered Records";
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Controls.Add(this.groupBox1);
		this.panel2.Controls.Add(this.label6);
		this.panel2.Controls.Add(this.txtInvoiceNo);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 53);
		this.panel2.Margin = new System.Windows.Forms.Padding(4);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(984, 152);
		this.panel2.TabIndex = 0;
		this.groupBox1.Controls.Add(this.dateGdnToDate);
		this.groupBox1.Controls.Add(this.datGdnFromDate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbperemeter);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbjoblot);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(342, 152);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.dateGdnToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.dateGdnToDate.CustomFormat = "dd-MMM-yy";
		this.dateGdnToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.dateGdnToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.dateGdnToDate.Location = new System.Drawing.Point(217, 70);
		this.dateGdnToDate.Margin = new System.Windows.Forms.Padding(4);
		this.dateGdnToDate.Name = "dateGdnToDate";
		this.dateGdnToDate.Size = new System.Drawing.Size(118, 23);
		this.dateGdnToDate.TabIndex = 3;
		this.datGdnFromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datGdnFromDate.CustomFormat = "dd-MMM-yy";
		this.datGdnFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datGdnFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datGdnFromDate.Location = new System.Drawing.Point(94, 70);
		this.datGdnFromDate.Margin = new System.Windows.Forms.Padding(4);
		this.datGdnFromDate.Name = "datGdnFromDate";
		this.datGdnFromDate.Size = new System.Drawing.Size(122, 23);
		this.datGdnFromDate.TabIndex = 2;
		this.cmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance123).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance123).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance124;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance130).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance131).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(94, 13);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(241, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Leave += new System.EventHandler(cmbBranchName_Leave);
		this.cmbperemeter.AllowNull = (DefaultableBoolean)1;
		this.cmbperemeter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbperemeter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbperemeter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance111;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance116).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbperemeter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Location = new System.Drawing.Point(94, 41);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Name = "cmbperemeter";
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Size = new System.Drawing.Size(241, 26);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).TabIndex = 1;
		this.cmbperemeter.ValueChanged += new System.EventHandler(cmbperemeter_ValueChanged);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(4, 19);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(79, 15);
		this.label17.TabIndex = 599;
		this.label17.Text = "Branch Name";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(4, 47);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(60, 15);
		this.label15.TabIndex = 594;
		this.label15.Text = "Date Type";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(4, 74);
		this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(91, 15);
		this.label27.TabIndex = 145;
		this.label27.Text = "From && To Date";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(4, 102);
		this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(43, 15);
		this.label12.TabIndex = 155;
		this.label12.Text = "JobLot";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(281, 124);
		this.btnshow.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(54, 23);
		this.btnshow.TabIndex = 5;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.btnshow.Leave += new System.EventHandler(btnshow_Leave);
		this.cmbjoblot.AllowNull = (DefaultableBoolean)1;
		this.cmbjoblot.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbjoblot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbjoblot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbjoblot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance135;
		((SpecialBoxBase)((UltraGridBase)this.cmbjoblot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbjoblot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbjoblot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbjoblot).Location = new System.Drawing.Point(94, 96);
		((System.Windows.Forms.Control)(object)this.cmbjoblot).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.cmbjoblot).Name = "cmbjoblot";
		((System.Windows.Forms.Control)(object)this.cmbjoblot).Size = new System.Drawing.Size(241, 26);
		((System.Windows.Forms.Control)(object)this.cmbjoblot).TabIndex = 4;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(1304, 251);
		this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(64, 15);
		this.label6.TabIndex = 154;
		this.label6.Text = "Invoice No";
		this.label6.Visible = false;
		this.txtInvoiceNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceNo.Location = new System.Drawing.Point(1308, 273);
		this.txtInvoiceNo.Margin = new System.Windows.Forms.Padding(4);
		this.txtInvoiceNo.Name = "txtInvoiceNo";
		this.txtInvoiceNo.Size = new System.Drawing.Size(228, 22);
		this.txtInvoiceNo.TabIndex = 5;
		this.txtInvoiceNo.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(4);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(984, 26);
		this.panel3.TabIndex = 5;
		this.label23.AutoSize = true;
		this.label23.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 3);
		this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(298, 19);
		this.label23.TabIndex = 3;
		this.label23.Text = "Purchase And Sale Detail By JobLot";
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.toolStripButton1, this.btnRegister, this.toolStripButton2 });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(984, 27);
		this.toolStrip3.TabIndex = 6;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.toolStripButton1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(80, 24);
		this.toolStripButton1.Text = "&Refresh";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.btnRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRegister.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRegister.Name = "btnRegister";
		this.btnRegister.Size = new System.Drawing.Size(132, 24);
		this.btnRegister.Text = "56_08_Register";
		this.btnRegister.Click += new System.EventHandler(btnRegister_Click);
		this.toolStripButton2.Image = Architecture.WinApp.Properties.Resources._55download;
		this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton2.Name = "toolStripButton2";
		this.toolStripButton2.Size = new System.Drawing.Size(126, 24);
		this.toolStripButton2.Text = "ShortCut Keys";
		this.toolStripButton2.Click += new System.EventHandler(toolStripButton2_Click);
		this.grdSummaryByJob.AllowEdit = (InheritableBoolean)2;
		this.grdSummaryByJob.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdSummaryByJob_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSummaryByJob.DesignTimeLayout = grdSummaryByJob_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSummaryByJob).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdSummaryByJob.DynamicFiltering = true;
		this.grdSummaryByJob.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdSummaryByJob).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSummaryByJob.GroupByBoxVisible = false;
		this.grdSummaryByJob.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdSummaryByJob).Location = new System.Drawing.Point(0, 18);
		((System.Windows.Forms.Control)(object)this.grdSummaryByJob).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdSummaryByJob).Name = "grdSummaryByJob";
		((ScrollableBase)this.grdSummaryByJob).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSummaryByJob).Size = new System.Drawing.Size(642, 134);
		((System.Windows.Forms.Control)(object)this.grdSummaryByJob).TabIndex = 1;
		this.grdSummaryByJob.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSummaryByJob).VisualStyle = (VisualStyle)4;
		this.label1.AutoSize = true;
		this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label1.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(4, 2);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(100, 13);
		this.label1.TabIndex = 3;
		this.label1.Text = "Summary by job";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(944, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 8;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdSummaryByJob);
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(342, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(642, 152);
		this.panel6.TabIndex = 155;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Margin = new System.Windows.Forms.Padding(4);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(642, 18);
		this.panel7.TabIndex = 6;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(984, 461);
		base.Controls.Add(this.panel1);
		this.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmPurchaseAndSaleDetailByJobLot";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmPurchaseAndSaleDetailByJobLot_FormClosing);
		base.Load += new System.EventHandler(frmPurchaseAndSaleDetailByJobLot_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmPurchaseAndSaleDetailByJobLot_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbjoblot).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSummaryByJob).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
