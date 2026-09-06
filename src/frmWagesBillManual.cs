using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.ContractorWages;
using Architecture.BLL.Inventory;
using Architecture.BLL.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.ContractorWages;
using Architecture.Model.Inventory;
using Architecture.WinApp.Common;
using Architecture.WinApp.Lookups;
using Architecture.WinApp.Properties;
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
namespace Architecture.WinApp.Contractor_Wages
{

public class frmWagesBillManual : Form
{
	private UserAccount UserAccount = new UserAccount();

	private Rightsobjects formright = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtdetail = new DataTable();

	private DataTable dtcus = new DataTable();

	private DataTable dtwagesacc = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable dtWare = new DataTable();

	private DataTable dtPackType = new DataTable();

	private DataTable dtCrop = new DataTable();

	private DataTable dtJob = new DataTable();

	private int updateDetailIndex = -1;

	private int WagesScheduleId;

	private bool IsRegularOrFreeOfCost;

	private int RECID;

	public bool UpdateMode;

	private bool WagesAmountCalculateOnQty;

	private bool Party_ProcessingFeature;

	private IContainer components;

	private Panel panel1;

	private TabControl tabIncomingLabourwages;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel4;

	private Panel panel3;

	private Label label15;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton Print;

	private CheckBox chkisapprove;

	private TextBox txtdocnumber;

	private DateTimePicker txtDocdate;

	private Label label6;

	private Label label1;

	internal GridEX grdwagesDetail;

	private Panel panel7;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel9;

	private ToolStrip toolStrip2;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnRefresh;

	private BackgroundWorker backgroundWorker1;

	private Label label11;

	internal UltraCombo cmbReferenceDocType;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStripButton btnWagesSchedule;

	private ToolStripButton btnWagesExempt;

	private Panel panel6;

	private TabControl tabControl1;

	private TabPage tabPage3;

	private Panel panel13;

	private Panel panel15;

	private Label label2;

	private GroupBox groupBox1;

	internal GridEX GrdHistoryDetail;

	private Panel panel12;

	private Label label10;

	private TextBox txtRemarks;

	private Label label7;

	internal UltraCombo CmbJobOrder;

	private Label labelJobOrder;

	private GroupBox groupBox2;

	private RadioButton rdbtnItemCode;

	private RadioButton rdbtnItemName;

	internal UltraCombo CmbWagesAccount;

	internal UltraCombo CmbContractor;

	internal UltraCombo CmbWarehouseFrom;

	internal UltraCombo CmbJobLot;

	internal UltraCombo CmbCropYear;

	internal UltraCombo CmbPackingType;

	internal UltraCombo comItem;

	private Label label41;

	private Label label13;

	private Label label32;

	private Label label8;

	private Label label33;

	private Label label31;

	private Label label9;

	private Label label18;

	private Label label17;

	private Label label16;

	private Label label14;

	private TextBox txtAmount;

	private TextBox txtWagesRate;

	private TextBox txtWeight;

	private TextBox txtQty;

	private Label label42;

	private TextBox txtPackSize;

	internal UltraCombo CmbWareHouseTo;

	private Label label19;

	private Button btnAdd;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	internal GridEX DataGridHistory;

	private Panel panel10;

	private Label label26;

	private GroupBox groupBox4;

	private Button btnShowHistory;

	private Label label51;

	private Label label57;

	private Label label58;

	private Label label64;

	private Label label65;

	internal UltraCombo CmbDocumentTypeHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private Label label3;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripSeparator toolStripSeparator3;

	private CheckBox ChkPreview;

	private TextBox txtwagesType;

	private Label label4;

	private ToolStripButton btnshortcutkeys;

	private CheckBox chkIsCompany;

	internal UltraCombo cmbStockParty;

	private Label labelStockParty;

