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
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.HRM.PolicyManagment;

public class LeaveQuotaPolicy : Form
{
	public UserAccount UserAccount = new UserAccount();

	private int RecId;

	private bool UpdateMode;

	private DropDownBind DDL = new DropDownBind();

	private DataTable tablecasualleave = new DataTable();

	private DataTable tableSpecialLeave = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel3;

	private Panel panel4;

	private Panel panel7;

	private Panel panel11;

	private Label label2;

	private Label label1;

	private DateTimePicker txtdateto;

	private DateTimePicker txtdatefrom;

	private TextBox txtdescription;

	private Label label50;

	private Label label49;

	private Label label48;

	private ToolStrip tsItemType;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private Panel panel2;

	private Panel panel6;

	internal GridEX grdspecialleave;

	private Panel panel5;

	internal GridEX grdcasualleave;

	private Panel panel8;

	private CtrlGrdBar ctrlGrdBar3;

	private Panel panel9;

	internal GridEX grdHistory;

	private Panel panel12;

	private Panel panel14;

	internal GridEX grdSpecialHistory;

	private Panel panel13;

	internal GridEX grdCasualHistory;

	private Label label5;

	private Panel panel17;

	private Panel panel18;

	private Panel panel16;

	private Panel panel15;

	private Panel panel21;

	private Panel panel22;

	private Label label3;

	private Panel panel19;

	private Panel panel20;

	private Label label6;

	private ToolStripButton btnlookup;

	private ToolStripButton btnOfficialleave;

