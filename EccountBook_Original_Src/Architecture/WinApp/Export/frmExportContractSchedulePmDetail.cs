using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.BLL.MRP;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.WinApp.Common;
using Architecture.WinApp.PackingMaterial_Store;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Export;

public class frmExportContractSchedulePmDetail : Form
{
	private Rightsobjects formrights = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private int ContractId;

	private int RecId;

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool FormInitialized;

	private DataTable dtPmDetail = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private DataTable dtPMItem = new DataTable();

	private DataTable dtItemAndPMItemMap = new DataTable();

	private DataTable dtcol = new DataTable();

	private List<ExImScheduleDetailWithPm> lstRemoveRecordInput = new List<ExImScheduleDetailWithPm>();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private TabPage tabPage2;

	private ToolStrip toolstrip;

	private ToolStripButton btnnew;

	private ToolStripButton Print;

	private Panel panel17;

	private ToolStrip toolStrip3;

	private ToolStripButton btnUpdate;

	private ToolStripButton BtnRefresh;

	private Panel panel5;

	private TabControl tabControl2;

	private CtrlGrdBar ctrlGrdBar1;

	private TabPage tabPackingMaterialDetail;

	private Label label66;

	private ToolStripButton btnShortCutKey;

	private Label label52;

	internal GridEX DataGridHistory;

	private Panel panel29;

	private Label label65;

	private Panel panel18;

	private GroupBox groupBox5;

	private RadioButton rdapproveddate;

	private Button btnShowHistory;

	private RadioButton rdmodifydate;

	private Label label67;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Label label90;

	private Label label91;

	internal UltraCombo CmbCustomerHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	private CheckBox ChkPreview;

	private TabControl tabControl3;

	private TabPage tabPmH;

	internal GridEX grdPMHistory;

	private Panel panel32;

	private Label label75;

	private Panel panel16;

	private Label label56;

	internal GridEX grdPackingMaterialDetail;

	private TabPage tabOtherItems;

	internal GridEX grdotheritems;

	private DateTimePicker datFromLaodingDate;

	private RadioButton rdUnMatched;

	private RadioButton rdMatched;

	private Label label1;

	private RadioButton rdAll;

	internal GridEX grdScheduleInfo;

	private Panel panel7;

	private Panel panel6;

	private Label label2;

	private Button btnShow;

	private ToolStripButton btnAutoQtyGenerate;

	private ToolStripButton btnPackingMaterialRequirementPlanning;

	private Button btnAutoMap;