	public frmWagesBillManual(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmWagesBillManualLoad(object sender, EventArgs e)
	{
		try
		{
			tabPage1.AutoScroll = true;
			tabPage1.AutoScrollMargin = new Size(20, 20);
			tabPage1.AutoScrollMinSize = new Size(1100, 700);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			Print.Enabled = formright.DoHavePrintRights;
			ChkPreview.Checked = formright.DoHavePrintRights;
			ChkPreview.Enabled = formright.DoHavePrintRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar2.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			Party_ProcessingFeature = CommonServices.GetERPFeatureById(8);
			if (Party_ProcessingFeature)
			{
				chkIsCompany.Visible = Party_ProcessingFeature;
				((Control)(object)cmbStockParty).Visible = Party_ProcessingFeature;
				labelStockParty.Visible = Party_ProcessingFeature;
				txtRemarks.Size = new Size(378, 23);
			}
			else
			{
				chkIsCompany.Visible = Party_ProcessingFeature;
				((Control)(object)cmbStockParty).Visible = Party_ProcessingFeature;
				labelStockParty.Visible = Party_ProcessingFeature;
				txtRemarks.Size = new Size(378, 54);
			}
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName", typeof(string));
			dtitem.Columns.Add("ItemCode", typeof(string));
			dtdetail.Columns.Add("Id", typeof(int));
			dtdetail.Columns.Add("Date", typeof(DateTime));
			dtdetail.Columns.Add("ContractorId", typeof(int));
			dtdetail.Columns.Add("WagesAccountId", typeof(int));
			dtdetail.Columns.Add("PackSize", typeof(double));
			dtdetail.Columns.Add("Qty", typeof(double));
			dtdetail.Columns.Add("BillWeight", typeof(double));
			dtdetail.Columns.Add("Rate", typeof(double));
			dtdetail.Columns.Add("Amount", typeof(double));
			dtdetail.Columns.Add("ItemId", typeof(int));
			dtdetail.Columns.Add("ItemCode", typeof(string));
			dtdetail.Columns.Add("CropYearId", typeof(string));
			dtdetail.Columns.Add("PackingTypeId", typeof(int));
			dtdetail.Columns.Add("JobLotId", typeof(int));
			dtdetail.Columns.Add("WareHouseFromId", typeof(int));
			dtdetail.Columns.Add("WareHouseToId", typeof(int));
			dtdetail.Columns.Add("WagesType", typeof(string));
			dtdetail.Columns.Add("IsCompany", typeof(bool));
			dtdetail.Columns.Add("WagesScheduleId", typeof(int));
			grdwagesDetail.DataSource = dtdetail;
			grdwagesDetail.RetrieveStructure();
			DetailGridSettings();
			WagesAmountCalculationsConfigurations();
			SupplierCustomerFill();
			WagesAccountFill();
			GenerateDocNo();
			ItemFill();
			PackingTypeFill();
			WareHouseFill();
			CropYearFill();
			JobLotFill();
			DocumentTypeFill();
			StockPartyFill();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			chkisapprove.Visible = true;
			ChkPreview.Visible = true;
			txtDocdate.Enabled = true;
			txtDocdate.Focus();
			((Control)(object)CmbJobOrder).Visible = false;
			((Control)(object)CmbJobOrder).Enabled = false;
			labelJobOrder.Visible = false;
			((Control)(object)CmbWarehouseFrom).Size = new Size(344, 26);
			((Control)(object)CmbWareHouseTo).Visible = false;
			((Control)(object)CmbWareHouseTo).Enabled = false;
			label19.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StockPartyFill()
	{
		try
		{
			int id = Conversion.ToInt(cmbStockParty.Value);
			DataTable dt = CommonServices.GetSupplierustomerForPartyProcessing();
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, cmbStockParty, "Id", "CompanyName", "Stock Party", true);
			((UltraGridBase)cmbStockParty).DisplayLayout.Bands[0].Columns[1].Width = 350;
			if (id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					cmbStockParty.Value = id;
				}
				else
				{
					cmbStockParty.Value = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateDocNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		try
		{
			int code = InvContractorWagesBillHeader.GenerateCode(new InvContractorWagesBillHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				RefDocumentTypeId = 810
			});
			if (code > 0)
			{
				txtdocnumber.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierCustomerFill()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(CmbContractor.Value);
			dtcus.Clear();
			SupplierCustomer supplier = new SupplierCustomer();
			supplier.OrganizationId = UserAccount.OrganizationId;
			supplier.CompanyId = UserAccount.CompanyId;
			dtcus = SupplierCustomer.ReadByOrganizationCompanyIdForContractorWages(supplier);
			if (dtcus.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtcus, CmbContractor, "Id", "CompanyName", "Contractor Name ", true);
				if (Id > 0)
				{
					if (dtcus.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbContractor.Value = Id;
					}
					else
					{
						((Control)(object)CmbContractor).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbContractor).DataSource = null;
				((Control)(object)CmbContractor).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemFill()
	{
		try
		{
			int Id = Conversion.ToInt(comItem.Value);
			dtitem.Clear();
			((Control)(object)comItem).Text = string.Empty;
			DataTable dt = CommonServices.ReadAllItems();
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtitem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCodeNew"]);
				}
			}
			if (dtitem.Rows.Count > 0)
			{
				if (rdbtnItemName.Checked)
				{
					DDL.BindDDL(dtitem, comItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, comItem, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				((UltraGridBase)comItem).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
			else
			{
				((Control)(object)comItem).Text = string.Empty;
				((UltraGridBase)comItem).DataSource = null;
			}
			if (Id > 0)
			{
				if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					comItem.Value = Id;
				}
				else
				{
					((Control)(object)comItem).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingTypeFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbPackingType.Value);
			dtPackType.Clear();
			dtPackType = InvPackingType.Getall();
			if (dtPackType.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtPackType, CmbPackingType, "Id", "PackTypeDesc", "Packing Type ", true);
				if (Id > 0)
				{
					if (dtPackType.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbPackingType.Value = Id;
					}
					else
					{
						((Control)(object)CmbPackingType).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbPackingType).DataSource = null;
				((Control)(object)CmbPackingType).Text = string.Empty;
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
			int Id = Conversion.ToInt(CmbWarehouseFrom.Value);
			int Idr = Conversion.ToInt(CmbWareHouseTo.Value);
			dtWare.Clear();
			dtWare = CommonServices.WareHouseGetAllService();
			if (dtWare.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtWare, CmbWarehouseFrom, "Id", "WareHouseName", "Ware House From", true);
				DDL.BindDDLNew(dtWare, CmbWareHouseTo, "Id", "WareHouseName", "Ware House To", true);
				if (Id > 0)
				{
					if (dtWare.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbWarehouseFrom.Value = Id;
					}
					else
					{
						((Control)(object)CmbWarehouseFrom).Text = string.Empty;
					}
				}
				if (Idr > 0)
				{
					if (dtWare.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Idr))
					{
						CmbWareHouseTo.Value = Idr;
					}
					else
					{
						((Control)(object)CmbWareHouseTo).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbWareHouseTo).Text = string.Empty;
				((UltraGridBase)CmbWareHouseTo).DataSource = null;
				((Control)(object)CmbWarehouseFrom).Text = string.Empty;
				((UltraGridBase)CmbWarehouseFrom).DataSource = null;
			}
		}
		catch (Exception)
		{
			MessageBox.Show("WareHouse Not Found");
		}
	}

	private void CropYearFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbCropYear.Value);
			dtCrop.Clear();
			dtCrop = CommonServices.CropYearGetAllService();
			if (dtCrop.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCrop, CmbCropYear, "Id", "CropYear", "Crop Year", true);
				if (Id > 0)
				{
					if (dtCrop.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbCropYear.Value = Id;
					}
					else
					{
						((Control)(object)CmbCropYear).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbCropYear).DataSource = null;
				((Control)(object)CmbCropYear).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumentTypeFill()
	{
		try
		{
			int Id = Conversion.ToInt(cmbReferenceDocType.Value);
			int Id2 = Conversion.ToInt(CmbDocumentTypeHistory.Value);
			DataTable dt = InvContractorWagesBillHeader.DocumentTypeForManualWages(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.BranchesId);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbReferenceDocType, "Id", "DocumentType", "DocumentType ", true);
				DDL.BindDDLNew(dt, CmbDocumentTypeHistory, "Id", "DocumentType", "DocumentType ", true);
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						cmbReferenceDocType.Value = Id;
					}
					else
					{
						((Control)(object)cmbReferenceDocType).Text = string.Empty;
					}
				}
				if (Id2 > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id2))
					{
						CmbDocumentTypeHistory.Value = Id2;
					}
					else
					{
						((Control)(object)CmbDocumentTypeHistory).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)cmbReferenceDocType).DataSource = null;
				((Control)(object)cmbReferenceDocType).Text = string.Empty;
				((UltraGridBase)CmbDocumentTypeHistory).DataSource = null;
				((Control)(object)CmbDocumentTypeHistory).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobLotFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbJobLot.Value);
			dtJob.Clear();
			dtJob = CommonServices.JobLotGetAllService();
			if (dtJob.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtJob, CmbJobLot, "Id", "JobLotDescription", "Job Lot ", true);
				if (Id > 0)
				{
					if (dtJob.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbJobLot.Value = Id;
					}
					else
					{
						((Control)(object)CmbJobLot).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbJobLot).DataSource = null;
				((Control)(object)CmbJobLot).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobOrderNoFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		try
		{
			int JobId = Conversion.ToInt(CmbJobOrder.Value);
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			};
			DataTable dtPJO = new DataTable();
			dtPJO = InvProductionJobOrder.GetJobOrderNoForInvFoodProduction(val);
			if (dtPJO.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("PlanCode");
				for (int i = 0; i < dtPJO.Rows.Count; i++)
				{
					dt.Rows.Add(dtPJO.Rows[i]["Id"], dtPJO.Rows[i]["PlanCode"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbJobOrder, "Id", "PlanCode", "JobOrderNo", ZeroIndex: true);
					if (JobId > 0)
					{
						if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == JobId))
						{
							CmbJobOrder.Value = JobId;
						}
						else
						{
							((Control)(object)CmbJobOrder).Text = string.Empty;
						}
					}
				}
				else
				{
					((UltraGridBase)CmbJobOrder).DataSource = null;
					((Control)(object)CmbJobOrder).Text = string.Empty;
				}
			}
			else
			{
				((UltraGridBase)CmbJobOrder).DataSource = null;
				((Control)(object)CmbJobOrder).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WagesAccountFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbWagesAccount.Value);
			dtwagesacc.Clear();
			dtwagesacc = CommonServices.GetWagesAccount("50", 0, 1);
			if (dtwagesacc.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtwagesacc, CmbWagesAccount, "Id", "WagesAccountName", "Wages Account", true);
				if (Id > 0)
				{
					if (dtwagesacc.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbWagesAccount.Value = Id;
					}
					else
					{
						((Control)(object)CmbWagesAccount).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbWagesAccount).DataSource = null;
				((Control)(object)CmbWagesAccount).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WagesAmountCalculationsConfigurations()
	{
		try
		{
			string Qty = CommonServices.GetConfigurationByOrgCompandConfigDescription("WagesAmountCalculateOnQty");
			if (Qty != string.Empty)
			{
				WagesAmountCalculateOnQty = Conversion.ToBool((object)Qty);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbReferenceDocType_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(cmbReferenceDocType.Value) == 112 || Conversion.ToInt(cmbReferenceDocType.Value) == 80)
			{
				((Control)(object)CmbJobOrder).Visible = true;
				((Control)(object)CmbJobOrder).Enabled = true;
				labelJobOrder.Visible = true;
				JobOrderNoFill();
				CmbJobOrder.Focus();
			}
			else
			{
				((Control)(object)CmbJobOrder).Visible = false;
				((Control)(object)CmbJobOrder).Text = string.Empty;
				((Control)(object)CmbJobOrder).Enabled = false;
				labelJobOrder.Visible = false;
				chkisapprove.Focus();
			}
			if (Conversion.ToInt(cmbReferenceDocType.Value) != 68 || Conversion.ToInt(cmbReferenceDocType.Value) != 806)
			{
				((Control)(object)CmbWarehouseFrom).Size = new Size(344, 26);
				((Control)(object)CmbWareHouseTo).Visible = false;
				((Control)(object)CmbWareHouseTo).Enabled = false;
				label19.Visible = false;
				if (grdwagesDetail.RowCount > 0)
				{
					grdwagesDetail.RootTable.Columns["WareHouseToId"].Visible = false;
				}
			}
			else
			{
				((Control)(object)CmbWarehouseFrom).Size = new Size(171, 26);
				((Control)(object)CmbWareHouseTo).Visible = true;
				((Control)(object)CmbWareHouseTo).Enabled = true;
				label19.Visible = true;
				if (grdwagesDetail.RowCount > 0)
				{
					grdwagesDetail.RootTable.Columns["WareHouseToId"].Visible = true;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				string WagesType = "";
				string ItemCode = "";
				IsRegularOrFreeOfCost = CommonServices.CheckItemsFreeofcostforWages(Conversion.ToDateTime((object)txtDocdate.Value), Conversion.ToInt(cmbReferenceDocType.Value), Conversion.ToInt(comItem.Value), Conversion.ToInt(CmbWagesAccount.Value));
				WagesType = ((!IsRegularOrFreeOfCost) ? "Regular" : "Free Of Cost");
				if (((UltraGridBase)comItem).ActiveRow != null && Conversion.ToInt(comItem.Value) > 0)
				{
					ItemCode = Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ItemCode"].Value);
				}
				dtdetail.Rows.Add(0, Conversion.ToDateTime((object)txtDocdate.Value), Conversion.ToInt(CmbContractor.Value), Conversion.ToInt(CmbWagesAccount.Value), Conversion.ToDouble((object)txtPackSize.Text), Conversion.ToDouble((object)txtQty.Text), Conversion.ToDouble((object)txtWeight.Text), Conversion.ToDouble((object)txtWagesRate.Text), Conversion.ToDouble((object)txtAmount.Text), Conversion.ToInt(comItem.Value), ItemCode, Conversion.ToInt(CmbCropYear.Value), Conversion.ToInt(CmbPackingType.Value), Conversion.ToInt(CmbJobLot.Value), Conversion.ToInt(CmbWarehouseFrom.Value), Conversion.ToInt(CmbWareHouseTo.Value), WagesType, Conversion.ToBool((object)chkIsCompany.Checked), WagesScheduleId);
				grdwagesDetail.DataSource = dtdetail;
				grdwagesDetail.RetrieveStructure();
				DetailGridSettings();
				FormRestDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdwagesDetail_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdwagesDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				updateDetailIndex = r.RowIndex;
				CmbContractor.Value = r.Cells["ContractorId"].Value.ToString();
				CmbWagesAccount.Value = r.Cells["WagesAccountId"].Value.ToString();
				txtPackSize.Text = Conversion.ToDecimal(r.Cells["PackSize"].Value).ToString("#,##0.##");
				txtQty.Text = Conversion.ToDecimal(r.Cells["Qty"].Value).ToString("#,##0.##");
				txtWeight.Text = Conversion.ToDecimal(r.Cells["BillWeight"].Value).ToString("#,##0.##");
				txtWagesRate.Text = Conversion.ToDecimal(r.Cells["Rate"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
				txtAmount.Text = Conversion.ToDecimal(r.Cells["Amount"].Value).ToString(clsGlobalVariables.stringFormatsingle);
				comItem.Value = r.Cells["ItemId"].Value.ToString();
				CmbCropYear.Value = r.Cells["CropYearId"].Value.ToString();
				CmbPackingType.Value = r.Cells["PackingTypeId"].Value.ToString();
				CmbJobLot.Value = r.Cells["JobLotId"].Value.ToString();
				CmbWarehouseFrom.Value = r.Cells["WareHouseFromId"].Value.ToString();
				CmbWareHouseTo.Value = r.Cells["WareHouseToId"].Value.ToString();
				chkIsCompany.Checked = Conversion.ToBool(r.Cells["IsCompany"].Value);
				WagesScheduleId = Conversion.ToInt(r.Cells["WagesScheduleId"].Value);
				btnAdd.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				CmbContractor.Focus();
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
				string WagesType = "";
				string ItemCode = "";
				IsRegularOrFreeOfCost = CommonServices.CheckItemsFreeofcostforWages(Conversion.ToDateTime((object)txtDocdate.Value), Conversion.ToInt(cmbReferenceDocType.Value), Conversion.ToInt(comItem.Value), Conversion.ToInt(CmbWagesAccount.Value));
				WagesType = ((!IsRegularOrFreeOfCost) ? "Regular" : "Free Of Cost");
				if (((UltraGridBase)comItem).ActiveRow != null && Conversion.ToInt(comItem.Value) > 0)
				{
					ItemCode = Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ItemCode"].Value);
				}
				dtdetail.Rows[updateDetailIndex]["Date"] = Conversion.ToDateTime((object)txtDocdate.Value);
				dtdetail.Rows[updateDetailIndex]["ContractorId"] = Conversion.ToInt(CmbContractor.Value);
				dtdetail.Rows[updateDetailIndex]["WagesAccountId"] = Conversion.ToInt(CmbWagesAccount.Value);
				dtdetail.Rows[updateDetailIndex]["PackSize"] = Conversion.ToDecimal((object)txtPackSize.Text);
				dtdetail.Rows[updateDetailIndex]["Qty"] = Conversion.ToDecimal((object)txtQty.Text);
				dtdetail.Rows[updateDetailIndex]["BillWeight"] = Conversion.ToDecimal((object)txtWeight.Text);
				dtdetail.Rows[updateDetailIndex]["Rate"] = Conversion.ToDecimal((object)txtWagesRate.Text);
				dtdetail.Rows[updateDetailIndex]["Amount"] = Conversion.ToDecimal((object)txtAmount.Text);
				dtdetail.Rows[updateDetailIndex]["ItemId"] = comItem.Value.ToString();
				dtdetail.Rows[updateDetailIndex]["ItemCode"] = ItemCode;
				dtdetail.Rows[updateDetailIndex]["CropYearId"] = CmbCropYear.Value.ToString();
				dtdetail.Rows[updateDetailIndex]["PackingTypeId"] = CmbPackingType.Value.ToString();
				dtdetail.Rows[updateDetailIndex]["JobLotId"] = CmbJobLot.Value.ToString();
				dtdetail.Rows[updateDetailIndex]["WareHouseFromId"] = CmbWarehouseFrom.Value.ToString();
				dtdetail.Rows[updateDetailIndex]["WareHouseToId"] = CmbWareHouseTo.Value.ToString();
				dtdetail.Rows[updateDetailIndex]["WagesType"] = WagesType;
				dtdetail.Rows[updateDetailIndex]["IsCompany"] = Conversion.ToBool((object)chkIsCompany.Checked);
				dtdetail.Rows[updateDetailIndex]["WagesScheduleId"] = WagesScheduleId;
				btnAdd.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				FormRestDetail();
				CmbContractor.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		try
		{
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			updateDetailIndex = -1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WagesItemAndContractorRefresh()
	{
		try
		{
			grdwagesDetail.RootTable.Columns["ContractorId"].ColumnType = (ColumnType)0;
			grdwagesDetail.RootTable.Columns["ContractorId"].HasValueList = true;
			grdwagesDetail.RootTable.Columns["ContractorId"].LimitToList = true;
			grdwagesDetail.RootTable.Columns["ContractorId"].EditType = (EditType)4;
			grdwagesDetail.RootTable.Columns["ContractorId"].Caption = "Contractor Name";
			grdwagesDetail.RootTable.Columns["ContractorId"].ValueList.PopulateValueList((IEnumerable)dtcus.DefaultView, "Id", "CompanyName");
			grdwagesDetail.RootTable.Columns["WagesAccountId"].ColumnType = (ColumnType)0;
			grdwagesDetail.RootTable.Columns["WagesAccountId"].HasValueList = true;
			grdwagesDetail.RootTable.Columns["WagesAccountId"].LimitToList = true;
			grdwagesDetail.RootTable.Columns["WagesAccountId"].EditType = (EditType)4;
			grdwagesDetail.RootTable.Columns["WagesAccountId"].Caption = "Labour / Wages Activity";
			grdwagesDetail.RootTable.Columns["WagesAccountId"].ValueList.PopulateValueList((IEnumerable)dtwagesacc.DefaultView, "Id", "WagesAccountName");
			grdwagesDetail.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdwagesDetail.RootTable.Columns["ItemId"].HasValueList = true;
			grdwagesDetail.RootTable.Columns["ItemId"].LimitToList = true;
			grdwagesDetail.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdwagesDetail.RootTable.Columns["ItemId"].Caption = "Item Name";
			grdwagesDetail.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtitem.DefaultView, "Id", "ItemName");
			grdwagesDetail.RootTable.Columns["CropYearId"].ColumnType = (ColumnType)0;
			grdwagesDetail.RootTable.Columns["CropYearId"].HasValueList = true;
			grdwagesDetail.RootTable.Columns["CropYearId"].LimitToList = true;
			grdwagesDetail.RootTable.Columns["CropYearId"].EditType = (EditType)4;
			grdwagesDetail.RootTable.Columns["CropYearId"].Caption = "Crop Year";
			grdwagesDetail.RootTable.Columns["CropYearId"].ValueList.PopulateValueList((IEnumerable)dtCrop.DefaultView, "Id", "CropYear");
			grdwagesDetail.RootTable.Columns["PackingTypeId"].ColumnType = (ColumnType)0;
			grdwagesDetail.RootTable.Columns["PackingTypeId"].HasValueList = true;
			grdwagesDetail.RootTable.Columns["PackingTypeId"].LimitToList = true;
			grdwagesDetail.RootTable.Columns["PackingTypeId"].EditType = (EditType)4;
			grdwagesDetail.RootTable.Columns["PackingTypeId"].Caption = "Packing Type";
			grdwagesDetail.RootTable.Columns["PackingTypeId"].ValueList.PopulateValueList((IEnumerable)dtPackType.DefaultView, "Id", "PackTypeDesc");
			grdwagesDetail.RootTable.Columns["JobLotId"].ColumnType = (ColumnType)0;
			grdwagesDetail.RootTable.Columns["JobLotId"].HasValueList = true;
			grdwagesDetail.RootTable.Columns["JobLotId"].LimitToList = true;
			grdwagesDetail.RootTable.Columns["JobLotId"].EditType = (EditType)4;
			grdwagesDetail.RootTable.Columns["JobLotId"].Caption = "Job Lot";
			grdwagesDetail.RootTable.Columns["JobLotId"].ValueList.PopulateValueList((IEnumerable)dtJob.DefaultView, "Id", "JobLotDescription");
			grdwagesDetail.RootTable.Columns["WareHouseFromId"].ColumnType = (ColumnType)0;
			grdwagesDetail.RootTable.Columns["WareHouseFromId"].HasValueList = true;
			grdwagesDetail.RootTable.Columns["WareHouseFromId"].LimitToList = true;
			grdwagesDetail.RootTable.Columns["WareHouseFromId"].EditType = (EditType)4;
			grdwagesDetail.RootTable.Columns["WareHouseFromId"].Caption = "WareHouse From";
			grdwagesDetail.RootTable.Columns["WareHouseFromId"].ValueList.PopulateValueList((IEnumerable)dtWare.DefaultView, "Id", "WareHouseName");
			grdwagesDetail.RootTable.Columns["WareHouseToId"].ColumnType = (ColumnType)0;
			grdwagesDetail.RootTable.Columns["WareHouseToId"].HasValueList = true;
			grdwagesDetail.RootTable.Columns["WareHouseToId"].LimitToList = true;
			grdwagesDetail.RootTable.Columns["WareHouseToId"].EditType = (EditType)4;
			grdwagesDetail.RootTable.Columns["WareHouseToId"].Caption = "WareHouse To";
			grdwagesDetail.RootTable.Columns["WareHouseToId"].ValueList.PopulateValueList((IEnumerable)dtWare.DefaultView, "Id", "WareHouseName");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridSettings()
	{
		//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0619: Unknown result type (might be due to invalid IL or missing references)
		//IL_061f: Expected O, but got Unknown
		try
		{
			WagesItemAndContractorRefresh();
			grdwagesDetail.RootTable.Columns["Id"].Visible = false;
			grdwagesDetail.RootTable.Columns["WagesScheduleId"].Visible = false;
			grdwagesDetail.RootTable.Columns["IsCompany"].Visible = Party_ProcessingFeature;
			if (Conversion.ToInt(cmbReferenceDocType.Value) != 68 || Conversion.ToInt(cmbReferenceDocType.Value) != 806)
			{
				grdwagesDetail.RootTable.Columns["WareHouseToId"].Visible = false;
			}
			GridEX_Helper.GridWrappingAndColumnSettings(grdwagesDetail, 2, 3);
			grdwagesDetail.RootTable.Columns["Date"].Width = Constants.InventoryConstants.DateConstant + 10;
			grdwagesDetail.RootTable.Columns["ContractorId"].Width = Constants.InventoryConstants.SupplierName + 20;
			grdwagesDetail.RootTable.Columns["WagesAccountId"].Width = Constants.InventoryConstants.WagesActity;
			grdwagesDetail.RootTable.Columns["PackSize"].Width = Constants.InventoryConstants.UomConstant;
			grdwagesDetail.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			grdwagesDetail.RootTable.Columns["BillWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdwagesDetail.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdwagesDetail.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdwagesDetail.RootTable.Columns["ItemId"].Width = Constants.InventoryConstants.ItemName + 20;
			grdwagesDetail.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grdwagesDetail.RootTable.Columns["CropYearId"].Width = Constants.InventoryConstants.CropYearConstant;
			grdwagesDetail.RootTable.Columns["PackingTypeId"].Width = Constants.InventoryConstants.PackingType;
			grdwagesDetail.RootTable.Columns["JobLotId"].Width = Constants.InventoryConstants.JobLotConstant;
			grdwagesDetail.RootTable.Columns["WareHouseFromId"].Width = Constants.InventoryConstants.WareHouseName;
			grdwagesDetail.RootTable.Columns["WareHouseToId"].Width = Constants.InventoryConstants.WareHouseName;
			grdwagesDetail.RootTable.Columns["WagesType"].Width = Constants.InventoryConstants.DiscountType;
			grdwagesDetail.RootTable.Columns["Id"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["Date"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["WagesType"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdwagesDetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdwagesDetail.RootTable.Columns["Qty"].FormatString = "##,#.####";
			grdwagesDetail.RootTable.Columns["Qty"].TotalFormatString = "##,#.####";
			grdwagesDetail.RootTable.Columns["BillWeight"].AggregateFunction = (AggregateFunction)2;
			grdwagesDetail.RootTable.Columns["BillWeight"].TextAlignment = (TextAlignment)3;
			grdwagesDetail.RootTable.Columns["BillWeight"].FormatString = "##,#.####";
			grdwagesDetail.RootTable.Columns["BillWeight"].TotalFormatString = "##,#.####";
			grdwagesDetail.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)3;
			grdwagesDetail.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdwagesDetail.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdwagesDetail.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdwagesDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdwagesDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdwagesDetail.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdwagesDetail.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			new GridEXColumn();
			GridEXFormatCondition fc = new GridEXFormatCondition(grdwagesDetail.RootTable.Columns["WagesType"], (ConditionOperator)0, (object)"Free Of Cost");
			fc.FormatStyle.ForeColor = Color.Red;
			grdwagesDetail.RootTable.FormatConditions.Add(fc);
			grdwagesDetail.RootTable.Columns.Add("Delete");
			grdwagesDetail.RootTable.Columns["Delete"].Key = "Delete";
			grdwagesDetail.RootTable.Columns["Delete"].Caption = "X";
			grdwagesDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdwagesDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdwagesDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdwagesDetail.RootTable.Columns["Delete"].Width = 20;
			grdwagesDetail.RootTable.Columns["Delete"].Position = 0;
			grdwagesDetail.RootTable.Columns.Add("Add");
			grdwagesDetail.RootTable.Columns["Add"].Key = "Add";
			grdwagesDetail.RootTable.Columns["Add"].Caption = "Add";
			grdwagesDetail.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdwagesDetail.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdwagesDetail.RootTable.Columns["Add"].ButtonText = "Add";
			grdwagesDetail.RootTable.Columns["Add"].Width = 40;
			grdwagesDetail.RootTable.Columns["Add"].Position = 1;
			grdwagesDetail.RootTable.Columns.Add("Edit");
			grdwagesDetail.RootTable.Columns["Edit"].Key = "Edit";
			grdwagesDetail.RootTable.Columns["Edit"].Caption = "Edit";
			grdwagesDetail.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdwagesDetail.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdwagesDetail.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdwagesDetail.RootTable.Columns["Edit"].Width = 40;
			grdwagesDetail.RootTable.Columns["Edit"].Position = 2;
			grdwagesDetail.FrozenColumns = 3;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdwagesDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdwagesDetail.UpdateData();
			GridEXRow r = grdwagesDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				double WagesRate = 0.0;
				int ScheduleId = 0;
				r.BeginEdit();
				if (e.Column.Key == "ContractorId" || e.Column.Key == "WagesAccountId")
				{
					IsRegularOrFreeOfCost = CommonServices.CheckItemsFreeofcostforWages(Conversion.ToDateTime(r.Cells["Date"].Value), Conversion.ToInt(cmbReferenceDocType.Value), Conversion.ToInt(r.Cells["ItemId"].Value), Conversion.ToInt(r.Cells["WagesAccountId"].Value));
					if (IsRegularOrFreeOfCost)
					{
						r.Cells["WagesType"].Value = "Free Of Cost";
					}
					else
					{
						r.Cells["WagesType"].Value = "Regular";
					}
				}
				if ((e.Column.Key == "Qty" || e.Column.Key == "PackSize") && Conversion.ToDouble(r.Cells["Qty"].Value) > 0.0 && Conversion.ToDouble(r.Cells["PackSize"].Value) > 0.0)
				{
					double num = Conversion.ToDouble(r.Cells["Qty"].Value);
					double PackSize2 = Conversion.ToDouble(r.Cells["PackSize"].Value);
					double Weight = num * PackSize2;
					r.Cells["BillWeight"].Value = Math.Round(Weight, 2);
				}
				if (e.Column.Key == "BillWeight" && Conversion.ToDouble(r.Cells["BillWeight"].Value) > 0.0 && Conversion.ToDouble(r.Cells["PackSize"].Value) > 0.0)
				{
					double num2 = Conversion.ToDouble(r.Cells["BillWeight"].Value);
					double PackSize = Conversion.ToDouble(r.Cells["PackSize"].Value);
					double Qty = num2 / PackSize;
					r.Cells["Qty"].Value = Math.Round(Qty, 2);
				}
				if (e.Column.Key == "WagesAccountId" || e.Column.Key == "ContractorId" || e.Column.Key == "Qty" || e.Column.Key == "PackSize" || e.Column.Key == "ItemId" || e.Column.Key == "BillWeight")
				{
					if (Conversion.ToString((object)r.Cells["WagesType"].Text) == "Free Of Cost")
					{
						r.Cells["Rate"].Value = 0;
						r.Cells["Amount"].Value = 0;
						r.Cells["WagesScheduleId"].Value = 0;
					}
					else
					{
						GetWagesSchedule d = CommonServices.GetWagesRate(Conversion.ToDateTime(r.Cells["Date"].Value), Conversion.ToDouble(r.Cells["PackSize"].Value), Conversion.ToInt(r.Cells["WagesAccountId"].Value), Conversion.ToInt(r.Cells["ContractorId"].Value));
						if (d != null)
						{
							WagesRate = d.WagesRate;
							ScheduleId = d.ScheduleId;
						}
					}
				}
				if (WagesRate > 0.0)
				{
					r.Cells["Rate"].Value = WagesRate;
					r.Cells["WagesScheduleId"].Value = ScheduleId;
					if (Conversion.ToDouble(r.Cells["Rate"].Value) > 0.0)
					{
						if (Conversion.ToDouble(r.Cells["BillWeight"].Value) > 0.0 && !WagesAmountCalculateOnQty)
						{
							double Packsize = Conversion.ToDouble(r.Cells["PackSize"].Value);
							double num3 = Conversion.ToDouble(r.Cells["BillWeight"].Value);
							double Rate = Conversion.ToDouble(r.Cells["Rate"].Value);
							double Amount = num3 / Packsize * Rate;
							r.Cells["Amount"].Value = Math.Round(Amount, 2);
						}
						else if (Conversion.ToDouble(r.Cells["Qty"].Value) > 0.0 && WagesAmountCalculateOnQty)
						{
							double num4 = Conversion.ToDouble(r.Cells["Qty"].Value);
							double Rate2 = Conversion.ToDouble(r.Cells["Rate"].Value);
							double Amount2 = num4 * Rate2;
							r.Cells["Amount"].Value = Math.Round(Amount2, 2);
						}
						else
						{
							r.Cells["Amount"].Value = 0;
						}
					}
					else
					{
						r.Cells["Amount"].Value = 0;
					}
				}
				else
				{
					r.Cells["Rate"].Value = 0;
					r.Cells["Amount"].Value = 0;
					r.Cells["WagesScheduleId"].Value = 0;
				}
				if (e.Column.Key == "ItemId" && dtitem.Select("Id=" + Conversion.ToInt(r.Cells["ItemId"].Value)).Length != 0)
				{
					DataRow foundRow = dtitem.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(r.Cells["ItemId"].Value));
					string ItemCode = ((foundRow != null) ? Conversion.ToString(foundRow["ItemCode"]) : "");
					r.Cells["ItemCode"].Value = ItemCode;
				}
				r.EndEdit();
			}
			grdwagesDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdwagesDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdwagesDetail.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				if (dtdetail.Rows.Count <= 1)
				{
					MessageBox.Show("You Can Not Delete All rows");
					return;
				}
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					dtdetail.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInGrid(grdwagesDetail, dtdetail);
			}
			if (e.Column.Key == "Edit")
			{
				grdwagesDetail_DoubleClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInGrid(GridEX Grd, DataTable dtGrid)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = Grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				DataRow dr = dtGrid.Rows[r.RowIndex];
				dtGrid.Rows.Add(dr.ItemArray);
				dtGrid.Rows[dtGrid.Rows.Count - 1]["Id"] = 0;
				Grd.DataSource = dtGrid;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtdocnumber.Text.Trim() == string.Empty || txtdocnumber.Text.Trim() == "0")
		{
			MessageBox.Show("Document Number Field Required");
			txtdocnumber.Focus();
			return false;
		}
		if (((Control)(object)cmbReferenceDocType).Text.Trim() == string.Empty || Conversion.ToInt(cmbReferenceDocType.Value) == 0)
		{
			MessageBox.Show("Document Type Field Required");
			cmbReferenceDocType.Focus();
			return false;
		}
		if ((Conversion.ToInt(cmbReferenceDocType.Value) == 112 || Conversion.ToInt(cmbReferenceDocType.Value) == 80) && Conversion.ToInt(CmbJobOrder.Value) == 0)
		{
			MessageBox.Show("JobOrder Field Required when Document Type is Production");
			CmbJobOrder.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((Control)(object)CmbContractor).Text.Trim() == string.Empty || Conversion.ToInt(CmbContractor.Value) == 0)
		{
			MessageBox.Show("Contractor Field Required");
			CmbContractor.Focus();
			return false;
		}
		if (((Control)(object)CmbWagesAccount).Text.Trim() == string.Empty || Conversion.ToInt(CmbWagesAccount.Value) == 0)
		{
			MessageBox.Show("Wages Account Field Required");
			CmbWagesAccount.Focus();
			return false;
		}
		if (txtPackSize.Text.Trim() == string.Empty || txtPackSize.Text.Trim() == "0")
		{
			MessageBox.Show("Pack Size Field Required");
			txtPackSize.Focus();
			return false;
		}
		if (txtQty.Text.Trim() == string.Empty || txtQty.Text.Trim() == "0")
		{
			MessageBox.Show("Qty Field Required");
			txtQty.Focus();
			return false;
		}
		if (txtWeight.Text.Trim() == string.Empty || txtWeight.Text.Trim() == "0")
		{
			MessageBox.Show("Weight Field Required");
			txtWeight.Focus();
			return false;
		}
		if (txtWagesRate.Text.Trim() == string.Empty || txtWagesRate.Text.Trim() == "0")
		{
			MessageBox.Show("WagesRate Field Required");
			txtWagesRate.Focus();
			return false;
		}
		if (txtAmount.Text.Trim() == string.Empty || txtAmount.Text.Trim() == "0")
		{
			MessageBox.Show("Amount Field Required");
			txtAmount.Focus();
			return false;
		}
		return true;
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			FormRest();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FormRest()
	{
		try
		{
			RECID = 0;
			updateDetailIndex = -1;
			((Control)(object)cmbReferenceDocType).Text = string.Empty;
			((Control)(object)cmbStockParty).Text = string.Empty;
			txtRemarks.Text = string.Empty;
			GenerateDocNo();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			dtdetail.Rows.Clear();
			grdwagesDetail.ClearStructure();
			FormRestDetail();
			txtDocdate.Focus();
			if (Conversion.ToInt(cmbReferenceDocType.Value) == 112 || Conversion.ToInt(cmbReferenceDocType.Value) == 80)
			{
				((Control)(object)CmbJobOrder).Visible = true;
				((Control)(object)CmbJobOrder).Enabled = true;
				labelJobOrder.Visible = true;
			}
			else
			{
				((Control)(object)CmbJobOrder).Visible = false;
				((Control)(object)CmbJobOrder).Enabled = false;
				labelJobOrder.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FormRestDetail()
	{
		try
		{
			updateDetailIndex = -1;
			WagesScheduleId = 0;
			((Control)(object)CmbContractor).Text = string.Empty;
			((Control)(object)CmbWagesAccount).Text = string.Empty;
			txtPackSize.Text = string.Empty;
			txtQty.Text = string.Empty;
			txtWeight.Text = string.Empty;
			txtWagesRate.Text = string.Empty;
			txtAmount.Text = string.Empty;
			((Control)(object)comItem).Text = string.Empty;
			((Control)(object)CmbCropYear).Text = string.Empty;
			((Control)(object)CmbPackingType).Text = string.Empty;
			((Control)(object)CmbJobLot).Text = string.Empty;
			((Control)(object)CmbWarehouseFrom).Text = string.Empty;
			((Control)(object)CmbWareHouseTo).Text = string.Empty;
			txtwagesType.Text = string.Empty;
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			CmbContractor.Focus();
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

	private void button1_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			WagesAmountCalculationsConfigurations();
			SupplierCustomerFill();
			SupplierCustomerFill();
			WagesAccountFill();
			ItemFill();
			PackingTypeFill();
			WareHouseFill();
			CropYearFill();
			JobLotFill();
			DocumentTypeFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void frmWagesBillManualFormClosing(object sender, FormClosingEventArgs e)
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

	private void btnAdd_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "rdbtnItemName")
			{
				CmbContractor.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Expected O, but got Unknown
		try
		{
			if (grdwagesDetail.GetRows().Count() == 0)
			{
				throw new Exception("Regular Wages Grid... Record Not Found");
			}
			if (!FormValidation())
			{
				return;
			}
			InvContractorWagesBillHeader obj = new InvContractorWagesBillHeader();
			if (RECID > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.Id = RECID;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			bool IsCompany = true;
			GridEXRow[] rows = grdwagesDetail.GetRows();
			for (int i = 0; i < rows.Length; i++)
			{
				if (!Conversion.ToBool(rows[i].Cells["IsCompany"].Value))
				{
					IsCompany = false;
					break;
				}
			}
			if (!IsCompany && (((UltraGridBase)cmbStockParty).ActiveRow == null || Conversion.ToInt(cmbStockParty.Value) == 0))
			{
				cmbStockParty.Focus();
				throw new Exception("StockParty Field required For third Party Rows In Regular Wages Grid...");
			}
			obj.CompanyId = UserAccount.CompanyId;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 810;
			obj.EntryUser = Conversion.ToInt((object)UserAccount.ID);
			obj.ModifyUser = Conversion.ToInt((object)UserAccount.ID);
			obj.ModifyDate = DateTime.Now;
			obj.EntryDate = DateTime.Now;
			if (chkisapprove.Checked)
			{
				obj.IsAproved = true;
				obj.ApprovedDate = DateTime.Now;
				obj.ApprovedUserId = Conversion.ToInt((object)UserAccount.ID);
			}
			else
			{
				obj.IsAproved = false;
			}
			obj.DocNo = Conversion.ToInt((object)txtdocnumber.Text);
			obj.DocDate = txtDocdate.Value;
			obj.RefDocumentTypeId = Conversion.ToInt(cmbReferenceDocType.Value);
			obj.JobOrderId = Conversion.ToInt(CmbJobOrder.Value);
			obj.RefDocument = Conversion.ToString((object)((Control)(object)cmbReferenceDocType).Text.Trim());
			obj.OtherRemarks = Conversion.ToString((object)txtRemarks.Text);
			obj.StockPartyId = Conversion.ToInt(cmbStockParty.Value);
			obj.invContractWagesBillDateil = new List<InvContractorWagesBillDetail>();
			double Qty = 0.0;
			double weight = 0.0;
			rows = grdwagesDetail.GetRows();
			foreach (GridEXRow r in rows)
			{
				InvContractorWagesBillDetail wd = new InvContractorWagesBillDetail();
				wd.Id = Conversion.ToInt(r.Cells["Id"].Value);
				wd.RefDocDate = Conversion.ToDateTime(r.Cells["Date"].Value);
				if (Conversion.ToInt(r.Cells["ContractorId"].Value) == 0 || r.Cells["ContractorId"].Text.ToString() == string.Empty)
				{
					throw new Exception("ContractorAccount Field required In Regular Wages Grid... and Row No is " + (r.RowIndex + 1));
				}
				wd.ContractorId = Conversion.ToInt(r.Cells["ContractorId"].Value);
				wd.CompanyName = Conversion.ToString((object)r.Cells["ContractorId"].Text);
				if (Conversion.ToInt(r.Cells["WagesAccountId"].Value) == 0)
				{
					throw new Exception("WagesAccount Field required In Regular Wages Grid... and Row No is " + (r.RowIndex + 1));
				}
				wd.InvConractorWagesAccountsId = Conversion.ToInt(r.Cells["WagesAccountId"].Value);
				wd.WagesAccountName = Conversion.ToString((object)r.Cells["WagesAccountId"].Text);
				if (Conversion.ToDouble(r.Cells["PackSize"].Value) == 0.0 || r.Cells["PackSize"].Text.ToString() == string.Empty)
				{
					throw new Exception("PackSize Field required In Regular Wages Grid... and Row No is " + (r.RowIndex + 1));
				}
				wd.PackSize = Conversion.ToInt((object)r.Cells["PackSize"].Text);
				if (Conversion.ToInt(r.Cells["Qty"].Value) == 0 || r.Cells["Qty"].Text.ToString() == string.Empty)
				{
					throw new Exception("Qty Field required In Regular Wages Grid... and Row No is " + (r.RowIndex + 1));
				}
				wd.Qty = Conversion.ToDouble(r.Cells["Qty"].Value);
				wd.BillQty = Conversion.ToDouble(r.Cells["Qty"].Value);
				Qty += wd.Qty;
				if (Conversion.ToDouble(r.Cells["BillWeight"].Value) == 0.0 || r.Cells["BillWeight"].Text.ToString() == string.Empty)
				{
					throw new Exception("BillWeight Field required In Regular Wages Grid... and Row No is " + (r.RowIndex + 1));
				}
				wd.Weight = Conversion.ToDouble((object)r.Cells["BillWeight"].Text);
				wd.BillWeight = Conversion.ToDouble(r.Cells["BillWeight"].Value);
				weight += wd.Weight;
				if (Conversion.ToString((object)r.Cells["WagesType"].Text) == "Free Of Cost")
				{
					wd.WageRate = 0.0;
					wd.WagesAmount = 0.0;
					wd.InvContractorWagesScheduleId = 0;
				}
				else
				{
					wd.WageRate = Conversion.ToDouble((object)r.Cells["Rate"].Text);
					if (wd.WageRate == 0.0)
					{
						throw new Exception("Rate Field required In Wages Grid... and Row No is " + (r.RowIndex + 1));
					}
					if (WagesAmountCalculateOnQty)
					{
						wd.WagesAmount = Math.Round(wd.Qty * wd.WageRate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					}
					else
					{
						wd.WagesAmount = Math.Round(wd.Weight / wd.PackSize * wd.WageRate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					}
				}
				wd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				wd.RefDocumentTypeId = Conversion.ToInt(cmbReferenceDocType.Value);
				wd.JobOrderId = Conversion.ToInt(CmbJobOrder.Value);
				wd.Crop = Conversion.ToString((object)r.Cells["CropYearId"].Text);
				wd.InvPackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
				wd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
				wd.WareHouseFromId = Conversion.ToInt(r.Cells["WareHouseFromId"].Value);
				wd.WareHouseToId = Conversion.ToInt(r.Cells["WareHouseToId"].Value);
				wd.IsCompany = Conversion.ToBool(r.Cells["IsCompany"].Value);
				wd.InvContractorWagesScheduleId = Conversion.ToInt(r.Cells["WagesScheduleId"].Value);
				if (Conversion.ToString((object)r.Cells["WagesType"].Text) == "Free Of Cost")
				{
					wd.WagesTypeId = 1;
					wd.FreeOfCost = true;
					wd.InvContractorWagesScheduleId = 0;
				}
				else
				{
					wd.WagesTypeId = 2;
					wd.FreeOfCost = false;
				}
				if (!Party_ProcessingFeature)
				{
					wd.IsCompany = true;
				}
				obj.invContractWagesBillDateil.Add(wd);
			}
			obj.QtyTotal = Conversion.ToDouble((object)Qty);
			obj.WeightTotal = Conversion.ToDouble((object)weight);
			int code = InvContractorWagesBillHeader.Save(obj);
			if (RECID > 0)
			{
				MessageBox.Show("Updated SuccessFully  [" + obj.DocNo + "]");
			}
			else
			{
				MessageBox.Show("Saved SuccessFully  [" + obj.DocNo + "]");
			}
			FormRest();
			if (ChkPreview.Checked)
			{
				CommonServices.ContractorWagesBillManualSlip004(code);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			RECID = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int ID)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		try
		{
			RECID = ID;
			InvContractorWagesBillHeader head = new InvContractorWagesBillHeader();
			head = InvContractorWagesBillHeader.GetByID(RECID);
			if (head != null && head.invContractWagesBillDateil.Count > 0)
			{
				tabIncomingLabourwages.SelectedIndex = 0;
				txtDocdate.Value = head.DocDate;
				txtdocnumber.Text = head.DocNo.ToString();
				cmbReferenceDocType.Value = head.RefDocumentTypeId;
				if (Conversion.ToInt(cmbReferenceDocType.Value) == 112 || Conversion.ToInt(cmbReferenceDocType.Value) == 80)
				{
					((Control)(object)CmbJobOrder).Visible = true;
					((Control)(object)CmbJobOrder).Enabled = true;
					labelJobOrder.Visible = true;
					JobOrderNoFill();
				}
				else
				{
					((Control)(object)CmbJobOrder).Visible = false;
					((Control)(object)CmbJobOrder).Enabled = false;
					labelJobOrder.Visible = false;
				}
				CmbJobOrder.Value = head.JobOrderId;
				cmbStockParty.Value = head.StockPartyId;
				txtRemarks.Text = head.OtherRemarks;
				chkisapprove.Checked = head.IsAproved;
				dtdetail.Rows.Clear();
				for (int i = 0; i < head.invContractWagesBillDateil.Count; i++)
				{
					dtdetail.Rows.Add(head.invContractWagesBillDateil[i].Id, Conversion.ToDateTime((object)head.invContractWagesBillDateil[i].RefDocDate).ToString("dd-MMM-yyyy"), head.invContractWagesBillDateil[i].ContractorId, head.invContractWagesBillDateil[i].InvConractorWagesAccountsId, head.invContractWagesBillDateil[i].PackSize, head.invContractWagesBillDateil[i].BillQty, head.invContractWagesBillDateil[i].BillWeight, head.invContractWagesBillDateil[i].WageRate, head.invContractWagesBillDateil[i].WagesAmount, head.invContractWagesBillDateil[i].ItemId, head.invContractWagesBillDateil[i].ItemCode, head.invContractWagesBillDateil[i].Crop, head.invContractWagesBillDateil[i].InvPackingTypeId, head.invContractWagesBillDateil[i].JobLotId, head.invContractWagesBillDateil[i].WareHouseFromId, head.invContractWagesBillDateil[i].WareHouseToId, head.invContractWagesBillDateil[i].FreeOfCost ? "Free Of Cost" : "Regular", head.invContractWagesBillDateil[i].IsCompany, head.invContractWagesBillDateil[i].InvContractorWagesScheduleId);
				}
				grdwagesDetail.DataSource = dtdetail;
				grdwagesDetail.RetrieveStructure();
				DetailGridSettings();
				UpdateMode = true;
				btnsave.Visible = false;
				btnUpdate.Visible = true;
				return;
			}
			FormRest();
			throw new Exception("Record Not found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateDetailWeight()
	{
		try
		{
			decimal unit = default(decimal);
			decimal qty = default(decimal);
			decimal ItemWeight = default(decimal);
			qty = Conversion.ToDecimal((object)txtQty.Text);
			unit = Conversion.ToDecimal((object)txtPackSize.Text);
			if (unit > 0m && qty > 0m)
			{
				ItemWeight = Conversion.ToDecimal((object)(unit * qty));
				txtWeight.Text = ItemWeight.ToString("##,#.###");
			}
			else
			{
				txtWeight.Text = default(decimal).ToString("##,#.###");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateDetailAmount()
	{
		try
		{
			decimal unit = default(decimal);
			decimal qty = default(decimal);
			decimal ItemWeight = default(decimal);
			decimal Rate = default(decimal);
			decimal amount = default(decimal);
			qty = Conversion.ToDecimal((object)txtQty.Text);
			unit = Conversion.ToDecimal((object)txtPackSize.Text);
			Rate = Conversion.ToDecimal((object)txtWagesRate.Text);
			ItemWeight = Conversion.ToDecimal((object)txtWeight.Text);
			if (Rate > 0m && ItemWeight > 0m && unit > 0m && !WagesAmountCalculateOnQty)
			{
				amount = ItemWeight / unit * Rate;
				amount = Math.Round(amount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				txtAmount.Text = amount.ToString(clsGlobalVariables.stringFormatsingle);
			}
			if (Rate > 0m && qty > 0m && WagesAmountCalculateOnQty)
			{
				amount = qty * Rate;
				amount = Math.Round(amount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				txtAmount.Text = amount.ToString(clsGlobalVariables.stringFormatsingle);
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

	private void GetRateAmount()
	{
		try
		{
			if (Conversion.ToInt(CmbWagesAccount.Value) > 0)
			{
				string WagesType = "";
				IsRegularOrFreeOfCost = CommonServices.CheckItemsFreeofcostforWages(Conversion.ToDateTime((object)txtDocdate.Value), Conversion.ToInt(cmbReferenceDocType.Value), Conversion.ToInt(comItem.Value), Conversion.ToInt(CmbWagesAccount.Value));
				WagesType = ((!IsRegularOrFreeOfCost) ? "Regular" : "Free Of Cost");
				txtwagesType.Text = WagesType;
				if (WagesType == "Free Of Cost")
				{
					txtWagesRate.Text = 0.ToString(clsGlobalVariables.DecimalRateFormate);
					txtAmount.Text = 0.ToString(clsGlobalVariables.stringFormatsingle);
					WagesScheduleId = 0;
					return;
				}
				double WagesRate = 0.0;
				WagesScheduleId = 0;
				GetWagesSchedule d = CommonServices.GetWagesRate(Conversion.ToDateTime((object)txtDocdate.Value), Conversion.ToDouble((object)Conversion.ToDouble((object)txtPackSize.Text)), Conversion.ToInt(CmbWagesAccount.Value), Conversion.ToInt(CmbContractor.Value));
				if (d != null)
				{
					WagesRate = d.WagesRate;
					WagesScheduleId = d.ScheduleId;
				}
				txtWagesRate.Text = WagesRate.ToString(clsGlobalVariables.DecimalRateFormate);
			}
			else
			{
				WagesScheduleId = 0;
				txtWagesRate.Text = 0.ToString(clsGlobalVariables.DecimalRateFormate);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPackSize_TextChanged(object sender, EventArgs e)
	{
		try
		{
			GetRateAmount();
			CalculateDetailWeight();
			CalculateDetailAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateDetailWeight();
			CalculateDetailAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateDetailAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtWagesRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateDetailAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbWagesAccount_Leave(object sender, EventArgs e)
	{
		try
		{
			GetRateAmount();
			CalculateDetailWeight();
			CalculateDetailAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbContractor_Leave(object sender, EventArgs e)
	{
		try
		{
			GetRateAmount();
			CalculateDetailWeight();
			CalculateDetailAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDocdate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			GetRateAmount();
			CalculateDetailWeight();
			CalculateDetailAmount();
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
			if (tabIncomingLabourwages.SelectedIndex == 1)
			{
				FromDateHistory.Focus();
			}
			else
			{
				txtDocdate.Focus();
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
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			FromDocNoHistory.Text = string.Empty;
			ToDocNoHistory.Text = string.Empty;
			FromDateHistory.Focus();
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
			DocumentTypeFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			bindHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindHistory()
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
			obj.DocumentTypeId = 810;
			if (FromDateHistory.Checked)
			{
				obj.FromDate = Conversion.ToDateTime((object)FromDateHistory.Value);
			}
			if (ToDateHistory.Checked)
			{
				obj.ToDate = Conversion.ToDateTime((object)ToDateHistory.Value);
			}
			obj.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			obj.RefDocumentTypeId = Conversion.ToInt(CmbDocumentTypeHistory.Value);
			DataTable dtGrid = InvContractorWagesBillHeader.FormHistoryNew(obj);
			if (dtGrid.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("RecordNo", typeof(int));
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("RefDocumentTypeId", typeof(int));
				dt.Columns.Add("DocumentType", typeof(string));
				dt.Columns.Add("StockParty", typeof(string));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("DocNo", typeof(string));
				dt.Columns.Add("QtyTotal", typeof(decimal));
				dt.Columns.Add("WeightTotal", typeof(decimal));
				dt.Columns.Add("WagesAmount", typeof(decimal));
				dt.Columns.Add("Remarks", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				foreach (DataRow row in dtGrid.Rows)
				{
					dt.Rows.Add(row["RecordNo"], row["Id"], row["DocumentTypeId"], row["RefDocumentTypeId"], row["DocumentTypeDescription"], row["StockParty"], row["DocDate"], row["DocNo"], row["QtyTotal"], row["WeightTotal"], row["WagesAmount"], row["OtherRemarks"], row["EntryDate"], row["UserName"], row["ModifyDate"], row["ModifyUser"]);
				}
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				DataGridHistory.ClearStructure();
				GrdHistoryDetail.ClearStructure();
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
			DataGridHistory.AllowDelete = (InheritableBoolean)2;
			DataGridHistory.AllowEdit = (InheritableBoolean)2;
			DataGridHistory.ColumnAutoResize = false;
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["StockParty"].Visible = Party_ProcessingFeature;
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 2, 3);
			DataGridHistory.RootTable.Columns["DocumentType"].Width = Constants.InventoryConstants.DocumentType;
			DataGridHistory.RootTable.Columns["StockParty"].Width = Constants.InventoryConstants.ReferenceParty;
			DataGridHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant + 10;
			DataGridHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			DataGridHistory.RootTable.Columns["QtyTotal"].Width = Constants.InventoryConstants.QtyConstant;
			DataGridHistory.RootTable.Columns["WeightTotal"].Width = Constants.InventoryConstants.WeightConstant;
			DataGridHistory.RootTable.Columns["WagesAmount"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DataGridHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			DataGridHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DataGridHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			DataGridHistory.RootTable.Columns["WagesAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["WagesAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["WagesAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["WagesAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["QtyTotal"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["QtyTotal"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["QtyTotal"].FormatString = "#,##.##";
			DataGridHistory.RootTable.Columns["QtyTotal"].TotalFormatString = "#,##.##";
			DataGridHistory.RootTable.Columns["WeightTotal"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["WeightTotal"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["WeightTotal"].FormatString = "#,##.##";
			DataGridHistory.RootTable.Columns["WeightTotal"].TotalFormatString = "#,##.##";
			DataGridHistory.RootTable.Columns.Add("Slip");
			DataGridHistory.RootTable.Columns["Slip"].Caption = "Slip";
			DataGridHistory.RootTable.Columns["Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Slip"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Slip"].ButtonText = "Slip";
			DataGridHistory.RootTable.Columns["Slip"].Width = 40;
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 40;
			DataGridHistory.RootTable.Columns.Add("Voucher");
			DataGridHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			DataGridHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			DataGridHistory.RootTable.Columns["Voucher"].Width = 60;
			DataGridHistory.RootTable.Columns["Edit"].Position = 0;
			DataGridHistory.RootTable.Columns["Slip"].Position = 1;
			DataGridHistory.RootTable.Columns["Voucher"].Position = 2;
			DataGridHistory.FrozenColumns = 3;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				ReadById(Conversion.ToInt((object)item.Cells["Id"].Value.ToString()));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				if (e.Column.Key == "Voucher")
				{
					CommonServices.VoucherReport_118(CommonServices.VoucherHeadIdGet(Conversion.ToInt(item.Cells["Id"].Value), 810), 810);
				}
				if (e.Column.Key == "Slip")
				{
					CommonServices.ContractorWagesBillManualSlip004(Conversion.ToInt((object)item.Cells["Id"].Value.ToString()));
				}
				if (e.Column.Key == "Edit")
				{
					ReadById(Conversion.ToInt((object)item.Cells["Id"].Value.ToString()));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryDetailGridBind(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InvContractorWagesBillHeader head = new InvContractorWagesBillHeader();
			head = InvContractorWagesBillHeader.GetByID(Id);
			if (head != null && head.invContractWagesBillDateil.Count > 0)
			{
				DataTable dtdetailHistory = new DataTable();
				dtdetailHistory.Columns.Add("Id", typeof(int));
				dtdetailHistory.Columns.Add("Contractor", typeof(string));
				dtdetailHistory.Columns.Add("WagesAccount", typeof(string));
				dtdetailHistory.Columns.Add("PackSize", typeof(double));
				dtdetailHistory.Columns.Add("Qty", typeof(double));
				dtdetailHistory.Columns.Add("BillWeight", typeof(double));
				dtdetailHistory.Columns.Add("Rate", typeof(double));
				dtdetailHistory.Columns.Add("Amount", typeof(double));
				dtdetailHistory.Columns.Add("ItemId", typeof(int));
				dtdetailHistory.Columns.Add("ItemName", typeof(string));
				dtdetailHistory.Columns.Add("ItemCode", typeof(string));
				dtdetailHistory.Columns.Add("CropYear", typeof(string));
				dtdetailHistory.Columns.Add("PackingType", typeof(string));
				dtdetailHistory.Columns.Add("JobLot", typeof(string));
				dtdetailHistory.Columns.Add("WareHouseFrom", typeof(string));
				dtdetailHistory.Columns.Add("WareHouseTo", typeof(string));
				dtdetailHistory.Columns.Add("WagesType", typeof(string));
				dtdetailHistory.Columns.Add("IsCompany", typeof(bool));
				for (int i = 0; i < head.invContractWagesBillDateil.Count; i++)
				{
					dtdetailHistory.Rows.Add(head.invContractWagesBillDateil[i].Id, head.invContractWagesBillDateil[i].CompanyName, head.invContractWagesBillDateil[i].WagesAccountName, head.invContractWagesBillDateil[i].PackSize, head.invContractWagesBillDateil[i].BillQty, head.invContractWagesBillDateil[i].BillWeight, head.invContractWagesBillDateil[i].WageRate, head.invContractWagesBillDateil[i].WagesAmount, head.invContractWagesBillDateil[i].ItemId, head.invContractWagesBillDateil[i].ItemName, head.invContractWagesBillDateil[i].ItemCode, head.invContractWagesBillDateil[i].Crop, head.invContractWagesBillDateil[i].PackTypeDesc, head.invContractWagesBillDateil[i].JobLotDescription, head.invContractWagesBillDateil[i].WareHouseFrom, head.invContractWagesBillDateil[i].WareHouseTo, head.invContractWagesBillDateil[i].FreeOfCost ? "Free Of Cost" : "Regular", head.invContractWagesBillDateil[i].IsCompany);
				}
				GrdHistoryDetail.DataSource = dtdetailHistory;
				GrdHistoryDetail.RetrieveStructure();
				HistoryDetailGridSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryDetailGridSettings()
	{
		//IL_05f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_061a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0620: Expected O, but got Unknown
		try
		{
			GrdHistoryDetail.AllowDelete = (InheritableBoolean)2;
			GrdHistoryDetail.AllowEdit = (InheritableBoolean)2;
			GrdHistoryDetail.ColumnAutoResize = false;
			GrdHistoryDetail.RootTable.Columns["Id"].Visible = false;
			GrdHistoryDetail.RootTable.Columns["ItemId"].Visible = false;
			GrdHistoryDetail.RootTable.Columns["IsCompany"].Visible = Party_ProcessingFeature;
			GrdHistoryDetail.RootTable.Columns["WagesAccount"].Caption = "Labour / Wages Activity";
			GridEX_Helper.GridWrappingAndColumnSettings(GrdHistoryDetail, 2, 3);
			GrdHistoryDetail.RootTable.Columns["Contractor"].Width = Constants.InventoryConstants.SupplierName + 20;
			GrdHistoryDetail.RootTable.Columns["WagesAccount"].Width = Constants.InventoryConstants.WagesActity;
			GrdHistoryDetail.RootTable.Columns["PackSize"].Width = Constants.InventoryConstants.UomConstant;
			GrdHistoryDetail.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			GrdHistoryDetail.RootTable.Columns["BillWeight"].Width = Constants.InventoryConstants.WeightConstant;
			GrdHistoryDetail.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			GrdHistoryDetail.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			GrdHistoryDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			GrdHistoryDetail.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			GrdHistoryDetail.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			GrdHistoryDetail.RootTable.Columns["PackingType"].Width = Constants.InventoryConstants.PackingType;
			GrdHistoryDetail.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			GrdHistoryDetail.RootTable.Columns["WareHouseFrom"].Width = Constants.InventoryConstants.WareHouseName;
			GrdHistoryDetail.RootTable.Columns["WareHouseTo"].Width = Constants.InventoryConstants.WareHouseName;
			GrdHistoryDetail.RootTable.Columns["WagesType"].Width = Constants.InventoryConstants.DiscountType;
			GrdHistoryDetail.RootTable.Columns["IsCompany"].Width = 70;
			if (Conversion.ToInt(DataGridHistory.CurrentRow.Cells["RefDocumentTypeId"].Value) != 68 || Conversion.ToInt(DataGridHistory.CurrentRow.Cells["RefDocumentTypeId"].Value) != 806)
			{
				GrdHistoryDetail.RootTable.Columns["WareHouseTo"].Visible = false;
			}
			GrdHistoryDetail.RootTable.Columns["Id"].EditType = (EditType)0;
			GrdHistoryDetail.RootTable.Columns["WagesType"].EditType = (EditType)0;
			GrdHistoryDetail.RootTable.Columns["Rate"].EditType = (EditType)0;
			GrdHistoryDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["Qty"].FormatString = "##,#.####";
			GrdHistoryDetail.RootTable.Columns["Qty"].TotalFormatString = "##,#.####";
			GrdHistoryDetail.RootTable.Columns["BillWeight"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["BillWeight"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["BillWeight"].FormatString = "##,#.####";
			GrdHistoryDetail.RootTable.Columns["BillWeight"].TotalFormatString = "##,#.####";
			GrdHistoryDetail.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)3;
			GrdHistoryDetail.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			GrdHistoryDetail.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			GrdHistoryDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GrdHistoryDetail.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			new GridEXColumn();
			GridEXFormatCondition fc = new GridEXFormatCondition(GrdHistoryDetail.RootTable.Columns["WagesType"], (ConditionOperator)0, (object)"Free Of Cost");
			fc.FormatStyle.ForeColor = Color.Red;
			GrdHistoryDetail.RootTable.FormatConditions.Add(fc);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				HistoryDetailGridBind(Conversion.ToInt((object)item.Cells["Id"].Value.ToString()));
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdwagesDetail).Name);
			if (fs != null)
			{
				grdwagesDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void Print_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.ContractorWagesBillManualSlip004(RECID);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnWagesSchedule_Click(object sender, EventArgs e)
	{
		try
		{
			frmContractWagesSchedule obj = new frmContractWagesSchedule(UserAccount);
			obj.Show();
			obj.BringToFront();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnWagesExempt_Click(object sender, EventArgs e)
	{
		try
		{
			new WagesExemptItemSchedule(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPackSize_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtWeight_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmWagesBillManualKeyDown(object sender, KeyEventArgs e)
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
					txtDocdate.Focus();
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
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					txtDocdate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdwagesDetail).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					CmbContractor.Focus();
				}
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 121;
				}
				if (e.Control && e.KeyCode == Keys.P)
				{
					Print_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
				{
					Print_Click(null, null);
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				((Control)(object)DataGridHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)GrdHistoryDetail).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)GrdHistoryDetail).Focused)
				{
					((Control)(object)DataGridHistory).Focus();
				}
				else
				{
					((Control)(object)GrdHistoryDetail).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				((Control)(object)DataGridHistory).Focus();
			}
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
			dt.Rows.Add("Alt+1", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+D", "For Adding an row in Focused Grid");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Contractor Name in Detail Box");
			dt.Rows.Add("Ctrl+ArrowRight", "For Change Focus from one Grid To another Grid");
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
	}

	private void grdwagesDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdwagesDetail.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdwagesDetail.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdwagesDetail.CurrentColumn.Key == "Delete")
				{
					if (dtdetail.Rows.Count <= 1)
					{
						MessageBox.Show("You Can Not Delete All rows");
						return;
					}
					if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
					{
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						dtdetail.Rows[r.RowIndex].Delete();
						r.Delete();
					}
					else if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					grdwagesDetail.UpdateData();
				}
				if (grdwagesDetail.CurrentColumn.Key == "Edit")
				{
					grdwagesDetail_DoubleClick(null, null);
				}
				if (grdwagesDetail.CurrentColumn.Key == "Add")
				{
					AddRowInGrid(grdwagesDetail, dtdetail);
				}
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (dtdetail.Rows.Count <= 1)
				{
					MessageBox.Show("You Can Not Delete All rows");
					return;
				}
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					dtdetail.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				grdwagesDetail.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdwagesDetail_DoubleClick(null, null);
			}
			if (e.KeyCode == Keys.F1)
			{
				double WagesRate = 0.0;
				int ScheduleId = 0;
				int Id = 0;
				if (grdwagesDetail.CurrentColumn != null && grdwagesDetail.CurrentColumn.Key == "ContractorId")
				{
					GrdPopUp grdPopUp = new GrdPopUp(dtcus, "Id", "CompanyName");
					grdPopUp.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp.ShowDialog();
					grdPopUp.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp.ReturnId);
					Conversion.ToString((object)grdPopUp.ReturnName);
					r.Cells["ContractorId"].Value = Id;
				}
				if (grdwagesDetail.CurrentColumn != null && grdwagesDetail.CurrentColumn.Key == "WagesAccountId")
				{
					GrdPopUp grdPopUp2 = new GrdPopUp(dtwagesacc, "Id", "WagesAccountName");
					grdPopUp2.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp2.ShowDialog();
					grdPopUp2.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp2.ReturnId);
					Conversion.ToString((object)grdPopUp2.ReturnName);
					r.Cells["WagesAccountId"].Value = Id;
				}
				if (grdwagesDetail.CurrentColumn != null && grdwagesDetail.CurrentColumn.Key == "ItemId")
				{
					GrdPopUp grdPopUp3 = new GrdPopUp(dtitem, "Id", "ItemName");
					grdPopUp3.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp3.ShowDialog();
					grdPopUp3.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp3.ReturnId);
					Conversion.ToString((object)grdPopUp3.ReturnName);
					r.Cells["ItemId"].Value = Id;
					if (dtitem.Select("Id=" + Conversion.ToInt(r.Cells["ItemId"].Value)).Length != 0)
					{
						DataRow foundRow = dtitem.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(r.Cells["ItemId"].Value));
						string ItemCode = ((foundRow != null) ? Conversion.ToString(foundRow["ItemCode"]) : "");
						r.Cells["ItemCode"].Value = ItemCode;
					}
				}
				if (grdwagesDetail.CurrentColumn != null && grdwagesDetail.CurrentColumn.Key == "CropYearId")
				{
					GrdPopUp grdPopUp4 = new GrdPopUp(dtCrop, "Id", "CropYear");
					grdPopUp4.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp4.ShowDialog();
					grdPopUp4.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp4.ReturnId);
					Conversion.ToString((object)grdPopUp4.ReturnName);
					r.Cells["CropYearId"].Value = Id;
				}
				if (grdwagesDetail.CurrentColumn != null && grdwagesDetail.CurrentColumn.Key == "PackingTypeId")
				{
					GrdPopUp grdPopUp5 = new GrdPopUp(dtPackType, "Id", "PackTypeDesc");
					grdPopUp5.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp5.ShowDialog();
					grdPopUp5.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp5.ReturnId);
					Conversion.ToString((object)grdPopUp5.ReturnName);
					r.Cells["PackingTypeId"].Value = Id;
				}
				if (grdwagesDetail.CurrentColumn != null && grdwagesDetail.CurrentColumn.Key == "JobLotId")
				{
					GrdPopUp grdPopUp6 = new GrdPopUp(dtJob, "Id", "JobLotDescription");
					grdPopUp6.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp6.ShowDialog();
					grdPopUp6.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp6.ReturnId);
					Conversion.ToString((object)grdPopUp6.ReturnName);
					r.Cells["JobLotId"].Value = Id;
				}
				if (grdwagesDetail.CurrentColumn != null && grdwagesDetail.CurrentColumn.Key == "WareHouseFromId")
				{
					GrdPopUp grdPopUp7 = new GrdPopUp(dtWare, "Id", "WareHouseName");
					grdPopUp7.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp7.ShowDialog();
					grdPopUp7.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp7.ReturnId);
					Conversion.ToString((object)grdPopUp7.ReturnName);
					r.Cells["WareHouseFromId"].Value = Id;
				}
				if (grdwagesDetail.CurrentColumn != null && grdwagesDetail.CurrentColumn.Key == "WareHouseToId")
				{
					GrdPopUp grdPopUp8 = new GrdPopUp(dtWare, "Id", "WareHouseName");
					grdPopUp8.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp8.ShowDialog();
					grdPopUp8.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp8.ReturnId);
					Conversion.ToString((object)grdPopUp8.ReturnName);
					r.Cells["WareHouseToId"].Value = Id;
				}
				if (grdwagesDetail.CurrentColumn != null && (grdwagesDetail.CurrentColumn.Key == "ContractorId" || grdwagesDetail.CurrentColumn.Key == "WagesAccountId"))
				{
					IsRegularOrFreeOfCost = CommonServices.CheckItemsFreeofcostforWages(Conversion.ToDateTime(r.Cells["Date"].Value), Conversion.ToInt(cmbReferenceDocType.Value), Conversion.ToInt(r.Cells["ItemId"].Value), Conversion.ToInt(r.Cells["WagesAccountId"].Value));
					if (IsRegularOrFreeOfCost)
					{
						r.Cells["WagesType"].Value = "Free Of Cost";
					}
					else
					{
						r.Cells["WagesType"].Value = "Regular";
					}
					if (Conversion.ToString((object)r.Cells["WagesType"].Text) == "Free Of Cost")
					{
						r.Cells["Rate"].Value = 0;
						r.Cells["Amount"].Value = 0;
					}
					else
					{
						GetWagesSchedule d = CommonServices.GetWagesRate(Conversion.ToDateTime(r.Cells["Date"].Value), Conversion.ToDouble(r.Cells["PackSize"].Value), Conversion.ToInt(r.Cells["WagesAccountId"].Value), Conversion.ToInt(r.Cells["ContractorId"].Value));
						if (d != null)
						{
							WagesRate = d.WagesRate;
							ScheduleId = d.ScheduleId;
						}
					}
					if (WagesRate > 0.0)
					{
						r.Cells["Rate"].Value = WagesRate;
						r.Cells["WagesScheduleId"].Value = ScheduleId;
						if (Conversion.ToDouble(r.Cells["Rate"].Value) > 0.0 && Conversion.ToDouble(r.Cells["BillWeight"].Value) > 0.0 && !WagesAmountCalculateOnQty)
						{
							double Packsize = Conversion.ToDouble(r.Cells["PackSize"].Value);
							double num = Conversion.ToDouble(r.Cells["BillWeight"].Value);
							double Rate = Conversion.ToDouble(r.Cells["Rate"].Value);
							double Amount = num / Packsize * Rate;
							r.Cells["Amount"].Value = Math.Round(Amount, 2);
						}
						else if (Conversion.ToDouble(r.Cells["Rate"].Value) > 0.0 && Conversion.ToDouble(r.Cells["Qty"].Value) > 0.0 && !WagesAmountCalculateOnQty)
						{
							Conversion.ToDouble(r.Cells["PackSize"].Value);
							double num2 = Conversion.ToDouble(r.Cells["Qty"].Value);
							double Rate2 = Conversion.ToDouble(r.Cells["Rate"].Value);
							double Amount2 = num2 * Rate2;
							r.Cells["Amount"].Value = Math.Round(Amount2, 2);
						}
						else
						{
							r.Cells["Amount"].Value = 0;
						}
					}
					else
					{
						r.Cells["Rate"].Value = 0;
						r.Cells["Amount"].Value = 0;
						r.Cells["WagesScheduleId"].Value = 0;
					}
				}
				grdwagesDetail.UpdateData();
			}
			((Control)(object)grdwagesDetail).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || DataGridHistory.CurrentColumn == null)
			{
				return;
			}
			int CurrId = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DataGridHistory.CurrentColumn.Key == "Edit" && formright.DoHaveUpdateRights)
				{
					RECID = CurrId;
					ReadById(RECID);
					txtDocdate.Focus();
				}
				if (DataGridHistory.CurrentColumn.Key == "Voucher")
				{
					CommonServices.VoucherReport_118(CommonServices.VoucherHeadIdGet(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), 810), 810);
				}
				if (DataGridHistory.CurrentColumn.Key == "Slip")
				{
					CommonServices.ContractorWagesBillManualSlip004(CurrId);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return && formright.DoHaveUpdateRights)
			{
				RECID = CurrId;
				ReadById(RECID);
				txtDocdate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
			{
				CommonServices.ContractorWagesBillManualSlip004(CurrId);
			}
			((Control)(object)DataGridHistory).Focus();
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
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Expected O, but got Unknown
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Expected O, but got Unknown
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Expected O, but got Unknown
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Expected O, but got Unknown
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Expected O, but got Unknown
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Expected O, but got Unknown
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Expected O, but got Unknown
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Expected O, but got Unknown
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Expected O, but got Unknown
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Expected O, but got Unknown
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Expected O, but got Unknown
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Expected O, but got Unknown
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Expected O, but got Unknown
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Expected O, but got Unknown
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Expected O, but got Unknown
		//IL_055d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0567: Expected O, but got Unknown
		//IL_0568: Unknown result type (might be due to invalid IL or missing references)
		//IL_0572: Expected O, but got Unknown
		//IL_0573: Unknown result type (might be due to invalid IL or missing references)
		//IL_057d: Expected O, but got Unknown
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0588: Expected O, but got Unknown
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_0593: Expected O, but got Unknown
		//IL_0594: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Expected O, but got Unknown
		//IL_059f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a9: Expected O, but got Unknown
		//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b4: Expected O, but got Unknown
		//IL_0623: Unknown result type (might be due to invalid IL or missing references)
		//IL_062d: Expected O, but got Unknown
		//IL_0686: Unknown result type (might be due to invalid IL or missing references)
		//IL_0690: Expected O, but got Unknown
		//IL_0691: Unknown result type (might be due to invalid IL or missing references)
		//IL_069b: Expected O, but got Unknown
		//IL_0778: Unknown result type (might be due to invalid IL or missing references)
		//IL_0782: Expected O, but got Unknown
		//IL_07fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0806: Expected O, but got Unknown
		//IL_0849: Unknown result type (might be due to invalid IL or missing references)
		//IL_0853: Expected O, but got Unknown
		//IL_0f49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f53: Expected O, but got Unknown
		//IL_0f60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f6a: Expected O, but got Unknown
		//IL_6245: Unknown result type (might be due to invalid IL or missing references)
		//IL_624f: Expected O, but got Unknown
		GridEXLayout grdwagesDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Contractor_Wages.frmWagesBillManual));
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		GridEXLayout GrdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabIncomingLabourwages = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel13 = new System.Windows.Forms.Panel();
		this.grdwagesDetail = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label2 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.btnAdd = new System.Windows.Forms.Button();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.chkIsCompany = new System.Windows.Forms.CheckBox();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.label18 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.txtWagesRate = new System.Windows.Forms.TextBox();
		this.txtWeight = new System.Windows.Forms.TextBox();
		this.txtwagesType = new System.Windows.Forms.TextBox();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label42 = new System.Windows.Forms.Label();
		this.txtPackSize = new System.Windows.Forms.TextBox();
		this.rdbtnItemCode = new System.Windows.Forms.RadioButton();
		this.rdbtnItemName = new System.Windows.Forms.RadioButton();
		this.CmbWagesAccount = new UltraCombo();
		this.CmbContractor = new UltraCombo();
		this.CmbWareHouseTo = new UltraCombo();
		this.CmbWarehouseFrom = new UltraCombo();
		this.CmbJobLot = new UltraCombo();
		this.CmbCropYear = new UltraCombo();
		this.CmbPackingType = new UltraCombo();
		this.comItem = new UltraCombo();
		this.label41 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.cmbStockParty = new UltraCombo();
		this.labelStockParty = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.txtDocdate = new System.Windows.Forms.DateTimePicker();
		this.chkisapprove = new System.Windows.Forms.CheckBox();
		this.txtdocnumber = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbJobOrder = new UltraCombo();
		this.cmbReferenceDocType = new UltraCombo();
		this.labelJobOrder = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.ChkPreview = new System.Windows.Forms.CheckBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.btnWagesSchedule = new System.Windows.Forms.ToolStripButton();
		this.btnWagesExempt = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label3 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label51 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.label58 = new System.Windows.Forms.Label();
		this.label64 = new System.Windows.Forms.Label();
		this.label65 = new System.Windows.Forms.Label();
		this.CmbDocumentTypeHistory = new UltraCombo();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label10 = new System.Windows.Forms.Label();
		this.GrdHistoryDetail = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.label26 = new System.Windows.Forms.Label();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.panel1.SuspendLayout();
		this.tabIncomingLabourwages.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel6.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdwagesDetail).BeginInit();
		this.panel15.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWagesAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContractor).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouseTo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseFrom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comItem).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbStockParty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrder).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReferenceDocType).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel10.SuspendLayout();
		this.panel9.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDocumentTypeHistory).BeginInit();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).BeginInit();
		this.panel7.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabIncomingLabourwages);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1084, 661);
		this.panel1.TabIndex = 0;
		this.tabIncomingLabourwages.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabIncomingLabourwages.Controls.Add(this.tabPage1);
		this.tabIncomingLabourwages.Controls.Add(this.tabPage2);
		this.tabIncomingLabourwages.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabIncomingLabourwages.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabIncomingLabourwages.Location = new System.Drawing.Point(0, 0);
		this.tabIncomingLabourwages.Name = "tabIncomingLabourwages";
		this.tabIncomingLabourwages.SelectedIndex = 0;
		this.tabIncomingLabourwages.Size = new System.Drawing.Size(1084, 661);
		this.tabIncomingLabourwages.TabIndex = 0;
		this.tabIncomingLabourwages.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.BackColor = System.Drawing.Color.White;
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1076, 632);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.panel6.Controls.Add(this.tabControl1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 252);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1070, 377);
		this.panel6.TabIndex = 8;
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1070, 377);
		this.tabControl1.TabIndex = 0;
		this.tabPage3.Controls.Add(this.panel13);
		this.tabPage3.Controls.Add(this.panel15);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1062, 348);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Regular Wages";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grdwagesDetail);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(3, 30);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1056, 315);
		this.panel13.TabIndex = 7;
		this.grdwagesDetail.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdwagesDetail).BackColor = System.Drawing.Color.White;
		grdwagesDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdwagesDetail.DesignTimeLayout = grdwagesDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdwagesDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdwagesDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdwagesDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdwagesDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdwagesDetail).Name = "grdwagesDetail";
		((ScrollableBase)this.grdwagesDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdwagesDetail).Size = new System.Drawing.Size(1056, 315);
		((System.Windows.Forms.Control)(object)this.grdwagesDetail).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdwagesDetail).Tag = "WagesGrid";
		this.grdwagesDetail.TotalRow = (InheritableBoolean)1;
		this.grdwagesDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdwagesDetail).VisualStyle = (VisualStyle)4;
		this.grdwagesDetail.CellUpdated += new ColumnActionEventHandler(grdwagesDetail_CellUpdated);
		this.grdwagesDetail.ColumnButtonClick += new ColumnActionEventHandler(grdwagesDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdwagesDetail).DoubleClick += new System.EventHandler(grdwagesDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdwagesDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdwagesDetail_KeyDown);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.ctrlGrdBar2);
		this.panel15.Controls.Add(this.label2);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(3, 3);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1056, 27);
		this.panel15.TabIndex = 6;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1009, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdwagesDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 23);
		this.ctrlGrdBar2.TabIndex = 63;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.Teal;
		this.label2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label2.Location = new System.Drawing.Point(3, 6);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(266, 16);
		this.label2.TabIndex = 0;
		this.label2.Text = "In this Grid User Will add Regular Wages";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox2);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(3, 56);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1070, 196);
		this.panel4.TabIndex = 0;
		this.groupBox2.Controls.Add(this.btnAdd);
		this.groupBox2.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox2.Controls.Add(this.chkIsCompany);
		this.groupBox2.Controls.Add(this.btnUpdateDetail);
		this.groupBox2.Controls.Add(this.label18);
		this.groupBox2.Controls.Add(this.label17);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add(this.txtAmount);
		this.groupBox2.Controls.Add(this.txtWagesRate);
		this.groupBox2.Controls.Add(this.txtWeight);
		this.groupBox2.Controls.Add(this.txtwagesType);
		this.groupBox2.Controls.Add(this.txtQty);
		this.groupBox2.Controls.Add(this.label42);
		this.groupBox2.Controls.Add(this.txtPackSize);
		this.groupBox2.Controls.Add(this.rdbtnItemCode);
		this.groupBox2.Controls.Add(this.rdbtnItemName);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWagesAccount);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbContractor);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWareHouseTo);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouseFrom);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYear);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackingType);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.comItem);
		this.groupBox2.Controls.Add(this.label41);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add(this.label32);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.label19);
		this.groupBox2.Controls.Add(this.label33);
		this.groupBox2.Controls.Add(this.label31);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(1, 76);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1064, 117);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main Information";
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(920, 85);
		this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(39, 27);
		this.btnAdd.TabIndex = 15;
		this.btnAdd.Text = "+";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.btnAdd.Leave += new System.EventHandler(btnAdd_Leave);
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(990, 85);
		this.btnCancelUpdateDetial.Margin = new System.Windows.Forms.Padding(2);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(68, 27);
		this.btnCancelUpdateDetial.TabIndex = 16;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.chkIsCompany.AutoSize = true;
		this.chkIsCompany.Checked = true;
		this.chkIsCompany.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkIsCompany.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkIsCompany.Location = new System.Drawing.Point(920, 66);
		this.chkIsCompany.Name = "chkIsCompany";
		this.chkIsCompany.Size = new System.Drawing.Size(89, 19);
		this.chkIsCompany.TabIndex = 14;
		this.chkIsCompany.Text = "Is Company";
		this.chkIsCompany.UseVisualStyleBackColor = true;
		this.chkIsCompany.Visible = false;
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(920, 85);
		this.btnUpdateDetail.Margin = new System.Windows.Forms.Padding(2);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(70, 27);
		this.btnUpdateDetail.TabIndex = 15;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(763, 18);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(51, 15);
		this.label18.TabIndex = 144;
		this.label18.Text = "Amount";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(678, 18);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(30, 15);
		this.label17.TabIndex = 144;
		this.label17.Text = "Rate";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(583, 18);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(46, 15);
		this.label16.TabIndex = 144;
		this.label16.Text = "Weight";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(495, 18);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(26, 15);
		this.label14.TabIndex = 144;
		this.label14.Text = "Qty";
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Enabled = false;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAmount.Location = new System.Drawing.Point(759, 39);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.Size = new System.Drawing.Size(100, 23);
		this.txtAmount.TabIndex = 6;
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtWagesRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWagesRate.Enabled = false;
		this.txtWagesRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtWagesRate.Location = new System.Drawing.Point(676, 39);
		this.txtWagesRate.Name = "txtWagesRate";
		this.txtWagesRate.Size = new System.Drawing.Size(82, 23);
		this.txtWagesRate.TabIndex = 5;
		this.txtWagesRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtWagesRate.TextChanged += new System.EventHandler(txtWagesRate_TextChanged);
		this.txtWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtWeight.Location = new System.Drawing.Point(581, 39);
		this.txtWeight.Name = "txtWeight";
		this.txtWeight.Size = new System.Drawing.Size(93, 23);
		this.txtWeight.TabIndex = 4;
		this.txtWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtWeight.TextChanged += new System.EventHandler(txtWeight_TextChanged);
		this.txtWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtWeight_KeyPress);
		this.txtwagesType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtwagesType.Enabled = false;
		this.txtwagesType.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtwagesType.Location = new System.Drawing.Point(759, 87);
		this.txtwagesType.Name = "txtwagesType";
		this.txtwagesType.Size = new System.Drawing.Size(158, 23);
		this.txtwagesType.TabIndex = 13;
		this.txtwagesType.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtwagesType.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.txtwagesType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQty_KeyPress);
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtQty.Location = new System.Drawing.Point(493, 39);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(86, 23);
		this.txtQty.TabIndex = 3;
		this.txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQty.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQty_KeyPress);
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(411, 18);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(57, 15);
		this.label42.TabIndex = 142;
		this.label42.Text = "Pack Size";
		this.txtPackSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackSize.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPackSize.Location = new System.Drawing.Point(411, 39);
		this.txtPackSize.Name = "txtPackSize";
		this.txtPackSize.Size = new System.Drawing.Size(80, 23);
		this.txtPackSize.TabIndex = 2;
		this.txtPackSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPackSize.TextChanged += new System.EventHandler(txtPackSize_TextChanged);
		this.txtPackSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPackSize_KeyPress);
		this.rdbtnItemCode.AutoSize = true;
		this.rdbtnItemCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemCode.Location = new System.Drawing.Point(945, 16);
		this.rdbtnItemCode.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemCode.Name = "rdbtnItemCode";
		this.rdbtnItemCode.Size = new System.Drawing.Size(52, 19);
		this.rdbtnItemCode.TabIndex = 140;
		this.rdbtnItemCode.Text = "Code";
		this.rdbtnItemCode.UseVisualStyleBackColor = true;
		this.rdbtnItemName.AutoSize = true;
		this.rdbtnItemName.Checked = true;
		this.rdbtnItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemName.Location = new System.Drawing.Point(892, 16);
		this.rdbtnItemName.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemName.Name = "rdbtnItemName";
		this.rdbtnItemName.Size = new System.Drawing.Size(57, 19);
		this.rdbtnItemName.TabIndex = 139;
		this.rdbtnItemName.TabStop = true;
		this.rdbtnItemName.Text = "Name";
		this.rdbtnItemName.UseVisualStyleBackColor = true;
		this.CmbWagesAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbWagesAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWagesAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWagesAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWagesAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance65;
		((SpecialBoxBase)((UltraGridBase)this.CmbWagesAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWagesAccount).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWagesAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWagesAccount).Location = new System.Drawing.Point(224, 37);
		((System.Windows.Forms.Control)(object)this.CmbWagesAccount).Name = "CmbWagesAccount";
		((System.Windows.Forms.Control)(object)this.CmbWagesAccount).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.CmbWagesAccount).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbWagesAccount).Leave += new System.EventHandler(CmbWagesAccount_Leave);
		this.CmbContractor.AllowNull = (DefaultableBoolean)1;
		this.CmbContractor.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbContractor.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbContractor.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbContractor).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbContractor).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbContractor).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractor).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractor).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance116;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractor).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractor).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbContractor).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbContractor).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbContractor).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbContractor).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbContractor).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbContractor).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractor).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance4).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbContractor).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbContractor).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbContractor).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractor).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance26).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbContractor).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbContractor).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbContractor).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbContractor).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbContractor).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbContractor).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbContractor).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbContractor).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbContractor).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbContractor).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbContractor.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbContractor).Location = new System.Drawing.Point(5, 37);
		((System.Windows.Forms.Control)(object)this.CmbContractor).Name = "CmbContractor";
		((System.Windows.Forms.Control)(object)this.CmbContractor).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbContractor).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbContractor).Leave += new System.EventHandler(CmbContractor_Leave);
		this.CmbWareHouseTo.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouseTo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouseTo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouseTo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance55;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance79).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseTo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWareHouseTo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseTo).Location = new System.Drawing.Point(583, 85);
		((System.Windows.Forms.Control)(object)this.CmbWareHouseTo).Name = "CmbWareHouseTo";
		((System.Windows.Forms.Control)(object)this.CmbWareHouseTo).Size = new System.Drawing.Size(175, 26);
		((System.Windows.Forms.Control)(object)this.CmbWareHouseTo).TabIndex = 12;
		this.CmbWarehouseFrom.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouseFrom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouseFrom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouseFrom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance130;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance119).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance30).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseFrom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWarehouseFrom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseFrom).Location = new System.Drawing.Point(411, 85);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseFrom).Name = "CmbWarehouseFrom";
		((System.Windows.Forms.Control)(object)this.CmbWarehouseFrom).Size = new System.Drawing.Size(171, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseFrom).TabIndex = 11;
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(224, 85);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 10;
		this.CmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance19;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance24).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance27).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Location = new System.Drawing.Point(5, 85);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Name = "CmbCropYear";
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Size = new System.Drawing.Size(97, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabIndex = 8;
		this.CmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance52;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance58).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Location = new System.Drawing.Point(105, 85);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Name = "CmbPackingType";
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Size = new System.Drawing.Size(118, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).TabIndex = 9;
		this.comItem.AllowNull = (DefaultableBoolean)1;
		this.comItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.comItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.comItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.comItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.comItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.comItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.comItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.comItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.comItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.comItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comItem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.comItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comItem).Location = new System.Drawing.Point(860, 37);
		((System.Windows.Forms.Control)(object)this.comItem).Name = "comItem";
		((System.Windows.Forms.Control)(object)this.comItem).Size = new System.Drawing.Size(198, 26);
		((System.Windows.Forms.Control)(object)this.comItem).TabIndex = 7;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label41.Location = new System.Drawing.Point(5, 66);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(57, 15);
		this.label41.TabIndex = 137;
		this.label41.Text = "Crop Year";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(224, 18);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(134, 15);
		this.label13.TabIndex = 134;
		this.label13.Text = "Labour / Wages Activity";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label32.Location = new System.Drawing.Point(857, 18);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(32, 15);
		this.label32.TabIndex = 136;
		this.label32.Text = "Item";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(5, 18);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(63, 15);
		this.label8.TabIndex = 134;
		this.label8.Text = "Contractor";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(761, 66);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(71, 15);
		this.label4.TabIndex = 134;
		this.label4.Text = "Wages Type";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(583, 66);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(85, 15);
		this.label19.TabIndex = 134;
		this.label19.Text = "WareHouse To";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(224, 66);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(46, 15);
		this.label33.TabIndex = 135;
		this.label33.Text = "Job Lot";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(411, 66);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(100, 15);
		this.label31.TabIndex = 134;
		this.label31.Text = "WareHouse From";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(102, 66);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(77, 15);
		this.label9.TabIndex = 133;
		this.label9.Text = "Packing Type";
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbStockParty);
		this.groupBox1.Controls.Add(this.labelStockParty);
		this.groupBox1.Controls.Add(this.txtRemarks);
		this.groupBox1.Controls.Add(this.txtDocdate);
		this.groupBox1.Controls.Add(this.chkisapprove);
		this.groupBox1.Controls.Add(this.txtdocnumber);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrder);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbReferenceDocType);
		this.groupBox1.Controls.Add(this.labelJobOrder);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(1, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1060, 74);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main Information";
		this.cmbStockParty.AllowNull = (DefaultableBoolean)1;
		this.cmbStockParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbStockParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbStockParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbStockParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbStockParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbStockParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance132).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance132).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbStockParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStockParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance133;
		((SpecialBoxBase)((UltraGridBase)this.cmbStockParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStockParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.cmbStockParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbStockParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbStockParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbStockParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbStockParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbStockParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStockParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance139).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbStockParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.cmbStockParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbStockParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance140).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStockParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbStockParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.cmbStockParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbStockParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbStockParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbStockParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbStockParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.cmbStockParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbStockParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbStockParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbStockParty).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbStockParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbStockParty).Location = new System.Drawing.Point(676, 41);
		((System.Windows.Forms.Control)(object)this.cmbStockParty).Name = "cmbStockParty";
		((System.Windows.Forms.Control)(object)this.cmbStockParty).Size = new System.Drawing.Size(378, 26);
		((System.Windows.Forms.Control)(object)this.cmbStockParty).TabIndex = 6;
		this.labelStockParty.AutoSize = true;
		this.labelStockParty.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelStockParty.Location = new System.Drawing.Point(611, 47);
		this.labelStockParty.Name = "labelStockParty";
		this.labelStockParty.Size = new System.Drawing.Size(67, 15);
		this.labelStockParty.TabIndex = 108;
		this.labelStockParty.Text = "Stock Party";
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(676, 14);
		this.txtRemarks.Multiline = true;
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(378, 23);
		this.txtRemarks.TabIndex = 5;
		this.txtDocdate.Enabled = false;
		this.txtDocdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtDocdate.Location = new System.Drawing.Point(69, 44);
		this.txtDocdate.Name = "txtDocdate";
		this.txtDocdate.Size = new System.Drawing.Size(150, 23);
		this.txtDocdate.TabIndex = 1;
		this.txtDocdate.ValueChanged += new System.EventHandler(txtDocdate_ValueChanged);
		this.chkisapprove.AutoSize = true;
		this.chkisapprove.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkisapprove.Location = new System.Drawing.Point(516, 46);
		this.chkisapprove.Name = "chkisapprove";
		this.chkisapprove.Size = new System.Drawing.Size(90, 19);
		this.chkisapprove.TabIndex = 4;
		this.chkisapprove.Text = "Is Approved";
		this.chkisapprove.UseVisualStyleBackColor = true;
		this.chkisapprove.Visible = false;
		this.txtdocnumber.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtdocnumber.Location = new System.Drawing.Point(69, 16);
		this.txtdocnumber.Name = "txtdocnumber";
		this.txtdocnumber.ReadOnly = true;
		this.txtdocnumber.Size = new System.Drawing.Size(150, 23);
		this.txtdocnumber.TabIndex = 0;
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.White;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(611, 20);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(52, 15);
		this.label6.TabIndex = 59;
		this.label6.Text = "Remarks";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(7, 48);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(57, 15);
		this.label7.TabIndex = 24;
		this.label7.Text = "Doc Date";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(7, 20);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(48, 15);
		this.label1.TabIndex = 24;
		this.label1.Text = "Doc No";
		this.CmbJobOrder.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrder.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrder.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrder.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Appearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrder).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance87;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrder).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance92).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance95).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrder.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).Location = new System.Drawing.Point(319, 42);
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).Name = "CmbJobOrder";
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).Size = new System.Drawing.Size(191, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).Visible = false;
		this.cmbReferenceDocType.AllowNull = (DefaultableBoolean)1;
		this.cmbReferenceDocType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbReferenceDocType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbReferenceDocType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance99).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance99).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance100;
		((SpecialBoxBase)((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance106).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbReferenceDocType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbReferenceDocType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbReferenceDocType).Location = new System.Drawing.Point(319, 14);
		((System.Windows.Forms.Control)(object)this.cmbReferenceDocType).Name = "cmbReferenceDocType";
		((System.Windows.Forms.Control)(object)this.cmbReferenceDocType).Size = new System.Drawing.Size(287, 26);
		((System.Windows.Forms.Control)(object)this.cmbReferenceDocType).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbReferenceDocType).Leave += new System.EventHandler(cmbReferenceDocType_Leave);
		this.labelJobOrder.AutoSize = true;
		this.labelJobOrder.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelJobOrder.Location = new System.Drawing.Point(223, 48);
		this.labelJobOrder.Name = "labelJobOrder";
		this.labelJobOrder.Size = new System.Drawing.Size(59, 15);
		this.labelJobOrder.TabIndex = 106;
		this.labelJobOrder.Text = "Job Order";
		this.labelJobOrder.Visible = false;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(223, 20);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(92, 15);
		this.label11.TabIndex = 106;
		this.label11.Text = "Document Type";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label15);
		this.panel3.Controls.Add(this.ChkPreview);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 32);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1070, 24);
		this.panel3.TabIndex = 5;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(3, 3);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(124, 19);
		this.label15.TabIndex = 0;
		this.label15.Text = "Labour Wages";
		this.ChkPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPreview.AutoSize = true;
		this.ChkPreview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPreview.ForeColor = System.Drawing.Color.White;
		this.ChkPreview.Location = new System.Drawing.Point(963, 2);
		this.ChkPreview.Name = "ChkPreview";
		this.ChkPreview.Size = new System.Drawing.Size(102, 20);
		this.ChkPreview.TabIndex = 8;
		this.ChkPreview.Text = "Is Preview";
		this.ChkPreview.UseVisualStyleBackColor = true;
		this.ChkPreview.Visible = false;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1070, 29);
		this.panel2.TabIndex = 4;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[12]
		{
			this.btnnew, this.btnRefresh, this.toolStripSeparator4, this.btnsave, this.btnUpdate, this.toolStripSeparator5, this.Print, this.toolStripSeparator6, this.btnWagesSchedule, this.btnWagesExempt,
			this.toolStripSeparator3, this.btnshortcutkeys
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1070, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f);
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
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(92, 24);
		this.Print.Text = "&Print-004";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 27);
		this.btnWagesSchedule.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnWagesSchedule.Name = "btnWagesSchedule";
		this.btnWagesSchedule.Size = new System.Drawing.Size(119, 24);
		this.btnWagesSchedule.Text = "Wages Schedule";
		this.btnWagesSchedule.Click += new System.EventHandler(btnWagesSchedule_Click);
		this.btnWagesExempt.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnWagesExempt.Name = "btnWagesExempt";
		this.btnWagesExempt.Size = new System.Drawing.Size(142, 24);
		this.btnWagesExempt.Text = "Wages Exempt Item";
		this.btnWagesExempt.Click += new System.EventHandler(btnWagesExempt_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel12);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistoryDetail);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1076, 632);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 125);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1070, 228);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 68;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.ctrlGrdBar1);
		this.panel10.Controls.Add(this.label3);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 101);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1070, 24);
		this.panel10.TabIndex = 67;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1023, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 22);
		this.ctrlGrdBar1.TabIndex = 62;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.Teal;
		this.label3.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label3.Location = new System.Drawing.Point(3, 3);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(124, 18);
		this.label3.TabIndex = 0;
		this.label3.Text = "Filtered Record";
		this.panel9.BackColor = System.Drawing.Color.White;
		this.panel9.Controls.Add(this.groupBox4);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 54);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1070, 47);
		this.panel9.TabIndex = 0;
		this.groupBox4.Controls.Add(this.btnShowHistory);
		this.groupBox4.Controls.Add(this.label51);
		this.groupBox4.Controls.Add(this.label57);
		this.groupBox4.Controls.Add(this.label58);
		this.groupBox4.Controls.Add(this.label64);
		this.groupBox4.Controls.Add(this.label65);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDocumentTypeHistory);
		this.groupBox4.Controls.Add(this.ToDocNoHistory);
		this.groupBox4.Controls.Add(this.FromDocNoHistory);
		this.groupBox4.Controls.Add(this.ToDateHistory);
		this.groupBox4.Controls.Add(this.FromDateHistory);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(5, 1);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(1060, 44);
		this.groupBox4.TabIndex = 5;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Filters";
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(996, 12);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(59, 26);
		this.btnShowHistory.TabIndex = 5;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(696, 18);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(92, 15);
		this.label51.TabIndex = 87;
		this.label51.Text = "Document Type";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(554, 18);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(64, 15);
		this.label57.TabIndex = 86;
		this.label57.Text = "To Doc No";
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.Location = new System.Drawing.Point(398, 18);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(79, 15);
		this.label58.TabIndex = 85;
		this.label58.Text = "From Doc No";
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.Location = new System.Drawing.Point(206, 18);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(48, 15);
		this.label64.TabIndex = 84;
		this.label64.Text = "To Date";
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.Location = new System.Drawing.Point(6, 18);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(63, 15);
		this.label65.TabIndex = 83;
		this.label65.Text = "From Date";
		this.CmbDocumentTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbDocumentTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDocumentTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDocumentTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance113;
		((SpecialBoxBase)((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance122).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance123).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance123).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance123).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance124).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDocumentTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDocumentTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDocumentTypeHistory).Location = new System.Drawing.Point(789, 12);
		((System.Windows.Forms.Control)(object)this.CmbDocumentTypeHistory).Name = "CmbDocumentTypeHistory";
		((System.Windows.Forms.Control)(object)this.CmbDocumentTypeHistory).Size = new System.Drawing.Size(205, 26);
		((System.Windows.Forms.Control)(object)this.CmbDocumentTypeHistory).TabIndex = 4;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(621, 14);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(73, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(480, 14);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(73, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(260, 14);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(136, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(69, 14);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(136, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label10);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel12.Location = new System.Drawing.Point(3, 353);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1070, 24);
		this.panel12.TabIndex = 65;
		this.label10.AutoSize = true;
		this.label10.BackColor = System.Drawing.Color.Teal;
		this.label10.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label10.Location = new System.Drawing.Point(3, 5);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(323, 16);
		this.label10.TabIndex = 1;
		this.label10.Text = "This grid shows the Detail of above Selected row";
		this.GrdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		GrdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistoryDetail.DesignTimeLayout = GrdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.GrdHistoryDetail.DynamicFiltering = true;
		this.GrdHistoryDetail.FilterMode = (FilterMode)2;
		this.GrdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Location = new System.Drawing.Point(3, 377);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Name = "GrdHistoryDetail";
		((ScrollableBase)this.GrdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Size = new System.Drawing.Size(1070, 252);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).TabIndex = 66;
		this.GrdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.GrdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.toolStrip2);
		this.panel7.Controls.Add(this.label26);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 3);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1070, 51);
		this.panel7.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.toolStripSeparator2, this.btnNewHistory, this.btnRefreshHistory, this.toolStripSeparator1 });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1070, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.label26.AutoSize = true;
		this.label26.BackColor = System.Drawing.Color.Teal;
		this.label26.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label26.Location = new System.Drawing.Point(3, 30);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(187, 19);
		this.label26.TabIndex = 0;
		this.label26.Text = "Labour Wages History";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1084, 661);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmWagesBillManual";
		this.Text = "Contractor Labour Wages";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmWagesBillManualFormClosing);
		base.Load += new System.EventHandler(frmWagesBillManualLoad);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmWagesBillManualKeyDown);
		this.panel1.ResumeLayout(false);
		this.tabIncomingLabourwages.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdwagesDetail).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWagesAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContractor).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouseTo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseFrom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comItem).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbStockParty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrder).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReferenceDocType).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDocumentTypeHistory).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
