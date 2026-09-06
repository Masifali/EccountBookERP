using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.BLL.Reports.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.ApprovalDashboard;

public class frmExportSalesContractApproval : Form
{
	private bool FormInitialized;

	public DateTime RequestedFromDate;

	public DateTime RequestedToDate;

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formrights = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Panel panel17;

	private ToolStrip toolStrip3;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grddetailhistory;

	private Label label52;

	private Panel panel13;

	private Label label59;

	internal GridEX DataGridHistory;

	private Panel panel29;

	private Label label65;

	private Panel panel18;

	private GroupBox groupBox5;

	private RadioButton rdapproveddate;

	private Button btnShowHistory;

	private RadioButton rdmodifydate;

	private Label label67;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Label label73;

	private Label label89;

	private Label label90;

	private Label label91;

	internal UltraCombo CmbCustomerHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	private TabControl tabControl3;

	private TabPage tabDetailH;

	private TabPage tabPaymentDetailH;

	internal GridEX grdPaymentHistory;

	private Panel panel31;

	private Label label74;

	private TabPage tabPmH;

	internal GridEX grdPMHistory;

	private Panel panel32;

	private Label label75;

	private TabPage tabOtherItemH;

	internal GridEX grdOtherItemHistory;

	private Panel panel33;

	private Label label76;

	private TabPage tabOtherChargesHistory;

	private Panel panel39;

	private Label label111;

	internal GridEX grdOtherChargesHistory;

	private ToolStripButton btnApprove;

