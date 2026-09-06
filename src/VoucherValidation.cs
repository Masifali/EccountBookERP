// using Architecture.Reports;
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
using Architecture.Model.Accounts;
using Architecture.WinApp.Common;
using Infragistics.Shared;
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
namespace Architecture.WinApp.Account_Reports
{

public class VoucherValidation : Form
{
	private WaitForm wf = new WaitForm();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtvoucher = new DataTable();

	private DataTable dtDocType = new DataTable();

	private bool AllowEditOnVoucherNoinReports;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel3;

	private Label label23;

	private Panel panel4;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton print;

	private ToolStripButton btnNew;

	private Panel panel6;

	private Panel panel5;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton btnPrint119;

	private ToolStripButton btnRefresh;

	private Panel panel7;

	private Label label3;

	private GroupBox groupBox1;

	private Label label4;

	private TextBox txtManualNo;

	internal UltraCombo CmbCustomGroup;

	private Label label6;

	internal UltraCombo CmbAccountTitle;

	private Label label7;

	private Button button1;

	internal UltraCombo cmbdoctype;

	private Label label8;

	private TextBox txtdocnoto;

	private Label label10;

	private TextBox txtdocnofrom;

	private DateTimePicker todate;

	private DateTimePicker fromdate;

	private Label label11;

	private Label label12;

	private GroupBox groupBox2;

	private RadioButton rdEntryFromToDate;

	private RadioButton rdDocFromToDate;

	private RadioButton RdAll;

	private RadioButton RdUnApproved;

	private RadioButton rdApproved;

	private CheckBox chkSkipCgs;

	internal UltraCombo CmbLanguage;

	private Label lblLanguage;

