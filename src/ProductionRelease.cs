// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.pcc;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Common;
using Architecture.Model.pcc;
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
namespace Architecture.WinApp.pcc.Production
{

public class ProductionRelease : Form
{
	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtHistory = new DataTable();

	private bool UpdateMode;

	private int RecId;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage HistoryTab;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private TabPage FormTab;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	private ToolStripSeparator toolStripSeparator1;

	private Label label21;

	private Panel panel38;

	private Label label32;

	private Panel panel45;

	private Panel panel8;

	private Panel panel19;

	internal GridEX grdHistoryInput;

	private Panel panel44;

	internal GridEX grd;

	private Panel panel15;

	private Label label17;

	private Panel panel13;

	private GroupBox groupBox1;

	private Label label19;

	internal UltraCombo cmbDateTypeHistory;

	private DateTimePicker txtToDateHistory;

	private Label label28;

	private TextBox txtToDocNoHistory;

	private DateTimePicker txtFromdateHistory;

	private Label label35;

	private TextBox txtFromNoHistory;

	private Label label30;

	internal UltraCombo cmbProductionNoHistory;

	internal UltraCombo CmbBomNoHistory;

	private Label label20;

	internal UltraCombo cmbItemhistory;

	private Label label29;

	private Button btnshow;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdHistoryOutput;

	private Panel panel12;

	private Label label14;

	private Label label15;

	internal UltraCombo CmbProductionType;

	private Label label40;

	internal UltraCombo CmbPlant;

	private Label label34;

	private ToolStripButton btnSave;

	private Panel panel2;

	private GroupBox groupBox2;

	private Label label2;

	private DateTimePicker ToDate;

	private Label label1;

	private DateTimePicker FromDate;

	private Button btnHistoryShow;

	private ToolStripButton btnRefresh;

	internal GridEX grdHistory;

