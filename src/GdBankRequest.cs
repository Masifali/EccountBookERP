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
namespace Architecture.WinApp.Export
{

public class GdBankRequest : Form
{
	private List<GDBreakUpBankRequestDetail> LstRemoveRecordDetail = new List<GDBreakUpBankRequestDetail>();

	private UserAccount UserAccount = new UserAccount();

	private Rightsobjects formRights = new Rightsobjects();

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtDetail = new DataTable();

	private int RecId;

	private int DefaultDaysToLessFromHistoryFromDate;

	private double RemaingOfRecvAmount;

	private int UniqueSubCodeForOneEntry = 1;

	private int updateDetailIndex = -1;

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel1;

	private ToolStrip toolStrip1;

	private ToolStripButton BtnNew;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnSave;

	private ToolStripButton btnUpdate;

	private ToolStripButton BtnPrint;

	private ToolStripButton btnAttachment;

	private ToolStripButton btnShortCutKey;

	private Panel panel2;

	private GroupBox groupBox1;

	private DateTimePicker DocDate;

	internal UltraCombo CmbBank;

	private Label label2;

	private Label label1;

	private TextBox txtRemarks;

	private Label label20;

	private Panel panel3;

	private GroupBox groupBox2;

	private TextBox txtFttAmount;

	private Label label8;

	private TextBox txtRealizedAmount;

	private Label label7;

	private TextBox txtGdValueBalance;

	private Label label6;

	private TextBox txtInvoiceNo;

	private Label label5;

	private TextBox txtGdValue;

	private Label label4;

	private Label label3;

	internal UltraCombo CmbGdNo;

	private TextBox txtAmountRcvd;

	private Label label51;

	private TextBox txtStatus;

	private Label label9;

	private TextBox txtRemarksDetail;

	private Label label10;

	private Button btnAdd;

	private Button btnDetailCancel;

	private Button btnDetailUpdate;

	internal GridEX grdDetail;

	private CtrlGrdBar ctrlGrdBar1;

	private TextBox txtDocNo;

	private Label label11;

	private TextBox txtFobValue;

	private Label Fob;

	private Panel panel4;

	private GroupBox groupBox3;

	private Panel panel5;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	private ToolStripButton toolStripButton5;

	private Label label12;

	private DateTimePicker FromDateHistory;

	private Label label13;

	private DateTimePicker ToDateHistory;

	private TextBox ToDocNo;

	private Label label15;

	private TextBox FromDocNo;

	private Label label14;

	private Label label16;

	internal UltraCombo CmbBankHistory;

	private Button btnShow;

	private Panel panel6;

	private Label label17;

	internal GridEX DataGridHistory;

	private Label label18;

	internal GridEX GridDetailHistory;

	private ToolStripButton btnRefreshHistory;

	private CtrlGrdBar ctrlGrdBar3;

