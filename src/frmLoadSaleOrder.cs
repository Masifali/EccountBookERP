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
using Architecture.WinApp.Purchase;
using Infragistics.Shared;
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
namespace Architecture.WinApp.Sale
{

public class frmLoadSaleOrder : Form
{
	private InvfrmPurchaseInvoice PI;

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	public DataTable dtGDNS = new DataTable();

	public DataTable dtdetail = new DataTable();

	public DataTable dtSecondview = new DataTable();

	public DataTable dtBranch = new DataTable();

	public DataTable dtItem = new DataTable();

	public string orderIds = "";

	public string orderMainIds = "";

	public string HeaderorderIds = "";

	public bool BranchImplemented;

	public bool MainDetail;

	public bool SubsidiaryAccountAllownOnVouchers;

	private DataTable dtMain = new DataTable();

	public int documentTypeId;

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

	internal UltraCombo cmbBranchName;

	private Label label4;

	public frmLoadSaleOrder(UserAccount user)
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
			BranchImplemented = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("SaleOrderBranchWise"));
			dtBranch.Columns.Add("Id", typeof(int));
			dtBranch.Columns.Add("BranchName", typeof(string));
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("ItemName", typeof(string));
			dtMain.Columns.Add("OrderId");
			dtMain.Columns.Add("DocumentType");
			dtMain.Columns.Add("OrderSupCustId");
			dtMain.Columns.Add("BranchName");
			dtMain.Columns.Add("PartyName");
			dtMain.Columns.Add("DocDate", typeof(DateTime));
			dtMain.Columns.Add("OrderExpiryDate", typeof(DateTime));
			dtMain.Columns.Add("DocNo", typeof(int));
			dtMain.Columns.Add("OrderQty", typeof(double));
			dtMain.Columns.Add("OrderWeight", typeof(double));
			dtMain.Columns.Add("DispatchedWeight", typeof(double));
			dtMain.Columns.Add("BalWeight", typeof(double));
			dtMain.Columns.Add("Remarks");
			grd.DataSource = dtMain;
			grd.RetrieveStructure();
			grdSettingsForGrnPending();
			dtdetail.Columns.Add("OrderType");
			dtdetail.Columns.Add("OrderId");
			dtdetail.Columns.Add("OrderNo", typeof(int));
			dtdetail.Columns.Add("OrderDetailId");
			dtdetail.Columns.Add("OrderSupCustId");
			dtdetail.Columns.Add("ItemId");
			dtdetail.Columns.Add("ItemName");
			dtdetail.Columns.Add("ItemUomId");
			dtdetail.Columns.Add("PackUOM");
			dtdetail.Columns.Add("OrderQTY", typeof(double));
			dtdetail.Columns.Add("DispatchQty", typeof(double));
			dtdetail.Columns.Add("BalQty", typeof(double));
			dtdetail.Columns.Add("OrderWeight", typeof(double));
			dtdetail.Columns.Add("DispatchedWeight", typeof(double));
			dtdetail.Columns.Add("BalWeight", typeof(double));
			dtdetail.Columns.Add("ItemRate", typeof(double));
			grdDetail.DataSource = dtdetail;
			grdDetail.RetrieveStructure();
			DataGridHistoryDetailSetting();
			dtSecondview.Columns.Add("OrderType");
			dtSecondview.Columns.Add("OrderId");
			dtSecondview.Columns.Add("OrderNo", typeof(int));
			dtSecondview.Columns.Add("DocDate", typeof(DateTime));
			dtSecondview.Columns.Add("OrderExpiryDate", typeof(DateTime));
			dtSecondview.Columns.Add("BranchName");
			dtSecondview.Columns.Add("OrderDetailId");
			dtSecondview.Columns.Add("OrderSupCustId");
			dtSecondview.Columns.Add("PartyName");
			dtSecondview.Columns.Add("ItemId");
			dtSecondview.Columns.Add("ItemName");
			dtSecondview.Columns.Add("ItemUomId");
			dtSecondview.Columns.Add("PackUOM");
			dtSecondview.Columns.Add("OrderQTY", typeof(double));
			dtSecondview.Columns.Add("DispatchQty", typeof(double));
			dtSecondview.Columns.Add("BalQty", typeof(double));
			dtSecondview.Columns.Add("OrderWeight", typeof(double));
			dtSecondview.Columns.Add("DispatchedWeight", typeof(double));
			dtSecondview.Columns.Add("BalWeight", typeof(double));
			dtSecondview.Columns.Add("ItemRate", typeof(double));
			GrdSecondView.DataSource = dtSecondview;
			GrdSecondView.RetrieveStructure();
			GridSecondViewDetailSetting();
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			dtGDNS.Columns.Add("Id");
			FromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			BranchFill();
			HistoryCombosFill();
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
			MessageBox.Show(ex.Message);
		}
	}

	private void PendingSaleOrderLoad()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters gdn = new ReportsParameters();
			gdn.OrganizationId = UserAccount.OrganizationId;
			gdn.CompanyId = UserAccount.CompanyId;
			gdn.SupplierCustomerId = Conversion.ToInt(combsuppname.Value);
			gdn.ItemId = Conversion.ToInt(combitem.Value);
			gdn.FromDate = Conversion.ToDateTime((object)FromDate.Value);
			gdn.ToDate = Conversion.ToDateTime((object)Todate.Value);
			gdn.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				gdn.BranchesIds = BranchIds;
				DataTable lst = SaleOrder.LoadSaleOrderForDeliveryOrder(gdn);
				if (lst.Rows.Count > 0)
				{
					dtMain.Clear();
					for (int i = 0; i < lst.Rows.Count; i++)
					{
						dtMain.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["DocumentType"], lst.Rows[i]["OrderSupCustId"], lst.Rows[i]["BranchName"], lst.Rows[i]["PartyName"], Conversion.ToDateTime(lst.Rows[i]["DocDate"]).ToShortDateString(), lst.Rows[i]["OrderExpiryDate"], lst.Rows[i]["DocNo"], Conversion.ToDouble(lst.Rows[i]["OrderQty"]), Conversion.ToDouble(lst.Rows[i]["OrderWeight"]), Conversion.ToDouble(lst.Rows[i]["DispatchWeight"]), Conversion.ToDouble(lst.Rows[i]["BalWeight"]), lst.Rows[i]["RemarksHeader"]);
					}
					grd.DataSource = dtMain;
					grd.RetrieveStructure();
					grdSettingsForGrnPending();
				}
				else
				{
					grd.ClearStructure();
				}
				return;
			}
			cmbBranchName.Focus();
			throw new Exception("Select branch first");
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
			grd.RootTable.Columns["OrderId"].Visible = false;
			grd.RootTable.Columns["OrderSupCustId"].Visible = false;
			if (BranchImplemented)
			{
				grd.RootTable.Columns["BranchName"].Visible = false;
			}
			else
			{
				grd.RootTable.Columns["BranchName"].Width = 250;
			}
			grd.RootTable.Columns["PartyName"].Width = 250;
			grd.RootTable.Columns["DocNo"].Caption = "Order No";
			grd.RootTable.Columns["DocDate"].Caption = "Order Date";
			grd.RootTable.Columns["OrderExpiryDate"].Caption = "Expiry Date";
			grd.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			grd.RootTable.Columns["OrderQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["OrderWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["DispatchedWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BalWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["OrderQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["OrderWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["DispatchedWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BalWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["OrderQty"].FormatString = "#,#";
			grd.RootTable.Columns["OrderWeight"].FormatString = "#,#";
			grd.RootTable.Columns["DispatchedWeight"].FormatString = "#,#";
			grd.RootTable.Columns["BalWeight"].FormatString = "#,#";
			grd.RootTable.Columns["OrderQty"].TotalFormatString = "#,#";
			grd.RootTable.Columns["OrderWeight"].TotalFormatString = "#,#";
			grd.RootTable.Columns["DispatchedWeight"].TotalFormatString = "#,#";
			grd.RootTable.Columns["BalWeight"].TotalFormatString = "#,#";
			grd.RootTable.Columns.Add("Select");
			grd.RootTable.Columns["Select"].ActAsSelector = true;
			grd.RootTable.Columns["Select"].UseHeaderSelector = true;
			grd.RootTable.Columns["Select"].Position = 0;
			grd.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void GridDetailBind(string Id, string OrderDetailIds)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters gdn = new ReportsParameters();
			gdn.OrganizationId = UserAccount.OrganizationId;
			gdn.CompanyId = UserAccount.CompanyId;
			gdn.Ids = Id;
			gdn.OrderDetailIds = OrderDetailIds;
			if (Id != string.Empty)
			{
				DataTable dt = SaleOrder.GetPendingSaleOrderDetailForDeliveryOrder(gdn);
				if (dt.Rows.Count > 0)
				{
					dtdetail.Clear();
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						dtdetail.Rows.Add(dt.Rows[i]["DocumentType"], dt.Rows[i]["Id"], dt.Rows[i]["DocNo"], dt.Rows[i]["OrderDetailId"], dt.Rows[i]["OrderSupCustId"], dt.Rows[i]["ItemId"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemUOMId"], dt.Rows[i]["PackUom"], Conversion.ToDouble(dt.Rows[i]["OrderQTY"]), Conversion.ToDouble(dt.Rows[i]["DispatchQty"]), Conversion.ToDouble(dt.Rows[i]["BalQty"]), Conversion.ToDouble(dt.Rows[i]["OrderWeight"]), Conversion.ToDouble(dt.Rows[i]["DispatchWeight"]), Conversion.ToDouble(dt.Rows[i]["BalWeight"]), Conversion.ToDouble(dt.Rows[i]["ItemRate"]));
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
			grdDetail.RootTable.Columns["OrderId"].Visible = false;
			grdDetail.RootTable.Columns["OrderSupCustId"].Visible = false;
			grdDetail.RootTable.Columns["OrderDetailId"].Visible = false;
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["ItemUomId"].Visible = false;
			grdDetail.RootTable.Columns["ItemName"].Width = 300;
			grdDetail.RootTable.Columns["PackUOM"].Width = 70;
			grdDetail.RootTable.Columns["OrderQTY"].Width = 90;
			grdDetail.RootTable.Columns["DispatchQty"].Width = 90;
			grdDetail.RootTable.Columns["BalQty"].Width = 90;
			grdDetail.RootTable.Columns["OrderWeight"].Width = 90;
			grdDetail.RootTable.Columns["BalWeight"].Width = 90;
			grdDetail.RootTable.Columns["DispatchedWeight"].Width = 120;
			grdDetail.RootTable.Columns["OrderQTY"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["DispatchQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["OrderWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["DispatchedWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["BalWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["OrderQTY"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["DispatchQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["OrderWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["DispatchedWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["BalWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["OrderQTY"].FormatString = "0,0";
			grdDetail.RootTable.Columns["DispatchQty"].FormatString = "0,0";
			grdDetail.RootTable.Columns["BalQty"].FormatString = "0,0";
			grdDetail.RootTable.Columns["OrderWeight"].FormatString = "0,0";
			grdDetail.RootTable.Columns["DispatchedWeight"].FormatString = "0,0";
			grdDetail.RootTable.Columns["BalWeight"].FormatString = "0,0";
			grdDetail.RootTable.Columns["OrderQTY"].TotalFormatString = "0,0";
			grdDetail.RootTable.Columns["DispatchQty"].TotalFormatString = "0,0";
			grdDetail.RootTable.Columns["BalQty"].TotalFormatString = "0,0";
			grdDetail.RootTable.Columns["OrderWeight"].TotalFormatString = "0,0";
			grdDetail.RootTable.Columns["DispatchedWeight"].TotalFormatString = "0,0";
			grdDetail.RootTable.Columns["BalWeight"].TotalFormatString = "0,0";
			grdDetail.RootTable.Columns.Add("Select");
			grdDetail.RootTable.Columns["Select"].ActAsSelector = true;
			grdDetail.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdDetail.RootTable.Columns["Select"].Position = 0;
			grdDetail.FrozenColumns = 1;
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
					int orderId = Conversion.ToInt(checkedRows[i].Cells["OrderId"].Value);
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
					if (orderIds.Contains(row.Cells["OrderDetailId"].Value.ToString()))
					{
						row.BeginEdit();
						row.IsChecked = Convert.ToBoolean(value: true);
						row.EndEdit();
					}
				}
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
			}
			grdDetail.UpdateData();
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
			ReportsParameters gdn = new ReportsParameters();
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				gdn.BranchesIds = BranchIds;
				gdn.OrganizationId = UserAccount.OrganizationId;
				gdn.CompanyId = UserAccount.CompanyId;
				gdn.SupplierCustomerId = Conversion.ToInt(combsuppname.Value);
				gdn.ItemId = Conversion.ToInt(combitem.Value);
				gdn.FromDate = Conversion.ToDateTime((object)FromDate.Value);
				gdn.ToDate = Conversion.ToDateTime((object)Todate.Value);
				DataTable dt = SaleOrder.GetPendingSaleOrderDetailForDeliveryOrder(gdn);
				if (dt.Rows.Count > 0)
				{
					dtSecondview.Clear();
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						dtSecondview.Rows.Add(dt.Rows[i]["DocumentType"], dt.Rows[i]["Id"], dt.Rows[i]["DocNo"], dt.Rows[i]["DocDate"], dt.Rows[i]["OrderExpiryDate"], dt.Rows[i]["BranchName"], dt.Rows[i]["OrderDetailId"], dt.Rows[i]["OrderSupCustId"], dt.Rows[i]["CustomerName"], dt.Rows[i]["ItemId"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemUOMId"], dt.Rows[i]["PackUom"], Conversion.ToDouble(dt.Rows[i]["OrderQTY"]), Conversion.ToDouble(dt.Rows[i]["DispatchQty"]), Conversion.ToDouble(dt.Rows[i]["BalQty"]), Conversion.ToDouble(dt.Rows[i]["OrderWeight"]), Conversion.ToDouble(dt.Rows[i]["DispatchWeight"]), Conversion.ToDouble(dt.Rows[i]["BalWeight"]), Conversion.ToDouble(dt.Rows[i]["ItemRate"]));
					}
					GrdSecondView.DataSource = dtSecondview;
					GrdSecondView.RetrieveStructure();
					GridSecondViewDetailSetting();
				}
				else
				{
					GrdSecondView.ClearStructure();
				}
				return;
			}
			cmbBranchName.Focus();
			throw new Exception("Select branch first");
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
			GrdSecondView.RootTable.Columns["OrderId"].Visible = false;
			GrdSecondView.RootTable.Columns["OrderSupCustId"].Visible = false;
			GrdSecondView.RootTable.Columns["OrderDetailId"].Visible = false;
			GrdSecondView.RootTable.Columns["ItemId"].Visible = false;
			GrdSecondView.RootTable.Columns["ItemUomId"].Visible = false;
			GrdSecondView.RootTable.Columns["OrderExpiryDate"].Caption = "Expiry Date";
			if (BranchImplemented)
			{
				GrdSecondView.RootTable.Columns["BranchName"].Visible = false;
			}
			else
			{
				GrdSecondView.RootTable.Columns["BranchName"].Width = 250;
			}
			GrdSecondView.RootTable.Columns["ItemName"].Width = 300;
			GrdSecondView.RootTable.Columns["PartyName"].Width = 300;
			GrdSecondView.RootTable.Columns["PackUOM"].Width = 70;
			GrdSecondView.RootTable.Columns["OrderQTY"].Width = 90;
			GrdSecondView.RootTable.Columns["DispatchQty"].Width = 90;
			GrdSecondView.RootTable.Columns["BalQty"].Width = 90;
			GrdSecondView.RootTable.Columns["OrderWeight"].Width = 90;
			GrdSecondView.RootTable.Columns["BalWeight"].Width = 90;
			GrdSecondView.RootTable.Columns["DispatchedWeight"].Width = 120;
			GrdSecondView.RootTable.Columns["OrderQTY"].AggregateFunction = (AggregateFunction)2;
			GrdSecondView.RootTable.Columns["DispatchQty"].AggregateFunction = (AggregateFunction)2;
			GrdSecondView.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			GrdSecondView.RootTable.Columns["OrderWeight"].AggregateFunction = (AggregateFunction)2;
			GrdSecondView.RootTable.Columns["DispatchedWeight"].AggregateFunction = (AggregateFunction)2;
			GrdSecondView.RootTable.Columns["BalWeight"].AggregateFunction = (AggregateFunction)2;
			GrdSecondView.RootTable.Columns["OrderQTY"].TextAlignment = (TextAlignment)3;
			GrdSecondView.RootTable.Columns["DispatchQty"].TextAlignment = (TextAlignment)3;
			GrdSecondView.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			GrdSecondView.RootTable.Columns["OrderWeight"].TextAlignment = (TextAlignment)3;
			GrdSecondView.RootTable.Columns["DispatchedWeight"].TextAlignment = (TextAlignment)3;
			GrdSecondView.RootTable.Columns["BalWeight"].TextAlignment = (TextAlignment)3;
			GrdSecondView.RootTable.Columns["OrderQTY"].FormatString = "0,0";
			GrdSecondView.RootTable.Columns["DispatchQty"].FormatString = "0,0";
			GrdSecondView.RootTable.Columns["BalQty"].FormatString = "0,0";
			GrdSecondView.RootTable.Columns["OrderWeight"].FormatString = "0,0";
			GrdSecondView.RootTable.Columns["DispatchedWeight"].FormatString = "0,0";
			GrdSecondView.RootTable.Columns["BalWeight"].FormatString = "0,0";
			GrdSecondView.RootTable.Columns["OrderQTY"].TotalFormatString = "0,0";
			GrdSecondView.RootTable.Columns["DispatchQty"].TotalFormatString = "0,0";
			GrdSecondView.RootTable.Columns["BalQty"].TotalFormatString = "0,0";
			GrdSecondView.RootTable.Columns["OrderWeight"].TotalFormatString = "0,0";
			GrdSecondView.RootTable.Columns["DispatchedWeight"].TotalFormatString = "0,0";
			GrdSecondView.RootTable.Columns["BalWeight"].TotalFormatString = "0,0";
			GrdSecondView.RootTable.Columns.Add("Select");
			GrdSecondView.RootTable.Columns["Select"].ActAsSelector = true;
			GrdSecondView.RootTable.Columns["Select"].UseHeaderSelector = true;
			GrdSecondView.RootTable.Columns["Select"].Position = 0;
			GrdSecondView.FrozenColumns = 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchFill()
	{
		try
		{
			dtBranch.Clear();
			((UltraGridBase)cmbBranchName).DataSource = null;
			if (BranchImplemented)
			{
				dtBranch.Rows.Add(UserAccount.BranchesId, UserAccount.BranchName);
			}
			else
			{
				DataTable dt1 = SaleOrder.GetBranchesAllocatedToUserFromSaleOrder(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, 81);
				if (dt1.Rows.Count > 0)
				{
					for (int i = 0; i < dt1.Rows.Count; i++)
					{
						dtBranch.Rows.Add(dt1.Rows[i]["BranchId"], dt1.Rows[i]["BranchName"]);
					}
				}
			}
			if (dtBranch.Rows.Count > 1)
			{
				DDL.BindDDL(dtBranch, cmbBranchName, "Id", "BranchName", "Branch Name", ZeroIndex: false);
				UltraGridColumn obj = ((UltraGridBase)cmbBranchName).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj).Key = "Selected";
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
				cmbBranchName.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)cmbBranchName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((Control)(object)cmbBranchName).Text = UserAccount.BranchName.ToString();
			}
			else
			{
				DDL.BindDDL(dtBranch, cmbBranchName, "Id", "BranchName", "Branch Name", ZeroIndex: false);
				((UltraGridBase)cmbBranchName).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void HistoryCombosFill()
	{
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Expected O, but got Unknown
		try
		{
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
			}
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("Customer", typeof(string));
			DataTable dt = SaleOrder.GetDataForDropDownFromSaleOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				AppId = UserAccount.AppId,
				UserId = UserAccount.ID,
				DocumentTypeIds = "81",
				BranchesIds = (BranchImplemented ? Conversion.ToString((object)UserAccount.BranchesId) : BranchIds)
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Customer")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Item")
					{
						dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtCustomer, combsuppname, "Id", "Customer", "Customer Name", ZeroIndex: false);
			DDL.BindDDL(dtItem, combitem, "Id", "ItemName", "Item Name", ZeroIndex: false);
			((UltraGridBase)combitem).DisplayLayout.Bands[0].Columns[1].Width = 500;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
				if (int.Parse(grdDetail.GetCheckedRows().Count().ToString()) > 0)
				{
					dtGDNS.Rows.Clear();
					int OrderId = 0;
					GridEXRow[] checkedRows = grdDetail.GetCheckedRows();
					foreach (GridEXRow r in checkedRows)
					{
						if (documentTypeId == 84)
						{
							dtGDNS.Rows.Add(r.Cells["OrderDetailId"].Value.ToString());
							continue;
						}
						int Id = Conversion.ToInt(r.Cells["OrderId"].Value);
						if (OrderId == 0)
						{
							OrderId = Id;
						}
						if (OrderId != Id)
						{
							dtGDNS.Rows.Clear();
							MessageBox.Show("Sorry! Select Same OrderNo Rows");
							return;
						}
						dtGDNS.Rows.Add(r.Cells["OrderId"].Value.ToString());
						OrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
					}
					Hide();
				}
				else
				{
					MessageBox.Show("No Row is Selected");
				}
			}
			else if (int.Parse(GrdSecondView.GetCheckedRows().Count().ToString()) > 0)
			{
				dtGDNS.Rows.Clear();
				int OrderId2 = 0;
				GridEXRow[] checkedRows = GrdSecondView.GetCheckedRows();
				foreach (GridEXRow r2 in checkedRows)
				{
					if (documentTypeId == 84)
					{
						dtGDNS.Rows.Add(r2.Cells["OrderDetailId"].Value.ToString());
						continue;
					}
					int Id2 = Conversion.ToInt(r2.Cells["OrderId"].Value);
					if (OrderId2 == 0)
					{
						OrderId2 = Id2;
					}
					if (OrderId2 != Id2)
					{
						dtGDNS.Rows.Clear();
						MessageBox.Show("Sorry! Select Same OrderNo Rows");
						return;
					}
					dtGDNS.Rows.Add(r2.Cells["OrderId"].Value.ToString());
					OrderId2 = Conversion.ToInt(r2.Cells["OrderId"].Value);
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
				if (orderMainIds.Contains(row.Cells["OrderId"].Value.ToString()))
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
			if (!(orderIds != ""))
			{
				return;
			}
			GridEXRow[] rows = GrdSecondView.GetRows();
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
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		dtGDNS.Rows.Add(0);
		Hide();
	}

	private void frmLoadSaleOrder_FormClosing(object sender, FormClosingEventArgs e)
	{
		dtGDNS.Rows.Add(0);
	}

	private void cmbBranchName_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbBranchName).Text != string.Empty)
			{
				HistoryCombosFill();
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
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Expected O, but got Unknown
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Expected O, but got Unknown
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Expected O, but got Unknown
		//IL_11d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_11da: Expected O, but got Unknown
		//IL_11e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f1: Expected O, but got Unknown
		//IL_11fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1208: Expected O, but got Unknown
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdSecondView_DesignTimeLayout = new GridEXLayout();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
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
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdDetail = new GridEX();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel11 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.GrdSecondView = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.cmbBranchName = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.combitem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combsuppname).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel11.SuspendLayout();
		this.panel10.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdSecondView).BeginInit();
		this.panel2.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.label23);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1082, 30);
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
		this.label1.Location = new System.Drawing.Point(455, 8);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(90, 15);
		this.label1.TabIndex = 20;
		this.label1.Text = "CustomerName";
		this.combitem.AllowNull = (DefaultableBoolean)1;
		this.combitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.combitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.combitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.combitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.combitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.combitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance24).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).TextHAlignAsString = "Left";
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.combitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.combitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combitem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combitem).Location = new System.Drawing.Point(550, 31);
		((System.Windows.Forms.Control)(object)this.combitem).Name = "combitem";
		((System.Windows.Forms.Control)(object)this.combitem).Size = new System.Drawing.Size(351, 26);
		((System.Windows.Forms.Control)(object)this.combitem).TabIndex = 3;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(455, 37);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(67, 15);
		this.label37.TabIndex = 19;
		this.label37.Text = "Item Name";
		this.combsuppname.AllowNull = (DefaultableBoolean)1;
		this.combsuppname.AutoCompleteMode = (AutoCompleteMode)4;
		this.combsuppname.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combsuppname.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combsuppname).DisplayLayout.Appearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.combsuppname).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combsuppname).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combsuppname).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsuppname).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance39;
		((SpecialBoxBase)((UltraGridBase)this.combsuppname).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsuppname).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.combsuppname).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combsuppname).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance44).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).TextHAlignAsString = "Left";
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.combsuppname).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combsuppname).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combsuppname).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combsuppname).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combsuppname.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combsuppname).Location = new System.Drawing.Point(550, 2);
		((System.Windows.Forms.Control)(object)this.combsuppname).Name = "combsuppname";
		((System.Windows.Forms.Control)(object)this.combsuppname).Size = new System.Drawing.Size(351, 26);
		((System.Windows.Forms.Control)(object)this.combsuppname).TabIndex = 2;
		this.Todate.CustomFormat = "dd-MMM-yy";
		this.Todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.Todate.Location = new System.Drawing.Point(289, 33);
		this.Todate.Name = "Todate";
		this.Todate.Size = new System.Drawing.Size(155, 23);
		this.Todate.TabIndex = 1;
		this.FromDate.CustomFormat = "dd-MMM-yy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(289, 4);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(155, 23);
		this.FromDate.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(218, 37);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 15);
		this.label2.TabIndex = 15;
		this.label2.Text = "To Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(218, 8);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(63, 15);
		this.label3.TabIndex = 16;
		this.label3.Text = "From Date";
		this.btnLoadOnInvoice.BackColor = System.Drawing.Color.Teal;
		this.btnLoadOnInvoice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnLoadOnInvoice.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadOnInvoice.ForeColor = System.Drawing.Color.White;
		this.btnLoadOnInvoice.Location = new System.Drawing.Point(978, 31);
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
		this.btngrnlod.Location = new System.Drawing.Point(907, 31);
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
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1068, 277);
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
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1068, 30);
		this.panel5.TabIndex = 3;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1020, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = null;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(44, 24);
		this.ctrlGrdBar1.TabIndex = 10;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
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
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1068, 284);
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
		this.tabControl1.Size = new System.Drawing.Size(1082, 652);
		this.tabControl1.TabIndex = 33;
		this.tabPage1.Controls.Add(this.panel11);
		this.tabPage1.Controls.Add(this.panel10);
		this.tabPage1.Location = new System.Drawing.Point(4, 22);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1074, 626);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Main Detail";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel11.Controls.Add(this.panel6);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(3, 310);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1068, 313);
		this.panel11.TabIndex = 1;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1068, 29);
		this.panel6.TabIndex = 6;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel10.Controls.Add(this.panel5);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 3);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1068, 307);
		this.panel10.TabIndex = 0;
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GrdSecondView);
		this.tabPage2.Controls.Add(this.panel2);
		this.tabPage2.Location = new System.Drawing.Point(4, 22);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1074, 626);
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
		((System.Windows.Forms.Control)(object)this.GrdSecondView).Size = new System.Drawing.Size(1068, 590);
		((System.Windows.Forms.Control)(object)this.GrdSecondView).TabIndex = 6;
		this.GrdSecondView.TotalRow = (InheritableBoolean)1;
		this.GrdSecondView.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdSecondView).VisualStyle = (VisualStyle)4;
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.ctrlGrdBar2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1068, 30);
		this.panel2.TabIndex = 5;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1020, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.GrdSecondView;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(44, 24);
		this.ctrlGrdBar2.TabIndex = 10;
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Controls.Add(this.panel1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1082, 749);
		this.panel7.TabIndex = 34;
		this.panel9.Controls.Add(this.tabControl1);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 97);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1082, 652);
		this.panel9.TabIndex = 32;
		this.panel8.BackColor = System.Drawing.Color.White;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.panel8.Controls.Add(this.label4);
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
		this.panel8.Size = new System.Drawing.Size(1082, 67);
		this.panel8.TabIndex = 31;
		this.cmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(8, 31);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(204, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 22;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Leave += new System.EventHandler(cmbBranchName_Leave);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(8, 11);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(79, 15);
		this.label4.TabIndex = 21;
		this.label4.Text = "Branch Name";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1082, 749);
		base.Controls.Add(this.panel7);
		base.HelpButton = true;
		base.Name = "frmLoadSaleOrder";
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
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdSecondView).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		base.ResumeLayout(false);
	}
}

}
