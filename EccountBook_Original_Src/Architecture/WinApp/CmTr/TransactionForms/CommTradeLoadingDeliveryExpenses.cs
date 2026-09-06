using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.CmTr.TransactionForm;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.CmTr.TransactionForm;
using Architecture.Model.Common;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.CmTr.TransactionForms;

public class CommTradeLoadingDeliveryExpenses : Form
{
	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtSupplierExpDetail = new DataTable();

	private DataTable dtCustomerExpDetail = new DataTable();

	private DataTable dtExpenseAccountTitleWith = new DataTable();

	private DataTable dtgrid = new DataTable();

	private int RecId;

	private int DefaultDaysToLessFromHistoryFromDate;

	private Rightsobjects formright = new Rightsobjects();

	private bool UpdateMode;

	private bool HasMultiLanguageFeature;

	private bool ExpenseGridsInTabOrinFront = true;

	private List<CommTradeLoadingDeliveryRevExp> lstRemoveRecord = new List<CommTradeLoadingDeliveryRevExp>();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel4;

	private Label label46;

	private Label label23;

	private ToolStrip toolStrip1;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton btnAttachment;

	private ToolStripButton btnshortcutkeys;

	internal GridEX grdSupplierExp;

	private Panel panel5;

	private ToolStrip miniToolStrip;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdHistorySupplierExpDetail;

	private Panel panel3;

	private Label label19;

	internal GridEX grdHistoryMain;

	private Panel panel6;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStrip toolStrip2;

	private Label label20;

	private Panel PanelCustomerExp;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator5;

	private Panel PanelSupplierExp;

	internal GridEX grdCustomerExp;

	private Panel panel8;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label21;

	private Label label33;

	private Panel panel2;

	private Panel panel9;

	internal GridEX grdHistoryCustomerExpDetail;

	private Panel panel10;

	private Label label1;

	private Panel panel7;

	public UltraCombo CmbLoadingDelivery;

	private Button BtnGenerateCustomerExpRows;

	private Panel panel12;

	private Label label2;

	private Panel panel11;

	private GroupBox groupBox7;

	private RadioButton rdapproveddate;

	private Button btnShowHistory;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Label label149;

	private Label label150;

	private Label label151;

	private Label label152;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	private TabControl tabControl2;

	private TabPage tabSupplierExp;

	private RadioButton rdTemplate2;

	private RadioButton rdTemplate1;

	private Panel PanelExpMain;

	internal UltraCombo CmbMultiLanguage;

	private Label labelmultilang;

	private ToolStripButton btnNew;

	private GroupBox GBaddAmount;

	public UltraCombo CmbSupplier;

	private Button BtnAddAmtInsteadOfRate;

	private Label label3;

	private TextBox txtAmount;

	private Label label4;

	public UltraCombo CmbExpense;

	private Label label5;

