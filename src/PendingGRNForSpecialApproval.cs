using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Architecture.WinApp.SpecialApprovalDashboard.UserControlCards;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.ApprovalDashboard
{

public class PendingGRNForSpecialApproval : Form
{
	public class ApproveRecord
	{
		public int Id { get; set; }

		public double RateCut { get; set; }
	}

	public int BranchesId;

	public int DocumentTypeId;

	public int SpecialApprovalGrn_ReasonId;

	private List<ApproveRecord> ListForApprove = new List<ApproveRecord>();

	private DataTable dtHistory = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private CheckBox ChkPrint;

	private Button btnApproved;

	private Button btnshow;

	internal UltraCombo cmbItem;

	internal UltraCombo CmbSupplier;

	private Panel panel3;

	private Label label23;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private GroupBox groupBox1;

	internal UltraCombo cmbperemeter;

	private Label label12;

	private Label label11;

	private Label label10;

	private Label label3;

	private TextBox txtVehicleNo;

	private DateTimePicker GRNfromdate;

	private Label label5;

	internal UltraCombo cmbParentCategory;

	private Label label2;

	private Label label27;

	private TextBox txtGrnNoFrom;

	private Label label8;

	private Label label7;

	private Label label4;

	private Label label9;

	internal UltraCombo CmbWarehouseName;

	private TextBox txtGrnNoTo;

	internal UltraCombo CmbCityName;

	internal UltraCombo CmbJobLotName;

	private Label label13;

	private DateTimePicker GRNToDate;

	private TextBox GpsNoFrom;

	private TextBox GpsNoTo;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnShortCutKeys;

	private FlowLayoutPanel flowLayoutPanel;

	private ToolStripButton btnApprove;

	private ToolStripLabel ApprovedCount;

	private Label label1;

	private TextBox txtApprovalRemarks;

	public PendingGRNForSpecialApproval(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmPurchaseInvoiceHistory_Load(object sender, EventArgs e)
	{
		try
		{
			UserAccount.BranchesId = ((BranchesId > 0) ? BranchesId : UserAccount.BranchesId);
			gridHisoryDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ParameterFill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			DDL.BindDDL(dt, cmbperemeter, "Id", "Parameters", "Parameters", ZeroIndex: false);
			((UltraGridBase)cmbperemeter).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllDropDownBind()
	{
		try
		{
			DataTable dtAll = InvGrn.GetDataForDropDownFromGrn(UserAccount.OrganizationId, UserAccount.CompanyId, (string)null, (string)null, BranchesId.ToString(), 0);
			if (dtAll.Rows.Count > 0)
			{
				EnumerableRowCollection<DataRow> rows = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "Supplier"
					select r;
				DataTable dtParty = (rows.Any() ? rows.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsitem = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "Item"
					select r;
				DataTable dtItem = (rowsitem.Any() ? rowsitem.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsParentCategory = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "ParentCategory"
					select r;
				DataTable dtParentCategory = (rowsParentCategory.Any() ? rowsParentCategory.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowswh = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "Warehouse"
					select r;
				DataTable dtWh = (rowswh.Any() ? rowswh.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsJob = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "JobLot"
					select r;
				DataTable dtJob = (rowsJob.Any() ? rowsJob.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsCity = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "City"
					select r;
				DataTable dtCity = (rowsCity.Any() ? rowsCity.CopyToDataTable() : dtAll.Clone());
				DDL.BindDDLNew(dtParty, CmbSupplier, "Id", "ReferenceName", "Supplier Name", true);
				((UltraGridBase)CmbSupplier).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dtItem, cmbItem, "Id", "ReferenceName", "Item Name", true);
				((UltraGridBase)cmbItem).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dtParentCategory, cmbParentCategory, "Id", "ReferenceName", "Parent Category", true);
				((UltraGridBase)cmbParentCategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dtWh, CmbWarehouseName, "Id", "ReferenceName", "WareHouse", true);
				((UltraGridBase)CmbWarehouseName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dtJob, CmbJobLotName, "Id", "ReferenceName", "JobLot", true);
				((UltraGridBase)CmbJobLotName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dtCity, CmbCityName, "Id", "ReferenceName", "CityName", true);
				((UltraGridBase)CmbCityName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbperemeter_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbperemeter.Value) == 1)
			{
				GRNfromdate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 2)
			{
				GRNfromdate.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				GRNfromdate.Value = first;
				GRNToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				GRNfromdate.Value = first2;
				GRNToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 5)
			{
				GRNfromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void Reset()
	{
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			AllDropDownBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			gridHisoryDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void gridHisoryDetail()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		try
		{
			dtHistory.Clear();
			flowLayoutPanel.Controls.Clear();
			ReportsParameters grn = new ReportsParameters();
			grn.OrganizationId = UserAccount.OrganizationId;
			grn.CompanyId = UserAccount.CompanyId;
			grn.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			grn.FromDate = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			grn.ToDate = Conversion.ToDateTime((object)DateTime.Now);
			grn.DocumentTypeId = 46;
			grn.ApprovedFilter = "All";
			grn.ReasonId = SpecialApprovalGrn_ReasonId;
			grn.BranchesIds = BranchesId.ToString();
			dtHistory = InvGrn.InvGrnDataForSpecialApproval(grn);
			if (dtHistory.Rows.Count <= 0)
			{
				return;
			}
			if (SpecialApprovalGrn_ReasonId == 1)
			{
				SpecialApprovalGrn_Reason1[] reason5 = new SpecialApprovalGrn_Reason1[dtHistory.Rows.Count];
				for (int m = 0; m < reason5.Length; m++)
				{
					reason5[m] = new SpecialApprovalGrn_Reason1();
					reason5[m].txtReason.Text = Conversion.ToString(dtHistory.Rows[m]["ReasonRemarks"]);
					reason5[m].txtGrnNo.Text = Conversion.ToString(dtHistory.Rows[m]["GrnNo"]);
					reason5[m].txtGrnNo.Tag = Conversion.ToString(dtHistory.Rows[m]["Id"]);
					reason5[m].datGrnDate.Text = Conversion.ToDateTime(dtHistory.Rows[m]["GrnDate"]).ToString("dd-MMM-yyyy");
					reason5[m].txtVehicleNo.Text = Conversion.ToString(dtHistory.Rows[m]["VehicleNo"]);
					reason5[m].txtGpNo.Tag = Conversion.ToString(dtHistory.Rows[m]["InwardGatePassId"]);
					reason5[m].txtGpNo.Text = Conversion.ToString(dtHistory.Rows[m]["GpNo"]);
					reason5[m].datGpDate.Text = Conversion.ToDateTime(dtHistory.Rows[m]["GpDate"]).ToString("dd-MMM-yyyy");
					reason5[m].txtPoNo.Tag = Conversion.ToString(dtHistory.Rows[m]["PurchaseOrderId"]);
					reason5[m].txtPoNo.Text = Conversion.ToString(dtHistory.Rows[m]["OrderNo"]);
					reason5[m].datPoDat.Text = Conversion.ToDateTime(dtHistory.Rows[m]["OrderDate"]).ToString("dd-MMM-yyyy");
					reason5[m].txtCityName.Text = Conversion.ToString(dtHistory.Rows[m]["AreaCity"]);
					reason5[m].txtSupplierName.Text = Conversion.ToString(dtHistory.Rows[m]["SupplierName"]);
					reason5[m].txtSupplierName.Tag = Conversion.ToString(dtHistory.Rows[m]["SupplierCustomerId"]);
					reason5[m].txtSuppCellNo.Text = Conversion.ToString(dtHistory.Rows[m]["MobilePersonal"]);
					reason5[m].txtSuppWhatsAppNo.Text = Conversion.ToString(dtHistory.Rows[m]["WhatsAppNo"]);
					reason5[m].txtDeliveryTerm.Text = Conversion.ToString(dtHistory.Rows[m]["DeliveryTerm"]);
					reason5[m].txtItemName.Text = Conversion.ToString(dtHistory.Rows[m]["ItemName"]);
					reason5[m].txtItemName.Tag = Conversion.ToString(dtHistory.Rows[m]["ItemId"]);
					reason5[m].txtOrderBal.Text = Conversion.ToDouble(dtHistory.Rows[m]["OrderBalWeight"]).ToString("#,##0.###");
					reason5[m].txtItemGrnWeight.Text = Conversion.ToDouble(dtHistory.Rows[m]["NetBillWeight"]).ToString("#,##0.###");
					reason5[m].txtExcessWeight.Text = Conversion.ToDouble(dtHistory.Rows[m]["AccessWeight"]).ToString("#,##0.###");
					flowLayoutPanel.Controls.Add(reason5[m]);
					reason5[m].UserControlClicked += reason1Card_Click;
				}
			}
			else if (SpecialApprovalGrn_ReasonId == 2)
			{
				SpecialApprovalGrn_Reason2[] reason3 = new SpecialApprovalGrn_Reason2[dtHistory.Rows.Count];
				for (int l = 0; l < reason3.Length; l++)
				{
					reason3[l] = new SpecialApprovalGrn_Reason2();
					reason3[l].txtReason.Text = Conversion.ToString(dtHistory.Rows[l]["ReasonRemarks"]);
					reason3[l].txtGrnNo.Text = Conversion.ToString(dtHistory.Rows[l]["GrnNo"]);
					reason3[l].txtGrnNo.Tag = Conversion.ToString(dtHistory.Rows[l]["Id"]);
					reason3[l].datGrnDate.Text = Conversion.ToDateTime(dtHistory.Rows[l]["GrnDate"]).ToString("dd-MMM-yyyy");
					reason3[l].txtVehicleNo.Text = Conversion.ToString(dtHistory.Rows[l]["VehicleNo"]);
					reason3[l].txtGpNo.Tag = Conversion.ToString(dtHistory.Rows[l]["InwardGatePassId"]);
					reason3[l].txtGpNo.Text = Conversion.ToString(dtHistory.Rows[l]["GpNo"]);
					reason3[l].datGpDate.Text = Conversion.ToDateTime(dtHistory.Rows[l]["GpDate"]).ToString("dd-MMM-yyyy");
					reason3[l].txtPoNo.Tag = Conversion.ToString(dtHistory.Rows[l]["PurchaseOrderId"]);
					reason3[l].txtPoNo.Text = Conversion.ToString(dtHistory.Rows[l]["OrderNo"]);
					reason3[l].datPoDat.Text = Conversion.ToDateTime(dtHistory.Rows[l]["OrderDate"]).ToString("dd-MMM-yyyy");
					reason3[l].txtSupplierName.Text = Conversion.ToString(dtHistory.Rows[l]["SupplierName"]);
					reason3[l].txtSupplierName.Tag = Conversion.ToString(dtHistory.Rows[l]["SupplierCustomerId"]);
					reason3[l].txtSuppCellNo.Text = Conversion.ToString(dtHistory.Rows[l]["MobilePersonal"]);
					reason3[l].txtSuppWhatsAppNo.Text = Conversion.ToString(dtHistory.Rows[l]["WhatsAppNo"]);
					reason3[l].txtDeliveryTerm.Text = Conversion.ToString(dtHistory.Rows[l]["DeliveryTerm"]);
					reason3[l].txtItemName.Text = Conversion.ToString(dtHistory.Rows[l]["ItemName"]);
					reason3[l].txtItemName.Tag = Conversion.ToString(dtHistory.Rows[l]["ItemId"]);
					reason3[l].txtSupplierWeight.Text = Conversion.ToDouble(dtHistory.Rows[l]["PartyWeight"]).ToString("#,##0.###");
					reason3[l].txtFactoryWeight.Text = Conversion.ToDouble(dtHistory.Rows[l]["FactoryWeight"]).ToString("#,##0.###");
					reason3[l].txtQuantity.Text = Conversion.ToDouble(dtHistory.Rows[l]["ItemQty"]).ToString("#,##0.###");
					reason3[l].txtGrossWeight.Text = Conversion.ToDouble(dtHistory.Rows[l]["GrossWeight"]).ToString("#,##0.###");
					reason3[l].txtEbTotal.Text = Conversion.ToDouble(dtHistory.Rows[l]["EBWTotal"]).ToString("#,##0.###");
					reason3[l].txtWtCutTotal.Text = Conversion.ToDouble(dtHistory.Rows[l]["WtCutTotal"]).ToString("#,##0.###");
					reason3[l].txtAddLessWeight.Text = Conversion.ToDouble(dtHistory.Rows[l]["AdLsWeight"]).ToString("#,##0.###");
					reason3[l].txtBillWeight.Text = Conversion.ToDouble(dtHistory.Rows[l]["NetBillWeight"]).ToString("#,##0.###");
					reason3[l].txtStockWeight.Text = Conversion.ToDouble(dtHistory.Rows[l]["NetStockWeight"]).ToString("#,##0.###");
					reason3[l].txtExcessWeight.Text = (Conversion.ToDouble(dtHistory.Rows[l]["NetBillWeight"]) - Conversion.ToDouble(dtHistory.Rows[l]["NetStockWeight"])).ToString("#,##0.###") + " KG";
					flowLayoutPanel.Controls.Add(reason3[l]);
					reason3[l].UserControlClicked += reason2Card_Click;
				}
			}
			else if (SpecialApprovalGrn_ReasonId == 3)
			{
				SpecialApprovalGrn_Reason3[] reason2 = new SpecialApprovalGrn_Reason3[dtHistory.Rows.Count];
				for (int j = 0; j < reason2.Length; j++)
				{
					reason2[j] = new SpecialApprovalGrn_Reason3();
					reason2[j].txtReason.Text = Conversion.ToString(dtHistory.Rows[j]["ReasonRemarks"]);
					reason2[j].txtGrnNo.Text = Conversion.ToString(dtHistory.Rows[j]["GrnNo"]);
					reason2[j].txtGrnNo.Tag = Conversion.ToString(dtHistory.Rows[j]["Id"]);
					reason2[j].datGrnDate.Text = Conversion.ToDateTime(dtHistory.Rows[j]["GrnDate"]).ToString("dd-MMM-yyyy");
					reason2[j].txtVehicleNo.Text = Conversion.ToString(dtHistory.Rows[j]["VehicleNo"]);
					reason2[j].txtGpNo.Tag = Conversion.ToString(dtHistory.Rows[j]["InwardGatePassId"]);
					reason2[j].txtGpNo.Text = Conversion.ToString(dtHistory.Rows[j]["GpNo"]);
					reason2[j].datGpDate.Text = Conversion.ToDateTime(dtHistory.Rows[j]["GpDate"]).ToString("dd-MMM-yyyy");
					reason2[j].txtPoNo.Tag = Conversion.ToString(dtHistory.Rows[j]["PurchaseOrderId"]);
					reason2[j].txtPoNo.Text = Conversion.ToString(dtHistory.Rows[j]["OrderNo"]);
					reason2[j].datPoDat.Text = Conversion.ToDateTime(dtHistory.Rows[j]["OrderDate"]).ToString("dd-MMM-yyyy");
					reason2[j].txtCityName.Text = Conversion.ToString(dtHistory.Rows[j]["AreaCity"]);
					reason2[j].txtSupplierName.Text = Conversion.ToString(dtHistory.Rows[j]["SupplierName"]);
					reason2[j].txtSupplierName.Tag = Conversion.ToString(dtHistory.Rows[j]["SupplierCustomerId"]);
					reason2[j].txtSuppCellNo.Text = Conversion.ToString(dtHistory.Rows[j]["MobilePersonal"]);
					reason2[j].txtSuppWhatsAppNo.Text = Conversion.ToString(dtHistory.Rows[j]["WhatsAppNo"]);
					reason2[j].txtDeliveryTerm.Text = Conversion.ToString(dtHistory.Rows[j]["DeliveryTerm"]);
					reason2[j].txtItemName.Text = Conversion.ToString(dtHistory.Rows[j]["ItemName"]);
					reason2[j].txtItemName.Tag = Conversion.ToString(dtHistory.Rows[j]["ItemId"]);
					reason2[j].txtEmptyBagTerm.Text = Conversion.ToString(dtHistory.Rows[j]["EmptyBagTerm"]);
					reason2[j].txtEbWeightCut.Text = Conversion.ToDouble(dtHistory.Rows[j]["EBWTotal"]).ToString("#,##0.###");
					flowLayoutPanel.Controls.Add(reason2[j]);
					reason2[j].UserControlClicked += reason3Card_Click;
				}
			}
			else if (SpecialApprovalGrn_ReasonId == 4)
			{
				SpecialApprovalGrn_Reason4[] reason1 = new SpecialApprovalGrn_Reason4[dtHistory.Rows.Count];
				for (int i = 0; i < reason1.Length; i++)
				{
					reason1[i] = new SpecialApprovalGrn_Reason4();
					reason1[i].txtReason.Text = Conversion.ToString(dtHistory.Rows[i]["ReasonRemarks"]);
					reason1[i].txtGrnNo.Text = Conversion.ToString(dtHistory.Rows[i]["GrnNo"]);
					reason1[i].txtGrnNo.Tag = Conversion.ToString(dtHistory.Rows[i]["Id"]);
					reason1[i].datGrnDate.Text = Conversion.ToDateTime(dtHistory.Rows[i]["GrnDate"]).ToString("dd-MMM-yyyy");
					reason1[i].txtVehicleNo.Text = Conversion.ToString(dtHistory.Rows[i]["VehicleNo"]);
					reason1[i].txtGpNo.Tag = Conversion.ToString(dtHistory.Rows[i]["InwardGatePassId"]);
					reason1[i].txtGpNo.Text = Conversion.ToString(dtHistory.Rows[i]["GpNo"]);
					reason1[i].datGpDate.Text = Conversion.ToDateTime(dtHistory.Rows[i]["GpDate"]).ToString("dd-MMM-yyyy");
					reason1[i].txtPoNo.Tag = Conversion.ToString(dtHistory.Rows[i]["PurchaseOrderId"]);
					reason1[i].txtPoNo.Text = Conversion.ToString(dtHistory.Rows[i]["OrderNo"]);
					reason1[i].datPoDat.Text = Conversion.ToDateTime(dtHistory.Rows[i]["OrderDate"]).ToString("dd-MMM-yyyy");
					reason1[i].txtCityName.Text = Conversion.ToString(dtHistory.Rows[i]["AreaCity"]);
					reason1[i].txtSupplierName.Text = Conversion.ToString(dtHistory.Rows[i]["SupplierName"]);
					reason1[i].txtSupplierName.Tag = Conversion.ToString(dtHistory.Rows[i]["SupplierCustomerId"]);
					reason1[i].txtSuppCellNo.Text = Conversion.ToString(dtHistory.Rows[i]["MobilePersonal"]);
					reason1[i].txtSuppWhatsAppNo.Text = Conversion.ToString(dtHistory.Rows[i]["WhatsAppNo"]);
					reason1[i].txtDeliveryTerm.Text = Conversion.ToString(dtHistory.Rows[i]["DeliveryTerm"]);
					reason1[i].txtItemName.Text = Conversion.ToString(dtHistory.Rows[i]["ItemName"]);
					reason1[i].txtItemName.Tag = Conversion.ToString(dtHistory.Rows[i]["ItemId"]);
					reason1[i].txtSupplierWeight.Text = Conversion.ToDouble(dtHistory.Rows[i]["PartyWeight"]).ToString("#,##0.###");
					reason1[i].txtFactoryWeight.Text = Conversion.ToDouble(dtHistory.Rows[i]["Factoryweight"]).ToString("#,##0.###");
					reason1[i].txtExcessWeight.Text = Math.Abs(Conversion.ToDouble(dtHistory.Rows[i]["DifferenceWeight"])).ToString("#,##0.###") + " KG";
					reason1[i].txtExcessWeightCaption.Text = ((Conversion.ToDouble(dtHistory.Rows[i]["DifferenceWeight"]) > 0.0) ? "Shortage Weight" : "Excess Weight");
					flowLayoutPanel.Controls.Add(reason1[i]);
					reason1[i].UserControlClicked += reason4Card_Click;
				}
			}
			if (SpecialApprovalGrn_ReasonId == 5)
			{
				SpecialApprovalGrn_Reason5[] reason4 = new SpecialApprovalGrn_Reason5[dtHistory.Rows.Count];
				for (int k = 0; k < reason4.Length; k++)
				{
					reason4[k] = new SpecialApprovalGrn_Reason5();
					reason4[k].txtReason.Text = Conversion.ToString(dtHistory.Rows[k]["ReasonRemarks"]);
					reason4[k].txtGrnNo.Text = Conversion.ToString(dtHistory.Rows[k]["GrnNo"]);
					reason4[k].txtGrnNo.Tag = Conversion.ToString(dtHistory.Rows[k]["Id"]);
					reason4[k].datGrnDate.Text = Conversion.ToDateTime(dtHistory.Rows[k]["GrnDate"]).ToString("dd-MMM-yyyy");
					reason4[k].txtVehicleNo.Text = Conversion.ToString(dtHistory.Rows[k]["VehicleNo"]);
					reason4[k].txtGpNo.Tag = Conversion.ToString(dtHistory.Rows[k]["InwardGatePassId"]);
					reason4[k].txtGpNo.Text = Conversion.ToString(dtHistory.Rows[k]["GpNo"]);
					reason4[k].datGpDate.Text = Conversion.ToDateTime(dtHistory.Rows[k]["GpDate"]).ToString("dd-MMM-yyyy");
					reason4[k].txtPoNo.Tag = Conversion.ToString(dtHistory.Rows[k]["PurchaseOrderId"]);
					reason4[k].txtPoNo.Text = Conversion.ToString(dtHistory.Rows[k]["OrderNo"]);
					reason4[k].datPoDat.Text = Conversion.ToDateTime(dtHistory.Rows[k]["OrderDate"]).ToString("dd-MMM-yyyy");
					reason4[k].txtCityName.Text = Conversion.ToString(dtHistory.Rows[k]["AreaCity"]);
					reason4[k].txtSupplierName.Text = Conversion.ToString(dtHistory.Rows[k]["SupplierName"]);
					reason4[k].txtSupplierName.Tag = Conversion.ToString(dtHistory.Rows[k]["SupplierCustomerId"]);
					reason4[k].txtSuppCellNo.Text = Conversion.ToString(dtHistory.Rows[k]["MobilePersonal"]);
					reason4[k].txtSuppWhatsAppNo.Text = Conversion.ToString(dtHistory.Rows[k]["WhatsAppNo"]);
					reason4[k].txtDeliveryTerm.Text = Conversion.ToString(dtHistory.Rows[k]["DeliveryTerm"]);
					reason4[k].txtItemName.Text = Conversion.ToString(dtHistory.Rows[k]["ItemName"]);
					reason4[k].txtItemName.Tag = Conversion.ToString(dtHistory.Rows[k]["ItemId"]);
					reason4[k].txtOrderBal.Text = Conversion.ToDouble(dtHistory.Rows[k]["OrderBalWeight"]).ToString("#,##0.###");
					reason4[k].txtItemGrnWeight.Text = Conversion.ToDouble(dtHistory.Rows[k]["NetBillWeight"]).ToString("#,##0.###");
					reason4[k].txtExcessWeight.Text = Conversion.ToDouble(dtHistory.Rows[k]["AccessWeight"]).ToString("#,##0.###");
					flowLayoutPanel.Controls.Add(reason4[k]);
					reason4[k].UserControlClicked += reason5Card_Click;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void reason1Card_Click(object sender, EventArgs e)
	{
		try
		{
			SpecialApprovalGrn_Reason1 obj = (SpecialApprovalGrn_Reason1)sender;
			if (Conversion.ToString(obj.Tag) == "FromCheckBox")
			{
				int tagId = Conversion.ToInt(obj.txtGrnNo.Tag);
				ApproveRecord record = ListForApprove.FirstOrDefault((ApproveRecord x) => x.Id == tagId);
				if (!obj.IsSelected)
				{
					if (record != null)
					{
						ListForApprove.Remove(record);
					}
				}
				else if (record == null)
				{
					ListForApprove.Add(new ApproveRecord
					{
						Id = tagId,
						RateCut = 0.0
					});
				}
				ApprovedCount.Text = ListForApprove.Count.ToString();
			}
			else if (Conversion.ToString(obj.Tag) == "ApproveBtnClicked")
			{
				int Id = Conversion.ToInt(obj.txtGrnNo.Tag);
				ApprovedRecord(Id, "AP", txtApprovalRemarks.Text, 0.0);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void reason2Card_Click(object sender, EventArgs e)
	{
		try
		{
			SpecialApprovalGrn_Reason2 obj = (SpecialApprovalGrn_Reason2)sender;
			if (Conversion.ToString(obj.Tag) == "FromCheckBox")
			{
				int tagId = Conversion.ToInt(obj.txtGrnNo.Tag);
				ApproveRecord record = ListForApprove.FirstOrDefault((ApproveRecord x) => x.Id == tagId);
				if (!obj.IsSelected)
				{
					if (record != null)
					{
						ListForApprove.Remove(record);
					}
				}
				else if (record == null)
				{
					ListForApprove.Add(new ApproveRecord
					{
						Id = tagId,
						RateCut = 0.0
					});
				}
				ApprovedCount.Text = ListForApprove.Count.ToString();
			}
			else if (Conversion.ToString(obj.Tag) == "ApproveBtnClicked")
			{
				int Id = Conversion.ToInt(obj.txtGrnNo.Tag);
				ApprovedRecord(Id, "AP", txtApprovalRemarks.Text, 0.0);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void reason3Card_Click(object sender, EventArgs e)
	{
		try
		{
			SpecialApprovalGrn_Reason3 obj = (SpecialApprovalGrn_Reason3)sender;
			if (Conversion.ToString(obj.Tag) == "FromCheckBox")
			{
				int tagId = Conversion.ToInt(obj.txtGrnNo.Tag);
				ApproveRecord record = ListForApprove.FirstOrDefault((ApproveRecord x) => x.Id == tagId);
				if (!obj.IsSelected)
				{
					if (record != null)
					{
						ListForApprove.Remove(record);
					}
				}
				else if (record == null)
				{
					ListForApprove.Add(new ApproveRecord
					{
						Id = tagId,
						RateCut = 0.0
					});
				}
				ApprovedCount.Text = ListForApprove.Count.ToString();
			}
			else if (Conversion.ToString(obj.Tag) == "ApproveBtnClicked")
			{
				int Id = Conversion.ToInt(obj.txtGrnNo.Tag);
				ApprovedRecord(Id, "AP", txtApprovalRemarks.Text, 0.0);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void reason4Card_Click(object sender, EventArgs e)
	{
		try
		{
			SpecialApprovalGrn_Reason4 obj = (SpecialApprovalGrn_Reason4)sender;
			if (Conversion.ToString(obj.Tag) == "FromCheckBox")
			{
				int tagId = Conversion.ToInt(obj.txtGrnNo.Tag);
				ApproveRecord record = ListForApprove.FirstOrDefault((ApproveRecord x) => x.Id == tagId);
				if (!obj.IsSelected)
				{
					if (record != null)
					{
						ListForApprove.Remove(record);
					}
				}
				else if (record == null)
				{
					ListForApprove.Add(new ApproveRecord
					{
						Id = tagId,
						RateCut = 0.0
					});
				}
				ApprovedCount.Text = ListForApprove.Count.ToString();
			}
			else if (Conversion.ToString(obj.Tag) == "ApproveBtnClicked")
			{
				int Id = Conversion.ToInt(obj.txtGrnNo.Tag);
				ApprovedRecord(Id, "AP", txtApprovalRemarks.Text, 0.0);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void reason5Card_Click(object sender, EventArgs e)
	{
		try
		{
			SpecialApprovalGrn_Reason5 obj = (SpecialApprovalGrn_Reason5)sender;
			if (Conversion.ToString(obj.Tag) == "FromCheckBox")
			{
				int tagId = Conversion.ToInt(obj.txtGrnNo.Tag);
				ApproveRecord record = ListForApprove.FirstOrDefault((ApproveRecord x) => x.Id == tagId);
				if (!obj.IsSelected)
				{
					if (record != null)
					{
						ListForApprove.Remove(record);
					}
				}
				else if (record == null)
				{
					ListForApprove.Add(new ApproveRecord
					{
						Id = tagId,
						RateCut = 0.0
					});
				}
				ApprovedCount.Text = ListForApprove.Count.ToString();
			}
			else if (Conversion.ToString(obj.Tag) == "ApproveBtnClicked")
			{
				int Id = Conversion.ToInt(obj.txtGrnNo.Tag);
				double rateCut = Conversion.ToDouble((object)obj.txtRateCut.Text);
				ApprovedRecord(Id, "AP", txtApprovalRemarks.Text, rateCut);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ApprovedRecord(int RecId, string ReqType, string Remarks, double RateCut)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Expected O, but got Unknown
		try
		{
			if (Remarks == string.Empty)
			{
				txtApprovalRemarks.Focus();
				MessageBox.Show("Approval Remarks Required: ");
				return;
			}
			if (ReqType == "AP")
			{
				if (MessageBox.Show("Are you sure to Approve GRN?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to UnApprove GRN?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			ApprovalList val = new ApprovalList
			{
				AllApprovalLists = new List<ApprovalList>()
			};
			ApprovalList item = new ApprovalList();
			item.OrganizationId = UserAccount.OrganizationId;
			item.CompanyId = UserAccount.CompanyId;
			item.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			item.DocumentTypeId = DocumentTypeId;
			item.Id = RecId;
			item.EntryUser = UserAccount.ID;
			item.ReqType = ReqType;
			item.Rate = RateCut;
			item.ReasonId = SpecialApprovalGrn_ReasonId;
			item.Comments = Remarks;
			val.AllApprovalLists.Add(item);
			InvGrn.UpdateApprovalStatus(val);
			if (ReqType == "AP")
			{
				MessageBox.Show("Record Approved Successfully");
			}
			else
			{
				MessageBox.Show("Record UnApproved Successfully");
			}
			gridHisoryDetail();
			txtApprovalRemarks.Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnApproved_Click(object sender, EventArgs e)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		try
		{
			if (ListForApprove.Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (MessageBox.Show("Are you sure to Approve GRN?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			ApprovalList obj = new ApprovalList();
			obj.AllApprovalLists = new List<ApprovalList>();
			foreach (ApproveRecord r in ListForApprove)
			{
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				item.DocumentTypeId = DocumentTypeId;
				item.Id = r.Id;
				item.Rate = r.RateCut;
				item.ReasonId = SpecialApprovalGrn_ReasonId;
				item.EntryUser = UserAccount.ID;
				item.ReqType = "AP";
				if (txtApprovalRemarks.Text == string.Empty)
				{
					txtApprovalRemarks.Focus();
					MessageBox.Show("ApprovalRemarks Required");
					return;
				}
				item.Comments = txtApprovalRemarks.Text;
				obj.AllApprovalLists.Add(item);
			}
			InvGrn.UpdateApprovalStatus(obj);
			MessageBox.Show("Record Approved Successfully");
			gridHisoryDetail();
			txtApprovalRemarks.Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void frmPurchaseInvoiceHistory_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				GRNfromdate.Focus();
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.A)
			{
				btnApproved_Click(null, null);
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 40;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnShortCutKeys_Click(object sender, EventArgs e)
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
			dt.Rows.Add("Ctrl+S", "For Show Record");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F5", "For Focus on Combo Date");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid History");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Date combo in Filters Box");
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

	private void GpsNoTo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void GpsNoFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtGrnNoFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtGrnNoTo_KeyPress(object sender, KeyPressEventArgs e)
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
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected O, but got Unknown
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Expected O, but got Unknown
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Expected O, but got Unknown
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Expected O, but got Unknown
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Expected O, but got Unknown
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Expected O, but got Unknown
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Expected O, but got Unknown
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Expected O, but got Unknown
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Expected O, but got Unknown
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Expected O, but got Unknown
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Expected O, but got Unknown
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Expected O, but got Unknown
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Expected O, but got Unknown
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Expected O, but got Unknown
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Expected O, but got Unknown
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Expected O, but got Unknown
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Expected O, but got Unknown
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Expected O, but got Unknown
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Expected O, but got Unknown
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Expected O, but got Unknown
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Expected O, but got Unknown
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.cmbperemeter = new UltraCombo();
		this.btnApproved = new System.Windows.Forms.Button();
		this.btnshow = new System.Windows.Forms.Button();
		this.ChkPrint = new System.Windows.Forms.CheckBox();
		this.label12 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.cmbItem = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.CmbSupplier = new UltraCombo();
		this.GRNfromdate = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.cmbParentCategory = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.txtGrnNoFrom = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.CmbWarehouseName = new UltraCombo();
		this.txtGrnNoTo = new System.Windows.Forms.TextBox();
		this.CmbCityName = new UltraCombo();
		this.CmbJobLotName = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.GRNToDate = new System.Windows.Forms.DateTimePicker();
		this.GpsNoFrom = new System.Windows.Forms.TextBox();
		this.GpsNoTo = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.btnApprove = new System.Windows.Forms.ToolStripButton();
		this.ApprovedCount = new System.Windows.Forms.ToolStripLabel();
		this.label1 = new System.Windows.Forms.Label();
		this.txtApprovalRemarks = new System.Windows.Forms.TextBox();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParentCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCityName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLotName).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.flowLayoutPanel);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.toolStrip3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(464, 619);
		this.panel1.TabIndex = 0;
		this.flowLayoutPanel.AutoScroll = true;
		this.flowLayoutPanel.BackColor = System.Drawing.Color.White;
		this.flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.flowLayoutPanel.Location = new System.Drawing.Point(0, 89);
		this.flowLayoutPanel.Name = "flowLayoutPanel";
		this.flowLayoutPanel.Size = new System.Drawing.Size(464, 530);
		this.flowLayoutPanel.TabIndex = 11;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.txtApprovalRemarks);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 56);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(464, 33);
		this.panel4.TabIndex = 4;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbperemeter);
		this.groupBox1.Controls.Add(this.btnApproved);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.ChkPrint);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.txtVehicleNo);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.groupBox1.Controls.Add(this.GRNfromdate);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbParentCategory);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.txtGrnNoFrom);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouseName);
		this.groupBox1.Controls.Add(this.txtGrnNoTo);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCityName);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLotName);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.GRNToDate);
		this.groupBox1.Controls.Add(this.GpsNoFrom);
		this.groupBox1.Controls.Add(this.GpsNoTo);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(753, 111);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(134, 25);
		this.groupBox1.TabIndex = 150;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.groupBox1.Visible = false;
		this.cmbperemeter.AllowNull = (DefaultableBoolean)1;
		this.cmbperemeter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbperemeter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbperemeter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbperemeter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Location = new System.Drawing.Point(110, 14);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Name = "cmbperemeter";
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Size = new System.Drawing.Size(148, 26);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).TabIndex = 0;
		this.cmbperemeter.ValueChanged += new System.EventHandler(cmbperemeter_ValueChanged);
		this.btnApproved.BackColor = System.Drawing.Color.Teal;
		this.btnApproved.FlatAppearance.BorderSize = 0;
		this.btnApproved.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApproved.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApproved.ForeColor = System.Drawing.Color.White;
		this.btnApproved.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnApproved.Location = new System.Drawing.Point(808, 70);
		this.btnApproved.Margin = new System.Windows.Forms.Padding(2);
		this.btnApproved.Name = "btnApproved";
		this.btnApproved.Size = new System.Drawing.Size(84, 26);
		this.btnApproved.TabIndex = 14;
		this.btnApproved.Text = "Approve";
		this.btnApproved.UseVisualStyleBackColor = false;
		this.btnApproved.Click += new System.EventHandler(btnApproved_Click);
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(807, 99);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(84, 26);
		this.btnshow.TabIndex = 15;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.ChkPrint.AutoSize = true;
		this.ChkPrint.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrint.Location = new System.Drawing.Point(808, 14);
		this.ChkPrint.Name = "ChkPrint";
		this.ChkPrint.Size = new System.Drawing.Size(67, 19);
		this.ChkPrint.TabIndex = 13;
		this.ChkPrint.Text = "Preview";
		this.ChkPrint.UseVisualStyleBackColor = true;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(5, 20);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(60, 15);
		this.label12.TabIndex = 588;
		this.label12.Text = "Date Type";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(266, 48);
		this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(64, 15);
		this.label11.TabIndex = 145;
		this.label11.Text = "Vehicle No";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(5, 105);
		this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(104, 15);
		this.label10.TabIndex = 144;
		this.label10.Text = "Grn No From && To";
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance68).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(567, 14);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(235, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 9;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(5, 76);
		this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(70, 15);
		this.label3.TabIndex = 143;
		this.label3.Text = "Grn Date To";
		this.txtVehicleNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleNo.Location = new System.Drawing.Point(364, 44);
		this.txtVehicleNo.Margin = new System.Windows.Forms.Padding(4);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.ShortcutsEnabled = false;
		this.txtVehicleNo.Size = new System.Drawing.Size(138, 23);
		this.txtVehicleNo.TabIndex = 6;
		this.txtVehicleNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance74;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance80).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(567, 70);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(235, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 11;
		this.GRNfromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNfromdate.CustomFormat = "dd-MMM-yy";
		this.GRNfromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNfromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.GRNfromdate.Location = new System.Drawing.Point(112, 44);
		this.GRNfromdate.Margin = new System.Windows.Forms.Padding(4);
		this.GRNfromdate.Name = "GRNfromdate";
		this.GRNfromdate.Size = new System.Drawing.Size(148, 23);
		this.GRNfromdate.TabIndex = 1;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(266, 76);
		this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(91, 15);
		this.label5.TabIndex = 141;
		this.label5.Text = "Parent Category";
		this.cmbParentCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbParentCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbParentCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbParentCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbParentCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Location = new System.Drawing.Point(364, 70);
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Name = "cmbParentCategory";
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Size = new System.Drawing.Size(138, 26);
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).TabIndex = 7;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(507, 76);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(51, 15);
		this.label2.TabIndex = 121;
		this.label2.Text = "Supplier";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(5, 48);
		this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(85, 15);
		this.label27.TabIndex = 119;
		this.label27.Text = "Grn Date From";
		this.txtGrnNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrnNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrnNoFrom.Location = new System.Drawing.Point(110, 101);
		this.txtGrnNoFrom.Margin = new System.Windows.Forms.Padding(4);
		this.txtGrnNoFrom.Name = "txtGrnNoFrom";
		this.txtGrnNoFrom.ShortcutsEnabled = false;
		this.txtGrnNoFrom.Size = new System.Drawing.Size(73, 23);
		this.txtGrnNoFrom.TabIndex = 3;
		this.txtGrnNoFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGrnNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGrnNoFrom_KeyPress);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(266, 105);
		this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(67, 15);
		this.label8.TabIndex = 139;
		this.label8.Text = "Warehouse";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(507, 48);
		this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(43, 15);
		this.label7.TabIndex = 138;
		this.label7.Text = "JobLot";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(507, 105);
		this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(27, 15);
		this.label4.TabIndex = 137;
		this.label4.Text = "City";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(507, 20);
		this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(32, 15);
		this.label9.TabIndex = 121;
		this.label9.Text = "Item";
		this.CmbWarehouseName.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouseName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouseName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouseName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehouseName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseName).Location = new System.Drawing.Point(364, 99);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseName).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseName).Name = "CmbWarehouseName";
		((System.Windows.Forms.Control)(object)this.CmbWarehouseName).Size = new System.Drawing.Size(138, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseName).TabIndex = 8;
		this.txtGrnNoTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrnNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrnNoTo.Location = new System.Drawing.Point(187, 101);
		this.txtGrnNoTo.Margin = new System.Windows.Forms.Padding(4);
		this.txtGrnNoTo.Name = "txtGrnNoTo";
		this.txtGrnNoTo.ShortcutsEnabled = false;
		this.txtGrnNoTo.Size = new System.Drawing.Size(73, 23);
		this.txtGrnNoTo.TabIndex = 4;
		this.txtGrnNoTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGrnNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGrnNoTo_KeyPress);
		this.CmbCityName.AllowNull = (DefaultableBoolean)1;
		this.CmbCityName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCityName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCityName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbCityName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCityName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbCityName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCityName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance62).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance63).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance63).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance63).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance65).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCityName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCityName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCityName).Location = new System.Drawing.Point(567, 99);
		((System.Windows.Forms.Control)(object)this.CmbCityName).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.CmbCityName).Name = "CmbCityName";
		((System.Windows.Forms.Control)(object)this.CmbCityName).Size = new System.Drawing.Size(235, 26);
		((System.Windows.Forms.Control)(object)this.CmbCityName).TabIndex = 12;
		this.CmbJobLotName.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLotName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLotName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLotName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLotName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbJobLotName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLotName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLotName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLotName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLotName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLotName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbJobLotName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLotName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLotName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLotName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbJobLotName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLotName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLotName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLotName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbJobLotName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLotName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLotName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLotName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbJobLotName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLotName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLotName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbJobLotName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLotName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbJobLotName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLotName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLotName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLotName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLotName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLotName).Location = new System.Drawing.Point(567, 42);
		((System.Windows.Forms.Control)(object)this.CmbJobLotName).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.CmbJobLotName).Name = "CmbJobLotName";
		((System.Windows.Forms.Control)(object)this.CmbJobLotName).Size = new System.Drawing.Size(235, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLotName).TabIndex = 10;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(266, 20);
		this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(97, 15);
		this.label13.TabIndex = 127;
		this.label13.Text = "GP No From&& To";
		this.GRNToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNToDate.CustomFormat = "dd-MMM-yy";
		this.GRNToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.GRNToDate.Location = new System.Drawing.Point(112, 72);
		this.GRNToDate.Margin = new System.Windows.Forms.Padding(4);
		this.GRNToDate.Name = "GRNToDate";
		this.GRNToDate.Size = new System.Drawing.Size(148, 23);
		this.GRNToDate.TabIndex = 2;
		this.GpsNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.GpsNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GpsNoFrom.Location = new System.Drawing.Point(364, 16);
		this.GpsNoFrom.Margin = new System.Windows.Forms.Padding(4);
		this.GpsNoFrom.Name = "GpsNoFrom";
		this.GpsNoFrom.ShortcutsEnabled = false;
		this.GpsNoFrom.Size = new System.Drawing.Size(68, 23);
		this.GpsNoFrom.TabIndex = 5;
		this.GpsNoFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.GpsNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(GpsNoFrom_KeyPress);
		this.GpsNoTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.GpsNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GpsNoTo.Location = new System.Drawing.Point(434, 16);
		this.GpsNoTo.Margin = new System.Windows.Forms.Padding(4);
		this.GpsNoTo.Name = "GpsNoTo";
		this.GpsNoTo.ShortcutsEnabled = false;
		this.GpsNoTo.Size = new System.Drawing.Size(68, 23);
		this.GpsNoTo.TabIndex = 6;
		this.GpsNoTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.GpsNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(GpsNoTo_KeyPress);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(464, 29);
		this.panel3.TabIndex = 3;
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.Teal;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(4, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(288, 19);
		this.label23.TabIndex = 4;
		this.label23.Text = "Pending GRN For Special Approval";
		this.toolStrip3.BackColor = System.Drawing.Color.White;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnNew, this.btnRefresh, this.btnShortCutKeys, this.btnApprove, this.ApprovedCount });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(464, 27);
		this.toolStrip3.TabIndex = 10;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Visible = false;
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Visible = false;
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(121, 24);
		this.btnShortCutKeys.Text = "ShortCutKeys";
		this.btnShortCutKeys.ToolTipText = "ShortCutKeys";
		this.btnShortCutKeys.Visible = false;
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		this.btnApprove.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.btnApprove.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnApprove.Name = "btnApprove";
		this.btnApprove.Size = new System.Drawing.Size(90, 24);
		this.btnApprove.Text = "Approve ";
		this.btnApprove.Click += new System.EventHandler(btnApproved_Click);
		this.ApprovedCount.Name = "ApprovedCount";
		this.ApprovedCount.Size = new System.Drawing.Size(15, 24);
		this.ApprovedCount.Text = "0";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Black;
		this.label1.Location = new System.Drawing.Point(7, 7);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(120, 17);
		this.label1.TabIndex = 151;
		this.label1.Text = "Approval Remarks";
		this.txtApprovalRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtApprovalRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtApprovalRemarks.Location = new System.Drawing.Point(126, 5);
		this.txtApprovalRemarks.Margin = new System.Windows.Forms.Padding(4);
		this.txtApprovalRemarks.Name = "txtApprovalRemarks";
		this.txtApprovalRemarks.ShortcutsEnabled = false;
		this.txtApprovalRemarks.Size = new System.Drawing.Size(334, 23);
		this.txtApprovalRemarks.TabIndex = 152;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(464, 619);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "PendingGRNForSpecialApproval";
		this.Text = "Goods Receiving Notes";
		base.Load += new System.EventHandler(frmPurchaseInvoiceHistory_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmPurchaseInvoiceHistory_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParentCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCityName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLotName).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