	private CtrlGrdBar ctrlGrdBar2;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	public GdBankRequest(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void GdBankRequest_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1180, 700);
			formRights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formRights.DoHaveSaveRight;
			btnUpdate.Enabled = formRights.DoHaveUpdateRights;
			BtnPrint.Enabled = formRights.DoHavePrintRights;
			GenerateCode();
			BankFill();
			HistoryBankFill();
			dtDetail.Columns.Add("Id");
			dtDetail.Columns.Add("SubCode", typeof(int));
			dtDetail.Columns.Add("AmountRcvd", typeof(double));
			dtDetail.Columns.Add("GdId", typeof(int));
			dtDetail.Columns.Add("RefDocumentTypeId", typeof(int));
			dtDetail.Columns.Add("GdNo");
			dtDetail.Columns.Add("GdValue", typeof(double));
			dtDetail.Columns.Add("FobValue", typeof(double));
			dtDetail.Columns.Add("GdBalance", typeof(double));
			dtDetail.Columns.Add("Realized", typeof(double));
			dtDetail.Columns.Add("FTT", typeof(double));
			dtDetail.Columns.Add("Status", typeof(string));
			dtDetail.Columns.Add("Remarks");
			grdDetail.DataSource = dtDetail;
			grdDetail.RetrieveStructure();
			GridDetailSetting();
			btnAdd.Visible = true;
			btnDetailUpdate.Visible = false;
			btnDetailCancel.Visible = false;
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			int Code = GDBreakUpBankRequestHeader.GenerateCode(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (Code > 0)
			{
				txtDocNo.Text = Code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BankFill()
	{
		try
		{
			DataTable dt = ExImInvoice.GetDataForDropDownFromExportGdBreakup(UserAccount.OrganizationId, UserAccount.CompanyId, "Bank");
			DataTable dtBank = new DataTable();
			dtBank.Columns.Add("Id", typeof(int));
			dtBank.Columns.Add("Name");
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtBank.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
				}
			}
			DDL.BindDDL(dtBank, CmbBank, "Id", "Name", "Bank Name", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryBankFill()
	{
		try
		{
			DataTable dtBank = new DataTable();
			dtBank.Columns.Add("Id", typeof(int));
			dtBank.Columns.Add("Bank");
			DataTable dt = GDBreakUpBankRequestHeader.GetDataForDropDownFromGDBreakUpBankRequest(UserAccount.OrganizationId, UserAccount.CompanyId, "Bank");
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtBank.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
				}
			}
			DDL.BindDDL(dtBank, CmbBankHistory, "Id", "Bank", "Bank Name", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetGdsAgainstBank(int BankId)
	{
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		try
		{
			DataTable dtGd = new DataTable();
			dtGd.Columns.Add("Id", typeof(int));
			dtGd.Columns.Add("GdNo");
			dtGd.Columns.Add("InvoiceNo");
			dtGd.Columns.Add("GdValue");
			dtGd.Columns.Add("GdBalance");
			dtGd.Columns.Add("Status");
			dtGd.Columns.Add("FobValue");
			dtGd.Columns.Add("RefDocumentTypeId");
			DataTable dt = ExImInvoice.GetGdBalanceFromBankRequest(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				AccountId = BankId,
				Id = RecId
			});
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				dtGd.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["GDNO"], dt.Rows[i]["BankInvoiceNo"], dt.Rows[i]["GDValue"], dt.Rows[i]["GDBalance"], dt.Rows[i]["GdStatus"], dt.Rows[i]["FobValue"], dt.Rows[i]["DocumentTypeId"]);
			}
			DDL.BindDDL(dtGd, CmbGdNo, "Id", "GdNo", "Gd No", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbBank_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbBank.Value) > 0)
			{
				GetGdsAgainstBank(Conversion.ToInt(CmbBank.Value));
				return;
			}
			((Control)(object)CmbGdNo).Text = string.Empty;
			((UltraGridBase)CmbGdNo).DataSource = null;
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
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["GdId"].Visible = false;
			grdDetail.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grdDetail.RootTable.Columns["Realized"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Realized"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Realized"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["Realized"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["FTT"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["FTT"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["FTT"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["FTT"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(grdDetail);
			grdDetail.RootTable.Columns.Add("Delete");
			grdDetail.RootTable.Columns["Delete"].Key = "Delete";
			grdDetail.RootTable.Columns["Delete"].Caption = "X";
			grdDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdDetail.RootTable.Columns["Delete"].Width = 20;
			grdDetail.RootTable.Columns["Delete"].Position = 1;
			grdDetail.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
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

	private void txtAmountRcvd_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbGdNo.Value) > 0)
			{
				RemaingOfRecvAmount = Conversion.ToDouble((object)txtAmountRcvd.Text);
				Calculation();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbGdNo_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbGdNo.Value) > 0)
			{
				txtInvoiceNo.Text = Conversion.ToString(((UltraDropDownBase)CmbGdNo).SelectedRow.Cells["InvoiceNo"].Value);
				txtGdValue.Text = Conversion.ToDouble(((UltraDropDownBase)CmbGdNo).SelectedRow.Cells["GdValue"].Value).ToString("#,##0.###");
				txtFobValue.Text = Conversion.ToDouble(((UltraDropDownBase)CmbGdNo).SelectedRow.Cells["FobValue"].Value).ToString("#,##0.###");
				txtGdValueBalance.Text = Conversion.ToDouble(((UltraDropDownBase)CmbGdNo).SelectedRow.Cells["GdBalance"].Value).ToString("#,##0.###");
				if (txtAmountRcvd.Enabled && !(Conversion.ToDouble((object)txtRealizedAmount.Text) > 0.0))
				{
					txtRealizedAmount.Text = Conversion.ToDouble(((UltraDropDownBase)CmbGdNo).SelectedRow.Cells["GdBalance"].Value).ToString("#,##0.###");
				}
				int count = Conversion.ToInt(((UltraDropDownBase)CmbGdNo).SelectedRow.Cells["Status"].Value);
				txtStatus.Text = GetOrdinal(count + 1) + " Part";
				if (Conversion.ToDouble((object)txtAmountRcvd.Text) > 0.0)
				{
					Calculation();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formvalidation()
	{
		if (txtDocNo.Text == "" || Conversion.ToInt((object)txtDocNo.Text) == 0)
		{
			MessageBox.Show("Doc No Is Required");
			txtDocNo.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbBank.Value) <= 0)
		{
			MessageBox.Show("Bank Is Required");
			CmbBank.Focus();
			return false;
		}
		if (!txtAmountRcvd.Enabled)
		{
			MessageBox.Show("You Cant Save Record Beacuse Bank Recieved Amount Is not Completly Proportionated");
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (!(Conversion.ToDouble((object)txtAmountRcvd.Text) > 0.0))
		{
			MessageBox.Show("Rcvd Amount field required");
			txtAmountRcvd.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbGdNo.Value) == 0)
		{
			MessageBox.Show("Gd No field required");
			CmbGdNo.Focus();
			return false;
		}
		if (txtGdValue.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtGdValue.Text.Trim()) > 0.0))
		{
			MessageBox.Show("Gd Value field required");
			txtGdValue.Focus();
			return false;
		}
		if (Conversion.ToString((object)txtInvoiceNo.Text) == string.Empty)
		{
			MessageBox.Show("Invoice No field required");
			txtInvoiceNo.Focus();
			return false;
		}
		if (txtRealizedAmount.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtRealizedAmount.Text.Trim()) > 0.0))
		{
			MessageBox.Show("Realized Amount field required");
			txtRealizedAmount.Focus();
			return false;
		}
		if (Conversion.ToString((object)txtStatus.Text) == string.Empty)
		{
			MessageBox.Show("Status field required");
			txtStatus.Focus();
			return false;
		}
		return true;
	}

	private void btnSave_Click(object sender, EventArgs e)
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
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Expected O, but got Unknown
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Expected O, but got Unknown
		try
		{
			if (!formvalidation())
			{
				return;
			}
			if (grdDetail.GetRows().Count() <= 0)
			{
				throw new Exception("Grid Record not found");
			}
			DataTable uniqueRows = (from row in dtDetail.AsEnumerable()
				group row by row.Field<int>("subCode") into @group
				select @group.First()).CopyToDataTable();
			for (int j = 0; j < uniqueRows.Rows.Count; j++)
			{
				double sum = Conversion.ToDouble(dtDetail.Compute("Sum(Realized)", "SubCode = '" + Conversion.ToString(uniqueRows.Rows[j]["SubCode"]) + "'"));
				if (Conversion.ToDouble(uniqueRows.Rows[j]["AmountRcvd"]) != sum)
				{
					MessageBox.Show("Amount Received " + Conversion.ToString(uniqueRows.Rows[j]["AmountRcvd"]) + " not Equal To Total Realized " + sum, "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
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
			GDBreakUpBankRequestHeader PIH = new GDBreakUpBankRequestHeader();
			if (RecId > 0)
			{
				PIH.Id = RecId;
			}
			PIH.OrganizationId = UserAccount.OrganizationId;
			PIH.CompanyId = UserAccount.CompanyId;
			PIH.EntryDate = DateTime.Now;
			PIH.EntryUserId = UserAccount.ID;
			PIH.ModifyDate = DateTime.Now;
			PIH.ModifyUserId = UserAccount.ID;
			PIH.DocNo = Conversion.ToInt((object)txtDocNo.Text);
			PIH.DocDate = Conversion.ToDateTime((object)DocDate.Value);
			PIH.BankId = Conversion.ToInt(CmbBank.Value);
			PIH.RemarksHeader = Conversion.ToString((object)txtRemarks.Text);
			PIH.GdBreakUpBankRequestDetailList = new List<GDBreakUpBankRequestDetail>();
			if (RecId > 0 && LstRemoveRecordDetail.Count > 0)
			{
				for (int i = 0; i < LstRemoveRecordDetail.Count; i++)
				{
					PIH.GdBreakUpBankRequestDetailList.Add(LstRemoveRecordDetail[i]);
				}
			}
			if (grdDetail.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow r in rows)
				{
					GDBreakUpBankRequestDetail vd = new GDBreakUpBankRequestDetail();
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd.SubCode = Conversion.ToInt(r.Cells["SubCode"].Value);
					vd.BankReceivedAmount = Conversion.ToDecimal(r.Cells["AmountRcvd"].Value);
					vd.GDId = Conversion.ToInt(r.Cells["GdId"].Value);
					vd.RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
					vd.RealizedAmount = Conversion.ToDecimal(r.Cells["Realized"].Value);
					vd.CommAmount = Conversion.ToDecimal(r.Cells["FTT"].Value);
					vd.Status = Conversion.ToString(r.Cells["Status"].Value);
					vd.RemarksDetail = Conversion.ToString((object)r.Cells["Remarks"].Text);
					vd.ActionTypeId = ((vd.Id <= 0) ? 1 : 2);
					PIH.GdBreakUpBankRequestDetailList.Add(vd);
				}
				PIH.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				PIH.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<GDBreakUpBankRequestHeader>(PIH, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				GDBreakUpBankRequestHeader.Save(PIH);
				if (RecId == 0)
				{
					MessageBox.Show("Save SuccessFully");
				}
				if (RecId > 0)
				{
					MessageBox.Show("Update SuccessFully");
					FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 0, base.Name);
				}
				Reset();
				HistoryBankFill();
				return;
			}
			throw new Exception("Detail Not Found. Please Check! ");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Calculation()
	{
		try
		{
			double RealizedAmount = 0.0;
			double rcvdAmount = 0.0;
			if (dtDetail.Rows.Count > 0)
			{
				if (!txtAmountRcvd.Enabled || updateDetailIndex > -1)
				{
					CalculateRealizedFromGrid();
					if (updateDetailIndex > -1)
					{
						RemaingOfRecvAmount = Conversion.ToDouble((object)txtRealizedAmount.Text);
					}
					rcvdAmount = RemaingOfRecvAmount;
				}
				else
				{
					rcvdAmount = Conversion.ToDouble((object)txtAmountRcvd.Text);
				}
			}
			else
			{
				rcvdAmount = Conversion.ToDouble((object)txtAmountRcvd.Text);
			}
			bool idExists = false;
			if ((updateDetailIndex <= -1) ? dtDetail.AsEnumerable().Any((DataRow row) => row.Field<int>("GdId") == Conversion.ToInt(CmbGdNo.Value)) : dtDetail.AsEnumerable().Where((DataRow row, int index) => index != grdDetail.CurrentRow.RowIndex).Any((DataRow row) => row.Field<int>("GdId") == Conversion.ToInt(CmbGdNo.Value)))
			{
				double SumOfSameGd = Conversion.ToDouble(dtDetail.Compute("Sum(Realized)", "GdId = '" + CmbGdNo.Value.ToString() + "'"));
				double SumOfFttOfSameGd = Conversion.ToDouble(dtDetail.Compute("Sum(FTT)", "GdId = '" + CmbGdNo.Value.ToString() + "'"));
				int Count = Conversion.ToInt(dtDetail.Compute("Count(GdId)", "GdId = '" + CmbGdNo.Value.ToString() + "'"));
				if (updateDetailIndex > -1)
				{
					if (SumOfSameGd + SumOfFttOfSameGd == Conversion.ToDouble(((UltraDropDownBase)CmbGdNo).SelectedRow.Cells["GdBalance"].Value))
					{
						return;
					}
					SumOfSameGd -= Conversion.ToDouble(grdDetail.CurrentRow.Cells["Realized"].Value);
					if (Count > 0)
					{
						txtStatus.Text = GetOrdinal(Count) + " Part";
					}
				}
				else
				{
					if (SumOfSameGd + SumOfFttOfSameGd == Conversion.ToDouble(((UltraDropDownBase)CmbGdNo).SelectedRow.Cells["GdBalance"].Value))
					{
						return;
					}
					if (Count > 0)
					{
						txtStatus.Text = GetOrdinal(Count + 1) + " Part";
					}
				}
				RealizedAmount = Conversion.ToDouble((object)txtGdValueBalance.Text) - SumOfSameGd;
			}
			else
			{
				RealizedAmount = Conversion.ToDouble(((UltraDropDownBase)CmbGdNo).SelectedRow.Cells["GdBalance"].Value);
			}
			Conversion.ToDouble((object)txtGdValue.Text);
			double Ftt = 0.0;
			double Realized = 0.0;
			if (rcvdAmount >= RealizedAmount)
			{
				Ftt = Conversion.ToDouble((object)txtFobValue.Text) * 10.0 / 100.0;
				txtFttAmount.Text = Ftt.ToString("#,##0.###");
				if (Ftt > rcvdAmount)
				{
					MessageBox.Show("Remaining Or Rcvd Amount Is Less than Ftt Amount.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				Realized = RealizedAmount - Ftt;
				txtRealizedAmount.Text = Realized.ToString("#,##0.###");
				txtStatus.Text = "Final";
			}
			else
			{
				txtRealizedAmount.Text = rcvdAmount.ToString("#,##0.###");
				txtFttAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateRealizedFromGrid()
	{
		try
		{
			if (dtDetail.Rows.Count <= 0)
			{
				return;
			}
			double Remaining1 = 0.0;
			if (updateDetailIndex > -1)
			{
				if (Conversion.ToInt(dtDetail.Compute("Count(SubCode)", "SubCode = '" + grdDetail.CurrentRow.Cells["SubCode"].Value.ToString() + "'")) > 1 && Conversion.ToDouble(grdDetail.CurrentRow.Cells["AmountRcvd"].Value) != Conversion.ToDouble((object)txtAmountRcvd.Text))
				{
					MessageBox.Show("You Are Changing Rcvd Amount and there are multiple rows are present in grid against this detail entry. You can delete The Entire entry against it", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					txtAmountRcvd.Text = grdDetail.CurrentRow.Cells["AmountRcvd"].Value.ToString();
				}
				double minusCurrentRow = Conversion.ToDouble(dtDetail.Compute("Sum(Realized)", "SubCode = '" + grdDetail.CurrentRow.Cells["SubCode"].Value.ToString() + "'")) - Conversion.ToDouble(grdDetail.CurrentRow.Cells["Realized"].Value);
				Remaining1 = Conversion.ToDouble((object)txtAmountRcvd.Text) - minusCurrentRow;
			}
			else
			{
				int subCodeWithUnproportionateAmount = (from row in dtDetail.AsEnumerable()
					where row.Field<double>("AmountRcvd") == Conversion.ToDouble((object)txtAmountRcvd.Text)
					group row by row.Field<int>("SubCode") into @group
					where @group.Sum((DataRow row) => row.Field<double>("Realized")) != Conversion.ToDouble((object)txtAmountRcvd.Text)
					select @group.Key).FirstOrDefault();
				string subCodeFilter = ((subCodeWithUnproportionateAmount != 0) ? subCodeWithUnproportionateAmount.ToString() : UniqueSubCodeForOneEntry.ToString());
				double Realized1 = Conversion.ToDouble(dtDetail.Compute("Sum(Realized)", "SubCode = " + subCodeFilter));
				Remaining1 = Conversion.ToDouble((object)txtAmountRcvd.Text) - Realized1;
			}
			txtRealizedAmount.Text = Remaining1.ToString("#,##0.###");
			RemaingOfRecvAmount = Remaining1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static string GetOrdinal(int number)
	{
		if (number <= 0)
		{
			return number.ToString();
		}
		int num = number % 100;
		if ((uint)(num - 11) <= 2u)
		{
			return number + "th";
		}
		return (number % 10) switch
		{
			1 => number + "st", 
			2 => number + "nd", 
			3 => number + "rd", 
			_ => number + "th", 
		};
	}

	private void CheckValidation()
	{
		try
		{
			double RcvdAmount = Conversion.ToDouble((object)txtAmountRcvd.Text);
			double gdBalance = Conversion.ToDouble((object)txtGdValueBalance.Text);
			double num = Conversion.ToDouble((object)txtFttAmount.Text);
			bool status = false;
			if (num > 0.0)
			{
				if (txtAmountRcvd.Enabled)
				{
					if (updateDetailIndex > -1)
					{
						RemaingOfRecvAmount -= Conversion.ToDouble((object)txtRealizedAmount.Text);
					}
					else
					{
						RemaingOfRecvAmount = RcvdAmount - Conversion.ToDouble((object)txtRealizedAmount.Text);
					}
				}
				else
				{
					RemaingOfRecvAmount -= Conversion.ToDouble((object)txtRealizedAmount.Text);
				}
				txtRealizedAmount.Text = RemaingOfRecvAmount.ToString("#,##0.###");
				status = true;
			}
			if (!status)
			{
				if (RemaingOfRecvAmount < gdBalance)
				{
					txtAmountRcvd.Enabled = true;
					txtAmountRcvd.Clear();
					txtRealizedAmount.Clear();
					txtStatus.Clear();
					txtRemarksDetail.Clear();
					txtAmountRcvd.Focus();
				}
				else
				{
					txtAmountRcvd.Enabled = true;
					((Control)(object)CmbGdNo).Enabled = true;
					txtAmountRcvd.Clear();
					((Control)(object)CmbGdNo).Text = string.Empty;
					txtGdValue.Clear();
					txtInvoiceNo.Clear();
					txtGdValueBalance.Clear();
					txtRealizedAmount.Clear();
					txtStatus.Clear();
					txtRemarksDetail.Clear();
				}
			}
			else
			{
				txtAmountRcvd.Enabled = false;
				((Control)(object)CmbGdNo).Enabled = true;
				((Control)(object)CmbGdNo).Text = string.Empty;
				txtGdValue.Clear();
				txtInvoiceNo.Clear();
				txtFobValue.Clear();
				txtGdValueBalance.Clear();
				txtStatus.Clear();
				txtFttAmount.Clear();
				txtRemarksDetail.Clear();
				CmbGdNo.Focus();
				_ = UniqueSubCodeForOneEntry;
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
			if (!FormValidationDetail())
			{
				return;
			}
			if (dtDetail.AsEnumerable().Any((DataRow row) => row.Field<int>("GdId") == Conversion.ToInt(CmbGdNo.Value)))
			{
				double num = Conversion.ToDouble(dtDetail.Compute("Sum(Realized)", "GdId = '" + CmbGdNo.Value.ToString() + "'"));
				double SumOfFttOfSameGd = Conversion.ToDouble(dtDetail.Compute("Sum(FTT)", "GdId = '" + CmbGdNo.Value.ToString() + "'"));
				if (num + SumOfFttOfSameGd == Conversion.ToDouble(((UltraDropDownBase)CmbGdNo).SelectedRow.Cells["GdBalance"].Value))
				{
					MessageBox.Show("Please Select Another Gd Because This Gd is Completed in Detail");
					return;
				}
			}
			int subCodeWithUnpropriateAmount = 0;
			if (dtDetail.Rows.Count > 0)
			{
				int subCodeWithUnproportionateAmount = (from row in dtDetail.AsEnumerable()
					where row.Field<double>("AmountRcvd") == Conversion.ToDouble((object)txtAmountRcvd.Text)
					group row by row.Field<int>("SubCode") into @group
					where @group.Sum((DataRow row) => row.Field<double>("Realized")) != Conversion.ToDouble((object)txtAmountRcvd.Text)
					select @group.Key).FirstOrDefault();
				if (subCodeWithUnproportionateAmount != 0)
				{
					subCodeWithUnpropriateAmount = subCodeWithUnproportionateAmount;
				}
			}
			dtDetail.Rows.Add(0, (subCodeWithUnpropriateAmount != 0) ? subCodeWithUnpropriateAmount : UniqueSubCodeForOneEntry, Conversion.ToDouble((object)txtAmountRcvd.Text), Conversion.ToInt(CmbGdNo.Value), Conversion.ToDouble(((UltraDropDownBase)CmbGdNo).SelectedRow.Cells["RefDocumentTypeId"].Value), Conversion.ToString((object)((Control)(object)CmbGdNo).Text), Conversion.ToDouble(((UltraDropDownBase)CmbGdNo).SelectedRow.Cells["GdValue"].Value), Conversion.ToDouble(((UltraDropDownBase)CmbGdNo).SelectedRow.Cells["FobValue"].Value), Conversion.ToDouble(((UltraDropDownBase)CmbGdNo).SelectedRow.Cells["GdBalance"].Value), Conversion.ToDouble((object)txtRealizedAmount.Text), Conversion.ToDouble((object)txtFttAmount.Text), Conversion.ToString((object)txtStatus.Text), Conversion.ToString((object)txtRemarksDetail.Text));
			grdDetail.DataSource = dtDetail;
			grdDetail.RetrieveStructure();
			GridDetailSetting();
			CheckValidation();
			if (txtAmountRcvd.Enabled)
			{
				UniqueSubCodeForOneEntry = Conversion.ToInt((object)grdDetail.GetRows().Max((GridEXRow o) => Conversion.ToInt(o.Cells["SubCode"].Value)));
				UniqueSubCodeForOneEntry++;
			}
			((Control)(object)CmbBank).Enabled = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDetailUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationDetail())
			{
				return;
			}
			GridEXRow item = grdDetail.CurrentRow;
			int CurrentRowsubCode = Conversion.ToInt(item.Cells["SubCode"].Value);
			DataRow[] SubCodeArray = dtDetail.Select("SubCode='" + Conversion.ToInt((object)CurrentRowsubCode) + "'");
			if (SubCodeArray.Length > 1 && Conversion.ToDouble(SubCodeArray[0]["AmountRcvd"]) != Conversion.ToDouble((object)txtAmountRcvd.Text))
			{
				MessageBox.Show("You Are Changing Rcvd Amount and there are multiple rows are present in grid against this detail entry. You can delete The Entire entry against it", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			dtDetail.Rows[updateDetailIndex]["SubCode"] = item.Cells["SubCode"].Value.ToString();
			dtDetail.Rows[updateDetailIndex]["AmountRcvd"] = Conversion.ToDouble((object)txtAmountRcvd.Text);
			dtDetail.Rows[updateDetailIndex]["GdId"] = Conversion.ToInt(CmbGdNo.Value);
			dtDetail.Rows[updateDetailIndex]["RefDocumentTypeId"] = Conversion.ToInt(((UltraDropDownBase)CmbGdNo).SelectedRow.Cells["RefDocumentTypeId"].Value);
			dtDetail.Rows[updateDetailIndex]["GdNo"] = ((Control)(object)CmbGdNo).Text.Trim();
			dtDetail.Rows[updateDetailIndex]["Realized"] = Conversion.ToDouble((object)txtRealizedAmount.Text);
			dtDetail.Rows[updateDetailIndex]["FTT"] = Conversion.ToDouble((object)txtFttAmount.Text);
			dtDetail.Rows[updateDetailIndex]["Status"] = txtStatus.Text;
			dtDetail.Rows[updateDetailIndex]["Remarks"] = txtRemarksDetail.Text;
			CheckValidation();
			if (txtAmountRcvd.Enabled)
			{
				UniqueSubCodeForOneEntry = Conversion.ToInt((object)grdDetail.GetRows().Max((GridEXRow o) => Conversion.ToInt(o.Cells["SubCode"].Value)));
				UniqueSubCodeForOneEntry++;
			}
			updateDetailIndex = -1;
			btnAdd.Visible = true;
			btnDetailUpdate.Visible = false;
			btnDetailCancel.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDetailCancel_Click(object sender, EventArgs e)
	{
		try
		{
			if (txtAmountRcvd.Enabled)
			{
				txtAmountRcvd.Clear();
			}
			((Control)(object)CmbGdNo).Text = string.Empty;
			txtGdValue.Clear();
			txtFobValue.Clear();
			txtInvoiceNo.Clear();
			txtGdValueBalance.Clear();
			txtRealizedAmount.Clear();
			txtStatus.Clear();
			txtRemarksDetail.Clear();
			updateDetailIndex = -1;
			btnAdd.Visible = true;
			btnDetailUpdate.Visible = false;
			btnDetailCancel.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdDetail.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				txtAmountRcvd.Text = item.Cells["AmountRcvd"].Value.ToString();
				CmbGdNo.Value = item.Cells["GdId"].Value.ToString();
				CmbGdNo_Leave(null, null);
				txtRealizedAmount.Text = Conversion.ToDouble(item.Cells["Realized"].Value).ToString("#,##0.###");
				txtFttAmount.Text = Conversion.ToDouble(item.Cells["FTT"].Value).ToString("#,##0.###");
				txtStatus.Text = item.Cells["Status"].Value.ToString();
				txtRemarksDetail.Text = item.Cells["Remarks"].Value.ToString();
				btnAdd.Visible = false;
				btnDetailUpdate.Visible = true;
				btnDetailCancel.Visible = true;
			}
			if (!txtAmountRcvd.Enabled && dtDetail.Select("SubCode='" + Conversion.ToInt(item.Cells["SubCode"].Value) + "'").Length == 1)
			{
				txtAmountRcvd.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Expected O, but got Unknown
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!(e.Column.Key == "Delete") || MessageBox.Show("Are you sure! You Want To Delete.Because Entire Entry Will be delete Against this?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			GridEXRow item = grdDetail.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int DetailId = Conversion.ToInt(item.Cells["Id"].Value);
				int subCode = Conversion.ToInt(item.Cells["SubCode"].Value);
				if (dtDetail.Select("SubCode='" + Conversion.ToInt((object)subCode) + "'").Length != 0)
				{
					if (DetailId > 0)
					{
						GridEXRow[] rows = grdDetail.GetRows();
						foreach (GridEXRow r in rows)
						{
							if (Conversion.ToInt(r.Cells["SubCode"].Value) == subCode)
							{
								GDBreakUpBankRequestDetail vd = new GDBreakUpBankRequestDetail();
								vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
								vd.BankReceivedAmount = Conversion.ToDecimal(r.Cells["AmountRcvd"].Value);
								vd.GDId = Conversion.ToInt(r.Cells["GdId"].Value);
								vd.RealizedAmount = Conversion.ToDecimal(r.Cells["Realized"].Value);
								vd.CommAmount = Conversion.ToDecimal(r.Cells["FTT"].Value);
								vd.Status = Conversion.ToString(r.Cells["Status"].Value);
								vd.RemarksDetail = Conversion.ToString((object)r.Cells["Remarks"].Text);
								vd.ActionTypeId = 3;
								LstRemoveRecordDetail.Add(vd);
								dtDetail.Rows[r.RowIndex].Delete();
								if (r != null && (int)r.RowType == 0)
								{
									r.Delete();
								}
							}
						}
					}
					else
					{
						GridEXRow[] rows = grdDetail.GetRows();
						foreach (GridEXRow row in rows)
						{
							if (Conversion.ToInt(row.Cells["SubCode"].Value) == subCode)
							{
								row.Delete();
							}
						}
					}
				}
			}
			grdDetail.UpdateData();
			if (dtDetail.Rows.Count > 0)
			{
				((Control)(object)CmbBank).Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int ID)
	{
		try
		{
			grdDetail.ClearStructure();
			RecId = Conversion.ToInt((object)ID);
			GDBreakUpBankRequestHeader obj = GDBreakUpBankRequestHeader.GetByID(ID);
			if (obj != null)
			{
				btnSave.Visible = false;
				btnUpdate.Visible = true;
				tabControl1.SelectedIndex = 0;
				DocDate.Value = obj.DocDate;
				txtDocNo.Text = obj.DocNo.ToString();
				((Control)(object)CmbBank).Enabled = false;
				CmbBank.Value = obj.BankId;
				CmbBank_Leave(null, null);
				txtRemarks.Text = obj.RemarksHeader;
				dtDetail.Rows.Clear();
				for (int i = 0; i < obj.GdBreakUpBankRequestDetailList.Count; i++)
				{
					dtDetail.Rows.Add(obj.GdBreakUpBankRequestDetailList[i].Id, obj.GdBreakUpBankRequestDetailList[i].SubCode, obj.GdBreakUpBankRequestDetailList[i].BankReceivedAmount, obj.GdBreakUpBankRequestDetailList[i].GDId, obj.GdBreakUpBankRequestDetailList[i].RefDocumentTypeId, obj.GdBreakUpBankRequestDetailList[i].GDNo, obj.GdBreakUpBankRequestDetailList[i].GdValue, obj.GdBreakUpBankRequestDetailList[i].FobValue, obj.GdBreakUpBankRequestDetailList[i].GdBalance, obj.GdBreakUpBankRequestDetailList[i].RealizedAmount, obj.GdBreakUpBankRequestDetailList[i].CommAmount, obj.GdBreakUpBankRequestDetailList[i].Status, obj.GdBreakUpBankRequestDetailList[i].RemarksDetail);
				}
				grdDetail.DataSource = dtDetail;
				grdDetail.RetrieveStructure();
				GridDetailSetting();
				FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			}
			else
			{
				Reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnNew_Click(object sender, EventArgs e)
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

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			LstRemoveRecordDetail.Clear();
			RecId = 0;
			UniqueSubCodeForOneEntry = 1;
			updateDetailIndex = -1;
			txtDocNo.Text = string.Empty;
			((Control)(object)CmbBank).Enabled = true;
			((Control)(object)CmbBank).Text = string.Empty;
			DocDate.Value = DateTime.Now;
			txtRemarks.Text = string.Empty;
			txtAmountRcvd.Enabled = true;
			txtAmountRcvd.Text = string.Empty;
			((UltraGridBase)CmbGdNo).DataSource = null;
			((Control)(object)CmbGdNo).Text = string.Empty;
			txtGdValue.Text = string.Empty;
			txtInvoiceNo.Text = string.Empty;
			txtFttAmount.Clear();
			txtRealizedAmount.Clear();
			txtStatus.Clear();
			txtRemarksDetail.Clear();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnAdd.Visible = true;
			btnDetailUpdate.Visible = false;
			btnDetailCancel.Visible = false;
			DocDate.Focus();
			dtDetail.Rows.Clear();
			((Control)(object)CmbBank).Enabled = true;
			GenerateCode();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now.AddDays(-7.0);
			ToDateHistory.Value = DateTime.Now;
			FromDocNo.Text = string.Empty;
			ToDocNo.Text = string.Empty;
			((Control)(object)CmbBankHistory).Text = string.Empty;
			DataGridHistory.DataSource = null;
			GridDetailHistory.DataSource = null;
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
			BankFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryBankFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				FromDateHistory.Value = DateTime.Now.AddDays(-7.0);
				ToDateHistory.Value = DateTime.Now;
				HistoryFill();
			}
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
			HistoryFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			obj.FromDocNo = Conversion.ToInt((object)FromDocNo.Text);
			obj.ToDocNo = Conversion.ToInt((object)ToDocNo.Text);
			obj.AccountId = Conversion.ToInt(CmbBankHistory.Value);
			DataTable dt = GDBreakUpBankRequestHeader.FormHistory(obj);
			if (dt.Rows.Count > 0)
			{
				DataTable dtHistory = new DataTable();
				dtHistory.Columns.Add("Id");
				dtHistory.Columns.Add("DocNo", typeof(int));
				dtHistory.Columns.Add("DocDate", typeof(DateTime));
				dtHistory.Columns.Add("BankName");
				dtHistory.Columns.Add("EntryDate", typeof(DateTime));
				dtHistory.Columns.Add("EntryUser");
				dtHistory.Columns.Add("ModifyDate", typeof(DateTime));
				dtHistory.Columns.Add("ModifyUser");
				dtHistory.Columns.Add("NoOfAttachments");
				dtHistory.Columns.Add("Remarks");
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtHistory.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["DocNo"], Conversion.ToDateTime(dt.Rows[i]["DocDate"]).ToShortDateString(), dt.Rows[i]["BankName"], Conversion.ToDateTime(dt.Rows[i]["EntryDate"]).ToShortDateString(), dt.Rows[i]["EntryUserName"], Conversion.ToDateTime(dt.Rows[i]["ModifyDate"]).ToShortDateString(), dt.Rows[i]["ModifyUserName"], dt.Rows[i]["NoOfAttachments"], dt.Rows[i]["RemarksHeader"]);
				}
				DataGridHistory.DataSource = dtHistory;
				DataGridHistory.RetrieveStructure();
				DataGridHistory.RootTable.Columns["Id"].Visible = false;
				DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
				CommonServices.GridAutoAdjustmentNew(DataGridHistory);
				DataGridHistory.RootTable.Columns.Add("Edit");
				DataGridHistory.RootTable.Columns["Edit"].Key = "Edit";
				DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
				DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
				DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
				DataGridHistory.RootTable.Columns["Edit"].Width = 50;
				DataGridHistory.RootTable.Columns["Edit"].Position = 0;
				DataGridHistory.RootTable.Columns.Add("Slip");
				DataGridHistory.RootTable.Columns["Slip"].Key = "Slip";
				DataGridHistory.RootTable.Columns["Slip"].Caption = "Slip";
				DataGridHistory.RootTable.Columns["Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				DataGridHistory.RootTable.Columns["Slip"].ButtonStyle = (ButtonStyle)5;
				DataGridHistory.RootTable.Columns["Slip"].ButtonText = "Slip";
				DataGridHistory.RootTable.Columns["Slip"].Width = 50;
				DataGridHistory.RootTable.Columns["Slip"].Position = 1;
				DataGridHistory.FrozenColumns = 2;
				ctrlGrdBar2_Load(null, null);
			}
			else
			{
				DataGridHistory.ClearStructure();
				GridDetailHistory.ClearStructure();
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
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int Id = Conversion.ToInt(item.Cells["Id"].Value);
				if (e.Column.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByScreenName(Id, base.Name);
				}
				else if (e.Column.Key == "Edit")
				{
					ReadById(Id);
				}
				else if (e.Column.Key == "Slip")
				{
					GenerateSlipReport(Id);
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
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count <= 0)
			{
				return;
			}
			GDBreakUpBankRequestHeader obj = GDBreakUpBankRequestHeader.GetByID(Conversion.ToInt(item.Cells["Id"].Value));
			DataTable table = new DataTable();
			table.Columns.Add("SubCode");
			table.Columns.Add("BankAmount");
			table.Columns.Add("GdNo");
			table.Columns.Add("GdValue", typeof(double));
			table.Columns.Add("FobValue", typeof(double));
			table.Columns.Add("Realized", typeof(double));
			table.Columns.Add("Status");
			table.Columns.Add("Remarks");
			table.Rows.Clear();
			if (obj.GdBreakUpBankRequestDetailList != null && obj.GdBreakUpBankRequestDetailList.Count > 0)
			{
				for (int i = 0; i < obj.GdBreakUpBankRequestDetailList.Count; i++)
				{
					table.Rows.Add(obj.GdBreakUpBankRequestDetailList[i].SubCode, obj.GdBreakUpBankRequestDetailList[i].BankReceivedAmount, obj.GdBreakUpBankRequestDetailList[i].GDNo, obj.GdBreakUpBankRequestDetailList[i].GdValue, obj.GdBreakUpBankRequestDetailList[i].FobValue, obj.GdBreakUpBankRequestDetailList[i].RealizedAmount, obj.GdBreakUpBankRequestDetailList[i].Status, obj.GdBreakUpBankRequestDetailList[i].RemarksDetail);
				}
			}
			GridDetailHistory.DataSource = table;
			GridDetailHistory.RetrieveStructure();
			GridDetailHistory.RootTable.Columns["GdValue"].TextAlignment = (TextAlignment)3;
			GridDetailHistory.RootTable.Columns["GdValue"].FormatString = "#,##0.###";
			GridDetailHistory.RootTable.Columns["FobValue"].TextAlignment = (TextAlignment)3;
			GridDetailHistory.RootTable.Columns["FobValue"].FormatString = "#,##0.###";
			GridDetailHistory.RootTable.Columns["Realized"].AggregateFunction = (AggregateFunction)2;
			GridDetailHistory.RootTable.Columns["Realized"].TextAlignment = (TextAlignment)3;
			GridDetailHistory.RootTable.Columns["Realized"].FormatString = "#,##0.###";
			GridDetailHistory.RootTable.Columns["Realized"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(GridDetailHistory);
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				GenerateSlipReport(RecId);
				return;
			}
			throw new Exception("Record Not Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateSlipReport(int ReportId)
	{
		try
		{
			CommonServices.GDBreakUpBankRequest_Slip560(ReportId);
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
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus in Detail Entry");
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

	private void GdBankRequest_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (btnSave.Visible && btnSave.Enabled && e.Control && e.KeyCode == Keys.S)
			{
				btnSave_Click(null, null);
			}
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnUpdate_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				BtnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					tabControl1.SelectedIndex = 1;
					FromDateHistory.Focus();
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					DocDate.Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P && formRights.DoHavePrintRights)
			{
				BtnPrint_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				DocDate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				btnAttachment_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdDetail).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				if (txtAmountRcvd.Enabled)
				{
					txtAmountRcvd.Focus();
				}
				else
				{
					CmbGdNo.Focus();
				}
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

	private void grdDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Expected O, but got Unknown
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdDetail.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdDetail_DoubleClick(null, null);
			}
			else
			{
				if (!e.Control || e.KeyCode != Keys.Space || !(grdDetail.CurrentColumn.Key == "Delete") || MessageBox.Show("Are you sure! You Want To Delete.Because Entire Entry Will be delete Against this?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				int DetailId = Conversion.ToInt(item.Cells["Id"].Value);
				int subCode = Conversion.ToInt(item.Cells["SubCode"].Value);
				if (dtDetail.Select("SubCode='" + Conversion.ToInt((object)subCode) + "'").Length != 0)
				{
					if (DetailId > 0)
					{
						GridEXRow[] rows = grdDetail.GetRows();
						foreach (GridEXRow r in rows)
						{
							if (Conversion.ToInt(r.Cells["SubCode"].Value) == subCode)
							{
								GDBreakUpBankRequestDetail vd = new GDBreakUpBankRequestDetail();
								vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
								vd.BankReceivedAmount = Conversion.ToDecimal(r.Cells["AmountRcvd"].Value);
								vd.GDId = Conversion.ToInt(r.Cells["GdId"].Value);
								vd.RealizedAmount = Conversion.ToDecimal(r.Cells["Realized"].Value);
								vd.CommAmount = Conversion.ToDecimal(r.Cells["FTT"].Value);
								vd.Status = Conversion.ToString(r.Cells["Status"].Value);
								vd.RemarksDetail = Conversion.ToString((object)r.Cells["Remarks"].Text);
								vd.ActionTypeId = 3;
								LstRemoveRecordDetail.Add(vd);
								dtDetail.Rows[r.RowIndex].Delete();
								if (r != null && (int)r.RowType == 0)
								{
									r.Delete();
								}
							}
						}
					}
					else
					{
						GridEXRow[] rows = grdDetail.GetRows();
						foreach (GridEXRow row in rows)
						{
							if (Conversion.ToInt(row.Cells["SubCode"].Value) == subCode)
							{
								row.Delete();
							}
						}
					}
				}
				grdDetail.UpdateData();
				if (dtDetail.Rows.Count > 0)
				{
					((Control)(object)CmbBank).Enabled = true;
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
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			int ID = 0;
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (DataGridHistory.GetRows().Count() > 0)
			{
				ID = Conversion.ToInt(item.Cells["Id"].Value);
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				Reset();
				ReadById(ID);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DataGridHistory.CurrentColumn.Key == "Edit")
				{
					Reset();
					ReadById(ID);
				}
				else if (DataGridHistory.CurrentColumn.Key == "Slip")
				{
					GenerateSlipReport(ID);
				}
				else if (DataGridHistory.CurrentColumn.Key == "Attachment")
				{
					CommonServices.GetNoofAttachmentsByScreenName(ID, base.Name);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAmountRcvd_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FromDocNo_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ToDocNo_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GdBankRequest_FormClosing(object sender, FormClosingEventArgs e)
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDetail).Name);
			if (fs != null)
			{
				grdDetail.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataGridHistory).Name);
			if (fs != null)
			{
				DataGridHistory.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GridDetailHistory).Name);
			if (fs != null)
			{
				GridDetailHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
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
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Expected O, but got Unknown
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Expected O, but got Unknown
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Expected O, but got Unknown
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Expected O, but got Unknown
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Expected O, but got Unknown
		//IL_07e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ef: Expected O, but got Unknown
		//IL_2ef4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2efe: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Export.GdBankRequest));
		GridEXLayout GridDetailHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grdDetail = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtFobValue = new System.Windows.Forms.TextBox();
		this.Fob = new System.Windows.Forms.Label();
		this.btnAdd = new System.Windows.Forms.Button();
		this.btnDetailCancel = new System.Windows.Forms.Button();
		this.btnDetailUpdate = new System.Windows.Forms.Button();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.txtStatus = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.txtFttAmount = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.txtRealizedAmount = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.txtGdValueBalance = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.txtInvoiceNo = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.txtGdValue = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbGdNo = new UltraCombo();
		this.txtAmountRcvd = new System.Windows.Forms.TextBox();
		this.label51 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtDocNo = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label20 = new System.Windows.Forms.Label();
		this.CmbBank = new UltraCombo();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.panel1 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.BtnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.BtnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.GridDetailHistory = new GridEX();
		this.label18 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.DataGridHistory = new GridEX();
		this.label17 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.btnShow = new System.Windows.Forms.Button();
		this.label16 = new System.Windows.Forms.Label();
		this.CmbBankHistory = new UltraCombo();
		this.ToDocNo = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.FromDocNo = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label12 = new System.Windows.Forms.Label();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel3.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbGdNo).BeginInit();
		this.panel2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBank).BeginInit();
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetailHistory).BeginInit();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel4.SuspendLayout();
		this.panel5.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBankHistory).BeginInit();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1015, 594);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1007, 568);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		this.grdDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(3, 236);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1001, 329);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 46;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetail_KeyDown);
		this.panel3.Controls.Add(this.groupBox2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 107);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1001, 129);
		this.panel3.TabIndex = 2;
		this.groupBox2.Controls.Add(this.txtFobValue);
		this.groupBox2.Controls.Add(this.Fob);
		this.groupBox2.Controls.Add(this.btnAdd);
		this.groupBox2.Controls.Add(this.btnDetailCancel);
		this.groupBox2.Controls.Add(this.btnDetailUpdate);
		this.groupBox2.Controls.Add(this.txtRemarksDetail);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.txtStatus);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add(this.txtFttAmount);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.txtRealizedAmount);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.txtGdValueBalance);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.txtInvoiceNo);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.txtGdValue);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGdNo);
		this.groupBox2.Controls.Add(this.txtAmountRcvd);
		this.groupBox2.Controls.Add(this.label51);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(3, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1003, 121);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Detail Information";
		this.txtFobValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFobValue.Enabled = false;
		this.txtFobValue.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFobValue.Location = new System.Drawing.Point(473, 44);
		this.txtFobValue.Name = "txtFobValue";
		this.txtFobValue.Size = new System.Drawing.Size(118, 23);
		this.txtFobValue.TabIndex = 129;
		this.txtFobValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.Fob.AutoSize = true;
		this.Fob.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.Fob.Location = new System.Drawing.Point(477, 22);
		this.Fob.Name = "Fob";
		this.Fob.Size = new System.Drawing.Size(61, 15);
		this.Fob.TabIndex = 130;
		this.Fob.Text = "FOB Value";
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(855, 87);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(36, 28);
		this.btnAdd.TabIndex = 9;
		this.btnAdd.Text = "+";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Visible = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.btnDetailCancel.AutoSize = true;
		this.btnDetailCancel.BackColor = System.Drawing.Color.Teal;
		this.btnDetailCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDetailCancel.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDetailCancel.ForeColor = System.Drawing.Color.White;
		this.btnDetailCancel.Location = new System.Drawing.Point(920, 87);
		this.btnDetailCancel.Name = "btnDetailCancel";
		this.btnDetailCancel.Size = new System.Drawing.Size(66, 28);
		this.btnDetailCancel.TabIndex = 11;
		this.btnDetailCancel.Text = "Cancel";
		this.btnDetailCancel.UseVisualStyleBackColor = false;
		this.btnDetailCancel.Click += new System.EventHandler(btnDetailCancel_Click);
		this.btnDetailUpdate.AutoSize = true;
		this.btnDetailUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnDetailUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDetailUpdate.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDetailUpdate.ForeColor = System.Drawing.Color.White;
		this.btnDetailUpdate.Location = new System.Drawing.Point(855, 87);
		this.btnDetailUpdate.Name = "btnDetailUpdate";
		this.btnDetailUpdate.Size = new System.Drawing.Size(66, 28);
		this.btnDetailUpdate.TabIndex = 10;
		this.btnDetailUpdate.Text = "Update";
		this.btnDetailUpdate.UseVisualStyleBackColor = false;
		this.btnDetailUpdate.Click += new System.EventHandler(btnDetailUpdate_Click);
		this.txtRemarksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(333, 90);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(517, 23);
		this.txtRemarksDetail.TabIndex = 8;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(335, 72);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(52, 15);
		this.label10.TabIndex = 128;
		this.label10.Text = "Remarks";
		this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtStatus.Enabled = false;
		this.txtStatus.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtStatus.Location = new System.Drawing.Point(220, 90);
		this.txtStatus.Name = "txtStatus";
		this.txtStatus.Size = new System.Drawing.Size(111, 23);
		this.txtStatus.TabIndex = 7;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(220, 72);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(40, 15);
		this.label9.TabIndex = 126;
		this.label9.Text = "Status";
		this.txtFttAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFttAmount.Enabled = false;
		this.txtFttAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFttAmount.Location = new System.Drawing.Point(119, 90);
		this.txtFttAmount.Name = "txtFttAmount";
		this.txtFttAmount.Size = new System.Drawing.Size(100, 23);
		this.txtFttAmount.TabIndex = 6;
		this.txtFttAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(123, 68);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(27, 15);
		this.label8.TabIndex = 124;
		this.label8.Text = "FTT";
		this.txtRealizedAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRealizedAmount.Enabled = false;
		this.txtRealizedAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRealizedAmount.Location = new System.Drawing.Point(7, 90);
		this.txtRealizedAmount.Name = "txtRealizedAmount";
		this.txtRealizedAmount.Size = new System.Drawing.Size(111, 23);
		this.txtRealizedAmount.TabIndex = 5;
		this.txtRealizedAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(11, 68);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(83, 15);
		this.label7.TabIndex = 122;
		this.label7.Text = "Realized Value";
		this.txtGdValueBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGdValueBalance.Enabled = false;
		this.txtGdValueBalance.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtGdValueBalance.Location = new System.Drawing.Point(851, 44);
		this.txtGdValueBalance.Name = "txtGdValueBalance";
		this.txtGdValueBalance.Size = new System.Drawing.Size(134, 23);
		this.txtGdValueBalance.TabIndex = 4;
		this.txtGdValueBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(855, 22);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(48, 15);
		this.label6.TabIndex = 120;
		this.label6.Text = "Balance";
		this.txtInvoiceNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceNo.Enabled = false;
		this.txtInvoiceNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtInvoiceNo.Location = new System.Drawing.Point(592, 44);
		this.txtInvoiceNo.Name = "txtInvoiceNo";
		this.txtInvoiceNo.Size = new System.Drawing.Size(258, 23);
		this.txtInvoiceNo.TabIndex = 3;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(596, 22);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(56, 15);
		this.label5.TabIndex = 118;
		this.label5.Text = "Invoice #";
		this.txtGdValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGdValue.Enabled = false;
		this.txtGdValue.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtGdValue.Location = new System.Drawing.Point(353, 44);
		this.txtGdValue.Name = "txtGdValue";
		this.txtGdValue.Size = new System.Drawing.Size(119, 23);
		this.txtGdValue.TabIndex = 2;
		this.txtGdValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(357, 22);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(56, 15);
		this.label4.TabIndex = 116;
		this.label4.Text = "GD Value";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(119, 25);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(34, 15);
		this.label3.TabIndex = 114;
		this.label3.Text = "GD #";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.CmbGdNo.AllowNull = (DefaultableBoolean)1;
		this.CmbGdNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGdNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGdNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGdNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance21;
		((SpecialBoxBase)((UltraGridBase)this.CmbGdNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGdNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGdNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbGdNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbGdNo).Location = new System.Drawing.Point(120, 42);
		((System.Windows.Forms.Control)(object)this.CmbGdNo).Name = "CmbGdNo";
		((System.Windows.Forms.Control)(object)this.CmbGdNo).Size = new System.Drawing.Size(231, 26);
		((System.Windows.Forms.Control)(object)this.CmbGdNo).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbGdNo).Leave += new System.EventHandler(CmbGdNo_Leave);
		this.txtAmountRcvd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmountRcvd.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAmountRcvd.Location = new System.Drawing.Point(7, 44);
		this.txtAmountRcvd.Name = "txtAmountRcvd";
		this.txtAmountRcvd.Size = new System.Drawing.Size(111, 23);
		this.txtAmountRcvd.TabIndex = 0;
		this.txtAmountRcvd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAmountRcvd.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAmountRcvd_KeyPress);
		this.txtAmountRcvd.Leave += new System.EventHandler(txtAmountRcvd_Leave);
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label51.Location = new System.Drawing.Point(7, 23);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(80, 15);
		this.label51.TabIndex = 112;
		this.label51.Text = "Amount Rcvd";
		this.panel2.Controls.Add(this.groupBox1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 31);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1001, 76);
		this.panel2.TabIndex = 1;
		this.groupBox1.Controls.Add(this.txtDocNo);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.txtRemarks);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBank);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1003, 69);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main Information";
		this.txtDocNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNo.Enabled = false;
		this.txtDocNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDocNo.Location = new System.Drawing.Point(7, 38);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.Size = new System.Drawing.Size(80, 23);
		this.txtDocNo.TabIndex = 0;
		this.txtDocNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(8, 18);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(48, 15);
		this.label11.TabIndex = 133;
		this.label11.Text = "Doc No";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(200, 19);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(68, 15);
		this.label2.TabIndex = 38;
		this.label2.Text = "Bank Name";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(89, 18);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(57, 15);
		this.label1.TabIndex = 37;
		this.label1.Text = "Doc Date";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(455, 39);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(530, 23);
		this.txtRemarks.TabIndex = 3;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(457, 19);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(52, 15);
		this.label20.TabIndex = 36;
		this.label20.Text = "Remarks";
		this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.CmbBank.AllowNull = (DefaultableBoolean)1;
		this.CmbBank.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbBank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance31;
		((SpecialBoxBase)((UltraGridBase)this.CmbBank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbBank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance9).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance10).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance10).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance10).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbBank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBank).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBank).Location = new System.Drawing.Point(201, 37);
		((System.Windows.Forms.Control)(object)this.CmbBank).Name = "CmbBank";
		((System.Windows.Forms.Control)(object)this.CmbBank).Size = new System.Drawing.Size(252, 26);
		((System.Windows.Forms.Control)(object)this.CmbBank).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbBank).Leave += new System.EventHandler(CmbBank_Leave);
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(89, 38);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(111, 23);
		this.DocDate.TabIndex = 1;
		this.panel1.Controls.Add(this.ctrlGrdBar1);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1001, 28);
		this.panel1.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(962, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdDetail;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 37;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.BtnNew, this.btnRefresh, this.btnSave, this.btnUpdate, this.BtnPrint, this.btnAttachment, this.btnShortCutKey });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1001, 27);
		this.toolStrip1.TabIndex = 2;
		this.toolStrip1.Text = "toolStrip1";
		this.BtnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNew.Name = "BtnNew";
		this.BtnNew.Size = new System.Drawing.Size(59, 24);
		this.BtnNew.Text = "&New";
		this.BtnNew.Click += new System.EventHandler(BtnNew_Click);
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
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.BtnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint.Name = "BtnPrint";
		this.BtnPrint.Size = new System.Drawing.Size(92, 24);
		this.BtnPrint.Text = "Print-560";
		this.BtnPrint.Click += new System.EventHandler(BtnPrint_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnShortCutKey.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShortCutKey.Image = (System.Drawing.Image)resources.GetObject("btnShortCutKey.Image");
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(106, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.tabPage2.Controls.Add(this.ctrlGrdBar3);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetailHistory);
		this.tabPage2.Controls.Add(this.label18);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Controls.Add(this.panel4);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1007, 568);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(964, 371);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.GridDetailHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 49;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.GridDetailHistory.AllowEdit = (InheritableBoolean)2;
		this.GridDetailHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		GridDetailHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridDetailHistory.DesignTimeLayout = GridDetailHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridDetailHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridDetailHistory.DynamicFiltering = true;
		this.GridDetailHistory.FilterMode = (FilterMode)2;
		this.GridDetailHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridDetailHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridDetailHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GridDetailHistory).Location = new System.Drawing.Point(3, 393);
		((System.Windows.Forms.Control)(object)this.GridDetailHistory).Name = "GridDetailHistory";
		((ScrollableBase)this.GridDetailHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridDetailHistory).Size = new System.Drawing.Size(1001, 172);
		((System.Windows.Forms.Control)(object)this.GridDetailHistory).TabIndex = 0;
		this.GridDetailHistory.TotalRow = (InheritableBoolean)1;
		this.GridDetailHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridDetailHistory).VisualStyle = (VisualStyle)4;
		this.label18.BackColor = System.Drawing.Color.Teal;
		this.label18.Dock = System.Windows.Forms.DockStyle.Top;
		this.label18.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(3, 371);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(1001, 22);
		this.label18.TabIndex = 2;
		this.label18.Text = "Detail Information of Above Selected Row";
		this.panel6.Controls.Add(this.ctrlGrdBar2);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel6.Controls.Add(this.label17);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 98);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1001, 273);
		this.panel6.TabIndex = 1;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(962, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 48;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1001, 251);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.label17.BackColor = System.Drawing.Color.Teal;
		this.label17.Dock = System.Windows.Forms.DockStyle.Top;
		this.label17.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(0, 0);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(1001, 22);
		this.label17.TabIndex = 0;
		this.label17.Text = "Filtered Records";
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Controls.Add(this.groupBox3);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 3);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1001, 95);
		this.panel4.TabIndex = 0;
		this.panel5.Controls.Add(this.toolStrip2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1001, 28);
		this.panel5.TabIndex = 1;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.btnRefreshHistory, this.toolStripButton5 });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1001, 27);
		this.toolStrip2.TabIndex = 3;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.toolStripButton5.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton5.Name = "toolStripButton5";
		this.toolStripButton5.Size = new System.Drawing.Size(61, 24);
		this.toolStripButton5.Text = "Print";
		this.groupBox3.Controls.Add(this.rdapproveddate);
		this.groupBox3.Controls.Add(this.rdmodifydate);
		this.groupBox3.Controls.Add(this.drdocdate);
		this.groupBox3.Controls.Add(this.rdentrydate);
		this.groupBox3.Controls.Add(this.btnShow);
		this.groupBox3.Controls.Add(this.label16);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBankHistory);
		this.groupBox3.Controls.Add(this.ToDocNo);
		this.groupBox3.Controls.Add(this.label15);
		this.groupBox3.Controls.Add(this.FromDocNo);
		this.groupBox3.Controls.Add(this.label14);
		this.groupBox3.Controls.Add(this.label13);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.label12);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(4, 29);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(992, 64);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.btnShow.AutoSize = true;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(741, 33);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(54, 28);
		this.btnShow.TabIndex = 5;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(477, 18);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(68, 15);
		this.label16.TabIndex = 139;
		this.label16.Text = "Bank Name";
		this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.CmbBankHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbBankHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBankHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBankHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBankHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.CmbBankHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance3).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBankHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBankHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBankHistory).Location = new System.Drawing.Point(478, 34);
		((System.Windows.Forms.Control)(object)this.CmbBankHistory).Name = "CmbBankHistory";
		((System.Windows.Forms.Control)(object)this.CmbBankHistory).Size = new System.Drawing.Size(262, 26);
		((System.Windows.Forms.Control)(object)this.CmbBankHistory).TabIndex = 4;
		this.ToDocNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.ToDocNo.Location = new System.Drawing.Point(390, 36);
		this.ToDocNo.Name = "ToDocNo";
		this.ToDocNo.Size = new System.Drawing.Size(87, 23);
		this.ToDocNo.TabIndex = 3;
		this.ToDocNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.ToDocNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNo_KeyPress);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(391, 18);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(64, 15);
		this.label15.TabIndex = 137;
		this.label15.Text = "To Doc No";
		this.FromDocNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.FromDocNo.Location = new System.Drawing.Point(302, 36);
		this.FromDocNo.Name = "FromDocNo";
		this.FromDocNo.Size = new System.Drawing.Size(87, 23);
		this.FromDocNo.TabIndex = 2;
		this.FromDocNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.FromDocNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNo_KeyPress);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(303, 18);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(79, 15);
		this.label14.TabIndex = 135;
		this.label14.Text = "From Doc No";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(154, 18);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(48, 15);
		this.label13.TabIndex = 41;
		this.label13.Text = "To Date";
		this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(154, 36);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(145, 23);
		this.ToDateHistory.TabIndex = 1;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(6, 18);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(63, 15);
		this.label12.TabIndex = 39;
		this.label12.Text = "From Date";
		this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(6, 36);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(145, 23);
		this.FromDateHistory.TabIndex = 0;
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(886, 38);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(886, 16);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(805, 16);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(805, 38);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1015, 594);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "GdBankRequest";
		this.Text = "GdBankRequest";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(GdBankRequest_FormClosing);
		base.Load += new System.EventHandler(GdBankRequest_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(GdBankRequest_KeyDown);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel3.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbGdNo).EndInit();
		this.panel2.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBank).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridDetailHistory).EndInit();
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBankHistory).EndInit();
		base.ResumeLayout(false);
	}
}

}
