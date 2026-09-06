using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.DAW;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.DAW;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp;

public class DefineApprovalDocument : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private int RecId;

	private bool UpdateMood;

	private int[] AryColWidth;

	private IContainer components;

	private ToolStripButton btnsave;

	private ToolStripButton btnnew;

	private Panel panel4;

	internal GridEX GridHistory;

	private Panel panel5;

	private Label label2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnUpdate;

	private Label label23;

	private Panel panel6;

	private Panel panel3;

	private Panel panel2;

	private Panel panel1;

	private Label label1;

	private TextBox txtDocmentDescription;

	internal UltraCombo CmbDocumentType;

	private Label label3;

	private TextBox txtTargetUrl;

	private CtrlGrdBar ctrlGrdBar3;

	public DefineApprovalDocument(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void DefineCropYear_Load(object sender, EventArgs e)
	{
		CmbDocumentType.Focus();
		DocumentTypesFill();
		btnUpdate.Visible = false;
		BindHistory();
	}

	private void DocumentTypesFill()
	{
		try
		{
			DataTable dt = DocumentType.GetDocumentTypes();
			if (dt.Rows.Count > 0)
			{
				DataTable dtDocument = new DataTable();
				dtDocument.Columns.Add("Id");
				dtDocument.Columns.Add("DocumentName");
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtDocument.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["DocumentTypeDescription"]);
				}
				DDL.BindDDL(dtDocument, CmbDocumentType, "Id", "DocumentName", "Document Type", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		try
		{
			if (FormValidation())
			{
				int success = DocumentList.Save(new DocumentList
				{
					Id = Conversion.ToInt(CmbDocumentType.Value),
					DocumentName = Conversion.ToString((object)((Control)(object)CmbDocumentType).Text),
					DocmentDescription = Conversion.ToString((object)txtDocmentDescription.Text),
					TargetURL = txtTargetUrl.Text
				});
				if (success >= 1)
				{
					MessageBox.Show("Record Save Successfully...[" + success + "]");
				}
				BindHistory();
				formReFresh();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BindHistory()
	{
		try
		{
			DataTable lst = DocumentList.GetAll();
			if (lst.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("DocumentName");
				dt.Columns.Add("DocumentDescription");
				dt.Columns.Add("TargetUrl");
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dt.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["DocumentName"], lst.Rows[i]["DocmentDescription"], lst.Rows[i]["TargetURL"]);
				}
				GridHistory.DataSource = dt;
				GridHistory.RetrieveStructure();
				GridHistory.RootTable.Columns["Id"].Visible = false;
				GridHistory.RootTable.Columns["DocumentName"].Width = 200;
				GridHistory.RootTable.Columns["DocumentDescription"].Width = 350;
				GridHistory.RootTable.Columns["TargetUrl"].Width = 250;
				CommonServices.GridAutoAdjustment(AryColWidth, GridHistory);
				ctrlGrdBar3_Load(null, null);
			}
			else
			{
				GridHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdcropyear_DoubleClick(object sender, EventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		try
		{
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			UpdateMood = true;
			DocumentList obj = new DocumentList();
			RecId = Conversion.ToInt((object)GridHistory.CurrentRow.Cells["Id"].Value.ToString());
			obj = DocumentList.GetByID(RecId);
			CmbDocumentType.Value = obj.Id;
			((Control)(object)CmbDocumentType).Text = obj.DocumentName;
			txtDocmentDescription.Text = obj.DocmentDescription;
			txtTargetUrl.Text = obj.TargetURL;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)CmbDocumentType).ActiveRow == null)
		{
			MessageBox.Show("Document Name Field is Required");
			CmbDocumentType.Focus();
			return false;
		}
		if (Conversion.ToString((object)txtTargetUrl.Text) == string.Empty)
		{
			MessageBox.Show("Target Url Field is Required");
			txtTargetUrl.Focus();
			return false;
		}
		return true;
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		try
		{
			if (FormValidation())
			{
				int success = DocumentList.Save(new DocumentList
				{
					Id = Conversion.ToInt(CmbDocumentType.Value),
					DocumentName = Conversion.ToString((object)((Control)(object)CmbDocumentType).Text),
					DocmentDescription = Conversion.ToString((object)txtDocmentDescription.Text),
					TargetURL = Conversion.ToString((object)txtTargetUrl.Text)
				});
				if (success >= 1)
				{
					MessageBox.Show("Record Update Successfully...[" + success + "]");
					btnsave.Visible = true;
					btnUpdate.Visible = false;
				}
				BindHistory();
				formReFresh();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void formReFresh()
	{
		((Control)(object)CmbDocumentType).Text = string.Empty;
		txtDocmentDescription.Clear();
		btnsave.Visible = true;
		btnUpdate.Visible = false;
		CmbDocumentType.Focus();
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		formReFresh();
		btnUpdate.Visible = false;
		btnsave.Visible = true;
	}

	private void DefineCropYear_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.Control && e.KeyCode == Keys.N)
		{
			btnnew_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
		{
			btnsave_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
		{
			btnUpdate_Click(null, null);
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GridHistory).Name);
			if (fs != null)
			{
				GridHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DefineApprovalDocument_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		GridEXLayout GridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		this.panel4 = new System.Windows.Forms.Panel();
		this.GridHistory = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.txtTargetUrl = new System.Windows.Forms.TextBox();
		this.CmbDocumentType = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.txtDocmentDescription = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.label23 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridHistory).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDocumentType).BeginInit();
		this.toolStrip1.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.GridHistory);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 137);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(919, 579);
		this.panel4.TabIndex = 3;
		this.GridHistory.AllowEdit = (InheritableBoolean)2;
		GridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridHistory.DesignTimeLayout = GridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridHistory.FilterMode = (FilterMode)2;
		this.GridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GridHistory).Name = "GridHistory";
		((ScrollableBase)this.GridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridHistory).Size = new System.Drawing.Size(919, 579);
		((System.Windows.Forms.Control)(object)this.GridHistory).TabIndex = 0;
		this.GridHistory.TotalRow = (InheritableBoolean)1;
		this.GridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridHistory).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.GridHistory).DoubleClick += new System.EventHandler(grdcropyear_DoubleClick);
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add(this.txtTargetUrl);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDocumentType);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Controls.Add(this.txtDocmentDescription);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 73);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(919, 64);
		this.panel5.TabIndex = 2;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(9, 37);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(67, 16);
		this.label3.TabIndex = 6;
		this.label3.Text = "Target Url";
		this.txtTargetUrl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTargetUrl.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTargetUrl.Location = new System.Drawing.Point(115, 34);
		this.txtTargetUrl.Multiline = true;
		this.txtTargetUrl.Name = "txtTargetUrl";
		this.txtTargetUrl.Size = new System.Drawing.Size(779, 22);
		this.txtTargetUrl.TabIndex = 2;
		this.CmbDocumentType.AllowNull = (DefaultableBoolean)1;
		this.CmbDocumentType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDocumentType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDocumentType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDocumentType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance39;
		((SpecialBoxBase)((UltraGridBase)this.CmbDocumentType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance44).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDocumentType).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDocumentType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDocumentType).Location = new System.Drawing.Point(116, 6);
		((System.Windows.Forms.Control)(object)this.CmbDocumentType).Name = "CmbDocumentType";
		((System.Windows.Forms.Control)(object)this.CmbDocumentType).Size = new System.Drawing.Size(230, 25);
		((System.Windows.Forms.Control)(object)this.CmbDocumentType).TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(355, 10);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(139, 16);
		this.label1.TabIndex = 3;
		this.label1.Text = "Document Description";
		this.txtDocmentDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocmentDescription.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocmentDescription.Location = new System.Drawing.Point(501, 7);
		this.txtDocmentDescription.Multiline = true;
		this.txtDocmentDescription.Name = "txtDocmentDescription";
		this.txtDocmentDescription.Size = new System.Drawing.Size(393, 22);
		this.txtDocmentDescription.TabIndex = 1;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(8, 10);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(108, 16);
		this.label2.TabIndex = 1;
		this.label2.Text = "Document Name";
		this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnnew, this.btnUpdate, this.btnsave });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(919, 27);
		this.toolStrip1.TabIndex = 3;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(55, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(9, 12);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(236, 24);
		this.label23.TabIndex = 25;
		this.label23.Text = "Define Approval Document";
		this.panel6.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel6.Controls.Add(this.label23);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(919, 46);
		this.panel6.TabIndex = 6;
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(919, 46);
		this.panel3.TabIndex = 2;
		this.panel2.Controls.Add(this.ctrlGrdBar3);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(919, 27);
		this.panel2.TabIndex = 0;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(875, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.GridHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(40, 25);
		this.ctrlGrdBar3.TabIndex = 23;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(919, 716);
		this.panel1.TabIndex = 1;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(919, 716);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "DefineApprovalDocument";
		this.Text = "Define Approval Document";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(DefineApprovalDocument_FormClosing);
		base.Load += new System.EventHandler(DefineCropYear_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(DefineCropYear_KeyDown);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridHistory).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDocumentType).EndInit();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
