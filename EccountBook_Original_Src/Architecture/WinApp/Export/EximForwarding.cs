using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Export;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Export;
using Architecture.Model.Inventory;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Contractor_Wages;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Inventory_Stocks_Report;
using Architecture.WinApp.Properties;
using Architecture.WinApp.Sale;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Export;

public class EximForwarding : Form
{
	private Attachment AT = new Attachment();

	private int RecId;

	private int inVoiceId;

	private int updateDetailIndex = -1;

	private int updateDetailIndexOther = -1;

	private int DefaultDaysToLessFromHistoryFromDate;

	private double PreStockWt;

	private bool UpdateMode;

	private bool WagesActiveOrInActive;

	private bool WagesCompulsoryOnForwarding;

	private bool StockLoaderVisibleInForwardingDetail;

	private bool SaleCostingJobOrderWise;

	private bool NetWeightAndStockWeightEqualOnForwarding;

	private DataTable dtDetail = new DataTable();

	private DataTable dtUomInner = new DataTable();

	private DataTable dtUomMaster = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private DataTable dtGetInvoiceDetail = new DataTable();

	private DataTable dtJobLotWithJobOrderAndItems = new DataTable();

	private bool ReferredInExportVoucher;

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formRights = new Rightsobjects();

	private LoadavailableTransactionsForGDN LoadTransactions;

	private UserAccount UserAccount = new UserAccount();

	private List<ExImForwardingPackingDetail> LstRemoveRecordDetail = new List<ExImForwardingPackingDetail>();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel3;

	private Panel panel2;

	private TabPage tabPage2;

	private ToolStrip toolStrip1;

	private ToolStripButton BtnNew;

	private ToolStripButton btnSave;

	private ToolStripButton btnUpdate;

	private ToolStripButton BtnPrint;

	internal UltraCombo cmbCustomer;

	internal UltraCombo cmbShippingAgent;

	private TextBox txtShippedContainer;

	private Label label25;

	private TextBox txtNoOfContainer;

	private Label label16;

	private Label label24;

	private DateTimePicker txtCompletedDate;

	private Label label26;

	private TextBox txtOtherCharges;

	internal UltraCombo cmbShippingLIne;

	private TextBox txtFreight;

	private Label label15;

	internal UltraCombo cmbDestinationPort;

	private Label label23;

	private TextBox txtCNICNO;

	internal UltraCombo cmbLoadingPort;

	private Label label22;

	private Label label21;

	private Label label12;

	private Label label19;

	private Label label14;

	private TextBox txtBilityNo;

	private Label label11;

	private Label label10;

	private TextBox txtDriverCellNo;

	internal UltraCombo cmbtransporter;

	private TextBox txtDriverName;

	private TextBox txtVehicleNo;

	private Label label8;

	private Label label7;

	private Label label4;

	private DateTimePicker txtGPDate;

	private DateTimePicker DocDate;

	private TextBox txtdocno;

	internal UltraCombo cmbInvoiceNo;

	private TextBox txtItemDesc;

	private Label label20;

	private Button btnAddinGrid;

	private TextBox txtNetWeight;

	private Label label30;

	private TextBox txtSeal;

	private Label label37;

	internal UltraCombo cmbPackType;

	private Label label29;

	internal UltraCombo cmbItem;

	private Label label28;

	internal GridEX grdDetail;

	private Panel panel6;

	private Panel panel10;

	private Panel panel8;

	private ToolStrip toolStrip2;

	private ToolStripSeparator toolStripSeparator1;

	private CheckBox chkdateisactive;

	internal UltraCombo cmbGpNo;

	private Button btnDetailCancel;

	private Button btnDetailUpdate;

	internal UltraCombo CmbJobLot;

	private Label label55;

	internal UltraCombo CmbWareHouse;

	private Label label54;

	private ToolStripButton btnRefresh;

	private TextBox txtContainer2;

	private Label label34;

	private TextBox txtconainer1;

	private Label label33;

	private TextBox txtFactoryWeight;

	private Label label35;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private TextBox txtDoNetWeight;

	private Label label2;

	private Label label5;

	private Label label18;

	private Label label39;

	private TextBox txtGrossWeight;

	internal UltraCombo CmbCropYear;

	private Label label40;

	private Panel panel4;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private Panel panel7;

	private Panel panel5;

	private TabPage tabPage4;

	private Panel panel11;

	internal GridEX grdotheritems;

	private Panel panel9;

	private Button btnoCancel;

	private Button btnoUpdate;

	private Button btnoadd;

	private Label label27;

	private TextBox txtoRemarks;

	private TextBox txtoQty;

	private Label label41;

	private Label label70;

	private TextBox txtoAmount;

	internal UltraCombo CmbOtherItem;

	private Label label71;

	private TextBox txtOContainer;

	private Label label42;

	private TextBox txtoRate;

	private Label label63;

	private Label label45;

	private TextBox txtStockWeight;

	private TextBox txtAddLess;

	private TextBox txtDoGrossWeight;

	private Label label1;

	private Label label47;

	private TextBox txtInvoiceNetWeight;

	private Label InvoiceNetWeight;

	private TextBox txtNoofInvoices;

	private Label label48;

	private TextBox txtBalFcWeight;

	private TextBox txtDispatchWeight;

	private Label label49;

	private Label label43;

	internal UltraCombo cmbcontainerNo;

	private Label label50;

	private CheckBox ChkPrintPreview;

	private ToolStripButton btnAttachment;

	private Panel panel12;

	private Label label36;

	private ToolStripButton btnShortCutKey;

	private Label label3;

	private TextBox txtEbTotalMaster;

	private Label label6;

	private TextBox txtEbUnitMaster;

	private TextBox txtEbUnitInner;

	private Label label9;

	private TextBox txtEbTotalInner;

	private Label label13;

	internal UltraCombo CmbPackUomInner;

	internal UltraCombo CmbPackUomMaster;

	private Label label17;

	private Label label38;

	private TextBox txtQtyInner;

	private TextBox txtQtyMaster;

	private Label label51;

	private Label label52;

	internal GridEX DataGridHistory;

	private Panel panel13;

	internal GridEX grdHistoryDetail;

	private GroupBox groupBox2;

	private RadioButton rdapproveddate;

	private Button btnShowHistory;

	private RadioButton rdmodifydate;

	private Label label31;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

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

	private Label label69;

	private Label label92;

	private Label label93;

	private ToolStripButton btnDelete;

	private Button btnRecordsUpdate;

	private ToolStripButton btnGenerateAvailableStock;

	private ToolStripButton btnStockReport;

	private Panel panel14;

	private Label label53;

	private Panel panel15;

	private Panel panel16;

	private Label label32;

	private CtrlGrdBar ctrlGrdBar3;

	internal GridEX grdgpInfo;

	private Label label46;

	internal UltraCombo CmbInvoiceNoHistory;

	private Label label44;

	internal UltraCombo CmbGpNoHistory;

