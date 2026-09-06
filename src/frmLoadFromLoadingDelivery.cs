using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.CmTr.TransactionForm;
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
namespace Architecture.WinApp.CmTr.TransactionForms
{

public class frmLoadFromLoadingDelivery : Form
{
	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	public DataTable dtGDNS = new DataTable();

	public DataTable dtGDNHeaderIdS = new DataTable();

	public DataTable dtdetail = new DataTable();

	public string orderDetailIds = "";

	public string orderMainIds = "";

	private DataTable dtMain = new DataTable();

	private DataTable dtAllPendingData = new DataTable();

	public int documentTypeId;

	private IContainer components;

	private Panel panel1;

	private Label label23;

	private Button btnShow;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnLoadOnInvoice;

	private DateTimePicker Todate;

	private DateTimePicker FromDate;

	private Label label2;

	private Label label3;

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

	private Panel panel8;

	public frmLoadFromLoadingDelivery(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmLoadFromLoadingDelivery_Load(object sender, EventArgs e)
	{
		try
		{
			dtGDNS.Columns.Add("Id");
			dtGDNHeaderIdS.Columns.Add("Id");
			dtMain.Columns.Add("DocumentTypeId", typeof(int));
			dtMain.Columns.Add("Id", typeof(int));
			dtMain.Columns.Add("DocDate", typeof(DateTime));
			dtMain.Columns.Add("DocNo", typeof(string));
			dtMain.Columns.Add("ManualRefNo", typeof(string));
			dtMain.Columns.Add("RemarksHeader", typeof(string));
			dtMain.Columns.Add("BrokerId", typeof(int));
			dtMain.Columns.Add("BrokerName", typeof(string));
			dtMain.Columns.Add("EntryDate", typeof(DateTime));
			dtMain.Columns.Add("ModifyDate", typeof(DateTime));
			dtMain.Columns.Add("IsApproved", typeof(bool));
			dtMain.Columns.Add("ApprovedDate", typeof(DateTime));
			dtMain.Columns.Add("EntryUserName", typeof(string));
			dtMain.Columns.Add("ModifyUserName", typeof(string));
			dtMain.Columns.Add("NoOfAttachments", typeof(int));
			grd.DataSource = dtMain;
			grd.RetrieveStructure();
			grdSettingsForGrnPending();
			dtdetail.Columns.Add("Id", typeof(int));
			dtdetail.Columns.Add("DetailId", typeof(int));
			dtdetail.Columns.Add("OrderId", typeof(int));
			dtdetail.Columns.Add("OrderDetailId", typeof(int));
			dtdetail.Columns.Add("OrderNo", typeof(string));
			dtdetail.Columns.Add("OrderDate", typeof(DateTime));
			dtdetail.Columns.Add("DocDateDt", typeof(DateTime));
			dtdetail.Columns.Add("CustomerId", typeof(int));
			dtdetail.Columns.Add("CustomerName", typeof(string));
			dtdetail.Columns.Add("SupplierId", typeof(int));
			dtdetail.Columns.Add("SupplierName", typeof(string));
			dtdetail.Columns.Add("ItemId", typeof(int));
			dtdetail.Columns.Add("ItemName", typeof(string));
			dtdetail.Columns.Add("ItemCode", typeof(string));
			dtdetail.Columns.Add("CropYearId", typeof(int));
			dtdetail.Columns.Add("CropYear", typeof(string));
			dtdetail.Columns.Add("PackingType", typeof(string));
			dtdetail.Columns.Add("UomId", typeof(int));
			dtdetail.Columns.Add("Uom", typeof(string));
			dtdetail.Columns.Add("UomEquivalent", typeof(decimal));
			dtdetail.Columns.Add("PackingTypeId", typeof(int));
			dtdetail.Columns.Add("Qty", typeof(decimal));
			dtdetail.Columns.Add("SupGrossWeight", typeof(decimal));
			dtdetail.Columns.Add("SupPckMtrlWeight", typeof(decimal));
			dtdetail.Columns.Add("SupAdLsWt", typeof(decimal));
			dtdetail.Columns.Add("SupNetWeight", typeof(decimal));
			dtdetail.Columns.Add("SupRemarks", typeof(string));
			dtdetail.Columns.Add("CusGrossWeight", typeof(decimal));
			dtdetail.Columns.Add("CusPckMtrlWeight", typeof(decimal));
			dtdetail.Columns.Add("CusAdLsWt", typeof(decimal));
			dtdetail.Columns.Add("CusNetWeight", typeof(decimal));
			dtdetail.Columns.Add("CusRemarks", typeof(string));
			dtdetail.Columns.Add("TotalRecOutQty", typeof(int));
			dtdetail.Columns.Add("SupTotalRecWeight", typeof(decimal));
			dtdetail.Columns.Add("CusTotalOutWeight", typeof(decimal));
			dtdetail.Columns.Add("BalQty", typeof(int));
			dtdetail.Columns.Add("BalSupWeight", typeof(decimal));
			dtdetail.Columns.Add("BalCusWeight", typeof(decimal));
			dtdetail.Columns.Add("TransporterId", typeof(int));
			dtdetail.Columns.Add("TransporterName", typeof(string));
			dtdetail.Columns.Add("BiltyNo", typeof(string));
			dtdetail.Columns.Add("BiltyDate", typeof(DateTime));
			dtdetail.Columns.Add("BiltyFreight", typeof(decimal));
			dtdetail.Columns.Add("BiltyExpense", typeof(decimal));
			dtdetail.Columns.Add("VehcileNo", typeof(string));
			dtdetail.Columns.Add("VehicleGrossWeight", typeof(decimal));
			grdDetail.DataSource = dtdetail;
			grdDetail.RetrieveStructure();
			DataGridHistoryDetailSetting();
			FromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			ItemNameFill();
			PendingCommTradeLoadingDelivery();
			CheckedAllMainRows();
			GetCheckedRowsId();
			GridDetailBind(orderMainIds);
			CheckedAllDetailRows();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			PendingCommTradeLoadingDelivery();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void PendingCommTradeLoadingDelivery()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			ReportsParameters gdn = new ReportsParameters();
			gdn.OrganizationId = UserAccount.OrganizationId;
			gdn.CompanyId = UserAccount.CompanyId;
			gdn.BranchesId = UserAccount.BranchesId;
			gdn.ItemId = Conversion.ToInt(combitem.Value);
			gdn.FromDate = Conversion.ToDateTime((object)FromDate.Value);
			gdn.ToDate = Conversion.ToDateTime((object)Todate.Value);
			gdn.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			gdn.DocumentTypeId = 1702;
			dtAllPendingData = CommTradeLoadingDelivery.PendingCommTradeLoadingDeliveryForBill(gdn);
			if (dtAllPendingData.Rows.Count > 0)
			{
				dtMain.Clear();
				grdDetail.ClearStructure();
				int i;
				for (i = 0; i < dtAllPendingData.Rows.Count; i++)
				{
					if (!dtMain.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(dtAllPendingData.Rows[i]["Id"])))
					{
						dtMain.Rows.Add(dtAllPendingData.Rows[i]["DocumentTypeId"], dtAllPendingData.Rows[i]["Id"], dtAllPendingData.Rows[i]["DocDate"], dtAllPendingData.Rows[i]["DocNo"], dtAllPendingData.Rows[i]["ManualRefNo"], dtAllPendingData.Rows[i]["RemarksHeader"], dtAllPendingData.Rows[i]["BrokerId"], dtAllPendingData.Rows[i]["BrokerName"], dtAllPendingData.Rows[i]["EntryDate"], dtAllPendingData.Rows[i]["ModifyDate"], dtAllPendingData.Rows[i]["IsApproved"], dtAllPendingData.Rows[i]["ApprovedDate"], dtAllPendingData.Rows[i]["EntryUserName"], dtAllPendingData.Rows[i]["ModifyUserName"], dtAllPendingData.Rows[i]["NoOfAttachments"]);
					}
				}
				grd.DataSource = dtMain;
				grd.RetrieveStructure();
				grdSettingsForGrnPending();
			}
			else
			{
				grd.ClearStructure();
				grdDetail.ClearStructure();
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
			grd.RootTable.Columns["DocumentTypeId"].Visible = false;
			grd.RootTable.Columns["BrokerId"].Visible = false;
			grd.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void grd_SelectionChanged(object sender, EventArgs e)
	{
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Select")
			{
				GetCheckedRowsId();
				GridDetailBind(orderMainIds);
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
				GridDetailBind(orderMainIds);
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
				GridDetailBind(orderMainIds);
			}
			CheckedAllDetailRows();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "NoOfAttachments")
			{
				int id = Conversion.ToInt(grd.CurrentRow.Cells["Id"].Value);
				int DocumentTypeId = Conversion.ToInt(grd.CurrentRow.Cells["DocumentTypeId"].Value);
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(id, DocumentTypeId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailBind(string OrderMainIds)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (OrderMainIds != "")
			{
				GridEXRow item = grd.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					if (item.Cells.Count > 0)
					{
						List<int> idList = (from id in OrderMainIds.Split(',')
							select Conversion.ToInt((object)id)).ToList();
						DataTable resultRows = (from row in dtAllPendingData.AsEnumerable()
							where idList.Contains(row.Field<int>("Id"))
							select row).CopyToDataTable();
						dtdetail.Clear();
						for (int i = 0; i < resultRows.Rows.Count; i++)
						{
							dtdetail.Rows.Add(resultRows.Rows[i]["Id"], resultRows.Rows[i]["DetailId"], resultRows.Rows[i]["OrderId"], resultRows.Rows[i]["OrderDetailId"], resultRows.Rows[i]["OrderNo"], resultRows.Rows[i]["OrderDate"], resultRows.Rows[i]["DocDateDt"], resultRows.Rows[i]["CustomerId"], resultRows.Rows[i]["CustomerName"], resultRows.Rows[i]["SupplierId"], resultRows.Rows[i]["SupplierName"], resultRows.Rows[i]["ItemId"], resultRows.Rows[i]["ItemName"], resultRows.Rows[i]["ItemCode"], resultRows.Rows[i]["CropYearId"], resultRows.Rows[i]["CropYear"], resultRows.Rows[i]["PackingType"], resultRows.Rows[i]["UomId"], resultRows.Rows[i]["PackUom"], resultRows.Rows[i]["PackUomEquivalent"], resultRows.Rows[i]["PackingTypeId"], resultRows.Rows[i]["Qty"], resultRows.Rows[i]["SupGrossWeight"], resultRows.Rows[i]["SupPckMtrlWeight"], resultRows.Rows[i]["SupAdLsWt"], resultRows.Rows[i]["SupNetWeight"], resultRows.Rows[i]["SupRemarks"], resultRows.Rows[i]["CusGrossWeight"], resultRows.Rows[i]["CusPckMtrlWeight"], resultRows.Rows[i]["CusAdLsWt"], resultRows.Rows[i]["CusNetWeight"], resultRows.Rows[i]["CusRemarks"], resultRows.Rows[i]["TotalRecOutQty"], resultRows.Rows[i]["SupTotalRecWeight"], resultRows.Rows[i]["CusTotalOutWeight"], resultRows.Rows[i]["BalQty"], resultRows.Rows[i]["BalSupWeight"], resultRows.Rows[i]["BalCusWeight"], resultRows.Rows[i]["TransporterId"], resultRows.Rows[i]["TransporterName"], resultRows.Rows[i]["BiltyNo"], resultRows.Rows[i]["BiltyDate"], resultRows.Rows[i]["BiltyFreight"], resultRows.Rows[i]["BiltyExpense"], resultRows.Rows[i]["VehicleNo"], resultRows.Rows[i]["VehicleGrossWeight"]);
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
			grdDetail.RootTable.Columns["SupplierId"].Visible = false;
			grdDetail.RootTable.Columns["CustomerId"].Visible = false;
			grdDetail.RootTable.Columns["OrderDetailId"].Visible = false;
			grdDetail.RootTable.Columns["CropYearId"].Visible = false;
			grdDetail.RootTable.Columns["UomEquivalent"].Visible = false;
			grdDetail.RootTable.Columns["PackingTypeId"].Visible = false;
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["DetailId"].Visible = false;
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["UomId"].Visible = false;
			grdDetail.RootTable.Columns["DocDateDt"].Visible = false;
			grdDetail.RootTable.Columns["TransporterId"].Visible = false;
			grdDetail.RootTable.Columns["VehicleGrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["VehicleGrossWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["VehicleGrossWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["VehicleGrossWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["BiltyFreight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["BiltyFreight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["BiltyFreight"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["BiltyFreight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["BiltyExpense"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["BiltyExpense"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["BiltyExpense"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["BiltyExpense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Qty"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["SupGrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["SupGrossWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["SupGrossWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["SupGrossWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["SupPckMtrlWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["SupPckMtrlWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["SupPckMtrlWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["SupPckMtrlWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["SupAdLsWt"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["SupAdLsWt"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["SupAdLsWt"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["SupAdLsWt"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["SupNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["SupNetWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["SupNetWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["SupNetWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["CusGrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CusGrossWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CusGrossWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["CusGrossWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["CusPckMtrlWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CusPckMtrlWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CusPckMtrlWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["CusPckMtrlWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["CusAdLsWt"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CusAdLsWt"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CusAdLsWt"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["CusAdLsWt"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["CusNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CusNetWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CusNetWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["CusNetWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["TotalRecOutQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["TotalRecOutQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["TotalRecOutQty"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["TotalRecOutQty"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["SupTotalRecWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["SupTotalRecWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["SupTotalRecWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["SupTotalRecWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["CusTotalOutWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CusTotalOutWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CusTotalOutWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["CusTotalOutWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["BalQty"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["BalQty"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["BalSupWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["BalSupWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["BalSupWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["BalSupWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["BalCusWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["BalCusWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["BalCusWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["BalCusWeight"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(grdDetail);
			grdDetail.RootTable.Columns.Add("Select");
			grdDetail.RootTable.Columns["Select"].ActAsSelector = true;
			grdDetail.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdDetail.RootTable.Columns["Select"].Position = 0;
			grdDetail.RootTable.Columns["Select"].Width = 30;
			grdDetail.FrozenColumns = 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetCheckedRowsId()
	{
		try
		{
			orderMainIds = "";
			if (grd.GetCheckedRows().Count() > 0)
			{
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				for (int i = 0; i < checkedRows.Length; i++)
				{
					int Id = Conversion.ToInt(checkedRows[i].Cells["Id"].Value);
					orderMainIds = orderMainIds + "," + Id;
				}
			}
			else
			{
				orderMainIds = "";
			}
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
			if (orderDetailIds != "")
			{
				foreach (int Id in from id in orderDetailIds.Split(',').Distinct()
					select Conversion.ToInt((object)id))
				{
					if (Id == 0)
					{
						continue;
					}
					GridEXRow[] rows = grdDetail.GetRows();
					foreach (GridEXRow row in rows)
					{
						row.BeginEdit();
						if (Conversion.ToInt(row.Cells["DetailId"].Value) == Id)
						{
							row.IsChecked = Convert.ToBoolean(value: true);
							row.EndEdit();
						}
					}
				}
				grdDetail.UpdateData();
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

	private void ItemNameFill()
	{
		try
		{
			DataTable dtItem = CommonServices.ItemGetForComboServiceBind();
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtItem, combitem, "Id", "ItemName", "Item Name", ZeroIndex: false);
				((UltraGridBase)combitem).DisplayLayout.Bands[0].Columns[1].Width = 500;
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

	private void btnLoadOnInvoice_Click(object sender, EventArgs e)
	{
		try
		{
			GridEXRow[] checkedRows = grdDetail.GetCheckedRows();
			if (checkedRows.Count() > 0)
			{
				dtGDNS.Rows.Clear();
				dtGDNHeaderIdS.Rows.Clear();
				HashSet<int> customerIds = new HashSet<int>();
				GridEXRow[] array = checkedRows;
				for (int i = 0; i < array.Length; i++)
				{
					int customerId = Conversion.ToInt(array[i].Cells["CustomerId"].Value);
					customerIds.Add(customerId);
				}
				if (customerIds.Count > 1)
				{
					dtGDNS.Rows.Clear();
					dtGDNHeaderIdS.Rows.Clear();
					MessageBox.Show("Sorry! Select Same Customer Data Rows");
					return;
				}
				int selectedCustomerId = customerIds.First();
				array = grdDetail.GetDataRows();
				foreach (GridEXRow r in array)
				{
					if (Conversion.ToInt(r.Cells["CustomerId"].Value) == selectedCustomerId)
					{
						dtGDNS.Rows.Add(r.Cells["DetailId"].Value.ToString());
						dtGDNHeaderIdS.Rows.Add(r.Cells["Id"].Value.ToString());
					}
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

	private void CheckedAllMainRows1()
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

	private void CheckedAllMainRows()
	{
		try
		{
			if (string.IsNullOrWhiteSpace(orderMainIds))
			{
				return;
			}
			int result;
			HashSet<int> orderIdsSet = new HashSet<int>((from id in orderMainIds.Split(',')
				where int.TryParse(id.Trim(), out result)
				select id).Select(int.Parse));
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow row in rows)
			{
				int rowId = Conversion.ToInt(row.Cells["Id"].Value);
				if (orderIdsSet.Contains(rowId))
				{
					row.BeginEdit();
					row.IsChecked = true;
					row.EndEdit();
				}
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		dtGDNS.Rows.Add(0);
		dtGDNHeaderIdS.Rows.Add(0);
		Hide();
	}

	private void frmLoadFromLoadingDelivery_FormClosing(object sender, FormClosingEventArgs e)
	{
		dtGDNS.Rows.Add(0);
		dtGDNHeaderIdS.Rows.Add(0);
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
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_0bf3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfd: Expected O, but got Unknown
		//IL_0c0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c14: Expected O, but got Unknown
		//IL_0c21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c2b: Expected O, but got Unknown
		//IL_0c4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c59: Expected O, but got Unknown
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
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.combitem = new UltraCombo();
		this.label37 = new System.Windows.Forms.Label();
		this.Todate = new System.Windows.Forms.DateTimePicker();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.btnLoadOnInvoice = new System.Windows.Forms.Button();
		this.btnShow = new System.Windows.Forms.Button();
		this.grd = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdDetail = new GridEX();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel11 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.combitem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel11.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel8.SuspendLayout();
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
		this.label23.Size = new System.Drawing.Size(216, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "Pending Loading Delivery";
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
		((System.Windows.Forms.Control)(object)this.combitem).Location = new System.Drawing.Point(336, 3);
		((System.Windows.Forms.Control)(object)this.combitem).Name = "combitem";
		((System.Windows.Forms.Control)(object)this.combitem).Size = new System.Drawing.Size(351, 26);
		((System.Windows.Forms.Control)(object)this.combitem).TabIndex = 3;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(241, 9);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(67, 15);
		this.label37.TabIndex = 19;
		this.label37.Text = "Item Name";
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
		this.btnLoadOnInvoice.Location = new System.Drawing.Point(616, 33);
		this.btnLoadOnInvoice.Name = "btnLoadOnInvoice";
		this.btnLoadOnInvoice.Size = new System.Drawing.Size(71, 26);
		this.btnLoadOnInvoice.TabIndex = 5;
		this.btnLoadOnInvoice.Text = "Load";
		this.btnLoadOnInvoice.UseVisualStyleBackColor = false;
		this.btnLoadOnInvoice.Click += new System.EventHandler(btnLoadOnInvoice_Click);
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(545, 33);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(71, 26);
		this.btnShow.TabIndex = 4;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
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
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
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
		this.ctrlGrdBar1.MyGrid = this.grd;
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
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1068, 287);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 6;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1082, 658);
		this.tabControl1.TabIndex = 33;
		this.tabPage1.Controls.Add(this.panel11);
		this.tabPage1.Controls.Add(this.panel10);
		this.tabPage1.Location = new System.Drawing.Point(4, 25);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1074, 629);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Main Detail";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel11.Controls.Add(this.panel6);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(3, 310);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1068, 316);
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
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Controls.Add(this.panel1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1082, 755);
		this.panel7.TabIndex = 34;
		this.panel9.Controls.Add(this.tabControl1);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 97);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1082, 658);
		this.panel9.TabIndex = 32;
		this.panel8.BackColor = System.Drawing.Color.White;
		this.panel8.Controls.Add(this.label3);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.combitem);
		this.panel8.Controls.Add(this.btnShow);
		this.panel8.Controls.Add(this.label37);
		this.panel8.Controls.Add(this.btnLoadOnInvoice);
		this.panel8.Controls.Add(this.label2);
		this.panel8.Controls.Add(this.Todate);
		this.panel8.Controls.Add(this.FromDate);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 30);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1082, 67);
		this.panel8.TabIndex = 31;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1082, 755);
		base.Controls.Add(this.panel7);
		base.HelpButton = true;
		base.Name = "frmLoadFromLoadingDelivery";
		this.Text = "LoadFromLoadingDelivery";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmLoadFromLoadingDelivery_FormClosing);
		base.Load += new System.EventHandler(frmLoadFromLoadingDelivery_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.combitem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
