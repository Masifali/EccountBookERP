// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
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

public class frmStockTranfserRegister : Form
{
	private DropDownBind DDL = new DropDownBind();

	private DataTable dtGrid = new DataTable();

	private DataTable dtBranch = new DataTable();

	private int[] AryColWidth;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Label lblCaption;

	private ToolStrip toolStrip1;

	private ToolStripButton btnPrint;

	private Panel panel3;

	private Panel panel5;

	private Panel panel2;

	internal GridEX grdfrm;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton btnRefresh;

	private ToolStripButton toolStripButton1;

	private Panel panel4;

	private Label label7;

	private GroupBox groupBox1;

	private Button btnshow;

	private Label label6;

	private Label label5;

	private Label label3;

	private Label label1;

	private Label label9;

	internal UltraCombo CmbJobLotTo;

	internal UltraCombo CmbFromJobLot;

	internal UltraCombo CmbToWareHouse;

	internal UltraCombo CmbFromWareHouse;

	internal UltraCombo cmbItem;

	private Label label4;

	private Label label2;

	private DateTimePicker gpToDate;

	private DateTimePicker gpFromDate;

	private Label label8;

	internal UltraCombo cmbDocumenttype;

	private Label label10;

	internal UltraCombo CmbBranchName;

	public frmStockTranfserRegister(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void ComboBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = 608;
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
			DataTable dt = InvStockTransferHeader.AllComboAgainstStockTransfer(obj);
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtItemName = new DataTable();
			dtItemName.Columns.Add("Id", typeof(int));
			dtItemName.Columns.Add("ItemName", typeof(string));
			DataTable dtJobLotFrom = new DataTable();
			dtJobLotFrom.Columns.Add("Id", typeof(int));
			dtJobLotFrom.Columns.Add("JobLotFrom", typeof(string));
			DataTable dtJobLotTo = new DataTable();
			dtJobLotTo.Columns.Add("Id", typeof(int));
			dtJobLotTo.Columns.Add("JobLotTo", typeof(string));
			DataTable dtFromWareHouseName = new DataTable();
			dtFromWareHouseName.Columns.Add("Id", typeof(int));
			dtFromWareHouseName.Columns.Add("FromWareHouseName", typeof(string));
			DataTable dtToWareHouseName = new DataTable();
			dtToWareHouseName.Columns.Add("Id", typeof(int));
			dtToWareHouseName.Columns.Add("ToWareHouseName", typeof(string));
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				string activity = dt.Rows[i]["Activity"].ToString();
				if (activity == "ItemName")
				{
					dtItemName.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				if (activity == "JobLotFrom")
				{
					dtJobLotFrom.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				if (activity == "JobLotTo")
				{
					dtJobLotTo.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				if (activity == "WareHouseFrom")
				{
					dtFromWareHouseName.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				if (activity == "WareHouseTo")
				{
					dtToWareHouseName.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
			}
			DDL.BindDDL(dtItemName, cmbItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
			DDL.BindDDL(dtJobLotFrom, CmbFromJobLot, "Id", "JobLotFrom", "From JobLot", ZeroIndex: true);
			DDL.BindDDL(dtJobLotTo, CmbJobLotTo, "Id", "JobLotTo", "To JobLot", ZeroIndex: true);
			DDL.BindDDL(dtFromWareHouseName, CmbFromWareHouse, "Id", "FromWareHouseName", "From Warehouse", ZeroIndex: true);
			DDL.BindDDL(dtToWareHouseName, CmbToWareHouse, "Id", "ToWareHouseName", "To Warehouse", ZeroIndex: true);
			((UltraGridBase)CmbFromJobLot).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbJobLotTo).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbFromWareHouse).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbToWareHouse).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumentTypefill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("DocumentType");
			dt.Rows.Add(1, "Auto");
			dt.Rows.Add(2, "Manual");
			DDL.BindDDLNew(dt, cmbDocumenttype, "Id", "DocumentType", "Document Type", false);
			((UltraGridBase)cmbDocumenttype).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchesFill()
	{
		try
		{
			dtBranch = InvStockTransferHeader.GetBranchesAllocatedToUserFromStockTransfer(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, 68);
			if (dtBranch.Rows.Count > 0)
			{
				DDL.BindDDL(dtBranch, CmbBranchName, "BranchId", "BranchName", "Branch Name", ZeroIndex: false);
				UltraGridColumn obj = ((UltraGridBase)CmbBranchName).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj).Key = "Selected";
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
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

	private void frmGatePassReport_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(950, 600);
			ctrlGrdBar1.mGridSaveLayouts.Visible = false;
			ctrlGrdBar1.mGridChooseFielder.Visible = false;
			ctrlGrdBar1.removeLayoutToolStripMenuItem.Visible = false;
			gpFromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			gpToDate.Value = DateTime.Now;
			BranchesFill();
			ComboBind();
			DocumentTypefill();
			gpFromDate.Focus();
			gpFromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			GridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj1 = new ReportsParameters();
			obj1.OrganizationId = UserAccount.OrganizationId;
			obj1.CompanyId = UserAccount.CompanyId;
			obj1.FromDate = gpFromDate.Value;
			obj1.ToDate = gpToDate.Value;
			obj1.ItemId = Conversion.ToInt(cmbItem.Value);
			obj1.FromWarehouseId = Conversion.ToInt(CmbFromWareHouse.Value);
			obj1.ToWarehouseId = Conversion.ToInt(CmbToWareHouse.Value);
			obj1.JobLotId = Conversion.ToInt(CmbFromJobLot.Value);
			obj1.ToJobLotId = Conversion.ToInt(CmbJobLotTo.Value);
			obj1.DocumentTypeId = ((((UltraGridBase)cmbDocumenttype).ActiveRow != null) ? ((((Control)(object)cmbDocumenttype).Text == "Auto") ? 68 : 806) : 0);
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
				obj1.BranchesIds = BranchIds;
				dtGrid = InvStockTransferHeader.StockTransferSlipandRegister(obj1);
				if (dtGrid.Rows.Count > 0)
				{
					DataTable dtlst = new DataTable();
					dtlst.Columns.Add("Id", typeof(int));
					dtlst.Columns.Add("DocNo", typeof(int));
					dtlst.Columns.Add("DocDate", typeof(DateTime));
					dtlst.Columns.Add("WbNetWeight", typeof(double));
					dtlst.Columns.Add("OtherWeight.", typeof(double));
					dtlst.Columns.Add("DiffWeight", typeof(double));
					dtlst.Columns.Add("EntryDate", typeof(DateTime));
					dtlst.Columns.Add("IsApproved", typeof(string));
					dtlst.Columns.Add("ApprovedUserId ", typeof(int));
					dtlst.Columns.Add("ApprovedDate", typeof(DateTime));
					dtlst.Columns.Add("TransferType", typeof(string));
					dtlst.Columns.Add("GpSrNo", typeof(int));
					dtlst.Columns.Add("VehicleNo", typeof(string));
					dtlst.Columns.Add("BiltyNo", typeof(string));
					dtlst.Columns.Add("DoNo", typeof(int));
					dtlst.Columns.Add("ItemName", typeof(string));
					dtlst.Columns.Add("Equivalent", typeof(double));
					dtlst.Columns.Add("ToWareHouseName", typeof(string));
					dtlst.Columns.Add("JobLotTo", typeof(string));
					dtlst.Columns.Add("TicketNo", typeof(int));
					dtlst.Columns.Add("WorkingReportNo", typeof(int));
					dtlst.Columns.Add("CropYear", typeof(string));
					dtlst.Columns.Add("Qty", typeof(double));
					dtlst.Columns.Add("GrossWeight", typeof(double));
					dtlst.Columns.Add("EbUnit", typeof(int));
					dtlst.Columns.Add("EbTotal ", typeof(double));
					dtlst.Columns.Add("AdLsWeight", typeof(double));
					dtlst.Columns.Add("NetWeight", typeof(double));
					dtlst.Columns.Add("EntryUserName", typeof(string));
					dtlst.Columns.Add("ApprovedUserName", typeof(string));
					for (int i = 0; i < dtGrid.Rows.Count; i++)
					{
						dtlst.Rows.Add(Conversion.ToInt(dtGrid.Rows[i]["Id"]), Conversion.ToInt(dtGrid.Rows[i]["DocNo"]), Conversion.ToDateTime(dtGrid.Rows[i]["DocDate"]).ToShortDateString(), Conversion.ToDouble(dtGrid.Rows[i]["WbNetWeight"]), Conversion.ToDouble(dtGrid.Rows[i]["OtherWeight"]), Conversion.ToDouble(dtGrid.Rows[i]["DiffWeight"]), Conversion.ToDateTime(dtGrid.Rows[i]["EntryDate"]), Conversion.ToString(dtGrid.Rows[i]["IsApproved"]), Conversion.ToInt(dtGrid.Rows[i]["ApprovedUserId"]), Conversion.ToDateTime(dtGrid.Rows[i]["ApprovedDate"]).ToShortDateString(), Conversion.ToString(dtGrid.Rows[i]["TransferType"]), Conversion.ToInt(dtGrid.Rows[i]["GpSrNo"]), Conversion.ToString(dtGrid.Rows[i]["VehicleNo"]), Conversion.ToInt(dtGrid.Rows[i]["BiltyNo"]), Conversion.ToInt(dtGrid.Rows[i]["DoNo"]), Conversion.ToString(dtGrid.Rows[i]["ItemName"]), Conversion.ToDouble(dtGrid.Rows[i]["Equivalent"]), Conversion.ToString(dtGrid.Rows[i]["ToWareHouseName"]), Conversion.ToString(dtGrid.Rows[i]["JobLotTo"]), Conversion.ToInt(dtGrid.Rows[i]["TicketNo"]), Conversion.ToInt(dtGrid.Rows[i]["WorkingReportNo"]), Conversion.ToString(dtGrid.Rows[i]["CropYear"]), Conversion.ToDouble(dtGrid.Rows[i]["Qty"]), Conversion.ToDouble(dtGrid.Rows[i]["GrossWeight"]), Conversion.ToInt(dtGrid.Rows[i]["EbUnit"]), Conversion.ToInt(dtGrid.Rows[i]["EbTotal"]), Conversion.ToDouble(dtGrid.Rows[i]["AdLsWeight"]), Conversion.ToDouble(dtGrid.Rows[i]["NetWeight"]), Conversion.ToString(dtGrid.Rows[i]["EntryUserName"]), Conversion.ToString(dtGrid.Rows[i]["ApprovedUserName"]));
					}
					grdfrm.DataSource = dtlst;
					grdfrmSetting();
				}
				else
				{
					grdfrm.DataSource = null;
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

	private void grdfrmSetting()
	{
		try
		{
			grdfrm.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yy";
			grdfrm.RootTable.Columns["Equivalent"].Caption = "Pack Size";
			grdfrm.RootTable.Columns["DocNo"].Width = 45;
			grdfrm.RootTable.Columns["DocDate"].Width = 75;
			grdfrm.RootTable.Columns["WbNetWeight"].Width = 90;
			grdfrm.RootTable.Columns["OtherWeight"].Width = 80;
			grdfrm.RootTable.Columns["DiffWeight"].Width = 80;
			grdfrm.RootTable.Columns["EntryDate"].Width = 80;
			grdfrm.RootTable.Columns["IsApproved"].Width = 100;
			grdfrm.RootTable.Columns["ApprovedDate"].Width = 80;
			grdfrm.RootTable.Columns["TransferType"].Width = 80;
			grdfrm.RootTable.Columns["GpSrNo"].Width = 43;
			grdfrm.RootTable.Columns["VehicleNo"].Width = 70;
			grdfrm.RootTable.Columns["BiltyNo"].Width = 70;
			grdfrm.RootTable.Columns["DoNo"].Width = 43;
			grdfrm.RootTable.Columns["ItemName"].Width = 160;
			grdfrm.RootTable.Columns["Equivalent"].Width = 50;
			grdfrm.RootTable.Columns["ToWareHouseName"].Width = 100;
			grdfrm.RootTable.Columns["JobLotTo"].Width = 120;
			grdfrm.RootTable.Columns["TicketNo"].Width = 60;
			grdfrm.RootTable.Columns["WorkingReportNo"].Width = 90;
			grdfrm.RootTable.Columns["CropYear"].Width = 60;
			grdfrm.RootTable.Columns["Qty"].Width = 65;
			grdfrm.RootTable.Columns["GrossWeight"].Width = 80;
			grdfrm.RootTable.Columns["EbUnit"].Width = 60;
			grdfrm.RootTable.Columns["EbTotal"].Width = 80;
			grdfrm.RootTable.Columns["AdLsWeight"].Width = 65;
			grdfrm.RootTable.Columns["NetWeight"].Width = 85;
			grdfrm.RootTable.Columns["EntryUserName"].Width = 90;
			grdfrm.RootTable.Columns["ApprovedUserName"].Width = 120;
			grdfrm.RootTable.Columns["Equivalent"].TextAlignment = (TextAlignment)2;
			grdfrm.RootTable.Columns["EbUnit"].TextAlignment = (TextAlignment)2;
			grdfrm.RootTable.Columns["WbNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["WbNetWeight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["WbNetWeight"].FormatString = "#,##0.##";
			grdfrm.RootTable.Columns["WbNetWeight"].TotalFormatString = "#,##0.##";
			grdfrm.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["Qty"].FormatString = "#,#";
			grdfrm.RootTable.Columns["Qty"].TotalFormatString = "#,#";
			grdfrm.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["GrossWeight"].FormatString = "#,##0.##";
			grdfrm.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
			grdfrm.RootTable.Columns["AdLsWeight"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["AdLsWeight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["AdLsWeight"].FormatString = "#,##0.##";
			grdfrm.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["NetWeight"].FormatString = "#,##0.##";
			grdfrm.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			grdfrm.RootTable.Columns["DiffWeight"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["DiffWeight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["DiffWeight"].FormatString = "#,##0.##";
			grdfrm.RootTable.Columns["DiffWeight"].TotalFormatString = "#,##0.##";
			grdfrm.RootTable.Columns["Id"].Visible = false;
			grdfrm.RootTable.ColumnSets["EntryDate"].Visible = false;
			grdfrm.RootTable.ColumnSets["IsApproved"].Visible = false;
			grdfrm.RootTable.ColumnSets["ApprovedDate"].Visible = false;
			grdfrm.RootTable.ColumnSets["ApprovedUserName"].Visible = false;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSearch_Click(object sender, EventArgs e)
	{
		try
		{
			GridFill();
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtGrid == null || dtGrid.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dtGrid, "408-StockTransferRegister.rpt");
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
			gpFromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			gpToDate.Value = DateTime.Now;
			((Control)(object)cmbItem).Text = string.Empty;
			GridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton1_Click_1(object sender, EventArgs e)
	{
		try
		{
			ComboBind();
			DocumentTypefill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmStockTranfserRegister_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.Control && e.KeyCode == Keys.P)
			{
				toolStripButton1_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnRefresh_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmStockTranfserRegister_FormClosing(object sender, FormClosingEventArgs e)
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

	private void CmbBranchName_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)CmbBranchName).Text != string.Empty && ((UltraGridBase)CmbBranchName).ActiveRow != null)
			{
				ComboBind();
				return;
			}
			((Control)(object)CmbFromWareHouse).Text = string.Empty;
			((UltraGridBase)CmbFromWareHouse).ActiveRow = null;
			((Control)(object)CmbToWareHouse).Text = string.Empty;
			((UltraGridBase)CmbToWareHouse).ActiveRow = null;
			((Control)(object)cmbItem).Text = string.Empty;
			((UltraGridBase)cmbItem).ActiveRow = null;
			((Control)(object)CmbFromJobLot).Text = string.Empty;
			((UltraGridBase)CmbFromJobLot).ActiveRow = null;
			((Control)(object)CmbJobLotTo).Text = string.Empty;
			((UltraGridBase)CmbJobLotTo).ActiveRow = null;
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
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected O, but got Unknown
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Expected O, but got Unknown
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Expected O, but got Unknown
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Expected O, but got Unknown
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Expected O, but got Unknown
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Expected O, but got Unknown
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Expected O, but got Unknown
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Expected O, but got Unknown
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Expected O, but got Unknown
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Expected O, but got Unknown
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Expected O, but got Unknown
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Expected O, but got Unknown
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Expected O, but got Unknown
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Expected O, but got Unknown
		GridEXLayout grdfrm_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Inventory_Reports.frmStockTranfserRegister));
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
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
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.grdfrm = new GridEX();
		this.lblCaption = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label10 = new System.Windows.Forms.Label();
		this.CmbBranchName = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.cmbDocumenttype = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.CmbJobLotTo = new UltraCombo();
		this.CmbFromJobLot = new UltraCombo();
		this.CmbToWareHouse = new UltraCombo();
		this.CmbFromWareHouse = new UltraCombo();
		this.cmbItem = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.gpToDate = new System.Windows.Forms.DateTimePicker();
		this.gpFromDate = new System.Windows.Forms.DateTimePicker();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label7 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel3.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranchName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDocumenttype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLotTo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFromJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbToWareHouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFromWareHouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		this.panel2.SuspendLayout();
		this.panel4.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(998, 28);
		this.panel1.TabIndex = 0;
		this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnRefresh, this.toolStripButton1, this.btnPrint });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(266, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(59, 24);
		this.btnRefresh.Text = "&New";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripButton1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(80, 24);
		this.toolStripButton1.Text = "&Refresh";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click_1);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(115, 24);
		this.btnPrint.Text = "408-Register";
		this.btnPrint.Click += new System.EventHandler(toolStripButton1_Click);
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdfrm).BackColor = System.Drawing.Color.White;
		this.grdfrm.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdfrm_DesignTimeLayout.LayoutString = resources.GetString("grdfrm_DesignTimeLayout.LayoutString");
		this.grdfrm.DesignTimeLayout = grdfrm_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupTotals = (GroupTotals)2;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(998, 387);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 0;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.lblCaption.AutoSize = true;
		this.lblCaption.BackColor = System.Drawing.Color.Teal;
		this.lblCaption.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCaption.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.lblCaption.Location = new System.Drawing.Point(7, 3);
		this.lblCaption.Name = "lblCaption";
		this.lblCaption.Size = new System.Drawing.Size(194, 19);
		this.lblCaption.TabIndex = 0;
		this.lblCaption.Text = "Stock Tranfer Register";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.lblCaption);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 28);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(998, 27);
		this.panel3.TabIndex = 3;
		this.panel5.Controls.Add(this.groupBox1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel5.Location = new System.Drawing.Point(0, 55);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(998, 168);
		this.panel5.TabIndex = 4;
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranchName);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDocumenttype);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLotTo);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFromJobLot);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbToWareHouse);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFromWareHouse);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.gpToDate);
		this.groupBox1.Controls.Add(this.gpFromDate);
		this.groupBox1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 5);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(694, 157);
		this.groupBox1.TabIndex = 65;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.label10.AutoSize = true;
		this.label10.BackColor = System.Drawing.Color.White;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(9, 22);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(79, 15);
		this.label10.TabIndex = 83;
		this.label10.Text = "Branch Name";
		this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.CmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.CmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBranchName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Location = new System.Drawing.Point(113, 16);
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Name = "CmbBranchName";
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Size = new System.Drawing.Size(184, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranchName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Leave += new System.EventHandler(CmbBranchName_Leave);
		this.label8.AutoSize = true;
		this.label8.BackColor = System.Drawing.Color.White;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(304, 107);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(92, 15);
		this.label8.TabIndex = 81;
		this.label8.Text = "Document Type";
		this.cmbDocumenttype.AllowNull = (DefaultableBoolean)1;
		this.cmbDocumenttype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDocumenttype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDocumenttype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDocumenttype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbDocumenttype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDocumenttype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDocumenttype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDocumenttype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.cmbDocumenttype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDocumenttype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbDocumenttype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDocumenttype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDocumenttype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDocumenttype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbDocumenttype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDocumenttype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDocumenttype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance78).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDocumenttype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbDocumenttype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDocumenttype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDocumenttype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDocumenttype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbDocumenttype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDocumenttype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDocumenttype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbDocumenttype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDocumenttype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbDocumenttype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDocumenttype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDocumenttype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDocumenttype).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDocumenttype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDocumenttype).Location = new System.Drawing.Point(401, 100);
		((System.Windows.Forms.Control)(object)this.cmbDocumenttype).Name = "cmbDocumenttype";
		((System.Windows.Forms.Control)(object)this.cmbDocumenttype).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.cmbDocumenttype).TabIndex = 8;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(574, 101);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(61, 25);
		this.btnshow.TabIndex = 9;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnSearch_Click);
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.White;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(304, 78);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(59, 15);
		this.label6.TabIndex = 75;
		this.label6.Text = "To JobLot";
		this.label5.AutoSize = true;
		this.label5.BackColor = System.Drawing.Color.White;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(304, 50);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(74, 15);
		this.label5.TabIndex = 76;
		this.label5.Text = "From JobLot";
		this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.White;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(9, 129);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(85, 15);
		this.label3.TabIndex = 77;
		this.label3.Text = "To WareHouse";
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.White;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(9, 101);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(100, 15);
		this.label1.TabIndex = 78;
		this.label1.Text = "From WareHouse";
		this.label9.AutoSize = true;
		this.label9.BackColor = System.Drawing.Color.White;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(304, 22);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(67, 15);
		this.label9.TabIndex = 79;
		this.label9.Text = "Item Name";
		this.CmbJobLotTo.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLotTo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLotTo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLotTo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLotTo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance60;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLotTo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance65).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance68).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLotTo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLotTo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLotTo).Location = new System.Drawing.Point(401, 72);
		((System.Windows.Forms.Control)(object)this.CmbJobLotTo).Name = "CmbJobLotTo";
		((System.Windows.Forms.Control)(object)this.CmbJobLotTo).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLotTo).TabIndex = 7;
		this.CmbFromJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbFromJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFromJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFromJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFromJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbFromJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFromJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFromJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFromJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance73;
		((SpecialBoxBase)((UltraGridBase)this.CmbFromJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFromJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbFromJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFromJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFromJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFromJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbFromJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFromJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFromJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance79).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFromJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbFromJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFromJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFromJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFromJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbFromJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFromJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFromJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbFromJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFromJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbFromJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFromJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFromJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFromJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbFromJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFromJobLot).Location = new System.Drawing.Point(401, 44);
		((System.Windows.Forms.Control)(object)this.CmbFromJobLot).Name = "CmbFromJobLot";
		((System.Windows.Forms.Control)(object)this.CmbFromJobLot).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.CmbFromJobLot).TabIndex = 6;
		this.CmbToWareHouse.AllowNull = (DefaultableBoolean)1;
		this.CmbToWareHouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbToWareHouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbToWareHouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbToWareHouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbToWareHouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbToWareHouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbToWareHouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbToWareHouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbToWareHouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbToWareHouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbToWareHouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbToWareHouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbToWareHouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbToWareHouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbToWareHouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbToWareHouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbToWareHouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbToWareHouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbToWareHouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbToWareHouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbToWareHouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbToWareHouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbToWareHouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbToWareHouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbToWareHouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbToWareHouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbToWareHouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbToWareHouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbToWareHouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbToWareHouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbToWareHouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbToWareHouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbToWareHouse).Location = new System.Drawing.Point(113, 123);
		((System.Windows.Forms.Control)(object)this.CmbToWareHouse).Name = "CmbToWareHouse";
		((System.Windows.Forms.Control)(object)this.CmbToWareHouse).Size = new System.Drawing.Size(184, 26);
		((System.Windows.Forms.Control)(object)this.CmbToWareHouse).TabIndex = 4;
		this.CmbFromWareHouse.AllowNull = (DefaultableBoolean)1;
		this.CmbFromWareHouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFromWareHouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFromWareHouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFromWareHouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbFromWareHouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFromWareHouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFromWareHouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFromWareHouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.CmbFromWareHouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFromWareHouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbFromWareHouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFromWareHouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFromWareHouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFromWareHouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbFromWareHouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFromWareHouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFromWareHouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFromWareHouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbFromWareHouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFromWareHouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFromWareHouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFromWareHouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbFromWareHouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFromWareHouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFromWareHouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbFromWareHouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFromWareHouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbFromWareHouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFromWareHouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFromWareHouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFromWareHouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbFromWareHouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFromWareHouse).Location = new System.Drawing.Point(113, 95);
		((System.Windows.Forms.Control)(object)this.CmbFromWareHouse).Name = "CmbFromWareHouse";
		((System.Windows.Forms.Control)(object)this.CmbFromWareHouse).Size = new System.Drawing.Size(184, 26);
		((System.Windows.Forms.Control)(object)this.CmbFromWareHouse).TabIndex = 3;
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(401, 16);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 5;
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.White;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(9, 74);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(48, 15);
		this.label4.TabIndex = 74;
		this.label4.Text = "Date To";
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.White;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(9, 48);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(63, 15);
		this.label2.TabIndex = 73;
		this.label2.Text = "Date From";
		this.gpToDate.CustomFormat = "dd-MMM-yy";
		this.gpToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.gpToDate.Location = new System.Drawing.Point(113, 70);
		this.gpToDate.Name = "gpToDate";
		this.gpToDate.Size = new System.Drawing.Size(184, 23);
		this.gpToDate.TabIndex = 2;
		this.gpFromDate.CustomFormat = "dd-MMM-yy";
		this.gpFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.gpFromDate.Location = new System.Drawing.Point(113, 44);
		this.gpFromDate.Name = "gpFromDate";
		this.gpFromDate.Size = new System.Drawing.Size(184, 23);
		this.gpFromDate.TabIndex = 1;
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 223);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(998, 387);
		this.panel2.TabIndex = 5;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(998, 27);
		this.panel4.TabIndex = 4;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(950, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 24);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.Teal;
		this.label7.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label7.Location = new System.Drawing.Point(7, 3);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(67, 19);
		this.label7.TabIndex = 0;
		this.label7.Text = "History";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(998, 610);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel5);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmStockTranfserRegister";
		this.Text = "Eccount Book";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmStockTranfserRegister_FormClosing);
		base.Load += new System.EventHandler(frmGatePassReport_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmStockTranfserRegister_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranchName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDocumenttype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLotTo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFromJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbToWareHouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFromWareHouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
