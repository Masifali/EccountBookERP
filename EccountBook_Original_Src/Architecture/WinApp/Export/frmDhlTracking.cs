using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.Export;

public class frmDhlTracking : Form
{
	private long RecId;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private DataTable dtHistory = new DataTable();

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects FormRights = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private Panel panel3;

	private Panel panel4;

	internal GridEX grdfrm;

	private Label label1;

	private TextBox txtDetail;

	private Label label2;

	internal UltraCombo CmbCurrentStatus;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private Panel panel6;

	private Label label39;

	private ToolStripButton btnShortcukeys;

	private Label label3;

	private DateTimePicker datDispatched;

	private Label label51;

	private DateTimePicker datFinalETA;

	private Label label5;

	private Label label4;

	private GroupBox groupBox1;

	private Panel panel5;

	private DateTimePicker datFinalEtaFromHistory;

	private Label label8;

	private Label label7;

	private DateTimePicker datDispatchedFromHistory;

	private Label label6;

	private TextBox txtAwbBillNo;

	private Label label9;

	internal UltraCombo CmbCurrentStatusHistory;

	private Button btnShow;

	private DateTimePicker datDispatchedToHistory;

	private Label label10;

	private DateTimePicker datFinalEtaToHistory;

	private Label label11;

	private ToolStripButton btnAttachment;

	internal UltraCombo txtAwbBillNoHistory;

	private ToolStripButton btnRegister;

