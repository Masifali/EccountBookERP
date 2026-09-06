using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Export;
using Architecture.BLL.SDT;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.SDT;
using Architecture.WinApp.Account_Definition;
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

public class frmShipmentDocumentSchedule : Form
{
	private UserAccount UserAccount = new UserAccount();

	private AttachmentForDetailRows ATd = new AttachmentForDetailRows();

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private DataTable dtUnAssigned = new DataTable();

	private DataTable dtdetailGrid = new DataTable();

	private DataTable dtStatus = new DataTable();

	private DataTable dtHistory = new DataTable();

	private int RecId;

	private int DefaultDaysToLessFromHistoryFromDate;

	private List<ShipmentDocumentSchedule> LstRemoveRecord = new List<ShipmentDocumentSchedule>();

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel6;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

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

	internal UltraCombo CmbScheduleNo;

	private Label label2;

	private Button btnShowByCustomGroup;

	private ToolStripButton BtnShortCutkeys;

	private Panel panel7;

	private ToolStrip toolStrip3;

	private ToolStripButton BtnSave;

	private ToolStripButton BtnRefreshUnAssigned;

	internal GridEX grdUnAssignDoc;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton rdentrydate;

	private Label label1;

	internal UltraCombo CmbCustomGroupHis;

	internal GridEX DataGridHistory;

	internal GridEX grdAssignDoc;

	private Panel panel1;

	private ToolStrip toolStrip2;

	private ToolStripButton BtnDelete;

	private CheckBox ChkIsLockSchedule;

	internal UltraCombo CmbCustomGroup;

	private Label label3;

	private ToolStripButton BtnUpdate;

	private RadioButton RadReferedNotRefDoc;

	private RadioButton RadReferedRefDoc;

	private Label label4;

	private Label label67;

	internal UltraCombo CmbSalesPersonContract;

	internal UltraCombo CmbCustomerContract;

	private ToolStripButton btnAttachment;

	public frmShipmentDocumentSchedule(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmShipmentDocumentSchedule_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1030, 600);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			BtnDelete.Enabled = formright.DoHaveSaveRight;
			BtnSave.Enabled = formright.DoHaveSaveRight;
			ctrlGrdBar7.mGridChooseFielder.Visible = false;
			ctrlGrdBar7.mGridSaveLayouts.Visible = false;
			ctrlGrdBar7.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar7.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar7.GroupCollaps.Visible = false;
			ctrlGrdBar7.GroupExpand.Visible = false;
			ctrlGrdBar7.removeLayoutToolStripMenuItem.Visible = false;
			dtUnAssigned.Columns.Add("DocumentId", typeof(int));
			dtUnAssigned.Columns.Add("Document", typeof(string));
			dtUnAssigned.Columns.Add("RequiredLevelId", typeof(int));
			dtUnAssigned.Columns.Add("RequiredLevel");
			grdUnAssignDoc.DataSource = dtUnAssigned;
			grdUnAssignDocSetting();
			dtdetailGrid.Columns.Add("Id", typeof(int));
			dtdetailGrid.Columns.Add("CustomGroupId", typeof(int));
			dtdetailGrid.Columns.Add("CustomGroup", typeof(string));
			dtdetailGrid.Columns.Add("DocumentId", typeof(int));
			dtdetailGrid.Columns.Add("Document", typeof(string));
			dtdetailGrid.Columns.Add("Original", typeof(int));
			dtdetailGrid.Columns.Add("Duplicate", typeof(int));
			dtdetailGrid.Columns.Add("BaseCriteriaDate", typeof(DateTime));
			dtdetailGrid.Columns.Add("BeforeDays", typeof(int));
			dtdetailGrid.Columns.Add("AfterDays", typeof(int));
			dtdetailGrid.Columns.Add("DueDate", typeof(DateTime));
			dtdetailGrid.Columns.Add("ReadyDate", typeof(string));
			dtdetailGrid.Columns.Add("DocStatus", typeof(int));
			dtdetailGrid.Columns.Add("PrioritySequence", typeof(int));
			dtdetailGrid.Columns.Add("Remarks", typeof(string));
			dtdetailGrid.Columns.Add("DocumentProviderName", typeof(string));
			dtdetailGrid.Columns.Add("AddAttachments", typeof(int));
			grdAssignDoc.DataSource = dtdetailGrid;
			grdAssignDocSettings();
			CombosFromContractBind(CombosFromContractDbCall());
			ScheduleNoComboBind(ScheduleNoComboDbCall());
			StatusFill();
			((Control)(object)grdUnAssignDoc).Focus();
			tabControl1.TabPages.Remove(tabPage3);
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

