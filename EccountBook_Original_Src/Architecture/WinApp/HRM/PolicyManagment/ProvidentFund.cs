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

public class ProvidentFund : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private int RecId;

	private int updateDetailIndex;

	private bool UpdateMode;

	private DataTable table = new DataTable();

	private DataTable dtEmployee = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private Panel panel3;

	private Panel panel4;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStrip tsItemType;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	internal UltraCombo cmbSalaryFector;

	private GroupBox groupBox1;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private Button btnplus;

	private TextBox txtpercent;

	private Label label6;

	private TextBox txttosalary;

	private TextBox txtfromsalary;

	private Label label4;

	private Label label5;

	private TextBox txtpolicydescription;

	private Label label3;

	private DateTimePicker txtdateto;

	private DateTimePicker txtdatefrom;

	private Label label1;

	private Label label2;

	private Label label48;

	private Panel panel8;

	private Panel panel10;

	private Panel panel9;

	internal GridEX grdHistory;

	private CtrlGrdBar ctrlGrdBar2;

	internal GridEX grdfrm;

	private ToolStripButton btnRefresh;

	private Panel panel7;

	private Panel panel6;

	private GroupBox groupBox2;

	private Panel panel5;

	private Button btnsalaryfactor;

	private Label label8;

	private Label label7;

	private Panel panel11;

	internal GridEX grddetail;

	private Label label9;

	private TextBox txtCompanyshare;

	private Label label10;

	public ProvidentFund(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void ProvidentFund_Load(object sender, EventArgs e)
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
			ctrlGrdBar2.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			ProfileType();
			table.Columns.Add("FromSalary", typeof(double));
			table.Columns.Add("ToSalary", typeof(double));
			table.Columns.Add("Employee%", typeof(double));
			table.Columns.Add("Company%", typeof(double));
			grdfrm.DataSource = table;
			grdfrm.RetrieveStructure();
			grdSettings();
			btnCancelUpdateDetial.Visible = false;
			btnUpdateDetail.Visible = false;
			cmbSalaryFector.Focus();
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
				DDL.BindDDLNew(dt, cmbSalaryFector, "ProfileId", "ProfileName", "BenefitProfile", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formvalidation()
	{
		if (((Control)(object)cmbSalaryFector).Text.Trim() == "")
		{
			MessageBox.Show("Please Select SalaryFactor");
			cmbSalaryFector.Focus();
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

	private bool DetailValidation()
	{
		if (Conversion.ToDouble((object)txtpercent.Text) > 100.0)
		{
			MessageBox.Show("Percent should not greater then 100");
			txtpercent.Focus();
			return false;
		}
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
		if (txtpercent.Text.Trim() == "")
		{
			MessageBox.Show("Please Insert Employee Percent");
			txtpercent.Focus();
			return false;
		}
		if (txtCompanyshare.Text.Trim() == "")
		{
			MessageBox.Show("Please Insert Company Percent");
			txtCompanyshare.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtpercent.Text) + Conversion.ToDouble((object)txtCompanyshare.Text) > 100.0)
		{
			MessageBox.Show("The sum of the Employee % and Company % should not exceed 100.");
			txtCompanyshare.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtfromsalary.Text) > Conversion.ToDouble((object)txttosalary.Text))
		{
			MessageBox.Show("Please Check From Salary Is Greater Than To Salary");
			txtfromsalary.Focus();
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
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Expected O, but got Unknown
		try
		{
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
			if (!formvalidation())
			{
				return;
			}
			if (txtdatefrom.Value > txtdateto.Value)
			{
				MessageBox.Show("DateFrom Is Greater Than Date To please Check: Thank You");
			}
			else if (grdfrm.RowCount > 0)
			{
				hrmPFPolicy pFPolicy = new hrmPFPolicy();
				pFPolicy.PFPolicyId = RecId;
				pFPolicy.OrganizationId = Conversion.ToInt((object)UserAccount.OrganizationId);
				pFPolicy.CompanyId = Conversion.ToInt((object)UserAccount.CompanyId);
				pFPolicy.CreatedOn = Conversion.ToDateTime((object)DateTime.Today);
				pFPolicy.CreatedById = Conversion.ToInt((object)UserAccount.ID);
				pFPolicy.AlteredById = Conversion.ToInt((object)UserAccount.ID);
				pFPolicy.AlteredOn = Conversion.ToDateTime((object)DateTime.Today);
				pFPolicy.FromDate = Conversion.ToDateTime((object)txtdatefrom.Value);
				pFPolicy.ToDate = Conversion.ToDateTime((object)txtdateto.Value);
				pFPolicy.PolicyDescription = Conversion.ToString((object)txtpolicydescription.Text.Trim());
				pFPolicy.SalaryFactorProfileId = Conversion.ToInt(cmbSalaryFector.Value);
				if (int.Parse(grdfrm.GetTotal(grdfrm.RootTable.Columns["Employee%"], (AggregateFunction)2).ToString()) > 100)
				{
					MessageBox.Show("Your Total Percent Is Greater Than 100 Please Check It");
					return;
				}
				pFPolicy.PolicyDetail = new List<hrmPFPolicySlab>();
				pFPolicy.PFPolicyEmployeesList = new List<hrmPFPolicyEmployee>();
				GridEXRow[] rows = grdfrm.GetRows();
				foreach (GridEXRow r in rows)
				{
					hrmPFPolicySlab PFSLabDetail = new hrmPFPolicySlab();
					PFSLabDetail.FromSalary = Conversion.ToInt(r.Cells["FromSalary"].Value);
					PFSLabDetail.ToSalary = Conversion.ToInt(r.Cells["ToSalary"].Value);
					PFSLabDetail.PFPercent = Conversion.ToInt(r.Cells["Employee%"].Value);
					PFSLabDetail.CompanyPercent = Conversion.ToInt(r.Cells["Company%"].Value);
					PFSLabDetail.PFPolicySlabLineId = r.RowIndex;
					pFPolicy.PolicyDetail.Add(PFSLabDetail);
				}
				hrmPFPolicy.Save(pFPolicy);
				if (RecId == 0)
				{
					MessageBox.Show("Save Successfully");
					Resetmain();
					resetdetail();
				}
				else
				{
					MessageBox.Show("Update Successfully");
					Resetmain();
					resetdetail();
				}
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

	private void RetrivedData(int Id)
	{
		try
		{
			hrmPFPolicy obj = hrmPFPolicy.GetByID(Id);
			if (obj == null)
			{
				return;
			}
			RecId = obj.PFPolicyId;
			cmbSalaryFector.Value = Conversion.ToInt((object)obj.SalaryFactorProfileId);
			txtpolicydescription.Text = Conversion.ToString((object)obj.PolicyDescription);
			txtdatefrom.Value = Conversion.ToDateTime((object)obj.FromDate);
			txtdateto.Value = Conversion.ToDateTime((object)obj.ToDate);
			if (obj.PolicyDetail.Count > 0)
			{
				table.Rows.Clear();
				for (int i = 0; i < obj.PolicyDetail.Count; i++)
				{
					table.Rows.Add(obj.PolicyDetail[i].FromSalary, obj.PolicyDetail[i].ToSalary, obj.PolicyDetail[i].PFPercent, obj.PolicyDetail[i].CompanyPercent);
				}
				grdfrm.DataSource = table;
				grdfrm.RetrieveStructure();
				grdSettings();
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

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			if (grdfrm.GetRows().Length != 0)
			{
				GridEXRow[] rows = grdfrm.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToDouble((object)txtfromsalary.Text) < Conversion.ToDouble((object)r.Cells["FromSalary"].Text))
					{
						MessageBox.Show("From Salary Should Not Greater then Record that Already Exist");
						return;
					}
				}
			}
			if (DetailValidation())
			{
				table.Rows.Add(txtfromsalary.Text.Trim(), txttosalary.Text.Trim(), txtpercent.Text.Trim(), txtCompanyshare.Text.Trim());
				grdfrm.DataSource = table;
				grdfrm.RetrieveStructure();
				grdSettings();
				double tosalary = Conversion.ToDouble((object)txttosalary.Text);
				txtfromsalary.Text = (1.0 + tosalary).ToString();
				resetdetail();
				txtfromsalary.ReadOnly = true;
				txttosalary.Focus();
			}
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
				txtpercent.Text = item.Cells["Employee%"].Value.ToString();
				txtCompanyshare.Text = item.Cells["Company%"].Value.ToString();
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				grdfrm.DataSource = table;
				grdfrm.RetrieveStructure();
				grdSettings();
				txtfromsalary.Enabled = false;
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
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!DetailValidation())
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
			table.Rows[updateDetailIndex]["FromSalary"] = txtfromsalary.Text;
			table.Rows[updateDetailIndex]["ToSalary"] = txttosalary.Text;
			table.Rows[updateDetailIndex]["Employee%"] = txtpercent.Text;
			table.Rows[updateDetailIndex]["Company%"] = txtCompanyshare.Text;
			grdfrm.DataSource = table;
			grdfrm.RetrieveStructure();
			grdSettings();
			resetdetail();
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
			grdfrm.RootTable.Columns["FromSalary"].Width = 100;
			grdfrm.RootTable.Columns["ToSalary"].Width = 100;
			grdfrm.RootTable.Columns["Employee%"].Width = 100;
			grdfrm.RootTable.Columns["Company%"].Width = 100;
			grdfrm.RootTable.Columns["FromSalary"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["ToSalary"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["Employee%"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["Company%"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["FromSalary"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["ToSalary"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["Employee%"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["Company%"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["Employee%"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["Company%"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns.Add("Delete");
			grdfrm.RootTable.Columns["Delete"].Key = "Delete";
			grdfrm.RootTable.Columns["Delete"].Caption = " X";
			grdfrm.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdfrm.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdfrm.RootTable.Columns["Delete"].ButtonText = " X ";
			grdfrm.RootTable.Columns["Delete"].Width = 20;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw ex;
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

	private void resetdetail()
	{
		btnplus.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
		txttosalary.Text = "";
		txtpercent.Text = "";
		txtCompanyshare.Text = "";
		txtfromsalary.Enabled = true;
	}

	private void Resetmain()
	{
		btnsave.Visible = true;
		btnupdate.Visible = false;
		UpdateMode = false;
		RecId = 0;
		txtpolicydescription.Text = "";
		((Control)(object)cmbSalaryFector).Text = "";
		txtdatefrom.Value = DateTime.Today;
		txtdateto.Value = DateTime.Today;
		cmbSalaryFector.Focus();
		txtfromsalary.ReadOnly = false;
		txtfromsalary.Text = string.Empty;
		table.Rows.Clear();
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Resetmain();
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

	private void ProvidentFund_KeyDown(object sender, KeyEventArgs e)
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
				btnUpdateDetail_Click(null, null);
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

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
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

	private void HistoryGridFill()
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("SalaryFactor");
			dt.Columns.Add("FromDate");
			dt.Columns.Add("ToDate");
			dt.Columns.Add("PolicyDescription");
			DataTable dtgrid = hrmPFPolicy.Getall(new hrmPFPolicy
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtgrid.Rows.Count > 0)
			{
				for (int i = 0; i < dtgrid.Rows.Count; i++)
				{
					dt.Rows.Add(dtgrid.Rows[i]["PFPolicyId"], dtgrid.Rows[i]["SalaryFactor"], Conversion.ToDateTime(dtgrid.Rows[i]["FromDate"]).ToString("dd-MMM-yyyy"), Conversion.ToDateTime(dtgrid.Rows[i]["ToDate"]).ToString("dd-MMM-yyyy"), dtgrid.Rows[i]["PolicyDescription"]);
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
			grdHistory.RootTable.Columns["Id"].Visible = false;
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
			grdHistory.RootTable.Columns["Edit"].Position = 0;
			grdHistory.RootTable.Columns["Detail"].Position = 1;
			grdHistory.FrozenColumns = 2;
			ctrlGrdBar2_Load(null, null);
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
			if (e.Column.Key == "Edit")
			{
				RetrivedData(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "Detail")
			{
				DetailGridBind();
			}
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
			RetrivedData(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
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
				HistoryGridFill();
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
			if (grdHistory.SelectedItems.Count <= 0)
			{
				return;
			}
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				hrmPFPolicy cat = new hrmPFPolicy();
				RecId = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
				cat = hrmPFPolicy.GetByID(RecId);
				cat.PFPolicyId = RecId;
				DataTable Detailtable = new DataTable();
				Detailtable.Columns.Add("FromSalary");
				Detailtable.Columns.Add("ToSalary");
				Detailtable.Columns.Add("Employee%", typeof(double));
				Detailtable.Columns.Add("Company%", typeof(double));
				Detailtable.Columns.Add("Total%", typeof(double));
				Detailtable.Clear();
				grddetail.DataSource = Detailtable;
				for (int i = 0; i < cat.PolicyDetail.Count; i++)
				{
					Detailtable.Rows.Add(cat.PolicyDetail[i].FromSalary, cat.PolicyDetail[i].ToSalary, Conversion.ToDouble((object)cat.PolicyDetail[i].PFPercent), Conversion.ToDouble((object)cat.PolicyDetail[i].CompanyPercent), Conversion.ToDouble((object)cat.PolicyDetail[i].PFPercent) * Conversion.ToDouble((object)cat.PolicyDetail[i].CompanyPercent));
				}
				grddetail.DataSource = Detailtable;
				grddetail.RetrieveStructure();
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
			grddetail.RootTable.Columns["FromSalary"].FilterEditType = (FilterEditType)1;
			grddetail.RootTable.Columns["ToSalary"].FilterEditType = (FilterEditType)1;
			grddetail.RootTable.Columns["Employee%"].FilterEditType = (FilterEditType)1;
			grddetail.RootTable.Columns["Company%"].FilterEditType = (FilterEditType)1;
			grddetail.RootTable.Columns["Total%"].FilterEditType = (FilterEditType)1;
			grddetail.RootTable.Columns["FromSalary"].EditType = (EditType)0;
			grddetail.RootTable.Columns["ToSalary"].EditType = (EditType)0;
			grddetail.RootTable.Columns["Employee%"].EditType = (EditType)0;
			grddetail.RootTable.Columns["Company%"].EditType = (EditType)0;
			grddetail.RootTable.Columns["Total%"].EditType = (EditType)0;
			grddetail.RootTable.Columns["FromSalary"].Width = 100;
			grddetail.RootTable.Columns["ToSalary"].Width = 100;
			grddetail.RootTable.Columns["Employee%"].Width = 100;
			grddetail.RootTable.Columns["Company%"].Width = 100;
			grddetail.RootTable.Columns["Total%"].Width = 100;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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

	private void txtpercent_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtCompanyshare_KeyPress(object sender, KeyPressEventArgs e)
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
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Expected O, but got Unknown
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Expected O, but got Unknown
		//IL_075d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0767: Expected O, but got Unknown
		//IL_24ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_24b7: Expected O, but got Unknown
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.PolicyManagment.ProvidentFund));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdfrm = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label8 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.btnsalaryfactor = new System.Windows.Forms.Button();
		this.cmbSalaryFector = new UltraCombo();
		this.label48 = new System.Windows.Forms.Label();
		this.txtpolicydescription = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtdatefrom = new System.Windows.Forms.DateTimePicker();
		this.label1 = new System.Windows.Forms.Label();
		this.txtdateto = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtCompanyshare = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.btnplus = new System.Windows.Forms.Button();
		this.txtpercent = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.txttosalary = new System.Windows.Forms.TextBox();
		this.txtfromsalary = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label7 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.grddetail = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel7.SuspendLayout();
		this.panel6.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSalaryFector).BeginInit();
		this.groupBox1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.tsItemType.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grddetail).BeginInit();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel9.SuspendLayout();
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
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(956, 635);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.panel7);
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(950, 629);
		this.panel2.TabIndex = 0;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 297);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(950, 332);
		this.panel5.TabIndex = 21;
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
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(950, 332);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 0;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.ColumnButtonClick += new ColumnActionEventHandler(grdfrm_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdfrm).DoubleClick += new System.EventHandler(grdfrm_DoubleClick);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar1);
		this.panel7.Controls.Add(this.label8);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 268);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(950, 29);
		this.panel7.TabIndex = 20;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(888, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(60, 27);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(6, 3);
		this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(46, 17);
		this.label8.TabIndex = 13;
		this.label8.Text = "Detail";
		this.panel6.Controls.Add(this.groupBox2);
		this.panel6.Controls.Add(this.groupBox1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 59);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(950, 209);
		this.panel6.TabIndex = 19;
		this.groupBox2.Controls.Add(this.btnsalaryfactor);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSalaryFector);
		this.groupBox2.Controls.Add(this.label48);
		this.groupBox2.Controls.Add(this.txtpolicydescription);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add(this.txtdatefrom);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.txtdateto);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(19, 6);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(731, 104);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main";
		this.btnsalaryfactor.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnsalaryfactor.Location = new System.Drawing.Point(258, 17);
		this.btnsalaryfactor.Name = "btnsalaryfactor";
		this.btnsalaryfactor.Size = new System.Drawing.Size(28, 26);
		this.btnsalaryfactor.TabIndex = 1;
		this.btnsalaryfactor.TabStop = false;
		this.btnsalaryfactor.UseVisualStyleBackColor = true;
		this.btnsalaryfactor.Click += new System.EventHandler(btnsalaryfactor_Click);
		this.cmbSalaryFector.AllowNull = (DefaultableBoolean)1;
		this.cmbSalaryFector.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSalaryFector.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSalaryFector.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSalaryFector).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbSalaryFector).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSalaryFector).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSalaryFector).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSalaryFector).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbSalaryFector).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSalaryFector).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbSalaryFector).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSalaryFector).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSalaryFector).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSalaryFector).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbSalaryFector).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSalaryFector).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSalaryFector).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSalaryFector).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbSalaryFector).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSalaryFector).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSalaryFector).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSalaryFector).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbSalaryFector).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSalaryFector).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSalaryFector).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbSalaryFector).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSalaryFector).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbSalaryFector).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSalaryFector).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSalaryFector).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSalaryFector).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSalaryFector.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSalaryFector).Location = new System.Drawing.Point(94, 17);
		((System.Windows.Forms.Control)(object)this.cmbSalaryFector).Name = "cmbSalaryFector";
		((System.Windows.Forms.Control)(object)this.cmbSalaryFector).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.cmbSalaryFector).TabIndex = 0;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(6, 23);
		this.label48.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(75, 15);
		this.label48.TabIndex = 11;
		this.label48.Text = "Salary Factor";
		this.txtpolicydescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtpolicydescription.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtpolicydescription.Location = new System.Drawing.Point(398, 17);
		this.txtpolicydescription.Margin = new System.Windows.Forms.Padding(4);
		this.txtpolicydescription.Multiline = true;
		this.txtpolicydescription.Name = "txtpolicydescription";
		this.txtpolicydescription.Size = new System.Drawing.Size(327, 80);
		this.txtpolicydescription.TabIndex = 4;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(293, 23);
		this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(103, 15);
		this.label3.TabIndex = 17;
		this.label3.Text = "Policy Description";
		this.txtdatefrom.CustomFormat = "dd-MMM-yy";
		this.txtdatefrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdatefrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdatefrom.Location = new System.Drawing.Point(94, 47);
		this.txtdatefrom.Name = "txtdatefrom";
		this.txtdatefrom.Size = new System.Drawing.Size(192, 23);
		this.txtdatefrom.TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(6, 51);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 16;
		this.label1.Text = "From Date";
		this.txtdateto.CustomFormat = "dd-MMM-yy";
		this.txtdateto.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdateto.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdateto.Location = new System.Drawing.Point(94, 74);
		this.txtdateto.Name = "txtdateto";
		this.txtdateto.Size = new System.Drawing.Size(192, 23);
		this.txtdateto.TabIndex = 3;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(6, 78);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 15);
		this.label2.TabIndex = 15;
		this.label2.Text = "To Date";
		this.groupBox1.Controls.Add(this.txtCompanyshare);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox1.Controls.Add(this.btnUpdateDetail);
		this.groupBox1.Controls.Add(this.btnplus);
		this.groupBox1.Controls.Add(this.txtpercent);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.txttosalary);
		this.groupBox1.Controls.Add(this.txtfromsalary);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(19, 118);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(731, 69);
		this.groupBox1.TabIndex = 1;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.txtCompanyshare.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCompanyshare.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCompanyshare.Location = new System.Drawing.Point(406, 38);
		this.txtCompanyshare.Margin = new System.Windows.Forms.Padding(4);
		this.txtCompanyshare.Name = "txtCompanyshare";
		this.txtCompanyshare.Size = new System.Drawing.Size(127, 23);
		this.txtCompanyshare.TabIndex = 3;
		this.txtCompanyshare.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCompanyshare_KeyPress);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(411, 19);
		this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(71, 15);
		this.label10.TabIndex = 16;
		this.label10.Text = "Company %";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(650, 36);
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
		this.btnUpdateDetail.Location = new System.Drawing.Point(579, 36);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(68, 26);
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
		this.btnplus.Location = new System.Drawing.Point(537, 36);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(39, 26);
		this.btnplus.TabIndex = 4;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.txtpercent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtpercent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtpercent.Location = new System.Drawing.Point(271, 38);
		this.txtpercent.Margin = new System.Windows.Forms.Padding(4);
		this.txtpercent.Name = "txtpercent";
		this.txtpercent.Size = new System.Drawing.Size(132, 23);
		this.txtpercent.TabIndex = 2;
		this.txtpercent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtpercent_KeyPress);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(277, 19);
		this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(72, 15);
		this.label6.TabIndex = 14;
		this.label6.Text = "Employee %";
		this.txttosalary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txttosalary.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txttosalary.Location = new System.Drawing.Point(133, 38);
		this.txttosalary.Margin = new System.Windows.Forms.Padding(4);
		this.txttosalary.Name = "txttosalary";
		this.txttosalary.Size = new System.Drawing.Size(133, 23);
		this.txttosalary.TabIndex = 1;
		this.txttosalary.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txttosalary_KeyPress);
		this.txtfromsalary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfromsalary.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfromsalary.Location = new System.Drawing.Point(7, 38);
		this.txtfromsalary.Margin = new System.Windows.Forms.Padding(4);
		this.txtfromsalary.Name = "txtfromsalary";
		this.txtfromsalary.Size = new System.Drawing.Size(122, 23);
		this.txtfromsalary.TabIndex = 0;
		this.txtfromsalary.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtfromsalary_KeyPress);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(137, 19);
		this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(110, 15);
		this.label4.TabIndex = 12;
		this.label4.Text = "To Salary (Amount)";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(4, 19);
		this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(125, 15);
		this.label5.TabIndex = 10;
		this.label5.Text = "From Salary (Amount)";
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label7);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 29);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(950, 30);
		this.panel4.TabIndex = 1;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(6, 6);
		this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(132, 19);
		this.label7.TabIndex = 12;
		this.label7.Text = "Provident Fund";
		this.panel3.Controls.Add(this.tsItemType);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(950, 29);
		this.panel3.TabIndex = 0;
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnupdate, this.btnsave, this.btnRefresh });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(950, 27);
		this.tsItemType.TabIndex = 21;
		this.tsItemType.Text = "toolStrip4";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(60, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(79, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(65, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(81, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(956, 635);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add(this.panel11);
		this.panel8.Controls.Add(this.panel10);
		this.panel8.Controls.Add(this.panel9);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(950, 629);
		this.panel8.TabIndex = 0;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.grddetail);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(0, 480);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(950, 149);
		this.panel11.TabIndex = 2;
		this.grddetail.AllowDelete = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grddetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grddetail.FilterMode = (FilterMode)2;
		this.grddetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grddetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grddetail.GroupByBoxVisible = false;
		this.grddetail.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grddetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grddetail).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grddetail).Name = "grddetail";
		((ScrollableBase)this.grddetail).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grddetail).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grddetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grddetail).Size = new System.Drawing.Size(950, 149);
		((System.Windows.Forms.Control)(object)this.grddetail).TabIndex = 4;
		this.grddetail.TotalRow = (InheritableBoolean)1;
		this.grddetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grddetail).VisualStyle = (VisualStyle)4;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 32);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(950, 448);
		this.panel10.TabIndex = 1;
		this.grdHistory.AllowDelete = (InheritableBoolean)1;
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
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(950, 448);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 4;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label9);
		this.panel9.Controls.Add(this.ctrlGrdBar2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(950, 32);
		this.panel9.TabIndex = 0;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(6, 7);
		this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(67, 19);
		this.label9.TabIndex = 22;
		this.label9.Text = "History";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(898, 3);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(48, 27);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 661);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "ProvidentFund";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(ProvidentFund_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ProvidentFund_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSalaryFector).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grddetail).EndInit();
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		base.ResumeLayout(false);
	}
}
