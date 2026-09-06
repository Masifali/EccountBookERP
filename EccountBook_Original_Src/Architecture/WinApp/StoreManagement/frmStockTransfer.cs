using System;
using System.Collections;
using System.Collections.Generic;
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
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Contractor_Wages;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Production;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.StoreManagement;

public class frmStockTransfer : Form
{
	private DataTable table = new DataTable();

	private DataTable dtExp = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtAccountlst = new DataTable();

	private DataTable dtItemsFromDO = new DataTable();

	private int updateDetailIndex = -1;

	public static int Id;

	private int RefDocumentTypeId;

	private int DefaultDaysToLessFromHistoryFromDate;

	private string DetailRowsRemoveIds = "";

	private bool IsApproved;

	private bool WagesStatus;

	private bool FIFOCGSFlag;

	private bool WagesActiveOrInActive;

	private bool StockTransferFinancialEffectIsActive;

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formrights = new Rightsobjects();

	private LoadavailableTransactionsForIssuance LoadTransactions;

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private TabPage tabPage2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnprint;

	private Panel panel4;

	private Label label33;

	private DateTimePicker DocDate;

	private Label label17;

	private TextBox txtdocno;

	private Label label5;

	private Panel panel5;

	private Button btnplus;

	private TextBox txtQty;

	private Label label45;

	private Label label38;

	private Label label36;

	private Label label37;

	internal UltraCombo cmbCropYear;

	internal UltraCombo cmbPackingType;

	internal UltraCombo CmbPackUom;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private Button btnUpdateDetail;

	private Button btnCancelUpdateDetial;

	internal GridEX grdhistory;

	private ToolStripButton btnRefresh;

	internal UltraCombo cmbItemName;

	private Label label2;

	private TextBox txtRemarksHead;

	internal UltraCombo CmbWareHouseFrom;

	private Label label3;

	internal GridEX grd;

	private Label label6;

	private Label label4;

	private Label label1;

	private TextBox txtHeadNetWeight;

	internal UltraCombo cmbTicketNo;

	internal UltraCombo cmbGatePass;

	private TextBox txtRemarksDetail;

	private Label label12;

	internal UltraCombo cmbWareHouseTo;

	private Label label11;

	private TextBox txtDetailNetWeight;

	private Label label10;

	private TextBox txtAdLsWeight;

	private Label label9;

	private TextBox txtGrossWeight;

	private Label label8;

	internal UltraCombo cmbJobLot;

	private Label label7;

	private ToolStripSeparator toolStripSeparator1;

	private Label label14;

	internal UltraCombo CmbTransferType;

	private Panel panel8;

	private CtrlGrdBar ctrlGrdBar4;

	private CtrlGrdBar ctrlGrdBar3;

	private CtrlGrdBar ctrlGrdBar5;

	private TextBox txtWorkingReportNo;

	private Label label15;

	private CheckBox ChkBoxPrintPreview;

	private TextBox txtEbTotal;

	private Label label19;

	private TextBox txtEbUnit;

	private Label label16;

	private Label label20;

	private TextBox txtAvailableStock;

	private ToolStripButton btnLoadInvoices;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private TabPage tabPage4;

	internal GridEX grdOtherCharges;

	private ToolStripButton btnAttachment;

	internal UltraCombo CmbJobLotTo;

	private Label label21;

	private TextBox txtAvgRate;

	private Label lblAvgRate;

	private TextBox txtAmount;

	private Label lblAmount;

	private Label label22;

	private TextBox txtOtherWeight;

	private TextBox txtNoofBranches;

	private Panel panel12;

	private Label label13;

	private Panel panel15;

	private Label label18;

	internal GridEX GridDetailHistory;

	private CtrlGrdBar ctrlGrdBar6;

	internal GridEX grdDetail;

	private Panel panel6;

	private Panel panel3;

	private Panel panel10;

	private Label label24;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator6;

	private Label label23;

	private Panel panel7;

	private Label label25;

	private GroupBox groupBox2;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Button btnshow;

	private Label label26;

	private Label label27;

	private Label label43;

	private Label label62;

	private Label label63;

	internal UltraCombo cmbTransactiontypeHistory;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private ToolStripButton BtnShortCutkeys;

	private Button BtnSameUpdateWarehouse;

	private Panel PanelMessage;

	private ToolStrip ToolStripMessage;

	private ToolStripButton BtnMessage;

	private CheckBox chkUpdateJobLot;

	private CheckBox chkUpdateWareHouse;

	private ToolStripButton btnDelete;