	private DataTable ScheduleNoComboDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		try
		{
			return ExImLcOrderShipmentSchedule.GetContactScheduleNoExistsInShipmentDocumentSchedule(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				SupplierCustomerId = Conversion.ToInt(CmbCustomerContract.Value),
				SalesPersonId = Conversion.ToInt(CmbSalesPersonContract.Value),
				ReferedInSDS = RadReferedRefDoc.Checked
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ScheduleNoComboBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbScheduleNo, dt, "Id", "ScheduleCode", "Schedule / Invoice No", AllColumns: true, Conversion.ToInt(CmbScheduleNo.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			if (dt != null && dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbScheduleNo).DisplayLayout.Bands[0].Columns[1].Width = 250;
				((UltraGridBase)CmbScheduleNo).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbScheduleNo).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbScheduleNo).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)CmbScheduleNo).DisplayLayout.Bands[0].Columns[7].Hidden = true;
				((UltraGridBase)CmbScheduleNo).DisplayLayout.Bands[0].Columns[10].Hidden = true;
				((UltraGridBase)CmbScheduleNo).DisplayLayout.Bands[0].Columns[11].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomGroupCombo(int SupplierCustomerId, int CustomGroupId)
	{
		try
		{
			int id = Conversion.ToInt(CmbCustomGroup.Value);
			DataTable lst = ClientCustomGroup.GetCustomGroupAllocatedToCustomerBySupplierCustomerId(UserAccount.OrganizationId, UserAccount.CompanyId, SupplierCustomerId);
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDL(lst, CmbCustomGroup, "CustomGroupId", "CustomGroup", "CustomGroup", ZeroIndex: true);
				if (id > 0)
				{
					if (lst.AsEnumerable().Any((DataRow row) => row.Field<int>("CustomGroupId") == id))
					{
						CmbCustomGroup.Value = id;
					}
					else
					{
						((Control)(object)CmbCustomGroup).Text = string.Empty;
					}
				}
				if (CustomGroupId > 0)
				{
					if (lst.AsEnumerable().Any((DataRow row) => row.Field<int>("CustomGroupId") == CustomGroupId))
					{
						CmbCustomGroup.Value = CustomGroupId;
					}
					else
					{
						((Control)(object)CmbCustomGroup).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbCustomGroup).DataSource = null;
				((Control)(object)CmbCustomGroup).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboBindForHistory()
	{
	}

	private void CmbScheduleNo_Leave(object sender, EventArgs e)
	{
		try
		{
			bool IsLockSchedule = false;
			int RefDocId = Conversion.ToInt(CmbScheduleNo.Value);
			bool isForInvoiced = false;
			if (((UltraGridBase)CmbScheduleNo).ActiveRow != null && RefDocId > 0)
			{
				int SupplierId = Conversion.ToInt(((UltraDropDownBase)CmbScheduleNo).SelectedRow.Cells[3].Value);
				IsLockSchedule = Conversion.ToBool(((UltraDropDownBase)CmbScheduleNo).SelectedRow.Cells[4].Value);
				int CustomGroupId = Conversion.ToInt(((UltraDropDownBase)CmbScheduleNo).SelectedRow.Cells[7].Value);
				CustomGroupCombo(SupplierId, CustomGroupId);
				isForInvoiced = ((UltraDropDownBase)CmbScheduleNo).SelectedRow.Cells["IsForInvoiced"].Value != null && Conversion.ToBool(((UltraDropDownBase)CmbScheduleNo).SelectedRow.Cells["IsForInvoiced"].Value);
			}
			else
			{
				((UltraGridBase)CmbCustomGroup).DataSource = null;
				((Control)(object)CmbCustomGroup).Text = "";
			}
			btnAttachment.Text = (isForInvoiced ? "Invoice Attachments" : "Schedule Attachments");
			ChkIsLockSchedule.Checked = IsLockSchedule;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StatusFill()
	{
		try
		{
			dtStatus = CommonServices.StaticColumnsService("ShipmentDocumentScheduleStatus");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable CombosFromContractDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return ExImLcOrder.GetDataForDropDownFromExportContract(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CombosFromContractBind(DataTable dtcombo)
	{
		try
		{
			DataTable dtST = new DataTable();
			dtST.Columns.Add("Id", typeof(int));
			dtST.Columns.Add("Name");
			DataTable dtCustomer = dtST.Clone();
			if (dtcombo.Rows.Count > 0)
			{
				for (int i = 0; i < dtcombo.Rows.Count; i++)
				{
					if (Conversion.ToString(dtcombo.Rows[i]["ActivityType"]) == "SalePerson")
					{
						dtST.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["name"]);
					}
					else if (Conversion.ToString(dtcombo.Rows[i]["ActivityType"]) == "Customer")
					{
						dtCustomer.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["name"]);
					}
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbSalesPersonContract, dtST, "Id", "Name", "Sales Person", AllColumns: true, Conversion.ToInt(CmbSalesPersonContract.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbCustomerContract, dtCustomer, "Id", "Name", "Customer", AllColumns: true, Conversion.ToInt(CmbCustomerContract.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbCustomerContract_Leave(object sender, EventArgs e)
	{
		ScheduleNoComboBind(ScheduleNoComboDbCall());
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
			ATd = new AttachmentForDetailRows();
			ATd.lst.Clear();
			RecId = 0;
			LstRemoveRecord.Clear();
			((Control)(object)CmbScheduleNo).Text = "";
			grdAssignDoc.DataSource = null;
			grdUnAssignDoc.DataSource = null;
			dtUnAssigned.Clear();
			dtdetailGrid.Clear();
			CmbScheduleNo.Focus();
			RadReferedRefDoc.Checked = true;
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
			ScheduleNoComboBind(ScheduleNoComboDbCall());
			StatusFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void frmShipmentDocumentSchedule_FormClosed(object sender, FormClosedEventArgs e)
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
			int RefDocId = Conversion.ToInt(CmbScheduleNo.Value);
			int CustomGroupId = Conversion.ToInt(CmbCustomGroup.Value);
			if (RefDocId == 0)
			{
				grdUnAssignDoc.DataSource = null;
				grdAssignDoc.DataSource = null;
				CmbScheduleNo.Focus();
				throw new Exception("Please Select Schedule First");
			}
			if (CustomGroupId == 0)
			{
				grdUnAssignDoc.DataSource = null;
				grdAssignDoc.DataSource = null;
				CmbCustomGroup.Focus();
				throw new Exception("Please Select CustomGroup First");
			}
			int RefDocumentTypeId = Conversion.ToInt(((UltraDropDownBase)CmbScheduleNo).SelectedRow.Cells[2].Value);
			if (Confirm)
			{
				if (grdAssignDoc.GetDataRows().Count() > 0)
				{
					if (MessageBox.Show("grd Allocated have Records,Do you want to refresh?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
				}
				else if (grdUnAssignDoc.GetDataRows().Count() > 0 && MessageBox.Show("grd UnAllocated have Records,Do you want to refresh?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				ATd = new AttachmentForDetailRows();
				ATd.lst.Clear();
			}
			FillUnAssignData(RefDocId, RefDocumentTypeId, CustomGroupId);
			ReadByRefDocumentTypeId(RefDocId, RefDocumentTypeId, CustomGroupId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadByRefDocumentTypeId(int RefDocId, int RefDocumentTypeId, int CustomGroupId)
	{
		try
		{
			RecId = RefDocId;
			dtdetailGrid.Clear();
			DataTable obj = ShipmentDocumentSchedule.ReadByRefDocument(RefDocId, RefDocumentTypeId, CustomGroupId);
			if (obj != null && obj.Rows.Count > 0)
			{
				List<DMSAttachments> list = (from r in global::Architecture.BLL.DMSAttachments.GetByID(RefDocId, base.Name)
					where r.DetailWiseAttachment
					select r).ToList();
				ATd = new AttachmentForDetailRows();
				foreach (DMSAttachments Attachment in list)
				{
					TempAttachmentsDetail TA = new TempAttachmentsDetail();
					TA.Id = Attachment.Id;
					TA.Path = Attachment.Attachment;
					TA.RefAccountId = Attachment.RefAccountId;
					TA.DetailRefId = Attachment.LineId;
					TA.CustomPath = Attachment.UploadedFileCustomName;
					ATd.lst.Add(TA);
				}
				int i;
				for (i = 0; i < obj.Rows.Count; i++)
				{
					int count = ATd.lst.Count((TempAttachmentsDetail r) => r.DetailRefId == Conversion.ToInt(obj.Rows[i]["chartOfDocumentId"]));
					DateTime DueDate = Conversion.ToDateTime(obj.Rows[i]["BaseCriteriaDate"]);
					DateTime ReadyDate = Conversion.ToDateTime(obj.Rows[i]["ReadyDate"]);
					int beforeDays = Conversion.ToInt(obj.Rows[i]["beforeDays"]);
					int afterDays = Conversion.ToInt(obj.Rows[i]["afterDays"]);
					if (beforeDays > 0)
					{
						DueDate = DueDate.AddDays(-beforeDays);
					}
					else if (afterDays > 0)
					{
						DueDate = DueDate.AddDays(afterDays);
					}
					dtdetailGrid.Rows.Add(obj.Rows[i]["shipmentDocumentScheduleId"], obj.Rows[i]["RefDocCustomGroupId"], obj.Rows[i]["customGroupName"], obj.Rows[i]["chartOfDocumentId"], obj.Rows[i]["documentName"], obj.Rows[i]["original"], obj.Rows[i]["duplicate"], obj.Rows[i]["BaseCriteriaDate"], obj.Rows[i]["beforeDays"], obj.Rows[i]["afterDays"], DueDate, Conversion.CheckDateTimeNull(ReadyDate) ? "" : ReadyDate.ToShortDateString(), obj.Rows[i]["DocStatus"], obj.Rows[i]["prioritySeqNo"], obj.Rows[i]["Remarks"], obj.Rows[i]["DocumentProviderName"], count);
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

	private void FillUnAssignData(int RefDocId, int RefDocumentTypeId, int CustomGroupId)
	{
		try
		{
			dtUnAssigned.Rows.Clear();
			DataTable dtGridRegister = ShipmentDocumentSchedule.GetChartOfDocumentForShipmentScheduleByRefDocument(RefDocId, RefDocumentTypeId, CustomGroupId);
			if (dtGridRegister.Rows.Count > 0)
			{
				for (int i = 0; i < dtGridRegister.Rows.Count; i++)
				{
					dtUnAssigned.Rows.Add(dtGridRegister.Rows[i]["chartOfDocumentId"], dtGridRegister.Rows[i]["documentName"], dtGridRegister.Rows[i]["requiredLevelId"], dtGridRegister.Rows[i]["RequiredLevel"]);
				}
				grdUnAssignDoc.DataSource = dtUnAssigned;
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

	private void grdUnAssignDocSetting()
	{
		try
		{
			grdUnAssignDoc.RootTable.Columns["DocumentId"].Visible = false;
			grdUnAssignDoc.RootTable.Columns["RequiredLevelId"].Visible = false;
			grdUnAssignDoc.RootTable.Columns.Add("Select");
			grdUnAssignDoc.RootTable.Columns["Select"].ActAsSelector = true;
			grdUnAssignDoc.RootTable.Columns["Select"].UseHeaderSelector = true;
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

	private void grdAssignDocSettings()
	{
		try
		{
			grdAssignDoc.RootTable.Columns["Id"].Visible = false;
			grdAssignDoc.RootTable.Columns["DocumentId"].Visible = false;
			grdAssignDoc.RootTable.Columns["CustomGroupId"].Visible = false;
			grdAssignDoc.RootTable.Columns["Id"].EditType = (EditType)0;
			grdAssignDoc.RootTable.Columns["DocumentId"].EditType = (EditType)0;
			grdAssignDoc.RootTable.Columns["Document"].EditType = (EditType)0;
			grdAssignDoc.RootTable.Columns["CustomGroupId"].EditType = (EditType)0;
			grdAssignDoc.RootTable.Columns["CustomGroup"].EditType = (EditType)0;
			grdAssignDoc.RootTable.Columns["Original"].EditType = (EditType)0;
			grdAssignDoc.RootTable.Columns["Duplicate"].EditType = (EditType)0;
			grdAssignDoc.RootTable.Columns["BaseCriteriaDate"].EditType = (EditType)0;
			grdAssignDoc.RootTable.Columns["BeforeDays"].EditType = (EditType)0;
			grdAssignDoc.RootTable.Columns["AfterDays"].EditType = (EditType)0;
			grdAssignDoc.RootTable.Columns["DueDate"].EditType = (EditType)0;
			grdAssignDoc.RootTable.Columns["PrioritySequence"].EditType = (EditType)0;
			grdAssignDoc.RootTable.Columns["AddAttachments"].EditType = (EditType)0;
			grdAssignDoc.RootTable.Columns["AddAttachments"].ColumnType = (ColumnType)5;
			grdAssignDoc.RootTable.Columns["ReadyDate"].EditType = (EditType)2;
			grdAssignDoc.RootTable.Columns["DocStatus"].EditType = (EditType)4;
			grdAssignDoc.RootTable.Columns["Remarks"].EditType = (EditType)1;
			grdAssignDoc.RootTable.Columns["DocumentProviderName"].EditType = (EditType)1;
			grdAssignDoc.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			grdAssignDoc.RootTable.Columns["DocumentId"].FilterEditType = (FilterEditType)1;
			grdAssignDoc.RootTable.Columns["Document"].FilterEditType = (FilterEditType)1;
			grdAssignDoc.RootTable.Columns["CustomGroupId"].FilterEditType = (FilterEditType)1;
			grdAssignDoc.RootTable.Columns["CustomGroup"].FilterEditType = (FilterEditType)1;
			grdAssignDoc.RootTable.Columns["Original"].FilterEditType = (FilterEditType)1;
			grdAssignDoc.RootTable.Columns["Duplicate"].FilterEditType = (FilterEditType)1;
			grdAssignDoc.RootTable.Columns["BaseCriteriaDate"].FilterEditType = (FilterEditType)1;
			grdAssignDoc.RootTable.Columns["BeforeDays"].FilterEditType = (FilterEditType)1;
			grdAssignDoc.RootTable.Columns["AfterDays"].FilterEditType = (FilterEditType)1;
			grdAssignDoc.RootTable.Columns["DueDate"].FilterEditType = (FilterEditType)1;
			grdAssignDoc.RootTable.Columns["PrioritySequence"].FilterEditType = (FilterEditType)1;
			grdAssignDoc.RootTable.Columns["AddAttachments"].FilterEditType = (FilterEditType)1;
			grdAssignDoc.RootTable.Columns["ReadyDate"].FilterEditType = (FilterEditType)1;
			grdAssignDoc.RootTable.Columns["DocStatus"].FilterEditType = (FilterEditType)1;
			grdAssignDoc.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grdAssignDoc.RootTable.Columns["Document"].Width = 150;
			grdAssignDoc.RootTable.Columns["CustomGroup"].Width = 100;
			grdAssignDoc.RootTable.Columns["Original"].Width = 70;
			grdAssignDoc.RootTable.Columns["Duplicate"].Width = 70;
			grdAssignDoc.RootTable.Columns["BaseCriteriaDate"].Width = 90;
			grdAssignDoc.RootTable.Columns["BeforeDays"].Width = 55;
			grdAssignDoc.RootTable.Columns["AfterDays"].Width = 55;
			grdAssignDoc.RootTable.Columns["DueDate"].Width = 90;
			grdAssignDoc.RootTable.Columns["PrioritySequence"].Width = 80;
			grdAssignDoc.RootTable.Columns["AddAttachments"].Width = 90;
			grdAssignDoc.RootTable.Columns["ReadyDate"].Width = 90;
			grdAssignDoc.RootTable.Columns["Remarks"].Width = 160;
			grdAssignDoc.RootTable.Columns["DocumentProviderName"].Width = 120;
			grdAssignDoc.RootTable.Columns["BaseCriteriaDate"].FormatString = "dd-MMM-yyyy";
			grdAssignDoc.RootTable.Columns["DueDate"].FormatString = "dd-MMM-yyyy";
			grdAssignDoc.RootTable.Columns["ReadyDate"].FormatString = "dd-MMM-yyyy";
			grdAssignDoc.RootTable.Columns["DocStatus"].ColumnType = (ColumnType)0;
			grdAssignDoc.RootTable.Columns["DocStatus"].HasValueList = true;
			grdAssignDoc.RootTable.Columns["DocStatus"].LimitToList = true;
			grdAssignDoc.RootTable.Columns["DocStatus"].ValueList.PopulateValueList((IEnumerable)dtStatus.DefaultView, "Id", "type");
			grdAssignDoc.RootTable.Columns["DocStatus"].Width = 100;
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
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				int DocumentId = Conversion.ToInt(r.Cells["DocumentId"].Value);
				if (!dtUnAssigned.AsEnumerable().Any((DataRow row) => row.Field<int>("DocumentId") == DocumentId))
				{
					dtUnAssigned.Rows.Add(Conversion.ToInt(r.Cells["DocumentId"].Value), Conversion.ToString(r.Cells["Document"].Value), Conversion.ToInt(r.Cells["RequiredLevelId"].Value), Conversion.ToString(r.Cells["RequiredLevel"].Value));
					grdUnAssignDoc.DataSource = dtUnAssigned;
					grdUnAssignDocSetting();
				}
				ATd.DeleteAttachments(Id);
				r.Delete();
			}
			grdAssignDoc.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdAssignDoc_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdAssignDoc.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "AddAttachments")
			{
				int chartOfDocumentId = Conversion.ToInt(r.Cells["DocumentId"].Value);
				ATd.ReferecneId = chartOfDocumentId;
				ATd.RefAccountId = Conversion.ToInt(CmbScheduleNo.Value);
				ATd.ShowDialog();
				int count = ATd.lst.Count((TempAttachmentsDetail rrr) => rrr.DetailRefId == chartOfDocumentId);
				DataRow[] array = dtdetailGrid.Select($"DocumentId = '{chartOfDocumentId}'");
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetField("AddAttachments", count);
				}
			}
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

	private void BtnTransferToAssign_Click(object sender, EventArgs e)
	{
		try
		{
			InsertAuto();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void InsertAuto()
	{
		try
		{
			if (Conversion.ToInt(CmbScheduleNo.Value) == 0)
			{
				CmbScheduleNo.Focus();
				throw new Exception("ScheduleNo Field Required...");
			}
			if (grdUnAssignDoc.GetDataRows().Count() == 0)
			{
				throw new Exception("Grid has no Record");
			}
			if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			int RefDocId = Conversion.ToInt(CmbScheduleNo.Value);
			int RefDocumentTypeId = 0;
			if (RefDocId > 0)
			{
				RefDocumentTypeId = Conversion.ToInt(((UltraDropDownBase)CmbScheduleNo).SelectedRow.Cells[2].Value);
			}
			GridEXRow[] checkedRows = grdUnAssignDoc.GetCheckedRows();
			if (checkedRows.Count() == 0)
			{
				throw new Exception("Please select at least one record.");
			}
			List<string> idsList = new List<string>();
			GridEXRow[] array = checkedRows;
			for (int i = 0; i < array.Length; i++)
			{
				string idValue = array[i].Cells["DocumentId"].Value?.ToString();
				if (!string.IsNullOrEmpty(idValue))
				{
					idsList.Add(idValue);
				}
			}
			if (idsList.Count == 0)
			{
				throw new Exception("No valid IDs to Insert.");
			}
			int CustomGroupId = Conversion.ToInt(CmbCustomGroup.Value);
			string Ids = string.Join(",", idsList);
			ShipmentDocumentSchedule.AutoSaveByRefDocumentType(RefDocumentTypeId, RefDocId, CustomGroupId, Ids);
			MessageBox.Show("Save SuccessFully ");
			ShowData(Confirm: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BtnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	private void Insert()
	{
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_06ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Expected O, but got Unknown
		//IL_07af: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b6: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(CmbScheduleNo.Value) == 0)
			{
				CmbScheduleNo.Focus();
				throw new Exception("ScheduleNo Field Required...");
			}
			if (Conversion.ToInt(CmbCustomGroup.Value) == 0)
			{
				CmbCustomGroup.Focus();
				throw new Exception("CustomGroup Field Required...");
			}
			if (grdAssignDoc.RowCount == 0)
			{
				throw new Exception("Grid Record not found...");
			}
			bool Update = false;
			GridEXRow[] rows = grdAssignDoc.GetRows();
			for (int l = 0; l < rows.Length; l++)
			{
				if (Conversion.ToInt(rows[l].Cells["Id"].Value) > 0)
				{
					Update = true;
					break;
				}
			}
			ShipmentDocumentSchedule obj = new ShipmentDocumentSchedule();
			if (Update)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.EntryDate = DateTime.Now;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyDate = DateTime.Now;
			obj.ModifyUserId = UserAccount.ID;
			obj.ShipmentDocumentScheduleList = new List<ShipmentDocumentSchedule>();
			obj.DetailAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			if (LstRemoveRecord.Count > 0)
			{
				for (int j = 0; j < LstRemoveRecord.Count; j++)
				{
					obj.ShipmentDocumentScheduleList.Add(LstRemoveRecord[j]);
				}
			}
			rows = grdAssignDoc.GetRows();
			foreach (GridEXRow r in rows)
			{
				ShipmentDocumentSchedule exIm = new ShipmentDocumentSchedule();
				if (Conversion.ToInt(r.Cells["DocumentId"].Value) == 0)
				{
					MessageBox.Show("Chart Of Document Required in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				if (Conversion.ToInt(r.Cells["Original"].Value) == 0)
				{
					MessageBox.Show("Original Required in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				if (Conversion.ToInt(r.Cells["Duplicate"].Value) == 0)
				{
					MessageBox.Show("Duplicate Required in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
				if (Conversion.ToInt(r.Cells["DocStatus"].Value) == 0)
				{
					MessageBox.Show("DocStatus Required in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				exIm.refDocumentId = Conversion.ToInt(CmbScheduleNo.Value);
				exIm.referenceDocumentTypeId = Conversion.ToInt(((UltraDropDownBase)CmbScheduleNo).SelectedRow.Cells[2].Value);
				exIm.RefDocCustomGroupId = Conversion.ToInt(CmbCustomGroup.Value);
				exIm.RefDocLockStatus = ChkIsLockSchedule.Checked;
				exIm.shipmentDocumentScheduleId = Conversion.ToInt(r.Cells["Id"].Value);
				exIm.chartOfDocumentId = Conversion.ToInt(r.Cells["DocumentId"].Value);
				exIm.original = Conversion.ToInt(r.Cells["Original"].Value);
				exIm.duplicate = Conversion.ToInt(r.Cells["Duplicate"].Value);
				exIm.BaseCriteriaDate = Conversion.ToDateTime(r.Cells["BaseCriteriaDate"].Value);
				exIm.beforeDays = Conversion.ToInt(r.Cells["BeforeDays"].Value);
				exIm.afterDays = Conversion.ToInt(r.Cells["AfterDays"].Value);
				exIm.BaseCriteriaDate = Conversion.ToDateTime(r.Cells["BaseCriteriaDate"].Value);
				exIm.DueDate = Conversion.ToDateTime(r.Cells["DueDate"].Value);
				exIm.ReadyDate = Conversion.ToDateTime(r.Cells["ReadyDate"].Value);
				exIm.DocStatus = Conversion.ToInt(r.Cells["DocStatus"].Value);
				exIm.prioritySeqNo = Conversion.ToInt(r.Cells["PrioritySequence"].Value);
				exIm.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
				exIm.documentPath = "";
				exIm.ApprovedDate = DateTime.Now;
				exIm.EntryDate = DateTime.Now;
				exIm.EntryUserId = UserAccount.ID;
				exIm.ApprovedUserId = UserAccount.ID;
				exIm.ModifyDate = DateTime.Now;
				exIm.ModifyUserId = UserAccount.ID;
				exIm.OrganizationId = UserAccount.OrganizationId;
				exIm.CompanyId = UserAccount.CompanyId;
				obj.ShipmentDocumentScheduleList.Add(exIm);
			}
			for (int i = 0; i < ATd.lst.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = ATd.lst[i].Path;
				attach.UploadedFileCustomName = ATd.lst[i].CustomPath;
				attach.RefAccountId = ATd.lst[i].RefAccountId;
				attach.LineId = ATd.lst[i].DetailRefId;
				attach.DetailWiseAttachment = true;
				attach.ScreenName = base.Name;
				obj.DetailAttachmentsList.Add(attach);
			}
			if (ATd.RemovedAttachmentListInUpdateCase.Count > 0)
			{
				obj.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				for (int k = 0; k < ATd.RemovedAttachmentListInUpdateCase.Count; k++)
				{
					DMSAttachments delete = new DMSAttachments();
					delete.Attachment = Path.GetFileName(ATd.RemovedAttachmentListInUpdateCase[k].Path);
					delete.ScreenName = base.Name;
					obj.DeleteAttachmentsList.Add(delete);
				}
			}
			ShipmentDocumentSchedule.Save(obj);
			if (!Update)
			{
				MessageBox.Show("Save SuccessFully ");
			}
			else
			{
				MessageBox.Show("Update SuccessFully");
				if (ATd.RemovedAttachmentListInUpdateCase.Count > 0 && ATd.lst.Count == 0)
				{
					global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(obj.ShipmentDocumentScheduleList[0].refDocumentId, "", obj.ShipmentDocumentScheduleList[0].referenceDocumentTypeId, 0);
				}
			}
			ShowData(Confirm: false);
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

	private void BtnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbScheduleNo.Value) == 0)
			{
				CmbScheduleNo.Focus();
				throw new Exception("ScheduleNo Field Required...");
			}
			if (Conversion.ToInt(CmbCustomGroup.Value) == 0)
			{
				CmbCustomGroup.Focus();
				throw new Exception("CustomGroup Field Required...");
			}
			if (grdAssignDoc.GetDataRows().Count() == 0)
			{
				throw new Exception("Grid has no records.");
			}
			GridEXRow[] checkedRows = grdAssignDoc.GetCheckedRows();
			if (checkedRows.Count() == 0)
			{
				throw new Exception("Please select at least one record.");
			}
			if (grdAssignDoc.GetCheckedRows().AsEnumerable().Any((GridEXRow r) => Conversion.ToInt(r.Cells["AddAttachments"].Value) > 0) && MessageBox.Show("Attachments will be deleted along with the selected rows. Are you sure you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			List<string> idsList = (from r in checkedRows
				select r.Cells["Id"].Value?.ToString() into id
				where !string.IsNullOrEmpty(id)
				select id).ToList();
			List<string> SelectedCODId = (from r in checkedRows
				select r.Cells["DocumentId"].Value?.ToString() into id
				where !string.IsNullOrEmpty(id)
				select id).ToList();
			if (idsList.Count == 0)
			{
				throw new Exception("No valid IDs to delete.");
			}
			List<TempAttachmentsDetail> list = ATd.lst.Where((TempAttachmentsDetail rrr) => SelectedCODId.Contains(rrr.DetailRefId.ToString())).ToList();
			string obj = string.Join(",", idsList);
			string SelectedCODIds = string.Join(",", SelectedCODId);
			ShipmentDocumentSchedule.DeleteByIds(obj, SelectedCODIds, Conversion.ToInt(((UltraDropDownBase)CmbScheduleNo).SelectedRow.Cells[2].Value), UserAccount.EntryUserId);
			MessageBox.Show("Delete Successfully");
			foreach (TempAttachmentsDetail attachment in list)
			{
				bool fileDeleted = false;
				int attempts = 0;
				string path = clsGlobalVariables.AttachmentFolderPath + attachment.CustomPath;
				FileInfo info = new FileInfo(path);
				if (info.Exists)
				{
					while (!fileDeleted && attempts < 5)
					{
						try
						{
							if (info.IsReadOnly)
							{
								info.IsReadOnly = false;
							}
							File.Delete(path);
							fileDeleted = true;
						}
						catch (IOException)
						{
							attempts++;
							Thread.Sleep(500);
						}
						catch (UnauthorizedAccessException)
						{
							MessageBox.Show("Access denied to file: " + attachment.Path + ".", "Permission Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
							fileDeleted = true;
						}
					}
				}
				else
				{
					MessageBox.Show("File not found: " + attachment.Path, "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			ShowData(Confirm: false);
		}
		catch (Exception ex3)
		{
			MessageBox.Show(ex3.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			obj.DocumentTypeId = 244;
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
			DataTable dtGridRegister = ShipmentDocumentSchedule.FormHistory(obj);
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
				CmbScheduleNo.Value = customGroupId;
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

	private void frmShipmentDocumentSchedule_KeyDown(object sender, KeyEventArgs e)
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
				if (formright.DoHaveUpdateRights && tabControl1.SelectedIndex == 0)
				{
					BtnUpdate_Click(null, null);
				}
				if (tabControl1.SelectedIndex == 1)
				{
					btnShow_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.U && formright.DoHaveUpdateRights && tabControl1.SelectedIndex == 0)
			{
				BtnUpdate_Click(null, null);
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
					CmbScheduleNo.Value = customGroupId;
					ShowData(Confirm: false);
				}
				if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					DataGridHistory_LinkClicked(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				CmbScheduleNo.Value = customGroupId;
				ShowData(Confirm: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnDefineCustomGroup_Click(object sender, EventArgs e)
	{
		try
		{
			new frmDocumentOfGroup(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void RadReferedRefDoc_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			ScheduleNoComboBind(ScheduleNoComboDbCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		try
		{
			UltraCombo cmbScheduleNo = CmbScheduleNo;
			UltraGridRow row = ((cmbScheduleNo != null) ? ((UltraDropDownBase)cmbScheduleNo).SelectedRow : null);
			if (row == null)
			{
				CmbScheduleNo.Focus();
				MessageBox.Show("Please select a Schedule / Invoice No first.");
				return;
			}
			CellsCollection cells = row.Cells;
			int invoiceId = Conversion.ToInt(cells["InvoiceId"].Value);
			int invoiceDocTypeId = Conversion.ToInt(cells["InvoiceDocumentTypeId"].Value);
			int scheduleDocTypeId = ((cells["DocumentTypeId"].Value != null) ? Conversion.ToInt(cells["DocumentTypeId"].Value) : 0);
			int exImLcOrderId = ((cells["Id"].Value != null) ? Conversion.ToInt(cells["Id"].Value) : 0);
			bool isForInvoiced = cells["IsForInvoiced"].Value != null && Conversion.ToBool(cells["IsForInvoiced"].Value);
			int id = (isForInvoiced ? invoiceId : exImLcOrderId);
			int documentTypeId = (isForInvoiced ? invoiceDocTypeId : scheduleDocTypeId);
			btnAttachment.Text = (isForInvoiced ? "Invoice Attachments" : "Schedule Attachments");
			GetAndDeleteAttachmentsForRecord(id, documentTypeId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void GetAndDeleteAttachmentsForRecord(int Id, int DocumentTypeId)
	{
		try
		{
			CommonServices.GetSaveAndDeleteAttachmentsForRecord(UserAccount, Id, DocumentTypeId, "", ShowSavebutton: true);
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
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Expected O, but got Unknown
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Expected O, but got Unknown
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Expected O, but got Unknown
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Expected O, but got Unknown
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Expected O, but got Unknown
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Expected O, but got Unknown
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Expected O, but got Unknown
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Expected O, but got Unknown
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Expected O, but got Unknown
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Expected O, but got Unknown
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Expected O, but got Unknown
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Expected O, but got Unknown
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Expected O, but got Unknown
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Expected O, but got Unknown
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Expected O, but got Unknown
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Expected O, but got Unknown
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Expected O, but got Unknown
		//IL_0536: Unknown result type (might be due to invalid IL or missing references)
		//IL_0540: Expected O, but got Unknown
		//IL_058e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0598: Expected O, but got Unknown
		//IL_0599: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a3: Expected O, but got Unknown
		//IL_0aac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab6: Expected O, but got Unknown
		//IL_0ac3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0acd: Expected O, but got Unknown
		//IL_0ada: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae4: Expected O, but got Unknown
		//IL_0af1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afb: Expected O, but got Unknown
		//IL_26b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_26bd: Expected O, but got Unknown
		//IL_26ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_26d4: Expected O, but got Unknown
		GridEXLayout grdAssignDoc_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.SDT.frmShipmentDocumentSchedule));
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
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
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
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.grdAssignDoc = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnDelete = new System.Windows.Forms.ToolStripButton();
		this.label14 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdUnAssignDoc = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label13 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.BtnSave = new System.Windows.Forms.ToolStripButton();
		this.panel6 = new System.Windows.Forms.Panel();
		this.RadReferedNotRefDoc = new System.Windows.Forms.RadioButton();
		this.RadReferedRefDoc = new System.Windows.Forms.RadioButton();
		this.ChkIsLockSchedule = new System.Windows.Forms.CheckBox();
		this.btnShowByCustomGroup = new System.Windows.Forms.Button();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label12 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshUnAssigned = new System.Windows.Forms.ToolStripButton();
		this.BtnUpdate = new System.Windows.Forms.ToolStripButton();
		this.BtnDefineCustomGroup = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.CmbCustomGroup = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbScheduleNo = new UltraCombo();
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
		this.label67 = new System.Windows.Forms.Label();
		this.CmbCustomerContract = new UltraCombo();
		this.CmbSalesPersonContract = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdAssignDoc).BeginInit();
		this.panel13.SuspendLayout();
		this.panel1.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdUnAssignDoc).BeginInit();
		this.panel4.SuspendLayout();
		this.panel7.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbScheduleNo).BeginInit();
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
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerContract).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSalesPersonContract).BeginInit();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(3, 3);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1170, 526);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1162, 497);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel12);
		this.panel2.Controls.Add(this.panel10);
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1156, 491);
		this.panel2.TabIndex = 0;
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.grdAssignDoc);
		this.panel12.Controls.Add(this.panel13);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel12.Location = new System.Drawing.Point(301, 108);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(855, 383);
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
		((System.Windows.Forms.Control)(object)this.grdAssignDoc).Size = new System.Drawing.Size(855, 352);
		((System.Windows.Forms.Control)(object)this.grdAssignDoc).TabIndex = 0;
		this.grdAssignDoc.TotalRow = (InheritableBoolean)1;
		this.grdAssignDoc.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdAssignDoc).VisualStyle = (VisualStyle)4;
		this.grdAssignDoc.CellUpdated += new ColumnActionEventHandler(grdAssignDoc_CellUpdated);
		this.grdAssignDoc.UpdatingCell += new UpdatingCellEventHandler(grdAssignDoc_UpdatingCell);
		this.grdAssignDoc.ColumnButtonClick += new ColumnActionEventHandler(grdAssignDoc_ColumnButtonClick);
		this.grdAssignDoc.LinkClicked += new ColumnActionEventHandler(grdAssignDoc_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdAssignDoc).Click += new System.EventHandler(grdAssignDoc_Click);
		((System.Windows.Forms.Control)(object)this.grdAssignDoc).KeyDown += new System.Windows.Forms.KeyEventHandler(grdAssignDoc_KeyDown);
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.panel1);
		this.panel13.Controls.Add(this.label14);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(855, 31);
		this.panel13.TabIndex = 6;
		this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.panel1.Controls.Add(this.toolStrip2);
		this.panel1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel1.Location = new System.Drawing.Point(817, 1);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(33, 27);
		this.panel1.TabIndex = 55;
		this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.BtnDelete });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(33, 27);
		this.toolStrip2.TabIndex = 7;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.BtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnDelete.Name = "BtnDelete";
		this.BtnDelete.Size = new System.Drawing.Size(24, 24);
		this.BtnDelete.ToolTipText = "Click to UnAllocate selected rows";
		this.BtnDelete.Click += new System.EventHandler(BtnDelete_Click);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(3, 7);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(146, 17);
		this.label14.TabIndex = 54;
		this.label14.Text = "Allocated Document";
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdUnAssignDoc);
		this.panel10.Controls.Add(this.panel4);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel10.Location = new System.Drawing.Point(0, 108);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(301, 383);
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
		((System.Windows.Forms.Control)(object)this.grdUnAssignDoc).Size = new System.Drawing.Size(301, 352);
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
		this.panel4.Size = new System.Drawing.Size(301, 31);
		this.panel4.TabIndex = 6;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(3, 7);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(171, 17);
		this.label13.TabIndex = 54;
		this.label13.Text = "Un-Allocated Document";
		this.panel7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.panel7.Controls.Add(this.toolStrip3);
		this.panel7.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel7.Location = new System.Drawing.Point(267, 2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(32, 27);
		this.panel7.TabIndex = 1;
		this.toolStrip3.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.BtnSave });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(32, 27);
		this.toolStrip3.TabIndex = 7;
		this.toolStrip3.Text = "toolStrip3";
		this.BtnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnSave.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.BtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSave.Name = "BtnSave";
		this.BtnSave.Size = new System.Drawing.Size(24, 24);
		this.BtnSave.ToolTipText = "Click to Allocate selected rows";
		this.BtnSave.Click += new System.EventHandler(BtnTransferToAssign_Click);
		this.panel6.BackColor = System.Drawing.Color.White;
		this.panel6.Controls.Add(this.label4);
		this.panel6.Controls.Add(this.label67);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSalesPersonContract);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerContract);
		this.panel6.Controls.Add(this.RadReferedNotRefDoc);
		this.panel6.Controls.Add(this.RadReferedRefDoc);
		this.panel6.Controls.Add(this.ChkIsLockSchedule);
		this.panel6.Controls.Add(this.btnShowByCustomGroup);
		this.panel6.Controls.Add(this.panel3);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomGroup);
		this.panel6.Controls.Add(this.label3);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbScheduleNo);
		this.panel6.Controls.Add(this.label2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1156, 108);
		this.panel6.TabIndex = 0;
		this.RadReferedNotRefDoc.AutoSize = true;
		this.RadReferedNotRefDoc.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadReferedNotRefDoc.Location = new System.Drawing.Point(687, 58);
		this.RadReferedNotRefDoc.Name = "RadReferedNotRefDoc";
		this.RadReferedNotRefDoc.Size = new System.Drawing.Size(105, 17);
		this.RadReferedNotRefDoc.TabIndex = 63;
		this.RadReferedNotRefDoc.Text = "Not Scheduled";
		this.RadReferedNotRefDoc.UseVisualStyleBackColor = true;
		this.RadReferedRefDoc.AutoSize = true;
		this.RadReferedRefDoc.Checked = true;
		this.RadReferedRefDoc.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadReferedRefDoc.Location = new System.Drawing.Point(608, 58);
		this.RadReferedRefDoc.Name = "RadReferedRefDoc";
		this.RadReferedRefDoc.Size = new System.Drawing.Size(83, 17);
		this.RadReferedRefDoc.TabIndex = 63;
		this.RadReferedRefDoc.TabStop = true;
		this.RadReferedRefDoc.Text = "Scheduled";
		this.RadReferedRefDoc.UseVisualStyleBackColor = true;
		this.RadReferedRefDoc.CheckedChanged += new System.EventHandler(RadReferedRefDoc_CheckedChanged);
		this.ChkIsLockSchedule.AutoSize = true;
		this.ChkIsLockSchedule.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkIsLockSchedule.Location = new System.Drawing.Point(1039, 56);
		this.ChkIsLockSchedule.Name = "ChkIsLockSchedule";
		this.ChkIsLockSchedule.Size = new System.Drawing.Size(112, 21);
		this.ChkIsLockSchedule.TabIndex = 4;
		this.ChkIsLockSchedule.Text = "Lock Schedule";
		this.ChkIsLockSchedule.UseVisualStyleBackColor = true;
		this.btnShowByCustomGroup.BackColor = System.Drawing.Color.Teal;
		this.btnShowByCustomGroup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowByCustomGroup.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowByCustomGroup.ForeColor = System.Drawing.Color.White;
		this.btnShowByCustomGroup.Location = new System.Drawing.Point(1039, 77);
		this.btnShowByCustomGroup.Name = "btnShowByCustomGroup";
		this.btnShowByCustomGroup.Size = new System.Drawing.Size(59, 26);
		this.btnShowByCustomGroup.TabIndex = 5;
		this.btnShowByCustomGroup.Text = "Show";
		this.btnShowByCustomGroup.UseVisualStyleBackColor = false;
		this.btnShowByCustomGroup.Click += new System.EventHandler(btnShowByCustomGroup_Click);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label12);
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1156, 55);
		this.panel3.TabIndex = 0;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(2, 31);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(251, 19);
		this.label12.TabIndex = 54;
		this.label12.Text = "Shipment Document Schedule";
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.btnnew, this.BtnRefreshUnAssigned, this.BtnUpdate, this.BtnDefineCustomGroup, this.BtnShortCutkeys, this.btnAttachment });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1156, 27);
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
		this.BtnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.BtnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnUpdate.Name = "BtnUpdate";
		this.BtnUpdate.Size = new System.Drawing.Size(152, 24);
		this.BtnUpdate.Text = "&UpdateLockStatus";
		this.BtnUpdate.ToolTipText = "Update checked Rows From Allocated Grid";
		this.BtnUpdate.Click += new System.EventHandler(BtnUpdate_Click);
		this.BtnDefineCustomGroup.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnDefineCustomGroup.Image = Architecture.WinApp.Properties.Resources.Account_Reports;
		this.BtnDefineCustomGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnDefineCustomGroup.Name = "BtnDefineCustomGroup";
		this.BtnDefineCustomGroup.Size = new System.Drawing.Size(166, 24);
		this.BtnDefineCustomGroup.Text = "&Document_Of_Group";
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
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Location = new System.Drawing.Point(806, 77);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Name = "CmbCustomGroup";
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Size = new System.Drawing.Size(231, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).TabIndex = 3;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(806, 59);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(84, 15);
		this.label3.TabIndex = 60;
		this.label3.Text = "Custom Group";
		this.CmbScheduleNo.AllowNull = (DefaultableBoolean)1;
		this.CmbScheduleNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbScheduleNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbScheduleNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbScheduleNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance59;
		((SpecialBoxBase)((UltraGridBase)this.CmbScheduleNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance92).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance66).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbScheduleNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbScheduleNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbScheduleNo).Location = new System.Drawing.Point(457, 77);
		((System.Windows.Forms.Control)(object)this.CmbScheduleNo).Name = "CmbScheduleNo";
		((System.Windows.Forms.Control)(object)this.CmbScheduleNo).Size = new System.Drawing.Size(346, 26);
		((System.Windows.Forms.Control)(object)this.CmbScheduleNo).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbScheduleNo).Leave += new System.EventHandler(CmbScheduleNo_Leave);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(458, 59);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(125, 15);
		this.label2.TabIndex = 60;
		this.label2.Text = "Schedule / Invoice No";
		this.tabPage3.Controls.Add(this.panel26);
		this.tabPage3.Controls.Add(this.panel25);
		this.tabPage3.Controls.Add(this.toolStrip4);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1162, 497);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "History";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel26.Controls.Add(this.panel27);
		this.panel26.Controls.Add(this.panel24);
		this.panel26.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel26.Location = new System.Drawing.Point(0, 105);
		this.panel26.Name = "panel26";
		this.panel26.Size = new System.Drawing.Size(1162, 392);
		this.panel26.TabIndex = 55;
		this.panel27.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel27.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel27.Location = new System.Drawing.Point(0, 27);
		this.panel27.Name = "panel27";
		this.panel27.Size = new System.Drawing.Size(1162, 365);
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1162, 365);
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
		this.panel24.Size = new System.Drawing.Size(1162, 27);
		this.panel24.TabIndex = 52;
		this.ctrlGrdBar7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar7.FormName = this;
		this.ctrlGrdBar7.Location = new System.Drawing.Point(1123, 3);
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
		this.panel25.Size = new System.Drawing.Size(1162, 78);
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
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance90).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance96).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDocumentProviderHis).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance96;
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
		this.toolStrip4.Size = new System.Drawing.Size(1162, 27);
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
		this.tabControlMain.Size = new System.Drawing.Size(1184, 561);
		this.tabControlMain.TabIndex = 1;
		this.tabPageMain.Controls.Add(this.tabControl1);
		this.tabPageMain.Location = new System.Drawing.Point(4, 25);
		this.tabPageMain.Name = "tabPageMain";
		this.tabPageMain.Padding = new System.Windows.Forms.Padding(3);
		this.tabPageMain.Size = new System.Drawing.Size(1176, 532);
		this.tabPageMain.TabIndex = 0;
		this.tabPageMain.Text = "Main Form";
		this.tabPageMain.UseVisualStyleBackColor = true;
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label67.Location = new System.Drawing.Point(9, 59);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(58, 15);
		this.label67.TabIndex = 89;
		this.label67.Text = "Customer";
		this.CmbCustomerContract.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerContract.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerContract.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerContract.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerContract).DisplayLayout.Appearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbCustomerContract).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerContract).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerContract).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerContract).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance80;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerContract).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerContract).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbCustomerContract).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerContract).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerContract).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerContract).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbCustomerContract).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerContract).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerContract).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance85).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerContract).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbCustomerContract).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerContract).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance86).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerContract).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerContract).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbCustomerContract).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerContract).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerContract).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbCustomerContract).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerContract).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbCustomerContract).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerContract).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerContract).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerContract).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerContract.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerContract).Location = new System.Drawing.Point(3, 77);
		((System.Windows.Forms.Control)(object)this.CmbCustomerContract).Name = "CmbCustomerContract";
		((System.Windows.Forms.Control)(object)this.CmbCustomerContract).Size = new System.Drawing.Size(225, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerContract).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbCustomerContract).Leave += new System.EventHandler(CmbCustomerContract_Leave);
		this.CmbSalesPersonContract.AllowNull = (DefaultableBoolean)1;
		this.CmbSalesPersonContract.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSalesPersonContract.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSalesPersonContract.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSalesPersonContract).DisplayLayout.Appearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbSalesPersonContract).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSalesPersonContract).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSalesPersonContract).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSalesPersonContract).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance65;
		((SpecialBoxBase)((UltraGridBase)this.CmbSalesPersonContract).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSalesPersonContract).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbSalesPersonContract).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSalesPersonContract).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSalesPersonContract).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSalesPersonContract).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbSalesPersonContract).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSalesPersonContract).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSalesPersonContract).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance74).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSalesPersonContract).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbSalesPersonContract).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSalesPersonContract).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance75).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSalesPersonContract).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSalesPersonContract).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbSalesPersonContract).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSalesPersonContract).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSalesPersonContract).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbSalesPersonContract).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSalesPersonContract).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbSalesPersonContract).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSalesPersonContract).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSalesPersonContract).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSalesPersonContract).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSalesPersonContract.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSalesPersonContract).Location = new System.Drawing.Point(230, 77);
		((System.Windows.Forms.Control)(object)this.CmbSalesPersonContract).Name = "CmbSalesPersonContract";
		((System.Windows.Forms.Control)(object)this.CmbSalesPersonContract).Size = new System.Drawing.Size(225, 26);
		((System.Windows.Forms.Control)(object)this.CmbSalesPersonContract).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbSalesPersonContract).Leave += new System.EventHandler(CmbCustomerContract_Leave);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(236, 59);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(73, 15);
		this.label4.TabIndex = 89;
		this.label4.Text = "Sales Person";
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(181, 24);
		this.btnAttachment.Text = "Schedule Attachments";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1184, 561);
		base.Controls.Add(this.tabControlMain);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmShipmentDocumentSchedule";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmShipmentDocumentSchedule_FormClosed);
		base.Load += new System.EventHandler(frmShipmentDocumentSchedule_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmShipmentDocumentSchedule_KeyDown);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdAssignDoc).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
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
		((System.ComponentModel.ISupportInitialize)this.CmbScheduleNo).EndInit();
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
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerContract).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSalesPersonContract).EndInit();
		base.ResumeLayout(false);
	}
}

}
