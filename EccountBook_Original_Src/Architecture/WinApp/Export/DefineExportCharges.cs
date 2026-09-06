using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Export;

public class DefineExportCharges : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DataTable dtHistoryGrid = new DataTable();

	private int RecId;

	private bool FormInitialized;

	private string ScreenName = "DefineExportCharges";

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private Panel panel4;

	internal GridEX grdfrm;

	private Button btnAdd;

	private Label label4;

	private Label label3;

	private Label label2;

	private Label label1;

	private TextBox txtbrandCode;

	private TextBox txtBrandName;

	private MenuStrip menuStrip1;

	private ToolStripButton btnnew;

	private ToolStripMenuItem btnSave;

	private ToolStripButton btnupdate;

	private ToolStripButton btnShortCutKeys;

	private CtrlGrdBar ctrlGrdBar1;

	internal UltraCombo CmbExpenseAccount;

	private Label label16;

	public DefineExportCharges(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
		InitializeComponentMethod();
	}

	private void InitializeComponentCustom()
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1100, 800);
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
			ScreenName = Conversion.ToString((object)base.Name);
			return new
			{
				HistoryData = ExportCharges.FormHistory(UserAccount.OrganizationId, UserAccount.CompanyId, 0)
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
				BindGrid(result.HistoryData);
				ExpensesAcBind();
				FormInitialized = true;
				btnSave.Visible = true;
				btnupdate.Visible = false;
				btnAdd.Text = "Save";
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void ExpensesAcBind()
	{
		try
		{
			DataTable dt = DatatableHelper.GetAccountsFromGlobalByTypeIds(new int[2] { 21, 10 }, null, null, null, null, new int[1] { 3 });
			InfragisticsHelper.BindAndRetainSelection(CmbExpenseAccount, dt, "Id", "AccountTitle", "Account Title", AllColumns: true, Conversion.ToInt(CmbExpenseAccount.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool ValidateInputs()
	{
		if (string.IsNullOrWhiteSpace(txtbrandCode.Text))
		{
			MessageBox.Show("Charges Code is required");
			txtbrandCode.Focus();
			return false;
		}
		if (string.IsNullOrWhiteSpace(txtBrandName.Text))
		{
			MessageBox.Show("Charges Name is required");
			txtBrandName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbExpenseAccount).ActiveRow == null || Conversion.ToInt(CmbExpenseAccount.Value) == 0)
		{
			MessageBox.Show("Account Title is required");
			CmbExpenseAccount.Focus();
			return false;
		}
		if (GridEX_Helper.IsAnyCellValueHaveSameValueExcept(grdfrm, "ChargesCode", txtbrandCode.Text.Trim(), "Id", RecId))
		{
			MessageBox.Show("This Charges Code already exists.");
			txtbrandCode.Focus();
			return false;
		}
		if (GridEX_Helper.IsAnyCellValueHaveSameValueExcept(grdfrm, "ChargesName", txtBrandName.Text.Trim(), "Id", RecId))
		{
			MessageBox.Show("This Charges Name already exists.");
			txtBrandName.Focus();
			return false;
		}
		return true;
	}

	private void Insert()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		try
		{
			if (ValidateInputs() && MessageBox.Show((RecId > 0) ? "Are you sure you want to update?" : "Are you sure you want to save?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ExportCharges.Save(new ExportCharges
				{
					ExportChargesID = RecId,
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					EntryUserId = UserAccount.ID,
					ModifyUserId = UserAccount.ID,
					EntryDate = DateTime.Now,
					ModifyDate = DateTime.Now,
					ExportChargesCode = txtbrandCode.Text.Trim(),
					ExportChargesName = txtBrandName.Text.Trim(),
					AccountId = Conversion.ToInt(CmbExpenseAccount.Value)
				});
				MessageBox.Show((RecId > 0) ? "Record updated successfully." : "Record saved successfully.");
				ResetForm();
				BindGrid(ExportCharges.FormHistory(UserAccount.OrganizationId, UserAccount.CompanyId, 0));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
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

	private void ReadById(int Id)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			RecId = Id;
			GridEXRow item = grdfrm.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				txtbrandCode.Text = Conversion.ToString(item.Cells["ChargesCode"].Value);
				txtBrandName.Text = Conversion.ToString(item.Cells["ChargesName"].Value);
				if (Conversion.ToInt(item.Cells["AccountId"].Value) > 0)
				{
					CmbExpenseAccount.Value = Conversion.ToInt(item.Cells["AccountId"].Value);
				}
				btnSave.Visible = false;
				btnupdate.Visible = true;
				btnAdd.Text = "Update";
				txtbrandCode.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAdd_Click(object sender, EventArgs e)
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

	public void BindGrid(DataTable dt)
	{
		try
		{
			if (dt.Rows.Count > 0)
			{
				if (dtHistoryGrid.Columns.Count == 0)
				{
					dtHistoryGrid.Columns.Add("Id", typeof(int));
					dtHistoryGrid.Columns.Add("ChargesCode");
					dtHistoryGrid.Columns.Add("ChargesName");
					dtHistoryGrid.Columns.Add("AccountId", typeof(int));
					dtHistoryGrid.Columns.Add("AccountTitle", typeof(string));
					dtHistoryGrid.Columns.Add("EntryUser");
					dtHistoryGrid.Columns.Add("EntryDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("ModifyUser");
					dtHistoryGrid.Columns.Add("ModifyDate", typeof(DateTime));
				}
				dtHistoryGrid.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtHistoryGrid.Rows.Add(dt.Rows[i]["ExportChargesID"], dt.Rows[i]["ExportChargesCode"], dt.Rows[i]["ExportChargesName"], dt.Rows[i]["AccountId"], dt.Rows[i]["AccountTitle"], dt.Rows[i]["EntryUserName"], dt.Rows[i]["EntryDate"], dt.Rows[i]["ModifyUserName"], dt.Rows[i]["ModifyDate"]);
				}
				grdfrm.DataSource = dtHistoryGrid;
				grdfrm.RetrieveStructure();
				grdfrm.RootTable.Columns["Id"].Visible = false;
				grdfrm.RootTable.Columns["AccountId"].Visible = false;
				grdfrm.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
				grdfrm.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
				GridEX_Helper.GridWrappingAndColumnSettings(grdfrm, 2);
			}
			else
			{
				grdfrm.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_DoubleClick(object sender, EventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdfrm.CurrentRow != null && (int)grdfrm.CurrentRow.RowType == 0)
			{
				ReadById(Conversion.ToInt(grdfrm.CurrentRow.Cells["Id"].Value));
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grdfrm, dtHistoryGrid);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ResetForm()
	{
		RecId = 0;
		txtbrandCode.Clear();
		txtBrandName.Clear();
		((Control)(object)CmbExpenseAccount).Text = string.Empty;
		btnSave.Visible = true;
		btnupdate.Visible = false;
		btnAdd.Text = "Save";
		txtbrandCode.Focus();
		List<string> allServices = new List<string> { "AccountsWithCustomGroupId" };
		DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
		ExpensesAcBind();
	}

	private void DefineExportCharges_FormClosed(object sender, FormClosedEventArgs e)
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

	private void btnnew_Click(object sender, EventArgs e)
	{
		ResetForm();
	}

	private void DefineExportCharges_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.Control && e.KeyCode == Keys.S && btnSave.Visible && btnSave.Enabled)
			{
				btnSave_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.U && btnupdate.Visible && btnupdate.Enabled)
			{
				btnupdate_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.N)
			{
				ResetForm();
			}
			else if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			else if (e.Control && e.KeyCode == Keys.F5)
			{
				txtbrandCode.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdfrm).Focus();
			}
			else if (e.Control && e.Alt)
			{
				ShowShortcutKeys();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Shortcut error: " + ex.Message);
		}
	}

	private void ShowShortcutKeys()
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
			dt.Rows.Add("Ctrl+F5", "For Focus on ChargesCode");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On ChargesCode");
			dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Update Record");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortcukeys_Click(object sender, EventArgs e)
	{
		try
		{
			ShowShortcutKeys();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdfrm.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Control && e.KeyCode == Keys.Return)
			{
				grdfrm_DoubleClick(null, null);
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
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Expected O, but got Unknown
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Export.DefineExportCharges));
		this.panel1 = new System.Windows.Forms.Panel();
		this.CmbExpenseAccount = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtbrandCode = new System.Windows.Forms.TextBox();
		this.txtBrandName = new System.Windows.Forms.TextBox();
		this.btnAdd = new System.Windows.Forms.Button();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.menuStrip1 = new System.Windows.Forms.MenuStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripMenuItem();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdfrm = new GridEX();
		this.label3 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbExpenseAccount).BeginInit();
		this.panel2.SuspendLayout();
		this.menuStrip1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbExpenseAccount);
		this.panel1.Controls.Add(this.label16);
		this.panel1.Controls.Add(this.label2);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Controls.Add(this.txtbrandCode);
		this.panel1.Controls.Add(this.txtBrandName);
		this.panel1.Controls.Add(this.btnAdd);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(532, 167);
		this.panel1.TabIndex = 0;
		this.CmbExpenseAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbExpenseAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbExpenseAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbExpenseAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbExpenseAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbExpenseAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbExpenseAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbExpenseAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExpenseAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance135;
		((SpecialBoxBase)((UltraGridBase)this.CmbExpenseAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExpenseAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbExpenseAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbExpenseAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbExpenseAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbExpenseAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbExpenseAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbExpenseAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExpenseAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbExpenseAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbExpenseAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbExpenseAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExpenseAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbExpenseAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbExpenseAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbExpenseAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbExpenseAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbExpenseAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbExpenseAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbExpenseAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbExpenseAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbExpenseAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbExpenseAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbExpenseAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbExpenseAccount).Location = new System.Drawing.Point(119, 104);
		((System.Windows.Forms.Control)(object)this.CmbExpenseAccount).Name = "CmbExpenseAccount";
		((System.Windows.Forms.Control)(object)this.CmbExpenseAccount).Size = new System.Drawing.Size(235, 26);
		((System.Windows.Forms.Control)(object)this.CmbExpenseAccount).TabIndex = 2;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(15, 108);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(78, 15);
		this.label16.TabIndex = 118;
		this.label16.Text = "Account Title";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(15, 85);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(84, 15);
		this.label2.TabIndex = 5;
		this.label2.Text = "Charges Name";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(15, 63);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(79, 15);
		this.label1.TabIndex = 5;
		this.label1.Text = "Charges Code";
		this.txtbrandCode.Location = new System.Drawing.Point(119, 56);
		this.txtbrandCode.Name = "txtbrandCode";
		this.txtbrandCode.Size = new System.Drawing.Size(235, 22);
		this.txtbrandCode.TabIndex = 0;
		this.txtBrandName.Location = new System.Drawing.Point(119, 80);
		this.txtBrandName.Name = "txtBrandName";
		this.txtBrandName.Size = new System.Drawing.Size(235, 22);
		this.txtBrandName.TabIndex = 1;
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(261, 131);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(93, 28);
		this.btnAdd.TabIndex = 3;
		this.btnAdd.Text = "Save";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add(this.menuStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(532, 50);
		this.panel2.TabIndex = 4;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(3, 28);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(192, 19);
		this.label4.TabIndex = 33;
		this.label4.Text = "Define Export Charges";
		this.menuStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnSave, this.btnupdate, this.btnShortCutKeys });
		this.menuStrip1.Location = new System.Drawing.Point(0, 0);
		this.menuStrip1.Name = "menuStrip1";
		this.menuStrip1.Size = new System.Drawing.Size(532, 27);
		this.menuStrip1.TabIndex = 0;
		this.menuStrip1.Text = "menuStrip1";
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 20);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(68, 23);
		this.btnSave.Text = "Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(74, 20);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(122, 20);
		this.btnShortCutKeys.Text = "ShortCut Keys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortcukeys_Click);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 167);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(532, 26);
		this.panel4.TabIndex = 6;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(490, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 34;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupTotals = (GroupTotals)2;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 193);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(532, 176);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 1;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdfrm).DoubleClick += new System.EventHandler(grdfrm_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdfrm).KeyDown += new System.Windows.Forms.KeyEventHandler(grdfrm_KeyDown);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(8, 4);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(67, 19);
		this.label3.TabIndex = 0;
		this.label3.Text = "History";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(532, 369);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		base.Controls.Add(this.panel4);
		base.Controls.Add(this.panel1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "DefineExportCharges";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(DefineExportCharges_FormClosed);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(DefineExportCharges_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbExpenseAccount).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.menuStrip1.ResumeLayout(false);
		this.menuStrip1.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		base.ResumeLayout(false);
	}
}
