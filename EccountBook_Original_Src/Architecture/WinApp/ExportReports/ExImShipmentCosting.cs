using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.BLL.Reports.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.ExportReports;

public class ExImShipmentCosting : Form
{
	public int InvoiceId;

	private DataTable dtGrid = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private Panel panel3;

	private Panel panel2;

	private ToolStrip tsItemType;

	private ToolStripButton btnnew;

	private Label label2;

	private Button btnShow;

	private Panel panel5;

	private Label label3;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton btnRegister;

	internal GridEX grdfrm;

	private GroupBox groupBox1;

	private Label label8;

	private TextBox txtGainLoss;

	private Label label7;

	private TextBox txtPackingMaterial;

	private Label label6;

	private TextBox txtStockValue;

	private Label label5;

	private TextBox txtSaleValue;

	private Label label4;

	private TextBox txtLogisticgsValue;

	private Label label1;

	public UltraCombo cmbInvoiceNo;

	private TextBox txtCommissionValue;

	private Label label9;

	private Label txtPlValue;

	public ExImShipmentCosting(UserAccount pUserAccount)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void ExImShipmentCosting_Load(object sender, EventArgs e)
	{
		try
		{
			InvoiceNoGetAll();
			if (InvoiceId > 0)
			{
				cmbInvoiceNo.Value = InvoiceId;
				btnShow_Click(null, null);
			}
			cmbInvoiceNo.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvoiceNoGetAll()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = ExImInvoice.GetExportInvoicesFromVouchers(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbInvoiceNo, "Id", "InvoiceNo", "Invoice No", ZeroIndex: false);
				((UltraGridBase)cmbInvoiceNo).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			GridBind();
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
			((Control)(object)cmbInvoiceNo).Text = string.Empty;
			cmbInvoiceNo.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmExportShipingLineBookingRpt_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdfrm).Name);
			if (fs != null)
			{
				grdfrm.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridBind()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)cmbInvoiceNo).ActiveRow == null || Conversion.ToInt(cmbInvoiceNo.Value) == 0)
			{
				cmbInvoiceNo.Focus();
				throw new Exception("Please select Invoice No first!");
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.ExImInvoiceId = Conversion.ToInt(cmbInvoiceNo.Value);
			dtGrid = ExportReports.EximShipmentCostingReport(obj);
			if (dtGrid.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("SortNo", typeof(int));
				dt.Columns.Add("ExImInvoiceId", typeof(int));
				dt.Columns.Add("SubSortNo", typeof(int));
				dt.Columns.Add("MasterType", typeof(string));
				dt.Columns.Add("VDate", typeof(DateTime));
				dt.Columns.Add("VType", typeof(string));
				dt.Columns.Add("VNo", typeof(int));
				dt.Columns.Add("InvoiceNo", typeof(string));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("JobLot", typeof(string));
				dt.Columns.Add("TranRemarks", typeof(string));
				dt.Columns.Add("ItemRate", typeof(double));
				dt.Columns.Add("Qty", typeof(int));
				dt.Columns.Add("NetWeight", typeof(double));
				dt.Columns.Add("FcyCode", typeof(string));
				dt.Columns.Add("ExchangeRate", typeof(double));
				dt.Columns.Add("FcyAmount", typeof(double));
				dt.Columns.Add("TranAmount", typeof(double));
				dt.Columns.Add("TotalTranAmount", typeof(double));
				dt.Columns.Add("ClearForwardValueTotal", typeof(double));
				dt.Columns.Add("PackingMaterialValueTotal", typeof(double));
				dt.Columns.Add("SalesValueTotal", typeof(double));
				dt.Columns.Add("GainLossValueTotal", typeof(double));
				dt.Columns.Add("ShipmentWeight", typeof(double));
				for (int i = 0; i < dtGrid.Rows.Count; i++)
				{
					if (Conversion.ToDouble(dtGrid.Rows[i]["StockValueTotal"]) > 0.0)
					{
						double Value6 = Conversion.ToDouble(dtGrid.Rows[i]["StockValueTotal"]);
						txtStockValue.Text = Value6.ToString(clsGlobalVariables.stringFormatsingle);
					}
					if (Conversion.ToDouble(dtGrid.Rows[i]["SalesValueTotal"]) > 0.0)
					{
						double Value5 = Conversion.ToDouble(dtGrid.Rows[i]["SalesValueTotal"]);
						txtSaleValue.Text = Value5.ToString(clsGlobalVariables.stringFormatsingleForFcy);
					}
					if (Conversion.ToDouble(dtGrid.Rows[i]["ClearForwardValueTotal"]) > 0.0)
					{
						double Value4 = Conversion.ToDouble(dtGrid.Rows[i]["ClearForwardValueTotal"]);
						txtLogisticgsValue.Text = Value4.ToString(clsGlobalVariables.stringFormatsingle);
					}
					if (Conversion.ToDouble(dtGrid.Rows[i]["PackingMaterialValueTotal"]) > 0.0)
					{
						double Value3 = Conversion.ToDouble(dtGrid.Rows[i]["PackingMaterialValueTotal"]);
						txtPackingMaterial.Text = Value3.ToString(clsGlobalVariables.stringFormatsingle);
					}
					if (Conversion.ToDouble(dtGrid.Rows[i]["GainLossValueTotal"]) > 0.0)
					{
						double Value2 = Conversion.ToDouble(dtGrid.Rows[i]["GainLossValueTotal"]);
						txtGainLoss.Text = Value2.ToString(clsGlobalVariables.stringFormatsingle);
					}
					if (Conversion.ToDouble(dtGrid.Rows[i]["CommissionValueTotal"]) > 0.0)
					{
						double Value = Conversion.ToDouble(dtGrid.Rows[i]["CommissionValueTotal"]);
						txtCommissionValue.Text = Value.ToString(clsGlobalVariables.stringFormatsingle);
					}
					dt.Rows.Add(Conversion.ToInt(dtGrid.Rows[i]["SortNo"]), Conversion.ToInt(dtGrid.Rows[i]["ExImInvoiceId"]), Conversion.ToInt(dtGrid.Rows[i]["SubSortNo"]), Conversion.ToString(dtGrid.Rows[i]["MasterType"]), Conversion.ToDateTime(dtGrid.Rows[i]["VDate"]), Conversion.ToString(dtGrid.Rows[i]["VType"]), Conversion.ToInt(dtGrid.Rows[i]["VNo"]), Conversion.ToString(dtGrid.Rows[i]["InvoiceNo"]), Conversion.ToString(dtGrid.Rows[i]["ItemName"]), Conversion.ToString(dtGrid.Rows[i]["JobLot"]), Conversion.ToString(dtGrid.Rows[i]["TranRemarks"]), Conversion.ToDouble(dtGrid.Rows[i]["ItemRate"]), Conversion.ToDouble(dtGrid.Rows[i]["Qty"]), Conversion.ToDouble(dtGrid.Rows[i]["NetWeight"]), Conversion.ToString(dtGrid.Rows[i]["FcyCode"]), Conversion.ToDouble(dtGrid.Rows[i]["ExchangeRate"]), Conversion.ToDouble(dtGrid.Rows[i]["FcyAmount"]), Conversion.ToDouble(dtGrid.Rows[i]["TranAmount"]), Conversion.ToDouble(dtGrid.Rows[i]["TotalTranAmount"]), Conversion.ToDouble(dtGrid.Rows[i]["ClearForwardValueTotal"]), Conversion.ToDouble(dtGrid.Rows[i]["PackingMaterialValueTotal"]), Conversion.ToDouble(dtGrid.Rows[i]["SalesValueTotal"]), Conversion.ToDouble(dtGrid.Rows[i]["GainLossValueTotal"]), Conversion.ToDouble(dtGrid.Rows[i]["ShipmentWeight"]));
					grdfrm.DataSource = dt;
					grdfrm.RetrieveStructure();
					GridSetting();
					ShimpmentPLCalculate();
				}
			}
			else
			{
				txtSaleValue.Text = string.Empty;
				txtStockValue.Text = string.Empty;
				txtPackingMaterial.Text = string.Empty;
				txtLogisticgsValue.Text = string.Empty;
				txtGainLoss.Text = string.Empty;
				txtCommissionValue.Text = string.Empty;
				txtPlValue.Text = string.Empty;
				grdfrm.ClearStructure();
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
			grdfrm.RootTable.Columns["SortNo"].Visible = false;
			grdfrm.RootTable.Columns["SubSortNo"].Visible = false;
			grdfrm.RootTable.Columns["ExImInvoiceId"].Visible = false;
			grdfrm.RootTable.Columns["TotalTranAmount"].Visible = false;
			grdfrm.RootTable.Columns["SalesValueTotal"].Visible = false;
			grdfrm.RootTable.Columns["GainLossValueTotal"].Visible = false;
			grdfrm.RootTable.Columns["ClearForwardValueTotal"].Visible = false;
			grdfrm.RootTable.Columns["PackingMaterialValueTotal"].Visible = false;
			grdfrm.RootTable.Columns["ShipmentWeight"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdfrm, 2, 3, null, null, null, ColumnSetting: false);
			grdfrm.RootTable.Columns["MasterType"].Width = Constants.InventoryConstants.CastingType;
			grdfrm.RootTable.Columns["VDate"].Width = Constants.ExportConstants.DateConstant + 20;
			grdfrm.RootTable.Columns["VDate"].Width = Constants.ExportConstants.DateConstant;
			grdfrm.RootTable.Columns["VType"].Width = Constants.ExportConstants.StringConstant - 40;
			grdfrm.RootTable.Columns["VNo"].Width = Constants.ExportConstants.DocNoConstant;
			grdfrm.RootTable.Columns["InvoiceNo"].Width = Constants.ExportConstants.InvoiceNoConstant;
			grdfrm.RootTable.Columns["ItemName"].Width = Constants.ExportConstants.ItemNameConstants;
			grdfrm.RootTable.Columns["JobLot"].Width = Constants.ExportConstants.JobLotConstant;
			grdfrm.RootTable.Columns["TranRemarks"].Width = Constants.ExportConstants.RemarksConstants;
			grdfrm.RootTable.Columns["ItemRate"].Width = Constants.ExportConstants.ExRateConstant;
			grdfrm.RootTable.Columns["Qty"].Width = Constants.ExportConstants.QtyConstant;
			grdfrm.RootTable.Columns["NetWeight"].Width = Constants.ExportConstants.WeightConstant;
			grdfrm.RootTable.Columns["FcyCode"].Width = Constants.ExportConstants.FcyConstant;
			grdfrm.RootTable.Columns["ExchangeRate"].Width = Constants.ExportConstants.ExRateConstant;
			grdfrm.RootTable.Columns["FcyAmount"].Width = Constants.ExportConstants.AmountConstant;
			grdfrm.RootTable.Columns["TranAmount"].Width = Constants.ExportConstants.AmountConstant;
			grdfrm.RootTable.Columns["TotalTranAmount"].Width = Constants.ExportConstants.AmountConstant;
			grdfrm.RootTable.Columns["ClearForwardValueTotal"].Width = Constants.ExportConstants.AmountConstant;
			grdfrm.RootTable.Columns["PackingMaterialValueTotal"].Width = Constants.ExportConstants.AmountConstant;
			grdfrm.RootTable.Columns["SalesValueTotal"].Width = Constants.ExportConstants.AmountConstant;
			grdfrm.RootTable.Columns["GainLossValueTotal"].Width = Constants.ExportConstants.AmountConstant;
			grdfrm.RootTable.Columns["ShipmentWeight"].Width = Constants.ExportConstants.WeightConstant;
			grdfrm.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["ItemRate"].FormatString = "#,#.####";
			grdfrm.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["Qty"].FormatString = "#,##0.##";
			grdfrm.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grdfrm.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["ExchangeRate"].FormatString = "#,##0.####";
			grdfrm.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdfrm.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdfrm.RootTable.Columns["GainLossValueTotal"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["GainLossValueTotal"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdfrm.RootTable.Columns["TranAmount"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["TranAmount"].FormatString = "#,##0.###";
			grdfrm.RootTable.Columns["TranAmount"].TotalFormatString = "#,##0.###";
			grdfrm.RootTable.Columns["TotalTranAmount"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["TotalTranAmount"].FormatString = "#,##0.####";
			grdfrm.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["NetWeight"].FormatString = "#,##0.###";
			grdfrm.RootTable.Columns["ClearForwardValueTotal"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["ClearForwardValueTotal"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdfrm.RootTable.Columns["SalesValueTotal"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["SalesValueTotal"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdfrm.RootTable.Columns["ShipmentWeight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["ShipmentWeight"].FormatString = "#,##0.###";
			((ScrollableBase)grdfrm).ScrollBars = (ScrollBars)4;
			grdfrm.ColumnAutoResize = false;
			((Control)(object)grdfrm).Refresh();
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ShimpmentPLCalculate()
	{
		try
		{
			double StockValue = 0.0;
			double SaleValue = 0.0;
			double PackingValue = 0.0;
			double LogisticsValue = 0.0;
			double GainLossValue = 0.0;
			double CommValue = 0.0;
			if (Conversion.ToDouble((object)txtStockValue.Text) > 0.0)
			{
				StockValue = Conversion.ToDouble((object)txtStockValue.Text);
			}
			if (Conversion.ToDouble((object)txtSaleValue.Text) > 0.0)
			{
				SaleValue = Conversion.ToDouble((object)txtSaleValue.Text);
			}
			if (Conversion.ToDouble((object)txtPackingMaterial.Text) > 0.0)
			{
				PackingValue = Conversion.ToDouble((object)txtPackingMaterial.Text);
			}
			if (Conversion.ToDouble((object)txtLogisticgsValue.Text) > 0.0)
			{
				LogisticsValue = Conversion.ToDouble((object)txtLogisticgsValue.Text);
			}
			if (Conversion.ToDouble((object)txtGainLoss.Text) > 0.0)
			{
				GainLossValue = Conversion.ToDouble((object)txtGainLoss.Text);
			}
			if (Conversion.ToDouble((object)txtCommissionValue.Text) > 0.0)
			{
				CommValue = Conversion.ToDouble((object)txtCommissionValue.Text);
			}
			double PlValue = SaleValue - StockValue - PackingValue - LogisticsValue + GainLossValue - CommValue;
			txtPlValue.Text = PlValue.ToString("#,#");
			txtPlValue.ForeColor = ((PlValue > 0.0) ? Color.Green : ((PlValue < 0.0) ? Color.Red : Color.Black));
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
			if (dtGrid.Rows.Count == 0 || dtGrid == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtGrid, "542-ShipmentCostingRegister.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExImShipmentCosting_FormClosed(object sender, FormClosedEventArgs e)
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
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Expected O, but got Unknown
		GridEXLayout grdfrm_DesignTimeLayout = new GridEXLayout();
		Appearance appearance14 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ExportReports.ExImShipmentCosting));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdfrm = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtCommissionValue = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.txtGainLoss = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.txtLogisticgsValue = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtPackingMaterial = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.txtStockValue = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.txtSaleValue = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label3 = new System.Windows.Forms.Label();
		this.btnShow = new System.Windows.Forms.Button();
		this.cmbInvoiceNo = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRegister = new System.Windows.Forms.ToolStripButton();
		this.txtPlValue = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbInvoiceNo).BeginInit();
		this.panel2.SuspendLayout();
		this.tsItemType.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1022, 665);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 133);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1022, 532);
		this.panel4.TabIndex = 2;
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		grdfrm_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdfrm.DesignTimeLayout = grdfrm_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.DynamicFiltering = true;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupTotals = (GroupTotals)2;
		this.grdfrm.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(1022, 532);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 47;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Controls.Add(this.btnShow);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbInvoiceNo);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1022, 106);
		this.panel3.TabIndex = 0;
		this.groupBox1.Controls.Add(this.txtCommissionValue);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.txtPlValue);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.txtGainLoss);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.txtLogisticgsValue);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.txtPackingMaterial);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.txtStockValue);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.txtSaleValue);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold);
		this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(0, 64, 0);
		this.groupBox1.Location = new System.Drawing.Point(272, 29);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(738, 74);
		this.groupBox1.TabIndex = 2;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Shimpment Summary";
		this.txtCommissionValue.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommissionValue.Location = new System.Drawing.Point(494, 14);
		this.txtCommissionValue.Name = "txtCommissionValue";
		this.txtCommissionValue.ReadOnly = true;
		this.txtCommissionValue.Size = new System.Drawing.Size(122, 23);
		this.txtCommissionValue.TabIndex = 25;
		this.txtCommissionValue.Text = "0";
		this.txtCommissionValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label9.AutoSize = true;
		this.label9.BackColor = System.Drawing.Color.White;
		this.label9.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.Black;
		this.label9.Location = new System.Drawing.Point(408, 17);
		this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(84, 18);
		this.label9.TabIndex = 24;
		this.label9.Text = "Commission";
		this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label8.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.Black;
		this.label8.Location = new System.Drawing.Point(625, 14);
		this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(110, 25);
		this.label8.TabIndex = 24;
		this.label8.Text = "PL Value";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtGainLoss.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGainLoss.Location = new System.Drawing.Point(494, 41);
		this.txtGainLoss.Name = "txtGainLoss";
		this.txtGainLoss.ReadOnly = true;
		this.txtGainLoss.Size = new System.Drawing.Size(122, 23);
		this.txtGainLoss.TabIndex = 25;
		this.txtGainLoss.Text = "0";
		this.txtGainLoss.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.Black;
		this.label7.Location = new System.Drawing.Point(408, 44);
		this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(69, 18);
		this.label7.TabIndex = 24;
		this.label7.Text = "Gain & Loss";
		this.txtLogisticgsValue.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLogisticgsValue.Location = new System.Drawing.Point(294, 41);
		this.txtLogisticgsValue.Name = "txtLogisticgsValue";
		this.txtLogisticgsValue.ReadOnly = true;
		this.txtLogisticgsValue.Size = new System.Drawing.Size(110, 23);
		this.txtLogisticgsValue.TabIndex = 25;
		this.txtLogisticgsValue.Text = "0";
		this.txtLogisticgsValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Black;
		this.label1.Location = new System.Drawing.Point(198, 44);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(98, 18);
		this.label1.TabIndex = 24;
		this.label1.Text = "Logistics Value";
		this.txtPackingMaterial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingMaterial.Location = new System.Drawing.Point(294, 14);
		this.txtPackingMaterial.Name = "txtPackingMaterial";
		this.txtPackingMaterial.ReadOnly = true;
		this.txtPackingMaterial.Size = new System.Drawing.Size(110, 23);
		this.txtPackingMaterial.TabIndex = 25;
		this.txtPackingMaterial.Text = "0";
		this.txtPackingMaterial.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.Black;
		this.label6.Location = new System.Drawing.Point(198, 17);
		this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(93, 18);
		this.label6.TabIndex = 24;
		this.label6.Text = "Packing Value";
		this.txtStockValue.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtStockValue.Location = new System.Drawing.Point(83, 41);
		this.txtStockValue.Name = "txtStockValue";
		this.txtStockValue.ReadOnly = true;
		this.txtStockValue.Size = new System.Drawing.Size(110, 23);
		this.txtStockValue.TabIndex = 25;
		this.txtStockValue.Text = "0";
		this.txtStockValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.Black;
		this.label5.Location = new System.Drawing.Point(4, 44);
		this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(79, 18);
		this.label5.TabIndex = 24;
		this.label5.Text = "Stock Value";
		this.txtSaleValue.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSaleValue.Location = new System.Drawing.Point(83, 14);
		this.txtSaleValue.Name = "txtSaleValue";
		this.txtSaleValue.ReadOnly = true;
		this.txtSaleValue.Size = new System.Drawing.Size(110, 23);
		this.txtSaleValue.TabIndex = 1;
		this.txtSaleValue.Text = "0";
		this.txtSaleValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Black;
		this.label4.Location = new System.Drawing.Point(4, 17);
		this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(72, 18);
		this.label4.TabIndex = 0;
		this.label4.Text = "Sale Value";
		this.panel5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1022, 28);
		this.panel5.TabIndex = 19;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(981, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 0;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(4, 3);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(226, 19);
		this.label3.TabIndex = 0;
		this.label3.Text = "Shipment Costing Register";
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(210, 80);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(56, 23);
		this.btnShow.TabIndex = 1;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.cmbInvoiceNo.AllowNull = (DefaultableBoolean)1;
		this.cmbInvoiceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbInvoiceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbInvoiceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbInvoiceNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Location = new System.Drawing.Point(7, 53);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Name = "cmbInvoiceNo";
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Size = new System.Drawing.Size(259, 26);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(5, 34);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(65, 15);
		this.label2.TabIndex = 18;
		this.label2.Text = "Invoice No";
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.tsItemType);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1022, 27);
		this.panel2.TabIndex = 0;
		this.tsItemType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnnew, this.btnRegister });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(1022, 27);
		this.tsItemType.TabIndex = 3;
		this.tsItemType.Text = "toolStrip4";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRegister.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRegister.Name = "btnRegister";
		this.btnRegister.Size = new System.Drawing.Size(225, 24);
		this.btnRegister.Text = "542-ShipmentCostingRegister";
		this.btnRegister.Click += new System.EventHandler(btnRegister_Click);
		this.txtPlValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPlValue.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPlValue.ForeColor = System.Drawing.Color.Black;
		this.txtPlValue.Location = new System.Drawing.Point(625, 41);
		this.txtPlValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.txtPlValue.Name = "txtPlValue";
		this.txtPlValue.Size = new System.Drawing.Size(110, 23);
		this.txtPlValue.TabIndex = 24;
		this.txtPlValue.Text = "0";
		this.txtPlValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1022, 665);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "ExImShipmentCosting";
		this.Text = "Shipment Costing Register";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(ExImShipmentCosting_FormClosed);
		base.Load += new System.EventHandler(ExImShipmentCosting_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmExportShipingLineBookingRpt_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbInvoiceNo).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		base.ResumeLayout(false);
	}
}
