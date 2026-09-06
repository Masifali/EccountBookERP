using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Mfg.Reports;

public class frmItemEvaluationLedger_Engr : Form
{
	private DropDownBind DDL = new DropDownBind();

	private DataTable dtBindCombo = new DataTable();

	public bool RequestedByOtherDocument;

	private DataTable lst = new DataTable();

	private DataTable dtcol = new DataTable();

	private bool FormInitialized;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel3;

	private Label label23;

	private Panel panel4;

	private Label label27;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton print;

	private ToolStripButton btnNew;

	private Panel panel6;

	private Panel panel5;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label2;

	private Button btnshow;

	private Panel panel7;

	private Label label10;

	private GroupBox groupBox1;

	private Label label11;

	internal UltraCombo cmbDateType;

	private ToolStripButton btnshortcut;

	private ToolStripButton btnRefresh;

	public DateTimePicker todate;

	public DateTimePicker fromdate;

	public UltraCombo CmbItem;

	private GroupBox GroupOpeningInfo;

	private Label label5;

	private Label label4;

	private Label label3;

	private Label label7;

	private Label label6;

	private Label label1;

	private TextBox txtAvgRateClosing;

	private TextBox txtAvgRateOpening;

	private TextBox txtAmountclosing;

	private TextBox txtAmountOpening;

	private TextBox txtWeightClosing;

	private TextBox txtWeightOpening;

	private TextBox txtQtyClosing;

	private TextBox txtQtyOpening;

