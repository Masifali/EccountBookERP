using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.HRM.PolicyManagment;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.PolicyManagment;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.HRM.PolicyManagment;

public class SocialSecurity : Form
{
	public UserAccount UserAccount = new UserAccount();

	private int RecId;

	private int updateDetailIndex;

	private DataTable table = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private bool UpdateMode;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private Panel panel3;

	private ToolStrip tsItemType;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private Panel panel4;

	private TextBox txtminyearlimit;

	private Label label8;

	internal UltraCombo cmbSalaryFactor;

	private TextBox txtpolicydescription;

	private Label label3;

	private DateTimePicker txttodate;

	private DateTimePicker txtfromdate;

	private Label label1;

	private Label label2;

	private Label label48;

	private GroupBox groupBox1;

	private TextBox txtpercent;

	private Label label7;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private Button btnplus;

	private TextBox txttosalary;

	private TextBox txtfromsalary;

	private Label label4;

	private Label label5;

	internal GridEX grdfrm;

	private Panel panel6;

	private Panel panel7;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel8;

	private Panel panel9;

	internal GridEX grdHistoryDetail;

	internal GridEX grdHistory;

	private ToolStripButton btnRefresh;

	private Panel panel12;

	private Panel panel11;

	private Panel panel10;

	private GroupBox groupBox2;

	private Label label6;

	private Label label9;

	private Label label10;

	private TextBox txtamount;

	private Label label11;

	private Button btnsalaryfactor;

