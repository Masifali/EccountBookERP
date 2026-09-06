using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.pcc;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.pcc;
using Architecture.WinApp.Common;
using Architecture.WinApp.Inventory_Definition;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Pcc;

public class ContractorWagesRateSchedule : Form
{
	private int RecId;

	public bool UpdateMode;

	private DropDownBind DDL = new DropDownBind();

	public UserAccount UserAccount = new UserAccount();

	private DataTable dtgetAll = new DataTable();

	private DataTable dtWagesGroup = new DataTable();

	private DataTable dtItems = new DataTable();

	public Rightsobjects formrights = new Rightsobjects();

	private IContainer components;

	private TabControl tabWagesSchedule;

	private TabPage tabPage1;

	internal GridEX grdWagesSchedule;

	private Panel panel2;

	private Label label10;

	internal UltraCombo combAccountName;

	private TextBox txtMeatureMTo;

	private TextBox txtMeatureMFrom;

	private Label label18;

	private Label label12;

	private Panel panel6;

	private Label label15;

	private ToolStrip btnAttachments;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private TabPage tabPage2;

	private DateTimePicker txtdate;

	private Label label3;

	internal GridEX gridhistory;

	private Panel panel1;

	private Label label4;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private TextBox txtqty;

	private Label label8;

	private TextBox txtRate;

	private Label label7;

	internal UltraCombo CmbWagesGroup;

	private ToolStripButton btnshortcutkeys;

	internal UltraCombo cmbrangeuom;

	private Label label1;

	private Panel panel5;

	private Label label5;

	private Panel panel4;

	private CheckBox isActive;

	private GroupBox groupBox1;

	private BackgroundWorker backgroundWorker1;

	private ToolStripButton btnrefrsh;

	private ToolStripButton btnDelete;

	private TextBox txtMeatureSTo;

	private TextBox txtMeatureSFrom;

	private Button BtnWagesActivity;

	private Button btnWagesGroupDefine;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator3;

	private Button BtnDefineRateUom;

	private RadioButton radItem;

	private RadioButton radWagesGroup;

	private Panel panel7;

	private Button btnshowHistory;

	private DateTimePicker txtToDateHistory;

	private Label label20;

	private DateTimePicker FromDateHistory;

	private Label label21;

	internal UltraCombo CmbWagesAccountHistory;

	private Label label22;

	internal UltraCombo CmbGroupNameHistory;

	private Label label23;

	private Panel panel12;

	private Label label24;

	private ToolStrip toolStrip2;

	private ToolStripButton BtnNewHistory;

	private ToolStripButton BtnRefreshHistory;

	private Button BtnSaveHistory;

