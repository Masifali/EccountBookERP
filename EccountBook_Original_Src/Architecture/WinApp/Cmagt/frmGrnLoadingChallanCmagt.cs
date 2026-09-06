using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.CommissionAgent;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.CommissionAgent;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.CommissionAgent.FromWise_Helper_Methods;
using Architecture.WinApp.CommissionAgent.Helper_Methods;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Cmagt;

public class frmGrnLoadingChallanCmagt : Form
{
	private Attachment AT = new Attachment();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private bool FormInitialized;

	private bool ItemSearchWithNameOrCode = true;

	private bool GrnLoadingAllowDifferentBiltyDate;

	private bool GrnLoadingDocDateAndBiltyDateSame;

	private int RecId;

	private int updateDetailIndex = -1;

	private int DocumentTypeId = 1054;

	private int DefaultDaysToLessFromHistoryFromDate;

	private decimal TolerancePercentageForCmagtPoWeight;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private string ScreenName = "frmGrnLoadingChallanCmagt";

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtDetail = new DataTable();

	private DataTable dtExpGrid = new DataTable();

	private DataTable dtEmptyBags = new DataTable();

	private DataTable dtHistoryForLayout = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtCrop = new DataTable();

	private DataTable dtPackType = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtViewsData = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private DataTable dtHistoryCombo = new DataTable();

	private DataTable dtShipToAddress = new DataTable();

	private DataTable dtItemForEmptyBag = new DataTable();

	private DataTable dtPendingOrdersData = new DataTable();

	private DataTable dtEbGridPackingTypes = new DataTable();

	private DataTable dtExpenseDataOrderWise = new DataTable();

	private DataTable dtEmptyBagDataOrderWise = new DataTable();

	private DataTable dtEmptyBagCutDataOrderWise = new DataTable();

	private DataTable dtSaleOrdersPurchaseOrderWise = new DataTable();

	private static bool CalculateByEbUnit = true;

	private static bool CalculateByEbTotal = false;

	private List<grnSupplierLoadingDetail> lstRemoveRecordDetail = new List<grnSupplierLoadingDetail>();

	private bool _internalChange;

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabForm;

	private TabPage tabHistory;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton BtnRefresh;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnDelete;

	private ToolStripButton BtnShortCutkeys;

	private ToolStripSeparator toolStripSeparator2;

	private Panel panel3;

	private Label label23;

	private CheckBox chkPrint;

	private Panel panel1;

	private DateTimePicker datDocDate;

	private Label label17;

	private TextBox txtDocNo;

	private Label label3;

	internal UltraCombo CmbSupplierName;

	private Label label7;

	internal UltraCombo CmbCommissionAgent;

	private Label label8;

	internal UltraCombo CmbLoadingCity;

	private Label label12;

	private TextBox txtRemarks;

	private Panel panel2;

	private Label label15;

	internal UltraCombo CmbItemName;

	private Label label14;

	internal UltraCombo CmbParentItem;

	private Label label19;

	internal UltraCombo CmbPackingType;

	private TextBox txtGrossWeight;

	private Label label16;

	private TextBox txtLoadingQty;

	private Label label21;

	internal UltraCombo CmbPackUom;

	private Label label20;

	internal UltraCombo CmbCropYear;

	private Label label26;

	internal UltraCombo CmbCompanyName;

	private ToolStripButton btnAttachments;

	private ToolStripButton btnPrint;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripButton BtnRefreshHistory;

	private Panel panel4;

	private Label label30;

	private Panel panel13;

	private GroupBox groupBox1;

	internal UltraCombo CmbBuyerNameHistory;

	private Label label29;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Button btnshow;

	internal UltraCombo CmbCommissionAgentHistory;

	private Label label31;

	private DateTimePicker ToDateHistory;

	private Label label33;

	private DateTimePicker FromDateHistory;

	private DateTimePicker datValidityToHistory;

	private Label label32;

	private DateTimePicker datValidityFromHistory;

	internal UltraCombo CmbItemNameHistory;

	private Label label37;

	internal UltraCombo CmbParentItemHistory;

	private Label label36;

	private Panel panel15;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label43;

	internal GridEX grdHistory;

	internal UltraCombo CmbTransporter;

	private Label label9;

	private GroupBox groupBox2;

	private GroupBox groupBox6;

	private TextBox txtRemarksDetail;

	private Label label51;

	private Button BtnAdd;

	private Button BtnCancelDetail;

	private Button BtnUpdateDetail;

	internal GridEX grdDetail;

	private Panel panel14;

	private CtrlGrdBar ctrlGrdBar3;

	internal GridEX grdEmptyBags;

	private Panel panel5;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel7;

	private CtrlGrdBar ctrlGrdBar4;

	internal GridEX grdInvExp;

	internal GridEX grdHistoryDetail;

	private Label label27;

	private Label label54;

	internal UltraCombo CmbBranch;

	private Label label35;

	internal UltraCombo CmbBuyerNameDetail;

	private Label label34;

	internal UltraCombo CmbPoNoDetail;

	private TextBox txtPoTotalWeightDetail;

	private Label label39;

	private TextBox txtPoBalWeightDetail;

	private Label label56;

	private TextBox txtPoAlreadyReceivedWeightDetail;

	private Label label55;

	private Label label6;

	internal UltraCombo CmbUnloadingCity;

	private Label label1;

	private TextBox txtBiltyFreight;

	private TextBox txtTransporterName;

	private Label label10;

	private Label label2;

	private Label label4;

	private Label label5;

	private TextBox txtVehicleQty;

	private TextBox txtBiltyNo;

	private TextBox txtOtherCharges;

	private TextBox txtVehicleNo;

	private TextBox txtTotalFreight;

	private DateTimePicker txtBiltyDate;

	private Label label13;

	private Label label44;

	private Label label42;

	private Label label41;

	private TextBox txtWbNetWeight;

	private TextBox txtTareWeight;

	private TextBox txtLoadWeight;

	private Panel panel6;

	private Panel panel10;

	private Panel panel8;

	private Label label22;

	private RadioButton RadItemCodePackListDetail;

	private RadioButton RadItemNamePackListDetail;

	private TextBox txtEbUnit;

	private TextBox txtAddLessWeight;

	private TextBox txtEbTotal;

	private TextBox txtNetWeight;

	private Label label46;

	private Label label25;

	private Label label45;

	private Label label28;

	private Label label38;

	internal UltraCombo CmbDeliveryTerm;

	private Label label18;

	private Label label24;

	private Label label47;

	private Label label60;

	internal UltraCombo CmbShipToAddress;

	private TextBox txtDeliveryToAddress;

	private Label label40;

	internal UltraCombo CmbEmptyBagTerm;

	private Label label48;

	private Button btnDefineShipToAddress;

	private Button BtnLoadOrder;

	internal UltraCombo cmbVehicleType;

	private Label label49;

	private TextBox txtSaleOrderNo;

	private TabControl tabControl3;

	private TabPage tabPage1;

	private Panel panel11;

	private Label label50;

	private TabPage tabPage2;

	internal GridEX grdExpenseHistory;

	private Panel panel12;

	private Label label57;

	private TabPage tabPage3;

	private Panel panel23;

	internal GridEX grdEmptyBagHistory;

	private Panel panel16;

	private Label label64;

	private Label label58;

	private ToolStripButton BtnPrintLoadingChallan;

	internal UltraCombo CmbDeliveryToParty;

	private Label label52;

	private Label label11;

	private TextBox txtWarningRemarks;

	private Label label53;

	internal UltraCombo CmbDeliverToAddressHistory;

	internal UltraCombo CmbDeliveryToPartyHistory;

	private Label label59;

	private Label label61;

	private Label label62;

	private TextBox txtAvgFillingPerBag;

	public frmGrnLoadingChallanCmagt(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
		InitializeComponentMethod();
		FormHelper.FormEventHandler(this);
	}

