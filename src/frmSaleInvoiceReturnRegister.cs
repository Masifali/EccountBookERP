// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
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
namespace Architecture.WinApp.Inventory_Reports
{

public class frmSaleInvoiceReturnRegister : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtInvRegister = new DataTable();

	private DataTable dtBranch = new DataTable();

	private int[] AryColWidth;

	public bool RequestFromAnotherForm;

	public DateTime FromDate;

	public DateTime ToDate;

	public int StockAccountId;

	public int ItemId;

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private Panel panel3;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	internal GridEX DataGridHistory;

	private Label label4;

	private Label label2;

	private Button btnshow;

	private Label label7;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel5;

	private Label label23;

	private ToolStripButton btnPrint;

	private GroupBox groupBox1;

	private Label label1;

	internal UltraCombo cmbDateType;

	private Panel panel6;

	private Label label3;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnShotCutKeys;

	private Label label5;

	internal UltraCombo CmbBranchName;

	public DateTimePicker txtTodate;

	public DateTimePicker txtFromDate;

	public UltraCombo cmbSupplier;

	public UltraCombo cmbItem;

	internal UltraCombo CmbStockAccount;

	private Label label38;

	public frmSaleInvoiceReturnRegister(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmPurchaseInvoiceRegister_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMargin = new Size(557, 643);
			ComboFill();
			BranchesFill();
			Datetypefill();
			if (!RequestFromAnotherForm)
			{
				txtFromDate.Value = DateTime.Now.AddDays(-15.0);
				((UltraGridBase)cmbDateType).Rows[2].Activate();
			}
			else
			{
				txtFromDate.Value = FromDate;
				txtTodate.Value = ToDate;
				if (StockAccountId > 0)
				{
					CmbStockAccount.Value = StockAccountId;
				}
				if (ItemId > 0)
				{
					cmbItem.Value = ItemId;
				}
				SaleInvoiceRegisterBind();
			}
			cmbDateType.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Datetypefill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			DDL.BindDDL(dt, cmbDateType, "Id", "Parameters", "Parameters", ZeroIndex: true);
			((UltraGridBase)cmbDateType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeIds = "98";
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)CmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)CmbBranchName).Text) + ",").Split(',');
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
			DataTable dt = InvPurchaseInvoice.AllComboBindAgainstPurchaseInvoice(obj);
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id");
			dtCustomer.Columns.Add("Name");
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id");
			dtItem.Columns.Add("Name");
			DataTable dtsa = new DataTable();
			dtsa.Columns.Add("Id");
			dtsa.Columns.Add("Name");
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Supplier")
				{
					dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "ItemName")
				{
					dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "StockAccount")
				{
					dtsa.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
			}
			if (dtCustomer.Rows.Count > 0)
			{
				DDL.BindDDL(dtCustomer, cmbSupplier, "Id", "Name", "Customer", ZeroIndex: true);
				((UltraGridBase)cmbSupplier).Rows[0].Activate();
			}
			else
			{
				((Control)(object)cmbSupplier).Text = string.Empty;
				((UltraGridBase)cmbSupplier).DataSource = null;
			}
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtItem, cmbItem, "Id", "Name", "Item Name", ZeroIndex: true);
				((UltraGridBase)cmbItem).Rows[0].Activate();
			}
			else
			{
				((Control)(object)cmbItem).Text = string.Empty;
				((UltraGridBase)cmbItem).DataSource = null;
			}
			if (dtsa.Rows.Count > 0)
			{
				DDL.BindDDL(dtsa, CmbStockAccount, "Id", "Name", "Stock Account", ZeroIndex: true);
				((UltraGridBase)CmbStockAccount).Rows[0].Activate();
			}
			else
			{
				((Control)(object)CmbStockAccount).Text = string.Empty;
				((UltraGridBase)CmbStockAccount).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchesFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			Branches obj = new Branches();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtBranch = Branches.GetAll(obj);
			if (dtBranch.Rows.Count > 0)
			{
				DDL.BindDDL(dtBranch, CmbBranchName, "Id", "BranchName", "Branch Name", ZeroIndex: false);
				UltraGridColumn obj2 = ((UltraGridBase)CmbBranchName).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj2).Key = "Selected";
				((HeaderBase)obj2.Header).Caption = string.Empty;
				obj2.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj2.DataType = typeof(bool);
				((HeaderBase)obj2.Header).VisiblePosition = 0;
				CmbBranchName.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)CmbBranchName.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)CmbBranchName.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)CmbBranchName.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)CmbBranchName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((Control)(object)CmbBranchName).Text = UserAccount.BranchName.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmSaleInvoiceReturnRegister_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnPrint_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbDateType.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				cmbDateType.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)DataGridHistory).Focus();
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
			dt.Rows.Add("Ctrl+S", "For Show");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl + P", "For print");
			dt.Rows.Add("Ctrl+F5", "For Focus On DateType ");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On DateType in Filter");
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

	private void btnShotCutKeys_Click(object sender, EventArgs e)
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

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			SaleInvoiceRegisterBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SaleInvoiceRegisterBind()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtInvRegister.Rows.Clear();
			ReportsParameters invoice = new ReportsParameters();
			invoice.OrganizationId = UserAccount.OrganizationId;
			invoice.CompanyId = UserAccount.CompanyId;
			invoice.DocumentTypeId = 98;
			invoice.SupplierCustomerId = Conversion.ToInt(cmbSupplier.Value);
			invoice.ItemId = Conversion.ToInt(cmbItem.Value);
			invoice.StockAccountId = Conversion.ToInt(CmbStockAccount.Value);
			invoice.FromDate = txtFromDate.Value;
			invoice.ToDate = txtTodate.Value;
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)CmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)CmbBranchName).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				invoice.BranchesIds = BranchIds;
				dtInvRegister = InvSaleInvoiceReports.InvSaleInvoiceReturnSlipandRegister(invoice);
				if (dtInvRegister.Rows.Count > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("Id", typeof(int));
					dt.Columns.Add("BranchSrNo", typeof(int));
					dt.Columns.Add("BranchName", typeof(string));
					dt.Columns.Add("DocNo", typeof(int));
					dt.Columns.Add("DocDate", typeof(DateTime));
					dt.Columns.Add("CustomerName", typeof(string));
					dt.Columns.Add("CommAgent", typeof(string));
					dt.Columns.Add("CommType", typeof(string));
					dt.Columns.Add("CommRate", typeof(double));
					dt.Columns.Add("CommAmount", typeof(double));
					dt.Columns.Add("ManualNo", typeof(string));
					dt.Columns.Add("BillAmount", typeof(double));
					dt.Columns.Add("ItemName", typeof(string));
					dt.Columns.Add("CropYear", typeof(string));
					dt.Columns.Add("JobLot", typeof(string));
					dt.Columns.Add("PackingType", typeof(string));
					dt.Columns.Add("PackUom", typeof(string));
					dt.Columns.Add("ItemQty", typeof(double));
					dt.Columns.Add("GrossWeight", typeof(double));
					dt.Columns.Add("WtCut", typeof(double));
					dt.Columns.Add("WtCutTotal", typeof(double));
					dt.Columns.Add("AdLsWt", typeof(double));
					dt.Columns.Add("NetWeight", typeof(double));
					dt.Columns.Add("StockWeight", typeof(double));
					dt.Columns.Add("ItemRate", typeof(double));
					dt.Columns.Add("RateUom", typeof(string));
					dt.Columns.Add("RateCut", typeof(double));
					dt.Columns.Add("RateCutAmount", typeof(double));
					dt.Columns.Add("ItemAmount", typeof(double));
					dt.Columns.Add("Warehouse", typeof(string));
					dt.Columns.Add("GpDate", typeof(DateTime));
					dt.Columns.Add("GpNo", typeof(int));
					dt.Columns.Add("VehicleNo", typeof(string));
					for (int i = 0; i < dtInvRegister.Rows.Count; i++)
					{
						dt.Rows.Add(dtInvRegister.Rows[i]["Id"], dtInvRegister.Rows[i]["BranchSrNo"], dtInvRegister.Rows[i]["BranchName"], dtInvRegister.Rows[i]["DocNo"], Conversion.ToDateTime(dtInvRegister.Rows[i]["DocDate"]).ToShortDateString(), dtInvRegister.Rows[i]["SupplierName"], dtInvRegister.Rows[i]["CommissionAgent"], dtInvRegister.Rows[i]["CommissionType"], dtInvRegister.Rows[i]["CommRate"], dtInvRegister.Rows[i]["CommAmount"], dtInvRegister.Rows[i]["ManualBillNo"], dtInvRegister.Rows[i]["BillAmount"], dtInvRegister.Rows[i]["ItemName"], dtInvRegister.Rows[i]["CropYear"], dtInvRegister.Rows[i]["JobLot"], dtInvRegister.Rows[i]["PackType"], dtInvRegister.Rows[i]["UOM"], dtInvRegister.Rows[i]["ItemQty"], dtInvRegister.Rows[i]["GrossWeight"], dtInvRegister.Rows[i]["WeightCut"], dtInvRegister.Rows[i]["WeightCutTotal"], dtInvRegister.Rows[i]["AdLsWeight"], dtInvRegister.Rows[i]["NetBillWeight"], dtInvRegister.Rows[i]["NetStockWeight"], dtInvRegister.Rows[i]["ItemRate"], dtInvRegister.Rows[i]["RateUOM"], dtInvRegister.Rows[i]["RateCut"], dtInvRegister.Rows[i]["RateCutAmount"], dtInvRegister.Rows[i]["Amount"], dtInvRegister.Rows[i]["WareHouseName"], Conversion.ToDateTime(dtInvRegister.Rows[i]["GpDate"]).ToShortDateString(), dtInvRegister.Rows[i]["GpNo"], dtInvRegister.Rows[i]["VehicleNo"]);
					}
					DataGridHistory.DataSource = dt;
					DataGridHistory.RetrieveStructure();
					DataGridHistorySetting();
				}
				else
				{
					DataGridHistory.DataSource = null;
				}
				return;
			}
			CmbBranchName.Focus();
			throw new Exception("Select Branch First");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistorySetting()
	{
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yy";
			DataGridHistory.RootTable.Columns["GpDate"].FormatString = "dd-MMM-yy";
			DataGridHistory.RootTable.Columns["BranchSrNo"].Width = 70;
			DataGridHistory.RootTable.Columns["BranchName"].Width = 150;
			DataGridHistory.RootTable.Columns["DocNo"].Width = 40;
			DataGridHistory.RootTable.Columns["DocDate"].Width = 70;
			DataGridHistory.RootTable.Columns["CustomerName"].Width = 150;
			DataGridHistory.RootTable.Columns["CommAgent"].Width = 70;
			DataGridHistory.RootTable.Columns["CommType"].Width = 60;
			DataGridHistory.RootTable.Columns["CommRate"].Width = 60;
			DataGridHistory.RootTable.Columns["CommAmount"].Width = 70;
			DataGridHistory.RootTable.Columns["ManualNo"].Width = 70;
			DataGridHistory.RootTable.Columns["BillAmount"].Width = 80;
			DataGridHistory.RootTable.Columns["ItemName"].Width = 150;
			DataGridHistory.RootTable.Columns["CropYear"].Width = 70;
			DataGridHistory.RootTable.Columns["JobLot"].Width = 70;
			DataGridHistory.RootTable.Columns["PackingType"].Width = 80;
			DataGridHistory.RootTable.Columns["PackUom"].Width = 70;
			DataGridHistory.RootTable.Columns["ItemQty"].Width = 70;
			DataGridHistory.RootTable.Columns["GrossWeight"].Width = 80;
			DataGridHistory.RootTable.Columns["WtCut"].Width = 70;
			DataGridHistory.RootTable.Columns["WtCutTotal"].Width = 100;
			DataGridHistory.RootTable.Columns["AdLsWt"].Width = 80;
			DataGridHistory.RootTable.Columns["NetWeight"].Width = 100;
			DataGridHistory.RootTable.Columns["StockWeight"].Width = 100;
			DataGridHistory.RootTable.Columns["ItemRate"].Width = 100;
			DataGridHistory.RootTable.Columns["RateUom"].Width = 80;
			DataGridHistory.RootTable.Columns["RateCut"].Width = 80;
			DataGridHistory.RootTable.Columns["RateCutAmount"].Width = 100;
			DataGridHistory.RootTable.Columns["ItemAmount"].Width = 120;
			DataGridHistory.RootTable.Columns["Warehouse"].Width = 120;
			DataGridHistory.RootTable.Columns["GpDate"].Width = 70;
			DataGridHistory.RootTable.Columns["GpNo"].Width = 40;
			DataGridHistory.RootTable.Columns["VehicleNo"].Width = 100;
			DataGridHistory.RootTable.Columns["CommRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["CommRate"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["CommAmount"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["CommAmount"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["GrossWeight"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["WtCut"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["WtCut"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["WtCut"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["WtCut"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["WtCutTotal"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["WtCutTotal"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["WtCutTotal"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["WtCutTotal"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["AdLsWt"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["AdLsWt"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["AdLsWt"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["AdLsWt"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["NetWeight"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["StockWeight"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistory.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["RateCut"].FormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistory.RootTable.Columns["RateCutAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["RateCutAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["RateCutAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["RateCutAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			ctrlGrdBar1_Load(null, null);
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

	private void btnPrint_Click(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtInvRegister != null && dtInvRegister.Rows.Count > 0)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.ShowReportWithDataTable(dtInvRegister, "0228-InvPurchaseInvoice_SaleReturnRegister.rpt");
				return;
			}
			throw new Exception("Record Not Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmSaleInvoiceReturnRegister_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			ComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
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

	private void Reset()
	{
		try
		{
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)cmbSupplier).Text = string.Empty;
			cmbDateType.Focus();
			Datetypefill();
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
			if (base.ActiveControl.Name == "DataGridHistory")
			{
				cmbDateType.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbDateType_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbDateType.Value) == 1)
			{
				txtFromDate.Value = DateTime.Today;
				btnshow_Click(null, null);
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 2)
			{
				txtFromDate.Value = DateTime.Today.AddDays(-7.0);
				btnshow_Click(null, null);
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				txtFromDate.Value = first;
				txtTodate.Value = DateTime.Today;
				btnshow_Click(null, null);
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				txtFromDate.Value = first2;
				txtTodate.Value = DateTime.Today;
				btnshow_Click(null, null);
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 5)
			{
				txtFromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
				btnshow_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbBranchName_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)CmbBranchName).Text != string.Empty && ((UltraGridBase)CmbBranchName).ActiveRow != null)
			{
				ComboFill();
				return;
			}
			((Control)(object)cmbSupplier).Text = string.Empty;
			((UltraGridBase)cmbSupplier).DataSource = null;
			((Control)(object)cmbItem).Text = string.Empty;
			((UltraGridBase)cmbItem).DataSource = null;
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
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Expected O, but got Unknown
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Inventory_Reports.frmSaleInvoiceReturnRegister));
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label3 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label5 = new System.Windows.Forms.Label();
		this.CmbBranchName = new UltraCombo();
		this.cmbItem = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.cmbDateType = new UltraCombo();
		this.cmbSupplier = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.txtTodate = new System.Windows.Forms.DateTimePicker();
		this.txtFromDate = new System.Windows.Forms.DateTimePicker();
		this.label4 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnShotCutKeys = new System.Windows.Forms.ToolStripButton();
		this.CmbStockAccount = new UltraCombo();
		this.label38 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel6.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranchName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplier).BeginInit();
		this.panel5.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbStockAccount).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(919, 604);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 165);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(919, 439);
		this.panel4.TabIndex = 2;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistory_DesignTimeLayout.LayoutString = resources.GetString("DataGridHistory_DesignTimeLayout.LayoutString");
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 30);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(919, 409);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Controls.Add(this.label3);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(919, 30);
		this.panel6.TabIndex = 205;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(874, 1);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(39, 26);
		this.ctrlGrdBar1.TabIndex = 12;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(3, 4);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(67, 19);
		this.label3.TabIndex = 1;
		this.label3.Text = "History";
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 29);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(919, 136);
		this.panel3.TabIndex = 0;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStockAccount);
		this.groupBox1.Controls.Add(this.label38);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranchName);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateType);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplier);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.txtTodate);
		this.groupBox1.Controls.Add(this.txtFromDate);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(6, 32);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(668, 101);
		this.groupBox1.TabIndex = 207;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(2, 20);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(79, 15);
		this.label5.TabIndex = 208;
		this.label5.Text = "Branch Name";
		this.CmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.CmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance45;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance50).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance56).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Location = new System.Drawing.Point(96, 14);
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Name = "CmbBranchName";
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Size = new System.Drawing.Size(224, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranchName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Leave += new System.EventHandler(CmbBranchName_Leave);
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance34;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(395, 42);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(265, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 5;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(2, 48);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(60, 15);
		this.label1.TabIndex = 206;
		this.label1.Text = "Date Type";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(325, 48);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(67, 15);
		this.label7.TabIndex = 198;
		this.label7.Text = "Item Name";
		this.cmbDateType.AllowNull = (DefaultableBoolean)1;
		this.cmbDateType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbDateType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance10).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance10).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance11;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbDateType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance17).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateType).Location = new System.Drawing.Point(96, 42);
		((System.Windows.Forms.Control)(object)this.cmbDateType).Name = "cmbDateType";
		((System.Windows.Forms.Control)(object)this.cmbDateType).Size = new System.Drawing.Size(224, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateType).TabIndex = 1;
		this.cmbDateType.ValueChanged += new System.EventHandler(cmbDateType_ValueChanged);
		this.cmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance38;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance6).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Location = new System.Drawing.Point(395, 14);
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Name = "cmbSupplier";
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Size = new System.Drawing.Size(265, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplier).TabIndex = 4;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.Location = new System.Drawing.Point(597, 70);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(63, 26);
		this.btnshow.TabIndex = 7;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.btnshow.Leave += new System.EventHandler(btnshow_Leave);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(325, 20);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(58, 15);
		this.label2.TabIndex = 199;
		this.label2.Text = "Customer";
		this.txtTodate.CustomFormat = "dd-MMM-yy";
		this.txtTodate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTodate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtTodate.Location = new System.Drawing.Point(209, 72);
		this.txtTodate.Name = "txtTodate";
		this.txtTodate.Size = new System.Drawing.Size(111, 23);
		this.txtTodate.TabIndex = 3;
		this.txtFromDate.CustomFormat = "dd-MMM-yy";
		this.txtFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromDate.Location = new System.Drawing.Point(96, 72);
		this.txtFromDate.Name = "txtFromDate";
		this.txtFromDate.Size = new System.Drawing.Size(110, 23);
		this.txtFromDate.TabIndex = 2;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(2, 76);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(91, 15);
		this.label4.TabIndex = 201;
		this.label4.Text = "From Date && To";
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label23);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(919, 30);
		this.panel5.TabIndex = 204;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(253, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Sale Invoice Return Register  ";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(919, 29);
		this.panel2.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnRefresh, this.btnPrint, this.btnShotCutKeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(919, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(68, 24);
		this.btnnew.Text = "&Reset";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(100, 24);
		this.btnPrint.Text = "Print-0228";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnShotCutKeys.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShotCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShotCutKeys.Image = (System.Drawing.Image)resources.GetObject("btnShotCutKeys.Image");
		this.btnShotCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShotCutKeys.Name = "btnShotCutKeys";
		this.btnShotCutKeys.Size = new System.Drawing.Size(106, 24);
		this.btnShotCutKeys.Text = "ShortCut Keys";
		this.btnShotCutKeys.Click += new System.EventHandler(btnShotCutKeys_Click);
		this.CmbStockAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbStockAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStockAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStockAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStockAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance22;
		((SpecialBoxBase)((UltraGridBase)this.CmbStockAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance28).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance29).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStockAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbStockAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStockAccount).Location = new System.Drawing.Point(395, 70);
		((System.Windows.Forms.Control)(object)this.CmbStockAccount).Name = "CmbStockAccount";
		((System.Windows.Forms.Control)(object)this.CmbStockAccount).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.CmbStockAccount).TabIndex = 6;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(325, 76);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(59, 15);
		this.label38.TabIndex = 611;
		this.label38.Text = "Stock A/c";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(919, 604);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmSaleInvoiceReturnRegister";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmSaleInvoiceReturnRegister_FormClosing);
		base.Load += new System.EventHandler(frmPurchaseInvoiceRegister_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmSaleInvoiceReturnRegister_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranchName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplier).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbStockAccount).EndInit();
		base.ResumeLayout(false);
	}
}

}
