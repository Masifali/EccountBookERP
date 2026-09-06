using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Steel.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Steel.Inventory;
using Architecture.WinApp.Common;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Steel.Sale;

public class frmLoadGDN_St : Form
{
	private int[] AryColWidth;

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	public DataTable dtGDNS = new DataTable();

	public int DocumentTypeId;

	private IContainer components;

	private Panel panel1;

	private Panel panelheading;

	private Label label23;

	private Panel panel2;

	private Button btngrnlod;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel3;

	internal GridEX grd;

	private Button btnLoadOnInvoice;

	private DateTimePicker Todate;

	private DateTimePicker FromDate;

	private Label label2;

	private Label label3;

	internal GridEX GridDetail;

	private Panel panel4;

	private Panel panel5;

	private Label label38;

	private Panel panel6;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label1;

	public frmLoadGDN_St(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void PendingGdnLoad()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		try
		{
			DataTable lst = InvSaleInvoice.GetPendingGdnForSaleInvoice(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 1508,
				FromDate = Conversion.ToDateTime((object)FromDate.Value),
				ToDate = Conversion.ToDateTime((object)Todate.Value),
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (lst.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("GdnType");
				dt.Columns.Add("RefDocId", typeof(int));
				dt.Columns.Add("SupplierCustomerId");
				dt.Columns.Add("CustomerName");
				dt.Columns.Add("GpNO", typeof(int));
				dt.Columns.Add("BiltyNo");
				dt.Columns.Add("VehicleNo");
				dt.Columns.Add("ItemQty", typeof(double));
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dt.Rows.Add(lst.Rows[i]["Id"], Conversion.ToDateTime(lst.Rows[i]["DocDate"]).ToString("dd-MMM-yy"), lst.Rows[i]["DocNo"], lst.Rows[i]["OrderType"], lst.Rows[i]["OrderTypeId"], lst.Rows[i]["SupplierCustomerId"], lst.Rows[i]["SupplierCustomer"], lst.Rows[i]["GpNO"], lst.Rows[i]["BiltyNo"], lst.Rows[i]["VehicleNo"], lst.Rows[i]["ItemQty"]);
				}
				grd.DataSource = dt;
				grd.RetrieveStructure();
				grdSettingsForGrnPending();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdSettingsForGrnPending()
	{
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["RefDocId"].Visible = false;
			grd.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			grd.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grd.RootTable.Columns.Add("Select");
			grd.RootTable.Columns["Select"].ActAsSelector = true;
			grd.RootTable.Columns["Select"].UseHeaderSelector = true;
			grd.RootTable.Columns["Select"].Position = 0;
			grd.FrozenColumns = 1;
			CommonServices.GridAutoAdjustment(AryColWidth, grd);
			ctrlGrdBar1_Load(null, null);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
			if (fs != null)
			{
				grd.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		dtGDNS.Rows.Add(0);
		Hide();
	}

	private void btngrnlod_Click(object sender, EventArgs e)
	{
		try
		{
			PendingGdnLoad();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void frmLoadGRN_Load_1(object sender, EventArgs e)
	{
		try
		{
			FromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			dtGDNS.Columns.Add("Id");
			PendingGdnLoad();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnLoadOnInvoice_Click_1(object sender, EventArgs e)
	{
		try
		{
			if (int.Parse(grd.GetCheckedRows().Count().ToString()) > 0)
			{
				int sID = 0;
				int GdnType = 0;
				dtGDNS.Rows.Clear();
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					int supplierCustomer = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
					int GdnRefId = Conversion.ToInt(r.Cells["RefDocId"].Value);
					if (supplierCustomer != 0)
					{
						if (sID == 0)
						{
							sID = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
						}
						if (GdnType == 0)
						{
							GdnType = GdnRefId;
						}
						if (GdnType != GdnRefId)
						{
							dtGDNS.Rows.Clear();
							MessageBox.Show("Sorry!. The Selected Gdn's are not of same Type");
							return;
						}
						if (sID != supplierCustomer)
						{
							dtGDNS.Rows.Clear();
							MessageBox.Show("Sorry!. The Selected Gdn's are not of same Customer");
							return;
						}
						dtGDNS.Rows.Add(r.Cells["Id"].Value.ToString());
						sID = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
						GdnType = Conversion.ToInt(r.Cells["RefDocId"].Value);
					}
				}
				Hide();
				return;
			}
			throw new Exception("Chek the row first");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grd_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				InvGdn invGdn = InvGdn.ReadById(Conversion.ToInt(r.Cells["Id"].Value));
				DataTable dthistorydetail = new DataTable();
				dthistorydetail.Columns.Add("SaleOrder");
				dthistorydetail.Columns.Add("ItemName");
				dthistorydetail.Columns.Add("JobLot");
				dthistorydetail.Columns.Add("PackingType");
				dthistorydetail.Columns.Add("PackUom");
				dthistorydetail.Columns.Add("ItemQty", typeof(double));
				dthistorydetail.Columns.Add("GrossWight", typeof(double));
				dthistorydetail.Columns.Add("WtCut", typeof(double));
				dthistorydetail.Columns.Add("WtCutTotal", typeof(double));
				dthistorydetail.Columns.Add("AddLesswt", typeof(double));
				dthistorydetail.Columns.Add("NetWeight", typeof(double));
				dthistorydetail.Columns.Add("StockWeight", typeof(double));
				dthistorydetail.Columns.Add("WareHouse");
				dthistorydetail.Columns.Add("LabNo");
				dthistorydetail.Columns.Add("City");
				dthistorydetail.Clear();
				for (int i = 0; i < invGdn.invGdnDetail.Count; i++)
				{
					dthistorydetail.Rows.Add(invGdn.invGdnDetail[i].SaleOrderNo, invGdn.invGdnDetail[i].ItemName, invGdn.invGdnDetail[i].JobLotDescription, invGdn.invGdnDetail[i].PackingType, invGdn.invGdnDetail[i].UOMCode, invGdn.invGdnDetail[i].ItemQty, invGdn.invGdnDetail[i].GrossWeight, invGdn.invGdnDetail[i].WtCut, invGdn.invGdnDetail[i].WtCutTotal, invGdn.invGdnDetail[i].AdLsWeight, invGdn.invGdnDetail[i].NetBillWeight, invGdn.invGdnDetail[i].StockWeight, invGdn.invGdnDetail[i].WareHouseName, invGdn.invGdnDetail[i].LabReportRef, invGdn.invGdnDetail[i].CityName);
				}
				GridDetail.DataSource = dthistorydetail;
				GridDetail.RetrieveStructure();
				GridDetailSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailSetting()
	{
		try
		{
			GridDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			GridDetail.RootTable.Columns["GrossWight"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["GrossWight"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["GrossWight"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["GrossWight"].TotalFormatString = "#,##0.##";
			GridDetail.RootTable.Columns["WtCut"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["WtCut"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["WtCutTotal"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["WtCutTotal"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["WtCutTotal"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["WtCutTotal"].TotalFormatString = "#,##0.##";
			GridDetail.RootTable.Columns["AddLesswt"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["AddLesswt"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["AddLesswt"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["AddLesswt"].TotalFormatString = "#,##0.##";
			GridDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["NetWeight"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			GridDetail.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["StockWeight"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustment(AryColWidth, GridDetail);
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GridDetail).Name);
			if (fs != null)
			{
				GridDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
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
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GridDetail_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panelheading = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.Todate = new System.Windows.Forms.DateTimePicker();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.btnLoadOnInvoice = new System.Windows.Forms.Button();
		this.btngrnlod = new System.Windows.Forms.Button();
		this.panel3 = new System.Windows.Forms.Panel();
		this.GridDetail = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label38 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel1.SuspendLayout();
		this.panelheading.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetail).BeginInit();
		this.panel4.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel6.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panelheading);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1037, 31);
		this.panel1.TabIndex = 0;
		this.panelheading.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panelheading.Controls.Add(this.label23);
		this.panelheading.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelheading.Location = new System.Drawing.Point(0, 0);
		this.panelheading.Name = "panelheading";
		this.panelheading.Size = new System.Drawing.Size(1037, 30);
		this.panelheading.TabIndex = 29;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(6, 6);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(89, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "GDN Load";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(994, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(40, 21);
		this.ctrlGrdBar1.TabIndex = 10;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grd.AllowDelete = (InheritableBoolean)1;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1037, 358);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 2;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.SelectionChanged += new System.EventHandler(grd_SelectionChanged);
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.Todate);
		this.panel2.Controls.Add(this.FromDate);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.btnLoadOnInvoice);
		this.panel2.Controls.Add(this.btngrnlod);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 31);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1037, 51);
		this.panel2.TabIndex = 30;
		this.Todate.CustomFormat = "dd-MMM-yy";
		this.Todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.Todate.Location = new System.Drawing.Point(137, 19);
		this.Todate.Name = "Todate";
		this.Todate.Size = new System.Drawing.Size(117, 23);
		this.Todate.TabIndex = 1;
		this.FromDate.CustomFormat = "dd-MMM-yy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(11, 19);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(117, 23);
		this.FromDate.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(135, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 15);
		this.label2.TabIndex = 15;
		this.label2.Text = "To Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(10, 3);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(63, 15);
		this.label3.TabIndex = 16;
		this.label3.Text = "From Date";
		this.btnLoadOnInvoice.BackColor = System.Drawing.Color.Teal;
		this.btnLoadOnInvoice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnLoadOnInvoice.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadOnInvoice.ForeColor = System.Drawing.Color.White;
		this.btnLoadOnInvoice.Location = new System.Drawing.Point(323, 19);
		this.btnLoadOnInvoice.Name = "btnLoadOnInvoice";
		this.btnLoadOnInvoice.Size = new System.Drawing.Size(58, 23);
		this.btnLoadOnInvoice.TabIndex = 3;
		this.btnLoadOnInvoice.Text = "Load";
		this.btnLoadOnInvoice.UseVisualStyleBackColor = false;
		this.btnLoadOnInvoice.Click += new System.EventHandler(btnLoadOnInvoice_Click_1);
		this.btngrnlod.BackColor = System.Drawing.Color.Teal;
		this.btngrnlod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btngrnlod.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btngrnlod.ForeColor = System.Drawing.Color.White;
		this.btngrnlod.Location = new System.Drawing.Point(258, 19);
		this.btngrnlod.Name = "btngrnlod";
		this.btngrnlod.Size = new System.Drawing.Size(66, 23);
		this.btngrnlod.TabIndex = 2;
		this.btngrnlod.Text = "Search";
		this.btngrnlod.UseVisualStyleBackColor = false;
		this.btngrnlod.Click += new System.EventHandler(btngrnlod_Click);
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Controls.Add(this.ctrlGrdBar2);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add(this.panel4);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 82);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1037, 594);
		this.panel3.TabIndex = 31;
		this.GridDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GridDetail).BackColor = System.Drawing.Color.White;
		this.GridDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		GridDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridDetail.DesignTimeLayout = GridDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridDetail.FilterMode = (FilterMode)2;
		this.GridDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridDetail.GroupByBoxVisible = false;
		this.GridDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GridDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GridDetail).Name = "GridDetail";
		((ScrollableBase)this.GridDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridDetail).Size = new System.Drawing.Size(1037, 184);
		((System.Windows.Forms.Control)(object)this.GridDetail).TabIndex = 4;
		this.GridDetail.TotalRow = (InheritableBoolean)1;
		this.GridDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridDetail).VisualStyle = (VisualStyle)4;
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Controls.Add(this.label38);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1037, 384);
		this.panel4.TabIndex = 5;
		this.label38.BackColor = System.Drawing.Color.Teal;
		this.label38.Dock = System.Windows.Forms.DockStyle.Top;
		this.label38.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.ForeColor = System.Drawing.Color.White;
		this.label38.Location = new System.Drawing.Point(0, 0);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(1037, 26);
		this.label38.TabIndex = 1;
		this.label38.Text = "Main Grid";
		this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 26);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1037, 358);
		this.panel5.TabIndex = 11;
		this.label1.BackColor = System.Drawing.Color.Teal;
		this.label1.Dock = System.Windows.Forms.DockStyle.Top;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(0, 384);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(1037, 26);
		this.label1.TabIndex = 6;
		this.label1.Text = "Detail Grid";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(994, 386);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.GridDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(40, 21);
		this.ctrlGrdBar2.TabIndex = 12;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetail);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 410);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1037, 184);
		this.panel6.TabIndex = 13;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1037, 676);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.Name = "frmLoadGDN_St";
		this.Text = "frmLoadGRN";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmLoadGRN_Load_1);
		this.panel1.ResumeLayout(false);
		this.panelheading.ResumeLayout(false);
		this.panelheading.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridDetail).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
