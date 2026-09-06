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
namespace Architecture.WinApp.SaleTrading
{

public class SaleOrderLoadForDo : Form
{
	private int[] AryColWidth;

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	public DataTable dtSaleOrder = new DataTable();

	public string orderIds = "";

	public string orderMainIds = "";

	public string HeaderorderIds = "";

	public bool MainDetail;

	private DataTable lst = new DataTable();

	private DataTable lstMainView = new DataTable();

	private IContainer components;

	private Panel panel1;

	private Label label23;

	private Button btngrnlod;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnLoadOnInvoice;

	private DateTimePicker Todate;

	private DateTimePicker FromDate;

	private Label label2;

	private Label label3;

	internal UltraCombo combsuppname;

	private Label label1;

	internal UltraCombo combitem;

	private Label label37;

	internal GridEX grdDetail;

	internal GridEX grd;

	private Panel panel5;

	private Panel panel7;

	private Panel panel9;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel11;

	private Panel panel6;

	private Panel panel10;

	private TabPage tabPage2;

	internal GridEX GrdSecondView;

	private Panel panel2;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel8;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label4;

	private Label label5;

	private Label label6;

	public SaleOrderLoadForDo(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmLoadGRN_Load_1(object sender, EventArgs e)
	{
		try
		{
			FromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			ComboFill();
			tabControl1.TabPages.Remove(tabPage1);
			tabControl1.TabPages.Remove(tabPage2);
			if (!MainDetail)
			{
				tabControl1.TabPages.Remove(tabPage1);
				tabControl1.TabPages.Add(tabPage2);
				GridSecondViewFill();
				CheckedAllMainRowsSecondView();
			}
			else
			{
				tabControl1.TabPages.Remove(tabPage2);
				tabControl1.TabPages.Add(tabPage1);
				PendingSaleOrderLoad();
				CheckedAllMainRows();
				GetCheckedRowsId();
				GridDetailBind(HeaderorderIds, orderIds);
				CheckedAllDetailRows();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void PendingSaleOrderLoad()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		try
		{
			lst.Rows.Clear();
			ReportsParameters Obj = new ReportsParameters();
			Obj.OrganizationId = UserAccount.OrganizationId;
			Obj.CompanyId = UserAccount.CompanyId;
			Obj.BranchesId = UserAccount.BranchesId;
			Obj.FromDate = Conversion.ToDateTime((object)FromDate.Value);
			Obj.ToDate = Conversion.ToDateTime((object)Todate.Value);
			Obj.DocumentTypeIds = "1605";
			Obj.SupplierCustomerId = Conversion.ToInt(combsuppname.Value);
			Obj.ItemId = Conversion.ToInt(combitem.Value);
			Obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			Obj.IsApproved = true;
			Obj.ZeroBalanceType = 1;
			lst = SaleOrder.SaleOrderLoadForDO_Engr(Obj);
			if (lst.Rows.Count > 0)
			{
				DataTable dtMain = new DataTable();
				dtMain.Columns.Add("Id", typeof(int));
				dtMain.Columns.Add("OrderDetailId", typeof(int));
				dtMain.Columns.Add("DocNo", typeof(int));
				dtMain.Columns.Add("DocDate", typeof(DateTime));
				dtMain.Columns.Add("CustomerId", typeof(int));
				dtMain.Columns.Add("CustomerName");
				dtMain.Columns.Add("SupplierRefNo");
				dtMain.Columns.Add("PaymentTerm");
				dtMain.Columns.Add("DueDays", typeof(int));
				dtMain.Columns.Add("DeliveryTerm");
				int i;
				for (i = 0; i < lst.Rows.Count; i++)
				{
					if (!dtMain.AsEnumerable().Any((DataRow dr) => (int)dr["Id"] == Conversion.ToInt(lst.Rows[i]["Id"])))
					{
						dtMain.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["PoDId"], lst.Rows[i]["DocNo"], Conversion.ToDateTime(lst.Rows[i]["DocDate"]).ToShortDateString(), lst.Rows[i]["OrderSupCustId"], lst.Rows[i]["SupplierName"], lst.Rows[i]["SupplierRefNo"], lst.Rows[i]["TermsDescription"], lst.Rows[i]["OrderDueDays"], lst.Rows[i]["DeliveryTerm"]);
					}
				}
				grd.DataSource = dtMain;
				grd.RetrieveStructure();
				grdSettingsForGrnPending();
			}
			else
			{
				grd.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsForGrnPending()
	{
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["OrderDetailId"].Visible = false;
			grd.RootTable.Columns["CustomerId"].Visible = false;
			grd.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			CommonServices.GridAutoAdjustment(AryColWidth, grd);
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void GridDetailBind(string Ids, string OrderDetailIds)
	{
		try
		{
			if (Ids != string.Empty)
			{
				if (lst.Rows.Count > 0)
				{
					DataTable dtdetail = new DataTable();
					dtdetail.Columns.Add("Id", typeof(int));
					dtdetail.Columns.Add("OrderDetailId", typeof(int));
					dtdetail.Columns.Add("DocDate", typeof(DateTime));
					dtdetail.Columns.Add("DocNo", typeof(int));
					dtdetail.Columns.Add("SupplierCustomerId", typeof(int));
					dtdetail.Columns.Add("CustomerName", typeof(string));
					dtdetail.Columns.Add("SupplierRefNo", typeof(string));
					dtdetail.Columns.Add("PaymentTerm", typeof(string));
					dtdetail.Columns.Add("DueDays", typeof(int));
					dtdetail.Columns.Add("DeliveryTerm", typeof(string));
					dtdetail.Columns.Add("ItemId", typeof(int));
					dtdetail.Columns.Add("ItemCode");
					dtdetail.Columns.Add("ItemName");
					dtdetail.Columns.Add("JobLotId", typeof(int));
					dtdetail.Columns.Add("JobLot", typeof(string));
					dtdetail.Columns.Add("PackUomId", typeof(int));
					dtdetail.Columns.Add("PackUom");
					dtdetail.Columns.Add("PackEquivalent", typeof(double));
					dtdetail.Columns.Add("ItemQty", typeof(double));
					dtdetail.Columns.Add("DispatchQty", typeof(double));
					dtdetail.Columns.Add("BalQty", typeof(double));
					dtdetail.Columns.Add("Weight", typeof(double));
					dtdetail.Columns.Add("DispatchWeight", typeof(double));
					dtdetail.Columns.Add("BalWeight", typeof(double));
					dtdetail.Columns.Add("ItemRate", typeof(double));
					dtdetail.Columns.Add("RateUomId", typeof(int));
					dtdetail.Columns.Add("RateUom");
					dtdetail.Columns.Add("RateEquivalent", typeof(double));
					char[] separators = new char[1] { ',' };
					int[] idArray = (from idStr in Ids.Split(separators, StringSplitOptions.RemoveEmptyEntries)
						select int.Parse(idStr)).ToArray();
					DataTable resultRows = (from row in lst.AsEnumerable()
						where idArray.Contains(row.Field<int>("Id"))
						select row).CopyToDataTable();
					for (int i = 0; i < resultRows.Rows.Count; i++)
					{
						dtdetail.Rows.Add(resultRows.Rows[i]["Id"], resultRows.Rows[i]["PoDId"], Conversion.ToDateTime(resultRows.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), resultRows.Rows[i]["DocNo"], resultRows.Rows[i]["OrderSupCustId"], resultRows.Rows[i]["SupplierName"], resultRows.Rows[i]["SupplierRefNo"], resultRows.Rows[i]["TermsDescription"], resultRows.Rows[i]["OrderDueDays"], resultRows.Rows[i]["DeliveryTerm"], resultRows.Rows[i]["OrderItemId"], resultRows.Rows[i]["ItemCode"], resultRows.Rows[i]["ItemName"], resultRows.Rows[i]["JobLotId"], resultRows.Rows[i]["JobLotCode"], resultRows.Rows[i]["PackUomId"], resultRows.Rows[i]["PackUom"], resultRows.Rows[i]["PackEquivalent"], resultRows.Rows[i]["OrderItemQty"], resultRows.Rows[i]["DispatchQty"], resultRows.Rows[i]["BalQty"], resultRows.Rows[i]["NetWeight"], resultRows.Rows[i]["DispatchWeight"], resultRows.Rows[i]["BalWeight"], resultRows.Rows[i]["OrderItemRate"], resultRows.Rows[i]["RateUomId"], resultRows.Rows[i]["RateUom"], resultRows.Rows[i]["EquivalentRate"]);
					}
					grdDetail.DataSource = dtdetail;
					grdDetail.RetrieveStructure();
					DataGridHistoryDetailSetting();
				}
				else
				{
					grdDetail.ClearStructure();
				}
			}
			else
			{
				grdDetail.ClearStructure();
			}
			CheckedAllDetailRows();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistoryDetailSetting()
	{
		try
		{
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["OrderDetailId"].Visible = false;
			grdDetail.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["JobLotId"].Visible = false;
			grdDetail.RootTable.Columns["PackUomId"].Visible = false;
			grdDetail.RootTable.Columns["PackEquivalent"].Visible = false;
			grdDetail.RootTable.Columns["RateUomId"].Visible = false;
			grdDetail.RootTable.Columns["RateEquivalent"].Visible = false;
			grdDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["DispatchQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["DispatchQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["DispatchQty"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["DispatchQty"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["BalQty"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["BalQty"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Weight"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
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
			CommonServices.GridAutoAdjustment(AryColWidth, grdDetail);
			grdDetail.RootTable.Columns.Add("Select");
			grdDetail.RootTable.Columns["Select"].ActAsSelector = true;
			grdDetail.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdDetail.RootTable.Columns["Select"].Position = 0;
			grdDetail.RootTable.Columns["Select"].Width = 30;
			grdDetail.FrozenColumns = 1;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_SelectionChanged(object sender, EventArgs e)
	{
	}

	private void GetCheckedRowsId()
	{
		try
		{
			HeaderorderIds = "";
			if (grd.GetCheckedRows().Count() > 0)
			{
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				for (int i = 0; i < checkedRows.Length; i++)
				{
					int orderId = Conversion.ToInt(checkedRows[i].Cells["Id"].Value);
					HeaderorderIds = HeaderorderIds + "," + orderId;
				}
			}
			else
			{
				HeaderorderIds = "";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Select")
			{
				GetCheckedRowsId();
				GridDetailBind(HeaderorderIds, orderIds);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_Click(object sender, EventArgs e)
	{
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grd.UpdateData();
			if (e.Column.Key == "Select")
			{
				GetCheckedRowsId();
				GridDetailBind(HeaderorderIds, orderIds);
			}
			CheckedAllDetailRows();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellValueChanged(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Select")
			{
				GetCheckedRowsId();
				GridDetailBind(HeaderorderIds, orderIds);
			}
			CheckedAllDetailRows();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CheckedAllDetailRows()
	{
		try
		{
			if (grd.GetCheckedRows().Count() <= 0)
			{
				return;
			}
			if (orderIds != "")
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow row in rows)
				{
					row.BeginEdit();
					if (orderIds.Contains(row.Cells["OrderDetailId"].Value.ToString()))
					{
						row.IsChecked = Convert.ToBoolean(value: true);
					}
					row.EndEdit();
				}
				GrdSecondView.UpdateData();
			}
			else
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSecondViewFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters Obj = new ReportsParameters();
			Obj.OrganizationId = UserAccount.OrganizationId;
			Obj.CompanyId = UserAccount.CompanyId;
			Obj.BranchesId = UserAccount.BranchesId;
			Obj.FromDate = Conversion.ToDateTime((object)FromDate.Value);
			Obj.ToDate = Conversion.ToDateTime((object)Todate.Value);
			Obj.DocumentTypeIds = "1605";
			Obj.SupplierCustomerId = Conversion.ToInt(combsuppname.Value);
			Obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			Obj.ZeroBalanceType = 1;
			Obj.IsApproved = true;
			Obj.ItemId = Conversion.ToInt(combitem.Value);
			lstMainView = SaleOrder.SaleOrderLoadForDO_Engr(Obj);
			if (lstMainView.Rows.Count > 0)
			{
				DataTable dtSecondview = new DataTable();
				dtSecondview.Columns.Add("Id", typeof(int));
				dtSecondview.Columns.Add("OrderDetailId", typeof(int));
				dtSecondview.Columns.Add("DocDate", typeof(DateTime));
				dtSecondview.Columns.Add("DocNo", typeof(int));
				dtSecondview.Columns.Add("SupplierCustomerId", typeof(int));
				dtSecondview.Columns.Add("CustomerName", typeof(string));
				dtSecondview.Columns.Add("SupplierRefNo", typeof(string));
				dtSecondview.Columns.Add("PaymentTerm", typeof(string));
				dtSecondview.Columns.Add("DueDays", typeof(int));
				dtSecondview.Columns.Add("DeliveryTerm", typeof(string));
				dtSecondview.Columns.Add("ItemId", typeof(int));
				dtSecondview.Columns.Add("ItemCode");
				dtSecondview.Columns.Add("ItemName");
				dtSecondview.Columns.Add("JobLotId", typeof(int));
				dtSecondview.Columns.Add("JobLot", typeof(string));
				dtSecondview.Columns.Add("PackUomId", typeof(int));
				dtSecondview.Columns.Add("PackUom");
				dtSecondview.Columns.Add("PackEquivalent", typeof(double));
				dtSecondview.Columns.Add("ItemQty", typeof(double));
				dtSecondview.Columns.Add("DispatchQty", typeof(double));
				dtSecondview.Columns.Add("BalQty", typeof(double));
				dtSecondview.Columns.Add("Weight", typeof(double));
				dtSecondview.Columns.Add("DispatchWeight", typeof(double));
				dtSecondview.Columns.Add("BalWeight", typeof(double));
				dtSecondview.Columns.Add("ItemRate", typeof(double));
				dtSecondview.Columns.Add("RateUomId", typeof(int));
				dtSecondview.Columns.Add("RateUom");
				dtSecondview.Columns.Add("RateEquivalent", typeof(double));
				for (int i = 0; i < lstMainView.Rows.Count; i++)
				{
					dtSecondview.Rows.Add(lstMainView.Rows[i]["Id"], lstMainView.Rows[i]["PoDId"], Conversion.ToDateTime(lstMainView.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), lstMainView.Rows[i]["DocNo"], lstMainView.Rows[i]["OrderSupCustId"], lstMainView.Rows[i]["SupplierName"], lstMainView.Rows[i]["SupplierRefNo"], lstMainView.Rows[i]["TermsDescription"], lstMainView.Rows[i]["OrderDueDays"], lstMainView.Rows[i]["DeliveryTerm"], lstMainView.Rows[i]["OrderItemId"], lstMainView.Rows[i]["ItemCode"], lstMainView.Rows[i]["ItemName"], lstMainView.Rows[i]["JobLotId"], lstMainView.Rows[i]["JobLotCode"], lstMainView.Rows[i]["PackUomId"], lstMainView.Rows[i]["PackUom"], lstMainView.Rows[i]["PackEquivalent"], lstMainView.Rows[i]["OrderItemQty"], lstMainView.Rows[i]["DispatchQty"], lstMainView.Rows[i]["BalQty"], lstMainView.Rows[i]["NetWeight"], lstMainView.Rows[i]["DispatchWeight"], lstMainView.Rows[i]["BalWeight"], lstMainView.Rows[i]["OrderItemRate"], lstMainView.Rows[i]["RateUomId"], lstMainView.Rows[i]["RateUom"], lstMainView.Rows[i]["EquivalentRate"]);
				}
				GrdSecondView.DataSource = dtSecondview;
				GrdSecondView.RetrieveStructure();
				GridSecondViewDetailSetting();
			}
			else
			{
				GrdSecondView.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSecondViewDetailSetting()
	{
		try
		{
			GrdSecondView.RootTable.Columns["Id"].Visible = false;
			GrdSecondView.RootTable.Columns["OrderDetailId"].Visible = false;
			GrdSecondView.RootTable.Columns["SupplierCustomerId"].Visible = false;
			GrdSecondView.RootTable.Columns["ItemId"].Visible = false;
			GrdSecondView.RootTable.Columns["PackUomId"].Visible = false;
			GrdSecondView.RootTable.Columns["RateUomId"].Visible = false;
			GrdSecondView.RootTable.Columns["PackEquivalent"].Visible = false;
			GrdSecondView.RootTable.Columns["RateEquivalent"].Visible = false;
			GrdSecondView.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			GrdSecondView.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			GrdSecondView.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			GrdSecondView.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			GrdSecondView.RootTable.Columns["DispatchQty"].AggregateFunction = (AggregateFunction)2;
			GrdSecondView.RootTable.Columns["DispatchQty"].TextAlignment = (TextAlignment)3;
			GrdSecondView.RootTable.Columns["DispatchQty"].FormatString = "#,##0.###";
			GrdSecondView.RootTable.Columns["DispatchQty"].TotalFormatString = "#,##0.###";
			GrdSecondView.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			GrdSecondView.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			GrdSecondView.RootTable.Columns["BalQty"].FormatString = "#,##0.###";
			GrdSecondView.RootTable.Columns["BalQty"].TotalFormatString = "#,##0.###";
			GrdSecondView.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			GrdSecondView.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			GrdSecondView.RootTable.Columns["Weight"].FormatString = "#,##0.###";
			GrdSecondView.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
			GrdSecondView.RootTable.Columns["DispatchWeight"].AggregateFunction = (AggregateFunction)2;
			GrdSecondView.RootTable.Columns["DispatchWeight"].TextAlignment = (TextAlignment)3;
			GrdSecondView.RootTable.Columns["DispatchWeight"].FormatString = "#,##0.###";
			GrdSecondView.RootTable.Columns["DispatchWeight"].TotalFormatString = "#,##0.###";
			GrdSecondView.RootTable.Columns["BalWeight"].AggregateFunction = (AggregateFunction)2;
			GrdSecondView.RootTable.Columns["BalWeight"].TextAlignment = (TextAlignment)3;
			GrdSecondView.RootTable.Columns["BalWeight"].FormatString = "#,##0.###";
			GrdSecondView.RootTable.Columns["BalWeight"].TotalFormatString = "#,##0.###";
			GrdSecondView.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			GrdSecondView.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			CommonServices.GridAutoAdjustment(AryColWidth, GrdSecondView);
			GrdSecondView.RootTable.Columns.Add("Select");
			GrdSecondView.RootTable.Columns["Select"].ActAsSelector = true;
			GrdSecondView.RootTable.Columns["Select"].UseHeaderSelector = true;
			GrdSecondView.RootTable.Columns["Select"].Width = 30;
			GrdSecondView.RootTable.Columns["Select"].Position = 0;
			GrdSecondView.FrozenColumns = 1;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		try
		{
			DataTable dt = SaleOrder.GetDataForDropDownFromSaleOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				AppId = UserAccount.AppId,
				UserId = UserAccount.ID
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id");
			dtCustomer.Columns.Add("Name");
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Name");
			dtItem.Columns.Add("Id");
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Customer")
				{
					dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Item")
				{
					dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
			}
			if (dtCustomer.Rows.Count > 0)
			{
				DDL.BindDDL(dtCustomer, combsuppname, "Id", "Name", "Customer Name", ZeroIndex: false);
				((UltraGridBase)combsuppname).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			else
			{
				((Control)(object)combsuppname).Text = string.Empty;
				((UltraGridBase)combsuppname).DataSource = null;
			}
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtItem, combitem, "Id", "Name", "Item Name", ZeroIndex: false);
				((UltraGridBase)combitem).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			else
			{
				((Control)(object)combitem).Text = string.Empty;
				((UltraGridBase)combitem).DataSource = null;
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void btngrnlod_Click(object sender, EventArgs e)
	{
		try
		{
			PendingSaleOrderLoad();
			GridSecondViewFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnLoadOnInvoice_Click_1(object sender, EventArgs e)
	{
		try
		{
			if (MainDetail)
			{
				int getCount = grdDetail.GetCheckedRows().Count();
				dtSaleOrder = lst.Clone();
				if (int.Parse(getCount.ToString()) > 0)
				{
					int saleOrderId = 0;
					GridEXRow[] checkedRows = grdDetail.GetCheckedRows();
					foreach (GridEXRow r2 in checkedRows)
					{
						int SaleOrder = Conversion.ToInt(r2.Cells["Id"].Value);
						if (saleOrderId == 0)
						{
							saleOrderId = SaleOrder;
						}
						if (saleOrderId != SaleOrder)
						{
							dtSaleOrder.Rows.Clear();
							MessageBox.Show("Sorry!. The Selected Row's are not of same Order");
							return;
						}
						DataRow dr = lst.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("PoDId") == Conversion.ToInt(r2.Cells["OrderDetailId"].Value));
						dtSaleOrder.Rows.Add(dr.ItemArray);
						saleOrderId = Conversion.ToInt(r2.Cells["Id"].Value);
					}
					Hide();
				}
				else
				{
					MessageBox.Show("No Row is Selected");
				}
				return;
			}
			int getCount2 = GrdSecondView.GetCheckedRows().Count();
			dtSaleOrder = lstMainView.Clone();
			int saleOrderId2 = 0;
			if (int.Parse(getCount2.ToString()) > 0)
			{
				GridEXRow[] checkedRows = GrdSecondView.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					int SaleOrder2 = Conversion.ToInt(r.Cells["Id"].Value);
					if (saleOrderId2 == 0)
					{
						saleOrderId2 = SaleOrder2;
					}
					if (saleOrderId2 != SaleOrder2)
					{
						dtSaleOrder.Rows.Clear();
						MessageBox.Show("Sorry!. The Selected Rows are not of same Order");
						return;
					}
					DataRow dr2 = lstMainView.Rows[r.RowIndex];
					dtSaleOrder.Rows.Add(dr2.ItemArray);
					saleOrderId2 = Conversion.ToInt(r.Cells["Id"].Value);
				}
				Hide();
			}
			else
			{
				MessageBox.Show("No Row is Selected");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CheckedAllMainRows()
	{
		try
		{
			if (!(orderMainIds != ""))
			{
				return;
			}
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow row in rows)
			{
				row.BeginEdit();
				if (orderMainIds.Contains(row.Cells["Id"].Value.ToString()))
				{
					row.IsChecked = Convert.ToBoolean(value: true);
				}
				row.EndEdit();
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CheckedAllMainRowsSecondView()
	{
		try
		{
			if (!(orderMainIds != ""))
			{
				return;
			}
			GridEXRow[] rows = GrdSecondView.GetRows();
			foreach (GridEXRow row in rows)
			{
				row.BeginEdit();
				if (orderMainIds.Contains(row.Cells["Id"].Value.ToString()))
				{
					row.IsChecked = Convert.ToBoolean(value: true);
				}
				row.EndEdit();
			}
			GrdSecondView.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Hide();
	}

	private void frmLoadSaleOrder_FormClosing(object sender, FormClosingEventArgs e)
	{
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
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

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GrdSecondView).Name);
			if (fs != null)
			{
				GrdSecondView.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
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
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Expected O, but got Unknown
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Expected O, but got Unknown
		//IL_1186: Unknown result type (might be due to invalid IL or missing references)
		//IL_1190: Expected O, but got Unknown
		//IL_119d: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a7: Expected O, but got Unknown
		//IL_11b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_11be: Expected O, but got Unknown
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdSecondView_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.combitem = new UltraCombo();
		this.label37 = new System.Windows.Forms.Label();
		this.combsuppname = new UltraCombo();
		this.Todate = new System.Windows.Forms.DateTimePicker();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.btnLoadOnInvoice = new System.Windows.Forms.Button();
		this.btngrnlod = new System.Windows.Forms.Button();
		this.grd = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.grdDetail = new GridEX();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel11 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.GrdSecondView = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.combitem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combsuppname).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel11.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel10.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdSecondView).BeginInit();
		this.panel2.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel8.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.label23);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1057, 30);
		this.panel1.TabIndex = 0;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 6);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(139, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "Sale Order Load";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(241, 9);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(90, 15);
		this.label1.TabIndex = 20;
		this.label1.Text = "CustomerName";
		this.combitem.AllowNull = (DefaultableBoolean)1;
		this.combitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.combitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.combitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance159;
		((SpecialBoxBase)((UltraGridBase)this.combitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance160).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.combitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.combitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance164).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance165).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).TextHAlignAsString = "Left";
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.combitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.combitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combitem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combitem).Location = new System.Drawing.Point(336, 33);
		((System.Windows.Forms.Control)(object)this.combitem).Name = "combitem";
		((System.Windows.Forms.Control)(object)this.combitem).Size = new System.Drawing.Size(286, 26);
		((System.Windows.Forms.Control)(object)this.combitem).TabIndex = 3;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(241, 39);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(67, 15);
		this.label37.TabIndex = 19;
		this.label37.Text = "Item Name";
		this.combsuppname.AllowNull = (DefaultableBoolean)1;
		this.combsuppname.AutoCompleteMode = (AutoCompleteMode)4;
		this.combsuppname.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combsuppname.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combsuppname).DisplayLayout.Appearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.combsuppname).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combsuppname).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combsuppname).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsuppname).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance171;
		((SpecialBoxBase)((UltraGridBase)this.combsuppname).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance172).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance172).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance172).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsuppname).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.combsuppname).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combsuppname).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance176).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance177).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance177).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance177).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance178).TextHAlignAsString = "Left";
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.combsuppname).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combsuppname).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combsuppname).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combsuppname).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combsuppname.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combsuppname).Location = new System.Drawing.Point(336, 3);
		((System.Windows.Forms.Control)(object)this.combsuppname).Name = "combsuppname";
		((System.Windows.Forms.Control)(object)this.combsuppname).Size = new System.Drawing.Size(286, 26);
		((System.Windows.Forms.Control)(object)this.combsuppname).TabIndex = 2;
		this.Todate.CustomFormat = "dd-MMM-yy";
		this.Todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.Todate.Location = new System.Drawing.Point(75, 35);
		this.Todate.Name = "Todate";
		this.Todate.Size = new System.Drawing.Size(155, 23);
		this.Todate.TabIndex = 1;
		this.FromDate.CustomFormat = "dd-MMM-yy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(75, 5);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(155, 23);
		this.FromDate.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(4, 39);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 15);
		this.label2.TabIndex = 15;
		this.label2.Text = "To Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(4, 9);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(63, 15);
		this.label3.TabIndex = 16;
		this.label3.Text = "From Date";
		this.btnLoadOnInvoice.BackColor = System.Drawing.Color.Teal;
		this.btnLoadOnInvoice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnLoadOnInvoice.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadOnInvoice.ForeColor = System.Drawing.Color.White;
		this.btnLoadOnInvoice.Location = new System.Drawing.Point(696, 33);
		this.btnLoadOnInvoice.Name = "btnLoadOnInvoice";
		this.btnLoadOnInvoice.Size = new System.Drawing.Size(71, 26);
		this.btnLoadOnInvoice.TabIndex = 5;
		this.btnLoadOnInvoice.Text = "Load";
		this.btnLoadOnInvoice.UseVisualStyleBackColor = false;
		this.btnLoadOnInvoice.Click += new System.EventHandler(btnLoadOnInvoice_Click_1);
		this.btngrnlod.BackColor = System.Drawing.Color.Teal;
		this.btngrnlod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btngrnlod.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btngrnlod.ForeColor = System.Drawing.Color.White;
		this.btngrnlod.Location = new System.Drawing.Point(625, 33);
		this.btngrnlod.Name = "btngrnlod";
		this.btngrnlod.Size = new System.Drawing.Size(71, 26);
		this.btngrnlod.TabIndex = 4;
		this.btngrnlod.Text = "Search";
		this.btngrnlod.UseVisualStyleBackColor = false;
		this.btngrnlod.Click += new System.EventHandler(btngrnlod_Click);
		this.grd.AllowDelete = (InheritableBoolean)1;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 30);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1043, 277);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 4;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellValueChanged += new ColumnActionEventHandler(grd_CellValueChanged);
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		this.grd.SelectionChanged += new System.EventHandler(grd_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grd).Click += new System.EventHandler(grd_Click);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label4);
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1043, 30);
		this.panel5.TabIndex = 3;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(5, 5);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(201, 19);
		this.label4.TabIndex = 25;
		this.label4.Text = "Order Main Information";
		this.grdDetail.AllowDelete = (InheritableBoolean)1;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 29);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1043, 254);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 6;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1057, 622);
		this.tabControl1.TabIndex = 33;
		this.tabPage1.Controls.Add(this.panel11);
		this.tabPage1.Controls.Add(this.panel10);
		this.tabPage1.Location = new System.Drawing.Point(4, 22);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1049, 596);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Main Detail";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel11.Controls.Add(this.panel6);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(3, 310);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1043, 283);
		this.panel11.TabIndex = 1;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label5);
		this.panel6.Controls.Add(this.ctrlGrdBar3);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1043, 29);
		this.panel6.TabIndex = 6;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(3, 5);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(160, 19);
		this.label5.TabIndex = 25;
		this.label5.Text = "Detail Information";
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel10.Controls.Add(this.panel5);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 3);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1043, 307);
		this.panel10.TabIndex = 0;
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GrdSecondView);
		this.tabPage2.Controls.Add(this.panel2);
		this.tabPage2.Location = new System.Drawing.Point(4, 22);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1074, 632);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Main";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.GrdSecondView.AllowDelete = (InheritableBoolean)1;
		this.GrdSecondView.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GrdSecondView).BackColor = System.Drawing.Color.White;
		GrdSecondView_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdSecondView.DesignTimeLayout = GrdSecondView_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdSecondView).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdSecondView.FilterMode = (FilterMode)2;
		this.GrdSecondView.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdSecondView).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdSecondView.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GrdSecondView).Location = new System.Drawing.Point(3, 33);
		((System.Windows.Forms.Control)(object)this.GrdSecondView).Name = "GrdSecondView";
		((ScrollableBase)this.GrdSecondView).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdSecondView).Size = new System.Drawing.Size(1068, 596);
		((System.Windows.Forms.Control)(object)this.GrdSecondView).TabIndex = 6;
		this.GrdSecondView.TotalRow = (InheritableBoolean)1;
		this.GrdSecondView.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdSecondView).VisualStyle = (VisualStyle)4;
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label6);
		this.panel2.Controls.Add(this.ctrlGrdBar2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1068, 30);
		this.panel2.TabIndex = 5;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(5, 5);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(157, 19);
		this.label6.TabIndex = 25;
		this.label6.Text = "Order Information";
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Controls.Add(this.panel1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1057, 719);
		this.panel7.TabIndex = 34;
		this.panel9.Controls.Add(this.tabControl1);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 97);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1057, 622);
		this.panel9.TabIndex = 32;
		this.panel8.BackColor = System.Drawing.Color.White;
		this.panel8.Controls.Add(this.label1);
		this.panel8.Controls.Add(this.label3);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.combitem);
		this.panel8.Controls.Add(this.btngrnlod);
		this.panel8.Controls.Add(this.label37);
		this.panel8.Controls.Add(this.btnLoadOnInvoice);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.combsuppname);
		this.panel8.Controls.Add(this.label2);
		this.panel8.Controls.Add(this.Todate);
		this.panel8.Controls.Add(this.FromDate);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 30);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1057, 67);
		this.panel8.TabIndex = 31;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1002, 4);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdDetail;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 11;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1002, 4);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 10;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1028, 4);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.GrdSecondView;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 10;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1057, 719);
		base.Controls.Add(this.panel7);
		base.HelpButton = true;
		base.Name = "SaleOrderLoadForDo";
		this.Text = "SaleOrderLoad";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmLoadSaleOrder_FormClosing);
		base.Load += new System.EventHandler(frmLoadGRN_Load_1);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.combitem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combsuppname).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdSecondView).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