	public LeaveQuotaPolicy(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void LeaveQuotaPolicy_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(970, 450);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnupdate.Enabled = formright.DoHaveUpdateRights;
			ctrlGrdBar3.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar3.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar3.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar3.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar3.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar3.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			txtdatefrom.Focus();
			tablecasualleave.Columns.Add("Id", typeof(int));
			tablecasualleave.Columns.Add("LeaveType", typeof(string));
			tablecasualleave.Columns.Add("QuotaValue", typeof(double));
			tableSpecialLeave.Columns.Add("Id", typeof(int));
			tableSpecialLeave.Columns.Add("LeaveType", typeof(string));
			tableSpecialLeave.Columns.Add("QuotaValue", typeof(double));
			gridCasualLeaveFill();
			gridSpecialLeaveFill();
			txtdatefrom.Focus();
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
			txtdatefrom.Value = DateTime.Now;
			txtdateto.Value = DateTime.Now;
			txtdescription.Text = string.Empty;
			txtdatefrom.Focus();
			gridCasualLeaveFill();
			gridSpecialLeaveFill();
			btnsave.Visible = true;
			btnupdate.Visible = false;
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
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridCasualLeaveFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		try
		{
			DataTable getList = genProfile.GetByProfileTypeId(new genProfile
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ProfileTypeId = 3,
				IsActive = true
			});
			tablecasualleave.Rows.Clear();
			for (int i = 0; i < getList.Rows.Count; i++)
			{
				tablecasualleave.Rows.Add(getList.Rows[i]["ProfileId"], getList.Rows[i]["ProfileName"], "0");
			}
			grdcasualleave.DataSource = tablecasualleave;
			grdcasualleave.RetrieveStructure();
			grdCasualleavesetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCasualleavesetting()
	{
		try
		{
			grdcasualleave.RootTable.Columns["Id"].Visible = false;
			grdcasualleave.RootTable.Columns["LeaveType"].Width = 300;
			grdcasualleave.RootTable.Columns["QuotaValue"].Width = 100;
			grdcasualleave.RootTable.Columns["QuotaValue"].TotalFormatString = "0,##.##";
			grdcasualleave.RootTable.Columns["QuotaValue"].FormatString = "0,##.##";
			grdcasualleave.RootTable.Columns["QuotaValue"].TextAlignment = (TextAlignment)3;
			grdcasualleave.RootTable.Columns["QuotaValue"].AggregateFunction = (AggregateFunction)2;
			grdcasualleave.RootTable.Columns["Id"].EditType = (EditType)0;
			grdcasualleave.RootTable.Columns["LeaveType"].EditType = (EditType)0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdcasualleave_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if (e.Column.Key == "QuotaValue" && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridSpecialLeaveFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		try
		{
			DataTable getList = genProfile.GetByProfileTypeId(new genProfile
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				IsActive = true,
				ProfileTypeId = 4
			});
			tableSpecialLeave.Rows.Clear();
			for (int i = 0; i < getList.Rows.Count; i++)
			{
				tableSpecialLeave.Rows.Add(getList.Rows[i]["ProfileId"], getList.Rows[i]["ProfileName"], "0");
			}
			grdspecialleave.DataSource = tableSpecialLeave;
			grdspecialleave.RetrieveStructure();
			grdSpecialleavesetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSpecialleavesetting()
	{
		try
		{
			grdspecialleave.RootTable.Columns["Id"].Visible = false;
			grdspecialleave.RootTable.Columns["LeaveType"].Width = 300;
			grdspecialleave.RootTable.Columns["QuotaValue"].Width = 100;
			grdspecialleave.RootTable.Columns["QuotaValue"].TotalFormatString = "0,##.##";
			grdspecialleave.RootTable.Columns["QuotaValue"].FormatString = "0,##.##";
			grdspecialleave.RootTable.Columns["QuotaValue"].TextAlignment = (TextAlignment)3;
			grdspecialleave.RootTable.Columns["QuotaValue"].AggregateFunction = (AggregateFunction)2;
			grdspecialleave.RootTable.Columns["Id"].EditType = (EditType)0;
			grdspecialleave.RootTable.Columns["LeaveType"].EditType = (EditType)0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdspecialleave_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if (e.Column.Key == "QuotaValue" && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
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

	private void Insert()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Expected O, but got Unknown
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Expected O, but got Unknown
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
			hrmLeaveQuota leaveQuota = new hrmLeaveQuota();
			leaveQuota.LeaveQuotaId = RecId;
			leaveQuota.OrganizationId = UserAccount.OrganizationId;
			leaveQuota.CompanyId = UserAccount.CompanyId;
			leaveQuota.CreatedOn = Conversion.ToDateTime((object)DateTime.Today);
			leaveQuota.CreatedById = Conversion.ToInt((object)UserAccount.ID);
			leaveQuota.AlteredById = Conversion.ToInt((object)UserAccount.ID);
			leaveQuota.AlteredOn = Conversion.ToDateTime((object)DateTime.Today);
			leaveQuota.FromDate = Conversion.ToDateTime((object)txtdatefrom.Value);
			leaveQuota.ToDate = Conversion.ToDateTime((object)txtdateto.Value);
			leaveQuota.LocationId = 0;
			leaveQuota.IsActive = true;
			leaveQuota.Description = Conversion.ToString((object)txtdescription.Text.Trim());
			leaveQuota.HrmLeaveQuotaDetailList = new List<hrmLeaveQuotaDetail>();
			bool isConditionMet = false;
			for (int j = 0; j < grdcasualleave.RowCount; j++)
			{
				if (grdcasualleave.GetRow(j).Cells["QuotaValue"].Value != null && Conversion.ToInt(grdcasualleave.GetRow(j).Cells["QuotaValue"].Value) != 0)
				{
					isConditionMet = true;
					break;
				}
			}
			if (!isConditionMet)
			{
				for (int i = 0; i < grdspecialleave.RowCount; i++)
				{
					if (grdspecialleave.GetRow(i).Cells["QuotaValue"].Value != null && Conversion.ToInt((object)grdspecialleave.GetRow(i).Cells["QuotaValue"]) != 0)
					{
						isConditionMet = true;
						break;
					}
				}
			}
			if (!isConditionMet)
			{
				MessageBox.Show("At least one row needs to be filled in one of the grids.");
				return;
			}
			GridEXRow[] rows = grdcasualleave.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				hrmLeaveQuotaDetail leaveQuotaDetail2 = new hrmLeaveQuotaDetail();
				leaveQuotaDetail2.LeaveTypeProfileId = Conversion.ToInt(r2.Cells["Id"].Value);
				leaveQuotaDetail2.QuotaValue = Conversion.ToInt(r2.Cells["QuotaValue"].Value);
				leaveQuota.HrmLeaveQuotaDetailList.Add(leaveQuotaDetail2);
			}
			rows = grdspecialleave.GetRows();
			foreach (GridEXRow r in rows)
			{
				hrmLeaveQuotaDetail leaveQuotaDetail = new hrmLeaveQuotaDetail();
				leaveQuotaDetail.LeaveTypeProfileId = Conversion.ToInt(r.Cells["Id"].Value);
				leaveQuotaDetail.QuotaValue = Conversion.ToInt(r.Cells["QuotaValue"].Value);
				leaveQuota.HrmLeaveQuotaDetailList.Add(leaveQuotaDetail);
			}
			hrmLeaveQuota.Save(leaveQuota);
			if (RecId > 0)
			{
				MessageBox.Show("Update Successfully");
			}
			else
			{
				MessageBox.Show("Save Successfully");
			}
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "DatabaseError", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	private void GetById(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			hrmLeaveQuota obj = new hrmLeaveQuota();
			obj = hrmLeaveQuota.GetByID(Id);
			if (obj == null)
			{
				return;
			}
			RecId = obj.LeaveQuotaId;
			txtdatefrom.Value = obj.FromDate;
			txtdateto.Value = obj.ToDate;
			txtdescription.Text = obj.Description;
			if (obj.HrmLeaveQuotaDetailList.Count > 0 && obj.HrmLeaveQuotaDetailList.Count > 0)
			{
				tablecasualleave.Rows.Clear();
				tableSpecialLeave.Rows.Clear();
				for (int i = 0; i < obj.HrmLeaveQuotaDetailList.Count; i++)
				{
					if (obj.HrmLeaveQuotaDetailList[i].ProfileTypeName == "Casual Leave")
					{
						tablecasualleave.Rows.Add(obj.HrmLeaveQuotaDetailList[i].LeaveTypeProfileId, obj.HrmLeaveQuotaDetailList[i].ProfileName, obj.HrmLeaveQuotaDetailList[i].QuotaValue);
					}
					if (obj.HrmLeaveQuotaDetailList[i].ProfileTypeName == "Special Leave")
					{
						tableSpecialLeave.Rows.Add(obj.HrmLeaveQuotaDetailList[i].LeaveTypeProfileId, obj.HrmLeaveQuotaDetailList[i].ProfileName, obj.HrmLeaveQuotaDetailList[i].QuotaValue);
					}
				}
				grdcasualleave.DataSource = tablecasualleave;
				grdcasualleave.RetrieveStructure();
				grdCasualleavesetting();
				grdspecialleave.DataSource = tableSpecialLeave;
				grdspecialleave.RetrieveStructure();
				grdSpecialleavesetting();
			}
			tabControl1.SelectedIndex = 0;
			btnsave.Visible = false;
			btnupdate.Visible = true;
			UpdateMode = true;
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

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Detail")
			{
				HistoryDetailBind(Conversion.ToInt(grdHistory.CurrentRow.Cells["LeaveQuotaId"].Value));
			}
			if (e.Column.Key == "Edit")
			{
				GetById(Conversion.ToInt(grdHistory.CurrentRow.Cells["LeaveQuotaId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridFill()
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("LeaveQuotaId");
			dt.Columns.Add("FromDate");
			dt.Columns.Add("ToDate");
			dt.Columns.Add("Description");
			DataTable dtHistory = hrmLeaveQuota.Getall(new hrmLeaveQuota
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtHistory.Rows.Count > 0)
			{
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					dt.Rows.Add(dtHistory.Rows[i]["LeaveQuotaId"], Conversion.ToDateTime(dtHistory.Rows[i]["FromDate"]).ToString("dd-MMM-yyyy"), Conversion.ToDateTime(dtHistory.Rows[i]["ToDate"]).ToString("dd-MMM-yyyy"), dtHistory.Rows[i]["Description"]);
				}
				grdHistory.DataSource = dt;
				grdHistory.RetrieveStructure();
				grdHistorySetting();
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

	private void HistoryDetailBind(int Id)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		try
		{
			DataTable dtCasualLeave = new DataTable();
			dtCasualLeave.Columns.Add("LeaveType");
			dtCasualLeave.Columns.Add("QuotaValue");
			DataTable dtSpeacialLeave = new DataTable();
			dtSpeacialLeave.Columns.Add("LeaveType");
			dtSpeacialLeave.Columns.Add("QuotaValue");
			dtCasualLeave.Rows.Clear();
			dtSpeacialLeave.Rows.Clear();
			hrmLeaveQuota obj = new hrmLeaveQuota();
			obj = hrmLeaveQuota.GetByID(Id);
			if (obj.HrmLeaveQuotaDetailList.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < obj.HrmLeaveQuotaDetailList.Count; i++)
			{
				if (obj.HrmLeaveQuotaDetailList[i].ProfileTypeName == "Casual Leave")
				{
					dtCasualLeave.Rows.Add(obj.HrmLeaveQuotaDetailList[i].ProfileName, obj.HrmLeaveQuotaDetailList[i].QuotaValue);
				}
				if (obj.HrmLeaveQuotaDetailList[i].ProfileTypeName == "Special Leave")
				{
					dtSpeacialLeave.Rows.Add(obj.HrmLeaveQuotaDetailList[i].ProfileName, obj.HrmLeaveQuotaDetailList[i].QuotaValue);
				}
			}
			grdCasualHistory.DataSource = dtCasualLeave;
			grdCasualHistory.RetrieveStructure();
			grdCasualHistory.RootTable.Columns["LeaveType"].Width = 300;
			grdCasualHistory.RootTable.Columns["QuotaValue"].Width = 100;
			grdSpecialHistory.DataSource = dtSpeacialLeave;
			grdSpecialHistory.RetrieveStructure();
			grdSpecialHistory.RootTable.Columns["LeaveType"].Width = 300;
			grdSpecialHistory.RootTable.Columns["QuotaValue"].Width = 100;
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
			grdHistory.RootTable.Columns["LeaveQuotaId"].Visible = false;
			grdHistory.RootTable.Columns["Description"].Width = 300;
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
			grdHistory.RootTable.Columns["Detail"].Position = 0;
			grdHistory.RootTable.Columns["Edit"].Position = 1;
			grdHistory.FrozenColumns = 2;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LeaveQuotaPolicy_KeyDown(object sender, KeyEventArgs e)
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

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
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

	private void btnlookup_Click(object sender, EventArgs e)
	{
		try
		{
			ProfileDefine profileDefine = new ProfileDefine(UserAccount);
			profileDefine.profileTypeId = 4;
			profileDefine.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnOfficialleave_Click(object sender, EventArgs e)
	{
		try
		{
			ProfileDefine profileDefine = new ProfileDefine(UserAccount);
			profileDefine.profileTypeId = 3;
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
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Expected O, but got Unknown
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Expected O, but got Unknown
		//IL_085a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0864: Expected O, but got Unknown
		//IL_0b76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b80: Expected O, but got Unknown
		//IL_1eb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ebe: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.PolicyManagment.LeaveQuotaPolicy));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel17 = new System.Windows.Forms.Panel();
		this.grdspecialleave = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel16 = new System.Windows.Forms.Panel();
		this.grdcasualleave = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.txtdateto = new System.Windows.Forms.DateTimePicker();
		this.label48 = new System.Windows.Forms.Label();
		this.txtdatefrom = new System.Windows.Forms.DateTimePicker();
		this.label49 = new System.Windows.Forms.Label();
		this.txtdescription = new System.Windows.Forms.TextBox();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnlookup = new System.Windows.Forms.ToolStripButton();
		this.btnOfficialleave = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.panel14 = new System.Windows.Forms.Panel();
		this.panel21 = new System.Windows.Forms.Panel();
		this.grdSpecialHistory = new GridEX();
		this.panel22 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.panel19 = new System.Windows.Forms.Panel();
		this.grdCasualHistory = new GridEX();
		this.panel20 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdspecialleave).BeginInit();
		this.panel18.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdcasualleave).BeginInit();
		this.panel15.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel11.SuspendLayout();
		this.tsItemType.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel12.SuspendLayout();
		this.panel14.SuspendLayout();
		this.panel21.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSpecialHistory).BeginInit();
		this.panel22.SuspendLayout();
		this.panel13.SuspendLayout();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdCasualHistory).BeginInit();
		this.panel20.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel8.SuspendLayout();
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
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel7);
		this.tabPage1.Controls.Add(this.tsItemType);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(956, 635);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.panel2);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(3, 133);
		this.panel4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(950, 499);
		this.panel4.TabIndex = 23;
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(950, 499);
		this.panel2.TabIndex = 0;
		this.panel6.Controls.Add(this.panel17);
		this.panel6.Controls.Add(this.panel18);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(481, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(469, 499);
		this.panel6.TabIndex = 2;
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.grdspecialleave);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel17.Location = new System.Drawing.Point(0, 30);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(469, 469);
		this.panel17.TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.grdspecialleave).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdspecialleave.FilterMode = (FilterMode)2;
		this.grdspecialleave.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdspecialleave).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdspecialleave.GroupByBoxVisible = false;
		this.grdspecialleave.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdspecialleave).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdspecialleave).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdspecialleave).Name = "grdspecialleave";
		((ScrollableBase)this.grdspecialleave).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdspecialleave).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdspecialleave).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdspecialleave).Size = new System.Drawing.Size(469, 469);
		((System.Windows.Forms.Control)(object)this.grdspecialleave).TabIndex = 0;
		this.grdspecialleave.TotalRow = (InheritableBoolean)1;
		this.grdspecialleave.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdspecialleave).VisualStyle = (VisualStyle)4;
		this.grdspecialleave.UpdatingCell += new UpdatingCellEventHandler(grdspecialleave_UpdatingCell);
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.label2);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 0);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(469, 30);
		this.panel18.TabIndex = 3;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 10.75f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(7, 5);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(112, 18);
		this.label2.TabIndex = 6;
		this.label2.Text = "Special Leave";
		this.panel5.Controls.Add(this.panel16);
		this.panel5.Controls.Add(this.panel15);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(481, 499);
		this.panel5.TabIndex = 1;
		this.panel16.Controls.Add((System.Windows.Forms.Control)(object)this.grdcasualleave);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel16.Location = new System.Drawing.Point(0, 30);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(481, 469);
		this.panel16.TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.grdcasualleave).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdcasualleave.FilterMode = (FilterMode)2;
		this.grdcasualleave.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdcasualleave).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdcasualleave.GroupByBoxVisible = false;
		this.grdcasualleave.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdcasualleave).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdcasualleave).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdcasualleave).Name = "grdcasualleave";
		((ScrollableBase)this.grdcasualleave).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdcasualleave).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdcasualleave).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdcasualleave).Size = new System.Drawing.Size(481, 469);
		((System.Windows.Forms.Control)(object)this.grdcasualleave).TabIndex = 0;
		this.grdcasualleave.TotalRow = (InheritableBoolean)1;
		this.grdcasualleave.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdcasualleave).VisualStyle = (VisualStyle)4;
		this.grdcasualleave.UpdatingCell += new UpdatingCellEventHandler(grdcasualleave_UpdatingCell);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label1);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(481, 30);
		this.panel15.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 10.75f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(4, 5);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(108, 18);
		this.label1.TabIndex = 5;
		this.label1.Text = "Offical Leave";
		this.panel7.Controls.Add(this.panel11);
		this.panel7.Controls.Add(this.label50);
		this.panel7.Controls.Add(this.txtdateto);
		this.panel7.Controls.Add(this.label48);
		this.panel7.Controls.Add(this.txtdatefrom);
		this.panel7.Controls.Add(this.label49);
		this.panel7.Controls.Add(this.txtdescription);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 30);
		this.panel7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(950, 103);
		this.panel7.TabIndex = 22;
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label5);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Margin = new System.Windows.Forms.Padding(4);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(950, 32);
		this.panel11.TabIndex = 8;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(4, 6);
		this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(164, 19);
		this.label5.TabIndex = 9;
		this.label5.Text = "Leave Quota Policy";
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(12, 49);
		this.label50.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(63, 15);
		this.label50.TabIndex = 2;
		this.label50.Text = "From Date";
		this.txtdateto.CustomFormat = "dd-MMM-yy";
		this.txtdateto.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdateto.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdateto.Location = new System.Drawing.Point(78, 73);
		this.txtdateto.Name = "txtdateto";
		this.txtdateto.Size = new System.Drawing.Size(181, 23);
		this.txtdateto.TabIndex = 1;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(12, 77);
		this.label48.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(48, 15);
		this.label48.TabIndex = 0;
		this.label48.Text = "To Date";
		this.txtdatefrom.CustomFormat = "dd-MMM-yy";
		this.txtdatefrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdatefrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdatefrom.Location = new System.Drawing.Point(78, 45);
		this.txtdatefrom.Name = "txtdatefrom";
		this.txtdatefrom.Size = new System.Drawing.Size(181, 23);
		this.txtdatefrom.TabIndex = 0;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(269, 49);
		this.label49.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(68, 15);
		this.label49.TabIndex = 1;
		this.label49.Text = "Description";
		this.txtdescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdescription.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdescription.Location = new System.Drawing.Point(339, 45);
		this.txtdescription.Margin = new System.Windows.Forms.Padding(4);
		this.txtdescription.Multiline = true;
		this.txtdescription.Name = "txtdescription";
		this.txtdescription.Size = new System.Drawing.Size(559, 51);
		this.txtdescription.TabIndex = 2;
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnnew, this.btnupdate, this.btnsave, this.btnlookup, this.btnOfficialleave });
		this.tsItemType.Location = new System.Drawing.Point(3, 3);
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
		this.btnlookup.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnlookup.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnlookup.Name = "btnlookup";
		this.btnlookup.Size = new System.Drawing.Size(156, 24);
		this.btnlookup.Text = "Define Special Leaves";
		this.btnlookup.Click += new System.EventHandler(btnlookup_Click);
		this.btnOfficialleave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnOfficialleave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnOfficialleave.Name = "btnOfficialleave";
		this.btnOfficialleave.Size = new System.Drawing.Size(151, 24);
		this.btnOfficialleave.Text = "Define Offical Leaves";
		this.btnOfficialleave.Click += new System.EventHandler(btnOfficialleave_Click);
		this.tabPage2.Controls.Add(this.panel3);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(956, 635);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel3.Controls.Add(this.panel12);
		this.panel3.Controls.Add(this.panel9);
		this.panel3.Controls.Add(this.panel8);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(950, 629);
		this.panel3.TabIndex = 0;
		this.panel12.Controls.Add(this.panel14);
		this.panel12.Controls.Add(this.panel13);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel12.Location = new System.Drawing.Point(0, 334);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(950, 295);
		this.panel12.TabIndex = 3;
		this.panel14.Controls.Add(this.panel21);
		this.panel14.Controls.Add(this.panel22);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel14.Location = new System.Drawing.Point(480, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(470, 295);
		this.panel14.TabIndex = 1;
		this.panel21.Controls.Add((System.Windows.Forms.Control)(object)this.grdSpecialHistory);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel21.Location = new System.Drawing.Point(0, 30);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(470, 265);
		this.panel21.TabIndex = 5;
		this.grdSpecialHistory.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdSpecialHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdSpecialHistory.FilterMode = (FilterMode)2;
		this.grdSpecialHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdSpecialHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSpecialHistory.GroupByBoxVisible = false;
		this.grdSpecialHistory.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdSpecialHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdSpecialHistory).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdSpecialHistory).Name = "grdSpecialHistory";
		((ScrollableBase)this.grdSpecialHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdSpecialHistory).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdSpecialHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSpecialHistory).Size = new System.Drawing.Size(470, 265);
		((System.Windows.Forms.Control)(object)this.grdSpecialHistory).TabIndex = 3;
		this.grdSpecialHistory.TotalRow = (InheritableBoolean)1;
		this.grdSpecialHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSpecialHistory).VisualStyle = (VisualStyle)4;
		this.panel22.BackColor = System.Drawing.Color.Teal;
		this.panel22.Controls.Add(this.label3);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel22.Location = new System.Drawing.Point(0, 0);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(470, 30);
		this.panel22.TabIndex = 4;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 10.75f, System.Drawing.FontStyle.Bold);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(184, 7);
		this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(112, 18);
		this.label3.TabIndex = 7;
		this.label3.Text = "Special Leave";
		this.panel13.Controls.Add(this.panel19);
		this.panel13.Controls.Add(this.panel20);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(480, 295);
		this.panel13.TabIndex = 0;
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.grdCasualHistory);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel19.Location = new System.Drawing.Point(0, 30);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(480, 265);
		this.panel19.TabIndex = 4;
		this.grdCasualHistory.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdCasualHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdCasualHistory.FilterMode = (FilterMode)2;
		this.grdCasualHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdCasualHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdCasualHistory.GroupByBoxVisible = false;
		this.grdCasualHistory.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdCasualHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdCasualHistory).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdCasualHistory).Name = "grdCasualHistory";
		((ScrollableBase)this.grdCasualHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdCasualHistory).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdCasualHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdCasualHistory).Size = new System.Drawing.Size(480, 265);
		((System.Windows.Forms.Control)(object)this.grdCasualHistory).TabIndex = 2;
		this.grdCasualHistory.TotalRow = (InheritableBoolean)1;
		this.grdCasualHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdCasualHistory).VisualStyle = (VisualStyle)4;
		this.panel20.BackColor = System.Drawing.Color.Teal;
		this.panel20.Controls.Add(this.label6);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(480, 30);
		this.panel20.TabIndex = 3;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 10.75f, System.Drawing.FontStyle.Bold);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(197, 7);
		this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(108, 18);
		this.label6.TabIndex = 6;
		this.label6.Text = "Offical Leave";
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 31);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(950, 303);
		this.panel9.TabIndex = 1;
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
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(950, 303);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 1;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.ctrlGrdBar3);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(950, 31);
		this.panel8.TabIndex = 0;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(886, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.grdHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(60, 27);
		this.ctrlGrdBar3.TabIndex = 22;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 661);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "LeaveQuotaPolicy";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(LeaveQuotaPolicy_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(LeaveQuotaPolicy_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage1.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdspecialleave).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel16.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdcasualleave).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		this.panel21.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSpecialHistory).EndInit();
		this.panel22.ResumeLayout(false);
		this.panel22.PerformLayout();
		this.panel13.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdCasualHistory).EndInit();
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel8.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
