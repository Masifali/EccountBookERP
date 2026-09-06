using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Accounts;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Definition;

public class AcfrmDefPdcManagment : Form
{
	private bool DoHaveSaveRight;

	private bool DoHaveUpdateRights;

	private bool DoHaveCanViewAllRecordRights;

	private bool DoHaveDeleteRights;

	private DataTable dtvoucher = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	public UserAccount UserAccount = new UserAccount();

	private ChartofAccount coa = new ChartofAccount();

	private int RecId;

	private int VoucherHeadId;

	private int updateDetailIndex;

	private DataTable table = new DataTable();

	private DataTable dtAccounts = new DataTable();

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel1;

	private TabPage tabPage2;

	private ContextMenuStrip contextMenuStrip1;

	private PrintDialog printDialog1;

	internal GridEX grdfrm;

	private Panel panel5;

	private GroupBox groupBox1;

	private Button btnCancel;

	private Button btnAdd;

	private Label label1;

	private TextBox txtComments;

	internal UltraCombo cmbbank;

	internal UltraCombo cmbcheqstatus;

	private Button button1;

	private Label label6;

	private Label label8;

	private Label label5;

	private TextBox txtcheqno;

	private TextBox txtcheqamount;

	private DateTimePicker txtcheqdate;

	private Label label19;

	private Label label9;

	internal UltraCombo cmbcheqtype;

	internal UltraCombo CmbAccountTitleDebit;

	internal UltraCombo CmbAccountTitleCredit;

	private CheckBox chkapproved;

	private DateTimePicker txtdocdate;

	private TextBox txtbranchcode;

	private TextBox txtremarks;

	private Label label13;

	private TextBox txtdocno;

	private Label label7;

	private Label label3;

	private Label label11;

	private Label label10;

	private Label label4;

	private Label label2;

	private Label label40;

	private Panel panel4;

	private Label lblCaption;

	private Panel panel2;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnPrint;

	private ToolStripButton toolStripButton7;

	private ToolStripButton toolStripButton8;

	private ToolStripButton toolStripButton1;

	private Panel panel3;

	internal GridEX grdHistory;

	private ToolStripButton btnDelete;

	private Panel panel6;

	private ToolStrip toolStrip2;

	private ToolStripButton toolStripButton10;

	internal GridEX grdReprot;

	private Label label12;

	internal UltraCombo CmbChequeStatusReprot;

	private CheckBox ChkCheqToDate;

	private CheckBox ChkCheqFromdate;

	private CheckBox ChkTodate;

	private CheckBox ChkFromdate;

	private DateTimePicker datCheqToDate;

	private DateTimePicker datCheqFromDate;

	private DateTimePicker datToDate;

	private DateTimePicker datFromDate;

	private Button btnshow;

	internal UltraCombo CmbAccountTitle;

	private Label label14;

	private Label label30;

	private Label label15;

	private Label label34;

	private Label label16;

	private ToolStripButton btnRegister;

	private ToolStripButton btnPdcSummery;

	private ToolStripButton btnPdcRegisterGuriList;

	internal UltraCombo cmbDebitAccountrpt;

	private Label label17;

	private CtrlGrdBar ctrlGrdBar2;

	private CheckBox ChkPrint2;

	private CheckBox ChkPrint1;