	public frmDhlTracking(UserAccount user)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void DateLock_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(500, 400);
			FormRights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = FormRights.DoHaveSaveRight;
			btnUpdate.Enabled = FormRights.DoHaveUpdateRights;
			btnSave.Text = "Save";
			btnUpdate.Visible = false;
			datDispatched.Focus();
			CurrentStatusFill();
			ComboBindHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CurrentStatusFill()
	{
		try
		{
			int id = Conversion.ToInt(CmbCurrentStatus.Value);
			DataTable dt = CommonServices.GetExportLookUpsDataByTypeIdDt(6);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbCurrentStatus, "Id", "LookUpName", "Current Status", true);
				((UltraGridBase)CmbCurrentStatus).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (id > 0)
			{
				CommonServices.SetComboValue(CmbCurrentStatus, "Id", id, dt);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboBindHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = DHLTracking.GetDataForDropDown(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt == null || dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtCS = new DataTable();
			dtCS.Columns.Add("Id", typeof(int));
			dtCS.Columns.Add("name", typeof(string));
			DataTable dtAWBNo = dtCS.Clone();
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToString(dt.Rows[i]["Activity"]) == "AWBBillNumber")
				{
					dtAWBNo.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "CurrentStatus")
				{
					dtCS.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
			}
			DDL.BindDDL(dtAWBNo, txtAwbBillNoHistory, "Id", "name", "AWB Bill No", ZeroIndex: false);
			DDL.BindDDL(dtCS, CmbCurrentStatusHistory, "Id", "name", "current Status", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbLookupsType_Leave(object sender, EventArgs e)
	{
		try
		{
			BindGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtAwbBillNo.Text.Trim() == string.Empty)
		{
			MessageBox.Show("AWB Bill No Field is Required");
			txtAwbBillNo.Focus();
			return false;
		}
		if (txtDetail.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Detail/Description Field is Required");
			txtDetail.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCurrentStatus).ActiveRow == null || Conversion.ToInt(CmbCurrentStatus.Value) == 0)
		{
			MessageBox.Show("Current Status Field is Required");
			CmbCurrentStatus.Focus();
			return false;
		}
		if (datFinalETA.Value < datDispatched.Value)
		{
			MessageBox.Show("ETA date Can't be less than Dispatched date");
			datFinalETA.Focus();
			return false;
		}
		return true;
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			DHLTracking obj = new DHLTracking();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				obj.DHLTrackingId = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.EntryDate = DateTime.Now;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyDate = DateTime.Now;
			obj.ModifyUserId = UserAccount.ID;
			obj.ApprovedDate = DateTime.Now;
			obj.ApprovedUserId = UserAccount.ID;
			obj.DateOfDispatched = datDispatched.Value;
			obj.AWBBillNumber = txtAwbBillNo.Text;
			obj.FinalETADate = datFinalETA.Value;
			obj.Detail = txtDetail.Text.Trim();
			obj.CurrentStatusId = Conversion.ToInt(CmbCurrentStatus.Value);
			obj.DocumentTypeId = 248;
			obj.AttachmentsList = new List<DMSAttachments>();
			obj.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<DHLTracking>(obj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			obj.AttachmentsValues = AttachmentsValues;
			obj.CustomAttachmentsValues = CustomAttachmentsValues;
			DHLTracking.Save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update Successfully");
				FormHelper.DeleteAllAttachmentsFromDb(AT, Conversion.ToInt((object)RecId), obj.DocumentTypeId, base.Name);
			}
			else
			{
				MessageBox.Show("Record Save Successfully");
			}
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0L;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGrid()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = 248;
			if (datDispatchedFromHistory.Checked)
			{
				obj.DisPatchedDate = datDispatchedFromHistory.Value;
			}
			if (datDispatchedToHistory.Checked)
			{
				obj.DisPatchedToDate = datDispatchedToHistory.Value;
			}
			if (datFinalEtaFromHistory.Checked)
			{
				obj.ETADate = datFinalEtaFromHistory.Value;
			}
			if (datFinalEtaToHistory.Checked)
			{
				obj.ETAToDate = datFinalEtaToHistory.Value;
			}
			obj.AWBBillNo = ((Control)(object)txtAwbBillNoHistory).Text.Trim();
			obj.StatusId = Conversion.ToInt(CmbCurrentStatusHistory.Value);
			obj.CanViewAllRecord = true;
			DataSet ds = DHLTracking.FormHistory(obj);
			dtHistory = ds?.Tables[0].Copy();
			if (dtHistory.Rows.Count > 0)
			{
				DatatableHelper.ReplaceCompLogoColumn(dtHistory, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
				DataTable dt = new DataTable();
				dt.Columns.Add("DHLTrackingId", typeof(long));
				dt.Columns.Add("DateOfDispatched", typeof(DateTime));
				dt.Columns.Add("AWBBillNo", typeof(string));
				dt.Columns.Add("FinalETA", typeof(DateTime));
				dt.Columns.Add("CurrentStatus", typeof(string));
				dt.Columns.Add("Detail", typeof(string));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					dt.Rows.Add(dtHistory.Rows[i]["DHLTrackingId"], dtHistory.Rows[i]["DateOfDispatched"], dtHistory.Rows[i]["AWBBillNumber"], dtHistory.Rows[i]["FinalETADate"], dtHistory.Rows[i]["CurrentStatus"], dtHistory.Rows[i]["Detail"], dtHistory.Rows[i]["EntryUser"], dtHistory.Rows[i]["EntryDate"], dtHistory.Rows[i]["ModifyUser"], dtHistory.Rows[i]["ModifyDate"], dtHistory.Rows[i]["NoOfAttachments"]);
				}
				grdfrm.DataSource = dt;
				grdfrm.RetrieveStructure();
				GridSetting();
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
			grdfrm.ColumnAutoResize = false;
			grdfrm.RootTable.Columns["DHLTrackingId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdfrm, 2, 3);
			grdfrm.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdfrm.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdfrm.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdfrm.RootTable.Columns["DateOfDispatched"].Width = Constants.ExportConstants.DateConstant + 10;
			grdfrm.RootTable.Columns["AWBBillNo"].Width = 120;
			grdfrm.RootTable.Columns["FinalETA"].Width = Constants.ExportConstants.DateConstant + 10;
			grdfrm.RootTable.Columns["CurrentStatus"].Width = Constants.ExportConstants.StatusConstant + 70;
			grdfrm.RootTable.Columns["Detail"].Width = Constants.ExportConstants.RemarksConstants + 70;
			grdfrm.RootTable.Columns["EntryUser"].Width = Constants.ExportConstants.EntryUserConstants + 30;
			grdfrm.RootTable.Columns["EntryDate"].Width = Constants.ExportConstants.DateTimeConstantddMMM + 10;
			grdfrm.RootTable.Columns["ModifyUser"].Width = Constants.ExportConstants.EntryUserConstants;
			grdfrm.RootTable.Columns["ModifyDate"].Width = Constants.ExportConstants.DateTimeConstantddMMM + 10;
			grdfrm.RootTable.Columns["NoOfAttachments"].Width = Constants.ExportConstants.NoOfAttachmentsConstants;
			GridEX_Helper.AddButton(grdfrm, "Add-Attachment", "Add-Attachment", 110);
			GridEX_Helper.AddButton(grdfrm, "Delete", "X", 20);
			((Control)(object)grdfrm).Refresh();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_DoubleClick(object sender, EventArgs e)
	{
		RecId = Conversion.ToInt(grdfrm.CurrentRow.Cells["DHLTrackingId"].Value);
		DHLTracking lst = DHLTracking.GetByID(RecId);
		if (lst != null)
		{
			datDispatched.Value = lst.DateOfDispatched;
			txtAwbBillNo.Text = lst.AWBBillNumber;
			datFinalETA.Value = lst.FinalETADate;
			txtDetail.Text = lst.Detail;
			CmbCurrentStatus.Value = lst.CurrentStatusId;
			AttachmentsValues = lst.AttachmentsValues;
			CustomAttachmentsValues = lst.CustomAttachmentsValues;
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			FormHelper.LoadAttachmentsForObject(AT, Conversion.ToInt((object)RecId), base.Name);
		}
	}

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Clear();
			RecId = 0L;
			txtDetail.Text = string.Empty;
			txtAwbBillNo.Text = string.Empty;
			btnSave.Text = "Save";
			btnUpdate.Visible = false;
			btnSave.Visible = true;
			CmbCurrentStatus.Focus();
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
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			CurrentStatusFill();
			ComboBindHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExImLookups_FormClosed(object sender, FormClosedEventArgs e)
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

	private void ExImLookups_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (btnSave.Visible && btnSave.Enabled && e.Control && e.KeyCode == Keys.S)
			{
				btnSave_Click(null, null);
			}
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnUpdate_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnRegister_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				btnAttachment_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdfrm).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				datDispatched.Focus();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				datDispatched.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Return && ((Control)(object)grdfrm).Focused)
			{
				grdfrm_DoubleClick(null, null);
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

	private void btnShortcukeys_Click(object sender, EventArgs e)
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
			dt.Rows.Add("Ctrl+F5", "For Focus on LookUp");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Lookup");
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

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			BindGrid();
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
			AT.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdfrm.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			long Id = Conversion.ToInt64(r.Cells["DHLTrackingId"].Value);
			if (e.Column.Key == "Add-Attachment")
			{
				AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, Conversion.ToInt((object)Id), 248, base.Name);
				attachmentAddingFromHistory.BringToFront();
				attachmentAddingFromHistory.ShowDialog();
			}
			if (e.Column.Key == "Delete")
			{
				if (RecId == Id || RecId > 0)
				{
					throw new Exception("Please reset form first to delete");
				}
				if (MessageBox.Show("Are you sure to delete?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
				{
					DHLTracking.DeleteById(Id, UserAccount.ID);
					r.Delete();
					MessageBox.Show("Record deleted successfully!");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdfrm.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				long Id = Conversion.ToInt64(r.Cells["DHLTrackingId"].Value);
				if (e.Column.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID((int)Id, 248);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRegister_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtHistory == null || dtHistory.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtHistory, "248-DHLTrackingRegister.rpt");
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
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Expected O, but got Unknown
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Expected O, but got Unknown
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Expected O, but got Unknown
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Expected O, but got Unknown
		//IL_055a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0564: Expected O, but got Unknown
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
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grdfrm = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.datFinalEtaToHistory = new System.Windows.Forms.DateTimePicker();
		this.label11 = new System.Windows.Forms.Label();
		this.datDispatchedToHistory = new System.Windows.Forms.DateTimePicker();
		this.label10 = new System.Windows.Forms.Label();
		this.btnShow = new System.Windows.Forms.Button();
		this.label9 = new System.Windows.Forms.Label();
		this.CmbCurrentStatusHistory = new UltraCombo();
		this.datFinalEtaFromHistory = new System.Windows.Forms.DateTimePicker();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.datDispatchedFromHistory = new System.Windows.Forms.DateTimePicker();
		this.label6 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.txtAwbBillNo = new System.Windows.Forms.TextBox();
		this.datFinalETA = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.datDispatched = new System.Windows.Forms.DateTimePicker();
		this.label51 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label39 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbCurrentStatus = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.txtDetail = new System.Windows.Forms.TextBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnShortcukeys = new System.Windows.Forms.ToolStripButton();
		this.txtAwbBillNoHistory = new UltraCombo();
		this.btnRegister = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel5.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCurrentStatusHistory).BeginInit();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCurrentStatus).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.txtAwbBillNoHistory).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(963, 533);
		this.panel1.TabIndex = 0;
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		this.grdfrm.ColumnAutoResize = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupTotals = (GroupTotals)2;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 233);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(963, 300);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 77;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.ColumnButtonClick += new ColumnActionEventHandler(grdfrm_ColumnButtonClick);
		this.grdfrm.LinkClicked += new ColumnActionEventHandler(grdfrm_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdfrm).DoubleClick += new System.EventHandler(grdfrm_DoubleClick);
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.groupBox1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 163);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(963, 70);
		this.panel5.TabIndex = 79;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.txtAwbBillNoHistory);
		this.groupBox1.Controls.Add(this.datFinalEtaToHistory);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.datDispatchedToHistory);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.btnShow);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCurrentStatusHistory);
		this.groupBox1.Controls.Add(this.datFinalEtaFromHistory);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.datDispatchedFromHistory);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(743, 70);
		this.groupBox1.TabIndex = 78;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "History Filter's";
		this.datFinalEtaToHistory.Checked = false;
		this.datFinalEtaToHistory.CustomFormat = "dd-MMM-yy";
		this.datFinalEtaToHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFinalEtaToHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFinalEtaToHistory.Location = new System.Drawing.Point(298, 42);
		this.datFinalEtaToHistory.Name = "datFinalEtaToHistory";
		this.datFinalEtaToHistory.ShowCheckBox = true;
		this.datFinalEtaToHistory.Size = new System.Drawing.Size(136, 23);
		this.datFinalEtaToHistory.TabIndex = 3;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(239, 46);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(43, 15);
		this.label11.TabIndex = 588;
		this.label11.Text = "To ETA";
		this.datDispatchedToHistory.CustomFormat = "dd-MMM-yy";
		this.datDispatchedToHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datDispatchedToHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datDispatchedToHistory.Location = new System.Drawing.Point(104, 42);
		this.datDispatchedToHistory.Name = "datDispatchedToHistory";
		this.datDispatchedToHistory.ShowCheckBox = true;
		this.datDispatchedToHistory.Size = new System.Drawing.Size(132, 23);
		this.datDispatchedToHistory.TabIndex = 1;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(6, 46);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(83, 15);
		this.label10.TabIndex = 586;
		this.label10.Text = "To Dispatched";
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(684, 41);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(57, 25);
		this.btnShow.TabIndex = 6;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(436, 46);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(82, 15);
		this.label9.TabIndex = 584;
		this.label9.Text = "Current Status";
		this.CmbCurrentStatusHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCurrentStatusHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCurrentStatusHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCurrentStatusHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCurrentStatusHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbCurrentStatusHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCurrentStatusHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCurrentStatusHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCurrentStatusHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbCurrentStatusHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCurrentStatusHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbCurrentStatusHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCurrentStatusHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCurrentStatusHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCurrentStatusHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbCurrentStatusHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCurrentStatusHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCurrentStatusHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCurrentStatusHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbCurrentStatusHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCurrentStatusHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCurrentStatusHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCurrentStatusHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbCurrentStatusHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCurrentStatusHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCurrentStatusHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbCurrentStatusHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCurrentStatusHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbCurrentStatusHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCurrentStatusHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCurrentStatusHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCurrentStatusHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCurrentStatusHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCurrentStatusHistory).Location = new System.Drawing.Point(518, 40);
		((System.Windows.Forms.Control)(object)this.CmbCurrentStatusHistory).Name = "CmbCurrentStatusHistory";
		((System.Windows.Forms.Control)(object)this.CmbCurrentStatusHistory).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.CmbCurrentStatusHistory).TabIndex = 5;
		this.datFinalEtaFromHistory.Checked = false;
		this.datFinalEtaFromHistory.CustomFormat = "dd-MMM-yy";
		this.datFinalEtaFromHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFinalEtaFromHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFinalEtaFromHistory.Location = new System.Drawing.Point(298, 15);
		this.datFinalEtaFromHistory.Name = "datFinalEtaFromHistory";
		this.datFinalEtaFromHistory.ShowCheckBox = true;
		this.datFinalEtaFromHistory.Size = new System.Drawing.Size(136, 23);
		this.datFinalEtaFromHistory.TabIndex = 2;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(239, 19);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(58, 15);
		this.label8.TabIndex = 583;
		this.label8.Text = "From ETA";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(436, 19);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(72, 15);
		this.label7.TabIndex = 582;
		this.label7.Text = "AWB Bill No";
		this.datDispatchedFromHistory.CustomFormat = "dd-MMM-yy";
		this.datDispatchedFromHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datDispatchedFromHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datDispatchedFromHistory.Location = new System.Drawing.Point(104, 15);
		this.datDispatchedFromHistory.Name = "datDispatchedFromHistory";
		this.datDispatchedFromHistory.ShowCheckBox = true;
		this.datDispatchedFromHistory.Size = new System.Drawing.Size(132, 23);
		this.datDispatchedFromHistory.TabIndex = 0;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(6, 19);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(98, 15);
		this.label6.TabIndex = 580;
		this.label6.Text = "From Dispatched";
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label3);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 139);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(963, 24);
		this.panel4.TabIndex = 6;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(3, 3);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(58, 17);
		this.label3.TabIndex = 40;
		this.label3.Text = "History";
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.txtAwbBillNo);
		this.panel3.Controls.Add(this.datFinalETA);
		this.panel3.Controls.Add(this.label5);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add(this.datDispatched);
		this.panel3.Controls.Add(this.label51);
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCurrentStatus);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add(this.txtDetail);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 28);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(963, 111);
		this.panel3.TabIndex = 5;
		this.txtAwbBillNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAwbBillNo.Location = new System.Drawing.Point(127, 55);
		this.txtAwbBillNo.Name = "txtAwbBillNo";
		this.txtAwbBillNo.Size = new System.Drawing.Size(156, 23);
		this.txtAwbBillNo.TabIndex = 1;
		this.datFinalETA.CustomFormat = "dd-MMM-yy";
		this.datFinalETA.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFinalETA.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFinalETA.Location = new System.Drawing.Point(127, 80);
		this.datFinalETA.Name = "datFinalETA";
		this.datFinalETA.Size = new System.Drawing.Size(156, 23);
		this.datFinalETA.TabIndex = 2;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(10, 84);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(55, 15);
		this.label5.TabIndex = 578;
		this.label5.Text = "Final ETA";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(10, 58);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(72, 15);
		this.label4.TabIndex = 576;
		this.label4.Text = "AWB Bill No";
		this.datDispatched.CustomFormat = "dd-MMM-yy";
		this.datDispatched.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datDispatched.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datDispatched.Location = new System.Drawing.Point(127, 30);
		this.datDispatched.Name = "datDispatched";
		this.datDispatched.Size = new System.Drawing.Size(156, 23);
		this.datDispatched.TabIndex = 0;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(10, 34);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(111, 15);
		this.label51.TabIndex = 574;
		this.label51.Text = "Date Of Dispatched";
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label39);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(963, 25);
		this.panel6.TabIndex = 28;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label39.ForeColor = System.Drawing.Color.White;
		this.label39.Location = new System.Drawing.Point(2, 2);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(118, 19);
		this.label39.TabIndex = 40;
		this.label39.Text = "DHL Tracking";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(289, 76);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(52, 30);
		this.label2.TabIndex = 27;
		this.label2.Text = "Currenct\r\n Status";
		this.CmbCurrentStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbCurrentStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCurrentStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCurrentStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCurrentStatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCurrentStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCurrentStatus).Location = new System.Drawing.Point(357, 78);
		((System.Windows.Forms.Control)(object)this.CmbCurrentStatus).Name = "CmbCurrentStatus";
		((System.Windows.Forms.Control)(object)this.CmbCurrentStatus).Size = new System.Drawing.Size(384, 26);
		((System.Windows.Forms.Control)(object)this.CmbCurrentStatus).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbCurrentStatus).Leave += new System.EventHandler(CmbLookupsType_Leave);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(289, 32);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(68, 30);
		this.label1.TabIndex = 9;
		this.label1.Text = "Detail / \r\nDescription";
		this.txtDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDetail.Location = new System.Drawing.Point(357, 30);
		this.txtDetail.Multiline = true;
		this.txtDetail.Name = "txtDetail";
		this.txtDetail.Size = new System.Drawing.Size(384, 47);
		this.txtDetail.TabIndex = 3;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(963, 28);
		this.panel2.TabIndex = 4;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btnNew, this.btnRefresh, this.btnUpdate, this.btnSave, this.btnAttachment, this.btnShortcukeys, this.btnRegister });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(963, 25);
		this.toolStrip1.TabIndex = 29;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 22);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 22);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(74, 22);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(60, 22);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(105, 22);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnShortcukeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcukeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcukeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcukeys.Name = "btnShortcukeys";
		this.btnShortcukeys.Size = new System.Drawing.Size(122, 22);
		this.btnShortcukeys.Text = "ShortCut Keys";
		this.btnShortcukeys.Click += new System.EventHandler(btnShortcukeys_Click);
		this.txtAwbBillNoHistory.AllowNull = (DefaultableBoolean)1;
		this.txtAwbBillNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.txtAwbBillNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.txtAwbBillNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.txtAwbBillNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.txtAwbBillNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.txtAwbBillNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.txtAwbBillNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtAwbBillNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.txtAwbBillNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtAwbBillNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.txtAwbBillNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.txtAwbBillNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.txtAwbBillNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.txtAwbBillNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.txtAwbBillNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.txtAwbBillNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtAwbBillNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.txtAwbBillNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.txtAwbBillNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.txtAwbBillNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtAwbBillNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.txtAwbBillNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.txtAwbBillNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.txtAwbBillNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.txtAwbBillNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.txtAwbBillNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.txtAwbBillNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.txtAwbBillNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.txtAwbBillNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.txtAwbBillNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.txtAwbBillNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAwbBillNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.txtAwbBillNoHistory).Location = new System.Drawing.Point(518, 13);
		((System.Windows.Forms.Control)(object)this.txtAwbBillNoHistory).Name = "txtAwbBillNoHistory";
		((System.Windows.Forms.Control)(object)this.txtAwbBillNoHistory).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.txtAwbBillNoHistory).TabIndex = 4;
		this.btnRegister.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRegister.Name = "btnRegister";
		this.btnRegister.Size = new System.Drawing.Size(111, 22);
		this.btnRegister.Text = "Register-248";
		this.btnRegister.Click += new System.EventHandler(btnRegister_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(963, 533);
		base.Controls.Add(this.panel1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.KeyPreview = true;
		base.Name = "frmDhlTracking";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "EccountBookERP";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(ExImLookups_FormClosed);
		base.Load += new System.EventHandler(DateLock_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ExImLookups_KeyDown);
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel5.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCurrentStatusHistory).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCurrentStatus).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.txtAwbBillNoHistory).EndInit();
		base.ResumeLayout(false);
	}
}
