using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Themes_ERP;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Themes_ERP;
using Architecture.WinApp.Common;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.Themes_ERP;

public class ThemeSelectionForm : Form
{
	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private int RecId;

	private IContainer components;

	private Button btnPanelBackColor;

	private Button BtnButtonBackColor;

	private Panel PanelHeader1;

	private Label labelPanelHeaderControl1;

	private Label lbltocompany;

	internal UltraCombo CmbScreenName;

	private Panel panel1;

	private Label label3;

	private Label label2;

	private Label label1;

	private Button BtnPanelControlsBackColor;

	internal GridEX grd;

	private Button BtnSave;

	private ToolStrip tsItemType;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripButton btnRefresh;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripButton btnshortcutkeys;

	private TextBox txtPanelBackColorName;

	private TextBox txtPanelForeColorName;

	private TextBox txtPanelControlBackColorName;

	private Label label4;

	private Button btnPanelForeColor;

	private TextBox txtPanelControlForeColorName;

	private Label label5;

	private Button BtnPanelControlsForeColor;

	private TextBox txtButtonForeColorName;

	private TextBox txtButtonBackColorName;

	private Label label6;

	private Button BtnButtonForeColor;

	private ToolStripButton btnnew;

	public ThemeSelectionForm(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		UserAccount = user;
		InitializeComponent();
		base.Load += ThemeSelectionForm_Load;
	}

