using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Purchase;

public class LoadPendingGatePassForGrn : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DataTable data = new DataTable();

	private DataTable dtGrid = new DataTable();

	public GridEXRow GatePassLoadedRow;

	private bool FormInitialized;

	public bool LabCompulsoryNotCheckingOnGRN;

	private IContainer components;

	private Panel panel1;

	private ToolStrip toolStrip1;

	private ToolStripButton btnPrint;

	private Panel panel4;

	internal GridEX grdGp;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label4;

	private Label label47;

	private Label label33;

	private Label txtAcceptedGpTotal;

	private Label txtOpenGpTotal;

	public LoadPendingGatePassForGrn(UserAccount pUser, DataTable dt)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		data = dt;
		UserAccount = pUser;
	}

	private void AvgRateByItemPopUp_Load(object sender, EventArgs e)
	{
		try
		{
			LabCompulsoryNotCheckingOnGRN = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("LabCompulsoryNotCheckingOnGRN"));
			FormInitialized = true;
			if (data.Rows.Count > 0)
			{
				if (dtGrid.Columns.Count == 0)
				{
					dtGrid.Columns.Add("Id", typeof(int));
					dtGrid.Columns.Add("GpSrNo", typeof(int));
					dtGrid.Columns.Add("Status", typeof(string));
					dtGrid.Columns.Add("FVStatus", typeof(string));
					dtGrid.Columns.Add("GpDate", typeof(DateTime));
					dtGrid.Columns.Add("OrderType", typeof(string));
					dtGrid.Columns.Add("SupplierName", typeof(string));
					dtGrid.Columns.Add("VehicleNo", typeof(string));
					dtGrid.Columns.Add("BiltyNo", typeof(string));
					dtGrid.Columns.Add("VarietyName", typeof(string));
					dtGrid.Columns.Add("Qty", typeof(decimal));
					dtGrid.Columns.Add("SupplierWeight", typeof(decimal));
					dtGrid.Columns.Add("FactoryWeight", typeof(decimal));
					dtGrid.Columns.Add("ReceivedWeight", typeof(decimal));
					dtGrid.Columns.Add("StockWeight", typeof(decimal));
					dtGrid.Columns.Add("PoAccessWeight", typeof(decimal));
					dtGrid.Columns.Add("VehicleType", typeof(string));
					dtGrid.Columns.Add("NetPaid", typeof(decimal));
					dtGrid.Columns.Add("RefDocumentTypeId", typeof(int));
					dtGrid.Columns.Add("EntryDate", typeof(DateTime));
					dtGrid.Columns.Add("EntryUser", typeof(string));
					dtGrid.Columns.Add("ModifyDate", typeof(DateTime));
					dtGrid.Columns.Add("ModifyUser", typeof(string));
					dtGrid.Columns.Add("DetailIdsCount", typeof(int));
					dtGrid.Columns.Add("FreightOnCash", typeof(decimal));
					dtGrid.Columns.Add("FreightId", typeof(int));
					dtGrid.Columns.Add("LastLabId", typeof(int));
					dtGrid.Columns.Add("TotalPaidAmount", typeof(decimal));
					dtGrid.Columns.Add("TotalDeductionAmount", typeof(decimal));
				}
				dtGrid.Rows.Clear();
				foreach (DataRow row in data.Rows)
				{
					string FVStatus = ((Conversion.ToInt(row["FreightId"]) == 0) ? "Pending" : "Complete");
					dtGrid.Rows.Add(row["Id"], row["GpSrNo"], row["Status"], FVStatus, row["GpDate"], row["OrderType"], row["SupplierName"], row["VehicleNo"], row["BiltyNo"], row["VarietyName"], row["Qty"], row["SupplierWeight"], row["FactoryWeight"], row["ReceivedWeight"], row["StockWeight"], row["AccessWeight"], row["VehicleType"], row["NetPaid"], row["RefDocumentTypeId"], row["EntryDate"], row["EntryUser"], row["ModifyDate"], row["ModifyUser"], row["DetailIdsCount"], row["FreightOnCash"], row["FreightId"], row["LabId"], row["TotalPaidAmount"], row["TotalDeductionAmount"]);
				}
				grdGp.DataSource = dtGrid;
				grdGp.RetrieveStructure();
				GpGridSetting();
			}
			else
			{
				grdGp.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GpGridSetting()
	{
		try
		{
			FillCountFromGpGrid();
			List<string> hiddenColumns = new List<string> { "Id", "LastLabId", "DetailIdsCount", "RefDocumentTypeId", "FreightOnCash", "FreightId" };
			new Dictionary<string, EditType>
			{
				{
					"OrderId",
					(EditType)0
				},
				{
					"RecQty",
					(EditType)1
				},
				{
					"PurQty",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdGp, 2, 2, hiddenColumns);
			grdGp.RootTable.Columns["VarietyName"].Caption = "ItemName";
			grdGp.RootTable.Columns["PoAccessWeight"].Caption = "Po Excess Weight";
			grdGp.RootTable.Columns["GpSrNo"].ColumnType = (ColumnType)5;
			grdGp.RootTable.Groups.Add("Status");
			grdGp.RootTable.Columns["Status"].Visible = false;
			grdGp.RootTable.Columns["GpSrNo"].Width = Constants.InventoryConstants.GpNoConstant;
			grdGp.RootTable.Columns["Status"].Width = Constants.InventoryConstants.StatusConstant;
			grdGp.RootTable.Columns["GpDate"].Width = Constants.InventoryConstants.DateConstant;
			grdGp.RootTable.Columns["OrderType"].Width = Constants.InventoryConstants.GpType;
			grdGp.RootTable.Columns["SupplierName"].Width = Constants.InventoryConstants.SupplierName;
			grdGp.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grdGp.RootTable.Columns["BiltyNo"].Width = Constants.InventoryConstants.BiltyNoConstant;
			grdGp.RootTable.Columns["VarietyName"].Width = Constants.InventoryConstants.ItemName;
			grdGp.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			grdGp.RootTable.Columns["SupplierWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdGp.RootTable.Columns["FactoryWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdGp.RootTable.Columns["ReceivedWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdGp.RootTable.Columns["StockWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdGp.RootTable.Columns["PoAccessWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdGp.RootTable.Columns["VehicleType"].Width = Constants.InventoryConstants.GpType;
			grdGp.RootTable.Columns["FVStatus"].Caption = "F.V Status";
			grdGp.RootTable.Columns["NetPaid"].Width = Constants.InventoryConstants.AmountConstant;
			grdGp.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateConstant;
			grdGp.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdGp.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateConstant;
			grdGp.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants;
			grdGp.RootTable.Columns["FreightOnCash"].Width = 65;
			GridEX_Helper.AddButton(grdGp, "Load", "Load", 50, 0);
			grdGp.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillCountFromGpGrid()
	{
		try
		{
			GridEXRow[] rows = grdGp.GetRows();
			int acceptedCount = rows.Count((GridEXRow row) => Conversion.ToString(row.Cells["Status"].Value) == "Accepted");
			int openCount = rows.Count((GridEXRow row) => Conversion.ToString(row.Cells["Status"].Value) == "Open");
			txtAcceptedGpTotal.Text = acceptedCount.ToString();
			txtOpenGpTotal.Text = openCount.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridAvgRateByItem_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdGp.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "GpSrNo")
			{
				if (Conversion.ToInt(r.Cells["DetailIdsCount"].Value) > 0)
				{
					CommonServices.GatePassInwardWithDetailAndLabReport(Conversion.ToInt(r.Cells["Id"].Value));
				}
				else
				{
					CommonServices.GatePassInwardSlipAndRegisterReport(Conversion.ToInt(r.Cells["Id"].Value));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdGp, dtGrid);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGp_FormattingRow(object sender, RowLoadEventArgs e)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		try
		{
			if (grdGp.RowCount <= 0)
			{
				return;
			}
			GridEXColumn AccessWeight = grdGp.RootTable.Columns["PoAccessWeight"];
			if (AccessWeight != null)
			{
				GridEXCell AccessWeightCell = e.Row.Cells[((JanusCollectionElement)AccessWeight).Index];
				if (Conversion.ToDouble(AccessWeightCell.Value) == 0.0)
				{
					AccessWeightCell.FormatStyle = new GridEXFormatStyle();
					AccessWeightCell.FormatStyle.ForeColor = Color.Green;
				}
				else if (Conversion.ToDouble(AccessWeightCell.Value) > 0.0)
				{
					AccessWeightCell.FormatStyle = new GridEXFormatStyle();
					AccessWeightCell.FormatStyle.ForeColor = Color.Red;
					AccessWeightCell.FormatStyle.FontBold = (TriState)2;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGp_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdGp.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "Load")
			{
				LoadGPByRow(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadPendingGatePassForGrn_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdGp.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdGp.CurrentColumn == null || !e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			if (grdGp.CurrentColumn.Key == "Load")
			{
				LoadGPByRow(r);
			}
			else if (grdGp.CurrentColumn.Key == "GpSrNo")
			{
				if (Conversion.ToInt(r.Cells["DetailIdsCount"].Value) > 0)
				{
					CommonServices.GatePassInwardWithDetailAndLabReport(Conversion.ToInt(r.Cells["Id"].Value));
				}
				else
				{
					CommonServices.GatePassInwardSlipAndRegisterReport(Conversion.ToInt(r.Cells["Id"].Value));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void LoadGPByRow(GridEXRow r)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GatePassLoadedRow = null;
			if (r != null && (int)r.RowType == 0)
			{
				if (Conversion.ToString(r.Cells["Status"].Value) != "Accepted")
				{
					throw new Exception("Status Not Accepted Please check status");
				}
				if (!LabCompulsoryNotCheckingOnGRN && Conversion.ToInt(r.Cells["LastLabId"].Value) <= 0)
				{
					throw new Exception("Lab is pending for this gate pass. Please do lab first then Load");
				}
				GatePassLoadedRow = r;
				Hide();
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Expected O, but got Unknown
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Expected O, but got Unknown
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Expected O, but got Unknown
		GridEXLayout grdGp_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Purchase.LoadPendingGatePassForGrn));
		this.panel1 = new System.Windows.Forms.Panel();
		this.grdGp = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.label4 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.txtAcceptedGpTotal = new System.Windows.Forms.Label();
		this.txtOpenGpTotal = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGp).BeginInit();
		this.panel4.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdGp);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(800, 450);
		this.panel1.TabIndex = 0;
		this.grdGp.AllowEdit = (InheritableBoolean)2;
		this.grdGp.AlternatingColors = true;
		this.grdGp.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdGp_DesignTimeLayout.LayoutString = resources.GetString("grdGp_DesignTimeLayout.LayoutString");
		this.grdGp.DesignTimeLayout = grdGp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGp).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdGp.DynamicFiltering = true;
		this.grdGp.FilterMode = (FilterMode)2;
		this.grdGp.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdGp).Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.grdGp.GroupTotals = (GroupTotals)2;
		this.grdGp.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grdGp).Location = new System.Drawing.Point(0, 40);
		((System.Windows.Forms.Control)(object)this.grdGp).Name = "grdGp";
		((ScrollableBase)this.grdGp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGp).Size = new System.Drawing.Size(800, 410);
		((System.Windows.Forms.Control)(object)this.grdGp).TabIndex = 54;
		this.grdGp.TotalRow = (InheritableBoolean)1;
		this.grdGp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGp).VisualStyle = (VisualStyle)4;
		this.grdGp.FormattingRow += new RowLoadEventHandler(grdGp_FormattingRow);
		this.grdGp.ColumnButtonClick += new ColumnActionEventHandler(grdGp_ColumnButtonClick);
		this.grdGp.LinkClicked += new ColumnActionEventHandler(GridAvgRateByItem_LinkClicked);
		this.panel4.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel4.Controls.Add(this.label47);
		this.panel4.Controls.Add(this.label33);
		this.panel4.Controls.Add(this.txtAcceptedGpTotal);
		this.panel4.Controls.Add(this.txtOpenGpTotal);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(800, 40);
		this.panel4.TabIndex = 53;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(758, 7);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdGp;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(37, 23);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnPrint });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(800, 25);
		this.toolStrip1.TabIndex = 52;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStrip1.Visible = false;
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(174, 22);
		this.btnPrint.Text = "221-&Avg Rate By Item";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(3, 4);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(539, 34);
		this.label4.TabIndex = 25;
		this.label4.Text = "This Grid show Pending Gatepases For Grn Entry,User can Load GP With Accepted Status for Entry,GP With Open Status Will Not Load";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label47.ForeColor = System.Drawing.Color.Blue;
		this.label47.Location = new System.Drawing.Point(605, 2);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(95, 19);
		this.label47.TabIndex = 26;
		this.label47.Text = "Accepted Gp";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label33.ForeColor = System.Drawing.Color.Blue;
		this.label33.Location = new System.Drawing.Point(524, 2);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(72, 19);
		this.label33.TabIndex = 27;
		this.label33.Text = "Open  Gp";
		this.txtAcceptedGpTotal.AutoSize = true;
		this.txtAcceptedGpTotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAcceptedGpTotal.ForeColor = System.Drawing.Color.White;
		this.txtAcceptedGpTotal.Location = new System.Drawing.Point(630, 19);
		this.txtAcceptedGpTotal.Name = "txtAcceptedGpTotal";
		this.txtAcceptedGpTotal.Size = new System.Drawing.Size(15, 16);
		this.txtAcceptedGpTotal.TabIndex = 28;
		this.txtAcceptedGpTotal.Text = "0";
		this.txtAcceptedGpTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtOpenGpTotal.AutoSize = true;
		this.txtOpenGpTotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOpenGpTotal.ForeColor = System.Drawing.Color.White;
		this.txtOpenGpTotal.Location = new System.Drawing.Point(546, 19);
		this.txtOpenGpTotal.Name = "txtOpenGpTotal";
		this.txtOpenGpTotal.Size = new System.Drawing.Size(15, 16);
		this.txtOpenGpTotal.TabIndex = 29;
		this.txtOpenGpTotal.Text = "0";
		this.txtOpenGpTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(800, 450);
		base.Controls.Add(this.panel1);
		base.Name = "LoadPendingGatePassForGrn";
		this.Text = "Pending Gate Pass For Grn";
		base.Load += new System.EventHandler(AvgRateByItemPopUp_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(LoadPendingGatePassForGrn_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGp).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
