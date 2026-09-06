using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Purchase;

public class frmSupplierDispatch : Form
{
	private Attachment AT = new Attachment();

	private bool UpdateMode;

	private int updateDetailIndex = -1;

	private DropDownBind DDL = new DropDownBind();

	public static int RecId;

	private int DefaultDaysToLessFromHistoryFromDate;

	private DataTable table = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable dtHistory = new DataTable();

	private DataTable dtBranch = new DataTable();

	private DataTable dtOrders = new DataTable();

	private bool Approved;

	private Rightsobjects formright = new Rightsobjects();

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool HasMultiCurrencyFeature;

	private bool ItemSearchWithNameOrCode;

	private bool BranchFeature;

	private bool BranchImplemented;

	private bool FormInitialized;

	private string saveDirectory = "";

	private bool flagImage;

	private string VehicleImageFilePath = "";

	private string VehicleImageofd = "";

	private string DriverImageFilePath = "";

	private string DriverImageofd = "";

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private List<SupplierDispatchDetail> lstRemoveRecord = new List<SupplierDispatchDetail>();

	public UserAccount UserAccount = new UserAccount();

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

	private DateTimePicker txtDocDateMain;

	private Label label17;

	private TextBox txtDocNoMain;

	private Panel panel5;

	private GroupBox groupBox3;

	private Button btnplus;

	private TextBox txtRemarksdetail;

	private Label label47;

	private TextBox txtRateDetail;

	private TextBox txtAmountDetail;

	private TextBox txtWeightDetail;

	private TextBox txtQtyDetail;

	private Label label40;

	private Label label44;

	private Label label41;

	private Label label39;

	private Label label38;

	private Label label36;

	private Label label37;

	private Panel panel7;

	internal UltraCombo CmbItemNameDetail;

	internal UltraCombo CmbPackUomDetail;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar2;

	private Button btnUpdateDetail;

	private Button btnCancelUpdateDetial;

	private Label label3;

	internal UltraCombo CmbCropYearDetail;

	internal GridEX grdhistory;

	private ToolStripButton btnRefresh;

	private CheckBox ChkBox;

	internal GridEX DataGridHistoryDetail;

	private ToolStrip toolStrip2;

	internal UltraCombo CmbRateUomDetail;

	private ToolStripButton btnDelete;

	private ToolStripButton btnSaveAs;

	internal UltraCombo CmbPackingTypeDetail;

	private Label label1;

	private GroupBox groupBox1;

	private Panel panel10;

	internal UltraCombo CmbCurrencyMain;

	private Label label15;

	private Label label48;

	private TextBox txtExchangeRateMain;

	private TextBox txtFcyAmountMain;

	private Label label16;

	private ToolStripButton BtnShortCutkeys;

	private Panel panel8;

	private GroupBox groupBox2;

	private Button btnShow;

	private Label label51;

	private Label label52;

	private Label label53;

	private Label label54;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Panel panel9;

	private Panel panel12;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	private Panel panel13;

	private Label label49;

	private Label label55;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Panel panel14;

	private Label label5;

	private TextBox txtRemarksMain;

	private RadioButton rdSearchByCode;

	private RadioButton rdSearchByName;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private Panel panel6;

	internal UltraCombo cmbBranchName;

	private Label label2;

	private DateTimePicker txtETAdestinationMain;

	private DateTimePicker txtDepartureDateMain;

	private TextBox txtTotalAmountMain;

	private Label label22;

	private Label label11;

	private Label label9;

	private Label label7;

	private Label label6;

	private TextBox txtBiltyNoMain;

	private TextBox txtTransitDaysMain;

	private Button BtnBrowseDriverImage;

	private Button BtnBrowseVehicleImage;

	private PictureBox PicDriverImage;

	private PictureBox PicVehicleImage;

	private Label label12;

	private TextBox txtVehicleNoMain;

	internal UltraCombo CmbCityNameMain;

	private Label label28;

	private Label label20;

	private Label label19;

	private Label label14;

	private Label label26;

	private Label label25;

	private Label label24;

	private Label label21;

	private TextBox txtDriverCell2Main;

	private TextBox txtDriverCell1Main;

	private TextBox txtDriverNameMain;

	private TextBox txtOtherExpenseMain;

	private TextBox txtFreightMain;

	private TextBox txtTotalWeightMain;

	private TextBox txtQtyMain;

	internal UltraCombo CmbOrderNoDetail;

	private Label label4;

	internal UltraCombo CmbRefPartyDetail;

	private Label label18;

	internal UltraCombo CmbCommissionAgentDetail;

	private Label label13;

	internal UltraCombo CmbSupplierDetail;

	private Label label10;

	internal UltraCombo CmbBranchDetail;

	private Label label8;

	private TextBox txtBranchSrNo;

	private ToolStripButton BtnRefPartyForm;

	private Label label32;

	private Label label31;

	private Label label30;

	private Label label29;

	private Label label27;

	private Label label63;

	private Label label62;

	private Label label61;

	private Label label60;

	private Label label59;

	private Label label58;

	private Label label57;

	private Label label56;

	private Label label50;

	private Label label46;

	private Label label45;

	private Label label43;

	private Label label42;

	private Label label35;

	private Label label34;

	private Label label64;

	private Label label66;

	private Label label65;

	private TextBox txtSupplierWbWtMain;

	public frmSupplierDispatch(UserAccount user)
	{
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
	}

	private void SupplierDispatch_Load(object sender, EventArgs e)
	{
		try
		{
			InitializeForm();
			InitializeDataAsync();
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error during load: " + ex.Message);
		}
	}

	private void InitializeForm()
	{
		AutoScroll = true;
		base.AutoScrollMargin = new Size(5, 5);
		base.AutoScrollMinSize = new Size(1200, 500);
		btnupdate.Visible = false;
		btnDelete.Visible = false;
		btnsave.Visible = true;
		btnSaveAs.Visible = false;
		btnplus.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
		txtDocDateMain.Focus();
		InitializeHistory();
	}

