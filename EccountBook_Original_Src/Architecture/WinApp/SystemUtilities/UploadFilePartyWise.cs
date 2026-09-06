using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.WinApp.Common;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.SystemUtilities;

public class UploadFilePartyWise : Form
{
	public List<TempAttachments> lst = new List<TempAttachments>();

	public static DataTable dtAttachments = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private string fileSavePath = "";

	private string DriverPicPath = string.Empty;

	private string UniqFileName = string.Empty;

	private string saveDirectory = "";

	private IContainer components;

	private Panel panel1;

	private Button Choose;

	private Panel panel2;

	internal GridEX grdattach;

	private TextBox txtpath;

	private ContextMenuStrip contextMenuStrip1;

	private Button Add;

	private Button btnClose;

	private Panel panel3;

	internal UltraCombo CmbPartyName;

	private Label label12;

	public UploadFilePartyWise()
	{
		InitializeComponent();
	}

	private void Attachment_Load(object sender, EventArgs e)
	{
		try
		{
			SupplierNameFill();
			if (dtAttachments.Columns.Count == 0)
			{
				dtAttachments.Columns.Add("Id");
				dtAttachments.Columns.Add("Path");
			}
			AddToGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierNameFill()
	{
		try
		{
			DataTable dtsuppcus = CommonServices.SupplierCustomerGetforComboServiceBind();
			if (dtsuppcus.Rows.Count > 0)
			{
				DDL.BindDDL(dtsuppcus, CmbPartyName, "Id", "CompanyName", "Party Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void AddToGrid()
	{
		try
		{
			dtAttachments.Rows.Clear();
			grdattach.DataSource = null;
			foreach (TempAttachments item in lst)
			{
				dtAttachments.Rows.Add(item.Id, item.Path);
			}
			if (dtAttachments.Rows.Count > 0)
			{
				grdattach.DataSource = dtAttachments;
				grdattach.RetrieveStructure();
				grdattach.RootTable.Columns["Id"].Visible = false;
				grdattach.RootTable.Columns["Path"].Width = 900;
			}
			else
			{
				grdattach.DataSource = null;
				grdattach.RetrieveStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Choose_Click(object sender, EventArgs e)
	{
		try
		{
			string DestinationFolder = ((Control)(object)CmbPartyName).Text.Trim();
			if (!Directory.Exists(Application.ExecutablePath + "\\..\\UploadFiles"))
			{
				Directory.CreateDirectory(Application.ExecutablePath + "\\..\\UploadFiles");
			}
			if (!Directory.Exists(Application.ExecutablePath + "\\..\\UploadFiles\\" + DestinationFolder))
			{
				Directory.CreateDirectory(Application.ExecutablePath + "\\..\\UploadFiles\\" + DestinationFolder);
			}
			string saveDirectory = Application.ExecutablePath + "\\..\\UploadFiles\\" + DestinationFolder;
			if (saveDirectory != string.Empty)
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "Text files (All files (*.*)|*.*";
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					string fileName = Path.GetFileName(openFileDialog.FileName);
					fileSavePath = Path.Combine(saveDirectory, fileName);
					File.Copy(openFileDialog.FileName, fileSavePath, overwrite: true);
					txtpath.Text = fileName;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Add_Click(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbPartyName).ActiveRow == null)
			{
				CmbPartyName.Focus();
				throw new Exception("Party Name Field Required");
			}
			if (DriverPicPath == string.Empty && txtpath.Text == string.Empty)
			{
				throw new Exception("Please select Attechment");
			}
			if (txtpath.Text != null && txtpath.Text != string.Empty)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = txtpath.Text;
				lst.Add(TA);
				AddToGrid();
			}
			txtpath.Text = string.Empty;
			DriverPicPath = string.Empty;
			UniqFileName = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Attachment_FormClosing(object sender, FormClosingEventArgs e)
	{
		Hide();
		lst.Clear();
	}

	private void btnClose_Click(object sender, EventArgs e)
	{
		Hide();
		lst.Clear();
	}

	private void btnPreview_Click(object sender, EventArgs e)
	{
	}

	private void btnResetDriv_Click(object sender, EventArgs e)
	{
		try
		{
			File.Delete(saveDirectory + UniqFileName);
			UniqFileName = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Attachment_KeyDown(object sender, KeyEventArgs e)
	{
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
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
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		GridEXLayout grdattach_DesignTimeLayout = new GridEXLayout();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdattach = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.CmbPartyName = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.txtpath = new System.Windows.Forms.TextBox();
		this.Choose = new System.Windows.Forms.Button();
		this.btnClose = new System.Windows.Forms.Button();
		this.Add = new System.Windows.Forms.Button();
		this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdattach).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyName).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(819, 620);
		this.panel1.TabIndex = 0;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdattach);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 100);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(819, 520);
		this.panel3.TabIndex = 5;
		this.grdattach.AllowDelete = (InheritableBoolean)1;
		grdattach_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdattach.DesignTimeLayout = grdattach_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdattach).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdattach.FilterMode = (FilterMode)2;
		this.grdattach.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdattach).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdattach.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdattach).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdattach).Name = "grdattach";
		((ScrollableBase)this.grdattach).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdattach).Size = new System.Drawing.Size(819, 520);
		((System.Windows.Forms.Control)(object)this.grdattach).TabIndex = 2;
		this.grdattach.TotalRow = (InheritableBoolean)1;
		this.grdattach.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdattach).VisualStyle = (VisualStyle)4;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPartyName);
		this.panel2.Controls.Add(this.label12);
		this.panel2.Controls.Add(this.txtpath);
		this.panel2.Controls.Add(this.Choose);
		this.panel2.Controls.Add(this.btnClose);
		this.panel2.Controls.Add(this.Add);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(819, 100);
		this.panel2.TabIndex = 0;
		this.CmbPartyName.AllowNull = (DefaultableBoolean)1;
		this.CmbPartyName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPartyName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPartyName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPartyName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Location = new System.Drawing.Point(130, 14);
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Name = "CmbPartyName";
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Size = new System.Drawing.Size(247, 26);
		((System.Windows.Forms.Control)(object)this.CmbPartyName).TabIndex = 9;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(18, 18);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(71, 15);
		this.label12.TabIndex = 8;
		this.label12.Text = "Party Name";
		this.txtpath.Location = new System.Drawing.Point(130, 45);
		this.txtpath.Margin = new System.Windows.Forms.Padding(2);
		this.txtpath.Multiline = true;
		this.txtpath.Name = "txtpath";
		this.txtpath.Size = new System.Drawing.Size(620, 27);
		this.txtpath.TabIndex = 2;
		this.txtpath.Visible = false;
		this.Choose.Location = new System.Drawing.Point(9, 45);
		this.Choose.Margin = new System.Windows.Forms.Padding(2);
		this.Choose.Name = "Choose";
		this.Choose.Size = new System.Drawing.Size(56, 26);
		this.Choose.TabIndex = 1;
		this.Choose.Text = "Brows";
		this.Choose.UseVisualStyleBackColor = true;
		this.Choose.Click += new System.EventHandler(Choose_Click);
		this.btnClose.Location = new System.Drawing.Point(754, 45);
		this.btnClose.Margin = new System.Windows.Forms.Padding(2);
		this.btnClose.Name = "btnClose";
		this.btnClose.Size = new System.Drawing.Size(56, 27);
		this.btnClose.TabIndex = 4;
		this.btnClose.Text = "Close";
		this.btnClose.UseVisualStyleBackColor = true;
		this.btnClose.Click += new System.EventHandler(btnClose_Click);
		this.Add.Location = new System.Drawing.Point(70, 45);
		this.Add.Margin = new System.Windows.Forms.Padding(2);
		this.Add.Name = "Add";
		this.Add.Size = new System.Drawing.Size(56, 26);
		this.Add.TabIndex = 3;
		this.Add.Text = "Add";
		this.Add.UseVisualStyleBackColor = true;
		this.Add.Click += new System.EventHandler(Add_Click);
		this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenuStrip1.Name = "contextMenuStrip1";
		this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(819, 620);
		base.ControlBox = false;
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "UploadFilePartyWise";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Attachment";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Attachment_FormClosing);
		base.Load += new System.EventHandler(Attachment_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(Attachment_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdattach).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyName).EndInit();
		base.ResumeLayout(false);
	}
}
