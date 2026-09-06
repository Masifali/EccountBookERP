using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.CmTr.TransactionForm;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.CmTr.TransactionForm;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.CmTr.TransactionForms;

public class CommTradeLoadingDeliveryDirect : Form
{
	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtDetail = new DataTable();

	private DataTable dtPackingMaterialDetail = new DataTable();

	private DataTable dtSupplierPTDetail = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtAccountlst = new DataTable();

	private DataTable dtSupplierBindForPT = new DataTable();

	private DataTable dtMain = new DataTable();

	private DataTable dtgrid = new DataTable();

	private DataTable dtTerm = new DataTable();

	private DataTable dtAccountsForPMGrid = new DataTable();

	private DataTable dtPackingTypeForPMGrid = new DataTable();

	private DataTable dtitemForPMGrid = new DataTable();

	private DataTable dtSupCusRevgrd = new DataTable();

	private DataTable dtReveneuExpenseAc = new DataTable();

	private DataTable dtStockType = new DataTable();

	private DataTable dtWare = new DataTable();

	private int RecId;

	private int DefaultWareHouseId;

	private int DefaultPackingTypeId;

	private int updateDetailIndex = -1;

	private int DefaultDaysToLessFromHistoryFromDate;

	private Rightsobjects formright = new Rightsobjects();

	private bool ItemSearchWithNameOrCode;

	private DataTable lst = new DataTable();

	private DataTable dtVehicleSummary = new DataTable();

	private bool UpdateMode;

	private bool VehicleInfoEnable;

	private bool HasMultiLanguageFeature;

	private bool ExpenseGridsInTabOrinFront;

	private List<CommTradeLoadingDeliveryDetail> lstRemoveRecord = new List<CommTradeLoadingDeliveryDetail>();

	private IContainer components;

	private Panel panel1;

	private GroupBox groupBox1;

	private Label label6;

	private TextBox txtmanualno;

	private Label label5;

	private DateTimePicker txtdocdate;

	private TextBox txtdocno;

	private Label label33;

	private Label lbl;

	private TextBox txtBiltyNo;

	private Label label48;

	private TextBox txtvehicleno;

	private DateTimePicker txtbiltydate;

	private Label label2;

	internal UltraCombo cmbTransporter;

	private Label label29;

	private Label label3;

	private TextBox txtfrieght;

	private Label label4;

	private TextBox txtvehicleweight;

	private Label label1;

	private TextBox txtbiltyexpense;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	internal UltraCombo cmbitem;

	private Label labItemName;

	internal UltraCombo cmbuom;

	private Label label11;

	internal UltraCombo cmbsupplier;

	private Label label12;

	private Label label10;

	private TextBox txtItemSupQty;

	private Label label13;

	private TextBox txtItemSupGrossWt;

	private Label label14;

	private TextBox txtItemSupPmWt;

	private Label label18;

	private TextBox txtsupremarks;

	private Label label16;

	private TextBox txtItemSupNetWeight;

	private Label label15;

	private TextBox txtItemSupAdLsWt;

	internal UltraCombo cmbBroker;

	private Label label27;

	private GroupBox groupBox4;

	private Label label34;

	private TextBox txtcustremarks;

	private Label label35;

	private TextBox txtcustnetwt;

	private TextBox txtcustadlswt;

	private Label label37;

	private TextBox txtcustpckmtrlwt;

	private Label label38;

	private TextBox txtcustgrosswt;

	internal UltraCombo cmbcustomer;

	private Label label39;

	private Panel panel2;

	private Panel panel4;

	private Label label46;

	private GroupBox groupBox2;

	private Label label8;

	private Label label7;

	internal UltraCombo CmbPackingType;

	internal UltraCombo cmbcropYear;

	private Label label9;

	private Button btnAddDetail;

	private Button btnCancelDetail;

	private Button btnUpdateDetail;

	private GroupBox groupBox5;

	private Label label23;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton btnAttachment;

	private ToolStripButton btnPrint;

	private ToolStripButton btnDelete;

	private ToolStripButton btnshortcutkeys;

	internal GridEX grd;

	private Panel panel5;

	private ToolStrip miniToolStrip;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdHistoryDetail;

	private Panel panel3;

	private Label label19;

	internal GridEX grdHistory;

	private Panel panel6;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStrip toolStrip2;

	private Label label20;

	private RadioButton rdSearchByCode;

	private RadioButton rdSearchByName;

	private GroupBox groupBox6;

	internal GridEX grdPaymentTermSupplier;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label24;

	private TextBox txtCusRate;

	private Label label25;

	private TextBox txtCusQty;

	private Label label22;

	private TextBox txtSupRate;

	private TextBox txtVehcileQty;

	private Label label26;

	private Label label32;

	private TextBox txtSupNetWeight;

	private Label label31;

	private TextBox txtSupAdLsWt;

	private Label label30;

	private TextBox txtSupPmWt;

	private Label label28;

	private TextBox txtSupGrossWt;

	private Panel PanelOtherRevenue;

	private Panel panel8;

	private Label label21;

	private Panel PanelPmExpense;

	internal GridEX grdPackingMaterial;

	private Panel panel11;

	private Label label36;

	private CtrlGrdBar ctrlGrdBar4;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripSeparator toolStripSeparator5;

	private CheckBox chkPrint;

	private TextBox txtRemarksHeader;

	private ToolStripButton btnUrduPrint;

	private CheckBox ChkUrduPrintPreview;

	internal UltraCombo CmbMultiLanguage;

	private Label labelmultilang;

	private TabControl tabControl2;

	private TabPage tabDetail;

	private TabPage tabPM;

	private Panel PanelExxpenses;

	private TabPage tabRevenue;

	private RadioButton rdTemplate2;

	private RadioButton rdTemplate1;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripButton BtnLoadingExpenseForm;

	private Panel panel7;

	private GroupBox groupBox7;

	private RadioButton rdapproveddate;

	private Button btnShowHistory;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Label label149;

	private Label label150;

	private Label label151;

	private Label label152;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Panel panel12;

