using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Export;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.ExportReports;

public class frmExportContractSchedulePeriodicB : Form
{
	private UserAccount UserAccount = new UserAccount();

	private bool FormInitialized;

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtLoadingDateItemFromDb = new DataTable();

	private DataTable dtLoadingDateItemCustomerFromDb = new DataTable();

	private DataTable dtItemLoadingDate = new DataTable();

	private DataTable dtLoadingDateItem = new DataTable();

	private DataTable dtLoadingDateItemCustomer = new DataTable();

	private DataTable dtItemLoadingDateCustomer = new DataTable();

	private DateTime LastScheduleLoadingDate = DateTime.Now;

	private IContainer components;

	private Panel PanelDetail;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton btnPrint02;

	private Panel panel1;

	internal GridEX grdLoadingDateItem;

	private Label label11;

	private ToolStripButton btnShortCutKeys;

	private Panel panel9;

	internal GridEX grdLoadingDateItemCustomer;

	private Panel panel10;

	private Label label7;

	private Label label2;

	private DateTimePicker txtToDate;

	private Button BtnShowbyDates;

	private DateTimePicker txtDateFrom;

	private Label label12;

	private Label label1;

	private TextBox tztAddDays;

	private Panel panel3;

	private GroupBox groupBox1;

	private TextBox txtprcntfclton;

	private Label label3;

	private Panel panel12;

	private Label label8;

	internal GridEX grdDueWise;

	private Label label9;

	private Label label4;

	private TextBox txtIntervalDays;

	private Label label10;

	private ToolStripButton btnPrint03;

	private ToolStripSeparator toolStripSeparator1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private CheckBox chkNotReferredStatus;