	private void frmGrnLoadingChallanCmagt_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1150, 750);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeComponentCustom()
	{
		try
		{
			ScreenName = Conversion.ToString((object)base.Name);
			dtCrop.Columns.Add("Id", typeof(int));
			dtCrop.Columns.Add("CropYear");
			dtPackType.Columns.Add("Id", typeof(int));
			dtPackType.Columns.Add("PackTypeDesc");
			GrnLoadingChallan_Helper.InitializeDetailtable(dtDetail);
			GrnLoadingChallan_Helper.InitializeExpenseDetailtable(dtExpGrid);
			GrnLoadingChallan_Helper.InitializeEmptyBagDetailtable(dtEmptyBags);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeComponentMethod()
	{
		Task.Run(delegate
		{
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			dtViewsData = AllComboServices_Views();
			DataTable historyComboData = HistoryComboDbCall();
			string documentNoData = DocumentNoDbCall();
			DataTable companyData = CompanyDropdownData();
			DataTable branchsAllocatedToUser = BranchesAllocationToUser.GetBranchsAllocatedToUser(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID);
			DataTable termData = DeliveryTerm.FormHistory();
			dtShipToAddress = DatatableHelper.AllShipToAddressDbCall(UserAccount);
			OtherItemsdtFillDbCall();
			OutstandingOrdersdtFillDbCall();
			dtItemForEmptyBag = CommonServices.GetPackingMaterialItemsAllocateToFlow();
			return new
			{
				HistoryComboData = historyComboData,
				DocumentNoData = documentNoData,
				CompanyData = companyData,
				BranchData = branchsAllocatedToUser,
				termData = termData
			};
		}).ContinueWith(t =>
		{
			if (!base.IsDisposed)
			{
				if (t.IsFaulted)
				{
					MessageBox.Show("Error occurred during database call.");
				}
				else
				{
					var result = t.Result;
					btnsave.Enabled = formright.DoHaveSaveRight;
					btnUpdate.Enabled = formright.DoHaveUpdateRights;
					btnDelete.Enabled = formright.DoHaveCanDelete;
					btnPrint.Enabled = formright.DoHavePrintRights;
					GetConfigurationsFromGlobal();
					if (RecId == 0)
					{
						txtDocNo.Text = result.DocumentNoData;
					}
					CompanyBind(result.CompanyData);
					BranchesBind(result.BranchData);
					CmbCompanyName.Value = UserAccount.CompanyId;
					CommonBindings.SupplierDtFillFromGlobal(dtSupplier, useBusinessName: true);
					SupplierBind();
					CityBindFromGlobal();
					DeliveryTermBind(result.termData);
					vehicleTypefill();
					OrderNoBind();
					ParentCategoryBindFromGlobal();
					ItemdtFillFromGlobal();
					ItemNameBind();
					CropDtFillFromGlobalAndBind();
					PackingTypeDtFillFromGlobalAndBind();
					BindViewCombos();
					BindGrids();
					GetCommissionAgentConfigurationsFromGlobalandBind();
					HistoryComboBind(result.HistoryComboData);
					FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
					ToDateHistory.Value = DateTime.Now;
					FormInitialized = true;
					datDocDate.Focus();
				}
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private DataTable HistoryComboDbCall()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtHistoryCombo.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtHistoryCombo = grnSupplierLoadingMaster.GetDataForDropDown(obj);
			return dtHistoryCombo;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GetConfigurationsFromGlobal()
	{
		try
		{
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
			TolerancePercentageForCmagtPoWeight = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("TolerancePercentageForCmagtPoWeight"));
			GrnLoadingAllowDifferentBiltyDate = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("GrnLoadingAllowDifferentBiltyDate"));
			GrnLoadingDocDateAndBiltyDateSame = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("GrnLoadingDocDateAndBiltyDateSame"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetCommissionAgentConfigurationsFromGlobalandBind()
	{
		try
		{
			int commissionAgentId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultCommissionAgentIdForCommissionAgentPortal"));
			if (commissionAgentId > 0)
			{
				CmbCommissionAgent.Value = commissionAgentId;
			}
			int deliveryTermId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDeliveryTermIdForCommissionAgentPortal"));
			if (deliveryTermId > 0)
			{
				CmbDeliveryTerm.Value = deliveryTermId;
			}
			int cropYearId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultCropYearIdForCommissionAgentPortal"));
			if (cropYearId > 0)
			{
				CmbCropYear.Value = cropYearId;
			}
			int packingTypeId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultPackingTypeIdForCommissionAgentPortal"));
			if (packingTypeId > 0)
			{
				CmbPackingType.Value = packingTypeId;
			}
			int loadingCityId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultLoadingCityIdForCommissionAgentPortal"));
			if (loadingCityId > 0)
			{
				CmbLoadingCity.Value = loadingCityId;
			}
			int unloadingCityId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultUnloadingCityIdForCommissionAgentPortal"));
			if (unloadingCityId > 0)
			{
				CmbUnloadingCity.Value = unloadingCityId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable CompanyDropdownData()
	{
		try
		{
			return CommonServices.CompanyServiceBind();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CompanyBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbCompanyName, dt, "Id", "CompName", "Company Name", AllColumns: false, Conversion.ToInt(CmbCompanyName.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchesBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbBranch, dt, "BranchId", "BranchName", "Branch Name", AllColumns: true, Conversion.ToInt(CmbBranch.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void vehicleTypefill()
	{
		try
		{
			Conversion.ToInt(cmbVehicleType.Value);
			DataTable dt = VehicleType.GetAll();
			InfragisticsHelper.BindAndRetainSelection(cmbVehicleType, dt, "Id", "VehicleDescription", "Vehicle Type", AllColumns: true, Conversion.ToInt(cmbVehicleType.Value));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private string DocumentNoDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			return grnSupplierLoadingMaster.GenerateCode(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = DocumentTypeId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			}).ToString();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void SupplierBind()
	{
		CommonBindings.SupplierBind(CmbCommissionAgent, dtSupplier, "Commission Agent");
		CommonBindings.SupplierBind(CmbSupplierName, dtSupplier, "Supplier Name");
		CommonBindings.SupplierBind(CmbBuyerNameDetail, dtSupplier, "Buyer Name");
		CommonBindings.SupplierBind(CmbDeliveryToParty, dtSupplier, "DeliveryToParty Name");
		CommonBindings.SupplierBind(CmbTransporter, dtSupplier, "Transporter");
	}

	private void CityBindFromGlobal()
	{
		try
		{
			DataTable dt = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalAllCities, (GlobalAllCity x) => x.Id, (GlobalAllCity x) => x.CityName);
			InfragisticsHelper.BindAndRetainSelection(CmbLoadingCity, dt, "Id", "Description", "Loading City", AllColumns: true, Conversion.ToInt(CmbLoadingCity.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbUnloadingCity, dt, "Id", "Description", "Un-Loading City", AllColumns: true, Conversion.ToInt(CmbUnloadingCity.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveryTermBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbDeliveryTerm, dt, "Id", "Description", "Delivery Term", AllColumns: false, Conversion.ToInt(CmbDeliveryTerm.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSupplierName_Leave(object sender, EventArgs e)
	{
		try
		{
			OrderNoBind();
			if (Conversion.ToInt(CmbLoadingCity.Value) == 0 && Conversion.ToInt(CmbSupplierName.Value) > 0)
			{
				UltraCombo cmbLoadingCity = CmbLoadingCity;
				UltraCombo cmbSupplierName = CmbSupplierName;
				object obj;
				if (cmbSupplierName == null)
				{
					obj = null;
				}
				else
				{
					UltraGridRow selectedRow = ((UltraDropDownBase)cmbSupplierName).SelectedRow;
					obj = ((selectedRow != null) ? selectedRow.Cells["CityName"].Value : null);
				}
				((Control)(object)cmbLoadingCity).Text = Conversion.ToString(obj);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable AllComboServices_Views()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return purchaseOrderMaster.AllComboServices_FromViews(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindViewCombos()
	{
		try
		{
			dtEbGridPackingTypes.Rows.Clear();
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Name", typeof(string));
			dtEbGridPackingTypes = dt.Clone();
			for (int i = 0; i < dtViewsData.Rows.Count; i++)
			{
				string Activity = Conversion.ToString(dtViewsData.Rows[i]["Activity"]);
				if (Activity == "AllocatedPackingType")
				{
					dtEbGridPackingTypes.Rows.Add(dtViewsData.Rows[i]["Id"], dtViewsData.Rows[i]["ReferenceName"]);
				}
				else if (Activity == "EmptyBagTypes")
				{
					dt.Rows.Add(dtViewsData.Rows[i]["Id"], dtViewsData.Rows[i]["ReferenceName"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbEmptyBagTerm, dt, "Id", "Name", "Empty Bag term", AllColumns: true, Conversion.ToInt(CmbEmptyBagTerm.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void OutstandingOrdersdtFillDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		try
		{
			DataSet ds = purchaseOrderMaster.PendingDataLoaderForGrn(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 1052,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = RecId
			});
			dtPendingOrdersData = ds?.Tables?[0].Copy();
			dtSaleOrdersPurchaseOrderWise = ds?.Tables?[1].Copy();
			dtEmptyBagDataOrderWise = ds?.Tables?[2].Copy();
			dtExpenseDataOrderWise = ds?.Tables?[3].Copy();
			dtEmptyBagCutDataOrderWise = ds?.Tables?[4].Copy();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void OrderNoBind()
	{
		try
		{
			int supplierId = Conversion.ToInt(CmbSupplierName.Value);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("DocNo", typeof(string));
			dt.Columns.Add("EBWeightDeductionTermId", typeof(int));
			dt.Columns.Add("supplierId", typeof(int));
			dt.Columns.Add("CommissionAgentId", typeof(int));
			dt.Columns.Add("DeliveryStartDate", typeof(DateTime));
			dt.Columns.Add("ExpiryDate", typeof(DateTime));
			foreach (DataRow row in from r in dtPendingOrdersData.AsEnumerable()
				where supplierId <= 0 || Conversion.ToInt(r["supplierId"]) == supplierId
				group r by Conversion.ToInt(r["purchaseOrderMasterId"]) into g
				select g.First())
			{
				dt.Rows.Add(row["purchaseOrderMasterId"], row["DocNo"], row["EBWeightDeductionTermId"], row["supplierId"], row["commissionAgentId"], row["DeliveryStartDate"], row["ValidityDate"]);
			}
			InfragisticsHelper.BindAndRetainSelection(CmbPoNoDetail, dt, "Id", "DocNo", "Order No", AllColumns: true, Conversion.ToInt(CmbPoNoDetail.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbPoNoDetail).DisplayLayout.Bands[0].Columns[1].Width = 80;
				((UltraGridBase)CmbPoNoDetail).DisplayLayout.Bands[0].Columns[5].Width = 90;
				((UltraGridBase)CmbPoNoDetail).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbPoNoDetail).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbPoNoDetail).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ParentCategoryBindFromGlobal()
	{
		try
		{
			List<getGlobalAllItems> allItems = clsGlobalVariables.getGlobalAllItems;
			if (!allItems.Any())
			{
				return;
			}
			DataTable dtParentCategory = new DataTable();
			dtParentCategory.Columns.Add("Id", typeof(int));
			dtParentCategory.Columns.Add("Description", typeof(string));
			HashSet<string> parentSet = new HashSet<string>();
			foreach (getGlobalAllItems item in allItems)
			{
				if (!string.IsNullOrEmpty(item.InvParentCateDescription) && parentSet.Add(item.InvParentCateDescription))
				{
					dtParentCategory.Rows.Add(item.InventoryParentCategoriesId, item.InvParentCateDescription);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbParentItem, dtParentCategory, "Id", "Description", "Parent Item", AllColumns: true, Conversion.ToInt(CmbParentItem.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error loading categories: " + ex.Message);
		}
	}

	private void ItemdtFillAgainstOder()
	{
		try
		{
			dtItem.Rows.Clear();
			DataTable dataTable = dtItem;
			if (dataTable != null && dataTable.Columns.Count == 0)
			{
				dtItem.Columns.Add("MappingId", typeof(int));
				dtItem.Columns.Add("ItemName", typeof(string));
				dtItem.Columns.Add("ItemCode", typeof(string));
				dtItem.Columns.Add("ItemId", typeof(int));
				dtItem.Columns.Add("ItemQty", typeof(decimal));
				dtItem.Columns.Add("ReceivedQty", typeof(decimal));
				dtItem.Columns.Add("BalanceQty", typeof(decimal));
				dtItem.Columns.Add("ItemWeight", typeof(decimal));
				dtItem.Columns.Add("ReceivedWeight", typeof(decimal));
				dtItem.Columns.Add("BalanceWeight", typeof(decimal));
				dtItem.Columns.Add("ItemRate", typeof(decimal));
				dtItem.Columns.Add("ParentCategoryId", typeof(int));
				dtItem.Columns.Add("cropYearId", typeof(int));
				dtItem.Columns.Add("packingTypeId", typeof(int));
				dtItem.Columns.Add("packUomId", typeof(int));
				dtItem.Columns.Add("EbUnit", typeof(decimal));
				dtItem.Columns.Add("OrderDetailId", typeof(int));
				dtItem.Columns.Add("SoId", typeof(int));
				dtItem.Columns.Add("SoDetailId", typeof(int));
				dtItem.Columns.Add("SoNo", typeof(int));
				dtItem.Columns.Add("BuyerId", typeof(int));
				dtItem.Columns.Add("DeliveryToPartyId", typeof(int));
				dtItem.Columns.Add("ShipToAddressId", typeof(int));
				dtItem.Columns.Add("ShipToAddress", typeof(string));
			}
			if (((UltraGridBase)CmbPoNoDetail).ActiveRow == null || Conversion.ToInt(CmbPoNoDetail.Value) <= 0)
			{
				return;
			}
			int orderId = Conversion.ToInt(CmbPoNoDetail.Value);
			foreach (DataRow r2 in from r in dtPendingOrdersData.AsEnumerable()
				where Conversion.ToInt(r["purchaseOrderMasterId"]) == orderId
				select r)
			{
				dtItem.Rows.Add(Conversion.ToInt(r2["purchaseOrderSaleOrderMappingId"]), Conversion.ToString(r2["ItemName"]), Conversion.ToString(r2["ItemCode"]), Conversion.ToInt(r2["itemId"]), Conversion.ToDecimal(r2["itemQty"]), Conversion.ToDecimal(r2["UsedQty"]), Conversion.ToDecimal(r2["BalanceQty"]), Conversion.ToDecimal(r2["itemWeight"]), Conversion.ToDecimal(r2["UsedWeight"]), Conversion.ToDecimal(r2["BalanceWeight"]), Conversion.ToDecimal(r2["itemRate"]), Conversion.ToInt(r2["inventoryParentCategoryId"]), Conversion.ToInt(r2["cropYearId"]), Conversion.ToInt(r2["packingTypeId"]), Conversion.ToInt(r2["packUomId"]), Conversion.ToDecimal(r2["EbUnit"]), Conversion.ToInt(r2["DetailId"]), Conversion.ToInt(r2["saleOrderMasterId"]), Conversion.ToInt(r2["saleOrderDetailId"]), Conversion.ToInt(r2["SaleOrderNo"]), Conversion.ToInt(r2["buyerId"]), Conversion.ToInt(r2["DeliveryToPartyId"]), Conversion.ToInt(r2["ShipToAddressId"]), Conversion.ToString(r2["ShipToAddress"]));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemdtFillFromGlobal(int ParentCategoryId = 0)
	{
		try
		{
			DataTable dataTable = dtItem;
			if (dataTable != null && dataTable.Columns.Count == 0)
			{
				dtItem.Columns.Add("MappingId", typeof(int));
				dtItem.Columns.Add("ItemName", typeof(string));
				dtItem.Columns.Add("ItemCode", typeof(string));
				dtItem.Columns.Add("ItemId", typeof(int));
				dtItem.Columns.Add("ItemQty", typeof(decimal));
				dtItem.Columns.Add("ReceivedQty", typeof(decimal));
				dtItem.Columns.Add("BalanceQty", typeof(decimal));
				dtItem.Columns.Add("ItemWeight", typeof(decimal));
				dtItem.Columns.Add("ReceivedWeight", typeof(decimal));
				dtItem.Columns.Add("BalanceWeight", typeof(decimal));
				dtItem.Columns.Add("ItemRate", typeof(decimal));
				dtItem.Columns.Add("ParentCategoryId", typeof(int));
				dtItem.Columns.Add("cropYearId", typeof(int));
				dtItem.Columns.Add("packingTypeId", typeof(int));
				dtItem.Columns.Add("packUomId", typeof(int));
				dtItem.Columns.Add("EbUnit", typeof(decimal));
				dtItem.Columns.Add("OrderDetailId", typeof(int));
				dtItem.Columns.Add("SoId", typeof(int));
				dtItem.Columns.Add("SoDetailId", typeof(int));
				dtItem.Columns.Add("SoNo", typeof(int));
				dtItem.Columns.Add("BuyerId", typeof(int));
				dtItem.Columns.Add("DeliveryToPartyId", typeof(int));
				dtItem.Columns.Add("ShipToAddressId", typeof(int));
				dtItem.Columns.Add("ShipToAddress", typeof(string));
			}
			dtItem.Rows.Clear();
			List<getGlobalAllItems> dtItemAll = clsGlobalVariables.getGlobalAllItems.Where((getGlobalAllItems row) => ParentCategoryId == 0 || row.InventoryParentCategoriesId == ParentCategoryId).ToList();
			if (dtItemAll == null || dtItemAll.Count == 0)
			{
				return;
			}
			foreach (getGlobalAllItems row2 in dtItemAll)
			{
				dtItem.Rows.Add(row2.Id, row2.ItemName, row2.ItemCode, row2.Id, 0, 0, 0, 0, 0, 0, 0, row2.InventoryParentCategoriesId, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemNameBind(bool True = false)
	{
		try
		{
			int Id = 0;
			if (((UltraGridBase)CmbItemName).ActiveRow != null && Conversion.ToInt(CmbItemName.Value) > 0)
			{
				Id = Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["MappingId"].Value);
			}
			string ValueMember = (RadItemNamePackListDetail.Checked ? "ItemName" : "ItemCode");
			string DisplayMember = (RadItemNamePackListDetail.Checked ? "Item Name" : "Item Code");
			InfragisticsHelper.BindAndRetainSelection(CmbItemName, dtItem, "MappingId", ValueMember, DisplayMember, AllColumns: true, Id, insertDefaultRow: true, null, null, True, 1);
			if (dtItem.Rows.Count > 0)
			{
				if (((UltraGridBase)CmbPoNoDetail).ActiveRow != null && Conversion.ToInt(CmbPoNoDetail.Value) != 0)
				{
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[3].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[4].Hidden = false;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[5].Hidden = false;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[6].Hidden = false;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[7].Hidden = false;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[8].Hidden = false;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[9].Hidden = false;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[10].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[11].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[12].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[13].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[14].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[15].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[16].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[17].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[18].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[20].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[21].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[22].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[23].Hidden = true;
				}
				else
				{
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[3].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[4].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[5].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[6].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[7].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[8].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[9].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[10].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[11].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[12].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[13].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[14].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[15].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[16].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[17].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[18].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[19].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[20].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[21].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[22].Hidden = true;
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[23].Hidden = true;
				}
				if (True)
				{
					CmbItemName_Leave(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CropDtFillFromGlobalAndBind()
	{
		try
		{
			List<GlobalCropYear> lst = clsGlobalVariables.globalCropYear;
			DatatableHelper.PopulateDataTable(dtCrop, lst, (GlobalCropYear x) => x.Id, (GlobalCropYear x) => x.CropYear);
			int Id = Conversion.ToInt(CmbCropYear.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbCropYear, dtCrop, "Id", "CropYear", "Crop Year", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PackingTypeDtFillFromGlobalAndBind()
	{
		try
		{
			List<InvPackingType> Finallist = clsGlobalVariables.globalInvPackingType;
			DatatableHelper.PopulateDataTable(dtPackType, Finallist, (InvPackingType x) => x.Id, (InvPackingType x) => x.PackTypeDesc);
			int Id = Conversion.ToInt(CmbPackingType.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbPackingType, dtPackType, "Id", "PackTypeDesc", "Packing Type", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GetEmptyBagDetailOnBaseOfPO()
	{
		try
		{
			if (!dtEmptyBags.AsEnumerable().Any((DataRow r) => Conversion.ToDecimal(r["PurchaseRate"]) > 0m))
			{
				dtEmptyBags.Rows.Clear();
			}
			int PurchaseOrderId = Conversion.ToInt(CmbPoNoDetail.Value);
			HashSet<(int, int)> existingKeys = (from r in dtEmptyBags.AsEnumerable()
				select (PoId: Conversion.ToInt(r["PoId"]), PoEbId: Conversion.ToInt(r["PoEbId"]))).ToHashSet();
			foreach (DataRow row in from r in dtEmptyBagDataOrderWise.AsEnumerable()
				where Conversion.ToInt(r["purchaseOrderMasterId"]) == PurchaseOrderId
				select r)
			{
				int poId = Conversion.ToInt(row["purchaseOrderMasterId"]);
				int poEbId = Conversion.ToInt(row["purchaseOrderEmptyBagDetailId"]);
				if (!existingKeys.Contains((poId, poEbId)))
				{
					dtEmptyBags.Rows.Add(0, row["purchaseOrderMasterId"], row["purchaseOrderEmptyBagDetailId"], row["PurchaseOrderNo"], row["PackingTypeId"], row["Rate"], row["emptyBagPackingMaterialItemId"]);
					existingKeys.Add((poId, poEbId));
				}
			}
			grdEmptyBags.DataSource = dtEmptyBags;
			grdEmptyBags.RetrieveStructure();
			gridEmptyBagsSettings(grdEmptyBags);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetExpensesOnBaseOfPO()
	{
		try
		{
			if (!dtExpGrid.AsEnumerable().Any((DataRow r) => Conversion.ToDecimal(r["Amount"]) > 0m))
			{
				dtExpGrid.Rows.Clear();
			}
			int PurchaseOrderId = Conversion.ToInt(CmbPoNoDetail.Value);
			EnumerableRowCollection<DataRow> enumerableRowCollection = from r in dtExpenseDataOrderWise.AsEnumerable()
				where Conversion.ToInt(r["purchaseOrderMasterId"]) == PurchaseOrderId
				select r;
			HashSet<(int, int)> existingKeys = (from r in dtExpGrid.AsEnumerable()
				select (PoId: Conversion.ToInt(r["PoId"]), PoExpenseId: Conversion.ToInt(r["PoExpenseId"]))).ToHashSet();
			foreach (DataRow row in enumerableRowCollection)
			{
				int poId = Conversion.ToInt(row["purchaseOrderMasterId"]);
				int poExpenseId = Conversion.ToInt(row["purchaseOrderSupplierExpenseDetailId"]);
				if (!existingKeys.Contains((poId, poExpenseId)))
				{
					dtExpGrid.Rows.Add(0, poId, poExpenseId, row["PurchaseOrderNo"], row["ItemId"], row["Qty"], row["rate"], row["amount"], row["remarks"]);
					existingKeys.Add((poId, poExpenseId));
				}
			}
			grdInvExp.DataSource = dtExpGrid;
			grdInvExp.RetrieveStructure();
			grdInvExpSettings(grdInvExp);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RemoveInvalidRows(DataTable table, HashSet<int> validPOIds)
	{
		if (table == null || table.Rows.Count == 0)
		{
			return;
		}
		for (int i = table.Rows.Count - 1; i >= 0; i--)
		{
			int poId = Conversion.ToInt(table.Rows[i]["PoId"]);
			if (poId > 0 && !validPOIds.Contains(poId))
			{
				table.Rows.RemoveAt(i);
			}
		}
	}

	private void CheckPurchaseOrderInDetailAndExpenses()
	{
		try
		{
			HashSet<int> validPOIds = new HashSet<int>();
			if (dtDetail != null && dtDetail.Rows.Count > 0)
			{
				validPOIds = (from r in dtDetail.AsEnumerable()
					select Conversion.ToInt(r["PurchaseOrderId"])).ToHashSet();
			}
			RemoveInvalidRows(dtExpGrid, validPOIds);
			RemoveInvalidRows(dtEmptyBags, validPOIds);
			if (dtExpGrid.Rows.Count == 0)
			{
				AddRowsInExpenseGrid();
			}
			if (dtEmptyBags.Rows.Count == 0)
			{
				AddRowInvEmptyBagsGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPoNoDetail_Leave(object sender, EventArgs e)
	{
		if (((UltraGridBase)CmbPoNoDetail).ActiveRow != null && Conversion.ToInt(CmbPoNoDetail.Value) > 0)
		{
			((Control)(object)CmbParentItem).Enabled = false;
			((Control)(object)CmbEmptyBagTerm).Enabled = false;
			CmbEmptyBagTerm.Value = Conversion.ToInt(((UltraDropDownBase)CmbPoNoDetail).SelectedRow.Cells["EBWeightDeductionTermId"].Value);
			if (Conversion.ToInt(CmbSupplierName.Value) == 0)
			{
				CmbSupplierName.Value = Conversion.ToInt(((UltraDropDownBase)CmbPoNoDetail).SelectedRow.Cells["supplierId"].Value);
				CmbSupplierName_Leave(null, null);
			}
			if (Conversion.ToInt(CmbCommissionAgent.Value) == 0)
			{
				CmbCommissionAgent.Value = Conversion.ToInt(((UltraDropDownBase)CmbPoNoDetail).SelectedRow.Cells["CommissionAgentId"].Value);
			}
			((Control)(object)CmbBuyerNameDetail).Enabled = false;
			ItemdtFillAgainstOder();
		}
		else
		{
			((Control)(object)CmbParentItem).Enabled = true;
			((Control)(object)CmbEmptyBagTerm).Enabled = true;
			((Control)(object)CmbBuyerNameDetail).Enabled = true;
			ItemdtFillFromGlobal(Conversion.ToInt(CmbParentItem.Value));
			txtSaleOrderNo.Text = "";
		}
		ItemNameBind(True: true);
	}

	private void cmbParentItem_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbPoNoDetail).ActiveRow != null && Conversion.ToInt(CmbPoNoDetail.Value) > 0)
			{
				ItemdtFillAgainstOder();
				CmbBuyerNameDetail_Leave(null, null);
			}
			else
			{
				ItemdtFillFromGlobal(Conversion.ToInt(CmbParentItem.Value));
			}
			ItemNameBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbItemName_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbItemName).ActiveRow == null || ((UltraDropDownBase)CmbItemName).SelectedRow == null)
			{
				return;
			}
			UltraGridRow row = ((UltraDropDownBase)CmbItemName).SelectedRow;
			int itemId = Conversion.ToInt(row.Cells["ItemId"].Value);
			if (itemId > 0 && clsGlobalVariables.getGlobalAllItems != null)
			{
				getGlobalAllItems item = clsGlobalVariables.getGlobalAllItems.FirstOrDefault((getGlobalAllItems x) => x.Id == itemId);
				if (item != null)
				{
					CmbParentItem.Value = item.InventoryParentCategoriesId;
				}
			}
			CommonBindings.ItemUomFromGlobalBind(itemId, CmbPackUom);
			if (Conversion.ToInt(CmbPoNoDetail.Value) <= 0)
			{
				return;
			}
			CmbParentItem.Value = Conversion.ToInt(row.Cells["ParentCategoryId"].Value);
			txtPoTotalWeightDetail.Text = Conversion.ToDecimal(row.Cells["ItemWeight"].Value).ToString("#,##0.###");
			txtPoAlreadyReceivedWeightDetail.Text = Conversion.ToDecimal(row.Cells["ReceivedWeight"].Value).ToString("#,##0.###");
			txtPoBalWeightDetail.Text = Conversion.ToDecimal(row.Cells["BalanceWeight"].Value).ToString("#,##0.###");
			txtSaleOrderNo.Text = Conversion.ToInt(row.Cells["SoNo"].Value).ToString();
			CmbBuyerNameDetail.Value = Conversion.ToInt(row.Cells["BuyerId"].Value);
			if (((UltraGridBase)CmbDeliveryToParty).DataSource is DataTable dtDelivery)
			{
				int deliveryPartyId = Conversion.ToInt(row.Cells["DeliveryToPartyId"].Value);
				SafeSetComboValue(CmbDeliveryToParty, dtDelivery, deliveryPartyId);
			}
			CmbDeliveryToParty_Leave(null, null);
			if (Conversion.ToInt(row.Cells["ShipToAddressId"].Value) == 0)
			{
				txtDeliveryToAddress.Text = Conversion.ToString(row.Cells["ShipToAddress"].Value);
			}
			else if (((UltraGridBase)CmbShipToAddress).DataSource is DataTable dtShip)
			{
				int shipToId = Conversion.ToInt(row.Cells["ShipToAddressId"].Value);
				SafeSetComboValue(CmbShipToAddress, dtShip, shipToId);
			}
			decimal balQty = Conversion.ToDecimal(row.Cells["BalanceQty"].Value);
			decimal balWeight = Conversion.ToDecimal(row.Cells["BalanceWeight"].Value);
			decimal loadingQty = Conversion.ToDecimal((object)txtLoadingQty.Text);
			if (loadingQty == 0m || loadingQty > balQty)
			{
				txtLoadingQty.Text = balQty.ToString("#,##0.###");
			}
			decimal grossWeight = Conversion.ToDecimal((object)txtGrossWeight.Text);
			if (grossWeight == 0m || grossWeight > balWeight)
			{
				txtGrossWeight.Text = balWeight.ToString("#,##0.###");
			}
			if (updateDetailIndex == -1)
			{
				if (((UltraGridBase)CmbPackUom).DataSource is DataTable dtUom && dtUom.Rows.Count > 0)
				{
					CmbPackUom.Value = Conversion.ToInt(row.Cells["packUomId"].Value);
				}
				if (((UltraGridBase)CmbCropYear).DataSource is DataTable dtCrop && dtCrop.Rows.Count > 0)
				{
					CmbCropYear.Value = Conversion.ToInt(row.Cells["cropYearId"].Value);
				}
				if (((UltraGridBase)CmbPackingType).DataSource is DataTable dtPack && dtPack.Rows.Count > 0)
				{
					CmbPackingType.Value = Conversion.ToInt(row.Cells["packingTypeId"].Value);
					CmbPackingType_Leave(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SafeSetComboValue(UltraCombo combo, DataTable dt, int id)
	{
		if (dt == null || dt.Rows.Count == 0)
		{
			combo.Value = null;
		}
		else if (dt.AsEnumerable().Any((DataRow r) => Conversion.ToInt(r[((UltraDropDownBase)combo).ValueMember]) == id))
		{
			combo.Value = id;
		}
	}

	private void CmbPackingType_Leave(object sender, EventArgs e)
	{
		try
		{
			decimal EbUnit = default(decimal);
			int PtypeId = Conversion.ToInt(CmbPackingType.Value);
			int PoId = Conversion.ToInt(CmbPoNoDetail.Value);
			EbUnit = (from x in dtEmptyBagCutDataOrderWise.AsEnumerable()
				where x.Field<int>("PurchaseOrderMasterId") == PoId && x.Field<int>("PackingTypeId") == PtypeId
				select x.Field<decimal>("weightCutKg")).FirstOrDefault();
			txtEbUnit.Text = EbUnit.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbBuyerNameDetail_Leave(object sender, EventArgs e)
	{
		try
		{
			int buyerId = Conversion.ToInt(CmbBuyerNameDetail.Value);
			if (Conversion.ToInt(CmbUnloadingCity.Value) == 0 && buyerId > 0)
			{
				UltraCombo cmbUnloadingCity = CmbUnloadingCity;
				UltraCombo cmbBuyerNameDetail = CmbBuyerNameDetail;
				object obj;
				if (cmbBuyerNameDetail == null)
				{
					obj = null;
				}
				else
				{
					UltraGridRow selectedRow = ((UltraDropDownBase)cmbBuyerNameDetail).SelectedRow;
					obj = ((selectedRow != null) ? selectedRow.Cells["CityName"].Value : null);
				}
				((Control)(object)cmbUnloadingCity).Text = Conversion.ToString(obj);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDeliveryToParty_Leave(object sender, EventArgs e)
	{
		try
		{
			int buyerId = Conversion.ToInt(CmbDeliveryToParty.Value);
			CommonBindings.BindShipToAddressAgainstBuyer(CmbShipToAddress, dtShipToAddress, buyerId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RadItemNamePackListDetail_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			ItemNameBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OtherItemsdtFillDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InventoryItemsOther obj = new InventoryItemsOther();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtOtherItem = InventoryItemsOther.GetAll(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtvehicleno_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (_internalChange || char.IsControl(e.KeyChar))
		{
			return;
		}
		if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != '-')
		{
			e.Handled = true;
			return;
		}
		TextBox tb = (TextBox)sender;
		string text = tb.Text;
		int selectionStart = tb.SelectionStart;
		if (e.KeyChar == '-' && text.Contains("-"))
		{
			e.Handled = true;
			return;
		}
		string futureText = text.Substring(0, selectionStart) + e.KeyChar + text.Substring(selectionStart);
		int dashIndex = futureText.IndexOf('-');
		string lettersPart = ((dashIndex >= 0) ? futureText.Substring(0, dashIndex) : futureText);
		string digitsPart = ((dashIndex >= 0 && dashIndex < futureText.Length - 1) ? futureText.Substring(dashIndex + 1) : "");
		if (char.IsLetter(e.KeyChar) && lettersPart.Length > 6)
		{
			e.Handled = true;
		}
		else if (char.IsDigit(e.KeyChar) && dashIndex >= 0 && digitsPart.Length > 6)
		{
			e.Handled = true;
		}
		else if (char.IsDigit(e.KeyChar) && !text.Contains("-"))
		{
			int letterCount = text.TakeWhile(char.IsLetter).Count();
			if (letterCount >= 1 && letterCount <= 6)
			{
				_internalChange = true;
				tb.Text = text.Insert(letterCount, "-");
				tb.SelectionStart = selectionStart + 1;
				_internalChange = false;
			}
		}
	}

	private void txtvehicleno_Leave(object sender, EventArgs e)
	{
		if (_internalChange || string.IsNullOrWhiteSpace(txtVehicleNo.Text))
		{
			return;
		}
		string text = txtVehicleNo.Text.Trim().ToUpper();
		if (!text.Contains("-"))
		{
			int letterCount = text.TakeWhile(char.IsLetter).Count();
			if (letterCount >= 1 && letterCount <= 6)
			{
				text = text.Insert(letterCount, "-");
			}
		}
		_internalChange = true;
		txtVehicleNo.Text = text;
		_internalChange = false;
	}

	private void txtvehicleno_KeyDown(object sender, KeyEventArgs e)
	{
		if (!_internalChange && e.KeyCode == Keys.Back)
		{
			TextBox tb = (TextBox)sender;
			int pos = tb.SelectionStart;
			if (pos > 0 && pos <= tb.Text.Length && tb.Text[pos - 1] == '-')
			{
				e.Handled = true;
				e.SuppressKeyPress = true;
				_internalChange = true;
				string newText = tb.Text.Remove(pos - 1, 1);
				tb.Text = newText;
				tb.SelectionStart = pos - 1;
				tb.SelectionLength = 0;
				_internalChange = false;
			}
		}
	}

	private void txtvehicleno_KeyUp(object sender, KeyEventArgs e)
	{
		if (e.Control && e.KeyCode == Keys.V)
		{
			string text = txtVehicleNo.Text.Trim().ToUpper();
			text = Regex.Replace(text, "[^A-Z0-9]", "");
			int letterCount = 0;
			string text2 = text;
			for (int i = 0; i < text2.Length && char.IsLetter(text2[i]); i++)
			{
				letterCount++;
			}
			if (letterCount >= 1 && letterCount <= 6)
			{
				text = text.Insert(letterCount, "-");
			}
			txtVehicleNo.Text = text;
			txtVehicleNo.SelectionStart = txtVehicleNo.Text.Length;
		}
	}

	private void txtVehicleNo_TextChanged(object sender, EventArgs e)
	{
		if (!_internalChange)
		{
			TextBox obj = (TextBox)sender;
			int pos = obj.SelectionStart;
			_internalChange = true;
			obj.Text = obj.Text.ToUpper();
			_internalChange = false;
			obj.SelectionStart = pos;
		}
	}

	private void CmbShipToAddress_Leave(object sender, EventArgs e)
	{
		try
		{
			int DeliveryToPartyId = 0;
			int num = Conversion.ToInt(CmbShipToAddress.Value);
			UltraGridRow Row = ((UltraDropDownBase)CmbShipToAddress).SelectedRow;
			if (num > 0 && Row != null)
			{
				DeliveryToPartyId = Conversion.ToInt(Row.Cells["PartyId"].Value);
			}
			if (DeliveryToPartyId > 0)
			{
				CmbDeliveryToParty.Value = DeliveryToPartyId;
			}
			if (Conversion.ToInt(CmbShipToAddress.Value) > 0 && string.IsNullOrEmpty(Conversion.ToString(txtDeliveryToAddress.Tag)))
			{
				txtDeliveryToAddress.Text = ((Control)(object)CmbShipToAddress).Text;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDeliveryToAddress_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			txtDeliveryToAddress.Tag = txtDeliveryToAddress.Text;
			((Control)(object)CmbShipToAddress).Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbTransporter_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbTransporter.Value) > 0 && string.IsNullOrEmpty(Conversion.ToString(txtTransporterName.Tag)))
			{
				txtTransporterName.Text = ((Control)(object)CmbTransporter).Text;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtTransporterName_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			txtTransporterName.Tag = txtTransporterName.Text;
			((Control)(object)CmbTransporter).Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidationDetail()
	{
		if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
		{
			((Control)(object)CmbParentItem, "Parent Item", FormHelper.ValidType.INT),
			((Control)(object)CmbItemName, "Item Name", FormHelper.ValidType.INT),
			((Control)(object)CmbCropYear, "Crop Year", FormHelper.ValidType.INT),
			((Control)(object)CmbPackingType, "Packing Type", FormHelper.ValidType.INT),
			((Control)(object)CmbPackUom, "Pack Uom", FormHelper.ValidType.INT),
			(txtLoadingQty, "Qty", FormHelper.ValidType.Double),
			(txtGrossWeight, "Weight", FormHelper.ValidType.Double),
			((Control)(object)CmbEmptyBagTerm, "Empty Bag Term", FormHelper.ValidType.INT)
		}))
		{
			return false;
		}
		if (((UltraGridBase)CmbPoNoDetail).ActiveRow != null && Conversion.ToInt(CmbPoNoDetail.Value) != 0)
		{
			if (Conversion.ToDateTime(((UltraDropDownBase)CmbPoNoDetail).SelectedRow.Cells["ExpiryDate"].Value).Date < datDocDate.Value.Date)
			{
				MessageBox.Show(string.Format("Selected Purchase Order is Expired On: {0}", Conversion.ToDateTime(((UltraDropDownBase)CmbPoNoDetail).SelectedRow.Cells["ExpiryDate"].Value).Date));
				CmbPoNoDetail.Focus();
				return false;
			}
			decimal num = Conversion.ToDecimal(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["BalanceWeight"].Value);
			decimal tolerancePercent = ((TolerancePercentageForCmagtPoWeight > 0m) ? (TolerancePercentageForCmagtPoWeight / 100m) : 0m);
			decimal toleranceWeight = num * tolerancePercent;
			decimal allowWeight = num + toleranceWeight;
			if (Conversion.ToDecimal((object)txtGrossWeight.Text) > allowWeight)
			{
				MessageBox.Show($"Weight can't be greater than Allowed Weight: {allowWeight:N2} (Tolerance: {TolerancePercentageForCmagtPoWeight}%)");
				txtGrossWeight.Focus();
				return false;
			}
		}
		return true;
	}

	private void BindGrids()
	{
		try
		{
			DataTable dataTable = dtDetail;
			if (dataTable != null && dataTable.Columns.Count > 0)
			{
				grdDetail.DataSource = dtDetail;
				grdDetail.RetrieveStructure();
				grdDetailSetting(grdDetail);
			}
			DataTable dataTable2 = dtExpGrid;
			if (dataTable2 != null && dataTable2.Columns.Count > 0)
			{
				if (dtExpGrid.Rows.Count == 0)
				{
					AddRowsInExpenseGrid();
				}
				grdInvExp.DataSource = dtExpGrid;
				grdInvExp.RetrieveStructure();
				grdInvExpSettings(grdInvExp);
			}
			DataTable dataTable3 = dtEmptyBags;
			if (dataTable3 != null && dataTable3.Columns.Count > 0)
			{
				if (dtEmptyBags.Rows.Count == 0)
				{
					AddRowInvEmptyBagsGrid();
				}
				grdEmptyBags.RetrieveStructure();
				gridEmptyBagsSettings(grdEmptyBags);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationDetail())
			{
				return;
			}
			if (((UltraGridBase)CmbPoNoDetail).ActiveRow != null && Conversion.ToInt(CmbPoNoDetail.Value) != 0)
			{
				if (Conversion.ToDateTime(((UltraDropDownBase)CmbPoNoDetail).SelectedRow.Cells["ExpiryDate"].Value).Date == datDocDate.Value.Date)
				{
					txtWarningRemarks.Enabled = true;
					if (string.IsNullOrWhiteSpace(txtWarningRemarks.Text))
					{
						MessageBox.Show("Document Date is the same as the selected Purchase Order Expiry Date.\nPlease enter warning remarks before proceeding.", "Remarks Required", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						txtWarningRemarks.Focus();
						return;
					}
				}
				else
				{
					txtWarningRemarks.Enabled = false;
				}
				if (Conversion.ToDateTime(((UltraDropDownBase)CmbPoNoDetail).SelectedRow.Cells["DeliveryStartDate"].Value).Date > datDocDate.Value.Date)
				{
					MessageBox.Show("Document Date is the Less then DeliveryStartDate of selected Purchase Order.", "Error Mesaage", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					datDocDate.Focus();
					return;
				}
			}
			if (grdDetail.GetRows().Count() > 0)
			{
				int MappingId = Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["MappingId"].Value);
				int ItemId = Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemId"].Value);
				if (dtDetail.AsEnumerable().Any((DataRow r) => Conversion.ToInt(r["ItemId"]) == ItemId && Conversion.ToInt(r["MappingId"]) == MappingId))
				{
					MessageBox.Show(((Control)(object)CmbItemName).Text + " already in detail. So can't add this item");
					CmbItemName.Focus();
					return;
				}
			}
			DataRow dr = dtDetail.NewRow();
			dr["Id"] = 0;
			FillCustomerDetailRow(dr);
			dtDetail.Rows.Add(dr);
			grdDetail.DataSource = dtDetail;
			grdDetail.RetrieveStructure();
			grdDetailSetting(grdDetail);
			ResetDetail();
			((Control)(object)CmbSupplierName).Enabled = !GridEX_Helper.IsAnyCellValueGreaterThanZero(grdDetail, "PurchaseOrderId");
			GetEmptyBagDetailOnBaseOfPO();
			GetExpensesOnBaseOfPO();
			CheckPurchaseOrderInDetailAndExpenses();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillCustomerDetailRow(DataRow dr)
	{
		dr["PurchaseOrderId"] = Conversion.ToInt(CmbPoNoDetail.Value);
		dr["PurchaseOrderNo"] = Conversion.ToInt((object)((Control)(object)CmbPoNoDetail).Text);
		dr["PurchaseOrderDetailId"] = ((((UltraGridBase)CmbPoNoDetail).ActiveRow != null) ? Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["OrderDetailId"].Value) : 0);
		dr["POExpiryDate"] = ((((UltraGridBase)CmbPoNoDetail).ActiveRow != null) ? Conversion.ToDateTime(((UltraDropDownBase)CmbPoNoDetail).SelectedRow.Cells["ExpiryDate"].Value) : DateTime.Now);
		dr["PODeliveryStartDate"] = ((((UltraGridBase)CmbPoNoDetail).ActiveRow != null) ? Conversion.ToDateTime(((UltraDropDownBase)CmbPoNoDetail).SelectedRow.Cells["DeliveryStartDate"].Value) : DateTime.Now);
		dr["ParentItemId"] = Conversion.ToInt(CmbParentItem.Value);
		dr["ParentItem"] = Conversion.ToString((object)((Control)(object)CmbParentItem).Text);
		int itemId = Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemId"].Value);
		dr["ItemId"] = itemId;
		string value;
		if (itemId <= 0)
		{
			value = string.Empty;
		}
		else
		{
			UltraGridRow selectedRow = ((UltraDropDownBase)CmbItemName).SelectedRow;
			object obj;
			if (selectedRow == null)
			{
				obj = null;
			}
			else
			{
				UltraGridCell obj2 = selectedRow.Cells["ItemName"];
				obj = ((obj2 != null) ? obj2.Value : null);
			}
			value = Conversion.ToString(obj);
		}
		dr["ItemName"] = value;
		string value2;
		if (itemId <= 0)
		{
			value2 = string.Empty;
		}
		else
		{
			UltraGridRow selectedRow2 = ((UltraDropDownBase)CmbItemName).SelectedRow;
			object obj3;
			if (selectedRow2 == null)
			{
				obj3 = null;
			}
			else
			{
				UltraGridCell obj4 = selectedRow2.Cells["ItemCode"];
				obj3 = ((obj4 != null) ? obj4.Value : null);
			}
			value2 = Conversion.ToString(obj3);
		}
		dr["ItemCode"] = value2;
		dr["CropYearId"] = Conversion.ToInt(CmbCropYear.Value);
		dr["CropYear"] = Conversion.ToString((object)((Control)(object)CmbCropYear).Text);
		dr["PackingTypeId"] = Conversion.ToInt(CmbPackingType.Value);
		dr["PackingType"] = Conversion.ToString((object)((Control)(object)CmbPackingType).Text);
		dr["PackUomId"] = Conversion.ToInt(CmbPackUom.Value);
		dr["PackUom"] = Conversion.ToString((object)((Control)(object)CmbPackUom).Text);
		UltraGridRow selectedRow3 = ((UltraDropDownBase)CmbPackUom).SelectedRow;
		object obj5;
		if (selectedRow3 == null)
		{
			obj5 = null;
		}
		else
		{
			UltraGridCell obj6 = selectedRow3.Cells["Equivalent"];
			obj5 = ((obj6 != null) ? obj6.Value : null);
		}
		dr["PackUomEquivalent"] = Conversion.ToString(obj5);
		dr["Qty"] = Conversion.ToDecimal((object)txtLoadingQty.Text);
		dr["GrossWeight"] = Conversion.ToDecimal((object)txtGrossWeight.Text);
		dr["EbUnit"] = Conversion.ToDecimal((object)txtEbUnit.Text);
		dr["EbTotal"] = Conversion.ToDecimal((object)txtEbTotal.Text);
		dr["AddLessWeight"] = Conversion.ToDecimal((object)txtAddLessWeight.Text);
		dr["NetBillWeight"] = Conversion.ToDecimal((object)txtNetWeight.Text);
		dr["EmptyBagTermId"] = Conversion.ToInt(CmbEmptyBagTerm.Value);
		dr["EmptyBagTerm"] = Conversion.ToString((object)((Control)(object)CmbEmptyBagTerm).Text);
		dr["MappingId"] = ((((UltraGridBase)CmbPoNoDetail).ActiveRow != null) ? Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["MappingId"].Value) : 0);
		dr["SaleOrderId"] = ((((UltraGridBase)CmbPoNoDetail).ActiveRow != null) ? Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["SoId"].Value) : 0);
		dr["SaleOrderDetailId"] = ((((UltraGridBase)CmbPoNoDetail).ActiveRow != null) ? Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["SoDetailId"].Value) : 0);
		dr["SaleOrderNo"] = ((((UltraGridBase)CmbPoNoDetail).ActiveRow != null) ? Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["SoNo"].Value) : 0);
		dr["BuyerId"] = Conversion.ToInt(CmbBuyerNameDetail.Value);
		dr["BuyerName"] = Conversion.ToString((object)((Control)(object)CmbBuyerNameDetail).Text);
		dr["DeliveryToPartyId"] = Conversion.ToInt(CmbDeliveryToParty.Value);
		dr["ShipToAddressId"] = Conversion.ToInt(CmbShipToAddress.Value);
		dr["ShipToAddress"] = Conversion.ToString((object)txtDeliveryToAddress.Text);
		dr["Remarks"] = Conversion.ToString((object)txtRemarksDetail.Text);
		dr["WarningRemarks"] = Conversion.ToString((object)txtWarningRemarks.Text);
	}

	private void BtnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationDetail() || updateDetailIndex < 0 || updateDetailIndex >= dtDetail.Rows.Count)
			{
				return;
			}
			if (((UltraGridBase)CmbPoNoDetail).ActiveRow != null && Conversion.ToInt(CmbPoNoDetail.Value) != 0)
			{
				if (Conversion.ToDateTime(((UltraDropDownBase)CmbPoNoDetail).SelectedRow.Cells["ExpiryDate"].Value).Date == datDocDate.Value.Date)
				{
					txtWarningRemarks.Enabled = true;
					if (string.IsNullOrWhiteSpace(txtWarningRemarks.Text))
					{
						MessageBox.Show("Document Date is the same as the selected Purchase Order Expiry Date.\nPlease enter warning remarks before proceeding.", "Remarks Required", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						txtWarningRemarks.Focus();
						return;
					}
				}
				else
				{
					txtWarningRemarks.Enabled = false;
				}
				if (Conversion.ToDateTime(((UltraDropDownBase)CmbPoNoDetail).SelectedRow.Cells["DeliveryStartDate"].Value).Date > datDocDate.Value.Date)
				{
					MessageBox.Show("Document Date is the Less then DeliveryStartDate of selected Purchase Order.", "Error Mesaage", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					datDocDate.Focus();
					return;
				}
			}
			if (grdDetail.GetRows().Count() > 0)
			{
				int itemId = Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemId"].Value);
				int mappingId = Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["MappingId"].Value);
				if ((from r in dtDetail.AsEnumerable()
					where r.RowState != DataRowState.Deleted
					select r).Where((DataRow r, int index) => index != updateDetailIndex).Any((DataRow r) => Conversion.ToInt(r["ItemId"]) == itemId && Conversion.ToInt(r["MappingId"]) == mappingId))
				{
					MessageBox.Show(((Control)(object)CmbItemName).Text + " already in detail. So can't add this item.");
					CmbItemName.Focus();
					return;
				}
			}
			DataRow dr = dtDetail.Rows[updateDetailIndex];
			FillCustomerDetailRow(dr);
			grdDetail.DataSource = dtDetail;
			((Control)(object)grdDetail).Refresh();
			ResetDetail();
			((Control)(object)CmbSupplierName).Enabled = !GridEX_Helper.IsAnyCellValueGreaterThanZero(grdDetail, "PurchaseOrderId");
			GetEmptyBagDetailOnBaseOfPO();
			GetExpensesOnBaseOfPO();
			CheckPurchaseOrderInDetailAndExpenses();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow row = grdDetail.CurrentRow;
			if (row != null)
			{
				updateDetailIndex = row.RowIndex;
				if (Conversion.ToInt(row.Cells["PurchaseOrderId"].Value) > 0)
				{
					CmbPoNoDetail.Value = Conversion.ToInt(row.Cells["PurchaseOrderId"].Value);
					CmbPoNoDetail_Leave(null, null);
				}
				CmbParentItem.Value = Conversion.ToInt(row.Cells["ParentItemId"].Value);
				cmbParentItem_Leave(null, null);
				if (Conversion.ToInt(row.Cells["MappingId"].Value) > 0)
				{
					CmbItemName.Value = Conversion.ToInt(row.Cells["MappingId"].Value);
					CmbPoNoDetail.Focus();
				}
				else
				{
					CmbItemName.Value = Conversion.ToInt(row.Cells["ItemId"].Value);
					CmbParentItem.Focus();
				}
				CmbItemName_Leave(null, null);
				CmbCropYear.Value = Conversion.ToInt(row.Cells["CropYearId"].Value);
				CmbPackingType.Value = Conversion.ToInt(row.Cells["PackingTypeId"].Value);
				CmbPackUom.Value = Conversion.ToInt(row.Cells["PackUomId"].Value);
				txtLoadingQty.Text = Conversion.ToDecimal(row.Cells["Qty"].Value).ToString("#,##0.###");
				txtGrossWeight.Text = Conversion.ToDecimal(row.Cells["GrossWeight"].Value).ToString("#,##0.###");
				txtEbUnit.Text = Conversion.ToDecimal(row.Cells["EbUnit"].Value).ToString("#,##0.###");
				txtEbTotal.Text = Conversion.ToDecimal(row.Cells["EbTotal"].Value).ToString("#,##0.###");
				txtAddLessWeight.Text = Conversion.ToDecimal(row.Cells["AddLessWeight"].Value).ToString("#,##0.###");
				txtNetWeight.Text = Conversion.ToDecimal(row.Cells["NetBillWeight"].Value).ToString("#,##0.###");
				CmbEmptyBagTerm.Value = Conversion.ToInt(row.Cells["EmptyBagTermId"].Value);
				if (Conversion.ToInt(row.Cells["BuyerId"].Value) > 0)
				{
					CmbBuyerNameDetail.Value = Conversion.ToInt(row.Cells["BuyerId"].Value);
				}
				if (Conversion.ToInt(row.Cells["DeliveryToPartyId"].Value) > 0)
				{
					CmbDeliveryToParty.Value = Conversion.ToInt(row.Cells["DeliveryToPartyId"].Value);
					CmbDeliveryToParty_Leave(null, null);
				}
				if (Conversion.ToInt(row.Cells["ShipToAddressId"].Value) > 0)
				{
					CmbShipToAddress.Value = Conversion.ToInt(row.Cells["ShipToAddressId"].Value);
				}
				txtDeliveryToAddress.Text = Conversion.ToString(row.Cells["ShipToAddress"].Value);
				txtRemarksDetail.Text = Conversion.ToString(row.Cells["Remarks"].Value);
				txtWarningRemarks.Text = Conversion.ToString(row.Cells["WarningRemarks"].Value);
				BtnAdd.Visible = false;
				BtnUpdateDetail.Visible = true;
				BtnCancelDetail.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCancelDetail_Click(object sender, EventArgs e)
	{
		try
		{
			ResetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailSetting(GridEX gridEX)
	{
		try
		{
			if (gridEX == null)
			{
				return;
			}
			GrnLoadingChallan_Helper.DetailGridCommonSetting(gridEX);
			if (((Control)(object)gridEX).Name == "grdDetail")
			{
				GridEX_Helper.AddButton(grdDetail, "Delete", "X", 20, 0);
				GridEX_Helper.AddButton(grdDetail, "Edit", "Edit", 40, 1);
				grdDetail.FrozenColumns = 2;
				ctrlGrdBar2_Load(null, null);
			}
			DataTable dt = (DataTable)gridEX.DataSource;
			if (dt != null)
			{
				if (dt.AsEnumerable().Any((DataRow r) => Conversion.ToInt(r["PurchaseOrderId"]) > 0))
				{
					gridEX.RootTable.Columns["PurchaseOrderNo"].Visible = true;
					gridEX.RootTable.Columns["SaleOrderNo"].Visible = true;
				}
				else
				{
					gridEX.RootTable.Columns["PurchaseOrderNo"].Visible = false;
					gridEX.RootTable.Columns["SaleOrderNo"].Visible = false;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (r != null)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
				if (e.Column.Key == "Edit")
				{
					grdDetail_DoubleClick(null, null);
				}
				grdDetail.UpdateData();
				((Control)(object)CmbSupplierName).Enabled = !GridEX_Helper.IsAnyCellValueGreaterThanZero(grdDetail, "PurchaseOrderId");
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
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		try
		{
			if (r != null && (int)r.RowType == 0)
			{
				if (updateDetailIndex != -1)
				{
					throw new Exception("Please Reset the Detail first..");
				}
				if (Conversion.ToInt((object)r.Cells["Id"].Value.ToString()) != 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					grnSupplierLoadingDetail d = new grnSupplierLoadingDetail();
					d.grnSupplierLoadingDetailId = Conversion.ToInt(r.Cells["Id"].Value);
					FillDetailListCommonForInsertAndDelete(d, r);
					d.actionTypeId = 3;
					lstRemoveRecordDetail.Add(d);
					dtDetail.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
				{
					dtDetail.Rows[r.RowIndex].Delete();
					r.Delete();
				}
			}
			CheckPurchaseOrderInDetailAndExpenses();
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
			if (r == null || (int)r.RowType != 0 || r.Cells.Count == 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdDetail.CurrentColumn.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
				if (grdDetail.CurrentColumn.Key == "Edit")
				{
					grdDetail_DoubleClick(null, null);
				}
				grdDetail.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteDetailRow(r);
				grdDetail.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdDetail_DoubleClick(null, null);
			}
			if (e.KeyCode == Keys.F1)
			{
				grdDetail.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowsInExpenseGrid()
	{
		try
		{
			dtExpGrid.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, "");
			grdInvExp.DataSource = dtExpGrid;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteRowInExpenseGrid(GridEXRow item)
	{
		try
		{
			dtExpGrid.Rows[item.RowIndex].Delete();
			item.Delete();
			if (dtExpGrid.Rows.Count == 0)
			{
				AddRowsInExpenseGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExpenseGridCombBind()
	{
		try
		{
			GridEX_Helper.ComboBind(grdInvExp.RootTable.Columns["ItemId"], dtOtherItem, "Id", "OtherItemName", "Other Item Name", 200);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExpSettings(GridEX gridEX)
	{
		try
		{
			if (gridEX == null)
			{
				return;
			}
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"Qty",
					(EditType)1
				},
				{
					"Rate",
					(EditType)1
				},
				{
					"Amount",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			GrnLoadingChallan_Helper.ExpenseGridCommonSetting(gridEX, editableColumns);
			if (((Control)(object)gridEX).Name == "grdInvExp")
			{
				ExpenseGridCombBind();
				GridEX_Helper.AddButton(gridEX, "Add", "+", 20, 0);
				GridEX_Helper.AddButton(gridEX, "Delete", "X", 20, 0);
				gridEX.FrozenColumns = 2;
				ctrlGrdBar3_Load(null, null);
			}
			DataTable dt = (DataTable)gridEX.DataSource;
			if (dt != null)
			{
				if (dt.AsEnumerable().Any((DataRow r) => Conversion.ToInt(r["PoId"]) > 0))
				{
					gridEX.RootTable.Columns["PurchaseOrderNo"].Visible = true;
				}
				else
				{
					gridEX.RootTable.Columns["PurchaseOrderNo"].Visible = false;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdInvExp.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteRowInExpenseGrid(item);
				}
				if (e.Column.Key == "Add")
				{
					AddRowsInExpenseGrid();
				}
			}
			grdInvExp.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdInvExp.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				switch (e.Column.Key)
				{
				default:
					return;
				case "Qty":
				case "Rate":
					UpdateAmount(row);
					break;
				case "Amount":
					UpdateRate(row);
					break;
				}
				grdInvExp.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateAmount(GridEXRow row)
	{
		double qty = Conversion.ToDouble(row.Cells["Qty"].Value);
		double rate = Conversion.ToDouble(row.Cells["Rate"].Value);
		row.Cells["Amount"].Value = qty * rate;
	}

	private void UpdateRate(GridEXRow row)
	{
		double amount = Conversion.ToDouble(row.Cells["Amount"].Value);
		double qty = Conversion.ToDouble(row.Cells["Qty"].Value);
		row.Cells["Rate"].Value = ((qty == 0.0) ? 0.0 : (amount / qty));
	}

	private void AddRowInvEmptyBagsGrid()
	{
		try
		{
			dtEmptyBags.Rows.Add(0, 0, 0, 0, 0, 0, 0);
			grdEmptyBags.DataSource = dtEmptyBags;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdEmptyBagsRefresh()
	{
		try
		{
			GridEX_Helper.ComboBind(grdEmptyBags.RootTable.Columns["PackingType"], dtEbGridPackingTypes, "Id", "Name", "Packing Type");
			GridEX_Helper.ComboBind(grdEmptyBags.RootTable.Columns["EmptyBagItem"], dtItemForEmptyBag, "ItemId", "ItemName", "Empty Bag Item");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridEmptyBagsSettings(GridEX gridEX)
	{
		try
		{
			if (gridEX == null)
			{
				return;
			}
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType> { 
			{
				"PurchaseRate",
				(EditType)1
			} };
			GrnLoadingChallan_Helper.EmptyBagGridCommonSetting(gridEX, editableColumns);
			if (((Control)(object)gridEX).Name == "grdEmptyBags")
			{
				GrdEmptyBagsRefresh();
				GridEX_Helper.AddButton(gridEX, "Add", "+", 20, 0);
				GridEX_Helper.AddButton(gridEX, "Delete", "X", 20, 0);
				gridEX.FrozenColumns = 2;
				ctrlGrdBar4_Load(null, null);
			}
			DataTable dt = (DataTable)gridEX.DataSource;
			if (dt != null)
			{
				if (dt.AsEnumerable().Any((DataRow r) => Conversion.ToInt(r["PoId"]) > 0))
				{
					gridEX.RootTable.Columns["PurchaseOrderNo"].Visible = true;
				}
				else
				{
					gridEX.RootTable.Columns["PurchaseOrderNo"].Visible = false;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteRowInEmptyBagGrid(GridEXRow item)
	{
		try
		{
			dtEmptyBags.Rows[item.RowIndex].Delete();
			item.Delete();
			if (dtEmptyBags.Rows.Count == 0)
			{
				AddRowInvEmptyBagsGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmptyBags_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdEmptyBags.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteRowInEmptyBagGrid(item);
				}
				if (e.Column.Key == "Add")
				{
					AddRowInvEmptyBagsGrid();
				}
			}
			grdEmptyBags.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailListCommonForInsertAndDelete(grnSupplierLoadingDetail vd, GridEXRow r)
	{
		vd.grnSupplierLoadingDetailId = Conversion.ToInt(r.Cells["Id"].Value);
		vd.purchaseOrderMasterId = Conversion.ToInt(r.Cells["PurchaseOrderId"].Value);
		vd.purchaseOrderDetailId = Conversion.ToInt(r.Cells["PurchaseOrderDetailId"].Value);
		vd.PurchaseOrderNo = Conversion.ToInt(r.Cells["PurchaseOrderNo"].Value);
		vd.POExpiryDate = Conversion.ToDateTime(r.Cells["POExpiryDate"].Value);
		vd.PODeliveryStartDate = Conversion.ToDateTime(r.Cells["PODeliveryStartDate"].Value);
		vd.inventoryParentCategoryId = Conversion.ToInt(r.Cells["ParentItemId"].Value);
		vd.InvParentCateDescription = Conversion.ToString(r.Cells["ParentItem"].Value);
		vd.itemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		vd.ItemName = Conversion.ToString(r.Cells["ItemName"].Value);
		vd.cropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
		vd.cropYear = Conversion.ToString(r.Cells["CropYear"].Value);
		vd.packingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
		vd.PackingType = Conversion.ToString(r.Cells["PackingType"].Value);
		vd.packUomId = Conversion.ToInt(r.Cells["PackUomId"].Value);
		vd.PackSize = Conversion.ToString(r.Cells["PackUom"].Value);
		vd.loadingQty = Conversion.ToDecimal(r.Cells["Qty"].Value);
		vd.wbGrossWeight = Conversion.ToDecimal(r.Cells["GrossWeight"].Value);
		vd.ebwPerUnit = Conversion.ToDecimal(r.Cells["EbUnit"].Value);
		vd.ebwTotal = Conversion.ToDecimal(r.Cells["EbTotal"].Value);
		vd.addLessWeight = Conversion.ToDecimal(r.Cells["AddLessWeight"].Value);
		vd.netBillWeight = Conversion.ToDecimal(r.Cells["NetBillWeight"].Value);
		vd.EBWeightDeductionTermId = Conversion.ToInt(r.Cells["EmptyBagTermId"].Value);
		vd.purchaseOrderSaleOrderMappingId = Conversion.ToInt(r.Cells["MappingId"].Value);
		vd.saleOrderMasterId = Conversion.ToInt(r.Cells["SaleOrderId"].Value);
		vd.saleOrderDetailId = Conversion.ToInt(r.Cells["SaleOrderDetailId"].Value);
		vd.SaleOrderNo = Conversion.ToInt(r.Cells["SaleOrderNo"].Value);
		vd.buyerId = Conversion.ToInt(r.Cells["BuyerId"].Value);
		vd.BuyerName = Conversion.ToString(r.Cells["BuyerName"].Value);
		vd.DeliverToPartyId = Conversion.ToInt(r.Cells["DeliveryToPartyId"].Value);
		vd.DeliverToPartyName = Conversion.ToString(r.Cells["DeliveryToParty"].Value);
		vd.DeliverToAddressId = Conversion.ToInt(r.Cells["ShipToAddressId"].Value);
		vd.DeliverToAddress = Conversion.ToString(r.Cells["ShipToAddress"].Value);
		vd.remarks = Conversion.ToString(r.Cells["Remarks"].Value);
		vd.warningRemarks = Conversion.ToString(r.Cells["WarningRemarks"].Value);
	}

	private void Insert()
	{
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Expected O, but got Unknown
		//IL_0670: Unknown result type (might be due to invalid IL or missing references)
		//IL_0677: Expected O, but got Unknown
		//IL_0a08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0f: Expected O, but got Unknown
		//IL_0ba8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0baf: Expected O, but got Unknown
		try
		{
			if (grdDetail.GetRows().Count() == 0)
			{
				throw new Exception("Detail Record Not Found");
			}
			List<(Control, string, FormHelper.ValidType)> controlsToValidate = new List<(Control, string, FormHelper.ValidType)>
			{
				((Control)(object)CmbCompanyName, "Company Name", FormHelper.ValidType.INT),
				((Control)(object)CmbBranch, "Branch Name", FormHelper.ValidType.INT),
				(txtDocNo, "Doc No", FormHelper.ValidType.INT),
				((Control)(object)CmbCommissionAgent, "Commission Agent", FormHelper.ValidType.INT),
				((Control)(object)CmbSupplierName, "Supplier Name", FormHelper.ValidType.INT),
				((Control)(object)CmbLoadingCity, "Loading City", FormHelper.ValidType.INT),
				((Control)(object)CmbUnloadingCity, "Un-Loading City", FormHelper.ValidType.INT),
				((Control)(object)cmbVehicleType, "Vehicle Type", FormHelper.ValidType.INT),
				(txtVehicleNo, "Vehicle No", FormHelper.ValidType.String),
				(txtBiltyNo, "Bilty No", FormHelper.ValidType.String),
				((Control)(object)CmbDeliveryTerm, "Delivery Term", FormHelper.ValidType.INT),
				(txtVehicleQty, "Vehicle Qty", FormHelper.ValidType.Double),
				(txtWbNetWeight, "Net Wb Weight", FormHelper.ValidType.Double)
			};
			if (Conversion.ToDecimal((object)txtTotalFreight.Text) > 0m && Conversion.ToInt(CmbTransporter.Value) == 0 && string.IsNullOrEmpty(txtTransporterName.Text))
			{
				MessageBox.Show("Please Select Transporter or Fill Transporter Name");
				CmbTransporter.Focus();
				return;
			}
			string value = txtVehicleNo.Text.Trim().ToUpper();
			if (!Regex.IsMatch(value, "^[A-Z]{1,6}-\\d{1,6}$"))
			{
				MessageBox.Show("Vehicle no is not valid. Please check!");
				txtVehicleNo.Focus();
				return;
			}
			string[] array = value.Split('-');
			string letters = array[0];
			string digits = array[1];
			if ((letters.Length <= 2 || digits.Length <= 2) && (letters.Length <= 2 || digits.Length <= 2) && MessageBox.Show("The vehicle number is unusually short (letters or digits less than or equal to 2).\nAre you sure you want to proceed?", "Confirm Vehicle Number", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
			{
				txtVehicleNo.Focus();
			}
			else if (GrnLoadingDocDateAndBiltyDateSame && datDocDate.Value.Date != txtBiltyDate.Value.Date)
			{
				MessageBox.Show("Doc Date And Bilty Date Can't be different");
				txtBiltyDate.Focus();
			}
			else if (GrnLoadingAllowDifferentBiltyDate && datDocDate.Value.Date != txtBiltyDate.Value.Date && MessageBox.Show("Document Date and Bilty Date are different.\nDo you want to continue?", "Date Mismatch Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
			{
				txtBiltyDate.Focus();
			}
			else
			{
				if (!FormHelper.ValidateControls(controlsToValidate) || !FormHelper.ConfirmAction((RecId == 0) ? "Are you sure to Save" : "Are you sure to Update"))
				{
					return;
				}
				grnSupplierLoadingMaster obj = new grnSupplierLoadingMaster();
				obj.organizationId = UserAccount.OrganizationId;
				obj.companyId = Conversion.ToInt(CmbCompanyName.Value);
				obj.branchId = Conversion.ToInt(CmbBranch.Value);
				obj.financialYearId = clsGlobalVariables.ActiveYr.Id;
				obj.documentTypeId = DocumentTypeId;
				obj.entryDate = DateTime.Now;
				obj.entryUserId = UserAccount.ID;
				obj.modifyDate = DateTime.Now;
				obj.modifyUserId = UserAccount.ID;
				obj.approvedDate = DateTime.Now;
				obj.isApproved = false;
				obj.grnSupplierLoadingMasterId = RecId;
				obj.docNo = Conversion.ToInt((object)txtDocNo.Text);
				obj.docDate = Conversion.ToDateTime((object)datDocDate.Value);
				obj.commissionAgentId = Conversion.ToInt(CmbCommissionAgent.Value);
				obj.supplierId = Conversion.ToInt(CmbSupplierName.Value);
				obj.loadingCityId = Conversion.ToInt(CmbLoadingCity.Value);
				obj.unloadingCityId = Conversion.ToInt(CmbUnloadingCity.Value);
				if (Conversion.ToDecimal((object)txtTotalFreight.Text) > 0m)
				{
					obj.transporterId = Conversion.ToInt(CmbTransporter.Value);
					obj.transporterName = Conversion.ToString((object)txtTransporterName.Text);
					obj.biltyFreight = Conversion.ToDecimal((object)txtBiltyFreight.Text);
					obj.otherAdLesCharges = Conversion.ToDecimal((object)txtOtherCharges.Text);
					obj.totalFreight = Conversion.ToDecimal((object)txtTotalFreight.Text);
				}
				obj.vehicleTypeId = Conversion.ToInt(cmbVehicleType.Value);
				obj.vehicleNo = txtVehicleNo.Text.Trim();
				obj.biltyDate = Conversion.ToDateTime((object)txtBiltyDate.Value);
				obj.biltyNo = txtBiltyNo.Text.Trim();
				obj.deliveryTermId = Conversion.ToInt(CmbDeliveryTerm.Value);
				obj.biltyQty = Conversion.ToDecimal((object)txtVehicleQty.Text);
				obj.loadWeight = Conversion.ToDecimal((object)txtLoadWeight.Text);
				obj.tareWeight = Conversion.ToDecimal((object)txtTareWeight.Text);
				obj.scaleNetWeight = Conversion.ToDecimal((object)txtWbNetWeight.Text);
				obj.remarksHeader = txtRemarks.Text;
				if (lstRemoveRecordDetail.Count > 0 && RecId > 0)
				{
					for (int i = 0; i < lstRemoveRecordDetail.Count; i++)
					{
						obj.grnSupplierLoadingDetailList.Add(lstRemoveRecordDetail[i]);
					}
				}
				decimal grossWeight = default(decimal);
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow r3 in rows)
				{
					grnSupplierLoadingDetail vd = new grnSupplierLoadingDetail();
					vd.grnSupplierLoadingDetailId = ((RecId != 0) ? Conversion.ToInt(r3.Cells["Id"].Value) : 0);
					vd.actionTypeId = ((vd.grnSupplierLoadingDetailId <= 0) ? 1 : 2);
					FillDetailListCommonForInsertAndDelete(vd, r3);
					FormHelper.ValidateField(vd.inventoryParentCategoryId, "Parent Category", r3.RowIndex);
					FormHelper.ValidateField(vd.itemId, "Item Name", r3.RowIndex);
					FormHelper.ValidateField(vd.cropYearId, "Crop Year", r3.RowIndex);
					FormHelper.ValidateField(vd.packingTypeId, "Packing Type", r3.RowIndex);
					FormHelper.ValidateField(vd.packUomId, "Pack Uom", r3.RowIndex);
					FormHelper.ValidateField(vd.loadingQty, "Loading Qty", r3.RowIndex);
					FormHelper.ValidateField(vd.wbGrossWeight, "Gross Weight", r3.RowIndex);
					FormHelper.ValidateField(vd.netBillWeight, "Net Bill Weight", r3.RowIndex);
					grossWeight += vd.wbGrossWeight;
					string WarningRemarks = "";
					if (vd.purchaseOrderMasterId > 0)
					{
						DateTime poExpiryDate = Conversion.ToDateTime(r3.Cells["POExpiryDate"].Value).Date;
						DateTime PODeliveryStartDate = Conversion.ToDateTime(r3.Cells["PODeliveryStartDate"].Value).Date;
						if (obj.docDate.Date == poExpiryDate && string.IsNullOrEmpty(vd.warningRemarks))
						{
							MessageBox.Show("Vehicle has reached late compared to PO Expiry Date.\n\nRemarks are required to proceed.", "Vehicle Late Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							frmRemarks frm = new frmRemarks("Late Vehicle Remarks");
							if (frm.ShowDialog() != DialogResult.OK)
							{
								datDocDate.Focus();
								return;
							}
							WarningRemarks = frm.RemarksText;
							if (string.IsNullOrWhiteSpace(WarningRemarks))
							{
								MessageBox.Show("Remarks are required for late vehicle entry.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
								datDocDate.Focus();
								return;
							}
							r3.BeginEdit();
							r3.Cells["WarningRemarks"].Value = WarningRemarks;
							r3.EndEdit();
							vd.warningRemarks = WarningRemarks;
						}
						else
						{
							if (obj.docDate.Date > poExpiryDate)
							{
								MessageBox.Show("Grn Date is greater than Purchase Order Expiry date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
								datDocDate.Focus();
								return;
							}
							if (obj.docDate.Date < PODeliveryStartDate)
							{
								MessageBox.Show("Grn Date is less than Purchase Order Delivery Start date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
								datDocDate.Focus();
								return;
							}
						}
					}
					obj.grnSupplierLoadingDetailList.Add(vd);
				}
				rows = grdEmptyBags.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					if (Conversion.ToInt(r2.Cells["EmptyBagItem"].Value) > 0 && Conversion.ToInt(r2.Cells["PackingType"].Value) > 0)
					{
						grnSupplierLoadingEmptyBagDetail emptyBags = new grnSupplierLoadingEmptyBagDetail();
						emptyBags.purchaseOrderMasterId = Conversion.ToInt(r2.Cells["PoId"].Value);
						emptyBags.PurchaseOrderNo = Conversion.ToInt(r2.Cells["PurchaseOrderNo"].Value);
						emptyBags.purchaseOrderEmptyBagDetailId = Conversion.ToInt(r2.Cells["PoEbId"].Value);
						emptyBags.PackingTypeId = Conversion.ToInt(r2.Cells["PackingType"].Value);
						emptyBags.PackingType = Conversion.ToString((object)r2.Cells["PackingType"].Text);
						emptyBags.Rate = Conversion.ToDecimal(r2.Cells["PurchaseRate"].Value);
						emptyBags.emptyBagPackingMaterialItemId = Conversion.ToInt(r2.Cells["EmptyBagItem"].Value);
						emptyBags.EmptyBagItem = Conversion.ToString((object)r2.Cells["EmptyBagItem"].Text);
						obj.grnSupplierLoadingEmptyBagDetailList.Add(emptyBags);
					}
				}
				rows = grdInvExp.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToInt((object)r.Cells["ItemId"].Value.ToString()) != 0 && Conversion.ToDouble(r.Cells["Amount"].Value) > 0.0)
					{
						grnSupplierLoadingExpenseDetail PE = new grnSupplierLoadingExpenseDetail();
						PE.purchaseOrderMasterId = Conversion.ToInt(r.Cells["PoId"].Value);
						PE.purchaseOrderExpenseDetailId = Conversion.ToInt(r.Cells["PoExpenseId"].Value);
						PE.PurchaseOrderNo = Conversion.ToInt(r.Cells["PurchaseOrderNo"].Value);
						PE.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						PE.OtherItemName = Conversion.ToString((object)r.Cells["ItemId"].Text);
						PE.Qty = Conversion.ToDecimal(r.Cells["Qty"].Value);
						PE.rate = Conversion.ToDouble(r.Cells["Rate"].Value);
						PE.amount = Conversion.ToDouble(r.Cells["Amount"].Value);
						PE.remarks = Conversion.ToString((object)r.Cells["Remarks"].Text.Trim());
						if (PE.remarks == "0" || PE.remarks == string.Empty || PE.remarks == null)
						{
							PE.remarks = "Expense : " + Conversion.ToString((object)r.Cells["ItemId"].Text.Trim()) + "  Qty" + PE.Qty + "  @" + PE.rate;
						}
						obj.grnSupplierLoadingExpenseDetailList.Add(PE);
					}
				}
				_ = obj.grnSupplierLoadingExpenseDetailList;
				if (obj.scaleNetWeight != grossWeight)
				{
					throw new Exception("Header Net Weight:" + obj.scaleNetWeight.ToString("#,##0.###") + " not equal to detail gross weight:" + grossWeight.ToString("#,###0.###") + ". please Check!");
				}
				obj.AttachmentsList = new List<DMSAttachments>();
				obj.DeleteAttachmentsList = new List<DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<grnSupplierLoadingMaster>(obj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				int success = grnSupplierLoadingMaster.Save(obj);
				if (RecId > 0)
				{
					MessageBox.Show("Update Successfully");
					FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, DocumentTypeId, base.Name);
				}
				else
				{
					MessageBox.Show("Save Successfully");
				}
				Reset();
				if (chkPrint.Checked)
				{
					OpenPrint(success);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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

	private void ReadById(int recordId)
	{
		try
		{
			Reset();
			RecId = recordId;
			grnSupplierLoadingMaster obj = grnSupplierLoadingMaster.ReadById(recordId);
			if (obj == null || obj.grnSupplierLoadingDetailList.Count == 0)
			{
				throw new Exception("Record Not Found");
			}
			tabControl1.SelectedIndex = 0;
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			txtDocNo.Text = Conversion.ToString((object)obj.docNo);
			datDocDate.Value = Conversion.ToDateTime((object)obj.docDate);
			CmbCompanyName.Value = Conversion.ToInt((object)obj.companyId);
			CmbBranch.Value = Conversion.ToInt((object)obj.branchId);
			CmbCommissionAgent.Value = Conversion.ToInt((object)obj.commissionAgentId);
			CmbSupplierName.Value = Conversion.ToInt((object)obj.supplierId);
			CmbSupplierName_Leave(null, null);
			CmbLoadingCity.Value = Conversion.ToInt((object)obj.loadingCityId);
			CmbUnloadingCity.Value = Conversion.ToInt((object)obj.unloadingCityId);
			if (Conversion.ToInt((object)obj.transporterId) > 0)
			{
				CmbTransporter.Value = Conversion.ToInt((object)obj.transporterId);
			}
			txtTransporterName.Text = Conversion.ToString((object)obj.transporterName);
			txtBiltyFreight.Text = Conversion.ToDecimal((object)obj.biltyFreight).ToString("#,##0.###");
			txtOtherCharges.Text = Conversion.ToDecimal((object)obj.otherAdLesCharges).ToString("#,##0.###");
			txtTotalFreight.Text = Conversion.ToDecimal((object)obj.totalFreight).ToString("#,##0.###");
			cmbVehicleType.Value = Conversion.ToInt((object)obj.vehicleTypeId);
			txtVehicleNo.Text = Conversion.ToString((object)obj.vehicleNo);
			txtBiltyNo.Text = Conversion.ToString((object)obj.biltyNo);
			txtBiltyDate.Value = Conversion.ToDateTime((object)obj.biltyDate);
			CmbDeliveryTerm.Value = Conversion.ToInt((object)obj.deliveryTermId);
			txtLoadWeight.Text = Conversion.ToDecimal((object)obj.loadWeight).ToString("#,##0.###");
			txtTareWeight.Text = Conversion.ToDecimal((object)obj.tareWeight).ToString("#,##0.###");
			txtWbNetWeight.Text = Conversion.ToDecimal((object)obj.scaleNetWeight).ToString("#,##0.###");
			txtWbNetWeight.AccessibleDescription = Conversion.ToDecimal((object)obj.scaleNetWeight).ToString("#,##0.###");
			txtVehicleQty.Text = Conversion.ToDecimal((object)obj.biltyQty).ToString("#,##0.###");
			txtRemarks.Text = Conversion.ToString((object)obj.remarksHeader);
			AttachmentsValues = obj.attachmentsValues;
			CustomAttachmentsValues = obj.customAttachmentsValues;
			dtDetail.Rows.Clear();
			GrnLoadingChallan_Helper.FillDetailtableFromListCommonForReadById(obj, dtDetail);
			dtEmptyBags.Rows.Clear();
			GrnLoadingChallan_Helper.FillEmptyBagTableFromListCommonForReadById(obj, dtEmptyBags);
			dtExpGrid.Rows.Clear();
			GrnLoadingChallan_Helper.FillExpenseTableFromListCommonForReadById(obj, dtExpGrid);
			BindGrids();
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			OutstandingOrdersdtFillDbCall();
			OrderNoBind();
			((Control)(object)CmbSupplierName).Enabled = !GridEX_Helper.IsAnyCellValueGreaterThanZero(grdDetail, "PurchaseOrderId");
			datDocDate.Focus();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
					grnSupplierLoadingMaster.DeleteByID(UserAccount.ID, RecId);
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

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			Reset();
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
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			RecId = 0;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			((Control)(object)CmbCommissionAgent).Text = string.Empty;
			((Control)(object)CmbSupplierName).Text = string.Empty;
			((Control)(object)CmbLoadingCity).Text = string.Empty;
			((Control)(object)CmbUnloadingCity).Text = string.Empty;
			((Control)(object)CmbTransporter).Text = string.Empty;
			((Control)(object)CmbDeliveryToParty).Text = string.Empty;
			txtTransporterName.Text = string.Empty;
			txtBiltyFreight.Text = string.Empty;
			txtOtherCharges.Text = string.Empty;
			txtTotalFreight.Text = string.Empty;
			txtVehicleNo.Text = string.Empty;
			txtBiltyNo.Text = string.Empty;
			txtVehicleQty.Text = string.Empty;
			txtLoadWeight.Text = string.Empty;
			txtTareWeight.Text = string.Empty;
			txtWbNetWeight.Text = string.Empty;
			txtWbNetWeight.AccessibleDescription = string.Empty;
			txtAvgFillingPerBag.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			ResetDetail();
			dtDetail.Rows.Clear();
			dtExpGrid.Rows.Clear();
			dtEmptyBags.Rows.Clear();
			BindGrids();
			OutstandingOrdersdtFillDbCall();
			OrderNoBind();
			((Control)(object)CmbSupplierName).Enabled = true;
			txtDocNo.Text = DocumentNoDbCall();
			datDocDate.Focus();
			GetCommissionAgentConfigurationsFromGlobalandBind();
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
			txtPoTotalWeightDetail.Text = string.Empty;
			txtPoAlreadyReceivedWeightDetail.Text = string.Empty;
			txtPoBalWeightDetail.Text = string.Empty;
			((Control)(object)CmbItemName).Text = string.Empty;
			((UltraGridBase)CmbPackUom).DataSource = null;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtLoadingQty.Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			txtEbUnit.Text = string.Empty;
			txtEbTotal.Text = string.Empty;
			txtNetWeight.Text = string.Empty;
			((Control)(object)CmbEmptyBagTerm).Text = string.Empty;
			((Control)(object)CmbEmptyBagTerm).Enabled = true;
			txtSaleOrderNo.Text = string.Empty;
			((Control)(object)CmbBuyerNameDetail).Text = string.Empty;
			((Control)(object)CmbBuyerNameDetail).Enabled = true;
			((Control)(object)CmbShipToAddress).Text = string.Empty;
			txtDeliveryToAddress.Text = string.Empty;
			txtRemarksDetail.Text = string.Empty;
			updateDetailIndex = -1;
			BtnAdd.Visible = true;
			BtnCancelDetail.Visible = false;
			BtnUpdateDetail.Visible = false;
			txtWarningRemarks.Text = string.Empty;
			txtWarningRemarks.Enabled = false;
			CmbPoNoDetail.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "SupplierCustomerLists", "AllItems", "CropYear", "UomSchedule", "InvPackingType" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			GetConfigurationsFromGlobal();
			CompanyBind(CompanyDropdownData());
			BranchesBind(BranchesAllocationToUser.GetBranchsAllocatedToUser(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID));
			CommonBindings.SupplierDtFillFromGlobal(dtSupplier, useBusinessName: true);
			SupplierBind();
			CityBindFromGlobal();
			DeliveryTermBind(DeliveryTerm.FormHistory());
			OutstandingOrdersdtFillDbCall();
			OrderNoBind();
			dtShipToAddress = DatatableHelper.AllShipToAddressDbCall(UserAccount);
			ParentCategoryBindFromGlobal();
			if (((UltraGridBase)CmbPoNoDetail).ActiveRow != null && Conversion.ToInt(CmbPoNoDetail.Value) > 0)
			{
				ItemdtFillAgainstOder();
				CmbBuyerNameDetail_Leave(null, null);
			}
			else
			{
				ItemdtFillFromGlobal(Conversion.ToInt(CmbParentItem.Value));
			}
			ItemNameBind();
			CropDtFillFromGlobalAndBind();
			PackingTypeDtFillFromGlobalAndBind();
			GetCommissionAgentConfigurationsFromGlobalandBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmGrnLoadingChallanCmagt_FormClosing(object sender, FormClosingEventArgs e)
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

	private void EbCalculations()
	{
		try
		{
			double EBUnit = Conversion.ToDouble((object)txtEbUnit.Text);
			double EBTotal = Conversion.ToDouble((object)txtEbTotal.Text);
			double factor = Conversion.ToDouble((object)txtLoadingQty.Text);
			string activeTag = Convert.ToString(base.ActiveControl?.AccessibleName);
			double calcValue = 0.0;
			if (factor <= 0.0)
			{
				txtEbUnit.Text = "0";
				txtEbTotal.Text = "0";
			}
			else if (activeTag == Convert.ToString(txtEbUnit.AccessibleName))
			{
				calcValue = ((EBUnit > 0.0) ? (EBUnit * factor) : 0.0);
				calcValue = Math.Round(calcValue, 3);
				txtEbTotal.Text = calcValue.ToString("#,##0.###");
				CalculateByEbUnit = true;
				CalculateByEbTotal = false;
			}
			else if (activeTag == Convert.ToString(txtEbTotal.AccessibleName))
			{
				calcValue = ((EBTotal > 0.0) ? (EBTotal / factor) : 0.0);
				calcValue = Math.Round(calcValue, 3);
				txtEbUnit.Text = calcValue.ToString("#,##0.###");
				CalculateByEbUnit = false;
				CalculateByEbTotal = true;
			}
			else if (CalculateByEbTotal)
			{
				calcValue = ((EBTotal > 0.0) ? (EBTotal / factor) : 0.0);
				calcValue = Math.Round(calcValue, 3);
				txtEbUnit.Text = calcValue.ToString("#,##0.###");
			}
			else if (CalculateByEbUnit)
			{
				calcValue = ((EBUnit > 0.0) ? (EBUnit * factor) : 0.0);
				calcValue = Math.Round(calcValue, 3);
				txtEbTotal.Text = calcValue.ToString("#,##0.###");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateGrossWeight()
	{
		try
		{
			decimal num = Conversion.ToDecimal((object)txtLoadingQty.Text);
			decimal PackEquivalent = ((Conversion.ToInt(CmbPackUom.Value) > 0 && ((UltraGridBase)CmbPackUom).ActiveRow != null) ? Conversion.ToDecimal(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value) : 0m);
			decimal weight = num * PackEquivalent;
			txtGrossWeight.Text = weight.ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CalculateNetWeight()
	{
		try
		{
			decimal num = Conversion.ToDecimal((object)txtGrossWeight.Text);
			decimal ebTotal = Conversion.ToDecimal((object)txtEbTotal.Text);
			decimal addLessWeight = Conversion.ToDecimal((object)txtAddLessWeight.Text);
			decimal netWeight = num - ebTotal + addLessWeight;
			txtNetWeight.Text = netWeight.ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbPackUom_Leave(object sender, EventArgs e)
	{
		try
		{
			EbCalculations();
			CalculateGrossWeight();
			CalculateNetWeight();
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
			EbCalculations();
			CalculateGrossWeight();
			CalculateNetWeight();
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
			EbCalculations();
			CalculateNetWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtebu_TextChanged(object sender, EventArgs e)
	{
		try
		{
			EbCalculations();
			CalculateNetWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEmptyBagsTotal_TextChanged(object sender, EventArgs e)
	{
		try
		{
			EbCalculations();
			CalculateNetWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtadlswt_TextChanged(object sender, EventArgs e)
	{
		try
		{
			EbCalculations();
			CalculateNetWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateTotalFreight()
	{
		try
		{
			decimal num = Conversion.ToDecimal((object)txtBiltyFreight.Text);
			decimal OtherCharges = Conversion.ToDecimal((object)txtOtherCharges.Text);
			decimal TotalFreight = num + OtherCharges;
			txtTotalFreight.Text = TotalFreight.ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtBiltyFreight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateTotalFreight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOtherCharges_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateTotalFreight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtBiltyFreight_Leave(object sender, EventArgs e)
	{
		try
		{
			txtBiltyFreight.Text = Conversion.ToDecimal((object)txtBiltyFreight.Text).ToString("#,##0.####");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOtherCharges_Leave(object sender, EventArgs e)
	{
		try
		{
			txtOtherCharges.Text = Conversion.ToDecimal((object)txtOtherCharges.Text).ToString("#,##0.####");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtLoadWeight_Leave(object sender, EventArgs e)
	{
		try
		{
			txtLoadWeight.Text = Conversion.ToDecimal((object)txtLoadWeight.Text).ToString("#,##0.####");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtTareWeight_Leave(object sender, EventArgs e)
	{
		try
		{
			txtTareWeight.Text = Conversion.ToDecimal((object)txtTareWeight.Text).ToString("#,##0.####");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtLoadWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateHeaderWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtTareWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateHeaderWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateHeaderWeight()
	{
		try
		{
			decimal num = Conversion.ToDecimal((object)txtLoadWeight.Text);
			decimal TareWeight = Conversion.ToDecimal((object)txtTareWeight.Text);
			decimal NetWeight = Math.Abs(num - TareWeight);
			txtWbNetWeight.Text = NetWeight.ToString("#,##0.###");
			if (string.IsNullOrEmpty(txtWbNetWeight.AccessibleDescription))
			{
				txtWbNetWeight.AccessibleDescription = NetWeight.ToString("#,##0.###");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtWbNetWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			double NetWeight = Conversion.ToDouble((object)txtWbNetWeight.Text);
			if (NetWeight != Conversion.ToDouble((object)txtWbNetWeight.AccessibleDescription))
			{
				txtLoadWeight.Text = string.Empty;
				txtTareWeight.Text = string.Empty;
			}
			txtWbNetWeight.Text = NetWeight.ToString("#,##0.###");
			txtWbNetWeight.AccessibleDescription = NetWeight.ToString("#,##0.###");
			double AvgUom = GridEX_Helper.GetColumnAvg(grdDetail, "PackUomEquivalent");
			if (Conversion.ToDouble((object)txtVehicleQty.Text) == 0.0)
			{
				txtVehicleQty.Text = ((AvgUom > 0.0) ? (NetWeight / AvgUom) : 0.0).ToString("#,##0.###");
			}
			double qty = Conversion.ToDouble((object)txtVehicleQty.Text);
			txtAvgFillingPerBag.Text = ((qty > 0.0) ? (NetWeight / qty) : 0.0).ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachments_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		OpenPrint(RecId);
	}

	private void OpenPrint(int PrintId)
	{
		try
		{
			if (PrintId > 0)
			{
				CrystalReportPrint_Helper.GrnSupplierLoadingSlip_1054(PrintId);
				return;
			}
			throw new Exception("No Data found to display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrintLoadingChallan_Click(object sender, EventArgs e)
	{
		OpenChallanPrint(RecId);
	}

	private void OpenChallanPrint(int PrintId)
	{
		try
		{
			if (PrintId > 0)
			{
				CrystalReportPrint_Helper.GrnSupplierLoadingChallanSlip_1054_01(PrintId);
				return;
			}
			throw new Exception("No Data found to display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbParentItemHistory_Leave(object sender, EventArgs e)
	{
		try
		{
			BindDropdownsAgainstParentCategory();
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
			((Control)(object)CmbItemNameHistory).Text = string.Empty;
			((Control)(object)CmbCommissionAgentHistory).Text = string.Empty;
			((Control)(object)CmbBuyerNameHistory).Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryComboDbCall();
			if (((Control)(object)CmbParentItemHistory).Text != null && ((Control)(object)CmbParentItemHistory).Text != string.Empty)
			{
				BindDropdownsAgainstParentCategory();
			}
			else
			{
				HistoryComboBind(dtHistoryCombo);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryComboBind(DataTable dtcombo)
	{
		try
		{
			((UltraGridBase)CmbParentItemHistory).DataSource = null;
			if (dtcombo == null || dtcombo.Rows.Count == 0)
			{
				return;
			}
			DataTable obj = new DataTable
			{
				Columns = 
				{
					{
						"Id",
						typeof(int)
					},
					"Name"
				}
			};
			DataTable dtCA = obj.Clone();
			DataTable dtBuyer = obj.Clone();
			DataTable dtParentCategory = obj.Clone();
			DataTable dtItem = obj.Clone();
			DataTable dtMostUsedPC = obj.Clone();
			DataTable dtDeliveryParty = obj.Clone();
			DataTable dtDeliverAddress = obj.Clone();
			Dictionary<string, List<DataRow>> lookup = (from r in dtcombo.AsEnumerable()
				group r by r["Activity"]?.ToString() ?? "").ToDictionary((IGrouping<string, DataRow> g) => g.Key, (IGrouping<string, DataRow> g) => g.ToList());
			int mostUsedId = 0;
			if (lookup.TryGetValue("GetMostUsedParentCategoryId", out var mostList) && mostList.Count > 0)
			{
				DataRow r3 = mostList[0];
				mostUsedId = Conversion.ToInt(r3["Id"]);
				dtMostUsedPC.Rows.Add(r3["Id"], r3["ReferenceName"]);
			}
			dtCA.BeginLoadData();
			dtBuyer.BeginLoadData();
			dtItem.BeginLoadData();
			dtParentCategory.BeginLoadData();
			dtDeliveryParty.BeginLoadData();
			dtDeliverAddress.BeginLoadData();
			if (lookup.TryGetValue("ParentCategories", out var parents))
			{
				foreach (DataRow r2 in parents)
				{
					dtParentCategory.LoadDataRow(new object[2]
					{
						r2["Id"],
						r2["ReferenceName"]
					}, fAcceptChanges: true);
				}
			}
			LoadFiltered("CommissionAgent", dtCA);
			LoadFiltered("SupplierName", dtBuyer);
			LoadFiltered("Item", dtItem);
			LoadFiltered("DeliveryToParty", dtDeliveryParty);
			LoadFiltered("DeliverToAddress", dtDeliverAddress);
			dtCA.EndLoadData();
			dtBuyer.EndLoadData();
			dtItem.EndLoadData();
			dtParentCategory.EndLoadData();
			dtDeliveryParty.EndLoadData();
			dtDeliverAddress.EndLoadData();
			InfragisticsHelper.BindAndRetainSelection(CmbCommissionAgentHistory, dtCA, "Id", "Name", "Commission Agent", AllColumns: true, Conversion.ToInt(CmbCommissionAgentHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbBuyerNameHistory, dtBuyer, "Id", "Name", "Supplier", AllColumns: true, Conversion.ToInt(CmbBuyerNameHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbItemNameHistory, dtItem, "Id", "Name", "Item Name", AllColumns: true, Conversion.ToInt(CmbItemNameHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbDeliveryToPartyHistory, dtDeliveryParty, "Id", "Name", "Deliver To Party", AllColumns: true, Conversion.ToInt(CmbDeliveryToPartyHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbDeliverToAddressHistory, dtDeliverAddress, "Id", "Name", "Deliver To Address", AllColumns: true, Conversion.ToInt(CmbDeliverToAddressHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			DDL.BindDDL(dtParentCategory, CmbParentItemHistory, "Id", "Name", "Parent Categories", ZeroIndex: false);
			((UltraGridBase)CmbParentItemHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			UltraGridColumn obj2 = ((UltraGridBase)CmbParentItemHistory).DisplayLayout.Bands[0].Columns.Add();
			((KeyedSubObjectBase)obj2).Key = "Selected";
			((HeaderBase)obj2.Header).Caption = string.Empty;
			obj2.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
			obj2.DataType = typeof(bool);
			((HeaderBase)obj2.Header).VisiblePosition = 0;
			CmbParentItemHistory.LimitToList = false;
			CmbParentItemHistory.CheckedListSettings.CheckStateMember = "Selected";
			((EditorCheckedListSettings)CmbParentItemHistory.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
			((EditorCheckedListSettings)CmbParentItemHistory.CheckedListSettings).ListSeparator = ",";
			((EditorCheckedListSettings)CmbParentItemHistory.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
			if (dtMostUsedPC.Rows.Count > 0)
			{
				((Control)(object)CmbParentItemHistory).Text = Conversion.ToString(dtMostUsedPC.Rows[0]["Name"]);
			}
			void LoadFiltered(string key, DataTable target)
			{
				if (!lookup.TryGetValue(key, out var list))
				{
					return;
				}
				foreach (DataRow r4 in list)
				{
					if (Conversion.ToInt(r4["ParentCategoryId"]) == mostUsedId)
					{
						target.LoadDataRow(new object[2]
						{
							r4["Id"],
							r4["ReferenceName"]
						}, fAcceptChanges: true);
					}
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindDropdownsAgainstParentCategory()
	{
		try
		{
			string ParentIds = "";
			if (!string.IsNullOrWhiteSpace(((Control)(object)CmbParentItemHistory).Text))
			{
				DataTable dt2 = (DataTable)((UltraGridBase)CmbParentItemHistory).DataSource;
				string[] array = ((Control)(object)CmbParentItemHistory).Text.Trim().Split(',');
				foreach (string name in array)
				{
					DataRow[] dr = dt2.Select("name='" + Conversion.ToString((object)name) + "'");
					if (dr.Length != 0)
					{
						ParentIds = ParentIds + Conversion.ToString(dr[0]["Id"]) + ",";
					}
				}
			}
			if (ParentIds.EndsWith(","))
			{
				ParentIds = ParentIds.TrimEnd(',');
			}
			DataTable dtcombo = dtHistoryCombo;
			DataTable dtBase = new DataTable();
			dtBase.Columns.Add("Id", typeof(int));
			dtBase.Columns.Add("Name");
			DataTable dtCA = dtBase.Clone();
			DataTable dtBuyer = dtBase.Clone();
			DataTable dtItem = dtBase.Clone();
			DataTable dtDeliveryParty = dtBase.Clone();
			DataTable dtDeliverAddress = dtBase.Clone();
			HashSet<int> CaIds = new HashSet<int>();
			HashSet<int> BuyerIds = new HashSet<int>();
			HashSet<int> DeliveryPartyIds = new HashSet<int>();
			HashSet<int> DeliverAddressIds = new HashSet<int>();
			Dictionary<string, Action<DataRow>> map = new Dictionary<string, Action<DataRow>>(StringComparer.OrdinalIgnoreCase)
			{
				["CommissionAgent"] = delegate(DataRow r)
				{
					int num4 = Conversion.ToInt(r["Id"]);
					if (CaIds.Add(num4))
					{
						dtCA.Rows.Add(num4, r["ReferenceName"]);
					}
				},
				["SupplierName"] = delegate(DataRow r)
				{
					int num3 = Conversion.ToInt(r["Id"]);
					if (BuyerIds.Add(num3))
					{
						dtBuyer.Rows.Add(num3, r["ReferenceName"]);
					}
				},
				["Item"] = delegate(DataRow r)
				{
					dtItem.Rows.Add(r["Id"], r["ReferenceName"]);
				},
				["DeliveryToParty"] = delegate(DataRow r)
				{
					int num2 = Conversion.ToInt(r["Id"]);
					if (DeliveryPartyIds.Add(num2))
					{
						dtDeliveryParty.Rows.Add(num2, r["ReferenceName"]);
					}
				},
				["DeliverToAddress"] = delegate(DataRow r)
				{
					int num = Conversion.ToInt(r["Id"]);
					if (DeliverAddressIds.Add(num))
					{
						dtDeliverAddress.Rows.Add(num, r["ReferenceName"]);
					}
				}
			};
			if (!string.IsNullOrEmpty(ParentIds))
			{
				HashSet<string> parentSet = new HashSet<string>(ParentIds.Split(','));
				foreach (DataRow row in dtcombo.Rows)
				{
					string parentId = row["ParentCategoryId"].ToString();
					string activity = row["Activity"].ToString();
					if (parentSet.Contains(parentId) && map.TryGetValue(activity, out var action))
					{
						action(row);
					}
				}
			}
			else
			{
				foreach (DataRow row2 in dtcombo.Rows)
				{
					string activity2 = row2["Activity"].ToString();
					if (map.TryGetValue(activity2, out var action2))
					{
						action2(row2);
					}
				}
			}
			Bind(CmbCommissionAgentHistory, dtCA, "Commission Agent");
			Bind(CmbBuyerNameHistory, dtBuyer, "Supplier");
			Bind(CmbItemNameHistory, dtItem, "Item Name");
			Bind(CmbDeliveryToPartyHistory, dtDeliveryParty, "Deliver To Party");
			Bind(CmbDeliverToAddressHistory, dtDeliverAddress, "Deliver To Address");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
		static void Bind(UltraCombo combo, DataTable dt, string caption)
		{
			InfragisticsHelper.BindAndRetainSelection(combo, dt, "Id", "Name", caption, AllColumns: true, Conversion.ToInt(combo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryFill();
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
			obj.CompanyId = UserAccount.CompanyId;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = DocumentTypeId;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			obj.EntryUser = UserAccount.ID;
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
			if (datValidityFromHistory.Checked)
			{
				obj.ValidityDateFrom = datValidityFromHistory.Value;
			}
			if (datValidityToHistory.Checked)
			{
				obj.ValidityDateTo = datValidityToHistory.Value;
			}
			string ParentIds = "";
			if (((Control)(object)CmbParentItemHistory).Text != null && ((Control)(object)CmbParentItemHistory).Text != string.Empty)
			{
				DataTable dt2 = (DataTable)((UltraGridBase)CmbParentItemHistory).DataSource;
				string[] array = (Conversion.ToString((object)((Control)(object)CmbParentItemHistory).Text.Trim()) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr2 = dt2.Select("name='" + Conversion.ToString((object)author) + "'");
					if (dr2.Length != 0)
					{
						ParentIds = ParentIds + Conversion.ToString(dr2[0].ItemArray[0]) + ",";
					}
				}
			}
			obj.ParentCategoryIds = ParentIds;
			obj.ItemId = Conversion.ToInt(CmbItemNameHistory.Value);
			obj.CommissionAgentId = Conversion.ToInt(CmbCommissionAgentHistory.Value);
			obj.SupplierId = Conversion.ToInt(CmbBuyerNameHistory.Value);
			obj.DeliverToPartyId = Conversion.ToInt(CmbDeliveryToPartyHistory.Value);
			obj.ShipToAddress = Conversion.ToString((object)((Control)(object)CmbDeliverToAddressHistory).Text);
			DataTable dtHistory = grnSupplierLoadingMaster.FormHistory(obj);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("CommissionAgent", typeof(string));
				dt.Columns.Add("SupplierName", typeof(string));
				dt.Columns.Add("LoadingCity", typeof(string));
				dt.Columns.Add("UnloadingCity", typeof(string));
				dt.Columns.Add("BiltyFreight", typeof(decimal));
				dt.Columns.Add("OtherCharges", typeof(decimal));
				dt.Columns.Add("TotalFreight", typeof(decimal));
				dt.Columns.Add("VehicleType", typeof(string));
				dt.Columns.Add("VehicleNo", typeof(string));
				dt.Columns.Add("BiltyDate", typeof(DateTime));
				dt.Columns.Add("BiltyNo", typeof(string));
				dt.Columns.Add("DeliveryTerm", typeof(string));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				dtHistoryForLayout = dt.Clone();
				foreach (DataRow row in dtHistory.Rows)
				{
					DataRow dr = dt.NewRow();
					dr["Id"] = Conversion.ToInt(row["grnSupplierLoadingMasterId"]);
					dr["DocNo"] = Conversion.ToInt(row["docNo"]);
					dr["DocDate"] = Conversion.ToDateTime(row["docDate"]).ToShortDateString();
					dr["CommissionAgent"] = Conversion.ToString(row["CommissionAgentName"]);
					dr["SupplierName"] = Conversion.ToString(row["SupplierName"]);
					dr["LoadingCity"] = Conversion.ToString(row["LoadingCity"]);
					dr["UnloadingCity"] = Conversion.ToString(row["UnloadingCity"]);
					dr["BiltyFreight"] = Conversion.ToString(row["biltyFreight"]);
					dr["OtherCharges"] = Conversion.ToString(row["otherAdLesCharges"]);
					dr["TotalFreight"] = Conversion.ToString(row["totalFreight"]);
					dr["VehicleType"] = Conversion.ToString(row["VehicleType"]);
					dr["VehicleNo"] = Conversion.ToString(row["VehicleNo"]);
					dr["BiltyDate"] = Conversion.ToDateTime(row["BiltyDate"]);
					dr["BiltyNo"] = Conversion.ToString(row["BiltyNo"]);
					dr["DeliveryTerm"] = Conversion.ToString(row["DeliveryTerm"]);
					dr["EntryUser"] = Conversion.ToString(row["EntryUserName"]);
					dr["EntryDate"] = Conversion.ToDateTime(row["EntryDate"]);
					dr["ModifyUser"] = Conversion.ToString(row["ModifyUserName"]);
					dr["ModifyDate"] = row["ModifyDate"];
					dr["NoOfAttachments"] = Conversion.ToInt(row["NoOfAttachments"]);
					dt.Rows.Add(dr);
				}
				dtHistoryForLayout = dt.Copy();
				grdHistory.DataSource = dt;
				grdHistory.RetrieveStructure();
				GridSettingHistory();
			}
			else
			{
				grdHistory.DataSource = null;
				grdHistoryDetail.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridSettingHistory()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id" };
			List<string> NotTotalColumns = new List<string> { "BuyerTargetPrice", "TargetPurchasePrice" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistory, 3, 3, hiddenColumns, null, null, ColumnSetting: true, BracketsForamt: false, NotTotalColumns);
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yyyy";
			grdHistory.RootTable.Columns["BiltyDate"].FormatString = "dd-MMM-yyyy";
			grdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant + 10;
			grdHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant + 15;
			grdHistory.RootTable.Columns["CommissionAgent"].Width = Constants.InventoryConstants.SupplierName + 20;
			grdHistory.RootTable.Columns["SupplierName"].Width = Constants.InventoryConstants.SupplierName + 20;
			grdHistory.RootTable.Columns["LoadingCity"].Width = Constants.InventoryConstants.CityName;
			grdHistory.RootTable.Columns["UnloadingCity"].Width = Constants.InventoryConstants.CityName;
			grdHistory.RootTable.Columns["BiltyFreight"].Width = Constants.InventoryConstants.AmountConstant - 15;
			grdHistory.RootTable.Columns["OtherCharges"].Width = Constants.InventoryConstants.AmountConstant - 15;
			grdHistory.RootTable.Columns["TotalFreight"].Width = Constants.InventoryConstants.AmountConstant - 15;
			grdHistory.RootTable.Columns["VehicleType"].Width = Constants.InventoryConstants.VehicleTypeConstant;
			grdHistory.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grdHistory.RootTable.Columns["BiltyDate"].Width = Constants.InventoryConstants.DateConstant + 15;
			grdHistory.RootTable.Columns["BiltyNo"].Width = Constants.InventoryConstants.BiltyNoConstant;
			grdHistory.RootTable.Columns["DeliveryTerm"].Width = Constants.InventoryConstants.TermConstant;
			grdHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grdHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grdHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			GridEX_Helper.AddButton(grdHistory, "Edit", "Edit", 40, 0);
			GridEX_Helper.AddButton(grdHistory, "Print", "Print", 40, 2);
			GridEX_Helper.AddButton(grdHistory, "PrintChallan", "Print Challan", 80, 2);
			grdHistory.FrozenColumns = 3;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				ReadById(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					ReadById(Id);
				}
				else if (e.Column.Key == "Print")
				{
					OpenPrint(Id);
				}
				else if (e.Column.Key == "PrintChallan")
				{
					OpenChallanPrint(Id);
				}
			}
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
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), DocumentTypeId);
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
			GridEXRow row = grdHistory.CurrentRow;
			if (row != null && (int)row.RowType == 0 && row.Cells.Count > 0)
			{
				GetDetailGrdByHeadId(Conversion.ToInt(row.Cells["Id"].Value));
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
			grnSupplierLoadingMaster Obj = grnSupplierLoadingMaster.ReadById(Id);
			if (Obj != null && Obj.grnSupplierLoadingDetailList.Count == 0)
			{
				grdHistoryDetail.DataSource = null;
				return;
			}
			DataTable dtDetailHistory = dtDetail.Clone();
			GrnLoadingChallan_Helper.FillDetailtableFromListCommonForReadById(Obj, dtDetailHistory);
			grdHistoryDetail.DataSource = dtDetailHistory;
			grdHistoryDetail.RetrieveStructure();
			grddetailhistorySettings();
			DataTable dtEb = dtEmptyBags.Clone();
			dtEb.Columns.Add("PackType", typeof(string));
			dtEb.Columns.Add("EmptyBagItemName", typeof(string));
			GrnLoadingChallan_Helper.FillEmptyBagTableFromListCommonForReadById(Obj, dtEb, AddExtraColumns: true);
			grdEmptyBagHistory.DataSource = dtEb;
			grdEmptyBagHistory.RetrieveStructure();
			gridEmptyBagsSettings(grdEmptyBagHistory);
			grdEmptyBagHistory.RootTable.Columns["PackingType"].Visible = false;
			grdEmptyBagHistory.RootTable.Columns["EmptyBagItem"].Visible = false;
			DataTable dtEx = dtExpGrid.Clone();
			dtEx.Columns.Add("OtherItemName", typeof(string));
			GrnLoadingChallan_Helper.FillExpenseTableFromListCommonForReadById(Obj, dtEx, AddExtraColumns: true);
			grdExpenseHistory.DataSource = dtEx;
			grdExpenseHistory.RetrieveStructure();
			grdInvExpSettings(grdExpenseHistory);
			grdExpenseHistory.RootTable.Columns["ItemId"].Visible = false;
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
			grdDetailSetting(grdHistoryDetail);
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
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdHistory, dtHistoryForLayout);
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdDetail, dtDetail);
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdInvExp, dtExpGrid);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdEmptyBags, dtEmptyBags);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmGrnLoadingChallanCmagt_KeyDown(object sender, KeyEventArgs e)
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
					datDocDate.Focus();
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
				if (e.Control && e.Shift && e.KeyCode == Keys.Delete)
				{
					if (formright.DoHaveCanDelete && btnDelete.Enabled)
					{
						btnDelete_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.S)
				{
					if (btnsave.Visible && btnsave.Enabled)
					{
						btnsave_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.U)
				{
					if (btnUpdate.Visible && btnUpdate.Enabled)
					{
						btnUpdate_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.P)
				{
					if (btnPrint.Visible && btnPrint.Enabled)
					{
						btnPrint_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.R)
				{
					BtnRefresh_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.F5)
				{
					datDocDate.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					CmbParentItem.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					datDocDate.Focus();
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdHistory).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.S)
				{
					btnshow_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Up)
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

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Save When on Entry form and For Show Data when on History Form");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Inquiry Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Parent Item");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On on Inquiry Date");
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

	private void grdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count == 0)
			{
				return;
			}
			int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdHistory.CurrentColumn.Key == "Edit")
				{
					if (!formright.DoHaveUpdateRights)
					{
						throw new Exception("you don't have update rights...");
					}
					ReadById(Conversion.ToInt((object)CurrId));
				}
				if (grdHistory.CurrentColumn.Key == "Print")
				{
					if (!formright.DoHavePrintRights)
					{
						throw new Exception("you don't have print rights...");
					}
					OpenPrint(CurrId);
				}
				if (grdHistory.CurrentColumn.Key == "PrintChallan")
				{
					if (!formright.DoHavePrintRights)
					{
						throw new Exception("you don't have print rights...");
					}
					OpenChallanPrint(CurrId);
				}
				if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CurrId, DocumentTypeId);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("you don't have update rights...");
				}
				ReadById(Conversion.ToInt((object)CurrId));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDefineShipToAddress_Click(object sender, EventArgs e)
	{
		try
		{
			using SupfrmShipToAddress obj = new SupfrmShipToAddress(UserAccount);
			obj.ShowDialog();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnLoadOrder_Click(object sender, EventArgs e)
	{
		try
		{
			int alreadySelectedOrderId = Conversion.ToInt(CmbPoNoDetail.Value);
			using frmLoadPurchaseOrderForGrnLoading loadGp = new frmLoadPurchaseOrderForGrnLoading(UserAccount);
			loadGp.AlreadySelectedId = alreadySelectedOrderId;
			loadGp.ShowDialog();
			if (loadGp.dtLoader != null && loadGp.dtLoader.Rows.Count > 0)
			{
				int SupplierId = Conversion.ToInt(loadGp.dtLoader.Rows[0]["SupplierId"]);
				if (Conversion.ToInt(CmbSupplierName.Value) > 0 && Conversion.ToInt(CmbSupplierName.Value) != SupplierId)
				{
					throw new Exception("you can only load order of supplier:" + ((Control)(object)CmbSupplierName).Text);
				}
				int newSelectedOrderId = Conversion.ToInt(loadGp.dtLoader.Rows[0]["purchaseOrderMasterId"]);
				if (alreadySelectedOrderId != newSelectedOrderId)
				{
					CmbPoNoDetail.Value = newSelectedOrderId;
				}
				CmbPoNoDetail.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void datDocDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			txtBiltyDate.Value = datDocDate.Value;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtVehicleQty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			decimal qty = Conversion.ToDecimal((object)txtVehicleQty.Text);
			txtAvgFillingPerBag.Text = ((qty > 0m) ? (Conversion.ToDecimal((object)txtWbNetWeight.Text) / qty) : 0m).ToString("#,##0.###");
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
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Expected O, but got Unknown
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Expected O, but got Unknown
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Expected O, but got Unknown
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Expected O, but got Unknown
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Expected O, but got Unknown
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c8: Expected O, but got Unknown
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Expected O, but got Unknown
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d6: Expected O, but got Unknown
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Expected O, but got Unknown
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Expected O, but got Unknown
		//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Expected O, but got Unknown
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Expected O, but got Unknown
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f9: Expected O, but got Unknown
		//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0600: Expected O, but got Unknown
		//IL_0600: Unknown result type (might be due to invalid IL or missing references)
		//IL_0607: Expected O, but got Unknown
		//IL_0607: Unknown result type (might be due to invalid IL or missing references)
		//IL_060e: Expected O, but got Unknown
		//IL_060e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0615: Expected O, but got Unknown
		//IL_0615: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Expected O, but got Unknown
		//IL_061c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0623: Expected O, but got Unknown
		//IL_0623: Unknown result type (might be due to invalid IL or missing references)
		//IL_062a: Expected O, but got Unknown
		//IL_062a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Expected O, but got Unknown
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_0638: Expected O, but got Unknown
		//IL_0638: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Expected O, but got Unknown
		//IL_063f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0646: Expected O, but got Unknown
		//IL_0646: Unknown result type (might be due to invalid IL or missing references)
		//IL_064d: Expected O, but got Unknown
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
		//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c7: Expected O, but got Unknown
		//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ce: Expected O, but got Unknown
		//IL_06ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Expected O, but got Unknown
		//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dc: Expected O, but got Unknown
		//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e3: Expected O, but got Unknown
		//IL_06e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ea: Expected O, but got Unknown
		//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f1: Expected O, but got Unknown
		//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f8: Expected O, but got Unknown
		//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ff: Expected O, but got Unknown
		//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0708: Expected O, but got Unknown
		//IL_070a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0713: Expected O, but got Unknown
		//IL_0715: Unknown result type (might be due to invalid IL or missing references)
		//IL_071e: Expected O, but got Unknown
		//IL_0720: Unknown result type (might be due to invalid IL or missing references)
		//IL_0729: Expected O, but got Unknown
		//IL_072b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0734: Expected O, but got Unknown
		//IL_0736: Unknown result type (might be due to invalid IL or missing references)
		//IL_073f: Expected O, but got Unknown
		//IL_0741: Unknown result type (might be due to invalid IL or missing references)
		//IL_074a: Expected O, but got Unknown
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0755: Expected O, but got Unknown
		//IL_0757: Unknown result type (might be due to invalid IL or missing references)
		//IL_0760: Expected O, but got Unknown
		//IL_0762: Unknown result type (might be due to invalid IL or missing references)
		//IL_076b: Expected O, but got Unknown
		//IL_076d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0776: Expected O, but got Unknown
		//IL_0778: Unknown result type (might be due to invalid IL or missing references)
		//IL_0781: Expected O, but got Unknown
		//IL_0783: Unknown result type (might be due to invalid IL or missing references)
		//IL_078c: Expected O, but got Unknown
		//IL_078e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0797: Expected O, but got Unknown
		//IL_0799: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a2: Expected O, but got Unknown
		//IL_07a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ad: Expected O, but got Unknown
		//IL_07af: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b8: Expected O, but got Unknown
		//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c3: Expected O, but got Unknown
		//IL_07c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ce: Expected O, but got Unknown
		//IL_07d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Expected O, but got Unknown
		//IL_07db: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e4: Expected O, but got Unknown
		//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ef: Expected O, but got Unknown
		//IL_07f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fa: Expected O, but got Unknown
		//IL_07fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0805: Expected O, but got Unknown
		//IL_0807: Unknown result type (might be due to invalid IL or missing references)
		//IL_0810: Expected O, but got Unknown
		//IL_0812: Unknown result type (might be due to invalid IL or missing references)
		//IL_081b: Expected O, but got Unknown
		//IL_081d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0826: Expected O, but got Unknown
		//IL_0828: Unknown result type (might be due to invalid IL or missing references)
		//IL_0831: Expected O, but got Unknown
		//IL_0833: Unknown result type (might be due to invalid IL or missing references)
		//IL_083c: Expected O, but got Unknown
		//IL_083e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0847: Expected O, but got Unknown
		//IL_0849: Unknown result type (might be due to invalid IL or missing references)
		//IL_0852: Expected O, but got Unknown
		//IL_0854: Unknown result type (might be due to invalid IL or missing references)
		//IL_085d: Expected O, but got Unknown
		//IL_085f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0868: Expected O, but got Unknown
		//IL_086a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0873: Expected O, but got Unknown
		//IL_0875: Unknown result type (might be due to invalid IL or missing references)
		//IL_087e: Expected O, but got Unknown
		//IL_0880: Unknown result type (might be due to invalid IL or missing references)
		//IL_0889: Expected O, but got Unknown
		//IL_088b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0894: Expected O, but got Unknown
		//IL_0896: Unknown result type (might be due to invalid IL or missing references)
		//IL_089f: Expected O, but got Unknown
		//IL_08a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08aa: Expected O, but got Unknown
		//IL_08ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b5: Expected O, but got Unknown
		//IL_08b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c0: Expected O, but got Unknown
		//IL_08c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cb: Expected O, but got Unknown
		//IL_08cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d6: Expected O, but got Unknown
		//IL_08d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e1: Expected O, but got Unknown
		//IL_08e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ec: Expected O, but got Unknown
		//IL_08ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f7: Expected O, but got Unknown
		//IL_08f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0902: Expected O, but got Unknown
		//IL_0904: Unknown result type (might be due to invalid IL or missing references)
		//IL_090d: Expected O, but got Unknown
		//IL_090f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0918: Expected O, but got Unknown
		//IL_091a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0923: Expected O, but got Unknown
		//IL_0925: Unknown result type (might be due to invalid IL or missing references)
		//IL_092e: Expected O, but got Unknown
		//IL_0930: Unknown result type (might be due to invalid IL or missing references)
		//IL_0939: Expected O, but got Unknown
		//IL_093b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0944: Expected O, but got Unknown
		//IL_095d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0967: Expected O, but got Unknown
		//IL_097e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0988: Expected O, but got Unknown
		//IL_09b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bf: Expected O, but got Unknown
		//IL_0a23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2d: Expected O, but got Unknown
		//IL_0a44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4e: Expected O, but got Unknown
		//IL_0a70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7a: Expected O, but got Unknown
		//IL_0a9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa6: Expected O, but got Unknown
		//IL_0aa7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab1: Expected O, but got Unknown
		//IL_0ab2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abc: Expected O, but got Unknown
		//IL_0ac8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad2: Expected O, but got Unknown
		//IL_0ad3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0add: Expected O, but got Unknown
		//IL_0ade: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae8: Expected O, but got Unknown
		//IL_0b0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b14: Expected O, but got Unknown
		//IL_0c75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7f: Expected O, but got Unknown
		//IL_0c8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c95: Expected O, but got Unknown
		//IL_0da9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db3: Expected O, but got Unknown
		//IL_0dca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd4: Expected O, but got Unknown
		//IL_0dd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ddf: Expected O, but got Unknown
		//IL_0e0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e16: Expected O, but got Unknown
		//IL_0e17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e21: Expected O, but got Unknown
		//IL_0e43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4d: Expected O, but got Unknown
		//IL_0e6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e79: Expected O, but got Unknown
		//IL_0efe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f08: Expected O, but got Unknown
		//IL_0f1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f29: Expected O, but got Unknown
		//IL_0f4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f55: Expected O, but got Unknown
		//IL_0f82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8c: Expected O, but got Unknown
		//IL_0fe5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fef: Expected O, but got Unknown
		//IL_0ffb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1005: Expected O, but got Unknown
		//IL_1048: Unknown result type (might be due to invalid IL or missing references)
		//IL_1052: Expected O, but got Unknown
		//IL_105e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1068: Expected O, but got Unknown
		//IL_10a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_10aa: Expected O, but got Unknown
		//IL_10b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c0: Expected O, but got Unknown
		//IL_167b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1685: Expected O, but got Unknown
		//IL_18b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_18bc: Expected O, but got Unknown
		//IL_18c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d3: Expected O, but got Unknown
		//IL_1bfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c05: Expected O, but got Unknown
		//IL_af8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_af96: Expected O, but got Unknown
		//IL_afba: Unknown result type (might be due to invalid IL or missing references)
		//IL_afc4: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInvExp_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdEmptyBags_DesignTimeLayout = new GridEXLayout();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance23 = new Appearance();
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
		Appearance appearance168 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance278 = new Appearance();
		Appearance appearance280 = new Appearance();
		Appearance appearance281 = new Appearance();
		Appearance appearance282 = new Appearance();
		Appearance appearance283 = new Appearance();
		Appearance appearance284 = new Appearance();
		Appearance appearance285 = new Appearance();
		Appearance appearance286 = new Appearance();
		Appearance appearance287 = new Appearance();
		Appearance appearance288 = new Appearance();
		Appearance appearance289 = new Appearance();
		Appearance appearance291 = new Appearance();
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
		Appearance appearance199 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance213 = new Appearance();
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
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance292 = new Appearance();
		Appearance appearance293 = new Appearance();
		Appearance appearance294 = new Appearance();
		Appearance appearance295 = new Appearance();
		Appearance appearance296 = new Appearance();
		Appearance appearance297 = new Appearance();
		Appearance appearance298 = new Appearance();
		Appearance appearance299 = new Appearance();
		Appearance appearance300 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance252 = new Appearance();
		Appearance appearance253 = new Appearance();
		Appearance appearance254 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance256 = new Appearance();
		Appearance appearance258 = new Appearance();
		Appearance appearance272 = new Appearance();
		Appearance appearance276 = new Appearance();
		Appearance appearance277 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance248 = new Appearance();
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
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance141 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Cmagt.frmGrnLoadingChallanCmagt));
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdExpenseHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdEmptyBagHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance255 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance265 = new Appearance();
		Appearance appearance266 = new Appearance();
		Appearance appearance267 = new Appearance();
		Appearance appearance269 = new Appearance();
		Appearance appearance270 = new Appearance();
		Appearance appearance271 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance273 = new Appearance();
		Appearance appearance274 = new Appearance();
		Appearance appearance275 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance268 = new Appearance();
		Appearance appearance279 = new Appearance();
		Appearance appearance290 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance259 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance260 = new Appearance();
		Appearance appearance261 = new Appearance();
		Appearance appearance262 = new Appearance();
		Appearance appearance263 = new Appearance();
		Appearance appearance264 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabForm = new System.Windows.Forms.TabPage();
		this.grdDetail = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdInvExp = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label22 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdEmptyBags = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label27 = new System.Windows.Forms.Label();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel14 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox6 = new System.Windows.Forms.GroupBox();
		this.txtWarningRemarks = new System.Windows.Forms.TextBox();
		this.label53 = new System.Windows.Forms.Label();
		this.CmbDeliveryToParty = new UltraCombo();
		this.label52 = new System.Windows.Forms.Label();
		this.txtDeliveryToAddress = new System.Windows.Forms.TextBox();
		this.CmbShipToAddress = new UltraCombo();
		this.txtSaleOrderNo = new System.Windows.Forms.TextBox();
		this.BtnLoadOrder = new System.Windows.Forms.Button();
		this.btnDefineShipToAddress = new System.Windows.Forms.Button();
		this.CmbPoNoDetail = new UltraCombo();
		this.txtPoTotalWeightDetail = new System.Windows.Forms.TextBox();
		this.txtPoAlreadyReceivedWeightDetail = new System.Windows.Forms.TextBox();
		this.txtPoBalWeightDetail = new System.Windows.Forms.TextBox();
		this.CmbEmptyBagTerm = new UltraCombo();
		this.CmbPackingType = new UltraCombo();
		this.CmbPackUom = new UltraCombo();
		this.txtLoadingQty = new System.Windows.Forms.TextBox();
		this.CmbParentItem = new UltraCombo();
		this.CmbItemName = new UltraCombo();
		this.CmbCropYear = new UltraCombo();
		this.label47 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.txtEbUnit = new System.Windows.Forms.TextBox();
		this.CmbBuyerNameDetail = new UltraCombo();
		this.label35 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.txtAddLessWeight = new System.Windows.Forms.TextBox();
		this.label34 = new System.Windows.Forms.Label();
		this.txtEbTotal = new System.Windows.Forms.TextBox();
		this.label39 = new System.Windows.Forms.Label();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.label46 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.RadItemCodePackListDetail = new System.Windows.Forms.RadioButton();
		this.RadItemNamePackListDetail = new System.Windows.Forms.RadioButton();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.label51 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.BtnAdd = new System.Windows.Forms.Button();
		this.BtnCancelDetail = new System.Windows.Forms.Button();
		this.BtnUpdateDetail = new System.Windows.Forms.Button();
		this.label14 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.panel1 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label11 = new System.Windows.Forms.Label();
		this.cmbVehicleType = new UltraCombo();
		this.label49 = new System.Windows.Forms.Label();
		this.CmbDeliveryTerm = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.txtTotalFreight = new System.Windows.Forms.TextBox();
		this.label44 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtWbNetWeight = new System.Windows.Forms.TextBox();
		this.txtTareWeight = new System.Windows.Forms.TextBox();
		this.txtLoadWeight = new System.Windows.Forms.TextBox();
		this.txtVehicleQty = new System.Windows.Forms.TextBox();
		this.txtBiltyNo = new System.Windows.Forms.TextBox();
		this.txtOtherCharges = new System.Windows.Forms.TextBox();
		this.txtBiltyFreight = new System.Windows.Forms.TextBox();
		this.txtTransporterName = new System.Windows.Forms.TextBox();
		this.label40 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.txtBiltyDate = new System.Windows.Forms.DateTimePicker();
		this.datDocDate = new System.Windows.Forms.DateTimePicker();
		this.label17 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.CmbTransporter = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbUnloadingCity = new UltraCombo();
		this.CmbLoadingCity = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.CmbCommissionAgent = new UltraCombo();
		this.CmbSupplierName = new UltraCombo();
		this.txtDocNo = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label54 = new System.Windows.Forms.Label();
		this.CmbBranch = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.chkPrint = new System.Windows.Forms.CheckBox();
		this.CmbCompanyName = new UltraCombo();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnAttachments = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.BtnPrintLoadingChallan = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.tabHistory = new System.Windows.Forms.TabPage();
		this.grdHistory = new GridEX();
		this.tabControl3 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grdHistoryDetail = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label50 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdExpenseHistory = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label57 = new System.Windows.Forms.Label();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel23 = new System.Windows.Forms.Panel();
		this.grdEmptyBagHistory = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label64 = new System.Windows.Forms.Label();
		this.label58 = new System.Windows.Forms.Label();
		this.panel15 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label43 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbDeliverToAddressHistory = new UltraCombo();
		this.datValidityToHistory = new System.Windows.Forms.DateTimePicker();
		this.CmbDeliveryToPartyHistory = new UltraCombo();
		this.label59 = new System.Windows.Forms.Label();
		this.datValidityFromHistory = new System.Windows.Forms.DateTimePicker();
		this.label61 = new System.Windows.Forms.Label();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.btnshow = new System.Windows.Forms.Button();
		this.CmbItemNameHistory = new UltraCombo();
		this.label37 = new System.Windows.Forms.Label();
		this.CmbParentItemHistory = new UltraCombo();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.label36 = new System.Windows.Forms.Label();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.label32 = new System.Windows.Forms.Label();
		this.CmbBuyerNameHistory = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.CmbCommissionAgentHistory = new UltraCombo();
		this.label31 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label30 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.txtAvgFillingPerBag = new System.Windows.Forms.TextBox();
		this.label62 = new System.Windows.Forms.Label();
		this.tabControl1.SuspendLayout();
		this.tabForm.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel6.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).BeginInit();
		this.panel5.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmptyBags).BeginInit();
		this.panel7.SuspendLayout();
		this.panel14.SuspendLayout();
		this.panel2.SuspendLayout();
		this.groupBox6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryToParty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbShipToAddress).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPoNoDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbEmptyBagTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBuyerNameDetail).BeginInit();
		this.panel1.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbVehicleType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransporter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUnloadingCity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLoadingCity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionAgent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierName).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranch).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCompanyName).BeginInit();
		this.toolStrip1.SuspendLayout();
		this.tabHistory.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.tabControl3.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).BeginInit();
		this.panel11.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdExpenseHistory).BeginInit();
		this.panel12.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel23.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmptyBagHistory).BeginInit();
		this.panel16.SuspendLayout();
		this.panel15.SuspendLayout();
		this.panel13.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliverToAddressHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryToPartyHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentItemHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBuyerNameHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionAgentHistory).BeginInit();
		this.panel4.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabForm);
		this.tabControl1.Controls.Add(this.tabHistory);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1134, 697);
		this.tabControl1.TabIndex = 0;
		this.tabForm.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabForm.Controls.Add(this.panel6);
		this.tabForm.Controls.Add(this.panel14);
		this.tabForm.Controls.Add(this.panel2);
		this.tabForm.Controls.Add(this.panel1);
		this.tabForm.Controls.Add(this.panel3);
		this.tabForm.Controls.Add(this.toolStrip1);
		this.tabForm.Location = new System.Drawing.Point(4, 4);
		this.tabForm.Name = "tabForm";
		this.tabForm.Padding = new System.Windows.Forms.Padding(3);
		this.tabForm.Size = new System.Drawing.Size(1126, 668);
		this.tabForm.TabIndex = 0;
		this.tabForm.Text = "Form";
		this.tabForm.UseVisualStyleBackColor = true;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(3, 395);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1120, 116);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 132;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetail_KeyDown);
		this.panel6.Controls.Add(this.panel8);
		this.panel6.Controls.Add(this.panel10);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel6.Location = new System.Drawing.Point(3, 511);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1120, 154);
		this.panel6.TabIndex = 134;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdInvExp);
		this.panel8.Controls.Add(this.panel5);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(515, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(605, 154);
		this.panel8.TabIndex = 0;
		((ScrollableBase)this.grdInvExp).BackColor = System.Drawing.Color.White;
		grdInvExp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInvExp.DesignTimeLayout = grdInvExp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInvExp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdInvExp).Name = "grdInvExp";
		((ScrollableBase)this.grdInvExp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Size = new System.Drawing.Size(605, 132);
		((System.Windows.Forms.Control)(object)this.grdInvExp).TabIndex = 135;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Tag = "GrdExpense";
		this.grdInvExp.TotalRow = (InheritableBoolean)1;
		this.grdInvExp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInvExp).VisualStyle = (VisualStyle)4;
		this.grdInvExp.CellUpdated += new ColumnActionEventHandler(grdInvExp_CellUpdated);
		this.grdInvExp.ColumnButtonClick += new ColumnActionEventHandler(grdInvExp_ColumnButtonClick);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar3);
		this.panel5.Controls.Add(this.label22);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(605, 22);
		this.panel5.TabIndex = 134;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(565, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.grdInvExp;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 20);
		this.ctrlGrdBar3.TabIndex = 127;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.Yellow;
		this.label22.Location = new System.Drawing.Point(3, 4);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(94, 15);
		this.label22.TabIndex = 238;
		this.label22.Text = "Other Expenses";
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmptyBags);
		this.panel10.Controls.Add(this.panel7);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(515, 154);
		this.panel10.TabIndex = 1;
		((ScrollableBase)this.grdEmptyBags).BackColor = System.Drawing.Color.White;
		grdEmptyBags_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdEmptyBags.DesignTimeLayout = grdEmptyBags_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmptyBags.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Name = "grdEmptyBags";
		((ScrollableBase)this.grdEmptyBags).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Size = new System.Drawing.Size(515, 132);
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Tag = "GrdEmptyBags";
		this.grdEmptyBags.TotalRow = (InheritableBoolean)1;
		this.grdEmptyBags.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmptyBags).VisualStyle = (VisualStyle)4;
		this.grdEmptyBags.ColumnButtonClick += new ColumnActionEventHandler(grdEmptyBags_ColumnButtonClick);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label27);
		this.panel7.Controls.Add(this.ctrlGrdBar4);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(515, 22);
		this.panel7.TabIndex = 134;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.ForeColor = System.Drawing.Color.Yellow;
		this.label27.Location = new System.Drawing.Point(1, 4);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(272, 15);
		this.label27.TabIndex = 238;
		this.label27.Text = "Empty Bags Weight Deduction From Bill Weight";
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.ForeColor = System.Drawing.SystemColors.ControlText;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(474, 1);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar4.MyGrid = this.grdEmptyBags;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(38, 19);
		this.ctrlGrdBar4.TabIndex = 127;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.ctrlGrdBar2);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(3, 374);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1120, 21);
		this.panel14.TabIndex = 133;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1079, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.grdDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 20);
		this.ctrlGrdBar2.TabIndex = 127;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel2.Controls.Add(this.groupBox6);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 216);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1120, 158);
		this.panel2.TabIndex = 4;
		this.groupBox6.Controls.Add(this.txtWarningRemarks);
		this.groupBox6.Controls.Add(this.label53);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty);
		this.groupBox6.Controls.Add(this.label52);
		this.groupBox6.Controls.Add(this.txtDeliveryToAddress);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbShipToAddress);
		this.groupBox6.Controls.Add(this.txtSaleOrderNo);
		this.groupBox6.Controls.Add(this.BtnLoadOrder);
		this.groupBox6.Controls.Add(this.btnDefineShipToAddress);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPoNoDetail);
		this.groupBox6.Controls.Add(this.txtPoTotalWeightDetail);
		this.groupBox6.Controls.Add(this.txtPoAlreadyReceivedWeightDetail);
		this.groupBox6.Controls.Add(this.txtPoBalWeightDetail);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbEmptyBagTerm);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackingType);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.groupBox6.Controls.Add(this.txtLoadingQty);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentItem);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYear);
		this.groupBox6.Controls.Add(this.label47);
		this.groupBox6.Controls.Add(this.label60);
		this.groupBox6.Controls.Add(this.txtEbUnit);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBuyerNameDetail);
		this.groupBox6.Controls.Add(this.label35);
		this.groupBox6.Controls.Add(this.label24);
		this.groupBox6.Controls.Add(this.txtAddLessWeight);
		this.groupBox6.Controls.Add(this.label34);
		this.groupBox6.Controls.Add(this.txtEbTotal);
		this.groupBox6.Controls.Add(this.label39);
		this.groupBox6.Controls.Add(this.txtNetWeight);
		this.groupBox6.Controls.Add(this.label46);
		this.groupBox6.Controls.Add(this.label55);
		this.groupBox6.Controls.Add(this.label25);
		this.groupBox6.Controls.Add(this.label45);
		this.groupBox6.Controls.Add(this.label28);
		this.groupBox6.Controls.Add(this.label56);
		this.groupBox6.Controls.Add(this.label38);
		this.groupBox6.Controls.Add(this.RadItemCodePackListDetail);
		this.groupBox6.Controls.Add(this.RadItemNamePackListDetail);
		this.groupBox6.Controls.Add(this.txtRemarksDetail);
		this.groupBox6.Controls.Add(this.label51);
		this.groupBox6.Controls.Add(this.label16);
		this.groupBox6.Controls.Add(this.label48);
		this.groupBox6.Controls.Add(this.label21);
		this.groupBox6.Controls.Add(this.label19);
		this.groupBox6.Controls.Add(this.BtnAdd);
		this.groupBox6.Controls.Add(this.BtnCancelDetail);
		this.groupBox6.Controls.Add(this.BtnUpdateDetail);
		this.groupBox6.Controls.Add(this.label14);
		this.groupBox6.Controls.Add(this.label15);
		this.groupBox6.Controls.Add(this.label20);
		this.groupBox6.Controls.Add(this.txtGrossWeight);
		this.groupBox6.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold);
		this.groupBox6.Location = new System.Drawing.Point(2, 3);
		this.groupBox6.Name = "groupBox6";
		this.groupBox6.Size = new System.Drawing.Size(1116, 154);
		this.groupBox6.TabIndex = 50;
		this.groupBox6.TabStop = false;
		this.groupBox6.Text = "Detail";
		this.txtWarningRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWarningRemarks.Enabled = false;
		this.txtWarningRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWarningRemarks.Location = new System.Drawing.Point(820, 126);
		this.txtWarningRemarks.Name = "txtWarningRemarks";
		this.txtWarningRemarks.Size = new System.Drawing.Size(162, 23);
		this.txtWarningRemarks.TabIndex = 21;
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(820, 108);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(101, 15);
		this.label53.TabIndex = 689;
		this.label53.Text = "Warning Remarks";
		this.CmbDeliveryToParty.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryToParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryToParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryToParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance119;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance21).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance100).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryToParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty).Location = new System.Drawing.Point(821, 82);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty).Name = "CmbDeliveryToParty";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty).Size = new System.Drawing.Size(293, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty).TabIndex = 686;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty).Leave += new System.EventHandler(CmbDeliveryToParty_Leave);
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(821, 66);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(125, 15);
		this.label52.TabIndex = 687;
		this.label52.Text = "Deliver / Ship To Party";
		this.txtDeliveryToAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDeliveryToAddress.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDeliveryToAddress.Location = new System.Drawing.Point(284, 126);
		this.txtDeliveryToAddress.Name = "txtDeliveryToAddress";
		this.txtDeliveryToAddress.Size = new System.Drawing.Size(250, 23);
		this.txtDeliveryToAddress.TabIndex = 19;
		this.txtDeliveryToAddress.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDeliveryToAddress_KeyPress);
		this.CmbShipToAddress.AllowNull = (DefaultableBoolean)1;
		this.CmbShipToAddress.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbShipToAddress.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbShipToAddress.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbShipToAddress).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.CmbShipToAddress).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance14).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbShipToAddress).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbShipToAddress.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbShipToAddress).Location = new System.Drawing.Point(4, 124);
		((System.Windows.Forms.Control)(object)this.CmbShipToAddress).Name = "CmbShipToAddress";
		((System.Windows.Forms.Control)(object)this.CmbShipToAddress).Size = new System.Drawing.Size(251, 26);
		((System.Windows.Forms.Control)(object)this.CmbShipToAddress).TabIndex = 18;
		((System.Windows.Forms.Control)(object)this.CmbShipToAddress).Leave += new System.EventHandler(CmbShipToAddress_Leave);
		this.txtSaleOrderNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSaleOrderNo.Enabled = false;
		this.txtSaleOrderNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSaleOrderNo.Location = new System.Drawing.Point(536, 84);
		this.txtSaleOrderNo.Name = "txtSaleOrderNo";
		this.txtSaleOrderNo.Size = new System.Drawing.Size(80, 23);
		this.txtSaleOrderNo.TabIndex = 685;
		this.BtnLoadOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnLoadOrder.Image = Architecture.WinApp.Properties.Resources.download;
		this.BtnLoadOrder.Location = new System.Drawing.Point(82, 12);
		this.BtnLoadOrder.Name = "BtnLoadOrder";
		this.BtnLoadOrder.Size = new System.Drawing.Size(20, 23);
		this.BtnLoadOrder.TabIndex = 684;
		this.BtnLoadOrder.TabStop = false;
		this.BtnLoadOrder.UseVisualStyleBackColor = true;
		this.BtnLoadOrder.Click += new System.EventHandler(BtnLoadOrder_Click);
		this.btnDefineShipToAddress.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnDefineShipToAddress.Location = new System.Drawing.Point(256, 124);
		this.btnDefineShipToAddress.Name = "btnDefineShipToAddress";
		this.btnDefineShipToAddress.Size = new System.Drawing.Size(26, 26);
		this.btnDefineShipToAddress.TabIndex = 19;
		this.btnDefineShipToAddress.TabStop = false;
		this.btnDefineShipToAddress.UseVisualStyleBackColor = true;
		this.btnDefineShipToAddress.Click += new System.EventHandler(btnDefineShipToAddress_Click);
		this.CmbPoNoDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbPoNoDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPoNoDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPoNoDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPoNoDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbPoNoDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPoNoDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPoNoDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance185).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPoNoDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance185;
		((SpecialBoxBase)((UltraGridBase)this.CmbPoNoDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPoNoDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.CmbPoNoDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPoNoDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPoNoDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance200;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPoNoDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.CmbPoNoDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPoNoDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPoNoDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance223;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance227).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPoNoDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.CmbPoNoDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPoNoDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance228).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance228).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance228).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance228).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPoNoDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance228;
		((AppearanceBase)appearance229).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPoNoDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.CmbPoNoDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPoNoDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance230).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPoNoDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance230;
		((UltraGridBase)this.CmbPoNoDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPoNoDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.CmbPoNoDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPoNoDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPoNoDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPoNoDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPoNoDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPoNoDetail).Location = new System.Drawing.Point(4, 37);
		((System.Windows.Forms.Control)(object)this.CmbPoNoDetail).Name = "CmbPoNoDetail";
		((System.Windows.Forms.Control)(object)this.CmbPoNoDetail).Size = new System.Drawing.Size(98, 26);
		((System.Windows.Forms.Control)(object)this.CmbPoNoDetail).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbPoNoDetail).Leave += new System.EventHandler(CmbPoNoDetail_Leave);
		this.txtPoTotalWeightDetail.BackColor = System.Drawing.SystemColors.Menu;
		this.txtPoTotalWeightDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPoTotalWeightDetail.Enabled = false;
		this.txtPoTotalWeightDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPoTotalWeightDetail.Location = new System.Drawing.Point(104, 39);
		this.txtPoTotalWeightDetail.Name = "txtPoTotalWeightDetail";
		this.txtPoTotalWeightDetail.ReadOnly = true;
		this.txtPoTotalWeightDetail.Size = new System.Drawing.Size(90, 23);
		this.txtPoTotalWeightDetail.TabIndex = 1;
		this.txtPoTotalWeightDetail.TabStop = false;
		this.txtPoTotalWeightDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPoAlreadyReceivedWeightDetail.BackColor = System.Drawing.SystemColors.Menu;
		this.txtPoAlreadyReceivedWeightDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPoAlreadyReceivedWeightDetail.Enabled = false;
		this.txtPoAlreadyReceivedWeightDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPoAlreadyReceivedWeightDetail.Location = new System.Drawing.Point(195, 39);
		this.txtPoAlreadyReceivedWeightDetail.Name = "txtPoAlreadyReceivedWeightDetail";
		this.txtPoAlreadyReceivedWeightDetail.ReadOnly = true;
		this.txtPoAlreadyReceivedWeightDetail.Size = new System.Drawing.Size(90, 23);
		this.txtPoAlreadyReceivedWeightDetail.TabIndex = 2;
		this.txtPoAlreadyReceivedWeightDetail.TabStop = false;
		this.txtPoAlreadyReceivedWeightDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPoBalWeightDetail.BackColor = System.Drawing.SystemColors.Menu;
		this.txtPoBalWeightDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPoBalWeightDetail.Enabled = false;
		this.txtPoBalWeightDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPoBalWeightDetail.Location = new System.Drawing.Point(286, 39);
		this.txtPoBalWeightDetail.Name = "txtPoBalWeightDetail";
		this.txtPoBalWeightDetail.ReadOnly = true;
		this.txtPoBalWeightDetail.Size = new System.Drawing.Size(91, 23);
		this.txtPoBalWeightDetail.TabIndex = 3;
		this.txtPoBalWeightDetail.TabStop = false;
		this.txtPoBalWeightDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbEmptyBagTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbEmptyBagTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbEmptyBagTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbEmptyBagTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance76;
		((SpecialBoxBase)((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance158).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance159;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance160).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance161).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance161).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance161).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbEmptyBagTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbEmptyBagTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbEmptyBagTerm).Location = new System.Drawing.Point(379, 82);
		((System.Windows.Forms.Control)(object)this.CmbEmptyBagTerm).Name = "CmbEmptyBagTerm";
		((System.Windows.Forms.Control)(object)this.CmbEmptyBagTerm).Size = new System.Drawing.Size(155, 26);
		((System.Windows.Forms.Control)(object)this.CmbEmptyBagTerm).TabIndex = 15;
		this.CmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance278).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance278).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance278;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance280).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance280).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance280).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance280).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance280;
		((AppearanceBase)appearance281).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance281;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance282).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance282).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance282).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance282).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance282;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance283).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance283).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance283;
		((AppearanceBase)appearance284).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance284).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance284;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance285).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance285;
		((AppearanceBase)appearance286).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance286).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance286;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance287).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance287).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance287).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance287).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance287).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance287;
		((AppearanceBase)appearance288).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance288;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance289).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance289).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance289;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance291).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance291;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Location = new System.Drawing.Point(840, 37);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Name = "CmbPackingType";
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Size = new System.Drawing.Size(115, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Leave += new System.EventHandler(CmbPackingType_Leave);
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(957, 37);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(85, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Leave += new System.EventHandler(CmbPackUom_Leave);
		this.txtLoadingQty.BackColor = System.Drawing.Color.White;
		this.txtLoadingQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLoadingQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLoadingQty.Location = new System.Drawing.Point(1044, 39);
		this.txtLoadingQty.Name = "txtLoadingQty";
		this.txtLoadingQty.Size = new System.Drawing.Size(70, 23);
		this.txtLoadingQty.TabIndex = 9;
		this.txtLoadingQty.Tag = "double";
		this.txtLoadingQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtLoadingQty.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.CmbParentItem.AllowNull = (DefaultableBoolean)1;
		this.CmbParentItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance201).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance201;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance202;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance203).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance203).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance204;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance205).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance206;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance207).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance208).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance208).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance208).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance208).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance208;
		((AppearanceBase)appearance209).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance210).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance213;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbParentItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParentItem).Location = new System.Drawing.Point(379, 37);
		((System.Windows.Forms.Control)(object)this.CmbParentItem).Name = "CmbParentItem";
		((System.Windows.Forms.Control)(object)this.CmbParentItem).Size = new System.Drawing.Size(155, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentItem).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbParentItem).Leave += new System.EventHandler(cmbParentItem_Leave);
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(536, 37);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Leave += new System.EventHandler(CmbItemName_Leave);
		this.CmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance144;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance145).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance145).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance150).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance151).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance152).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Location = new System.Drawing.Point(738, 37);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Name = "CmbCropYear";
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabIndex = 6;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(284, 111);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(111, 15);
		this.label47.TabIndex = 683;
		this.label47.Text = "Delivery To Address";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.Location = new System.Drawing.Point(4, 111);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(111, 15);
		this.label60.TabIndex = 683;
		this.label60.Text = "Delivery To Address";
		this.txtEbUnit.AccessibleName = "EbUnit";
		this.txtEbUnit.BackColor = System.Drawing.Color.White;
		this.txtEbUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbUnit.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEbUnit.Location = new System.Drawing.Point(95, 84);
		this.txtEbUnit.Name = "txtEbUnit";
		this.txtEbUnit.Size = new System.Drawing.Size(55, 23);
		this.txtEbUnit.TabIndex = 11;
		this.txtEbUnit.Tag = "double";
		this.txtEbUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEbUnit.TextChanged += new System.EventHandler(txtebu_TextChanged);
		this.CmbBuyerNameDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbBuyerNameDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBuyerNameDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBuyerNameDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBuyerNameDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.CmbBuyerNameDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBuyerNameDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance187).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance187).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance187).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBuyerNameDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance187;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBuyerNameDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance188;
		((SpecialBoxBase)((UltraGridBase)this.CmbBuyerNameDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance190).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance190).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBuyerNameDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.CmbBuyerNameDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBuyerNameDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBuyerNameDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance191;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBuyerNameDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.CmbBuyerNameDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBuyerNameDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBuyerNameDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance193;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance194).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBuyerNameDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.CmbBuyerNameDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBuyerNameDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance195).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance195).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance195).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance195).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBuyerNameDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance195;
		((AppearanceBase)appearance196).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBuyerNameDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbBuyerNameDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBuyerNameDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBuyerNameDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.CmbBuyerNameDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBuyerNameDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.CmbBuyerNameDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBuyerNameDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBuyerNameDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBuyerNameDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameDetail).Location = new System.Drawing.Point(617, 82);
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameDetail).Name = "CmbBuyerNameDetail";
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameDetail).Size = new System.Drawing.Size(202, 26);
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameDetail).TabIndex = 17;
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameDetail).Leave += new System.EventHandler(CmbBuyerNameDetail_Leave);
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(617, 66);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(72, 15);
		this.label35.TabIndex = 663;
		this.label35.Text = "Buyer Name";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(536, 66);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(77, 15);
		this.label24.TabIndex = 661;
		this.label24.Text = "SO / Deal No";
		this.txtAddLessWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAddLessWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAddLessWeight.Location = new System.Drawing.Point(222, 84);
		this.txtAddLessWeight.Name = "txtAddLessWeight";
		this.txtAddLessWeight.Size = new System.Drawing.Size(60, 23);
		this.txtAddLessWeight.TabIndex = 13;
		this.txtAddLessWeight.Tag = "double";
		this.txtAddLessWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAddLessWeight.TextChanged += new System.EventHandler(txtadlswt_TextChanged);
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(4, 16);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(77, 15);
		this.label34.TabIndex = 661;
		this.label34.Text = "PO / Deal No";
		this.txtEbTotal.AccessibleName = "EbTotal";
		this.txtEbTotal.BackColor = System.Drawing.Color.White;
		this.txtEbTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbTotal.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEbTotal.Location = new System.Drawing.Point(151, 84);
		this.txtEbTotal.Name = "txtEbTotal";
		this.txtEbTotal.Size = new System.Drawing.Size(70, 23);
		this.txtEbTotal.TabIndex = 12;
		this.txtEbTotal.Tag = "double";
		this.txtEbTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEbTotal.TextChanged += new System.EventHandler(txtEmptyBagsTotal_TextChanged);
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(104, 16);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(75, 15);
		this.label39.TabIndex = 665;
		this.label39.Text = "Total Weight";
		this.txtNetWeight.BackColor = System.Drawing.Color.White;
		this.txtNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNetWeight.Location = new System.Drawing.Point(283, 84);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.ReadOnly = true;
		this.txtNetWeight.Size = new System.Drawing.Size(94, 23);
		this.txtNetWeight.TabIndex = 14;
		this.txtNetWeight.TabStop = false;
		this.txtNetWeight.Tag = "double";
		this.txtNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(284, 66);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(65, 15);
		this.label46.TabIndex = 680;
		this.label46.Text = "Bill Weight";
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(188, 8);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(100, 30);
		this.label55.TabIndex = 667;
		this.label55.Text = "Already Received \r\nWeight";
		this.label55.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(219, 66);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(62, 15);
		this.label25.TabIndex = 681;
		this.label25.Text = "Add / Less";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(155, 66);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(52, 15);
		this.label45.TabIndex = 677;
		this.label45.Text = "E.b Total";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(4, 66);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(78, 15);
		this.label28.TabIndex = 678;
		this.label28.Text = "Gross Weight";
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(288, 8);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(99, 30);
		this.label56.TabIndex = 669;
		this.label56.Text = "Available Balance\r\nWeight";
		this.label56.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(95, 66);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(56, 15);
		this.label38.TabIndex = 679;
		this.label38.Text = "E.b / Unit";
		this.RadItemCodePackListDetail.AutoSize = true;
		this.RadItemCodePackListDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadItemCodePackListDetail.Location = new System.Drawing.Point(619, 14);
		this.RadItemCodePackListDetail.Name = "RadItemCodePackListDetail";
		this.RadItemCodePackListDetail.Size = new System.Drawing.Size(52, 19);
		this.RadItemCodePackListDetail.TabIndex = 671;
		this.RadItemCodePackListDetail.Text = "Code";
		this.RadItemCodePackListDetail.UseVisualStyleBackColor = true;
		this.RadItemNamePackListDetail.AutoSize = true;
		this.RadItemNamePackListDetail.Checked = true;
		this.RadItemNamePackListDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadItemNamePackListDetail.Location = new System.Drawing.Point(566, 14);
		this.RadItemNamePackListDetail.Name = "RadItemNamePackListDetail";
		this.RadItemNamePackListDetail.Size = new System.Drawing.Size(57, 19);
		this.RadItemNamePackListDetail.TabIndex = 670;
		this.RadItemNamePackListDetail.TabStop = true;
		this.RadItemNamePackListDetail.Text = "Name";
		this.RadItemNamePackListDetail.UseVisualStyleBackColor = true;
		this.RadItemNamePackListDetail.CheckedChanged += new System.EventHandler(RadItemNamePackListDetail_CheckedChanged);
		this.txtRemarksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(536, 126);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(283, 23);
		this.txtRemarksDetail.TabIndex = 20;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(536, 111);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(52, 15);
		this.label51.TabIndex = 652;
		this.label51.Text = "Remarks";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(1044, 16);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(72, 15);
		this.label16.TabIndex = 31;
		this.label16.Text = "Loading Qty";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(379, 66);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(94, 15);
		this.label48.TabIndex = 36;
		this.label48.Text = "Empty Bag Term";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(957, 16);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(61, 15);
		this.label21.TabIndex = 40;
		this.label21.Text = "Pack Uom";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(840, 16);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(77, 15);
		this.label19.TabIndex = 36;
		this.label19.Text = "Packing Type";
		this.BtnAdd.BackColor = System.Drawing.Color.Teal;
		this.BtnAdd.FlatAppearance.BorderSize = 0;
		this.BtnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAdd.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAdd.ForeColor = System.Drawing.Color.White;
		this.BtnAdd.Location = new System.Drawing.Point(985, 126);
		this.BtnAdd.Name = "BtnAdd";
		this.BtnAdd.Size = new System.Drawing.Size(36, 23);
		this.BtnAdd.TabIndex = 22;
		this.BtnAdd.Text = "+";
		this.BtnAdd.UseVisualStyleBackColor = false;
		this.BtnAdd.Click += new System.EventHandler(BtnAdd_Click);
		this.BtnCancelDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnCancelDetail.FlatAppearance.BorderSize = 0;
		this.BtnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.BtnCancelDetail.Location = new System.Drawing.Point(1051, 126);
		this.BtnCancelDetail.Name = "BtnCancelDetail";
		this.BtnCancelDetail.Size = new System.Drawing.Size(62, 23);
		this.BtnCancelDetail.TabIndex = 24;
		this.BtnCancelDetail.Text = "Clear";
		this.BtnCancelDetail.UseVisualStyleBackColor = false;
		this.BtnCancelDetail.Visible = false;
		this.BtnCancelDetail.Click += new System.EventHandler(BtnCancelDetail_Click);
		this.BtnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.BtnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateDetail.Location = new System.Drawing.Point(985, 126);
		this.BtnUpdateDetail.Name = "BtnUpdateDetail";
		this.BtnUpdateDetail.Size = new System.Drawing.Size(65, 23);
		this.BtnUpdateDetail.TabIndex = 23;
		this.BtnUpdateDetail.Text = "Update";
		this.BtnUpdateDetail.UseVisualStyleBackColor = false;
		this.BtnUpdateDetail.Visible = false;
		this.BtnUpdateDetail.Click += new System.EventHandler(BtnUpdateDetail_Click);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(386, 16);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(69, 15);
		this.label14.TabIndex = 30;
		this.label14.Text = "Parent Item";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(536, 16);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(32, 15);
		this.label15.TabIndex = 32;
		this.label15.Text = "Item";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(738, 16);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(57, 15);
		this.label20.TabIndex = 38;
		this.label20.Text = "Crop Year";
		this.txtGrossWeight.BackColor = System.Drawing.Color.White;
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrossWeight.Location = new System.Drawing.Point(4, 84);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(90, 23);
		this.txtGrossWeight.TabIndex = 10;
		this.txtGrossWeight.Tag = "double";
		this.txtGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGrossWeight.TextChanged += new System.EventHandler(txtWeight_TextChanged);
		this.panel1.Controls.Add(this.groupBox2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 61);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1120, 155);
		this.panel1.TabIndex = 3;
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbVehicleType);
		this.groupBox2.Controls.Add(this.label49);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm);
		this.groupBox2.Controls.Add(this.label18);
		this.groupBox2.Controls.Add(this.txtVehicleNo);
		this.groupBox2.Controls.Add(this.txtTotalFreight);
		this.groupBox2.Controls.Add(this.label62);
		this.groupBox2.Controls.Add(this.label44);
		this.groupBox2.Controls.Add(this.label42);
		this.groupBox2.Controls.Add(this.label41);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.txtAvgFillingPerBag);
		this.groupBox2.Controls.Add(this.txtWbNetWeight);
		this.groupBox2.Controls.Add(this.txtTareWeight);
		this.groupBox2.Controls.Add(this.txtLoadWeight);
		this.groupBox2.Controls.Add(this.txtVehicleQty);
		this.groupBox2.Controls.Add(this.txtBiltyNo);
		this.groupBox2.Controls.Add(this.txtOtherCharges);
		this.groupBox2.Controls.Add(this.txtBiltyFreight);
		this.groupBox2.Controls.Add(this.txtTransporterName);
		this.groupBox2.Controls.Add(this.label40);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add(this.txtBiltyDate);
		this.groupBox2.Controls.Add(this.datDocDate);
		this.groupBox2.Controls.Add(this.label17);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransporter);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbUnloadingCity);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLoadingCity);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.txtRemarks);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommissionAgent);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplierName);
		this.groupBox2.Controls.Add(this.txtDocNo);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(3, 1);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1114, 149);
		this.groupBox2.TabIndex = 50;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main Information";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(844, 9);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(67, 30);
		this.label11.TabIndex = 676;
		this.label11.Text = "Vehicle\r\nNo Of Bags";
		this.cmbVehicleType.AllowNull = (DefaultableBoolean)1;
		this.cmbVehicleType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbVehicleType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbVehicleType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbVehicleType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance97;
		((SpecialBoxBase)((UltraGridBase)this.cmbVehicleType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance211).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance111).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance99).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance99).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance99).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance114).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbVehicleType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).Location = new System.Drawing.Point(544, 91);
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).Name = "cmbVehicleType";
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).Size = new System.Drawing.Size(91, 26);
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).TabIndex = 11;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(496, 89);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(48, 30);
		this.label49.TabIndex = 675;
		this.label49.Text = "Vehicle \r\nType";
		this.CmbDeliveryTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance292).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance292).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance292;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance293).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance293).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance293).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance293).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance293;
		((AppearanceBase)appearance294).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance294;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance295).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance295).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance295).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance295).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance295;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance296).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance296).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance296;
		((AppearanceBase)appearance297).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance297).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance297;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance298).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance298;
		((AppearanceBase)appearance299).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance299).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance299;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance300).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance300).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance300).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance300).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance300).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance300;
		((AppearanceBase)appearance3).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Location = new System.Drawing.Point(720, 91);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Name = "CmbDeliveryTerm";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).TabIndex = 16;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(639, 97);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(80, 15);
		this.label18.TabIndex = 673;
		this.label18.Text = "Delivery Term";
		this.txtVehicleNo.BackColor = System.Drawing.Color.White;
		this.txtVehicleNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleNo.Location = new System.Drawing.Point(720, 13);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.Size = new System.Drawing.Size(120, 23);
		this.txtVehicleNo.TabIndex = 13;
		this.txtVehicleNo.Tag = "";
		this.txtVehicleNo.TextChanged += new System.EventHandler(txtVehicleNo_TextChanged);
		this.txtVehicleNo.KeyDown += new System.Windows.Forms.KeyEventHandler(txtvehicleno_KeyDown);
		this.txtVehicleNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtvehicleno_KeyPress);
		this.txtVehicleNo.KeyUp += new System.Windows.Forms.KeyEventHandler(txtvehicleno_KeyUp);
		this.txtVehicleNo.Leave += new System.EventHandler(txtvehicleno_Leave);
		this.txtTotalFreight.BackColor = System.Drawing.Color.White;
		this.txtTotalFreight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalFreight.Enabled = false;
		this.txtTotalFreight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalFreight.Location = new System.Drawing.Point(404, 93);
		this.txtTotalFreight.Name = "txtTotalFreight";
		this.txtTotalFreight.Size = new System.Drawing.Size(90, 23);
		this.txtTotalFreight.TabIndex = 10;
		this.txtTotalFreight.Tag = "double";
		this.txtTotalFreight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(844, 97);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(68, 15);
		this.label44.TabIndex = 671;
		this.label44.Text = "Net Weight";
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(844, 43);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(75, 15);
		this.label42.TabIndex = 671;
		this.label42.Text = "Load Weight";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(844, 70);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(71, 15);
		this.label41.TabIndex = 671;
		this.label41.Text = "Tare Weight";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(639, 70);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(49, 15);
		this.label10.TabIndex = 671;
		this.label10.Text = "Bilty No";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(495, 64);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(49, 26);
		this.label2.TabIndex = 671;
		this.label2.Text = "Other \r\nCharges";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(319, 97);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(73, 15);
		this.label4.TabIndex = 671;
		this.label4.Text = "Total Freight";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(639, 17);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(64, 15);
		this.label5.TabIndex = 671;
		this.label5.Text = "Vehicle No";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(319, 70);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(70, 15);
		this.label1.TabIndex = 671;
		this.label1.Text = "Bilty Freight";
		this.txtWbNetWeight.BackColor = System.Drawing.Color.White;
		this.txtWbNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWbNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWbNetWeight.Location = new System.Drawing.Point(921, 93);
		this.txtWbNetWeight.Name = "txtWbNetWeight";
		this.txtWbNetWeight.Size = new System.Drawing.Size(93, 23);
		this.txtWbNetWeight.TabIndex = 20;
		this.txtWbNetWeight.Tag = "double";
		this.txtWbNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtWbNetWeight.Leave += new System.EventHandler(txtWbNetWeight_TextChanged);
		this.txtTareWeight.BackColor = System.Drawing.Color.White;
		this.txtTareWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTareWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTareWeight.Location = new System.Drawing.Point(921, 66);
		this.txtTareWeight.Name = "txtTareWeight";
		this.txtTareWeight.Size = new System.Drawing.Size(93, 23);
		this.txtTareWeight.TabIndex = 19;
		this.txtTareWeight.Tag = "double";
		this.txtTareWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTareWeight.TextChanged += new System.EventHandler(txtTareWeight_TextChanged);
		this.txtTareWeight.Leave += new System.EventHandler(txtTareWeight_Leave);
		this.txtLoadWeight.BackColor = System.Drawing.Color.White;
		this.txtLoadWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLoadWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLoadWeight.Location = new System.Drawing.Point(921, 39);
		this.txtLoadWeight.Name = "txtLoadWeight";
		this.txtLoadWeight.Size = new System.Drawing.Size(93, 23);
		this.txtLoadWeight.TabIndex = 18;
		this.txtLoadWeight.Tag = "double";
		this.txtLoadWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtLoadWeight.TextChanged += new System.EventHandler(txtLoadWeight_TextChanged);
		this.txtLoadWeight.Leave += new System.EventHandler(txtLoadWeight_Leave);
		this.txtVehicleQty.BackColor = System.Drawing.Color.White;
		this.txtVehicleQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleQty.Location = new System.Drawing.Point(921, 13);
		this.txtVehicleQty.Name = "txtVehicleQty";
		this.txtVehicleQty.Size = new System.Drawing.Size(93, 23);
		this.txtVehicleQty.TabIndex = 17;
		this.txtVehicleQty.Tag = "double";
		this.txtVehicleQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtVehicleQty.TextChanged += new System.EventHandler(txtVehicleQty_TextChanged);
		this.txtBiltyNo.BackColor = System.Drawing.Color.White;
		this.txtBiltyNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBiltyNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBiltyNo.Location = new System.Drawing.Point(720, 66);
		this.txtBiltyNo.Name = "txtBiltyNo";
		this.txtBiltyNo.Size = new System.Drawing.Size(120, 23);
		this.txtBiltyNo.TabIndex = 15;
		this.txtBiltyNo.Tag = "";
		this.txtOtherCharges.BackColor = System.Drawing.Color.White;
		this.txtOtherCharges.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherCharges.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOtherCharges.Location = new System.Drawing.Point(544, 66);
		this.txtOtherCharges.Name = "txtOtherCharges";
		this.txtOtherCharges.Size = new System.Drawing.Size(90, 23);
		this.txtOtherCharges.TabIndex = 9;
		this.txtOtherCharges.Tag = "double";
		this.txtOtherCharges.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOtherCharges.TextChanged += new System.EventHandler(txtOtherCharges_TextChanged);
		this.txtOtherCharges.Leave += new System.EventHandler(txtOtherCharges_Leave);
		this.txtBiltyFreight.BackColor = System.Drawing.Color.White;
		this.txtBiltyFreight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBiltyFreight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBiltyFreight.Location = new System.Drawing.Point(404, 66);
		this.txtBiltyFreight.Name = "txtBiltyFreight";
		this.txtBiltyFreight.Size = new System.Drawing.Size(90, 23);
		this.txtBiltyFreight.TabIndex = 8;
		this.txtBiltyFreight.Tag = "double";
		this.txtBiltyFreight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBiltyFreight.TextChanged += new System.EventHandler(txtBiltyFreight_TextChanged);
		this.txtBiltyFreight.Leave += new System.EventHandler(txtBiltyFreight_Leave);
		this.txtTransporterName.BackColor = System.Drawing.Color.White;
		this.txtTransporterName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTransporterName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTransporterName.Location = new System.Drawing.Point(404, 39);
		this.txtTransporterName.Name = "txtTransporterName";
		this.txtTransporterName.Size = new System.Drawing.Size(230, 23);
		this.txtTransporterName.TabIndex = 7;
		this.txtTransporterName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtTransporterName_KeyPress);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(319, 35);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(67, 30);
		this.label40.TabIndex = 33;
		this.label40.Text = "Transporter\r\nName";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(319, 17);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(67, 15);
		this.label9.TabIndex = 33;
		this.label9.Text = "Transporter";
		this.txtBiltyDate.CustomFormat = "dd-MMM-yy";
		this.txtBiltyDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtBiltyDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtBiltyDate.Location = new System.Drawing.Point(720, 39);
		this.txtBiltyDate.Name = "txtBiltyDate";
		this.txtBiltyDate.Size = new System.Drawing.Size(120, 22);
		this.txtBiltyDate.TabIndex = 14;
		this.datDocDate.CustomFormat = "dd-MMM-yy";
		this.datDocDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.datDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datDocDate.Location = new System.Drawing.Point(178, 13);
		this.datDocDate.Name = "datDocDate";
		this.datDocDate.Size = new System.Drawing.Size(130, 22);
		this.datDocDate.TabIndex = 1;
		this.datDocDate.ValueChanged += new System.EventHandler(datDocDate_ValueChanged);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(4, 17);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(76, 15);
		this.label17.TabIndex = 7;
		this.label17.Text = "Doc No,Date";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(639, 43);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(58, 15);
		this.label13.TabIndex = 7;
		this.label13.Text = "Bilty Date";
		this.CmbTransporter.AllowNull = (DefaultableBoolean)1;
		this.CmbTransporter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransporter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransporter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance249).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance249;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance250).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance250).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance250).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransporter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance250;
		((AppearanceBase)appearance251).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance251;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransporter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance252).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance252).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance252).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance253).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance253;
		((AppearanceBase)appearance254).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance254).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance254;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance256).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance256).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance256;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance258).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance258).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance258).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance258).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance258;
		((AppearanceBase)appearance272).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance272;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance276).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance276;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance277).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance277;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbTransporter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Location = new System.Drawing.Point(404, 11);
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Name = "CmbTransporter";
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Size = new System.Drawing.Size(231, 26);
		((System.Windows.Forms.Control)(object)this.CmbTransporter).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Leave += new System.EventHandler(CmbTransporter_Leave);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(4, 35);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(73, 30);
		this.label7.TabIndex = 12;
		this.label7.Text = "Commission\r\nAgent";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(4, 70);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(51, 15);
		this.label3.TabIndex = 14;
		this.label3.Text = "Supplier";
		this.CmbUnloadingCity.AllowNull = (DefaultableBoolean)1;
		this.CmbUnloadingCity.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbUnloadingCity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbUnloadingCity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbUnloadingCity).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbUnloadingCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbUnloadingCity).Location = new System.Drawing.Point(87, 118);
		((System.Windows.Forms.Control)(object)this.CmbUnloadingCity).Name = "CmbUnloadingCity";
		((System.Windows.Forms.Control)(object)this.CmbUnloadingCity).Size = new System.Drawing.Size(221, 26);
		((System.Windows.Forms.Control)(object)this.CmbUnloadingCity).TabIndex = 5;
		this.CmbLoadingCity.AllowNull = (DefaultableBoolean)1;
		this.CmbLoadingCity.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLoadingCity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLoadingCity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance214).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance215).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance215).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLoadingCity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance215;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance216;
		((SpecialBoxBase)((UltraGridBase)this.CmbLoadingCity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance217).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance217).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance217).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance218).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance219).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance220;
		((AppearanceBase)appearance221).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance221).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance222).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance222).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance222).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance222;
		((AppearanceBase)appearance224).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance225).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLoadingCity).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLoadingCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLoadingCity).Location = new System.Drawing.Point(87, 91);
		((System.Windows.Forms.Control)(object)this.CmbLoadingCity).Name = "CmbLoadingCity";
		((System.Windows.Forms.Control)(object)this.CmbLoadingCity).Size = new System.Drawing.Size(221, 26);
		((System.Windows.Forms.Control)(object)this.CmbLoadingCity).TabIndex = 4;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(319, 124);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(52, 15);
		this.label12.TabIndex = 29;
		this.label12.Text = "Remarks";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(4, 124);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(85, 15);
		this.label6.TabIndex = 20;
		this.label6.Text = "Unloading City";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(4, 97);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(73, 15);
		this.label8.TabIndex = 20;
		this.label8.Text = "Loading City";
		this.txtRemarks.BackColor = System.Drawing.Color.White;
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(404, 120);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(436, 23);
		this.txtRemarks.TabIndex = 12;
		this.CmbCommissionAgent.AllowNull = (DefaultableBoolean)1;
		this.CmbCommissionAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommissionAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommissionAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance31;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance136).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance137).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance137).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance137).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommissionAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Location = new System.Drawing.Point(87, 37);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Name = "CmbCommissionAgent";
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Size = new System.Drawing.Size(221, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).TabIndex = 2;
		this.CmbSupplierName.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplierName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplierName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplierName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance232).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance232;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance38;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance166).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance245).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance245;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance247).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance248;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplierName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Location = new System.Drawing.Point(87, 64);
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Name = "CmbSupplierName";
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Size = new System.Drawing.Size(221, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Leave += new System.EventHandler(CmbSupplierName_Leave);
		this.txtDocNo.BackColor = System.Drawing.Color.White;
		this.txtDocNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNo.Location = new System.Drawing.Point(87, 13);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.ReadOnly = true;
		this.txtDocNo.Size = new System.Drawing.Size(90, 23);
		this.txtDocNo.TabIndex = 0;
		this.txtDocNo.TabStop = false;
		this.txtDocNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label54);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranch);
		this.panel3.Controls.Add(this.label26);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.chkPrint);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCompanyName);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 30);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1120, 31);
		this.panel3.TabIndex = 2;
		this.label54.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.ForeColor = System.Drawing.Color.White;
		this.label54.Location = new System.Drawing.Point(762, 8);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(46, 15);
		this.label54.TabIndex = 32;
		this.label54.Text = "Branch";
		this.CmbBranch.AllowNull = (DefaultableBoolean)1;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.CmbBranch.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbBranch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbBranch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBranch.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Location = new System.Drawing.Point(815, 2);
		((System.Windows.Forms.Control)(object)this.CmbBranch).Name = "CmbBranch";
		((System.Windows.Forms.Control)(object)this.CmbBranch).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranch).TabIndex = 33;
		this.label26.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.White;
		this.label26.Location = new System.Drawing.Point(476, 8);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(58, 15);
		this.label26.TabIndex = 30;
		this.label26.Text = "Company";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 6);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(348, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "Goods Receiving Notes && Loading Challan";
		this.chkPrint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPrint.AutoSize = true;
		this.chkPrint.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPrint.ForeColor = System.Drawing.Color.White;
		this.chkPrint.Location = new System.Drawing.Point(1037, 6);
		this.chkPrint.Name = "chkPrint";
		this.chkPrint.Size = new System.Drawing.Size(81, 18);
		this.chkPrint.TabIndex = 2;
		this.chkPrint.Text = "Preview";
		this.chkPrint.UseVisualStyleBackColor = true;
		this.CmbCompanyName.AllowNull = (DefaultableBoolean)1;
		((System.Windows.Forms.Control)(object)this.CmbCompanyName).Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.CmbCompanyName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCompanyName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCompanyName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance234).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance234).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance234).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCompanyName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance234;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance65;
		((SpecialBoxBase)((UltraGridBase)this.CmbCompanyName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCompanyName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCompanyName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCompanyName).Location = new System.Drawing.Point(538, 2);
		((System.Windows.Forms.Control)(object)this.CmbCompanyName).Name = "CmbCompanyName";
		((System.Windows.Forms.Control)(object)this.CmbCompanyName).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbCompanyName).TabIndex = 31;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.btnnew, this.BtnRefresh, this.btnsave, this.btnUpdate, this.btnDelete, this.btnAttachments, this.btnPrint, this.BtnPrintLoadingChallan, this.BtnShortCutkeys, this.toolStripSeparator2 });
		this.toolStrip1.Location = new System.Drawing.Point(3, 3);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1120, 27);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.BtnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefresh.Image = (System.Drawing.Image)resources.GetObject("BtnRefresh.Image");
		this.BtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefresh.Name = "BtnRefresh";
		this.BtnRefresh.Size = new System.Drawing.Size(80, 24);
		this.BtnRefresh.Text = "Refresh";
		this.BtnRefresh.Click += new System.EventHandler(BtnRefresh_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = (System.Drawing.Image)resources.GetObject("btnUpdate.Image");
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = (System.Drawing.Image)resources.GetObject("btnDelete.Image");
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnAttachments.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachments.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachments.Name = "btnAttachments";
		this.btnAttachments.Size = new System.Drawing.Size(116, 24);
		this.btnAttachments.Text = "Attachments";
		this.btnAttachments.Click += new System.EventHandler(btnAttachments_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(100, 24);
		this.btnPrint.Text = "1054-&Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.BtnPrintLoadingChallan.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrintLoadingChallan.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrintLoadingChallan.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrintLoadingChallan.Name = "BtnPrintLoadingChallan";
		this.BtnPrintLoadingChallan.Size = new System.Drawing.Size(124, 24);
		this.BtnPrintLoadingChallan.Text = "1054_01-&Print";
		this.BtnPrintLoadingChallan.Click += new System.EventHandler(BtnPrintLoadingChallan_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = (System.Drawing.Image)resources.GetObject("BtnShortCutkeys.Image");
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.tabHistory.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.tabHistory.Controls.Add(this.tabControl3);
		this.tabHistory.Controls.Add(this.panel15);
		this.tabHistory.Controls.Add(this.panel13);
		this.tabHistory.Controls.Add(this.panel4);
		this.tabHistory.Controls.Add(this.toolStrip2);
		this.tabHistory.Location = new System.Drawing.Point(4, 4);
		this.tabHistory.Name = "tabHistory";
		this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
		this.tabHistory.Size = new System.Drawing.Size(1126, 668);
		this.tabHistory.TabIndex = 1;
		this.tabHistory.Text = "History";
		this.tabHistory.UseVisualStyleBackColor = true;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(3, 184);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1120, 245);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 0;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.tabControl3.Controls.Add(this.tabPage1);
		this.tabControl3.Controls.Add(this.tabPage2);
		this.tabControl3.Controls.Add(this.tabPage3);
		this.tabControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.tabControl3.Location = new System.Drawing.Point(3, 429);
		this.tabControl3.Name = "tabControl3";
		this.tabControl3.SelectedIndex = 0;
		this.tabControl3.Size = new System.Drawing.Size(1120, 236);
		this.tabControl3.TabIndex = 59;
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryDetail);
		this.tabPage1.Controls.Add(this.panel11);
		this.tabPage1.Location = new System.Drawing.Point(4, 25);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1112, 207);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Detail";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.grdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		grdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryDetail.DesignTimeLayout = grdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryDetail.DynamicFiltering = true;
		this.grdHistoryDetail.FilterMode = (FilterMode)2;
		this.grdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Location = new System.Drawing.Point(3, 27);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Name = "grdHistoryDetail";
		((ScrollableBase)this.grdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(1106, 177);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabIndex = 0;
		this.grdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label50);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 3);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1106, 24);
		this.panel11.TabIndex = 57;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.ForeColor = System.Drawing.Color.White;
		this.label50.Location = new System.Drawing.Point(4, 5);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(225, 16);
		this.label50.TabIndex = 129;
		this.label50.Text = "Detail of the Above Selected Row";
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdExpenseHistory);
		this.tabPage2.Controls.Add(this.panel12);
		this.tabPage2.Location = new System.Drawing.Point(4, 25);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1112, 207);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Other Expense";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.grdExpenseHistory.AllowEdit = (InheritableBoolean)2;
		grdExpenseHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdExpenseHistory.DesignTimeLayout = grdExpenseHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdExpenseHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdExpenseHistory.DynamicFiltering = true;
		this.grdExpenseHistory.FilterMode = (FilterMode)2;
		this.grdExpenseHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdExpenseHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdExpenseHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdExpenseHistory).Location = new System.Drawing.Point(3, 27);
		((System.Windows.Forms.Control)(object)this.grdExpenseHistory).Name = "grdExpenseHistory";
		((ScrollableBase)this.grdExpenseHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdExpenseHistory).Size = new System.Drawing.Size(1106, 177);
		((System.Windows.Forms.Control)(object)this.grdExpenseHistory).TabIndex = 59;
		this.grdExpenseHistory.TotalRow = (InheritableBoolean)1;
		this.grdExpenseHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdExpenseHistory).VisualStyle = (VisualStyle)4;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label57);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 3);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1106, 24);
		this.panel12.TabIndex = 58;
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.ForeColor = System.Drawing.Color.White;
		this.label57.Location = new System.Drawing.Point(4, 5);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(0, 16);
		this.label57.TabIndex = 129;
		this.tabPage3.Controls.Add(this.panel23);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1112, 207);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "Empty Bags Weight Deduction From Bill Weight";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel23.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmptyBagHistory);
		this.panel23.Controls.Add(this.panel16);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel23.Location = new System.Drawing.Point(0, 0);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(1112, 207);
		this.panel23.TabIndex = 62;
		this.grdEmptyBagHistory.AllowEdit = (InheritableBoolean)2;
		grdEmptyBagHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdEmptyBagHistory.DesignTimeLayout = grdEmptyBagHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdEmptyBagHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdEmptyBagHistory.DynamicFiltering = true;
		this.grdEmptyBagHistory.FilterMode = (FilterMode)2;
		this.grdEmptyBagHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdEmptyBagHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmptyBagHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdEmptyBagHistory).Location = new System.Drawing.Point(0, 24);
		((System.Windows.Forms.Control)(object)this.grdEmptyBagHistory).Name = "grdEmptyBagHistory";
		((ScrollableBase)this.grdEmptyBagHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmptyBagHistory).Size = new System.Drawing.Size(1112, 183);
		((System.Windows.Forms.Control)(object)this.grdEmptyBagHistory).TabIndex = 61;
		this.grdEmptyBagHistory.TotalRow = (InheritableBoolean)1;
		this.grdEmptyBagHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmptyBagHistory).VisualStyle = (VisualStyle)4;
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label64);
		this.panel16.Controls.Add(this.label58);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1112, 24);
		this.panel16.TabIndex = 60;
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.ForeColor = System.Drawing.Color.Yellow;
		this.label64.Location = new System.Drawing.Point(5, 4);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(272, 15);
		this.label64.TabIndex = 239;
		this.label64.Text = "Empty Bags Weight Deduction From Bill Weight";
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.ForeColor = System.Drawing.Color.White;
		this.label58.Location = new System.Drawing.Point(4, 5);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(0, 16);
		this.label58.TabIndex = 129;
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.ctrlGrdBar1);
		this.panel15.Controls.Add(this.label43);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(3, 161);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1120, 23);
		this.panel15.TabIndex = 54;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1078, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(9, 6, 9, 6);
		this.ctrlGrdBar1.MyGrid = this.grdHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(42, 22);
		this.ctrlGrdBar1.TabIndex = 128;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.Color.White;
		this.label43.Location = new System.Drawing.Point(7, 3);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(112, 16);
		this.label43.TabIndex = 127;
		this.label43.Text = "Filtered Records";
		this.panel13.BackColor = System.Drawing.Color.White;
		this.panel13.Controls.Add(this.groupBox1);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(3, 59);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1120, 102);
		this.panel13.TabIndex = 6;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliverToAddressHistory);
		this.groupBox1.Controls.Add(this.datValidityToHistory);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryToPartyHistory);
		this.groupBox1.Controls.Add(this.label59);
		this.groupBox1.Controls.Add(this.datValidityFromHistory);
		this.groupBox1.Controls.Add(this.label61);
		this.groupBox1.Controls.Add(this.ToDateHistory);
		this.groupBox1.Controls.Add(this.FromDateHistory);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemNameHistory);
		this.groupBox1.Controls.Add(this.label37);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentItemHistory);
		this.groupBox1.Controls.Add(this.rdmodifydate);
		this.groupBox1.Controls.Add(this.rdentrydate);
		this.groupBox1.Controls.Add(this.label36);
		this.groupBox1.Controls.Add(this.drdocdate);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBuyerNameHistory);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommissionAgentHistory);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1116, 98);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.CmbDeliverToAddressHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliverToAddressHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliverToAddressHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliverToAddressHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance123).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance123).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance125;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance126).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance126).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance130).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance131).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliverToAddressHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliverToAddressHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliverToAddressHistory).Location = new System.Drawing.Point(774, 39);
		((System.Windows.Forms.Control)(object)this.CmbDeliverToAddressHistory).Name = "CmbDeliverToAddressHistory";
		((System.Windows.Forms.Control)(object)this.CmbDeliverToAddressHistory).Size = new System.Drawing.Size(240, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliverToAddressHistory).TabIndex = 9;
		this.datValidityToHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datValidityToHistory.Checked = false;
		this.datValidityToHistory.CustomFormat = "dd-MMM-yy";
		this.datValidityToHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.datValidityToHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datValidityToHistory.Location = new System.Drawing.Point(228, 41);
		this.datValidityToHistory.Name = "datValidityToHistory";
		this.datValidityToHistory.ShowCheckBox = true;
		this.datValidityToHistory.Size = new System.Drawing.Size(130, 22);
		this.datValidityToHistory.TabIndex = 3;
		this.CmbDeliveryToPartyHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryToPartyHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryToPartyHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryToPartyHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance235).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance237).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance237).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance237).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance238).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance238;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance239).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance240;
		((AppearanceBase)appearance241).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance241).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance241;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance242).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance242).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance242).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance242).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance242;
		((AppearanceBase)appearance243).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance243;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance244).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance255).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToPartyHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryToPartyHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToPartyHistory).Location = new System.Drawing.Point(774, 11);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToPartyHistory).Name = "CmbDeliveryToPartyHistory";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToPartyHistory).Size = new System.Drawing.Size(240, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToPartyHistory).TabIndex = 8;
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label59.Location = new System.Drawing.Point(671, 45);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(105, 15);
		this.label59.TabIndex = 527;
		this.label59.Text = "Deliver To Address\r\n";
		this.datValidityFromHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datValidityFromHistory.Checked = false;
		this.datValidityFromHistory.CustomFormat = "dd-MMM-yy";
		this.datValidityFromHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.datValidityFromHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datValidityFromHistory.Location = new System.Drawing.Point(97, 41);
		this.datValidityFromHistory.Name = "datValidityFromHistory";
		this.datValidityFromHistory.ShowCheckBox = true;
		this.datValidityFromHistory.Size = new System.Drawing.Size(130, 22);
		this.datValidityFromHistory.TabIndex = 2;
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.Location = new System.Drawing.Point(671, 17);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(90, 15);
		this.label61.TabIndex = 526;
		this.label61.Text = "Deliver To Party";
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(228, 13);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(130, 22);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(97, 13);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(130, 22);
		this.FromDateHistory.TabIndex = 0;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(964, 68);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(50, 25);
		this.btnshow.TabIndex = 10;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.CmbItemNameHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance265).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance265;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance266).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance266).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance266).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance266).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance266;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance267).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance267).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance267;
		((AppearanceBase)appearance269).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance269).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance269;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance270).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance270;
		((AppearanceBase)appearance271).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance271).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance271;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance171).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance171).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance171).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance171).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance171;
		((AppearanceBase)appearance273).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance273;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance274).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance274).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance274;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance275).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance275;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Location = new System.Drawing.Point(437, 11);
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Name = "CmbItemNameHistory";
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).TabIndex = 5;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(363, 17);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(67, 15);
		this.label37.TabIndex = 189;
		this.label37.Text = "Item Name";
		this.CmbParentItemHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbParentItemHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentItemHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentItemHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance246).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance246).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance246).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance246).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance246;
		((AppearanceBase)appearance257).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance257;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance268).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance268).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance268).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance268).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance268;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance279).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance279).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance279;
		((AppearanceBase)appearance290).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance290).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance290;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance236).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentItemHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbParentItemHistory).Location = new System.Drawing.Point(97, 67);
		((System.Windows.Forms.Control)(object)this.CmbParentItemHistory).Name = "CmbParentItemHistory";
		((System.Windows.Forms.Control)(object)this.CmbParentItemHistory).Size = new System.Drawing.Size(261, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentItemHistory).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbParentItemHistory).Leave += new System.EventHandler(CmbParentItemHistory_Leave);
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(1023, 47);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 13;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(1023, 30);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 12;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(5, 73);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(69, 15);
		this.label36.TabIndex = 187;
		this.label36.Text = "Parent Item";
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(1022, 13);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 11;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(5, 45);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(93, 15);
		this.label32.TabIndex = 185;
		this.label32.Text = "Validity From,To";
		this.CmbBuyerNameHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbBuyerNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBuyerNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBuyerNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance259).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance259).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance259).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance259;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance175;
		((SpecialBoxBase)((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance176).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance176).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance260).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance260).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance260;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance261;
		((AppearanceBase)appearance262).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance262).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance262;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance263).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance263).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance263).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance263).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance263;
		((AppearanceBase)appearance264).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance264;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBuyerNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameHistory).Location = new System.Drawing.Point(437, 67);
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameHistory).Name = "CmbBuyerNameHistory";
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameHistory).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameHistory).TabIndex = 7;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(363, 73);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(51, 15);
		this.label29.TabIndex = 182;
		this.label29.Text = "Supplier";
		this.CmbCommissionAgentHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCommissionAgentHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommissionAgentHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommissionAgentHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance183).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance183;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance184).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance184).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance106).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommissionAgentHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentHistory).Location = new System.Drawing.Point(437, 39);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentHistory).Name = "CmbCommissionAgentHistory";
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentHistory).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentHistory).TabIndex = 6;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(363, 37);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(73, 30);
		this.label31.TabIndex = 180;
		this.label31.Text = "Commission\r\nAgent";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(5, 17);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(91, 15);
		this.label33.TabIndex = 171;
		this.label33.Text = "Date From && To";
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label30);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 30);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1120, 29);
		this.panel4.TabIndex = 5;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.White;
		this.label30.Location = new System.Drawing.Point(4, 4);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(67, 19);
		this.label30.TabIndex = 0;
		this.label30.Text = "History";
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.toolStripSeparator3, this.BtnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(3, 3);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1120, 27);
		this.toolStrip2.TabIndex = 4;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.BtnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshHistory.Text = "&Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		this.txtAvgFillingPerBag.BackColor = System.Drawing.SystemColors.Control;
		this.txtAvgFillingPerBag.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAvgFillingPerBag.Enabled = false;
		this.txtAvgFillingPerBag.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAvgFillingPerBag.Location = new System.Drawing.Point(921, 120);
		this.txtAvgFillingPerBag.Name = "txtAvgFillingPerBag";
		this.txtAvgFillingPerBag.Size = new System.Drawing.Size(93, 23);
		this.txtAvgFillingPerBag.TabIndex = 20;
		this.txtAvgFillingPerBag.Tag = "double";
		this.txtAvgFillingPerBag.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.Location = new System.Drawing.Point(844, 116);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(66, 30);
		this.label62.TabIndex = 671;
		this.label62.Text = "Avg Filling \r\nPer Bag";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1134, 697);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "frmGrnLoadingChallanCmagt";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmGrnLoadingChallanCmagt_FormClosing);
		base.Load += new System.EventHandler(frmGrnLoadingChallanCmagt_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmGrnLoadingChallanCmagt_KeyDown);
		this.tabControl1.ResumeLayout(false);
		this.tabForm.ResumeLayout(false);
		this.tabForm.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdEmptyBags).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel14.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.groupBox6.ResumeLayout(false);
		this.groupBox6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryToParty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbShipToAddress).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPoNoDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbEmptyBagTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBuyerNameDetail).EndInit();
		this.panel1.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbVehicleType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransporter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUnloadingCity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLoadingCity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionAgent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierName).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranch).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCompanyName).EndInit();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabHistory.ResumeLayout(false);
		this.tabHistory.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.tabControl3.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdExpenseHistory).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.tabPage3.ResumeLayout(false);
		this.panel23.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdEmptyBagHistory).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel13.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliverToAddressHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryToPartyHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentItemHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBuyerNameHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionAgentHistory).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