	public CommTradeLoadingDeliveryExpenses(UserAccount userAccount)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void CommTradeLoadingDelivery_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1180, 800);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar2.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ExpenseAccountTitleWithId();
			LoadingDeliveryFill();
			dtSupplierExpDetail.Columns.Add("Id", typeof(int));
			dtSupplierExpDetail.Columns.Add("LoadingDeliveryId", typeof(int));
			dtSupplierExpDetail.Columns.Add("SupplierId", typeof(int));
			dtSupplierExpDetail.Columns.Add("SupplierName", typeof(string));
			dtSupplierExpDetail.Columns.Add("TotalQty", typeof(double));
			dtSupplierExpDetail.Columns.Add("MachineStitching", typeof(string));
			dtSupplierExpDetail.Columns.Add("LabourWages", typeof(string));
			dtSupplierExpDetail.Columns.Add("SootriSalaee", typeof(string));
			dtSupplierExpDetail.Columns.Add("Insurance", typeof(string));
			dtSupplierExpDetail.Columns.Add("Munshiana", typeof(string));
			dtSupplierExpDetail.Columns.Add("ScreenPrinting", typeof(string));
			dtSupplierExpDetail.Columns.Add("TruckFreight", typeof(string));
			dtSupplierExpDetail.Columns.Add("MiscExpense", typeof(string));
			grdSupplierExp.DataSource = dtSupplierExpDetail;
			grdSupplierExp.RetrieveStructure();
			grdSupplierExpDetailSetting();
			dtCustomerExpDetail.Columns.Add("Id", typeof(int));
			dtCustomerExpDetail.Columns.Add("OtherRevenueId", typeof(int));
			dtCustomerExpDetail.Columns.Add("OtherRevenue", typeof(string));
			dtCustomerExpDetail.Columns.Add("Qty", typeof(double));
			dtCustomerExpDetail.Columns.Add("Rate", typeof(double));
			dtCustomerExpDetail.Columns.Add("AmountDr", typeof(double));
			dtCustomerExpDetail.Columns.Add("AmountCr", typeof(double));
			dtCustomerExpDetail.Columns.Add("CustomerIdDr", typeof(int));
			dtCustomerExpDetail.Columns.Add("CustomerDr", typeof(string));
			dtCustomerExpDetail.Columns.Add("SupplierIdCr", typeof(int));
			dtCustomerExpDetail.Columns.Add("SupplierCr", typeof(string));
			grdCustomerExp.DataSource = dtCustomerExpDetail;
			grdCustomerExp.RetrieveStructure();
			grdCustomerExpSetting();
			HasMultiLanguageFeature = CommonServices.GetERPFeatureById(7);
			if (HasMultiLanguageFeature)
			{
				labelmultilang.Visible = true;
				((Control)(object)CmbMultiLanguage).Visible = true;
				MultiLanguagefill();
			}
			else
			{
				labelmultilang.Visible = false;
				((Control)(object)CmbMultiLanguage).Visible = false;
			}
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			tabControl2.Visible = false;
			ChangeTemplate(ExpenseGridsInTabOrinFront);
			if (Conversion.ToInt(CmbLoadingDelivery.Value) > 0)
			{
				ReadById(Conversion.ToInt(CmbLoadingDelivery.Value));
			}
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDateHistory.Value = DateTime.Now;
			CmbLoadingDelivery.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Reset()
	{
		try
		{
			RecId = 0;
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			lstRemoveRecord.Clear();
			((Control)(object)CmbLoadingDelivery).Text = string.Empty;
			((Control)(object)CmbLoadingDelivery).Enabled = true;
			txtAmount.Text = string.Empty;
			((Control)(object)CmbSupplier).Text = string.Empty;
			((UltraGridBase)CmbSupplier).DataSource = null;
			dtSupplierExpDetail.Clear();
			grdSupplierExp.DataSource = null;
			dtCustomerExpDetail.Clear();
			grdCustomerExp.DataSource = null;
			AddRowOtherRevenue();
			CmbLoadingDelivery.Focus();
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			LoadingDeliveryFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommTradeLoadingDeliveryExpenses_FormClosed(object sender, FormClosedEventArgs e)
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

	private void CommTradeLoadingDeliveryExpenses_FormClosing(object sender, FormClosingEventArgs e)
	{
	}

	private void LoadingDeliveryFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbLoadingDelivery.Value);
			((Control)(object)CmbLoadingDelivery).Text = string.Empty;
			DataTable dt = CommTradeLoadingDelivery.GetLoadingNoForCombo(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (dt != null && dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbLoadingDelivery, "Id", "DocNo", "Loading Delivery No", ZeroIndex: false);
				((UltraGridBase)CmbLoadingDelivery).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbLoadingDelivery.Value = Id;
					}
					else
					{
						((Control)(object)CmbLoadingDelivery).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbLoadingDelivery).Text = string.Empty;
				((UltraGridBase)CmbLoadingDelivery).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MultiLanguagefill()
	{
		try
		{
			int id = Conversion.ToInt(CmbMultiLanguage.Value);
			DataTable dt = CommonServices.MultiLanguagesGetAll();
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, CmbMultiLanguage, "Id", "LanguageDescription", "Language", true);
			((UltraGridBase)CmbMultiLanguage).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					CmbMultiLanguage.Value = id;
				}
				else
				{
					((Control)(object)CmbMultiLanguage).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ExpenseAccountTitleWithId()
	{
		try
		{
			int Id = Conversion.ToInt(CmbExpense.Value);
			((Control)(object)CmbExpense).Text = string.Empty;
			dtExpenseAccountTitleWith.Clear();
			dtExpenseAccountTitleWith.Columns.Add("Id", typeof(int));
			dtExpenseAccountTitleWith.Columns.Add("ExpenseAccount", typeof(string));
			dtExpenseAccountTitleWith.Rows.Add(13, "MachineStitching");
			dtExpenseAccountTitleWith.Rows.Add(14, "LabourWages");
			dtExpenseAccountTitleWith.Rows.Add(15, "SootriSalaee");
			dtExpenseAccountTitleWith.Rows.Add(16, "Insurance");
			dtExpenseAccountTitleWith.Rows.Add(19, "Munshiana");
			dtExpenseAccountTitleWith.Rows.Add(20, "ScreenPrinting");
			dtExpenseAccountTitleWith.Rows.Add(21, "TruckFreight");
			dtExpenseAccountTitleWith.Rows.Add(22, "MiscExpense");
			if (dtExpenseAccountTitleWith != null && dtExpenseAccountTitleWith.Rows.Count > 0)
			{
				DDL.BindDDL(dtExpenseAccountTitleWith, CmbExpense, "Id", "ExpenseAccount", "ExpenseAccount", ZeroIndex: true);
				if (Id > 0)
				{
					if (dtExpenseAccountTitleWith.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbExpense.Value = Id;
					}
					else
					{
						((Control)(object)CmbExpense).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbExpense).Text = string.Empty;
				((UltraGridBase)CmbExpense).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbLoadingDelivery_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbLoadingDelivery).ActiveRow != null && Conversion.ToInt(CmbLoadingDelivery.Value) > 0)
			{
				ReadById(Conversion.ToInt(CmbLoadingDelivery.Value));
				SupplierBindForSupplierComboFromSupplierExpGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdTemplate1_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (rdTemplate1.Checked)
			{
				ExpenseGridsInTabOrinFront = false;
			}
			else
			{
				ExpenseGridsInTabOrinFront = true;
			}
			ChangeTemplate(ExpenseGridsInTabOrinFront);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChangeTemplate(bool Type)
	{
		try
		{
			tabControl2.TabPages.Remove(tabSupplierExp);
			if (Type)
			{
				PanelExpMain.Visible = true;
				tabControl2.Visible = true;
				tabControl2.TabPages.Add(tabSupplierExp);
				tabSupplierExp.Controls.Add(PanelSupplierExp);
				PanelSupplierExp.Dock = DockStyle.Fill;
				tabControl2.Dock = DockStyle.Top;
				PanelExpMain.Controls.Add(PanelCustomerExp);
				PanelCustomerExp.Dock = DockStyle.Fill;
				PanelCustomerExp.BringToFront();
			}
			else
			{
				PanelExpMain.Visible = true;
				tabControl2.Visible = false;
				PanelExpMain.Controls.Add(PanelSupplierExp);
				PanelSupplierExp.Dock = DockStyle.Left;
				PanelExpMain.Controls.Add(PanelCustomerExp);
				PanelCustomerExp.Dock = DockStyle.Fill;
				PanelCustomerExp.BringToFront();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnAddAmtInsteadOfRate_Click(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbExpense).ActiveRow == null || Conversion.ToInt(CmbExpense.Value) == 0)
			{
				CmbExpense.Focus();
				throw new Exception("Please Select an Expense");
			}
			if (Conversion.ToDecimal((object)txtAmount.Text) == 0m)
			{
				txtAmount.Focus();
				throw new Exception("Please add Amount");
			}
			ProportionateExpensesAmountInSupplierGrid(Conversion.ToInt(CmbSupplier.Value), Conversion.ToString((object)((Control)(object)CmbExpense).Text), Conversion.ToDecimal((object)txtAmount.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FillSupplierExpenseGridByLoadingId(int Id)
	{
		bool Exists = false;
		try
		{
			dtSupplierExpDetail.Clear();
			DataTable dtReadById = CommTradeLoadingDelivery.CommTradeLoadingDeliverySupplierExp_ReadByIds(Id.ToString());
			if (dtReadById.Rows.Count > 0)
			{
				Exists = true;
				Dictionary<string, DataRow> supplierAggregates = new Dictionary<string, DataRow>();
				foreach (DataRow row2 in dtReadById.Rows)
				{
					string supplierName = row2["SupplierName"].ToString();
					if (supplierAggregates.ContainsKey(supplierName))
					{
						DataRow existingRow = supplierAggregates[supplierName];
						existingRow["TotalQty"] = Conversion.ToDouble(row2["Qty"]);
						existingRow["MachineStitching"] = Convert.ToDouble(existingRow["MachineStitching"]) + ((row2["AccountTitle"].ToString() == "Machine Stitching") ? Convert.ToDouble(row2["ExpenseRate"]) : 0.0);
						existingRow["LabourWages"] = Convert.ToDouble(existingRow["LabourWages"]) + ((row2["AccountTitle"].ToString() == "Labour Wages") ? Convert.ToDouble(row2["ExpenseRate"]) : 0.0);
						existingRow["SootriSalaee"] = Convert.ToDouble(existingRow["SootriSalaee"]) + ((row2["AccountTitle"].ToString() == "Sootri Salaee") ? Convert.ToDouble(row2["ExpenseRate"]) : 0.0);
						existingRow["Insurance"] = Convert.ToDouble(existingRow["Insurance"]) + ((row2["AccountTitle"].ToString() == "Insurance") ? Convert.ToDouble(row2["ExpenseRate"]) : 0.0);
						existingRow["Munshiana"] = Convert.ToDouble(existingRow["Munshiana"]) + ((row2["AccountTitle"].ToString() == "Munshiana") ? Convert.ToDouble(row2["ExpenseRate"]) : 0.0);
						existingRow["ScreenPrinting"] = Convert.ToDouble(existingRow["ScreenPrinting"]) + ((row2["AccountTitle"].ToString() == "Screen Printing") ? Convert.ToDouble(row2["ExpenseRate"]) : 0.0);
						existingRow["TruckFreight"] = Convert.ToDouble(existingRow["TruckFreight"]) + ((row2["AccountTitle"].ToString() == "Truck Freight") ? Convert.ToDouble(row2["ExpenseRate"]) : 0.0);
						existingRow["MiscExpense"] = Convert.ToDouble(existingRow["MiscExpense"]) + ((row2["AccountTitle"].ToString() == "Misc Expense") ? Convert.ToDouble(row2["ExpenseRate"]) : 0.0);
						supplierAggregates[supplierName] = existingRow;
					}
					else
					{
						DataRow newRow = dtSupplierExpDetail.NewRow();
						newRow["LoadingDeliveryId"] = Id;
						newRow["SupplierId"] = Convert.ToInt32(row2["SupplierCustomerId"]);
						newRow["SupplierName"] = supplierName;
						newRow["TotalQty"] = Conversion.ToDouble(row2["Qty"]);
						newRow["MachineStitching"] = ((row2["AccountTitle"].ToString() == "Machine Stitching") ? row2["ExpenseRate"] : ((object)0));
						newRow["LabourWages"] = ((row2["AccountTitle"].ToString() == "Labour Wages") ? row2["ExpenseRate"] : ((object)0));
						newRow["SootriSalaee"] = ((row2["AccountTitle"].ToString() == "Sootri Salaee") ? row2["ExpenseRate"] : ((object)0));
						newRow["Insurance"] = ((row2["AccountTitle"].ToString() == "Insurance") ? row2["ExpenseRate"] : ((object)0));
						newRow["Munshiana"] = ((row2["AccountTitle"].ToString() == "Munshiana") ? row2["ExpenseRate"] : ((object)0));
						newRow["ScreenPrinting"] = ((row2["AccountTitle"].ToString() == "Screen Printing") ? row2["ExpenseRate"] : ((object)0));
						newRow["TruckFreight"] = ((row2["AccountTitle"].ToString() == "Truck Freight") ? row2["ExpenseRate"] : ((object)0));
						newRow["MiscExpense"] = ((row2["AccountTitle"].ToString() == "Misc Expense") ? row2["ExpenseRate"] : ((object)0));
						supplierAggregates.Add(supplierName, newRow);
						dtSupplierExpDetail.Rows.Add(newRow);
					}
				}
				if (!dtSupplierExpDetail.AsEnumerable().Any((DataRow row) => row.Field<int>("SupplierId") == 0))
				{
					dtSupplierExpDetail.Rows.Add(0, Id, 0, "", 0);
				}
			}
			else
			{
				Exists = false;
				btnSave.Visible = true;
				UpdateMode = false;
				btnUpdate.Visible = false;
				((Control)(object)CmbLoadingDelivery).Enabled = true;
				DataTable dt = CommTradeLoadingDelivery.CommTradeLoadingDeliveryDataForSupplierExpense(Id.ToString());
				if (dt.Rows.Count > 0)
				{
					dtSupplierExpDetail.Rows.Add(0, Id, 0, "AmountToProportionate", 0);
					foreach (DataRow r in dt.Rows)
					{
						dtSupplierExpDetail.Rows.Add(0, r["CommTradeLoadingDeliveryId"], r["SupplierId"], r["SupplierName"], r["ItemSupQty"]);
					}
				}
			}
			if (dtSupplierExpDetail.Rows.Count > 0)
			{
				grdSupplierExp.DataSource = dtSupplierExpDetail;
				grdSupplierExp.RetrieveStructure();
				grdSupplierExpDetailSetting();
			}
			else
			{
				grdSupplierExp.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return Exists;
	}

	private void grdSupplierExpDetailSetting()
	{
		try
		{
			grdSupplierExp.RootTable.Columns["Id"].Visible = false;
			grdSupplierExp.RootTable.Columns["SupplierId"].Visible = false;
			grdSupplierExp.RootTable.Columns["LoadingDeliveryId"].Visible = false;
			grdSupplierExp.RootTable.Columns["Id"].EditType = (EditType)0;
			grdSupplierExp.RootTable.Columns["SupplierId"].EditType = (EditType)0;
			grdSupplierExp.RootTable.Columns["LoadingDeliveryId"].EditType = (EditType)0;
			grdSupplierExp.RootTable.Columns["SupplierName"].EditType = (EditType)0;
			grdSupplierExp.RootTable.Columns["TotalQty"].EditType = (EditType)0;
			grdSupplierExp.RootTable.Columns["TotalQty"].AggregateFunction = (AggregateFunction)2;
			grdSupplierExp.RootTable.Columns["TotalQty"].TextAlignment = (TextAlignment)3;
			grdSupplierExp.RootTable.Columns["TotalQty"].FormatString = "#,##0.######";
			grdSupplierExp.RootTable.Columns["TotalQty"].TotalFormatString = "#,##0.######";
			grdSupplierExp.RootTable.Columns["MachineStitching"].AggregateFunction = (AggregateFunction)2;
			grdSupplierExp.RootTable.Columns["MachineStitching"].TextAlignment = (TextAlignment)3;
			grdSupplierExp.RootTable.Columns["MachineStitching"].FormatString = "#,##0.######";
			grdSupplierExp.RootTable.Columns["MachineStitching"].TotalFormatString = "#,##0.######";
			grdSupplierExp.RootTable.Columns["LabourWages"].AggregateFunction = (AggregateFunction)2;
			grdSupplierExp.RootTable.Columns["LabourWages"].TextAlignment = (TextAlignment)3;
			grdSupplierExp.RootTable.Columns["LabourWages"].FormatString = "#,##0.######";
			grdSupplierExp.RootTable.Columns["LabourWages"].TotalFormatString = "#,##0.######";
			grdSupplierExp.RootTable.Columns["SootriSalaee"].AggregateFunction = (AggregateFunction)2;
			grdSupplierExp.RootTable.Columns["SootriSalaee"].TextAlignment = (TextAlignment)3;
			grdSupplierExp.RootTable.Columns["SootriSalaee"].FormatString = "#,##0.######";
			grdSupplierExp.RootTable.Columns["SootriSalaee"].TotalFormatString = "#,##0.######";
			grdSupplierExp.RootTable.Columns["Insurance"].AggregateFunction = (AggregateFunction)2;
			grdSupplierExp.RootTable.Columns["Insurance"].TextAlignment = (TextAlignment)3;
			grdSupplierExp.RootTable.Columns["Insurance"].FormatString = "#,##0.######";
			grdSupplierExp.RootTable.Columns["Insurance"].TotalFormatString = "#,##0.######";
			grdSupplierExp.RootTable.Columns["Munshiana"].AggregateFunction = (AggregateFunction)2;
			grdSupplierExp.RootTable.Columns["Munshiana"].TextAlignment = (TextAlignment)3;
			grdSupplierExp.RootTable.Columns["Munshiana"].FormatString = "#,##0.######";
			grdSupplierExp.RootTable.Columns["Munshiana"].TotalFormatString = "#,##0.######";
			grdSupplierExp.RootTable.Columns["ScreenPrinting"].AggregateFunction = (AggregateFunction)2;
			grdSupplierExp.RootTable.Columns["ScreenPrinting"].TextAlignment = (TextAlignment)3;
			grdSupplierExp.RootTable.Columns["ScreenPrinting"].FormatString = "#,##0.######";
			grdSupplierExp.RootTable.Columns["ScreenPrinting"].TotalFormatString = "#,##0.######";
			grdSupplierExp.RootTable.Columns["TruckFreight"].AggregateFunction = (AggregateFunction)2;
			grdSupplierExp.RootTable.Columns["TruckFreight"].TextAlignment = (TextAlignment)3;
			grdSupplierExp.RootTable.Columns["TruckFreight"].FormatString = "#,##0.######";
			grdSupplierExp.RootTable.Columns["TruckFreight"].TotalFormatString = "#,##0.######";
			grdSupplierExp.RootTable.Columns["MiscExpense"].AggregateFunction = (AggregateFunction)2;
			grdSupplierExp.RootTable.Columns["MiscExpense"].TextAlignment = (TextAlignment)3;
			grdSupplierExp.RootTable.Columns["MiscExpense"].FormatString = "#,##0.######";
			grdSupplierExp.RootTable.Columns["MiscExpense"].TotalFormatString = "#,##0.######";
			CommonServices.GridAutoAdjustmentNew(grdSupplierExp);
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSupplierExp_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdSupplierExp.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				_ = grdSupplierExp.CurrentColumn;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProportionateExpensesInSupplierGrid(string ColumnName, bool Proportionate)
	{
		try
		{
			if (string.IsNullOrEmpty(ColumnName))
			{
				return;
			}
			decimal TotalAmt = default(decimal);
			decimal TotalQty = default(decimal);
			if (Proportionate)
			{
				if (grdSupplierExp.GetDataRows().Count() > 0)
				{
					TotalAmt = (from r in grdSupplierExp.GetDataRows()
						where Conversion.ToInt(r.Cells["SupplierId"].Value) == 0
						select r).Sum((GridEXRow r) => Conversion.ToDecimal(r.Cells[ColumnName].Value));
					TotalQty = (from r in grdSupplierExp.GetDataRows()
						where Conversion.ToInt(r.Cells["SupplierId"].Value) > 0
						select r).Sum((GridEXRow r) => Conversion.ToDecimal(r.Cells["TotalQty"].Value));
				}
				if (!(TotalAmt > 0m))
				{
					return;
				}
				{
					foreach (GridEXRow r2 in from rr in grdSupplierExp.GetDataRows()
						where rr.Cells[ColumnName].Value != null
						select rr)
					{
						if (Conversion.ToInt(r2.Cells["SupplierId"].Value) != 0)
						{
							r2.BeginEdit();
							decimal Exp = ((TotalQty > 0m) ? (TotalAmt / TotalQty) : 0m);
							r2.Cells[ColumnName].Value = Exp.ToString("#,##.######");
							r2.EndEdit();
						}
					}
					return;
				}
			}
			foreach (GridEXRow item in from rr in grdSupplierExp.GetDataRows()
				where rr.Cells[ColumnName].Value != null
				select rr)
			{
				Conversion.ToInt(item.Cells["SupplierId"].Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProportionateExpensesAmountInSupplierGrid(int supplierId, string expenseName, decimal amount)
	{
		try
		{
			if (string.IsNullOrEmpty(expenseName) || amount <= 0m)
			{
				return;
			}
			decimal totalQty = default(decimal);
			GridEXRow[] dataRows = grdSupplierExp.GetDataRows();
			if (dataRows.Count() == 0)
			{
				return;
			}
			totalQty = ((supplierId <= 0) ? dataRows.Sum((GridEXRow r) => Conversion.ToDecimal(r.Cells["TotalQty"].Value)) : dataRows.Where((GridEXRow r) => Conversion.ToInt(r.Cells["SupplierId"].Value) == supplierId).Sum((GridEXRow r) => Conversion.ToDecimal(r.Cells["TotalQty"].Value)));
			if (!(totalQty > 0m))
			{
				return;
			}
			foreach (GridEXRow r2 in dataRows.Where((GridEXRow rr) => rr.Cells[expenseName].Value != null))
			{
				if ((supplierId > 0) ? (Conversion.ToInt(r2.Cells["SupplierId"].Value) == supplierId) : ((byte)Conversion.ToInt(r2.Cells["SupplierId"].Value) != 0))
				{
					r2.BeginEdit();
					decimal expRate = amount / totalQty;
					r2.Cells[expenseName].Value = expRate.ToString("#,##.######");
					r2.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierBindForSupplierComboFromSupplierExpGrid()
	{
		try
		{
			if (grdSupplierExp.RowCount <= 0)
			{
				return;
			}
			int Id = Conversion.ToInt(CmbSupplier.Value);
			((Control)(object)CmbSupplier).Text = string.Empty;
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Name", typeof(string));
			GridEXRow[] rows = grdSupplierExp.GetRows();
			foreach (GridEXRow row2 in rows)
			{
				int SupplierId = Conversion.ToInt(row2.Cells["SupplierId"].Value);
				string SupplierName = Conversion.ToString((object)row2.Cells["SupplierName"].Text);
				if (!dt.AsEnumerable().Any((DataRow rr) => rr.Field<int>("Id") == SupplierId) && SupplierId > 0)
				{
					dt.Rows.Add(SupplierId, SupplierName);
				}
			}
			if (dt != null && dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbSupplier, "Id", "Name", "Supplier", ZeroIndex: true);
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbSupplier.Value = Id;
					}
					else
					{
						((Control)(object)CmbSupplier).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbSupplier).Text = string.Empty;
				((UltraGridBase)CmbSupplier).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnGenerateCustomerExpRows_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)CmbLoadingDelivery).Enabled = false;
			if (grdCustomerExp.GetDataRows().Count() <= 0 || MessageBox.Show("grid customer Expense Already Have Rows Do you Want To Reset And Load Again...?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				if (((UltraGridBase)CmbLoadingDelivery).ActiveRow == null || Conversion.ToInt(CmbLoadingDelivery.Value) == 0)
				{
					((Control)(object)CmbLoadingDelivery).Enabled = true;
					CmbLoadingDelivery.Focus();
					throw new Exception("Please Select Loading Delivery First...");
				}
				if (grdSupplierExp.RowCount == 0)
				{
					throw new Exception("Supplier Expense Grid Should have AtLest one Row...");
				}
				FillCustomerExpenseGridByLoadingIdAndSupplierGridData(Conversion.ToInt(CmbLoadingDelivery.Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillCustomerExpenseGridByLoadingIdAndSupplierGridData(int LoadingId)
	{
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Expected O, but got Unknown
		try
		{
			dtCustomerExpDetail.Clear();
			grdCustomerExp.ClearStructure();
			DataTable dtReadById = CommTradeLoadingDelivery.GetCustomersFromCommTradeLoadingDeliveryByIds(LoadingId.ToString());
			if (dtReadById.Rows.Count > 0)
			{
				foreach (DataRow dr in dtReadById.Rows)
				{
					GridEXRow[] rows = grdSupplierExp.GetRows();
					foreach (GridEXRow r in rows)
					{
						if (Conversion.ToInt(r.Cells["SupplierId"].Value) == Conversion.ToInt(dr["SupplierId"]))
						{
							foreach (GridEXColumn item in (JanusCollectionBase)grdSupplierExp.RootTable.Columns)
							{
								GridEXColumn c2 = item;
								if (((JanusCollectionElement)c2).Index > 4 && Conversion.ToDecimal(r.Cells[c2.Key].Value) > 0m)
								{
									int id2 = (from x in dtExpenseAccountTitleWith.AsEnumerable()
										where x.Field<string>("ExpenseAccount") == c2.Key
										select x.Field<int>("Id")).FirstOrDefault();
									dtCustomerExpDetail.Rows.Add(0, id2, c2.Key, dr["Qty"], r.Cells[c2.Key].Value, Conversion.ToDecimal(dr["Qty"]) * Conversion.ToDecimal(r.Cells[c2.Key].Value), 0, dr["CustomerId"], dr["CustomerName"], r.Cells["SupplierId"].Value, r.Cells["SupplierName"].Value);
								}
							}
						}
						else
						{
							if (Conversion.ToInt(r.Cells["SupplierId"].Value) != 0)
							{
								continue;
							}
							foreach (GridEXColumn item2 in (JanusCollectionBase)grdSupplierExp.RootTable.Columns)
							{
								GridEXColumn c = item2;
								if (((JanusCollectionElement)c).Index <= 4 || !(Conversion.ToDecimal(r.Cells[c.Key].Value) > 0m))
								{
									continue;
								}
								int id = (from x in dtExpenseAccountTitleWith.AsEnumerable()
									where x.Field<string>("ExpenseAccount") == c.Key
									select x.Field<int>("Id")).FirstOrDefault();
								if (!dtCustomerExpDetail.AsEnumerable().Any((DataRow row) => row.Field<int>("SupplierIdCr") == 0 && row.Field<int>("CustomerIdDr") == Conversion.ToInt(dr["CustomerId"]) && row.Field<int>("OtherRevenueId") == id))
								{
									decimal CustomerTotalQty = (from x in dtReadById.AsEnumerable()
										where x.Field<int>("CustomerId") == Conversion.ToInt(dr["CustomerId"])
										select x.Field<decimal>("Qty")).Sum();
									dtCustomerExpDetail.Rows.Add(0, id, c.Key, CustomerTotalQty, r.Cells[c.Key].Value, CustomerTotalQty * Conversion.ToDecimal(r.Cells[c.Key].Value), 0, dr["CustomerId"], dr["CustomerName"], r.Cells["SupplierId"].Value, r.Cells["SupplierName"].Value);
								}
							}
						}
					}
				}
			}
			if (dtCustomerExpDetail.Rows.Count > 0)
			{
				grdCustomerExp.DataSource = dtCustomerExpDetail;
				grdCustomerExp.RetrieveStructure();
				grdCustomerExpSetting();
			}
			else
			{
				grdCustomerExp.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCustomerExpSetting()
	{
		try
		{
			grdCustomerExp.RootTable.Columns["Id"].Visible = false;
			grdCustomerExp.RootTable.Columns["OtherRevenueId"].Visible = false;
			grdCustomerExp.RootTable.Columns["CustomerIdDr"].Visible = false;
			grdCustomerExp.RootTable.Columns["SupplierIdCr"].Visible = false;
			grdCustomerExp.RootTable.Columns["AmountCr"].Visible = false;
			grdCustomerExp.RootTable.Columns["Id"].EditType = (EditType)0;
			grdCustomerExp.RootTable.Columns["OtherRevenueId"].EditType = (EditType)0;
			grdCustomerExp.RootTable.Columns["OtherRevenue"].EditType = (EditType)0;
			grdCustomerExp.RootTable.Columns["Qty"].EditType = (EditType)0;
			grdCustomerExp.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdCustomerExp.RootTable.Columns["AmountDr"].EditType = (EditType)0;
			grdCustomerExp.RootTable.Columns["AmountCr"].EditType = (EditType)0;
			grdCustomerExp.RootTable.Columns["CustomerIdDr"].EditType = (EditType)0;
			grdCustomerExp.RootTable.Columns["CustomerDr"].EditType = (EditType)0;
			grdCustomerExp.RootTable.Columns["SupplierIdCr"].EditType = (EditType)0;
			grdCustomerExp.RootTable.Columns["SupplierCr"].EditType = (EditType)0;
			grdCustomerExp.GroupByBoxVisible = true;
			grdCustomerExp.GroupTotals = (GroupTotals)2;
			grdCustomerExp.RootTable.Columns["SupplierCr"].Visible = false;
			grdCustomerExp.RootTable.Groups.Add("SupplierCr");
			grdCustomerExp.RootTable.Columns["AmountCr"].AggregateFunction = (AggregateFunction)2;
			grdCustomerExp.RootTable.Columns["AmountCr"].TextAlignment = (TextAlignment)3;
			grdCustomerExp.RootTable.Columns["AmountCr"].FormatString = "#,##0.######";
			grdCustomerExp.RootTable.Columns["AmountCr"].TotalFormatString = "#,##0.######";
			grdCustomerExp.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdCustomerExp.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdCustomerExp.RootTable.Columns["Qty"].FormatString = "#,###.######";
			grdCustomerExp.RootTable.Columns["Qty"].TotalFormatString = "#,###.######";
			grdCustomerExp.RootTable.Columns["AmountDr"].AggregateFunction = (AggregateFunction)2;
			grdCustomerExp.RootTable.Columns["AmountDr"].TextAlignment = (TextAlignment)3;
			grdCustomerExp.RootTable.Columns["AmountDr"].FormatString = "#,##0.######";
			grdCustomerExp.RootTable.Columns["AmountDr"].TotalFormatString = "#,##0.######";
			grdCustomerExp.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)3;
			grdCustomerExp.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdCustomerExp.RootTable.Columns["Rate"].FormatString = "#,##0.######";
			grdCustomerExp.RootTable.Columns["Rate"].TotalFormatString = "#,##0.######";
			CommonServices.GridAutoAdjustmentNew(grdCustomerExp);
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddExpenseAccountRowsOtherRevenue()
	{
	}

	private void AddRowOtherRevenue()
	{
	}

	private void grdrevenue_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
	}

	private void grdrevenue_CellUpdated(object sender, ColumnActionEventArgs e)
	{
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)CmbLoadingDelivery).ActiveRow == null || Conversion.ToInt(CmbLoadingDelivery.Value) == 0)
		{
			MessageBox.Show("Loading Delivery Field is Required");
			CmbLoadingDelivery.Focus();
			return false;
		}
		return true;
	}

	private void ValidationBothGrids()
	{
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		try
		{
			foreach (var cn2 in (from row in dtCustomerExpDetail.AsEnumerable()
				select new
				{
					SupplierId = row.Field<int>("SupplierIdCr"),
					SupplierName = row.Field<string>("SupplierCr")
				}).Distinct().ToList())
			{
				if (cn2.SupplierId > 0 && !dtSupplierExpDetail.AsEnumerable().Any((DataRow row) => row.Field<int>("SupplierId") == cn2.SupplierId))
				{
					throw new Exception("Supplier " + cn2.SupplierName + " in 'Customer Expense Grid' not Exists in 'Supplier Expense Grid'");
				}
			}
			double supplierAmount = 0.0;
			double TotalTxpense = 0.0;
			GridEXRow[] rows = grdSupplierExp.GetRows();
			foreach (GridEXRow r in rows)
			{
				foreach (GridEXColumn item in (JanusCollectionBase)grdSupplierExp.RootTable.Columns)
				{
					GridEXColumn c = item;
					if (((JanusCollectionElement)c).Index <= 4 || !(Conversion.ToDecimal(r.Cells[c.Key].Value) > 0m))
					{
						continue;
					}
					if (Conversion.ToInt(r.Cells["SupplierId"].Value) > 0)
					{
						TotalTxpense += Conversion.ToDouble(r.Cells[c.Key].Value);
						supplierAmount += Conversion.ToDouble(r.Cells["TotalQty"].Value) * Conversion.ToDouble(r.Cells[c.Key].Value);
						continue;
					}
					double TotalQty = dtSupplierExpDetail.AsEnumerable().Sum((DataRow cn) => cn.Field<double>("TotalQty"));
					TotalTxpense += Conversion.ToDouble(r.Cells[c.Key].Value);
					supplierAmount += TotalQty * Conversion.ToDouble(r.Cells[c.Key].Value);
				}
			}
			double totalAmountDr = (from row in dtCustomerExpDetail.AsEnumerable()
				group row by new
				{
					SupplierIdCr = row.Field<int>("SupplierIdCr"),
					CustomerIdDr = row.Field<int>("CustomerIdDr"),
					OtherRevenueId = row.Field<int>("OtherRevenueId")
				} into @group
				select @group.First()).Sum((DataRow row) => row.Field<double>("AmountDr"));
			if (Conversion.ToDecimal((object)supplierAmount) != Conversion.ToDecimal((object)totalAmountDr))
			{
				throw new Exception("Amount Calculate From Supplier Expense Grid does not Match With Amount From Customer Expense Grid.");
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void Insert()
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Expected O, but got Unknown
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Expected O, but got Unknown
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (grdSupplierExp.GetRows().Count() == 0)
			{
				MessageBox.Show("Grid Supplier Exp Record Not Found");
				return;
			}
			if (grdCustomerExp.GetRows().Count() == 0)
			{
				MessageBox.Show("Grid Customer Exp Record Not Found");
				return;
			}
			CommTradeLoadingDelivery ct = new CommTradeLoadingDelivery();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				ct.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			FillCustomerExpenseGridByLoadingIdAndSupplierGridData(Conversion.ToInt(CmbLoadingDelivery.Value));
			ValidationBothGrids();
			GridEXRow[] dataRows = grdCustomerExp.GetDataRows();
			foreach (GridEXRow r3 in dataRows)
			{
				if ((Conversion.ToDouble(r3.Cells["AmountCr"].Value) > 0.0 || Conversion.ToInt(r3.Cells["AmountDr"].Value) > 0) && Conversion.ToInt(r3.Cells["OtherRevenueId"].Value) == 0)
				{
					MessageBox.Show("Please Select an Other Item In Revenue Grid In Row No" + (r3.RowIndex + 1));
					return;
				}
			}
			ct.OrganizationId = UserAccount.OrganizationId;
			ct.CompanyId = UserAccount.CompanyId;
			ct.BranchesId = UserAccount.BranchesId;
			ct.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			ct.DocumentTypeId = 1702;
			ct.ModifyUserId = UserAccount.ID;
			ct.EntryUserId = UserAccount.ID;
			ct.EntryDate = DateTime.Now;
			ct.ModifyDate = DateTime.Now;
			ct.ApprovedDate = DateTime.Now;
			ct.ScreenName = Conversion.ToString((object)base.Name);
			ct.CommTradeLoadingDeliveryDetailList = new List<CommTradeLoadingDeliveryDetail>();
			ct.CommTradeLoadingDeliveryRevExpList = new List<CommTradeLoadingDeliveryRevExp>();
			ct.CommTradeLoadingDeliveryPmOtherList = new List<CommTradeLoadingDeliveryPmOther>();
			ct.CommTradeLoadingDeliveryPaymentTermsDetailList = new List<CommTradeLoadingDeliveryPaymentTermsDetail>();
			ct.CommTradeLoadingDeliverySupplierExpList = new List<CommTradeLoadingDeliverySupplierExp>();
			ct.AttachmentsList = new List<DMSAttachments>();
			dataRows = grdSupplierExp.GetRows();
			foreach (GridEXRow r2 in dataRows)
			{
				foreach (GridEXColumn item in (JanusCollectionBase)grdSupplierExp.RootTable.Columns)
				{
					GridEXColumn c = item;
					if (((JanusCollectionElement)c).Index > 4 && Conversion.ToDecimal(r2.Cells[c.Key].Value) > 0m)
					{
						int id = (from x in dtExpenseAccountTitleWith.AsEnumerable()
							where x.Field<string>("ExpenseAccount") == c.Key
							select x.Field<int>("Id")).FirstOrDefault();
						CommTradeLoadingDeliverySupplierExp vd = new CommTradeLoadingDeliverySupplierExp();
						vd.Id = Conversion.ToInt(r2.Cells["Id"].Value);
						vd.CommTradeLoadingDeliveryId = Conversion.ToInt(r2.Cells["LoadingDeliveryId"].Value);
						vd.SupplierCustomerId = Conversion.ToInt(r2.Cells["SupplierId"].Value);
						vd.SupplierName = Conversion.ToString(r2.Cells["SupplierName"].Value);
						vd.Qty = Conversion.ToDecimal(r2.Cells["TotalQty"].Value);
						vd.ExpenseAccountId = id;
						vd.ExpenseRate = Conversion.ToDecimal(r2.Cells[c.Key].Value);
						ct.CommTradeLoadingDeliverySupplierExpList.Add(vd);
					}
				}
			}
			if (grdCustomerExp.RowCount > 0)
			{
				dataRows = grdCustomerExp.GetDataRows();
				foreach (GridEXRow r in dataRows)
				{
					if (Conversion.ToInt(r.Cells["OtherRevenueId"].Value) != 0)
					{
						CommTradeLoadingDeliveryRevExp PJ = new CommTradeLoadingDeliveryRevExp();
						PJ.Id = Conversion.ToInt(r.Cells["Id"].Value);
						PJ.CommTradeLoadingDeliveryId = Conversion.ToInt(CmbLoadingDelivery.Value);
						if (PJ.Id > 0)
						{
							PJ.ActionTypeId = 2;
						}
						else
						{
							PJ.ActionTypeId = 1;
						}
						PJ.AccountId = Conversion.ToInt(r.Cells["OtherRevenueId"].Value);
						PJ.SupplierCustomerId = Conversion.ToInt(r.Cells["SupplierIdCr"].Value);
						if (Conversion.ToInt(r.Cells["CustomerIdDr"].Value) == 0)
						{
							MessageBox.Show("Customer (Dr) Is Required In Revenue Grid In row No " + (r.RowIndex + 1));
							return;
						}
						PJ.SupplierCustomerIdDr = Conversion.ToInt(r.Cells["CustomerIdDr"].Value);
						PJ.Qty = Conversion.ToDecimal(r.Cells["Qty"].Value);
						PJ.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
						PJ.DebitAmt = Conversion.ToDecimal(r.Cells["AmountDr"].Value);
						PJ.CreditAmt = Conversion.ToDecimal(r.Cells["AmountCr"].Value);
						PJ.ReRemarks = "";
						ct.CommTradeLoadingDeliveryRevExpList.Add(PJ);
					}
				}
			}
			if (lstRemoveRecord.Count > 0)
			{
				for (int i = 0; i < lstRemoveRecord.Count; i++)
				{
					ct.CommTradeLoadingDeliveryRevExpList.Add(lstRemoveRecord[i]);
				}
			}
			FormHelper.UpdateAttachmentsForObject<CommTradeLoadingDelivery>(ct, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = CommTradeLoadingDelivery.SaveSupplierAndCustomerExpense(ct);
			ct.Id = success;
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + ct.DocNo);
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					DMSAttachments.RemoveByIdAndNames(RecId, base.Name, ct.DocumentTypeId, 0);
				}
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + ct.DocNo);
			}
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int ID)
	{
		try
		{
			Reset();
			RecId = ID;
			CommTradeLoadingDelivery po = CommTradeLoadingDelivery.ReadById(RecId);
			if (po == null)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			btnSave.Visible = false;
			UpdateMode = true;
			btnUpdate.Visible = true;
			CmbLoadingDelivery.Value = po.Id;
			((Control)(object)CmbLoadingDelivery).Enabled = false;
			if (!FillSupplierExpenseGridByLoadingId(po.Id))
			{
				return;
			}
			dtCustomerExpDetail.Rows.Clear();
			if (po.CommTradeLoadingDeliveryRevExpList.Count > 0)
			{
				for (int i = 0; i < po.CommTradeLoadingDeliveryRevExpList.Count; i++)
				{
					dtCustomerExpDetail.Rows.Add(po.CommTradeLoadingDeliveryRevExpList[i].Id, po.CommTradeLoadingDeliveryRevExpList[i].AccountId, po.CommTradeLoadingDeliveryRevExpList[i].AccountTitle, po.CommTradeLoadingDeliveryRevExpList[i].Qty, po.CommTradeLoadingDeliveryRevExpList[i].Rate, po.CommTradeLoadingDeliveryRevExpList[i].DebitAmt, po.CommTradeLoadingDeliveryRevExpList[i].CreditAmt, po.CommTradeLoadingDeliveryRevExpList[i].SupplierCustomerIdDr, po.CommTradeLoadingDeliveryRevExpList[i].SupplierCustomerNameCrDr, po.CommTradeLoadingDeliveryRevExpList[i].SupplierCustomerId, po.CommTradeLoadingDeliveryRevExpList[i].SupplierName);
				}
			}
			if (dtCustomerExpDetail.Rows.Count == 0)
			{
				AddRowOtherRevenue();
			}
			grdCustomerExp.DataSource = dtCustomerExpDetail;
			grdCustomerExp.RetrieveStructure();
			grdCustomerExpSetting();
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			CmbLoadingDelivery.Focus();
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
				FromDateHistory.Focus();
			}
			else
			{
				CmbLoadingDelivery.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnResetHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDateHistory.Value = DateTime.Now;
			FromDocNoHistory.Clear();
			ToDocNoHistory.Clear();
			grdHistoryMain.DataSource = null;
			grdHistorySupplierExpDetail.DataSource = null;
			grdHistoryCustomerExpDetail.DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnShowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryGridfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters Obj = new ReportsParameters();
			Obj.OrganizationId = UserAccount.OrganizationId;
			Obj.CompanyId = UserAccount.CompanyId;
			Obj.BranchesId = UserAccount.BranchesId;
			Obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			Obj.DocumentTypeId = 1702;
			Obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			Obj.EntryUser = UserAccount.ID;
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					Obj.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					Obj.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					Obj.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					Obj.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					Obj.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					Obj.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					Obj.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					Obj.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			Obj.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			Obj.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			dtgrid = CommTradeLoadingDelivery.FormHistory(Obj);
			if (dtgrid.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("DocNo", typeof(string));
				dt.Columns.Add("ManualRefNo", typeof(string));
				dt.Columns.Add("TotalQty", typeof(decimal));
				dt.Columns.Add("SupTotalNetWeight", typeof(decimal));
				dt.Columns.Add("CusTotalNetWeight", typeof(decimal));
				dt.Columns.Add("TotalRecOutQty", typeof(decimal));
				dt.Columns.Add("SupTotalRecWeight", typeof(decimal));
				dt.Columns.Add("CusTotalOutWeight", typeof(decimal));
				dt.Columns.Add("BalQty", typeof(decimal));
				dt.Columns.Add("BalSupWeight", typeof(decimal));
				dt.Columns.Add("BalCusWeight", typeof(decimal));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("IsApproved", typeof(bool));
				dt.Columns.Add("RemarksHeader", typeof(string));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				dt.Clear();
				for (int i = 0; i < dtgrid.Rows.Count; i++)
				{
					dt.Rows.Add(Conversion.ToInt(dtgrid.Rows[i]["DocumentTypeId"]), Conversion.ToInt(dtgrid.Rows[i]["Id"]), Conversion.ToDateTime(dtgrid.Rows[i]["DocDate"]), Conversion.ToString(dtgrid.Rows[i]["DocNo"]), Conversion.ToString(dtgrid.Rows[i]["ManualRefNo"]), Conversion.ToDecimal(dtgrid.Rows[i]["TotalQty"]), Conversion.ToDecimal(dtgrid.Rows[i]["SupTotalNetWeight"]), Conversion.ToDecimal(dtgrid.Rows[i]["CusTotalNetWeight"]), Conversion.ToDecimal(dtgrid.Rows[i]["TotalRecOutQty"]), Conversion.ToDecimal(dtgrid.Rows[i]["SupTotalRecWeight"]), Conversion.ToDecimal(dtgrid.Rows[i]["CusTotalOutWeight"]), Conversion.ToDecimal(dtgrid.Rows[i]["BalQty"]), Conversion.ToDecimal(dtgrid.Rows[i]["BalSupWeight"]), Conversion.ToDecimal(dtgrid.Rows[i]["BalCusWeight"]), Conversion.ToDateTime(dtgrid.Rows[i]["EntryDate"]), Conversion.ToString(dtgrid.Rows[i]["EntryUserName"]), Conversion.ToDateTime(dtgrid.Rows[i]["ModifyDate"]), Conversion.ToString(dtgrid.Rows[i]["ModifyUserName"]), Conversion.ToBool(dtgrid.Rows[i]["IsApproved"]), Conversion.ToString(dtgrid.Rows[i]["RemarksHeader"]), Conversion.ToInt(dtgrid.Rows[i]["NoOfAttachments"]));
				}
				grdHistoryMain.DataSource = dt;
				grdHistoryMain.RetrieveStructure();
				grdsetting();
			}
			else
			{
				grdHistoryMain.ClearStructure();
				grdHistoryCustomerExpDetail.ClearStructure();
				grdHistorySupplierExpDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdsetting()
	{
		try
		{
			grdHistoryMain.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdHistoryMain.RootTable.Columns["Id"].Visible = false;
			grdHistoryMain.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistoryMain.RootTable.Columns["TotalQty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryMain.RootTable.Columns["TotalQty"].TextAlignment = (TextAlignment)3;
			grdHistoryMain.RootTable.Columns["TotalQty"].FormatString = "#,##0.###";
			grdHistoryMain.RootTable.Columns["TotalQty"].TotalFormatString = "#,##0.###";
			grdHistoryMain.RootTable.Columns["SupTotalNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryMain.RootTable.Columns["SupTotalNetWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryMain.RootTable.Columns["SupTotalNetWeight"].FormatString = "#,##0.###";
			grdHistoryMain.RootTable.Columns["SupTotalNetWeight"].TotalFormatString = "#,##0.###";
			grdHistoryMain.RootTable.Columns["CusTotalNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryMain.RootTable.Columns["CusTotalNetWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryMain.RootTable.Columns["CusTotalNetWeight"].FormatString = "#,##0.###";
			grdHistoryMain.RootTable.Columns["CusTotalNetWeight"].TotalFormatString = "#,##0.###";
			grdHistoryMain.RootTable.Columns["TotalRecOutQty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryMain.RootTable.Columns["TotalRecOutQty"].TextAlignment = (TextAlignment)3;
			grdHistoryMain.RootTable.Columns["TotalRecOutQty"].FormatString = "#,##0.###";
			grdHistoryMain.RootTable.Columns["TotalRecOutQty"].TotalFormatString = "#,##0.###";
			grdHistoryMain.RootTable.Columns["SupTotalRecWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryMain.RootTable.Columns["SupTotalRecWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryMain.RootTable.Columns["SupTotalRecWeight"].FormatString = "#,##0.###";
			grdHistoryMain.RootTable.Columns["SupTotalRecWeight"].TotalFormatString = "#,##0.###";
			grdHistoryMain.RootTable.Columns["CusTotalOutWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryMain.RootTable.Columns["CusTotalOutWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryMain.RootTable.Columns["CusTotalOutWeight"].FormatString = "#,##0.###";
			grdHistoryMain.RootTable.Columns["CusTotalOutWeight"].TotalFormatString = "#,##0.###";
			grdHistoryMain.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryMain.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			grdHistoryMain.RootTable.Columns["BalQty"].FormatString = "#,##0.###";
			grdHistoryMain.RootTable.Columns["BalQty"].TotalFormatString = "#,##0.###";
			grdHistoryMain.RootTable.Columns["BalSupWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryMain.RootTable.Columns["BalSupWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryMain.RootTable.Columns["BalSupWeight"].FormatString = "#,##0.###";
			grdHistoryMain.RootTable.Columns["BalSupWeight"].TotalFormatString = "#,##0.###";
			grdHistoryMain.RootTable.Columns["BalCusWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryMain.RootTable.Columns["BalCusWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryMain.RootTable.Columns["BalCusWeight"].FormatString = "#,##0.###";
			grdHistoryMain.RootTable.Columns["BalCusWeight"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grdHistoryMain);
			grdHistoryMain.RootTable.Columns.Remove("Print");
			grdHistoryMain.RootTable.Columns.Remove("UrduPrint");
			grdHistoryMain.RootTable.Columns.Remove("Edit");
			if (formright.DoHaveUpdateRights)
			{
				grdHistoryMain.RootTable.Columns.Add("Edit");
				grdHistoryMain.RootTable.Columns["Edit"].Key = "Edit";
				grdHistoryMain.RootTable.Columns["Edit"].Caption = "Edit";
				grdHistoryMain.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdHistoryMain.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
				grdHistoryMain.RootTable.Columns["Edit"].ButtonText = "Edit";
				grdHistoryMain.RootTable.Columns["Edit"].Width = 50;
			}
			if (formright.DoHavePrintRights)
			{
				grdHistoryMain.RootTable.Columns.Add("Print");
				grdHistoryMain.RootTable.Columns["Print"].Key = "Print";
				grdHistoryMain.RootTable.Columns["Print"].Caption = "1702-Print";
				grdHistoryMain.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdHistoryMain.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
				grdHistoryMain.RootTable.Columns["Print"].ButtonText = "1702-Print";
				grdHistoryMain.RootTable.Columns["Print"].Width = 80;
				if (HasMultiLanguageFeature)
				{
					grdHistoryMain.RootTable.Columns.Add("UrduPrint");
					grdHistoryMain.RootTable.Columns["UrduPrint"].Key = "UrduPrint";
					grdHistoryMain.RootTable.Columns["UrduPrint"].Caption = "1702-Urdu";
					grdHistoryMain.RootTable.Columns["UrduPrint"].ButtonDisplayMode = (CellButtonDisplayMode)3;
					grdHistoryMain.RootTable.Columns["UrduPrint"].ButtonStyle = (ButtonStyle)5;
					grdHistoryMain.RootTable.Columns["UrduPrint"].ButtonText = "1702-Urdu";
					grdHistoryMain.RootTable.Columns["UrduPrint"].Width = 80;
				}
			}
			if (grdHistoryMain.RootTable.Columns.Contains("Edit"))
			{
				grdHistoryMain.RootTable.Columns["Edit"].Position = grdHistoryMain.RootTable.Columns["DocNo"].Position - 1;
				grdHistoryMain.FrozenColumns = grdHistoryMain.RootTable.Columns["DocNo"].Position - 1;
			}
			if (grdHistoryMain.RootTable.Columns.Contains("UrduPrint"))
			{
				grdHistoryMain.RootTable.Columns["UrduPrint"].Position = grdHistoryMain.RootTable.Columns["DocNo"].Position - 1;
				grdHistoryMain.FrozenColumns = grdHistoryMain.RootTable.Columns["DocNo"].Position - 1;
			}
			if (grdHistoryMain.RootTable.Columns.Contains("Print"))
			{
				grdHistoryMain.RootTable.Columns["Print"].Position = grdHistoryMain.RootTable.Columns["DocNo"].Position - 1;
				grdHistoryMain.FrozenColumns = grdHistoryMain.RootTable.Columns["DocNo"].Position - 1;
			}
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistoryMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int currId = Conversion.ToInt(r.Cells["Id"].Value);
				if (formright.DoHaveUpdateRights)
				{
					RecId = currId;
					ReadById(RecId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistoryMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int currId = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Edit" && formright.DoHaveUpdateRights)
				{
					RecId = currId;
					ReadById(RecId);
				}
				if (e.Column.Key == "Print" && formright.DoHavePrintRights)
				{
					GenerateSlip1702Print(currId);
				}
				if (e.Column.Key == "UrduPrint" && formright.DoHavePrintRights)
				{
					GenerateUrduSlip1702Print(currId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistoryMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), 1702);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistoryMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				grdHistoryCustomerExpDetailbind(Conversion.ToInt(r.Cells["Id"].Value));
				GridSupplierExpDetailbind(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryCustomerExpDetailbind(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			new CommTradeLoadingDelivery();
			DataTable dtdeta = CommTradeLoadingDelivery.CommTradeLoadingDeliveryRevExpByHeaderIds(Id.ToString());
			if (dtdeta.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("OtherRevenueId", typeof(int));
				dt.Columns.Add("OtherRevenue", typeof(string));
				dt.Columns.Add("Qty", typeof(double));
				dt.Columns.Add("Rate", typeof(double));
				dt.Columns.Add("AmountDr", typeof(double));
				dt.Columns.Add("AmountCr", typeof(double));
				dt.Columns.Add("CustomerIdDr", typeof(int));
				dt.Columns.Add("CustomerDr", typeof(string));
				dt.Columns.Add("SupplierIdCr", typeof(int));
				dt.Columns.Add("SupplierCr", typeof(string));
				dt.Clear();
				foreach (DataRow r in dtdeta.Rows)
				{
					dt.Rows.Add(r["Id"], r["AccountId"], r["AccountTitle"], r["Qty"], r["Rate"], r["DebitAmt"], r["CreditAmt"], r["SupplierCustomerIdDr"], r["SupplierCustomerNameDr"], r["SupplierCustomerId"], r["SupplierName"]);
				}
				grdHistoryCustomerExpDetail.DataSource = dt;
				grdHistoryCustomerExpDetail.RetrieveStructure();
				grdHistoryCustomerExpDetailSetting();
			}
			else
			{
				grdHistoryCustomerExpDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryCustomerExpDetailSetting()
	{
		try
		{
			grdHistoryCustomerExpDetail.RootTable.Columns["Id"].Visible = false;
			grdHistoryCustomerExpDetail.RootTable.Columns["OtherRevenueId"].Visible = false;
			grdHistoryCustomerExpDetail.RootTable.Columns["CustomerIdDr"].Visible = false;
			grdHistoryCustomerExpDetail.RootTable.Columns["SupplierIdCr"].Visible = false;
			grdHistoryCustomerExpDetail.RootTable.Columns["AmountCr"].Visible = false;
			grdHistoryCustomerExpDetail.GroupByBoxVisible = true;
			grdHistoryCustomerExpDetail.GroupTotals = (GroupTotals)2;
			grdHistoryCustomerExpDetail.RootTable.Columns["SupplierCr"].Visible = false;
			grdHistoryCustomerExpDetail.RootTable.Groups.Add("SupplierCr");
			grdHistoryCustomerExpDetail.RootTable.Columns["AmountCr"].AggregateFunction = (AggregateFunction)2;
			grdHistoryCustomerExpDetail.RootTable.Columns["AmountCr"].TextAlignment = (TextAlignment)3;
			grdHistoryCustomerExpDetail.RootTable.Columns["AmountCr"].FormatString = "#,##0.######";
			grdHistoryCustomerExpDetail.RootTable.Columns["AmountCr"].TotalFormatString = "#,##0.######";
			grdHistoryCustomerExpDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryCustomerExpDetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdHistoryCustomerExpDetail.RootTable.Columns["Qty"].FormatString = "#,###.######";
			grdHistoryCustomerExpDetail.RootTable.Columns["Qty"].TotalFormatString = "#,###.######";
			grdHistoryCustomerExpDetail.RootTable.Columns["AmountDr"].AggregateFunction = (AggregateFunction)2;
			grdHistoryCustomerExpDetail.RootTable.Columns["AmountDr"].TextAlignment = (TextAlignment)3;
			grdHistoryCustomerExpDetail.RootTable.Columns["AmountDr"].FormatString = "#,##0.######";
			grdHistoryCustomerExpDetail.RootTable.Columns["AmountDr"].TotalFormatString = "#,##0.######";
			grdHistoryCustomerExpDetail.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)3;
			grdHistoryCustomerExpDetail.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdHistoryCustomerExpDetail.RootTable.Columns["Rate"].FormatString = "#,##0.######";
			grdHistoryCustomerExpDetail.RootTable.Columns["Rate"].TotalFormatString = "#,##0.######";
			CommonServices.GridAutoAdjustmentNew(grdHistoryCustomerExpDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSupplierExpDetailbind(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			new CommTradeLoadingDelivery();
			DataTable dtdeta = CommTradeLoadingDelivery.CommTradeLoadingDeliverySupplierExp_ReadByIds(Id.ToString());
			if (dtdeta.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("LoadingDeliveryId", typeof(int));
				dt.Columns.Add("SupplierId", typeof(int));
				dt.Columns.Add("SupplierName", typeof(string));
				dt.Columns.Add("TotalQty", typeof(double));
				dt.Columns.Add("MachineStitching", typeof(string));
				dt.Columns.Add("LabourWages", typeof(string));
				dt.Columns.Add("SootriSalaee", typeof(string));
				dt.Columns.Add("Insurance", typeof(string));
				dt.Columns.Add("Munshiana", typeof(string));
				dt.Columns.Add("ScreenPrinting", typeof(string));
				dt.Columns.Add("TruckFreight", typeof(string));
				dt.Columns.Add("MiscExpense", typeof(string));
				dt.Clear();
				Dictionary<string, DataRow> supplierAggregates = new Dictionary<string, DataRow>();
				foreach (DataRow row in dtdeta.Rows)
				{
					string supplierName = row["SupplierName"].ToString();
					if (supplierAggregates.ContainsKey(supplierName))
					{
						DataRow existingRow = supplierAggregates[supplierName];
						existingRow["TotalQty"] = Conversion.ToDouble(row["Qty"]);
						existingRow["MachineStitching"] = Convert.ToDouble(existingRow["MachineStitching"]) + ((row["AccountTitle"].ToString() == "Machine Stitching") ? Convert.ToDouble(row["ExpenseRate"]) : 0.0);
						existingRow["LabourWages"] = Convert.ToDouble(existingRow["LabourWages"]) + ((row["AccountTitle"].ToString() == "Labour Wages") ? Convert.ToDouble(row["ExpenseRate"]) : 0.0);
						existingRow["SootriSalaee"] = Convert.ToDouble(existingRow["SootriSalaee"]) + ((row["AccountTitle"].ToString() == "Sootri Salaee") ? Convert.ToDouble(row["ExpenseRate"]) : 0.0);
						existingRow["Insurance"] = Convert.ToDouble(existingRow["Insurance"]) + ((row["AccountTitle"].ToString() == "Insurance") ? Convert.ToDouble(row["ExpenseRate"]) : 0.0);
						existingRow["Munshiana"] = Convert.ToDouble(existingRow["Munshiana"]) + ((row["AccountTitle"].ToString() == "Munshiana") ? Convert.ToDouble(row["ExpenseRate"]) : 0.0);
						existingRow["ScreenPrinting"] = Convert.ToDouble(existingRow["ScreenPrinting"]) + ((row["AccountTitle"].ToString() == "Screen Printing") ? Convert.ToDouble(row["ExpenseRate"]) : 0.0);
						existingRow["TruckFreight"] = Convert.ToDouble(existingRow["TruckFreight"]) + ((row["AccountTitle"].ToString() == "Truck Freight") ? Convert.ToDouble(row["ExpenseRate"]) : 0.0);
						existingRow["MiscExpense"] = Convert.ToDouble(existingRow["MiscExpense"]) + ((row["AccountTitle"].ToString() == "Misc Expense") ? Convert.ToDouble(row["ExpenseRate"]) : 0.0);
						supplierAggregates[supplierName] = existingRow;
					}
					else
					{
						DataRow newRow = dt.NewRow();
						newRow["LoadingDeliveryId"] = Id;
						newRow["SupplierId"] = Convert.ToInt32(row["SupplierCustomerId"]);
						newRow["SupplierName"] = supplierName;
						newRow["TotalQty"] = Conversion.ToDouble(row["Qty"]);
						newRow["MachineStitching"] = ((row["AccountTitle"].ToString() == "Machine Stitching") ? row["ExpenseRate"] : ((object)0));
						newRow["LabourWages"] = ((row["AccountTitle"].ToString() == "Labour Wages") ? row["ExpenseRate"] : ((object)0));
						newRow["SootriSalaee"] = ((row["AccountTitle"].ToString() == "Sootri Salaee") ? row["ExpenseRate"] : ((object)0));
						newRow["Insurance"] = ((row["AccountTitle"].ToString() == "Insurance") ? row["ExpenseRate"] : ((object)0));
						newRow["Munshiana"] = ((row["AccountTitle"].ToString() == "Munshiana") ? row["ExpenseRate"] : ((object)0));
						newRow["ScreenPrinting"] = ((row["AccountTitle"].ToString() == "Screen Printing") ? row["ExpenseRate"] : ((object)0));
						newRow["TruckFreight"] = ((row["AccountTitle"].ToString() == "Truck Freight") ? row["ExpenseRate"] : ((object)0));
						newRow["MiscExpense"] = ((row["AccountTitle"].ToString() == "Misc Expense") ? row["ExpenseRate"] : ((object)0));
						supplierAggregates.Add(supplierName, newRow);
						dt.Rows.Add(newRow);
					}
				}
				grdHistorySupplierExpDetail.DataSource = dt;
				grdHistorySupplierExpDetail.RetrieveStructure();
				grdHistorySupplierExpDetailSetting();
			}
			else
			{
				grdHistorySupplierExpDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistorySupplierExpDetailSetting()
	{
		try
		{
			grdHistorySupplierExpDetail.RootTable.Columns["Id"].Visible = false;
			grdHistorySupplierExpDetail.RootTable.Columns["SupplierId"].Visible = false;
			grdHistorySupplierExpDetail.RootTable.Columns["LoadingDeliveryId"].Visible = false;
			grdHistorySupplierExpDetail.RootTable.Columns["TotalQty"].AggregateFunction = (AggregateFunction)2;
			grdHistorySupplierExpDetail.RootTable.Columns["TotalQty"].TextAlignment = (TextAlignment)3;
			grdHistorySupplierExpDetail.RootTable.Columns["TotalQty"].FormatString = "#,##.######";
			grdHistorySupplierExpDetail.RootTable.Columns["TotalQty"].TotalFormatString = "#,##.######";
			CommonServices.GridAutoAdjustmentNew(grdHistorySupplierExpDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		try
		{
			AT.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommTradeLoadingDelivery_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					CmbLoadingDelivery.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					((Control)(object)grdHistoryMain).Focus();
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S && btnSave.Visible && btnSave.Enabled)
				{
					btnSave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled && UpdateMode)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2))
				{
					if (rdTemplate1.Checked)
					{
						rdTemplate2.Checked = true;
					}
					else
					{
						rdTemplate1.Checked = true;
					}
					if (rdTemplate1.Checked)
					{
						ExpenseGridsInTabOrinFront = false;
					}
					else
					{
						ExpenseGridsInTabOrinFront = true;
					}
					ChangeTemplate(ExpenseGridsInTabOrinFront);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					CmbLoadingDelivery.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				if (e.Control && e.KeyCode == Keys.F12)
				{
					AT.Show();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					((Control)(object)grdSupplierExp).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdCustomerExp).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					CmbLoadingDelivery.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Left)
				{
					if (((Control)(object)CmbLoadingDelivery).Focused)
					{
						((Control)(object)grdHistorySupplierExpDetail).Focus();
					}
					else if (((Control)(object)grdSupplierExp).Focused)
					{
						((Control)(object)grdCustomerExp).Focus();
					}
					else if (((Control)(object)grdCustomerExp).Focused)
					{
						CmbLoadingDelivery.Focus();
					}
					else
					{
						CmbLoadingDelivery.Focus();
					}
				}
				if (e.Control && e.Shift && e.KeyCode == Keys.L)
				{
					CmbMultiLanguage.Focus();
				}
			}
			else
			{
				if (tabControl1.SelectedIndex != 1)
				{
					return;
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdHistoryMain).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					FromDateHistory.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Left)
				{
					if (((Control)(object)grdHistoryMain).Focused)
					{
						((Control)(object)grdHistorySupplierExpDetail).Focus();
					}
					else if (((Control)(object)grdHistorySupplierExpDetail).Focused)
					{
						((Control)(object)grdHistoryCustomerExpDetail).Focus();
					}
					else if (((Control)(object)grdHistoryCustomerExpDetail).Focused)
					{
						((Control)(object)grdHistoryMain).Focus();
					}
					else
					{
						((Control)(object)grdHistoryMain).Focus();
					}
				}
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
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			if (tabControl1.SelectedIndex == 0)
			{
				dt.Rows.Add("Ctrl+S", "For Save");
				dt.Rows.Add("Ctrl+U", "For Update");
				dt.Rows.Add("Ctrl+F5", "For Focus on Loading Delivery combo");
				dt.Rows.Add("Ctrl+F10", "For Open Attachments");
				dt.Rows.Add("Ctrl+Num2", "For Template Change");
				dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Supplier Expense Grid");
				dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Loading Delivery combo");
				dt.Rows.Add("Ctrl+Left", "For Focus from one Grid To another");
			}
			else
			{
				dt.Rows.Add("Ctrl+S", "For Show");
				dt.Rows.Add("Ctrl+F5", "For Focus on From Date");
				dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
				dt.Rows.Add("Ctrl+ArrowUp", "For Focus On  From Date");
				dt.Rows.Add("Ctrl+Left", "For Focus from one Grid To another");
				dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Update Record");
				dt.Rows.Add("Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link");
			}
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshortcutkeys_Click(object sender, EventArgs e)
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

	private void grdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistoryMain.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdHistoryMain.CurrentColumn == null)
			{
				return;
			}
			int Id = Conversion.ToInt((object)item.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdHistoryMain.CurrentColumn.Key == "Edit")
				{
					RecId = Id;
					ReadById(Id);
				}
				if (grdHistoryMain.CurrentColumn.Key == "Print")
				{
					GenerateSlip1702Print(Id);
				}
				if (grdHistoryMain.CurrentColumn.Key == "UrduPrint")
				{
					GenerateUrduSlip1702Print(Id);
				}
				if (grdHistoryMain.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt((object)Id.ToString()), 1702);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return)
			{
				if (formright.DoHaveUpdateRights)
				{
					RecId = Id;
					ReadById(RecId);
				}
			}
			else if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
			{
				GenerateSlip1702Print(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdrevenue_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdCustomerExp.CurrentRow;
			if (item != null && (int)item.RowType == 0 && grdCustomerExp.CurrentColumn != null)
			{
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 32;
				}
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 46;
				}
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 68;
				}
				if (e.KeyCode == Keys.F1)
				{
					CommonServices.GridAutoAdjustmentNew(grdCustomerExp);
				}
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdSupplierExp).Name);
			if (fs != null)
			{
				grdSupplierExp.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistoryMain).Name);
			if (fs != null)
			{
				grdHistoryMain.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdCustomerExp).Name);
			if (fs != null)
			{
				grdCustomerExp.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void FromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void GenerateSlip1702Print(int PrintId)
	{
	}

	private void GenerateUrduSlip1702Print(int PrintId)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (Conversion.ToInt(CmbMultiLanguage.Value) <= 0)
			{
				tabControl1.SelectedIndex = 0;
				CmbMultiLanguage.Focus();
				MessageBox.Show("Please Select Language!", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.Id = PrintId;
			obj.DocumentTypeId = 1702;
			obj.LanguageId = Conversion.ToInt(CmbMultiLanguage.Value);
			DataTable dt = CommTradeLoadingDelivery.CommissionTradeLoadingDeliverySlipAndRegister(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(CommTradeLoadingDelivery.CommTradeLoadingDeliveryExpense_SubReport(obj.Id), "CommTradeLoadingDeliveryExpense_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1702-CommissionTradeLoadingDelivery_Slip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
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
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Expected O, but got Unknown
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Expected O, but got Unknown
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Expected O, but got Unknown
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Expected O, but got Unknown
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Expected O, but got Unknown
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Expected O, but got Unknown
		//IL_0425: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Expected O, but got Unknown
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_045b: Expected O, but got Unknown
		//IL_047d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Expected O, but got Unknown
		//IL_0f5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f68: Expected O, but got Unknown
		//IL_0f75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7f: Expected O, but got Unknown
		//IL_139d: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a7: Expected O, but got Unknown
		//IL_2b11: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b1b: Expected O, but got Unknown
		//IL_2b3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b49: Expected O, but got Unknown
		GridEXLayout grdCustomerExp_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdSupplierExp_DesignTimeLayout = new GridEXLayout();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
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
		GridEXLayout grdHistoryMain_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryCustomerExpDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistorySupplierExpDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.PanelExpMain = new System.Windows.Forms.Panel();
		this.PanelCustomerExp = new System.Windows.Forms.Panel();
		this.grdCustomerExp = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.BtnGenerateCustomerExpRows = new System.Windows.Forms.Button();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label21 = new System.Windows.Forms.Label();
		this.PanelSupplierExp = new System.Windows.Forms.Panel();
		this.grdSupplierExp = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label46 = new System.Windows.Forms.Label();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabSupplierExp = new System.Windows.Forms.TabPage();
		this.panel4 = new System.Windows.Forms.Panel();
		this.GBaddAmount = new System.Windows.Forms.GroupBox();
		this.CmbSupplier = new UltraCombo();
		this.BtnAddAmtInsteadOfRate = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbExpense = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.rdTemplate2 = new System.Windows.Forms.RadioButton();
		this.rdTemplate1 = new System.Windows.Forms.RadioButton();
		this.CmbLoadingDelivery = new UltraCombo();
		this.label33 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdHistoryMain = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel11 = new System.Windows.Forms.Panel();
		this.groupBox7 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.label149 = new System.Windows.Forms.Label();
		this.label150 = new System.Windows.Forms.Label();
		this.label151 = new System.Windows.Forms.Label();
		this.label152 = new System.Windows.Forms.Label();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdHistoryCustomerExpDetail = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdHistorySupplierExpDetail = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label19 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.CmbMultiLanguage = new UltraCombo();
		this.labelmultilang = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.label20 = new System.Windows.Forms.Label();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.PanelExpMain.SuspendLayout();
		this.PanelCustomerExp.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdCustomerExp).BeginInit();
		this.panel8.SuspendLayout();
		this.PanelSupplierExp.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSupplierExp).BeginInit();
		this.panel5.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.panel4.SuspendLayout();
		this.GBaddAmount.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbExpense).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLoadingDelivery).BeginInit();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryMain).BeginInit();
		this.panel12.SuspendLayout();
		this.panel11.SuspendLayout();
		this.groupBox7.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryCustomerExpDetail).BeginInit();
		this.panel10.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistorySupplierExpDetail).BeginInit();
		this.panel3.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMultiLanguage).BeginInit();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.label23);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1170, 50);
		this.panel1.TabIndex = 29;
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.Teal;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 27);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(301, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "Commission Trade Loading Expense";
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnNew, this.btnRefresh, this.toolStripSeparator2, this.btnUpdate, this.btnSave, this.toolStripSeparator3, this.btnAttachment, this.toolStripSeparator5, this.btnshortcutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1170, 25);
		this.toolStrip1.TabIndex = 28;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 22);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 22);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(74, 22);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(60, 22);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(105, 22);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Visible = false;
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(122, 22);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1184, 561);
		this.tabControl1.TabIndex = 30;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.PanelExpMain);
		this.tabPage1.Controls.Add(this.tabControl2);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1176, 532);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.PanelExpMain.Controls.Add(this.PanelCustomerExp);
		this.PanelExpMain.Controls.Add(this.PanelSupplierExp);
		this.PanelExpMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanelExpMain.Location = new System.Drawing.Point(3, 361);
		this.PanelExpMain.Name = "PanelExpMain";
		this.PanelExpMain.Size = new System.Drawing.Size(1170, 168);
		this.PanelExpMain.TabIndex = 36;
		this.PanelCustomerExp.BackColor = System.Drawing.Color.White;
		this.PanelCustomerExp.Controls.Add((System.Windows.Forms.Control)(object)this.grdCustomerExp);
		this.PanelCustomerExp.Controls.Add(this.panel8);
		this.PanelCustomerExp.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanelCustomerExp.Location = new System.Drawing.Point(666, 0);
		this.PanelCustomerExp.Name = "PanelCustomerExp";
		this.PanelCustomerExp.Size = new System.Drawing.Size(504, 168);
		this.PanelCustomerExp.TabIndex = 34;
		((ScrollableBase)this.grdCustomerExp).BackColor = System.Drawing.Color.White;
		grdCustomerExp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdCustomerExp.DesignTimeLayout = grdCustomerExp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdCustomerExp).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdCustomerExp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdCustomerExp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdCustomerExp).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdCustomerExp).Name = "grdCustomerExp";
		((ScrollableBase)this.grdCustomerExp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdCustomerExp).Size = new System.Drawing.Size(504, 142);
		((System.Windows.Forms.Control)(object)this.grdCustomerExp).TabIndex = 0;
		this.grdCustomerExp.TotalRow = (InheritableBoolean)1;
		this.grdCustomerExp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdCustomerExp).VisualStyle = (VisualStyle)4;
		this.grdCustomerExp.CellUpdated += new ColumnActionEventHandler(grdrevenue_CellUpdated);
		this.grdCustomerExp.ColumnButtonClick += new ColumnActionEventHandler(grdrevenue_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdCustomerExp).KeyDown += new System.Windows.Forms.KeyEventHandler(grdrevenue_KeyDown);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.BtnGenerateCustomerExpRows);
		this.panel8.Controls.Add(this.ctrlGrdBar3);
		this.panel8.Controls.Add(this.label21);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(504, 26);
		this.panel8.TabIndex = 33;
		this.BtnGenerateCustomerExpRows.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnGenerateCustomerExpRows.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnGenerateCustomerExpRows.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnGenerateCustomerExpRows.ForeColor = System.Drawing.Color.Cyan;
		this.BtnGenerateCustomerExpRows.Location = new System.Drawing.Point(240, 1);
		this.BtnGenerateCustomerExpRows.Name = "BtnGenerateCustomerExpRows";
		this.BtnGenerateCustomerExpRows.Size = new System.Drawing.Size(213, 25);
		this.BtnGenerateCustomerExpRows.TabIndex = 46;
		this.BtnGenerateCustomerExpRows.Text = "Generate Customer Exp Rows";
		this.BtnGenerateCustomerExpRows.UseVisualStyleBackColor = false;
		this.BtnGenerateCustomerExpRows.Click += new System.EventHandler(BtnGenerateCustomerExpRows_Click);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(459, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
		this.ctrlGrdBar3.MyGrid = this.grdCustomerExp;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(41, 23);
		this.ctrlGrdBar3.TabIndex = 45;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label21.AutoSize = true;
		this.label21.BackColor = System.Drawing.Color.Teal;
		this.label21.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.White;
		this.label21.Location = new System.Drawing.Point(8, 3);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(236, 16);
		this.label21.TabIndex = 0;
		this.label21.Text = "Customer Expenses (Supplier Wise)";
		this.PanelSupplierExp.Controls.Add((System.Windows.Forms.Control)(object)this.grdSupplierExp);
		this.PanelSupplierExp.Controls.Add(this.panel5);
		this.PanelSupplierExp.Dock = System.Windows.Forms.DockStyle.Left;
		this.PanelSupplierExp.Location = new System.Drawing.Point(0, 0);
		this.PanelSupplierExp.Name = "PanelSupplierExp";
		this.PanelSupplierExp.Size = new System.Drawing.Size(666, 168);
		this.PanelSupplierExp.TabIndex = 35;
		((ScrollableBase)this.grdSupplierExp).BackColor = System.Drawing.Color.White;
		grdSupplierExp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSupplierExp.DesignTimeLayout = grdSupplierExp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSupplierExp).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdSupplierExp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSupplierExp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSupplierExp).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdSupplierExp).Name = "grdSupplierExp";
		((ScrollableBase)this.grdSupplierExp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSupplierExp).Size = new System.Drawing.Size(666, 142);
		((System.Windows.Forms.Control)(object)this.grdSupplierExp).TabIndex = 0;
		this.grdSupplierExp.TotalRow = (InheritableBoolean)1;
		this.grdSupplierExp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSupplierExp).VisualStyle = (VisualStyle)4;
		this.grdSupplierExp.CellUpdated += new ColumnActionEventHandler(grdSupplierExp_CellUpdated);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Controls.Add(this.label46);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(666, 26);
		this.panel5.TabIndex = 31;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(622, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(7, 5, 7, 5);
		this.ctrlGrdBar1.MyGrid = this.grdSupplierExp;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 23);
		this.ctrlGrdBar1.TabIndex = 45;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label46.AutoSize = true;
		this.label46.BackColor = System.Drawing.Color.Teal;
		this.label46.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.ForeColor = System.Drawing.Color.White;
		this.label46.Location = new System.Drawing.Point(5, 5);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(116, 16);
		this.label46.TabIndex = 0;
		this.label46.Text = "Supplier Expense";
		this.tabControl2.Controls.Add(this.tabSupplierExp);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Top;
		this.tabControl2.Location = new System.Drawing.Point(3, 107);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1170, 254);
		this.tabControl2.TabIndex = 1;
		this.tabSupplierExp.Location = new System.Drawing.Point(4, 25);
		this.tabSupplierExp.Name = "tabSupplierExp";
		this.tabSupplierExp.Padding = new System.Windows.Forms.Padding(3);
		this.tabSupplierExp.Size = new System.Drawing.Size(1162, 225);
		this.tabSupplierExp.TabIndex = 0;
		this.tabSupplierExp.Text = "Supplier Expense";
		this.tabSupplierExp.UseVisualStyleBackColor = true;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.GBaddAmount);
		this.panel4.Controls.Add(this.rdTemplate2);
		this.panel4.Controls.Add(this.rdTemplate1);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLoadingDelivery);
		this.panel4.Controls.Add(this.label33);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 53);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1170, 54);
		this.panel4.TabIndex = 0;
		this.GBaddAmount.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.GBaddAmount.Controls.Add(this.BtnAddAmtInsteadOfRate);
		this.GBaddAmount.Controls.Add(this.label3);
		this.GBaddAmount.Controls.Add(this.txtAmount);
		this.GBaddAmount.Controls.Add(this.label4);
		this.GBaddAmount.Controls.Add((System.Windows.Forms.Control)(object)this.CmbExpense);
		this.GBaddAmount.Controls.Add(this.label5);
		this.GBaddAmount.Location = new System.Drawing.Point(242, 2);
		this.GBaddAmount.Name = "GBaddAmount";
		this.GBaddAmount.Size = new System.Drawing.Size(710, 47);
		this.GBaddAmount.TabIndex = 1;
		this.GBaddAmount.TabStop = false;
		this.GBaddAmount.Text = "Add Amount Instead of Rate";
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance26;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance31).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(54, 18);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(229, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 0;
		this.BtnAddAmtInsteadOfRate.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnAddAmtInsteadOfRate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAddAmtInsteadOfRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAddAmtInsteadOfRate.ForeColor = System.Drawing.Color.White;
		this.BtnAddAmtInsteadOfRate.Location = new System.Drawing.Point(655, 20);
		this.BtnAddAmtInsteadOfRate.Name = "BtnAddAmtInsteadOfRate";
		this.BtnAddAmtInsteadOfRate.Size = new System.Drawing.Size(47, 23);
		this.BtnAddAmtInsteadOfRate.TabIndex = 3;
		this.BtnAddAmtInsteadOfRate.Text = "Add";
		this.BtnAddAmtInsteadOfRate.UseVisualStyleBackColor = false;
		this.BtnAddAmtInsteadOfRate.Click += new System.EventHandler(BtnAddAmtInsteadOfRate_Click);
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.White;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(2, 24);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(51, 15);
		this.label3.TabIndex = 105;
		this.label3.Text = "Supplier";
		this.txtAmount.Location = new System.Drawing.Point(549, 20);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.Size = new System.Drawing.Size(100, 23);
		this.txtAmount.TabIndex = 2;
		this.txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAmount_KeyPress);
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.White;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(288, 24);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(50, 15);
		this.label4.TabIndex = 104;
		this.label4.Text = "Expense";
		this.CmbExpense.AllowNull = (DefaultableBoolean)1;
		this.CmbExpense.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbExpense.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbExpense.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbExpense).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbExpense).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbExpense).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbExpense).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExpense).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbExpense).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExpense).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbExpense).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbExpense).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbExpense).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbExpense).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbExpense).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbExpense).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExpense).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbExpense).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbExpense).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbExpense).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExpense).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbExpense).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbExpense).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbExpense).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbExpense).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbExpense).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbExpense).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbExpense).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbExpense).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbExpense).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbExpense).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbExpense.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbExpense).Location = new System.Drawing.Point(342, 18);
		((System.Windows.Forms.Control)(object)this.CmbExpense).Name = "CmbExpense";
		((System.Windows.Forms.Control)(object)this.CmbExpense).Size = new System.Drawing.Size(154, 26);
		((System.Windows.Forms.Control)(object)this.CmbExpense).TabIndex = 1;
		this.label5.AutoSize = true;
		this.label5.BackColor = System.Drawing.Color.White;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(500, 24);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(51, 15);
		this.label5.TabIndex = 103;
		this.label5.Text = "Amount";
		this.rdTemplate2.AutoSize = true;
		this.rdTemplate2.Checked = true;
		this.rdTemplate2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdTemplate2.Location = new System.Drawing.Point(1073, 28);
		this.rdTemplate2.Name = "rdTemplate2";
		this.rdTemplate2.Size = new System.Drawing.Size(85, 19);
		this.rdTemplate2.TabIndex = 3;
		this.rdTemplate2.TabStop = true;
		this.rdTemplate2.Text = "Template II";
		this.rdTemplate2.UseVisualStyleBackColor = true;
		this.rdTemplate1.AutoSize = true;
		this.rdTemplate1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdTemplate1.Location = new System.Drawing.Point(1074, 6);
		this.rdTemplate1.Name = "rdTemplate1";
		this.rdTemplate1.Size = new System.Drawing.Size(81, 19);
		this.rdTemplate1.TabIndex = 2;
		this.rdTemplate1.Text = "Template I";
		this.rdTemplate1.UseVisualStyleBackColor = true;
		this.rdTemplate1.CheckedChanged += new System.EventHandler(rdTemplate1_CheckedChanged);
		this.CmbLoadingDelivery.AllowNull = (DefaultableBoolean)1;
		this.CmbLoadingDelivery.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLoadingDelivery.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLoadingDelivery.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLoadingDelivery).DisplayLayout.Appearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbLoadingDelivery).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLoadingDelivery).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLoadingDelivery).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLoadingDelivery).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance13;
		((SpecialBoxBase)((UltraGridBase)this.CmbLoadingDelivery).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLoadingDelivery).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbLoadingDelivery).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLoadingDelivery).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLoadingDelivery).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLoadingDelivery).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbLoadingDelivery).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLoadingDelivery).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLoadingDelivery).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance18).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLoadingDelivery).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbLoadingDelivery).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLoadingDelivery).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLoadingDelivery).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLoadingDelivery).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbLoadingDelivery).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLoadingDelivery).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLoadingDelivery).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbLoadingDelivery).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLoadingDelivery).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbLoadingDelivery).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLoadingDelivery).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLoadingDelivery).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLoadingDelivery).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLoadingDelivery.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLoadingDelivery).Location = new System.Drawing.Point(7, 23);
		((System.Windows.Forms.Control)(object)this.CmbLoadingDelivery).Name = "CmbLoadingDelivery";
		((System.Windows.Forms.Control)(object)this.CmbLoadingDelivery).Size = new System.Drawing.Size(229, 26);
		((System.Windows.Forms.Control)(object)this.CmbLoadingDelivery).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbLoadingDelivery).Leave += new System.EventHandler(CmbLoadingDelivery_Leave);
		this.label33.AutoSize = true;
		this.label33.BackColor = System.Drawing.Color.White;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(7, 5);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(108, 15);
		this.label33.TabIndex = 7;
		this.label33.Text = "Loading_Delivery #";
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryMain);
		this.tabPage2.Controls.Add(this.panel12);
		this.tabPage2.Controls.Add(this.panel11);
		this.tabPage2.Controls.Add(this.panel2);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1176, 532);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.grdHistoryMain.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdHistoryMain).BackColor = System.Drawing.Color.White;
		this.grdHistoryMain.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdHistoryMain_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryMain.DesignTimeLayout = grdHistoryMain_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryMain).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryMain.FilterMode = (FilterMode)2;
		this.grdHistoryMain.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryMain).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryMain.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistoryMain).Location = new System.Drawing.Point(3, 121);
		((System.Windows.Forms.Control)(object)this.grdHistoryMain).Name = "grdHistoryMain";
		((ScrollableBase)this.grdHistoryMain).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryMain).Size = new System.Drawing.Size(1170, 131);
		((System.Windows.Forms.Control)(object)this.grdHistoryMain).TabIndex = 0;
		this.grdHistoryMain.TotalRow = (InheritableBoolean)1;
		this.grdHistoryMain.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryMain).VisualStyle = (VisualStyle)4;
		this.grdHistoryMain.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistoryMain.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistoryMain.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistoryMain).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistoryMain).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label2);
		this.panel12.Controls.Add(this.ctrlGrdBar2);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 97);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1170, 24);
		this.panel12.TabIndex = 18;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(4, 4);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(116, 17);
		this.label2.TabIndex = 40;
		this.label2.Text = "Fillered Records";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1126, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(7, 4, 7, 4);
		this.ctrlGrdBar2.MyGrid = this.grdHistoryMain;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(40, 21);
		this.ctrlGrdBar2.TabIndex = 44;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel11.Controls.Add(this.groupBox7);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 51);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1170, 46);
		this.panel11.TabIndex = 17;
		this.groupBox7.Controls.Add(this.rdapproveddate);
		this.groupBox7.Controls.Add(this.btnShowHistory);
		this.groupBox7.Controls.Add(this.rdmodifydate);
		this.groupBox7.Controls.Add(this.drdocdate);
		this.groupBox7.Controls.Add(this.rdentrydate);
		this.groupBox7.Controls.Add(this.label149);
		this.groupBox7.Controls.Add(this.label150);
		this.groupBox7.Controls.Add(this.label151);
		this.groupBox7.Controls.Add(this.label152);
		this.groupBox7.Controls.Add(this.ToDocNoHistory);
		this.groupBox7.Controls.Add(this.FromDocNoHistory);
		this.groupBox7.Controls.Add(this.ToDateHistory);
		this.groupBox7.Controls.Add(this.FromDateHistory);
		this.groupBox7.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox7.Location = new System.Drawing.Point(0, 2);
		this.groupBox7.Name = "groupBox7";
		this.groupBox7.Size = new System.Drawing.Size(1167, 44);
		this.groupBox7.TabIndex = 0;
		this.groupBox7.TabStop = false;
		this.groupBox7.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(1045, 18);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 8;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(714, 16);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 4;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(948, 18);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 7;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(781, 18);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 5;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(862, 18);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 6;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.label149.AutoSize = true;
		this.label149.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label149.Location = new System.Drawing.Point(569, 20);
		this.label149.Name = "label149";
		this.label149.Size = new System.Drawing.Size(64, 15);
		this.label149.TabIndex = 86;
		this.label149.Text = "To Doc No";
		this.label150.AutoSize = true;
		this.label150.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label150.Location = new System.Drawing.Point(407, 20);
		this.label150.Name = "label150";
		this.label150.Size = new System.Drawing.Size(79, 15);
		this.label150.TabIndex = 85;
		this.label150.Text = "From Doc No";
		this.label151.AutoSize = true;
		this.label151.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label151.Location = new System.Drawing.Point(215, 20);
		this.label151.Name = "label151";
		this.label151.Size = new System.Drawing.Size(48, 15);
		this.label151.TabIndex = 84;
		this.label151.Text = "To Date";
		this.label152.AutoSize = true;
		this.label152.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label152.Location = new System.Drawing.Point(3, 20);
		this.label152.Name = "label152";
		this.label152.Size = new System.Drawing.Size(63, 15);
		this.label152.TabIndex = 83;
		this.label152.Text = "From Date";
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(634, 16);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(488, 16);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(263, 16);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(142, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(69, 16);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(142, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel2.Controls.Add(this.panel9);
		this.panel2.Controls.Add(this.panel7);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel2.Location = new System.Drawing.Point(3, 252);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1170, 277);
		this.panel2.TabIndex = 16;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryCustomerExpDetail);
		this.panel9.Controls.Add(this.panel10);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(688, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(482, 277);
		this.panel9.TabIndex = 1;
		this.grdHistoryCustomerExpDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdHistoryCustomerExpDetail).BackColor = System.Drawing.Color.White;
		this.grdHistoryCustomerExpDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdHistoryCustomerExpDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryCustomerExpDetail.DesignTimeLayout = grdHistoryCustomerExpDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryCustomerExpDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryCustomerExpDetail.FilterMode = (FilterMode)2;
		this.grdHistoryCustomerExpDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryCustomerExpDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryCustomerExpDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryCustomerExpDetail).Location = new System.Drawing.Point(0, 24);
		((System.Windows.Forms.Control)(object)this.grdHistoryCustomerExpDetail).Name = "grdHistoryCustomerExpDetail";
		((ScrollableBase)this.grdHistoryCustomerExpDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryCustomerExpDetail).Size = new System.Drawing.Size(482, 253);
		((System.Windows.Forms.Control)(object)this.grdHistoryCustomerExpDetail).TabIndex = 0;
		this.grdHistoryCustomerExpDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryCustomerExpDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryCustomerExpDetail).VisualStyle = (VisualStyle)4;
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label1);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(482, 24);
		this.panel10.TabIndex = 14;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(5, 5);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(361, 17);
		this.label1.TabIndex = 40;
		this.label1.Text = "Customer Expense (Supplier Wise) of Selected Row";
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistorySupplierExpDetail);
		this.panel7.Controls.Add(this.panel3);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(688, 277);
		this.panel7.TabIndex = 0;
		this.grdHistorySupplierExpDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdHistorySupplierExpDetail).BackColor = System.Drawing.Color.White;
		this.grdHistorySupplierExpDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdHistorySupplierExpDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistorySupplierExpDetail.DesignTimeLayout = grdHistorySupplierExpDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistorySupplierExpDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistorySupplierExpDetail.FilterMode = (FilterMode)2;
		this.grdHistorySupplierExpDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistorySupplierExpDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistorySupplierExpDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistorySupplierExpDetail).Location = new System.Drawing.Point(0, 24);
		((System.Windows.Forms.Control)(object)this.grdHistorySupplierExpDetail).Name = "grdHistorySupplierExpDetail";
		((ScrollableBase)this.grdHistorySupplierExpDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistorySupplierExpDetail).Size = new System.Drawing.Size(688, 253);
		((System.Windows.Forms.Control)(object)this.grdHistorySupplierExpDetail).TabIndex = 0;
		this.grdHistorySupplierExpDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistorySupplierExpDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistorySupplierExpDetail).VisualStyle = (VisualStyle)4;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label19);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(688, 24);
		this.panel3.TabIndex = 14;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label19.ForeColor = System.Drawing.Color.White;
		this.label19.Location = new System.Drawing.Point(5, 5);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(285, 17);
		this.label19.TabIndex = 40;
		this.label19.Text = "Supplier Expense of Above Selected row";
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbMultiLanguage);
		this.panel6.Controls.Add(this.labelmultilang);
		this.panel6.Controls.Add(this.toolStrip2);
		this.panel6.Controls.Add(this.label20);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1170, 48);
		this.panel6.TabIndex = 12;
		this.CmbMultiLanguage.AllowNull = (DefaultableBoolean)1;
		((System.Windows.Forms.Control)(object)this.CmbMultiLanguage).Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.CmbMultiLanguage.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbMultiLanguage.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbMultiLanguage.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Appearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance6).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance44).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbMultiLanguage).Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbMultiLanguage.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbMultiLanguage).Location = new System.Drawing.Point(1039, 1);
		((System.Windows.Forms.Control)(object)this.CmbMultiLanguage).Name = "CmbMultiLanguage";
		((System.Windows.Forms.Control)(object)this.CmbMultiLanguage).Size = new System.Drawing.Size(128, 23);
		((System.Windows.Forms.Control)(object)this.CmbMultiLanguage).TabIndex = 49;
		((System.Windows.Forms.Control)(object)this.CmbMultiLanguage).TabStop = false;
		((System.Windows.Forms.Control)(object)this.CmbMultiLanguage).Visible = false;
		this.labelmultilang.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.labelmultilang.AutoSize = true;
		this.labelmultilang.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.labelmultilang.ForeColor = System.Drawing.Color.White;
		this.labelmultilang.Location = new System.Drawing.Point(957, 2);
		this.labelmultilang.Name = "labelmultilang";
		this.labelmultilang.Size = new System.Drawing.Size(76, 20);
		this.labelmultilang.TabIndex = 50;
		this.labelmultilang.Text = "Language";
		this.labelmultilang.Visible = false;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnResetHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1170, 27);
		this.toolStrip2.TabIndex = 7;
		this.toolStrip2.Text = "toolStrip2";
		this.btnResetHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(59, 24);
		this.btnResetHistory.Text = "&New";
		this.btnResetHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Visible = false;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label20.ForeColor = System.Drawing.Color.White;
		this.label20.Location = new System.Drawing.Point(2, 28);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(286, 19);
		this.label20.TabIndex = 40;
		this.label20.Text = "Loading Delivery Expense History ";
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.Location = new System.Drawing.Point(611, 3);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(1273, 25);
		this.miniToolStrip.TabIndex = 27;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1184, 561);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "CommTradeLoadingDeliveryExpenses";
		this.Text = "CommTradeLoadingDeliveryExpenses";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(CommTradeLoadingDeliveryExpenses_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(CommTradeLoadingDeliveryExpenses_FormClosed);
		base.Load += new System.EventHandler(CommTradeLoadingDelivery_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(CommTradeLoadingDelivery_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.PanelExpMain.ResumeLayout(false);
		this.PanelCustomerExp.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdCustomerExp).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.PanelSupplierExp.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSupplierExp).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.tabControl2.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.GBaddAmount.ResumeLayout(false);
		this.GBaddAmount.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbExpense).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLoadingDelivery).EndInit();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryMain).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.groupBox7.ResumeLayout(false);
		this.groupBox7.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryCustomerExpDetail).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistorySupplierExpDetail).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMultiLanguage).EndInit();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