	private void btnPanelBackColor_Click(object sender, EventArgs e)
	{
		try
		{
			Color color = ColorSelection();
			txtPanelBackColorName.Text = color.Name;
			txtPanelBackColorName.ForeColor = color;
			PanelHeader1.BackColor = color;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPanelForeColor_Click(object sender, EventArgs e)
	{
		try
		{
			Color color = ColorSelection();
			txtPanelForeColorName.Text = color.Name;
			txtPanelForeColorName.ForeColor = color;
			PanelHeader1.ForeColor = color;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private Color ColorSelection()
	{
		Color color = Color.Transparent;
		try
		{
			using ColorDialog colorDialog = new ColorDialog();
			if (colorDialog.ShowDialog() == DialogResult.OK)
			{
				color = colorDialog.Color;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return color;
	}

	private void BtnPanelControlsBackColor_Click(object sender, EventArgs e)
	{
		try
		{
			Color color = ColorSelection();
			txtPanelControlBackColorName.Text = color.Name;
			txtPanelControlBackColorName.ForeColor = color;
			labelPanelHeaderControl1.BackColor = color;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPanelControlsForeColor_Click(object sender, EventArgs e)
	{
		try
		{
			Color color = ColorSelection();
			txtPanelControlForeColorName.Text = color.Name;
			txtPanelControlForeColorName.ForeColor = color;
			labelPanelHeaderControl1.ForeColor = color;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnButtonBackColor_Click(object sender, EventArgs e)
	{
		try
		{
			Color color = ColorSelection();
			txtButtonBackColorName.Text = color.Name;
			txtButtonBackColorName.ForeColor = color;
			BtnSave.BackColor = color;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnButtonForeColor_Click(object sender, EventArgs e)
	{
		try
		{
			Color color = ColorSelection();
			txtButtonForeColorName.Text = color.Name;
			txtButtonForeColorName.ForeColor = color;
			BtnSave.ForeColor = color;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPanelColor_MouseDown(object sender, MouseEventArgs e)
	{
		try
		{
			if (e.Button != MouseButtons.Right)
			{
				return;
			}
			using ColorDialog colorDialog = new ColorDialog();
			if (colorDialog.ShowDialog() == DialogResult.OK)
			{
				btnPanelBackColor.BackColor = colorDialog.Color;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnButtonColor_MouseDown(object sender, MouseEventArgs e)
	{
		try
		{
			foreach (Control control in base.Controls)
			{
				if (control is Button btn)
				{
					btn.MouseDown += btnButtonColor_MouseDown;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ThemeSelectionForm_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1050, 500);
			BtnSave.Visible = true;
			BtnSave.Text = "Save";
			CmbScreenName.Focus();
			ScreenNamefill();
			FormHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ScreenNamefill()
	{
		try
		{
			int id = Conversion.ToInt(CmbScreenName.Value);
			DataTable dataTable = ThemeSettings.GetScreensForTheme(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (dataTable.Rows.Count > 0)
			{
				DDL.BindDDLNew(dataTable, CmbScreenName, "ScreenId", "ScreenName", "Screen Name", false);
				((UltraGridBase)CmbScreenName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (id > 0)
			{
				if (dataTable.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					CmbScreenName.Value = id;
				}
				else
				{
					((Control)(object)CmbScreenName).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (string.IsNullOrEmpty(((Control)(object)CmbScreenName).Text))
		{
			MessageBox.Show("Screen Name Field is Required");
			CmbScreenName.Focus();
			return false;
		}
		if (string.IsNullOrEmpty(txtPanelBackColorName.Text))
		{
			MessageBox.Show("Panel Back Color Name Field is Required");
			txtPanelBackColorName.Focus();
			return false;
		}
		if (string.IsNullOrEmpty(txtPanelForeColorName.Text))
		{
			MessageBox.Show("Panel Font Color Name Field is Required");
			txtPanelForeColorName.Focus();
			return false;
		}
		if (string.IsNullOrEmpty(txtPanelControlBackColorName.Text))
		{
			MessageBox.Show("Panel's Control Back Color Name Field is Required");
			txtPanelControlBackColorName.Focus();
			return false;
		}
		if (string.IsNullOrEmpty(txtPanelControlForeColorName.Text))
		{
			MessageBox.Show("Panel's Control Font Color Name Field is Required");
			txtPanelControlForeColorName.Focus();
			return false;
		}
		if (string.IsNullOrEmpty(txtButtonBackColorName.Text))
		{
			MessageBox.Show("Button Back Color Name Field is Required");
			txtButtonBackColorName.Focus();
			return false;
		}
		if (string.IsNullOrEmpty(txtButtonForeColorName.Text))
		{
			MessageBox.Show("Button Font Color Name Field is Required");
			txtButtonForeColorName.Focus();
			return false;
		}
		return true;
	}

	private void BtnSave_Click(object sender, EventArgs e)
	{
		try
		{
			if (BtnSave.Text == "Save")
			{
				RecId = 0;
				Insert();
			}
			else if (BtnSave.Text == "Update")
			{
				if (RecId == 0)
				{
					throw new Exception("RecId not found");
				}
				Insert();
			}
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
			ThemeSettings obj = new ThemeSettings();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				obj.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.EntryUserId = UserAccount.EntryUserId;
			obj.EntryDate = DateTime.Now;
			obj.ModifyUserId = UserAccount.ModifyUserId;
			obj.ModifyDate = DateTime.Now;
			obj.ScreenId = Conversion.ToInt(CmbScreenName.Value);
			obj.ScreenName = Conversion.ToString((object)((Control)(object)CmbScreenName).Text);
			obj.HeaderPanel_BackColor = Conversion.ToString((object)txtPanelBackColorName.Text);
			obj.HeaderPanel_FontColor = Conversion.ToString((object)txtPanelForeColorName.Text);
			obj.HeaderPanel_ControlsBackColor = Conversion.ToString((object)txtPanelControlBackColorName.Text);
			obj.HeaderPanel_ControlsFontColor = Conversion.ToString((object)txtPanelControlForeColorName.Text);
			obj.Button_BackColor = Conversion.ToString((object)txtButtonBackColorName.Text);
			obj.Button_FontColor = Conversion.ToString((object)txtButtonForeColorName.Text);
			ThemeSettings.Save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update Successfully");
			}
			else
			{
				MessageBox.Show("Record Save Successfully");
			}
			Reset();
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
			((Control)(object)CmbScreenName).Text = string.Empty;
			CmbScreenName.Focus();
			BtnSave.Visible = true;
			BtnSave.Text = "Save";
			FormHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnReset_Click(object sender, EventArgs e)
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

	private void ThemeSelectionForm_FormClosed(object sender, FormClosedEventArgs e)
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			ScreenNamefill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FormHistory()
	{
		try
		{
			DataTable dataTable = ThemeSettings.GetThemeSettingsByScreen(UserAccount.OrganizationId, UserAccount.CompanyId, 0, (string)null);
			if (dataTable.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("ScreenName", typeof(string));
			dt.Columns.Add("HeaderPanel_BackColor", typeof(string));
			dt.Columns.Add("HeaderPanel_FontColor", typeof(string));
			dt.Columns.Add("HeaderPanel_ControlsBackColor", typeof(string));
			dt.Columns.Add("HeaderPanel_ControlsFontColor", typeof(string));
			dt.Columns.Add("Button_BackColor", typeof(string));
			dt.Columns.Add("Button_FontColor", typeof(string));
			dt.Clear();
			foreach (DataRow row in dataTable.Rows)
			{
				dt.Rows.Add(Conversion.ToInt(row["ScreenId"]), Conversion.ToString(row["ScreenName"]), Conversion.ToString(row["HeaderPanel_BackColor"]), Conversion.ToString(row["HeaderPanel_FontColor"]), Conversion.ToString(row["HeaderPanel_ControlsBackColor"]), Conversion.ToString(row["HeaderPanel_ControlsFontColor"]), Conversion.ToString(row["Button_BackColor"]), Conversion.ToString(row["Button_FontColor"]));
			}
			grd.DataSource = dt;
			grd.RetrieveStructure();
			GrdSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private string GetColorName(string hexColor)
	{
		try
		{
			Color color = ColorTranslator.FromHtml(hexColor);
			return color.IsKnownColor ? color.Name : "Unknown Color";
		}
		catch
		{
			return "Unknown Color";
		}
	}

	private void GrdSetting()
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.HeaderLines = 3;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
				item.Width = 100;
			}
			grd.RootTable.Columns["ScreenName"].Width = 200;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				BtnSave.Text = "Update";
				RecId = Conversion.ToInt(r.Cells["Id"].Value);
				((Control)(object)CmbScreenName).Text = Conversion.ToString(r.Cells["ScreenName"].Value);
				txtPanelBackColorName.Text = Conversion.ToString(r.Cells["HeaderPanel_BackColor"].Value);
				txtPanelForeColorName.Text = Conversion.ToString(r.Cells["HeaderPanel_FontColor"].Value);
				txtPanelControlBackColorName.Text = Conversion.ToString(r.Cells["HeaderPanel_ControlsBackColor"].Value);
				txtPanelControlForeColorName.Text = Conversion.ToString(r.Cells["HeaderPanel_ControlsFontColor"].Value);
				txtButtonBackColorName.Text = Conversion.ToString(r.Cells["Button_BackColor"].Value);
				txtButtonForeColorName.Text = Conversion.ToString(r.Cells["Button_FontColor"].Value);
				PanelHeader1.BackColor = ColorTranslator.FromHtml(Conversion.ToString(r.Cells["HeaderPanel_BackColor"].Value));
				PanelHeader1.ForeColor = ColorTranslator.FromHtml(Conversion.ToString(r.Cells["HeaderPanel_FontColor"].Value));
				labelPanelHeaderControl1.BackColor = ColorTranslator.FromHtml(Conversion.ToString(r.Cells["HeaderPanel_ControlsBackColor"].Value));
				labelPanelHeaderControl1.ForeColor = ColorTranslator.FromHtml(Conversion.ToString(r.Cells["HeaderPanel_ControlsFontColor"].Value));
				BtnSave.BackColor = ColorTranslator.FromHtml(Conversion.ToString(r.Cells["Button_BackColor"].Value));
				BtnSave.ForeColor = ColorTranslator.FromHtml(Conversion.ToString(r.Cells["Button_FontColor"].Value));
				CmbScreenName.Focus();
			}
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

	private void WorkStationOperation_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.Control && e.KeyCode == Keys.N)
			{
				BtnReset_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				BtnSave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.U)
			{
				BtnSave_Click(null, null);
			}
			else if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			else if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.F5)
			{
				CmbScreenName.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Up)
			{
				CmbScreenName.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
			}
			else if (e.Control && e.Alt)
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F5", "For Focus On Screen Name");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Screen Name");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Expected O, but got Unknown
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Themes_ERP.ThemeSelectionForm));
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		this.BtnButtonBackColor = new System.Windows.Forms.Button();
		this.PanelHeader1 = new System.Windows.Forms.Panel();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.labelPanelHeaderControl1 = new System.Windows.Forms.Label();
		this.lbltocompany = new System.Windows.Forms.Label();
		this.CmbScreenName = new UltraCombo();
		this.btnPanelBackColor = new System.Windows.Forms.Button();
		this.panel1 = new System.Windows.Forms.Panel();
		this.txtPanelForeColorName = new System.Windows.Forms.TextBox();
		this.txtButtonForeColorName = new System.Windows.Forms.TextBox();
		this.BtnSave = new System.Windows.Forms.Button();
		this.txtButtonBackColorName = new System.Windows.Forms.TextBox();
		this.txtPanelControlForeColorName = new System.Windows.Forms.TextBox();
		this.txtPanelControlBackColorName = new System.Windows.Forms.TextBox();
		this.txtPanelBackColorName = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.BtnButtonForeColor = new System.Windows.Forms.Button();
		this.BtnPanelControlsForeColor = new System.Windows.Forms.Button();
		this.btnPanelForeColor = new System.Windows.Forms.Button();
		this.BtnPanelControlsBackColor = new System.Windows.Forms.Button();
		this.grd = new GridEX();
		this.PanelHeader1.SuspendLayout();
		this.tsItemType.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbScreenName).BeginInit();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		base.SuspendLayout();
		this.BtnButtonBackColor.BackColor = System.Drawing.Color.Transparent;
		this.BtnButtonBackColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnButtonBackColor.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);
		this.BtnButtonBackColor.ForeColor = System.Drawing.Color.White;
		this.BtnButtonBackColor.Image = (System.Drawing.Image)resources.GetObject("BtnButtonBackColor.Image");
		this.BtnButtonBackColor.Location = new System.Drawing.Point(1033, 3);
		this.BtnButtonBackColor.Name = "BtnButtonBackColor";
		this.BtnButtonBackColor.Size = new System.Drawing.Size(33, 33);
		this.BtnButtonBackColor.TabIndex = 10;
		this.BtnButtonBackColor.UseVisualStyleBackColor = false;
		this.BtnButtonBackColor.Click += new System.EventHandler(BtnButtonBackColor_Click);
		this.PanelHeader1.BackColor = System.Drawing.Color.Teal;
		this.PanelHeader1.Controls.Add(this.tsItemType);
		this.PanelHeader1.Controls.Add(this.labelPanelHeaderControl1);
		this.PanelHeader1.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanelHeader1.Location = new System.Drawing.Point(0, 0);
		this.PanelHeader1.Name = "PanelHeader1";
		this.PanelHeader1.Size = new System.Drawing.Size(1074, 53);
		this.PanelHeader1.TabIndex = 46;
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.toolStripSeparator5, this.btnnew, this.btnRefresh, this.toolStripSeparator4, this.btnshortcutkeys });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(1074, 27);
		this.tsItemType.TabIndex = 3;
		this.tsItemType.Text = "toolStrip4";
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(BtnReset_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = (System.Drawing.Image)resources.GetObject("btnRefresh.Image");
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = (System.Drawing.Image)resources.GetObject("btnshortcutkeys.Image");
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.labelPanelHeaderControl1.AutoSize = true;
		this.labelPanelHeaderControl1.BackColor = System.Drawing.Color.Teal;
		this.labelPanelHeaderControl1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelPanelHeaderControl1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.labelPanelHeaderControl1.Location = new System.Drawing.Point(5, 29);
		this.labelPanelHeaderControl1.Name = "labelPanelHeaderControl1";
		this.labelPanelHeaderControl1.Size = new System.Drawing.Size(118, 19);
		this.labelPanelHeaderControl1.TabIndex = 0;
		this.labelPanelHeaderControl1.Text = "Color Change";
		this.lbltocompany.AutoSize = true;
		this.lbltocompany.BackColor = System.Drawing.Color.White;
		this.lbltocompany.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.lbltocompany.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbltocompany.Location = new System.Drawing.Point(7, 12);
		this.lbltocompany.Name = "lbltocompany";
		this.lbltocompany.Size = new System.Drawing.Size(78, 15);
		this.lbltocompany.TabIndex = 48;
		this.lbltocompany.Text = "Screen Name";
		this.CmbScreenName.AllowNull = (DefaultableBoolean)1;
		this.CmbScreenName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbScreenName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbScreenName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbScreenName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.CmbScreenName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbScreenName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbScreenName).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbScreenName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbScreenName).Location = new System.Drawing.Point(6, 38);
		((System.Windows.Forms.Control)(object)this.CmbScreenName).Name = "CmbScreenName";
		((System.Windows.Forms.Control)(object)this.CmbScreenName).Size = new System.Drawing.Size(215, 26);
		((System.Windows.Forms.Control)(object)this.CmbScreenName).TabIndex = 0;
		this.btnPanelBackColor.BackColor = System.Drawing.Color.Transparent;
		this.btnPanelBackColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnPanelBackColor.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPanelBackColor.ForeColor = System.Drawing.Color.White;
		this.btnPanelBackColor.Image = (System.Drawing.Image)resources.GetObject("btnPanelBackColor.Image");
		this.btnPanelBackColor.Location = new System.Drawing.Point(444, 3);
		this.btnPanelBackColor.Name = "btnPanelBackColor";
		this.btnPanelBackColor.Size = new System.Drawing.Size(33, 33);
		this.btnPanelBackColor.TabIndex = 2;
		this.btnPanelBackColor.UseVisualStyleBackColor = false;
		this.btnPanelBackColor.Click += new System.EventHandler(btnPanelBackColor_Click);
		this.panel1.Controls.Add(this.txtPanelForeColorName);
		this.panel1.Controls.Add(this.txtButtonForeColorName);
		this.panel1.Controls.Add(this.BtnSave);
		this.panel1.Controls.Add(this.txtButtonBackColorName);
		this.panel1.Controls.Add(this.txtPanelControlForeColorName);
		this.panel1.Controls.Add(this.txtPanelControlBackColorName);
		this.panel1.Controls.Add(this.txtPanelBackColorName);
		this.panel1.Controls.Add(this.label6);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbScreenName);
		this.panel1.Controls.Add(this.label3);
		this.panel1.Controls.Add(this.label5);
		this.panel1.Controls.Add(this.label4);
		this.panel1.Controls.Add(this.label2);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Controls.Add(this.BtnButtonForeColor);
		this.panel1.Controls.Add(this.lbltocompany);
		this.panel1.Controls.Add(this.BtnButtonBackColor);
		this.panel1.Controls.Add(this.BtnPanelControlsForeColor);
		this.panel1.Controls.Add(this.btnPanelForeColor);
		this.panel1.Controls.Add(this.BtnPanelControlsBackColor);
		this.panel1.Controls.Add(this.btnPanelBackColor);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 53);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1074, 106);
		this.panel1.TabIndex = 0;
		this.txtPanelForeColorName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPanelForeColorName.Enabled = false;
		this.txtPanelForeColorName.Font = new System.Drawing.Font("Verdana", 10.25f);
		this.txtPanelForeColorName.Location = new System.Drawing.Point(292, 40);
		this.txtPanelForeColorName.Name = "txtPanelForeColorName";
		this.txtPanelForeColorName.Size = new System.Drawing.Size(149, 24);
		this.txtPanelForeColorName.TabIndex = 3;
		this.txtButtonForeColorName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtButtonForeColorName.Enabled = false;
		this.txtButtonForeColorName.Font = new System.Drawing.Font("Verdana", 10.25f);
		this.txtButtonForeColorName.Location = new System.Drawing.Point(880, 40);
		this.txtButtonForeColorName.Name = "txtButtonForeColorName";
		this.txtButtonForeColorName.Size = new System.Drawing.Size(149, 24);
		this.txtButtonForeColorName.TabIndex = 11;
		this.BtnSave.BackColor = System.Drawing.Color.Teal;
		this.BtnSave.FlatAppearance.BorderSize = 0;
		this.BtnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnSave.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnSave.ForeColor = System.Drawing.Color.White;
		this.BtnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnSave.Location = new System.Drawing.Point(984, 74);
		this.BtnSave.Margin = new System.Windows.Forms.Padding(2);
		this.BtnSave.Name = "BtnSave";
		this.BtnSave.Size = new System.Drawing.Size(79, 24);
		this.BtnSave.TabIndex = 13;
		this.BtnSave.Text = "Save";
		this.BtnSave.UseVisualStyleBackColor = false;
		this.BtnSave.Click += new System.EventHandler(BtnSave_Click);
		this.txtButtonBackColorName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtButtonBackColorName.Enabled = false;
		this.txtButtonBackColorName.Font = new System.Drawing.Font("Verdana", 10.25f);
		this.txtButtonBackColorName.Location = new System.Drawing.Point(880, 7);
		this.txtButtonBackColorName.Name = "txtButtonBackColorName";
		this.txtButtonBackColorName.Size = new System.Drawing.Size(149, 24);
		this.txtButtonBackColorName.TabIndex = 9;
		this.txtPanelControlForeColorName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPanelControlForeColorName.Enabled = false;
		this.txtPanelControlForeColorName.Font = new System.Drawing.Font("Verdana", 10.25f);
		this.txtPanelControlForeColorName.Location = new System.Drawing.Point(604, 40);
		this.txtPanelControlForeColorName.Name = "txtPanelControlForeColorName";
		this.txtPanelControlForeColorName.Size = new System.Drawing.Size(149, 24);
		this.txtPanelControlForeColorName.TabIndex = 7;
		this.txtPanelControlBackColorName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPanelControlBackColorName.Enabled = false;
		this.txtPanelControlBackColorName.Font = new System.Drawing.Font("Verdana", 10.25f);
		this.txtPanelControlBackColorName.Location = new System.Drawing.Point(604, 7);
		this.txtPanelControlBackColorName.Name = "txtPanelControlBackColorName";
		this.txtPanelControlBackColorName.Size = new System.Drawing.Size(149, 24);
		this.txtPanelControlBackColorName.TabIndex = 5;
		this.txtPanelBackColorName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPanelBackColorName.Enabled = false;
		this.txtPanelBackColorName.Font = new System.Drawing.Font("Verdana", 10.25f);
		this.txtPanelBackColorName.Location = new System.Drawing.Point(292, 7);
		this.txtPanelBackColorName.Name = "txtPanelBackColorName";
		this.txtPanelBackColorName.Size = new System.Drawing.Size(149, 24);
		this.txtPanelBackColorName.TabIndex = 1;
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.White;
		this.label6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(798, 45);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(75, 15);
		this.label6.TabIndex = 48;
		this.label6.Text = "Buttons Font";
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.White;
		this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(798, 12);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(76, 15);
		this.label3.TabIndex = 48;
		this.label3.Text = "Buttons Back";
		this.label5.AutoSize = true;
		this.label5.BackColor = System.Drawing.Color.White;
		this.label5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(484, 45);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(118, 15);
		this.label5.TabIndex = 48;
		this.label5.Text = "Panel's Controls Font";
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.White;
		this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(224, 45);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(63, 15);
		this.label4.TabIndex = 48;
		this.label4.Text = "Panel Font";
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.White;
		this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(484, 12);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(119, 15);
		this.label2.TabIndex = 48;
		this.label2.Text = "Panel's Controls Back";
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.White;
		this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(224, 12);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(64, 15);
		this.label1.TabIndex = 48;
		this.label1.Text = "Panel Back";
		this.BtnButtonForeColor.BackColor = System.Drawing.Color.Transparent;
		this.BtnButtonForeColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnButtonForeColor.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);
		this.BtnButtonForeColor.ForeColor = System.Drawing.Color.White;
		this.BtnButtonForeColor.Image = (System.Drawing.Image)resources.GetObject("BtnButtonForeColor.Image");
		this.BtnButtonForeColor.Location = new System.Drawing.Point(1033, 36);
		this.BtnButtonForeColor.Name = "BtnButtonForeColor";
		this.BtnButtonForeColor.Size = new System.Drawing.Size(33, 33);
		this.BtnButtonForeColor.TabIndex = 12;
		this.BtnButtonForeColor.UseVisualStyleBackColor = false;
		this.BtnButtonForeColor.Click += new System.EventHandler(BtnButtonForeColor_Click);
		this.BtnPanelControlsForeColor.BackColor = System.Drawing.Color.Transparent;
		this.BtnPanelControlsForeColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnPanelControlsForeColor.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPanelControlsForeColor.ForeColor = System.Drawing.Color.White;
		this.BtnPanelControlsForeColor.Image = (System.Drawing.Image)resources.GetObject("BtnPanelControlsForeColor.Image");
		this.BtnPanelControlsForeColor.Location = new System.Drawing.Point(759, 36);
		this.BtnPanelControlsForeColor.Name = "BtnPanelControlsForeColor";
		this.BtnPanelControlsForeColor.Size = new System.Drawing.Size(33, 33);
		this.BtnPanelControlsForeColor.TabIndex = 8;
		this.BtnPanelControlsForeColor.UseVisualStyleBackColor = false;
		this.BtnPanelControlsForeColor.Click += new System.EventHandler(BtnPanelControlsForeColor_Click);
		this.btnPanelForeColor.BackColor = System.Drawing.Color.Transparent;
		this.btnPanelForeColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnPanelForeColor.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPanelForeColor.ForeColor = System.Drawing.Color.White;
		this.btnPanelForeColor.Image = (System.Drawing.Image)resources.GetObject("btnPanelForeColor.Image");
		this.btnPanelForeColor.Location = new System.Drawing.Point(444, 36);
		this.btnPanelForeColor.Name = "btnPanelForeColor";
		this.btnPanelForeColor.Size = new System.Drawing.Size(33, 33);
		this.btnPanelForeColor.TabIndex = 4;
		this.btnPanelForeColor.UseVisualStyleBackColor = false;
		this.btnPanelForeColor.Click += new System.EventHandler(btnPanelForeColor_Click);
		this.BtnPanelControlsBackColor.BackColor = System.Drawing.Color.Transparent;
		this.BtnPanelControlsBackColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnPanelControlsBackColor.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPanelControlsBackColor.ForeColor = System.Drawing.Color.White;
		this.BtnPanelControlsBackColor.Image = (System.Drawing.Image)resources.GetObject("BtnPanelControlsBackColor.Image");
		this.BtnPanelControlsBackColor.Location = new System.Drawing.Point(759, 3);
		this.BtnPanelControlsBackColor.Name = "BtnPanelControlsBackColor";
		this.BtnPanelControlsBackColor.Size = new System.Drawing.Size(33, 33);
		this.BtnPanelControlsBackColor.TabIndex = 6;
		this.BtnPanelControlsBackColor.UseVisualStyleBackColor = false;
		this.BtnPanelControlsBackColor.Click += new System.EventHandler(BtnPanelControlsBackColor_Click);
		this.grd.AllowEdit = (InheritableBoolean)2;
		this.grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		this.grd.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 159);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grd).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1074, 402);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.grd).Tag = "UnAllocatedGrid";
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1074, 561);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.PanelHeader1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "ThemeSelectionForm";
		this.Text = "Theme Selection";
		this.PanelHeader1.ResumeLayout(false);
		this.PanelHeader1.PerformLayout();
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbScreenName).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		base.ResumeLayout(false);
	}
}
