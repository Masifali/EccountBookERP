using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Inventory_Definition.Stock_Closing;

public class MonthlyStockClosing : Form
{
	private WaitForm wf = new WaitForm();

	private Rightsobjects formRights = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private int RecId;

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton BtnShortCutkeys;

	private DateTimePicker datAsOnDate;

	private Label label17;

	private Panel panel12;

	private Label label24;

	private Panel panel1;

	internal GridEX grd;

	private Button btnShow;

	private ToolStripButton btnSave;

	private ToolStripButton btnprint;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label13;

	internal UltraCombo CmbDifferenceAccount;

	private TextBox txtRemarks;

	private Label label1;

	private ToolStripButton btnSlip;

	private CheckBox chkPrintPreview;

	private Label txtDifferenceAmount;

	private Label lblDiffAmount;

	private Panel AmountsPanel;

	private Label label2;

	private Label txtOpeningAmount;

	private Label label4;

	private Label txtClosingAmount;

	public MonthlyStockClosing(UserAccount pUser)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUser;
	}

	private void MonthlyStockClosing_Load(object sender, EventArgs e)
	{
		try
		{
			formRights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formRights.DoHaveSaveRight;
			btnprint.Enabled = formRights.DoHavePrintRights;
			btnSlip.Enabled = formRights.DoHavePrintRights;
			datAsOnDate.Focus();
			StockCreditAccountBind();
		}
		catch (Exception ex)
		{
			wf.Close();
			MessageBox.Show(ex.Message);
		}
	}

	private void StockCreditAccountBind()
	{
		try
		{
			int ID = Conversion.ToInt(CmbDifferenceAccount.Value);
			DataTable dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("10,12");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbDifferenceAccount, "Id", "AccountTitle", "Difference Account", ZeroIndex: true);
				((UltraGridBase)CmbDifferenceAccount).DisplayLayout.Bands[0].Columns[1].Width = 203;
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbDifferenceAccount.Value = ID;
					}
					else
					{
						((Control)(object)CmbDifferenceAccount).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbDifferenceAccount).Text = string.Empty;
				((UltraGridBase)CmbDifferenceAccount).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			wf.Close();
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			wf.Show();
			GetAvailableStockForMonthlyClosing();
		}
		catch (Exception ex)
		{
			wf.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			wf.Close();
		}
	}

	private void GetAvailableStockForMonthlyClosing()
	{
		try
		{
			DataTable dt = new DataTable();
			DataTable dtData = new DataTable();
			dtData.Columns.Add("HeaderId", typeof(int));
			dtData.Columns.Add("DetailId", typeof(int));
			dtData.Columns.Add("ActionId", typeof(int));
			dtData.Columns.Add("ClosingDate", typeof(DateTime));
			dtData.Columns.Add("ItemId", typeof(int));
			dtData.Columns.Add("ItemName", typeof(string));
			dtData.Columns.Add("PackUomId", typeof(int));
			dtData.Columns.Add("PackUom", typeof(string));
			dtData.Columns.Add("PackEquivalent", typeof(double));
			dtData.Columns.Add("WarehouseId", typeof(int));
			dtData.Columns.Add("WareHouseName", typeof(string));
			dtData.Columns.Add("CropBatch", typeof(string));
			dtData.Columns.Add("JobLotId", typeof(int));
			dtData.Columns.Add("JobLotDescription", typeof(string));
			dtData.Columns.Add("InvPackingTypeId", typeof(int));
			dtData.Columns.Add("PackingType", typeof(string));
			dtData.Columns.Add("Qty", typeof(double));
			dtData.Columns.Add("Weight", typeof(double));
			dtData.Columns.Add("Value", typeof(double));
			dtData.Columns.Add("ClosingRate", typeof(double));
			dtData.Columns.Add("OpeningRate", typeof(double));
			dtData.Columns.Add("RateUomId", typeof(int));
			dtData.Columns.Add("RateUom", typeof(double));
			dtData.Columns.Add("OpeningAmount", typeof(double));
			dt = StockMonthlyClosingHeader.ReadByClosingDateFromStockClosing(datAsOnDate.Value);
			if (dt == null || dt.Rows.Count == 0)
			{
				dt = StockMonthlyClosingHeader.GetAvailableStockForMonthlyClosing(UserAccount.OrganizationId, UserAccount.CompanyId, datAsOnDate.Value);
				if (dt.Rows.Count > 0)
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						dtData.Rows.Add(0, 0, 1, Conversion.ToDateTime((object)datAsOnDate.Value).ToShortDateString(), dt.Rows[i]["ItemId"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemUomId"], dt.Rows[i]["PackUom"], dt.Rows[i]["Equivalent"], dt.Rows[i]["WarehouseId"], dt.Rows[i]["WareHouseName"], dt.Rows[i]["CropBatch"], dt.Rows[i]["JobLotId"], dt.Rows[i]["JobLotDescription"], dt.Rows[i]["InvPackingTypeId"], dt.Rows[i]["PackTypeDesc"], dt.Rows[i]["BalQty"], dt.Rows[i]["BalWeight"], dt.Rows[i]["BalAmount"], dt.Rows[i]["AvgRate"], 0, dt.Rows[i]["ItemUomId"], dt.Rows[i]["Equivalent"], 0);
					}
				}
			}
			else
			{
				for (int j = 0; j < dt.Rows.Count; j++)
				{
					if (j == 0)
					{
						RecId = Conversion.ToInt(dt.Rows[j]["HeaderId"]);
						CmbDifferenceAccount.Value = Conversion.ToInt(dt.Rows[j]["AccountId"]);
						txtRemarks.Text = Conversion.ToString(dt.Rows[j]["Remarks"]);
					}
					dtData.Rows.Add(dt.Rows[j]["HeaderId"], dt.Rows[j]["Id"], 2, Conversion.ToDateTime((object)datAsOnDate.Value).ToShortDateString(), dt.Rows[j]["ItemId"], dt.Rows[j]["ItemName"], dt.Rows[j]["ItemUomId"], dt.Rows[j]["PackUom"], dt.Rows[j]["PackEquivalent"], dt.Rows[j]["WarehouseId"], dt.Rows[j]["WareHouseName"], dt.Rows[j]["CropYear"], dt.Rows[j]["JobLotId"], dt.Rows[j]["JobLotDescription"], dt.Rows[j]["InvPackingTypeId"], dt.Rows[j]["PackTypeDesc"], dt.Rows[j]["Qty"], dt.Rows[j]["Weight"], dt.Rows[j]["Amount"], dt.Rows[j]["ClosingRate"], dt.Rows[j]["OpeningRate"], dt.Rows[j]["RateUomId"], dt.Rows[j]["RateUom"], dt.Rows[j]["OpeningAmount"]);
				}
			}
			grd.DataSource = dtData;
			grd.RetrieveStructure();
			if (dtData.Rows.Count > 0)
			{
				AmountsPanel.Visible = true;
				CalculateDifferenceAmount();
			}
			else
			{
				AmountsPanel.Visible = false;
			}
			grd.FilterMode = (FilterMode)2;
			grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
			grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
			grd.RootTable.Columns["HeaderId"].Visible = false;
			grd.RootTable.Columns["RateUomId"].Visible = false;
			grd.RootTable.Columns["DetailId"].Visible = false;
			grd.RootTable.Columns["ActionId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["PackUomId"].Visible = false;
			grd.RootTable.Columns["InvPackingTypeId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["WarehouseId"].Visible = false;
			grd.RootTable.Columns["PackEquivalent"].Visible = false;
			grd.RootTable.Columns["PackEquivalent"].EditType = (EditType)0;
			grd.RootTable.Columns["RateUomId"].EditType = (EditType)0;
			grd.RootTable.Columns["DetailId"].EditType = (EditType)0;
			grd.RootTable.Columns["HeaderId"].EditType = (EditType)0;
			grd.RootTable.Columns["ClosingDate"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grd.RootTable.Columns["PackUomId"].EditType = (EditType)0;
			grd.RootTable.Columns["PackUom"].EditType = (EditType)0;
			grd.RootTable.Columns["Qty"].EditType = (EditType)0;
			grd.RootTable.Columns["Weight"].EditType = (EditType)0;
			grd.RootTable.Columns["Value"].EditType = (EditType)0;
			grd.RootTable.Columns["ClosingRate"].EditType = (EditType)0;
			grd.RootTable.Columns["WarehouseId"].EditType = (EditType)0;
			grd.RootTable.Columns["WareHouseName"].EditType = (EditType)0;
			grd.RootTable.Columns["CropBatch"].EditType = (EditType)0;
			grd.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grd.RootTable.Columns["JobLotDescription"].EditType = (EditType)0;
			grd.RootTable.Columns["InvPackingTypeId"].EditType = (EditType)0;
			grd.RootTable.Columns["PackingType"].EditType = (EditType)0;
			grd.RootTable.Columns["OpeningAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["ClosingDate"].FilterEditType = (FilterEditType)6;
			grd.RootTable.Columns["ItemId"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["ItemName"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["PackUomId"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["PackUom"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["WareHouseName"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["CropBatch"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["JobLotDescription"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["PackingType"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["RateUom"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Qty"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Weight"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Value"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["ClosingRate"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["OpeningAmount"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Weight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["Value"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Value"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Value"].FormatString = "#,##0.####";
			grd.RootTable.Columns["Value"].TotalFormatString = "#,##0.####";
			grd.RootTable.Columns["OpeningAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["OpeningAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["OpeningAmount"].FormatString = "#,##0.####";
			grd.RootTable.Columns["OpeningAmount"].TotalFormatString = "#,##0.####";
			grd.RootTable.Columns["ClosingRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ClosingRate"].FormatString = "#,##0.####";
			grd.RootTable.Columns["OpeningRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["OpeningRate"].FormatString = "#,##0.####";
			grd.RootTable.Columns["ClosingDate"].Width = 85;
			grd.RootTable.Columns["ItemName"].Width = 200;
			grd.RootTable.Columns["PackUom"].Width = 75;
			grd.RootTable.Columns["RateUom"].Width = 75;
			grd.RootTable.Columns["Qty"].Width = 90;
			grd.RootTable.Columns["Weight"].Width = 100;
			grd.RootTable.Columns["Value"].Width = 110;
			grd.RootTable.Columns["ClosingRate"].Width = 95;
			grd.RootTable.Columns["OpeningRate"].Width = 100;
			grd.RootTable.Columns["OpeningAmount"].Width = 110;
			ctrlGrdBar1_Load(null, null);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
			if (fs != null)
			{
				grd.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			wf.Close();
			MessageBox.Show(ex.ToString());
		}
	}

	private void MonthlyStockClosing_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.Shift && e.KeyCode == Keys.G)
			{
				ctrlGrdBar1.Visible = true;
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				datAsOnDate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				CmbDifferenceAccount.Focus();
			}
			if (e.Control && e.KeyCode == Keys.S && formRights.DoHaveSaveRight)
			{
				btnSave_Click(null, null);
			}
			if (e.Alt && (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1) && formRights.DoHavePrintRights)
			{
				btnSlip_Click(null, null);
			}
			if (e.Alt && (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2) && formRights.DoHavePrintRights)
			{
				btnprint_Click(null, null);
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

	private void btnSave_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Expected O, but got Unknown
		try
		{
			StockMonthlyClosingHeader obj = new StockMonthlyClosingHeader();
			obj.StockMonthlyClosingList = new List<StockMonthlyClosing>();
			if (grd.GetRows().Count() == 0)
			{
				throw new Exception("No Record found");
			}
			if (((UltraGridBase)CmbDifferenceAccount).ActiveRow == null || Conversion.ToInt(CmbDifferenceAccount.Value) == 0)
			{
				CmbDifferenceAccount.Focus();
				throw new Exception("Difference Account field is required");
			}
			if (txtRemarks.Text == null || txtRemarks.Text == string.Empty)
			{
				txtRemarks.Focus();
				throw new Exception("Remarks field is required");
			}
			GridEXRow row = grd.GetRow(0);
			if (Conversion.ToInt(row.Cells["HeaderId"].Value) > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.Id = Conversion.ToInt(row.Cells["HeaderId"].Value);
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 300;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyUserId = UserAccount.ID;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.AccountId = Conversion.ToInt(CmbDifferenceAccount.Value);
			obj.Remarks = txtRemarks.Text;
			obj.DocDate = datAsOnDate.Value;
			obj.ActionId = Conversion.ToInt(row.Cells["ActionId"].Value);
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow item in rows)
			{
				StockMonthlyClosing d = new StockMonthlyClosing();
				d.OrganizationId = UserAccount.OrganizationId;
				d.CompanyId = UserAccount.CompanyId;
				d.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				d.DocumentTypeId = 300;
				d.EntryUserId = UserAccount.ID;
				d.ModifyUserId = UserAccount.ID;
				d.ApprovedUserId = UserAccount.ID;
				d.EntryDate = DateTime.Now;
				d.ModifyDate = DateTime.Now;
				d.ApprovedDate = DateTime.Now;
				d.Id = Conversion.ToInt(item.Cells["DetailId"].Value);
				d.ActionId = Conversion.ToInt(item.Cells["ActionId"].Value);
				d.ClosingDate = Conversion.ToDateTime(item.Cells["ClosingDate"].Value);
				d.ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
				d.ItemUomId = Conversion.ToInt(item.Cells["PackUomId"].Value);
				d.WarehouseId = Conversion.ToInt(item.Cells["WarehouseId"].Value);
				d.JobLotId = Conversion.ToInt(item.Cells["JobLotId"].Value);
				d.InvPackingTypeId = Conversion.ToInt(item.Cells["InvPackingTypeId"].Value);
				d.CropYear = Conversion.ToString(item.Cells["CropBatch"].Value);
				d.Qty = Conversion.ToDouble(item.Cells["Qty"].Value);
				if (d.Qty == 0.0)
				{
					d.Qty = 1.0;
				}
				d.Weight = Conversion.ToDouble(item.Cells["Weight"].Value);
				d.Amount = Conversion.ToDouble(item.Cells["Value"].Value);
				d.ClosingRate = Conversion.ToDouble(item.Cells["ClosingRate"].Value);
				d.OpeningRate = Conversion.ToDouble(item.Cells["OpeningRate"].Value);
				d.OpeningAmount = Conversion.ToDouble(item.Cells["OpeningAmount"].Value);
				d.Remarks = txtRemarks.Text;
				d.AccountId = obj.AccountId;
				obj.StockMonthlyClosingList.Add(d);
			}
			StockMonthlyClosingHeader.Save(obj);
			if (Conversion.ToInt(row.Cells["ActionId"].Value) == 2)
			{
				MessageBox.Show("Record Update Successfully ");
			}
			else
			{
				MessageBox.Show("Record Save Successfully ");
			}
			grd.ClearStructure();
			((Control)(object)CmbDifferenceAccount).Text = string.Empty;
			txtRemarks.Text = string.Empty;
			if (chkPrintPreview.Checked)
			{
				CommonServices.StockMonthlyClosing_Slip(datAsOnDate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnprint_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Record not found for display");
			}
			CommonServices.AcRptPurchaseSalesVoucherSlip_103(CommonServices.VoucherHeadIdGet(RecId, 300), 300);
		}
		catch (Exception ex)
		{
			wf.Close();
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "OpeningRate")
			{
				double num = Conversion.ToDouble(r.Cells["Weight"].Value);
				double Equivalent = Conversion.ToDouble(r.Cells["PackEquivalent"].Value);
				double OpeningRate = Conversion.ToDouble(r.Cells["OpeningRate"].Value);
				double Amount = num / Equivalent * OpeningRate;
				r.Cells["OpeningAmount"].Value = Amount;
			}
			grd.UpdateData();
			CalculateDifferenceAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateDifferenceAmount()
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				double ClosingAmount = Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["Value"], (AggregateFunction)2));
				double OpeningAmount = Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["OpeningAmount"], (AggregateFunction)2));
				double diffAmount = ClosingAmount - OpeningAmount;
				txtClosingAmount.Text = ClosingAmount.ToString("#,##0.####");
				txtOpeningAmount.Text = OpeningAmount.ToString("#,##0.####");
				txtDifferenceAmount.Text = diffAmount.ToString("#,##0.####");
			}
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
			((Control)(object)CmbDifferenceAccount).Text = string.Empty;
			txtRemarks.Text = string.Empty;
			grd.ClearStructure();
			AmountsPanel.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSlip_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Record not found for display");
			}
			CommonServices.StockMonthlyClosing_Slip(datAsOnDate.Value);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShortCutkeys_Click(object sender, EventArgs e)
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
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Alt+1", "For Print Slip");
			dt.Rows.Add("Alt+2", "For Print Voucher");
			dt.Rows.Add("Ctrl+F5", "For Focus on AsOnDate");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Difference Account");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MonthlyStockClosing_FormClosed(object sender, FormClosedEventArgs e)
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
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Expected O, but got Unknown
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Expected O, but got Unknown
		//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ac: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnSlip = new System.Windows.Forms.ToolStripButton();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.datAsOnDate = new System.Windows.Forms.DateTimePicker();
		this.label17 = new System.Windows.Forms.Label();
		this.panel12 = new System.Windows.Forms.Panel();
		this.chkPrintPreview = new System.Windows.Forms.CheckBox();
		this.label24 = new System.Windows.Forms.Label();
		this.grd = new GridEX();
		this.panel1 = new System.Windows.Forms.Panel();
		this.AmountsPanel = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.txtClosingAmount = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtOpeningAmount = new System.Windows.Forms.Label();
		this.lblDiffAmount = new System.Windows.Forms.Label();
		this.txtDifferenceAmount = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.CmbDifferenceAccount = new UltraCombo();
		this.btnShow = new System.Windows.Forms.Button();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip1.SuspendLayout();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel1.SuspendLayout();
		this.AmountsPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDifferenceAccount).BeginInit();
		base.SuspendLayout();
		this.toolStrip1.BackColor = System.Drawing.Color.White;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnnew, this.btnSave, this.btnSlip, this.btnprint, this.BtnShortCutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(800, 27);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnSlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlip.Name = "btnSlip";
		this.btnSlip.Size = new System.Drawing.Size(85, 24);
		this.btnSlip.Text = "300-Slip";
		this.btnSlip.ToolTipText = "300-StockMonthlyClosingSlip";
		this.btnSlip.Click += new System.EventHandler(btnSlip_Click);
		this.btnprint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(115, 24);
		this.btnprint.Text = "103-Voucher";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Visible = false;
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.datAsOnDate.CustomFormat = "dd-MMM-yyyy";
		this.datAsOnDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datAsOnDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datAsOnDate.Location = new System.Drawing.Point(134, 35);
		this.datAsOnDate.Name = "datAsOnDate";
		this.datAsOnDate.Size = new System.Drawing.Size(168, 23);
		this.datAsOnDate.TabIndex = 2;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(6, 38);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(82, 17);
		this.label17.TabIndex = 5;
		this.label17.Text = "As On Date:";
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.chkPrintPreview);
		this.panel12.Controls.Add(this.label24);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 27);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(800, 26);
		this.panel12.TabIndex = 54;
		this.chkPrintPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPrintPreview.AutoSize = true;
		this.chkPrintPreview.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPrintPreview.ForeColor = System.Drawing.Color.White;
		this.chkPrintPreview.Location = new System.Drawing.Point(695, 4);
		this.chkPrintPreview.Name = "chkPrintPreview";
		this.chkPrintPreview.Size = new System.Drawing.Size(102, 19);
		this.chkPrintPreview.TabIndex = 119;
		this.chkPrintPreview.Text = "Print Preview";
		this.chkPrintPreview.UseVisualStyleBackColor = true;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label24.ForeColor = System.Drawing.Color.White;
		this.label24.Location = new System.Drawing.Point(4, 3);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(188, 19);
		this.label24.TabIndex = 35;
		this.label24.Text = "Stock Monthly Closing";
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 129);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(800, 389);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 56;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.panel1.Controls.Add(this.AmountsPanel);
		this.panel1.Controls.Add(this.txtRemarks);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Controls.Add(this.label13);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDifferenceAccount);
		this.panel1.Controls.Add(this.btnShow);
		this.panel1.Controls.Add(this.datAsOnDate);
		this.panel1.Controls.Add(this.label17);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 53);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(800, 76);
		this.panel1.TabIndex = 55;
		this.AmountsPanel.Controls.Add(this.label4);
		this.AmountsPanel.Controls.Add(this.txtClosingAmount);
		this.AmountsPanel.Controls.Add(this.label2);
		this.AmountsPanel.Controls.Add(this.txtOpeningAmount);
		this.AmountsPanel.Controls.Add(this.lblDiffAmount);
		this.AmountsPanel.Controls.Add(this.txtDifferenceAmount);
		this.AmountsPanel.Location = new System.Drawing.Point(423, 30);
		this.AmountsPanel.Name = "AmountsPanel";
		this.AmountsPanel.Size = new System.Drawing.Size(373, 42);
		this.AmountsPanel.TabIndex = 121;
		this.AmountsPanel.Visible = false;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(3, 3);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(106, 17);
		this.label4.TabIndex = 123;
		this.label4.Text = "Closing Amount";
		this.txtClosingAmount.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtClosingAmount.Location = new System.Drawing.Point(4, 22);
		this.txtClosingAmount.Name = "txtClosingAmount";
		this.txtClosingAmount.Size = new System.Drawing.Size(110, 14);
		this.txtClosingAmount.TabIndex = 124;
		this.txtClosingAmount.Text = "0";
		this.txtClosingAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(120, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(114, 17);
		this.label2.TabIndex = 121;
		this.label2.Text = "Opening Amount";
		this.txtOpeningAmount.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOpeningAmount.Location = new System.Drawing.Point(123, 22);
		this.txtOpeningAmount.Name = "txtOpeningAmount";
		this.txtOpeningAmount.Size = new System.Drawing.Size(119, 14);
		this.txtOpeningAmount.TabIndex = 122;
		this.txtOpeningAmount.Text = "0";
		this.txtOpeningAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblDiffAmount.AutoSize = true;
		this.lblDiffAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDiffAmount.Location = new System.Drawing.Point(246, 3);
		this.lblDiffAmount.Name = "lblDiffAmount";
		this.lblDiffAmount.Size = new System.Drawing.Size(122, 17);
		this.lblDiffAmount.TabIndex = 119;
		this.lblDiffAmount.Text = "Difference Amount";
		this.txtDifferenceAmount.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDifferenceAmount.Location = new System.Drawing.Point(248, 20);
		this.txtDifferenceAmount.Name = "txtDifferenceAmount";
		this.txtDifferenceAmount.Size = new System.Drawing.Size(122, 17);
		this.txtDifferenceAmount.TabIndex = 120;
		this.txtDifferenceAmount.Text = "0";
		this.txtDifferenceAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(423, 6);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(373, 23);
		this.txtRemarks.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(362, 9);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(60, 17);
		this.label1.TabIndex = 118;
		this.label1.Text = "Remarks";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(6, 10);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(122, 17);
		this.label13.TabIndex = 117;
		this.label13.Text = "Difference Account";
		this.CmbDifferenceAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbDifferenceAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDifferenceAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDifferenceAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance123;
		((SpecialBoxBase)((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance128).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDifferenceAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDifferenceAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDifferenceAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDifferenceAccount).Location = new System.Drawing.Point(134, 5);
		((System.Windows.Forms.Control)(object)this.CmbDifferenceAccount).Name = "CmbDifferenceAccount";
		((System.Windows.Forms.Control)(object)this.CmbDifferenceAccount).Size = new System.Drawing.Size(227, 26);
		((System.Windows.Forms.Control)(object)this.CmbDifferenceAccount).TabIndex = 0;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatAppearance.BorderSize = 0;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(308, 35);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(53, 23);
		this.btnShow.TabIndex = 3;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(760, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 57;
		this.ctrlGrdBar1.Visible = false;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(800, 518);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		base.Controls.Add(this.ctrlGrdBar1);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel12);
		base.Controls.Add(this.toolStrip1);
		base.KeyPreview = true;
		base.Name = "MonthlyStockClosing";
		this.Text = "Monthly Stock Closing";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(MonthlyStockClosing_FormClosed);
		base.Load += new System.EventHandler(MonthlyStockClosing_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(MonthlyStockClosing_KeyDown);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.AmountsPanel.ResumeLayout(false);
		this.AmountsPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDifferenceAccount).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
