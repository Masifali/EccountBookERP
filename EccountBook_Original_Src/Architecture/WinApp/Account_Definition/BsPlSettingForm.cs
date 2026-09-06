using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Definition;

public class BsPlSettingForm : Form
{
	private DataTable table = new DataTable();

	private DataTable tableNote = new DataTable();

	private DataTable dtchartofaccount = new DataTable();

	private DataTable dtNoteTile = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private ToolStripButton btnnew;

	private Panel panel1;

	private Panel panel3;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnUpdate;

	private Label label23;

	private GroupBox groupBox1;

	private RadioButton radBS;

	private RadioButton radPL;

	private Button btnShow;

	private RadioButton radLiablities;

	private RadioButton radAssets;

	private Panel panel6;

	private Panel panel5;

	private CtrlGrdBar ctrlGrdBar2;

	private TabControl tabControl1;

	private TabPage tabPage1;

	internal GridEX grdBsPl;

	private Panel panel4;

	private TabPage tabPage2;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdNoteChange;

	private Panel panel7;

	private Panel panel8;

	private GroupBox groupBox2;

	private Panel panel10;

	private RadioButton radAllNote;

	private RadioButton radBSNote;

	private RadioButton radPLNote;

	private Button btnShowNote;

	private Panel panel11;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewNote;

	private ToolStripButton btnUpdateNote;

	private Label label1;

	private Label label2;

	private Label label3;

	private ToolStripButton BtnShortCutkeys;

	private ToolStripButton btnSkey;

