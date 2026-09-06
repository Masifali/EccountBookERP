using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Lab;
using Architecture.BLL.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Lab;
using Architecture.Model.Production;
using Architecture.WinApp.Common;
using Architecture.WinApp.Production;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Lab;

public class LabInProcessAnalysisStepAndParameterSchedule : Form
{
	public UserAccount UserAccount = new UserAccount();

	private int RecId;

	private DropDownBind DDL = new DropDownBind();

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	internal GridEX grdfrm;

	private Panel panel3;

	private Panel panel2;

	private ToolStrip tsItemTaxSchedule;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	internal UltraCombo CmbParentParameter;

	private Label label6;

	internal UltraCombo cmbProcessStepName;

	private Label label1;

	internal UltraCombo cmbPlantName;

	private Label label2;

	private TextBox txtSortNo;

	private Button btnPlant;

	private Button BtnStep;

	private Button btnDefineParameter;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton btnRefresh;

	private GroupBox groupBox1;

	private Panel panel5;

	private Label label3;

	public LabInProcessAnalysisStepAndParameterSchedule(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private bool formvalidation()
	{
		if (((UltraGridBase)cmbPlantName).ActiveRow == null)
		{
			MessageBox.Show("Plant Name Field is Required");
			cmbPlantName.Focus();
			return false;
		}
		if (((UltraGridBase)cmbProcessStepName).ActiveRow == null)
		{
			MessageBox.Show("Process Step Field is Required");
			cmbProcessStepName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbParentParameter).ActiveRow == null)
		{
			MessageBox.Show("Parent Parameter Field is Required");
			CmbParentParameter.Focus();
			return false;
		}
		return true;
	}

