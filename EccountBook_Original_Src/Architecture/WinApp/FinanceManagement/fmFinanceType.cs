using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.FinanceManagment;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.FinanceManagment;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.FinanceManagement;

public class fmFinanceType : Form
{
	private DropDownBind DDL = new DropDownBind();

	private int RecId;

	private bool UpdateMode;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Panel panel5;

	private Panel panel4;

	private Panel panel3;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private Label label15;

	private TextBox txtDesc;

	private Label label8;

	private Label label1;

	private ToolStripButton btnUpdate;

	internal GridEX DataGridHistory;

	private Button button3;

	private Label label41;

	internal UltraCombo cmbFundType;

	private Label label2;

	private Label label3;

	private ToolStripButton btnshortcutkeys;

	private ToolStripButton btnRefresh;

	public fmFinanceType(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void fmFinanceType_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(500, 450);
			TypeFill();
			BindHistory();
			cmbFundType.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TypeFill()
	{
		try
		{
			DataTable dttypes = new DataTable();
			dttypes.Columns.Add("Id");
			dttypes.Columns.Add("Type");
			dttypes.Rows.Add(1, "Fund Base");
			dttypes.Rows.Add(2, "NonFund Base");
			DDL.BindDDL(dttypes, cmbFundType, "Type", "Type", "Fund type", ZeroIndex: true);
			((UltraGridBase)cmbFundType).Rows[1].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formvalidation()
	{
		if (((UltraGridBase)cmbFundType).ActiveRow == null || Conversion.ToInt(cmbFundType.Value) == 0)
		{
			MessageBox.Show("Type Is Required");
			cmbFundType.Focus();
			return false;
		}
		if (txtDesc.Text == "")
		{
			MessageBox.Show("Description Is Required");
			txtDesc.Focus();
			return false;
		}
		return true;
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

	private void button3_Click(object sender, EventArgs e)
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

	private void FormReset()
	{
		try
		{
			((Control)(object)cmbFundType).Text = string.Empty;
			txtDesc.Text = string.Empty;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			UpdateMode = false;
			RecId = 0;
			BindHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!formvalidation())
			{
				return;
			}
			FmFinanceType fmFinance = new FmFinanceType();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				fmFinance.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			fmFinance.OrganizationId = UserAccount.OrganizationId;
			fmFinance.CompanyId = UserAccount.OrganizationId;
			fmFinance.FundNonFund = ((Control)(object)cmbFundType).Text;
			fmFinance.FundTypeDescription = txtDesc.Text;
			int success = FmFinanceType.Save(fmFinance);
			if (!UpdateMode && success > 0)
			{
				MessageBox.Show("Save SuccessFully");
			}
			if (UpdateMode && success > 0)
			{
				MessageBox.Show("Update SuccessFully");
			}
			FormReset();
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

	private void BindHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = FmFinanceType.GetAll(new FmFinanceType
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("FundType");
			dt.Columns.Add("Fund Description");
			for (int i = 0; i < lst.Rows.Count; i++)
			{
				dt.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["FundNonFund"], lst.Rows[i]["FundTypeDescription"]);
			}
			DataGridHistory.DataSource = dt;
			DataGridHistory.RetrieveStructure();
			grdSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		try
		{
			DataGridHistory.ColumnAutoResize = false;
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(DataGridHistory);
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Key = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 30;
			DataGridHistory.RootTable.Columns["Edit"].Position = 0;
			DataGridHistory.FrozenColumns = 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			FmFinanceType fm = new FmFinanceType();
			RecId = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
			fm = FmFinanceType.GetByID(RecId);
			((Control)(object)cmbFundType).Text = fm.FundNonFund;
			txtDesc.Text = fm.FundTypeDescription;
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			UpdateMode = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void fmFinanceType_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S)
			{
				btnsave_Click(null, null);
			}
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnUpdate_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)DataGridHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				cmbFundType.Focus();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbFundType.Focus();
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
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Type");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On on Type");
			dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Update Record");
			dt.Rows.Add("Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link");
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

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || DataGridHistory.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (DataGridHistory.CurrentColumn.Key == "Edit")
				{
					DataGridHistory_DoubleClick(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				DataGridHistory_DoubleClick(null, null);
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
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance231 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.FinanceManagement.fmFinanceType));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.cmbFundType = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.txtDesc = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.button3 = new System.Windows.Forms.Button();
		this.label15 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.label3 = new System.Windows.Forms.Label();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbFundType).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(404, 418);
		this.panel1.TabIndex = 0;
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 136);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(404, 26);
		this.panel5.TabIndex = 3;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.DataGridHistory).BackColor = System.Drawing.Color.White;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 162);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(404, 256);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 1;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.label41);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbFundType);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.txtDesc);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 52);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(404, 84);
		this.panel4.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.Red;
		this.label2.Location = new System.Drawing.Point(92, 45);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(14, 17);
		this.label2.TabIndex = 548;
		this.label2.Text = "*";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.ForeColor = System.Drawing.Color.Red;
		this.label41.Location = new System.Drawing.Point(92, 17);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(14, 17);
		this.label41.TabIndex = 547;
		this.label41.Text = "*";
		this.cmbFundType.AllowNull = (DefaultableBoolean)1;
		this.cmbFundType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbFundType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbFundType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbFundType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.cmbFundType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbFundType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance221).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance221).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance221).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbFundType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance221;
		((AppearanceBase)appearance222).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFundType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance222;
		((SpecialBoxBase)((UltraGridBase)this.cmbFundType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance223).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance223).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFundType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.cmbFundType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbFundType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance224).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbFundType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance224;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance225).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbFundType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.cmbFundType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbFundType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFundType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance226;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance227).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbFundType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.cmbFundType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbFundType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance228).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance228).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance228).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance228).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFundType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance228;
		((AppearanceBase)appearance229).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbFundType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.cmbFundType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbFundType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance230).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbFundType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance230;
		((UltraGridBase)this.cmbFundType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbFundType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.cmbFundType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbFundType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbFundType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbFundType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbFundType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbFundType).Location = new System.Drawing.Point(109, 12);
		((System.Windows.Forms.Control)(object)this.cmbFundType).Name = "cmbFundType";
		((System.Windows.Forms.Control)(object)this.cmbFundType).Size = new System.Drawing.Size(257, 26);
		((System.Windows.Forms.Control)(object)this.cmbFundType).TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(16, 46);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(68, 15);
		this.label1.TabIndex = 39;
		this.label1.Text = "Description";
		this.txtDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDesc.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDesc.Location = new System.Drawing.Point(109, 42);
		this.txtDesc.Name = "txtDesc";
		this.txtDesc.Size = new System.Drawing.Size(257, 23);
		this.txtDesc.TabIndex = 1;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(16, 18);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(32, 15);
		this.label8.TabIndex = 37;
		this.label8.Text = "Type";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.button3);
		this.panel3.Controls.Add(this.label15);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 25);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(404, 27);
		this.panel3.TabIndex = 1;
		this.button3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button3.BackColor = System.Drawing.Color.Teal;
		this.button3.FlatAppearance.BorderSize = 0;
		this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button3.Image = (System.Drawing.Image)resources.GetObject("button3.Image");
		this.button3.Location = new System.Drawing.Point(373, 1);
		this.button3.Margin = new System.Windows.Forms.Padding(2);
		this.button3.Name = "button3";
		this.button3.Size = new System.Drawing.Size(27, 24);
		this.button3.TabIndex = 7;
		this.button3.UseVisualStyleBackColor = false;
		this.button3.Click += new System.EventHandler(button3_Click);
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(3, 3);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(116, 19);
		this.label15.TabIndex = 1;
		this.label15.Text = "Finance Type";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(404, 25);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnnew, this.btnRefresh, this.btnsave, this.btnUpdate, this.btnshortcutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(404, 27);
		this.toolStrip1.TabIndex = 2;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
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
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.Teal;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label3.Location = new System.Drawing.Point(3, 2);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(67, 19);
		this.label3.TabIndex = 1;
		this.label3.Text = "History";
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(404, 418);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "fmFinanceType";
		this.Text = "fmFinanceType";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(fmFinanceType_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(fmFinanceType_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbFundType).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
