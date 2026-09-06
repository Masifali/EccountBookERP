using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Export;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Architecture.WinApp.Sale;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Export;

public class ExportDeliveryOrderB : Form
{
	public int RecId;

	private int updateDetailIndex = -1;

	private int updateDetailIndexOtherItem = -1;

	private int TotalDoNetWeightOfDoBeingUpdated;

	private int DefaultDaysToLessFromHistoryFromDate;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private bool IsApproved;

	private bool FormInitialized;

	private bool RestrictOneInvoiceOnDo;

	private bool ContainerNoCompulsoryOnDeliveryOrderExport;

	private bool SaleCostingJobOrderWise;

	private bool IsInspectionLotMappedOnDeliveryOrderByInvoice;

	private bool IsMandatoryInspectionLotMappedOnDeliveryOrder;

	private static bool bool_EbMasterUnit;

	private static bool bool_EbMasterTotal;

	private static bool bool_EbInnerUnit;

	private static bool bool_EbInnerTotal;

	private DataTable dtWh = new DataTable();

	private DataTable dtjob = new DataTable();

	private DataTable table = new DataTable();

	private DataTable dtCont = new DataTable();

	private DataTable dtExport = new DataTable();

	private DataTable dtInvoice = new DataTable();

	private DataTable dtHistory = new DataTable();

	private DataTable dtsuppcus = new DataTable();

	private DataTable BranchList = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private DataTable dtPackingType = new DataTable();

	private DataTable dtOtherItemCombo = new DataTable();

	private DataTable dtContFromReadById = new DataTable();

	private DataTable dtJobLotWithJobOrderAndItems = new DataTable();

	private Attachment AT = new Attachment();

	private LoadCommercialInvoiceForDO LoadPO;

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formrights = new Rightsobjects();

	public UserAccount UserAccount = new UserAccount();

	private List<InvDeliveryOrderdetail> lstRemoveRecord = new List<InvDeliveryOrderdetail>();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private TabPage tabPage2;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnprint;

	private ToolStripButton btnattachment;

	private Label label23;

	private Panel panel4;

	private Label label33;

	private DateTimePicker DocDate;

	private Label lblBranchName;

	private TextBox txtdocno;

	private Label label5;

	private Panel panel6;

	private CtrlGrdBar ctrlGrdBar2;

	internal GridEX grdhistory;

	private ToolStripButton btnRefresh;

	private ToolStrip toolStrip2;

	internal GridEX grd;

	private Label label4;

	internal UltraCombo cmbInvoiceNo;

	private GroupBox groupBox3;

	internal UltraCombo CmbJobLot;

	private Label label8;

	internal UltraCombo CmbWareHouse;

	private Label label3;

	internal UltraCombo CmbSupplierCustomer;

	private Label label2;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	internal UltraCombo CmbPackingType;

	internal UltraCombo CmbPackUomMaster;

	internal UltraCombo CmbOrderNo;

	internal UltraCombo CmbItemName;

	private Button btnplus;

	private TextBox txtremarksdetail;

	private Label label47;

	private TextBox txtNetWeight;

	private TextBox txtQtyMaster;

	private Label label35;

	private Label label45;

	private Label label39;

	private Label label38;

	private Label label36;

	private Label label37;

	private TextBox txtGrossWeight;

	private Label label7;

	private TextBox txtEbTotalInner;

	private Label label6;

	private TextBox txtContainerRemarks;

	private Label label10;

	private TextBox txtInspectionRemarks;

	private Label label9;

	private TextBox txtBalanceWeight;

	private Label label15;

	private TextBox txtDoWeight;

	private Label label14;

	private TextBox txtInvoiceWeight;

	private Label label12;

	private DateTimePicker txtInvoiceDate;

	private Label label11;

	internal UltraCombo CmbCropYear;

	private Label label16;

	private TextBox txtrejectedWeight;

	private Label label21;

	private TextBox txtEbUnitInner;

	private Label label1;

	private GroupBox groupBox1;

	private GroupBox groupBox2;

	internal GridEX grdDetail;

	private Panel panel8;

	private Panel panel7;

	private Panel panel5;

	private Label label19;

	private Label label13;

	private TextBox txtVehicleNo;

	private Label label20;

	internal UltraCombo cmbContainerNoDetail;

	private Label label22;

	private ToolStripButton btnShortCutKeys;

	private GroupBox groupBox4;

	private Label label24;

	private TextBox txtOtherRemarks;

	private TextBox txtTotalGrossWeight;

	private Label label25;

	private Label label26;

	private Label label27;

	private TextBox txtPackingWeight;

	private TextBox txtOtherWeight;

	private Label label28;

	private TextBox txtItemWeight;

	private Label label30;

	private TextBox txtEbTotalMaster;

	private Label label29;

	private TextBox txtEbUnitMaster;

	internal UltraCombo CmbPackUomInner;

	private Label label32;

	private TextBox txtQtyInner;

	private Label label31;

	internal UltraCombo CmbLoadingPort;

	private ToolStripButton btnExportDoSlip396;

	internal UltraCombo CmbBranch;

	private Panel panel9;

	private Label label18;

	private Label label17;

	private GroupBox groupBox5;

	private RadioButton rdapproveddate;

	private Button btnShowHistory;

	private RadioButton rdmodifydate;

	private Label label34;

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

	private CheckBox ChkPreview;

	private ToolStripButton btnPurchaseOrderLoader;

	private TabControl tabControl2;

	private TabPage tabDetail;

	private TabPage tabOtherItem;

	internal GridEX grdotheritems;

	private Panel panel10;

	private CtrlGrdBar ctrlGrdBar3;

	private Panel panel19;

	private Button btnoCancel;

	internal UltraCombo CmbOtherItem;

	private Button btnoUpdate;

	private Label label71;

	private Button btnoadd;

	private Label label40;

	private TextBox txtoRemarks;

	private TextBox txtoQty;

	private Label label41;

	private Panel panel18;

	private TextBox txtremarks;

	private TextBox txtAccessWtSet;

	private Label label42;

	private Label label43;

	private TextBox txtTotalAccessWtSet;

	private TextBox txtOtherItemTotalWt;

	private Label label44;

	private TextBox txtNetWeightOtherItem;

	private Label label48;

	private TextBox txtWtPerQtyOtherItem;

	private Label label46;

	private TextBox txtExportReturnWeight;

	private Label label49;

	private ToolStripButton btnCroForm;

	internal UltraCombo cmbtransporter;

	private Label label50;

	private Label label52;

	private Label label51;

	private Label label53;

	internal UltraCombo CmbInvoiceNoHistory;

	internal UltraCombo CmbInspectionSubLot;

	private Label label54;

	internal UltraCombo CmbInspectionNoPackListDetail;

	private Label label144;

	private Label txtBalnceMtonOfSelectedItem;

	private LinkLabel labelEximForwardingForm;

	private LinkLabel labelGatePassForm;

	private RadioButton RadNoneFormToOpenOnInsert;

	private RadioButton RadGrnFormToOpenOnInsert;

	private RadioButton RadWBFormToOpenOnInsert;

