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
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Definition;

public class ExchangeRateForwardBooking : Form
{
	private Attachment AT = new Attachment();

	private Rightsobjects formright = new Rightsobjects();

	private DataTable table = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private int RecId;

	private int DefaultDaysToLessFromHistoryFromDate;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Label label2;

	private ToolStripButton btnsave;

	private ToolStripButton btnnew;

	private Panel panel1;

	private Panel panel5;

	internal UltraCombo CmbBank;

	private Label label1;

	private Panel panel3;

	private Panel panel6;

	private Label label23;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private Label label3;

	internal UltraCombo CmbBranch;

	private Panel panel4;

	internal GridEX grdcheq;

	private ToolStripButton btnUpdate;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton btnAttachment;

	private ToolStripButton btnShortCutKey;

	private Label label5;

	private ToolStripButton btnRefresh;

	private TextBox txtContractRef;

	private Label label6;

	private DateTimePicker datDocDate;

	private Label label4;

	private TextBox txtHcyAmount;

	private TextBox txtFcyAmount;

	internal UltraCombo CmbHcyCode;

	private Label label7;

	internal UltraCombo CmbFcyCode;

	private Label lblFcyCode;

	private DateTimePicker datFixedMaturity;

	private Label label9;

	private Label label8;

	private DateTimePicker datStartOfOptionalPeriod;

	private Label label10;

	private Label label11;

	private TextBox txtOptionalDays;

	private TextBox txtFixedDays;

	private Label label13;

	private TextBox txtTotalDays;

	private DateTimePicker datFinalMaturity;

	private Label label12;

	private TextBox txtBookingRate;

	private Label label16;

	private Label label15;

	private Label label14;

	private GroupBox groupBox1;

	private Label label17;

	private TextBox txtDocNo;

	private Panel panel7;

	private GroupBox groupBox2;

	private Button btnShow;

	private Label label50;

	private Label label51;

	private RadioButton rdmodifydate;

	private RadioButton rdentrydate;

	private Label label18;

	private Label label52;

	private RadioButton drdocdate;

	private Label label53;

	private Label label54;

	internal UltraCombo CmbBankHistory;

	internal UltraCombo CmbBranchHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Panel panel8;

	private ToolStripButton btnPrint;

