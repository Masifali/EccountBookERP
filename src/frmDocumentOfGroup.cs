using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.SDT;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.SDT;
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
namespace Architecture.WinApp.SDT
{

public class frmDocumentOfGroup : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private DataTable dtUnAssigned = new DataTable();

	private DataTable dtdetailGrid = new DataTable();

	private DataTable dtPType = new DataTable();

	private DataTable dtCType = new DataTable();

	private DataTable dtHistory = new DataTable();

	private int RecId;

	private int LastCriteriaDateTypeId;

	private int DefaultDaysToLessFromHistoryFromDate;

	private List<DocumentOfGroup> LstRemoveRecord = new List<DocumentOfGroup>();

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel6;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private Panel panel12;

	private Panel panel13;

	private Label label14;

	private Panel panel10;

	private Label label13;

	private Label label12;

	private ToolStripButton BtnDefineCustomGroup;

	private TabPage tabPage3;

	private Panel panel24;

	private CtrlGrdBar ctrlGrdBar7;

	private Label label5;

	private ToolStrip toolStrip4;

	private ToolStripButton btnLoadAllHistoryNew;

	private Panel panel25;

	private GroupBox groupBox2;

	private Label label10;

	internal UltraCombo CmbDocumentHis;

	private DateTimePicker txtToDate;

	private Label label50;

	private Label label11;

	internal UltraCombo CmbRequiredLevelHis;

	private DateTimePicker txtFromDate;

	private Label label19;

	private Label label51;

	internal UltraCombo CmbDocumentProviderHis;

	private Button btnShowHis;

	private Panel panel26;

	private Panel panel27;

	private ToolStripButton btnRefreshHistoryCombos;

	private TabControl tabControlMain;

	private TabPage tabPageMain;

	private Panel panel4;

	internal UltraCombo CmbCustomGroup;

	private Label label2;

	private Button btnShowByCustomGroup;

	private ToolStripButton BtnShortCutkeys;

	private Panel panel7;

	private ToolStrip toolStrip3;

	private ToolStripButton BtnTransferToAssign;

	private ToolStripButton BtnRefreshUnAssigned;

	internal GridEX grdUnAssignDoc;

	private ToolStripButton BtnDefineDocument;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton rdentrydate;

	private Label label1;

	internal UltraCombo CmbCustomGroupHis;

	internal GridEX DataGridHistory;

	internal GridEX grdAssignDoc;

