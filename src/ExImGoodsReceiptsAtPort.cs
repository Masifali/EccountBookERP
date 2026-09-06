// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
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
namespace Architecture.WinApp.Export
{

public class ExImGoodsReceiptsAtPort : Form
{
	private Rightsobjects formrights = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private GetForwardingDataForGoodsReceiptsAsPort LoadData;

	private int[] AryColWidth;

	private DataTable dtDetail = new DataTable();

	private DataTable dthistory = new DataTable();

	private bool UpdateMode;

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private ToolStrip toolStrip4;

	private ToolStripButton btnNew;

	private ToolStripButton btnsave;

	private ToolStripButton btnupdate;

	private ToolStripButton btnLoaderForm;

	private Panel panel1;

	private Label label23;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStrip toolStrip1;

	private Panel panel2;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label1;

	internal GridEX grdHistory;

	private ToolStripButton PrintRegister;

	public ExImGoodsReceiptsAtPort(UserAccount PUser)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = PUser;
	}

	private void ExImGoodsReceiptsAtPort_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1100, 700);
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formrights.DoHaveSaveRight;
			btnupdate.Enabled = formrights.DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			ctrlGrdBar2.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			BindHistory();
			dtDetail.Columns.Add("Id", typeof(int));
			dtDetail.Columns.Add("ForwardingId", typeof(int));
			dtDetail.Columns.Add("ForwardingDetailId", typeof(int));
			dtDetail.Columns.Add("ForwardingNo", typeof(int));
			dtDetail.Columns.Add("ForwardingDate", typeof(DateTime));
			dtDetail.Columns.Add("CustomerName", typeof(string));
			dtDetail.Columns.Add("GpNo", typeof(int));
			dtDetail.Columns.Add("GpDate", typeof(DateTime));
			dtDetail.Columns.Add("VehicleNo", typeof(string));
			dtDetail.Columns.Add("BiltyNo", typeof(string));
			dtDetail.Columns.Add("WarehouseId", typeof(int));
			dtDetail.Columns.Add("Warehouse", typeof(string));
			dtDetail.Columns.Add("PackEquivalent", typeof(double));
			dtDetail.Columns.Add("QtyReceived", typeof(double));
			dtDetail.Columns.Add("BalQty", typeof(double));
			dtDetail.Columns.Add("WeightReceived", typeof(double));
			dtDetail.Columns.Add("BalWeight", typeof(double));
			dtDetail.Columns.Add("RemarksDetail", typeof(string));
			grd.DataSource = dtDetail;
			grd.RetrieveStructure();
			grdSetting();
			dthistory.Columns.Add("Id", typeof(int));
			dthistory.Columns.Add("SortNo", typeof(int));
			dthistory.Columns.Add("ForwardingId", typeof(int));
			dthistory.Columns.Add("ForwardingDetailId", typeof(int));
			dthistory.Columns.Add("DocNo", typeof(int));
			dthistory.Columns.Add("DocDate", typeof(DateTime));
			dthistory.Columns.Add("CustomerName", typeof(string));
			dthistory.Columns.Add("GpNo", typeof(int));
			dthistory.Columns.Add("GpDate", typeof(DateTime));
			dthistory.Columns.Add("VehicleNo", typeof(string));
			dthistory.Columns.Add("BiltyNo", typeof(string));
			dthistory.Columns.Add("WarehouseId", typeof(int));
			dthistory.Columns.Add("Warehouse", typeof(string));
			dthistory.Columns.Add("QtyReceived", typeof(double));
			dthistory.Columns.Add("WeightReceived", typeof(double));
			dthistory.Columns.Add("Remarks", typeof(string));
			dthistory.Columns.Add("EntryUser", typeof(string));
			dthistory.Columns.Add("EntryDate", typeof(DateTime));
			dthistory.Columns.Add("ModifyUser", typeof(string));
			dthistory.Columns.Add("ModifyDate", typeof(DateTime));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdSetting()
	{
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["ForwardingId"].Visible = false;
			grd.RootTable.Columns["ForwardingDetailId"].Visible = false;
			grd.RootTable.Columns["WarehouseId"].Visible = false;
			grd.RootTable.Columns["PackEquivalent"].Visible = false;
			grd.RootTable.Columns["BalQty"].Visible = false;
			grd.RootTable.Columns["BalWeight"].Visible = false;
			grd.RootTable.Columns["Id"].EditType = (EditType)0;
			grd.RootTable.Columns["ForwardingId"].EditType = (EditType)0;
			grd.RootTable.Columns["ForwardingDetailId"].EditType = (EditType)0;
			grd.RootTable.Columns["ForwardingNo"].EditType = (EditType)0;
			grd.RootTable.Columns["ForwardingDate"].EditType = (EditType)0;
			grd.RootTable.Columns["CustomerName"].EditType = (EditType)0;
			grd.RootTable.Columns["GpNo"].EditType = (EditType)0;
			grd.RootTable.Columns["GpDate"].EditType = (EditType)0;
			grd.RootTable.Columns["VehicleNo"].EditType = (EditType)0;
			grd.RootTable.Columns["BiltyNo"].EditType = (EditType)0;
			grd.RootTable.Columns["WarehouseId"].EditType = (EditType)0;
			grd.RootTable.Columns["Warehouse"].EditType = (EditType)0;
			grd.RootTable.Columns["PackEquivalent"].EditType = (EditType)0;
			grd.RootTable.Columns["BalQty"].EditType = (EditType)0;
			grd.RootTable.Columns["BalWeight"].EditType = (EditType)0;
			CommonServices.GridAutoAdjustment(AryColWidth, grd);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnLoaderForm_Click(object sender, EventArgs e)
	{
		try
		{
			LoadData = new GetForwardingDataForGoodsReceiptsAsPort(UserAccount);
			LoadData.ShowDialog();
			LoadData.BringToFront();
			LoadDataFromLoader();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void LoadDataFromLoader()
	{
		try
		{
			DataTable dt = LoadData.dtSupply;
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			bool flag = false;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (grd.GetRows().Length != 0)
				{
					GridEXRow[] rows = grd.GetRows();
					for (int j = 0; j < rows.Length; j++)
					{
						if (Conversion.ToInt(rows[j].Cells["ForwardingDetailId"].Value) == Conversion.ToInt(dt.Rows[i]["ForwardingDetailId"]))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					dtDetail.Rows.Add(0, Conversion.ToInt(dt.Rows[i]["Id"]), Conversion.ToInt(dt.Rows[i]["ForwardingDetailId"]), Conversion.ToInt(dt.Rows[i]["DocNo"]), Conversion.ToDateTime(dt.Rows[i]["DocDate"]).ToShortDateString(), Conversion.ToString(dt.Rows[i]["CustomerName"]), Conversion.ToInt(dt.Rows[i]["GpNo"]), Conversion.ToDateTime(dt.Rows[i]["GpDate"]).ToShortDateString(), Conversion.ToString(dt.Rows[i]["VehicleNo"]), Conversion.ToString(dt.Rows[i]["BiltyNo"]), Conversion.ToInt(dt.Rows[i]["WarehouseToId"]), Conversion.ToString(dt.Rows[i]["WareHouseName"]), Conversion.ToDouble(dt.Rows[i]["NetWeight"]) / Conversion.ToDouble(dt.Rows[i]["NoOfBags"]), Conversion.ToDouble(dt.Rows[i]["NoOfBags"]), Conversion.ToDouble(dt.Rows[i]["NoOfBags"]), Conversion.ToDouble(dt.Rows[i]["NetWeight"]), Conversion.ToDouble(dt.Rows[i]["NetWeight"]), "");
				}
				flag = false;
			}
			grd.DataSource = dtDetail;
			grd.RetrieveStructure();
			grdSetting();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		try
		{
			ExImGoodsReceiptsAtPort obj = new ExImGoodsReceiptsAtPort();
			if (!UpdateMode)
			{
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
			}
			else
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				obj.Id = 1;
			}
			obj.EnteryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.EnteryUserId = UserAccount.ID;
			obj.ModifyUserId = UserAccount.ID;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchId = UserAccount.BranchesId;
			obj.ExImGoodsReceiptsAtPortsList = new List<ExImGoodsReceiptsAtPort>();
			GridEXRow[] dataRows = grd.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				ExImGoodsReceiptsAtPort vd = new ExImGoodsReceiptsAtPort();
				vd.EnteryDate = DateTime.Now;
				vd.ModifyDate = DateTime.Now;
				vd.EnteryUserId = UserAccount.ID;
				vd.ModifyUserId = UserAccount.ID;
				vd.OrganizationId = UserAccount.OrganizationId;
				vd.CompanyId = UserAccount.CompanyId;
				vd.BranchId = UserAccount.BranchesId;
				vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
				vd.ExImForwardingId = Conversion.ToInt(r.Cells["ForwardingId"].Value);
				vd.ExImForwardingPackingDetailId = Conversion.ToInt(r.Cells["ForwardingDetailId"].Value);
				vd.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
				vd.QtyReceived = Conversion.ToDecimal(r.Cells["QtyReceived"].Value);
				vd.WeightReceived = Conversion.ToDecimal(r.Cells["WeightReceived"].Value);
				vd.RemarksDetail = Conversion.ToString(r.Cells["RemarksDetail"].Value);
				if (!UpdateMode)
				{
					vd.SortNo = Conversion.ToInt(dthistory.Compute("Max(SortNo)", "")) + 1;
				}
				obj.ExImGoodsReceiptsAtPortsList.Add(vd);
			}
			ExImGoodsReceiptsAtPort.Save(obj);
			if (!UpdateMode)
			{
				MessageBox.Show("Record Saved Successfully");
			}
			else
			{
				MessageBox.Show("Record Updated Successfully");
			}
			Reset();
			BindHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		try
		{
			btnsave_Click(null, null);
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
			btnsave.Visible = true;
			btnupdate.Visible = false;
			UpdateMode = false;
			dtDetail.Rows.Clear();
			grd.DataSource = dtDetail;
			grd.RetrieveStructure();
			grdSetting();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistory).Name);
			if (fs != null)
			{
				grdHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				BindHistory();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BindHistory()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		try
		{
			dthistory.Rows.Clear();
			DataTable dt = ExImGoodsReceiptsAtPort.Getall(new ExImGoodsReceiptsAtPort
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchId = UserAccount.BranchesId
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dthistory.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["SortNo"], dt.Rows[i]["ExImForwardingId"], dt.Rows[i]["ExImForwardingPackingDetailId"], dt.Rows[i]["DocNo"], Conversion.ToDateTime(dt.Rows[i]["DocDate"]).ToShortDateString(), dt.Rows[i]["CustomerName"], dt.Rows[i]["GpNo"], Conversion.ToDateTime(dt.Rows[i]["GpDate"]).ToShortDateString(), dt.Rows[i]["VehicleNo"], dt.Rows[i]["BiltyNo"], dt.Rows[i]["WarehouseId"], dt.Rows[i]["WareHouseName"], dt.Rows[i]["QtyReceived"], dt.Rows[i]["WeightReceived"], dt.Rows[i]["RemarksDetail"], dt.Rows[i]["EntryUser"], Conversion.ToDateTime(dt.Rows[i]["EnteryDate"]).ToShortDateString(), dt.Rows[i]["ModifyUser"], Conversion.ToDateTime(dt.Rows[i]["ModifyDate"]).ToShortDateString());
				}
				grdHistory.DataSource = dthistory;
				grdHistory.RetrieveStructure();
				HistorySetting();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void HistorySetting()
	{
		try
		{
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["SortNo"].Visible = false;
			grdHistory.RootTable.Columns["ForwardingId"].Visible = false;
			grdHistory.RootTable.Columns["ForwardingDetailId"].Visible = false;
			grdHistory.RootTable.Columns["WarehouseId"].Visible = false;
			CommonServices.GridAutoAdjustment(AryColWidth, grdHistory);
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int sortNo = Conversion.ToInt(item.Cells["SortNo"].Value);
			dtDetail.Rows.Clear();
			EnumerableRowCollection<DataRow> Rows = from row in dthistory.AsEnumerable()
				where row.Field<int>("SortNo") == sortNo
				select row;
			DataTable detail = (Rows.Any() ? Rows.CopyToDataTable() : dthistory.Clone());
			if (detail.Rows.Count > 0)
			{
				tabControl1.SelectedIndex = 0;
				btnsave.Visible = false;
				btnupdate.Visible = true;
				UpdateMode = true;
				for (int i = 0; i < detail.Rows.Count; i++)
				{
					dtDetail.Rows.Add(detail.Rows[i]["Id"], detail.Rows[i]["ForwardingId"], detail.Rows[i]["ForwardingDetailId"], detail.Rows[i]["DocNo"], Conversion.ToDateTime(detail.Rows[i]["DocDate"]).ToShortDateString(), detail.Rows[i]["CustomerName"], detail.Rows[i]["GpNo"], Conversion.ToDateTime(detail.Rows[i]["GpDate"]).ToShortDateString(), detail.Rows[i]["VehicleNo"], detail.Rows[i]["BiltyNo"], detail.Rows[i]["WarehouseId"], detail.Rows[i]["Warehouse"], Conversion.ToDouble(detail.Rows[i]["WeightReceived"]) / Conversion.ToDouble(detail.Rows[i]["QtyReceived"]), detail.Rows[i]["QtyReceived"], detail.Rows[i]["QtyReceived"], detail.Rows[i]["WeightReceived"], detail.Rows[i]["WeightReceived"], detail.Rows[i]["Remarks"]);
				}
				grd.DataSource = dtDetail;
				grd.RetrieveStructure();
				grdSetting();
			}
			else
			{
				btnsave.Visible = true;
				btnupdate.Visible = false;
				grd.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow Row = grd.CurrentRow;
			double BalQty = Conversion.ToDouble(Row.Cells["BalQty"].Value);
			double QtyRcvd = Conversion.ToDouble(Row.Cells["QtyReceived"].Value);
			double BalWeight = Conversion.ToDouble(Row.Cells["BalWeight"].Value);
			double WeightRcvd = Conversion.ToDouble(Row.Cells["WeightReceived"].Value);
			double PackEquivalent = Conversion.ToDouble(Row.Cells["PackEquivalent"].Value);
			if (e.Column.Key == "QtyReceived")
			{
				if (QtyRcvd > BalQty)
				{
					Row.Cells["QtyReceived"].Value = BalQty;
					Row.Cells["WeightReceived"].Value = BalQty * PackEquivalent;
					MessageBox.Show("Qty Rcvd Can not Greater Than Balance Qty Of " + BalQty);
				}
				else
				{
					Row.Cells["WeightReceived"].Value = QtyRcvd * PackEquivalent;
				}
			}
			if (e.Column.Key == "WeightReceived")
			{
				if (WeightRcvd > BalWeight)
				{
					Row.Cells["WeightReceived"].Value = BalWeight;
					Row.Cells["QtyReceived"].Value = BalWeight / PackEquivalent;
					MessageBox.Show("Weight Rcvd Can not Greater Than Balance Weight Of " + BalWeight);
				}
				else
				{
					Row.Cells["QtyReceived"].Value = WeightRcvd / PackEquivalent;
				}
			}
			grd.UpdateData();
			CommonServices.GridAutoAdjustment(AryColWidth, grd);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PrintRegister_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dthistory == null || dthistory.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dthistory, "466-ExportGoodsReceiptsAtPortRegister.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel1 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label23 = new System.Windows.Forms.Label();
		this.toolStrip4 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnLoaderForm = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdHistory = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label1 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.PrintRegister = new System.Windows.Forms.ToolStripButton();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel1.SuspendLayout();
		this.toolStrip4.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(944, 676);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Controls.Add(this.toolStrip4);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(936, 650);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		this.grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 58);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(930, 589);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 7;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.ctrlGrdBar1);
		this.panel1.Controls.Add(this.label23);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 30);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(930, 28);
		this.panel1.TabIndex = 6;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(888, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(40, 23);
		this.ctrlGrdBar1.TabIndex = 66;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(199, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "Goods Receipts As Port";
		this.toolStrip4.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip4.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.btnsave, this.btnupdate, this.btnLoaderForm });
		this.toolStrip4.Location = new System.Drawing.Point(3, 3);
		this.toolStrip4.Name = "toolStrip4";
		this.toolStrip4.Size = new System.Drawing.Size(930, 27);
		this.toolStrip4.TabIndex = 5;
		this.toolStrip4.Text = "toolStrip4";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(60, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(65, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(79, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnLoaderForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoaderForm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoaderForm.Name = "btnLoaderForm";
		this.btnLoaderForm.Size = new System.Drawing.Size(156, 24);
		this.btnLoaderForm.Text = "&Load Forwarding Data";
		this.btnLoaderForm.Click += new System.EventHandler(btnLoaderForm_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.tabPage2.Controls.Add(this.panel2);
		this.tabPage2.Controls.Add(this.toolStrip1);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(936, 650);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdHistory).BackColor = System.Drawing.Color.White;
		this.grdHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(3, 58);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(930, 589);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 8;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.ctrlGrdBar2);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 30);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(930, 28);
		this.panel2.TabIndex = 7;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(887, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(40, 23);
		this.ctrlGrdBar2.TabIndex = 66;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(5, 4);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(262, 19);
		this.label1.TabIndex = 24;
		this.label1.Text = "Goods Receipts As Port History";
		this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.PrintRegister });
		this.toolStrip1.Location = new System.Drawing.Point(3, 3);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(930, 27);
		this.toolStrip1.TabIndex = 6;
		this.toolStrip1.Text = "toolStrip1";
		this.PrintRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.PrintRegister.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.PrintRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.PrintRegister.Name = "PrintRegister";
		this.PrintRegister.Size = new System.Drawing.Size(122, 24);
		this.PrintRegister.Text = "Register-Print";
		this.PrintRegister.Click += new System.EventHandler(PrintRegister_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(944, 676);
		base.Controls.Add(this.tabControl1);
		base.Name = "ExImGoodsReceiptsAtPort";
		this.Text = "Goods Receipts At Port";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(ExImGoodsReceiptsAtPort_Load);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip4.ResumeLayout(false);
		this.toolStrip4.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.tabPage2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
