using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Lookups;

public class GenerateExportInvoiceNos : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtHistory = new DataTable();

	private int RecId;

	private int UpdateIndex;

	private int[] AryColWidth;

	private IContainer components;

	private Panel panel1;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private Panel panel2;

	internal GridEX Grd;

	private Button BtnCancel;

	private Button BtnSavef;

	private Button btnUpdateF;

	private Panel panel3;

	private Panel panel6;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label23;

	private Panel panel4;

	private Label label3;

	private TextBox txtSerialTo;

	private Label label6;

	private TextBox txtPrefix;

	private Label label4;

	private TextBox txtSerialFrom;

	private Label label2;

	internal GridEX grdDetail;

	private Panel panel5;

	private Label label1;

	private CtrlGrdBar ctrlGrdBar2;

	private TextBox txtMainId;

	internal UltraCombo CmbPrefixType;

	private Label label5;

	private ToolStripButton btnDefinePrefixType;

	public GenerateExportInvoiceNos(UserAccount Puser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = Puser;
	}

	private void GenerateExportInvoiceNos_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(390, 700);
			BackColor = Color.Teal;
			BindPrefixType();
			BindHistory();
			btnUpdate.Visible = false;
			btnsave.Visible = true;
			btnUpdateF.Visible = false;
			BtnSavef.Visible = true;
			CmbPrefixType.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindPrefixType()
	{
		try
		{
			DataTable dt = ExportPrefixType.FormHistory();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbPrefixType, "Id", "PrefixDescription", "Prefix Type", ZeroIndex: false);
				return;
			}
			((Control)(object)CmbPrefixType).Text = string.Empty;
			((UltraGridBase)CmbPrefixType).DataSource = null;
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
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			BtnSavef.Visible = true;
			btnUpdateF.Visible = false;
			txtPrefix.Text = string.Empty;
			txtSerialFrom.Text = string.Empty;
			txtSerialTo.Text = string.Empty;
			BindHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCancel_Click(object sender, EventArgs e)
	{
		Close();
	}

	private bool FormValiadation()
	{
		if (txtPrefix.Text == string.Empty)
		{
			MessageBox.Show("PrefixField Is Required");
			txtPrefix.Focus();
			return false;
		}
		if (txtSerialFrom.Text == string.Empty && Conversion.ToInt((object)txtSerialFrom.Text) == 0)
		{
			MessageBox.Show("SerialFrom Field Is Required");
			txtSerialFrom.Focus();
			return false;
		}
		if (txtSerialTo.Text == string.Empty && Conversion.ToInt((object)txtSerialTo.Text) == 0)
		{
			MessageBox.Show("Serial To Field Is Required");
			txtSerialTo.Focus();
			return false;
		}
		if (Conversion.ToInt((object)txtSerialTo.Text) < Conversion.ToInt((object)txtSerialFrom.Text))
		{
			MessageBox.Show("Serial To Can not Be Less than Sreial From");
			txtSerialTo.Focus();
			return false;
		}
		return true;
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

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Expected O, but got Unknown
		try
		{
			if (!FormValiadation())
			{
				return;
			}
			GenerateExportInvoiceNos obj = new GenerateExportInvoiceNos();
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
			obj.CompanyId = UserAccount.CompanyId;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyUserId = UserAccount.ID;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.GenerateExportInvoiceNosList = new List<GenerateExportInvoiceNos>();
			for (int i = Conversion.ToInt((object)txtSerialFrom.Text); i <= Conversion.ToInt((object)txtSerialTo.Text); i++)
			{
				GenerateExportInvoiceNos detail = new GenerateExportInvoiceNos();
				detail.CompanyId = UserAccount.CompanyId;
				detail.OrganizationId = UserAccount.OrganizationId;
				detail.EntryUserId = UserAccount.ID;
				detail.ModifyUserId = UserAccount.ID;
				detail.EntryDate = DateTime.Now;
				detail.ModifyDate = DateTime.Now;
				detail.IsActive = true;
				detail.PrefixTypeId = Conversion.ToInt(CmbPrefixType.Value);
				detail.SerialFrom = Conversion.ToInt((object)txtSerialFrom.Text);
				detail.SerialTo = Conversion.ToInt((object)txtSerialTo.Text);
				detail.PreFix = Conversion.ToString((object)txtPrefix.Text);
				detail.MainId = Conversion.ToInt((object)GenerateExportInvoiceNos.GetMainId(UserAccount.OrganizationId, UserAccount.CompanyId));
				string FinalNo = Conversion.ToString((object)txtPrefix.Text) + "-" + i;
				detail.FinalInvoiceNo = FinalNo;
				obj.GenerateExportInvoiceNosList.Add(detail);
			}
			int Id = GenerateExportInvoiceNos.save(obj);
			if (RecId == 0 && Id > 0)
			{
				MessageBox.Show("Save Successfully");
			}
			else if (RecId > 0)
			{
				MessageBox.Show("Update Successfully");
			}
			Reset();
			BindHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteAndInsert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		try
		{
			if (!FormValiadation())
			{
				return;
			}
			GenerateExportInvoiceNos obj = new GenerateExportInvoiceNos();
			if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				obj.Id = RecId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.EntryUserId = UserAccount.ID;
				obj.ModifyUserId = UserAccount.ID;
				obj.MainId = Conversion.ToInt((object)txtMainId.Text);
				obj.EntryDate = DateTime.Now;
				obj.ModifyDate = DateTime.Now;
				obj.GenerateExportInvoiceNosList = new List<GenerateExportInvoiceNos>();
				for (int i = Conversion.ToInt((object)txtSerialFrom.Text); i <= Conversion.ToInt((object)txtSerialTo.Text); i++)
				{
					GenerateExportInvoiceNos detail = new GenerateExportInvoiceNos();
					detail.CompanyId = UserAccount.CompanyId;
					detail.OrganizationId = UserAccount.OrganizationId;
					detail.EntryUserId = UserAccount.ID;
					detail.ModifyUserId = UserAccount.ID;
					detail.EntryDate = DateTime.Now;
					detail.ModifyDate = DateTime.Now;
					detail.IsActive = true;
					detail.PrefixTypeId = Conversion.ToInt(CmbPrefixType.Value);
					detail.SerialFrom = Conversion.ToInt((object)txtSerialFrom.Text);
					detail.SerialTo = Conversion.ToInt((object)txtSerialTo.Text);
					detail.PreFix = Conversion.ToString((object)txtPrefix.Text);
					detail.MainId = Conversion.ToInt((object)txtMainId.Text);
					string FinalNo = Conversion.ToString((object)txtPrefix.Text) + "-" + i;
					detail.FinalInvoiceNo = FinalNo;
					obj.GenerateExportInvoiceNosList.Add(detail);
				}
				GenerateExportInvoiceNos.save(obj);
				MessageBox.Show("Update Successfully");
				Reset();
				BindHistory();
			}
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
			DeleteAndInsert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadById()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = Grd.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				RecId = Conversion.ToInt(item.Cells["Id"].Value);
				CmbPrefixType.Value = Conversion.ToString(item.Cells["PrefixTypeId"].Value);
				txtMainId.Text = Conversion.ToString(item.Cells["MainId"].Value);
				txtPrefix.Text = Conversion.ToString(item.Cells["Prefix"].Value);
				txtSerialFrom.Text = Conversion.ToString(item.Cells["SerialFrom"].Value);
				txtSerialTo.Text = Conversion.ToString(item.Cells["SerialTo"].Value);
				btnUpdate.Visible = true;
				btnsave.Visible = false;
				btnUpdateF.Visible = true;
				BtnSavef.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindHistory()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			GenerateExportInvoiceNos obj = new GenerateExportInvoiceNos();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtHistory = GenerateExportInvoiceNos.GetAll(obj);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("MainId", typeof(int));
				dt.Columns.Add("PrefixTypeId", typeof(int));
				dt.Columns.Add("PrefixType", typeof(string));
				dt.Columns.Add("Prefix", typeof(string));
				dt.Columns.Add("SerialFrom", typeof(int));
				dt.Columns.Add("SerialTo", typeof(int));
				dt.Columns.Add("FinalInvoiceNo", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser", typeof(string));
				int i;
				for (i = 0; i < dtHistory.Rows.Count; i++)
				{
					if (!dt.AsEnumerable().Any((DataRow dr) => (int)dr["MainId"] == Conversion.ToInt(dtHistory.Rows[i]["MainId"])))
					{
						dt.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["MainId"], dtHistory.Rows[i]["PrefixTypeId"], dtHistory.Rows[i]["PrefixDescription"], dtHistory.Rows[i]["Prefix"], dtHistory.Rows[i]["SerialFrom"], dtHistory.Rows[i]["SerialTo"], dtHistory.Rows[i]["FinalInvoiceNo"], Conversion.ToDateTime(dtHistory.Rows[i]["EntryDate"]).ToShortDateString(), dtHistory.Rows[i]["EntryUser"]);
					}
				}
				Grd.DataSource = dt;
				Grd.RetrieveStructure();
				grdSettings();
			}
			else
			{
				Grd.ClearStructure();
				grdDetail.ClearStructure();
			}
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
			Grd.RootTable.Columns["Id"].Visible = false;
			Grd.RootTable.Columns["MainId"].Visible = false;
			Grd.RootTable.Columns["PrefixTypeId"].Visible = false;
			Grd.RootTable.Columns["SerialFrom"].Visible = false;
			Grd.RootTable.Columns["SerialTo"].Visible = false;
			Grd.RootTable.Columns.Add("Edit");
			Grd.RootTable.Columns["Edit"].Key = "Edit";
			Grd.RootTable.Columns["Edit"].Caption = "Edit";
			Grd.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			Grd.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			Grd.RootTable.Columns["Edit"].ButtonText = "Edit";
			Grd.RootTable.Columns["Edit"].Width = 40;
			Grd.RootTable.Columns["Edit"].Position = 0;
			Grd.FrozenColumns = 1;
			CommonServices.GridAutoAdjustment(AryColWidth, Grd);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Grd_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			ReadById();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		if (e.Column.Key == "Edit")
		{
			ReadById();
		}
	}

	private void Grd_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = Grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				DataTable dtDetail = new DataTable();
				dtDetail.Columns.Add("Id", typeof(int));
				dtDetail.Columns.Add("MainId", typeof(int));
				dtDetail.Columns.Add("Prefix", typeof(string));
				dtDetail.Columns.Add("SerialFrom", typeof(int));
				dtDetail.Columns.Add("SerialTo", typeof(int));
				dtDetail.Columns.Add("FinalInvoiceNo", typeof(string));
				DataTable resultRows = (from row in dtHistory.AsEnumerable()
					where row.Field<int>("MainId") == Conversion.ToInt(item.Cells["MainId"].Value)
					select row).CopyToDataTable();
				for (int i = 0; i < resultRows.Rows.Count; i++)
				{
					dtDetail.Rows.Add(resultRows.Rows[i]["Id"], resultRows.Rows[i]["MainId"], resultRows.Rows[i]["Prefix"], resultRows.Rows[i]["SerialFrom"], resultRows.Rows[i]["SerialTo"], resultRows.Rows[i]["FinalInvoiceNo"]);
				}
				grdDetail.DataSource = dtDetail;
				grdDetail.RetrieveStructure();
				GridDetailSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailSettings()
	{
		try
		{
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["MainId"].Visible = false;
			CommonServices.GridAutoAdjustment(AryColWidth, grdDetail);
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)Grd).Name);
			if (fs != null)
			{
				Grd.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDetail).Name);
			if (fs != null)
			{
				grdDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void txtSerialFrom_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtSerialTo_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDefinePrefixType_Click(object sender, EventArgs e)
	{
		try
		{
			DefineExportPrefixType defineExportPrefixType = new DefineExportPrefixType(UserAccount);
			defineExportPrefixType.StartPosition = FormStartPosition.CenterScreen;
			defineExportPrefixType.ShowDialog();
			defineExportPrefixType.BringToFront();
			BindPrefixType();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected O, but got Unknown
		//IL_075a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0764: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout Grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Lookups.GenerateExportInvoiceNos));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.Grd = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel2 = new System.Windows.Forms.Panel();
		this.CmbPrefixType = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.txtMainId = new System.Windows.Forms.TextBox();
		this.txtSerialTo = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.txtPrefix = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.txtSerialFrom = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.btnUpdateF = new System.Windows.Forms.Button();
		this.BtnCancel = new System.Windows.Forms.Button();
		this.BtnSavef = new System.Windows.Forms.Button();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDefinePrefixType = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.Grd).BeginInit();
		this.panel6.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPrefixType).BeginInit();
		this.panel4.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.AutoScroll = true;
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel6);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(459, 672);
		this.panel1.TabIndex = 0;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.Grd);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 203);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(459, 469);
		this.panel3.TabIndex = 10;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 233);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(459, 236);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 11;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Controls.Add(this.ctrlGrdBar2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 202);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(459, 31);
		this.panel5.TabIndex = 10;
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(7, 4);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(253, 19);
		this.label1.TabIndex = 25;
		this.label1.Text = "Detail Of Selected Above Row";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(410, 3);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar2.TabIndex = 26;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.Grd.AllowEdit = (InheritableBoolean)2;
		Grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.Grd.DesignTimeLayout = Grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.Grd).Dock = System.Windows.Forms.DockStyle.Top;
		this.Grd.DynamicFiltering = true;
		this.Grd.FilterMode = (FilterMode)2;
		this.Grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.Grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.Grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.Grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.Grd).Name = "Grd";
		((ScrollableBase)this.Grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.Grd).Size = new System.Drawing.Size(459, 202);
		((System.Windows.Forms.Control)(object)this.Grd).TabIndex = 8;
		this.Grd.TotalRow = (InheritableBoolean)1;
		this.Grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.Grd).VisualStyle = (VisualStyle)4;
		this.Grd.ColumnButtonClick += new ColumnActionEventHandler(Grd_ColumnButtonClick);
		this.Grd.SelectionChanged += new System.EventHandler(Grd_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.Grd).DoubleClick += new System.EventHandler(Grd_DoubleClick);
		this.panel6.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel6.Controls.Add(this.label23);
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 172);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(459, 31);
		this.panel6.TabIndex = 9;
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(8, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(67, 19);
		this.label23.TabIndex = 25;
		this.label23.Text = "History";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(410, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.Grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar1.TabIndex = 26;
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPrefixType);
		this.panel2.Controls.Add(this.label5);
		this.panel2.Controls.Add(this.txtMainId);
		this.panel2.Controls.Add(this.txtSerialTo);
		this.panel2.Controls.Add(this.label6);
		this.panel2.Controls.Add(this.txtPrefix);
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add(this.txtSerialFrom);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.btnUpdateF);
		this.panel2.Controls.Add(this.BtnCancel);
		this.panel2.Controls.Add(this.BtnSavef);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 27);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(459, 145);
		this.panel2.TabIndex = 0;
		this.CmbPrefixType.AllowNull = (DefaultableBoolean)1;
		this.CmbPrefixType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPrefixType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPrefixType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPrefixType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbPrefixType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPrefixType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPrefixType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPrefixType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance9;
		((SpecialBoxBase)((UltraGridBase)this.CmbPrefixType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance10).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance10).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPrefixType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbPrefixType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPrefixType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPrefixType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPrefixType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbPrefixType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPrefixType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPrefixType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance3).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPrefixType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbPrefixType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPrefixType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPrefixType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPrefixType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbPrefixType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPrefixType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPrefixType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbPrefixType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPrefixType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbPrefixType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPrefixType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPrefixType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPrefixType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbPrefixType).Location = new System.Drawing.Point(96, 34);
		((System.Windows.Forms.Control)(object)this.CmbPrefixType).Name = "CmbPrefixType";
		((System.Windows.Forms.Control)(object)this.CmbPrefixType).Size = new System.Drawing.Size(191, 26);
		((System.Windows.Forms.Control)(object)this.CmbPrefixType).TabIndex = 0;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(13, 40);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(65, 15);
		this.label5.TabIndex = 612;
		this.label5.Text = "Prefix Type";
		this.txtMainId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMainId.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMainId.Location = new System.Drawing.Point(291, 88);
		this.txtMainId.Name = "txtMainId";
		this.txtMainId.Size = new System.Drawing.Size(75, 23);
		this.txtMainId.TabIndex = 610;
		this.txtMainId.Visible = false;
		this.txtSerialTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSerialTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSerialTo.Location = new System.Drawing.Point(96, 117);
		this.txtSerialTo.Name = "txtSerialTo";
		this.txtSerialTo.Size = new System.Drawing.Size(191, 23);
		this.txtSerialTo.TabIndex = 3;
		this.txtSerialTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSerialTo_KeyPress);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(13, 120);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(58, 17);
		this.label6.TabIndex = 609;
		this.label6.Text = "Serial To";
		this.txtPrefix.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPrefix.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPrefix.Location = new System.Drawing.Point(96, 62);
		this.txtPrefix.Name = "txtPrefix";
		this.txtPrefix.Size = new System.Drawing.Size(191, 23);
		this.txtPrefix.TabIndex = 1;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(13, 65);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(49, 17);
		this.label4.TabIndex = 607;
		this.label4.Text = "Pre Fix";
		this.txtSerialFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSerialFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSerialFrom.Location = new System.Drawing.Point(96, 89);
		this.txtSerialFrom.Name = "txtSerialFrom";
		this.txtSerialFrom.Size = new System.Drawing.Size(191, 23);
		this.txtSerialFrom.TabIndex = 2;
		this.txtSerialFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSerialFrom_KeyPress);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(13, 92);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(76, 17);
		this.label2.TabIndex = 605;
		this.label2.Text = "Serial From";
		this.panel4.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(459, 31);
		this.panel4.TabIndex = 603;
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(8, 5);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(243, 19);
		this.label3.TabIndex = 25;
		this.label3.Text = "Generate Export Invoice Nos";
		this.btnUpdateF.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateF.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateF.ForeColor = System.Drawing.Color.White;
		this.btnUpdateF.Location = new System.Drawing.Point(291, 113);
		this.btnUpdateF.Name = "btnUpdateF";
		this.btnUpdateF.Size = new System.Drawing.Size(75, 28);
		this.btnUpdateF.TabIndex = 4;
		this.btnUpdateF.Text = "Update";
		this.btnUpdateF.UseVisualStyleBackColor = false;
		this.btnUpdateF.Visible = false;
		this.btnUpdateF.Click += new System.EventHandler(btnUpdate_Click);
		this.BtnCancel.BackColor = System.Drawing.Color.Teal;
		this.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancel.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancel.ForeColor = System.Drawing.Color.White;
		this.BtnCancel.Location = new System.Drawing.Point(291, 58);
		this.BtnCancel.Name = "BtnCancel";
		this.BtnCancel.Size = new System.Drawing.Size(75, 28);
		this.BtnCancel.TabIndex = 5;
		this.BtnCancel.Text = "Cancel";
		this.BtnCancel.UseVisualStyleBackColor = false;
		this.BtnCancel.Click += new System.EventHandler(BtnCancel_Click);
		this.BtnSavef.BackColor = System.Drawing.Color.Teal;
		this.BtnSavef.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnSavef.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnSavef.ForeColor = System.Drawing.Color.White;
		this.BtnSavef.Location = new System.Drawing.Point(291, 113);
		this.BtnSavef.Name = "BtnSavef";
		this.BtnSavef.Size = new System.Drawing.Size(75, 28);
		this.BtnSavef.TabIndex = 3;
		this.BtnSavef.Text = "Save";
		this.BtnSavef.UseVisualStyleBackColor = false;
		this.BtnSavef.Click += new System.EventHandler(btnsave_Click);
		this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnsave, this.btnUpdate, this.btnDefinePrefixType });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(459, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
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
		this.btnDefinePrefixType.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnDefinePrefixType.Image = (System.Drawing.Image)resources.GetObject("btnDefinePrefixType.Image");
		this.btnDefinePrefixType.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDefinePrefixType.Name = "btnDefinePrefixType";
		this.btnDefinePrefixType.Size = new System.Drawing.Size(116, 24);
		this.btnDefinePrefixType.Text = "Define Prefix Type";
		this.btnDefinePrefixType.Click += new System.EventHandler(btnDefinePrefixType_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(459, 672);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "GenerateExportInvoiceNos";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "GenerateExportInvoiceNos";
		base.Load += new System.EventHandler(GenerateExportInvoiceNos_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.Grd).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPrefixType).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