	public ExportDeliveryOrderB(UserAccount user)
	{
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void PurchsaeOrder_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1250, 600);
			FormInitialized = true;
			formrights = CommonServices.SetRightsValueInRightsObject("ExportDeliveryOrderB");
			btnsave.Enabled = formrights.DoHaveSaveRight;
			btnupdate.Enabled = formrights.DoHaveUpdateRights;
			btnprint.Enabled = formrights.DoHavePrintRights;
			btnExportDoSlip396.Enabled = formrights.DoHavePrintRights;
			SaleCostingJobOrderWise = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("SaleCostingJobOrderWise"));
			IsInspectionLotMappedOnDeliveryOrderByInvoice = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("IsInspectionLotMappedOnDeliveryOrderByInvoice"));
			IsMandatoryInspectionLotMappedOnDeliveryOrder = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("IsMandatoryInspectionLotMappedOnDeliveryOrder"));
			_ = SaleCostingJobOrderWise;
			dtCont.Columns.Add("ContainerId");
			dtCont.Columns.Add("ContainerNo");
			dtContFromReadById.Columns.Add("ContainerId");
			dtContFromReadById.Columns.Add("ContainerNo");
			table.Columns.Add("Id", typeof(int));
			table.Columns.Add("InvoiceDetailId", typeof(int));
			table.Columns.Add("InvoiceId", typeof(int));
			table.Columns.Add("InvoiceNo");
			table.Columns.Add("SupplierCustomerId", typeof(int));
			table.Columns.Add("SupplierCustomer");
			table.Columns.Add("OrderId", typeof(int));
			table.Columns.Add("OrderNo");
			table.Columns.Add("WareHouseId", typeof(int));
			table.Columns.Add("WareHouse");
			table.Columns.Add("JobLotId", typeof(int));
			table.Columns.Add("JobLot");
			table.Columns.Add("CropYearId", typeof(int));
			table.Columns.Add("CropYear");
			table.Columns.Add("ItemId", typeof(int));
			table.Columns.Add("Item");
			table.Columns.Add("PackingTypeId", typeof(int));
			table.Columns.Add("PackingType");
			table.Columns.Add("MasterQTY", typeof(double));
			table.Columns.Add("MasterUOMId", typeof(int));
			table.Columns.Add("MasterUOM");
			table.Columns.Add("MasterUOMEquivalent", typeof(double));
			table.Columns.Add("EbUnitMaster", typeof(double));
			table.Columns.Add("EbTotalMaster", typeof(double));
			table.Columns.Add("InnerQTY", typeof(double));
			table.Columns.Add("InnerUOMId", typeof(int));
			table.Columns.Add("InnerUOM");
			table.Columns.Add("InnerUOMEquivalent", typeof(double));
			table.Columns.Add("EbUnitInner", typeof(double));
			table.Columns.Add("EbTotalInner", typeof(double));
			table.Columns.Add("AccessWtSet", typeof(double));
			table.Columns.Add("NetWeight", typeof(double));
			table.Columns.Add("LoadQty", typeof(double));
			table.Columns.Add("LoadWeight", typeof(double));
			table.Columns.Add("GrossWeight", typeof(double));
			table.Columns.Add("OtherWeight", typeof(double));
			table.Columns.Add("ThirdPartyAnalysisId", typeof(int));
			table.Columns.Add("ThirdPartyAnalysisNo");
			table.Columns.Add("ThirdPartyAnalysisSubId", typeof(int));
			table.Columns.Add("ThirdPartyAnalysisSubNo");
			table.Columns.Add("ContainerId", typeof(int));
			table.Columns.Add("ContainerNo");
			table.Columns.Add("ContainerRemarks");
			table.Columns.Add("InspectionRemarks");
			table.Columns.Add("Remarks");
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			dtOtherItem.Columns.Add("Id", typeof(int));
			dtOtherItem.Columns.Add("ExImInvoiceId", typeof(int));
			dtOtherItem.Columns.Add("InvoiceDetailId", typeof(int));
			dtOtherItem.Columns.Add("InvoiceNo", typeof(string));
			dtOtherItem.Columns.Add("ItemId", typeof(int));
			dtOtherItem.Columns.Add("ItemName");
			dtOtherItem.Columns.Add("Qty", typeof(double));
			dtOtherItem.Columns.Add("WeightPerQty", typeof(double));
			dtOtherItem.Columns.Add("NetWeight", typeof(double));
			dtOtherItem.Columns.Add("Remarks");
			grdotheritems.DataSource = dtOtherItem;
			grdotheritems.RetrieveStructure();
			grdSettingsOtherItems();
			if (dtWh.Columns.Count == 0)
			{
				dtWh.Columns.Add("Id");
				dtWh.Columns.Add("WareHouseName");
			}
			CmbInvoiceNoFill();
			CmbExportPackingTypeFill();
			DocumentNoFill();
			WareHouseFill();
			CmbLoadingPortFill();
			TransporterFill();
			JobLotFill();
			OtherItemsBind();
			DocDate.Focus();
			btnupdate.Visible = false;
			btnsave.Visible = true;
			btnplus.Visible = true;
			btnprint.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			BranchList = CommonServices.BrancheServiceBind();
			if (BranchList.Rows.Count > 1)
			{
				lblBranchName.Visible = true;
				((Control)(object)CmbBranch).Visible = true;
			}
			else
			{
				lblBranchName.Visible = false;
				((Control)(object)CmbBranch).Visible = false;
			}
			BranchFill();
			CmbBranch.Value = UserAccount.BranchesId;
			List<ConfigrationsAllocation> list2 = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "DefaultDaysToLessFromHistoryFromDate").ToList();
			if (list2.Count > 0)
			{
				DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)list2[0].ConfigKey.ToString());
			}
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "ContainerNoCompulsoryOnDeliveryOrderExport").ToList();
			if (list.Count > 0)
			{
				ContainerNoCompulsoryOnDeliveryOrderExport = Conversion.ToBool((object)list[0].ConfigKey.ToString());
			}
			RestrictOneInvoiceOnDo = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("RestrictOneInvoiceOnDo"));
			defaultConfiquration();
			HistoryCombosFill();
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDateHistory.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumentNoFill()
	{
		try
		{
			int code = CommonServices.DeliveryOrderGenerateCode(84);
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

	private void BranchFill()
	{
		try
		{
			if (BranchList.Rows.Count > 0)
			{
				DDL.BindDDLNew(BranchList, CmbBranch, "Id", "BranchName", "Branch Name", false);
				((UltraGridBase)CmbBranch).Rows[0].Activate();
			}
			if (BranchList.Rows.Count > 1)
			{
				lblBranchName.Visible = true;
				((Control)(object)CmbBranch).Visible = true;
			}
			else
			{
				lblBranchName.Visible = false;
				((Control)(object)CmbBranch).Visible = false;
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
			int Id = Conversion.ToInt(CmbWareHouse.Value);
			dtWh = CommonServices.getActiveWareHouse();
			if (dtWh.Rows.Count > 0)
			{
				DDL.BindDDL(dtWh, CmbWareHouse, "Id", "WareHouseName", "WareHouseName", ZeroIndex: true);
				CommonServices.SetComboValue(CmbWareHouse, "Id", Id, dtWh);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OtherItemsBind()
	{
		try
		{
			dtOtherItemCombo.Clear();
			int Id = Conversion.ToInt(CmbOtherItem.Value);
			dtOtherItemCombo = CommonServices.GetItemByItemTypeId("14");
			if (dtOtherItemCombo.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtOtherItemCombo, CmbOtherItem, "Id", "ItemName", "Item Name", true);
				CommonServices.SetComboValue(CmbOtherItem, "Id", Id, dtOtherItem);
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
			dtjob = CommonServices.JobLotGetAllService();
			if (dtjob.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtjob, CmbJobLot, "Id", "JobLotDescription", "JobLot", true);
				CommonServices.SetComboValue(CmbJobLot, "Id", Id, dtjob);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindPackUomAndPackingType(int ID)
	{
		try
		{
			_ = ((Control)(object)CmbPackUomMaster).Text;
			_ = ((Control)(object)CmbPackUomInner).Text;
			((Control)(object)CmbPackUomMaster).Text = string.Empty;
			((Control)(object)CmbPackUomInner).Text = string.Empty;
			DataTable dt = ExImInvoice.GetInvoiceDetailForDetailId(ID);
			if (dt != null && dt.Rows.Count > 0)
			{
				txtEbTotalInner.Text = Conversion.ToString(dt.Rows[0]["TotalPackingWeight"]);
				txtEbUnitInner.Text = Conversion.ToString(dt.Rows[0]["PackingWeight"]);
				if (Conversion.ToInt(dt.Rows[0]["JobLotId"]) > 0)
				{
					CmbJobLot.Value = Conversion.ToInt(dt.Rows[0]["JobLotId"]);
				}
				DataTable dtUom = new DataTable();
				dtUom.Columns.Add("PackUomId", typeof(int));
				dtUom.Columns.Add("PackUom", typeof(string));
				dtUom.Columns.Add("Equivalent", typeof(double));
				DataTable dtInnerUom = new DataTable();
				dtInnerUom.Columns.Add("PackUomId", typeof(int));
				dtInnerUom.Columns.Add("PackUom", typeof(string));
				dtInnerUom.Columns.Add("Equivalent", typeof(double));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtUom.Rows.Add(dt.Rows[i]["PackUomId"], dt.Rows[i]["PackUom"], dt.Rows[i]["Equivalent"]);
					dtInnerUom.Rows.Add(dt.Rows[i]["InnerPackUomId"], dt.Rows[i]["InnerPackUom"], dt.Rows[i]["InnerPackEquivalent"]);
				}
				if (dtUom.Rows.Count > 0)
				{
					DDL.BindDDL(dtUom, CmbPackUomMaster, "PackUomId", "PackUom", "PackUom Master", ZeroIndex: true);
					((UltraGridBase)CmbPackUomMaster).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)CmbPackUomMaster).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
					((UltraGridBase)CmbPackUomMaster).Rows[1].Activate();
				}
				if (dtInnerUom.Rows.Count > 0)
				{
					DDL.BindDDL(dtInnerUom, CmbPackUomInner, "PackUomId", "PackUom", "Pack Uom Inner", ZeroIndex: true);
					((UltraGridBase)CmbPackUomInner).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)CmbPackUomInner).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
					((UltraGridBase)CmbPackUomInner).Rows[1].Activate();
				}
			}
			else
			{
				((UltraGridBase)CmbPackUomMaster).DataSource = null;
				((UltraGridBase)CmbPackUomInner).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindInnerPackUom(int ItemId)
	{
		try
		{
			string PackUomInner = ((Control)(object)CmbPackUomInner).Text;
			((Control)(object)CmbPackUomInner).Text = string.Empty;
			DataTable dt = CommonServices.GetUomScheduleByItemId(ItemId);
			if (dt != null && dt.Rows.Count > 0)
			{
				DataTable dtUom = new DataTable();
				dtUom.Columns.Add("PackUomId", typeof(int));
				dtUom.Columns.Add("PackUom", typeof(string));
				dtUom.Columns.Add("Equivalent", typeof(double));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtUom.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["UOMCode"], dt.Rows[i]["Equivalent"]);
				}
				if (dtUom.Rows.Count > 0)
				{
					DDL.BindDDL(dtUom, CmbPackUomInner, "PackUomId", "PackUom", "PackUom Inner", ZeroIndex: true);
					((UltraGridBase)CmbPackUomInner).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)CmbPackUomInner).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
					((UltraGridBase)CmbPackUomInner).Rows[1].Activate();
					if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("PackUom") == PackUomInner))
					{
						((Control)(object)CmbPackUomInner).Text = PackUomInner;
					}
					else
					{
						((Control)(object)CmbPackUomInner).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbPackUomInner).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbExportPackingTypeFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbJobLot.Value);
			dtPackingType = InvPackingType.Getall();
			if (dtPackingType.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dtPackingType, CmbPackingType, "Id", "PackTypeDesc", "Packing Type", true);
			((UltraGridBase)CmbPackingType).Rows[1].Activate();
			if (Id > 0)
			{
				if (dtPackingType.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					CmbPackingType.Value = Id;
				}
				else
				{
					((Control)(object)CmbJobLot).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbInvoiceNoFill()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		try
		{
			int InvoiceId = Conversion.ToInt(cmbInvoiceNo.Value);
			DataTable dt = new DataTable();
			ExImInvoice obj = new ExImInvoice();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.Id = ((RecId > 0) ? RecId : 0);
			dtInvoice = ExImInvoice.getOutstandingExportInvoicesForDeliveryOrder(obj);
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("InvoiceNo");
			for (int i = 0; i < dtInvoice.Rows.Count; i++)
			{
				dt.Rows.Add(dtInvoice.Rows[i]["Id"], dtInvoice.Rows[i]["InvoiceNo"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbInvoiceNo, "Id", "InvoiceNo", "Invoice No", ZeroIndex: true);
				if (InvoiceId > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == InvoiceId))
					{
						cmbInvoiceNo.Value = InvoiceId;
					}
					else
					{
						((Control)(object)cmbInvoiceNo).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)cmbInvoiceNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TransporterFill()
	{
		try
		{
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
			DDL.BindDDL(dt, cmbtransporter, "Id", "CompanyName", "Transporter", ZeroIndex: true);
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

	private void CmbLoadingPortFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbLoadingPort.Value);
			DataTable dt = CommonServices.GetSeaPortForComboServiceBind();
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtPorts = new DataTable();
			dtPorts.Columns.Add("Id", typeof(int));
			dtPorts.Columns.Add("PortName");
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToInt(dt.Rows[i]["PortTypeId"]) == 2)
				{
					dtPorts.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["PortName"]);
				}
			}
			if (dtPorts.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtPorts, CmbLoadingPort, "Id", "PortName", "Port Name", true);
				if (Id > 0)
				{
					if (dtPorts.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbLoadingPort.Value = Id;
					}
					else
					{
						((Control)(object)CmbLoadingPort).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbLoadingPort).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void defaultConfiquration()
	{
		try
		{
			List<ConfigrationsAllocation> lstJob = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Job/Lot").ToList();
			if (lstJob.Count > 0)
			{
				int JobId = Conversion.ToInt((object)lstJob[0].ConfigKey.ToString());
				CmbJobLot.Value = JobId;
			}
			List<ConfigrationsAllocation> lstWareHouse = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Warehouse").ToList();
			if (lstWareHouse.Count > 0)
			{
				int WareHouseId = Conversion.ToInt((object)lstWareHouse[0].ConfigKey.ToString());
				CmbWareHouse.Value = WareHouseId;
			}
			RestrictOneInvoiceOnDo = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("RestrictOneInvoiceOnDo"));
		}
		catch (Exception)
		{
			MessageBox.Show("Configuration Error");
		}
	}

	private void ThirdPartyNoBind(int ItemId, int InvoiceId)
	{
		try
		{
			DataTable dt = new DataTable();
			if (ItemId > 0 && InvoiceId > 0)
			{
				dt = InvLabPreProductionExportLotInspectionHeader.LabPreProductionExportLotInspectionHeader_GetByItemId(UserAccount.OrganizationId, UserAccount.CompanyId, clsGlobalVariables.ActiveYr.Id, ItemId, InvoiceId);
			}
			InfragisticsHelper.BindAndRetainSelection(CmbInspectionNoPackListDetail, dt, "Id", "InspectionNo", "Inspection No", AllColumns: true, Conversion.ToInt(CmbInspectionNoPackListDetail.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ThirdPartySubLotNoBind(int AnalysisId)
	{
		try
		{
			int InvoiceId = Conversion.ToInt(cmbInvoiceNo.Value);
			DataTable dt = new DataTable();
			if (AnalysisId > 0)
			{
				dt = InvLabPreProductionExportLotInspectionHeader.ThirdPartyInspectionSubLot_GetByLotId(UserAccount.OrganizationId, UserAccount.CompanyId, AnalysisId, InvoiceId);
			}
			InfragisticsHelper.BindAndRetainSelection(CmbInspectionSubLot, dt, "Id", "SubLot", "Inspection Sub Lot No", AllColumns: true, Conversion.ToInt(CmbInspectionSubLot.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (lblBranchName.Visible && ((UltraGridBase)CmbBranch).ActiveRow == null)
		{
			MessageBox.Show("Branch field is required");
			CmbBranch.Focus();
			return false;
		}
		if (txtdocno.Text.Trim() == string.Empty || txtdocno.Text.Trim() == "0")
		{
			MessageBox.Show("DocNo Field is Required");
			txtdocno.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtItemWeight.Text) == 0.0)
		{
			MessageBox.Show("Item Weight Field is Required");
			txtItemWeight.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtTotalGrossWeight.Text) == 0.0)
		{
			MessageBox.Show("Total Gross Weight Field is Required");
			txtTotalGrossWeight.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)cmbInvoiceNo).ActiveRow == null || Conversion.ToInt(cmbInvoiceNo.Value) == 0)
		{
			MessageBox.Show("InvoiceNo field is required");
			cmbInvoiceNo.Focus();
			return false;
		}
		if (((UltraGridBase)CmbSupplierCustomer).ActiveRow == null || Conversion.ToInt(CmbSupplierCustomer.Value) == 0)
		{
			MessageBox.Show("Customer field is required");
			CmbSupplierCustomer.Focus();
			return false;
		}
		if (((UltraGridBase)CmbOrderNo).ActiveRow == null || Conversion.ToInt(CmbOrderNo.Value) == 0)
		{
			MessageBox.Show("OrderNo field is required");
			CmbOrderNo.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWareHouse).ActiveRow == null || Conversion.ToInt(CmbWareHouse.Value) == 0)
		{
			MessageBox.Show("Warehouse field is required");
			CmbWareHouse.Focus();
			return false;
		}
		if (((UltraGridBase)CmbItemName).ActiveRow == null || Conversion.ToInt(CmbItemName.Value) == 0)
		{
			MessageBox.Show("ItemName field is required");
			CmbItemName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobLot).ActiveRow == null || Conversion.ToInt(CmbJobLot.Value) == 0)
		{
			MessageBox.Show("JobLot field is required");
			CmbJobLot.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCropYear).ActiveRow == null || Conversion.ToInt(CmbCropYear.Value) == 0)
		{
			MessageBox.Show("CropYear field is required");
			CmbCropYear.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackingType).ActiveRow == null || Conversion.ToInt(CmbPackingType.Value) == 0)
		{
			MessageBox.Show("Packing type field is required");
			CmbPackingType.Focus();
			return false;
		}
		if (txtQtyMaster.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtQtyMaster.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Master Qty field is required");
			txtQtyMaster.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUomMaster).ActiveRow == null || Conversion.ToInt(CmbPackUomMaster.Value) == 0)
		{
			MessageBox.Show("Master PackUOM field is required");
			CmbPackUomMaster.Focus();
			return false;
		}
		if (txtNetWeight.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtNetWeight.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Net Weight field is required");
			txtNetWeight.Focus();
			return false;
		}
		if (SaleCostingJobOrderWise && Conversion.ToInt(CmbInspectionNoPackListDetail.Value) == 0)
		{
			MessageBox.Show("Inspection No field is required");
			CmbInspectionNoPackListDetail.Focus();
			return false;
		}
		if (ContainerNoCompulsoryOnDeliveryOrderExport && (((UltraGridBase)cmbContainerNoDetail).ActiveRow == null || Conversion.ToInt(cmbContainerNoDetail.Value) == 0))
		{
			MessageBox.Show("ContainerNo field is required");
			cmbContainerNoDetail.Focus();
			return false;
		}
		return true;
	}

	private bool OtherItemFormValidation()
	{
		if (Conversion.ToInt(CmbOtherItem.Value) == 0)
		{
			MessageBox.Show("Item field required");
			CmbOtherItem.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtoQty.Text) == 0.0)
		{
			MessageBox.Show("Qty field required");
			txtoQty.Focus();
			return false;
		}
		return true;
	}

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationDetail())
			{
				return;
			}
			if (grd.RowCount > 0 && RestrictOneInvoiceOnDo && !GridEX_Helper.IsAnyCellValueHaveSameValue(grd, "InvoiceId", Conversion.ToInt(cmbInvoiceNo.Value)))
			{
				throw new Exception("Data Against another Invoice Exists in Grid,So you can't add different Invoice");
			}
			if (IsMandatoryInspectionLotMappedOnDeliveryOrder)
			{
				if (IsInspectionLotMappedOnDeliveryOrderByInvoice)
				{
					if (((UltraGridBase)CmbInspectionNoPackListDetail).ActiveRow == null || Conversion.ToInt(CmbInspectionNoPackListDetail.Value) == 0)
					{
						CmbInspectionNoPackListDetail.Focus();
						MessageBox.Show("Inspection No field is required");
						return;
					}
					if (((UltraGridBase)CmbInspectionSubLot).ActiveRow == null || Conversion.ToInt(CmbInspectionSubLot.Value) == 0)
					{
						CmbInspectionSubLot.Focus();
						MessageBox.Show("Inspection SubLot No field is required");
						return;
					}
				}
				else if (((UltraGridBase)CmbInspectionNoPackListDetail).ActiveRow == null || Conversion.ToInt(CmbInspectionNoPackListDetail.Value) == 0)
				{
					CmbInspectionNoPackListDetail.Focus();
					MessageBox.Show("Inspection No field is required");
					return;
				}
			}
			int InvoiceDetailId = 0;
			if (((UltraGridBase)CmbItemName).ActiveRow != null)
			{
				InvoiceDetailId = Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["InvoiceDetailId"].Value);
			}
			int ContainerId = 0;
			string ContainerNo = "";
			if (((UltraGridBase)cmbContainerNoDetail).ActiveRow != null && Conversion.ToInt(cmbContainerNoDetail.Value) != 0)
			{
				ContainerId = Conversion.ToInt(cmbContainerNoDetail.Value);
				ContainerNo = Conversion.ToString((object)((Control)(object)cmbContainerNoDetail).Text);
			}
			int UomInnerId = 0;
			string UomInnerCode = "";
			double UomInnerEquivalent = 0.0;
			if (((UltraGridBase)CmbPackUomInner).ActiveRow != null && Conversion.ToInt(CmbPackUomInner.Value) != 0)
			{
				UomInnerId = Conversion.ToInt(CmbPackUomInner.Value);
				UomInnerCode = Conversion.ToString((object)((Control)(object)CmbPackUomInner).Text);
				UomInnerEquivalent = Conversion.ToDouble(((UltraDropDownBase)CmbPackUomInner).SelectedRow.Cells[2].Value);
			}
			table.Rows.Add(0, InvoiceDetailId, cmbInvoiceNo.Value, ((Control)(object)cmbInvoiceNo).Text.Trim(), CmbSupplierCustomer.Value, ((Control)(object)CmbSupplierCustomer).Text.Trim(), CmbOrderNo.Value, ((Control)(object)CmbOrderNo).Text.Trim(), CmbWareHouse.Value, ((Control)(object)CmbWareHouse).Text.Trim(), CmbJobLot.Value, ((Control)(object)CmbJobLot).Text.Trim(), CmbCropYear.Value, ((Control)(object)CmbCropYear).Text.Trim(), ((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemId"].Value, ((Control)(object)CmbItemName).Text.Trim(), CmbPackingType.Value, ((Control)(object)CmbPackingType).Text.Trim(), Conversion.ToDouble((object)txtQtyMaster.Text.Trim()), CmbPackUomMaster.Value, ((Control)(object)CmbPackUomMaster).Text.Trim(), Conversion.ToDouble(((UltraDropDownBase)CmbPackUomMaster).SelectedRow.Cells[2].Value), Conversion.ToDouble((object)txtEbUnitMaster.Text), Conversion.ToDouble((object)txtEbTotalMaster.Text), Conversion.ToDouble((object)txtQtyInner.Text.Trim()), UomInnerId, UomInnerCode, UomInnerEquivalent, Conversion.ToDouble((object)txtEbUnitInner.Text), Conversion.ToDouble((object)txtEbTotalInner.Text), Conversion.ToDouble((object)txtAccessWtSet.Text), Conversion.ToDouble((object)txtNetWeight.Text.Trim()), Conversion.ToDouble((object)txtQtyMaster.Text.Trim()), Conversion.ToDouble((object)txtNetWeight.Text.Trim()), Conversion.ToDouble((object)txtGrossWeight.Text), 0, Conversion.ToInt(CmbInspectionNoPackListDetail.Value), Conversion.ToString((object)((Control)(object)CmbInspectionNoPackListDetail).Text), Conversion.ToInt(CmbInspectionSubLot.Value), Conversion.ToString((object)((Control)(object)CmbInspectionSubLot).Text), ContainerId, ContainerNo, txtContainerRemarks.Text.Trim(), txtInspectionRemarks.Text.Trim(), txtremarksdetail.Text.Trim());
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			ResetDetail();
			CalculateTotalInformation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateTotalInformation()
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				decimal ItemWeight = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["LoadWeight"], (AggregateFunction)2));
				Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["MasterQTY"], (AggregateFunction)2));
				decimal EbTotalMaster = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["EbTotalMaster"], (AggregateFunction)2));
				decimal NetWeightOther = ((grdotheritems.GetDataRows().Count() > 0) ? Conversion.ToDecimal(grdotheritems.GetTotal(grdotheritems.RootTable.Columns["NetWeight"], (AggregateFunction)2)) : 0m);
				decimal EbTotalInner = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["EbTotalInner"], (AggregateFunction)2));
				decimal AccessWtSet = default(decimal);
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					AccessWtSet += Conversion.ToDecimal(r.Cells["AccessWtSet"].Value) * Conversion.ToDecimal(r.Cells["MasterQTY"].Value);
				}
				decimal AccessWtSetInKg = AccessWtSet / 1000m;
				decimal GrossWeight = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["GrossWeight"], (AggregateFunction)2));
				if (Conversion.ToDouble((object)txtOtherWeight.Text) > 0.0)
				{
					GrossWeight += Conversion.ToDecimal((object)txtOtherWeight.Text);
				}
				GrossWeight += NetWeightOther;
				txtItemWeight.Text = Math.Round(ItemWeight, 2).ToString("#,##0.##");
				txtPackingWeight.Text = Math.Round(EbTotalMaster + EbTotalInner, 2).ToString("#,##0.##");
				txtTotalAccessWtSet.Text = Math.Round(AccessWtSetInKg, 2).ToString("#,##0.##");
				txtOtherItemTotalWt.Text = Math.Round(NetWeightOther, 2).ToString("#,##0.##");
				txtTotalGrossWeight.Text = Math.Round(GrossWeight, 2).ToString("#,##0.##");
			}
			else
			{
				txtItemWeight.Text = "0";
				txtPackingWeight.Text = "0";
				txtTotalAccessWtSet.Text = "0";
				txtOtherItemTotalWt.Text = "0";
				txtTotalGrossWeight.Text = "0";
			}
			OtherWeightProportion();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdSettings()
	{
		try
		{
			grd.ColumnAutoResize = false;
			List<string> hiddenColumns = new List<string>
			{
				"Id", "InvoiceId", "InvoiceDetailId", "ItemId", "WareHouse", "SupplierCustomerId", "OrderId", "MasterUOMId", "InnerUOMId", "MasterUOMEquivalent",
				"InnerUOMEquivalent", "PackingType", "JobLot", "NetWeight", "CropYearId", "ContainerId", "ThirdPartyAnalysisId", "ThirdPartyAnalysisSubId"
			};
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"WareHouseId",
					(EditType)4
				},
				{
					"JobLotId",
					(EditType)4
				},
				{
					"PackingTypeId",
					(EditType)4
				},
				{
					"MasterQTY",
					(EditType)1
				},
				{
					"EbUnitMaster",
					(EditType)1
				},
				{
					"EbTotalMaster",
					(EditType)1
				},
				{
					"InnerQTY",
					(EditType)1
				},
				{
					"EbUnitInner",
					(EditType)1
				},
				{
					"EbTotalInner",
					(EditType)1
				},
				{
					"AccessWtSet",
					(EditType)1
				},
				{
					"ContainerRemarks",
					(EditType)1
				},
				{
					"InspectionRemarks",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 2, 3, hiddenColumns, editableColumns);
			grd.RootTable.Columns["SupplierCustomer"].Caption = "Customer";
			grd.RootTable.Columns["AccessWtSet"].Caption = "Access Wt Set(g)";
			grd.RootTable.Columns["InvoiceNo"].Width = Constants.ExportConstants.InvoiceNoConstant;
			grd.RootTable.Columns["SupplierCustomer"].Width = Constants.ExportConstants.StringConstant;
			grd.RootTable.Columns["OrderNo"].Width = Constants.ExportConstants.InvoiceNoConstant;
			grd.RootTable.Columns["CropYear"].Width = Constants.ExportConstants.CropYearConstant;
			grd.RootTable.Columns["Item"].Width = Constants.ExportConstants.ItemNameConstants;
			grd.RootTable.Columns["PackingType"].Width = Constants.ExportConstants.CarrierTypeConstant - 10;
			grd.RootTable.Columns["MasterQTY"].Width = Constants.ExportConstants.QtyConstant;
			grd.RootTable.Columns["MasterUOM"].Width = Constants.ExportConstants.UomConstant;
			grd.RootTable.Columns["EbUnitMaster"].Width = Constants.ExportConstants.QtyConstant;
			grd.RootTable.Columns["EbTotalMaster"].Width = Constants.ExportConstants.QtyConstant;
			grd.RootTable.Columns["InnerQTY"].Width = Constants.ExportConstants.QtyConstant;
			grd.RootTable.Columns["InnerUOM"].Width = Constants.ExportConstants.UomConstant;
			grd.RootTable.Columns["EbUnitInner"].Width = Constants.ExportConstants.WeightConstant;
			grd.RootTable.Columns["EbTotalInner"].Width = Constants.ExportConstants.WeightConstant;
			grd.RootTable.Columns["AccessWtSet"].Width = Constants.ExportConstants.WeightConstant;
			grd.RootTable.Columns["NetWeight"].Width = Constants.ExportConstants.WeightConstant;
			grd.RootTable.Columns["LoadQty"].Width = Constants.ExportConstants.QtyConstant;
			grd.RootTable.Columns["LoadWeight"].Width = Constants.ExportConstants.WeightConstant;
			grd.RootTable.Columns["GrossWeight"].Width = Constants.ExportConstants.WeightConstant;
			grd.RootTable.Columns["OtherWeight"].Width = Constants.ExportConstants.WeightConstant;
			grd.RootTable.Columns["ContainerNo"].Width = Constants.ExportConstants.ContainerNo;
			grd.RootTable.Columns["ContainerRemarks"].Width = Constants.ExportConstants.RemarksConstants;
			grd.RootTable.Columns["InspectionRemarks"].Width = Constants.ExportConstants.RemarksConstants;
			grd.RootTable.Columns["Remarks"].Width = Constants.ExportConstants.RemarksConstants;
			grd.RootTable.Columns["ThirdPartyAnalysisNo"].Width = Constants.ExportConstants.JobLotConstant;
			grd.RootTable.Columns["ThirdPartyAnalysisSubNo"].Width = Constants.ExportConstants.JobLotConstant;
			grd.RootTable.Columns["WareHouseId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["WareHouseId"].HasValueList = true;
			grd.RootTable.Columns["WareHouseId"].LimitToList = true;
			grd.RootTable.Columns["WareHouseId"].EditType = (EditType)4;
			grd.RootTable.Columns["WareHouseId"].Caption = "WareHouse";
			grd.RootTable.Columns["WareHouseId"].ValueList.PopulateValueList((IEnumerable)dtWh.DefaultView, "Id", "WareHouseName");
			grd.RootTable.Columns["WareHouseId"].Width = 100;
			grd.RootTable.Columns["JobLotId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["JobLotId"].HasValueList = true;
			grd.RootTable.Columns["JobLotId"].LimitToList = true;
			grd.RootTable.Columns["JobLotId"].EditType = (EditType)4;
			grd.RootTable.Columns["JobLotId"].Caption = "JobLot";
			grd.RootTable.Columns["JobLotId"].ValueList.PopulateValueList((IEnumerable)dtjob.DefaultView, "Id", "JobLotDescription");
			grd.RootTable.Columns["JobLotId"].Width = 70;
			grd.RootTable.Columns["PackingTypeId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["PackingTypeId"].HasValueList = true;
			grd.RootTable.Columns["PackingTypeId"].LimitToList = true;
			grd.RootTable.Columns["PackingTypeId"].EditType = (EditType)4;
			grd.RootTable.Columns["PackingTypeId"].Caption = "PackingType";
			grd.RootTable.Columns["PackingTypeId"].ValueList.PopulateValueList((IEnumerable)dtPackingType.DefaultView, "Id", "PackTypeDesc");
			grd.RootTable.Columns["PackingTypeId"].Width = 120;
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 20;
			grd.RootTable.Columns["Delete"].Position = 0;
			grd.FrozenColumns = 1;
			grd.RootTable.Columns.Add("Add");
			grd.RootTable.Columns["Add"].Key = "Add";
			grd.RootTable.Columns["Add"].Caption = "+";
			grd.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Add"].ButtonText = "+";
			grd.RootTable.Columns["Add"].Width = 20;
			grd.RootTable.Columns["Add"].Position = 1;
			grd.RootTable.Columns.Add("Edit");
			grd.RootTable.Columns["Edit"].Key = "Edit";
			grd.RootTable.Columns["Edit"].Caption = "Edit";
			grd.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Edit"].ButtonText = "Edit";
			grd.RootTable.Columns["Edit"].Width = 50;
			grd.RootTable.Columns["Edit"].Position = 2;
			grd.FrozenColumns = 3;
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
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				updateDetailIndex = item.RowIndex;
				cmbInvoiceNo.Value = Conversion.ToInt(item.Cells["InvoiceId"].Value);
				cmbInvoiceNo_Leave(null, null);
				CmbSupplierCustomer.Value = Conversion.ToInt(item.Cells["SupplierCustomerId"].Value);
				CmbOrderNo.Value = Conversion.ToInt(item.Cells["OrderId"].Value);
				CmbItemName.Value = Conversion.ToInt(item.Cells["InvoiceDetailId"].Value);
				CmbItemName_TextChanged(null, null);
				CmbPackingType.Value = item.Cells["PackingTypeId"].Value.ToString();
				CmbCropYear.Value = item.Cells["CropYearId"].Value.ToString();
				txtQtyMaster.Text = item.Cells["MasterQTY"].Value.ToString();
				CmbPackUomMaster.Value = item.Cells["MasterUOMId"].Value.ToString();
				txtEbUnitMaster.Text = item.Cells["EbUnitMaster"].Value.ToString();
				txtEbTotalMaster.Text = item.Cells["EbTotalMaster"].Value.ToString();
				txtQtyInner.Text = item.Cells["InnerQTY"].Value.ToString();
				CmbPackUomInner.Value = item.Cells["InnerUOMId"].Value.ToString();
				txtEbUnitInner.Text = item.Cells["EbUnitInner"].Value.ToString();
				txtEbTotalInner.Text = item.Cells["EbTotalInner"].Value.ToString();
				txtAccessWtSet.Text = item.Cells["AccessWtSet"].Value.ToString();
				txtNetWeight.Text = item.Cells["NetWeight"].Value.ToString();
				txtQtyMaster.Text = item.Cells["LoadQty"].Value.ToString();
				txtNetWeight.Text = item.Cells["LoadWeight"].Value.ToString();
				txtGrossWeight.Text = item.Cells["GrossWeight"].Value.ToString();
				CmbWareHouse.Value = item.Cells["WareHouseId"].Value.ToString();
				((Control)(object)CmbWareHouse).Text = item.Cells["WareHouse"].Text.ToString();
				CmbJobLot.Value = item.Cells["JobLotId"].Value.ToString();
				((Control)(object)CmbJobLot).Text = item.Cells["JobLot"].Text.ToString();
				if (Conversion.ToInt(item.Cells["ThirdPartyAnalysisId"].Value) > 0)
				{
					CmbInspectionNoPackListDetail.Value = item.Cells["ThirdPartyAnalysisId"].Value.ToString();
				}
				CmbInspectionNoPackListDetail_Leave(null, null);
				if (Conversion.ToInt(item.Cells["ThirdPartyAnalysisSubId"].Value) > 0)
				{
					CmbInspectionSubLot.Value = item.Cells["ThirdPartyAnalysisSubId"].Value.ToString();
				}
				if (Conversion.ToInt(item.Cells["ContainerId"].Value) > 0)
				{
					cmbContainerNoDetail.Value = item.Cells["ContainerId"].Value.ToString();
				}
				txtInspectionRemarks.Text = item.Cells["InspectionRemarks"].Value.ToString();
				txtremarksdetail.Text = item.Cells["Remarks"].Value.ToString();
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				CmbWareHouse.Focus();
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
			if (!FormValidationDetail())
			{
				return;
			}
			if (grd.RowCount > 0 && RestrictOneInvoiceOnDo && !GridEX_Helper.IsAnyCellValueHaveSameValue(grd, "InvoiceId", Conversion.ToInt(cmbInvoiceNo.Value), updateDetailIndex))
			{
				throw new Exception("Data Against another Invoice Exists in Grid,So you can't add different Invoice");
			}
			if (IsMandatoryInspectionLotMappedOnDeliveryOrder)
			{
				if (IsInspectionLotMappedOnDeliveryOrderByInvoice)
				{
					if (((UltraGridBase)CmbInspectionNoPackListDetail).ActiveRow == null || Conversion.ToInt(CmbInspectionNoPackListDetail.Value) == 0)
					{
						CmbInspectionNoPackListDetail.Focus();
						MessageBox.Show("Inspection No field is required");
						return;
					}
					if (((UltraGridBase)CmbInspectionSubLot).ActiveRow == null || Conversion.ToInt(CmbInspectionSubLot.Value) == 0)
					{
						CmbInspectionSubLot.Focus();
						MessageBox.Show("Inspection SubLot No field is required");
						return;
					}
				}
				else if (((UltraGridBase)CmbInspectionNoPackListDetail).ActiveRow == null || Conversion.ToInt(CmbInspectionNoPackListDetail.Value) == 0)
				{
					CmbInspectionNoPackListDetail.Focus();
					MessageBox.Show("Inspection No field is required");
					return;
				}
			}
			int ContainerId = 0;
			string ContainerNo = "";
			if (((UltraGridBase)cmbContainerNoDetail).ActiveRow != null && Conversion.ToInt(cmbContainerNoDetail.Value) != 0)
			{
				ContainerId = Conversion.ToInt(cmbContainerNoDetail.Value);
				ContainerNo = Conversion.ToString((object)((Control)(object)cmbContainerNoDetail).Text);
			}
			int UomInnerId = 0;
			string UomInnerCode = "";
			double UomInnerEquivalent = 0.0;
			if (((UltraGridBase)CmbPackUomInner).ActiveRow != null && Conversion.ToInt(CmbPackUomInner.Value) != 0)
			{
				UomInnerId = Conversion.ToInt(CmbPackUomInner.Value);
				UomInnerCode = Conversion.ToString((object)((Control)(object)CmbPackUomInner).Text);
				UomInnerEquivalent = Conversion.ToDouble(((UltraDropDownBase)CmbPackUomInner).SelectedRow.Cells[2].Value);
			}
			table.Rows[updateDetailIndex]["InvoiceId"] = cmbInvoiceNo.Value;
			table.Rows[updateDetailIndex]["InvoiceNo"] = ((Control)(object)cmbInvoiceNo).Text;
			table.Rows[updateDetailIndex]["SupplierCustomerId"] = CmbSupplierCustomer.Value;
			table.Rows[updateDetailIndex]["SupplierCustomer"] = ((Control)(object)CmbSupplierCustomer).Text;
			table.Rows[updateDetailIndex]["InvoiceDetailId"] = ((UltraDropDownBase)CmbItemName).SelectedRow.Cells[2].Value;
			table.Rows[updateDetailIndex]["OrderId"] = CmbOrderNo.Value;
			table.Rows[updateDetailIndex]["OrderNo"] = ((Control)(object)CmbOrderNo).Text;
			table.Rows[updateDetailIndex]["WareHouseId"] = CmbWareHouse.Value;
			table.Rows[updateDetailIndex]["WareHouse"] = ((Control)(object)CmbWareHouse).Text;
			table.Rows[updateDetailIndex]["JobLotId"] = CmbJobLot.Value;
			table.Rows[updateDetailIndex]["JobLot"] = ((Control)(object)CmbJobLot).Text.Trim();
			table.Rows[updateDetailIndex]["CropYearId"] = CmbCropYear.Value;
			table.Rows[updateDetailIndex]["CropYear"] = ((Control)(object)CmbCropYear).Text.Trim();
			table.Rows[updateDetailIndex]["ItemId"] = ((UltraDropDownBase)CmbItemName).SelectedRow.Cells[0].Value;
			table.Rows[updateDetailIndex]["Item"] = ((Control)(object)CmbItemName).Text;
			table.Rows[updateDetailIndex]["PackingTypeId"] = CmbPackingType.Value;
			table.Rows[updateDetailIndex]["PackingType"] = ((Control)(object)CmbPackingType).Text.Trim();
			table.Rows[updateDetailIndex]["MasterQTY"] = Conversion.ToDouble((object)txtQtyMaster.Text);
			table.Rows[updateDetailIndex]["MasterUOMId"] = CmbPackUomMaster.Value;
			table.Rows[updateDetailIndex]["MasterUOM"] = ((Control)(object)CmbPackUomMaster).Text;
			table.Rows[updateDetailIndex]["MasterUOMEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)CmbPackUomMaster).SelectedRow.Cells[2].Value);
			table.Rows[updateDetailIndex]["EbUnitMaster"] = txtEbUnitMaster.Text;
			table.Rows[updateDetailIndex]["EbTotalMaster"] = txtEbTotalMaster.Text;
			table.Rows[updateDetailIndex]["InnerQTY"] = Conversion.ToDouble((object)txtQtyInner.Text);
			table.Rows[updateDetailIndex]["InnerUOMId"] = UomInnerId;
			table.Rows[updateDetailIndex]["InnerUOM"] = UomInnerCode;
			table.Rows[updateDetailIndex]["InnerUOMEquivalent"] = UomInnerEquivalent;
			table.Rows[updateDetailIndex]["EbUnitInner"] = Conversion.ToDouble((object)txtEbUnitInner.Text);
			table.Rows[updateDetailIndex]["EbTotalInner"] = Conversion.ToDouble((object)txtEbTotalInner.Text);
			table.Rows[updateDetailIndex]["AccessWtSet"] = Conversion.ToDouble((object)txtAccessWtSet.Text);
			table.Rows[updateDetailIndex]["NetWeight"] = Conversion.ToDouble((object)txtNetWeight.Text);
			table.Rows[updateDetailIndex]["LoadQTY"] = Conversion.ToDouble((object)txtQtyMaster.Text);
			table.Rows[updateDetailIndex]["LoadWeight"] = Conversion.ToDouble((object)txtNetWeight.Text);
			table.Rows[updateDetailIndex]["GrossWeight"] = Conversion.ToDouble((object)txtGrossWeight.Text);
			table.Rows[updateDetailIndex]["ThirdPartyAnalysisId"] = Conversion.ToInt(CmbInspectionNoPackListDetail.Value);
			table.Rows[updateDetailIndex]["ThirdPartyAnalysisNo"] = Conversion.ToString((object)((Control)(object)CmbInspectionNoPackListDetail).Text);
			table.Rows[updateDetailIndex]["ThirdPartyAnalysisSubId"] = Conversion.ToInt(CmbInspectionSubLot.Value);
			table.Rows[updateDetailIndex]["ThirdPartyAnalysisSubNo"] = Conversion.ToString((object)((Control)(object)CmbInspectionSubLot).Text);
			table.Rows[updateDetailIndex]["ContainerId"] = ContainerId;
			table.Rows[updateDetailIndex]["ContainerNo"] = ContainerNo;
			table.Rows[updateDetailIndex]["ContainerRemarks"] = txtContainerRemarks.Text;
			table.Rows[updateDetailIndex]["InspectionRemarks"] = txtInspectionRemarks.Text;
			table.Rows[updateDetailIndex]["Remarks"] = txtremarksdetail.Text;
			grd.DataSource = table;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			ResetDetail();
			CmbInvoiceNoFill();
			CalculateTotalInformation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		ResetDetail();
		CmbInvoiceNoFill();
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		try
		{
			GridEXRow r = grd.CurrentRow;
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
					InvDeliveryOrderdetail vd = new InvDeliveryOrderdetail();
					vd.RefDocumentTypeId = 0;
					vd.RefDocIdNo = 0;
					vd.RefDocSubIdNo = 0;
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd.ExImInvoiceId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
					vd.SupplierCustomerId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
					vd.SaleOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
					vd.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
					vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					vd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
					vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					vd.InvPackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					vd.PackUomId = Conversion.ToInt(r.Cells["MasterUOMId"].Value);
					vd.PackingWeight = Conversion.ToDouble(r.Cells["EbUnitMaster"].Value);
					vd.OuterEbTotal = Conversion.ToDouble(r.Cells["EbTotalMaster"].Value);
					vd.InnerQty = Conversion.ToDouble(r.Cells["InnerQTY"].Value);
					vd.InnerUomId = Conversion.ToInt(r.Cells["InnerUOMId"].Value);
					vd.InnerEbUnit = Conversion.ToDouble(r.Cells["EbUnitInner"].Value);
					vd.InnerEbTotal = Conversion.ToDouble(r.Cells["EbTotalInner"].Value);
					vd.AccessWtSet = Conversion.ToDouble(r.Cells["AccessWtSet"].Value);
					vd.DoWeight = Conversion.ToDouble(r.Cells["NetWeight"].Value);
					vd.DoQty = Conversion.ToDouble(r.Cells["LoadQty"].Value);
					vd.DoWeight = Conversion.ToDouble(r.Cells["LoadWeight"].Value);
					vd.LoadingQty = vd.DoQty;
					vd.LoadingWeight = vd.DoWeight;
					vd.TotalPackingWeight = Conversion.ToDouble((object)(vd.InnerEbTotal + vd.OuterEbTotal));
					vd.GrossWeight = Conversion.ToDouble(r.Cells["GrossWeight"].Value);
					vd.ThirdPartyAnalysisId = Conversion.ToInt(r.Cells["ThirdPartyAnalysisId"].Value);
					vd.ThirdPartyAnalysisNo = Conversion.ToString(r.Cells["ThirdPartyAnalysisNo"].Value);
					vd.ThirdPartyAnalysisSubId = Conversion.ToInt(r.Cells["ThirdPartyAnalysisSubId"].Value);
					vd.ThirdPartyAnalysisSubNo = Conversion.ToString(r.Cells["ThirdPartyAnalysisSubNo"].Value);
					vd.LoadingRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
					vd.InspectionRemarks = Conversion.ToString(r.Cells["InspectionRemarks"].Value);
					vd.ContainerRemarks = Conversion.ToString(r.Cells["ContainerRemarks"].Value);
					vd.ActionTypeId = 3;
					lstRemoveRecord.Add(vd);
					table.Rows[r.RowIndex].Delete();
					r.Delete();
					grd.UpdateData();
				}
				else
				{
					r.Delete();
				}
				CalculateTotalInformation();
			}
			if (e.Column.Key == "Edit")
			{
				grd_DoubleClick(null, null);
			}
			if (e.Column.Key == "Add")
			{
				DataRow dr = table.Rows[r.RowIndex];
				table.Rows.Add(dr.ItemArray);
				if (btnupdate.Visible && btnupdate.Enabled)
				{
					table.Rows[table.Rows.Count - 1]["Id"] = 0;
				}
				grd.DataSource = table;
			}
			CalculateTotalInformation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "WareHouseId" || e.Column.Key == "WareHouse")
			{
				int warehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
				DataRow[] rows = dtWh.Select($"Id = {warehouseId}");
				if (rows.Length != 0)
				{
					string warehouseName = Conversion.ToString(rows[0]["WarehouseName"]);
					r.Cells["WareHouse"].Value = warehouseName;
				}
			}
			if (e.Column.Key == "JobLotId" || e.Column.Key == "JobLot")
			{
				int JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
				DataRow[] rows2 = dtjob.Select($"Id = {JobLotId}");
				if (rows2.Length != 0)
				{
					string JobLot = Conversion.ToString(rows2[0]["JobLotDescription"]);
					r.Cells["JobLot"].Value = JobLot;
				}
			}
			if (e.Column.Key == "PackingTypeId" || e.Column.Key == "PackingType")
			{
				int PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
				DataRow[] rows3 = dtPackingType.Select($"Id = {PackingTypeId}");
				if (rows3.Length != 0)
				{
					string PackingType = Conversion.ToString(rows3[0]["PackTypeDesc"]);
					r.Cells["PackingType"].Value = PackingType;
				}
			}
			if (e.Column.Key == "MasterQTY" || e.Column.Key == "EbUnitMaster")
			{
				double num = Conversion.ToDouble(r.Cells["EbUnitMaster"].Value);
				double AccessWtSet5 = Conversion.ToDouble(r.Cells["AccessWtSet"].Value);
				double Qty3 = Conversion.ToDouble(r.Cells["MasterQTY"].Value);
				r.Cells["LoadQty"].Value = Qty3.ToString("#,##0.##");
				double EBTotalMaster4 = num * Qty3;
				r.Cells["EbTotalMaster"].Value = EBTotalMaster4.ToString("#,##0.##");
				double EBTotalInner3 = Conversion.ToDouble(r.Cells["EbTotalInner"].Value);
				double MasterUOM4 = Conversion.ToDouble(r.Cells["MasterUOMEquivalent"].Value);
				double GrossWeight5 = 0.0;
				double NetWeight4 = 0.0;
				if (MasterUOM4 != 0.0 && Qty3 != 0.0)
				{
					NetWeight4 = MasterUOM4 * Qty3;
					r.Cells["NetWeight"].Value = NetWeight4.ToString("#,##0.####");
					r.Cells["LoadWeight"].Value = NetWeight4.ToString("#,##0.####");
					GrossWeight5 = NetWeight4 + EBTotalMaster4 + EBTotalInner3 + AccessWtSet5 * Qty3;
					r.Cells["GrossWeight"].Value = GrossWeight5.ToString("#,##0.##");
				}
				else
				{
					r.Cells["NetWeight"].Value = 0.ToString("#,##0.####");
					r.Cells["LoadWeight"].Value = 0.ToString("#,##0.####");
					r.Cells["GrossWeight"].Value = 0.ToString("#,##0.####");
				}
			}
			if (e.Column.Key == "EbTotalMaster")
			{
				double EBTotalMaster3 = Conversion.ToDouble(r.Cells["EbTotalMaster"].Value);
				double AccessWtSet4 = Conversion.ToDouble(r.Cells["AccessWtSet"].Value);
				double Qty2 = Conversion.ToDouble(r.Cells["MasterQTY"].Value);
				double EbUnitMaster = ((Qty2 > 0.0) ? (EBTotalMaster3 / Qty2) : 0.0);
				r.Cells["EbUnitMaster"].Value = EbUnitMaster.ToString("#,##0.##");
				double EBTotalInner2 = Conversion.ToDouble(r.Cells["EbTotalInner"].Value);
				double MasterUOM3 = Conversion.ToDouble(r.Cells["MasterUOMEquivalent"].Value);
				double GrossWeight4 = 0.0;
				double NetWeight3 = 0.0;
				if (MasterUOM3 != 0.0 && Qty2 != 0.0)
				{
					NetWeight3 = MasterUOM3 * Qty2;
					r.Cells["NetWeight"].Value = NetWeight3.ToString("#,##0.####");
					r.Cells["LoadWeight"].Value = NetWeight3.ToString("#,##0.####");
					GrossWeight4 = NetWeight3 + EBTotalMaster3 + EBTotalInner2 + AccessWtSet4 * Qty2;
					r.Cells["GrossWeight"].Value = GrossWeight4.ToString("#,##0.####");
				}
				else
				{
					r.Cells["NetWeight"].Value = 0.ToString("#,##0.####");
					r.Cells["LoadWeight"].Value = 0.ToString("#,##0.####");
					r.Cells["GrossWeight"].Value = 0.ToString("#,##0.####");
				}
			}
			if (e.Column.Key == "InnerQTY" || e.Column.Key == "EbUnitInner")
			{
				double AccessWtSet3 = Conversion.ToDouble(r.Cells["AccessWtSet"].Value);
				double num2 = Conversion.ToDouble(r.Cells["EbUnitInner"].Value);
				double InnerQty2 = Conversion.ToDouble(r.Cells["InnerQTY"].Value);
				double EbTotalInner2 = num2 * InnerQty2;
				r.Cells["EbTotalInner"].Value = EbTotalInner2.ToString("#,##0.##");
				double EBTotalMaster2 = Conversion.ToDouble(r.Cells["EBTotalMaster"].Value);
				double MasterQTY2 = Conversion.ToDouble(r.Cells["MasterQTY"].Value);
				double MasterUOM2 = Conversion.ToDouble(r.Cells["MasterUOMEquivalent"].Value);
				double GrossWeight3 = 0.0;
				double NetWeight2 = 0.0;
				if (MasterUOM2 != 0.0 && MasterQTY2 != 0.0)
				{
					NetWeight2 = MasterUOM2 * MasterQTY2;
					r.Cells["NetWeight"].Value = NetWeight2.ToString("#,##0.####");
					r.Cells["LoadWeight"].Value = NetWeight2.ToString("#,##0.####");
					GrossWeight3 = NetWeight2 + EbTotalInner2 + EBTotalMaster2 + AccessWtSet3 * MasterQTY2;
					r.Cells["GrossWeight"].Value = GrossWeight3.ToString("#,##0.##");
				}
				else
				{
					r.Cells["NetWeight"].Value = 0.ToString("#,##0.####");
					r.Cells["LoadWeight"].Value = 0.ToString("#,##0.####");
					r.Cells["GrossWeight"].Value = 0.ToString("#,##0.####");
				}
			}
			if (e.Column.Key == "EbTotalInner")
			{
				double AccessWtSet2 = Conversion.ToDouble(r.Cells["AccessWtSet"].Value);
				double EbTotalInner = Conversion.ToDouble(r.Cells["EbTotalInner"].Value);
				double InnerQty = Conversion.ToDouble(r.Cells["InnerQty"].Value);
				double EbUnitInner = ((InnerQty > 0.0) ? (EbTotalInner / InnerQty) : 0.0);
				r.Cells["EbUnitInner"].Value = EbUnitInner.ToString("#,##0.##");
				double EbTotalMaster = Conversion.ToDouble(r.Cells["EbTotalMaster"].Value);
				double MasterQTY = Conversion.ToDouble(r.Cells["MasterQTY"].Value);
				double MasterUOM = Conversion.ToDouble(r.Cells["MasterUOMEquivalent"].Value);
				double GrossWeight2 = 0.0;
				double NetWeight = 0.0;
				if (MasterUOM != 0.0 && MasterQTY != 0.0)
				{
					NetWeight = MasterUOM * MasterQTY;
					r.Cells["NetWeight"].Value = NetWeight.ToString("#,##0.####");
					r.Cells["LoadWeight"].Value = NetWeight.ToString("#,##0.####");
					GrossWeight2 = NetWeight + EbTotalMaster + EbTotalInner + AccessWtSet2 * MasterQTY;
					r.Cells["GrossWeight"].Value = GrossWeight2.ToString("#,##0.##");
				}
				else
				{
					r.Cells["NetWeight"].Value = 0.ToString("#,##0.####");
					r.Cells["LoadWeight"].Value = 0.ToString("#,##0.####");
					r.Cells["GrossWeight"].Value = 0.ToString("#,##0.####");
				}
			}
			if (e.Column.Key == "NetWeight" || e.Column.Key == "AccessWtSet")
			{
				double AccessWtSet = Conversion.ToDouble(r.Cells["AccessWtSet"].Value);
				double Qty = Conversion.ToDouble(r.Cells["MasterQTY"].Value);
				double EBTotalMaster = Conversion.ToDouble(r.Cells["EbTotalMaster"].Value);
				double EBTotalInner = Conversion.ToDouble(r.Cells["EbTotalInner"].Value);
				double GrossWeight = 0.0;
				GrossWeight = Conversion.ToDouble(r.Cells["NetWeight"].Value) + EBTotalMaster + EBTotalInner + AccessWtSet * Qty;
				r.Cells["GrossWeight"].Value = GrossWeight.ToString("#,##0.####");
			}
			grd.UpdateData();
			CalculateTotalInformation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OtherWeightProportion()
	{
		try
		{
			double NetWeight = ((grd.RowCount > 0) ? Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["NetWeight"], (AggregateFunction)2).ToString()) : 0.0);
			double OtherWeightHeader = Conversion.ToDouble((object)txtOtherWeight.Text);
			if (OtherWeightHeader > 0.0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Weight = Conversion.ToDouble((object)obj.Cells["NetWeight"].Value.ToString());
					obj.Cells["OtherWeight"].Value = OtherWeightHeader / NetWeight * Weight;
					obj.EndEdit();
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["OtherWeight"].Value = 0;
					obj2.EndEdit();
				}
			}
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
			if (OtherItemFormValidation())
			{
				dtOtherItem.Rows.Add(0, 0, 0, 0, CmbOtherItem.Value, ((Control)(object)CmbOtherItem).Text, Conversion.ToDouble((object)txtoQty.Text.Trim()), Conversion.ToDouble((object)txtWtPerQtyOtherItem.Text.Trim()), Conversion.ToDouble((object)txtNetWeightOtherItem.Text.Trim()), txtoRemarks.Text.Trim());
				grdotheritems.DataSource = dtOtherItem;
				grdotheritems.RetrieveStructure();
				grdSettingsOtherItems();
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
		try
		{
			GridEXRow item = grdotheritems.CurrentRow;
			if (item != null)
			{
				updateDetailIndexOtherItem = item.RowIndex;
				CmbOtherItem.Value = item.Cells["ItemId"].Value.ToString();
				txtoQty.Text = item.Cells["Qty"].Value.ToString();
				txtWtPerQtyOtherItem.Text = item.Cells["WeightPerQty"].Value.ToString();
				txtNetWeightOtherItem.Text = item.Cells["NetWeight"].Value.ToString();
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

	private void btnoUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (OtherItemFormValidation())
			{
				dtOtherItem.Rows[updateDetailIndexOtherItem]["ItemId"] = CmbOtherItem.Value;
				dtOtherItem.Rows[updateDetailIndexOtherItem]["ItemName"] = ((Control)(object)CmbOtherItem).Text;
				dtOtherItem.Rows[updateDetailIndexOtherItem]["Qty"] = txtoQty.Text;
				dtOtherItem.Rows[updateDetailIndexOtherItem]["WeightPerQty"] = txtWtPerQtyOtherItem.Text;
				dtOtherItem.Rows[updateDetailIndexOtherItem]["NetWeight"] = txtNetWeightOtherItem.Text;
				dtOtherItem.Rows[updateDetailIndexOtherItem]["Remarks"] = txtoRemarks.Text;
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

	private void btnoCancel_Click(object sender, EventArgs e)
	{
		btnoadd.Visible = true;
		btnoUpdate.Visible = false;
		btnoCancel.Visible = false;
		ResetOtherItems();
	}

	private void grdSettingsOtherItems()
	{
		try
		{
			grdotheritems.AllowEdit = (InheritableBoolean)1;
			grdotheritems.RootTable.Columns["Id"].Visible = false;
			grdotheritems.RootTable.Columns["ExImInvoiceId"].Visible = false;
			grdotheritems.RootTable.Columns["InvoiceDetailId"].Visible = false;
			grdotheritems.RootTable.Columns["ItemId"].Visible = false;
			grdotheritems.RootTable.Columns["Id"].EditType = (EditType)0;
			grdotheritems.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdotheritems.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdotheritems.RootTable.Columns["InvoiceNo"].EditType = (EditType)0;
			grdotheritems.RootTable.Columns["InvoiceNo"].Width = 120;
			grdotheritems.RootTable.Columns["ItemName"].Width = 350;
			grdotheritems.RootTable.Columns["Remarks"].Width = 350;
			grdotheritems.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdotheritems.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdotheritems.RootTable.Columns["Qty"].FormatString = "#,##0.####";
			grdotheritems.RootTable.Columns["Qty"].TotalFormatString = "#,##0.####";
			grdotheritems.RootTable.Columns["WeightPerQty"].AggregateFunction = (AggregateFunction)2;
			grdotheritems.RootTable.Columns["WeightPerQty"].TextAlignment = (TextAlignment)3;
			grdotheritems.RootTable.Columns["WeightPerQty"].FormatString = "#,##0.####";
			grdotheritems.RootTable.Columns["WeightPerQty"].TotalFormatString = "#,##0.####";
			grdotheritems.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdotheritems.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdotheritems.RootTable.Columns["NetWeight"].FormatString = "#,##0.####";
			grdotheritems.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.####";
			grdotheritems.RootTable.Columns.Add("Delete");
			grdotheritems.RootTable.Columns["Delete"].Key = "Delete";
			grdotheritems.RootTable.Columns["Delete"].Caption = "X";
			grdotheritems.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdotheritems.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdotheritems.RootTable.Columns["Delete"].ButtonText = "X";
			grdotheritems.RootTable.Columns["Delete"].Width = 20;
			grdotheritems.RootTable.Columns["Delete"].Position = 0;
			grdotheritems.RootTable.Columns.Add("Add");
			grdotheritems.RootTable.Columns["Add"].Key = "Add";
			grdotheritems.RootTable.Columns["Add"].Caption = "+";
			grdotheritems.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdotheritems.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdotheritems.RootTable.Columns["Add"].ButtonText = "+";
			grdotheritems.RootTable.Columns["Add"].Width = 20;
			grdotheritems.RootTable.Columns["Add"].Position = 1;
			grdotheritems.RootTable.Columns.Add("Edit");
			grdotheritems.RootTable.Columns["Edit"].Key = "Edit";
			grdotheritems.RootTable.Columns["Edit"].Caption = "Edit";
			grdotheritems.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdotheritems.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdotheritems.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdotheritems.RootTable.Columns["Edit"].Width = 50;
			grdotheritems.RootTable.Columns["Edit"].Position = 2;
			grdotheritems.FrozenColumns = 3;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdotheritems_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdotheritems.CurrentRow;
				dtOtherItem.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtOtherItem.Rows.Count == 0)
				{
					AddRowInvExpGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInvExpGrid();
			}
			if (e.Column.Key == "Edit")
			{
				grdotheritems_DoubleClick(null, null);
			}
			grdotheritems.UpdateData();
			CalculateTotalInformation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInvExpGrid()
	{
		try
		{
			dtOtherItem.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, "");
			grdotheritems.DataSource = dtOtherItem;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridExpense_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdotheritems.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdotheritems.CurrentColumn == null)
			{
				return;
			}
			if (e.KeyCode == Keys.F1 && grdotheritems.CurrentColumn != null && (grdotheritems.CurrentColumn.Key == "ItemName" || grdotheritems.CurrentColumn.Key == "ItemId"))
			{
				GrdPopUp obj = new GrdPopUp(dtOtherItemCombo, "Id", "ItemName");
				obj.StartPosition = FormStartPosition.CenterScreen;
				obj.ShowDialog();
				obj.BringToFront();
				r.Cells["ItemId"].Value = obj.ReturnId;
				r.Cells["ItemName"].Value = obj.ReturnName;
				grdotheritems.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdotheritems.CurrentColumn.Key == "Delete")
				{
					GridEXRow item = grdotheritems.CurrentRow;
					dtOtherItem.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
					if (dtOtherItem.Rows.Count == 0)
					{
						AddRowInvExpGrid();
					}
					grdotheritems.UpdateData();
				}
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInvExpGrid();
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				GridEXRow item2 = grdotheritems.CurrentRow;
				dtOtherItem.Rows[item2.RowIndex].Delete();
				if (item2 != null && (int)item2.RowType == 0)
				{
					item2.Delete();
				}
				grdotheritems.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grdotheritems_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdotheritems.UpdateData();
			GridEXRow r = grdotheritems.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "QTY" || e.Column.Key == "WeightPerQty")
				{
					double num = Conversion.ToDouble(r.Cells["WeightPerQty"].Value);
					double Qty2 = Conversion.ToDouble(r.Cells["QTY"].Value);
					r.Cells["NetWeight"].Value = (num * Qty2).ToString("#,##0.##");
				}
				if (e.Column.Key == "NetWeight")
				{
					double NetWeight = Conversion.ToDouble(r.Cells["NetWeight"].Value);
					double Qty = Conversion.ToDouble(r.Cells["QTY"].Value);
					double WeightPerQty = ((Qty > 0.0) ? (NetWeight / Qty) : 0.0);
					r.Cells["WeightPerQty"].Value = WeightPerQty.ToString("#,##0.##");
				}
				grdotheritems.UpdateData();
				CalculateTotalInformation();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Expected O, but got Unknown
		//IL_0c4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c54: Expected O, but got Unknown
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
			InvDeliveryOrder po = new InvDeliveryOrder();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				po.Id = RecId;
				po.ModifyUser = UserAccount.ID;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			OtherWeightProportion();
			po.DocumentTypeId = 84;
			po.DocDate = DocDate.Value;
			po.DocNo = Conversion.ToInt((object)txtdocno.Text);
			po.LoadingInstructions = Conversion.ToString((object)txtremarks.Text);
			po.EximInvoiceId = Conversion.ToInt(cmbInvoiceNo.Value);
			po.LoadingPortId = Conversion.ToInt(CmbLoadingPort.Value);
			po.OrganizationId = UserAccount.OrganizationId;
			po.CompanyId = UserAccount.CompanyId;
			po.EntryUser = UserAccount.ID;
			po.ScreenName = Conversion.ToString((object)base.Name);
			if (BranchList.Rows.Count > 1)
			{
				po.BranchesId = Conversion.ToInt(CmbBranch.Value);
			}
			else
			{
				po.BranchesId = UserAccount.BranchesId;
			}
			po.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			po.DeliveryOrderType = "Export";
			po.VehicleNo = Conversion.ToString((object)txtVehicleNo.Text.Trim());
			po.TransporterId = Conversion.ToInt(cmbtransporter.Value);
			po.NetWeight = Conversion.ToDouble((object)txtItemWeight.Text);
			po.PackingWeight = Conversion.ToDouble((object)txtPackingWeight.Text);
			po.OtherWeight = Conversion.ToDouble((object)txtOtherWeight.Text);
			po.GrossWeight = Conversion.ToDouble((object)txtTotalGrossWeight.Text);
			po.OtherWeightRemarks = Conversion.ToString((object)txtOtherRemarks.Text);
			po.InvDeliveryOrderDetailList = new List<InvDeliveryOrderdetail>();
			po.InvDeliveryOrderExpenseslist = new List<InvDeliveryOrderExpense>();
			double TotalQty = 0.0;
			double TotalNetWeight = 0.0;
			double TotalGrossWeight = 0.0;
			if (lstRemoveRecord.Count > 0)
			{
				for (int i = 0; i < lstRemoveRecord.Count; i++)
				{
					po.InvDeliveryOrderDetailList.Add(lstRemoveRecord[i]);
				}
			}
			int InvoiceId = 0;
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				InvDeliveryOrderdetail vd = new InvDeliveryOrderdetail();
				if (Conversion.ToInt(r2.Cells["InvoiceId"].Value) == 0)
				{
					throw new Exception($"InvoiceNo field is required in Detail Grid Row no : {r2.RowIndex + 1}");
				}
				if (Conversion.ToInt(r2.Cells["SupplierCustomerId"].Value) == 0)
				{
					throw new Exception($"Customer field is required in Detail Grid Row no : {r2.RowIndex + 1}");
				}
				if (Conversion.ToInt(r2.Cells["OrderId"].Value) == 0)
				{
					throw new Exception($"OrderNo field is required in Detail Grid Row no : {r2.RowIndex + 1}");
				}
				if (Conversion.ToInt(r2.Cells["WareHouseId"].Value) == 0)
				{
					throw new Exception($"Warehouse field is required in Detail Grid Row no : {r2.RowIndex + 1}");
				}
				if (Conversion.ToInt(r2.Cells["ItemId"].Value) == 0)
				{
					throw new Exception($"ItemName field is required in Detail Grid Row no : {r2.RowIndex + 1}");
				}
				if (Conversion.ToInt(r2.Cells["JobLotId"].Value) == 0)
				{
					throw new Exception($"JobLot field is required in Detail Grid Row no : {r2.RowIndex + 1}");
				}
				if (Conversion.ToInt(r2.Cells["CropYearId"].Value) == 0)
				{
					throw new Exception($"CropYear field is required in Detail Grid Row no : {r2.RowIndex + 1}");
				}
				if (Conversion.ToInt(r2.Cells["PackingTypeId"].Value) == 0)
				{
					throw new Exception($"Packing type field is required in Detail Grid Row no : {r2.RowIndex + 1}");
				}
				if (Conversion.ToDouble(r2.Cells["MasterQTY"].Value) == 0.0)
				{
					throw new Exception($"Master Qty field is required in Detail Grid Row no : {r2.RowIndex + 1}");
				}
				if (Conversion.ToInt(r2.Cells["MasterUOMId"].Value) == 0)
				{
					throw new Exception($"Master PackUOM field is required in Detail Grid Row no : {r2.RowIndex + 1}");
				}
				if (Conversion.ToDouble(r2.Cells["NetWeight"].Value) == 0.0)
				{
					throw new Exception($"Net Weight field is required in Detail Grid Row no : {r2.RowIndex + 1}");
				}
				vd.RefDocumentTypeId = 0;
				vd.RefDocIdNo = 0;
				vd.RefDocSubIdNo = 0;
				if (RecId == 0)
				{
					vd.ActionTypeId = 1;
				}
				else
				{
					vd.Id = Conversion.ToInt(r2.Cells["Id"].Value);
					if (vd.Id > 0)
					{
						vd.ActionTypeId = 2;
					}
					else
					{
						vd.ActionTypeId = 1;
					}
				}
				vd.ExImInvoiceId = Conversion.ToInt(r2.Cells["InvoiceId"].Value);
				if (InvoiceId == 0)
				{
					InvoiceId = vd.ExImInvoiceId;
				}
				else if (RestrictOneInvoiceOnDo && InvoiceId != vd.ExImInvoiceId)
				{
					throw new Exception("All Detail Rows Must Have Same Invoice No");
				}
				vd.InvoiceDetailId = Conversion.ToInt(r2.Cells["InvoiceDetailId"].Value);
				vd.SupplierCustomerId = Conversion.ToInt(r2.Cells["SupplierCustomerId"].Value);
				vd.SaleOrderId = Conversion.ToInt(r2.Cells["OrderId"].Value);
				vd.WarehouseId = Conversion.ToInt(r2.Cells["WareHouseId"].Value);
				vd.JobLotId = Conversion.ToInt(r2.Cells["JobLotId"].Value);
				vd.CropYearId = Conversion.ToInt(r2.Cells["CropYearId"].Value);
				vd.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
				vd.InvPackingTypeId = Conversion.ToInt(r2.Cells["PackingTypeId"].Value);
				vd.ItemName = Conversion.ToString(r2.Cells["Item"].Value);
				vd.JobLotDescription = Conversion.ToString(r2.Cells["JobLot"].Value);
				vd.PackUomId = Conversion.ToInt(r2.Cells["MasterUOMId"].Value);
				vd.PackingWeight = Conversion.ToDouble(r2.Cells["EbUnitMaster"].Value);
				vd.OuterEbTotal = Conversion.ToDouble(r2.Cells["EbTotalMaster"].Value);
				vd.DoQty = Conversion.ToDouble(r2.Cells["LoadQty"].Value);
				if (Conversion.ToDouble(r2.Cells["InnerQTY"].Value) <= 0.0)
				{
					vd.InnerQty = vd.DoQty;
				}
				else
				{
					vd.InnerQty = Conversion.ToDouble(r2.Cells["InnerQTY"].Value);
				}
				if (Conversion.ToInt(r2.Cells["InnerUOMId"].Value) <= 0)
				{
					vd.InnerUomId = vd.PackUomId;
				}
				else
				{
					vd.InnerUomId = Conversion.ToInt(r2.Cells["InnerUOMId"].Value);
				}
				vd.InnerEbUnit = Conversion.ToDouble(r2.Cells["EbUnitInner"].Value);
				vd.InnerEbTotal = Conversion.ToDouble(r2.Cells["EbTotalInner"].Value);
				vd.AccessWtSet = Conversion.ToDouble(r2.Cells["AccessWtSet"].Value);
				vd.DoWeight = Conversion.ToDouble(r2.Cells["NetWeight"].Value);
				vd.LoadingQty = vd.DoQty;
				vd.LoadingWeight = vd.DoWeight;
				vd.TotalPackingWeight = Conversion.ToDouble((object)(vd.InnerEbTotal + vd.OuterEbTotal));
				vd.GrossWeight = Conversion.ToDouble(r2.Cells["GrossWeight"].Value);
				vd.ThirdPartyAnalysisId = Conversion.ToInt(r2.Cells["ThirdPartyAnalysisId"].Value);
				if (SaleCostingJobOrderWise && vd.ThirdPartyAnalysisId == 0)
				{
					throw new Exception($"Inspection No field is required in Detail Grid Row no : {r2.RowIndex + 1}");
				}
				vd.ThirdPartyAnalysisNo = Conversion.ToString(r2.Cells["ThirdPartyAnalysisNo"].Value);
				vd.ThirdPartyAnalysisSubId = Conversion.ToInt(r2.Cells["ThirdPartyAnalysisSubId"].Value);
				vd.ThirdPartyAnalysisSubNo = Conversion.ToString(r2.Cells["ThirdPartyAnalysisSubNo"].Value);
				if (IsMandatoryInspectionLotMappedOnDeliveryOrder)
				{
					if (IsInspectionLotMappedOnDeliveryOrderByInvoice)
					{
						if (vd.ThirdPartyAnalysisId == 0)
						{
							throw new Exception($"Inspection No field is required in Detail Grid Row no : {r2.RowIndex + 1}");
						}
						if (vd.ThirdPartyAnalysisSubId == 0)
						{
							throw new Exception($"Inspection Sub Lot No field is required in Detail Grid Row no : {r2.RowIndex + 1}");
						}
					}
					else if (vd.ThirdPartyAnalysisId == 0)
					{
						throw new Exception($"Inspection No field is required in Detail Grid Row no : {r2.RowIndex + 1}");
					}
				}
				vd.OtherWeight = Conversion.ToDouble(r2.Cells["OtherWeight"].Value);
				TotalQty += vd.DoQty;
				TotalNetWeight += vd.LoadingWeight;
				TotalGrossWeight += vd.GrossWeight;
				if (ContainerNoCompulsoryOnDeliveryOrderExport && Conversion.ToInt(r2.Cells["ContainerId"].Value) == 0)
				{
					throw new Exception("Container No Required in Row No" + (r2.RowIndex + 1));
				}
				vd.ContainerId = Conversion.ToInt(r2.Cells["ContainerId"].Value);
				vd.ContainerRemarks = Conversion.ToString(r2.Cells["ContainerRemarks"].Value);
				vd.InspectionRemarks = Conversion.ToString(r2.Cells["InspectionRemarks"].Value);
				vd.LoadingRemarks = Conversion.ToString(r2.Cells["Remarks"].Value);
				po.InvDeliveryOrderDetailList.Add(vd);
			}
			po.DoTotalQty = Conversion.ToDecimal((object)TotalQty);
			rows = grdotheritems.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt(r.Cells["ItemId"].Value) > 0)
				{
					InvDeliveryOrderExpense obj = new InvDeliveryOrderExpense();
					obj.Id = Conversion.ToInt(r.Cells["Id"].Value);
					if (Conversion.ToDouble(r.Cells["Qty"].Value) == 0.0)
					{
						throw new Exception($"Qty Required In Other Item Grid Row No : {r.RowIndex + 1}");
					}
					obj.SaleOrderId = 0;
					obj.SaleOrderCustomerExpId = 0;
					obj.InvoiceOtherItemDetailId = Conversion.ToInt(r.Cells["InvoiceDetailId"].Value);
					obj.ExImInvoiceId = Conversion.ToInt(r.Cells["ExImInvoiceId"].Value);
					obj.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					obj.Qty = Conversion.ToDouble(r.Cells["Qty"].Value);
					obj.WeightPerQty = Conversion.ToDouble(r.Cells["WeightPerQty"].Value);
					obj.NetWeight = Conversion.ToDouble(r.Cells["NetWeight"].Value);
					obj.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
					po.InvDeliveryOrderExpenseslist.Add(obj);
				}
			}
			po.AttachmentsList = new List<DMSAttachments>();
			po.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvDeliveryOrder>(po, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			po.AttachmentsValues = AttachmentsValues;
			po.CustomAttachmentsValues = CustomAttachmentsValues;
			int success = InvDeliveryOrder.Save(po);
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + po.DocNo);
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, po.DocumentTypeId, base.Name);
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + po.DocNo);
			}
			Reset();
			OpenLinkedFormOnInsert(po.DocNo);
			if (ChkPreview.Checked)
			{
				CommonServices.InvDeliveryOrderSlipWithSubReport(success, 84);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
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

	private void btnupdate_Click(object sender, EventArgs e)
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

	public void ReadById(int ID)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			Reset();
			RecId = ID;
			InvDeliveryOrder po = new InvDeliveryOrder();
			po = InvDeliveryOrder.GetByID(ID);
			po.Id = RecId;
			CmbInvoiceNoFill();
			tabControl1.SelectedIndex = 0;
			DocDate.Value = po.DocDate;
			txtdocno.Text = Conversion.ToString((object)po.DocNo);
			CmbBranch.Value = po.BranchesId;
			txtremarks.Text = Conversion.ToString((object)po.LoadingInstructions);
			txtVehicleNo.Text = Conversion.ToString((object)po.VehicleNo);
			CmbLoadingPort.Value = Conversion.ToInt((object)po.LoadingPortId);
			txtItemWeight.Text = po.NetWeight.ToString("#,##0.##");
			txtPackingWeight.Text = po.PackingWeight.ToString("#,##0.##");
			txtOtherWeight.Text = po.OtherWeight.ToString("#,##0.##");
			txtTotalGrossWeight.Text = po.GrossWeight.ToString("#,##0.##");
			txtOtherRemarks.Text = po.OtherWeightRemarks;
			if (po.TransporterId > 0)
			{
				cmbtransporter.Value = po.TransporterId;
			}
			IsApproved = po.IsApproved;
			AttachmentsValues = po.AttachmentsValues;
			CustomAttachmentsValues = po.CustomAttachmentsValues;
			table.Clear();
			for (int j = 0; j < po.InvDeliveryOrderDetailList.Count; j++)
			{
				if (Conversion.ToDouble((object)po.InvDeliveryOrderDetailList[j].TotalPackingWeight) == 0.0)
				{
					po.InvDeliveryOrderDetailList[j].TotalPackingWeight = po.InvDeliveryOrderDetailList[j].PackingWeight;
					if (Conversion.ToDouble((object)po.InvDeliveryOrderDetailList[j].TotalPackingWeight) > 0.0)
					{
						po.InvDeliveryOrderDetailList[j].PackingWeight = Conversion.ToDouble((object)po.InvDeliveryOrderDetailList[j].TotalPackingWeight) / Conversion.ToDouble((object)po.InvDeliveryOrderDetailList[j].LoadingQty);
					}
				}
				table.Rows.Add(po.InvDeliveryOrderDetailList[j].Id, po.InvDeliveryOrderDetailList[j].InvoiceDetailId, po.InvDeliveryOrderDetailList[j].ExImInvoiceId, po.InvDeliveryOrderDetailList[j].InvoiceNo, po.InvDeliveryOrderDetailList[j].SupplierCustomerId, po.InvDeliveryOrderDetailList[j].SupplierCustomer, po.InvDeliveryOrderDetailList[j].SaleOrderId, po.InvDeliveryOrderDetailList[j].OrderNo, po.InvDeliveryOrderDetailList[j].WarehouseId, po.InvDeliveryOrderDetailList[j].WareHouseName, po.InvDeliveryOrderDetailList[j].JobLotId, po.InvDeliveryOrderDetailList[j].JobLotDescription, po.InvDeliveryOrderDetailList[j].CropYearId, po.InvDeliveryOrderDetailList[j].CropYear, po.InvDeliveryOrderDetailList[j].ItemId, po.InvDeliveryOrderDetailList[j].ItemName, po.InvDeliveryOrderDetailList[j].InvPackingTypeId, po.InvDeliveryOrderDetailList[j].PackTypeDesc, po.InvDeliveryOrderDetailList[j].DoQty, po.InvDeliveryOrderDetailList[j].PackUomId, po.InvDeliveryOrderDetailList[j].PackUOM, po.InvDeliveryOrderDetailList[j].PUomEquivalent, po.InvDeliveryOrderDetailList[j].PackingWeight, po.InvDeliveryOrderDetailList[j].OuterEbTotal, po.InvDeliveryOrderDetailList[j].InnerQty, po.InvDeliveryOrderDetailList[j].InnerUomId, po.InvDeliveryOrderDetailList[j].InnerPackUom, po.InvDeliveryOrderDetailList[j].InnerPackUomEquivalent, po.InvDeliveryOrderDetailList[j].InnerEbUnit, po.InvDeliveryOrderDetailList[j].InnerEbTotal, po.InvDeliveryOrderDetailList[j].AccessWtSet, po.InvDeliveryOrderDetailList[j].DoWeight, po.InvDeliveryOrderDetailList[j].LoadingQty, po.InvDeliveryOrderDetailList[j].LoadingWeight, po.InvDeliveryOrderDetailList[j].GrossWeight, po.InvDeliveryOrderDetailList[j].OtherWeight, po.InvDeliveryOrderDetailList[j].ThirdPartyAnalysisId, po.InvDeliveryOrderDetailList[j].ThirdPartyAnalysisNo, po.InvDeliveryOrderDetailList[j].ThirdPartyAnalysisSubId, po.InvDeliveryOrderDetailList[j].ThirdPartyAnalysisSubNo, po.InvDeliveryOrderDetailList[j].ContainerId, po.InvDeliveryOrderDetailList[j].Container, po.InvDeliveryOrderDetailList[j].ContainerRemarks, po.InvDeliveryOrderDetailList[j].InspectionRemarks, po.InvDeliveryOrderDetailList[j].LoadingRemarks);
				dtContFromReadById.Rows.Add(po.InvDeliveryOrderDetailList[j].ContainerId, po.InvDeliveryOrderDetailList[j].Container);
			}
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			CalculateTotalInformation();
			dtOtherItem.Rows.Clear();
			if (po.InvDeliveryOrderExpenseslist != null && po.InvDeliveryOrderExpenseslist.Count > 0)
			{
				for (int i = 0; i < po.InvDeliveryOrderExpenseslist.Count; i++)
				{
					dtOtherItem.Rows.Add(po.InvDeliveryOrderExpenseslist[i].Id, po.InvDeliveryOrderExpenseslist[i].ExImInvoiceId, po.InvDeliveryOrderExpenseslist[i].InvoiceOtherItemDetailId, po.InvDeliveryOrderExpenseslist[i].InvoiceNo, po.InvDeliveryOrderExpenseslist[i].ItemId, po.InvDeliveryOrderExpenseslist[i].OtherItemName, po.InvDeliveryOrderExpenseslist[i].Qty, po.InvDeliveryOrderExpenseslist[i].WeightPerQty, po.InvDeliveryOrderExpenseslist[i].NetWeight, po.InvDeliveryOrderExpenseslist[i].Remarks);
				}
			}
			grdotheritems.DataSource = dtOtherItem;
			grdotheritems.RetrieveStructure();
			grdSettingsOtherItems();
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			btnsave.Visible = false;
			btnupdate.Visible = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDatabyInvoiceId()
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_00f3: Expected O, but got Unknown
		try
		{
			((Control)(object)CmbSupplierCustomer).Text = string.Empty;
			((UltraGridBase)CmbSupplierCustomer).DataSource = null;
			((Control)(object)CmbOrderNo).Text = string.Empty;
			((UltraGridBase)CmbOrderNo).DataSource = null;
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(cmbInvoiceNo.Value)
			};
			DataTable dtInvHeaderData = ExImInvoice.GetExportInvoiceHeaderDataByInvoiceId(val);
			if (dtInvHeaderData.Rows.Count > 0)
			{
				txtInvoiceDate.Value = Conversion.ToDateTime(dtInvHeaderData.Rows[0]["DocDate"]);
				DDL.BindDDLNew(dtInvHeaderData, CmbSupplierCustomer, "Id", "CustomerName", "Customer Name", false);
				((UltraGridBase)CmbSupplierCustomer).Rows[0].Activate();
			}
			DataTable dt = ExImInvoice.GetContractsByInvoiceId(val);
			if (dt.Rows.Count > 0)
			{
				DataTable dtinvoice = new DataTable();
				dtinvoice.Columns.Add("OrderId");
				dtinvoice.Columns.Add("OrderNo");
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtinvoice.Rows.Add(dt.Rows[i]["ContractId"], dt.Rows[i]["ContractNo"]);
				}
				DDL.BindDDL(dtinvoice, CmbOrderNo, "OrderId", "OrderNo", "Sale ContractNo", ZeroIndex: false);
				((UltraGridBase)CmbOrderNo).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetInvoicewiseDoWeightandBalanceWeight()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable dt = ExImInvoice.GetInvoicewiseDoWeightandBalanceWeight(new ExImInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(cmbInvoiceNo.Value)
			});
			if (dt.Rows.Count > 0)
			{
				txtInvoiceWeight.Text = Conversion.ToString(dt.Rows[0]["InvoiceWeight"]);
				txtDoWeight.Text = Conversion.ToString(dt.Rows[0]["DoWeight"]);
				txtrejectedWeight.Text = Conversion.ToString(dt.Rows[0]["GpRejectedWeight"]);
				txtBalanceWeight.Text = Conversion.ToString(dt.Rows[0]["WeightAvailableForDo"]);
				txtExportReturnWeight.Text = Conversion.ToString(dt.Rows[0]["ReturnWeight"]);
			}
			else
			{
				txtInvoiceWeight.Text = "0";
				txtDoWeight.Text = "0";
				txtrejectedWeight.Text = "0";
				txtBalanceWeight.Text = "0";
				txtExportReturnWeight.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbItemName_TextChanged(object sender, EventArgs e)
	{
		try
		{
			double BalMTon = 0.0;
			int ItemId = 0;
			int InvoiceId = Conversion.ToInt(cmbInvoiceNo.Value);
			if (((UltraGridBase)CmbItemName).ActiveRow != null)
			{
				ItemId = Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemId"].Value);
				BindPackUomAndPackingType(Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["InvoiceDetailId"].Value));
				if ((DataTable)((UltraGridBase)CmbPackUomInner).DataSource == null)
				{
					BindInnerPackUom(ItemId);
				}
				CmbPackingType.Value = Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["PackingTypeId"].Value);
				BalMTon = Conversion.ToDouble(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["BalMTon"].Value);
			}
			ThirdPartyNoBind(ItemId, InvoiceId);
			txtBalnceMtonOfSelectedItem.Text = BalMTon.ToString("#,##.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbInspectionNoPackListDetail_Leave(object sender, EventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(CmbInspectionNoPackListDetail.Value);
			ThirdPartySubLotNoBind(Id);
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
			int InvoiceId = Conversion.ToInt(cmbInvoiceNo.Value);
			GetDatabyInvoiceId();
			GetInvoicewiseDoWeightandBalanceWeight();
			if (InvoiceId != 0)
			{
				GetContainerNoFromCROByInvoiceId(InvoiceId);
			}
			else
			{
				((UltraGridBase)cmbContainerNoDetail).DataSource = null;
			}
			int ItemId = 0;
			if (((UltraGridBase)CmbItemName).ActiveRow != null)
			{
				ItemId = Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemId"].Value);
			}
			ThirdPartyNoBind(ItemId, InvoiceId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbOrderNo_TextChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)CmbItemName).Text = string.Empty;
			((UltraGridBase)CmbItemName).DataSource = null;
			((Control)(object)CmbCropYear).Text = string.Empty;
			((UltraGridBase)CmbCropYear).DataSource = null;
			int Id = Conversion.ToInt(cmbInvoiceNo.Value);
			int OrderId = Conversion.ToInt(CmbOrderNo.Value);
			if (Id > 0 && OrderId > 0)
			{
				DataTable dtItem = ExImInvoice.ExportInvoiceDetailByHeaderIdNew(UserAccount.OrganizationId, UserAccount.CompanyId, OrderId, Id);
				if (dtItem.Rows.Count > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("ItemId", typeof(int));
					dt.Columns.Add("ItemName");
					dt.Columns.Add("InvoiceDetailId", typeof(int));
					dt.Columns.Add("PackUomId");
					dt.Columns.Add("PackUom");
					dt.Columns.Add("PackingTypeId", typeof(int));
					dt.Columns.Add("BalMTon", typeof(double));
					DataTable dtCrop = new DataTable();
					dtCrop.Columns.Add("CropYearId", typeof(int));
					dtCrop.Columns.Add("CropYear");
					int i;
					for (i = 0; i < dtItem.Rows.Count; i++)
					{
						dt.Rows.Add(dtItem.Rows[i]["ItemId"], dtItem.Rows[i]["ItemName"], dtItem.Rows[i]["InvoiceDetailId"], dtItem.Rows[i]["PackUomId"], dtItem.Rows[i]["PackUom"], dtItem.Rows[i]["PackingMaterialTypeId"], dtItem.Rows[i]["BalMTon"]);
						if (!dtCrop.AsEnumerable().Any((DataRow row) => row.Field<int>("CropYearId") == Conversion.ToInt(dtItem.Rows[i]["CropYearId"])))
						{
							dtCrop.Rows.Add(dtItem.Rows[i]["CropYearId"], dtItem.Rows[i]["CropYear"]);
						}
					}
					if (dt.Rows.Count > 0)
					{
						DDL.BindDDL(dt, CmbItemName, "InvoiceDetailId", "ItemName", "Item Name", ZeroIndex: false);
						if (dt.Rows.Count == 1)
						{
							((UltraGridBase)CmbItemName).Rows[0].Activate();
						}
						((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[3].Hidden = true;
						((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[5].Hidden = true;
					}
					if (dtCrop.Rows.Count > 0)
					{
						DDL.BindDDL(dtCrop, CmbCropYear, "CropYearId", "CropYear", "CropYear", ZeroIndex: false);
						((UltraGridBase)CmbCropYear).Rows[0].Activate();
					}
				}
				else
				{
					((Control)(object)CmbItemName).Text = string.Empty;
					((UltraGridBase)CmbItemName).DataSource = null;
					((Control)(object)CmbCropYear).Text = string.Empty;
					((UltraGridBase)CmbCropYear).DataSource = null;
				}
			}
			else
			{
				((Control)(object)CmbItemName).Text = string.Empty;
				((UltraGridBase)CmbItemName).DataSource = null;
				((Control)(object)CmbCropYear).Text = string.Empty;
				((UltraGridBase)CmbCropYear).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbInvoiceNo_TextChanged(object sender, EventArgs e)
	{
		try
		{
			GetInvoicewiseDoWeightandBalanceWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetContainerNoFromCROByInvoiceId(int InvoiceId)
	{
		try
		{
			int ConNo = Conversion.ToInt(cmbContainerNoDetail.Value);
			DataTable dt = ExImInvoice.GetContainerNoFromShipingLineBookingDetailByInvoiceId(UserAccount.OrganizationId, UserAccount.CompanyId, InvoiceId, RecId);
			if (dt.Rows.Count > 0)
			{
				dtCont.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtCont.Rows.Add(dt.Rows[i]["ContainerId"], dt.Rows[i]["Container"]);
				}
				if (dtContFromReadById.Rows.Count > 0)
				{
					foreach (DataRow r in dtContFromReadById.Rows)
					{
						int containerIdClone = Conversion.ToInt(r["ContainerId"]);
						if (!dtCont.AsEnumerable().Any((DataRow contRow) => Conversion.ToInt(contRow["ContainerId"]) == containerIdClone))
						{
							dtCont.Rows.Add(r["ContainerId"], r["ContainerNo"]);
						}
					}
				}
				if (dtCont.Rows.Count > 0)
				{
					DDL.BindDDL(dtCont, cmbContainerNoDetail, "ContainerId", "ContainerNo", "Container No", ZeroIndex: true);
					((UltraGridBase)cmbContainerNoDetail).Rows[0].Activate();
				}
				else
				{
					((UltraGridBase)cmbContainerNoDetail).DataSource = null;
				}
			}
			else
			{
				((UltraGridBase)cmbContainerNoDetail).DataSource = null;
			}
			if (ConNo > 0)
			{
				cmbContainerNoDetail.Value = ConNo;
			}
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
			updateDetailIndexOtherItem = -1;
			((Control)(object)CmbOtherItem).Text = string.Empty;
			txtoQty.Text = string.Empty;
			txtoRemarks.Text = string.Empty;
			CmbOtherItem.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
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
			RecId = 0;
			IsApproved = false;
			DocDate.Focus();
			((Control)(object)cmbtransporter).Text = string.Empty;
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			txtItemWeight.Text = string.Empty;
			txtPackingWeight.Text = string.Empty;
			txtOtherWeight.Text = string.Empty;
			txtTotalGrossWeight.Text = string.Empty;
			txtOtherRemarks.Text = string.Empty;
			((Control)(object)CmbItemName).Text = string.Empty;
			((Control)(object)CmbPackUomMaster).Text = string.Empty;
			txtQtyMaster.Text = string.Empty;
			txtNetWeight.Text = string.Empty;
			((Control)(object)CmbSupplierCustomer).Text = string.Empty;
			((Control)(object)CmbOrderNo).Text = string.Empty;
			((Control)(object)CmbPackingType).Text = string.Empty;
			txtremarks.Text = string.Empty;
			((Control)(object)cmbInvoiceNo).Text = string.Empty;
			txtVehicleNo.Text = string.Empty;
			txtInvoiceDate.Value = DateTime.Now;
			dtContFromReadById.Rows.Clear();
			((UltraGridBase)CmbSupplierCustomer).DataSource = null;
			((UltraGridBase)CmbOrderNo).DataSource = null;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			DocumentNoFill();
			table.Rows.Clear();
			grd.DataSource = table;
			((Control)(object)cmbInvoiceNo).Enabled = true;
			CmbInvoiceNoFill();
			txtInvoiceWeight.Text = string.Empty;
			txtDoWeight.Text = string.Empty;
			txtBalanceWeight.Text = string.Empty;
			txtExportReturnWeight.Text = string.Empty;
			txtrejectedWeight.Text = string.Empty;
			CmbBranch.Value = UserAccount.BranchesId;
			ResetOtherItems();
			dtOtherItem.Rows.Clear();
			grdotheritems.ClearStructure();
			defaultConfiquration();
			ResetDetail();
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
			cmbInvoiceNo.Focus();
			((Control)(object)CmbItemName).Text = string.Empty;
			((UltraGridBase)CmbItemName).DataSource = null;
			((Control)(object)CmbPackUomMaster).Text = string.Empty;
			((UltraGridBase)CmbPackUomMaster).DataSource = null;
			txtQtyMaster.Text = string.Empty;
			txtNetWeight.Text = string.Empty;
			((Control)(object)CmbOrderNo).Text = string.Empty;
			((Control)(object)CmbPackingType).Text = string.Empty;
			txtremarksdetail.Text = string.Empty;
			txtEbUnitMaster.Text = string.Empty;
			txtQtyInner.Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			txtEbUnitInner.Text = string.Empty;
			txtEbTotalInner.Text = string.Empty;
			txtInspectionRemarks.Text = string.Empty;
			txtContainerRemarks.Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			((Control)(object)CmbSupplierCustomer).Text = string.Empty;
			((Control)(object)cmbContainerNoDetail).Text = string.Empty;
			((UltraGridBase)cmbContainerNoDetail).DataSource = null;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			updateDetailIndex = -1;
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
			BranchList = CommonServices.BrancheServiceBind();
			SaleCostingJobOrderWise = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("SaleCostingJobOrderWise"));
			CmbExportPackingTypeFill();
			WareHouseFill();
			JobLotFill();
			CmbInvoiceNoFill();
			CmbLoadingPortFill();
			TransporterFill();
			OtherItemsBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExportDeliveryOrderNew_FormClosing(object sender, FormClosingEventArgs e)
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

	private void txtQtyMaster_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToDouble((object)txtQtyMaster.Text) != 0.0 && Conversion.ToDouble((object)txtQtyInner.Text) == 0.0)
			{
				double Qty = Conversion.ToDouble((object)txtQtyMaster.Text);
				txtQtyInner.Text = Qty.ToString("#,##0.##");
				CalculateEbUnitOrTotalOfInner();
			}
			CalculateNetWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Print_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.InvDeliveryOrderSlipWithSubReport(RecId, 84);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnExportDoSlip396_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				CommonServices.ExportDeliveryOrderSlip396(RecId, 84);
				return;
			}
			throw new Exception("No record found");
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
			grdhistory.DataSource = null;
			grdDetail.DataSource = null;
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
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		try
		{
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("Customer", typeof(string));
			DataTable dtInvoice = new DataTable();
			dtInvoice.Columns.Add("Id", typeof(int));
			dtInvoice.Columns.Add("InvoiceNo", typeof(string));
			DataTable dt = InvDeliveryOrder.GetDataForDropDownFromDeliveryOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				ReqType = "Export",
				DocumentTypeIds = "84"
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Customer")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "InvoiceNo")
					{
						dtInvoice.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtCustomer, CmbCustomerHistory, "Id", "Customer", "Party", ZeroIndex: false);
			DDL.BindDDL(dtInvoice, CmbInvoiceNoHistory, "Id", "InvoiceNo", "Invoice No", ZeroIndex: false);
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
			gridhistoryfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				ReadById(Conversion.ToInt(r.Cells["Id"].Value));
			}
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
			obj.DocumentTypeId = 84;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
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
			obj.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			obj.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			obj.ExImInvoiceId = Conversion.ToInt(CmbInvoiceNoHistory.Value);
			obj.BranchesId = UserAccount.BranchesId;
			DataTable dtHis = InvDeliveryOrder.ExportFormHistoryNew(obj);
			if (dtHis.Rows.Count > 0)
			{
				if (dtHistory.Columns.Count == 0)
				{
					dtHistory.Columns.Add("Id", typeof(int));
					dtHistory.Columns.Add("DoDate", typeof(DateTime));
					dtHistory.Columns.Add("DoNo", typeof(int));
					dtHistory.Columns.Add("InvoiceId", typeof(int));
					dtHistory.Columns.Add("InvoiceDate", typeof(DateTime));
					dtHistory.Columns.Add("InvoiceNo", typeof(string));
					dtHistory.Columns.Add("InvoiceNetWeight", typeof(double));
					dtHistory.Columns.Add("InvoiceGrossWeight", typeof(double));
					dtHistory.Columns.Add("CustomerName", typeof(string));
					dtHistory.Columns.Add("OrderNo", typeof(string));
					dtHistory.Columns.Add("GatePassId", typeof(int));
					dtHistory.Columns.Add("VehicleNo", typeof(string));
					dtHistory.Columns.Add("Transporter", typeof(string));
					dtHistory.Columns.Add("LoadingPort", typeof(string));
					dtHistory.Columns.Add("GpDate", typeof(DateTime));
					dtHistory.Columns.Add("GpSrNo", typeof(int));
					dtHistory.Columns.Add("GpStatus", typeof(string));
					dtHistory.Columns.Add("ForwardingNo", typeof(int));
					dtHistory.Columns.Add("FactoryWeight", typeof(double));
					dtHistory.Columns.Add("EntryDate", typeof(DateTime));
					dtHistory.Columns.Add("EntryUser", typeof(string));
					dtHistory.Columns.Add("ModifyDate", typeof(DateTime));
					dtHistory.Columns.Add("ModifyUser", typeof(string));
					dtHistory.Columns.Add("ApprovedDate", typeof(DateTime));
					dtHistory.Columns.Add("ApprovedUser", typeof(string));
					dtHistory.Columns.Add("BranchName", typeof(string));
					dtHistory.Columns.Add("NoOfAttachments", typeof(int));
				}
				dtHistory.Clear();
				HashSet<int> existingIds = new HashSet<int>();
				foreach (DataRow dr in dtHis.Rows)
				{
					int id = Conversion.ToInt(dr["Id"]);
					if (!existingIds.Contains(id))
					{
						dtHistory.Rows.Add(dr["Id"], Conversion.ToDateTime(dr["DocDate"]).ToShortDateString(), dr["DocNo"], dr["InvoiceId"], dr["InvoiceDate"], dr["InvoiceNo"], dr["InvoiceNetWeight"], dr["InvoiceGrossWeight"], dr["SupplierCustomer"], dr["OrderNo"], dr["GatePassId"], dr["VehicleNo"], dr["Transporter"], dr["LoadingPort"], dr["GpDate"], dr["GpSrNo"], dr["GpStatus"], dr["ForwardingNo"], dr["FactoryWeight"], dr["EntryDate"], dr["EntryUser"], dr["ModifyDate"], dr["ModifyUser"], dr["ApprovedDate"], dr["ApprovedUser"], dr["BranchName"], dr["NoOfAttachments"]);
						existingIds.Add(id);
					}
				}
				grdhistory.DataSource = dtHistory;
				grdhistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				grdhistory.ClearStructure();
				grdDetail.ClearStructure();
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
			List<string> hiddenColumns = new List<string> { "Id", "InvoiceId", "GatePassId" };
			List<string> ExcludeC = new List<string> { "InvoiceNetWeight", "InvoiceGrossWeight", "FactoryWeight" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdhistory, 2, 4, hiddenColumns, null, null, ColumnSetting: true, BracketsForamt: false, ExcludeC);
			grdhistory.RootTable.Columns["Branchname"].Visible = BranchList.Rows.Count > 1;
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.Columns["DoDate"].Width = Constants.ExportConstants.DateConstant;
			grdhistory.RootTable.Columns["DoNo"].Width = Constants.ExportConstants.DocNoConstant;
			grdhistory.RootTable.Columns["InvoiceDate"].Width = Constants.ExportConstants.DateConstant;
			grdhistory.RootTable.Columns["InvoiceNo"].Width = Constants.ExportConstants.InvoiceNoConstant;
			grdhistory.RootTable.Columns["InvoiceNetWeight"].Width = Constants.ExportConstants.WeightConstant;
			grdhistory.RootTable.Columns["InvoiceGrossWeight"].Width = Constants.ExportConstants.WeightConstant;
			grdhistory.RootTable.Columns["CustomerName"].Width = Constants.ExportConstants.StringConstant;
			grdhistory.RootTable.Columns["OrderNo"].Width = Constants.ExportConstants.ContractNoConstant;
			grdhistory.RootTable.Columns["VehicleNo"].Width = Constants.ExportConstants.VehicleNoConstant;
			grdhistory.RootTable.Columns["Transporter"].Width = Constants.ExportConstants.StringConstant;
			grdhistory.RootTable.Columns["LoadingPort"].Width = Constants.ExportConstants.PortConstant;
			grdhistory.RootTable.Columns["GpDate"].Width = Constants.ExportConstants.DateConstant;
			grdhistory.RootTable.Columns["GpSrNo"].Width = Constants.ExportConstants.DocNoConstant;
			grdhistory.RootTable.Columns["GpStatus"].Width = Constants.ExportConstants.StatusConstant;
			grdhistory.RootTable.Columns["ForwardingNo"].Width = Constants.ExportConstants.DocNoConstant;
			grdhistory.RootTable.Columns["FactoryWeight"].Width = Constants.ExportConstants.WeightConstant;
			grdhistory.RootTable.Columns["EntryDate"].Width = Constants.ExportConstants.DateTimeConstantddMMM;
			grdhistory.RootTable.Columns["EntryUser"].Width = Constants.ExportConstants.EntryUserConstants;
			grdhistory.RootTable.Columns["ModifyDate"].Width = Constants.ExportConstants.DateTimeConstantddMMM;
			grdhistory.RootTable.Columns["ModifyUser"].Width = Constants.ExportConstants.EntryUserConstants;
			grdhistory.RootTable.Columns["ApprovedDate"].Width = Constants.ExportConstants.DateTimeConstantddMMM;
			grdhistory.RootTable.Columns["ApprovedUser"].Width = Constants.ExportConstants.EntryUserConstants;
			grdhistory.RootTable.Columns["BranchName"].Width = Constants.ExportConstants.StringConstant;
			grdhistory.RootTable.Columns["NoOfAttachments"].Width = Constants.ExportConstants.NoOfAttachmentsConstants;
			GridEX_Helper.AddButton(grdhistory, "Edit", "Edit", 40, 0);
			GridEX_Helper.AddButton(grdhistory, "Print", "Print_262", 70, 1);
			GridEX_Helper.AddButton(grdhistory, "Print_396", "Print_396", 70, 2);
			GridEX_Helper.AddButton(grdhistory, "GpPrint", "GpPrint", 60, 3);
			GridEX_Helper.AddButton(grdhistory, "DoPrintByInvoice", "DoPrintByInvoice", 120, 4);
			grdhistory.FrozenColumns = 5;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value), 84);
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
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					ReadById(CurrId);
				}
				if (e.Column.Key == "Print")
				{
					CommonServices.InvDeliveryOrderSlipWithSubReport(CurrId, 84);
				}
				if (e.Column.Key == "Print_396")
				{
					CommonServices.ExportDeliveryOrderSlip396(CurrId, 84);
				}
				if (e.Column.Key == "GpPrint")
				{
					int InvoiceId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
					CrystalReportPrint_Helper.OutwardGatePassPrintByInvoicesIds_295(null, InvoiceId);
				}
				if (e.Column.Key == "DoPrintByInvoice")
				{
					CrystalReportPrint_Helper.DeliveryOrderPrintByInvoicesId(Conversion.ToInt(r.Cells["InvoiceId"].Value), 84);
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
		try
		{
			GetDetailByHeaderId();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailByHeaderId()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			InvDeliveryOrder po = InvDeliveryOrder.GetByID(Conversion.ToInt(r.Cells["Id"].Value));
			if (po != null && po.InvDeliveryOrderDetailList?.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("InvoiceId", typeof(int));
				dt.Columns.Add("InvocieNo", typeof(string));
				dt.Columns.Add("CustomerName", typeof(string));
				dt.Columns.Add("OrderNo", typeof(string));
				dt.Columns.Add("WareHouse", typeof(string));
				dt.Columns.Add("JobLot", typeof(string));
				dt.Columns.Add("Item", typeof(string));
				dt.Columns.Add("ItemUOM", typeof(string));
				dt.Columns.Add("PackingType", typeof(string));
				dt.Columns.Add("MasterQTY", typeof(double));
				dt.Columns.Add("MasterUOM");
				dt.Columns.Add("EbUnitMaster", typeof(double));
				dt.Columns.Add("EbTotalMaster", typeof(double));
				dt.Columns.Add("InnerQTY", typeof(double));
				dt.Columns.Add("InnerUOM");
				dt.Columns.Add("EbUnitInner", typeof(double));
				dt.Columns.Add("EbTotalInner", typeof(double));
				dt.Columns.Add("AccessWtSet", typeof(double));
				dt.Columns.Add("NetWeight", typeof(double));
				dt.Columns.Add("LoadQty", typeof(double));
				dt.Columns.Add("LoadWeight", typeof(double));
				dt.Columns.Add("GrossWeight", typeof(double));
				dt.Columns.Add("OtherWeight", typeof(double));
				dt.Columns.Add("ContainerNo", typeof(string));
				dt.Columns.Add("ContainerRemarks", typeof(string));
				dt.Columns.Add("InspectionRemarks", typeof(string));
				dt.Columns.Add("Remarks", typeof(string));
				dt.Rows.Clear();
				for (int i = 0; i < po.InvDeliveryOrderDetailList.Count; i++)
				{
					dt.Rows.Add(po.InvDeliveryOrderDetailList[i].Id, po.InvDeliveryOrderDetailList[i].ExImInvoiceId, po.InvDeliveryOrderDetailList[i].InvoiceNo, po.InvDeliveryOrderDetailList[i].SupplierCustomer, po.InvDeliveryOrderDetailList[i].OrderNo, po.InvDeliveryOrderDetailList[i].WareHouseName, po.InvDeliveryOrderDetailList[i].JobLotDescription, po.InvDeliveryOrderDetailList[i].ItemName, po.InvDeliveryOrderDetailList[i].PackUOM, po.InvDeliveryOrderDetailList[i].PackTypeDesc, po.InvDeliveryOrderDetailList[i].DoQty, po.InvDeliveryOrderDetailList[i].PackUOM, po.InvDeliveryOrderDetailList[i].PackingWeight, po.InvDeliveryOrderDetailList[i].OuterEbTotal, po.InvDeliveryOrderDetailList[i].InnerQty, po.InvDeliveryOrderDetailList[i].InnerPackUom, po.InvDeliveryOrderDetailList[i].InnerEbUnit, po.InvDeliveryOrderDetailList[i].InnerEbTotal, po.InvDeliveryOrderDetailList[i].AccessWtSet, po.InvDeliveryOrderDetailList[i].DoWeight, po.InvDeliveryOrderDetailList[i].LoadingQty, po.InvDeliveryOrderDetailList[i].LoadingWeight, po.InvDeliveryOrderDetailList[i].GrossWeight, po.InvDeliveryOrderDetailList[i].OtherWeight, po.InvDeliveryOrderDetailList[i].Container, po.InvDeliveryOrderDetailList[i].ContainerRemarks, po.InvDeliveryOrderDetailList[i].InspectionRemarks, po.InvDeliveryOrderDetailList[i].LoadingRemarks);
				}
				grdDetail.DataSource = dt;
				grdDetail.RetrieveStructure();
				grdDetailSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailSettings()
	{
		try
		{
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["InvoiceId"].Visible = false;
			CommonServices.GridWrappingAndColumnSettings(grdDetail, 2);
			GridEX_Helper.AddButton(grdDetail, "DoPrintByInvoice", "DoPrintByInvoice", 120, 0);
			grdDetail.FrozenColumns = 1;
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
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "DoPrintByInvoice")
			{
				CrystalReportPrint_Helper.DeliveryOrderPrintByInvoicesId(Conversion.ToInt(r.Cells["InvoiceId"].Value), 84);
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grd, table);
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdhistory, dtHistory);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnattachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void CalculateNetWeight()
	{
		try
		{
			double NetWeight = 0.0;
			if (((UltraGridBase)CmbPackUomMaster).ActiveRow != null && Conversion.ToInt(CmbPackUomMaster.Value) != 0 && Conversion.ToDouble((object)txtQtyMaster.Text) != 0.0)
			{
				double num = Conversion.ToDouble(((UltraDropDownBase)CmbPackUomMaster).SelectedRow.Cells[2].Value);
				double Qty = Conversion.ToDouble((object)txtQtyMaster.Text);
				txtNetWeight.Text = (num * Qty).ToString("#,##0.##");
			}
			else
			{
				txtNetWeight.Text = 0.ToString("#,##0.##");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculaterEbMaster()
	{
		try
		{
			double PackingWeight = Conversion.ToDouble((object)txtEbUnitMaster.Text);
			double TotalPackingWeight = Conversion.ToDouble((object)txtEbTotalMaster.Text);
			double Qty = Conversion.ToDouble((object)txtQtyMaster.Text);
			if (Conversion.ToString(base.ActiveControl.Tag) == "TotalPackingWeightMaster")
			{
				double EbUnit = TotalPackingWeight / Qty;
				txtEbUnitMaster.Text = EbUnit.ToString("#,##0.##");
				bool_EbMasterUnit = true;
				bool_EbMasterTotal = false;
			}
			else if (Conversion.ToString(base.ActiveControl.Tag) == "PackingWeightMaster")
			{
				txtEbTotalMaster.Text = (PackingWeight * Qty).ToString("#,##0.##");
				bool_EbMasterTotal = true;
				bool_EbMasterUnit = false;
			}
			else if (bool_EbMasterUnit)
			{
				txtEbTotalMaster.Text = (PackingWeight * Qty).ToString("#,##0.##");
			}
			else if (bool_EbMasterTotal)
			{
				double EbUnit2 = TotalPackingWeight / Qty;
				txtEbUnitMaster.Text = EbUnit2.ToString("#,##0.##");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateEbUnitOrTotalOfInner()
	{
		try
		{
			double PackingWeight = Conversion.ToDouble((object)txtEbUnitInner.Text);
			double TotalPackingWeight = Conversion.ToDouble((object)txtEbTotalInner.Text);
			double Qty = Conversion.ToDouble((object)txtQtyInner.Text);
			if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "TotalPackingWeight")
			{
				double EbUnit = TotalPackingWeight / Qty;
				txtEbUnitInner.Text = EbUnit.ToString("#,##0.##");
				bool_EbInnerTotal = true;
				bool_EbInnerUnit = false;
			}
			else if (Conversion.ToString(base.ActiveControl.Tag) == "PackingWeight")
			{
				txtEbTotalInner.Text = (PackingWeight * Qty).ToString("#,##0.##");
				bool_EbInnerUnit = true;
				bool_EbInnerTotal = false;
			}
			else if (bool_EbInnerUnit)
			{
				txtEbTotalInner.Text = (PackingWeight * Qty).ToString("#,##0.##");
			}
			else if (bool_EbInnerTotal)
			{
				double EbUnit2 = TotalPackingWeight / Qty;
				txtEbUnitInner.Text = EbUnit2.ToString("#,##0.##");
			}
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
			CalculaterEbMaster();
			CalculateNetWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPackUomMaster_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculaterEbMaster();
			if (Conversion.ToInt(CmbPackUomMaster.Value) != 0)
			{
				DataTable dt = (DataTable)((UltraGridBase)CmbPackUomInner).DataSource;
				if (dt != null && dt.Rows.Count > 2)
				{
					int UOM = Conversion.ToInt(CmbPackUomMaster.Value);
					CmbPackUomInner.Value = UOM;
				}
				CalculateEbUnitOrTotalOfInner();
			}
			CalculateNetWeight();
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
			CalculaterEbMaster();
			CalculateGrossWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEbTotalMaster_TextChanged(object sender, EventArgs e)
	{
		try
		{
			double NetWeight = Conversion.ToDouble((object)txtNetWeight.Text);
			if (NetWeight > 0.0)
			{
				double GrossWeight = NetWeight + Conversion.ToDouble((object)txtEbTotalMaster.Text) + Conversion.ToDouble((object)txtEbTotalInner.Text);
				txtGrossWeight.Text = GrossWeight.ToString("#,##0.##");
			}
			else
			{
				txtGrossWeight.Text = NetWeight.ToString("#,##0.##");
			}
			CalculaterEbMaster();
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
			CalculateEbUnitOrTotalOfInner();
			CalculateGrossWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPackUomInner_Leave(object sender, EventArgs e)
	{
		try
		{
			calculateAutoInnerQty();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEbTotal_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateEbUnitOrTotalOfInner();
			CalculateGrossWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEbUnit_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateEbUnitOrTotalOfInner();
			CalculateGrossWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOtherWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateTotalInformation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAccessWtSet_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateGrossWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtNetWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			calculateAutoInnerQty();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void calculateAutoInnerQty()
	{
		try
		{
			double weight = Conversion.ToDouble((object)txtNetWeight.Text);
			if (weight > 0.0 && ((UltraGridBase)CmbPackUomInner).ActiveRow != null && Conversion.ToInt(CmbPackUomInner.Value) > 0)
			{
				double Equivalent = Conversion.ToDouble(((UltraDropDownBase)CmbPackUomInner).SelectedRow.Cells["Equivalent"].Value);
				double qtyInner = weight / Equivalent;
				txtQtyInner.Text = qtyInner.ToString("#,##0.###");
			}
			else
			{
				txtQtyInner.Text = "0";
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CalculateGrossWeight()
	{
		try
		{
			double NetWeight = Conversion.ToDouble((object)txtNetWeight.Text);
			if (NetWeight > 0.0)
			{
				double Qty = Conversion.ToDouble((object)txtQtyMaster.Text);
				double AccessWtSetInKg = Conversion.ToDouble((object)txtAccessWtSet.Text) * Qty / 1000.0;
				double GrossWeight = NetWeight + AccessWtSetInKg + Conversion.ToDouble((object)txtEbTotalMaster.Text) + Conversion.ToDouble((object)txtEbTotalInner.Text);
				txtGrossWeight.Text = GrossWeight.ToString("#,##0.##");
			}
			else
			{
				txtGrossWeight.Text = NetWeight.ToString("#,##0.##");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtTotalQty_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void txtqty_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void txtPackingWeight_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void txtweight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtPackingWt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtOtherWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtEbTotalMaster_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtAccessWtSet_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtNetWeightOtherItem_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtWtPerQtyOtherItem_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtoQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void btnPurchaseOrderLoader_Click(object sender, EventArgs e)
	{
		try
		{
			LoadPO = new LoadCommercialInvoiceForDO(UserAccount);
			LoadPO.ShowDialog();
			LoadPO.BringToFront();
			LoadInGridDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetail()
	{
		try
		{
			DataTable dtInvoice = LoadPO.dtLoader;
			if (dtInvoice.Rows.Count <= 0)
			{
				return;
			}
			string InvoiceIds = "";
			for (int j = 0; j < dtInvoice.Rows.Count; j++)
			{
				InvoiceIds = InvoiceIds + "," + Conversion.ToInt(dtInvoice.Rows[j]["ExImInvoiceId"]);
			}
			if (RestrictOneInvoiceOnDo && (from r in dtInvoice.AsEnumerable()
				where r["ExImInvoiceId"] != DBNull.Value
				select r["ExImInvoiceId"].ToString()).Distinct().ToList().Count > 1)
			{
				throw new Exception("You have Selected Multiple different Invoices.but Only rows with the same Invoice are allowed.");
			}
			bool flag = false;
			for (int i = 0; i < dtInvoice.Rows.Count; i++)
			{
				if (grd.GetRows().Length != 0)
				{
					GridEXRow[] rows = grd.GetRows();
					for (int k = 0; k < rows.Length; k++)
					{
						if (Conversion.ToInt(rows[k].Cells["InvoiceDetailId"].Value) == Conversion.ToInt(dtInvoice.Rows[i]["InvoiceDetailId"]))
						{
							flag = true;
							break;
						}
					}
					if (RestrictOneInvoiceOnDo && !GridEX_Helper.IsAnyCellValueHaveSameValue(grd, "InvoiceId", Conversion.ToInt(dtInvoice.Rows[i]["ExImInvoiceId"])))
					{
						throw new Exception("Data Against another Invoice Exists in Grid,So you can't add different Invoice");
					}
				}
				if (!flag)
				{
					table.Rows.Add(0, dtInvoice.Rows[i]["InvoiceDetailId"], dtInvoice.Rows[i]["ExImInvoiceId"], dtInvoice.Rows[i]["InvoiceNo"], dtInvoice.Rows[i]["SupplierCustomerId"], dtInvoice.Rows[i]["CustomerName"], dtInvoice.Rows[i]["ContractId"], dtInvoice.Rows[i]["ContractNo"], 0, "", dtInvoice.Rows[i]["JobLotId"], dtInvoice.Rows[i]["JobLotDescription"], dtInvoice.Rows[i]["CropYearId"], dtInvoice.Rows[i]["CropYear"], dtInvoice.Rows[i]["ItemId"], dtInvoice.Rows[i]["ItemName"], dtInvoice.Rows[i]["PackingMaterialTypeId"], dtInvoice.Rows[i]["PackTypeDesc"], dtInvoice.Rows[i]["BalQty"], dtInvoice.Rows[i]["PackUomId"], dtInvoice.Rows[i]["PackUom"], dtInvoice.Rows[i]["PackEquivalent"], 0, 0, 0, 0, "", 0, 0, 0, 0, dtInvoice.Rows[i]["BalWeight"], dtInvoice.Rows[i]["BalQty"], dtInvoice.Rows[i]["BalWeight"], dtInvoice.Rows[i]["BalWeight"], 0, dtInvoice.Rows[i]["ThirdPartyAnalysisId"], dtInvoice.Rows[i]["ThirdPartyAnalysisNo"], 0, "", 0, "", "", "", "");
				}
				flag = false;
			}
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			LoadOtherItemsData(InvoiceIds);
			CalculateTotalInformation();
			InvoiceIds = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadOtherItemsData(string IDs)
	{
		try
		{
			DataTable dt = ExImInvoice.InvoiceOtherItemsByInvoiceIds(IDs);
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			bool flag = false;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (grdotheritems.GetRows().Length != 0)
				{
					GridEXRow[] rows = grdotheritems.GetRows();
					for (int j = 0; j < rows.Length; j++)
					{
						if (Conversion.ToInt(rows[j].Cells["InvoiceDetailId"].Value) == Conversion.ToInt(dt.Rows[i]["Id"]))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					dtOtherItem.Rows.Add(0, dt.Rows[i]["ExImInvoiceId"], dt.Rows[i]["Id"], dt.Rows[i]["InvoiceNo"], dt.Rows[i]["ItemId"], dt.Rows[i]["ItemName"], dt.Rows[i]["oItemQty"], dt.Rows[i]["oItemWeightKgs"], Conversion.ToDouble(dt.Rows[i]["oItemWeightKgs"]) * Conversion.ToDouble(dt.Rows[i]["oItemQty"]), dt.Rows[i]["OtherItemRemarks"]);
				}
				flag = false;
			}
			grdotheritems.DataSource = dtOtherItem;
			grdotheritems.RetrieveStructure();
			grdSettingsOtherItems();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCroForm_Click(object sender, EventArgs e)
	{
		try
		{
			EximShippingBookingInfo CroForm = new EximShippingBookingInfo(UserAccount);
			CroForm.Show();
			if (Conversion.ToInt(cmbInvoiceNo.Value) > 0)
			{
				int CroId = ExImExportShipingLineBooking.GetBookingCROIdByInvoiceId(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(cmbInvoiceNo.Value));
				if (CroId > 0)
				{
					CroForm.ReadById(CroId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void labelOutwardGatePassForm_Click(object sender, EventArgs e)
	{
		FormHelper.CanOpenForm("OutwardGatePass", delegate
		{
			OutwardGatePass outwardGatePass = new OutwardGatePass(UserAccount);
			outwardGatePass.Show();
			outwardGatePass.BringToFront();
		}, UserAccount);
	}

	private void labelEximForwardingForm_Click(object sender, EventArgs e)
	{
		FormHelper.CanOpenForm("EximForwarding", delegate
		{
			EximForwarding eximForwarding = new EximForwarding(UserAccount);
			eximForwarding.Show();
			eximForwarding.BringToFront();
		}, UserAccount);
	}

	private void OpenLinkedFormOnInsert(int DoId)
	{
		try
		{
			if (RadWBFormToOpenOnInsert.Checked)
			{
				if (btnsave.Visible && btnsave.Enabled)
				{
					OutwardGatePassForm(DoId);
				}
			}
			else if (RadGrnFormToOpenOnInsert.Checked)
			{
				ForwardingForm(DoId);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void OutwardGatePassForm(int DoId)
	{
		try
		{
			FormHelper.CanOpenForm("OutwardGatePass", delegate
			{
				OutwardGatePass outwardGatePass = new OutwardGatePass(UserAccount);
				outwardGatePass.Show();
				if (DoId > 0)
				{
					((Control)(object)outwardGatePass.cmbgptype).Text = "Export";
					outwardGatePass.CmbGPBasedOn.Value = 84;
					outwardGatePass.CmbOrderno.Text = DoId.ToString();
					outwardGatePass.CmbOrderno_Leave(null, null);
				}
				outwardGatePass.BringToFront();
				bool formClosed = false;
				outwardGatePass.FormClosed += delegate
				{
					formClosed = true;
				};
				while (!formClosed)
				{
					Application.DoEvents();
				}
			}, UserAccount);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ForwardingForm(int DoId)
	{
		try
		{
			FormHelper.CanOpenForm("EximForwarding", delegate
			{
				EximForwarding eximForwarding = new EximForwarding(UserAccount);
				eximForwarding.Show();
				eximForwarding.BringToFront();
				bool formClosed = false;
				eximForwarding.FormClosed += delegate
				{
					formClosed = true;
				};
				while (!formClosed)
				{
					Application.DoEvents();
				}
			}, UserAccount);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculaterOtherItem()
	{
		try
		{
			double PackingWeight = Conversion.ToDouble((object)txtWtPerQtyOtherItem.Text);
			double TotalPackingWeight = Conversion.ToDouble((object)txtNetWeightOtherItem.Text);
			double Qty = Conversion.ToDouble((object)txtoQty.Text);
			if (Conversion.ToString(base.ActiveControl.Tag) == "NetWeight")
			{
				txtWtPerQtyOtherItem.Text = (TotalPackingWeight / Qty).ToString("#,##0.##");
			}
			else if (Conversion.ToString(base.ActiveControl.Tag) == "WeightPerQty")
			{
				txtNetWeightOtherItem.Text = (PackingWeight * Qty).ToString("#,##0.##");
			}
			else
			{
				txtNetWeightOtherItem.Text = (PackingWeight * Qty).ToString("#,##0.##");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtoQty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculaterOtherItem();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtWtPerQtyOtherItem_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculaterOtherItem();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtNetWeightOtherItem_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculaterOtherItem();
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
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
				}
				else
				{
					tabControl1.SelectedIndex = 1;
				}
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnupdate.Visible && btnupdate.Enabled)
				{
					btnupdate_Click(null, null);
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
					DocDate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					cmbInvoiceNo.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				if (e.Control && e.KeyCode == Keys.P && formrights.DoHavePrintRights)
				{
					Print_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1) && formrights.DoHavePrintRights)
				{
					Print_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2) && formrights.DoHavePrintRights)
				{
					btnExportDoSlip396_Click(null, null);
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
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefreshHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdhistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)grdhistory).Focused)
				{
					((Control)(object)grdDetail).Focus();
				}
				else
				{
					((Control)(object)grdhistory).Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Alt+1", "For Print 262");
			dt.Rows.Add("Alt+2", "For Print 396");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Invoice no in Detail Box");
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grd.CurrentColumn != null && grd.CurrentColumn.Key == "Delete")
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
						InvDeliveryOrderdetail vd = new InvDeliveryOrderdetail();
						vd.RefDocumentTypeId = 0;
						vd.RefDocIdNo = 0;
						vd.RefDocSubIdNo = 0;
						vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
						vd.ExImInvoiceId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
						vd.SupplierCustomerId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
						vd.SaleOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
						vd.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
						vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
						vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						vd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
						vd.InvPackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
						vd.PackUomId = Conversion.ToInt(r.Cells["MasterUOMId"].Value);
						vd.PackingWeight = Conversion.ToDouble(r.Cells["EbUnitMaster"].Value);
						vd.OuterEbTotal = Conversion.ToDouble(r.Cells["EbTotalMaster"].Value);
						vd.InnerQty = Conversion.ToDouble(r.Cells["InnerQTY"].Value);
						vd.InnerUomId = Conversion.ToInt(r.Cells["InnerUOMId"].Value);
						vd.InnerEbUnit = Conversion.ToDouble(r.Cells["EbUnitInner"].Value);
						vd.InnerEbTotal = Conversion.ToDouble(r.Cells["EbTotalInner"].Value);
						vd.DoWeight = Conversion.ToDouble(r.Cells["NetWeight"].Value);
						vd.DoQty = Conversion.ToDouble(r.Cells["LoadQty"].Value);
						vd.DoWeight = Conversion.ToDouble(r.Cells["LoadWeight"].Value);
						vd.LoadingQty = vd.DoQty;
						vd.LoadingWeight = vd.DoWeight;
						vd.TotalPackingWeight = Conversion.ToDouble((object)(vd.InnerEbTotal + vd.OuterEbTotal));
						vd.GrossWeight = Conversion.ToDouble(r.Cells["GrossWeight"].Value);
						vd.ThirdPartyAnalysisId = Conversion.ToInt(r.Cells["ThirdPartyAnalysisId"].Value);
						vd.ThirdPartyAnalysisNo = Conversion.ToString(r.Cells["ThirdPartyAnalysisNo"].Value);
						vd.ThirdPartyAnalysisSubId = Conversion.ToInt(r.Cells["ThirdPartyAnalysisSubId"].Value);
						vd.ThirdPartyAnalysisSubNo = Conversion.ToString(r.Cells["ThirdPartyAnalysisSubNo"].Value);
						vd.LoadingRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
						vd.InspectionRemarks = Conversion.ToString(r.Cells["InspectionRemarks"].Value);
						vd.ContainerRemarks = Conversion.ToString(r.Cells["ContainerRemarks"].Value);
						vd.ActionTypeId = 3;
						lstRemoveRecord.Add(vd);
						table.Rows[r.RowIndex].Delete();
						r.Delete();
						grd.UpdateData();
					}
					else
					{
						r.Delete();
					}
				}
				if (grd.CurrentColumn != null && grd.CurrentColumn.Key == "Edit")
				{
					grd_DoubleClick(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
			if (e.KeyCode != Keys.F1)
			{
				return;
			}
			int Id = 0;
			string Name = "";
			if (grd.CurrentColumn != null && (grd.CurrentColumn.Key == "WareHouseId" || grd.CurrentColumn.Key == "WareHouse"))
			{
				GrdPopUp obj5 = new GrdPopUp(dtWh, "Id", "WareHouseName");
				obj5.StartPosition = FormStartPosition.CenterScreen;
				obj5.ShowDialog();
				obj5.BringToFront();
				Id = Conversion.ToInt((object)obj5.ReturnId);
				Name = Conversion.ToString((object)obj5.ReturnName);
				r.Cells["WarehouseId"].Value = Id;
				r.Cells["Warehouse"].Value = Name;
			}
			if (grd.CurrentColumn != null && (grd.CurrentColumn.Key == "JobLotId" || grd.CurrentColumn.Key == "JobLot"))
			{
				GrdPopUp obj4 = new GrdPopUp(dtjob, "Id", "JobLotDescription");
				obj4.StartPosition = FormStartPosition.CenterScreen;
				obj4.ShowDialog();
				obj4.BringToFront();
				Id = Conversion.ToInt((object)obj4.ReturnId);
				Name = Conversion.ToString((object)obj4.ReturnName);
				r.Cells["JobLotId"].Value = Id;
				r.Cells["JobLot"].Value = Name;
			}
			if (grd.CurrentColumn != null && (grd.CurrentColumn.Key == "PackingTypeId" || grd.CurrentColumn.Key == "PackingType"))
			{
				GrdPopUp obj3 = new GrdPopUp(dtPackingType, "Id", "PackTypeDesc");
				obj3.StartPosition = FormStartPosition.CenterScreen;
				obj3.ShowDialog();
				obj3.BringToFront();
				Id = Conversion.ToInt((object)obj3.ReturnId);
				Name = Conversion.ToString((object)obj3.ReturnName);
				r.Cells["PackingTypeId"].Value = Id;
				r.Cells["PackingType"].Value = Name;
			}
			if (grd.CurrentColumn != null && (grd.CurrentColumn.Key == "ContainerId" || grd.CurrentColumn.Key == "ContainerNo"))
			{
				int InvoiceId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
				if (InvoiceId <= 0)
				{
					throw new Exception("InvoiceId not found");
				}
				DataTable dt2 = ExImInvoice.GetContainerNoFromShipingLineBookingDetailByInvoiceId(UserAccount.OrganizationId, UserAccount.CompanyId, InvoiceId, RecId);
				if (dt2.Rows.Count > 0)
				{
					dtCont.Clear();
					for (int i = 0; i < dt2.Rows.Count; i++)
					{
						dtCont.Rows.Add(dt2.Rows[i]["ContainerId"], dt2.Rows[i]["Container"]);
					}
					if (dtContFromReadById.Rows.Count > 0)
					{
						foreach (DataRow dr in dtContFromReadById.Rows)
						{
							int containerIdClone = Conversion.ToInt(dr["ContainerId"]);
							if (!dtCont.AsEnumerable().Any((DataRow contRow) => Conversion.ToInt(contRow["ContainerId"]) == containerIdClone))
							{
								dtCont.Rows.Add(dr["ContainerId"], dr["ContainerNo"]);
							}
						}
					}
					if (dtCont.Rows.Count > 0)
					{
						GrdPopUp obj2 = new GrdPopUp(dtCont, "ContainerId", "ContainerNo");
						obj2.StartPosition = FormStartPosition.CenterScreen;
						obj2.ShowDialog();
						obj2.BringToFront();
						Id = Conversion.ToInt((object)obj2.ReturnId);
						Name = Conversion.ToString((object)obj2.ReturnName);
						r.Cells["ContainerId"].Value = Id;
						r.Cells["ContainerNo"].Value = Name;
					}
				}
			}
			if (grd.CurrentColumn == null || (!(grd.CurrentColumn.Key == "InnerUOMId") && !(grd.CurrentColumn.Key == "InnerUOM") && !(grd.CurrentColumn.Key == "InnerUOMEquivalent")))
			{
				return;
			}
			int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
			if (ItemId > 0)
			{
				DataTable dt = CommonServices.GetUomScheduleByItemId(ItemId);
				if (dt == null || dt.Rows.Count <= 0)
				{
					throw new Exception("ItemId not found");
				}
				GrdPopUp obj = new GrdPopUp(dt, "Id", "UOMCode");
				obj.StartPosition = FormStartPosition.CenterScreen;
				obj.ShowDialog();
				obj.BringToFront();
				Id = Conversion.ToInt((object)obj.ReturnId);
				Name = Conversion.ToString((object)obj.ReturnName);
				r.Cells["InnerUOMId"].Value = Id;
				r.Cells["InnerUOM"].Value = Name;
				DataRow foundRow = dt.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == Id);
				decimal InnerUOMEquivalent = ((foundRow != null) ? Conversion.ToDecimal(foundRow["Equivalent"]) : 0m);
				r.Cells["InnerUOMEquivalent"].Value = InnerUOMEquivalent;
			}
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
			if (r == null || (int)r.RowType != 0 || grdhistory.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdhistory.CurrentColumn.Key == "Print")
				{
					if (formrights.DoHavePrintRights)
					{
						CommonServices.InvDeliveryOrderSlipWithSubReport(Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value), 84);
					}
				}
				else if (grdhistory.CurrentColumn.Key == "Print_396")
				{
					if (formrights.DoHavePrintRights)
					{
						CommonServices.ExportDeliveryOrderSlip396(Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value), 84);
					}
				}
				else if (grdhistory.CurrentColumn.Key == "GpPrint")
				{
					int InvoiceId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
					CrystalReportPrint_Helper.OutwardGatePassPrintByInvoicesIds_295(null, InvoiceId);
				}
				else if (grdhistory.CurrentColumn.Key == "DoPrintByInvoice")
				{
					CrystalReportPrint_Helper.DeliveryOrderPrintByInvoicesId(Conversion.ToInt(r.Cells["InvoiceId"].Value), 84);
				}
				else if (grdhistory.CurrentColumn.Key == "Edit")
				{
					if (formrights.DoHaveUpdateRights)
					{
						int RecId2 = 0;
						RecId2 = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
						ReadById(RecId2);
					}
				}
				else if (grdhistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value), 84);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return && formrights.DoHaveUpdateRights)
			{
				int RecId = 0;
				RecId = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
				ReadById(RecId);
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
		//IL_05af: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b6: Expected O, but got Unknown
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
		//IL_069d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a7: Expected O, but got Unknown
		//IL_06c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d3: Expected O, but got Unknown
		//IL_070b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0715: Expected O, but got Unknown
		//IL_08ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f9: Expected O, but got Unknown
		//IL_0905: Unknown result type (might be due to invalid IL or missing references)
		//IL_090f: Expected O, but got Unknown
		//IL_0968: Unknown result type (might be due to invalid IL or missing references)
		//IL_0972: Expected O, but got Unknown
		//IL_097e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0988: Expected O, but got Unknown
		//IL_09cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d5: Expected O, but got Unknown
		//IL_09e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09eb: Expected O, but got Unknown
		//IL_0a18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a22: Expected O, but got Unknown
		//IL_0a9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa6: Expected O, but got Unknown
		//IL_0aa7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab1: Expected O, but got Unknown
		//IL_0abd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac7: Expected O, but got Unknown
		//IL_0ad3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0add: Expected O, but got Unknown
		//IL_0af4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afe: Expected O, but got Unknown
		//IL_0aff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b09: Expected O, but got Unknown
		//IL_0b15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1f: Expected O, but got Unknown
		//IL_0b36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b40: Expected O, but got Unknown
		//IL_0b4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b56: Expected O, but got Unknown
		//IL_0cb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc1: Expected O, but got Unknown
		//IL_0cd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce2: Expected O, but got Unknown
		//IL_0d25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2f: Expected O, but got Unknown
		//IL_0d9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da8: Expected O, but got Unknown
		//IL_1475: Unknown result type (might be due to invalid IL or missing references)
		//IL_147f: Expected O, but got Unknown
		//IL_148c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1496: Expected O, but got Unknown
		//IL_17b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_17bb: Expected O, but got Unknown
		//IL_17c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_17d2: Expected O, but got Unknown
		//IL_aa10: Unknown result type (might be due to invalid IL or missing references)
		//IL_aa1a: Expected O, but got Unknown
		//IL_aa3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_aa48: Expected O, but got Unknown
		//IL_ac93: Unknown result type (might be due to invalid IL or missing references)
		//IL_ac9d: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdotheritems_DesignTimeLayout = new GridEXLayout();
		Appearance appearance153 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance166 = new Appearance();
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
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
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
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
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
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
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
		Appearance appearance35 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
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
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Export.ExportDeliveryOrderB));
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabDetail = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label19 = new System.Windows.Forms.Label();
		this.tabOtherItem = new System.Windows.Forms.TabPage();
		this.grdotheritems = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel19 = new System.Windows.Forms.Panel();
		this.btnoadd = new System.Windows.Forms.Button();
		this.btnoCancel = new System.Windows.Forms.Button();
		this.CmbOtherItem = new UltraCombo();
		this.btnoUpdate = new System.Windows.Forms.Button();
		this.label71 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.txtoRemarks = new System.Windows.Forms.TextBox();
		this.txtNetWeightOtherItem = new System.Windows.Forms.TextBox();
		this.label48 = new System.Windows.Forms.Label();
		this.txtWtPerQtyOtherItem = new System.Windows.Forms.TextBox();
		this.label46 = new System.Windows.Forms.Label();
		this.txtoQty = new System.Windows.Forms.TextBox();
		this.label41 = new System.Windows.Forms.Label();
		this.panel18 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.txtItemWeight = new System.Windows.Forms.TextBox();
		this.txtOtherRemarks = new System.Windows.Forms.TextBox();
		this.txtPackingWeight = new System.Windows.Forms.TextBox();
		this.txtOtherWeight = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.txtTotalGrossWeight = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.txtTotalAccessWtSet = new System.Windows.Forms.TextBox();
		this.txtOtherItemTotalWt = new System.Windows.Forms.TextBox();
		this.label44 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtInvoiceDate = new System.Windows.Forms.DateTimePicker();
		this.label11 = new System.Windows.Forms.Label();
		this.txtBalanceWeight = new System.Windows.Forms.TextBox();
		this.txtExportReturnWeight = new System.Windows.Forms.TextBox();
		this.txtrejectedWeight = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.txtInvoiceWeight = new System.Windows.Forms.TextBox();
		this.txtDoWeight = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label52 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.cmbtransporter = new UltraCombo();
		this.label50 = new System.Windows.Forms.Label();
		this.CmbBranch = new UltraCombo();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.label20 = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.lblBranchName = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.CmbLoadingPort = new UltraCombo();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.CmbInspectionNoPackListDetail = new UltraCombo();
		this.label144 = new System.Windows.Forms.Label();
		this.btnplus = new System.Windows.Forms.Button();
		this.label30 = new System.Windows.Forms.Label();
		this.txtEbTotalMaster = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.txtEbUnitMaster = new System.Windows.Forms.TextBox();
		this.CmbInspectionSubLot = new UltraCombo();
		this.label54 = new System.Windows.Forms.Label();
		this.cmbContainerNoDetail = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.txtAccessWtSet = new System.Windows.Forms.TextBox();
		this.txtEbUnitInner = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbCropYear = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.txtContainerRemarks = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.txtInspectionRemarks = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.txtEbTotalInner = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbJobLot = new UltraCombo();
		this.cmbInvoiceNo = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.CmbWareHouse = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbSupplierCustomer = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.CmbPackUomInner = new UltraCombo();
		this.CmbPackUomMaster = new UltraCombo();
		this.label32 = new System.Windows.Forms.Label();
		this.CmbPackingType = new UltraCombo();
		this.label36 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.CmbOrderNo = new UltraCombo();
		this.txtQtyInner = new System.Windows.Forms.TextBox();
		this.CmbItemName = new UltraCombo();
		this.txtQtyMaster = new System.Windows.Forms.TextBox();
		this.label31 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.txtremarksdetail = new System.Windows.Forms.TextBox();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.label47 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.txtBalnceMtonOfSelectedItem = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.labelEximForwardingForm = new System.Windows.Forms.LinkLabel();
		this.labelGatePassForm = new System.Windows.Forms.LinkLabel();
		this.RadNoneFormToOpenOnInsert = new System.Windows.Forms.RadioButton();
		this.RadGrnFormToOpenOnInsert = new System.Windows.Forms.RadioButton();
		this.RadWBFormToOpenOnInsert = new System.Windows.Forms.RadioButton();
		this.ChkPreview = new System.Windows.Forms.CheckBox();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.btnExportDoSlip396 = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.btnPurchaseOrderLoader = new System.Windows.Forms.ToolStripButton();
		this.btnCroForm = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdhistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label18 = new System.Windows.Forms.Label();
		this.grdDetail = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label13 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.label53 = new System.Windows.Forms.Label();
		this.CmbInvoiceNoHistory = new UltraCombo();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.label34 = new System.Windows.Forms.Label();
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
		this.panel5 = new System.Windows.Forms.Panel();
		this.label17 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel6.SuspendLayout();
		this.tabOtherItem.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdotheritems).BeginInit();
		this.panel10.SuspendLayout();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOtherItem).BeginInit();
		this.panel4.SuspendLayout();
		this.groupBox4.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbtransporter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBranch).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLoadingPort).BeginInit();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInspectionNoPackListDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInspectionSubLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbContainerNoDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbInvoiceNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierCustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUomInner).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUomMaster).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel8.SuspendLayout();
		this.panel7.SuspendLayout();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNoHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel5.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1281, 701);
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
		this.tabControl1.Size = new System.Drawing.Size(1281, 701);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.tabControl2);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(1273, 672);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.tabControl2.Controls.Add(this.tabDetail);
		this.tabControl2.Controls.Add(this.tabOtherItem);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(2, 346);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1269, 324);
		this.tabControl2.TabIndex = 5;
		this.tabDetail.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabDetail.Controls.Add(this.panel6);
		this.tabDetail.Location = new System.Drawing.Point(4, 25);
		this.tabDetail.Name = "tabDetail";
		this.tabDetail.Padding = new System.Windows.Forms.Padding(3);
		this.tabDetail.Size = new System.Drawing.Size(1261, 295);
		this.tabDetail.TabIndex = 0;
		this.tabDetail.Text = "Detail";
		this.tabDetail.UseVisualStyleBackColor = true;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		this.grd.ColumnAutoResize = true;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 28);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1255, 264);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label19);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1255, 25);
		this.panel6.TabIndex = 4;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label19.ForeColor = System.Drawing.Color.White;
		this.label19.Location = new System.Drawing.Point(6, 4);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(46, 17);
		this.label19.TabIndex = 0;
		this.label19.Text = "Detail";
		this.tabOtherItem.Controls.Add((System.Windows.Forms.Control)(object)this.grdotheritems);
		this.tabOtherItem.Controls.Add(this.panel10);
		this.tabOtherItem.Controls.Add(this.panel19);
		this.tabOtherItem.Controls.Add(this.panel18);
		this.tabOtherItem.Location = new System.Drawing.Point(4, 25);
		this.tabOtherItem.Name = "tabOtherItem";
		this.tabOtherItem.Padding = new System.Windows.Forms.Padding(3);
		this.tabOtherItem.Size = new System.Drawing.Size(1261, 295);
		this.tabOtherItem.TabIndex = 1;
		this.tabOtherItem.Text = "Other Item";
		this.tabOtherItem.UseVisualStyleBackColor = true;
		this.grdotheritems.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdotheritems).BackColor = System.Drawing.Color.White;
		grdotheritems_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdotheritems.DesignTimeLayout = grdotheritems_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdotheritems.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Location = new System.Drawing.Point(3, 99);
		((System.Windows.Forms.Control)(object)this.grdotheritems).Name = "grdotheritems";
		((ScrollableBase)this.grdotheritems).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdotheritems).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdotheritems).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Size = new System.Drawing.Size(1255, 193);
		((System.Windows.Forms.Control)(object)this.grdotheritems).TabIndex = 1;
		this.grdotheritems.TotalRow = (InheritableBoolean)1;
		this.grdotheritems.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdotheritems).VisualStyle = (VisualStyle)4;
		this.grdotheritems.CellUpdated += new ColumnActionEventHandler(grdotheritems_CellUpdated);
		this.grdotheritems.ColumnButtonClick += new ColumnActionEventHandler(grdotheritems_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdotheritems).DoubleClick += new System.EventHandler(grdotheritems_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdotheritems).KeyDown += new System.Windows.Forms.KeyEventHandler(GridExpense_KeyDown);
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.ctrlGrdBar3);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 76);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1255, 23);
		this.panel10.TabIndex = 5;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = null;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1208, 0);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdotheritems;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(44, 22);
		this.ctrlGrdBar3.TabIndex = 26;
		this.panel19.Controls.Add(this.btnoadd);
		this.panel19.Controls.Add(this.btnoCancel);
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOtherItem);
		this.panel19.Controls.Add(this.btnoUpdate);
		this.panel19.Controls.Add(this.label71);
		this.panel19.Controls.Add(this.label40);
		this.panel19.Controls.Add(this.txtoRemarks);
		this.panel19.Controls.Add(this.txtNetWeightOtherItem);
		this.panel19.Controls.Add(this.label48);
		this.panel19.Controls.Add(this.txtWtPerQtyOtherItem);
		this.panel19.Controls.Add(this.label46);
		this.panel19.Controls.Add(this.txtoQty);
		this.panel19.Controls.Add(this.label41);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel19.Location = new System.Drawing.Point(3, 24);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(1255, 52);
		this.panel19.TabIndex = 0;
		this.btnoadd.BackColor = System.Drawing.Color.Teal;
		this.btnoadd.FlatAppearance.BorderSize = 0;
		this.btnoadd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnoadd.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.btnoadd.ForeColor = System.Drawing.Color.White;
		this.btnoadd.Location = new System.Drawing.Point(1111, 24);
		this.btnoadd.Name = "btnoadd";
		this.btnoadd.Size = new System.Drawing.Size(36, 24);
		this.btnoadd.TabIndex = 6;
		this.btnoadd.Text = "+";
		this.btnoadd.UseVisualStyleBackColor = false;
		this.btnoadd.Click += new System.EventHandler(btnoadd_Click);
		this.btnoCancel.BackColor = System.Drawing.Color.Teal;
		this.btnoCancel.FlatAppearance.BorderSize = 0;
		this.btnoCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnoCancel.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.btnoCancel.ForeColor = System.Drawing.Color.White;
		this.btnoCancel.Location = new System.Drawing.Point(1180, 24);
		this.btnoCancel.Name = "btnoCancel";
		this.btnoCancel.Size = new System.Drawing.Size(70, 24);
		this.btnoCancel.TabIndex = 7;
		this.btnoCancel.Text = "Cancel";
		this.btnoCancel.UseVisualStyleBackColor = false;
		this.btnoCancel.Visible = false;
		this.btnoCancel.Click += new System.EventHandler(btnoCancel_Click);
		this.CmbOtherItem.AllowNull = (DefaultableBoolean)1;
		this.CmbOtherItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOtherItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOtherItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance156;
		((SpecialBoxBase)((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance157).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance157).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance158).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance161).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance162).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance163).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Location = new System.Drawing.Point(5, 24);
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Name = "CmbOtherItem";
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Size = new System.Drawing.Size(358, 25);
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).TabIndex = 0;
		this.btnoUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnoUpdate.FlatAppearance.BorderSize = 0;
		this.btnoUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnoUpdate.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.btnoUpdate.ForeColor = System.Drawing.Color.White;
		this.btnoUpdate.Location = new System.Drawing.Point(1111, 24);
		this.btnoUpdate.Name = "btnoUpdate";
		this.btnoUpdate.Size = new System.Drawing.Size(67, 24);
		this.btnoUpdate.TabIndex = 4;
		this.btnoUpdate.Text = "Update";
		this.btnoUpdate.UseVisualStyleBackColor = false;
		this.btnoUpdate.Visible = false;
		this.btnoUpdate.Click += new System.EventHandler(btnoUpdate_Click);
		this.label71.AutoSize = true;
		this.label71.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label71.Location = new System.Drawing.Point(8, 5);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(67, 15);
		this.label71.TabIndex = 504;
		this.label71.Text = "Item Name";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(676, 5);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(52, 15);
		this.label40.TabIndex = 508;
		this.label40.Text = "Remarks";
		this.txtoRemarks.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtoRemarks.Location = new System.Drawing.Point(675, 25);
		this.txtoRemarks.Name = "txtoRemarks";
		this.txtoRemarks.Size = new System.Drawing.Size(431, 22);
		this.txtoRemarks.TabIndex = 4;
		this.txtNetWeightOtherItem.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeightOtherItem.Location = new System.Drawing.Point(547, 25);
		this.txtNetWeightOtherItem.Name = "txtNetWeightOtherItem";
		this.txtNetWeightOtherItem.Size = new System.Drawing.Size(126, 22);
		this.txtNetWeightOtherItem.TabIndex = 3;
		this.txtNetWeightOtherItem.Tag = "NetWeight";
		this.txtNetWeightOtherItem.Text = "0";
		this.txtNetWeightOtherItem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNetWeightOtherItem.TextChanged += new System.EventHandler(txtNetWeightOtherItem_TextChanged);
		this.txtNetWeightOtherItem.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNetWeightOtherItem_KeyPress);
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(550, 5);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(68, 15);
		this.label48.TabIndex = 505;
		this.label48.Text = "Net Weight";
		this.txtWtPerQtyOtherItem.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWtPerQtyOtherItem.Location = new System.Drawing.Point(469, 25);
		this.txtWtPerQtyOtherItem.Name = "txtWtPerQtyOtherItem";
		this.txtWtPerQtyOtherItem.Size = new System.Drawing.Size(76, 22);
		this.txtWtPerQtyOtherItem.TabIndex = 2;
		this.txtWtPerQtyOtherItem.Tag = "WeightPerQty";
		this.txtWtPerQtyOtherItem.Text = "0";
		this.txtWtPerQtyOtherItem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtWtPerQtyOtherItem.TextChanged += new System.EventHandler(txtWtPerQtyOtherItem_TextChanged);
		this.txtWtPerQtyOtherItem.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtWtPerQtyOtherItem_KeyPress);
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(469, 5);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(73, 15);
		this.label46.TabIndex = 505;
		this.label46.Text = "Weight /Qty";
		this.txtoQty.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtoQty.Location = new System.Drawing.Point(366, 25);
		this.txtoQty.Name = "txtoQty";
		this.txtoQty.Size = new System.Drawing.Size(101, 22);
		this.txtoQty.TabIndex = 1;
		this.txtoQty.Text = "0";
		this.txtoQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtoQty.TextChanged += new System.EventHandler(txtoQty_TextChanged);
		this.txtoQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtoQty_KeyPress);
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(369, 5);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(26, 15);
		this.label41.TabIndex = 505;
		this.label41.Text = "Qty";
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(3, 3);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1255, 21);
		this.panel18.TabIndex = 3;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox4);
		this.panel4.Controls.Add(this.groupBox2);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.groupBox3);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 56);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1269, 290);
		this.panel4.TabIndex = 0;
		this.groupBox4.BackColor = System.Drawing.Color.WhiteSmoke;
		this.groupBox4.Controls.Add(this.txtItemWeight);
		this.groupBox4.Controls.Add(this.txtOtherRemarks);
		this.groupBox4.Controls.Add(this.txtPackingWeight);
		this.groupBox4.Controls.Add(this.txtOtherWeight);
		this.groupBox4.Controls.Add(this.label24);
		this.groupBox4.Controls.Add(this.txtTotalGrossWeight);
		this.groupBox4.Controls.Add(this.label25);
		this.groupBox4.Controls.Add(this.label43);
		this.groupBox4.Controls.Add(this.label26);
		this.groupBox4.Controls.Add(this.label27);
		this.groupBox4.Controls.Add(this.txtTotalAccessWtSet);
		this.groupBox4.Controls.Add(this.txtOtherItemTotalWt);
		this.groupBox4.Controls.Add(this.label44);
		this.groupBox4.Controls.Add(this.label28);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.groupBox4.Location = new System.Drawing.Point(822, 3);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(444, 115);
		this.groupBox4.TabIndex = 1;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Weight Information";
		this.txtItemWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemWeight.Enabled = false;
		this.txtItemWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemWeight.Location = new System.Drawing.Point(95, 12);
		this.txtItemWeight.Name = "txtItemWeight";
		this.txtItemWeight.Size = new System.Drawing.Size(131, 23);
		this.txtItemWeight.TabIndex = 0;
		this.txtItemWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOtherRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOtherRemarks.Location = new System.Drawing.Point(95, 87);
		this.txtOtherRemarks.Name = "txtOtherRemarks";
		this.txtOtherRemarks.Size = new System.Drawing.Size(346, 23);
		this.txtOtherRemarks.TabIndex = 3;
		this.txtPackingWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackingWeight.Enabled = false;
		this.txtPackingWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingWeight.Location = new System.Drawing.Point(95, 37);
		this.txtPackingWeight.Name = "txtPackingWeight";
		this.txtPackingWeight.Size = new System.Drawing.Size(131, 23);
		this.txtPackingWeight.TabIndex = 1;
		this.txtPackingWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOtherWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOtherWeight.Location = new System.Drawing.Point(95, 62);
		this.txtOtherWeight.Name = "txtOtherWeight";
		this.txtOtherWeight.Size = new System.Drawing.Size(131, 23);
		this.txtOtherWeight.TabIndex = 2;
		this.txtOtherWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOtherWeight.TextChanged += new System.EventHandler(txtOtherWeight_TextChanged);
		this.txtOtherWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOtherWeight_KeyPress);
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(4, 16);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(74, 15);
		this.label24.TabIndex = 30;
		this.label24.Text = "Item Weight";
		this.txtTotalGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalGrossWeight.Enabled = false;
		this.txtTotalGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalGrossWeight.Location = new System.Drawing.Point(313, 62);
		this.txtTotalGrossWeight.Name = "txtTotalGrossWeight";
		this.txtTotalGrossWeight.Size = new System.Drawing.Size(128, 23);
		this.txtTotalGrossWeight.TabIndex = 6;
		this.txtTotalGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(4, 91);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(85, 15);
		this.label25.TabIndex = 36;
		this.label25.Text = "Other Remarks";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label43.Location = new System.Drawing.Point(229, 16);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(82, 15);
		this.label43.TabIndex = 32;
		this.label43.Text = "Access Wt Set";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(4, 41);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(91, 15);
		this.label26.TabIndex = 32;
		this.label26.Text = "Packing Weight";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label27.Location = new System.Drawing.Point(229, 66);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(84, 15);
		this.label27.TabIndex = 36;
		this.label27.Text = "Total Gross Wt";
		this.txtTotalAccessWtSet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalAccessWtSet.Enabled = false;
		this.txtTotalAccessWtSet.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalAccessWtSet.Location = new System.Drawing.Point(313, 12);
		this.txtTotalAccessWtSet.Name = "txtTotalAccessWtSet";
		this.txtTotalAccessWtSet.Size = new System.Drawing.Size(128, 23);
		this.txtTotalAccessWtSet.TabIndex = 4;
		this.txtTotalAccessWtSet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOtherItemTotalWt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherItemTotalWt.Enabled = false;
		this.txtOtherItemTotalWt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOtherItemTotalWt.Location = new System.Drawing.Point(313, 37);
		this.txtOtherItemTotalWt.Name = "txtOtherItemTotalWt";
		this.txtOtherItemTotalWt.Size = new System.Drawing.Size(128, 23);
		this.txtOtherItemTotalWt.TabIndex = 5;
		this.txtOtherItemTotalWt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOtherItemTotalWt.TextChanged += new System.EventHandler(txtOtherWeight_TextChanged);
		this.txtOtherItemTotalWt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOtherWeight_KeyPress);
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label44.Location = new System.Drawing.Point(229, 41);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(84, 15);
		this.label44.TabIndex = 34;
		this.label44.Text = "Other Item Wt";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(4, 66);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(79, 15);
		this.label28.TabIndex = 34;
		this.label28.Text = "Other Weight";
		this.groupBox2.BackColor = System.Drawing.Color.WhiteSmoke;
		this.groupBox2.Controls.Add(this.txtInvoiceDate);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.txtBalanceWeight);
		this.groupBox2.Controls.Add(this.txtExportReturnWeight);
		this.groupBox2.Controls.Add(this.txtrejectedWeight);
		this.groupBox2.Controls.Add(this.label15);
		this.groupBox2.Controls.Add(this.label49);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.label21);
		this.groupBox2.Controls.Add(this.txtInvoiceWeight);
		this.groupBox2.Controls.Add(this.txtDoWeight);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(1048, 117);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(218, 170);
		this.groupBox2.TabIndex = 3;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Invoice Information";
		this.txtInvoiceDate.CustomFormat = "dd-MMM-yy";
		this.txtInvoiceDate.Enabled = false;
		this.txtInvoiceDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtInvoiceDate.Location = new System.Drawing.Point(97, 18);
		this.txtInvoiceDate.Name = "txtInvoiceDate";
		this.txtInvoiceDate.Size = new System.Drawing.Size(112, 23);
		this.txtInvoiceDate.TabIndex = 0;
		this.txtInvoiceDate.TabStop = false;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(5, 22);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(74, 15);
		this.label11.TabIndex = 30;
		this.label11.Text = "Invoice Date";
		this.txtBalanceWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBalanceWeight.Enabled = false;
		this.txtBalanceWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceWeight.Location = new System.Drawing.Point(97, 68);
		this.txtBalanceWeight.Name = "txtBalanceWeight";
		this.txtBalanceWeight.Size = new System.Drawing.Size(112, 23);
		this.txtBalanceWeight.TabIndex = 2;
		this.txtBalanceWeight.TabStop = false;
		this.txtExportReturnWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExportReturnWeight.Enabled = false;
		this.txtExportReturnWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExportReturnWeight.Location = new System.Drawing.Point(97, 143);
		this.txtExportReturnWeight.Name = "txtExportReturnWeight";
		this.txtExportReturnWeight.Size = new System.Drawing.Size(112, 23);
		this.txtExportReturnWeight.TabIndex = 4;
		this.txtExportReturnWeight.TabStop = false;
		this.txtrejectedWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtrejectedWeight.Enabled = false;
		this.txtrejectedWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtrejectedWeight.Location = new System.Drawing.Point(97, 118);
		this.txtrejectedWeight.Name = "txtrejectedWeight";
		this.txtrejectedWeight.Size = new System.Drawing.Size(112, 23);
		this.txtrejectedWeight.TabIndex = 4;
		this.txtrejectedWeight.TabStop = false;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(5, 72);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(90, 15);
		this.label15.TabIndex = 36;
		this.label15.Text = "Balance Weight";
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label49.Location = new System.Drawing.Point(5, 147);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(84, 15);
		this.label49.TabIndex = 36;
		this.label49.Text = "Return Weight";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(5, 47);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(88, 15);
		this.label12.TabIndex = 32;
		this.label12.Text = "Invoice Weight";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(5, 122);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(94, 15);
		this.label21.TabIndex = 36;
		this.label21.Text = "Rejected Weight";
		this.txtInvoiceWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceWeight.Enabled = false;
		this.txtInvoiceWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceWeight.Location = new System.Drawing.Point(97, 43);
		this.txtInvoiceWeight.Name = "txtInvoiceWeight";
		this.txtInvoiceWeight.Size = new System.Drawing.Size(112, 23);
		this.txtInvoiceWeight.TabIndex = 1;
		this.txtInvoiceWeight.TabStop = false;
		this.txtDoWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDoWeight.Enabled = false;
		this.txtDoWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDoWeight.Location = new System.Drawing.Point(97, 93);
		this.txtDoWeight.Name = "txtDoWeight";
		this.txtDoWeight.Size = new System.Drawing.Size(112, 23);
		this.txtDoWeight.TabIndex = 3;
		this.txtDoWeight.TabStop = false;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(5, 97);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(65, 15);
		this.label14.TabIndex = 34;
		this.label14.Text = "Do Weight";
		this.groupBox1.Controls.Add(this.label52);
		this.groupBox1.Controls.Add(this.label51);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbtransporter);
		this.groupBox1.Controls.Add(this.label50);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranch);
		this.groupBox1.Controls.Add(this.txtVehicleNo);
		this.groupBox1.Controls.Add(this.txtremarks);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Controls.Add(this.lblBranchName);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLoadingPort);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(814, 116);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main Information";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(522, 19);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(75, 15);
		this.label52.TabIndex = 66;
		this.label52.Text = "Loading Port";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(10, 47);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(57, 15);
		this.label51.TabIndex = 65;
		this.label51.Text = "Doc Date";
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
		((System.Windows.Forms.Control)(object)this.cmbtransporter).Location = new System.Drawing.Point(301, 13);
		((System.Windows.Forms.Control)(object)this.cmbtransporter).Name = "cmbtransporter";
		((System.Windows.Forms.Control)(object)this.cmbtransporter).Size = new System.Drawing.Size(217, 26);
		((System.Windows.Forms.Control)(object)this.cmbtransporter).TabIndex = 2;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(224, 19);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(67, 15);
		this.label50.TabIndex = 64;
		this.label50.Text = "Transporter";
		this.CmbBranch.AllowNull = (DefaultableBoolean)1;
		this.CmbBranch.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbBranch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance150;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbBranch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance165).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance168).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance169).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance169).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance169).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance170).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance171).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbBranch.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Location = new System.Drawing.Point(602, 41);
		((System.Windows.Forms.Control)(object)this.CmbBranch).Name = "CmbBranch";
		((System.Windows.Forms.Control)(object)this.CmbBranch).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranch).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Visible = false;
		this.txtVehicleNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleNo.Location = new System.Drawing.Point(301, 43);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.Size = new System.Drawing.Size(217, 23);
		this.txtVehicleNo.TabIndex = 3;
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(94, 69);
		this.txtremarks.Multiline = true;
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(708, 39);
		this.txtremarks.TabIndex = 6;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(224, 47);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(52, 15);
		this.label20.TabIndex = 9;
		this.label20.Text = "Vehicle#";
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(94, 43);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(124, 23);
		this.DocDate.TabIndex = 1;
		this.DocDate.TabStop = false;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Enabled = false;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(94, 15);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.Size = new System.Drawing.Size(124, 23);
		this.txtdocno.TabIndex = 0;
		this.lblBranchName.AutoSize = true;
		this.lblBranchName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBranchName.Location = new System.Drawing.Point(522, 47);
		this.lblBranchName.Name = "lblBranchName";
		this.lblBranchName.Size = new System.Drawing.Size(79, 15);
		this.lblBranchName.TabIndex = 3;
		this.lblBranchName.Text = "Branch Name";
		this.lblBranchName.Visible = false;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(10, 19);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 15);
		this.label5.TabIndex = 3;
		this.label5.Text = "Doc No";
		this.label33.BackColor = System.Drawing.Color.Transparent;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(10, 66);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(78, 45);
		this.label33.TabIndex = 8;
		this.label33.Text = "Remarks / Loading Instructions";
		this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.CmbLoadingPort.AllowNull = (DefaultableBoolean)1;
		this.CmbLoadingPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLoadingPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLoadingPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLoadingPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbLoadingPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbLoadingPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Location = new System.Drawing.Point(602, 13);
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Name = "CmbLoadingPort";
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).TabIndex = 4;
		this.groupBox3.BackColor = System.Drawing.Color.White;
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInspectionNoPackListDetail);
		this.groupBox3.Controls.Add(this.label144);
		this.groupBox3.Controls.Add(this.btnplus);
		this.groupBox3.Controls.Add(this.label30);
		this.groupBox3.Controls.Add(this.txtEbTotalMaster);
		this.groupBox3.Controls.Add(this.label29);
		this.groupBox3.Controls.Add(this.txtEbUnitMaster);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInspectionSubLot);
		this.groupBox3.Controls.Add(this.label54);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbContainerNoDetail);
		this.groupBox3.Controls.Add(this.label22);
		this.groupBox3.Controls.Add(this.txtAccessWtSet);
		this.groupBox3.Controls.Add(this.txtEbUnitInner);
		this.groupBox3.Controls.Add(this.label1);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYear);
		this.groupBox3.Controls.Add(this.label16);
		this.groupBox3.Controls.Add(this.txtContainerRemarks);
		this.groupBox3.Controls.Add(this.label10);
		this.groupBox3.Controls.Add(this.txtInspectionRemarks);
		this.groupBox3.Controls.Add(this.label9);
		this.groupBox3.Controls.Add(this.txtGrossWeight);
		this.groupBox3.Controls.Add(this.label7);
		this.groupBox3.Controls.Add(this.label42);
		this.groupBox3.Controls.Add(this.txtEbTotalInner);
		this.groupBox3.Controls.Add(this.label6);
		this.groupBox3.Controls.Add(this.label4);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbInvoiceNo);
		this.groupBox3.Controls.Add(this.label8);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWareHouse);
		this.groupBox3.Controls.Add(this.label3);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer);
		this.groupBox3.Controls.Add(this.label2);
		this.groupBox3.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUomInner);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUomMaster);
		this.groupBox3.Controls.Add(this.label32);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackingType);
		this.groupBox3.Controls.Add(this.label36);
		this.groupBox3.Controls.Add(this.btnUpdateDetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOrderNo);
		this.groupBox3.Controls.Add(this.txtQtyInner);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.groupBox3.Controls.Add(this.txtQtyMaster);
		this.groupBox3.Controls.Add(this.label31);
		this.groupBox3.Controls.Add(this.label38);
		this.groupBox3.Controls.Add(this.label35);
		this.groupBox3.Controls.Add(this.label39);
		this.groupBox3.Controls.Add(this.txtremarksdetail);
		this.groupBox3.Controls.Add(this.txtNetWeight);
		this.groupBox3.Controls.Add(this.label47);
		this.groupBox3.Controls.Add(this.label45);
		this.groupBox3.Controls.Add(this.txtBalnceMtonOfSelectedItem);
		this.groupBox3.Controls.Add(this.label37);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(3, 119);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1043, 168);
		this.groupBox3.TabIndex = 2;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Detail";
		this.CmbInspectionNoPackListDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbInspectionNoPackListDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInspectionNoPackListDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInspectionNoPackListDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance137;
		((SpecialBoxBase)((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance143).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance144).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance144).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance144).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInspectionNoPackListDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInspectionNoPackListDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInspectionNoPackListDetail).Location = new System.Drawing.Point(2, 137);
		((System.Windows.Forms.Control)(object)this.CmbInspectionNoPackListDetail).Name = "CmbInspectionNoPackListDetail";
		((System.Windows.Forms.Control)(object)this.CmbInspectionNoPackListDetail).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.CmbInspectionNoPackListDetail).TabIndex = 19;
		((System.Windows.Forms.Control)(object)this.CmbInspectionNoPackListDetail).Leave += new System.EventHandler(CmbInspectionNoPackListDetail_Leave);
		this.label144.AutoSize = true;
		this.label144.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label144.Location = new System.Drawing.Point(2, 120);
		this.label144.Name = "label144";
		this.label144.Size = new System.Drawing.Size(82, 15);
		this.label144.TabIndex = 617;
		this.label144.Text = "Inspection No";
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(976, 140);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(39, 21);
		this.btnplus.TabIndex = 25;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label30.Location = new System.Drawing.Point(356, 70);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(49, 15);
		this.label30.TabIndex = 53;
		this.label30.Text = "Eb Total";
		this.txtEbTotalMaster.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbTotalMaster.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEbTotalMaster.Location = new System.Drawing.Point(356, 93);
		this.txtEbTotalMaster.Name = "txtEbTotalMaster";
		this.txtEbTotalMaster.Size = new System.Drawing.Size(80, 23);
		this.txtEbTotalMaster.TabIndex = 11;
		this.txtEbTotalMaster.Tag = "TotalPackingWeightMaster";
		this.txtEbTotalMaster.TextChanged += new System.EventHandler(txtEbTotalMaster_TextChanged);
		this.txtEbTotalMaster.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEbTotalMaster_KeyPress);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(300, 70);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(45, 15);
		this.label29.TabIndex = 51;
		this.label29.Text = "Eb Unit";
		this.txtEbUnitMaster.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbUnitMaster.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEbUnitMaster.Location = new System.Drawing.Point(300, 93);
		this.txtEbUnitMaster.Name = "txtEbUnitMaster";
		this.txtEbUnitMaster.Size = new System.Drawing.Size(55, 23);
		this.txtEbUnitMaster.TabIndex = 10;
		this.txtEbUnitMaster.Tag = "PackingWeightMaster";
		this.txtEbUnitMaster.TextChanged += new System.EventHandler(txtEbUnitMaster_TextChanged);
		this.txtEbUnitMaster.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEbUnitMaster_KeyPress);
		this.CmbInspectionSubLot.AllowNull = (DefaultableBoolean)1;
		this.CmbInspectionSubLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInspectionSubLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInspectionSubLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInspectionSubLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbInspectionSubLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInspectionSubLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInspectionSubLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInspectionSubLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbInspectionSubLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInspectionSubLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbInspectionSubLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInspectionSubLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInspectionSubLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInspectionSubLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbInspectionSubLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInspectionSubLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInspectionSubLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInspectionSubLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbInspectionSubLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInspectionSubLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInspectionSubLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInspectionSubLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbInspectionSubLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInspectionSubLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInspectionSubLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbInspectionSubLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInspectionSubLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbInspectionSubLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInspectionSubLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInspectionSubLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInspectionSubLot).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbInspectionSubLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInspectionSubLot).Location = new System.Drawing.Point(123, 137);
		((System.Windows.Forms.Control)(object)this.CmbInspectionSubLot).Name = "CmbInspectionSubLot";
		((System.Windows.Forms.Control)(object)this.CmbInspectionSubLot).Size = new System.Drawing.Size(125, 26);
		((System.Windows.Forms.Control)(object)this.CmbInspectionSubLot).TabIndex = 20;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label54.Location = new System.Drawing.Point(123, 120);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(126, 15);
		this.label54.TabIndex = 49;
		this.label54.Text = "Inspection Sub Lot No";
		this.cmbContainerNoDetail.AllowNull = (DefaultableBoolean)1;
		this.cmbContainerNoDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbContainerNoDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbContainerNoDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbContainerNoDetail).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbContainerNoDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbContainerNoDetail).Location = new System.Drawing.Point(249, 137);
		((System.Windows.Forms.Control)(object)this.cmbContainerNoDetail).Name = "cmbContainerNoDetail";
		((System.Windows.Forms.Control)(object)this.cmbContainerNoDetail).Size = new System.Drawing.Size(140, 26);
		((System.Windows.Forms.Control)(object)this.cmbContainerNoDetail).TabIndex = 21;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(249, 120);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(77, 15);
		this.label22.TabIndex = 49;
		this.label22.Text = "Container No";
		this.txtAccessWtSet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAccessWtSet.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAccessWtSet.Location = new System.Drawing.Point(751, 93);
		this.txtAccessWtSet.Name = "txtAccessWtSet";
		this.txtAccessWtSet.Size = new System.Drawing.Size(80, 23);
		this.txtAccessWtSet.TabIndex = 16;
		this.txtAccessWtSet.Tag = "PackingWeight";
		this.txtAccessWtSet.TextChanged += new System.EventHandler(txtAccessWtSet_TextChanged);
		this.txtAccessWtSet.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAccessWtSet_KeyPress);
		this.txtEbUnitInner.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbUnitInner.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEbUnitInner.Location = new System.Drawing.Point(614, 93);
		this.txtEbUnitInner.Name = "txtEbUnitInner";
		this.txtEbUnitInner.Size = new System.Drawing.Size(55, 23);
		this.txtEbUnitInner.TabIndex = 14;
		this.txtEbUnitInner.Tag = "PackingWeight";
		this.txtEbUnitInner.TextChanged += new System.EventHandler(txtEbUnit_TextChanged);
		this.txtEbUnitInner.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPackingWt_KeyPress);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(670, 70);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(49, 15);
		this.label1.TabIndex = 47;
		this.label1.Text = "Eb Total";
		this.CmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance210).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance211).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance211).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance211).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance211;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance212;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance216).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance216).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance217).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance217;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance219).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance220;
		((AppearanceBase)appearance221).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance221).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance222).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance222).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance222).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance222;
		((AppearanceBase)appearance223).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance224).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Location = new System.Drawing.Point(758, 34);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Name = "CmbCropYear";
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabIndex = 5;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(759, 17);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(54, 15);
		this.label16.TabIndex = 45;
		this.label16.Text = "CropYear";
		this.txtContainerRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtContainerRemarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtContainerRemarks.Location = new System.Drawing.Point(390, 139);
		this.txtContainerRemarks.Name = "txtContainerRemarks";
		this.txtContainerRemarks.Size = new System.Drawing.Size(170, 23);
		this.txtContainerRemarks.TabIndex = 22;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(390, 120);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(106, 15);
		this.label10.TabIndex = 43;
		this.label10.Text = "Container Remarks";
		this.txtInspectionRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInspectionRemarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtInspectionRemarks.Location = new System.Drawing.Point(561, 139);
		this.txtInspectionRemarks.Name = "txtInspectionRemarks";
		this.txtInspectionRemarks.Size = new System.Drawing.Size(210, 23);
		this.txtInspectionRemarks.TabIndex = 23;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(561, 120);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(111, 15);
		this.label9.TabIndex = 41;
		this.label9.Text = "Inspection Remarks";
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Enabled = false;
		this.txtGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtGrossWeight.Location = new System.Drawing.Point(933, 93);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(107, 23);
		this.txtGrossWeight.TabIndex = 18;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(933, 70);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(78, 15);
		this.label7.TabIndex = 39;
		this.label7.Text = "Gross Weight";
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label42.Location = new System.Drawing.Point(751, 62);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(61, 30);
		this.label42.TabIndex = 37;
		this.label42.Text = "Access Wt Set (g)";
		this.txtEbTotalInner.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbTotalInner.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEbTotalInner.Location = new System.Drawing.Point(670, 93);
		this.txtEbTotalInner.Name = "txtEbTotalInner";
		this.txtEbTotalInner.Size = new System.Drawing.Size(80, 23);
		this.txtEbTotalInner.TabIndex = 15;
		this.txtEbTotalInner.Tag = "TotalPackingWeight";
		this.txtEbTotalInner.TextChanged += new System.EventHandler(txtEbTotal_TextChanged);
		this.txtEbTotalInner.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPackingWeight_KeyPress);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(614, 70);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(45, 15);
		this.label6.TabIndex = 37;
		this.label6.Text = "Eb Unit";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(2, 17);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(65, 15);
		this.label4.TabIndex = 27;
		this.label4.Text = "Invoice No";
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance226).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance227).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance227).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance227;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance228;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(607, 34);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(150, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 4;
		this.cmbInvoiceNo.AllowNull = (DefaultableBoolean)1;
		this.cmbInvoiceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbInvoiceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbInvoiceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance140;
		((SpecialBoxBase)((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance185).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance196;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance207).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance218).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance218).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance218).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance218).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbInvoiceNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Location = new System.Drawing.Point(2, 34);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Name = "cmbInvoiceNo";
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).TextChanged += new System.EventHandler(cmbInvoiceNo_TextChanged);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Leave += new System.EventHandler(cmbInvoiceNo_Leave);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(607, 17);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(43, 15);
		this.label8.TabIndex = 35;
		this.label8.Text = "JobLot";
		this.CmbWareHouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance68;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance123).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance125).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWareHouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Location = new System.Drawing.Point(446, 34);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Name = "CmbWareHouse";
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).TabIndex = 3;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(446, 17);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(69, 15);
		this.label3.TabIndex = 33;
		this.label3.Text = "WareHouse";
		this.CmbSupplierCustomer.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplierCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplierCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplierCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbSupplierCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Location = new System.Drawing.Point(123, 34);
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Name = "CmbSupplierCustomer";
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).TabIndex = 1;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(123, 17);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(93, 15);
		this.label2.TabIndex = 31;
		this.label2.Text = "Customer Name";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(976, 118);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(64, 21);
		this.btnCancelUpdateDetial.TabIndex = 27;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
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
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance133).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance213).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance213;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance214).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUomInner).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUomInner).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackUomInner.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUomInner).Location = new System.Drawing.Point(513, 91);
		((System.Windows.Forms.Control)(object)this.CmbPackUomInner).Name = "CmbPackUomInner";
		((System.Windows.Forms.Control)(object)this.CmbPackUomInner).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUomInner).TabIndex = 13;
		((System.Windows.Forms.Control)(object)this.CmbPackUomInner).Leave += new System.EventHandler(CmbPackUomInner_Leave);
		this.CmbPackUomMaster.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUomMaster.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUomMaster.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUomMaster.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Appearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance40;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance188;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance43).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUomMaster).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUomMaster).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackUomMaster.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUomMaster).Location = new System.Drawing.Point(199, 91);
		((System.Windows.Forms.Control)(object)this.CmbPackUomMaster).Name = "CmbPackUomMaster";
		((System.Windows.Forms.Control)(object)this.CmbPackUomMaster).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUomMaster).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.CmbPackUomMaster).Leave += new System.EventHandler(CmbPackUomMaster_Leave);
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label32.Location = new System.Drawing.Point(513, 70);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(66, 15);
		this.label32.TabIndex = 8;
		this.label32.Text = "Inner UOM";
		this.CmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Location = new System.Drawing.Point(2, 91);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Name = "CmbPackingType";
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).TabIndex = 7;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label36.Location = new System.Drawing.Point(201, 70);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(74, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "Master UOM";
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(976, 140);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(64, 21);
		this.btnUpdateDetail.TabIndex = 26;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.CmbOrderNo.AllowNull = (DefaultableBoolean)1;
		this.CmbOrderNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOrderNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOrderNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance198).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance198).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance198;
		((AppearanceBase)appearance199).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance199;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance200).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance200).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance201;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance204).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance204).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance205).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance205).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance205).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance205).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance206).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance208).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbOrderNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Location = new System.Drawing.Point(325, 34);
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Name = "CmbOrderNo";
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).TextChanged += new System.EventHandler(CmbOrderNo_TextChanged);
		this.txtQtyInner.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQtyInner.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtQtyInner.Location = new System.Drawing.Point(437, 93);
		this.txtQtyInner.Name = "txtQtyInner";
		this.txtQtyInner.Size = new System.Drawing.Size(75, 23);
		this.txtQtyInner.TabIndex = 12;
		this.txtQtyInner.TextChanged += new System.EventHandler(txtQtyInner_TextChanged);
		this.txtQtyInner.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQtyInner_KeyPress);
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance131;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance178).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(859, 34);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Leave += new System.EventHandler(CmbItemName_TextChanged);
		this.txtQtyMaster.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQtyMaster.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtQtyMaster.Location = new System.Drawing.Point(123, 93);
		this.txtQtyMaster.Name = "txtQtyMaster";
		this.txtQtyMaster.Size = new System.Drawing.Size(75, 23);
		this.txtQtyMaster.TabIndex = 8;
		this.txtQtyMaster.TextChanged += new System.EventHandler(txtQtyMaster_TextChanged);
		this.txtQtyMaster.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.txtQtyMaster.Leave += new System.EventHandler(txtQtyMaster_Leave);
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(437, 70);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(57, 15);
		this.label31.TabIndex = 13;
		this.label31.Text = "Inner Qty";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(123, 70);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(65, 15);
		this.label38.TabIndex = 13;
		this.label38.Text = "Master Qty";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label35.Location = new System.Drawing.Point(2, 70);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(74, 15);
		this.label35.TabIndex = 25;
		this.label35.Text = "PackingType";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label39.Location = new System.Drawing.Point(832, 70);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(68, 15);
		this.label39.TabIndex = 15;
		this.label39.Text = "Net Weight";
		this.txtremarksdetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarksdetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtremarksdetail.Location = new System.Drawing.Point(772, 139);
		this.txtremarksdetail.Name = "txtremarksdetail";
		this.txtremarksdetail.Size = new System.Drawing.Size(202, 23);
		this.txtremarksdetail.TabIndex = 24;
		this.txtNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeight.Enabled = false;
		this.txtNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNetWeight.Location = new System.Drawing.Point(832, 93);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.Size = new System.Drawing.Size(100, 23);
		this.txtNetWeight.TabIndex = 17;
		this.txtNetWeight.TextChanged += new System.EventHandler(txtNetWeight_TextChanged);
		this.txtNetWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtweight_KeyPress);
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label47.Location = new System.Drawing.Point(772, 120);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(65, 15);
		this.label47.TabIndex = 25;
		this.label47.Text = "Comments";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label45.Location = new System.Drawing.Point(325, 17);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(96, 15);
		this.label45.TabIndex = 23;
		this.label45.Text = "Sale Contract No";
		this.txtBalnceMtonOfSelectedItem.AutoSize = true;
		this.txtBalnceMtonOfSelectedItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.txtBalnceMtonOfSelectedItem.Location = new System.Drawing.Point(972, 16);
		this.txtBalnceMtonOfSelectedItem.Name = "txtBalnceMtonOfSelectedItem";
		this.txtBalnceMtonOfSelectedItem.Size = new System.Drawing.Size(55, 15);
		this.txtBalnceMtonOfSelectedItem.TabIndex = 9;
		this.txtBalnceMtonOfSelectedItem.Text = "Bal Mton";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label37.Location = new System.Drawing.Point(859, 17);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(67, 15);
		this.label37.TabIndex = 9;
		this.label37.Text = "Item Name";
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.labelEximForwardingForm);
		this.panel3.Controls.Add(this.labelGatePassForm);
		this.panel3.Controls.Add(this.RadNoneFormToOpenOnInsert);
		this.panel3.Controls.Add(this.RadGrnFormToOpenOnInsert);
		this.panel3.Controls.Add(this.RadWBFormToOpenOnInsert);
		this.panel3.Controls.Add(this.ChkPreview);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1269, 29);
		this.panel3.TabIndex = 1;
		this.labelEximForwardingForm.AutoSize = true;
		this.labelEximForwardingForm.BackColor = System.Drawing.Color.Teal;
		this.labelEximForwardingForm.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.labelEximForwardingForm.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.labelEximForwardingForm.Location = new System.Drawing.Point(539, 7);
		this.labelEximForwardingForm.Name = "labelEximForwardingForm";
		this.labelEximForwardingForm.Size = new System.Drawing.Size(121, 14);
		this.labelEximForwardingForm.TabIndex = 564;
		this.labelEximForwardingForm.TabStop = true;
		this.labelEximForwardingForm.Text = "Forwarding Form";
		this.labelEximForwardingForm.Click += new System.EventHandler(labelEximForwardingForm_Click);
		this.labelGatePassForm.AutoSize = true;
		this.labelGatePassForm.BackColor = System.Drawing.Color.Teal;
		this.labelGatePassForm.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.labelGatePassForm.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.labelGatePassForm.Location = new System.Drawing.Point(376, 7);
		this.labelGatePassForm.Name = "labelGatePassForm";
		this.labelGatePassForm.Size = new System.Drawing.Size(111, 14);
		this.labelGatePassForm.TabIndex = 565;
		this.labelGatePassForm.TabStop = true;
		this.labelGatePassForm.Text = "Gate Pass Form";
		this.labelGatePassForm.Click += new System.EventHandler(labelOutwardGatePassForm_Click);
		this.RadNoneFormToOpenOnInsert.AutoSize = true;
		this.RadNoneFormToOpenOnInsert.Checked = true;
		this.RadNoneFormToOpenOnInsert.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadNoneFormToOpenOnInsert.ForeColor = System.Drawing.Color.White;
		this.RadNoneFormToOpenOnInsert.Location = new System.Drawing.Point(289, 5);
		this.RadNoneFormToOpenOnInsert.Name = "RadNoneFormToOpenOnInsert";
		this.RadNoneFormToOpenOnInsert.Size = new System.Drawing.Size(59, 18);
		this.RadNoneFormToOpenOnInsert.TabIndex = 566;
		this.RadNoneFormToOpenOnInsert.TabStop = true;
		this.RadNoneFormToOpenOnInsert.Text = "None";
		this.RadNoneFormToOpenOnInsert.UseVisualStyleBackColor = true;
		this.RadGrnFormToOpenOnInsert.AutoSize = true;
		this.RadGrnFormToOpenOnInsert.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadGrnFormToOpenOnInsert.ForeColor = System.Drawing.Color.White;
		this.RadGrnFormToOpenOnInsert.Location = new System.Drawing.Point(519, 8);
		this.RadGrnFormToOpenOnInsert.Name = "RadGrnFormToOpenOnInsert";
		this.RadGrnFormToOpenOnInsert.Size = new System.Drawing.Size(14, 13);
		this.RadGrnFormToOpenOnInsert.TabIndex = 567;
		this.RadGrnFormToOpenOnInsert.UseVisualStyleBackColor = true;
		this.RadWBFormToOpenOnInsert.AutoSize = true;
		this.RadWBFormToOpenOnInsert.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadWBFormToOpenOnInsert.ForeColor = System.Drawing.Color.White;
		this.RadWBFormToOpenOnInsert.Location = new System.Drawing.Point(358, 8);
		this.RadWBFormToOpenOnInsert.Name = "RadWBFormToOpenOnInsert";
		this.RadWBFormToOpenOnInsert.Size = new System.Drawing.Size(14, 13);
		this.RadWBFormToOpenOnInsert.TabIndex = 568;
		this.RadWBFormToOpenOnInsert.UseVisualStyleBackColor = true;
		this.ChkPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPreview.AutoSize = true;
		this.ChkPreview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPreview.ForeColor = System.Drawing.Color.White;
		this.ChkPreview.Location = new System.Drawing.Point(1179, 5);
		this.ChkPreview.Name = "ChkPreview";
		this.ChkPreview.Size = new System.Drawing.Size(84, 20);
		this.ChkPreview.TabIndex = 563;
		this.ChkPreview.Text = "Preview";
		this.ChkPreview.UseVisualStyleBackColor = true;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(8, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(186, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "Delivery Order Export";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1269, 25);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.btnnew, this.btnRefresh, this.btnupdate, this.btnsave, this.btnprint, this.btnExportDoSlip396, this.btnattachment, this.btnShortCutKeys, this.btnPurchaseOrderLoader, this.btnCroForm });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1269, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
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
		this.btnprint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(92, 24);
		this.btnprint.Text = "262-Print";
		this.btnprint.Click += new System.EventHandler(Print_Click);
		this.btnExportDoSlip396.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnExportDoSlip396.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnExportDoSlip396.Name = "btnExportDoSlip396";
		this.btnExportDoSlip396.Size = new System.Drawing.Size(92, 24);
		this.btnExportDoSlip396.Text = "396-Print";
		this.btnExportDoSlip396.Click += new System.EventHandler(btnExportDoSlip396_Click);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(106, 24);
		this.btnShortCutKeys.Text = "ShortCut Keys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		this.btnPurchaseOrderLoader.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnPurchaseOrderLoader.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPurchaseOrderLoader.Name = "btnPurchaseOrderLoader";
		this.btnPurchaseOrderLoader.Size = new System.Drawing.Size(122, 24);
		this.btnPurchaseOrderLoader.Text = "Load Invoices";
		this.btnPurchaseOrderLoader.Click += new System.EventHandler(btnPurchaseOrderLoader_Click);
		this.btnCroForm.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnCroForm.Image = (System.Drawing.Image)resources.GetObject("btnCroForm.Image");
		this.btnCroForm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnCroForm.Name = "btnCroForm";
		this.btnCroForm.Size = new System.Drawing.Size(38, 24);
		this.btnCroForm.Text = "CRO";
		this.btnCroForm.Click += new System.EventHandler(btnCroForm_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1273, 672);
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
		this.grdhistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(2, 151);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1269, 259);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 1;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.SelectionChanged += new System.EventHandler(grdhistory_SelectionChanged);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdhistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdhistory_KeyDown);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label18);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel9.Location = new System.Drawing.Point(2, 410);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1269, 21);
		this.panel9.TabIndex = 51;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(6, 2);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(228, 17);
		this.label18.TabIndex = 23;
		this.label18.Text = "Detail of the selected above row";
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(2, 431);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1269, 239);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 2;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label13);
		this.panel8.Controls.Add(this.ctrlGrdBar2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(2, 129);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1269, 22);
		this.panel8.TabIndex = 48;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(5, 2);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(132, 18);
		this.label13.TabIndex = 22;
		this.label13.Text = "Filtered Records";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1223, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdhistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(43, 20);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.groupBox5);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 54);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1269, 75);
		this.panel7.TabIndex = 0;
		this.groupBox5.Controls.Add(this.label53);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory);
		this.groupBox5.Controls.Add(this.rdapproveddate);
		this.groupBox5.Controls.Add(this.btnShowHistory);
		this.groupBox5.Controls.Add(this.rdmodifydate);
		this.groupBox5.Controls.Add(this.label34);
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
		this.groupBox5.Location = new System.Drawing.Point(3, 2);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(923, 71);
		this.groupBox5.TabIndex = 0;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Filters";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(374, 47);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(46, 15);
		this.label53.TabIndex = 89;
		this.label53.Text = "Invoice";
		this.CmbInvoiceNoHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance182;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance184).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance184).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance190;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance191).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance192).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance192).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance192).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInvoiceNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Location = new System.Drawing.Point(434, 41);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Name = "CmbInvoiceNoHistory";
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Size = new System.Drawing.Size(238, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).TabIndex = 5;
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(816, 44);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 10;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(668, 41);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 6;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(816, 18);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 9;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(374, 20);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(58, 15);
		this.label34.TabIndex = 87;
		this.label34.Text = "Customer";
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(735, 18);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 7;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(735, 44);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 8;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(213, 47);
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
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
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
		this.ToDocNoHistory.Location = new System.Drawing.Point(294, 43);
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
		this.ToDateHistory.Location = new System.Drawing.Point(69, 43);
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
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label17);
		this.panel5.Controls.Add(this.toolStrip2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(2, 2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1269, 52);
		this.panel5.TabIndex = 46;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(3, 30);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(249, 19);
		this.label17.TabIndex = 22;
		this.label17.Text = "Delivery Order Export History";
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnResetHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1269, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
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
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1281, 701);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "ExportDeliveryOrderB";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ExportDeliveryOrderNew_FormClosing);
		base.Load += new System.EventHandler(PurchsaeOrder_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PurchsaeOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabDetail.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.tabOtherItem.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdotheritems).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		this.panel19.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOtherItem).EndInit();
		this.panel4.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbtransporter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBranch).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLoadingPort).EndInit();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInspectionNoPackListDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInspectionSubLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbContainerNoDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbInvoiceNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierCustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUomInner).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUomMaster).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNoHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
