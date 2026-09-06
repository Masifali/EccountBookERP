using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.WinApp.Common;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.SaleTrading;

public class LoadGdnTrading_Engr : Form
{
	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	public DataTable dtGDNS = new DataTable();

	public int DocumentTypeId;

	private int[] AryColWidth;

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

	public LoadGdnTrading_Engr(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void PendingGdnLoad()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters gdn = new ReportsParameters();
			gdn.OrganizationId = UserAccount.OrganizationId;
			gdn.CompanyId = UserAccount.CompanyId;
			if (DocumentTypeId > 0)
			{
				gdn.DocumentTypeId = DocumentTypeId;
			}
			else
			{
				gdn.DocumentTypeId = 1607;
			}
			gdn.FromDate = Conversion.ToDateTime((object)FromDate.Value);
			gdn.ToDate = Conversion.ToDateTime((object)Todate.Value);
			gdn.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			DataTable lst = InvSaleInvoice.GetPendingGDNForSaleInvoice_Engr(gdn);
			if (lst.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("SaleOrderId", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("SupplierCustomerId");
				dt.Columns.Add("CustomerName", typeof(string));
				dt.Columns.Add("GpNO", typeof(int));
				dt.Columns.Add("BiltyNo", typeof(string));
				dt.Columns.Add("VehicleNo", typeof(string));
				dt.Columns.Add("ItemQty", typeof(double));
				dt.Columns.Add("DeliveryTypeId", typeof(int));
				dt.Columns.Add("DeliveryType", typeof(string));
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dt.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["SaleOrderId"], Conversion.ToDateTime(lst.Rows[i]["DocDate"]).ToString("dd-MMM-yy"), lst.Rows[i]["DocNo"], lst.Rows[i]["SupplierCustomerId"], lst.Rows[i]["SupplierCustomer"], lst.Rows[i]["GpNO"], lst.Rows[i]["BiltyNo"], lst.Rows[i]["VehicleNo"], lst.Rows[i]["ItemQty"], lst.Rows[i]["DeliveryTypeId"], lst.Rows[i]["DeliveryType"]);
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
			grd.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			grd.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grd.RootTable.Columns["DeliveryTypeId"].Visible = false;
			grd.RootTable.Columns["SaleOrderId"].Visible = false;
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemQty"].FormatString = "#,###.###";
			grd.RootTable.Columns["ItemQty"].TotalFormatString = "#,###.###";
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns.Add("Select");
			grd.RootTable.Columns["Select"].ActAsSelector = true;
			grd.RootTable.Columns["Select"].UseHeaderSelector = true;
			grd.RootTable.Columns["Select"].Width = 30;
			grd.RootTable.Columns["Select"].Position = 0;
			grd.FrozenColumns = 1;
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
		dtGDNS.Rows.Add(0, 0);
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
			dtGDNS.Columns.Add("Id", typeof(int));
			dtGDNS.Columns.Add("DeliveryTypeId", typeof(int));
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
				int saleOrderId = 0;
				int sID = 0;
				int TId = 0;
				dtGDNS.Rows.Clear();
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					int supplierCustomer = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
					int TypeId = Conversion.ToInt(r.Cells["DeliveryTypeId"].Value);
					int SaleOrder = Conversion.ToInt(r.Cells["SaleOrderId"].Value);
					if (supplierCustomer == 0)
					{
						continue;
					}
					if (sID == 0)
					{
						sID = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
					}
					if (TId == 0)
					{
						TId = TypeId;
					}
					if (saleOrderId == 0)
					{
						saleOrderId = SaleOrder;
					}
					if (TId != TypeId)
					{
						dtGDNS.Rows.Clear();
						MessageBox.Show("Sorry!. The Selected Gdn's are not of same Delivery Type");
						return;
					}
					if (TypeId == 2)
					{
						if (sID != supplierCustomer)
						{
							dtGDNS.Rows.Clear();
							MessageBox.Show("Sorry!. The Selected Gdn's are not of same Customer");
							return;
						}
					}
					else if (saleOrderId != SaleOrder)
					{
						dtGDNS.Rows.Clear();
						MessageBox.Show("Sorry!. The Selected Gdn's are not of same Sale Order");
						return;
					}
					dtGDNS.Rows.Add(Conversion.ToInt(r.Cells["Id"].Value), Conversion.ToInt(r.Cells["DeliveryTypeId"].Value));
					sID = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
					TId = Conversion.ToInt(r.Cells["DeliveryTypeId"].Value);
					saleOrderId = Conversion.ToInt(r.Cells["SaleOrderId"].Value);
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
				InvGdn invGdn = InvGdn.GetByID(Conversion.ToInt(r.Cells["Id"].Value));
				DataTable dthistorydetail = new DataTable();
				dthistorydetail.Columns.Add("OrderNo");
				dthistorydetail.Columns.Add("ItemName");
				dthistorydetail.Columns.Add("JobLot");
				dthistorydetail.Columns.Add("UOM");
				dthistorydetail.Columns.Add("Qty", typeof(double));
				dthistorydetail.Columns.Add("GrossWight", typeof(double));
				dthistorydetail.Columns.Add("WtCut", typeof(double));
				dthistorydetail.Columns.Add("WtCutTotal", typeof(double));
				dthistorydetail.Columns.Add("AddLesswt", typeof(double));
				dthistorydetail.Columns.Add("NetWeight", typeof(double));
				dthistorydetail.Columns.Add("StockWeight", typeof(double));
				dthistorydetail.Columns.Add("WareHouse");
				dthistorydetail.Columns.Add("City");
				dthistorydetail.Clear();
				for (int i = 0; i < invGdn.invGdnDetail.Count; i++)
				{
					dthistorydetail.Rows.Add(invGdn.invGdnDetail[i].SaleOrderNo, invGdn.invGdnDetail[i].Item, invGdn.invGdnDetail[i].JobLot, invGdn.invGdnDetail[i].UOMCode, invGdn.invGdnDetail[i].ItemQty, invGdn.invGdnDetail[i].GrossWeight, invGdn.invGdnDetail[i].WtCut, invGdn.invGdnDetail[i].WtCutTotal, invGdn.invGdnDetail[i].AdLsWeight, invGdn.invGdnDetail[i].NetBillWeight, invGdn.invGdnDetail[i].StockWeight, invGdn.invGdnDetail[i].WareHouseCode, invGdn.invGdnDetail[i].AreaCity);
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
			if (DocumentTypeId == 1612)
			{
				GridDetail.RootTable.Columns["GrossWight"].Visible = false;
				GridDetail.RootTable.Columns["WtCut"].Visible = false;
				GridDetail.RootTable.Columns["WtCutTotal"].Visible = false;
				GridDetail.RootTable.Columns["AddLesswt"].Visible = false;
				GridDetail.RootTable.Columns["StockWeight"].Visible = false;
				GridDetail.RootTable.Columns["NetWeight"].Visible = false;
				GridDetail.RootTable.Columns["City"].Visible = false;
			}
			GridDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["Qty"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
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
		this.panel1.SuspendLayout();
		this.panelheading.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetail).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panelheading);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1037, 30);
		this.panel1.TabIndex = 0;
		this.panelheading.BackColor = System.Drawing.Color.Teal;
		this.panelheading.Controls.Add(this.label23);
		this.panelheading.Controls.Add(this.ctrlGrdBar1);
		this.panelheading.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelheading.Location = new System.Drawing.Point(0, 0);
		this.panelheading.Name = "panelheading";
		this.panelheading.Size = new System.Drawing.Size(1037, 31);
		this.panelheading.TabIndex = 29;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(6, 7);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(89, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "GDN Load";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(995, 5);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 10;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grd.AllowDelete = (InheritableBoolean)1;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Top;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1037, 265);
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
		this.panel2.Location = new System.Drawing.Point(0, 30);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1037, 51);
		this.panel2.TabIndex = 30;
		this.Todate.CustomFormat = "dd-MMM-yy";
		this.Todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.Todate.Location = new System.Drawing.Point(141, 23);
		this.Todate.Name = "Todate";
		this.Todate.Size = new System.Drawing.Size(117, 23);
		this.Todate.TabIndex = 1;
		this.FromDate.CustomFormat = "dd-MMM-yy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(15, 23);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(117, 23);
		this.FromDate.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(139, 4);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 15);
		this.label2.TabIndex = 15;
		this.label2.Text = "To Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(14, 4);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(63, 15);
		this.label3.TabIndex = 16;
		this.label3.Text = "From Date";
		this.btnLoadOnInvoice.BackColor = System.Drawing.Color.Teal;
		this.btnLoadOnInvoice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnLoadOnInvoice.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadOnInvoice.ForeColor = System.Drawing.Color.White;
		this.btnLoadOnInvoice.Location = new System.Drawing.Point(326, 22);
		this.btnLoadOnInvoice.Name = "btnLoadOnInvoice";
		this.btnLoadOnInvoice.Size = new System.Drawing.Size(58, 25);
		this.btnLoadOnInvoice.TabIndex = 3;
		this.btnLoadOnInvoice.Text = "Load";
		this.btnLoadOnInvoice.UseVisualStyleBackColor = false;
		this.btnLoadOnInvoice.Click += new System.EventHandler(btnLoadOnInvoice_Click_1);
		this.btngrnlod.BackColor = System.Drawing.Color.Teal;
		this.btngrnlod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btngrnlod.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btngrnlod.ForeColor = System.Drawing.Color.White;
		this.btngrnlod.Location = new System.Drawing.Point(262, 22);
		this.btngrnlod.Name = "btngrnlod";
		this.btngrnlod.Size = new System.Drawing.Size(64, 25);
		this.btngrnlod.TabIndex = 2;
		this.btngrnlod.Text = "Search";
		this.btngrnlod.UseVisualStyleBackColor = false;
		this.btngrnlod.Click += new System.EventHandler(btngrnlod_Click);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetail);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 81);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1037, 508);
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
		((System.Windows.Forms.Control)(object)this.GridDetail).Location = new System.Drawing.Point(0, 265);
		((System.Windows.Forms.Control)(object)this.GridDetail).Name = "GridDetail";
		((ScrollableBase)this.GridDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridDetail).Size = new System.Drawing.Size(1037, 243);
		((System.Windows.Forms.Control)(object)this.GridDetail).TabIndex = 4;
		this.GridDetail.TotalRow = (InheritableBoolean)1;
		this.GridDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridDetail).VisualStyle = (VisualStyle)4;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1037, 589);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.Name = "LoadGdnTrading_Engr";
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
		base.ResumeLayout(false);
	}
}