	public frmDocumentOfGroup(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmDocumentOfGroup_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1100, 600);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			ctrlGrdBar7.mGridChooseFielder.Visible = false;
			ctrlGrdBar7.mGridSaveLayouts.Visible = false;
			ctrlGrdBar7.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar7.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar7.GroupCollaps.Visible = false;
			ctrlGrdBar7.GroupExpand.Visible = false;
			ctrlGrdBar7.removeLayoutToolStripMenuItem.Visible = false;
			dtCType.Columns.Add("Id", typeof(int));
			dtCType.Columns.Add("name", typeof(string));
			dtPType.Columns.Add("Id", typeof(int));
			dtPType.Columns.Add("name", typeof(string));
			FillgrdCombo();
			dtUnAssigned.Columns.Add("DocumentId", typeof(int));
			dtUnAssigned.Columns.Add("Document", typeof(string));
			dtUnAssigned.Columns.Add("RequiredLevelId", typeof(int));
			dtUnAssigned.Columns.Add("RequiredLevel");
			dtUnAssigned.Columns.Add("DocumentProviderId", typeof(int));
			dtUnAssigned.Columns.Add("DocProviderName");
			grdUnAssignDoc.DataSource = dtUnAssigned;
			grdUnAssignDoc.RetrieveStructure();
			grdUnAssignDocSetting();
			dtdetailGrid.Columns.Add("Id", typeof(int));
			dtdetailGrid.Columns.Add("DocumentId", typeof(int));
			dtdetailGrid.Columns.Add("Document", typeof(string));
			dtdetailGrid.Columns.Add("RequiredLevelId", typeof(int));
			dtdetailGrid.Columns.Add("RequiredLevel", typeof(string));
			dtdetailGrid.Columns.Add("DocumentProviderId", typeof(int));
			dtdetailGrid.Columns.Add("Original", typeof(int));
			dtdetailGrid.Columns.Add("Duplicate", typeof(int));
			dtdetailGrid.Columns.Add("CriteriaDateTypeId", typeof(int));
			dtdetailGrid.Columns.Add("BeforeDays", typeof(int));
			dtdetailGrid.Columns.Add("AfterDays", typeof(int));
			dtdetailGrid.Columns.Add("PrioritySequence", typeof(int));
			dtdetailGrid.Columns.Add("Remarks", typeof(string));
			dtdetailGrid.Columns.Add("Active", typeof(bool));
			grdAssignDoc.DataSource = dtdetailGrid;
			grdAssignDocSettings();
			CustomGroupBind();
			((Control)(object)grdUnAssignDoc).Focus();
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			ComboBindForHistory();
			txtFromDate.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			txtToDate.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CustomGroupBind()
	{
		try
		{
			int ID = Conversion.ToInt(CmbCustomGroup.Value);
			DataTable dt = CustomGroup.GetCustomGroupForCombo(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbCustomGroup, "customGroupId", "customGroupName", "CustomGroup Name", ZeroIndex: false);
				((UltraGridBase)CmbCustomGroup).DisplayLayout.Bands[0].Columns[1].Width = 250;
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("customGroupId") == ID))
					{
						CmbCustomGroup.Value = ID;
					}
					else
					{
						((Control)(object)CmbCustomGroup).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbCustomGroup).Text = string.Empty;
				((UltraGridBase)CmbCustomGroup).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboBindForHistory()
	{
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		try
		{
			DataTable dtcus = new DataTable();
			dtcus.Columns.Add("Id", typeof(int));
			dtcus.Columns.Add("Name");
			DataTable dtPort = new DataTable();
			dtPort.Columns.Add("Id", typeof(int));
			dtPort.Columns.Add("Name");
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("Name");
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("Name");
			DataTable dt = DocumentOfGroup.GetDataForDropDownFromDocumentOfGroup(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "customGroup")
					{
						dtcus.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "chartOfDocument")
					{
						dtPort.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "RequiredLevel")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "documentProvider")
					{
						dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtcus, CmbCustomGroupHis, "Id", "Name", "Custom Group", ZeroIndex: false);
			DDL.BindDDL(dtPort, CmbDocumentHis, "Id", "Name", "Document Name", ZeroIndex: false);
			DDL.BindDDL(dtCustomer, CmbRequiredLevelHis, "Id", "Name", "RequiredLevel", ZeroIndex: false);
			DDL.BindDDL(dtItem, CmbDocumentProviderHis, "Id", "Name", "document Provider", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillgrdCombo()
	{
		try
		{
			dtCType.Clear();
			dtPType.Clear();
			DataTable dt2 = ChartOfDocument.CriteriaDateTypeForCombo();
			if (dt2.Rows.Count > 0)
			{
				foreach (DataRow r2 in dt2.Rows)
				{
					dtCType.Rows.Add(r2["CriteriaDateTypeId"], r2["CriteriaDateType"]);
				}
			}
			DataTable dt = ChartOfDocument.DocumentProviderTypeForCombo();
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			foreach (DataRow r in dt.Rows)
			{
				dtPType.Rows.Add(r["DocumentProviderId"], r["DocumentProvider"]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			FormReset();
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
			btnsave.Visible = true;
			RecId = 0;
			LstRemoveRecord.Clear();
			((Control)(object)CmbCustomGroup).Text = "";
			grdAssignDoc.DataSource = null;
			grdUnAssignDoc.DataSource = null;
			dtUnAssigned.Clear();
			dtdetailGrid.Clear();
			CmbCustomGroup.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnRefreshUnAssigned_Click(object sender, EventArgs e)
	{
		try
		{
			CustomGroupBind();
			FillgrdCombo();
			grdAssignCombo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void frmDocumentOfGroup_FormClosed(object sender, FormClosedEventArgs e)
	{
		try
		{
			HistoryStack.Remove(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnShowByCustomGroup_Click(object sender, EventArgs e)
	{
		try
		{
			ShowData();
			int CustomGroupId = Conversion.ToInt(CmbCustomGroup.Value);
			if (CustomGroupId > 0)
			{
				DataTable dt = DocumentOfGroup.LastRecordBycustomGroupId(UserAccount.OrganizationId, UserAccount.CompanyId, CustomGroupId);
				if (dt.Rows.Count > 0)
				{
					LastCriteriaDateTypeId = Conversion.ToInt(dt.Rows[0]["CriteriaDateTypeId"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ShowData(bool Confirm = true)
	{
		try
		{
			tabControl1.SelectedIndex = 0;
			int CustomGroupId = Conversion.ToInt(CmbCustomGroup.Value);
			if (CustomGroupId > 0)
			{
				if (!((grdUnAssignDoc.GetDataRows().Count() > 0 || grdAssignDoc.GetDataRows().Count() > 0) && Confirm) || MessageBox.Show("grd have Records,Do you want to refresh?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					FillUnAssignData(CustomGroupId);
					ReadByCustomGroupId(CustomGroupId);
				}
				return;
			}
			grdUnAssignDoc.DataSource = null;
			grdAssignDoc.DataSource = null;
			CmbCustomGroup.Focus();
			throw new Exception("Please Select custom Group First");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillUnAssignData(int CustomGroupId)
	{
		try
		{
			dtUnAssigned.Rows.Clear();
			DataTable dtGridRegister = ChartOfDocument.GetChartOfDocumentByCustomGroupId(UserAccount.OrganizationId, UserAccount.CompanyId, CustomGroupId);
			if (dtGridRegister.Rows.Count > 0)
			{
				for (int i = 0; i < dtGridRegister.Rows.Count; i++)
				{
					dtUnAssigned.Rows.Add(dtGridRegister.Rows[i]["chartOfDocumentId"], dtGridRegister.Rows[i]["documentName"], dtGridRegister.Rows[i]["requiredLevelId"], dtGridRegister.Rows[i]["RequiredLevel"], dtGridRegister.Rows[i]["DocumentProviderId"], dtGridRegister.Rows[i]["DocProviderName"]);
				}
				grdUnAssignDoc.DataSource = dtUnAssigned;
				grdUnAssignDoc.RetrieveStructure();
				grdUnAssignDocSetting();
				CheckedAllRowsWithMadatoryStatusInUnAssign();
			}
			else
			{
				grdUnAssignDoc.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CheckedAllRowsWithMadatoryStatusInUnAssign()
	{
		try
		{
			GridEXRow[] dataRows = grdUnAssignDoc.GetDataRows();
			foreach (GridEXRow row in dataRows)
			{
				row.BeginEdit();
				if (Conversion.ToInt(row.Cells["RequiredLevelId"].Value) == 1)
				{
					row.IsChecked = Convert.ToBoolean(value: true);
				}
				row.EndEdit();
			}
			grdUnAssignDoc.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdUnAssignDocSetting()
	{
		try
		{
			grdUnAssignDoc.RootTable.Columns["DocumentId"].Visible = false;
			grdUnAssignDoc.RootTable.Columns["RequiredLevelId"].Visible = false;
			grdUnAssignDoc.RootTable.Columns["DocumentProviderId"].Visible = false;
			grdUnAssignDoc.RootTable.Columns["RequiredLevel"].Visible = false;
			grdUnAssignDoc.RootTable.Columns["Document"].Width = 170;
			grdUnAssignDoc.RootTable.Columns["RequiredLevel"].Width = 120;
			grdUnAssignDoc.RootTable.Columns["DocProviderName"].Width = 140;
			grdUnAssignDoc.RootTable.Groups.Add("RequiredLevel");
			grdUnAssignDoc.RootTable.Columns.Add("Select");
			grdUnAssignDoc.RootTable.Columns["Select"].ActAsSelector = true;
			grdUnAssignDoc.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdUnAssignDoc.RootTable.Columns["Select"].Width = 30;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnTransferToAssign_Click(object sender, EventArgs e)
	{
		try
		{
			TransferDataToAssign();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TransferDataToAssign()
	{
		try
		{
			if (grdUnAssignDoc.GetDataRows().Count() == 0)
			{
				throw new Exception("No Record Found for transfer");
			}
			if (grdUnAssignDoc.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please Select any Record");
			}
			HashSet<int> documentIdsToRemove = new HashSet<int>();
			GridEXRow[] checkedRows = grdUnAssignDoc.GetCheckedRows();
			foreach (GridEXRow r in checkedRows)
			{
				int documentId = Conversion.ToInt(r.Cells["DocumentId"].Value);
				dtdetailGrid.Rows.Add(0, documentId, Conversion.ToString(r.Cells["Document"].Value), Conversion.ToInt(r.Cells["RequiredLevelId"].Value), Conversion.ToString(r.Cells["RequiredLevel"].Value), Conversion.ToInt(r.Cells["DocumentProviderId"].Value), 1, 0, LastCriteriaDateTypeId, 0, 1, 1, "", true);
				documentIdsToRemove.Add(documentId);
			}
			grdAssignDoc.DataSource = dtdetailGrid;
			grdAssignDocSettings();
			if (documentIdsToRemove.Count > 0)
			{
				DataRow[] array = dtUnAssigned.Select("DocumentId IN (" + string.Join(",", documentIdsToRemove) + ")");
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Delete();
				}
				dtUnAssigned.AcceptChanges();
				grdUnAssignDoc.DataSource = dtUnAssigned;
				grdUnAssignDoc.RetrieveStructure();
				grdUnAssignDocSetting();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdAssignCombo()
	{
		try
		{
			if (dtCType.Rows.Count > 0)
			{
				grdAssignDoc.RootTable.Columns["CriteriaDateTypeId"].ColumnType = (ColumnType)0;
				grdAssignDoc.RootTable.Columns["CriteriaDateTypeId"].HasValueList = true;
				grdAssignDoc.RootTable.Columns["CriteriaDateTypeId"].LimitToList = true;
				grdAssignDoc.RootTable.Columns["CriteriaDateTypeId"].EditType = (EditType)4;
				grdAssignDoc.RootTable.Columns["CriteriaDateTypeId"].ColumnType = (ColumnType)0;
				grdAssignDoc.RootTable.Columns["CriteriaDateTypeId"].ValueList.PopulateValueList((IEnumerable)dtCType.DefaultView, "Id", "name");
			}
			if (dtPType.Rows.Count > 0)
			{
				grdAssignDoc.RootTable.Columns["DocumentProviderId"].ColumnType = (ColumnType)0;
				grdAssignDoc.RootTable.Columns["DocumentProviderId"].HasValueList = true;
				grdAssignDoc.RootTable.Columns["DocumentProviderId"].LimitToList = true;
				grdAssignDoc.RootTable.Columns["DocumentProviderId"].EditType = (EditType)4;
				grdAssignDoc.RootTable.Columns["DocumentProviderId"].ColumnType = (ColumnType)0;
				grdAssignDoc.RootTable.Columns["DocumentProviderId"].ValueList.PopulateValueList((IEnumerable)dtPType.DefaultView, "Id", "name");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdAssignDocSettings()
	{
		try
		{
			grdAssignCombo();
			grdAssignDoc.RootTable.Columns["Id"].Visible = false;
			grdAssignDoc.RootTable.Columns["DocumentId"].Visible = false;
			grdAssignDoc.RootTable.Columns["RequiredLevelId"].Visible = false;
			grdAssignDoc.RootTable.Columns["Id"].EditType = (EditType)0;
			grdAssignDoc.RootTable.Columns["DocumentId"].EditType = (EditType)0;
			grdAssignDoc.RootTable.Columns["Document"].EditType = (EditType)0;
			grdAssignDoc.RootTable.Columns["RequiredLevelId"].EditType = (EditType)0;
			grdAssignDoc.RootTable.Columns["RequiredLevel"].EditType = (EditType)0;
			grdAssignDoc.RootTable.Columns["Active"].EditType = (EditType)8;
			grdAssignDoc.RootTable.Columns["Original"].AggregateFunction = (AggregateFunction)2;
			grdAssignDoc.RootTable.Columns["Original"].TextAlignment = (TextAlignment)3;
			grdAssignDoc.RootTable.Columns["Original"].FormatString = "#,##0.##";
			grdAssignDoc.RootTable.Columns["Original"].TotalFormatString = "#,##0.##";
			grdAssignDoc.RootTable.Columns["Duplicate"].AggregateFunction = (AggregateFunction)2;
			grdAssignDoc.RootTable.Columns["Duplicate"].TextAlignment = (TextAlignment)3;
			grdAssignDoc.RootTable.Columns["Duplicate"].FormatString = "#,##0.##";
			grdAssignDoc.RootTable.Columns["Duplicate"].TotalFormatString = "#,##0.##";
			grdAssignDoc.RootTable.Columns["BeforeDays"].AggregateFunction = (AggregateFunction)2;
			grdAssignDoc.RootTable.Columns["BeforeDays"].TextAlignment = (TextAlignment)3;
			grdAssignDoc.RootTable.Columns["BeforeDays"].FormatString = "#,##0.##";
			grdAssignDoc.RootTable.Columns["BeforeDays"].TotalFormatString = "#,##0.##";
			grdAssignDoc.RootTable.Columns["PrioritySequence"].AggregateFunction = (AggregateFunction)2;
			grdAssignDoc.RootTable.Columns["PrioritySequence"].TextAlignment = (TextAlignment)3;
			grdAssignDoc.RootTable.Columns["PrioritySequence"].FormatString = "#,##0.##";
			grdAssignDoc.RootTable.Columns["PrioritySequence"].TotalFormatString = "#,##0.##";
			grdAssignDoc.RootTable.Columns.Add("Delete");
			grdAssignDoc.RootTable.Columns["Delete"].Key = "Delete";
			grdAssignDoc.RootTable.Columns["Delete"].Caption = "X";
			grdAssignDoc.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdAssignDoc.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdAssignDoc.RootTable.Columns["Delete"].ButtonText = "X";
			grdAssignDoc.RootTable.Columns["Delete"].Width = 20;
			grdAssignDoc.RootTable.Columns["Delete"].Position = 0;
			grdAssignDoc.FrozenColumns = 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdAssignDoc_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdAssignDoc.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeletedtGridRow(r);
				}
				grdAssignDoc.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DeletedtGridRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (r != null && (int)r.RowType == 0)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					throw new Exception("You Can't Delete Saved Record...");
				}
				int DocumentId = Conversion.ToInt(r.Cells["DocumentId"].Value);
				if (!dtUnAssigned.AsEnumerable().Any((DataRow row) => row.Field<int>("DocumentId") == DocumentId))
				{
					dtUnAssigned.Rows.Add(Conversion.ToInt(r.Cells["DocumentId"].Value), Conversion.ToString(r.Cells["Document"].Value), Conversion.ToInt(r.Cells["RequiredLevelId"].Value), Conversion.ToString(r.Cells["RequiredLevel"].Value), Conversion.ToInt(r.Cells["DocumentProviderId"].Value), Conversion.ToString((object)r.Cells["DocumentProviderId"].Text));
					grdUnAssignDoc.DataSource = dtUnAssigned;
					grdUnAssignDoc.RetrieveStructure();
					grdUnAssignDocSetting();
				}
				r.Delete();
			}
			grdAssignDoc.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdAssignDoc_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdAssignDoc.CurrentRow;
			if (item != null)
			{
				_ = item.RowType;
			}
			grdAssignDoc.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			btnsave_Click(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Insert(bool Confirm = true)
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(CmbCustomGroup.Value) == 0)
			{
				CmbCustomGroup.Focus();
				throw new Exception("Custom Group Field Required...");
			}
			if (grdAssignDoc.RowCount == 0)
			{
				throw new Exception("Grid Record not found...");
			}
			bool Update = false;
			GridEXRow[] rows = grdAssignDoc.GetRows();
			for (int j = 0; j < rows.Length; j++)
			{
				if (Conversion.ToInt(rows[j].Cells["Id"].Value) > 0)
				{
					Update = true;
					break;
				}
			}
			DocumentOfGroup obj = new DocumentOfGroup();
			if (Confirm)
			{
				if (Update)
				{
					if (MessageBox.Show("Are you sure to Update Main Schedule?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
				}
				else if (MessageBox.Show("Are you sure to Save Main Schedule?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}
			obj.EntryDate = DateTime.Now;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyDate = DateTime.Now;
			obj.ModifyUserId = UserAccount.ID;
			obj.DocumentOfGroupList = new List<DocumentOfGroup>();
			if (LstRemoveRecord.Count > 0)
			{
				for (int i = 0; i < LstRemoveRecord.Count; i++)
				{
					obj.DocumentOfGroupList.Add(LstRemoveRecord[i]);
				}
			}
			bool MandatoryExist = false;
			rows = grdAssignDoc.GetRows();
			foreach (GridEXRow r in rows)
			{
				DocumentOfGroup exIm = new DocumentOfGroup();
				if (Conversion.ToInt(r.Cells["DocumentId"].Value) == 0)
				{
					MessageBox.Show("Chart Of Document Required in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				if (Conversion.ToInt(r.Cells["RequiredLevelId"].Value) == 0)
				{
					MessageBox.Show("RequiredLevel Required in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				if (Conversion.ToInt(r.Cells["DocumentProviderId"].Value) == 0)
				{
					MessageBox.Show("DocumentProvider Required in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				if (Conversion.ToInt(r.Cells["Original"].Value) == 0)
				{
					MessageBox.Show("Original Required in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				if (Conversion.ToInt(r.Cells["CriteriaDateTypeId"].Value) == 0)
				{
					MessageBox.Show("CriteriaDateType Required in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				if (Conversion.ToInt(r.Cells["BeforeDays"].Value) == 0 && Conversion.ToInt(r.Cells["AfterDays"].Value) == 0)
				{
					MessageBox.Show("BeforeDays or AfterDays Required in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				if (Conversion.ToInt(r.Cells["BeforeDays"].Value) > 0 && Conversion.ToInt(r.Cells["AfterDays"].Value) > 0)
				{
					MessageBox.Show("BeforeDays or AfterDays Can Be Added,not both.please check in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				if (Conversion.ToInt(r.Cells["PrioritySequence"].Value) == 0)
				{
					MessageBox.Show("PrioritySequence Required in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				if (!MandatoryExist)
				{
					MandatoryExist = Conversion.ToInt(r.Cells["RequiredLevelId"].Value) == 1;
				}
				exIm.customGroupId = Conversion.ToInt(CmbCustomGroup.Value);
				exIm.documentOfGroupId = Conversion.ToInt(r.Cells["Id"].Value);
				exIm.chartOfDocumentId = Conversion.ToInt(r.Cells["DocumentId"].Value);
				exIm.RequiredLevelId = Conversion.ToInt(r.Cells["RequiredLevelId"].Value);
				exIm.documentProviderId = Conversion.ToInt(r.Cells["DocumentProviderId"].Value);
				exIm.original = Conversion.ToInt(r.Cells["Original"].Value);
				exIm.duplicate = Conversion.ToInt(r.Cells["Duplicate"].Value);
				exIm.CriteriaDateTypeId = Conversion.ToInt(r.Cells["CriteriaDateTypeId"].Value);
				exIm.beforeDays = Conversion.ToInt(r.Cells["BeforeDays"].Value);
				exIm.afterDays = Conversion.ToInt(r.Cells["AfterDays"].Value);
				exIm.prioritySeqNo = Conversion.ToInt(r.Cells["PrioritySequence"].Value);
				exIm.Remarks = r.Cells["Remarks"].Value.ToString();
				exIm.isActive = Conversion.ToBool(r.Cells["Active"].Value);
				exIm.ApprovedDate = DateTime.Now;
				exIm.EntryDate = DateTime.Now;
				exIm.EntryUserId = UserAccount.ID;
				exIm.ApprovedUserId = UserAccount.ID;
				exIm.ModifyDate = DateTime.Now;
				exIm.ModifyUserId = UserAccount.ID;
				exIm.OrganizationId = UserAccount.OrganizationId;
				exIm.CompanyId = UserAccount.CompanyId;
				exIm.ActionId = ((exIm.documentOfGroupId <= 0) ? 1 : 2);
				obj.DocumentOfGroupList.Add(exIm);
			}
			if (!MandatoryExist)
			{
				throw new Exception("Grid Should have at least one Record With Mandatory Status...");
			}
			DocumentOfGroup.Save(obj);
			if (!Update)
			{
				MessageBox.Show("Save SuccessFully ");
			}
			else
			{
				MessageBox.Show("Update SuccessFully");
			}
			if (Confirm)
			{
				FormReset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ReadByCustomGroupId(int CustomGroupId)
	{
		try
		{
			RecId = CustomGroupId;
			dtdetailGrid.Rows.Clear();
			DataTable obj = DocumentOfGroup.ReadByCustomGroupId(CustomGroupId);
			if (obj != null && obj.Rows.Count > 0)
			{
				for (int i = 0; i < obj.Rows.Count; i++)
				{
					dtdetailGrid.Rows.Add(obj.Rows[i]["documentOfGroupId"], obj.Rows[i]["chartOfDocumentId"], obj.Rows[i]["documentName"], obj.Rows[i]["RequiredLevelId"], obj.Rows[i]["requiredLevelDesc"], obj.Rows[i]["documentProviderId"], obj.Rows[i]["original"], obj.Rows[i]["duplicate"], obj.Rows[i]["CriteriaDateTypeId"], obj.Rows[i]["beforeDays"], obj.Rows[i]["afterDays"], obj.Rows[i]["prioritySeqNo"], obj.Rows[i]["Remarks"], obj.Rows[i]["isActive"]);
				}
				grdAssignDoc.DataSource = dtdetailGrid;
				grdAssignDocSettings();
			}
			else
			{
				grdAssignDoc.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdAssignDoc_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "Original" || e.Column.Key == "Duplicate" || e.Column.Key == "BeforeDays" || e.Column.Key == "AfterDays" || e.Column.Key == "PrioritySequence") && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				txtFromDate.Focus();
			}
			else
			{
				((Control)(object)grdUnAssignDoc).Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DataGridHistoryFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = 243;
			if (rdentrydate.Checked)
			{
				if (txtFromDate.Checked)
				{
					obj.EntryDateFrom = txtFromDate.Value;
				}
				if (txtToDate.Checked)
				{
					obj.EntryDateTo = txtToDate.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (txtFromDate.Checked)
				{
					obj.ModifyDateFrom = txtFromDate.Value;
				}
				if (txtToDate.Checked)
				{
					obj.ModifyDateTo = txtToDate.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (txtFromDate.Checked)
				{
					obj.ApprovedDateFrom = txtFromDate.Value;
				}
				if (txtToDate.Checked)
				{
					obj.ApprovedDateTo = txtToDate.Value;
				}
			}
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			obj.CustomerGroupId = Conversion.ToInt(CmbCustomGroupHis.Value);
			obj.RefDocumentTypeId = Conversion.ToInt(CmbDocumentHis.Value);
			obj.RequiredLevelId = Conversion.ToInt(CmbRequiredLevelHis.Value);
			obj.DocumentProviderId = Conversion.ToInt(CmbDocumentProviderHis.Value);
			DataTable dtGridRegister = DocumentOfGroup.FormHistory(obj);
			if (dtGridRegister.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("DocumentTypeId", typeof(int));
				dtcol.Columns.Add("CustomGroupId", typeof(int));
				dtcol.Columns.Add("CustomGroup", typeof(string));
				dtcol.Columns.Add("Document", typeof(string));
				dtcol.Columns.Add("RequiredLevel", typeof(string));
				dtcol.Columns.Add("DocumentProvider", typeof(string));
				dtcol.Columns.Add("Original", typeof(int));
				dtcol.Columns.Add("Duplicate", typeof(int));
				dtcol.Columns.Add("CriteriaDateType", typeof(string));
				dtcol.Columns.Add("BeforeDays", typeof(int));
				dtcol.Columns.Add("AfterDays", typeof(int));
				dtcol.Columns.Add("PrioritySequence", typeof(int));
				dtcol.Columns.Add("Remarks", typeof(string));
				dtcol.Columns.Add("Active", typeof(bool));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("EntryUser", typeof(string));
				dtcol.Columns.Add("ModifyDate", typeof(DateTime));
				dtcol.Columns.Add("ModifyUser", typeof(string));
				dtcol.Columns.Add("ApprovedDate", typeof(DateTime));
				dtcol.Columns.Add("ApprovedUser", typeof(string));
				for (int i = 0; i < dtGridRegister.Rows.Count; i++)
				{
					dtcol.Rows.Add(dtGridRegister.Rows[i]["documentOfGroupId"], dtGridRegister.Rows[i]["DocumentTypeId"], dtGridRegister.Rows[i]["customGroupId"], dtGridRegister.Rows[i]["customGroupName"], dtGridRegister.Rows[i]["documentName"], dtGridRegister.Rows[i]["requiredLevelDesc"], dtGridRegister.Rows[i]["DocProviderName"], dtGridRegister.Rows[i]["original"], dtGridRegister.Rows[i]["duplicate"], dtGridRegister.Rows[i]["criteriaDateTypeDesc"], dtGridRegister.Rows[i]["beforeDays"], dtGridRegister.Rows[i]["afterDays"], dtGridRegister.Rows[i]["prioritySeqNo"], dtGridRegister.Rows[i]["Remarks"], dtGridRegister.Rows[i]["isActive"], dtGridRegister.Rows[i]["EntryDate"], dtGridRegister.Rows[i]["EntryUserName"], dtGridRegister.Rows[i]["ModifyDate"], dtGridRegister.Rows[i]["ModifyUserName"], dtGridRegister.Rows[i]["ApprovedDate"], dtGridRegister.Rows[i]["ApprovedUserName"]);
				}
				DataGridHistory.DataSource = dtcol;
				DataGridHistorySetting();
			}
			else
			{
				DataGridHistory.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DataGridHistorySetting()
	{
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["CustomGroupId"].Visible = false;
			DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm:ss";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm:ss";
			DataGridHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yyyy hh:mm:ss";
			DataGridHistory.RootTable.Columns["Original"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["Original"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["Original"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["Original"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["Duplicate"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["Duplicate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["Duplicate"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["Duplicate"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["BeforeDays"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["BeforeDays"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["BeforeDays"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["BeforeDays"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["PrioritySequence"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["PrioritySequence"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["PrioritySequence"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["PrioritySequence"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Key = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 50;
			DataGridHistory.RootTable.Columns["Edit"].Position = 0;
			DataGridHistory.FrozenColumns = 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnLoadAllHistoryNew_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)CmbCustomGroupHis).Text = string.Empty;
			((Control)(object)CmbDocumentHis).Text = string.Empty;
			((Control)(object)CmbRequiredLevelHis).Text = string.Empty;
			((Control)(object)CmbDocumentProviderHis).Text = string.Empty;
			DataGridHistory.DataSource = null;
			txtFromDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int customGroupId = Conversion.ToInt(r.Cells["CustomGroupId"].Value);
				CmbCustomGroup.Value = customGroupId;
				ShowData(Confirm: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
	}

	private void btnRefreshHistoryCombos_Click(object sender, EventArgs e)
	{
		try
		{
			ComboBindForHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			DataGridHistoryFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmDocumentOfGroup_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.N && tabControl1.SelectedIndex == 0)
			{
				btnnew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
				}
				else
				{
					tabControl1.SelectedIndex = 1;
				}
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 76;
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				if (formright.DoHaveSaveRight && tabControl1.SelectedIndex == 0)
				{
					btnsave_Click(null, null);
				}
				if (tabControl1.SelectedIndex == 1)
				{
					btnShow_Click(null, null);
				}
			}
			if (e.Alt)
			{
				_ = e.KeyCode;
				_ = 80;
			}
			if (tabControl1.SelectedIndex == 1 && (!e.Control || (e.KeyCode != Keys.D1 && e.KeyCode != Keys.NumPad1)) && e.Control && e.KeyCode != Keys.D2)
			{
				_ = e.KeyCode;
				_ = 98;
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					((Control)(object)grdUnAssignDoc).Focus();
				}
				else
				{
					txtFromDate.Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					((Control)(object)grdAssignDoc).Focus();
				}
				else
				{
					((Control)(object)DataGridHistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Right && tabControl1.SelectedIndex == 0)
			{
				if (((Control)(object)grdUnAssignDoc).Focused)
				{
					((Control)(object)grdAssignDoc).Focus();
				}
				else if (((Control)(object)grdAssignDoc).Focused)
				{
					((Control)(object)grdAssignDoc).Focus();
				}
				else
				{
					((Control)(object)grdUnAssignDoc).Focus();
				}
			}
			if (e.Control && e.Alt)
			{
				BtnShortCutkeys_Click(null, null);
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
			dt.Rows.Add("Ctrl+S", "For Save of Focused Grid");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New of Focused Grid");
			dt.Rows.Add("Ctrl+L", "For Load All");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Alt+1", "For Print 551 in History");
			dt.Rows.Add("Alt+2", "For Print 551_01 in History");
			dt.Rows.Add("Ctrl+F5", "For Focus on 'Sale Contract for Schedule' Grid");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+Right", "For Move focus From One Grid to Next");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Schedule Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Date Combo in Detail Box");
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

	private void grdUnAssignDoc_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow r = grdUnAssignDoc.CurrentRow;
				if (r != null && (int)r.RowType == 0 && grdUnAssignDoc.CurrentColumn != null)
				{
					_ = grdUnAssignDoc.CurrentColumn.Key == "";
					grdUnAssignDoc.UpdateData();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdAssignDoc_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdAssignDoc.CurrentRow;
			if (row != null && (int)row.RowType == 0 && grdAssignDoc.CurrentColumn != null)
			{
				if (e.Control && e.KeyCode == Keys.Space && grdAssignDoc.CurrentColumn.Key == "Delete")
				{
					DeletedtGridRow(row);
				}
				if (e.Control && e.KeyCode == Keys.Delete)
				{
					DeletedtGridRow(row);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			int customGroupId = Conversion.ToInt(r.Cells["CustomGroupId"].Value);
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DataGridHistory.CurrentColumn.Key == "Edit")
				{
					CmbCustomGroup.Value = customGroupId;
					ShowData(Confirm: false);
				}
				if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					DataGridHistory_LinkClicked(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				CmbCustomGroup.Value = customGroupId;
				ShowData(Confirm: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnDefineDocument_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineChartOfDocument(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnDefineCustomGroup_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineCustomGroup(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdAssignDoc_Click(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdAssignDoc.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0 && grdAssignDoc.CurrentColumn != null && grdAssignDoc.CurrentColumn.Key == "Active")
			{
				grdAssignDoc.UpdateData();
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
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Expected O, but got Unknown
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Expected O, but got Unknown
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Expected O, but got Unknown
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Expected O, but got Unknown
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Expected O, but got Unknown
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Expected O, but got Unknown
		//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cb: Expected O, but got Unknown
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Expected O, but got Unknown
		//IL_08f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ff: Expected O, but got Unknown
		//IL_090c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0916: Expected O, but got Unknown
		//IL_0923: Unknown result type (might be due to invalid IL or missing references)
		//IL_092d: Expected O, but got Unknown
		//IL_1c68: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c72: Expected O, but got Unknown
		//IL_1c7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c89: Expected O, but got Unknown
		GridEXLayout grdAssignDoc_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.SDT.frmDocumentOfGroup));
		GridEXLayout grdUnAssignDoc_DesignTimeLayout = new GridEXLayout();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.grdAssignDoc = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label14 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdUnAssignDoc = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label13 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.BtnTransferToAssign = new System.Windows.Forms.ToolStripButton();
		this.panel6 = new System.Windows.Forms.Panel();
		this.btnShowByCustomGroup = new System.Windows.Forms.Button();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label12 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshUnAssigned = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.BtnDefineDocument = new System.Windows.Forms.ToolStripButton();
		this.BtnDefineCustomGroup = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.CmbCustomGroup = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel26 = new System.Windows.Forms.Panel();
		this.panel27 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel24 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar7 = new grid.User_Conrtols.CtrlGrdBar();
		this.label5 = new System.Windows.Forms.Label();
		this.panel25 = new System.Windows.Forms.Panel();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.CmbCustomGroupHis = new UltraCombo();
		this.CmbDocumentHis = new UltraCombo();
		this.txtToDate = new System.Windows.Forms.DateTimePicker();
		this.label50 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.CmbRequiredLevelHis = new UltraCombo();
		this.txtFromDate = new System.Windows.Forms.DateTimePicker();
		this.label19 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.CmbDocumentProviderHis = new UltraCombo();
		this.btnShowHis = new System.Windows.Forms.Button();
		this.toolStrip4 = new System.Windows.Forms.ToolStrip();
		this.btnLoadAllHistoryNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistoryCombos = new System.Windows.Forms.ToolStripButton();
		this.tabControlMain = new System.Windows.Forms.TabControl();
		this.tabPageMain = new System.Windows.Forms.TabPage();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdAssignDoc).BeginInit();
		this.panel13.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdUnAssignDoc).BeginInit();
		this.panel4.SuspendLayout();
		this.panel7.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomGroup).BeginInit();
		this.tabPage3.SuspendLayout();
		this.panel26.SuspendLayout();
		this.panel27.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel24.SuspendLayout();
		this.panel25.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomGroupHis).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDocumentHis).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRequiredLevelHis).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDocumentProviderHis).BeginInit();
		this.toolStrip4.SuspendLayout();
		this.tabControlMain.SuspendLayout();
		this.tabPageMain.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(3, 3);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1070, 526);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1062, 497);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel12);
		this.panel2.Controls.Add(this.panel10);
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1056, 491);
		this.panel2.TabIndex = 0;
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.grdAssignDoc);
		this.panel12.Controls.Add(this.panel13);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel12.Location = new System.Drawing.Point(360, 90);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(696, 401);
		this.panel12.TabIndex = 3;
		grdAssignDoc_DesignTimeLayout.LayoutString = resources.GetString("grdAssignDoc_DesignTimeLayout.LayoutString");
		this.grdAssignDoc.DesignTimeLayout = grdAssignDoc_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdAssignDoc).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdAssignDoc.DynamicFiltering = true;
		this.grdAssignDoc.FilterMode = (FilterMode)2;
		this.grdAssignDoc.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdAssignDoc).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdAssignDoc.GroupByBoxVisible = false;
		this.grdAssignDoc.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.grdAssignDoc).Location = new System.Drawing.Point(0, 31);
		((System.Windows.Forms.Control)(object)this.grdAssignDoc).Name = "grdAssignDoc";
		((ScrollableBase)this.grdAssignDoc).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdAssignDoc).Size = new System.Drawing.Size(696, 370);
		((System.Windows.Forms.Control)(object)this.grdAssignDoc).TabIndex = 7;
		this.grdAssignDoc.TotalRow = (InheritableBoolean)1;
		this.grdAssignDoc.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdAssignDoc).VisualStyle = (VisualStyle)4;
		this.grdAssignDoc.CellUpdated += new ColumnActionEventHandler(grdAssignDoc_CellUpdated);
		this.grdAssignDoc.UpdatingCell += new UpdatingCellEventHandler(grdAssignDoc_UpdatingCell);
		this.grdAssignDoc.ColumnButtonClick += new ColumnActionEventHandler(grdAssignDoc_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdAssignDoc).Click += new System.EventHandler(grdAssignDoc_Click);
		((System.Windows.Forms.Control)(object)this.grdAssignDoc).KeyDown += new System.Windows.Forms.KeyEventHandler(grdAssignDoc_KeyDown);
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label14);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(696, 31);
		this.panel13.TabIndex = 6;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(3, 7);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(145, 17);
		this.label14.TabIndex = 54;
		this.label14.Text = "Assigned Document";
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdUnAssignDoc);
		this.panel10.Controls.Add(this.panel4);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel10.Location = new System.Drawing.Point(0, 90);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(360, 401);
		this.panel10.TabIndex = 4;
		this.grdUnAssignDoc.AllowEdit = (InheritableBoolean)2;
		grdUnAssignDoc_DesignTimeLayout.LayoutString = resources.GetString("grdUnAssignDoc_DesignTimeLayout.LayoutString");
		this.grdUnAssignDoc.DesignTimeLayout = grdUnAssignDoc_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdUnAssignDoc).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdUnAssignDoc.DynamicFiltering = true;
		this.grdUnAssignDoc.FilterMode = (FilterMode)2;
		this.grdUnAssignDoc.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdUnAssignDoc).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdUnAssignDoc.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.grdUnAssignDoc).Location = new System.Drawing.Point(0, 31);
		((System.Windows.Forms.Control)(object)this.grdUnAssignDoc).Name = "grdUnAssignDoc";
		((ScrollableBase)this.grdUnAssignDoc).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdUnAssignDoc).Size = new System.Drawing.Size(360, 370);
		((System.Windows.Forms.Control)(object)this.grdUnAssignDoc).TabIndex = 0;
		this.grdUnAssignDoc.TotalRow = (InheritableBoolean)1;
		this.grdUnAssignDoc.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdUnAssignDoc).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdUnAssignDoc).KeyDown += new System.Windows.Forms.KeyEventHandler(grdUnAssignDoc_KeyDown);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add(this.panel7);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(360, 31);
		this.panel4.TabIndex = 6;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(3, 7);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(170, 17);
		this.label13.TabIndex = 54;
		this.label13.Text = "Un-Assigned Document";
		this.panel7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.panel7.Controls.Add(this.toolStrip3);
		this.panel7.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel7.Location = new System.Drawing.Point(326, 2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(32, 27);
		this.panel7.TabIndex = 1;
		this.toolStrip3.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.BtnTransferToAssign });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(32, 27);
		this.toolStrip3.TabIndex = 7;
		this.toolStrip3.Text = "toolStrip3";
		this.BtnTransferToAssign.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnTransferToAssign.Image = Architecture.WinApp.Properties.Resources.Forward2;
		this.BtnTransferToAssign.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnTransferToAssign.Name = "BtnTransferToAssign";
		this.BtnTransferToAssign.Size = new System.Drawing.Size(24, 24);
		this.BtnTransferToAssign.ToolTipText = "Click to transfer selected rows to AssigneddocumentGrid";
		this.BtnTransferToAssign.Click += new System.EventHandler(BtnTransferToAssign_Click);
		this.panel6.BackColor = System.Drawing.Color.White;
		this.panel6.Controls.Add(this.btnShowByCustomGroup);
		this.panel6.Controls.Add(this.panel3);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomGroup);
		this.panel6.Controls.Add(this.label2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1056, 90);
		this.panel6.TabIndex = 3;
		this.btnShowByCustomGroup.BackColor = System.Drawing.Color.Teal;
		this.btnShowByCustomGroup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowByCustomGroup.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowByCustomGroup.ForeColor = System.Drawing.Color.White;
		this.btnShowByCustomGroup.Location = new System.Drawing.Point(330, 57);
		this.btnShowByCustomGroup.Name = "btnShowByCustomGroup";
		this.btnShowByCustomGroup.Size = new System.Drawing.Size(59, 28);
		this.btnShowByCustomGroup.TabIndex = 61;
		this.btnShowByCustomGroup.Text = "Show";
		this.btnShowByCustomGroup.UseVisualStyleBackColor = false;
		this.btnShowByCustomGroup.Click += new System.EventHandler(btnShowByCustomGroup_Click);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label12);
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1056, 55);
		this.panel3.TabIndex = 0;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(2, 31);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(295, 19);
		this.label12.TabIndex = 54;
		this.label12.Text = "Assign Document To Custom Group";
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.btnnew, this.BtnRefreshUnAssigned, this.btnsave, this.BtnDefineDocument, this.BtnDefineCustomGroup, this.BtnShortCutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1056, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.BtnRefreshUnAssigned.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshUnAssigned.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshUnAssigned.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshUnAssigned.Name = "BtnRefreshUnAssigned";
		this.BtnRefreshUnAssigned.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshUnAssigned.Text = "&Refresh";
		this.BtnRefreshUnAssigned.Click += new System.EventHandler(BtnRefreshUnAssigned_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(117, 24);
		this.btnsave.Text = "&Save/Update";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.BtnDefineDocument.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnDefineDocument.Image = Architecture.WinApp.Properties.Resources.Account_Reports;
		this.BtnDefineDocument.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnDefineDocument.Name = "BtnDefineDocument";
		this.BtnDefineDocument.Size = new System.Drawing.Size(158, 24);
		this.BtnDefineDocument.Text = "&Chart Of Document";
		this.BtnDefineDocument.Click += new System.EventHandler(BtnDefineDocument_Click);
		this.BtnDefineCustomGroup.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnDefineCustomGroup.Image = Architecture.WinApp.Properties.Resources.Account_Reports;
		this.BtnDefineCustomGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnDefineCustomGroup.Name = "BtnDefineCustomGroup";
		this.BtnDefineCustomGroup.Size = new System.Drawing.Size(172, 24);
		this.BtnDefineCustomGroup.Text = "&Define Custom_Group";
		this.BtnDefineCustomGroup.Click += new System.EventHandler(BtnDefineCustomGroup_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.CmbCustomGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance10;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance16).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Location = new System.Drawing.Point(97, 58);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Name = "CmbCustomGroup";
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Size = new System.Drawing.Size(231, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).TabIndex = 59;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(7, 64);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(84, 15);
		this.label2.TabIndex = 60;
		this.label2.Text = "Custom Group";
		this.tabPage3.Controls.Add(this.panel26);
		this.tabPage3.Controls.Add(this.panel25);
		this.tabPage3.Controls.Add(this.toolStrip4);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1062, 497);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "History";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel26.Controls.Add(this.panel27);
		this.panel26.Controls.Add(this.panel24);
		this.panel26.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel26.Location = new System.Drawing.Point(0, 105);
		this.panel26.Name = "panel26";
		this.panel26.Size = new System.Drawing.Size(1062, 392);
		this.panel26.TabIndex = 55;
		this.panel27.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel27.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel27.Location = new System.Drawing.Point(0, 27);
		this.panel27.Name = "panel27";
		this.panel27.Size = new System.Drawing.Size(1062, 365);
		this.panel27.TabIndex = 54;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		DataGridHistory_DesignTimeLayout.LayoutString = resources.GetString("DataGridHistory_DesignTimeLayout.LayoutString");
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupByBoxVisible = false;
		this.DataGridHistory.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1062, 365);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 1;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel24.BackColor = System.Drawing.Color.Teal;
		this.panel24.Controls.Add(this.ctrlGrdBar7);
		this.panel24.Controls.Add(this.label5);
		this.panel24.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel24.Location = new System.Drawing.Point(0, 0);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(1062, 27);
		this.panel24.TabIndex = 52;
		this.ctrlGrdBar7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar7.FormName = this;
		this.ctrlGrdBar7.Location = new System.Drawing.Point(1023, 3);
		this.ctrlGrdBar7.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar7.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar7.Name = "ctrlGrdBar7";
		this.ctrlGrdBar7.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar7.TabIndex = 56;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(6, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(67, 19);
		this.label5.TabIndex = 55;
		this.label5.Text = "History";
		this.panel25.Controls.Add(this.rdapproveddate);
		this.panel25.Controls.Add(this.rdmodifydate);
		this.panel25.Controls.Add(this.rdentrydate);
		this.panel25.Controls.Add(this.groupBox2);
		this.panel25.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel25.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel25.Location = new System.Drawing.Point(0, 27);
		this.panel25.Name = "panel25";
		this.panel25.Size = new System.Drawing.Size(1062, 78);
		this.panel25.TabIndex = 0;
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(961, 48);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 3;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(961, 27);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 2;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Checked = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(961, 9);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 1;
		this.rdentrydate.TabStop = true;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomGroupHis);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDocumentHis);
		this.groupBox2.Controls.Add(this.txtToDate);
		this.groupBox2.Controls.Add(this.label50);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRequiredLevelHis);
		this.groupBox2.Controls.Add(this.txtFromDate);
		this.groupBox2.Controls.Add(this.label19);
		this.groupBox2.Controls.Add(this.label51);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDocumentProviderHis);
		this.groupBox2.Controls.Add(this.btnShowHis);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(4, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(951, 72);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(222, 19);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(84, 15);
		this.label1.TabIndex = 577;
		this.label1.Text = "Custom Group";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(222, 48);
		this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(64, 15);
		this.label10.TabIndex = 577;
		this.label10.Text = "Document";
		this.CmbCustomGroupHis.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomGroupHis.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomGroupHis.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomGroupHis.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomGroupHis).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbCustomGroupHis).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomGroupHis).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomGroupHis).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomGroupHis).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomGroupHis).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomGroupHis).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbCustomGroupHis).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomGroupHis).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomGroupHis).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomGroupHis).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbCustomGroupHis).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomGroupHis).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomGroupHis).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance25).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomGroupHis).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbCustomGroupHis).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomGroupHis).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomGroupHis).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomGroupHis).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbCustomGroupHis).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomGroupHis).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomGroupHis).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbCustomGroupHis).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomGroupHis).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbCustomGroupHis).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomGroupHis).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomGroupHis).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomGroupHis).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomGroupHis.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomGroupHis).Location = new System.Drawing.Point(307, 13);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroupHis).Name = "CmbCustomGroupHis";
		((System.Windows.Forms.Control)(object)this.CmbCustomGroupHis).Size = new System.Drawing.Size(217, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroupHis).TabIndex = 2;
		this.CmbDocumentHis.AllowNull = (DefaultableBoolean)1;
		this.CmbDocumentHis.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDocumentHis.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDocumentHis.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDocumentHis).DisplayLayout.Appearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbDocumentHis).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDocumentHis).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDocumentHis).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDocumentHis).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance45;
		((SpecialBoxBase)((UltraGridBase)this.CmbDocumentHis).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDocumentHis).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbDocumentHis).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDocumentHis).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDocumentHis).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDocumentHis).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbDocumentHis).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDocumentHis).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDocumentHis).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance51).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDocumentHis).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbDocumentHis).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDocumentHis).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDocumentHis).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDocumentHis).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbDocumentHis).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDocumentHis).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDocumentHis).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbDocumentHis).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDocumentHis).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbDocumentHis).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDocumentHis).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDocumentHis).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDocumentHis).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDocumentHis.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDocumentHis).Location = new System.Drawing.Point(307, 42);
		((System.Windows.Forms.Control)(object)this.CmbDocumentHis).Name = "CmbDocumentHis";
		((System.Windows.Forms.Control)(object)this.CmbDocumentHis).Size = new System.Drawing.Size(217, 26);
		((System.Windows.Forms.Control)(object)this.CmbDocumentHis).TabIndex = 3;
		this.txtToDate.CustomFormat = "dd-MMM-yy";
		this.txtToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDate.Location = new System.Drawing.Point(66, 44);
		this.txtToDate.Name = "txtToDate";
		this.txtToDate.ShowCheckBox = true;
		this.txtToDate.Size = new System.Drawing.Size(150, 23);
		this.txtToDate.TabIndex = 1;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(528, 19);
		this.label50.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(84, 15);
		this.label50.TabIndex = 16;
		this.label50.Text = "Required Level";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(4, 48);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(48, 15);
		this.label11.TabIndex = 575;
		this.label11.Text = "Date To";
		this.CmbRequiredLevelHis.AllowNull = (DefaultableBoolean)1;
		this.CmbRequiredLevelHis.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRequiredLevelHis.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRequiredLevelHis.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRequiredLevelHis).DisplayLayout.Appearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbRequiredLevelHis).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRequiredLevelHis).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRequiredLevelHis).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRequiredLevelHis).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance32;
		((SpecialBoxBase)((UltraGridBase)this.CmbRequiredLevelHis).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRequiredLevelHis).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbRequiredLevelHis).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRequiredLevelHis).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRequiredLevelHis).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRequiredLevelHis).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbRequiredLevelHis).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRequiredLevelHis).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRequiredLevelHis).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance38).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRequiredLevelHis).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbRequiredLevelHis).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRequiredLevelHis).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRequiredLevelHis).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRequiredLevelHis).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbRequiredLevelHis).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRequiredLevelHis).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRequiredLevelHis).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbRequiredLevelHis).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRequiredLevelHis).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbRequiredLevelHis).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRequiredLevelHis).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRequiredLevelHis).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRequiredLevelHis).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRequiredLevelHis.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRequiredLevelHis).Location = new System.Drawing.Point(641, 13);
		((System.Windows.Forms.Control)(object)this.CmbRequiredLevelHis).Name = "CmbRequiredLevelHis";
		((System.Windows.Forms.Control)(object)this.CmbRequiredLevelHis).Size = new System.Drawing.Size(243, 26);
		((System.Windows.Forms.Control)(object)this.CmbRequiredLevelHis).TabIndex = 4;
		this.txtFromDate.CustomFormat = "dd-MMM-yy";
		this.txtFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromDate.Location = new System.Drawing.Point(66, 15);
		this.txtFromDate.Name = "txtFromDate";
		this.txtFromDate.ShowCheckBox = true;
		this.txtFromDate.Size = new System.Drawing.Size(150, 23);
		this.txtFromDate.TabIndex = 0;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(528, 48);
		this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(111, 15);
		this.label19.TabIndex = 18;
		this.label19.Text = "Document Provider";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(4, 19);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(63, 15);
		this.label51.TabIndex = 572;
		this.label51.Text = "Date From";
		this.CmbDocumentProviderHis.AllowNull = (DefaultableBoolean)1;
		this.CmbDocumentProviderHis.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDocumentProviderHis.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDocumentProviderHis.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDocumentProviderHis).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDocumentProviderHis.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDocumentProviderHis).Location = new System.Drawing.Point(641, 42);
		((System.Windows.Forms.Control)(object)this.CmbDocumentProviderHis).Name = "CmbDocumentProviderHis";
		((System.Windows.Forms.Control)(object)this.CmbDocumentProviderHis).Size = new System.Drawing.Size(243, 26);
		((System.Windows.Forms.Control)(object)this.CmbDocumentProviderHis).TabIndex = 5;
		this.btnShowHis.BackColor = System.Drawing.Color.Teal;
		this.btnShowHis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHis.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHis.ForeColor = System.Drawing.Color.White;
		this.btnShowHis.Location = new System.Drawing.Point(889, 42);
		this.btnShowHis.Name = "btnShowHis";
		this.btnShowHis.Size = new System.Drawing.Size(58, 26);
		this.btnShowHis.TabIndex = 6;
		this.btnShowHis.Text = "Show";
		this.btnShowHis.UseVisualStyleBackColor = false;
		this.btnShowHis.Click += new System.EventHandler(btnShow_Click);
		this.toolStrip4.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnLoadAllHistoryNew, this.btnRefreshHistoryCombos });
		this.toolStrip4.Location = new System.Drawing.Point(0, 0);
		this.toolStrip4.Name = "toolStrip4";
		this.toolStrip4.Size = new System.Drawing.Size(1062, 27);
		this.toolStrip4.TabIndex = 8;
		this.toolStrip4.Text = "toolStrip4";
		this.btnLoadAllHistoryNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadAllHistoryNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnLoadAllHistoryNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAllHistoryNew.Name = "btnLoadAllHistoryNew";
		this.btnLoadAllHistoryNew.Size = new System.Drawing.Size(59, 24);
		this.btnLoadAllHistoryNew.Text = "New";
		this.btnLoadAllHistoryNew.Click += new System.EventHandler(btnLoadAllHistoryNew_Click);
		this.btnRefreshHistoryCombos.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistoryCombos.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistoryCombos.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistoryCombos.Name = "btnRefreshHistoryCombos";
		this.btnRefreshHistoryCombos.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistoryCombos.Text = "&Refresh";
		this.btnRefreshHistoryCombos.Click += new System.EventHandler(btnRefreshHistoryCombos_Click);
		this.tabControlMain.Controls.Add(this.tabPageMain);
		this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControlMain.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControlMain.Location = new System.Drawing.Point(0, 0);
		this.tabControlMain.Name = "tabControlMain";
		this.tabControlMain.SelectedIndex = 0;
		this.tabControlMain.Size = new System.Drawing.Size(1084, 561);
		this.tabControlMain.TabIndex = 1;
		this.tabPageMain.Controls.Add(this.tabControl1);
		this.tabPageMain.Location = new System.Drawing.Point(4, 25);
		this.tabPageMain.Name = "tabPageMain";
		this.tabPageMain.Padding = new System.Windows.Forms.Padding(3);
		this.tabPageMain.Size = new System.Drawing.Size(1076, 532);
		this.tabPageMain.TabIndex = 0;
		this.tabPageMain.Text = "Main Form";
		this.tabPageMain.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1084, 561);
		base.Controls.Add(this.tabControlMain);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmDocumentOfGroup";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmDocumentOfGroup_FormClosed);
		base.Load += new System.EventHandler(frmDocumentOfGroup_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmDocumentOfGroup_KeyDown);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdAssignDoc).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdUnAssignDoc).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomGroup).EndInit();
		this.tabPage3.ResumeLayout(false);
		this.tabPage3.PerformLayout();
		this.panel26.ResumeLayout(false);
		this.panel27.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel24.ResumeLayout(false);
		this.panel24.PerformLayout();
		this.panel25.ResumeLayout(false);
		this.panel25.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomGroupHis).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDocumentHis).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRequiredLevelHis).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDocumentProviderHis).EndInit();
		this.toolStrip4.ResumeLayout(false);
		this.toolStrip4.PerformLayout();
		this.tabControlMain.ResumeLayout(false);
		this.tabPageMain.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

}
