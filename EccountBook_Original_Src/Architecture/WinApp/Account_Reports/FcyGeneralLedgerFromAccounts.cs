using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Reports;

public class FcyGeneralLedgerFromAccounts : Form
{
	private WaitForm wf = new WaitForm();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtData = new DataTable();

	public DateTime FromDate;

	public DateTime ToDate;

	public int AccountId;

	private bool SubsidiaryFeature;

	private bool AfterCloseUp;

	private bool AllowEditOnVoucherNoinReports;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel3;

	private Label label23;

	private Panel panel4;

	private DateTimePicker todate;

	private DateTimePicker fromdate;

	private Label label33;

	private Label label27;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton print;

	private ToolStripButton btnNew;

	private Panel panel5;

	private Panel panel6;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grd;

	private Label label2;

	internal UltraCombo cmbAccountTitle;

	private Button btnshow;

	internal UltraCombo CmbSubsidiaryAccount;

	private Label lblSubsidiaryAccount;

	private ToolStripButton btnPrint105D;

	public FcyGeneralLedgerFromAccounts(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void VoucherValidation_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				print_Click_1(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				fromdate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountTitleFill()
	{
		try
		{
			DataTable dt = CommonServices.GetAllDetailAccount();
			if (dt.Rows.Count > 0 && dt != null)
			{
				DDL.BindDDL(dt, cmbAccountTitle, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
				((UltraGridBase)cmbAccountTitle).DisplayLayout.Bands[0].Columns[1].Width = 400;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbSubsidiaryAccountFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = VoucherHead.GetSubsidiaryAccountFromVouchers(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbSubsidiaryAccount, "SubsidiaryAccountId", "SubsidaryAccount", "Subsidiary Account", ZeroIndex: false);
				((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			}
			else
			{
				((UltraGridBase)CmbSubsidiaryAccount).DataSource = null;
				((Control)(object)CmbSubsidiaryAccount).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void VoucherValidation_Load(object sender, EventArgs e)
	{
		try
		{
			wf.Show();
			SubsidiaryFeature = CommonServices.GetERPFeatureById(4);
			AllowEditOnVoucherNoinReports = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("AllowEditOnVoucherNoinReports"));
			AccountTitleFill();
			fromdate.Focus();
			if (AccountId > 0)
			{
				cmbAccountTitle.Value = AccountId;
				fromdate.Value = FromDate;
				todate.Value = ToDate;
				GetData();
			}
			else
			{
				fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
			if (SubsidiaryFeature)
			{
				CmbSubsidiaryAccountFill();
				lblSubsidiaryAccount.Visible = true;
				((Control)(object)CmbSubsidiaryAccount).Visible = true;
				btnshow.Location = new Point(781, 25);
			}
			else
			{
				lblSubsidiaryAccount.Visible = false;
				((Control)(object)CmbSubsidiaryAccount).Visible = false;
				btnshow.Location = new Point(522, 25);
			}
		}
		catch (Exception ex)
		{
			wf.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			wf.Close();
		}
	}

	private void reset()
	{
		try
		{
			((Control)(object)cmbAccountTitle).Text = string.Empty;
			fromdate.Focus();
			grd.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void print_Click_1(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtData == null || dtData.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtData, "105C-AcRptFcyGeneralLedger.rpt");
		}
		catch (Exception)
		{
			MessageBox.Show("Report Not Found");
		}
	}

	private void toolStripButton17_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			wf.Show();
			GetData();
		}
		catch (Exception ex)
		{
			wf.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			wf.Close();
		}
	}

	private void GetData()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtData.Rows.Clear();
			ReportsParameters vh = new ReportsParameters();
			vh.OrganizationId = UserAccount.OrganizationId;
			vh.CompanyId = UserAccount.CompanyId;
			vh.AccountId = Conversion.ToInt(cmbAccountTitle.Value);
			if (vh.AccountId == 0)
			{
				MessageBox.Show("Account Title Required", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				cmbAccountTitle.Focus();
				return;
			}
			vh.FromDate = fromdate.Value;
			vh.ToDate = todate.Value;
			vh.SubsidiaryAccountId = Conversion.ToInt(CmbSubsidiaryAccount.Value);
			vh.ApprovedFilter = "All";
			dtData = VoucherReports.Accounts_FcyGeneralLedger_Rpt(vh);
			if (dtData.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("VoucherDate", typeof(DateTime));
				dt.Columns.Add("DocType");
				dt.Columns.Add("VoucherNo");
				dt.Columns.Add("AccountCode");
				dt.Columns.Add("OffSetTitle");
				dt.Columns.Add("FcyCode");
				dt.Columns.Add("ExchangeRate", typeof(double));
				dt.Columns.Add("FcyDebit", typeof(double));
				dt.Columns.Add("FcyCredit", typeof(double));
				dt.Columns.Add("FcyBalance", typeof(double));
				dt.Columns.Add("BalType");
				dt.Columns.Add("LcyDebit", typeof(double));
				dt.Columns.Add("LcyCredit", typeof(double));
				dt.Columns.Add("LcyBalance", typeof(double));
				dt.Columns.Add("LcyBalType");
				dt.Columns.Add("Comments");
				dt.Columns.Add("MannualNo");
				dt.Columns.Add("NoOfAttachments", typeof(double));
				dt.Columns.Add("VoucherHeadId");
				dt.Columns.Add("DocumentTypeId");
				dt.Columns.Add("DocumentTypeSrNo");
				for (int i = 0; i < dtData.Rows.Count; i++)
				{
					string type = "Nill";
					if (Conversion.ToDouble(dtData.Rows[i]["FcyBalance"]) > 0.0)
					{
						type = "Dr";
					}
					else if (Conversion.ToDouble(dtData.Rows[i]["FcyBalance"]) < 0.0)
					{
						type = "Cr";
					}
					string typeLcy = "Nill";
					if (Conversion.ToDouble(dtData.Rows[i]["Balance"]) > 0.0)
					{
						typeLcy = "Dr";
					}
					else if (Conversion.ToDouble(dtData.Rows[i]["Balance"]) < 0.0)
					{
						typeLcy = "Cr";
					}
					dt.Rows.Add(Conversion.ToDateTime(dtData.Rows[i]["VoucherDate"]).ToShortDateString(), dtData.Rows[i]["DocTypeCode"], dtData.Rows[i]["VoucherCode"], dtData.Rows[i]["AccountCode"], dtData.Rows[i]["OffsetAccountTitle"], dtData.Rows[i]["FcyCode"], dtData.Rows[i]["ExRateCal"], dtData.Rows[i]["FcyDebit"], dtData.Rows[i]["FcyCredit"], dtData.Rows[i]["FcyBalance"], type, dtData.Rows[i]["DebitAmount"], dtData.Rows[i]["CreditAmount"], dtData.Rows[i]["Balance"], typeLcy, dtData.Rows[i]["Comments"], dtData.Rows[i]["MannualNo"], dtData.Rows[i]["NoOfAttachments"], dtData.Rows[i]["VoucherHeadId"], dtData.Rows[i]["DocumentTypeId"], dtData.Rows[i]["DocumentTypeSrNo"]);
				}
				grd.DataSource = dt;
				grd.RetrieveStructure();
				GridSetting();
			}
			else
			{
				grd.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridSetting()
	{
		try
		{
			grd.AutomaticSort = false;
			grd.RootTable.Columns["VoucherHeadId"].Visible = false;
			grd.RootTable.Columns["DocumentTypeId"].Visible = false;
			grd.RootTable.Columns["DocumentTypeSrNo"].Visible = false;
			grd.RootTable.Columns["MannualNo"].Caption = "Manual No";
			grd.RootTable.Columns["VoucherDate"].Width = 85;
			grd.RootTable.Columns["DocType"].Width = 100;
			grd.RootTable.Columns["VoucherNo"].Width = 80;
			grd.RootTable.Columns["AccountCode"].Width = 90;
			grd.RootTable.Columns["OffSetTitle"].Width = 150;
			grd.RootTable.Columns["FcyCode"].Width = 75;
			grd.RootTable.Columns["ExchangeRate"].Width = 95;
			grd.RootTable.Columns["FcyDebit"].Width = 100;
			grd.RootTable.Columns["FcyCredit"].Width = 100;
			grd.RootTable.Columns["FcyBalance"].Width = 100;
			grd.RootTable.Columns["BalType"].Width = 75;
			grd.RootTable.Columns["LcyDebit"].Width = 100;
			grd.RootTable.Columns["LcyCredit"].Width = 100;
			grd.RootTable.Columns["LcyBalance"].Width = 100;
			grd.RootTable.Columns["LcyBalType"].Width = 85;
			grd.RootTable.Columns["Comments"].Width = 200;
			grd.RootTable.Columns["MannualNo"].Width = 85;
			grd.RootTable.Columns["NoOfAttachments"].Width = 110;
			grd.RootTable.Columns["DocumentTypeSrNo"].Width = 110;
			grd.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ExchangeRate"].FormatString = clsGlobalVariables.DecimalFCYRateFormate;
			grd.RootTable.Columns["FcyDebit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FcyDebit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcyDebit"].FormatString = clsGlobalVariables.stringFormatbothForFcy;
			grd.RootTable.Columns["FcyDebit"].TotalFormatString = clsGlobalVariables.stringFormatbothForFcy;
			grd.RootTable.Columns["FcyCredit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FcyCredit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcyCredit"].FormatString = clsGlobalVariables.stringFormatbothForFcy;
			grd.RootTable.Columns["FcyCredit"].TotalFormatString = clsGlobalVariables.stringFormatbothForFcy;
			grd.RootTable.Columns["FcyBalance"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcyBalance"].FormatString = clsGlobalVariables.stringFormatbothForFcy;
			grd.RootTable.Columns["FcyBalance"].TotalFormatString = clsGlobalVariables.stringFormatbothForFcy;
			grd.RootTable.Columns["LcyDebit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["LcyDebit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["LcyDebit"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["LcyDebit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["LcyCredit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["LcyCredit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["LcyCredit"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["LcyCredit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["LcyBalance"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["LcyBalance"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["LcyBalance"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			if (AllowEditOnVoucherNoinReports)
			{
				grd.RootTable.Columns["VoucherNo"].ColumnType = (ColumnType)5;
			}
			grd.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["DocType"].ColumnType = (ColumnType)5;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
			wf.Close();
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void FcyGeneralLedgerRpt_FormClosing(object sender, FormClosingEventArgs e)
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

	private void cmbAccountTitle_Leave(object sender, EventArgs e)
	{
		try
		{
			if (SubsidiaryFeature)
			{
				if (((UltraGridBase)cmbAccountTitle).ActiveRow != null && ((Control)(object)cmbAccountTitle).Text != string.Empty)
				{
					SubsidiaryAccountBindByParentAccount();
				}
				else
				{
					CmbSubsidiaryAccountFill();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SubsidiaryAccountBindByParentAccount()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable dt = VoucherHead.GetSubsidiaryAccountFromVouchers(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				AccountId = Conversion.ToInt(cmbAccountTitle.Value)
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbSubsidiaryAccount, "SubsidiaryAccountId", "SubsidaryAccount", "Subsidiary Account", ZeroIndex: false);
				((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			}
			else
			{
				((Control)(object)CmbSubsidiaryAccount).Text = string.Empty;
				((UltraGridBase)CmbSubsidiaryAccount).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSubsidiaryAccount_AfterCloseUp(object sender, EventArgs e)
	{
		try
		{
			UltraGridRow selectedRow = ((UltraDropDownBase)CmbSubsidiaryAccount).SelectedRow;
			if (selectedRow != null)
			{
				AfterCloseUp = true;
				int GLOfSubSidory = Conversion.ToInt(selectedRow.Cells[2].Value);
				if (GLOfSubSidory != 0)
				{
					cmbAccountTitle.Value = GLOfSubSidory;
					cmbAccountTitle_Leave(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSubsidiaryAccount_Leave(object sender, EventArgs e)
	{
		try
		{
			if (!AfterCloseUp && ((UltraGridBase)CmbSubsidiaryAccount).ActiveRow != null && ((Control)(object)CmbSubsidiaryAccount).Text != string.Empty)
			{
				cmbAccountTitle.Value = Conversion.ToInt(((UltraDropDownBase)CmbSubsidiaryAccount).SelectedRow.Cells[2].Value);
				cmbAccountTitle_Leave(null, null);
			}
			AfterCloseUp = false;
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
			GridEXRow currentRow = grd.CurrentRow;
			int Id = int.Parse(currentRow.Cells["VoucherHeadId"].Value.ToString());
			int DocumentTypeId = Conversion.ToInt((object)currentRow.Cells["DocumentTypeId"].Value.ToString());
			int DocumentTypeIdSrNo = int.Parse(currentRow.Cells["DocumentTypeSrNo"].Value.ToString());
			if (e.Column.Key == "VoucherNo")
			{
				CommonServices.EditMethodFromLinked(DocumentTypeId, Id, DocumentTypeIdSrNo);
			}
			if (e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(DocumentTypeIdSrNo, DocumentTypeId);
			}
			if (e.Column.Key == "DocType")
			{
				CommonServices.GetSlipsByDocumentTypeId(DocumentTypeId, DocumentTypeIdSrNo, Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint105D_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtData == null || dtData.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtData, "105D-AcRptFcyGeneralLedger.rpt");
		}
		catch (Exception)
		{
			MessageBox.Show("Report Not Found");
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
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Expected O, but got Unknown
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.label23 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.CmbSubsidiaryAccount = new UltraCombo();
		this.lblSubsidiaryAccount = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.cmbAccountTitle = new UltraCombo();
		this.todate = new System.Windows.Forms.DateTimePicker();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.print = new System.Windows.Forms.ToolStripButton();
		this.btnPrint105D = new System.Windows.Forms.ToolStripButton();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSubsidiaryAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAccountTitle).BeginInit();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(962, 29);
		this.panel3.TabIndex = 4;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(921, 4);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grd.AllowEdit = (InheritableBoolean)2;
		this.grd.AlternatingColors = true;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(962, 493);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(6, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(101, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "FCY Ledger";
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 56);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(962, 551);
		this.panel4.TabIndex = 5;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 58);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(962, 493);
		this.panel6.TabIndex = 108;
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount);
		this.panel5.Controls.Add(this.lblSubsidiaryAccount);
		this.panel5.Controls.Add(this.btnshow);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAccountTitle);
		this.panel5.Controls.Add(this.todate);
		this.panel5.Controls.Add(this.fromdate);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.label33);
		this.panel5.Controls.Add(this.label27);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(962, 58);
		this.panel5.TabIndex = 107;
		this.CmbSubsidiaryAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbSubsidiaryAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSubsidiaryAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSubsidiaryAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance10;
		((SpecialBoxBase)((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance15).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSubsidiaryAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Location = new System.Drawing.Point(522, 25);
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Name = "CmbSubsidiaryAccount";
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Size = new System.Drawing.Size(257, 26);
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).TabIndex = 3;
		this.CmbSubsidiaryAccount.AfterCloseUp += new System.EventHandler(CmbSubsidiaryAccount_AfterCloseUp);
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Leave += new System.EventHandler(CmbSubsidiaryAccount_Leave);
		this.lblSubsidiaryAccount.AutoSize = true;
		this.lblSubsidiaryAccount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSubsidiaryAccount.Location = new System.Drawing.Point(522, 6);
		this.lblSubsidiaryAccount.Name = "lblSubsidiaryAccount";
		this.lblSubsidiaryAccount.Size = new System.Drawing.Size(79, 15);
		this.lblSubsidiaryAccount.TabIndex = 111;
		this.lblSubsidiaryAccount.Text = "Subsidiary Ac";
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(781, 25);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(61, 26);
		this.btnshow.TabIndex = 4;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.cmbAccountTitle.AllowNull = (DefaultableBoolean)1;
		this.cmbAccountTitle.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAccountTitle.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAccountTitle.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccountTitle).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance21;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccountTitle).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance3).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbAccountTitle.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Location = new System.Drawing.Point(237, 25);
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Name = "cmbAccountTitle";
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Size = new System.Drawing.Size(283, 26);
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Leave += new System.EventHandler(cmbAccountTitle_Leave);
		this.todate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.CustomFormat = "dd-MMM-yy";
		this.todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.todate.Location = new System.Drawing.Point(124, 27);
		this.todate.Name = "todate";
		this.todate.Size = new System.Drawing.Size(111, 23);
		this.todate.TabIndex = 1;
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yy";
		this.fromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(11, 27);
		this.fromdate.Name = "fromdate";
		this.fromdate.Size = new System.Drawing.Size(111, 23);
		this.fromdate.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(237, 6);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(78, 15);
		this.label2.TabIndex = 105;
		this.label2.Text = "Account Title";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(124, 6);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(48, 15);
		this.label33.TabIndex = 102;
		this.label33.Text = "To Date";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(11, 6);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 103;
		this.label27.Text = "From Date";
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(962, 607);
		this.panel1.TabIndex = 0;
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(962, 27);
		this.panel2.TabIndex = 3;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNew, this.print, this.btnPrint105D });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(962, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&Reset";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print.Name = "print";
		this.print.Size = new System.Drawing.Size(87, 24);
		this.print.Text = "105C-Print";
		this.print.Click += new System.EventHandler(print_Click_1);
		this.btnPrint105D.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint105D.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint105D.Name = "btnPrint105D";
		this.btnPrint105D.Size = new System.Drawing.Size(87, 24);
		this.btnPrint105D.Text = "105D-Print";
		this.btnPrint105D.Click += new System.EventHandler(btnPrint105D_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(962, 607);
		base.Controls.Add(this.panel1);
		this.DoubleBuffered = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "FcyGeneralLedgerFromAccounts";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(FcyGeneralLedgerRpt_FormClosing);
		base.Load += new System.EventHandler(VoucherValidation_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(VoucherValidation_KeyDown);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSubsidiaryAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAccountTitle).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