	private async void InitializeDataAsync()
	{
		try
		{
			ApplyDataToUI(await Task.Run(delegate
			{
				Rightsobjects formRight = CommonServices.SetRightsValueInRightsObject(base.Name);
				bool eRPFeatureById = CommonServices.GetERPFeatureById(4);
				bool eRPFeatureById2 = CommonServices.GetERPFeatureById(6);
				bool eRPFeatureById3 = CommonServices.GetERPFeatureById(9);
				bool itemSearchByCode = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("ItemSearchByCode"));
				bool branchImplemented = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("SupplierDispatchBranchWise"));
				int defaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
				string docNo = "";
				string branchSrNo = "";
				if (RecId == 0)
				{
					docNo = DocumentNoFill();
					branchSrNo = BranchDocumentNoFill();
				}
				return new
				{
					formRight = formRight,
					subsidiaryAccountAllowed = eRPFeatureById,
					hasMultiCurrencyFeature = eRPFeatureById2,
					itemSearchByCode = itemSearchByCode,
					branchImplemented = branchImplemented,
					docNo = docNo,
					branchSrNo = branchSrNo,
					DefaultDaysToLessFromHistoryFromDate = defaultDaysToLessFromHistoryFromDate,
					BranchFeature = eRPFeatureById3
				};
			}));
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error during data initialization: " + ex.Message);
		}
	}

	private void InitializeHistory()
	{
		HistoryBranchComboFill();
		FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
		ToDateHistory.Value = DateTime.Now;
	}

	private void ApplyDataToUI(dynamic data)
	{
		formright = data.formRight;
		btnsave.Enabled = formright.DoHaveSaveRight;
		btnSaveAs.Enabled = formright.DoHaveSaveRight;
		btnprint.Enabled = formright.DoHavePrintRights;
		btnupdate.Enabled = formright.DoHaveUpdateRights;
		btnDelete.Enabled = formright.DoHaveCanDelete;
		ChkBox.Checked = formright.DoHavePrintRights;
		UpdateControlGroupRights(ctrlGrdBar1, formright);
		UpdateControlGroupRights(ctrlGrdBar2, formright);
		if (RecId == 0)
		{
			UpdateDocumentNoUI(data.docNo, data.branchSrNo);
		}
		BranchImplemented = data.branchImplemented;
		HandleMultiCurrencyFeature(data.hasMultiCurrencyFeature);
		DefaultDaysToLessFromHistoryFromDate = data.DefaultDaysToLessFromHistoryFromDate;
		HandleBranchFeature(data.BranchFeature);
		rdSearchByCode.Checked = data.itemSearchByCode;
		rdSearchByName.Checked = !data.itemSearchByCode;
	}

	private void UpdateControlGroupRights(dynamic ctrlGrdBar, dynamic rights)
	{
		ctrlGrdBar.mGridChooseFielder.Enabled = rights.DoHaveFieldChooserRights;
		ctrlGrdBar.mGridSaveLayouts.Enabled = rights.DoHaveSaveLayoutRights;
		ctrlGrdBar.mGridPrint.Enabled = rights.DoHaveGridPrintRights;
		ctrlGrdBar.mGridExport.Enabled = rights.DoHaveGridExportRights;
		ctrlGrdBar.GroupCollaps.Enabled = rights.DoHaveGridGroupCollapseRights;
		ctrlGrdBar.GroupExpand.Enabled = rights.DoHaveGridGroupExpandRights;
	}

	private void HandleBranchFeature(bool BranchFeature)
	{
		label8.Visible = BranchFeature && !BranchImplemented;
		((Control)(object)CmbBranchDetail).Visible = BranchFeature && !BranchImplemented;
		txtRemarksdetail.Size = new Size(489, 23);
		if (BranchFeature && !BranchImplemented)
		{
			txtRemarksdetail.Size = new Size(343, 23);
		}
	}

	private void HandleMultiCurrencyFeature(bool hasMultiCurrencyFeature)
	{
		((Control)(object)CmbCurrencyMain).Visible = hasMultiCurrencyFeature;
		txtExchangeRateMain.Visible = hasMultiCurrencyFeature;
		txtFcyAmountMain.Visible = hasMultiCurrencyFeature;
		((Control)(object)CmbCurrencyMain).TabStop = hasMultiCurrencyFeature;
		txtExchangeRateMain.TabStop = hasMultiCurrencyFeature;
		txtFcyAmountMain.TabStop = hasMultiCurrencyFeature;
		label15.Visible = hasMultiCurrencyFeature;
		label16.Visible = hasMultiCurrencyFeature;
		label48.Visible = hasMultiCurrencyFeature;
		label43.Visible = hasMultiCurrencyFeature;
		if (!hasMultiCurrencyFeature)
		{
			GetBaseCurrencyAndRate();
		}
	}

	private void InitializeComponentCustom()
	{
		try
		{
			FormInitialized = true;
			dtBranch.Columns.Add("Id", typeof(int));
			dtBranch.Columns.Add("BranchName", typeof(string));
			table.Columns.Add("Id", typeof(int));
			table.Columns.Add("SupplierId", typeof(int));
			table.Columns.Add("Supplier");
			table.Columns.Add("OrderId", typeof(int));
			table.Columns.Add("OrderNo");
			table.Columns.Add("CommissionAgentId", typeof(int));
			table.Columns.Add("CommissionAgent");
			table.Columns.Add("ReferencePartyId", typeof(int));
			table.Columns.Add("ReferenceParty");
			table.Columns.Add("OrderDetailId", typeof(int));
			table.Columns.Add("ItemId", typeof(int));
			table.Columns.Add("ItemCode", typeof(string));
			table.Columns.Add("ItemName", typeof(string));
			table.Columns.Add("CropYearId", typeof(int));
			table.Columns.Add("CropYear");
			table.Columns.Add("PackingTypeId", typeof(int));
			table.Columns.Add("PackingType");
			table.Columns.Add("PackUomId", typeof(int));
			table.Columns.Add("PackUom");
			table.Columns.Add("PackUomEquivalent", typeof(double));
			table.Columns.Add("Qty", typeof(double));
			table.Columns.Add("Weight", typeof(double));
			table.Columns.Add("RateUomId", typeof(int));
			table.Columns.Add("RateUom");
			table.Columns.Add("RateUomEquivalent", typeof(double));
			table.Columns.Add("Rate", typeof(double));
			table.Columns.Add("Amount", typeof(double));
			table.Columns.Add("FcyAmount", typeof(double));
			table.Columns.Add("Remarks");
			table.Columns.Add("ReceiverLocationId", typeof(int));
			table.Columns.Add("ReceiverLocation");
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName");
			dtitem.Columns.Add("ItemCode");
			dtitem.Columns.Add("CropYearId", typeof(int));
			dtitem.Columns.Add("ItemUomId", typeof(int));
			dtitem.Columns.Add("ItemQty", typeof(double));
			dtitem.Columns.Add("NetWeight", typeof(double));
			dtitem.Columns.Add("ItemRate", typeof(double));
			dtitem.Columns.Add("RateUomId", typeof(int));
			dtitem.Columns.Add("OrderDetailId", typeof(int));
			cmbCityFill();
			CurrencyFill();
			SupplierNameFill();
			ReferencePartyFill();
			ItemWithoutOrderFill();
			CropYear();
			CmbPackingTypeFill();
			BranchDetailFill();
			ConfigurationDefault();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ConfigurationDefault()
	{
		try
		{
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "City Area").ToList();
			if (list.Count > 0)
			{
				int CityId = Conversion.ToInt((object)list[0].ConfigKey.ToString());
				CmbCityNameMain.Value = CityId;
			}
			List<ConfigrationsAllocation> lstCrop = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Default Crop Year").ToList();
			if (lstCrop.Count > 0)
			{
				int CropYearId = Conversion.ToInt((object)lstCrop[0].ConfigKey.ToString());
				CmbCropYearDetail.Value = CropYearId;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private string DocumentNoFill()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		string Code = "";
		try
		{
			Code = Conversion.ToString((object)SupplierDispatch.GenerateCode(new SupplierDispatch
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 246,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			}));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return Code.ToString();
	}

	private string BranchDocumentNoFill()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		string Code = "";
		try
		{
			Code = Conversion.ToString((object)SupplierDispatch.GenerateBranchCode(new SupplierDispatch
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 246,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			}));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return Code.ToString();
	}

	private void UpdateDocumentNoUI(string DocNo, string BranchSrNo)
	{
		txtDocNoMain.Text = DocNo;
		txtBranchSrNo.Text = BranchSrNo;
	}

	private void cmbCityFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int ID = Conversion.ToInt(CmbCityNameMain.Value);
			DataTable dt = City.GetAll(new City
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbCityNameMain, "Id", "CityName", "City Name", true);
				((UltraGridBase)CmbCityNameMain).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbCityNameMain.Value = ID;
					}
					else
					{
						((Control)(object)CmbCityNameMain).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbCityNameMain).Text = string.Empty;
				((UltraGridBase)CmbCityNameMain).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void SupplierNameFill()
	{
		try
		{
			int ID = Conversion.ToInt(CmbSupplierDetail.Value);
			DataTable dtSupplier = new DataTable();
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("CompanyName", typeof(string));
			dtSupplier.Columns.Add("GlAccountId", typeof(int));
			if (SubsidiaryAccountAllownOnVouchers)
			{
				DataTable Supplierlst = CommonServices.GetVendorsAndCustomers(1);
				if (Supplierlst.Rows.Count > 0)
				{
					dtSupplier.Clear();
					foreach (DataRow row2 in Supplierlst.Rows)
					{
						dtSupplier.Rows.Add(row2["Id"], row2["CompanyName"], row2["GlAccountId"]);
					}
				}
			}
			else
			{
				DataTable Supplierlst2 = CommonServices.SupplierCustomerGetforComboServiceBind();
				if (Supplierlst2.Rows.Count > 0)
				{
					dtSupplier.Clear();
					foreach (DataRow row3 in Supplierlst2.Rows)
					{
						dtSupplier.Rows.Add(row3["Id"], row3["CompanyName"], row3["GlAccountId"]);
					}
				}
			}
			if (dtSupplier.Rows.Count > 0)
			{
				DDL.BindDDL(dtSupplier, CmbSupplierDetail, "Id", "CompanyName", "Supplier Name", ZeroIndex: true);
				((UltraGridBase)CmbSupplierDetail).DisplayLayout.Bands[0].Columns[1].Width = 300;
				if (ID > 0)
				{
					if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbSupplierDetail.Value = ID;
					}
					else
					{
						((Control)(object)CmbSupplierDetail).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbSupplierDetail).Text = string.Empty;
				((UltraGridBase)CmbSupplierDetail).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReferencePartyFill()
	{
		try
		{
			int ItId = Conversion.ToInt(CmbRefPartyDetail.Value);
			DataTable dt = CommonServices.ReferencePartyServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbRefPartyDetail, "Id", "ReferencePartyName", "ReferencePartyName", true);
				((UltraGridBase)CmbRefPartyDetail).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (ItId > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
					{
						CmbRefPartyDetail.Value = ItId;
					}
					else
					{
						((Control)(object)CmbRefPartyDetail).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbRefPartyDetail).Text = string.Empty;
				((UltraGridBase)CmbRefPartyDetail).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindRateUomAndItemPackUom(int ID)
	{
		try
		{
			string Packuom = ((Control)(object)CmbPackUomDetail).Text;
			string Rateuom = ((Control)(object)CmbRateUomDetail).Text;
			((Control)(object)CmbPackUomDetail).Text = string.Empty;
			((Control)(object)CmbRateUomDetail).Text = string.Empty;
			DataTable dtUom = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(CmbItemNameDetail.Value));
			if (dtUom.Rows.Count > 0)
			{
				DDL.BindDDL(dtUom, CmbPackUomDetail, "Id", "UOMCode", "PackUOM", ZeroIndex: false);
				((UltraGridBase)CmbPackUomDetail).DisplayLayout.Bands[0].Columns[1].Width = 75;
				((UltraGridBase)CmbPackUomDetail).DisplayLayout.Bands[0].Columns[2].Width = 85;
				DDL.BindDDL(dtUom, CmbRateUomDetail, "Id", "UOMCode", "RateUOM", ZeroIndex: false);
				((UltraGridBase)CmbRateUomDetail).DisplayLayout.Bands[0].Columns[1].Width = 75;
				((UltraGridBase)CmbRateUomDetail).DisplayLayout.Bands[0].Columns[2].Width = 85;
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Packuom))
				{
					((Control)(object)CmbPackUomDetail).Text = Packuom;
				}
				else
				{
					((Control)(object)CmbPackUomDetail).Text = string.Empty;
				}
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Rateuom))
				{
					((Control)(object)CmbRateUomDetail).Text = Rateuom;
				}
				else
				{
					((Control)(object)CmbRateUomDetail).Text = string.Empty;
				}
			}
			else
			{
				((Control)(object)CmbPackUomDetail).Text = string.Empty;
				((Control)(object)CmbRateUomDetail).Text = string.Empty;
				((UltraGridBase)CmbPackUomDetail).DataSource = null;
				((UltraGridBase)CmbRateUomDetail).DataSource = null;
			}
			DataTable dtuomsch = CommonServices.GetUomScheduleByItemId(ID);
			if (dtuomsch.Rows.Count > 0)
			{
				DDL.BindDDL(dtuomsch, CmbPackUomDetail, "Id", "UOMCode", "PackUom", ZeroIndex: false);
				DDL.BindDDL(dtuomsch, CmbRateUomDetail, "Id", "UOMCode", "RateUom", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CropYear()
	{
		try
		{
			string ID = Conversion.ToString((object)((Control)(object)CmbCropYearDetail).Text);
			DataTable dtCrop = CommonServices.CropYearGetAllService();
			if (dtCrop.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCrop, CmbCropYearDetail, "Id", "CropYear", "Crop Year", true);
				if (!string.IsNullOrEmpty(ID))
				{
					if (dtCrop.AsEnumerable().Any((DataRow row) => row.Field<string>("CropYear") == ID))
					{
						((Control)(object)CmbCropYearDetail).Text = ID;
					}
					else
					{
						((Control)(object)CmbCropYearDetail).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbCropYearDetail).Text = string.Empty;
				((UltraGridBase)CmbCropYearDetail).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BranchDetailFill()
	{
		try
		{
			int ID = Conversion.ToInt(CmbBranchDetail.Value);
			DataTable dtbranch = BranchesAllocationToUser.GetBranchsAllocatedToUser(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID);
			if (dtbranch.Rows.Count > 0)
			{
				DDL.BindDDL(dtbranch, CmbBranchDetail, "BranchId", "BranchName", "BranchName", ZeroIndex: true);
				((UltraGridBase)CmbBranchDetail).Rows[1].Activate();
				if (ID != 0)
				{
					if (dtbranch.AsEnumerable().Any((DataRow row) => row.Field<int>("BranchId") == ID))
					{
						CmbBranchDetail.Value = ID;
					}
					else
					{
						((Control)(object)CmbBranchDetail).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbBranchDetail).Text = string.Empty;
				((UltraGridBase)CmbBranchDetail).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbPackingTypeFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbPackingTypeDetail.Value);
			DataTable dt = InvPackingType.Getall();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbPackingTypeDetail, "Id", "PackTypeDesc", "PackTypeDesc", true);
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbPackingTypeDetail.Value = Id;
					}
					else
					{
						((Control)(object)CmbPackingTypeDetail).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbPackingTypeDetail).Text = string.Empty;
				((UltraGridBase)CmbPackingTypeDetail).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void combitem_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			bindRateUomAndItemPackUom(Conversion.ToInt(CmbItemNameDetail.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdSearchByName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtitem.Rows.Count > 0)
			{
				int ItemId = 0;
				if (Conversion.ToInt(CmbItemNameDetail.Value) > 0)
				{
					ItemId = Conversion.ToInt(CmbItemNameDetail.Value);
				}
				if (rdSearchByName.Checked)
				{
					DDL.BindDDL(dtitem, CmbItemNameDetail, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, CmbItemNameDetail, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				CmbItemNameDetail.Value = ItemId;
				CmbItemNameDetail.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSupplierDetail_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbSupplierDetail).ActiveRow != null && Conversion.ToInt(CmbSupplierDetail.Value) > 0)
			{
				OrderBind(Conversion.ToInt(CmbSupplierDetail.Value));
				return;
			}
			((Control)(object)CmbOrderNoDetail).Text = string.Empty;
			((UltraGridBase)CmbOrderNoDetail).DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbOrderNoDetail_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbOrderNoDetail).ActiveRow != null && Conversion.ToInt(CmbOrderNoDetail.Value) > 0)
			{
				BindOtherCombosByOrder(Conversion.ToInt(CmbOrderNoDetail.Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindOtherCombosByOrder(int OrderId)
	{
		try
		{
			if (OrderId > 0 && dtOrders.Rows.Count > 0)
			{
				BindCommissionAgent(OrderId);
				BindItem(OrderId);
			}
			else
			{
				ItemWithoutOrderFill();
				((UltraGridBase)CmbCommissionAgentDetail).DataSource = null;
				((Control)(object)CmbCommissionAgentDetail).Text = "";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindCommissionAgent(int OrderId)
	{
		try
		{
			int IDComm = Conversion.ToInt(CmbCommissionAgentDetail.Value);
			DataTable dtComm = new DataTable();
			dtComm.Columns.Add("CommissionAgentId", typeof(int));
			dtComm.Columns.Add("CommissionAgent", typeof(string));
			foreach (DataRow dr in dtOrders.Rows)
			{
				if (Conversion.ToInt(dr["OrderId"]) == OrderId && Conversion.ToInt(dr["CommissionAgentId"]) > 0)
				{
					dtComm.Rows.Add(dr["CommissionAgentId"], dr["CommissionAgent"]);
				}
			}
			if (dtComm.Rows.Count > 0)
			{
				DDL.BindDDL(dtComm, CmbCommissionAgentDetail, "CommissionAgentId", "CommissionAgent", "CommissionAgent", ZeroIndex: true);
				if (IDComm > 0)
				{
					if (dtComm.AsEnumerable().Any((DataRow row) => row.Field<int>("CommissionAgentId") == IDComm))
					{
						CmbCommissionAgentDetail.Value = IDComm;
					}
					else
					{
						((Control)(object)CmbCommissionAgentDetail).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbCommissionAgentDetail).Text = string.Empty;
				((UltraGridBase)CmbCommissionAgentDetail).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindItem(int OrderId)
	{
		try
		{
			int ID = Conversion.ToInt(CmbItemNameDetail.Value);
			dtitem.Rows.Clear();
			for (int i = 0; i < dtOrders.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtOrders.Rows[i]["OrderId"]) == OrderId)
				{
					dtitem.Rows.Add(dtOrders.Rows[i]["OrderItemId"], dtOrders.Rows[i]["ItemName"], dtOrders.Rows[i]["ItemCode"], dtOrders.Rows[i]["CropYearId"], dtOrders.Rows[i]["OrderItemUOMId"], dtOrders.Rows[i]["OrderItemQty"], dtOrders.Rows[i]["NetWeight"], dtOrders.Rows[i]["OrderItemRate"], dtOrders.Rows[i]["OrderItemRateUOMId"], dtOrders.Rows[i]["OrderDetailId"]);
				}
			}
			if (dtitem.Rows.Count > 0)
			{
				if (rdSearchByName.Checked)
				{
					DDL.BindDDL(dtitem, CmbItemNameDetail, "Id", "ItemName", "Item", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, CmbItemNameDetail, "Id", "ItemCode", "Item", ZeroIndex: true);
				}
				((UltraGridBase)CmbItemNameDetail).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)CmbItemNameDetail).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbItemNameDetail).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)CmbItemNameDetail).DisplayLayout.Bands[0].Columns[5].Hidden = true;
				((UltraGridBase)CmbItemNameDetail).DisplayLayout.Bands[0].Columns[6].Hidden = true;
				((UltraGridBase)CmbItemNameDetail).DisplayLayout.Bands[0].Columns[7].Hidden = true;
				((UltraGridBase)CmbItemNameDetail).DisplayLayout.Bands[0].Columns[8].Hidden = true;
				((UltraGridBase)CmbItemNameDetail).DisplayLayout.Bands[0].Columns[9].Hidden = true;
				if (ID > 0)
				{
					if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbItemNameDetail.Value = ID;
					}
					else
					{
						((Control)(object)CmbItemNameDetail).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbItemNameDetail).Text = string.Empty;
				((UltraGridBase)CmbItemNameDetail).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OrderBind(int SupplierCustomerId)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(CmbOrderNoDetail.Value);
			dtOrders.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (BranchFeature && BranchImplemented)
			{
				obj.BranchesId = UserAccount.BranchesId;
			}
			obj.SupplierCustomerId = SupplierCustomerId;
			dtOrders = SupplierDispatch.PurchaseOrder_ForSupplierDispatch(obj);
			if (dtOrders.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtOrders, CmbOrderNoDetail, "OrderId", "OrderNo", "Order No", false);
				if (Id > 0)
				{
					if (dtOrders.AsEnumerable().Any((DataRow row) => row.Field<int>("OrderId") == Id))
					{
						CmbOrderNoDetail.Value = Id;
					}
					else
					{
						((Control)(object)CmbOrderNoDetail).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbOrderNoDetail).Text = string.Empty;
				((UltraGridBase)CmbOrderNoDetail).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemWithoutOrderFill()
	{
		try
		{
			int ID = Conversion.ToInt(CmbItemNameDetail.Value);
			dtitem.Rows.Clear();
			DataTable dtItemAll = CommonServices.ReadAllItems();
			if (dtItemAll.Rows.Count > 0)
			{
				for (int i = 0; i < dtItemAll.Rows.Count; i++)
				{
					dtitem.Rows.Add(dtItemAll.Rows[i]["Id"], dtItemAll.Rows[i]["ItemName"], dtItemAll.Rows[i]["ItemCodeNew"], 0, 0, 0, 0, 0, 0, 0);
				}
			}
			if (dtitem.Rows.Count > 0)
			{
				if (rdSearchByName.Checked)
				{
					DDL.BindDDL(dtitem, CmbItemNameDetail, "Id", "ItemName", "Item", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, CmbItemNameDetail, "Id", "ItemCode", "Item", ZeroIndex: true);
				}
				((UltraGridBase)CmbItemNameDetail).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)CmbItemNameDetail).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbItemNameDetail).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)CmbItemNameDetail).DisplayLayout.Bands[0].Columns[5].Hidden = true;
				((UltraGridBase)CmbItemNameDetail).DisplayLayout.Bands[0].Columns[6].Hidden = true;
				((UltraGridBase)CmbItemNameDetail).DisplayLayout.Bands[0].Columns[7].Hidden = true;
				((UltraGridBase)CmbItemNameDetail).DisplayLayout.Bands[0].Columns[8].Hidden = true;
				((UltraGridBase)CmbItemNameDetail).DisplayLayout.Bands[0].Columns[9].Hidden = true;
				if (ID > 0)
				{
					if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbItemNameDetail.Value = ID;
					}
					else
					{
						((Control)(object)CmbItemNameDetail).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbItemNameDetail).Text = string.Empty;
				((UltraGridBase)CmbItemNameDetail).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void combitem_Leave(object sender, EventArgs e)
	{
		try
		{
			bindRateUomAndItemPackUom(Conversion.ToInt(CmbItemNameDetail.Value));
			if (((UltraGridBase)CmbOrderNoDetail).ActiveRow != null && Conversion.ToInt(CmbOrderNoDetail.Value) > 0 && ((UltraGridBase)CmbItemNameDetail).ActiveRow != null && Conversion.ToInt(CmbItemNameDetail.Value) > 0)
			{
				if (Conversion.ToInt(CmbCropYearDetail.Value) == 0)
				{
					CmbCropYearDetail.Value = Conversion.ToInt(((UltraDropDownBase)CmbItemNameDetail).SelectedRow.Cells["CropYearId"].Value);
				}
				if (Conversion.ToInt(CmbPackUomDetail.Value) == 0)
				{
					CmbPackUomDetail.Value = Conversion.ToInt(((UltraDropDownBase)CmbItemNameDetail).SelectedRow.Cells["ItemUomId"].Value);
				}
				if (Conversion.ToDouble((object)txtQtyDetail.Text) == 0.0)
				{
					txtQtyDetail.Text = Conversion.ToDouble(((UltraDropDownBase)CmbItemNameDetail).SelectedRow.Cells["ItemQty"].Value).ToString("#,##.##");
				}
				if (Conversion.ToDouble((object)txtWeightDetail.Text) == 0.0)
				{
					txtWeightDetail.Text = Conversion.ToDouble(((UltraDropDownBase)CmbItemNameDetail).SelectedRow.Cells["NetWeight"].Value).ToString("#,##.##");
				}
				if (Conversion.ToDouble((object)txtRateDetail.Text) == 0.0)
				{
					txtRateDetail.Text = Conversion.ToDouble(((UltraDropDownBase)CmbItemNameDetail).SelectedRow.Cells["ItemRate"].Value).ToString("#,##.##");
				}
				if (Conversion.ToInt(CmbRateUomDetail.Value) == 0)
				{
					CmbRateUomDetail.Value = Conversion.ToInt(((UltraDropDownBase)CmbItemNameDetail).SelectedRow.Cells["RateUomId"].Value);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtDocNoMain.Text.Trim() == string.Empty || txtDocNoMain.Text.Trim() == "0" || txtDocNoMain.Text.Trim() == null)
		{
			MessageBox.Show("Doc No Field is Required");
			txtDocNoMain.Focus();
			return false;
		}
		if (txtVehicleNoMain.Text.Trim() == string.Empty || txtVehicleNoMain.Text.Trim() == "0" || txtVehicleNoMain.Text.Trim() == null)
		{
			MessageBox.Show("Vehicle No Field is Required");
			txtVehicleNoMain.Focus();
			return false;
		}
		if (txtBiltyNoMain.Text.Trim() == string.Empty || txtBiltyNoMain.Text.Trim() == "0" || txtBiltyNoMain.Text.Trim() == null)
		{
			MessageBox.Show("Bilty No Field is Required");
			txtBiltyNoMain.Focus();
			return false;
		}
		if (txtDriverNameMain.Text.Trim() == string.Empty || txtDriverNameMain.Text.Trim() == "0" || txtDriverNameMain.Text.Trim() == null)
		{
			MessageBox.Show("Driver Name Field is Required");
			txtDriverNameMain.Focus();
			return false;
		}
		if (txtDriverCell1Main.Text.Trim() == string.Empty || txtDriverCell1Main.Text.Trim() == "0" || txtDriverCell1Main.Text.Trim() == null)
		{
			MessageBox.Show("Driver CellNo Field is Required");
			txtDriverCell1Main.Focus();
			return false;
		}
		if (txtTransitDaysMain.Text.Trim() == string.Empty || Conversion.ToInt((object)txtTransitDaysMain.Text) == 0)
		{
			MessageBox.Show("TransitDays Field is Required");
			txtTransitDaysMain.Focus();
			return false;
		}
		if (txtFreightMain.Text.Trim() == string.Empty || Conversion.ToInt((object)txtFreightMain.Text) == 0)
		{
			MessageBox.Show("Freight Field is Required");
			txtFreightMain.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCityNameMain).ActiveRow == null || Conversion.ToInt(CmbCityNameMain.Value) == 0)
		{
			MessageBox.Show("CityName Field is Required");
			CmbCityNameMain.Focus();
			return false;
		}
		if (HasMultiCurrencyFeature)
		{
			if (((UltraGridBase)CmbCurrencyMain).ActiveRow == null || Conversion.ToInt(CmbCurrencyMain.Value) == 0)
			{
				MessageBox.Show("Fcy Code Field is Required");
				CmbCurrencyMain.Focus();
				return false;
			}
			if (txtExchangeRateMain.Text.Trim() == string.Empty || txtExchangeRateMain.Text.Trim() == "0")
			{
				MessageBox.Show("Exchange Rate Field is Required");
				txtExchangeRateMain.Focus();
				return false;
			}
			if (txtFcyAmountMain.Text.Trim() == string.Empty || txtFcyAmountMain.Text.Trim() == "0")
			{
				MessageBox.Show("Fcy Amount Rate Field is Required");
				txtFcyAmountMain.Focus();
				return false;
			}
		}
		else
		{
			if (((UltraGridBase)CmbCurrencyMain).ActiveRow == null || Conversion.ToInt(CmbCurrencyMain.Value) == 0)
			{
				MessageBox.Show("Please Configure Your Base Currency In configurations");
				CmbCurrencyMain.Focus();
				return false;
			}
			if (txtExchangeRateMain.Text.Trim() == string.Empty || txtExchangeRateMain.Text.Trim() == "0")
			{
				MessageBox.Show("Please Configure Your Base Currency Rate In configurations");
				txtExchangeRateMain.Focus();
				return false;
			}
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)CmbSupplierDetail).ActiveRow == null || Conversion.ToInt(CmbSupplierDetail.Value) == 0)
		{
			MessageBox.Show("Supplier Name Field is Required");
			CmbSupplierDetail.Focus();
			return false;
		}
		if (((UltraGridBase)CmbItemNameDetail).ActiveRow == null || Conversion.ToInt(CmbItemNameDetail.Value) == 0)
		{
			MessageBox.Show("Item Name Field is Required");
			CmbItemNameDetail.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCropYearDetail).ActiveRow == null || Conversion.ToInt(CmbCropYearDetail.Value) == 0)
		{
			MessageBox.Show("Crop Year Field is Required");
			CmbCropYearDetail.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackingTypeDetail).ActiveRow == null || Conversion.ToInt(CmbPackingTypeDetail.Value) == 0)
		{
			MessageBox.Show("Pack Type Field is Required");
			CmbPackingTypeDetail.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUomDetail).ActiveRow == null || Conversion.ToInt(CmbPackUomDetail.Value) == 0)
		{
			MessageBox.Show("Item Pack Unit Field is Required");
			CmbPackUomDetail.Focus();
			return false;
		}
		if (txtQtyDetail.Text.Trim() == string.Empty || txtQtyDetail.Text.Trim() == "0" || Conversion.ToDouble((object)txtQtyDetail.Text) == 0.0)
		{
			MessageBox.Show("Qty Field is Required");
			txtQtyDetail.Focus();
			return false;
		}
		if (txtWeightDetail.Text.Trim() == string.Empty || txtWeightDetail.Text.Trim() == "0" || Conversion.ToDouble((object)txtWeightDetail.Text) == 0.0)
		{
			MessageBox.Show("Net Weight Field is Required");
			txtWeightDetail.Focus();
			return false;
		}
		if (txtRateDetail.Text.Trim() == string.Empty || txtRateDetail.Text.Trim() == "0" || Conversion.ToDouble((object)txtRateDetail.Text) == 0.0)
		{
			MessageBox.Show("Item Rate Field is Required");
			txtRateDetail.Focus();
			return false;
		}
		if (((UltraGridBase)CmbRateUomDetail).ActiveRow == null || Conversion.ToInt(CmbRateUomDetail.Value) == 0)
		{
			MessageBox.Show("Rate UOM Field is Required");
			CmbRateUomDetail.Focus();
			return false;
		}
		if (txtAmountDetail.Text.Trim() == string.Empty || txtAmountDetail.Text.Trim() == "0" || Conversion.ToDouble((object)txtAmountDetail.Text) == 0.0)
		{
			MessageBox.Show("Item amount Field is Required");
			txtAmountDetail.Focus();
			return false;
		}
		if (BranchFeature && (((UltraGridBase)CmbBranchDetail).ActiveRow == null || Conversion.ToInt(CmbBranchDetail.Value) == 0))
		{
			MessageBox.Show("Receiver's Location Field is Required");
			CmbBranchDetail.Focus();
			return false;
		}
		return true;
	}

	private void ChkCommission_Click(object sender, EventArgs e)
	{
	}

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				int OrderDetailId = 0;
				int ItemId = Conversion.ToInt(CmbItemNameDetail.Value);
				string ItemName = Conversion.ToString(((UltraDropDownBase)CmbItemNameDetail).SelectedRow.Cells["ItemName"].Value);
				string ItemCode = Conversion.ToString(((UltraDropDownBase)CmbItemNameDetail).SelectedRow.Cells["ItemCode"].Value);
				if (Conversion.ToInt(CmbOrderNoDetail.Value) > 0 && ItemId > 0)
				{
					OrderDetailId = Conversion.ToInt(((UltraDropDownBase)CmbItemNameDetail).SelectedRow.Cells["OrderDetailId"].Value);
				}
				table.Rows.Add(0, Conversion.ToInt(CmbSupplierDetail.Value), Conversion.ToString((object)((Control)(object)CmbSupplierDetail).Text.Trim()), Conversion.ToInt(CmbOrderNoDetail.Value), Conversion.ToString((object)((Control)(object)CmbOrderNoDetail).Text.Trim()), Conversion.ToInt(CmbCommissionAgentDetail.Value), Conversion.ToString((object)((Control)(object)CmbCommissionAgentDetail).Text.Trim()), Conversion.ToInt(CmbRefPartyDetail.Value), Conversion.ToString((object)((Control)(object)CmbRefPartyDetail).Text.Trim()), OrderDetailId, ItemId, ItemCode, ItemName, Conversion.ToInt(CmbCropYearDetail.Value), Conversion.ToString((object)((Control)(object)CmbCropYearDetail).Text.Trim()), Conversion.ToInt(CmbPackingTypeDetail.Value), Conversion.ToString((object)((Control)(object)CmbPackingTypeDetail).Text.Trim()), Conversion.ToInt(CmbPackUomDetail.Value), Conversion.ToString((object)((Control)(object)CmbPackUomDetail).Text.Trim()), Conversion.ToDouble(((UltraDropDownBase)CmbPackUomDetail).SelectedRow.Cells[2].Value), Conversion.ToDouble((object)txtQtyDetail.Text.Trim()), Conversion.ToDouble((object)txtWeightDetail.Text.Trim()), Conversion.ToInt(CmbRateUomDetail.Value), Conversion.ToString((object)((Control)(object)CmbRateUomDetail).Text), Conversion.ToDouble(((UltraDropDownBase)CmbRateUomDetail).SelectedRow.Cells[2].Value), Conversion.ToDouble((object)txtRateDetail.Text.Trim()), Conversion.ToDouble((object)txtAmountDetail.Text), (Conversion.ToDecimal((object)txtExchangeRateMain.Text) > 0m && Conversion.ToDecimal((object)txtAmountDetail.Text) > 0m) ? (Conversion.ToDecimal((object)txtAmountDetail.Text) / Conversion.ToDecimal((object)txtExchangeRateMain.Text)) : 0m, Conversion.ToString((object)txtRemarksdetail.Text.Trim()), Conversion.ToInt(CmbBranchDetail.Value), Conversion.ToString((object)((Control)(object)CmbBranchDetail).Text));
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				((Control)(object)CmbItemNameDetail).Text = string.Empty;
				((Control)(object)CmbPackUomDetail).Text = string.Empty;
				((Control)(object)CmbPackingTypeDetail).Text = string.Empty;
				txtQtyDetail.Text = string.Empty;
				txtWeightDetail.Text = string.Empty;
				((Control)(object)CmbRateUomDetail).Text = string.Empty;
				txtRateDetail.Text = string.Empty;
				txtAmountDetail.Text = string.Empty;
				txtRemarksdetail.Text = string.Empty;
				CmbItemNameDetail.Focus();
				CalculateTotalInformation();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
				if (e.Column.Key == "Edit")
				{
					grd_DoubleClick(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (updateDetailIndex == -1)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
					{
						SupplierDispatchDetail vd = new SupplierDispatchDetail();
						vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
						vd.SupplierId = Conversion.ToInt(r.Cells["SupplierId"].Value);
						vd.OrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
						vd.CommissionAgentId = Conversion.ToInt(r.Cells["CommissionAgentId"].Value);
						vd.ReferencePartyId = Conversion.ToInt(r.Cells["ReferencePartyId"].Value);
						vd.OrderDetailId = Conversion.ToInt(r.Cells["OrderDetailId"].Value);
						vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						vd.CropYearId = Conversion.ToInt((object)r.Cells["CropYearId"].Text);
						vd.PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
						vd.PackSizeId = Conversion.ToInt(r.Cells["PackUomId"].Value);
						vd.Qty = Conversion.ToDouble(r.Cells["Qty"].Value);
						vd.WeightKgs = Conversion.ToDouble(r.Cells["Weight"].Value);
						vd.RateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
						vd.Rate = Conversion.ToDouble(r.Cells["Rate"].Value);
						vd.Amount = Conversion.ToDouble(r.Cells["Amount"].Value);
						vd.CurrencyId = Conversion.ToInt(CmbCurrencyMain.Value);
						vd.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRateMain.Text);
						vd.FcyAmount = Conversion.ToDecimal(r.Cells["FcyAmount"].Value);
						vd.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
						vd.BranchId = Conversion.ToInt(r.Cells["ReceiverLocationId"].Value);
						vd.ActionTypeId = 3;
						lstRemoveRecord.Add(vd);
						table.Rows[r.RowIndex].Delete();
						r.Delete();
						grd.UpdateData();
					}
				}
				else
				{
					r.Delete();
					grd.UpdateData();
				}
				return;
			}
			throw new Exception("Please Reset the Detail First...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Expected O, but got Unknown
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["SupplierId"].Visible = false;
			grd.RootTable.Columns["OrderId"].Visible = false;
			grd.RootTable.Columns["CommissionAgentId"].Visible = false;
			grd.RootTable.Columns["ReferencePartyId"].Visible = false;
			grd.RootTable.Columns["OrderDetailId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["CropYearId"].Visible = false;
			grd.RootTable.Columns["PackingTypeId"].Visible = false;
			grd.RootTable.Columns["PackUomId"].Visible = false;
			grd.RootTable.Columns["PackUomEquivalent"].Visible = false;
			grd.RootTable.Columns["RateUomId"].Visible = false;
			grd.RootTable.Columns["RateUomEquivalent"].Visible = false;
			grd.RootTable.Columns["FcyAmount"].Visible = HasMultiCurrencyFeature;
			grd.RootTable.Columns["ReceiverLocationId"].Visible = false;
			grd.RootTable.Columns["ReceiverLocation"].Visible = BranchFeature && !BranchImplemented;
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.HeaderAlignment = (TextAlignment)2;
				column.EditType = (EditType)0;
				if (column.Type == typeof(double) || column.Type == typeof(decimal))
				{
					ConfigureNumericalColumn(column);
				}
			}
			grd.RootTable.Columns["Supplier"].Width = Constants.InventoryConstants.SupplierName;
			grd.RootTable.Columns["OrderNo"].Width = Constants.InventoryConstants.OrderNo;
			grd.RootTable.Columns["CommissionAgent"].Width = Constants.InventoryConstants.CommissionAgent;
			grd.RootTable.Columns["ReferenceParty"].Width = Constants.InventoryConstants.ReferenceParty;
			grd.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grd.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grd.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			grd.RootTable.Columns["PackingType"].Width = Constants.InventoryConstants.PackingType;
			grd.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["Weight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["FcyAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grd.RootTable.Columns["ReceiverLocation"].Width = Constants.InventoryConstants.BranchName;
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 30;
			grd.RootTable.Columns["Delete"].Position = 0;
			grd.RootTable.Columns.Add("Edit");
			grd.RootTable.Columns["Edit"].Key = "Edit";
			grd.RootTable.Columns["Edit"].Caption = "Edit";
			grd.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Edit"].ButtonText = "Edit";
			grd.RootTable.Columns["Edit"].Width = 60;
			grd.RootTable.Columns["Edit"].Position = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				updateDetailIndex = r.RowIndex;
				CmbSupplierDetail.Value = r.Cells["SupplierId"].Value.ToString();
				CmbSupplierDetail_Leave(null, null);
				CmbOrderNoDetail.Value = r.Cells["OrderId"].Value.ToString();
				CmbOrderNoDetail_Leave(null, null);
				CmbCommissionAgentDetail.Value = r.Cells["CommissionAgentId"].Value;
				CmbRefPartyDetail.Value = r.Cells["ReferencePartyId"].Value;
				CmbItemNameDetail.Value = r.Cells["ItemId"].Value.ToString();
				CmbRateUomDetail.Value = r.Cells["RateUomId"].Value.ToString();
				CmbPackUomDetail.Value = r.Cells["PackUomId"].Value.ToString();
				combitem_Leave(null, null);
				CmbCropYearDetail.Value = r.Cells["CropYearId"].Value.ToString();
				CmbPackingTypeDetail.Value = r.Cells["PackingTypeId"].Value.ToString();
				CmbPackUomDetail.Value = r.Cells["PackUomId"].Value.ToString();
				txtQtyDetail.Text = Conversion.ToDouble(r.Cells["Qty"].Value).ToString("#,##0.###");
				txtWeightDetail.Text = Conversion.ToDouble(r.Cells["Weight"].Value).ToString("#,##0.###");
				txtRateDetail.Text = Conversion.ToDouble(r.Cells["Rate"].Value).ToString("#,##0.###");
				CmbRateUomDetail.Value = r.Cells["RateUomId"].Value.ToString();
				txtAmountDetail.Text = Conversion.ToDouble(r.Cells["Amount"].Value).ToString("#,##0.###");
				txtRemarksdetail.Text = r.Cells["Remarks"].Value.ToString();
				CmbBranchDetail.Value = r.Cells["ReceiverLocationId"].Value.ToString();
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				CmbItemNameDetail.Focus();
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
				int OrderDetailId = 0;
				int ItemId = Conversion.ToInt(CmbItemNameDetail.Value);
				string ItemName = Conversion.ToString(((UltraDropDownBase)CmbItemNameDetail).SelectedRow.Cells["ItemName"].Value);
				string ItemCode = Conversion.ToString(((UltraDropDownBase)CmbItemNameDetail).SelectedRow.Cells["ItemCode"].Value);
				if (Conversion.ToInt(CmbOrderNoDetail.Value) > 0 && ItemId > 0)
				{
					OrderDetailId = Conversion.ToInt(((UltraDropDownBase)CmbItemNameDetail).SelectedRow.Cells["OrderDetailId"].Value);
				}
				table.Rows[updateDetailIndex]["SupplierId"] = Conversion.ToInt(CmbSupplierDetail.Value);
				table.Rows[updateDetailIndex]["Supplier"] = Conversion.ToString((object)((Control)(object)CmbSupplierDetail).Text);
				table.Rows[updateDetailIndex]["OrderId"] = Conversion.ToInt(CmbOrderNoDetail.Value);
				table.Rows[updateDetailIndex]["OrderNo"] = Conversion.ToString((object)((Control)(object)CmbOrderNoDetail).Text);
				table.Rows[updateDetailIndex]["CommissionAgentId"] = Conversion.ToInt(CmbCommissionAgentDetail.Value);
				table.Rows[updateDetailIndex]["CommissionAgent"] = Conversion.ToString((object)((Control)(object)CmbCommissionAgentDetail).Text);
				table.Rows[updateDetailIndex]["ReferencePartyId"] = Conversion.ToInt(CmbRefPartyDetail.Value);
				table.Rows[updateDetailIndex]["ReferenceParty"] = Conversion.ToString((object)((Control)(object)CmbRefPartyDetail).Text);
				table.Rows[updateDetailIndex]["OrderDetailId"] = OrderDetailId;
				table.Rows[updateDetailIndex]["ItemId"] = ItemId;
				table.Rows[updateDetailIndex]["ItemCode"] = ItemCode;
				table.Rows[updateDetailIndex]["ItemName"] = ItemName;
				table.Rows[updateDetailIndex]["CropYearId"] = Conversion.ToInt(CmbCropYearDetail.Value);
				table.Rows[updateDetailIndex]["CropYear"] = Conversion.ToString((object)((Control)(object)CmbCropYearDetail).Text);
				table.Rows[updateDetailIndex]["PackingTypeId"] = Conversion.ToInt(CmbPackingTypeDetail.Value);
				table.Rows[updateDetailIndex]["PackingType"] = Conversion.ToString((object)((Control)(object)CmbPackingTypeDetail).Text);
				table.Rows[updateDetailIndex]["PackUomId"] = Conversion.ToInt(CmbPackUomDetail.Value);
				table.Rows[updateDetailIndex]["PackUom"] = Conversion.ToString((object)((Control)(object)CmbPackUomDetail).Text);
				table.Rows[updateDetailIndex]["PackUomEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)CmbPackUomDetail).SelectedRow.Cells[2].Value);
				table.Rows[updateDetailIndex]["Qty"] = Conversion.ToDouble((object)txtQtyDetail.Text);
				table.Rows[updateDetailIndex]["Weight"] = Conversion.ToDouble((object)txtWeightDetail.Text);
				table.Rows[updateDetailIndex]["RateUomId"] = Conversion.ToInt(CmbRateUomDetail.Value);
				table.Rows[updateDetailIndex]["RateUom"] = Conversion.ToString((object)((Control)(object)CmbRateUomDetail).Text);
				table.Rows[updateDetailIndex]["RateUomEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)CmbRateUomDetail).SelectedRow.Cells[2].Value);
				table.Rows[updateDetailIndex]["Rate"] = Conversion.ToDouble((object)txtRateDetail.Text);
				table.Rows[updateDetailIndex]["Amount"] = Conversion.ToDouble((object)txtAmountDetail.Text);
				table.Rows[updateDetailIndex]["Remarks"] = Conversion.ToString((object)txtRemarksdetail.Text);
				table.Rows[updateDetailIndex]["ReceiverLocationId"] = Conversion.ToInt(CmbBranchDetail.Value);
				table.Rows[updateDetailIndex]["ReceiverLocation"] = Conversion.ToString((object)((Control)(object)CmbBranchDetail).Text);
				grd.DataSource = table;
				txtExchangeRate_TextChanged(null, null);
				ResetDetail();
				CmbItemNameDetail.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		ResetDetail();
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grd.UpdateData();
			_ = grd.CurrentRow;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (Approved)
			{
				throw new Exception("Record Not Delete because Record has approved");
			}
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					SupplierDispatch.DeleteByID(RecId, UserAccount.ID);
					MessageBox.Show("Delete Record Successfully");
					Reset();
				}
			}
			else
			{
				MessageBox.Show("Record Not Found");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSaveAs_Click(object sender, EventArgs e)
	{
		try
		{
			Approved = false;
			RecId = 0;
			Insert();
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
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Expected O, but got Unknown
		try
		{
			if (grd.RowCount == 0)
			{
				MessageBox.Show("Grid Record Not Found");
			}
			else
			{
				if (!FormValidation())
				{
					return;
				}
				SupplierDispatch po = new SupplierDispatch();
				if (RecId > 0)
				{
					if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					po.Id = RecId;
				}
				else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				po.OrganizationId = UserAccount.OrganizationId;
				po.CompanyId = UserAccount.CompanyId;
				po.BranchesId = UserAccount.BranchesId;
				po.ProjectId = UserAccount.BranchesId;
				po.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				po.DocumentTypeId = 246;
				po.EntryUserId = UserAccount.ID;
				po.ModifyUserId = UserAccount.ID;
				po.ApprovedUserId = UserAccount.ID;
				po.ModifyDate = DateTime.Now;
				po.EntryDate = DateTime.Now;
				po.ApprovedDate = DateTime.Now;
				po.DocDate = txtDocDateMain.Value;
				po.DocNo = Conversion.ToInt((object)txtDocNoMain.Text);
				po.BranchSrNo = Conversion.ToInt((object)txtBranchSrNo.Text);
				po.VehicleNo = Conversion.ToString((object)txtVehicleNoMain.Text);
				po.BiltyNo = Conversion.ToString((object)txtBiltyNoMain.Text);
				po.DriverName = Conversion.ToString((object)txtDriverNameMain.Text);
				po.Driver1CellNo = Conversion.ToString((object)txtDriverCell1Main.Text);
				po.Driver2CellNo = Conversion.ToString((object)txtDriverCell2Main.Text);
				po.DepartureDate = Conversion.ToDateTime((object)txtDepartureDateMain.Value);
				po.TransitDays = Conversion.ToInt((object)txtTransitDaysMain.Text);
				po.ETAdestination = Conversion.ToDateTime((object)txtETAdestinationMain.Value);
				po.Freight = Conversion.ToDecimal((object)txtFreightMain.Text);
				po.OtherExpense = Conversion.ToDecimal((object)txtOtherExpenseMain.Text);
				po.CityId = Conversion.ToInt(CmbCityNameMain.Value);
				po.SupplierWbWeight = Conversion.ToDecimal((object)txtSupplierWbWtMain.Text);
				po.TotalQty = Conversion.ToDecimal((object)txtQtyMain.Text);
				po.TotalWeight = Conversion.ToDecimal((object)txtTotalWeightMain.Text);
				po.TotalAmount = Conversion.ToDecimal((object)txtTotalAmountMain.Text);
				po.CurrencyId = Conversion.ToInt(CmbCurrencyMain.Value);
				po.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRateMain.Text);
				po.FcyAmount = Conversion.ToDecimal((object)txtFcyAmountMain.Text);
				po.RemarksHeader = Conversion.ToString((object)txtRemarksMain.Text);
				po.VehicleImage = Conversion.ToString((object)VehicleImageFilePath);
				po.DriverPic = Conversion.ToString((object)DriverImageFilePath);
				po.SupplierDispatchDetailList = new List<SupplierDispatchDetail>();
				po.AttachmentsList = new List<DMSAttachments>();
				po.DeleteAttachmentsList = new List<DMSAttachments>();
				po.SupplierDispatchCommDetailList = new List<SupplierDispatchCommDetail>();
				po.SupplierDispatchExpenseList = new List<SupplierDispatchExpense>();
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					SupplierDispatchDetail vd = new SupplierDispatchDetail();
					if (RecId == 0)
					{
						vd.ActionTypeId = 1;
					}
					else
					{
						vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
						if (vd.Id > 0)
						{
							vd.ActionTypeId = 2;
						}
						else
						{
							vd.ActionTypeId = 1;
						}
					}
					if (Conversion.ToInt(r.Cells["SupplierId"].Value) == 0)
					{
						throw new Exception($"Supplier Field Required in Grid Row no : {r.RowIndex + 1}");
					}
					if (Conversion.ToInt(r.Cells["ItemId"].Value) == 0)
					{
						throw new Exception($"Item Field Required in Grid Row no : {r.RowIndex + 1}");
					}
					if (Conversion.ToInt(r.Cells["CropYearId"].Value) == 0)
					{
						throw new Exception($"CropYear Field Required in Grid Row no : {r.RowIndex + 1}");
					}
					if (Conversion.ToInt(r.Cells["PackingTypeId"].Value) == 0)
					{
						throw new Exception($"PackingType Field Required in Grid Row no : {r.RowIndex + 1}");
					}
					if (Conversion.ToInt(r.Cells["PackUomId"].Value) == 0)
					{
						throw new Exception($"PackUom Field Required in Grid Row no : {r.RowIndex + 1}");
					}
					if (Conversion.ToDouble(r.Cells["Qty"].Value) == 0.0)
					{
						throw new Exception($"Qty Field Required in Grid Row no : {r.RowIndex + 1}");
					}
					if (Conversion.ToDouble(r.Cells["Weight"].Value) == 0.0)
					{
						throw new Exception($"Weight Field Required in Grid Row no : {r.RowIndex + 1}");
					}
					if (Conversion.ToInt(r.Cells["RateUomId"].Value) == 0)
					{
						throw new Exception($"RateUom Field Required in Grid Row no : {r.RowIndex + 1}");
					}
					if (Conversion.ToDouble(r.Cells["Rate"].Value) == 0.0)
					{
						throw new Exception($"Rate Field Required in Grid Row no : {r.RowIndex + 1}");
					}
					if (Conversion.ToDouble(r.Cells["Amount"].Value) == 0.0)
					{
						throw new Exception($"Amount Field Required in Grid Row no : {r.RowIndex + 1}");
					}
					if (BranchFeature && !BranchImplemented && Conversion.ToInt(r.Cells["ReceiverLocationId"].Value) == 0)
					{
						throw new Exception($"ReceiverLocation Field Required in Grid Row no : {r.RowIndex + 1}");
					}
					vd.SupplierId = Conversion.ToInt(r.Cells["SupplierId"].Value);
					vd.OrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
					vd.CommissionAgentId = Conversion.ToInt(r.Cells["CommissionAgentId"].Value);
					vd.ReferencePartyId = Conversion.ToInt(r.Cells["ReferencePartyId"].Value);
					vd.OrderDetailId = Conversion.ToInt(r.Cells["OrderDetailId"].Value);
					vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					vd.CropYearId = Conversion.ToInt((object)r.Cells["CropYearId"].Text);
					vd.PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					vd.PackSizeId = Conversion.ToInt(r.Cells["PackUomId"].Value);
					vd.Qty = Conversion.ToDouble(r.Cells["Qty"].Value);
					vd.WeightKgs = Conversion.ToDouble(r.Cells["Weight"].Value);
					vd.RateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
					vd.Rate = Conversion.ToDouble(r.Cells["Rate"].Value);
					vd.Amount = Conversion.ToDouble(r.Cells["Amount"].Value);
					vd.CurrencyId = Conversion.ToInt(CmbCurrencyMain.Value);
					vd.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRateMain.Text);
					vd.FcyAmount = Conversion.ToDecimal(r.Cells["FcyAmount"].Value);
					vd.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
					vd.BranchId = Conversion.ToInt(r.Cells["ReceiverLocationId"].Value);
					po.SupplierDispatchDetailList.Add(vd);
				}
				if (po.SupplierDispatchDetailList.Count == 0)
				{
					throw new Exception("At least enter value/quantity in one of the rows");
				}
				if (RecId > 0 && lstRemoveRecord.Count > 0)
				{
					for (int i = 0; i < lstRemoveRecord.Count; i++)
					{
						po.SupplierDispatchDetailList.Add(lstRemoveRecord[i]);
					}
				}
				po.DeleteAttachmentsList = new List<DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<SupplierDispatch>(po, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				po.AttachmentsValues = AttachmentsValues;
				po.CustomAttachmentsValues = CustomAttachmentsValues;
				int success = (po.Id = SupplierDispatch.Save(po));
				if (RecId > 0)
				{
					MessageBox.Show("Data Update Successfully....  " + po.DocNo);
					FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 246, base.Name);
				}
				else
				{
					MessageBox.Show("Data Save Successfully....  " + po.DocNo);
				}
				if (VehicleImageofd != null && VehicleImageofd != string.Empty && File.Exists(VehicleImageofd))
				{
					File.Copy(VehicleImageofd, VehicleImageFilePath, overwrite: true);
					VehicleImageofd = string.Empty;
					VehicleImageFilePath = string.Empty;
					PicVehicleImage.Image = null;
					PicVehicleImage.Invalidate();
				}
				if (DriverImageofd != null && DriverImageofd != string.Empty && File.Exists(DriverImageofd))
				{
					File.Copy(DriverImageofd, DriverImageFilePath, overwrite: true);
					DriverImageofd = string.Empty;
					DriverImageFilePath = string.Empty;
					PicDriverImage.Image = null;
					PicDriverImage.Invalidate();
				}
				Reset();
				if (ChkBox.Checked)
				{
					CommonServices.SupplierDispatchSlip246(success);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			Approved = false;
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
			if (RecId == 0)
			{
				throw new Exception("Record Not Update because RecId Not Found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById(int ID)
	{
		try
		{
			RecId = Conversion.ToInt((object)ID);
			lstRemoveRecord.Clear();
			SupplierDispatch po = SupplierDispatch.GetByID(RecId);
			if (po == null || po.SupplierDispatchDetailList.Count <= 0)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			txtDocDateMain.Value = po.DocDate;
			txtDocNoMain.Text = Conversion.ToString((object)po.DocNo);
			txtBranchSrNo.Text = Conversion.ToString((object)po.BranchSrNo);
			txtVehicleNoMain.Text = Conversion.ToString((object)po.VehicleNo);
			txtBiltyNoMain.Text = Conversion.ToString((object)po.BiltyNo);
			txtDriverNameMain.Text = Conversion.ToString((object)po.DriverName);
			txtDriverCell1Main.Text = Conversion.ToString((object)po.Driver1CellNo);
			txtDriverCell2Main.Text = Conversion.ToString((object)po.Driver2CellNo);
			txtDepartureDateMain.Value = po.DepartureDate;
			txtTransitDaysMain.Text = Conversion.ToString((object)po.TransitDays);
			txtETAdestinationMain.Value = po.ETAdestination;
			txtFreightMain.Text = Conversion.ToString((object)po.Freight);
			txtOtherExpenseMain.Text = Conversion.ToString((object)po.OtherExpense);
			CmbCityNameMain.Value = Conversion.ToString((object)po.CityId);
			txtSupplierWbWtMain.Text = po.SupplierWbWeight.ToString("#,##0.##");
			txtQtyMain.Text = po.TotalQty.ToString("#,##0.##");
			txtTotalWeightMain.Text = po.TotalWeight.ToString("#,##0.##");
			txtTotalAmountMain.Text = po.TotalAmount.ToString(clsGlobalVariables.stringFormatsingle);
			CmbCurrencyMain.Value = Conversion.ToString((object)po.CurrencyId);
			txtExchangeRateMain.Text = po.ExchangeRate.ToString("#,##0.##");
			txtFcyAmountMain.Text = po.FcyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
			txtRemarksMain.Text = Conversion.ToString((object)po.RemarksHeader);
			Approved = Conversion.ToBool((object)po.IsApproved);
			if (po.CurrencyId == 0)
			{
				GetBaseCurrencyAndRate();
			}
			AttachmentsValues = po.AttachmentsValues;
			CustomAttachmentsValues = po.CustomAttachmentsValues;
			table.Clear();
			for (int i = 0; i < po.SupplierDispatchDetailList.Count; i++)
			{
				table.Rows.Add(po.SupplierDispatchDetailList[i].Id, po.SupplierDispatchDetailList[i].SupplierId, po.SupplierDispatchDetailList[i].SupplierName, po.SupplierDispatchDetailList[i].OrderId, po.SupplierDispatchDetailList[i].OrderNo, po.SupplierDispatchDetailList[i].CommissionAgentId, po.SupplierDispatchDetailList[i].CommissionAgentName, po.SupplierDispatchDetailList[i].ReferencePartyId, po.SupplierDispatchDetailList[i].ReferencePartyName, po.SupplierDispatchDetailList[i].OrderDetailId, po.SupplierDispatchDetailList[i].ItemId, po.SupplierDispatchDetailList[i].ItemCode, po.SupplierDispatchDetailList[i].ItemName, po.SupplierDispatchDetailList[i].CropYearId, po.SupplierDispatchDetailList[i].CropYear, po.SupplierDispatchDetailList[i].PackingTypeId, po.SupplierDispatchDetailList[i].PackingType, po.SupplierDispatchDetailList[i].PackSizeId, po.SupplierDispatchDetailList[i].PackUomCode, po.SupplierDispatchDetailList[i].PackUomEquivalent, po.SupplierDispatchDetailList[i].Qty, po.SupplierDispatchDetailList[i].WeightKgs, po.SupplierDispatchDetailList[i].RateUomId, po.SupplierDispatchDetailList[i].RateUomCode, po.SupplierDispatchDetailList[i].RateUomEquivalent, po.SupplierDispatchDetailList[i].Rate, po.SupplierDispatchDetailList[i].Amount, po.SupplierDispatchDetailList[i].FcyAmount, po.SupplierDispatchDetailList[i].Remarks, po.SupplierDispatchDetailList[i].BranchId, po.SupplierDispatchDetailList[i].BranchName);
			}
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			btnsave.Visible = false;
			btnupdate.Visible = true;
			btnDelete.Visible = true;
			VehicleImageFilePath = po.VehicleImage;
			if (VehicleImageFilePath != "")
			{
				if (File.Exists(VehicleImageFilePath))
				{
					PicVehicleImage.Image = Image.FromFile(po.VehicleImage);
				}
				else
				{
					VehicleImageFilePath = "";
					PicVehicleImage.Image = null;
					PicVehicleImage.Invalidate();
				}
			}
			else
			{
				VehicleImageFilePath = "";
				PicVehicleImage.Image = null;
				PicVehicleImage.Invalidate();
			}
			DriverImageFilePath = po.DriverPic;
			if (DriverImageFilePath != "")
			{
				if (File.Exists(DriverImageFilePath))
				{
					PicDriverImage.Image = Image.FromFile(po.DriverPic);
				}
				else
				{
					DriverImageFilePath = "";
					PicDriverImage.Image = null;
					PicDriverImage.Invalidate();
				}
			}
			else
			{
				DriverImageFilePath = "";
				PicDriverImage.Image = null;
				PicDriverImage.Invalidate();
			}
			txtExchangeRate_TextChanged(null, null);
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
			CropYear();
			GetBaseCurrencyAndRate();
			cmbCityFill();
			CurrencyFill();
			SupplierNameFill();
			ReferencePartyFill();
			if (Conversion.ToInt(CmbOrderNoDetail.Value) == 0)
			{
				ItemWithoutOrderFill();
			}
			CropYear();
			CmbPackingTypeFill();
			BranchDetailFill();
			ConfigurationDefault();
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
			lstRemoveRecord.Clear();
			Approved = false;
			RecId = 0;
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			txtVehicleNoMain.Text = string.Empty;
			txtBiltyNoMain.Text = string.Empty;
			txtDriverCell1Main.Text = string.Empty;
			txtDriverCell2Main.Text = string.Empty;
			txtRemarksMain.Text = string.Empty;
			txtTransitDaysMain.Text = string.Empty;
			txtFreightMain.Text = string.Empty;
			txtOtherExpenseMain.Text = string.Empty;
			txtQtyMain.Text = string.Empty;
			txtTotalWeightMain.Text = string.Empty;
			txtTotalAmountMain.Text = string.Empty;
			txtFcyAmountMain.Text = string.Empty;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			btnDelete.Visible = false;
			btnSaveAs.Visible = false;
			txtDocDateMain.Focus();
			updateDetailIndex = -1;
			((Control)(object)CmbSupplierDetail).Text = string.Empty;
			((Control)(object)CmbCommissionAgentDetail).Text = string.Empty;
			((Control)(object)CmbRefPartyDetail).Text = string.Empty;
			((Control)(object)CmbItemNameDetail).Text = string.Empty;
			((Control)(object)CmbPackUomDetail).Text = string.Empty;
			txtQtyDetail.Text = string.Empty;
			txtWeightDetail.Text = string.Empty;
			((Control)(object)CmbRateUomDetail).Text = string.Empty;
			txtRateDetail.Text = string.Empty;
			txtAmountDetail.Text = string.Empty;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			table.Clear();
			grd.DataSource = table;
			VehicleImageofd = string.Empty;
			VehicleImageFilePath = string.Empty;
			PicVehicleImage.Image = null;
			DriverImageofd = string.Empty;
			DriverImageFilePath = string.Empty;
			PicDriverImage.Image = null;
			txtDocNoMain.Text = DocumentNoFill();
			txtBranchSrNo.Text = BranchDocumentNoFill();
			ConfigurationDefault();
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
			((Control)(object)CmbItemNameDetail).Text = string.Empty;
			((Control)(object)CmbPackUomDetail).Text = string.Empty;
			txtQtyDetail.Text = string.Empty;
			txtWeightDetail.Text = string.Empty;
			((Control)(object)CmbRateUomDetail).Text = string.Empty;
			txtRateDetail.Text = string.Empty;
			txtAmountDetail.Text = string.Empty;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierDispatch_FormClosing(object sender, FormClosingEventArgs e)
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

	private void CurrencyFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int ID = Conversion.ToInt(CmbCurrencyMain.Value);
			DataTable dt = MultiCurrency.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbCurrencyMain, "Id", "CurrencyCode", "Currency", true);
				((UltraGridBase)CmbCurrencyMain).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbCurrencyMain.Value = ID;
					}
					else
					{
						((Control)(object)CmbCurrencyMain).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbCurrencyMain).Text = string.Empty;
				((UltraGridBase)CmbCurrencyMain).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GetBaseCurrencyAndRate()
	{
		try
		{
			int CurrId = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("Base Currency"));
			double Rate = Conversion.ToDouble((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("BaseCurrencyRate"));
			if (Conversion.ToInt(CmbCurrencyMain.Value) == 0)
			{
				CmbCurrencyMain.Value = CurrId;
			}
			if (Conversion.ToDouble((object)txtExchangeRateMain.Text) == 0.0)
			{
				txtExchangeRateMain.Text = Conversion.ToString((object)Rate);
			}
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CalculateTotalInformation()
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				decimal TotalQty = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["QTY"], (AggregateFunction)2));
				decimal TotalWeight = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["Weight"], (AggregateFunction)2));
				decimal TotalAmount = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["Amount"], (AggregateFunction)2));
				decimal TotalFcyAmount = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["FcyAmount"], (AggregateFunction)2));
				txtQtyMain.Text = Math.Round(TotalQty, 2).ToString("#,##0.##");
				txtTotalWeightMain.Text = Math.Round(TotalWeight, 2).ToString("#,##0.##");
				txtTotalAmountMain.Text = TotalAmount.ToString(clsGlobalVariables.stringFormatsingle);
				txtFcyAmountMain.Text = TotalFcyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
			}
			else
			{
				txtQtyMain.Text = "0";
				txtTotalWeightMain.Text = "0";
				txtTotalAmountMain.Text = "0";
				txtFcyAmountMain.Text = "0";
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtExchangeRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() > 0 && Conversion.ToDecimal((object)txtExchangeRateMain.Text) > 0m)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					decimal amount = Conversion.ToDecimal(obj.Cells["Amount"].Value) / Conversion.ToDecimal((object)txtExchangeRateMain.Text);
					obj.Cells["FcyAmount"].Value = Math.Round(amount, clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
					obj.EndEdit();
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["FcyAmount"].Value = 0;
					obj2.EndEdit();
				}
			}
			CalculateTotalInformation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtExchangeRate_Leave(object sender, EventArgs e)
	{
		try
		{
			txtExchangeRateMain.Text = Conversion.ToDecimal((object)txtExchangeRateMain.Text).ToString(clsGlobalVariables.DecimalRateFormate);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CalculateWeight()
	{
		try
		{
			if (((UltraGridBase)CmbPackUomDetail).ActiveRow != null && Conversion.ToDouble((object)txtQtyDetail.Text) > 0.0)
			{
				double ItemPack = Conversion.ToDouble(((UltraDropDownBase)CmbPackUomDetail).SelectedRow.Cells[2].Value);
				double Qty = Conversion.ToDouble((object)txtQtyDetail.Text);
				txtWeightDetail.Text = (ItemPack * Qty).ToString("#,##0.####");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalAmount()
	{
		try
		{
			double RateUOM = 0.0;
			double Rate = 0.0;
			double TotalAmount = 0.0;
			if (Conversion.ToDouble((object)txtWeightDetail.Text) > 0.0 && ((UltraGridBase)CmbRateUomDetail).ActiveRow != null && Conversion.ToDouble((object)txtRateDetail.Text) > 0.0)
			{
				txtAmountDetail.Text = string.Empty;
				double num = Conversion.ToDouble((object)txtWeightDetail.Text);
				RateUOM = Conversion.ToDouble(((UltraDropDownBase)CmbRateUomDetail).SelectedRow.Cells[2].Value);
				Rate = Conversion.ToDouble((object)txtRateDetail.Text);
				TotalAmount = num / RateUOM * Rate;
				txtAmountDetail.Text = Math.Round(TotalAmount, 0, MidpointRounding.AwayFromZero).ToString("#,##0.####");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combitempck_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			TotalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtqty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			TotalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtweight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			TotalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combrateuom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combrateuom_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbBranchName_Leave(object sender, EventArgs e)
	{
		try
		{
			_ = ((UltraGridBase)cmbBranchName).ActiveRow;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryBranchComboFill()
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
				DataTable dt = SupplierDispatch.GetBranchsAllocatedToUserFromSupplierDispatch(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, 0);
				if (dt.Rows.Count > 0)
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						dtBranch.Rows.Add(dt.Rows[i]["BranchId"], dt.Rows[i]["BranchName"]);
					}
				}
			}
			if (dtBranch.Rows.Count > 0)
			{
				DDL.BindDDL(dtBranch, cmbBranchName, "Id", "BranchName", "Branch Name", ZeroIndex: true);
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
				txtDocDateMain.Focus();
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
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			FromDocNoHistory.Clear();
			ToDocNoHistory.Clear();
			grdhistory.DataSource = null;
			DataGridHistoryDetail.DataSource = null;
			drdocdate.Checked = true;
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
			HistoryBranchComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			gridhistoryfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void gridhistoryfill()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtHistory.Rows.Clear();
			ReportsParameters reports = new ReportsParameters();
			reports.OrganizationId = UserAccount.OrganizationId;
			reports.CompanyId = UserAccount.CompanyId;
			reports.DocumentTypeId = 246;
			reports.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			reports.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!reports.CanViewAllRecord)
			{
				reports.EntryUser = UserAccount.ID;
			}
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					reports.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					reports.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					reports.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					reports.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					reports.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					reports.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					reports.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					reports.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			reports.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			reports.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
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
				reports.BranchesIds = BranchIds;
				DataTable dtGridRegister = SupplierDispatch.FormHistory(reports);
				if (dtGridRegister.Rows.Count > 0)
				{
					DataTable dtcol = new DataTable();
					dtcol.Columns.Add("Id", typeof(int));
					dtcol.Columns.Add("DocumentTypeId", typeof(int));
					dtcol.Columns.Add("DocNo", typeof(int));
					dtcol.Columns.Add("BranchSrNo", typeof(int));
					dtcol.Columns.Add("DocDate", typeof(DateTime));
					dtcol.Columns.Add("VehicleNo", typeof(string));
					dtcol.Columns.Add("BiltyNo", typeof(string));
					dtcol.Columns.Add("DriverName", typeof(string));
					dtcol.Columns.Add("Driver1CellNo", typeof(string));
					dtcol.Columns.Add("Driver2CellNo", typeof(string));
					dtcol.Columns.Add("DepartureDate", typeof(DateTime));
					dtcol.Columns.Add("TransitDays", typeof(int));
					dtcol.Columns.Add("ETADestination", typeof(DateTime));
					dtcol.Columns.Add("SupplierWbWeight", typeof(decimal));
					dtcol.Columns.Add("TotalQty", typeof(decimal));
					dtcol.Columns.Add("TotalWeight", typeof(decimal));
					dtcol.Columns.Add("TotalAmount", typeof(decimal));
					dtcol.Columns.Add("FcyAmount", typeof(decimal));
					dtcol.Columns.Add("Freight", typeof(decimal));
					dtcol.Columns.Add("OtherExpense", typeof(decimal));
					dtcol.Columns.Add("CityName", typeof(string));
					dtcol.Columns.Add("RemarksHeader", typeof(string));
					dtcol.Columns.Add("EntryDate", typeof(DateTime));
					dtcol.Columns.Add("EntryUser", typeof(string));
					dtcol.Columns.Add("ModifyDate", typeof(DateTime));
					dtcol.Columns.Add("ModifyUser", typeof(string));
					dtcol.Columns.Add("IsApproved", typeof(string));
					dtcol.Columns.Add("ApprovedDate", typeof(DateTime));
					dtcol.Columns.Add("ApprovedUser", typeof(string));
					dtcol.Columns.Add("NoOfAttachments", typeof(int));
					dtcol.Rows.Clear();
					foreach (DataRow i in dtGridRegister.Rows)
					{
						dtcol.Rows.Add(i["Id"], i["DocumentTypeId"], i["DocNo"], i["BranchSrNo"], i["DocDate"], i["VehicleNo"], i["BiltyNo"], i["DriverName"], i["Driver1CellNo"], i["Driver2CellNo"], i["DepartureDate"], i["TransitDays"], i["ETAdestination"], i["SupplierWbWeight"], i["TotalQty"], i["TotalWeight"], i["TotalAmount"], i["FcyAmount"], i["Freight"], i["OtherExpense"], i["CityName"], i["RemarksHeader"], i["EntryDate"], i["EntryUser"], i["ModifyDate"], i["ModifyUser"], Conversion.ToBool(i["IsApproved"]) ? "Approved" : "Not Approved", i["ApprovedDate"], i["ApprovedUser"], i["NoOfAttachments"]);
					}
					grdhistory.DataSource = dtcol;
					grdhistory.RetrieveStructure();
					HistoryGridSettings();
				}
				else
				{
					grdhistory.ClearStructure();
					DataGridHistoryDetail.ClearStructure();
				}
				return;
			}
			cmbBranchName.Focus();
			throw new Exception("Select branch first");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void HistoryGridSettings()
	{
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Expected O, but got Unknown
		try
		{
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdhistory.RootTable.Columns["FcyAmount"].Visible = HasMultiCurrencyFeature;
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdhistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdhistory.RootTable.Columns["BranchSrNo"].Visible = BranchFeature && !BranchImplemented;
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdhistory.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.HeaderAlignment = (TextAlignment)2;
				if (column.Type == typeof(double) || column.Type == typeof(decimal))
				{
					ConfigureNumericalColumn(column);
				}
			}
			grdhistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdhistory.RootTable.Columns["BranchSrNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdhistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdhistory.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grdhistory.RootTable.Columns["BiltyNo"].Width = Constants.InventoryConstants.BiltyNoConstant;
			grdhistory.RootTable.Columns["DriverName"].Width = Constants.InventoryConstants.DriverName;
			grdhistory.RootTable.Columns["Driver1CellNo"].Width = Constants.InventoryConstants.CellNo;
			grdhistory.RootTable.Columns["Driver2CellNo"].Width = Constants.InventoryConstants.CellNo;
			grdhistory.RootTable.Columns["DepartureDate"].Width = Constants.InventoryConstants.DateConstant;
			grdhistory.RootTable.Columns["TransitDays"].Width = Constants.InventoryConstants.DueDays;
			grdhistory.RootTable.Columns["ETADestination"].Width = Constants.InventoryConstants.DateConstant;
			grdhistory.RootTable.Columns["TotalQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdhistory.RootTable.Columns["SupplierWbWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdhistory.RootTable.Columns["TotalAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdhistory.RootTable.Columns["FcyAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdhistory.RootTable.Columns["Freight"].Width = Constants.InventoryConstants.FreightConstant;
			grdhistory.RootTable.Columns["OtherExpense"].Width = Constants.InventoryConstants.FreightConstant;
			grdhistory.RootTable.Columns["CityName"].Width = Constants.InventoryConstants.CityName;
			grdhistory.RootTable.Columns["RemarksHeader"].Width = Constants.InventoryConstants.RemarksConstants;
			grdhistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateConstant;
			grdhistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdhistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateConstant;
			grdhistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants;
			grdhistory.RootTable.Columns["IsApproved"].Width = Constants.InventoryConstants.IsApproved;
			grdhistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateConstant;
			grdhistory.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.ModifyUserConstants;
			grdhistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			grdhistory.RootTable.Columns.Add("Edit");
			grdhistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdhistory.RootTable.Columns["Edit"].Width = 40;
			grdhistory.RootTable.Columns.Add("Print");
			grdhistory.RootTable.Columns["Print"].Caption = "Print";
			grdhistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdhistory.RootTable.Columns["Print"].Width = 40;
			grdhistory.RootTable.Columns["Edit"].Position = 0;
			grdhistory.RootTable.Columns["Print"].Position = 1;
			grdhistory.FrozenColumns = 2;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ConfigureNumericalColumn(GridEXColumn column)
	{
		if (column.Key.Contains("Amount"))
		{
			column.AggregateFunction = (AggregateFunction)2;
			column.TextAlignment = (TextAlignment)3;
			column.FormatString = clsGlobalVariables.stringFormatsingle;
			column.TotalFormatString = clsGlobalVariables.stringFormatsingle;
		}
		else if (column.Key.Contains("Rate"))
		{
			column.TextAlignment = (TextAlignment)3;
			column.FormatString = clsGlobalVariables.DecimalRateFormate;
		}
		else
		{
			column.AggregateFunction = (AggregateFunction)2;
			column.TextAlignment = (TextAlignment)3;
			column.FormatString = "#,##0.##";
			column.TotalFormatString = "#,##0.##";
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
					Reset();
					RecId = CurrId;
					ReadById(RecId);
				}
				if (e.Column.Key == "Print")
				{
					CommonServices.SupplierDispatchSlip246(CurrId);
				}
				if (e.Column.Key == "SaveAs")
				{
					ReadbyIdForSaveAsRecord(CurrId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadbyIdForSaveAsRecord(int CurrId)
	{
	}

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdhistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 246);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
				RecId = Conversion.ToInt(r.Cells["Id"].Value);
				ReadById(RecId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				GridDetailBind();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailBind()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			SupplierDispatch po = SupplierDispatch.GetByID(Conversion.ToInt(r.Cells["Id"].Value));
			if (po != null)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("SupplierId", typeof(int));
				dt.Columns.Add("Supplier");
				dt.Columns.Add("OrderId", typeof(int));
				dt.Columns.Add("OrderNo");
				dt.Columns.Add("CommissionAgent");
				dt.Columns.Add("ReferenceParty");
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("ItemCode", typeof(string));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("CropYear");
				dt.Columns.Add("PackingType");
				dt.Columns.Add("PackUom");
				dt.Columns.Add("Qty", typeof(double));
				dt.Columns.Add("Weight", typeof(double));
				dt.Columns.Add("RateUom");
				dt.Columns.Add("Rate", typeof(double));
				dt.Columns.Add("Amount", typeof(double));
				dt.Columns.Add("FcyAmount", typeof(double));
				dt.Columns.Add("Remarks");
				dt.Columns.Add("ReceiverLocation");
				for (int i = 0; i < po.SupplierDispatchDetailList.Count; i++)
				{
					dt.Rows.Add(po.SupplierDispatchDetailList[i].Id, po.SupplierDispatchDetailList[i].SupplierId, po.SupplierDispatchDetailList[i].SupplierName, po.SupplierDispatchDetailList[i].OrderId, po.SupplierDispatchDetailList[i].OrderNo, po.SupplierDispatchDetailList[i].CommissionAgentName, po.SupplierDispatchDetailList[i].ReferencePartyName, po.SupplierDispatchDetailList[i].ItemId, po.SupplierDispatchDetailList[i].ItemCode, po.SupplierDispatchDetailList[i].ItemName, po.SupplierDispatchDetailList[i].CropYear, po.SupplierDispatchDetailList[i].PackingType, po.SupplierDispatchDetailList[i].PackUomCode, po.SupplierDispatchDetailList[i].Qty, po.SupplierDispatchDetailList[i].WeightKgs, po.SupplierDispatchDetailList[i].RateUomCode, po.SupplierDispatchDetailList[i].Rate, po.SupplierDispatchDetailList[i].Amount, po.SupplierDispatchDetailList[i].FcyAmount, po.SupplierDispatchDetailList[i].Remarks, po.SupplierDispatchDetailList[i].BranchName);
				}
				DataGridHistoryDetail.DataSource = dt;
				DataGridHistoryDetail.RetrieveStructure();
				DataGridHistoryDetailSetting();
			}
			else
			{
				DataGridHistoryDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistoryDetailSetting()
	{
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		try
		{
			DataGridHistoryDetail.RootTable.Columns["Id"].Visible = false;
			DataGridHistoryDetail.RootTable.Columns["SupplierId"].Visible = false;
			DataGridHistoryDetail.RootTable.Columns["OrderId"].Visible = false;
			DataGridHistoryDetail.RootTable.Columns["ItemId"].Visible = false;
			DataGridHistoryDetail.RootTable.Columns["FcyAmount"].Visible = HasMultiCurrencyFeature;
			DataGridHistoryDetail.RootTable.Columns["ReceiverLocation"].Visible = BranchFeature && !BranchImplemented;
			DataGridHistoryDetail.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)DataGridHistoryDetail.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.HeaderAlignment = (TextAlignment)2;
				if (column.Type == typeof(double) || column.Type == typeof(decimal))
				{
					ConfigureNumericalColumn(column);
				}
			}
			DataGridHistoryDetail.RootTable.Columns["Supplier"].Width = Constants.InventoryConstants.SupplierName;
			DataGridHistoryDetail.RootTable.Columns["OrderNo"].Width = Constants.InventoryConstants.OrderNo;
			DataGridHistoryDetail.RootTable.Columns["CommissionAgent"].Width = Constants.InventoryConstants.CommissionAgent;
			DataGridHistoryDetail.RootTable.Columns["ReferenceParty"].Width = Constants.InventoryConstants.ReferenceParty;
			DataGridHistoryDetail.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			DataGridHistoryDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			DataGridHistoryDetail.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			DataGridHistoryDetail.RootTable.Columns["PackingType"].Width = Constants.InventoryConstants.PackingType;
			DataGridHistoryDetail.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			DataGridHistoryDetail.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			DataGridHistoryDetail.RootTable.Columns["Weight"].Width = Constants.InventoryConstants.WeightConstant;
			DataGridHistoryDetail.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			DataGridHistoryDetail.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			DataGridHistoryDetail.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistoryDetail.RootTable.Columns["FcyAmount"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistoryDetail.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			DataGridHistoryDetail.RootTable.Columns["ReceiverLocation"].Width = Constants.InventoryConstants.BranchName;
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
			if (!FormInitialized)
			{
				return;
			}
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
			if (fs == null)
			{
				return;
			}
			string fileName = fs.Name;
			grd.LoadLayoutFile((Stream)fs);
			fs.Close();
			fs.Dispose();
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(fileName);
			int count = ((JanusCollectionBase)grd.RootTable.Columns).Count;
			int TableColumnsCount = table.Columns.Count + 1;
			if (count == TableColumnsCount)
			{
				return;
			}
			List<string> missingColumns = CommonServices.GetMissingColumns(table, xmlDoc);
			foreach (string column in missingColumns)
			{
				string d = CommonServices.GetDataTypeString(table.Columns[column].DataType);
				CommonServices.AddNewColumn(xmlDoc, fileName, column, d);
			}
			if (missingColumns.Count > 0)
			{
				FileStream fsNew = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
				grd.LoadLayoutFile((Stream)fsNew);
				fsNew.Close();
				fsNew.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			if (!FormInitialized)
			{
				return;
			}
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdhistory).Name);
			if (fs == null)
			{
				return;
			}
			string fileName = fs.Name;
			grdhistory.LoadLayoutFile((Stream)fs);
			fs.Close();
			fs.Dispose();
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(fileName);
			int count = ((JanusCollectionBase)grdhistory.RootTable.Columns).Count;
			int TableColumnsCount = dtHistory.Columns.Count + 3;
			if (count == TableColumnsCount)
			{
				return;
			}
			List<string> missingColumns = CommonServices.GetMissingColumns(dtHistory, xmlDoc);
			foreach (string column in missingColumns)
			{
				string d = CommonServices.GetDataTypeString(dtHistory.Columns[column].DataType);
				CommonServices.AddNewColumn(xmlDoc, fileName, column, d);
			}
			if (missingColumns.Count > 0)
			{
				FileStream fsNew = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdhistory).Name);
				grdhistory.LoadLayoutFile((Stream)fsNew);
				fsNew.Close();
				fsNew.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnattachment_Click(object sender, EventArgs e)
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

	private void btnprint_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.SupplierDispatchSlip246(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtqty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtExchangeRate_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	private void txtTransitDaysMain_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFreightMain_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtOtherExpenseMain_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtWeightDetail_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnRefPartyForm_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineReferenceParties(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierDispatch_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					txtDocDateMain.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					FromDateHistory.Focus();
				}
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnupdate.Visible && btnupdate.Enabled)
				{
					btnupdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F1)
				{
					BtnRefPartyForm_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					txtDocDateMain.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					btnattachment_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F12 && btnSaveAs.Visible && btnSaveAs.Enabled)
				{
					btnSaveAs_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					CmbItemNameDetail.Focus();
				}
				if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
				{
					btnprint_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1))
				{
					if (formright.DoHavePrintRights)
					{
						btnprint_Click(null, null);
					}
				}
				else if (e.Control && e.Shift && e.KeyCode == Keys.Delete && btnDelete.Visible && btnDelete.Enabled)
				{
					btnDelete_Click(null, null);
				}
			}
			else
			{
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnShow_Click(null, null);
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
			dt.Rows.Add("Ctrl+S", "For Save in Form Tab and For Show History in History Tab");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print 273");
			dt.Rows.Add("Alt+1", "For Print 273");
			dt.Rows.Add("Ctrl+F1", "For Open Reference Party Form");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid when focus in form tab and for focus on history grid when in history tab");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus on Item in Detail Grid when in Form tab and For focus on FromDate in history tab");
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
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grd.CurrentColumn.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteDetailRow(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdhistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int CuurID = Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Return)
			{
				Reset();
				ReadById(CuurID);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdhistory.CurrentColumn.Key == "Edit")
				{
					Reset();
					ReadById(CuurID);
				}
				else if (grdhistory.CurrentColumn.Key == "Print")
				{
					CommonServices.SupplierDispatchSlip246(CuurID);
				}
				else if (grdhistory.CurrentColumn.Key == "SaveAs")
				{
					ReadbyIdForSaveAsRecord(CuurID);
				}
				else if (grdhistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CuurID, 246);
				}
			}
			if (e.Control && e.KeyCode == Keys.F12)
			{
				ReadbyIdForSaveAsRecord(CuurID);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void txtSupplierWbWtMain_KeyPress(object sender, KeyPressEventArgs e)
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

	private void BtnBrowseVehicleImage_Click(object sender, EventArgs e)
	{
		try
		{
			definition();
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "(*.jpg; *.jpeg; *.gif; *.PNG; *.png)| *.jpg; *.jpeg; *.gif; *.PNG;*.png; ";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				VehicleImageofd = openFileDialog.FileName;
				string fileName = Path.GetFileNameWithoutExtension(VehicleImageofd);
				string exten = Path.GetExtension(VehicleImageofd);
				string date = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
				fileName = fileName + date + exten;
				VehicleImageFilePath = Path.Combine(saveDirectory, fileName);
				PicVehicleImage.Image = new Bitmap(openFileDialog.FileName);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnBrowseDriverImage_Click(object sender, EventArgs e)
	{
		try
		{
			definition();
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "(*.jpg; *.jpeg; *.gif; *.PNG; *.png)| *.jpg; *.jpeg; *.gif; *.PNG;*.png; ";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				DriverImageofd = openFileDialog.FileName;
				string fileName = Path.GetFileNameWithoutExtension(DriverImageofd);
				string exten = Path.GetExtension(DriverImageofd);
				string date = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
				fileName = fileName + date + exten;
				DriverImageFilePath = Path.Combine(saveDirectory, fileName);
				PicDriverImage.Image = new Bitmap(openFileDialog.FileName);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void definition()
	{
		string value = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Attachment Folder Path").ToList()[0].ConfigKey.ToString();
		if (value != null)
		{
			saveDirectory = value;
			flagImage = true;
		}
		else
		{
			MessageBox.Show("Please Create the Path in Configration");
		}
	}

	private void txtDocDateMain_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			txtDepartureDateMain.Value = txtDocDateMain.Value;
			ETACalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ETACalculations()
	{
		try
		{
			int Days = Conversion.ToInt((object)txtTransitDaysMain.Text);
			DateTime DepartureDate = Conversion.ToDateTime((object)txtDepartureDateMain.Value);
			txtETAdestinationMain.Value = DepartureDate.AddDays(Days);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtTransitDaysMain_TextChanged(object sender, EventArgs e)
	{
		try
		{
			txtDepartureDateMain.Value = txtDocDateMain.Value;
			ETACalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtETAdestinationMain_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			DateTime DepartureDate = Conversion.ToDateTime((object)txtDepartureDateMain.Value);
			int differenceInDays = Math.Abs((Conversion.ToDateTime((object)txtETAdestinationMain.Value) - DepartureDate).Days);
			txtTransitDaysMain.Text = differenceInDays.ToString();
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
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Expected O, but got Unknown
		//IL_0559: Unknown result type (might be due to invalid IL or missing references)
		//IL_0563: Expected O, but got Unknown
		//IL_056f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0579: Expected O, but got Unknown
		//IL_057a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0584: Expected O, but got Unknown
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Expected O, but got Unknown
		//IL_05b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bb: Expected O, but got Unknown
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Expected O, but got Unknown
		//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fd: Expected O, but got Unknown
		//IL_05fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0608: Expected O, but got Unknown
		//IL_0614: Unknown result type (might be due to invalid IL or missing references)
		//IL_061e: Expected O, but got Unknown
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0629: Expected O, but got Unknown
		//IL_07c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cb: Expected O, but got Unknown
		//IL_085b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0865: Expected O, but got Unknown
		//IL_0a29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a33: Expected O, but got Unknown
		//IL_0a4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a54: Expected O, but got Unknown
		//IL_0aa2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aac: Expected O, but got Unknown
		//IL_1204: Unknown result type (might be due to invalid IL or missing references)
		//IL_120e: Expected O, but got Unknown
		//IL_121b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1225: Expected O, but got Unknown
		//IL_8d3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_8d48: Expected O, but got Unknown
		//IL_8d6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_8d76: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance34 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
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
		Appearance appearance80 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistoryDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance153 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Purchase.frmSupplierDispatch));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.label63 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.label59 = new System.Windows.Forms.Label();
		this.label58 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.CmbOrderNoDetail = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbBranchDetail = new UltraCombo();
		this.CmbRefPartyDetail = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.CmbCommissionAgentDetail = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.CmbSupplierDetail = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.rdSearchByCode = new System.Windows.Forms.RadioButton();
		this.rdSearchByName = new System.Windows.Forms.RadioButton();
		this.CmbPackingTypeDetail = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbRateUomDetail = new UltraCombo();
		this.CmbCropYearDetail = new UltraCombo();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.CmbPackUomDetail = new UltraCombo();
		this.CmbItemNameDetail = new UltraCombo();
		this.btnplus = new System.Windows.Forms.Button();
		this.txtRemarksdetail = new System.Windows.Forms.TextBox();
		this.label47 = new System.Windows.Forms.Label();
		this.txtRateDetail = new System.Windows.Forms.TextBox();
		this.txtAmountDetail = new System.Windows.Forms.TextBox();
		this.txtWeightDetail = new System.Windows.Forms.TextBox();
		this.txtQtyDetail = new System.Windows.Forms.TextBox();
		this.label40 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label22 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label66 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label65 = new System.Windows.Forms.Label();
		this.label64 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.txtVehicleNoMain = new System.Windows.Forms.TextBox();
		this.CmbCityNameMain = new UltraCombo();
		this.txtDriverCell2Main = new System.Windows.Forms.TextBox();
		this.txtTotalAmountMain = new System.Windows.Forms.TextBox();
		this.txtDriverCell1Main = new System.Windows.Forms.TextBox();
		this.txtDriverNameMain = new System.Windows.Forms.TextBox();
		this.txtOtherExpenseMain = new System.Windows.Forms.TextBox();
		this.txtETAdestinationMain = new System.Windows.Forms.DateTimePicker();
		this.txtFreightMain = new System.Windows.Forms.TextBox();
		this.txtExchangeRateMain = new System.Windows.Forms.TextBox();
		this.txtSupplierWbWtMain = new System.Windows.Forms.TextBox();
		this.txtTotalWeightMain = new System.Windows.Forms.TextBox();
		this.txtQtyMain = new System.Windows.Forms.TextBox();
		this.txtFcyAmountMain = new System.Windows.Forms.TextBox();
		this.txtTransitDaysMain = new System.Windows.Forms.TextBox();
		this.CmbCurrencyMain = new UltraCombo();
		this.BtnBrowseDriverImage = new System.Windows.Forms.Button();
		this.BtnBrowseVehicleImage = new System.Windows.Forms.Button();
		this.label28 = new System.Windows.Forms.Label();
		this.PicDriverImage = new System.Windows.Forms.PictureBox();
		this.PicVehicleImage = new System.Windows.Forms.PictureBox();
		this.txtDepartureDateMain = new System.Windows.Forms.DateTimePicker();
		this.txtDocDateMain = new System.Windows.Forms.DateTimePicker();
		this.txtRemarksMain = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.txtBranchSrNo = new System.Windows.Forms.TextBox();
		this.txtDocNoMain = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.txtBiltyNoMain = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.BtnRefPartyForm = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.grdhistory = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label49 = new System.Windows.Forms.Label();
		this.DataGridHistoryDetail = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label55 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.cmbBranchName = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnShow = new System.Windows.Forms.Button();
		this.label51 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel10.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderNoDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBranchDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRefPartyDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionAgentDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingTypeDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUomDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYearDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUomDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameDetail).BeginInit();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCityNameMain).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCurrencyMain).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.PicDriverImage).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.PicVehicleImage).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistoryDetail).BeginInit();
		this.panel9.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel14.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1144, 684);
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
		this.tabControl1.Size = new System.Drawing.Size(1144, 684);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.tabControl2);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(1136, 655);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(2, 372);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1132, 281);
		this.tabControl2.TabIndex = 4;
		this.tabControl2.SelectedIndexChanged += new System.EventHandler(tabControl2_SelectedIndexChanged);
		this.tabPage3.Controls.Add(this.panel6);
		this.tabPage3.Controls.Add(this.panel10);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1124, 252);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Detail";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 28);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1118, 221);
		this.panel6.TabIndex = 3;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1118, 221);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.ctrlGrdBar1);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 3);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1118, 25);
		this.panel10.TabIndex = 2;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1077, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.groupBox3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(2, 252);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1132, 120);
		this.panel5.TabIndex = 3;
		this.groupBox3.BackColor = System.Drawing.Color.White;
		this.groupBox3.Controls.Add(this.label63);
		this.groupBox3.Controls.Add(this.label62);
		this.groupBox3.Controls.Add(this.label61);
		this.groupBox3.Controls.Add(this.label60);
		this.groupBox3.Controls.Add(this.label59);
		this.groupBox3.Controls.Add(this.label58);
		this.groupBox3.Controls.Add(this.label57);
		this.groupBox3.Controls.Add(this.label56);
		this.groupBox3.Controls.Add(this.label50);
		this.groupBox3.Controls.Add(this.label46);
		this.groupBox3.Controls.Add(this.label45);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOrderNoDetail);
		this.groupBox3.Controls.Add(this.label4);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranchDetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRefPartyDetail);
		this.groupBox3.Controls.Add(this.label8);
		this.groupBox3.Controls.Add(this.label18);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommissionAgentDetail);
		this.groupBox3.Controls.Add(this.label13);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplierDetail);
		this.groupBox3.Controls.Add(this.label10);
		this.groupBox3.Controls.Add(this.rdSearchByCode);
		this.groupBox3.Controls.Add(this.rdSearchByName);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackingTypeDetail);
		this.groupBox3.Controls.Add(this.label1);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUomDetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYearDetail);
		this.groupBox3.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUomDetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemNameDetail);
		this.groupBox3.Controls.Add(this.btnplus);
		this.groupBox3.Controls.Add(this.txtRemarksdetail);
		this.groupBox3.Controls.Add(this.label47);
		this.groupBox3.Controls.Add(this.txtRateDetail);
		this.groupBox3.Controls.Add(this.txtAmountDetail);
		this.groupBox3.Controls.Add(this.txtWeightDetail);
		this.groupBox3.Controls.Add(this.txtQtyDetail);
		this.groupBox3.Controls.Add(this.label40);
		this.groupBox3.Controls.Add(this.label44);
		this.groupBox3.Controls.Add(this.label3);
		this.groupBox3.Controls.Add(this.label41);
		this.groupBox3.Controls.Add(this.label39);
		this.groupBox3.Controls.Add(this.label38);
		this.groupBox3.Controls.Add(this.label36);
		this.groupBox3.Controls.Add(this.label37);
		this.groupBox3.Controls.Add(this.btnUpdateDetail);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(4, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1122, 113);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Detail";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.ForeColor = System.Drawing.Color.Red;
		this.label63.Location = new System.Drawing.Point(1033, 64);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(14, 17);
		this.label63.TabIndex = 218;
		this.label63.Text = "*";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.ForeColor = System.Drawing.Color.Red;
		this.label62.Location = new System.Drawing.Point(540, 64);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(14, 17);
		this.label62.TabIndex = 218;
		this.label62.Text = "*";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.ForeColor = System.Drawing.Color.Red;
		this.label61.Location = new System.Drawing.Point(432, 64);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(14, 17);
		this.label61.TabIndex = 218;
		this.label61.Text = "*";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.ForeColor = System.Drawing.Color.Red;
		this.label60.Location = new System.Drawing.Point(349, 64);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(14, 17);
		this.label60.TabIndex = 218;
		this.label60.Text = "*";
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label59.ForeColor = System.Drawing.Color.Red;
		this.label59.Location = new System.Drawing.Point(257, 64);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(14, 17);
		this.label59.TabIndex = 218;
		this.label59.Text = "*";
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.ForeColor = System.Drawing.Color.Red;
		this.label58.Location = new System.Drawing.Point(163, 64);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(14, 17);
		this.label58.TabIndex = 218;
		this.label58.Text = "*";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.ForeColor = System.Drawing.Color.Red;
		this.label57.Location = new System.Drawing.Point(68, 64);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(14, 17);
		this.label57.TabIndex = 218;
		this.label57.Text = "*";
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.ForeColor = System.Drawing.Color.Red;
		this.label56.Location = new System.Drawing.Point(1102, 17);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(14, 17);
		this.label56.TabIndex = 218;
		this.label56.Text = "*";
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.ForeColor = System.Drawing.Color.Red;
		this.label50.Location = new System.Drawing.Point(971, 17);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(14, 17);
		this.label50.TabIndex = 218;
		this.label50.Text = "*";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.ForeColor = System.Drawing.Color.Red;
		this.label46.Location = new System.Drawing.Point(885, 17);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(14, 17);
		this.label46.TabIndex = 218;
		this.label46.Text = "*";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.ForeColor = System.Drawing.Color.Red;
		this.label45.Location = new System.Drawing.Point(163, 17);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(14, 17);
		this.label45.TabIndex = 218;
		this.label45.Text = "*";
		this.CmbOrderNoDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbOrderNoDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOrderNoDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOrderNoDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance63;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance78).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance141).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOrderNoDetail).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbOrderNoDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOrderNoDetail).Location = new System.Drawing.Point(183, 36);
		((System.Windows.Forms.Control)(object)this.CmbOrderNoDetail).Name = "CmbOrderNoDetail";
		((System.Windows.Forms.Control)(object)this.CmbOrderNoDetail).Size = new System.Drawing.Size(99, 26);
		((System.Windows.Forms.Control)(object)this.CmbOrderNoDetail).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbOrderNoDetail).Leave += new System.EventHandler(CmbOrderNoDetail_Leave);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(183, 18);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(53, 15);
		this.label4.TabIndex = 56;
		this.label4.Text = "OrderNo";
		this.CmbBranchDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbBranchDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranchDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranchDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranchDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance51;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranchDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance57).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance58).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranchDetail).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBranchDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBranchDetail).Location = new System.Drawing.Point(905, 82);
		((System.Windows.Forms.Control)(object)this.CmbBranchDetail).Name = "CmbBranchDetail";
		((System.Windows.Forms.Control)(object)this.CmbBranchDetail).Size = new System.Drawing.Size(146, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranchDetail).TabIndex = 14;
		this.CmbRefPartyDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbRefPartyDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRefPartyDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRefPartyDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance10;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance29).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRefPartyDetail).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRefPartyDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRefPartyDetail).Location = new System.Drawing.Point(497, 36);
		((System.Windows.Forms.Control)(object)this.CmbRefPartyDetail).Name = "CmbRefPartyDetail";
		((System.Windows.Forms.Control)(object)this.CmbRefPartyDetail).Size = new System.Drawing.Size(194, 26);
		((System.Windows.Forms.Control)(object)this.CmbRefPartyDetail).TabIndex = 3;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(905, 65);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(108, 15);
		this.label8.TabIndex = 55;
		this.label8.Text = "Receiver's Location";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(497, 18);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(89, 15);
		this.label18.TabIndex = 55;
		this.label18.Text = "Reference Party";
		this.CmbCommissionAgentDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbCommissionAgentDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommissionAgentDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommissionAgentDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommissionAgentDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbCommissionAgentDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommissionAgentDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionAgentDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionAgentDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance116;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionAgentDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionAgentDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbCommissionAgentDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommissionAgentDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommissionAgentDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommissionAgentDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbCommissionAgentDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommissionAgentDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAgentDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance128).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommissionAgentDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbCommissionAgentDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommissionAgentDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAgentDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommissionAgentDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbCommissionAgentDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommissionAgentDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommissionAgentDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbCommissionAgentDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommissionAgentDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbCommissionAgentDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommissionAgentDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommissionAgentDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentDetail).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommissionAgentDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentDetail).Location = new System.Drawing.Point(284, 36);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentDetail).Name = "CmbCommissionAgentDetail";
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentDetail).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentDetail).TabIndex = 2;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(284, 18);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(108, 15);
		this.label13.TabIndex = 55;
		this.label13.Text = "Commission Agent";
		this.CmbSupplierDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplierDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplierDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplierDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance103;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance95).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance96).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplierDetail).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplierDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplierDetail).Location = new System.Drawing.Point(3, 36);
		((System.Windows.Forms.Control)(object)this.CmbSupplierDetail).Name = "CmbSupplierDetail";
		((System.Windows.Forms.Control)(object)this.CmbSupplierDetail).Size = new System.Drawing.Size(179, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplierDetail).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbSupplierDetail).Leave += new System.EventHandler(CmbSupplierDetail_Leave);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(3, 18);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(86, 15);
		this.label10.TabIndex = 55;
		this.label10.Text = "Supplier Name";
		this.rdSearchByCode.AutoSize = true;
		this.rdSearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCode.Location = new System.Drawing.Point(800, 16);
		this.rdSearchByCode.Name = "rdSearchByCode";
		this.rdSearchByCode.Size = new System.Drawing.Size(52, 19);
		this.rdSearchByCode.TabIndex = 52;
		this.rdSearchByCode.TabStop = true;
		this.rdSearchByCode.Text = "Code";
		this.rdSearchByCode.UseVisualStyleBackColor = true;
		this.rdSearchByName.AutoSize = true;
		this.rdSearchByName.Checked = true;
		this.rdSearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByName.Location = new System.Drawing.Point(741, 16);
		this.rdSearchByName.Name = "rdSearchByName";
		this.rdSearchByName.Size = new System.Drawing.Size(57, 19);
		this.rdSearchByName.TabIndex = 51;
		this.rdSearchByName.TabStop = true;
		this.rdSearchByName.Text = "Name";
		this.rdSearchByName.UseVisualStyleBackColor = true;
		this.rdSearchByName.CheckedChanged += new System.EventHandler(rdSearchByName_CheckedChanged);
		this.CmbPackingTypeDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingTypeDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingTypeDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingTypeDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance146).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance87).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackingTypeDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeDetail).Location = new System.Drawing.Point(991, 36);
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeDetail).Name = "CmbPackingTypeDetail";
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeDetail).Size = new System.Drawing.Size(127, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeDetail).TabIndex = 6;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(991, 18);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(60, 15);
		this.label1.TabIndex = 34;
		this.label1.Text = "Pack Type";
		this.CmbRateUomDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUomDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUomDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUomDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance37;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance43).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUomDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRateUomDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUomDetail).Location = new System.Drawing.Point(365, 82);
		((System.Windows.Forms.Control)(object)this.CmbRateUomDetail).Name = "CmbRateUomDetail";
		((System.Windows.Forms.Control)(object)this.CmbRateUomDetail).Size = new System.Drawing.Size(84, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateUomDetail).TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.CmbRateUomDetail).TextChanged += new System.EventHandler(combrateuom_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbRateUomDetail).Leave += new System.EventHandler(combrateuom_Leave);
		this.CmbCropYearDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYearDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYearDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYearDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance102;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYearDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropYearDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYearDetail).Location = new System.Drawing.Point(905, 36);
		((System.Windows.Forms.Control)(object)this.CmbCropYearDetail).Name = "CmbCropYearDetail";
		((System.Windows.Forms.Control)(object)this.CmbCropYearDetail).Size = new System.Drawing.Size(84, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYearDetail).TabIndex = 5;
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1053, 64);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(63, 21);
		this.btnCancelUpdateDetial.TabIndex = 17;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.CmbPackUomDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUomDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUomDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUomDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance106;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance111).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance133).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUomDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUomDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUomDetail).Location = new System.Drawing.Point(3, 82);
		((System.Windows.Forms.Control)(object)this.CmbPackUomDetail).Name = "CmbPackUomDetail";
		((System.Windows.Forms.Control)(object)this.CmbPackUomDetail).Size = new System.Drawing.Size(84, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUomDetail).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.CmbPackUomDetail).Leave += new System.EventHandler(combitempck_Leave);
		this.CmbItemNameDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbItemNameDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemNameDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemNameDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance139;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance148).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance149).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemNameDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemNameDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemNameDetail).Location = new System.Drawing.Point(693, 36);
		((System.Windows.Forms.Control)(object)this.CmbItemNameDetail).Name = "CmbItemNameDetail";
		((System.Windows.Forms.Control)(object)this.CmbItemNameDetail).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemNameDetail).TabIndex = 4;
		this.CmbItemNameDetail.ValueChanged += new System.EventHandler(combitem_ValueChanged);
		((System.Windows.Forms.Control)(object)this.CmbItemNameDetail).Leave += new System.EventHandler(combitem_Leave);
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(1053, 87);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(39, 21);
		this.btnplus.TabIndex = 15;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.txtRemarksdetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksdetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksdetail.Location = new System.Drawing.Point(560, 84);
		this.txtRemarksdetail.Name = "txtRemarksdetail";
		this.txtRemarksdetail.Size = new System.Drawing.Size(343, 23);
		this.txtRemarksdetail.TabIndex = 13;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label47.Location = new System.Drawing.Point(560, 65);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(52, 15);
		this.label47.TabIndex = 25;
		this.label47.Text = "Remarks";
		this.txtRateDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRateDetail.Location = new System.Drawing.Point(284, 84);
		this.txtRateDetail.Name = "txtRateDetail";
		this.txtRateDetail.Size = new System.Drawing.Size(79, 23);
		this.txtRateDetail.TabIndex = 10;
		this.txtRateDetail.Text = "0";
		this.txtRateDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRateDetail.TextChanged += new System.EventHandler(txtRate_TextChanged);
		this.txtRateDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRate_KeyPress);
		this.txtAmountDetail.BackColor = System.Drawing.Color.White;
		this.txtAmountDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmountDetail.Enabled = false;
		this.txtAmountDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmountDetail.Location = new System.Drawing.Point(451, 84);
		this.txtAmountDetail.Name = "txtAmountDetail";
		this.txtAmountDetail.Size = new System.Drawing.Size(108, 23);
		this.txtAmountDetail.TabIndex = 12;
		this.txtAmountDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtWeightDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeightDetail.Enabled = false;
		this.txtWeightDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWeightDetail.Location = new System.Drawing.Point(183, 84);
		this.txtWeightDetail.Name = "txtWeightDetail";
		this.txtWeightDetail.Size = new System.Drawing.Size(99, 23);
		this.txtWeightDetail.TabIndex = 9;
		this.txtWeightDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtWeightDetail.TextChanged += new System.EventHandler(txtweight_TextChanged);
		this.txtWeightDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtWeightDetail_KeyPress);
		this.txtQtyDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQtyDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyDetail.Location = new System.Drawing.Point(90, 84);
		this.txtQtyDetail.Name = "txtQtyDetail";
		this.txtQtyDetail.Size = new System.Drawing.Size(92, 23);
		this.txtQtyDetail.TabIndex = 8;
		this.txtQtyDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQtyDetail.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtQtyDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label40.Location = new System.Drawing.Point(284, 65);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(30, 15);
		this.label40.TabIndex = 17;
		this.label40.Text = "Rate";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label44.Location = new System.Drawing.Point(451, 65);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(51, 15);
		this.label44.TabIndex = 23;
		this.label44.Text = "Amount";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(905, 18);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(57, 15);
		this.label3.TabIndex = 19;
		this.label3.Text = "Crop Year";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label41.Location = new System.Drawing.Point(365, 65);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(61, 15);
		this.label41.TabIndex = 19;
		this.label41.Text = "Rate UOM";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label39.Location = new System.Drawing.Point(183, 65);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(68, 15);
		this.label39.TabIndex = 15;
		this.label39.Text = "Net Weight";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(90, 65);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(26, 15);
		this.label38.TabIndex = 13;
		this.label38.Text = "Qty";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label36.Location = new System.Drawing.Point(3, 65);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(61, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "Pack Uom";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label37.Location = new System.Drawing.Point(693, 18);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(32, 15);
		this.label37.TabIndex = 9;
		this.label37.Text = "Item";
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(1053, 87);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(63, 21);
		this.btnUpdateDetail.TabIndex = 16;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 51);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1132, 201);
		this.panel4.TabIndex = 0;
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.label48);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add(this.label66);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.label65);
		this.groupBox1.Controls.Add(this.label64);
		this.groupBox1.Controls.Add(this.label43);
		this.groupBox1.Controls.Add(this.label42);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.label34);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.txtVehicleNoMain);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCityNameMain);
		this.groupBox1.Controls.Add(this.txtDriverCell2Main);
		this.groupBox1.Controls.Add(this.txtTotalAmountMain);
		this.groupBox1.Controls.Add(this.txtDriverCell1Main);
		this.groupBox1.Controls.Add(this.txtDriverNameMain);
		this.groupBox1.Controls.Add(this.txtOtherExpenseMain);
		this.groupBox1.Controls.Add(this.txtETAdestinationMain);
		this.groupBox1.Controls.Add(this.txtFreightMain);
		this.groupBox1.Controls.Add(this.txtExchangeRateMain);
		this.groupBox1.Controls.Add(this.txtSupplierWbWtMain);
		this.groupBox1.Controls.Add(this.txtTotalWeightMain);
		this.groupBox1.Controls.Add(this.txtQtyMain);
		this.groupBox1.Controls.Add(this.txtFcyAmountMain);
		this.groupBox1.Controls.Add(this.txtTransitDaysMain);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCurrencyMain);
		this.groupBox1.Controls.Add(this.BtnBrowseDriverImage);
		this.groupBox1.Controls.Add(this.BtnBrowseVehicleImage);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.PicDriverImage);
		this.groupBox1.Controls.Add(this.PicVehicleImage);
		this.groupBox1.Controls.Add(this.txtDepartureDateMain);
		this.groupBox1.Controls.Add(this.txtDocDateMain);
		this.groupBox1.Controls.Add(this.txtRemarksMain);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.txtBranchSrNo);
		this.groupBox1.Controls.Add(this.txtDocNoMain);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.label25);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.txtBiltyNoMain);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1122, 197);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(541, 95);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(80, 15);
		this.label22.TabIndex = 37;
		this.label22.Text = "Total Amount";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label48.Location = new System.Drawing.Point(541, 173);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(72, 15);
		this.label48.TabIndex = 43;
		this.label48.Text = "Fcy Amount";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(541, 121);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(55, 15);
		this.label15.TabIndex = 36;
		this.label15.Text = "Fcy Code";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(541, 148);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(78, 15);
		this.label16.TabIndex = 42;
		this.label16.Text = "Echange Rate";
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label66.Location = new System.Drawing.Point(541, 18);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(89, 15);
		this.label66.TabIndex = 3;
		this.label66.Text = "SupplierWb Wt";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(541, 68);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(75, 15);
		this.label24.TabIndex = 3;
		this.label24.Text = "Total Weight";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(541, 43);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(55, 15);
		this.label21.TabIndex = 3;
		this.label21.Text = "Total Qty";
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.ForeColor = System.Drawing.Color.Red;
		this.label65.Location = new System.Drawing.Point(626, 17);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(14, 17);
		this.label65.TabIndex = 218;
		this.label65.Text = "*";
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.ForeColor = System.Drawing.Color.Red;
		this.label64.Location = new System.Drawing.Point(626, 67);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(14, 17);
		this.label64.TabIndex = 218;
		this.label64.Text = "*";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.Color.Red;
		this.label43.Location = new System.Drawing.Point(626, 120);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(14, 17);
		this.label43.TabIndex = 218;
		this.label43.Text = "*";
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.ForeColor = System.Drawing.Color.Red;
		this.label42.Location = new System.Drawing.Point(626, 94);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(14, 17);
		this.label42.TabIndex = 218;
		this.label42.Text = "*";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.ForeColor = System.Drawing.Color.Red;
		this.label35.Location = new System.Drawing.Point(626, 42);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(14, 17);
		this.label35.TabIndex = 218;
		this.label35.Text = "*";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.ForeColor = System.Drawing.Color.Red;
		this.label34.Location = new System.Drawing.Point(391, 148);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(14, 17);
		this.label34.TabIndex = 218;
		this.label34.Text = "*";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.Red;
		this.label32.Location = new System.Drawing.Point(391, 93);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(14, 17);
		this.label32.TabIndex = 218;
		this.label32.Text = "*";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.ForeColor = System.Drawing.Color.Red;
		this.label31.Location = new System.Drawing.Point(391, 41);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(14, 17);
		this.label31.TabIndex = 218;
		this.label31.Text = "*";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.Red;
		this.label30.Location = new System.Drawing.Point(72, 121);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(14, 17);
		this.label30.TabIndex = 218;
		this.label30.Text = "*";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.Color.Red;
		this.label29.Location = new System.Drawing.Point(72, 93);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(14, 17);
		this.label29.TabIndex = 218;
		this.label29.Text = "*";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.ForeColor = System.Drawing.Color.Red;
		this.label27.Location = new System.Drawing.Point(72, 68);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(14, 17);
		this.label27.TabIndex = 218;
		this.label27.Text = "*";
		this.txtVehicleNoMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleNoMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtVehicleNoMain.Location = new System.Drawing.Point(87, 39);
		this.txtVehicleNoMain.Name = "txtVehicleNoMain";
		this.txtVehicleNoMain.Size = new System.Drawing.Size(224, 23);
		this.txtVehicleNoMain.TabIndex = 3;
		this.CmbCityNameMain.AllowNull = (DefaultableBoolean)1;
		this.CmbCityNameMain.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCityNameMain.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCityNameMain.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCityNameMain).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbCityNameMain).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCityNameMain).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCityNameMain.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCityNameMain).Location = new System.Drawing.Point(407, 142);
		((System.Windows.Forms.Control)(object)this.CmbCityNameMain).Name = "CmbCityNameMain";
		((System.Windows.Forms.Control)(object)this.CmbCityNameMain).Size = new System.Drawing.Size(133, 26);
		((System.Windows.Forms.Control)(object)this.CmbCityNameMain).TabIndex = 14;
		this.txtDriverCell2Main.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDriverCell2Main.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDriverCell2Main.Location = new System.Drawing.Point(87, 144);
		this.txtDriverCell2Main.Name = "txtDriverCell2Main";
		this.txtDriverCell2Main.Size = new System.Drawing.Size(224, 23);
		this.txtDriverCell2Main.TabIndex = 7;
		this.txtTotalAmountMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalAmountMain.Enabled = false;
		this.txtTotalAmountMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTotalAmountMain.Location = new System.Drawing.Point(642, 91);
		this.txtTotalAmountMain.Name = "txtTotalAmountMain";
		this.txtTotalAmountMain.Size = new System.Drawing.Size(125, 23);
		this.txtTotalAmountMain.TabIndex = 18;
		this.txtTotalAmountMain.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDriverCell1Main.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDriverCell1Main.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDriverCell1Main.Location = new System.Drawing.Point(87, 117);
		this.txtDriverCell1Main.Name = "txtDriverCell1Main";
		this.txtDriverCell1Main.Size = new System.Drawing.Size(224, 23);
		this.txtDriverCell1Main.TabIndex = 6;
		this.txtDriverNameMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDriverNameMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDriverNameMain.Location = new System.Drawing.Point(87, 91);
		this.txtDriverNameMain.Name = "txtDriverNameMain";
		this.txtDriverNameMain.Size = new System.Drawing.Size(224, 23);
		this.txtDriverNameMain.TabIndex = 5;
		this.txtOtherExpenseMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherExpenseMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOtherExpenseMain.Location = new System.Drawing.Point(407, 117);
		this.txtOtherExpenseMain.Name = "txtOtherExpenseMain";
		this.txtOtherExpenseMain.Size = new System.Drawing.Size(133, 23);
		this.txtOtherExpenseMain.TabIndex = 13;
		this.txtOtherExpenseMain.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOtherExpenseMain_KeyPress);
		this.txtETAdestinationMain.CustomFormat = "dd-MMM-yy";
		this.txtETAdestinationMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtETAdestinationMain.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtETAdestinationMain.Location = new System.Drawing.Point(407, 64);
		this.txtETAdestinationMain.Name = "txtETAdestinationMain";
		this.txtETAdestinationMain.Size = new System.Drawing.Size(133, 23);
		this.txtETAdestinationMain.TabIndex = 11;
		this.txtETAdestinationMain.TabStop = false;
		this.txtETAdestinationMain.ValueChanged += new System.EventHandler(txtETAdestinationMain_ValueChanged);
		this.txtFreightMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreightMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFreightMain.Location = new System.Drawing.Point(407, 91);
		this.txtFreightMain.Name = "txtFreightMain";
		this.txtFreightMain.Size = new System.Drawing.Size(133, 23);
		this.txtFreightMain.TabIndex = 12;
		this.txtFreightMain.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFreightMain_KeyPress);
		this.txtExchangeRateMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRateMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtExchangeRateMain.Location = new System.Drawing.Point(642, 144);
		this.txtExchangeRateMain.Name = "txtExchangeRateMain";
		this.txtExchangeRateMain.Size = new System.Drawing.Size(125, 23);
		this.txtExchangeRateMain.TabIndex = 20;
		this.txtExchangeRateMain.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRateMain.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.txtExchangeRateMain.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.txtExchangeRateMain.Leave += new System.EventHandler(txtExchangeRate_Leave);
		this.txtSupplierWbWtMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierWbWtMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtSupplierWbWtMain.Location = new System.Drawing.Point(642, 14);
		this.txtSupplierWbWtMain.Name = "txtSupplierWbWtMain";
		this.txtSupplierWbWtMain.Size = new System.Drawing.Size(125, 23);
		this.txtSupplierWbWtMain.TabIndex = 15;
		this.txtSupplierWbWtMain.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSupplierWbWtMain.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSupplierWbWtMain_KeyPress);
		this.txtTotalWeightMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalWeightMain.Enabled = false;
		this.txtTotalWeightMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTotalWeightMain.Location = new System.Drawing.Point(642, 64);
		this.txtTotalWeightMain.Name = "txtTotalWeightMain";
		this.txtTotalWeightMain.Size = new System.Drawing.Size(125, 23);
		this.txtTotalWeightMain.TabIndex = 17;
		this.txtTotalWeightMain.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQtyMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQtyMain.Enabled = false;
		this.txtQtyMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtQtyMain.Location = new System.Drawing.Point(642, 39);
		this.txtQtyMain.Name = "txtQtyMain";
		this.txtQtyMain.Size = new System.Drawing.Size(125, 23);
		this.txtQtyMain.TabIndex = 16;
		this.txtQtyMain.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFcyAmountMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmountMain.Enabled = false;
		this.txtFcyAmountMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFcyAmountMain.Location = new System.Drawing.Point(642, 169);
		this.txtFcyAmountMain.Name = "txtFcyAmountMain";
		this.txtFcyAmountMain.Size = new System.Drawing.Size(125, 23);
		this.txtFcyAmountMain.TabIndex = 21;
		this.txtFcyAmountMain.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTransitDaysMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTransitDaysMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTransitDaysMain.Location = new System.Drawing.Point(407, 39);
		this.txtTransitDaysMain.Name = "txtTransitDaysMain";
		this.txtTransitDaysMain.Size = new System.Drawing.Size(133, 23);
		this.txtTransitDaysMain.TabIndex = 10;
		this.txtTransitDaysMain.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtTransitDaysMain_KeyPress);
		this.txtTransitDaysMain.Leave += new System.EventHandler(txtTransitDaysMain_TextChanged);
		this.CmbCurrencyMain.AllowNull = (DefaultableBoolean)1;
		this.CmbCurrencyMain.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCurrencyMain.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCurrencyMain.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Appearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance123;
		((SpecialBoxBase)((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance85).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance86).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance125).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCurrencyMain).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCurrencyMain.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCurrencyMain).Location = new System.Drawing.Point(642, 115);
		((System.Windows.Forms.Control)(object)this.CmbCurrencyMain).Name = "CmbCurrencyMain";
		((System.Windows.Forms.Control)(object)this.CmbCurrencyMain).Size = new System.Drawing.Size(125, 26);
		((System.Windows.Forms.Control)(object)this.CmbCurrencyMain).TabIndex = 19;
		this.BtnBrowseDriverImage.BackColor = System.Drawing.Color.Teal;
		this.BtnBrowseDriverImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnBrowseDriverImage.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnBrowseDriverImage.ForeColor = System.Drawing.Color.White;
		this.BtnBrowseDriverImage.Location = new System.Drawing.Point(948, 167);
		this.BtnBrowseDriverImage.Name = "BtnBrowseDriverImage";
		this.BtnBrowseDriverImage.Size = new System.Drawing.Size(174, 26);
		this.BtnBrowseDriverImage.TabIndex = 23;
		this.BtnBrowseDriverImage.Text = "Browse Driver Pic";
		this.BtnBrowseDriverImage.UseVisualStyleBackColor = false;
		this.BtnBrowseDriverImage.Click += new System.EventHandler(BtnBrowseDriverImage_Click);
		this.BtnBrowseVehicleImage.BackColor = System.Drawing.Color.Teal;
		this.BtnBrowseVehicleImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnBrowseVehicleImage.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnBrowseVehicleImage.ForeColor = System.Drawing.Color.White;
		this.BtnBrowseVehicleImage.Location = new System.Drawing.Point(770, 167);
		this.BtnBrowseVehicleImage.Name = "BtnBrowseVehicleImage";
		this.BtnBrowseVehicleImage.Size = new System.Drawing.Size(174, 26);
		this.BtnBrowseVehicleImage.TabIndex = 22;
		this.BtnBrowseVehicleImage.Text = "Browse Vehicle Pic";
		this.BtnBrowseVehicleImage.UseVisualStyleBackColor = false;
		this.BtnBrowseVehicleImage.Click += new System.EventHandler(BtnBrowseVehicleImage_Click);
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(316, 148);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(62, 15);
		this.label28.TabIndex = 34;
		this.label28.Text = "City Name";
		this.PicDriverImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PicDriverImage.Location = new System.Drawing.Point(948, 11);
		this.PicDriverImage.Name = "PicDriverImage";
		this.PicDriverImage.Size = new System.Drawing.Size(174, 154);
		this.PicDriverImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.PicDriverImage.TabIndex = 217;
		this.PicDriverImage.TabStop = false;
		this.PicVehicleImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PicVehicleImage.Location = new System.Drawing.Point(770, 11);
		this.PicVehicleImage.Name = "PicVehicleImage";
		this.PicVehicleImage.Size = new System.Drawing.Size(174, 154);
		this.PicVehicleImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.PicVehicleImage.TabIndex = 217;
		this.PicVehicleImage.TabStop = false;
		this.txtDepartureDateMain.CustomFormat = "dd-MMM-yy";
		this.txtDepartureDateMain.Enabled = false;
		this.txtDepartureDateMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDepartureDateMain.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDepartureDateMain.Location = new System.Drawing.Point(407, 14);
		this.txtDepartureDateMain.Name = "txtDepartureDateMain";
		this.txtDepartureDateMain.Size = new System.Drawing.Size(133, 23);
		this.txtDepartureDateMain.TabIndex = 9;
		this.txtDepartureDateMain.TabStop = false;
		this.txtDocDateMain.CustomFormat = "dd-MMM-yy";
		this.txtDocDateMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDocDateMain.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocDateMain.Location = new System.Drawing.Point(199, 14);
		this.txtDocDateMain.Name = "txtDocDateMain";
		this.txtDocDateMain.Size = new System.Drawing.Size(112, 23);
		this.txtDocDateMain.TabIndex = 2;
		this.txtDocDateMain.TabStop = false;
		this.txtDocDateMain.ValueChanged += new System.EventHandler(txtDocDateMain_ValueChanged);
		this.txtRemarksMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRemarksMain.Location = new System.Drawing.Point(87, 169);
		this.txtRemarksMain.Name = "txtRemarksMain";
		this.txtRemarksMain.Size = new System.Drawing.Size(453, 23);
		this.txtRemarksMain.TabIndex = 8;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(316, 68);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(91, 15);
		this.label11.TabIndex = 3;
		this.label11.Text = "ETA Destination";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(316, 18);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(88, 15);
		this.label9.TabIndex = 3;
		this.label9.Text = "Departure Date";
		this.txtBranchSrNo.BackColor = System.Drawing.Color.White;
		this.txtBranchSrNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBranchSrNo.Enabled = false;
		this.txtBranchSrNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBranchSrNo.Location = new System.Drawing.Point(150, 14);
		this.txtBranchSrNo.Name = "txtBranchSrNo";
		this.txtBranchSrNo.Size = new System.Drawing.Size(48, 23);
		this.txtBranchSrNo.TabIndex = 1;
		this.txtBranchSrNo.TabStop = false;
		this.txtBranchSrNo.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.txtDocNoMain.BackColor = System.Drawing.Color.White;
		this.txtDocNoMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoMain.Enabled = false;
		this.txtDocNoMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDocNoMain.Location = new System.Drawing.Point(87, 14);
		this.txtDocNoMain.Name = "txtDocNoMain";
		this.txtDocNoMain.Size = new System.Drawing.Size(62, 23);
		this.txtDocNoMain.TabIndex = 0;
		this.txtDocNoMain.TabStop = false;
		this.txtDocNoMain.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(1, 19);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(72, 13);
		this.label17.TabIndex = 3;
		this.label17.Text = "Doc No,Date";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(1, 173);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(52, 15);
		this.label33.TabIndex = 8;
		this.label33.Text = "Remarks";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(1, 148);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(71, 15);
		this.label20.TabIndex = 3;
		this.label20.Text = "Driver Cell 2";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(1, 121);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(69, 15);
		this.label19.TabIndex = 3;
		this.label19.Text = "Driver Cell 1";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(1, 95);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(74, 15);
		this.label14.TabIndex = 3;
		this.label14.Text = "Driver Name";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(316, 121);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(83, 15);
		this.label26.TabIndex = 3;
		this.label26.Text = "Other Expense";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(316, 95);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(44, 15);
		this.label25.TabIndex = 3;
		this.label25.Text = "Freight";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(1, 43);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(64, 15);
		this.label12.TabIndex = 3;
		this.label12.Text = "Vehicle No";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(1, 68);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(49, 15);
		this.label7.TabIndex = 3;
		this.label7.Text = "Bilty No";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(316, 43);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(71, 15);
		this.label6.TabIndex = 3;
		this.label6.Text = "Transit Days";
		this.txtBiltyNoMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBiltyNoMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBiltyNoMain.Location = new System.Drawing.Point(87, 64);
		this.txtBiltyNoMain.Name = "txtBiltyNoMain";
		this.txtBiltyNoMain.Size = new System.Drawing.Size(224, 23);
		this.txtBiltyNoMain.TabIndex = 4;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.ChkBox);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1132, 24);
		this.panel3.TabIndex = 0;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(6, 2);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(151, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "Supplier Dispatch";
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.Location = new System.Drawing.Point(1008, 3);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(122, 20);
		this.ChkBox.TabIndex = 0;
		this.ChkBox.Text = "Print Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1132, 25);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.btnnew, this.btnRefresh, this.btnsave, this.btnSaveAs, this.btnupdate, this.btnDelete, this.btnattachment, this.btnprint, this.BtnShortCutkeys, this.BtnRefPartyForm });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1132, 27);
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
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnSaveAs.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveAs.Name = "btnSaveAs";
		this.btnSaveAs.Size = new System.Drawing.Size(80, 24);
		this.btnSaveAs.Text = "SaveAs";
		this.btnSaveAs.Visible = false;
		this.btnSaveAs.Click += new System.EventHandler(btnSaveAs_Click);
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.btnprint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(92, 24);
		this.btnprint.Text = "246-Print";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.BtnRefPartyForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefPartyForm.Image = Architecture.WinApp.Properties.Resources.Export_Reports;
		this.BtnRefPartyForm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefPartyForm.Name = "BtnRefPartyForm";
		this.BtnRefPartyForm.Size = new System.Drawing.Size(137, 24);
		this.BtnRefPartyForm.Text = "Reference Party";
		this.BtnRefPartyForm.Click += new System.EventHandler(BtnRefPartyForm_Click);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1136, 655);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add(this.panel12);
		this.panel7.Controls.Add(this.panel13);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail);
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Controls.Add(this.toolStrip2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(2, 2);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1132, 651);
		this.panel7.TabIndex = 0;
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel12.Location = new System.Drawing.Point(0, 141);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1132, 333);
		this.panel12.TabIndex = 47;
		this.grdhistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdhistory).BackColor = System.Drawing.Color.White;
		this.grdhistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistory.DesignTimeLayout = grdhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.DynamicFiltering = true;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1132, 333);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 0;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.SelectionChanged += new System.EventHandler(grdhistory_SelectionChanged);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdhistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdhistory_KeyDown);
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label49);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel13.Location = new System.Drawing.Point(0, 474);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1132, 19);
		this.panel13.TabIndex = 49;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label49.ForeColor = System.Drawing.Color.White;
		this.label49.Location = new System.Drawing.Point(2, 3);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(269, 14);
		this.label49.TabIndex = 0;
		this.label49.Text = "Detail Information Of Above Selected Row";
		this.DataGridHistoryDetail.AllowDelete = (InheritableBoolean)1;
		this.DataGridHistoryDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.DataGridHistoryDetail).BackColor = System.Drawing.Color.White;
		this.DataGridHistoryDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistoryDetail.DesignTimeLayout = DataGridHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.DataGridHistoryDetail.DynamicFiltering = true;
		this.DataGridHistoryDetail.FilterMode = (FilterMode)2;
		this.DataGridHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Location = new System.Drawing.Point(0, 493);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Name = "DataGridHistoryDetail";
		((ScrollableBase)this.DataGridHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Size = new System.Drawing.Size(1132, 158);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).TabIndex = 1;
		this.DataGridHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.DataGridHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label55);
		this.panel9.Controls.Add(this.ctrlGrdBar2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 119);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1132, 22);
		this.panel9.TabIndex = 47;
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label55.ForeColor = System.Drawing.Color.White;
		this.label55.Location = new System.Drawing.Point(1, 1);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(132, 18);
		this.label55.TabIndex = 0;
		this.label55.Text = "Filtered Records";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1087, -1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdhistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel8.Controls.Add(this.panel14);
		this.panel8.Controls.Add(this.groupBox2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 27);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1132, 92);
		this.panel8.TabIndex = 0;
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label5);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1132, 25);
		this.panel14.TabIndex = 50;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(3, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(214, 19);
		this.label5.TabIndex = 0;
		this.label5.Text = "Supplier Dispatch History";
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.rdapproveddate);
		this.groupBox2.Controls.Add(this.rdmodifydate);
		this.groupBox2.Controls.Add(this.drdocdate);
		this.groupBox2.Controls.Add(this.rdentrydate);
		this.groupBox2.Controls.Add(this.btnShow);
		this.groupBox2.Controls.Add(this.label51);
		this.groupBox2.Controls.Add(this.label52);
		this.groupBox2.Controls.Add(this.label53);
		this.groupBox2.Controls.Add(this.label54);
		this.groupBox2.Controls.Add(this.ToDocNoHistory);
		this.groupBox2.Controls.Add(this.FromDocNoHistory);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(2, 25);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1127, 66);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.cmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance48;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance3).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(6, 34);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(204, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Leave += new System.EventHandler(cmbBranchName_Leave);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(6, 18);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(79, 15);
		this.label2.TabIndex = 0;
		this.label2.Text = "Branch Name";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(845, 38);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(845, 16);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(764, 16);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(764, 38);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(699, 35);
		this.btnShow.Margin = new System.Windows.Forms.Padding(2);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(58, 25);
		this.btnShow.TabIndex = 13;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(613, 18);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(64, 15);
		this.label51.TabIndex = 9;
		this.label51.Text = "To Doc No";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(533, 18);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(79, 15);
		this.label52.TabIndex = 7;
		this.label52.Text = "From Doc No";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(371, 18);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(48, 15);
		this.label53.TabIndex = 5;
		this.label53.Text = "To Date";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(210, 18);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(63, 15);
		this.label54.TabIndex = 3;
		this.label54.Text = "From Date";
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(616, 36);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistory.TabIndex = 10;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(535, 36);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistory.TabIndex = 8;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(373, 36);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(159, 23);
		this.ToDateHistory.TabIndex = 6;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(213, 36);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(159, 23);
		this.FromDateHistory.TabIndex = 4;
		this.toolStrip2.BackColor = System.Drawing.Color.Transparent;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnResetHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1132, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(68, 24);
		this.btnResetHistory.Text = "Reset";
		this.btnResetHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1144, 684);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "frmSupplierDispatch";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(SupplierDispatch_FormClosing);
		base.Load += new System.EventHandler(SupplierDispatch_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(SupplierDispatch_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderNoDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBranchDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRefPartyDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionAgentDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingTypeDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUomDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYearDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUomDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameDetail).EndInit();
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCityNameMain).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCurrencyMain).EndInit();
		((System.ComponentModel.ISupportInitialize)this.PicDriverImage).EndInit();
		((System.ComponentModel.ISupportInitialize)this.PicVehicleImage).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel12.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistoryDetail).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