	public ExchangeRateForwardBooking(UserAccount user)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void DefineCity_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1120, 890);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			GenerateDocNo();
			BankComboFill();
			BranchFill();
			MultiCurrencyfill();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			CmbBank.Focus();
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			HistoryCombosFill();
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDateHistory.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateDocNo()
	{
		try
		{
			int Code = fcyExRateForwardBooking.GenerateCode(UserAccount.OrganizationId, UserAccount.CompanyId, clsGlobalVariables.ActiveYr.Id);
			if (Code > 0)
			{
				txtDocNo.Text = Code.ToString();
			}
			else
			{
				txtDocNo.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BankComboFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(CmbBank.Value);
			Bank val = new Bank
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dt = new DataTable
			{
				Columns = 
				{
					{
						"Id",
						typeof(int)
					},
					"BranchName"
				}
			};
			DataTable dt2 = Bank.GetAllDt(val);
			if (dt2.Rows.Count > 0)
			{
				foreach (var item in from x in dt2.AsEnumerable()
					where x.Field<string>("IsHomeland") == "Home Country"
					select new
					{
						Id = x.Field<int>("Id"),
						BranchName = x.Field<string>("BranchName")
					})
				{
					DataRow newRow = dt.NewRow();
					newRow["Id"] = item.Id;
					newRow["BranchName"] = item.BranchName;
					dt.Rows.Add(newRow);
				}
			}
			DDL.BindDDL(dt, CmbBank, "Id", "BranchName", "Bank A/c", ZeroIndex: true);
			if (Id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					CmbBank.Value = Id;
				}
				else
				{
					((Control)(object)CmbBank).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchFill()
	{
		try
		{
			DataTable dt = CommonServices.BrancheServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbBranch, "Id", "BranchName", "Client", true);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MultiCurrencyfill()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		try
		{
			int id = Conversion.ToInt(CmbFcyCode.Value);
			int HcyId = Conversion.ToInt(CmbHcyCode.Value);
			DataTable dt = MultiCurrency.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DDL.BindDDLNew(dt, CmbFcyCode, "Id", "CurrencyCode", "Fcy Code", true);
			DDL.BindDDLNew(dt, CmbHcyCode, "Id", "CurrencyCode", "Hcy Code", true);
			if (id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					CmbFcyCode.Value = id;
				}
				else
				{
					((Control)(object)CmbFcyCode).Text = string.Empty;
				}
			}
			if (HcyId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == HcyId))
				{
					CmbHcyCode.Value = HcyId;
				}
				else
				{
					((Control)(object)CmbHcyCode).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryCombosFill()
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		try
		{
			DataTable dtBank = new DataTable();
			dtBank.Columns.Add("Id", typeof(int));
			dtBank.Columns.Add("name", typeof(string));
			DataTable dtBranch = new DataTable();
			dtBranch.Columns.Add("Id", typeof(int));
			dtBranch.Columns.Add("name", typeof(string));
			DataTable dt = fcyExRateForwardBooking.GetDataForDropDownFromExRateForwardBooking(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["ActivityType"]) == "Bank")
					{
						dtBank.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["ActivityType"]) == "Branches")
					{
						dtBranch.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
					}
				}
			}
			DDL.BindDDL(dtBank, CmbBankHistory, "Id", "name", "Bank A/C", ZeroIndex: false);
			DDL.BindDDL(dtBranch, CmbBranchHistory, "Id", "name", "Client", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			RecId = 0;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			CmbBank.Focus();
			txtDocNo.Text = string.Empty;
			((Control)(object)CmbBank).Text = string.Empty;
			txtContractRef.Text = string.Empty;
			((Control)(object)CmbBranch).Text = string.Empty;
			((Control)(object)CmbFcyCode).Text = string.Empty;
			txtFcyAmount.Text = string.Empty;
			txtBookingRate.Text = string.Empty;
			((Control)(object)CmbHcyCode).Text = string.Empty;
			txtHcyAmount.Text = string.Empty;
			txtFixedDays.Text = string.Empty;
			txtOptionalDays.Text = string.Empty;
			txtTotalDays.Text = string.Empty;
			GenerateDocNo();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		try
		{
			Close();
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
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CheqbookStatus_FormClosing(object sender, FormClosingEventArgs e)
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
			BankComboFill();
			BranchFill();
			MultiCurrencyfill();
			HistoryCombosFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtDocNo.Text.Trim() == string.Empty || Conversion.ToInt((object)txtDocNo.Text) == 0)
		{
			MessageBox.Show("DocNo Field Required");
			txtDocNo.Focus();
			return false;
		}
		if (((Control)(object)CmbBank).Text.Trim() == string.Empty || Conversion.ToInt(CmbBank.Value) == 0)
		{
			MessageBox.Show("Bank Field Required");
			CmbBank.Focus();
			return false;
		}
		if (txtContractRef.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Contract Ref No Field Required");
			txtContractRef.Focus();
			return false;
		}
		if (((Control)(object)CmbBranch).Text.Trim() == string.Empty || Conversion.ToInt(CmbBranch.Value) == 0)
		{
			MessageBox.Show("Client Field Required");
			CmbBranch.Focus();
			return false;
		}
		if (((Control)(object)CmbFcyCode).Text.Trim() == string.Empty || Conversion.ToInt(CmbFcyCode.Value) == 0)
		{
			MessageBox.Show("Fcy Code Field Required");
			CmbFcyCode.Focus();
			return false;
		}
		if (txtFcyAmount.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtFcyAmount.Text) == 0.0)
		{
			MessageBox.Show("Fcy Amount Field Required");
			txtFcyAmount.Focus();
			return false;
		}
		if (txtBookingRate.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtBookingRate.Text) == 0.0)
		{
			MessageBox.Show("Booking Rate Field Required");
			txtBookingRate.Focus();
			return false;
		}
		if (((Control)(object)CmbHcyCode).Text.Trim() == string.Empty || Conversion.ToInt(CmbHcyCode.Value) == 0)
		{
			MessageBox.Show("Hcy Code Field Required");
			CmbHcyCode.Focus();
			return false;
		}
		if (txtHcyAmount.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtHcyAmount.Text) == 0.0)
		{
			MessageBox.Show("Hcy Amount Field Required");
			txtHcyAmount.Focus();
			return false;
		}
		if (txtFixedDays.Text.Trim() == string.Empty || Conversion.ToInt((object)txtFixedDays.Text) == 0)
		{
			MessageBox.Show("Tenor Fixed Days Field Required");
			txtFixedDays.Focus();
			return false;
		}
		if (txtOptionalDays.Text.Trim() == string.Empty || Conversion.ToInt((object)txtOptionalDays.Text) == 0)
		{
			MessageBox.Show("Tenor Optional Days Field Required");
			txtOptionalDays.Focus();
			return false;
		}
		if (txtTotalDays.Text.Trim() == string.Empty || Conversion.ToInt((object)txtTotalDays.Text) == 0)
		{
			MessageBox.Show("Tenor Total Days Field Required");
			txtTotalDays.Focus();
			return false;
		}
		return true;
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
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
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
			fcyExRateForwardBooking obj = new fcyExRateForwardBooking();
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
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.EntryUserId = UserAccount.ID;
			obj.EntryDate = DateTime.Now;
			obj.ModifyUserId = UserAccount.ID;
			obj.ModifyDate = DateTime.Now;
			obj.ScreenName = base.Name;
			obj.DocNo = Conversion.ToInt((object)txtDocNo.Text);
			obj.BankId = Conversion.ToInt(CmbBank.Value);
			obj.BankContractNo = txtContractRef.Text;
			obj.DocDate = datDocDate.Value;
			obj.DateOfPurchase = datDocDate.Value;
			obj.BranchId = Conversion.ToInt(CmbBranch.Value);
			obj.FcyId = Conversion.ToInt(CmbFcyCode.Value);
			obj.FcyAmount = Conversion.ToDecimal((object)txtFcyAmount.Text);
			obj.HcyId = Conversion.ToInt(CmbHcyCode.Value);
			obj.HcyAmount = Conversion.ToDecimal((object)txtHcyAmount.Text);
			obj.BookingRate = Conversion.ToDecimal((object)txtBookingRate.Text);
			obj.FixedDays = Conversion.ToInt((object)txtFixedDays.Text);
			obj.FirstMaturityDate = Conversion.ToDateTime((object)datFixedMaturity.Value);
			obj.OptionalDays = Conversion.ToInt((object)txtOptionalDays.Text);
			obj.OptionalPeriod = Conversion.ToDateTime((object)datStartOfOptionalPeriod.Value);
			obj.FinalMaturityDate = Conversion.ToDateTime((object)datFinalMaturity.Value);
			obj.TotalDays = Conversion.ToInt((object)txtTotalDays.Text);
			obj.AttachmentsList = new List<DMSAttachments>();
			obj.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<fcyExRateForwardBooking>(obj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			fcyExRateForwardBooking.Save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + obj.DocNo);
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					DMSAttachments.RemoveByIdAndNames(RecId, base.Name, 0, 0);
				}
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + obj.DocNo);
			}
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("RecId not found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById(int Id)
	{
		try
		{
			RecId = Id;
			fcyExRateForwardBooking obj = fcyExRateForwardBooking.GetById(RecId);
			if (obj == null)
			{
				return;
			}
			txtDocNo.Text = obj.DocNo.ToString();
			CmbBank.Value = obj.BankId;
			txtContractRef.Text = obj.BankContractNo;
			datDocDate.Value = obj.DocDate;
			CmbBranch.Value = obj.BranchId;
			CmbFcyCode.Value = obj.FcyId;
			txtFcyAmount.Text = obj.FcyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
			txtBookingRate.Text = obj.BookingRate.ToString(clsGlobalVariables.DecimalRateFormate);
			CmbHcyCode.Value = obj.FcyId;
			txtHcyAmount.Text = obj.FcyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
			txtFixedDays.Text = obj.FixedDays.ToString();
			datFixedMaturity.Value = obj.FirstMaturityDate;
			txtOptionalDays.Text = obj.OptionalDays.ToString();
			datStartOfOptionalPeriod.Value = obj.FirstMaturityDate;
			datFinalMaturity.Value = obj.FirstMaturityDate;
			txtTotalDays.Text = obj.TotalDays.ToString();
			List<DMSAttachments> byID = DMSAttachments.GetByID(RecId, base.Name);
			AT = new Attachment();
			foreach (DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.ActionId = 2;
				TA.Path = Attachment.Attachment;
				TA.CustomFileName = Attachment.UploadedFileCustomName;
				AT.lst.Add(TA);
			}
			CmbBank.Focus();
			btnUpdate.Visible = true;
			btnsave.Visible = false;
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
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
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
			obj.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			obj.BankId = Conversion.ToInt(CmbBankHistory.Value);
			obj.BranchesId = Conversion.ToInt(CmbBranchHistory.Value);
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			DataTable dtGrid = fcyExRateForwardBooking.FormHistory(obj);
			if (dtGrid.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("BankName", typeof(string));
				dt.Columns.Add("ContractRefNo", typeof(string));
				dt.Columns.Add("DealDate", typeof(DateTime));
				dt.Columns.Add("Client", typeof(string));
				dt.Columns.Add("FcyCode", typeof(string));
				dt.Columns.Add("FcyAmount", typeof(double));
				dt.Columns.Add("BookingRate", typeof(double));
				dt.Columns.Add("HcyCode", typeof(string));
				dt.Columns.Add("HcyAmount", typeof(double));
				dt.Columns.Add("FixedDays", typeof(int));
				dt.Columns.Add("FixedMaturity", typeof(DateTime));
				dt.Columns.Add("OptionalDays", typeof(int));
				dt.Columns.Add("StartOfOptianlPeriod", typeof(DateTime));
				dt.Columns.Add("FinalMaturity", typeof(DateTime));
				dt.Columns.Add("TotalDays", typeof(int));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				foreach (DataRow row in dtGrid.Rows)
				{
					dt.Rows.Add(row["Id"], row["DocNo"], row["BankName"], row["BankContractNo"], Conversion.ToDateTime(row["DocDate"]).ToShortDateString(), row["BranchName"], row["FcyCode"], row["FcyAmount"], row["BookingRate"], row["HcyCode"], row["HcyAmount"], row["FixedDays"], Conversion.ToDateTime(row["FirstMaturityDate"]).ToShortDateString(), row["OptionalDays"], Conversion.ToDateTime(row["OptionalPeriod"]).ToShortDateString(), Conversion.ToDateTime(row["FinalMaturityDate"]).ToShortDateString(), row["TotalDays"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["NoOfAttachments"]);
				}
				grdcheq.DataSource = dt;
				grdcheq.RetrieveStructure();
				GrdHistorySetting();
			}
			else
			{
				grdcheq.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistorySetting()
	{
		try
		{
			grdcheq.RootTable.Columns["Id"].Visible = false;
			grdcheq.RootTable.Columns["BankName"].Width = 100;
			grdcheq.RootTable.Columns["ContractRefNo"].Width = 110;
			grdcheq.RootTable.Columns["Client"].Width = 120;
			grdcheq.RootTable.Columns["DocNo"].Width = 60;
			grdcheq.RootTable.Columns["DealDate"].Width = 70;
			grdcheq.RootTable.Columns["FcyCode"].Width = 70;
			grdcheq.RootTable.Columns["FcyAmount"].Width = 90;
			grdcheq.RootTable.Columns["BookingRate"].Width = 90;
			grdcheq.RootTable.Columns["HcyCode"].Width = 70;
			grdcheq.RootTable.Columns["HcyAmount"].Width = 90;
			grdcheq.RootTable.Columns["FixedDays"].Width = 80;
			grdcheq.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdcheq.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdcheq.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdcheq.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdcheq.RootTable.Columns["BookingRate"].TextAlignment = (TextAlignment)3;
			grdcheq.RootTable.Columns["BookingRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdcheq.RootTable.Columns["HcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdcheq.RootTable.Columns["HcyAmount"].TextAlignment = (TextAlignment)3;
			grdcheq.RootTable.Columns["HcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdcheq.RootTable.Columns["HcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdcheq.RootTable.Columns["FixedDays"].AggregateFunction = (AggregateFunction)2;
			grdcheq.RootTable.Columns["FixedDays"].TextAlignment = (TextAlignment)3;
			grdcheq.RootTable.Columns["FixedDays"].FormatString = "#,##0.###";
			grdcheq.RootTable.Columns["FixedDays"].TotalFormatString = "#,##0.###";
			grdcheq.RootTable.Columns["OptionalDays"].AggregateFunction = (AggregateFunction)2;
			grdcheq.RootTable.Columns["OptionalDays"].TextAlignment = (TextAlignment)3;
			grdcheq.RootTable.Columns["OptionalDays"].FormatString = "#,##0.###";
			grdcheq.RootTable.Columns["OptionalDays"].TotalFormatString = "#,##0.###";
			grdcheq.RootTable.Columns["TotalDays"].AggregateFunction = (AggregateFunction)2;
			grdcheq.RootTable.Columns["TotalDays"].TextAlignment = (TextAlignment)3;
			grdcheq.RootTable.Columns["TotalDays"].FormatString = "#,##0.###";
			grdcheq.RootTable.Columns["TotalDays"].TotalFormatString = "#,##0.###";
			grdcheq.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdcheq.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdcheq.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdcheq.RootTable.Columns.Add("Slip");
			grdcheq.RootTable.Columns["Slip"].Caption = "Slip";
			grdcheq.RootTable.Columns["Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdcheq.RootTable.Columns["Slip"].ButtonStyle = (ButtonStyle)5;
			grdcheq.RootTable.Columns["Slip"].ButtonText = "Slip";
			grdcheq.RootTable.Columns["Slip"].Width = 40;
			grdcheq.RootTable.Columns["Slip"].Position = 0;
			grdcheq.RootTable.Columns.Add("Edit");
			grdcheq.RootTable.Columns["Edit"].Caption = "Edit";
			grdcheq.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdcheq.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdcheq.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdcheq.RootTable.Columns["Edit"].Width = 40;
			grdcheq.RootTable.Columns["Edit"].Position = 2;
			grdcheq.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdcheq_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdcheq.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int CurrId = Conversion.ToInt(item.Cells["Id"].Value);
				ReadById(CurrId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdcheq_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdcheq.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByScreenName(Conversion.ToInt(item.Cells["Id"].Value), base.Name);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdcheq).Name);
			if (fs != null)
			{
				grdcheq.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
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

	private void DefineCity_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.Shift && e.KeyCode == Keys.S)
			{
				btnShow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
			{
				btnUpdate_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				CmbBank.Focus();
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				AT.Show();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdcheq).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				CmbBank.Focus();
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
			dt.Rows.Add("Ctrl+F5", "For Focus on Bank Account");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
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

	private void grdcheq_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdcheq.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int CurrId = Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Return)
			{
				ReadById(CurrId);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdcheq.CurrentColumn.Key == "Slip")
				{
					CommonServices.FcyExchangeRateForwardingSlip(CurrId);
				}
				else if (grdcheq.CurrentColumn.Key == "Edit")
				{
					Reset();
					ReadById(CurrId);
				}
				if (grdcheq.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByScreenName(CurrId, base.Name);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFcyAmount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtBookingRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFixedDays_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtOptionalDays_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtBookingRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			calculateHcyAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void calculateHcyAmount()
	{
		try
		{
			double FcyAmount = Conversion.ToDouble((object)txtFcyAmount.Text);
			double bookingRate = Conversion.ToDouble((object)txtBookingRate.Text);
			double HcyAmount = 0.0;
			if (FcyAmount > 0.0 && bookingRate > 0.0)
			{
				txtHcyAmount.Text = (FcyAmount * bookingRate).ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtHcyAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFcyAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			calculateHcyAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFixedDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateFixedMaturity();
			CalculateTotalDays();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void datDocDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateFixedMaturity();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOptionalDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateFinalMaturity();
			CalculateTotalDays();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void datFixedMaturity_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateFinalMaturity();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateFixedMaturity()
	{
		try
		{
			if (txtFixedDays.Text.Trim() != string.Empty && txtFixedDays.Text != null)
			{
				datFixedMaturity.Value = datDocDate.Value.AddDays(Conversion.ToDouble((object)txtFixedDays.Text.Trim()));
				datStartOfOptionalPeriod.Value = datDocDate.Value.AddDays(Conversion.ToDouble((object)txtFixedDays.Text.Trim()));
			}
			else
			{
				datFixedMaturity.Value = datDocDate.Value;
				datStartOfOptionalPeriod.Value = datDocDate.Value;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateFinalMaturity()
	{
		try
		{
			if (txtOptionalDays.Text.Trim() != string.Empty && txtOptionalDays.Text != null)
			{
				datFinalMaturity.Value = datFixedMaturity.Value.AddDays(Conversion.ToDouble((object)txtOptionalDays.Text.Trim()));
			}
			else
			{
				datFinalMaturity.Value = datFixedMaturity.Value;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateTotalDays()
	{
		try
		{
			double FixedDays = Conversion.ToDouble((object)txtFixedDays.Text);
			double optionalDays = Conversion.ToDouble((object)txtOptionalDays.Text);
			if (FixedDays > 0.0 || optionalDays > 0.0)
			{
				txtTotalDays.Text = (FixedDays + optionalDays).ToString();
			}
			else
			{
				txtTotalDays.Text = "0";
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
			GridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void grdcheq_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdcheq.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Slip")
				{
					CommonServices.FcyExchangeRateForwardingSlip(Conversion.ToInt(r.Cells["Id"].Value));
				}
				if (e.Column.Key == "Edit")
				{
					Reset();
					RecId = Conversion.ToInt(r.Cells["Id"].Value);
					ReadById(RecId);
				}
			}
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
			CommonServices.FcyExchangeRateForwardingSlip(RecId);
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
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Expected O, but got Unknown
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Expected O, but got Unknown
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Expected O, but got Unknown
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Expected O, but got Unknown
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Expected O, but got Unknown
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Expected O, but got Unknown
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Expected O, but got Unknown
		//IL_0883: Unknown result type (might be due to invalid IL or missing references)
		//IL_088d: Expected O, but got Unknown
		//IL_089a: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a4: Expected O, but got Unknown
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Definition.ExchangeRateForwardBooking));
		this.label2 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdcheq = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.btnShow = new System.Windows.Forms.Button();
		this.label50 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.label18 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.label53 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.CmbBankHistory = new UltraCombo();
		this.CmbBranchHistory = new UltraCombo();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label5 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label17 = new System.Windows.Forms.Label();
		this.txtDocNo = new System.Windows.Forms.TextBox();
		this.txtHcyAmount = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.CmbBank = new UltraCombo();
		this.txtBookingRate = new System.Windows.Forms.TextBox();
		this.CmbBranch = new UltraCombo();
		this.txtFixedDays = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.txtContractRef = new System.Windows.Forms.TextBox();
		this.txtTotalDays = new System.Windows.Forms.TextBox();
		this.datDocDate = new System.Windows.Forms.DateTimePicker();
		this.datFinalMaturity = new System.Windows.Forms.DateTimePicker();
		this.lblFcyCode = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.CmbFcyCode = new UltraCombo();
		this.datStartOfOptionalPeriod = new System.Windows.Forms.DateTimePicker();
		this.label7 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.CmbHcyCode = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.txtOptionalDays = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.datFixedMaturity = new System.Windows.Forms.DateTimePicker();
		this.label8 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdcheq).BeginInit();
		this.panel7.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBankHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBranchHistory).BeginInit();
		this.panel4.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBank).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBranch).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFcyCode).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbHcyCode).BeginInit();
		this.panel3.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(599, 18);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(37, 15);
		this.label2.TabIndex = 1;
		this.label2.Text = "Client";
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.panel8);
		this.panel1.Controls.Add(this.panel7);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1034, 616);
		this.panel1.TabIndex = 2;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdcheq);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 277);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1034, 339);
		this.panel8.TabIndex = 4;
		this.grdcheq.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdcheq).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdcheq.FilterMode = (FilterMode)2;
		this.grdcheq.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdcheq).Font = new System.Drawing.Font("Verdana", 8.25f);
		((System.Windows.Forms.Control)(object)this.grdcheq).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdcheq).Name = "grdcheq";
		((ScrollableBase)this.grdcheq).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdcheq).Size = new System.Drawing.Size(1034, 339);
		((System.Windows.Forms.Control)(object)this.grdcheq).TabIndex = 0;
		this.grdcheq.TotalRow = (InheritableBoolean)1;
		this.grdcheq.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdcheq).VisualStyle = (VisualStyle)4;
		this.grdcheq.ColumnButtonClick += new ColumnActionEventHandler(grdcheq_ColumnButtonClick);
		this.grdcheq.LinkClicked += new ColumnActionEventHandler(grdcheq_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdcheq).DoubleClick += new System.EventHandler(grdcheq_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdcheq).KeyDown += new System.Windows.Forms.KeyEventHandler(grdcheq_KeyDown);
		this.panel7.Controls.Add(this.groupBox2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 209);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1034, 68);
		this.panel7.TabIndex = 0;
		this.groupBox2.Controls.Add(this.btnShow);
		this.groupBox2.Controls.Add(this.label50);
		this.groupBox2.Controls.Add(this.label51);
		this.groupBox2.Controls.Add(this.rdmodifydate);
		this.groupBox2.Controls.Add(this.rdentrydate);
		this.groupBox2.Controls.Add(this.label18);
		this.groupBox2.Controls.Add(this.label52);
		this.groupBox2.Controls.Add(this.drdocdate);
		this.groupBox2.Controls.Add(this.label53);
		this.groupBox2.Controls.Add(this.label54);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBankHistory);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranchHistory);
		this.groupBox2.Controls.Add(this.ToDocNoHistory);
		this.groupBox2.Controls.Add(this.FromDocNoHistory);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(0, 0);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1020, 68);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(864, 37);
		this.btnShow.Margin = new System.Windows.Forms.Padding(2);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(58, 25);
		this.btnShow.TabIndex = 6;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(437, 17);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(56, 15);
		this.label50.TabIndex = 87;
		this.label50.Text = "Bank A/C";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(357, 17);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(64, 15);
		this.label51.TabIndex = 86;
		this.label51.Text = "To Doc No";
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(927, 48);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(927, 29);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(640, 17);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(37, 15);
		this.label18.TabIndex = 89;
		this.label18.Text = "Client";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(277, 17);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(79, 15);
		this.label52.TabIndex = 85;
		this.label52.Text = "From Doc No";
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(927, 10);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 7;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(142, 17);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(48, 15);
		this.label53.TabIndex = 84;
		this.label53.Text = "To Date";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(6, 17);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(63, 15);
		this.label54.TabIndex = 83;
		this.label54.Text = "From Date";
		this.CmbBankHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbBankHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBankHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBankHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBankHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.CmbBankHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance68).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBankHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBankHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBankHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBankHistory).Location = new System.Drawing.Point(437, 36);
		((System.Windows.Forms.Control)(object)this.CmbBankHistory).Name = "CmbBankHistory";
		((System.Windows.Forms.Control)(object)this.CmbBankHistory).Size = new System.Drawing.Size(202, 26);
		((System.Windows.Forms.Control)(object)this.CmbBankHistory).TabIndex = 4;
		this.CmbBranchHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbBranchHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranchHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranchHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranchHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbBranchHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranchHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranchHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranchHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance53;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranchHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranchHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbBranchHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranchHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranchHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranchHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbBranchHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranchHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranchHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance62).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranchHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbBranchHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranchHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance63).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance63).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance63).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranchHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranchHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbBranchHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranchHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranchHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbBranchHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranchHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbBranchHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranchHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranchHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranchHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBranchHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBranchHistory).Location = new System.Drawing.Point(640, 36);
		((System.Windows.Forms.Control)(object)this.CmbBranchHistory).Name = "CmbBranchHistory";
		((System.Windows.Forms.Control)(object)this.CmbBranchHistory).Size = new System.Drawing.Size(224, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranchHistory).TabIndex = 5;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(357, 38);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(277, 38);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(142, 38);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(134, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(6, 38);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(134, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 182);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1034, 27);
		this.panel4.TabIndex = 3;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(993, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdcheq;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(5, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(67, 19);
		this.label5.TabIndex = 26;
		this.label5.Text = "History";
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.groupBox1);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 56);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1034, 126);
		this.panel5.TabIndex = 0;
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.txtDocNo);
		this.groupBox1.Controls.Add(this.txtHcyAmount);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBank);
		this.groupBox1.Controls.Add(this.txtBookingRate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranch);
		this.groupBox1.Controls.Add(this.txtFixedDays);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.txtContractRef);
		this.groupBox1.Controls.Add(this.txtTotalDays);
		this.groupBox1.Controls.Add(this.datDocDate);
		this.groupBox1.Controls.Add(this.datFinalMaturity);
		this.groupBox1.Controls.Add(this.lblFcyCode);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFcyCode);
		this.groupBox1.Controls.Add(this.datStartOfOptionalPeriod);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbHcyCode);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.txtFcyAmount);
		this.groupBox1.Controls.Add(this.txtOptionalDays);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.datFixedMaturity);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1017, 115);
		this.groupBox1.TabIndex = 32;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Information";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(6, 17);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(48, 15);
		this.label17.TabIndex = 33;
		this.label17.Text = "Doc No";
		this.txtDocNo.Enabled = false;
		this.txtDocNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNo.Location = new System.Drawing.Point(6, 38);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.Size = new System.Drawing.Size(97, 23);
		this.txtDocNo.TabIndex = 0;
		this.txtDocNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtHcyAmount.Enabled = false;
		this.txtHcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtHcyAmount.Location = new System.Drawing.Point(200, 85);
		this.txtHcyAmount.Name = "txtHcyAmount";
		this.txtHcyAmount.Size = new System.Drawing.Size(125, 23);
		this.txtHcyAmount.TabIndex = 9;
		this.txtHcyAmount.Text = "0";
		this.txtHcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(5, 65);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(77, 15);
		this.label16.TabIndex = 31;
		this.label16.Text = "Booking Rate";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(104, 18);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(56, 15);
		this.label1.TabIndex = 1;
		this.label1.Text = "Bank A/C";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(197, 64);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(75, 15);
		this.label15.TabIndex = 30;
		this.label15.Text = "Hcy Amount";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(915, 18);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(72, 15);
		this.label14.TabIndex = 29;
		this.label14.Text = "Fcy Amount";
		this.CmbBank.AllowNull = (DefaultableBoolean)1;
		this.CmbBank.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbBank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbBank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbBank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbBank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBank).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBank).Location = new System.Drawing.Point(104, 36);
		((System.Windows.Forms.Control)(object)this.CmbBank).Name = "CmbBank";
		((System.Windows.Forms.Control)(object)this.CmbBank).Size = new System.Drawing.Size(221, 26);
		((System.Windows.Forms.Control)(object)this.CmbBank).TabIndex = 1;
		this.txtBookingRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBookingRate.Location = new System.Drawing.Point(5, 85);
		this.txtBookingRate.Name = "txtBookingRate";
		this.txtBookingRate.Size = new System.Drawing.Size(98, 23);
		this.txtBookingRate.TabIndex = 7;
		this.txtBookingRate.Text = "0";
		this.txtBookingRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBookingRate.TextChanged += new System.EventHandler(txtBookingRate_TextChanged);
		this.txtBookingRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBookingRate_KeyPress);
		this.CmbBranch.AllowNull = (DefaultableBoolean)1;
		this.CmbBranch.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbBranch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbBranch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBranch.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Location = new System.Drawing.Point(600, 36);
		((System.Windows.Forms.Control)(object)this.CmbBranch).Name = "CmbBranch";
		((System.Windows.Forms.Control)(object)this.CmbBranch).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranch).TabIndex = 4;
		this.txtFixedDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFixedDays.Location = new System.Drawing.Point(326, 85);
		this.txtFixedDays.Name = "txtFixedDays";
		this.txtFixedDays.Size = new System.Drawing.Size(98, 23);
		this.txtFixedDays.TabIndex = 10;
		this.txtFixedDays.Text = "0";
		this.txtFixedDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFixedDays.TextChanged += new System.EventHandler(txtFixedDays_TextChanged);
		this.txtFixedDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFixedDays_KeyPress);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(326, 18);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(91, 15);
		this.label6.TabIndex = 5;
		this.label6.Text = "Contract Ref No";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(918, 64);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(62, 15);
		this.label13.TabIndex = 25;
		this.label13.Text = "Total Days";
		this.txtContractRef.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtContractRef.Location = new System.Drawing.Point(326, 38);
		this.txtContractRef.Name = "txtContractRef";
		this.txtContractRef.Size = new System.Drawing.Size(152, 23);
		this.txtContractRef.TabIndex = 2;
		this.txtTotalDays.Enabled = false;
		this.txtTotalDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalDays.Location = new System.Drawing.Point(918, 85);
		this.txtTotalDays.Name = "txtTotalDays";
		this.txtTotalDays.Size = new System.Drawing.Size(94, 23);
		this.txtTotalDays.TabIndex = 15;
		this.txtTotalDays.Text = "0";
		this.txtTotalDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.datDocDate.CustomFormat = "dd-MMM-yy";
		this.datDocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datDocDate.Location = new System.Drawing.Point(479, 38);
		this.datDocDate.Name = "datDocDate";
		this.datDocDate.Size = new System.Drawing.Size(120, 23);
		this.datDocDate.TabIndex = 3;
		this.datDocDate.ValueChanged += new System.EventHandler(datDocDate_ValueChanged);
		this.datFinalMaturity.CustomFormat = "dd-MMM-yy";
		this.datFinalMaturity.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFinalMaturity.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFinalMaturity.Location = new System.Drawing.Point(803, 85);
		this.datFinalMaturity.Name = "datFinalMaturity";
		this.datFinalMaturity.Size = new System.Drawing.Size(114, 23);
		this.datFinalMaturity.TabIndex = 14;
		this.lblFcyCode.AutoSize = true;
		this.lblFcyCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFcyCode.Location = new System.Drawing.Point(819, 18);
		this.lblFcyCode.Name = "lblFcyCode";
		this.lblFcyCode.Size = new System.Drawing.Size(55, 15);
		this.lblFcyCode.TabIndex = 9;
		this.lblFcyCode.Text = "Fcy Code";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(806, 64);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(80, 15);
		this.label12.TabIndex = 22;
		this.label12.Text = "Final Maturity";
		this.CmbFcyCode.AllowNull = (DefaultableBoolean)1;
		this.CmbFcyCode.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFcyCode.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFcyCode.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFcyCode).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.CmbFcyCode).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFcyCode).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbFcyCode.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFcyCode).Location = new System.Drawing.Point(821, 36);
		((System.Windows.Forms.Control)(object)this.CmbFcyCode).Name = "CmbFcyCode";
		((System.Windows.Forms.Control)(object)this.CmbFcyCode).Size = new System.Drawing.Size(92, 26);
		((System.Windows.Forms.Control)(object)this.CmbFcyCode).TabIndex = 5;
		this.datStartOfOptionalPeriod.CustomFormat = "dd-MMM-yy";
		this.datStartOfOptionalPeriod.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datStartOfOptionalPeriod.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datStartOfOptionalPeriod.Location = new System.Drawing.Point(665, 85);
		this.datStartOfOptionalPeriod.Name = "datStartOfOptionalPeriod";
		this.datStartOfOptionalPeriod.Size = new System.Drawing.Size(137, 23);
		this.datStartOfOptionalPeriod.TabIndex = 13;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(104, 64);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(58, 15);
		this.label7.TabIndex = 11;
		this.label7.Text = "Hcy Code";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(665, 64);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(134, 15);
		this.label10.TabIndex = 20;
		this.label10.Text = "Start Of Optional Period";
		this.CmbHcyCode.AllowNull = (DefaultableBoolean)1;
		this.CmbHcyCode.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbHcyCode.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbHcyCode.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbHcyCode).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbHcyCode).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbHcyCode).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbHcyCode).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbHcyCode).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbHcyCode).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbHcyCode).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbHcyCode).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbHcyCode).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbHcyCode).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbHcyCode).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbHcyCode).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbHcyCode).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbHcyCode).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbHcyCode).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbHcyCode).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbHcyCode).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbHcyCode).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbHcyCode).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbHcyCode).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbHcyCode).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbHcyCode).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbHcyCode).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbHcyCode).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbHcyCode).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbHcyCode).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbHcyCode).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbHcyCode).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbHcyCode.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbHcyCode).Location = new System.Drawing.Point(104, 83);
		((System.Windows.Forms.Control)(object)this.CmbHcyCode).Name = "CmbHcyCode";
		((System.Windows.Forms.Control)(object)this.CmbHcyCode).Size = new System.Drawing.Size(95, 26);
		((System.Windows.Forms.Control)(object)this.CmbHcyCode).TabIndex = 8;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(549, 64);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(115, 15);
		this.label11.TabIndex = 19;
		this.label11.Text = "Tenor Optional Days";
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyAmount.Location = new System.Drawing.Point(914, 38);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.Size = new System.Drawing.Size(98, 23);
		this.txtFcyAmount.TabIndex = 6;
		this.txtFcyAmount.Text = "0";
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFcyAmount.TextChanged += new System.EventHandler(txtFcyAmount_TextChanged);
		this.txtFcyAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFcyAmount_KeyPress);
		this.txtOptionalDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOptionalDays.Location = new System.Drawing.Point(549, 85);
		this.txtOptionalDays.Name = "txtOptionalDays";
		this.txtOptionalDays.Size = new System.Drawing.Size(115, 23);
		this.txtOptionalDays.TabIndex = 12;
		this.txtOptionalDays.Text = "0";
		this.txtOptionalDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOptionalDays.TextChanged += new System.EventHandler(txtOptionalDays_TextChanged);
		this.txtOptionalDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOptionalDays_KeyPress);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(481, 18);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(59, 15);
		this.label4.TabIndex = 14;
		this.label4.Text = "Deal Date";
		this.datFixedMaturity.CustomFormat = "dd-MMM-yy";
		this.datFixedMaturity.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFixedMaturity.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFixedMaturity.Location = new System.Drawing.Point(426, 85);
		this.datFixedMaturity.Name = "datFixedMaturity";
		this.datFixedMaturity.Size = new System.Drawing.Size(122, 23);
		this.datFixedMaturity.TabIndex = 11;
		this.datFixedMaturity.ValueChanged += new System.EventHandler(datFixedMaturity_ValueChanged);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(326, 64);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(97, 15);
		this.label8.TabIndex = 15;
		this.label8.Text = "Tenor Fixed Days";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(426, 64);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(83, 15);
		this.label9.TabIndex = 16;
		this.label9.Text = "Fixed Maturity";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(1111, 40);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(109, 16);
		this.label3.TabIndex = 1;
		this.label3.Text = "Select Provience";
		this.label3.Visible = false;
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1034, 29);
		this.panel3.TabIndex = 2;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label23);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1034, 29);
		this.panel6.TabIndex = 6;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(274, 19);
		this.label23.TabIndex = 25;
		this.label23.Text = "Exchange Rate Forward Booking";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1034, 27);
		this.panel2.TabIndex = 0;
		this.toolStrip1.BackColor = System.Drawing.Color.White;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btnnew, this.btnRefresh, this.btnsave, this.btnUpdate, this.btnAttachment, this.btnPrint, this.btnShortCutKey });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1034, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
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
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = (System.Drawing.Image)resources.GetObject("btnAttachment.Image");
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(84, 24);
		this.btnPrint.Text = "Print-01";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1034, 616);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "ExchangeRateForwardBooking";
		this.Text = "Check Status";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(CheqbookStatus_FormClosing);
		base.Load += new System.EventHandler(DefineCity_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(DefineCity_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdcheq).EndInit();
		this.panel7.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBankHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBranchHistory).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBank).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBranch).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFcyCode).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbHcyCode).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
