// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Lab;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Lab;
using Architecture.WinApp.Common;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Lab
{

public class InvLabPurchaseReport : Form
{
	public UserAccount UserAccount = new UserAccount();

	private int RecId;

	private string fileSavePath = "";

	private string ofd = "";

	private string fileSavePathcookingpic = "";

	private string ofdcookingpic = "";

	private DropDownBind DDL = new DropDownBind();

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private Panel panel3;

	private Panel panel4;

	internal GridEX grdhistory;

	private Panel panel8;

	private TextBox txtgpnof;

	private TextBox txtgpnot;

	private Label label45;

	private Label label44;

	private Panel panel7;

	private Label label43;

	private ToolStrip tsItemTaxSchedule;

	private ToolStripButton btnnew;

	private DateTimePicker txtdatef;

	private Label label3;

	private Label label2;

	private Label label1;

	private DateTimePicker txtdatet;

	private Button btnsearch;

	private Panel panel5;

	internal GridEX griddetail;

	private Panel panel6;

	private Label label4;

	private Label label5;

	private PictureBox cookingpic;

	private PictureBox sampleanalysispicture;

	private Button btnimgpreviewcookingpic;

	private Button btnimgpreanalysispic;

	internal UltraCombo cmbsupplier;

	private ToolStripButton btnAllRecord;

	private CtrlGrdBar ctrlGrdBar1;

	public InvLabPurchaseReport(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void cmbsupplierfill()
	{
		try
		{
			DataTable dt = CommonServices.SupplierCustomerGetforComboServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbsupplier, "Id", "CompanyName", "Supplier Customer", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void historygridfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = txtdatef.Value,
				ToDate = txtdatet.Value,
				SupplierCustomerId = Conversion.ToInt(cmbsupplier.Value),
				GpSrNoF = Conversion.ToInt((object)txtgpnof.Text.Trim()),
				GpSrNoT = Conversion.ToInt((object)txtgpnot.Text.Trim())
			};
			DataTable dtPurchaseReport = new DataTable();
			dtPurchaseReport = InvLabAnalysisPurchaseHeader.PurchaseReport(val);
			if (dtPurchaseReport.Rows.Count > 0)
			{
				DataTable table = new DataTable();
				table.Clear();
				table.Columns.Add("Id");
				table.Columns.Add("DocumentNo");
				table.Columns.Add("SupplierName");
				table.Columns.Add("Analysis Item");
				table.Columns.Add("GatePass#");
				table.Columns.Add("BuiltyNo");
				table.Columns.Add("Vehicle No");
				table.Columns.Add("Crop Year");
				table.Columns.Add("Remarks");
				table.Columns.Add("AnalysisPicture");
				table.Columns.Add("CookingPicture");
				table.Columns.Add("AnalystName");
				table.Clear();
				grdhistory.DataSource = table;
				for (int i = 0; i < dtPurchaseReport.Rows.Count; i++)
				{
					table.Rows.Add(dtPurchaseReport.Rows[i]["Id"], dtPurchaseReport.Rows[i]["DocNo"], dtPurchaseReport.Rows[i]["SupplierName"], dtPurchaseReport.Rows[i]["ItemName"], dtPurchaseReport.Rows[i]["GpSrNo"], dtPurchaseReport.Rows[i]["BiltyNo"], dtPurchaseReport.Rows[i]["VehicleNo"], dtPurchaseReport.Rows[i]["Crop"], dtPurchaseReport.Rows[i]["RemarksHeader"], dtPurchaseReport.Rows[i]["Pic1Pathe"], dtPurchaseReport.Rows[i]["CookingPic"], dtPurchaseReport.Rows[i]["AnalystName"]);
				}
				grdhistory.DataSource = table;
				grdhistory.RetrieveStructure();
				gridsetting();
			}
			else
			{
				grdhistory.ClearStructure();
				griddetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridsetting()
	{
		try
		{
			grdhistory.RootTable.Columns.Add("Detail");
			grdhistory.RootTable.Columns["Detail"].Caption = "Detail";
			grdhistory.RootTable.Columns["Detail"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Detail"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Detail"].ButtonText = "Detail";
			grdhistory.RootTable.Columns["Detail"].Width = 50;
			grdhistory.RootTable.Columns.Add("View");
			grdhistory.RootTable.Columns["View"].Caption = "Slip";
			grdhistory.RootTable.Columns["View"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["View"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["View"].ButtonText = "Slip";
			grdhistory.RootTable.Columns["View"].Width = 50;
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns["AnalysisPicture"].Visible = false;
			grdhistory.RootTable.Columns["CookingPicture"].Visible = false;
			grdhistory.RootTable.Columns["DocumentNo"].Width = 70;
			grdhistory.RootTable.Columns["Analysis Item"].Width = 300;
			grdhistory.RootTable.Columns["SupplierName"].Width = 300;
			grdhistory.RootTable.Columns["GatePass#"].Width = 70;
			grdhistory.RootTable.Columns["BuiltyNo"].Width = 70;
			grdhistory.RootTable.Columns["Vehicle No"].Width = 100;
			grdhistory.RootTable.Columns["Crop Year"].Width = 120;
			grdhistory.RootTable.Columns["Remarks"].Width = 300;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Expected O, but got Unknown
		try
		{
			if (e.Column.Key == "View")
			{
				DataTable dt = InvLabAnalysisPurchaseHeader.RptInvLabPurchaseAnalysisSlip653(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Conversion.ToInt(grdhistory.CurrentRow.Cells[0].Value)
				});
				if (dt.Rows.Count == 0 || dt == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "653-RptInvLabPurchaseAnalysisSlip.rpt");
			}
			if (!(e.Column.Key == "Detail") || grdhistory.SelectedItems.Count <= 0)
			{
				return;
			}
			GridEXRow item = grdhistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			InvLabAnalysisPurchaseHeader cat = new InvLabAnalysisPurchaseHeader();
			RecId = Conversion.ToInt(grdhistory.CurrentRow.Cells[0].Value);
			DataTable dtdetail = new DataTable();
			dtdetail = InvLabAnalysisPurchaseHeader.GetById(RecId);
			if (dtdetail.Rows.Count > 0)
			{
				cat.Id = RecId;
				DataTable Detailtable = new DataTable();
				Detailtable.Columns.Add("Perameter Description");
				Detailtable.Columns.Add("Min Value");
				Detailtable.Columns.Add("Max Value");
				Detailtable.Columns.Add("Analysis Result");
				Detailtable.Columns.Add("Remarks Detail");
				Detailtable.Clear();
				griddetail.DataSource = Detailtable;
				for (int i = 0; i < dtdetail.Rows.Count; i++)
				{
					Detailtable.Rows.Add(dtdetail.Rows[i]["AnalysisParameterDescription"], dtdetail.Rows[i]["MinValue"], dtdetail.Rows[i]["MaxValue"], dtdetail.Rows[i]["InAnalysisResult"], dtdetail.Rows[i]["RemarksDetail"]);
				}
				fileSavePath = dtdetail.Rows[0]["Pic1Pathe"].ToString();
				fileSavePathcookingpic = dtdetail.Rows[0]["CookingPic"].ToString();
				if (fileSavePath != "")
				{
					if (File.Exists(fileSavePath))
					{
						sampleanalysispicture.Image = Image.FromFile(dtdetail.Rows[0]["Pic1Pathe"].ToString());
					}
					else
					{
						fileSavePath = "";
						sampleanalysispicture.Image = null;
						sampleanalysispicture.Invalidate();
					}
				}
				else
				{
					fileSavePath = "";
					sampleanalysispicture.Image = null;
					sampleanalysispicture.Invalidate();
				}
				if (fileSavePathcookingpic != "")
				{
					if (File.Exists(fileSavePathcookingpic))
					{
						cookingpic.Image = Image.FromFile(dtdetail.Rows[0]["CookingPic"].ToString());
					}
					else
					{
						fileSavePathcookingpic = "";
						cookingpic.Image = null;
						cookingpic.Invalidate();
					}
				}
				else
				{
					fileSavePathcookingpic = "";
					cookingpic.Image = null;
					cookingpic.Invalidate();
				}
				griddetail.DataSource = Detailtable;
				griddetail.RetrieveStructure();
				griddetailsetting();
			}
			else
			{
				griddetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void griddetailsetting()
	{
		try
		{
			griddetail.RootTable.Columns["Perameter Description"].Width = 200;
			griddetail.RootTable.Columns["Min Value"].Width = 70;
			griddetail.RootTable.Columns["Max Value"].Width = 70;
			griddetail.RootTable.Columns["Analysis Result"].Width = 70;
			griddetail.RootTable.Columns["Remarks Detail"].Width = 250;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsearch_Click(object sender, EventArgs e)
	{
		try
		{
			historygridfill();
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
			txtgpnof.Text = string.Empty;
			txtgpnot.Text = string.Empty;
			txtdatef.Text = string.Empty;
			txtdatet.Text = string.Empty;
			((Control)(object)cmbsupplier).Text = "";
			txtdatef.Focus();
			griddetail.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnprint_Click(object sender, EventArgs e)
	{
	}

	private void InvLabPurchaseReport_Load(object sender, EventArgs e)
	{
		try
		{
			cmbsupplierfill();
			txtdatef.Focus();
			txtdatef.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			historygridfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnimgpreanalysispic_Click(object sender, EventArgs e)
	{
		try
		{
			LabImagesPreview labImagesPreview = new LabImagesPreview();
			InvLabPurchaseAnalysis.imgpreview = sampleanalysispicture.Image;
			labImagesPreview.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnimgpreviewcookingpic_Click(object sender, EventArgs e)
	{
		try
		{
			LabImagesPreview labImagesPreview = new LabImagesPreview();
			InvLabPurchaseAnalysis.imgpreview = cookingpic.Image;
			labImagesPreview.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAllRecord_Click(object sender, EventArgs e)
	{
		try
		{
			historygridfill();
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
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdhistory).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdhistory).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				grdhistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void InvLabPurchaseReport_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		//IL_05db: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e5: Expected O, but got Unknown
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
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Lab.InvLabPurchaseReport));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdhistory = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.btnimgpreviewcookingpic = new System.Windows.Forms.Button();
		this.btnimgpreanalysispic = new System.Windows.Forms.Button();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.cookingpic = new System.Windows.Forms.PictureBox();
		this.sampleanalysispicture = new System.Windows.Forms.PictureBox();
		this.panel5 = new System.Windows.Forms.Panel();
		this.griddetail = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.cmbsupplier = new UltraCombo();
		this.btnsearch = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtdatet = new System.Windows.Forms.DateTimePicker();
		this.txtdatef = new System.Windows.Forms.DateTimePicker();
		this.txtgpnof = new System.Windows.Forms.TextBox();
		this.txtgpnot = new System.Windows.Forms.TextBox();
		this.label45 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label43 = new System.Windows.Forms.Label();
		this.tsItemTaxSchedule = new System.Windows.Forms.ToolStrip();
		this.btnAllRecord = new System.Windows.Forms.ToolStripButton();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cookingpic).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.sampleanalysispicture).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.griddetail).BeginInit();
		this.panel4.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbsupplier).BeginInit();
		this.panel7.SuspendLayout();
		this.tsItemTaxSchedule.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1207, 738);
		this.panel1.TabIndex = 0;
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1207, 738);
		this.panel2.TabIndex = 0;
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Controls.Add(this.panel4);
		this.panel3.Controls.Add(this.panel8);
		this.panel3.Controls.Add(this.panel7);
		this.panel3.Controls.Add(this.tsItemTaxSchedule);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1207, 738);
		this.panel3.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1156, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdhistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar1.TabIndex = 83;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grdhistory.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupByBoxVisible = false;
		this.grdhistory.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdhistory).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1207, 281);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 0;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.panel6.BackColor = System.Drawing.Color.White;
		this.panel6.Controls.Add(this.btnimgpreviewcookingpic);
		this.panel6.Controls.Add(this.btnimgpreanalysispic);
		this.panel6.Controls.Add(this.label4);
		this.panel6.Controls.Add(this.label5);
		this.panel6.Controls.Add(this.cookingpic);
		this.panel6.Controls.Add(this.sampleanalysispicture);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(735, 412);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(472, 326);
		this.panel6.TabIndex = 82;
		this.btnimgpreviewcookingpic.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnimgpreviewcookingpic.Location = new System.Drawing.Point(322, 201);
		this.btnimgpreviewcookingpic.Name = "btnimgpreviewcookingpic";
		this.btnimgpreviewcookingpic.Size = new System.Drawing.Size(76, 27);
		this.btnimgpreviewcookingpic.TabIndex = 39;
		this.btnimgpreviewcookingpic.Text = "Preview";
		this.btnimgpreviewcookingpic.UseVisualStyleBackColor = true;
		this.btnimgpreviewcookingpic.Click += new System.EventHandler(btnimgpreviewcookingpic_Click);
		this.btnimgpreanalysispic.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnimgpreanalysispic.Location = new System.Drawing.Point(79, 201);
		this.btnimgpreanalysispic.Name = "btnimgpreanalysispic";
		this.btnimgpreanalysispic.Size = new System.Drawing.Size(76, 27);
		this.btnimgpreanalysispic.TabIndex = 38;
		this.btnimgpreanalysispic.Text = "Preview";
		this.btnimgpreanalysispic.UseVisualStyleBackColor = true;
		this.btnimgpreanalysispic.Click += new System.EventHandler(btnimgpreanalysispic_Click);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Times New Roman", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(310, 9);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(71, 15);
		this.label4.TabIndex = 33;
		this.label4.Text = "Cooking Pic";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Times New Roman", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(81, 9);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(74, 15);
		this.label5.TabIndex = 32;
		this.label5.Text = "Analysis Pic";
		this.cookingpic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.cookingpic.Location = new System.Drawing.Point(254, 27);
		this.cookingpic.Name = "cookingpic";
		this.cookingpic.Size = new System.Drawing.Size(213, 168);
		this.cookingpic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.cookingpic.TabIndex = 30;
		this.cookingpic.TabStop = false;
		this.sampleanalysispicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.sampleanalysispicture.Location = new System.Drawing.Point(10, 27);
		this.sampleanalysispicture.Name = "sampleanalysispicture";
		this.sampleanalysispicture.Size = new System.Drawing.Size(216, 168);
		this.sampleanalysispicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.sampleanalysispicture.TabIndex = 31;
		this.sampleanalysispicture.TabStop = false;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.griddetail);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(0, 412);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(735, 326);
		this.panel5.TabIndex = 81;
		this.griddetail.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.griddetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.griddetail.FilterMode = (FilterMode)2;
		this.griddetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.griddetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.griddetail.GroupByBoxVisible = false;
		this.griddetail.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.griddetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.griddetail).Name = "griddetail";
		((ScrollableBase)this.griddetail).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.griddetail).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.griddetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.griddetail).Size = new System.Drawing.Size(735, 326);
		((System.Windows.Forms.Control)(object)this.griddetail).TabIndex = 0;
		this.griddetail.TotalRow = (InheritableBoolean)1;
		this.griddetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.griddetail).VisualStyle = (VisualStyle)4;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 131);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1207, 281);
		this.panel4.TabIndex = 80;
		this.panel8.BackColor = System.Drawing.Color.White;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsupplier);
		this.panel8.Controls.Add(this.btnsearch);
		this.panel8.Controls.Add(this.label3);
		this.panel8.Controls.Add(this.label2);
		this.panel8.Controls.Add(this.label1);
		this.panel8.Controls.Add(this.txtdatet);
		this.panel8.Controls.Add(this.txtdatef);
		this.panel8.Controls.Add(this.txtgpnof);
		this.panel8.Controls.Add(this.txtgpnot);
		this.panel8.Controls.Add(this.label45);
		this.panel8.Controls.Add(this.label44);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 61);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1207, 70);
		this.panel8.TabIndex = 79;
		this.cmbsupplier.AllowNull = (DefaultableBoolean)1;
		this.cmbsupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbsupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Location = new System.Drawing.Point(235, 33);
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Name = "cmbsupplier";
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Size = new System.Drawing.Size(252, 25);
		((System.Windows.Forms.Control)(object)this.cmbsupplier).TabIndex = 2;
		this.btnsearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnsearch.Location = new System.Drawing.Point(679, 33);
		this.btnsearch.Name = "btnsearch";
		this.btnsearch.Size = new System.Drawing.Size(58, 25);
		this.btnsearch.TabIndex = 5;
		this.btnsearch.Text = "Search";
		this.btnsearch.UseVisualStyleBackColor = true;
		this.btnsearch.Click += new System.EventHandler(btnsearch_Click);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label3.Location = new System.Drawing.Point(121, 9);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(53, 17);
		this.label3.TabIndex = 10;
		this.label3.Text = "Date To";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label2.Location = new System.Drawing.Point(12, 9);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(69, 17);
		this.label2.TabIndex = 9;
		this.label2.Text = "Date From";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label1.Location = new System.Drawing.Point(232, 9);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(95, 17);
		this.label1.TabIndex = 8;
		this.label1.Text = "Supplier Name";
		this.txtdatet.CustomFormat = "dd-MM-yy";
		this.txtdatet.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtdatet.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdatet.Location = new System.Drawing.Point(122, 33);
		this.txtdatet.Name = "txtdatet";
		this.txtdatet.Size = new System.Drawing.Size(108, 25);
		this.txtdatet.TabIndex = 1;
		this.txtdatef.CustomFormat = "dd-MM-yy";
		this.txtdatef.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtdatef.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdatef.Location = new System.Drawing.Point(10, 33);
		this.txtdatef.Name = "txtdatef";
		this.txtdatef.Size = new System.Drawing.Size(108, 25);
		this.txtdatef.TabIndex = 0;
		this.txtgpnof.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtgpnof.Location = new System.Drawing.Point(493, 33);
		this.txtgpnof.Name = "txtgpnof";
		this.txtgpnof.Size = new System.Drawing.Size(93, 25);
		this.txtgpnof.TabIndex = 3;
		this.txtgpnot.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtgpnot.Location = new System.Drawing.Point(590, 33);
		this.txtgpnot.Name = "txtgpnot";
		this.txtgpnot.Size = new System.Drawing.Size(84, 25);
		this.txtgpnot.TabIndex = 4;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label45.Location = new System.Drawing.Point(589, 10);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(61, 17);
		this.label45.TabIndex = 1;
		this.label45.Text = "GpNo To";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label44.Location = new System.Drawing.Point(490, 10);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(81, 17);
		this.label44.TabIndex = 0;
		this.label44.Text = "GpNo From ";
		this.panel7.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel7.Controls.Add(this.label43);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 27);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1207, 34);
		this.panel7.TabIndex = 78;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.Color.White;
		this.label43.Location = new System.Drawing.Point(9, 4);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(249, 24);
		this.label43.TabIndex = 0;
		this.label43.Text = "Purchase Analysis Report";
		this.tsItemTaxSchedule.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemTaxSchedule.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnAllRecord, this.btnnew });
		this.tsItemTaxSchedule.Location = new System.Drawing.Point(0, 0);
		this.tsItemTaxSchedule.Name = "tsItemTaxSchedule";
		this.tsItemTaxSchedule.Size = new System.Drawing.Size(1207, 27);
		this.tsItemTaxSchedule.TabIndex = 0;
		this.tsItemTaxSchedule.Text = "toolStrip3";
		this.btnAllRecord.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAllRecord.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAllRecord.Name = "btnAllRecord";
		this.btnAllRecord.Size = new System.Drawing.Size(65, 24);
		this.btnAllRecord.Text = "All Record";
		this.btnAllRecord.Click += new System.EventHandler(btnAllRecord_Click);
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1207, 738);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "InvLabPurchaseReport";
		this.Text = "InvLabAnalysisGroup";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvLabPurchaseReport_FormClosing);
		base.Load += new System.EventHandler(InvLabPurchaseReport_Load);
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cookingpic).EndInit();
		((System.ComponentModel.ISupportInitialize)this.sampleanalysispicture).EndInit();
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.griddetail).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbsupplier).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.tsItemTaxSchedule.ResumeLayout(false);
		this.tsItemTaxSchedule.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
