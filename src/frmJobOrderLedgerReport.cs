// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.BLL.Production;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.WinApp.Common;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Account_Reports
{

public class frmJobOrderLedgerReport : Form
{
	private WaitForm wf = new WaitForm();

	private DropDownBind DDL = new DropDownBind();

	private bool FilterRecordsByDocType;

	public static int GeneralLedgerId = 0;

	private int PointsRate = 2;

	private int PointsAmount = 2;

	private bool IsStockReservedPerParty;

	private bool AllowEditOnVoucherNoinReports;

	public static DateTime FromDate = DateTime.Now;

	public static DateTime ToDate = DateTime.Now;

	private DataTable dtDetail = new DataTable();

	private DataTable dtForDetailGrid = new DataTable();

	private DataTable dtDetailByDocTypes = new DataTable();

	private DataTable dtForDocumentTypeList = new DataTable();

	private bool FormInitializedForDetail;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	internal GridEX grd;

	private Button btnshow;

	internal UltraCombo cmbproject;

	internal UltraCombo CmbJobOrder;

	private Label label2;

	private Label label30;

	private Label label33;

	private Label label27;

	private Label label1;

	private Label label23;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton print105;

	internal UltraCombo comSupplier;

	private ToolStripButton rBForCheckAllPreviousRows;

	private Panel panel3;

	private Panel panel15;

	private GroupBox groupBox1;

	private Label label37;

	internal UltraCombo CmbDateType;

	private ToolStripButton btnShortcutKey1;

	public DateTimePicker txtToDate;

	public DateTimePicker txtFromDate;

	private Button BtnShowFilteredRecords;

	internal GridEX GrdDocType;

	internal UltraCombo CmbLanguage;

	private Label lblLanguage;

	private Panel panel33;

	private ToolStripButton btnSaveCheckedRows;

	private Label label3;

	internal UltraCombo CmbAccount;

	public frmJobOrderLedgerReport(UserAccount user)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void VoucherValidation_Load(object sender, EventArgs e)
	{
		try
		{
			FormInitializedForDetail = true;
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1350, 550);
			MultiLanguagesGetAll();
			List<ConfigrationsAllocation> configList = clsGlobalVariables.configrationsAllocation;
			ConfigrationsAllocation obj = configList.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "IsStockReservedPerParty");
			IsStockReservedPerParty = Conversion.ToBool((object)((obj != null) ? obj.ConfigKey : null));
			DataTable dtConfiq = CommonServices.GetMultipleConfigurationsByConfigDescriptions("Default NoofDecimal Points For Rate,Default NoofDecimal Points For Amount,DefaulLedgerDetail,DefaulLedgerSummaryOne,DefaulLedgerSummaryTwo,AllowEditOnVoucherNoinReports,FilterRecordsByDocumentTypeOnGeneralLedger");
			if (dtConfiq.Rows.Count > 0)
			{
				for (int i = 0; i < dtConfiq.Rows.Count; i++)
				{
					if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "Default NoofDecimal Points For Rate")
					{
						PointsRate = Conversion.ToInt(dtConfiq.Rows[i]["ConfigKey"]);
					}
					else if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "Default NoofDecimal Points For Amount")
					{
						PointsAmount = Conversion.ToInt(dtConfiq.Rows[i]["ConfigKey"]);
					}
					else if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "AllowEditOnVoucherNoinReports")
					{
						AllowEditOnVoucherNoinReports = Conversion.ToBool(dtConfiq.Rows[i]["ConfigKey"]);
					}
					else if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "FilterRecordsByDocumentTypeOnGeneralLedger")
					{
						FilterRecordsByDocType = Conversion.ToBool(dtConfiq.Rows[i]["ConfigKey"]);
					}
				}
			}
			if (FilterRecordsByDocType)
			{
				((Control)(object)GrdDocType).Visible = true;
				BtnShowFilteredRecords.Visible = true;
			}
			else
			{
				((Control)(object)GrdDocType).Visible = false;
				BtnShowFilteredRecords.Visible = false;
			}
			_ = clsGlobalVariables.ScreenViewReights;
			CompanyFill();
			WipAccountCombobind();
			jobOrderCombobind();
			DateTypeFill();
			ProjectFill();
			((UltraGridBase)CmbDateType).Rows[4].Activate();
			if (dtForDocumentTypeList.Columns.Count == 0)
			{
				dtForDocumentTypeList.Columns.Add("DocumentTypeId", typeof(int));
				dtForDocumentTypeList.Columns.Add("D.Type", typeof(string));
				dtForDocumentTypeList.Columns.Add("Balance", typeof(double));
			}
			if (GeneralLedgerId == 0)
			{
				txtFromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
			if (GeneralLedgerId > 0)
			{
				CmbJobOrder.Value = GeneralLedgerId;
				txtFromDate.Value = FromDate;
				txtToDate.Value = ToDate;
				btnshow_Click(null, null);
			}
			CmbAccount.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MultiLanguagesGetAll()
	{
		try
		{
			DataTable dt = CommonServices.MultiLanguagesGetAll();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbLanguage, "Id", "LanguageDescription", "Language Name", false);
				((UltraGridBase)CmbLanguage).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void jobOrderCombobind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		try
		{
			DataTable dtcombo = InvFoodProduction.GetDataForDropDownFromFoodProduction(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesIds = UserAccount.BranchesId.ToString(),
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Activity = "JobOrder"
			});
			if (dtcombo.Rows.Count > 0)
			{
				InfragisticsHelper.BindAndRetainSelection(CmbJobOrder, dtcombo, "Id", "ReferenceName", "Job Order", AllColumns: true, Conversion.ToInt(CmbJobOrder.Value), insertDefaultRow: false, null, null, ActivateRow: false);
				((UltraGridBase)CmbJobOrder).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			}
			else
			{
				((Control)(object)CmbJobOrder).Text = string.Empty;
				((UltraGridBase)CmbJobOrder).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WipAccountCombobind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		try
		{
			DataTable dtcombo = InvFoodProduction.GetDataForDropDownFromFoodProduction(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesIds = UserAccount.BranchesId.ToString(),
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Activity = "WipAccount"
			});
			if (dtcombo.Rows.Count > 0)
			{
				InfragisticsHelper.BindAndRetainSelection(CmbAccount, dtcombo, "Id", "ReferenceName", "Wip Account", AllColumns: true, Conversion.ToInt(CmbAccount.Value), insertDefaultRow: false, null, null, ActivateRow: false);
				((UltraGridBase)CmbAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			}
			else
			{
				((Control)(object)CmbAccount).Text = string.Empty;
				((UltraGridBase)CmbAccount).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CompanyFill()
	{
	}

	private void ProjectFill()
	{
		try
		{
			DataTable dt = CommonServices.ProjectServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbproject, "Id", "ProjectName", "Project Name", false);
			}
			((UltraGridBase)cmbproject).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DateTypeFill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			DDL.BindDDL(dt, CmbDateType, "Id", "Parameters", "Date Type", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDateType_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(CmbDateType.Value) == 1)
			{
				txtFromDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateType.Value) == 2)
			{
				txtFromDate.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(CmbDateType.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				txtFromDate.Value = first;
				txtToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateType.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				txtFromDate.Value = first2;
				txtToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateType.Value) == 5)
			{
				txtFromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
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
			grd.DataSource = null;
			((Control)(object)CmbJobOrder).Text = string.Empty;
			((Control)(object)CmbAccount).Text = string.Empty;
			CmbAccount.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GeneralLedger_FormClosing(object sender, FormClosingEventArgs e)
	{
		try
		{
			HistoryStack.Remove(this);
			GeneralLedgerId = 0;
			FromDate = DateTime.Now;
			ToDate = DateTime.Now;
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
			reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherValidation_KeyDown(object sender, KeyEventArgs e)
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
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				CmbDateType.Focus();
			}
			if ((e.Alt && e.KeyCode == Keys.D1) || (e.Alt && e.KeyCode == Keys.NumPad1))
			{
				print_Click_1(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys1();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
			}
			if (e.Control && e.Shift && e.KeyCode == Keys.S)
			{
				btnSaveCheckedRows_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.L)
			{
				lblLanguage.Visible = true;
				((Control)(object)CmbLanguage).Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void print_Click_1(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtDetail == null || dtDetail.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtDetail, "105_002-AcRptGeneralLedger.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton17_Click(object sender, EventArgs e)
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

	private void btnshow_Click(object sender, EventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			dtDetail.Rows.Clear();
			dtForDocumentTypeList.Rows.Clear();
			grd.ClearStructure();
			ReportsParameters obj = new ReportsParameters();
			if (Conversion.ToInt(CmbJobOrder.Value) == 0)
			{
				throw new Exception("Job Order Field is Required");
			}
			((Control)(object)grd).Name = "Detailgrd";
			print105.Visible = true;
			btnSaveCheckedRows.Visible = true;
			rBForCheckAllPreviousRows.Visible = true;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.AccountId = Conversion.ToInt(CmbAccount.Value);
			obj.JobOrderId = Conversion.ToInt(CmbJobOrder.Value);
			obj.FromDate = txtFromDate.Value;
			obj.ToDate = txtToDate.Value;
			obj.EntryUser = UserAccount.ID;
			obj.LanguageId = Conversion.ToInt(CmbLanguage.Value);
			dtDetail = VoucherReports.AccountsLedgerByJobOrder(obj);
			if (dtDetail.Rows.Count > 0 && dtDetail != null)
			{
				dtForDetailGrid.Clear();
				if (dtForDetailGrid.Columns.Count == 0)
				{
					dtForDetailGrid.Columns.Add("DetailId", typeof(int));
					dtForDetailGrid.Columns.Add("Checked", typeof(bool));
					dtForDetailGrid.Columns.Add("BookmarkRemarks", typeof(string));
					dtForDetailGrid.Columns.Add("VoucherDate", typeof(DateTime));
					dtForDetailGrid.Columns.Add("DocType");
					dtForDetailGrid.Columns.Add("VoucherNo");
					dtForDetailGrid.Columns.Add("AccountCode");
					dtForDetailGrid.Columns.Add("OffSetTitle");
					dtForDetailGrid.Columns.Add("Debit", typeof(double));
					dtForDetailGrid.Columns.Add("Credit", typeof(double));
					dtForDetailGrid.Columns.Add("Balance", typeof(double));
					dtForDetailGrid.Columns.Add("BalType");
					dtForDetailGrid.Columns.Add("Comments");
					dtForDetailGrid.Columns.Add("JobLot");
					dtForDetailGrid.Columns.Add("MannualNo");
					dtForDetailGrid.Columns.Add("RefNo");
					dtForDetailGrid.Columns.Add("QtyIn", typeof(double));
					dtForDetailGrid.Columns.Add("QtyOut", typeof(double));
					dtForDetailGrid.Columns.Add("ItemRate", typeof(double));
					dtForDetailGrid.Columns.Add("NoOfAttachments", typeof(double));
					dtForDetailGrid.Columns.Add("VoucherHeadId");
					dtForDetailGrid.Columns.Add("DocumentTypeId");
					dtForDetailGrid.Columns.Add("DocumentTypeSrNo");
					dtForDetailGrid.Columns.Add("BaseDocumentTypeId", typeof(int));
					dtForDetailGrid.Columns.Add("AccountId", typeof(int));
				}
				for (int i = 0; i < dtDetail.Rows.Count; i++)
				{
					string type = "Nill";
					if (Conversion.ToDouble(dtDetail.Rows[i]["Balance"]) > 0.0)
					{
						type = "Dr";
					}
					else if (Conversion.ToDouble(dtDetail.Rows[i]["Balance"]) < 0.0)
					{
						type = "Cr";
					}
					dtForDetailGrid.Rows.Add(dtDetail.Rows[i]["DetailId"], Conversion.ToBool(dtDetail.Rows[i]["BookmarkStatus"]), dtDetail.Rows[i]["BookmarkRemarks"], Conversion.ToDateTime(dtDetail.Rows[i]["VoucherDate"]).ToShortDateString(), dtDetail.Rows[i]["DocTypeCode"], dtDetail.Rows[i]["VoucherCode"], dtDetail.Rows[i]["AccountCode"], dtDetail.Rows[i]["OffsetAccountTitle"], dtDetail.Rows[i]["DebitAmount"], dtDetail.Rows[i]["CreditAmount"], dtDetail.Rows[i]["Balance"], type, dtDetail.Rows[i]["Comments"], dtDetail.Rows[i]["JobLot"], dtDetail.Rows[i]["MannualNo"], dtDetail.Rows[i]["RefInvoiceNo"], dtDetail.Rows[i]["QtyIn"], dtDetail.Rows[i]["QtyOut"], dtDetail.Rows[i]["ItemRate"], dtDetail.Rows[i]["NoOfAttachments"], dtDetail.Rows[i]["VoucherHeadId"], dtDetail.Rows[i]["DocumentTypeId"], dtDetail.Rows[i]["DocumentTypeSrNo"], dtDetail.Rows[i]["BaseDocumentTypeId"], dtDetail.Rows[i]["AccountId"]);
				}
				foreach (var item in (from row in dtDetail.AsEnumerable()
					group row by new
					{
						DocumentTypeId = Conversion.ToInt(row["DocumentTypeId"]),
						DocTypeCode = row["DocTypeCode"].ToString()
					} into g
					select new
					{
						DocumentTypeId = g.Key.DocumentTypeId,
						DocTypeCode = g.Key.DocTypeCode,
						Balance = g.Sum((DataRow row) => Conversion.ToDouble(row["DebitAmount"]) - Conversion.ToDouble(row["CreditAmount"]))
					}).ToList())
				{
					dtForDocumentTypeList.Rows.Add(item.DocumentTypeId, item.DocTypeCode, item.Balance);
				}
				grd.DataSource = dtForDetailGrid;
				grd.RetrieveStructure();
				GridSettings();
				GeneralLedgerId = 0;
				FillDocTypeList(dtForDocumentTypeList);
				return;
			}
			grd.ClearStructure();
			throw new Exception("Record Not Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettings()
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.AllowEdit = (InheritableBoolean)1;
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grd.RootTable.Columns["DetailId"].EditType = (EditType)0;
			grd.RootTable.Columns["VoucherDate"].EditType = (EditType)0;
			grd.RootTable.Columns["DocType"].EditType = (EditType)0;
			grd.RootTable.Columns["VoucherNo"].EditType = (EditType)0;
			grd.RootTable.Columns["AccountCode"].EditType = (EditType)0;
			grd.RootTable.Columns["OffSetTitle"].EditType = (EditType)0;
			grd.RootTable.Columns["Debit"].EditType = (EditType)0;
			grd.RootTable.Columns["Credit"].EditType = (EditType)0;
			grd.RootTable.Columns["Balance"].EditType = (EditType)0;
			grd.RootTable.Columns["BalType"].EditType = (EditType)0;
			grd.RootTable.Columns["Comments"].EditType = (EditType)0;
			grd.RootTable.Columns["MannualNo"].EditType = (EditType)0;
			grd.RootTable.Columns["RefNo"].EditType = (EditType)0;
			grd.RootTable.Columns["QtyIn"].EditType = (EditType)0;
			grd.RootTable.Columns["QtyOut"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemRate"].EditType = (EditType)0;
			grd.RootTable.Columns["NoOfAttachments"].EditType = (EditType)0;
			grd.RootTable.Columns["VoucherHeadId"].EditType = (EditType)0;
			grd.RootTable.Columns["DocumentTypeId"].EditType = (EditType)0;
			grd.RootTable.Columns["BaseDocumentTypeId"].EditType = (EditType)0;
			grd.RootTable.Columns["DocumentTypeSrNo"].EditType = (EditType)0;
			grd.RootTable.Columns["AccountId"].EditType = (EditType)0;
			grd.RootTable.Columns["VoucherDate"].FormatString = "dd-MMM-yyyy";
			grd.RootTable.Columns["Checked"].ColumnType = (ColumnType)3;
			grd.RootTable.Columns["VoucherDate"].FilterEditType = (FilterEditType)3;
			grd.RootTable.Columns["DocType"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["VoucherNo"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["AccountCode"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["OffSetTitle"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Debit"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Credit"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Balance"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["BalType"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Comments"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["MannualNo"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["RefNo"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["QtyIn"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["QtyOut"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["ItemRate"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["NoOfAttachments"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["DetailId"].Visible = false;
			grd.RootTable.Columns["VoucherHeadId"].Visible = false;
			grd.RootTable.Columns["DocumentTypeId"].Visible = false;
			grd.RootTable.Columns["BaseDocumentTypeId"].Visible = false;
			grd.RootTable.Columns["DocumentTypeSrNo"].Visible = false;
			grd.RootTable.Columns["RefNo"].Visible = false;
			grd.RootTable.Columns["AccountId"].Visible = false;
			grd.RootTable.Columns["Checked"].Width = 50;
			grd.RootTable.Columns["BookmarkRemarks"].Width = 100;
			grd.RootTable.Columns["VoucherDate"].Width = 90;
			grd.RootTable.Columns["DocType"].Width = 65;
			grd.RootTable.Columns["VoucherNo"].Width = 65;
			grd.RootTable.Columns["AccountCode"].Width = 85;
			grd.RootTable.Columns["OffSetTitle"].Width = 150;
			grd.RootTable.Columns["Debit"].Width = 120;
			grd.RootTable.Columns["Credit"].Width = 120;
			grd.RootTable.Columns["Balance"].Width = 130;
			grd.RootTable.Columns["BalType"].Width = 55;
			grd.RootTable.Columns["Comments"].Width = 250;
			grd.RootTable.Columns["JobLot"].Width = 110;
			grd.RootTable.Columns["MannualNo"].Width = 80;
			grd.RootTable.Columns["RefNo"].Width = 60;
			grd.RootTable.Columns["QtyIn"].Width = 65;
			grd.RootTable.Columns["QtyOut"].Width = 65;
			grd.RootTable.Columns["ItemRate"].Width = 70;
			grd.RootTable.Columns["NoOfAttachments"].Width = 100;
			grd.RootTable.Columns["DocumentTypeSrNo"].Width = 100;
			grd.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Debit"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["Debit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["Credit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Credit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Credit"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["Credit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["QtyIn"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["QtyIn"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["QtyIn"].FormatString = "#,##0.###";
			grd.RootTable.Columns["QtyIn"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["QtyOut"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["QtyOut"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["QtyOut"].FormatString = "#,##0.###";
			grd.RootTable.Columns["QtyOut"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["Balance"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Balance"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["VoucherNo"].ColumnType = (ColumnType)((UserAccount.RoleName == "Admin") ? 5 : (AllowEditOnVoucherNoinReports ? 5 : 0));
			grd.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["DocType"].ColumnType = (ColumnType)5;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void FillDocTypeList(DataTable dt)
	{
		try
		{
			if (FilterRecordsByDocType)
			{
				if (dt.Rows.Count > 0)
				{
					BtnShowFilteredRecords.Visible = true;
					BtnShowFilteredRecords.Enabled = true;
					((Control)(object)GrdDocType).Visible = true;
					GrdDocType.DataSource = dt;
					GrdDocType.RetrieveStructure();
					GrdDocType.ColumnAutoResize = true;
					GrdDocType.TotalRow = (InheritableBoolean)2;
					GrdDocType.RootTable.Columns["DocumentTypeId"].Visible = false;
					GrdDocType.RootTable.Columns["Balance"].TextAlignment = (TextAlignment)3;
					GrdDocType.RootTable.Columns["Balance"].FormatString = clsGlobalVariables.stringFormatboth;
					GrdDocType.RootTable.Columns.Add("Select");
					GrdDocType.RootTable.Columns["Select"].ActAsSelector = true;
					GrdDocType.RootTable.Columns["Select"].UseHeaderSelector = true;
					GrdDocType.RootTable.Columns["Select"].Position = 0;
					GrdDocType.RootTable.Columns["Select"].Width = 25;
					GrdDocType.FrozenColumns = 1;
					GrdDocType.RootTable.Columns["D.Type"].Width = 50;
					((JanusCollectionBase)GrdDocType.RootTable.SortKeys).Clear();
					GrdDocType.RootTable.SortKeys.Add(GrdDocType.RootTable.Columns["DocumentTypeId"], (SortOrder)1);
				}
				else
				{
					GrdDocType.ClearStructure();
					((Control)(object)GrdDocType).Visible = false;
					BtnShowFilteredRecords.Visible = false;
				}
			}
			else
			{
				((Control)(object)GrdDocType).Visible = false;
				BtnShowFilteredRecords.Visible = false;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitializedForDetail)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grd, dtForDetailGrid);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow currentRow = grd.CurrentRow;
			int BaseDocumentTypeId = 0;
			int Id = Conversion.ToInt(currentRow.Cells["VoucherHeadId"].Value);
			int AccountId = Conversion.ToInt(currentRow.Cells["AccountId"].Value);
			DateTime VoucherDate = Conversion.ToDateTime(currentRow.Cells["VoucherDate"].Value);
			int DocumentTypeId = Conversion.ToInt((object)currentRow.Cells["DocumentTypeId"].Value.ToString());
			BaseDocumentTypeId = Conversion.ToInt(currentRow.Cells["BaseDocumentTypeId"].Value);
			int DocumentTypeIdSrNo = int.Parse(currentRow.Cells["DocumentTypeSrNo"].Value.ToString());
			if (e.Column.Key == "VoucherNo" || e.Column.Key == "VoucherCode")
			{
				int JobOrderDocumentTypeId = 0;
				if (DocumentTypeId == 80 || DocumentTypeId == 110 || DocumentTypeId == 111 || DocumentTypeId == 112 || DocumentTypeId == 181)
				{
					JobOrderDocumentTypeId = InvFoodProduction.InvFoodProduction_GetJobOrderIdByProduction(UserAccount.OrganizationId, UserAccount.CompanyId, DocumentTypeId, DocumentTypeIdSrNo);
				}
				CommonServices.EditMethodFromLinked(DocumentTypeId, Id, DocumentTypeIdSrNo, BaseDocumentTypeId, JobOrderDocumentTypeId);
			}
			if (e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(DocumentTypeIdSrNo, DocumentTypeId);
			}
			if (e.Column.Key == "DocType" || e.Column.Key == "VoucherType")
			{
				CommonServices.GetSlipsByDocumentTypeId(DocumentTypeId, DocumentTypeIdSrNo, Id, AccountId, VoucherDate, VoucherDate);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rBForCheckAllPreviousRows_Click(object sender, EventArgs e)
	{
		try
		{
			CheckRowsWhichAreBookmard();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CheckRowsWhichAreBookmard()
	{
		try
		{
			if (dtDetail.Rows.Count <= 0)
			{
				return;
			}
			int LastIndex = GetIndexOfLastCheckedRow();
			if (LastIndex == -1)
			{
				MessageBox.Show("No Row Checked");
				return;
			}
			int counter = 0;
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (counter < LastIndex)
				{
					item.BeginEdit();
					if (Conversion.ToInt(item.Cells["DetailId"].Value) > 0)
					{
						item.Cells["Checked"].Value = true;
					}
					item.EndEdit();
					counter++;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private int GetIndexOfLastCheckedRow()
	{
		try
		{
			int LastIndex = -1;
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToBool(item.Cells["Checked"].Value))
				{
					LastIndex = item.RowIndex;
				}
			}
			return LastIndex;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnSaveCheckedRows_Click(object sender, EventArgs e)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Expected O, but got Unknown
		try
		{
			int CheckedRowCount = 0;
			GridEXRow[] rows = grd.GetRows();
			for (int i = 0; i < rows.Length; i++)
			{
				if (Conversion.ToBool(rows[i].Cells["Checked"].Value))
				{
					CheckedRowCount++;
				}
			}
			if (CheckedRowCount > 0)
			{
				if (MessageBox.Show("Are you sure to Save Checked Rows?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				ApprovalList obj = new ApprovalList();
				obj.AllApprovalLists = new List<ApprovalList>();
				rows = grd.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (Conversion.ToBool(item.Cells["Checked"].Value) && Conversion.ToInt(item.Cells["DetailId"].Value) > 0)
					{
						ApprovalList detail = new ApprovalList();
						detail.Id = Conversion.ToInt(item.Cells["DetailId"].Value);
						detail.ActionTypeId = Conversion.ToBool(item.Cells["Checked"].Value);
						detail.Comments = Conversion.ToString(item.Cells["BookmarkRemarks"].Value);
						obj.AllApprovalLists.Add(detail);
					}
					else if (!Conversion.ToBool(item.Cells["Checked"].Value) && Conversion.ToInt(item.Cells["DetailId"].Value) > 0)
					{
						ApprovalList detail2 = new ApprovalList();
						detail2.Id = Conversion.ToInt(item.Cells["DetailId"].Value);
						detail2.ActionTypeId = Conversion.ToBool(item.Cells["Checked"].Value);
						detail2.Comments = Conversion.ToString(item.Cells["BookmarkRemarks"].Value);
						obj.AllApprovalLists.Add(detail2);
					}
				}
				VoucherHead.InsertBookmarkStatusInBulk(obj);
				MessageBox.Show("Bookmark Status Saved Successfully");
				btnshow_Click(null, null);
				return;
			}
			throw new Exception("No Rows Checked");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grd_Click(object sender, EventArgs e)
	{
		try
		{
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnShortcutKey1_Click(object sender, EventArgs e)
	{
		try
		{
			MakeShortCutKeys1();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCutKey_Click(object sender, EventArgs e)
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

	private void MakeShortCutKeys1()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For Reset");
			dt.Rows.Add("alt+1", "For Print 1");
			dt.Rows.Add("Ctrl+S", "For Show Data");
			dt.Rows.Add("Ctrl+Shift+S", "For Save Bookmark");
			dt.Rows.Add("Ctrl+F5", "For Focus on Date Type");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grids");
			dt.Rows.Add("Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
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
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For Reset");
			dt.Rows.Add("alt+1", "For Print 1");
			dt.Rows.Add("alt+2", "For Print 2");
			dt.Rows.Add("alt+3", "For Print 3");
			dt.Rows.Add("Ctrl+S", "For Show Data");
			dt.Rows.Add("Ctrl+F5", "For Focus on To Date");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grids");
			dt.Rows.Add("Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Control && e.KeyCode == Keys.Space && grd.CurrentColumn != null)
			{
				int Id = Conversion.ToInt(item.Cells["VoucherHeadId"].Value);
				int DocumentTypeId = Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
				int DocumentTypeIdSrNo = Conversion.ToInt(item.Cells["DocumentTypeSrNo"].Value);
				if (grd.CurrentColumn.Key == "VoucherNo" || grd.CurrentColumn.Key == "VoucherNo")
				{
					CommonServices.EditMethodFromLinked(DocumentTypeId, Id, DocumentTypeIdSrNo);
				}
				else if (grd.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(DocumentTypeIdSrNo, DocumentTypeId);
				}
				else if (grd.CurrentColumn.Key == "DocType" || grd.CurrentColumn.Key == "DocType")
				{
					CommonServices.GetSlipsByDocumentTypeId(DocumentTypeId, DocumentTypeIdSrNo, Id);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShowFilteredRecords_Click(object sender, EventArgs e)
	{
		try
		{
			if (dtForDocumentTypeList.Rows.Count > 0)
			{
				dtDetailByDocTypes.Clear();
				string Ids = "";
				GridEXRow[] checkedRows = GrdDocType.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					Ids = Ids + "," + r.Cells["DocumentTypeId"].Value.ToString();
				}
				if (string.IsNullOrEmpty(Ids))
				{
					if (MessageBox.Show("No Document Type Selected. Are You Sure to Show?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
					{
						grd.ClearStructure();
					}
					return;
				}
				((Control)(object)grd).Name = "Detailgrd";
				print105.Visible = false;
				btnSaveCheckedRows.Visible = true;
				rBForCheckAllPreviousRows.Visible = true;
				List<int> idList = (from id in Ids.Split(',')
					select Conversion.ToInt((object)id)).ToList();
				dtDetailByDocTypes = (from row in dtDetail.AsEnumerable()
					where idList.Contains(row.Field<int>("DocumentTypeId"))
					select row).CopyToDataTable();
				if (dtDetailByDocTypes.Rows.Count <= 0)
				{
					return;
				}
				DataTable dt = new DataTable();
				dt.Columns.Add("DetailId", typeof(int));
				dt.Columns.Add("Checked", typeof(bool));
				dt.Columns.Add("BookmarkRemarks", typeof(string));
				dt.Columns.Add("VoucherDate", typeof(DateTime));
				dt.Columns.Add("DocType");
				dt.Columns.Add("VoucherNo");
				dt.Columns.Add("AccountCode");
				dt.Columns.Add("OffSetTitle");
				dt.Columns.Add("Debit", typeof(double));
				dt.Columns.Add("Credit", typeof(double));
				dt.Columns.Add("Balance", typeof(double));
				dt.Columns.Add("BalType");
				dt.Columns.Add("Comments");
				dt.Columns.Add("JobLot");
				dt.Columns.Add("MannualNo");
				dt.Columns.Add("RefNo");
				dt.Columns.Add("QtyIn", typeof(double));
				dt.Columns.Add("QtyOut", typeof(double));
				dt.Columns.Add("ItemRate", typeof(double));
				dt.Columns.Add("NoOfAttachments", typeof(double));
				dt.Columns.Add("VoucherHeadId");
				dt.Columns.Add("DocumentTypeId");
				dt.Columns.Add("BaseDocumentTypeId");
				dt.Columns.Add("DocumentTypeSrNo");
				dt.Columns.Add("AccountId");
				for (int i = 0; i < dtDetailByDocTypes.Rows.Count; i++)
				{
					string type = "Nill";
					if (Conversion.ToDouble(dtDetailByDocTypes.Rows[i]["Balance"]) > 0.0)
					{
						type = "Dr";
					}
					else if (Conversion.ToDouble(dtDetailByDocTypes.Rows[i]["Balance"]) < 0.0)
					{
						type = "Cr";
					}
					dt.Rows.Add(dtDetailByDocTypes.Rows[i]["DetailId"], Conversion.ToBool(dtDetailByDocTypes.Rows[i]["BookmarkStatus"]), dtDetailByDocTypes.Rows[i]["BookmarkRemarks"], Conversion.ToDateTime(dtDetailByDocTypes.Rows[i]["VoucherDate"]).ToShortDateString(), dtDetailByDocTypes.Rows[i]["DocTypeCode"], dtDetailByDocTypes.Rows[i]["VoucherCode"], dtDetailByDocTypes.Rows[i]["AccountCode"], dtDetailByDocTypes.Rows[i]["OffsetAccountTitle"], dtDetailByDocTypes.Rows[i]["DebitAmount"], dtDetailByDocTypes.Rows[i]["CreditAmount"], dtDetailByDocTypes.Rows[i]["Balance"], type, dtDetailByDocTypes.Rows[i]["Comments"], dtDetailByDocTypes.Rows[i]["JobLot"], dtDetailByDocTypes.Rows[i]["MannualNo"], dtDetailByDocTypes.Rows[i]["RefInvoiceNo"], dtDetailByDocTypes.Rows[i]["QtyIn"], dtDetailByDocTypes.Rows[i]["QtyOut"], dtDetailByDocTypes.Rows[i]["ItemRate"], dtDetailByDocTypes.Rows[i]["NoOfAttachments"], dtDetailByDocTypes.Rows[i]["VoucherHeadId"], dtDetailByDocTypes.Rows[i]["DocumentTypeId"], dtDetailByDocTypes.Rows[i]["BaseDocumentTypeId"], dtDetailByDocTypes.Rows[i]["DocumentTypeSrNo"], dtDetailByDocTypes.Rows[i]["AccountId"]);
				}
				grd.DataSource = dt;
				grd.RetrieveStructure();
				GridSettings();
			}
			else
			{
				grd.ClearStructure();
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
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected O, but got Unknown
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Expected O, but got Unknown
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Expected O, but got Unknown
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Expected O, but got Unknown
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Expected O, but got Unknown
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Expected O, but got Unknown
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Expected O, but got Unknown
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Expected O, but got Unknown
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Expected O, but got Unknown
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Expected O, but got Unknown
		//IL_0600: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdDocType_DesignTimeLayout = new GridEXLayout();
		Appearance appearance47 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Reports.frmJobOrderLedgerReport));
		Appearance appearance63 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.panel33 = new System.Windows.Forms.Panel();
		this.GrdDocType = new GridEX();
		this.BtnShowFilteredRecords = new System.Windows.Forms.Button();
		this.cmbproject = new UltraCombo();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.btnshow = new System.Windows.Forms.Button();
		this.label37 = new System.Windows.Forms.Label();
		this.CmbDateType = new UltraCombo();
		this.txtToDate = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.txtFromDate = new System.Windows.Forms.DateTimePicker();
		this.label33 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbAccount = new UltraCombo();
		this.CmbJobOrder = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.CmbLanguage = new UltraCombo();
		this.lblLanguage = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.print105 = new System.Windows.Forms.ToolStripButton();
		this.btnSaveCheckedRows = new System.Windows.Forms.ToolStripButton();
		this.rBForCheckAllPreviousRows = new System.Windows.Forms.ToolStripButton();
		this.btnShortcutKey1 = new System.Windows.Forms.ToolStripButton();
		this.comSupplier = new UltraCombo();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel15.SuspendLayout();
		this.panel33.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdDocType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbproject).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDateType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrder).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbLanguage).BeginInit();
		this.toolStrip3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.comSupplier).BeginInit();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel1.Controls.Add(this.panel15);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.toolStrip3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1009, 559);
		this.panel1.TabIndex = 0;
		this.grd.AllowEdit = (InheritableBoolean)2;
		this.grd.AlternatingColors = true;
		this.grd.AutomaticSort = false;
		this.grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 235);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1009, 324);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grd).Click += new System.EventHandler(grd_Click);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel15.Controls.Add(this.panel33);
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.cmbproject);
		this.panel15.Controls.Add(this.groupBox1);
		this.panel15.Controls.Add(this.label1);
		this.panel15.Controls.Add(this.label30);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 56);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1009, 179);
		this.panel15.TabIndex = 22;
		this.panel33.Controls.Add((System.Windows.Forms.Control)(object)this.GrdDocType);
		this.panel33.Controls.Add(this.BtnShowFilteredRecords);
		this.panel33.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel33.Location = new System.Drawing.Point(257, 0);
		this.panel33.Name = "panel33";
		this.panel33.Size = new System.Drawing.Size(254, 179);
		this.panel33.TabIndex = 118;
		this.GrdDocType.AllowEdit = (InheritableBoolean)2;
		this.GrdDocType.AlternatingColors = true;
		this.GrdDocType.AutomaticSort = false;
		GrdDocType_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdDocType.DesignTimeLayout = GrdDocType_DesignTimeLayout;
		this.GrdDocType.DynamicFiltering = true;
		this.GrdDocType.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdDocType).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdDocType.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdDocType).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GrdDocType).Name = "GrdDocType";
		((System.Windows.Forms.Control)(object)this.GrdDocType).Size = new System.Drawing.Size(200, 179);
		((System.Windows.Forms.Control)(object)this.GrdDocType).TabIndex = 0;
		this.GrdDocType.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdDocType).VisualStyle = (VisualStyle)4;
		this.BtnShowFilteredRecords.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.BtnShowFilteredRecords.FlatAppearance.BorderSize = 0;
		this.BtnShowFilteredRecords.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowFilteredRecords.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShowFilteredRecords.ForeColor = System.Drawing.Color.White;
		this.BtnShowFilteredRecords.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnShowFilteredRecords.Location = new System.Drawing.Point(200, 150);
		this.BtnShowFilteredRecords.Margin = new System.Windows.Forms.Padding(2);
		this.BtnShowFilteredRecords.Name = "BtnShowFilteredRecords";
		this.BtnShowFilteredRecords.Size = new System.Drawing.Size(54, 26);
		this.BtnShowFilteredRecords.TabIndex = 1;
		this.BtnShowFilteredRecords.Text = "Show";
		this.BtnShowFilteredRecords.UseVisualStyleBackColor = false;
		this.BtnShowFilteredRecords.Click += new System.EventHandler(BtnShowFilteredRecords_Click);
		this.cmbproject.AllowNull = (DefaultableBoolean)1;
		this.cmbproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance50;
		((SpecialBoxBase)((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance55).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance56).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance57).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbproject).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbproject).Location = new System.Drawing.Point(188, 237);
		((System.Windows.Forms.Control)(object)this.cmbproject).Name = "cmbproject";
		((System.Windows.Forms.Control)(object)this.cmbproject).Size = new System.Drawing.Size(137, 26);
		((System.Windows.Forms.Control)(object)this.cmbproject).TabIndex = 1;
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.label37);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDateType);
		this.groupBox1.Controls.Add(this.txtToDate);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.txtFromDate);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccount);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrder);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(257, 179);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters ";
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(194, 150);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(58, 26);
		this.btnshow.TabIndex = 5;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(8, 19);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(60, 15);
		this.label37.TabIndex = 144;
		this.label37.Text = "Date Type";
		this.CmbDateType.AllowNull = (DefaultableBoolean)1;
		this.CmbDateType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDateType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDateType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbDateType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDateType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDateType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDateType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance22;
		((SpecialBoxBase)((UltraGridBase)this.CmbDateType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDateType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbDateType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDateType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance27).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbDateType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDateType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDateType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDateType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDateType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDateType).Location = new System.Drawing.Point(83, 13);
		((System.Windows.Forms.Control)(object)this.CmbDateType).Name = "CmbDateType";
		((System.Windows.Forms.Control)(object)this.CmbDateType).Size = new System.Drawing.Size(167, 26);
		((System.Windows.Forms.Control)(object)this.CmbDateType).TabIndex = 0;
		this.CmbDateType.ValueChanged += new System.EventHandler(CmbDateType_ValueChanged);
		this.txtToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtToDate.Location = new System.Drawing.Point(83, 64);
		this.txtToDate.Name = "txtToDate";
		this.txtToDate.Size = new System.Drawing.Size(167, 23);
		this.txtToDate.TabIndex = 2;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(8, 44);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 103;
		this.label27.Text = "From Date";
		this.txtFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtFromDate.Location = new System.Drawing.Point(83, 40);
		this.txtFromDate.Name = "txtFromDate";
		this.txtFromDate.Size = new System.Drawing.Size(167, 23);
		this.txtFromDate.TabIndex = 1;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(8, 68);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(48, 15);
		this.label33.TabIndex = 102;
		this.label33.Text = "To Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(8, 97);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(52, 15);
		this.label3.TabIndex = 105;
		this.label3.Text = "Account";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(8, 126);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(59, 15);
		this.label2.TabIndex = 105;
		this.label2.Text = "Job Order";
		this.CmbAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance34;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance40).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccount).Location = new System.Drawing.Point(83, 91);
		((System.Windows.Forms.Control)(object)this.CmbAccount).Name = "CmbAccount";
		((System.Windows.Forms.Control)(object)this.CmbAccount).RightToLeft = System.Windows.Forms.RightToLeft.No;
		((System.Windows.Forms.Control)(object)this.CmbAccount).Size = new System.Drawing.Size(167, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccount).TabIndex = 3;
		this.CmbJobOrder.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrder.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrder.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrder.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrder).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance45;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrder).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance62).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance65).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrder.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).Location = new System.Drawing.Point(83, 120);
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).Name = "CmbJobOrder";
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).RightToLeft = System.Windows.Forms.RightToLeft.No;
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).Size = new System.Drawing.Size(167, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).TabIndex = 4;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(142, 243);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(44, 15);
		this.label1.TabIndex = 104;
		this.label1.Text = "Project";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(142, 216);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(44, 15);
		this.label30.TabIndex = 105;
		this.label30.Text = "Branch";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLanguage);
		this.panel3.Controls.Add(this.lblLanguage);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1009, 29);
		this.panel3.TabIndex = 21;
		this.CmbLanguage.AllowNull = (DefaultableBoolean)1;
		this.CmbLanguage.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLanguage.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLanguage.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Appearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance10;
		((SpecialBoxBase)((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance15).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLanguage.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Location = new System.Drawing.Point(831, 2);
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Name = "CmbLanguage";
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Size = new System.Drawing.Size(133, 26);
		((System.Windows.Forms.Control)(object)this.CmbLanguage).TabIndex = 133;
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Visible = false;
		this.lblLanguage.AutoSize = true;
		this.lblLanguage.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblLanguage.ForeColor = System.Drawing.Color.White;
		this.lblLanguage.Location = new System.Drawing.Point(762, 7);
		this.lblLanguage.Name = "lblLanguage";
		this.lblLanguage.Size = new System.Drawing.Size(68, 17);
		this.lblLanguage.TabIndex = 134;
		this.lblLanguage.Text = "Language";
		this.lblLanguage.Visible = false;
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.Teal;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(6, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(202, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Job Order (WIP) Ledger";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(967, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnNew, this.print105, this.btnSaveCheckedRows, this.rBForCheckAllPreviousRows, this.btnShortcutKey1 });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
		this.toolStrip3.Size = new System.Drawing.Size(1009, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = (System.Drawing.Image)resources.GetObject("btnNew.Image");
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(68, 24);
		this.btnNew.Text = "&Reset";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.print105.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.print105.Image = (System.Drawing.Image)resources.GetObject("print105.Image");
		this.print105.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print105.Name = "print105";
		this.print105.Size = new System.Drawing.Size(124, 24);
		this.print105.Text = "&Print-105_002";
		this.print105.Click += new System.EventHandler(print_Click_1);
		this.btnSaveCheckedRows.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaveCheckedRows.Image = (System.Drawing.Image)resources.GetObject("btnSaveCheckedRows.Image");
		this.btnSaveCheckedRows.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveCheckedRows.Name = "btnSaveCheckedRows";
		this.btnSaveCheckedRows.Size = new System.Drawing.Size(154, 24);
		this.btnSaveCheckedRows.Text = "SaveCheckedRows";
		this.btnSaveCheckedRows.Visible = false;
		this.btnSaveCheckedRows.Click += new System.EventHandler(btnSaveCheckedRows_Click);
		this.rBForCheckAllPreviousRows.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.rBForCheckAllPreviousRows.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.rBForCheckAllPreviousRows.Image = (System.Drawing.Image)resources.GetObject("rBForCheckAllPreviousRows.Image");
		this.rBForCheckAllPreviousRows.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.rBForCheckAllPreviousRows.Name = "rBForCheckAllPreviousRows";
		this.rBForCheckAllPreviousRows.Size = new System.Drawing.Size(356, 24);
		this.rBForCheckAllPreviousRows.Text = "Bookmark All Previous Rows After Last Checked Row";
		this.rBForCheckAllPreviousRows.Visible = false;
		this.rBForCheckAllPreviousRows.Click += new System.EventHandler(rBForCheckAllPreviousRows_Click);
		this.btnShortcutKey1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShortcutKey1.Image = (System.Drawing.Image)resources.GetObject("btnShortcutKey1.Image");
		this.btnShortcutKey1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutKey1.Name = "btnShortcutKey1";
		this.btnShortcutKey1.Size = new System.Drawing.Size(106, 24);
		this.btnShortcutKey1.Text = "ShortCut Keys";
		this.btnShortcutKey1.Click += new System.EventHandler(btnShortcutKey1_Click);
		this.comSupplier.AllowNull = (DefaultableBoolean)1;
		this.comSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.comSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.comSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance5;
		((SpecialBoxBase)((UltraGridBase)this.comSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.comSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance68).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).TextHAlignAsString = "Left";
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.comSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comSupplier).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.comSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comSupplier).Location = new System.Drawing.Point(318, 30);
		((System.Windows.Forms.Control)(object)this.comSupplier).Name = "comSupplier";
		((System.Windows.Forms.Control)(object)this.comSupplier).Size = new System.Drawing.Size(359, 27);
		((System.Windows.Forms.Control)(object)this.comSupplier).TabIndex = 2;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1009, 559);
		base.Controls.Add(this.panel1);
		this.DoubleBuffered = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "frmJobOrderLedgerReport";
		this.Text = "Ledgers";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(GeneralLedger_FormClosing);
		base.Load += new System.EventHandler(VoucherValidation_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(VoucherValidation_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel33.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdDocType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbproject).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDateType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrder).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbLanguage).EndInit();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.comSupplier).EndInit();
		base.ResumeLayout(false);
	}
}

}