	public AcfrmDefPdcManagment(UserAccount puserAccount)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = puserAccount;
	}

	private void bankfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtbnk = PdcBankName.Getall(new PdcBankName
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtbnk.Rows.Count > 0)
			{
				DDL.BindDDL(dtbnk, cmbbank, "Id", "BankName", "BankName", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumentNoFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		try
		{
			int Code = PdcInventory.GetGenerateCode(new PdcInventoryHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 16,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (Code > 0)
			{
				txtdocno.Text = Code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CheqStatusFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "Pending");
			dt.Rows.Add(2, "Clear");
			dt.Rows.Add(3, "Cancel");
			dt.Rows.Add(4, "Return");
			DDL.BindDDL(dt, cmbcheqstatus, "Id", "Name", "Name", ZeroIndex: false);
			((UltraGridBase)cmbcheqstatus).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CheqStatusReprots()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "Pending");
			dt.Rows.Add(2, "Clear");
			dt.Rows.Add(3, "All");
			DDL.BindDDL(dt, CmbChequeStatusReprot, "Id", "Status", "Cheque Status", ZeroIndex: false);
			((UltraGridBase)CmbChequeStatusReprot).DisplayLayout.Bands[0].Columns[1].Width = 220;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountsFill()
	{
		try
		{
			dtAccounts = CommonServices.CoaAllocationGetAllServiceBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CreditAccountFill()
	{
		try
		{
			if (dtAccounts.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("AccountTitle");
			for (int i = 0; i < dtAccounts.Rows.Count; i++)
			{
				if (Convert.ToInt32(dtAccounts.Rows[i]["AccountTypeId"]) != 2 && Convert.ToInt32(dtAccounts.Rows[i]["AccountTypeId"]) != 11 && Convert.ToInt32(dtAccounts.Rows[i]["AccountTypeId"]) != 15)
				{
					dt.Rows.Add(dtAccounts.Rows[i]["Id"], dtAccounts.Rows[i]["AccountTitle"]);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbAccountTitleCredit, "Id", "AccountTitle", "Account", ZeroIndex: false);
				DDL.BindDDL(dt, CmbAccountTitleDebit, "Id", "AccountTitle", "Account", ZeroIndex: false);
				DDL.BindDDL(dt, CmbAccountTitle, "Id", "AccountTitle", "Account", ZeroIndex: false);
				DDL.BindDDL(dt, cmbDebitAccountrpt, "Id", "AccountTitle", "Account", ZeroIndex: false);
				((UltraGridBase)CmbAccountTitle).DisplayLayout.Bands[0].Columns[1].Width = 320;
				((UltraGridBase)cmbDebitAccountrpt).DisplayLayout.Bands[0].Columns[1].Width = 320;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formvalidation()
	{
		if (txtdocno.Text.Trim() == "" || txtdocno.Text.Trim() == "0")
		{
			MessageBox.Show("Doc No field required");
			txtdocno.Focus();
			return false;
		}
		if (((UltraGridBase)CmbAccountTitleCredit).ActiveRow == null)
		{
			MessageBox.Show("Please Select Party Account");
			CmbAccountTitleCredit.Focus();
			return false;
		}
		if (((UltraGridBase)CmbAccountTitleDebit).ActiveRow == null)
		{
			MessageBox.Show("Please Select Debit Account");
			CmbAccountTitleDebit.Focus();
			return false;
		}
		return true;
	}

	private bool FormvalidationDetail()
	{
		if (((UltraGridBase)cmbbank).ActiveRow == null)
		{
			MessageBox.Show("Bank Field Required");
			cmbbank.Focus();
			return false;
		}
		if (txtcheqno.Text.Trim() == "" || txtcheqno.Text.Trim() == "0")
		{
			MessageBox.Show("Cheq No Field Required");
			txtcheqno.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtcheqamount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("CheqAmount Field Required");
			txtcheqamount.Focus();
			return false;
		}
		return true;
	}

	private void AcfrmDefPdcManagment_Load(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			List<tblUserRights> lstRights = tblUserRights.GetByUserId(new tblUserRights
			{
				UserId = UserAccount.ID,
				ScreenName = base.Name,
				RightName = UserAccount.RoleName
			});
			if (lstRights.Count > 0)
			{
				List<tblUserRights> newList = lstRights.Where((tblUserRights x) => x.RightName == "Save").ToList();
				DoHaveSaveRight = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Update").ToList();
				DoHaveUpdateRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "CanView AllRecord").ToList();
				DoHaveCanViewAllRecordRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Delete").ToList();
				DoHaveDeleteRights = Conversion.ToBool((object)newList[0].Value.ToString());
			}
			btnsave.Enabled = DoHaveSaveRight;
			btnUpdate.Enabled = DoHaveUpdateRights;
			btnDelete.Enabled = DoHaveDeleteRights;
			table.Columns.Add("Id");
			table.Columns.Add("BankId");
			table.Columns.Add("BankName");
			table.Columns.Add("CheqDate");
			table.Columns.Add("CheqNo");
			table.Columns.Add("CheqAmount", typeof(double));
			table.Columns.Add("Comments");
			table.Columns.Add("ChequeStatus");
			grdfrm.DataSource = table;
			grdfrm.RetrieveStructure();
			dtViewAttachments.Columns.Add("AttchmentPath");
			bankfill();
			AccountsFill();
			CreditAccountFill();
			DocumentNoFill();
			CheqStatusReprots();
			txtdocdate.Focus();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			((UltraGridBase)CmbChequeStatusReprot).Rows[2].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Expected O, but got Unknown
		try
		{
			if (!formvalidation())
			{
				return;
			}
			PdcInventoryHeader obj = new PdcInventoryHeader();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			obj.DocumentTypeId = 16;
			obj.DocDate = txtdocdate.Value;
			obj.DocNo = Conversion.ToInt((object)txtdocno.Text);
			obj.RemarkHeader = txtremarks.Text;
			obj.MGLAccountCrId = Conversion.ToInt(CmbAccountTitleCredit.Value);
			obj.MGLAccountDrId = Conversion.ToInt(CmbAccountTitleDebit.Value);
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.EntryUser = UserAccount.ID;
			obj.ModifyUser = UserAccount.ID;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.PdcInventoriesList = new List<PdcInventory>();
			if (grdfrm.GetRows().Count() == 0)
			{
				MessageBox.Show("grid record not found");
				return;
			}
			GridEXRow[] rows = grdfrm.GetRows();
			foreach (GridEXRow r in rows)
			{
				PdcInventory pdc = new PdcInventory();
				pdc.Id = Conversion.ToInt(r.Cells["Id"].Value);
				pdc.GlCreditAcId = Conversion.ToInt(CmbAccountTitleCredit.Value);
				pdc.GlDebitAcId = Conversion.ToInt(CmbAccountTitleDebit.Value);
				pdc.DocNo = Conversion.ToInt((object)txtdocno.Text);
				pdc.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
				pdc.DocumentTypeId = 16;
				pdc.BankId = Conversion.ToInt(r.Cells["BankId"].Value);
				pdc.BankName = Conversion.ToString((object)r.Cells["BankName"].Text);
				pdc.CheqNo = Conversion.ToString((object)r.Cells["CheqNo"].Text.Trim());
				pdc.CheqAmount = Conversion.ToDouble(r.Cells["CheqAmount"].Value);
				pdc.CheqDate = Conversion.ToDateTime(r.Cells["CheqDate"].Value);
				pdc.CheqStatus = Conversion.ToString(r.Cells["ChequeStatus"].Value);
				pdc.Remarks = Conversion.ToString(r.Cells["Comments"].Value);
				obj.PdcInventoriesList.Add(pdc);
			}
			obj.AttachmentsList = new List<DMSAttachments>();
			obj.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<PdcInventoryHeader>(obj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = PdcInventory.Save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update Successfully");
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					DMSAttachments.RemoveByIdAndNames(RecId, base.Name, 16, 0);
				}
			}
			else
			{
				MessageBox.Show("Record Save Successfully");
			}
			if (ChkPrint1.Checked)
			{
				SlipPrint(success);
			}
			if (ChkPrint2.Checked)
			{
				CommonServices.VoucherReport_118(CommonServices.VoucherHeadIdGet(success, 16));
			}
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		try
		{
			int Id = PdcInventory.GetIdByDocNo(new PdcInventoryHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 16,
				DocNo = Conversion.ToInt((object)txtdocno.Text.Trim()),
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (Id > 0)
			{
				if (RecId == Id)
				{
					Insert();
					return;
				}
				throw new Exception("RecId Not match against this DocNo");
			}
			throw new Exception("Record Not Found For Update");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdfrm.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				cmbbank.Value = item.Cells["BankId"].Value.ToString();
				((Control)(object)cmbbank).Text = item.Cells["BankName"].Value.ToString();
				txtcheqdate.Value = Conversion.ToDateTime(item.Cells["CheqDate"].Value);
				txtcheqno.Text = Conversion.ToString((object)item.Cells["CheqNo"].Text);
				txtcheqamount.Text = Conversion.ToString(item.Cells["CheqAmount"].Value);
				txtComments.Text = Conversion.ToString((object)item.Cells["Comments"].Text);
				btnAdd.Text = "Update";
				btnCancel.Visible = true;
				cmbbank.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int ID)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		try
		{
			RecId = ID;
			PdcInventoryHeader obj = new PdcInventoryHeader();
			obj = PdcInventory.GetByID(RecId);
			if (obj != null)
			{
				tabControl1.SelectedIndex = 0;
				txtdocdate.Value = Conversion.ToDateTime((object)obj.DocDate);
				txtdocno.Text = Conversion.ToString((object)obj.DocNo);
				CmbAccountTitleCredit.Value = obj.MGLAccountCrId;
				CmbAccountTitleDebit.Value = obj.MGLAccountDrId;
				txtremarks.Text = Conversion.ToString((object)obj.RemarkHeader);
				table.Rows.Clear();
				for (int i = 0; i < obj.PdcInventoriesList.Count; i++)
				{
					table.Rows.Add(obj.PdcInventoriesList[i].Id, obj.PdcInventoriesList[i].BankId, obj.PdcInventoriesList[i].BankName, Conversion.ToDateTime((object)obj.PdcInventoriesList[i].CheqDate).ToString("dd-MMM-yy"), obj.PdcInventoriesList[i].CheqNo, obj.PdcInventoriesList[i].CheqAmount, Conversion.ToString((object)obj.PdcInventoriesList[i].Remarks), Conversion.ToString((object)obj.PdcInventoriesList[i].CheqStatus));
				}
				VoucherHeadId = CommonServices.VoucherHeadIdGet(RecId, 16);
				grdfrm.DataSource = table;
				grdfrm.RetrieveStructure();
				gridsetting();
				FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
				btnsave.Visible = false;
				btnUpdate.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
		ResetReport();
		((Control)(object)CmbAccountTitleCredit).Text = string.Empty;
		((Control)(object)CmbAccountTitleDebit).Text = string.Empty;
		((Control)(object)cmbbank).Text = string.Empty;
	}

	private void ResetReport()
	{
		((Control)(object)CmbAccountTitle).Text = string.Empty;
		ChkCheqFromdate.Checked = false;
		ChkCheqToDate.Checked = false;
		ChkFromdate.Checked = false;
		ChkTodate.Checked = false;
		ChkFromdate.Focus();
		grdReprot.DataSource = null;
		grdReprot.RetrieveStructure();
	}

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			RecId = 0;
			VoucherHeadId = 0;
			txtremarks.Text = string.Empty;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnAdd.Text = "Add";
			table.Rows.Clear();
			btnCancel.Visible = false;
			grdfrm.DataSource = table;
			grdfrm.RetrieveStructure();
			DocumentNoFill();
			txtdocdate.Focus();
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
			txtcheqamount.Text = string.Empty;
			txtcheqno.Text = string.Empty;
			txtComments.Text = string.Empty;
			cmbbank.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridsetting()
	{
		try
		{
			grdfrm.AllowEdit = (InheritableBoolean)2;
			grdfrm.RootTable.Columns["Id"].Visible = false;
			grdfrm.RootTable.Columns["BankId"].Visible = false;
			grdfrm.RootTable.Columns["CheqAmount"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["CheqAmount"].FormatString = "#,#";
			grdfrm.RootTable.Columns["CheqAmount"].TotalFormatString = "#,#";
			grdfrm.RootTable.Columns["CheqAmount"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns.Add("Delete");
			grdfrm.RootTable.Columns["Delete"].Caption = "Delete";
			grdfrm.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdfrm.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdfrm.RootTable.Columns["Delete"].Image = Resources.icons8_delete_bin_16;
			grdfrm.RootTable.Columns["Delete"].Width = 35;
			grdfrm.ColumnAutoResize = true;
			ctrlGrdBar1_Load(null, null);
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

	private void toolStripButton7_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.VoucherReport_118(VoucherHeadId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AcfrmDefPdcManagment_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnUpdate_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton8_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void grdfrm_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdfrm.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			List<DMSAttachments> dMSList = DMSAttachments.GetByID(int.Parse(item.Cells["Id"].Value.ToString().ToString()), base.Name);
			dtViewAttachments.Clear();
			if (dMSList.Count > 0)
			{
				foreach (DMSAttachments itemList in dMSList)
				{
					dtViewAttachments.Rows.Add(itemList.Attachment);
				}
			}
			else
			{
				dtViewAttachments.Clear();
			}
			new AttachmentView(dtViewAttachments).Show();
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
			SlipPrint(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SlipPrint(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = GeneralReprots.PdcInventorySlip(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = PrintId,
				DocumentTypeId = 16
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("Record Not Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "123-AcRptPdcInventorySlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		new PdcBank(UserAccount).Show();
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		bankfill();
		AccountsFill();
		CreditAccountFill();
		CheqStatusFill();
	}

	private void txtcheqamount_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtdocno_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (!(((Control)(object)cmbbank).Text != "CASH") || FormvalidationDetail())
			{
				if (btnAdd.Text == "Add")
				{
					table.Rows.Add(0, cmbbank.Value, ((Control)(object)cmbbank).Text, Conversion.ToDateTime((object)txtcheqdate.Value).ToString("dd-MMM-yy"), txtcheqno.Text, txtcheqamount.Text, txtComments.Text, "Pending");
					grdfrm.DataSource = table;
					grdfrm.RetrieveStructure();
				}
				else
				{
					table.Rows[updateDetailIndex]["BankId"] = cmbbank.Value;
					table.Rows[updateDetailIndex]["BankName"] = ((Control)(object)cmbbank).Text;
					table.Rows[updateDetailIndex]["CheqDate"] = txtcheqdate.Value;
					table.Rows[updateDetailIndex]["CheqNo"] = txtcheqno.Text;
					table.Rows[updateDetailIndex]["CheqAmount"] = txtcheqamount.Text;
					table.Rows[updateDetailIndex]["Comments"] = txtComments.Text;
					grdfrm.DataSource = table;
					grdfrm.RetrieveStructure();
					btnAdd.Text = "Add";
					btnCancel.Visible = false;
				}
				gridsetting();
				ResetDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			PdcInventoryHeader obj = new PdcInventoryHeader();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = 16;
			obj.DocNo = Conversion.ToInt((object)txtdocno.Text.Trim());
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			RecId = PdcInventory.GetIdByDocNo(obj);
			if (RecId > 0)
			{
				ReadById(RecId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		btnAdd.Text = "Add";
		btnCancel.Visible = false;
		ResetDetail();
	}

	private void HistoryFill(int NoOfRecords = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = 16;
			obj.NoOfRecords = NoOfRecords;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.CanViewAllRecord = DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			DataTable dt = PdcInventory.FormHistory(obj);
			if (dt.Rows.Count > 0 && dt != null)
			{
				grdHistory.DataSource = dt;
				grdHistory.RetrieveStructure();
				gridHistorysetting();
			}
			else
			{
				grdHistory.ClearStructure();
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
			HistoryFill(50);
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			List<DMSAttachments> dMSList = DMSAttachments.GetByID(int.Parse(item.Cells["Id"].Value.ToString().ToString()), base.Name);
			dtViewAttachments.Clear();
			if (dMSList.Count > 0)
			{
				foreach (DMSAttachments itemList in dMSList)
				{
					dtViewAttachments.Rows.Add(itemList.Attachment);
				}
			}
			else
			{
				dtViewAttachments.Clear();
			}
			new AttachmentView(dtViewAttachments).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridHistorysetting()
	{
		try
		{
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["RecordNo"].Visible = false;
			grdHistory.RootTable.Columns["CreditAccount"].Width = 220;
			grdHistory.RootTable.Columns["DebitAccount"].Width = 220;
			grdHistory.RootTable.Columns["RemarkHeader"].Width = 350;
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton10_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				if (RecId <= 0)
				{
					throw new Exception("Record Not Found For Delete");
				}
				InvPurchaseInvoice.RemoveByID(new InvPurchaseInvoice
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = 16,
					Id = RecId,
					EntryUser = UserAccount.ID
				});
				MessageBox.Show("Delete Record Seccessfully");
				Reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridSettings()
	{
		try
		{
			grdReprot.RootTable.Columns["Id"].Visible = false;
			grdReprot.RootTable.Columns["Remarks"].Visible = false;
			grdReprot.RootTable.Columns["MGLAccountCrId"].Visible = false;
			grdReprot.RootTable.Columns["MGLAccountDrId"].Visible = false;
			grdReprot.RootTable.Columns["AccountTitleDebit"].Visible = false;
			grdReprot.RootTable.Columns["PaidOnDate"].Visible = false;
			grdReprot.RootTable.Columns["CheqDate"].FilterEditType = (FilterEditType)6;
			grdReprot.RootTable.Columns["CheqAmount"].FormatString = "0,0";
			grdReprot.RootTable.Columns["CheqAmount"].TextAlignment = (TextAlignment)3;
			grdReprot.RootTable.Columns["CheqAmount"].AggregateFunction = (AggregateFunction)2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.AccountId = Conversion.ToInt(CmbAccountTitle.Value);
			obj.ChartOfAccountId = Conversion.ToInt(cmbDebitAccountrpt.Value);
			if (ChkFromdate.Checked)
			{
				obj.FromDate = datFromDate.Value;
			}
			if (ChkTodate.Checked)
			{
				obj.ToDate = datToDate.Value;
			}
			if (ChkCheqFromdate.Checked)
			{
				obj.CheqDateFrom = datCheqFromDate.Value;
			}
			if (ChkCheqToDate.Checked)
			{
				obj.CheqDateTo = datCheqToDate.Value;
			}
			obj.Status = ((Control)(object)CmbChequeStatusReprot).Text.Trim();
			dtvoucher = GeneralReprots.PdcReceiptsRegister(obj);
			if (dtvoucher.Rows.Count > 0)
			{
				grdReprot.DataSource = dtvoucher;
				grdReprot.RetrieveStructure();
				GridSettings();
			}
			else
			{
				dtvoucher.Rows.Clear();
				grdReprot.DataSource = dtvoucher;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ShowReport()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtvoucher.Rows.Count > 0)
			{
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtvoucher, "125-PdcInventoryPending.rpt");
				return;
			}
			throw new Exception("Record Not Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRegister_Click(object sender, EventArgs e)
	{
		ShowReport();
	}

	private void btnPdcSummery_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters pdc = new ReportsParameters();
			pdc.OrganizationId = UserAccount.OrganizationId;
			pdc.CompanyId = UserAccount.CompanyId;
			pdc.AccountId = Conversion.ToInt(CmbAccountTitle.Value);
			if (ChkFromdate.Checked)
			{
				pdc.FromDate = datFromDate.Value;
			}
			if (ChkTodate.Checked)
			{
				pdc.ToDate = datToDate.Value;
			}
			if (ChkCheqFromdate.Checked)
			{
				pdc.CheqDateFrom = datCheqFromDate.Value;
			}
			if (ChkCheqToDate.Checked)
			{
				pdc.CheqDateTo = datCheqToDate.Value;
			}
			pdc.Status = ((Control)(object)CmbChequeStatusReprot).Text.Trim();
			DataTable dt = GeneralReprots.PdcReceiptsRegisterSummery(pdc);
			if (dt.Rows.Count > 0)
			{
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "126-PdcInventoryPendingSummery.rpt");
				return;
			}
			throw new Exception("Record Not Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPdcRegisterGuriList_Click(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtvoucher.Rows.Count > 0)
			{
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtvoucher, "127-PdcInventoryPendingGuriWise.rpt");
				return;
			}
			throw new Exception("Record Not Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			RecId = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
			ReadById(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (MessageBox.Show("Are you sure to delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			if (btnsave.Visible && btnsave.Enabled)
			{
				if (e.Column.Key == "Delete")
				{
					GridEXRow item = grdfrm.CurrentRow;
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
				}
			}
			else if (e.Column.Key == "Delete")
			{
				GridEXRow item2 = grdfrm.CurrentRow;
				PdcInventory.PdcCheqDeleteByPdcInventoryId(new PdcInventoryHeader
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Conversion.ToInt(item2.Cells["Id"].Value)
				});
				if (item2 != null && (int)item2.RowType == 0)
				{
					item2.Delete();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistory).Name);
		if (fs != null)
		{
			grdHistory.LoadLayoutFile((Stream)fs);
			fs.Close();
			fs.Dispose();
		}
	}

	private void AcfrmDefPdcManagment_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Expected O, but got Unknown
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Expected O, but got Unknown
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Expected O, but got Unknown
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Expected O, but got Unknown
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Expected O, but got Unknown
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Expected O, but got Unknown
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Expected O, but got Unknown
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Expected O, but got Unknown
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Expected O, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Expected O, but got Unknown
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Expected O, but got Unknown
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Expected O, but got Unknown
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Expected O, but got Unknown
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Expected O, but got Unknown
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Expected O, but got Unknown
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Expected O, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Expected O, but got Unknown
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Expected O, but got Unknown
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Expected O, but got Unknown
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Expected O, but got Unknown
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Expected O, but got Unknown
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Expected O, but got Unknown
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Expected O, but got Unknown
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Expected O, but got Unknown
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Expected O, but got Unknown
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Expected O, but got Unknown
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Expected O, but got Unknown
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Expected O, but got Unknown
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Expected O, but got Unknown
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Expected O, but got Unknown
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Expected O, but got Unknown
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Expected O, but got Unknown
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Expected O, but got Unknown
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Expected O, but got Unknown
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Expected O, but got Unknown
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Expected O, but got Unknown
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Expected O, but got Unknown
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Expected O, but got Unknown
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Expected O, but got Unknown
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Expected O, but got Unknown
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Expected O, but got Unknown
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Expected O, but got Unknown
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Expected O, but got Unknown
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Expected O, but got Unknown
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_0470: Expected O, but got Unknown
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Expected O, but got Unknown
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Expected O, but got Unknown
		//IL_0492: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Expected O, but got Unknown
		//IL_05fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0607: Expected O, but got Unknown
		//IL_0bb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc2: Expected O, but got Unknown
		//IL_5250: Unknown result type (might be due to invalid IL or missing references)
		//IL_525a: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		Appearance appearance1 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
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
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
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
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Definition.AcfrmDefPdcManagment));
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grdReprot = new GridEX();
		this.grdfrm = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.cmbDebitAccountrpt = new UltraCombo();
		this.label17 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.CmbChequeStatusReprot = new UltraCombo();
		this.ChkCheqToDate = new System.Windows.Forms.CheckBox();
		this.ChkCheqFromdate = new System.Windows.Forms.CheckBox();
		this.ChkTodate = new System.Windows.Forms.CheckBox();
		this.ChkFromdate = new System.Windows.Forms.CheckBox();
		this.datCheqToDate = new System.Windows.Forms.DateTimePicker();
		this.datCheqFromDate = new System.Windows.Forms.DateTimePicker();
		this.datToDate = new System.Windows.Forms.DateTimePicker();
		this.datFromDate = new System.Windows.Forms.DateTimePicker();
		this.btnshow = new System.Windows.Forms.Button();
		this.CmbAccountTitle = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.btnCancel = new System.Windows.Forms.Button();
		this.btnAdd = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.txtComments = new System.Windows.Forms.TextBox();
		this.cmbbank = new UltraCombo();
		this.button1 = new System.Windows.Forms.Button();
		this.label6 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.txtcheqno = new System.Windows.Forms.TextBox();
		this.txtcheqamount = new System.Windows.Forms.TextBox();
		this.txtcheqdate = new System.Windows.Forms.DateTimePicker();
		this.label19 = new System.Windows.Forms.Label();
		this.cmbcheqtype = new UltraCombo();
		this.CmbAccountTitleDebit = new UltraCombo();
		this.CmbAccountTitleCredit = new UltraCombo();
		this.chkapproved = new System.Windows.Forms.CheckBox();
		this.cmbcheqstatus = new UltraCombo();
		this.txtdocdate = new System.Windows.Forms.DateTimePicker();
		this.txtbranchcode = new System.Windows.Forms.TextBox();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.lblCaption = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnRegister = new System.Windows.Forms.ToolStripButton();
		this.btnPdcSummery = new System.Windows.Forms.ToolStripButton();
		this.btnPdcRegisterGuriList = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdHistory = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton10 = new System.Windows.Forms.ToolStripButton();
		this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.printDialog1 = new System.Windows.Forms.PrintDialog();
		this.ChkPrint2 = new System.Windows.Forms.CheckBox();
		this.ChkPrint1 = new System.Windows.Forms.CheckBox();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdReprot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDebitAccountrpt).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChequeStatusReprot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitle).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbbank).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcheqtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitleDebit).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitleCredit).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcheqstatus).BeginInit();
		this.panel4.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel6.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1289, 736);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1281, 710);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdReprot);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1275, 704);
		this.panel1.TabIndex = 0;
		this.grdReprot.AllowDelete = (InheritableBoolean)1;
		this.grdReprot.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdReprot).Cursor = System.Windows.Forms.Cursors.Default;
		((System.Windows.Forms.Control)(object)this.grdReprot).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdReprot.FilterMode = (FilterMode)2;
		this.grdReprot.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdReprot).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdReprot.GroupByBoxVisible = false;
		this.grdReprot.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdReprot).Location = new System.Drawing.Point(711, 225);
		((System.Windows.Forms.Control)(object)this.grdReprot).Name = "grdReprot";
		((ScrollableBase)this.grdReprot).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdReprot).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdReprot).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdReprot).Size = new System.Drawing.Size(564, 479);
		((System.Windows.Forms.Control)(object)this.grdReprot).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.grdReprot).TabStop = false;
		this.grdReprot.TotalRow = (InheritableBoolean)1;
		this.grdReprot.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdReprot).VisualStyle = (VisualStyle)4;
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdfrm).Cursor = System.Windows.Forms.Cursors.Default;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Left;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupByBoxVisible = false;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 225);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(711, 479);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.grdfrm).TabStop = false;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.ColumnButtonClick += new ColumnActionEventHandler(grdfrm_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdfrm).DoubleClick += new System.EventHandler(grdfrm_DoubleClick);
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDebitAccountrpt);
		this.panel5.Controls.Add(this.label17);
		this.panel5.Controls.Add(this.label12);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbChequeStatusReprot);
		this.panel5.Controls.Add(this.ChkCheqToDate);
		this.panel5.Controls.Add(this.ChkCheqFromdate);
		this.panel5.Controls.Add(this.ChkTodate);
		this.panel5.Controls.Add(this.ChkFromdate);
		this.panel5.Controls.Add(this.datCheqToDate);
		this.panel5.Controls.Add(this.datCheqFromDate);
		this.panel5.Controls.Add(this.datToDate);
		this.panel5.Controls.Add(this.datFromDate);
		this.panel5.Controls.Add(this.btnshow);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountTitle);
		this.panel5.Controls.Add(this.label14);
		this.panel5.Controls.Add(this.label30);
		this.panel5.Controls.Add(this.label15);
		this.panel5.Controls.Add(this.label34);
		this.panel5.Controls.Add(this.label16);
		this.panel5.Controls.Add(this.groupBox1);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcheqtype);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountTitleDebit);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountTitleCredit);
		this.panel5.Controls.Add(this.chkapproved);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcheqstatus);
		this.panel5.Controls.Add(this.txtdocdate);
		this.panel5.Controls.Add(this.txtbranchcode);
		this.panel5.Controls.Add(this.txtremarks);
		this.panel5.Controls.Add(this.label13);
		this.panel5.Controls.Add(this.txtdocno);
		this.panel5.Controls.Add(this.label7);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add(this.label11);
		this.panel5.Controls.Add(this.label9);
		this.panel5.Controls.Add(this.label10);
		this.panel5.Controls.Add(this.label4);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.label40);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel5.Location = new System.Drawing.Point(0, 64);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1275, 161);
		this.panel5.TabIndex = 2;
		this.cmbDebitAccountrpt.AllowNull = (DefaultableBoolean)1;
		this.cmbDebitAccountrpt.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDebitAccountrpt.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDebitAccountrpt.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDebitAccountrpt).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbDebitAccountrpt).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDebitAccountrpt).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDebitAccountrpt).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDebitAccountrpt).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.cmbDebitAccountrpt).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDebitAccountrpt).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbDebitAccountrpt).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDebitAccountrpt).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDebitAccountrpt).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDebitAccountrpt).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbDebitAccountrpt).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDebitAccountrpt).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDebitAccountrpt).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance3).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDebitAccountrpt).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbDebitAccountrpt).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDebitAccountrpt).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDebitAccountrpt).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDebitAccountrpt).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbDebitAccountrpt).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDebitAccountrpt).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDebitAccountrpt).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbDebitAccountrpt).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDebitAccountrpt).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbDebitAccountrpt).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDebitAccountrpt).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDebitAccountrpt).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDebitAccountrpt).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDebitAccountrpt.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDebitAccountrpt).Location = new System.Drawing.Point(928, 98);
		((System.Windows.Forms.Control)(object)this.cmbDebitAccountrpt).Name = "cmbDebitAccountrpt";
		((System.Windows.Forms.Control)(object)this.cmbDebitAccountrpt).Size = new System.Drawing.Size(339, 26);
		((System.Windows.Forms.Control)(object)this.cmbDebitAccountrpt).TabIndex = 117;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(836, 104);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(93, 15);
		this.label17.TabIndex = 126;
		this.label17.Text = "Debit Ac (Gurai)";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(836, 133);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(70, 15);
		this.label12.TabIndex = 124;
		this.label12.Text = "CheqStatus";
		this.CmbChequeStatusReprot.AllowNull = (DefaultableBoolean)1;
		this.CmbChequeStatusReprot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbChequeStatusReprot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbChequeStatusReprot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbChequeStatusReprot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbChequeStatusReprot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbChequeStatusReprot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbChequeStatusReprot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChequeStatusReprot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbChequeStatusReprot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChequeStatusReprot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbChequeStatusReprot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbChequeStatusReprot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbChequeStatusReprot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbChequeStatusReprot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbChequeStatusReprot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbChequeStatusReprot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChequeStatusReprot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbChequeStatusReprot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbChequeStatusReprot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbChequeStatusReprot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChequeStatusReprot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbChequeStatusReprot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbChequeStatusReprot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbChequeStatusReprot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbChequeStatusReprot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbChequeStatusReprot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbChequeStatusReprot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbChequeStatusReprot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbChequeStatusReprot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbChequeStatusReprot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbChequeStatusReprot).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbChequeStatusReprot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbChequeStatusReprot).Location = new System.Drawing.Point(928, 127);
		((System.Windows.Forms.Control)(object)this.CmbChequeStatusReprot).Name = "CmbChequeStatusReprot";
		((System.Windows.Forms.Control)(object)this.CmbChequeStatusReprot).Size = new System.Drawing.Size(198, 26);
		((System.Windows.Forms.Control)(object)this.CmbChequeStatusReprot).TabIndex = 118;
		this.ChkCheqToDate.AutoSize = true;
		this.ChkCheqToDate.Location = new System.Drawing.Point(1128, 48);
		this.ChkCheqToDate.Name = "ChkCheqToDate";
		this.ChkCheqToDate.Size = new System.Drawing.Size(15, 14);
		this.ChkCheqToDate.TabIndex = 114;
		this.ChkCheqToDate.UseVisualStyleBackColor = true;
		this.ChkCheqFromdate.AutoSize = true;
		this.ChkCheqFromdate.Location = new System.Drawing.Point(904, 50);
		this.ChkCheqFromdate.Name = "ChkCheqFromdate";
		this.ChkCheqFromdate.Size = new System.Drawing.Size(15, 14);
		this.ChkCheqFromdate.TabIndex = 112;
		this.ChkCheqFromdate.UseVisualStyleBackColor = true;
		this.ChkTodate.AutoSize = true;
		this.ChkTodate.Location = new System.Drawing.Point(1126, 22);
		this.ChkTodate.Name = "ChkTodate";
		this.ChkTodate.Size = new System.Drawing.Size(15, 14);
		this.ChkTodate.TabIndex = 110;
		this.ChkTodate.UseVisualStyleBackColor = true;
		this.ChkFromdate.AutoSize = true;
		this.ChkFromdate.Location = new System.Drawing.Point(905, 20);
		this.ChkFromdate.Name = "ChkFromdate";
		this.ChkFromdate.Size = new System.Drawing.Size(15, 14);
		this.ChkFromdate.TabIndex = 108;
		this.ChkFromdate.UseVisualStyleBackColor = true;
		this.datCheqToDate.CustomFormat = "dd-MMM-yyyy";
		this.datCheqToDate.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datCheqToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datCheqToDate.Location = new System.Drawing.Point(1152, 39);
		this.datCheqToDate.Name = "datCheqToDate";
		this.datCheqToDate.Size = new System.Drawing.Size(115, 25);
		this.datCheqToDate.TabIndex = 115;
		this.datCheqFromDate.CustomFormat = "dd-MMM-yyyy";
		this.datCheqFromDate.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datCheqFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datCheqFromDate.Location = new System.Drawing.Point(928, 39);
		this.datCheqFromDate.Name = "datCheqFromDate";
		this.datCheqFromDate.Size = new System.Drawing.Size(116, 25);
		this.datCheqFromDate.TabIndex = 113;
		this.datToDate.CustomFormat = "dd-MMM-yyyy";
		this.datToDate.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDate.Location = new System.Drawing.Point(1152, 11);
		this.datToDate.Name = "datToDate";
		this.datToDate.Size = new System.Drawing.Size(115, 25);
		this.datToDate.TabIndex = 111;
		this.datFromDate.CustomFormat = "dd-MMM-yyyy";
		this.datFromDate.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDate.Location = new System.Drawing.Point(928, 11);
		this.datFromDate.Name = "datFromDate";
		this.datFromDate.Size = new System.Drawing.Size(116, 25);
		this.datFromDate.TabIndex = 109;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(1131, 127);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(63, 26);
		this.btnshow.TabIndex = 119;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.CmbAccountTitle.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountTitle.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountTitle.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountTitle.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Appearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance86;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance92).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAccountTitle.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Location = new System.Drawing.Point(928, 68);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Name = "CmbAccountTitle";
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Size = new System.Drawing.Size(339, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).TabIndex = 116;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(833, 44);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(69, 15);
		this.label14.TabIndex = 122;
		this.label14.Text = "CheqFDate";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(834, 16);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(62, 15);
		this.label30.TabIndex = 123;
		this.label30.Text = "FromDate";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(1050, 44);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(76, 15);
		this.label15.TabIndex = 120;
		this.label15.Text = "CheqToDate";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(836, 74);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(73, 15);
		this.label34.TabIndex = 119;
		this.label34.Text = "AccountTitle";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(1050, 16);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(47, 15);
		this.label16.TabIndex = 121;
		this.label16.Text = "ToDate";
		this.groupBox1.Controls.Add(this.btnCancel);
		this.groupBox1.Controls.Add(this.btnAdd);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.txtComments);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbbank);
		this.groupBox1.Controls.Add(this.button1);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.txtcheqno);
		this.groupBox1.Controls.Add(this.txtcheqamount);
		this.groupBox1.Controls.Add(this.txtcheqdate);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 71);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(819, 85);
		this.groupBox1.TabIndex = 27;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancel.Location = new System.Drawing.Point(755, 45);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(53, 26);
		this.btnCancel.TabIndex = 7;
		this.btnCancel.Text = "Cancel";
		this.btnCancel.UseVisualStyleBackColor = true;
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.Location = new System.Drawing.Point(694, 45);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(58, 26);
		this.btnAdd.TabIndex = 6;
		this.btnAdd.Text = "Add";
		this.btnAdd.UseVisualStyleBackColor = true;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label1.Location = new System.Drawing.Point(260, 48);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(70, 17);
		this.label1.TabIndex = 28;
		this.label1.Text = "Comments";
		this.txtComments.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtComments.Location = new System.Drawing.Point(357, 45);
		this.txtComments.Name = "txtComments";
		this.txtComments.Size = new System.Drawing.Size(333, 25);
		this.txtComments.TabIndex = 5;
		this.cmbbank.AllowNull = (DefaultableBoolean)1;
		this.cmbbank.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbbank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbbank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbbank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbbank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbbank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbbank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.cmbbank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbbank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbbank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbbank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbbank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbbank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbbank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance79).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbbank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbbank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbbank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbbank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbbank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbbank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbbank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbbank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbbank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbbank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbbank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbbank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbbank).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbbank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbbank).Location = new System.Drawing.Point(94, 17);
		((System.Windows.Forms.Control)(object)this.cmbbank).Name = "cmbbank";
		((System.Windows.Forms.Control)(object)this.cmbbank).Size = new System.Drawing.Size(160, 24);
		((System.Windows.Forms.Control)(object)this.cmbbank).TabIndex = 0;
		this.button1.BackColor = System.Drawing.Color.White;
		this.button1.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.button1.Location = new System.Drawing.Point(258, 18);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(23, 23);
		this.button1.TabIndex = 26;
		this.button1.TabStop = false;
		this.button1.UseVisualStyleBackColor = false;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label6.Location = new System.Drawing.Point(467, 19);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(60, 17);
		this.label6.TabIndex = 12;
		this.label6.Text = "Cheq No";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label8.Location = new System.Drawing.Point(2, 51);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(87, 17);
		this.label8.TabIndex = 12;
		this.label8.Text = "Cheq Amount";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label5.Location = new System.Drawing.Point(284, 22);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(69, 17);
		this.label5.TabIndex = 12;
		this.label5.Text = "Cheq Date";
		this.txtcheqno.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtcheqno.Location = new System.Drawing.Point(536, 16);
		this.txtcheqno.Name = "txtcheqno";
		this.txtcheqno.Size = new System.Drawing.Size(154, 25);
		this.txtcheqno.TabIndex = 2;
		this.txtcheqamount.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtcheqamount.Location = new System.Drawing.Point(94, 45);
		this.txtcheqamount.Name = "txtcheqamount";
		this.txtcheqamount.Size = new System.Drawing.Size(160, 25);
		this.txtcheqamount.TabIndex = 3;
		this.txtcheqamount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcheqamount_KeyPress);
		this.txtcheqdate.CustomFormat = "dd-MMM-yy";
		this.txtcheqdate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtcheqdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtcheqdate.Location = new System.Drawing.Point(357, 16);
		this.txtcheqdate.Name = "txtcheqdate";
		this.txtcheqdate.Size = new System.Drawing.Size(104, 25);
		this.txtcheqdate.TabIndex = 1;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label19.Location = new System.Drawing.Point(5, 16);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(74, 17);
		this.label19.TabIndex = 25;
		this.label19.Text = "Bank Name";
		this.cmbcheqtype.AllowNull = (DefaultableBoolean)1;
		this.cmbcheqtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcheqtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcheqtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcheqtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbcheqtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcheqtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcheqtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcheqtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.cmbcheqtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcheqtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbcheqtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcheqtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcheqtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcheqtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbcheqtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcheqtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcheqtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcheqtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbcheqtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcheqtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcheqtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcheqtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbcheqtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcheqtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcheqtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbcheqtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcheqtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbcheqtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcheqtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcheqtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcheqtype).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcheqtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcheqtype).Location = new System.Drawing.Point(481, 319);
		((System.Windows.Forms.Control)(object)this.cmbcheqtype).Name = "cmbcheqtype";
		((System.Windows.Forms.Control)(object)this.cmbcheqtype).Size = new System.Drawing.Size(219, 24);
		((System.Windows.Forms.Control)(object)this.cmbcheqtype).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.cmbcheqtype).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbcheqtype).Visible = false;
		this.CmbAccountTitleDebit.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountTitleDebit.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountTitleDebit.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountTitleDebit.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountTitleDebit).DisplayLayout.Appearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbAccountTitleDebit).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountTitleDebit).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitleDebit).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitleDebit).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance73;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitleDebit).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitleDebit).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbAccountTitleDebit).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountTitleDebit).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountTitleDebit).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountTitleDebit).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbAccountTitleDebit).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountTitleDebit).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitleDebit).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountTitleDebit).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbAccountTitleDebit).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountTitleDebit).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitleDebit).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountTitleDebit).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbAccountTitleDebit).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountTitleDebit).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountTitleDebit).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbAccountTitleDebit).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountTitleDebit).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbAccountTitleDebit).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountTitleDebit).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountTitleDebit).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleDebit).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAccountTitleDebit.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleDebit).Location = new System.Drawing.Point(79, 42);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleDebit).Name = "CmbAccountTitleDebit";
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleDebit).Size = new System.Drawing.Size(201, 24);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleDebit).TabIndex = 3;
		this.CmbAccountTitleCredit.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountTitleCredit.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountTitleCredit.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountTitleCredit.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountTitleCredit).DisplayLayout.Appearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbAccountTitleCredit).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountTitleCredit).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitleCredit).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitleCredit).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance60;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitleCredit).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitleCredit).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbAccountTitleCredit).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountTitleCredit).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountTitleCredit).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountTitleCredit).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbAccountTitleCredit).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountTitleCredit).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitleCredit).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance65).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountTitleCredit).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbAccountTitleCredit).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountTitleCredit).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitleCredit).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance68).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountTitleCredit).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbAccountTitleCredit).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountTitleCredit).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountTitleCredit).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbAccountTitleCredit).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountTitleCredit).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbAccountTitleCredit).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountTitleCredit).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountTitleCredit).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleCredit).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAccountTitleCredit.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleCredit).Location = new System.Drawing.Point(605, 12);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleCredit).Name = "CmbAccountTitleCredit";
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleCredit).Size = new System.Drawing.Size(219, 24);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleCredit).TabIndex = 2;
		this.chkapproved.AutoSize = true;
		this.chkapproved.ForeColor = System.Drawing.Color.White;
		this.chkapproved.Location = new System.Drawing.Point(463, 299);
		this.chkapproved.Name = "chkapproved";
		this.chkapproved.Size = new System.Drawing.Size(15, 14);
		this.chkapproved.TabIndex = 13;
		this.chkapproved.TabStop = false;
		this.chkapproved.UseVisualStyleBackColor = true;
		this.chkapproved.Visible = false;
		this.cmbcheqstatus.AllowNull = (DefaultableBoolean)1;
		this.cmbcheqstatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcheqstatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcheqstatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcheqstatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbcheqstatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcheqstatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcheqstatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcheqstatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.cmbcheqstatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcheqstatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbcheqstatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcheqstatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcheqstatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcheqstatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbcheqstatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcheqstatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcheqstatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcheqstatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbcheqstatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcheqstatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcheqstatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcheqstatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbcheqstatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcheqstatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcheqstatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbcheqstatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcheqstatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbcheqstatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcheqstatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcheqstatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcheqstatus).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcheqstatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcheqstatus).Location = new System.Drawing.Point(112, 289);
		((System.Windows.Forms.Control)(object)this.cmbcheqstatus).Name = "cmbcheqstatus";
		this.cmbcheqstatus.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbcheqstatus).Size = new System.Drawing.Size(197, 24);
		((System.Windows.Forms.Control)(object)this.cmbcheqstatus).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.cmbcheqstatus).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbcheqstatus).Visible = false;
		this.txtdocdate.CustomFormat = "dd-MMM-yyyy";
		this.txtdocdate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(83, 12);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(197, 25);
		this.txtdocdate.TabIndex = 0;
		this.txtbranchcode.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtbranchcode.Location = new System.Drawing.Point(112, 318);
		this.txtbranchcode.Name = "txtbranchcode";
		this.txtbranchcode.Size = new System.Drawing.Size(220, 25);
		this.txtbranchcode.TabIndex = 5;
		this.txtbranchcode.TabStop = false;
		this.txtbranchcode.Visible = false;
		this.txtremarks.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(353, 42);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(471, 25);
		this.txtremarks.TabIndex = 4;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label13.Location = new System.Drawing.Point(10, 42);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(57, 17);
		this.label13.TabIndex = 12;
		this.label13.Text = "Debit Ac";
		this.txtdocno.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtdocno.Location = new System.Drawing.Point(353, 15);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.Size = new System.Drawing.Size(160, 25);
		this.txtdocno.TabIndex = 1;
		this.txtdocno.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtdocno_KeyPress);
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label7.Location = new System.Drawing.Point(519, 16);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(76, 17);
		this.label7.TabIndex = 12;
		this.label7.Text = "Party Name";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label3.Location = new System.Drawing.Point(390, 324);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(69, 17);
		this.label3.TabIndex = 12;
		this.label3.Text = "Cheq Type";
		this.label3.Visible = false;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label11.Location = new System.Drawing.Point(390, 296);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(66, 17);
		this.label11.TabIndex = 13;
		this.label11.Text = "Approved";
		this.label11.Visible = false;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label9.Location = new System.Drawing.Point(45, 293);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(43, 17);
		this.label9.TabIndex = 12;
		this.label9.Text = "Status";
		this.label9.Visible = false;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label10.Location = new System.Drawing.Point(289, 45);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(58, 17);
		this.label10.TabIndex = 12;
		this.label10.Text = "Remarks";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label4.Location = new System.Drawing.Point(21, 322);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(82, 17);
		this.label4.TabIndex = 12;
		this.label4.Text = "Branch Code";
		this.label4.Visible = false;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label2.Location = new System.Drawing.Point(292, 19);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(53, 17);
		this.label2.TabIndex = 12;
		this.label2.Text = "Doc No";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label40.Location = new System.Drawing.Point(10, 16);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(62, 17);
		this.label40.TabIndex = 12;
		this.label40.Text = "Doc Date";
		this.panel4.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel4.Controls.Add(this.ChkPrint2);
		this.panel4.Controls.Add(this.ChkPrint1);
		this.panel4.Controls.Add(this.lblCaption);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 28);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1275, 36);
		this.panel4.TabIndex = 5;
		this.lblCaption.AutoSize = true;
		this.lblCaption.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.lblCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCaption.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.lblCaption.Location = new System.Drawing.Point(8, 6);
		this.lblCaption.Name = "lblCaption";
		this.lblCaption.Size = new System.Drawing.Size(220, 24);
		this.lblCaption.TabIndex = 0;
		this.lblCaption.Text = "Post Dated Cheq Receipt";
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1275, 28);
		this.panel2.TabIndex = 3;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1233, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(40, 25);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[11]
		{
			this.btnNew, this.btnsave, this.btnUpdate, this.btnPrint, this.toolStripButton7, this.toolStripButton8, this.toolStripButton1, this.btnDelete, this.btnRegister, this.btnPdcSummery,
			this.btnPdcRegisterGuriList
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1275, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 24);
		this.btnNew.Text = "New";
		this.btnNew.Click += new System.EventHandler(btnnew_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(55, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(79, 24);
		this.btnPrint.Text = "123-Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.toolStripButton7.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton7.Image = (System.Drawing.Image)resources.GetObject("toolStripButton7.Image");
		this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton7.Name = "toolStripButton7";
		this.toolStripButton7.Size = new System.Drawing.Size(97, 24);
		this.toolStripButton7.Text = "118-Voucher";
		this.toolStripButton7.Click += new System.EventHandler(toolStripButton7_Click);
		this.toolStripButton8.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton8.Image = (System.Drawing.Image)resources.GetObject("toolStripButton8.Image");
		this.toolStripButton8.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton8.Name = "toolStripButton8";
		this.toolStripButton8.Size = new System.Drawing.Size(94, 24);
		this.toolStripButton8.Text = "&Attachment";
		this.toolStripButton8.Click += new System.EventHandler(toolStripButton8_Click);
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(50, 24);
		this.toolStripButton1.Text = "Refresh";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.icons8_cut_16;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(64, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnRegister.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRegister.Name = "btnRegister";
		this.btnRegister.Size = new System.Drawing.Size(116, 24);
		this.btnRegister.Text = "125-PdcRegister";
		this.btnRegister.Click += new System.EventHandler(btnRegister_Click);
		this.btnPdcSummery.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPdcSummery.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPdcSummery.Name = "btnPdcSummery";
		this.btnPdcSummery.Size = new System.Drawing.Size(125, 24);
		this.btnPdcSummery.Text = "126-PdcSummery";
		this.btnPdcSummery.Click += new System.EventHandler(btnPdcSummery_Click);
		this.btnPdcRegisterGuriList.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPdcRegisterGuriList.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPdcRegisterGuriList.Name = "btnPdcRegisterGuriList";
		this.btnPdcRegisterGuriList.Size = new System.Drawing.Size(163, 24);
		this.btnPdcRegisterGuriList.Text = "127-PdcRegisterGuriWise";
		this.btnPdcRegisterGuriList.Click += new System.EventHandler(btnPdcRegisterGuriList_Click);
		this.tabPage2.Controls.Add(this.panel3);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1489, 710);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel3.Controls.Add(this.ctrlGrdBar2);
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Controls.Add(this.toolStrip2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1483, 704);
		this.panel3.TabIndex = 0;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1438, -3);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(40, 24);
		this.ctrlGrdBar2.TabIndex = 22;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.grdHistory.AllowDelete = (InheritableBoolean)1;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Cursor = System.Windows.Forms.Cursors.Default;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupByBoxVisible = false;
		this.grdHistory.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdHistory).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1483, 679);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.grdHistory).TabStop = false;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 25);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1483, 679);
		this.panel6.TabIndex = 7;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.toolStripButton10 });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1483, 25);
		this.toolStrip2.TabIndex = 6;
		this.toolStrip2.Text = "toolStrip2";
		this.toolStripButton10.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton10.Name = "toolStripButton10";
		this.toolStripButton10.Size = new System.Drawing.Size(51, 22);
		this.toolStripButton10.Text = "LoadAll";
		this.toolStripButton10.Click += new System.EventHandler(toolStripButton10_Click);
		this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenuStrip1.Name = "contextMenuStrip1";
		this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
		this.printDialog1.UseEXDialog = true;
		this.ChkPrint2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrint2.AutoSize = true;
		this.ChkPrint2.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrint2.ForeColor = System.Drawing.Color.White;
		this.ChkPrint2.ImageAlign = System.Drawing.ContentAlignment.TopRight;
		this.ChkPrint2.Location = new System.Drawing.Point(1153, 12);
		this.ChkPrint2.Margin = new System.Windows.Forms.Padding(2);
		this.ChkPrint2.Name = "ChkPrint2";
		this.ChkPrint2.Size = new System.Drawing.Size(121, 19);
		this.ChkPrint2.TabIndex = 12;
		this.ChkPrint2.TabStop = false;
		this.ChkPrint2.Text = "Voucher Preview";
		this.ChkPrint2.UseVisualStyleBackColor = true;
		this.ChkPrint1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrint1.AutoSize = true;
		this.ChkPrint1.Checked = true;
		this.ChkPrint1.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPrint1.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrint1.ForeColor = System.Drawing.Color.White;
		this.ChkPrint1.ImageAlign = System.Drawing.ContentAlignment.TopRight;
		this.ChkPrint1.Location = new System.Drawing.Point(1047, 12);
		this.ChkPrint1.Margin = new System.Windows.Forms.Padding(2);
		this.ChkPrint1.Name = "ChkPrint1";
		this.ChkPrint1.Size = new System.Drawing.Size(95, 19);
		this.ChkPrint1.TabIndex = 11;
		this.ChkPrint1.TabStop = false;
		this.ChkPrint1.Text = "Slip Preview";
		this.ChkPrint1.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1289, 736);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "AcfrmDefPdcManagment";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(AcfrmDefPdcManagment_FormClosing);
		base.Load += new System.EventHandler(AcfrmDefPdcManagment_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(AcfrmDefPdcManagment_KeyDown);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdReprot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDebitAccountrpt).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChequeStatusReprot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitle).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbbank).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcheqtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitleDebit).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitleCredit).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcheqstatus).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel6.ResumeLayout(false);
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