	public frmItemEvaluationLedger_Engr(UserAccount user)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void PurchaseOrderHistory_Load(object sender, EventArgs e)
	{
		try
		{
			FormInitialized = true;
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(970, 500);
			ctrlGrdBar1.mGridPrint.Enabled = true;
			ctrlGrdBar1.mGridExport.Enabled = true;
			ctrlGrdBar1.mGridSaveLayouts.Visible = false;
			ctrlGrdBar1.mGridChooseFielder.Visible = false;
			ctrlGrdBar1.removeLayoutToolStripMenuItem.Visible = false;
			ctrlGrdBar1.GroupExpand.Visible = false;
			ctrlGrdBar1.GroupCollaps.Visible = false;
			StockComboFill();
			Datetypefill();
			cmbDateType.Focus();
			if (!RequestedByOtherDocument)
			{
				((UltraGridBase)cmbDateType).Rows[2].Activate();
			}
			else
			{
				btnshow_Click(null, null);
			}
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

	private void StockComboFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.Activity = "Items";
			dtBindCombo = StocksReport.Inventory_StockEvalautionDetail_DropDownAndLists(obj);
			if (dtBindCombo.Rows.Count > 0)
			{
				DataTable dtItem = new DataTable();
				dtItem.Columns.Add("Id");
				dtItem.Columns.Add("name");
				for (int i = 0; i < dtBindCombo.Rows.Count; i++)
				{
					dtItem.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
				}
				if (dtItem.Rows.Count > 0)
				{
					DDL.BindDDL(dtItem, CmbItem, "Id", "name", "Item Name", ZeroIndex: false);
					((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[1].Width = 325;
				}
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
				fromdate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 2)
			{
				fromdate.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				fromdate.Value = first;
				todate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				fromdate.Value = first2;
				todate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 5)
			{
				fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
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
			((Control)(object)CmbItem).Text = string.Empty;
			GroupOpeningInfo.Visible = false;
			cmbDateType.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
	}

	private void frmItemEvaluationLedger_Engr_FormClosing(object sender, FormClosingEventArgs e)
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
			StockComboFill();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
			if (fs != null)
			{
				grd.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grd, dtcol);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		try
		{
			GroupOpeningInfo.Visible = false;
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = fromdate.Value;
			obj.ToDate = todate.Value;
			obj.ItemId = Conversion.ToInt(CmbItem.Value);
			if (obj.ItemId == 0)
			{
				CmbItem.Focus();
				throw new Exception("Item Field Required...");
			}
			lst = InventoryStockEvalautionDetail.ItemLedgerFromStockEvaluations_Engr(obj);
			if (lst.Rows.Count > 0)
			{
				GroupOpeningInfo.Visible = true;
				if (dtcol.Columns.Count == 0)
				{
					dtcol.Columns.Add("TranDate", typeof(DateTime));
					dtcol.Columns.Add("DocNo", typeof(int));
					dtcol.Columns.Add("RefDocumentTypeId", typeof(int));
					dtcol.Columns.Add("DocType", typeof(string));
					dtcol.Columns.Add("QtyIn", typeof(decimal));
					dtcol.Columns.Add("WeightIn", typeof(decimal));
					dtcol.Columns.Add("RateIn", typeof(decimal));
					dtcol.Columns.Add("RateUomIn", typeof(string));
					dtcol.Columns.Add("AmountIn", typeof(decimal));
					dtcol.Columns.Add("QtyOut", typeof(decimal));
					dtcol.Columns.Add("WeightOut", typeof(decimal));
					dtcol.Columns.Add("RateOut", typeof(decimal));
					dtcol.Columns.Add("RateUomOut", typeof(string));
					dtcol.Columns.Add("AmountOut", typeof(decimal));
					dtcol.Columns.Add("BalQty", typeof(decimal));
					dtcol.Columns.Add("BalWeight", typeof(decimal));
					dtcol.Columns.Add("BalAmount", typeof(decimal));
					dtcol.Columns.Add("AvgRate", typeof(decimal));
					dtcol.Columns.Add("SupplierCustomerId", typeof(int));
					dtcol.Columns.Add("PartyName", typeof(string));
					dtcol.Columns.Add("WarehouseId", typeof(int));
					dtcol.Columns.Add("WareHouseName", typeof(string));
					dtcol.Columns.Add("ItemId", typeof(int));
					dtcol.Columns.Add("ItemName", typeof(string));
					dtcol.Columns.Add("PackUom", typeof(string));
					dtcol.Columns.Add("VariantDescription", typeof(string));
					dtcol.Columns.Add("JobLot", typeof(string));
					dtcol.Columns.Add("CastingType", typeof(string));
					dtcol.Columns.Add("ProductionStage", typeof(string));
				}
				dtcol.Rows.Clear();
				GroupOpeningInfo.Text = "Opening And Closing Info Of Item : (" + Conversion.ToString((object)((Control)(object)CmbItem).Text) + ")";
				DataRow dataRow = lst.Rows[0];
				DataRow lastRow = lst.Rows[lst.Rows.Count - 1];
				double OpeningQty = Conversion.ToDouble(dataRow["BalQty"]);
				double OpeningWeight = Conversion.ToDouble(dataRow["BalWeight"]);
				double OpeningAmount = Conversion.ToDouble(dataRow["BalAmount"]);
				double OpeningAvgRate = Conversion.ToDouble(dataRow["AvgRate"]);
				double ClosingQty = Conversion.ToDouble(lastRow["BalQty"]);
				double ClosingWeight = Conversion.ToDouble(lastRow["BalWeight"]);
				double ClosingAmount = Conversion.ToDouble(lastRow["BalAmount"]);
				double ClosingAvgRate = Conversion.ToDouble(lastRow["AvgRate"]);
				foreach (DataRow r in lst.Rows)
				{
					dtcol.Rows.Add(r["TrancDate"], r["DocNo"], r["RefDocumentTypeId"], r["DocType"], r["QtyIn"], r["WeightIn"], r["RateIn"], r["RateUom"], r["AmountIn"], r["QtyOut"], r["WeightOut"], r["RateOut"], r["RateUom"], r["AmountOut"], r["BalQty"], r["BalWeight"], r["BalAmount"], r["AvgRate"], r["SupplierCustomerId"], r["PartyName"], r["WarehouseId"], r["WareHouseName"], r["ItemId"], r["ItemName"], r["PackUom"], r["VarientDescription"], r["JobLot"], r["CastingType"], r["ProductionProcessStage"]);
				}
				grd.DataSource = dtcol;
				GridSettings();
				txtQtyOpening.Text = OpeningQty.ToString("#,#;(#,#);0");
				txtWeightOpening.Text = OpeningWeight.ToString("#,#;(#,#);0");
				txtAmountOpening.Text = OpeningAmount.ToString("#,#;(#,#);0");
				txtAvgRateOpening.Text = OpeningAvgRate.ToString("#,##.##");
				txtQtyClosing.Text = ClosingQty.ToString("#,#;(#,#);0");
				txtWeightClosing.Text = ClosingWeight.ToString("#,#;(#,#);0");
				txtAmountclosing.Text = ClosingAmount.ToString("#,#;(#,#);0");
				txtAvgRateClosing.Text = ClosingAvgRate.ToString("#,##.##");
			}
			else
			{
				grd.DataSource = null;
				txtQtyOpening.Text = 0.ToString("#,#;(#,#);0");
				txtWeightOpening.Text = 0.ToString("#,#;(#,#);0");
				txtAmountOpening.Text = 0.ToString("#,#;(#,#);0");
				txtAvgRateOpening.Text = 0.ToString("#,##.##");
				txtQtyClosing.Text = 0.ToString("#,#;(#,#);0");
				txtWeightClosing.Text = 0.ToString("#,#;(#,#);0");
				txtAmountclosing.Text = 0.ToString("#,#;(#,#);0");
				txtAvgRateClosing.Text = 0.ToString("#,##.##");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettings()
	{
		try
		{
			((Control)(object)grd).Font = new Font("Verdana", 8.75f);
			grd.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grd.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grd.RootTable.Columns["WarehouseId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["ItemName"].Visible = false;
			grd.RootTable.Columns["TranDate"].FormatString = "dd-MMM-yyyy";
			grd.RootTable.Columns["TranDate"].Width = 100;
			grd.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["DocType"].Width = Constants.InventoryConstants.DocumentTypeCode;
			grd.RootTable.ColumnSets["Transaction"].Width = 100 + Constants.InventoryConstants.DoNoConstant + Constants.InventoryConstants.DocumentTypeCode;
			grd.RootTable.Columns["QtyIn"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["WeightIn"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["AmountIn"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grd.RootTable.Columns["RateIn"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["RateUomIn"].Width = 65;
			grd.RootTable.ColumnSets["Receipt"].Width = Constants.InventoryConstants.QtyConstant + Constants.InventoryConstants.WeightConstant + Constants.InventoryConstants.ExRateConstant + 65 + Constants.InventoryConstants.AmountForReportConstant;
			grd.RootTable.Columns["QtyOut"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["WeightOut"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["RateOut"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["RateUomOut"].Width = 65;
			grd.RootTable.Columns["AmountOut"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grd.RootTable.ColumnSets["Consumption"].Width = Constants.InventoryConstants.QtyConstant + Constants.InventoryConstants.WeightConstant + Constants.InventoryConstants.ExRateConstant + 65 + Constants.InventoryConstants.AmountForReportConstant;
			grd.RootTable.Columns["BalWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["BalAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grd.RootTable.Columns["BalQty"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["AvgRate"].Width = 90;
			grd.RootTable.ColumnSets["Balance"].Width = Constants.InventoryConstants.WeightConstant + Constants.InventoryConstants.AmountForReportConstant + Constants.InventoryConstants.QtyConstant + 90;
			grd.RootTable.Columns["PartyName"].Width = Constants.InventoryConstants.SupplierName;
			grd.RootTable.Columns["WareHouseName"].Width = Constants.InventoryConstants.WareHouseName;
			grd.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grd.RootTable.Columns["PackUom"].Width = 65;
			grd.RootTable.Columns["VariantDescription"].Width = Constants.InventoryConstants.VariantEquivalent;
			grd.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			grd.RootTable.Columns["CastingType"].Width = Constants.InventoryConstants.CastingType;
			grd.RootTable.Columns["ProductionStage"].Width = Constants.InventoryConstants.ProductionStage;
			grd.RootTable.ColumnSets["Description"].Width = Constants.InventoryConstants.SupplierName + Constants.InventoryConstants.WareHouseName + Constants.InventoryConstants.ItemName + 65 + Constants.InventoryConstants.VariantEquivalent + Constants.InventoryConstants.ProductionStage + Constants.InventoryConstants.CastingType + Constants.InventoryConstants.JobLotConstant;
			grd.RootTable.Columns["QtyIn"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["QtyIn"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["QtyIn"].FormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["QtyIn"].TotalFormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["QtyOut"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["QtyOut"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["QtyOut"].FormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["QtyOut"].TotalFormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["WeightIn"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["WeightIn"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["WeightIn"].FormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["WeightIn"].TotalFormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["WeightOut"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["WeightOut"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["WeightOut"].FormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["WeightOut"].TotalFormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["AmountIn"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AmountIn"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AmountIn"].FormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["AmountIn"].TotalFormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["AmountOut"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AmountOut"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AmountOut"].FormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["AmountOut"].TotalFormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BalQty"].FormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["BalQty"].TotalFormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["BalWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BalWeight"].FormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["BalWeight"].TotalFormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["RateIn"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RateIn"].FormatString = "#,##.##";
			grd.RootTable.Columns["RateIn"].TotalFormatString = "#,##.##";
			grd.RootTable.Columns["RateOut"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RateOut"].FormatString = "#,##.##";
			grd.RootTable.Columns["RateOut"].TotalFormatString = "#,##.##";
			grd.RootTable.Columns["AvgRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AvgRate"].FormatString = "#,##.##";
			grd.RootTable.Columns["AvgRate"].TotalFormatString = "#,##.##";
			grd.RootTable.Columns["BalAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BalAmount"].FormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["BalAmount"].TotalFormatString = "#,#;(#,#);0";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				_ = r.Cells.Count;
				_ = 0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PurchaseOrderHistory_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.Alt && e.KeyCode == Keys.P)
			{
				print_Click_1(null, null);
			}
			if ((e.Alt && e.KeyCode == Keys.D2) || (e.Alt && e.KeyCode == Keys.NumPad2))
			{
				print_Click_1(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
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
				cmbDateType.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				cmbDateType.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
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
			dt.Rows.Add("Ctrl+P", "For print 421");
			dt.Rows.Add("Alt+1", "For print 421");
			dt.Rows.Add("Ctrl+F5", "For Focus On DateType ");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On DateType in Filter");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshortcut_Click(object sender, EventArgs e)
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

	private void print_Click_1(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (lst.Rows.Count > 0 && lst != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(lst, "1672_01_ItemEvaluationLedger.rpt");
			}
			else
			{
				MessageBox.Show("Record Not Found For Display");
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
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Expected O, but got Unknown
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Expected O, but got Unknown
		//IL_06b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c0: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Mfg.Reports.frmItemEvaluationLedger_Engr));
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label10 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.GroupOpeningInfo = new System.Windows.Forms.GroupBox();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtAvgRateClosing = new System.Windows.Forms.TextBox();
		this.txtAvgRateOpening = new System.Windows.Forms.TextBox();
		this.txtAmountclosing = new System.Windows.Forms.TextBox();
		this.txtAmountOpening = new System.Windows.Forms.TextBox();
		this.txtWeightClosing = new System.Windows.Forms.TextBox();
		this.txtWeightOpening = new System.Windows.Forms.TextBox();
		this.txtQtyClosing = new System.Windows.Forms.TextBox();
		this.txtQtyOpening = new System.Windows.Forms.TextBox();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label11 = new System.Windows.Forms.Label();
		this.cmbDateType = new UltraCombo();
		this.label27 = new System.Windows.Forms.Label();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.todate = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbItem = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.print = new System.Windows.Forms.ToolStripButton();
		this.btnshortcut = new System.Windows.Forms.ToolStripButton();
		this.panel3.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel7.SuspendLayout();
		this.panel5.SuspendLayout();
		this.GroupOpeningInfo.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDateType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).BeginInit();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 31);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1034, 25);
		this.panel3.TabIndex = 4;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(6, 3);
		this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(159, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Item Stock Ledger";
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 56);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1034, 405);
		this.panel4.TabIndex = 5;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 104);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1034, 301);
		this.panel6.TabIndex = 110;
		this.grd.AllowEdit = (InheritableBoolean)2;
		this.grd.AlternatingColors = true;
		this.grd.AutomaticSort = false;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = resources.GetString("grd_DesignTimeLayout.LayoutString");
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.grd.GroupByBoxVisible = false;
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 24);
		((System.Windows.Forms.Control)(object)this.grd).Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).Office2007CustomColor = System.Drawing.Color.White;
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1034, 277);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grd).TabStop = false;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar1);
		this.panel7.Controls.Add(this.label10);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1034, 24);
		this.panel7.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(988, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(42, 22);
		this.ctrlGrdBar1.TabIndex = 1;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.White;
		this.label10.Location = new System.Drawing.Point(2, 3);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(142, 19);
		this.label10.TabIndex = 0;
		this.label10.Text = "Filtered Records";
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.GroupOpeningInfo);
		this.panel5.Controls.Add(this.groupBox1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1034, 104);
		this.panel5.TabIndex = 0;
		this.GroupOpeningInfo.Controls.Add(this.label5);
		this.GroupOpeningInfo.Controls.Add(this.label4);
		this.GroupOpeningInfo.Controls.Add(this.label3);
		this.GroupOpeningInfo.Controls.Add(this.label7);
		this.GroupOpeningInfo.Controls.Add(this.label6);
		this.GroupOpeningInfo.Controls.Add(this.label1);
		this.GroupOpeningInfo.Controls.Add(this.txtAvgRateClosing);
		this.GroupOpeningInfo.Controls.Add(this.txtAvgRateOpening);
		this.GroupOpeningInfo.Controls.Add(this.txtAmountclosing);
		this.GroupOpeningInfo.Controls.Add(this.txtAmountOpening);
		this.GroupOpeningInfo.Controls.Add(this.txtWeightClosing);
		this.GroupOpeningInfo.Controls.Add(this.txtWeightOpening);
		this.GroupOpeningInfo.Controls.Add(this.txtQtyClosing);
		this.GroupOpeningInfo.Controls.Add(this.txtQtyOpening);
		this.GroupOpeningInfo.Location = new System.Drawing.Point(356, 2);
		this.GroupOpeningInfo.Name = "GroupOpeningInfo";
		this.GroupOpeningInfo.Size = new System.Drawing.Size(602, 100);
		this.GroupOpeningInfo.TabIndex = 1;
		this.GroupOpeningInfo.TabStop = false;
		this.GroupOpeningInfo.Text = "Opening and Closing Of Item '[ItemName]'";
		this.GroupOpeningInfo.Visible = false;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(505, 18);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(63, 17);
		this.label5.TabIndex = 594;
		this.label5.Text = "Avg Rate";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(367, 18);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(58, 17);
		this.label4.TabIndex = 594;
		this.label4.Text = "Amount";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(220, 18);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(76, 17);
		this.label3.TabIndex = 594;
		this.label3.Text = "Weight(Kg)";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(5, 71);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(56, 19);
		this.label7.TabIndex = 594;
		this.label7.Text = "Closing";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(5, 42);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(63, 19);
		this.label6.TabIndex = 594;
		this.label6.Text = "Opening";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(115, 18);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(37, 17);
		this.label1.TabIndex = 594;
		this.label1.Text = "Bags";
		this.txtAvgRateClosing.Enabled = false;
		this.txtAvgRateClosing.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAvgRateClosing.Location = new System.Drawing.Point(475, 67);
		this.txtAvgRateClosing.Name = "txtAvgRateClosing";
		this.txtAvgRateClosing.Size = new System.Drawing.Size(123, 26);
		this.txtAvgRateClosing.TabIndex = 0;
		this.txtAvgRateClosing.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAvgRateOpening.Enabled = false;
		this.txtAvgRateOpening.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAvgRateOpening.Location = new System.Drawing.Point(475, 38);
		this.txtAvgRateOpening.Name = "txtAvgRateOpening";
		this.txtAvgRateOpening.Size = new System.Drawing.Size(123, 26);
		this.txtAvgRateOpening.TabIndex = 0;
		this.txtAvgRateOpening.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAmountclosing.Enabled = false;
		this.txtAmountclosing.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmountclosing.Location = new System.Drawing.Point(335, 67);
		this.txtAmountclosing.Name = "txtAmountclosing";
		this.txtAmountclosing.Size = new System.Drawing.Size(136, 26);
		this.txtAmountclosing.TabIndex = 0;
		this.txtAmountclosing.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAmountOpening.Enabled = false;
		this.txtAmountOpening.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmountOpening.Location = new System.Drawing.Point(335, 38);
		this.txtAmountOpening.Name = "txtAmountOpening";
		this.txtAmountOpening.Size = new System.Drawing.Size(136, 26);
		this.txtAmountOpening.TabIndex = 0;
		this.txtAmountOpening.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtWeightClosing.Enabled = false;
		this.txtWeightClosing.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWeightClosing.Location = new System.Drawing.Point(197, 67);
		this.txtWeightClosing.Name = "txtWeightClosing";
		this.txtWeightClosing.Size = new System.Drawing.Size(136, 26);
		this.txtWeightClosing.TabIndex = 0;
		this.txtWeightClosing.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtWeightOpening.Enabled = false;
		this.txtWeightOpening.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWeightOpening.Location = new System.Drawing.Point(197, 38);
		this.txtWeightOpening.Name = "txtWeightOpening";
		this.txtWeightOpening.Size = new System.Drawing.Size(136, 26);
		this.txtWeightOpening.TabIndex = 0;
		this.txtWeightOpening.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQtyClosing.Enabled = false;
		this.txtQtyClosing.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyClosing.Location = new System.Drawing.Point(72, 67);
		this.txtQtyClosing.Name = "txtQtyClosing";
		this.txtQtyClosing.Size = new System.Drawing.Size(123, 26);
		this.txtQtyClosing.TabIndex = 0;
		this.txtQtyClosing.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQtyOpening.Enabled = false;
		this.txtQtyOpening.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyOpening.Location = new System.Drawing.Point(72, 38);
		this.txtQtyOpening.Name = "txtQtyOpening";
		this.txtQtyOpening.Size = new System.Drawing.Size(123, 26);
		this.txtQtyOpening.TabIndex = 0;
		this.txtQtyOpening.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateType);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.fromdate);
		this.groupBox1.Controls.Add(this.todate);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItem);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(351, 100);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(3, 20);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(60, 15);
		this.label11.TabIndex = 594;
		this.label11.Text = "Date Type";
		this.cmbDateType.AllowNull = (DefaultableBoolean)1;
		this.cmbDateType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbDateType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbDateType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateType).Location = new System.Drawing.Point(97, 14);
		((System.Windows.Forms.Control)(object)this.cmbDateType).Name = "cmbDateType";
		((System.Windows.Forms.Control)(object)this.cmbDateType).Size = new System.Drawing.Size(247, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateType).TabIndex = 0;
		this.cmbDateType.ValueChanged += new System.EventHandler(cmbDateType_ValueChanged);
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(3, 48);
		this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(91, 15);
		this.label27.TabIndex = 103;
		this.label27.Text = "From && To Date";
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yy";
		this.fromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(97, 44);
		this.fromdate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.fromdate.Name = "fromdate";
		this.fromdate.Size = new System.Drawing.Size(123, 23);
		this.fromdate.TabIndex = 1;
		this.todate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.CustomFormat = "dd-MMM-yy";
		this.todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.todate.Location = new System.Drawing.Point(221, 44);
		this.todate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.todate.Name = "todate";
		this.todate.Size = new System.Drawing.Size(123, 23);
		this.todate.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(3, 76);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 15);
		this.label2.TabIndex = 106;
		this.label2.Text = "Item Name";
		this.CmbItem.AllowNull = (DefaultableBoolean)1;
		this.CmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance12;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance17).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance20).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItem).Location = new System.Drawing.Point(97, 70);
		((System.Windows.Forms.Control)(object)this.CmbItem).Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		((System.Windows.Forms.Control)(object)this.CmbItem).Name = "CmbItem";
		((System.Windows.Forms.Control)(object)this.CmbItem).Size = new System.Drawing.Size(188, 26);
		((System.Windows.Forms.Control)(object)this.CmbItem).TabIndex = 3;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(288, 70);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(56, 26);
		this.btnshow.TabIndex = 4;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1034, 461);
		this.panel1.TabIndex = 0;
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1034, 31);
		this.panel2.TabIndex = 3;
		this.toolStrip3.BackColor = System.Drawing.Color.White;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.btnRefresh, this.print, this.btnshortcut });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1034, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print.Name = "print";
		this.print.Size = new System.Drawing.Size(92, 24);
		this.print.Text = "421-Print";
		this.print.Click += new System.EventHandler(print_Click_1);
		this.btnshortcut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcut.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcut.Name = "btnshortcut";
		this.btnshortcut.Size = new System.Drawing.Size(126, 24);
		this.btnshortcut.Text = "ShortCut Keys";
		this.btnshortcut.Click += new System.EventHandler(btnshortcut_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 15f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1034, 461);
		base.Controls.Add(this.panel1);
		this.DoubleBuffered = true;
		this.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "frmItemEvaluationLedger_Engr";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmItemEvaluationLedger_Engr_FormClosing);
		base.Load += new System.EventHandler(PurchaseOrderHistory_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PurchaseOrderHistory_KeyDown);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.GroupOpeningInfo.ResumeLayout(false);
		this.GroupOpeningInfo.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDateType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
