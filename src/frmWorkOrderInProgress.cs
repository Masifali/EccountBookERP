using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.Mfg;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Mfg;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
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
namespace Architecture.WinApp.Mfg
{

public class frmWorkOrderInProgress : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private DataTable dtMain = new DataTable();

	private DataTable dtdetail = new DataTable();

	private DataTable dtHistory = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable dtuomGetAll = new DataTable();

	private int updateDetailIndex = -1;

	private int RecId;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel6;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnUpdate;

	internal GridEX grd;

	private Panel panel9;

	private Panel panel4;

	private Panel panel16;

	private Panel panel15;

	private Panel panel12;

	internal GridEX grdWorkOrderMain;

	private Panel panel13;

	private Label label14;

	private Label label12;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label18;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStripButton btnattachment;

	private TabPage tabPage3;

	private CtrlGrdBar ctrlGrdBar7;

	private ToolStrip toolStrip4;

	private ToolStripButton btnLoadAllHistoryNew;

	private Panel panel25;

	private ToolStripButton btnRefreshHistoryCombos;

	internal GridEX DatagridHistory;

	private Panel panel8;

	private Label label2;

	private Panel panel24;

	private Panel panel7;

	private Label label1;

	internal GridEX grdhistoryoperations;

	private Label label5;

	private GroupBox groupBox4;

	private Label label36;

	internal UltraCombo cmbItemUOM;

	private Label label26;

	private Label label7;

	private Label label4;

	private Label label3;

	private Label label6;

	private Button BtnCancelDetail;

	internal UltraCombo CmbItemDetail;

	private Button BtnAddDetail;

	private DateTimePicker txtCompleteDate;

	private DateTimePicker txtCompleteTime;

	private DateTimePicker txtStartDate;

	private DateTimePicker txtStartTime;

	private TextBox txtRemarksDetail;

	private Label label27;

	private TextBox txtQtyDetail;

	private TextBox txtOperation;

	private TextBox txtWorkStation;

	private Label label16;

	private Label label9;

	private Label label10;

	private Button BtnUpdateDetail;

	private GroupBox groupBox1;

	private Button btnshow;

	internal UltraCombo cmbItemhistory;

	private Label label29;

	internal UltraCombo CmbBomNoHistory;

	private Label label20;

	private Label label19;

	internal UltraCombo cmbDateTypeHistory;

	private DateTimePicker txtToDateHistory;

	private Label label28;

	private TextBox txtToDocNoHistory;

	private DateTimePicker txtFromdateHistory;

	private Label label35;

	private TextBox txtFromNoHistory;

	private Panel panel5;

	private ToolStripButton btnPrint;

	private ToolStripButton btnshortcutkeys;

	private ToolStripButton btnRefresh;

