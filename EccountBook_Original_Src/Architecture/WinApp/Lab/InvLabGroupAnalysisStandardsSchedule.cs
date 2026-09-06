using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Lab;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Lab;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Lab;

public class InvLabGroupAnalysisStandardsSchedule : Form
{
	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtGrid = new DataTable();

	private string ScreenName = "InvLabGroupAnalysisStandardsSchedule";

	private bool UpdateMode;

	private int RecId;

	private bool FormInitialized;

	private bool FieldsChanged;

	private bool IsLoading;

	private DataTable dtDataFromDb = new DataTable();

	private IContainer components;

	private Panel panel2;

	private Panel panel1;

	private Panel panel5;

	internal GridEX grdfrm;

	private Panel panel4;

	private Panel panel3;

	private ToolStrip tsItemTaxSchedule;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnRefresh;

	private Panel panel6;

	private Label label3;

	private GroupBox groupBox1;

	private Label label4;

	private TextBox txtScheduleName;

	private Label label2;

	private Label label1;

	private DateTimePicker txtEffectedDateTo;

	private DateTimePicker txtEffectedDateFrom;

	internal UltraCombo CmbAnalysisGroup;

	private Label label44;

	private CtrlGrdBar ctrlGrdBar3;

	public InvLabGroupAnalysisStandardsSchedule(UserAccount pUserAccount)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
		AttachChangeHandlers((Control)(object)grdfrm);
		InitializeComponentCustom();
		InitializeComponentMethod();
		FormHelper.FormEventHandler(this);
	}

	private void InvLabAnalysisGroup_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1120, 650);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeComponentCustom()
	{
		try
		{
			ScreenName = Conversion.ToString((object)base.Name);
			if (dtGrid.Columns.Count == 0)
			{
				dtGrid.Columns.Add("Id", typeof(int));
				dtGrid.Columns.Add("AnalysisGroupId", typeof(int));
				dtGrid.Columns.Add("AnalysisGroup", typeof(string));
				dtGrid.Columns.Add("AnalysisItemId", typeof(int));
				dtGrid.Columns.Add("AnalysisItem", typeof(string));
				dtGrid.Columns.Add("MinValue", typeof(double));
				dtGrid.Columns.Add("MaxValue", typeof(double));
				dtGrid.Columns.Add("Active", typeof(bool));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeComponentMethod()
	{
		Task.Run(delegate
		{
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			DataTable data = AnalysisGroupDbCall();
			LabGroupAnalysisStandardsDbCall();
			return new
			{
				Data = data
			};
		}).ContinueWith(t =>
		{
			if (t.IsFaulted)
			{
				MessageBox.Show("Error occurred during database call.");
			}
			else
			{
				var result = t.Result;
				btnsave.Enabled = formright.DoHaveSaveRight;
				btnupdate.Enabled = formright.DoHaveUpdateRights;
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar3, formright);
				ctrlGrdBar3.mGridSaveLayouts.Visible = false;
				ctrlGrdBar3.removeLayoutToolStripMenuItem.Visible = false;
				_ = RecId;
				AnalysisGroupBind(result.Data);
				FormInitialized = true;
				FieldsChanged = false;
				IsLoading = false;
				CmbAnalysisGroup.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private DataTable AnalysisGroupDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return InvLabAnalysisGroup.GetAllOrById(new ReportsParameters
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

	private void AnalysisGroupBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbAnalysisGroup, dt, "Id", "AnalysisGroupDescription", "Analysis Group", AllColumns: false, Conversion.ToInt(CmbAnalysisGroup.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbAnalysisGroup_Leave(object sender, EventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(CmbAnalysisGroup.Value);
			string GroupName = Conversion.ToString((object)((Control)(object)CmbAnalysisGroup).Text);
			if (grdfrm.RowCount > 0)
			{
				string GridGroup = GridEX_Helper.GetFirstOrDefaultValueOfColumn(grdfrm, "AnalysisGroup");
				if (FieldsChanged && !string.IsNullOrEmpty(GridGroup) && GridGroup != GroupName)
				{
					if (MessageBox.Show("Analysis Group : " + GridGroup + " has been changed to " + GroupName + ". Do you want to discard changes and continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						int previousValue = Conversion.ToInt((object)GridEX_Helper.GetFirstOrDefaultValueOfColumn(grdfrm, "AnalysisGroupId"));
						CmbAnalysisGroup.Value = previousValue;
						return;
					}
					IsLoading = true;
					GridFillByAnalysisGroup(Id);
					IsLoading = false;
					FieldsChanged = false;
				}
				else
				{
					IsLoading = true;
					GridFillByAnalysisGroup(Id);
					IsLoading = false;
					FieldsChanged = false;
				}
			}
			else
			{
				IsLoading = true;
				GridFillByAnalysisGroup(Id);
				IsLoading = false;
				FieldsChanged = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AttachChangeHandlers(Control parent)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		foreach (Control ctrl in parent.Controls)
		{
			if (ctrl is TextBox txtBox)
			{
				txtBox.TextChanged += delegate
				{
					FieldsChanged = true;
				};
			}
			else if (ctrl is DateTimePicker dateTime)
			{
				dateTime.ValueChanged += delegate
				{
					FieldsChanged = true;
				};
			}
			else
			{
				GridEX grid = (GridEX)(object)((ctrl is GridEX) ? ctrl : null);
				if (grid != null)
				{
					grid.CellValueChanged += (ColumnActionEventHandler)delegate
					{
						if (!IsLoading)
						{
							FieldsChanged = true;
						}
					};
					grid.CellEdited += (ColumnActionEventHandler)delegate
					{
						if (!IsLoading)
						{
							FieldsChanged = true;
						}
					};
					grid.UpdatingRecord += delegate
					{
						if (!IsLoading)
						{
							FieldsChanged = true;
						}
					};
					grid.AddingRecord += delegate
					{
						if (!IsLoading)
						{
							FieldsChanged = true;
						}
					};
					grid.DeletingRecord += (RowActionCancelEventHandler)delegate
					{
						if (!IsLoading)
						{
							FieldsChanged = true;
						}
					};
				}
			}
			if (ctrl.HasChildren)
			{
				AttachChangeHandlers(ctrl);
			}
		}
	}

	private void Insert()
	{
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Expected O, but got Unknown
		try
		{
			if (grdfrm.RowCount == 0)
			{
				MessageBox.Show("At least one record is required in grid.");
			}
			else
			{
				if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
				{
					((Control)(object)CmbAnalysisGroup, "Analysis Group", FormHelper.ValidType.INT),
					(txtScheduleName, "Schedule Name", FormHelper.ValidType.String),
					(txtEffectedDateFrom, "Effected Date From", FormHelper.ValidType.DateTime),
					(txtEffectedDateTo, "Effected Date To", FormHelper.ValidType.DateTime)
				}) || !FormHelper.ConfirmAction((RecId == 0) ? "Are you sure to Save" : "Are you sure to Update"))
				{
					return;
				}
				LabAnalysisStandardSchedule Or = new LabAnalysisStandardSchedule();
				Or.LabAnalysisStandardScheduleId = RecId;
				Or.OrganizationId = UserAccount.OrganizationId;
				Or.CompanyId = UserAccount.CompanyId;
				Or.EntryUserId = UserAccount.ID;
				Or.EntryDate = DateTime.Now;
				Or.ModifyDate = DateTime.Now;
				Or.ModifyUserId = UserAccount.ID;
				Or.LabAnalysisStandardScheduleList = new List<LabAnalysisStandardSchedule>();
				bool hasActiveRow = false;
				GridEXRow[] rows = grdfrm.GetRows();
				foreach (GridEXRow r in rows)
				{
					LabAnalysisStandardSchedule vd = new LabAnalysisStandardSchedule();
					vd.LabAnalysisStandardScheduleId = ((RecId != 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
					vd.ActionId = ((vd.LabAnalysisStandardScheduleId <= 0) ? 1 : 2);
					FillLabGroupAnalysisStandardsFromGrid(vd, r);
					FormHelper.ValidateField(vd.LabAnalysisGroupId, "Analysis Group", r.RowIndex);
					FormHelper.ValidateField(vd.LabAnalysisItemId, "Analysis Item", r.RowIndex);
					FormHelper.ValidateField(vd.MaxValue, "MaxValue", r.RowIndex);
					Or.LabAnalysisStandardScheduleList.Add(vd);
					if (vd.Active)
					{
						hasActiveRow = true;
					}
				}
				if (!hasActiveRow)
				{
					throw new ArgumentException("At least one row must have Active = 1.");
				}
				int success = LabAnalysisStandardSchedule.Save(Or);
				Or.LabAnalysisStandardScheduleId = success;
				if (RecId > 0)
				{
					MessageBox.Show("Data Update Successfully....  " + Or.LabAnalysisStandardScheduleName);
				}
				else
				{
					MessageBox.Show("Data Save Successfully....  " + Or.LabAnalysisStandardScheduleName);
				}
				Reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillLabGroupAnalysisStandardsFromGrid(LabAnalysisStandardSchedule standards, GridEXRow r)
	{
		standards.LabAnalysisStandardScheduleName = Conversion.ToString((object)txtScheduleName.Text);
		standards.EffectiveDateFrom = Conversion.ToDateTime((object)txtEffectedDateFrom.Value);
		standards.EffectiveDateTo = Conversion.ToDateTime((object)txtEffectedDateTo.Value);
		standards.LabAnalysisGroupId = Conversion.ToInt(r.Cells["AnalysisGroupId"].Value);
		standards.LabAnalysisGroupDescription = Conversion.ToString(r.Cells["AnalysisGroup"].Value);
		standards.LabAnalysisItemId = Conversion.ToInt(r.Cells["AnalysisItemId"].Value);
		standards.LabAnalysisItemName = Conversion.ToString(r.Cells["AnalysisItem"].Value);
		standards.MinValue = Conversion.ToDecimal(r.Cells["MinValue"].Value);
		standards.MaxValue = Conversion.ToDecimal(r.Cells["MaxValue"].Value);
		standards.Active = Conversion.ToBool(r.Cells["Active"].Value);
		standards.LabAnalysisStandardScheduleId = RecId;
		standards.OrganizationId = UserAccount.OrganizationId;
		standards.CompanyId = UserAccount.CompanyId;
		standards.EntryUserId = UserAccount.ID;
		standards.EntryDate = DateTime.Now;
		standards.ModifyDate = DateTime.Now;
		standards.ModifyUserId = UserAccount.ID;
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
			if (RecId == 0)
			{
				MessageBox.Show("No record selected to update.");
			}
			else
			{
				Insert();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGrids()
	{
		try
		{
			DataTable dataTable = dtGrid;
			if (dataTable != null && dataTable.Columns.Count > 0)
			{
				grdfrm.DataSource = dtGrid;
				grdfrm.RetrieveStructure();
				grdfrmSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LabGroupAnalysisStandardsDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtDataFromDb = InvLabGroupAnalysisStandards.GetAllOrById(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridFillByAnalysisGroup(int GroupId)
	{
		try
		{
			dtGrid.Clear();
			EnumerableRowCollection<DataRow> filteredRows = from r in dtDataFromDb.AsEnumerable()
				where Conversion.ToInt(r["InvLabAnalysisGroup"]) == GroupId
				select r;
			DataTable dt = ((!filteredRows.Any()) ? dtDataFromDb.Clone() : filteredRows.CopyToDataTable());
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				dtGrid.Rows.Add(0, dt.Rows[i]["InvLabAnalysisGroup"], dt.Rows[i]["AnalysisGroupDescription"], dt.Rows[i]["InvLabAnalysisItems"], dt.Rows[i]["AnalysisParameterDescription"], dt.Rows[i]["MinValue"], dt.Rows[i]["MaxValue"], false);
			}
			BindGrids();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdfrmSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "AnalysisGroupId", "AnalysisItemId" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"MinValue",
					(EditType)1
				},
				{
					"MaxValue",
					(EditType)1
				},
				{
					"Active",
					(EditType)8
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdfrm, 2, 3, hiddenColumns, editableColumns);
			grdfrm.RootTable.Columns["AnalysisGroup"].Width = 200;
			grdfrm.RootTable.Columns["AnalysisItem"].Width = 200;
			grdfrm.RootTable.Columns["MinValue"].Width = 65;
			grdfrm.RootTable.Columns["MaxValue"].Width = 65;
			grdfrm.RootTable.Columns["Active"].Width = 80;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdfrm.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				Conversion.ToInt(item.Cells["Id"].Value);
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 13;
				}
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
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		AnalysisGroupBind(AnalysisGroupDbCall());
		LabGroupAnalysisStandardsDbCall();
	}

	private void InvLabGroupAnalysisStandardsSchedule_FormClosing(object sender, FormClosingEventArgs e)
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

	private void Reset()
	{
		try
		{
			RecId = 0;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			CmbAnalysisGroup.Value = null;
			txtScheduleName.Text = string.Empty;
			dtGrid.Clear();
			BindGrids();
			txtScheduleName.Focus();
			FieldsChanged = false;
			IsLoading = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvLabGroupAnalysisStandardsSchedule_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 80;
			}
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
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Lab.InvLabGroupAnalysisStandardsSchedule));
		this.panel2 = new System.Windows.Forms.Panel();
		this.grdfrm = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbAnalysisGroup = new UltraCombo();
		this.label44 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtEffectedDateTo = new System.Windows.Forms.DateTimePicker();
		this.txtEffectedDateFrom = new System.Windows.Forms.DateTimePicker();
		this.txtScheduleName = new System.Windows.Forms.TextBox();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.tsItemTaxSchedule = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel5.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAnalysisGroup).BeginInit();
		this.panel6.SuspendLayout();
		this.panel3.SuspendLayout();
		this.tsItemTaxSchedule.SuspendLayout();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(764, 532);
		this.panel2.TabIndex = 0;
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupByBoxVisible = false;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 146);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(764, 386);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 1;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdfrm).KeyDown += new System.Windows.Forms.KeyEventHandler(grdfrm_KeyDown);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 125);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(764, 21);
		this.panel5.TabIndex = 80;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(721, 0);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.grdfrm;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(43, 20);
		this.ctrlGrdBar3.TabIndex = 128;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 31);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(764, 94);
		this.panel4.TabIndex = 0;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup);
		this.groupBox1.Controls.Add(this.label44);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.txtEffectedDateTo);
		this.groupBox1.Controls.Add(this.txtEffectedDateFrom);
		this.groupBox1.Controls.Add(this.txtScheduleName);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 26);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(752, 67);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Information";
		this.CmbAnalysisGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbAnalysisGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAnalysisGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAnalysisGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAnalysisGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).Location = new System.Drawing.Point(6, 35);
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).Name = "CmbAnalysisGroup";
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).Size = new System.Drawing.Size(191, 26);
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).TextChanged += new System.EventHandler(CmbAnalysisGroup_Leave);
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).Leave += new System.EventHandler(CmbAnalysisGroup_Leave);
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(10, 19);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(86, 15);
		this.label44.TabIndex = 28;
		this.label44.Text = "Analysis Group";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(203, 18);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(91, 15);
		this.label4.TabIndex = 19;
		this.label4.Text = "Schedule Name";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(553, 18);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(94, 15);
		this.label2.TabIndex = 26;
		this.label2.Text = "Effected Date To";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(431, 18);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(109, 15);
		this.label1.TabIndex = 26;
		this.label1.Text = "Effected Date From";
		this.txtEffectedDateTo.CustomFormat = "dd-MMM-yy";
		this.txtEffectedDateTo.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtEffectedDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtEffectedDateTo.Location = new System.Drawing.Point(553, 38);
		this.txtEffectedDateTo.Name = "txtEffectedDateTo";
		this.txtEffectedDateTo.Size = new System.Drawing.Size(121, 22);
		this.txtEffectedDateTo.TabIndex = 3;
		this.txtEffectedDateFrom.CustomFormat = "dd-MMM-yy";
		this.txtEffectedDateFrom.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtEffectedDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtEffectedDateFrom.Location = new System.Drawing.Point(431, 38);
		this.txtEffectedDateFrom.Name = "txtEffectedDateFrom";
		this.txtEffectedDateFrom.Size = new System.Drawing.Size(121, 22);
		this.txtEffectedDateFrom.TabIndex = 2;
		this.txtScheduleName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtScheduleName.Location = new System.Drawing.Point(200, 38);
		this.txtScheduleName.Name = "txtScheduleName";
		this.txtScheduleName.Size = new System.Drawing.Size(229, 23);
		this.txtScheduleName.TabIndex = 1;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label3);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(764, 27);
		this.panel6.TabIndex = 21;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(6, 3);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(244, 17);
		this.label3.TabIndex = 22;
		this.label3.Text = "Group Analysis Standard Schedule";
		this.panel3.Controls.Add(this.tsItemTaxSchedule);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(764, 31);
		this.panel3.TabIndex = 0;
		this.tsItemTaxSchedule.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemTaxSchedule.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemTaxSchedule.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnRefresh, this.btnupdate, this.btnsave });
		this.tsItemTaxSchedule.Location = new System.Drawing.Point(0, 0);
		this.tsItemTaxSchedule.Name = "tsItemTaxSchedule";
		this.tsItemTaxSchedule.Size = new System.Drawing.Size(764, 27);
		this.tsItemTaxSchedule.TabIndex = 78;
		this.tsItemTaxSchedule.Text = "toolStrip3";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(764, 532);
		this.panel1.TabIndex = 1;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(764, 532);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "InvLabGroupAnalysisStandardsSchedule";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvLabGroupAnalysisStandardsSchedule_FormClosing);
		base.Load += new System.EventHandler(InvLabAnalysisGroup_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvLabGroupAnalysisStandardsSchedule_KeyDown);
		this.panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAnalysisGroup).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.tsItemTaxSchedule.ResumeLayout(false);
		this.tsItemTaxSchedule.PerformLayout();
		this.panel1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