	public ProductionRelease(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmBillOfMaterial_Manufacturing_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 750);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formright.DoHaveSaveRight;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			Datetypefill();
			AllCombobind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllCombobind()
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		try
		{
			int PlantId = Conversion.ToInt(CmbPlant.Value);
			int ProdType = Conversion.ToInt(CmbProductionType.Value);
			int Idname = Conversion.ToInt(CmbBomNoHistory.Value);
			int Idtype = Conversion.ToInt(cmbProductionNoHistory.Value);
			int Iditem = Conversion.ToInt(cmbItemhistory.Value);
			DataTable dtcombo = ProductionHeader.GetDataForDropDownFromProductionHeader(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable dtPlant = new DataTable();
			dtPlant.Columns.Add("Id", typeof(int));
			dtPlant.Columns.Add("Plant");
			DataTable dtProdType = new DataTable();
			dtProdType.Columns.Add("Id", typeof(int));
			dtProdType.Columns.Add("ProductionType");
			DataTable dtProductionNo = new DataTable();
			dtProductionNo.Columns.Add("Id", typeof(int));
			dtProductionNo.Columns.Add("ProductionNo");
			DataTable dtBOMName = new DataTable();
			dtBOMName.Columns.Add("Id", typeof(int));
			dtBOMName.Columns.Add("BOMName");
			DataTable dtitem = new DataTable();
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName");
			if (dtcombo.Rows.Count > 0)
			{
				for (int i = 0; i < dtcombo.Rows.Count; i++)
				{
					if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "Plant")
					{
						dtPlant.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ProductionType")
					{
						dtProdType.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ProductionNo")
					{
						dtProductionNo.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "BOMName")
					{
						dtBOMName.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ItemHeader")
					{
						dtitem.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtPlant, CmbPlant, "Id", "Plant", "Plant", ZeroIndex: true);
			((UltraGridBase)CmbPlant).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (PlantId > 0)
			{
				if (dtPlant.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == PlantId))
				{
					CmbPlant.Value = PlantId;
				}
				else
				{
					((Control)(object)CmbPlant).Text = string.Empty;
				}
			}
			DDL.BindDDL(dtProdType, CmbProductionType, "Id", "ProductionType", "Production Type", ZeroIndex: true);
			((UltraGridBase)CmbProductionType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (ProdType > 0)
			{
				if (dtProdType.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ProdType))
				{
					CmbProductionType.Value = ProdType;
				}
				else
				{
					((Control)(object)CmbProductionType).Text = string.Empty;
				}
			}
			DDL.BindDDL(dtProductionNo, cmbProductionNoHistory, "Id", "ProductionNo", "Production No", ZeroIndex: true);
			((UltraGridBase)cmbProductionNoHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (Idtype > 0)
			{
				if (dtProductionNo.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Idtype))
				{
					cmbProductionNoHistory.Value = Idtype;
				}
				else
				{
					((Control)(object)cmbProductionNoHistory).Text = string.Empty;
				}
			}
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

	private void cmbDateTypeHistory_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbDateTypeHistory.Value) == 1)
			{
				txtFromdateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 2)
			{
				txtFromdateHistory.Value = DateTime.Today.AddDays(-7.0);
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
			}
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
			FromDate.Value = DateTime.Now.AddDays(-7.0);
			ToDate.Value = DateTime.Now;
			HistoryBind();
		}
	}

	private void HistoryBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		try
		{
			DataTable dt = ProductionRelease.FormHistory(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 1857,
				FromDate = Conversion.ToDateTime((object)txtFromdateHistory.Value),
				ToDate = Conversion.ToDateTime((object)txtToDateHistory.Value),
				CanViewAllRecord = formright.DoHaveCanViewAllRecordRights,
				EntryUser = UserAccount.ID
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtcol = new DataTable();
			dtcol.Columns.Add("Id", typeof(int));
			dtcol.Columns.Add("DocNo", typeof(int));
			dtcol.Columns.Add("DocDate", typeof(DateTime));
			dtcol.Columns.Add("RefDocId", typeof(int));
			dtcol.Columns.Add("RefDocNo", typeof(int));
			dtcol.Columns.Add("RefDocDate", typeof(DateTime));
			dtcol.Columns.Add("ProductionNo", typeof(string));
			dtcol.Columns.Add("ProductionType", typeof(string));
			dtcol.Columns.Add("PlantName", typeof(string));
			dtcol.Columns.Add("BomHeaderNo", typeof(string));
			dtcol.Columns.Add("ItemName", typeof(string));
			dtcol.Columns.Add("ItemCode", typeof(string));
			dtcol.Columns.Add("UOM", typeof(string));
			dtcol.Columns.Add("ItemQty", typeof(decimal));
			dtcol.Columns.Add("WipWareHouseName", typeof(string));
			dtcol.Columns.Add("WIPAccountTitle", typeof(string));
			dtcol.Columns.Add("RemarksHeader", typeof(string));
			dtcol.Columns.Add("EntryDate", typeof(DateTime));
			dtcol.Columns.Add("EntryUserName", typeof(string));
			dtcol.Columns.Add("ModifyDate", typeof(DateTime));
			dtcol.Columns.Add("ModifyUserName", typeof(string));
			dtcol.Rows.Clear();
			foreach (DataRow row in dt.Rows)
			{
				dtcol.Rows.Add(row["Id"], row["DocNo"], Conversion.ToDateTime(row["DocDate"]).ToShortDateString(), row["RefDocId"], row["RefDocNo"], Conversion.ToDateTime(row["RefDocDate"]).ToShortDateString(), row["ProductionNo"], row["ProductionType"], row["PlantName"], row["BomHeaderNo"], row["ItemName"], row["ItemCode"], row["UOM"], row["ItemQty"], row["WareHouseName"], row["WIPAccountTitle"], row["RemarksHeader"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"]);
			}
			grdHistory.DataSource = dtcol;
			grdHistory.RetrieveStructure();
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["RefDocId"].Visible = false;
			grdHistory.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["ItemQty"].FormatString = "##,#.##";
			grdHistory.RootTable.Columns["ItemQty"].TotalFormatString = "##,#.##";
			CommonServices.GridAutoAdjustmentNew(grdHistory);
			grdHistory.RootTable.Columns.Add("Delete");
			grdHistory.RootTable.Columns["Delete"].Key = "Delete";
			grdHistory.RootTable.Columns["Delete"].Caption = "X";
			grdHistory.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Delete"].ButtonText = "X";
			grdHistory.RootTable.Columns["Delete"].Width = 20;
			grdHistory.RootTable.Columns["Delete"].Position = 0;
			grdHistory.RootTable.Columns.Add("Voucher");
			grdHistory.RootTable.Columns["Voucher"].Key = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].Width = 60;
			grdHistory.RootTable.Columns["Voucher"].Position = 1;
			grdHistory.RootTable.Columns.Add("Print");
			grdHistory.RootTable.Columns["Print"].Key = "Print";
			grdHistory.RootTable.Columns["Print"].Caption = "Ref Print";
			grdHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Print"].ButtonText = "Ref Print";
			grdHistory.RootTable.Columns["Print"].Width = 60;
			grdHistory.RootTable.Columns["Print"].Position = 2;
			grdHistory.FrozenColumns = 3;
			ctrlGrdBar1_Load(null, null);
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
			header.DocumentTypeId = 1850;
			header.FromDate = Conversion.ToDateTime((object)txtFromdateHistory.Value);
			header.ToDate = Conversion.ToDateTime((object)txtToDateHistory.Value);
			header.FromDocNo = Conversion.ToInt((object)txtFromNoHistory.Text.Trim());
			header.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text.Trim());
			header.Id = Conversion.ToInt(cmbProductionNoHistory.Value);
			header.BOMHeaderId = Conversion.ToInt(CmbBomNoHistory.Value);
			header.ItemId = Conversion.ToInt(cmbItemhistory.Value);
			header.PlantId = Conversion.ToInt(CmbPlant.Value);
			header.ProductionType = Conversion.ToInt(CmbProductionType.Value);
			dtHistory = ProductionHeader.PendingProductionsForApproval(header);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("DocDate", typeof(DateTime));
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("DocumentTypeId", typeof(int));
				dtcol.Columns.Add("RefDoNo", typeof(string));
				dtcol.Columns.Add("ProductionDate", typeof(DateTime));
				dtcol.Columns.Add("ProductionNo", typeof(string));
				dtcol.Columns.Add("ProductionType", typeof(string));
				dtcol.Columns.Add("PlantName", typeof(string));
				dtcol.Columns.Add("BomHeaderNo", typeof(string));
				dtcol.Columns.Add("ItemName", typeof(string));
				dtcol.Columns.Add("ItemCode", typeof(string));
				dtcol.Columns.Add("UOM", typeof(string));
				dtcol.Columns.Add("ItemQty", typeof(decimal));
				dtcol.Columns.Add("WipWareHouseName", typeof(string));
				dtcol.Columns.Add("WIPAccountTitle", typeof(string));
				dtcol.Columns.Add("RemarksHeader", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("EntryUserName", typeof(string));
				dtcol.Columns.Add("ModifyDate", typeof(DateTime));
				dtcol.Columns.Add("ModifyUserName", typeof(string));
				dtcol.Columns.Add("NoOfAttachments", typeof(int));
				dtcol.Rows.Clear();
				foreach (DataRow row in dtHistory.Rows)
				{
					dtcol.Rows.Add(DateTime.Now.ToShortDateString(), row["Id"], row["DocumentTypeId"], row["DocNo"], row["DocDate"], row["ProductionNo"], row["ProductionType"], row["PlantName"], row["BomHeaderNo"], row["ItemName"], row["ItemCode"], row["UOM"], row["ItemQty"], row["WareHouseName"], row["WIPAccountTitle"], row["RemarksHeader"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["NoOfAttachments"]);
				}
				grd.DataSource = dtcol;
				grd.RetrieveStructure();
				HistoryGridSetting();
			}
			else
			{
				grd.DataSource = null;
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
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["DocumentTypeId"].Visible = false;
			grd.RootTable.Columns["Id"].EditType = (EditType)0;
			grd.RootTable.Columns["DocumentTypeId"].EditType = (EditType)0;
			grd.RootTable.Columns["RefDoNo"].EditType = (EditType)0;
			grd.RootTable.Columns["ProductionDate"].EditType = (EditType)0;
			grd.RootTable.Columns["ProductionNo"].EditType = (EditType)0;
			grd.RootTable.Columns["ProductionType"].EditType = (EditType)0;
			grd.RootTable.Columns["PlantName"].EditType = (EditType)0;
			grd.RootTable.Columns["BomHeaderNo"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grd.RootTable.Columns["UOM"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemQty"].EditType = (EditType)0;
			grd.RootTable.Columns["WipWareHouseName"].EditType = (EditType)0;
			grd.RootTable.Columns["WIPAccountTitle"].EditType = (EditType)0;
			grd.RootTable.Columns["RemarksHeader"].EditType = (EditType)0;
			grd.RootTable.Columns["EntryDate"].EditType = (EditType)0;
			grd.RootTable.Columns["EntryUserName"].EditType = (EditType)0;
			grd.RootTable.Columns["ModifyDate"].EditType = (EditType)0;
			grd.RootTable.Columns["ModifyUserName"].EditType = (EditType)0;
			grd.RootTable.Columns["NoOfAttachments"].EditType = (EditType)0;
			grd.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["DocumentTypeId"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["RefDoNo"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["ProductionDate"].FilterEditType = (FilterEditType)6;
			grd.RootTable.Columns["ProductionNo"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["ProductionType"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["PlantName"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["BomHeaderNo"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["ItemName"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["ItemCode"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["UOM"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["ItemQty"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["WipWareHouseName"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["WIPAccountTitle"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["RemarksHeader"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["EntryDate"].FilterEditType = (FilterEditType)6;
			grd.RootTable.Columns["EntryUserName"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["ModifyDate"].FilterEditType = (FilterEditType)6;
			grd.RootTable.Columns["ModifyUserName"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["NoOfAttachments"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemQty"].FormatString = "##,#.##";
			grd.RootTable.Columns["ItemQty"].TotalFormatString = "##,#.##";
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns.Add("Select");
			grd.RootTable.Columns["Select"].ActAsSelector = true;
			grd.RootTable.Columns["Select"].UseHeaderSelector = true;
			grd.RootTable.Columns["Select"].Width = 40;
			grd.RootTable.Columns["Select"].Position = 0;
			grd.RootTable.Columns["DocDate"].Position = 1;
			grd.RootTable.Columns.Add("Print");
			grd.RootTable.Columns["Print"].Key = "Print";
			grd.RootTable.Columns["Print"].Caption = "Print";
			grd.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Print"].ButtonText = "Print";
			grd.RootTable.Columns["Print"].Width = 50;
			grd.RootTable.Columns["Print"].Position = 2;
			grd.FrozenColumns = 3;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProductionSlip(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = PrintId;
			DataTable dt = ProductionHeader.Production_SlipAndRegister(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(ProductionHeader.ProductionAllDetails_SubReport(obj.Id), "ProductionAllDetails_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1850_Production_Slip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Print")
			{
				ProductionSlip(Conversion.ToInt(grd.CurrentRow.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(grd.CurrentRow.Cells["Id"].Value), 1850);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				grdDetailbind(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailbind(int Id)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		try
		{
			grdHistoryInput.ClearStructure();
			grdHistoryOutput.ClearStructure();
			ProductionHeader ProductionHeader = new ProductionHeader();
			ProductionHeader = ProductionHeader.ReadById(Id);
			if (ProductionHeader == null)
			{
				return;
			}
			DataTable dtInputHistory = new DataTable();
			DataTable dtOutPutByProductHistory = new DataTable();
			dtInputHistory.Columns.Add("Id", typeof(int));
			dtInputHistory.Columns.Add("WarehouseName", typeof(string));
			dtInputHistory.Columns.Add("JobLot", typeof(string));
			dtInputHistory.Columns.Add("ItemId", typeof(int));
			dtInputHistory.Columns.Add("ItemName", typeof(string));
			dtInputHistory.Columns.Add("Uom", typeof(string));
			dtInputHistory.Columns.Add("ItemQty", typeof(decimal));
			dtInputHistory.Columns.Add("NetWeight", typeof(decimal));
			dtInputHistory.Columns.Add("Rate", typeof(decimal));
			dtInputHistory.Columns.Add("RateUom", typeof(string));
			dtInputHistory.Columns.Add("Amount", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("Id", typeof(int));
			dtOutPutByProductHistory.Columns.Add("EntryType", typeof(string));
			dtOutPutByProductHistory.Columns.Add("SaleOrderId", typeof(int));
			dtOutPutByProductHistory.Columns.Add("SaleOrder", typeof(int));
			dtOutPutByProductHistory.Columns.Add("WarehouseName", typeof(string));
			dtOutPutByProductHistory.Columns.Add("JobLot", typeof(string));
			dtOutPutByProductHistory.Columns.Add("ItemId", typeof(int));
			dtOutPutByProductHistory.Columns.Add("ItemName", typeof(string));
			dtOutPutByProductHistory.Columns.Add("AttributeVarient", typeof(string));
			dtOutPutByProductHistory.Columns.Add("VarientUnit", typeof(string));
			dtOutPutByProductHistory.Columns.Add("Uom", typeof(string));
			dtOutPutByProductHistory.Columns.Add("ItemQty", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("VarientQty", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("NetWeight", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("Rate", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("RateUom", typeof(string));
			dtOutPutByProductHistory.Columns.Add("Amount", typeof(decimal));
			if (ProductionHeader.ProductionInputDetailList.Count > 0)
			{
				for (int j = 0; j < ProductionHeader.ProductionInputDetailList.Count; j++)
				{
					dtInputHistory.Rows.Add(ProductionHeader.ProductionInputDetailList[j].Id, ProductionHeader.ProductionInputDetailList[j].WareHouseName, ProductionHeader.ProductionInputDetailList[j].JobLotDescription, ProductionHeader.ProductionInputDetailList[j].ItemId, ProductionHeader.ProductionInputDetailList[j].ItemName, ProductionHeader.ProductionInputDetailList[j].ItemUomCode, ProductionHeader.ProductionInputDetailList[j].ItemQty, ProductionHeader.ProductionInputDetailList[j].NetWeight, ProductionHeader.ProductionInputDetailList[j].Rate, ProductionHeader.ProductionInputDetailList[j].RateUomCode, ProductionHeader.ProductionInputDetailList[j].Amount);
				}
			}
			else
			{
				grdHistoryInput.ClearStructure();
			}
			if (ProductionHeader.ProductionOutPutDetailList.Count > 0)
			{
				for (int i = 0; i < ProductionHeader.ProductionOutPutDetailList.Count; i++)
				{
					dtOutPutByProductHistory.Rows.Add(ProductionHeader.ProductionOutPutDetailList[i].Id, ProductionHeader.ProductionOutPutDetailList[i].EntryType, ProductionHeader.ProductionOutPutDetailList[i].SaleOrderId, ProductionHeader.ProductionOutPutDetailList[i].SaleOrderNo, ProductionHeader.ProductionOutPutDetailList[i].WareHouseName, ProductionHeader.ProductionOutPutDetailList[i].JobLotDescription, ProductionHeader.ProductionOutPutDetailList[i].ItemId, ProductionHeader.ProductionOutPutDetailList[i].ItemName, ProductionHeader.ProductionOutPutDetailList[i].ItemAttribute, ProductionHeader.ProductionOutPutDetailList[i].VarientEquivalent, ProductionHeader.ProductionOutPutDetailList[i].ItemUomCode, ProductionHeader.ProductionOutPutDetailList[i].ItemQty, ProductionHeader.ProductionOutPutDetailList[i].VarientQty, ProductionHeader.ProductionOutPutDetailList[i].NetWeight, ProductionHeader.ProductionOutPutDetailList[i].Rate, ProductionHeader.ProductionOutPutDetailList[i].RateUomCode, ProductionHeader.ProductionOutPutDetailList[i].Amount);
				}
			}
			else
			{
				grdHistoryOutput.ClearStructure();
			}
			grdHistoryInput.DataSource = dtInputHistory;
			grdHistoryInput.RetrieveStructure();
			grdHistoryInputSetting();
			grdHistoryOutput.DataSource = dtOutPutByProductHistory;
			grdHistoryOutput.RetrieveStructure();
			grdHistoryOutputSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryInputSetting()
	{
		try
		{
			grdHistoryInput.RootTable.Columns["Id"].Visible = false;
			grdHistoryInput.RootTable.Columns["ItemId"].Visible = false;
			grdHistoryInput.RootTable.Columns["NetWeight"].Visible = false;
			grdHistoryInput.RootTable.Columns["JobLot"].Visible = false;
			grdHistoryInput.RootTable.Columns["RateUom"].Visible = false;
			grdHistoryInput.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryInput.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdHistoryInput.RootTable.Columns["ItemQty"].FormatString = "##,#.##";
			grdHistoryInput.RootTable.Columns["ItemQty"].TotalFormatString = "##,#.##";
			grdHistoryInput.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)2;
			grdHistoryInput.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdHistoryInput.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryInput.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryInput.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryInput.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryInput.RootTable.Columns["NetWeight"].FormatString = "##,#.##";
			grdHistoryInput.RootTable.Columns["NetWeight"].TotalFormatString = "##,#.##";
			grdHistoryInput.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdHistoryInput.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryInput.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistoryInput.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdHistoryInput);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryOutputSetting()
	{
		try
		{
			grdHistoryOutput.RootTable.Columns["Id"].Visible = false;
			grdHistoryOutput.RootTable.Columns["ItemId"].Visible = false;
			grdHistoryOutput.RootTable.Columns["EntryType"].Visible = false;
			grdHistoryOutput.RootTable.Columns["SaleOrderId"].Visible = false;
			grdHistoryOutput.RootTable.Columns["NetWeight"].Visible = false;
			grdHistoryOutput.RootTable.Columns["RateUom"].Visible = false;
			grdHistoryOutput.RootTable.Columns["Uom"].Visible = false;
			grdHistoryOutput.RootTable.Columns["JobLot"].Visible = false;
			grdHistoryOutput.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOutput.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdHistoryOutput.RootTable.Columns["ItemQty"].FormatString = "##,#.##";
			grdHistoryOutput.RootTable.Columns["ItemQty"].TotalFormatString = "##,#.##";
			grdHistoryOutput.RootTable.Columns["VarientQty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOutput.RootTable.Columns["VarientQty"].TextAlignment = (TextAlignment)3;
			grdHistoryOutput.RootTable.Columns["VarientQty"].FormatString = "##,#.##";
			grdHistoryOutput.RootTable.Columns["VarientQty"].TotalFormatString = "##,#.##";
			grdHistoryOutput.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOutput.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdHistoryOutput.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryOutput.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryOutput.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryOutput.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOutput.RootTable.Columns["NetWeight"].FormatString = "##,#.##";
			grdHistoryOutput.RootTable.Columns["NetWeight"].TotalFormatString = "##,#.##";
			grdHistoryOutput.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdHistoryOutput.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOutput.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistoryOutput.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdHistoryOutput);
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
			cmbDateTypeHistory.Value = 3;
			txtFromNoHistory.Clear();
			txtToDocNoHistory.Clear();
			((Control)(object)CmbPlant).Text = string.Empty;
			((Control)(object)CmbProductionType).Text = string.Empty;
			((Control)(object)CmbBomNoHistory).Text = string.Empty;
			((Control)(object)cmbProductionNoHistory).Text = string.Empty;
			((Control)(object)cmbItemhistory).Text = string.Empty;
			grd.ClearStructure();
			grdHistoryOutput.ClearStructure();
			grdHistoryInput.ClearStructure();
			cmbDateTypeHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmBillOfMaterial_Manufacturing_KeyDown(object sender, KeyEventArgs e)
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
					txtFromdateHistory.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
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
			if (tabControl1.SelectedIndex != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
			}
			else if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.Up)
			{
				((Control)(object)grdHistoryOutput).Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)grdHistoryOutput).Focused)
				{
					((Control)(object)grdHistoryInput).Focus();
				}
				else if (((Control)(object)grdHistoryInput).Focused)
				{
					((Control)(object)grdHistoryOutput).Focus();
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
			dt.Rows.Add("Ctrl+S", "For Show Data History Form");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Update Record");
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			int Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grd.CurrentColumn.Key == "Print")
				{
					ProductionSlip(Id);
				}
				if (grd.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, 1850);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void btnVoucherReport_Click(object sender, EventArgs e)
	{
		try
		{
			VoucherReprot(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherReprot(int Id)
	{
		try
		{
			CommonServices.VoucherReport_118(Conversion.ToInt((object)CommonServices.VoucherHeadIdGet(Id, 1857)), 1857);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		try
		{
			if (grd.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ProductionRelease obj = new ProductionRelease();
				obj.ProductionReleaseslist = new List<ProductionRelease>();
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					ProductionRelease item = new ProductionRelease();
					item.OrganizationId = UserAccount.OrganizationId;
					item.CompanyId = UserAccount.CompanyId;
					item.BranchesId = UserAccount.BranchesId;
					item.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
					item.DocumentTypeId = 1857;
					item.RefDocId = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
					item.RefDocumentTypeId = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
					item.DocDate = Conversion.ToDateTime(r.Cells["DocDate"].Value);
					item.EntryUserId = UserAccount.ID;
					item.ModifyUserId = UserAccount.ID;
					item.EntryDate = DateTime.Now;
					item.ModifyDate = DateTime.Now;
					item.ActionId = 1;
					obj.ProductionReleaseslist.Add(item);
				}
				ProductionRelease.Save(obj);
				MessageBox.Show("Record Save Successfully");
				HistoryGridFill();
			}
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
			AllCombobind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Print")
			{
				ProductionSlip(Conversion.ToInt(grdHistory.CurrentRow.Cells["RefDocId"].Value));
			}
			else if (e.Column.Key == "Voucher")
			{
				VoucherReprot(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			}
			else if (e.Column.Key == "Delete" && MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				int Id = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
				ProductionRelease.DeleteByID(UserAccount.ID, Id);
				MessageBox.Show("Record Deleted Successfully.", "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				HistoryBind();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnHistoryShow_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProductionRelease_FormClosed(object sender, FormClosedEventArgs e)
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
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Expected O, but got Unknown
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Expected O, but got Unknown
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Expected O, but got Unknown
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Expected O, but got Unknown
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Expected O, but got Unknown
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Expected O, but got Unknown
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Expected O, but got Unknown
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Expected O, but got Unknown
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Expected O, but got Unknown
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Expected O, but got Unknown
		//IL_0e24: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2e: Expected O, but got Unknown
		//IL_0e52: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5c: Expected O, but got Unknown
		//IL_3778: Unknown result type (might be due to invalid IL or missing references)
		//IL_3782: Expected O, but got Unknown
		GridEXLayout grdHistoryInput_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryOutput_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
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
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.FormTab = new System.Windows.Forms.TabPage();
		this.panel45 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdHistoryInput = new GridEX();
		this.panel44 = new System.Windows.Forms.Panel();
		this.label14 = new System.Windows.Forms.Label();
		this.panel19 = new System.Windows.Forms.Panel();
		this.grdHistoryOutput = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.grd = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label17 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbProductionType = new UltraCombo();
		this.label40 = new System.Windows.Forms.Label();
		this.CmbPlant = new UltraCombo();
		this.label34 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.cmbItemhistory = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.cmbProductionNoHistory = new UltraCombo();
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
		this.panel38 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.label32 = new System.Windows.Forms.Label();
		this.HistoryTab = new System.Windows.Forms.TabPage();
		this.grdHistory = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.btnHistoryShow = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.ToDate = new System.Windows.Forms.DateTimePicker();
		this.label1 = new System.Windows.Forms.Label();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.label21 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.FormTab.SuspendLayout();
		this.panel45.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryInput).BeginInit();
		this.panel44.SuspendLayout();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryOutput).BeginInit();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel15.SuspendLayout();
		this.panel13.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlant).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemhistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbProductionNoHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBomNoHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).BeginInit();
		this.panel38.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.HistoryTab.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel2.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.panel3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1184, 624);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.FormTab);
		this.tabControl1.Controls.Add(this.HistoryTab);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1184, 624);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.FormTab.Controls.Add(this.panel45);
		this.FormTab.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.FormTab.Controls.Add(this.panel15);
		this.FormTab.Controls.Add(this.panel13);
		this.FormTab.Controls.Add(this.panel38);
		this.FormTab.Location = new System.Drawing.Point(4, 4);
		this.FormTab.Name = "FormTab";
		this.FormTab.Size = new System.Drawing.Size(1176, 595);
		this.FormTab.TabIndex = 2;
		this.FormTab.Text = "Form";
		this.FormTab.UseVisualStyleBackColor = true;
		this.panel45.Controls.Add(this.panel8);
		this.panel45.Controls.Add(this.panel19);
		this.panel45.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel45.Location = new System.Drawing.Point(0, 436);
		this.panel45.Name = "panel45";
		this.panel45.Size = new System.Drawing.Size(1176, 159);
		this.panel45.TabIndex = 55;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryInput);
		this.panel8.Controls.Add(this.panel44);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(645, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(531, 159);
		this.panel8.TabIndex = 50;
		this.grdHistoryInput.AllowEdit = (InheritableBoolean)2;
		grdHistoryInput_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryInput.DesignTimeLayout = grdHistoryInput_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryInput.DynamicFiltering = true;
		this.grdHistoryInput.FilterMode = (FilterMode)2;
		this.grdHistoryInput.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryInput.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Location = new System.Drawing.Point(0, 19);
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Name = "grdHistoryInput";
		((ScrollableBase)this.grdHistoryInput).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Size = new System.Drawing.Size(531, 140);
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).TabIndex = 0;
		this.grdHistoryInput.TotalRow = (InheritableBoolean)1;
		this.grdHistoryInput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryInput).VisualStyle = (VisualStyle)4;
		this.panel44.BackColor = System.Drawing.Color.Teal;
		this.panel44.Controls.Add(this.label14);
		this.panel44.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel44.Location = new System.Drawing.Point(0, 0);
		this.panel44.Name = "panel44";
		this.panel44.Size = new System.Drawing.Size(531, 19);
		this.panel44.TabIndex = 49;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(2, 1);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(222, 16);
		this.label14.TabIndex = 128;
		this.label14.Text = "Input Material (Consumed Items)";
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryOutput);
		this.panel19.Controls.Add(this.panel12);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel19.Location = new System.Drawing.Point(0, 0);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(645, 159);
		this.panel19.TabIndex = 4;
		this.grdHistoryOutput.AllowEdit = (InheritableBoolean)2;
		grdHistoryOutput_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryOutput.DesignTimeLayout = grdHistoryOutput_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryOutput.DynamicFiltering = true;
		this.grdHistoryOutput.FilterMode = (FilterMode)2;
		this.grdHistoryOutput.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryOutput.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Location = new System.Drawing.Point(0, 19);
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Name = "grdHistoryOutput";
		((ScrollableBase)this.grdHistoryOutput).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Size = new System.Drawing.Size(645, 140);
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).TabIndex = 0;
		this.grdHistoryOutput.TotalRow = (InheritableBoolean)1;
		this.grdHistoryOutput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryOutput).VisualStyle = (VisualStyle)4;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label15);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(645, 19);
		this.panel12.TabIndex = 52;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(6, 1);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(170, 16);
		this.label15.TabIndex = 127;
		this.label15.Text = "OutPut (Produced Items)";
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Top;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 185);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1176, 251);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		this.grd.SelectionChanged += new System.EventHandler(grd_SelectionChanged);
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.ctrlGrdBar1);
		this.panel15.Controls.Add(this.label17);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 162);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1176, 23);
		this.panel15.TabIndex = 53;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = null;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1129, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(9, 6, 9, 6);
		this.ctrlGrdBar1.MyGrid = null;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(42, 22);
		this.ctrlGrdBar1.TabIndex = 128;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(8, 3);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(54, 16);
		this.label17.TabIndex = 127;
		this.label17.Text = "History";
		this.panel13.BackColor = System.Drawing.Color.White;
		this.panel13.Controls.Add(this.groupBox1);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 56);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1176, 106);
		this.panel13.TabIndex = 0;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbProductionType);
		this.groupBox1.Controls.Add(this.label40);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPlant);
		this.groupBox1.Controls.Add(this.label34);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemhistory);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbProductionNoHistory);
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
		this.groupBox1.Location = new System.Drawing.Point(5, 1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(915, 102);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.CmbProductionType.AllowNull = (DefaultableBoolean)1;
		this.CmbProductionType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbProductionType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbProductionType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbProductionType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Location = new System.Drawing.Point(416, 44);
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Name = "CmbProductionType";
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Size = new System.Drawing.Size(195, 26);
		((System.Windows.Forms.Control)(object)this.CmbProductionType).TabIndex = 6;
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(324, 50);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(94, 15);
		this.label40.TabIndex = 184;
		this.label40.Text = "Production Type";
		this.CmbPlant.AllowNull = (DefaultableBoolean)1;
		this.CmbPlant.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPlant.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPlant.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPlant).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbPlant).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPlant).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlant).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlant).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlant).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlant).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbPlant).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPlant).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPlant).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPlant).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbPlant).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPlant).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlant).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPlant).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbPlant).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPlant).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlant).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPlant).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbPlant).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPlant).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPlant).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbPlant).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPlant).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbPlant).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPlant).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPlant).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPlant).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPlant.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPlant).Location = new System.Drawing.Point(416, 16);
		((System.Windows.Forms.Control)(object)this.CmbPlant).Name = "CmbPlant";
		((System.Windows.Forms.Control)(object)this.CmbPlant).Size = new System.Drawing.Size(195, 26);
		((System.Windows.Forms.Control)(object)this.CmbPlant).TabIndex = 5;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(324, 22);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(69, 15);
		this.label34.TabIndex = 182;
		this.label34.Text = "Plant Name";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(845, 73);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(62, 25);
		this.btnshow.TabIndex = 10;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.cmbItemhistory.AllowNull = (DefaultableBoolean)1;
		this.cmbItemhistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemhistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemhistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemhistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemhistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemhistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemhistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemhistory).Location = new System.Drawing.Point(706, 44);
		((System.Windows.Forms.Control)(object)this.cmbItemhistory).Name = "cmbItemhistory";
		((System.Windows.Forms.Control)(object)this.cmbItemhistory).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemhistory).TabIndex = 9;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(618, 50);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(67, 15);
		this.label29.TabIndex = 180;
		this.label29.Text = "Item Name";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(618, 22);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(85, 15);
		this.label30.TabIndex = 178;
		this.label30.Text = "Production No";
		this.cmbProductionNoHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbProductionNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbProductionNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbProductionNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbProductionNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbProductionNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbProductionNoHistory).Location = new System.Drawing.Point(706, 16);
		((System.Windows.Forms.Control)(object)this.cmbProductionNoHistory).Name = "cmbProductionNoHistory";
		((System.Windows.Forms.Control)(object)this.cmbProductionNoHistory).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.cmbProductionNoHistory).TabIndex = 8;
		this.CmbBomNoHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbBomNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBomNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBomNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBomNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBomNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBomNoHistory).Location = new System.Drawing.Point(416, 71);
		((System.Windows.Forms.Control)(object)this.CmbBomNoHistory).Name = "CmbBomNoHistory";
		((System.Windows.Forms.Control)(object)this.CmbBomNoHistory).Size = new System.Drawing.Size(195, 26);
		((System.Windows.Forms.Control)(object)this.CmbBomNoHistory).TabIndex = 7;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(324, 77);
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
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance60;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance65).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance68).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance70;
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
		this.txtToDocNoHistory.Location = new System.Drawing.Point(210, 73);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.Size = new System.Drawing.Size(111, 23);
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
		this.label35.Location = new System.Drawing.Point(5, 77);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(88, 15);
		this.label35.TabIndex = 173;
		this.label35.Text = "Doc From && To";
		this.txtFromNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromNoHistory.Location = new System.Drawing.Point(97, 73);
		this.txtFromNoHistory.Name = "txtFromNoHistory";
		this.txtFromNoHistory.Size = new System.Drawing.Size(111, 23);
		this.txtFromNoHistory.TabIndex = 3;
		this.panel38.BackColor = System.Drawing.Color.Teal;
		this.panel38.Controls.Add(this.toolStrip2);
		this.panel38.Controls.Add(this.label32);
		this.panel38.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel38.Location = new System.Drawing.Point(0, 0);
		this.panel38.Name = "panel38";
		this.panel38.Size = new System.Drawing.Size(1176, 56);
		this.panel38.TabIndex = 48;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNewHistory, this.toolStripSeparator1, this.btnRefresh, this.btnSave });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1176, 27);
		this.toolStrip2.TabIndex = 3;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnHistoryRefresh_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.White;
		this.label32.Location = new System.Drawing.Point(5, 31);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(167, 19);
		this.label32.TabIndex = 127;
		this.label32.Text = "Production Release";
		this.HistoryTab.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.HistoryTab.Controls.Add(this.panel2);
		this.HistoryTab.Controls.Add(this.panel3);
		this.HistoryTab.Location = new System.Drawing.Point(4, 4);
		this.HistoryTab.Name = "HistoryTab";
		this.HistoryTab.Padding = new System.Windows.Forms.Padding(3);
		this.HistoryTab.Size = new System.Drawing.Size(1176, 595);
		this.HistoryTab.TabIndex = 0;
		this.HistoryTab.Text = "History";
		this.HistoryTab.UseVisualStyleBackColor = true;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(3, 130);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1170, 462);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 2;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.panel2.Controls.Add(this.groupBox2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 58);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1170, 72);
		this.panel2.TabIndex = 1;
		this.groupBox2.Controls.Add(this.btnHistoryShow);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.ToDate);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.FromDate);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(5, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(330, 65);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.btnHistoryShow.BackColor = System.Drawing.Color.Teal;
		this.btnHistoryShow.FlatAppearance.BorderSize = 0;
		this.btnHistoryShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnHistoryShow.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnHistoryShow.ForeColor = System.Drawing.Color.White;
		this.btnHistoryShow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnHistoryShow.Location = new System.Drawing.Point(234, 35);
		this.btnHistoryShow.Margin = new System.Windows.Forms.Padding(2);
		this.btnHistoryShow.Name = "btnHistoryShow";
		this.btnHistoryShow.Size = new System.Drawing.Size(62, 25);
		this.btnHistoryShow.TabIndex = 176;
		this.btnHistoryShow.Text = "Show";
		this.btnHistoryShow.UseVisualStyleBackColor = false;
		this.btnHistoryShow.Click += new System.EventHandler(btnHistoryShow_Click);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(116, 19);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 15);
		this.label2.TabIndex = 175;
		this.label2.Text = "To Date";
		this.ToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.CustomFormat = "dd-MMM-yy";
		this.ToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDate.Location = new System.Drawing.Point(119, 36);
		this.ToDate.Name = "ToDate";
		this.ToDate.Size = new System.Drawing.Size(111, 23);
		this.ToDate.TabIndex = 173;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(6, 19);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 174;
		this.label1.Text = "From Date";
		this.FromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.CustomFormat = "dd-MMM-yy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(6, 36);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(111, 23);
		this.FromDate.TabIndex = 172;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Controls.Add(this.label21);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1170, 55);
		this.panel3.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1170, 25);
		this.toolStrip1.TabIndex = 2;
		this.toolStrip1.Text = "toolStrip1";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.White;
		this.label21.Location = new System.Drawing.Point(7, 30);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(230, 19);
		this.label21.TabIndex = 126;
		this.label21.Text = "Production Release History";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1184, 624);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "ProductionRelease";
		this.Text = "Concrete Production";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(ProductionRelease_FormClosed);
		base.Load += new System.EventHandler(frmBillOfMaterial_Manufacturing_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmBillOfMaterial_Manufacturing_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.FormTab.ResumeLayout(false);
		this.panel45.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryInput).EndInit();
		this.panel44.ResumeLayout(false);
		this.panel44.PerformLayout();
		this.panel19.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryOutput).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel13.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlant).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemhistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbProductionNoHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBomNoHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).EndInit();
		this.panel38.ResumeLayout(false);
		this.panel38.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.HistoryTab.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel2.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
