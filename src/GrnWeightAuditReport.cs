// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
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
namespace Architecture.WinApp.Audit_Dashboard
{

public class GrnWeightAuditReport : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dt = new DataTable();

	private DataTable dtBindCombo = new DataTable();

	public DataTable dtViewAttachments = new DataTable();

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private Panel panel3;

	private Label label23;

	private Panel panel4;

	private Panel panel5;

	private Label label2;

	internal UltraCombo CmbSupplier;

	private TextBox txtGrnNoTo;

	private Label label1;

	private TextBox txtGrnNoFrom;

	private DateTimePicker GRNfromdate;

	private Label label27;

	private TextBox GpsNoTo;

	private TextBox GpsNoFrom;

	private DateTimePicker GRNToDate;

	internal GridEX DataGridHistory;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnshow;

	private ToolStripButton btn334Register;

	internal UltraCombo CmbItem;

	private Label label9;

	private ToolStripButton btnRefresh;

	private Panel panel6;

	private Label label4;

	private GroupBox groupBox1;

	private BackgroundWorker backgroundWorker1;

	private ToolStripButton btnShortCutKeys;

	private Label label10;

	private Label label3;

	internal UltraCombo cmbperemeter;

	private Label label12;

	private Label label5;

	private TextBox txtDiffFrom;

	private TextBox txtDiffTo;