	public VoucherValidation(UserAccount user)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void VoucherValidation_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
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
			if (e.Control && e.KeyCode == Keys.L)
			{
				lblLanguage.Visible = true;
				((Control)(object)CmbLanguage).Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAllDetailAccount()
	{
		try
		{
			DataTable dt = CommonServices.GetAllDetailAccount();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbAccountTitle, "Id", "AccountTitle", "AccountTitle", false);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CustomeGroupsDefine()
	{
		try
		{
			DataTable dt = CommonServices.CustomeGroupsDefine(1);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbCustomGroup, "Id", "AcLookUpsDescription", "Custome Group", false);
				((UltraGridBase)CmbCustomGroup).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void VouchertypeFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			VoucherHead obj = new VoucherHead();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtDocType = VoucherHead.GetDocumentTypesFromVouchers(obj);
			if (dtDocType.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtDocType, cmbdoctype, "Id", "DocumentTypeDescription", "Document Type", false);
				UltraGridColumn obj2 = ((UltraGridBase)cmbdoctype).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj2).Key = "Selected";
				((HeaderBase)obj2.Header).Caption = string.Empty;
				obj2.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj2.DataType = typeof(bool);
				((HeaderBase)obj2.Header).VisiblePosition = 0;
				cmbdoctype.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)cmbdoctype.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)cmbdoctype.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)cmbdoctype.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)cmbdoctype).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void MultiLanguagesGetAll()
	{
		try
		{
			DataTable dt = CommonServices.MultiLanguagesGetAll();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbLanguage, "Id", "LanguageDescription", "Language Name", false);
				((UltraGridBase)CmbLanguage).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
			RdAll.Checked = true;
			AllowEditOnVoucherNoinReports = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("AllowEditOnVoucherNoinReports"));
			MultiLanguagesGetAll();
			VouchertypeFill();
			GetAllDetailAccount();
			CustomeGroupsDefine();
			fromdate.Value = DateTime.Now;
			fromdate.Focus();
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
			txtdocnofrom.Clear();
			txtdocnoto.Clear();
			((Control)(object)CmbAccountTitle).Text = string.Empty;
			((Control)(object)CmbCustomGroup).Text = string.Empty;
			RdAll.Checked = true;
			cmbdoctype.Focus();
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
			wf.Close();
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		try
		{
			wf.Show();
			string DocumentTypeIds = "";
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.UserId = UserAccount.ID;
			if (rdDocFromToDate.Checked)
			{
				obj.FromDate = Conversion.ToDateTime((object)fromdate.Value);
				obj.ToDate = Conversion.ToDateTime((object)todate.Value);
			}
			else if (rdEntryFromToDate.Checked)
			{
				obj.EntryDateFrom = Conversion.ToDateTime((object)fromdate.Value);
				obj.EntryDateTo = Conversion.ToDateTime((object)todate.Value);
			}
			obj.FromDocNo = Conversion.ToInt((object)txtdocnofrom.Text.Trim());
			obj.ToDocNo = Conversion.ToInt((object)txtdocnoto.Text.Trim());
			obj.AccountId = Conversion.ToInt(CmbAccountTitle.Value);
			obj.CustomerGroupId = Conversion.ToInt(CmbCustomGroup.Value);
			obj.ManualBillNo = Conversion.ToString((object)txtManualNo.Text);
			if (rdApproved.Checked)
			{
				obj.IsApproved = true;
			}
			else if (RdUnApproved.Checked)
			{
				obj.IsApproved = false;
			}
			else if (RdAll.Checked)
			{
				obj.ApprovedFilter = "All";
			}
			if (chkSkipCgs.Checked)
			{
				obj.ActionId = 1;
			}
			if (Conversion.ToString((object)((Control)(object)cmbdoctype).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbdoctype).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtDocType.Select("DocumentTypeDescription='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						DocumentTypeIds = DocumentTypeIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.SaleInvoiceDocumentTypeIds = DocumentTypeIds;
			}
			obj.LanguageId = Conversion.ToInt(CmbLanguage.Value);
			dtvoucher = VoucherReports.VoucherValidationReport(obj);
			if (dtvoucher.Rows.Count > 0)
			{
				DataTable dtGrid = new DataTable();
				dtGrid.Columns.Add("Id", typeof(int));
				dtGrid.Columns.Add("AccountId", typeof(int));
				dtGrid.Columns.Add("DocumentType", typeof(string));
				dtGrid.Columns.Add("DocumentTypeId", typeof(int));
				dtGrid.Columns.Add("DocumentTypeSrNo", typeof(int));
				dtGrid.Columns.Add("VoucherCode", typeof(int));
				dtGrid.Columns.Add("VoucherDate", typeof(DateTime));
				dtGrid.Columns.Add("AccountCode", typeof(string));
				dtGrid.Columns.Add("AccountTitle", typeof(string));
				dtGrid.Columns.Add("AccountTitleCoag", typeof(string));
				dtGrid.Columns.Add("DebitAmount", typeof(double));
				dtGrid.Columns.Add("CreditAmount", typeof(double));
				dtGrid.Columns.Add("ChequeNo", typeof(string));
				dtGrid.Columns.Add("ManualBillNo", typeof(string));
				dtGrid.Columns.Add("RefInvoiceNo", typeof(string));
				dtGrid.Columns.Add("Comments", typeof(string));
				dtGrid.Columns.Add("NoOfAttachments", typeof(int));
				for (int i = 0; i < dtvoucher.Rows.Count; i++)
				{
					dtGrid.Rows.Add(Conversion.ToInt(dtvoucher.Rows[i]["Id"]), Conversion.ToInt(dtvoucher.Rows[i]["AccountId"]), Conversion.ToString(dtvoucher.Rows[i]["DocumentTypeDescription"]), Conversion.ToInt(dtvoucher.Rows[i]["DocumentTypeId"]), Conversion.ToInt(dtvoucher.Rows[i]["DocumentTypeSrNo"]), Conversion.ToInt(dtvoucher.Rows[i]["VoucherCode"]), Conversion.ToDateTime(dtvoucher.Rows[i]["VoucherDate"]).ToShortDateString(), Conversion.ToString(dtvoucher.Rows[i]["AccountCode"]), Conversion.ToString(dtvoucher.Rows[i]["AccountTitle"]), Conversion.ToString(dtvoucher.Rows[i]["AccountTitleCoag"]), Conversion.ToDouble(dtvoucher.Rows[i]["DebitAmount"]), Conversion.ToDouble(dtvoucher.Rows[i]["CreditAmount"]), Conversion.ToString(dtvoucher.Rows[i]["CheqNoDetail"]), Conversion.ToString(dtvoucher.Rows[i]["ManualBillNo"]), Conversion.ToString(dtvoucher.Rows[i]["RefInvoiceNo"]), Conversion.ToString(dtvoucher.Rows[i]["Comments"]), Conversion.ToInt(dtvoucher.Rows[i]["NoOfAttachments"]));
				}
				grd.DataSource = dtGrid;
				grd.RetrieveStructure();
				GridSettings();
			}
			else
			{
				grd.ClearStructure();
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

	private void GridSettings()
	{
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["DocumentTypeId"].Visible = false;
			grd.RootTable.Columns["DocumentTypeSrNo"].Visible = false;
			grd.RootTable.Columns["AccountId"].Visible = false;
			grd.RootTable.Columns["VoucherCode"].Caption = "V.Code";
			grd.RootTable.Columns["AccountTitleCoag"].Caption = "Offset Account";
			grd.RootTable.Columns["DocumentType"].Width = 110;
			grd.RootTable.Columns["DocumentTypeSrNo"].Width = 100;
			grd.RootTable.Columns["VoucherCode"].Width = 65;
			grd.RootTable.Columns["VoucherDate"].Width = 95;
			grd.RootTable.Columns["AccountCode"].Width = 100;
			grd.RootTable.Columns["AccountTitle"].Width = 140;
			grd.RootTable.Columns["AccountTitleCoag"].Width = 140;
			grd.RootTable.Columns["DebitAmount"].Width = 100;
			grd.RootTable.Columns["CreditAmount"].Width = 100;
			grd.RootTable.Columns["ChequeNo"].Width = 95;
			grd.RootTable.Columns["ManualBillNo"].Width = 100;
			grd.RootTable.Columns["RefInvoiceNo"].Width = 90;
			grd.RootTable.Columns["Comments"].Width = 250;
			grd.RootTable.Columns["NoOfAttachments"].Width = 100;
			grd.RootTable.Columns["VoucherDate"].FilterEditType = (FilterEditType)6;
			grd.RootTable.Columns["DebitAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["DebitAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["DebitAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["DebitAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["CreditAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["CreditAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["CreditAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["CreditAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			if (AllowEditOnVoucherNoinReports)
			{
				grd.RootTable.Columns["VoucherCode"].ColumnType = (ColumnType)5;
			}
			grd.RootTable.Columns["AccountCode"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grd.RootTable.Groups.Add("VoucherCode");
			grd.HideColumnsWhenGrouped = (InheritableBoolean)1;
			grd.RootTable.Columns.Add("Slip");
			grd.RootTable.Columns["Slip"].Caption = "print";
			grd.RootTable.Columns["Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Slip"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Slip"].ButtonText = "print";
			grd.RootTable.Columns["Slip"].Width = 40;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow currentRow = grd.CurrentRow;
			int Id = int.Parse(currentRow.Cells["Id"].Value.ToString());
			int DocumentTypeId = Conversion.ToInt((object)currentRow.Cells["DocumentTypeId"].Value.ToString());
			int DocumentTypeIdSrNo = Conversion.ToInt((object)currentRow.Cells["DocumentTypeSrNo"].Value.ToString());
			int AccountId = Conversion.ToInt((object)currentRow.Cells["AccountId"].Value.ToString());
			if (e.Column.Key == "VoucherCode")
			{
				CommonServices.EditMethodFromLinked(DocumentTypeId, Id, DocumentTypeIdSrNo);
			}
			if (e.Column.Key == "AccountCode")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(AccountId, fromdate.Value, todate.Value);
			}
			if (e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint119_Click(object sender, EventArgs e)
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
				val.ShowReportWithDataTable(dtvoucher, "119-AcRptVoucherValidation2.rpt");
				return;
			}
			throw new Exception("Record Not Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ShowReport()
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
				val.ShowReportWithDataTable(dtvoucher, "115-AcRptVoucherValidation.rpt");
				return;
			}
			throw new Exception("Record Not Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = VoucherReports.VoucherValidationReport(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				UserId = UserAccount.ID,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = Conversion.ToInt(grd.CurrentRow.Cells["Id"].Value)
			});
			if (dt.Rows.Count > 0)
			{
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "115-AcRptVoucherValidation.rpt");
				return;
			}
			throw new Exception("Record Not Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocnofrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtdocnoto_KeyPress(object sender, KeyPressEventArgs e)
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			wf.Show();
			VouchertypeFill();
			GetAllDetailAccount();
			CustomeGroupsDefine();
			MultiLanguagesGetAll();
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

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void print_Click_1(object sender, EventArgs e)
	{
		try
		{
			ShowReport();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherValidation_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Expected O, but got Unknown
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Expected O, but got Unknown
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Expected O, but got Unknown
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Expected O, but got Unknown
		//IL_074b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0755: Expected O, but got Unknown
		//IL_0762: Unknown result type (might be due to invalid IL or missing references)
		//IL_076c: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Reports.VoucherValidation));
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label3 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.chkSkipCgs = new System.Windows.Forms.CheckBox();
		this.RdAll = new System.Windows.Forms.RadioButton();
		this.RdUnApproved = new System.Windows.Forms.RadioButton();
		this.rdApproved = new System.Windows.Forms.RadioButton();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.rdEntryFromToDate = new System.Windows.Forms.RadioButton();
		this.rdDocFromToDate = new System.Windows.Forms.RadioButton();
		this.label4 = new System.Windows.Forms.Label();
		this.txtManualNo = new System.Windows.Forms.TextBox();
		this.CmbCustomGroup = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbAccountTitle = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.button1 = new System.Windows.Forms.Button();
		this.cmbdoctype = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.txtdocnoto = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.txtdocnofrom = new System.Windows.Forms.TextBox();
		this.todate = new System.Windows.Forms.DateTimePicker();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.label11 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.print = new System.Windows.Forms.ToolStripButton();
		this.btnPrint119 = new System.Windows.Forms.ToolStripButton();
		this.CmbLanguage = new UltraCombo();
		this.lblLanguage = new System.Windows.Forms.Label();
		this.panel3.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel7.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitle).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdoctype).BeginInit();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbLanguage).BeginInit();
		base.SuspendLayout();
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLanguage);
		this.panel3.Controls.Add(this.lblLanguage);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1101, 28);
		this.panel3.TabIndex = 4;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(163, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Voucher Validation";
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 55);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1101, 585);
		this.panel4.TabIndex = 5;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 135);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1101, 450);
		this.panel6.TabIndex = 110;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 28);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1101, 422);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar1);
		this.panel7.Controls.Add(this.label3);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1101, 28);
		this.panel7.TabIndex = 5;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1053, 1);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar1.TabIndex = 1;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(5, 4);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(67, 19);
		this.label3.TabIndex = 1;
		this.label3.Text = "History";
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.groupBox1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1101, 135);
		this.panel5.TabIndex = 109;
		this.groupBox1.Controls.Add(this.chkSkipCgs);
		this.groupBox1.Controls.Add(this.RdAll);
		this.groupBox1.Controls.Add(this.RdUnApproved);
		this.groupBox1.Controls.Add(this.rdApproved);
		this.groupBox1.Controls.Add(this.groupBox2);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.txtManualNo);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomGroup);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountTitle);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.button1);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbdoctype);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.txtdocnoto);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.txtdocnofrom);
		this.groupBox1.Controls.Add(this.todate);
		this.groupBox1.Controls.Add(this.fromdate);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(812, 130);
		this.groupBox1.TabIndex = 130;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.chkSkipCgs.AutoSize = true;
		this.chkSkipCgs.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkSkipCgs.Location = new System.Drawing.Point(603, 105);
		this.chkSkipCgs.Name = "chkSkipCgs";
		this.chkSkipCgs.Size = new System.Drawing.Size(112, 19);
		this.chkSkipCgs.TabIndex = 12;
		this.chkSkipCgs.Text = "Skip CGS Entries";
		this.chkSkipCgs.UseVisualStyleBackColor = true;
		this.RdAll.AutoSize = true;
		this.RdAll.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.RdAll.Location = new System.Drawing.Point(603, 79);
		this.RdAll.Name = "RdAll";
		this.RdAll.Size = new System.Drawing.Size(39, 19);
		this.RdAll.TabIndex = 11;
		this.RdAll.TabStop = true;
		this.RdAll.Text = "All";
		this.RdAll.UseVisualStyleBackColor = true;
		this.RdUnApproved.AutoSize = true;
		this.RdUnApproved.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.RdUnApproved.Location = new System.Drawing.Point(603, 51);
		this.RdUnApproved.Name = "RdUnApproved";
		this.RdUnApproved.Size = new System.Drawing.Size(95, 19);
		this.RdUnApproved.TabIndex = 10;
		this.RdUnApproved.TabStop = true;
		this.RdUnApproved.Text = "Un Approved";
		this.RdUnApproved.UseVisualStyleBackColor = true;
		this.rdApproved.AutoSize = true;
		this.rdApproved.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.rdApproved.Location = new System.Drawing.Point(603, 23);
		this.rdApproved.Name = "rdApproved";
		this.rdApproved.Size = new System.Drawing.Size(77, 19);
		this.rdApproved.TabIndex = 9;
		this.rdApproved.TabStop = true;
		this.rdApproved.Text = "Approved";
		this.rdApproved.UseVisualStyleBackColor = true;
		this.groupBox2.Controls.Add(this.rdEntryFromToDate);
		this.groupBox2.Controls.Add(this.rdDocFromToDate);
		this.groupBox2.Location = new System.Drawing.Point(700, 9);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(107, 65);
		this.groupBox2.TabIndex = 13;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filter Type";
		this.rdEntryFromToDate.AutoSize = true;
		this.rdEntryFromToDate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdEntryFromToDate.Location = new System.Drawing.Point(6, 44);
		this.rdEntryFromToDate.Name = "rdEntryFromToDate";
		this.rdEntryFromToDate.Size = new System.Drawing.Size(80, 19);
		this.rdEntryFromToDate.TabIndex = 1;
		this.rdEntryFromToDate.Text = "Entry Date";
		this.rdEntryFromToDate.UseVisualStyleBackColor = true;
		this.rdDocFromToDate.AutoSize = true;
		this.rdDocFromToDate.Checked = true;
		this.rdDocFromToDate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdDocFromToDate.Location = new System.Drawing.Point(6, 21);
		this.rdDocFromToDate.Name = "rdDocFromToDate";
		this.rdDocFromToDate.Size = new System.Drawing.Size(75, 19);
		this.rdDocFromToDate.TabIndex = 0;
		this.rdDocFromToDate.TabStop = true;
		this.rdDocFromToDate.Text = "Doc Date";
		this.rdDocFromToDate.UseVisualStyleBackColor = true;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(323, 24);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(66, 15);
		this.label4.TabIndex = 147;
		this.label4.Text = "Manual No";
		this.txtManualNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtManualNo.Location = new System.Drawing.Point(407, 20);
		this.txtManualNo.Name = "txtManualNo";
		this.txtManualNo.Size = new System.Drawing.Size(191, 23);
		this.txtManualNo.TabIndex = 5;
		this.CmbCustomGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Location = new System.Drawing.Point(407, 74);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Name = "CmbCustomGroup";
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Size = new System.Drawing.Size(191, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).TabIndex = 7;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(323, 80);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(84, 15);
		this.label6.TabIndex = 146;
		this.label6.Text = "Custom Group";
		this.CmbAccountTitle.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountTitle.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountTitle.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountTitle.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance42).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance43).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance5).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Location = new System.Drawing.Point(407, 46);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Name = "CmbAccountTitle";
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Size = new System.Drawing.Size(191, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).TabIndex = 6;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(323, 52);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(78, 15);
		this.label7.TabIndex = 145;
		this.label7.Text = "Account Title";
		this.button1.BackColor = System.Drawing.Color.Teal;
		this.button1.FlatAppearance.BorderSize = 0;
		this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button1.ForeColor = System.Drawing.Color.White;
		this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.button1.Location = new System.Drawing.Point(535, 102);
		this.button1.Margin = new System.Windows.Forms.Padding(2);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(63, 23);
		this.button1.TabIndex = 8;
		this.button1.Text = "Show";
		this.button1.UseVisualStyleBackColor = false;
		this.button1.Click += new System.EventHandler(btnshow_Click);
		this.cmbdoctype.AllowNull = (DefaultableBoolean)1;
		this.cmbdoctype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbdoctype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbdoctype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbdoctype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbdoctype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbdoctype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbdoctype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdoctype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance14;
		((SpecialBoxBase)((UltraGridBase)this.cmbdoctype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdoctype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbdoctype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbdoctype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbdoctype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbdoctype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbdoctype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbdoctype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdoctype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance36).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbdoctype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbdoctype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbdoctype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance37).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdoctype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbdoctype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbdoctype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbdoctype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbdoctype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbdoctype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbdoctype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbdoctype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbdoctype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbdoctype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbdoctype).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbdoctype).Location = new System.Drawing.Point(114, 74);
		((System.Windows.Forms.Control)(object)this.cmbdoctype).Name = "cmbdoctype";
		((System.Windows.Forms.Control)(object)this.cmbdoctype).Size = new System.Drawing.Size(203, 26);
		((System.Windows.Forms.Control)(object)this.cmbdoctype).TabIndex = 2;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(7, 80);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(92, 15);
		this.label8.TabIndex = 143;
		this.label8.Text = "Document Type";
		this.txtdocnoto.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocnoto.Location = new System.Drawing.Point(210, 102);
		this.txtdocnoto.Name = "txtdocnoto";
		this.txtdocnoto.Size = new System.Drawing.Size(107, 23);
		this.txtdocnoto.TabIndex = 4;
		this.txtdocnoto.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtdocnoto_KeyPress);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(7, 106);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(107, 15);
		this.label10.TabIndex = 141;
		this.label10.Text = "Doc No From && To";
		this.txtdocnofrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocnofrom.Location = new System.Drawing.Point(114, 102);
		this.txtdocnofrom.Name = "txtdocnofrom";
		this.txtdocnofrom.Size = new System.Drawing.Size(90, 23);
		this.txtdocnofrom.TabIndex = 3;
		this.txtdocnofrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtdocnofrom_KeyPress);
		this.todate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.CustomFormat = "dd-MMM-yy";
		this.todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.todate.Location = new System.Drawing.Point(114, 48);
		this.todate.Name = "todate";
		this.todate.Size = new System.Drawing.Size(203, 23);
		this.todate.TabIndex = 1;
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yy";
		this.fromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(114, 20);
		this.fromdate.Name = "fromdate";
		this.fromdate.Size = new System.Drawing.Size(203, 23);
		this.fromdate.TabIndex = 0;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(7, 24);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(63, 15);
		this.label11.TabIndex = 140;
		this.label11.Text = "From Date";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(7, 52);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(48, 15);
		this.label12.TabIndex = 139;
		this.label12.Text = "To Date";
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1101, 640);
		this.panel1.TabIndex = 0;
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1101, 27);
		this.panel2.TabIndex = 3;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.btnRefresh, this.print, this.btnPrint119 });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1101, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = (System.Drawing.Image)resources.GetObject("btnNew.Image");
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(68, 24);
		this.btnNew.Text = "&Reset";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = (System.Drawing.Image)resources.GetObject("btnRefresh.Image");
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.print.Image = (System.Drawing.Image)resources.GetObject("print.Image");
		this.print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print.Name = "print";
		this.print.Size = new System.Drawing.Size(92, 24);
		this.print.Text = "115-Print";
		this.print.Click += new System.EventHandler(print_Click_1);
		this.btnPrint119.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint119.Image = (System.Drawing.Image)resources.GetObject("btnPrint119.Image");
		this.btnPrint119.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint119.Name = "btnPrint119";
		this.btnPrint119.Size = new System.Drawing.Size(92, 24);
		this.btnPrint119.Text = "119-Print";
		this.btnPrint119.Click += new System.EventHandler(btnPrint119_Click);
		this.CmbLanguage.AllowNull = (DefaultableBoolean)1;
		this.CmbLanguage.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLanguage.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLanguage.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLanguage.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Location = new System.Drawing.Point(274, 1);
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Name = "CmbLanguage";
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Size = new System.Drawing.Size(173, 26);
		((System.Windows.Forms.Control)(object)this.CmbLanguage).TabIndex = 129;
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Visible = false;
		this.lblLanguage.AutoSize = true;
		this.lblLanguage.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblLanguage.ForeColor = System.Drawing.Color.White;
		this.lblLanguage.Location = new System.Drawing.Point(205, 5);
		this.lblLanguage.Name = "lblLanguage";
		this.lblLanguage.Size = new System.Drawing.Size(68, 17);
		this.lblLanguage.TabIndex = 130;
		this.lblLanguage.Text = "Language";
		this.lblLanguage.Visible = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1101, 640);
		base.Controls.Add(this.panel1);
		this.DoubleBuffered = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "VoucherValidation";
		this.Text = "Voucher Validation";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(VoucherValidation_FormClosing);
		base.Load += new System.EventHandler(VoucherValidation_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(VoucherValidation_KeyDown);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitle).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdoctype).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbLanguage).EndInit();
		base.ResumeLayout(false);
	}
}

}