	public frmExportContractSchedulePmDetail(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmExportContractSchedulePmDetail_Load(object sender, EventArgs e)
	{
		try
		{
			FormInitialized = true;
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 700);
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnUpdate.Enabled = true;
			ctrlGrdBar1.mGridChooseFielder.Visible = false;
			ctrlGrdBar1.mGridSaveLayouts.Visible = false;
			ctrlGrdBar1.removeLayoutToolStripMenuItem.Visible = false;
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			dtPMItem.Columns.Add("Id", typeof(int));
			dtPMItem.Columns.Add("ItemName");
			dtPMItem.Columns.Add("ItemCode");
			PackingItemFill();
			dtPmDetail.Columns.Add("ContractId", typeof(int));
			dtPmDetail.Columns.Add("ScheduleId", typeof(int));
			dtPmDetail.Columns.Add("Id", typeof(int));
			dtPmDetail.Columns.Add("BrandId", typeof(int));
			dtPmDetail.Columns.Add("BrandCode");
			dtPmDetail.Columns.Add("BrandName");
			dtPmDetail.Columns.Add("BrandUomId", typeof(int));
			dtPmDetail.Columns.Add("BrandUom");
			dtPmDetail.Columns.Add("QtyEquivalent", typeof(double));
			dtPmDetail.Columns.Add("PackingTypeId", typeof(int));
			dtPmDetail.Columns.Add("PackingType");
			dtPmDetail.Columns.Add("BrandOuterQty", typeof(double));
			dtPmDetail.Columns.Add("BrandInnerQty", typeof(double));
			dtPmDetail.Columns.Add("PmItemId", typeof(int));
			dtPmDetail.Columns.Add("PmItemCode");
			dtPmDetail.Columns.Add("PmItemName");
			dtPmDetail.Columns.Add("PmItemUomId", typeof(int));
			dtPmDetail.Columns.Add("PmItemUom");
			dtPmDetail.Columns.Add("PmItemOuterQty", typeof(double));
			dtPmDetail.Columns.Add("PmItemInnerQty", typeof(double));
			dtPmDetail.Columns.Add("Remarks");
			dtPmDetail.Columns.Add("Status");
			grdPackingMaterialDetail.DataSource = dtPmDetail;
			grdSettingsPM();
			dtOtherItem.Columns.Add("Id");
			dtOtherItem.Columns.Add("ContractId");
			dtOtherItem.Columns.Add("ContractOtherItemId");
			dtOtherItem.Columns.Add("ItemId");
			dtOtherItem.Columns.Add("ItemName");
			dtOtherItem.Columns.Add("Qty", typeof(double));
			dtOtherItem.Columns.Add("Remarks");
			grdotheritems.DataSource = dtOtherItem;
			grdSettingsOtherItems();
			dtItemAndPMItemMap = ItemAndPMItemMap.ItemAndPMItemMap_GetByItemIds(UserAccount.CompanyId, UserAccount.OrganizationId, (string)null);
			tabControl2.SelectedIndex = 0;
			HistoryCombosFill();
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingItemFill()
	{
		try
		{
			dtPMItem.Rows.Clear();
			DataTable dt = CommonServices.GetItemByItemTypeId("14");
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtPMItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCodeNew"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCancelDepartmentDetail_Click(object sender, EventArgs e)
	{
		try
		{
			ScheduleInforFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ScheduleInforFill()
	{
		try
		{
			string Allstatus = "";
			bool status = rdMatched.Checked;
			if (rdAll.Checked)
			{
				Allstatus = "All";
			}
			DataTable dt = ExImLcOrderShipmentSchedule.GetContractScheduleNo(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToDateTime((object)datFromLaodingDate.Value), status, Allstatus);
			if (dt.Rows.Count > 0)
			{
				DataTable dtSch = new DataTable();
				dtSch.Columns.Add("Id", typeof(int));
				dtSch.Columns.Add("ScheduleNo", typeof(string));
				dtSch.Columns.Add("ContractId", typeof(int));
				dtSch.Columns.Add("LoadingDate", typeof(DateTime));
				dtSch.Columns.Add("FCL", typeof(double));
				dtSch.Columns.Add("MTon", typeof(double));
				dtSch.Columns.Add("Status", typeof(string));
				foreach (DataRow row in dt.Rows)
				{
					dtSch.Rows.Add(row["Id"], row["ScheduleCode"], row["ExImLcOrderId"], row["LoadingDate"], row["FCL"], row["MTon"], row["Status"]);
				}
				grdScheduleInfo.DataSource = dtSch;
				grdScheduleSetting();
			}
			else
			{
				grdScheduleInfo.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdScheduleSetting()
	{
		grdScheduleInfo.RootTable.Columns["Id"].Visible = false;
		grdScheduleInfo.RootTable.Columns["ContractId"].Visible = false;
		grdScheduleInfo.RootTable.Columns["ScheduleNo"].ColumnType = (ColumnType)5;
		grdScheduleInfo.RootTable.Columns["LoadingDate"].FormatString = "dd-MMM-yy";
		grdScheduleInfo.RootTable.Columns["LoadingDate"].Caption = "Loading Date";
		grdScheduleInfo.RootTable.Columns["LoadingDate"].TextAlignment = (TextAlignment)2;
		grdScheduleInfo.RootTable.Columns["ScheduleNo"].Width = Constants.ExportConstants.ContractNoConstant;
		grdScheduleInfo.RootTable.Columns["LoadingDate"].Width = Constants.ExportConstants.DateConstant;
		grdScheduleInfo.RootTable.Columns["FCL"].Width = Constants.ExportConstants.FclConstant;
		grdScheduleInfo.RootTable.Columns["MTon"].Width = Constants.ExportConstants.FclConstant;
		grdScheduleInfo.RootTable.Columns["Status"].Width = Constants.ExportConstants.StatusConstant;
	}

	private void grdScheduleInfo_LinkClicked(object sender, EventArgs e)
	{
		try
		{
			RecId = 0;
			ContractId = 0;
			GridEXRow item = grdScheduleInfo.CurrentRow;
			RecId = Conversion.ToInt(item.Cells["Id"].Value);
			ContractId = Conversion.ToInt(item.Cells["ContractId"].Value);
			if (RecId != 0 && (grdPackingMaterialDetail.GetDataRows().Count() <= 0 || MessageBox.Show("Grid already have Records.Do you want To Refresh Data?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No) && !ReadById(RecId, ContractId))
			{
				ExImLcOrderShipmentSchedule.PackingMaterialRequirementSchedule_PlanningAutoInsert(RecId, ContractId);
				ReadById(RecId, ContractId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdotheritems_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdotheritems.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Control && e.KeyCode == Keys.Delete)
				{
					DeleteOtherItemDetailRow(item);
				}
				else if (e.Control && e.KeyCode == Keys.Space && grdotheritems.CurrentColumn.Key == "Delete")
				{
					DeleteOtherItemDetailRow(item);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsOtherItems()
	{
		try
		{
			grdotheritems.RootTable.Columns["Id"].Visible = false;
			grdotheritems.RootTable.Columns["ContractId"].Visible = false;
			grdotheritems.RootTable.Columns["ContractOtherItemId"].Visible = false;
			grdotheritems.RootTable.Columns["ItemId"].Visible = false;
			grdotheritems.RootTable.Columns["Id"].EditType = (EditType)0;
			grdotheritems.RootTable.Columns["ContractId"].EditType = (EditType)0;
			grdotheritems.RootTable.Columns["ContractOtherItemId"].EditType = (EditType)0;
			grdotheritems.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdotheritems.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdotheritems.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdotheritems.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdotheritems.RootTable.Columns["Qty"].FormatString = "0,0";
			grdotheritems.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grdotheritems.RootTable.Columns.Add("Delete");
			grdotheritems.RootTable.Columns["Delete"].Key = "Delete";
			grdotheritems.RootTable.Columns["Delete"].Caption = "X";
			grdotheritems.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdotheritems.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdotheritems.RootTable.Columns["Delete"].ButtonText = "X";
			grdotheritems.RootTable.Columns["Delete"].Width = 20;
			grdotheritems.RootTable.Columns["Delete"].Position = 0;
			grdotheritems.RootTable.Columns.Add("Add");
			grdotheritems.RootTable.Columns["Add"].Key = "Add";
			grdotheritems.RootTable.Columns["Add"].Caption = "+";
			grdotheritems.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdotheritems.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdotheritems.RootTable.Columns["Add"].ButtonText = "+";
			grdotheritems.RootTable.Columns["Add"].Width = 20;
			grdotheritems.RootTable.Columns["Add"].Position = 1;
			grdotheritems.RootTable.Columns["Add"].Visible = false;
			grdotheritems.RootTable.ColumnSets["Add"].Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdotheritems_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdotheritems.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteOtherItemDetailRow(r);
				}
				_ = e.Column.Key == "Add";
				grdotheritems.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteOtherItemDetailRow(GridEXRow r)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
			{
				if (MessageBox.Show("Are you sure To delete?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				r.Delete();
			}
			else
			{
				r.Delete();
			}
			r.GridEX.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsPM()
	{
		try
		{
			grdPackingMaterialDetail.AllowEdit = (InheritableBoolean)1;
			grdPackingMaterialDetail.RootTable.Columns["Id"].Visible = false;
			grdPackingMaterialDetail.RootTable.Columns["ContractId"].Visible = false;
			grdPackingMaterialDetail.RootTable.Columns["ScheduleId"].Visible = false;
			grdPackingMaterialDetail.RootTable.Columns["BrandId"].Visible = false;
			grdPackingMaterialDetail.RootTable.Columns["BrandUomId"].Visible = false;
			grdPackingMaterialDetail.RootTable.Columns["PackingTypeId"].Visible = false;
			grdPackingMaterialDetail.RootTable.Columns["PmItemId"].Visible = false;
			grdPackingMaterialDetail.RootTable.Columns["PmItemId"].Visible = false;
			grdPackingMaterialDetail.RootTable.Columns["PmItemUomId"].Visible = false;
			grdPackingMaterialDetail.RootTable.Columns["PmItemUomId"].Visible = false;
			grdPackingMaterialDetail.RootTable.Columns["BrandCode"].Width = 45;
			grdPackingMaterialDetail.RootTable.Columns["BrandName"].Width = 200;
			grdPackingMaterialDetail.RootTable.Columns["BrandUom"].Width = 100;
			grdPackingMaterialDetail.RootTable.Columns["PackingType"].Width = 90;
			grdPackingMaterialDetail.RootTable.Columns["BrandOuterQty"].Width = 60;
			grdPackingMaterialDetail.RootTable.Columns["QtyEquivalent"].Width = 70;
			grdPackingMaterialDetail.RootTable.Columns["BrandInnerQty"].Width = 60;
			grdPackingMaterialDetail.RootTable.Columns["PmItemCode"].Width = 45;
			grdPackingMaterialDetail.RootTable.Columns["PmItemName"].Width = 200;
			grdPackingMaterialDetail.RootTable.Columns["PmItemUom"].Width = 45;
			grdPackingMaterialDetail.RootTable.Columns["PmItemOuterQty"].Width = 60;
			grdPackingMaterialDetail.RootTable.Columns["PmItemInnerQty"].Width = 60;
			grdPackingMaterialDetail.RootTable.Columns["Remarks"].Width = 200;
			grdPackingMaterialDetail.RootTable.Columns["Status"].Width = 80;
			grdPackingMaterialDetail.RootTable.Columns["Id"].EditType = (EditType)0;
			grdPackingMaterialDetail.RootTable.Columns["BrandId"].EditType = (EditType)0;
			grdPackingMaterialDetail.RootTable.Columns["BrandCode"].EditType = (EditType)0;
			grdPackingMaterialDetail.RootTable.Columns["BrandName"].EditType = (EditType)0;
			grdPackingMaterialDetail.RootTable.Columns["BrandUomId"].EditType = (EditType)0;
			grdPackingMaterialDetail.RootTable.Columns["BrandUom"].EditType = (EditType)0;
			grdPackingMaterialDetail.RootTable.Columns["QtyEquivalent"].EditType = (EditType)0;
			grdPackingMaterialDetail.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
			grdPackingMaterialDetail.RootTable.Columns["PackingType"].EditType = (EditType)0;
			grdPackingMaterialDetail.RootTable.Columns["PmItemId"].EditType = (EditType)0;
			grdPackingMaterialDetail.RootTable.Columns["PmItemCode"].EditType = (EditType)0;
			grdPackingMaterialDetail.RootTable.Columns["PmItemName"].EditType = (EditType)0;
			grdPackingMaterialDetail.RootTable.Columns["PmItemUomId"].EditType = (EditType)0;
			grdPackingMaterialDetail.RootTable.Columns["PmItemUom"].EditType = (EditType)0;
			grdPackingMaterialDetail.RootTable.Columns["BrandInnerQty"].EditType = (EditType)0;
			grdPackingMaterialDetail.RootTable.Columns["PmItemOuterQty"].EditType = (EditType)0;
			grdPackingMaterialDetail.RootTable.Columns["PmItemInnerQty"].EditType = (EditType)0;
			grdPackingMaterialDetail.RootTable.Columns["Status"].EditType = (EditType)0;
			grdPackingMaterialDetail.RootTable.Columns["BrandOuterQty"].EditType = (EditType)1;
			grdPackingMaterialDetail.RootTable.Columns["Remarks"].EditType = (EditType)1;
			grdPackingMaterialDetail.RootTable.Columns["QtyEquivalent"].TextAlignment = (TextAlignment)2;
			grdPackingMaterialDetail.RootTable.Columns["PmItemOuterQty"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterialDetail.RootTable.Columns["PmItemOuterQty"].TextAlignment = (TextAlignment)3;
			grdPackingMaterialDetail.RootTable.Columns["PmItemOuterQty"].FormatString = "#,##0.###";
			grdPackingMaterialDetail.RootTable.Columns["PmItemOuterQty"].TotalFormatString = "#,##0.###";
			grdPackingMaterialDetail.RootTable.Columns["PmItemInnerQty"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterialDetail.RootTable.Columns["PmItemInnerQty"].TextAlignment = (TextAlignment)3;
			grdPackingMaterialDetail.RootTable.Columns["PmItemInnerQty"].FormatString = "#,##0.###";
			grdPackingMaterialDetail.RootTable.Columns["PmItemInnerQty"].TotalFormatString = "#,##0.###";
			grdPackingMaterialDetail.RootTable.Columns.Add("Delete");
			grdPackingMaterialDetail.RootTable.Columns["Delete"].Key = "Delete";
			grdPackingMaterialDetail.RootTable.Columns["Delete"].Caption = "X";
			grdPackingMaterialDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPackingMaterialDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdPackingMaterialDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdPackingMaterialDetail.RootTable.Columns["Delete"].Width = 20;
			grdPackingMaterialDetail.RootTable.Columns["Delete"].Position = 0;
			grdPackingMaterialDetail.RootTable.Columns.Add("Add");
			grdPackingMaterialDetail.RootTable.Columns["Add"].Key = "Add";
			grdPackingMaterialDetail.RootTable.Columns["Add"].Caption = "+";
			grdPackingMaterialDetail.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPackingMaterialDetail.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdPackingMaterialDetail.RootTable.Columns["Add"].ButtonText = "+";
			grdPackingMaterialDetail.RootTable.Columns["Add"].Width = 20;
			grdPackingMaterialDetail.RootTable.Columns["Add"].Position = 1;
			grdPackingMaterialDetail.RootTable.Columns["Add"].Visible = false;
			grdPackingMaterialDetail.RootTable.ColumnSets["Add"].Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterialDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPackingMaterialDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
				if (e.Column.Key == "Add")
				{
					AddDetailRow(r);
				}
				grdPackingMaterialDetail.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					ExImScheduleDetailWithPm vd = new ExImScheduleDetailWithPm();
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd.ContractId = Conversion.ToInt(r.Cells["ContractId"].Value);
					vd.ContractScheduleId = Conversion.ToInt(r.Cells["ScheduleId"].Value);
					vd.BrandId = Conversion.ToInt(r.Cells["BrandId"].Value);
					vd.BrandUomId = Conversion.ToInt(r.Cells["BrandUomId"].Value);
					vd.BrandPackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					vd.BrandOuterQty = Conversion.ToDouble(r.Cells["BrandOuterQty"].Value);
					vd.BrandInnerQty = Conversion.ToDouble(r.Cells["BrandInnerQty"].Value);
					vd.PMItemId = Conversion.ToInt(r.Cells["PmItemId"].Value);
					vd.PmItemUomId = Conversion.ToInt(r.Cells["PmItemUomId"].Value);
					vd.PmOuterQty = Conversion.ToDouble(r.Cells["PmItemOuterQty"].Value);
					vd.PmInnerQty = Conversion.ToDouble(r.Cells["PmItemInnerQty"].Value);
					vd.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
					vd.ActionTypeId = 3;
					lstRemoveRecordInput.Add(vd);
					dtPmDetail.Rows[r.RowIndex].Delete();
					r.Delete();
					grdPackingMaterialDetail.UpdateData();
				}
			}
			else
			{
				r.Delete();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddDetailRow(GridEXRow r)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (r != null && (int)r.RowType == 0)
			{
				DataRow dr = dtPmDetail.Rows[r.RowIndex];
				dtPmDetail.Rows.Add(dr.ItemArray);
				dtPmDetail.Rows[dtPmDetail.Rows.Count - 1]["Id"] = 0;
				grdPackingMaterialDetail.DataSource = dtPmDetail;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterialDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdPackingMaterialDetail.UpdateData();
			GridEXRow r = grdPackingMaterialDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "BrandOuterQty")
			{
				double BrandOuterQty = Conversion.ToDouble(r.Cells["BrandOuterQty"].Value);
				double QtyEquivalent = Conversion.ToDouble(r.Cells["QtyEquivalent"].Value);
				if (Conversion.ToDouble(r.Cells["PmItemOuterQty"].Value) > 0.0)
				{
					r.Cells["PmItemOuterQty"].Value = BrandOuterQty.ToString();
				}
				else if (Conversion.ToDouble(r.Cells["PmItemInnerQty"].Value) > 0.0 && QtyEquivalent > 0.0)
				{
					double PmInnerItemQty = BrandOuterQty * QtyEquivalent;
					r.Cells["PmItemInnerQty"].Value = PmInnerItemQty.ToString();
				}
			}
			grdPackingMaterialDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterialDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow r = grdPackingMaterialDetail.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (!e.Control || e.KeyCode != Keys.Return)
			{
				if (e.Control && e.KeyCode == Keys.Space)
				{
					if (grdPackingMaterialDetail.CurrentColumn != null && grdPackingMaterialDetail.CurrentColumn.Key == "Delete")
					{
						DeleteDetailRow(r);
					}
					if (grdPackingMaterialDetail.CurrentColumn != null && grdPackingMaterialDetail.CurrentColumn.Key == "Add")
					{
						DataRow dr = dtPmDetail.Rows[r.RowIndex];
						dtPmDetail.Rows.Add(dr.ItemArray);
						dtPmDetail.Rows[dtPmDetail.Rows.Count - 1]["Id"] = 0;
						grdPackingMaterialDetail.DataSource = dtPmDetail;
					}
					grdPackingMaterialDetail.UpdateData();
				}
				else if (e.Control && e.KeyCode == Keys.Delete)
				{
					DeleteDetailRow(r);
				}
				else if (e.Control && e.KeyCode == Keys.D)
				{
					DataRow dr2 = dtPmDetail.Rows[r.RowIndex];
					dtPmDetail.Rows.Add(dr2.ItemArray);
					dtPmDetail.Rows[dtPmDetail.Rows.Count - 1]["Id"] = 0;
					grdPackingMaterialDetail.DataSource = dtPmDetail;
					grdPackingMaterialDetail.UpdateData();
				}
			}
			if (e.KeyCode != Keys.F1)
			{
				return;
			}
			int Id = 0;
			string NameCode = "";
			if (grdPackingMaterialDetail.CurrentColumn != null && (grdPackingMaterialDetail.CurrentColumn.Key == "PmItemName" || grdPackingMaterialDetail.CurrentColumn.Key == "PmItemCode"))
			{
				int PreviousItemId = Conversion.ToInt(r.Cells["PmItemId"].Value);
				GrdPopUp obj2 = new GrdPopUp(dtPMItem, "Id", "ItemName");
				obj2.StartPosition = FormStartPosition.CenterScreen;
				obj2.ShowDialog();
				obj2.BringToFront();
				if (obj2.ReturnId > 0)
				{
					Id = Conversion.ToInt((object)obj2.ReturnId);
					NameCode = Conversion.ToString((object)obj2.ReturnName);
					r.Cells["PmItemId"].Value = Id;
					r.Cells["PmItemName"].Value = NameCode;
					DataRow foundRow = (from rrr in dtPMItem.AsEnumerable()
						where rrr.Field<int>("Id") == Id
						select rrr).FirstOrDefault();
					string ItemCode = ((foundRow != null) ? Conversion.ToString(foundRow["ItemCode"]) : "");
					r.Cells["PmItemCode"].Value = ItemCode;
					if (Id != PreviousItemId)
					{
						r.Cells["PmItemUomId"].Value = 0;
						r.Cells["PmItemUom"].Value = "";
					}
				}
			}
			if (grdPackingMaterialDetail.CurrentColumn != null && (grdPackingMaterialDetail.CurrentColumn.Key == "PmItemUom" || grdPackingMaterialDetail.CurrentColumn.Key == "PmItemUomId"))
			{
				GrdPopUp obj = new GrdPopUp(CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r.Cells["PmItemId"].Value)), "Id", "UOMCode");
				obj.StartPosition = FormStartPosition.CenterScreen;
				obj.ShowDialog();
				obj.BringToFront();
				if (obj.ReturnId > 0)
				{
					Id = Conversion.ToInt((object)obj.ReturnId);
					NameCode = Conversion.ToString((object)obj.ReturnName);
					r.Cells["PmItemUomId"].Value = Id;
					r.Cells["PmItemUom"].Value = NameCode;
				}
			}
			grdPackingMaterialDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Expected O, but got Unknown
		try
		{
			if (RecId == 0)
			{
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			ExImScheduleDetailWithPm obj = new ExImScheduleDetailWithPm();
			ExImScheduleOtherItemsDetail objOther = new ExImScheduleOtherItemsDetail();
			if (RecId > 0)
			{
				obj.ContractScheduleId = RecId;
				objOther.ContractScheduleId = RecId;
				obj.ExImScheduleDetailWithPmList = new List<ExImScheduleDetailWithPm>();
				objOther.ExImScheduleOtherItemsDetailList = new List<ExImScheduleOtherItemsDetail>();
				if (grdPackingMaterialDetail.RowCount > 0)
				{
					GridEXRow[] rows = grdPackingMaterialDetail.GetRows();
					foreach (GridEXRow r2 in rows)
					{
						ExImScheduleDetailWithPm vd2 = new ExImScheduleDetailWithPm();
						vd2.Id = Conversion.ToInt(r2.Cells["Id"].Value);
						vd2.ActionTypeId = ((vd2.Id <= 0) ? 1 : 2);
						if (Conversion.ToInt(r2.Cells["BrandId"].Value) == 0)
						{
							throw new Exception($"Brand Item Is Required In Packing Material Grid in Row No {r2.RowIndex + 1}");
						}
						if (Conversion.ToInt(r2.Cells["BrandUomId"].Value) == 0)
						{
							throw new Exception($"Brand Uom Is Required In Packing Material Grid in Row No {r2.RowIndex + 1}");
						}
						if (Conversion.ToInt(r2.Cells["PackingTypeId"].Value) == 0)
						{
							throw new Exception($"Brand PackingType Is Required In Packing Material Grid in Row No {r2.RowIndex + 1}");
						}
						if (Conversion.ToDouble(r2.Cells["BrandOuterQty"].Value) == 0.0)
						{
							throw new Exception($"BrandOuterQty Is Required In Packing Material Grid in Row No {r2.RowIndex + 1}");
						}
						if (Conversion.ToInt(r2.Cells["PmItemId"].Value) == 0)
						{
							throw new Exception($"PmItem Is Required In Packing Material Grid in Row No {r2.RowIndex + 1}");
						}
						if (Conversion.ToInt(r2.Cells["PmItemUomId"].Value) == 0)
						{
							throw new Exception($"PmItemUom Is Required In Packing Material Grid in Row No {r2.RowIndex + 1}");
						}
						if (Conversion.ToInt(r2.Cells["PmItemOuterQty"].Value) == 0 && Conversion.ToInt(r2.Cells["PmItemInnerQty"].Value) == 0)
						{
							throw new Exception($"PmItemOuterQty or PmItemInnerQty Is Required In Packing Material Grid in Row No {r2.RowIndex + 1}");
						}
						vd2.ContractId = Conversion.ToInt(r2.Cells["ContractId"].Value);
						vd2.ContractScheduleId = Conversion.ToInt(r2.Cells["ScheduleId"].Value);
						vd2.BrandId = Conversion.ToInt(r2.Cells["BrandId"].Value);
						vd2.BrandUomId = Conversion.ToInt(r2.Cells["BrandUomId"].Value);
						vd2.BrandPackingTypeId = Conversion.ToInt(r2.Cells["PackingTypeId"].Value);
						vd2.BrandOuterQty = Conversion.ToDouble(r2.Cells["BrandOuterQty"].Value);
						vd2.BrandInnerQty = Conversion.ToDouble(r2.Cells["BrandInnerQty"].Value);
						vd2.PMItemId = Conversion.ToInt(r2.Cells["PmItemId"].Value);
						vd2.PmItemUomId = Conversion.ToInt(r2.Cells["PmItemUomId"].Value);
						vd2.PmOuterQty = Conversion.ToDouble(r2.Cells["PmItemOuterQty"].Value);
						vd2.PmInnerQty = Conversion.ToDouble(r2.Cells["PmItemInnerQty"].Value);
						vd2.Remarks = Conversion.ToString(r2.Cells["Remarks"].Value);
						vd2.EntryDate = DateTime.Now;
						vd2.ModifyDate = DateTime.Now;
						vd2.EntryUserId = UserAccount.ID;
						vd2.ModifyUserId = UserAccount.ID;
						obj.ExImScheduleDetailWithPmList.Add(vd2);
					}
					if (grdotheritems.RowCount > 0)
					{
						rows = grdotheritems.GetRows();
						foreach (GridEXRow r in rows)
						{
							ExImScheduleOtherItemsDetail vd = new ExImScheduleOtherItemsDetail();
							vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
							if (Conversion.ToInt(r.Cells["ItemId"].Value) == 0)
							{
								throw new Exception($"Item Is Required In Other_Item Grid in Row No {r.RowIndex + 1}");
							}
							if (Conversion.ToInt(r.Cells["Qty"].Value) == 0)
							{
								throw new Exception($"Qty Is Required In Other_Item Grid in Row No {r.RowIndex + 1}");
							}
							vd.ContractId = ContractId;
							vd.ContractScheduleId = RecId;
							vd.ContractOtherItemId = Conversion.ToInt(r.Cells["ContractOtherItemId"].Value);
							vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
							vd.Qty = Conversion.ToDouble(r.Cells["Qty"].Value);
							vd.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
							vd.EntryDate = DateTime.Now;
							vd.ModifyDate = DateTime.Now;
							vd.EntryUserId = UserAccount.ID;
							vd.ModifyUserId = UserAccount.ID;
							objOther.ExImScheduleOtherItemsDetailList.Add(vd);
						}
					}
					ExImLcOrderShipmentSchedule.SaveScheduleDetailWithPm(obj, objOther);
					if (RecId == 0)
					{
						MessageBox.Show("Record Save Successfully");
					}
					else
					{
						MessageBox.Show("Record Update Successfully");
					}
					Reset();
					return;
				}
				throw new Exception("Packing Material Grid record not found");
			}
			throw new Exception("RecId not found...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool ReadById(int ID, int ContractId)
	{
		bool SuccessFlag = false;
		try
		{
			RecId = ID;
			lstRemoveRecordInput.Clear();
			grdPackingMaterialDetail.DataSource = null;
			dtPmDetail.Clear();
			dtOtherItem.Rows.Clear();
			grdotheritems.DataSource = null;
			btnUpdate.Visible = true;
			tabControl1.SelectedIndex = 0;
			DataTable Obj = ExImLcOrderShipmentSchedule.ExImScheduleDetailWithPmByScheduleId(ID);
			if (Obj != null && Obj.Rows.Count > 0)
			{
				foreach (DataRow r2 in Obj.Rows)
				{
					dtPmDetail.Rows.Add(r2["ContractId"], r2["ContractScheduleId"], r2["Id"], r2["BrandId"], r2["BrandCode"], r2["BrandName"], r2["BrandUomId"], r2["BrandUomCode"], r2["QtyEquivalent"], r2["BrandPackingTypeId"], r2["BrandPackingType"], r2["BrandOuterQty"], r2["BrandInnerQty"], r2["PMItemId"], r2["PmItemCode"], r2["PmItemName"], r2["PmItemUomId"], r2["PmUomCode"], r2["PmOuterQty"], r2["PmInnerQty"], r2["Remarks"], r2["Status"]);
				}
				grdPackingMaterialDetail.DataSource = dtPmDetail;
				grdSettingsPM();
				SuccessFlag = true;
				DataTable dtOther = ExImLcOrderShipmentSchedule.GetOtherItemsDataForSchedule(UserAccount.OrganizationId, UserAccount.CompanyId, ContractId, ID);
				if (dtOther != null && dtOther.Rows.Count > 0)
				{
					foreach (DataRow r in dtOther.Rows)
					{
						if (Conversion.ToDouble(r["Id"]) > 0.0)
						{
							double Qty = Conversion.ToDouble(r["UsedQtyInSchedule"]);
							dtOtherItem.Rows.Add(r["Id"], r["ContractId"], r["ContractOtherItemId"], r["ItemId"], r["OtherItemName"], Qty, r["Remarks"]);
						}
						else if (Conversion.ToDouble(r["BalQty"]) > 0.0)
						{
							double Qty2 = Conversion.ToDouble(r["BalQty"]);
							dtOtherItem.Rows.Add(r["Id"], r["ContractId"], r["ContractOtherItemId"], r["ItemId"], r["OtherItemName"], Qty2, r["Remarks"]);
						}
					}
					grdotheritems.DataSource = dtOtherItem;
					grdSettingsOtherItems();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return SuccessFlag;
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("RecId not Found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmExportContractSchedulePmDetail_FormClosing(object sender, FormClosingEventArgs e)
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

	private void Reset()
	{
		try
		{
			lstRemoveRecordInput.Clear();
			RecId = 0;
			ContractId = 0;
			tabControl2.SelectedIndex = 0;
			dtPmDetail.Rows.Clear();
			dtOtherItem.Rows.Clear();
			grdPackingMaterialDetail.DataSource = null;
			grdotheritems.DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			PackingItemFill();
			dtItemAndPMItemMap = ItemAndPMItemMap.ItemAndPMItemMap_GetByItemIds(UserAccount.CompanyId, UserAccount.OrganizationId, (string)null);
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
			FromDateHistory.Focus();
			tabControl3.SelectedIndex = 0;
		}
		else
		{
			((Control)(object)grdPackingMaterialDetail).Focus();
		}
	}

	private void HistoryCombosFill()
	{
		try
		{
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("Name");
			DataTable dt = ExImLcOrderShipmentScheduleHeader.GetDataForDropDownFromShipmentSchedule(UserAccount.OrganizationId, UserAccount.CompanyId, (string)null);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Customer")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtCustomer, CmbCustomerHistory, "Id", "Name", "Customer Name", ZeroIndex: false);
			((UltraGridBase)CmbCustomerHistory).DisplayLayout.Bands[0].Columns[1].Width = 250;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnResetHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			DataGridHistory.DataSource = null;
			grdPMHistory.DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryCombosFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnShowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ToDate = ToDateHistory.Value;
				}
			}
			obj.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			DataTable dtHistory = ExImLcOrderShipmentSchedule.ContractSchedule_FormHistory(obj);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dataTable = dtcol;
				if (dataTable != null && dataTable.Columns.Count == 0)
				{
					dtcol.Columns.Add("RecordNo", typeof(int));
					dtcol.Columns.Add("Id", typeof(int));
					dtcol.Columns.Add("ContractId", typeof(int));
					dtcol.Columns.Add("ContractNo", typeof(string));
					dtcol.Columns.Add("SupplierCustomerId", typeof(int));
					dtcol.Columns.Add("CustomerName", typeof(string));
					dtcol.Columns.Add("ItemName", typeof(string));
					dtcol.Columns.Add("NoOfContainer", typeof(int));
					dtcol.Columns.Add("MTon", typeof(decimal));
					dtcol.Columns.Add("DestinationPort", typeof(string));
					dtcol.Columns.Add("LoadingDate", typeof(DateTime));
					dtcol.Columns.Add("ProductionDate", typeof(DateTime));
					dtcol.Columns.Add("InspectionDate", typeof(DateTime));
					dtcol.Columns.Add("PackingMaterialDate", typeof(DateTime));
					dtcol.Columns.Add("Remarks", typeof(string));
					dtcol.Columns.Add("Status", typeof(string));
					dtcol.Columns.Add("EntryDate", typeof(DateTime));
					dtcol.Columns.Add("EntryUser", typeof(string));
					dtcol.Columns.Add("NoOfAttachments", typeof(int));
				}
				dtcol.Rows.Clear();
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					dtcol.Rows.Add(dtHistory.Rows[i]["RecordNo"], dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["ExImLcOrderId"], dtHistory.Rows[i]["ContractNo"], dtHistory.Rows[i]["SupplierCustomerId"], dtHistory.Rows[i]["CustomerName"], dtHistory.Rows[i]["ItemName"], dtHistory.Rows[i]["NoOfContainer"], dtHistory.Rows[i]["MTon"], dtHistory.Rows[i]["DestinationPort"], Conversion.ToDateTime(dtHistory.Rows[i]["AttentiveLoadingDate"]).ToShortDateString(), Conversion.ToDateTime(dtHistory.Rows[i]["AttentiveProductionDate"]).ToShortDateString(), Conversion.ToDateTime(dtHistory.Rows[i]["AttentiveInspectionDate"]).ToShortDateString(), Conversion.ToDateTime(dtHistory.Rows[i]["PackingMaterialDate"]).ToShortDateString(), dtHistory.Rows[i]["ScheduleRemarks"], dtHistory.Rows[i]["Status"], Conversion.ToDateTime(dtHistory.Rows[i]["EntryDate"]).ToShortDateString(), dtHistory.Rows[i]["EntryUser"], dtHistory.Rows[i]["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dtcol;
				GridSetting();
			}
			else
			{
				DataGridHistory.DataSource = null;
				grdPMHistory.DataSource = null;
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
			DataGridHistory.RootTable.Columns["SupplierCustomerId"].Visible = false;
			DataGridHistory.RootTable.Columns["ContractId"].Visible = false;
			DataGridHistory.RootTable.ColumnSets["InspectionDate"].Visible = false;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["RecordNo"].Width = 50;
			DataGridHistory.RootTable.Columns["ContractNo"].Width = 90;
			DataGridHistory.RootTable.Columns["CustomerName"].Width = 220;
			DataGridHistory.RootTable.Columns["ItemName"].Width = 220;
			DataGridHistory.RootTable.Columns["MTon"].Width = 75;
			DataGridHistory.RootTable.Columns["NoOfContainer"].Width = 50;
			DataGridHistory.RootTable.Columns["NoOfContainer"].Caption = "Fcl";
			DataGridHistory.RootTable.Columns["PackingMaterialDate"].Width = 80;
			DataGridHistory.RootTable.Columns["InspectionDate"].Width = 80;
			DataGridHistory.RootTable.Columns["ProductionDate"].Width = 80;
			DataGridHistory.RootTable.Columns["LoadingDate"].Width = 70;
			DataGridHistory.RootTable.Columns["Status"].Width = 70;
			DataGridHistory.RootTable.Columns["DestinationPort"].Width = 80;
			DataGridHistory.RootTable.Columns["EntryUser"].Width = 80;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = 70;
			DataGridHistory.RootTable.Columns["Remarks"].Width = 65;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].Width = 80;
			DataGridHistory.RootTable.Columns["LoadingDate"].FormatString = "dd-MMM-yy";
			DataGridHistory.RootTable.Columns["InspectionDate"].FormatString = "dd-MMM-yy";
			DataGridHistory.RootTable.Columns["PackingMaterialDate"].FormatString = "dd-MMM-yy";
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy";
			DataGridHistory.RootTable.Columns["ProductionDate"].FormatString = "dd-MMM-yy";
			DataGridHistory.RootTable.Columns["NoOfContainer"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["NoOfContainer"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["NoOfContainer"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["NoOfContainer"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["MTon"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["MTon"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["MTon"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["MTon"].TotalFormatString = "#,##0.##";
			if (formrights.DoHaveUpdateRights)
			{
				DataGridHistory.RootTable.Columns.Add("Edit");
				DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
				DataGridHistory.RootTable.Columns["Edit"].Key = "Edit";
				DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
				DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
				DataGridHistory.RootTable.Columns["Edit"].Width = 50;
			}
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && (int)r.RowType != 5 && r.Cells.Count > 0)
			{
				RecId = Conversion.ToInt(r.Cells["Id"].Value);
				ContractId = Conversion.ToInt(r.Cells["ContractId"].Value);
				ReadById(RecId, ContractId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && (int)r.RowType != 5 && r.Cells.Count > 0 && e.Column.Key == "Edit")
			{
				RecId = Conversion.ToInt(r.Cells["Id"].Value);
				ContractId = Conversion.ToInt(r.Cells["ContractId"].Value);
				ReadById(RecId, ContractId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			CommonServices.GetNoofAttachmentsByScreenName(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), "frmSaleContractSchedule");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || DataGridHistory.CurrentColumn == null)
			{
				return;
			}
			Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (formrights.DoHaveUpdateRights)
				{
					Reset();
					RecId = Conversion.ToInt(r.Cells["Id"].Value);
					ContractId = Conversion.ToInt(r.Cells["ContractId"].Value);
					ReadById(RecId, ContractId);
				}
			}
			else
			{
				if (!e.Control || e.KeyCode != Keys.Space)
				{
					return;
				}
				if (DataGridHistory.CurrentColumn.Key == "Edit" && formrights.DoHaveUpdateRights)
				{
					Reset();
					RecId = Conversion.ToInt(r.Cells["Id"].Value);
					ContractId = Conversion.ToInt(r.Cells["ContractId"].Value);
					ReadById(RecId, ContractId);
				}
				if (DataGridHistory.CurrentColumn.Key == "Print")
				{
					if (!formrights.DoHavePrintRights)
					{
					}
				}
				else if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), 202);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && (int)r.RowType != 5 && r.Cells.Count > 0)
			{
				GetDetailGrdByHeadId(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId(int Id)
	{
		try
		{
			DataTable Obj = ExImLcOrderShipmentSchedule.ExImScheduleDetailWithPmByScheduleId(Id);
			if (Obj != null && Obj.Rows.Count > 0)
			{
				DataTable dtPmH = new DataTable();
				dtPmH.Columns.Add("Id", typeof(int));
				dtPmH.Columns.Add("BrandId", typeof(int));
				dtPmH.Columns.Add("BrandCode");
				dtPmH.Columns.Add("BrandName");
				dtPmH.Columns.Add("BrandUomId", typeof(int));
				dtPmH.Columns.Add("BrandUom");
				dtPmH.Columns.Add("PackingTypeId", typeof(int));
				dtPmH.Columns.Add("PackingType");
				dtPmH.Columns.Add("BrandOuterQty", typeof(double));
				dtPmH.Columns.Add("BrandInnerQty", typeof(double));
				dtPmH.Columns.Add("PmItemId", typeof(int));
				dtPmH.Columns.Add("PmItemCode");
				dtPmH.Columns.Add("PmItemName");
				dtPmH.Columns.Add("PmItemUomId", typeof(int));
				dtPmH.Columns.Add("PmItemUom");
				dtPmH.Columns.Add("PmItemOuterQty", typeof(double));
				dtPmH.Columns.Add("PmItemInnerQty", typeof(double));
				dtPmH.Columns.Add("Remarks");
				dtPmH.Clear();
				foreach (DataRow r in Obj.Rows)
				{
					dtPmH.Rows.Add(r["Id"], r["BrandId"], r["BrandCode"], r["BrandName"], r["BrandUomId"], r["BrandUomCode"], r["BrandPackingTypeId"], r["BrandPackingType"], r["BrandOuterQty"], r["BrandInnerQty"], r["PMItemId"], r["PmItemCode"], r["PmItemName"], r["PmItemUomId"], r["PmUomCode"], r["PmOuterQty"], r["PmInnerQty"], r["Remarks"]);
				}
				grdPMHistory.DataSource = dtPmH;
				grdSettingsPMHistory();
			}
			else
			{
				grdPMHistory.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsPMHistory()
	{
		try
		{
			grdPMHistory.RootTable.Columns["Id"].Visible = false;
			grdPMHistory.RootTable.Columns["BrandId"].Visible = false;
			grdPMHistory.RootTable.Columns["BrandUomId"].Visible = false;
			grdPMHistory.RootTable.Columns["PackingTypeId"].Visible = false;
			grdPMHistory.RootTable.Columns["PmItemId"].Visible = false;
			grdPMHistory.RootTable.Columns["PmItemId"].Visible = false;
			grdPMHistory.RootTable.Columns["PmItemUomId"].Visible = false;
			grdPMHistory.RootTable.Columns["PmItemUomId"].Visible = false;
			grdPMHistory.RootTable.Columns["BrandCode"].Width = 45;
			grdPMHistory.RootTable.Columns["BrandName"].Width = 200;
			grdPMHistory.RootTable.Columns["BrandUom"].Width = 100;
			grdPMHistory.RootTable.Columns["PackingType"].Width = 100;
			grdPMHistory.RootTable.Columns["BrandOuterQty"].Width = 60;
			grdPMHistory.RootTable.Columns["BrandInnerQty"].Width = 60;
			grdPMHistory.RootTable.Columns["PmItemCode"].Width = 45;
			grdPMHistory.RootTable.Columns["PmItemName"].Width = 200;
			grdPMHistory.RootTable.Columns["PmItemUom"].Width = 45;
			grdPMHistory.RootTable.Columns["PmItemOuterQty"].Width = 60;
			grdPMHistory.RootTable.Columns["PmItemInnerQty"].Width = 60;
			grdPMHistory.RootTable.Columns["Remarks"].Width = 200;
			grdPMHistory.RootTable.Columns["BrandOuterQty"].AggregateFunction = (AggregateFunction)2;
			grdPMHistory.RootTable.Columns["BrandOuterQty"].TextAlignment = (TextAlignment)3;
			grdPMHistory.RootTable.Columns["BrandOuterQty"].FormatString = "#,##0.###";
			grdPMHistory.RootTable.Columns["BrandOuterQty"].TotalFormatString = "#,##0.###";
			grdPMHistory.RootTable.Columns["BrandInnerQty"].AggregateFunction = (AggregateFunction)2;
			grdPMHistory.RootTable.Columns["BrandInnerQty"].TextAlignment = (TextAlignment)3;
			grdPMHistory.RootTable.Columns["BrandInnerQty"].FormatString = "#,##0.###";
			grdPMHistory.RootTable.Columns["BrandInnerQty"].TotalFormatString = "#,##0.###";
			grdPMHistory.RootTable.Columns["PmItemOuterQty"].AggregateFunction = (AggregateFunction)2;
			grdPMHistory.RootTable.Columns["PmItemOuterQty"].TextAlignment = (TextAlignment)3;
			grdPMHistory.RootTable.Columns["PmItemOuterQty"].FormatString = "#,##0.###";
			grdPMHistory.RootTable.Columns["PmItemOuterQty"].TotalFormatString = "#,##0.###";
			grdPMHistory.RootTable.Columns["PmItemInnerQty"].AggregateFunction = (AggregateFunction)2;
			grdPMHistory.RootTable.Columns["PmItemInnerQty"].TextAlignment = (TextAlignment)3;
			grdPMHistory.RootTable.Columns["PmItemInnerQty"].FormatString = "#,##0.###";
			grdPMHistory.RootTable.Columns["PmItemInnerQty"].TotalFormatString = "#,##0.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Print_Click(object sender, EventArgs e)
	{
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, DataGridHistory, dtcol);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPackingMaterialRequirementPlanning_Click(object sender, EventArgs e)
	{
		try
		{
			new frmPackingMaterialRequirementPlanning(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAutoQtyGenerate_Click(object sender, EventArgs e)
	{
		try
		{
			if (grdPackingMaterialDetail.GetRows().Count() <= 0)
			{
				return;
			}
			GridEXRow[] rows = grdPackingMaterialDetail.GetRows();
			foreach (GridEXRow item in rows)
			{
				item.BeginEdit();
				double BrandOuterQty = Conversion.ToDouble(item.Cells["BrandOuterQty"].Value);
				double QtyEquivalent = Conversion.ToDouble(item.Cells["QtyEquivalent"].Value);
				if (Conversion.ToDouble(item.Cells["PmItemOuterQty"].Value) > 0.0)
				{
					item.Cells["PmItemOuterQty"].Value = BrandOuterQty.ToString();
				}
				else if (Conversion.ToDouble(item.Cells["PmItemInnerQty"].Value) > 0.0 && QtyEquivalent > 0.0)
				{
					double PmInnerItemQty = BrandOuterQty * QtyEquivalent;
					item.Cells["PmItemInnerQty"].Value = PmInnerItemQty.ToString();
				}
				item.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAutoMap_Click(object sender, EventArgs e)
	{
		try
		{
			AutoFillGridFromMapping();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AutoFillGridFromMapping()
	{
		try
		{
			if (ContractId == 0)
			{
				throw new Exception("Please Select the record before auto mapping");
			}
			DataTable dataTable = dtItemAndPMItemMap;
			if (dataTable != null && dataTable.Rows.Count == 0)
			{
				throw new Exception("There are no items to map with Packing Material");
			}
			List<ExImLcOrderShipmentSchedulePackingDetail> packingDetailByContractId = ExImLcOrderShipmentSchedule.GetPackingDetailByContractId(ContractId);
			if (packingDetailByContractId != null && packingDetailByContractId.Count == 0)
			{
				throw new Exception("No Brand Items found");
			}
			EnumerableRowCollection<DataRow> itemMapRows = dtItemAndPMItemMap.AsEnumerable();
			EnumerableRowCollection<DataRow> existingDetails = dtPmDetail.AsEnumerable();
			foreach (ExImLcOrderShipmentSchedulePackingDetail item in packingDetailByContractId)
			{
				int brandId = Conversion.ToInt((object)item.ItemId);
				foreach (DataRow row2 in itemMapRows.Where((DataRow row) => row.Field<int>("ItemId") == brandId).ToList())
				{
					int pmItemId = row2.Field<int>("PmItemId");
					if (!existingDetails.Any((DataRow r) => r.Field<int>("BrandId") == brandId && r.Field<int>("PmItemId") == pmItemId))
					{
						dtPmDetail.Rows.Add(ContractId, RecId, 0, brandId, item.ItemCode, item.ItemName, item.PackUomId, item.PackUom, item.PackUomQtyEquivalent, item.PackingTypeId, item.PackingType, item.NoOfBags, item.InnerQty, pmItemId, row2.Field<string>("PmItemCode"), row2.Field<string>("PmItemName"), row2.Field<int>("PmBaseUomId"), row2.Field<string>("PmBaseUomCode"), 0, item.InnerQty, "", "UnMatched");
					}
				}
			}
			grdPackingMaterialDetail.DataSource = dtPmDetail;
			grdSettingsPM();
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error in AutoFillGridFromMapping: " + ex.Message);
		}
	}

	private void frmExportContractSchedulePmDetail_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					tabControl2.SelectedIndex = 0;
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					((Control)(object)DataGridHistory).Focus();
				}
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (((e.Control && e.KeyCode == Keys.S) || (e.Control && e.KeyCode == Keys.U)) && btnUpdate.Visible && RecId > 0)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P)
				{
					Print_Click(null, null);
				}
				if (e.Control && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
				{
					Print_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdPackingMaterialDetail).Focus();
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnShowHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnResetHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefreshHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)DataGridHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Left)
			{
				if (((Control)(object)DataGridHistory).Focused)
				{
					((Control)(object)grdPMHistory).Focus();
				}
				else
				{
					((Control)(object)DataGridHistory).Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCutKey_Click(object sender, EventArgs e)
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Schedule No Combo");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+L", "For Load All Records");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus on Schedule No Combo");
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
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Expected O, but got Unknown
		//IL_0874: Unknown result type (might be due to invalid IL or missing references)
		//IL_087e: Expected O, but got Unknown
		//IL_088b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0895: Expected O, but got Unknown
		//IL_0c04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0e: Expected O, but got Unknown
		//IL_0da1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dab: Expected O, but got Unknown
		//IL_1b21: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b2b: Expected O, but got Unknown
		//IL_1b4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b59: Expected O, but got Unknown
		GridEXLayout grdPackingMaterialDetail_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Export.frmExportContractSchedulePmDetail));
		GridEXLayout grdotheritems_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdScheduleInfo_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPMHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPackingMaterialDetail = new System.Windows.Forms.TabPage();
		this.grdPackingMaterialDetail = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.btnAutoMap = new System.Windows.Forms.Button();
		this.label56 = new System.Windows.Forms.Label();
		this.tabOtherItems = new System.Windows.Forms.TabPage();
		this.grdotheritems = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdScheduleInfo = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.btnShow = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.rdAll = new System.Windows.Forms.RadioButton();
		this.datFromLaodingDate = new System.Windows.Forms.DateTimePicker();
		this.rdUnMatched = new System.Windows.Forms.RadioButton();
		this.rdMatched = new System.Windows.Forms.RadioButton();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolstrip = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.btnAutoQtyGenerate = new System.Windows.Forms.ToolStripButton();
		this.btnPackingMaterialRequirementPlanning = new System.Windows.Forms.ToolStripButton();
		this.ChkPreview = new System.Windows.Forms.CheckBox();
		this.label66 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.tabControl3 = new System.Windows.Forms.TabControl();
		this.tabPmH = new System.Windows.Forms.TabPage();
		this.grdPMHistory = new GridEX();
		this.panel32 = new System.Windows.Forms.Panel();
		this.label75 = new System.Windows.Forms.Label();
		this.panel29 = new System.Windows.Forms.Panel();
		this.label65 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel18 = new System.Windows.Forms.Panel();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.label67 = new System.Windows.Forms.Label();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.label90 = new System.Windows.Forms.Label();
		this.label91 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel17 = new System.Windows.Forms.Panel();
		this.label52 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPackingMaterialDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterialDetail).BeginInit();
		this.panel16.SuspendLayout();
		this.tabOtherItems.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdotheritems).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdScheduleInfo).BeginInit();
		this.panel7.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolstrip.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.tabControl3.SuspendLayout();
		this.tabPmH.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPMHistory).BeginInit();
		this.panel32.SuspendLayout();
		this.panel29.SuspendLayout();
		this.panel18.SuspendLayout();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel17.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1234, 711);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1234, 711);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.tabControl2);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1226, 682);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.tabControl2.Controls.Add(this.tabPackingMaterialDetail);
		this.tabControl2.Controls.Add(this.tabOtherItems);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(359, 57);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(864, 622);
		this.tabControl2.TabIndex = 0;
		this.tabPackingMaterialDetail.Controls.Add((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail);
		this.tabPackingMaterialDetail.Controls.Add(this.panel16);
		this.tabPackingMaterialDetail.Location = new System.Drawing.Point(4, 25);
		this.tabPackingMaterialDetail.Margin = new System.Windows.Forms.Padding(2);
		this.tabPackingMaterialDetail.Name = "tabPackingMaterialDetail";
		this.tabPackingMaterialDetail.Size = new System.Drawing.Size(856, 593);
		this.tabPackingMaterialDetail.TabIndex = 3;
		this.tabPackingMaterialDetail.Text = "Packing Material Detail";
		this.tabPackingMaterialDetail.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdPackingMaterialDetail).BackColor = System.Drawing.Color.White;
		grdPackingMaterialDetail_DesignTimeLayout.LayoutString = resources.GetString("grdPackingMaterialDetail_DesignTimeLayout.LayoutString");
		this.grdPackingMaterialDetail.DesignTimeLayout = grdPackingMaterialDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPackingMaterialDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).Location = new System.Drawing.Point(0, 28);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).Name = "grdPackingMaterialDetail";
		((ScrollableBase)this.grdPackingMaterialDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).Size = new System.Drawing.Size(856, 565);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).TabIndex = 3;
		this.grdPackingMaterialDetail.TotalRow = (InheritableBoolean)1;
		this.grdPackingMaterialDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPackingMaterialDetail).VisualStyle = (VisualStyle)4;
		this.grdPackingMaterialDetail.CellUpdated += new ColumnActionEventHandler(grdPackingMaterialDetail_CellUpdated);
		this.grdPackingMaterialDetail.ColumnButtonClick += new ColumnActionEventHandler(grdPackingMaterialDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPackingMaterialDetail_KeyDown);
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.btnAutoMap);
		this.panel16.Controls.Add(this.label56);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Margin = new System.Windows.Forms.Padding(2);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(856, 28);
		this.panel16.TabIndex = 1;
		this.btnAutoMap.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnAutoMap.BackColor = System.Drawing.Color.Maroon;
		this.btnAutoMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAutoMap.Font = new System.Drawing.Font("Verdana", 9.25f, System.Drawing.FontStyle.Bold);
		this.btnAutoMap.ForeColor = System.Drawing.Color.White;
		this.btnAutoMap.Location = new System.Drawing.Point(76, 1);
		this.btnAutoMap.Name = "btnAutoMap";
		this.btnAutoMap.Size = new System.Drawing.Size(86, 26);
		this.btnAutoMap.TabIndex = 563;
		this.btnAutoMap.Text = "Auto Map";
		this.btnAutoMap.UseVisualStyleBackColor = false;
		this.btnAutoMap.Click += new System.EventHandler(btnAutoMap_Click);
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label56.ForeColor = System.Drawing.Color.White;
		this.label56.Location = new System.Drawing.Point(3, 6);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(46, 17);
		this.label56.TabIndex = 560;
		this.label56.Text = "Detail";
		this.tabOtherItems.Controls.Add((System.Windows.Forms.Control)(object)this.grdotheritems);
		this.tabOtherItems.Location = new System.Drawing.Point(4, 25);
		this.tabOtherItems.Name = "tabOtherItems";
		this.tabOtherItems.Size = new System.Drawing.Size(856, 593);
		this.tabOtherItems.TabIndex = 4;
		this.tabOtherItems.Text = "Other Items";
		this.tabOtherItems.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdotheritems).BackColor = System.Drawing.Color.White;
		grdotheritems_DesignTimeLayout.LayoutString = resources.GetString("grdotheritems_DesignTimeLayout.LayoutString");
		this.grdotheritems.DesignTimeLayout = grdotheritems_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdotheritems.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdotheritems).Name = "grdotheritems";
		((ScrollableBase)this.grdotheritems).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Size = new System.Drawing.Size(856, 593);
		((System.Windows.Forms.Control)(object)this.grdotheritems).TabIndex = 4;
		this.grdotheritems.TotalRow = (InheritableBoolean)1;
		this.grdotheritems.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdotheritems).VisualStyle = (VisualStyle)4;
		this.grdotheritems.ColumnButtonClick += new ColumnActionEventHandler(grdotheritems_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdotheritems).KeyDown += new System.Windows.Forms.KeyEventHandler(grdotheritems_KeyDown);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdScheduleInfo);
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(3, 57);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(356, 622);
		this.panel5.TabIndex = 2;
		((ScrollableBase)this.grdScheduleInfo).BackColor = System.Drawing.Color.White;
		grdScheduleInfo_DesignTimeLayout.LayoutString = resources.GetString("grdScheduleInfo_DesignTimeLayout.LayoutString");
		this.grdScheduleInfo.DesignTimeLayout = grdScheduleInfo_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdScheduleInfo).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdScheduleInfo).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdScheduleInfo.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdScheduleInfo).Location = new System.Drawing.Point(0, 95);
		((System.Windows.Forms.Control)(object)this.grdScheduleInfo).Name = "grdScheduleInfo";
		((ScrollableBase)this.grdScheduleInfo).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdScheduleInfo).Size = new System.Drawing.Size(356, 527);
		((System.Windows.Forms.Control)(object)this.grdScheduleInfo).TabIndex = 0;
		this.grdScheduleInfo.TotalRow = (InheritableBoolean)1;
		this.grdScheduleInfo.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdScheduleInfo).VisualStyle = (VisualStyle)4;
		this.grdScheduleInfo.LinkClicked += new ColumnActionEventHandler(grdScheduleInfo_LinkClicked);
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.btnShow);
		this.panel7.Controls.Add(this.label1);
		this.panel7.Controls.Add(this.rdAll);
		this.panel7.Controls.Add(this.datFromLaodingDate);
		this.panel7.Controls.Add(this.rdUnMatched);
		this.panel7.Controls.Add(this.rdMatched);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 21);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(356, 74);
		this.panel7.TabIndex = 6;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(128, 27);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(54, 25);
		this.btnShow.TabIndex = 1;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(BtnCancelDepartmentDetail_Click);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(3, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(109, 15);
		this.label1.TabIndex = 466;
		this.label1.Text = "From Loading Date";
		this.rdAll.AutoSize = true;
		this.rdAll.Location = new System.Drawing.Point(204, 49);
		this.rdAll.Name = "rdAll";
		this.rdAll.Size = new System.Drawing.Size(40, 20);
		this.rdAll.TabIndex = 4;
		this.rdAll.Text = "All";
		this.rdAll.UseVisualStyleBackColor = true;
		this.datFromLaodingDate.CustomFormat = "dd-MM-yyyy";
		this.datFromLaodingDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromLaodingDate.Location = new System.Drawing.Point(6, 28);
		this.datFromLaodingDate.Name = "datFromLaodingDate";
		this.datFromLaodingDate.Size = new System.Drawing.Size(116, 23);
		this.datFromLaodingDate.TabIndex = 0;
		this.rdUnMatched.AutoSize = true;
		this.rdUnMatched.Checked = true;
		this.rdUnMatched.Location = new System.Drawing.Point(204, 26);
		this.rdUnMatched.Name = "rdUnMatched";
		this.rdUnMatched.Size = new System.Drawing.Size(104, 20);
		this.rdUnMatched.TabIndex = 3;
		this.rdUnMatched.TabStop = true;
		this.rdUnMatched.Text = "Un Matched";
		this.rdUnMatched.UseVisualStyleBackColor = true;
		this.rdMatched.AutoSize = true;
		this.rdMatched.Location = new System.Drawing.Point(204, 7);
		this.rdMatched.Name = "rdMatched";
		this.rdMatched.Size = new System.Drawing.Size(82, 20);
		this.rdMatched.TabIndex = 2;
		this.rdMatched.Text = "Matched";
		this.rdMatched.UseVisualStyleBackColor = true;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(356, 21);
		this.panel6.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(3, 2);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(104, 17);
		this.label2.TabIndex = 560;
		this.label2.Text = "Schedule Info";
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.toolstrip);
		this.panel2.Controls.Add(this.ChkPreview);
		this.panel2.Controls.Add(this.label66);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1220, 54);
		this.panel2.TabIndex = 0;
		this.toolstrip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolstrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btnnew, this.BtnRefresh, this.btnUpdate, this.Print, this.btnShortCutKey, this.btnAutoQtyGenerate, this.btnPackingMaterialRequirementPlanning });
		this.toolstrip.Location = new System.Drawing.Point(0, 0);
		this.toolstrip.Name = "toolstrip";
		this.toolstrip.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
		this.toolstrip.Size = new System.Drawing.Size(1220, 27);
		this.toolstrip.TabIndex = 5;
		this.toolstrip.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.BtnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefresh.Name = "BtnRefresh";
		this.BtnRefresh.Size = new System.Drawing.Size(80, 24);
		this.BtnRefresh.Text = "Refresh";
		this.BtnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.Print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(92, 24);
		this.Print.Text = "501-Print";
		this.Print.Visible = false;
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnShortCutKey.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.btnAutoQtyGenerate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAutoQtyGenerate.Name = "btnAutoQtyGenerate";
		this.btnAutoQtyGenerate.Size = new System.Drawing.Size(248, 24);
		this.btnAutoQtyGenerate.Text = "Auto Generate Packing Material Qty";
		this.btnAutoQtyGenerate.Click += new System.EventHandler(btnAutoQtyGenerate_Click);
		this.btnPackingMaterialRequirementPlanning.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPackingMaterialRequirementPlanning.Name = "btnPackingMaterialRequirementPlanning";
		this.btnPackingMaterialRequirementPlanning.Size = new System.Drawing.Size(263, 24);
		this.btnPackingMaterialRequirementPlanning.Text = "Packing Material Requirement Planning";
		this.btnPackingMaterialRequirementPlanning.Click += new System.EventHandler(btnPackingMaterialRequirementPlanning_Click);
		this.ChkPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPreview.AutoSize = true;
		this.ChkPreview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPreview.ForeColor = System.Drawing.Color.White;
		this.ChkPreview.Location = new System.Drawing.Point(1133, 30);
		this.ChkPreview.Name = "ChkPreview";
		this.ChkPreview.Size = new System.Drawing.Size(84, 20);
		this.ChkPreview.TabIndex = 1;
		this.ChkPreview.Text = "Preview";
		this.ChkPreview.UseVisualStyleBackColor = true;
		this.ChkPreview.Visible = false;
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label66.ForeColor = System.Drawing.Color.White;
		this.label66.Location = new System.Drawing.Point(2, 30);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(511, 19);
		this.label66.TabIndex = 561;
		this.label66.Text = "Packing Material Requirement Planning By Contract Schedule";
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.tabControl3);
		this.tabPage2.Controls.Add(this.panel29);
		this.tabPage2.Controls.Add(this.panel18);
		this.tabPage2.Controls.Add(this.panel17);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1226, 682);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.DataGridHistory).BackColor = System.Drawing.Color.White;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistory_DesignTimeLayout.LayoutString = resources.GetString("DataGridHistory_DesignTimeLayout.LayoutString");
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 124);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1220, 317);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.tabControl3.Controls.Add(this.tabPmH);
		this.tabControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.tabControl3.Location = new System.Drawing.Point(3, 441);
		this.tabControl3.Name = "tabControl3";
		this.tabControl3.SelectedIndex = 0;
		this.tabControl3.Size = new System.Drawing.Size(1220, 238);
		this.tabControl3.TabIndex = 349;
		this.tabPmH.Controls.Add((System.Windows.Forms.Control)(object)this.grdPMHistory);
		this.tabPmH.Controls.Add(this.panel32);
		this.tabPmH.Location = new System.Drawing.Point(4, 25);
		this.tabPmH.Name = "tabPmH";
		this.tabPmH.Size = new System.Drawing.Size(1212, 209);
		this.tabPmH.TabIndex = 2;
		this.tabPmH.Text = "Packing Material";
		this.tabPmH.UseVisualStyleBackColor = true;
		this.grdPMHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPMHistory).BackColor = System.Drawing.Color.White;
		this.grdPMHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPMHistory_DesignTimeLayout.LayoutString = resources.GetString("grdPMHistory_DesignTimeLayout.LayoutString");
		this.grdPMHistory.DesignTimeLayout = grdPMHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPMHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPMHistory.FilterMode = (FilterMode)2;
		this.grdPMHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPMHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPMHistory.GroupByBoxVisible = false;
		this.grdPMHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdPMHistory).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdPMHistory).Name = "grdPMHistory";
		((ScrollableBase)this.grdPMHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPMHistory).Size = new System.Drawing.Size(1212, 184);
		((System.Windows.Forms.Control)(object)this.grdPMHistory).TabIndex = 0;
		this.grdPMHistory.TotalRow = (InheritableBoolean)1;
		this.grdPMHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPMHistory).VisualStyle = (VisualStyle)4;
		this.panel32.BackColor = System.Drawing.Color.Teal;
		this.panel32.Controls.Add(this.label75);
		this.panel32.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel32.Location = new System.Drawing.Point(0, 0);
		this.panel32.Name = "panel32";
		this.panel32.Size = new System.Drawing.Size(1212, 25);
		this.panel32.TabIndex = 348;
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label75.ForeColor = System.Drawing.Color.White;
		this.label75.Location = new System.Drawing.Point(5, 5);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(343, 17);
		this.label75.TabIndex = 562;
		this.label75.Text = "Packing Material Detail of the selected above row";
		this.panel29.BackColor = System.Drawing.Color.Teal;
		this.panel29.Controls.Add(this.label65);
		this.panel29.Controls.Add(this.ctrlGrdBar1);
		this.panel29.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel29.Location = new System.Drawing.Point(3, 100);
		this.panel29.Name = "panel29";
		this.panel29.Size = new System.Drawing.Size(1220, 24);
		this.panel29.TabIndex = 348;
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.ForeColor = System.Drawing.Color.White;
		this.label65.Location = new System.Drawing.Point(4, 2);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(142, 19);
		this.label65.TabIndex = 562;
		this.label65.Text = "Filtered Records";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1169, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(48, 23);
		this.ctrlGrdBar1.TabIndex = 8;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel18.Controls.Add(this.groupBox5);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(3, 56);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1220, 44);
		this.panel18.TabIndex = 0;
		this.groupBox5.Controls.Add(this.rdapproveddate);
		this.groupBox5.Controls.Add(this.btnShowHistory);
		this.groupBox5.Controls.Add(this.rdmodifydate);
		this.groupBox5.Controls.Add(this.label67);
		this.groupBox5.Controls.Add(this.drdocdate);
		this.groupBox5.Controls.Add(this.rdentrydate);
		this.groupBox5.Controls.Add(this.label90);
		this.groupBox5.Controls.Add(this.label91);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox5.Controls.Add(this.ToDateHistory);
		this.groupBox5.Controls.Add(this.FromDateHistory);
		this.groupBox5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox5.Location = new System.Drawing.Point(4, 0);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(1211, 45);
		this.groupBox5.TabIndex = 0;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(1087, 19);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 7;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdapproveddate.Visible = false;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(774, 15);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 3;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(996, 19);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 6;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label67.Location = new System.Drawing.Point(415, 21);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(58, 15);
		this.label67.TabIndex = 87;
		this.label67.Text = "Customer";
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(841, 19);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 4;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(917, 19);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 5;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.label90.AutoSize = true;
		this.label90.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label90.Location = new System.Drawing.Point(215, 21);
		this.label90.Name = "label90";
		this.label90.Size = new System.Drawing.Size(48, 15);
		this.label90.TabIndex = 84;
		this.label90.Text = "To Date";
		this.label91.AutoSize = true;
		this.label91.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label91.Location = new System.Drawing.Point(3, 21);
		this.label91.Name = "label91";
		this.label91.Size = new System.Drawing.Size(63, 15);
		this.label91.TabIndex = 83;
		this.label91.Text = "From Date";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance62).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance63).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance63).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance63).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(476, 15);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(296, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 2;
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(267, 17);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(142, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(69, 17);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(142, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.label52);
		this.panel17.Controls.Add(this.toolStrip3);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(3, 3);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1220, 53);
		this.panel17.TabIndex = 0;
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.ForeColor = System.Drawing.Color.White;
		this.label52.Location = new System.Drawing.Point(5, 30);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(386, 19);
		this.label52.TabIndex = 562;
		this.label52.Text = "Contract Schedule History With Packing Detail";
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnResetHistory, this.btnRefreshHistory });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
		this.toolStrip3.Size = new System.Drawing.Size(1220, 27);
		this.toolStrip3.TabIndex = 7;
		this.toolStrip3.Text = "toolStrip3";
		this.btnResetHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(59, 24);
		this.btnResetHistory.Text = "&New";
		this.btnResetHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1234, 711);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmExportContractSchedulePmDetail";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmExportContractSchedulePmDetail_FormClosing);
		base.Load += new System.EventHandler(frmExportContractSchedulePmDetail_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmExportContractSchedulePmDetail_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPackingMaterialDetail.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterialDetail).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.tabOtherItems.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdotheritems).EndInit();
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdScheduleInfo).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolstrip.ResumeLayout(false);
		this.toolstrip.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.tabControl3.ResumeLayout(false);
		this.tabPmH.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPMHistory).EndInit();
		this.panel32.ResumeLayout(false);
		this.panel32.PerformLayout();
		this.panel29.ResumeLayout(false);
		this.panel29.PerformLayout();
		this.panel18.ResumeLayout(false);
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