	public ContractorWagesRateSchedule(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmContractWagesSchedule_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(815, 749);
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formrights.DoHaveSaveRight;
			BtnSaveHistory.Enabled = formrights.DoHaveSaveRight;
			btnUpdate.Enabled = formrights.DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			ctrlGrdBar2.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			dtItems.Columns.Add("Id", typeof(int));
			dtItems.Columns.Add("Name");
			dtWagesGroup.Columns.Add("Id", typeof(int));
			dtWagesGroup.Columns.Add("Name");
			FilldtWagesAndItems();
			WagesGroupFill();
			FillUOM();
			accountName();
			txtdate.Focus();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			ComboFromWagesScheduleBind(ComboFromWagesScheduleDbCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FilldtWagesAndItems()
	{
		try
		{
			dtWagesGroup.Rows.Clear();
			dtItems.Rows.Clear();
			DataTable dt = ContractorWagesGroup.FormHistory();
			if (dt.Rows.Count > 0)
			{
				for (int j = 0; j < dt.Rows.Count; j++)
				{
					dtWagesGroup.Rows.Add(dt.Rows[j]["Id"], dt.Rows[j]["ItemWagesGroupName"]);
				}
			}
			DataTable dt2 = ItemAllocationToWagesGroup.ItemNotReferedInWagesGroupAllocation(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.BranchesId);
			if (dt2.Rows.Count > 0)
			{
				for (int i = 0; i < dt2.Rows.Count; i++)
				{
					dtItems.Rows.Add(dt2.Rows[i]["ItemId"], dt2.Rows[i]["ItemName"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WagesGroupFill()
	{
		try
		{
			int BId = Conversion.ToInt(CmbWagesGroup.Value);
			if (radItem.Checked)
			{
				if (dtItems.Rows.Count > 0)
				{
					DDL.BindDDL(dtItems, CmbWagesGroup, "Id", "Name", "Item Name", ZeroIndex: true);
					if (dtItems.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == BId))
					{
						CmbWagesGroup.Value = BId;
					}
					else
					{
						((Control)(object)CmbWagesGroup).Text = string.Empty;
					}
				}
				else
				{
					((Control)(object)CmbWagesGroup).Text = "";
					((UltraGridBase)CmbWagesGroup).DataSource = null;
				}
			}
			else if (dtWagesGroup.Rows.Count > 0)
			{
				DDL.BindDDL(dtWagesGroup, CmbWagesGroup, "Id", "Name", "Group Name", ZeroIndex: true);
				if (dtWagesGroup.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == BId))
				{
					CmbWagesGroup.Value = BId;
				}
				else
				{
					((Control)(object)CmbWagesGroup).Text = string.Empty;
				}
			}
			else
			{
				((Control)(object)CmbWagesGroup).Text = "";
				((UltraGridBase)CmbWagesGroup).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillUOM()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(cmbrangeuom.Value);
			DataTable dt = UOM.Getall(new UOM
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtUom = new DataTable();
			dtUom.Columns.Add("Id", typeof(int));
			dtUom.Columns.Add("UOMCode", typeof(string));
			dtUom.Columns.Add("ParentUOMId", typeof(int));
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				dtUom.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["UOMCode"], dt.Rows[i]["ParentUOMId"]);
			}
			if (dtUom.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtUom, cmbrangeuom, "Id", "UOMCode", "Range UOM", ZeroIndex: true);
			if (Id > 0)
			{
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					cmbrangeuom.Value = Id;
				}
				else
				{
					((Control)(object)cmbrangeuom).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void accountName()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			int AId = Conversion.ToInt(combAccountName.Value);
			DataTable dt = ContractorWagesActivity.FormHistory(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, combAccountName, "Id", "WagesActivity", "Wages Service Activity", true);
				((UltraGridBase)combAccountName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (AId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == AId))
				{
					combAccountName.Value = AId;
				}
				else
				{
					((Control)(object)combAccountName).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combAccountName_Leave(object sender, EventArgs e)
	{
		if (Conversion.ToInt(combAccountName.Value) > 0)
		{
			BindGrid();
		}
	}

	private void cmbrangeuom_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbrangeuom).ActiveRow != null && Conversion.ToInt(cmbrangeuom.Value) != 0)
			{
				int ParentId = Conversion.ToInt(((UltraDropDownBase)cmbrangeuom).SelectedRow.Cells["ParentUOMId"].Value);
				txtMeatureSFrom.Enabled = ParentId == 1 || ParentId == 3;
				txtMeatureSTo.Enabled = ParentId == 1 || ParentId == 3;
			}
			else
			{
				txtMeatureSFrom.Enabled = false;
				txtMeatureSTo.Enabled = false;
			}
			txtMeatureSFrom_TextChanged(null, null);
			txtMeatureSTo_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void radWagesGroup_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (radItem.Checked)
			{
				if (dtItems.Rows.Count > 0)
				{
					DDL.BindDDL(dtItems, CmbWagesGroup, "Id", "Name", "Item Name", ZeroIndex: true);
					return;
				}
				((Control)(object)CmbWagesGroup).Text = "";
				((UltraGridBase)CmbWagesGroup).DataSource = null;
			}
			else if (dtWagesGroup.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtWagesGroup, CmbWagesGroup, "Id", "Name", "Item Name", true);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FormRest()
	{
		try
		{
			RecId = 0;
			((Control)(object)CmbWagesGroup).Text = string.Empty;
			((Control)(object)cmbrangeuom).Text = string.Empty;
			((Control)(object)combAccountName).Text = string.Empty;
			txtMeatureMTo.Text = "0";
			txtMeatureMFrom.Text = "0";
			txtMeatureSTo.Text = "0";
			txtMeatureSFrom.Text = "0";
			txtRate.Text = "0";
			txtqty.Text = "0";
			BindGridHistory();
			BindGrid();
			txtdate.Focus();
			grdWagesSchedule.ClearStructure();
			txtMeatureSFrom.Enabled = true;
			txtMeatureSTo.Enabled = true;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			UpdateMode = false;
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
			FormRest();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void isActive_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "ctrlGrdBar1")
			{
				txtdate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnrefrsh_Click(object sender, EventArgs e)
	{
		try
		{
			FilldtWagesAndItems();
			WagesGroupFill();
			FillUOM();
			accountName();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmContractWagesSchedule_FormClosing(object sender, FormClosingEventArgs e)
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

	private bool FormValidation()
	{
		if (((UltraGridBase)CmbWagesGroup).ActiveRow == null || Conversion.ToInt(CmbWagesGroup.Value) == 0)
		{
			MessageBox.Show("Item Field is Required");
			CmbWagesGroup.Focus();
			return false;
		}
		if (((UltraGridBase)combAccountName).ActiveRow == null || Conversion.ToInt(combAccountName.Value) == 0)
		{
			MessageBox.Show("Wages Account Field is Required");
			combAccountName.Focus();
			return false;
		}
		if (((UltraGridBase)cmbrangeuom).ActiveRow == null || Conversion.ToInt(cmbrangeuom.Value) == 0)
		{
			MessageBox.Show("Range UOM Field is Required");
			cmbrangeuom.Focus();
			return false;
		}
		if (txtMeatureMFrom.Text.Trim() == string.Empty || txtMeatureMFrom.Text.Trim() == "0")
		{
			MessageBox.Show("Range From Field Required");
			txtMeatureMFrom.Focus();
			return false;
		}
		if (txtMeatureMTo.Text.Trim() == string.Empty || txtMeatureMTo.Text.Trim() == "0")
		{
			MessageBox.Show("Range To Field Required");
			txtMeatureMTo.Focus();
			return false;
		}
		if (txtRate.Text.Trim() == string.Empty || txtRate.Text.Trim() == "0")
		{
			MessageBox.Show("Rate Field Required");
			txtRate.Focus();
			return false;
		}
		if (txtqty.Text.Trim() == string.Empty || txtqty.Text.Trim() == "0")
		{
			MessageBox.Show("Qty Field Required");
			txtqty.Focus();
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
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			ContractorWagesRateSchedule wagesSchedule = new ContractorWagesRateSchedule();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				wagesSchedule.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			wagesSchedule.OrganizationId = UserAccount.OrganizationId;
			wagesSchedule.CompanyId = UserAccount.CompanyId;
			wagesSchedule.BranchesId = UserAccount.BranchesId;
			wagesSchedule.ProjectsId = UserAccount.BranchesId;
			wagesSchedule.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			wagesSchedule.EntryDate = DateTime.Now;
			wagesSchedule.ModifyDate = DateTime.Now;
			wagesSchedule.EntryUserId = UserAccount.ID;
			wagesSchedule.ModifyUserId = UserAccount.ID;
			wagesSchedule.ContractorWagesActivityId = Conversion.ToInt(combAccountName.Value);
			wagesSchedule.EffectiveDate = Conversion.ToDateTime((object)txtdate.Value);
			wagesSchedule.Rate = Conversion.ToDecimal((object)txtRate.Text);
			wagesSchedule.Qty = Conversion.ToDecimal((object)txtqty.Text);
			wagesSchedule.MeatureMFrom = Conversion.ToInt((object)txtMeatureMFrom.Text);
			wagesSchedule.MeatureSFrom = Conversion.ToInt((object)txtMeatureSFrom.Text);
			wagesSchedule.MatureMTo = Conversion.ToInt((object)txtMeatureMTo.Text);
			wagesSchedule.MatureSTo = Conversion.ToInt((object)txtMeatureSTo.Text);
			wagesSchedule.RangeUomId = Conversion.ToInt(cmbrangeuom.Value);
			if (radItem.Checked)
			{
				wagesSchedule.ItemId = Conversion.ToInt(CmbWagesGroup.Value);
				wagesSchedule.ContractorWagesGroupId = 0;
			}
			else
			{
				wagesSchedule.ContractorWagesGroupId = Conversion.ToInt(CmbWagesGroup.Value);
				wagesSchedule.ItemId = 0;
			}
			wagesSchedule.IsActive = true;
			wagesSchedule.ActionId = 1;
			int success = ContractorWagesRateSchedule.Save(wagesSchedule);
			wagesSchedule.Id = success;
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully.");
			}
			else
			{
				MessageBox.Show("Data Save Successfully.");
			}
			FormRest();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ReadbyId(int Id)
	{
		try
		{
			RecId = Id;
			ContractorWagesRateSchedule obj = ContractorWagesRateSchedule.ReadById(RecId);
			if (obj != null)
			{
				tabWagesSchedule.SelectedIndex = 0;
				txtdate.Value = Conversion.ToDateTime((object)obj.EffectiveDate);
				combAccountName.Value = Conversion.ToInt((object)obj.ContractorWagesActivityId);
				if (obj.ItemId > 0)
				{
					radItem.Checked = true;
					CmbWagesGroup.Value = Conversion.ToInt((object)obj.ItemId);
				}
				else
				{
					radWagesGroup.Checked = true;
					CmbWagesGroup.Value = Conversion.ToInt((object)obj.ContractorWagesGroupId);
				}
				cmbrangeuom.Value = Conversion.ToInt((object)obj.RangeUomId);
				txtMeatureMFrom.Text = Conversion.ToDouble((object)obj.MeatureMFrom).ToString();
				txtMeatureSFrom.Text = Conversion.ToDouble((object)obj.MeatureSFrom).ToString();
				txtMeatureMTo.Text = Conversion.ToDouble((object)obj.MatureMTo).ToString();
				txtMeatureSTo.Text = Conversion.ToDouble((object)obj.MatureSTo).ToString();
				isActive.Checked = Convert.ToBoolean(obj.IsActive);
				txtRate.Text = obj.Rate.ToString();
				txtqty.Text = obj.Qty.ToString();
				UpdateMode = true;
				btnsave.Visible = false;
				btnUpdate.Visible = true;
				btnDelete.Visible = true;
			}
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

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					ContractorWagesRateSchedule.DeleteByID(UserAccount.ID, RecId);
					MessageBox.Show("Delete Record Seccessfully");
					FormRest();
				}
			}
			else
			{
				MessageBox.Show("No Record Found For Deletion");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGrid()
	{
		try
		{
			if (dtgetAll.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("EffectiveDate", typeof(DateTime));
				dt.Columns.Add("ContractorWagesActivityId", typeof(int));
				dt.Columns.Add("WagesActivity", typeof(string));
				dt.Columns.Add("ContractorWagesGroupId", typeof(int));
				dt.Columns.Add("ItemWagesGroupName", typeof(string));
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("RangeUomId", typeof(int));
				dt.Columns.Add("RangeUom", typeof(string));
				dt.Columns.Add("RangeFrom", typeof(string));
				dt.Columns.Add("RangeTo", typeof(string));
				dt.Columns.Add("Rate", typeof(decimal));
				dt.Columns.Add("Qty", typeof(decimal));
				dt.Clear();
				for (int i = 0; i < dtgetAll.Rows.Count; i++)
				{
					if (Conversion.ToInt(combAccountName.Value) == Conversion.ToInt(dtgetAll.Rows[i]["ContractorWagesActivityId"]))
					{
						dt.Rows.Add(Conversion.ToInt(dtgetAll.Rows[i]["Id"]), Conversion.ToDateTime(dtgetAll.Rows[i]["EffectiveDate"]).ToShortDateString(), Conversion.ToInt(dtgetAll.Rows[i]["ContractorWagesActivityId"]), Conversion.ToString(dtgetAll.Rows[i]["WagesActivity"]), Conversion.ToInt(dtgetAll.Rows[i]["ContractorWagesGroupId"]), Conversion.ToString(dtgetAll.Rows[i]["ItemWagesGroupName"]), Conversion.ToInt(dtgetAll.Rows[i]["ItemId"]), Conversion.ToString(dtgetAll.Rows[i]["ItemName"]), Conversion.ToInt(dtgetAll.Rows[i]["RangeUomId"]), Conversion.ToString(dtgetAll.Rows[i]["RangeUom"]), Conversion.ToInt(dtgetAll.Rows[i]["MeatureMFrom"]) + "-" + Conversion.ToInt(dtgetAll.Rows[i]["MeatureSFrom"]), Conversion.ToInt(dtgetAll.Rows[i]["MatureMTo"]) + "-" + Conversion.ToInt(dtgetAll.Rows[i]["MatureSTo"]), Conversion.ToDecimal(dtgetAll.Rows[i]["Rate"]), Conversion.ToDecimal(dtgetAll.Rows[i]["Qty"]));
					}
				}
				grdWagesSchedule.DataSource = dt;
				grdWagesSchedule.RetrieveStructure();
				grdWagesSetting();
			}
			else
			{
				grdWagesSchedule.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdWagesSchedule_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			ReadbyId(Conversion.ToInt(grdWagesSchedule.CurrentRow.Cells["Id"].Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdWagesSetting()
	{
		try
		{
			grdWagesSchedule.RootTable.Columns["Id"].Visible = false;
			grdWagesSchedule.RootTable.Columns["RangeUomId"].Visible = false;
			grdWagesSchedule.RootTable.Columns["ItemId"].Visible = false;
			grdWagesSchedule.RootTable.Columns["ContractorWagesActivityId"].Visible = false;
			grdWagesSchedule.RootTable.Columns["ContractorWagesGroupId"].Visible = false;
			grdWagesSchedule.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)3;
			grdWagesSchedule.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdWagesSchedule.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdWagesSchedule.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdWagesSchedule.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)3;
			grdWagesSchedule.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdWagesSchedule.RootTable.Columns["Qty"].FormatString = "#,##0.##";
			grdWagesSchedule.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(grdWagesSchedule);
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabWagesSchedule_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabWagesSchedule.SelectedIndex == 1)
		{
			FromDateHistory.Focus();
		}
		else
		{
			txtdate.Focus();
		}
	}

	private void Resethistory()
	{
		try
		{
			FromDateHistory.Text = string.Empty;
			txtToDateHistory.Text = string.Empty;
			((Control)(object)CmbGroupNameHistory).Text = string.Empty;
			((Control)(object)CmbWagesAccountHistory).Text = string.Empty;
			FromDateHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			Resethistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			ComboFromWagesScheduleBind(ComboFromWagesScheduleDbCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable ComboFromWagesScheduleDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return ContractorWagesRateSchedule.GetDataForDropDownFromWagesSchedule(new ReportsParameters
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

	private void ComboFromWagesScheduleBind(DataTable dt)
	{
		try
		{
			DataTable dtGroup = new DataTable();
			dtGroup.Columns.Add("Id", typeof(int));
			dtGroup.Columns.Add("name");
			DataTable dtwagAc = dtGroup.Clone();
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "GroupName")
					{
						dtGroup.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Service Activity")
					{
						dtwagAc.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbGroupNameHistory, dtGroup, "Id", "name", "Group Name", AllColumns: true, Conversion.ToInt(CmbGroupNameHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbWagesAccountHistory, dtwagAc, "Id", "name", "Wages Account", AllColumns: true, Conversion.ToInt(CmbWagesAccountHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			BindGridHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGridHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (FromDateHistory.Checked)
			{
				obj.EffectedDate = Conversion.ToDateTime((object)FromDateHistory.Value);
			}
			if (txtToDateHistory.Checked)
			{
				obj.EffectedDateTo = Conversion.ToDateTime((object)txtToDateHistory.Value);
			}
			obj.InvConractorWagesAccountsId = Conversion.ToInt(CmbGroupNameHistory.Value);
			obj.ContractorWagesActivityId = Conversion.ToInt(CmbWagesAccountHistory.Value);
			obj.ActionId = 2;
			dtgetAll = ContractorWagesRateSchedule.FormHistory(obj);
			if (dtgetAll.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("EffectiveDate", typeof(DateTime));
				dt.Columns.Add("ContractorWagesActivityId", typeof(int));
				dt.Columns.Add("WagesActivity", typeof(string));
				dt.Columns.Add("ContractorWagesGroupId", typeof(int));
				dt.Columns.Add("ItemWagesGroupName", typeof(string));
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("RangeUomId", typeof(int));
				dt.Columns.Add("RangeUom", typeof(string));
				dt.Columns.Add("MeatureMFrom", typeof(int));
				dt.Columns.Add("MeatureSFrom", typeof(int));
				dt.Columns.Add("MatureMTo", typeof(int));
				dt.Columns.Add("MatureSTo", typeof(int));
				dt.Columns.Add("RangeFrom", typeof(string));
				dt.Columns.Add("RangeTo", typeof(string));
				dt.Columns.Add("Rate", typeof(decimal));
				dt.Columns.Add("NewRate", typeof(decimal));
				dt.Columns.Add("Qty", typeof(decimal));
				dt.Clear();
				for (int i = 0; i < dtgetAll.Rows.Count; i++)
				{
					dt.Rows.Add(Conversion.ToInt(dtgetAll.Rows[i]["Id"]), Conversion.ToDateTime(dtgetAll.Rows[i]["EffectiveDate"]).ToShortDateString(), Conversion.ToInt(dtgetAll.Rows[i]["ContractorWagesActivityId"]), Conversion.ToString(dtgetAll.Rows[i]["WagesActivity"]), Conversion.ToInt(dtgetAll.Rows[i]["ContractorWagesGroupId"]), Conversion.ToString(dtgetAll.Rows[i]["ItemWagesGroupName"]), Conversion.ToInt(dtgetAll.Rows[i]["ItemId"]), Conversion.ToString(dtgetAll.Rows[i]["ItemName"]), Conversion.ToInt(dtgetAll.Rows[i]["RangeUomId"]), Conversion.ToString(dtgetAll.Rows[i]["RangeUom"]), Conversion.ToInt(dtgetAll.Rows[i]["MeatureMFrom"]), Conversion.ToInt(dtgetAll.Rows[i]["MeatureSFrom"]), Conversion.ToInt(dtgetAll.Rows[i]["MatureMTo"]), Conversion.ToInt(dtgetAll.Rows[i]["MatureSTo"]), Conversion.ToInt(dtgetAll.Rows[i]["MeatureMFrom"]) + "-" + Conversion.ToInt(dtgetAll.Rows[i]["MeatureSFrom"]), Conversion.ToInt(dtgetAll.Rows[i]["MatureMTo"]) + "-" + Conversion.ToInt(dtgetAll.Rows[i]["MatureSTo"]), Conversion.ToDecimal(dtgetAll.Rows[i]["Rate"]), 0, Conversion.ToDecimal(dtgetAll.Rows[i]["Qty"]));
				}
				gridhistory.DataSource = dt;
				gridhistory.RetrieveStructure();
				grdHistorySetting();
			}
			else
			{
				gridhistory.ClearStructure();
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
			gridhistory.RootTable.Columns["Id"].Visible = false;
			gridhistory.RootTable.Columns["ItemId"].Visible = false;
			gridhistory.RootTable.Columns["RangeUomId"].Visible = false;
			gridhistory.RootTable.Columns["ContractorWagesActivityId"].Visible = false;
			gridhistory.RootTable.Columns["ContractorWagesGroupId"].Visible = false;
			gridhistory.RootTable.Columns["MeatureMFrom"].Visible = false;
			gridhistory.RootTable.Columns["MeatureSFrom"].Visible = false;
			gridhistory.RootTable.Columns["MatureMTo"].Visible = false;
			gridhistory.RootTable.Columns["MatureSTo"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(gridhistory, 3, 2);
			gridhistory.RootTable.Columns["NewRate"].EditType = (EditType)1;
			gridhistory.RootTable.Columns["EffectiveDate"].EditType = (EditType)2;
			gridhistory.RootTable.Columns["Qty"].EditType = (EditType)1;
			ctrlGrdBar2_Load(null, null);
			GridEX_Helper.AddSelectButton(gridhistory, "Select", 30, 0);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridhistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			ReadbyId(Conversion.ToInt((object)gridhistory.CurrentRow.Cells["Id"].Value.ToString()));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnSaveHistory_Click(object sender, EventArgs e)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Expected O, but got Unknown
		try
		{
			GridEXRow[] checkedRows = gridhistory.GetCheckedRows();
			if (checkedRows.Count() == 0)
			{
				MessageBox.Show("Please check at least one row first.");
			}
			else
			{
				if (MessageBox.Show("Are you sure to Save As Selected Rows? This operation will insert new rows with the provided new rate.", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				ContractorWagesRateSchedule obj = new ContractorWagesRateSchedule();
				obj.ContractorWagesRateScheduleList = new List<ContractorWagesRateSchedule>();
				DateTime now = DateTime.Now;
				GridEXRow[] array = checkedRows;
				foreach (GridEXRow row in array)
				{
					decimal rate = Conversion.ToDecimal((object)row.Cells["NewRate"].Text);
					if (rate == 0m)
					{
						MessageBox.Show("Rate not found in a row. Please provide a valid rate.");
						return;
					}
					ContractorWagesRateSchedule wagesSchedule = new ContractorWagesRateSchedule
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						BranchesId = UserAccount.BranchesId,
						ProjectsId = UserAccount.BranchesId,
						FinancialYearId = clsGlobalVariables.ActiveYr.Id,
						EntryDate = now,
						ModifyDate = now,
						EntryUserId = UserAccount.ID,
						ModifyUserId = UserAccount.ID,
						ContractorWagesActivityId = Conversion.ToInt(row.Cells["ContractorWagesActivityId"].Value),
						EffectiveDate = Conversion.ToDateTime(row.Cells["EffectiveDate"].Value),
						Rate = rate,
						Qty = Conversion.ToDecimal((object)row.Cells["Qty"].Text),
						MeatureMFrom = Conversion.ToInt((object)row.Cells["MeatureMFrom"].Text),
						MeatureSFrom = Conversion.ToInt((object)row.Cells["MeatureSFrom"].Text),
						MatureMTo = Conversion.ToInt((object)row.Cells["MatureMTo"].Text),
						MatureSTo = Conversion.ToInt((object)row.Cells["MatureSTo"].Text),
						RangeUomId = Conversion.ToInt(row.Cells["RangeUomId"].Value),
						ItemId = Conversion.ToInt(row.Cells["ItemId"].Value),
						ContractorWagesGroupId = Conversion.ToInt(row.Cells["ContractorWagesGroupId"].Value),
						IsActive = true,
						ActionId = 1
					};
					obj.ContractorWagesRateScheduleList.Add(wagesSchedule);
				}
				ContractorWagesRateSchedule.SaveList(obj);
				MessageBox.Show("Record updated successfully.");
				btnshowHistory_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error: " + ex.Message);
		}
	}

	private void frmContractWagesSchedule_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled && tabWagesSchedule.SelectedIndex == 0)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabWagesSchedule.SelectedIndex == 0)
				{
					btnnew_Click(null, null);
				}
				else
				{
					btnnew_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabWagesSchedule.SelectedIndex == 1)
				{
					tabWagesSchedule.SelectedIndex = 0;
				}
				else
				{
					tabWagesSchedule.SelectedIndex = 1;
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled && UpdateMode)
			{
				btnUpdate_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnrefrsh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				txtdate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				txtdate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdWagesSchedule).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (tabWagesSchedule.SelectedIndex == 0)
				{
					grdWagesSchedule_DoubleClick(null, null);
				}
				else
				{
					gridhistory_DoubleClick(null, null);
				}
			}
			if (e.Control && e.Alt)
			{
				btnshortcutkeys_Click(null, null);
			}
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
			dt.Rows.Add("Ctrl+F5", "For Focus on Effective Date");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Item Combo in Effective Date");
			dt.Rows.Add("Ctrl+Enter", "For Update Record When Focus On Any Grid ");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshortcutkeys_Click(object sender, EventArgs e)
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdWagesSchedule).Name);
			if (fs != null)
			{
				grdWagesSchedule.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)gridhistory).Name);
			if (fs != null)
			{
				gridhistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtPackUOMFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtPackUOMTO_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtqty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtMeatureSFrom_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			if ((!char.IsDigit(e.KeyChar) || txtMeatureSFrom.Text.Length >= 3) && !char.IsControl(e.KeyChar))
			{
				e.Handled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtMeatureSTo_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			if ((!char.IsDigit(e.KeyChar) || txtMeatureSTo.Text.Length >= 3) && !char.IsControl(e.KeyChar))
			{
				e.Handled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtMeatureSFrom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbrangeuom).ActiveRow != null && Conversion.ToInt(cmbrangeuom.Value) != 0)
			{
				switch (Conversion.ToInt(((UltraDropDownBase)cmbrangeuom).SelectedRow.Cells["ParentUOMId"].Value))
				{
				case 1:
					if (Conversion.ToInt((object)txtMeatureSFrom.Text) > 11)
					{
						txtMeatureSFrom.Text = 11.ToString();
					}
					break;
				case 2:
					if (Conversion.ToInt((object)txtMeatureSFrom.Text) > 0)
					{
						txtMeatureSFrom.Text = 0.ToString();
					}
					break;
				case 3:
					if (Conversion.ToInt((object)txtMeatureSFrom.Text) > 999)
					{
						txtMeatureSFrom.Text = 999.ToString();
					}
					break;
				default:
					txtMeatureSFrom.Text = 0.ToString();
					break;
				}
			}
			else
			{
				txtMeatureSFrom.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtMeatureSTo_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbrangeuom).ActiveRow != null && Conversion.ToInt(cmbrangeuom.Value) != 0)
			{
				switch (Conversion.ToInt(((UltraDropDownBase)cmbrangeuom).SelectedRow.Cells["ParentUOMId"].Value))
				{
				case 1:
					if (Conversion.ToInt((object)txtMeatureSTo.Text) > 11)
					{
						txtMeatureSTo.Text = 11.ToString();
					}
					break;
				case 2:
					if (Conversion.ToInt((object)txtMeatureSTo.Text) > 0)
					{
						txtMeatureSTo.Text = 0.ToString();
					}
					break;
				case 3:
					if (Conversion.ToInt((object)txtMeatureSTo.Text) > 999)
					{
						txtMeatureSTo.Text = 999.ToString();
					}
					break;
				default:
					txtMeatureSTo.Text = 0.ToString();
					break;
				}
			}
			else
			{
				txtMeatureSTo.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnWagesActivity_Click(object sender, EventArgs e)
	{
		try
		{
			new ContractorWagesActivity(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnWagesGroupDefine_Click(object sender, EventArgs e)
	{
		try
		{
			new ContractorWagesGroup(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnDefineRateUom_Click(object sender, EventArgs e)
	{
		try
		{
			new InvDeffrmItemItemUom(UserAccount).Show();
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
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Expected O, but got Unknown
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Expected O, but got Unknown
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Expected O, but got Unknown
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Expected O, but got Unknown
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Expected O, but got Unknown
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Expected O, but got Unknown
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Expected O, but got Unknown
		GridEXLayout grdWagesSchedule_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Pcc.ContractorWagesRateSchedule));
		GridEXLayout gridhistory_DesignTimeLayout = new GridEXLayout();
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
		this.tabWagesSchedule = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdWagesSchedule = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.radItem = new System.Windows.Forms.RadioButton();
		this.radWagesGroup = new System.Windows.Forms.RadioButton();
		this.BtnDefineRateUom = new System.Windows.Forms.Button();
		this.BtnWagesActivity = new System.Windows.Forms.Button();
		this.btnWagesGroupDefine = new System.Windows.Forms.Button();
		this.combAccountName = new UltraCombo();
		this.isActive = new System.Windows.Forms.CheckBox();
		this.label3 = new System.Windows.Forms.Label();
		this.cmbrangeuom = new UltraCombo();
		this.txtdate = new System.Windows.Forms.DateTimePicker();
		this.label1 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.txtMeatureSTo = new System.Windows.Forms.TextBox();
		this.txtMeatureMTo = new System.Windows.Forms.TextBox();
		this.CmbWagesGroup = new UltraCombo();
		this.txtRate = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.txtMeatureSFrom = new System.Windows.Forms.TextBox();
		this.txtMeatureMFrom = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.btnAttachments = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnrefrsh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.gridhistory = new GridEX();
		this.panel1 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label4 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.btnshowHistory = new System.Windows.Forms.Button();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label20 = new System.Windows.Forms.Label();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label21 = new System.Windows.Forms.Label();
		this.CmbWagesAccountHistory = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.CmbGroupNameHistory = new UltraCombo();
		this.label23 = new System.Windows.Forms.Label();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label24 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.BtnSaveHistory = new System.Windows.Forms.Button();
		this.tabWagesSchedule.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdWagesSchedule).BeginInit();
		this.panel5.SuspendLayout();
		this.panel2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.combAccountName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbrangeuom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWagesGroup).BeginInit();
		this.panel6.SuspendLayout();
		this.btnAttachments.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.gridhistory).BeginInit();
		this.panel1.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWagesAccountHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGroupNameHistory).BeginInit();
		this.panel12.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.tabWagesSchedule.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabWagesSchedule.Controls.Add(this.tabPage1);
		this.tabWagesSchedule.Controls.Add(this.tabPage2);
		this.tabWagesSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabWagesSchedule.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabWagesSchedule.Location = new System.Drawing.Point(0, 0);
		this.tabWagesSchedule.Name = "tabWagesSchedule";
		this.tabWagesSchedule.SelectedIndex = 0;
		this.tabWagesSchedule.Size = new System.Drawing.Size(964, 661);
		this.tabWagesSchedule.TabIndex = 0;
		this.tabWagesSchedule.SelectedIndexChanged += new System.EventHandler(tabWagesSchedule_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.btnAttachments);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(956, 632);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdWagesSchedule);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(3, 225);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(950, 404);
		this.panel4.TabIndex = 71;
		this.grdWagesSchedule.AllowEdit = (InheritableBoolean)2;
		grdWagesSchedule_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdWagesSchedule.DesignTimeLayout = grdWagesSchedule_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdWagesSchedule).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdWagesSchedule).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdWagesSchedule.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdWagesSchedule).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdWagesSchedule).Name = "grdWagesSchedule";
		((ScrollableBase)this.grdWagesSchedule).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdWagesSchedule).Size = new System.Drawing.Size(950, 404);
		((System.Windows.Forms.Control)(object)this.grdWagesSchedule).TabIndex = 0;
		this.grdWagesSchedule.TotalRow = (InheritableBoolean)1;
		this.grdWagesSchedule.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdWagesSchedule).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdWagesSchedule).DoubleClick += new System.EventHandler(grdWagesSchedule_DoubleClick);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label5);
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 193);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(950, 32);
		this.panel5.TabIndex = 70;
		this.label5.AutoSize = true;
		this.label5.BackColor = System.Drawing.Color.Teal;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label5.Location = new System.Drawing.Point(8, 4);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(287, 19);
		this.label5.TabIndex = 0;
		this.label5.Text = "History of Selected Sevice Activity";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(898, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdWagesSchedule;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(43, 28);
		this.ctrlGrdBar1.TabIndex = 69;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.groupBox1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 60);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(950, 133);
		this.panel2.TabIndex = 0;
		this.groupBox1.Controls.Add(this.radItem);
		this.groupBox1.Controls.Add(this.radWagesGroup);
		this.groupBox1.Controls.Add(this.BtnDefineRateUom);
		this.groupBox1.Controls.Add(this.BtnWagesActivity);
		this.groupBox1.Controls.Add(this.btnWagesGroupDefine);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combAccountName);
		this.groupBox1.Controls.Add(this.isActive);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbrangeuom);
		this.groupBox1.Controls.Add(this.txtdate);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.txtqty);
		this.groupBox1.Controls.Add(this.txtMeatureSTo);
		this.groupBox1.Controls.Add(this.txtMeatureMTo);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWagesGroup);
		this.groupBox1.Controls.Add(this.txtRate);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.txtMeatureSFrom);
		this.groupBox1.Controls.Add(this.txtMeatureMFrom);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(6, 4);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(788, 126);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.radItem.AutoSize = true;
		this.radItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.radItem.Location = new System.Drawing.Point(228, 24);
		this.radItem.Name = "radItem";
		this.radItem.Size = new System.Drawing.Size(85, 19);
		this.radItem.TabIndex = 37;
		this.radItem.TabStop = true;
		this.radItem.Text = "Item Name";
		this.radItem.UseVisualStyleBackColor = true;
		this.radWagesGroup.AutoSize = true;
		this.radWagesGroup.Checked = true;
		this.radWagesGroup.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.radWagesGroup.Location = new System.Drawing.Point(129, 24);
		this.radWagesGroup.Name = "radWagesGroup";
		this.radWagesGroup.Size = new System.Drawing.Size(97, 19);
		this.radWagesGroup.TabIndex = 37;
		this.radWagesGroup.TabStop = true;
		this.radWagesGroup.Text = "Wages Group";
		this.radWagesGroup.UseVisualStyleBackColor = true;
		this.radWagesGroup.CheckedChanged += new System.EventHandler(radWagesGroup_CheckedChanged);
		this.BtnDefineRateUom.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnDefineRateUom.Location = new System.Drawing.Point(756, 45);
		this.BtnDefineRateUom.Name = "BtnDefineRateUom";
		this.BtnDefineRateUom.Size = new System.Drawing.Size(30, 26);
		this.BtnDefineRateUom.TabIndex = 6;
		this.BtnDefineRateUom.TabStop = false;
		this.BtnDefineRateUom.UseVisualStyleBackColor = true;
		this.BtnDefineRateUom.Click += new System.EventHandler(BtnDefineRateUom_Click);
		this.BtnWagesActivity.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnWagesActivity.Location = new System.Drawing.Point(600, 45);
		this.BtnWagesActivity.Name = "BtnWagesActivity";
		this.BtnWagesActivity.Size = new System.Drawing.Size(30, 26);
		this.BtnWagesActivity.TabIndex = 4;
		this.BtnWagesActivity.TabStop = false;
		this.BtnWagesActivity.UseVisualStyleBackColor = true;
		this.BtnWagesActivity.Click += new System.EventHandler(BtnWagesActivity_Click);
		this.btnWagesGroupDefine.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnWagesGroupDefine.Location = new System.Drawing.Point(348, 45);
		this.btnWagesGroupDefine.Name = "btnWagesGroupDefine";
		this.btnWagesGroupDefine.Size = new System.Drawing.Size(30, 26);
		this.btnWagesGroupDefine.TabIndex = 2;
		this.btnWagesGroupDefine.TabStop = false;
		this.btnWagesGroupDefine.UseVisualStyleBackColor = true;
		this.btnWagesGroupDefine.Click += new System.EventHandler(btnWagesGroupDefine_Click);
		this.combAccountName.AllowNull = (DefaultableBoolean)1;
		this.combAccountName.AutoCompleteMode = (AutoCompleteMode)4;
		this.combAccountName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combAccountName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combAccountName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.combAccountName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combAccountName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combAccountName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combAccountName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.combAccountName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combAccountName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.combAccountName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combAccountName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combAccountName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combAccountName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.combAccountName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combAccountName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combAccountName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combAccountName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.combAccountName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combAccountName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combAccountName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.combAccountName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.combAccountName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combAccountName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combAccountName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.combAccountName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combAccountName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.combAccountName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combAccountName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combAccountName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combAccountName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combAccountName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combAccountName).Location = new System.Drawing.Point(379, 45);
		((System.Windows.Forms.Control)(object)this.combAccountName).Name = "combAccountName";
		((System.Windows.Forms.Control)(object)this.combAccountName).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.combAccountName).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.combAccountName).Leave += new System.EventHandler(combAccountName_Leave);
		this.isActive.AutoSize = true;
		this.isActive.Checked = true;
		this.isActive.CheckState = System.Windows.Forms.CheckState.Checked;
		this.isActive.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.isActive.Location = new System.Drawing.Point(570, 98);
		this.isActive.Name = "isActive";
		this.isActive.Size = new System.Drawing.Size(71, 19);
		this.isActive.TabIndex = 13;
		this.isActive.Text = "Is Active";
		this.isActive.UseVisualStyleBackColor = true;
		this.isActive.Leave += new System.EventHandler(isActive_Leave);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(5, 26);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(78, 15);
		this.label3.TabIndex = 23;
		this.label3.Text = "Effected Date";
		this.cmbrangeuom.AllowNull = (DefaultableBoolean)1;
		this.cmbrangeuom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbrangeuom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbrangeuom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbrangeuom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbrangeuom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbrangeuom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbrangeuom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbrangeuom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.cmbrangeuom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbrangeuom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbrangeuom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbrangeuom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbrangeuom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbrangeuom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbrangeuom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbrangeuom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbrangeuom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbrangeuom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbrangeuom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbrangeuom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbrangeuom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbrangeuom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbrangeuom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbrangeuom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbrangeuom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbrangeuom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbrangeuom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbrangeuom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbrangeuom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbrangeuom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbrangeuom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbrangeuom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbrangeuom).Location = new System.Drawing.Point(633, 45);
		((System.Windows.Forms.Control)(object)this.cmbrangeuom).Name = "cmbrangeuom";
		((System.Windows.Forms.Control)(object)this.cmbrangeuom).Size = new System.Drawing.Size(121, 26);
		((System.Windows.Forms.Control)(object)this.cmbrangeuom).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.cmbrangeuom).Leave += new System.EventHandler(cmbrangeuom_Leave);
		this.txtdate.CustomFormat = "dd-MMM-yy";
		this.txtdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdate.Location = new System.Drawing.Point(5, 47);
		this.txtdate.Name = "txtdate";
		this.txtdate.Size = new System.Drawing.Size(121, 23);
		this.txtdate.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(633, 26);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(71, 15);
		this.label1.TabIndex = 36;
		this.label1.Text = "Range UOM";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(327, 75);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(30, 15);
		this.label7.TabIndex = 30;
		this.label7.Text = "Rate";
		this.txtqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtqty.Location = new System.Drawing.Point(451, 96);
		this.txtqty.Name = "txtqty";
		this.txtqty.ShortcutsEnabled = false;
		this.txtqty.Size = new System.Drawing.Size(111, 23);
		this.txtqty.TabIndex = 12;
		this.txtqty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.txtMeatureSTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMeatureSTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMeatureSTo.Location = new System.Drawing.Point(247, 96);
		this.txtMeatureSTo.Name = "txtMeatureSTo";
		this.txtMeatureSTo.ShortcutsEnabled = false;
		this.txtMeatureSTo.Size = new System.Drawing.Size(78, 23);
		this.txtMeatureSTo.TabIndex = 10;
		this.txtMeatureSTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtMeatureSTo.TextChanged += new System.EventHandler(txtMeatureSTo_TextChanged);
		this.txtMeatureSTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtMeatureSTo_KeyPress);
		this.txtMeatureMTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMeatureMTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMeatureMTo.Location = new System.Drawing.Point(168, 96);
		this.txtMeatureMTo.Name = "txtMeatureMTo";
		this.txtMeatureMTo.ShortcutsEnabled = false;
		this.txtMeatureMTo.Size = new System.Drawing.Size(78, 23);
		this.txtMeatureMTo.TabIndex = 9;
		this.txtMeatureMTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtMeatureMTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPackUOMTO_KeyPress);
		this.CmbWagesGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbWagesGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWagesGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWagesGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWagesGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbWagesGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWagesGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWagesGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWagesGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.CmbWagesGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWagesGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbWagesGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWagesGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWagesGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWagesGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbWagesGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWagesGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWagesGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWagesGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbWagesGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWagesGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWagesGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWagesGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbWagesGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWagesGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWagesGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbWagesGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWagesGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbWagesGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWagesGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWagesGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWagesGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWagesGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWagesGroup).Location = new System.Drawing.Point(129, 45);
		((System.Windows.Forms.Control)(object)this.CmbWagesGroup).Name = "CmbWagesGroup";
		((System.Windows.Forms.Control)(object)this.CmbWagesGroup).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbWagesGroup).TabIndex = 1;
		this.txtRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRate.Location = new System.Drawing.Point(327, 96);
		this.txtRate.Name = "txtRate";
		this.txtRate.ShortcutsEnabled = false;
		this.txtRate.Size = new System.Drawing.Size(121, 23);
		this.txtRate.TabIndex = 11;
		this.txtRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRate_KeyPress);
		this.label10.BackColor = System.Drawing.Color.WhiteSmoke;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(168, 75);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(157, 18);
		this.label10.TabIndex = 20;
		this.label10.Text = "Range To";
		this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtMeatureSFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMeatureSFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMeatureSFrom.Location = new System.Drawing.Point(84, 95);
		this.txtMeatureSFrom.Name = "txtMeatureSFrom";
		this.txtMeatureSFrom.ShortcutsEnabled = false;
		this.txtMeatureSFrom.Size = new System.Drawing.Size(78, 23);
		this.txtMeatureSFrom.TabIndex = 8;
		this.txtMeatureSFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtMeatureSFrom.TextChanged += new System.EventHandler(txtMeatureSFrom_TextChanged);
		this.txtMeatureSFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtMeatureSFrom_KeyPress);
		this.txtMeatureMFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMeatureMFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMeatureMFrom.Location = new System.Drawing.Point(5, 95);
		this.txtMeatureMFrom.Name = "txtMeatureMFrom";
		this.txtMeatureMFrom.ShortcutsEnabled = false;
		this.txtMeatureMFrom.Size = new System.Drawing.Size(78, 23);
		this.txtMeatureMFrom.TabIndex = 7;
		this.txtMeatureMFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtMeatureMFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPackUOMFrom_KeyPress);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(451, 75);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(26, 15);
		this.label8.TabIndex = 32;
		this.label8.Text = "Qty";
		this.label12.BackColor = System.Drawing.Color.WhiteSmoke;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(5, 74);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(157, 18);
		this.label12.TabIndex = 9;
		this.label12.Text = "Range From";
		this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(379, 26);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(88, 15);
		this.label18.TabIndex = 11;
		this.label18.Text = "Service Activity";
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label15);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 30);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(950, 30);
		this.panel6.TabIndex = 66;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(7, 5);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(260, 19);
		this.label15.TabIndex = 0;
		this.label15.Text = "Wages Contract Rate Schedule";
		this.btnAttachments.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.btnAttachments.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnnew, this.btnrefrsh, this.toolStripSeparator1, this.btnsave, this.btnUpdate, this.btnDelete, this.toolStripSeparator2, this.btnshortcutkeys, this.toolStripSeparator3 });
		this.btnAttachments.Location = new System.Drawing.Point(3, 3);
		this.btnAttachments.Name = "btnAttachments";
		this.btnAttachments.Size = new System.Drawing.Size(950, 27);
		this.btnAttachments.TabIndex = 65;
		this.btnAttachments.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnrefrsh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnrefrsh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnrefrsh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnrefrsh.Name = "btnrefrsh";
		this.btnrefrsh.Size = new System.Drawing.Size(80, 24);
		this.btnrefrsh.Text = "&Refresh";
		this.btnrefrsh.Click += new System.EventHandler(btnrefrsh_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
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
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnshortcutkeys.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = (System.Drawing.Image)resources.GetObject("btnshortcutkeys.Image");
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(106, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.gridhistory);
		this.tabPage2.Controls.Add(this.panel1);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(956, 632);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		gridhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.gridhistory.DesignTimeLayout = gridhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.gridhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.gridhistory.FilterMode = (FilterMode)2;
		this.gridhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.gridhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.gridhistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.gridhistory).Location = new System.Drawing.Point(3, 145);
		((System.Windows.Forms.Control)(object)this.gridhistory).Name = "gridhistory";
		((ScrollableBase)this.gridhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.gridhistory).Size = new System.Drawing.Size(950, 484);
		((System.Windows.Forms.Control)(object)this.gridhistory).TabIndex = 69;
		this.gridhistory.TotalRow = (InheritableBoolean)1;
		this.gridhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.gridhistory).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.gridhistory).DoubleClick += new System.EventHandler(gridhistory_DoubleClick);
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.ctrlGrdBar2);
		this.panel1.Controls.Add(this.label4);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 121);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(950, 24);
		this.panel1.TabIndex = 67;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(907, 1);
		this.ctrlGrdBar2.MyGrid = this.gridhistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(39, 21);
		this.ctrlGrdBar2.TabIndex = 1;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.Teal;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label4.Location = new System.Drawing.Point(3, 2);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(117, 19);
		this.label4.TabIndex = 0;
		this.label4.Text = "Filter records";
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.btnshowHistory);
		this.panel7.Controls.Add(this.txtToDateHistory);
		this.panel7.Controls.Add(this.label20);
		this.panel7.Controls.Add(this.FromDateHistory);
		this.panel7.Controls.Add(this.label21);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWagesAccountHistory);
		this.panel7.Controls.Add(this.label22);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGroupNameHistory);
		this.panel7.Controls.Add(this.label23);
		this.panel7.Controls.Add(this.panel12);
		this.panel7.Controls.Add(this.toolStrip2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 3);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(950, 118);
		this.panel7.TabIndex = 70;
		this.btnshowHistory.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshowHistory.FlatAppearance.BorderSize = 0;
		this.btnshowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshowHistory.ForeColor = System.Drawing.Color.White;
		this.btnshowHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshowHistory.Location = new System.Drawing.Point(696, 87);
		this.btnshowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnshowHistory.Name = "btnshowHistory";
		this.btnshowHistory.Size = new System.Drawing.Size(68, 26);
		this.btnshowHistory.TabIndex = 4;
		this.btnshowHistory.Text = "Show";
		this.btnshowHistory.UseVisualStyleBackColor = false;
		this.btnshowHistory.Click += new System.EventHandler(btnshowHistory_Click);
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(73, 89);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.ShowCheckBox = true;
		this.txtToDateHistory.Size = new System.Drawing.Size(183, 23);
		this.txtToDateHistory.TabIndex = 1;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(7, 93);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(48, 15);
		this.label20.TabIndex = 159;
		this.label20.Text = "Date To";
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(73, 60);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(183, 23);
		this.FromDateHistory.TabIndex = 0;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(7, 64);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(63, 15);
		this.label21.TabIndex = 158;
		this.label21.Text = "Date From";
		this.CmbWagesAccountHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbWagesAccountHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWagesAccountHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWagesAccountHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWagesAccountHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWagesAccountHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWagesAccountHistory).Location = new System.Drawing.Point(355, 87);
		((System.Windows.Forms.Control)(object)this.CmbWagesAccountHistory).Name = "CmbWagesAccountHistory";
		((System.Windows.Forms.Control)(object)this.CmbWagesAccountHistory).Size = new System.Drawing.Size(336, 26);
		((System.Windows.Forms.Control)(object)this.CmbWagesAccountHistory).TabIndex = 3;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(258, 93);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(88, 15);
		this.label22.TabIndex = 157;
		this.label22.Text = "Service Activity";
		this.CmbGroupNameHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbGroupNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGroupNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGroupNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGroupNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbGroupNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGroupNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGroupNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGroupNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbGroupNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGroupNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbGroupNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGroupNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGroupNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGroupNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbGroupNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGroupNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGroupNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGroupNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbGroupNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGroupNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGroupNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGroupNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbGroupNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGroupNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGroupNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbGroupNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGroupNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbGroupNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGroupNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGroupNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGroupNameHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbGroupNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbGroupNameHistory).Location = new System.Drawing.Point(355, 58);
		((System.Windows.Forms.Control)(object)this.CmbGroupNameHistory).Name = "CmbGroupNameHistory";
		((System.Windows.Forms.Control)(object)this.CmbGroupNameHistory).Size = new System.Drawing.Size(336, 26);
		((System.Windows.Forms.Control)(object)this.CmbGroupNameHistory).TabIndex = 2;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(258, 64);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(75, 15);
		this.label23.TabIndex = 156;
		this.label23.Text = "Group Name";
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.BtnSaveHistory);
		this.panel12.Controls.Add(this.label24);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 27);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(950, 28);
		this.panel12.TabIndex = 155;
		this.label24.AutoSize = true;
		this.label24.BackColor = System.Drawing.Color.Teal;
		this.label24.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.Transparent;
		this.label24.Location = new System.Drawing.Point(2, 3);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(204, 19);
		this.label24.TabIndex = 154;
		this.label24.Text = "Wages Schedule History";
		this.toolStrip2.BackColor = System.Drawing.Color.White;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.BtnNewHistory, this.BtnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(950, 27);
		this.toolStrip2.TabIndex = 150;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewHistory.Name = "BtnNewHistory";
		this.BtnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.BtnNewHistory.Text = "New";
		this.BtnNewHistory.Click += new System.EventHandler(BtnNewHistory_Click);
		this.BtnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshHistory.Text = "&Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		this.BtnSaveHistory.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnSaveHistory.FlatAppearance.BorderSize = 0;
		this.BtnSaveHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnSaveHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnSaveHistory.ForeColor = System.Drawing.Color.White;
		this.BtnSaveHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnSaveHistory.Location = new System.Drawing.Point(757, 0);
		this.BtnSaveHistory.Margin = new System.Windows.Forms.Padding(2);
		this.BtnSaveHistory.Name = "BtnSaveHistory";
		this.BtnSaveHistory.Size = new System.Drawing.Size(189, 26);
		this.BtnSaveHistory.TabIndex = 4;
		this.BtnSaveHistory.Text = "Save As Selected Rows";
		this.BtnSaveHistory.UseVisualStyleBackColor = false;
		this.BtnSaveHistory.Click += new System.EventHandler(BtnSaveHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 661);
		base.Controls.Add(this.tabWagesSchedule);
		base.KeyPreview = true;
		base.Name = "ContractorWagesRateSchedule";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Wages_Account_Schedule";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmContractWagesSchedule_FormClosing);
		base.Load += new System.EventHandler(frmContractWagesSchedule_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmContractWagesSchedule_KeyDown);
		this.tabWagesSchedule.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage1.PerformLayout();
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdWagesSchedule).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.combAccountName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbrangeuom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWagesGroup).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.btnAttachments.ResumeLayout(false);
		this.btnAttachments.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.gridhistory).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWagesAccountHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGroupNameHistory).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