	private void refresh()
	{
		try
		{
			btnsave.Visible = true;
			btnupdate.Visible = false;
			((Control)(object)CmbParentParameter).Text = string.Empty;
			((Control)(object)cmbProcessStepName).Text = string.Empty;
			((Control)(object)cmbPlantName).Text = string.Empty;
			gridfill();
			GenerateSortNo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		refresh();
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void BindPlantName()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			DataTable dtProductionplant = new DataTable();
			dtProductionplant = InvProductionPlant.GetAll(new InvProductionPlant
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtProductionplant.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtProductionplant, cmbPlantName, "Id", "Description", "Plant/Feader ", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindParameter()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			InvLabAnalysisItems val = new InvLabAnalysisItems
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtgetAllItem = new DataTable();
			dtgetAllItem = InvLabAnalysisItems.GetAllOrById(val);
			if (dtgetAllItem.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtgetAllItem, CmbParentParameter, "Id", "AnalysisParameterDescription", "Item", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindProcessStepName()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			ProcessStep val = new ProcessStep
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtBindProcessStepName = new DataTable();
			dtBindProcessStepName = InvProcessAnalysisStepSchedule.BindProcessStepName(val);
			if (dtBindProcessStepName.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtBindProcessStepName, cmbProcessStepName, "Id", "ProcessStepName", "Process Step Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		try
		{
			if (!formvalidation())
			{
				return;
			}
			if (grdfrm.GetRows().Length != 0)
			{
				GridEXRow[] rows = grdfrm.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (((Control)(object)CmbParentParameter).Text == Conversion.ToString(item.Cells["AnalysisParameterDescription"].Value) && ((Control)(object)cmbPlantName).Text == Conversion.ToString(item.Cells["PlantName"].Value) && ((Control)(object)cmbProcessStepName).Text == Conversion.ToString(item.Cells["ProcessStepName"].Value))
					{
						MessageBox.Show("This Schedule Already Exits In Grid");
						return;
					}
				}
			}
			InvProcessAnalysisStepSchedule obj = new InvProcessAnalysisStepSchedule();
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
			obj.SortNo = Conversion.ToInt((object)txtSortNo.Text);
			obj.PlantId = Conversion.ToInt(cmbPlantName.Value);
			obj.LabInProcessAnalysisStepId = Conversion.ToInt(cmbProcessStepName.Value);
			obj.AnalysisParameterId = Conversion.ToInt(CmbParentParameter.Value);
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.EnteryUserId = UserAccount.EntryUserId;
			obj.EnteryDate = DateTime.Now;
			InvProcessAnalysisStepSchedule.Save(obj);
			if (RecId == 0)
			{
				MessageBox.Show("Save Successfully");
			}
			else
			{
				MessageBox.Show("Update Successfully");
			}
			refresh();
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

	private void GenerateSortNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			int SortNo = InvProcessAnalysisStepSchedule.GenerateCode(new InvProcessAnalysisStepSchedule
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (SortNo > 0)
			{
				txtSortNo.Text = SortNo.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtgetAll = InvProcessAnalysisStepSchedule.GetAllOrById(new InvProcessAnalysisStepSchedule
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtgetAll.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("PlantId", typeof(int));
				dt.Columns.Add("LabInProcessAnalysisStepId", typeof(int));
				dt.Columns.Add("AnalysisParameterId", typeof(int));
				dt.Columns.Add("PlantName", typeof(string));
				dt.Columns.Add("ProcessStepName", typeof(string));
				dt.Columns.Add("AnalysisParameterDescription", typeof(string));
				for (int i = 0; i < dtgetAll.Rows.Count; i++)
				{
					dt.Rows.Add(dtgetAll.Rows[i]["Id"], dtgetAll.Rows[i]["PlantId"], dtgetAll.Rows[i]["LabInProcessAnalysisStepId"], dtgetAll.Rows[i]["AnalysisParameterId"], dtgetAll.Rows[i]["PlantName"], dtgetAll.Rows[i]["ProcessStepName"], dtgetAll.Rows[i]["AnalysisParameterDescription"]);
				}
				grdfrm.DataSource = dt;
				grdfrm.RetrieveStructure();
				grdfrmSetting();
			}
			else
			{
				grdfrm.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrmSetting()
	{
		try
		{
			grdfrm.AllowEdit = (InheritableBoolean)2;
			grdfrm.AllowDelete = (InheritableBoolean)2;
			grdfrm.RootTable.Columns["Id"].Visible = false;
			grdfrm.RootTable.Columns["PlantId"].Visible = false;
			grdfrm.RootTable.Columns["AnalysisParameterId"].Visible = false;
			grdfrm.RootTable.Columns["LabInProcessAnalysisStepId"].Visible = false;
			grdfrm.RootTable.Columns["ProcessStepName"].Width = 250;
			grdfrm.RootTable.Columns["PlantName"].Width = 250;
			grdfrm.RootTable.Columns["AnalysisParameterDescription"].Width = 250;
			ctrlGrdBar1_Load_1(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_DoubleClick(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InvProcessAnalysisStepSchedule obj = new InvProcessAnalysisStepSchedule();
			btnsave.Visible = false;
			btnupdate.Visible = true;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.Id = Conversion.ToInt((object)grdfrm.CurrentRow.Cells["Id"].Value.ToString());
			RecId = obj.Id;
			DataTable dtgetbyId = new DataTable();
			dtgetbyId = InvProcessAnalysisStepSchedule.GetAllOrById(obj);
			if (dtgetbyId.Rows.Count > 0)
			{
				txtSortNo.Text = Conversion.ToString(dtgetbyId.Rows[0]["SortNo"]);
				cmbPlantName.Value = Conversion.ToInt(dtgetbyId.Rows[0]["PlantId"]);
				cmbProcessStepName.Value = Conversion.ToInt(dtgetbyId.Rows[0]["LabInProcessAnalysisStepId"]);
				CmbParentParameter.Value = Conversion.ToInt(dtgetbyId.Rows[0]["AnalysisParameterId"]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvLabAnalysisItems_Load(object sender, EventArgs e)
	{
		try
		{
			btnsave.Visible = true;
			btnupdate.Visible = false;
			gridfill();
			BindPlantName();
			BindProcessStepName();
			BindParameter();
			GenerateSortNo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvLabAnalysisItems_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S)
			{
				btnsave_Click(null, null);
			}
			if (btnupdate.Visible && btnupdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnupdate_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
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

	private void btnPlantDefine_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineProductionPlant(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnProcessStep_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineProcessStep(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDefineParameter_Click(object sender, EventArgs e)
	{
		try
		{
			new InvLabAnalysisItems(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar1_Load_1(object sender, EventArgs e)
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			BindPlantName();
			BindProcessStepName();
			BindParameter();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LabInProcessAnalysisStepAndParameterSchedule_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Expected O, but got Unknown
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Lab.LabInProcessAnalysisStepAndParameterSchedule));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdfrm = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.btnPlant = new System.Windows.Forms.Button();
		this.BtnStep = new System.Windows.Forms.Button();
		this.btnDefineParameter = new System.Windows.Forms.Button();
		this.txtSortNo = new System.Windows.Forms.TextBox();
		this.cmbPlantName = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbProcessStepName = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbParentParameter = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.tsItemTaxSchedule = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPlantName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbProcessStepName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentParameter).BeginInit();
		this.panel2.SuspendLayout();
		this.tsItemTaxSchedule.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(783, 535);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 161);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(783, 374);
		this.panel4.TabIndex = 84;
		this.grdfrm.AllowDelete = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupByBoxVisible = false;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(783, 374);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 0;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdfrm).DoubleClick += new System.EventHandler(grdfrm_DoubleClick);
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 28);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(783, 133);
		this.panel3.TabIndex = 83;
		this.btnPlant.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPlant.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnPlant.Location = new System.Drawing.Point(421, 20);
		this.btnPlant.Name = "btnPlant";
		this.btnPlant.Size = new System.Drawing.Size(27, 26);
		this.btnPlant.TabIndex = 1;
		this.btnPlant.TabStop = false;
		this.btnPlant.UseVisualStyleBackColor = true;
		this.btnPlant.Click += new System.EventHandler(btnPlantDefine_Click);
		this.BtnStep.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnStep.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnStep.Location = new System.Drawing.Point(421, 48);
		this.BtnStep.Name = "BtnStep";
		this.BtnStep.Size = new System.Drawing.Size(27, 26);
		this.BtnStep.TabIndex = 3;
		this.BtnStep.TabStop = false;
		this.BtnStep.UseVisualStyleBackColor = true;
		this.BtnStep.Click += new System.EventHandler(btnProcessStep_Click);
		this.btnDefineParameter.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDefineParameter.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnDefineParameter.Location = new System.Drawing.Point(421, 76);
		this.btnDefineParameter.Name = "btnDefineParameter";
		this.btnDefineParameter.Size = new System.Drawing.Size(27, 26);
		this.btnDefineParameter.TabIndex = 5;
		this.btnDefineParameter.TabStop = false;
		this.btnDefineParameter.UseVisualStyleBackColor = true;
		this.btnDefineParameter.Click += new System.EventHandler(btnDefineParameter_Click);
		this.txtSortNo.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSortNo.Location = new System.Drawing.Point(452, 22);
		this.txtSortNo.Name = "txtSortNo";
		this.txtSortNo.ReadOnly = true;
		this.txtSortNo.Size = new System.Drawing.Size(41, 23);
		this.txtSortNo.TabIndex = 39;
		this.txtSortNo.Visible = false;
		this.cmbPlantName.AllowNull = (DefaultableBoolean)1;
		this.cmbPlantName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPlantName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPlantName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPlantName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.cmbPlantName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPlantName).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPlantName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPlantName).Location = new System.Drawing.Point(165, 20);
		((System.Windows.Forms.Control)(object)this.cmbPlantName).Name = "cmbPlantName";
		((System.Windows.Forms.Control)(object)this.cmbPlantName).Size = new System.Drawing.Size(254, 26);
		((System.Windows.Forms.Control)(object)this.cmbPlantName).TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(8, 26);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(69, 15);
		this.label2.TabIndex = 38;
		this.label2.Text = "Plant Name";
		this.cmbProcessStepName.AllowNull = (DefaultableBoolean)1;
		this.cmbProcessStepName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbProcessStepName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbProcessStepName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbProcessStepName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbProcessStepName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbProcessStepName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbProcessStepName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbProcessStepName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbProcessStepName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbProcessStepName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbProcessStepName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbProcessStepName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbProcessStepName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbProcessStepName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbProcessStepName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbProcessStepName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbProcessStepName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbProcessStepName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbProcessStepName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbProcessStepName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbProcessStepName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbProcessStepName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbProcessStepName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbProcessStepName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbProcessStepName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbProcessStepName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbProcessStepName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbProcessStepName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbProcessStepName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbProcessStepName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbProcessStepName).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbProcessStepName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbProcessStepName).Location = new System.Drawing.Point(165, 48);
		((System.Windows.Forms.Control)(object)this.cmbProcessStepName).Name = "cmbProcessStepName";
		((System.Windows.Forms.Control)(object)this.cmbProcessStepName).Size = new System.Drawing.Size(254, 26);
		((System.Windows.Forms.Control)(object)this.cmbProcessStepName).TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(8, 54);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(155, 15);
		this.label1.TabIndex = 36;
		this.label1.Text = "Process Analysis Step Name";
		this.CmbParentParameter.AllowNull = (DefaultableBoolean)1;
		this.CmbParentParameter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentParameter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentParameter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentParameter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentParameter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentParameter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentParameter).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbParentParameter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParentParameter).Location = new System.Drawing.Point(165, 76);
		((System.Windows.Forms.Control)(object)this.CmbParentParameter).Name = "CmbParentParameter";
		((System.Windows.Forms.Control)(object)this.CmbParentParameter).Size = new System.Drawing.Size(254, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentParameter).TabIndex = 4;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(8, 82);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(96, 15);
		this.label6.TabIndex = 34;
		this.label6.Text = "Parameter Name";
		this.panel2.Controls.Add(this.tsItemTaxSchedule);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(783, 28);
		this.panel2.TabIndex = 0;
		this.tsItemTaxSchedule.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemTaxSchedule.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemTaxSchedule.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnRefresh, this.btnupdate, this.btnsave });
		this.tsItemTaxSchedule.Location = new System.Drawing.Point(0, 0);
		this.tsItemTaxSchedule.Name = "tsItemTaxSchedule";
		this.tsItemTaxSchedule.Size = new System.Drawing.Size(783, 27);
		this.tsItemTaxSchedule.TabIndex = 82;
		this.tsItemTaxSchedule.Text = "toolStrip3";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(60, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(742, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 0;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load_1);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(783, 26);
		this.panel5.TabIndex = 40;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(3, 3);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(448, 19);
		this.label3.TabIndex = 41;
		this.label3.Text = "Lab In Process Analysis Step And Parameter Schedule";
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.txtSortNo);
		this.groupBox1.Controls.Add(this.btnDefineParameter);
		this.groupBox1.Controls.Add(this.BtnStep);
		this.groupBox1.Controls.Add(this.btnPlant);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentParameter);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPlantName);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbProcessStepName);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 26);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(501, 107);
		this.groupBox1.TabIndex = 41;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Information";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.SystemColors.Control;
		base.ClientSize = new System.Drawing.Size(783, 535);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "LabInProcessAnalysisStepAndParameterSchedule";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(LabInProcessAnalysisStepAndParameterSchedule_FormClosing);
		base.Load += new System.EventHandler(InvLabAnalysisItems_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvLabAnalysisItems_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.cmbPlantName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbProcessStepName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentParameter).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.tsItemTaxSchedule.ResumeLayout(false);
		this.tsItemTaxSchedule.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		base.ResumeLayout(false);
	}
}