	public BsPlSettingForm(UserAccount user)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void BsPlSettingForm_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 700);
			table.Columns.Add("PlBsId");
			table.Columns.Add("ChartofAccountId");
			table.Columns.Add("AccountTitle");
			table.Columns.Add("NoteTitle");
			table.Columns.Add("NoteRole");
			table.Columns.Add("AccountClassId");
			table.Columns.Add("ClassName");
			tableNote.Columns.Add("Id");
			tableNote.Columns.Add("NoteTitle");
			tableNote.Columns.Add("NoteRole");
			tableNote.Columns.Add("AccountClassId");
			tableNote.Columns.Add("ClassName");
			grdNoteChange.DataSource = tableNote;
			grdNoteChange.RetrieveStructure();
			GridNoteSettings();
			radPL.Focus();
			radAssets.Visible = false;
			radLiablities.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DefineCity_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnnew_Click(null, null);
				}
				else
				{
					btnNewNote_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnShow_Click(null, null);
				}
				else
				{
					btnShowNote_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.U)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnUpdate_Click(null, null);
				}
				else
				{
					btnUpdateNote_Click(null, null);
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					tabControl1.SelectedIndex = 1;
				}
				else
				{
					tabControl1.SelectedIndex = 0;
				}
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					radPL.Focus();
				}
				else
				{
					radAllNote.Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					radPL.Focus();
				}
				else
				{
					radAllNote.Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					((Control)(object)grdBsPl).Focus();
				}
				else
				{
					((Control)(object)grdNoteChange).Focus();
				}
			}
			if (e.Control && e.Alt)
			{
				BtnShortCutkeys_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShortCutkeys_Click(object sender, EventArgs e)
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

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Show Grid Data");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New");
			if (tabControl1.SelectedIndex == 0)
			{
				dt.Rows.Add("Ctrl+F5", "For Focus on Radio Button PL");
			}
			else
			{
				dt.Rows.Add("Ctrl+F5", "For Focus on Radio Button All");
			}
			if (tabControl1.SelectedIndex == 0)
			{
				dt.Rows.Add("Ctrl+ArrowUp", "For Focus on Radio Button PL");
			}
			else
			{
				dt.Rows.Add("Ctrl+ArrowUp", "For Focus on Radio Button All");
			}
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void radPL_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (radBS.Checked)
			{
				radAssets.Visible = true;
				radAssets.Checked = true;
				radLiablities.Visible = true;
			}
			else
			{
				radAssets.Visible = false;
				radLiablities.Visible = false;
				radAssets.Checked = false;
				radLiablities.Checked = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			GridLoad();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridLoad()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		try
		{
			if (radBS.Checked && !radLiablities.Checked && !radAssets.Checked)
			{
				MessageBox.Show("Please select either the 'Assets' or 'Liabilities' button...");
				radAssets.Focus();
				return;
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			if (radAssets.Visible && radLiablities.Visible)
			{
				if (radAssets.Checked)
				{
					obj.AccouuntClassId = 2;
				}
				else
				{
					obj.AccouuntClassId = 3;
				}
			}
			if (radBS.Checked)
			{
				obj.ReqType = "BS";
			}
			else
			{
				obj.ReqType = "PL";
			}
			dtchartofaccount = PLBSSetting.GetAllForBsPlSetting(obj);
			if (dtchartofaccount.Rows.Count > 0)
			{
				table.Rows.Clear();
				for (int i = 0; i < dtchartofaccount.Rows.Count; i++)
				{
					table.Rows.Add(dtchartofaccount.Rows[i]["PlBsId"], dtchartofaccount.Rows[i]["Id"], dtchartofaccount.Rows[i]["AccountTitle"], dtchartofaccount.Rows[i]["NoteTitle"], dtchartofaccount.Rows[i]["NoteRole"], dtchartofaccount.Rows[i]["AccountClass"], dtchartofaccount.Rows[i]["ClassName"]);
				}
				grdBsPl.DataSource = table;
				grdBsPl.RetrieveStructure();
				GridSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettings()
	{
		try
		{
			DataTable dtPLNotes = new DataTable();
			DataTable dtBSNotes = new DataTable();
			if (radAssets.Checked)
			{
				dtBSNotes = AccountNotes.ReadByBSNote(2);
			}
			else if (radLiablities.Checked)
			{
				dtBSNotes = AccountNotes.ReadByBSNote(3);
			}
			dtPLNotes = AccountNotes.ReadByPLNote();
			grdBsPl.RootTable.Columns["ChartofAccountId"].Visible = false;
			grdBsPl.RootTable.Columns["PlBsId"].Visible = false;
			grdBsPl.RootTable.Columns["AccountClassId"].Visible = false;
			grdBsPl.RootTable.Columns["NoteTitle"].FilterEditType = (FilterEditType)1;
			grdBsPl.FilterRowButtonStyle = (FilterRowButtonStyle)3;
			grdBsPl.DefaultFilterRowComparison = (FilterConditionOperator)7;
			grdBsPl.RootTable.Columns["AccountTitle"].EditType = (EditType)0;
			grdBsPl.RootTable.Columns["AccountClassId"].EditType = (EditType)0;
			grdBsPl.RootTable.Columns["ClassName"].EditType = (EditType)0;
			grdBsPl.RootTable.Columns["NoteRole"].EditType = (EditType)0;
			grdBsPl.RootTable.Columns["PlBsId"].EditType = (EditType)0;
			grdBsPl.RootTable.Columns["ChartofAccountId"].EditType = (EditType)0;
			CommonServices.GridAutoAdjustmentNew(grdBsPl);
			if (radPL.Checked)
			{
				grdBsPl.RootTable.Columns["NoteTitle"].ColumnType = (ColumnType)0;
				grdBsPl.RootTable.Columns["NoteTitle"].HasValueList = true;
				grdBsPl.RootTable.Columns["NoteTitle"].LimitToList = true;
				grdBsPl.RootTable.Columns["NoteTitle"].EditType = (EditType)4;
				grdBsPl.RootTable.Columns["NoteTitle"].Caption = "PLNotes";
				grdBsPl.RootTable.Columns["NoteTitle"].ValueList.PopulateValueList((IEnumerable)dtPLNotes.DefaultView, "Id", "NoteTitle");
				grdBsPl.RootTable.Columns["NoteTitle"].Width = 250;
			}
			else
			{
				grdBsPl.RootTable.Columns["NoteTitle"].ColumnType = (ColumnType)0;
				grdBsPl.RootTable.Columns["NoteTitle"].HasValueList = true;
				grdBsPl.RootTable.Columns["NoteTitle"].LimitToList = true;
				grdBsPl.RootTable.Columns["NoteTitle"].EditType = (EditType)4;
				grdBsPl.RootTable.Columns["NoteTitle"].Caption = "BSNotes";
				grdBsPl.RootTable.Columns["NoteTitle"].ValueList.PopulateValueList((IEnumerable)dtBSNotes.DefaultView, "Id", "NoteTitle");
				grdBsPl.RootTable.Columns["NoteTitle"].Width = 250;
			}
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdBsPl_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			_ = grdBsPl.CurrentRow;
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
			radPL.Focus();
			GridLoad();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			PLBSSetting obj = new PLBSSetting();
			obj.PLBSSettingslist = new List<PLBSSetting>();
			for (int i = 0; i < dtchartofaccount.Rows.Count; i++)
			{
				if (Conversion.ToInt(table.Rows[i]["NoteTitle"]) != 0 && Conversion.ToInt(table.Rows[i]["NoteTitle"]) != Conversion.ToInt(dtchartofaccount.Rows[i]["NoteTitle"]))
				{
					PLBSSetting data = new PLBSSetting();
					data.ChartOfAccountId = Conversion.ToInt(table.Rows[i]["ChartofAccountId"]);
					data.EntryDate = DateTime.Now;
					data.ModifyDate = DateTime.Now;
					if (radBS.Checked)
					{
						data.BSNoteId = Conversion.ToInt(table.Rows[i]["NoteTitle"]);
						data.PLNoteId = 0;
					}
					else if (radPL.Checked)
					{
						data.BSNoteId = 0;
						data.PLNoteId = Conversion.ToInt(table.Rows[i]["NoteTitle"]);
					}
					data.CompanyId = UserAccount.CompanyId;
					data.OrganizationId = UserAccount.OrganizationId;
					data.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
					data.Id = 0;
					data.EntryUserId = UserAccount.EntryUserId;
					data.ModifyUserId = UserAccount.ModifyUserId;
					obj.PLBSSettingslist.Add(data);
				}
			}
			if (obj.PLBSSettingslist.Count > 0)
			{
				PLBSSetting.Save(obj);
				MessageBox.Show("Receord Update Successfully");
			}
			GridLoad();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdBsPl).Name);
			if (fs != null)
			{
				grdBsPl.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnNewNote_Click(object sender, EventArgs e)
	{
		try
		{
			radPLNote.Focus();
			GridNoteTitleFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShowNote_Click(object sender, EventArgs e)
	{
		try
		{
			GridNoteTitleFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridNoteTitleFill()
	{
		try
		{
			string Note = "";
			if (radBSNote.Checked)
			{
				Note = "BS";
			}
			else if (radPLNote.Checked)
			{
				Note = "PL";
			}
			dtNoteTile = AccountNotes.ReadAllNotes(Note);
			if (dtNoteTile.Rows.Count > 0)
			{
				tableNote.Clear();
				for (int i = 0; i < dtNoteTile.Rows.Count; i++)
				{
					tableNote.Rows.Add(dtNoteTile.Rows[i]["Id"], dtNoteTile.Rows[i]["NoteTitle"], dtNoteTile.Rows[i]["NoteRole"], dtNoteTile.Rows[i]["AccountClass"], dtNoteTile.Rows[i]["ClassName"]);
				}
				grdNoteChange.DataSource = tableNote;
				grdNoteChange.RetrieveStructure();
				GridNoteSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridNoteSettings()
	{
		try
		{
			grdNoteChange.RootTable.Columns["Id"].Visible = false;
			grdNoteChange.RootTable.Columns["AccountClassId"].Visible = false;
			grdNoteChange.RootTable.Columns["Id"].EditType = (EditType)0;
			grdNoteChange.RootTable.Columns["NoteRole"].EditType = (EditType)0;
			grdNoteChange.RootTable.Columns["AccountClassId"].EditType = (EditType)0;
			grdNoteChange.RootTable.Columns["ClassName"].EditType = (EditType)0;
			CommonServices.GridAutoAdjustmentNew(grdNoteChange);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdNoteChange).Name);
			if (fs != null)
			{
				grdNoteChange.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnUpdateNote_Click(object sender, EventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			AccountNotes obj = new AccountNotes();
			obj.AccountNoteslist = new List<AccountNotes>();
			for (int i = 0; i < dtNoteTile.Rows.Count; i++)
			{
				if (Conversion.ToInt(tableNote.Rows[i]["Id"]) != 0 && Conversion.ToString(tableNote.Rows[i]["NoteTitle"]) != Conversion.ToString(dtNoteTile.Rows[i]["NoteTitle"]))
				{
					AccountNotes data = new AccountNotes();
					data.Id = Conversion.ToInt(tableNote.Rows[i]["Id"]);
					data.NoteTitle = Conversion.ToString(tableNote.Rows[i]["NoteTitle"]);
					obj.AccountNoteslist.Add(data);
				}
			}
			if (obj.AccountNoteslist.Count > 0)
			{
				AccountNotes.UpdateTitleById(obj);
				MessageBox.Show("Receord Update Successfully");
			}
			GridNoteTitleFill();
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
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		//IL_0f11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1b: Expected O, but got Unknown
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.panel6 = new System.Windows.Forms.Panel();
		this.radAssets = new System.Windows.Forms.RadioButton();
		this.radLiablities = new System.Windows.Forms.RadioButton();
		this.panel5 = new System.Windows.Forms.Panel();
		this.radPL = new System.Windows.Forms.RadioButton();
		this.radBS = new System.Windows.Forms.RadioButton();
		this.btnShow = new System.Windows.Forms.Button();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdBsPl = new GridEX();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewNote = new System.Windows.Forms.ToolStripButton();
		this.btnUpdateNote = new System.Windows.Forms.ToolStripButton();
		this.panel8 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.panel10 = new System.Windows.Forms.Panel();
		this.radAllNote = new System.Windows.Forms.RadioButton();
		this.radPLNote = new System.Windows.Forms.RadioButton();
		this.btnShowNote = new System.Windows.Forms.Button();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.radBSNote = new System.Windows.Forms.RadioButton();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdNoteChange = new GridEX();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.btnSkey = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel5.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdBsPl).BeginInit();
		this.toolStrip2.SuspendLayout();
		this.panel8.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel11.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdNoteChange).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(950, 28);
		this.panel1.TabIndex = 2;
		this.toolStrip1.BackColor = System.Drawing.Color.White;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnnew, this.btnUpdate, this.BtnShortCutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(950, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 31);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(950, 27);
		this.panel3.TabIndex = 2;
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.Teal;
		this.label23.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(12, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(144, 18);
		this.label23.TabIndex = 25;
		this.label23.Text = "BS && PL Setting";
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.ctrlGrdBar2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 137);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(950, 28);
		this.panel2.TabIndex = 0;
		this.groupBox1.Controls.Add(this.panel6);
		this.groupBox1.Controls.Add(this.panel5);
		this.groupBox1.Controls.Add(this.btnShow);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(243, 73);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.panel6.Controls.Add(this.radAssets);
		this.panel6.Controls.Add(this.radLiablities);
		this.panel6.Location = new System.Drawing.Point(59, 20);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(97, 49);
		this.panel6.TabIndex = 1;
		this.radAssets.AutoSize = true;
		this.radAssets.Location = new System.Drawing.Point(6, 3);
		this.radAssets.Name = "radAssets";
		this.radAssets.Size = new System.Drawing.Size(70, 20);
		this.radAssets.TabIndex = 0;
		this.radAssets.TabStop = true;
		this.radAssets.Text = "Assets";
		this.radAssets.UseVisualStyleBackColor = true;
		this.radAssets.Visible = false;
		this.radLiablities.AutoSize = true;
		this.radLiablities.Location = new System.Drawing.Point(6, 26);
		this.radLiablities.Name = "radLiablities";
		this.radLiablities.Size = new System.Drawing.Size(84, 20);
		this.radLiablities.TabIndex = 1;
		this.radLiablities.TabStop = true;
		this.radLiablities.Text = "Liabilities";
		this.radLiablities.UseVisualStyleBackColor = true;
		this.radLiablities.Visible = false;
		this.panel5.Controls.Add(this.radPL);
		this.panel5.Controls.Add(this.radBS);
		this.panel5.Location = new System.Drawing.Point(4, 20);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(55, 49);
		this.panel5.TabIndex = 0;
		this.radPL.AutoSize = true;
		this.radPL.Checked = true;
		this.radPL.Location = new System.Drawing.Point(3, 3);
		this.radPL.Name = "radPL";
		this.radPL.Size = new System.Drawing.Size(40, 20);
		this.radPL.TabIndex = 0;
		this.radPL.TabStop = true;
		this.radPL.Text = "PL";
		this.radPL.UseVisualStyleBackColor = true;
		this.radPL.CheckedChanged += new System.EventHandler(radPL_CheckedChanged);
		this.radBS.AutoSize = true;
		this.radBS.Location = new System.Drawing.Point(4, 26);
		this.radBS.Name = "radBS";
		this.radBS.Size = new System.Drawing.Size(41, 20);
		this.radBS.TabIndex = 1;
		this.radBS.Text = "BS";
		this.radBS.UseVisualStyleBackColor = true;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(160, 40);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(61, 29);
		this.btnShow.TabIndex = 2;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(904, 3);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
		this.ctrlGrdBar2.MyGrid = this.grdBsPl;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(41, 21);
		this.ctrlGrdBar2.TabIndex = 23;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(964, 561);
		this.tabControl1.TabIndex = 4;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdBsPl);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 25);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(956, 532);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Change Notes For Group Accounts";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdNoteChange);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.panel11);
		this.tabPage2.Location = new System.Drawing.Point(4, 25);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(956, 532);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Change Note Title";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 58);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(950, 79);
		this.panel4.TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdBsPl).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdBsPl.FilterMode = (FilterMode)2;
		((System.Windows.Forms.Control)(object)this.grdBsPl).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdBsPl.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdBsPl).Location = new System.Drawing.Point(3, 165);
		((System.Windows.Forms.Control)(object)this.grdBsPl).Name = "grdBsPl";
		((ScrollableBase)this.grdBsPl).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdBsPl).Size = new System.Drawing.Size(950, 364);
		((System.Windows.Forms.Control)(object)this.grdBsPl).TabIndex = 1;
		this.grdBsPl.TotalRow = (InheritableBoolean)1;
		this.grdBsPl.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdBsPl).VisualStyle = (VisualStyle)4;
		this.grdBsPl.CellUpdated += new ColumnActionEventHandler(grdBsPl_CellUpdated);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(905, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
		this.ctrlGrdBar1.MyGrid = this.grdNoteChange;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(43, 22);
		this.ctrlGrdBar1.TabIndex = 24;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip2.BackColor = System.Drawing.Color.White;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewNote, this.btnUpdateNote, this.btnSkey });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(950, 27);
		this.toolStrip2.TabIndex = 8;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewNote.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewNote.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewNote.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewNote.Name = "btnNewNote";
		this.btnNewNote.Size = new System.Drawing.Size(59, 24);
		this.btnNewNote.Text = "&New";
		this.btnNewNote.Click += new System.EventHandler(btnNewNote_Click);
		this.btnUpdateNote.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdateNote.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdateNote.Name = "btnUpdateNote";
		this.btnUpdateNote.Size = new System.Drawing.Size(78, 24);
		this.btnUpdateNote.Text = "Update";
		this.btnUpdateNote.Click += new System.EventHandler(btnUpdateNote_Click);
		this.panel8.BackColor = System.Drawing.Color.White;
		this.panel8.Controls.Add(this.groupBox2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 59);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(950, 63);
		this.panel8.TabIndex = 0;
		this.groupBox2.Controls.Add(this.panel10);
		this.groupBox2.Controls.Add(this.btnShowNote);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(3, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(228, 56);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.panel10.Controls.Add(this.radAllNote);
		this.panel10.Controls.Add(this.radBSNote);
		this.panel10.Controls.Add(this.radPLNote);
		this.panel10.Location = new System.Drawing.Point(4, 20);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(144, 32);
		this.panel10.TabIndex = 0;
		this.radAllNote.AutoSize = true;
		this.radAllNote.Checked = true;
		this.radAllNote.Location = new System.Drawing.Point(3, 5);
		this.radAllNote.Name = "radAllNote";
		this.radAllNote.Size = new System.Drawing.Size(41, 20);
		this.radAllNote.TabIndex = 0;
		this.radAllNote.TabStop = true;
		this.radAllNote.Text = "All";
		this.radAllNote.UseVisualStyleBackColor = true;
		this.radPLNote.AutoSize = true;
		this.radPLNote.Location = new System.Drawing.Point(49, 5);
		this.radPLNote.Name = "radPLNote";
		this.radPLNote.Size = new System.Drawing.Size(40, 20);
		this.radPLNote.TabIndex = 1;
		this.radPLNote.Text = "PL";
		this.radPLNote.UseVisualStyleBackColor = true;
		this.btnShowNote.BackColor = System.Drawing.Color.Teal;
		this.btnShowNote.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowNote.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowNote.ForeColor = System.Drawing.Color.White;
		this.btnShowNote.Location = new System.Drawing.Point(154, 22);
		this.btnShowNote.Name = "btnShowNote";
		this.btnShowNote.Size = new System.Drawing.Size(61, 29);
		this.btnShowNote.TabIndex = 1;
		this.btnShowNote.Text = "Show";
		this.btnShowNote.UseVisualStyleBackColor = false;
		this.btnShowNote.Click += new System.EventHandler(btnShowNote_Click);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.toolStrip2);
		this.panel11.Controls.Add(this.label1);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 3);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(950, 56);
		this.panel11.TabIndex = 9;
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.Teal;
		this.label1.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(0, 31);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(165, 18);
		this.label1.TabIndex = 25;
		this.label1.Text = "Change Note Title";
		this.radBSNote.AutoSize = true;
		this.radBSNote.Location = new System.Drawing.Point(99, 5);
		this.radBSNote.Name = "radBSNote";
		this.radBSNote.Size = new System.Drawing.Size(41, 20);
		this.radBSNote.TabIndex = 2;
		this.radBSNote.Text = "BS";
		this.radBSNote.UseVisualStyleBackColor = true;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label3);
		this.panel7.Controls.Add(this.ctrlGrdBar1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 122);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(950, 24);
		this.panel7.TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.grdNoteChange).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdNoteChange.FilterMode = (FilterMode)2;
		((System.Windows.Forms.Control)(object)this.grdNoteChange).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdNoteChange.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdNoteChange).Location = new System.Drawing.Point(3, 146);
		((System.Windows.Forms.Control)(object)this.grdNoteChange).Name = "grdNoteChange";
		((ScrollableBase)this.grdNoteChange).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdNoteChange).Size = new System.Drawing.Size(950, 383);
		((System.Windows.Forms.Control)(object)this.grdNoteChange).TabIndex = 1;
		this.grdNoteChange.TotalRow = (InheritableBoolean)1;
		this.grdNoteChange.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdNoteChange).VisualStyle = (VisualStyle)4;
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.Teal;
		this.label2.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(3, 5);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(308, 18);
		this.label2.TabIndex = 25;
		this.label2.Text = "Change Notes For Group Accounts";
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.Teal;
		this.label3.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(4, 3);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(165, 18);
		this.label3.TabIndex = 26;
		this.label3.Text = "Change Note Title";
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(106, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.btnSkey.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSkey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSkey.Name = "btnSkey";
		this.btnSkey.Size = new System.Drawing.Size(106, 24);
		this.btnSkey.Text = "ShortCut Keys";
		this.btnSkey.Click += new System.EventHandler(BtnShortCutkeys_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 561);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "BsPlSettingForm";
		this.Text = "EccountbookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(BsPlSettingForm_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(DefineCity_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage2.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdBsPl).EndInit();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdNoteChange).EndInit();
		base.ResumeLayout(false);
	}
}
