using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.HRM.PolicyManagment;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.PolicyManagment;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.HRM.PolicyManagment
{

public class IncomeTax : Form
{
	public UserAccount UserAccount = new UserAccount();

	private int RecId;

	private int updateDetailIndex;

	private bool UpdateMode;

	private DataTable table = new DataTable();

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	internal GridEX grdfrm;

	private Panel panel2;

	private Panel panel3;

	private Panel panel11;

	internal UltraCombo CmbSalaryFactor;

	private GroupBox groupBox1;

	private TextBox txttaxpercent;

	private Label label7;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private Button btnplus;

	private TextBox txttaxflatamount;

	private Label label6;

	private TextBox txttosalary;

	private TextBox txtfromsalary;

	private Label label4;

	private Label label5;

	private TextBox txtpolicydescription;

	private Label label3;

	private DateTimePicker txttodate;

	private DateTimePicker txtfromdate;

	private Label label1;

	private Label label2;

	private Label label48;

	private ToolStrip tsItemType;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private TabPage tabPage2;

	private Panel panel4;

	private Panel panel6;

	internal GridEX gridDetail;

	private Panel panel5;

	internal GridEX grdMain;

	private Panel panel8;

	private Panel panel7;

	private Label label8;

	private Panel panel9;

	private Label label9;

	private GroupBox groupBox2;

	private ToolStripButton btnRefresh;

	private Button btnsalaryfactor;

	private ToolStripButton btnAttachment;

	private Panel panel10;

	private Label label10;

	private CtrlGrdBar ctrlGrdBar3;

	public IncomeTax(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void ProfileType()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		try
		{
			DataTable lst = genProfile.GetByProfileTypeId(new genProfile
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				IsActive = true,
				ProfileTypeId = Conversion.ToInt((object)5)
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("ProfileName");
			for (int i = 0; i < lst.Rows.Count; i++)
			{
				dt.Rows.Add(lst.Rows[i]["ProfileId"], lst.Rows[i]["ProfileName"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbSalaryFactor, "Id", "ProfileName", "Salary Factor", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void resetdetail()
	{
		btnCancelUpdateDetial.Visible = false;
		btnUpdateDetail.Visible = false;
		btnplus.Visible = true;
		txttosalary.Text = "";
		txttaxpercent.Text = "";
		txttaxflatamount.Text = "";
	}

	private void Resetmain()
	{
		AT = new Attachment();
		txtpolicydescription.Text = "";
		((Control)(object)CmbSalaryFactor).Text = "";
		txtfromdate.Value = DateTime.Today;
		txttodate.Value = DateTime.Today;
		resetdetail();
		txtfromsalary.ReadOnly = false;
		txtfromsalary.Text = string.Empty;
		table.Rows.Clear();
		UpdateMode = false;
		RecId = 0;
		btnsave.Visible = true;
		btnupdate.Visible = false;
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			ProfileType();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Resetmain();
	}

	private void Insert()
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		try
		{
			if (!formvalidation())
			{
				return;
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
			hrmIncomeTaxPolicy obj = new hrmIncomeTaxPolicy();
			obj.IncomeTaxPolicyId = RecId;
			obj.SalaryFactorProfileId = Conversion.ToInt(CmbSalaryFactor.Value);
			obj.PolicyDescription = txtpolicydescription.Text;
			obj.FromDate = Conversion.ToDateTime((object)txtfromdate.Value);
			obj.ToDate = Conversion.ToDateTime((object)txttodate.Value);
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.AlteredOn = DateTime.Now;
			obj.CreatedById = UserAccount.ID;
			obj.CreatedOn = DateTime.Now;
			obj.ActionTypeId = 1;
			obj.IncomeTaxDetail = new List<hrmIncomeTaxPolicySlab>();
			obj.TaxPolicyEmployeesLists = new List<hrmIncomeTaxPolicyEmployee>();
			GridEXRow[] rows = grdfrm.GetRows();
			foreach (GridEXRow a in rows)
			{
				hrmIncomeTaxPolicySlab Detail = new hrmIncomeTaxPolicySlab();
				Detail.FromSalary = Conversion.ToInt(a.Cells["FromSalary"].Value);
				Detail.ToSalary = Conversion.ToInt(a.Cells["ToSalary"].Value);
				Detail.TaxPercent = Conversion.ToInt(a.Cells["Percent%"].Value);
				Detail.FlateTaxAmount = Conversion.ToInt(a.Cells["FlatAmount"].Value);
				Detail.IncomeTaxPolicySlabLineId = a.RowIndex;
				obj.IncomeTaxDetail.Add(Detail);
			}
			obj.dmsAttachment = new List<DMSAttachments>();
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Attachment.dtAttachments.Rows[i][1].ToString();
				attach.ScreenName = base.Name;
				obj.dmsAttachment.Add(attach);
			}
			hrmIncomeTaxPolicy.Save(obj);
			if (RecId == 0)
			{
				MessageBox.Show("Saved Successfully");
			}
			else
			{
				MessageBox.Show("Update Successfully");
			}
			Resetmain();
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

	private void btnupdate_Click(object sender, EventArgs e)
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

	private void GetById(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			hrmIncomeTaxPolicy obj = new hrmIncomeTaxPolicy();
			obj = hrmIncomeTaxPolicy.GetByID(Id);
			if (obj != null)
			{
				RecId = obj.IncomeTaxPolicyId;
				CmbSalaryFactor.Value = obj.SalaryFactorProfileId;
				txtpolicydescription.Text = obj.PolicyDescription;
				txtfromdate.Value = DateTime.Now;
				txttodate.Value = DateTime.Now;
				if (obj.IncomeTaxDetail.Count > 0)
				{
					table.Clear();
					for (int i = 0; i < obj.IncomeTaxDetail.Count; i++)
					{
						table.Rows.Add(obj.IncomeTaxDetail[i].FromSalary, obj.IncomeTaxDetail[i].ToSalary, obj.IncomeTaxDetail[i].TaxPercent, obj.IncomeTaxDetail[i].FlateTaxAmount);
					}
					grdfrm.DataSource = table;
					grdfrm.RetrieveStructure();
					grdSettings();
				}
				txtfromsalary.ReadOnly = true;
				btnsave.Visible = false;
				btnupdate.Visible = true;
				UpdateMode = true;
				tabControl1.SelectedIndex = 0;
			}
			List<DMSAttachments> byID = global::Architecture.BLL.DMSAttachments.GetByID(int.Parse(RecId.ToString()), base.Name);
			AT = new Attachment();
			foreach (global::Architecture.Model.DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				AT.lst.Add(TA);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void IncomeTax_Load(object sender, EventArgs e)
	{
		tabPage1.AutoScroll = true;
		tabPage1.AutoScrollMargin = new Size(20, 20);
		tabPage1.AutoScrollMinSize = new Size(980, 700);
		tabPage2.AutoScroll = true;
		tabPage2.AutoScrollMargin = new Size(20, 20);
		tabPage2.AutoScrollMinSize = new Size(980, 700);
		formright = CommonServices.SetRightsValueInRightsObject(base.Name);
		btnsave.Enabled = formright.DoHaveSaveRight;
		btnupdate.Enabled = formright.DoHaveUpdateRights;
		ctrlGrdBar3.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
		ctrlGrdBar3.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
		ctrlGrdBar3.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
		ctrlGrdBar3.mGridExport.Enabled = formright.DoHaveGridExportRights;
		ctrlGrdBar3.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
		ctrlGrdBar3.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
		BindGrid();
		btnsave.Visible = true;
		btnupdate.Visible = false;
		ProfileType();
		table.Columns.Add("FromSalary");
		table.Columns.Add("ToSalary");
		table.Columns.Add("Percent%", typeof(double));
		table.Columns.Add("FlatAmount", typeof(double));
		grdfrm.DataSource = table;
		grdfrm.RetrieveStructure();
		grdSettings();
		CmbSalaryFactor.Focus();
	}

	private bool formvalidation()
	{
		if (((Control)(object)CmbSalaryFactor).Text.Trim() == "")
		{
			MessageBox.Show("Please Select Salary Factor");
			CmbSalaryFactor.Focus();
			return false;
		}
		if (txtpolicydescription.Text.Trim() == "")
		{
			MessageBox.Show("Please Insert Policy Descripton");
			txtpolicydescription.Focus();
			return false;
		}
		return true;
	}

	private bool formvalidationDetail()
	{
		if (txtfromsalary.Text.Trim() == "")
		{
			MessageBox.Show("Please Insert From Salary");
			txtfromsalary.Focus();
			return false;
		}
		if (txttosalary.Text.Trim() == "")
		{
			MessageBox.Show("Please Insert To Salary");
			txttosalary.Focus();
			return false;
		}
		if (txttaxpercent.Text.Trim() == "" && txttaxflatamount.Text.Trim() == "")
		{
			MessageBox.Show("Please Insert Percent Or Flat Amount");
			txttaxpercent.Focus();
			return false;
		}
		if (int.Parse(txtfromsalary.Text) > int.Parse(txttosalary.Text))
		{
			MessageBox.Show("Please Check From Salary Is Greater Than To Salary");
			txtfromsalary.Focus();
			return false;
		}
		return true;
	}

	private void BindGrid()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		try
		{
			hrmIncomeTaxPolicy val = new hrmIncomeTaxPolicy
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtMail = new DataTable
			{
				Columns = { "Id", "SalaryFactor", "PolicyDescription", "FromDate", "ToDate" }
			};
			DataTable dtgrid = new DataTable();
			dtgrid = hrmIncomeTaxPolicy.Getall(val);
			for (int i = 0; i < dtgrid.Rows.Count; i++)
			{
				dtMail.Rows.Add(dtgrid.Rows[i]["IncomeTaxPolicyId"], dtgrid.Rows[i]["SalaryFactor"], dtgrid.Rows[i]["PolicyDescription"], Conversion.ToDateTime(dtgrid.Rows[i]["FromDate"]).ToString("dd-MMM-yyyy"), Conversion.ToDateTime(dtgrid.Rows[i]["ToDate"]).ToString("dd-MMM-yyyy"));
			}
			grdMain.DataSource = dtMail;
			grdMain.RetrieveStructure();
			historyGrdSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void historyGrdSetting()
	{
		try
		{
			grdMain.RootTable.Columns["Id"].EditType = (EditType)0;
			grdMain.RootTable.Columns["SalaryFactor"].EditType = (EditType)0;
			grdMain.RootTable.Columns["PolicyDescription"].EditType = (EditType)0;
			grdMain.RootTable.Columns["FromDate"].EditType = (EditType)0;
			grdMain.RootTable.Columns["ToDate"].EditType = (EditType)0;
			grdMain.RootTable.Columns["SalaryFactor"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["PolicyDescription"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["FromDate"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["ToDate"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["Id"].Visible = false;
			grdMain.RootTable.Columns["SalaryFactor"].Width = 150;
			grdMain.RootTable.Columns["PolicyDescription"].Width = 250;
			grdMain.RootTable.Columns["FromDate"].Width = 100;
			grdMain.RootTable.Columns["ToDate"].Width = 100;
			grdMain.RootTable.Columns.Add("Detail");
			grdMain.RootTable.Columns["Detail"].Key = "Detail";
			grdMain.RootTable.Columns["Detail"].Caption = "Detail";
			grdMain.RootTable.Columns["Detail"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdMain.RootTable.Columns["Detail"].ButtonStyle = (ButtonStyle)5;
			grdMain.RootTable.Columns["Detail"].ButtonText = "Detail";
			grdMain.RootTable.Columns["Detail"].Width = 70;
			grdMain.RootTable.Columns.Add("Edit");
			grdMain.RootTable.Columns["Edit"].Key = "Edit";
			grdMain.RootTable.Columns["Edit"].Caption = "Edit";
			grdMain.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdMain.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdMain.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdMain.RootTable.Columns["Edit"].Width = 70;
			ctrlGrdBar3_Load(null, null);
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
				BindGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdMain_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Detail")
			{
				DetailGridBind();
			}
			if (e.Column.Key == "Edit")
			{
				GetById(Conversion.ToInt(grdMain.CurrentRow.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridBind()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		try
		{
			if (grdMain.SelectedItems.Count <= 0)
			{
				return;
			}
			GridEXRow item = grdMain.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				hrmIncomeTaxPolicy cat = new hrmIncomeTaxPolicy();
				RecId = Conversion.ToInt(grdMain.CurrentRow.Cells[0].Value);
				cat = hrmIncomeTaxPolicy.GetByID(RecId);
				cat.IncomeTaxPolicyId = RecId;
				DataTable Detailtable = new DataTable();
				Detailtable.Columns.Add("FromSalary");
				Detailtable.Columns.Add("ToSalary");
				Detailtable.Columns.Add("Percent%", typeof(double));
				Detailtable.Columns.Add("FlatAmount", typeof(double));
				Detailtable.Clear();
				gridDetail.DataSource = Detailtable;
				for (int i = 0; i < cat.IncomeTaxDetail.Count; i++)
				{
					Detailtable.Rows.Add(cat.IncomeTaxDetail[i].FromSalary, cat.IncomeTaxDetail[i].ToSalary, cat.IncomeTaxDetail[i].TaxPercent, cat.IncomeTaxDetail[i].FlateTaxAmount);
				}
				gridDetail.DataSource = Detailtable;
				gridDetail.RetrieveStructure();
				griddetailsetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void griddetailsetting()
	{
		try
		{
			gridDetail.RootTable.Columns["FromSalary"].FilterEditType = (FilterEditType)1;
			gridDetail.RootTable.Columns["ToSalary"].FilterEditType = (FilterEditType)1;
			gridDetail.RootTable.Columns["Percent%"].FilterEditType = (FilterEditType)1;
			gridDetail.RootTable.Columns["FlatAmount"].FilterEditType = (FilterEditType)1;
			gridDetail.RootTable.Columns["FromSalary"].EditType = (EditType)0;
			gridDetail.RootTable.Columns["ToSalary"].EditType = (EditType)0;
			gridDetail.RootTable.Columns["Percent%"].EditType = (EditType)0;
			gridDetail.RootTable.Columns["FlatAmount"].EditType = (EditType)0;
			gridDetail.RootTable.Columns["FromSalary"].Width = 100;
			gridDetail.RootTable.Columns["ToSalary"].Width = 100;
			gridDetail.RootTable.Columns["Percent%"].Width = 100;
			gridDetail.RootTable.Columns["FlatAmount"].Width = 100;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!(e.Column.Key == "Delete"))
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				GridEXRow r = grdfrm.CurrentRow;
				if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
			}
			int amountColumnIndex = ((JanusCollectionElement)grdfrm.RootTable.Columns["ToSalary"]).Index;
			GridEXRow lastRow = grdfrm.GetRow(grdfrm.RowCount - 1);
			double lastAmountValue = 0.0;
			if (lastRow != null && (int)lastRow.RowType == 0)
			{
				lastAmountValue = Conversion.ToDouble(lastRow.Cells[amountColumnIndex].Value);
			}
			txtfromsalary.Text = (lastAmountValue + 1.0).ToString();
			txtfromsalary.Enabled = false;
			txttosalary.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			if (formvalidationDetail())
			{
				if (Conversion.ToDouble((object)txttaxflatamount.Text) > Conversion.ToDouble((object)txtfromsalary.Text))
				{
					MessageBox.Show("Flat Amount should not greater then from salary");
					txttaxflatamount.Focus();
					return;
				}
				if (Conversion.ToDouble((object)txttaxpercent.Text) > 100.0)
				{
					MessageBox.Show("Percent should not greater then 100");
					txttaxpercent.Focus();
					return;
				}
				table.Rows.Add(txtfromsalary.Text.Trim(), txttosalary.Text.Trim(), txttaxpercent.Text.Trim(), txttaxflatamount.Text.Trim());
				grdfrm.DataSource = table;
				grdfrm.RetrieveStructure();
				grdSettings();
				txtfromsalary.ReadOnly = true;
				double tosalary = Conversion.ToDouble((object)txttosalary.Text);
				txtfromsalary.Text = (1.0 + tosalary).ToString();
				resetdetail();
				txttosalary.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!formvalidationDetail())
			{
				return;
			}
			if (Conversion.ToDouble((object)txttaxflatamount.Text) > Conversion.ToDouble((object)txtfromsalary.Text))
			{
				MessageBox.Show("Flat Amount should not greater then from salary");
				txttaxflatamount.Focus();
				return;
			}
			if (Conversion.ToDouble((object)txttaxpercent.Text) > 100.0)
			{
				MessageBox.Show("Percent should not greater then 100");
				txttaxpercent.Focus();
				return;
			}
			table.Rows[updateDetailIndex]["FromSalary"] = txtfromsalary.Text;
			table.Rows[updateDetailIndex]["ToSalary"] = txttosalary.Text;
			table.Rows[updateDetailIndex]["Percent%"] = txttaxpercent.Text;
			table.Rows[updateDetailIndex]["FlatAmount"] = txttaxflatamount.Text;
			grdfrm.DataSource = table;
			grdfrm.RetrieveStructure();
			resetdetail();
			grdSettings();
			int amountColumnIndex = ((JanusCollectionElement)grdfrm.RootTable.Columns["ToSalary"]).Index;
			GridEXRow lastRow = grdfrm.GetRow(grdfrm.RowCount - 1);
			double lastAmountValue = 0.0;
			if (lastRow != null && (int)lastRow.RowType == 0)
			{
				lastAmountValue = Conversion.ToDouble(lastRow.Cells[amountColumnIndex].Value);
			}
			txtfromsalary.Text = (lastAmountValue + 1.0).ToString();
			txtfromsalary.Enabled = false;
			txttosalary.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		try
		{
			resetdetail();
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
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
				txtfromsalary.Text = item.Cells["FromSalary"].Text.ToString();
				txttosalary.Text = item.Cells["ToSalary"].Text.ToString();
				txttaxpercent.Text = item.Cells["Percent%"].Text.ToString();
				txttaxflatamount.Text = item.Cells["FlatAmount"].Text.ToString();
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				btnplus.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void grdSettings()
	{
		try
		{
			grdfrm.RootTable.Columns["FromSalary"].Width = 120;
			grdfrm.RootTable.Columns["ToSalary"].Width = 120;
			grdfrm.RootTable.Columns["Percent%"].Width = 150;
			grdfrm.RootTable.Columns["FromSalary"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["ToSalary"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["Percent%"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["FlatAmount"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["FromSalary"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["ToSalary"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["FlatAmount"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["Percent%"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["FlatAmount"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns.Add("Delete");
			grdfrm.RootTable.Columns["Delete"].Key = "Delete";
			grdfrm.RootTable.Columns["Delete"].Caption = " X";
			grdfrm.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdfrm.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdfrm.RootTable.Columns["Delete"].ButtonText = " X ";
			grdfrm.RootTable.Columns["Delete"].Width = 20;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void txttaxpercent_TextChanged(object sender, EventArgs e)
	{
		if (Conversion.ToDouble((object)txttaxpercent.Text) > 0.0)
		{
			txttaxflatamount.Text = 0.ToString();
		}
	}

	private void txttaxflatamount_TextChanged(object sender, EventArgs e)
	{
		if (Conversion.ToDouble((object)txttaxflatamount.Text) > 0.0)
		{
			txttaxpercent.Text = 0.ToString();
		}
	}

	private void txtfromsalary_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
			{
				e.Handled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txttosalary_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
			{
				e.Handled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txttaxpercent_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
			{
				e.Handled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txttaxflatamount_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
			{
				e.Handled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void IncomeTax_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S && !UpdateMode)
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
			if (e.Control && e.KeyCode == Keys.U && UpdateMode)
			{
				btnupdate_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsalaryfactor_Click(object sender, EventArgs e)
	{
		try
		{
			ProfileDefine profileDefine = new ProfileDefine(UserAccount);
			profileDefine.profileTypeId = 5;
			profileDefine.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdMain).Name);
		if (fs != null)
		{
			grdMain.LoadLayoutFile((Stream)fs);
			fs.Close();
			fs.Dispose();
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
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Expected O, but got Unknown
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Expected O, but got Unknown
		//IL_07f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0802: Expected O, but got Unknown
		//IL_2518: Unknown result type (might be due to invalid IL or missing references)
		//IL_2522: Expected O, but got Unknown
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.PolicyManagment.IncomeTax));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdfrm = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label8 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.btnsalaryfactor = new System.Windows.Forms.Button();
		this.CmbSalaryFactor = new UltraCombo();
		this.label48 = new System.Windows.Forms.Label();
		this.txtpolicydescription = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtfromdate = new System.Windows.Forms.DateTimePicker();
		this.txttodate = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txttaxpercent = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.btnplus = new System.Windows.Forms.Button();
		this.txttaxflatamount = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.txttosalary = new System.Windows.Forms.TextBox();
		this.txtfromsalary = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.gridDetail = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdMain = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label10 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel7.SuspendLayout();
		this.panel11.SuspendLayout();
		this.panel9.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSalaryFactor).BeginInit();
		this.groupBox1.SuspendLayout();
		this.tsItemType.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.gridDetail).BeginInit();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdMain).BeginInit();
		this.panel5.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(964, 661);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(964, 661);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.tsItemType);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(956, 632);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel3.Controls.Add(this.panel2);
		this.panel3.Controls.Add(this.panel11);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(3, 30);
		this.panel3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(950, 599);
		this.panel3.TabIndex = 22;
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.panel8);
		this.panel2.Controls.Add(this.panel7);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 235);
		this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(950, 364);
		this.panel2.TabIndex = 23;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 24);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(950, 340);
		this.panel8.TabIndex = 3;
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		this.grdfrm.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupByBoxVisible = false;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdfrm).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(950, 340);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 0;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.ColumnButtonClick += new ColumnActionEventHandler(grdfrm_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdfrm).DoubleClick += new System.EventHandler(grdfrm_DoubleClick);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label8);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(950, 24);
		this.panel7.TabIndex = 2;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(9, 5);
		this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(45, 16);
		this.label8.TabIndex = 13;
		this.label8.Text = "Detail";
		this.panel11.BackColor = System.Drawing.SystemColors.ButtonHighlight;
		this.panel11.Controls.Add(this.panel9);
		this.panel11.Controls.Add(this.groupBox2);
		this.panel11.Controls.Add(this.groupBox1);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Margin = new System.Windows.Forms.Padding(4);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(950, 235);
		this.panel11.TabIndex = 8;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label9);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(950, 29);
		this.panel9.TabIndex = 11;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(9, 4);
		this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(105, 19);
		this.label9.TabIndex = 14;
		this.label9.Text = "Income Tax";
		this.groupBox2.Controls.Add(this.btnsalaryfactor);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSalaryFactor);
		this.groupBox2.Controls.Add(this.label48);
		this.groupBox2.Controls.Add(this.txtpolicydescription);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add(this.txtfromdate);
		this.groupBox2.Controls.Add(this.txttodate);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(12, 37);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(804, 111);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main";
		this.btnsalaryfactor.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnsalaryfactor.Location = new System.Drawing.Point(251, 19);
		this.btnsalaryfactor.Name = "btnsalaryfactor";
		this.btnsalaryfactor.Size = new System.Drawing.Size(28, 26);
		this.btnsalaryfactor.TabIndex = 8;
		this.btnsalaryfactor.TabStop = false;
		this.btnsalaryfactor.UseVisualStyleBackColor = true;
		this.btnsalaryfactor.Click += new System.EventHandler(btnsalaryfactor_Click);
		this.CmbSalaryFactor.AllowNull = (DefaultableBoolean)1;
		this.CmbSalaryFactor.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSalaryFactor.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSalaryFactor.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSalaryFactor).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbSalaryFactor).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSalaryFactor).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSalaryFactor).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSalaryFactor).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbSalaryFactor).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSalaryFactor).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbSalaryFactor).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSalaryFactor).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSalaryFactor).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSalaryFactor).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbSalaryFactor).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSalaryFactor).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSalaryFactor).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSalaryFactor).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbSalaryFactor).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSalaryFactor).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSalaryFactor).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSalaryFactor).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbSalaryFactor).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSalaryFactor).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSalaryFactor).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbSalaryFactor).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSalaryFactor).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbSalaryFactor).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSalaryFactor).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSalaryFactor).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSalaryFactor).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSalaryFactor.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSalaryFactor).Location = new System.Drawing.Point(87, 19);
		((System.Windows.Forms.Control)(object)this.CmbSalaryFactor).Name = "CmbSalaryFactor";
		((System.Windows.Forms.Control)(object)this.CmbSalaryFactor).Size = new System.Drawing.Size(162, 26);
		((System.Windows.Forms.Control)(object)this.CmbSalaryFactor).TabIndex = 0;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(11, 25);
		this.label48.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(75, 15);
		this.label48.TabIndex = 0;
		this.label48.Text = "Salary Factor";
		this.txtpolicydescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtpolicydescription.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtpolicydescription.Location = new System.Drawing.Point(385, 19);
		this.txtpolicydescription.Margin = new System.Windows.Forms.Padding(4);
		this.txtpolicydescription.Multiline = true;
		this.txtpolicydescription.Name = "txtpolicydescription";
		this.txtpolicydescription.Size = new System.Drawing.Size(396, 78);
		this.txtpolicydescription.TabIndex = 3;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(11, 52);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 6;
		this.label1.Text = "From Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(285, 25);
		this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(103, 15);
		this.label3.TabIndex = 7;
		this.label3.Text = "Policy Description";
		this.txtfromdate.CustomFormat = "dd-MMM-yy";
		this.txtfromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtfromdate.Location = new System.Drawing.Point(87, 48);
		this.txtfromdate.Name = "txtfromdate";
		this.txtfromdate.Size = new System.Drawing.Size(192, 23);
		this.txtfromdate.TabIndex = 1;
		this.txttodate.CustomFormat = "dd-MMM-yy";
		this.txttodate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txttodate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txttodate.Location = new System.Drawing.Point(87, 74);
		this.txttodate.Name = "txttodate";
		this.txttodate.Size = new System.Drawing.Size(192, 23);
		this.txttodate.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(11, 78);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 15);
		this.label2.TabIndex = 4;
		this.label2.Text = "To Date";
		this.groupBox1.Controls.Add(this.txttaxpercent);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox1.Controls.Add(this.btnUpdateDetail);
		this.groupBox1.Controls.Add(this.btnplus);
		this.groupBox1.Controls.Add(this.txttaxflatamount);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.txttosalary);
		this.groupBox1.Controls.Add(this.txtfromsalary);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(12, 154);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(804, 75);
		this.groupBox1.TabIndex = 1;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.txttaxpercent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txttaxpercent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txttaxpercent.Location = new System.Drawing.Point(367, 40);
		this.txttaxpercent.Margin = new System.Windows.Forms.Padding(4);
		this.txttaxpercent.Name = "txttaxpercent";
		this.txttaxpercent.Size = new System.Drawing.Size(118, 23);
		this.txttaxpercent.TabIndex = 2;
		this.txttaxpercent.TextChanged += new System.EventHandler(txttaxpercent_TextChanged);
		this.txttaxpercent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txttaxpercent_KeyPress);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(376, 18);
		this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(78, 15);
		this.label7.TabIndex = 16;
		this.label7.Text = "Tax Percent%";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(724, 40);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(68, 23);
		this.btnCancelUpdateDetial.TabIndex = 6;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(653, 40);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(68, 23);
		this.btnUpdateDetail.TabIndex = 5;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(611, 40);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(39, 23);
		this.btnplus.TabIndex = 4;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.txttaxflatamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txttaxflatamount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txttaxflatamount.Location = new System.Drawing.Point(488, 40);
		this.txttaxflatamount.Margin = new System.Windows.Forms.Padding(4);
		this.txttaxflatamount.Name = "txttaxflatamount";
		this.txttaxflatamount.Size = new System.Drawing.Size(118, 23);
		this.txttaxflatamount.TabIndex = 3;
		this.txttaxflatamount.TextChanged += new System.EventHandler(txttaxflatamount_TextChanged);
		this.txttaxflatamount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txttaxflatamount_KeyPress);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(485, 18);
		this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(100, 15);
		this.label6.TabIndex = 14;
		this.label6.Text = "Tax Flate Amount";
		this.txttosalary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txttosalary.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txttosalary.Location = new System.Drawing.Point(184, 40);
		this.txttosalary.Margin = new System.Windows.Forms.Padding(4);
		this.txttosalary.Name = "txttosalary";
		this.txttosalary.Size = new System.Drawing.Size(181, 23);
		this.txttosalary.TabIndex = 1;
		this.txttosalary.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txttosalary_KeyPress);
		this.txtfromsalary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfromsalary.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfromsalary.Location = new System.Drawing.Point(9, 40);
		this.txtfromsalary.Margin = new System.Windows.Forms.Padding(4);
		this.txtfromsalary.Name = "txtfromsalary";
		this.txtfromsalary.Size = new System.Drawing.Size(171, 23);
		this.txtfromsalary.TabIndex = 0;
		this.txtfromsalary.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtfromsalary_KeyPress);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(185, 18);
		this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(55, 15);
		this.label4.TabIndex = 12;
		this.label4.Text = "To Salary";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(14, 18);
		this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(70, 15);
		this.label5.TabIndex = 10;
		this.label5.Text = "From Salary";
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnnew, this.btnupdate, this.btnsave, this.btnRefresh, this.btnAttachment });
		this.tsItemType.Location = new System.Drawing.Point(3, 3);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(950, 27);
		this.tsItemType.TabIndex = 21;
		this.tsItemType.Text = "toolStrip4";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(58, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnupdate.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(75, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(59, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 10.25f);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(78, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(110, 24);
		this.btnAttachment.Text = "Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.tabPage2.Controls.Add(this.panel4);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(956, 632);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel4.Controls.Add(this.panel10);
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(3, 3);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(950, 626);
		this.panel4.TabIndex = 0;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.gridDetail);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(0, 358);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(950, 268);
		this.panel10.TabIndex = 3;
		this.gridDetail.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.gridDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.gridDetail.FilterMode = (FilterMode)2;
		this.gridDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.gridDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.gridDetail.GroupByBoxVisible = false;
		this.gridDetail.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.gridDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.gridDetail).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.gridDetail).Name = "gridDetail";
		((ScrollableBase)this.gridDetail).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.gridDetail).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.gridDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.gridDetail).Size = new System.Drawing.Size(950, 268);
		((System.Windows.Forms.Control)(object)this.gridDetail).TabIndex = 2;
		this.gridDetail.TotalRow = (InheritableBoolean)1;
		this.gridDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.gridDetail).VisualStyle = (VisualStyle)4;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdMain);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 27);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(950, 331);
		this.panel6.TabIndex = 1;
		this.grdMain.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdMain).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdMain.FilterMode = (FilterMode)2;
		this.grdMain.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdMain).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdMain.GroupTotals = (GroupTotals)2;
		this.grdMain.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdMain).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdMain).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdMain).Name = "grdMain";
		((ScrollableBase)this.grdMain).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdMain).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdMain).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdMain).Size = new System.Drawing.Size(950, 331);
		((System.Windows.Forms.Control)(object)this.grdMain).TabIndex = 2;
		this.grdMain.TotalRow = (InheritableBoolean)1;
		this.grdMain.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdMain).VisualStyle = (VisualStyle)4;
		this.grdMain.ColumnButtonClick += new ColumnActionEventHandler(grdMain_ColumnButtonClick);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar3);
		this.panel5.Controls.Add(this.label10);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(950, 27);
		this.panel5.TabIndex = 0;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(896, -1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.grdMain;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(47, 26);
		this.ctrlGrdBar3.TabIndex = 24;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label10.ForeColor = System.Drawing.Color.White;
		this.label10.Location = new System.Drawing.Point(6, 4);
		this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(67, 19);
		this.label10.TabIndex = 15;
		this.label10.Text = "History";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 661);
		base.Controls.Add(this.panel1);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "IncomeTax";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(IncomeTax_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(IncomeTax_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage1.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSalaryFactor).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.gridDetail).EndInit();
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdMain).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
