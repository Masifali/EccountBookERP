using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.CmTr.TransactionForm;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.CmTr.TransactionForm;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.CmTr.LookUp;

public class InvLookup : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private int RecId;

	private bool UpdateMode;

	private DataTable dtgrid = new DataTable();

	private IContainer components;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnRefresh;

	private Panel panel4;

	private Label label15;

	private Panel panel1;

	internal UltraCombo cmbaccounttitle;

	internal UltraCombo cmbProfileName;

	private Label label2;

	private TextBox txtLookUpName;

	private TextBox txtCode;

	private Label label4;

	private Label label1;

	private Label label3;

	private Panel panel2;

	private Label label5;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton btnshortcut;

	internal GridEX grdHistory;

	private TextBox textBox1;

	private Label label6;

	private CheckBox chkisactive;

	public InvLookup(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void InvLookup_Load(object sender, EventArgs e)
	{
		try
		{
			LookupTypeBind();
			AccountTypeFill();
			GridBind();
			((Control)(object)cmbProfileName).Enabled = false;
			txtCode.Enabled = false;
			txtLookUpName.Enabled = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void LookupTypeBind()
	{
		try
		{
			int Id = Conversion.ToInt(cmbProfileName.Value);
			DataTable lst = LookUps.FormHistory_LookUpType();
			if (lst.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtLTCombo = new DataTable();
			dtLTCombo.Columns.Add("Id");
			dtLTCombo.Columns.Add("ProfileName");
			for (int i = 0; i < lst.Rows.Count; i++)
			{
				dtLTCombo.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["LookUpTypeName"]);
			}
			if (dtLTCombo.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtLTCombo, cmbProfileName, "Id", "ProfileName", "LookUpType Name ", ZeroIndex: true);
			if (Id > 0)
			{
				if (dtLTCombo.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					cmbProfileName.Value = Id;
				}
				else
				{
					((Control)(object)cmbProfileName).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountTypeFill()
	{
		try
		{
			int Id = Conversion.ToInt(cmbaccounttitle.Value);
			DataTable dataTable = CommonServices.GetAllDetailAccount();
			if (dataTable.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dataTable, cmbaccounttitle, "Id", "AccountTitle", "AccountTitle ", ZeroIndex: true);
			if (Id > 0)
			{
				if (dataTable.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					cmbaccounttitle.Value = Id;
				}
				else
				{
					((Control)(object)cmbaccounttitle).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			LookUps lookupType = new LookUps();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				lookupType.Id = RecId;
			}
			lookupType.InvLookupTypeId = Conversion.ToInt(cmbProfileName.Value);
			lookupType.GLAccountId = Conversion.ToInt(cmbaccounttitle.Value);
			lookupType.LookupName = Conversion.ToString((object)txtLookUpName.Text);
			lookupType.LookupNameOtherLingo = Conversion.ToString((object)textBox1.Text);
			lookupType.Code = Conversion.ToString((object)txtCode.Text);
			lookupType.IsActive = Conversion.ToBool((object)txtCode.Text);
			LookUps.Save_Lookups(lookupType);
			if (RecId > 0)
			{
				MessageBox.Show("Update Successfully");
				FormReset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((Control)(object)cmbProfileName).Text.Trim() == string.Empty)
		{
			MessageBox.Show("LookUpTypeName Field Required");
			cmbProfileName.Focus();
			return false;
		}
		if (((Control)(object)cmbaccounttitle).Text.Trim() == string.Empty)
		{
			MessageBox.Show("Account Title Field Required");
			cmbaccounttitle.Focus();
			return false;
		}
		if (txtLookUpName.Text.Trim() == string.Empty)
		{
			MessageBox.Show("LookUpName Field Required");
			txtLookUpName.Focus();
			return false;
		}
		if (textBox1.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Other Name Field Required");
			textBox1.Focus();
			return false;
		}
		if (txtCode.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Code Field Required");
			txtCode.Focus();
			return false;
		}
		return true;
	}

	private void FormReset()
	{
		try
		{
			txtCode.Text = string.Empty;
			txtLookUpName.Text = string.Empty;
			((Control)(object)cmbProfileName).Text = string.Empty;
			((Control)(object)cmbaccounttitle).Text = string.Empty;
			btnUpdate.Visible = true;
			UpdateMode = true;
			cmbProfileName.Focus();
			GridBind();
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
			FormReset();
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
			AccountTypeFill();
			LookupTypeBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvLookup_FormClosed(object sender, FormClosedEventArgs e)
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

	public void GridBind()
	{
		try
		{
			dtgrid = LookUps.FormHistory_LookUps();
			if (dtgrid.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("LookUpTypeName", typeof(string));
				dt.Columns.Add("AccountTitle", typeof(string));
				dt.Columns.Add("Code", typeof(string));
				dt.Columns.Add("LookupName", typeof(string));
				dt.Columns.Add("OtherName", typeof(string));
				dt.Columns.Add("Active", typeof(bool));
				dt.Clear();
				for (int i = 0; i < dtgrid.Rows.Count; i++)
				{
					dt.Rows.Add(Conversion.ToInt(dtgrid.Rows[i]["Id"]), Conversion.ToString(dtgrid.Rows[i]["LookUpTypeName"]), Conversion.ToString(dtgrid.Rows[i]["AccountTitle"]), Conversion.ToString(dtgrid.Rows[i]["Code"]), Conversion.ToString(dtgrid.Rows[i]["LookupName"]), Conversion.ToString(dtgrid.Rows[i]["LookupNameOtherLingo"]), Conversion.ToBool(dtgrid.Rows[i]["IsActive"]));
				}
				grdHistory.DataSource = dt;
				grdHistory.RetrieveStructure();
				GridSetting();
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

	public void GridSetting()
	{
		try
		{
			grdHistory.RootTable.Columns["Id"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(grdHistory);
			ctrlGrdBar1_Load(null, null);
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

	private void grdlookups_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				LookUps eduLookups = new LookUps();
				RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
				eduLookups = LookUps.ReadById_LookUps(RecId);
				if (eduLookups != null)
				{
					txtCode.Text = eduLookups.Code.ToString();
					txtLookUpName.Text = eduLookups.LookupName;
					textBox1.Text = eduLookups.LookupNameOtherLingo;
					((Control)(object)cmbProfileName).Text = eduLookups.InvLookupTypeId.ToString();
					((Control)(object)cmbaccounttitle).Text = eduLookups.GLAccountId.ToString();
					chkisactive.Checked = eduLookups.IsActive;
					btnUpdate.Visible = true;
					UpdateMode = true;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbProfileName_Leave(object sender, EventArgs e)
	{
	}

	private void InvLookup_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnUpdate_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				cmbProfileName.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbProfileName.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdlookups_DoubleClick(null, null);
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

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F5", "For Focus on Qc Grade");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Item Combo in Qc Grade");
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

	private void btnshortcut_Click(object sender, EventArgs e)
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
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnshortcut = new System.Windows.Forms.ToolStripButton();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.cmbaccounttitle = new UltraCombo();
		this.cmbProfileName = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.txtLookUpName = new System.Windows.Forms.TextBox();
		this.txtCode = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdHistory = new GridEX();
		this.label5 = new System.Windows.Forms.Label();
		this.chkisactive = new System.Windows.Forms.CheckBox();
		this.toolStrip1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbaccounttitle).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbProfileName).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		base.SuspendLayout();
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnUpdate, this.btnRefresh, this.btnshortcut });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1002, 27);
		this.toolStrip1.TabIndex = 5;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnshortcut.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcut.Name = "btnshortcut";
		this.btnshortcut.Size = new System.Drawing.Size(126, 24);
		this.btnshortcut.Text = "ShortCut Keys";
		this.btnshortcut.Click += new System.EventHandler(btnshortcut_Click);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label15);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 27);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1002, 28);
		this.panel4.TabIndex = 6;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(5, 4);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(75, 19);
		this.label15.TabIndex = 1;
		this.label15.Text = "Look Up";
		this.panel1.Controls.Add(this.chkisactive);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbaccounttitle);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbProfileName);
		this.panel1.Controls.Add(this.label2);
		this.panel1.Controls.Add(this.textBox1);
		this.panel1.Controls.Add(this.txtLookUpName);
		this.panel1.Controls.Add(this.txtCode);
		this.panel1.Controls.Add(this.label6);
		this.panel1.Controls.Add(this.label4);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Controls.Add(this.label3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel1.Location = new System.Drawing.Point(0, 55);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1002, 61);
		this.panel1.TabIndex = 0;
		this.cmbaccounttitle.AllowNull = (DefaultableBoolean)1;
		this.cmbaccounttitle.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbaccounttitle.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbaccounttitle.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Appearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbaccounttitle).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance24;
		((SpecialBoxBase)((UltraGridBase)this.cmbaccounttitle).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbaccounttitle).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbaccounttitle.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbaccounttitle).Location = new System.Drawing.Point(743, 25);
		((System.Windows.Forms.Control)(object)this.cmbaccounttitle).Name = "cmbaccounttitle";
		((System.Windows.Forms.Control)(object)this.cmbaccounttitle).Size = new System.Drawing.Size(155, 26);
		((System.Windows.Forms.Control)(object)this.cmbaccounttitle).TabIndex = 4;
		this.cmbProfileName.AllowNull = (DefaultableBoolean)1;
		this.cmbProfileName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbProfileName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbProfileName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbProfileName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbProfileName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbProfileName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbProfileName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbProfileName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbProfileName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbProfileName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbProfileName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbProfileName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbProfileName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbProfileName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbProfileName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbProfileName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbProfileName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbProfileName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbProfileName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbProfileName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbProfileName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbProfileName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbProfileName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbProfileName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbProfileName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbProfileName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbProfileName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbProfileName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbProfileName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbProfileName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbProfileName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbProfileName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbProfileName).Location = new System.Drawing.Point(8, 25);
		((System.Windows.Forms.Control)(object)this.cmbProfileName).Name = "cmbProfileName";
		((System.Windows.Forms.Control)(object)this.cmbProfileName).Size = new System.Drawing.Size(154, 26);
		((System.Windows.Forms.Control)(object)this.cmbProfileName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbProfileName).Leave += new System.EventHandler(cmbProfileName_Leave);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(164, 7);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(34, 15);
		this.label2.TabIndex = 19;
		this.label2.Text = "Code";
		this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.textBox1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox1.Location = new System.Drawing.Point(504, 27);
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(237, 23);
		this.textBox1.TabIndex = 3;
		this.txtLookUpName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLookUpName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLookUpName.Location = new System.Drawing.Point(306, 27);
		this.txtLookUpName.Name = "txtLookUpName";
		this.txtLookUpName.Size = new System.Drawing.Size(196, 23);
		this.txtLookUpName.TabIndex = 2;
		this.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCode.Enabled = false;
		this.txtCode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCode.Location = new System.Drawing.Point(164, 27);
		this.txtCode.Name = "txtCode";
		this.txtCode.Size = new System.Drawing.Size(140, 23);
		this.txtCode.TabIndex = 1;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(506, 7);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(72, 15);
		this.label6.TabIndex = 18;
		this.label6.Text = "Other Name";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(743, 7);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(78, 15);
		this.label4.TabIndex = 17;
		this.label4.Text = "Account Title";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(308, 7);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(39, 15);
		this.label1.TabIndex = 18;
		this.label1.Text = "Name";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(8, 7);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(32, 15);
		this.label3.TabIndex = 17;
		this.label3.Text = "Type";
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.label5);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 116);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1002, 28);
		this.panel2.TabIndex = 8;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(956, 2);
		this.ctrlGrdBar1.MyGrid = this.grdHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(40, 26);
		this.ctrlGrdBar1.TabIndex = 11;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdHistory).BackColor = System.Drawing.Color.White;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 144);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1002, 404);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 1;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdlookups_DoubleClick);
		this.label5.AutoSize = true;
		this.label5.BackColor = System.Drawing.Color.Teal;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label5.Location = new System.Drawing.Point(5, 4);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(72, 19);
		this.label5.TabIndex = 1;
		this.label5.Text = "History ";
		this.chkisactive.AutoSize = true;
		this.chkisactive.Location = new System.Drawing.Point(903, 28);
		this.chkisactive.Name = "chkisactive";
		this.chkisactive.Size = new System.Drawing.Size(68, 20);
		this.chkisactive.TabIndex = 5;
		this.chkisactive.Text = "Active";
		this.chkisactive.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1002, 548);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel4);
		base.Controls.Add(this.toolStrip1);
		base.KeyPreview = true;
		base.Name = "InvLookup";
		this.Text = "InvLookup";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(InvLookup_FormClosed);
		base.Load += new System.EventHandler(InvLookup_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvLookup_KeyDown);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbaccounttitle).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbProfileName).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