	public GrnWeightAuditReport(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmGRNHistory_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(950, 600);
			dtViewAttachments.Columns.Add("AttchmentPath");
			ParameterFill();
			AllDropDownBind();
			cmbperemeter.Focus();
			if (Conversion.ToInt(cmbperemeter.Value) == 0)
			{
				((UltraGridBase)cmbperemeter).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ParameterFill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			DDL.BindDDL(dt, cmbperemeter, "Id", "Parameters", "Parameters", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllDropDownBind()
	{
		try
		{
			DataTable dtAll = InvGrn.GetDataForDropDownFromGrn(UserAccount.OrganizationId, UserAccount.CompanyId, (string)null, (string)null, (string)null, 0);
			if (dtAll.Rows.Count > 0)
			{
				EnumerableRowCollection<DataRow> rows = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "Supplier"
					select r;
				DataTable dtParty = (rows.Any() ? rows.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsitem = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "Item"
					select r;
				DataTable dtItem = (rowsitem.Any() ? rowsitem.CopyToDataTable() : dtAll.Clone());
				DDL.BindDDLNew(dtParty, CmbSupplier, "Id", "ReferenceName", "Supplier Name", true);
				((UltraGridBase)CmbSupplier).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dtItem, CmbItem, "Id", "ReferenceName", "Item Name", true);
				((UltraGridBase)CmbItem).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
				GRNfromdate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 2)
			{
				GRNfromdate.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				GRNfromdate.Value = first;
				GRNToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				GRNfromdate.Value = first2;
				GRNToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 5)
			{
				GRNfromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridHisory()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dt.Clear();
			ReportsParameters grn = new ReportsParameters();
			grn.OrganizationId = UserAccount.OrganizationId;
			grn.CompanyId = UserAccount.CompanyId;
			grn.FromDocNo = Conversion.ToInt((object)txtGrnNoFrom.Text.Trim());
			grn.ToDocNo = Conversion.ToInt((object)txtGrnNoTo.Text.Trim());
			grn.GpSrNoF = Conversion.ToInt((object)GpsNoFrom.Text.Trim());
			grn.GpSrNoT = Conversion.ToInt((object)GpsNoTo.Text.Trim());
			grn.FromDate = Conversion.ToDateTime((object)GRNfromdate.Value);
			grn.ToDate = Conversion.ToDateTime((object)GRNToDate.Value);
			grn.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			grn.ItemId = Conversion.ToInt(CmbItem.Value);
			grn.BalanceFrom = Conversion.ToDouble((object)txtDiffFrom.Text);
			grn.BalanceTo = Conversion.ToDouble((object)txtDiffTo.Text);
			dt = InvGrn.GRNWeightAuditReport(grn);
			if (dt.Rows.Count > 0)
			{
				DataTable dtGrn = new DataTable();
				dtGrn.Columns.Add("Id", typeof(int));
				dtGrn.Columns.Add("InvGrnId", typeof(int));
				dtGrn.Columns.Add("InwardGatePassId", typeof(int));
				dtGrn.Columns.Add("SupplierCustomerId", typeof(int));
				dtGrn.Columns.Add("DocDate", typeof(DateTime));
				dtGrn.Columns.Add("DocNo", typeof(int));
				dtGrn.Columns.Add("SupplierName", typeof(string));
				dtGrn.Columns.Add("PartyWeight", typeof(double));
				dtGrn.Columns.Add("FactoryWeight", typeof(double));
				dtGrn.Columns.Add("DiffWt", typeof(double));
				dtGrn.Columns.Add("DeliveryTerm", typeof(string));
				dtGrn.Columns.Add("PurchaseOrderId", typeof(int));
				dtGrn.Columns.Add("PurchaseOrderDetailId", typeof(int));
				dtGrn.Columns.Add("ItemId", typeof(int));
				dtGrn.Columns.Add("ItemName", typeof(string));
				dtGrn.Columns.Add("PackTypeCode", typeof(string));
				dtGrn.Columns.Add("ItemQty", typeof(double));
				dtGrn.Columns.Add("GrossWeight", typeof(double));
				dtGrn.Columns.Add("EbWt", typeof(double));
				dtGrn.Columns.Add("EBWTotal", typeof(double));
				dtGrn.Columns.Add("AddLess", typeof(double));
				dtGrn.Columns.Add("WtCutTotal", typeof(double));
				dtGrn.Columns.Add("BillWeight", typeof(double));
				dtGrn.Columns.Add("StockWeight", typeof(double));
				dtGrn.Columns.Add("BwSwDiff", typeof(double));
				dtGrn.Columns.Add("ItemRate", typeof(double));
				dtGrn.Columns.Add("RateUom", typeof(double));
				dtGrn.Columns.Add("DiffAmount", typeof(double));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					DataRow newRow = dtGrn.NewRow();
					newRow["Id"] = dt.Rows[i]["Id"];
					newRow["InvGrnId"] = dt.Rows[i]["InvGrnId"];
					newRow["InwardGatePassId"] = dt.Rows[i]["InwardGatePassId"];
					newRow["SupplierCustomerId"] = dt.Rows[i]["SupplierCustomerId"];
					newRow["DocDate"] = dt.Rows[i]["DocDate"];
					newRow["DocNo"] = dt.Rows[i]["DocNo"];
					newRow["SupplierName"] = dt.Rows[i]["CompanyName"];
					newRow["PartyWeight"] = dt.Rows[i]["PartyWeight"];
					newRow["FactoryWeight"] = dt.Rows[i]["FactoryWeight"];
					newRow["DiffWt"] = dt.Rows[i]["DiffWt"];
					newRow["DeliveryTerm"] = dt.Rows[i]["DeliveryTerm"];
					newRow["PurchaseOrderId"] = dt.Rows[i]["PurchaseOrderId"];
					newRow["PurchaseOrderDetailId"] = dt.Rows[i]["PurchaseOrderDetailId"];
					newRow["ItemId"] = dt.Rows[i]["ItemId"];
					newRow["ItemName"] = dt.Rows[i]["ItemName"];
					newRow["PackTypeCode"] = dt.Rows[i]["PackTypeCode"];
					newRow["ItemQty"] = dt.Rows[i]["ItemQty"];
					newRow["GrossWeight"] = dt.Rows[i]["GrossWeight"];
					newRow["EbWt"] = dt.Rows[i]["EbWt"];
					newRow["EBWTotal"] = dt.Rows[i]["EBWTotal"];
					newRow["AddLess"] = dt.Rows[i]["AddLess"];
					newRow["WtCutTotal"] = dt.Rows[i]["WtCutTotal"];
					newRow["BillWeight"] = dt.Rows[i]["BillWeight"];
					newRow["StockWeight"] = dt.Rows[i]["StockWeight"];
					newRow["BwSwDiff"] = dt.Rows[i]["BwSwDiff"];
					newRow["ItemRate"] = dt.Rows[i]["ItemRate"];
					newRow["RateUom"] = dt.Rows[i]["RateUom"];
					newRow["DiffAmount"] = dt.Rows[i]["DiffAmount"];
					dtGrn.Rows.Add(newRow);
				}
				DataGridHistory.DataSource = dtGrn;
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
			DataGridHistory.RootTable.Columns["InvGrnId"].Visible = false;
			DataGridHistory.RootTable.Columns["InwardGatePassId"].Visible = false;
			DataGridHistory.RootTable.Columns["SupplierCustomerId"].Visible = false;
			DataGridHistory.RootTable.Columns["PurchaseOrderId"].Visible = false;
			DataGridHistory.RootTable.Columns["PurchaseOrderDetailId"].Visible = false;
			DataGridHistory.RootTable.Columns["ItemId"].Visible = false;
			DataGridHistory.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["GrossWeight"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["EBWTotal"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["EBWTotal"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["EBWTotal"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["EBWTotal"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["WtCutTotal"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["WtCutTotal"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["WtCutTotal"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["WtCutTotal"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["AddLess"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["AddLess"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["AddLess"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["AddLess"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["BillWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["BillWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["BillWeight"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["BillWeight"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["StockWeight"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["BwSwDiff"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["BwSwDiff"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["BwSwDiff"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["BwSwDiff"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["DiffAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["DiffAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["DiffAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["DiffAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["EbWt"].AggregateFunction = (AggregateFunction)3;
			DataGridHistory.RootTable.Columns["PartyWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FactoryWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["DiffWt"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["PartyWeight"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["FactoryWeight"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["DiffWt"].FormatString = "0,0";
			CommonServices.GridAutoAdjustmentNew(DataGridHistory);
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 50;
			DataGridHistory.RootTable.Columns["Print"].Position = 0;
			DataGridHistory.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Print")
			{
				GenerateReport();
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

	private void frmGRNHistory_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbperemeter.Focus();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Alt && e.KeyCode == Keys.D1)
			{
				btn334Register_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)DataGridHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				cmbperemeter.Focus();
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
			dt.Rows.Add("Ctrl+S", "For Shoe Record");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Alt+1", "For Print 334-Register");
			dt.Rows.Add("Alt+2", "For Print 335-Register");
			dt.Rows.Add("Ctrl+F5", "For Focus on Combo Date");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid History");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Date combo in Filters Box");
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

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && DataGridHistory.CurrentColumn != null)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				Conversion.ToInt((object)r.Cells["DocumentTypeId"].Value.ToString());
				if (DataGridHistory.CurrentColumn.Key == "Print")
				{
					GenerateReport();
				}
				if (DataGridHistory.CurrentColumn.Key == "DocNo")
				{
					CommonServices.GrnSlipWithSubReports(Conversion.ToInt(r.Cells["Id"].Value));
				}
				if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), Conversion.ToInt(r.Cells["DocumentTypeId"].Value));
				}
				if (DataGridHistory.CurrentColumn.Key == "GpNo")
				{
					CommonServices.GatePassInwardSlipAndRegisterReport(Conversion.ToInt(r.Cells["InwardGatePassId"].Value));
				}
				if (DataGridHistory.CurrentColumn.Key == "WagesNo")
				{
					CommonServices.ContractorWagesBill_SlipandRegister_002(Conversion.ToInt(r.Cells["WagesId"].Value));
				}
				if (DataGridHistory.CurrentColumn.Key == "PoNo" && Conversion.ToInt(r.Cells["OrderId"].Value) > 0)
				{
					CommonServices.PurchaseOrderSlipReport203(Conversion.ToInt(r.Cells["OrderId"].Value));
				}
			}
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
			GRNfromdate.Focus();
			((Control)(object)CmbSupplier).Text = string.Empty;
			txtGrnNoFrom.Text = string.Empty;
			txtGrnNoTo.Text = string.Empty;
			GpsNoFrom.Text = string.Empty;
			GpsNoTo.Text = string.Empty;
			txtDiffFrom.Text = string.Empty;
			txtDiffTo.Text = string.Empty;
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
			AllDropDownBind();
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
				GRNfromdate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmGRNHistory_FormClosing(object sender, FormClosingEventArgs e)
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

	private void GpsNoFrom_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void GpsNoTo_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtGrnNoFrom_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtGrnNoTo_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void btn334Register_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "387-GRNWeightAuditReport.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReport()
	{
		try
		{
			CommonServices.GrnSlipWithSubReports(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["InvGrnId"].Value));
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
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Expected O, but got Unknown
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Expected O, but got Unknown
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Expected O, but got Unknown
		//IL_0593: Unknown result type (might be due to invalid IL or missing references)
		//IL_059d: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Audit_Dashboard.GrnWeightAuditReport));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label4 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.cmbperemeter = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.GRNfromdate = new System.Windows.Forms.DateTimePicker();
		this.CmbSupplier = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.txtGrnNoFrom = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.txtGrnNoTo = new System.Windows.Forms.TextBox();
		this.CmbItem = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.GRNToDate = new System.Windows.Forms.DateTimePicker();
		this.GpsNoFrom = new System.Windows.Forms.TextBox();
		this.GpsNoTo = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btn334Register = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.txtDiffTo = new System.Windows.Forms.TextBox();
		this.txtDiffFrom = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel6.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(4);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(934, 561);
		this.panel1.TabIndex = 0;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 207);
		this.panel5.Margin = new System.Windows.Forms.Padding(4);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(934, 354);
		this.panel5.TabIndex = 3;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 29);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(934, 325);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Controls.Add(this.label4);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Margin = new System.Windows.Forms.Padding(4);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(934, 29);
		this.panel6.TabIndex = 2;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(887, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar1.TabIndex = 3;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(3, 4);
		this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(67, 19);
		this.label4.TabIndex = 0;
		this.label4.Text = "History";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(0, 57);
		this.panel4.Margin = new System.Windows.Forms.Padding(4);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(934, 150);
		this.panel4.TabIndex = 0;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbperemeter);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.GRNfromdate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.txtGrnNoFrom);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.txtGrnNoTo);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItem);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.GRNToDate);
		this.groupBox1.Controls.Add(this.txtDiffFrom);
		this.groupBox1.Controls.Add(this.txtDiffTo);
		this.groupBox1.Controls.Add(this.GpsNoFrom);
		this.groupBox1.Controls.Add(this.GpsNoTo);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(10, 4);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(916, 141);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.cmbperemeter.AllowNull = (DefaultableBoolean)1;
		this.cmbperemeter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbperemeter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbperemeter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbperemeter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Location = new System.Drawing.Point(110, 17);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Name = "cmbperemeter";
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Size = new System.Drawing.Size(148, 26);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).TabIndex = 0;
		this.cmbperemeter.ValueChanged += new System.EventHandler(cmbperemeter_ValueChanged);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(6, 23);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(60, 15);
		this.label12.TabIndex = 588;
		this.label12.Text = "Date Type";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(6, 106);
		this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(104, 15);
		this.label10.TabIndex = 144;
		this.label10.Text = "Grn No From && To";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(6, 83);
		this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(70, 15);
		this.label3.TabIndex = 143;
		this.label3.Text = "Grn Date To";
		this.GRNfromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNfromdate.CustomFormat = "dd-MMM-yy";
		this.GRNfromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNfromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.GRNfromdate.Location = new System.Drawing.Point(112, 49);
		this.GRNfromdate.Margin = new System.Windows.Forms.Padding(4);
		this.GRNfromdate.Name = "GRNfromdate";
		this.GRNfromdate.Size = new System.Drawing.Size(148, 23);
		this.GRNfromdate.TabIndex = 1;
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(393, 74);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(193, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 9;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(268, 78);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(86, 15);
		this.label1.TabIndex = 121;
		this.label1.Text = "Supplier Name";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(6, 53);
		this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(85, 15);
		this.label27.TabIndex = 119;
		this.label27.Text = "Grn Date From";
		this.txtGrnNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrnNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrnNoFrom.Location = new System.Drawing.Point(110, 106);
		this.txtGrnNoFrom.Margin = new System.Windows.Forms.Padding(4);
		this.txtGrnNoFrom.Name = "txtGrnNoFrom";
		this.txtGrnNoFrom.ShortcutsEnabled = false;
		this.txtGrnNoFrom.Size = new System.Drawing.Size(73, 23);
		this.txtGrnNoFrom.TabIndex = 3;
		this.txtGrnNoFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGrnNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGrnNoFrom_KeyPress);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(268, 106);
		this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(32, 15);
		this.label9.TabIndex = 121;
		this.label9.Text = "Item";
		this.txtGrnNoTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrnNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrnNoTo.Location = new System.Drawing.Point(187, 106);
		this.txtGrnNoTo.Margin = new System.Windows.Forms.Padding(4);
		this.txtGrnNoTo.Name = "txtGrnNoTo";
		this.txtGrnNoTo.ShortcutsEnabled = false;
		this.txtGrnNoTo.Size = new System.Drawing.Size(73, 23);
		this.txtGrnNoTo.TabIndex = 4;
		this.txtGrnNoTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGrnNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGrnNoTo_KeyPress);
		this.CmbItem.AllowNull = (DefaultableBoolean)1;
		this.CmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance39;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance44).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItem).Location = new System.Drawing.Point(393, 106);
		((System.Windows.Forms.Control)(object)this.CmbItem).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.CmbItem).Name = "CmbItem";
		((System.Windows.Forms.Control)(object)this.CmbItem).Size = new System.Drawing.Size(193, 26);
		((System.Windows.Forms.Control)(object)this.CmbItem).TabIndex = 10;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(595, 106);
		this.btnshow.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(65, 26);
		this.btnshow.TabIndex = 11;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.btnshow.Leave += new System.EventHandler(btnshow_Leave);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(266, 23);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(97, 15);
		this.label2.TabIndex = 127;
		this.label2.Text = "GP No From&& To";
		this.GRNToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNToDate.CustomFormat = "dd-MMM-yy";
		this.GRNToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.GRNToDate.Location = new System.Drawing.Point(112, 79);
		this.GRNToDate.Margin = new System.Windows.Forms.Padding(4);
		this.GRNToDate.Name = "GRNToDate";
		this.GRNToDate.Size = new System.Drawing.Size(148, 23);
		this.GRNToDate.TabIndex = 2;
		this.GpsNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.GpsNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GpsNoFrom.Location = new System.Drawing.Point(393, 19);
		this.GpsNoFrom.Margin = new System.Windows.Forms.Padding(4);
		this.GpsNoFrom.Name = "GpsNoFrom";
		this.GpsNoFrom.ShortcutsEnabled = false;
		this.GpsNoFrom.Size = new System.Drawing.Size(94, 23);
		this.GpsNoFrom.TabIndex = 5;
		this.GpsNoFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.GpsNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(GpsNoFrom_KeyPress);
		this.GpsNoTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.GpsNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GpsNoTo.Location = new System.Drawing.Point(491, 19);
		this.GpsNoTo.Margin = new System.Windows.Forms.Padding(4);
		this.GpsNoTo.Name = "GpsNoTo";
		this.GpsNoTo.ShortcutsEnabled = false;
		this.GpsNoTo.Size = new System.Drawing.Size(95, 23);
		this.GpsNoTo.TabIndex = 6;
		this.GpsNoTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.GpsNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(GpsNoTo_KeyPress);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 28);
		this.panel3.Margin = new System.Windows.Forms.Padding(4);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(934, 29);
		this.panel3.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(2, 3);
		this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(305, 19);
		this.label23.TabIndex = 2;
		this.label23.Text = "Goods Receiving Notes Audit Report";
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(4);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(934, 28);
		this.panel2.TabIndex = 0;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.btnRefresh, this.btn334Register, this.btnShortCutKeys });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(934, 27);
		this.toolStrip3.TabIndex = 2;
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
		this.btn334Register.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btn334Register.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btn334Register.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn334Register.Name = "btn334Register";
		this.btn334Register.Size = new System.Drawing.Size(115, 24);
		this.btn334Register.Text = "387-Register";
		this.btn334Register.Click += new System.EventHandler(btn334Register_Click);
		this.btnShortCutKeys.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.Image = (System.Drawing.Image)resources.GetObject("btnShortCutKeys.Image");
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(101, 24);
		this.btnShortCutKeys.Text = "ShortCutKeys";
		this.btnShortCutKeys.ToolTipText = "ShortCutKeys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		this.txtDiffTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDiffTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDiffTo.Location = new System.Drawing.Point(491, 47);
		this.txtDiffTo.Margin = new System.Windows.Forms.Padding(4);
		this.txtDiffTo.Name = "txtDiffTo";
		this.txtDiffTo.ShortcutsEnabled = false;
		this.txtDiffTo.Size = new System.Drawing.Size(95, 23);
		this.txtDiffTo.TabIndex = 8;
		this.txtDiffTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDiffTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(GpsNoTo_KeyPress);
		this.txtDiffFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDiffFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDiffFrom.Location = new System.Drawing.Point(393, 47);
		this.txtDiffFrom.Margin = new System.Windows.Forms.Padding(4);
		this.txtDiffFrom.Name = "txtDiffFrom";
		this.txtDiffFrom.ShortcutsEnabled = false;
		this.txtDiffFrom.Size = new System.Drawing.Size(94, 23);
		this.txtDiffFrom.TabIndex = 7;
		this.txtDiffFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDiffFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(GpsNoFrom_KeyPress);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(266, 51);
		this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(119, 15);
		this.label5.TabIndex = 127;
		this.label5.Text = "Weight Diff From & To";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(934, 561);
		base.Controls.Add(this.panel1);
		this.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "GrnWeightAuditReport";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmGRNHistory_FormClosing);
		base.Load += new System.EventHandler(frmGRNHistory_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmGRNHistory_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
