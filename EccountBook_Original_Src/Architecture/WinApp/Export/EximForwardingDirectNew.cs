using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Contractor_Wages;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Export;

public class EximForwardingDirectNew : Form
{
	private int RecId;

	private int inVoiceId;

	private bool UpdateMode;

	private int updateDetailIndex = -1;

	private int updateDetailIndexOther = -1;

	private int DefaultDaysToLessFromHistoryFromDate;

	private DataTable dtitem = new DataTable();

	private DataTable dtDetail = new DataTable();

	private DataTable dtUomInner = new DataTable();

	private DataTable dtitemOther = new DataTable();

	private DataTable dtUomMaster = new DataTable();

	private DataTable dtOtherItemDetail = new DataTable();

	private bool WagesCompulsoryOnForwarding;

	private bool WagesActiveOrInActive;

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formRights = new Rightsobjects();

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

	internal UltraCombo cmbShippingLIne;

	private TextBox txtFreight;

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

	private Button btnDetailCancel;

	private Button btnDetailUpdate;

	internal UltraCombo CmbJobLot;

	private Label label55;

	internal UltraCombo CmbWareHouse;

	private Label label54;

	private ToolStripButton btnRefresh;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

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

	private TextBox txtOContainer;

	private Label label42;

	private TextBox txtoRate;

	private Label label63;

	private Label label45;

	private TextBox txtStockWeight;

	private TextBox txtAddLess;

	private Label label47;

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

	private Label label69;

	private Label label92;

	private Label label93;

	private ToolStripButton btnDelete;

	private Button btnRecordsUpdate;

	private TextBox cmbGpNo;

	private TextBox cmbcontainerNo;

	private RadioButton rdbtnItemCode;

	private RadioButton rdbtnItemName;

	private Label label2;

	private TextBox txtRemarksHEader;

	private Label label1;

