using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WebHelper.Helper;
using Architecture.WinApp.Commentaries;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.DashBoard;

public class OrderManagemantPopUpForPurchaseSale : Form
{
	private UserAccount UserAccount = new UserAccount();

	public int InvoiceType;

	public int ItemId;

	private DataTable data = new DataTable();

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip toolStrip3;

	internal GridEX grd;

	private ToolStripButton BtnPrint205;

	private ToolStripButton BtnPrint206;

	private ToolStripButton BtnPrint271;

	public OrderManagemantPopUpForPurchaseSale(UserAccount Puser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = Puser;
	}

	private void OrderManagemantPopUpForPurchaseSale_Load(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		try
		{
			DataTable grdlst = new DataTable();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.AppId = UserAccount.AppId;
			obj.EntryUser = UserAccount.ID;
			obj.ItemId = ItemId;
			obj.Status = "Open";
			obj.IsApproved = true;
			if (InvoiceType == 1)
			{
				obj.DocumentTypeId = 41;
				BtnPrint271.Visible = false;
				BtnPrint205.Visible = true;
				BtnPrint206.Visible = true;
				grdlst = PurchaseOrder.PurchaseOrderHistory(obj);
				if (grdlst.Rows.Count > 0)
				{
					GridFillForPuschaseOrder(grdlst);
				}
				else
				{
					grd.ClearStructure();
				}
				return;
			}
			BtnPrint271.Visible = true;
			BtnPrint205.Visible = false;
			BtnPrint206.Visible = false;
			obj.ApprovedFilter = "All";
			DataSet ds = SaleOrder.SaleOrderHistory(obj);
			grdlst = ds?.Tables[0];
			if (grdlst.Rows.Count > 0)
			{
				ReportHelper.ReplaceCompLogoColumn(grdlst, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
				GridFillForSaleOrder(grdlst);
				data = grdlst;
			}
			else
			{
				grd.ClearStructure();
				data.Rows.Clear();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridFillForPuschaseOrder(DataTable grdlst)
	{
		try
		{
			DataTable data = new DataTable();
			data.Columns.Add("Id");
			data.Columns.Add("DocDate", typeof(DateTime));
			data.Columns.Add("DocNo", typeof(int));
			data.Columns.Add("SupCustId");
			data.Columns.Add("SupplierName");
			data.Columns.Add("DeliveryTerm");
			data.Columns.Add("PurchaseGLAC");
			data.Columns.Add("ItemId");
			data.Columns.Add("ItemName");
			data.Columns.Add("Crop");
			data.Columns.Add("PackUom");
			data.Columns.Add("Qty", typeof(double));
			data.Columns.Add("RcvdQty", typeof(double));
			data.Columns.Add("RejQty", typeof(double));
			data.Columns.Add("BalQty", typeof(double));
			data.Columns.Add("Weight", typeof(double));
			data.Columns.Add("RcvdWeight", typeof(double));
			data.Columns.Add("BalWeight", typeof(double));
			data.Columns.Add("Rate", typeof(double));
			data.Columns.Add("ExpiryDate", typeof(DateTime));
			data.Clear();
			if (grdlst.Rows.Count > 0)
			{
				for (int i = 0; i < grdlst.Rows.Count; i++)
				{
					data.Rows.Add(grdlst.Rows[i]["Id"], Convert.ToDateTime(grdlst.Rows[i]["DocDate"]).ToShortDateString(), grdlst.Rows[i]["DocNo"], grdlst.Rows[i]["OrderSupCustId"], grdlst.Rows[i]["SupplierName"], grdlst.Rows[i]["DeliveryTerm"], grdlst.Rows[i]["PurchaseGLAC"], grdlst.Rows[i]["OrderItemId"], grdlst.Rows[i]["ItemName"], grdlst.Rows[i]["Crop"], grdlst.Rows[i]["UOMCodeItm"], grdlst.Rows[i]["OrderItemQty"], grdlst.Rows[i]["ReceivedQty"], grdlst.Rows[i]["RejQty"], grdlst.Rows[i]["BalQty"], grdlst.Rows[i]["NetWeight"], grdlst.Rows[i]["ReceivedWeight"], grdlst.Rows[i]["BalWeight"], grdlst.Rows[i]["OrderItemRate"], Convert.ToDateTime(grdlst.Rows[i]["OrderExpiryDate"]).ToShortDateString());
				}
				grd.DataSource = data;
				grd.RetrieveStructure();
				GridSettingsForPurchaseOrder();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingsForPurchaseOrder()
	{
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["SupCustId"].Visible = false;
			grd.RootTable.Columns["PurchaseGLAC"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Groups.Add("ItemName");
			grd.RootTable.GroupTotals = (GroupTotals)2;
			grd.RootTable.Columns["ItemName"].Width = 200;
			grd.RootTable.Columns["SupplierName"].Width = 200;
			grd.RootTable.Columns["DocNo"].Width = 60;
			grd.RootTable.Columns["Crop"].Width = 60;
			grd.RootTable.Columns["PackUom"].Width = 70;
			grd.RootTable.Columns["Qty"].Width = 80;
			grd.RootTable.Columns["RcvdQty"].Width = 80;
			grd.RootTable.Columns["RejQty"].Width = 80;
			grd.RootTable.Columns["BalQty"].Width = 80;
			grd.RootTable.Columns["Weight"].Width = 80;
			grd.RootTable.Columns["RcvdWeight"].Width = 80;
			grd.RootTable.Columns["BalWeight"].Width = 80;
			grd.RootTable.Columns["Rate"].Width = 80;
			grd.RootTable.Columns["ExpiryDate"].Width = 80;
			grd.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["SupplierName"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["ItemName"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["RcvdQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["RejQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["RcvdWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BalWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RcvdQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RejQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RcvdWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BalWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Qty"].FormatString = "0,0";
			grd.RootTable.Columns["RcvdQty"].FormatString = "0,0";
			grd.RootTable.Columns["RejQty"].FormatString = "0,0";
			grd.RootTable.Columns["BalQty"].FormatString = "0,0";
			grd.RootTable.Columns["Weight"].FormatString = "0,0";
			grd.RootTable.Columns["RcvdWeight"].FormatString = "0,0";
			grd.RootTable.Columns["BalWeight"].FormatString = "0,0";
			grd.RootTable.Columns["Rate"].FormatString = "#,0.00";
			grd.RootTable.Columns["Qty"].TotalFormatString = "0,0";
			grd.RootTable.Columns["RcvdQty"].TotalFormatString = "0,0";
			grd.RootTable.Columns["RejQty"].TotalFormatString = "0,0";
			grd.RootTable.Columns["BalQty"].TotalFormatString = "0,0";
			grd.RootTable.Columns["Weight"].TotalFormatString = "0,0";
			grd.RootTable.Columns["RcvdWeight"].TotalFormatString = "0,0";
			grd.RootTable.Columns["BalWeight"].TotalFormatString = "0,0";
			grd.RootTable.Columns.Add("Slip");
			grd.RootTable.Columns["Slip"].Caption = "Slip203";
			grd.RootTable.Columns["Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Slip"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Slip"].ButtonText = "Slip203";
			grd.RootTable.Columns["Slip"].Width = 60;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridFillForSaleOrder(DataTable grdlst)
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("DocDate", typeof(DateTime));
			dt.Columns.Add("DocNo", typeof(int));
			dt.Columns.Add("CustomerName");
			dt.Columns.Add("SupCustId");
			dt.Columns.Add("ItemId");
			dt.Columns.Add("SaleGLAC");
			dt.Columns.Add("DeliveryTerm");
			dt.Columns.Add("ItemName");
			dt.Columns.Add("Crop");
			dt.Columns.Add("PackUom");
			dt.Columns.Add("Qty", typeof(double));
			dt.Columns.Add("DispatchQty", typeof(double));
			dt.Columns.Add("BalQty", typeof(double));
			dt.Columns.Add("Weight", typeof(double));
			dt.Columns.Add("DispatchWeight", typeof(double));
			dt.Columns.Add("BalWeight", typeof(double));
			dt.Columns.Add("Rate", typeof(double));
			dt.Columns.Add("ExpiryDate", typeof(DateTime));
			dt.Clear();
			for (int i = 0; i < grdlst.Rows.Count; i++)
			{
				dt.Rows.Add(grdlst.Rows[i]["Id"], Conversion.ToDateTime(grdlst.Rows[i]["DocDate"]).ToShortDateString(), grdlst.Rows[i]["DocNo"], grdlst.Rows[i]["SupplierName"], grdlst.Rows[i]["OrderSupCustId"], grdlst.Rows[i]["OrderItemId"], grdlst.Rows[i]["SaleGLAC"], grdlst.Rows[i]["DeliveryTerm"], grdlst.Rows[i]["ItemName"], grdlst.Rows[i]["Crop"], grdlst.Rows[i]["UOMCodeItm"], grdlst.Rows[i]["OrderItemQty"], grdlst.Rows[i]["DispatchQty"], Conversion.ToDouble(grdlst.Rows[i]["OrderItemQty"]) - Conversion.ToDouble(grdlst.Rows[i]["DispatchQty"]), grdlst.Rows[i]["NetWeight"], grdlst.Rows[i]["DispatchWeight"], Conversion.ToDouble(grdlst.Rows[i]["NetWeight"]) - Conversion.ToDouble(grdlst.Rows[i]["DispatchWeight"]), grdlst.Rows[i]["OrderItemRate"], Convert.ToDateTime(grdlst.Rows[i]["OrderExpiryDate"]).ToString("dd-MMM-yy"));
			}
			grd.DataSource = dt;
			grd.RetrieveStructure();
			GridSettingsForSale();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingsForSale()
	{
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["SupCustId"].Visible = false;
			grd.RootTable.Columns["SaleGLAC"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["ItemName"].Width = 200;
			grd.RootTable.Columns["CustomerName"].Width = 200;
			grd.RootTable.Columns["DocNo"].Width = 60;
			grd.RootTable.Columns["Crop"].Width = 60;
			grd.RootTable.Columns["PackUom"].Width = 70;
			grd.RootTable.Columns["Qty"].Width = 70;
			grd.RootTable.Columns["DispatchQty"].Width = 90;
			grd.RootTable.Columns["BalQty"].Width = 70;
			grd.RootTable.Columns["Weight"].Width = 80;
			grd.RootTable.Columns["DispatchWeight"].Width = 110;
			grd.RootTable.Columns["BalWeight"].Width = 80;
			grd.RootTable.Columns["ExpiryDate"].Width = 80;
			grd.RootTable.Columns["Rate"].Width = 80;
			grd.RootTable.Groups.Add("ItemName");
			grd.RootTable.GroupTotals = (GroupTotals)2;
			grd.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["CustomerName"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["ItemName"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["DispatchQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["DispatchWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BalWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["DispatchQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["DispatchWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BalWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Qty"].FormatString = "0,0";
			grd.RootTable.Columns["DispatchQty"].FormatString = "0,0";
			grd.RootTable.Columns["BalQty"].FormatString = "0,0";
			grd.RootTable.Columns["Weight"].FormatString = "0,0";
			grd.RootTable.Columns["DispatchWeight"].FormatString = "0,0";
			grd.RootTable.Columns["BalWeight"].FormatString = "0,0";
			grd.RootTable.Columns["Rate"].FormatString = "#,0.00";
			grd.RootTable.Columns["Qty"].TotalFormatString = "0,0";
			grd.RootTable.Columns["DispatchQty"].TotalFormatString = "0,0";
			grd.RootTable.Columns["BalQty"].TotalFormatString = "0,0";
			grd.RootTable.Columns["Weight"].TotalFormatString = "0,0";
			grd.RootTable.Columns["DispatchWeight"].TotalFormatString = "0,0";
			grd.RootTable.Columns["BalWeight"].TotalFormatString = "0,0";
			grd.RootTable.Columns["Rate"].TotalFormatString = "#,0.00";
			grd.RootTable.Columns.Add("Slip");
			grd.RootTable.Columns["Slip"].Caption = "Slip273";
			grd.RootTable.Columns["Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Slip"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Slip"].ButtonText = "Slip273";
			grd.RootTable.Columns["Slip"].Width = 60;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = grd.CurrentRow;
			int Id = Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Column.Key == "DocNo")
			{
				if (InvoiceType == 1)
				{
					new ActivityDetails(UserAccount, Id).Show();
				}
				else
				{
					new SaleActivityDetails(UserAccount, Id).Show();
				}
			}
			if (e.Column.Key == "SupplierName" || e.Column.Key == "CustomerName")
			{
				CommonServices.GoToCustomerLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["SupCustId"].Value), clsGlobalVariables.ActiveYr.Start_Period, DateTime.Now);
			}
			if (e.Column.Key == "ItemName")
			{
				if (InvoiceType == 1)
				{
					Conversion.ToInt(item.Cells["PurchaseGLAC"].Value);
				}
				else
				{
					Conversion.ToInt(item.Cells["SaleGLAC"].Value);
				}
				CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["ItemId"].Value), clsGlobalVariables.ActiveYr.Start_Period, DateTime.Now);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void BtnPrint205_Click(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = PurchaseOrderReports.PurchaseOrderDetailRegisterReport206(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 41,
				FromDate = clsGlobalVariables.ActiveYr.Start_Period,
				ToDate = DateTime.Now,
				ItemId = ItemId,
				Status = "Open",
				IsApproved = true
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "205-InvRptPurchaseOrderDetailRegisterRice.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void BtnPrint206_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = PurchaseOrderReports.PurchaseOrderDetailRegisterReport206(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 41,
				FromDate = clsGlobalVariables.ActiveYr.Start_Period,
				ToDate = DateTime.Now,
				ItemId = ItemId,
				Status = "Open",
				IsApproved = true
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@PrintedBy", UserAccount.UserName.ToString());
			val.ShowReportWithDataTable(dt, "206-RptPurchaseOrderRegisterRice.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void BtnPrint271_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (data.Rows.Count == 0 || data == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(data, "271-InvRptSalesOrderRegister.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Slip")
			{
				if (InvoiceType == 1)
				{
					GenerateReportForPurchaseOrder();
				}
				else
				{
					CommonServices.SaleOrderReports273(Conversion.ToInt(grd.CurrentRow.Cells["Id"].Value));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void GenerateReportForPurchaseOrder()
	{
		try
		{
			CommonServices.PurchaseOrderSlipReport203(Convert.ToInt32(grd.CurrentRow.Cells["Id"].Value));
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
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Expected O, but got Unknown
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.BtnPrint205 = new System.Windows.Forms.ToolStripButton();
		this.BtnPrint206 = new System.Windows.Forms.ToolStripButton();
		this.BtnPrint271 = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(935, 588);
		this.panel1.TabIndex = 0;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		this.grd.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 29);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(935, 559);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 5;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(935, 29);
		this.panel2.TabIndex = 4;
		this.toolStrip3.BackColor = System.Drawing.Color.White;
		this.toolStrip3.Dock = System.Windows.Forms.DockStyle.None;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.BtnPrint205, this.BtnPrint206, this.BtnPrint271 });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(397, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.BtnPrint205.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint205.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint205.Name = "BtnPrint205";
		this.BtnPrint205.Size = new System.Drawing.Size(140, 24);
		this.BtnPrint205.Text = "205-PoDetailRegister";
		this.BtnPrint205.Click += new System.EventHandler(BtnPrint205_Click);
		this.BtnPrint206.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint206.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint206.Name = "BtnPrint206";
		this.BtnPrint206.Size = new System.Drawing.Size(98, 24);
		this.BtnPrint206.Text = "206-PoDetail";
		this.BtnPrint206.Click += new System.EventHandler(BtnPrint206_Click);
		this.BtnPrint271.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint271.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint271.Name = "BtnPrint271";
		this.BtnPrint271.Size = new System.Drawing.Size(147, 24);
		this.BtnPrint271.Text = "271-RegisterSummery";
		this.BtnPrint271.Click += new System.EventHandler(BtnPrint271_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(935, 588);
		base.Controls.Add(this.panel1);
		base.Name = "OrderManagemantPopUpForPurchaseSale";
		this.Text = "Order Managemant PopUp For Purchase Sale";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(OrderManagemantPopUpForPurchaseSale_Load);
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