	public frmWorkOrderInProgress(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmSaleContractSchedule_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1400, 700);
			formright = CommonServices.SetRightsValueInRightsObject("frmWorkOrderInProgress");
			btnUpdate.Enabled = formright.DoHaveSaveRight;
			btnPrint.Enabled = formright.DoHavePrintRights;
			ctrlGrdBar2.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar3.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar3.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar3.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar3.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar3.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar3.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			tabControl1.TabPages.Remove(tabPage3);
			dtViewAttachments.Columns.Add("AttchmentPath");
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName", typeof(string));
			dtitem.Columns.Add("ItemCode", typeof(string));
			dtMain.Columns.Add("Id", typeof(int));
			dtMain.Columns.Add("DocumentTypeId", typeof(int));
			dtMain.Columns.Add("DocNo", typeof(string));
			dtMain.Columns.Add("DocDate", typeof(DateTime));
			dtMain.Columns.Add("WorkOrderId", typeof(int));
			dtMain.Columns.Add("WorkOrderNo", typeof(string));
			dtMain.Columns.Add("BomHeaderId", typeof(int));
			dtMain.Columns.Add("BomHeaderNo", typeof(string));
			dtMain.Columns.Add("ItemId", typeof(int));
			dtMain.Columns.Add("ItemName", typeof(string));
			dtMain.Columns.Add("ItemCode", typeof(string));
			dtMain.Columns.Add("ItemUomId", typeof(int));
			dtMain.Columns.Add("ItemUomCode", typeof(string));
			dtMain.Columns.Add("ItemQty", typeof(decimal));
			dtMain.Columns.Add("StartDate", typeof(DateTime));
			dtMain.Columns.Add("EndDate", typeof(DateTime));
			dtMain.Columns.Add("DeadLine", typeof(DateTime));
			dtMain.Columns.Add("RemarksHeader", typeof(string));
			grdWorkOrderMain.DataSource = dtMain;
			grdWorkOrderMain.RetrieveStructure();
			grdWorkorderMainSettings();
			dtdetail.Columns.Add("Id", typeof(int));
			dtdetail.Columns.Add("WorkOrderOperationId", typeof(int));
			dtdetail.Columns.Add("ItemId", typeof(int));
			dtdetail.Columns.Add("ItemName", typeof(string));
			dtdetail.Columns.Add("UOMId", typeof(int));
			dtdetail.Columns.Add("UOM", typeof(string));
			dtdetail.Columns.Add("Qty", typeof(decimal));
			dtdetail.Columns.Add("WorkStationOperationId", typeof(int));
			dtdetail.Columns.Add("WorkStationName", typeof(string));
			dtdetail.Columns.Add("OperationName", typeof(string));
			dtdetail.Columns.Add("StartDate", typeof(DateTime));
			dtdetail.Columns.Add("StartTime", typeof(DateTime));
			dtdetail.Columns.Add("CompleteDate", typeof(DateTime));
			dtdetail.Columns.Add("CompleteTime", typeof(DateTime));
			dtdetail.Columns.Add("Remarks", typeof(string));
			grd.DataSource = dtdetail;
			grd.RetrieveStructure();
			GridDetailSetting();
			ItemFill();
			uomgetAll();
			FillgrdWorkOrderMain();
			Datetypefill();
			AllCombobind();
			((Control)(object)grdWorkOrderMain).Focus();
			BtnAddDetail.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ItemFill()
	{
		try
		{
			int DetailId = Conversion.ToInt(CmbItemDetail.Value);
			DataTable dt = CommonServices.ItemGetAllServiceBind();
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtitem = new DataTable();
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName");
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				dtitem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ItemName"]);
			}
			if (dtitem.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtitem, CmbItemDetail, "Id", "ItemName", "Item Name", ZeroIndex: true);
			((UltraGridBase)CmbItemDetail).DisplayLayout.Bands[0].Columns[1].Width = 350;
			if (DetailId > 0)
			{
				if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == DetailId))
				{
					CmbItemDetail.Value = DetailId;
				}
				else
				{
					((Control)(object)CmbItemDetail).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void uomgetAll()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			UOMSchedule obj = new UOMSchedule();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtuomGetAll = UOMSchedule.Getall(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WorkStationOpertaionByWSOId(int Id)
	{
		try
		{
			txtWorkStation.Text = string.Empty;
			txtOperation.Text = string.Empty;
			DataTable dt = WorkStationOperation.GetWorkStationOpertaionByWSOId(UserAccount.OrganizationId, UserAccount.CompanyId, Id);
			if (dt.Rows.Count > 0)
			{
				txtOperation.Text = Conversion.ToString(dt.Rows[0]["OperationName"]);
				txtWorkStation.Text = Conversion.ToString(dt.Rows[0]["WorkStationName"]);
			}
			else
			{
				txtOperation.Text = "";
				txtWorkStation.Text = "";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbItemDetail_Leave(object sender, EventArgs e)
	{
		try
		{
			int UomId = Conversion.ToInt(cmbItemUOM.Value);
			((Control)(object)cmbItemUOM).Text = string.Empty;
			DataTable dtUomDetail = new DataTable();
			dtUomDetail.Columns.Add("Id", typeof(int));
			dtUomDetail.Columns.Add("ItemUOM");
			for (int i = 0; i < dtuomGetAll.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtuomGetAll.Rows[i]["ItemId"]) == Conversion.ToInt(CmbItemDetail.Value))
				{
					dtUomDetail.Rows.Add(dtuomGetAll.Rows[i]["Id"], dtuomGetAll.Rows[i]["UOMDescription"]);
				}
			}
			if (dtUomDetail.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtUomDetail, cmbItemUOM, "Id", "ItemUOM", "UOM", ZeroIndex: false);
			((UltraGridBase)cmbItemUOM).DisplayLayout.Bands[0].Columns[1].Width = 193;
			if (UomId > 0)
			{
				if (dtUomDetail.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == UomId))
				{
					cmbItemUOM.Value = UomId;
				}
				else
				{
					((Control)(object)cmbItemUOM).Text = string.Empty;
				}
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
			FormReset();
			DetailFormReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void FormReset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			dtAttachmentsUpload.Clear();
			RecId = 0;
			updateDetailIndex = -1;
			dtMain.Clear();
			dtdetail.Clear();
			btnUpdate.Visible = true;
			grdWorkOrderMain.DataSource = null;
			FillgrdWorkOrderMain();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DetailFormReset()
	{
		try
		{
			((Control)(object)CmbItemDetail).Text = string.Empty;
			((Control)(object)cmbItemUOM).Text = string.Empty;
			txtQtyDetail.Text = string.Empty;
			txtWorkStation.Text = string.Empty;
			txtOperation.Text = string.Empty;
			BtnUpdateDetail.Visible = false;
			BtnCancelDetail.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			ItemFill();
			uomgetAll();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmWorkOrderInProgress_FormClosed(object sender, FormClosedEventArgs e)
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

	private void FillgrdWorkOrderMain()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		try
		{
			DataTable dtGrid = WorkOrderInProgress.GetWorkOrderInProgressData(new ReportsParameters
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (dtGrid.Rows.Count <= 0)
			{
				return;
			}
			dtMain.Clear();
			foreach (DataRow row in dtGrid.Rows)
			{
				dtMain.Rows.Add(row["Id"], row["DocumentTypeId"], row["DocNo"], row["DocDate"], row["WorkOrderId"], row["WorkOrderNo"], row["BomHeaderId"], row["BomHeaderNo"], row["ItemId"], row["ItemName"], row["ItemCode"], row["ItemUomId"], row["ItemUomCode"], row["ItemQty"], row["StartDate"], row["EndDate"], row["DeadLine"], row["RemarksHeader"]);
			}
			grdWorkOrderMain.DataSource = dtMain;
			grdWorkOrderMain.RetrieveStructure();
			grdWorkorderMainSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdWorkorderMainSettings()
	{
		try
		{
			grdWorkOrderMain.RootTable.Columns["Id"].Visible = false;
			grdWorkOrderMain.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdWorkOrderMain.RootTable.Columns["WorkOrderId"].Visible = false;
			grdWorkOrderMain.RootTable.Columns["BomHeaderId"].Visible = false;
			grdWorkOrderMain.RootTable.Columns["ItemId"].Visible = false;
			grdWorkOrderMain.RootTable.Columns["ItemUomId"].Visible = false;
			grdWorkOrderMain.RootTable.Columns["WorkOrderNo"].ColumnType = (ColumnType)5;
			grdWorkOrderMain.RootTable.Columns["Id"].EditType = (EditType)0;
			grdWorkOrderMain.RootTable.Columns["DocumentTypeId"].EditType = (EditType)0;
			grdWorkOrderMain.RootTable.Columns["DocNo"].EditType = (EditType)0;
			grdWorkOrderMain.RootTable.Columns["DocDate"].EditType = (EditType)0;
			grdWorkOrderMain.RootTable.Columns["WorkOrderId"].EditType = (EditType)0;
			grdWorkOrderMain.RootTable.Columns["WorkOrderNo"].EditType = (EditType)0;
			grdWorkOrderMain.RootTable.Columns["BomHeaderId"].EditType = (EditType)0;
			grdWorkOrderMain.RootTable.Columns["BomHeaderNo"].EditType = (EditType)0;
			grdWorkOrderMain.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdWorkOrderMain.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdWorkOrderMain.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grdWorkOrderMain.RootTable.Columns["ItemUomId"].EditType = (EditType)0;
			grdWorkOrderMain.RootTable.Columns["ItemUomCode"].EditType = (EditType)0;
			grdWorkOrderMain.RootTable.Columns["ItemQty"].EditType = (EditType)0;
			grdWorkOrderMain.RootTable.Columns["StartDate"].EditType = (EditType)2;
			grdWorkOrderMain.RootTable.Columns["EndDate"].EditType = (EditType)2;
			grdWorkOrderMain.RootTable.Columns["DeadLine"].EditType = (EditType)0;
			grdWorkOrderMain.RootTable.Columns["RemarksHeader"].EditType = (EditType)1;
			grdWorkOrderMain.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdWorkOrderMain.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdWorkOrderMain.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grdWorkOrderMain.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(grdWorkOrderMain);
			grdWorkOrderMain.RootTable.Columns["RemarksHeader"].Width = 200;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ChangeGridRowColor(GridEX grd, Color color)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		try
		{
			new GridEXColumn();
			GridEXFormatCondition fc = new GridEXFormatCondition(grd.RootTable.Columns["Id"], (ConditionOperator)0, (object)RecId);
			fc.FormatStyle.FontBold = (TriState)2;
			fc.FormatStyle.ForeColor = color;
			grd.RootTable.FormatConditions.Add(fc);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdWorkOrderMain_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdWorkOrderMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "Delete")
			{
				if (dtMain.Rows.Count <= 1)
				{
					MessageBox.Show("You Can Not Delete All rows");
				}
				else
				{
					grdWorkOrderMain.UpdateData();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdWorkOrderMain_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdWorkOrderMain.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdWorkOrderMain_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdWorkOrderMain.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0 || !(e.Column.Key == "WorkOrderNo"))
			{
				return;
			}
			if (grd.GetRows().Count() > 0)
			{
				if (MessageBox.Show("Are you sure to Reload Data For Detail?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				dtdetail.Rows.Clear();
			}
			ReadById(Conversion.ToInt(r.Cells["Id"].Value));
			ChangeGridRowColor(grdWorkOrderMain, Color.DarkGreen);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnAddDetail_Click(object sender, EventArgs e)
	{
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				updateDetailIndex = item.RowIndex;
				CmbItemDetail.Value = Conversion.ToInt(item.Cells["ItemId"].Value);
				CmbItemDetail_Leave(null, null);
				cmbItemUOM.Value = Conversion.ToInt(item.Cells["UOMId"].Value);
				txtQtyDetail.Text = Conversion.ToDecimal(item.Cells["Qty"].Value).ToString("#,##.###");
				txtOperation.Text = Conversion.ToString(item.Cells["OperationName"].Value);
				txtWorkStation.Text = Conversion.ToString(item.Cells["WorkStationName"].Value);
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(item.Cells["StartDate"].Value)))
				{
					txtStartDate.Value = Conversion.ToDateTime(item.Cells["StartDate"].Value);
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(item.Cells["StartTime"].Value)))
				{
					txtStartTime.Value = Conversion.ToDateTime(item.Cells["StartTime"].Value);
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(item.Cells["CompleteDate"].Value)))
				{
					txtCompleteDate.Value = Conversion.ToDateTime(item.Cells["CompleteDate"].Value);
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(item.Cells["CompleteTime"].Value)))
				{
					txtCompleteTime.Value = Conversion.ToDateTime(item.Cells["CompleteTime"].Value);
				}
				txtRemarksDetail.Text = Conversion.ToString(item.Cells["Remarks"].Value);
				BtnAddDetail.Visible = false;
				BtnUpdateDetail.Visible = true;
				BtnCancelDetail.Visible = true;
				txtStartDate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			dtdetail.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(CmbItemDetail.Value);
			dtdetail.Rows[updateDetailIndex]["UOMId"] = Conversion.ToInt(cmbItemUOM.Value);
			dtdetail.Rows[updateDetailIndex]["Qty"] = Conversion.ToDecimal((object)txtQtyDetail.Text);
			dtdetail.Rows[updateDetailIndex]["WorkStationName"] = Conversion.ToString((object)txtWorkStation.Text);
			dtdetail.Rows[updateDetailIndex]["OperationName"] = Conversion.ToString((object)txtOperation.Text);
			dtdetail.Rows[updateDetailIndex]["StartDate"] = Conversion.ToDateTime((object)txtStartDate.Value);
			dtdetail.Rows[updateDetailIndex]["StartTime"] = Conversion.ToDateTime((object)txtStartTime.Value);
			dtdetail.Rows[updateDetailIndex]["CompleteDate"] = Conversion.ToDateTime((object)txtCompleteDate.Value);
			dtdetail.Rows[updateDetailIndex]["CompleteTime"] = Conversion.ToDateTime((object)txtCompleteTime.Value);
			dtdetail.Rows[updateDetailIndex]["Remarks"] = Conversion.ToString((object)txtRemarksDetail.Text);
			grd.DataSource = dtdetail;
			DetailFormReset();
			txtStartDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnCancelDetail_Click(object sender, EventArgs e)
	{
		try
		{
			DetailFormReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailSetting()
	{
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["UOMId"].Visible = false;
			grd.RootTable.Columns["WorkOrderOperationId"].Visible = false;
			grd.RootTable.Columns["WorkStationOperationId"].Visible = false;
			grd.RootTable.Columns["StartTime"].FormatString = "HH:tt:ss";
			grd.RootTable.Columns["CompleteTime"].FormatString = "HH:tt:ss";
			grd.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Qty"].FormatString = "#,##0";
			grd.RootTable.Columns["Qty"].TotalFormatString = "#,##0";
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns["Remarks"].Width = 200;
			grd.RootTable.Columns.Add("Edit");
			grd.RootTable.Columns["Edit"].Key = "Edit";
			grd.RootTable.Columns["Edit"].Caption = "Edit";
			grd.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Edit"].ButtonText = "Edit";
			grd.RootTable.Columns["Edit"].Width = 50;
			grd.RootTable.Columns["Edit"].Position = 0;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) <= 0 && r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				grd.UpdateData();
			}
			if (e.Column.Key == "Edit")
			{
				grd_DoubleClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnAddDetail_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "rdbtnItemName")
			{
				txtStartDate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void BtnCancelDetail_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "rdbtnItemName")
			{
				txtStartDate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				MessageBox.Show("Please Select Any Record first...");
			}
			else
			{
				Insert();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Insert()
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		try
		{
			if (grdWorkOrderMain.GetRows().Count() == 0)
			{
				MessageBox.Show("Grid WorkOrderMain Record Not Found...\nGrid Should have atleast one record");
				return;
			}
			if (grd.GetRows().Count() == 0)
			{
				MessageBox.Show("Grid Record Not Found...\nGrid Should have atleast one record");
				return;
			}
			WorkOrderInProgress Or = new WorkOrderInProgress();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				Or.WorkOrderInProgressId = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			Or.CompanyId = UserAccount.CompanyId;
			Or.BranchesId = UserAccount.BranchesId;
			Or.ProjectsId = UserAccount.BranchesId;
			Or.OrganizationId = UserAccount.OrganizationId;
			Or.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			Or.DocumentTypeId = 1620;
			Or.EntryUserId = UserAccount.ID;
			Or.EntryDate = DateTime.Now;
			Or.ModifyDate = DateTime.Now;
			Or.ModifyUserId = UserAccount.ID;
			Or.ApprovedUserId = UserAccount.ID;
			Or.ApprovedDate = DateTime.Now;
			Or.IsApproved = false;
			GridEXRow r3 = grdWorkOrderMain.GetRows().FirstOrDefault((GridEXRow r) => r.Cells["Id"].Value != null && r.Cells["Id"].Value.ToString() == RecId.ToString());
			if (r3 != null && (int)r3.RowType == 0 && r3.Cells.Count > 0)
			{
				Or.DocDate = Conversion.ToDateTime(r3.Cells["DocDate"].Value);
				Or.DocNo = Conversion.ToInt((object)r3.Cells["DocNo"].Text);
				Or.WorkOrderId = Conversion.ToInt(r3.Cells["WorkOrderId"].Value);
				Or.StartDate = Conversion.ToDateTime(r3.Cells["StartDate"].Value);
				Or.EndDate = Conversion.ToDateTime(r3.Cells["EndDate"].Value);
				Or.RemarksHeader = Conversion.ToString((object)r3.Cells["RemarksHeader"].Text);
			}
			Or.WorkOrderInProgressDetailList = new List<WorkOrderInProgressDetail>();
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToInt((object)r2.Cells["WorkStationOperationId"].Value.ToString()) != 0)
				{
					WorkOrderInProgressDetail d1 = new WorkOrderInProgressDetail();
					d1.WorkOrderInProgressDetailId = Conversion.ToInt(r2.Cells["Id"].Value);
					if (d1.WorkOrderInProgressDetailId > 0)
					{
						d1.ActionTypeId = 2;
					}
					else
					{
						d1.ActionTypeId = 1;
					}
					d1.WorkOrderOperationId = Conversion.ToInt(r2.Cells["WorkOrderOperationId"].Value);
					d1.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
					d1.ItemUomId = Conversion.ToInt(r2.Cells["UOMId"].Value);
					d1.Qty = Conversion.ToDecimal((object)r2.Cells["Qty"].Text);
					if (r2.Cells["WorkStationOperationId"].Text == string.Empty)
					{
						MessageBox.Show("WorkStationOperation Field Required in Grid Operations Row No: " + (r2.RowIndex + 1));
						return;
					}
					d1.WorkStationOperationId = Conversion.ToInt(r2.Cells["WorkStationOperationId"].Value);
					if (Conversion.CheckDateTimeNull(Conversion.ToDateTime(r2.Cells["StartDate"].Value)))
					{
						MessageBox.Show("StartDate can not be null in Grid Operations Row No: " + (r2.RowIndex + 1));
						return;
					}
					d1.WsOpStartDate = Conversion.ToDateTime(r2.Cells["StartDate"].Value);
					d1.WsOpStartTime = Conversion.ToDateTime(r2.Cells["StartTime"].Value);
					if (Conversion.CheckDateTimeNull(Conversion.ToDateTime(r2.Cells["CompleteDate"].Value)))
					{
						MessageBox.Show("CompleteDate can not be null in Grid Operations Row No: " + (r2.RowIndex + 1));
						return;
					}
					d1.WsOpCompleteDate = Conversion.ToDateTime(r2.Cells["CompleteDate"].Value);
					d1.WsOpCompleteTime = Conversion.ToDateTime(r2.Cells["CompleteTime"].Value);
					d1.Remarks = Conversion.ToString(r2.Cells["Remarks"].Value);
					Or.WorkOrderInProgressDetailList.Add(d1);
				}
			}
			Or.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			Or.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<WorkOrderInProgress>(Or, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = WorkOrderInProgress.Save(Or);
			Or.WorkOrderInProgressId = success;
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + Or.DocNo);
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 1620, base.Name);
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + Or.DocNo);
			}
			FormReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int Id)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		try
		{
			if (Id > 0)
			{
				WorkOrderInProgress Or = new WorkOrderInProgress();
				RecId = Id;
				Or = WorkOrderInProgress.ReadById(RecId);
				if (Or != null && Or.WorkOrderInProgressDetailList.Count > 0)
				{
					tabControl1.SelectedIndex = 0;
					FillgrdWorkOrderMain();
					dtdetail.Clear();
					for (int i = 0; i < Or.WorkOrderInProgressDetailList.Count; i++)
					{
						dtdetail.Rows.Add(Or.WorkOrderInProgressDetailList[i].WorkOrderInProgressDetailId, Or.WorkOrderInProgressDetailList[i].WorkOrderOperationId, Or.WorkOrderInProgressDetailList[i].ItemId, Or.WorkOrderInProgressDetailList[i].ItemName, Or.WorkOrderInProgressDetailList[i].ItemUomId, Or.WorkOrderInProgressDetailList[i].PackUom, Or.WorkOrderInProgressDetailList[i].Qty, Or.WorkOrderInProgressDetailList[i].WorkStationOperationId, Or.WorkOrderInProgressDetailList[i].WorkStationName, Or.WorkOrderInProgressDetailList[i].OperationName, Or.WorkOrderInProgressDetailList[i].WsOpStartDate, Or.WorkOrderInProgressDetailList[i].WsOpStartTime, Or.WorkOrderInProgressDetailList[i].WsOpCompleteDate, Or.WorkOrderInProgressDetailList[i].WsOpCompleteTime, Or.WorkOrderInProgressDetailList[i].Remarks);
					}
					grd.DataSource = dtdetail;
					grd.RetrieveStructure();
					GridDetailSetting();
					FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
					btnUpdate.Visible = true;
				}
				else
				{
					grd.ClearStructure();
				}
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

	private void AllCombobind()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		try
		{
			int Idname = Conversion.ToInt(CmbBomNoHistory.Value);
			int Iditem = Conversion.ToInt(cmbItemhistory.Value);
			DataTable dtcombo = WorkOrder.GetDataForDropDownFromWorkOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtcombo.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtBOMName = new DataTable();
			dtBOMName.Columns.Add("Id", typeof(int));
			dtBOMName.Columns.Add("BOMName");
			DataTable dtitem = new DataTable();
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName");
			for (int i = 0; i < dtcombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "BOMName")
				{
					dtBOMName.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ItemHeader")
				{
					dtitem.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
			}
			if (dtBOMName.Rows.Count > 0)
			{
				DDL.BindDDL(dtBOMName, CmbBomNoHistory, "Id", "BOMName", "BOM Name", ZeroIndex: true);
				((UltraGridBase)CmbBomNoHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (Idname > 0)
				{
					if (dtBOMName.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Idname))
					{
						CmbBomNoHistory.Value = Idname;
					}
					else
					{
						((Control)(object)CmbBomNoHistory).Text = string.Empty;
					}
				}
			}
			if (dtitem.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtitem, cmbItemhistory, "Id", "ItemName", "Item Name", ZeroIndex: true);
			((UltraGridBase)cmbItemhistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (Iditem > 0)
			{
				if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Iditem))
				{
					cmbItemhistory.Value = Iditem;
				}
				else
				{
					((Control)(object)cmbItemhistory).Text = string.Empty;
				}
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
			DDL.BindDDL(dt, cmbDateTypeHistory, "Id", "Parameters", "Parameters", ZeroIndex: true);
			((UltraGridBase)cmbDateTypeHistory).Rows[2].Activate();
			((UltraGridBase)cmbDateTypeHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			cmbDateTypeHistory.Focus();
		}
		else
		{
			((Control)(object)grdWorkOrderMain).Focus();
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridFill()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dtHistory.Clear();
			ReportsParameters header = new ReportsParameters();
			header.CompanyId = UserAccount.CompanyId;
			header.OrganizationId = UserAccount.OrganizationId;
			header.BranchesId = UserAccount.BranchesId;
			header.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			header.FromDocNo = Conversion.ToInt((object)txtFromNoHistory.Text.Trim());
			header.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text.Trim());
			header.FromDate = Conversion.ToDateTime((object)txtFromdateHistory.Value);
			header.ToDate = Conversion.ToDateTime((object)txtToDateHistory.Value);
			header.Id = Conversion.ToInt(CmbBomNoHistory.Value);
			header.ItemId = Conversion.ToInt(cmbItemhistory.Value);
			header.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			dtHistory = WorkOrderInProgress.FormHistory(header);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("RecordNo", typeof(int));
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("DocumentTypeId", typeof(int));
				dtcol.Columns.Add("DocNo", typeof(string));
				dtcol.Columns.Add("DocDate", typeof(DateTime));
				dtcol.Columns.Add("WorkOrderNo", typeof(string));
				dtcol.Columns.Add("BomNo", typeof(string));
				dtcol.Columns.Add("ItemName", typeof(string));
				dtcol.Columns.Add("Uom", typeof(string));
				dtcol.Columns.Add("ItemQty", typeof(decimal));
				dtcol.Columns.Add("StartDate", typeof(DateTime));
				dtcol.Columns.Add("EndDate", typeof(DateTime));
				dtcol.Columns.Add("DeadLine", typeof(string));
				dtcol.Columns.Add("BatchDesc", typeof(string));
				dtcol.Columns.Add("RemarksHeader", typeof(string));
				dtcol.Columns.Add("EntryUserName", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("ModifyUserName", typeof(string));
				dtcol.Columns.Add("ModifyDate", typeof(DateTime));
				dtcol.Columns.Add("IsApproved", typeof(bool));
				dtcol.Columns.Add("NoOfAttachments", typeof(int));
				dtcol.Rows.Clear();
				foreach (DataRow row in dtHistory.Rows)
				{
					dtcol.Rows.Add(row["RecordNo"], row["Id"], row["DocumentTypeId"], row["DocNo"], row["DocDate"], row["WorkOrderNo"], row["BomHeaderNo"], row["ItemName"], row["Uom"], row["ItemQty"], row["StartDate"], row["EndDate"], row["DeadLine"], row["BatchDesc"], row["RemarksHeader"], row["EntryUserName"], row["EntryDate"], row["ModifyUserName"], row["ModifyDate"], row["IsApproved"], row["NoOfAttachments"]);
				}
				DatagridHistory.DataSource = dtcol;
				DatagridHistory.RetrieveStructure();
				HistoryGridSetting();
			}
			else
			{
				DatagridHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSetting()
	{
		try
		{
			DatagridHistory.RootTable.Columns["Id"].Visible = false;
			DatagridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DatagridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DatagridHistory.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			DatagridHistory.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			DatagridHistory.RootTable.Columns["ItemQty"].FormatString = "##,#.##";
			DatagridHistory.RootTable.Columns["ItemQty"].TotalFormatString = "##,#.##";
			CommonServices.GridAutoAdjustmentNew(DatagridHistory);
			DatagridHistory.RootTable.Columns.Add("Print");
			DatagridHistory.RootTable.Columns["Print"].Key = "Print";
			DatagridHistory.RootTable.Columns["Print"].Caption = "Print";
			DatagridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DatagridHistory.RootTable.Columns["Print"].Width = 50;
			DatagridHistory.RootTable.Columns["Print"].Position = 0;
			DatagridHistory.RootTable.Columns.Add("Edit");
			DatagridHistory.RootTable.Columns["Edit"].Key = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].Width = 50;
			DatagridHistory.RootTable.Columns["Edit"].Position = 0;
			DatagridHistory.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("ypu don't have updae rights...");
				}
				ReadById(Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells["Id"].Value.ToString()));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			_ = e.Column.Key == "Print";
			if (e.Column.Key == "Edit")
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("ypu don't have updae rights...");
				}
				ReadById(Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells["Id"].Value.ToString()));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DatagridHistory.CurrentRow.Cells["Id"].Value), 1613);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				DatagridHistoryDetailbind(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistoryDetailbind(int Id)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			grdhistoryoperations.ClearStructure();
			WorkOrderInProgress Or = new WorkOrderInProgress();
			Or = WorkOrderInProgress.ReadById(Id);
			if (Or == null)
			{
				return;
			}
			DataTable dtOperationProcessHistory = new DataTable();
			dtOperationProcessHistory.Columns.Add("Id", typeof(int));
			dtOperationProcessHistory.Columns.Add("ItemId", typeof(int));
			dtOperationProcessHistory.Columns.Add("ItemName", typeof(string));
			dtOperationProcessHistory.Columns.Add("UOM", typeof(string));
			dtOperationProcessHistory.Columns.Add("Qty", typeof(decimal));
			dtOperationProcessHistory.Columns.Add("WorkStationName", typeof(string));
			dtOperationProcessHistory.Columns.Add("OperationName", typeof(string));
			dtOperationProcessHistory.Columns.Add("SatrtDate", typeof(DateTime));
			dtOperationProcessHistory.Columns.Add("SatrtTime", typeof(DateTime));
			dtOperationProcessHistory.Columns.Add("CompleteDate", typeof(DateTime));
			dtOperationProcessHistory.Columns.Add("CompleteTime", typeof(DateTime));
			dtOperationProcessHistory.Columns.Add("Remarks", typeof(string));
			if (Or.WorkOrderInProgressDetailList.Count > 0)
			{
				for (int i = 0; i < Or.WorkOrderInProgressDetailList.Count; i++)
				{
					dtOperationProcessHistory.Rows.Add(Or.WorkOrderInProgressDetailList[i].WorkOrderInProgressId, Or.WorkOrderInProgressDetailList[i].ItemId, Or.WorkOrderInProgressDetailList[i].ItemName, Or.WorkOrderInProgressDetailList[i].PackUom, Or.WorkOrderInProgressDetailList[i].Qty, Or.WorkOrderInProgressDetailList[i].WorkStationName, Or.WorkOrderInProgressDetailList[i].OperationName, Or.WorkOrderInProgressDetailList[i].WsOpStartDate, Or.WorkOrderInProgressDetailList[i].WsOpStartTime, Or.WorkOrderInProgressDetailList[i].WsOpCompleteDate, Or.WorkOrderInProgressDetailList[i].WsOpCompleteTime, Or.WorkOrderInProgressDetailList[i].Remarks);
				}
			}
			else
			{
				grdhistoryoperations.ClearStructure();
			}
			grdhistoryoperations.DataSource = dtOperationProcessHistory;
			grdhistoryoperations.RetrieveStructure();
			grdHistoryOperationSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryOperationSetting()
	{
		try
		{
			grdhistoryoperations.RootTable.Columns["Id"].Visible = false;
			grdhistoryoperations.RootTable.Columns["ItemId"].Visible = false;
			grdhistoryoperations.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdhistoryoperations.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdhistoryoperations.RootTable.Columns["Qty"].FormatString = "##,#.##";
			grdhistoryoperations.RootTable.Columns["Qty"].TotalFormatString = "##,#.##";
			CommonServices.GridAutoAdjustmentNew(grdhistoryoperations);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnHistoryRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)cmbDateTypeHistory).Text = string.Empty;
			((Control)(object)cmbItemhistory).Text = string.Empty;
			((Control)(object)CmbBomNoHistory).Text = string.Empty;
			grdhistoryoperations.ClearStructure();
			cmbDateTypeHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshHistoryCombos_Click(object sender, EventArgs e)
	{
		try
		{
			AllCombobind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbDateTypeHistory_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbDateTypeHistory.Value) == 1)
			{
				txtFromdateHistory.Value = DateTime.Today;
				txtToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 2)
			{
				txtFromdateHistory.Value = DateTime.Today.AddDays(-7.0);
				txtToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 3)
			{
				DateTime now = DateTime.Now;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				txtFromdateHistory.Value = first;
				txtToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				txtFromdateHistory.Value = first2;
				txtToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 5)
			{
				txtFromdateHistory.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
				txtToDateHistory.Value = DateTime.Today;
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdWorkOrderMain).Name);
			if (fs != null)
			{
				grdWorkOrderMain.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdWorkOrderMain).Name);
			if (fs != null)
			{
				grdWorkOrderMain.LoadLayoutFile((Stream)fs);
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

	private void ctrlGrdBar7_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DatagridHistory).Name);
			if (fs != null)
			{
				DatagridHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void frmSaleContractSchedule_KeyDown(object sender, KeyEventArgs e)
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
					((Control)(object)grdWorkOrderMain).Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					cmbDateTypeHistory.Focus();
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.Alt)
			{
				BtnShortCutkeys_Click(null, null);
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && (e.KeyCode == Keys.S || e.KeyCode == Keys.U) && formright.DoHaveSaveRight)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P)
				{
					btnPrint_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					((Control)(object)grdWorkOrderMain).Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					btnattachment_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					txtStartDate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grdWorkOrderMain).Focused)
					{
						((Control)(object)grdWorkOrderMain).Focus();
					}
					else if (((Control)(object)grdWorkOrderMain).Focused)
					{
						((Control)(object)grd).Focus();
					}
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnHistoryRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbDateTypeHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				txtFromdateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdhistoryoperations).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)DatagridHistory).Focused)
				{
					((Control)(object)grdhistoryoperations).Focus();
				}
				else if (((Control)(object)grdhistoryoperations).Focused)
				{
					((Control)(object)DatagridHistory).Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			dt.Rows.Add("Ctrl+S", "For Update");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Main Grid");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+Right", "For Move focus From One Grid to Next");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On StartDate in Detail Box");
			dt.Rows.Add("Ctrl+Enter", "For Update Record When Focus On Any Grid ");
			dt.Rows.Add("Ctrl+Space", "To Call Function's On Button Or Link When Focus On Any Grid ");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdWorkOrderMain_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow row = grdWorkOrderMain.CurrentRow;
			if (row == null || (int)row.RowType != 0 || grdWorkOrderMain.CurrentColumn == null || !(grdWorkOrderMain.CurrentColumn.Key == "Delete"))
			{
				return;
			}
			GridEXRow item = grdWorkOrderMain.CurrentRow;
			if (item != null && (int)item.RowType == 0 && dtMain.Rows.Count > 1)
			{
				if (Conversion.ToInt(item.Cells["Id"].Value) == 0)
				{
					item.Delete();
				}
				else
				{
					MessageBox.Show("You Can Not Delete Save Record");
				}
			}
			else
			{
				MessageBox.Show("You Can Not Delete All rows");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow row = grd.CurrentRow;
				if (row != null && (int)row.RowType == 0 && grd.CurrentColumn != null && grd.CurrentColumn.Key == "Delete")
				{
					GridEXRow item = grd.CurrentRow;
					if (item != null && (int)item.RowType == 0)
					{
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						if (Conversion.ToInt(item.Cells["Id"].Value) != 0)
						{
							MessageBox.Show("You Can Not Delete Saved Records");
							return;
						}
						item.Delete();
					}
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnattachment_Click(object sender, EventArgs e)
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

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			WorkOrderSlip(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WorkOrderSlip(int PrintId)
	{
		try
		{
			CommonServices.WorkOrderInProgress_Slip1620(PrintId);
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
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Expected O, but got Unknown
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Expected O, but got Unknown
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Expected O, but got Unknown
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Expected O, but got Unknown
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Expected O, but got Unknown
		//IL_043b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Expected O, but got Unknown
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_045b: Expected O, but got Unknown
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Expected O, but got Unknown
		//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ea: Expected O, but got Unknown
		//IL_0af0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afa: Expected O, but got Unknown
		//IL_25c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ca: Expected O, but got Unknown
		//IL_25d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_25e1: Expected O, but got Unknown
		//IL_25ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_25f8: Expected O, but got Unknown
		//IL_2f0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f17: Expected O, but got Unknown
		//IL_2f24: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f2e: Expected O, but got Unknown
		//IL_46ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_46c4: Expected O, but got Unknown
		//IL_46d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_46db: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		GridEXLayout grdWorkOrderMain_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DatagridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance16 = new Appearance();
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
		GridEXLayout grdhistoryoperations_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel15 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.label36 = new System.Windows.Forms.Label();
		this.cmbItemUOM = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.BtnCancelDetail = new System.Windows.Forms.Button();
		this.CmbItemDetail = new UltraCombo();
		this.BtnAddDetail = new System.Windows.Forms.Button();
		this.txtCompleteDate = new System.Windows.Forms.DateTimePicker();
		this.txtCompleteTime = new System.Windows.Forms.DateTimePicker();
		this.txtStartDate = new System.Windows.Forms.DateTimePicker();
		this.txtStartTime = new System.Windows.Forms.DateTimePicker();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.label27 = new System.Windows.Forms.Label();
		this.txtQtyDetail = new System.Windows.Forms.TextBox();
		this.txtOperation = new System.Windows.Forms.TextBox();
		this.txtWorkStation = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.BtnUpdateDetail = new System.Windows.Forms.Button();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label18 = new System.Windows.Forms.Label();
		this.panel12 = new System.Windows.Forms.Panel();
		this.grdWorkOrderMain = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label14 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label12 = new System.Windows.Forms.Label();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.DatagridHistory = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.panel24 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.btnshow = new System.Windows.Forms.Button();
		this.cmbItemhistory = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.CmbBomNoHistory = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.cmbDateTypeHistory = new UltraCombo();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label28 = new System.Windows.Forms.Label();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromdateHistory = new System.Windows.Forms.DateTimePicker();
		this.label35 = new System.Windows.Forms.Label();
		this.txtFromNoHistory = new System.Windows.Forms.TextBox();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.ctrlGrdBar7 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdhistoryoperations = new GridEX();
		this.panel25 = new System.Windows.Forms.Panel();
		this.toolStrip4 = new System.Windows.Forms.ToolStrip();
		this.btnLoadAllHistoryNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistoryCombos = new System.Windows.Forms.ToolStripButton();
		this.label5 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel16.SuspendLayout();
		this.panel15.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemDetail).BeginInit();
		this.panel5.SuspendLayout();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdWorkOrderMain).BeginInit();
		this.panel13.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.panel6.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).BeginInit();
		this.panel8.SuspendLayout();
		this.panel24.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemhistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBomNoHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).BeginInit();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistoryoperations).BeginInit();
		this.panel25.SuspendLayout();
		this.toolStrip4.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1084, 661);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1084, 661);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1076, 632);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel9);
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1070, 626);
		this.panel2.TabIndex = 0;
		this.panel9.Controls.Add(this.panel4);
		this.panel9.Controls.Add(this.panel12);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 27);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1070, 599);
		this.panel9.TabIndex = 4;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel4.Controls.Add(this.panel16);
		this.panel4.Controls.Add(this.panel15);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 295);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1070, 304);
		this.panel4.TabIndex = 5;
		this.grd.AllowEdit = (InheritableBoolean)2;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 157);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1070, 147);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.ctrlGrdBar3);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 134);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1070, 23);
		this.panel16.TabIndex = 4;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1030, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
		this.ctrlGrdBar3.MyGrid = this.grd;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar3.TabIndex = 56;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel15.Controls.Add(this.groupBox4);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 24);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1070, 110);
		this.panel15.TabIndex = 3;
		this.groupBox4.BackColor = System.Drawing.Color.White;
		this.groupBox4.Controls.Add(this.label36);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemUOM);
		this.groupBox4.Controls.Add(this.label26);
		this.groupBox4.Controls.Add(this.label7);
		this.groupBox4.Controls.Add(this.label4);
		this.groupBox4.Controls.Add(this.label3);
		this.groupBox4.Controls.Add(this.label6);
		this.groupBox4.Controls.Add(this.BtnCancelDetail);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemDetail);
		this.groupBox4.Controls.Add(this.BtnAddDetail);
		this.groupBox4.Controls.Add(this.txtCompleteDate);
		this.groupBox4.Controls.Add(this.txtCompleteTime);
		this.groupBox4.Controls.Add(this.txtStartDate);
		this.groupBox4.Controls.Add(this.txtStartTime);
		this.groupBox4.Controls.Add(this.txtRemarksDetail);
		this.groupBox4.Controls.Add(this.label27);
		this.groupBox4.Controls.Add(this.txtQtyDetail);
		this.groupBox4.Controls.Add(this.txtOperation);
		this.groupBox4.Controls.Add(this.txtWorkStation);
		this.groupBox4.Controls.Add(this.label16);
		this.groupBox4.Controls.Add(this.label9);
		this.groupBox4.Controls.Add(this.label10);
		this.groupBox4.Controls.Add(this.BtnUpdateDetail);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(1, 2);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(1062, 107);
		this.groupBox4.TabIndex = 1;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Detail";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(6, 18);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(67, 15);
		this.label36.TabIndex = 139;
		this.label36.Text = "Item Name";
		this.cmbItemUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbItemUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance51;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance56).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemUOM).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbItemUOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemUOM).Location = new System.Drawing.Point(273, 34);
		((System.Windows.Forms.Control)(object)this.cmbItemUOM).Name = "cmbItemUOM";
		((System.Windows.Forms.Control)(object)this.cmbItemUOM).Size = new System.Drawing.Size(87, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemUOM).TabIndex = 1;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(272, 18);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(35, 15);
		this.label26.TabIndex = 140;
		this.label26.Text = "UOM";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(6, 63);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(86, 15);
		this.label7.TabIndex = 138;
		this.label7.Text = "Complete Date";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(791, 18);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(60, 15);
		this.label4.TabIndex = 138;
		this.label4.Text = "Start Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(140, 63);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(88, 15);
		this.label3.TabIndex = 138;
		this.label3.Text = "Complete Time";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(924, 18);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(62, 15);
		this.label6.TabIndex = 138;
		this.label6.Text = "Start Time";
		this.BtnCancelDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnCancelDetail.FlatAppearance.BorderSize = 0;
		this.BtnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.BtnCancelDetail.Location = new System.Drawing.Point(994, 78);
		this.BtnCancelDetail.Name = "BtnCancelDetail";
		this.BtnCancelDetail.Size = new System.Drawing.Size(61, 26);
		this.BtnCancelDetail.TabIndex = 12;
		this.BtnCancelDetail.Text = "Cancel";
		this.BtnCancelDetail.UseVisualStyleBackColor = false;
		this.BtnCancelDetail.Visible = false;
		this.BtnCancelDetail.Click += new System.EventHandler(BtnCancelDetail_Click);
		this.BtnCancelDetail.Leave += new System.EventHandler(BtnCancelDetail_Leave);
		this.CmbItemDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbItemDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance5;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance10).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemDetail).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbItemDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemDetail).Location = new System.Drawing.Point(6, 34);
		((System.Windows.Forms.Control)(object)this.CmbItemDetail).Name = "CmbItemDetail";
		((System.Windows.Forms.Control)(object)this.CmbItemDetail).Size = new System.Drawing.Size(264, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemDetail).TabIndex = 0;
		this.BtnAddDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnAddDetail.Enabled = false;
		this.BtnAddDetail.FlatAppearance.BorderSize = 0;
		this.BtnAddDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAddDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAddDetail.ForeColor = System.Drawing.Color.White;
		this.BtnAddDetail.Location = new System.Drawing.Point(924, 78);
		this.BtnAddDetail.Name = "BtnAddDetail";
		this.BtnAddDetail.Size = new System.Drawing.Size(39, 26);
		this.BtnAddDetail.TabIndex = 11;
		this.BtnAddDetail.Text = "+";
		this.BtnAddDetail.UseVisualStyleBackColor = false;
		this.BtnAddDetail.Click += new System.EventHandler(BtnAddDetail_Click);
		this.BtnAddDetail.Leave += new System.EventHandler(BtnAddDetail_Leave);
		this.txtCompleteDate.CustomFormat = "dd-MMM-yy";
		this.txtCompleteDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCompleteDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtCompleteDate.Location = new System.Drawing.Point(6, 80);
		this.txtCompleteDate.Name = "txtCompleteDate";
		this.txtCompleteDate.Size = new System.Drawing.Size(131, 23);
		this.txtCompleteDate.TabIndex = 9;
		this.txtCompleteTime.CustomFormat = "HH:MM:tt";
		this.txtCompleteTime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCompleteTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtCompleteTime.Location = new System.Drawing.Point(140, 80);
		this.txtCompleteTime.Name = "txtCompleteTime";
		this.txtCompleteTime.Size = new System.Drawing.Size(130, 23);
		this.txtCompleteTime.TabIndex = 9;
		this.txtStartDate.CustomFormat = "dd-MMM-yy";
		this.txtStartDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtStartDate.Location = new System.Drawing.Point(791, 36);
		this.txtStartDate.Name = "txtStartDate";
		this.txtStartDate.Size = new System.Drawing.Size(131, 23);
		this.txtStartDate.TabIndex = 9;
		this.txtStartTime.CustomFormat = "HH:MM:tt";
		this.txtStartTime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtStartTime.Location = new System.Drawing.Point(924, 36);
		this.txtStartTime.Name = "txtStartTime";
		this.txtStartTime.Size = new System.Drawing.Size(131, 23);
		this.txtStartTime.TabIndex = 9;
		this.txtRemarksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(273, 80);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(649, 23);
		this.txtRemarksDetail.TabIndex = 10;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(273, 63);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(52, 15);
		this.label27.TabIndex = 131;
		this.label27.Text = "Remarks";
		this.txtQtyDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQtyDetail.Enabled = false;
		this.txtQtyDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyDetail.Location = new System.Drawing.Point(363, 36);
		this.txtQtyDetail.Name = "txtQtyDetail";
		this.txtQtyDetail.Size = new System.Drawing.Size(88, 23);
		this.txtQtyDetail.TabIndex = 2;
		this.txtOperation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOperation.Enabled = false;
		this.txtOperation.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOperation.Location = new System.Drawing.Point(622, 36);
		this.txtOperation.Name = "txtOperation";
		this.txtOperation.Size = new System.Drawing.Size(167, 23);
		this.txtOperation.TabIndex = 5;
		this.txtWorkStation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWorkStation.Enabled = false;
		this.txtWorkStation.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWorkStation.Location = new System.Drawing.Point(453, 36);
		this.txtWorkStation.Name = "txtWorkStation";
		this.txtWorkStation.Size = new System.Drawing.Size(167, 23);
		this.txtWorkStation.TabIndex = 4;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(363, 18);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(26, 15);
		this.label16.TabIndex = 13;
		this.label16.Text = "Qty";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(621, 18);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(60, 15);
		this.label9.TabIndex = 131;
		this.label9.Text = "Operation";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(452, 18);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(77, 15);
		this.label10.TabIndex = 131;
		this.label10.Text = "Work Station";
		this.BtnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.BtnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateDetail.Location = new System.Drawing.Point(924, 78);
		this.BtnUpdateDetail.Name = "BtnUpdateDetail";
		this.BtnUpdateDetail.Size = new System.Drawing.Size(67, 26);
		this.BtnUpdateDetail.TabIndex = 7;
		this.BtnUpdateDetail.Text = "Update";
		this.BtnUpdateDetail.UseVisualStyleBackColor = false;
		this.BtnUpdateDetail.Visible = false;
		this.BtnUpdateDetail.Click += new System.EventHandler(BtnUpdateDetail_Click);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label18);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1070, 24);
		this.panel5.TabIndex = 2;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(3, 3);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(250, 17);
		this.label18.TabIndex = 54;
		this.label18.Text = "Work Order In Progress Detail Info";
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.grdWorkOrderMain);
		this.panel12.Controls.Add(this.panel13);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1070, 295);
		this.panel12.TabIndex = 3;
		grdWorkOrderMain_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdWorkOrderMain.DesignTimeLayout = grdWorkOrderMain_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdWorkOrderMain).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdWorkOrderMain.DynamicFiltering = true;
		this.grdWorkOrderMain.FilterMode = (FilterMode)2;
		this.grdWorkOrderMain.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdWorkOrderMain).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdWorkOrderMain.GroupByBoxVisible = false;
		this.grdWorkOrderMain.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.grdWorkOrderMain).Location = new System.Drawing.Point(0, 51);
		((System.Windows.Forms.Control)(object)this.grdWorkOrderMain).Name = "grdWorkOrderMain";
		((ScrollableBase)this.grdWorkOrderMain).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdWorkOrderMain).Size = new System.Drawing.Size(1070, 244);
		((System.Windows.Forms.Control)(object)this.grdWorkOrderMain).TabIndex = 0;
		this.grdWorkOrderMain.TotalRow = (InheritableBoolean)1;
		this.grdWorkOrderMain.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdWorkOrderMain).VisualStyle = (VisualStyle)4;
		this.grdWorkOrderMain.CellUpdated += new ColumnActionEventHandler(grdWorkOrderMain_CellUpdated);
		this.grdWorkOrderMain.ColumnButtonClick += new ColumnActionEventHandler(grdWorkOrderMain_ColumnButtonClick);
		this.grdWorkOrderMain.LinkClicked += new ColumnActionEventHandler(grdWorkOrderMain_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdWorkOrderMain).KeyDown += new System.Windows.Forms.KeyEventHandler(grdWorkOrderMain_KeyDown);
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.ctrlGrdBar2);
		this.panel13.Controls.Add(this.label14);
		this.panel13.Controls.Add(this.panel3);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1070, 51);
		this.panel13.TabIndex = 6;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1031, 29);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.grdWorkOrderMain;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 56;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(3, 30);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(159, 17);
		this.label14.TabIndex = 54;
		this.label14.Text = "Work Order Main Info";
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1070, 27);
		this.panel3.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.btnnew, this.btnRefresh, this.btnUpdate, this.btnattachment, this.btnPrint, this.btnshortcutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1070, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnsave_Click);
		this.btnattachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(100, 24);
		this.btnPrint.Text = "1620-&Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label12);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1070, 27);
		this.panel6.TabIndex = 3;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(5, 5);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(201, 19);
		this.label12.TabIndex = 54;
		this.label12.Text = "Work Order In Progress";
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.DatagridHistory);
		this.tabPage3.Controls.Add(this.panel8);
		this.tabPage3.Controls.Add(this.panel24);
		this.tabPage3.Controls.Add(this.panel7);
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistoryoperations);
		this.tabPage3.Controls.Add(this.panel25);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1076, 632);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "History";
		this.tabPage3.UseVisualStyleBackColor = true;
		DatagridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DatagridHistory.DesignTimeLayout = DatagridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DatagridHistory.DynamicFiltering = true;
		this.DatagridHistory.FilterMode = (FilterMode)2;
		this.DatagridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DatagridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Location = new System.Drawing.Point(0, 161);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Name = "DatagridHistory";
		((ScrollableBase)this.DatagridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Size = new System.Drawing.Size(1076, 230);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).TabIndex = 59;
		this.DatagridHistory.TotalRow = (InheritableBoolean)1;
		this.DatagridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DatagridHistory).VisualStyle = (VisualStyle)4;
		this.DatagridHistory.ColumnButtonClick += new ColumnActionEventHandler(DatagridHistory_ColumnButtonClick);
		this.DatagridHistory.LinkClicked += new ColumnActionEventHandler(DatagridHistory_LinkClicked);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 134);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1076, 27);
		this.panel8.TabIndex = 58;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(6, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 19);
		this.label2.TabIndex = 55;
		this.label2.Text = "History";
		this.panel24.BackColor = System.Drawing.Color.White;
		this.panel24.Controls.Add(this.groupBox1);
		this.panel24.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel24.Location = new System.Drawing.Point(0, 50);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(1076, 84);
		this.panel24.TabIndex = 57;
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemhistory);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBomNoHistory);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory);
		this.groupBox1.Controls.Add(this.txtToDateHistory);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.txtToDocNoHistory);
		this.groupBox1.Controls.Add(this.txtFromdateHistory);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.txtFromNoHistory);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(2, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(894, 77);
		this.groupBox1.TabIndex = 1;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(821, 45);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(62, 25);
		this.btnshow.TabIndex = 7;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.cmbItemhistory.AllowNull = (DefaultableBoolean)1;
		this.cmbItemhistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemhistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemhistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemhistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance46;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemhistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemhistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemhistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemhistory).Location = new System.Drawing.Point(682, 16);
		((System.Windows.Forms.Control)(object)this.cmbItemhistory).Name = "cmbItemhistory";
		((System.Windows.Forms.Control)(object)this.cmbItemhistory).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemhistory).TabIndex = 6;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(621, 22);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(32, 15);
		this.label29.TabIndex = 180;
		this.label29.Text = "Item";
		this.CmbBomNoHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbBomNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBomNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBomNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance21;
		((SpecialBoxBase)((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBomNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBomNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBomNoHistory).Location = new System.Drawing.Point(417, 44);
		((System.Windows.Forms.Control)(object)this.CmbBomNoHistory).Name = "CmbBomNoHistory";
		((System.Windows.Forms.Control)(object)this.CmbBomNoHistory).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbBomNoHistory).TabIndex = 5;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(325, 50);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(56, 15);
		this.label20.TabIndex = 176;
		this.label20.Text = "BOM No.";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(5, 22);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(60, 15);
		this.label19.TabIndex = 174;
		this.label19.Text = "Date Type";
		this.cmbDateTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbDateTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance31;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance36).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance37).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Location = new System.Drawing.Point(97, 16);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Name = "cmbDateTypeHistory";
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Size = new System.Drawing.Size(224, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).TabIndex = 0;
		this.cmbDateTypeHistory.ValueChanged += new System.EventHandler(cmbDateTypeHistory_ValueChanged);
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(210, 46);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.Size = new System.Drawing.Size(111, 23);
		this.txtToDateHistory.TabIndex = 2;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(5, 50);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(91, 15);
		this.label28.TabIndex = 171;
		this.label28.Text = "Date From && To";
		this.txtToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(518, 18);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.Size = new System.Drawing.Size(99, 23);
		this.txtToDocNoHistory.TabIndex = 4;
		this.txtFromdateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromdateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromdateHistory.Location = new System.Drawing.Point(97, 46);
		this.txtFromdateHistory.Name = "txtFromdateHistory";
		this.txtFromdateHistory.Size = new System.Drawing.Size(111, 23);
		this.txtFromdateHistory.TabIndex = 1;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(325, 22);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(88, 15);
		this.label35.TabIndex = 173;
		this.label35.Text = "Doc From && To";
		this.txtFromNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromNoHistory.Location = new System.Drawing.Point(417, 18);
		this.txtFromNoHistory.Name = "txtFromNoHistory";
		this.txtFromNoHistory.Size = new System.Drawing.Size(99, 23);
		this.txtFromNoHistory.TabIndex = 3;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label1);
		this.panel7.Controls.Add(this.ctrlGrdBar7);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel7.Location = new System.Drawing.Point(0, 391);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1076, 27);
		this.panel7.TabIndex = 56;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(6, 3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(67, 19);
		this.label1.TabIndex = 55;
		this.label1.Text = "History";
		this.ctrlGrdBar7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar7.FormName = this;
		this.ctrlGrdBar7.Location = new System.Drawing.Point(1029, 5);
		this.ctrlGrdBar7.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar7.MyGrid = null;
		this.ctrlGrdBar7.Name = "ctrlGrdBar7";
		this.ctrlGrdBar7.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar7.TabIndex = 56;
		this.ctrlGrdBar7.Load += new System.EventHandler(ctrlGrdBar7_Load);
		grdhistoryoperations_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistoryoperations.DesignTimeLayout = grdhistoryoperations_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistoryoperations).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdhistoryoperations.DynamicFiltering = true;
		this.grdhistoryoperations.FilterMode = (FilterMode)2;
		this.grdhistoryoperations.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistoryoperations).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistoryoperations.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdhistoryoperations).Location = new System.Drawing.Point(0, 418);
		((System.Windows.Forms.Control)(object)this.grdhistoryoperations).Name = "grdhistoryoperations";
		((ScrollableBase)this.grdhistoryoperations).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistoryoperations).Size = new System.Drawing.Size(1076, 214);
		((System.Windows.Forms.Control)(object)this.grdhistoryoperations).TabIndex = 53;
		this.grdhistoryoperations.TotalRow = (InheritableBoolean)1;
		this.grdhistoryoperations.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistoryoperations).VisualStyle = (VisualStyle)4;
		this.grdhistoryoperations.ColumnButtonClick += new ColumnActionEventHandler(DatagridHistory_ColumnButtonClick);
		this.grdhistoryoperations.LinkClicked += new ColumnActionEventHandler(DatagridHistory_LinkClicked);
		this.panel25.BackColor = System.Drawing.Color.Teal;
		this.panel25.Controls.Add(this.toolStrip4);
		this.panel25.Controls.Add(this.label5);
		this.panel25.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel25.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel25.Location = new System.Drawing.Point(0, 0);
		this.panel25.Name = "panel25";
		this.panel25.Size = new System.Drawing.Size(1076, 50);
		this.panel25.TabIndex = 54;
		this.toolStrip4.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnLoadAllHistoryNew, this.btnRefreshHistoryCombos });
		this.toolStrip4.Location = new System.Drawing.Point(0, 0);
		this.toolStrip4.Name = "toolStrip4";
		this.toolStrip4.Size = new System.Drawing.Size(1076, 27);
		this.toolStrip4.TabIndex = 8;
		this.toolStrip4.Text = "toolStrip4";
		this.btnLoadAllHistoryNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadAllHistoryNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnLoadAllHistoryNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAllHistoryNew.Name = "btnLoadAllHistoryNew";
		this.btnLoadAllHistoryNew.Size = new System.Drawing.Size(59, 24);
		this.btnLoadAllHistoryNew.Text = "&New";
		this.btnLoadAllHistoryNew.Click += new System.EventHandler(btnHistoryRefresh_Click);
		this.btnRefreshHistoryCombos.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistoryCombos.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistoryCombos.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistoryCombos.Name = "btnRefreshHistoryCombos";
		this.btnRefreshHistoryCombos.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistoryCombos.Text = "&Refresh";
		this.btnRefreshHistoryCombos.Click += new System.EventHandler(btnRefreshHistoryCombos_Click);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(8, 27);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(67, 19);
		this.label5.TabIndex = 55;
		this.label5.Text = "History";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1084, 661);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmWorkOrderInProgress";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmWorkOrderInProgress_FormClosed);
		base.Load += new System.EventHandler(frmSaleContractSchedule_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmSaleContractSchedule_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel15.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemDetail).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel12.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdWorkOrderMain).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel24.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemhistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBomNoHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistoryoperations).EndInit();
		this.panel25.ResumeLayout(false);
		this.panel25.PerformLayout();
		this.toolStrip4.ResumeLayout(false);
		this.toolStrip4.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