	public EximForwardingDirectNew(UserAccount user)
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
			base.AutoScrollMinSize = new Size(1180, 700);
			formRights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formRights.DoHaveSaveRight;
			btnUpdate.Enabled = formRights.DoHaveUpdateRights;
			BtnPrint.Enabled = formRights.DoHavePrintRights;
			btnDelete.Enabled = formRights.DoHaveCanDelete;
			WagesCompulsoryOnForwarding = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("WagesCompulsoryOnForwarding"));
			WagesRefDocumentsStatus obj = clsGlobalVariables.WagesRefDocumentsStatusList.Find((WagesRefDocumentsStatus row) => row.RefDocumentTypeId == 206);
			WagesActiveOrInActive = obj != null && obj.IsActive;
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName", typeof(string));
			dtitem.Columns.Add("ItemCode", typeof(string));
			dtitemOther.Columns.Add("Id", typeof(int));
			dtitemOther.Columns.Add("ItemName", typeof(string));
			dtUomMaster.Columns.Add("Id", typeof(int));
			dtUomMaster.Columns.Add("Code", typeof(string));
			dtUomMaster.Columns.Add("Equivalent", typeof(double));
			dtUomInner.Columns.Add("Id", typeof(int));
			dtUomInner.Columns.Add("Code", typeof(string));
			dtUomInner.Columns.Add("Equivalent", typeof(double));
			dtDetail.Columns.Add("Id", typeof(int));
			dtDetail.Columns.Add("InvoiceId", typeof(int));
			dtDetail.Columns.Add("InvoiceDetailId", typeof(int));
			dtDetail.Columns.Add("ContractId", typeof(int));
			dtDetail.Columns.Add("ContractDetailId", typeof(int));
			dtDetail.Columns.Add("Contract");
			dtDetail.Columns.Add("WarehouseId", typeof(int));
			dtDetail.Columns.Add("Warehouse");
			dtDetail.Columns.Add("ItemId", typeof(int));
			dtDetail.Columns.Add("ItemCode");
			dtDetail.Columns.Add("ItemName");
			dtDetail.Columns.Add("ItemDescription");
			dtDetail.Columns.Add("CropYearId", typeof(int));
			dtDetail.Columns.Add("CropYear");
			dtDetail.Columns.Add("JobLotId", typeof(int));
			dtDetail.Columns.Add("JobLot");
			dtDetail.Columns.Add("PackTypeId", typeof(int));
			dtDetail.Columns.Add("Packtype");
			dtDetail.Columns.Add("MasterQTY", typeof(double));
			dtDetail.Columns.Add("MasterUOMId", typeof(int));
			dtDetail.Columns.Add("MasterUOM");
			dtDetail.Columns.Add("MasterEquivalent", typeof(double));
			dtDetail.Columns.Add("EbUnitMaster", typeof(double));
			dtDetail.Columns.Add("EbTotalMaster", typeof(double));
			dtDetail.Columns.Add("InnerQTY", typeof(double));
			dtDetail.Columns.Add("InnerUOMId", typeof(int));
			dtDetail.Columns.Add("InnerUOM");
			dtDetail.Columns.Add("InnerEquivalent", typeof(double));
			dtDetail.Columns.Add("EbUnitInner", typeof(double));
			dtDetail.Columns.Add("EbTotalInner", typeof(double));
			dtDetail.Columns.Add("AddLess", typeof(double));
			dtDetail.Columns.Add("NetWeight", typeof(double));
			dtDetail.Columns.Add("GrossWeight", typeof(double));
			dtDetail.Columns.Add("StockWeight", typeof(double));
			dtDetail.Columns.Add("Container#");
			dtDetail.Columns.Add("Seal#");
			grdDetail.DataSource = dtDetail;
			grdDetail.RetrieveStructure();
			GridDetailSetting();
			dtOtherItemDetail.Columns.Add("Id", typeof(int));
			dtOtherItemDetail.Columns.Add("ExImInvoiceId", typeof(int));
			dtOtherItemDetail.Columns.Add("InvoiceOtherItemsDetailId", typeof(int));
			dtOtherItemDetail.Columns.Add("ItemId", typeof(int));
			dtOtherItemDetail.Columns.Add("ItemName");
			dtOtherItemDetail.Columns.Add("Qty", typeof(double));
			dtOtherItemDetail.Columns.Add("Rate", typeof(double));
			dtOtherItemDetail.Columns.Add("Amount", typeof(double));
			dtOtherItemDetail.Columns.Add("ContainerNo");
			dtOtherItemDetail.Columns.Add("Remarks");
			grdotheritems.DataSource = dtOtherItemDetail;
			grdotheritems.RetrieveStructure();
			grdSettingsOtherItems();
			generateCode();
			bindInvoiceNo();
			ShippingAgentFill();
			bindSeaPort();
			WarehouseFill();
			ItemFillForExport();
			CropYearFill();
			JobLotFill();
			packingType();
			cmbInvoiceNo.Focus();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			txtCompletedDate.Checked = false;
			btnDetailUpdate.Visible = false;
			btnDetailCancel.Visible = false;
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
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
				DocumentTypeId = 206
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

	private void bindInvoiceNo()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(cmbInvoiceNo.Value);
			DataTable dtInvoice = ExImInvoice.GetInvoicesForForwarding(new ExImInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = RecId
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
			if (dt.Rows.Count > 0)
			{
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
				if (dt.Rows.Count == 1)
				{
					((UltraGridBase)cmbInvoiceNo).Rows[1].Activate();
				}
				cmbInvoiceNo_Leave(null, null);
			}
			else
			{
				((Control)(object)cmbInvoiceNo).Text = string.Empty;
				((UltraGridBase)cmbInvoiceNo).DataSource = null;
			}
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
			if (((UltraGridBase)cmbInvoiceNo).ActiveRow != null && Conversion.ToInt(cmbInvoiceNo.Value) != 0)
			{
				GetDetailDataByInvoiceId(Conversion.ToInt(cmbInvoiceNo.Value));
				FillOtherItemDataByInvoiceId(Conversion.ToInt(cmbInvoiceNo.Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailDataByInvoiceId(int InvoiceId)
	{
		try
		{
			txtShippedContainer.Text = string.Empty;
			dtUomMaster.Rows.Clear();
			dtUomInner.Rows.Clear();
			double TotalNetWeight = 0.0;
			double TotalGrossWeight = 0.0;
			if (grdDetail.GetRows().Count() <= 0)
			{
				inVoiceId = Conversion.ToInt(cmbInvoiceNo.Value);
			}
			if (inVoiceId > 0 && Conversion.ToInt(cmbInvoiceNo.Value) != inVoiceId && grdDetail.GetRows().Count() > 0)
			{
				if (MessageBox.Show("Are you sure Change Invoice?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					cmbInvoiceNo.Value = inVoiceId;
					return;
				}
				grdotheritems.ClearStructure();
				dtOtherItemDetail.Rows.Clear();
				grdDetail.ClearStructure();
				dtDetail.Rows.Clear();
			}
			DataTable dt = ExImInvoice.InvoiceDetailForExportForwarding(UserAccount.OrganizationId, UserAccount.CompanyId, InvoiceId);
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtcustomer = new DataTable();
			dtcustomer.Columns.Add("SupplierCustomerId", typeof(int));
			dtcustomer.Columns.Add("Customer");
			int count = 0;
			bool flag = false;
			int i;
			for (i = 0; i < dt.Rows.Count; i++)
			{
				TotalNetWeight += Conversion.ToDouble(dt.Rows[i]["NetWeight"]);
				TotalGrossWeight += Conversion.ToDouble(dt.Rows[i]["GrossWeight"]);
				if (!dtcustomer.AsEnumerable().Any((DataRow row) => row.Field<int>("SupplierCustomerId") == Conversion.ToInt(dt.Rows[i]["SupplierCustomerId"])))
				{
					count = i;
					dtcustomer.Rows.Add(dt.Rows[i]["SupplierCustomerId"], dt.Rows[i]["CustomerName"]);
				}
				if (!dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(dt.Rows[i]["ItemId"])))
				{
					dtitem.Rows.Add(dt.Rows[i]["ItemId"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCode"]);
				}
				if (!dtUomMaster.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(dt.Rows[i]["OuterQtyUomId"])))
				{
					dtUomMaster.Rows.Add(dt.Rows[i]["OuterQtyUomId"], dt.Rows[i]["OuterUOM"], dt.Rows[i]["OuterEquivalent"]);
				}
				if (!dtUomInner.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(dt.Rows[i]["InnerQtyUomId"])))
				{
					dtUomInner.Rows.Add(dt.Rows[i]["InnerQtyUomId"], dt.Rows[i]["UomCodeInner"], dt.Rows[i]["InnerEquivalent"]);
				}
				if (grdDetail.GetRows().Length != 0)
				{
					GridEXRow[] rows = grdDetail.GetRows();
					for (int j = 0; j < rows.Length; j++)
					{
						if (Conversion.ToInt(rows[j].Cells["InvoiceDetailId"].Value) == Conversion.ToInt(dt.Rows[i]["InvoiceDetailId"]))
						{
							flag = true;
							break;
						}
					}
				}
				if (RecId == 0 && !btnUpdate.Visible && !flag)
				{
					dtDetail.Rows.Add(0, dt.Rows[i]["ExImInvoiceId"], dt.Rows[i]["InvoiceDetailId"], dt.Rows[i]["ContractId"], dt.Rows[i]["ContractDetailId"], dt.Rows[i]["ContractNo"], 0, "", dt.Rows[i]["ItemId"], dt.Rows[i]["ItemCode"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemDiscription"], dt.Rows[i]["CropYearId"], dt.Rows[i]["CropYear"], dt.Rows[i]["JobLotId"], dt.Rows[i]["JobLot"], dt.Rows[i]["PackingMaterialTypeId"], dt.Rows[i]["PackingType"], dt.Rows[i]["OuterQty"], dt.Rows[i]["OuterQtyUomId"], dt.Rows[i]["OuterUOM"], dt.Rows[i]["OuterEquivalent"], Conversion.ToDouble(dt.Rows[i]["EbUnit"]), Conversion.ToDouble(dt.Rows[i]["EbTotal"]), dt.Rows[i]["InnerQty"], dt.Rows[i]["InnerQtyUomId"], dt.Rows[i]["UomCodeInner"], dt.Rows[i]["InnerEquivalent"], Conversion.ToDouble(dt.Rows[i]["InnerEbUnit"]), Conversion.ToDouble(dt.Rows[i]["InnerEbTotal"]), 0, Conversion.ToDouble(dt.Rows[i]["NetWeight"]), Conversion.ToDouble(dt.Rows[i]["GrossWeight"]), Conversion.ToDouble(dt.Rows[i]["GrossWeight"]), "", "");
				}
				flag = false;
			}
			grdDetail.DataSource = dtDetail;
			grdDetail.RetrieveStructure();
			GridDetailSetting();
			DataRow dr = dt.Rows[count];
			cmbLoadingPort.Value = Conversion.ToInt(dr["LoadingPortId"]);
			cmbDestinationPort.Value = Conversion.ToInt(dr["DestinationPortId"]);
			txtShippedContainer.Text = Conversion.ToInt(dr["ShippedContainer"]).ToString();
			if (dtcustomer.Rows.Count > 0)
			{
				DDL.BindDDL(dtcustomer, cmbCustomer, "SupplierCustomerId", "Customer", "Customer", ZeroIndex: true);
				if (dtcustomer.Rows.Count > 1)
				{
					((UltraGridBase)cmbCustomer).Rows[1].Activate();
				}
			}
			if (dtitem.Rows.Count > 0)
			{
				DDL.BindDDL(dtitem, cmbItem, "Id", "ItemName", "Item", ZeroIndex: true);
				((UltraGridBase)cmbItem).Rows[1].Activate();
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 350;
			}
			if (dtUomInner.Rows.Count > 0)
			{
				DDL.BindDDL(dtUomInner, CmbPackUomInner, "Id", "Code", "Inner Uom", ZeroIndex: false);
				((UltraGridBase)CmbPackUomInner).Rows[0].Activate();
				((UltraGridBase)CmbPackUomInner).DisplayLayout.Bands[0].Columns[1].Width = 450;
			}
			if (dtUomMaster.Rows.Count > 0)
			{
				DDL.BindDDL(dtUomMaster, CmbPackUomMaster, "Id", "Code", "Master Uom", ZeroIndex: false);
				((UltraGridBase)CmbPackUomMaster).Rows[0].Activate();
				((UltraGridBase)CmbPackUomMaster).DisplayLayout.Bands[0].Columns[1].Width = 450;
				if (dtUomInner.Rows.Count == 0)
				{
					DDL.BindDDL(dtUomMaster, CmbPackUomInner, "Id", "Code", "Inner Uom", ZeroIndex: false);
					((UltraGridBase)CmbPackUomInner).Rows[0].Activate();
					((UltraGridBase)CmbPackUomInner).DisplayLayout.Bands[0].Columns[1].Width = 450;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillOtherItemDataByInvoiceId(int InvoiceId)
	{
		try
		{
			if (InvoiceId <= 0)
			{
				return;
			}
			ExImInvoice ex = ExImInvoice.GetByID(InvoiceId);
			DataTable dtOItem = new DataTable();
			dtOItem.Columns.Add("Id", typeof(int));
			dtOItem.Columns.Add("ItemName", typeof(string));
			dtOtherItemDetail.Rows.Clear();
			bool flag = false;
			int i;
			for (i = 0; i < ex.ExImInvoiceOtherItemslist.Count; i++)
			{
				if (!dtOItem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ex.ExImInvoiceOtherItemslist[i].otherItemId))
				{
					dtOItem.Rows.Add(ex.ExImInvoiceOtherItemslist[i].otherItemId, ex.ExImInvoiceOtherItemslist[i].ItemName);
				}
				if (grdotheritems.GetRows().Length != 0)
				{
					GridEXRow[] rows = grdotheritems.GetRows();
					for (int j = 0; j < rows.Length; j++)
					{
						if (Conversion.ToInt(rows[j].Cells["InvoiceOtherItemsDetailId"].Value) == Conversion.ToInt((object)ex.ExImInvoiceOtherItemslist[i].Id))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					dtOtherItemDetail.Rows.Add(0, ex.ExImInvoiceOtherItemslist[i].ExImInvoiceId, ex.ExImInvoiceOtherItemslist[i].Id, ex.ExImInvoiceOtherItemslist[i].otherItemId, ex.ExImInvoiceOtherItemslist[i].ItemName, ex.ExImInvoiceOtherItemslist[i].oItemQty, ex.ExImInvoiceOtherItemslist[i].oItemRate, ex.ExImInvoiceOtherItemslist[i].oItemAmount, "", ex.ExImInvoiceOtherItemslist[i].OtherItemRemarks);
				}
				flag = false;
			}
			DDL.BindDDL(dtOItem, CmbOtherItem, "Id", "ItemName", "Item Name", ZeroIndex: false);
			((UltraGridBase)CmbOtherItem).DisplayLayout.Bands[0].Columns[1].Width = 400;
			grdotheritems.DataSource = dtOtherItemDetail;
			grdotheritems.RetrieveStructure();
			grdSettingsOtherItems();
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
		}
	}

	private void ItemFillForExport()
	{
		try
		{
			int Id = Conversion.ToInt(cmbItem.Value);
			DataTable dt = CommonServices.GetExportItemsByOrganizationCompanyIdForComboServiceBind();
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtitem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCode"]);
				}
			}
			if (dtitem.Rows.Count > 0)
			{
				if (rdbtnItemName.Checked)
				{
					DDL.BindDDL(dtitem, cmbItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, cmbItem, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 300;
				if (Id > 0)
				{
					if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						cmbItem.Value = Id;
					}
					else
					{
						((Control)(object)cmbItem).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbItem).Text = string.Empty;
				((UltraGridBase)cmbItem).DataSource = null;
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
			string Uom = Conversion.ToString((object)((Control)(object)CmbPackUomInner).Text);
			DataTable dtUom = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(cmbItem.Value));
			if (dtUom.Rows.Count > 0)
			{
				for (int i = 0; i < dtUom.Rows.Count; i++)
				{
					dtUomInner.Rows.Add(dtUom.Rows[i]["Id"], dtUom.Rows[i]["UOMCode"], dtUom.Rows[i]["Equivalent"]);
				}
			}
			if (dtUomInner.Rows.Count > 0)
			{
				DDL.BindDDL(dtUomInner, CmbPackUomInner, "Id", "Code", "PackUom", ZeroIndex: false);
				if (!string.IsNullOrEmpty(Uom))
				{
					if (dtUomInner.AsEnumerable().Any((DataRow row) => row.Field<string>("Code") == Uom))
					{
						((Control)(object)CmbPackUomInner).Text = Uom;
					}
					else
					{
						((Control)(object)CmbPackUomInner).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbPackUomInner).Text = string.Empty;
				((UltraGridBase)CmbPackUomInner).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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
			int Id = Conversion.ToInt(CmbJobLot.Value);
			DataTable lst = CommonServices.JobLotGetAllService();
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDLNew(lst, CmbJobLot, "Id", "JobLotDescription", "JobLot Description", true);
				if (Id > 0)
				{
					if (lst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
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
				((Control)(object)CmbJobLot).Text = string.Empty;
				((UltraGridBase)CmbJobLot).DataSource = null;
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
			int Id = Conversion.ToInt(CmbCropYear.Value);
			DataTable dt = CommonServices.CropYearGetAllService();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbCropYear, "Id", "CropYear", "CropYear", true);
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
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
				((Control)(object)CmbCropYear).Text = string.Empty;
				((UltraGridBase)CmbCropYear).DataSource = null;
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
			int Id = Conversion.ToInt(CmbWareHouse.Value);
			DataTable dtwh = CommonServices.getActiveWareHouse();
			if (dtwh.Rows.Count > 0)
			{
				DDL.BindDDL(dtwh, CmbWareHouse, "Id", "WareHouseName", "Godown Name", ZeroIndex: true);
				if (Id > 0)
				{
					if (dtwh.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbWareHouse.Value = Id;
					}
					else
					{
						((Control)(object)CmbWareHouse).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbWareHouse).Text = string.Empty;
				((UltraGridBase)CmbWareHouse).DataSource = null;
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
			int Id = Conversion.ToInt(cmbPackType.Value);
			DataTable dt = InvPackingType.Getall();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbPackType, "Id", "PackTypeDesc", "Pack Type ", true);
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						cmbPackType.Value = Id;
					}
					else
					{
						((Control)(object)cmbPackType).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbPackType).Text = string.Empty;
				((UltraGridBase)cmbPackType).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindOtherItems()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(CmbOtherItem.Value);
			DataTable dt = ExImInvoice.GetOtherItemByExImInvoiceId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(cmbInvoiceNo.Value)
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtitemOther.Rows.Add(dt.Rows[i]["ItemId"], dt.Rows[i]["ItemName"]);
				}
			}
			if (dtitemOther.Rows.Count > 0)
			{
				DDL.BindDDL(dtitemOther, CmbOtherItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				((UltraGridBase)CmbOtherItem).DisplayLayout.Bands[0].Columns[1].Width = 400;
				((UltraGridBase)CmbOtherItem).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				if (Id > 0)
				{
					if (dtitemOther.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbOtherItem.Value = Id;
					}
					else
					{
						((Control)(object)CmbOtherItem).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbOtherItem).Text = string.Empty;
				((UltraGridBase)CmbOtherItem).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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
		if (cmbGpNo.Text.Trim() == "" || Conversion.ToInt((object)cmbGpNo.Text) == 0)
		{
			MessageBox.Show("Please Enter GatePass No");
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
		return true;
	}

	private bool formdetailvalidation()
	{
		if (((UltraGridBase)CmbWareHouse).ActiveRow == null || Conversion.ToInt(CmbWareHouse.Value) == 0)
		{
			MessageBox.Show("Please Select Warehouse");
			CmbWareHouse.Focus();
			return false;
		}
		if (((UltraGridBase)cmbItem).ActiveRow == null || Conversion.ToInt(cmbItem.Value) == 0)
		{
			MessageBox.Show("Please select Item");
			cmbItem.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCropYear).ActiveRow == null || Conversion.ToInt(CmbCropYear.Value) == 0)
		{
			MessageBox.Show("Please Select CropYear");
			CmbCropYear.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobLot).ActiveRow == null || Conversion.ToInt(CmbJobLot.Value) == 0)
		{
			MessageBox.Show("Please Select Job Lot");
			CmbJobLot.Focus();
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
		if (cmbcontainerNo.Text.Trim() == "" || cmbcontainerNo.Text == "0")
		{
			MessageBox.Show("Please Select  Container No");
			cmbcontainerNo.Focus();
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

	private bool OtherItemFormValidation()
	{
		if (((UltraGridBase)CmbOtherItem).ActiveRow == null || Conversion.ToInt(CmbOtherItem.Value) == 0)
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
			throw new Exception("you can not Add Record Manually");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdDetail.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count <= 0)
			{
				return;
			}
			updateDetailIndex = item.RowIndex;
			if (RecId > 0)
			{
				dtitem.Clear();
				((Control)(object)cmbItem).Text = item.Cells["ItemName"].Text.ToString();
				dtitem.Rows.Add(item.Cells["ItemId"].Value, item.Cells["ItemCode"].Text.ToString(), item.Cells["ItemName"].Text.ToString());
				if (dtitem.Rows.Count > 0)
				{
					DDL.BindDDL(dtitem, cmbItem, "Id", "ItemName", "ItemName", ZeroIndex: false);
					((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 450;
				}
				dtUomMaster.Clear();
				dtUomMaster.Rows.Add(Conversion.ToInt(item.Cells["MasterUOMId"].Value), item.Cells["MasterUOM"].Text.ToString(), Conversion.ToDouble((object)item.Cells["MasterEquivalent"].Text));
				if (dtUomMaster.Rows.Count > 0)
				{
					DDL.BindDDL(dtUomMaster, CmbPackUomMaster, "Id", "Code", "Master UOM", ZeroIndex: false);
					((UltraGridBase)CmbPackUomMaster).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
				DDL.BindDDL(dtUomInner, CmbPackUomInner, "Id", "Code", "Master UOM", ZeroIndex: false);
				((UltraGridBase)CmbPackUomInner).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			CmbWareHouse.Value = item.Cells["WarehouseId"].Value.ToString();
			((Control)(object)CmbWareHouse).Text = item.Cells["Warehouse"].Text.ToString();
			cmbItem.Value = Conversion.ToInt(item.Cells["ItemId"].Value);
			cmbItem_Leave(null, null);
			CmbCropYear.Value = item.Cells["CropYearId"].Value.ToString();
			CmbJobLot.Value = item.Cells["JobLotId"].Value.ToString();
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
			cmbcontainerNo.Text = item.Cells["Container#"].Value.ToString();
			txtSeal.Text = item.Cells["Seal#"].Value.ToString();
			btnDetailUpdate.Visible = true;
			btnDetailCancel.Visible = true;
			btnAddinGrid.Visible = false;
			CmbWareHouse.Focus();
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
				dtDetail.Rows[updateDetailIndex]["WarehouseId"] = Conversion.ToInt(CmbWareHouse.Value);
				dtDetail.Rows[updateDetailIndex]["Warehouse"] = Conversion.ToString((object)((Control)(object)CmbWareHouse).Text);
				dtDetail.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(cmbItem.Value);
				dtDetail.Rows[updateDetailIndex]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)cmbItem).SelectedRow.Cells["ItemCode"].Value);
				dtDetail.Rows[updateDetailIndex]["ItemName"] = Conversion.ToString(((UltraDropDownBase)cmbItem).SelectedRow.Cells["ItemName"].Value);
				dtDetail.Rows[updateDetailIndex]["ItemDescription"] = Conversion.ToString((object)txtItemDesc.Text);
				dtDetail.Rows[updateDetailIndex]["JobLotId"] = Conversion.ToInt(CmbJobLot.Value);
				dtDetail.Rows[updateDetailIndex]["JobLot"] = Conversion.ToString((object)((Control)(object)CmbJobLot).Text);
				dtDetail.Rows[updateDetailIndex]["CropYearId"] = Conversion.ToInt(CmbCropYear.Value);
				dtDetail.Rows[updateDetailIndex]["CropYear"] = Conversion.ToString((object)((Control)(object)CmbCropYear).Text);
				dtDetail.Rows[updateDetailIndex]["PackTypeId"] = Conversion.ToInt(cmbPackType.Value);
				dtDetail.Rows[updateDetailIndex]["Packtype"] = Conversion.ToString((object)((Control)(object)cmbPackType).Text);
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
				dtDetail.Rows[updateDetailIndex]["Container#"] = Conversion.ToString((object)cmbcontainerNo.Text);
				dtDetail.Rows[updateDetailIndex]["Seal#"] = Conversion.ToString((object)txtSeal.Text);
				grdDetail.DataSource = dtDetail;
				grdDetail.RetrieveStructure();
				GridDetailSetting();
				btnDetailUpdate.Visible = false;
				btnDetailCancel.Visible = false;
				btnAddinGrid.Visible = false;
				FormDetailReset();
				CmbWareHouse.Focus();
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
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
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
					d.InvoiceId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
					d.InvoiceDetailId = Conversion.ToInt(r.Cells["InvoiceDetailId"].Value);
					d.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					d.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					d.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
					d.LotJobId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					d.PackingMaterialId = Conversion.ToInt(r.Cells["PackTypeId"].Value);
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
					d.ContainerNo = Conversion.ToString((object)r.Cells["Container#"].Text.Trim());
					d.ContainerNo = Conversion.ToString((object)r.Cells["Container#"].Text.Trim());
					d.SealNo = Conversion.ToString((object)r.Cells["Seal#"].Text.Trim());
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
			grdDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailSetting()
	{
		try
		{
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["InvoiceId"].Visible = false;
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
			grdDetail.RootTable.Columns["Id"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["InvoiceId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["InvoiceDetailId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ContractId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ContractDetailId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Contract"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["WarehouseId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Warehouse"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ItemDescription"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CropYearId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["PackTypeId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Packtype"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["NetWeight"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["MasterUOM"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["MasterQTY"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["EbUnitMaster"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["EbTotalMaster"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["InnerUOM"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["InnerQTY"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["EbUnitInner"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["EbTotalInner"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["AddLess"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["StockWeight"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Container#"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Seal#"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["StockWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["GrossWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetWeight"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["MasterQTY"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["MasterQTY"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["MasterQTY"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["MasterQTY"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["InnerQTY"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["InnerQTY"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["InnerQTY"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["InnerQTY"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["EbUnitInner"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["EbUnitInner"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["EbUnitInner"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["EbUnitInner"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["EbTotalInner"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["EbTotalInner"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["EbTotalInner"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["EbTotalInner"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["EbTotalMaster"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["EbTotalMaster"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["EbTotalMaster"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["EbTotalMaster"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["EbUnitMaster"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["EbUnitMaster"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["EbUnitMaster"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["EbUnitMaster"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["AddLess"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["AddLess"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["AddLess"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["AddLess"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(grdDetail);
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
			grdDetail.FrozenColumns = 2;
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

	private void btnoadd_Click(object sender, EventArgs e)
	{
		try
		{
			throw new Exception("you can not Add Record Manually");
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
				dtOtherItemDetail.Rows[updateDetailIndexOther]["ItemId"] = CmbOtherItem.Value;
				dtOtherItemDetail.Rows[updateDetailIndexOther]["ItemName"] = ((Control)(object)CmbOtherItem).Text;
				dtOtherItemDetail.Rows[updateDetailIndexOther]["Qty"] = txtoQty.Text;
				dtOtherItemDetail.Rows[updateDetailIndexOther]["Rate"] = txtoRate.Text;
				dtOtherItemDetail.Rows[updateDetailIndexOther]["Amount"] = txtoAmount.Text;
				dtOtherItemDetail.Rows[updateDetailIndexOther]["ContainerNo"] = txtOContainer.Text;
				dtOtherItemDetail.Rows[updateDetailIndexOther]["Remarks"] = txtoRemarks.Text;
				grdotheritems.DataSource = dtOtherItemDetail;
				btnoadd.Visible = true;
				btnoUpdate.Visible = false;
				btnoCancel.Visible = false;
				ResetOtherItems();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdotheritems_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdotheritems.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
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
				CmbOtherItem.Focus();
			}
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
			grdotheritems.RootTable.Columns["Id"].Visible = false;
			grdotheritems.RootTable.Columns["ExImInvoiceId"].Visible = false;
			grdotheritems.RootTable.Columns["InvoiceOtherItemsDetailId"].Visible = false;
			grdotheritems.RootTable.Columns["ItemId"].Visible = false;
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
			CommonServices.GridAutoAdjustmentNew(grdotheritems);
			grdotheritems.RootTable.Columns["ItemName"].Width = 350;
			grdotheritems.RootTable.Columns["Remarks"].Width = 350;
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
			ResetOtherItems();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool AutoUpdateRecord(int UpdateRecID)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Expected O, but got Unknown
		//IL_0b91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b98: Expected O, but got Unknown
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
			Conversion.ToDouble((object)grdDetail.GetTotal(grdDetail.RootTable.Columns["NetWeight"], (AggregateFunction)2).ToString());
			Conversion.ToDouble((object)grdDetail.GetTotal(grdDetail.RootTable.Columns["StockWeight"], (AggregateFunction)2).ToString());
			forwarding.OrganizationId = UserAccount.OrganizationId;
			forwarding.CompanyId = UserAccount.CompanyId;
			forwarding.BranchesId = Conversion.ToInt((object)UserAccount.BranchesId);
			forwarding.ProjectsId = Conversion.ToInt((object)UserAccount.BranchesId);
			forwarding.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			forwarding.DocumentTypeId = 206;
			forwarding.EntryUser = UserAccount.ID;
			forwarding.ModifyUser = UserAccount.ID;
			forwarding.EntryDate = DateTime.Now;
			forwarding.ModifyDate = DateTime.Now;
			forwarding.IsApproved = false;
			forwarding.DocNo = Conversion.ToInt((object)txtdocno.Text.Trim());
			forwarding.DocDate = DocDate.Value;
			forwarding.ExImInvoiceId = Conversion.ToInt(cmbInvoiceNo.Value);
			forwarding.SupplierCustomerId = Conversion.ToInt(cmbCustomer.Value);
			forwarding.ShippingLineId = Conversion.ToInt(cmbShippingLIne.Value);
			forwarding.ShippingAgentId = Conversion.ToInt(cmbShippingAgent.Value);
			forwarding.DriverName = txtDriverName.Text;
			forwarding.DriverCellNo = txtDriverCellNo.Text;
			forwarding.DriverCnicNo = txtCNICNO.Text;
			forwarding.TransporterId = Conversion.ToInt(cmbtransporter.Value);
			forwarding.FreightAmt = Conversion.ToDouble((object)txtFreight.Text);
			forwarding.GpNo = Conversion.ToString((object)cmbGpNo.Text.Trim());
			forwarding.GPDate = txtGPDate.Value;
			forwarding.VehicleNo = txtVehicleNo.Text;
			forwarding.BiltyNo = txtBilityNo.Text;
			forwarding.LoadingPortId = Conversion.ToInt(cmbLoadingPort.Value);
			forwarding.DestinationPortId = Conversion.ToInt(cmbDestinationPort.Value);
			forwarding.ShippedContainer = Conversion.ToInt((object)txtShippedContainer.Text);
			forwarding.NoOfContainer = Conversion.ToInt((object)txtNoOfContainer.Text);
			forwarding.OtherRemarks = Conversion.ToString((object)txtRemarksHEader.Text);
			if (txtCompletedDate.Checked)
			{
				forwarding.LotCompletedDate = txtCompletedDate.Value;
			}
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
				ExImForwardingPackingDetail d = new ExImForwardingPackingDetail();
				if (Conversion.ToInt(r2.Cells["ContractId"].Value) == 0)
				{
					MessageBox.Show("ContractId Required in row#" + (r2.RowIndex + 1));
					return false;
				}
				if (Conversion.ToInt(r2.Cells["ContractDetailId"].Value) == 0)
				{
					MessageBox.Show("ContractDetailId Required in row#" + (r2.RowIndex + 1));
					return false;
				}
				if (Conversion.ToInt(r2.Cells["InvoiceDetailId"].Value) == 0)
				{
					MessageBox.Show("InvoiceDetailId Required in row#" + (r2.RowIndex + 1));
					return false;
				}
				if (Conversion.ToInt(r2.Cells["WarehouseId"].Value) == 0)
				{
					MessageBox.Show("Warehouse Required in row#" + (r2.RowIndex + 1));
					return false;
				}
				if (Conversion.ToInt(r2.Cells["ItemId"].Value) == 0)
				{
					MessageBox.Show("Item Required in row#" + (r2.RowIndex + 1));
					return false;
				}
				if (Conversion.ToInt((object)r2.Cells["CropYearId"]) == 0)
				{
					MessageBox.Show("CropYear Required in row#" + (r2.RowIndex + 1));
					return false;
				}
				if (Conversion.ToInt(r2.Cells["JobLotId"].Value) != 0)
				{
					MessageBox.Show("Job Lot Required in row#" + (r2.RowIndex + 1));
					return false;
				}
				if (Conversion.ToInt(r2.Cells["PackTypeId"].Value) == 0)
				{
					MessageBox.Show("Packing Type Required in row#" + (r2.RowIndex + 1));
					return false;
				}
				if (Conversion.ToInt(r2.Cells["MasterUOMId"].Value) == 0)
				{
					MessageBox.Show("Master UOM Required In Grid");
					return false;
				}
				if (Conversion.ToDouble(r2.Cells["MasterQTY"].Value) == 0.0)
				{
					MessageBox.Show("Master QTY Required in row#" + (r2.RowIndex + 1));
					return false;
				}
				if (Conversion.ToDouble(r2.Cells["NetWeight"].Value) == 0.0)
				{
					MessageBox.Show("NetWeight Required in row#" + (r2.RowIndex + 1));
					return false;
				}
				if (Conversion.ToDouble(r2.Cells["GrossWeight"].Value) == 0.0)
				{
					MessageBox.Show("Gross Weight Required in row#" + (r2.RowIndex + 1));
					return false;
				}
				if (Conversion.ToString(r2.Cells["Container#"].Value) == string.Empty)
				{
					MessageBox.Show("Container Required in row#" + (r2.RowIndex + 1));
					return false;
				}
				if (Conversion.ToString(r2.Cells["Seal#"].Value) != string.Empty)
				{
					MessageBox.Show("Seal No Required in row#" + (r2.RowIndex + 1));
					return false;
				}
				d.Id = Conversion.ToInt(r2.Cells["Id"].Value);
				d.ExImLcOrderId = Conversion.ToInt(r2.Cells["ContractId"].Value);
				d.ContractDetailId = Conversion.ToInt(r2.Cells["ContractDetailId"].Value);
				d.InvoiceId = Conversion.ToInt(cmbInvoiceNo.Value);
				d.InvoiceDetailId = Conversion.ToInt(r2.Cells["InvoiceDetailId"].Value);
				d.WarehouseId = Conversion.ToInt(r2.Cells["WarehouseId"].Value);
				d.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
				d.ItemDescription = Conversion.ToString((object)r2.Cells["ItemDescription"].Text.Trim());
				d.CropYearId = Conversion.ToInt(r2.Cells["CropYearId"].Value);
				d.LotJobId = Conversion.ToInt(r2.Cells["JobLotId"].Value);
				d.PackingMaterialId = Conversion.ToInt(r2.Cells["PackTypeId"].Value);
				d.UOMScheduleIdOuter = Conversion.ToInt(r2.Cells["MasterUOMId"].Value);
				d.EbUnit = Conversion.ToDouble(r2.Cells["EbUnitMaster"].Value);
				d.EbTotal = Conversion.ToDouble(r2.Cells["EbTotalMaster"].Value);
				d.OuterQty = Conversion.ToDouble(r2.Cells["MasterQTY"].Value);
				d.InnerQty = Conversion.ToDouble(r2.Cells["InnerQTY"].Value);
				d.UOMScheduleIdInner = Conversion.ToInt(r2.Cells["InnerUOMId"].Value);
				d.InnerEbUnit = Conversion.ToDouble(r2.Cells["EbUnitInner"].Value);
				d.InnerEbTotal = Conversion.ToDouble(r2.Cells["EbTotalInner"].Value);
				d.TotalPackingWeight = Conversion.ToDouble((object)(d.InnerEbTotal + d.EbTotal));
				d.AdLsWeight = Conversion.ToDouble((object)r2.Cells["AddLess"].Text.Trim());
				d.NetWeight = Conversion.ToDouble((object)r2.Cells["NetWeight"].Text.Trim());
				d.GrossWeight = Conversion.ToDouble((object)r2.Cells["GrossWeight"].Text.Trim());
				GrossWeight += d.GrossWeight;
				d.StockWeight = Conversion.ToDouble((object)r2.Cells["StockWeight"].Text.Trim());
				d.ContainerNo = Conversion.ToString((object)r2.Cells["Container#"].Text.Trim());
				d.SealNo = Conversion.ToString((object)r2.Cells["Seal#"].Text.Trim());
				d.ActionTypeId = ((d.Id <= 0) ? 1 : 2);
				forwarding.ExImForwardingDetail.Add(d);
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

	private void Insert()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_044c: Expected O, but got Unknown
		//IL_0c10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c17: Expected O, but got Unknown
		try
		{
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
			Conversion.ToDouble((object)grdDetail.GetTotal(grdDetail.RootTable.Columns["NetWeight"], (AggregateFunction)2).ToString());
			Conversion.ToDouble((object)grdDetail.GetTotal(grdDetail.RootTable.Columns["StockWeight"], (AggregateFunction)2).ToString());
			if (Conversion.ToDouble((object)txtFreight.Text.Trim()) > 0.0)
			{
				if (((UltraGridBase)cmbtransporter).ActiveRow == null || Conversion.ToInt(cmbtransporter.Value) == 0)
				{
					MessageBox.Show("Transporter Account field required");
					cmbtransporter.Focus();
					return;
				}
				forwarding.TransporterId = Conversion.ToInt(cmbtransporter.Value);
				forwarding.FreightAmt = Conversion.ToDouble((object)txtFreight.Text.Trim());
			}
			forwarding.OrganizationId = UserAccount.OrganizationId;
			forwarding.CompanyId = UserAccount.CompanyId;
			forwarding.BranchesId = UserAccount.BranchesId;
			forwarding.ProjectsId = Conversion.ToInt((object)UserAccount.BranchesId);
			forwarding.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			forwarding.DocumentTypeId = 206;
			forwarding.EntryUser = UserAccount.ID;
			forwarding.ModifyUser = UserAccount.ID;
			forwarding.EntryDate = DateTime.Now;
			forwarding.ModifyDate = DateTime.Now;
			forwarding.IsApproved = false;
			forwarding.DocNo = Conversion.ToInt((object)txtdocno.Text.Trim());
			forwarding.DocDate = DocDate.Value;
			forwarding.ExImInvoiceId = Conversion.ToInt(cmbInvoiceNo.Value);
			forwarding.SupplierCustomerId = Conversion.ToInt(cmbCustomer.Value);
			forwarding.ShippingLineId = Conversion.ToInt(cmbShippingLIne.Value);
			forwarding.ShippingAgentId = Conversion.ToInt(cmbShippingAgent.Value);
			forwarding.DriverName = txtDriverName.Text;
			forwarding.DriverCellNo = txtDriverCellNo.Text;
			forwarding.DriverCnicNo = txtCNICNO.Text;
			forwarding.TransporterId = Conversion.ToInt(cmbtransporter.Value);
			forwarding.FreightAmt = Conversion.ToDouble((object)txtFreight.Text);
			forwarding.GpNo = Conversion.ToString((object)cmbGpNo.Text.Trim());
			forwarding.GPDate = txtGPDate.Value;
			forwarding.VehicleNo = txtVehicleNo.Text;
			forwarding.BiltyNo = txtBilityNo.Text;
			forwarding.LoadingPortId = Conversion.ToInt(cmbLoadingPort.Value);
			forwarding.DestinationPortId = Conversion.ToInt(cmbDestinationPort.Value);
			forwarding.ShippedContainer = Conversion.ToInt((object)txtShippedContainer.Text);
			forwarding.NoOfContainer = Conversion.ToInt((object)txtNoOfContainer.Text);
			forwarding.OtherRemarks = Conversion.ToString((object)txtRemarksHEader.Text);
			if (txtCompletedDate.Checked)
			{
				forwarding.LotCompletedDate = txtCompletedDate.Value;
			}
			forwarding.ExImForwardingDetail = new List<ExImForwardingPackingDetail>();
			forwarding.ExImForwardingOtherItemslist = new List<ExImForwardingOtherItems>();
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
				ExImForwardingPackingDetail d = new ExImForwardingPackingDetail();
				if (Conversion.ToInt(r2.Cells["ContractId"].Value) == 0)
				{
					MessageBox.Show("ContractId Required in row#" + (r2.RowIndex + 1));
					return;
				}
				if (Conversion.ToInt(r2.Cells["ContractDetailId"].Value) == 0)
				{
					MessageBox.Show("ContractDetailId Required in row#" + (r2.RowIndex + 1));
					return;
				}
				if (Conversion.ToInt(r2.Cells["InvoiceDetailId"].Value) == 0)
				{
					MessageBox.Show("InvoiceDetailId Required in row#" + (r2.RowIndex + 1));
					return;
				}
				if (Conversion.ToInt(r2.Cells["WarehouseId"].Value) == 0)
				{
					MessageBox.Show("Warehouse Required in row#" + (r2.RowIndex + 1));
					return;
				}
				if (Conversion.ToInt(r2.Cells["ItemId"].Value) == 0)
				{
					MessageBox.Show("Item Required in row#" + (r2.RowIndex + 1));
					return;
				}
				if (Conversion.ToInt(r2.Cells["CropYearId"].Value) == 0)
				{
					MessageBox.Show("CropYear Required in row#" + (r2.RowIndex + 1));
					return;
				}
				if (Conversion.ToInt(r2.Cells["JobLotId"].Value) == 0)
				{
					MessageBox.Show("Job Lot Required in row#" + (r2.RowIndex + 1));
					return;
				}
				if (Conversion.ToInt(r2.Cells["PackTypeId"].Value) == 0)
				{
					MessageBox.Show("Packing Type Required in row#" + (r2.RowIndex + 1));
					return;
				}
				if (Conversion.ToInt(r2.Cells["MasterUOMId"].Value) == 0)
				{
					MessageBox.Show("Master UOM Required In Grid");
					return;
				}
				if (Conversion.ToDouble(r2.Cells["MasterQTY"].Value) == 0.0)
				{
					MessageBox.Show("Master QTY Required in row#" + (r2.RowIndex + 1));
					return;
				}
				if (Conversion.ToDouble(r2.Cells["NetWeight"].Value) == 0.0)
				{
					MessageBox.Show("NetWeight Required in row#" + (r2.RowIndex + 1));
					return;
				}
				if (Conversion.ToDouble(r2.Cells["GrossWeight"].Value) == 0.0)
				{
					MessageBox.Show("Gross Weight Required in row#" + (r2.RowIndex + 1));
					return;
				}
				if (Conversion.ToString(r2.Cells["Container#"].Value) == string.Empty)
				{
					MessageBox.Show("Container Required in row#" + (r2.RowIndex + 1));
					return;
				}
				if (Conversion.ToString(r2.Cells["Seal#"].Value) == string.Empty)
				{
					MessageBox.Show("Seal No Required in row#" + (r2.RowIndex + 1));
					return;
				}
				d.Id = Conversion.ToInt(r2.Cells["Id"].Value);
				d.ExImLcOrderId = Conversion.ToInt(r2.Cells["ContractId"].Value);
				d.ContractDetailId = Conversion.ToInt(r2.Cells["ContractDetailId"].Value);
				d.InvoiceId = Conversion.ToInt(cmbInvoiceNo.Value);
				d.InvoiceDetailId = Conversion.ToInt(r2.Cells["InvoiceDetailId"].Value);
				d.WarehouseId = Conversion.ToInt(r2.Cells["WarehouseId"].Value);
				d.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
				d.ItemDescription = Conversion.ToString((object)r2.Cells["ItemDescription"].Text.Trim());
				d.CropYearId = Conversion.ToInt(r2.Cells["CropYearId"].Value);
				d.LotJobId = Conversion.ToInt(r2.Cells["JobLotId"].Value);
				d.PackingMaterialId = Conversion.ToInt(r2.Cells["PackTypeId"].Value);
				d.UOMScheduleIdOuter = Conversion.ToInt(r2.Cells["MasterUOMId"].Value);
				d.EbUnit = Conversion.ToDouble(r2.Cells["EbUnitMaster"].Value);
				d.EbTotal = Conversion.ToDouble(r2.Cells["EbTotalMaster"].Value);
				d.OuterQty = Conversion.ToDouble(r2.Cells["MasterQTY"].Value);
				d.InnerQty = Conversion.ToDouble(r2.Cells["InnerQTY"].Value);
				d.UOMScheduleIdInner = Conversion.ToInt(r2.Cells["InnerUOMId"].Value);
				d.InnerEbUnit = Conversion.ToDouble(r2.Cells["EbUnitInner"].Value);
				d.InnerEbTotal = Conversion.ToDouble(r2.Cells["EbTotalInner"].Value);
				d.TotalPackingWeight = Conversion.ToDouble((object)(d.InnerEbTotal + d.EbTotal));
				d.AdLsWeight = Conversion.ToDouble((object)r2.Cells["AddLess"].Text.Trim());
				d.NetWeight = Conversion.ToDouble((object)r2.Cells["NetWeight"].Text.Trim());
				d.GrossWeight = Conversion.ToDouble((object)r2.Cells["GrossWeight"].Text.Trim());
				GrossWeight += d.GrossWeight;
				d.StockWeight = Conversion.ToDouble((object)r2.Cells["StockWeight"].Text.Trim());
				d.ContainerNo = Conversion.ToString((object)r2.Cells["Container#"].Text.Trim());
				d.SealNo = Conversion.ToString((object)r2.Cells["Seal#"].Text.Trim());
				d.ActionTypeId = ((d.Id <= 0) ? 1 : 2);
				forwarding.ExImForwardingDetail.Add(d);
			}
			if (grdotheritems.RowCount > 0)
			{
				rows = grdotheritems.GetRows();
				foreach (GridEXRow r in rows)
				{
					ExImForwardingOtherItems vd = new ExImForwardingOtherItems();
					vd.otherItemId = Conversion.ToInt(r.Cells["Id"].Value);
					vd.ExImInvoiceId = Conversion.ToInt(r.Cells["ExImInvoiceId"].Value);
					vd.InvoiceOtherItemsDetailId = Conversion.ToInt(r.Cells["InvoiceOtherItemsDetailId"].Value);
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
				frmwagesBillHeader.RefDocTypeId = 206;
				frmwagesBillHeader.RefDocId = success;
				frmwagesBillHeader.GrossWeightTotal = GrossWeight;
				frmwagesBillHeader.ShowDialog();
				frmwagesBillHeader.BringToFront();
			}
			FormReset();
			if (ChkPrintPreview.Checked)
			{
				GeneratePrint(success);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
				throw new Exception("Rec Id not found");
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
		try
		{
			ExImForwarding exImLc = ExImForwarding.GetByID(Id);
			if (exImLc != null)
			{
				UpdateMode = true;
				btnSave.Visible = false;
				btnUpdate.Visible = true;
				btnDelete.Visible = true;
				tabControl1.SelectedIndex = 0;
				tabControl2.SelectedIndex = 0;
				txtdocno.Text = Conversion.ToString((object)exImLc.DocNo);
				DocDate.Value = exImLc.DocDate;
				bindInvoiceNo();
				cmbInvoiceNo.Value = exImLc.ExImInvoiceId;
				DataTable dtCustomer = new DataTable();
				dtCustomer.Columns.Add("SupplierCustomerId");
				dtCustomer.Columns.Add("Customer");
				dtCustomer.Rows.Add(exImLc.SupplierCustomerId, exImLc.Customer);
				if (dtCustomer.Rows.Count > 0)
				{
					DDL.BindDDL(dtCustomer, cmbCustomer, "SupplierCustomerId", "Customer", "CustomerName", ZeroIndex: false);
					((UltraGridBase)cmbCustomer).Rows[0].Activate();
				}
				cmbShippingLIne.Value = Conversion.ToInt((object)exImLc.ShippingLineId);
				cmbShippingAgent.Value = Conversion.ToInt((object)exImLc.ShippingAgentId);
				txtDriverCellNo.Text = Conversion.ToString((object)exImLc.DriverCellNo);
				txtDriverName.Text = Conversion.ToString((object)exImLc.DriverName);
				txtCNICNO.Text = Conversion.ToString((object)exImLc.DriverCnicNo);
				cmbtransporter.Value = Conversion.ToInt((object)exImLc.TransporterId);
				txtFreight.Text = Conversion.ToString((object)exImLc.FreightAmt);
				cmbGpNo.Text = exImLc.GpNo;
				txtGPDate.Value = exImLc.GPDate;
				txtVehicleNo.Text = Conversion.ToString((object)exImLc.VehicleNo);
				txtBilityNo.Text = Conversion.ToString((object)exImLc.BiltyNo);
				cmbDestinationPort.Value = exImLc.DestinationPortId;
				cmbLoadingPort.Value = exImLc.LoadingPortId;
				txtShippedContainer.Text = Conversion.ToString((object)exImLc.ShippedContainer);
				txtNoOfContainer.Text = Conversion.ToString((object)exImLc.NoOfContainer);
				txtCompletedDate.Value = Conversion.ToDateTime((object)exImLc.LotCompletedDate);
				txtRemarksHEader.Text = Conversion.ToString((object)exImLc.OtherRemarks);
				dtDetail.Rows.Clear();
				double InvoiceWt = 0.0;
				for (int i = 0; i < exImLc.ExImForwardingDetail.Count; i++)
				{
					dtDetail.Rows.Add(exImLc.ExImForwardingDetail[i].Id, exImLc.ExImForwardingDetail[i].InvoiceId, exImLc.ExImForwardingDetail[i].InvoiceDetailId, exImLc.ExImForwardingDetail[i].ExImLcOrderId, exImLc.ExImForwardingDetail[i].ContractDetailId, exImLc.ExImForwardingDetail[i].LcOrderNo, exImLc.ExImForwardingDetail[i].WarehouseId, exImLc.ExImForwardingDetail[i].WareHouseName, exImLc.ExImForwardingDetail[i].ItemId, exImLc.ExImForwardingDetail[i].ItemCode, exImLc.ExImForwardingDetail[i].ItemName, exImLc.ExImForwardingDetail[i].ItemDescription, exImLc.ExImForwardingDetail[i].CropYearId, exImLc.ExImForwardingDetail[i].CropYear, exImLc.ExImForwardingDetail[i].LotJobId, exImLc.ExImForwardingDetail[i].JobLotName, exImLc.ExImForwardingDetail[i].PackingMaterialId, exImLc.ExImForwardingDetail[i].Packtype, exImLc.ExImForwardingDetail[i].OuterQty, exImLc.ExImForwardingDetail[i].UOMScheduleIdOuter, exImLc.ExImForwardingDetail[i].OuterUOM, exImLc.ExImForwardingDetail[i].OuterEquivalent, exImLc.ExImForwardingDetail[i].EbUnit, exImLc.ExImForwardingDetail[i].EbTotal, exImLc.ExImForwardingDetail[i].InnerQty, exImLc.ExImForwardingDetail[i].UOMScheduleIdInner, exImLc.ExImForwardingDetail[i].InnerUOM, exImLc.ExImForwardingDetail[i].InnerEquivalent, exImLc.ExImForwardingDetail[i].InnerEbUnit, exImLc.ExImForwardingDetail[i].InnerEbTotal, exImLc.ExImForwardingDetail[i].AdLsWeight, exImLc.ExImForwardingDetail[i].NetWeight, exImLc.ExImForwardingDetail[i].GrossWeight, exImLc.ExImForwardingDetail[i].StockWeight, exImLc.ExImForwardingDetail[i].ContainerNo, exImLc.ExImForwardingDetail[i].SealNo);
					InvoiceWt += exImLc.ExImForwardingDetail[i].NetWeight;
				}
				grdDetail.DataSource = dtDetail;
				grdDetail.RetrieveStructure();
				GridDetailSetting();
				dtOtherItemDetail.Rows.Clear();
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
					dtOtherItemDetail.Rows.Add(exImLc.ExImForwardingOtherItemslist[x].Id, exImLc.ExImForwardingOtherItemslist[x].ExImInvoiceId, exImLc.ExImForwardingOtherItemslist[x].InvoiceOtherItemsDetailId, exImLc.ExImForwardingOtherItemslist[x].otherItemId, exImLc.ExImForwardingOtherItemslist[x].ItemName, exImLc.ExImForwardingOtherItemslist[x].oItemQty, exImLc.ExImForwardingOtherItemslist[x].oItemRate, exImLc.ExImForwardingOtherItemslist[x].oItemAmount, exImLc.ExImForwardingOtherItemslist[x].ContainerNo, exImLc.ExImForwardingOtherItemslist[x].OtherItemRemarks);
				}
				grdotheritems.DataSource = dtOtherItemDetail;
				grdotheritems.RetrieveStructure();
				grdSettingsOtherItems();
				DDL.BindDDL(dtOItem, CmbOtherItem, "Id", "ItemName", "Item Name", ZeroIndex: false);
				((UltraGridBase)CmbOtherItem).DisplayLayout.Bands[0].Columns[1].Width = 400;
				if (!AutoUpdate)
				{
					FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
				}
				return;
			}
			FormReset();
			throw new Exception("Record not found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		try
		{
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvPurchaseInvoice.RemoveByID(new InvPurchaseInvoice
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocumentTypeId = 206,
						Id = RecId,
						EntryUser = UserAccount.ID
					});
					MessageBox.Show("Delete Record Seccessfully");
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

	private void FormReset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			LstRemoveRecordDetail.Clear();
			RecId = 0;
			inVoiceId = 0;
			updateDetailIndex = -1;
			updateDetailIndexOther = -1;
			generateCode();
			bindInvoiceNo();
			((Control)(object)cmbInvoiceNo).Text = string.Empty;
			((Control)(object)cmbCustomer).Text = string.Empty;
			((Control)(object)cmbShippingLIne).Text = string.Empty;
			((Control)(object)cmbShippingAgent).Text = string.Empty;
			txtDriverName.Text = string.Empty;
			txtDriverCellNo.Text = string.Empty;
			txtCNICNO.Text = string.Empty;
			((Control)(object)cmbtransporter).Text = string.Empty;
			txtFreight.Text = string.Empty;
			cmbGpNo.Text = string.Empty;
			txtGPDate.Value = DateTime.Now;
			txtVehicleNo.Text = string.Empty;
			txtBilityNo.Text = string.Empty;
			((Control)(object)cmbLoadingPort).Text = string.Empty;
			((Control)(object)cmbDestinationPort).Text = string.Empty;
			txtShippedContainer.Text = string.Empty;
			txtNoOfContainer.Text = string.Empty;
			txtCompletedDate.Value = DateTime.Now;
			((Control)(object)cmbItem).Text = string.Empty;
			((UltraGridBase)cmbItem).DataSource = null;
			((Control)(object)CmbPackUomMaster).Text = string.Empty;
			((UltraGridBase)CmbPackUomMaster).DataSource = null;
			((Control)(object)CmbPackUomInner).Text = string.Empty;
			((UltraGridBase)CmbPackUomInner).DataSource = null;
			dtDetail.Rows.Clear();
			grdDetail.ClearStructure();
			dtOtherItemDetail.Rows.Clear();
			grdotheritems.ClearStructure();
			btnSave.Visible = true;
			UpdateMode = false;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			FormDetailReset();
			cmbInvoiceNo.Focus();
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
			updateDetailIndex = -1;
			((Control)(object)CmbWareHouse).Text = string.Empty;
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)CmbJobLot).Text = string.Empty;
			txtItemDesc.Text = string.Empty;
			((Control)(object)cmbPackType).Text = string.Empty;
			((Control)(object)CmbPackUomMaster).Text = string.Empty;
			((Control)(object)CmbPackUomInner).Text = string.Empty;
			txtEbUnitInner.Text = string.Empty;
			txtEbTotalInner.Text = string.Empty;
			txtEbUnitMaster.Text = string.Empty;
			txtEbTotalMaster.Text = string.Empty;
			txtAddLess.Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			txtNetWeight.Text = string.Empty;
			txtStockWeight.Text = string.Empty;
			txtSeal.Text = string.Empty;
			cmbcontainerNo.Text = string.Empty;
			btnDetailUpdate.Visible = false;
			btnDetailCancel.Visible = false;
			btnAddinGrid.Visible = false;
			CmbWareHouse.Focus();
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
			updateDetailIndexOther = -1;
			((Control)(object)CmbOtherItem).Text = string.Empty;
			txtoQty.Text = string.Empty;
			txtoRate.Text = string.Empty;
			txtoAmount.Text = string.Empty;
			txtOContainer.Text = string.Empty;
			txtoRemarks.Text = string.Empty;
			btnoadd.Visible = true;
			btnoUpdate.Visible = false;
			btnoCancel.Visible = false;
			CmbOtherItem.Focus();
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
			WagesRefDocumentsStatus obj = clsGlobalVariables.WagesRefDocumentsStatusList.Find((WagesRefDocumentsStatus row) => row.RefDocumentTypeId == 206);
			WagesActiveOrInActive = obj != null && obj.IsActive;
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
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		try
		{
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("Customer", typeof(string));
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
				}
			}
			DDL.BindDDL(dtCustomer, CmbCustomerHistory, "Id", "Customer", "Party", ZeroIndex: false);
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
			order.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			order.DocumentTypeIds = "206";
			order.CanViewAllRecord = true;
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
			order.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			DataTable dtHistory = ExImForwarding.FormHistory(order);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("InvoiceNo", typeof(string));
				dt.Columns.Add("CustomerName", typeof(string));
				dt.Columns.Add("ShippingLine", typeof(string));
				dt.Columns.Add("DriverName", typeof(string));
				dt.Columns.Add("DriverCellNo", typeof(string));
				dt.Columns.Add("DriverCnicNo", typeof(string));
				dt.Columns.Add("Transporter", typeof(string));
				dt.Columns.Add("Freight", typeof(double));
				dt.Columns.Add("GpNo", typeof(int));
				dt.Columns.Add("GPDate", typeof(DateTime));
				dt.Columns.Add("VehicleNo", typeof(string));
				dt.Columns.Add("BiltyNo", typeof(string));
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
					dt.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["DocNo"], Conversion.ToDateTime(dtHistory.Rows[i]["DocDate"]).ToShortDateString(), dtHistory.Rows[i]["InvoiceNo"], dtHistory.Rows[i]["CustomerName"], dtHistory.Rows[i]["ShippingLine"], dtHistory.Rows[i]["DriverName"], dtHistory.Rows[i]["DriverCellNo"], dtHistory.Rows[i]["DriverCnicNo"], dtHistory.Rows[i]["TransporterName"], dtHistory.Rows[i]["FreightAmt"], dtHistory.Rows[i]["GpNo"], dtHistory.Rows[i]["GpDate"], dtHistory.Rows[i]["VehicleNo"], dtHistory.Rows[i]["BiltyNo"], dtHistory.Rows[i]["EntryUserName"], dtHistory.Rows[i]["EntryDate"], dtHistory.Rows[i]["ModifyUserName"], dtHistory.Rows[i]["ModifyDate"], dtHistory.Rows[i]["ApprovedStatus"], dtHistory.Rows[i]["ApprovedUserName"], dtHistory.Rows[i]["ApprovedDate"], dtHistory.Rows[i]["NoOfAttachments"]);
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
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["GPDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["EntryDate"].Width = 150;
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].Width = 150;
			DataGridHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ApprovedDate"].Width = 150;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["Freight"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["Freight"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(DataGridHistory);
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 50;
			DataGridHistory.RootTable.Columns["Edit"].Position = 0;
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 50;
			DataGridHistory.RootTable.Columns["Print"].Position = 1;
			DataGridHistory.FrozenColumns = 2;
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
				RecId = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
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
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), 206);
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
				dt.Columns.Add("InvoiceId", typeof(int));
				dt.Columns.Add("InvoiceDetailId", typeof(int));
				dt.Columns.Add("ContractId", typeof(int));
				dt.Columns.Add("ContractDetailId", typeof(int));
				dt.Columns.Add("Contract");
				dt.Columns.Add("Warehouse");
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("ItemCode");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("ItemDescription");
				dt.Columns.Add("CropYear");
				dt.Columns.Add("JobLot");
				dt.Columns.Add("PackingType");
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
				dt.Columns.Add("Container#");
				dt.Columns.Add("Seal#");
				for (int i = 0; i < exImLc.ExImForwardingDetail.Count; i++)
				{
					dt.Rows.Add(exImLc.ExImForwardingDetail[i].Id, exImLc.ExImForwardingDetail[i].InvoiceId, exImLc.ExImForwardingDetail[i].InvoiceDetailId, exImLc.ExImForwardingDetail[i].ExImLcOrderId, exImLc.ExImForwardingDetail[i].ContractDetailId, exImLc.ExImForwardingDetail[i].LcOrderNo, exImLc.ExImForwardingDetail[i].WareHouseName, exImLc.ExImForwardingDetail[i].ItemId, exImLc.ExImForwardingDetail[i].ItemCode, exImLc.ExImForwardingDetail[i].ItemName, exImLc.ExImForwardingDetail[i].ItemDescription, exImLc.ExImForwardingDetail[i].CropYear, exImLc.ExImForwardingDetail[i].JobLotName, exImLc.ExImForwardingDetail[i].Packtype, exImLc.ExImForwardingDetail[i].OuterQty, exImLc.ExImForwardingDetail[i].OuterUOM, exImLc.ExImForwardingDetail[i].EbUnit, exImLc.ExImForwardingDetail[i].EbTotal, exImLc.ExImForwardingDetail[i].InnerQty, exImLc.ExImForwardingDetail[i].InnerUOM, exImLc.ExImForwardingDetail[i].InnerEbUnit, exImLc.ExImForwardingDetail[i].InnerEbTotal, exImLc.ExImForwardingDetail[i].AdLsWeight, exImLc.ExImForwardingDetail[i].NetWeight, exImLc.ExImForwardingDetail[i].GrossWeight, exImLc.ExImForwardingDetail[i].StockWeight, exImLc.ExImForwardingDetail[i].ContainerNo, exImLc.ExImForwardingDetail[i].SealNo);
				}
				grdHistoryDetail.DataSource = dt;
				grdHistoryDetail.RetrieveStructure();
				GridHistoryDetailSetting();
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
			grdHistoryDetail.RootTable.Columns["InvoiceId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["InvoiceDetailId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["ContractId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["ContractDetailId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["ItemId"].Visible = false;
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
			CommonServices.GridAutoAdjustmentNew(grdHistoryDetail);
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
			CommonServices.ForwardingDirectSlip507_01(printId);
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
			txtGrossWeight.Text = 0.ToString();
			txtStockWeight.Text = 0.ToString();
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
			if (DoNetWt > 0.0)
			{
				GrossWt = DoNetWt + EBTotalMaster + EBTotalInner + AddLss;
				txtGrossWeight.Text = GrossWt.ToString();
				StockWeight = GrossWt - (EBTotalMaster + EBTotalInner);
				txtStockWeight.Text = Math.Round(StockWeight, 3).ToString();
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
			if (Conversion.ToInt(CmbPackUomMaster.Value) != 0)
			{
				int UOM = Conversion.ToInt(CmbPackUomMaster.Value);
				CmbPackUomInner.Value = UOM;
			}
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
					throw new Exception("You don't have rihgt to update");
				}
				FormReset();
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
					RecId = id;
					ReadById(RecId);
				}
				else if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(id, 206);
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
			DataTable dtIds = ExImForwarding.GetForwardingIdsForAutoUpdation(UserAccount.OrganizationId, UserAccount.CompanyId, 206);
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
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Expected O, but got Unknown
		//IL_0640: Unknown result type (might be due to invalid IL or missing references)
		//IL_064a: Expected O, but got Unknown
		//IL_064b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0655: Expected O, but got Unknown
		//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ad: Expected O, but got Unknown
		//IL_06b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c3: Expected O, but got Unknown
		//IL_071c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0726: Expected O, but got Unknown
		//IL_0727: Unknown result type (might be due to invalid IL or missing references)
		//IL_0731: Expected O, but got Unknown
		//IL_073d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0747: Expected O, but got Unknown
		//IL_07c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cb: Expected O, but got Unknown
		//IL_087c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0886: Expected O, but got Unknown
		//IL_08d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08de: Expected O, but got Unknown
		//IL_090b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0915: Expected O, but got Unknown
		//IL_0916: Unknown result type (might be due to invalid IL or missing references)
		//IL_0920: Expected O, but got Unknown
		//IL_096e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0978: Expected O, but got Unknown
		//IL_0984: Unknown result type (might be due to invalid IL or missing references)
		//IL_098e: Expected O, but got Unknown
		//IL_099a: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a4: Expected O, but got Unknown
		//IL_0a4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a54: Expected O, but got Unknown
		//IL_0ae4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aee: Expected O, but got Unknown
		//IL_0b05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0f: Expected O, but got Unknown
		//IL_0bb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbf: Expected O, but got Unknown
		//IL_1280: Unknown result type (might be due to invalid IL or missing references)
		//IL_128a: Expected O, but got Unknown
		//IL_948c: Unknown result type (might be due to invalid IL or missing references)
		//IL_9496: Expected O, but got Unknown
		//IL_94ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_94c4: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance121 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance277 = new Appearance();
		Appearance appearance278 = new Appearance();
		Appearance appearance279 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance265 = new Appearance();
		Appearance appearance266 = new Appearance();
		Appearance appearance267 = new Appearance();
		Appearance appearance254 = new Appearance();
		Appearance appearance255 = new Appearance();
		Appearance appearance268 = new Appearance();
		Appearance appearance269 = new Appearance();
		Appearance appearance270 = new Appearance();
		Appearance appearance271 = new Appearance();
		Appearance appearance272 = new Appearance();
		Appearance appearance273 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
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
		Appearance appearance241 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance274 = new Appearance();
		Appearance appearance275 = new Appearance();
		Appearance appearance276 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance262 = new Appearance();
		Appearance appearance263 = new Appearance();
		Appearance appearance264 = new Appearance();
		GridEXLayout grdotheritems_DesignTimeLayout = new GridEXLayout();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance280 = new Appearance();
		Appearance appearance281 = new Appearance();
		Appearance appearance282 = new Appearance();
		Appearance appearance283 = new Appearance();
		Appearance appearance284 = new Appearance();
		Appearance appearance285 = new Appearance();
		Appearance appearance286 = new Appearance();
		Appearance appearance287 = new Appearance();
		Appearance appearance288 = new Appearance();
		Appearance appearance161 = new Appearance();
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
		Appearance appearance174 = new Appearance();
		Appearance appearance289 = new Appearance();
		Appearance appearance290 = new Appearance();
		Appearance appearance291 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
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
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance247 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance252 = new Appearance();
		Appearance appearance253 = new Appearance();
		Appearance appearance256 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance258 = new Appearance();
		Appearance appearance259 = new Appearance();
		Appearance appearance260 = new Appearance();
		Appearance appearance261 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel4 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grdDetail = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel5 = new System.Windows.Forms.Panel();
		this.rdbtnItemCode = new System.Windows.Forms.RadioButton();
		this.rdbtnItemName = new System.Windows.Forms.RadioButton();
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
		this.cmbcontainerNo = new System.Windows.Forms.TextBox();
		this.txtSeal = new System.Windows.Forms.TextBox();
		this.label30 = new System.Windows.Forms.Label();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.grdotheritems = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.btnoadd = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.txtOContainer = new System.Windows.Forms.TextBox();
		this.label42 = new System.Windows.Forms.Label();
		this.btnoCancel = new System.Windows.Forms.Button();
		this.btnoUpdate = new System.Windows.Forms.Button();
		this.label27 = new System.Windows.Forms.Label();
		this.txtoRemarks = new System.Windows.Forms.TextBox();
		this.txtoQty = new System.Windows.Forms.TextBox();
		this.label41 = new System.Windows.Forms.Label();
		this.txtoRate = new System.Windows.Forms.TextBox();
		this.label63 = new System.Windows.Forms.Label();
		this.label70 = new System.Windows.Forms.Label();
		this.txtoAmount = new System.Windows.Forms.TextBox();
		this.CmbOtherItem = new UltraCombo();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.btnRecordsUpdate = new System.Windows.Forms.Button();
		this.ChkPrintPreview = new System.Windows.Forms.CheckBox();
		this.label36 = new System.Windows.Forms.Label();
		this.txtCNICNO = new System.Windows.Forms.TextBox();
		this.txtDriverCellNo = new System.Windows.Forms.TextBox();
		this.cmbtransporter = new UltraCombo();
		this.txtDriverName = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.cmbGpNo = new System.Windows.Forms.TextBox();
		this.cmbCustomer = new UltraCombo();
		this.cmbShippingAgent = new UltraCombo();
		this.txtShippedContainer = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.txtNoOfContainer = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.txtCompletedDate = new System.Windows.Forms.DateTimePicker();
		this.label26 = new System.Windows.Forms.Label();
		this.cmbShippingLIne = new UltraCombo();
		this.txtFreight = new System.Windows.Forms.TextBox();
		this.cmbDestinationPort = new UltraCombo();
		this.label23 = new System.Windows.Forms.Label();
		this.cmbLoadingPort = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.txtBilityNo = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtGPDate = new System.Windows.Forms.DateTimePicker();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.cmbInvoiceNo = new UltraCombo();
		this.txtRemarksHEader = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.BtnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.BtnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
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
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.label31 = new System.Windows.Forms.Label();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.label73 = new System.Windows.Forms.Label();
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
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel7.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUomInner).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUomMaster).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).BeginInit();
		this.tabPage4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdotheritems).BeginInit();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOtherItem).BeginInit();
		this.panel3.SuspendLayout();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbtransporter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbShippingAgent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbShippingLIne).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDestinationPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoadingPort).BeginInit();
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
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1247, 681);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1247, 681);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1239, 652);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel4.Controls.Add(this.tabControl2);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(3, 206);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1233, 443);
		this.panel4.TabIndex = 4;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1233, 443);
		this.tabControl2.TabIndex = 0;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabPage3.Controls.Add(this.panel7);
		this.tabPage3.Controls.Add(this.panel5);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1225, 414);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Detail";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(3, 124);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1219, 287);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 1;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetail_KeyDown);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 103);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1219, 21);
		this.panel7.TabIndex = 1;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1171, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdDetail;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar1.TabIndex = 36;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel5.Controls.Add(this.rdbtnItemCode);
		this.panel5.Controls.Add(this.rdbtnItemName);
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
		this.panel5.Controls.Add(this.cmbcontainerNo);
		this.panel5.Controls.Add(this.txtSeal);
		this.panel5.Controls.Add(this.label30);
		this.panel5.Controls.Add(this.txtNetWeight);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1219, 100);
		this.panel5.TabIndex = 0;
		this.rdbtnItemCode.AutoSize = true;
		this.rdbtnItemCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemCode.Location = new System.Drawing.Point(258, 2);
		this.rdbtnItemCode.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemCode.Name = "rdbtnItemCode";
		this.rdbtnItemCode.Size = new System.Drawing.Size(52, 19);
		this.rdbtnItemCode.TabIndex = 129;
		this.rdbtnItemCode.Text = "Code";
		this.rdbtnItemCode.UseVisualStyleBackColor = true;
		this.rdbtnItemName.AutoSize = true;
		this.rdbtnItemName.Checked = true;
		this.rdbtnItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemName.Location = new System.Drawing.Point(205, 2);
		this.rdbtnItemName.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemName.Name = "rdbtnItemName";
		this.rdbtnItemName.Size = new System.Drawing.Size(57, 19);
		this.rdbtnItemName.TabIndex = 128;
		this.rdbtnItemName.TabStop = true;
		this.rdbtnItemName.Text = "Name";
		this.rdbtnItemName.UseVisualStyleBackColor = true;
		this.btnAddinGrid.BackColor = System.Drawing.Color.Teal;
		this.btnAddinGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddinGrid.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddinGrid.ForeColor = System.Drawing.Color.White;
		this.btnAddinGrid.Location = new System.Drawing.Point(1080, 68);
		this.btnAddinGrid.Name = "btnAddinGrid";
		this.btnAddinGrid.Size = new System.Drawing.Size(36, 28);
		this.btnAddinGrid.TabIndex = 20;
		this.btnAddinGrid.Text = "+";
		this.btnAddinGrid.UseVisualStyleBackColor = false;
		this.btnAddinGrid.Visible = false;
		this.btnAddinGrid.Click += new System.EventHandler(btnAddinGrid_Click);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(3, 53);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(49, 15);
		this.label3.TabIndex = 117;
		this.label3.Text = "Eb Total";
		this.txtEbTotalMaster.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbTotalMaster.Enabled = false;
		this.txtEbTotalMaster.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEbTotalMaster.Location = new System.Drawing.Point(3, 71);
		this.txtEbTotalMaster.Name = "txtEbTotalMaster";
		this.txtEbTotalMaster.Size = new System.Drawing.Size(86, 23);
		this.txtEbTotalMaster.TabIndex = 9;
		this.txtEbTotalMaster.Tag = "TotalPackingWeightMaster";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(1139, 5);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(62, 15);
		this.label6.TabIndex = 116;
		this.label6.Text = "Eb Weight";
		this.txtEbUnitMaster.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbUnitMaster.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEbUnitMaster.Location = new System.Drawing.Point(1139, 25);
		this.txtEbUnitMaster.Name = "txtEbUnitMaster";
		this.txtEbUnitMaster.Size = new System.Drawing.Size(72, 23);
		this.txtEbUnitMaster.TabIndex = 8;
		this.txtEbUnitMaster.Tag = "PackingWeightMaster";
		this.txtEbUnitMaster.TextChanged += new System.EventHandler(txtEbUnitMaster_TextChanged);
		this.txtEbUnitMaster.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEbUnitMaster_KeyPress);
		this.txtEbUnitInner.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbUnitInner.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEbUnitInner.Location = new System.Drawing.Point(268, 71);
		this.txtEbUnitInner.Name = "txtEbUnitInner";
		this.txtEbUnitInner.Size = new System.Drawing.Size(72, 23);
		this.txtEbUnitInner.TabIndex = 12;
		this.txtEbUnitInner.Tag = "PackingWeight";
		this.txtEbUnitInner.TextChanged += new System.EventHandler(txtEbUnitInner_TextChanged);
		this.txtEbUnitInner.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEbUnitInner_KeyPress);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(342, 53);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(49, 15);
		this.label9.TabIndex = 115;
		this.label9.Text = "Eb Total";
		this.txtEbTotalInner.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbTotalInner.Enabled = false;
		this.txtEbTotalInner.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEbTotalInner.Location = new System.Drawing.Point(342, 71);
		this.txtEbTotalInner.Name = "txtEbTotalInner";
		this.txtEbTotalInner.Size = new System.Drawing.Size(86, 23);
		this.txtEbTotalInner.TabIndex = 13;
		this.txtEbTotalInner.Tag = "TotalPackingWeight";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(268, 53);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(62, 15);
		this.label13.TabIndex = 114;
		this.label13.Text = "Eb Weight";
		this.CmbPackUomInner.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUomInner.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUomInner.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUomInner.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Appearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUomInner).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance173;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUomInner).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance184).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance184).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance195).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance195;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance206).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance214;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance215).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance216).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance216).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance216).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance216).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance216;
		((AppearanceBase)appearance277).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance277;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance278).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance278).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance278;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance279).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance279;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUomInner).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackUomInner.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUomInner).Location = new System.Drawing.Point(171, 69);
		((System.Windows.Forms.Control)(object)this.CmbPackUomInner).Name = "CmbPackUomInner";
		((System.Windows.Forms.Control)(object)this.CmbPackUomInner).Size = new System.Drawing.Size(94, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUomInner).TabIndex = 11;
		this.CmbPackUomInner.ValueChanged += new System.EventHandler(CmbPackUomInner_ValueChanged);
		this.CmbPackUomMaster.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUomMaster.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUomMaster.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUomMaster.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance250).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Appearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance265).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance265).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance265).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance265).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance265;
		((AppearanceBase)appearance266).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance266;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance267).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance267).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance267).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance267).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance267;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance254).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance254).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance254;
		((AppearanceBase)appearance255).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance255).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance268).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance268;
		((AppearanceBase)appearance269).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance269).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance269;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance270).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance270).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance270).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance270).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance270).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance270;
		((AppearanceBase)appearance271).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance271;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance272).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance272).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance272;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance273).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance273;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUomMaster).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbPackUomMaster).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackUomMaster.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUomMaster).Location = new System.Drawing.Point(1042, 23);
		((System.Windows.Forms.Control)(object)this.CmbPackUomMaster).Name = "CmbPackUomMaster";
		((System.Windows.Forms.Control)(object)this.CmbPackUomMaster).Size = new System.Drawing.Size(94, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUomMaster).TabIndex = 7;
		this.CmbPackUomMaster.ValueChanged += new System.EventHandler(CmbPackUomMaster_ValueChanged);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(173, 53);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(63, 15);
		this.label17.TabIndex = 102;
		this.label17.Text = "Pack UOM";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(1044, 5);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(63, 15);
		this.label38.TabIndex = 103;
		this.label38.Text = "Pack UOM";
		this.txtQtyInner.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQtyInner.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtQtyInner.Location = new System.Drawing.Point(91, 71);
		this.txtQtyInner.Name = "txtQtyInner";
		this.txtQtyInner.Size = new System.Drawing.Size(77, 23);
		this.txtQtyInner.TabIndex = 10;
		this.txtQtyInner.TextChanged += new System.EventHandler(txtQtyInner_TextChanged);
		this.txtQtyInner.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQtyInner_KeyPress);
		this.txtQtyMaster.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQtyMaster.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtQtyMaster.Location = new System.Drawing.Point(962, 25);
		this.txtQtyMaster.Name = "txtQtyMaster";
		this.txtQtyMaster.Size = new System.Drawing.Size(77, 23);
		this.txtQtyMaster.TabIndex = 6;
		this.txtQtyMaster.TextChanged += new System.EventHandler(txtQtyMaster_TextChanged);
		this.txtQtyMaster.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQtyMaster_KeyPress);
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label51.Location = new System.Drawing.Point(91, 53);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(57, 15);
		this.label51.TabIndex = 110;
		this.label51.Text = "Inner Qty";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label52.Location = new System.Drawing.Point(962, 5);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(65, 15);
		this.label52.TabIndex = 111;
		this.label52.Text = "Master Qty";
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(827, 53);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(68, 15);
		this.label50.TabIndex = 101;
		this.label50.Text = "Container #";
		this.label50.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.CmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance140;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance191;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance192).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance193).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance193).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance193).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance193;
		((AppearanceBase)appearance194).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Location = new System.Drawing.Point(559, 23);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Name = "CmbCropYear";
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabIndex = 3;
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(560, 5);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(54, 15);
		this.label40.TabIndex = 44;
		this.label40.Text = "CropYear";
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance217).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance218).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance218).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance218).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance219).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance219;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance220).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance220).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance220).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance221).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance221;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance222).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance223;
		((AppearanceBase)appearance224).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance224).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance225).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance225).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance225).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance225).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance225;
		((AppearanceBase)appearance226).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance228;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(175, 23);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		this.cmbItem.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(206, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbItem).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbItem).Leave += new System.EventHandler(cmbItem_Leave);
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(720, 53);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(79, 15);
		this.label45.TabIndex = 42;
		this.label45.Text = "Stock Weight";
		this.label45.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(431, 53);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(51, 15);
		this.label47.TabIndex = 42;
		this.label47.Text = "AddLess";
		this.label47.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(616, 53);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(78, 15);
		this.label39.TabIndex = 42;
		this.label39.Text = "Gross Weight";
		this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(175, 5);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(67, 15);
		this.label28.TabIndex = 2;
		this.label28.Text = "Item Name";
		this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtStockWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtStockWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtStockWeight.Location = new System.Drawing.Point(720, 71);
		this.txtStockWeight.Name = "txtStockWeight";
		this.txtStockWeight.ReadOnly = true;
		this.txtStockWeight.Size = new System.Drawing.Size(105, 23);
		this.txtStockWeight.TabIndex = 17;
		this.txtStockWeight.TabStop = false;
		this.txtStockWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAddLess.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAddLess.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAddLess.Location = new System.Drawing.Point(431, 71);
		this.txtAddLess.Name = "txtAddLess";
		this.txtAddLess.Size = new System.Drawing.Size(72, 23);
		this.txtAddLess.TabIndex = 14;
		this.txtAddLess.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAddLess.TextChanged += new System.EventHandler(txtAddLess_TextChanged);
		this.txtAddLess.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAddLess_KeyPress);
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrossWeight.Location = new System.Drawing.Point(616, 71);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.ReadOnly = true;
		this.txtGrossWeight.Size = new System.Drawing.Size(102, 23);
		this.txtGrossWeight.TabIndex = 16;
		this.txtGrossWeight.TabStop = false;
		this.txtGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(827, 5);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(57, 15);
		this.label29.TabIndex = 4;
		this.label29.Text = "PackType";
		this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance229).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance230).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance230).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance230).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance231).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance231;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance232).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance232).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance232).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance232;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance233).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance233;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance234).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance234;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance235;
		((AppearanceBase)appearance236).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance236).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance237).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance237).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance237).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance237;
		((AppearanceBase)appearance238).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance239).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance240;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(661, 23);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(164, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 4;
		this.cmbPackType.AllowNull = (DefaultableBoolean)1;
		this.cmbPackType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance241).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance241;
		((UltraGridBase)this.cmbPackType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance242).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance242).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance242).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance242;
		((AppearanceBase)appearance243).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance243;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance244).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance244).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance244).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.cmbPackType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance245).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance245;
		((AppearanceBase)appearance246).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance246).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance246;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance274).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance274;
		((AppearanceBase)appearance275).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance275).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance275;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance276).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance276).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance276).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance276).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance276;
		((AppearanceBase)appearance198).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.cmbPackType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPackType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackType).Location = new System.Drawing.Point(827, 23);
		((System.Windows.Forms.Control)(object)this.cmbPackType).Name = "cmbPackType";
		this.cmbPackType.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbPackType).Size = new System.Drawing.Size(133, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackType).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.cmbPackType).TabStop = false;
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(660, 5);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(43, 15);
		this.label55.TabIndex = 40;
		this.label55.Text = "JobLot";
		this.label55.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.CmbWareHouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance201).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance202).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance202).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance202).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance202;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance203;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance204).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance204).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance205).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance207).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance208;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance209).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance210).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance210).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance210).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance210).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance210;
		((AppearanceBase)appearance262).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance262;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance263).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance263;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance264;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWareHouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Location = new System.Drawing.Point(3, 23);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Name = "CmbWareHouse";
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).TabIndex = 0;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(2, 5);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(69, 15);
		this.label54.TabIndex = 38;
		this.label54.Text = "WareHouse";
		this.label54.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(963, 53);
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
		this.btnDetailCancel.Location = new System.Drawing.Point(1147, 68);
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
		this.btnDetailUpdate.Location = new System.Drawing.Point(1080, 68);
		this.btnDetailUpdate.Name = "btnDetailUpdate";
		this.btnDetailUpdate.Size = new System.Drawing.Size(66, 28);
		this.btnDetailUpdate.TabIndex = 21;
		this.btnDetailUpdate.Text = "Update";
		this.btnDetailUpdate.UseVisualStyleBackColor = false;
		this.btnDetailUpdate.Click += new System.EventHandler(btnDetailUpdate_Click);
		this.txtItemDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemDesc.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemDesc.Location = new System.Drawing.Point(383, 25);
		this.txtItemDesc.Name = "txtItemDesc";
		this.txtItemDesc.Size = new System.Drawing.Size(173, 23);
		this.txtItemDesc.TabIndex = 2;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(383, 5);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(96, 15);
		this.label20.TabIndex = 34;
		this.label20.Text = "Item Description";
		this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.cmbcontainerNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.cmbcontainerNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcontainerNo.Location = new System.Drawing.Point(827, 71);
		this.cmbcontainerNo.Name = "cmbcontainerNo";
		this.cmbcontainerNo.Size = new System.Drawing.Size(133, 23);
		this.cmbcontainerNo.TabIndex = 18;
		this.txtSeal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSeal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSeal.Location = new System.Drawing.Point(963, 71);
		this.txtSeal.Name = "txtSeal";
		this.txtSeal.Size = new System.Drawing.Size(115, 23);
		this.txtSeal.TabIndex = 19;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(504, 53);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(65, 15);
		this.label30.TabIndex = 6;
		this.label30.Text = "NetWeight";
		this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeight.Location = new System.Drawing.Point(506, 71);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.ReadOnly = true;
		this.txtNetWeight.Size = new System.Drawing.Size(108, 23);
		this.txtNetWeight.TabIndex = 15;
		this.txtNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.grdotheritems);
		this.tabPage4.Controls.Add(this.panel11);
		this.tabPage4.Controls.Add(this.panel9);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(1225, 414);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Other Items";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.grdotheritems.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdotheritems).BackColor = System.Drawing.Color.White;
		grdotheritems_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdotheritems.DesignTimeLayout = grdotheritems_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdotheritems.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Location = new System.Drawing.Point(3, 80);
		((System.Windows.Forms.Control)(object)this.grdotheritems).Name = "grdotheritems";
		((ScrollableBase)this.grdotheritems).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Size = new System.Drawing.Size(1219, 331);
		((System.Windows.Forms.Control)(object)this.grdotheritems).TabIndex = 1;
		this.grdotheritems.TotalRow = (InheritableBoolean)1;
		this.grdotheritems.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdotheritems).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdotheritems).DoubleClick += new System.EventHandler(grdotheritems_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdotheritems).KeyDown += new System.Windows.Forms.KeyEventHandler(grdotheritems_KeyDown);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 56);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1219, 24);
		this.panel11.TabIndex = 1;
		this.panel9.Controls.Add(this.btnoadd);
		this.panel9.Controls.Add(this.label2);
		this.panel9.Controls.Add(this.txtOContainer);
		this.panel9.Controls.Add(this.label42);
		this.panel9.Controls.Add(this.btnoCancel);
		this.panel9.Controls.Add(this.btnoUpdate);
		this.panel9.Controls.Add(this.label27);
		this.panel9.Controls.Add(this.txtoRemarks);
		this.panel9.Controls.Add(this.txtoQty);
		this.panel9.Controls.Add(this.label41);
		this.panel9.Controls.Add(this.txtoRate);
		this.panel9.Controls.Add(this.label63);
		this.panel9.Controls.Add(this.label70);
		this.panel9.Controls.Add(this.txtoAmount);
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOtherItem);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 3);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1219, 53);
		this.panel9.TabIndex = 0;
		this.btnoadd.BackColor = System.Drawing.Color.Teal;
		this.btnoadd.FlatAppearance.BorderSize = 0;
		this.btnoadd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnoadd.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnoadd.ForeColor = System.Drawing.Color.White;
		this.btnoadd.Location = new System.Drawing.Point(1062, 21);
		this.btnoadd.Name = "btnoadd";
		this.btnoadd.Size = new System.Drawing.Size(25, 26);
		this.btnoadd.TabIndex = 6;
		this.btnoadd.Text = "+";
		this.btnoadd.UseVisualStyleBackColor = false;
		this.btnoadd.Visible = false;
		this.btnoadd.Click += new System.EventHandler(btnoadd_Click);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(5, 5);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 15);
		this.label2.TabIndex = 523;
		this.label2.Text = "Item Name";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtOContainer.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOContainer.Location = new System.Drawing.Point(515, 23);
		this.txtOContainer.Name = "txtOContainer";
		this.txtOContainer.Size = new System.Drawing.Size(132, 23);
		this.txtOContainer.TabIndex = 4;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(515, 5);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(77, 15);
		this.label42.TabIndex = 522;
		this.label42.Text = "Container No";
		this.btnoCancel.BackColor = System.Drawing.Color.Teal;
		this.btnoCancel.FlatAppearance.BorderSize = 0;
		this.btnoCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnoCancel.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnoCancel.ForeColor = System.Drawing.Color.White;
		this.btnoCancel.Location = new System.Drawing.Point(1136, 21);
		this.btnoCancel.Name = "btnoCancel";
		this.btnoCancel.Size = new System.Drawing.Size(66, 26);
		this.btnoCancel.TabIndex = 8;
		this.btnoCancel.Text = "Cancel";
		this.btnoCancel.UseVisualStyleBackColor = false;
		this.btnoCancel.Visible = false;
		this.btnoCancel.Click += new System.EventHandler(btnoCancel_Click);
		this.btnoUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnoUpdate.FlatAppearance.BorderSize = 0;
		this.btnoUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnoUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnoUpdate.ForeColor = System.Drawing.Color.White;
		this.btnoUpdate.Location = new System.Drawing.Point(1059, 21);
		this.btnoUpdate.Name = "btnoUpdate";
		this.btnoUpdate.Size = new System.Drawing.Size(75, 26);
		this.btnoUpdate.TabIndex = 7;
		this.btnoUpdate.Text = "Update";
		this.btnoUpdate.UseVisualStyleBackColor = false;
		this.btnoUpdate.Visible = false;
		this.btnoUpdate.Click += new System.EventHandler(btnoUpdate_Click);
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(648, 5);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(52, 15);
		this.label27.TabIndex = 521;
		this.label27.Text = "Remarks";
		this.txtoRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtoRemarks.Location = new System.Drawing.Point(649, 23);
		this.txtoRemarks.Name = "txtoRemarks";
		this.txtoRemarks.Size = new System.Drawing.Size(407, 23);
		this.txtoRemarks.TabIndex = 5;
		this.txtoQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtoQty.Location = new System.Drawing.Point(227, 23);
		this.txtoQty.Name = "txtoQty";
		this.txtoQty.Size = new System.Drawing.Size(101, 23);
		this.txtoQty.TabIndex = 1;
		this.txtoQty.Text = "0";
		this.txtoQty.TextChanged += new System.EventHandler(txtoQty_TextChanged);
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(227, 5);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(26, 15);
		this.label41.TabIndex = 518;
		this.label41.Text = "Qty";
		this.txtoRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtoRate.Location = new System.Drawing.Point(330, 23);
		this.txtoRate.Name = "txtoRate";
		this.txtoRate.ReadOnly = true;
		this.txtoRate.Size = new System.Drawing.Size(80, 23);
		this.txtoRate.TabIndex = 2;
		this.txtoRate.Text = "0";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.Location = new System.Drawing.Point(329, 5);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(30, 15);
		this.label63.TabIndex = 520;
		this.label63.Text = "Rate";
		this.label70.AutoSize = true;
		this.label70.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label70.Location = new System.Drawing.Point(411, 5);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(51, 15);
		this.label70.TabIndex = 519;
		this.label70.Text = "Amount";
		this.txtoAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtoAmount.Location = new System.Drawing.Point(412, 23);
		this.txtoAmount.Name = "txtoAmount";
		this.txtoAmount.ReadOnly = true;
		this.txtoAmount.Size = new System.Drawing.Size(101, 23);
		this.txtoAmount.TabIndex = 3;
		this.txtoAmount.Text = "0";
		this.CmbOtherItem.AllowNull = (DefaultableBoolean)1;
		this.CmbOtherItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOtherItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOtherItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance211).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance212).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance212).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance212;
		((AppearanceBase)appearance213).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance213;
		((SpecialBoxBase)((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance280).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance280).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance280).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance280).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance280;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance281).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance281).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance281;
		((AppearanceBase)appearance282).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance282).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance282;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance283).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance283;
		((AppearanceBase)appearance284).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance284).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance284;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance285).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance285).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance285).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance285).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance285).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance285;
		((AppearanceBase)appearance286).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance286;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance287).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance287).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance287;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance288).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance288;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Location = new System.Drawing.Point(5, 21);
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Name = "CmbOtherItem";
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).TabIndex = 0;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.panel12);
		this.panel3.Controls.Add(this.txtCNICNO);
		this.panel3.Controls.Add(this.txtDriverCellNo);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbtransporter);
		this.panel3.Controls.Add(this.txtDriverName);
		this.panel3.Controls.Add(this.label18);
		this.panel3.Controls.Add(this.label5);
		this.panel3.Controls.Add(this.cmbGpNo);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCustomer);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbShippingAgent);
		this.panel3.Controls.Add(this.txtShippedContainer);
		this.panel3.Controls.Add(this.label25);
		this.panel3.Controls.Add(this.txtNoOfContainer);
		this.panel3.Controls.Add(this.label16);
		this.panel3.Controls.Add(this.label24);
		this.panel3.Controls.Add(this.txtCompletedDate);
		this.panel3.Controls.Add(this.label26);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbShippingLIne);
		this.panel3.Controls.Add(this.txtFreight);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDestinationPort);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLoadingPort);
		this.panel3.Controls.Add(this.label22);
		this.panel3.Controls.Add(this.label21);
		this.panel3.Controls.Add(this.label12);
		this.panel3.Controls.Add(this.label19);
		this.panel3.Controls.Add(this.label14);
		this.panel3.Controls.Add(this.txtBilityNo);
		this.panel3.Controls.Add(this.label11);
		this.panel3.Controls.Add(this.label10);
		this.panel3.Controls.Add(this.txtVehicleNo);
		this.panel3.Controls.Add(this.label8);
		this.panel3.Controls.Add(this.label7);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add(this.txtGPDate);
		this.panel3.Controls.Add(this.DocDate);
		this.panel3.Controls.Add(this.txtdocno);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbInvoiceNo);
		this.panel3.Controls.Add(this.txtRemarksHEader);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(3, 33);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1233, 173);
		this.panel3.TabIndex = 0;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.btnRecordsUpdate);
		this.panel12.Controls.Add(this.ChkPrintPreview);
		this.panel12.Controls.Add(this.label36);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1233, 27);
		this.panel12.TabIndex = 101;
		this.btnRecordsUpdate.BackColor = System.Drawing.Color.FromArgb(192, 0, 0);
		this.btnRecordsUpdate.FlatAppearance.BorderColor = System.Drawing.Color.Black;
		this.btnRecordsUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnRecordsUpdate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRecordsUpdate.ForeColor = System.Drawing.Color.White;
		this.btnRecordsUpdate.Location = new System.Drawing.Point(222, 1);
		this.btnRecordsUpdate.Name = "btnRecordsUpdate";
		this.btnRecordsUpdate.Size = new System.Drawing.Size(159, 24);
		this.btnRecordsUpdate.TabIndex = 114;
		this.btnRecordsUpdate.Text = "Auto Update Records";
		this.btnRecordsUpdate.UseVisualStyleBackColor = false;
		this.btnRecordsUpdate.Visible = false;
		this.btnRecordsUpdate.Click += new System.EventHandler(btnRecordsUpdate_Click);
		this.ChkPrintPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrintPreview.AutoSize = true;
		this.ChkPrintPreview.Checked = true;
		this.ChkPrintPreview.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPrintPreview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrintPreview.ForeColor = System.Drawing.Color.White;
		this.ChkPrintPreview.Location = new System.Drawing.Point(1105, 3);
		this.ChkPrintPreview.Name = "ChkPrintPreview";
		this.ChkPrintPreview.Size = new System.Drawing.Size(123, 20);
		this.ChkPrintPreview.TabIndex = 31;
		this.ChkPrintPreview.Text = "Print Preview";
		this.ChkPrintPreview.UseVisualStyleBackColor = true;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.ForeColor = System.Drawing.Color.White;
		this.label36.Location = new System.Drawing.Point(6, 4);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(245, 19);
		this.label36.TabIndex = 0;
		this.label36.Text = "Export Goods Forwarding      ";
		this.txtCNICNO.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCNICNO.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCNICNO.Location = new System.Drawing.Point(402, 117);
		this.txtCNICNO.Name = "txtCNICNO";
		this.txtCNICNO.Size = new System.Drawing.Size(220, 23);
		this.txtCNICNO.TabIndex = 9;
		this.txtDriverCellNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDriverCellNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDriverCellNo.Location = new System.Drawing.Point(402, 60);
		this.txtDriverCellNo.Name = "txtDriverCellNo";
		this.txtDriverCellNo.Size = new System.Drawing.Size(220, 23);
		this.txtDriverCellNo.TabIndex = 7;
		this.cmbtransporter.AllowNull = (DefaultableBoolean)1;
		this.cmbtransporter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbtransporter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbtransporter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance163).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance163).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbtransporter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance164;
		((SpecialBoxBase)((UltraGridBase)this.cmbtransporter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance165).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance166).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance168;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance169).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbtransporter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbtransporter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbtransporter).Location = new System.Drawing.Point(721, 30);
		((System.Windows.Forms.Control)(object)this.cmbtransporter).Name = "cmbtransporter";
		((System.Windows.Forms.Control)(object)this.cmbtransporter).Size = new System.Drawing.Size(226, 26);
		((System.Windows.Forms.Control)(object)this.cmbtransporter).TabIndex = 10;
		this.txtDriverName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDriverName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDriverName.Location = new System.Drawing.Point(402, 89);
		this.txtDriverName.Name = "txtDriverName";
		this.txtDriverName.Size = new System.Drawing.Size(220, 23);
		this.txtDriverName.TabIndex = 8;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(3, 64);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(72, 15);
		this.label18.TabIndex = 98;
		this.label18.Text = "Invoice No#";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(3, 36);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(88, 15);
		this.label5.TabIndex = 97;
		this.label5.Text = "Doc No && Date";
		this.cmbGpNo.BackColor = System.Drawing.Color.White;
		this.cmbGpNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.cmbGpNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbGpNo.Location = new System.Drawing.Point(722, 89);
		this.cmbGpNo.Name = "cmbGpNo";
		this.cmbGpNo.Size = new System.Drawing.Size(112, 23);
		this.cmbGpNo.TabIndex = 12;
		this.cmbCustomer.AllowNull = (DefaultableBoolean)1;
		this.cmbCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance289).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance289).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance289;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance290).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance290).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance290).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance290).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance290;
		((AppearanceBase)appearance291).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance291;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance104).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCustomer).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCustomer).Location = new System.Drawing.Point(93, 87);
		((System.Windows.Forms.Control)(object)this.cmbCustomer).Name = "cmbCustomer";
		((System.Windows.Forms.Control)(object)this.cmbCustomer).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbCustomer).TabIndex = 3;
		this.cmbShippingAgent.AllowNull = (DefaultableBoolean)1;
		this.cmbShippingAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbShippingAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbShippingAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbShippingAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance111;
		((SpecialBoxBase)((UltraGridBase)this.cmbShippingAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance116).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbShippingAgent).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbShippingAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbShippingAgent).Location = new System.Drawing.Point(402, 30);
		((System.Windows.Forms.Control)(object)this.cmbShippingAgent).Name = "cmbShippingAgent";
		((System.Windows.Forms.Control)(object)this.cmbShippingAgent).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbShippingAgent).TabIndex = 6;
		this.txtShippedContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtShippedContainer.Enabled = false;
		this.txtShippedContainer.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtShippedContainer.Location = new System.Drawing.Point(1058, 89);
		this.txtShippedContainer.Name = "txtShippedContainer";
		this.txtShippedContainer.Size = new System.Drawing.Size(160, 23);
		this.txtShippedContainer.TabIndex = 18;
		this.txtShippedContainer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(952, 93);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(105, 15);
		this.label25.TabIndex = 58;
		this.label25.Text = "Shipped Container";
		this.txtNoOfContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNoOfContainer.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoOfContainer.Location = new System.Drawing.Point(1058, 117);
		this.txtNoOfContainer.Name = "txtNoOfContainer";
		this.txtNoOfContainer.Size = new System.Drawing.Size(160, 23);
		this.txtNoOfContainer.TabIndex = 19;
		this.txtNoOfContainer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(3, 121);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(80, 15);
		this.label16.TabIndex = 50;
		this.label16.Text = "Shipping Line";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(952, 121);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(96, 15);
		this.label24.TabIndex = 55;
		this.label24.Text = "No. Of Container";
		this.txtCompletedDate.CustomFormat = "dd-MMM-yy";
		this.txtCompletedDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCompletedDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtCompletedDate.Location = new System.Drawing.Point(1058, 144);
		this.txtCompletedDate.Name = "txtCompletedDate";
		this.txtCompletedDate.ShowCheckBox = true;
		this.txtCompletedDate.Size = new System.Drawing.Size(160, 23);
		this.txtCompletedDate.TabIndex = 20;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(952, 148);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(93, 15);
		this.label26.TabIndex = 52;
		this.label26.Text = "Completed Date";
		this.cmbShippingLIne.AllowNull = (DefaultableBoolean)1;
		this.cmbShippingLIne.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbShippingLIne.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbShippingLIne.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Appearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance123).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance123).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbShippingLIne).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance124;
		((SpecialBoxBase)((UltraGridBase)this.cmbShippingLIne).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance130).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance131).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbShippingLIne).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbShippingLIne.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbShippingLIne).Location = new System.Drawing.Point(93, 115);
		((System.Windows.Forms.Control)(object)this.cmbShippingLIne).Name = "cmbShippingLIne";
		((System.Windows.Forms.Control)(object)this.cmbShippingLIne).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbShippingLIne).TabIndex = 4;
		this.txtFreight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreight.Location = new System.Drawing.Point(722, 60);
		this.txtFreight.Name = "txtFreight";
		this.txtFreight.Size = new System.Drawing.Size(225, 23);
		this.txtFreight.TabIndex = 11;
		this.cmbDestinationPort.AllowNull = (DefaultableBoolean)1;
		this.cmbDestinationPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDestinationPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDestinationPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance137;
		((SpecialBoxBase)((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance143).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance144).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance144).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance144).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDestinationPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Location = new System.Drawing.Point(1058, 30);
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Name = "cmbDestinationPort";
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).TabIndex = 16;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(952, 36);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(93, 15);
		this.label23.TabIndex = 47;
		this.label23.Text = "Destination Port";
		this.cmbLoadingPort.AllowNull = (DefaultableBoolean)1;
		this.cmbLoadingPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLoadingPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLoadingPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance150;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance156).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance157).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance157).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance157).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance158).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLoadingPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Location = new System.Drawing.Point(1058, 58);
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Name = "cmbLoadingPort";
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).TabIndex = 17;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(952, 64);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(75, 15);
		this.label22.TabIndex = 41;
		this.label22.Text = "Loading Port";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(3, 93);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(58, 15);
		this.label21.TabIndex = 36;
		this.label21.Text = "Customer";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(314, 121);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(53, 15);
		this.label12.TabIndex = 37;
		this.label12.Text = "CNIC No";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(314, 36);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(90, 15);
		this.label19.TabIndex = 26;
		this.label19.Text = "Shipping Agent";
		this.label14.AutoSize = true;
		this.label14.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(627, 64);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(91, 15);
		this.label14.TabIndex = 40;
		this.label14.Text = "Freight Amount";
		this.txtBilityNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBilityNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBilityNo.Location = new System.Drawing.Point(836, 117);
		this.txtBilityNo.Name = "txtBilityNo";
		this.txtBilityNo.Size = new System.Drawing.Size(111, 23);
		this.txtBilityNo.TabIndex = 15;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(314, 64);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(80, 15);
		this.label11.TabIndex = 32;
		this.label11.Text = "Driver Cell No";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(314, 93);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(74, 15);
		this.label10.TabIndex = 25;
		this.label10.Text = "Driver Name";
		this.txtVehicleNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleNo.Location = new System.Drawing.Point(722, 117);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.Size = new System.Drawing.Size(112, 23);
		this.txtVehicleNo.TabIndex = 14;
		this.label8.AutoSize = true;
		this.label8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(627, 121);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(93, 15);
		this.label8.TabIndex = 65;
		this.label8.Text = "Vehicle && Bilty #";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(627, 36);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(67, 15);
		this.label7.TabIndex = 62;
		this.label7.Text = "Transporter";
		this.label4.AutoSize = true;
		this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(627, 93);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(83, 15);
		this.label4.TabIndex = 56;
		this.label4.Text = "GP NO && Date";
		this.txtGPDate.CustomFormat = "dd-MMM-yy";
		this.txtGPDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGPDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtGPDate.Location = new System.Drawing.Point(836, 89);
		this.txtGPDate.Name = "txtGPDate";
		this.txtGPDate.Size = new System.Drawing.Size(111, 23);
		this.txtGPDate.TabIndex = 13;
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(175, 32);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(138, 23);
		this.DocDate.TabIndex = 1;
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(93, 32);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(80, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.TabStop = false;
		this.cmbInvoiceNo.AllowNull = (DefaultableBoolean)1;
		this.cmbInvoiceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbInvoiceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbInvoiceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance175).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance176).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance176).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance177;
		((SpecialBoxBase)((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance178).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance178).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance178).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance182).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance183).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance183).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance183).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance183;
		((AppearanceBase)appearance185).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbInvoiceNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Location = new System.Drawing.Point(93, 58);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Name = "cmbInvoiceNo";
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Leave += new System.EventHandler(cmbInvoiceNo_Leave);
		this.txtRemarksHEader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksHEader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksHEader.Location = new System.Drawing.Point(93, 144);
		this.txtRemarksHEader.Name = "txtRemarksHEader";
		this.txtRemarksHEader.Size = new System.Drawing.Size(854, 23);
		this.txtRemarksHEader.TabIndex = 5;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(3, 148);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(52, 15);
		this.label1.TabIndex = 34;
		this.label1.Text = "Remarks";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1233, 30);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.BtnNew, this.btnRefresh, this.btnSave, this.btnUpdate, this.BtnPrint, this.btnDelete, this.btnAttachment, this.btnShortCutKey });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1233, 27);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.BtnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNew.Name = "BtnNew";
		this.BtnNew.Size = new System.Drawing.Size(60, 24);
		this.BtnNew.Text = "&New";
		this.BtnNew.Click += new System.EventHandler(BtnNew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(81, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(65, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(79, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(74, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.BtnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint.Name = "BtnPrint";
		this.BtnPrint.Size = new System.Drawing.Size(117, 24);
		this.BtnPrint.Text = "507_01-Print";
		this.BtnPrint.Click += new System.EventHandler(BtnPrint_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(110, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(127, 24);
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
		this.tabPage2.Size = new System.Drawing.Size(1239, 652);
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 130);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1233, 283);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
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
		this.panel13.Location = new System.Drawing.Point(3, 413);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1233, 19);
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
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Location = new System.Drawing.Point(3, 432);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Name = "grdHistoryDetail";
		((ScrollableBase)this.grdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(1233, 217);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabIndex = 1;
		this.grdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label92);
		this.panel6.Controls.Add(this.ctrlGrdBar2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 107);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1233, 23);
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
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1184, 0);
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
		this.panel10.Size = new System.Drawing.Size(1233, 52);
		this.panel10.TabIndex = 0;
		this.groupBox2.Controls.Add(this.rdentrydate);
		this.groupBox2.Controls.Add(this.rdapproveddate);
		this.groupBox2.Controls.Add(this.btnShowHistory);
		this.groupBox2.Controls.Add(this.rdmodifydate);
		this.groupBox2.Controls.Add(this.label31);
		this.groupBox2.Controls.Add(this.drdocdate);
		this.groupBox2.Controls.Add(this.label73);
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
		this.groupBox2.Size = new System.Drawing.Size(1224, 48);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(1037, 28);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(1118, 28);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(974, 15);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 5;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(1118, 12);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(640, 21);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(58, 15);
		this.label31.TabIndex = 87;
		this.label31.Text = "Customer";
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(1037, 12);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(533, 21);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(45, 15);
		this.label73.TabIndex = 86;
		this.label73.Text = "To Doc";
		this.label89.AutoSize = true;
		this.label89.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label89.Location = new System.Drawing.Point(407, 21);
		this.label89.Name = "label89";
		this.label89.Size = new System.Drawing.Size(60, 15);
		this.label89.TabIndex = 85;
		this.label89.Text = "From Doc";
		this.label90.AutoSize = true;
		this.label90.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label90.Location = new System.Drawing.Point(214, 21);
		this.label90.Name = "label90";
		this.label90.Size = new System.Drawing.Size(48, 15);
		this.label90.TabIndex = 84;
		this.label90.Text = "To Date";
		this.label91.AutoSize = true;
		this.label91.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label91.Location = new System.Drawing.Point(3, 21);
		this.label91.Name = "label91";
		this.label91.Size = new System.Drawing.Size(63, 15);
		this.label91.TabIndex = 83;
		this.label91.Text = "From Date";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance247).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance248).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance248).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance248).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance248;
		((AppearanceBase)appearance249).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance249;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance251).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance251).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance251).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance251;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance252).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance252;
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance253).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance253;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance256;
		((AppearanceBase)appearance257).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance257).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance257;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance258).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance258).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance258).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance258).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance258;
		((AppearanceBase)appearance259).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance259;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance260).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance260).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance260;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance261;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(701, 15);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(269, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 4;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(578, 17);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(62, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(468, 17);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(62, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(262, 17);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(142, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(69, 17);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(142, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label69);
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1233, 52);
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
		this.toolStrip2.Size = new System.Drawing.Size(1233, 27);
		this.toolStrip2.TabIndex = 53;
		this.toolStrip2.Text = "toolStrip2";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnResetHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(60, 24);
		this.btnResetHistory.Text = "&New";
		this.btnResetHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(81, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1247, 681);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "EximForwardingDirectNew";
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
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUomInner).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUomMaster).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).EndInit();
		this.tabPage4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdotheritems).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOtherItem).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbtransporter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbShippingAgent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbShippingLIne).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDestinationPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoadingPort).EndInit();
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
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
