using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.FeedMill.Sales;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.FeedMill.Sale;

public class LoadSaleOrderFeed : Form
{
	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	public DataTable dtLoader = new DataTable();

	private DataTable dtGrid = new DataTable();

	private DataTable dtDetail = new DataTable();

	private IContainer components;

	private Panel panel1;

	private Label label23;

	private Panel panel2;

	private Button btngrnlod;

	private DateTimePicker Todate;

	private DateTimePicker FromDate;

	private Label label1;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel3;

	internal GridEX grdMain;

	private Button btnLoadOnInvoice;

	private Label label2;

	internal UltraCombo CmbSupplier;

	private Button btnReset;

	private GroupBox groupBox1;

	private ToolStrip toolStrip1;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripButton btnNew;

	private ToolStripButton btnRefresh;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton btnShortcutKeys;

	private ToolStripSeparator toolStripSeparator1;

	private Label label9;

	internal UltraCombo cmbItem;

	private Label label3;

	internal GridEX grdDetail;

	private Panel panel4;

	private GroupBox groupBox3;

	private Label label7;

	private Label label11;

	private Label label10;

	private Label label24;

	private Label label25;

	private TextBox txtBalanceWeightHeadrer;

	private TextBox txtBalanceQtyHeader;

	private TextBox txtDispatchWeightHeader;

	private TextBox txtDispatchQtyHeader;

	private TextBox txtOrderWeightHeader;

	private TextBox txtOrderQtyHeader;