	public EximForwarding(UserAccount user)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void ExpfrmForwarding_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1180, 820);
			formRights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formRights.DoHaveSaveRight;
			btnUpdate.Enabled = formRights.DoHaveUpdateRights;
			BtnPrint.Enabled = formRights.DoHavePrintRights;
			btnDelete.Enabled = formRights.DoHaveCanDelete;
			ConfigrationsAllocation obj = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "WagesCompulsoryOnForwarding");
			WagesCompulsoryOnForwarding = Conversion.ToBool((object)((obj != null) ? obj.ConfigKey : null));
			ConfigrationsAllocation obj2 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "DefaultDaysToLessFromHistoryFromDate");
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)((obj2 != null) ? obj2.ConfigKey : null));
			ConfigrationsAllocation obj3 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "StockLoaderVisibleInForwardingDetail");
			StockLoaderVisibleInForwardingDetail = Conversion.ToBool((object)((obj3 != null) ? obj3.ConfigKey : null));
			ConfigrationsAllocation obj4 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "NetWeightAndStockWeightEqualOnForwarding");
			NetWeightAndStockWeightEqualOnForwarding = Conversion.ToBool((object)((obj4 != null) ? obj4.ConfigKey : null));
			SaleCostingJobOrderWise = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("SaleCostingJobOrderWise"));
			if (SaleCostingJobOrderWise)
			{
				dtJobLotWithJobOrderAndItems = jobLot.JobLot_GetWithJobOrderAndItem(UserAccount.OrganizationId, UserAccount.CompanyId, 0);
			}
			WagesRefDocumentsStatus obj5 = clsGlobalVariables.WagesRefDocumentsStatusList.Find((WagesRefDocumentsStatus row) => row.RefDocumentTypeId == 205);
			WagesActiveOrInActive = obj5 != null && obj5.IsActive;
			dtUomMaster.Columns.Add("Id", typeof(int));
			dtUomMaster.Columns.Add("Code", typeof(string));
			dtUomMaster.Columns.Add("Equivalent", typeof(double));
			dtUomInner.Columns.Add("Id", typeof(int));
			dtUomInner.Columns.Add("Code", typeof(string));
			dtUomInner.Columns.Add("Equivalent", typeof(double));
			Project();
			braches();
			generateCode();
			gatepassGetAll();
			GridGatepassFill();
			cmbGpNo.Focus();
			packingType();
			JobLotFill();
			WarehouseFill();
			CropYearFill();
			chkdateisactive.Checked = false;
			txtCompletedDate.Enabled = false;
			bindSeaPort();
			ShippingAgentFill();
			ItemFillForExport();
			dtDetail.Columns.Add("Id");
			dtDetail.Columns.Add("RefDocumentTypeId");
			dtDetail.Columns.Add("RefDocIdNo");
			dtDetail.Columns.Add("RefDocSubIdNo");
			dtDetail.Columns.Add("InvoiceDetailId", typeof(int));
			dtDetail.Columns.Add("ContractId", typeof(int));
			dtDetail.Columns.Add("ContractDetailId", typeof(int));
			dtDetail.Columns.Add("Contract");
			dtDetail.Columns.Add("ItemId");
			dtDetail.Columns.Add("ItemName");
			dtDetail.Columns.Add("PackTypeId");
			dtDetail.Columns.Add("Packtype");
			dtDetail.Columns.Add("ItemDescription");
			dtDetail.Columns.Add("MasterQTY", typeof(double));
			dtDetail.Columns.Add("MasterUOMId", typeof(int));
			dtDetail.Columns.Add("MasterUOM");
			dtDetail.Columns.Add("MasterEquivalent", typeof(double));
			dtDetail.Columns.Add("EbUnitMaster", typeof(double));
			dtDetail.Columns.Add("EbTotalMaster", typeof(double));
			dtDetail.Columns.Add("InnerQTY", typeof(double));
			dtDetail.Columns.Add("InnerUOMId", typeof(int));
			dtDetail.Columns.Add("InnerUOM");
			dtDetail.Columns.Add("InnerEquivalent");
			dtDetail.Columns.Add("EbUnitInner", typeof(double));
			dtDetail.Columns.Add("EbTotalInner", typeof(double));
			dtDetail.Columns.Add("AddLess", typeof(double));
			dtDetail.Columns.Add("NetWeight", typeof(double));
			dtDetail.Columns.Add("GrossWeight", typeof(double));
			dtDetail.Columns.Add("StockWeight", typeof(double));
			dtDetail.Columns.Add("WarehouseId");
			dtDetail.Columns.Add("Warehouse");
			dtDetail.Columns.Add("JobLotId");
			dtDetail.Columns.Add("JobLot");
			dtDetail.Columns.Add("CropYearId");
			dtDetail.Columns.Add("CropYear");
			dtDetail.Columns.Add("Container#");
			dtDetail.Columns.Add("Seal#");
			dtDetail.Columns.Add("ContainerId");
			dtDetail.Columns.Add("RefDocType");
			dtDetail.Columns.Add("RefDocNo", typeof(int));
			dtDetail.Columns.Add("BalQty", typeof(double));
			dtDetail.Columns.Add("BalWeight", typeof(double));
			dtDetail.Columns.Add("DoDetailId", typeof(int));
			dtDetail.Columns.Add("FlagToUpdateContainer", typeof(bool));
			dtDetail.Columns.Add("AvailableStock", typeof(double));
			grdDetail.DataSource = dtDetail;
			grdDetail.RetrieveStructure();
			GridDetailSetting();
			dtOtherItem.Columns.Add("ItemId");
			dtOtherItem.Columns.Add("ItemName");
			dtOtherItem.Columns.Add("Qty", typeof(double));
			dtOtherItem.Columns.Add("Rate", typeof(double));
			dtOtherItem.Columns.Add("Amount", typeof(double));
			dtOtherItem.Columns.Add("ContainerNo");
			dtOtherItem.Columns.Add("Remarks");
			grdotheritems.DataSource = dtOtherItem;
			grdotheritems.RetrieveStructure();
			grdSettingsOtherItems();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			btnDetailUpdate.Visible = false;
			btnDetailCancel.Visible = false;
			HistoryCombosFill();
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void generateCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		try
		{
			int Code = ExImForwarding.GenerateCode(new ExImForwarding
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 205
			});
			if (Code > 0)
			{
				txtdocno.Text = Code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ShippingAgentFill()
	{
		try
		{
			int ShipLineId = Conversion.ToInt(cmbShippingLIne.Value);
			int ShipAgntId = Conversion.ToInt(cmbShippingAgent.Value);
			int TransId = Conversion.ToInt(cmbtransporter.Value);
			DataTable dt = CommonServices.GetSupplierustomerByCustomerGroupId("10");
			DataTable dtCompany = new DataTable();
			dtCompany.Columns.Add("Id", typeof(int));
			dtCompany.Columns.Add("CompanyName");
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtCompany.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["CompanyName"]);
				}
			}
			DDL.BindDDL(dt, cmbShippingLIne, "Id", "CompanyName", "Shipping Line", ZeroIndex: true);
			DDL.BindDDL(dt, cmbShippingAgent, "Id", "CompanyName", "Shipping Agent", ZeroIndex: true);
			DDL.BindDDL(dt, cmbtransporter, "Id", "CompanyName", "Transporter", ZeroIndex: true);
			if (ShipLineId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ShipLineId))
				{
					cmbShippingLIne.Value = ShipLineId;
				}
				else
				{
					((Control)(object)cmbShippingLIne).Text = string.Empty;
				}
			}
			if (ShipAgntId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ShipAgntId))
				{
					cmbShippingAgent.Value = ShipAgntId;
				}
				else
				{
					((Control)(object)cmbShippingAgent).Text = string.Empty;
				}
			}
			if (TransId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == TransId))
				{
					cmbtransporter.Value = TransId;
				}
				else
				{
					((Control)(object)cmbtransporter).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkdateisactive_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (chkdateisactive.Checked)
			{
				txtCompletedDate.Enabled = true;
			}
			else
			{
				txtCompletedDate.Enabled = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindInvoiceNo()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		try
		{
			inVoiceId = 0;
			int Id = Conversion.ToInt(cmbInvoiceNo.Value);
			DataTable dtInvoice = ExImInvoice.GetInvoiceNoForExportForwarding(new ExImInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(cmbGpNo.Value),
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("InvoiceNo");
			if (dtInvoice.Rows.Count > 0)
			{
				for (int i = 0; i < dtInvoice.Rows.Count; i++)
				{
					dt.Rows.Add(dtInvoice.Rows[i]["Id"], dtInvoice.Rows[i]["InvoiceNo"]);
				}
			}
			DDL.BindDDL(dt, cmbInvoiceNo, "Id", "InvoiceNo", "Invoice No", ZeroIndex: true);
			if (Id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					cmbInvoiceNo.Value = Id;
				}
				else
				{
					((Control)(object)cmbInvoiceNo).Text = string.Empty;
				}
			}
			if (dt.Rows.Count > 0)
			{
				int NoOfInvoices = dt.Rows.Count - 1;
				txtNoofInvoices.Text = NoOfInvoices.ToString();
			}
			if (dtInvoice.Rows.Count == 1)
			{
				((UltraGridBase)cmbInvoiceNo).Rows[1].Activate();
			}
			cmbInvoiceNo_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbInvoiceNo_Leave(object sender, EventArgs e)
	{
		try
		{
			GetDeliveryOrderbyGatepassId();
			GetContainersForForwarding();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetContainersForForwarding()
	{
		try
		{
			if (Conversion.ToInt(cmbInvoiceNo.Value) > 0 && Conversion.ToInt(cmbGpNo.Value) > 0)
			{
				int num = Conversion.ToInt(cmbGpNo.Value);
				int InvoiceId = Conversion.ToInt(cmbInvoiceNo.Value);
				DataTable dt = ExImForwarding.GetContainersForForwarding(num, InvoiceId);
				if (dt.Rows.Count > 0)
				{
					DataTable dtContainer = new DataTable();
					dtContainer.Columns.Add("Id", typeof(string));
					dtContainer.Columns.Add("Container", typeof(string));
					dtContainer.Columns.Add("SealNo", typeof(string));
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						dtContainer.Rows.Add(dt.Rows[i]["ContainerId"], dt.Rows[i]["Container"], dt.Rows[i]["SealNo"]);
					}
					DDL.BindDDL(dtContainer, cmbcontainerNo, "Id", "Container", "Container No", ZeroIndex: false);
				}
				else
				{
					((Control)(object)cmbcontainerNo).Text = string.Empty;
					((UltraGridBase)cmbcontainerNo).DataSource = null;
				}
			}
			else
			{
				((Control)(object)cmbcontainerNo).Text = string.Empty;
				((UltraGridBase)cmbcontainerNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GetGatePassDataForForwarding()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(cmbGpNo.Value)
			};
			new DataTable
			{
				Columns = { "Id", "Container" }
			};
			DataTable dtgatepass = GatePassOutward.GatePassDetailForForwardingByGpId(val);
			if (dtgatepass.Rows.Count > 0)
			{
				txtVehicleNo.Text = Conversion.ToString(dtgatepass.Rows[0]["VehicleNo"]);
				txtBilityNo.Text = Conversion.ToString(dtgatepass.Rows[0]["BiltyNo"]);
				txtGPDate.Value = Conversion.ToDateTime(dtgatepass.Rows[0]["GpDate"]);
				txtFreight.Text = Conversion.ToString(dtgatepass.Rows[0]["Freight"]);
				double FcWt = Conversion.ToDouble(dtgatepass.Rows[0]["WbGrossWeight"]) - Conversion.ToDouble(dtgatepass.Rows[0]["StockWeight"]) + PreStockWt;
				txtFactoryWeight.Text = Conversion.ToString(dtgatepass.Rows[0]["WbGrossWeight"]);
				txtDispatchWeight.Text = (Conversion.ToDouble(dtgatepass.Rows[0]["StockWeight"]) - PreStockWt).ToString("#,##0.###");
				txtBalFcWeight.Text = FcWt.ToString();
				txtDoGrossWeight.Text = Conversion.ToString(dtgatepass.Rows[0]["DoGrossWeight"]);
				txtDoNetWeight.Text = Conversion.ToString(dtgatepass.Rows[0]["DoWeight"]);
				txtconainer1.Text = Conversion.ToString(dtgatepass.Rows[0]["Container"]);
				txtContainer2.Text = Conversion.ToString(dtgatepass.Rows[0]["Container1"]);
				if (txtconainer1.Text != string.Empty && txtconainer1.Text != "0" && (txtContainer2.Text == string.Empty || txtContainer2.Text == "0"))
				{
					txtNoOfContainer.Text = "1";
				}
				else if ((txtconainer1.Text == string.Empty || txtconainer1.Text == "0") && txtContainer2.Text != string.Empty && txtContainer2.Text != "0")
				{
					txtNoOfContainer.Text = "1";
				}
				else if (txtconainer1.Text != string.Empty && txtconainer1.Text != "0" && txtContainer2.Text != string.Empty && txtContainer2.Text != "0")
				{
					txtNoOfContainer.Text = "2";
				}
			}
			else
			{
				txtDoGrossWeight.Text = string.Empty;
				txtDoNetWeight.Text = string.Empty;
				txtVehicleNo.Text = string.Empty;
				txtBilityNo.Text = string.Empty;
				txtconainer1.Text = string.Empty;
				txtContainer2.Text = string.Empty;
				txtFactoryWeight.Text = "0";
				txtFreight.Text = "0";
				txtNoOfContainer.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetInformationFromInvoice()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.Ids = "204,209";
			DataTable dtcustomer = new DataTable();
			dtcustomer.Columns.Add("SupplierCustomerId", typeof(int));
			dtcustomer.Columns.Add("Customer");
			if (Conversion.ToInt(cmbInvoiceNo.Value) > 0)
			{
				DataTable dteform = ExImInvoice.GetData(obj);
				if (dteform.Rows.Count <= 0)
				{
					return;
				}
				int count = 0;
				for (int i = 0; i < dteform.Rows.Count; i++)
				{
					if (Conversion.ToInt(dteform.Rows[i]["Id"]) == Conversion.ToInt(cmbInvoiceNo.Value))
					{
						count = i;
						dtcustomer.Rows.Add(dteform.Rows[i]["SupplierCustomerId"], dteform.Rows[i]["Customer"]);
						break;
					}
				}
				if (dteform.Rows.Count > 0)
				{
					DDL.BindDDL(dtcustomer, cmbCustomer, "SupplierCustomerId", "Customer", "Customer", ZeroIndex: true);
					if (dtcustomer.Rows.Count > 1)
					{
						((UltraGridBase)cmbCustomer).Rows[1].Activate();
					}
				}
				DataRow dr = dteform.Rows[count];
				cmbLoadingPort.Value = Conversion.ToInt(dr["LoadingPortId"]);
				cmbDestinationPort.Value = Conversion.ToInt(dr["DestinationPortId"]);
				cmbShippingLIne.Value = Conversion.ToInt(dr["ShippingLineId"]);
				cmbShippingAgent.Value = Conversion.ToInt(dr["ShippingAgentId"]);
				cmbtransporter.Value = Conversion.ToInt(dr["TransporterId"]);
			}
			else
			{
				DDL.BindDDL(dtcustomer, cmbCustomer, "SupplierCustomerId", "Customer", "Customer", ZeroIndex: true);
				cmbShippingLIne.Value = 0;
				cmbShippingAgent.Value = 0;
				cmbtransporter.Value = 0;
				cmbLoadingPort.Value = 0;
				cmbDestinationPort.Value = 0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gatepassGetAll()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(cmbGpNo.Value);
			DataTable dt = GatePassOutward.GetPendingGatePassForExportForwarding(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			DataTable dtGp = new DataTable();
			dtGp.Columns.Add("Id", typeof(int));
			dtGp.Columns.Add("GpSrNo");
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtGp.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["GpSrNo"]);
				}
			}
			DDL.BindDDL(dtGp, cmbGpNo, "Id", "GpSrNo", "GatePass No", ZeroIndex: true);
			if (dtGp.Rows.Count > 0)
			{
				if (Id > 0)
				{
					if (dtGp.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						cmbGpNo.Value = Id;
					}
					else
					{
						((Control)(object)cmbGpNo).Text = string.Empty;
					}
				}
			}
			else
			{
				txtGPDate.Value = DateTime.Now;
				txtBilityNo.Text = string.Empty;
				txtVehicleNo.Text = string.Empty;
				txtFreight.Text = string.Empty;
				txtconainer1.Text = string.Empty;
				txtContainer2.Text = string.Empty;
				txtFactoryWeight.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemFillForExport()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtItemCombo = Item.GetExportItemsByOrganizationCompanyId(new Item
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtItemCombo.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtItemCombo, cmbItem, "Id", "ItemName", "Item", false);
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 450;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Project()
	{
	}

	private void braches()
	{
	}

	private void bindSeaPort()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		try
		{
			int LoadId = Conversion.ToInt(cmbLoadingPort.Value);
			int DestId = Conversion.ToInt(cmbDestinationPort.Value);
			DataTable dtSeaport = SeaPorts.Getall(new SeaPorts
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId
			});
			DataTable dtLoad = new DataTable();
			dtLoad.Columns.Add("Id", typeof(int));
			dtLoad.Columns.Add("PortName");
			DataTable dtDest = new DataTable();
			dtDest.Columns.Add("Id", typeof(int));
			dtDest.Columns.Add("PortName");
			if (dtSeaport.Rows.Count > 0)
			{
				for (int i = 0; i < dtSeaport.Rows.Count; i++)
				{
					if (Conversion.ToString(dtSeaport.Rows[i]["PortType"]) == "Loading")
					{
						dtLoad.Rows.Add(dtSeaport.Rows[i]["Id"], dtSeaport.Rows[i]["PortName"]);
					}
					else if (Conversion.ToString(dtSeaport.Rows[i]["PortType"]) == "Destination")
					{
						dtDest.Rows.Add(dtSeaport.Rows[i]["Id"], dtSeaport.Rows[i]["PortName"]);
					}
				}
			}
			DDL.BindDDL(dtLoad, cmbLoadingPort, "Id", "PortName", "Port Name", ZeroIndex: true);
			DDL.BindDDL(dtDest, cmbDestinationPort, "Id", "PortName", "Port Name", ZeroIndex: true);
			if (LoadId > 0)
			{
				if (dtLoad.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == LoadId))
				{
					cmbLoadingPort.Value = LoadId;
				}
				else
				{
					((Control)(object)cmbLoadingPort).Text = string.Empty;
				}
			}
			if (DestId > 0)
			{
				if (dtDest.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == DestId))
				{
					cmbDestinationPort.Value = DestId;
				}
				else
				{
					((Control)(object)cmbDestinationPort).Text = string.Empty;
				}
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
			DataTable dtjob = new DataTable();
			dtjob.Columns.Add("Id", typeof(int));
			dtjob.Columns.Add("JobLotDescription", typeof(string));
			int Id = Conversion.ToInt(CmbJobLot.Value);
			if (SaleCostingJobOrderWise)
			{
				HashSet<int> Ids = new HashSet<int>();
				foreach (DataRow dr in dtJobLotWithJobOrderAndItems.Rows)
				{
					int ID = Conversion.ToInt(dr["Id"]);
					if (!Ids.Contains(ID))
					{
						dtjob.Rows.Add(ID, dr["JobLotDescription"]);
						Ids.Add(ID);
					}
				}
			}
			else
			{
				dtjob = CommonServices.JobLotGetAllService();
			}
			if (dtjob.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtjob, CmbJobLot, "Id", "JobLotDescription", "JobLot Description", false);
				CommonServices.SetComboValue(CmbJobLot, "Id", Id, dtjob);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CropYearFill()
	{
		try
		{
			DataTable dtCrop = CommonServices.CropYearGetAllService();
			if (dtCrop.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCrop, CmbCropYear, "Id", "CropYear", "CropYear", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WarehouseFill()
	{
		try
		{
			DataTable dtwh = CommonServices.getActiveWareHouse();
			if (dtwh.Rows.Count > 0)
			{
				DDL.BindDDL(dtwh, CmbWareHouse, "Id", "WareHouseName", "Godown Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void packingType()
	{
		try
		{
			DataTable dt = InvPackingType.Getall();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbPackType, "Id", "PackTypeDesc", "Pack Type ", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindOtherItems()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable lst = ExImInvoice.GetOtherItemByExImInvoiceId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(cmbInvoiceNo.Value)
			});
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDL(lst, CmbOtherItem, "ItemId", "ItemName", "Item Name", ZeroIndex: false);
				((UltraGridBase)CmbOtherItem).DisplayLayout.Bands[0].Columns[1].Width = 400;
				((UltraGridBase)CmbOtherItem).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItem_Leave(object sender, EventArgs e)
	{
		try
		{
			BindPackUomAndRateUom();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindPackUomAndRateUom()
	{
		try
		{
			string Packuom = ((Control)(object)CmbPackUomMaster).Text;
			((Control)(object)CmbPackUomMaster).Text = string.Empty;
			string PackUomInner = ((Control)(object)CmbPackUomInner).Text;
			((Control)(object)CmbPackUomInner).Text = string.Empty;
			DataTable dtUom = CommonServices.dtUomFromGloablUomScheduleByItemId(Conversion.ToInt(cmbItem.Value));
			if (dtUom.Rows.Count > 0)
			{
				DDL.BindDDL(dtUom, CmbPackUomMaster, "Id", "UOMCode", "Pack Uom", ZeroIndex: false);
				((UltraGridBase)CmbPackUomMaster).DisplayLayout.Bands[0].Columns[1].Width = 70;
				((UltraGridBase)CmbPackUomMaster).DisplayLayout.Bands[0].Columns[2].Width = 85;
				DDL.BindDDL(dtUom, CmbPackUomInner, "Id", "UOMCode", "Pack Uom", ZeroIndex: false);
				((UltraGridBase)CmbPackUomInner).DisplayLayout.Bands[0].Columns[1].Width = 70;
				((UltraGridBase)CmbPackUomInner).DisplayLayout.Bands[0].Columns[1].Width = 85;
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Packuom))
				{
					((Control)(object)CmbPackUomMaster).Text = Packuom;
				}
				else
				{
					((UltraGridBase)CmbPackUomMaster).Rows[0].Activate();
				}
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == PackUomInner))
				{
					((Control)(object)CmbPackUomInner).Text = PackUomInner;
				}
				else
				{
					((UltraGridBase)CmbPackUomInner).Rows[0].Activate();
				}
			}
			else
			{
				((Control)(object)CmbPackUomMaster).Text = string.Empty;
				((UltraGridBase)CmbPackUomMaster).DataSource = null;
				((Control)(object)CmbPackUomInner).Text = string.Empty;
				((UltraGridBase)CmbPackUomInner).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void cmbGpNo_TextChanged(object sender, EventArgs e)
	{
		try
		{
			GetGatePassDataForForwarding();
			GetContainersForForwarding();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void GetDeliveryOrderbyGatepassId()
	{
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		try
		{
			txtDoGrossWeight.Text = string.Empty;
			txtInvoiceNetWeight.Text = string.Empty;
			txtShippedContainer.Text = string.Empty;
			if (grdDetail.GetRows().Count() <= 0)
			{
				inVoiceId = Conversion.ToInt(cmbInvoiceNo.Value);
			}
			if (inVoiceId > 0 && Conversion.ToInt(cmbInvoiceNo.Value) != inVoiceId && grdDetail.GetRows().Count() > 0 && MessageBox.Show("Are you sure Change Invoice?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				cmbInvoiceNo.Value = inVoiceId;
				return;
			}
			GetInformationFromInvoice();
			grdDetail.ClearStructure();
			double TotalNetWeight = 0.0;
			double TotalGrossWeight = 0.0;
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("Name");
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.Id = Conversion.ToInt(cmbGpNo.Value);
			obj.ExImInvoiceId = Conversion.ToInt(cmbInvoiceNo.Value);
			DataTable dtDeliveryOrder = InvDeliveryOrder.DeliveryOrderDetailForExportGdnForwarding(obj);
			if (dtDeliveryOrder.Rows.Count <= 0)
			{
				return;
			}
			dtDetail.Rows.Clear();
			dtUomMaster.Rows.Clear();
			dtUomInner.Rows.Clear();
			int i;
			for (i = 0; i < dtDeliveryOrder.Rows.Count; i++)
			{
				string ContainerNumber = "";
				ContainerNumber = ((i == 0) ? txtconainer1.Text : ((!(txtContainer2.Text != string.Empty) || !(txtContainer2.Text != "0")) ? txtconainer1.Text : txtContainer2.Text));
				TotalNetWeight += Conversion.ToDouble(dtDeliveryOrder.Rows[i]["NetWeight"]);
				TotalGrossWeight += Conversion.ToDouble(dtDeliveryOrder.Rows[i]["GrossWeight"]);
				if (!dtItem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(dtDeliveryOrder.Rows[i]["ItemId"])))
				{
					dtItem.Rows.Add(dtDeliveryOrder.Rows[i]["ItemId"], dtDeliveryOrder.Rows[i]["ItemName"]);
				}
				if (RecId == 0 && !btnUpdate.Visible)
				{
					dtDetail.Rows.Add(0, dtDeliveryOrder.Rows[i]["RefDocumentTypeId"], dtDeliveryOrder.Rows[i]["RefDocIdNo"], dtDeliveryOrder.Rows[i]["RefDocSubIdNo"], dtDeliveryOrder.Rows[i]["InvoiceDetailId"], dtDeliveryOrder.Rows[i]["ContractId"], dtDeliveryOrder.Rows[i]["ContractDetailId"], dtDeliveryOrder.Rows[i]["ProformaNo"], dtDeliveryOrder.Rows[i]["ItemId"], dtDeliveryOrder.Rows[i]["ItemName"], dtDeliveryOrder.Rows[i]["PackingMaterialTypeId"], dtDeliveryOrder.Rows[i]["PackMaterilaType"], dtDeliveryOrder.Rows[i]["ItemDescriptionManual"], dtDeliveryOrder.Rows[i]["OuterQty"], dtDeliveryOrder.Rows[i]["OuterQtyUomId"], dtDeliveryOrder.Rows[i]["OuterUOM"], dtDeliveryOrder.Rows[i]["OuterEquivalent"], Conversion.ToDouble(dtDeliveryOrder.Rows[i]["EbUnit"]), Conversion.ToDouble(dtDeliveryOrder.Rows[i]["TotalPackingWeight"]), dtDeliveryOrder.Rows[i]["InnerQty"], dtDeliveryOrder.Rows[i]["InnerUomId"], dtDeliveryOrder.Rows[i]["UomCodeInner"], dtDeliveryOrder.Rows[i]["InnerEquivalent"], Conversion.ToDouble(dtDeliveryOrder.Rows[i]["InnerEbUnit"]), Conversion.ToDouble(dtDeliveryOrder.Rows[i]["InnerEbTotal"]), 0, Conversion.ToDouble(dtDeliveryOrder.Rows[i]["NetWeight"]), Conversion.ToDouble(dtDeliveryOrder.Rows[i]["GrossWeight"]), Conversion.ToDouble((object)txtFactoryWeight.Text) / Conversion.ToDouble((object)txtDoNetWeight.Text) * Conversion.ToDouble(dtDeliveryOrder.Rows[i]["NetWeight"]) - Conversion.ToDouble(dtDeliveryOrder.Rows[i]["TotalPackingWeight"]), dtDeliveryOrder.Rows[i]["WarehouseId"], dtDeliveryOrder.Rows[i]["WareHouseName"], dtDeliveryOrder.Rows[i]["JobLotId"], dtDeliveryOrder.Rows[i]["JobLotName"], dtDeliveryOrder.Rows[i]["CropYearId"], dtDeliveryOrder.Rows[i]["CropYear"], (Conversion.ToInt(dtDeliveryOrder.Rows[i]["ContainerId"]) > 0) ? dtDeliveryOrder.Rows[i]["Container"] : ContainerNumber, dtDeliveryOrder.Rows[i]["SealNo"], dtDeliveryOrder.Rows[i]["ContainerId"], "", 0, 0, 0, dtDeliveryOrder.Rows[i]["DoDetailId"], Conversion.ToInt(dtDeliveryOrder.Rows[i]["ContainerId"]) > 0);
				}
			}
			txtShippedContainer.Text = dtDeliveryOrder.Rows[0]["ShippedContainer"].ToString();
			txtInvoiceNetWeight.Text = TotalNetWeight.ToString();
			txtDoGrossWeight.Text = TotalGrossWeight.ToString();
			grdDetail.DataSource = dtDetail;
			grdDetail.RetrieveStructure();
			GridDetailSetting();
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtItem, cmbItem, "Id", "Name", "Item", ZeroIndex: false);
				((UltraGridBase)cmbItem).Rows[0].Activate();
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 450;
			}
			ExImInvoice ex = ExImInvoice.GetByID(Conversion.ToInt(cmbInvoiceNo.Value));
			DataTable dtOItem = new DataTable();
			dtOItem.Columns.Add("Id", typeof(int));
			dtOItem.Columns.Add("ItemName", typeof(string));
			dtOtherItem.Rows.Clear();
			int x;
			for (x = 0; x < ex.ExImInvoiceOtherItemslist.Count; x++)
			{
				if (!dtOItem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ex.ExImInvoiceOtherItemslist[x].otherItemId))
				{
					dtOItem.Rows.Add(ex.ExImInvoiceOtherItemslist[x].otherItemId, ex.ExImInvoiceOtherItemslist[x].ItemName);
				}
				dtOtherItem.Rows.Add(ex.ExImInvoiceOtherItemslist[x].otherItemId, ex.ExImInvoiceOtherItemslist[x].ItemName, ex.ExImInvoiceOtherItemslist[x].oItemQty, ex.ExImInvoiceOtherItemslist[x].oItemRate, ex.ExImInvoiceOtherItemslist[x].oItemAmount, txtconainer1.Text, ex.ExImInvoiceOtherItemslist[x].OtherItemRemarks);
			}
			DDL.BindDDL(dtOItem, CmbOtherItem, "Id", "ItemName", "Item Name", ZeroIndex: false);
			((UltraGridBase)CmbOtherItem).DisplayLayout.Bands[0].Columns[1].Width = 400;
			grdotheritems.DataSource = dtOtherItem;
			grdotheritems.RetrieveStructure();
			grdSettingsOtherItems();
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
		}
	}

	public void cmbGpNo_Leave(object sender, EventArgs e)
	{
		try
		{
			GetGatePassDataForForwarding();
			GetContainersForForwarding();
			bindInvoiceNo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool AutoUpdateRecord(int UpdateRecID)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Expected O, but got Unknown
		//IL_0d27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2e: Expected O, but got Unknown
		try
		{
			if (grdDetail.RowCount == 0)
			{
				throw new Exception("Please Check Detail Grid");
			}
			if (!formValidation())
			{
				return false;
			}
			ExImForwarding forwarding = new ExImForwarding();
			if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return false;
			}
			forwarding.Id = UpdateRecID;
			forwarding.AutoUpdateId = 1;
			double StockWeight = 0.0;
			double DoInvoiceWt = Conversion.ToDouble((object)txtInvoiceNetWeight.Text.Trim());
			double FactoryWeight = Conversion.ToDouble((object)txtBalFcWeight.Text.Trim());
			double num = Conversion.ToDouble((object)grdDetail.GetTotal(grdDetail.RootTable.Columns["NetWeight"], (AggregateFunction)2).ToString());
			StockWeight = Conversion.ToDouble((object)grdDetail.GetTotal(grdDetail.RootTable.Columns["StockWeight"], (AggregateFunction)2).ToString());
			if (num != DoInvoiceWt)
			{
				throw new Exception("SumOfGrid NetWeight and DoNetWeight not equal please check");
			}
			if (btnSave.Visible && btnSave.Enabled)
			{
				if (Conversion.ToInt((object)txtNoofInvoices.Text) == 1)
				{
					if (StockWeight == FactoryWeight)
					{
					}
				}
				else if (StockWeight > FactoryWeight)
				{
					throw new Exception("StockWeight cannot be greater than FactoryWeight please check");
				}
			}
			forwarding.OrganizationId = UserAccount.OrganizationId;
			forwarding.CompanyId = UserAccount.CompanyId;
			forwarding.DocNo = Conversion.ToInt((object)txtdocno.Text.Trim());
			forwarding.DocDate = DocDate.Value;
			forwarding.DocumentTypeId = 205;
			forwarding.ExImInvoiceId = Conversion.ToInt(cmbInvoiceNo.Value);
			forwarding.GatePassOutwardId = Conversion.ToInt(cmbGpNo.Value);
			forwarding.GpNo = Conversion.ToString((object)((Control)(object)cmbGpNo).Text.Trim());
			forwarding.GPDate = txtGPDate.Value;
			forwarding.TransporterId = Conversion.ToInt(cmbtransporter.Value);
			forwarding.VehicleNo = txtVehicleNo.Text;
			forwarding.BiltyNo = txtBilityNo.Text;
			forwarding.DriverName = txtDriverName.Text;
			forwarding.DriverCellNo = txtDriverCellNo.Text;
			forwarding.DriverCnicNo = txtCNICNO.Text;
			forwarding.Container = txtconainer1.Text;
			forwarding.Container1 = txtContainer2.Text;
			forwarding.FreightAmt = Conversion.ToDouble((object)txtFreight.Text);
			forwarding.FactoryWeight = Conversion.ToDouble((object)txtFactoryWeight.Text);
			forwarding.OtherCharges = Conversion.ToDouble((object)txtFactoryWeight.Text);
			forwarding.ShippingLineId = Conversion.ToInt(cmbShippingLIne.Value);
			forwarding.ShippingAgentId = Conversion.ToInt(cmbShippingAgent.Value);
			forwarding.SupplierCustomerId = Conversion.ToInt(cmbCustomer.Value);
			forwarding.LoadingPortId = Conversion.ToInt(cmbLoadingPort.Value);
			forwarding.DestinationPortId = Conversion.ToInt(cmbDestinationPort.Value);
			forwarding.ShippedContainer = Conversion.ToInt((object)txtShippedContainer.Text);
			if (chkdateisactive.Checked)
			{
				forwarding.LotCompletedDate = txtCompletedDate.Value;
			}
			forwarding.LotCompletedDate = txtCompletedDate.Value;
			forwarding.NoOfContainer = Conversion.ToInt((object)txtNoOfContainer.Text);
			forwarding.EntryUser = UserAccount.ID;
			forwarding.ModifyUser = UserAccount.ID;
			forwarding.EntryDate = DateTime.Now;
			forwarding.ModifyDate = DateTime.Now;
			forwarding.IsApproved = false;
			forwarding.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			forwarding.ExImForwardingDetail = new List<ExImForwardingPackingDetail>();
			forwarding.ExImForwardingOtherItemslist = new List<ExImForwardingOtherItems>();
			forwarding.AttachmentsList = new List<DMSAttachments>();
			if (LstRemoveRecordDetail.Count > 0)
			{
				for (int i = 0; i < LstRemoveRecordDetail.Count; i++)
				{
					forwarding.ExImForwardingDetail.Add(LstRemoveRecordDetail[i]);
				}
			}
			double GrossWeight = 0.0;
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				ExImForwardingPackingDetail exImForwardingPackingDetail = new ExImForwardingPackingDetail();
				exImForwardingPackingDetail.Id = Conversion.ToInt(r2.Cells["Id"].Value);
				exImForwardingPackingDetail.RefDocumentTypeId = Conversion.ToInt(r2.Cells["RefDocumentTypeId"].Value);
				exImForwardingPackingDetail.RefDocIdNo = Conversion.ToInt(r2.Cells["RefDocIdNo"].Value);
				exImForwardingPackingDetail.RefDocSubIdNo = Conversion.ToInt(r2.Cells["RefDocSubIdNo"].Value);
				exImForwardingPackingDetail.ExImLcOrderId = Conversion.ToInt(r2.Cells["ContractId"].Value);
				if (exImForwardingPackingDetail.ExImLcOrderId == 0)
				{
					throw new Exception("ContractId Not found in row#" + (r2.RowIndex + 1));
				}
				exImForwardingPackingDetail.ContractDetailId = Conversion.ToInt(r2.Cells["ContractDetailId"].Value);
				if (exImForwardingPackingDetail.ContractDetailId == 0)
				{
					throw new Exception("Contract DetailId Not found in row#" + (r2.RowIndex + 1));
				}
				exImForwardingPackingDetail.InvoiceId = Conversion.ToInt(cmbInvoiceNo.Value);
				exImForwardingPackingDetail.InvoiceDetailId = Conversion.ToInt(r2.Cells["InvoiceDetailId"].Value);
				if (exImForwardingPackingDetail.InvoiceDetailId == 0)
				{
					throw new Exception("Invoice Detail Id Not found in row#" + (r2.RowIndex + 1));
				}
				if (Conversion.ToInt(r2.Cells["ItemId"].Value) != 0)
				{
					exImForwardingPackingDetail.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
					exImForwardingPackingDetail.ItemDescription = Conversion.ToString((object)r2.Cells["ItemDescription"].Text.Trim());
					if (Conversion.ToInt(r2.Cells["PackTypeId"].Value) != 0)
					{
						exImForwardingPackingDetail.PackingMaterialId = Conversion.ToInt(r2.Cells["PackTypeId"].Value);
						if (Conversion.ToInt(r2.Cells["MasterUOMId"].Value) != 0)
						{
							exImForwardingPackingDetail.UOMScheduleIdOuter = Conversion.ToInt(r2.Cells["MasterUOMId"].Value);
							exImForwardingPackingDetail.EbUnit = Conversion.ToDouble(r2.Cells["EbUnitMaster"].Value);
							exImForwardingPackingDetail.EbTotal = Conversion.ToDouble(r2.Cells["EbTotalMaster"].Value);
							if (Conversion.ToDouble(r2.Cells["MasterQTY"].Value) != 0.0)
							{
								exImForwardingPackingDetail.OuterQty = Conversion.ToDouble(r2.Cells["MasterQTY"].Value);
								exImForwardingPackingDetail.InnerQty = Conversion.ToDouble(r2.Cells["InnerQTY"].Value);
								exImForwardingPackingDetail.UOMScheduleIdInner = Conversion.ToInt(r2.Cells["InnerUOMId"].Value);
								exImForwardingPackingDetail.InnerEbUnit = Conversion.ToDouble(r2.Cells["EbUnitInner"].Value);
								exImForwardingPackingDetail.InnerEbTotal = Conversion.ToDouble(r2.Cells["EbTotalInner"].Value);
								exImForwardingPackingDetail.TotalPackingWeight = Conversion.ToDouble((object)(exImForwardingPackingDetail.InnerEbTotal + exImForwardingPackingDetail.EbTotal));
								exImForwardingPackingDetail.AdLsWeight = Conversion.ToDouble((object)r2.Cells["AddLess"].Text.Trim());
								if (Conversion.ToDouble(r2.Cells["NetWeight"].Value) != 0.0)
								{
									exImForwardingPackingDetail.NetWeight = Conversion.ToDouble((object)r2.Cells["NetWeight"].Text.Trim());
									if (Conversion.ToDouble(r2.Cells["GrossWeight"].Value) != 0.0)
									{
										exImForwardingPackingDetail.GrossWeight = Conversion.ToDouble((object)r2.Cells["GrossWeight"].Text.Trim());
										GrossWeight += exImForwardingPackingDetail.GrossWeight;
										if (!NetWeightAndStockWeightEqualOnForwarding)
										{
											exImForwardingPackingDetail.StockWeight = Conversion.ToDouble((object)r2.Cells["StockWeight"].Text.Trim());
										}
										else
										{
											exImForwardingPackingDetail.StockWeight = exImForwardingPackingDetail.NetWeight;
										}
										exImForwardingPackingDetail.CropYearId = Conversion.ToInt(r2.Cells["CropYearId"].Value);
										exImForwardingPackingDetail.CropYear = Conversion.ToString(r2.Cells["CropYear"].Value);
										if (exImForwardingPackingDetail.CropYearId == 0)
										{
											MessageBox.Show("CropYear Required in row#" + (r2.RowIndex + 1));
											return false;
										}
										if (Conversion.ToInt(r2.Cells["WarehouseId"].Value) != 0)
										{
											exImForwardingPackingDetail.WarehouseId = Conversion.ToInt(r2.Cells["WarehouseId"].Value);
											if (Conversion.ToInt(r2.Cells["JobLotId"].Value) != 0)
											{
												exImForwardingPackingDetail.LotJobId = Conversion.ToInt(r2.Cells["JobLotId"].Value);
												exImForwardingPackingDetail.CropYearId = Conversion.ToInt(r2.Cells["CropYearId"].Value);
												if (txtconainer1.Text != string.Empty && txtContainer2.Text != string.Empty)
												{
													if (!(Conversion.ToString(r2.Cells["Container#"].Value) != string.Empty))
													{
														MessageBox.Show("Container Required in row#" + (r2.RowIndex + 1));
														return false;
													}
													exImForwardingPackingDetail.ContainerNo = Conversion.ToString((object)r2.Cells["Container#"].Text.Trim());
												}
												else
												{
													exImForwardingPackingDetail.ContainerNo = Conversion.ToString((object)r2.Cells["Container#"].Text.Trim());
												}
												if (Conversion.ToString(r2.Cells["Seal#"].Value) != string.Empty)
												{
													exImForwardingPackingDetail.SealNo = Conversion.ToString((object)r2.Cells["Seal#"].Text.Trim());
													exImForwardingPackingDetail.ActionTypeId = ((exImForwardingPackingDetail.Id <= 0) ? 1 : 2);
													forwarding.ExImForwardingDetail.Add(exImForwardingPackingDetail);
													continue;
												}
												MessageBox.Show("Seal No Required in row#" + (r2.RowIndex + 1));
												return false;
											}
											MessageBox.Show("Job Lot Required in row#" + (r2.RowIndex + 1));
											return false;
										}
										MessageBox.Show("Warehouse Required in row#" + (r2.RowIndex + 1));
										return false;
									}
									MessageBox.Show("Gross Weight Required in row#" + (r2.RowIndex + 1));
									return false;
								}
								MessageBox.Show("NetWeight Required in row#" + (r2.RowIndex + 1));
								return false;
							}
							MessageBox.Show("Master QTY Required in row#" + (r2.RowIndex + 1));
							return false;
						}
						MessageBox.Show("Master UOM Required In Grid");
						return false;
					}
					MessageBox.Show("Packing Type Required in row#" + (r2.RowIndex + 1));
					return false;
				}
				MessageBox.Show("Item Required in row#" + (r2.RowIndex + 1));
				return false;
			}
			if (grdotheritems.RowCount > 0)
			{
				rows = grdotheritems.GetRows();
				foreach (GridEXRow r in rows)
				{
					ExImForwardingOtherItems vd = new ExImForwardingOtherItems();
					vd.otherItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					vd.oItemQty = Conversion.ToDecimal(r.Cells["Qty"].Value);
					vd.oItemRate = Conversion.ToDouble(r.Cells["Rate"].Value);
					vd.oItemAmount = Conversion.ToDecimal(r.Cells["Amount"].Value);
					vd.OtherItemRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
					vd.ContainerNo = Conversion.ToString(r.Cells["ContainerNo"].Value);
					forwarding.ExImForwardingOtherItemslist.Add(vd);
				}
			}
			ExImForwarding.Save(forwarding);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return false;
		}
		return true;
	}

	private void ValidateDetailRecordsWithorWithoutLoader()
	{
		try
		{
			bool AnyRowFromLoader = false;
			int indexOfRowFromLoader = -1;
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				int refDocumentTypeId = Conversion.ToInt(r2.Cells["RefDocumentTypeId"].Value);
				int refDocIdNo2 = Conversion.ToInt(r2.Cells["RefDocIdNo"].Value);
				int refDocSubIdNo2 = Conversion.ToInt(r2.Cells["RefDocSubIdNo"].Value);
				if (refDocumentTypeId > 0 || refDocIdNo2 > 0 || refDocSubIdNo2 > 0)
				{
					indexOfRowFromLoader = r2.RowIndex;
					AnyRowFromLoader = true;
					break;
				}
			}
			if (!AnyRowFromLoader)
			{
				return;
			}
			rows = grdDetail.GetRows();
			foreach (GridEXRow r in rows)
			{
				int num = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
				int refDocIdNo = Conversion.ToInt(r.Cells["RefDocIdNo"].Value);
				int refDocSubIdNo = Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value);
				if (num == 0 || refDocIdNo == 0 || refDocSubIdNo == 0)
				{
					throw new Exception("All rows in the detail grid must either be from the loader or none should be from the loader.\n" + $"Row No: {r.RowIndex + 1} does not have loader values, while Row No: {indexOfRowFromLoader + 1} has loader values.");
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_04df: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Expected O, but got Unknown
		//IL_0e28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2f: Expected O, but got Unknown
		//IL_0fef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1005: Unknown result type (might be due to invalid IL or missing references)
		//IL_1016: Unknown result type (might be due to invalid IL or missing references)
		//IL_102d: Expected O, but got Unknown
		//IL_104e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1053: Unknown result type (might be due to invalid IL or missing references)
		//IL_106e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecId > 0 && ReferredInExportVoucher)
			{
				MessageBox.Show("This Document is referred in Export Voucher. So you can't update this record.", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (grdDetail.RowCount == 0)
			{
				throw new Exception("Please Check Detail Grid");
			}
			if (!formValidation())
			{
				return;
			}
			ExImForwarding forwarding = new ExImForwarding();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				forwarding.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			double StockWeight = 0.0;
			double DoInvoiceWt = Conversion.ToDouble((object)txtInvoiceNetWeight.Text.Trim());
			double FactoryWeight = Conversion.ToDouble((object)txtBalFcWeight.Text.Trim());
			double num = Conversion.ToDouble((object)grdDetail.GetTotal(grdDetail.RootTable.Columns["NetWeight"], (AggregateFunction)2).ToString());
			StockWeight = Conversion.ToDouble((object)grdDetail.GetTotal(grdDetail.RootTable.Columns["StockWeight"], (AggregateFunction)2).ToString());
			if (Math.Abs(Conversion.ToDecimal((object)(num - DoInvoiceWt))) > 1m)
			{
				throw new Exception("SumOfGrid NetWeight and DoNetWeight not equal please check");
			}
			if (btnSave.Visible && btnSave.Enabled)
			{
				if (Conversion.ToInt((object)txtNoofInvoices.Text) == 1)
				{
					if (StockWeight == FactoryWeight)
					{
					}
				}
				else if (StockWeight > FactoryWeight)
				{
					throw new Exception("StockWeight cannot be greater than FactoryWeight please check");
				}
			}
			ValidateDetailRecordsWithorWithoutLoader();
			forwarding.OrganizationId = UserAccount.OrganizationId;
			forwarding.CompanyId = UserAccount.CompanyId;
			forwarding.BranchesId = UserAccount.BranchesId;
			forwarding.DocNo = Conversion.ToInt((object)txtdocno.Text.Trim());
			forwarding.DocDate = DocDate.Value;
			forwarding.DocumentTypeId = 205;
			forwarding.ExImInvoiceId = Conversion.ToInt(cmbInvoiceNo.Value);
			forwarding.GatePassOutwardId = Conversion.ToInt(cmbGpNo.Value);
			forwarding.GpNo = Conversion.ToString((object)((Control)(object)cmbGpNo).Text.Trim());
			forwarding.GPDate = txtGPDate.Value;
			forwarding.TransporterId = Conversion.ToInt(cmbtransporter.Value);
			forwarding.VehicleNo = txtVehicleNo.Text;
			forwarding.BiltyNo = txtBilityNo.Text;
			forwarding.DriverName = txtDriverName.Text;
			forwarding.DriverCellNo = txtDriverCellNo.Text;
			forwarding.DriverCnicNo = txtCNICNO.Text;
			forwarding.Container = txtconainer1.Text;
			forwarding.Container1 = txtContainer2.Text;
			forwarding.FreightAmt = Conversion.ToDouble((object)txtFreight.Text);
			forwarding.FactoryWeight = Conversion.ToDouble((object)txtFactoryWeight.Text);
			forwarding.OtherCharges = Conversion.ToDouble((object)txtFactoryWeight.Text);
			forwarding.ShippingLineId = Conversion.ToInt(cmbShippingLIne.Value);
			forwarding.ShippingAgentId = Conversion.ToInt(cmbShippingAgent.Value);
			forwarding.SupplierCustomerId = Conversion.ToInt(cmbCustomer.Value);
			forwarding.LoadingPortId = Conversion.ToInt(cmbLoadingPort.Value);
			forwarding.DestinationPortId = Conversion.ToInt(cmbDestinationPort.Value);
			forwarding.ShippedContainer = Conversion.ToInt((object)txtShippedContainer.Text);
			if (chkdateisactive.Checked)
			{
				forwarding.LotCompletedDate = txtCompletedDate.Value;
			}
			forwarding.LotCompletedDate = txtCompletedDate.Value;
			forwarding.NoOfContainer = Conversion.ToInt((object)txtNoOfContainer.Text);
			forwarding.EntryUser = UserAccount.ID;
			forwarding.ModifyUser = UserAccount.ID;
			forwarding.EntryDate = DateTime.Now;
			forwarding.ModifyDate = DateTime.Now;
			forwarding.IsApproved = false;
			forwarding.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			forwarding.ExImForwardingDetail = new List<ExImForwardingPackingDetail>();
			forwarding.ExImForwardingOtherItemslist = new List<ExImForwardingOtherItems>();
			if (RecId > 0 && LstRemoveRecordDetail.Count > 0)
			{
				for (int i = 0; i < LstRemoveRecordDetail.Count; i++)
				{
					forwarding.ExImForwardingDetail.Add(LstRemoveRecordDetail[i]);
				}
			}
			double GrossWeight = 0.0;
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				ExImForwardingPackingDetail exImForwardingPackingDetail = new ExImForwardingPackingDetail();
				exImForwardingPackingDetail.Id = Conversion.ToInt(r2.Cells["Id"].Value);
				exImForwardingPackingDetail.RefDocumentTypeId = Conversion.ToInt(r2.Cells["RefDocumentTypeId"].Value);
				exImForwardingPackingDetail.RefDocIdNo = Conversion.ToInt(r2.Cells["RefDocIdNo"].Value);
				exImForwardingPackingDetail.RefDocSubIdNo = Conversion.ToInt(r2.Cells["RefDocSubIdNo"].Value);
				exImForwardingPackingDetail.ExImLcOrderId = Conversion.ToInt(r2.Cells["ContractId"].Value);
				if (exImForwardingPackingDetail.ExImLcOrderId == 0)
				{
					throw new Exception("ContractId Not found in row#" + (r2.RowIndex + 1));
				}
				exImForwardingPackingDetail.ContractDetailId = Conversion.ToInt(r2.Cells["ContractDetailId"].Value);
				if (exImForwardingPackingDetail.ContractDetailId == 0)
				{
					throw new Exception("Contract DetailId Not found in row#" + (r2.RowIndex + 1));
				}
				exImForwardingPackingDetail.InvoiceId = Conversion.ToInt(cmbInvoiceNo.Value);
				exImForwardingPackingDetail.InvoiceDetailId = Conversion.ToInt(r2.Cells["InvoiceDetailId"].Value);
				if (exImForwardingPackingDetail.InvoiceDetailId == 0)
				{
					throw new Exception("Invoice Detail Id Not found in row#" + (r2.RowIndex + 1));
				}
				if (Conversion.ToInt(r2.Cells["ItemId"].Value) != 0)
				{
					exImForwardingPackingDetail.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
					exImForwardingPackingDetail.ItemName = Conversion.ToString((object)r2.Cells["ItemName"].Text.Trim());
					exImForwardingPackingDetail.ItemDescription = Conversion.ToString((object)r2.Cells["ItemDescription"].Text.Trim());
					if (Conversion.ToInt(r2.Cells["PackTypeId"].Value) != 0)
					{
						exImForwardingPackingDetail.PackingMaterialId = Conversion.ToInt(r2.Cells["PackTypeId"].Value);
						if (Conversion.ToInt(r2.Cells["MasterUOMId"].Value) != 0)
						{
							exImForwardingPackingDetail.UOMScheduleIdOuter = Conversion.ToInt(r2.Cells["MasterUOMId"].Value);
							exImForwardingPackingDetail.EbUnit = Conversion.ToDouble(r2.Cells["EbUnitMaster"].Value);
							exImForwardingPackingDetail.EbTotal = Conversion.ToDouble(r2.Cells["EbTotalMaster"].Value);
							if (Conversion.ToDouble(r2.Cells["MasterQTY"].Value) != 0.0)
							{
								exImForwardingPackingDetail.OuterQty = Conversion.ToDouble(r2.Cells["MasterQTY"].Value);
								exImForwardingPackingDetail.InnerQty = Conversion.ToDouble(r2.Cells["InnerQTY"].Value);
								exImForwardingPackingDetail.UOMScheduleIdInner = Conversion.ToInt(r2.Cells["InnerUOMId"].Value);
								exImForwardingPackingDetail.InnerEbUnit = Conversion.ToDouble(r2.Cells["EbUnitInner"].Value);
								exImForwardingPackingDetail.InnerEbTotal = Conversion.ToDouble(r2.Cells["EbTotalInner"].Value);
								exImForwardingPackingDetail.TotalPackingWeight = Conversion.ToDouble((object)(exImForwardingPackingDetail.InnerEbTotal + exImForwardingPackingDetail.EbTotal));
								exImForwardingPackingDetail.AdLsWeight = Conversion.ToDouble((object)r2.Cells["AddLess"].Text.Trim());
								if (Conversion.ToDouble(r2.Cells["NetWeight"].Value) != 0.0)
								{
									exImForwardingPackingDetail.NetWeight = Conversion.ToDouble((object)r2.Cells["NetWeight"].Text.Trim());
									if (Conversion.ToDouble(r2.Cells["GrossWeight"].Value) != 0.0)
									{
										exImForwardingPackingDetail.GrossWeight = Conversion.ToDouble((object)r2.Cells["GrossWeight"].Text.Trim());
										GrossWeight += exImForwardingPackingDetail.GrossWeight;
										if (!NetWeightAndStockWeightEqualOnForwarding)
										{
											exImForwardingPackingDetail.StockWeight = Conversion.ToDouble((object)r2.Cells["StockWeight"].Text.Trim());
										}
										else
										{
											exImForwardingPackingDetail.StockWeight = exImForwardingPackingDetail.NetWeight;
										}
										exImForwardingPackingDetail.CropYearId = Conversion.ToInt(r2.Cells["CropYearId"].Value);
										exImForwardingPackingDetail.CropYear = Conversion.ToString(r2.Cells["CropYear"].Value);
										if (exImForwardingPackingDetail.CropYearId == 0)
										{
											MessageBox.Show("CropYear Required in row#" + (r2.RowIndex + 1));
											return;
										}
										if (Conversion.ToInt(r2.Cells["WarehouseId"].Value) != 0)
										{
											exImForwardingPackingDetail.WarehouseId = Conversion.ToInt(r2.Cells["WarehouseId"].Value);
											if (Conversion.ToInt(r2.Cells["JobLotId"].Value) != 0)
											{
												exImForwardingPackingDetail.LotJobId = Conversion.ToInt(r2.Cells["JobLotId"].Value);
												exImForwardingPackingDetail.JobLotName = Conversion.ToString(r2.Cells["JobLot"].Value);
												ItemIdExistsInJobLot(exImForwardingPackingDetail.ItemId, exImForwardingPackingDetail.ItemName, exImForwardingPackingDetail.LotJobId, exImForwardingPackingDetail.JobLotName);
												exImForwardingPackingDetail.CropYearId = Conversion.ToInt(r2.Cells["CropYearId"].Value);
												if (txtconainer1.Text != string.Empty || txtContainer2.Text != string.Empty)
												{
													if (!(Conversion.ToString(r2.Cells["Container#"].Value) != string.Empty))
													{
														MessageBox.Show("Container Required in row#" + (r2.RowIndex + 1));
														return;
													}
													exImForwardingPackingDetail.ContainerNo = Conversion.ToString((object)r2.Cells["Container#"].Text.Trim());
												}
												else
												{
													exImForwardingPackingDetail.ContainerNo = Conversion.ToString((object)r2.Cells["Container#"].Text.Trim());
												}
												exImForwardingPackingDetail.ContainerId = Conversion.ToInt(r2.Cells["ContainerId"].Value);
												exImForwardingPackingDetail.DoDetailId = Conversion.ToInt(r2.Cells["DoDetailId"].Value);
												if (Conversion.ToString(r2.Cells["Seal#"].Value) != string.Empty)
												{
													exImForwardingPackingDetail.SealNo = Conversion.ToString((object)r2.Cells["Seal#"].Text.Trim());
													exImForwardingPackingDetail.ActionTypeId = ((exImForwardingPackingDetail.Id <= 0) ? 1 : 2);
													forwarding.ExImForwardingDetail.Add(exImForwardingPackingDetail);
													continue;
												}
												MessageBox.Show("Seal No Required in row#" + (r2.RowIndex + 1));
												return;
											}
											MessageBox.Show("Job Lot Required in row#" + (r2.RowIndex + 1));
											return;
										}
										MessageBox.Show("Warehouse Required in row#" + (r2.RowIndex + 1));
										return;
									}
									MessageBox.Show("Gross Weight Required in row#" + (r2.RowIndex + 1));
									return;
								}
								MessageBox.Show("NetWeight Required in row#" + (r2.RowIndex + 1));
								return;
							}
							MessageBox.Show("Master QTY Required in row#" + (r2.RowIndex + 1));
							return;
						}
						MessageBox.Show("Master UOM Required In Grid");
						return;
					}
					MessageBox.Show("Packing Type Required in row#" + (r2.RowIndex + 1));
					return;
				}
				MessageBox.Show("Item Required in row#" + (r2.RowIndex + 1));
				return;
			}
			if (grdotheritems.RowCount > 0)
			{
				rows = grdotheritems.GetRows();
				foreach (GridEXRow r in rows)
				{
					ExImForwardingOtherItems vd = new ExImForwardingOtherItems();
					vd.otherItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					vd.oItemQty = Conversion.ToDecimal(r.Cells["Qty"].Value);
					vd.oItemRate = Conversion.ToDouble(r.Cells["Rate"].Value);
					vd.oItemAmount = Conversion.ToDecimal(r.Cells["Amount"].Value);
					vd.OtherItemRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
					vd.ContainerNo = Conversion.ToString(r.Cells["ContainerNo"].Value);
					forwarding.ExImForwardingOtherItemslist.Add(vd);
				}
			}
			forwarding.AttachmentsList = new List<DMSAttachments>();
			forwarding.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<ExImForwarding>(forwarding, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = ExImForwarding.Save(forwarding);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update SuccessFully");
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, forwarding.DocumentTypeId, base.Name);
			}
			else
			{
				MessageBox.Show("Record Save SuccessFully");
			}
			if (WagesCompulsoryOnForwarding && WagesActiveOrInActive)
			{
				frmwagesBillHeader frmwagesBillHeader = new frmwagesBillHeader(UserAccount);
				frmwagesBillHeader.RefDocTypeId = 205;
				frmwagesBillHeader.RefDocId = success;
				frmwagesBillHeader.GrossWeightTotal = GrossWeight;
				frmwagesBillHeader.ShowDialog();
				frmwagesBillHeader.BringToFront();
			}
			FormReset();
			GridGatepassFill();
			if (ChkPrintPreview.Checked)
			{
				DataTable dt = ExportPdfReport.ExImForwarding_Slip_507(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Conversion.ToInt((object)success)
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("Not Record Found For Display");
					return;
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "507-ExImForwarding_Slip.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ItemIdExistsInJobLot(int itemId, string itemName, int jobLotId, string jobLotName, int rowNo = 0)
	{
		try
		{
			if (!SaleCostingJobOrderWise || dtJobLotWithJobOrderAndItems.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == jobLotId && row.Field<int>("ItemId") == itemId))
			{
				return;
			}
			throw new Exception((rowNo == 0) ? ("The selected item '" + itemName + "' does not exist against the job lot '" + jobLotName + "'.") : $"The selected item '{itemName}' does not exist against the job lot '{jobLotName}' in detail grid row no: {rowNo}.");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0;
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
			if (RecId == 0)
			{
				throw new Exception("Record not update because Id not found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int Id, bool AutoUpdate = false)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Expected O, but got Unknown
		try
		{
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			LstRemoveRecordDetail.Clear();
			ExImForwarding exImLc = new ExImForwarding();
			exImLc = ExImForwarding.GetByID(Id);
			if (exImLc != null)
			{
				tabControl1.SelectedIndex = 0;
				tabControl2.SelectedIndex = 0;
				txtdocno.Text = Conversion.ToString((object)exImLc.DocNo);
				DocDate.Value = exImLc.DocDate;
				txtGPDate.Value = exImLc.GPDate;
				DataTable dtGatePass = new DataTable();
				dtGatePass.Columns.Add("Id");
				dtGatePass.Columns.Add("GpSrNo");
				dtGatePass.Rows.Add(exImLc.GatePassOutwardId, exImLc.GpSrNo);
				if (dtGatePass.Rows.Count > 0)
				{
					DDL.BindDDL(dtGatePass, cmbGpNo, "Id", "GpSrNo", "GpNo", ZeroIndex: false);
					((UltraGridBase)cmbGpNo).Rows[0].Activate();
				}
				DataTable dtInv = new DataTable();
				dtInv.Columns.Add("Id");
				dtInv.Columns.Add("InvoiceNo");
				dtInv.Rows.Add(exImLc.ExImInvoiceId, exImLc.InvoiceNo);
				if (dtInv.Rows.Count > 0)
				{
					DDL.BindDDL(dtInv, cmbInvoiceNo, "Id", "InvoiceNo", "InvoiceNo", ZeroIndex: false);
					((UltraGridBase)cmbInvoiceNo).Rows[0].Activate();
				}
				DataTable dtCustomer = new DataTable();
				dtCustomer.Columns.Add("SupplierCustomerId");
				dtCustomer.Columns.Add("Customer");
				dtCustomer.Rows.Add(exImLc.SupplierCustomerId, exImLc.Customer);
				if (dtCustomer.Rows.Count > 0)
				{
					DDL.BindDDL(dtCustomer, cmbCustomer, "SupplierCustomerId", "Customer", "CustomerName", ZeroIndex: false);
					((UltraGridBase)cmbCustomer).Rows[0].Activate();
				}
				int NoOfInvoices = ExImInvoice.GetInvoiceNoForExportForwarding(new ExImInvoice
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Conversion.ToInt(cmbGpNo.Value),
					FinancialYearId = clsGlobalVariables.ActiveYr.Id
				}).Rows.Count + 1;
				txtNoofInvoices.Text = NoOfInvoices.ToString();
				cmbShippingLIne.Value = Conversion.ToInt((object)exImLc.ShippingLineId);
				cmbShippingAgent.Value = Conversion.ToInt((object)exImLc.ShippingAgentId);
				cmbtransporter.Value = Conversion.ToInt((object)exImLc.TransporterId);
				txtDriverCellNo.Text = Conversion.ToString((object)exImLc.DriverCellNo);
				txtDriverName.Text = Conversion.ToString((object)exImLc.DriverName);
				txtCNICNO.Text = Conversion.ToString((object)exImLc.DriverCnicNo);
				txtGPDate.Value = exImLc.GPDate;
				txtVehicleNo.Text = Conversion.ToString((object)exImLc.VehicleNo);
				txtBilityNo.Text = Conversion.ToString((object)exImLc.BiltyNo);
				txtconainer1.Text = Conversion.ToString((object)exImLc.Container);
				txtContainer2.Text = Conversion.ToString((object)exImLc.Container1);
				txtFactoryWeight.Text = Conversion.ToString((object)exImLc.OtherCharges);
				cmbDestinationPort.Value = exImLc.DestinationPortId;
				cmbLoadingPort.Value = exImLc.LoadingPortId;
				txtShippedContainer.Text = Conversion.ToString((object)exImLc.ShippedContainer);
				txtNoOfContainer.Text = Conversion.ToString((object)exImLc.NoOfContainer);
				txtCompletedDate.Value = Conversion.ToDateTime((object)exImLc.LotCompletedDate);
				dtDetail.Rows.Clear();
				double InvoiceWt = 0.0;
				for (int i = 0; i < exImLc.ExImForwardingDetail.Count; i++)
				{
					dtDetail.Rows.Add(exImLc.ExImForwardingDetail[i].Id, exImLc.ExImForwardingDetail[i].RefDocumentTypeId, exImLc.ExImForwardingDetail[i].RefDocIdNo, exImLc.ExImForwardingDetail[i].RefDocSubIdNo, exImLc.ExImForwardingDetail[i].InvoiceDetailId, exImLc.ExImForwardingDetail[i].ExImLcOrderId, exImLc.ExImForwardingDetail[i].ContractDetailId, exImLc.ExImForwardingDetail[i].LcOrderNo, exImLc.ExImForwardingDetail[i].ItemId, exImLc.ExImForwardingDetail[i].ItemName, exImLc.ExImForwardingDetail[i].PackingMaterialId, exImLc.ExImForwardingDetail[i].Packtype, exImLc.ExImForwardingDetail[i].ItemDescription, exImLc.ExImForwardingDetail[i].OuterQty, exImLc.ExImForwardingDetail[i].UOMScheduleIdOuter, exImLc.ExImForwardingDetail[i].OuterUOM, exImLc.ExImForwardingDetail[i].OuterEquivalent, exImLc.ExImForwardingDetail[i].EbUnit, exImLc.ExImForwardingDetail[i].EbTotal, exImLc.ExImForwardingDetail[i].InnerQty, exImLc.ExImForwardingDetail[i].UOMScheduleIdInner, exImLc.ExImForwardingDetail[i].InnerUOM, exImLc.ExImForwardingDetail[i].InnerEquivalent, exImLc.ExImForwardingDetail[i].InnerEbUnit, exImLc.ExImForwardingDetail[i].InnerEbTotal, exImLc.ExImForwardingDetail[i].AdLsWeight, exImLc.ExImForwardingDetail[i].NetWeight, exImLc.ExImForwardingDetail[i].GrossWeight, exImLc.ExImForwardingDetail[i].StockWeight, exImLc.ExImForwardingDetail[i].WarehouseId, exImLc.ExImForwardingDetail[i].WareHouseName, exImLc.ExImForwardingDetail[i].LotJobId, exImLc.ExImForwardingDetail[i].JobLotName, exImLc.ExImForwardingDetail[i].CropYearId, exImLc.ExImForwardingDetail[i].CropYear, exImLc.ExImForwardingDetail[i].ContainerNo, exImLc.ExImForwardingDetail[i].SealNo, exImLc.ExImForwardingDetail[i].ContainerId, exImLc.ExImForwardingDetail[i].RefDocType, exImLc.ExImForwardingDetail[i].RefDocNo, exImLc.ExImForwardingDetail[i].OuterQty, exImLc.ExImForwardingDetail[i].StockWeight, exImLc.ExImForwardingDetail[i].DoDetailId);
					InvoiceWt += exImLc.ExImForwardingDetail[i].NetWeight;
					PreStockWt += exImLc.ExImForwardingDetail[i].StockWeight + exImLc.ExImForwardingDetail[i].TotalPackingWeight + exImLc.ExImForwardingDetail[i].AdLsWeight;
				}
				txtInvoiceNetWeight.Text = InvoiceWt.ToString();
				GetGatePassDataForForwarding();
				GetContainersForForwarding();
				txtFreight.Text = Conversion.ToString((object)exImLc.FreightAmt);
				grdDetail.DataSource = dtDetail;
				grdDetail.RetrieveStructure();
				GridDetailSetting();
				dtOtherItem.Rows.Clear();
				DataTable dtOItem = new DataTable();
				dtOItem.Columns.Add("Id", typeof(int));
				dtOItem.Columns.Add("ItemName", typeof(string));
				int x;
				for (x = 0; x < exImLc.ExImForwardingOtherItemslist.Count; x++)
				{
					if (!dtOItem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == exImLc.ExImForwardingOtherItemslist[x].otherItemId))
					{
						dtOItem.Rows.Add(exImLc.ExImForwardingOtherItemslist[x].otherItemId, exImLc.ExImForwardingOtherItemslist[x].ItemName);
					}
					dtOtherItem.Rows.Add(exImLc.ExImForwardingOtherItemslist[x].otherItemId, exImLc.ExImForwardingOtherItemslist[x].ItemName, exImLc.ExImForwardingOtherItemslist[x].oItemQty, exImLc.ExImForwardingOtherItemslist[x].oItemRate, exImLc.ExImForwardingOtherItemslist[x].oItemAmount, exImLc.ExImForwardingOtherItemslist[x].ContainerNo, exImLc.ExImForwardingOtherItemslist[x].OtherItemRemarks);
				}
				grdotheritems.DataSource = dtOtherItem;
				grdotheritems.RetrieveStructure();
				grdSettingsOtherItems();
				DDL.BindDDL(dtOItem, CmbOtherItem, "Id", "ItemName", "Item Name", ZeroIndex: false);
				if (dtOItem.Rows.Count > 0)
				{
					((UltraGridBase)CmbOtherItem).DisplayLayout.Bands[0].Columns[1].Width = 400;
				}
				UpdateMode = true;
				((Control)(object)cmbGpNo).Enabled = false;
				((Control)(object)cmbInvoiceNo).Enabled = false;
				if (!AutoUpdate)
				{
					FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
				}
			}
			else
			{
				FormReset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		try
		{
			if (RecId > 0 && ReferredInExportVoucher)
			{
				MessageBox.Show("This Document is referred in Export Voucher. So you can't delete this record.", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvPurchaseInvoice.RemoveByID(new InvPurchaseInvoice
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocumentTypeId = 205,
						Id = RecId,
						EntryUser = UserAccount.ID
					});
					MessageBox.Show("Delete Record Successfully");
					FormReset();
				}
				return;
			}
			throw new Exception("Record Not Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void FormReset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			LstRemoveRecordDetail.Clear();
			RecId = 0;
			PreStockWt = 0.0;
			generateCode();
			gatepassGetAll();
			((Control)(object)cmbGpNo).Enabled = true;
			ReferredInExportVoucher = false;
			((Control)(object)cmbtransporter).Text = string.Empty;
			((Control)(object)cmbLoadingPort).Text = string.Empty;
			((Control)(object)cmbDestinationPort).Text = string.Empty;
			((Control)(object)cmbShippingLIne).Text = string.Empty;
			((Control)(object)cmbInvoiceNo).Text = string.Empty;
			((Control)(object)cmbShippingAgent).Text = string.Empty;
			txtCompletedDate.Value = DateTime.Now;
			((Control)(object)cmbGpNo).Text = string.Empty;
			txtFactoryWeight.Text = "0";
			txtVehicleNo.Text = string.Empty;
			txtBilityNo.Text = string.Empty;
			txtDriverName.Text = string.Empty;
			txtDriverCellNo.Text = string.Empty;
			txtCNICNO.Text = string.Empty;
			txtOtherCharges.Text = string.Empty;
			txtFreight.Text = string.Empty;
			txtDispatchWeight.Clear();
			txtBalFcWeight.Clear();
			txtShippedContainer.Text = string.Empty;
			txtNoOfContainer.Text = string.Empty;
			txtCompletedDate.Value = DateTime.Now;
			((Control)(object)cmbCustomer).Text = string.Empty;
			txtconainer1.Text = string.Empty;
			txtContainer2.Text = string.Empty;
			txtDoGrossWeight.Text = string.Empty;
			txtDoNetWeight.Text = string.Empty;
			txtInvoiceNetWeight.Text = string.Empty;
			((Control)(object)cmbItem).Text = string.Empty;
			((UltraGridBase)cmbItem).DataSource = null;
			((Control)(object)CmbPackUomMaster).Text = string.Empty;
			((UltraGridBase)CmbPackUomMaster).DataSource = null;
			((Control)(object)CmbPackUomInner).Text = string.Empty;
			((UltraGridBase)CmbPackUomInner).DataSource = null;
			cmbGpNo.Focus();
			dtDetail.Rows.Clear();
			dtOtherItem.Rows.Clear();
			grdDetail.ClearStructure();
			grdotheritems.ClearStructure();
			btnSave.Visible = true;
			UpdateMode = false;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			((Control)(object)cmbGpNo).Enabled = true;
			((Control)(object)cmbInvoiceNo).Enabled = true;
			FormDetailReset();
			updateDetailIndexOther = -1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FormDetailReset()
	{
		try
		{
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)cmbPackType).Text = string.Empty;
			txtItemDesc.Text = string.Empty;
			txtNetWeight.Text = string.Empty;
			((Control)(object)CmbPackUomMaster).Text = string.Empty;
			((Control)(object)CmbPackUomInner).Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			txtEbUnitInner.Text = string.Empty;
			txtEbTotalInner.Text = string.Empty;
			txtEbUnitMaster.Text = string.Empty;
			txtEbTotalMaster.Text = string.Empty;
			txtAddLess.Text = string.Empty;
			txtStockWeight.Text = string.Empty;
			txtSeal.Text = string.Empty;
			((Control)(object)CmbWareHouse).Text = string.Empty;
			((Control)(object)CmbJobLot).Text = string.Empty;
			((Control)(object)cmbcontainerNo).Text = string.Empty;
			btnDetailUpdate.Visible = false;
			btnDetailCancel.Visible = false;
			btnAddinGrid.Visible = false;
			((Control)(object)cmbcontainerNo).Enabled = true;
			updateDetailIndex = -1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetOtherItems()
	{
		try
		{
			((Control)(object)CmbOtherItem).Text = string.Empty;
			txtoQty.Text = string.Empty;
			txtoRate.Text = string.Empty;
			txtoAmount.Text = string.Empty;
			txtOContainer.Text = string.Empty;
			txtoRemarks.Text = string.Empty;
			updateDetailIndexOther = -1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnNew_Click(object sender, EventArgs e)
	{
		try
		{
			FormReset();
			FormDetailReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EximForwarding_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "UomSchedule" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			SaleCostingJobOrderWise = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("SaleCostingJobOrderWise"));
			if (SaleCostingJobOrderWise)
			{
				dtJobLotWithJobOrderAndItems = jobLot.JobLot_GetWithJobOrderAndItem(UserAccount.OrganizationId, UserAccount.CompanyId, 0);
			}
			WagesRefDocumentsStatus obj = clsGlobalVariables.WagesRefDocumentsStatusList.Find((WagesRefDocumentsStatus row) => row.RefDocumentTypeId == 205);
			WagesActiveOrInActive = obj != null && obj.IsActive;
			NetWeightAndStockWeightEqualOnForwarding = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("NetWeightAndStockWeightEqualOnForwarding"));
			gatepassGetAll();
			ShippingAgentFill();
			packingType();
			bindSeaPort();
			JobLotFill();
			WarehouseFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formdetailvalidation()
	{
		if (((UltraGridBase)cmbItem).ActiveRow == null || Conversion.ToInt(cmbItem.Value) == 0)
		{
			MessageBox.Show("Please select Item");
			cmbItem.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPackType).ActiveRow == null || Conversion.ToInt(cmbPackType.Value) == 0)
		{
			MessageBox.Show("Please select PackType");
			cmbPackType.Focus();
			return false;
		}
		if (txtQtyMaster.Text.Trim() == "" || !(Conversion.ToDouble((object)txtQtyMaster.Text) > 0.0))
		{
			MessageBox.Show("Please Insert Master Qty");
			txtQtyMaster.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUomMaster).ActiveRow == null || Conversion.ToInt(CmbPackUomMaster.Value) == 0)
		{
			MessageBox.Show("Please Select Master Pack UOM");
			CmbPackUomMaster.Focus();
			return false;
		}
		if (txtQtyInner.Text.Trim() == "" || !(Conversion.ToDouble((object)txtQtyInner.Text) > 0.0))
		{
			MessageBox.Show("Please Insert Inner Qty");
			txtQtyInner.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUomInner).ActiveRow == null || Conversion.ToInt(CmbPackUomInner.Value) == 0)
		{
			MessageBox.Show("Please Select Pack UOM");
			CmbPackUomInner.Focus();
			return false;
		}
		if (txtNetWeight.Text.Trim() == "" || Conversion.ToDouble((object)txtNetWeight.Text) == 0.0)
		{
			MessageBox.Show("Please Insert NetWeight");
			txtNetWeight.Focus();
			return false;
		}
		if (txtGrossWeight.Text.Trim() == "" || !(Conversion.ToDouble((object)txtGrossWeight.Text) > 0.0))
		{
			MessageBox.Show("Please Insert Gross weight");
			txtGrossWeight.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWareHouse).ActiveRow == null || Conversion.ToInt(CmbWareHouse.Value) == 0)
		{
			MessageBox.Show("Please Select Warehouse");
			CmbWareHouse.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobLot).ActiveRow == null || Conversion.ToInt(CmbJobLot.Value) == 0)
		{
			MessageBox.Show("Please Select Job Lot");
			CmbJobLot.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCropYear).ActiveRow == null || Conversion.ToInt(CmbCropYear.Value) == 0)
		{
			MessageBox.Show("Please Select CropYear");
			CmbCropYear.Focus();
			return false;
		}
		if (txtSeal.Text.Trim() == "" || txtSeal.Text == "0")
		{
			MessageBox.Show("Please Insert Seal No");
			txtSeal.Focus();
			return false;
		}
		return true;
	}

	private bool formValidation()
	{
		if (txtdocno.Text == string.Empty || txtdocno.Text == null || txtdocno.Text == "0")
		{
			MessageBox.Show("Please Check Doc No");
			txtdocno.Focus();
			return false;
		}
		if (((Control)(object)cmbInvoiceNo).Text.Trim() == string.Empty || ((Control)(object)cmbInvoiceNo).Text == null || Conversion.ToInt(cmbInvoiceNo.Value) == 0)
		{
			MessageBox.Show("Please Select Invoice No");
			cmbInvoiceNo.Focus();
			return false;
		}
		if (((Control)(object)cmbCustomer).Text.Trim() == string.Empty || ((Control)(object)cmbCustomer).Text == null || Conversion.ToInt(cmbCustomer.Value) == 0)
		{
			MessageBox.Show("Please Select Customer");
			cmbCustomer.Focus();
			return false;
		}
		if (((Control)(object)cmbGpNo).Text.Trim() == "" || Conversion.ToInt(cmbGpNo.Value) == 0)
		{
			MessageBox.Show("Please Select GatePass No");
			cmbGpNo.Focus();
			return false;
		}
		if (txtVehicleNo.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Please Check Vehicle No");
			txtVehicleNo.Focus();
			return false;
		}
		if (((Control)(object)cmbLoadingPort).Text.Trim() == string.Empty || ((Control)(object)cmbLoadingPort).Text == null || Conversion.ToInt(cmbLoadingPort.Value) == 0)
		{
			MessageBox.Show("Please Select Loading Port");
			cmbLoadingPort.Focus();
			return false;
		}
		if (((Control)(object)cmbDestinationPort).Text.Trim() == string.Empty || ((Control)(object)cmbDestinationPort).Text == null || Conversion.ToInt(cmbDestinationPort.Value) == 0)
		{
			MessageBox.Show("Please Select Destination Port");
			cmbDestinationPort.Focus();
			return false;
		}
		if (txtNoOfContainer.Text.Trim() == string.Empty || Conversion.ToInt((object)txtNoOfContainer.Text) == 0)
		{
			MessageBox.Show("Please Check No Of Container");
			txtNoOfContainer.Focus();
			return false;
		}
		if (txtFactoryWeight.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtFactoryWeight.Text) == 0.0)
		{
			MessageBox.Show("Please Check Factory Weight");
			txtFactoryWeight.Focus();
			return false;
		}
		if (txtDoGrossWeight.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtDoGrossWeight.Text) == 0.0)
		{
			MessageBox.Show("Please Check Do GrossWeight");
			txtDoGrossWeight.Focus();
			return false;
		}
		if (txtDoNetWeight.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtDoNetWeight.Text) == 0.0)
		{
			MessageBox.Show("Please Check Do NetWeight");
			txtDoNetWeight.Focus();
			return false;
		}
		return true;
	}

	private bool OtherItemFormValidation()
	{
		if (((UltraGridBase)CmbOtherItem).ActiveRow == null)
		{
			MessageBox.Show("Item field required");
			CmbOtherItem.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtoQty.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Qty field required");
			txtoQty.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtoRate.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Rate field required");
			txtoRate.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtoAmount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Amount field required");
			txtoAmount.Focus();
			return false;
		}
		if (Conversion.ToString((object)txtOContainer.Text.Trim()) == string.Empty)
		{
			MessageBox.Show("ContainerNo field required");
			txtOContainer.Focus();
			return false;
		}
		return true;
	}

	private void btnAddinGrid_Click(object sender, EventArgs e)
	{
		try
		{
			if (formdetailvalidation())
			{
				dtDetail.Rows.Add(0, 0, 0, 0, 0, 0, 0, "", Conversion.ToInt(cmbItem.Value), ((Control)(object)cmbItem).Text, Conversion.ToInt(cmbPackType.Value), ((Control)(object)cmbPackType).Text, txtItemDesc.Text, Conversion.ToDouble((object)txtQtyMaster.Text.Trim()), Conversion.ToInt(CmbPackUomMaster.Value), ((Control)(object)CmbPackUomMaster).Text.Trim(), Conversion.ToDouble(((UltraDropDownBase)CmbPackUomMaster).SelectedRow.Cells[2].Value), Conversion.ToDouble((object)txtEbUnitMaster.Text), Conversion.ToDouble((object)txtEbTotalMaster.Text), Conversion.ToDouble((object)txtQtyInner.Text.Trim()), Conversion.ToInt(CmbPackUomInner.Value), ((Control)(object)CmbPackUomInner).Text.Trim(), Conversion.ToDouble(((UltraDropDownBase)CmbPackUomInner).SelectedRow.Cells[2].Value), Conversion.ToDouble((object)txtEbUnitInner.Text), Conversion.ToDouble((object)txtEbTotalInner.Text), Conversion.ToDouble((object)txtAddLess.Text.Trim()), Conversion.ToDouble((object)txtNetWeight.Text.Trim()), Conversion.ToDouble((object)txtGrossWeight.Text.Trim()), Conversion.ToDouble((object)txtStockWeight.Text.Trim()), Conversion.ToInt(CmbWareHouse.Value), ((Control)(object)CmbWareHouse).Text.Trim(), Conversion.ToInt(CmbJobLot.Value), Conversion.ToString((object)((Control)(object)CmbJobLot).Text.Trim()), Conversion.ToInt(CmbCropYear.Value), Conversion.ToString((object)((Control)(object)CmbCropYear).Text.Trim()), ((Control)(object)cmbcontainerNo).Text.Trim(), txtSeal.Text);
				grdDetail.DataSource = dtDetail;
				grdDetail.RetrieveStructure();
				FormDetailReset();
				GridDetailSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_DoubleClick(object sender, EventArgs e)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdDetail.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count <= 0)
			{
				return;
			}
			updateDetailIndex = item.RowIndex;
			if (Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) > 0 && Conversion.ToInt(item.Cells["RefDocIdNo"].Value) > 0 && Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value) > 0)
			{
				((Control)(object)CmbWareHouse).Enabled = false;
				((Control)(object)CmbJobLot).Enabled = false;
				((Control)(object)CmbCropYear).Enabled = false;
			}
			else
			{
				((Control)(object)CmbWareHouse).Enabled = true;
				((Control)(object)CmbJobLot).Enabled = true;
				((Control)(object)CmbCropYear).Enabled = true;
			}
			if (RecId > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("ItemName");
				((Control)(object)cmbItem).Text = item.Cells["ItemName"].Text.ToString();
				dt.Rows.Add(item.Cells["ItemId"].Value, item.Cells["ItemName"].Text.ToString());
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbItem, "Id", "ItemName", "ItemName", ZeroIndex: false);
					((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 450;
				}
			}
			DataTable dtContainer = (DataTable)((UltraGridBase)cmbcontainerNo).DataSource;
			if (dtDetail != null && dtDetail.Rows.Count > 0)
			{
				int i;
				for (i = 0; i < dtDetail.Rows.Count; i++)
				{
					if (dtContainer == null)
					{
						dtContainer = new DataTable();
						dtContainer.Columns.Add("Id", typeof(string));
						dtContainer.Columns.Add("Container", typeof(string));
						dtContainer.Columns.Add("SealNo", typeof(string));
					}
					if (!dtContainer.AsEnumerable().Any((DataRow row) => row.Field<string>("Container") == Conversion.ToString(dtDetail.Rows[i]["Container#"])) && (Conversion.ToInt(dtDetail.Rows[i]["ContainerId"]) > 0 || (Conversion.ToString(dtDetail.Rows[i]["Container#"]) != null && Conversion.ToString(dtDetail.Rows[i]["Container#"]) != string.Empty)))
					{
						if (Conversion.ToString(dtDetail.Rows[i]["ContainerId"]) == 0.ToString())
						{
							dtContainer.Rows.Add(dtDetail.Rows[i]["Container#"], dtDetail.Rows[i]["Container#"], "");
						}
						else
						{
							dtContainer.Rows.Add(Conversion.ToString(dtDetail.Rows[i]["ContainerId"]), dtDetail.Rows[i]["Container#"], dtDetail.Rows[i]["Seal#"]);
						}
					}
				}
				DDL.BindDDL(dtContainer, cmbcontainerNo, "Id", "Container", "Container No", ZeroIndex: false);
			}
			cmbItem.Value = Conversion.ToInt(item.Cells["ItemId"].Value);
			cmbItem_Leave(null, null);
			cmbPackType.Value = item.Cells["PackTypeId"].Value;
			txtItemDesc.Text = item.Cells["ItemDescription"].Value.ToString();
			txtQtyMaster.Text = item.Cells["MasterQTY"].Value.ToString();
			CmbPackUomMaster.Value = Conversion.ToInt(item.Cells["MasterUOMId"].Value);
			txtEbUnitMaster.Text = item.Cells["EbUnitMaster"].Value.ToString();
			txtEbTotalMaster.Text = item.Cells["EbTotalMaster"].Value.ToString();
			txtQtyInner.Text = item.Cells["InnerQTY"].Value.ToString();
			CmbPackUomInner.Value = Conversion.ToInt(item.Cells["InnerUOMId"].Value);
			txtEbUnitInner.Text = item.Cells["EbUnitInner"].Value.ToString();
			txtEbTotalInner.Text = item.Cells["EbTotalInner"].Value.ToString();
			txtAddLess.Text = item.Cells["AddLess"].Value.ToString();
			txtNetWeight.Text = item.Cells["NetWeight"].Value.ToString();
			txtGrossWeight.Text = item.Cells["GrossWeight"].Value.ToString();
			txtStockWeight.Text = item.Cells["StockWeight"].Value.ToString();
			CmbWareHouse.Value = item.Cells["WarehouseId"].Value.ToString();
			((Control)(object)CmbWareHouse).Text = item.Cells["Warehouse"].Text.ToString();
			if (Conversion.ToInt(item.Cells["ContainerId"].Value) > 0)
			{
				cmbcontainerNo.Value = Conversion.ToInt(item.Cells["ContainerId"].Value);
			}
			else if (Conversion.ToString(item.Cells["Container#"].Value) != string.Empty)
			{
				cmbcontainerNo.Value = Conversion.ToString(item.Cells["Container#"].Value);
			}
			if (Conversion.ToBool(item.Cells["FlagToUpdateContainer"].Value))
			{
				((Control)(object)cmbcontainerNo).Enabled = false;
			}
			else
			{
				((Control)(object)cmbcontainerNo).Enabled = true;
			}
			txtSeal.Text = item.Cells["Seal#"].Value.ToString();
			CmbJobLot.Value = item.Cells["JobLotId"].Value.ToString();
			CmbCropYear.Value = item.Cells["CropYearId"].Value.ToString();
			btnDetailUpdate.Visible = true;
			btnDetailCancel.Visible = true;
			btnAddinGrid.Visible = false;
			cmbItem.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDetailUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (formdetailvalidation())
			{
				dtDetail.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(cmbItem.Value);
				dtDetail.Rows[updateDetailIndex]["ItemName"] = Conversion.ToString((object)((Control)(object)cmbItem).Text);
				dtDetail.Rows[updateDetailIndex]["PackTypeId"] = Conversion.ToInt(cmbPackType.Value);
				dtDetail.Rows[updateDetailIndex]["Packtype"] = Conversion.ToString((object)((Control)(object)cmbPackType).Text);
				dtDetail.Rows[updateDetailIndex]["ItemDescription"] = Conversion.ToString((object)txtItemDesc.Text);
				dtDetail.Rows[updateDetailIndex]["MasterQTY"] = Conversion.ToDouble((object)txtQtyMaster.Text);
				dtDetail.Rows[updateDetailIndex]["MasterUOMId"] = Conversion.ToInt(CmbPackUomMaster.Value);
				dtDetail.Rows[updateDetailIndex]["MasterUOM"] = Conversion.ToString((object)((Control)(object)CmbPackUomMaster).Text);
				dtDetail.Rows[updateDetailIndex]["MasterEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)CmbPackUomMaster).SelectedRow.Cells[2].Value);
				dtDetail.Rows[updateDetailIndex]["EbUnitMaster"] = Conversion.ToDouble((object)txtEbUnitMaster.Text);
				dtDetail.Rows[updateDetailIndex]["EbTotalMaster"] = Conversion.ToDouble((object)txtEbTotalMaster.Text);
				dtDetail.Rows[updateDetailIndex]["InnerQTY"] = Conversion.ToDouble((object)txtQtyInner.Text);
				dtDetail.Rows[updateDetailIndex]["InnerUOMId"] = Conversion.ToInt(CmbPackUomInner.Value);
				dtDetail.Rows[updateDetailIndex]["InnerUOM"] = Conversion.ToString((object)((Control)(object)CmbPackUomInner).Text);
				dtDetail.Rows[updateDetailIndex]["InnerEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)CmbPackUomInner).SelectedRow.Cells[2].Value);
				dtDetail.Rows[updateDetailIndex]["EbUnitInner"] = Conversion.ToDouble((object)txtEbUnitInner.Text);
				dtDetail.Rows[updateDetailIndex]["EbTotalInner"] = Conversion.ToDouble((object)txtEbTotalInner.Text);
				dtDetail.Rows[updateDetailIndex]["AddLess"] = Conversion.ToDouble((object)txtAddLess.Text);
				dtDetail.Rows[updateDetailIndex]["NetWeight"] = Conversion.ToDouble((object)txtNetWeight.Text);
				dtDetail.Rows[updateDetailIndex]["GrossWeight"] = Conversion.ToDouble((object)txtGrossWeight.Text);
				dtDetail.Rows[updateDetailIndex]["StockWeight"] = Math.Round(Conversion.ToDouble((object)txtStockWeight.Text), 2);
				dtDetail.Rows[updateDetailIndex]["WarehouseId"] = Conversion.ToInt(CmbWareHouse.Value);
				dtDetail.Rows[updateDetailIndex]["Warehouse"] = Conversion.ToString((object)((Control)(object)CmbWareHouse).Text);
				dtDetail.Rows[updateDetailIndex]["JobLotId"] = Conversion.ToInt(CmbJobLot.Value);
				dtDetail.Rows[updateDetailIndex]["JobLot"] = Conversion.ToString((object)((Control)(object)CmbJobLot).Text);
				dtDetail.Rows[updateDetailIndex]["CropYearId"] = Conversion.ToInt(CmbCropYear.Value);
				dtDetail.Rows[updateDetailIndex]["CropYear"] = Conversion.ToString((object)((Control)(object)CmbCropYear).Text);
				dtDetail.Rows[updateDetailIndex]["Container#"] = Conversion.ToString((object)((Control)(object)cmbcontainerNo).Text);
				dtDetail.Rows[updateDetailIndex]["Seal#"] = Conversion.ToString((object)txtSeal.Text);
				dtDetail.Rows[updateDetailIndex]["ContainerId"] = Conversion.ToString(cmbcontainerNo.Value);
				grdDetail.DataSource = dtDetail;
				grdDetail.RetrieveStructure();
				GridDetailSetting();
				btnDetailUpdate.Visible = false;
				btnDetailCancel.Visible = false;
				btnAddinGrid.Visible = false;
				FormDetailReset();
				cmbItem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					if (updateDetailIndex != -1)
					{
						throw new Exception("Reset the Detail First...");
					}
					if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
					{
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						ExImForwardingPackingDetail d = new ExImForwardingPackingDetail();
						d.Id = Conversion.ToInt(r.Cells["Id"].Value);
						d.ExImLcOrderId = Conversion.ToInt(r.Cells["ContractId"].Value);
						d.ContractDetailId = Conversion.ToInt(r.Cells["ContractDetailId"].Value);
						d.InvoiceDetailId = Conversion.ToInt(r.Cells["InvoiceDetailId"].Value);
						d.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						d.PackingMaterialId = Conversion.ToInt(r.Cells["PackTypeId"].Value);
						d.RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
						d.RefDocIdNo = Conversion.ToInt(r.Cells["RefDocIdNo"].Value);
						d.RefDocSubIdNo = Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value);
						d.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
						d.UOMScheduleIdOuter = Conversion.ToInt(r.Cells["MasterUOMId"].Value);
						d.EbUnit = Conversion.ToDouble(r.Cells["EbUnitMaster"].Value);
						d.EbTotal = Conversion.ToDouble(r.Cells["EbTotalMaster"].Value);
						d.InnerQty = Conversion.ToDouble(r.Cells["InnerQTY"].Value);
						d.UOMScheduleIdInner = Conversion.ToInt(r.Cells["InnerUOMId"].Value);
						d.InnerEbUnit = Conversion.ToDouble(r.Cells["EbUnitInner"].Value);
						d.InnerEbTotal = Conversion.ToDouble(r.Cells["EbTotalInner"].Value);
						d.TotalPackingWeight = Conversion.ToDouble((object)(d.InnerEbTotal + d.EbTotal));
						d.AdLsWeight = Conversion.ToDouble((object)r.Cells["AddLess"].Text.Trim());
						d.NetWeight = Conversion.ToDouble((object)r.Cells["NetWeight"].Text.Trim());
						d.GrossWeight = Conversion.ToDouble((object)r.Cells["GrossWeight"].Text.Trim());
						d.StockWeight = Conversion.ToDouble((object)r.Cells["StockWeight"].Text.Trim());
						d.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
						d.ContainerNo = Conversion.ToString((object)r.Cells["Container#"].Text.Trim());
						d.ContainerId = Conversion.ToInt(r.Cells["ContainerId"].Value);
						d.DoDetailId = Conversion.ToInt(r.Cells["DoDetailId"].Value);
						d.SealNo = Conversion.ToString((object)r.Cells["Seal#"].Text.Trim());
						d.LotJobId = Conversion.ToInt(r.Cells["JobLotId"].Value);
						d.ItemDescription = Conversion.ToString((object)r.Cells["ItemDescription"].Text.Trim());
						d.ActionTypeId = 3;
						LstRemoveRecordDetail.Add(d);
						dtDetail.Rows[r.RowIndex].Delete();
						r.Delete();
					}
					else
					{
						r.Delete();
					}
				}
				if (e.Column.Key == "Add")
				{
					DataRow dr = dtDetail.Rows[r.RowIndex];
					dtDetail.Rows.Add(dr.ItemArray);
					dtDetail.Rows[dtDetail.Rows.Count - 1]["Id"] = 0;
					grdDetail.DataSource = dtDetail;
				}
				if (e.Column.Key == "Loader")
				{
					int Index = Conversion.ToInt((object)r.RowIndex);
					int BeforeLoadGridRowCount = grdDetail.GetRows().Count();
					if (updateDetailIndex > -1)
					{
						throw new Exception("Reset Detail first...");
					}
					if (Conversion.ToInt(r.Cells["ItemId"].Value) == 0)
					{
						throw new Exception("Item Id not found");
					}
					if (Conversion.ToInt(r.Cells["WarehouseId"].Value) == 0)
					{
						throw new Exception("WareHouseId not found");
					}
					if (string.IsNullOrEmpty(Conversion.ToString(r.Cells["CropYear"].Value)))
					{
						throw new Exception("CropYear not found");
					}
					if (Conversion.ToInt(r.Cells["JobLotId"].Value) == 0)
					{
						throw new Exception("JobLot Id not found");
					}
					if (Conversion.ToInt(r.Cells["PackTypeId"].Value) == 0)
					{
						throw new Exception("PackType Id not found");
					}
					if (Conversion.ToInt(r.Cells["MasterUOMId"].Value) == 0)
					{
						throw new Exception("Master UOM Id not found");
					}
					LoadTransactions = new LoadavailableTransactionsForGDN(UserAccount);
					LoadTransactions.FromDate = clsGlobalVariables.ActiveYr.Start_Period;
					LoadTransactions.Todate = DocDate.Value;
					LoadTransactions.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					LoadTransactions.PackingTypeId = Conversion.ToInt(r.Cells["PackTypeId"].Value);
					LoadTransactions.ItemUomId = Conversion.ToInt(r.Cells["MasterUOMId"].Value);
					LoadTransactions.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					LoadTransactions.CropYear = Conversion.ToString((object)r.Cells["CropYear"].Text);
					LoadTransactions.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					LoadTransactions.RequestFrom = "Forwarding";
					LoadTransactions.ShowDialog();
					LoadTransactions.BringToFront();
					LoadInGridDetail(r);
					if (grdDetail.GetRows().Count() > BeforeLoadGridRowCount)
					{
						dtDetail.Rows[Index].Delete();
						r.Delete();
					}
				}
			}
			grdDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetail(GridEXRow r)
	{
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
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocumentTypeId"]) == Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) && Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocIdNo"]) == Conversion.ToInt(item.Cells["RefDocIdNo"].Value) && Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocSubIdNo"]) == Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					dtDetail.Rows.Add((i == 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0, Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocumentTypeId"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocIdNo"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocSubIdNo"]), Conversion.ToInt(r.Cells["InvoiceDetailId"].Value), Conversion.ToInt(r.Cells["ContractId"].Value), Conversion.ToInt(r.Cells["ContractDetailId"].Value), Conversion.ToString(r.Cells["Contract"].Value), Conversion.ToInt(r.Cells["ItemId"].Value), Conversion.ToString(r.Cells["ItemName"].Value), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["InvPackingTypeId"]), Conversion.ToString(dtpurchaseinvoice.Rows[i]["PackingType"]), Conversion.ToString(r.Cells["ItemDescription"].Value), Conversion.ToDouble(r.Cells["MasterQTY"].Value), Conversion.ToInt(r.Cells["MasterUOMId"].Value), Conversion.ToString(r.Cells["MasterUOM"].Value), Conversion.ToDouble(r.Cells["MasterEquivalent"].Value), Conversion.ToDouble(r.Cells["EbUnitMaster"].Value), Conversion.ToDouble(r.Cells["EbTotalMaster"].Value), Conversion.ToInt(r.Cells["InnerQTY"].Value), Conversion.ToInt(r.Cells["InnerUOMId"].Value), Conversion.ToString(r.Cells["InnerUOM"].Value), Conversion.ToDouble(r.Cells["InnerEquivalent"].Value), Conversion.ToDouble(r.Cells["EbUnitInner"].Value), Conversion.ToDouble(r.Cells["EbTotalInner"].Value), Conversion.ToDouble(r.Cells["AddLess"].Value), Conversion.ToDouble(r.Cells["NetWeight"].Value), Conversion.ToDouble(r.Cells["GrossWeight"].Value), Conversion.ToDouble(r.Cells["StockWeight"].Value), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["WarehouseId"]), Conversion.ToString(dtpurchaseinvoice.Rows[i]["WareHouseCode"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["JobLotId"]), Conversion.ToString(dtpurchaseinvoice.Rows[i]["JobLotCode"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["CropYearId"]), Conversion.ToString(dtpurchaseinvoice.Rows[i]["CropBatch"]), Conversion.ToString(r.Cells["Container#"].Value), Conversion.ToString(r.Cells["Seal#"].Value), Conversion.ToString(r.Cells["ContainerId"].Value), Conversion.ToString(dtpurchaseinvoice.Rows[i]["RefDocumentType"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["DocCodeNo"]), Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["QtyBalance"]), Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["WeightBalance"]), Conversion.ToInt(r.Cells["DoDetailId"].Value), Conversion.ToInt(r.Cells["FlagToUpdateContainer"].Value), Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["WeightBalance"]));
				}
				grdDetail.DataSource = dtDetail;
				grdDetail.RetrieveStructure();
				GridDetailSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdDetail.CurrentRow;
			if (row == null || (int)row.RowType != 0 || !(e.Column.Key == "AvailableStock") || !(Conversion.ToDouble(row.Cells["AvailableStock"].Value) > 0.0))
			{
				return;
			}
			List<tblUserRights> newList = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "frmStockReport").ToList();
			if (newList.Count > 0)
			{
				if (Conversion.ToBool((object)newList[0].Value.ToString()))
				{
					frmStockReport frmStockReport = new frmStockReport(UserAccount);
					frmStockReport.RequestedbyOtherForm = true;
					frmStockReport.DateTo = DocDate.Value;
					frmStockReport.ItemId = Conversion.ToInt(row.Cells["ItemId"].Value);
					frmStockReport.WarehouseId = Conversion.ToInt(row.Cells["WareHouseId"].Value);
					frmStockReport.CropYear = Conversion.ToString((object)row.Cells["CropYear"].Text);
					frmStockReport.JobLotId = Conversion.ToInt(row.Cells["JobLotId"].Value);
					frmStockReport.PackingTypeId = Conversion.ToInt(row.Cells["PackTypeId"].Value);
					frmStockReport.UomId = Conversion.ToInt(row.Cells["MasterUOMId"].Value);
					frmStockReport.ShowDialog();
				}
				else
				{
					MessageBox.Show("You dont have right");
				}
			}
			else
			{
				MessageBox.Show("You dont have right");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailSetting()
	{
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Expected O, but got Unknown
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grdDetail.RootTable.Columns["RefDocIdNo"].Visible = false;
			grdDetail.RootTable.Columns["RefDocSubIdNo"].Visible = false;
			grdDetail.RootTable.Columns["InvoiceDetailId"].Visible = false;
			grdDetail.RootTable.Columns["ContractId"].Visible = false;
			grdDetail.RootTable.Columns["ContractDetailId"].Visible = false;
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["PackTypeId"].Visible = false;
			grdDetail.RootTable.Columns["MasterUOMId"].Visible = false;
			grdDetail.RootTable.Columns["InnerUOMId"].Visible = false;
			grdDetail.RootTable.Columns["JobLotId"].Visible = false;
			grdDetail.RootTable.Columns["WarehouseId"].Visible = false;
			grdDetail.RootTable.Columns["CropYearId"].Visible = false;
			grdDetail.RootTable.Columns["InnerEquivalent"].Visible = false;
			grdDetail.RootTable.Columns["MasterEquivalent"].Visible = false;
			grdDetail.RootTable.Columns["ContainerId"].Visible = false;
			grdDetail.RootTable.Columns["BalQty"].Visible = false;
			grdDetail.RootTable.Columns["BalWeight"].Visible = false;
			grdDetail.RootTable.Columns["DoDetailId"].Visible = false;
			grdDetail.RootTable.Columns["FlagToUpdateContainer"].Visible = false;
			grdDetail.RootTable.Columns["AvailableStock"].ColumnType = (ColumnType)5;
			grdDetail.RootTable.HeaderLines = 3;
			foreach (GridEXColumn item in (JanusCollectionBase)grdDetail.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
				CommonServices.GridColumnSettings(item);
				item.EditType = (EditType)0;
				item.FilterEditType = (FilterEditType)1;
			}
			grdDetail.RootTable.Columns["Contract"].Width = Constants.InventoryConstants.ContractNoConstant;
			grdDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdDetail.RootTable.Columns["Packtype"].Width = Constants.InventoryConstants.PackingType;
			grdDetail.RootTable.Columns["ItemDescription"].Width = Constants.InventoryConstants.OtherDescription;
			grdDetail.RootTable.Columns["MasterQTY"].Width = Constants.InventoryConstants.QtyConstant;
			grdDetail.RootTable.Columns["MasterUOM"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["MasterEquivalent"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["EbUnitMaster"].Width = Constants.InventoryConstants.EbWt;
			grdDetail.RootTable.Columns["EbTotalMaster"].Width = Constants.InventoryConstants.EbWt;
			grdDetail.RootTable.Columns["InnerQTY"].Width = Constants.InventoryConstants.QtyConstant;
			grdDetail.RootTable.Columns["InnerUOM"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["InnerEquivalent"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["EbUnitInner"].Width = Constants.InventoryConstants.EbWt;
			grdDetail.RootTable.Columns["EbTotalInner"].Width = Constants.InventoryConstants.EbWt;
			grdDetail.RootTable.Columns["AddLess"].Width = Constants.InventoryConstants.EbWt;
			grdDetail.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["GrossWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["StockWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["Warehouse"].Width = Constants.InventoryConstants.WareHouseName;
			grdDetail.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			grdDetail.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			grdDetail.RootTable.Columns["Container#"].Width = Constants.InventoryConstants.ContainerConstant;
			grdDetail.RootTable.Columns["Seal#"].Width = Constants.InventoryConstants.SealNo;
			grdDetail.RootTable.Columns["RefDocType"].Width = Constants.InventoryConstants.DocumentType;
			grdDetail.RootTable.Columns["RefDocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdDetail.RootTable.Columns["BalQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdDetail.RootTable.Columns["BalWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["NetWeight"].FormatString = "#,#.####";
			grdDetail.RootTable.Columns["GrossWeight"].FormatString = "#,#.####";
			grdDetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,#.####";
			grdDetail.RootTable.Columns["GrossWeight"].TotalFormatString = "#,#.####";
			grdDetail.RootTable.Columns["AvailableStock"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns.Add("Add");
			grdDetail.RootTable.Columns["Add"].Key = "Add";
			grdDetail.RootTable.Columns["Add"].Caption = "+";
			grdDetail.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Add"].ButtonText = "+";
			grdDetail.RootTable.Columns["Add"].Width = 20;
			grdDetail.RootTable.Columns["Add"].Position = 0;
			grdDetail.RootTable.Columns.Add("Delete");
			grdDetail.RootTable.Columns["Delete"].Key = "Delete";
			grdDetail.RootTable.Columns["Delete"].Caption = "X";
			grdDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdDetail.RootTable.Columns["Delete"].Width = 20;
			grdDetail.RootTable.Columns["Delete"].Position = 1;
			if (StockLoaderVisibleInForwardingDetail)
			{
				grdDetail.RootTable.Columns["RefDocType"].Visible = true;
				grdDetail.RootTable.Columns["RefDocNo"].Visible = true;
				grdDetail.RootTable.Columns["BalQty"].Visible = true;
				grdDetail.RootTable.Columns["BalWeight"].Visible = true;
				grdDetail.RootTable.Columns.Add("Loader");
				grdDetail.RootTable.Columns["Loader"].Key = "Loader";
				grdDetail.RootTable.Columns["Loader"].Caption = "Loader";
				grdDetail.RootTable.Columns["Loader"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdDetail.RootTable.Columns["Loader"].ButtonStyle = (ButtonStyle)5;
				grdDetail.RootTable.Columns["Loader"].ButtonText = "Loader";
				grdDetail.RootTable.Columns["Loader"].Width = 60;
				grdDetail.RootTable.Columns["Loader"].Position = 2;
				grdDetail.FrozenColumns = 3;
			}
			else
			{
				grdDetail.FrozenColumns = 2;
				grdDetail.RootTable.Columns["RefDocType"].Visible = false;
				grdDetail.RootTable.Columns["RefDocNo"].Visible = false;
				grdDetail.RootTable.Columns["BalQty"].Visible = false;
				grdDetail.RootTable.Columns["BalWeight"].Visible = false;
			}
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDetailCancel_Click(object sender, EventArgs e)
	{
		try
		{
			FormDetailReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsOtherItems()
	{
		try
		{
			grdotheritems.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdotheritems.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdotheritems.RootTable.Columns["Qty"].FormatString = "0,0";
			grdotheritems.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grdotheritems.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdotheritems.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdotheritems.RootTable.Columns["Amount"].FormatString = "0,0";
			grdotheritems.RootTable.Columns["Amount"].TotalFormatString = "#,##0.##";
			grdotheritems.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdotheritems.RootTable.Columns["Rate"].FormatString = "0,0";
			grdotheritems.RootTable.Columns["ItemId"].Visible = false;
			grdotheritems.RootTable.Columns["ItemName"].Width = 350;
			grdotheritems.RootTable.Columns["Remarks"].Width = 350;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnoUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (OtherItemFormValidation())
			{
				dtOtherItem.Rows[updateDetailIndexOther]["ItemId"] = CmbOtherItem.Value;
				dtOtherItem.Rows[updateDetailIndexOther]["ItemName"] = ((Control)(object)CmbOtherItem).Text;
				dtOtherItem.Rows[updateDetailIndexOther]["Qty"] = txtoQty.Text;
				dtOtherItem.Rows[updateDetailIndexOther]["Rate"] = txtoRate.Text;
				dtOtherItem.Rows[updateDetailIndexOther]["Amount"] = txtoAmount.Text;
				dtOtherItem.Rows[updateDetailIndexOther]["ContainerNo"] = txtOContainer.Text;
				dtOtherItem.Rows[updateDetailIndexOther]["Remarks"] = txtoRemarks.Text;
				grdotheritems.DataSource = dtOtherItem;
				btnoadd.Visible = true;
				btnoUpdate.Visible = false;
				btnoCancel.Visible = false;
				ResetOtherItems();
				txtoQty.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdotheritems_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdotheritems.CurrentRow;
			if (item != null)
			{
				updateDetailIndexOther = item.RowIndex;
				CmbOtherItem.Value = item.Cells["ItemId"].Value.ToString();
				txtoQty.Text = item.Cells["Qty"].Value.ToString();
				txtoRate.Text = item.Cells["Rate"].Value.ToString();
				txtoAmount.Text = item.Cells["Amount"].Value.ToString();
				txtOContainer.Text = Conversion.ToString(item.Cells["ContainerNo"].Value);
				txtoRemarks.Text = item.Cells["Remarks"].Value.ToString();
				btnoadd.Visible = false;
				btnoUpdate.Visible = true;
				btnoCancel.Visible = true;
				txtoQty.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnoCancel_Click(object sender, EventArgs e)
	{
		try
		{
			btnoadd.Visible = true;
			btnoUpdate.Visible = false;
			btnoCancel.Visible = false;
			ResetOtherItems();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
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
			grdHistoryDetail.DataSource = null;
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

	private void HistoryCombosFill()
	{
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		try
		{
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("Customer", typeof(string));
			DataTable dtGpNo = new DataTable();
			dtGpNo.Columns.Add("Id", typeof(int));
			dtGpNo.Columns.Add("GpNo", typeof(int));
			DataTable dtInvoiceNo = new DataTable();
			dtInvoiceNo.Columns.Add("Id", typeof(int));
			dtInvoiceNo.Columns.Add("InvoiceNo", typeof(string));
			DataTable dt = ExImForwarding.GetDataForDropDownFromGoodsForwarding(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Customer")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "GpNo")
					{
						dtGpNo.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Invoice")
					{
						dtInvoiceNo.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtCustomer, CmbCustomerHistory, "Id", "Customer", "Party", ZeroIndex: false);
			DDL.BindDDL(dtGpNo, CmbGpNoHistory, "Id", "GpNo", "Gp No", ZeroIndex: false);
			((UltraGridBase)CmbGpNoHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			DDL.BindDDL(dtInvoiceNo, CmbInvoiceNoHistory, "Id", "InvoiceNo", "Invoice No", ZeroIndex: false);
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
			GridBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters order = new ReportsParameters();
			order.OrganizationId = UserAccount.OrganizationId;
			order.CompanyId = UserAccount.CompanyId;
			order.BranchesId = UserAccount.BranchesId;
			order.CanViewAllRecord = true;
			order.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					order.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					order.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					order.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					order.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					order.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					order.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					order.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					order.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			order.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			order.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			order.GpId = Conversion.ToInt(CmbGpNoHistory.Value);
			order.InvoiceId = Conversion.ToInt(CmbInvoiceNoHistory.Value);
			order.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			DataTable dtHistory = ExImForwarding.FormHistory(order);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("GpNo", typeof(int));
				dt.Columns.Add("GPDate", typeof(DateTime));
				dt.Columns.Add("InvoiceNo", typeof(string));
				dt.Columns.Add("ExportVoucherNo", typeof(string));
				dt.Columns.Add("CustomerName", typeof(string));
				dt.Columns.Add("ShippingLine", typeof(string));
				dt.Columns.Add("DriverName", typeof(string));
				dt.Columns.Add("DriverCellNo", typeof(string));
				dt.Columns.Add("DriverCnicNo", typeof(string));
				dt.Columns.Add("Transporter", typeof(string));
				dt.Columns.Add("VehicleNo", typeof(string));
				dt.Columns.Add("BiltyNo", typeof(string));
				dt.Columns.Add("Freight", typeof(double));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ApprovedStatus", typeof(string));
				dt.Columns.Add("ApprovedUser", typeof(string));
				dt.Columns.Add("ApprovedDate", typeof(DateTime));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					dt.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["DocNo"], Conversion.ToDateTime(dtHistory.Rows[i]["DocDate"]).ToShortDateString(), dtHistory.Rows[i]["GpNo"], Conversion.ToDateTime(dtHistory.Rows[i]["GpDate"]).ToShortDateString(), dtHistory.Rows[i]["InvoiceNo"], dtHistory.Rows[i]["ExportVoucherNo"], dtHistory.Rows[i]["CustomerName"], dtHistory.Rows[i]["ShippingLine"], dtHistory.Rows[i]["DriverName"], dtHistory.Rows[i]["DriverCellNo"], dtHistory.Rows[i]["DriverCnicNo"], dtHistory.Rows[i]["TransporterName"], dtHistory.Rows[i]["VehicleNo"], dtHistory.Rows[i]["BiltyNo"], dtHistory.Rows[i]["FreightAmt"], dtHistory.Rows[i]["EntryUserName"], dtHistory.Rows[i]["EntryDate"], dtHistory.Rows[i]["ModifyUserName"], dtHistory.Rows[i]["ModifyDate"], dtHistory.Rows[i]["ApprovedStatus"], dtHistory.Rows[i]["ApprovedUserName"], dtHistory.Rows[i]["ApprovedDate"], dtHistory.Rows[i]["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				DataGridHistory.ClearStructure();
				grdHistoryDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSettings()
	{
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["Freight"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["Freight"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)DataGridHistory.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			DataGridHistory.RootTable.Columns["DocNo"].Width = Constants.ExportConstants.DocNoConstant;
			DataGridHistory.RootTable.Columns["DocDate"].Width = Constants.ExportConstants.DateConstant;
			DataGridHistory.RootTable.Columns["GpNo"].Width = Constants.ExportConstants.GpNoConstant;
			DataGridHistory.RootTable.Columns["GPDate"].Width = Constants.ExportConstants.DateConstant;
			DataGridHistory.RootTable.Columns["InvoiceNo"].Width = Constants.ExportConstants.InvoiceNoConstant;
			DataGridHistory.RootTable.Columns["ExportVoucherNo"].Width = Constants.ExportConstants.ExportVoucherNo;
			DataGridHistory.RootTable.Columns["CustomerName"].Width = Constants.ExportConstants.StringConstant;
			DataGridHistory.RootTable.Columns["ShippingLine"].Width = Constants.ExportConstants.StringConstant;
			DataGridHistory.RootTable.Columns["DriverName"].Width = Constants.ExportConstants.StringConstant;
			DataGridHistory.RootTable.Columns["DriverCellNo"].Width = Constants.ExportConstants.DriverCellNoConstant;
			DataGridHistory.RootTable.Columns["DriverCnicNo"].Width = Constants.ExportConstants.DriverCnicNoConstant;
			DataGridHistory.RootTable.Columns["Transporter"].Width = Constants.ExportConstants.StringConstant;
			DataGridHistory.RootTable.Columns["VehicleNo"].Width = Constants.ExportConstants.VehicleNoConstant;
			DataGridHistory.RootTable.Columns["BiltyNo"].Width = Constants.ExportConstants.BiltyNoConstant;
			DataGridHistory.RootTable.Columns["Freight"].Width = Constants.ExportConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["EntryUser"].Width = Constants.ExportConstants.EntryUserConstants;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = Constants.ExportConstants.DateTimeConstantddMMM;
			DataGridHistory.RootTable.Columns["ModifyUser"].Width = Constants.ExportConstants.EntryUserConstants;
			DataGridHistory.RootTable.Columns["ModifyDate"].Width = Constants.ExportConstants.DateTimeConstantddMMM;
			DataGridHistory.RootTable.Columns["ApprovedStatus"].Width = Constants.ExportConstants.IsApproved;
			DataGridHistory.RootTable.Columns["ApprovedUser"].Width = Constants.ExportConstants.ApprovedByConstant;
			DataGridHistory.RootTable.Columns["ApprovedDate"].Width = Constants.ExportConstants.DateTimeConstantddMMM;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.ExportConstants.NoOfAttachmentsConstants;
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			DataGridHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yy hh:mm tt";
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 40;
			DataGridHistory.RootTable.Columns["Edit"].Position = 0;
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 45;
			DataGridHistory.RootTable.Columns["Print"].Position = 1;
			DataGridHistory.FrozenColumns = 2;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				FormReset();
				if (Conversion.ToInt(r.Cells["ExportVoucherNo"].Value) > 0)
				{
					ReferredInExportVoucher = true;
				}
				RecId = Conversion.ToInt(r.Cells["Id"].Value);
				ReadById(RecId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), 205);
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
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (e.Column.Key == "Edit")
			{
				if (!formRights.DoHaveUpdateRights)
				{
					throw new Exception("You don't have right to Update");
				}
				FormReset();
				if (Conversion.ToInt(r.Cells["ExportVoucherNo"].Value) > 0)
				{
					ReferredInExportVoucher = true;
				}
				RecId = Conversion.ToInt(r.Cells["Id"].Value);
				ReadById(RecId);
			}
			if (e.Column.Key == "Print")
			{
				if (!formRights.DoHavePrintRights)
				{
					throw new Exception("You don't have right to print");
				}
				GeneratePrint(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			GetDetailByHeaderId();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GetDetailByHeaderId()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			int num = Conversion.ToInt(r.Cells["Id"].Value);
			ExImForwarding exImLc = new ExImForwarding();
			exImLc = ExImForwarding.GetByID(num);
			if (exImLc != null && exImLc.ExImForwardingDetail.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("InvoiceDetailId", typeof(int));
				dt.Columns.Add("ContractId", typeof(int));
				dt.Columns.Add("ContractDetailId", typeof(int));
				dt.Columns.Add("Contract");
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("ItemName");
				dt.Columns.Add("PackingType");
				dt.Columns.Add("ItemDescription");
				dt.Columns.Add("MasterQTY", typeof(double));
				dt.Columns.Add("MasterUOM");
				dt.Columns.Add("EbUnitMaster", typeof(double));
				dt.Columns.Add("EbTotalMaster", typeof(double));
				dt.Columns.Add("InnerQTY", typeof(double));
				dt.Columns.Add("InnerUOM");
				dt.Columns.Add("EbUnitInner", typeof(double));
				dt.Columns.Add("EbTotalInner", typeof(double));
				dt.Columns.Add("AddLess", typeof(double));
				dt.Columns.Add("NetWeight", typeof(double));
				dt.Columns.Add("GrossWeight", typeof(double));
				dt.Columns.Add("StockWeight", typeof(double));
				dt.Columns.Add("Warehouse");
				dt.Columns.Add("JobLot");
				dt.Columns.Add("CropYear");
				dt.Columns.Add("Container#");
				dt.Columns.Add("Seal#");
				for (int i = 0; i < exImLc.ExImForwardingDetail.Count; i++)
				{
					dt.Rows.Add(exImLc.ExImForwardingDetail[i].Id, exImLc.ExImForwardingDetail[i].InvoiceDetailId, exImLc.ExImForwardingDetail[i].ExImLcOrderId, exImLc.ExImForwardingDetail[i].ContractDetailId, exImLc.ExImForwardingDetail[i].LcOrderNo, exImLc.ExImForwardingDetail[i].ItemId, exImLc.ExImForwardingDetail[i].ItemName, exImLc.ExImForwardingDetail[i].Packtype, exImLc.ExImForwardingDetail[i].ItemDescription, exImLc.ExImForwardingDetail[i].OuterQty, exImLc.ExImForwardingDetail[i].OuterUOM, exImLc.ExImForwardingDetail[i].EbUnit, exImLc.ExImForwardingDetail[i].EbTotal, exImLc.ExImForwardingDetail[i].InnerQty, exImLc.ExImForwardingDetail[i].InnerUOM, exImLc.ExImForwardingDetail[i].InnerEbUnit, exImLc.ExImForwardingDetail[i].InnerEbTotal, exImLc.ExImForwardingDetail[i].AdLsWeight, exImLc.ExImForwardingDetail[i].NetWeight, exImLc.ExImForwardingDetail[i].GrossWeight, exImLc.ExImForwardingDetail[i].StockWeight, exImLc.ExImForwardingDetail[i].WareHouseName, exImLc.ExImForwardingDetail[i].JobLotName, exImLc.ExImForwardingDetail[i].CropYear, exImLc.ExImForwardingDetail[i].ContainerNo, exImLc.ExImForwardingDetail[i].SealNo);
				}
				grdHistoryDetail.DataSource = dt;
				grdHistoryDetail.RetrieveStructure();
				GridHistoryDetailSetting();
			}
			else
			{
				grdHistoryDetail.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridHistoryDetailSetting()
	{
		try
		{
			grdHistoryDetail.RootTable.Columns["Id"].Visible = false;
			grdHistoryDetail.RootTable.Columns["InvoiceDetailId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["ContractId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["ContractDetailId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["ItemId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistoryDetail, 2, 3);
			grdHistoryDetail.RootTable.Columns["Contract"].Width = Constants.ExportConstants.ContractNoConstant;
			grdHistoryDetail.RootTable.Columns["ItemName"].Width = Constants.ExportConstants.ItemNameConstants;
			grdHistoryDetail.RootTable.Columns["PackingType"].Width = Constants.ExportConstants.PackingTypeConstant;
			grdHistoryDetail.RootTable.Columns["ItemDescription"].Width = Constants.ExportConstants.CommodityDetail;
			grdHistoryDetail.RootTable.Columns["MasterQTY"].Width = Constants.ExportConstants.QtyConstant;
			grdHistoryDetail.RootTable.Columns["MasterUOM"].Width = Constants.ExportConstants.UomConstant;
			grdHistoryDetail.RootTable.Columns["EbUnitMaster"].Width = Constants.ExportConstants.EbUnit;
			grdHistoryDetail.RootTable.Columns["EbTotalMaster"].Width = Constants.ExportConstants.EbTotal;
			grdHistoryDetail.RootTable.Columns["InnerQTY"].Width = Constants.ExportConstants.QtyConstant;
			grdHistoryDetail.RootTable.Columns["InnerUOM"].Width = Constants.ExportConstants.UomConstant;
			grdHistoryDetail.RootTable.Columns["EbUnitInner"].Width = Constants.ExportConstants.EbUnit;
			grdHistoryDetail.RootTable.Columns["EbTotalInner"].Width = Constants.ExportConstants.EbTotal;
			grdHistoryDetail.RootTable.Columns["AddLess"].Width = Constants.ExportConstants.AddLessConstant;
			grdHistoryDetail.RootTable.Columns["NetWeight"].Width = Constants.ExportConstants.WeightConstant;
			grdHistoryDetail.RootTable.Columns["GrossWeight"].Width = Constants.ExportConstants.WeightConstant;
			grdHistoryDetail.RootTable.Columns["StockWeight"].Width = Constants.ExportConstants.WeightConstant;
			grdHistoryDetail.RootTable.Columns["Warehouse"].Width = Constants.InventoryConstants.WareHouseName;
			grdHistoryDetail.RootTable.Columns["JobLot"].Width = Constants.ExportConstants.JobLotConstant;
			grdHistoryDetail.RootTable.Columns["CropYear"].Width = Constants.ExportConstants.CropYearConstant;
			grdHistoryDetail.RootTable.Columns["Container#"].Width = Constants.ExportConstants.ContainerNo;
			grdHistoryDetail.RootTable.Columns["Seal#"].Width = Constants.ExportConstants.SealNo;
			grdHistoryDetail.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["StockWeight"].FormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["GrossWeight"].FormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["NetWeight"].FormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["MasterQTY"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["MasterQTY"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["MasterQTY"].FormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["MasterQTY"].TotalFormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["InnerQTY"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["InnerQTY"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["InnerQTY"].FormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["InnerQTY"].TotalFormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["EbUnitInner"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["EbUnitInner"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["EbUnitInner"].FormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["EbUnitInner"].TotalFormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["EbTotalInner"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["EbTotalInner"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["EbTotalInner"].FormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["EbTotalInner"].TotalFormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["EbTotalMaster"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["EbTotalMaster"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["EbTotalMaster"].FormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["EbTotalMaster"].TotalFormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["EbUnitMaster"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["EbUnitMaster"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["EbUnitMaster"].FormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["EbUnitMaster"].TotalFormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["AddLess"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["AddLess"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["AddLess"].FormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["AddLess"].TotalFormatString = "#,##0.##";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				GeneratePrint(RecId);
				return;
			}
			throw new Exception("No Record Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GeneratePrint(int printId)
	{
		try
		{
			CommonServices.ForwardingSlip507(printId);
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

	private void txtoQty_TextChanged(object sender, EventArgs e)
	{
		OtherItemsCalculations();
	}

	private void OtherItemsCalculations()
	{
		try
		{
			double Qty = Conversion.ToDouble((object)txtoQty.Text.Trim());
			double Rate = Conversion.ToDouble((object)txtoRate.Text.Trim());
			if (Qty > 0.0 && Rate > 0.0)
			{
				txtoAmount.Text = (Qty * Rate).ToString();
			}
			else
			{
				txtoAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WeightCalculation()
	{
		try
		{
			double EBTotalInner = 0.0;
			double EBUnitInner = 0.0;
			double PckUomInner = 0.0;
			double QtyInner = 0.0;
			double EBTotalMaster = 0.0;
			double EBUnitMaster = 0.0;
			double PckUomMaster = 0.0;
			double QtyMaster = 0.0;
			double GrossWt = 0.0;
			double AddLss = 0.0;
			double DoNetWt = 0.0;
			double StockWeight = 0.0;
			double FcWeight = 0.0;
			double DoTotalWt = 0.0;
			txtGrossWeight.Text = 0.ToString();
			txtStockWeight.Text = 0.ToString();
			FcWeight = Conversion.ToDouble((object)txtFactoryWeight.Text);
			DoTotalWt = Conversion.ToDouble((object)txtDoNetWeight.Text);
			DoNetWt = Conversion.ToDouble((object)txtNetWeight.Text);
			EBUnitMaster = Conversion.ToDouble((object)txtEbUnitMaster.Text);
			EBUnitInner = Conversion.ToDouble((object)txtEbUnitInner.Text);
			AddLss = Conversion.ToDouble((object)txtAddLess.Text);
			QtyMaster = Conversion.ToDouble((object)txtQtyMaster.Text);
			QtyInner = Conversion.ToDouble((object)txtQtyInner.Text);
			if (((UltraGridBase)CmbPackUomMaster).ActiveRow != null && Conversion.ToInt(CmbPackUomMaster.Value) != 0)
			{
				PckUomMaster = Conversion.ToDouble(((UltraDropDownBase)CmbPackUomMaster).SelectedRow.Cells[2].Value);
				if (EBUnitMaster > 0.0 && PckUomMaster > 0.0 && QtyMaster > 0.0)
				{
					EBTotalMaster = QtyMaster * EBUnitMaster;
					txtEbTotalMaster.Text = EBTotalMaster.ToString();
				}
				else
				{
					txtEbTotalMaster.Text = "0";
				}
			}
			if (((UltraGridBase)CmbPackUomInner).ActiveRow != null && Conversion.ToInt(CmbPackUomInner.Value) != 0)
			{
				PckUomInner = Conversion.ToDouble(((UltraDropDownBase)CmbPackUomInner).SelectedRow.Cells[2].Value);
				if (EBUnitInner > 0.0 && PckUomInner > 0.0 && QtyInner > 0.0)
				{
					EBTotalInner = QtyInner * EBUnitInner;
					txtEbTotalInner.Text = EBTotalInner.ToString();
				}
				else
				{
					txtEbTotalInner.Text = "0";
				}
			}
			if (DoNetWt > 0.0 && FcWeight > 0.0 && DoTotalWt > 0.0)
			{
				GrossWt = DoNetWt + EBTotalMaster + EBTotalInner + AddLss;
				txtGrossWeight.Text = GrossWt.ToString();
				StockWeight = FcWeight / DoTotalWt * DoNetWt - (EBTotalMaster + EBTotalInner + AddLss);
				if (!NetWeightAndStockWeightEqualOnForwarding)
				{
					txtStockWeight.Text = Math.Round(StockWeight, 3).ToString();
				}
				else
				{
					txtStockWeight.Text = txtNetWeight.Text;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void NetWeightCalculation()
	{
		try
		{
			double NetWeight = 0.0;
			double OuterUom = 0.0;
			double QtyMaster = 0.0;
			if (((UltraGridBase)CmbPackUomMaster).ActiveRow != null && Conversion.ToInt(CmbPackUomMaster.Value) != 0)
			{
				QtyMaster = Conversion.ToDouble((object)txtQtyMaster.Text);
				OuterUom = Conversion.ToDouble(((UltraDropDownBase)CmbPackUomMaster).SelectedRow.Cells[2].Value);
				if (QtyMaster > 0.0 && OuterUom > 0.0)
				{
					NetWeight = QtyMaster * OuterUom;
					txtNetWeight.Text = Conversion.ToString((object)NetWeight);
					txtStockWeight.Text = Conversion.ToString((object)NetWeight);
				}
				else
				{
					txtNetWeight.Text = "0";
					txtStockWeight.Text = "0";
				}
			}
			else
			{
				txtNetWeight.Text = "0";
				txtStockWeight.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAddLess_TextChanged(object sender, EventArgs e)
	{
		try
		{
			WeightCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQtyMaster_TextChanged(object sender, EventArgs e)
	{
		try
		{
			NetWeightCalculation();
			WeightCalculation();
			if (Conversion.ToDouble((object)txtQtyMaster.Text) != 0.0)
			{
				double Qty = Conversion.ToDouble((object)txtQtyMaster.Text);
				txtQtyInner.Text = Qty.ToString("#,##0.##");
				WeightCalculation();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPackUomMaster_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			NetWeightCalculation();
			WeightCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEbUnitMaster_TextChanged(object sender, EventArgs e)
	{
		try
		{
			WeightCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQtyInner_TextChanged(object sender, EventArgs e)
	{
		try
		{
			WeightCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPackUomInner_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			WeightCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEbUnitInner_TextChanged(object sender, EventArgs e)
	{
		try
		{
			WeightCalculation();
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

	private void ExpfrmForwarding_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			btnRecordsUpdate.Visible = false;
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.Shift && e.KeyCode == Keys.A)
			{
				btnRecordsUpdate.Visible = true;
			}
			if (e.Control && e.Alt && tabControl1.SelectedIndex == 0)
			{
				MakeShortCutKeys();
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
					((Control)(object)DataGridHistory).Focus();
				}
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S && !UpdateMode && btnSave.Enabled)
				{
					btnSave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && UpdateMode && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.Shift && e.KeyCode == Keys.Delete && UpdateMode && btnDelete.Enabled)
				{
					btnDelete_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					BtnNew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P && formRights.DoHavePrintRights)
				{
					BtnPrint_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Right && tabControl1.SelectedIndex == 0)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						tabControl2.SelectedIndex = 1;
						CmbOtherItem.Focus();
					}
					else
					{
						tabControl2.SelectedIndex = 0;
						cmbItem.Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						((Control)(object)grdDetail).Focus();
					}
					else
					{
						((Control)(object)grdotheritems).Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						cmbItem.Focus();
					}
					else
					{
						CmbOtherItem.Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					DocDate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					btnAttachment_Click(null, null);
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnShowHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnResetHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)DataGridHistory).Focused)
				{
					((Control)(object)grdHistoryDetail).Focus();
				}
				else
				{
					((Control)(object)DataGridHistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)DataGridHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				FromDateHistory.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdDetail.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdDetail_DoubleClick(null, null);
			}
			else
			{
				if (!e.Control || e.KeyCode != Keys.Space)
				{
					return;
				}
				if (grdDetail.CurrentColumn.Key == "Delete")
				{
					if (updateDetailIndex != -1)
					{
						throw new Exception("Reset the Detail First...");
					}
					if (Conversion.ToInt(item.Cells["Id"].Value) > 0)
					{
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						ExImForwardingPackingDetail d = new ExImForwardingPackingDetail();
						d.Id = Conversion.ToInt(item.Cells["Id"].Value);
						d.ExImLcOrderId = Conversion.ToInt(item.Cells["ContractId"].Value);
						d.ContractDetailId = Conversion.ToInt(item.Cells["ContractDetailId"].Value);
						d.InvoiceDetailId = Conversion.ToInt(item.Cells["InvoiceDetailId"].Value);
						d.ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
						d.PackingMaterialId = Conversion.ToInt(item.Cells["PackTypeId"].Value);
						d.NetWeight = Conversion.ToDouble((object)item.Cells["NetWeight"].Text.Trim());
						d.GrossWeight = Conversion.ToDouble((object)item.Cells["GrossWeight"].Text.Trim());
						d.UOMScheduleIdOuter = Conversion.ToInt(item.Cells["OuterUOMId"].Value);
						d.UOMScheduleIdInner = Conversion.ToInt(item.Cells["OuterUOMId"].Value);
						d.OuterQty = Conversion.ToDouble((object)item.Cells["NoOfBags"].Text.Trim());
						d.InnerQty = Conversion.ToDouble((object)item.Cells["NoOfBags"].Text.Trim());
						d.RefDocumentTypeId = Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value);
						d.RefDocIdNo = Conversion.ToInt(item.Cells["RefDocIdNo"].Value);
						d.RefDocSubIdNo = Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value);
						d.CropYearId = Conversion.ToInt(item.Cells["CropYearId"].Value);
						d.StockWeight = Conversion.ToDouble((object)item.Cells["StockWeight"].Text.Trim());
						d.EbUnit = Conversion.ToDouble((object)item.Cells["EbUnit"].Text.Trim());
						d.EbTotal = Conversion.ToDouble((object)item.Cells["EbTotal"].Text.Trim());
						d.AdLsWeight = Conversion.ToDouble((object)item.Cells["AddLess"].Text.Trim());
						d.WarehouseId = Conversion.ToInt(item.Cells["WarehouseId"].Value);
						d.ContainerNo = Conversion.ToString((object)item.Cells["Container#"].Text.Trim());
						d.ContainerNo = Conversion.ToString((object)item.Cells["Container#"].Text.Trim());
						d.SealNo = Conversion.ToString((object)item.Cells["Seal#"].Text.Trim());
						d.LotJobId = Conversion.ToInt(item.Cells["JobLotId"].Value);
						d.ItemDescription = Conversion.ToString((object)item.Cells["ItemDescription"].Text.Trim());
						d.ActionTypeId = 3;
						LstRemoveRecordDetail.Add(d);
						dtDetail.Rows[item.RowIndex].Delete();
						item.Delete();
					}
					else
					{
						item.Delete();
					}
				}
				else if (grdDetail.CurrentColumn.Key == "Add")
				{
					DataRow dr = dtDetail.Rows[item.RowIndex];
					dtDetail.Rows.Add(dr.ItemArray);
					dtDetail.Rows[dtDetail.Rows.Count - 1]["Id"] = 0;
					grdDetail.DataSource = dtDetail;
				}
				grdDetail.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdotheritems_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdotheritems.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Control && e.KeyCode == Keys.Return)
			{
				grdotheritems_DoubleClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int id = Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (!formRights.DoHaveUpdateRights)
				{
					throw new Exception("You don't have right to update");
				}
				FormReset();
				if (Conversion.ToInt(item.Cells["ExportVoucherNo"].Value) > 0)
				{
					ReferredInExportVoucher = true;
				}
				RecId = id;
				ReadById(RecId);
			}
			else
			{
				if (!e.Control || e.KeyCode != Keys.Space)
				{
					return;
				}
				if (DataGridHistory.CurrentColumn.Key == "Print")
				{
					if (!formRights.DoHavePrintRights)
					{
						throw new Exception("You don't have Right to Print");
					}
					GeneratePrint(id);
				}
				else if (DataGridHistory.CurrentColumn.Key == "Edit")
				{
					if (!formRights.DoHaveUpdateRights)
					{
						throw new Exception("You don't have Right to update");
					}
					FormReset();
					if (Conversion.ToInt(item.Cells["ExportVoucherNo"].Value) > 0)
					{
						ReferredInExportVoucher = true;
					}
					RecId = id;
					ReadById(RecId);
				}
				else if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(id, 205);
				}
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
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+L", "For Load All History Records");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Detail Entry First Colum");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grids");
			dt.Rows.Add("Ctrl+ArrowRight", "For Toggle Between Detail Tabs");
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

	private void txtQtyMaster_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtEbUnitMaster_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtQtyInner_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtEbUnitInner_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtAddLess_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunctionWithMinus(sender, e);
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
			MessageBox.Show(ex.Message);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRecordsUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			DataTable dtIds = ExImForwarding.GetForwardingIdsForAutoUpdation(UserAccount.OrganizationId, UserAccount.CompanyId, 205);
			if (dtIds != null && dtIds.Rows.Count > 0)
			{
				bool ShouldStop = false;
				for (int i = 0; i < dtIds.Rows.Count; i++)
				{
					int RecId = Conversion.ToInt(dtIds.Rows[i]["Id"]);
					ReadById(RecId, AutoUpdate: true);
					ShouldStop = AutoUpdateRecord(RecId);
					if (!ShouldStop)
					{
						break;
					}
				}
				if (ShouldStop)
				{
					MessageBox.Show("Records Update Successfully");
				}
				btnRecordsUpdate.Visible = false;
				return;
			}
			btnRecordsUpdate.Visible = false;
			throw new Exception("Ids Not Found For Update");
		}
		catch (Exception ex)
		{
			btnRecordsUpdate.Visible = false;
			MessageBox.Show(ex.Message);
		}
	}

	private void btnStockReport_Click(object sender, EventArgs e)
	{
		try
		{
			List<tblUserRights> newList = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "frmStockReport").ToList();
			if (newList.Count > 0)
			{
				if (Conversion.ToBool((object)newList[0].Value.ToString()))
				{
					new frmStockReport(UserAccount).ShowDialog();
				}
				else
				{
					MessageBox.Show("You dont have right");
				}
			}
			else
			{
				MessageBox.Show("You dont have right");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnGenerateAvailableStock_Click(object sender, EventArgs e)
	{
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		try
		{
			if (grdDetail.GetRows().Count() <= 0)
			{
				return;
			}
			bool FifoFeature = CommonServices.GetERPFeatureById(5);
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (FifoFeature)
				{
					double AvailableWeight = 0.0;
					DataTable dt = GetAvgRatesAndStockInHand.GetStockByFifoMethod(new ReportsParameters
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						ItemId = Conversion.ToInt(item.Cells["ItemId"].Value),
						DocDate = DocDate.Value,
						WarehouseId = Conversion.ToInt(item.Cells["WareHouseId"].Value),
						CropYear = Conversion.ToString((object)item.Cells["CropYear"].Text),
						JobLotId = Conversion.ToInt(item.Cells["JobLotId"].Value),
						PackingTypeId = Conversion.ToInt(item.Cells["PackTypeId"].Value),
						stockUOM = Conversion.ToInt(item.Cells["MasterUOMId"].Value)
					});
					if (dt.Rows.Count > 0)
					{
						AvailableWeight = dt.AsEnumerable().Sum((DataRow row) => row.Field<double>("BalWeight"));
					}
					item.BeginEdit();
					item.Cells["AvailableStock"].Value = AvailableWeight;
					item.EndEdit();
				}
				else
				{
					double Weight = 0.0;
					DataTable dt2 = GetAvgRatesAndStockInHand.GetAvailableStock(new ReportsParameters
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						ItemId = Conversion.ToInt(item.Cells["ItemId"].Value),
						DocDate = DocDate.Value,
						WarehouseId = Conversion.ToInt(item.Cells["WareHouseId"].Value),
						CropYear = Conversion.ToString((object)item.Cells["CropYear"].Text),
						JobLotId = Conversion.ToInt(item.Cells["JobLotId"].Value),
						PackingTypeId = Conversion.ToInt(item.Cells["PackTypeId"].Value),
						stockUOM = Conversion.ToInt(item.Cells["MasterUOMId"].Value),
						ActionId = 1
					});
					if (dt2.Rows.Count > 0)
					{
						Weight = Conversion.ToDouble(dt2.Rows[0]["BalWeight"]);
					}
					item.BeginEdit();
					item.Cells["AvailableStock"].Value = Weight;
					item.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridGatepassFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dtGridRegister = GatePassOutward.GatePassOutwardDataForExportForwarding(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (dtGridRegister.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("ExImInvoiceId", typeof(int));
				dtcol.Columns.Add("InvoiceNo", typeof(string));
				dtcol.Columns.Add("InvoiceDate", typeof(DateTime));
				dtcol.Columns.Add("DoId", typeof(int));
				dtcol.Columns.Add("DoNo", typeof(int));
				dtcol.Columns.Add("DoDate", typeof(DateTime));
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("GpDate", typeof(DateTime));
				dtcol.Columns.Add("GpSrNo", typeof(int));
				dtcol.Columns.Add("Status", typeof(string));
				dtcol.Columns.Add("VehicleType", typeof(string));
				dtcol.Columns.Add("VehicleNo", typeof(string));
				dtcol.Columns.Add("BiltyNo", typeof(string));
				dtcol.Columns.Add("Container", typeof(string));
				dtcol.Columns.Add("Container1", typeof(string));
				dtcol.Columns.Add("SupplierWeight", typeof(decimal));
				dtcol.Columns.Add("FactoryWeight", typeof(decimal));
				dtcol.Columns.Add("DifferenceWeight", typeof(decimal));
				dtcol.Rows.Clear();
				for (int i = 0; i < dtGridRegister.Rows.Count; i++)
				{
					dtcol.Rows.Add(dtGridRegister.Rows[i]["ExImInvoiceId"], dtGridRegister.Rows[i]["InvoiceNo"], dtGridRegister.Rows[i]["InvoiceDate"], dtGridRegister.Rows[i]["DoId"], dtGridRegister.Rows[i]["DoNo"], dtGridRegister.Rows[i]["DoDate"], dtGridRegister.Rows[i]["Id"], dtGridRegister.Rows[i]["GpDate"], dtGridRegister.Rows[i]["GpSrNo"], dtGridRegister.Rows[i]["Status"], dtGridRegister.Rows[i]["VehicleType"], dtGridRegister.Rows[i]["VehicleNo"], dtGridRegister.Rows[i]["BiltyNo"], dtGridRegister.Rows[i]["Container"], dtGridRegister.Rows[i]["Container1"], dtGridRegister.Rows[i]["SupplierWeight"], dtGridRegister.Rows[i]["FactoryWeight"], dtGridRegister.Rows[i]["DifferenceWeight"]);
				}
				grdgpInfo.DataSource = dtcol;
				grdgpInfo.RetrieveStructure();
				grdgpInfo.RootTable.HeaderLines = 2;
				foreach (GridEXColumn item in (JanusCollectionBase)grdgpInfo.RootTable.Columns)
				{
					item.HeaderStyle.TextAlignment = (TextAlignment)2;
				}
				grdgpInfo.RootTable.Columns["Id"].Visible = false;
				grdgpInfo.RootTable.Columns["ExImInvoiceId"].Visible = false;
				grdgpInfo.RootTable.Columns["DoId"].Visible = false;
				grdgpInfo.RootTable.Columns["DoNo"].ColumnType = (ColumnType)5;
				grdgpInfo.RootTable.Columns["InvoiceNo"].Width = Constants.ExportConstants.InvoiceNoConstant;
				grdgpInfo.RootTable.Columns["InvoiceDate"].Width = Constants.ExportConstants.DateConstant;
				grdgpInfo.RootTable.Columns["DoNo"].Width = Constants.ExportConstants.DoNoConstant;
				grdgpInfo.RootTable.Columns["DoDate"].Width = Constants.ExportConstants.DateConstant;
				grdgpInfo.RootTable.Columns["GpDate"].Width = Constants.ExportConstants.DateConstant;
				grdgpInfo.RootTable.Columns["GpSrNo"].Width = Constants.ExportConstants.DocNoConstant;
				grdgpInfo.RootTable.Columns["Status"].Width = Constants.ExportConstants.StatusConstant;
				grdgpInfo.RootTable.Columns["VehicleType"].Width = Constants.ExportConstants.VehicleType;
				grdgpInfo.RootTable.Columns["VehicleNo"].Width = Constants.ExportConstants.VehicleNoConstant;
				grdgpInfo.RootTable.Columns["BiltyNo"].Width = Constants.ExportConstants.BiltyNoConstant;
				grdgpInfo.RootTable.Columns["Container"].Width = Constants.ExportConstants.ContainerNo;
				grdgpInfo.RootTable.Columns["Container1"].Width = Constants.ExportConstants.ContainerNo;
				grdgpInfo.RootTable.Columns["SupplierWeight"].Width = Constants.ExportConstants.WeightConstant;
				grdgpInfo.RootTable.Columns["FactoryWeight"].Width = Constants.ExportConstants.WeightConstant;
				grdgpInfo.RootTable.Columns["DifferenceWeight"].Width = Constants.ExportConstants.WeightConstant;
				grdgpInfo.RootTable.Columns["SupplierWeight"].AggregateFunction = (AggregateFunction)2;
				grdgpInfo.RootTable.Columns["SupplierWeight"].TextAlignment = (TextAlignment)3;
				grdgpInfo.RootTable.Columns["SupplierWeight"].FormatString = "##,#.##";
				grdgpInfo.RootTable.Columns["SupplierWeight"].TotalFormatString = "##,#.##";
				grdgpInfo.RootTable.Columns["FactoryWeight"].AggregateFunction = (AggregateFunction)2;
				grdgpInfo.RootTable.Columns["FactoryWeight"].TextAlignment = (TextAlignment)3;
				grdgpInfo.RootTable.Columns["FactoryWeight"].FormatString = "##,#.##";
				grdgpInfo.RootTable.Columns["FactoryWeight"].TotalFormatString = "##,#.##";
				grdgpInfo.RootTable.Columns["DifferenceWeight"].AggregateFunction = (AggregateFunction)2;
				grdgpInfo.RootTable.Columns["DifferenceWeight"].TextAlignment = (TextAlignment)3;
				grdgpInfo.RootTable.Columns["DifferenceWeight"].FormatString = "##,#.##";
				grdgpInfo.RootTable.Columns["DifferenceWeight"].TotalFormatString = "##,#.##";
				grdgpInfo.RootTable.Columns.Add("Load");
				grdgpInfo.RootTable.Columns["Load"].Key = "Load";
				grdgpInfo.RootTable.Columns["Load"].Caption = "Load";
				grdgpInfo.RootTable.Columns["Load"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdgpInfo.RootTable.Columns["Load"].ButtonStyle = (ButtonStyle)5;
				grdgpInfo.RootTable.Columns["Load"].ButtonText = "Load";
				grdgpInfo.RootTable.Columns["Load"].Width = 40;
				grdgpInfo.RootTable.Columns["Load"].Position = 0;
				grdgpInfo.FrozenColumns = 1;
				ctrlGrdBar3_Load(null, null);
			}
			else
			{
				grdgpInfo.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdgpInfo_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdgpInfo.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0 && e.Column.Key == "Load")
			{
				FormReset();
				int GPId = Conversion.ToInt(item.Cells["Id"].Value);
				cmbGpNo.Value = GPId;
				cmbGpNo_Leave(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdgpInfo_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdgpInfo.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0 && e.Column.Key == "DoNo")
			{
				CommonServices.InvDeliveryOrderSlipWithSubReport(Conversion.ToInt(item.Cells["DoId"].Value), 84);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdgpInfo).Name);
			if (fs != null)
			{
				grdgpInfo.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbcontainerNo_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbcontainerNo).ActiveRow != null && ((Control)(object)cmbcontainerNo).Text != "")
			{
				string SealNo = Conversion.ToString(((UltraDropDownBase)cmbcontainerNo).SelectedRow.Cells["SealNo"].Value);
				if (SealNo != "")
				{
					txtSeal.Text = SealNo;
				}
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
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Expected O, but got Unknown
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Expected O, but got Unknown
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Expected O, but got Unknown
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Expected O, but got Unknown
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Expected O, but got Unknown
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Expected O, but got Unknown
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Expected O, but got Unknown
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Expected O, but got Unknown
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Expected O, but got Unknown
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Expected O, but got Unknown
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Expected O, but got Unknown
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Expected O, but got Unknown
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Expected O, but got Unknown
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Expected O, but got Unknown
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Expected O, but got Unknown
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Expected O, but got Unknown
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Expected O, but got Unknown
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Expected O, but got Unknown
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Expected O, but got Unknown
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Expected O, but got Unknown
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Expected O, but got Unknown
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Expected O, but got Unknown
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Expected O, but got Unknown
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Expected O, but got Unknown
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Expected O, but got Unknown
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Expected O, but got Unknown
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Expected O, but got Unknown
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Expected O, but got Unknown
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Expected O, but got Unknown
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Expected O, but got Unknown
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Expected O, but got Unknown
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Expected O, but got Unknown
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Expected O, but got Unknown
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Expected O, but got Unknown
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Expected O, but got Unknown
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Expected O, but got Unknown
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Expected O, but got Unknown
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Expected O, but got Unknown
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Expected O, but got Unknown
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Expected O, but got Unknown
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Expected O, but got Unknown
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Expected O, but got Unknown
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Expected O, but got Unknown
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Expected O, but got Unknown
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Expected O, but got Unknown
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Expected O, but got Unknown
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Expected O, but got Unknown
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Expected O, but got Unknown
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Expected O, but got Unknown
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Expected O, but got Unknown
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Expected O, but got Unknown
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Expected O, but got Unknown
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Expected O, but got Unknown
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Expected O, but got Unknown
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Expected O, but got Unknown
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Expected O, but got Unknown
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Expected O, but got Unknown
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Expected O, but got Unknown
		//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bd: Expected O, but got Unknown
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Expected O, but got Unknown
		//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cb: Expected O, but got Unknown
		//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d2: Expected O, but got Unknown
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Expected O, but got Unknown
		//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e0: Expected O, but got Unknown
		//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Expected O, but got Unknown
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ee: Expected O, but got Unknown
		//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f5: Expected O, but got Unknown
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Expected O, but got Unknown
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0603: Expected O, but got Unknown
		//IL_0603: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Expected O, but got Unknown
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0611: Expected O, but got Unknown
		//IL_0611: Unknown result type (might be due to invalid IL or missing references)
		//IL_0618: Expected O, but got Unknown
		//IL_0618: Unknown result type (might be due to invalid IL or missing references)
		//IL_061f: Expected O, but got Unknown
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0626: Expected O, but got Unknown
		//IL_0626: Unknown result type (might be due to invalid IL or missing references)
		//IL_062d: Expected O, but got Unknown
		//IL_062d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Expected O, but got Unknown
		//IL_0634: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Expected O, but got Unknown
		//IL_063b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0642: Expected O, but got Unknown
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Expected O, but got Unknown
		//IL_0649: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Expected O, but got Unknown
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_0657: Expected O, but got Unknown
		//IL_0657: Unknown result type (might be due to invalid IL or missing references)
		//IL_065e: Expected O, but got Unknown
		//IL_065e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0665: Expected O, but got Unknown
		//IL_0665: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Expected O, but got Unknown
		//IL_066c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0673: Expected O, but got Unknown
		//IL_0673: Unknown result type (might be due to invalid IL or missing references)
		//IL_067a: Expected O, but got Unknown
		//IL_067a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0681: Expected O, but got Unknown
		//IL_0681: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Expected O, but got Unknown
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_068f: Expected O, but got Unknown
		//IL_068f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0696: Expected O, but got Unknown
		//IL_0696: Unknown result type (might be due to invalid IL or missing references)
		//IL_069d: Expected O, but got Unknown
		//IL_069d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a4: Expected O, but got Unknown
		//IL_06a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ab: Expected O, but got Unknown
		//IL_06ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b2: Expected O, but got Unknown
		//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b9: Expected O, but got Unknown
		//IL_06b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c0: Expected O, but got Unknown
		//IL_070e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0718: Expected O, but got Unknown
		//IL_0745: Unknown result type (might be due to invalid IL or missing references)
		//IL_074f: Expected O, but got Unknown
		//IL_07df: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e9: Expected O, but got Unknown
		//IL_07ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f4: Expected O, but got Unknown
		//IL_0842: Unknown result type (might be due to invalid IL or missing references)
		//IL_084c: Expected O, but got Unknown
		//IL_084d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0857: Expected O, but got Unknown
		//IL_0863: Unknown result type (might be due to invalid IL or missing references)
		//IL_086d: Expected O, but got Unknown
		//IL_08c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d0: Expected O, but got Unknown
		//IL_08d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08db: Expected O, but got Unknown
		//IL_08e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f1: Expected O, but got Unknown
		//IL_096b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0975: Expected O, but got Unknown
		//IL_0a10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1a: Expected O, but got Unknown
		//IL_0b44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4e: Expected O, but got Unknown
		//IL_0b4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b59: Expected O, but got Unknown
		//IL_0bb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbc: Expected O, but got Unknown
		//IL_0bbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc7: Expected O, but got Unknown
		//IL_0bde: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be8: Expected O, but got Unknown
		//IL_0bff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c09: Expected O, but got Unknown
		//IL_0c6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c77: Expected O, but got Unknown
		//IL_0cd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cda: Expected O, but got Unknown
		//IL_0d6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d74: Expected O, but got Unknown
		//IL_0d8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d95: Expected O, but got Unknown
		//IL_0dd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de2: Expected O, but got Unknown
		//IL_0dee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df8: Expected O, but got Unknown
		//IL_0e5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e66: Expected O, but got Unknown
		//IL_1625: Unknown result type (might be due to invalid IL or missing references)
		//IL_162f: Expected O, but got Unknown
		//IL_163c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1646: Expected O, but got Unknown
		//IL_1951: Unknown result type (might be due to invalid IL or missing references)
		//IL_195b: Expected O, but got Unknown
		//IL_1968: Unknown result type (might be due to invalid IL or missing references)
		//IL_1972: Expected O, but got Unknown
		//IL_aef0: Unknown result type (might be due to invalid IL or missing references)
		//IL_aefa: Expected O, but got Unknown
		//IL_af1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_af28: Expected O, but got Unknown
		GridEXLayout grdgpInfo_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance35 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
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
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		GridEXLayout grdotheritems_DesignTimeLayout = new GridEXLayout();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
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
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Export.EximForwarding));
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel4 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdgpInfo = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label32 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel15 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label53 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.btnAddinGrid = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.txtEbTotalMaster = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.txtEbUnitMaster = new System.Windows.Forms.TextBox();
		this.txtEbUnitInner = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.txtEbTotalInner = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.CmbPackUomInner = new UltraCombo();
		this.CmbPackUomMaster = new UltraCombo();
		this.label17 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.txtQtyInner = new System.Windows.Forms.TextBox();
		this.txtQtyMaster = new System.Windows.Forms.TextBox();
		this.label51 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.cmbcontainerNo = new UltraCombo();
		this.CmbCropYear = new UltraCombo();
		this.label40 = new System.Windows.Forms.Label();
		this.cmbItem = new UltraCombo();
		this.label45 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.txtStockWeight = new System.Windows.Forms.TextBox();
		this.txtAddLess = new System.Windows.Forms.TextBox();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.CmbJobLot = new UltraCombo();
		this.cmbPackType = new UltraCombo();
		this.label55 = new System.Windows.Forms.Label();
		this.CmbWareHouse = new UltraCombo();
		this.label54 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.btnDetailCancel = new System.Windows.Forms.Button();
		this.btnDetailUpdate = new System.Windows.Forms.Button();
		this.txtItemDesc = new System.Windows.Forms.TextBox();
		this.label20 = new System.Windows.Forms.Label();
		this.txtSeal = new System.Windows.Forms.TextBox();
		this.label30 = new System.Windows.Forms.Label();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.panel11 = new System.Windows.Forms.Panel();
		this.grdotheritems = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.txtOContainer = new System.Windows.Forms.TextBox();
		this.label42 = new System.Windows.Forms.Label();
		this.btnoCancel = new System.Windows.Forms.Button();
		this.btnoUpdate = new System.Windows.Forms.Button();
		this.btnoadd = new System.Windows.Forms.Button();
		this.label27 = new System.Windows.Forms.Label();
		this.txtoRemarks = new System.Windows.Forms.TextBox();
		this.txtoQty = new System.Windows.Forms.TextBox();
		this.label41 = new System.Windows.Forms.Label();
		this.txtoRate = new System.Windows.Forms.TextBox();
		this.label63 = new System.Windows.Forms.Label();
		this.label70 = new System.Windows.Forms.Label();
		this.txtoAmount = new System.Windows.Forms.TextBox();
		this.CmbOtherItem = new UltraCombo();
		this.label71 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.btnRecordsUpdate = new System.Windows.Forms.Button();
		this.label36 = new System.Windows.Forms.Label();
		this.ChkPrintPreview = new System.Windows.Forms.CheckBox();
		this.label18 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.txtNoofInvoices = new System.Windows.Forms.TextBox();
		this.label48 = new System.Windows.Forms.Label();
		this.txtInvoiceNetWeight = new System.Windows.Forms.TextBox();
		this.InvoiceNetWeight = new System.Windows.Forms.Label();
		this.txtDoNetWeight = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtDoGrossWeight = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtBalFcWeight = new System.Windows.Forms.TextBox();
		this.txtDispatchWeight = new System.Windows.Forms.TextBox();
		this.label49 = new System.Windows.Forms.Label();
		this.txtFactoryWeight = new System.Windows.Forms.TextBox();
		this.label43 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.txtContainer2 = new System.Windows.Forms.TextBox();
		this.label34 = new System.Windows.Forms.Label();
		this.txtconainer1 = new System.Windows.Forms.TextBox();
		this.label33 = new System.Windows.Forms.Label();
		this.chkdateisactive = new System.Windows.Forms.CheckBox();
		this.cmbCustomer = new UltraCombo();
		this.cmbShippingAgent = new UltraCombo();
		this.txtShippedContainer = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.txtNoOfContainer = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.txtCompletedDate = new System.Windows.Forms.DateTimePicker();
		this.label26 = new System.Windows.Forms.Label();
		this.txtOtherCharges = new System.Windows.Forms.TextBox();
		this.cmbGpNo = new UltraCombo();
		this.cmbShippingLIne = new UltraCombo();
		this.txtFreight = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.cmbDestinationPort = new UltraCombo();
		this.label23 = new System.Windows.Forms.Label();
		this.txtCNICNO = new System.Windows.Forms.TextBox();
		this.cmbLoadingPort = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.txtBilityNo = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.txtDriverCellNo = new System.Windows.Forms.TextBox();
		this.cmbtransporter = new UltraCombo();
		this.txtDriverName = new System.Windows.Forms.TextBox();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtGPDate = new System.Windows.Forms.DateTimePicker();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.cmbInvoiceNo = new UltraCombo();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.BtnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.BtnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnGenerateAvailableStock = new System.Windows.Forms.ToolStripButton();
		this.btnStockReport = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label93 = new System.Windows.Forms.Label();
		this.grdHistoryDetail = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label92 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel10 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label46 = new System.Windows.Forms.Label();
		this.CmbInvoiceNoHistory = new UltraCombo();
		this.label44 = new System.Windows.Forms.Label();
		this.CmbGpNoHistory = new UltraCombo();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.label31 = new System.Windows.Forms.Label();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.label89 = new System.Windows.Forms.Label();
		this.label90 = new System.Windows.Forms.Label();
		this.label91 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label69 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdgpInfo).BeginInit();
		this.panel16.SuspendLayout();
		this.panel15.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel14.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUomInner).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUomMaster).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcontainerNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).BeginInit();
		this.tabPage4.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdotheritems).BeginInit();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOtherItem).BeginInit();
		this.panel3.SuspendLayout();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbShippingAgent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGpNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbShippingLIne).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDestinationPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoadingPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbtransporter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbInvoiceNo).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).BeginInit();
		this.panel6.SuspendLayout();
		this.panel10.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNoHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGpNoHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1307, 749);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1307, 749);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1299, 720);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel4.Controls.Add(this.tabControl2);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(3, 238);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1293, 479);
		this.panel4.TabIndex = 4;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1293, 479);
		this.tabControl2.TabIndex = 0;
		this.tabPage3.Controls.Add(this.panel7);
		this.tabPage3.Controls.Add(this.panel5);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1285, 450);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Detail";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdgpInfo);
		this.panel7.Controls.Add(this.panel16);
		this.panel7.Controls.Add(this.panel15);
		this.panel7.Controls.Add(this.panel14);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(3, 111);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1279, 336);
		this.panel7.TabIndex = 1;
		this.grdgpInfo.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdgpInfo).BackColor = System.Drawing.Color.White;
		grdgpInfo_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdgpInfo.DesignTimeLayout = grdgpInfo_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdgpInfo).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdgpInfo).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdgpInfo.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdgpInfo).Location = new System.Drawing.Point(0, 232);
		((System.Windows.Forms.Control)(object)this.grdgpInfo).Name = "grdgpInfo";
		((ScrollableBase)this.grdgpInfo).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdgpInfo).Size = new System.Drawing.Size(1279, 104);
		((System.Windows.Forms.Control)(object)this.grdgpInfo).TabIndex = 528;
		this.grdgpInfo.TotalRow = (InheritableBoolean)1;
		this.grdgpInfo.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdgpInfo).VisualStyle = (VisualStyle)4;
		this.grdgpInfo.ColumnButtonClick += new ColumnActionEventHandler(grdgpInfo_ColumnButtonClick);
		this.grdgpInfo.LinkClicked += new ColumnActionEventHandler(grdgpInfo_LinkClicked);
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label32);
		this.panel16.Controls.Add(this.ctrlGrdBar3);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 210);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1279, 22);
		this.panel16.TabIndex = 527;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label32.ForeColor = System.Drawing.Color.White;
		this.label32.Location = new System.Drawing.Point(3, 2);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(159, 17);
		this.label32.TabIndex = 101;
		this.label32.Text = "Gate Pass information";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = null;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1237, 0);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = null;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(39, 21);
		this.ctrlGrdBar3.TabIndex = 36;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 24);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1279, 186);
		this.panel15.TabIndex = 2;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1279, 186);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		this.grdDetail.LinkClicked += new ColumnActionEventHandler(grdDetail_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetail_KeyDown);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.ctrlGrdBar1);
		this.panel14.Controls.Add(this.label53);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1279, 24);
		this.panel14.TabIndex = 1;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1239, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdDetail;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 36;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label53.ForeColor = System.Drawing.Color.White;
		this.label53.Location = new System.Drawing.Point(4, 3);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(228, 17);
		this.label53.TabIndex = 102;
		this.label53.Text = "Export Goods Forwarding Detail";
		this.panel5.Controls.Add(this.btnAddinGrid);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add(this.txtEbTotalMaster);
		this.panel5.Controls.Add(this.label6);
		this.panel5.Controls.Add(this.txtEbUnitMaster);
		this.panel5.Controls.Add(this.txtEbUnitInner);
		this.panel5.Controls.Add(this.label9);
		this.panel5.Controls.Add(this.txtEbTotalInner);
		this.panel5.Controls.Add(this.label13);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUomInner);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUomMaster);
		this.panel5.Controls.Add(this.label17);
		this.panel5.Controls.Add(this.label38);
		this.panel5.Controls.Add(this.txtQtyInner);
		this.panel5.Controls.Add(this.txtQtyMaster);
		this.panel5.Controls.Add(this.label51);
		this.panel5.Controls.Add(this.label52);
		this.panel5.Controls.Add(this.label50);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcontainerNo);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYear);
		this.panel5.Controls.Add(this.label40);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.panel5.Controls.Add(this.label45);
		this.panel5.Controls.Add(this.label47);
		this.panel5.Controls.Add(this.label39);
		this.panel5.Controls.Add(this.label28);
		this.panel5.Controls.Add(this.txtStockWeight);
		this.panel5.Controls.Add(this.txtAddLess);
		this.panel5.Controls.Add(this.txtGrossWeight);
		this.panel5.Controls.Add(this.label29);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackType);
		this.panel5.Controls.Add(this.label55);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWareHouse);
		this.panel5.Controls.Add(this.label54);
		this.panel5.Controls.Add(this.label37);
		this.panel5.Controls.Add(this.btnDetailCancel);
		this.panel5.Controls.Add(this.btnDetailUpdate);
		this.panel5.Controls.Add(this.txtItemDesc);
		this.panel5.Controls.Add(this.label20);
		this.panel5.Controls.Add(this.txtSeal);
		this.panel5.Controls.Add(this.label30);
		this.panel5.Controls.Add(this.txtNetWeight);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1279, 108);
		this.panel5.TabIndex = 0;
		this.btnAddinGrid.BackColor = System.Drawing.Color.Teal;
		this.btnAddinGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddinGrid.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddinGrid.ForeColor = System.Drawing.Color.White;
		this.btnAddinGrid.Location = new System.Drawing.Point(1139, 76);
		this.btnAddinGrid.Name = "btnAddinGrid";
		this.btnAddinGrid.Size = new System.Drawing.Size(36, 28);
		this.btnAddinGrid.TabIndex = 20;
		this.btnAddinGrid.Text = "+";
		this.btnAddinGrid.UseVisualStyleBackColor = false;
		this.btnAddinGrid.Visible = false;
		this.btnAddinGrid.Click += new System.EventHandler(btnAddinGrid_Click);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(851, 9);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(49, 15);
		this.label3.TabIndex = 117;
		this.label3.Text = "Eb Total";
		this.txtEbTotalMaster.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbTotalMaster.Enabled = false;
		this.txtEbTotalMaster.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEbTotalMaster.Location = new System.Drawing.Point(851, 31);
		this.txtEbTotalMaster.Name = "txtEbTotalMaster";
		this.txtEbTotalMaster.Size = new System.Drawing.Size(86, 23);
		this.txtEbTotalMaster.TabIndex = 6;
		this.txtEbTotalMaster.Tag = "TotalPackingWeightMaster";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(777, 9);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(62, 15);
		this.label6.TabIndex = 116;
		this.label6.Text = "Eb Weight";
		this.txtEbUnitMaster.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbUnitMaster.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEbUnitMaster.Location = new System.Drawing.Point(777, 31);
		this.txtEbUnitMaster.Name = "txtEbUnitMaster";
		this.txtEbUnitMaster.Size = new System.Drawing.Size(72, 23);
		this.txtEbUnitMaster.TabIndex = 5;
		this.txtEbUnitMaster.Tag = "PackingWeightMaster";
		this.txtEbUnitMaster.TextChanged += new System.EventHandler(txtEbUnitMaster_TextChanged);
		this.txtEbUnitMaster.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEbUnitMaster_KeyPress);
		this.txtEbUnitInner.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbUnitInner.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEbUnitInner.Location = new System.Drawing.Point(1115, 31);
		this.txtEbUnitInner.Name = "txtEbUnitInner";
		this.txtEbUnitInner.Size = new System.Drawing.Size(72, 23);
		this.txtEbUnitInner.TabIndex = 9;
		this.txtEbUnitInner.Tag = "PackingWeight";
		this.txtEbUnitInner.TextChanged += new System.EventHandler(txtEbUnitInner_TextChanged);
		this.txtEbUnitInner.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEbUnitInner_KeyPress);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(1189, 9);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(49, 15);
		this.label9.TabIndex = 115;
		this.label9.Text = "Eb Total";
		this.txtEbTotalInner.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbTotalInner.Enabled = false;
		this.txtEbTotalInner.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEbTotalInner.Location = new System.Drawing.Point(1189, 31);
		this.txtEbTotalInner.Name = "txtEbTotalInner";
		this.txtEbTotalInner.Size = new System.Drawing.Size(86, 23);
		this.txtEbTotalInner.TabIndex = 10;
		this.txtEbTotalInner.Tag = "TotalPackingWeight";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(1115, 9);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(62, 15);
		this.label13.TabIndex = 114;
		this.label13.Text = "Eb Weight";
		this.CmbPackUomInner.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUomInner.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUomInner.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUomInner.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUomInner).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance90;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUomInner).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance146).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance157).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance157).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance157).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance225).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance226).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUomInner).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackUomInner.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUomInner).Location = new System.Drawing.Point(1018, 29);
		((System.Windows.Forms.Control)(object)this.CmbPackUomInner).Name = "CmbPackUomInner";
		((System.Windows.Forms.Control)(object)this.CmbPackUomInner).Size = new System.Drawing.Size(94, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUomInner).TabIndex = 8;
		this.CmbPackUomInner.ValueChanged += new System.EventHandler(CmbPackUomInner_ValueChanged);
		this.CmbPackUomMaster.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUomMaster.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUomMaster.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUomMaster.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance195).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Appearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance212).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance212).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance212;
		((AppearanceBase)appearance213).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance213;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance214).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance214).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance200;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance215;
		((AppearanceBase)appearance216).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance216).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance217).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance217).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance217).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance217).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance217;
		((AppearanceBase)appearance218).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance218;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUomMaster).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackUomMaster.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUomMaster).Location = new System.Drawing.Point(680, 29);
		((System.Windows.Forms.Control)(object)this.CmbPackUomMaster).Name = "CmbPackUomMaster";
		((System.Windows.Forms.Control)(object)this.CmbPackUomMaster).Size = new System.Drawing.Size(94, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUomMaster).TabIndex = 4;
		this.CmbPackUomMaster.ValueChanged += new System.EventHandler(CmbPackUomMaster_ValueChanged);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(1020, 9);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(63, 15);
		this.label17.TabIndex = 102;
		this.label17.Text = "Pack UOM";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(682, 9);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(63, 15);
		this.label38.TabIndex = 103;
		this.label38.Text = "Pack UOM";
		this.txtQtyInner.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQtyInner.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtQtyInner.Location = new System.Drawing.Point(939, 31);
		this.txtQtyInner.Name = "txtQtyInner";
		this.txtQtyInner.Size = new System.Drawing.Size(77, 23);
		this.txtQtyInner.TabIndex = 7;
		this.txtQtyInner.TextChanged += new System.EventHandler(txtQtyInner_TextChanged);
		this.txtQtyInner.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQtyInner_KeyPress);
		this.txtQtyMaster.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQtyMaster.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtQtyMaster.Location = new System.Drawing.Point(600, 31);
		this.txtQtyMaster.Name = "txtQtyMaster";
		this.txtQtyMaster.Size = new System.Drawing.Size(77, 23);
		this.txtQtyMaster.TabIndex = 3;
		this.txtQtyMaster.TextChanged += new System.EventHandler(txtQtyMaster_TextChanged);
		this.txtQtyMaster.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQtyMaster_KeyPress);
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label51.Location = new System.Drawing.Point(939, 9);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(57, 15);
		this.label51.TabIndex = 110;
		this.label51.Text = "Inner Qty";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label52.Location = new System.Drawing.Point(600, 9);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(65, 15);
		this.label52.TabIndex = 111;
		this.label52.Text = "Master Qty";
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(882, 58);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(68, 15);
		this.label50.TabIndex = 101;
		this.label50.Text = "Container #";
		this.label50.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.cmbcontainerNo.AllowNull = (DefaultableBoolean)1;
		this.cmbcontainerNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcontainerNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcontainerNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcontainerNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance164;
		((SpecialBoxBase)((UltraGridBase)this.cmbcontainerNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance197).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance208;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance219).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance230).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance230).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance230).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance230).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcontainerNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcontainerNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcontainerNo).Location = new System.Drawing.Point(885, 77);
		((System.Windows.Forms.Control)(object)this.cmbcontainerNo).Name = "cmbcontainerNo";
		((System.Windows.Forms.Control)(object)this.cmbcontainerNo).Size = new System.Drawing.Size(127, 26);
		((System.Windows.Forms.Control)(object)this.cmbcontainerNo).TabIndex = 18;
		((System.Windows.Forms.Control)(object)this.cmbcontainerNo).Leave += new System.EventHandler(cmbcontainerNo_Leave);
		this.CmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance109).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Location = new System.Drawing.Point(784, 77);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Name = "CmbCropYear";
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabIndex = 17;
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(785, 58);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(54, 15);
		this.label40.TabIndex = 44;
		this.label40.Text = "CropYear";
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance161;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance165).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance167).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance168).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance168).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance168).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance168;
		((AppearanceBase)appearance169).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(10, 29);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		this.cmbItem.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(206, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbItem).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbItem).Leave += new System.EventHandler(cmbItem_Leave);
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(304, 58);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(79, 15);
		this.label45.TabIndex = 42;
		this.label45.Text = "Stock Weight";
		this.label45.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(10, 58);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(51, 15);
		this.label47.TabIndex = 42;
		this.label47.Text = "AddLess";
		this.label47.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(194, 58);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(78, 15);
		this.label39.TabIndex = 42;
		this.label39.Text = "Gross Weight";
		this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(10, 9);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(67, 15);
		this.label28.TabIndex = 2;
		this.label28.Text = "Item Name";
		this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtStockWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtStockWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtStockWeight.Location = new System.Drawing.Point(304, 79);
		this.txtStockWeight.Name = "txtStockWeight";
		this.txtStockWeight.ReadOnly = true;
		this.txtStockWeight.Size = new System.Drawing.Size(108, 23);
		this.txtStockWeight.TabIndex = 14;
		this.txtStockWeight.TabStop = false;
		this.txtStockWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAddLess.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAddLess.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAddLess.Location = new System.Drawing.Point(10, 79);
		this.txtAddLess.Name = "txtAddLess";
		this.txtAddLess.Size = new System.Drawing.Size(72, 23);
		this.txtAddLess.TabIndex = 11;
		this.txtAddLess.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAddLess.TextChanged += new System.EventHandler(txtAddLess_TextChanged);
		this.txtAddLess.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAddLess_KeyPress);
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrossWeight.Location = new System.Drawing.Point(194, 79);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.ReadOnly = true;
		this.txtGrossWeight.Size = new System.Drawing.Size(108, 23);
		this.txtGrossWeight.TabIndex = 13;
		this.txtGrossWeight.TabStop = false;
		this.txtGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(220, 9);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(57, 15);
		this.label29.TabIndex = 4;
		this.label29.Text = "PackType";
		this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance174;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance176).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance176).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance178).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance180).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance181).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(619, 77);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(164, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 16;
		this.cmbPackType.AllowNull = (DefaultableBoolean)1;
		this.cmbPackType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.cmbPackType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance187).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance187).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance187).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance187;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance188;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.cmbPackType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance190;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance222;
		((AppearanceBase)appearance223).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance223).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance224).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance224).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance224).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance224).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance224;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbPackType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPackType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackType).Location = new System.Drawing.Point(220, 29);
		((System.Windows.Forms.Control)(object)this.cmbPackType).Name = "cmbPackType";
		this.cmbPackType.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbPackType).Size = new System.Drawing.Size(134, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackType).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbPackType).TabStop = false;
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(617, 58);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(43, 15);
		this.label55.TabIndex = 40;
		this.label55.Text = "JobLot";
		this.label55.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.CmbWareHouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance209).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance210).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWareHouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Location = new System.Drawing.Point(415, 77);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Name = "CmbWareHouse";
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Size = new System.Drawing.Size(203, 26);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).TabIndex = 15;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(414, 58);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(69, 15);
		this.label54.TabIndex = 38;
		this.label54.Text = "WareHouse";
		this.label54.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(1015, 58);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(39, 15);
		this.label37.TabIndex = 20;
		this.label37.Text = "Seal #";
		this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.btnDetailCancel.AutoSize = true;
		this.btnDetailCancel.BackColor = System.Drawing.Color.Teal;
		this.btnDetailCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDetailCancel.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDetailCancel.ForeColor = System.Drawing.Color.White;
		this.btnDetailCancel.Location = new System.Drawing.Point(1206, 76);
		this.btnDetailCancel.Name = "btnDetailCancel";
		this.btnDetailCancel.Size = new System.Drawing.Size(66, 28);
		this.btnDetailCancel.TabIndex = 22;
		this.btnDetailCancel.Text = "Cancel";
		this.btnDetailCancel.UseVisualStyleBackColor = false;
		this.btnDetailCancel.Click += new System.EventHandler(btnDetailCancel_Click);
		this.btnDetailUpdate.AutoSize = true;
		this.btnDetailUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnDetailUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDetailUpdate.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDetailUpdate.ForeColor = System.Drawing.Color.White;
		this.btnDetailUpdate.Location = new System.Drawing.Point(1139, 76);
		this.btnDetailUpdate.Name = "btnDetailUpdate";
		this.btnDetailUpdate.Size = new System.Drawing.Size(66, 28);
		this.btnDetailUpdate.TabIndex = 21;
		this.btnDetailUpdate.Text = "Update";
		this.btnDetailUpdate.UseVisualStyleBackColor = false;
		this.btnDetailUpdate.Click += new System.EventHandler(btnDetailUpdate_Click);
		this.txtItemDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemDesc.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemDesc.Location = new System.Drawing.Point(356, 31);
		this.txtItemDesc.Name = "txtItemDesc";
		this.txtItemDesc.Size = new System.Drawing.Size(242, 23);
		this.txtItemDesc.TabIndex = 2;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(356, 9);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(96, 15);
		this.label20.TabIndex = 34;
		this.label20.Text = "Item Description";
		this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtSeal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSeal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSeal.Location = new System.Drawing.Point(1015, 79);
		this.txtSeal.Name = "txtSeal";
		this.txtSeal.Size = new System.Drawing.Size(122, 23);
		this.txtSeal.TabIndex = 19;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(82, 58);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(65, 15);
		this.label30.TabIndex = 6;
		this.label30.Text = "NetWeight";
		this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeight.Location = new System.Drawing.Point(84, 79);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.ReadOnly = true;
		this.txtNetWeight.Size = new System.Drawing.Size(108, 23);
		this.txtNetWeight.TabIndex = 12;
		this.txtNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.tabPage4.Controls.Add(this.panel11);
		this.tabPage4.Controls.Add(this.panel9);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(1285, 450);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Other Items";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.grdotheritems);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(3, 71);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1279, 376);
		this.panel11.TabIndex = 1;
		this.grdotheritems.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdotheritems).BackColor = System.Drawing.Color.White;
		grdotheritems_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdotheritems.DesignTimeLayout = grdotheritems_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdotheritems.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdotheritems).Name = "grdotheritems";
		((ScrollableBase)this.grdotheritems).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Size = new System.Drawing.Size(1279, 376);
		((System.Windows.Forms.Control)(object)this.grdotheritems).TabIndex = 0;
		this.grdotheritems.TotalRow = (InheritableBoolean)1;
		this.grdotheritems.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdotheritems).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdotheritems).DoubleClick += new System.EventHandler(grdotheritems_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdotheritems).KeyDown += new System.Windows.Forms.KeyEventHandler(grdotheritems_KeyDown);
		this.panel9.Controls.Add(this.txtOContainer);
		this.panel9.Controls.Add(this.label42);
		this.panel9.Controls.Add(this.btnoCancel);
		this.panel9.Controls.Add(this.btnoUpdate);
		this.panel9.Controls.Add(this.btnoadd);
		this.panel9.Controls.Add(this.label27);
		this.panel9.Controls.Add(this.txtoRemarks);
		this.panel9.Controls.Add(this.txtoQty);
		this.panel9.Controls.Add(this.label41);
		this.panel9.Controls.Add(this.txtoRate);
		this.panel9.Controls.Add(this.label63);
		this.panel9.Controls.Add(this.label70);
		this.panel9.Controls.Add(this.txtoAmount);
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOtherItem);
		this.panel9.Controls.Add(this.label71);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 3);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1279, 68);
		this.panel9.TabIndex = 0;
		this.txtOContainer.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOContainer.Location = new System.Drawing.Point(556, 34);
		this.txtOContainer.Name = "txtOContainer";
		this.txtOContainer.Size = new System.Drawing.Size(132, 23);
		this.txtOContainer.TabIndex = 4;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(561, 14);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(77, 15);
		this.label42.TabIndex = 522;
		this.label42.Text = "Container No";
		this.btnoCancel.BackColor = System.Drawing.Color.Teal;
		this.btnoCancel.FlatAppearance.BorderSize = 0;
		this.btnoCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnoCancel.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnoCancel.ForeColor = System.Drawing.Color.White;
		this.btnoCancel.Location = new System.Drawing.Point(1127, 32);
		this.btnoCancel.Name = "btnoCancel";
		this.btnoCancel.Size = new System.Drawing.Size(59, 26);
		this.btnoCancel.TabIndex = 8;
		this.btnoCancel.Text = "Cancel";
		this.btnoCancel.UseVisualStyleBackColor = false;
		this.btnoCancel.Click += new System.EventHandler(btnoCancel_Click);
		this.btnoUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnoUpdate.FlatAppearance.BorderSize = 0;
		this.btnoUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnoUpdate.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnoUpdate.ForeColor = System.Drawing.Color.White;
		this.btnoUpdate.Location = new System.Drawing.Point(1061, 32);
		this.btnoUpdate.Name = "btnoUpdate";
		this.btnoUpdate.Size = new System.Drawing.Size(65, 26);
		this.btnoUpdate.TabIndex = 7;
		this.btnoUpdate.Text = "Update";
		this.btnoUpdate.UseVisualStyleBackColor = false;
		this.btnoUpdate.Click += new System.EventHandler(btnoUpdate_Click);
		this.btnoadd.BackColor = System.Drawing.Color.White;
		this.btnoadd.FlatAppearance.BorderSize = 0;
		this.btnoadd.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnoadd.Location = new System.Drawing.Point(1189, 32);
		this.btnoadd.Name = "btnoadd";
		this.btnoadd.Size = new System.Drawing.Size(25, 26);
		this.btnoadd.TabIndex = 6;
		this.btnoadd.Text = "+";
		this.btnoadd.UseVisualStyleBackColor = false;
		this.btnoadd.Visible = false;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(691, 12);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(52, 15);
		this.label27.TabIndex = 521;
		this.label27.Text = "Remarks";
		this.txtoRemarks.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtoRemarks.Location = new System.Drawing.Point(694, 34);
		this.txtoRemarks.Name = "txtoRemarks";
		this.txtoRemarks.Size = new System.Drawing.Size(364, 23);
		this.txtoRemarks.TabIndex = 5;
		this.txtoQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtoQty.Location = new System.Drawing.Point(263, 34);
		this.txtoQty.Name = "txtoQty";
		this.txtoQty.Size = new System.Drawing.Size(101, 23);
		this.txtoQty.TabIndex = 1;
		this.txtoQty.Text = "0";
		this.txtoQty.TextChanged += new System.EventHandler(txtoQty_TextChanged);
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(270, 11);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(26, 15);
		this.label41.TabIndex = 518;
		this.label41.Text = "Qty";
		this.txtoRate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtoRate.Location = new System.Drawing.Point(367, 34);
		this.txtoRate.Name = "txtoRate";
		this.txtoRate.ReadOnly = true;
		this.txtoRate.Size = new System.Drawing.Size(80, 23);
		this.txtoRate.TabIndex = 2;
		this.txtoRate.Text = "0";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.Location = new System.Drawing.Point(364, 11);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(30, 15);
		this.label63.TabIndex = 520;
		this.label63.Text = "Rate";
		this.label70.AutoSize = true;
		this.label70.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label70.Location = new System.Drawing.Point(448, 11);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(51, 15);
		this.label70.TabIndex = 519;
		this.label70.Text = "Amount";
		this.txtoAmount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtoAmount.Location = new System.Drawing.Point(451, 34);
		this.txtoAmount.Name = "txtoAmount";
		this.txtoAmount.ReadOnly = true;
		this.txtoAmount.Size = new System.Drawing.Size(101, 23);
		this.txtoAmount.TabIndex = 3;
		this.txtoAmount.Text = "0";
		this.CmbOtherItem.AllowNull = (DefaultableBoolean)1;
		this.CmbOtherItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOtherItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOtherItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance131;
		((SpecialBoxBase)((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance228).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance228).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance228;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance229).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance229;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance231).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance233).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance234).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance234).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance234).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance234).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance234;
		((AppearanceBase)appearance235).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance236).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Location = new System.Drawing.Point(12, 32);
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Name = "CmbOtherItem";
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Size = new System.Drawing.Size(247, 26);
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).TabIndex = 0;
		this.label71.AutoSize = true;
		this.label71.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label71.Location = new System.Drawing.Point(9, 10);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(67, 15);
		this.label71.TabIndex = 517;
		this.label71.Text = "Item Name";
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.panel12);
		this.panel3.Controls.Add(this.ChkPrintPreview);
		this.panel3.Controls.Add(this.label18);
		this.panel3.Controls.Add(this.label5);
		this.panel3.Controls.Add(this.txtNoofInvoices);
		this.panel3.Controls.Add(this.label48);
		this.panel3.Controls.Add(this.txtInvoiceNetWeight);
		this.panel3.Controls.Add(this.InvoiceNetWeight);
		this.panel3.Controls.Add(this.txtDoNetWeight);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add(this.txtDoGrossWeight);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add(this.txtBalFcWeight);
		this.panel3.Controls.Add(this.txtDispatchWeight);
		this.panel3.Controls.Add(this.label49);
		this.panel3.Controls.Add(this.txtFactoryWeight);
		this.panel3.Controls.Add(this.label43);
		this.panel3.Controls.Add(this.label35);
		this.panel3.Controls.Add(this.txtContainer2);
		this.panel3.Controls.Add(this.label34);
		this.panel3.Controls.Add(this.txtconainer1);
		this.panel3.Controls.Add(this.label33);
		this.panel3.Controls.Add(this.chkdateisactive);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCustomer);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbShippingAgent);
		this.panel3.Controls.Add(this.txtShippedContainer);
		this.panel3.Controls.Add(this.label25);
		this.panel3.Controls.Add(this.txtNoOfContainer);
		this.panel3.Controls.Add(this.label16);
		this.panel3.Controls.Add(this.label24);
		this.panel3.Controls.Add(this.txtCompletedDate);
		this.panel3.Controls.Add(this.label26);
		this.panel3.Controls.Add(this.txtOtherCharges);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbGpNo);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbShippingLIne);
		this.panel3.Controls.Add(this.txtFreight);
		this.panel3.Controls.Add(this.label15);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDestinationPort);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.txtCNICNO);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLoadingPort);
		this.panel3.Controls.Add(this.label22);
		this.panel3.Controls.Add(this.label21);
		this.panel3.Controls.Add(this.label12);
		this.panel3.Controls.Add(this.label19);
		this.panel3.Controls.Add(this.label14);
		this.panel3.Controls.Add(this.txtBilityNo);
		this.panel3.Controls.Add(this.label11);
		this.panel3.Controls.Add(this.label10);
		this.panel3.Controls.Add(this.txtDriverCellNo);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbtransporter);
		this.panel3.Controls.Add(this.txtDriverName);
		this.panel3.Controls.Add(this.txtVehicleNo);
		this.panel3.Controls.Add(this.label8);
		this.panel3.Controls.Add(this.label7);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add(this.txtGPDate);
		this.panel3.Controls.Add(this.DocDate);
		this.panel3.Controls.Add(this.txtdocno);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbInvoiceNo);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(3, 33);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1293, 205);
		this.panel3.TabIndex = 0;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.btnRecordsUpdate);
		this.panel12.Controls.Add(this.label36);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1293, 30);
		this.panel12.TabIndex = 101;
		this.btnRecordsUpdate.BackColor = System.Drawing.Color.FromArgb(192, 0, 0);
		this.btnRecordsUpdate.FlatAppearance.BorderColor = System.Drawing.Color.Black;
		this.btnRecordsUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnRecordsUpdate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRecordsUpdate.ForeColor = System.Drawing.Color.White;
		this.btnRecordsUpdate.Location = new System.Drawing.Point(222, 4);
		this.btnRecordsUpdate.Name = "btnRecordsUpdate";
		this.btnRecordsUpdate.Size = new System.Drawing.Size(159, 24);
		this.btnRecordsUpdate.TabIndex = 114;
		this.btnRecordsUpdate.Text = "Auto Update Records";
		this.btnRecordsUpdate.UseVisualStyleBackColor = false;
		this.btnRecordsUpdate.Visible = false;
		this.btnRecordsUpdate.Click += new System.EventHandler(btnRecordsUpdate_Click);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.ForeColor = System.Drawing.Color.White;
		this.label36.Location = new System.Drawing.Point(6, 4);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(245, 19);
		this.label36.TabIndex = 0;
		this.label36.Text = "Export Goods Forwarding      ";
		this.ChkPrintPreview.AutoSize = true;
		this.ChkPrintPreview.Checked = true;
		this.ChkPrintPreview.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPrintPreview.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrintPreview.Location = new System.Drawing.Point(1087, 177);
		this.ChkPrintPreview.Name = "ChkPrintPreview";
		this.ChkPrintPreview.Size = new System.Drawing.Size(95, 19);
		this.ChkPrintPreview.TabIndex = 31;
		this.ChkPrintPreview.Text = "Print Preview";
		this.ChkPrintPreview.UseVisualStyleBackColor = true;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(3, 95);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(72, 15);
		this.label18.TabIndex = 98;
		this.label18.Text = "Invoice No#";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(3, 40);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(88, 15);
		this.label5.TabIndex = 97;
		this.label5.Text = "Doc No && Date";
		this.txtNoofInvoices.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNoofInvoices.Enabled = false;
		this.txtNoofInvoices.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoofInvoices.Location = new System.Drawing.Point(1180, 91);
		this.txtNoofInvoices.Name = "txtNoofInvoices";
		this.txtNoofInvoices.Size = new System.Drawing.Size(105, 23);
		this.txtNoofInvoices.TabIndex = 29;
		this.label48.AutoSize = true;
		this.label48.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(1087, 95);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(86, 15);
		this.label48.TabIndex = 93;
		this.label48.Text = "No Of Invoices";
		this.txtInvoiceNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceNetWeight.Enabled = false;
		this.txtInvoiceNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceNetWeight.Location = new System.Drawing.Point(714, 91);
		this.txtInvoiceNetWeight.Name = "txtInvoiceNetWeight";
		this.txtInvoiceNetWeight.Size = new System.Drawing.Size(112, 23);
		this.txtInvoiceNetWeight.TabIndex = 17;
		this.txtInvoiceNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.InvoiceNetWeight.AutoSize = true;
		this.InvoiceNetWeight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.InvoiceNetWeight.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.InvoiceNetWeight.Location = new System.Drawing.Point(619, 95);
		this.InvoiceNetWeight.Name = "InvoiceNetWeight";
		this.InvoiceNetWeight.Size = new System.Drawing.Size(87, 15);
		this.InvoiceNetWeight.TabIndex = 93;
		this.InvoiceNetWeight.Text = "Do Net Weight";
		this.txtDoNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDoNetWeight.Enabled = false;
		this.txtDoNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDoNetWeight.Location = new System.Drawing.Point(714, 64);
		this.txtDoNetWeight.Name = "txtDoNetWeight";
		this.txtDoNetWeight.Size = new System.Drawing.Size(112, 23);
		this.txtDoNetWeight.TabIndex = 16;
		this.txtDoNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label2.AutoSize = true;
		this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(619, 68);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(94, 15);
		this.label2.TabIndex = 93;
		this.label2.Text = "Do Total Weight";
		this.txtDoGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDoGrossWeight.Enabled = false;
		this.txtDoGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDoGrossWeight.Location = new System.Drawing.Point(714, 36);
		this.txtDoGrossWeight.Name = "txtDoGrossWeight";
		this.txtDoGrossWeight.Size = new System.Drawing.Size(112, 23);
		this.txtDoGrossWeight.TabIndex = 15;
		this.txtDoGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label1.AutoSize = true;
		this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(619, 40);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(74, 15);
		this.label1.TabIndex = 91;
		this.label1.Text = "Do Gross Wt";
		this.txtBalFcWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBalFcWeight.Enabled = false;
		this.txtBalFcWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalFcWeight.Location = new System.Drawing.Point(714, 173);
		this.txtBalFcWeight.Name = "txtBalFcWeight";
		this.txtBalFcWeight.Size = new System.Drawing.Size(112, 23);
		this.txtBalFcWeight.TabIndex = 20;
		this.txtBalFcWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDispatchWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDispatchWeight.Enabled = false;
		this.txtDispatchWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDispatchWeight.Location = new System.Drawing.Point(714, 145);
		this.txtDispatchWeight.Name = "txtDispatchWeight";
		this.txtDispatchWeight.Size = new System.Drawing.Size(112, 23);
		this.txtDispatchWeight.TabIndex = 19;
		this.txtDispatchWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label49.AutoSize = true;
		this.label49.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(619, 177);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(65, 15);
		this.label49.TabIndex = 89;
		this.label49.Text = "Bal Weight";
		this.txtFactoryWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFactoryWeight.Enabled = false;
		this.txtFactoryWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFactoryWeight.Location = new System.Drawing.Point(714, 118);
		this.txtFactoryWeight.Name = "txtFactoryWeight";
		this.txtFactoryWeight.Size = new System.Drawing.Size(112, 23);
		this.txtFactoryWeight.TabIndex = 18;
		this.txtFactoryWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label43.AutoSize = true;
		this.label43.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(619, 149);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(73, 15);
		this.label43.TabIndex = 89;
		this.label43.Text = "Dispatch Wt";
		this.label35.AutoSize = true;
		this.label35.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(619, 122);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(65, 15);
		this.label35.TabIndex = 89;
		this.label35.Text = "Fct Weight";
		this.txtContainer2.BackColor = System.Drawing.Color.White;
		this.txtContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtContainer2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtContainer2.Location = new System.Drawing.Point(938, 173);
		this.txtContainer2.Name = "txtContainer2";
		this.txtContainer2.ReadOnly = true;
		this.txtContainer2.Size = new System.Drawing.Size(146, 23);
		this.txtContainer2.TabIndex = 26;
		this.txtContainer2.TabStop = false;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(830, 177);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(86, 15);
		this.label34.TabIndex = 87;
		this.label34.Text = "ContainerNo-2";
		this.txtconainer1.BackColor = System.Drawing.Color.White;
		this.txtconainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtconainer1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtconainer1.Location = new System.Drawing.Point(938, 145);
		this.txtconainer1.Name = "txtconainer1";
		this.txtconainer1.ReadOnly = true;
		this.txtconainer1.Size = new System.Drawing.Size(146, 23);
		this.txtconainer1.TabIndex = 25;
		this.txtconainer1.TabStop = false;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(830, 149);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(84, 15);
		this.label33.TabIndex = 85;
		this.label33.Text = "ContainerNo-1";
		this.chkdateisactive.AutoSize = true;
		this.chkdateisactive.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkdateisactive.Location = new System.Drawing.Point(1182, 41);
		this.chkdateisactive.Name = "chkdateisactive";
		this.chkdateisactive.Size = new System.Drawing.Size(87, 19);
		this.chkdateisactive.TabIndex = 28;
		this.chkdateisactive.Text = "Date Active";
		this.chkdateisactive.UseVisualStyleBackColor = true;
		this.chkdateisactive.CheckedChanged += new System.EventHandler(chkdateisactive_CheckedChanged);
		this.cmbCustomer.AllowNull = (DefaultableBoolean)1;
		this.cmbCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance238).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance239).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance239).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance239).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance239;
		((AppearanceBase)appearance240).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance240;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCustomer).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCustomer).Location = new System.Drawing.Point(93, 116);
		((System.Windows.Forms.Control)(object)this.cmbCustomer).Name = "cmbCustomer";
		((System.Windows.Forms.Control)(object)this.cmbCustomer).Size = new System.Drawing.Size(193, 26);
		((System.Windows.Forms.Control)(object)this.cmbCustomer).TabIndex = 5;
		this.cmbShippingAgent.AllowNull = (DefaultableBoolean)1;
		this.cmbShippingAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbShippingAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbShippingAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbShippingAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbShippingAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbShippingAgent).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbShippingAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbShippingAgent).Location = new System.Drawing.Point(93, 171);
		((System.Windows.Forms.Control)(object)this.cmbShippingAgent).Name = "cmbShippingAgent";
		((System.Windows.Forms.Control)(object)this.cmbShippingAgent).Size = new System.Drawing.Size(193, 26);
		((System.Windows.Forms.Control)(object)this.cmbShippingAgent).TabIndex = 7;
		this.txtShippedContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtShippedContainer.Enabled = false;
		this.txtShippedContainer.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtShippedContainer.Location = new System.Drawing.Point(938, 91);
		this.txtShippedContainer.Name = "txtShippedContainer";
		this.txtShippedContainer.Size = new System.Drawing.Size(146, 23);
		this.txtShippedContainer.TabIndex = 23;
		this.txtShippedContainer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(830, 95);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(105, 15);
		this.label25.TabIndex = 58;
		this.label25.Text = "Shipped Container";
		this.txtNoOfContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNoOfContainer.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoOfContainer.Location = new System.Drawing.Point(938, 118);
		this.txtNoOfContainer.Name = "txtNoOfContainer";
		this.txtNoOfContainer.Size = new System.Drawing.Size(146, 23);
		this.txtNoOfContainer.TabIndex = 24;
		this.txtNoOfContainer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(3, 149);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(80, 15);
		this.label16.TabIndex = 50;
		this.label16.Text = "Shipping Line";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(830, 122);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(96, 15);
		this.label24.TabIndex = 55;
		this.label24.Text = "No. Of Container";
		this.txtCompletedDate.CustomFormat = "dd-MMM-yy";
		this.txtCompletedDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCompletedDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtCompletedDate.Location = new System.Drawing.Point(1180, 64);
		this.txtCompletedDate.Name = "txtCompletedDate";
		this.txtCompletedDate.Size = new System.Drawing.Size(105, 23);
		this.txtCompletedDate.TabIndex = 27;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(1087, 68);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(93, 15);
		this.label26.TabIndex = 52;
		this.label26.Text = "Completed Date";
		this.txtOtherCharges.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherCharges.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOtherCharges.Location = new System.Drawing.Point(1180, 118);
		this.txtOtherCharges.Name = "txtOtherCharges";
		this.txtOtherCharges.Size = new System.Drawing.Size(105, 22);
		this.txtOtherCharges.TabIndex = 30;
		this.txtOtherCharges.TabStop = false;
		this.txtOtherCharges.Visible = false;
		this.cmbGpNo.AllowNull = (DefaultableBoolean)1;
		this.cmbGpNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbGpNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbGpNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbGpNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmbGpNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbGpNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbGpNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbGpNo).Location = new System.Drawing.Point(93, 62);
		((System.Windows.Forms.Control)(object)this.cmbGpNo).Name = "cmbGpNo";
		((System.Windows.Forms.Control)(object)this.cmbGpNo).Size = new System.Drawing.Size(80, 26);
		((System.Windows.Forms.Control)(object)this.cmbGpNo).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbGpNo).TextChanged += new System.EventHandler(cmbGpNo_TextChanged);
		((System.Windows.Forms.Control)(object)this.cmbGpNo).Leave += new System.EventHandler(cmbGpNo_Leave);
		this.cmbShippingLIne.AllowNull = (DefaultableBoolean)1;
		this.cmbShippingLIne.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbShippingLIne.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbShippingLIne.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbShippingLIne).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.cmbShippingLIne).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbShippingLIne).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbShippingLIne.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbShippingLIne).Location = new System.Drawing.Point(93, 143);
		((System.Windows.Forms.Control)(object)this.cmbShippingLIne).Name = "cmbShippingLIne";
		((System.Windows.Forms.Control)(object)this.cmbShippingLIne).Size = new System.Drawing.Size(193, 26);
		((System.Windows.Forms.Control)(object)this.cmbShippingLIne).TabIndex = 6;
		this.txtFreight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreight.Enabled = false;
		this.txtFreight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreight.Location = new System.Drawing.Point(391, 173);
		this.txtFreight.Name = "txtFreight";
		this.txtFreight.Size = new System.Drawing.Size(225, 23);
		this.txtFreight.TabIndex = 14;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(1087, 122);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(82, 15);
		this.label15.TabIndex = 46;
		this.label15.Text = "Other Charges";
		this.label15.Visible = false;
		this.cmbDestinationPort.AllowNull = (DefaultableBoolean)1;
		this.cmbDestinationPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDestinationPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDestinationPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDestinationPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Location = new System.Drawing.Point(938, 34);
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Name = "cmbDestinationPort";
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Size = new System.Drawing.Size(146, 26);
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).TabIndex = 21;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(830, 40);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(93, 15);
		this.label23.TabIndex = 47;
		this.label23.Text = "Destination Port";
		this.txtCNICNO.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCNICNO.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCNICNO.Location = new System.Drawing.Point(391, 91);
		this.txtCNICNO.Name = "txtCNICNO";
		this.txtCNICNO.Size = new System.Drawing.Size(225, 23);
		this.txtCNICNO.TabIndex = 10;
		this.cmbLoadingPort.AllowNull = (DefaultableBoolean)1;
		this.cmbLoadingPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLoadingPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLoadingPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLoadingPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Location = new System.Drawing.Point(938, 62);
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Name = "cmbLoadingPort";
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Size = new System.Drawing.Size(146, 26);
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).TabIndex = 22;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(830, 68);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(75, 15);
		this.label22.TabIndex = 41;
		this.label22.Text = "Loading Port";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(3, 122);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(58, 15);
		this.label21.TabIndex = 36;
		this.label21.Text = "Customer";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(289, 95);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(53, 15);
		this.label12.TabIndex = 37;
		this.label12.Text = "CNIC No";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(3, 177);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(90, 15);
		this.label19.TabIndex = 26;
		this.label19.Text = "Shipping Agent";
		this.label14.AutoSize = true;
		this.label14.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(289, 177);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(44, 15);
		this.label14.TabIndex = 40;
		this.label14.Text = "Freight";
		this.txtBilityNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBilityNo.Enabled = false;
		this.txtBilityNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBilityNo.Location = new System.Drawing.Point(505, 145);
		this.txtBilityNo.Name = "txtBilityNo";
		this.txtBilityNo.Size = new System.Drawing.Size(111, 23);
		this.txtBilityNo.TabIndex = 13;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(289, 40);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(80, 15);
		this.label11.TabIndex = 32;
		this.label11.Text = "Driver Cell No";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(289, 68);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(74, 15);
		this.label10.TabIndex = 25;
		this.label10.Text = "Driver Name";
		this.txtDriverCellNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDriverCellNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDriverCellNo.Location = new System.Drawing.Point(391, 36);
		this.txtDriverCellNo.Name = "txtDriverCellNo";
		this.txtDriverCellNo.Size = new System.Drawing.Size(225, 23);
		this.txtDriverCellNo.TabIndex = 8;
		this.cmbtransporter.AllowNull = (DefaultableBoolean)1;
		this.cmbtransporter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbtransporter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbtransporter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbtransporter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.cmbtransporter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbtransporter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbtransporter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbtransporter).Location = new System.Drawing.Point(391, 116);
		((System.Windows.Forms.Control)(object)this.cmbtransporter).Name = "cmbtransporter";
		((System.Windows.Forms.Control)(object)this.cmbtransporter).Size = new System.Drawing.Size(225, 26);
		((System.Windows.Forms.Control)(object)this.cmbtransporter).TabIndex = 11;
		this.txtDriverName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDriverName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDriverName.Location = new System.Drawing.Point(391, 64);
		this.txtDriverName.Name = "txtDriverName";
		this.txtDriverName.Size = new System.Drawing.Size(225, 23);
		this.txtDriverName.TabIndex = 9;
		this.txtVehicleNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleNo.Enabled = false;
		this.txtVehicleNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleNo.Location = new System.Drawing.Point(391, 145);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.Size = new System.Drawing.Size(112, 23);
		this.txtVehicleNo.TabIndex = 12;
		this.label8.AutoSize = true;
		this.label8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(289, 149);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(102, 15);
		this.label8.TabIndex = 65;
		this.label8.Text = "Vehicle && Bilty No";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(289, 122);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(67, 15);
		this.label7.TabIndex = 62;
		this.label7.Text = "Transporter";
		this.label4.AutoSize = true;
		this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(3, 68);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(85, 15);
		this.label4.TabIndex = 56;
		this.label4.Text = "GP NO. && Date";
		this.txtGPDate.CustomFormat = "dd-MMM-yy";
		this.txtGPDate.Enabled = false;
		this.txtGPDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGPDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtGPDate.Location = new System.Drawing.Point(175, 64);
		this.txtGPDate.Name = "txtGPDate";
		this.txtGPDate.Size = new System.Drawing.Size(111, 23);
		this.txtGPDate.TabIndex = 3;
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(175, 36);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(111, 23);
		this.DocDate.TabIndex = 1;
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(93, 36);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(80, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.TabStop = false;
		this.cmbInvoiceNo.AllowNull = (DefaultableBoolean)1;
		this.cmbInvoiceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbInvoiceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbInvoiceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbInvoiceNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Location = new System.Drawing.Point(93, 89);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Name = "cmbInvoiceNo";
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Size = new System.Drawing.Size(193, 26);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Leave += new System.EventHandler(cmbInvoiceNo_Leave);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1293, 30);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.BtnNew, this.btnRefresh, this.btnSave, this.btnUpdate, this.btnDelete, this.BtnPrint, this.btnAttachment, this.btnGenerateAvailableStock, this.btnStockReport, this.btnShortCutKey });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1293, 27);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.BtnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNew.Name = "BtnNew";
		this.BtnNew.Size = new System.Drawing.Size(59, 24);
		this.BtnNew.Text = "&New";
		this.BtnNew.Click += new System.EventHandler(BtnNew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.icons8_delete_bin_16;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.BtnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint.Name = "BtnPrint";
		this.BtnPrint.Size = new System.Drawing.Size(92, 24);
		this.BtnPrint.Text = "507-Print";
		this.BtnPrint.Click += new System.EventHandler(BtnPrint_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnGenerateAvailableStock.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnGenerateAvailableStock.Image = (System.Drawing.Image)resources.GetObject("btnGenerateAvailableStock.Image");
		this.btnGenerateAvailableStock.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGenerateAvailableStock.Name = "btnGenerateAvailableStock";
		this.btnGenerateAvailableStock.Size = new System.Drawing.Size(114, 24);
		this.btnGenerateAvailableStock.Text = "Generate Stock";
		this.btnGenerateAvailableStock.Click += new System.EventHandler(btnGenerateAvailableStock_Click);
		this.btnStockReport.Image = Architecture.WinApp.Properties.Resources.Fixed_Assets;
		this.btnStockReport.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnStockReport.Name = "btnStockReport";
		this.btnStockReport.Size = new System.Drawing.Size(117, 24);
		this.btnStockReport.Text = "Stock Report";
		this.btnStockReport.Click += new System.EventHandler(btnStockReport_Click);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel13);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryDetail);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1299, 720);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 155);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1293, 326);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 1;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label93);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel13.Location = new System.Drawing.Point(3, 481);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1293, 19);
		this.panel13.TabIndex = 47;
		this.label93.AutoSize = true;
		this.label93.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label93.ForeColor = System.Drawing.Color.White;
		this.label93.Location = new System.Drawing.Point(0, 1);
		this.label93.Name = "label93";
		this.label93.Size = new System.Drawing.Size(234, 17);
		this.label93.TabIndex = 2;
		this.label93.Text = "Detail of the Selected above Row";
		this.grdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		this.grdHistoryDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryDetail.DesignTimeLayout = grdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdHistoryDetail.DynamicFiltering = true;
		this.grdHistoryDetail.FilterMode = (FilterMode)2;
		this.grdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Location = new System.Drawing.Point(3, 500);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Name = "grdHistoryDetail";
		((ScrollableBase)this.grdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(1293, 217);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabIndex = 2;
		this.grdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label92);
		this.panel6.Controls.Add(this.ctrlGrdBar2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 132);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1293, 23);
		this.panel6.TabIndex = 0;
		this.label92.AutoSize = true;
		this.label92.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label92.ForeColor = System.Drawing.Color.White;
		this.label92.Location = new System.Drawing.Point(3, 2);
		this.label92.Name = "label92";
		this.label92.Size = new System.Drawing.Size(142, 19);
		this.label92.TabIndex = 55;
		this.label92.Text = "Filtered Records";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1244, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 23);
		this.ctrlGrdBar2.TabIndex = 54;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel10.Controls.Add(this.groupBox2);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 55);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1293, 77);
		this.panel10.TabIndex = 0;
		this.groupBox2.Controls.Add(this.label46);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory);
		this.groupBox2.Controls.Add(this.label44);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGpNoHistory);
		this.groupBox2.Controls.Add(this.rdapproveddate);
		this.groupBox2.Controls.Add(this.btnShowHistory);
		this.groupBox2.Controls.Add(this.rdmodifydate);
		this.groupBox2.Controls.Add(this.label31);
		this.groupBox2.Controls.Add(this.drdocdate);
		this.groupBox2.Controls.Add(this.rdentrydate);
		this.groupBox2.Controls.Add(this.label89);
		this.groupBox2.Controls.Add(this.label90);
		this.groupBox2.Controls.Add(this.label91);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox2.Controls.Add(this.ToDocNoHistory);
		this.groupBox2.Controls.Add(this.FromDocNoHistory);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(4, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1025, 72);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(480, 47);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(65, 15);
		this.label46.TabIndex = 91;
		this.label46.Text = "Invoice No";
		this.CmbInvoiceNoHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance193).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance193).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance193;
		((AppearanceBase)appearance194).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance194;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance196).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance196).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance196).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance198).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance198;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance199).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance202;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance203).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance204).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance204).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance204).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance204).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance204;
		((AppearanceBase)appearance205).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInvoiceNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Location = new System.Drawing.Point(549, 41);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Name = "CmbInvoiceNoHistory";
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Size = new System.Drawing.Size(223, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).TabIndex = 6;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(211, 47);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(67, 15);
		this.label44.TabIndex = 89;
		this.label44.Text = "Gate Pass #";
		this.CmbGpNoHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbGpNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGpNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGpNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance148;
		((SpecialBoxBase)((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance153).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGpNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbGpNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbGpNoHistory).Location = new System.Drawing.Point(319, 41);
		((System.Windows.Forms.Control)(object)this.CmbGpNoHistory).Name = "CmbGpNoHistory";
		((System.Windows.Forms.Control)(object)this.CmbGpNoHistory).Size = new System.Drawing.Size(157, 26);
		((System.Windows.Forms.Control)(object)this.CmbGpNoHistory).TabIndex = 4;
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(918, 45);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 11;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(773, 41);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(59, 26);
		this.btnShowHistory.TabIndex = 7;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(918, 18);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 10;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(480, 20);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(58, 15);
		this.label31.TabIndex = 87;
		this.label31.Text = "Customer";
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(837, 18);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 8;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(837, 45);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 9;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.label89.AutoSize = true;
		this.label89.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label89.Location = new System.Drawing.Point(211, 20);
		this.label89.Name = "label89";
		this.label89.Size = new System.Drawing.Size(107, 15);
		this.label89.TabIndex = 85;
		this.label89.Text = "From && To Doc No";
		this.label90.AutoSize = true;
		this.label90.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label90.Location = new System.Drawing.Point(3, 47);
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
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance134;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(549, 14);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(283, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 5;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(398, 16);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(319, 16);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(69, 43);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(138, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(69, 16);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(138, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label69);
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1293, 52);
		this.panel8.TabIndex = 0;
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.ForeColor = System.Drawing.Color.White;
		this.label69.Location = new System.Drawing.Point(3, 30);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(223, 19);
		this.label69.TabIndex = 54;
		this.label69.Text = "Export Forwarding History";
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.toolStripSeparator1, this.btnResetHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1293, 27);
		this.toolStrip2.TabIndex = 53;
		this.toolStrip2.Text = "toolStrip2";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
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
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1307, 749);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "EximForwarding";
		this.Text = "Forwarding";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(EximForwarding_FormClosing);
		base.Load += new System.EventHandler(ExpfrmForwarding_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ExpfrmForwarding_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdgpInfo).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.panel15.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUomInner).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUomMaster).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcontainerNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).EndInit();
		this.tabPage4.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdotheritems).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOtherItem).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbShippingAgent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGpNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbShippingLIne).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDestinationPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoadingPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbtransporter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbInvoiceNo).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNoHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGpNoHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
