using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Definition;

public class AcfrmChequebookRegistration : Form
{
	private int RecId;

	private DataTable dtHistory = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private ToolStrip miniToolStrip;

	private Panel panel2;

	private ToolStrip toolStrip2;

	private ToolStripButton New;

	private new ToolStripButton Update;

	private ToolStripButton Save;

	private Panel panel3;

	private Label label23;

	private Panel panel1;

	private Panel panel4;

	private Panel panel5;

	private DateTimePicker docdate;

	private Label label7;

	private Label label6;

	private TextBox txtserialfrom;

	private TextBox txtserialto;

	private TextBox txtdocno;

	private TextBox txtprefix;

	private Label label4;

	private Label label2;

	private Label label1;

	private Label label5;

	internal GridEX grdcheq;

	private CtrlGrdBar ctrlGrdBar1;

	internal UltraCombo cmbbankac;

	private ToolStripButton btnCheqBookStatus;

	private ToolStripButton btnShortCutKey;

	private ToolStripButton btnRefresh;

	private Label label8;

	private Label label3;

	private TextBox txtremarks;

	private Panel panel6;

	private Panel panel7;

	private Panel panel8;

	private Label label9;

	internal GridEX GrdAccountDetail;

	public AcfrmChequebookRegistration(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void FrmChequebookRegistration_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1120, 890);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			Save.Enabled = formright.DoHaveSaveRight;
			Update.Enabled = formright.DoHaveUpdateRights;
			toolStrip2.Enabled = formright.DoHavePrintRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			cmbBnkac();
			CheqbookRegistratioinGridFill();
			cmbbankac.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbBnkac()
	{
		try
		{
			int id = Conversion.ToInt(cmbbankac.Value);
			DataTable dtType = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("15");
			if (dtType.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtType, cmbbankac, "Id", "AccountTitle", "Account Title", ZeroIndex: true);
			if (id > 0)
			{
				if (dtType.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					cmbbankac.Value = id;
				}
				else
				{
					((Control)(object)cmbbankac).Text = string.Empty;
				}
			}
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
			((Control)(object)cmbbankac).Text = string.Empty;
			txtdocno.Clear();
			txtprefix.Clear();
			txtserialfrom.Clear();
			txtserialto.Clear();
			cmbbankac.Focus();
			CheqbookRegistratioinGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void New_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void AcfrmChequebookRegistration_FormClosing(object sender, FormClosingEventArgs e)
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
			cmbBnkac();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Save_Click(object sender, EventArgs e)
	{
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt64((object)txtserialfrom.Text) > Conversion.ToInt64((object)txtserialto.Text))
			{
				throw new Exception("Serial From Can't Be Greater Than Serial To! Please Check.");
			}
			if (Conversion.ToInt64((object)txtserialfrom.Text) == Conversion.ToInt64((object)txtserialto.Text))
			{
				throw new Exception("Serial From and Serial To Can't Be Same! Please Check.");
			}
			long diff = Conversion.ToInt64((object)txtserialto.Text) - Conversion.ToInt64((object)txtserialfrom.Text);
			if (diff >= 100 && diff < 500)
			{
				if (MessageBox.Show("Are you sure to Save Cheques? Cheques Quantity Greater Than 100", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}
			else if (diff >= 500)
			{
				MessageBox.Show("You are not Allowed to Save Cheques? Cheques Quantity Greater Than 500");
				return;
			}
			CheqBookHeader chqbook = new CheqBookHeader();
			chqbook.DocDate = docdate.Value;
			chqbook.DocNo = Conversion.ToInt((object)txtdocno.Text.Trim());
			chqbook.BankId = Conversion.ToInt(cmbbankac.Value);
			chqbook.CbPrefix = Conversion.ToString((object)txtprefix.Text.Trim());
			chqbook.CbSrFrom = Conversion.ToString((object)txtserialfrom.Text.Trim());
			chqbook.CbSrTo = Conversion.ToString((object)txtserialto.Text.Trim());
			chqbook.Remarks = Conversion.ToString((object)txtremarks.Text.Trim());
			chqbook.ChartOfAccountId = Conversion.ToInt(cmbbankac.Value);
			chqbook.OrganizationId = UserAccount.OrganizationId;
			chqbook.CompanyId = UserAccount.CompanyId;
			chqbook.EntryUser = UserAccount.ID;
			chqbook.Cheqbookdetaillist = new List<CheqBookDetail>();
			for (double i = Conversion.ToDouble((object)txtserialfrom.Text); i <= Conversion.ToDouble((object)txtserialto.Text); i += 1.0)
			{
				CheqBookDetail cd = new CheqBookDetail();
				cd.CheqNo = Conversion.ToString((object)i);
				cd.CheqStatus = "Blank";
				cd.OtherRemarks = txtremarks.Text;
				chqbook.Cheqbookdetaillist.Add(cd);
			}
			CheqBookHeader.Save(chqbook);
			MessageBox.Show("Save Successfully");
			Reset();
			CheqbookRegistratioinGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Update_Click(object sender, EventArgs e)
	{
	}

	private void CheqbookRegistratioinGridFill()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtHistory.Rows.Clear();
			CheqBookHeader chq = new CheqBookHeader();
			chq.OrganizationId = UserAccount.OrganizationId;
			chq.CompanyId = UserAccount.CompanyId;
			dtHistory = CheqBookHeader.GetData(chq);
			if (dtHistory.Rows.Count > 0)
			{
				HashSet<int> existingIds = new HashSet<int>();
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("BankName");
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("CheqBookPrefix");
				dt.Columns.Add("SerialFrom");
				dt.Columns.Add("SerialTo");
				dt.Columns.Add("EntryUser");
				dt.Columns.Add("EntryDate", typeof(DateTime));
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					int id = Conversion.ToInt(dtHistory.Rows[i]["Id"]);
					if (!existingIds.Contains(id))
					{
						dt.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["AccountTitle"], dtHistory.Rows[i]["DocDate"], dtHistory.Rows[i]["CbPrefix"], dtHistory.Rows[i]["CbSrFrom"], dtHistory.Rows[i]["CbSrTo"], dtHistory.Rows[i]["EntryUserName"], dtHistory.Rows[i]["EntryDate"]);
					}
					existingIds.Add(id);
				}
				grdcheq.DataSource = dt;
				grdcheq.RetrieveStructure();
				grdHistorySetting();
			}
			else
			{
				grdcheq.ClearStructure();
			}
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
			grdcheq.RootTable.Columns["Id"].Visible = false;
			grdcheq.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yy";
			grdcheq.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdcheq.RootTable.HeaderLines = 2;
			grdcheq.RootTable.Columns["BankName"].Width = Constants.InventoryConstants.BankName + 25;
			grdcheq.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdcheq.RootTable.Columns["CheqBookPrefix"].Width = 100;
			grdcheq.RootTable.Columns["SerialFrom"].Width = 100;
			grdcheq.RootTable.Columns["SerialTo"].Width = 100;
			grdcheq.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 30;
			grdcheq.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdcheq.RootTable.Columns["BankName"].ColumnType = (ColumnType)5;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCheqBookStatus_Click(object sender, EventArgs e)
	{
		try
		{
			bool Open = false;
			if (UserAccount.RoleName == "Admin")
			{
				Open = true;
			}
			else if (CommonServices.CheckScreenRight("CheqbookStatus"))
			{
				Open = true;
			}
			if (Open)
			{
				new CheqbookStatus(UserAccount).Show();
			}
			else
			{
				MessageBox.Show("You don't have right");
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdcheq).Name);
			if (fs != null)
			{
				grdcheq.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtserialfrom_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtserialto_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCutKey_Click(object sender, EventArgs e)
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Alt+F1", "For Open Cheque Book Status");
			dt.Rows.Add("Ctrl+F5", "For Focus on Bank Account");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
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

	private void AcfrmChequebookRegistration_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (Save.Visible && Save.Enabled && e.Control && e.KeyCode == Keys.S)
			{
				Save_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				New_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Alt && e.KeyCode == Keys.F1)
			{
				btnCheqBookStatus_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbbankac.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdcheq).Focus();
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

	private void grdcheq_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdcheq.CurrentRow;
			if (row != null && (int)row.RowType == 0 && e.Column.Key == "BankName")
			{
				int Id = Conversion.ToInt(row.Cells["Id"].Value);
				DataTable dt = new DataTable();
				dt.Columns.Add("BankName");
				dt.Columns.Add("ChequeNo");
				DataTable resultRows = (from r in dtHistory.AsEnumerable()
					where r.Field<int>("Id") == Id
					select r).CopyToDataTable();
				for (int i = 0; i < resultRows.Rows.Count; i++)
				{
					dt.Rows.Add(resultRows.Rows[i]["AccountTitle"], resultRows.Rows[i]["CheqNo"]);
				}
				GrdAccountDetail.DataSource = dt;
				GrdAccountDetail.RetrieveStructure();
				GrdAccountDetail.RootTable.HeaderLines = 2;
				GrdAccountDetail.RootTable.Columns["BankName"].Width = 250;
				GrdAccountDetail.RootTable.Columns["ChequeNo"].Width = 100;
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
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected O, but got Unknown
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Expected O, but got Unknown
		//IL_07cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d5: Expected O, but got Unknown
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
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.New = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.Save = new System.Windows.Forms.ToolStripButton();
		this.Update = new System.Windows.Forms.ToolStripButton();
		this.btnCheqBookStatus = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.grdcheq = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.GrdAccountDetail = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label8 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.cmbbankac = new UltraCombo();
		this.docdate = new System.Windows.Forms.DateTimePicker();
		this.label7 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.txtserialfrom = new System.Windows.Forms.TextBox();
		this.txtserialto = new System.Windows.Forms.TextBox();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.txtprefix = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.panel2.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdcheq).BeginInit();
		this.panel3.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdAccountDetail).BeginInit();
		this.panel8.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbbankac).BeginInit();
		base.SuspendLayout();
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.miniToolStrip.Location = new System.Drawing.Point(686, 1);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(1810, 27);
		this.miniToolStrip.TabIndex = 8;
		this.panel2.Controls.Add(this.toolStrip2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1305, 28);
		this.panel2.TabIndex = 0;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.New, this.btnRefresh, this.Save, this.Update, this.btnCheqBookStatus, this.btnShortCutKey });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
		this.toolStrip2.Size = new System.Drawing.Size(1305, 27);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.New.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.New.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.New.Name = "New";
		this.New.Size = new System.Drawing.Size(59, 24);
		this.New.Text = "&New";
		this.New.Click += new System.EventHandler(New_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.Save.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.Save.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Save.Name = "Save";
		this.Save.Size = new System.Drawing.Size(64, 24);
		this.Save.Text = "&Save";
		this.Save.Click += new System.EventHandler(Save_Click);
		this.Update.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.Update.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Update.Name = "Update";
		this.Update.Size = new System.Drawing.Size(56, 24);
		this.Update.Text = "&Edit";
		this.Update.Visible = false;
		this.Update.Click += new System.EventHandler(Update_Click);
		this.btnCheqBookStatus.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnCheqBookStatus.Name = "btnCheqBookStatus";
		this.btnCheqBookStatus.Size = new System.Drawing.Size(149, 24);
		this.btnCheqBookStatus.Text = "Cheque Cancel/Open";
		this.btnCheqBookStatus.Click += new System.EventHandler(btnCheqBookStatus_Click);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.grdcheq.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdcheq).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdcheq.FilterMode = (FilterMode)2;
		this.grdcheq.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdcheq).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdcheq.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdcheq).Location = new System.Drawing.Point(0, 28);
		((System.Windows.Forms.Control)(object)this.grdcheq).Name = "grdcheq";
		((ScrollableBase)this.grdcheq).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdcheq).Size = new System.Drawing.Size(828, 426);
		((System.Windows.Forms.Control)(object)this.grdcheq).TabIndex = 0;
		this.grdcheq.TotalRow = (InheritableBoolean)1;
		this.grdcheq.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdcheq).VisualStyle = (VisualStyle)4;
		this.grdcheq.LinkClicked += new ColumnActionEventHandler(grdcheq_LinkClicked);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 28);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1305, 30);
		this.panel3.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(7, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(222, 19);
		this.label23.TabIndex = 26;
		this.label23.Text = "Cheque Book Registration";
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.panel7);
		this.panel1.Controls.Add(this.panel6);
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1305, 652);
		this.panel1.TabIndex = 0;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.GrdAccountDetail);
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel7.Location = new System.Drawing.Point(828, 198);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(370, 454);
		this.panel7.TabIndex = 9;
		this.GrdAccountDetail.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.GrdAccountDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdAccountDetail.FilterMode = (FilterMode)2;
		this.GrdAccountDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdAccountDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdAccountDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdAccountDetail).Location = new System.Drawing.Point(0, 28);
		((System.Windows.Forms.Control)(object)this.GrdAccountDetail).Name = "GrdAccountDetail";
		((ScrollableBase)this.GrdAccountDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdAccountDetail).Size = new System.Drawing.Size(370, 426);
		((System.Windows.Forms.Control)(object)this.GrdAccountDetail).TabIndex = 9;
		this.GrdAccountDetail.TotalRow = (InheritableBoolean)1;
		this.GrdAccountDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdAccountDetail).VisualStyle = (VisualStyle)4;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label9);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(370, 28);
		this.panel8.TabIndex = 8;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(7, 5);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(203, 19);
		this.label9.TabIndex = 27;
		this.label9.Text = "Selected Account Detail";
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdcheq);
		this.panel6.Controls.Add(this.panel4);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel6.Location = new System.Drawing.Point(0, 198);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(828, 454);
		this.panel6.TabIndex = 8;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(828, 28);
		this.panel4.TabIndex = 7;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(7, 5);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(67, 19);
		this.label8.TabIndex = 27;
		this.label8.Text = "History";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(787, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdcheq;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add(this.txtremarks);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbbankac);
		this.panel5.Controls.Add(this.docdate);
		this.panel5.Controls.Add(this.label7);
		this.panel5.Controls.Add(this.label6);
		this.panel5.Controls.Add(this.txtserialfrom);
		this.panel5.Controls.Add(this.txtserialto);
		this.panel5.Controls.Add(this.txtdocno);
		this.panel5.Controls.Add(this.txtprefix);
		this.panel5.Controls.Add(this.label4);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Controls.Add(this.label5);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 58);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1305, 140);
		this.panel5.TabIndex = 6;
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.White;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(11, 92);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(52, 15);
		this.label3.TabIndex = 19;
		this.label3.Text = "Remarks";
		this.txtremarks.BackColor = System.Drawing.Color.White;
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtremarks.Location = new System.Drawing.Point(97, 88);
		this.txtremarks.Multiline = true;
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(363, 46);
		this.txtremarks.TabIndex = 18;
		this.cmbbankac.AllowNull = (DefaultableBoolean)1;
		this.cmbbankac.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbbankac.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbbankac.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbbankac).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbbankac).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbbankac).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbbankac).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbankac).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.cmbbankac).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbankac).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbbankac).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbbankac).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbbankac).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbbankac).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbbankac).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbbankac).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbankac).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbbankac).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbbankac).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbbankac).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbankac).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbbankac).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbbankac).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbbankac).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbbankac).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbbankac).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbbankac).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbbankac).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbbankac).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbbankac).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbbankac).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbbankac.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbbankac).Location = new System.Drawing.Point(97, 8);
		((System.Windows.Forms.Control)(object)this.cmbbankac).Name = "cmbbankac";
		((System.Windows.Forms.Control)(object)this.cmbbankac).Size = new System.Drawing.Size(363, 26);
		((System.Windows.Forms.Control)(object)this.cmbbankac).TabIndex = 0;
		this.docdate.CustomFormat = "dd-MMM-yy";
		this.docdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.docdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.docdate.Location = new System.Drawing.Point(97, 37);
		this.docdate.Name = "docdate";
		this.docdate.Size = new System.Drawing.Size(125, 23);
		this.docdate.TabIndex = 1;
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.White;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(224, 67);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(57, 15);
		this.label7.TabIndex = 9;
		this.label7.Text = "Serial To*";
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.White;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(11, 67);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(72, 15);
		this.label6.TabIndex = 10;
		this.label6.Text = "Serial From*";
		this.txtserialfrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtserialfrom.Location = new System.Drawing.Point(97, 63);
		this.txtserialfrom.Name = "txtserialfrom";
		this.txtserialfrom.Size = new System.Drawing.Size(125, 23);
		this.txtserialfrom.TabIndex = 3;
		this.txtserialfrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtserialfrom_KeyPress);
		this.txtserialto.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtserialto.Location = new System.Drawing.Point(282, 63);
		this.txtserialto.Name = "txtserialto";
		this.txtserialto.Size = new System.Drawing.Size(178, 23);
		this.txtserialto.TabIndex = 4;
		this.txtserialto.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtserialto_KeyPress);
		this.txtdocno.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(929, 193);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.Size = new System.Drawing.Size(163, 23);
		this.txtdocno.TabIndex = 17;
		this.txtprefix.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtprefix.Location = new System.Drawing.Point(282, 37);
		this.txtprefix.Name = "txtprefix";
		this.txtprefix.Size = new System.Drawing.Size(178, 23);
		this.txtprefix.TabIndex = 2;
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.White;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(926, 162);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(89, 17);
		this.label4.TabIndex = 11;
		this.label4.Text = "Document No";
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.White;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(11, 41);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(57, 15);
		this.label2.TabIndex = 12;
		this.label2.Text = "Doc Date";
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.White;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(11, 14);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(81, 15);
		this.label1.TabIndex = 13;
		this.label1.Text = "Bank Account";
		this.label5.AutoSize = true;
		this.label5.BackColor = System.Drawing.Color.White;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(224, 41);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(40, 15);
		this.label5.TabIndex = 14;
		this.label5.Text = "Prefix ";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1305, 652);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "AcfrmChequebookRegistration";
		this.Text = "FrmChequebookRegistration";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(AcfrmChequebookRegistration_FormClosing);
		base.Load += new System.EventHandler(FrmChequebookRegistration_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(AcfrmChequebookRegistration_KeyDown);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdcheq).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdAccountDetail).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbbankac).EndInit();
		base.ResumeLayout(false);
	}
}
