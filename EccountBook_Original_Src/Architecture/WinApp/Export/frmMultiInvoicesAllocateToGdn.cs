using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL;
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
using grid.User_Conrtols;

namespace Architecture.WinApp.Export;

public class frmMultiInvoicesAllocateToGdn : Form
{
	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private Attachment AT = new Attachment();

	private Rightsobjects formrights = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtdetail = new DataTable();

	private DataTable dtcol = new DataTable();

	private bool FormInitialized;

	private string ScreenName = "frmMultiInvoicesAllocateToGdn";

	private int DocumentTypeId = 2;

	private int RecId;

	private int updateDetailIndex = -1;

	private string CustomAttachmentsValues = "";

	private string AttachmentsValues = "";

	private int DefaultDaysToLessFromHistoryFromDate;

	private List<GDBreakUpDetail> lstRemoveRecord = new List<GDBreakUpDetail>();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel4;

	private Panel panel3;

	private TabPage tabPage2;

	private ToolStrip toolstrip;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton Print;

	private Panel panel17;

	private ToolStrip toolStrip3;

	private ToolStripButton btnAttachment;

	private ToolStripButton btnUpdate;

	private ToolStripButton BtnRefresh;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel28;

	private Label label44;

	private ToolStripButton btnShortCutKey;

	private TextBox txtDeliveryRemarks;

	private TextBox txtApprovalRemarks;

	private Label label50;

	private Label label52;

	internal GridEX DataGridHistory;

	private Panel panel29;

	private Label label65;

	private Panel panel18;

	private GroupBox groupBox5;

	private Button btnShowHistory;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Label label91;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	private ToolStripButton BtnSaveAs;

	private ToolStripButton BtnDelete;

	private CheckBox ChkPreview;

	private GroupBox groupBox3;

	private TextBox txtRemarksMultiInvoiceForm;

	private Label label130;

	private TextBox FttAmountMultiInvoiceForm;

	private Label label131;

	private TextBox txtNetAmountMultiInvoiceForm;

	private Label label132;

	private TextBox FttPrcntMultiInvoiceForm;

	private Label label133;

	private TextBox txtFobValueMultiInvoiceForm;

	private Label label134;

	private TextBox txtFreightMultiInvoiceForm;

	private Label label135;

	private TextBox txtGdValueMultiInvoiceForm;

	private Label label136;

	private TextBox txtExchangeRateMultiInvoiceForm;

	private Label label123;

	private TextBox txtDueDaysMultiInvoiceForm;

	private Label label124;

	internal UltraCombo CmbBankMultiInvoiceForm;

	private Label label125;

	private TextBox GdNoMultiInvoiceForm;

	private Label label126;

	private TextBox BankInvoiceNoMultiInvoiceForm;

	private Label label127;

	private DateTimePicker GdDateMultiInvoiceForm;

	private DateTimePicker BankInvoiceDateMultiInvoiceForm;

	private Label label122;

	private GroupBox groupBox4;

	private Label label139;

	private TextBox txtRemarksDetail;

	private Button btnAdd;

	private Button btnUpdateDetail;

	private Button btnCancelDetail;

	private TextBox txtInvoiceAmountDetail;

	private Label label138;

	internal UltraCombo CmbInvoiceNoDetail;

	private Label label137;

	internal GridEX grd;

	private Panel panel38;

	private Label label140;

	private CtrlGrdBar ctrlGrdBar2;

	internal GridEX grddetailhistory;

	private Panel panel13;

	private Label label59;

	internal UltraCombo CmbBankMultiInvoiceAgainstGdHistory;

	private Label label142;