	public SocialSecurity(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void SocialSecurity_Load(object sender, EventArgs e)
	{
		try
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
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			ProfileType();
			table.Columns.Add("FromSalary");
			table.Columns.Add("ToSalary");
			table.Columns.Add("Percent%", typeof(double));
			table.Columns.Add("Amount", typeof(double));
			cmbSalaryFactor.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Expected O, but got Unknown
		try
		{
			if (!formvalidation())
			{
				return;
			}
			if (txtfromdate.Value > txttodate.Value)
			{
				MessageBox.Show("DateFrom Is Greater Than DateTo please Check: Thank You");
			}
			else if (grdfrm.RowCount > 0)
			{
				hrmSocialPolicy socialpolicy = new hrmSocialPolicy();
				socialpolicy.SocialPolicyId = RecId;
				socialpolicy.OrganizationId = Conversion.ToInt((object)UserAccount.OrganizationId);
				socialpolicy.CompanyId = Conversion.ToInt((object)UserAccount.CompanyId);
				socialpolicy.CreatedOn = Conversion.ToDateTime((object)DateTime.Today);
				socialpolicy.CreatedById = Conversion.ToInt((object)UserAccount.ID);
				socialpolicy.AlteredById = Conversion.ToInt((object)UserAccount.ID);
				socialpolicy.AlteredOn = Conversion.ToDateTime((object)DateTime.Today);
				socialpolicy.FromDate = Conversion.ToDateTime((object)txtfromdate.Value);
				socialpolicy.ToDate = Conversion.ToDateTime((object)txttodate.Value);
				socialpolicy.PolicyDescription = Conversion.ToString((object)txtpolicydescription.Text.Trim());
				socialpolicy.SalaryFactorProfileId = Conversion.ToInt(cmbSalaryFactor.Value);
				socialpolicy.MinYearLimit = Conversion.ToInt((object)txtminyearlimit.Text);
				if (int.Parse(grdfrm.GetTotal(grdfrm.RootTable.Columns["Percent%"], (AggregateFunction)2).ToString()) > 100)
				{
					MessageBox.Show("Your Total Percent Is Greater Than 100 Please Check It");
					return;
				}
				socialpolicy.SocialSecurityDetail = new List<hrmSocialPolicySlab>();
				GridEXRow[] rows = grdfrm.GetRows();
				foreach (GridEXRow r in rows)
				{
					hrmSocialPolicySlab socialdetail = new hrmSocialPolicySlab();
					socialdetail.FromSalary = Conversion.ToInt(r.Cells["FromSalary"].Value);
					socialdetail.ToSalary = Conversion.ToInt(r.Cells["ToSalary"].Value);
					socialdetail.SecurityPercent = Conversion.ToInt(r.Cells["Percent%"].Value);
					socialdetail.SecurityAmount = Conversion.ToInt(r.Cells["Amount"].Value);
					socialpolicy.SocialSecurityDetail.Add(socialdetail);
				}
				hrmSocialPolicy.Save(socialpolicy);
				if (RecId > 0)
				{
					MessageBox.Show("Update Successfully");
				}
				else
				{
					MessageBox.Show("Save Successfully");
				}
				Resetmain();
			}
			else
			{
				MessageBox.Show("Please Insert Record In Detail ThankYou");
			}
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
			hrmSocialPolicy obj = new hrmSocialPolicy();
			obj = hrmSocialPolicy.GetByID(Id);
			if (obj == null)
			{
				return;
			}
			RecId = obj.SocialPolicyId;
			cmbSalaryFactor.Value = obj.SalaryFactorProfileId;
			txtfromdate.Value = obj.FromDate;
			txttodate.Value = obj.ToDate;
			txtminyearlimit.Text = obj.MinYearLimit.ToString();
			txtpolicydescription.Text = obj.PolicyDescription;
			if (obj.SocialSecurityDetail.Count > 0)
			{
				for (int i = 0; i < obj.SocialSecurityDetail.Count; i++)
				{
					table.Rows.Add(obj.SocialSecurityDetail[i].FromSalary, obj.SocialSecurityDetail[i].ToSalary, Conversion.ToDouble((object)obj.SocialSecurityDetail[i].SecurityPercent), Conversion.ToDouble((object)obj.SocialSecurityDetail[i].SecurityPercent));
				}
				grdfrm.DataSource = table;
				grdfrm.RetrieveStructure();
				grdSettings();
				txtfromsalary.Enabled = false;
			}
			btnsave.Visible = false;
			btnupdate.Visible = true;
			UpdateMode = true;
			tabControl1.SelectedIndex = 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProfileType()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		try
		{
			DataTable dt = genProfile.GetByProfileTypeId(new genProfile
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ProfileTypeId = Conversion.ToInt((object)5),
				IsActive = true
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbSalaryFactor, "ProfileId", "ProfileName", "BenefitProfile", false);
			}
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
			Resetmain();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void resetdetail()
	{
		txttosalary.Text = "";
		txtpercent.Text = "";
		txtamount.Text = "";
		btnplus.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
	}

	private void Resetmain()
	{
		txtpolicydescription.Text = "";
		((Control)(object)cmbSalaryFactor).Text = "";
		txtminyearlimit.Text = "";
		txtfromdate.Value = DateTime.Today;
		txttodate.Value = DateTime.Today;
		table.Rows.Clear();
		grdfrm.ClearStructure();
		txtfromsalary.ReadOnly = false;
		btnsave.Visible = true;
		btnupdate.Visible = false;
		UpdateMode = false;
		RecId = 0;
		resetdetail();
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

	private bool formvalidation()
	{
		if (((Control)(object)cmbSalaryFactor).Text.Trim() == "")
		{
			MessageBox.Show("Please Select Salary Factor");
			cmbSalaryFactor.Focus();
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
		if (txtpercent.Text.Trim() == "" && txtamount.Text.Trim() == "")
		{
			MessageBox.Show("Please Insert Percent Or Flat Amount");
			txtpercent.Focus();
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

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			if (!formvalidationDetail())
			{
				return;
			}
			if (grdfrm.GetRows().Length != 0)
			{
				GridEXRow[] rows = grdfrm.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToDouble((object)txtfromsalary.Text) < Conversion.ToDouble(r.Cells["FromSalary"].Value))
					{
						MessageBox.Show("From Salary Should Not Greater then Record that Already Exist");
						return;
					}
				}
			}
			table.Rows.Add(txtfromsalary.Text.Trim(), txttosalary.Text.Trim(), Conversion.ToDouble((object)txtpercent.Text.Trim()), Conversion.ToDouble((object)txtamount.Text.Trim()));
			grdfrm.DataSource = table;
			grdfrm.RetrieveStructure();
			double tosalary = Conversion.ToDouble((object)txttosalary.Text);
			txtfromsalary.Text = (1.0 + tosalary).ToString();
			resetdetail();
			txtfromsalary.ReadOnly = true;
			txttosalary.Focus();
			grdSettings();
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
				txtfromsalary.Text = item.Cells["FromSalary"].Value.ToString();
				txttosalary.Text = item.Cells["ToSalary"].Value.ToString();
				txtpercent.Text = item.Cells["Percent%"].Value.ToString();
				txtamount.Text = item.Cells["Amount"].Value.ToString();
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				grdfrm.DataSource = table;
				grdfrm.RetrieveStructure();
			}
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (formvalidationDetail())
			{
				table.Rows[updateDetailIndex]["FromSalary"] = txtfromsalary.Text;
				table.Rows[updateDetailIndex]["ToSalary"] = txttosalary.Text;
				table.Rows[updateDetailIndex]["Percent%"] = Conversion.ToDouble((object)txtpercent.Text);
				table.Rows[updateDetailIndex]["Amount"] = Conversion.ToDouble((object)txtamount.Text);
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

	public void grdSettings()
	{
		try
		{
			grdfrm.RootTable.Columns["FromSalary"].Width = 120;
			grdfrm.RootTable.Columns["ToSalary"].Width = 120;
			grdfrm.RootTable.Columns["Percent%"].Width = 150;
			grdfrm.RootTable.Columns["Amount"].Width = 150;
			grdfrm.RootTable.Columns["FromSalary"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["ToSalary"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["Percent%"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["Amount%"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["FromSalary"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["ToSalary"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["Percent%"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["Percent%"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["Amount%"].AggregateFunction = (AggregateFunction)2;
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

	private void SocialSecurity_KeyDown(object sender, KeyEventArgs e)
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
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryFill()
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("SocialPolicyId");
			dt.Columns.Add("ProfileName");
			dt.Columns.Add("FromDate");
			dt.Columns.Add("ToDate");
			dt.Columns.Add("MinYearLimit");
			dt.Columns.Add("PolicyDescription");
			DataTable dtHistory = hrmSocialPolicy.Getall(new hrmSocialPolicy
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtHistory.Rows.Count > 0)
			{
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					dt.Rows.Add(dtHistory.Rows[i]["SocialPolicyId"], dtHistory.Rows[i]["ProfileName"], Conversion.ToDateTime(dtHistory.Rows[i]["FromDate"]).ToString("dd-MMM-yyyy"), Conversion.ToDateTime(dtHistory.Rows[i]["ToDate"]).ToString("dd-MMM-yyyy"), dtHistory.Rows[i]["MinYearLimit"], dtHistory.Rows[i]["PolicyDescription"]);
				}
				grdHistory.DataSource = dt;
				grdHistory.RetrieveStructure();
				grdHistorySetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistorySetting()
	{
		try
		{
			grdHistory.RootTable.Columns["SocialPolicyId"].Visible = false;
			grdHistory.RootTable.Columns.Add("Edit");
			grdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdHistory.RootTable.Columns["Edit"].Key = "Edit";
			grdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdHistory.RootTable.Columns["Edit"].Width = 50;
			grdHistory.RootTable.Columns.Add("Detail");
			grdHistory.RootTable.Columns["Detail"].Caption = "Detail";
			grdHistory.RootTable.Columns["Detail"].Key = "Detail";
			grdHistory.RootTable.Columns["Detail"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Detail"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Detail"].ButtonText = "Detail";
			grdHistory.RootTable.Columns["Detail"].Width = 50;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Detail")
			{
				HistoryDetailBind(Conversion.ToInt(grdHistory.CurrentRow.Cells["SocialPolicyId"].Value));
			}
			if (e.Column.Key == "Edit")
			{
				GetById(Conversion.ToInt(grdHistory.CurrentRow.Cells["SocialPolicyId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryDetailBind(int Id)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("FromSalary");
			dt.Columns.Add("ToSalary");
			dt.Columns.Add("Percent%");
			dt.Columns.Add("Amount");
			hrmSocialPolicy obj = new hrmSocialPolicy();
			obj = hrmSocialPolicy.GetByID(Id);
			if (obj != null && obj.SocialSecurityDetail.Count > 0)
			{
				for (int i = 0; i < obj.SocialSecurityDetail.Count; i++)
				{
					dt.Rows.Add(obj.SocialSecurityDetail[i].FromSalary, obj.SocialSecurityDetail[i].ToSalary, Conversion.ToDouble((object)obj.SocialSecurityDetail[i].SecurityPercent), Conversion.ToDouble((object)obj.SocialSecurityDetail[i].SecurityPercent));
				}
				grdHistoryDetail.DataSource = dt;
				grdHistoryDetail.RetrieveStructure();
			}
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
				HistoryFill();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistory).Name);
			if (fs != null)
			{
				grdHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
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
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Expected O, but got Unknown
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Expected O, but got Unknown
		//IL_0768: Unknown result type (might be due to invalid IL or missing references)
		//IL_0772: Expected O, but got Unknown
		//IL_24f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_24fb: Expected O, but got Unknown
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.PolicyManagment.SocialSecurity));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.grdfrm = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.btnsalaryfactor = new System.Windows.Forms.Button();
		this.cmbSalaryFactor = new UltraCombo();
		this.label48 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtfromdate = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.txtminyearlimit = new System.Windows.Forms.TextBox();
		this.txttodate = new System.Windows.Forms.DateTimePicker();
		this.txtpolicydescription = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtamount = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.txtpercent = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.btnplus = new System.Windows.Forms.Button();
		this.txttosalary = new System.Windows.Forms.TextBox();
		this.txtfromsalary = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdHistoryDetail = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label10 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel11.SuspendLayout();
		this.panel10.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSalaryFactor).BeginInit();
		this.groupBox1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.tsItemType.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).BeginInit();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel7.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(874, 607);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(874, 607);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(866, 581);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel12);
		this.panel2.Controls.Add(this.panel11);
		this.panel2.Controls.Add(this.panel10);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(860, 575);
		this.panel2.TabIndex = 0;
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel12.Location = new System.Drawing.Point(0, 292);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(860, 283);
		this.panel12.TabIndex = 25;
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
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
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(860, 283);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 0;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.ColumnButtonClick += new ColumnActionEventHandler(grdfrm_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdfrm).DoubleClick += new System.EventHandler(grdfrm_DoubleClick);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label9);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 261);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(860, 31);
		this.panel11.TabIndex = 24;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(6, 6);
		this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(46, 17);
		this.label9.TabIndex = 12;
		this.label9.Text = "Detail";
		this.panel10.Controls.Add(this.groupBox2);
		this.panel10.Controls.Add(this.groupBox1);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 58);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(860, 203);
		this.panel10.TabIndex = 23;
		this.groupBox2.Controls.Add(this.btnsalaryfactor);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSalaryFactor);
		this.groupBox2.Controls.Add(this.label48);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.txtfromdate);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.txtminyearlimit);
		this.groupBox2.Controls.Add(this.txttodate);
		this.groupBox2.Controls.Add(this.txtpolicydescription);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(28, 6);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(673, 112);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main";
		this.btnsalaryfactor.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnsalaryfactor.Location = new System.Drawing.Point(245, 17);
		this.btnsalaryfactor.Name = "btnsalaryfactor";
		this.btnsalaryfactor.Size = new System.Drawing.Size(28, 26);
		this.btnsalaryfactor.TabIndex = 1;
		this.btnsalaryfactor.TabStop = false;
		this.btnsalaryfactor.UseVisualStyleBackColor = true;
		this.btnsalaryfactor.Click += new System.EventHandler(btnsalaryfactor_Click);
		this.cmbSalaryFactor.AllowNull = (DefaultableBoolean)1;
		this.cmbSalaryFactor.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSalaryFactor.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSalaryFactor.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSalaryFactor).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbSalaryFactor).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSalaryFactor).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSalaryFactor).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSalaryFactor).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbSalaryFactor).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSalaryFactor).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbSalaryFactor).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSalaryFactor).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSalaryFactor).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSalaryFactor).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbSalaryFactor).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSalaryFactor).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSalaryFactor).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSalaryFactor).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbSalaryFactor).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSalaryFactor).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSalaryFactor).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSalaryFactor).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbSalaryFactor).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSalaryFactor).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSalaryFactor).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbSalaryFactor).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSalaryFactor).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbSalaryFactor).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSalaryFactor).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSalaryFactor).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSalaryFactor).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSalaryFactor.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSalaryFactor).Location = new System.Drawing.Point(90, 17);
		((System.Windows.Forms.Control)(object)this.cmbSalaryFactor).Name = "cmbSalaryFactor";
		((System.Windows.Forms.Control)(object)this.cmbSalaryFactor).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmbSalaryFactor).TabIndex = 0;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(7, 23);
		this.label48.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(75, 15);
		this.label48.TabIndex = 13;
		this.label48.Text = "Salary Factor";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(287, 23);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 19;
		this.label1.Text = "From Date";
		this.txtfromdate.CustomFormat = "dd-MMM-yy";
		this.txtfromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtfromdate.Location = new System.Drawing.Point(353, 19);
		this.txtfromdate.Name = "txtfromdate";
		this.txtfromdate.Size = new System.Drawing.Size(181, 23);
		this.txtfromdate.TabIndex = 5;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(287, 50);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 15);
		this.label2.TabIndex = 18;
		this.label2.Text = "To Date";
		this.txtminyearlimit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtminyearlimit.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtminyearlimit.Location = new System.Drawing.Point(90, 46);
		this.txtminyearlimit.Margin = new System.Windows.Forms.Padding(4);
		this.txtminyearlimit.Name = "txtminyearlimit";
		this.txtminyearlimit.Size = new System.Drawing.Size(178, 23);
		this.txtminyearlimit.TabIndex = 2;
		this.txttodate.CustomFormat = "dd-MMM-yy";
		this.txttodate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txttodate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txttodate.Location = new System.Drawing.Point(353, 46);
		this.txttodate.Name = "txttodate";
		this.txttodate.Size = new System.Drawing.Size(181, 23);
		this.txttodate.TabIndex = 6;
		this.txtpolicydescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtpolicydescription.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtpolicydescription.Location = new System.Drawing.Point(90, 74);
		this.txtpolicydescription.Margin = new System.Windows.Forms.Padding(4);
		this.txtpolicydescription.Name = "txtpolicydescription";
		this.txtpolicydescription.Size = new System.Drawing.Size(444, 23);
		this.txtpolicydescription.TabIndex = 3;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(7, 50);
		this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(83, 15);
		this.label8.TabIndex = 21;
		this.label8.Text = "Min Year Limit";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(7, 78);
		this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(68, 15);
		this.label3.TabIndex = 20;
		this.label3.Text = "Description";
		this.groupBox1.Controls.Add(this.txtamount);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.txtpercent);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox1.Controls.Add(this.btnUpdateDetail);
		this.groupBox1.Controls.Add(this.btnplus);
		this.groupBox1.Controls.Add(this.txttosalary);
		this.groupBox1.Controls.Add(this.txtfromsalary);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(26, 124);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(675, 73);
		this.groupBox1.TabIndex = 1;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.txtamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtamount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtamount.Location = new System.Drawing.Point(386, 44);
		this.txtamount.Margin = new System.Windows.Forms.Padding(4);
		this.txtamount.Name = "txtamount";
		this.txtamount.Size = new System.Drawing.Size(95, 23);
		this.txtamount.TabIndex = 17;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(387, 23);
		this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(73, 15);
		this.label11.TabIndex = 18;
		this.label11.Text = "Flat Amount";
		this.txtpercent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtpercent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtpercent.Location = new System.Drawing.Point(289, 44);
		this.txtpercent.Margin = new System.Windows.Forms.Padding(4);
		this.txtpercent.Name = "txtpercent";
		this.txtpercent.Size = new System.Drawing.Size(95, 23);
		this.txtpercent.TabIndex = 2;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(286, 23);
		this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(57, 15);
		this.label7.TabIndex = 16;
		this.label7.Text = "Percent%";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(601, 42);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(68, 26);
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
		this.btnUpdateDetail.Location = new System.Drawing.Point(528, 42);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(68, 26);
		this.btnUpdateDetail.TabIndex = 4;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(485, 42);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(39, 26);
		this.btnplus.TabIndex = 3;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.txttosalary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txttosalary.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txttosalary.Location = new System.Drawing.Point(150, 44);
		this.txttosalary.Margin = new System.Windows.Forms.Padding(4);
		this.txttosalary.Name = "txttosalary";
		this.txttosalary.Size = new System.Drawing.Size(135, 23);
		this.txttosalary.TabIndex = 1;
		this.txtfromsalary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfromsalary.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfromsalary.Location = new System.Drawing.Point(12, 44);
		this.txtfromsalary.Margin = new System.Windows.Forms.Padding(4);
		this.txtfromsalary.Name = "txtfromsalary";
		this.txtfromsalary.Size = new System.Drawing.Size(135, 23);
		this.txtfromsalary.TabIndex = 0;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(147, 23);
		this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(55, 15);
		this.label4.TabIndex = 12;
		this.label4.Text = "To Salary";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(17, 23);
		this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(70, 15);
		this.label5.TabIndex = 10;
		this.label5.Text = "From Salary";
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label6);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 27);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(860, 31);
		this.panel4.TabIndex = 1;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(6, 6);
		this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(128, 19);
		this.label6.TabIndex = 11;
		this.label6.Text = "Social Security";
		this.panel3.Controls.Add(this.tsItemType);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(860, 27);
		this.panel3.TabIndex = 0;
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnupdate, this.btnsave, this.btnRefresh });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(860, 27);
		this.tsItemType.TabIndex = 2;
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
		this.btnupdate.Visible = false;
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
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(866, 581);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add(this.panel9);
		this.panel6.Controls.Add(this.panel8);
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(860, 575);
		this.panel6.TabIndex = 0;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryDetail);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 334);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(860, 241);
		this.panel9.TabIndex = 2;
		this.grdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryDetail.FilterMode = (FilterMode)2;
		this.grdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryDetail.GroupByBoxVisible = false;
		this.grdHistoryDetail.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Name = "grdHistoryDetail";
		((ScrollableBase)this.grdHistoryDetail).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdHistoryDetail).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(860, 241);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabIndex = 2;
		this.grdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 33);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(860, 301);
		this.panel8.TabIndex = 1;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupByBoxVisible = false;
		this.grdHistory.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistory).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdHistory).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(860, 301);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 1;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label10);
		this.panel7.Controls.Add(this.ctrlGrdBar1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(860, 33);
		this.panel7.TabIndex = 0;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.White;
		this.label10.Location = new System.Drawing.Point(6, 7);
		this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(67, 19);
		this.label10.TabIndex = 23;
		this.label10.Text = "History";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(793, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grdHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(60, 27);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(874, 607);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "SocialSecurity";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(SocialSecurity_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(SocialSecurity_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSalaryFactor).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).EndInit();
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		base.ResumeLayout(false);
	}
}