	public frmStockTransfer(UserAccount user)
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmStockTransfer_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1310, 830);
			FIFOCGSFlag = CommonServices.GetERPFeatureById(5);
			((Control)(object)CmbTransferType).Enabled = false;
			((Control)(object)cmbGatePass).Enabled = false;
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formrights.DoHaveSaveRight;
			btnupdate.Enabled = formrights.DoHaveUpdateRights;
			btnDelete.Enabled = formrights.DoHaveCanDelete;
			btnDelete.Visible = false;
			txtRemarksDetail.Width = 332;
			lblAvgRate.Visible = false;
			lblAmount.Visible = false;
			txtAvgRate.Visible = false;
			txtAmount.Visible = false;
			tabControl2.TabPages.Remove(tabPage4);
			StockTransferFinancialEffectIsActive = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("StockTransferFinancialEffectIsActive"));
			WagesStatus = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("WagesCompulsoryOnStockTransfer"));
			WagesRefDocumentsStatus obj = clsGlobalVariables.WagesRefDocumentsStatusList.Find((WagesRefDocumentsStatus row) => row.RefDocumentTypeId == 68);
			WagesActiveOrInActive = obj != null && obj.IsActive;
			if (StockTransferFinancialEffectIsActive)
			{
				tabControl2.TabPages.Add(tabPage4);
				txtRemarksDetail.Width = 167;
				lblAvgRate.Visible = true;
				lblAmount.Visible = true;
				txtAvgRate.Visible = true;
				txtAmount.Visible = true;
			}
			table.Columns.Add("FlagForSplitAndDelete", typeof(bool));
			table.Columns.Add("Id", typeof(int));
			table.Columns.Add("RefDocumentTypeId", typeof(int));
			table.Columns.Add("RefDocNoId", typeof(int));
			table.Columns.Add("RefDocSubIdNo", typeof(int));
			table.Columns.Add("ItemId", typeof(int));
			table.Columns.Add("ItemName", typeof(string));
			table.Columns.Add("PackTypeId", typeof(int));
			table.Columns.Add("PackType", typeof(string));
			table.Columns.Add("CropYear", typeof(string));
			table.Columns.Add("JobLotId", typeof(int));
			table.Columns.Add("JobLot", typeof(string));
			table.Columns.Add("JobLotIdTo", typeof(int));
			table.Columns.Add("JobLotTo", typeof(string));
			table.Columns.Add("QTY", typeof(double));
			table.Columns.Add("PackUOMId", typeof(int));
			table.Columns.Add("PackUOM", typeof(string));
			table.Columns.Add("GrossWeight", typeof(double));
			table.Columns.Add("EbUnit", typeof(double));
			table.Columns.Add("EbTotal", typeof(double));
			table.Columns.Add("Ad/LsWeight", typeof(double));
			table.Columns.Add("NetWeight", typeof(double));
			table.Columns.Add("BalQty", typeof(double));
			table.Columns.Add("BalWeight", typeof(double));
			table.Columns.Add("WareHouseFromId", typeof(int));
			table.Columns.Add("WareHouseFrom", typeof(string));
			table.Columns.Add("WareHouseToId", typeof(int));
			table.Columns.Add("WareHouseTo", typeof(string));
			table.Columns.Add("Remarks", typeof(string));
			table.Columns.Add("ItemRate", typeof(double));
			table.Columns.Add("RateUOMId", typeof(int));
			table.Columns.Add("RateUOM", typeof(string));
			table.Columns.Add("ItemAmount", typeof(double));
			table.Columns.Add("Expense", typeof(double));
			table.Columns.Add("TransferDocumentTypeId", typeof(int));
			table.Columns.Add("TransferId", typeof(int));
			table.Columns.Add("TransferDetailId", typeof(int));
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			dtAccountlst.Columns.Add("Id");
			dtAccountlst.Columns.Add("AccountTitle");
			AccountsFill();
			dtExp.Columns.Add("Account");
			dtExp.Columns.Add("Percentage");
			dtExp.Columns.Add("Qty", typeof(double));
			dtExp.Columns.Add("Rate", typeof(double));
			dtExp.Columns.Add("Amount", typeof(double));
			dtExp.Columns.Add("Remarks");
			AddRowInExpenseGrid();
			grdOtherCharges.RetrieveStructure();
			grdExpenseSettings();
			DocumentNoFill();
			WareHouseFill();
			GetPackingType();
			TransferTypeFill();
			GetCropYear();
			GetJobLot();
			ItemDetailFill();
			PendingRecordForStockTransfer();
			DocDate.Focus();
			btnupdate.Visible = false;
			btnsave.Visible = true;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			ChkBoxPrintPreview.Checked = true;
			CmbTransferType_Leave(null, null);
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			HistoryComboFill();
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDateHistory.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CombGatePassNoFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		try
		{
			int ID = Conversion.ToInt(cmbGatePass.Value);
			GatePassOutward gatePass = new GatePassOutward();
			gatePass.OrganizationId = UserAccount.OrganizationId;
			gatePass.CompanyId = UserAccount.CompanyId;
			gatePass.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			gatePass.BranchesId = UserAccount.BranchesId;
			if (((Control)(object)CmbTransferType).Text == "Inward")
			{
				gatePass.DocumentTypeId = RefDocumentTypeId;
			}
			if (((Control)(object)CmbTransferType).Text == "Outward")
			{
				gatePass.DocumentTypeId = 91;
			}
			DataTable dt = GatePassGeneral.GetGatePassNoForStockTransfer(gatePass);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbGatePass, "Id", "GpSrNo", "Gate Pass #", ZeroIndex: true);
				((UltraGridBase)cmbGatePass).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						cmbGatePass.Value = ID;
					}
					else
					{
						((Control)(object)cmbGatePass).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)cmbGatePass).DataSource = null;
				((UltraGridBase)cmbTicketNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combTicketNo(int GpId)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		try
		{
			int ID = Conversion.ToInt(cmbTicketNo.Value);
			GatePassGeneral tickt = new GatePassGeneral();
			tickt.OrganizationId = UserAccount.OrganizationId;
			tickt.CompanyId = UserAccount.CompanyId;
			tickt.Id = Conversion.ToInt((object)GpId);
			tickt.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (((Control)(object)CmbTransferType).Text == "Inward")
			{
				tickt.RefDocumentTypeId = RefDocumentTypeId;
			}
			if (((Control)(object)CmbTransferType).Text == "Outward")
			{
				tickt.RefDocumentTypeId = 91;
			}
			DataTable dt = GatePassGeneral.GetTicketNoForStockTransfer(tickt);
			if (dt.Rows.Count > 0)
			{
				txtOtherWeight.Text = Conversion.ToString(dt.Rows[0]["OtherWeight"]);
				DDL.BindDDLNew(dt, cmbTicketNo, "Id", "TicketNo", "Ticket No #", true);
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						cmbTicketNo.Value = ID;
					}
					else
					{
						((Control)(object)cmbTicketNo).Text = string.Empty;
					}
				}
			}
			else
			{
				txtOtherWeight.Text = string.Empty;
				if (((Control)(object)CmbTransferType).Text != "MoveOrder")
				{
					((UltraGridBase)cmbTicketNo).DataSource = null;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetItemIdFromDeliveryOrderForStockTransfer(int GpId)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			int ID = Conversion.ToInt(cmbItemName.Value);
			dtItemsFromDO.Rows.Clear();
			InvStockTransferHeader obj = new InvStockTransferHeader();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.GatePassId = GpId;
			dtItemsFromDO = InvStockTransferHeader.GetItemIdFromDeliveryOrderForStockTransfer(obj);
			if (dtItemsFromDO.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtItemsFromDO, cmbItemName, "ItemId", "ItemName", "Item Name", true);
				((UltraGridBase)cmbItemName).DisplayLayout.Bands[0].Columns[1].Width = 500;
				if (ID > 0)
				{
					if (dtItemsFromDO.AsEnumerable().Any((DataRow row) => row.Field<int>("ItemId") == ID))
					{
						cmbItemName.Value = ID;
					}
					else
					{
						((Control)(object)cmbItemName).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbItemName).Text = string.Empty;
				((UltraGridBase)cmbItemName).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WareHouseFill()
	{
		try
		{
			int ID = Conversion.ToInt(CmbWareHouseFrom.Value);
			int ID2 = Conversion.ToInt(cmbWareHouseTo.Value);
			DataTable dt = CommonServices.getActiveWareHouse();
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dt, CmbWareHouseFrom, "Id", "WareHouseName", "WareHouseName", ZeroIndex: true);
			DDL.BindDDL(dt, cmbWareHouseTo, "Id", "WareHouseName", "WareHouseName", ZeroIndex: true);
			if (ID > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
				{
					CmbWareHouseFrom.Value = ID;
				}
				else
				{
					((Control)(object)CmbWareHouseFrom).Text = string.Empty;
				}
			}
			if (ID2 > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID2))
				{
					cmbWareHouseTo.Value = ID2;
				}
				else
				{
					((Control)(object)cmbWareHouseTo).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumentNoFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		try
		{
			int code = InvStockTransferHeader.GenerateCode(new InvStockTransferHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 68,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (code > 0)
			{
				txtdocno.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailFill()
	{
		try
		{
			dtItem = CommonServices.ReadAllItems();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailFillForInwardandMoveOrder()
	{
		try
		{
			int ID = Conversion.ToInt(cmbItemName.Value);
			((Control)(object)cmbItemName).Text = string.Empty;
			((UltraGridBase)cmbItemName).DataSource = null;
			if (dtItem.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dtItem, cmbItemName, "Id", "ItemName", "Item Name", true);
			((UltraGridBase)cmbItemName).DisplayLayout.Bands[0].Columns[1].Width = 500;
			if (ID > 0)
			{
				string ItemId = "";
				if (dtItem.Columns.Contains("ItemId"))
				{
					ItemId = "ItemId";
				}
				else
				{
					ItemId = "Id";
				}
				if (dtItem.AsEnumerable().Any((DataRow row) => row.Field<int>(ItemId) == ID))
				{
					cmbItemName.Value = ID;
				}
				else
				{
					((Control)(object)cmbItemName).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void bindRateUomAndItemPackUom(int ID)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		try
		{
			string Uom = ((Control)(object)CmbPackUom).Text;
			DataTable dt = UOMSchedule.SearchByObject(new UOMSchedule
			{
				ItemId = Conversion.ToInt((object)ID),
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbPackUom, "Id", "Equivalent", "Pack Uom", true);
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<double>("Equivalent") == Conversion.ToDouble((object)Uom)))
				{
					((Control)(object)CmbPackUom).Text = Uom;
				}
				else
				{
					((Control)(object)CmbPackUom).Text = string.Empty;
				}
			}
			else
			{
				((Control)(object)CmbPackUom).Text = string.Empty;
				((UltraGridBase)CmbPackUom).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetPackingType()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			int ID = Conversion.ToInt(cmbPackingType.Value);
			new InvPackingType();
			DataTable dt = InvPackingType.Getall();
			if (dt.Rows.Count <= 0 || dt == null)
			{
				return;
			}
			DDL.BindDDLNew(dt, cmbPackingType, "Id", "PackTypeDesc", "Packing Type", true);
			if (ID > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
				{
					cmbPackingType.Value = ID;
				}
				else
				{
					((Control)(object)cmbPackingType).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetCropYear()
	{
		try
		{
			int ID = Conversion.ToInt(cmbCropYear.Value);
			DataTable dt = CommonServices.CropYearGetAllService();
			if (dt.Rows.Count <= 0 || dt == null)
			{
				return;
			}
			DDL.BindDDLNew(dt, cmbCropYear, "Id", "CropYear", "Crop Year", true);
			if (ID > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
				{
					cmbCropYear.Value = ID;
				}
				else
				{
					((Control)(object)cmbCropYear).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetJobLot()
	{
		try
		{
			int ID = Conversion.ToInt(cmbJobLot.Value);
			int ID2 = Conversion.ToInt(CmbJobLotTo.Value);
			DataTable dt = CommonServices.JobLotGetAllService();
			if (dt.Rows.Count <= 0 || dt == null)
			{
				return;
			}
			DDL.BindDDLNew(dt, cmbJobLot, "Id", "JobLotDescription", "Job Lot From", true);
			DDL.BindDDLNew(dt, CmbJobLotTo, "Id", "JobLotDescription", "Job Lot To", true);
			if (ID > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
				{
					cmbJobLot.Value = ID;
				}
				else
				{
					((Control)(object)cmbJobLot).Text = string.Empty;
				}
			}
			if (ID2 > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID2))
				{
					CmbJobLotTo.Value = ID2;
				}
				else
				{
					((Control)(object)CmbJobLotTo).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TransferTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("TransferType");
			dt.Rows.Add(1, "Inward");
			dt.Rows.Add(2, "Outward");
			dt.Rows.Add(3, "MoveOrder");
			DDL.BindDDLNew(dt, CmbTransferType, "Id", "TransferType", "TransferType", false);
			((UltraGridBase)CmbTransferType).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetSlipNoForMoveOrder()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		try
		{
			int ID = Conversion.ToInt(cmbTicketNo.Value);
			DataTable dt = GatePassGeneral.GetTicketNoMoveOrderBaseForStockTransfer(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				BranchesId = UserAccount.BranchesId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbTicketNo, "Id", "TicketNo", "TicketNo", ZeroIndex: true);
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						cmbTicketNo.Value = ID;
					}
					else
					{
						((Control)(object)cmbTicketNo).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)cmbGatePass).DataSource = null;
				((UltraGridBase)cmbTicketNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountsFill()
	{
		try
		{
			DataTable dtcoa = CommonServices.CoaAllocationGetAllServiceBind();
			if (dtcoa.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtcoa.Rows.Count; i++)
			{
				if (Convert.ToInt32(dtcoa.Rows[i]["AccountTypeId"]) != 2 && Convert.ToInt32(dtcoa.Rows[i]["AccountTypeId"]) != 11 && Convert.ToInt32(dtcoa.Rows[i]["AccountTypeId"]) != 12 && Convert.ToInt32(dtcoa.Rows[i]["AccountTypeId"]) != 15)
				{
					dtAccountlst.Rows.Add(dtcoa.Rows[i]["Id"], dtcoa.Rows[i]["AccountTitle"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbGatePass_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)CmbTransferType).Text == "Inward" || ((Control)(object)CmbTransferType).Text == "Outward")
			{
				((Control)(object)cmbTicketNo).Text = string.Empty;
				if (Conversion.ToInt(cmbGatePass.Value) > 0)
				{
					combTicketNo(Conversion.ToInt(cmbGatePass.Value));
				}
			}
			else
			{
				((UltraGridBase)cmbGatePass).DataSource = null;
			}
			if (((Control)(object)CmbTransferType).Text == "Outward")
			{
				((Control)(object)cmbItemName).Text = string.Empty;
				((UltraGridBase)cmbItemName).DataSource = null;
				GetItemIdFromDeliveryOrderForStockTransfer(Conversion.ToInt(cmbGatePass.Value));
			}
			if (((Control)(object)CmbTransferType).Text == "Inward" && ((UltraGridBase)cmbGatePass).ActiveRow != null)
			{
				if (RefDocumentTypeId == 51)
				{
					((Control)(object)cmbItemName).Text = string.Empty;
					((UltraGridBase)cmbItemName).DataSource = null;
					GetItemIdFromDeliveryOrderForStockTransfer(Conversion.ToInt(((UltraDropDownBase)cmbGatePass).SelectedRow.Cells["RefDocId"].Value));
				}
				else if (RefDocumentTypeId == 52)
				{
					ItemDetailFillForInwardandMoveOrder();
					panel5.Visible = true;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItemName_Leave(object sender, EventArgs e)
	{
		try
		{
			bindRateUomAndItemPackUom(Conversion.ToInt(cmbItemName.Value));
			AvailableStockGetByItem();
			GetAvgRate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbTransferType_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)CmbTransferType).Text == "Inward" || ((Control)(object)CmbTransferType).Text == "Outward")
			{
				CombGatePassNoFill();
				return;
			}
			GetSlipNoForMoveOrder();
			ItemDetailFillForInwardandMoveOrder();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbTicketNo_Leave(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			HandleVisibilityOfLoader();
			DataTable dt = GatePassGeneral.GetNetWeightByTicketId(new GatePassGeneral
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(cmbTicketNo.Value)
			});
			if (dt.Rows.Count > 0)
			{
				txtHeadNetWeight.Text = Conversion.ToString(dt.Rows[0]["NetWbWeight"]);
				txtWorkingReportNo.Text = Conversion.ToString(dt.Rows[0]["WorkingReportNo"]);
			}
			else
			{
				txtHeadNetWeight.Text = "0";
				txtWorkingReportNo.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtdocno.Text.Trim() == string.Empty || txtdocno.Text.Trim() == "0")
		{
			MessageBox.Show("DocNo Field is Required");
			txtdocno.Focus();
			return false;
		}
		if (((UltraGridBase)CmbTransferType).ActiveRow == null || Conversion.ToInt(CmbTransferType.Value) == 0)
		{
			MessageBox.Show("Transfer Type Field is Required");
			CmbTransferType.Focus();
			return false;
		}
		if (btnsave.Visible && btnsave.Enabled)
		{
			if (((UltraGridBase)cmbGatePass).ActiveRow == null || Conversion.ToInt(cmbGatePass.Value) == 0)
			{
				if (((Control)(object)CmbTransferType).Text != "MoveOrder")
				{
					MessageBox.Show("GatePassNo Field is Required");
					cmbGatePass.Focus();
					return false;
				}
				return true;
			}
		}
		else if (((Control)(object)cmbGatePass).Text.Trim() == string.Empty || Conversion.ToInt(cmbGatePass.Value) == 0)
		{
			if (((Control)(object)CmbTransferType).Text != "MoveOrder")
			{
				MessageBox.Show("GatePassNo Field is Required");
				cmbGatePass.Focus();
				return false;
			}
			return true;
		}
		if (((UltraGridBase)cmbTicketNo).ActiveRow == null || Conversion.ToInt(cmbTicketNo.Value) == 0)
		{
			MessageBox.Show("TicketNo Field is Required");
			cmbTicketNo.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (Conversion.ToInt(CmbTransferType.Value) == 1 && Conversion.ToInt((object)txtNoofBranches.Text) > 1 && (((UltraGridBase)cmbWareHouseTo).ActiveRow == null || Conversion.ToInt(cmbWareHouseTo.Value) == 0))
		{
			MessageBox.Show("WarehouseTo field is required");
			cmbWareHouseTo.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbTransferType.Value) == 2 && Conversion.ToInt((object)txtNoofBranches.Text) > 1 && (((UltraGridBase)CmbWareHouseFrom).ActiveRow == null || Conversion.ToInt(CmbWareHouseFrom.Value) == 0))
		{
			MessageBox.Show("WarehouseFrom field is required");
			CmbWareHouseFrom.Focus();
			return false;
		}
		if (Conversion.ToInt((object)txtNoofBranches.Text) <= 1)
		{
			if (((UltraGridBase)CmbWareHouseFrom).ActiveRow == null || Conversion.ToInt(CmbWareHouseFrom.Value) == 0)
			{
				MessageBox.Show("WarehouseFrom field is required");
				CmbWareHouseFrom.Focus();
				return false;
			}
			if (((UltraGridBase)cmbWareHouseTo).ActiveRow == null || Conversion.ToInt(cmbWareHouseTo.Value) == 0)
			{
				MessageBox.Show("WarehouseTo field is required");
				cmbWareHouseTo.Focus();
				return false;
			}
		}
		if (((UltraGridBase)cmbItemName).ActiveRow == null || Conversion.ToInt(cmbItemName.Value) == 0)
		{
			MessageBox.Show("Item Name field is required");
			cmbItemName.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPackingType).ActiveRow == null || Conversion.ToInt(cmbPackingType.Value) == 0)
		{
			MessageBox.Show("PackingType field is required");
			cmbPackingType.Focus();
			return false;
		}
		if (((UltraGridBase)cmbCropYear).ActiveRow == null || Conversion.ToInt(cmbCropYear.Value) == 0)
		{
			MessageBox.Show("ItemName field is required");
			cmbCropYear.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUom).ActiveRow == null || Conversion.ToInt(CmbPackUom.Value) == 0)
		{
			MessageBox.Show("PackUOM field is required");
			CmbPackUom.Focus();
			return false;
		}
		if (((UltraGridBase)cmbJobLot).ActiveRow == null || Conversion.ToInt(cmbJobLot.Value) == 0)
		{
			MessageBox.Show("JobLot From field is required");
			cmbJobLot.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobLotTo).ActiveRow == null || Conversion.ToInt(CmbJobLotTo.Value) == 0)
		{
			MessageBox.Show("JobLotTo field is required");
			CmbJobLotTo.Focus();
			return false;
		}
		if (txtQty.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtQty.Text) == 0.0)
		{
			MessageBox.Show("Qty field is required");
			txtQty.Focus();
			return false;
		}
		if (txtGrossWeight.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtGrossWeight.Text) == 0.0)
		{
			MessageBox.Show("GrossWeight field is required");
			txtGrossWeight.Focus();
			return false;
		}
		if (StockTransferFinancialEffectIsActive && !FIFOCGSFlag)
		{
			if (txtAvgRate.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtAvgRate.Text) == 0.0)
			{
				MessageBox.Show("AvgRate field is required");
				txtAvgRate.Focus();
				return false;
			}
			if (txtAmount.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtAmount.Text) == 0.0)
			{
				MessageBox.Show("Amount field is required");
				txtAmount.Focus();
				return false;
			}
		}
		if (txtDetailNetWeight.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtDetailNetWeight.Text) == 0.0)
		{
			MessageBox.Show("NetWeight field is required");
			txtDetailNetWeight.Focus();
			return false;
		}
		return true;
	}

	private void AddRowInExpenseGrid()
	{
		dtExp.Rows.Add(0, 0, 0, 0, 0, "");
		grdOtherCharges.DataSource = dtExp;
	}

	private void grdExpenseSettings()
	{
		try
		{
			grdOtherCharges.RootTable.Columns["Amount"].Caption = "Credit";
			grdOtherCharges.RootTable.Columns["Account"].Caption = "AccountTitle";
			grdOtherCharges.RootTable.Columns["Percentage"].Caption = "%";
			grdOtherCharges.RootTable.Columns["Account"].ColumnType = (ColumnType)0;
			grdOtherCharges.RootTable.Columns["Account"].HasValueList = true;
			grdOtherCharges.RootTable.Columns["Account"].LimitToList = true;
			grdOtherCharges.RootTable.Columns["Account"].EditType = (EditType)4;
			grdOtherCharges.RootTable.Columns["Account"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "Id", "AccountTitle");
			grdOtherCharges.RootTable.Columns["Account"].Width = 150;
			grdOtherCharges.RootTable.Columns["Remarks"].Width = 120;
			grdOtherCharges.RootTable.Columns["Amount"].Width = 80;
			grdOtherCharges.RootTable.Columns["Percentage"].Width = 50;
			grdOtherCharges.RootTable.Columns.Add("Delete");
			grdOtherCharges.RootTable.Columns["Delete"].Key = "Delete";
			grdOtherCharges.RootTable.Columns["Delete"].Caption = "X";
			grdOtherCharges.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOtherCharges.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdOtherCharges.RootTable.Columns["Delete"].ButtonText = "X";
			grdOtherCharges.RootTable.Columns["Delete"].Width = 20;
			grdOtherCharges.RootTable.Columns.Add("Add");
			grdOtherCharges.RootTable.Columns["Add"].Key = "Add";
			grdOtherCharges.RootTable.Columns["Add"].Caption = "+";
			grdOtherCharges.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOtherCharges.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdOtherCharges.RootTable.Columns["Add"].ButtonText = "+";
			grdOtherCharges.RootTable.Columns["Add"].Width = 20;
			grdOtherCharges.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdOtherCharges.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdOtherCharges.RootTable.Columns["Amount"].FormatString = "#,#.###";
			grdOtherCharges.RootTable.Columns["Amount"].TotalFormatString = "#,#.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OtherExpensesProportion()
	{
		try
		{
			if (grd.GetRows().Length != 0)
			{
				double NetWeight = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["NetWeight"], (AggregateFunction)2).ToString());
				double CreditForExpense = Conversion.ToDouble((object)grdOtherCharges.GetTotal(grdOtherCharges.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
				if (CreditForExpense > 0.0)
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj in rows)
					{
						obj.BeginEdit();
						double Weight = Conversion.ToDouble((object)obj.Cells["NetWeight"].Value.ToString());
						obj.Cells["Expense"].Value = CreditForExpense / NetWeight * Weight;
					}
				}
				else
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj2 in rows)
					{
						obj2.BeginEdit();
						obj2.Cells["Expense"].Value = 0;
					}
				}
			}
			grd.UpdateData();
			grdOtherCharges.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OtherChargesAmountUpdate()
	{
		try
		{
			GridEXRow[] rows = grdOtherCharges.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToDouble(item.Cells["Amount"].Value) > 0.0 && item.Cells["Percentage"].Value.ToString() != string.Empty)
				{
					item.BeginEdit();
					double num = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
					double Percentage = Conversion.ToDouble((object)item.Cells["Percentage"].Value.ToString());
					double TotlaPercentage = num / 100.0 * Percentage;
					if (TotlaPercentage > 0.0)
					{
						item.Cells["Amount"].Value = Conversion.ToDouble((object)TotlaPercentage);
					}
					item.Cells["Qty"].Value = 0;
					item.Cells["Rate"].Value = 0;
					item.EndEdit();
				}
			}
			grdOtherCharges.UpdateData();
			OtherExpensesProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOtherCharges_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdOtherCharges.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				dtExp.Rows[item.RowIndex].Delete();
				item.Delete();
				if (dtExp.Rows.Count == 0)
				{
					AddRowInExpenseGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInExpenseGrid();
			}
			grdOtherCharges.UpdateData();
			OtherExpensesProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOtherCharges_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdOtherCharges.UpdateData();
			GridEXRow item = grdOtherCharges.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
				{
					double Qty = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
					double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
					item.Cells["Amount"].Value = Qty * Rate;
					item.Cells["Percentage"].Value = 0;
				}
				if (e.Column.Key == "Percentage" && item.Cells["Percentage"].Value.ToString() != string.Empty)
				{
					double num = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
					double Percentage = Conversion.ToDouble((object)item.Cells["Percentage"].Value.ToString());
					double TotlaPercentage = num / 100.0 * Percentage;
					if (TotlaPercentage > 0.0)
					{
						item.Cells["Amount"].Value = Conversion.ToDouble((object)TotlaPercentage);
					}
					item.Cells["Qty"].Value = 0;
					item.Cells["Rate"].Value = 0;
				}
			}
			grdOtherCharges.UpdateData();
			OtherExpensesProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PendingRecordForStockTransfer()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		try
		{
			DataTable dtGrid = InvStockTransferHeader.PendingRecordsForStockTransfer(new InvStockTransferHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (dtGrid.Rows.Count > 0 && dtGrid != null)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("RefDocId", typeof(int));
				dt.Columns.Add("Doc_Type", typeof(string));
				dt.Columns.Add("WB_DocDate", typeof(DateTime));
				dt.Columns.Add("TicketNo", typeof(string));
				dt.Columns.Add("GpNo", typeof(string));
				dt.Columns.Add("WorkingReportNo", typeof(string));
				dt.Columns.Add("DoNo", typeof(string));
				dt.Columns.Add("VehicleNo", typeof(string));
				dt.Columns.Add("ItemQty", typeof(int));
				dt.Columns.Add("FirstWeight", typeof(decimal));
				dt.Columns.Add("SecondWeight", typeof(decimal));
				dt.Columns.Add("NetWeight", typeof(decimal));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("Remarks", typeof(string));
				dt.Columns.Add("Status", typeof(string));
				dt.Columns.Add("NoofBranches", typeof(int));
				dt.Columns.Add("RefGpNo", typeof(string));
				dt.Columns.Add("TransferNo", typeof(string));
				foreach (DataRow row in dtGrid.Rows)
				{
					dt.Rows.Add(row["Id"], row["DocumentTypeId"], row["RefDocId"], row["Doc_Type"], row["WB_DocDate"], row["TicketNo"], row["GpNo"], row["WorkingReportNo"], row["DoNo"], row["VehicleNo"], row["ItemQty"], row["FirstWeight"], row["SecondWeight"], row["NetWeight"], row["ItemName"], row["Remarks"], row["Status"], row["NoofBranches"], row["RefGpNo"], row["TransferNo"]);
				}
				grdDetail.DataSource = dt;
				grdDetail.RetrieveStructure();
				GridEX_Helper.GridWrappingAndColumnSettings(grdDetail, 2);
				grdDetail.RootTable.Columns["Doc_Type"].Width = Constants.InventoryConstants.DocumentType;
				grdDetail.RootTable.Columns["WB_DocDate"].Width = Constants.InventoryConstants.DateConstant;
				grdDetail.RootTable.Columns["TicketNo"].Width = Constants.InventoryConstants.DocNoConstant;
				grdDetail.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.DocNoConstant;
				grdDetail.RootTable.Columns["WorkingReportNo"].Width = Constants.InventoryConstants.DocNoConstant + 20;
				grdDetail.RootTable.Columns["DoNo"].Width = Constants.InventoryConstants.DocNoConstant;
				grdDetail.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
				grdDetail.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
				grdDetail.RootTable.Columns["FirstWeight"].Width = Constants.InventoryConstants.WeightConstant;
				grdDetail.RootTable.Columns["SecondWeight"].Width = Constants.InventoryConstants.WeightConstant;
				grdDetail.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
				grdDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
				grdDetail.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
				grdDetail.RootTable.Columns["Status"].Width = Constants.InventoryConstants.StatusConstant;
				grdDetail.RootTable.Columns["NoofBranches"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
				grdDetail.RootTable.Columns["RefGpNo"].Width = Constants.InventoryConstants.DocNoConstant;
				grdDetail.RootTable.Columns["TransferNo"].Width = Constants.InventoryConstants.DocNoConstant;
				grdDetail.RootTable.Columns["Id"].Visible = false;
				grdDetail.RootTable.Columns["DocumentTypeId"].Visible = false;
				grdDetail.RootTable.Columns["RefDocId"].Visible = false;
				grdDetail.RootTable.Columns["NoofBranches"].Visible = false;
				grdDetail.RootTable.Columns["FirstWeight"].AggregateFunction = (AggregateFunction)2;
				grdDetail.RootTable.Columns["FirstWeight"].TextAlignment = (TextAlignment)3;
				grdDetail.RootTable.Columns["FirstWeight"].FormatString = "#,#;(#,#);0";
				grdDetail.RootTable.Columns["FirstWeight"].TotalFormatString = "#,#";
				grdDetail.RootTable.Columns["SecondWeight"].AggregateFunction = (AggregateFunction)2;
				grdDetail.RootTable.Columns["SecondWeight"].TextAlignment = (TextAlignment)3;
				grdDetail.RootTable.Columns["SecondWeight"].FormatString = "#,#;(#,#);0";
				grdDetail.RootTable.Columns["SecondWeight"].TotalFormatString = "#,#";
				grdDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
				grdDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
				grdDetail.RootTable.Columns["NetWeight"].FormatString = "#,#;(#,#);0";
				grdDetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,#";
				grdDetail.RootTable.Columns.Add("Load");
				grdDetail.RootTable.Columns["Load"].Caption = "Load";
				grdDetail.RootTable.Columns["Load"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdDetail.RootTable.Columns["Load"].ButtonStyle = (ButtonStyle)5;
				grdDetail.RootTable.Columns["Load"].ButtonText = "Load";
				grdDetail.RootTable.Columns["Load"].Width = 50;
				grdDetail.RootTable.Columns["Load"].Position = 0;
				grdDetail.FrozenColumns = 1;
				ctrlGrdBar4_Load(null, null);
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

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdDetail.CurrentColumn != null && e.Column.Key == "Load")
			{
				GetDataFromGPGrid(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDataFromGPGrid(GridEXRow item)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			RefDocumentTypeId = Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
			if (RefDocumentTypeId != 52 && Conversion.ToString((object)item.Cells["Status"].Text) != "Accepted")
			{
				throw new Exception("Record cannot be loaded because record status not Accepted");
			}
			if (grd.GetRows().Count() > 0)
			{
				if (MessageBox.Show("Are you sure to Load? If You Load, Already Loaded Data Will Erased", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				table.Rows.Clear();
			}
			txtNoofBranches.Text = Conversion.ToString(item.Cells["NoofBranches"].Value);
			if (RefDocumentTypeId == 51 && Conversion.ToInt((object)txtNoofBranches.Text) > 1)
			{
				((Control)(object)CmbWareHouseFrom).Enabled = false;
				((Control)(object)CmbWareHouseFrom).Text = string.Empty;
			}
			else if (RefDocumentTypeId == 91 && Conversion.ToInt((object)txtNoofBranches.Text) > 1)
			{
				((Control)(object)cmbWareHouseTo).Enabled = false;
				((Control)(object)cmbWareHouseTo).Text = string.Empty;
			}
			else if (RefDocumentTypeId == 52)
			{
				CmbTransferType.Value = 1;
				((Control)(object)CmbWareHouseFrom).Enabled = true;
				((Control)(object)cmbWareHouseTo).Enabled = true;
				((Control)(object)cmbGatePass).Text = string.Empty;
				txtOtherWeight.Text = string.Empty;
				ItemDetailFillForInwardandMoveOrder();
			}
			else if (RefDocumentTypeId == 74)
			{
				CmbTransferType.Value = 3;
				((Control)(object)CmbWareHouseFrom).Enabled = true;
				((Control)(object)cmbWareHouseTo).Enabled = true;
				((Control)(object)cmbGatePass).Text = string.Empty;
				txtOtherWeight.Text = string.Empty;
				GetSlipNoForMoveOrder();
				ItemDetailFillForInwardandMoveOrder();
				cmbTicketNo.Value = Conversion.ToInt(item.Cells["Id"].Value);
			}
			if (RefDocumentTypeId == 51)
			{
				CmbTransferType.Value = 1;
				DataTable dt3 = new DataTable();
				dt3.Columns.Add("Id");
				dt3.Columns.Add("GpNo");
				dt3.Columns.Add("RefDocId");
				dt3.Rows.Add(item.Cells["Id"].Value, item.Cells["GpNo"].Value, item.Cells["RefDocId"].Value);
				DDL.BindDDL(dt3, cmbGatePass, "Id", "GpNo", "GpNo", ZeroIndex: false);
				((UltraGridBase)cmbGatePass).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbGatePass).Rows[0].Activate();
				cmbGatePass_Leave(null, null);
				if (Conversion.ToInt((object)txtNoofBranches.Text) > 1)
				{
					BindDetailFromLoader();
				}
			}
			if (RefDocumentTypeId == 52)
			{
				CmbTransferType.Value = 1;
				DataTable dt2 = new DataTable();
				dt2.Columns.Add("Id");
				dt2.Columns.Add("GpNo");
				dt2.Columns.Add("RefDocId");
				dt2.Rows.Add(item.Cells["Id"].Value, item.Cells["GpNo"].Value, item.Cells["RefDocId"].Value);
				DDL.BindDDL(dt2, cmbGatePass, "Id", "GpNo", "GpNo", ZeroIndex: false);
				((UltraGridBase)cmbGatePass).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbGatePass).Rows[0].Activate();
				cmbGatePass_Leave(null, null);
			}
			if (RefDocumentTypeId == 91)
			{
				CmbTransferType.Value = 2;
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("GpNo");
				dt.Columns.Add("RefDocId");
				dt.Rows.Add(item.Cells["Id"].Value, item.Cells["GpNo"].Value, item.Cells["RefDocId"].Value);
				DDL.BindDDL(dt, cmbGatePass, "Id", "GpNo", "GpNo", ZeroIndex: false);
				((UltraGridBase)cmbGatePass).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbGatePass).Rows[0].Activate();
				cmbGatePass_Leave(null, null);
			}
			cmbTicketNo.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)CmbTransferType).Text == "Inward")
			{
				throw new Exception("You Can't Add Direct Record");
			}
			if (!FormValidationDetail())
			{
				return;
			}
			if (grd.GetRows().Length != 0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value) > 0 || Conversion.ToInt(r.Cells["RefDocNoId"].Value) > 0 || Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value) > 0)
					{
						throw new Exception("Record cannot be add in grid because some rows already exist in grid of Loader");
					}
				}
			}
			table.Rows.Add(false, 0, 0, 0, 0, cmbItemName.Value, ((Control)(object)cmbItemName).Text.Trim(), cmbPackingType.Value, ((Control)(object)cmbPackingType).Text.Trim(), ((Control)(object)cmbCropYear).Text.Trim(), cmbJobLot.Value, ((Control)(object)cmbJobLot).Text.Trim(), CmbJobLotTo.Value, ((Control)(object)CmbJobLotTo).Text.Trim(), Conversion.ToDouble((object)txtQty.Text.Trim()), CmbPackUom.Value, ((Control)(object)CmbPackUom).Text.Trim(), Conversion.ToDouble((object)txtGrossWeight.Text.Trim()), Conversion.ToDouble((object)txtEbUnit.Text.Trim()), Conversion.ToDouble((object)txtEbTotal.Text.Trim()), Conversion.ToDouble((object)txtAdLsWeight.Text.Trim()), Conversion.ToDouble((object)txtDetailNetWeight.Text.Trim()), 0, 0, CmbWareHouseFrom.Value, ((Control)(object)CmbWareHouseFrom).Text.Trim(), cmbWareHouseTo.Value, ((Control)(object)cmbWareHouseTo).Text.Trim(), txtRemarksDetail.Text.Trim(), txtAvgRate.Text, 0, 40, txtAmount.Text, 0);
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			OtherExpensesProportion();
			ResetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grdSettings()
	{
		try
		{
			grd.ColumnAutoResize = false;
			GridEXRow item = grd.CurrentRow;
			if (item != null && Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) == 0)
			{
				grd.RootTable.Columns["BalQty"].Visible = false;
				grd.RootTable.Columns["BalWeight"].Visible = false;
			}
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["FlagForSplitAndDelete"].Visible = false;
			grd.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grd.RootTable.Columns["RefDocNoId"].Visible = false;
			grd.RootTable.Columns["RefDocSubIdNo"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["PackTypeId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["PackUOMId"].Visible = false;
			grd.RootTable.Columns["WareHouseFromId"].Visible = false;
			grd.RootTable.Columns["WareHouseToId"].Visible = false;
			grd.RootTable.Columns["RateUOMId"].Visible = false;
			grd.RootTable.Columns["JobLotIdTo"].Visible = false;
			grd.RootTable.Columns["TransferDocumentTypeId"].Visible = false;
			grd.RootTable.Columns["TransferId"].Visible = false;
			grd.RootTable.Columns["TransferDetailId"].Visible = false;
			if (!StockTransferFinancialEffectIsActive)
			{
				grd.RootTable.Columns["ItemRate"].Visible = false;
				grd.RootTable.Columns["ItemAmount"].Visible = false;
				grd.RootTable.Columns["Expense"].Visible = false;
				grd.RootTable.Columns["RateUOM"].Visible = false;
			}
			if (Conversion.ToInt(CmbTransferType.Value) == 1 && Conversion.ToInt((object)txtNoofBranches.Text) > 1)
			{
				grd.RootTable.Columns["WareHouseFrom"].Visible = false;
			}
			if (Conversion.ToInt(CmbTransferType.Value) == 2 && Conversion.ToInt((object)txtNoofBranches.Text) > 1)
			{
				grd.RootTable.Columns["WareHouseTo"].Visible = false;
			}
			if (Conversion.ToInt((object)txtNoofBranches.Text) <= 1)
			{
				grd.RootTable.Columns["WareHouseFrom"].Visible = true;
				grd.RootTable.Columns["WareHouseTo"].Visible = true;
			}
			grd.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BalWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["QTY"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EbUnit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EbTotal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Ad/LsWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Expense"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BalWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["QTY"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EbUnit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EbTotal"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Ad/LsWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Expense"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["PackUOM"].FormatString = "#,#.###";
			grd.RootTable.Columns["ItemRate"].FormatString = "#,#.###";
			grd.RootTable.Columns["BalQty"].FormatString = "#,#";
			grd.RootTable.Columns["BalWeight"].FormatString = "#,#.##";
			grd.RootTable.Columns["QTY"].FormatString = "#,#";
			grd.RootTable.Columns["GrossWeight"].FormatString = "#,#.##";
			grd.RootTable.Columns["EbUnit"].FormatString = "#,#.##";
			grd.RootTable.Columns["EbTotal"].FormatString = "#,#.##";
			grd.RootTable.Columns["Ad/LsWeight"].FormatString = "#,#.##";
			grd.RootTable.Columns["NetWeight"].FormatString = "#,#.##";
			grd.RootTable.Columns["ItemAmount"].FormatString = "#,#.###";
			grd.RootTable.Columns["ItemRate"].TotalFormatString = "#,#.###";
			grd.RootTable.Columns["BalQty"].TotalFormatString = "#,#";
			grd.RootTable.Columns["BalWeight"].TotalFormatString = "#,#.##";
			grd.RootTable.Columns["QTY"].TotalFormatString = "#,#";
			grd.RootTable.Columns["GrossWeight"].TotalFormatString = "#,#.##";
			grd.RootTable.Columns["EbUnit"].TotalFormatString = "#,#.##";
			grd.RootTable.Columns["EbTotal"].TotalFormatString = "#,#.##";
			grd.RootTable.Columns["Ad/LsWeight"].TotalFormatString = "#,#.##";
			grd.RootTable.Columns["NetWeight"].TotalFormatString = "#,#.##";
			grd.RootTable.Columns["ItemAmount"].TotalFormatString = "#,#.###";
			grd.RootTable.Columns["Expense"].TotalFormatString = "#,#.###";
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 20;
			grd.RootTable.Columns["Delete"].Position = 0;
			if (((Control)(object)CmbTransferType).Text == "Inward")
			{
				grd.RootTable.Columns.Add("Add");
				grd.RootTable.Columns["Add"].Key = "Add";
				grd.RootTable.Columns["Add"].Caption = "+";
				grd.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grd.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
				grd.RootTable.Columns["Add"].ButtonText = "+";
				grd.RootTable.Columns["Add"].Width = 20;
				grd.RootTable.Columns["Add"].Position = 1;
				grd.FrozenColumns = 2;
			}
			else
			{
				grd.FrozenColumns = 1;
			}
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				updateDetailIndex = item.RowIndex;
				if (Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) > 0)
				{
					((Control)(object)CmbWareHouseFrom).Enabled = false;
					((Control)(object)cmbItemName).Enabled = false;
					((Control)(object)CmbPackUom).Enabled = false;
					((Control)(object)cmbCropYear).Enabled = false;
					((Control)(object)cmbJobLot).Enabled = false;
				}
				else
				{
					txtAvgRate.Text = item.Cells["ItemRate"].Value.ToString();
					txtAmount.Text = item.Cells["ItemAmount"].Value.ToString();
					((Control)(object)CmbWareHouseFrom).Enabled = true;
					((Control)(object)cmbItemName).Enabled = true;
					((Control)(object)CmbPackUom).Enabled = true;
					((Control)(object)cmbCropYear).Enabled = true;
					((Control)(object)cmbJobLot).Enabled = true;
				}
				((Control)(object)CmbWareHouseFrom).Text = item.Cells["WareHouseFrom"].Value.ToString();
				CmbWareHouseFrom.Value = item.Cells["WareHouseFromId"].Value.ToString();
				if (Conversion.ToInt(item.Cells["WareHouseFromId"].Value) == 0)
				{
					((Control)(object)CmbWareHouseFrom).Enabled = false;
					((Control)(object)CmbWareHouseFrom).Text = string.Empty;
				}
				((Control)(object)cmbWareHouseTo).Text = item.Cells["WareHouseTo"].Value.ToString();
				cmbWareHouseTo.Value = item.Cells["WareHouseToId"].Value.ToString();
				if (((Control)(object)CmbTransferType).Text == "MoveOrder" || RefDocumentTypeId == 52)
				{
					ItemDetailFillForInwardandMoveOrder();
				}
				cmbItemName.Value = item.Cells["ItemId"].Value.ToString();
				if (btnupdate.Visible && btnupdate.Enabled && ((Control)(object)CmbTransferType).Text != "MoveOrder" && RefDocumentTypeId != 52)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("ItemId");
					dt.Columns.Add("ItemName");
					dt.Rows.Add(item.Cells["ItemId"].Value, item.Cells["ItemName"].Value);
					DDL.BindDDL(dt, cmbItemName, "ItemId", "ItemName", "ItemName", ZeroIndex: false);
					((UltraGridBase)cmbItemName).Rows[0].Activate();
				}
				cmbPackingType.Value = item.Cells["PackTypeId"].Value.ToString();
				((Control)(object)cmbPackingType).Text = item.Cells["PackType"].Text.ToString();
				((Control)(object)cmbCropYear).Text = item.Cells["CropYear"].Value.ToString();
				cmbJobLot.Value = item.Cells["JobLotId"].Value.ToString();
				((Control)(object)cmbJobLot).Text = item.Cells["JobLot"].Value.ToString();
				CmbJobLotTo.Value = item.Cells["JobLotIdTo"].Value.ToString();
				((Control)(object)CmbJobLotTo).Text = item.Cells["JobLotTo"].Value.ToString();
				txtQty.Text = item.Cells["QTY"].Value.ToString();
				bindRateUomAndItemPackUom(Conversion.ToInt(cmbItemName.Value));
				CmbPackUom.Value = Conversion.ToInt(item.Cells["PackUOMId"].Value);
				txtGrossWeight.Text = item.Cells["GrossWeight"].Value.ToString();
				txtEbUnit.Text = item.Cells["EbUnit"].Value.ToString();
				txtEbTotal.Text = item.Cells["EbTotal"].Value.ToString();
				txtAdLsWeight.Text = item.Cells["Ad/LsWeight"].Value.ToString();
				txtDetailNetWeight.Text = item.Cells["NetWeight"].Value.ToString();
				txtRemarksDetail.Text = item.Cells["Remarks"].Value.ToString();
				if (((Control)(object)CmbTransferType).Text == "Inward")
				{
					cmbWareHouseTo.Focus();
				}
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				table.Rows[updateDetailIndex]["ItemId"] = cmbItemName.Value;
				table.Rows[updateDetailIndex]["ItemName"] = ((Control)(object)cmbItemName).Text;
				table.Rows[updateDetailIndex]["PackTypeId"] = cmbPackingType.Value;
				table.Rows[updateDetailIndex]["PackType"] = ((Control)(object)cmbPackingType).Text;
				table.Rows[updateDetailIndex]["CropYear"] = ((Control)(object)cmbCropYear).Text;
				table.Rows[updateDetailIndex]["JobLotId"] = cmbJobLot.Value;
				table.Rows[updateDetailIndex]["JobLot"] = ((Control)(object)cmbJobLot).Text;
				table.Rows[updateDetailIndex]["JobLotIdTo"] = CmbJobLotTo.Value;
				table.Rows[updateDetailIndex]["JobLotTo"] = ((Control)(object)CmbJobLotTo).Text;
				table.Rows[updateDetailIndex]["QTY"] = Conversion.ToDouble((object)txtQty.Text);
				table.Rows[updateDetailIndex]["PackUOMId"] = CmbPackUom.Value;
				table.Rows[updateDetailIndex]["PackUOM"] = ((Control)(object)CmbPackUom).Text;
				table.Rows[updateDetailIndex]["GrossWeight"] = Conversion.ToDouble((object)txtGrossWeight.Text);
				table.Rows[updateDetailIndex]["EbUnit"] = Conversion.ToDouble((object)txtEbUnit.Text);
				table.Rows[updateDetailIndex]["EbTotal"] = Conversion.ToDouble((object)txtEbTotal.Text);
				table.Rows[updateDetailIndex]["Ad/LsWeight"] = Conversion.ToDouble((object)txtAdLsWeight.Text);
				table.Rows[updateDetailIndex]["NetWeight"] = Conversion.ToDouble((object)txtDetailNetWeight.Text);
				table.Rows[updateDetailIndex]["WareHouseFromId"] = Conversion.ToInt(CmbWareHouseFrom.Value);
				table.Rows[updateDetailIndex]["WareHouseFrom"] = Conversion.ToString((object)((Control)(object)CmbWareHouseFrom).Text);
				table.Rows[updateDetailIndex]["WareHouseToId"] = Conversion.ToInt(cmbWareHouseTo.Value);
				table.Rows[updateDetailIndex]["WareHouseTo"] = Conversion.ToString((object)((Control)(object)cmbWareHouseTo).Text);
				table.Rows[updateDetailIndex]["Remarks"] = txtRemarksDetail.Text;
				if (Conversion.ToInt(table.Rows[updateDetailIndex]["RefDocumentTypeId"]) == 0)
				{
					table.Rows[updateDetailIndex]["ItemRate"] = txtAvgRate.Text;
					table.Rows[updateDetailIndex]["ItemAmount"] = txtAmount.Text;
				}
				grd.DataSource = table;
				btnplus.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				ItemAmountCalculation();
				ResetDetail();
				_ = ((Control)(object)CmbTransferType).Text == "Outward";
				OtherChargesAmountUpdate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			int RowId = Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Column.Key == "Add")
			{
				DataRow dr = table.Rows[r.RowIndex];
				table.Rows.Add(dr.ItemArray);
				grd.DataSource = table;
				table.Rows[table.Rows.Count - 1]["FlagForSplitAndDelete"] = false;
			}
			else if (e.Column.Key == "Delete")
			{
				if (Conversion.ToBool(r.Cells["FlagForSplitAndDelete"].Value))
				{
					throw new Exception("You Can't Delete This Row");
				}
				if (RowId != 0)
				{
					throw new Exception("You Can't Delete This Row");
				}
				r.Delete();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		btnplus.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
	}

	private void Insert()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Expected O, but got Unknown
		//IL_0921: Unknown result type (might be due to invalid IL or missing references)
		//IL_0928: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (grd.GetRows().Count() == 0)
			{
				MessageBox.Show("Grid Record Not Found");
				return;
			}
			InvStockTransferHeader sh = new InvStockTransferHeader();
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				sh.Id = Id;
				sh.ModifyUser = UserAccount.ID;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			GridEXRow[] rows = grdOtherCharges.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				if (!(Conversion.ToDouble(r3.Cells["Amount"].Value) > 0.0))
				{
					continue;
				}
				if (Conversion.ToInt(r3.Cells["Account"].Value) == 0)
				{
					throw new Exception("Please Select an Account Against OtherCharges Amount First");
				}
				if (Conversion.ToDouble(r3.Cells["Amount"].Value) > 0.0)
				{
					string FrRemarks = Conversion.ToString(r3.Cells["Remarks"].Value);
					if (FrRemarks == string.Empty || FrRemarks == null)
					{
						tabControl2.SelectedIndex = 1;
						((Control)(object)grdOtherCharges).Focus();
						throw new Exception("Grid Charge to Product Remarks required Please Check");
					}
				}
			}
			sh.DocumentTypeId = 68;
			sh.BranchesId = UserAccount.BranchesId;
			sh.DocDate = Conversion.ToDateTime((object)DocDate.Value);
			sh.DocNo = Conversion.ToInt((object)txtdocno.Text);
			sh.NoOfBranches = Conversion.ToInt((object)txtNoofBranches.Text);
			sh.RemarksHeader = Conversion.ToString((object)txtRemarksHead.Text);
			sh.GatePassId = Conversion.ToInt(cmbGatePass.Value);
			sh.WbTicketId = Conversion.ToInt(cmbTicketNo.Value);
			sh.WbNetWeight = Conversion.ToDouble((object)txtHeadNetWeight.Text);
			sh.OtherWeight = Conversion.ToDouble((object)txtOtherWeight.Text);
			sh.TransferType = Conversion.ToString((object)((Control)(object)CmbTransferType).Text);
			sh.OrganizationId = UserAccount.OrganizationId;
			sh.CompanyId = UserAccount.CompanyId;
			sh.EntryUser = UserAccount.ID;
			sh.ModifyUser = UserAccount.ID;
			sh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			sh.RefDocumentTypeId = RefDocumentTypeId;
			sh.InvStockTransferDetail = new List<InvStockTransferDetail>();
			sh.InvStockTransferExpense = new List<InvStockTransferExpense>();
			double GrossWeight = 0.0;
			rows = grd.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				InvStockTransferDetail vd = new InvStockTransferDetail();
				vd.Id = Conversion.ToInt(r2.Cells["Id"].Value);
				vd.RefDocumentTypeId = Conversion.ToInt(r2.Cells["RefDocumentTypeId"].Value);
				vd.RefDocNoId = Conversion.ToInt(r2.Cells["RefDocNoId"].Value);
				vd.RefDocSubIdNo = Conversion.ToInt(r2.Cells["RefDocSubIdNo"].Value);
				if (vd.RefDocumentTypeId > 0 && Conversion.ToDouble(r2.Cells["NetWeight"].Value) > Conversion.ToDouble(r2.Cells["BalWeight"].Value))
				{
					throw new Exception("NetWeight cannot be greater than BalWeight Please Check!");
				}
				vd.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
				if (((Control)(object)CmbTransferType).Text == "Outward" && dtItemsFromDO.Rows.Count > 0 && !dtItemsFromDO.AsEnumerable().Any((DataRow row) => row.Field<int>("ItemId") == vd.ItemId))
				{
					throw new Exception(string.Format("Item {0} in Detail Grid Row No : {1} ", Conversion.ToString((object)r2.Cells["ItemName"].Text), r2.RowIndex + 1) + "does not Exists in Delivery Order");
				}
				vd.InvPackingTypeId = Conversion.ToInt(r2.Cells["PackTypeId"].Value);
				vd.CropYear = Conversion.ToString(r2.Cells["CropYear"].Value);
				vd.JobLotId = Conversion.ToInt(r2.Cells["JobLotId"].Value);
				vd.JobLotIdTo = Conversion.ToInt(r2.Cells["JobLotIdTo"].Value);
				vd.Qty = Conversion.ToDouble(r2.Cells["QTY"].Value);
				vd.PackUomId = Conversion.ToInt(r2.Cells["PackUOMId"].Value);
				vd.GrossWeight = Conversion.ToDouble(r2.Cells["GrossWeight"].Value);
				GrossWeight += vd.GrossWeight;
				vd.AdLsWeight = Conversion.ToDouble(r2.Cells["Ad/LsWeight"].Value);
				vd.EbUnit = Conversion.ToDouble(r2.Cells["EbUnit"].Value);
				vd.EbTotal = Conversion.ToDouble(r2.Cells["EbTotal"].Value);
				vd.NetWeight = Conversion.ToDouble(r2.Cells["NetWeight"].Value);
				vd.ItemRate = Conversion.ToDouble(r2.Cells["ItemRate"].Value);
				vd.RateUomId = Conversion.ToInt(r2.Cells["RateUOMId"].Value);
				vd.ItemAmount = Conversion.ToDouble(r2.Cells["ItemAmount"].Value);
				vd.ExpenseAmount = Conversion.ToDouble(r2.Cells["Expense"].Value);
				vd.ToWarehouseId = Conversion.ToInt(r2.Cells["WareHouseToId"].Value);
				vd.FromWarehouseId = Conversion.ToInt(r2.Cells["WareHouseFromId"].Value);
				vd.TransferDocumentTypeId = Conversion.ToInt(r2.Cells["TransferDocumentTypeId"].Value);
				vd.TransferId = Conversion.ToInt(r2.Cells["TransferId"].Value);
				vd.TransferDetailId = Conversion.ToInt(r2.Cells["TransferDetailId"].Value);
				if (sh.NoOfBranches > 1 && sh.TransferType == "Inward" && vd.ToWarehouseId == 0)
				{
					throw new Exception("WarehouseTo Field is Required.");
				}
				if (sh.TransferType == "MoveOrder" || RefDocumentTypeId == 52)
				{
					if (vd.FromWarehouseId == 0)
					{
						throw new Exception("FromWarehouse Field is Required.");
					}
					if (vd.ToWarehouseId == 0)
					{
						throw new Exception("WarehouseTo Field is Required.");
					}
				}
				vd.RemarksDetail = Conversion.ToString((object)r2.Cells["Remarks"].Text);
				if (StockTransferFinancialEffectIsActive)
				{
					if (vd.ItemRate <= 0.0)
					{
						throw new Exception("AvgRate field required");
					}
					if (vd.ItemAmount <= 0.0)
					{
						throw new Exception("Amount field required");
					}
				}
				sh.InvStockTransferDetail.Add(vd);
			}
			rows = grdOtherCharges.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt((object)r.Cells["Account"].Value.ToString()) != 0)
				{
					InvStockTransferExpense PF = new InvStockTransferExpense();
					PF.AccountId = Conversion.ToInt(r.Cells["Account"].Value);
					PF.Percentage = Conversion.ToDouble(r.Cells["Percentage"].Value);
					PF.Qty = Conversion.ToDouble(r.Cells["Qty"].Value);
					PF.Rate = Conversion.ToDouble(r.Cells["Rate"].Value);
					PF.Amount = Conversion.ToDouble(r.Cells["Amount"].Value);
					PF.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
					sh.InvStockTransferExpense.Add(PF);
				}
			}
			if (Conversion.ToDouble((object)txtHeadNetWeight.Text.Trim()) != GrossWeight)
			{
				MessageBox.Show("GrossWeight and WeighBridge Weight Not Match", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			sh.AttachmentsList = new List<DMSAttachments>();
			sh.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvStockTransferHeader>(sh, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = InvStockTransferHeader.Save(sh);
			if (Id > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + sh.DocNo);
				FormHelper.DeleteAllAttachmentsFromDb(AT, Id, 68, base.Name);
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + sh.DocNo);
			}
			if (WagesStatus && WagesActiveOrInActive)
			{
				frmwagesBillHeader frmwagesBillHeader = new frmwagesBillHeader(UserAccount);
				frmwagesBillHeader.RefDocTypeId = 68;
				frmwagesBillHeader.RefDocId = success;
				frmwagesBillHeader.GrossWeightTotal = Conversion.ToDouble((object)txtHeadNetWeight.Text);
				frmwagesBillHeader.ShowDialog();
				frmwagesBillHeader.BringToFront();
			}
			Reset();
			if (ChkBoxPrintPreview.Checked)
			{
				CommonServices.StockTransferSlip406(success);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			Id = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (IsApproved)
			{
				MessageBox.Show("Approved Record Not Update");
			}
			else
			{
				Insert();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int ID)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		try
		{
			Reset();
			Id = ID;
			InvStockTransferHeader Stock = new InvStockTransferHeader();
			Stock = InvStockTransferHeader.GetByID(ID);
			if (Stock != null)
			{
				tabControl1.SelectedIndex = 0;
				RefDocumentTypeId = Stock.RefDocumentTypeId;
				DocDate.Value = Conversion.ToDateTime((object)Stock.DocDate);
				txtdocno.Text = Conversion.ToString((object)Stock.DocNo);
				txtNoofBranches.Text = Conversion.ToString((object)Stock.NoOfBranches);
				txtRemarksHead.Text = Conversion.ToString((object)Stock.RemarksHeader);
				if (Stock.TransferType != "MoveOrder")
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("Id");
					dt.Columns.Add("GatePassNo");
					dt.Rows.Add(Stock.GatePassId, Stock.GpSrNo);
					DDL.BindDDL(dt, cmbGatePass, "Id", "GatePassNo", "GpNo", ZeroIndex: false);
					((UltraGridBase)cmbGatePass).Rows[0].Activate();
					DataTable dtTicket = new DataTable();
					dtTicket.Columns.Add("Id");
					dtTicket.Columns.Add("TicketNo");
					dtTicket.Rows.Add(Stock.WbTicketId, Stock.TicketNo);
					DDL.BindDDL(dtTicket, cmbTicketNo, "Id", "TicketNo", "TicketNo", ZeroIndex: false);
					((UltraGridBase)cmbTicketNo).Rows[0].Activate();
				}
				else
				{
					DataTable dtTicket2 = new DataTable();
					dtTicket2.Columns.Add("Id");
					dtTicket2.Columns.Add("TicketNo");
					dtTicket2.Rows.Add(Stock.WbTicketId, Stock.TicketNo);
					DDL.BindDDL(dtTicket2, cmbTicketNo, "Id", "TicketNo", "TicketNo", ZeroIndex: false);
					((UltraGridBase)cmbTicketNo).Rows[0].Activate();
					((UltraGridBase)cmbGatePass).DataSource = null;
					((Control)(object)cmbGatePass).Text = string.Empty;
				}
				txtHeadNetWeight.Text = Conversion.ToString((object)Stock.WbNetWeight);
				txtOtherWeight.Text = Conversion.ToString((object)Stock.OtherWeight);
				((Control)(object)CmbTransferType).Text = Conversion.ToString((object)Stock.TransferType);
				txtWorkingReportNo.Text = Conversion.ToString((object)Stock.WorkingReportNo);
				IsApproved = Stock.IsApproved;
				table.Clear();
				for (int j = 0; j < Stock.InvStockTransferDetail.Count; j++)
				{
					bool flag = ((Stock.RefDocumentTypeId == 51 && Stock.NoOfBranches > 1) ? true : false);
					table.Rows.Add(flag, Stock.InvStockTransferDetail[j].Id, Stock.InvStockTransferDetail[j].RefDocumentTypeId, Stock.InvStockTransferDetail[j].RefDocNoId, Stock.InvStockTransferDetail[j].RefDocSubIdNo, Stock.InvStockTransferDetail[j].ItemId, Stock.InvStockTransferDetail[j].ItemName, Stock.InvStockTransferDetail[j].InvPackingTypeId, Stock.InvStockTransferDetail[j].PackTypeDesc, Stock.InvStockTransferDetail[j].CropYear, Stock.InvStockTransferDetail[j].JobLotId, Stock.InvStockTransferDetail[j].JobLotDescription, Stock.InvStockTransferDetail[j].JobLotIdTo, Stock.InvStockTransferDetail[j].JobLotTo, Stock.InvStockTransferDetail[j].Qty, Stock.InvStockTransferDetail[j].PackUomId, Stock.InvStockTransferDetail[j].Equivalent, Stock.InvStockTransferDetail[j].GrossWeight, Stock.InvStockTransferDetail[j].EbUnit, Stock.InvStockTransferDetail[j].EbTotal, Stock.InvStockTransferDetail[j].AdLsWeight, Stock.InvStockTransferDetail[j].NetWeight, Stock.InvStockTransferDetail[j].Qty, Stock.InvStockTransferDetail[j].NetWeight, Stock.InvStockTransferDetail[j].FromWarehouseId, Stock.InvStockTransferDetail[j].FromWareHouseName, Stock.InvStockTransferDetail[j].ToWarehouseId, Stock.InvStockTransferDetail[j].ToWareHouseName, Stock.InvStockTransferDetail[j].RemarksDetail, Stock.InvStockTransferDetail[j].ItemRate, Stock.InvStockTransferDetail[j].RateUomId, Stock.InvStockTransferDetail[j].RateUom, Stock.InvStockTransferDetail[j].ItemAmount, Stock.InvStockTransferDetail[j].ExpenseAmount, Stock.InvStockTransferDetail[j].TransferDocumentTypeId, Stock.InvStockTransferDetail[j].TransferId, Stock.InvStockTransferDetail[j].TransferDetailId);
				}
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				dtExp.Rows.Clear();
				for (int i = 0; i < Stock.InvStockTransferExpense.Count; i++)
				{
					dtExp.Rows.Add(Stock.InvStockTransferExpense[i].AccountId, Stock.InvStockTransferExpense[i].Percentage, Stock.InvStockTransferExpense[i].Qty, Stock.InvStockTransferExpense[i].Rate, Stock.InvStockTransferExpense[i].Amount, Stock.InvStockTransferExpense[i].Remarks);
				}
				if (dtExp.Rows.Count == 0)
				{
					AddRowInExpenseGrid();
				}
				grdOtherCharges.DataSource = dtExp;
				grdOtherCharges.RetrieveStructure();
				grdExpenseSettings();
				btnsave.Visible = false;
				btnupdate.Visible = true;
				btnDelete.Visible = formrights.DoHaveCanDelete;
				FormHelper.LoadAttachmentsForObject(AT, Id, base.Name);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
		ResetDetail();
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			Id = 0;
			RefDocumentTypeId = 0;
			IsApproved = false;
			((Control)(object)cmbCropYear).Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			((Control)(object)cmbGatePass).Text = string.Empty;
			((Control)(object)cmbTicketNo).Text = string.Empty;
			((UltraGridBase)cmbTicketNo).DataSource = null;
			txtHeadNetWeight.Text = string.Empty;
			txtOtherWeight.Text = string.Empty;
			((Control)(object)cmbPackingType).Text = string.Empty;
			txtRemarksHead.Text = string.Empty;
			txtWorkingReportNo.Text = string.Empty;
			txtAvgRate.Text = string.Empty;
			txtAmount.Text = string.Empty;
			txtNoofBranches.Text = string.Empty;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			btnDelete.Visible = false;
			dtExp.Rows.Clear();
			AddRowInExpenseGrid();
			grdOtherCharges.RetrieveStructure();
			grdExpenseSettings();
			((Control)(object)CmbWareHouseFrom).Enabled = true;
			((Control)(object)cmbWareHouseTo).Enabled = true;
			((UltraGridBase)cmbItemName).DataSource = null;
			((Control)(object)cmbItemName).Enabled = true;
			((Control)(object)CmbPackUom).Enabled = true;
			((Control)(object)cmbCropYear).Enabled = true;
			((Control)(object)cmbJobLot).Enabled = true;
			DocumentNoFill();
			table.Rows.Clear();
			grd.DataSource = table;
			CmbTransferType_Leave(null, null);
			PendingRecordForStockTransfer();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetDetail()
	{
		try
		{
			cmbItemName.Focus();
			updateDetailIndex = -1;
			((Control)(object)cmbCropYear).Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtDetailNetWeight.Text = string.Empty;
			((Control)(object)cmbItemName).Text = string.Empty;
			((Control)(object)cmbJobLot).Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			txtAdLsWeight.Text = string.Empty;
			((Control)(object)CmbWareHouseFrom).Text = string.Empty;
			((Control)(object)cmbWareHouseTo).Text = string.Empty;
			txtQty.Text = string.Empty;
			((Control)(object)cmbPackingType).Text = string.Empty;
			txtRemarksDetail.Text = string.Empty;
			((Control)(object)CmbJobLotTo).Text = string.Empty;
			txtAvgRate.Text = string.Empty;
			txtAmount.Text = string.Empty;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			WagesRefDocumentsStatus obj = clsGlobalVariables.WagesRefDocumentsStatusList.Find((WagesRefDocumentsStatus row) => row.RefDocumentTypeId == 68);
			WagesActiveOrInActive = obj != null && obj.IsActive;
			ItemDetailFill();
			WareHouseFill();
			CombGatePassNoFill();
			GetPackingType();
			GetCropYear();
			GetJobLot();
			bindRateUomAndItemPackUom(Conversion.ToInt(cmbItemName.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmStockTransfer_FormClosing(object sender, FormClosingEventArgs e)
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

	private void PurchsaeOrder_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					DocDate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					FromDateHistory.Focus();
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnupdate.Visible && btnupdate.Enabled)
				{
					btnupdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					DocDate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					cmbWareHouseTo.Focus();
				}
			}
			else
			{
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnshow_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNewHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					FromDateHistory.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Return)
				{
					Id = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
					ReadById(Id);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShortCutkeys_Click(object sender, EventArgs e)
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
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On WareHouse To Combo in Detail Box");
			dt.Rows.Add("Ctrl+Enter", "For Update Record When Focus On Any Grid ");
			dt.Rows.Add("Ctrl+Space", "To Call Function's On Button Or Link When Focus On Any Grid ");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			int RowId = Conversion.ToInt(r.Cells["Id"].Value);
			if (grd.CurrentColumn.Key == "Delete")
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				if (Conversion.ToBool(r.Cells["FlagForSplitAndDelete"].Value))
				{
					throw new Exception("You Can't Delete This Row");
				}
				if (RowId != 0)
				{
					throw new Exception("You Can't Delete This Row");
				}
				r.Delete();
			}
			if (grd.CurrentColumn.Key == "Add")
			{
				DataRow dr = table.Rows[r.RowIndex];
				table.Rows.Add(dr.ItemArray);
				grd.DataSource = table;
				table.Rows[table.Rows.Count - 1]["FlagForSplitAndDelete"] = false;
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdhistory.CurrentColumn != null && e.Control && e.KeyCode == Keys.Space)
			{
				int CurrId = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdhistory.CurrentColumn.Key == "Edit" && formrights.DoHaveUpdateRights)
				{
					ReadById(CurrId);
				}
				if (grdhistory.CurrentColumn.Key == "Print" && formrights.DoHavePrintRights)
				{
					CommonServices.StockTransferSlip406(CurrId);
				}
				if (grdhistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByScreenName(CurrId, base.Name);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdDetail.CurrentColumn != null && e.Control && e.KeyCode == Keys.Space)
			{
				if (grdDetail.CurrentColumn.Key == "Load")
				{
					GetDataFromGPGrid(r);
				}
				grdDetail.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				FromDateHistory.Focus();
			}
			else
			{
				DocDate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
			txtFromDocNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			((Control)(object)cmbTransactiontypeHistory).Text = string.Empty;
			grdhistory.ClearStructure();
			grdDetail.ClearStructure();
			drdocdate.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryComboFill();
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
			gridhistoryfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryComboFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeIds = "68"
			};
			DataTable dtTransferType = new DataTable
			{
				Columns = 
				{
					{
						"Id",
						typeof(int)
					},
					{
						"TransferType",
						typeof(string)
					}
				}
			};
			DataTable dt = InvStockTransferHeader.AllComboAgainstStockTransfer(val);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "TransferType")
					{
						dtTransferType.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtTransferType, cmbTransactiontypeHistory, "Id", "TransferType", "TransferType", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridhistoryfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 68;
			obj.CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
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
			obj.FromDocNo = Conversion.ToInt((object)txtFromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			obj.TransferType = Conversion.ToString((object)((Control)(object)cmbTransactiontypeHistory).Text);
			DataTable dthistoryfill = InvStockTransferHeader.FormHistory(obj);
			if (dthistoryfill.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("TransferType", typeof(string));
				dt.Columns.Add("WorkingReportNo", typeof(string));
				dt.Columns.Add("GatePassId", typeof(int));
				dt.Columns.Add("WbTicketId", typeof(int));
				dt.Columns.Add("GpSrNo", typeof(int));
				dt.Columns.Add("TicketNo");
				dt.Columns.Add("WbNetWeight", typeof(double));
				dt.Columns.Add("OtherWeight", typeof(double));
				dt.Columns.Add("DiffWeight", typeof(double));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("ModifyDate");
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				dt.Columns.Add("RemarksHeader", typeof(string));
				for (int i = 0; i < dthistoryfill.Rows.Count; i++)
				{
					dt.Rows.Add(dthistoryfill.Rows[i]["Id"], dthistoryfill.Rows[i]["DocNo"], Conversion.ToDateTime(dthistoryfill.Rows[i]["DocDate"]).ToShortDateString(), dthistoryfill.Rows[i]["TransferType"], dthistoryfill.Rows[i]["WorkingReportNo"], dthistoryfill.Rows[i]["GatePassId"], dthistoryfill.Rows[i]["WbTicketId"], dthistoryfill.Rows[i]["GpSrNo"], dthistoryfill.Rows[i]["TicketNo"], dthistoryfill.Rows[i]["WbNetWeight"], dthistoryfill.Rows[i]["OtherWeight"], dthistoryfill.Rows[i]["DiffWeight"], Conversion.ToDateTime(dthistoryfill.Rows[i]["EntryDate"]).ToShortDateString(), dthistoryfill.Rows[i]["EntryUserName"], Conversion.CheckDateTimeNull(Conversion.ToDateTime(dthistoryfill.Rows[i]["ModifyDate"])) ? "" : Conversion.ToDateTime(dthistoryfill.Rows[i]["EntryDate"]).ToShortDateString(), dthistoryfill.Rows[i]["ModifyUserName"], dthistoryfill.Rows[i]["NoOfAttachments"], dthistoryfill.Rows[i]["RemarksHeader"]);
				}
				grdhistory.DataSource = dt;
				grdhistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				grdhistory.ClearStructure();
				GridDetailHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSettings()
	{
		try
		{
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns["GatePassId"].Visible = false;
			grdhistory.RootTable.Columns["WbTicketId"].Visible = false;
			grdhistory.RootTable.Columns["WbNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["WbNetWeight"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["WbNetWeight"].FormatString = "#,##0.###";
			grdhistory.RootTable.Columns["WbNetWeight"].TotalFormatString = "#,##0.###";
			grdhistory.RootTable.Columns["OtherWeight"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["OtherWeight"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["OtherWeight"].FormatString = "#,##0.###";
			grdhistory.RootTable.Columns["OtherWeight"].TotalFormatString = "#,##0.###";
			grdhistory.RootTable.Columns["DiffWeight"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["DiffWeight"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["DiffWeight"].FormatString = "#,##0.###";
			grdhistory.RootTable.Columns["DiffWeight"].TotalFormatString = "#,##0.###";
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			CommonServices.GridAutoAdjustmentNew(grdhistory);
			grdhistory.RootTable.Columns.Add("Edit");
			grdhistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdhistory.RootTable.Columns["Edit"].Width = 50;
			grdhistory.RootTable.Columns["Edit"].Position = 0;
			grdhistory.RootTable.Columns.Add("Print");
			grdhistory.RootTable.Columns["Print"].Caption = "Print";
			grdhistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdhistory.RootTable.Columns["Print"].Width = 50;
			grdhistory.RootTable.Columns["Print"].Position = 1;
			grdhistory.FrozenColumns = 2;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdhistory_DoubleClick_1(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				ReadById(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				if (e.Column.Key == "Edit")
				{
					Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
					ReadById(Id);
				}
				if (e.Column.Key == "Print")
				{
					CommonServices.StockTransferSlip406(Conversion.ToInt((object)r.Cells["Id"].Value.ToString()));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		try
		{
			GridEXRow row = grdhistory.CurrentRow;
			if (row == null || (int)row.RowType != 0 || row.Cells.Count <= 0)
			{
				return;
			}
			int num = Conversion.ToInt(row.Cells["Id"].Value);
			InvStockTransferHeader obj = new InvStockTransferHeader();
			obj = InvStockTransferHeader.GetByID(num);
			if (obj.InvStockTransferDetail != null && obj.InvStockTransferDetail.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("FromWarehouse");
				dt.Columns.Add("ToWarehouse");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("PackingType");
				dt.Columns.Add("CropYear");
				dt.Columns.Add("JobLotFrom");
				dt.Columns.Add("JobLotTo");
				dt.Columns.Add("Qty", typeof(double));
				dt.Columns.Add("PackUom");
				dt.Columns.Add("GrossWeight", typeof(double));
				dt.Columns.Add("EbUnit", typeof(double));
				dt.Columns.Add("EbTotal", typeof(double));
				dt.Columns.Add("AddLessWt", typeof(double));
				dt.Columns.Add("StockWeight", typeof(double));
				dt.Columns.Add("ItemRate", typeof(double));
				dt.Columns.Add("ItemAmount", typeof(double));
				dt.Columns.Add("ExpenseAmount", typeof(double));
				dt.Columns.Add("RemarksDetail");
				for (int i = 0; i < obj.InvStockTransferDetail.Count; i++)
				{
					dt.Rows.Add(obj.InvStockTransferDetail[i].FromWareHouseName, obj.InvStockTransferDetail[i].ToWareHouseName, obj.InvStockTransferDetail[i].ItemName, obj.InvStockTransferDetail[i].CropYear, obj.InvStockTransferDetail[i].PackTypeDesc, obj.InvStockTransferDetail[i].JobLotDescription, obj.InvStockTransferDetail[i].JobLotTo, obj.InvStockTransferDetail[i].Qty, obj.InvStockTransferDetail[i].PackUom, obj.InvStockTransferDetail[i].GrossWeight, obj.InvStockTransferDetail[i].EbUnit, obj.InvStockTransferDetail[i].EbTotal, obj.InvStockTransferDetail[i].AdLsWeight, obj.InvStockTransferDetail[i].NetWeight, obj.InvStockTransferDetail[i].ItemRate, obj.InvStockTransferDetail[i].ItemAmount, obj.InvStockTransferDetail[i].ExpenseAmount, obj.InvStockTransferDetail[i].RemarksDetail);
				}
				GridDetailHistory.DataSource = dt;
				GridDetailHistory.RetrieveStructure();
				GridDetailHistory.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
				GridDetailHistory.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
				GridDetailHistory.RootTable.Columns["Qty"].FormatString = "#,##0.##";
				GridDetailHistory.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
				GridDetailHistory.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
				GridDetailHistory.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
				GridDetailHistory.RootTable.Columns["GrossWeight"].FormatString = "#,##0.##";
				GridDetailHistory.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
				GridDetailHistory.RootTable.Columns["EbUnit"].AggregateFunction = (AggregateFunction)2;
				GridDetailHistory.RootTable.Columns["EbUnit"].TextAlignment = (TextAlignment)3;
				GridDetailHistory.RootTable.Columns["EbUnit"].FormatString = "#,##0.##";
				GridDetailHistory.RootTable.Columns["EbUnit"].TotalFormatString = "#,##0.##";
				GridDetailHistory.RootTable.Columns["EbTotal"].AggregateFunction = (AggregateFunction)2;
				GridDetailHistory.RootTable.Columns["EbTotal"].TextAlignment = (TextAlignment)3;
				GridDetailHistory.RootTable.Columns["EbTotal"].FormatString = "#,##0.##";
				GridDetailHistory.RootTable.Columns["EbTotal"].TotalFormatString = "#,##0.##";
				GridDetailHistory.RootTable.Columns["AddLessWt"].AggregateFunction = (AggregateFunction)2;
				GridDetailHistory.RootTable.Columns["AddLessWt"].TextAlignment = (TextAlignment)3;
				GridDetailHistory.RootTable.Columns["AddLessWt"].FormatString = "#,##0.##";
				GridDetailHistory.RootTable.Columns["AddLessWt"].TotalFormatString = "#,##0.##";
				GridDetailHistory.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
				GridDetailHistory.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
				GridDetailHistory.RootTable.Columns["StockWeight"].FormatString = "#,##0.##";
				GridDetailHistory.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.##";
				GridDetailHistory.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
				GridDetailHistory.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
				GridDetailHistory.RootTable.Columns["ExpenseAmount"].AggregateFunction = (AggregateFunction)2;
				GridDetailHistory.RootTable.Columns["ExpenseAmount"].TextAlignment = (TextAlignment)3;
				GridDetailHistory.RootTable.Columns["ExpenseAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
				GridDetailHistory.RootTable.Columns["ExpenseAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				GridDetailHistory.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
				GridDetailHistory.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
				GridDetailHistory.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
				GridDetailHistory.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				CommonServices.GridAutoAdjustmentNew(GridDetailHistory);
				ctrlGrdBar6_Load(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				CommonServices.GetNoofAttachmentsByScreenName(Conversion.ToInt(r.Cells["Id"].Value), base.Name);
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

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdhistory).Name);
			if (fs != null)
			{
				grdhistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
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

	private void ctrlGrdBar6_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GridDetailHistory).Name);
			if (fs != null)
			{
				GridDetailHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemAmountCalculation()
	{
		try
		{
			if (grd.GetRows().Length != 0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow item in rows)
				{
					item.BeginEdit();
					double NetWeight = Conversion.ToDouble(item.Cells["NetWeight"].Value);
					double RateUOM = Conversion.ToDouble(item.Cells["RateUOM"].Value);
					double ItemRate = Conversion.ToDouble(item.Cells["ItemRate"].Value);
					double ItemAmount = NetWeight / RateUOM * ItemRate;
					if (NetWeight > 0.0 && RateUOM > 0.0 && ItemRate > 0.0)
					{
						item.Cells["ItemAmount"].Value = ItemAmount.ToString();
					}
					else
					{
						item.Cells["ItemAmount"].Value = 0;
					}
					item.BeginEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		grd.UpdateData();
	}

	private void WeightCalculation()
	{
		try
		{
			double GrossWeight = 0.0;
			double Qty = 0.0;
			double EbUnit = 0.0;
			double EbTotal = 0.0;
			double addless = 0.0;
			if (txtQty.Text.Trim() != string.Empty && txtQty.Text != "0")
			{
				Qty = Conversion.ToDouble((object)txtQty.Text.Trim());
			}
			if (txtGrossWeight.Text.Trim() != string.Empty && txtGrossWeight.Text != "0")
			{
				GrossWeight = Conversion.ToDouble((object)txtGrossWeight.Text.Trim());
			}
			addless = ((!(txtAdLsWeight.Text.Trim() != string.Empty)) ? 0.0 : Conversion.ToDouble((object)txtAdLsWeight.Text.Trim()));
			if (txtEbUnit.Text.Trim() != string.Empty)
			{
				EbUnit = Conversion.ToDouble((object)txtEbUnit.Text.Trim());
			}
			else
			{
				EbUnit = 0.0;
				txtEbTotal.Text = "0";
			}
			if (EbUnit > 0.0)
			{
				EbTotal = Qty * EbUnit;
				txtEbTotal.Text = Conversion.ToString((object)EbTotal);
			}
			if (txtQty.Text.Trim() != string.Empty && Conversion.ToDouble(CmbPackUom.Value) != 0.0 && txtGrossWeight.Text.Trim() != string.Empty)
			{
				txtDetailNetWeight.Text = (GrossWeight - EbTotal + addless).ToString();
			}
			AvgAmountCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrossWeightCalculation()
	{
		try
		{
			double GrossWeight = 0.0;
			if (Conversion.ToDouble((object)txtQty.Text.Trim()) != 0.0 && Conversion.ToDouble(CmbPackUom.Value) != 0.0)
			{
				double num = Conversion.ToDouble((object)txtQty.Text.Trim());
				double UOM = Conversion.ToDouble((object)((Control)(object)CmbPackUom).Text.Trim());
				txtGrossWeight.Text = (num * UOM).ToString();
			}
			AvgAmountCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQty_TextChanged(object sender, EventArgs e)
	{
		GrossWeightCalculation();
		WeightCalculation();
	}

	private void CmbPackUom_TextChanged(object sender, EventArgs e)
	{
		GrossWeightCalculation();
		WeightCalculation();
	}

	private void txtGrossWeight_TextChanged(object sender, EventArgs e)
	{
		WeightCalculation();
	}

	private void txtAdLsWeight_TextChanged(object sender, EventArgs e)
	{
		WeightCalculation();
	}

	private void txtdocno_Leave(object sender, EventArgs e)
	{
	}

	private void CmbWareHouseFrom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			AvailableStockGetByItem();
			GetAvgRate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AvailableStockGetByItem()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		try
		{
			DataTable dt = CommonServices.GetWeightCurrStockByItem(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				WarehouseId = Conversion.ToInt(CmbWareHouseFrom.Value),
				ItemId = Conversion.ToInt(cmbItemName.Value),
				JobLotId = Conversion.ToInt(cmbJobLot.Value),
				CropYear = Conversion.ToString((object)((Control)(object)cmbCropYear).Text.Trim()),
				ToDate = Conversion.ToDateTime((object)DocDate.Value)
			});
			if (dt.Rows.Count > 0)
			{
				txtAvailableStock.Text = dt.Rows[0]["AvailableStock"].ToString();
			}
			else
			{
				txtAvailableStock.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEbUnit_TextChanged(object sender, EventArgs e)
	{
		WeightCalculation();
	}

	private void CmbTransferType_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)CmbTransferType).Text == "Outward")
			{
				((Control)(object)cmbItemName).Text = string.Empty;
				((UltraGridBase)cmbItemName).DataSource = null;
				if (Conversion.ToInt(cmbGatePass.Value) > 0)
				{
					GetItemIdFromDeliveryOrderForStockTransfer(Conversion.ToInt(cmbGatePass.Value));
				}
			}
			HandleVisibilityOfLoader();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HandleVisibilityOfLoader()
	{
		try
		{
			if (RefDocumentTypeId != 51 && ((Control)(object)cmbTicketNo).Text != string.Empty)
			{
				btnLoadInvoices.Visible = true;
			}
			else
			{
				btnLoadInvoices.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbCropYear_TextChanged(object sender, EventArgs e)
	{
		AvailableStockGetByItem();
		GetAvgRate();
	}

	private void cmbJobLot_TextChanged(object sender, EventArgs e)
	{
		AvailableStockGetByItem();
		GetAvgRate();
	}

	private void cmbItemName_TextChanged(object sender, EventArgs e)
	{
	}

	private void GetAvgRate()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			bool NewRateGet = true;
			if (((Control)(object)CmbTransferType).Text == "Inward" && updateDetailIndex > -1)
			{
				GridEXRow r = grd.CurrentRow;
				if (r != null && (int)r.RowType == 0)
				{
					int num = Conversion.ToInt(r.Cells["TransferDocumentTypeId"].Value);
					int TransferId = Conversion.ToInt(r.Cells["TransferId"].Value);
					int TransferDetailId = Conversion.ToInt(r.Cells["TransferDetailId"].Value);
					if (num > 0 && TransferId > 0 && TransferDetailId > 0)
					{
						NewRateGet = false;
					}
				}
			}
			if (!NewRateGet)
			{
				return;
			}
			if (!FIFOCGSFlag)
			{
				double rate = CommonServices.AvgRateOnlyForCGS(Conversion.ToInt(cmbItemName.Value), Conversion.ToDateTime((object)DocDate.Value), 68, Id, Conversion.ToInt(cmbJobLot.Value), Conversion.ToInt(cmbCropYear.Value), null, Conversion.ToInt(CmbWareHouseFrom.Value)) * 40.0;
				if (rate > 0.0)
				{
					txtAvgRate.Text = Conversion.ToString((object)rate);
					AvgAmountCalculation();
				}
				else
				{
					txtAvgRate.Text = 0.ToString();
					txtAmount.Text = 0.ToString();
				}
			}
			else
			{
				txtAvgRate.Text = 0.ToString();
				txtAmount.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AvgAmountCalculation()
	{
		try
		{
			if (Conversion.ToDouble((object)txtDetailNetWeight.Text) > 0.0 && Conversion.ToDouble((object)txtAvgRate.Text) > 0.0)
			{
				double rate = Conversion.ToDouble((object)txtAvgRate.Text);
				double Amount = Conversion.ToDouble((object)txtDetailNetWeight.Text) / 40.0 * rate;
				txtAmount.Text = Amount.ToString("#,#");
			}
			else
			{
				txtAmount.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnprint_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.StockTransferSlip406(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		try
		{
			AT.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadInvoices_Click(object sender, EventArgs e)
	{
		try
		{
			LoadTransactions = new LoadavailableTransactionsForIssuance(UserAccount);
			LoadTransactions.ShowDialog();
			LoadTransactions.BringToFront();
			LoadDataDetailfromPurchaseInvoivce();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadDataDetailfromPurchaseInvoivce()
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dtpurchaseinvoice = LoadTransactions.dtIssuance;
			if (dtpurchaseinvoice.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtpurchaseinvoice.Rows.Count; i++)
			{
				bool flag = false;
				if (grd.RowCount > 0)
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow item in rows)
					{
						if (item != null && (int)item.RowType == 0)
						{
							if (Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) == 0 || Conversion.ToInt(item.Cells["RefDocNoId"].Value) == 0)
							{
								item.Delete();
							}
							if (grd.GetRows().Length != 0 && Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocumentTypeId"]) == Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) && Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocIdNo"]) == Conversion.ToInt(item.Cells["RefDocNoId"].Value) && Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocSubIdNo"]) == Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value))
							{
								item.BeginEdit();
								double BalQTY = Conversion.ToDouble(item.Cells["QTY"].Value);
								double BalWeight = Conversion.ToDouble(item.Cells["BalWeight"].Value);
								item.Cells["QTY"].Value = Conversion.ToDouble((object)BalQTY) + Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["ItemQty"]);
								item.Cells["BalWeight"].Value = Conversion.ToDouble((object)BalWeight) + Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["Weight"]);
								item.EndEdit();
								flag = true;
							}
						}
					}
				}
				if (!flag && !flag)
				{
					table.Rows.Add(true, 0, dtpurchaseinvoice.Rows[i]["RefDocumentTypeId"], dtpurchaseinvoice.Rows[i]["RefDocIdNo"], dtpurchaseinvoice.Rows[i]["RefDocSubIdNo"], dtpurchaseinvoice.Rows[i]["ItemId"], dtpurchaseinvoice.Rows[i]["ItemName"], dtpurchaseinvoice.Rows[i]["PackingTypeId"], dtpurchaseinvoice.Rows[i]["PackTypeCode"], dtpurchaseinvoice.Rows[i]["CropYear"], dtpurchaseinvoice.Rows[i]["JobLotId"], dtpurchaseinvoice.Rows[i]["JobLotCode"], dtpurchaseinvoice.Rows[i]["JobLotId"], dtpurchaseinvoice.Rows[i]["JobLotCode"], dtpurchaseinvoice.Rows[i]["ItemQty"], dtpurchaseinvoice.Rows[i]["ItemUOMId"], dtpurchaseinvoice.Rows[i]["ItemUOM"], dtpurchaseinvoice.Rows[i]["Weight"], 0, 0, 0, dtpurchaseinvoice.Rows[i]["Weight"], dtpurchaseinvoice.Rows[i]["ItemQty"], dtpurchaseinvoice.Rows[i]["Weight"], dtpurchaseinvoice.Rows[i]["WareHouseId"], dtpurchaseinvoice.Rows[i]["WareHouse"], 0, "", "", dtpurchaseinvoice.Rows[i]["ItemRate"], dtpurchaseinvoice.Rows[i]["RateUOMId"], dtpurchaseinvoice.Rows[i]["Equivalent"], dtpurchaseinvoice.Rows[i]["ItemAmount"]);
				}
			}
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			OtherChargesAmountUpdate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindDetailFromLoader()
	{
		try
		{
			int organizationId = UserAccount.OrganizationId;
			int compId = UserAccount.CompanyId;
			int branchId = UserAccount.BranchesId;
			int gpId = Conversion.ToInt(cmbGatePass.Value);
			DataTable dt = InvStockTransferHeader.GetBranchDataFromStockTransfer(organizationId, compId, branchId, gpId);
			if (dt.Rows.Count > 0)
			{
				table.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					table.Rows.Add(true, 0, 0, 0, 0, dt.Rows[i]["ItemId"], dt.Rows[i]["ItemName"], dt.Rows[i]["InvPackingTypeId"], dt.Rows[i]["PackingType"], dt.Rows[i]["CropYear"], dt.Rows[i]["JobLotId"], dt.Rows[i]["JobLotFrom"], dt.Rows[i]["JobLotIdTo"], dt.Rows[i]["JobLotTo"], dt.Rows[i]["Qty"], dt.Rows[i]["PackUomId"], dt.Rows[i]["PEquivalent"], dt.Rows[i]["NetWeight"], 0, 0, 0, dt.Rows[i]["NetWeight"], dt.Rows[i]["Qty"], dt.Rows[i]["NetWeight"], 0, "", 0, "", "", dt.Rows[i]["ItemRate"], dt.Rows[i]["RateUomId"], dt.Rows[i]["REquivalent"], dt.Rows[i]["ItemAmount"], 0, dt.Rows[i]["DocumentTypeId"], dt.Rows[i]["InvStockTransferHeaderId"], dt.Rows[i]["TransferDetailId"]);
				}
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				return;
			}
			throw new Exception("Detail Record Not Found.Because Stock From Transaction is Incomplete!");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtqty_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtFromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void BtnSameUpdateWarehouse_Click(object sender, EventArgs e)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!chkUpdateWareHouse.Checked && !chkUpdateJobLot.Checked)
			{
				throw new Exception("Please Checked atleast one value (warehouse,JobLot) value to Update");
			}
			if (grd.GetDataRows().Count() > 0)
			{
				GridEXRow rowsToValidate = grd.GetDataRows().FirstOrDefault();
				if (rowsToValidate == null || (int)rowsToValidate.RowType != 0 || rowsToValidate.Cells.Count <= 0)
				{
					return;
				}
				if (Conversion.ToInt(rowsToValidate.Cells["WareHouseToId"].Value) != 0)
				{
					int wareId = 0;
					string wareName = "";
					wareId = Conversion.ToInt(rowsToValidate.Cells["WareHouseToId"].Value);
					wareName = Conversion.ToString(rowsToValidate.Cells["WareHouseTo"].Value);
					int JobId = 0;
					string jobName = "";
					JobId = Conversion.ToInt(rowsToValidate.Cells["JobLotIdTo"].Value);
					jobName = Conversion.ToString(rowsToValidate.Cells["JobLotTo"].Value);
					GridEXRow[] dataRows = grd.GetDataRows();
					foreach (GridEXRow r in dataRows)
					{
						r.BeginEdit();
						if (chkUpdateWareHouse.Checked)
						{
							r.Cells["WareHouseToId"].Value = wareId;
							r.Cells["WareHouseTo"].Value = wareName;
						}
						if (chkUpdateJobLot.Checked)
						{
							r.Cells["JobLotIdTo"].Value = JobId;
							r.Cells["JobLotTo"].Value = jobName;
						}
						r.EndEdit();
					}
					return;
				}
				throw new Exception("WareHouseTo Field is Empty in Grid Row No: 01");
			}
			throw new Exception("Grid Record not Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		try
		{
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvPurchaseInvoice.RemoveByID(new InvPurchaseInvoice
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocumentTypeId = 68,
						Id = Id,
						EntryUser = UserAccount.ID
					});
					MessageBox.Show("Delete Record Successfully");
					Reset();
				}
				return;
			}
			throw new Exception("RecordId Not Found.....");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			AvgRateRecalculateOnDocDateChange();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AvgRateRecalculateOnDocDateChange()
	{
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grd.RowCount <= 0)
			{
				return;
			}
			bool NewRateGet = true;
			GridEXRow[] dataRows = grd.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				r.BeginEdit();
				int RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
				int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				int JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
				int WareHouseId = Conversion.ToInt(r.Cells["WareHouseFromId"].Value);
				string CropYear = Conversion.ToString(r.Cells["CropYear"].Value);
				if (((Control)(object)CmbTransferType).Text == "Inward" && r != null && (int)r.RowType == 0)
				{
					int num = Conversion.ToInt(r.Cells["TransferDocumentTypeId"].Value);
					int TransferId = Conversion.ToInt(r.Cells["TransferId"].Value);
					int TransferDetailId = Conversion.ToInt(r.Cells["TransferDetailId"].Value);
					if (num > 0 && TransferId > 0 && TransferDetailId > 0)
					{
						NewRateGet = false;
					}
				}
				if (!FIFOCGSFlag && RefDocumentTypeId == 0 && NewRateGet)
				{
					double rate = CommonServices.AvgRateOnlyForCGS(ItemId, Conversion.ToDateTime((object)DocDate.Value), 68, Id, JobLotId, 0, CropYear, WareHouseId) * 40.0;
					if (rate > 0.0)
					{
						r.Cells["ItemRate"].Value = rate;
						double Amount = Conversion.ToDouble(r.Cells["NetWeight"].Value) / 40.0 * rate;
						r.Cells["ItemAmount"].Value = Amount;
					}
					else
					{
						r.Cells["ItemRate"].Value = 0;
						r.Cells["ItemAmount"].Value = 0;
					}
				}
				r.EndEdit();
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Expected O, but got Unknown
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Expected O, but got Unknown
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Expected O, but got Unknown
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Expected O, but got Unknown
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Expected O, but got Unknown
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Expected O, but got Unknown
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Expected O, but got Unknown
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Expected O, but got Unknown
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Expected O, but got Unknown
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Expected O, but got Unknown
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Expected O, but got Unknown
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Expected O, but got Unknown
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Expected O, but got Unknown
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected O, but got Unknown
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Expected O, but got Unknown
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Expected O, but got Unknown
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Expected O, but got Unknown
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Expected O, but got Unknown
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Expected O, but got Unknown
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Expected O, but got Unknown
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Expected O, but got Unknown
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Expected O, but got Unknown
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Expected O, but got Unknown
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Expected O, but got Unknown
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Expected O, but got Unknown
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Expected O, but got Unknown
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Expected O, but got Unknown
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Expected O, but got Unknown
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Expected O, but got Unknown
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Expected O, but got Unknown
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Expected O, but got Unknown
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Expected O, but got Unknown
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Expected O, but got Unknown
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Expected O, but got Unknown
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Expected O, but got Unknown
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Expected O, but got Unknown
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Expected O, but got Unknown
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Expected O, but got Unknown
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Expected O, but got Unknown
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Expected O, but got Unknown
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Expected O, but got Unknown
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Expected O, but got Unknown
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Expected O, but got Unknown
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Expected O, but got Unknown
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Expected O, but got Unknown
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Expected O, but got Unknown
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Expected O, but got Unknown
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Expected O, but got Unknown
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Expected O, but got Unknown
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Expected O, but got Unknown
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Expected O, but got Unknown
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Expected O, but got Unknown
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Expected O, but got Unknown
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Expected O, but got Unknown
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Expected O, but got Unknown
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Expected O, but got Unknown
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Expected O, but got Unknown
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Expected O, but got Unknown
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_043b: Expected O, but got Unknown
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Expected O, but got Unknown
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Expected O, but got Unknown
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_057a: Expected O, but got Unknown
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Expected O, but got Unknown
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0614: Expected O, but got Unknown
		//IL_0620: Unknown result type (might be due to invalid IL or missing references)
		//IL_062a: Expected O, but got Unknown
		//IL_062b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Expected O, but got Unknown
		//IL_0636: Unknown result type (might be due to invalid IL or missing references)
		//IL_0640: Expected O, but got Unknown
		//IL_064c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0656: Expected O, but got Unknown
		//IL_066d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0677: Expected O, but got Unknown
		//IL_06fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0706: Expected O, but got Unknown
		//IL_0733: Unknown result type (might be due to invalid IL or missing references)
		//IL_073d: Expected O, but got Unknown
		//IL_073e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0748: Expected O, but got Unknown
		//IL_0867: Unknown result type (might be due to invalid IL or missing references)
		//IL_0871: Expected O, but got Unknown
		//IL_0917: Unknown result type (might be due to invalid IL or missing references)
		//IL_0921: Expected O, but got Unknown
		//IL_0964: Unknown result type (might be due to invalid IL or missing references)
		//IL_096e: Expected O, but got Unknown
		//IL_0ecc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed6: Expected O, but got Unknown
		//IL_1314: Unknown result type (might be due to invalid IL or missing references)
		//IL_131e: Expected O, but got Unknown
		//IL_1a3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a44: Expected O, but got Unknown
		//IL_1a51: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a5b: Expected O, but got Unknown
		//IL_7155: Unknown result type (might be due to invalid IL or missing references)
		//IL_715f: Expected O, but got Unknown
		//IL_7183: Unknown result type (might be due to invalid IL or missing references)
		//IL_718d: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOtherCharges_DesignTimeLayout = new GridEXLayout();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance141 = new Appearance();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance12 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		GridEXLayout GridDetailHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grdDetail = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.label24 = new System.Windows.Forms.Label();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.PanelMessage = new System.Windows.Forms.Panel();
		this.ToolStripMessage = new System.Windows.Forms.ToolStrip();
		this.BtnMessage = new System.Windows.Forms.ToolStripButton();
		this.chkUpdateJobLot = new System.Windows.Forms.CheckBox();
		this.chkUpdateWareHouse = new System.Windows.Forms.CheckBox();
		this.BtnSameUpdateWarehouse = new System.Windows.Forms.Button();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.grdOtherCharges = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.lblAmount = new System.Windows.Forms.Label();
		this.txtAvgRate = new System.Windows.Forms.TextBox();
		this.lblAvgRate = new System.Windows.Forms.Label();
		this.txtEbTotal = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.txtEbUnit = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.cmbItemName = new UltraCombo();
		this.cmbWareHouseTo = new UltraCombo();
		this.label37 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.txtDetailNetWeight = new System.Windows.Forms.TextBox();
		this.label38 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.txtAdLsWeight = new System.Windows.Forms.TextBox();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.btnplus = new System.Windows.Forms.Button();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.cmbCropYear = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.cmbPackingType = new UltraCombo();
		this.CmbJobLotTo = new UltraCombo();
		this.cmbJobLot = new UltraCombo();
		this.label21 = new System.Windows.Forms.Label();
		this.CmbPackUom = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.CmbWareHouseFrom = new UltraCombo();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.txtNoofBranches = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.txtOtherWeight = new System.Windows.Forms.TextBox();
		this.label20 = new System.Windows.Forms.Label();
		this.txtAvailableStock = new System.Windows.Forms.TextBox();
		this.txtWorkingReportNo = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.CmbTransferType = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtHeadNetWeight = new System.Windows.Forms.TextBox();
		this.cmbTicketNo = new UltraCombo();
		this.cmbGatePass = new UltraCombo();
		this.txtRemarksHead = new System.Windows.Forms.TextBox();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label33 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnLoadInvoices = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.ChkBoxPrintPreview = new System.Windows.Forms.CheckBox();
		this.label23 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdhistory = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label13 = new System.Windows.Forms.Label();
		this.panel12 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.label26 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.cmbTransactiontypeHistory = new UltraCombo();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel15 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar6 = new grid.User_Conrtols.CtrlGrdBar();
		this.GridDetailHistory = new GridEX();
		this.label18 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.label25 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel6.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel3.SuspendLayout();
		this.PanelMessage.SuspendLayout();
		this.ToolStripMessage.SuspendLayout();
		this.tabPage4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOtherCharges).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouseTo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLotTo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouseFrom).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTransferType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbTicketNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGatePass).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel7.SuspendLayout();
		this.panel12.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbTransactiontypeHistory).BeginInit();
		this.panel15.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetailHistory).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1064, 724);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1064, 724);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.tabControl2);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(1056, 695);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
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
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(2, 493);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1052, 200);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 2;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetail_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar4);
		this.panel6.Controls.Add(this.label24);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(2, 469);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1052, 24);
		this.panel6.TabIndex = 8;
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(1004, 1);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.grdDetail;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(45, 23);
		this.ctrlGrdBar4.TabIndex = 24;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label24.ForeColor = System.Drawing.Color.White;
		this.label24.Location = new System.Drawing.Point(3, 2);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(268, 18);
		this.label24.TabIndex = 3;
		this.label24.Text = "Pending Entries For Stock Transfer";
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Top;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(2, 249);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1052, 220);
		this.tabControl2.TabIndex = 0;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage3.Controls.Add(this.panel3);
		this.tabPage3.Font = new System.Drawing.Font("Segoe UI", 10.75f);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1044, 191);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Detail";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		this.grd.ColumnAutoResize = true;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 27);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1038, 161);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.PanelMessage);
		this.panel3.Controls.Add(this.chkUpdateJobLot);
		this.panel3.Controls.Add(this.chkUpdateWareHouse);
		this.panel3.Controls.Add(this.BtnSameUpdateWarehouse);
		this.panel3.Controls.Add(this.ctrlGrdBar5);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1038, 24);
		this.panel3.TabIndex = 9;
		this.PanelMessage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.PanelMessage.BackColor = System.Drawing.Color.White;
		this.PanelMessage.Controls.Add(this.ToolStripMessage);
		this.PanelMessage.Location = new System.Drawing.Point(600, 1);
		this.PanelMessage.Name = "PanelMessage";
		this.PanelMessage.Size = new System.Drawing.Size(25, 23);
		this.PanelMessage.TabIndex = 132;
		this.ToolStripMessage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ToolStripMessage.Dock = System.Windows.Forms.DockStyle.None;
		this.ToolStripMessage.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.ToolStripMessage.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.BtnMessage });
		this.ToolStripMessage.Location = new System.Drawing.Point(0, 0);
		this.ToolStripMessage.Name = "ToolStripMessage";
		this.ToolStripMessage.Size = new System.Drawing.Size(26, 25);
		this.ToolStripMessage.TabIndex = 0;
		this.ToolStripMessage.Text = "toolStrip3";
		this.BtnMessage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.BtnMessage.Image = Architecture.WinApp.Properties.Resources.info_i;
		this.BtnMessage.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnMessage.Name = "BtnMessage";
		this.BtnMessage.Size = new System.Drawing.Size(23, 22);
		this.BtnMessage.ToolTipText = "Click to 'Update Same In All Rows' Button To Update Checked Column (Warehouse or JobLot) In All rows";
		this.chkUpdateJobLot.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkUpdateJobLot.AutoSize = true;
		this.chkUpdateJobLot.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkUpdateJobLot.ForeColor = System.Drawing.Color.White;
		this.chkUpdateJobLot.Location = new System.Drawing.Point(727, 3);
		this.chkUpdateJobLot.Name = "chkUpdateJobLot";
		this.chkUpdateJobLot.Size = new System.Drawing.Size(63, 19);
		this.chkUpdateJobLot.TabIndex = 129;
		this.chkUpdateJobLot.TabStop = false;
		this.chkUpdateJobLot.Text = "JobLot";
		this.chkUpdateJobLot.UseVisualStyleBackColor = true;
		this.chkUpdateWareHouse.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkUpdateWareHouse.AutoSize = true;
		this.chkUpdateWareHouse.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkUpdateWareHouse.ForeColor = System.Drawing.Color.White;
		this.chkUpdateWareHouse.Location = new System.Drawing.Point(634, 3);
		this.chkUpdateWareHouse.Name = "chkUpdateWareHouse";
		this.chkUpdateWareHouse.Size = new System.Drawing.Size(91, 19);
		this.chkUpdateWareHouse.TabIndex = 130;
		this.chkUpdateWareHouse.TabStop = false;
		this.chkUpdateWareHouse.Text = "WareHouse";
		this.chkUpdateWareHouse.UseVisualStyleBackColor = true;
		this.BtnSameUpdateWarehouse.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnSameUpdateWarehouse.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnSameUpdateWarehouse.FlatAppearance.BorderSize = 0;
		this.BtnSameUpdateWarehouse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnSameUpdateWarehouse.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnSameUpdateWarehouse.ForeColor = System.Drawing.Color.White;
		this.BtnSameUpdateWarehouse.Location = new System.Drawing.Point(795, 2);
		this.BtnSameUpdateWarehouse.Name = "BtnSameUpdateWarehouse";
		this.BtnSameUpdateWarehouse.Size = new System.Drawing.Size(192, 21);
		this.BtnSameUpdateWarehouse.TabIndex = 37;
		this.BtnSameUpdateWarehouse.Text = "Update Same In All Rows";
		this.BtnSameUpdateWarehouse.UseVisualStyleBackColor = false;
		this.BtnSameUpdateWarehouse.Click += new System.EventHandler(BtnSameUpdateWarehouse_Click);
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(989, 1);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
		this.ctrlGrdBar5.MyGrid = this.grd;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(47, 24);
		this.ctrlGrdBar5.TabIndex = 25;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.grdOtherCharges);
		this.tabPage4.Controls.Add(this.panel10);
		this.tabPage4.Font = new System.Drawing.Font("Segoe UI", 10.75f);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(1044, 191);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Other Charges";
		this.tabPage4.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdOtherCharges).BackColor = System.Drawing.Color.White;
		this.grdOtherCharges.ColumnAutoResize = true;
		grdOtherCharges_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOtherCharges.DesignTimeLayout = grdOtherCharges_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOtherCharges).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdOtherCharges).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOtherCharges.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOtherCharges).Location = new System.Drawing.Point(3, 27);
		((System.Windows.Forms.Control)(object)this.grdOtherCharges).Name = "grdOtherCharges";
		((ScrollableBase)this.grdOtherCharges).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOtherCharges).Size = new System.Drawing.Size(1038, 161);
		((System.Windows.Forms.Control)(object)this.grdOtherCharges).TabIndex = 1;
		this.grdOtherCharges.TotalRow = (InheritableBoolean)1;
		this.grdOtherCharges.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOtherCharges).VisualStyle = (VisualStyle)4;
		this.grdOtherCharges.CellUpdated += new ColumnActionEventHandler(grdOtherCharges_CellUpdated);
		this.grdOtherCharges.ColumnButtonClick += new ColumnActionEventHandler(grdOtherCharges_ColumnButtonClick);
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 3);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1038, 24);
		this.panel10.TabIndex = 9;
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.txtAmount);
		this.panel5.Controls.Add(this.lblAmount);
		this.panel5.Controls.Add(this.txtAvgRate);
		this.panel5.Controls.Add(this.lblAvgRate);
		this.panel5.Controls.Add(this.txtEbTotal);
		this.panel5.Controls.Add(this.label19);
		this.panel5.Controls.Add(this.txtEbUnit);
		this.panel5.Controls.Add(this.label16);
		this.panel5.Controls.Add(this.txtRemarksDetail);
		this.panel5.Controls.Add(this.label12);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemName);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWareHouseTo);
		this.panel5.Controls.Add(this.label37);
		this.panel5.Controls.Add(this.label11);
		this.panel5.Controls.Add(this.label36);
		this.panel5.Controls.Add(this.txtDetailNetWeight);
		this.panel5.Controls.Add(this.label38);
		this.panel5.Controls.Add(this.label10);
		this.panel5.Controls.Add(this.label45);
		this.panel5.Controls.Add(this.txtAdLsWeight);
		this.panel5.Controls.Add(this.txtQty);
		this.panel5.Controls.Add(this.label9);
		this.panel5.Controls.Add(this.btnplus);
		this.panel5.Controls.Add(this.txtGrossWeight);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCropYear);
		this.panel5.Controls.Add(this.label8);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackingType);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLotTo);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbJobLot);
		this.panel5.Controls.Add(this.label21);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.panel5.Controls.Add(this.label7);
		this.panel5.Controls.Add(this.btnUpdateDetail);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom);
		this.panel5.Controls.Add(this.btnCancelUpdateDetial);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(2, 150);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1052, 99);
		this.panel5.TabIndex = 1;
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAmount.Location = new System.Drawing.Point(811, 72);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.ReadOnly = true;
		this.txtAmount.Size = new System.Drawing.Size(93, 23);
		this.txtAmount.TabIndex = 16;
		this.txtAmount.TabStop = false;
		this.lblAmount.AutoSize = true;
		this.lblAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblAmount.Location = new System.Drawing.Point(811, 54);
		this.lblAmount.Name = "lblAmount";
		this.lblAmount.Size = new System.Drawing.Size(51, 15);
		this.lblAmount.TabIndex = 51;
		this.lblAmount.Text = "Amount";
		this.txtAvgRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAvgRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAvgRate.Location = new System.Drawing.Point(740, 72);
		this.txtAvgRate.Name = "txtAvgRate";
		this.txtAvgRate.ReadOnly = true;
		this.txtAvgRate.Size = new System.Drawing.Size(69, 23);
		this.txtAvgRate.TabIndex = 15;
		this.txtAvgRate.TabStop = false;
		this.txtAvgRate.Text = "0";
		this.lblAvgRate.AutoSize = true;
		this.lblAvgRate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblAvgRate.Location = new System.Drawing.Point(739, 54);
		this.lblAvgRate.Name = "lblAvgRate";
		this.lblAvgRate.Size = new System.Drawing.Size(54, 15);
		this.lblAvgRate.TabIndex = 49;
		this.lblAvgRate.Text = "Avg Rate";
		this.txtEbTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbTotal.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEbTotal.Location = new System.Drawing.Point(302, 72);
		this.txtEbTotal.Name = "txtEbTotal";
		this.txtEbTotal.ReadOnly = true;
		this.txtEbTotal.Size = new System.Drawing.Size(81, 23);
		this.txtEbTotal.TabIndex = 11;
		this.txtEbTotal.TabStop = false;
		this.txtEbTotal.Text = "0";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(301, 54);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(46, 15);
		this.label19.TabIndex = 47;
		this.label19.Text = "EbTotal";
		this.txtEbUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbUnit.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEbUnit.Location = new System.Drawing.Point(240, 72);
		this.txtEbUnit.Name = "txtEbUnit";
		this.txtEbUnit.Size = new System.Drawing.Size(60, 23);
		this.txtEbUnit.TabIndex = 10;
		this.txtEbUnit.Text = "0";
		this.txtEbUnit.TextChanged += new System.EventHandler(txtEbUnit_TextChanged);
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(239, 54);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(42, 15);
		this.label16.TabIndex = 45;
		this.label16.Text = "EbUnit";
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRemarksDetail.Location = new System.Drawing.Point(570, 72);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(167, 23);
		this.txtRemarksDetail.TabIndex = 14;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(567, 54);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(52, 15);
		this.label12.TabIndex = 16;
		this.label12.Text = "Remarks";
		this.cmbItemName.AllowNull = (DefaultableBoolean)1;
		this.cmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance55;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance61).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemName).Location = new System.Drawing.Point(353, 24);
		((System.Windows.Forms.Control)(object)this.cmbItemName).Name = "cmbItemName";
		((System.Windows.Forms.Control)(object)this.cmbItemName).Size = new System.Drawing.Size(240, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemName).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbItemName).TextChanged += new System.EventHandler(cmbItemName_TextChanged);
		((System.Windows.Forms.Control)(object)this.cmbItemName).Leave += new System.EventHandler(cmbItemName_Leave);
		this.cmbWareHouseTo.AllowNull = (DefaultableBoolean)1;
		this.cmbWareHouseTo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWareHouseTo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWareHouseTo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance79).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWareHouseTo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbWareHouseTo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWareHouseTo).Location = new System.Drawing.Point(175, 24);
		((System.Windows.Forms.Control)(object)this.cmbWareHouseTo).Name = "cmbWareHouseTo";
		((System.Windows.Forms.Control)(object)this.cmbWareHouseTo).Size = new System.Drawing.Size(176, 26);
		((System.Windows.Forms.Control)(object)this.cmbWareHouseTo).TabIndex = 1;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label37.Location = new System.Drawing.Point(715, 6);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(57, 15);
		this.label37.TabIndex = 9;
		this.label37.Text = "Crop Year";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(175, 6);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(85, 15);
		this.label11.TabIndex = 43;
		this.label11.Text = "WareHouse To";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label36.Location = new System.Drawing.Point(78, 54);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(63, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "Pack UOM";
		this.txtDetailNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDetailNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDetailNetWeight.Location = new System.Drawing.Point(466, 72);
		this.txtDetailNetWeight.Name = "txtDetailNetWeight";
		this.txtDetailNetWeight.ReadOnly = true;
		this.txtDetailNetWeight.Size = new System.Drawing.Size(102, 23);
		this.txtDetailNetWeight.TabIndex = 13;
		this.txtDetailNetWeight.TabStop = false;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(7, 54);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(26, 15);
		this.label38.TabIndex = 13;
		this.label38.Text = "Qty";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(466, 54);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(79, 15);
		this.label10.TabIndex = 41;
		this.label10.Text = "Stock Weight";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label45.Location = new System.Drawing.Point(595, 6);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(77, 15);
		this.label45.TabIndex = 23;
		this.label45.Text = "Packing Type";
		this.txtAdLsWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAdLsWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAdLsWeight.Location = new System.Drawing.Point(385, 72);
		this.txtAdLsWeight.Name = "txtAdLsWeight";
		this.txtAdLsWeight.Size = new System.Drawing.Size(79, 23);
		this.txtAdLsWeight.TabIndex = 12;
		this.txtAdLsWeight.Text = "0";
		this.txtAdLsWeight.TextChanged += new System.EventHandler(txtAdLsWeight_TextChanged);
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtQty.Location = new System.Drawing.Point(7, 72);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(68, 23);
		this.txtQty.TabIndex = 7;
		this.txtQty.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(385, 54);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(80, 15);
		this.label9.TabIndex = 39;
		this.label9.Text = "Ad/Ls Weight";
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(907, 70);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(39, 26);
		this.btnplus.TabIndex = 17;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtGrossWeight.Location = new System.Drawing.Point(153, 72);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(84, 23);
		this.txtGrossWeight.TabIndex = 9;
		this.txtGrossWeight.TextChanged += new System.EventHandler(txtGrossWeight_TextChanged);
		this.cmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.cmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance87;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance1).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance1).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance1).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance105).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Location = new System.Drawing.Point(715, 24);
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Name = "cmbCropYear";
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Size = new System.Drawing.Size(84, 26);
		((System.Windows.Forms.Control)(object)this.cmbCropYear).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Leave += new System.EventHandler(cmbCropYear_TextChanged);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(153, 54);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(78, 15);
		this.label8.TabIndex = 37;
		this.label8.Text = "Gross Weight";
		this.cmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.cmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance42;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance48).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Location = new System.Drawing.Point(595, 24);
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Name = "cmbPackingType";
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Size = new System.Drawing.Size(118, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackingType).TabIndex = 3;
		this.CmbJobLotTo.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLotTo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLotTo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLotTo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLotTo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLotTo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance121).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLotTo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbJobLotTo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLotTo).Location = new System.Drawing.Point(906, 24);
		((System.Windows.Forms.Control)(object)this.CmbJobLotTo).Name = "CmbJobLotTo";
		((System.Windows.Forms.Control)(object)this.CmbJobLotTo).Size = new System.Drawing.Size(143, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLotTo).TabIndex = 6;
		this.cmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.cmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Location = new System.Drawing.Point(802, 24);
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Name = "cmbJobLot";
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Size = new System.Drawing.Size(101, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobLot).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Leave += new System.EventHandler(cmbJobLot_TextChanged);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(906, 6);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(62, 15);
		this.label21.TabIndex = 35;
		this.label21.Text = "Job Lot To";
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance2).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance24;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(78, 70);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(73, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TextChanged += new System.EventHandler(CmbPackUom_TextChanged);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(802, 6);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(77, 15);
		this.label7.TabIndex = 35;
		this.label7.Text = "Job Lot From";
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(907, 70);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(72, 26);
		this.btnUpdateDetail.TabIndex = 18;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.CmbWareHouseFrom.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouseFrom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouseFrom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouseFrom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance135;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance103).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance104).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWareHouseFrom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).Location = new System.Drawing.Point(7, 24);
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).Name = "CmbWareHouseFrom";
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).Size = new System.Drawing.Size(166, 26);
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).Leave += new System.EventHandler(CmbWareHouseFrom_TextChanged);
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(981, 70);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(68, 26);
		this.btnCancelUpdateDetial.TabIndex = 19;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(7, 6);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(100, 15);
		this.label3.TabIndex = 0;
		this.label3.Text = "WareHouse From";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(353, 6);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 15);
		this.label2.TabIndex = 0;
		this.label2.Text = "Item Name";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.txtNoofBranches);
		this.panel4.Controls.Add(this.label22);
		this.panel4.Controls.Add(this.txtOtherWeight);
		this.panel4.Controls.Add(this.label20);
		this.panel4.Controls.Add(this.txtAvailableStock);
		this.panel4.Controls.Add(this.txtWorkingReportNo);
		this.panel4.Controls.Add(this.label15);
		this.panel4.Controls.Add(this.label14);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransferType);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.txtHeadNetWeight);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbTicketNo);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbGatePass);
		this.panel4.Controls.Add(this.txtRemarksHead);
		this.panel4.Controls.Add(this.DocDate);
		this.panel4.Controls.Add(this.label33);
		this.panel4.Controls.Add(this.label17);
		this.panel4.Controls.Add(this.txtdocno);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 57);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1052, 93);
		this.panel4.TabIndex = 0;
		this.txtNoofBranches.BackColor = System.Drawing.Color.White;
		this.txtNoofBranches.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNoofBranches.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNoofBranches.Location = new System.Drawing.Point(1008, 62);
		this.txtNoofBranches.Name = "txtNoofBranches";
		this.txtNoofBranches.Size = new System.Drawing.Size(41, 23);
		this.txtNoofBranches.TabIndex = 22;
		this.txtNoofBranches.Visible = false;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(791, 40);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(79, 15);
		this.label22.TabIndex = 21;
		this.label22.Text = "Other Weight";
		this.txtOtherWeight.BackColor = System.Drawing.Color.White;
		this.txtOtherWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOtherWeight.Location = new System.Drawing.Point(881, 36);
		this.txtOtherWeight.Name = "txtOtherWeight";
		this.txtOtherWeight.ReadOnly = true;
		this.txtOtherWeight.Size = new System.Drawing.Size(121, 23);
		this.txtOtherWeight.TabIndex = 8;
		this.txtOtherWeight.TabStop = false;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(791, 66);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(76, 15);
		this.label20.TabIndex = 19;
		this.label20.Text = "StockWeight";
		this.txtAvailableStock.BackColor = System.Drawing.Color.White;
		this.txtAvailableStock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAvailableStock.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAvailableStock.Location = new System.Drawing.Point(881, 62);
		this.txtAvailableStock.Name = "txtAvailableStock";
		this.txtAvailableStock.ReadOnly = true;
		this.txtAvailableStock.Size = new System.Drawing.Size(121, 23);
		this.txtAvailableStock.TabIndex = 9;
		this.txtAvailableStock.TabStop = false;
		this.txtWorkingReportNo.BackColor = System.Drawing.Color.White;
		this.txtWorkingReportNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWorkingReportNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtWorkingReportNo.Location = new System.Drawing.Point(304, 7);
		this.txtWorkingReportNo.Name = "txtWorkingReportNo";
		this.txtWorkingReportNo.ReadOnly = true;
		this.txtWorkingReportNo.Size = new System.Drawing.Size(235, 23);
		this.txtWorkingReportNo.TabIndex = 3;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(6, 66);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(52, 15);
		this.label15.TabIndex = 17;
		this.label15.Text = "Remarks";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(241, 40);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(63, 15);
		this.label14.TabIndex = 16;
		this.label14.Text = "Trans.Type";
		this.CmbTransferType.AllowNull = (DefaultableBoolean)1;
		this.CmbTransferType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransferType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransferType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransferType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance111;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransferType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance116).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransferType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbTransferType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransferType).Location = new System.Drawing.Point(304, 34);
		((System.Windows.Forms.Control)(object)this.CmbTransferType).Name = "CmbTransferType";
		((System.Windows.Forms.Control)(object)this.CmbTransferType).Size = new System.Drawing.Size(235, 26);
		((System.Windows.Forms.Control)(object)this.CmbTransferType).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbTransferType).TabStop = false;
		this.CmbTransferType.ValueChanged += new System.EventHandler(CmbTransferType_ValueChanged);
		((System.Windows.Forms.Control)(object)this.CmbTransferType).Leave += new System.EventHandler(CmbTransferType_Leave);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(791, 11);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(88, 15);
		this.label6.TabIndex = 14;
		this.label6.Text = "Factory Weight";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(545, 40);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(65, 15);
		this.label4.TabIndex = 13;
		this.label4.Text = "Ticket No#";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(545, 11);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(64, 15);
		this.label1.TabIndex = 12;
		this.label1.Text = "Gate Pass#";
		this.txtHeadNetWeight.BackColor = System.Drawing.Color.White;
		this.txtHeadNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtHeadNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtHeadNetWeight.Location = new System.Drawing.Point(881, 7);
		this.txtHeadNetWeight.Name = "txtHeadNetWeight";
		this.txtHeadNetWeight.ReadOnly = true;
		this.txtHeadNetWeight.Size = new System.Drawing.Size(121, 23);
		this.txtHeadNetWeight.TabIndex = 7;
		this.txtHeadNetWeight.TabStop = false;
		this.cmbTicketNo.AllowNull = (DefaultableBoolean)1;
		this.cmbTicketNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbTicketNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbTicketNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbTicketNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbTicketNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbTicketNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbTicketNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTicketNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance90;
		((SpecialBoxBase)((UltraGridBase)this.cmbTicketNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTicketNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbTicketNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbTicketNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbTicketNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbTicketNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbTicketNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbTicketNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTicketNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance96).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbTicketNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbTicketNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbTicketNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance97).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance97).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance97).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTicketNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance122).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbTicketNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbTicketNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbTicketNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbTicketNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbTicketNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbTicketNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbTicketNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbTicketNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbTicketNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbTicketNo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbTicketNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbTicketNo).Location = new System.Drawing.Point(615, 34);
		((System.Windows.Forms.Control)(object)this.cmbTicketNo).Name = "cmbTicketNo";
		((System.Windows.Forms.Control)(object)this.cmbTicketNo).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.cmbTicketNo).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.cmbTicketNo).Leave += new System.EventHandler(cmbTicketNo_Leave);
		this.cmbGatePass.AllowNull = (DefaultableBoolean)1;
		this.cmbGatePass.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbGatePass.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbGatePass.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Appearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance126).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance126).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbGatePass).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance127;
		((SpecialBoxBase)((UltraGridBase)this.cmbGatePass).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance136).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance137).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance137).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance137).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbGatePass).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbGatePass.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbGatePass).Location = new System.Drawing.Point(615, 5);
		((System.Windows.Forms.Control)(object)this.cmbGatePass).Name = "cmbGatePass";
		((System.Windows.Forms.Control)(object)this.cmbGatePass).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.cmbGatePass).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.cmbGatePass).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbGatePass).Leave += new System.EventHandler(cmbGatePass_Leave);
		this.txtRemarksHead.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksHead.Location = new System.Drawing.Point(63, 62);
		this.txtRemarksHead.Name = "txtRemarksHead";
		this.txtRemarksHead.Size = new System.Drawing.Size(722, 23);
		this.txtRemarksHead.TabIndex = 2;
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(63, 36);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(174, 23);
		this.DocDate.TabIndex = 1;
		this.DocDate.TabStop = false;
		this.DocDate.ValueChanged += new System.EventHandler(DocDate_ValueChanged);
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(241, 11);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(58, 15);
		this.label33.TabIndex = 8;
		this.label33.Text = "ReportNo";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(6, 40);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(57, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Doc Date";
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(63, 7);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(174, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_Leave);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(6, 11);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 15);
		this.label5.TabIndex = 3;
		this.label5.Text = "Doc No";
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Controls.Add(this.ChkBoxPrintPreview);
		this.panel2.Controls.Add(this.label23);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1052, 55);
		this.panel2.TabIndex = 0;
		this.toolStrip1.BackColor = System.Drawing.Color.White;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[15]
		{
			this.toolStripSeparator1, this.btnnew, this.btnRefresh, this.toolStripSeparator2, this.btnupdate, this.btnsave, this.toolStripSeparator3, this.btnprint, this.toolStripSeparator4, this.btnAttachment,
			this.toolStripSeparator5, this.btnLoadInvoices, this.toolStripSeparator6, this.BtnShortCutkeys, this.btnDelete
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1052, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.btnprint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(92, 24);
		this.btnprint.Text = "406-Print";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnLoadInvoices.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoadInvoices.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadInvoices.Name = "btnLoadInvoices";
		this.btnLoadInvoices.Size = new System.Drawing.Size(75, 24);
		this.btnLoadInvoices.Text = "Loader";
		this.btnLoadInvoices.Click += new System.EventHandler(btnLoadInvoices_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 27);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.ChkBoxPrintPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBoxPrintPreview.AutoSize = true;
		this.ChkBoxPrintPreview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBoxPrintPreview.ForeColor = System.Drawing.Color.White;
		this.ChkBoxPrintPreview.Location = new System.Drawing.Point(927, 31);
		this.ChkBoxPrintPreview.Name = "ChkBoxPrintPreview";
		this.ChkBoxPrintPreview.Size = new System.Drawing.Size(122, 20);
		this.ChkBoxPrintPreview.TabIndex = 9;
		this.ChkBoxPrintPreview.Text = "Print Preview";
		this.ChkBoxPrintPreview.UseVisualStyleBackColor = true;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(6, 31);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(128, 19);
		this.label23.TabIndex = 3;
		this.label23.Text = "Stock Transfer";
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel12);
		this.tabPage2.Controls.Add(this.panel15);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetailHistory);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1056, 695);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.grdhistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdhistory).BackColor = System.Drawing.Color.White;
		grdhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistory.DesignTimeLayout = grdhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.DynamicFiltering = true;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(2, 158);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1052, 294);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 45;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.SelectionChanged += new System.EventHandler(grdhistory_SelectionChanged);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick_1);
		((System.Windows.Forms.Control)(object)this.grdhistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdhistory_KeyDown);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar3);
		this.panel7.Controls.Add(this.label13);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 134);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1052, 24);
		this.panel7.TabIndex = 47;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1007, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdhistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(43, 21);
		this.ctrlGrdBar3.TabIndex = 23;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(3, 4);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(132, 18);
		this.label13.TabIndex = 0;
		this.label13.Text = "Filtered Records";
		this.panel12.BackColor = System.Drawing.Color.White;
		this.panel12.Controls.Add(this.groupBox2);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(2, 59);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1052, 75);
		this.panel12.TabIndex = 0;
		this.groupBox2.Controls.Add(this.rdapproveddate);
		this.groupBox2.Controls.Add(this.rdmodifydate);
		this.groupBox2.Controls.Add(this.drdocdate);
		this.groupBox2.Controls.Add(this.rdentrydate);
		this.groupBox2.Controls.Add(this.btnshow);
		this.groupBox2.Controls.Add(this.label26);
		this.groupBox2.Controls.Add(this.label27);
		this.groupBox2.Controls.Add(this.label43);
		this.groupBox2.Controls.Add(this.label62);
		this.groupBox2.Controls.Add(this.label63);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbTransactiontypeHistory);
		this.groupBox2.Controls.Add(this.txtToDocNoHistory);
		this.groupBox2.Controls.Add(this.txtFromDocNoHistory);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(2, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(987, 69);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(880, 43);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(880, 23);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(799, 23);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(799, 43);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(722, 36);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(72, 26);
		this.btnshow.TabIndex = 5;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(479, 19);
		this.label26.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(77, 15);
		this.label26.TabIndex = 142;
		this.label26.Text = "Transfer Type";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label27.Location = new System.Drawing.Point(378, 19);
		this.label27.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(64, 15);
		this.label27.TabIndex = 141;
		this.label27.Text = "To Doc No";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label43.Location = new System.Drawing.Point(276, 19);
		this.label43.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(79, 15);
		this.label43.TabIndex = 140;
		this.label43.Text = "From Doc No";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label62.Location = new System.Drawing.Point(143, 19);
		this.label62.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(48, 15);
		this.label62.TabIndex = 139;
		this.label62.Text = "To Date";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label63.Location = new System.Drawing.Point(5, 20);
		this.label63.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(63, 15);
		this.label63.TabIndex = 13;
		this.label63.Text = "From Date";
		this.cmbTransactiontypeHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbTransactiontypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbTransactiontypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbTransactiontypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbTransactiontypeHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbTransactiontypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbTransactiontypeHistory).Location = new System.Drawing.Point(479, 36);
		((System.Windows.Forms.Control)(object)this.cmbTransactiontypeHistory).Name = "cmbTransactiontypeHistory";
		((System.Windows.Forms.Control)(object)this.cmbTransactiontypeHistory).Size = new System.Drawing.Size(240, 26);
		((System.Windows.Forms.Control)(object)this.cmbTransactiontypeHistory).TabIndex = 4;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(378, 38);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(100, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(276, 38);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(100, 23);
		this.txtFromDocNoHistory.TabIndex = 2;
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(143, 38);
		this.ToDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(131, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(5, 38);
		this.FromDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(137, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.ctrlGrdBar6);
		this.panel15.Controls.Add(this.label18);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel15.Location = new System.Drawing.Point(2, 452);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1052, 24);
		this.panel15.TabIndex = 1;
		this.ctrlGrdBar6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar6.FormName = this;
		this.ctrlGrdBar6.Location = new System.Drawing.Point(1007, 1);
		this.ctrlGrdBar6.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar6.MyGrid = this.GridDetailHistory;
		this.ctrlGrdBar6.Name = "ctrlGrdBar6";
		this.ctrlGrdBar6.Size = new System.Drawing.Size(42, 21);
		this.ctrlGrdBar6.TabIndex = 24;
		this.ctrlGrdBar6.Load += new System.EventHandler(ctrlGrdBar6_Load);
		this.GridDetailHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GridDetailHistory).BackColor = System.Drawing.Color.White;
		GridDetailHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridDetailHistory.DesignTimeLayout = GridDetailHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridDetailHistory).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.GridDetailHistory.DynamicFiltering = true;
		this.GridDetailHistory.FilterMode = (FilterMode)2;
		this.GridDetailHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridDetailHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridDetailHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GridDetailHistory).Location = new System.Drawing.Point(2, 476);
		((System.Windows.Forms.Control)(object)this.GridDetailHistory).Name = "GridDetailHistory";
		((ScrollableBase)this.GridDetailHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridDetailHistory).Size = new System.Drawing.Size(1052, 217);
		((System.Windows.Forms.Control)(object)this.GridDetailHistory).TabIndex = 46;
		this.GridDetailHistory.TotalRow = (InheritableBoolean)1;
		this.GridDetailHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridDetailHistory).VisualStyle = (VisualStyle)4;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(7, 4);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(301, 17);
		this.label18.TabIndex = 0;
		this.label18.Text = "Detail Information of Above Selected Row ";
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Controls.Add(this.label25);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(2, 2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1052, 57);
		this.panel8.TabIndex = 46;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1052, 27);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(68, 24);
		this.btnNewHistory.Text = "&Reset";
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "&Refresh";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.ForeColor = System.Drawing.Color.White;
		this.label25.Location = new System.Drawing.Point(5, 34);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(196, 19);
		this.label25.TabIndex = 0;
		this.label25.Text = "Stock Transfer  History";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1064, 724);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "frmStockTransfer";
		this.Text = "Stock Transfer";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmStockTransfer_FormClosing);
		base.Load += new System.EventHandler(frmStockTransfer_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PurchsaeOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.PanelMessage.ResumeLayout(false);
		this.PanelMessage.PerformLayout();
		this.ToolStripMessage.ResumeLayout(false);
		this.ToolStripMessage.PerformLayout();
		this.tabPage4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOtherCharges).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouseTo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLotTo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouseFrom).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTransferType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbTicketNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGatePass).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel12.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbTransactiontypeHistory).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetailHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