	public frmExportSalesContractApproval(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void FrmExportSalesContract_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 700);
			HistoryCombosFill();
			if (!Conversion.CheckDateTimeNull(RequestedFromDate) && !Conversion.CheckDateTimeNull(RequestedToDate))
			{
				FromDateHistory.Value = Convert.ToDateTime(RequestedFromDate);
				ToDateHistory.Value = Convert.ToDateTime(RequestedToDate);
			}
			else
			{
				FromDateHistory.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillOtherChargesDetailFromListCommonForReadById(ExImLcOrder lcOrder, DataTable dt)
	{
		if (lcOrder == null)
		{
			return;
		}
		List<ExImLcOrderOtherChargesDetail> details = lcOrder.ExImLcOrderOtherChargesDetailList;
		if (details == null || !details.Any())
		{
			return;
		}
		foreach (ExImLcOrderOtherChargesDetail detail in details)
		{
			List<object> rowValues = new List<object> { detail.Id, detail.ChargesItemId, detail.ChargesItemName, detail.AddAmount, detail.LessAmount, detail.Remarks };
			dt.Rows.Add(rowValues.ToArray());
		}
	}

	private void FrmExportSalesContract_FormClosing(object sender, FormClosingEventArgs e)
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

	private void HistoryFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeIds = "202";
			obj.CanViewAllRecord = true;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			obj.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			obj.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			obj.IsApproved = false;
			DataTable dt = ExImLcOrder.FormHistory(obj);
			if (dt.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("DocNo", typeof(int));
				dtcol.Columns.Add("DocDate", typeof(DateTime));
				dtcol.Columns.Add("OrderNo", typeof(string));
				dtcol.Columns.Add("OrderDate", typeof(DateTime));
				dtcol.Columns.Add("CustomerName", typeof(string));
				dtcol.Columns.Add("LotReference", typeof(string));
				dtcol.Columns.Add("M.Tons", typeof(double));
				dtcol.Columns.Add("NoOfContainers", typeof(int));
				dtcol.Columns.Add("FcyCode", typeof(string));
				dtcol.Columns.Add("FcyAmount", typeof(double));
				dtcol.Columns.Add("LastShipmentDate", typeof(DateTime));
				dtcol.Columns.Add("NotifyParty", typeof(string));
				dtcol.Columns.Add("DeliveryTerm", typeof(string));
				dtcol.Columns.Add("PaymentTerm", typeof(string));
				dtcol.Columns.Add("LoadingPort", typeof(string));
				dtcol.Columns.Add("DestinationPort", typeof(string));
				dtcol.Columns.Add("Commodity_Description", typeof(string));
				dtcol.Columns.Add("EntryUser");
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("ModifyUser");
				dtcol.Columns.Add("ModifyDate", typeof(DateTime));
				dtcol.Columns.Add("NoOfAttachments", typeof(int));
				dtcol.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtcol.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["LcOrderDocNo"], Conversion.ToDateTime(dt.Rows[i]["SalesContratDate"]).ToShortDateString(), dt.Rows[i]["LcOrderNo"], Conversion.ToDateTime(dt.Rows[i]["LcOrderDate"]).ToShortDateString(), dt.Rows[i]["CustomerName"], dt.Rows[i]["LotReference"], dt.Rows[i]["NetKgs"], dt.Rows[i]["NoOfContr"], dt.Rows[i]["CurrencyCode"], dt.Rows[i]["FCurrencyAmount"], Conversion.ToDateTime(dt.Rows[i]["LastShipmentDate"]).ToShortDateString(), dt.Rows[i]["NotifyParty"], dt.Rows[i]["DeliveryTerm"], dt.Rows[i]["PaymentTerm"], dt.Rows[i]["LoadingPort"], dt.Rows[i]["PortName"], dt.Rows[i]["Commodity_Description"], dt.Rows[i]["EntryUserName"], dt.Rows[i]["EntryDate"], dt.Rows[i]["ModifyUserName"], dt.Rows[i]["ModifyDate"], dt.Rows[i]["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dtcol;
				DataGridHistory.RetrieveStructure();
				GridSetting();
			}
			else
			{
				DataGridHistory.ClearStructure();
				grddetailhistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting()
	{
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 3, 3);
			DataGridHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			DataGridHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant + 15;
			DataGridHistory.RootTable.Columns["OrderNo"].Width = Constants.InventoryConstants.StringConstant;
			DataGridHistory.RootTable.Columns["OrderDate"].Width = Constants.InventoryConstants.DateConstant + 15;
			DataGridHistory.RootTable.Columns["CustomerName"].Width = Constants.InventoryConstants.SupplierName + 50;
			DataGridHistory.RootTable.Columns["LotReference"].Width = Constants.InventoryConstants.JobLotConstant + 40;
			DataGridHistory.RootTable.Columns["M.Tons"].Width = Constants.InventoryConstants.QtyConstant;
			DataGridHistory.RootTable.Columns["NoOfContainers"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 15;
			DataGridHistory.RootTable.Columns["FcyCode"].Width = Constants.InventoryConstants.FcyConstant;
			DataGridHistory.RootTable.Columns["FcyAmount"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["LastShipmentDate"].Width = Constants.InventoryConstants.DateConstant + 15;
			DataGridHistory.RootTable.Columns["NotifyParty"].Width = Constants.InventoryConstants.SupplierName;
			DataGridHistory.RootTable.Columns["DeliveryTerm"].Width = Constants.InventoryConstants.TermConstant + 20;
			DataGridHistory.RootTable.Columns["PaymentTerm"].Width = Constants.InventoryConstants.TermConstant;
			DataGridHistory.RootTable.Columns["LoadingPort"].Width = Constants.InventoryConstants.PortConstant;
			DataGridHistory.RootTable.Columns["DestinationPort"].Width = Constants.InventoryConstants.PortConstant;
			DataGridHistory.RootTable.Columns["Commodity_Description"].Width = Constants.InventoryConstants.StringConstant;
			DataGridHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DataGridHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			DataGridHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			DataGridHistory.RootTable.Columns["M.Tons"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["M.Tons"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["M.Tons"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["M.Tons"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["NoOfContainers"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["NoOfContainers"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			DataGridHistory.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 45;
			DataGridHistory.RootTable.Columns["Print"].Position = 0;
			DataGridHistory.RootTable.Columns.Add("Print501A");
			DataGridHistory.RootTable.Columns["Print501A"].Caption = "Print501A";
			DataGridHistory.RootTable.Columns["Print501A"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print501A"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print501A"].ButtonText = "Print501A";
			DataGridHistory.RootTable.Columns["Print501A"].Width = 65;
			DataGridHistory.RootTable.Columns["Print501A"].Position = 1;
			DataGridHistory.RootTable.Columns.Add("PrintII");
			DataGridHistory.RootTable.Columns["PrintII"].Caption = "PrintII";
			DataGridHistory.RootTable.Columns["PrintII"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["PrintII"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["PrintII"].ButtonText = "PrintII";
			DataGridHistory.RootTable.Columns["PrintII"].Width = 50;
			DataGridHistory.RootTable.Columns["PrintII"].Position = 2;
			DataGridHistory.RootTable.Columns.Add("Print523");
			DataGridHistory.RootTable.Columns["Print523"].Caption = "Print523";
			DataGridHistory.RootTable.Columns["Print523"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print523"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print523"].ButtonText = "Print523";
			DataGridHistory.RootTable.Columns["Print523"].Width = 60;
			DataGridHistory.RootTable.Columns["Print523"].Position = 3;
			GridEX_Helper.AddButton(DataGridHistory, "Approve", "Approve", 60, 4);
			GridEX_Helper.AddSelectButton(DataGridHistory, "Select", 30, 5);
			DataGridHistory.FrozenColumns = 6;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && (int)r.RowType != 5)
			{
				_ = r.Cells.Count;
				_ = 0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && (int)r.RowType != 5 && r.Cells.Count > 0)
			{
				GetDetailGrdByHeadId(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId(int Id)
	{
		try
		{
			ExImLcOrder Obj = ExImLcOrder.GetByID(Id);
			if (Obj != null && Obj.LcOrderDetail.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("ItemCode");
				dt.Columns.Add("ItemId");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("PackTypeId");
				dt.Columns.Add("PackType");
				dt.Columns.Add("CropYearId");
				dt.Columns.Add("CropYear");
				dt.Columns.Add("Qty/M.Ton", typeof(double));
				dt.Columns.Add("PackSizeId");
				dt.Columns.Add("PackSize");
				dt.Columns.Add("NoOfBags", typeof(double));
				dt.Columns.Add("Rate", typeof(double));
				dt.Columns.Add("RateUOMId");
				dt.Columns.Add("RateUOM");
				dt.Columns.Add("Amount", typeof(double));
				dt.Columns.Add("OtherItemAmount", typeof(double));
				dt.Columns.Add("CommodityDetail");
				dt.Columns.Add("FarmingNTrade");
				dt.Columns.Add("Remarks");
				dt.Clear();
				for (int l = 0; l < Obj.LcOrderDetail.Count; l++)
				{
					dt.Rows.Add(Obj.LcOrderDetail[l].Id, Obj.LcOrderDetail[l].ItemCode, Obj.LcOrderDetail[l].ExImItemId, Obj.LcOrderDetail[l].ItemName, Obj.LcOrderDetail[l].InvPackingMaterialTypeId, Obj.LcOrderDetail[l].ExImPackMaterialTypeName, Obj.LcOrderDetail[l].CropYearId, Obj.LcOrderDetail[l].CropYear, Obj.LcOrderDetail[l].NetWeight / 1000.0, Obj.LcOrderDetail[l].UOMScheduleIdOuter, Obj.LcOrderDetail[l].PackUom, Obj.LcOrderDetail[l].NoOfUnit, Obj.LcOrderDetail[l].RatePrice, Obj.LcOrderDetail[l].UOMScheduleIdRate, Obj.LcOrderDetail[l].RateUom, Obj.LcOrderDetail[l].TotalAmount, 0, Obj.LcOrderDetail[l].CommodityDetail, Obj.LcOrderDetail[l].FarmingNTrade, Obj.LcOrderDetail[l].Other_Description);
				}
				grddetailhistory.DataSource = dt;
				grddetailhistory.RetrieveStructure();
				grddetailhistorySettings();
				DataTable dtPaymentH = new DataTable();
				dtPaymentH.Columns.Add("PaymentTerm");
				dtPaymentH.Columns.Add("%ofTotal", typeof(double));
				dtPaymentH.Columns.Add("FcyAmount", typeof(double));
				dtPaymentH.Columns.Add("Remarks");
				dtPaymentH.Clear();
				for (int k = 0; k < Obj.ExImLcOrderPaymentTermsDetailslist.Count; k++)
				{
					dtPaymentH.Rows.Add(Obj.ExImLcOrderPaymentTermsDetailslist[k].LcOrderTerm, Obj.ExImLcOrderPaymentTermsDetailslist[k].PrcntOfTotal, Obj.ExImLcOrderPaymentTermsDetailslist[k].FcyAmount, Obj.ExImLcOrderPaymentTermsDetailslist[k].Remarks);
				}
				grdPaymentHistory.DataSource = dtPaymentH;
				grdPaymentHistory.RetrieveStructure();
				grdSettingsPaymentHistory();
				DataTable dtPmH = new DataTable();
				dtPmH.Columns.Add("Id", typeof(int));
				dtPmH.Columns.Add("BrandId", typeof(int));
				dtPmH.Columns.Add("BrandCode");
				dtPmH.Columns.Add("BrandName");
				dtPmH.Columns.Add("BrandUomId", typeof(int));
				dtPmH.Columns.Add("BrandUom");
				dtPmH.Columns.Add("PackingTypeId", typeof(int));
				dtPmH.Columns.Add("PackingType");
				dtPmH.Columns.Add("BrandOuterQty", typeof(double));
				dtPmH.Columns.Add("BrandInnerQty", typeof(double));
				dtPmH.Columns.Add("PmItemId", typeof(int));
				dtPmH.Columns.Add("PmItemCode");
				dtPmH.Columns.Add("PmItemName");
				dtPmH.Columns.Add("PmItemUomId", typeof(int));
				dtPmH.Columns.Add("PmItemUom");
				dtPmH.Columns.Add("PmItemOuterQty", typeof(double));
				dtPmH.Columns.Add("PmItemInnerQty", typeof(double));
				dtPmH.Columns.Add("Rate", typeof(double));
				dtPmH.Columns.Add("Amount", typeof(double));
				dtPmH.Columns.Add("Remarks");
				dtPmH.Clear();
				for (int j = 0; j < Obj.ExImLcContractPackingMaterialDetailslist.Count; j++)
				{
					dtPmH.Rows.Add(Obj.ExImLcContractPackingMaterialDetailslist[j].Id, Obj.ExImLcContractPackingMaterialDetailslist[j].BrandId, Obj.ExImLcContractPackingMaterialDetailslist[j].BrandCode, Obj.ExImLcContractPackingMaterialDetailslist[j].BrandName, Obj.ExImLcContractPackingMaterialDetailslist[j].BrandUomId, Obj.ExImLcContractPackingMaterialDetailslist[j].BrandUomCode, Obj.ExImLcContractPackingMaterialDetailslist[j].PackingTypeId, Obj.ExImLcContractPackingMaterialDetailslist[j].PackingType, Obj.ExImLcContractPackingMaterialDetailslist[j].BrandOuterQty, Obj.ExImLcContractPackingMaterialDetailslist[j].BrandInnerQty, Obj.ExImLcContractPackingMaterialDetailslist[j].ItemId, Obj.ExImLcContractPackingMaterialDetailslist[j].ItemCode, Obj.ExImLcContractPackingMaterialDetailslist[j].ItemName, Obj.ExImLcContractPackingMaterialDetailslist[j].ItemUomId, Obj.ExImLcContractPackingMaterialDetailslist[j].ItemUomCode, Obj.ExImLcContractPackingMaterialDetailslist[j].Qty, Obj.ExImLcContractPackingMaterialDetailslist[j].InnerQty, Obj.ExImLcContractPackingMaterialDetailslist[j].Rate, Obj.ExImLcContractPackingMaterialDetailslist[j].Amount, Obj.ExImLcContractPackingMaterialDetailslist[j].pmRemarks);
				}
				grdPMHistory.DataSource = dtPmH;
				grdPMHistory.RetrieveStructure();
				grdSettingsPMHistory();
				DataTable dtOtherH = new DataTable();
				dtOtherH.Columns.Add("ItemName");
				dtOtherH.Columns.Add("Qty", typeof(double));
				dtOtherH.Columns.Add("Rate", typeof(double));
				dtOtherH.Columns.Add("Amount", typeof(double));
				dtOtherH.Columns.Add("Remarks");
				dtOtherH.Clear();
				for (int i = 0; i < Obj.ExImLcOrderOtherItemslist.Count; i++)
				{
					dtOtherH.Rows.Add(Obj.ExImLcOrderOtherItemslist[i].ItemName, Obj.ExImLcOrderOtherItemslist[i].oItemQty, Obj.ExImLcOrderOtherItemslist[i].oItemRate, Obj.ExImLcOrderOtherItemslist[i].oItemAmount, Obj.ExImLcOrderOtherItemslist[i].OtherItemRemarks);
				}
				grdOtherItemHistory.DataSource = dtOtherH;
				grdOtherItemHistory.RetrieveStructure();
				grdSettingsOtherItemHistory();
				DataTable dtOCH = new DataTable();
				dtOCH.Columns.Add("Id", typeof(int));
				dtOCH.Columns.Add("ChargesItemId", typeof(int));
				dtOCH.Columns.Add("ChargesItem");
				dtOCH.Columns.Add("AddAmount", typeof(double));
				dtOCH.Columns.Add("LessAmount", typeof(double));
				dtOCH.Columns.Add("Remarks");
				FillOtherChargesDetailFromListCommonForReadById(Obj, dtOCH);
				grdOtherChargesHistory.DataSource = dtOCH;
				grdOtherChargesHistory.RetrieveStructure();
				grdSettingsOtherChargesDetail(grdOtherChargesHistory);
			}
			else
			{
				grddetailhistory.ClearStructure();
				grdPaymentHistory.ClearStructure();
				grdPMHistory.ClearStructure();
				grdOtherItemHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsPaymentHistory()
	{
		try
		{
			grdPaymentHistory.RootTable.Columns["%ofTotal"].TextAlignment = (TextAlignment)3;
			grdPaymentHistory.RootTable.Columns["%ofTotal"].FormatString = "#,##0.###";
			grdPaymentHistory.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdPaymentHistory.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdPaymentHistory.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdPaymentHistory.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			CommonServices.GridAutoAdjustmentNew(grdPaymentHistory);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsPMHistory()
	{
		try
		{
			grdPMHistory.RootTable.Columns["Id"].Visible = false;
			grdPMHistory.RootTable.Columns["BrandId"].Visible = false;
			grdPMHistory.RootTable.Columns["BrandUomId"].Visible = false;
			grdPMHistory.RootTable.Columns["PackingTypeId"].Visible = false;
			grdPMHistory.RootTable.Columns["PmItemId"].Visible = false;
			grdPMHistory.RootTable.Columns["PmItemId"].Visible = false;
			grdPMHistory.RootTable.Columns["PmItemUomId"].Visible = false;
			grdPMHistory.RootTable.Columns["PmItemUomId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdPMHistory, 2, 3);
			grdPMHistory.RootTable.Columns["BrandCode"].Width = Constants.InventoryConstants.ItemCode;
			grdPMHistory.RootTable.Columns["BrandName"].Width = Constants.InventoryConstants.ItemName;
			grdPMHistory.RootTable.Columns["BrandUom"].Width = Constants.InventoryConstants.UomConstant;
			grdPMHistory.RootTable.Columns["PackingType"].Width = Constants.InventoryConstants.PackingType;
			grdPMHistory.RootTable.Columns["BrandOuterQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdPMHistory.RootTable.Columns["BrandInnerQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdPMHistory.RootTable.Columns["PmItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grdPMHistory.RootTable.Columns["PmItemName"].Width = Constants.InventoryConstants.ItemName;
			grdPMHistory.RootTable.Columns["PmItemUom"].Width = Constants.InventoryConstants.UomConstant;
			grdPMHistory.RootTable.Columns["PmItemOuterQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdPMHistory.RootTable.Columns["PmItemInnerQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdPMHistory.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdPMHistory.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdPMHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdPMHistory.RootTable.Columns["BrandOuterQty"].AggregateFunction = (AggregateFunction)2;
			grdPMHistory.RootTable.Columns["BrandOuterQty"].TextAlignment = (TextAlignment)3;
			grdPMHistory.RootTable.Columns["BrandOuterQty"].FormatString = "#,##0.###";
			grdPMHistory.RootTable.Columns["BrandOuterQty"].TotalFormatString = "#,##0.###";
			grdPMHistory.RootTable.Columns["BrandInnerQty"].AggregateFunction = (AggregateFunction)2;
			grdPMHistory.RootTable.Columns["BrandInnerQty"].TextAlignment = (TextAlignment)3;
			grdPMHistory.RootTable.Columns["BrandInnerQty"].FormatString = "#,##0.###";
			grdPMHistory.RootTable.Columns["BrandInnerQty"].TotalFormatString = "#,##0.###";
			grdPMHistory.RootTable.Columns["PmItemOuterQty"].AggregateFunction = (AggregateFunction)2;
			grdPMHistory.RootTable.Columns["PmItemOuterQty"].TextAlignment = (TextAlignment)3;
			grdPMHistory.RootTable.Columns["PmItemOuterQty"].FormatString = "#,##0.###";
			grdPMHistory.RootTable.Columns["PmItemOuterQty"].TotalFormatString = "#,##0.###";
			grdPMHistory.RootTable.Columns["PmItemInnerQty"].AggregateFunction = (AggregateFunction)2;
			grdPMHistory.RootTable.Columns["PmItemInnerQty"].TextAlignment = (TextAlignment)3;
			grdPMHistory.RootTable.Columns["PmItemInnerQty"].FormatString = "#,##0.###";
			grdPMHistory.RootTable.Columns["PmItemInnerQty"].TotalFormatString = "#,##0.###";
			grdPMHistory.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdPMHistory.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdPMHistory.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdPMHistory.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdPMHistory.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdPMHistory.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsOtherItemHistory()
	{
		try
		{
			grdOtherItemHistory.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdOtherItemHistory.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdOtherItemHistory.RootTable.Columns["Qty"].FormatString = "0,0";
			grdOtherItemHistory.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grdOtherItemHistory.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdOtherItemHistory.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdOtherItemHistory.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdOtherItemHistory.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdOtherItemHistory.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdOtherItemHistory.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			CommonServices.GridAutoAdjustmentNew(grdOtherItemHistory);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grddetailhistorySettings()
	{
		try
		{
			GridEX_Helper.GridWrappingAndColumnSettings(grddetailhistory, 2, 3);
			grddetailhistory.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grddetailhistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grddetailhistory.RootTable.Columns["PackType"].Width = Constants.InventoryConstants.PackingType;
			grddetailhistory.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			grddetailhistory.RootTable.Columns["Qty/M.Ton"].Width = Constants.InventoryConstants.QtyConstant;
			grddetailhistory.RootTable.Columns["PackSize"].Width = Constants.InventoryConstants.UomConstant;
			grddetailhistory.RootTable.Columns["NoOfBags"].Width = Constants.InventoryConstants.QtyConstant;
			grddetailhistory.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grddetailhistory.RootTable.Columns["RateUOM"].Width = Constants.InventoryConstants.UomConstant;
			grddetailhistory.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grddetailhistory.RootTable.Columns["OtherItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grddetailhistory.RootTable.Columns["CommodityDetail"].Width = Constants.InventoryConstants.StringConstant;
			grddetailhistory.RootTable.Columns["FarmingNTrade"].Width = Constants.InventoryConstants.StringConstant;
			grddetailhistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants + 50;
			grddetailhistory.RootTable.Columns["Qty/M.Ton"].AggregateFunction = (AggregateFunction)2;
			grddetailhistory.RootTable.Columns["Qty/M.Ton"].TextAlignment = (TextAlignment)3;
			grddetailhistory.RootTable.Columns["Qty/M.Ton"].FormatString = "0,0";
			grddetailhistory.RootTable.Columns["Qty/M.Ton"].TotalFormatString = "#,##0.##";
			grddetailhistory.RootTable.Columns["NoOfBags"].AggregateFunction = (AggregateFunction)2;
			grddetailhistory.RootTable.Columns["NoOfBags"].TextAlignment = (TextAlignment)3;
			grddetailhistory.RootTable.Columns["NoOfBags"].FormatString = "0,0";
			grddetailhistory.RootTable.Columns["NoOfBags"].TotalFormatString = "#,##0.##";
			grddetailhistory.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grddetailhistory.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grddetailhistory.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grddetailhistory.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grddetailhistory.RootTable.Columns["OtherItemAmount"].AggregateFunction = (AggregateFunction)2;
			grddetailhistory.RootTable.Columns["OtherItemAmount"].TextAlignment = (TextAlignment)3;
			grddetailhistory.RootTable.Columns["OtherItemAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grddetailhistory.RootTable.Columns["OtherItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grddetailhistory.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grddetailhistory.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grddetailhistory.RootTable.Columns["Id"].Visible = false;
			grddetailhistory.RootTable.Columns["ItemId"].Visible = false;
			grddetailhistory.RootTable.Columns["CropYearId"].Visible = false;
			grddetailhistory.RootTable.Columns["PackTypeId"].Visible = false;
			grddetailhistory.RootTable.Columns["PackSizeId"].Visible = false;
			grddetailhistory.RootTable.Columns["RateUOMId"].Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsOtherChargesDetail(GridEX grid)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "ChargesItemId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grid, 2, 3, hiddenColumns);
			grid.RootTable.Columns["ChargesItem"].Width = 350;
			grid.RootTable.Columns["Remarks"].Width = 350;
			if (((Control)(object)grid).Name != "grdOtherChargesHistory" && ((Control)(object)grid).Name != "grdOtherChargesCustomHistory")
			{
				GridEX_Helper.AddButton(grid, "Delete", "X", 20, 1);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void btnResetHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			FromDocNoHistory.Clear();
			ToDocNoHistory.Clear();
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			DataGridHistory.DataSource = null;
			grddetailhistory.DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryCombosFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnShowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryCombosFill()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		try
		{
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("Customer", typeof(string));
			DataTable dt = ExImLcOrder.GetDataForDropDownFromExportContract(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["ActivityType"]) == "Customer")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
					}
				}
			}
			DDL.BindDDL(dtCustomer, CmbCustomerHistory, "Id", "Customer", "Party", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && (int)r.RowType != 5 && r.Cells.Count > 0)
			{
				if (e.Column.Key == "Print")
				{
					GenerateReport(Conversion.ToInt(r.Cells["Id"].Value));
				}
				else if (e.Column.Key == "Print501A")
				{
					CommonServices.SaleContractReportsSlipNew501A(Conversion.ToInt(r.Cells["Id"].Value));
				}
				else if (e.Column.Key == "PrintII")
				{
					CommonServices.SaleContractReportsSlipNew501(Conversion.ToInt(r.Cells["Id"].Value));
				}
				else if (e.Column.Key == "Print523")
				{
					CommonServices.SalesContractExportSlip523(Conversion.ToInt(r.Cells["Id"].Value));
				}
				else if (e.Column.Key == "Approve")
				{
					ApproveSingleRecord();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ApproveSingleRecord()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Approve Record?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.Id = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
				item.EntryUser = UserAccount.ID;
				val.AllApprovalLists.Add(item);
				ExImLcOrder.ExportSalesContractApproval(val);
				MessageBox.Show("Record Approve Successfully");
				HistoryFill();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), 202);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReport(int id)
	{
		try
		{
			CommonServices.ExportSalesContractExport501(id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FrmExportSalesContract_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnShowHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnResetHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefreshHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)DataGridHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Left)
			{
				if (((Control)(object)DataGridHistory).Focused)
				{
					((Control)(object)grddetailhistory).Focus();
				}
				else
				{
					((Control)(object)DataGridHistory).Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Expected O, but got Unknown
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || DataGridHistory.CurrentColumn == null)
			{
				return;
			}
			Conversion.ToInt(r.Cells["Id"].Value);
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			if (DataGridHistory.CurrentColumn.Key == "Print")
			{
				if (formrights.DoHavePrintRights)
				{
					GenerateReport(Conversion.ToInt(r.Cells["Id"].Value));
				}
			}
			else if (DataGridHistory.CurrentColumn.Key == "Print501A")
			{
				if (formrights.DoHavePrintRights)
				{
					CommonServices.SaleContractReportsSlipNew501A(Conversion.ToInt(r.Cells["Id"].Value));
				}
			}
			else if (DataGridHistory.CurrentColumn.Key == "PrintII")
			{
				if (formrights.DoHavePrintRights)
				{
					CommonServices.SaleContractReportsSlipNew501(Conversion.ToInt(r.Cells["Id"].Value));
				}
			}
			else if (DataGridHistory.CurrentColumn.Key == "Print523")
			{
				if (formrights.DoHavePrintRights)
				{
					DataTable dt = new DataTable();
					dt = ExportPdfReport.ExpRptSalesContractExportRegister_523(new ReportsParameters
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						ExImLcOrderId = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value)
					});
					if (dt == null || dt.Rows.Count == 0)
					{
						MessageBox.Show("Not Record Found For Display");
						return;
					}
					Reporting val = new Reporting();
					val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
					val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
					val.ShowReportWithDataTable(dt, "523-ExpRptSalesContractWiseInvoiceRegister.rpt");
				}
			}
			else if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), 202);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCutKey_Click(object sender, EventArgs e)
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

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F1", "For Open Delivery Term Popup");
			dt.Rows.Add("Ctrl+F2", "For Open Payterm Popup");
			dt.Rows.Add("Ctrl+F3", "For Open Sea Ports Popup");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+L", "For Load All Records");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowRight", "For Moving In Detail Grids");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On First Column in Detail for Entry");
			dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Update Record");
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataGridHistory).Name);
			if (fs != null)
			{
				DataGridHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnApprove_Click(object sender, EventArgs e)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		try
		{
			if (DataGridHistory.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select row's first");
			}
			if (MessageBox.Show("Are you sure to approve selected records?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList obj = new ApprovalList();
				obj.AllApprovalLists = new List<ApprovalList>();
				GridEXRow[] checkedRows = DataGridHistory.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					ApprovalList item = new ApprovalList();
					item.OrganizationId = UserAccount.OrganizationId;
					item.CompanyId = UserAccount.CompanyId;
					item.Id = Conversion.ToInt(r.Cells["Id"].Value);
					item.EntryUser = UserAccount.ID;
					obj.AllApprovalLists.Add(item);
				}
				ExImLcOrder.ExportSalesContractApproval(obj);
				MessageBox.Show("Record's Approve Successfully");
				HistoryFill();
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
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Expected O, but got Unknown
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Expected O, but got Unknown
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_0581: Expected O, but got Unknown
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grddetailhistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPaymentHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPMHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOtherItemHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOtherChargesHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.tabControl3 = new System.Windows.Forms.TabControl();
		this.tabDetailH = new System.Windows.Forms.TabPage();
		this.grddetailhistory = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label59 = new System.Windows.Forms.Label();
		this.tabPaymentDetailH = new System.Windows.Forms.TabPage();
		this.grdPaymentHistory = new GridEX();
		this.panel31 = new System.Windows.Forms.Panel();
		this.label74 = new System.Windows.Forms.Label();
		this.tabPmH = new System.Windows.Forms.TabPage();
		this.grdPMHistory = new GridEX();
		this.panel32 = new System.Windows.Forms.Panel();
		this.label75 = new System.Windows.Forms.Label();
		this.tabOtherItemH = new System.Windows.Forms.TabPage();
		this.grdOtherItemHistory = new GridEX();
		this.panel33 = new System.Windows.Forms.Panel();
		this.label76 = new System.Windows.Forms.Label();
		this.tabOtherChargesHistory = new System.Windows.Forms.TabPage();
		this.grdOtherChargesHistory = new GridEX();
		this.panel39 = new System.Windows.Forms.Panel();
		this.label111 = new System.Windows.Forms.Label();
		this.panel29 = new System.Windows.Forms.Panel();
		this.label65 = new System.Windows.Forms.Label();
		this.panel18 = new System.Windows.Forms.Panel();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.label67 = new System.Windows.Forms.Label();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.label73 = new System.Windows.Forms.Label();
		this.label89 = new System.Windows.Forms.Label();
		this.label90 = new System.Windows.Forms.Label();
		this.label91 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel17 = new System.Windows.Forms.Panel();
		this.label52 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.btnApprove = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.tabControl3.SuspendLayout();
		this.tabDetailH.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grddetailhistory).BeginInit();
		this.panel13.SuspendLayout();
		this.tabPaymentDetailH.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentHistory).BeginInit();
		this.panel31.SuspendLayout();
		this.tabPmH.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPMHistory).BeginInit();
		this.panel32.SuspendLayout();
		this.tabOtherItemH.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOtherItemHistory).BeginInit();
		this.panel33.SuspendLayout();
		this.tabOtherChargesHistory.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOtherChargesHistory).BeginInit();
		this.panel39.SuspendLayout();
		this.panel29.SuspendLayout();
		this.panel18.SuspendLayout();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel17.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel1.Controls.Add(this.panel29);
		this.panel1.Controls.Add(this.panel18);
		this.panel1.Controls.Add(this.tabControl3);
		this.panel1.Controls.Add(this.panel17);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1234, 590);
		this.panel1.TabIndex = 0;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.DataGridHistory).BackColor = System.Drawing.Color.White;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 149);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1234, 200);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 1;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.tabControl3.Controls.Add(this.tabDetailH);
		this.tabControl3.Controls.Add(this.tabPaymentDetailH);
		this.tabControl3.Controls.Add(this.tabPmH);
		this.tabControl3.Controls.Add(this.tabOtherItemH);
		this.tabControl3.Controls.Add(this.tabOtherChargesHistory);
		this.tabControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.tabControl3.Location = new System.Drawing.Point(0, 349);
		this.tabControl3.Name = "tabControl3";
		this.tabControl3.SelectedIndex = 0;
		this.tabControl3.Size = new System.Drawing.Size(1234, 241);
		this.tabControl3.TabIndex = 349;
		this.tabDetailH.Controls.Add((System.Windows.Forms.Control)(object)this.grddetailhistory);
		this.tabDetailH.Controls.Add(this.panel13);
		this.tabDetailH.Location = new System.Drawing.Point(4, 22);
		this.tabDetailH.Name = "tabDetailH";
		this.tabDetailH.Padding = new System.Windows.Forms.Padding(3);
		this.tabDetailH.Size = new System.Drawing.Size(1226, 215);
		this.tabDetailH.TabIndex = 0;
		this.tabDetailH.Text = "Detail";
		this.tabDetailH.UseVisualStyleBackColor = true;
		this.grddetailhistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grddetailhistory).BackColor = System.Drawing.Color.White;
		this.grddetailhistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grddetailhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grddetailhistory.DesignTimeLayout = grddetailhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grddetailhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grddetailhistory.FilterMode = (FilterMode)2;
		this.grddetailhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grddetailhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grddetailhistory.GroupByBoxVisible = false;
		this.grddetailhistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grddetailhistory).Location = new System.Drawing.Point(3, 28);
		((System.Windows.Forms.Control)(object)this.grddetailhistory).Name = "grddetailhistory";
		((ScrollableBase)this.grddetailhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grddetailhistory).Size = new System.Drawing.Size(1220, 184);
		((System.Windows.Forms.Control)(object)this.grddetailhistory).TabIndex = 2;
		this.grddetailhistory.TotalRow = (InheritableBoolean)1;
		this.grddetailhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grddetailhistory).VisualStyle = (VisualStyle)4;
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label59);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(3, 3);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1220, 25);
		this.panel13.TabIndex = 346;
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label59.ForeColor = System.Drawing.Color.White;
		this.label59.Location = new System.Drawing.Point(5, 5);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(228, 17);
		this.label59.TabIndex = 562;
		this.label59.Text = "Detail of the selected above row";
		this.tabPaymentDetailH.Controls.Add((System.Windows.Forms.Control)(object)this.grdPaymentHistory);
		this.tabPaymentDetailH.Controls.Add(this.panel31);
		this.tabPaymentDetailH.Location = new System.Drawing.Point(4, 22);
		this.tabPaymentDetailH.Name = "tabPaymentDetailH";
		this.tabPaymentDetailH.Padding = new System.Windows.Forms.Padding(3);
		this.tabPaymentDetailH.Size = new System.Drawing.Size(1226, 215);
		this.tabPaymentDetailH.TabIndex = 1;
		this.tabPaymentDetailH.Text = "Payment Detail";
		this.tabPaymentDetailH.UseVisualStyleBackColor = true;
		this.grdPaymentHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPaymentHistory).BackColor = System.Drawing.Color.White;
		this.grdPaymentHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPaymentHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPaymentHistory.DesignTimeLayout = grdPaymentHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPaymentHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPaymentHistory.FilterMode = (FilterMode)2;
		this.grdPaymentHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPaymentHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPaymentHistory.GroupByBoxVisible = false;
		this.grdPaymentHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdPaymentHistory).Location = new System.Drawing.Point(3, 28);
		((System.Windows.Forms.Control)(object)this.grdPaymentHistory).Name = "grdPaymentHistory";
		((ScrollableBase)this.grdPaymentHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPaymentHistory).Size = new System.Drawing.Size(1220, 184);
		((System.Windows.Forms.Control)(object)this.grdPaymentHistory).TabIndex = 347;
		this.grdPaymentHistory.TotalRow = (InheritableBoolean)1;
		this.grdPaymentHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPaymentHistory).VisualStyle = (VisualStyle)4;
		this.panel31.BackColor = System.Drawing.Color.Teal;
		this.panel31.Controls.Add(this.label74);
		this.panel31.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel31.Location = new System.Drawing.Point(3, 3);
		this.panel31.Name = "panel31";
		this.panel31.Size = new System.Drawing.Size(1220, 25);
		this.panel31.TabIndex = 348;
		this.label74.AutoSize = true;
		this.label74.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label74.ForeColor = System.Drawing.Color.White;
		this.label74.Location = new System.Drawing.Point(5, 5);
		this.label74.Name = "label74";
		this.label74.Size = new System.Drawing.Size(301, 17);
		this.label74.TabIndex = 562;
		this.label74.Text = "Payement Detail of the selected above row";
		this.tabPmH.Controls.Add((System.Windows.Forms.Control)(object)this.grdPMHistory);
		this.tabPmH.Controls.Add(this.panel32);
		this.tabPmH.Location = new System.Drawing.Point(4, 22);
		this.tabPmH.Name = "tabPmH";
		this.tabPmH.Size = new System.Drawing.Size(1226, 215);
		this.tabPmH.TabIndex = 2;
		this.tabPmH.Text = "Packing Material";
		this.tabPmH.UseVisualStyleBackColor = true;
		this.grdPMHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPMHistory).BackColor = System.Drawing.Color.White;
		this.grdPMHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPMHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPMHistory.DesignTimeLayout = grdPMHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPMHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPMHistory.FilterMode = (FilterMode)2;
		this.grdPMHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPMHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPMHistory.GroupByBoxVisible = false;
		this.grdPMHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdPMHistory).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdPMHistory).Name = "grdPMHistory";
		((ScrollableBase)this.grdPMHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPMHistory).Size = new System.Drawing.Size(1226, 190);
		((System.Windows.Forms.Control)(object)this.grdPMHistory).TabIndex = 347;
		this.grdPMHistory.TotalRow = (InheritableBoolean)1;
		this.grdPMHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPMHistory).VisualStyle = (VisualStyle)4;
		this.panel32.BackColor = System.Drawing.Color.Teal;
		this.panel32.Controls.Add(this.label75);
		this.panel32.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel32.Location = new System.Drawing.Point(0, 0);
		this.panel32.Name = "panel32";
		this.panel32.Size = new System.Drawing.Size(1226, 25);
		this.panel32.TabIndex = 348;
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label75.ForeColor = System.Drawing.Color.White;
		this.label75.Location = new System.Drawing.Point(5, 5);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(343, 17);
		this.label75.TabIndex = 562;
		this.label75.Text = "Packing Material Detail of the selected above row";
		this.tabOtherItemH.Controls.Add((System.Windows.Forms.Control)(object)this.grdOtherItemHistory);
		this.tabOtherItemH.Controls.Add(this.panel33);
		this.tabOtherItemH.Location = new System.Drawing.Point(4, 22);
		this.tabOtherItemH.Name = "tabOtherItemH";
		this.tabOtherItemH.Size = new System.Drawing.Size(1226, 215);
		this.tabOtherItemH.TabIndex = 3;
		this.tabOtherItemH.Text = "Other Item";
		this.tabOtherItemH.UseVisualStyleBackColor = true;
		this.grdOtherItemHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdOtherItemHistory).BackColor = System.Drawing.Color.White;
		this.grdOtherItemHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdOtherItemHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOtherItemHistory.DesignTimeLayout = grdOtherItemHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOtherItemHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdOtherItemHistory.FilterMode = (FilterMode)2;
		this.grdOtherItemHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdOtherItemHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOtherItemHistory.GroupByBoxVisible = false;
		this.grdOtherItemHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdOtherItemHistory).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdOtherItemHistory).Name = "grdOtherItemHistory";
		((ScrollableBase)this.grdOtherItemHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOtherItemHistory).Size = new System.Drawing.Size(1226, 190);
		((System.Windows.Forms.Control)(object)this.grdOtherItemHistory).TabIndex = 347;
		this.grdOtherItemHistory.TotalRow = (InheritableBoolean)1;
		this.grdOtherItemHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOtherItemHistory).VisualStyle = (VisualStyle)4;
		this.panel33.BackColor = System.Drawing.Color.Teal;
		this.panel33.Controls.Add(this.label76);
		this.panel33.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel33.Location = new System.Drawing.Point(0, 0);
		this.panel33.Name = "panel33";
		this.panel33.Size = new System.Drawing.Size(1226, 25);
		this.panel33.TabIndex = 348;
		this.label76.AutoSize = true;
		this.label76.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label76.ForeColor = System.Drawing.Color.White;
		this.label76.Location = new System.Drawing.Point(5, 5);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(310, 17);
		this.label76.TabIndex = 562;
		this.label76.Text = "Other Item Detail of the selected above row";
		this.tabOtherChargesHistory.Controls.Add((System.Windows.Forms.Control)(object)this.grdOtherChargesHistory);
		this.tabOtherChargesHistory.Controls.Add(this.panel39);
		this.tabOtherChargesHistory.Location = new System.Drawing.Point(4, 22);
		this.tabOtherChargesHistory.Name = "tabOtherChargesHistory";
		this.tabOtherChargesHistory.Size = new System.Drawing.Size(1226, 215);
		this.tabOtherChargesHistory.TabIndex = 4;
		this.tabOtherChargesHistory.Text = "Other Charges";
		this.tabOtherChargesHistory.UseVisualStyleBackColor = true;
		this.grdOtherChargesHistory.AllowEdit = (InheritableBoolean)2;
		grdOtherChargesHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOtherChargesHistory.DesignTimeLayout = grdOtherChargesHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOtherChargesHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdOtherChargesHistory.DynamicFiltering = true;
		this.grdOtherChargesHistory.FilterMode = (FilterMode)2;
		this.grdOtherChargesHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdOtherChargesHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOtherChargesHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOtherChargesHistory).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdOtherChargesHistory).Name = "grdOtherChargesHistory";
		((ScrollableBase)this.grdOtherChargesHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdOtherChargesHistory).Office2007CustomColor = System.Drawing.Color.LimeGreen;
		((ScrollableBase)this.grdOtherChargesHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOtherChargesHistory).Size = new System.Drawing.Size(1226, 190);
		((System.Windows.Forms.Control)(object)this.grdOtherChargesHistory).TabIndex = 352;
		this.grdOtherChargesHistory.TotalRow = (InheritableBoolean)1;
		this.grdOtherChargesHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOtherChargesHistory).VisualStyle = (VisualStyle)4;
		this.panel39.BackColor = System.Drawing.Color.Teal;
		this.panel39.Controls.Add(this.label111);
		this.panel39.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel39.Location = new System.Drawing.Point(0, 0);
		this.panel39.Name = "panel39";
		this.panel39.Size = new System.Drawing.Size(1226, 25);
		this.panel39.TabIndex = 351;
		this.label111.AutoSize = true;
		this.label111.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label111.ForeColor = System.Drawing.Color.White;
		this.label111.Location = new System.Drawing.Point(5, 5);
		this.label111.Name = "label111";
		this.label111.Size = new System.Drawing.Size(333, 17);
		this.label111.TabIndex = 562;
		this.label111.Text = "Other Charges Detail of the selected above row";
		this.panel29.BackColor = System.Drawing.Color.Teal;
		this.panel29.Controls.Add(this.label65);
		this.panel29.Controls.Add(this.ctrlGrdBar1);
		this.panel29.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel29.Location = new System.Drawing.Point(0, 125);
		this.panel29.Name = "panel29";
		this.panel29.Size = new System.Drawing.Size(1234, 24);
		this.panel29.TabIndex = 348;
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.ForeColor = System.Drawing.Color.White;
		this.label65.Location = new System.Drawing.Point(4, 2);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(142, 19);
		this.label65.TabIndex = 562;
		this.label65.Text = "Filtered Records";
		this.panel18.Controls.Add(this.groupBox5);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 53);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1234, 72);
		this.panel18.TabIndex = 0;
		this.groupBox5.Controls.Add(this.rdapproveddate);
		this.groupBox5.Controls.Add(this.btnShowHistory);
		this.groupBox5.Controls.Add(this.rdmodifydate);
		this.groupBox5.Controls.Add(this.label67);
		this.groupBox5.Controls.Add(this.drdocdate);
		this.groupBox5.Controls.Add(this.rdentrydate);
		this.groupBox5.Controls.Add(this.label73);
		this.groupBox5.Controls.Add(this.label89);
		this.groupBox5.Controls.Add(this.label90);
		this.groupBox5.Controls.Add(this.label91);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox5.Controls.Add(this.ToDocNoHistory);
		this.groupBox5.Controls.Add(this.FromDocNoHistory);
		this.groupBox5.Controls.Add(this.ToDateHistory);
		this.groupBox5.Controls.Add(this.FromDateHistory);
		this.groupBox5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox5.Location = new System.Drawing.Point(4, 0);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(923, 71);
		this.groupBox5.TabIndex = 0;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(816, 44);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdapproveddate.Visible = false;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(668, 40);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 5;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(816, 18);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label67.Location = new System.Drawing.Point(373, 20);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(58, 15);
		this.label67.TabIndex = 87;
		this.label67.Text = "Customer";
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(735, 18);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(735, 44);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(213, 46);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(64, 15);
		this.label73.TabIndex = 86;
		this.label73.Text = "To Doc No";
		this.label89.AutoSize = true;
		this.label89.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label89.Location = new System.Drawing.Point(213, 20);
		this.label89.Name = "label89";
		this.label89.Size = new System.Drawing.Size(79, 15);
		this.label89.TabIndex = 85;
		this.label89.Text = "From Doc No";
		this.label90.AutoSize = true;
		this.label90.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label90.Location = new System.Drawing.Point(3, 46);
		this.label90.Name = "label90";
		this.label90.Size = new System.Drawing.Size(48, 15);
		this.label90.TabIndex = 84;
		this.label90.Text = "To Date";
		this.label91.AutoSize = true;
		this.label91.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label91.Location = new System.Drawing.Point(3, 20);
		this.label91.Name = "label91";
		this.label91.Size = new System.Drawing.Size(63, 15);
		this.label91.TabIndex = 83;
		this.label91.Text = "From Date";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance62).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance63).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance63).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance63).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(434, 14);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(296, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 4;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(294, 42);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(294, 16);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(69, 42);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(142, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(69, 16);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(142, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.toolStrip3);
		this.panel17.Controls.Add(this.label52);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(0, 0);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1234, 53);
		this.panel17.TabIndex = 0;
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.ForeColor = System.Drawing.Color.White;
		this.label52.Location = new System.Drawing.Point(5, 30);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(249, 19);
		this.label52.TabIndex = 562;
		this.label52.Text = "Export Sales Contract History";
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnResetHistory, this.btnRefreshHistory, this.btnApprove });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
		this.toolStrip3.Size = new System.Drawing.Size(1234, 27);
		this.toolStrip3.TabIndex = 7;
		this.toolStrip3.Text = "toolStrip3";
		this.btnResetHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(59, 24);
		this.btnResetHistory.Text = "&New";
		this.btnResetHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1192, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar1.TabIndex = 8;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.btnApprove.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApprove.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.btnApprove.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnApprove.Name = "btnApprove";
		this.btnApprove.Size = new System.Drawing.Size(85, 24);
		this.btnApprove.Text = "Approve";
		this.btnApprove.Click += new System.EventHandler(btnApprove_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1234, 590);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmExportSalesContractApproval";
		this.Text = "Export Contract";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(FrmExportSalesContract_FormClosing);
		base.Load += new System.EventHandler(FrmExportSalesContract_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(FrmExportSalesContract_KeyDown);
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.tabControl3.ResumeLayout(false);
		this.tabDetailH.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grddetailhistory).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.tabPaymentDetailH.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPaymentHistory).EndInit();
		this.panel31.ResumeLayout(false);
		this.panel31.PerformLayout();
		this.tabPmH.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPMHistory).EndInit();
		this.panel32.ResumeLayout(false);
		this.panel32.PerformLayout();
		this.tabOtherItemH.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOtherItemHistory).EndInit();
		this.panel33.ResumeLayout(false);
		this.panel33.PerformLayout();
		this.tabOtherChargesHistory.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOtherChargesHistory).EndInit();
		this.panel39.ResumeLayout(false);
		this.panel39.PerformLayout();
		this.panel29.ResumeLayout(false);
		this.panel29.PerformLayout();
		this.panel18.ResumeLayout(false);
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