	private Label label17;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	public CommTradeLoadingDeliveryDirect(UserAccount userAccount)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void CommTradeLoadingDelivery_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1180, 780);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnPrint.Enabled = formright.DoHavePrintRights;
			btnUrduPrint.Enabled = formright.DoHavePrintRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
			btnDelete.Visible = formright.DoHaveCanDelete;
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
			ItemSearchWithNameOrCode = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("ItemSearchByCode"));
			if (ItemSearchWithNameOrCode)
			{
				rdSearchByCode.Checked = true;
			}
			else
			{
				rdSearchByName.Checked = true;
			}
			dtVehicleSummary.Columns.Add("VehcileNo", typeof(string));
			dtVehicleSummary.Columns.Add("VehicleGrossWeight", typeof(string));
			dtVehicleSummary.Columns.Add("VehicleBalanceWeight", typeof(string));
			dtStockType.Columns.Add("Id", typeof(int));
			dtStockType.Columns.Add("StockType", typeof(string));
			lst.Columns.Add("Id", typeof(int));
			lst.Columns.Add("ItemName", typeof(string));
			lst.Columns.Add("ItemCode", typeof(string));
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("SupplierName", typeof(string));
			dtSupplier.Columns.Add("GLCode", typeof(string));
			dtSupCusRevgrd.Columns.Add("SupplierCustomerId", typeof(int));
			dtSupCusRevgrd.Columns.Add("SupplierCustomerName", typeof(string));
			dtSupplierBindForPT.Columns.Add("SupplierId", typeof(int));
			dtSupplierBindForPT.Columns.Add("SupplierName", typeof(string));
			defaultConfiquration();
			GridDtFill();
			DocumentNoFill();
			Itemfill();
			PackingTypefill();
			PackUOMFill();
			Cropfill();
			suppliercustomer();
			TransportFill();
			dtDetail.Columns.Add("Id", typeof(int));
			dtDetail.Columns.Add("BrokerId", typeof(int));
			dtDetail.Columns.Add("BrokerName", typeof(string));
			dtDetail.Columns.Add("TransporterId", typeof(int));
			dtDetail.Columns.Add("TransporterName", typeof(string));
			dtDetail.Columns.Add("BiltyNo", typeof(string));
			dtDetail.Columns.Add("BiltyDate", typeof(DateTime));
			dtDetail.Columns.Add("BiltyExpense", typeof(decimal));
			dtDetail.Columns.Add("BiltyFreight", typeof(decimal));
			dtDetail.Columns.Add("VehicleNo", typeof(string));
			dtDetail.Columns.Add("VehicleQty", typeof(decimal));
			dtDetail.Columns.Add("VehicleWeight", typeof(decimal));
			dtDetail.Columns.Add("ItemId", typeof(int));
			dtDetail.Columns.Add("ItemCode", typeof(string));
			dtDetail.Columns.Add("ItemName", typeof(string));
			dtDetail.Columns.Add("CropYearId", typeof(int));
			dtDetail.Columns.Add("CropYear", typeof(string));
			dtDetail.Columns.Add("PackingTypeId", typeof(int));
			dtDetail.Columns.Add("PackingType", typeof(string));
			dtDetail.Columns.Add("UomId", typeof(int));
			dtDetail.Columns.Add("UOM", typeof(string));
			dtDetail.Columns.Add("ItemSupQty", typeof(decimal));
			dtDetail.Columns.Add("ItemSupWeight", typeof(decimal));
			dtDetail.Columns.Add("Qty", typeof(decimal));
			dtDetail.Columns.Add("SupplierId", typeof(int));
			dtDetail.Columns.Add("SupplierName", typeof(string));
			dtDetail.Columns.Add("SupGrossWeight", typeof(decimal));
			dtDetail.Columns.Add("SupPckMtrlWeight", typeof(decimal));
			dtDetail.Columns.Add("SupAdLsWt", typeof(decimal));
			dtDetail.Columns.Add("SupNetWeight", typeof(decimal));
			dtDetail.Columns.Add("SupRate", typeof(decimal));
			dtDetail.Columns.Add("SupRemarks", typeof(string));
			dtDetail.Columns.Add("CustomerId", typeof(int));
			dtDetail.Columns.Add("CustomerName", typeof(string));
			dtDetail.Columns.Add("CusGrossWeight", typeof(decimal));
			dtDetail.Columns.Add("CusPckMtrlWeight", typeof(decimal));
			dtDetail.Columns.Add("CusAdLsWt", typeof(decimal));
			dtDetail.Columns.Add("CusNetWeight", typeof(decimal));
			dtDetail.Columns.Add("CusRate", typeof(decimal));
			dtDetail.Columns.Add("CusRemarks", typeof(string));
			grd.DataSource = dtDetail;
			grd.RetrieveStructure();
			grdSettings();
			dtPackingMaterialDetail.Columns.Add("Id", typeof(int));
			dtPackingMaterialDetail.Columns.Add("WareHouseId", typeof(int));
			dtPackingMaterialDetail.Columns.Add("StockTypeId", typeof(int));
			dtPackingMaterialDetail.Columns.Add("PackingTypeId", typeof(int));
			dtPackingMaterialDetail.Columns.Add("ItemId", typeof(int));
			dtPackingMaterialDetail.Columns.Add("ItemName", typeof(string));
			dtPackingMaterialDetail.Columns.Add("ItemSize", typeof(string));
			dtPackingMaterialDetail.Columns.Add("Weight", typeof(double));
			dtPackingMaterialDetail.Columns.Add("Qty", typeof(double));
			dtPackingMaterialDetail.Columns.Add("Rate", typeof(double));
			dtPackingMaterialDetail.Columns.Add("Amount", typeof(double));
			dtPackingMaterialDetail.Columns.Add("CustomerAccountId", typeof(int));
			dtPackingMaterialDetail.Columns.Add("CustomerAccount", typeof(string));
			dtPackingMaterialDetail.Columns.Add("CreditAccountId", typeof(int));
			dtPackingMaterialDetail.Columns.Add("CreditAccount", typeof(string));
			grdPackingMaterial.DataSource = dtPackingMaterialDetail;
			grdPackingMaterial.RetrieveStructure();
			AddRowPackingmaterial();
			grdpackingsetting();
			dtSupplierPTDetail.Columns.Add("Id", typeof(int));
			dtSupplierPTDetail.Columns.Add("SupplierId", typeof(int));
			dtSupplierPTDetail.Columns.Add("PaymentTermId", typeof(int));
			dtSupplierPTDetail.Columns.Add("%ofTotal", typeof(decimal));
			dtSupplierPTDetail.Columns.Add("DueDays", typeof(int));
			dtSupplierPTDetail.Columns.Add("DueDate", typeof(DateTime));
			dtSupplierPTDetail.Columns.Add("Remarks");
			AddRowInPaymentTermSupplierGrid();
			grdPaymentTermSupplier.RetrieveStructure();
			grdSettingsPaymentTermSupplier();
			HasMultiLanguageFeature = CommonServices.GetERPFeatureById(7);
			if (HasMultiLanguageFeature)
			{
				labelmultilang.Visible = true;
				((Control)(object)CmbMultiLanguage).Visible = true;
				btnUrduPrint.Visible = true;
				MultiLanguagefill();
			}
			else
			{
				labelmultilang.Visible = false;
				((Control)(object)CmbMultiLanguage).Visible = false;
				btnUrduPrint.Visible = false;
			}
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			txtdocno.Focus();
			btnAddDetail.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			ChangeTemplate(ExpenseGridsInTabOrinFront);
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDateHistory.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Reset()
	{
		try
		{
			RecId = 0;
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			txtdocno.Text = string.Empty;
			txtdocdate.Value = DateTime.Now;
			txtmanualno.Text = string.Empty;
			txtRemarksHeader.Text = string.Empty;
			((Control)(object)cmbBroker).Text = string.Empty;
			txtdocno.Focus();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			lstRemoveRecord.Clear();
			DocumentNoFill();
			dtDetail.Clear();
			dtVehicleSummary.Clear();
			grd.DataSource = null;
			ResetDetail();
			EnableVehcileInfo();
			ResetVehcileInfo();
			dtSupplierPTDetail.Clear();
			dtPackingMaterialDetail.Clear();
			grdPaymentTermSupplier.DataSource = null;
			AddRowInPaymentTermSupplierGrid();
			grdPaymentTermSupplier.RetrieveStructure();
			grdSettingsPaymentTermSupplier();
			AddRowPackingmaterial();
			grdPackingMaterial.RetrieveStructure();
			grdpackingsetting();
			((Control)(object)cmbitem).Text = string.Empty;
			((Control)(object)cmbuom).Text = string.Empty;
			((Control)(object)cmbsupplier).Text = string.Empty;
			txtItemSupQty.Text = string.Empty;
			txtItemSupGrossWt.Text = string.Empty;
			txtItemSupAdLsWt.Text = string.Empty;
			txtItemSupPmWt.Text = string.Empty;
			txtItemSupNetWeight.Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EnableVehcileInfo()
	{
		try
		{
			txtvehicleno.Enabled = true;
			txtBiltyNo.Enabled = true;
			txtbiltydate.Enabled = true;
			((Control)(object)cmbTransporter).Enabled = true;
			txtfrieght.Enabled = true;
			txtbiltyexpense.Enabled = true;
			txtvehicleweight.Enabled = true;
			VehicleInfoEnable = true;
			cmbTransporter.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetVehcileInfo()
	{
		try
		{
			txtvehicleno.Text = string.Empty;
			txtBiltyNo.Text = string.Empty;
			txtbiltydate.Value = DateTime.Now;
			((Control)(object)cmbTransporter).Text = string.Empty;
			txtfrieght.Text = string.Empty;
			txtbiltyexpense.Text = string.Empty;
			txtVehcileQty.Text = string.Empty;
			txtvehicleweight.Text = string.Empty;
			cmbTransporter.Focus();
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
			updateDetailIndex = -1;
			txtCusQty.Text = string.Empty;
			((Control)(object)cmbcustomer).Text = string.Empty;
			txtcustgrosswt.Text = string.Empty;
			txtcustnetwt.Text = string.Empty;
			txtcustpckmtrlwt.Text = string.Empty;
			txtcustadlswt.Text = string.Empty;
			txtcustremarks.Text = string.Empty;
			txtCusRate.Text = string.Empty;
			btnAddDetail.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			txtSupGrossWt.Text = string.Empty;
			txtSupAdLsWt.Text = string.Empty;
			txtSupPmWt.Text = string.Empty;
			txtSupNetWeight.Text = string.Empty;
			txtsupremarks.Text = string.Empty;
			cmbcustomer.Focus();
			updateDetailIndex = -1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			Reset();
			ResetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRemarksHeader_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Text == "1" || base.ActiveControl.Text == "")
			{
				if (((Control)(object)cmbTransporter).Enabled)
				{
					cmbTransporter.Focus();
				}
				else if (((Control)(object)cmbitem).Enabled)
				{
					cmbitem.Focus();
				}
				else if (((Control)(object)cmbcustomer).Enabled)
				{
					cmbcustomer.Focus();
				}
			}
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
			Itemfill();
			PackingTypefill();
			Cropfill();
			suppliercustomer();
			TransportFill();
			GridDtFill();
			GridPackingMaterialCombobind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Itemfill()
	{
		try
		{
			int idits = Conversion.ToInt(cmbitem.Value);
			DataTable dt = CommonServices.ItemGetForComboServiceBind();
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					lst.Rows.Add(Conversion.ToInt(dt.Rows[i]["Id"]), Conversion.ToString(dt.Rows[i]["ItemName"]), Conversion.ToString(dt.Rows[i]["ItemCode"]));
				}
			}
			if (rdSearchByName.Checked)
			{
				DDL.BindDDL(lst, cmbitem, "Id", "ItemName", "Item", ZeroIndex: true);
			}
			else
			{
				DDL.BindDDL(lst, cmbitem, "Id", "ItemCode", "Item", ZeroIndex: true);
			}
			((UltraGridBase)cmbitem).DisplayLayout.Bands[0].Columns[1].Width = 300;
			if (idits > 0)
			{
				if (lst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == idits))
				{
					cmbitem.Value = idits;
				}
				else
				{
					((Control)(object)cmbitem).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingTypefill()
	{
		try
		{
			int idits = Conversion.ToInt(CmbPackingType.Value);
			DataTable lst = CommonServices.InvPackingTypeGetAllService();
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDLNew(lst, CmbPackingType, "Id", "PackTypeDesc", "Packing Type", true);
				((UltraGridBase)CmbPackingType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (idits > 0)
			{
				if (lst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == idits))
				{
					CmbPackingType.Value = idits;
				}
				else
				{
					((Control)(object)CmbPackingType).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Cropfill()
	{
		try
		{
			int idits = Conversion.ToInt(cmbcropYear.Value);
			DataTable lst = CommonServices.CropYearGetAllService();
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDLNew(lst, cmbcropYear, "Id", "CropYear", "Crop Year", true);
				((UltraGridBase)cmbcropYear).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (idits > 0)
			{
				if (lst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == idits))
				{
					cmbcropYear.Value = idits;
				}
				else
				{
					((Control)(object)cmbcropYear).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackUOMFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		try
		{
			int OrderId = Conversion.ToInt(cmbuom.Value);
			UOMSchedule val = new UOMSchedule
			{
				ItemId = Conversion.ToInt(cmbitem.Value),
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtsearchby = new DataTable();
			dtsearchby = UOMSchedule.SearchByObject(val);
			if (dtsearchby.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtsearchby, cmbuom, "Id", "Equivalent", "UOM", true);
				((UltraGridBase)cmbuom).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (OrderId > 0)
			{
				if (dtsearchby.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == OrderId))
				{
					cmbuom.Value = OrderId;
				}
				else
				{
					((Control)(object)cmbuom).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void suppliercustomer()
	{
		try
		{
			int suppId = Conversion.ToInt(cmbsupplier.Value);
			int custId = Conversion.ToInt(cmbcustomer.Value);
			int bcustId = Conversion.ToInt(cmbBroker.Value);
			DataTable dt = CommonServices.SupplierCustomerGetAllServiceBind();
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			dtSupplier.Clear();
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				dtSupplier.Rows.Add(Conversion.ToInt(dt.Rows[i]["Id"]), Conversion.ToString(dt.Rows[i]["CompanyName"]), Conversion.ToString(dt.Rows[i]["GLAccountCode"]));
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dtSupplier, cmbsupplier, "Id", "SupplierName", "Supplier", ZeroIndex: true);
				DDL.BindDDL(dtSupplier, cmbcustomer, "Id", "SupplierName", "Customer", ZeroIndex: true);
				DDL.BindDDL(dtSupplier, cmbBroker, "Id", "SupplierName", "Broker", ZeroIndex: true);
				((UltraGridBase)cmbsupplier).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)cmbcustomer).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)cmbBroker).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
			if (suppId > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == suppId))
				{
					cmbsupplier.Value = suppId;
				}
				else
				{
					((Control)(object)cmbsupplier).Text = string.Empty;
				}
			}
			if (custId > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == custId))
				{
					cmbcustomer.Value = custId;
				}
				else
				{
					((Control)(object)cmbcustomer).Text = string.Empty;
				}
			}
			if (bcustId > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == bcustId))
				{
					cmbBroker.Value = bcustId;
				}
				else
				{
					((Control)(object)cmbBroker).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TransportFill()
	{
		try
		{
			int Id = Conversion.ToInt(cmbTransporter.Value);
			dtAccountlst = CommonServices.CoaAllocationGetAllServiceBind();
			if (dtAccountlst.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("AccountTitle");
				for (int i = 0; i < dtAccountlst.Rows.Count; i++)
				{
					if (Conversion.ToInt(dtAccountlst.Rows[i]["AccountTypeId"]) != 2 && Conversion.ToInt(dtAccountlst.Rows[i]["AccountTypeId"]) != 15 && Conversion.ToInt(dtAccountlst.Rows[i]["AccountTypeId"]) != 11)
					{
						dt.Rows.Add(dtAccountlst.Rows[i]["Id"], dtAccountlst.Rows[i]["AccountTitle"]);
					}
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbTransporter, "Id", "AccountTitle", "Transporter", ZeroIndex: true);
				}
			}
			if (Id > 0)
			{
				if (dtAccountlst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					cmbTransporter.Value = Id;
				}
				else
				{
					((Control)(object)cmbTransporter).Text = string.Empty;
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
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			int code = CommTradeLoadingDelivery.GenerateCode(new CommTradeLoadingDelivery
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 1702,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			txtdocno.Text = code.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MultiLanguagefill()
	{
		try
		{
			int id = Conversion.ToInt(CmbMultiLanguage.Value);
			DataTable dt = CommonServices.MultiLanguagesGetAll();
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, CmbMultiLanguage, "Id", "LanguageDescription", "Language", true);
			((UltraGridBase)CmbMultiLanguage).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbMultiLanguage).Rows[1].Activate();
			if (id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					CmbMultiLanguage.Value = id;
				}
				else
				{
					((Control)(object)CmbMultiLanguage).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void rdSearchByName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			int OrderId = Conversion.ToInt(cmbitem.Value);
			if (lst.Rows.Count > 0)
			{
				int ItemId = 0;
				if (Conversion.ToInt(cmbitem.Value) > 0)
				{
					ItemId = Conversion.ToInt(cmbitem.Value);
				}
				if (rdSearchByName.Checked)
				{
					DDL.BindDDL(lst, cmbitem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(lst, cmbitem, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				cmbitem.Value = ItemId;
				cmbitem.Focus();
			}
			if (OrderId > 0)
			{
				if (lst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == OrderId))
				{
					cmbitem.Value = OrderId;
				}
				else
				{
					((Control)(object)cmbitem).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbitem_Leave(object sender, EventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		try
		{
			int OrderId = Conversion.ToInt(cmbuom.Value);
			UOMSchedule val = new UOMSchedule
			{
				ItemId = Conversion.ToInt(cmbitem.Value),
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtsearchby = new DataTable();
			dtsearchby = UOMSchedule.SearchByObject(val);
			if (dtsearchby.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtsearchby, cmbuom, "Id", "Equivalent", "UOM", true);
				((UltraGridBase)cmbuom).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (OrderId > 0)
			{
				if (dtsearchby.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == OrderId))
				{
					cmbuom.Value = OrderId;
				}
				else
				{
					((Control)(object)cmbuom).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDtFill()
	{
		try
		{
			dtAccountsForPMGrid.Clear();
			dtPackingTypeForPMGrid.Clear();
			dtitemForPMGrid.Clear();
			dtWare.Clear();
			dtReveneuExpenseAc.Clear();
			dtStockType.Clear();
			dtStockType.Rows.Add(1, "Company");
			dtStockType.Rows.Add(2, "Other");
			dtReveneuExpenseAc = CommTradeRevenueExpenseSchedule.GetLookUpsBylookTypeId(6, 0);
			dtAccountsForPMGrid = CommonServices.CoaAllocationAccountTitleByAccountTypeIds(null, "2,15");
			dtPackingTypeForPMGrid = InvPackingType.Getall();
			dtitemForPMGrid = CommonServices.GetItemByItemTypeId("14");
			dtWare = CommonServices.getActiveWareHouse();
			DetailSupCusForRevenueGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailSupCusForRevenueGrid()
	{
	}

	private void cmbcustomer_Leave(object sender, EventArgs e)
	{
		try
		{
			txtCusRate.Text = txtSupRate.Text;
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
				Conversion.ToInt((object)lstJob[0].ConfigKey.ToString());
			}
			List<ConfigrationsAllocation> lstCrop = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Default Crop Year").ToList();
			if (lstCrop.Count > 0)
			{
				Conversion.ToInt((object)lstCrop[0].ConfigKey.ToString());
			}
			List<ConfigrationsAllocation> lstCity = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "City Area").ToList();
			if (lstCity.Count > 0)
			{
				Conversion.ToInt((object)lstCity[0].ConfigKey.ToString());
			}
			List<ConfigrationsAllocation> cmbpaking = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Paking Type").ToList();
			if (cmbpaking.Count > 0)
			{
				DefaultPackingTypeId = Conversion.ToInt((object)cmbpaking[0].ConfigKey.ToString());
			}
			List<ConfigrationsAllocation> cmbowarehouse = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Warehouse").ToList();
			if (cmbowarehouse.Count > 0)
			{
				DefaultWareHouseId = Conversion.ToInt((object)cmbowarehouse[0].ConfigKey.ToString());
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdTemplate1_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (rdTemplate1.Checked)
			{
				ExpenseGridsInTabOrinFront = false;
			}
			else
			{
				ExpenseGridsInTabOrinFront = true;
			}
			ChangeTemplate(ExpenseGridsInTabOrinFront);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChangeTemplate(bool Type)
	{
		try
		{
			tabControl2.TabPages.Remove(tabPM);
			tabControl2.TabPages.Remove(tabRevenue);
			if (Type)
			{
				PanelExxpenses.Visible = false;
				tabControl2.TabPages.Add(tabPM);
				tabControl2.TabPages.Add(tabRevenue);
				PanelPmExpense.Dock = DockStyle.Fill;
				tabPM.Controls.Add(PanelPmExpense);
				PanelOtherRevenue.Dock = DockStyle.Fill;
				tabRevenue.Controls.Add(PanelOtherRevenue);
				tabControl2.Dock = DockStyle.Fill;
			}
			else
			{
				PanelExxpenses.Visible = true;
				PanelExxpenses.Controls.Add(PanelPmExpense);
				PanelPmExpense.Dock = DockStyle.Left;
				PanelExxpenses.Controls.Add(PanelOtherRevenue);
				PanelOtherRevenue.Dock = DockStyle.Fill;
				PanelOtherRevenue.BringToFront();
				tabControl2.Dock = DockStyle.Top;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridPackingMaterialCombobind()
	{
		try
		{
			bool IdExsitWare = false;
			bool IdExsitPT = false;
			if (dtPackingTypeForPMGrid.Rows.Count > 0)
			{
				grdPackingMaterial.RootTable.Columns["PackingTypeId"].ColumnType = (ColumnType)0;
				grdPackingMaterial.RootTable.Columns["PackingTypeId"].HasValueList = true;
				grdPackingMaterial.RootTable.Columns["PackingTypeId"].LimitToList = true;
				grdPackingMaterial.RootTable.Columns["PackingTypeId"].ValueList.PopulateValueList((IEnumerable)dtPackingTypeForPMGrid.DefaultView, "Id", "PackTypeDesc");
				grdPackingMaterial.RootTable.Columns["PackingTypeId"].Width = 250;
				IdExsitPT = dtPackingTypeForPMGrid.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == DefaultPackingTypeId);
			}
			if (dtitemForPMGrid.Rows.Count > 0)
			{
				grdPackingMaterial.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
				grdPackingMaterial.RootTable.Columns["ItemId"].HasValueList = true;
				grdPackingMaterial.RootTable.Columns["ItemId"].LimitToList = true;
				grdPackingMaterial.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtitemForPMGrid.DefaultView, "Id", "ItemName");
				grdPackingMaterial.RootTable.Columns["ItemId"].Width = 250;
			}
			if (dtAccountsForPMGrid.Rows.Count > 0)
			{
				grdPackingMaterial.RootTable.Columns["CreditAccountId"].ColumnType = (ColumnType)0;
				grdPackingMaterial.RootTable.Columns["CreditAccountId"].HasValueList = true;
				grdPackingMaterial.RootTable.Columns["CreditAccountId"].LimitToList = true;
				grdPackingMaterial.RootTable.Columns["CreditAccountId"].ValueList.PopulateValueList((IEnumerable)dtAccountsForPMGrid.DefaultView, "Id", "AccountTitle");
				grdPackingMaterial.RootTable.Columns["CreditAccountId"].Width = 250;
				grdPackingMaterial.RootTable.Columns["CustomerAccountId"].ColumnType = (ColumnType)0;
				grdPackingMaterial.RootTable.Columns["CustomerAccountId"].HasValueList = true;
				grdPackingMaterial.RootTable.Columns["CustomerAccountId"].LimitToList = true;
				grdPackingMaterial.RootTable.Columns["CustomerAccountId"].ValueList.PopulateValueList((IEnumerable)dtSupplier.DefaultView, "Id", "SupplierName");
				grdPackingMaterial.RootTable.Columns["CustomerAccountId"].Width = 250;
			}
			if (dtStockType.Rows.Count > 0)
			{
				grdPackingMaterial.RootTable.Columns["StockTypeId"].ColumnType = (ColumnType)0;
				grdPackingMaterial.RootTable.Columns["StockTypeId"].HasValueList = true;
				grdPackingMaterial.RootTable.Columns["StockTypeId"].LimitToList = true;
				grdPackingMaterial.RootTable.Columns["StockTypeId"].ValueList.PopulateValueList((IEnumerable)dtStockType.DefaultView, "Id", "StockType");
				grdPackingMaterial.RootTable.Columns["StockTypeId"].Width = 250;
			}
			if (dtWare.Rows.Count > 0)
			{
				grdPackingMaterial.RootTable.Columns["WareHouseId"].ColumnType = (ColumnType)0;
				grdPackingMaterial.RootTable.Columns["WareHouseId"].HasValueList = true;
				grdPackingMaterial.RootTable.Columns["WareHouseId"].LimitToList = true;
				grdPackingMaterial.RootTable.Columns["WareHouseId"].ValueList.PopulateValueList((IEnumerable)dtWare.DefaultView, "Id", "WareHouseName");
				grdPackingMaterial.RootTable.Columns["WareHouseId"].Width = 250;
				IdExsitWare = dtWare.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == DefaultWareHouseId);
			}
			if (IdExsitWare || IdExsitPT)
			{
				GridEXRow[] rows = grdPackingMaterial.GetRows();
				foreach (GridEXRow r in rows)
				{
					r.BeginEdit();
					if (IdExsitWare)
					{
						r.Cells["WareHouseId"].Value = DefaultWareHouseId;
					}
					if (IdExsitPT)
					{
						r.Cells["PackingTypeId"].Value = DefaultPackingTypeId;
					}
					r.EndEdit();
				}
			}
			grdPackingMaterial.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdPackingMaterial.RootTable.Columns["ItemId"].Caption = "Item Name";
			grdPackingMaterial.RootTable.Columns["CreditAccountId"].EditType = (EditType)4;
			grdPackingMaterial.RootTable.Columns["CreditAccountId"].Caption = "Cr Account";
			grdPackingMaterial.RootTable.Columns["CustomerAccountId"].EditType = (EditType)4;
			grdPackingMaterial.RootTable.Columns["CustomerAccountId"].Caption = "Customer Account";
			grdPackingMaterial.RootTable.Columns["PackingTypeId"].EditType = (EditType)4;
			grdPackingMaterial.RootTable.Columns["StockTypeId"].EditType = (EditType)4;
			grdPackingMaterial.RootTable.Columns["StockTypeId"].Caption = "StockType";
			grdPackingMaterial.RootTable.Columns["WareHouseId"].EditType = (EditType)4;
			grdPackingMaterial.RootTable.Columns["WareHouseId"].Caption = "WareHouse";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdpackingsetting()
	{
		try
		{
			GridPackingMaterialCombobind();
			grdPackingMaterial.RootTable.Columns["Id"].EditType = (EditType)0;
			grdPackingMaterial.RootTable.Columns["Id"].Visible = false;
			grdPackingMaterial.RootTable.Columns["ItemSize"].Visible = false;
			grdPackingMaterial.RootTable.Columns["Weight"].Visible = false;
			grdPackingMaterial.RootTable.Columns["CreditAccount"].Visible = false;
			grdPackingMaterial.RootTable.Columns["CustomerAccount"].Visible = false;
			grdPackingMaterial.RootTable.Columns["ItemName"].Visible = false;
			grdPackingMaterial.RootTable.Columns["StockTypeId"].Visible = false;
			grdPackingMaterial.RootTable.Columns["PackingTypeId"].Caption = "Packing Type";
			grdPackingMaterial.RootTable.Columns["PackingTypeId"].EditType = (EditType)4;
			grdPackingMaterial.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdPackingMaterial.RootTable.Columns["CreditAccount"].EditType = (EditType)0;
			grdPackingMaterial.RootTable.Columns["CustomerAccount"].EditType = (EditType)0;
			grdPackingMaterial.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterial.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdPackingMaterial.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdPackingMaterial.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdPackingMaterial.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterial.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdPackingMaterial.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdPackingMaterial.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grdPackingMaterial.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterial.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdPackingMaterial.RootTable.Columns["Weight"].FormatString = "#,##0.###";
			grdPackingMaterial.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
			grdPackingMaterial.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)3;
			grdPackingMaterial.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdPackingMaterial.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdPackingMaterial.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			CommonServices.GridAutoAdjustmentNew(grdPackingMaterial);
			grdPackingMaterial.RootTable.Columns.Add("Delete");
			grdPackingMaterial.RootTable.Columns["Delete"].Key = "Delete";
			grdPackingMaterial.RootTable.Columns["Delete"].Caption = "X";
			grdPackingMaterial.RootTable.Columns["Delete"].ButtonText = "X";
			grdPackingMaterial.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPackingMaterial.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdPackingMaterial.RootTable.Columns["Delete"].Width = 30;
			grdPackingMaterial.RootTable.Columns["Delete"].Position = 0;
			grdPackingMaterial.RootTable.Columns.Add("Add");
			grdPackingMaterial.RootTable.Columns["Add"].Key = "Add";
			grdPackingMaterial.RootTable.Columns["Add"].Caption = "Add";
			grdPackingMaterial.RootTable.Columns["Add"].ButtonText = "Add";
			grdPackingMaterial.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPackingMaterial.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdPackingMaterial.RootTable.Columns["Add"].Width = 40;
			grdPackingMaterial.RootTable.Columns["Add"].Position = 1;
			grdPackingMaterial.FrozenColumns = 2;
			ctrlGrdBar4_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowPackingmaterial()
	{
		try
		{
			dtPackingMaterialDetail.Rows.Add(0, 0, 1, 0, 0, "", "", 0, 0, 0, 0, 0, "", 0, "");
			grdPackingMaterial.DataSource = dtPackingMaterialDetail;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterial_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdPackingMaterial.UpdateData();
			GridEXRow item = grdPackingMaterial.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0)
			{
				decimal Rate = Conversion.ToDecimal((object)item.Cells["Rate"].Value.ToString());
				decimal Amount = Conversion.ToDecimal((object)Conversion.ToDecimal((object)item.Cells["Qty"].Value.ToString())) * Conversion.ToDecimal((object)Rate);
				item.Cells["Amount"].Value = Math.Round(Amount, 0, MidpointRounding.AwayFromZero);
			}
			if (e.Column.Key == "Amount" && item != null && (int)item.RowType == 0)
			{
				item.Cells["Rate"].Value = 0;
				item.Cells["Qty"].Value = 0;
			}
			grdPackingMaterial.UpdateData();
			CommonServices.GridAutoAdjustmentNew(grdPackingMaterial);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterial_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdPackingMaterial.CurrentRow;
				dtPackingMaterialDetail.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtPackingMaterialDetail.Rows.Count == 0)
				{
					AddRowPackingmaterial();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowPackingmaterial();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierCustomerBindForPT()
	{
		try
		{
			dtSupplierBindForPT.Clear();
			dtTerm.Clear();
			if (grd.RowCount > 0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow row in rows)
				{
					int SupplierId = Conversion.ToInt(row.Cells["SupplierId"].Value);
					string SupplierName = Conversion.ToString((object)row.Cells["SupplierName"].Text);
					if (!dtSupplierBindForPT.AsEnumerable().Any((DataRow rr) => rr.Field<int>("SupplierId") == SupplierId))
					{
						dtSupplierBindForPT.Rows.Add(SupplierId, SupplierName);
					}
				}
			}
			dtTerm = CommTradeRevenueExpenseSchedule.GetLookUpsBylookTypeId(5, 0);
			if (grdPaymentTermSupplier.RowCount > 0)
			{
				if (dtSupplierBindForPT.Rows.Count > 0)
				{
					grdPaymentTermSupplier.RootTable.Columns["SupplierId"].ColumnType = (ColumnType)0;
					grdPaymentTermSupplier.RootTable.Columns["SupplierId"].HasValueList = true;
					grdPaymentTermSupplier.RootTable.Columns["SupplierId"].LimitToList = true;
					grdPaymentTermSupplier.RootTable.Columns["SupplierId"].ValueList.PopulateValueList((IEnumerable)dtSupplierBindForPT.DefaultView, "SupplierId", "SupplierName");
					grdPaymentTermSupplier.RootTable.Columns["SupplierId"].Width = 120;
				}
				if (dtTerm.Rows.Count > 0)
				{
					grdPaymentTermSupplier.RootTable.Columns["PaymentTermId"].ColumnType = (ColumnType)0;
					grdPaymentTermSupplier.RootTable.Columns["PaymentTermId"].HasValueList = true;
					grdPaymentTermSupplier.RootTable.Columns["PaymentTermId"].LimitToList = true;
					grdPaymentTermSupplier.RootTable.Columns["PaymentTermId"].ValueList.PopulateValueList((IEnumerable)dtTerm.DefaultView, "Id", "LookUpName");
					grdPaymentTermSupplier.RootTable.Columns["PaymentTermId"].Width = 120;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInPaymentTermSupplierGrid()
	{
		try
		{
			dtSupplierPTDetail.Rows.Add(0, 0, 0, 0, 0, DateTime.Now, "");
			grdPaymentTermSupplier.DataSource = dtSupplierPTDetail;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsPaymentTermSupplier()
	{
		try
		{
			SupplierCustomerBindForPT();
			grdPaymentTermSupplier.RootTable.Columns["SupplierId"].EditType = (EditType)4;
			grdPaymentTermSupplier.RootTable.Columns["SupplierId"].Caption = "Supplier Name";
			grdPaymentTermSupplier.RootTable.Columns["PaymentTermId"].EditType = (EditType)4;
			grdPaymentTermSupplier.RootTable.Columns["PaymentTermId"].Caption = "Payment Term";
			grdPaymentTermSupplier.RootTable.Columns["Id"].Visible = false;
			grdPaymentTermSupplier.RootTable.Columns["Id"].EditType = (EditType)0;
			grdPaymentTermSupplier.RootTable.Columns["DueDate"].EditType = (EditType)2;
			grdPaymentTermSupplier.RootTable.Columns["DueDate"].FormatString = "dd-MMM-yyyy";
			grdPaymentTermSupplier.RootTable.Columns["%ofTotal"].AggregateFunction = (AggregateFunction)2;
			grdPaymentTermSupplier.RootTable.Columns["%ofTotal"].TextAlignment = (TextAlignment)3;
			grdPaymentTermSupplier.RootTable.Columns["%ofTotal"].FormatString = "#,##0.####";
			grdPaymentTermSupplier.RootTable.Columns["%ofTotal"].TotalFormatString = "#,##0.####";
			grdPaymentTermSupplier.RootTable.Columns["DueDays"].AggregateFunction = (AggregateFunction)2;
			grdPaymentTermSupplier.RootTable.Columns["DueDays"].TextAlignment = (TextAlignment)3;
			grdPaymentTermSupplier.RootTable.Columns["DueDays"].FormatString = "#,##0.###";
			grdPaymentTermSupplier.RootTable.Columns["DueDays"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grdPaymentTermSupplier);
			grdPaymentTermSupplier.RootTable.Columns.Add("Delete");
			grdPaymentTermSupplier.RootTable.Columns["Delete"].Key = "Delete";
			grdPaymentTermSupplier.RootTable.Columns["Delete"].Caption = "X";
			grdPaymentTermSupplier.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPaymentTermSupplier.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdPaymentTermSupplier.RootTable.Columns["Delete"].ButtonText = "X";
			grdPaymentTermSupplier.RootTable.Columns["Delete"].Width = 20;
			grdPaymentTermSupplier.RootTable.Columns.Add("Add");
			grdPaymentTermSupplier.RootTable.Columns["Add"].Key = "Add";
			grdPaymentTermSupplier.RootTable.Columns["Add"].Caption = "+";
			grdPaymentTermSupplier.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPaymentTermSupplier.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdPaymentTermSupplier.RootTable.Columns["Add"].ButtonText = "+";
			grdPaymentTermSupplier.RootTable.Columns["Add"].Width = 20;
			grdPaymentTermSupplier.RootTable.Columns["Add"].Position = 0;
			grdPaymentTermSupplier.RootTable.Columns["Delete"].Position = 1;
			grdPaymentTermSupplier.FrozenColumns = 2;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentTermSupplier_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPaymentTermSupplier.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (Conversion.ToInt(item.Cells["SupplierId"].Value) <= 0)
				{
					MessageBox.Show("Supplier Should be Selected...", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				if (e.Column.Key == "DueDays")
				{
					int days2 = Conversion.ToInt(item.Cells["DueDays"].Value);
					item.Cells["DueDate"].Value = txtdocdate.Value.AddDays(days2);
				}
				if (e.Column.Key == "DueDate")
				{
					DateTime date = Conversion.ToDateTime(item.Cells["DueDate"].Value);
					if (date.Date < txtdocdate.Value.Date)
					{
						item.Cells["DueDate"].Value = txtdocdate.Value;
						throw new Exception("Due Date Can't less Than DocDate");
					}
					int days = (date.Date - txtdocdate.Value.Date).Days;
					item.Cells["DueDays"].Value = days;
				}
			}
			grdPaymentTermSupplier.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentTermSupplier_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if (e.Column.Key == "%ofTotal" && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grdPaymentTermSupplier_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPaymentTermSupplier.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				dtSupplierPTDetail.Rows[r.RowIndex].Delete();
				r.Delete();
				if (dtSupplierPTDetail.Rows.Count == 0)
				{
					AddRowInPaymentTermSupplierGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInPaymentTermSupplierGrid();
			}
			grdPaymentTermSupplier.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentTermSupplier_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPaymentTermSupplier.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdPaymentTermSupplier.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdPaymentTermSupplier.CurrentColumn.Key == "Delete")
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					dtSupplierPTDetail.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
					if (dtSupplierPTDetail.Rows.Count == 0)
					{
						AddRowInPaymentTermSupplierGrid();
					}
				}
				if (grdPaymentTermSupplier.CurrentColumn.Key == "Add")
				{
					AddRowInPaymentTermSupplierGrid();
				}
				grdPaymentTermSupplier.UpdateData();
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					dtSupplierPTDetail.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
					if (dtSupplierPTDetail.Rows.Count == 0)
					{
						AddRowInPaymentTermSupplierGrid();
					}
				}
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInPaymentTermSupplierGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				int customerId = Conversion.ToInt(cmbcustomer.Value);
				int supplierId = Conversion.ToInt(cmbsupplier.Value);
				if (IsSupplierOrCustomerAlreadyAdded(customerId, supplierId))
				{
					dtDetail.Rows.Add(0, Conversion.ToInt(cmbBroker.Value), (Conversion.ToInt(cmbBroker.Value) > 0) ? Conversion.ToString((object)((Control)(object)cmbBroker).Text) : "", Conversion.ToInt(cmbTransporter.Value), (Conversion.ToInt(cmbTransporter.Value) > 0) ? Conversion.ToString((object)((Control)(object)cmbTransporter).Text) : "", Conversion.ToString((object)txtBiltyNo.Text), Conversion.ToDateTime((object)txtbiltydate.Value), Conversion.ToDecimal((object)txtbiltyexpense.Text), Conversion.ToDecimal((object)txtfrieght.Text), Conversion.ToString((object)txtvehicleno.Text), Conversion.ToDecimal((object)txtVehcileQty.Text), Conversion.ToDecimal((object)txtvehicleweight.Text), Conversion.ToInt(cmbitem.Value), Conversion.ToString((object)((UltraDropDownBase)cmbitem).SelectedRow.Cells["ItemCode"].Text), Conversion.ToString((object)((UltraDropDownBase)cmbitem).SelectedRow.Cells["ItemName"].Text), Conversion.ToInt(cmbcropYear.Value), Conversion.ToString((object)((Control)(object)cmbcropYear).Text), Conversion.ToInt(CmbPackingType.Value), Conversion.ToString((object)((Control)(object)CmbPackingType).Text), Conversion.ToInt(cmbuom.Value), Conversion.ToDecimal((object)((Control)(object)cmbuom).Text), Conversion.ToDecimal((object)txtItemSupQty.Text.Trim()), Conversion.ToDecimal((object)txtItemSupGrossWt.Text.Trim()), Conversion.ToDecimal((object)txtCusQty.Text.Trim()), Conversion.ToInt(cmbsupplier.Value), Conversion.ToString((object)((Control)(object)cmbsupplier).Text), Conversion.ToDecimal((object)txtSupGrossWt.Text.Trim()), Conversion.ToDecimal((object)txtSupPmWt.Text.Trim()), Conversion.ToDecimal((object)txtSupAdLsWt.Text.Trim()), Conversion.ToDecimal((object)txtSupNetWeight.Text.Trim()), Conversion.ToDecimal((object)txtSupRate.Text.Trim()), Conversion.ToString((object)txtsupremarks.Text.Trim()), Conversion.ToInt(cmbcustomer.Value), Conversion.ToString((object)((Control)(object)cmbcustomer).Text), Conversion.ToDecimal((object)txtcustgrosswt.Text.Trim()), Conversion.ToDecimal((object)txtcustpckmtrlwt.Text.Trim()), Conversion.ToDecimal((object)txtcustadlswt.Text.Trim()), Conversion.ToDecimal((object)txtcustnetwt.Text.Trim()), Conversion.ToDecimal((object)txtCusRate.Text.Trim()), Conversion.ToString((object)txtcustremarks.Text.Trim()));
					grd.DataSource = dtDetail;
					grd.RetrieveStructure();
					UpdateVehicleInfoInGrid();
					UpdateItemSupplierGrossWtQty();
					grdSettings();
					ResetDetail();
					EnableDisableVehicleInfo();
					SupplierCustomerBindForPT();
					cmbcustomer.Focus();
				}
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
			int customerId = Conversion.ToInt(cmbcustomer.Value);
			int supplierId = Conversion.ToInt(cmbsupplier.Value);
			if (IsSupplierOrCustomerAlreadyAdded(customerId, supplierId))
			{
				dtDetail.Rows[updateDetailIndex]["BrokerId"] = Conversion.ToInt(cmbBroker.Value);
				if (Conversion.ToInt(cmbBroker.Value) > 0)
				{
					dtDetail.Rows[updateDetailIndex]["BrokerName"] = Conversion.ToString((object)((Control)(object)cmbBroker).Text);
				}
				else
				{
					dtDetail.Rows[updateDetailIndex]["BrokerName"] = "";
				}
				dtDetail.Rows[updateDetailIndex]["TransporterId"] = Conversion.ToInt(cmbTransporter.Value);
				if (Conversion.ToInt(cmbTransporter.Value) > 0)
				{
					dtDetail.Rows[updateDetailIndex]["TransporterName"] = Conversion.ToString((object)((Control)(object)cmbTransporter).Text);
				}
				else
				{
					dtDetail.Rows[updateDetailIndex]["TransporterName"] = "";
				}
				dtDetail.Rows[updateDetailIndex]["BiltyNo"] = Conversion.ToString((object)txtBiltyNo.Text);
				dtDetail.Rows[updateDetailIndex]["BiltyDate"] = Conversion.ToDateTime((object)txtbiltydate.Text);
				dtDetail.Rows[updateDetailIndex]["BiltyExpense"] = Conversion.ToDecimal((object)txtbiltyexpense.Text);
				dtDetail.Rows[updateDetailIndex]["BiltyFreight"] = Conversion.ToDecimal((object)txtfrieght.Text);
				dtDetail.Rows[updateDetailIndex]["VehicleNo"] = Conversion.ToString((object)txtvehicleno.Text);
				dtDetail.Rows[updateDetailIndex]["VehicleQty"] = Conversion.ToDecimal((object)txtVehcileQty.Text);
				dtDetail.Rows[updateDetailIndex]["VehicleWeight"] = Conversion.ToDecimal((object)txtvehicleweight.Text);
				dtDetail.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(cmbitem.Value);
				dtDetail.Rows[updateDetailIndex]["ItemName"] = ((UltraDropDownBase)cmbitem).SelectedRow.Cells["ItemName"].Value;
				dtDetail.Rows[updateDetailIndex]["ItemCode"] = ((UltraDropDownBase)cmbitem).SelectedRow.Cells["ItemCode"].Value;
				dtDetail.Rows[updateDetailIndex]["CropYearId"] = Conversion.ToInt(cmbcropYear.Value);
				dtDetail.Rows[updateDetailIndex]["CropYear"] = Conversion.ToString((object)((Control)(object)cmbcropYear).Text);
				dtDetail.Rows[updateDetailIndex]["PackingTypeId"] = Conversion.ToInt(CmbPackingType.Value);
				dtDetail.Rows[updateDetailIndex]["PackingType"] = Conversion.ToString((object)((Control)(object)CmbPackingType).Text);
				dtDetail.Rows[updateDetailIndex]["UomId"] = Conversion.ToInt(cmbuom.Value);
				dtDetail.Rows[updateDetailIndex]["Uom"] = Conversion.ToString((object)((Control)(object)cmbuom).Text);
				dtDetail.Rows[updateDetailIndex]["ItemSupQty"] = Conversion.ToDecimal((object)txtItemSupQty.Text);
				dtDetail.Rows[updateDetailIndex]["ItemSupWeight"] = Conversion.ToDecimal((object)txtItemSupGrossWt.Text);
				dtDetail.Rows[updateDetailIndex]["Qty"] = Conversion.ToDecimal((object)txtCusQty.Text);
				dtDetail.Rows[updateDetailIndex]["SupplierId"] = Conversion.ToInt(cmbsupplier.Value);
				dtDetail.Rows[updateDetailIndex]["SupplierName"] = Conversion.ToString((object)((Control)(object)cmbsupplier).Text);
				dtDetail.Rows[updateDetailIndex]["SupGrossWeight"] = Conversion.ToDecimal((object)txtSupGrossWt.Text);
				dtDetail.Rows[updateDetailIndex]["SupPckMtrlWeight"] = Conversion.ToDecimal((object)txtSupPmWt.Text);
				dtDetail.Rows[updateDetailIndex]["SupAdLsWt"] = Conversion.ToDecimal((object)txtSupAdLsWt.Text);
				dtDetail.Rows[updateDetailIndex]["SupNetWeight"] = Conversion.ToDecimal((object)txtSupNetWeight.Text);
				dtDetail.Rows[updateDetailIndex]["SupRate"] = Conversion.ToDecimal((object)txtSupRate.Text);
				dtDetail.Rows[updateDetailIndex]["SupRemarks"] = Conversion.ToString((object)txtsupremarks.Text);
				dtDetail.Rows[updateDetailIndex]["CustomerId"] = Conversion.ToInt(cmbcustomer.Value);
				dtDetail.Rows[updateDetailIndex]["CustomerName"] = Conversion.ToString((object)((Control)(object)cmbcustomer).Text);
				dtDetail.Rows[updateDetailIndex]["CusGrossWeight"] = Conversion.ToDecimal((object)txtcustgrosswt.Text);
				dtDetail.Rows[updateDetailIndex]["CusPckMtrlWeight"] = Conversion.ToDecimal((object)txtcustpckmtrlwt.Text);
				dtDetail.Rows[updateDetailIndex]["CusAdLsWt"] = Conversion.ToDecimal((object)txtcustadlswt.Text);
				dtDetail.Rows[updateDetailIndex]["CusNetWeight"] = Conversion.ToDecimal((object)txtcustnetwt.Text);
				dtDetail.Rows[updateDetailIndex]["CusRate"] = Conversion.ToDecimal((object)txtCusRate.Text);
				dtDetail.Rows[updateDetailIndex]["CusRemarks"] = Conversion.ToString((object)txtcustremarks.Text);
				grd.DataSource = dtDetail;
				UpdateVehicleInfoInGrid();
				UpdateItemSupplierGrossWtQty();
				btnAddDetail.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelDetail.Visible = false;
				ResetDetail();
				EnableDisableVehicleInfo();
				SupplierCustomerBindForPT();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["BrokerId"].Visible = false;
			grd.RootTable.Columns["TransporterId"].Visible = false;
			grd.RootTable.Columns["CropYearId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["PackingTypeId"].Visible = false;
			grd.RootTable.Columns["UomId"].Visible = false;
			grd.RootTable.Columns["SupplierId"].Visible = false;
			grd.RootTable.Columns["CustomerId"].Visible = false;
			grd.RootTable.Columns["CustomerId"].Visible = false;
			grd.RootTable.Columns["CusGrossWeight"].Caption = "Gross Weight";
			grd.RootTable.Columns["CusAdLsWt"].Caption = "Adls Weight";
			grd.RootTable.Columns["CusPckMtrlWeight"].Caption = "PM Weight";
			grd.RootTable.Columns["CusNetWeight"].Caption = "Net Weight";
			grd.RootTable.Columns["CusRate"].Caption = "Rate";
			grd.RootTable.Columns["SupGrossWeight"].Caption = "Gross Weight";
			grd.RootTable.Columns["SupAdLsWt"].Caption = "Adls Weight";
			grd.RootTable.Columns["SupPckMtrlWeight"].Caption = "PM Weight";
			grd.RootTable.Columns["SupNetWeight"].Caption = "Net Weight";
			grd.RootTable.Columns["SupRate"].Caption = "Rate";
			grd.RootTable.Columns["ItemSupQty"].Caption = "Item Uom Sup Qty";
			grd.RootTable.Columns["ItemSupWeight"].Caption = "Item Uom Sup Weight";
			grd.RootTable.Columns["BiltyExpense"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BiltyExpense"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["BiltyExpense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["BiltyFreight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BiltyFreight"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["BiltyFreight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["VehicleWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["VehicleWeight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["VehicleWeight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["VehicleQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["VehicleQty"].FormatString = "#,##0.###";
			grd.RootTable.Columns["VehicleQty"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["ItemSupQty"].AggregateFunction = (AggregateFunction)3;
			grd.RootTable.Columns["ItemSupQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemSupQty"].FormatString = "#,##0.###";
			grd.RootTable.Columns["ItemSupQty"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["ItemSupWeight"].AggregateFunction = (AggregateFunction)3;
			grd.RootTable.Columns["ItemSupWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemSupWeight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["ItemSupWeight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["SupGrossWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["SupGrossWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["SupGrossWeight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["SupGrossWeight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["SupPckMtrlWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["SupPckMtrlWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["SupPckMtrlWeight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["SupPckMtrlWeight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["SupAdLsWt"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["SupAdLsWt"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["SupAdLsWt"].FormatString = "#,##0.###";
			grd.RootTable.Columns["SupAdLsWt"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["SupNetWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["SupNetWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["SupNetWeight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["SupNetWeight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["SupRate"].AggregateFunction = (AggregateFunction)3;
			grd.RootTable.Columns["SupRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["SupRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["SupRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["CusGrossWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["CusGrossWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["CusGrossWeight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["CusGrossWeight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["CusPckMtrlWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["CusPckMtrlWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["CusPckMtrlWeight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["CusPckMtrlWeight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["CusAdLsWt"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["CusAdLsWt"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["CusAdLsWt"].FormatString = "#,##0.###";
			grd.RootTable.Columns["CusAdLsWt"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["CusNetWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["CusNetWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["CusNetWeight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["CusNetWeight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["CusRate"].AggregateFunction = (AggregateFunction)3;
			grd.RootTable.Columns["CusRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["CusRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["CusRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns.Add("Edit");
			grd.RootTable.Columns["Edit"].Key = "Edit";
			grd.RootTable.Columns["Edit"].Caption = "Edit";
			grd.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Edit"].ButtonText = "Edit";
			grd.RootTable.Columns["Edit"].Width = 50;
			grd.RootTable.Columns["Edit"].Position = 0;
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 50;
			grd.RootTable.Columns["Delete"].Position = 1;
			grd.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelDetail_Click(object sender, EventArgs e)
	{
		try
		{
			ResetDetail();
			EnableDisableVehicleInfo();
			if (VehicleInfoEnable)
			{
				EnableVehcileInfo();
				ResetVehcileInfo();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && grd.CurrentColumn != null)
			{
				List<DataRow> source = (from r in dtDetail.AsEnumerable()
					where r.Field<int>("ItemId") == Conversion.ToInt(item.Cells["ItemId"].Value) && r.Field<int>("SupplierId") == Conversion.ToInt(item.Cells["SupplierId"].Value)
					select r).ToList();
				decimal GridTotalSupPckMtrlWeight = source.Sum((DataRow r) => r.Field<decimal>("SupPckMtrlWeight"));
				decimal GridTotalSupAdLsWt = source.Sum((DataRow r) => r.Field<decimal>("SupAdLsWt"));
				updateDetailIndex = item.RowIndex;
				cmbBroker.Value = Conversion.ToString(item.Cells["BrokerId"].Value);
				cmbTransporter.Value = Conversion.ToString(item.Cells["TransporterId"].Value);
				txtBiltyNo.Text = Conversion.ToString(item.Cells["BiltyNo"].Value);
				txtbiltydate.Value = Conversion.ToDateTime(item.Cells["BiltyDate"].Value);
				txtbiltyexpense.Text = Conversion.ToDecimal(item.Cells["BiltyExpense"].Value).ToString("##,#.###");
				txtfrieght.Text = Conversion.ToDecimal(item.Cells["BiltyFreight"].Value).ToString("##,#.###");
				txtvehicleno.Text = Conversion.ToString(item.Cells["VehicleNo"].Value);
				txtVehcileQty.Text = Conversion.ToDecimal(item.Cells["VehicleQty"].Value).ToString("##,#.###");
				txtvehicleweight.Text = Conversion.ToDecimal(item.Cells["VehicleWeight"].Value).ToString("##,#.###");
				cmbitem.Value = Conversion.ToString(item.Cells["ItemId"].Value);
				cmbitem_Leave(null, null);
				cmbcropYear.Value = item.Cells["CropYearId"].Value.ToString();
				CmbPackingType.Value = item.Cells["PackingTypeId"].Value.ToString();
				cmbuom.Value = item.Cells["UomId"].Value.ToString();
				txtItemSupQty.Text = Conversion.ToDecimal(item.Cells["ItemSupQty"].Value).ToString("##,#.###");
				txtItemSupGrossWt.Text = Conversion.ToDecimal(item.Cells["ItemSupWeight"].Value).ToString("##,#.###");
				txtItemSupPmWt.Text = GridTotalSupPckMtrlWeight.ToString("##,#.###");
				txtItemSupAdLsWt.Text = GridTotalSupAdLsWt.ToString("##,#.###");
				cmbsupplier.Value = item.Cells["SupplierId"].Value.ToString();
				txtSupGrossWt.Text = Conversion.ToDecimal(item.Cells["SupGrossWeight"].Value).ToString("##,#.###");
				txtSupPmWt.Text = Conversion.ToDecimal(item.Cells["SupPckMtrlWeight"].Value).ToString("##,#.###");
				txtSupAdLsWt.Text = Conversion.ToDecimal(item.Cells["SupAdLsWt"].Value).ToString("##,#.###");
				txtSupNetWeight.Text = Conversion.ToDecimal(item.Cells["SupNetWeight"].Value).ToString("##,#.###");
				txtSupRate.Text = Conversion.ToDecimal(item.Cells["SupRate"].Value).ToString("##,#.###");
				txtsupremarks.Text = item.Cells["SupRemarks"].Value.ToString();
				txtCusQty.Text = Conversion.ToDecimal(item.Cells["Qty"].Value).ToString("##,#.###");
				cmbcustomer.Value = Conversion.ToInt(item.Cells["CustomerId"].Value);
				txtcustgrosswt.Text = Conversion.ToDecimal(item.Cells["CusGrossWeight"].Value).ToString("##,#.###");
				txtcustpckmtrlwt.Text = Conversion.ToDecimal(item.Cells["CusPckMtrlWeight"].Value).ToString("##,#.###");
				txtcustadlswt.Text = Conversion.ToDecimal(item.Cells["CusAdLsWt"].Value).ToString("##,#.###");
				txtcustnetwt.Text = Conversion.ToDecimal(item.Cells["CusNetWeight"].Value).ToString("##,#.###");
				txtCusRate.Text = Conversion.ToDecimal(item.Cells["CusRate"].Value).ToString("##,#.###");
				txtcustremarks.Text = item.Cells["CusRemarks"].Value.ToString();
				EnableDisableVehicleInfo();
				VehicleInfoFillOnRowRemove();
				btnAddDetail.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelDetail.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Edit")
			{
				grd_DoubleClick(null, null);
			}
			if (e.Column.Key == "Delete")
			{
				if (updateDetailIndex > -1)
				{
					MessageBox.Show("Please Reset Detail Box Data If You want to Delete Any row from the Grid....");
					return;
				}
				GridEXRow r = grd.CurrentRow;
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					CommTradeLoadingDeliveryDetail vd = new CommTradeLoadingDeliveryDetail();
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd.BrokerId = Conversion.ToInt(r.Cells["BrokerId"].Value);
					vd.DocDateDt = Conversion.ToDateTime((object)txtdocdate.Value);
					vd.TransporterId = Conversion.ToInt(r.Cells["TransporterId"].Value);
					vd.BiltyNo = Conversion.ToString(r.Cells["BiltyNo"].Value);
					vd.BiltyDate = Conversion.ToDateTime(r.Cells["BiltyDate"].Value);
					vd.BiltyExpense = Conversion.ToDecimal(r.Cells["BiltyExpense"].Value);
					vd.VehicleNo = Conversion.ToString(r.Cells["VehicleNo"].Value);
					vd.BiltyFreight = Conversion.ToDecimal(r.Cells["BiltyFreight"].Value);
					vd.VehicleQty = Conversion.ToDecimal(r.Cells["VehicleQty"].Value);
					vd.VehicleGrossWeight = Conversion.ToDecimal(r.Cells["VehicleWeight"].Value);
					vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					vd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
					vd.PackingTypeId = Conversion.ToInt((object)r.Cells["PackingTypeId"].Text);
					vd.UomId = Conversion.ToInt((object)r.Cells["UomId"].Text);
					vd.ItemSupQty = Conversion.ToDecimal(r.Cells["ItemSupQty"].Value);
					vd.ItemSupWeight = Conversion.ToDecimal(r.Cells["ItemSupWeight"].Value);
					vd.SupplierId = Conversion.ToInt(r.Cells["SupplierId"].Value);
					vd.SupGrossWeight = Conversion.ToDecimal(r.Cells["SupGrossWeight"].Value);
					vd.SupPckMtrlWeight = Conversion.ToDecimal(r.Cells["SupPckMtrlWeight"].Value);
					vd.SupAdLsWt = Conversion.ToDecimal(r.Cells["SupAdLsWt"].Value);
					vd.SupNetWeight = Conversion.ToDecimal(r.Cells["SupNetWeight"].Value);
					vd.SupRate = Conversion.ToDecimal(r.Cells["SupRate"].Value);
					vd.SupRemarks = Conversion.ToString(r.Cells["SupRemarks"].Value);
					vd.Qty = Conversion.ToDecimal(r.Cells["Qty"].Value);
					vd.CustomerId = Conversion.ToInt(r.Cells["CustomerId"].Value);
					vd.CusGrossWeight = Conversion.ToDecimal(r.Cells["CusGrossWeight"].Value);
					vd.CusPckMtrlWeight = Conversion.ToDecimal(r.Cells["CusPckMtrlWeight"].Value);
					vd.CusAdLsWt = Conversion.ToDecimal(r.Cells["CusAdLsWt"].Value);
					vd.CusNetWeight = Conversion.ToDecimal(r.Cells["CusNetWeight"].Value);
					vd.CusRate = Conversion.ToDecimal(r.Cells["CusRate"].Value);
					vd.CusRemarks = Conversion.ToString(r.Cells["CusRemarks"].Value);
					vd.ActionTypeId = 3;
					lstRemoveRecord.Add(vd);
					dtDetail.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					grd.UpdateData();
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				SupplierCustomerBindForPT();
			}
			EnableDisableVehicleInfo();
			VehicleInfoFillOnRowRemove();
			DetailSupCusForRevenueGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateVehicleInfoInGrid()
	{
		try
		{
			if (grd.RowCount <= 0)
			{
				return;
			}
			string VehicleNo = Conversion.ToString((object)txtvehicleno.Text);
			string BiltyNo = Conversion.ToString((object)txtBiltyNo.Text);
			int TransporterId = Conversion.ToInt(cmbTransporter.Value);
			string Transporter = Conversion.ToString((object)((Control)(object)cmbTransporter).Text);
			DateTime BiltyDate = Conversion.ToDateTime((object)txtbiltydate.Value);
			decimal VehicleQty = Conversion.ToDecimal((object)txtVehcileQty.Text);
			decimal VehicleWeight = Conversion.ToDecimal((object)txtvehicleweight.Text);
			decimal BiltyFreight = Conversion.ToDecimal((object)txtfrieght.Text);
			decimal BiltyExpense = Conversion.ToDecimal((object)txtbiltyexpense.Text);
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToString(r.Cells["VehicleNo"].Value) == VehicleNo)
				{
					r.BeginEdit();
					r.Cells["BiltyNo"].Value = BiltyNo;
					r.Cells["TransporterId"].Value = TransporterId;
					r.Cells["TransporterName"].Value = Transporter;
					r.Cells["BiltyDate"].Value = BiltyDate;
					r.Cells["VehicleQty"].Value = VehicleQty;
					r.Cells["VehicleWeight"].Value = VehicleWeight;
					r.Cells["BiltyFreight"].Value = BiltyFreight;
					r.Cells["BiltyExpense"].Value = BiltyExpense;
					r.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateItemSupplierGrossWtQty()
	{
		try
		{
			if (grd.RowCount <= 0)
			{
				return;
			}
			int ItemId = Conversion.ToInt(cmbitem.Value);
			int PackUomId = Conversion.ToInt(cmbuom.Value);
			int SupplierId = Conversion.ToInt(cmbsupplier.Value);
			decimal SupRate = Conversion.ToDecimal((object)txtSupRate.Text);
			decimal ItemQty = Conversion.ToDecimal((object)txtItemSupQty.Text);
			decimal ItemWeight = Conversion.ToDecimal((object)txtItemSupGrossWt.Text);
			string VehicleNo = Conversion.ToString((object)txtvehicleno.Text);
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt(r.Cells["ItemId"].Value) == ItemId && Conversion.ToInt(r.Cells["UomId"].Value) == PackUomId && Conversion.ToInt(r.Cells["SupplierId"].Value) == SupplierId && Conversion.ToDecimal(r.Cells["SupRate"].Value) == SupRate && Conversion.ToString(r.Cells["VehicleNo"].Value) == VehicleNo)
				{
					r.BeginEdit();
					r.Cells["ItemSupQty"].Value = ItemQty;
					r.Cells["ItemSupWeight"].Value = ItemWeight;
					r.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool IsSupplierOrCustomerAlreadyAdded(int customerId, int supplierId)
	{
		try
		{
			if (grd.GetRows().Count() != 0)
			{
				bool isCustomerExist = dtDetail.AsEnumerable().Any((DataRow r) => r.Field<int>("SupplierId") == customerId);
				if (dtDetail.AsEnumerable().Any((DataRow r) => r.Field<int>("CustomerId") == supplierId))
				{
					MessageBox.Show("Selected Supplier Account Already Added As A Customer in Grid, So It Cannot Be Added As A Supplier...");
					cmbsupplier.Focus();
					return false;
				}
				if (isCustomerExist)
				{
					MessageBox.Show("Selected Customer Account Already Added As A Supplier in Grid, So It Cannot Be Added As A Customer...");
					cmbcustomer.Focus();
					return false;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return true;
	}

	private void EnableDisableVehicleInfo()
	{
		try
		{
			if (Conversion.ToInt((object)grd.GetRows().Count()) > 0)
			{
				decimal GridTotalSupGrossWtOfCurVehicle = default(decimal);
				string CurrentVehicleNo = Conversion.ToString((object)txtvehicleno.Text);
				decimal CurrentVehicleGrossWeight = Conversion.ToDecimal((object)txtvehicleweight.Text);
				if (updateDetailIndex > -1)
				{
					GridTotalSupGrossWtOfCurVehicle = (from item in dtDetail.AsEnumerable().Select((DataRow row, int index) => new
						{
							Row = row,
							Index = index
						})
						where item.Row.Field<string>("VehicleNo") == CurrentVehicleNo && item.Index != updateDetailIndex
						select item).Sum(item => item.Row.Field<decimal>("SupGrossWeight"));
				}
				else
				{
					CurrentVehicleGrossWeight = Conversion.ToDecimal((object)txtvehicleweight.Text);
					GridTotalSupGrossWtOfCurVehicle = (from r in dtDetail.AsEnumerable()
						where r.Field<string>("VehicleNo") == CurrentVehicleNo
						select r).Sum((DataRow r) => r.Field<decimal>("SupGrossWeight"));
				}
				if (GridTotalSupGrossWtOfCurVehicle == 0m || CurrentVehicleGrossWeight - GridTotalSupGrossWtOfCurVehicle == 0m)
				{
					if (updateDetailIndex > -1)
					{
						VehicleInfoEnable = true;
					}
					else
					{
						VehicleInfoEnable = true;
					}
				}
				else
				{
					VehicleInfoEnable = false;
				}
			}
			else
			{
				VehicleInfoEnable = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VehicleInfoFillOnRowRemove()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			EnableDisableVehicleInfo();
			if (VehicleInfoEnable)
			{
				return;
			}
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			string VehicleNos = "";
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow row2 in rows)
			{
				VehicleNos = VehicleNos + "," + Conversion.ToString((object)row2.Cells["VehicleNo"].Text);
			}
			foreach (string No in (from s in VehicleNos.Split(',')
				where !string.IsNullOrEmpty(s)
				select s).Distinct())
			{
				decimal CurrentVehicleGrossWeight = (from row in dtDetail.AsEnumerable()
					where row.Field<string>("VehicleNo") == No
					select row.Field<decimal>("VehicleWeight")).FirstOrDefault();
				if ((from row in dtDetail.AsEnumerable()
					where row.Field<string>("VehicleNo") == No
					select row).Sum((DataRow row) => row.Field<decimal>("SupGrossWeight")) != CurrentVehicleGrossWeight)
				{
					int CurrentTransporterId = (from row in dtDetail.AsEnumerable()
						where row.Field<string>("VehicleNo") == No
						select row.Field<int>("TransporterId")).FirstOrDefault();
					string CurrentBiltyNo = (from row in dtDetail.AsEnumerable()
						where row.Field<string>("VehicleNo") == No
						select row.Field<string>("BiltyNo")).FirstOrDefault();
					DateTime CurrentBiltyDate = (from row in dtDetail.AsEnumerable()
						where row.Field<string>("VehicleNo") == No
						select row.Field<DateTime>("BiltyDate")).FirstOrDefault();
					decimal CurrentBiltyExpense = (from row in dtDetail.AsEnumerable()
						where row.Field<string>("VehicleNo") == No
						select row.Field<decimal>("BiltyExpense")).FirstOrDefault();
					decimal CurrentBiltyFreight = (from row in dtDetail.AsEnumerable()
						where row.Field<string>("VehicleNo") == No
						select row.Field<decimal>("BiltyFreight")).FirstOrDefault();
					cmbTransporter.Value = CurrentTransporterId;
					txtBiltyNo.Text = Conversion.ToString((object)CurrentBiltyNo);
					txtbiltydate.Text = Conversion.ToString((object)CurrentBiltyDate);
					txtbiltyexpense.Text = Conversion.ToString((object)CurrentBiltyExpense);
					txtfrieght.Text = Conversion.ToString((object)CurrentBiltyFreight);
					txtvehicleno.Text = Conversion.ToString((object)No);
					txtvehicleweight.Text = Conversion.ToString((object)CurrentVehicleGrossWeight);
					break;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("An error occurred: " + ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtdocno.Text.Trim() == "" || txtdocno.Text.Trim() == "0" || txtdocno.Text.Trim() == null)
		{
			MessageBox.Show("DocNo Field is Required");
			txtdocno.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)cmbBroker).ActiveRow == null || Conversion.ToInt(cmbBroker.Value) == 0)
		{
			MessageBox.Show("Broker Field is Required");
			cmbBroker.Focus();
			return false;
		}
		if ((((UltraGridBase)cmbTransporter).ActiveRow == null || Conversion.ToInt(cmbTransporter.Value) == 0) && (Conversion.ToDecimal((object)txtbiltyexpense.Text) != 0m || Conversion.ToDecimal((object)txtfrieght.Text) != 0m))
		{
			MessageBox.Show("Transporter Field is Required");
			cmbTransporter.Focus();
			return false;
		}
		if (txtBiltyNo.Text.Trim() == "" || txtBiltyNo.Text.Trim() == "0" || txtBiltyNo.Text.Trim() == null)
		{
			MessageBox.Show("BiltyNo Field is Required");
			txtBiltyNo.Focus();
			return false;
		}
		if ((((UltraGridBase)cmbTransporter).ActiveRow == null || Conversion.ToInt(cmbTransporter.Value) == 0) && (Conversion.ToDecimal((object)txtfrieght.Text) != 0m || Conversion.ToDecimal((object)txtbiltyexpense.Text) != 0m))
		{
			MessageBox.Show("Transporter Field is Required when Bilty expense or Frieght is not Zero... ");
			txtfrieght.Focus();
			return false;
		}
		if (txtvehicleno.Text.Trim() == "" || txtvehicleno.Text.Trim() == "0" || txtvehicleno.Text.Trim() == null)
		{
			MessageBox.Show("Vehicle No Field is Required");
			txtvehicleno.Focus();
			return false;
		}
		if (txtVehcileQty.Text.Trim() == "" || Conversion.ToDouble((object)txtVehcileQty.Text) == 0.0 || txtVehcileQty.Text.Trim() == null)
		{
			MessageBox.Show("Vehicle Qty Field is Required");
			txtVehcileQty.Focus();
			return false;
		}
		if (txtvehicleweight.Text.Trim() == "" || Conversion.ToDouble((object)txtvehicleweight.Text) == 0.0 || txtvehicleweight.Text.Trim() == null)
		{
			MessageBox.Show("Vehicle Weight Field is Required");
			txtvehicleweight.Focus();
			return false;
		}
		if (((UltraGridBase)cmbitem).ActiveRow == null || Conversion.ToInt(cmbitem.Value) == 0)
		{
			MessageBox.Show("Item Field is Required");
			cmbitem.Focus();
			return false;
		}
		if (((UltraGridBase)cmbcropYear).ActiveRow == null || Conversion.ToInt(cmbcropYear.Value) == 0)
		{
			MessageBox.Show("Crop Year Field is Required");
			cmbcropYear.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackingType).ActiveRow == null || Conversion.ToInt(CmbPackingType.Value) == 0)
		{
			MessageBox.Show("Packing Type Field is Required");
			CmbPackingType.Focus();
			return false;
		}
		if (((UltraGridBase)cmbuom).ActiveRow == null || Conversion.ToInt(cmbuom.Value) == 0)
		{
			MessageBox.Show("UOM Field is Required");
			cmbuom.Focus();
			return false;
		}
		if (txtItemSupQty.Text.Trim() == "" || Conversion.ToDouble((object)txtItemSupQty.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Qty Field is Required");
			txtItemSupQty.Focus();
			return false;
		}
		if (((UltraGridBase)cmbsupplier).ActiveRow == null || Conversion.ToInt(cmbsupplier.Value) == 0)
		{
			MessageBox.Show("Supplier Field is Required");
			cmbsupplier.Focus();
			return false;
		}
		if (txtItemSupGrossWt.Text.Trim() == "" || Conversion.ToDouble((object)txtItemSupGrossWt.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Supplier Gross Weight Field is Required");
			txtItemSupGrossWt.Focus();
			return false;
		}
		if (txtItemSupNetWeight.Text.Trim() == "" || Conversion.ToDouble((object)txtItemSupNetWeight.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Supplier Net Weught Field is Required");
			txtItemSupNetWeight.Focus();
			return false;
		}
		if (((UltraGridBase)cmbcustomer).ActiveRow == null || Conversion.ToInt(cmbcustomer.Value) == 0)
		{
			MessageBox.Show("Customer Field is Required");
			cmbcustomer.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbsupplier.Value) == Conversion.ToInt(cmbcustomer.Value))
		{
			MessageBox.Show("Supplier And Customer Can Not be Same!");
			cmbcustomer.Focus();
			return false;
		}
		if (txtcustgrosswt.Text.Trim() == "" || Conversion.ToDouble((object)txtcustgrosswt.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Customer Gross Weight Field is Required");
			txtcustgrosswt.Focus();
			return false;
		}
		if (txtcustnetwt.Text.Trim() == "" || Conversion.ToDouble((object)txtcustnetwt.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Customer Net Weught Field is Required");
			txtcustnetwt.Focus();
			return false;
		}
		return true;
	}

	private void ValidationforItemUomSup()
	{
		try
		{
			foreach (var combination in (from row in dtDetail.AsEnumerable()
				select new
				{
					ItemId = row.Field<int>("ItemId"),
					SupplierId = row.Field<int>("SupplierId"),
					UomId = row.Field<int>("UomId"),
					SupRate = row.Field<decimal>("SupRate"),
					VehicleNo = row.Field<string>("VehicleNo")
				}).Distinct().ToList())
			{
				int ItemId = combination.ItemId;
				int SupplierId = combination.SupplierId;
				int UomId = combination.UomId;
				decimal SupRate = combination.SupRate;
				string VehicleNo = combination.VehicleNo;
				List<DataRow> matchingRows = (from r in dtDetail.AsEnumerable()
					where r.Field<int>("ItemId") == ItemId && r.Field<int>("SupplierId") == SupplierId && r.Field<int>("UomId") == UomId && r.Field<decimal>("SupRate") == SupRate && r.Field<string>("VehicleNo") == VehicleNo
					select r).ToList();
				decimal GridTotalSupGrossWeight = matchingRows.Sum((DataRow r) => r.Field<decimal>("SupGrossWeight"));
				decimal GridMatchFirstItemUomSupGrossWeight = matchingRows.Select((DataRow r) => r.Field<decimal>("ItemSupWeight")).FirstOrDefault();
				decimal value = 0.99m;
				if (Math.Round(GridTotalSupGrossWeight, 4) - Math.Round(GridMatchFirstItemUomSupGrossWeight, 4) > value)
				{
					string ItemName = Conversion.ToString((object)matchingRows.Select((DataRow r) => r.Field<string>("ItemName")).FirstOrDefault());
					string UomName = Conversion.ToString((object)matchingRows.Select((DataRow r) => r.Field<string>("UOM")).FirstOrDefault());
					string SupplierName = Conversion.ToString((object)matchingRows.Select((DataRow r) => r.Field<string>("SupplierName")).FirstOrDefault());
					throw new Exception("Please Check ItemSupWeight against Item '" + ItemName + "' ,Uom " + UomName + " ," + $"Supplier Name '{SupplierName}' and Vehcile {VehicleNo} with Rate {SupRate} \n" + "Is not Equal To Total SupplierGrossWeight \n" + $"ItemUomSupWeight  against this Rate {SupRate} Is '{GridMatchFirstItemUomSupGrossWeight}' and Supplier Total Gross Weight Against this Item Is '{GridTotalSupGrossWeight}'...");
				}
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void ValidationforSupplierPT()
	{
		try
		{
			foreach (int supplierId2 in (from row in dtDetail.AsEnumerable()
				select row.Field<int>("SupplierId")).Distinct().ToList())
			{
				if (!dtSupplierPTDetail.AsEnumerable().Any((DataRow row) => row.Field<int>("SupplierId") == supplierId2))
				{
					string supplierName3 = (from r in dtDetail.AsEnumerable()
						where r.Field<int>("SupplierId") == supplierId2
						select r.Field<string>("SupplierName")).FirstOrDefault();
					throw new Exception("Please check payment detail of supplier '" + supplierName3 + "' not found in supplier payment detail grid.");
				}
			}
			foreach (int supplierId in (from row in dtSupplierPTDetail.AsEnumerable()
				select row.Field<int>("SupplierId")).Distinct().ToList())
			{
				List<DataRow> matchingRows = (from r in dtSupplierPTDetail.AsEnumerable()
					where r.Field<int>("SupplierId") == supplierId
					select r).ToList();
				if (Math.Round(matchingRows.Sum((DataRow r) => r.Field<decimal>("%ofTotal")), 0) != 100m)
				{
					string supplierName = (from r in dtDetail.AsEnumerable()
						where r.Field<int>("SupplierId") == supplierId
						select r.Field<string>("SupplierName")).FirstOrDefault();
					throw new Exception("Please check %ofTotal against SupplierName '" + supplierName + "' is not equal to 100.");
				}
				if (matchingRows.Select((DataRow r) => r.Field<int>("PaymentTermId")).Distinct().ToList()
					.Count != matchingRows.Count)
				{
					string supplierName2 = (from r in dtDetail.AsEnumerable()
						where r.Field<int>("SupplierId") == supplierId
						select r.Field<string>("SupplierName")).FirstOrDefault();
					throw new Exception("Supplier '" + supplierName2 + "' has duplicate Payment Terms in the payment detail grid.");
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
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Expected O, but got Unknown
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Expected O, but got Unknown
		//IL_0dbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc5: Expected O, but got Unknown
		//IL_0b1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b24: Expected O, but got Unknown
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
			string VehicleNos = "";
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow row2 in rows)
			{
				VehicleNos = VehicleNos + "," + Conversion.ToString((object)row2.Cells["VehicleNo"].Text);
			}
			foreach (string No in (from s in VehicleNos.Split(',')
				where !string.IsNullOrEmpty(s)
				select s).Distinct())
			{
				decimal CurrentVehicleGrossWeight = (from row in dtDetail.AsEnumerable()
					where row.Field<string>("VehicleNo") == No
					select row.Field<decimal>("VehicleWeight")).FirstOrDefault();
				decimal CurrentSupGrossWeight = (from row in dtDetail.AsEnumerable()
					where row.Field<string>("VehicleNo") == No
					select row).Sum((DataRow row) => row.Field<decimal>("SupGrossWeight"));
				decimal value = 0.99m;
				if (Math.Round(CurrentSupGrossWeight, 4) - Math.Round(CurrentVehicleGrossWeight, 4) > value)
				{
					CurrentSupGrossWeight = Math.Round(CurrentSupGrossWeight, 4);
					CurrentVehicleGrossWeight = Math.Round(CurrentVehicleGrossWeight, 4);
					throw new Exception($"Please Check the weight of vehicle {No} has not been completed yet in the Grid....\nVehicle Weight Is {CurrentVehicleGrossWeight} and Total Supplier,Item and Uom Weight is {CurrentSupGrossWeight}.\nSo Difference is {CurrentVehicleGrossWeight - CurrentSupGrossWeight}");
				}
			}
			ValidationforItemUomSup();
			ValidationforSupplierPT();
			Conversion.ToDouble((object)txtbiltyexpense.Text.Trim());
			Conversion.ToDouble((object)txtfrieght.Text.Trim());
			if (dtPackingMaterialDetail.Rows.Count > 0)
			{
				foreach (DataRow r4 in dtPackingMaterialDetail.Rows)
				{
					int num = Conversion.ToInt(r4["PackingTypeId"]);
					int ItemId2 = Conversion.ToInt(r4["ItemId"]);
					int CreditAccountId2 = Conversion.ToInt(r4["CreditAccountId"]);
					if (num > 0 || ItemId2 > 0 || CreditAccountId2 > 0)
					{
						int CustomerAccountId = Conversion.ToInt(r4["CustomerAccountId"]);
						string CustomerAccount = Conversion.ToString(r4["CustomerAccount"]);
						if (CustomerAccountId <= 0)
						{
							throw new Exception("Customer Account Required In PM Grid");
						}
						if (!dtDetail.AsEnumerable().Any((DataRow row) => row.Field<int>("CustomerId") == CustomerAccountId))
						{
							throw new Exception("Customer Account " + CustomerAccount + " In PM Grid does not exists in Detail Grid");
						}
					}
				}
			}
			CommTradeLoadingDelivery ct = new CommTradeLoadingDelivery();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				ct.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			ct.OrganizationId = UserAccount.OrganizationId;
			ct.CompanyId = UserAccount.CompanyId;
			ct.BranchesId = UserAccount.BranchesId;
			ct.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			ct.DocumentTypeId = 1702;
			ct.ModifyUserId = UserAccount.ID;
			ct.EntryUserId = UserAccount.ID;
			ct.EntryDate = DateTime.Now;
			ct.ModifyDate = DateTime.Now;
			ct.ApprovedDate = DateTime.Now;
			ct.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
			ct.ManualRefNo = txtmanualno.Text;
			ct.ApprovedUserId = UserAccount.ID;
			ct.ActionId = UserAccount.ID;
			ct.DocNo = Conversion.ToInt((object)txtdocno.Text);
			ct.IsApproved = true;
			ct.RemarksHeader = Conversion.ToString((object)txtRemarksHeader.Text);
			ct.ScreenName = Conversion.ToString((object)base.Name);
			ct.CommTradeLoadingDeliveryDetailList = new List<CommTradeLoadingDeliveryDetail>();
			ct.CommTradeLoadingDeliveryRevExpList = new List<CommTradeLoadingDeliveryRevExp>();
			ct.CommTradeLoadingDeliveryPmOtherList = new List<CommTradeLoadingDeliveryPmOther>();
			ct.CommTradeLoadingDeliveryPaymentTermsDetailList = new List<CommTradeLoadingDeliveryPaymentTermsDetail>();
			ct.CommTradeLoadingDeliverySupplierExpList = new List<CommTradeLoadingDeliverySupplierExp>();
			rows = grd.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				CommTradeLoadingDeliveryDetail vd = new CommTradeLoadingDeliveryDetail();
				vd.Id = Conversion.ToInt(r3.Cells["Id"].Value);
				if (vd.Id > 0)
				{
					vd.ActionTypeId = 2;
				}
				else
				{
					vd.ActionTypeId = 1;
				}
				vd.BrokerId = Conversion.ToInt(r3.Cells["BrokerId"].Value);
				vd.DocDateDt = Conversion.ToDateTime((object)txtdocdate.Value);
				vd.TransporterId = Conversion.ToInt(r3.Cells["TransporterId"].Value);
				vd.BiltyNo = Conversion.ToString(r3.Cells["BiltyNo"].Value);
				vd.BiltyDate = Conversion.ToDateTime(r3.Cells["BiltyDate"].Value);
				vd.BiltyExpense = Conversion.ToDecimal(r3.Cells["BiltyExpense"].Value);
				vd.VehicleNo = Conversion.ToString(r3.Cells["VehicleNo"].Value);
				vd.BiltyFreight = Conversion.ToDecimal(r3.Cells["BiltyFreight"].Value);
				vd.VehicleQty = Conversion.ToDecimal(r3.Cells["VehicleQty"].Value);
				vd.VehicleGrossWeight = Conversion.ToDecimal(r3.Cells["VehicleWeight"].Value);
				vd.ItemId = Conversion.ToInt(r3.Cells["ItemId"].Value);
				vd.CropYearId = Conversion.ToInt(r3.Cells["CropYearId"].Value);
				vd.PackingTypeId = Conversion.ToInt((object)r3.Cells["PackingTypeId"].Text);
				vd.UomId = Conversion.ToInt((object)r3.Cells["UomId"].Text);
				vd.ItemSupQty = Conversion.ToDecimal(r3.Cells["ItemSupQty"].Value);
				vd.ItemSupWeight = Conversion.ToDecimal(r3.Cells["ItemSupWeight"].Value);
				vd.SupplierId = Conversion.ToInt(r3.Cells["SupplierId"].Value);
				vd.SupGrossWeight = Conversion.ToDecimal(r3.Cells["SupGrossWeight"].Value);
				vd.SupPckMtrlWeight = Conversion.ToDecimal(r3.Cells["SupPckMtrlWeight"].Value);
				vd.SupAdLsWt = Conversion.ToDecimal(r3.Cells["SupAdLsWt"].Value);
				vd.SupNetWeight = Conversion.ToDecimal(r3.Cells["SupNetWeight"].Value);
				vd.SupRate = Conversion.ToDecimal(r3.Cells["SupRate"].Value);
				vd.SupRemarks = Conversion.ToString(r3.Cells["SupRemarks"].Value);
				vd.Qty = Conversion.ToDecimal(r3.Cells["Qty"].Value);
				vd.CustomerId = Conversion.ToInt(r3.Cells["CustomerId"].Value);
				vd.CusGrossWeight = Conversion.ToDecimal(r3.Cells["CusGrossWeight"].Value);
				vd.CusPckMtrlWeight = Conversion.ToDecimal(r3.Cells["CusPckMtrlWeight"].Value);
				vd.CusAdLsWt = Conversion.ToDecimal(r3.Cells["CusAdLsWt"].Value);
				vd.CusNetWeight = Conversion.ToDecimal(r3.Cells["CusNetWeight"].Value);
				vd.CusRate = Conversion.ToDecimal(r3.Cells["CusRate"].Value);
				vd.CusRemarks = Conversion.ToString(r3.Cells["CusRemarks"].Value);
				ct.CommTradeLoadingDeliveryDetailList.Add(vd);
			}
			if (lstRemoveRecord.Count > 0)
			{
				for (int i = 0; i < lstRemoveRecord.Count; i++)
				{
					ct.CommTradeLoadingDeliveryDetailList.Add(lstRemoveRecord[i]);
				}
			}
			rows = grdPackingMaterial.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				int PackingTypeId = Conversion.ToInt(r2.Cells["PackingTypeId"].Value);
				int ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
				int CreditAccountId = Conversion.ToInt(r2.Cells["CreditAccountId"].Value);
				int StockTypeId = Conversion.ToInt(r2.Cells["StockTypeId"].Value);
				int WareHouseId = Conversion.ToInt(r2.Cells["WareHouseId"].Value);
				if (PackingTypeId > 0 || ItemId > 0 || CreditAccountId > 0)
				{
					if (ItemId == 0)
					{
						MessageBox.Show("Please Select an Item Against Packing Material Expense In Row No" + (r2.RowIndex + 1));
						return;
					}
					if (PackingTypeId == 0)
					{
						MessageBox.Show("Please Select PackingType  Against Packing Material Expense In Row No" + (r2.RowIndex + 1));
						return;
					}
					if (CreditAccountId == 0 && StockTypeId != 1)
					{
						MessageBox.Show("Please Select CreditAccount Against Packing Material Expense In Row No" + (r2.RowIndex + 1));
						return;
					}
					CommTradeLoadingDeliveryPmOther PE = new CommTradeLoadingDeliveryPmOther();
					PE.Id = Conversion.ToInt(r2.Cells["Id"].Value);
					PE.StockTypeId = StockTypeId;
					if (StockTypeId == 0)
					{
						MessageBox.Show("Please Select StockType Against Packing Material In Row No" + (r2.RowIndex + 1));
						return;
					}
					if (WareHouseId == 0)
					{
						StockTypeId = 2;
					}
					else
					{
						StockTypeId = 1;
					}
					PE.PackingTypeId = PackingTypeId;
					PE.ItemId = ItemId;
					PE.WareHouseId = WareHouseId;
					PE.ItemSize = Conversion.ToString(r2.Cells["ItemSize"].Value);
					PE.ItemWeight = Conversion.ToDecimal(r2.Cells["Weight"].Value);
					PE.Qty = Conversion.ToInt(r2.Cells["Qty"].Value);
					if (PE.StockTypeId == 1 && PE.Qty == 0m)
					{
						MessageBox.Show("Please Insert Qty (because StockType is Company) Packing Material In Row No" + (r2.RowIndex + 1));
						return;
					}
					PE.Rate = Conversion.ToDecimal(r2.Cells["Rate"].Value);
					if (PE.StockTypeId == 1 && PE.Rate == 0m)
					{
						MessageBox.Show("Please Insert Rate (because StockType is Company) Packing Material In Row No" + (r2.RowIndex + 1));
						return;
					}
					PE.Amount = Conversion.ToDecimal(r2.Cells["Amount"].Value);
					if (PE.Amount == 0m)
					{
						MessageBox.Show("Please Insert Amount Against Packing Material Expense In Row No" + (r2.RowIndex + 1));
						return;
					}
					PE.CreditAccountId = CreditAccountId;
					PE.DebitAccountId = Conversion.ToInt(r2.Cells["CustomerAccountId"].Value);
					ct.CommTradeLoadingDeliveryPmOtherList.Add(PE);
				}
			}
			if (grdPaymentTermSupplier.GetRows().Count() <= 0)
			{
				tabControl2.SelectedIndex = 2;
				((Control)(object)grdPaymentTermSupplier).Focus();
				MessageBox.Show("Supplier Payment Detail Record Not Found", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			string SupplierIds = "";
			rows = grdPaymentTermSupplier.GetRows();
			foreach (GridEXRow r in rows)
			{
				CommTradeLoadingDeliveryPaymentTermsDetail PJ = new CommTradeLoadingDeliveryPaymentTermsDetail();
				PJ.Id = Conversion.ToInt(r.Cells["Id"].Value);
				PJ.SupplierCustomerTypeId = 1;
				PJ.SupplierCustomerId = Conversion.ToInt(r.Cells["SupplierId"].Value);
				SupplierIds = SupplierIds + "," + Conversion.ToString((object)PJ.SupplierCustomerId);
				if (Conversion.ToInt(r.Cells["SupplierId"].Value) == 0)
				{
					tabControl2.SelectedIndex = 2;
					MessageBox.Show("Supplier Is Required In 'Supplier PaymentTerm' Grid In row No " + (r.RowIndex + 1));
					return;
				}
				PJ.PaymentTermId = Conversion.ToInt(r.Cells["PaymentTermId"].Value);
				if (Conversion.ToInt(r.Cells["PaymentTermId"].Value) == 0)
				{
					tabControl2.SelectedIndex = 2;
					MessageBox.Show("PaymentTerm Is Required In 'Supplier PaymentTerm' Grid In row No " + (r.RowIndex + 1));
					return;
				}
				PJ.PrcntOfTotal = Conversion.ToDecimal(r.Cells["%ofTotal"].Value);
				if (Conversion.ToInt(r.Cells["%ofTotal"].Value) == 0)
				{
					tabControl2.SelectedIndex = 2;
					MessageBox.Show("%ofTotal Is Required In 'Supplier PaymentTerm' Grid In row No " + (r.RowIndex + 1));
					return;
				}
				PJ.DueDays = Conversion.ToInt(r.Cells["DueDays"].Value);
				if (PJ.PaymentTermId == 11 && PJ.DueDays == 0)
				{
					tabControl2.SelectedIndex = 2;
					MessageBox.Show("DueDays Is Required When PaymentTerm is Credit In 'Supplier PaymentTerm' Grid In row No " + (r.RowIndex + 1));
					return;
				}
				PJ.DueDate = Conversion.ToDateTime(r.Cells["DueDate"].Value);
				if (Conversion.CheckDateTimeNull(PJ.DueDate) && PJ.PaymentTermId == 11)
				{
					tabControl2.SelectedIndex = 2;
					MessageBox.Show("DueDate Is Required When PaymentTerm is Credit In 'Supplier PaymentTerm' Grid In row No " + (r.RowIndex + 1));
					return;
				}
				PJ.PaymentRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
				ct.CommTradeLoadingDeliveryPaymentTermsDetailList.Add(PJ);
			}
			ct.AttachmentsList = new List<DMSAttachments>();
			ct.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<CommTradeLoadingDelivery>(ct, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = (ct.Id = CommTradeLoadingDelivery.Save(ct));
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + ct.DocNo);
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					DMSAttachments.RemoveByIdAndNames(RecId, base.Name, ct.DocumentTypeId, 0);
				}
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + ct.DocNo);
			}
			Reset();
			ResetDetail();
			CommTradeLoadingDeliveryExpenses commTradeLoadingDeliveryExpenses = new CommTradeLoadingDeliveryExpenses(UserAccount);
			commTradeLoadingDeliveryExpenses.CmbLoadingDelivery.Value = success;
			((Control)(object)commTradeLoadingDeliveryExpenses.CmbLoadingDelivery).Enabled = false;
			commTradeLoadingDeliveryExpenses.ShowDialog();
			commTradeLoadingDeliveryExpenses.BringToFront();
			if (chkPrint.Checked)
			{
				GenerateSlip1702Print(success);
			}
			if (ChkUrduPrintPreview.Checked)
			{
				GenerateUrduSlip1702Print(success);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int ID)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		try
		{
			Reset();
			RecId = Conversion.ToInt((object)ID);
			CommTradeLoadingDelivery po = new CommTradeLoadingDelivery();
			po = CommTradeLoadingDelivery.ReadById(RecId);
			tabControl1.SelectedIndex = 0;
			txtdocdate.Value = po.DocDate;
			txtdocno.Text = Conversion.ToString((object)po.DocNo);
			txtmanualno.Text = Conversion.ToString((object)po.ManualRefNo);
			txtRemarksHeader.Text = Conversion.ToString((object)po.RemarksHeader);
			if (po.CommTradeLoadingDeliveryDetailList.Count > 0)
			{
				cmbBroker.Value = Conversion.ToInt((object)po.CommTradeLoadingDeliveryDetailList[0].BrokerId);
				dtDetail.Clear();
				for (int k = 0; k < po.CommTradeLoadingDeliveryDetailList.Count; k++)
				{
					dtDetail.Rows.Add(po.CommTradeLoadingDeliveryDetailList[k].Id, po.CommTradeLoadingDeliveryDetailList[k].BrokerId, po.CommTradeLoadingDeliveryDetailList[k].BrokerName, po.CommTradeLoadingDeliveryDetailList[k].TransporterId, po.CommTradeLoadingDeliveryDetailList[k].TransporterName, po.CommTradeLoadingDeliveryDetailList[k].BiltyNo, po.CommTradeLoadingDeliveryDetailList[k].BiltyDate, po.CommTradeLoadingDeliveryDetailList[k].BiltyExpense, po.CommTradeLoadingDeliveryDetailList[k].BiltyFreight, po.CommTradeLoadingDeliveryDetailList[k].VehicleNo, po.CommTradeLoadingDeliveryDetailList[k].VehicleQty, po.CommTradeLoadingDeliveryDetailList[k].VehicleGrossWeight, po.CommTradeLoadingDeliveryDetailList[k].ItemId, po.CommTradeLoadingDeliveryDetailList[k].ItemCode, po.CommTradeLoadingDeliveryDetailList[k].ItemName, po.CommTradeLoadingDeliveryDetailList[k].CropYearId, po.CommTradeLoadingDeliveryDetailList[k].CropYear, po.CommTradeLoadingDeliveryDetailList[k].PackingTypeId, po.CommTradeLoadingDeliveryDetailList[k].PackingType, po.CommTradeLoadingDeliveryDetailList[k].UomId, po.CommTradeLoadingDeliveryDetailList[k].PackUomEquivalent, po.CommTradeLoadingDeliveryDetailList[k].ItemSupQty, po.CommTradeLoadingDeliveryDetailList[k].ItemSupWeight, po.CommTradeLoadingDeliveryDetailList[k].Qty, po.CommTradeLoadingDeliveryDetailList[k].SupplierId, po.CommTradeLoadingDeliveryDetailList[k].SupplierName, po.CommTradeLoadingDeliveryDetailList[k].SupGrossWeight, po.CommTradeLoadingDeliveryDetailList[k].SupPckMtrlWeight, po.CommTradeLoadingDeliveryDetailList[k].SupAdLsWt, po.CommTradeLoadingDeliveryDetailList[k].SupNetWeight, po.CommTradeLoadingDeliveryDetailList[k].SupRate, po.CommTradeLoadingDeliveryDetailList[k].SupRemarks, po.CommTradeLoadingDeliveryDetailList[k].CustomerId, po.CommTradeLoadingDeliveryDetailList[k].CustomerName, po.CommTradeLoadingDeliveryDetailList[k].CusGrossWeight, po.CommTradeLoadingDeliveryDetailList[k].CusPckMtrlWeight, po.CommTradeLoadingDeliveryDetailList[k].CusAdLsWt, po.CommTradeLoadingDeliveryDetailList[k].CusNetWeight, po.CommTradeLoadingDeliveryDetailList[k].CusRate, po.CommTradeLoadingDeliveryDetailList[k].CusRemarks);
				}
				grd.DataSource = dtDetail;
				grd.RetrieveStructure();
				grdSettings();
			}
			dtSupplierPTDetail.Rows.Clear();
			if (po.CommTradeLoadingDeliveryPaymentTermsDetailList.Count > 0)
			{
				for (int j = 0; j < po.CommTradeLoadingDeliveryPaymentTermsDetailList.Count; j++)
				{
					if (Conversion.ToInt((object)po.CommTradeLoadingDeliveryPaymentTermsDetailList[j].SupplierCustomerTypeId) == 1)
					{
						dtSupplierPTDetail.Rows.Add(po.CommTradeLoadingDeliveryPaymentTermsDetailList[j].Id, po.CommTradeLoadingDeliveryPaymentTermsDetailList[j].SupplierCustomerId, po.CommTradeLoadingDeliveryPaymentTermsDetailList[j].PaymentTermId, po.CommTradeLoadingDeliveryPaymentTermsDetailList[j].PrcntOfTotal, po.CommTradeLoadingDeliveryPaymentTermsDetailList[j].DueDays, po.CommTradeLoadingDeliveryPaymentTermsDetailList[j].DueDate, po.CommTradeLoadingDeliveryPaymentTermsDetailList[j].PaymentRemarks);
					}
				}
			}
			if (dtSupplierPTDetail.Rows.Count == 0)
			{
				AddRowInPaymentTermSupplierGrid();
			}
			grdPaymentTermSupplier.DataSource = dtSupplierPTDetail;
			grdPaymentTermSupplier.RetrieveStructure();
			grdSettingsPaymentTermSupplier();
			dtPackingMaterialDetail.Rows.Clear();
			if (po.CommTradeLoadingDeliveryPmOtherList.Count > 0)
			{
				for (int i = 0; i < po.CommTradeLoadingDeliveryPmOtherList.Count; i++)
				{
					dtPackingMaterialDetail.Rows.Add(po.CommTradeLoadingDeliveryPmOtherList[i].Id, po.CommTradeLoadingDeliveryPmOtherList[i].WareHouseId, po.CommTradeLoadingDeliveryPmOtherList[i].StockTypeId, po.CommTradeLoadingDeliveryPmOtherList[i].PackingTypeId, po.CommTradeLoadingDeliveryPmOtherList[i].ItemId, po.CommTradeLoadingDeliveryPmOtherList[i].ItemName, po.CommTradeLoadingDeliveryPmOtherList[i].ItemSize, po.CommTradeLoadingDeliveryPmOtherList[i].ItemWeight, po.CommTradeLoadingDeliveryPmOtherList[i].Qty, po.CommTradeLoadingDeliveryPmOtherList[i].Rate, po.CommTradeLoadingDeliveryPmOtherList[i].Amount, po.CommTradeLoadingDeliveryPmOtherList[i].DebitAccountId, po.CommTradeLoadingDeliveryPmOtherList[i].DebitAccountTitle, po.CommTradeLoadingDeliveryPmOtherList[i].CreditAccountId, po.CommTradeLoadingDeliveryPmOtherList[i].CreditAccountTitle);
				}
			}
			if (dtPackingMaterialDetail.Rows.Count == 0)
			{
				AddRowPackingmaterial();
			}
			grdPackingMaterial.DataSource = dtPackingMaterialDetail;
			grdPackingMaterial.RetrieveStructure();
			grdpackingsetting();
			btnSave.Visible = false;
			UpdateMode = true;
			btnUpdate.Visible = true;
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					CommTradeLoadingDelivery.DeleteByID(UserAccount.ID, RecId);
					MessageBox.Show("Delete Record Successfully");
					Reset();
				}
			}
			else
			{
				MessageBox.Show("No record found to Delete");
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
				txtdocdate.Focus();
			}
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
			HistoryGridfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters Obj = new ReportsParameters();
			Obj.OrganizationId = UserAccount.OrganizationId;
			Obj.CompanyId = UserAccount.CompanyId;
			Obj.BranchesId = UserAccount.BranchesId;
			Obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			Obj.DocumentTypeId = 1702;
			Obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			Obj.EntryUser = UserAccount.ID;
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					Obj.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					Obj.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					Obj.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					Obj.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					Obj.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					Obj.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					Obj.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					Obj.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			Obj.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			Obj.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			dtgrid = CommTradeLoadingDelivery.FormHistory(Obj);
			if (dtgrid.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("RecordNo", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("DocNo", typeof(string));
				dt.Columns.Add("ManualRefNo", typeof(string));
				dt.Columns.Add("TotalQty", typeof(decimal));
				dt.Columns.Add("SupTotalNetWeight", typeof(decimal));
				dt.Columns.Add("CusTotalNetWeight", typeof(decimal));
				dt.Columns.Add("TotalRecOutQty", typeof(decimal));
				dt.Columns.Add("SupTotalRecWeight", typeof(decimal));
				dt.Columns.Add("CusTotalOutWeight", typeof(decimal));
				dt.Columns.Add("BalQty", typeof(decimal));
				dt.Columns.Add("BalSupWeight", typeof(decimal));
				dt.Columns.Add("BalCusWeight", typeof(decimal));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("IsApproved", typeof(bool));
				dt.Columns.Add("RemarksHeader", typeof(string));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				dt.Clear();
				for (int i = 0; i < dtgrid.Rows.Count; i++)
				{
					dt.Rows.Add(Conversion.ToInt(dtgrid.Rows[i]["RecordNo"]), Conversion.ToInt(dtgrid.Rows[i]["DocumentTypeId"]), Conversion.ToInt(dtgrid.Rows[i]["Id"]), Conversion.ToDateTime(dtgrid.Rows[i]["DocDate"]), Conversion.ToString(dtgrid.Rows[i]["DocNo"]), Conversion.ToString(dtgrid.Rows[i]["ManualRefNo"]), Conversion.ToDecimal(dtgrid.Rows[i]["TotalQty"]), Conversion.ToDecimal(dtgrid.Rows[i]["SupTotalNetWeight"]), Conversion.ToDecimal(dtgrid.Rows[i]["CusTotalNetWeight"]), Conversion.ToDecimal(dtgrid.Rows[i]["TotalRecOutQty"]), Conversion.ToDecimal(dtgrid.Rows[i]["SupTotalRecWeight"]), Conversion.ToDecimal(dtgrid.Rows[i]["CusTotalOutWeight"]), Conversion.ToDecimal(dtgrid.Rows[i]["BalQty"]), Conversion.ToDecimal(dtgrid.Rows[i]["BalSupWeight"]), Conversion.ToDecimal(dtgrid.Rows[i]["BalCusWeight"]), Conversion.ToDateTime(dtgrid.Rows[i]["EntryDate"]), Conversion.ToString(dtgrid.Rows[i]["EntryUserName"]), Conversion.ToDateTime(dtgrid.Rows[i]["ModifyDate"]), Conversion.ToString(dtgrid.Rows[i]["ModifyUserName"]), Conversion.ToBool(dtgrid.Rows[i]["IsApproved"]), Conversion.ToString(dtgrid.Rows[i]["RemarksHeader"]), Conversion.ToInt(dtgrid.Rows[i]["NoOfAttachments"]));
				}
				grdHistory.DataSource = dt;
				grdHistory.RetrieveStructure();
				grdsetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdsetting()
	{
		try
		{
			grdHistory.RootTable.Columns["RecordNo"].Visible = false;
			grdHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["TotalQty"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["TotalQty"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["TotalQty"].FormatString = "#,##0.###";
			grdHistory.RootTable.Columns["TotalQty"].TotalFormatString = "#,##0.###";
			grdHistory.RootTable.Columns["SupTotalNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["SupTotalNetWeight"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["SupTotalNetWeight"].FormatString = "#,##0.###";
			grdHistory.RootTable.Columns["SupTotalNetWeight"].TotalFormatString = "#,##0.###";
			grdHistory.RootTable.Columns["CusTotalNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["CusTotalNetWeight"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["CusTotalNetWeight"].FormatString = "#,##0.###";
			grdHistory.RootTable.Columns["CusTotalNetWeight"].TotalFormatString = "#,##0.###";
			grdHistory.RootTable.Columns["TotalRecOutQty"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["TotalRecOutQty"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["TotalRecOutQty"].FormatString = "#,##0.###";
			grdHistory.RootTable.Columns["TotalRecOutQty"].TotalFormatString = "#,##0.###";
			grdHistory.RootTable.Columns["SupTotalRecWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["SupTotalRecWeight"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["SupTotalRecWeight"].FormatString = "#,##0.###";
			grdHistory.RootTable.Columns["SupTotalRecWeight"].TotalFormatString = "#,##0.###";
			grdHistory.RootTable.Columns["CusTotalOutWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["CusTotalOutWeight"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["CusTotalOutWeight"].FormatString = "#,##0.###";
			grdHistory.RootTable.Columns["CusTotalOutWeight"].TotalFormatString = "#,##0.###";
			grdHistory.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["BalQty"].FormatString = "#,##0.###";
			grdHistory.RootTable.Columns["BalQty"].TotalFormatString = "#,##0.###";
			grdHistory.RootTable.Columns["BalSupWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["BalSupWeight"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["BalSupWeight"].FormatString = "#,##0.###";
			grdHistory.RootTable.Columns["BalSupWeight"].TotalFormatString = "#,##0.###";
			grdHistory.RootTable.Columns["BalCusWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["BalCusWeight"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["BalCusWeight"].FormatString = "#,##0.###";
			grdHistory.RootTable.Columns["BalCusWeight"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grdHistory);
			if (formright.DoHaveUpdateRights)
			{
				grdHistory.RootTable.Columns.Add("Edit");
				grdHistory.RootTable.Columns["Edit"].Key = "Edit";
				grdHistory.RootTable.Columns["Edit"].Caption = "Edit";
				grdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
				grdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
				grdHistory.RootTable.Columns["Edit"].Width = 50;
			}
			if (formright.DoHavePrintRights)
			{
				grdHistory.RootTable.Columns.Add("Print");
				grdHistory.RootTable.Columns["Print"].Key = "Print";
				grdHistory.RootTable.Columns["Print"].Caption = "1702-Print";
				grdHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
				grdHistory.RootTable.Columns["Print"].ButtonText = "1702-Print";
				grdHistory.RootTable.Columns["Print"].Width = 80;
				if (HasMultiLanguageFeature)
				{
					grdHistory.RootTable.Columns.Add("UrduPrint");
					grdHistory.RootTable.Columns["UrduPrint"].Key = "UrduPrint";
					grdHistory.RootTable.Columns["UrduPrint"].Caption = "1702-Urdu";
					grdHistory.RootTable.Columns["UrduPrint"].ButtonDisplayMode = (CellButtonDisplayMode)3;
					grdHistory.RootTable.Columns["UrduPrint"].ButtonStyle = (ButtonStyle)5;
					grdHistory.RootTable.Columns["UrduPrint"].ButtonText = "1702-Urdu";
					grdHistory.RootTable.Columns["UrduPrint"].Width = 80;
				}
			}
			if (formright.DoHaveUpdateRights && formright.DoHavePrintRights)
			{
				if (HasMultiLanguageFeature)
				{
					grdHistory.RootTable.Columns["Print"].Position = 0;
					grdHistory.RootTable.Columns["UrduPrint"].Position = 1;
					grdHistory.RootTable.Columns["Edit"].Position = 2;
					grdHistory.FrozenColumns = 3;
				}
				else
				{
					grdHistory.RootTable.Columns["Print"].Position = 0;
					grdHistory.RootTable.Columns["Edit"].Position = 1;
					grdHistory.FrozenColumns = 2;
				}
			}
			else if (formright.DoHaveUpdateRights)
			{
				grdHistory.RootTable.Columns["Edit"].Position = 0;
				grdHistory.FrozenColumns = 1;
			}
			else if (formright.DoHavePrintRights)
			{
				if (HasMultiLanguageFeature)
				{
					grdHistory.RootTable.Columns["Print"].Position = 0;
					grdHistory.RootTable.Columns["UrduPrint"].Position = 1;
					grdHistory.FrozenColumns = 2;
				}
				else
				{
					grdHistory.RootTable.Columns["Print"].Position = 0;
					grdHistory.FrozenColumns = 1;
				}
			}
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (formright.DoHaveUpdateRights)
			{
				RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(RecId);
				txtdocdate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit" && formright.DoHaveUpdateRights)
			{
				RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(RecId);
				txtdocdate.Focus();
			}
			if (e.Column.Key == "Print" && formright.DoHavePrintRights)
			{
				RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
				GenerateSlip1702Print(RecId);
			}
			if (e.Column.Key == "UrduPrint" && formright.DoHavePrintRights)
			{
				RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
				GenerateUrduSlip1702Print(RecId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grddetailbind(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			new CommTradeLoadingDelivery();
			DataTable dtdeta = CommTradeLoadingDelivery.FormHistoryDetail(Id);
			if (dtdeta.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("BrokerId", typeof(int));
				dt.Columns.Add("BrokerName", typeof(string));
				dt.Columns.Add("TransporterId", typeof(int));
				dt.Columns.Add("TransporterName", typeof(string));
				dt.Columns.Add("BiltyNo", typeof(string));
				dt.Columns.Add("BiltyDate", typeof(DateTime));
				dt.Columns.Add("BiltyExpense", typeof(decimal));
				dt.Columns.Add("BiltyFreight", typeof(decimal));
				dt.Columns.Add("VehicleNo", typeof(string));
				dt.Columns.Add("VehicleQty", typeof(decimal));
				dt.Columns.Add("VehicleWeight", typeof(decimal));
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("ItemCode", typeof(string));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("CropYearId", typeof(int));
				dt.Columns.Add("CropYear", typeof(string));
				dt.Columns.Add("PackingTypeId", typeof(int));
				dt.Columns.Add("PackingType", typeof(string));
				dt.Columns.Add("UomId", typeof(int));
				dt.Columns.Add("UOM", typeof(string));
				dt.Columns.Add("Qty", typeof(decimal));
				dt.Columns.Add("SupplierId", typeof(int));
				dt.Columns.Add("SupplierName", typeof(string));
				dt.Columns.Add("SupGrossWeight", typeof(decimal));
				dt.Columns.Add("SupPckMtrlWeight", typeof(decimal));
				dt.Columns.Add("SupAdLsWt", typeof(decimal));
				dt.Columns.Add("SupNetWeight", typeof(decimal));
				dt.Columns.Add("SupRate", typeof(decimal));
				dt.Columns.Add("SupRemarks", typeof(string));
				dt.Columns.Add("CustomerId", typeof(int));
				dt.Columns.Add("CustomerName", typeof(string));
				dt.Columns.Add("CusGrossWeight", typeof(decimal));
				dt.Columns.Add("CusPckMtrlWeight", typeof(decimal));
				dt.Columns.Add("CusAdLsWt", typeof(decimal));
				dt.Columns.Add("CusNetWeight", typeof(decimal));
				dt.Columns.Add("CusRate", typeof(decimal));
				dt.Columns.Add("CusRemarks", typeof(string));
				dt.Clear();
				for (int i = 0; i < dtdeta.Rows.Count; i++)
				{
					dt.Rows.Add(dtdeta.Rows[i]["Id"], dtdeta.Rows[i]["BrokerId"], dtdeta.Rows[i]["BrokerName"], dtdeta.Rows[i]["TransporterId"], dtdeta.Rows[i]["TransporterName"], dtdeta.Rows[i]["BiltyNo"], dtdeta.Rows[i]["BiltyDate"], dtdeta.Rows[i]["BiltyExpense"], dtdeta.Rows[i]["BiltyFreight"], dtdeta.Rows[i]["VehicleNo"], dtdeta.Rows[i]["VehicleQty"], dtdeta.Rows[i]["VehicleGrossWeight"], dtdeta.Rows[i]["ItemId"], dtdeta.Rows[i]["ItemCode"], dtdeta.Rows[i]["ItemName"], dtdeta.Rows[i]["CropYearId"], dtdeta.Rows[i]["CropYear"], dtdeta.Rows[i]["PackingTypeId"], dtdeta.Rows[i]["PackingType"], dtdeta.Rows[i]["UomId"], dtdeta.Rows[i]["PackUom"], dtdeta.Rows[i]["Qty"], dtdeta.Rows[i]["SupplierId"], dtdeta.Rows[i]["SupplierName"], dtdeta.Rows[i]["SupGrossWeight"], dtdeta.Rows[i]["SupPckMtrlWeight"], dtdeta.Rows[i]["SupAdLsWt"], dtdeta.Rows[i]["SupNetWeight"], dtdeta.Rows[i]["SupRate"], dtdeta.Rows[i]["SupRemarks"], dtdeta.Rows[i]["CustomerId"], dtdeta.Rows[i]["CustomerName"], dtdeta.Rows[i]["CusGrossWeight"], dtdeta.Rows[i]["CusPckMtrlWeight"], dtdeta.Rows[i]["CusAdLsWt"], dtdeta.Rows[i]["CusNetWeight"], dtdeta.Rows[i]["CusRate"], dtdeta.Rows[i]["CusRemarks"]);
				}
				grdHistoryDetail.DataSource = dt;
				grdHistoryDetail.RetrieveStructure();
				grddetailsetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grddetailsetting()
	{
		try
		{
			grdHistoryDetail.RootTable.Columns["Id"].Visible = false;
			grdHistoryDetail.RootTable.Columns["CropYearId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["TransporterId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["ItemId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["PackingTypeId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["UomId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["SupplierId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["BrokerId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["CustomerId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["CusGrossWeight"].Caption = "Gross Weight";
			grdHistoryDetail.RootTable.Columns["CusAdLsWt"].Caption = "Adls Weight";
			grdHistoryDetail.RootTable.Columns["CusPckMtrlWeight"].Caption = "PM Weight";
			grdHistoryDetail.RootTable.Columns["CusNetWeight"].Caption = "Net Weight";
			grdHistoryDetail.RootTable.Columns["CusRate"].Caption = "Rate";
			grdHistoryDetail.RootTable.Columns["SupGrossWeight"].Caption = "Gross Weight";
			grdHistoryDetail.RootTable.Columns["SupAdLsWt"].Caption = "Adls Weight";
			grdHistoryDetail.RootTable.Columns["SupPckMtrlWeight"].Caption = "PM Weight";
			grdHistoryDetail.RootTable.Columns["SupNetWeight"].Caption = "Net Weight";
			grdHistoryDetail.RootTable.Columns["SupRate"].Caption = "Rate";
			grdHistoryDetail.RootTable.Columns["BiltyExpense"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["BiltyExpense"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["BiltyExpense"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistoryDetail.RootTable.Columns["BiltyExpense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistoryDetail.RootTable.Columns["BiltyFreight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["BiltyFreight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["BiltyFreight"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistoryDetail.RootTable.Columns["BiltyFreight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistoryDetail.RootTable.Columns["VehicleWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["VehicleWeight"].FormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["VehicleWeight"].TotalFormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["VehicleQty"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["VehicleQty"].FormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["VehicleQty"].TotalFormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["SupRate"].AggregateFunction = (AggregateFunction)3;
			grdHistoryDetail.RootTable.Columns["SupRate"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["SupRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryDetail.RootTable.Columns["SupRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryDetail.RootTable.Columns["CusRate"].AggregateFunction = (AggregateFunction)3;
			grdHistoryDetail.RootTable.Columns["CusRate"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["CusRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryDetail.RootTable.Columns["CusRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryDetail.RootTable.Columns["SupGrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["SupGrossWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["SupGrossWeight"].FormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["SupGrossWeight"].TotalFormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["SupPckMtrlWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["SupPckMtrlWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["SupPckMtrlWeight"].FormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["SupPckMtrlWeight"].TotalFormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["SupAdLsWt"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["SupAdLsWt"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["SupAdLsWt"].FormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["SupAdLsWt"].TotalFormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["SupNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["SupNetWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["SupNetWeight"].FormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["SupNetWeight"].TotalFormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["CusGrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["CusGrossWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["CusGrossWeight"].FormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["CusGrossWeight"].TotalFormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["CusPckMtrlWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["CusPckMtrlWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["CusPckMtrlWeight"].FormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["CusPckMtrlWeight"].TotalFormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["CusAdLsWt"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["CusAdLsWt"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["CusAdLsWt"].FormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["CusAdLsWt"].TotalFormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["CusNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["CusNetWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["CusNetWeight"].FormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["CusNetWeight"].TotalFormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grdHistoryDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt((object)item.Cells["Id"].Value.ToString().ToString()), 1702);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				grddetailbind(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnResetHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDateHistory.Value = DateTime.Now;
			FromDocNoHistory.Clear();
			ToDocNoHistory.Clear();
			grdHistory.DataSource = null;
			grdHistoryDetail.DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
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

	private void CommTradeLoadingDelivery_KeyDown(object sender, KeyEventArgs e)
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
					txtdocdate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					((Control)(object)grdHistory).Focus();
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
				if (e.Control && e.Shift && e.KeyCode == Keys.Delete && formright.DoHaveCanDelete && btnDelete.Enabled)
				{
					btnDelete_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.S && btnSave.Visible && btnSave.Enabled && tabControl1.SelectedIndex == 0)
				{
					btnSave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled && UpdateMode)
				{
					btnUpdate_Click(null, null);
				}
				if (HasMultiLanguageFeature)
				{
					if (e.Alt && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1) && formright.DoHavePrintRights)
					{
						btnPrint_Click(null, null);
					}
					if (e.Alt && (e.KeyCode == Keys.NumPad2 || e.KeyCode == Keys.D2) && formright.DoHavePrintRights)
					{
						btnUrduPrint_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.P && btnPrint.Visible && btnPrint.Enabled)
				{
					btnPrint_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N && tabControl1.SelectedIndex == 0)
				{
					btnNew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					txtdocdate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				if (e.Control && e.KeyCode == Keys.F12)
				{
					AT.Show();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					cmbitem.Focus();
				}
				if (e.Control && e.Shift && e.KeyCode == Keys.L)
				{
					CmbMultiLanguage.Focus();
				}
				if (e.Control && e.KeyCode == Keys.NumPad2)
				{
					if (rdTemplate1.Checked)
					{
						rdTemplate2.Checked = true;
					}
					else
					{
						rdTemplate1.Checked = true;
					}
					if (rdTemplate1.Checked)
					{
						ExpenseGridsInTabOrinFront = false;
					}
					else
					{
						ExpenseGridsInTabOrinFront = true;
					}
					ChangeTemplate(ExpenseGridsInTabOrinFront);
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnShowHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					FromDateHistory.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdHistoryDetail).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					((Control)(object)grdHistory).Focus();
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnResetHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefreshHistory_Click(null, null);
				}
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
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			if (tabControl1.SelectedIndex == 0)
			{
				dt.Rows.Add("Ctrl+S", "For Save");
				dt.Rows.Add("F12", "For Save As");
				dt.Rows.Add("Ctrl+U", "For Update");
				dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
				if (HasMultiLanguageFeature)
				{
					dt.Rows.Add("Ctrl+Shift+L ", "For Focus On MultiLanguageCombo in Form Tab");
					dt.Rows.Add("Alt+1", "For 1702-Print");
					dt.Rows.Add("Alt+2", "For 1702-Urdu Print");
				}
				else
				{
					dt.Rows.Add("Ctrl+P", "For Print");
				}
				dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
				dt.Rows.Add("Ctrl+F10", "For Open Attachments");
				dt.Rows.Add("Ctrl+Num2", "For Template Change");
				dt.Rows.Add("Ctrl+D", "For Adding an row in Focused Grid");
				dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
				dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
				dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Item in Detail Box");
				dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Update Record");
				dt.Rows.Add("Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link");
			}
			else
			{
				dt.Rows.Add("Ctrl+S", "For Show");
				dt.Rows.Add("Ctrl+F5", "For Focus on From Date");
				dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
				dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Main Grid");
				dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Update Record");
				dt.Rows.Add("Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link");
			}
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshortcutkeys_Click(object sender, EventArgs e)
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0605: Unknown result type (might be due to invalid IL or missing references)
		//IL_060c: Expected O, but got Unknown
		//IL_0557: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Expected O, but got Unknown
		//IL_0a5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)item.Cells["Id"].Value.ToString());
				if (grd.CurrentColumn.Key == "Edit")
				{
					grd_DoubleClick(null, null);
				}
				if (grd.CurrentColumn.Key == "Delete")
				{
					if (updateDetailIndex > -1)
					{
						MessageBox.Show("Please Reset Detail Box Data If You want to Delete Any row from the Grid....");
						return;
					}
					GridEXRow r2 = grd.CurrentRow;
					if (Conversion.ToInt(r2.Cells["Id"].Value) > 0)
					{
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						CommTradeLoadingDeliveryDetail vd2 = new CommTradeLoadingDeliveryDetail();
						vd2.Id = Conversion.ToInt(r2.Cells["Id"].Value);
						vd2.BrokerId = Conversion.ToInt(r2.Cells["BrokerId"].Value);
						vd2.DocDateDt = Conversion.ToDateTime((object)txtdocdate.Value);
						vd2.TransporterId = Conversion.ToInt(r2.Cells["TransporterId"].Value);
						vd2.BiltyNo = Conversion.ToString(r2.Cells["BiltyNo"].Value);
						vd2.BiltyDate = Conversion.ToDateTime(r2.Cells["BiltyDate"].Value);
						vd2.BiltyExpense = Conversion.ToDecimal(r2.Cells["BiltyExpense"].Value);
						vd2.VehicleNo = Conversion.ToString(r2.Cells["VehicleNo"].Value);
						vd2.BiltyFreight = Conversion.ToDecimal(r2.Cells["BiltyFreight"].Value);
						vd2.VehicleQty = Conversion.ToDecimal(r2.Cells["VehicleQty"].Value);
						vd2.VehicleGrossWeight = Conversion.ToDecimal(r2.Cells["VehicleWeight"].Value);
						vd2.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
						vd2.CropYearId = Conversion.ToInt(r2.Cells["CropYearId"].Value);
						vd2.PackingTypeId = Conversion.ToInt((object)r2.Cells["PackingTypeId"].Text);
						vd2.UomId = Conversion.ToInt((object)r2.Cells["UomId"].Text);
						vd2.ItemSupQty = Conversion.ToDecimal(r2.Cells["ItemSupQty"].Value);
						vd2.ItemSupWeight = Conversion.ToDecimal(r2.Cells["ItemSupWeight"].Value);
						vd2.SupplierId = Conversion.ToInt(r2.Cells["SupplierId"].Value);
						vd2.SupGrossWeight = Conversion.ToDecimal(r2.Cells["SupGrossWeight"].Value);
						vd2.SupPckMtrlWeight = Conversion.ToDecimal(r2.Cells["SupPckMtrlWeight"].Value);
						vd2.SupAdLsWt = Conversion.ToDecimal(r2.Cells["SupAdLsWt"].Value);
						vd2.SupNetWeight = Conversion.ToDecimal(r2.Cells["SupNetWeight"].Value);
						vd2.SupRate = Conversion.ToDecimal(r2.Cells["SupRate"].Value);
						vd2.SupRemarks = Conversion.ToString(r2.Cells["SupRemarks"].Value);
						vd2.Qty = Conversion.ToDecimal(r2.Cells["Qty"].Value);
						vd2.CustomerId = Conversion.ToInt(r2.Cells["CustomerId"].Value);
						vd2.CusGrossWeight = Conversion.ToDecimal(r2.Cells["CusGrossWeight"].Value);
						vd2.CusPckMtrlWeight = Conversion.ToDecimal(r2.Cells["CusPckMtrlWeight"].Value);
						vd2.CusAdLsWt = Conversion.ToDecimal(r2.Cells["CusAdLsWt"].Value);
						vd2.CusNetWeight = Conversion.ToDecimal(r2.Cells["CusNetWeight"].Value);
						vd2.CusRate = Conversion.ToDecimal(r2.Cells["CusRate"].Value);
						vd2.CusRemarks = Conversion.ToString(r2.Cells["CusRemarks"].Value);
						vd2.ActionTypeId = 3;
						lstRemoveRecord.Add(vd2);
						dtDetail.Rows[r2.RowIndex].Delete();
						if (r2 != null && (int)r2.RowType == 0)
						{
							r2.Delete();
						}
						grd.UpdateData();
					}
					else if (r2 != null && (int)r2.RowType == 0)
					{
						r2.Delete();
					}
				}
				EnableDisableVehicleInfo();
				VehicleInfoFillOnRowRemove();
				DetailSupCusForRevenueGrid();
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
			if (!e.Control || e.KeyCode != Keys.Delete)
			{
				return;
			}
			if (updateDetailIndex > -1)
			{
				MessageBox.Show("Please Reset Detail Box Data If You want to Delete Any row from the Grid....");
				return;
			}
			GridEXRow r = grd.CurrentRow;
			if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				CommTradeLoadingDeliveryDetail vd = new CommTradeLoadingDeliveryDetail();
				vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
				vd.BrokerId = Conversion.ToInt(r.Cells["BrokerId"].Value);
				vd.DocDateDt = Conversion.ToDateTime((object)txtdocdate.Value);
				vd.TransporterId = Conversion.ToInt(r.Cells["TransporterId"].Value);
				vd.BiltyNo = Conversion.ToString(r.Cells["BiltyNo"].Value);
				vd.BiltyDate = Conversion.ToDateTime(r.Cells["BiltyDate"].Value);
				vd.BiltyExpense = Conversion.ToDecimal(r.Cells["BiltyExpense"].Value);
				vd.VehicleNo = Conversion.ToString(r.Cells["VehicleNo"].Value);
				vd.BiltyFreight = Conversion.ToDecimal(r.Cells["BiltyFreight"].Value);
				vd.VehicleQty = Conversion.ToDecimal(r.Cells["VehicleQty"].Value);
				vd.VehicleGrossWeight = Conversion.ToDecimal(r.Cells["VehicleWeight"].Value);
				vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				vd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
				vd.PackingTypeId = Conversion.ToInt((object)r.Cells["PackingTypeId"].Text);
				vd.UomId = Conversion.ToInt((object)r.Cells["UomId"].Text);
				vd.ItemSupQty = Conversion.ToDecimal(r.Cells["ItemSupQty"].Value);
				vd.ItemSupWeight = Conversion.ToDecimal(r.Cells["ItemSupWeight"].Value);
				vd.SupplierId = Conversion.ToInt(r.Cells["SupplierId"].Value);
				vd.SupGrossWeight = Conversion.ToDecimal(r.Cells["SupGrossWeight"].Value);
				vd.SupPckMtrlWeight = Conversion.ToDecimal(r.Cells["SupPckMtrlWeight"].Value);
				vd.SupAdLsWt = Conversion.ToDecimal(r.Cells["SupAdLsWt"].Value);
				vd.SupNetWeight = Conversion.ToDecimal(r.Cells["SupNetWeight"].Value);
				vd.SupRate = Conversion.ToDecimal(r.Cells["SupRate"].Value);
				vd.SupRemarks = Conversion.ToString(r.Cells["SupRemarks"].Value);
				vd.Qty = Conversion.ToDecimal(r.Cells["Qty"].Value);
				vd.CustomerId = Conversion.ToInt(r.Cells["CustomerId"].Value);
				vd.CusGrossWeight = Conversion.ToDecimal(r.Cells["CusGrossWeight"].Value);
				vd.CusPckMtrlWeight = Conversion.ToDecimal(r.Cells["CusPckMtrlWeight"].Value);
				vd.CusAdLsWt = Conversion.ToDecimal(r.Cells["CusAdLsWt"].Value);
				vd.CusNetWeight = Conversion.ToDecimal(r.Cells["CusNetWeight"].Value);
				vd.CusRate = Conversion.ToDecimal(r.Cells["CusRate"].Value);
				vd.CusRemarks = Conversion.ToString(r.Cells["CusRemarks"].Value);
				vd.ActionTypeId = 3;
				lstRemoveRecord.Add(vd);
				dtDetail.Rows[r.RowIndex].Delete();
				if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				grd.UpdateData();
			}
			else if (r != null && (int)r.RowType == 0)
			{
				r.Delete();
			}
			EnableDisableVehicleInfo();
			VehicleInfoFillOnRowRemove();
			DetailSupCusForRevenueGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdHistory.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				int Id = Conversion.ToInt((object)item.Cells["Id"].Value.ToString());
				if (grdHistory.CurrentColumn.Key == "Edit")
				{
					Id = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
					ReadById(Id);
					txtdocdate.Focus();
				}
				if (grdHistory.CurrentColumn.Key == "Print")
				{
					Id = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
					GenerateSlip1702Print(Id);
				}
				if (grdHistory.CurrentColumn.Key == "UrduPrint")
				{
					Id = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
					GenerateUrduSlip1702Print(Id);
				}
				if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt((object)Id.ToString()), 1702);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return)
			{
				if (formright.DoHaveUpdateRights)
				{
					RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
					ReadById(RecId);
					txtdocdate.Focus();
				}
			}
			else if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
			{
				RecId = Conversion.ToInt((object)item.Cells["Id"].Value.ToString());
				GenerateSlip1702Print(RecId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterial_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPackingMaterial.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdPackingMaterial.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdPackingMaterial.CurrentColumn.Key == "Delete")
				{
					dtPackingMaterialDetail.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
					if (dtPackingMaterialDetail.Rows.Count == 0)
					{
						AddRowPackingmaterial();
					}
				}
				if (grdPackingMaterial.CurrentColumn.Key == "Add")
				{
					AddRowPackingmaterial();
				}
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtPackingMaterialDetail.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtPackingMaterialDetail.Rows.Count == 0)
				{
					AddRowPackingmaterial();
				}
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowPackingmaterial();
			}
			if (e.KeyCode == Keys.F1)
			{
				int Id = 0;
				string Name = "";
				if (grdPackingMaterial.CurrentColumn != null && (grdPackingMaterial.CurrentColumn.Key == "ItemId" || grdPackingMaterial.CurrentColumn.Key == "ItemName"))
				{
					GridDtFill();
					Conversion.ToInt(item.Cells["ItemId"].Value);
					GrdPopUp grdPopUp = new GrdPopUp(dtitemForPMGrid, "Id", "ItemName");
					grdPopUp.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp.ShowDialog();
					grdPopUp.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp.ReturnId);
					Name = Conversion.ToString((object)grdPopUp.ReturnName);
					item.Cells["ItemId"].Value = Id;
					item.Cells["ItemName"].Value = Name;
				}
				if (grdPackingMaterial.CurrentColumn != null && (grdPackingMaterial.CurrentColumn.Key == "CreditAccountId" || grdPackingMaterial.CurrentColumn.Key == "CreditAccount"))
				{
					GridDtFill();
					GrdPopUp grdPopUp2 = new GrdPopUp(dtAccountsForPMGrid, "Id", "AccountTitle");
					grdPopUp2.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp2.ShowDialog();
					grdPopUp2.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp2.ReturnId);
					Name = Conversion.ToString((object)grdPopUp2.ReturnName);
					item.Cells["CreditAccountId"].Value = Id;
					item.Cells["CreditAccount"].Value = Name;
				}
				if (grdPackingMaterial.CurrentColumn != null && (grdPackingMaterial.CurrentColumn.Key == "CustomerAccountId" || grdPackingMaterial.CurrentColumn.Key == "CustomerAccount"))
				{
					GridDtFill();
					GrdPopUp grdPopUp3 = new GrdPopUp(dtSupplier, "Id", "SupplierName");
					grdPopUp3.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp3.ShowDialog();
					grdPopUp3.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp3.ReturnId);
					Name = Conversion.ToString((object)grdPopUp3.ReturnName);
					item.Cells["CustomerAccountId"].Value = Id;
					item.Cells["CustomerAccount"].Value = Name;
				}
				if (grdPackingMaterial.CurrentColumn != null && grdPackingMaterial.CurrentColumn.Key == "StockTypeId")
				{
					GridDtFill();
					GrdPopUp grdPopUp4 = new GrdPopUp(dtStockType, "Id", "StockType");
					grdPopUp4.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp4.ShowDialog();
					grdPopUp4.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp4.ReturnId);
					Name = Conversion.ToString((object)grdPopUp4.ReturnName);
					item.Cells["StockTypeId"].Value = Id;
				}
				if (grdPackingMaterial.CurrentColumn != null && grdPackingMaterial.CurrentColumn.Key == "WareHouseId")
				{
					GridDtFill();
					GrdPopUp grdPopUp5 = new GrdPopUp(dtWare, "Id", "WareHouseName");
					grdPopUp5.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp5.ShowDialog();
					grdPopUp5.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp5.ReturnId);
					Name = Conversion.ToString((object)grdPopUp5.ReturnName);
					item.Cells["WareHouseId"].Value = Id;
				}
				CommonServices.GridAutoAdjustmentNew(grdPackingMaterial);
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

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistory).Name);
			if (fs != null)
			{
				grdHistory.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdPaymentTermSupplier).Name);
			if (fs != null)
			{
				grdPaymentTermSupplier.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdPackingMaterial).Name);
			if (fs != null)
			{
				grdPackingMaterial.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void txtbiltyexpense_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtfrieght_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtvehicleweight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtItemQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtsupgrosswt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtSupPckMtrlWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtSupAdLsWt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtSupNetWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtcustgrosswt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtcustpckmtrlwt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtcustadlswt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtcustnetwt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtVehcileQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtCusQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void DetailItemSupplierGrossWeightCalculation()
	{
		try
		{
			decimal PackUOM = default(decimal);
			decimal Qty = default(decimal);
			decimal GrossWeight = default(decimal);
			if (!string.IsNullOrEmpty(txtItemSupQty.Text))
			{
				Qty = Conversion.ToDecimal((object)txtItemSupQty.Text.ToString());
			}
			if (((UltraGridBase)cmbuom).ActiveRow != null && Conversion.ToInt(cmbuom.Value) != 0)
			{
				PackUOM = Conversion.ToDecimal((object)((Control)(object)cmbuom).Text.ToString());
			}
			GrossWeight = Qty * PackUOM;
			txtItemSupGrossWt.Text = GrossWeight.ToString("##,#.###");
			txtItemSupNetWeight.Text = GrossWeight.ToString("##,#.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailCustomerGrossWeightCalculation()
	{
		try
		{
			decimal Qty = default(decimal);
			decimal ItemQty = default(decimal);
			decimal SupGrossWeight = default(decimal);
			decimal GrossWeight = default(decimal);
			if (!string.IsNullOrEmpty(txtItemSupQty.Text))
			{
				ItemQty = Conversion.ToDecimal((object)txtItemSupQty.Text.ToString());
			}
			if (!string.IsNullOrEmpty(txtCusQty.Text))
			{
				Qty = Conversion.ToDecimal((object)txtCusQty.Text.ToString());
			}
			if (!string.IsNullOrEmpty(txtCusQty.Text))
			{
				SupGrossWeight = Conversion.ToDecimal((object)txtItemSupGrossWt.Text.ToString());
			}
			if (SupGrossWeight != 0m && ItemQty != 0m)
			{
				GrossWeight = SupGrossWeight / ItemQty * Qty;
				txtcustgrosswt.Text = GrossWeight.ToString("##,#.###");
				txtcustnetwt.Text = GrossWeight.ToString("##,#.###");
			}
			else
			{
				txtcustgrosswt.Text = 0.ToString("##,#.###");
				txtcustnetwt.Text = 0.ToString("##,#.###");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailSupplierGrossWeightCalculation()
	{
		try
		{
			decimal CusSupQty = default(decimal);
			decimal ItemSupQty = default(decimal);
			decimal ItemSupGrossWeight = default(decimal);
			decimal SupGrossWeight = default(decimal);
			if (!string.IsNullOrEmpty(txtItemSupQty.Text))
			{
				ItemSupQty = Conversion.ToDecimal((object)txtItemSupQty.Text.ToString());
			}
			if (!string.IsNullOrEmpty(txtCusQty.Text))
			{
				CusSupQty = Conversion.ToDecimal((object)txtCusQty.Text.ToString());
			}
			if (!string.IsNullOrEmpty(txtCusQty.Text))
			{
				ItemSupGrossWeight = Conversion.ToDecimal((object)txtItemSupGrossWt.Text.ToString());
			}
			if (ItemSupGrossWeight != 0m && ItemSupQty != 0m)
			{
				SupGrossWeight = ItemSupGrossWeight / ItemSupQty * CusSupQty;
				txtSupGrossWt.Text = SupGrossWeight.ToString("##,#.###");
				txtSupNetWeight.Text = SupGrossWeight.ToString("##,#.###");
			}
			else
			{
				txtSupGrossWt.Text = 0.ToString("##,#.###");
				txtSupNetWeight.Text = 0.ToString("##,#.###");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailItemSupplierNetWeightCalculation()
	{
		try
		{
			decimal SupGrossWeight = default(decimal);
			decimal NetWeightSup = default(decimal);
			decimal AddLessSup = default(decimal);
			decimal WeightCutSup = default(decimal);
			if (txtItemSupAdLsWt.Text != string.Empty && txtItemSupAdLsWt.Text != null)
			{
				AddLessSup = Conversion.ToDecimal((object)txtItemSupAdLsWt.Text.Trim().ToString());
			}
			if (txtItemSupPmWt.Text != string.Empty && txtItemSupPmWt.Text != null)
			{
				WeightCutSup = Conversion.ToDecimal((object)txtItemSupPmWt.Text.Trim().ToString());
			}
			if (txtItemSupGrossWt.Text != string.Empty && txtItemSupGrossWt.Text != null)
			{
				SupGrossWeight = Conversion.ToDecimal((object)txtItemSupGrossWt.Text.Trim().ToString());
			}
			NetWeightSup = SupGrossWeight - WeightCutSup + AddLessSup;
			txtItemSupNetWeight.Text = Conversion.ToDecimal((object)NetWeightSup).ToString("##,#.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailSupplierAllWeightsCalculationBySupWeights()
	{
		try
		{
			decimal ItemSupGrossWeight = default(decimal);
			decimal AddLessItemSup = default(decimal);
			decimal WeightCutItemSup = default(decimal);
			decimal ItemSupQty = default(decimal);
			decimal CusQty = default(decimal);
			if (txtItemSupQty.Text != string.Empty && txtItemSupQty.Text != null)
			{
				ItemSupQty = Conversion.ToDecimal((object)txtItemSupQty.Text.Trim().ToString());
			}
			if (txtCusQty.Text != string.Empty && txtCusQty.Text != null)
			{
				CusQty = Conversion.ToDecimal((object)txtCusQty.Text.Trim().ToString());
			}
			if (txtItemSupAdLsWt.Text != string.Empty && txtItemSupAdLsWt.Text != null)
			{
				AddLessItemSup = Conversion.ToDecimal((object)txtItemSupAdLsWt.Text.Trim().ToString());
			}
			if (txtItemSupPmWt.Text != string.Empty && txtItemSupPmWt.Text != null)
			{
				WeightCutItemSup = Conversion.ToDecimal((object)txtItemSupPmWt.Text.Trim().ToString());
			}
			if (txtItemSupGrossWt.Text != string.Empty && txtItemSupGrossWt.Text != null)
			{
				ItemSupGrossWeight = Conversion.ToDecimal((object)txtItemSupGrossWt.Text.Trim().ToString());
			}
			if (ItemSupGrossWeight > 0m && ItemSupQty > 0m && CusQty > 0m)
			{
				txtSupGrossWt.Text = (ItemSupGrossWeight / ItemSupQty * CusQty).ToString("##,#.###");
			}
			else
			{
				txtSupGrossWt.Text = 0.ToString();
			}
			if (AddLessItemSup != 0m && ItemSupQty > 0m && CusQty > 0m)
			{
				txtSupAdLsWt.Text = (AddLessItemSup / ItemSupQty * CusQty).ToString("##,#.###");
			}
			else
			{
				txtSupAdLsWt.Text = 0.ToString();
			}
			if (WeightCutItemSup != 0m && ItemSupQty > 0m && CusQty > 0m)
			{
				txtSupPmWt.Text = (WeightCutItemSup / ItemSupQty * CusQty).ToString("##,#.###");
			}
			else
			{
				txtSupPmWt.Text = 0.ToString();
			}
			decimal SupGrossWeight = Conversion.ToDecimal((object)txtSupGrossWt.Text);
			decimal SupPmWt = Conversion.ToDecimal((object)txtSupPmWt.Text);
			decimal SupAdlsWt = Conversion.ToDecimal((object)txtSupAdLsWt.Text);
			if (SupGrossWeight > 0m)
			{
				if (SupPmWt != 0m || SupAdlsWt != 0m)
				{
					decimal SupNetWeight = SupGrossWeight - SupPmWt + SupAdlsWt;
					txtSupNetWeight.Text = Conversion.ToDecimal((object)SupNetWeight).ToString("##,#.###");
				}
				else
				{
					txtSupNetWeight.Text = SupGrossWeight.ToString("##,#.###");
				}
			}
			else
			{
				txtSupNetWeight.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailCustomerAllWeightsCalculationBySupWeights()
	{
		try
		{
			decimal SupGrossWeight = default(decimal);
			decimal NetWeightCus = default(decimal);
			decimal AddLessSup = default(decimal);
			decimal WeightCutSup = default(decimal);
			decimal ItemQty = default(decimal);
			decimal CusQty = default(decimal);
			if (txtItemSupQty.Text != string.Empty && txtItemSupQty.Text != null)
			{
				ItemQty = Conversion.ToDecimal((object)txtItemSupQty.Text.Trim().ToString());
			}
			if (txtCusQty.Text != string.Empty && txtCusQty.Text != null)
			{
				CusQty = Conversion.ToDecimal((object)txtCusQty.Text.Trim().ToString());
			}
			if (txtItemSupAdLsWt.Text != string.Empty && txtItemSupAdLsWt.Text != null)
			{
				AddLessSup = Conversion.ToDecimal((object)txtItemSupAdLsWt.Text.Trim().ToString());
			}
			if (txtItemSupPmWt.Text != string.Empty && txtItemSupPmWt.Text != null)
			{
				WeightCutSup = Conversion.ToDecimal((object)txtItemSupPmWt.Text.Trim().ToString());
			}
			if (txtItemSupGrossWt.Text != string.Empty && txtItemSupGrossWt.Text != null)
			{
				SupGrossWeight = Conversion.ToDecimal((object)txtItemSupGrossWt.Text.Trim().ToString());
			}
			if (SupGrossWeight > 0m && ItemQty > 0m && CusQty > 0m)
			{
				txtcustgrosswt.Text = (SupGrossWeight / ItemQty * CusQty).ToString("##,#.###");
			}
			else
			{
				txtcustgrosswt.Text = 0.ToString("##,#.###");
			}
			if (AddLessSup != 0m && ItemQty > 0m && CusQty > 0m)
			{
				txtcustadlswt.Text = (AddLessSup / ItemQty * CusQty).ToString("##,#.###");
			}
			else
			{
				txtcustadlswt.Text = 0.ToString("##,#.###");
			}
			if (WeightCutSup != 0m && ItemQty > 0m && CusQty > 0m)
			{
				txtcustpckmtrlwt.Text = (WeightCutSup / ItemQty * CusQty).ToString("##,#.###");
			}
			decimal CusGrossWeight = Conversion.ToDecimal((object)txtcustgrosswt.Text);
			decimal WeightCutCus = Conversion.ToDecimal((object)txtcustpckmtrlwt.Text);
			decimal AdlsCus = Conversion.ToDecimal((object)txtcustadlswt.Text);
			if (CusGrossWeight > 0m)
			{
				if (WeightCutCus != 0m || AdlsCus > 0m)
				{
					NetWeightCus = CusGrossWeight - WeightCutCus + AdlsCus;
					txtcustnetwt.Text = Conversion.ToDecimal((object)NetWeightCus).ToString("##,#.###");
				}
				else
				{
					NetWeightCus = CusGrossWeight;
					txtcustnetwt.Text = Conversion.ToDecimal((object)NetWeightCus).ToString("##,#.###");
				}
			}
			else
			{
				txtcustnetwt.Text = 0.ToString("##,#.###");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbuom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DetailItemSupplierGrossWeightCalculation();
			DetailSupplierGrossWeightCalculation();
			DetailCustomerGrossWeightCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtItemQty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DetailItemSupplierGrossWeightCalculation();
			DetailSupplierGrossWeightCalculation();
			DetailCustomerGrossWeightCalculation();
			DetailItemSupplierNetWeightCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtsupgrosswt_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DetailItemSupplierNetWeightCalculation();
			DetailSupplierAllWeightsCalculationBySupWeights();
			DetailCustomerAllWeightsCalculationBySupWeights();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtSupAdLsWt_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DetailItemSupplierNetWeightCalculation();
			DetailSupplierAllWeightsCalculationBySupWeights();
			DetailCustomerAllWeightsCalculationBySupWeights();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtSupPckMtrlWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DetailItemSupplierNetWeightCalculation();
			DetailSupplierAllWeightsCalculationBySupWeights();
			DetailCustomerAllWeightsCalculationBySupWeights();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCusQty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DetailSupplierGrossWeightCalculation();
			DetailSupplierAllWeightsCalculationBySupWeights();
			DetailCustomerGrossWeightCalculation();
			DetailCustomerAllWeightsCalculationBySupWeights();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtcustgrosswt_TextChanged(object sender, EventArgs e)
	{
		try
		{
			decimal NetWeightCus = default(decimal);
			decimal AddLessSup = default(decimal);
			decimal WeightCutSup = default(decimal);
			decimal ItemQty = default(decimal);
			decimal CusQty = default(decimal);
			if (txtItemSupQty.Text != string.Empty && txtItemSupQty.Text != null)
			{
				ItemQty = Conversion.ToDecimal((object)txtItemSupQty.Text.Trim().ToString());
			}
			if (txtCusQty.Text != string.Empty && txtCusQty.Text != null)
			{
				CusQty = Conversion.ToDecimal((object)txtCusQty.Text.Trim().ToString());
			}
			if (txtItemSupAdLsWt.Text != string.Empty && txtItemSupAdLsWt.Text != null)
			{
				AddLessSup = Conversion.ToDecimal((object)txtItemSupAdLsWt.Text.Trim().ToString());
			}
			if (txtItemSupPmWt.Text != string.Empty && txtItemSupPmWt.Text != null)
			{
				WeightCutSup = Conversion.ToDecimal((object)txtItemSupPmWt.Text.Trim().ToString());
			}
			if (txtItemSupGrossWt.Text != string.Empty && txtItemSupGrossWt.Text != null)
			{
				Conversion.ToDecimal((object)txtItemSupGrossWt.Text.Trim().ToString());
			}
			if (WeightCutSup != 0m && ItemQty > 0m && CusQty > 0m)
			{
				txtcustpckmtrlwt.Text = (WeightCutSup / ItemQty * CusQty).ToString("##,#.###");
			}
			else
			{
				txtcustpckmtrlwt.Text = 0.ToString("##,#.###");
			}
			if (AddLessSup != 0m && ItemQty > 0m && CusQty > 0m)
			{
				txtcustadlswt.Text = (AddLessSup / ItemQty * CusQty).ToString("##,#.###");
			}
			else
			{
				txtcustadlswt.Text = 0.ToString("##,#.###");
			}
			decimal CusGrossWeight = Conversion.ToDecimal((object)txtcustgrosswt.Text);
			decimal WeightCutCus = Conversion.ToDecimal((object)txtcustpckmtrlwt.Text);
			decimal AdlsCus = Conversion.ToDecimal((object)txtcustadlswt.Text);
			if (CusGrossWeight > 0m)
			{
				if (WeightCutCus != 0m || AdlsCus != 0m)
				{
					NetWeightCus = CusGrossWeight - WeightCutCus + AdlsCus;
					txtcustnetwt.Text = Conversion.ToDecimal((object)NetWeightCus).ToString("##,#.###");
				}
				else
				{
					NetWeightCus = CusGrossWeight;
					txtcustnetwt.Text = Conversion.ToDecimal((object)NetWeightCus).ToString("##,#.###");
				}
			}
			else
			{
				txtcustnetwt.Text = 0.ToString("##,#.###");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtcustpckmtrlwt_TextChanged(object sender, EventArgs e)
	{
		try
		{
			decimal NetWeightCus = default(decimal);
			decimal AddLessSup = default(decimal);
			decimal ItemQty = default(decimal);
			decimal CusQty = default(decimal);
			if (txtItemSupQty.Text != string.Empty && txtItemSupQty.Text != null)
			{
				ItemQty = Conversion.ToDecimal((object)txtItemSupQty.Text.Trim().ToString());
			}
			if (txtCusQty.Text != string.Empty && txtCusQty.Text != null)
			{
				CusQty = Conversion.ToDecimal((object)txtCusQty.Text.Trim().ToString());
			}
			if (txtItemSupAdLsWt.Text != string.Empty && txtItemSupAdLsWt.Text != null)
			{
				AddLessSup = Conversion.ToDecimal((object)txtItemSupAdLsWt.Text.Trim().ToString());
			}
			if (txtItemSupGrossWt.Text != string.Empty && txtItemSupGrossWt.Text != null)
			{
				Conversion.ToDecimal((object)txtItemSupGrossWt.Text.Trim().ToString());
			}
			if (AddLessSup != 0m && ItemQty > 0m && CusQty > 0m)
			{
				txtcustadlswt.Text = (AddLessSup / ItemQty * CusQty).ToString("##,#.###");
			}
			else
			{
				txtcustadlswt.Text = 0.ToString("##,#.###");
			}
			decimal CusGrossWeight = Conversion.ToDecimal((object)txtcustgrosswt.Text);
			decimal WeightCutCus = Conversion.ToDecimal((object)txtcustpckmtrlwt.Text);
			decimal AdlsCus = Conversion.ToDecimal((object)txtcustadlswt.Text);
			if (CusGrossWeight > 0m)
			{
				if (WeightCutCus != 0m || AdlsCus != 0m)
				{
					NetWeightCus = CusGrossWeight - WeightCutCus + AdlsCus;
					txtcustnetwt.Text = Conversion.ToDecimal((object)NetWeightCus).ToString("##,#.###");
				}
				else
				{
					NetWeightCus = CusGrossWeight;
					txtcustnetwt.Text = Conversion.ToDecimal((object)NetWeightCus).ToString("##,#.###");
				}
			}
			else
			{
				txtcustnetwt.Text = 0.ToString("##,#.###");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtcustadlswt_TextChanged(object sender, EventArgs e)
	{
		try
		{
			decimal NetWeightCus = default(decimal);
			decimal CusGrossWeight = Conversion.ToDecimal((object)txtcustgrosswt.Text);
			decimal WeightCutCus = Conversion.ToDecimal((object)txtcustpckmtrlwt.Text);
			decimal AdlsCus = Conversion.ToDecimal((object)txtcustadlswt.Text);
			if (CusGrossWeight > 0m)
			{
				if (WeightCutCus != 0m || AdlsCus != 0m)
				{
					NetWeightCus = CusGrossWeight - WeightCutCus + AdlsCus;
					txtcustnetwt.Text = Conversion.ToDecimal((object)NetWeightCus).ToString("##,#.###");
				}
				else
				{
					NetWeightCus = CusGrossWeight;
					txtcustnetwt.Text = Conversion.ToDecimal((object)NetWeightCus).ToString("##,#.###");
				}
			}
			else
			{
				txtcustnetwt.Text = 0.ToString("##,#.###");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtSupRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			txtCusRate.Text = txtSupRate.Text;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtbiltyexpense_TextChanged(object sender, EventArgs e)
	{
		try
		{
			txtfrieght.Text = txtbiltyexpense.Text;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateSlip1702Print(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateSlip1702Print(int PrintId)
	{
		try
		{
			CommonServices.CommissionTradeLoadingDelivery_Slip_1702(PrintId, 0);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUrduPrint_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				GenerateUrduSlip1702Print(RecId);
				return;
			}
			throw new Exception("Record Not Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateUrduSlip1702Print(int PrintId)
	{
		try
		{
			if (Conversion.ToInt(CmbMultiLanguage.Value) <= 0)
			{
				tabControl1.SelectedIndex = 0;
				CmbMultiLanguage.Focus();
				MessageBox.Show("Please Select Language!", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else
			{
				CommonServices.CommissionTradeLoadingDelivery_Slip_1702A(PrintId, Conversion.ToInt(CmbMultiLanguage.Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DisableVehcileInfo()
	{
		try
		{
			txtvehicleno.Enabled = false;
			txtBiltyNo.Enabled = false;
			txtbiltydate.Enabled = false;
			((Control)(object)cmbTransporter).Enabled = false;
			txtfrieght.Enabled = false;
			txtbiltyexpense.Enabled = false;
			txtvehicleweight.Enabled = false;
			VehicleInfoEnable = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool VehcileBalanceWeightCheckBeforeRemoveRow()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		bool Allow = true;
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				GridEXRow r = grd.CurrentRow;
				if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
				{
					string CurrentVehicleNo = Conversion.ToString((object)r.Cells["VehicleNo"].Text);
					string VehicleNos = "";
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow row2 in rows)
					{
						if (Conversion.ToString((object)row2.Cells["VehicleNo"].Text) != CurrentVehicleNo)
						{
							VehicleNos = VehicleNos + "," + Conversion.ToString((object)row2.Cells["VehicleNo"].Text);
						}
					}
					foreach (string No in (from s in VehicleNos.Split(',')
						where !string.IsNullOrEmpty(s)
						select s).Distinct())
					{
						decimal CurrentVehicleGrossWeight = (from row in dtDetail.AsEnumerable()
							where row.Field<string>("VehicleNo") == No
							select row.Field<decimal>("VehicleWeight")).FirstOrDefault();
						if ((from row in dtDetail.AsEnumerable()
							where row.Field<string>("VehicleNo") == No
							select row).Sum((DataRow row) => row.Field<decimal>("SupGrossWeight")) != CurrentVehicleGrossWeight)
						{
							Allow = false;
							throw new Exception("You cannot delete the row of this vehicle '" + CurrentVehicleNo + "' because the weight of vehicle '" + No + "' has not been completed yet.");
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return Allow;
	}

	private void EnableDisableItemAndSupllierInfo()
	{
		try
		{
			if (Conversion.ToInt((object)grd.GetRows().Count()) > 0)
			{
				decimal CurrentItemSupGrossWeight = Conversion.ToDecimal((object)txtItemSupGrossWt.Text);
				int CurrentItemId = Conversion.ToInt(cmbitem.Value);
				int CurrentSupId = Conversion.ToInt(cmbsupplier.Value);
				Conversion.ToDecimal((object)txtcustgrosswt.Text);
				decimal GridTotalItemSupGrossWeight = default(decimal);
				GridTotalItemSupGrossWeight = ((updateDetailIndex <= -1) ? (from r in dtDetail.AsEnumerable()
					where r.Field<int>("ItemId") == CurrentItemId && r.Field<int>("SupplierId") == CurrentSupId
					select r).Sum((DataRow r) => r.Field<decimal>("SupGrossWeight")) : (from item in dtDetail.AsEnumerable().Select((DataRow row, int index) => new
					{
						Row = row,
						Index = index
					})
					where item.Row.Field<int>("ItemId") == CurrentItemId && item.Row.Field<int>("SupplierId") == CurrentSupId && item.Index != updateDetailIndex
					select item).Sum(item => item.Row.Field<decimal>("SupGrossWeight")));
				if (!(GridTotalItemSupGrossWeight == 0m))
				{
					_ = CurrentItemSupGrossWeight - GridTotalItemSupGrossWeight == 0m;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool IsTotalGrossWeightExceeded()
	{
		try
		{
			if (Conversion.ToInt((object)grd.GetRows().Count()) > 0)
			{
				if (updateDetailIndex > -1)
				{
					decimal CurrentVehicleGrossWeight = Conversion.ToDecimal((object)txtvehicleweight.Text);
					string CurrentVehicleNo = Conversion.ToString((object)txtvehicleno.Text);
					decimal CurrentItemSupGrossWeight = Conversion.ToDecimal((object)txtItemSupGrossWt.Text);
					int CurrentItemId = Conversion.ToInt(cmbitem.Value);
					int CurrentSupId = Conversion.ToInt(cmbsupplier.Value);
					decimal GridTotalSupGrossWtOfCurVehicle = dtDetail.AsEnumerable().Where((DataRow row, int index) => row.Field<string>("VehicleNo") == CurrentVehicleNo && index != updateDetailIndex).Sum((DataRow row) => row.Field<decimal>("SupGrossWeight"));
					decimal GridTotalCurrItemSupGrossWeight = dtDetail.AsEnumerable().Where((DataRow row, int index) => row.Field<int>("ItemId") == CurrentItemId && row.Field<int>("SupplierId") == CurrentSupId && index != updateDetailIndex).Sum((DataRow row) => row.Field<decimal>("SupGrossWeight"));
					if (GridTotalSupGrossWtOfCurVehicle + CurrentItemSupGrossWeight - GridTotalCurrItemSupGrossWeight > CurrentVehicleGrossWeight)
					{
						MessageBox.Show("You can Not Add Total Gross Weight More than Vehicle Weight Against Current Vehicle\nYou are Adding Weight = " + CurrentItemSupGrossWeight + " But Balance Weight Against this Vehcile is = " + (CurrentVehicleGrossWeight - GridTotalSupGrossWtOfCurVehicle));
						return false;
					}
				}
				else
				{
					decimal currentVehicleGrossWeight = Conversion.ToDecimal((object)txtvehicleweight.Text);
					string currentVehicleNo = Conversion.ToString((object)txtvehicleno.Text);
					decimal currentItemSupGrossWeight = Conversion.ToDecimal((object)txtItemSupGrossWt.Text);
					int currentItemId = Conversion.ToInt(cmbitem.Value);
					int currentSupId = Conversion.ToInt(cmbsupplier.Value);
					decimal gridTotalSupGrossWtOfCurVehicle = (from r in dtDetail.AsEnumerable()
						where r.Field<string>("VehicleNo") == currentVehicleNo
						select r).Sum((DataRow r) => r.Field<decimal>("SupGrossWeight"));
					decimal gridTotalCurrItemSupGrossWeight = (from r in dtDetail.AsEnumerable()
						where r.Field<int>("ItemId") == currentItemId && r.Field<int>("SupplierId") == currentSupId
						select r).Sum((DataRow r) => r.Field<decimal>("SupGrossWeight"));
					if (gridTotalSupGrossWtOfCurVehicle + currentItemSupGrossWeight - gridTotalCurrItemSupGrossWeight > currentVehicleGrossWeight)
					{
						MessageBox.Show($"You can Not Add Total Gross Weight More than Vehicle Weight Against Current Vehicle\nYou are Adding Supplier Weight = {currentItemSupGrossWeight} But Balance Weight Against this Vehcile is = {currentVehicleGrossWeight - gridTotalSupGrossWtOfCurVehicle}");
						return false;
					}
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return true;
	}

	private void ItemSupplierInfoFillOnRowRemove()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grd.GetRows().Count() <= 0)
			{
				return;
			}
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			string SupplierIds = "";
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow row2 in rows)
			{
				SupplierIds = SupplierIds + "," + Conversion.ToString((object)row2.Cells["SupplierId"].Text);
			}
			if (string.IsNullOrEmpty(SupplierIds))
			{
				return;
			}
			foreach (int SupId in from id in SupplierIds.Split(',').Distinct()
				select Conversion.ToInt((object)id))
			{
				string ItemIdsOfSupplier = "";
				rows = grd.GetRows();
				foreach (GridEXRow ro in rows)
				{
					if (Conversion.ToInt((object)ro.Cells["SupplierId"]) == SupId)
					{
						ItemIdsOfSupplier = ItemIdsOfSupplier + "," + Conversion.ToString((object)ro.Cells["ItemId"].Text);
					}
				}
				foreach (int ItemId in from id in ItemIdsOfSupplier.Split(',').Distinct()
					select Conversion.ToInt((object)id))
				{
					(from row in dtDetail.AsEnumerable()
						where row.Field<int>("ItemId") == ItemId && row.Field<int>("SupplierId") == SupId
						select row).Sum((DataRow row) => row.Field<decimal>("SupGrossWeight"));
					(from row in dtDetail.AsEnumerable()
						where row.Field<int>("ItemId") == ItemId && row.Field<int>("SupplierId") == SupId
						select row).Sum((DataRow row) => row.Field<decimal>("CusGrossWeight"));
					(from row in dtDetail.AsEnumerable()
						where row.Field<int>("ItemId") == ItemId && row.Field<int>("SupplierId") == SupId
						select row.Field<int>("CropYearId")).FirstOrDefault();
					(from row in dtDetail.AsEnumerable()
						where row.Field<int>("ItemId") == ItemId && row.Field<int>("SupplierId") == SupId
						select row.Field<int>("PackingTypeId")).FirstOrDefault();
					(from row in dtDetail.AsEnumerable()
						where row.Field<int>("ItemId") == ItemId && row.Field<int>("SupplierId") == SupId
						select row.Field<int>("UomId")).FirstOrDefault();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("An error occurred: " + ex.Message);
		}
	}

	private void VehcilSummary()
	{
		try
		{
			decimal GridTotalSupGrossVehicle = default(decimal);
			if (dtDetail.Rows.Count > 0)
			{
				GridTotalSupGrossVehicle = (from r in dtDetail.AsEnumerable()
					where r.Field<string>("VehicleNo") == Conversion.ToString((object)txtvehicleno.Text)
					select r).Sum((DataRow r) => r.Field<decimal>("SupGrossWeight"));
			}
			if (dtVehicleSummary.Rows.Count > 0)
			{
				dtVehicleSummary.Columns.Add("VehcileNo", typeof(string));
				dtVehicleSummary.Columns.Add("VehicleGrossWeight", typeof(string));
				dtVehicleSummary.Columns.Add("VehicleBalanceWeight", typeof(string));
				if (!dtVehicleSummary.AsEnumerable().Any((DataRow r) => r.Field<string>("VehcileNo") == Conversion.ToString((object)txtvehicleno.Text)))
				{
					dtVehicleSummary.Rows.Add(Conversion.ToString((object)txtvehicleno.Text), Conversion.ToDecimal((object)txtvehicleweight.Text), Conversion.ToDecimal((object)txtvehicleweight.Text) - GridTotalSupGrossVehicle);
				}
			}
			else
			{
				dtVehicleSummary.Rows.Add(Conversion.ToString((object)txtvehicleno.Text), Conversion.ToDecimal((object)txtvehicleweight.Text), Conversion.ToDecimal((object)txtvehicleweight.Text) - GridTotalSupGrossVehicle);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnLoadingExpenseForm_Click(object sender, EventArgs e)
	{
		try
		{
			new CommTradeLoadingDeliveryExpenses(UserAccount).Show();
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
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Expected O, but got Unknown
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Expected O, but got Unknown
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Expected O, but got Unknown
		//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d3: Expected O, but got Unknown
		//IL_064d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0657: Expected O, but got Unknown
		//IL_0679: Unknown result type (might be due to invalid IL or missing references)
		//IL_0683: Expected O, but got Unknown
		//IL_068f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0699: Expected O, but got Unknown
		//IL_06d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06db: Expected O, but got Unknown
		//IL_0708: Unknown result type (might be due to invalid IL or missing references)
		//IL_0712: Expected O, but got Unknown
		//IL_074a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0754: Expected O, but got Unknown
		//IL_0831: Unknown result type (might be due to invalid IL or missing references)
		//IL_083b: Expected O, but got Unknown
		//IL_083c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0846: Expected O, but got Unknown
		//IL_08b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bf: Expected O, but got Unknown
		//IL_08d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e0: Expected O, but got Unknown
		//IL_5722: Unknown result type (might be due to invalid IL or missing references)
		//IL_572c: Expected O, but got Unknown
		//IL_5739: Unknown result type (might be due to invalid IL or missing references)
		//IL_5743: Expected O, but got Unknown
		//IL_5750: Unknown result type (might be due to invalid IL or missing references)
		//IL_575a: Expected O, but got Unknown
		//IL_5aa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_5aaf: Expected O, but got Unknown
		//IL_5abc: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ac6: Expected O, but got Unknown
		//IL_5ed8: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ee2: Expected O, but got Unknown
		//IL_81bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_81c9: Expected O, but got Unknown
		//IL_81ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_81f7: Expected O, but got Unknown
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance56 = new Appearance();
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
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		GridEXLayout grdPaymentTermSupplier_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPackingMaterial_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
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
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		GridEXLayout grdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.CmbMultiLanguage = new UltraCombo();
		this.labelmultilang = new System.Windows.Forms.Label();
		this.ChkUrduPrintPreview = new System.Windows.Forms.CheckBox();
		this.chkPrint = new System.Windows.Forms.CheckBox();
		this.label23 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnUrduPrint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnLoadingExpenseForm = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.rdTemplate2 = new System.Windows.Forms.RadioButton();
		this.rdTemplate1 = new System.Windows.Forms.RadioButton();
		this.txtRemarksHeader = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.txtmanualno = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.txtdocdate = new System.Windows.Forms.DateTimePicker();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.cmbBroker = new UltraCombo();
		this.label27 = new System.Windows.Forms.Label();
		this.cmbTransporter = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.txtbiltyexpense = new System.Windows.Forms.TextBox();
		this.txtfrieght = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtvehicleweight = new System.Windows.Forms.TextBox();
		this.txtbiltydate = new System.Windows.Forms.DateTimePicker();
		this.lbl = new System.Windows.Forms.Label();
		this.txtBiltyNo = new System.Windows.Forms.TextBox();
		this.label48 = new System.Windows.Forms.Label();
		this.txtvehicleno = new System.Windows.Forms.TextBox();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.label34 = new System.Windows.Forms.Label();
		this.txtcustremarks = new System.Windows.Forms.TextBox();
		this.label35 = new System.Windows.Forms.Label();
		this.txtcustnetwt = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.txtcustadlswt = new System.Windows.Forms.TextBox();
		this.label37 = new System.Windows.Forms.Label();
		this.txtCusRate = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.txtcustpckmtrlwt = new System.Windows.Forms.TextBox();
		this.label38 = new System.Windows.Forms.Label();
		this.txtCusQty = new System.Windows.Forms.TextBox();
		this.txtcustgrosswt = new System.Windows.Forms.TextBox();
		this.cmbcustomer = new UltraCombo();
		this.label39 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.txtsupremarks = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.txtItemSupNetWeight = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.txtItemSupAdLsWt = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.txtItemSupPmWt = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.txtItemSupGrossWt = new System.Windows.Forms.TextBox();
		this.cmbsupplier = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.txtItemSupQty = new System.Windows.Forms.TextBox();
		this.cmbuom = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.cmbitem = new UltraCombo();
		this.labItemName = new System.Windows.Forms.Label();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.PanelExxpenses = new System.Windows.Forms.Panel();
		this.PanelOtherRevenue = new System.Windows.Forms.Panel();
		this.grdPaymentTermSupplier = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label21 = new System.Windows.Forms.Label();
		this.PanelPmExpense = new System.Windows.Forms.Panel();
		this.grdPackingMaterial = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.label36 = new System.Windows.Forms.Label();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabDetail = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label46 = new System.Windows.Forms.Label();
		this.tabPM = new System.Windows.Forms.TabPage();
		this.tabRevenue = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.label32 = new System.Windows.Forms.Label();
		this.rdSearchByCode = new System.Windows.Forms.RadioButton();
		this.txtSupNetWeight = new System.Windows.Forms.TextBox();
		this.rdSearchByName = new System.Windows.Forms.RadioButton();
		this.label22 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.txtSupAdLsWt = new System.Windows.Forms.TextBox();
		this.label30 = new System.Windows.Forms.Label();
		this.txtSupRate = new System.Windows.Forms.TextBox();
		this.txtSupPmWt = new System.Windows.Forms.TextBox();
		this.label28 = new System.Windows.Forms.Label();
		this.txtSupGrossWt = new System.Windows.Forms.TextBox();
		this.cmbcropYear = new UltraCombo();
		this.CmbPackingType = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.groupBox6 = new System.Windows.Forms.GroupBox();
		this.txtVehcileQty = new System.Windows.Forms.TextBox();
		this.label26 = new System.Windows.Forms.Label();
		this.btnAddDetail = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.panel4 = new System.Windows.Forms.Panel();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdHistoryDetail = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label19 = new System.Windows.Forms.Label();
		this.grdHistory = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.label20 = new System.Windows.Forms.Label();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox7 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.label149 = new System.Windows.Forms.Label();
		this.label150 = new System.Windows.Forms.Label();
		this.label151 = new System.Windows.Forms.Label();
		this.label152 = new System.Windows.Forms.Label();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label17 = new System.Windows.Forms.Label();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMultiLanguage).BeginInit();
		this.toolStrip1.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBroker).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbTransporter).BeginInit();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbuom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).BeginInit();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.PanelExxpenses.SuspendLayout();
		this.PanelOtherRevenue.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTermSupplier).BeginInit();
		this.panel8.SuspendLayout();
		this.PanelPmExpense.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterial).BeginInit();
		this.panel11.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel5.SuspendLayout();
		this.panel2.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).BeginInit();
		this.groupBox6.SuspendLayout();
		this.panel4.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel6.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.panel7.SuspendLayout();
		this.groupBox7.SuspendLayout();
		this.panel12.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbMultiLanguage);
		this.panel1.Controls.Add(this.labelmultilang);
		this.panel1.Controls.Add(this.ChkUrduPrintPreview);
		this.panel1.Controls.Add(this.chkPrint);
		this.panel1.Controls.Add(this.label23);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1172, 50);
		this.panel1.TabIndex = 29;
		this.CmbMultiLanguage.AllowNull = (DefaultableBoolean)1;
		((System.Windows.Forms.Control)(object)this.CmbMultiLanguage).Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.CmbMultiLanguage.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbMultiLanguage.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbMultiLanguage.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance64).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance65).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbMultiLanguage).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbMultiLanguage).Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbMultiLanguage.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbMultiLanguage).Location = new System.Drawing.Point(835, 26);
		((System.Windows.Forms.Control)(object)this.CmbMultiLanguage).Name = "CmbMultiLanguage";
		((System.Windows.Forms.Control)(object)this.CmbMultiLanguage).Size = new System.Drawing.Size(128, 23);
		((System.Windows.Forms.Control)(object)this.CmbMultiLanguage).TabIndex = 47;
		((System.Windows.Forms.Control)(object)this.CmbMultiLanguage).TabStop = false;
		((System.Windows.Forms.Control)(object)this.CmbMultiLanguage).Visible = false;
		this.labelmultilang.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.labelmultilang.AutoSize = true;
		this.labelmultilang.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.labelmultilang.ForeColor = System.Drawing.Color.White;
		this.labelmultilang.Location = new System.Drawing.Point(753, 27);
		this.labelmultilang.Name = "labelmultilang";
		this.labelmultilang.Size = new System.Drawing.Size(76, 20);
		this.labelmultilang.TabIndex = 48;
		this.labelmultilang.Text = "Language";
		this.labelmultilang.Visible = false;
		this.ChkUrduPrintPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkUrduPrintPreview.AutoSize = true;
		this.ChkUrduPrintPreview.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkUrduPrintPreview.ForeColor = System.Drawing.Color.White;
		this.ChkUrduPrintPreview.Location = new System.Drawing.Point(1074, 27);
		this.ChkUrduPrintPreview.Margin = new System.Windows.Forms.Padding(2);
		this.ChkUrduPrintPreview.Name = "ChkUrduPrintPreview";
		this.ChkUrduPrintPreview.Size = new System.Drawing.Size(94, 20);
		this.ChkUrduPrintPreview.TabIndex = 46;
		this.ChkUrduPrintPreview.Text = "Urdu Print";
		this.ChkUrduPrintPreview.UseVisualStyleBackColor = true;
		this.chkPrint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPrint.AutoSize = true;
		this.chkPrint.Checked = true;
		this.chkPrint.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPrint.ForeColor = System.Drawing.Color.White;
		this.chkPrint.Location = new System.Drawing.Point(977, 27);
		this.chkPrint.Name = "chkPrint";
		this.chkPrint.Size = new System.Drawing.Size(84, 20);
		this.chkPrint.TabIndex = 4;
		this.chkPrint.Text = "Preview";
		this.chkPrint.UseVisualStyleBackColor = true;
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.Teal;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 27);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(354, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "Commission Trade Loading Delivery Direct";
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[15]
		{
			this.btnNew, this.btnRefresh, this.toolStripSeparator2, this.btnUpdate, this.btnSave, this.btnDelete, this.toolStripSeparator3, this.btnAttachment, this.toolStripSeparator1, this.btnPrint,
			this.btnUrduPrint, this.toolStripSeparator4, this.BtnLoadingExpenseForm, this.toolStripSeparator5, this.btnshortcutkeys
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1172, 25);
		this.toolStrip1.TabIndex = 28;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 22);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 22);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(74, 22);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(60, 22);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(69, 22);
		this.btnDelete.Text = "&Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(105, 22);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(96, 22);
		this.btnPrint.Text = "1702-&Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnUrduPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUrduPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnUrduPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUrduPrint.Name = "btnUrduPrint";
		this.btnUrduPrint.Size = new System.Drawing.Size(96, 22);
		this.btnUrduPrint.Text = "1702-Urdu";
		this.btnUrduPrint.Click += new System.EventHandler(btnUrduPrint_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
		this.BtnLoadingExpenseForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnLoadingExpenseForm.Image = Architecture.WinApp.Properties.Resources.Account_Reports;
		this.BtnLoadingExpenseForm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadingExpenseForm.Name = "BtnLoadingExpenseForm";
		this.BtnLoadingExpenseForm.Size = new System.Drawing.Size(173, 22);
		this.BtnLoadingExpenseForm.Text = "Loading Expense Form";
		this.BtnLoadingExpenseForm.Click += new System.EventHandler(BtnLoadingExpenseForm_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(122, 22);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.groupBox1.Controls.Add(this.rdTemplate2);
		this.groupBox1.Controls.Add(this.rdTemplate1);
		this.groupBox1.Controls.Add(this.txtRemarksHeader);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.txtmanualno);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.txtdocdate);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1165, 73);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main Infomation";
		this.rdTemplate2.AutoSize = true;
		this.rdTemplate2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdTemplate2.Location = new System.Drawing.Point(1073, 39);
		this.rdTemplate2.Name = "rdTemplate2";
		this.rdTemplate2.Size = new System.Drawing.Size(85, 19);
		this.rdTemplate2.TabIndex = 98;
		this.rdTemplate2.Text = "Template II";
		this.rdTemplate2.UseVisualStyleBackColor = true;
		this.rdTemplate1.AutoSize = true;
		this.rdTemplate1.Checked = true;
		this.rdTemplate1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdTemplate1.Location = new System.Drawing.Point(1074, 17);
		this.rdTemplate1.Name = "rdTemplate1";
		this.rdTemplate1.Size = new System.Drawing.Size(81, 19);
		this.rdTemplate1.TabIndex = 97;
		this.rdTemplate1.TabStop = true;
		this.rdTemplate1.Text = "Template I";
		this.rdTemplate1.UseVisualStyleBackColor = true;
		this.rdTemplate1.CheckedChanged += new System.EventHandler(rdTemplate1_CheckedChanged);
		this.txtRemarksHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksHeader.Location = new System.Drawing.Point(405, 17);
		this.txtRemarksHeader.Multiline = true;
		this.txtRemarksHeader.Name = "txtRemarksHeader";
		this.txtRemarksHeader.Size = new System.Drawing.Size(620, 49);
		this.txtRemarksHeader.TabIndex = 3;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(5, 47);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(66, 15);
		this.label6.TabIndex = 95;
		this.label6.Text = "Manual No";
		this.label33.BackColor = System.Drawing.Color.LightGray;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(321, 17);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(78, 23);
		this.label33.TabIndex = 20;
		this.label33.Text = "Remarks";
		this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtmanualno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtmanualno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtmanualno.Location = new System.Drawing.Point(95, 43);
		this.txtmanualno.Name = "txtmanualno";
		this.txtmanualno.Size = new System.Drawing.Size(220, 23);
		this.txtmanualno.TabIndex = 2;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(5, 21);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(88, 15);
		this.label5.TabIndex = 96;
		this.label5.Text = "Doc No && Date";
		this.txtdocdate.CustomFormat = "dd-MMM-yy";
		this.txtdocdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(187, 17);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(128, 23);
		this.txtdocdate.TabIndex = 1;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(95, 17);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(90, 23);
		this.txtdocno.TabIndex = 0;
		this.cmbBroker.AllowNull = (DefaultableBoolean)1;
		this.cmbBroker.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBroker.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBroker.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBroker).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbBroker).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBroker).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBroker).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBroker).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance68;
		((SpecialBoxBase)((UltraGridBase)this.cmbBroker).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBroker).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbBroker).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBroker).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBroker).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBroker).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbBroker).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBroker).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBroker).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance3).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBroker).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbBroker).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBroker).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBroker).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance31).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBroker).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbBroker).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBroker).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBroker).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbBroker).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBroker).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbBroker).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBroker).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBroker).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBroker).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbBroker.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBroker).Location = new System.Drawing.Point(5, 32);
		((System.Windows.Forms.Control)(object)this.cmbBroker).Name = "cmbBroker";
		((System.Windows.Forms.Control)(object)this.cmbBroker).Size = new System.Drawing.Size(182, 26);
		((System.Windows.Forms.Control)(object)this.cmbBroker).TabIndex = 0;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(5, 16);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(41, 15);
		this.label27.TabIndex = 127;
		this.label27.Text = "Broker";
		this.cmbTransporter.AllowNull = (DefaultableBoolean)1;
		this.cmbTransporter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbTransporter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbTransporter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbTransporter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance24;
		((SpecialBoxBase)((UltraGridBase)this.cmbTransporter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance81).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance82).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbTransporter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbTransporter).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbTransporter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbTransporter).Location = new System.Drawing.Point(189, 32);
		((System.Windows.Forms.Control)(object)this.cmbTransporter).Name = "cmbTransporter";
		((System.Windows.Forms.Control)(object)this.cmbTransporter).Size = new System.Drawing.Size(184, 26);
		((System.Windows.Forms.Control)(object)this.cmbTransporter).TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(612, 16);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(76, 15);
		this.label1.TabIndex = 107;
		this.label1.Text = "Bilty Expense\r\n";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(1025, 16);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(87, 15);
		this.label4.TabIndex = 109;
		this.label4.Text = "Vehicle Weight";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(189, 16);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(67, 15);
		this.label29.TabIndex = 103;
		this.label29.Text = "Transporter";
		this.txtbiltyexpense.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbiltyexpense.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbiltyexpense.Location = new System.Drawing.Point(612, 34);
		this.txtbiltyexpense.Name = "txtbiltyexpense";
		this.txtbiltyexpense.ShortcutsEnabled = false;
		this.txtbiltyexpense.Size = new System.Drawing.Size(99, 23);
		this.txtbiltyexpense.TabIndex = 4;
		this.txtbiltyexpense.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtbiltyexpense.TextChanged += new System.EventHandler(txtbiltyexpense_TextChanged);
		this.txtbiltyexpense.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtbiltyexpense_KeyPress);
		this.txtfrieght.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfrieght.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfrieght.Location = new System.Drawing.Point(713, 34);
		this.txtfrieght.Name = "txtfrieght";
		this.txtfrieght.ShortcutsEnabled = false;
		this.txtfrieght.Size = new System.Drawing.Size(99, 23);
		this.txtfrieght.TabIndex = 5;
		this.txtfrieght.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtfrieght.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtfrieght_KeyPress);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(483, 16);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(58, 15);
		this.label2.TabIndex = 99;
		this.label2.Text = "Bilty Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(713, 16);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(70, 15);
		this.label3.TabIndex = 105;
		this.label3.Text = "Bilty Frieght";
		this.txtvehicleweight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehicleweight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtvehicleweight.Location = new System.Drawing.Point(1025, 34);
		this.txtvehicleweight.Name = "txtvehicleweight";
		this.txtvehicleweight.ShortcutsEnabled = false;
		this.txtvehicleweight.Size = new System.Drawing.Size(122, 23);
		this.txtvehicleweight.TabIndex = 8;
		this.txtvehicleweight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtvehicleweight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtvehicleweight_KeyPress);
		this.txtbiltydate.CustomFormat = "dd-MMM-yy";
		this.txtbiltydate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbiltydate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtbiltydate.Location = new System.Drawing.Point(483, 34);
		this.txtbiltydate.Name = "txtbiltydate";
		this.txtbiltydate.Size = new System.Drawing.Size(127, 23);
		this.txtbiltydate.TabIndex = 3;
		this.lbl.AutoSize = true;
		this.lbl.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbl.Location = new System.Drawing.Point(376, 16);
		this.lbl.Name = "lbl";
		this.lbl.Size = new System.Drawing.Size(49, 15);
		this.lbl.TabIndex = 74;
		this.lbl.Text = "Bilty No";
		this.txtBiltyNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBiltyNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBiltyNo.Location = new System.Drawing.Point(376, 34);
		this.txtBiltyNo.Name = "txtBiltyNo";
		this.txtBiltyNo.Size = new System.Drawing.Size(105, 23);
		this.txtBiltyNo.TabIndex = 2;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(814, 16);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(64, 15);
		this.label48.TabIndex = 72;
		this.label48.Text = "Vehicle No";
		this.txtvehicleno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehicleno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtvehicleno.Location = new System.Drawing.Point(814, 34);
		this.txtvehicleno.Name = "txtvehicleno";
		this.txtvehicleno.Size = new System.Drawing.Size(105, 23);
		this.txtvehicleno.TabIndex = 6;
		this.groupBox4.Controls.Add(this.label34);
		this.groupBox4.Controls.Add(this.txtcustremarks);
		this.groupBox4.Controls.Add(this.label35);
		this.groupBox4.Controls.Add(this.txtcustnetwt);
		this.groupBox4.Controls.Add(this.label24);
		this.groupBox4.Controls.Add(this.label9);
		this.groupBox4.Controls.Add(this.txtcustadlswt);
		this.groupBox4.Controls.Add(this.label37);
		this.groupBox4.Controls.Add(this.txtCusRate);
		this.groupBox4.Controls.Add(this.label25);
		this.groupBox4.Controls.Add(this.txtcustpckmtrlwt);
		this.groupBox4.Controls.Add(this.label38);
		this.groupBox4.Controls.Add(this.txtCusQty);
		this.groupBox4.Controls.Add(this.txtcustgrosswt);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcustomer);
		this.groupBox4.Controls.Add(this.label39);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(2, 184);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(1074, 59);
		this.groupBox4.TabIndex = 2;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Customer Information";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(738, 13);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(52, 15);
		this.label34.TabIndex = 121;
		this.label34.Text = "Remarks";
		this.txtcustremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcustremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcustremarks.Location = new System.Drawing.Point(738, 31);
		this.txtcustremarks.Name = "txtcustremarks";
		this.txtcustremarks.Size = new System.Drawing.Size(330, 23);
		this.txtcustremarks.TabIndex = 7;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(558, 13);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(45, 15);
		this.label35.TabIndex = 119;
		this.label35.Text = "Net Wt";
		this.txtcustnetwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcustnetwt.Enabled = false;
		this.txtcustnetwt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcustnetwt.Location = new System.Drawing.Point(552, 31);
		this.txtcustnetwt.Name = "txtcustnetwt";
		this.txtcustnetwt.ShortcutsEnabled = false;
		this.txtcustnetwt.Size = new System.Drawing.Size(103, 23);
		this.txtcustnetwt.TabIndex = 5;
		this.txtcustnetwt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtcustnetwt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcustnetwt_KeyPress);
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(657, 13);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(58, 15);
		this.label24.TabIndex = 119;
		this.label24.Text = "Item Rate";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(486, 13);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(52, 15);
		this.label9.TabIndex = 117;
		this.label9.Text = "AdLs Wt";
		this.txtcustadlswt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcustadlswt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcustadlswt.Location = new System.Drawing.Point(486, 31);
		this.txtcustadlswt.Name = "txtcustadlswt";
		this.txtcustadlswt.ShortcutsEnabled = false;
		this.txtcustadlswt.Size = new System.Drawing.Size(64, 23);
		this.txtcustadlswt.TabIndex = 4;
		this.txtcustadlswt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtcustadlswt.TextChanged += new System.EventHandler(txtcustadlswt_TextChanged);
		this.txtcustadlswt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcustadlswt_KeyPress);
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(420, 13);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(45, 15);
		this.label37.TabIndex = 115;
		this.label37.Text = "P.M Wt";
		this.txtCusRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCusRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCusRate.Location = new System.Drawing.Point(657, 31);
		this.txtCusRate.Name = "txtCusRate";
		this.txtCusRate.ShortcutsEnabled = false;
		this.txtCusRate.Size = new System.Drawing.Size(79, 23);
		this.txtCusRate.TabIndex = 6;
		this.txtCusRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCusRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSupNetWeight_KeyPress);
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(241, 13);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(26, 15);
		this.label25.TabIndex = 109;
		this.label25.Text = "Qty";
		this.txtcustpckmtrlwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcustpckmtrlwt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcustpckmtrlwt.Location = new System.Drawing.Point(420, 31);
		this.txtcustpckmtrlwt.Name = "txtcustpckmtrlwt";
		this.txtcustpckmtrlwt.ShortcutsEnabled = false;
		this.txtcustpckmtrlwt.Size = new System.Drawing.Size(64, 23);
		this.txtcustpckmtrlwt.TabIndex = 3;
		this.txtcustpckmtrlwt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtcustpckmtrlwt.TextChanged += new System.EventHandler(txtcustpckmtrlwt_TextChanged);
		this.txtcustpckmtrlwt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcustpckmtrlwt_KeyPress);
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(315, 13);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(55, 15);
		this.label38.TabIndex = 113;
		this.label38.Text = "Gross Wt";
		this.txtCusQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCusQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCusQty.Location = new System.Drawing.Point(241, 31);
		this.txtCusQty.Name = "txtCusQty";
		this.txtCusQty.ShortcutsEnabled = false;
		this.txtCusQty.Size = new System.Drawing.Size(72, 23);
		this.txtCusQty.TabIndex = 1;
		this.txtCusQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCusQty.TextChanged += new System.EventHandler(txtCusQty_TextChanged);
		this.txtCusQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCusQty_KeyPress);
		this.txtcustgrosswt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcustgrosswt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcustgrosswt.Location = new System.Drawing.Point(315, 31);
		this.txtcustgrosswt.Name = "txtcustgrosswt";
		this.txtcustgrosswt.ShortcutsEnabled = false;
		this.txtcustgrosswt.Size = new System.Drawing.Size(103, 23);
		this.txtcustgrosswt.TabIndex = 2;
		this.txtcustgrosswt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtcustgrosswt.TextChanged += new System.EventHandler(txtcustgrosswt_TextChanged);
		this.txtcustgrosswt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcustgrosswt_KeyPress);
		this.cmbcustomer.AllowNull = (DefaultableBoolean)1;
		this.cmbcustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance49;
		((SpecialBoxBase)((UltraGridBase)this.cmbcustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance54).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance55).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance35).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcustomer).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbcustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcustomer).Location = new System.Drawing.Point(4, 29);
		((System.Windows.Forms.Control)(object)this.cmbcustomer).Name = "cmbcustomer";
		((System.Windows.Forms.Control)(object)this.cmbcustomer).Size = new System.Drawing.Size(235, 26);
		((System.Windows.Forms.Control)(object)this.cmbcustomer).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbcustomer).Leave += new System.EventHandler(cmbcustomer_Leave);
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(4, 13);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(93, 15);
		this.label39.TabIndex = 111;
		this.label39.Text = "Customer Name";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(103, 67);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(52, 15);
		this.label18.TabIndex = 121;
		this.label18.Text = "Remarks";
		this.txtsupremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsupremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsupremarks.Location = new System.Drawing.Point(100, 84);
		this.txtsupremarks.Name = "txtsupremarks";
		this.txtsupremarks.Size = new System.Drawing.Size(712, 23);
		this.txtsupremarks.TabIndex = 12;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(1049, 20);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(45, 15);
		this.label16.TabIndex = 119;
		this.label16.Text = "Net Wt";
		this.txtItemSupNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemSupNetWeight.Enabled = false;
		this.txtItemSupNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemSupNetWeight.Location = new System.Drawing.Point(1049, 40);
		this.txtItemSupNetWeight.Name = "txtItemSupNetWeight";
		this.txtItemSupNetWeight.ShortcutsEnabled = false;
		this.txtItemSupNetWeight.Size = new System.Drawing.Size(100, 23);
		this.txtItemSupNetWeight.TabIndex = 10;
		this.txtItemSupNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemSupNetWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSupNetWeight_KeyPress);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(984, 20);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(52, 15);
		this.label15.TabIndex = 117;
		this.label15.Text = "AdLs Wt";
		this.txtItemSupAdLsWt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemSupAdLsWt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemSupAdLsWt.Location = new System.Drawing.Point(984, 40);
		this.txtItemSupAdLsWt.Name = "txtItemSupAdLsWt";
		this.txtItemSupAdLsWt.ShortcutsEnabled = false;
		this.txtItemSupAdLsWt.Size = new System.Drawing.Size(64, 23);
		this.txtItemSupAdLsWt.TabIndex = 9;
		this.txtItemSupAdLsWt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemSupAdLsWt.TextChanged += new System.EventHandler(txtSupAdLsWt_TextChanged);
		this.txtItemSupAdLsWt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSupAdLsWt_KeyPress);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(919, 20);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(45, 15);
		this.label14.TabIndex = 115;
		this.label14.Text = "P.M Wt";
		this.txtItemSupPmWt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemSupPmWt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemSupPmWt.Location = new System.Drawing.Point(919, 40);
		this.txtItemSupPmWt.Name = "txtItemSupPmWt";
		this.txtItemSupPmWt.ShortcutsEnabled = false;
		this.txtItemSupPmWt.Size = new System.Drawing.Size(64, 23);
		this.txtItemSupPmWt.TabIndex = 8;
		this.txtItemSupPmWt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemSupPmWt.TextChanged += new System.EventHandler(txtSupPckMtrlWeight_TextChanged);
		this.txtItemSupPmWt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSupPckMtrlWeight_KeyPress);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(815, 20);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(83, 15);
		this.label13.TabIndex = 113;
		this.label13.Text = "Item Gross Wt";
		this.txtItemSupGrossWt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemSupGrossWt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemSupGrossWt.Location = new System.Drawing.Point(815, 40);
		this.txtItemSupGrossWt.Name = "txtItemSupGrossWt";
		this.txtItemSupGrossWt.ShortcutsEnabled = false;
		this.txtItemSupGrossWt.Size = new System.Drawing.Size(100, 23);
		this.txtItemSupGrossWt.TabIndex = 7;
		this.txtItemSupGrossWt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemSupGrossWt.TextChanged += new System.EventHandler(txtsupgrosswt_TextChanged);
		this.txtItemSupGrossWt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtsupgrosswt_KeyPress);
		this.cmbsupplier.AllowNull = (DefaultableBoolean)1;
		this.cmbsupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance89;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance74).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance75).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbsupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Location = new System.Drawing.Point(5, 38);
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Name = "cmbsupplier";
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Size = new System.Drawing.Size(202, 26);
		((System.Windows.Forms.Control)(object)this.cmbsupplier).TabIndex = 0;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(5, 20);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(51, 15);
		this.label12.TabIndex = 111;
		this.label12.Text = "Supplier";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(735, 20);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(54, 15);
		this.label10.TabIndex = 109;
		this.label10.Text = "Item Qty";
		this.txtItemSupQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemSupQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemSupQty.Location = new System.Drawing.Point(735, 40);
		this.txtItemSupQty.Name = "txtItemSupQty";
		this.txtItemSupQty.ShortcutsEnabled = false;
		this.txtItemSupQty.Size = new System.Drawing.Size(78, 23);
		this.txtItemSupQty.TabIndex = 6;
		this.txtItemSupQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemSupQty.TextChanged += new System.EventHandler(txtItemQty_TextChanged);
		this.txtItemSupQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtItemQty_KeyPress);
		this.cmbuom.AllowNull = (DefaultableBoolean)1;
		this.cmbuom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbuom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbuom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbuom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbuom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbuom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbuom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbuom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance36;
		((SpecialBoxBase)((UltraGridBase)this.cmbuom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbuom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbuom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbuom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance41).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance42).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance56).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbuom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbuom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbuom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbuom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbuom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbuom).Location = new System.Drawing.Point(657, 38);
		((System.Windows.Forms.Control)(object)this.cmbuom).Name = "cmbuom";
		((System.Windows.Forms.Control)(object)this.cmbuom).Size = new System.Drawing.Size(76, 26);
		((System.Windows.Forms.Control)(object)this.cmbuom).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.cmbuom).TextChanged += new System.EventHandler(ctrlGrdBar1_Load);
		((System.Windows.Forms.Control)(object)this.cmbuom).Leave += new System.EventHandler(cmbuom_TextChanged);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(657, 20);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(35, 15);
		this.label11.TabIndex = 107;
		this.label11.Text = "UOM";
		this.cmbitem.AllowNull = (DefaultableBoolean)1;
		this.cmbitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance97).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance97).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance98;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance99).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance99).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance104).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbitem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbitem).Location = new System.Drawing.Point(210, 38);
		((System.Windows.Forms.Control)(object)this.cmbitem).Name = "cmbitem";
		((System.Windows.Forms.Control)(object)this.cmbitem).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.cmbitem).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbitem).Leave += new System.EventHandler(cmbitem_Leave);
		this.labItemName.AutoSize = true;
		this.labItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labItemName.Location = new System.Drawing.Point(210, 20);
		this.labItemName.Name = "labItemName";
		this.labItemName.Size = new System.Drawing.Size(32, 15);
		this.labItemName.TabIndex = 105;
		this.labItemName.Text = "Item";
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1186, 741);
		this.tabControl1.TabIndex = 30;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.PanelExxpenses);
		this.tabPage1.Controls.Add(this.tabControl2);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1178, 712);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.PanelExxpenses.Controls.Add(this.PanelOtherRevenue);
		this.PanelExxpenses.Controls.Add(this.PanelPmExpense);
		this.PanelExxpenses.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanelExxpenses.Location = new System.Drawing.Point(3, 587);
		this.PanelExxpenses.Name = "PanelExxpenses";
		this.PanelExxpenses.Size = new System.Drawing.Size(1172, 122);
		this.PanelExxpenses.TabIndex = 35;
		this.PanelOtherRevenue.BackColor = System.Drawing.Color.White;
		this.PanelOtherRevenue.Controls.Add((System.Windows.Forms.Control)(object)this.grdPaymentTermSupplier);
		this.PanelOtherRevenue.Controls.Add(this.panel8);
		this.PanelOtherRevenue.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanelOtherRevenue.Location = new System.Drawing.Point(552, 0);
		this.PanelOtherRevenue.Name = "PanelOtherRevenue";
		this.PanelOtherRevenue.Size = new System.Drawing.Size(620, 122);
		this.PanelOtherRevenue.TabIndex = 34;
		((ScrollableBase)this.grdPaymentTermSupplier).BackColor = System.Drawing.Color.White;
		grdPaymentTermSupplier_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPaymentTermSupplier.DesignTimeLayout = grdPaymentTermSupplier_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermSupplier).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermSupplier).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPaymentTermSupplier.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermSupplier).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdPaymentTermSupplier).Name = "grdPaymentTermSupplier";
		((ScrollableBase)this.grdPaymentTermSupplier).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermSupplier).Size = new System.Drawing.Size(620, 100);
		((System.Windows.Forms.Control)(object)this.grdPaymentTermSupplier).TabIndex = 0;
		this.grdPaymentTermSupplier.TotalRow = (InheritableBoolean)1;
		this.grdPaymentTermSupplier.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPaymentTermSupplier).VisualStyle = (VisualStyle)4;
		this.grdPaymentTermSupplier.CellUpdated += new ColumnActionEventHandler(grdPaymentTermSupplier_CellUpdated);
		this.grdPaymentTermSupplier.UpdatingCell += new UpdatingCellEventHandler(grdPaymentTermSupplier_UpdatingCell);
		this.grdPaymentTermSupplier.ColumnButtonClick += new ColumnActionEventHandler(grdPaymentTermSupplier_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPaymentTermSupplier).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPaymentTermSupplier_KeyDown);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.ctrlGrdBar3);
		this.panel8.Controls.Add(this.label21);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(620, 22);
		this.panel8.TabIndex = 33;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(575, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
		this.ctrlGrdBar3.MyGrid = this.grdPaymentTermSupplier;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(41, 19);
		this.ctrlGrdBar3.TabIndex = 45;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label21.AutoSize = true;
		this.label21.BackColor = System.Drawing.Color.Teal;
		this.label21.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.White;
		this.label21.Location = new System.Drawing.Point(8, 3);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(161, 16);
		this.label21.TabIndex = 0;
		this.label21.Text = "Supplier Payment Detail";
		this.PanelPmExpense.BackColor = System.Drawing.Color.White;
		this.PanelPmExpense.Controls.Add((System.Windows.Forms.Control)(object)this.grdPackingMaterial);
		this.PanelPmExpense.Controls.Add(this.panel11);
		this.PanelPmExpense.Dock = System.Windows.Forms.DockStyle.Left;
		this.PanelPmExpense.Location = new System.Drawing.Point(0, 0);
		this.PanelPmExpense.Name = "PanelPmExpense";
		this.PanelPmExpense.Size = new System.Drawing.Size(552, 122);
		this.PanelPmExpense.TabIndex = 33;
		((ScrollableBase)this.grdPackingMaterial).BackColor = System.Drawing.Color.White;
		grdPackingMaterial_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPackingMaterial.DesignTimeLayout = grdPackingMaterial_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPackingMaterial.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Name = "grdPackingMaterial";
		((ScrollableBase)this.grdPackingMaterial).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Size = new System.Drawing.Size(552, 100);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).TabIndex = 0;
		this.grdPackingMaterial.TotalRow = (InheritableBoolean)1;
		this.grdPackingMaterial.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPackingMaterial).VisualStyle = (VisualStyle)4;
		this.grdPackingMaterial.CellUpdated += new ColumnActionEventHandler(grdPackingMaterial_CellUpdated);
		this.grdPackingMaterial.ColumnButtonClick += new ColumnActionEventHandler(grdPackingMaterial_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPackingMaterial_KeyDown);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.ctrlGrdBar4);
		this.panel11.Controls.Add(this.label36);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(552, 22);
		this.panel11.TabIndex = 33;
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(507, 1);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
		this.ctrlGrdBar4.MyGrid = this.grdPackingMaterial;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(41, 19);
		this.ctrlGrdBar4.TabIndex = 45;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.label36.AutoSize = true;
		this.label36.BackColor = System.Drawing.Color.Teal;
		this.label36.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.ForeColor = System.Drawing.Color.White;
		this.label36.Location = new System.Drawing.Point(8, 3);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(105, 16);
		this.label36.TabIndex = 0;
		this.label36.Text = "Packing Matrial";
		this.tabControl2.Controls.Add(this.tabDetail);
		this.tabControl2.Controls.Add(this.tabPM);
		this.tabControl2.Controls.Add(this.tabRevenue);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Top;
		this.tabControl2.Location = new System.Drawing.Point(3, 377);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1172, 210);
		this.tabControl2.TabIndex = 34;
		this.tabDetail.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabDetail.Controls.Add(this.panel5);
		this.tabDetail.Location = new System.Drawing.Point(4, 25);
		this.tabDetail.Name = "tabDetail";
		this.tabDetail.Padding = new System.Windows.Forms.Padding(3);
		this.tabDetail.Size = new System.Drawing.Size(1164, 181);
		this.tabDetail.TabIndex = 0;
		this.tabDetail.Text = "Detail";
		this.tabDetail.UseVisualStyleBackColor = true;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 26);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1158, 152);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Controls.Add(this.label46);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1158, 23);
		this.panel5.TabIndex = 31;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1114, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(7, 5, 7, 5);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 21);
		this.ctrlGrdBar1.TabIndex = 45;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label46.AutoSize = true;
		this.label46.BackColor = System.Drawing.Color.Teal;
		this.label46.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.ForeColor = System.Drawing.Color.White;
		this.label46.Location = new System.Drawing.Point(8, 3);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(44, 16);
		this.label46.TabIndex = 0;
		this.label46.Text = "Detail";
		this.tabPM.Location = new System.Drawing.Point(4, 25);
		this.tabPM.Name = "tabPM";
		this.tabPM.Padding = new System.Windows.Forms.Padding(3);
		this.tabPM.Size = new System.Drawing.Size(1164, 181);
		this.tabPM.TabIndex = 1;
		this.tabPM.Text = "Packing Material";
		this.tabPM.UseVisualStyleBackColor = true;
		this.tabRevenue.Location = new System.Drawing.Point(4, 25);
		this.tabRevenue.Name = "tabRevenue";
		this.tabRevenue.Size = new System.Drawing.Size(1164, 181);
		this.tabRevenue.TabIndex = 2;
		this.tabRevenue.Text = "Supplier Payment Detail";
		this.tabRevenue.UseVisualStyleBackColor = true;
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.groupBox2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 130);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1172, 247);
		this.panel2.TabIndex = 30;
		this.groupBox2.Controls.Add(this.groupBox4);
		this.groupBox2.Controls.Add(this.groupBox5);
		this.groupBox2.Controls.Add(this.groupBox6);
		this.groupBox2.Controls.Add(this.btnAddDetail);
		this.groupBox2.Controls.Add(this.btnUpdateDetail);
		this.groupBox2.Controls.Add(this.btnCancelDetail);
		this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(0, 0);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1172, 244);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Detail Infomation";
		this.groupBox5.Controls.Add(this.txtsupremarks);
		this.groupBox5.Controls.Add(this.label18);
		this.groupBox5.Controls.Add(this.label32);
		this.groupBox5.Controls.Add(this.rdSearchByCode);
		this.groupBox5.Controls.Add(this.txtSupNetWeight);
		this.groupBox5.Controls.Add(this.rdSearchByName);
		this.groupBox5.Controls.Add(this.label22);
		this.groupBox5.Controls.Add(this.label31);
		this.groupBox5.Controls.Add(this.label16);
		this.groupBox5.Controls.Add(this.txtSupAdLsWt);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbitem);
		this.groupBox5.Controls.Add(this.label30);
		this.groupBox5.Controls.Add(this.txtSupRate);
		this.groupBox5.Controls.Add(this.txtItemSupNetWeight);
		this.groupBox5.Controls.Add(this.label10);
		this.groupBox5.Controls.Add(this.txtSupPmWt);
		this.groupBox5.Controls.Add(this.label15);
		this.groupBox5.Controls.Add(this.label28);
		this.groupBox5.Controls.Add(this.txtItemSupQty);
		this.groupBox5.Controls.Add(this.txtItemSupAdLsWt);
		this.groupBox5.Controls.Add(this.txtSupGrossWt);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbuom);
		this.groupBox5.Controls.Add(this.txtItemSupPmWt);
		this.groupBox5.Controls.Add(this.label14);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcropYear);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackingType);
		this.groupBox5.Controls.Add(this.txtItemSupGrossWt);
		this.groupBox5.Controls.Add(this.label13);
		this.groupBox5.Controls.Add(this.label11);
		this.groupBox5.Controls.Add(this.labItemName);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsupplier);
		this.groupBox5.Controls.Add(this.label12);
		this.groupBox5.Controls.Add(this.label7);
		this.groupBox5.Controls.Add(this.label8);
		this.groupBox5.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox5.Location = new System.Drawing.Point(2, 77);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(1153, 112);
		this.groupBox5.TabIndex = 1;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Item && Supplier Information";
		this.label32.AutoSize = true;
		this.label32.Enabled = false;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(1049, 67);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(45, 15);
		this.label32.TabIndex = 119;
		this.label32.Text = "Net Wt";
		this.rdSearchByCode.AutoSize = true;
		this.rdSearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCode.Location = new System.Drawing.Point(331, 18);
		this.rdSearchByCode.Name = "rdSearchByCode";
		this.rdSearchByCode.Size = new System.Drawing.Size(52, 19);
		this.rdSearchByCode.TabIndex = 1;
		this.rdSearchByCode.TabStop = true;
		this.rdSearchByCode.Text = "Code";
		this.rdSearchByCode.UseVisualStyleBackColor = true;
		this.txtSupNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupNetWeight.Enabled = false;
		this.txtSupNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupNetWeight.Location = new System.Drawing.Point(1049, 84);
		this.txtSupNetWeight.Name = "txtSupNetWeight";
		this.txtSupNetWeight.ShortcutsEnabled = false;
		this.txtSupNetWeight.Size = new System.Drawing.Size(100, 23);
		this.txtSupNetWeight.TabIndex = 16;
		this.txtSupNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSupNetWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcustnetwt_KeyPress);
		this.rdSearchByName.AutoSize = true;
		this.rdSearchByName.Checked = true;
		this.rdSearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByName.Location = new System.Drawing.Point(267, 18);
		this.rdSearchByName.Name = "rdSearchByName";
		this.rdSearchByName.Size = new System.Drawing.Size(57, 19);
		this.rdSearchByName.TabIndex = 1;
		this.rdSearchByName.TabStop = true;
		this.rdSearchByName.Text = "Name";
		this.rdSearchByName.UseVisualStyleBackColor = true;
		this.rdSearchByName.CheckedChanged += new System.EventHandler(rdSearchByName_CheckedChanged);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(5, 67);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(58, 15);
		this.label22.TabIndex = 119;
		this.label22.Text = "Item Rate";
		this.label31.AutoSize = true;
		this.label31.Enabled = false;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(984, 67);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(52, 15);
		this.label31.TabIndex = 117;
		this.label31.Text = "AdLs Wt";
		this.txtSupAdLsWt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupAdLsWt.Enabled = false;
		this.txtSupAdLsWt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupAdLsWt.Location = new System.Drawing.Point(984, 84);
		this.txtSupAdLsWt.Name = "txtSupAdLsWt";
		this.txtSupAdLsWt.ShortcutsEnabled = false;
		this.txtSupAdLsWt.Size = new System.Drawing.Size(64, 23);
		this.txtSupAdLsWt.TabIndex = 15;
		this.txtSupAdLsWt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSupAdLsWt.TextChanged += new System.EventHandler(txtcustadlswt_TextChanged);
		this.txtSupAdLsWt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcustadlswt_KeyPress);
		this.label30.AutoSize = true;
		this.label30.Enabled = false;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(919, 67);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(45, 15);
		this.label30.TabIndex = 115;
		this.label30.Text = "P.M Wt";
		this.txtSupRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupRate.Location = new System.Drawing.Point(5, 84);
		this.txtSupRate.Name = "txtSupRate";
		this.txtSupRate.ShortcutsEnabled = false;
		this.txtSupRate.Size = new System.Drawing.Size(93, 23);
		this.txtSupRate.TabIndex = 11;
		this.txtSupRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSupRate.TextChanged += new System.EventHandler(txtSupRate_TextChanged);
		this.txtSupRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSupNetWeight_KeyPress);
		this.txtSupPmWt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupPmWt.Enabled = false;
		this.txtSupPmWt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupPmWt.Location = new System.Drawing.Point(919, 84);
		this.txtSupPmWt.Name = "txtSupPmWt";
		this.txtSupPmWt.ShortcutsEnabled = false;
		this.txtSupPmWt.Size = new System.Drawing.Size(64, 23);
		this.txtSupPmWt.TabIndex = 14;
		this.txtSupPmWt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSupPmWt.TextChanged += new System.EventHandler(txtcustpckmtrlwt_TextChanged);
		this.txtSupPmWt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcustpckmtrlwt_KeyPress);
		this.label28.AutoSize = true;
		this.label28.Enabled = false;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(814, 67);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(55, 15);
		this.label28.TabIndex = 113;
		this.label28.Text = "Gross Wt";
		this.txtSupGrossWt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupGrossWt.Enabled = false;
		this.txtSupGrossWt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupGrossWt.Location = new System.Drawing.Point(814, 84);
		this.txtSupGrossWt.Name = "txtSupGrossWt";
		this.txtSupGrossWt.ShortcutsEnabled = false;
		this.txtSupGrossWt.Size = new System.Drawing.Size(103, 23);
		this.txtSupGrossWt.TabIndex = 13;
		this.txtSupGrossWt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSupGrossWt.TextChanged += new System.EventHandler(txtcustgrosswt_TextChanged);
		this.txtSupGrossWt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcustgrosswt_KeyPress);
		this.cmbcropYear.AllowNull = (DefaultableBoolean)1;
		this.cmbcropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance12;
		((SpecialBoxBase)((UltraGridBase)this.cmbcropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance25).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcropYear).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbcropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcropYear).Location = new System.Drawing.Point(446, 38);
		((System.Windows.Forms.Control)(object)this.cmbcropYear).Name = "cmbcropYear";
		((System.Windows.Forms.Control)(object)this.cmbcropYear).Size = new System.Drawing.Size(93, 26);
		((System.Windows.Forms.Control)(object)this.cmbcropYear).TabIndex = 3;
		this.CmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance9;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance10).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance10).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance15).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Location = new System.Drawing.Point(541, 38);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Name = "CmbPackingType";
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Size = new System.Drawing.Size(113, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).TabIndex = 4;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(446, 20);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(57, 15);
		this.label7.TabIndex = 107;
		this.label7.Text = "Crop Year";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(541, 20);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(77, 15);
		this.label8.TabIndex = 107;
		this.label8.Text = "Packing Type";
		this.groupBox6.Controls.Add(this.label1);
		this.groupBox6.Controls.Add(this.label27);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBroker);
		this.groupBox6.Controls.Add(this.txtvehicleweight);
		this.groupBox6.Controls.Add(this.label4);
		this.groupBox6.Controls.Add(this.txtVehcileQty);
		this.groupBox6.Controls.Add(this.txtvehicleno);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbTransporter);
		this.groupBox6.Controls.Add(this.txtfrieght);
		this.groupBox6.Controls.Add(this.label3);
		this.groupBox6.Controls.Add(this.label26);
		this.groupBox6.Controls.Add(this.label29);
		this.groupBox6.Controls.Add(this.label48);
		this.groupBox6.Controls.Add(this.txtBiltyNo);
		this.groupBox6.Controls.Add(this.txtbiltyexpense);
		this.groupBox6.Controls.Add(this.lbl);
		this.groupBox6.Controls.Add(this.label2);
		this.groupBox6.Controls.Add(this.txtbiltydate);
		this.groupBox6.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox6.Location = new System.Drawing.Point(2, 17);
		this.groupBox6.Name = "groupBox6";
		this.groupBox6.Size = new System.Drawing.Size(1152, 68);
		this.groupBox6.TabIndex = 0;
		this.groupBox6.TabStop = false;
		this.groupBox6.Text = "Vehicle Information";
		this.txtVehcileQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehcileQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehcileQty.Location = new System.Drawing.Point(921, 34);
		this.txtVehcileQty.Name = "txtVehcileQty";
		this.txtVehcileQty.Size = new System.Drawing.Size(102, 23);
		this.txtVehcileQty.TabIndex = 7;
		this.txtVehcileQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtVehcileQty_KeyPress);
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(921, 16);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(67, 15);
		this.label26.TabIndex = 72;
		this.label26.Text = "Vehicle Qty";
		this.btnAddDetail.BackColor = System.Drawing.Color.Teal;
		this.btnAddDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddDetail.ForeColor = System.Drawing.Color.White;
		this.btnAddDetail.Location = new System.Drawing.Point(1080, 215);
		this.btnAddDetail.Name = "btnAddDetail";
		this.btnAddDetail.Size = new System.Drawing.Size(33, 24);
		this.btnAddDetail.TabIndex = 4;
		this.btnAddDetail.Text = "+";
		this.btnAddDetail.UseVisualStyleBackColor = false;
		this.btnAddDetail.Click += new System.EventHandler(btnAddDetail_Click);
		this.btnAddDetail.Leave += new System.EventHandler(txtRemarksHeader_Leave);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(1080, 215);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(71, 24);
		this.btnUpdateDetail.TabIndex = 4;
		this.btnUpdateDetail.Text = "Update ";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.btnCancelDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetail.Location = new System.Drawing.Point(1080, 191);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(71, 24);
		this.btnCancelDetail.TabIndex = 5;
		this.btnCancelDetail.Text = "Cancel";
		this.btnCancelDetail.UseVisualStyleBackColor = false;
		this.btnCancelDetail.Visible = false;
		this.btnCancelDetail.Click += new System.EventHandler(btnCancelDetail_Click);
		this.btnCancelDetail.Leave += new System.EventHandler(txtRemarksHeader_Leave);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 53);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1172, 77);
		this.panel4.TabIndex = 0;
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.tabPage2.Controls.Add(this.panel12);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel3);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryDetail);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1178, 712);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.grdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdHistoryDetail).BackColor = System.Drawing.Color.White;
		this.grdHistoryDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryDetail.DesignTimeLayout = grdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdHistoryDetail.FilterMode = (FilterMode)2;
		this.grdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Location = new System.Drawing.Point(3, 438);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Name = "grdHistoryDetail";
		((ScrollableBase)this.grdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(1172, 271);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabIndex = 1;
		this.grdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label19);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel3.Location = new System.Drawing.Point(3, 414);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1172, 24);
		this.panel3.TabIndex = 14;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label19.ForeColor = System.Drawing.Color.White;
		this.label19.Location = new System.Drawing.Point(5, 5);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(232, 17);
		this.label19.TabIndex = 40;
		this.label19.Text = "Detail of the Selected Above row";
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdHistory).BackColor = System.Drawing.Color.White;
		this.grdHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(3, 127);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1172, 287);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 0;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar2);
		this.panel6.Controls.Add(this.toolStrip2);
		this.panel6.Controls.Add(this.label20);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1172, 54);
		this.panel6.TabIndex = 12;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1129, 28);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(7, 4, 7, 4);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(40, 25);
		this.ctrlGrdBar2.TabIndex = 44;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnResetHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1172, 27);
		this.toolStrip2.TabIndex = 7;
		this.toolStrip2.Text = "toolStrip2";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label20.ForeColor = System.Drawing.Color.White;
		this.label20.Location = new System.Drawing.Point(2, 31);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(417, 19);
		this.label20.TabIndex = 40;
		this.label20.Text = "Commission Trade Loading Delivery Direct History";
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.Location = new System.Drawing.Point(611, 3);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(1273, 25);
		this.miniToolStrip.TabIndex = 27;
		this.panel7.Controls.Add(this.groupBox7);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 57);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1172, 46);
		this.panel7.TabIndex = 18;
		this.groupBox7.Controls.Add(this.rdapproveddate);
		this.groupBox7.Controls.Add(this.btnShowHistory);
		this.groupBox7.Controls.Add(this.rdmodifydate);
		this.groupBox7.Controls.Add(this.drdocdate);
		this.groupBox7.Controls.Add(this.rdentrydate);
		this.groupBox7.Controls.Add(this.label149);
		this.groupBox7.Controls.Add(this.label150);
		this.groupBox7.Controls.Add(this.label151);
		this.groupBox7.Controls.Add(this.label152);
		this.groupBox7.Controls.Add(this.ToDocNoHistory);
		this.groupBox7.Controls.Add(this.FromDocNoHistory);
		this.groupBox7.Controls.Add(this.ToDateHistory);
		this.groupBox7.Controls.Add(this.FromDateHistory);
		this.groupBox7.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox7.Location = new System.Drawing.Point(0, 2);
		this.groupBox7.Name = "groupBox7";
		this.groupBox7.Size = new System.Drawing.Size(1167, 44);
		this.groupBox7.TabIndex = 0;
		this.groupBox7.TabStop = false;
		this.groupBox7.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(1045, 18);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 8;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(714, 16);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 4;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(948, 18);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 7;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(781, 18);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 5;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(862, 18);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 6;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.label149.AutoSize = true;
		this.label149.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label149.Location = new System.Drawing.Point(569, 20);
		this.label149.Name = "label149";
		this.label149.Size = new System.Drawing.Size(64, 15);
		this.label149.TabIndex = 86;
		this.label149.Text = "To Doc No";
		this.label150.AutoSize = true;
		this.label150.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label150.Location = new System.Drawing.Point(407, 20);
		this.label150.Name = "label150";
		this.label150.Size = new System.Drawing.Size(79, 15);
		this.label150.TabIndex = 85;
		this.label150.Text = "From Doc No";
		this.label151.AutoSize = true;
		this.label151.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label151.Location = new System.Drawing.Point(215, 20);
		this.label151.Name = "label151";
		this.label151.Size = new System.Drawing.Size(48, 15);
		this.label151.TabIndex = 84;
		this.label151.Text = "To Date";
		this.label152.AutoSize = true;
		this.label152.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label152.Location = new System.Drawing.Point(3, 20);
		this.label152.Name = "label152";
		this.label152.Size = new System.Drawing.Size(63, 15);
		this.label152.TabIndex = 83;
		this.label152.Text = "From Date";
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(634, 16);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(488, 16);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(263, 16);
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
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label17);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 103);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1172, 24);
		this.panel12.TabIndex = 19;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(3, 2);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(116, 17);
		this.label17.TabIndex = 41;
		this.label17.Text = "Fillered Records";
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
		this.btnRefreshHistory.Visible = false;
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1186, 741);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "CommTradeLoadingDeliveryDirect";
		this.Text = "CommTradeLoadingDeliveryDirect";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(CommTradeLoadingDelivery_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(CommTradeLoadingDelivery_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMultiLanguage).EndInit();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBroker).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbTransporter).EndInit();
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbuom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).EndInit();
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.PanelExxpenses.ResumeLayout(false);
		this.PanelOtherRevenue.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTermSupplier).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.PanelPmExpense.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterial).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.tabControl2.ResumeLayout(false);
		this.tabDetail.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).EndInit();
		this.groupBox6.ResumeLayout(false);
		this.groupBox6.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.groupBox7.ResumeLayout(false);
		this.groupBox7.PerformLayout();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		base.ResumeLayout(false);
	}
}