	public frmExportContractSchedulePeriodicB(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmEvaulationDetailSalesReports_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1240, 500);
			FormInitialized = true;
			dtItemLoadingDate.Columns.Add("GroupTitle", typeof(string));
			dtItemLoadingDate.Columns.Add("ItemId", typeof(int));
			dtItemLoadingDate.Columns.Add("ItemName", typeof(string));
			dtItemLoadingDate.Columns.Add("LoadingDate", typeof(DateTime));
			dtItemLoadingDate.Columns.Add("Fcl", typeof(decimal));
			dtItemLoadingDate.Columns.Add("MTons", typeof(decimal));
			dtLoadingDateItem.Columns.Add("GroupTitle", typeof(string));
			dtLoadingDateItem.Columns.Add("ItemId", typeof(int));
			dtLoadingDateItem.Columns.Add("LoadingDate", typeof(DateTime));
			dtLoadingDateItem.Columns.Add("ItemName", typeof(string));
			dtLoadingDateItem.Columns.Add("Fcl", typeof(decimal));
			dtLoadingDateItem.Columns.Add("MTons", typeof(decimal));
			dtLoadingDateItemCustomer.Columns.Add("GroupTitle", typeof(string));
			dtLoadingDateItemCustomer.Columns.Add("ScheduleId", typeof(int));
			dtLoadingDateItemCustomer.Columns.Add("ExImLcOrderId", typeof(int));
			dtLoadingDateItemCustomer.Columns.Add("LoadingDate", typeof(DateTime));
			dtLoadingDateItemCustomer.Columns.Add("ScheduleCode", typeof(string));
			dtLoadingDateItemCustomer.Columns.Add("SupplierCustomerId", typeof(int));
			dtLoadingDateItemCustomer.Columns.Add("Customer", typeof(string));
			dtLoadingDateItemCustomer.Columns.Add("CustomerContractNo", typeof(string));
			dtLoadingDateItemCustomer.Columns.Add("ItemId", typeof(int));
			dtLoadingDateItemCustomer.Columns.Add("ItemName", typeof(string));
			dtLoadingDateItemCustomer.Columns.Add("PackSize", typeof(string));
			dtLoadingDateItemCustomer.Columns.Add("NoOfBags", typeof(decimal));
			dtLoadingDateItemCustomer.Columns.Add("MTons", typeof(decimal));
			dtLoadingDateItemCustomer.Columns.Add("Fcl", typeof(decimal));
			dtLoadingDateItemCustomer.Columns.Add("ReferredStatus", typeof(string));
			dtLoadingDateItemCustomer.Columns.Add("DestinationPort", typeof(string));
			dtLoadingDateItemCustomer.Columns.Add("NoOfAttachments", typeof(int));
			dtLoadingDateItemCustomer.Columns.Add("GroupCaption", typeof(string));
			dtItemLoadingDateCustomer.Columns.Add("GroupTitle", typeof(string));
			dtItemLoadingDateCustomer.Columns.Add("ItemId", typeof(int));
			dtItemLoadingDateCustomer.Columns.Add("LoadingDate", typeof(DateTime));
			dtItemLoadingDateCustomer.Columns.Add("ItemName", typeof(string));
			dtItemLoadingDateCustomer.Columns.Add("SupplierCustomerId", typeof(int));
			dtItemLoadingDateCustomer.Columns.Add("Customer", typeof(string));
			dtItemLoadingDateCustomer.Columns.Add("Fcl", typeof(decimal));
			dtItemLoadingDateCustomer.Columns.Add("MTons", typeof(decimal));
			txtDateFrom.Value = DateTime.Now;
			LastScheduleLoadingDate = ExImLcOrderShipmentSchedule.GetLatestAttentiveLoadDate(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (LastScheduleLoadingDate > DateTime.Now)
			{
				tztAddDays.Text = (LastScheduleLoadingDate - DateTime.Now).Days.ToString();
			}
			else
			{
				txtDateFrom.Value = LastScheduleLoadingDate;
			}
			txtToDate.Value = LastScheduleLoadingDate;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			ShowDataByDates();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ShowDataByDates(int SortNo = 0)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		try
		{
			if (string.IsNullOrEmpty(txtprcntfclton.Text))
			{
				txtprcntfclton.Text = "24";
			}
			int prcntoffclTon = Conversion.ToInt((object)txtprcntfclton.Text);
			if (prcntoffclTon < 18)
			{
				MessageBox.Show("Minimum Value Of MTon is 18 .", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (prcntoffclTon > 30)
			{
				MessageBox.Show("Maximum Value Of MTon is 30.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			if (txtDateFrom.Checked)
			{
				obj.FromDate = Conversion.ToDateTime((object)txtDateFrom.Value);
			}
			if (txtToDate.Checked)
			{
				obj.ToDate = Conversion.ToDateTime((object)txtToDate.Value);
			}
			obj.NetWeight = prcntoffclTon;
			obj.SortNo = SortNo;
			obj.IntervalDays = Conversion.ToInt((object)txtIntervalDays.Text);
			obj.StatusId = (chkNotReferredStatus.Checked ? 1 : 0);
			DataSet DtSet = ExImLcOrderShipmentSchedule.ExportContractSchedulePeriodicB(obj);
			FillAllGrids(DtSet, SortNo);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void FillAllGrids(DataSet DtSet, int SortNo = 0)
	{
		//IL_05d8: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			dtLoadingDateItemFromDb.Clear();
			dtLoadingDateItemCustomerFromDb.Clear();
			if (DtSet == null)
			{
				grdLoadingDateItem.DataSource = null;
				grdLoadingDateItemCustomer.DataSource = null;
				grdDueWise.DataSource = null;
			}
			if (DtSet.Tables[1].Rows.Count > 0)
			{
				dtLoadingDateItemFromDb = DtSet.Tables[1];
				DatatableHelper.ReplaceCompLogoColumn(dtLoadingDateItemFromDb, (DtSet.Tables[0].Rows.Count > 0) ? DtSet.Tables[0] : null);
				dtLoadingDateItem.Rows.Clear();
				foreach (DataRow dr2 in dtLoadingDateItemFromDb.Rows)
				{
					dtLoadingDateItem.Rows.Add(dr2["GroupTitle"], dr2["ItemId"], dr2["LoadingDate"], dr2["ItemName"], dr2["Flc"], dr2["MTons"]);
				}
				if (dtLoadingDateItem.Rows.Count > 0)
				{
					grdLoadingDateItem.DataSource = dtLoadingDateItem;
					grdLoadingDateItem.RetrieveStructure();
					grdLoadingDateItem.RootTable.Groups.Add("LoadingDate");
					GrdByLoadingDateItemSetting();
				}
				else
				{
					grdLoadingDateItem.DataSource = null;
				}
			}
			else
			{
				grdLoadingDateItem.DataSource = null;
			}
			if (DtSet.Tables[2].Rows.Count > 0)
			{
				dtLoadingDateItemCustomerFromDb = DtSet.Tables[2];
				DatatableHelper.ReplaceCompLogoColumn(dtLoadingDateItemCustomerFromDb, (DtSet.Tables[0].Rows.Count > 0) ? DtSet.Tables[0] : null);
				dtLoadingDateItemCustomer.Rows.Clear();
				foreach (DataRow dr in dtLoadingDateItemCustomerFromDb.Rows)
				{
					dtLoadingDateItemCustomer.Rows.Add(dr["GroupTitle"], dr["Id"], dr["ExImLcOrderId"], dr["LoadingDate"], dr["ScheduleCode"], dr["SupplierCustomerId"], dr["Customer"], dr["CustomerContractNo"], dr["ItemId"], dr["ItemName"], dr["Packsize"], dr["NoOfBags"], dr["MTons"], dr["Flc"], dr["ReferredStatus"], dr["DestinationPort"], dr["NoOfAttachments"], Conversion.ToDateTime(dr["LoadingDate"]).ToString("dd-MMM-yyyy") + " " + dr["DestinationPort"]?.ToString() + " " + dr["ScheduleCode"]);
				}
				if (dtLoadingDateItemCustomer.Rows.Count > 0)
				{
					grdLoadingDateItemCustomer.DataSource = dtLoadingDateItemCustomer;
					grdLoadingDateItemCustomer.RetrieveStructure();
					grdLoadingDateItemCustomer.RootTable.Groups.Add("GroupCaption");
					GrdByLoadingDateItemCustomerSetting();
				}
				else
				{
					grdLoadingDateItemCustomer.DataSource = null;
				}
			}
			else
			{
				grdLoadingDateItemCustomer.DataSource = null;
			}
			if (SortNo != 0)
			{
				return;
			}
			DataTable dtPeriodic = DtSet.Tables[3];
			if (dtPeriodic.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("SortNo", typeof(int));
				dt.Columns.Add("PeriodDescription");
				dt.Columns.Add("FCL", typeof(double));
				dt.Columns.Add("M.Tons", typeof(double));
				for (int i = 0; i < dtPeriodic.Rows.Count; i++)
				{
					dt.Rows.Add(dtPeriodic.Rows[i]["SortNo"], dtPeriodic.Rows[i]["Description"], dtPeriodic.Rows[i]["FCL"], dtPeriodic.Rows[i]["MTons"]);
				}
				grdDueWise.DataSource = dt;
				grdDueWise.RetrieveStructure();
				grdDueWise.RootTable.HeaderLines = 2;
				foreach (GridEXColumn item in (JanusCollectionBase)grdDueWise.RootTable.Columns)
				{
					item.HeaderAlignment = (TextAlignment)2;
				}
				grdDueWise.RootTable.Columns["SortNo"].Visible = false;
				grdDueWise.RootTable.Columns["PeriodDescription"].Width = 325;
				grdDueWise.RootTable.Columns["FCL"].Width = Constants.ExportConstants.FclConstant + 30;
				grdDueWise.RootTable.Columns["M.Tons"].Width = Constants.ExportConstants.QtyConstant + 10;
				grdDueWise.RootTable.Columns["FCL"].AggregateFunction = (AggregateFunction)2;
				grdDueWise.RootTable.Columns["FCL"].TextAlignment = (TextAlignment)3;
				grdDueWise.RootTable.Columns["FCL"].FormatString = "#,##0.00";
				grdDueWise.RootTable.Columns["FCL"].TotalFormatString = "#,##0.00";
				grdDueWise.RootTable.Columns["M.Tons"].AggregateFunction = (AggregateFunction)2;
				grdDueWise.RootTable.Columns["M.Tons"].TextAlignment = (TextAlignment)3;
				grdDueWise.RootTable.Columns["M.Tons"].FormatString = "#,##0.00";
				grdDueWise.RootTable.Columns["M.Tons"].TotalFormatString = "#,##0.00";
				grdDueWise.TotalRow = (InheritableBoolean)1;
			}
			else
			{
				grdDueWise.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdByLoadingDateItemSetting()
	{
		try
		{
			grdLoadingDateItem.RootTable.Columns["Fcl"].AggregateFunction = (AggregateFunction)2;
			grdLoadingDateItem.RootTable.Columns["Fcl"].TextAlignment = (TextAlignment)3;
			grdLoadingDateItem.RootTable.Columns["Fcl"].FormatString = "#,##0.##";
			grdLoadingDateItem.RootTable.Columns["Fcl"].TotalFormatString = "#,##0.##";
			grdLoadingDateItem.RootTable.Columns["MTons"].AggregateFunction = (AggregateFunction)2;
			grdLoadingDateItem.RootTable.Columns["MTons"].TextAlignment = (TextAlignment)3;
			grdLoadingDateItem.RootTable.Columns["MTons"].FormatString = "#,##0.###";
			grdLoadingDateItem.RootTable.Columns["MTons"].TotalFormatString = "#,##0.###";
			grdLoadingDateItem.RootTable.Columns["GroupTitle"].Visible = false;
			grdLoadingDateItem.RootTable.Columns["ItemId"].Visible = false;
			grdLoadingDateItem.RootTable.Columns["LoadingDate"].Visible = false;
			grdLoadingDateItem.RootTable.Columns["LoadingDate"].Width = 80;
			grdLoadingDateItem.RootTable.Columns["ItemName"].Width = 380;
			grdLoadingDateItem.RootTable.Columns["Fcl"].Width = 70;
			grdLoadingDateItem.RootTable.Columns["MTons"].Width = 80;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdByLoadingDateItemCustomerSetting()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdLoadingDateItemCustomer.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdLoadingDateItemCustomer.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdLoadingDateItemCustomer.RootTable.Columns["Fcl"].AggregateFunction = (AggregateFunction)2;
			grdLoadingDateItemCustomer.RootTable.Columns["Fcl"].TextAlignment = (TextAlignment)3;
			grdLoadingDateItemCustomer.RootTable.Columns["Fcl"].FormatString = "#,##0.##";
			grdLoadingDateItemCustomer.RootTable.Columns["Fcl"].TotalFormatString = "#,##0.##";
			grdLoadingDateItemCustomer.RootTable.Columns["MTons"].AggregateFunction = (AggregateFunction)2;
			grdLoadingDateItemCustomer.RootTable.Columns["MTons"].TextAlignment = (TextAlignment)3;
			grdLoadingDateItemCustomer.RootTable.Columns["MTons"].FormatString = "#,##0.###";
			grdLoadingDateItemCustomer.RootTable.Columns["MTons"].TotalFormatString = "#,##0.###";
			grdLoadingDateItemCustomer.RootTable.Columns["NoOfBags"].AggregateFunction = (AggregateFunction)2;
			grdLoadingDateItemCustomer.RootTable.Columns["NoOfBags"].TextAlignment = (TextAlignment)3;
			grdLoadingDateItemCustomer.RootTable.Columns["NoOfBags"].FormatString = "#,##0.###";
			grdLoadingDateItemCustomer.RootTable.Columns["NoOfBags"].TotalFormatString = "#,##0.###";
			grdLoadingDateItemCustomer.RootTable.Columns["GroupTitle"].Visible = false;
			grdLoadingDateItemCustomer.RootTable.Columns["ScheduleId"].Visible = false;
			grdLoadingDateItemCustomer.RootTable.Columns["ExImLcOrderId"].Visible = false;
			grdLoadingDateItemCustomer.RootTable.Columns["ItemId"].Visible = false;
			grdLoadingDateItemCustomer.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdLoadingDateItemCustomer.RootTable.Columns["LoadingDate"].Visible = false;
			grdLoadingDateItemCustomer.RootTable.Columns["GroupCaption"].Visible = false;
			grdLoadingDateItemCustomer.RootTable.Columns["LoadingDate"].Width = 80;
			grdLoadingDateItemCustomer.RootTable.Columns["ItemName"].Width = 275;
			grdLoadingDateItemCustomer.RootTable.Columns["ScheduleCode"].Width = 108;
			grdLoadingDateItemCustomer.RootTable.Columns["Customer"].Width = 178;
			grdLoadingDateItemCustomer.RootTable.Columns["Fcl"].Width = 70;
			grdLoadingDateItemCustomer.RootTable.Columns["MTons"].Width = 80;
			grdLoadingDateItemCustomer.RootTable.Columns["NoOfBags"].Width = 80;
			grdLoadingDateItemCustomer.RootTable.Columns["PackSize"].Width = 100;
			grdLoadingDateItemCustomer.RootTable.Columns["CustomerContractNo"].Width = 250;
			grdLoadingDateItemCustomer.RootTable.Columns["ReferredStatus"].Width = 100;
			grdLoadingDateItemCustomer.RootTable.Columns["DestinationPort"].Width = 120;
			grdLoadingDateItemCustomer.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			GridEX_Helper.AddConditionalFormatting(grdLoadingDateItemCustomer, "ReferredStatus", new List<(ConditionOperator, object, Color, bool)>
			{
				((ConditionOperator)0, "Referred", Color.DarkGreen, true),
				((ConditionOperator)1, "Referred", Color.Red, true)
			});
			grdLoadingDateItemCustomer.RootTable.Columns["ScheduleCode"].ColumnType = (ColumnType)5;
			grdLoadingDateItemCustomer.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdLoadingDateItemCustomer_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdLoadingDateItemCustomer.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (e.Column.Key == "ScheduleCode")
			{
				int ScheduleId = Conversion.ToInt(r.Cells["ExImLcOrderId"].Value);
				if (CommonServices.SetRightsValueInRightsObject("FrmExportSalesContractSchedule").DoHaveViewRight)
				{
					frmSaleContractSchedule frmSaleContractSchedule = new frmSaleContractSchedule(UserAccount);
					frmSaleContractSchedule.Show();
					frmSaleContractSchedule.ReadById(ScheduleId);
				}
				else
				{
					MessageBox.Show("You Don't Have rights to View This Form..");
				}
			}
			else if (e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["ScheduleId"].Value), 244);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDueWise_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDueWise.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int SortNo = Conversion.ToInt(r.Cells["SortNo"].Value);
				ShowDataByDates(SortNo);
				r.CheckState = (RowCheckState)0;
			}
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
			if (LastScheduleLoadingDate > DateTime.Now)
			{
				tztAddDays.Text = (LastScheduleLoadingDate - DateTime.Now).Days.ToString();
			}
			else
			{
				txtDateFrom.Value = LastScheduleLoadingDate;
			}
			grdLoadingDateItem.DataSource = null;
			grdLoadingDateItemCustomer.DataSource = null;
			grdDueWise.DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tztAddDays_KeyPress(object sender, KeyPressEventArgs e)
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

	private void tztAddDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DateTime ToDate = Conversion.ToDateTime((object)txtDateFrom.Value).AddDays(Conversion.ToInt((object)tztAddDays.Text));
			txtToDate.Value = ToDate;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCutKeys_Click(object sender, EventArgs e)
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

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Focus On From Date combo in First Filter Box");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F5", "For Focus on Combo From Date");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On From Date combo in First Filter Box");
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

	private void frmExportContractSchedulePeriodic_KeyDown(object sender, KeyEventArgs e)
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

	private void btnPrint02_Click(object sender, EventArgs e)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dataTable = dtLoadingDateItemFromDb;
			if (dataTable != null && dataTable.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtLoadingDateItemFromDb, "551_02_ExportContractScheduleLoadingDateItemWise_Slip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint03_Click(object sender, EventArgs e)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dataTable = dtLoadingDateItemCustomerFromDb;
			if (dataTable != null && dataTable.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtLoadingDateItemCustomerFromDb, "551_03_ExportContractScheduleLoadingDateCustomerItemWise_Slip.rpt");
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdLoadingDateItemCustomer, dtLoadingDateItemCustomer);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdLoadingDateItem, dtLoadingDateItem);
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
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0bab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb5: Expected O, but got Unknown
		GridEXLayout grdLoadingDateItem_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdLoadingDateItemCustomer_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDueWise_DesignTimeLayout = new GridEXLayout();
		this.PanelDetail = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnPrint03 = new System.Windows.Forms.ToolStripButton();
		this.btnPrint02 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.grdLoadingDateItem = new GridEX();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grdLoadingDateItemCustomer = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label7 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel12 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label8 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtToDate = new System.Windows.Forms.DateTimePicker();
		this.BtnShowbyDates = new System.Windows.Forms.Button();
		this.txtDateFrom = new System.Windows.Forms.DateTimePicker();
		this.label12 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.tztAddDays = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdDueWise = new GridEX();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtIntervalDays = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.txtprcntfclton = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.chkNotReferredStatus = new System.Windows.Forms.CheckBox();
		this.PanelDetail.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdLoadingDateItem).BeginInit();
		this.panel1.SuspendLayout();
		this.panel9.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdLoadingDateItemCustomer).BeginInit();
		this.panel10.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel12.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDueWise).BeginInit();
		this.groupBox1.SuspendLayout();
		base.SuspendLayout();
		this.PanelDetail.BackColor = System.Drawing.Color.Teal;
		this.PanelDetail.Controls.Add(this.label9);
		this.PanelDetail.Controls.Add(this.label4);
		this.PanelDetail.Controls.Add(this.label11);
		this.PanelDetail.Controls.Add(this.toolStrip3);
		this.PanelDetail.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanelDetail.Location = new System.Drawing.Point(0, 0);
		this.PanelDetail.Name = "PanelDetail";
		this.PanelDetail.Size = new System.Drawing.Size(1436, 58);
		this.PanelDetail.TabIndex = 0;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(373, 42);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(311, 15);
		this.label9.TabIndex = 174;
		this.label9.Text = "Proactive Planning To Avoid Shipment Delays && Penalties";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(439, 27);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(192, 15);
		this.label4.TabIndex = 173;
		this.label4.Text = "Shipment Schedule Aging Report";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(3, 32);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(300, 19);
		this.label11.TabIndex = 0;
		this.label11.Text = "Export Contract Scheduling Reports";
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnNew, this.btnPrint03, this.btnPrint02, this.toolStripSeparator1, this.btnShortCutKeys });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1436, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnPrint03.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint03.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint03.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint03.Name = "btnPrint03";
		this.btnPrint03.Size = new System.Drawing.Size(355, 24);
		this.btnPrint03.Text = "&551_03-Loading_Date_Item_And_Customer_Wise";
		this.btnPrint03.Click += new System.EventHandler(btnPrint03_Click);
		this.btnPrint02.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint02.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint02.Name = "btnPrint02";
		this.btnPrint02.Size = new System.Drawing.Size(285, 24);
		this.btnPrint02.Text = "&551_02-Loading_Date_And_Item_Wise";
		this.btnPrint02.Click += new System.EventHandler(btnPrint02_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(121, 24);
		this.btnShortCutKeys.Text = "ShortCutKeys";
		this.btnShortCutKeys.ToolTipText = "ShortCutKeys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		this.grdLoadingDateItem.AllowEdit = (InheritableBoolean)2;
		grdLoadingDateItem_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdLoadingDateItem.DesignTimeLayout = grdLoadingDateItem_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdLoadingDateItem).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdLoadingDateItem.DynamicFiltering = true;
		this.grdLoadingDateItem.FilterMode = (FilterMode)2;
		this.grdLoadingDateItem.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdLoadingDateItem).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdLoadingDateItem.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdLoadingDateItem).Location = new System.Drawing.Point(3, 30);
		((System.Windows.Forms.Control)(object)this.grdLoadingDateItem).Name = "grdLoadingDateItem";
		((ScrollableBase)this.grdLoadingDateItem).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdLoadingDateItem).Size = new System.Drawing.Size(1422, 426);
		((System.Windows.Forms.Control)(object)this.grdLoadingDateItem).TabIndex = 0;
		this.grdLoadingDateItem.TotalRow = (InheritableBoolean)1;
		this.grdLoadingDateItem.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdLoadingDateItem).VisualStyle = (VisualStyle)4;
		this.panel1.Controls.Add(this.panel9);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 221);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1436, 485);
		this.panel1.TabIndex = 2;
		this.panel9.Controls.Add(this.tabControl1);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1436, 485);
		this.panel9.TabIndex = 173;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1436, 485);
		this.tabControl1.TabIndex = 0;
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdLoadingDateItemCustomer);
		this.tabPage1.Controls.Add(this.panel10);
		this.tabPage1.Location = new System.Drawing.Point(4, 22);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1428, 459);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Schedule Detail";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.grdLoadingDateItemCustomer.AllowEdit = (InheritableBoolean)2;
		grdLoadingDateItemCustomer_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdLoadingDateItemCustomer.DesignTimeLayout = grdLoadingDateItemCustomer_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdLoadingDateItemCustomer).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdLoadingDateItemCustomer.DynamicFiltering = true;
		this.grdLoadingDateItemCustomer.FilterMode = (FilterMode)2;
		this.grdLoadingDateItemCustomer.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdLoadingDateItemCustomer).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdLoadingDateItemCustomer.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdLoadingDateItemCustomer).Location = new System.Drawing.Point(3, 30);
		((System.Windows.Forms.Control)(object)this.grdLoadingDateItemCustomer).Name = "grdLoadingDateItemCustomer";
		((ScrollableBase)this.grdLoadingDateItemCustomer).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdLoadingDateItemCustomer).Size = new System.Drawing.Size(1422, 426);
		((System.Windows.Forms.Control)(object)this.grdLoadingDateItemCustomer).TabIndex = 0;
		this.grdLoadingDateItemCustomer.TotalRow = (InheritableBoolean)1;
		this.grdLoadingDateItemCustomer.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdLoadingDateItemCustomer).VisualStyle = (VisualStyle)4;
		this.grdLoadingDateItemCustomer.LinkClicked += new ColumnActionEventHandler(grdLoadingDateItemCustomer_LinkClicked);
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.ctrlGrdBar1);
		this.panel10.Controls.Add(this.label7);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 3);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1422, 27);
		this.panel10.TabIndex = 2;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1380, 2);
		this.ctrlGrdBar1.MyGrid = this.grdLoadingDateItemCustomer;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar1.TabIndex = 1;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.Teal;
		this.label7.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(5, 3);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(252, 18);
		this.label7.TabIndex = 0;
		this.label7.Text = "Loading Date && Item && Customer";
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdLoadingDateItem);
		this.tabPage2.Controls.Add(this.panel12);
		this.tabPage2.Location = new System.Drawing.Point(4, 22);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1428, 459);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Loading Date And Item Wise";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.ctrlGrdBar2);
		this.panel12.Controls.Add(this.label8);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 3);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1422, 27);
		this.panel12.TabIndex = 2;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1382, 2);
		this.ctrlGrdBar2.MyGrid = this.grdLoadingDateItem;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar2.TabIndex = 2;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(4, 4);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(161, 18);
		this.label8.TabIndex = 0;
		this.label8.Text = "Loading Date && Item";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(14, 75);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 15);
		this.label2.TabIndex = 3;
		this.label2.Text = "To Date";
		this.txtToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtToDate.Location = new System.Drawing.Point(98, 67);
		this.txtToDate.Name = "txtToDate";
		this.txtToDate.ShowCheckBox = true;
		this.txtToDate.Size = new System.Drawing.Size(137, 23);
		this.txtToDate.TabIndex = 2;
		this.BtnShowbyDates.BackColor = System.Drawing.Color.Teal;
		this.BtnShowbyDates.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.BtnShowbyDates.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnShowbyDates.ForeColor = System.Drawing.Color.White;
		this.BtnShowbyDates.Location = new System.Drawing.Point(181, 92);
		this.BtnShowbyDates.Name = "BtnShowbyDates";
		this.BtnShowbyDates.Size = new System.Drawing.Size(54, 23);
		this.BtnShowbyDates.TabIndex = 5;
		this.BtnShowbyDates.Text = "Show";
		this.BtnShowbyDates.UseVisualStyleBackColor = false;
		this.BtnShowbyDates.Click += new System.EventHandler(btnShow_Click);
		this.txtDateFrom.Checked = false;
		this.txtDateFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtDateFrom.Location = new System.Drawing.Point(98, 18);
		this.txtDateFrom.Name = "txtDateFrom";
		this.txtDateFrom.ShowCheckBox = true;
		this.txtDateFrom.Size = new System.Drawing.Size(137, 23);
		this.txtDateFrom.TabIndex = 0;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(14, 47);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(58, 15);
		this.label12.TabIndex = 165;
		this.label12.Text = "Add Days";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(14, 22);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 1;
		this.label1.Text = "From Date";
		this.tztAddDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tztAddDays.Location = new System.Drawing.Point(98, 43);
		this.tztAddDays.Name = "tztAddDays";
		this.tztAddDays.Size = new System.Drawing.Size(137, 23);
		this.tztAddDays.TabIndex = 1;
		this.tztAddDays.TextChanged += new System.EventHandler(tztAddDays_TextChanged);
		this.tztAddDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(tztAddDays_KeyPress);
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdDueWise);
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 58);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1436, 163);
		this.panel3.TabIndex = 0;
		this.grdDueWise.AllowEdit = (InheritableBoolean)2;
		this.grdDueWise.AlternatingColors = true;
		grdDueWise_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDueWise.DesignTimeLayout = grdDueWise_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDueWise).Dock = System.Windows.Forms.DockStyle.Left;
		this.grdDueWise.DynamicFiltering = true;
		this.grdDueWise.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDueWise).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDueWise.GroupByBoxVisible = false;
		this.grdDueWise.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.grdDueWise).Location = new System.Drawing.Point(241, 0);
		((System.Windows.Forms.Control)(object)this.grdDueWise).Name = "grdDueWise";
		((ScrollableBase)this.grdDueWise).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDueWise).Size = new System.Drawing.Size(511, 163);
		((System.Windows.Forms.Control)(object)this.grdDueWise).TabIndex = 172;
		this.grdDueWise.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDueWise).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdDueWise).DoubleClick += new System.EventHandler(grdDueWise_DoubleClick);
		this.groupBox1.Controls.Add(this.chkNotReferredStatus);
		this.groupBox1.Controls.Add(this.txtIntervalDays);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.txtprcntfclton);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.BtnShowbyDates);
		this.groupBox1.Controls.Add(this.tztAddDays);
		this.groupBox1.Controls.Add(this.txtDateFrom);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.txtToDate);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(241, 163);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters For Data";
		this.txtIntervalDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtIntervalDays.Location = new System.Drawing.Point(98, 92);
		this.txtIntervalDays.Name = "txtIntervalDays";
		this.txtIntervalDays.Size = new System.Drawing.Size(82, 23);
		this.txtIntervalDays.TabIndex = 4;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(14, 96);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(76, 15);
		this.label10.TabIndex = 169;
		this.label10.Text = "Interval Days";
		this.txtprcntfclton.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtprcntfclton.Location = new System.Drawing.Point(98, 117);
		this.txtprcntfclton.Name = "txtprcntfclton";
		this.txtprcntfclton.Size = new System.Drawing.Size(137, 23);
		this.txtprcntfclton.TabIndex = 3;
		this.txtprcntfclton.Visible = false;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(14, 122);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(80, 15);
		this.label3.TabIndex = 167;
		this.label3.Text = "Per FCL MTon";
		this.label3.Visible = false;
		this.chkNotReferredStatus.AutoSize = true;
		this.chkNotReferredStatus.Checked = true;
		this.chkNotReferredStatus.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkNotReferredStatus.Font = new System.Drawing.Font("Tahoma", 8.25f);
		this.chkNotReferredStatus.Location = new System.Drawing.Point(98, 142);
		this.chkNotReferredStatus.Name = "chkNotReferredStatus";
		this.chkNotReferredStatus.Size = new System.Drawing.Size(89, 17);
		this.chkNotReferredStatus.TabIndex = 170;
		this.chkNotReferredStatus.Text = "Not Referred";
		this.chkNotReferredStatus.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1436, 706);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.PanelDetail);
		base.KeyPreview = true;
		base.Name = "frmExportContractSchedulePeriodicB";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmEvaulationDetailSalesReports_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmExportContractSchedulePeriodic_KeyDown);
		this.PanelDetail.ResumeLayout(false);
		this.PanelDetail.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdLoadingDateItem).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdLoadingDateItemCustomer).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDueWise).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		base.ResumeLayout(false);
	}
}
