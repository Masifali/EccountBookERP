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

namespace Architecture.WinApp.Steel.Purchase;

public class frmLoadGRN_St : Form
{
	private int[] AryColWidth;

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	public DataTable dtGRNS = new DataTable();

	public int DocumentTypeId = 1501;

	private IContainer components;

	private Panel panel1;

	private Panel panelheading;

	private Label label23;

	private Panel panel2;

	private Button btngrnlod;

	private DateTimePicker Todate;

	private DateTimePicker FromDate;

	private Label label13;

	private Label label1;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel3;

	internal GridEX grd;

	private Button btnLoadOnInvoice;

	private Button btnReset;

	internal GridEX GridDetail;

	public frmLoadGRN_St(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void PendingGrnLoad()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		try
		{
			DataTable lst = InvPurchaseInvoice.GetPendingGrnForPurchaseInvoice(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = DocumentTypeId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				FromDate = FromDate.Value,
				ToDate = Todate.Value
			});
			if (lst.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("RefDocumentTypeId");
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("SupplierCustomerId");
				dt.Columns.Add("SupplierName");
				dt.Columns.Add("GpNO");
				dt.Columns.Add("BiltyNo");
				dt.Columns.Add("VehicleNo");
				dt.Columns.Add("PurchaseOrderId");
				dt.Columns.Add("OrderNo", typeof(int));
				dt.Columns.Add("PurchaseAgainst");
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dt.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["RefDocumentTypeId"], Conversion.ToDateTime(lst.Rows[i]["DocDate"]).ToString("dd-MMM.yy"), lst.Rows[i]["DocNo"], lst.Rows[i]["SupplierCustomerId"], lst.Rows[i]["SupplierName"], lst.Rows[i]["GpNO"], lst.Rows[i]["BiltyNo"], lst.Rows[i]["VehicleNo"], lst.Rows[i]["PurchaseOrderId"], lst.Rows[i]["OrderNo"], lst.Rows[i]["PurchaseAgainst"]);
				}
				grd.DataSource = dt;
				grd.RetrieveStructure();
				grdSettingsForGrnPending();
			}
			else
			{
				grd.ClearStructure();
			}
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
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		dtGRNS = null;
		Hide();
	}

	private void grdSettingsForGrnPending()
	{
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grd.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			grd.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grd.RootTable.Columns["PurchaseOrderId"].Visible = false;
			grd.RootTable.Columns["SupplierName"].DefaultFilterRowComparison = (FilterConditionOperator)7;
			CommonServices.GridAutoAdjustment(AryColWidth, grd);
			grd.RootTable.Columns.Add("Select");
			grd.RootTable.Columns["Select"].ActAsSelector = true;
			grd.RootTable.Columns["Select"].UseHeaderSelector = true;
			grd.RootTable.Columns["Select"].Position = 0;
			grd.RootTable.Columns["Select"].Width = 60;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btngrnlod_Click(object sender, EventArgs e)
	{
		try
		{
			PendingGrnLoad();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void frmLoadGRN_Load_1(object sender, EventArgs e)
	{
		try
		{
			FromDate.Focus();
			FromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			dtGRNS.Columns.Add("Id");
			dtGRNS.Columns.Add("GrnType");
			PendingGrnLoad();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadOnInvoice_Click_1(object sender, EventArgs e)
	{
		try
		{
			int GrnType = 1;
			if (int.Parse(grd.GetCheckedRows().Count().ToString()) > 0)
			{
				int supplierCustomer = 0;
				int OrderId = 0;
				int sID = 0;
				int sOrderId = 1;
				int sDocId = 1;
				int RefDocumentTypeId = 0;
				int RefId = 0;
				int DocId = 0;
				dtGRNS.Rows.Clear();
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					supplierCustomer = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
					OrderId = Conversion.ToInt(r.Cells["PurchaseOrderId"].Value);
					RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
					DocId = Conversion.ToInt(r.Cells["Id"].Value);
					if (supplierCustomer != 0)
					{
						if (sID == 0)
						{
							sID = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
						}
						if (RefId == 0)
						{
							RefId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
						}
						if (sOrderId == 1)
						{
							sOrderId = Conversion.ToInt(r.Cells["PurchaseOrderId"].Value);
						}
						if (sDocId == 0)
						{
							sDocId = Conversion.ToInt(r.Cells["Id"].Value);
						}
						if (DocumentTypeId == 166 && DocId != sDocId)
						{
							dtGRNS.Rows.Clear();
							MessageBox.Show("only single grn select");
							break;
						}
						if (sID != supplierCustomer)
						{
							dtGRNS.Rows.Clear();
							throw new Exception("Sorry the Grn Not should be of same Supplier");
						}
						if (RefId != RefDocumentTypeId)
						{
							dtGRNS.Rows.Clear();
							throw new Exception("Sorry the Grn should be of same PurchaseAgainst type");
						}
						if (sOrderId != OrderId)
						{
							dtGRNS.Rows.Clear();
							throw new Exception("Sorry the Grn Not should be of same PurchaseOrder");
						}
						if (r.Cells["PurchaseAgainst"].Value.ToString() == "PurchaseOrder")
						{
							GrnType = 1;
						}
						else if (r.Cells["PurchaseAgainst"].Value.ToString() == "Market Purchase")
						{
							GrnType = 2;
						}
						else if (r.Cells["PurchaseAgainst"].Value.ToString() == "Gate Purchase")
						{
							GrnType = 3;
						}
						dtGRNS.Rows.Add(r.Cells["Id"].Value.ToString(), GrnType.ToString());
						sID = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
						sOrderId = Conversion.ToInt(r.Cells["PurchaseOrderId"].Value);
						RefId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
						sDocId = Conversion.ToInt(r.Cells["Id"].Value);
					}
				}
				Hide();
			}
			else
			{
				MessageBox.Show("Chek the row first");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnReset_Click(object sender, EventArgs e)
	{
		FromDate.Focus();
		FromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
	}

	private void rdRegularGrn_Click(object sender, EventArgs e)
	{
		PendingGrnLoad();
	}

	private void frmLoadGRN_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.Control && e.KeyCode == Keys.F5)
			{
				FromDate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btngrnlod_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.L)
			{
				btnLoadOnInvoice_Click_1(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			GridDetail.RootTable.Columns["WtCut"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["WtCut"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["WtCut"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["WtCut"].TotalFormatString = "#,##0.##";
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
			MessageBox.Show(ex.Message);
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
				InvGrn invGrn = InvGrn.GetByID(Conversion.ToInt(r.Cells["Id"].Value));
				DataTable dthistorydetail = new DataTable();
				dthistorydetail.Columns.Add("Order", typeof(int));
				dthistorydetail.Columns.Add("Item");
				dthistorydetail.Columns.Add("Job");
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
				for (int i = 0; i < invGrn.invGrnDetail.Count; i++)
				{
					dthistorydetail.Rows.Add(invGrn.invGrnDetail[i].PurchaserOrderNo, invGrn.invGrnDetail[i].ItemName, invGrn.invGrnDetail[i].JobLotDescription, invGrn.invGrnDetail[i].PackingType, invGrn.invGrnDetail[i].PackUom, invGrn.invGrnDetail[i].ItemQty, invGrn.invGrnDetail[i].GrossWeight, invGrn.invGrnDetail[i].WtCut, invGrn.invGrnDetail[i].WtCutTotal, invGrn.invGrnDetail[i].AdLsWeight, invGrn.invGrnDetail[i].NetBillWeight, invGrn.invGrnDetail[i].StockWeight, invGrn.invGrnDetail[i].WareHouseName, invGrn.invGrnDetail[i].LabReportRef, invGrn.invGrnDetail[i].CityName);
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
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GridDetail_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panelheading = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.btnReset = new System.Windows.Forms.Button();
		this.btnLoadOnInvoice = new System.Windows.Forms.Button();
		this.btngrnlod = new System.Windows.Forms.Button();
		this.Todate = new System.Windows.Forms.DateTimePicker();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.label13 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
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
		this.panel1.Size = new System.Drawing.Size(959, 26);
		this.panel1.TabIndex = 0;
		this.panelheading.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panelheading.Controls.Add(this.ctrlGrdBar1);
		this.panelheading.Controls.Add(this.label23);
		this.panelheading.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelheading.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panelheading.Location = new System.Drawing.Point(0, 0);
		this.panelheading.Name = "panelheading";
		this.panelheading.Size = new System.Drawing.Size(959, 28);
		this.panelheading.TabIndex = 29;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(912, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(43, 25);
		this.ctrlGrdBar1.TabIndex = 10;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		this.grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Top;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(959, 328);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 2;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.SelectionChanged += new System.EventHandler(grd_SelectionChanged);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(9, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(89, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "GRN Load";
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.btnReset);
		this.panel2.Controls.Add(this.btnLoadOnInvoice);
		this.panel2.Controls.Add(this.btngrnlod);
		this.panel2.Controls.Add(this.Todate);
		this.panel2.Controls.Add(this.FromDate);
		this.panel2.Controls.Add(this.label13);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel2.Location = new System.Drawing.Point(0, 26);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(959, 52);
		this.panel2.TabIndex = 30;
		this.btnReset.BackColor = System.Drawing.Color.Teal;
		this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnReset.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnReset.ForeColor = System.Drawing.Color.White;
		this.btnReset.Location = new System.Drawing.Point(406, 24);
		this.btnReset.Name = "btnReset";
		this.btnReset.Size = new System.Drawing.Size(56, 22);
		this.btnReset.TabIndex = 7;
		this.btnReset.Text = "Reset";
		this.btnReset.UseVisualStyleBackColor = false;
		this.btnReset.Click += new System.EventHandler(btnReset_Click);
		this.btnLoadOnInvoice.BackColor = System.Drawing.Color.Teal;
		this.btnLoadOnInvoice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnLoadOnInvoice.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadOnInvoice.ForeColor = System.Drawing.Color.White;
		this.btnLoadOnInvoice.Location = new System.Drawing.Point(343, 24);
		this.btnLoadOnInvoice.Name = "btnLoadOnInvoice";
		this.btnLoadOnInvoice.Size = new System.Drawing.Size(61, 22);
		this.btnLoadOnInvoice.TabIndex = 6;
		this.btnLoadOnInvoice.Text = "Load";
		this.btnLoadOnInvoice.UseVisualStyleBackColor = false;
		this.btnLoadOnInvoice.Click += new System.EventHandler(btnLoadOnInvoice_Click_1);
		this.btngrnlod.BackColor = System.Drawing.Color.Teal;
		this.btngrnlod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btngrnlod.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btngrnlod.ForeColor = System.Drawing.Color.White;
		this.btngrnlod.Location = new System.Drawing.Point(279, 24);
		this.btngrnlod.Name = "btngrnlod";
		this.btngrnlod.Size = new System.Drawing.Size(62, 22);
		this.btngrnlod.TabIndex = 5;
		this.btngrnlod.Text = "Search";
		this.btngrnlod.UseVisualStyleBackColor = false;
		this.btngrnlod.Click += new System.EventHandler(btngrnlod_Click);
		this.Todate.CustomFormat = "dd-MMM-yyyy";
		this.Todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.Todate.Location = new System.Drawing.Point(148, 24);
		this.Todate.Name = "Todate";
		this.Todate.Size = new System.Drawing.Size(128, 23);
		this.Todate.TabIndex = 1;
		this.FromDate.CustomFormat = "dd-MMM-yyyy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(14, 24);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(128, 23);
		this.FromDate.TabIndex = 0;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(150, 5);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(48, 15);
		this.label13.TabIndex = 11;
		this.label13.Text = "To Date";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(14, 5);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 12;
		this.label1.Text = "From Date";
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetail);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 78);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(959, 629);
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
		((System.Windows.Forms.Control)(object)this.GridDetail).Location = new System.Drawing.Point(0, 328);
		((System.Windows.Forms.Control)(object)this.GridDetail).Name = "GridDetail";
		((ScrollableBase)this.GridDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridDetail).Size = new System.Drawing.Size(959, 301);
		((System.Windows.Forms.Control)(object)this.GridDetail).TabIndex = 3;
		this.GridDetail.TotalRow = (InheritableBoolean)1;
		this.GridDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridDetail).VisualStyle = (VisualStyle)4;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(959, 707);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmLoadGRN_St";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmLoadGRN_Load_1);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmLoadGRN_KeyDown);
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