	public frmMultiInvoicesAllocateToGdn(UserAccount user)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
		InitializeComponentMethod();
		FormHelper.FormEventHandler(this);
	}

	private void InitializeComponentCustom()
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1250, 800);
			ScreenName = Conversion.ToString((object)ScreenName);
			dtViewAttachments.Columns.Add("AttchmentPath");
			dtdetail.Columns.Add("Id", typeof(int));
			dtdetail.Columns.Add("InvoiceId", typeof(int));
			dtdetail.Columns.Add("InvoiceNo");
			dtdetail.Columns.Add("InvoiceAmount", typeof(double));
			dtdetail.Columns.Add("Remarks");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeComponentMethod()
	{
		Task.Run(delegate
		{
			formrights = CommonServices.SetRightsValueInRightsObject(ScreenName);
			List<Bank> bankData = BanksDbCall();
			DataTable bankInvoicesForGdBreakUp = ExImInvoice.GetBankInvoicesForGdBreakUp(UserAccount.OrganizationId, UserAccount.CompanyId, RecId);
			DataTable dataForDropDownFromGDBreakUpHeader = GDBreakUpHeader.GetDataForDropDownFromGDBreakUpHeader(UserAccount.OrganizationId, UserAccount.CompanyId, "Bank");
			return new
			{
				BankData = bankData,
				InvoiceData = bankInvoicesForGdBreakUp,
				HistoryComboData = dataForDropDownFromGDBreakUpHeader
			};
		}).ContinueWith(t =>
		{
			if (t.IsFaulted)
			{
				MessageBox.Show("Error occurred during database call.");
			}
			else
			{
				var result = t.Result;
				btnsave.Enabled = formrights.DoHaveSaveRight;
				BtnSaveAs.Enabled = formrights.DoHaveSaveRight;
				btnUpdate.Enabled = formrights.DoHaveUpdateRights;
				BtnDelete.Enabled = formrights.DoHaveCanDelete;
				Print.Enabled = formrights.DoHavePrintRights;
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar1, formrights);
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar2, formrights);
				GetConfigurationsFromGlobal();
				_ = RecId;
				ImporterAndExportBankBind(result.BankData);
				BankInvoicesBind(result.InvoiceData);
				BindGrids();
				HistoryComboBind(result.HistoryComboData);
				FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				ToDateHistory.Value = DateTime.Now;
				FormInitialized = true;
				GdDateMultiInvoiceForm.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void HistoryComboBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbBankMultiInvoiceAgainstGdHistory, dt, "Id", "name", "Bank Name", AllColumns: true, Conversion.ToInt(CmbBankMultiInvoiceAgainstGdHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetConfigurationsFromGlobal()
	{
		try
		{
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private List<Bank> BanksDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return Bank.GetAll(new Bank
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

	private void ImporterAndExportBankBind(List<Bank> lst)
	{
		DataTable dtForeign = new DataTable();
		dtForeign.Columns.Add("Id", typeof(int));
		dtForeign.Columns.Add("BranchName");
		dtForeign.Columns.Add("BankIBANNo");
		DataTable dtHome = dtForeign.Clone();
		if (lst.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < lst.Count; i++)
		{
			if (lst[i].IsHomeland == "Foreign Country")
			{
				dtForeign.Rows.Add(lst[i].Id, lst[i].BranchName, lst[i].BankIBANNo);
			}
			else if (lst[i].IsHomeland == "Home Country")
			{
				dtHome.Rows.Add(lst[i].Id, lst[i].BranchName, lst[i].BankIBANNo);
			}
		}
		InfragisticsHelper.BindAndRetainSelection(CmbBankMultiInvoiceForm, dtHome, "Id", "BranchName", "Importer Bank", AllColumns: true, Conversion.ToInt(CmbBankMultiInvoiceForm.Value), insertDefaultRow: false, null, null, ActivateRow: false);
	}

	private void BankInvoicesBind(DataTable dtInvoice)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbInvoiceNoDetail, dtInvoice, "Id", "InvoiceNo", "Invoice No", AllColumns: true, Conversion.ToInt(CmbInvoiceNoDetail.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool DetailFormValidation()
	{
		if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
		{
			((Control)(object)CmbInvoiceNoDetail, "Invoice No", FormHelper.ValidType.INT),
			(txtInvoiceAmountDetail, "Invoice Amount", FormHelper.ValidType.Double)
		}))
		{
			return false;
		}
		return true;
	}

	private void BindGrids()
	{
		try
		{
			if (dtdetail != null && dtdetail.Columns.Count > 0)
			{
				grd.DataSource = dtdetail;
				grd.RetrieveStructure();
				grdSettings(grd);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (DetailFormValidation())
			{
				if (GridEX_Helper.IsAnyCellValueHaveSameValue(grd, "InvoiceId", Conversion.ToInt(CmbInvoiceNoDetail.Value)))
				{
					MessageBox.Show("Invoice " + ((Control)(object)CmbInvoiceNoDetail).Text + " Already in Detail Grid.So you can't select this invoice again.", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				dtdetail.Rows.Add(0, Conversion.ToInt(CmbInvoiceNoDetail.Value), ((Control)(object)CmbInvoiceNoDetail).Text, Conversion.ToDouble((object)txtInvoiceAmountDetail.Text), txtRemarksDetail.Text);
				grd.DataSource = dtdetail;
				grd.RetrieveStructure();
				grdSettings(grd);
				ResetDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				updateDetailIndex = item.RowIndex;
				CmbInvoiceNoDetail.Value = item.Cells["InvoiceId"].Value;
				txtInvoiceAmountDetail.Text = Conversion.ToDouble(item.Cells["InvoiceAmount"].Value).ToString("#,##0.###");
				txtRemarksDetail.Text = item.Cells["Remarks"].Value.ToString();
				btnAdd.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelDetail.Visible = true;
				CmbInvoiceNoDetail.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (DetailFormValidation())
			{
				if (GridEX_Helper.IsAnyCellValueHaveSameValue(grd, "InvoiceId", Conversion.ToInt(CmbInvoiceNoDetail.Value), updateDetailIndex))
				{
					MessageBox.Show("Invoice " + ((Control)(object)CmbInvoiceNoDetail).Text + " Already in Detail Grid.So you can't select this invoice again.", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				dtdetail.Rows[updateDetailIndex]["InvoiceId"] = Conversion.ToInt(CmbInvoiceNoDetail.Value);
				dtdetail.Rows[updateDetailIndex]["InvoiceNo"] = Conversion.ToString((object)((Control)(object)CmbInvoiceNoDetail).Text);
				dtdetail.Rows[updateDetailIndex]["InvoiceAmount"] = Conversion.ToDouble((object)txtInvoiceAmountDetail.Text);
				dtdetail.Rows[updateDetailIndex]["Remarks"] = Conversion.ToString((object)txtRemarksDetail.Text);
				ResetDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		ResetDetail();
	}

	private void grdSettings(GridEX grid)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "InvoiceId" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType> { 
			{
				"Remarks",
				(EditType)1
			} };
			GridEX_Helper.GridWrappingAndColumnSettings(grid, 2, 3, hiddenColumns, editableColumns);
			if (((Control)(object)grid).Name == "grd")
			{
				GridEX_Helper.AddButton(grd, "Delete", "X", 20, 1);
				grid.FrozenColumns = 1;
				ctrlGrdBar1_Load(null, null);
			}
			else
			{
				grid.AllowEdit = (InheritableBoolean)2;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "Delete")
			{
				DeleteDetailRow(r);
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
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (updateDetailIndex == -1)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
					{
						GDBreakUpDetail vd = new GDBreakUpDetail();
						vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
						FillDetailListCommonForInsertAndDelete(vd, r);
						vd.ActionTypeId = 3;
						lstRemoveRecord.Add(vd);
						dtdetail.Rows[r.RowIndex].Delete();
						r.Delete();
						grd.UpdateData();
					}
				}
				else
				{
					r.Delete();
					grd.UpdateData();
				}
				return;
			}
			throw new Exception("Reset Detail First");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
			{
				(GdNoMultiInvoiceForm, "Gd No", FormHelper.ValidType.INT),
				(BankInvoiceNoMultiInvoiceForm, "Bank Invoice No", FormHelper.ValidType.INT),
				((Control)(object)CmbBankMultiInvoiceForm, "Bank", FormHelper.ValidType.INT),
				(txtDueDaysMultiInvoiceForm, "Due Days", FormHelper.ValidType.INT),
				(txtGdValueMultiInvoiceForm, "GD Value", FormHelper.ValidType.Double),
				(txtFobValueMultiInvoiceForm, "FOB Value", FormHelper.ValidType.Double),
				(txtNetAmountMultiInvoiceForm, "Net Amount", FormHelper.ValidType.Double)
			}) || !FormHelper.ConfirmAction((RecId == 0) ? "Are you sure to Save" : "Are you sure to Update"))
			{
				return;
			}
			GDBreakUpHeader obj = new GDBreakUpHeader();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = DocumentTypeId;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyUserId = UserAccount.ID;
			obj.Id = RecId;
			obj.GDNO = GdNoMultiInvoiceForm.Text;
			obj.GDDate = GdDateMultiInvoiceForm.Value;
			obj.BankInvoiceNo = BankInvoiceNoMultiInvoiceForm.Text;
			obj.BankInvoiceDate = BankInvoiceDateMultiInvoiceForm.Value;
			obj.BankId = Conversion.ToInt(CmbBankMultiInvoiceForm.Value);
			obj.DueDays = Conversion.ToInt((object)txtDueDaysMultiInvoiceForm.Text);
			obj.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRateMultiInvoiceForm.Text);
			obj.GDValue = Conversion.ToDecimal((object)txtGdValueMultiInvoiceForm.Text);
			obj.Freight = Conversion.ToDecimal((object)txtFreightMultiInvoiceForm.Text);
			obj.FobValue = Conversion.ToDecimal((object)txtFobValueMultiInvoiceForm.Text);
			obj.CommPercent = Conversion.ToDecimal((object)FttPrcntMultiInvoiceForm.Text);
			obj.CommAmount = Conversion.ToDecimal((object)FttAmountMultiInvoiceForm.Text);
			obj.NetToBeRealized = Conversion.ToDecimal((object)txtNetAmountMultiInvoiceForm.Text);
			obj.Remarks = txtRemarksMultiInvoiceForm.Text;
			obj.GDWeight = 0m;
			obj.GDNoofContrainers = 0;
			obj.GDBreakUpDetailList = new List<GDBreakUpDetail>();
			if (lstRemoveRecord.Count > 0 && RecId > 0)
			{
				for (int i = 0; i < lstRemoveRecord.Count; i++)
				{
					obj.GDBreakUpDetailList.Add(lstRemoveRecord[i]);
				}
			}
			string RemarksFromDtail = "";
			decimal InvoiceAmount = default(decimal);
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				GDBreakUpDetail vd = new GDBreakUpDetail();
				vd.Id = ((RecId != 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
				vd.ActionTypeId = ((vd.Id <= 0) ? 1 : 2);
				FillDetailListCommonForInsertAndDelete(vd, r);
				FormHelper.ValidateField(vd.ExImInvoiceId, "Invoice", r.RowIndex);
				FormHelper.ValidateField(vd.InvoiceAmount, "Invoice Amount", r.RowIndex);
				if (string.IsNullOrWhiteSpace(obj.Remarks))
				{
					RemarksFromDtail = vd.Remarks;
				}
				obj.GDBreakUpDetailList.Add(vd);
				InvoiceAmount += vd.InvoiceAmount;
			}
			if (obj.GDValue != InvoiceAmount)
			{
				throw new Exception("Invoice's Amount must be equal to Total GDValue Please Check");
			}
			if (obj.Remarks == "")
			{
				obj.Remarks = RemarksFromDtail;
			}
			obj.AttachmentsList = new List<DMSAttachments>();
			obj.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<GDBreakUpHeader>(obj, ScreenName, AT, "AttachmentsList", "DeleteAttachmentsList");
			obj.AttachmentsValues = AttachmentsValues;
			obj.CustomAttachmentsValues = CustomAttachmentsValues;
			int success = GDBreakUpHeader.Save(obj);
			if (RecId == 0)
			{
				MessageBox.Show("Record Save Successfully");
			}
			else
			{
				MessageBox.Show("Record Update Successfully");
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, obj.DocumentTypeId, base.Name);
			}
			Reset();
			if (ChkPreview.Checked)
			{
				GenerateReport(success);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnSaveAs_Click(object sender, EventArgs e)
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

	private void BtnDelete_Click(object sender, EventArgs e)
	{
	}

	private void FillDetailListCommonForInsertAndDelete(GDBreakUpDetail vd, GridEXRow r)
	{
		vd.ExImInvoiceId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
		vd.InvoiceNo = Conversion.ToString(r.Cells["InvoiceNo"].Value);
		vd.InvoiceAmount = Conversion.ToDecimal(r.Cells["InvoiceAmount"].Value);
		vd.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
	}

	public void ReadById(int ID)
	{
		try
		{
			Reset();
			RecId = ID;
			GDBreakUpHeader Obj = GDBreakUpHeader.GetByID(ID);
			if (Obj != null)
			{
				tabControl1.SelectedIndex = 0;
				btnsave.Visible = false;
				btnUpdate.Visible = true;
				BtnSaveAs.Visible = false;
				GdNoMultiInvoiceForm.Text = Obj.GDNO;
				GdDateMultiInvoiceForm.Value = Obj.GDDate;
				BankInvoiceNoMultiInvoiceForm.Text = Obj.BankInvoiceNo;
				BankInvoiceDateMultiInvoiceForm.Value = Obj.BankInvoiceDate;
				CmbBankMultiInvoiceForm.Value = Obj.BankId;
				txtDueDaysMultiInvoiceForm.Text = Obj.DueDays.ToString();
				txtExchangeRateMultiInvoiceForm.Text = Obj.ExchangeRate.ToString("#,##0.####");
				txtGdValueMultiInvoiceForm.Text = Obj.GDValue.ToString("#,##0.####");
				txtFreightMultiInvoiceForm.Text = Obj.Freight.ToString("#,##0.####");
				txtFobValueMultiInvoiceForm.Text = Obj.FobValue.ToString("#,##0.####");
				FttPrcntMultiInvoiceForm.Text = Obj.CommPercent.ToString("#,##0.####");
				FttAmountMultiInvoiceForm.Text = Obj.CommAmount.ToString("#,##0.####");
				txtNetAmountMultiInvoiceForm.Text = Obj.NetToBeRealized.ToString("#,##0.####");
				txtRemarksMultiInvoiceForm.Text = Obj.Remarks;
				AttachmentsValues = Obj.AttachmentsValues;
				CustomAttachmentsValues = Obj.CustomAttachmentsValues;
				dtdetail.Rows.Clear();
				FillDetailFromListCommonForReadById(Obj, dtdetail);
				BindGrids();
				FormHelper.LoadAttachmentsForObject(AT, RecId, ScreenName);
				BankInvoicesBind(ExImInvoice.GetBankInvoicesForGdBreakUp(UserAccount.OrganizationId, UserAccount.CompanyId, RecId));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailFromListCommonForReadById(GDBreakUpHeader invGrn, DataTable dt)
	{
		if (invGrn == null || invGrn.GDBreakUpDetailList == null || invGrn.GDBreakUpDetailList.Count == 0)
		{
			return;
		}
		foreach (GDBreakUpDetail detail in invGrn.GDBreakUpDetailList)
		{
			List<object> rowValues = new List<object> { detail.Id, detail.ExImInvoiceId, detail.InvoiceNo, detail.InvoiceAmount, detail.Remarks };
			dt.Rows.Add(rowValues.ToArray());
		}
	}

	private void frmMultiInvoicesAllocateToGdn_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			GetConfigurationsFromGlobal();
			ImporterAndExportBankBind(BanksDbCall());
			BankInvoicesBind(ExImInvoice.GetBankInvoicesForGdBreakUp(UserAccount.OrganizationId, UserAccount.CompanyId, RecId));
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
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			lstRemoveRecord.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			BtnDelete.Visible = false;
			BtnSaveAs.Visible = false;
			RecId = 0;
			GdNoMultiInvoiceForm.Text = string.Empty;
			BankInvoiceNoMultiInvoiceForm.Text = string.Empty;
			((Control)(object)CmbBankMultiInvoiceForm).Text = string.Empty;
			txtDueDaysMultiInvoiceForm.Text = string.Empty;
			txtExchangeRateMultiInvoiceForm.Text = string.Empty;
			txtGdValueMultiInvoiceForm.Text = string.Empty;
			txtFreightMultiInvoiceForm.Text = string.Empty;
			txtFobValueMultiInvoiceForm.Text = string.Empty;
			FttPrcntMultiInvoiceForm.Text = "10";
			FttAmountMultiInvoiceForm.Text = string.Empty;
			txtNetAmountMultiInvoiceForm.Text = string.Empty;
			txtRemarksMultiInvoiceForm.Text = string.Empty;
			txtApprovalRemarks.Text = string.Empty;
			txtDeliveryRemarks.Text = string.Empty;
			ResetDetail();
			dtdetail.Rows.Clear();
			BindGrids();
			tabControl1.SelectedIndex = 0;
			GdDateMultiInvoiceForm.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetDetail()
	{
		try
		{
			updateDetailIndex = -1;
			((Control)(object)CmbInvoiceNoDetail).Text = string.Empty;
			txtInvoiceAmountDetail.Text = string.Empty;
			txtRemarksDetail.Text = string.Empty;
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			CmbInvoiceNoDetail.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
		ResetDetail();
	}

	private void btnResetHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			((Control)(object)CmbBankMultiInvoiceAgainstGdHistory).Text = string.Empty;
			DataGridHistory.DataSource = null;
			grddetailhistory.DataSource = null;
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
			HistoryComboBind(GDBreakUpHeader.GetDataForDropDownFromGDBreakUpHeader(UserAccount.OrganizationId, UserAccount.CompanyId, "Bank"));
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
			obj.DocumentTypeIds = DocumentTypeId.ToString();
			obj.CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
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
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ModifyDateTo = ToDateHistory.Value;
				}
			}
			obj.AccountId = Conversion.ToInt(CmbBankMultiInvoiceAgainstGdHistory.Value);
			DataTable dt = GDBreakUpHeader.FormHistory(obj);
			if (dt.Rows.Count > 0)
			{
				DataTable dtHistory = new DataTable();
				dtHistory.Columns.Add("Id", typeof(int));
				dtHistory.Columns.Add("InvoiceNo's", typeof(string));
				dtHistory.Columns.Add("GDNo", typeof(string));
				dtHistory.Columns.Add("GDDate", typeof(DateTime));
				dtHistory.Columns.Add("BankInvoiceNo", typeof(string));
				dtHistory.Columns.Add("BankInvoiceDate", typeof(DateTime));
				dtHistory.Columns.Add("BankName", typeof(string));
				dtHistory.Columns.Add("DueDays", typeof(int));
				dtHistory.Columns.Add("ExchangeRate", typeof(double));
				dtHistory.Columns.Add("GDValue", typeof(double));
				dtHistory.Columns.Add("Freight", typeof(double));
				dtHistory.Columns.Add("FobValue", typeof(double));
				dtHistory.Columns.Add("Ftt%", typeof(double));
				dtHistory.Columns.Add("FttAmount", typeof(double));
				dtHistory.Columns.Add("NetAmount", typeof(double));
				dtHistory.Columns.Add("EntryUser", typeof(string));
				dtHistory.Columns.Add("EntryDate", typeof(DateTime));
				dtHistory.Columns.Add("ModifyUser", typeof(string));
				dtHistory.Columns.Add("ModifyDate", typeof(DateTime));
				dtHistory.Columns.Add("Remarks", typeof(string));
				dtHistory.Columns.Add("NoOfAttachments", typeof(int));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtHistory.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["InvoiceNo"], dt.Rows[i]["GDNO"], Conversion.ToDateTime(dt.Rows[i]["GDDate"]).ToShortDateString(), dt.Rows[i]["BankInvoiceNo"], Conversion.ToDateTime(dt.Rows[i]["BankInvoiceDate"]).ToShortDateString(), dt.Rows[i]["BankName"], dt.Rows[i]["DueDays"], dt.Rows[i]["ExchangeRate"], dt.Rows[i]["GDValue"], dt.Rows[i]["Freight"], dt.Rows[i]["FobValue"], dt.Rows[i]["CommPercent"], dt.Rows[i]["CommAmount"], dt.Rows[i]["NetToBeRealized"], dt.Rows[i]["EntryUser"], Conversion.ToDateTime(dt.Rows[i]["EntryDate"]).ToShortDateString(), dt.Rows[i]["ModifyUser"], Conversion.ToDateTime(dt.Rows[i]["ModifyDate"]).ToShortDateString(), dt.Rows[i]["Remarks"], dt.Rows[i]["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dtHistory;
				DataGridHistory.RetrieveStructure();
				GridSetting();
			}
			else
			{
				DataGridHistory.ClearStructure();
				grddetailhistory.ClearStructure();
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
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 2, 3);
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			GridEX_Helper.AddButton(DataGridHistory, "Edit", "Edit", 50, 0);
			GridEX_Helper.AddButton(DataGridHistory, "Print", "Print", 50, 1);
			DataGridHistory.FrozenColumns = 3;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
				ReadById(Conversion.ToInt(r.Cells["Id"].Value));
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
			if (r != null && (int)r.RowType == 0 && (int)r.RowType != 5 && r.Cells.Count > 0)
			{
				if (e.Column.Key == "Print")
				{
					GenerateReport(Conversion.ToInt(r.Cells["Id"].Value));
				}
				else if (e.Column.Key == "Edit")
				{
					ReadById(Conversion.ToInt(r.Cells["Id"].Value));
				}
				else if (e.Column.Key == "SaveAs")
				{
					HistoryGridSaveAsButtonClick(Conversion.ToInt(r.Cells["Id"].Value));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSaveAsButtonClick(int CurrId)
	{
		ReadById(CurrId);
		RecId = 0;
		BtnSaveAs.Visible = true;
		btnsave.Visible = false;
		btnUpdate.Visible = false;
		BtnDelete.Visible = false;
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				GetDetailGrdByHeadId(Conversion.ToInt(r.Cells["Id"].Value));
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
			CommonServices.GetNoofAttachmentsByScreenName(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), ScreenName);
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
			GDBreakUpHeader Obj = GDBreakUpHeader.GetByID(Id);
			if (Obj != null)
			{
				List<GDBreakUpDetail> gDBreakUpDetailList = Obj.GDBreakUpDetailList;
				if (gDBreakUpDetailList == null || gDBreakUpDetailList.Count != 0)
				{
					DataTable dtDetailHistory = dtdetail.Clone();
					FillDetailFromListCommonForReadById(Obj, dtDetailHistory);
					grddetailhistory.DataSource = dtDetailHistory;
					grddetailhistory.RetrieveStructure();
					grdSettings(grddetailhistory);
					return;
				}
			}
			grddetailhistory.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReport(int PrintId)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = new DataTable();
			dt = GDBreakUpHeader.GDBreakUpHeader_Slip(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "02_GDBreakUpHeader_Slip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Print_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				GenerateReport(RecId);
				return;
			}
			throw new Exception("No Record found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrintII_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.SaleContractReportsSlipNew501A(RecId);
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

	private void CmbInvoiceNoDetail_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbInvoiceNoDetail).ActiveRow != null)
			{
				txtInvoiceAmountDetail.Text = Conversion.ToDouble(((UltraDropDownBase)CmbInvoiceNoDetail).SelectedRow.Cells[2].Value).ToString("#,##0.####");
			}
			else
			{
				txtInvoiceAmountDetail.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtGdValueMultiInvoiceForm_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateFobValueMultiInvoiceForm();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFreightMultiInvoiceForm_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateFobValueMultiInvoiceForm();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFobValueMultiInvoiceForm_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateFTTAmountMultiInvoiceForm();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FttPrcntMultiInvoiceForm_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateFTTAmountMultiInvoiceForm();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FttAmountMultiInvoiceForm_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateNetAmountMultiInvoiceForm();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateFobValueMultiInvoiceForm()
	{
		try
		{
			double num = Conversion.ToDouble((object)txtGdValueMultiInvoiceForm.Text);
			double freight = Conversion.ToDouble((object)txtFreightMultiInvoiceForm.Text);
			if (num > 0.0 && freight > 0.0 && Conversion.ToDouble((object)txtFreightMultiInvoiceForm.Text) > Conversion.ToDouble((object)txtGdValueMultiInvoiceForm.Text))
			{
				MessageBox.Show("Freight Can't be Greater Than GD Value");
				txtFreightMultiInvoiceForm.Text = "0";
			}
			double Fob = Conversion.ToDouble((object)txtGdValueMultiInvoiceForm.Text) - Conversion.ToDouble((object)txtFreightMultiInvoiceForm.Text);
			txtFobValueMultiInvoiceForm.Text = Fob.ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CalculateFTTAmountMultiInvoiceForm()
	{
		try
		{
			if (Conversion.ToDouble((object)FttPrcntMultiInvoiceForm.Text) > 100.0)
			{
				FttPrcntMultiInvoiceForm.Text = "10";
				MessageBox.Show("Percent Cant be Greater Than 100", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			double FTTAmount = Conversion.ToDouble((object)txtFobValueMultiInvoiceForm.Text) * Conversion.ToDouble((object)FttPrcntMultiInvoiceForm.Text) / 100.0;
			FttAmountMultiInvoiceForm.Text = FTTAmount.ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CalculateNetAmountMultiInvoiceForm()
	{
		try
		{
			if (Conversion.ToDouble((object)txtGdValueMultiInvoiceForm.Text) > 0.0 && Conversion.ToDouble((object)FttAmountMultiInvoiceForm.Text) > 0.0)
			{
				double NetAmount = Conversion.ToDouble((object)txtGdValueMultiInvoiceForm.Text) - Conversion.ToDouble((object)FttAmountMultiInvoiceForm.Text);
				txtNetAmountMultiInvoiceForm.Text = NetAmount.ToString("#,##0.###");
			}
			else
			{
				txtNetAmountMultiInvoiceForm.Text = 0.ToString("#,##0.###");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grd, dtdetail);
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, DataGridHistory, dtcol);
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
			FromDateHistory.Focus();
		}
		else
		{
			GdDateMultiInvoiceForm.Focus();
		}
	}

	private void frmMultiInvoicesAllocateToGdn_KeyDown(object sender, KeyEventArgs e)
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
					GdDateMultiInvoiceForm.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					((Control)(object)DataGridHistory).Focus();
				}
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && RecId > 0)
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
				if (e.Control && e.KeyCode == Keys.F5)
				{
					GdDateMultiInvoiceForm.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					btnAttachment_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
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
					((Control)(object)grddetailhistory).Focus();
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Control && e.KeyCode == Keys.Return)
				{
					grd_DoubleClick(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Space && grd.CurrentColumn != null && grd.CurrentColumn.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
			}
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
			int Id = Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (formrights.DoHaveUpdateRights)
				{
					ReadById(Id);
					btnsave.Visible = false;
					btnUpdate.Visible = true;
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
					ReadById(Id);
				}
				if (DataGridHistory.CurrentColumn.Key == "SaveAs")
				{
					HistoryGridSaveAsButtonClick(Id);
				}
				if (DataGridHistory.CurrentColumn.Key == "Print")
				{
					if (formrights.DoHavePrintRights)
					{
						GenerateReport(Id);
					}
				}
				else if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByScreenName(Id, ScreenName);
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
			dt.Rows.Add("Ctrl+F1", "For Open Delivery Term Popup");
			dt.Rows.Add("Ctrl+F2", "For Open Payterm Popup");
			dt.Rows.Add("Ctrl+F3", "For Open Sea Ports Popup");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+L", "For Load All Records");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowRight", "For Moving In Detail Grids");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On First Column in Detail for Entry");
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
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Expected O, but got Unknown
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Expected O, but got Unknown
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Expected O, but got Unknown
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Expected O, but got Unknown
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Expected O, but got Unknown
		//IL_0952: Unknown result type (might be due to invalid IL or missing references)
		//IL_095c: Expected O, but got Unknown
		//IL_36f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_36fa: Expected O, but got Unknown
		//IL_371e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3728: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grddetailhistory_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Export.frmMultiInvoicesAllocateToGdn));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel38 = new System.Windows.Forms.Panel();
		this.label140 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.label139 = new System.Windows.Forms.Label();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.btnAdd = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.txtInvoiceAmountDetail = new System.Windows.Forms.TextBox();
		this.label138 = new System.Windows.Forms.Label();
		this.CmbInvoiceNoDetail = new UltraCombo();
		this.label137 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.txtRemarksMultiInvoiceForm = new System.Windows.Forms.TextBox();
		this.label130 = new System.Windows.Forms.Label();
		this.FttAmountMultiInvoiceForm = new System.Windows.Forms.TextBox();
		this.label131 = new System.Windows.Forms.Label();
		this.txtNetAmountMultiInvoiceForm = new System.Windows.Forms.TextBox();
		this.label132 = new System.Windows.Forms.Label();
		this.FttPrcntMultiInvoiceForm = new System.Windows.Forms.TextBox();
		this.label133 = new System.Windows.Forms.Label();
		this.txtFobValueMultiInvoiceForm = new System.Windows.Forms.TextBox();
		this.label134 = new System.Windows.Forms.Label();
		this.txtFreightMultiInvoiceForm = new System.Windows.Forms.TextBox();
		this.label135 = new System.Windows.Forms.Label();
		this.txtGdValueMultiInvoiceForm = new System.Windows.Forms.TextBox();
		this.label136 = new System.Windows.Forms.Label();
		this.txtExchangeRateMultiInvoiceForm = new System.Windows.Forms.TextBox();
		this.label123 = new System.Windows.Forms.Label();
		this.txtDueDaysMultiInvoiceForm = new System.Windows.Forms.TextBox();
		this.label124 = new System.Windows.Forms.Label();
		this.CmbBankMultiInvoiceForm = new UltraCombo();
		this.label125 = new System.Windows.Forms.Label();
		this.GdNoMultiInvoiceForm = new System.Windows.Forms.TextBox();
		this.label126 = new System.Windows.Forms.Label();
		this.BankInvoiceNoMultiInvoiceForm = new System.Windows.Forms.TextBox();
		this.label127 = new System.Windows.Forms.Label();
		this.GdDateMultiInvoiceForm = new System.Windows.Forms.DateTimePicker();
		this.BankInvoiceDateMultiInvoiceForm = new System.Windows.Forms.DateTimePicker();
		this.txtApprovalRemarks = new System.Windows.Forms.TextBox();
		this.label44 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.txtDeliveryRemarks = new System.Windows.Forms.TextBox();
		this.panel28 = new System.Windows.Forms.Panel();
		this.label122 = new System.Windows.Forms.Label();
		this.ChkPreview = new System.Windows.Forms.CheckBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolstrip = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.BtnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.BtnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label59 = new System.Windows.Forms.Label();
		this.grddetailhistory = new GridEX();
		this.panel29 = new System.Windows.Forms.Panel();
		this.label65 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel18 = new System.Windows.Forms.Panel();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.CmbBankMultiInvoiceAgainstGdHistory = new UltraCombo();
		this.label142 = new System.Windows.Forms.Label();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.label91 = new System.Windows.Forms.Label();
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
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel38.SuspendLayout();
		this.panel2.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNoDetail).BeginInit();
		this.panel4.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBankMultiInvoiceForm).BeginInit();
		this.panel28.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolstrip.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grddetailhistory).BeginInit();
		this.panel29.SuspendLayout();
		this.panel18.SuspendLayout();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBankMultiInvoiceAgainstGdHistory).BeginInit();
		this.panel17.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(964, 561);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(964, 561);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage1.Controls.Add(this.panel38);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel28);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(956, 532);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 277);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grd).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(950, 252);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 10;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel38.BackColor = System.Drawing.Color.Teal;
		this.panel38.Controls.Add(this.label140);
		this.panel38.Controls.Add(this.ctrlGrdBar2);
		this.panel38.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel38.Location = new System.Drawing.Point(3, 252);
		this.panel38.Name = "panel38";
		this.panel38.Size = new System.Drawing.Size(950, 25);
		this.panel38.TabIndex = 9;
		this.label140.AutoSize = true;
		this.label140.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label140.ForeColor = System.Drawing.Color.White;
		this.label140.Location = new System.Drawing.Point(6, 4);
		this.label140.Name = "label140";
		this.label140.Size = new System.Drawing.Size(111, 18);
		this.label140.TabIndex = 30;
		this.label140.Text = "Invoice's Info";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(908, 1);
		this.ctrlGrdBar2.MyGrid = this.grd;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(39, 22);
		this.ctrlGrdBar2.TabIndex = 27;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel2.Controls.Add(this.groupBox4);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 186);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(950, 66);
		this.panel2.TabIndex = 0;
		this.groupBox4.Controls.Add(this.label139);
		this.groupBox4.Controls.Add(this.txtRemarksDetail);
		this.groupBox4.Controls.Add(this.btnAdd);
		this.groupBox4.Controls.Add(this.btnUpdateDetail);
		this.groupBox4.Controls.Add(this.btnCancelDetail);
		this.groupBox4.Controls.Add(this.txtInvoiceAmountDetail);
		this.groupBox4.Controls.Add(this.label138);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceNoDetail);
		this.groupBox4.Controls.Add(this.label137);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.groupBox4.Location = new System.Drawing.Point(4, 3);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(910, 60);
		this.groupBox4.TabIndex = 9;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Detail Information";
		this.label139.AutoSize = true;
		this.label139.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label139.Location = new System.Drawing.Point(326, 15);
		this.label139.Name = "label139";
		this.label139.Size = new System.Drawing.Size(52, 15);
		this.label139.TabIndex = 639;
		this.label139.Text = "Remarks";
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(327, 31);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(450, 23);
		this.txtRemarksDetail.TabIndex = 2;
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatAppearance.BorderSize = 0;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(779, 29);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(31, 26);
		this.btnAdd.TabIndex = 3;
		this.btnAdd.Text = "+";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(779, 29);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(63, 26);
		this.btnUpdateDetail.TabIndex = 4;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.btnCancelDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelDetail.FlatAppearance.BorderSize = 0;
		this.btnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetail.Location = new System.Drawing.Point(843, 29);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(59, 26);
		this.btnCancelDetail.TabIndex = 5;
		this.btnCancelDetail.Text = "Cancel";
		this.btnCancelDetail.UseVisualStyleBackColor = false;
		this.btnCancelDetail.Visible = false;
		this.btnCancelDetail.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.txtInvoiceAmountDetail.Enabled = false;
		this.txtInvoiceAmountDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceAmountDetail.Location = new System.Drawing.Point(206, 31);
		this.txtInvoiceAmountDetail.Name = "txtInvoiceAmountDetail";
		this.txtInvoiceAmountDetail.Size = new System.Drawing.Size(120, 23);
		this.txtInvoiceAmountDetail.TabIndex = 1;
		this.txtInvoiceAmountDetail.Text = "0";
		this.txtInvoiceAmountDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label138.AutoSize = true;
		this.label138.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label138.Location = new System.Drawing.Point(206, 15);
		this.label138.Name = "label138";
		this.label138.Size = new System.Drawing.Size(93, 15);
		this.label138.TabIndex = 640;
		this.label138.Text = "Invoice Amount";
		this.CmbInvoiceNoDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceNoDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceNoDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceNoDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance39;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance44).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInvoiceNoDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoDetail).Location = new System.Drawing.Point(4, 29);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoDetail).Name = "CmbInvoiceNoDetail";
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoDetail).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoDetail).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoDetail).TextChanged += new System.EventHandler(CmbInvoiceNoDetail_TextChanged);
		this.label137.AutoSize = true;
		this.label137.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label137.Location = new System.Drawing.Point(4, 15);
		this.label137.Name = "label137";
		this.label137.Size = new System.Drawing.Size(65, 15);
		this.label137.TabIndex = 597;
		this.label137.Text = "Invoice No";
		this.panel4.Controls.Add(this.groupBox3);
		this.panel4.Controls.Add(this.txtApprovalRemarks);
		this.panel4.Controls.Add(this.label44);
		this.panel4.Controls.Add(this.label50);
		this.panel4.Controls.Add(this.txtDeliveryRemarks);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 58);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(950, 128);
		this.panel4.TabIndex = 1;
		this.groupBox3.Controls.Add(this.txtRemarksMultiInvoiceForm);
		this.groupBox3.Controls.Add(this.label130);
		this.groupBox3.Controls.Add(this.FttAmountMultiInvoiceForm);
		this.groupBox3.Controls.Add(this.label131);
		this.groupBox3.Controls.Add(this.txtNetAmountMultiInvoiceForm);
		this.groupBox3.Controls.Add(this.label132);
		this.groupBox3.Controls.Add(this.FttPrcntMultiInvoiceForm);
		this.groupBox3.Controls.Add(this.label133);
		this.groupBox3.Controls.Add(this.txtFobValueMultiInvoiceForm);
		this.groupBox3.Controls.Add(this.label134);
		this.groupBox3.Controls.Add(this.txtFreightMultiInvoiceForm);
		this.groupBox3.Controls.Add(this.label135);
		this.groupBox3.Controls.Add(this.txtGdValueMultiInvoiceForm);
		this.groupBox3.Controls.Add(this.label136);
		this.groupBox3.Controls.Add(this.txtExchangeRateMultiInvoiceForm);
		this.groupBox3.Controls.Add(this.label123);
		this.groupBox3.Controls.Add(this.txtDueDaysMultiInvoiceForm);
		this.groupBox3.Controls.Add(this.label124);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBankMultiInvoiceForm);
		this.groupBox3.Controls.Add(this.label125);
		this.groupBox3.Controls.Add(this.GdNoMultiInvoiceForm);
		this.groupBox3.Controls.Add(this.label126);
		this.groupBox3.Controls.Add(this.BankInvoiceNoMultiInvoiceForm);
		this.groupBox3.Controls.Add(this.label127);
		this.groupBox3.Controls.Add(this.GdDateMultiInvoiceForm);
		this.groupBox3.Controls.Add(this.BankInvoiceDateMultiInvoiceForm);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(3, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(911, 124);
		this.groupBox3.TabIndex = 571;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Header Information";
		this.txtRemarksMultiInvoiceForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksMultiInvoiceForm.Location = new System.Drawing.Point(79, 95);
		this.txtRemarksMultiInvoiceForm.Name = "txtRemarksMultiInvoiceForm";
		this.txtRemarksMultiInvoiceForm.Size = new System.Drawing.Size(624, 23);
		this.txtRemarksMultiInvoiceForm.TabIndex = 13;
		this.label130.AutoSize = true;
		this.label130.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label130.Location = new System.Drawing.Point(3, 101);
		this.label130.Name = "label130";
		this.label130.Size = new System.Drawing.Size(52, 15);
		this.label130.TabIndex = 638;
		this.label130.Text = "Remarks";
		this.FttAmountMultiInvoiceForm.Enabled = false;
		this.FttAmountMultiInvoiceForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FttAmountMultiInvoiceForm.Location = new System.Drawing.Point(785, 17);
		this.FttAmountMultiInvoiceForm.Name = "FttAmountMultiInvoiceForm";
		this.FttAmountMultiInvoiceForm.ShortcutsEnabled = false;
		this.FttAmountMultiInvoiceForm.Size = new System.Drawing.Size(120, 23);
		this.FttAmountMultiInvoiceForm.TabIndex = 11;
		this.FttAmountMultiInvoiceForm.Text = "0";
		this.FttAmountMultiInvoiceForm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.FttAmountMultiInvoiceForm.TextChanged += new System.EventHandler(FttAmountMultiInvoiceForm_TextChanged);
		this.label131.AutoSize = true;
		this.label131.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label131.Location = new System.Drawing.Point(706, 21);
		this.label131.Name = "label131";
		this.label131.Size = new System.Drawing.Size(74, 15);
		this.label131.TabIndex = 637;
		this.label131.Text = "FTT Amount";
		this.txtNetAmountMultiInvoiceForm.Enabled = false;
		this.txtNetAmountMultiInvoiceForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetAmountMultiInvoiceForm.Location = new System.Drawing.Point(785, 42);
		this.txtNetAmountMultiInvoiceForm.Name = "txtNetAmountMultiInvoiceForm";
		this.txtNetAmountMultiInvoiceForm.ShortcutsEnabled = false;
		this.txtNetAmountMultiInvoiceForm.Size = new System.Drawing.Size(120, 23);
		this.txtNetAmountMultiInvoiceForm.TabIndex = 12;
		this.txtNetAmountMultiInvoiceForm.Text = "0";
		this.txtNetAmountMultiInvoiceForm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label132.AutoSize = true;
		this.label132.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label132.Location = new System.Drawing.Point(706, 46);
		this.label132.Name = "label132";
		this.label132.Size = new System.Drawing.Size(73, 15);
		this.label132.TabIndex = 636;
		this.label132.Text = "Net Amount";
		this.FttPrcntMultiInvoiceForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FttPrcntMultiInvoiceForm.Location = new System.Drawing.Point(588, 69);
		this.FttPrcntMultiInvoiceForm.Name = "FttPrcntMultiInvoiceForm";
		this.FttPrcntMultiInvoiceForm.ShortcutsEnabled = false;
		this.FttPrcntMultiInvoiceForm.Size = new System.Drawing.Size(115, 23);
		this.FttPrcntMultiInvoiceForm.TabIndex = 10;
		this.FttPrcntMultiInvoiceForm.Text = "10";
		this.FttPrcntMultiInvoiceForm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.FttPrcntMultiInvoiceForm.TextChanged += new System.EventHandler(FttPrcntMultiInvoiceForm_TextChanged);
		this.label133.AutoSize = true;
		this.label133.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label133.Location = new System.Drawing.Point(527, 73);
		this.label133.Name = "label133";
		this.label133.Size = new System.Drawing.Size(40, 15);
		this.label133.TabIndex = 635;
		this.label133.Text = "% FTT";
		this.txtFobValueMultiInvoiceForm.Enabled = false;
		this.txtFobValueMultiInvoiceForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFobValueMultiInvoiceForm.Location = new System.Drawing.Point(588, 42);
		this.txtFobValueMultiInvoiceForm.Name = "txtFobValueMultiInvoiceForm";
		this.txtFobValueMultiInvoiceForm.ShortcutsEnabled = false;
		this.txtFobValueMultiInvoiceForm.Size = new System.Drawing.Size(115, 23);
		this.txtFobValueMultiInvoiceForm.TabIndex = 9;
		this.txtFobValueMultiInvoiceForm.Text = "0";
		this.txtFobValueMultiInvoiceForm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFobValueMultiInvoiceForm.TextChanged += new System.EventHandler(txtFobValueMultiInvoiceForm_TextChanged);
		this.label134.AutoSize = true;
		this.label134.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label134.Location = new System.Drawing.Point(527, 46);
		this.label134.Name = "label134";
		this.label134.Size = new System.Drawing.Size(61, 15);
		this.label134.TabIndex = 634;
		this.label134.Text = "FOB Value";
		this.txtFreightMultiInvoiceForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreightMultiInvoiceForm.Location = new System.Drawing.Point(588, 17);
		this.txtFreightMultiInvoiceForm.Name = "txtFreightMultiInvoiceForm";
		this.txtFreightMultiInvoiceForm.ShortcutsEnabled = false;
		this.txtFreightMultiInvoiceForm.Size = new System.Drawing.Size(115, 23);
		this.txtFreightMultiInvoiceForm.TabIndex = 8;
		this.txtFreightMultiInvoiceForm.Text = "0";
		this.txtFreightMultiInvoiceForm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFreightMultiInvoiceForm.TextChanged += new System.EventHandler(txtFreightMultiInvoiceForm_TextChanged);
		this.label135.AutoSize = true;
		this.label135.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label135.Location = new System.Drawing.Point(527, 21);
		this.label135.Name = "label135";
		this.label135.Size = new System.Drawing.Size(44, 15);
		this.label135.TabIndex = 633;
		this.label135.Text = "Freight";
		this.txtGdValueMultiInvoiceForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGdValueMultiInvoiceForm.Location = new System.Drawing.Point(408, 69);
		this.txtGdValueMultiInvoiceForm.Name = "txtGdValueMultiInvoiceForm";
		this.txtGdValueMultiInvoiceForm.ShortcutsEnabled = false;
		this.txtGdValueMultiInvoiceForm.Size = new System.Drawing.Size(115, 23);
		this.txtGdValueMultiInvoiceForm.TabIndex = 7;
		this.txtGdValueMultiInvoiceForm.Text = "0";
		this.txtGdValueMultiInvoiceForm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGdValueMultiInvoiceForm.TextChanged += new System.EventHandler(txtGdValueMultiInvoiceForm_TextChanged);
		this.label136.AutoSize = true;
		this.label136.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label136.Location = new System.Drawing.Point(326, 73);
		this.label136.Name = "label136";
		this.label136.Size = new System.Drawing.Size(56, 15);
		this.label136.TabIndex = 632;
		this.label136.Text = "GD Value";
		this.txtExchangeRateMultiInvoiceForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExchangeRateMultiInvoiceForm.Location = new System.Drawing.Point(408, 42);
		this.txtExchangeRateMultiInvoiceForm.Name = "txtExchangeRateMultiInvoiceForm";
		this.txtExchangeRateMultiInvoiceForm.ShortcutsEnabled = false;
		this.txtExchangeRateMultiInvoiceForm.Size = new System.Drawing.Size(115, 23);
		this.txtExchangeRateMultiInvoiceForm.TabIndex = 6;
		this.txtExchangeRateMultiInvoiceForm.Text = "0";
		this.txtExchangeRateMultiInvoiceForm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label123.AutoSize = true;
		this.label123.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label123.Location = new System.Drawing.Point(326, 46);
		this.label123.Name = "label123";
		this.label123.Size = new System.Drawing.Size(84, 15);
		this.label123.TabIndex = 624;
		this.label123.Text = "Exchange Rate";
		this.txtDueDaysMultiInvoiceForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDueDaysMultiInvoiceForm.Location = new System.Drawing.Point(408, 17);
		this.txtDueDaysMultiInvoiceForm.Name = "txtDueDaysMultiInvoiceForm";
		this.txtDueDaysMultiInvoiceForm.ShortcutsEnabled = false;
		this.txtDueDaysMultiInvoiceForm.Size = new System.Drawing.Size(115, 23);
		this.txtDueDaysMultiInvoiceForm.TabIndex = 5;
		this.txtDueDaysMultiInvoiceForm.Text = "0";
		this.txtDueDaysMultiInvoiceForm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label124.AutoSize = true;
		this.label124.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label124.Location = new System.Drawing.Point(326, 21);
		this.label124.Name = "label124";
		this.label124.Size = new System.Drawing.Size(58, 15);
		this.label124.TabIndex = 623;
		this.label124.Text = "Due Days";
		this.CmbBankMultiInvoiceForm.AllowNull = (DefaultableBoolean)1;
		this.CmbBankMultiInvoiceForm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBankMultiInvoiceForm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBankMultiInvoiceForm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBankMultiInvoiceForm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBankMultiInvoiceForm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBankMultiInvoiceForm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBankMultiInvoiceForm).Location = new System.Drawing.Point(79, 67);
		((System.Windows.Forms.Control)(object)this.CmbBankMultiInvoiceForm).Name = "CmbBankMultiInvoiceForm";
		((System.Windows.Forms.Control)(object)this.CmbBankMultiInvoiceForm).Size = new System.Drawing.Size(241, 26);
		((System.Windows.Forms.Control)(object)this.CmbBankMultiInvoiceForm).TabIndex = 4;
		this.label125.AutoSize = true;
		this.label125.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label125.Location = new System.Drawing.Point(3, 73);
		this.label125.Name = "label125";
		this.label125.Size = new System.Drawing.Size(68, 15);
		this.label125.TabIndex = 622;
		this.label125.Text = "Bank Name";
		this.GdNoMultiInvoiceForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GdNoMultiInvoiceForm.Location = new System.Drawing.Point(79, 17);
		this.GdNoMultiInvoiceForm.Name = "GdNoMultiInvoiceForm";
		this.GdNoMultiInvoiceForm.Size = new System.Drawing.Size(130, 23);
		this.GdNoMultiInvoiceForm.TabIndex = 0;
		this.label126.AutoSize = true;
		this.label126.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label126.Location = new System.Drawing.Point(3, 21);
		this.label126.Name = "label126";
		this.label126.Size = new System.Drawing.Size(71, 15);
		this.label126.TabIndex = 618;
		this.label126.Text = "GD No,Date";
		this.BankInvoiceNoMultiInvoiceForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BankInvoiceNoMultiInvoiceForm.Location = new System.Drawing.Point(79, 42);
		this.BankInvoiceNoMultiInvoiceForm.Name = "BankInvoiceNoMultiInvoiceForm";
		this.BankInvoiceNoMultiInvoiceForm.Size = new System.Drawing.Size(130, 23);
		this.BankInvoiceNoMultiInvoiceForm.TabIndex = 2;
		this.label127.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label127.Location = new System.Drawing.Point(3, 38);
		this.label127.Name = "label127";
		this.label127.Size = new System.Drawing.Size(75, 30);
		this.label127.TabIndex = 619;
		this.label127.Text = "Bank Invoice No && Date";
		this.GdDateMultiInvoiceForm.CustomFormat = "dd-MMM-yy";
		this.GdDateMultiInvoiceForm.Font = new System.Drawing.Font("Verdana", 9f);
		this.GdDateMultiInvoiceForm.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.GdDateMultiInvoiceForm.Location = new System.Drawing.Point(210, 17);
		this.GdDateMultiInvoiceForm.Name = "GdDateMultiInvoiceForm";
		this.GdDateMultiInvoiceForm.Size = new System.Drawing.Size(110, 22);
		this.GdDateMultiInvoiceForm.TabIndex = 1;
		this.BankInvoiceDateMultiInvoiceForm.CustomFormat = "dd-MMM-yy";
		this.BankInvoiceDateMultiInvoiceForm.Font = new System.Drawing.Font("Verdana", 9f);
		this.BankInvoiceDateMultiInvoiceForm.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.BankInvoiceDateMultiInvoiceForm.Location = new System.Drawing.Point(210, 42);
		this.BankInvoiceDateMultiInvoiceForm.Name = "BankInvoiceDateMultiInvoiceForm";
		this.BankInvoiceDateMultiInvoiceForm.Size = new System.Drawing.Size(110, 22);
		this.BankInvoiceDateMultiInvoiceForm.TabIndex = 3;
		this.txtApprovalRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtApprovalRemarks.Location = new System.Drawing.Point(1167, 370);
		this.txtApprovalRemarks.Name = "txtApprovalRemarks";
		this.txtApprovalRemarks.Size = new System.Drawing.Size(42, 23);
		this.txtApprovalRemarks.TabIndex = 20;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(1074, 374);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(52, 15);
		this.label44.TabIndex = 570;
		this.label44.Text = "Remarks";
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(1074, 401);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(52, 15);
		this.label50.TabIndex = 570;
		this.label50.Text = "Remarks";
		this.txtDeliveryRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDeliveryRemarks.Location = new System.Drawing.Point(1167, 397);
		this.txtDeliveryRemarks.Name = "txtDeliveryRemarks";
		this.txtDeliveryRemarks.Size = new System.Drawing.Size(42, 23);
		this.txtDeliveryRemarks.TabIndex = 22;
		this.panel28.BackColor = System.Drawing.Color.Teal;
		this.panel28.Controls.Add(this.label122);
		this.panel28.Controls.Add(this.ChkPreview);
		this.panel28.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel28.Location = new System.Drawing.Point(3, 32);
		this.panel28.Margin = new System.Windows.Forms.Padding(2);
		this.panel28.Name = "panel28";
		this.panel28.Size = new System.Drawing.Size(950, 26);
		this.panel28.TabIndex = 1;
		this.label122.AutoSize = true;
		this.label122.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label122.ForeColor = System.Drawing.Color.White;
		this.label122.Location = new System.Drawing.Point(3, 4);
		this.label122.Name = "label122";
		this.label122.Size = new System.Drawing.Size(227, 18);
		this.label122.TabIndex = 30;
		this.label122.Text = "Multi Invoices Allocate To GD";
		this.ChkPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPreview.AutoSize = true;
		this.ChkPreview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPreview.ForeColor = System.Drawing.Color.White;
		this.ChkPreview.Location = new System.Drawing.Point(863, 3);
		this.ChkPreview.Name = "ChkPreview";
		this.ChkPreview.Size = new System.Drawing.Size(84, 20);
		this.ChkPreview.TabIndex = 1;
		this.ChkPreview.Text = "Preview";
		this.ChkPreview.UseVisualStyleBackColor = true;
		this.panel3.Controls.Add(this.toolstrip);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(950, 29);
		this.panel3.TabIndex = 0;
		this.toolstrip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolstrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnnew, this.BtnRefresh, this.BtnSaveAs, this.btnsave, this.btnUpdate, this.BtnDelete, this.btnAttachment, this.Print, this.btnShortCutKey });
		this.toolstrip.Location = new System.Drawing.Point(0, 0);
		this.toolstrip.Name = "toolstrip";
		this.toolstrip.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
		this.toolstrip.Size = new System.Drawing.Size(950, 27);
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
		this.BtnSaveAs.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnSaveAs.Image = (System.Drawing.Image)resources.GetObject("BtnSaveAs.Image");
		this.BtnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSaveAs.Name = "BtnSaveAs";
		this.BtnSaveAs.Size = new System.Drawing.Size(85, 24);
		this.BtnSaveAs.Text = "&Save As";
		this.BtnSaveAs.Visible = false;
		this.BtnSaveAs.Click += new System.EventHandler(BtnSaveAs_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.BtnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.BtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnDelete.Name = "BtnDelete";
		this.BtnDelete.Size = new System.Drawing.Size(73, 24);
		this.BtnDelete.Text = "Delete";
		this.BtnDelete.Visible = false;
		this.BtnDelete.Click += new System.EventHandler(BtnDelete_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.Print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(84, 24);
		this.Print.Text = "02-Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnShortCutKey.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel13);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grddetailhistory);
		this.tabPage2.Controls.Add(this.panel29);
		this.tabPage2.Controls.Add(this.panel18);
		this.tabPage2.Controls.Add(this.panel17);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(956, 532);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.DataGridHistory).BackColor = System.Drawing.Color.White;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 125);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(950, 182);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label59);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel13.Location = new System.Drawing.Point(3, 307);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(950, 25);
		this.panel13.TabIndex = 346;
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label59.ForeColor = System.Drawing.Color.White;
		this.label59.Location = new System.Drawing.Point(5, 5);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(228, 17);
		this.label59.TabIndex = 562;
		this.label59.Text = "Detail of the selected above row";
		this.grddetailhistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grddetailhistory).BackColor = System.Drawing.Color.White;
		this.grddetailhistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grddetailhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grddetailhistory.DesignTimeLayout = grddetailhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grddetailhistory).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grddetailhistory.FilterMode = (FilterMode)2;
		this.grddetailhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grddetailhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grddetailhistory.GroupByBoxVisible = false;
		this.grddetailhistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grddetailhistory).Location = new System.Drawing.Point(3, 332);
		((System.Windows.Forms.Control)(object)this.grddetailhistory).Name = "grddetailhistory";
		((ScrollableBase)this.grddetailhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grddetailhistory).Size = new System.Drawing.Size(950, 197);
		((System.Windows.Forms.Control)(object)this.grddetailhistory).TabIndex = 1;
		this.grddetailhistory.TotalRow = (InheritableBoolean)1;
		this.grddetailhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grddetailhistory).VisualStyle = (VisualStyle)4;
		this.panel29.BackColor = System.Drawing.Color.Teal;
		this.panel29.Controls.Add(this.label65);
		this.panel29.Controls.Add(this.ctrlGrdBar1);
		this.panel29.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel29.Location = new System.Drawing.Point(3, 101);
		this.panel29.Name = "panel29";
		this.panel29.Size = new System.Drawing.Size(950, 24);
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
		this.ctrlGrdBar1.Location = new System.Drawing.Point(899, 0);
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
		this.panel18.Size = new System.Drawing.Size(950, 45);
		this.panel18.TabIndex = 0;
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBankMultiInvoiceAgainstGdHistory);
		this.groupBox5.Controls.Add(this.label142);
		this.groupBox5.Controls.Add(this.btnShowHistory);
		this.groupBox5.Controls.Add(this.rdmodifydate);
		this.groupBox5.Controls.Add(this.drdocdate);
		this.groupBox5.Controls.Add(this.rdentrydate);
		this.groupBox5.Controls.Add(this.label91);
		this.groupBox5.Controls.Add(this.ToDateHistory);
		this.groupBox5.Controls.Add(this.FromDateHistory);
		this.groupBox5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox5.Location = new System.Drawing.Point(4, 0);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(943, 43);
		this.groupBox5.TabIndex = 0;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Filters";
		this.CmbBankMultiInvoiceAgainstGdHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbBankMultiInvoiceAgainstGdHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBankMultiInvoiceAgainstGdHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBankMultiInvoiceAgainstGdHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBankMultiInvoiceAgainstGdHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBankMultiInvoiceAgainstGdHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBankMultiInvoiceAgainstGdHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBankMultiInvoiceAgainstGdHistory).Location = new System.Drawing.Point(408, 13);
		((System.Windows.Forms.Control)(object)this.CmbBankMultiInvoiceAgainstGdHistory).Name = "CmbBankMultiInvoiceAgainstGdHistory";
		((System.Windows.Forms.Control)(object)this.CmbBankMultiInvoiceAgainstGdHistory).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbBankMultiInvoiceAgainstGdHistory).TabIndex = 4;
		this.label142.AutoSize = true;
		this.label142.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label142.Location = new System.Drawing.Point(337, 19);
		this.label142.Name = "label142";
		this.label142.Size = new System.Drawing.Size(68, 15);
		this.label142.TabIndex = 88;
		this.label142.Text = "Bank Name";
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(611, 13);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 5;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(834, 17);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(678, 17);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(755, 17);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.label91.AutoSize = true;
		this.label91.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label91.Location = new System.Drawing.Point(3, 19);
		this.label91.Name = "label91";
		this.label91.Size = new System.Drawing.Size(79, 15);
		this.label91.TabIndex = 83;
		this.label91.Text = "From,To Date";
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(208, 15);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(125, 22);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(82, 15);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(125, 22);
		this.FromDateHistory.TabIndex = 0;
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.label52);
		this.panel17.Controls.Add(this.toolStrip3);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(3, 3);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(950, 53);
		this.panel17.TabIndex = 0;
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.ForeColor = System.Drawing.Color.White;
		this.label52.Location = new System.Drawing.Point(5, 30);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(312, 19);
		this.label52.TabIndex = 562;
		this.label52.Text = "Multi Invoices Allocate To GD History";
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnResetHistory, this.btnRefreshHistory });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
		this.toolStrip3.Size = new System.Drawing.Size(950, 27);
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
		base.ClientSize = new System.Drawing.Size(964, 561);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmMultiInvoicesAllocateToGdn";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmMultiInvoicesAllocateToGdn_FormClosing);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmMultiInvoicesAllocateToGdn_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel38.ResumeLayout(false);
		this.panel38.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNoDetail).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBankMultiInvoiceForm).EndInit();
		this.panel28.ResumeLayout(false);
		this.panel28.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolstrip.ResumeLayout(false);
		this.toolstrip.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grddetailhistory).EndInit();
		this.panel29.ResumeLayout(false);
		this.panel29.PerformLayout();
		this.panel18.ResumeLayout(false);
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBankMultiInvoiceAgainstGdHistory).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