	public LoadSaleOrderFeed(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void LoadInvoices_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 700);
			FromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			FromDate.Focus();
			dtDetail.Columns.Add("Id", typeof(int));
			dtDetail.Columns.Add("DetailId", typeof(int));
			dtDetail.Columns.Add("SupplierCustomerId", typeof(int));
			dtDetail.Columns.Add("PaymentTerm", typeof(string));
			dtDetail.Columns.Add("DeliveryTerm", typeof(string));
			dtDetail.Columns.Add("ItemCode", typeof(string));
			dtDetail.Columns.Add("ItemName", typeof(string));
			dtDetail.Columns.Add("PackUom", typeof(string));
			dtDetail.Columns.Add("ItemQty", typeof(decimal));
			dtDetail.Columns.Add("DispatchQty", typeof(decimal));
			dtDetail.Columns.Add("BalQty", typeof(decimal));
			dtDetail.Columns.Add("NetWeight", typeof(decimal));
			dtDetail.Columns.Add("DispatchWeight", typeof(decimal));
			dtDetail.Columns.Add("BalWeight", typeof(decimal));
			dtDetail.Columns.Add("RateUom", typeof(string));
			dtDetail.Columns.Add("ItemRate", typeof(decimal));
			dtDetail.Columns.Add("TaxName", typeof(string));
			dtDetail.Columns.Add("TaxPercent", typeof(decimal));
			dtDetail.Columns.Add("TaxAmount", typeof(decimal));
			dtDetail.Columns.Add("JobLot", typeof(string));
			dtDetail.Columns.Add("CityName", typeof(string));
			dtDetail.Columns.Add("RemarksDetail", typeof(string));
			ItemNameFill();
			CustomerFill();
			PendingSaleOrderForLoad();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemNameFill()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		try
		{
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id");
			dtItem.Columns.Add("name");
			DataTable dt = SaleOrder.GetDataForDropDownFromSaleOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeIds = "1110",
				Activity = "Item"
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
			}
			DDL.BindDDL(dtItem, cmbItem, "Id", "name", "Item Name", ZeroIndex: false);
			((UltraGridBase)cmbItem).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomerFill()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		try
		{
			DataTable dtSupplieerCustomer = new DataTable();
			dtSupplieerCustomer.Columns.Add("Id");
			dtSupplieerCustomer.Columns.Add("name");
			DataTable dt = SaleOrder.GetDataForDropDownFromSaleOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeIds = "1110",
				Activity = "Customer"
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtSupplieerCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				DDL.BindDDL(dtSupplieerCustomer, CmbSupplier, "Id", "name", "Party Name", ZeroIndex: false);
				((UltraGridBase)CmbSupplier).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnReset_Click(object sender, EventArgs e)
	{
		try
		{
			FromDate.Focus();
			dtDetail.Rows.Clear();
			dtGrid.Rows.Clear();
			((Control)(object)CmbSupplier).Text = string.Empty;
			((Control)(object)cmbItem).Text = string.Empty;
			txtOrderQtyHeader.Text = "0";
			txtDispatchQtyHeader.Text = "0";
			txtBalanceQtyHeader.Text = "0";
			txtOrderWeightHeader.Text = "0";
			txtDispatchWeightHeader.Text = "0";
			txtBalanceWeightHeadrer.Text = "0";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		dtLoader = null;
		Hide();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			CustomerFill();
			ItemNameFill();
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
			PendingSaleOrderForLoad();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PendingSaleOrderForLoad()
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		try
		{
			txtOrderQtyHeader.Text = "0";
			txtDispatchQtyHeader.Text = "0";
			txtBalanceQtyHeader.Text = "0";
			txtOrderWeightHeader.Text = "0";
			txtDispatchWeightHeader.Text = "0";
			txtBalanceWeightHeadrer.Text = "0";
			grdDetail.ClearStructure();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = Convert.ToDateTime(FromDate.Value);
			obj.ToDate = Convert.ToDateTime(Todate.Value);
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			obj.ItemId = Conversion.ToInt(cmbItem.Value);
			dtGrid = SaleOrder.GetPendingSaleOrders(obj);
			if (dtGrid.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("DocNo", typeof(string));
			dt.Columns.Add("DocDate", typeof(DateTime));
			dt.Columns.Add("OrderSupCustId", typeof(string));
			dt.Columns.Add("PartyName", typeof(string));
			dt.Columns.Add("PaymentTerm", typeof(string));
			dt.Columns.Add("DueDays", typeof(int));
			dt.Columns.Add("DueDate", typeof(DateTime));
			dt.Columns.Add("DeliveryTerm", typeof(string));
			dt.Columns.Add("OrderQty", typeof(decimal));
			dt.Columns.Add("DispatchQty", typeof(decimal));
			dt.Columns.Add("BalQty", typeof(decimal));
			dt.Columns.Add("OrderWeight", typeof(decimal));
			dt.Columns.Add("DispatchWeight", typeof(decimal));
			dt.Columns.Add("BalWeight", typeof(decimal));
			dt.Columns.Add("RemarksHeader", typeof(string));
			foreach (DataRow row in dtGrid.Rows)
			{
				if (!dt.AsEnumerable().Any((DataRow r) => r.Field<int>("Id") == Conversion.ToInt(row["Id"])))
				{
					double Qty = Conversion.ToDouble((object)(from r in dtGrid.AsEnumerable()
						where r.Field<int>("Id") == Conversion.ToInt(row["Id"])
						select r).Sum((DataRow r) => r.Field<double>("OrderQty")));
					double DispatchQty = Conversion.ToDouble((object)(from r in dtGrid.AsEnumerable()
						where r.Field<int>("Id") == Conversion.ToInt(row["Id"])
						select r).Sum((DataRow r) => r.Field<decimal>("DispatchQty")));
					double BalQty = Conversion.ToDouble((object)(from r in dtGrid.AsEnumerable()
						where r.Field<int>("Id") == Conversion.ToInt(row["Id"])
						select r).Sum((DataRow r) => r.Field<double>("BalQty")));
					double Weight = Conversion.ToDouble((object)(from r in dtGrid.AsEnumerable()
						where r.Field<int>("Id") == Conversion.ToInt(row["Id"])
						select r).Sum((DataRow r) => r.Field<double>("OrderWeight")));
					double DispatchWeight = Conversion.ToDouble((object)(from r in dtGrid.AsEnumerable()
						where r.Field<int>("Id") == Conversion.ToInt(row["Id"])
						select r).Sum((DataRow r) => r.Field<decimal>("DispatchWeight")));
					double BalWeight = Conversion.ToDouble((object)(from r in dtGrid.AsEnumerable()
						where r.Field<int>("Id") == Conversion.ToInt(row["Id"])
						select r).Sum((DataRow r) => r.Field<double>("BalWeight")));
					dt.Rows.Add(row["Id"], row["DocNo"], Conversion.ToDateTime(row["DocDate"]).ToShortDateString(), row["OrderSupCustId"], row["CustomerName"], row["TermsDescription"], row["OrderDueDays"], Conversion.ToDateTime(row["OrderDueDate"]).ToShortDateString(), row["DeliveryTerm"], Qty, DispatchQty, BalQty, Weight, DispatchWeight, BalWeight, row["RemarksHeader"]);
				}
			}
			grdMain.DataSource = dt;
			grdMain.RetrieveStructure();
			grdSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateTotal()
	{
		try
		{
			if (grdDetail.GetCheckedRows().Count() > 0)
			{
				decimal OrderQty = default(decimal);
				decimal OrderWeight = default(decimal);
				decimal DispatchQty = default(decimal);
				decimal DispatchWeight = default(decimal);
				decimal BalQty = default(decimal);
				decimal BalWeight = default(decimal);
				GridEXRow[] checkedRows = grdDetail.GetCheckedRows();
				foreach (GridEXRow item in checkedRows)
				{
					OrderQty += Conversion.ToDecimal(item.Cells["ItemQty"].Value);
					DispatchQty += Conversion.ToDecimal(item.Cells["DispatchQty"].Value);
					BalQty += Conversion.ToDecimal(item.Cells["BalQty"].Value);
					OrderWeight += Conversion.ToDecimal(item.Cells["NetWeight"].Value);
					DispatchWeight += Conversion.ToDecimal(item.Cells["DispatchWeight"].Value);
					BalWeight += Conversion.ToDecimal(item.Cells["BalWeight"].Value);
				}
				txtOrderQtyHeader.Text = OrderQty.ToString(clsGlobalVariables.stringFormatsingle);
				txtOrderWeightHeader.Text = OrderWeight.ToString(clsGlobalVariables.stringFormatsingle);
				txtDispatchQtyHeader.Text = DispatchQty.ToString(clsGlobalVariables.stringFormatsingle);
				txtDispatchWeightHeader.Text = DispatchWeight.ToString(clsGlobalVariables.stringFormatsingle);
				txtBalanceQtyHeader.Text = BalQty.ToString(clsGlobalVariables.stringFormatsingle);
				txtBalanceWeightHeadrer.Text = BalWeight.ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtOrderQtyHeader.Text = 0.ToString();
				txtOrderWeightHeader.Text = 0.ToString();
				txtDispatchQtyHeader.Text = 0.ToString();
				txtDispatchWeightHeader.Text = 0.ToString();
				txtBalanceQtyHeader.Text = 0.ToString();
				txtBalanceWeightHeadrer.Text = 0.ToString();
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
			grdMain.RootTable.Columns["Id"].Visible = false;
			grdMain.RootTable.Columns["OrderSupCustId"].Visible = false;
			grdMain.RootTable.Columns["DocNo"].Width = 60;
			grdMain.RootTable.Columns["DocDate"].Width = 80;
			grdMain.RootTable.Columns["PartyName"].Width = 150;
			grdMain.RootTable.Columns["PaymentTerm"].Width = 90;
			grdMain.RootTable.Columns["DueDays"].Width = 70;
			grdMain.RootTable.Columns["DueDate"].Width = 80;
			grdMain.RootTable.Columns["DeliveryTerm"].Width = 90;
			grdMain.RootTable.Columns["OrderQty"].Width = 90;
			grdMain.RootTable.Columns["DispatchQty"].Width = 100;
			grdMain.RootTable.Columns["BalQty"].Width = 100;
			grdMain.RootTable.Columns["OrderWeight"].Width = 100;
			grdMain.RootTable.Columns["DispatchWeight"].Width = 110;
			grdMain.RootTable.Columns["BalWeight"].Width = 100;
			grdMain.RootTable.Columns["RemarksHeader"].Width = 150;
			grdMain.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			grdMain.RootTable.Columns["OrderQty"].AggregateFunction = (AggregateFunction)2;
			grdMain.RootTable.Columns["OrderQty"].TextAlignment = (TextAlignment)3;
			grdMain.RootTable.Columns["OrderQty"].FormatString = "#,##0.##";
			grdMain.RootTable.Columns["OrderQty"].TotalFormatString = "#,##0.###";
			grdMain.RootTable.Columns["DispatchQty"].AggregateFunction = (AggregateFunction)2;
			grdMain.RootTable.Columns["DispatchQty"].TextAlignment = (TextAlignment)3;
			grdMain.RootTable.Columns["DispatchQty"].FormatString = "#,##0.###";
			grdMain.RootTable.Columns["DispatchQty"].TotalFormatString = "#,##0.###";
			grdMain.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			grdMain.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			grdMain.RootTable.Columns["BalQty"].FormatString = "#,##0.###";
			grdMain.RootTable.Columns["BalQty"].TotalFormatString = "#,##0.###";
			grdMain.RootTable.Columns["OrderWeight"].AggregateFunction = (AggregateFunction)2;
			grdMain.RootTable.Columns["OrderWeight"].TextAlignment = (TextAlignment)3;
			grdMain.RootTable.Columns["OrderWeight"].FormatString = "##,#.##";
			grdMain.RootTable.Columns["OrderWeight"].TotalFormatString = "##,#.##";
			grdMain.RootTable.Columns["DispatchWeight"].AggregateFunction = (AggregateFunction)2;
			grdMain.RootTable.Columns["DispatchWeight"].TextAlignment = (TextAlignment)3;
			grdMain.RootTable.Columns["DispatchWeight"].FormatString = "#,##0.###";
			grdMain.RootTable.Columns["DispatchWeight"].TotalFormatString = "#,##0.###";
			grdMain.RootTable.Columns["BalWeight"].AggregateFunction = (AggregateFunction)2;
			grdMain.RootTable.Columns["BalWeight"].TextAlignment = (TextAlignment)3;
			grdMain.RootTable.Columns["BalWeight"].FormatString = "#,##0.###";
			grdMain.RootTable.Columns["BalWeight"].TotalFormatString = "#,##0.###";
			grdMain.RootTable.Columns.Add("Select");
			grdMain.RootTable.Columns["Select"].ActAsSelector = true;
			grdMain.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdMain.RootTable.Columns["Select"].Width = 40;
			grdMain.RootTable.Columns["Select"].Position = 0;
			grdMain.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdMain_SelectionChanged(object sender, EventArgs e)
	{
	}

	private void DetailGridBind()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdMain.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				if (item.Cells.Count > 0)
				{
					DataTable dtOrder = new DataTable();
					dtOrder.Columns.Add("Id", typeof(int));
					dtOrder.Columns.Add("DetailId", typeof(int));
					dtOrder.Columns.Add("SupplierCustomerId", typeof(int));
					dtOrder.Columns.Add("PaymentTerm", typeof(string));
					dtOrder.Columns.Add("DeliveryTerm", typeof(string));
					dtOrder.Columns.Add("ItemCode", typeof(string));
					dtOrder.Columns.Add("ItemName", typeof(string));
					dtOrder.Columns.Add("PackUom", typeof(string));
					dtOrder.Columns.Add("ItemQty", typeof(decimal));
					dtOrder.Columns.Add("DispatchQty", typeof(decimal));
					dtOrder.Columns.Add("BalQty", typeof(decimal));
					dtOrder.Columns.Add("NetWeight", typeof(decimal));
					dtOrder.Columns.Add("DispatchWeight", typeof(decimal));
					dtOrder.Columns.Add("BalWeight", typeof(decimal));
					dtOrder.Columns.Add("RateUom", typeof(string));
					dtOrder.Columns.Add("ItemRate", typeof(decimal));
					dtOrder.Columns.Add("TaxName", typeof(string));
					dtOrder.Columns.Add("TaxPercent", typeof(decimal));
					dtOrder.Columns.Add("TaxAmount", typeof(decimal));
					dtOrder.Columns.Add("JobLot", typeof(string));
					dtOrder.Columns.Add("CityName", typeof(string));
					dtOrder.Columns.Add("RemarksDetail", typeof(string));
					dtOrder.Rows.Clear();
					DataTable resultRows = (from row in dtGrid.AsEnumerable()
						where row.Field<int>("Id") == Conversion.ToInt(item.Cells["Id"].Value)
						select row).CopyToDataTable();
					for (int i = 0; i < resultRows.Rows.Count; i++)
					{
						dtOrder.Rows.Add(resultRows.Rows[i]["Id"], resultRows.Rows[i]["OrderDetailId"], resultRows.Rows[i]["OrderSupCustId"], resultRows.Rows[i]["TermsDescription"], resultRows.Rows[i]["DeliveryTerm"], resultRows.Rows[i]["ItemCode"], resultRows.Rows[i]["ItemName"], resultRows.Rows[i]["PackUom"], resultRows.Rows[i]["OrderQty"], resultRows.Rows[i]["DispatchQty"], resultRows.Rows[i]["BalQty"], resultRows.Rows[i]["OrderWeight"], resultRows.Rows[i]["DispatchWeight"], resultRows.Rows[i]["BalWeight"], resultRows.Rows[i]["ItemRate"], resultRows.Rows[i]["RateUom"], resultRows.Rows[i]["TaxName"], resultRows.Rows[i]["TaxPercent"], resultRows.Rows[i]["TaxAmount"], resultRows.Rows[i]["JobLotDescription"], resultRows.Rows[i]["CityName"], resultRows.Rows[i]["RemarksDetail"]);
					}
					grdDetail.DataSource = dtOrder;
					grdDetail.RetrieveStructure();
					GridDetailSetting();
				}
			}
			else
			{
				grdDetail.ClearStructure();
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
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["DetailId"].Visible = false;
			grdDetail.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdDetail.RootTable.Columns["PaymentTerm"].Visible = false;
			grdDetail.RootTable.Columns["DeliveryTerm"].Visible = false;
			grdDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemQty"].FormatString = "##,#.##";
			grdDetail.RootTable.Columns["ItemQty"].TotalFormatString = "##,#.##";
			grdDetail.RootTable.Columns["DispatchQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["DispatchQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["DispatchQty"].FormatString = "##,#.##";
			grdDetail.RootTable.Columns["DispatchQty"].TotalFormatString = "##,#.##";
			grdDetail.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["BalQty"].FormatString = "##,#.##";
			grdDetail.RootTable.Columns["BalQty"].TotalFormatString = "##,#.##";
			grdDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetWeight"].FormatString = "##,#.##";
			grdDetail.RootTable.Columns["NetWeight"].TotalFormatString = "##,#.##";
			grdDetail.RootTable.Columns["DispatchWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["DispatchWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["DispatchWeight"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["DispatchWeight"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["BalWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["BalWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["BalWeight"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["BalWeight"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["TaxPercent"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["TaxPercent"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["TaxAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["TaxAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["TaxAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["TaxAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdDetail);
			grdDetail.RootTable.Columns.Add("Select");
			grdDetail.RootTable.Columns["Select"].ActAsSelector = true;
			grdDetail.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdDetail.RootTable.Columns["Select"].Width = 40;
			grdDetail.RootTable.Columns["Select"].Position = 0;
			grdDetail.FrozenColumns = 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CheckedAllDetailRows()
	{
		try
		{
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow obj in rows)
			{
				obj.BeginEdit();
				obj.IsChecked = Convert.ToBoolean(value: true);
				obj.EndEdit();
			}
			grdDetail.UpdateData();
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
			dtLoader = dtGrid.Clone();
			if (Conversion.ToInt((object)grdDetail.GetCheckedRows().Count().ToString()) > 0)
			{
				int OrderSupCustId = 0;
				int OrderDetailId = 0;
				dtLoader.Rows.Clear();
				string DeliveryTerm = "";
				string PaymentTerm = "";
				int OrderId = 0;
				GridEXRow[] checkedRows = grdDetail.GetCheckedRows();
				foreach (GridEXRow obj in checkedRows)
				{
					OrderDetailId = Conversion.ToInt(obj.Cells["DetailId"].Value);
					int DocId = Conversion.ToInt(obj.Cells["Id"].Value);
					int SupplierId = Conversion.ToInt(obj.Cells["SupplierCustomerId"].Value);
					string delTerm = Conversion.ToString(obj.Cells["DeliveryTerm"].Value);
					string payTerm = Conversion.ToString(obj.Cells["PaymentTerm"].Value);
					if (OrderSupCustId == 0)
					{
						OrderSupCustId = SupplierId;
					}
					if (OrderId == 0)
					{
						OrderId = DocId;
					}
					if (DeliveryTerm == "")
					{
						DeliveryTerm = delTerm;
					}
					if (PaymentTerm == "")
					{
						PaymentTerm = payTerm;
					}
					if (OrderId != DocId)
					{
						dtLoader.Rows.Clear();
						MessageBox.Show("You Can Only Select Rows Of Same Order");
						return;
					}
					if (OrderSupCustId != SupplierId)
					{
						dtLoader.Rows.Clear();
						MessageBox.Show("You Can Only Select Rows Of Same Supplier");
						return;
					}
					if (DeliveryTerm != delTerm)
					{
						dtLoader.Rows.Clear();
						MessageBox.Show("You Can Only Select Rows Of Same Delivery Term");
						return;
					}
					if (PaymentTerm != payTerm)
					{
						dtLoader.Rows.Clear();
						MessageBox.Show("You Can Only Select Rows Of Same Payment Term");
						return;
					}
					for (int i = 0; i < dtGrid.Rows.Count; i++)
					{
						if (Conversion.ToInt(dtGrid.Rows[i]["OrderDetailId"]) == OrderDetailId)
						{
							DataRow dr = dtGrid.Rows[i];
							dtLoader.Rows.Add(dr.ItemArray);
						}
					}
				}
				Hide();
			}
			else
			{
				MessageBox.Show("Check the Row first");
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdMain).Name);
			if (fs != null)
			{
				grdMain.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOrderNoFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtOrderNoTo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+L", "To Press Load Button");
			dt.Rows.Add("Ctrl+S", "For Search");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
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

	private void btnShortcutKeys_Click(object sender, EventArgs e)
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

	private void LoadPurchaseOrder_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			else if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				dtLoader = null;
				Hide();
			}
			else if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			else if (e.Control && e.KeyCode == Keys.S)
			{
				btngrnlod_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.L)
			{
				btnLoadOnInvoice_Click_1(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.N)
			{
				btnReset_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.F5)
			{
				FromDate.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdMain).Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Up)
			{
				FromDate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), 1100);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdMain_RowCheckStateChanged(object sender, RowCheckStateChangeEventArgs e)
	{
		try
		{
			GridEXRow row = grdMain.CurrentRow;
			if (row.IsChecked)
			{
				if (grdDetail.GetRows().Count() > 0)
				{
					GridEXRow[] checkedRows = grdMain.GetCheckedRows();
					foreach (GridEXRow obj in checkedRows)
					{
						if (Conversion.ToInt(obj.Cells["Id"].Value) != Conversion.ToInt(row.Cells["Id"].Value))
						{
							row.IsChecked = false;
							throw new Exception("You can only select one order at a time");
						}
						if (Conversion.ToInt(obj.Cells["OrderSupCustId"].Value) != Conversion.ToInt(row.Cells["OrderSupCustId"].Value))
						{
							row.IsChecked = false;
							throw new Exception("Select Same Supplier Row");
						}
						if (Conversion.ToString(obj.Cells["PaymentTerm"].Value) != Conversion.ToString(row.Cells["PaymentTerm"].Value))
						{
							row.IsChecked = false;
							throw new Exception("Select Same Payment Term Rows");
						}
						if (Conversion.ToString(obj.Cells["DeliveryTerm"].Value) != Conversion.ToString(row.Cells["DeliveryTerm"].Value))
						{
							row.IsChecked = false;
							throw new Exception("Select Same Delivery Term Rows");
						}
					}
				}
				BindDetailToDetailGridOfRowId(Conversion.ToInt(row.Cells["Id"].Value));
			}
			else
			{
				DeleteDetailFromDetailGridOfRowId(Conversion.ToInt(row.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BindDetailToDetailGridOfRowId(int Id)
	{
		try
		{
			int InitialCount = dtDetail.Rows.Count;
			EnumerableRowCollection<DataRow> Rows = from row in dtGrid.AsEnumerable()
				where row.Field<int>("Id") == Id
				select row;
			DataTable detail = (Rows.Any() ? Rows.CopyToDataTable() : dtGrid.Clone());
			if (detail.Rows.Count > 0)
			{
				for (int j = 0; j < detail.Rows.Count; j++)
				{
					dtDetail.Rows.Add(detail.Rows[j]["Id"], detail.Rows[j]["OrderDetailId"], detail.Rows[j]["OrderSupCustId"], detail.Rows[j]["TermsDescription"], detail.Rows[j]["DeliveryTerm"], detail.Rows[j]["ItemCode"], detail.Rows[j]["ItemName"], detail.Rows[j]["PackUom"], detail.Rows[j]["OrderQty"], detail.Rows[j]["DispatchQty"], detail.Rows[j]["BalQty"], detail.Rows[j]["OrderWeight"], detail.Rows[j]["DispatchWeight"], detail.Rows[j]["BalWeight"], detail.Rows[j]["RateUom"], detail.Rows[j]["ItemRate"], detail.Rows[j]["TaxName"], detail.Rows[j]["TaxPercent"], detail.Rows[j]["TaxAmount"], detail.Rows[j]["JobLotDescription"], detail.Rows[j]["CityName"], detail.Rows[j]["RemarksDetail"]);
				}
				grdDetail.DataSource = dtDetail;
				if (InitialCount == 0)
				{
					grdDetail.RetrieveStructure();
					GridDetailSetting();
				}
				int FinalCount = dtDetail.Rows.Count;
				if (InitialCount == 0)
				{
					CheckedAllDetailRows();
				}
				else
				{
					for (int i = InitialCount; i < FinalCount; i++)
					{
						GridEXRow row2 = grdDetail.GetRow(InitialCount++);
						row2.BeginEdit();
						row2.IsChecked = true;
						row2.EndEdit();
					}
					grdDetail.UpdateData();
				}
			}
			CalculateTotal();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteDetailFromDetailGridOfRowId(int Id)
	{
		try
		{
			EnumerableRowCollection<DataRow> Rows = from row in dtDetail.AsEnumerable()
				where row.Field<int>("Id") == Id
				select row;
			DataTable detail = (Rows.Any() ? Rows.CopyToDataTable() : dtDetail.Clone());
			if (detail.Rows.Count > 0)
			{
				int rowIndex = -1;
				for (int j = 0; j < dtDetail.Rows.Count; j++)
				{
					if (Conversion.ToInt(dtDetail.Rows[j]["Id"]) == Id)
					{
						rowIndex = j;
						break;
					}
				}
				if (rowIndex != -1)
				{
					for (int i = 0; i < detail.Rows.Count; i++)
					{
						dtDetail.Rows[rowIndex].Delete();
					}
				}
			}
			CalculateTotal();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_RowCheckStateChanged(object sender, RowCheckStateChangeEventArgs e)
	{
		try
		{
			CalculateTotal();
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
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Expected O, but got Unknown
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Expected O, but got Unknown
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Expected O, but got Unknown
		//IL_07a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b3: Expected O, but got Unknown
		//IL_07d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e1: Expected O, but got Unknown
		//IL_1ff5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fff: Expected O, but got Unknown
		GridEXLayout grdMain_DesignTimeLayout = new GridEXLayout();
		Appearance appearance8 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnShortcutKeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.label23 = new System.Windows.Forms.Label();
		this.grdMain = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.label7 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.txtBalanceWeightHeadrer = new System.Windows.Forms.TextBox();
		this.txtBalanceQtyHeader = new System.Windows.Forms.TextBox();
		this.txtDispatchWeightHeader = new System.Windows.Forms.TextBox();
		this.txtDispatchQtyHeader = new System.Windows.Forms.TextBox();
		this.txtOrderWeightHeader = new System.Windows.Forms.TextBox();
		this.txtOrderQtyHeader = new System.Windows.Forms.TextBox();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbItem = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.btnReset = new System.Windows.Forms.Button();
		this.btnLoadOnInvoice = new System.Windows.Forms.Button();
		this.CmbSupplier = new UltraCombo();
		this.btngrnlod = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.Todate = new System.Windows.Forms.DateTimePicker();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdDetail = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdMain).BeginInit();
		this.panel2.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Controls.Add(this.label23);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1034, 50);
		this.panel1.TabIndex = 0;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.toolStripSeparator6, this.btnNew, this.btnRefresh, this.toolStripSeparator2, this.btnShortcutKeys, this.toolStripSeparator1 });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1034, 25);
		this.toolStrip1.TabIndex = 27;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 22);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnReset_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 22);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
		this.btnShortcutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutKeys.Name = "btnShortcutKeys";
		this.btnShortcutKeys.Size = new System.Drawing.Size(122, 22);
		this.btnShortcutKeys.Text = "ShortCut Keys";
		this.btnShortcutKeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.Teal;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(4, 28);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(166, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "Pending Sale Order";
		this.grdMain.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdMain).BackColor = System.Drawing.Color.White;
		grdMain_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdMain.DesignTimeLayout = grdMain_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdMain).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdMain.FilterMode = (FilterMode)2;
		this.grdMain.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdMain).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdMain.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdMain).Location = new System.Drawing.Point(0, 212);
		((System.Windows.Forms.Control)(object)this.grdMain).Name = "grdMain";
		((ScrollableBase)this.grdMain).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdMain).Size = new System.Drawing.Size(1034, 240);
		((System.Windows.Forms.Control)(object)this.grdMain).TabIndex = 0;
		this.grdMain.TotalRow = (InheritableBoolean)1;
		this.grdMain.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdMain).VisualStyle = (VisualStyle)4;
		this.grdMain.RowCheckStateChanged += new RowCheckStateChangeEventHandler(grdMain_RowCheckStateChanged);
		this.grdMain.SelectionChanged += new System.EventHandler(grdMain_SelectionChanged);
		this.grdMain.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.groupBox3);
		this.panel2.Controls.Add(this.groupBox1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 50);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1034, 136);
		this.panel2.TabIndex = 0;
		this.groupBox3.Controls.Add(this.label7);
		this.groupBox3.Controls.Add(this.label11);
		this.groupBox3.Controls.Add(this.label10);
		this.groupBox3.Controls.Add(this.label24);
		this.groupBox3.Controls.Add(this.label25);
		this.groupBox3.Controls.Add(this.txtBalanceWeightHeadrer);
		this.groupBox3.Controls.Add(this.txtBalanceQtyHeader);
		this.groupBox3.Controls.Add(this.txtDispatchWeightHeader);
		this.groupBox3.Controls.Add(this.txtDispatchQtyHeader);
		this.groupBox3.Controls.Add(this.txtOrderWeightHeader);
		this.groupBox3.Controls.Add(this.txtOrderQtyHeader);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(372, -1);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(304, 133);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Total Of Selected Row";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(2, 79);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(54, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "Dispatch";
		this.label11.BackColor = System.Drawing.Color.Silver;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(180, 22);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(116, 20);
		this.label11.TabIndex = 0;
		this.label11.Text = "Weight";
		this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label10.BackColor = System.Drawing.Color.Silver;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(61, 22);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(116, 20);
		this.label10.TabIndex = 0;
		this.label10.Text = "Qty";
		this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(2, 50);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(59, 15);
		this.label24.TabIndex = 0;
		this.label24.Text = "Order Qty";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(2, 108);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(48, 15);
		this.label25.TabIndex = 0;
		this.label25.Text = "Balance";
		this.txtBalanceWeightHeadrer.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceWeightHeadrer.Location = new System.Drawing.Point(180, 104);
		this.txtBalanceWeightHeadrer.Name = "txtBalanceWeightHeadrer";
		this.txtBalanceWeightHeadrer.ShortcutsEnabled = false;
		this.txtBalanceWeightHeadrer.Size = new System.Drawing.Size(116, 23);
		this.txtBalanceWeightHeadrer.TabIndex = 5;
		this.txtBalanceQtyHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceQtyHeader.Location = new System.Drawing.Point(61, 104);
		this.txtBalanceQtyHeader.Name = "txtBalanceQtyHeader";
		this.txtBalanceQtyHeader.ShortcutsEnabled = false;
		this.txtBalanceQtyHeader.Size = new System.Drawing.Size(116, 23);
		this.txtBalanceQtyHeader.TabIndex = 2;
		this.txtDispatchWeightHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDispatchWeightHeader.Location = new System.Drawing.Point(180, 75);
		this.txtDispatchWeightHeader.Name = "txtDispatchWeightHeader";
		this.txtDispatchWeightHeader.ShortcutsEnabled = false;
		this.txtDispatchWeightHeader.Size = new System.Drawing.Size(116, 23);
		this.txtDispatchWeightHeader.TabIndex = 4;
		this.txtDispatchQtyHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDispatchQtyHeader.Location = new System.Drawing.Point(61, 75);
		this.txtDispatchQtyHeader.Name = "txtDispatchQtyHeader";
		this.txtDispatchQtyHeader.ShortcutsEnabled = false;
		this.txtDispatchQtyHeader.Size = new System.Drawing.Size(116, 23);
		this.txtDispatchQtyHeader.TabIndex = 1;
		this.txtOrderWeightHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderWeightHeader.Location = new System.Drawing.Point(180, 46);
		this.txtOrderWeightHeader.Name = "txtOrderWeightHeader";
		this.txtOrderWeightHeader.ShortcutsEnabled = false;
		this.txtOrderWeightHeader.Size = new System.Drawing.Size(116, 23);
		this.txtOrderWeightHeader.TabIndex = 3;
		this.txtOrderQtyHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderQtyHeader.Location = new System.Drawing.Point(61, 46);
		this.txtOrderQtyHeader.Name = "txtOrderQtyHeader";
		this.txtOrderQtyHeader.ShortcutsEnabled = false;
		this.txtOrderQtyHeader.Size = new System.Drawing.Size(116, 23);
		this.txtOrderQtyHeader.TabIndex = 0;
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.btnReset);
		this.groupBox1.Controls.Add(this.btnLoadOnInvoice);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.groupBox1.Controls.Add(this.btngrnlod);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.FromDate);
		this.groupBox1.Controls.Add(this.Todate);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, -1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(365, 131);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(10, 69);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(67, 15);
		this.label9.TabIndex = 116;
		this.label9.Text = "Item Name";
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance10).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance10).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance11;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance16).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(91, 63);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 7;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(10, 42);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(48, 15);
		this.label3.TabIndex = 12;
		this.label3.Text = "To Date";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(10, 18);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 12;
		this.label1.Text = "From Date";
		this.btnReset.BackColor = System.Drawing.Color.Teal;
		this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnReset.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnReset.ForeColor = System.Drawing.Color.White;
		this.btnReset.Location = new System.Drawing.Point(293, 15);
		this.btnReset.Name = "btnReset";
		this.btnReset.Size = new System.Drawing.Size(61, 26);
		this.btnReset.TabIndex = 11;
		this.btnReset.Text = "Reset";
		this.btnReset.UseVisualStyleBackColor = false;
		this.btnReset.Click += new System.EventHandler(btnReset_Click);
		this.btnLoadOnInvoice.BackColor = System.Drawing.Color.Teal;
		this.btnLoadOnInvoice.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnLoadOnInvoice.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadOnInvoice.ForeColor = System.Drawing.Color.White;
		this.btnLoadOnInvoice.Location = new System.Drawing.Point(293, 44);
		this.btnLoadOnInvoice.Name = "btnLoadOnInvoice";
		this.btnLoadOnInvoice.Size = new System.Drawing.Size(61, 26);
		this.btnLoadOnInvoice.TabIndex = 10;
		this.btnLoadOnInvoice.Text = "Load";
		this.btnLoadOnInvoice.UseVisualStyleBackColor = false;
		this.btnLoadOnInvoice.Click += new System.EventHandler(btnLoadOnInvoice_Click_1);
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(91, 91);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 8;
		this.btngrnlod.BackColor = System.Drawing.Color.Teal;
		this.btngrnlod.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btngrnlod.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btngrnlod.ForeColor = System.Drawing.Color.White;
		this.btngrnlod.Location = new System.Drawing.Point(293, 73);
		this.btngrnlod.Name = "btngrnlod";
		this.btngrnlod.Size = new System.Drawing.Size(61, 26);
		this.btngrnlod.TabIndex = 9;
		this.btngrnlod.Text = "Show";
		this.btngrnlod.UseVisualStyleBackColor = false;
		this.btngrnlod.Click += new System.EventHandler(btngrnlod_Click);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(10, 97);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(69, 15);
		this.label2.TabIndex = 24;
		this.label2.Text = "Party Name";
		this.FromDate.CustomFormat = "dd-MMM-yyyy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(91, 14);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(196, 23);
		this.FromDate.TabIndex = 0;
		this.Todate.CustomFormat = "dd-MMM-yyyy";
		this.Todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.Todate.Location = new System.Drawing.Point(91, 38);
		this.Todate.Name = "Todate";
		this.Todate.Size = new System.Drawing.Size(196, 23);
		this.Todate.TabIndex = 1;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 186);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1034, 26);
		this.panel3.TabIndex = 31;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(985, -1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdMain;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar1.TabIndex = 10;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 475);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1034, 216);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 32;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.RowCheckStateChanged += new RowCheckStateChangeEventHandler(grdDetail_RowCheckStateChanged);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 452);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1034, 23);
		this.panel4.TabIndex = 33;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1034, 691);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		base.Controls.Add(this.panel4);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grdMain);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "LoadSaleOrderFeed";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(LoadInvoices_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(LoadPurchaseOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdMain).EndInit();
		this.panel2.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		base.ResumeLayout(false);
	}
}
