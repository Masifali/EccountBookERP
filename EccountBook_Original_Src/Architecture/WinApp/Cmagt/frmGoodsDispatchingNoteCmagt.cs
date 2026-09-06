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
using Architecture.WinApp.Inventory_Definition;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Cmagt;

public class frmGoodsDispatchingNoteCmagt : Form
{
	private Attachment AT = new Attachment();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private bool FormInitialized;

	private bool ItemSearchWithNameOrCode = true;

	private int RecId;

	private int updateDetailIndex = -1;

	private int DocumentTypeId = 1055;

	private int DefaultDaysToLessFromHistoryFromDate;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private string ScreenName = "frmGoodsDispatchingNoteCmagt";

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtDetail = new DataTable();

	private DataTable dtExpGrid = new DataTable();

	private DataTable dtEmptyBags = new DataTable();

	private DataTable dtHistoryForLayout = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtViewsData = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private DataTable dtHistoryCombo = new DataTable();

	private DataTable dtShipToAddress = new DataTable();

	private DataTable dtItemForEmptyBag = new DataTable();

	private DataTable dtPendingGrnData = new DataTable();

	private DataTable dtEbGridPackingTypes = new DataTable();

	private DataTable dtExpenseDataOrderWise = new DataTable();

	private DataTable dtEmptyBagDataOrderWise = new DataTable();

	private bool ShowWarningForLateVehicleArrivalAfterPoExpiryCommissionAgentPortal;

	private bool BlockEntryForLateVehicleArrivalAfterPoExpiryCommissionAgentPortal;

	private static bool CalculateByEbUnit = true;

	private static bool CalculateByEbTotal = false;

	private static int BaseFreightForCalculation = 1;

	private List<gdnBuyerDispatchDetail> lstRemoveRecordDetail = new List<gdnBuyerDispatchDetail>();

	private bool _internalChange;

	private DataSet LoadedDataSetToGetExpenseAndEbDetail = new DataSet();

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

	internal UltraCombo CmbBuyerName;

	private Label label7;

	internal UltraCombo CmbCommissionAgent;

	private Label label8;

	internal UltraCombo CmbLoadingCity;

	private Label label12;

	private TextBox txtRemarks;

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

	private Label label34;

	internal UltraCombo CmbGrnNoDetail;

	private Label label6;

	internal UltraCombo CmbUnloadingCity;

	private Label label1;

	private TextBox txtBiltyFreightGdn;

	private TextBox txtTransporterName;

	private Label label11;

	private Label label10;

	private Label label2;

	private Label label4;

	private Label label5;

	private TextBox txtVehicleQtyGdn;

	private TextBox txtBiltyNo;

	private TextBox txtOtherChargesGdn;

	private TextBox txtVehicleNo;

	private TextBox txtTotalFreightGdn;

	private DateTimePicker txtBiltyDate;

	private Label label13;

	private Label label44;

	private Label label42;

	private Label label41;

	private TextBox txtWbNetWeightGdn;

	private TextBox txtTareWeightGdn;

	private TextBox txtLoadWeightGdn;

	private Panel panel6;

	private Panel panel10;

	private Panel panel8;

	private Label label22;

	internal UltraCombo CmbDeliveryTermGdn;

	private Label label18;

	private Label label60;

	internal UltraCombo CmbShipToAddress;

	private TextBox txtDeliveryToAddress;

	private Label label40;

	private Button btnDefineShipToAddress;

	internal UltraCombo cmbVehicleType;

	private Label label49;

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

	private TextBox txtBuyerRefNo;

	private Label label52;

	private DateTimePicker datGrnDate;

	private ToolStripButton btnLoadGrn;

	private Label label16;

	private Label label15;

	private Label label14;

	private TextBox txtBillWeightGdn;

	internal UltraCombo CmbDeliveryTermGrn;

	private TextBox txtVehicleQtyGrn;

	private TextBox txtLoadWeightGrn;

	private TextBox txtTareWeightGrn;

	private TextBox txtWbNetWeightGrn;

	private TextBox txtBillWeightGrn;

	private Label label20;

	private Label label19;

	private TextBox txtTotalFreightGrn;

	private TextBox txtOtherChargesGrn;

	private TextBox txtBiltyFreightGrn;

	private TextBox txtFreightRemarks;

	private Label label21;

	private Label label24;

	private TextBox txtTotalFreightDiff;

	private TextBox txtBillWeightDiff;

	private TextBox txtWbNetWeightDiff;

	private TextBox txtTareWeightDiff;

	private TextBox txtLoadWeightDiff;

	private TextBox txtVehicleQtyDiff;

	private TextBox txtOtherChargesDiff;

	private TextBox txtBiltyFreightDiff;

	private Label label25;

	internal UltraCombo CmbDeliveryToParty;

	private Label label28;

	private Button BtnAllocatePmItems;

	private Label LabelWarningRemarks;

	private TextBox txtWarningRemarks;

	internal UltraCombo CmbDeliverToAddressHistory;

	internal UltraCombo CmbDeliveryToPartyHistory;

	private Label label35;

	private Label label38;

	private TextBox txtAvgFillingPerBagGrn;

	private Label label39;

	private TextBox txtAvgFillingPerBagDiff;

	private TextBox txtAvgFillingPerBagGdn;

	public frmGoodsDispatchingNoteCmagt(UserAccount user)
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
			base.AutoScrollMinSize = new Size(1330, 650);
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
			GdnBuyerDispatch_Helper.InitializeDetailtable(dtDetail);
			GdnBuyerDispatch_Helper.InitializeExpenseDetailtable(dtExpGrid);
			GdnBuyerDispatch_Helper.InitializeEmptyBagDetailtable(dtEmptyBags);
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
			dtItemForEmptyBag = CommonServices.GetPackingMaterialItemsAllocateToFlow(2);
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
					GrnNoBind();
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
			dtHistoryCombo = gdnBuyerDispatchMaster.GetDataForDropDown(obj);
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
			BlockEntryForLateVehicleArrivalAfterPoExpiryCommissionAgentPortal = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("BlockEntryForLateVehicleArrivalAfterPoExpiryCommissionAgentPortal"));
			ShowWarningForLateVehicleArrivalAfterPoExpiryCommissionAgentPortal = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ShowWarningForLateVehicleArrivalAfterPoExpiryCommissionAgentPortal"));
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
			return gdnBuyerDispatchMaster.GenerateCode(new ReportsParameters
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
		CommonBindings.SupplierBind(CmbBuyerName, dtSupplier, "Buyer Name");
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
			InfragisticsHelper.BindAndRetainSelection(CmbDeliveryTermGdn, dt, "Id", "Description", "Delivery Term", AllColumns: false, Conversion.ToInt(CmbDeliveryTermGdn.Value));
			InfragisticsHelper.BindAndRetainSelection(CmbDeliveryTermGrn, dt, "Id", "Description", "Delivery Term", AllColumns: false, Conversion.ToInt(CmbDeliveryTermGrn.Value));
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
				CmbDeliveryTermGdn.Value = deliveryTermId;
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
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			DataSet ds = grnSupplierLoadingMaster.PendingDataLoaderForGdn(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = RecId
			});
			dtPendingGrnData = ds?.Tables?[0].Copy();
			dtEmptyBagDataOrderWise = ds?.Tables?[1].Copy();
			dtExpenseDataOrderWise = ds?.Tables?[2].Copy();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GrnNoBind()
	{
		try
		{
			int buyerId = Conversion.ToInt(CmbBuyerName.Value);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("DocNo", typeof(string));
			dt.Columns.Add("buyerId", typeof(int));
			dt.Columns.Add("loadingCityId", typeof(int));
			dt.Columns.Add("unloadingCityId", typeof(int));
			dt.Columns.Add("transporterId", typeof(int));
			dt.Columns.Add("vehicleTypeId", typeof(int));
			dt.Columns.Add("vehicleNo", typeof(string));
			dt.Columns.Add("biltyNo", typeof(string));
			dt.Columns.Add("biltyDate", typeof(DateTime));
			dt.Columns.Add("deliveryTermId", typeof(int));
			dt.Columns.Add("DeliveryToPartyId", typeof(int));
			foreach (DataRow row in from r in dtPendingGrnData.AsEnumerable()
				where Conversion.ToInt(r["buyerId"]) == buyerId
				group r by Conversion.ToInt(r["grnSupplierLoadingMasterId"]) into g
				select g.First())
			{
				dt.Rows.Add(row["grnSupplierLoadingMasterId"], row["docNo"], row["buyerId"], row["loadingCityId"], row["unloadingCityId"], row["transporterId"], row["vehicleTypeId"], row["vehicleNo"], row["biltyNo"], row["biltyDate"], row["deliveryTermId"], row["DeliverToPartyId"]);
			}
			InfragisticsHelper.BindAndRetainSelection(CmbGrnNoDetail, dt, "Id", "DocNo", "Grn No", AllColumns: true, Conversion.ToInt(CmbGrnNoDetail.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbGrnNoDetail).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)CmbGrnNoDetail).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbGrnNoDetail).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbGrnNoDetail).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)CmbGrnNoDetail).DisplayLayout.Bands[0].Columns[5].Hidden = true;
				((UltraGridBase)CmbGrnNoDetail).DisplayLayout.Bands[0].Columns[6].Hidden = true;
				((UltraGridBase)CmbGrnNoDetail).DisplayLayout.Bands[0].Columns[7].Hidden = true;
				((UltraGridBase)CmbGrnNoDetail).DisplayLayout.Bands[0].Columns[8].Hidden = true;
				((UltraGridBase)CmbGrnNoDetail).DisplayLayout.Bands[0].Columns[9].Hidden = true;
				((UltraGridBase)CmbGrnNoDetail).DisplayLayout.Bands[0].Columns[10].Hidden = true;
				((UltraGridBase)CmbGrnNoDetail).DisplayLayout.Bands[0].Columns[11].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetEmptyBagDetailOnBaseOfGrnDetailSO()
	{
		try
		{
			DataTable EbTable = LoadedDataSetToGetExpenseAndEbDetail?.Tables[1];
			if (EbTable == null || EbTable.Rows.Count == 0)
			{
				return;
			}
			HashSet<int> saleOrderIds = (from GridEXRow item in grdDetail.GetDataRows()
				select Conversion.ToInt(item.Cells["SaleOrderId"].Value)).ToHashSet();
			HashSet<(int, int)> existingKeys = (from r in dtEmptyBags.AsEnumerable()
				select (PoId: Conversion.ToInt(r["SoId"]), PoEbId: Conversion.ToInt(r["SoEbId"]))).ToHashSet();
			EnumerableRowCollection<DataRow> enumerableRowCollection = from r in EbTable.AsEnumerable()
				where saleOrderIds.Contains(Conversion.ToInt(r["saleOrderMasterId"]))
				select r;
			if (enumerableRowCollection.Any() && !dtEmptyBags.AsEnumerable().Any((DataRow r) => Conversion.ToDecimal(r["PurchaseRate"]) > 0m))
			{
				dtEmptyBags.Rows.Clear();
			}
			foreach (DataRow row in enumerableRowCollection)
			{
				int soId = Conversion.ToInt(row["saleOrderMasterId"]);
				int soEbId = Conversion.ToInt(row["saleOrderEmptyBagDetailId"]);
				if (!existingKeys.Contains((soId, soEbId)))
				{
					dtEmptyBags.Rows.Add(0, soId, soEbId, row["SaleOrderNo"], row["PackingTypeId"], row["Rate"], row["emptyBagPackingMaterialItemId"]);
					existingKeys.Add((soId, soEbId));
				}
			}
			if (dtEmptyBags.Rows.Count == 0)
			{
				AddRowInvEmptyBagsGrid();
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

	private void GetExpensesOnBaseOfGrnDetailSO()
	{
		try
		{
			DataTable expenseTable = LoadedDataSetToGetExpenseAndEbDetail?.Tables[2];
			if (expenseTable == null)
			{
				return;
			}
			HashSet<int> saleOrderIds = (from GridEXRow item in grdDetail.GetDataRows()
				select Conversion.ToInt(item.Cells["SaleOrderId"].Value)).ToHashSet();
			HashSet<(int, int)> existingKeys = (from r in dtExpGrid.AsEnumerable()
				select (SoId: Conversion.ToInt(r["SoId"]), SoExpenseId: Conversion.ToInt(r["SoExpenseId"]))).ToHashSet();
			EnumerableRowCollection<DataRow> enumerableRowCollection = from r in expenseTable.AsEnumerable()
				where saleOrderIds.Contains(Conversion.ToInt(r["saleOrderMasterId"]))
				select r;
			if (enumerableRowCollection.Any() && !dtExpGrid.AsEnumerable().Any((DataRow r) => Conversion.ToDecimal(r["Amount"]) > 0m))
			{
				dtExpGrid.Rows.Clear();
			}
			foreach (DataRow row in enumerableRowCollection)
			{
				int soId = Conversion.ToInt(row["saleOrderMasterId"]);
				int soExpenseId = Conversion.ToInt(row["saleOrderbuyerExpenseDetailId"]);
				if (existingKeys.Add((soId, soExpenseId)))
				{
					dtExpGrid.Rows.Add(0, soId, soExpenseId, row["SaleOrderNo"], row["ItemId"], row["Qty"], row["rate"], row["amount"], row["remarks"]);
				}
			}
			if (dtExpGrid.Rows.Count == 0)
			{
				AddRowsInExpenseGrid();
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

	private void CmbGrnNoDetail_Leave(object sender, EventArgs e)
	{
		if (((UltraGridBase)CmbGrnNoDetail).ActiveRow != null && Conversion.ToInt(CmbGrnNoDetail.Value) > 0)
		{
			int transporterId = Conversion.ToInt(((UltraDropDownBase)CmbGrnNoDetail).SelectedRow.Cells["transporterId"].Value);
			string transporterName = Conversion.ToString(((UltraDropDownBase)CmbGrnNoDetail).SelectedRow.Cells["transporterName"].Value);
			if (transporterId > 0)
			{
				CmbTransporter.Value = transporterId;
				txtTransporterName.Text = transporterName;
			}
			else
			{
				txtTransporterName.Text = transporterName;
			}
			CmbLoadingCity.Value = Conversion.ToInt(((UltraDropDownBase)CmbGrnNoDetail).SelectedRow.Cells["loadingCityId"].Value);
			CmbUnloadingCity.Value = Conversion.ToInt(((UltraDropDownBase)CmbGrnNoDetail).SelectedRow.Cells["unloadingCityId"].Value);
			cmbVehicleType.Value = Conversion.ToInt(((UltraDropDownBase)CmbGrnNoDetail).SelectedRow.Cells["vehicleTypeId"].Value);
			txtVehicleNo.Text = Conversion.ToString(((UltraDropDownBase)CmbGrnNoDetail).SelectedRow.Cells["vehicleNo"].Value);
			txtBiltyNo.Text = Conversion.ToString(((UltraDropDownBase)CmbGrnNoDetail).SelectedRow.Cells["biltyNo"].Value);
			txtBiltyDate.Value = Conversion.ToDateTime(((UltraDropDownBase)CmbGrnNoDetail).SelectedRow.Cells["biltyDate"].Value);
			CmbDeliveryTermGdn.Value = Conversion.ToInt(((UltraDropDownBase)CmbGrnNoDetail).SelectedRow.Cells["deliveryTermId"].Value);
			CmbDeliveryTermGrn.Value = Conversion.ToInt(((UltraDropDownBase)CmbGrnNoDetail).SelectedRow.Cells["deliveryTermId"].Value);
			CmbDeliveryToParty.Value = Conversion.ToInt(((UltraDropDownBase)CmbGrnNoDetail).SelectedRow.Cells["DeliveryToPartyId"].Value);
		}
	}

	private void CmbBuyerName_Leave(object sender, EventArgs e)
	{
		try
		{
			GrnNoBind();
			int buyerId = Conversion.ToInt(CmbBuyerName.Value);
			if (Conversion.ToInt(CmbUnloadingCity.Value) == 0 && buyerId > 0)
			{
				UltraCombo cmbUnloadingCity = CmbUnloadingCity;
				UltraCombo cmbBuyerName = CmbBuyerName;
				object obj;
				if (cmbBuyerName == null)
				{
					obj = null;
				}
				else
				{
					UltraGridRow selectedRow = ((UltraDropDownBase)cmbBuyerName).SelectedRow;
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

	private Dictionary<string, EditType> EditColumnsList()
	{
		return new Dictionary<string, EditType>
		{
			{
				"Qty",
				(EditType)1
			},
			{
				"GrossWeight",
				(EditType)1
			},
			{
				"EbUnit",
				(EditType)1
			},
			{
				"EbTotal",
				(EditType)1
			},
			{
				"AddLessWeight",
				(EditType)1
			},
			{
				"Remarks",
				(EditType)1
			}
		};
	}

	private void grdDetailSetting(GridEX gridEX)
	{
		try
		{
			if (gridEX == null)
			{
				return;
			}
			GdnBuyerDispatch_Helper.DetailGridCommonSetting(gridEX, (((Control)(object)gridEX).Name == "grdDetail") ? EditColumnsList() : null);
			if (((Control)(object)gridEX).Name == "grdDetail")
			{
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
				grdDetail.UpdateData();
				((Control)(object)CmbBuyerName).Enabled = !GridEX_Helper.IsAnyCellValueGreaterThanZero(grdDetail, "PurchaseOrderId");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteDetailRow(GridEXRow r)
	{
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
				grdDetail.UpdateData();
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

	private void grdDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdDetail.UpdateData();
			GridEXRow r = grdDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0 && (e.Column.Key == "Qty" || e.Column.Key == "EbUnit" || e.Column.Key == "EbTotal" || e.Column.Key == "GrossWeight" || e.Column.Key == "AddLessWeight"))
			{
				decimal qty = Conversion.ToDecimal(r.Cells["Qty"].Value);
				decimal packEquivalent = Conversion.ToDecimal(r.Cells["PackUomEquivalent"].Value);
				decimal GrossWeight = Conversion.ToDecimal(r.Cells["GrossWeight"].Value);
				decimal EbUnit = Conversion.ToDecimal(r.Cells["EbUnit"].Value);
				decimal EbTotal = Conversion.ToDecimal(r.Cells["EbTotal"].Value);
				decimal addLessWeight = Conversion.ToDecimal(r.Cells["AddLessWeight"].Value);
				r.BeginEdit();
				if (e.Column.Key == "EbUnit")
				{
					EbTotal = ((EbUnit > 0m) ? (EbUnit * qty) : 0m);
					EbTotal = Math.Round(EbTotal, 3);
					r.Cells["EbTotal"].Value = EbTotal;
					CalculateByEbUnit = true;
					CalculateByEbTotal = false;
				}
				else if (e.Column.Key == "EbTotal")
				{
					EbUnit = ((EbTotal > 0m) ? (EbTotal / qty) : 0m);
					EbUnit = Math.Round(EbUnit, 3);
					r.Cells["EbUnit"].Value = EbUnit;
					CalculateByEbUnit = false;
					CalculateByEbTotal = true;
				}
				else if (CalculateByEbTotal)
				{
					EbUnit = ((EbTotal > 0m) ? (EbTotal / qty) : 0m);
					EbUnit = Math.Round(EbUnit, 3);
					r.Cells["EbUnit"].Value = EbUnit;
				}
				else if (CalculateByEbUnit)
				{
					EbTotal = ((EbUnit > 0m) ? (EbUnit * qty) : 0m);
					EbTotal = Math.Round(EbTotal, 3);
					r.Cells["EbTotal"].Value = EbTotal;
				}
				if (RecId == 0 && e.Column.Key == "Qty")
				{
					GrossWeight = qty * packEquivalent;
					r.Cells["GrossWeight"].Value = GrossWeight;
				}
				r.Cells["NetBillWeight"].Value = GrossWeight - EbTotal + addLessWeight;
				r.EndEdit();
			}
			grdDetail.UpdateData();
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
			GdnBuyerDispatch_Helper.ExpenseGridCommonSetting(gridEX, editableColumns);
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
				if (dt.AsEnumerable().Any((DataRow r) => Conversion.ToInt(r["SoId"]) > 0))
				{
					gridEX.RootTable.Columns["SaleOrderNo"].Visible = true;
				}
				else
				{
					gridEX.RootTable.Columns["SaleOrderNo"].Visible = false;
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
			GdnBuyerDispatch_Helper.EmptyBagGridCommonSetting(gridEX, editableColumns);
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
				if (dt.AsEnumerable().Any((DataRow r) => Conversion.ToInt(r["SoId"]) > 0))
				{
					gridEX.RootTable.Columns["SaleOrderNo"].Visible = true;
				}
				else
				{
					gridEX.RootTable.Columns["SaleOrderNo"].Visible = false;
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

	private void FillDetailListCommonForInsertAndDelete(gdnBuyerDispatchDetail vd, GridEXRow r)
	{
		vd.gdnBuyerDispatchDetailId = Conversion.ToInt(r.Cells["Id"].Value);
		vd.grnSupplierLoadingMasterId = Conversion.ToInt(r.Cells["GrnId"].Value);
		vd.grnSupplierLoadingDetailId = Conversion.ToInt(r.Cells["GrnDetailId"].Value);
		vd.GrnNo = Conversion.ToInt(r.Cells["GrnNo"].Value);
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
		vd.purchaseOrderMasterId = Conversion.ToInt(r.Cells["PurchaseOrderId"].Value);
		vd.purchaseOrderDetailId = Conversion.ToInt(r.Cells["PurchaseOrderDetailId"].Value);
		vd.PurchaseOrderNo = Conversion.ToInt(r.Cells["PurchaseOrderNo"].Value);
		vd.SOExpiryDate = Conversion.ToDateTime(r.Cells["SOExpiryDate"].Value);
		vd.saleOrderMasterId = Conversion.ToInt(r.Cells["SaleOrderId"].Value);
		vd.saleOrderDetailId = Conversion.ToInt(r.Cells["SaleOrderDetailId"].Value);
		vd.SaleOrderNo = Conversion.ToInt(r.Cells["SaleOrderNo"].Value);
		vd.SOExpiryDate = Conversion.ToDateTime(r.Cells["SOExpiryDate"].Value);
		vd.supplierId = Conversion.ToInt(r.Cells["SupplierId"].Value);
		vd.SupplierName = Conversion.ToString(r.Cells["SupplierName"].Value);
		vd.remarks = Conversion.ToString(r.Cells["Remarks"].Value);
		vd.warningRemarks = Conversion.ToString(r.Cells["WarningRemarks"].Value);
	}

	private void Insert()
	{
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Expected O, but got Unknown
		//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f7: Expected O, but got Unknown
		//IL_0a5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a63: Expected O, but got Unknown
		//IL_0bfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c03: Expected O, but got Unknown
		try
		{
			if (grdDetail.GetRows().Count() == 0)
			{
				throw new Exception("Detail Record Not Found");
			}
			string warningString = Conversion.ToString((object)txtWarningRemarks.Text);
			DateTime grnDate = Conversion.ToDateTime((object)datGrnDate.Text).Date;
			DateTime gdnDate = Conversion.ToDateTime((object)datDocDate.Text).Date;
			if (gdnDate < grnDate)
			{
				MessageBox.Show("Invalid Entry!\n\nGDN Date cannot be less than GRN Date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				datDocDate.Focus();
				return;
			}
			if (gdnDate == grnDate && string.IsNullOrEmpty(warningString))
			{
				if (!FormHelper.AskForRemarks("GRN Date and GDN Date are the same.\n\nPlease provide remarks to proceed.", txtWarningRemarks))
				{
					return;
				}
				LabelWarningRemarks.Visible = true;
				txtWarningRemarks.Visible = true;
			}
			_ = gdnDate == grnDate.AddDays(1.0);
			if (gdnDate > grnDate.AddDays(1.0) && string.IsNullOrEmpty(warningString))
			{
				if (!FormHelper.AskForRemarks("GDN Date is more than 1 day after GRN Date.\n\nPlease provide remarks to proceed.", txtWarningRemarks))
				{
					return;
				}
				LabelWarningRemarks.Visible = true;
				txtWarningRemarks.Visible = true;
			}
			List<(Control, string, FormHelper.ValidType)> controlsToValidate = new List<(Control, string, FormHelper.ValidType)>
			{
				((Control)(object)CmbCompanyName, "Company Name", FormHelper.ValidType.INT),
				((Control)(object)CmbBranch, "Branch Name", FormHelper.ValidType.INT),
				(txtDocNo, "Doc No", FormHelper.ValidType.INT),
				((Control)(object)CmbCommissionAgent, "Commission Agent", FormHelper.ValidType.INT),
				((Control)(object)CmbBuyerName, "Buyer Name", FormHelper.ValidType.INT),
				((Control)(object)CmbGrnNoDetail, "Grn No", FormHelper.ValidType.String),
				((Control)(object)CmbLoadingCity, "Loading City", FormHelper.ValidType.INT),
				((Control)(object)CmbUnloadingCity, "Un-Loading City", FormHelper.ValidType.INT),
				((Control)(object)cmbVehicleType, "Vehicle Type", FormHelper.ValidType.INT),
				(txtVehicleNo, "Vehicle No", FormHelper.ValidType.String),
				(txtBiltyNo, "Bilty No", FormHelper.ValidType.String),
				((Control)(object)CmbDeliveryTermGdn, "Delivery Term", FormHelper.ValidType.INT),
				(txtVehicleQtyGdn, "Vehicle Qty", FormHelper.ValidType.Double),
				(txtWbNetWeightGdn, "Net Wb Weight", FormHelper.ValidType.Double)
			};
			if (Conversion.ToDecimal((object)txtTotalFreightGdn.Text) > 0m && Conversion.ToInt(CmbTransporter.Value) == 0 && string.IsNullOrEmpty(txtTransporterName.Text))
			{
				MessageBox.Show("Please Select Transporter or Fill Transporter Name");
				CmbTransporter.Focus();
			}
			else
			{
				if (!FormHelper.ValidateControls(controlsToValidate) || !FormHelper.ConfirmAction((RecId == 0) ? "Are you sure to Save" : "Are you sure to Update"))
				{
					return;
				}
				gdnBuyerDispatchMaster obj = new gdnBuyerDispatchMaster();
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
				obj.gdnBuyerDispatchMasterId = RecId;
				obj.docNo = Conversion.ToInt((object)txtDocNo.Text);
				obj.docDate = Conversion.ToDateTime((object)datDocDate.Value);
				obj.commissionAgentId = Conversion.ToInt(CmbCommissionAgent.Value);
				obj.BuyerId = Conversion.ToInt(CmbBuyerName.Value);
				obj.BuyerRefDocNo = Conversion.ToString((object)txtBuyerRefNo.Text);
				obj.DeliverToPartyId = Conversion.ToInt(CmbDeliveryToParty.Value);
				obj.DeliverToPartyName = ((Control)(object)CmbDeliveryToParty).Text;
				obj.DeliverToAddressId = Conversion.ToInt(CmbShipToAddress.Value);
				obj.DeliverToAddress = txtDeliveryToAddress.Text;
				obj.loadingCityId = Conversion.ToInt(CmbLoadingCity.Value);
				obj.unloadingCityId = Conversion.ToInt(CmbUnloadingCity.Value);
				if (Conversion.ToDecimal((object)txtTotalFreightGdn.Text) > 0m)
				{
					obj.transporterId = Conversion.ToInt(CmbTransporter.Value);
					obj.transporterName = Conversion.ToString((object)txtTransporterName.Text);
					obj.biltyFreight = Conversion.ToDecimal((object)txtBiltyFreightGdn.Text);
					obj.otherAdLesCharges = Conversion.ToDecimal((object)txtOtherChargesGdn.Text);
					obj.totalFreight = Conversion.ToDecimal((object)txtTotalFreightGdn.Text);
				}
				obj.FreightRemarks = Conversion.ToString((object)txtFreightRemarks.Text);
				obj.vehicleTypeId = Conversion.ToInt(cmbVehicleType.Value);
				obj.vehicleNo = txtVehicleNo.Text.Trim();
				obj.biltyDate = Conversion.ToDateTime((object)txtBiltyDate.Value);
				obj.biltyNo = txtBiltyNo.Text.Trim();
				obj.deliveryTermId = Conversion.ToInt(CmbDeliveryTermGdn.Value);
				obj.biltyQty = Conversion.ToDecimal((object)txtVehicleQtyGdn.Text);
				obj.loadWeight = Conversion.ToDecimal((object)txtLoadWeightGdn.Text);
				obj.tareWeight = Conversion.ToDecimal((object)txtTareWeightGdn.Text);
				obj.scaleNetWeight = Conversion.ToDecimal((object)txtWbNetWeightGdn.Text);
				obj.BillWeight = Conversion.ToDecimal((object)txtBillWeightGdn.Text);
				obj.remarksHeader = txtRemarks.Text;
				obj.warningRemarks = txtWarningRemarks.Text;
				if (lstRemoveRecordDetail.Count > 0 && RecId > 0)
				{
					for (int i = 0; i < lstRemoveRecordDetail.Count; i++)
					{
						obj.GdnBuyerDispatchDetailList.Add(lstRemoveRecordDetail[i]);
					}
				}
				decimal grossWeight = default(decimal);
				decimal NetWeight = default(decimal);
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow r3 in rows)
				{
					gdnBuyerDispatchDetail vd = new gdnBuyerDispatchDetail();
					vd.gdnBuyerDispatchDetailId = ((RecId != 0) ? Conversion.ToInt(r3.Cells["Id"].Value) : 0);
					vd.actionTypeId = ((vd.gdnBuyerDispatchDetailId <= 0) ? 1 : 2);
					FillDetailListCommonForInsertAndDelete(vd, r3);
					FormHelper.ValidateField(vd.inventoryParentCategoryId, "Parent Category", r3.RowIndex);
					FormHelper.ValidateField(vd.itemId, "Item Name", r3.RowIndex);
					FormHelper.ValidateField(vd.cropYearId, "Crop Year", r3.RowIndex);
					FormHelper.ValidateField(vd.packingTypeId, "Packing Type", r3.RowIndex);
					FormHelper.ValidateField(vd.packUomId, "Pack Uom", r3.RowIndex);
					FormHelper.ValidateField(vd.loadingQty, "Loading Qty", r3.RowIndex);
					FormHelper.ValidateField(vd.wbGrossWeight, "Gross Weight", r3.RowIndex);
					FormHelper.ValidateField(vd.netBillWeight, "Net Bill Weight", r3.RowIndex);
					string WarningRemarks = "";
					if (vd.purchaseOrderMasterId > 0)
					{
						DateTime poExpiryDate = Conversion.ToDateTime(r3.Cells["POExpiryDate"].Value).Date;
						if (gdnDate == poExpiryDate)
						{
							r3.BeginEdit();
							r3.Cells["WarningRemarks"].Value = WarningRemarks;
							r3.EndEdit();
							vd.warningRemarks = WarningRemarks;
						}
						else if (gdnDate > poExpiryDate)
						{
							if (BlockEntryForLateVehicleArrivalAfterPoExpiryCommissionAgentPortal)
							{
								MessageBox.Show("Vehicle has reached late compared to PO Expiry Date.\n\nEntry is not allowed.", "Late Vehicle Entry Blocked", MessageBoxButtons.OK, MessageBoxIcon.Hand);
								datDocDate.Focus();
								return;
							}
							if (ShowWarningForLateVehicleArrivalAfterPoExpiryCommissionAgentPortal && string.IsNullOrWhiteSpace(vd.warningRemarks))
							{
								MessageBox.Show("Vehicle has reached late compared to PO Expiry Date.\n\nRemarks are required to proceed.", "Late Delivery Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
						}
					}
					grossWeight += vd.wbGrossWeight;
					NetWeight += vd.netBillWeight;
					obj.GdnBuyerDispatchDetailList.Add(vd);
				}
				rows = grdEmptyBags.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					if (Conversion.ToInt(r2.Cells["EmptyBagItem"].Value) > 0 && Conversion.ToInt(r2.Cells["PackingType"].Value) > 0)
					{
						gdnBuyerDispatchEmptyBagDetail emptyBags = new gdnBuyerDispatchEmptyBagDetail();
						emptyBags.saleOrderMasterId = Conversion.ToInt(r2.Cells["SoId"].Value);
						emptyBags.SaleOrderNo = Conversion.ToInt(r2.Cells["SaleOrderNo"].Value);
						emptyBags.saleOrderEmptyBagDetailId = Conversion.ToInt(r2.Cells["SoEbId"].Value);
						emptyBags.PackingTypeId = Conversion.ToInt(r2.Cells["PackingType"].Value);
						emptyBags.PackingType = Conversion.ToString((object)r2.Cells["PackingType"].Text);
						emptyBags.Rate = Conversion.ToDecimal(r2.Cells["PurchaseRate"].Value);
						emptyBags.emptyBagPackingMaterialItemId = Conversion.ToInt(r2.Cells["EmptyBagItem"].Value);
						emptyBags.EmptyBagItem = Conversion.ToString((object)r2.Cells["EmptyBagItem"].Text);
						obj.gdnBuyerDispatchEmptyBagDetailList.Add(emptyBags);
					}
				}
				rows = grdInvExp.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToInt((object)r.Cells["ItemId"].Value.ToString()) != 0 && Conversion.ToDouble(r.Cells["Amount"].Value) > 0.0)
					{
						gdnBuyerDispatchExpenseDetail PE = new gdnBuyerDispatchExpenseDetail();
						PE.saleOrderMasterId = Conversion.ToInt(r.Cells["SoId"].Value);
						PE.saleOrderExpenseDetailId = Conversion.ToInt(r.Cells["SoExpenseId"].Value);
						PE.SaleOrderNo = Conversion.ToInt(r.Cells["SaleOrderNo"].Value);
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
						obj.gdnBuyerDispatchExpenseDetailList.Add(PE);
					}
				}
				if (obj.scaleNetWeight != grossWeight)
				{
					throw new Exception("Header Net Weight:" + obj.scaleNetWeight.ToString("#,##0.###") + " not equal to detail gross weight:" + grossWeight.ToString("#,###0.###") + ". please Check!");
				}
				if (obj.BillWeight != NetWeight)
				{
					throw new Exception("Header Bill Weight:" + obj.BillWeight.ToString("#,##0.###") + " not equal to detail NetBillWeight:" + NetWeight.ToString("#,###0.###") + ". please Check!");
				}
				obj.AttachmentsList = new List<DMSAttachments>();
				obj.DeleteAttachmentsList = new List<DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<gdnBuyerDispatchMaster>(obj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				int success = gdnBuyerDispatchMaster.Save(obj);
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
					CrystalReportPrint_Helper.GdnBuyerDispatchSlip_1055(success);
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
			gdnBuyerDispatchMaster obj = gdnBuyerDispatchMaster.ReadById(recordId);
			if (obj == null || obj.GdnBuyerDispatchDetailList.Count == 0)
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
			CmbBuyerName.Value = Conversion.ToInt((object)obj.BuyerId);
			CmbBuyerName_Leave(null, null);
			((Control)(object)CmbBuyerName).Enabled = false;
			txtBuyerRefNo.Text = Conversion.ToString((object)obj.BuyerRefDocNo);
			if (Conversion.ToInt((object)obj.DeliverToPartyId) > 0)
			{
				CmbDeliveryToParty.Value = Conversion.ToInt((object)obj.DeliverToPartyId);
			}
			CmbDeliveryToParty_Leave(null, null);
			if (Conversion.ToInt((object)obj.DeliverToAddressId) > 0)
			{
				CmbShipToAddress.Value = Conversion.ToInt((object)obj.DeliverToAddressId);
			}
			txtDeliveryToAddress.Text = Conversion.ToString((object)obj.DeliverToAddress);
			CmbLoadingCity.Value = Conversion.ToInt((object)obj.loadingCityId);
			CmbUnloadingCity.Value = Conversion.ToInt((object)obj.unloadingCityId);
			if (Conversion.ToInt((object)obj.transporterId) > 0)
			{
				CmbTransporter.Value = Conversion.ToInt((object)obj.transporterId);
			}
			txtTransporterName.Text = Conversion.ToString((object)obj.transporterName);
			txtBiltyFreightGdn.Text = Conversion.ToDecimal((object)obj.biltyFreight).ToString("#,##0.###");
			txtOtherChargesGdn.Text = Conversion.ToDecimal((object)obj.otherAdLesCharges).ToString("#,##0.###");
			txtTotalFreightGdn.Text = Conversion.ToDecimal((object)obj.totalFreight).ToString("#,##0.###");
			cmbVehicleType.Value = Conversion.ToInt((object)obj.vehicleTypeId);
			txtVehicleNo.Text = Conversion.ToString((object)obj.vehicleNo);
			txtBiltyNo.Text = Conversion.ToString((object)obj.biltyNo);
			txtBiltyDate.Value = Conversion.ToDateTime((object)obj.biltyDate);
			CmbDeliveryTermGdn.Value = Conversion.ToInt((object)obj.deliveryTermId);
			txtLoadWeightGdn.Text = Conversion.ToDecimal((object)obj.loadWeight).ToString("#,##0.###");
			txtTareWeightGdn.Text = Conversion.ToDecimal((object)obj.tareWeight).ToString("#,##0.###");
			txtWbNetWeightGdn.Text = Conversion.ToDecimal((object)obj.scaleNetWeight).ToString("#,##0.###");
			txtWbNetWeightGdn.AccessibleDescription = Conversion.ToDecimal((object)obj.scaleNetWeight).ToString("#,##0.###");
			txtVehicleQtyGdn.Text = Conversion.ToDecimal((object)obj.biltyQty).ToString("#,##0.###");
			txtBillWeightGdn.Text = Conversion.ToDecimal((object)obj.BillWeight).ToString("#,##0.###");
			txtFreightRemarks.Text = Conversion.ToString((object)obj.FreightRemarks);
			txtRemarks.Text = Conversion.ToString((object)obj.remarksHeader);
			AttachmentsValues = obj.attachmentsValues;
			CustomAttachmentsValues = obj.customAttachmentsValues;
			txtWarningRemarks.Text = Conversion.ToString((object)obj.warningRemarks);
			if (!string.IsNullOrWhiteSpace(obj.warningRemarks))
			{
				LabelWarningRemarks.Visible = true;
				txtWarningRemarks.Visible = true;
			}
			dtDetail.Rows.Clear();
			GdnBuyerDispatch_Helper.FillDetailtableFromListCommonForReadById(obj, dtDetail);
			dtEmptyBags.Rows.Clear();
			GdnBuyerDispatch_Helper.FillEmptyBagTableFromListCommonForReadById(obj, dtEmptyBags);
			dtExpGrid.Rows.Clear();
			GdnBuyerDispatch_Helper.FillExpenseTableFromListCommonForReadById(obj, dtExpGrid);
			BindGrids();
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			int grnSupplierLoadingMasterId = obj.GdnBuyerDispatchDetailList[0].grnSupplierLoadingMasterId;
			CmbGrnNoDetail.Value = grnSupplierLoadingMasterId;
			datGrnDate.Value = obj.GdnBuyerDispatchDetailList[0].GrnDate;
			OutstandingOrdersdtFillDbCall();
			DataTable dt = (from r in dtPendingGrnData.AsEnumerable()
				where r.Field<int>("grnSupplierLoadingMasterId") == grnSupplierLoadingMasterId
				select r).CopyToDataTable();
			BindFromGrn(dt);
			GrnNoBind();
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
					gdnBuyerDispatchMaster.DeleteByID(UserAccount.ID, RecId);
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
			lstRemoveRecordDetail.Clear();
			RecId = 0;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			((Control)(object)CmbBuyerName).Enabled = true;
			LabelWarningRemarks.Visible = false;
			txtWarningRemarks.Visible = false;
			txtWarningRemarks.Text = string.Empty;
			((Control)(object)CmbCommissionAgent).Text = string.Empty;
			((Control)(object)CmbBuyerName).Text = string.Empty;
			((Control)(object)CmbDeliveryToParty).Text = string.Empty;
			txtBuyerRefNo.Text = string.Empty;
			((Control)(object)CmbShipToAddress).Text = string.Empty;
			txtDeliveryToAddress.Text = string.Empty;
			((Control)(object)CmbLoadingCity).Text = string.Empty;
			((Control)(object)CmbUnloadingCity).Text = string.Empty;
			txtVehicleNo.Text = string.Empty;
			txtBiltyNo.Text = string.Empty;
			((Control)(object)CmbDeliveryTermGrn).Text = string.Empty;
			txtVehicleQtyGrn.Text = string.Empty;
			txtLoadWeightGrn.Text = string.Empty;
			txtTareWeightGrn.Text = string.Empty;
			txtWbNetWeightGrn.Text = string.Empty;
			txtAvgFillingPerBagGrn.Text = string.Empty;
			txtBillWeightGrn.Text = string.Empty;
			((Control)(object)CmbDeliveryTermGdn).Text = string.Empty;
			txtVehicleQtyGdn.Text = string.Empty;
			txtLoadWeightGdn.Text = string.Empty;
			txtTareWeightGdn.Text = string.Empty;
			txtWbNetWeightGdn.Text = string.Empty;
			txtWbNetWeightGdn.AccessibleDescription = string.Empty;
			txtAvgFillingPerBagGdn.Text = string.Empty;
			txtBillWeightGdn.Text = string.Empty;
			txtBiltyFreightGrn.Text = string.Empty;
			txtOtherChargesGrn.Text = string.Empty;
			txtTotalFreightGrn.Text = string.Empty;
			txtBiltyFreightGdn.Text = string.Empty;
			txtOtherChargesGdn.Text = string.Empty;
			txtTotalFreightGdn.Text = string.Empty;
			txtBiltyFreightDiff.Text = string.Empty;
			txtOtherChargesDiff.Text = string.Empty;
			txtTotalFreightDiff.Text = string.Empty;
			((Control)(object)CmbTransporter).Text = string.Empty;
			txtTransporterName.Text = string.Empty;
			txtFreightRemarks.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			dtDetail.Rows.Clear();
			dtExpGrid.Rows.Clear();
			dtEmptyBags.Rows.Clear();
			BindGrids();
			OutstandingOrdersdtFillDbCall();
			GrnNoBind();
			txtDocNo.Text = DocumentNoDbCall();
			datDocDate.Focus();
			GetCommissionAgentConfigurationsFromGlobalandBind();
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
			List<string> allServices = new List<string> { "SupplierCustomerLists", "AllItems", "CropYear", "InvPackingType", "UomSchedule" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			GetConfigurationsFromGlobal();
			CompanyBind(CompanyDropdownData());
			BranchesBind(BranchesAllocationToUser.GetBranchsAllocatedToUser(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID));
			dtShipToAddress = DatatableHelper.AllShipToAddressDbCall(UserAccount);
			CommonBindings.SupplierDtFillFromGlobal(dtSupplier, useBusinessName: true);
			SupplierBind();
			CmbBuyerName_Leave(null, null);
			CmbDeliveryToParty_Leave(null, null);
			CityBindFromGlobal();
			DeliveryTermBind(DeliveryTerm.FormHistory());
			OutstandingOrdersdtFillDbCall();
			GrnNoBind();
			dtItemForEmptyBag = CommonServices.GetPackingMaterialItemsAllocateToFlow(2);
			GrdEmptyBagsRefresh();
			ExpenseGridCombBind();
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

	private void CalculateTotalFreight()
	{
		decimal bilty = Conversion.ToDecimal((object)txtBiltyFreightGdn.Text);
		decimal other = Conversion.ToDecimal((object)txtOtherChargesGdn.Text);
		if (BaseFreightForCalculation == 2)
		{
			decimal total = Conversion.ToDecimal((object)txtTotalFreightGdn.Text);
			if (base.ActiveControl?.Name == txtBiltyFreightGdn.Name)
			{
				txtBiltyFreightGdn.Text = (total - other).ToString();
			}
			else
			{
				txtBiltyFreightGdn.Text = (total - other).ToString("#,##0.###");
			}
		}
		else if (base.ActiveControl?.Name == txtTotalFreightGdn.Name)
		{
			txtTotalFreightGdn.Text = (bilty + other).ToString();
		}
		else
		{
			txtTotalFreightGdn.Text = (bilty + other).ToString("#,##0.###");
		}
	}

	private void txtBiltyFreightGrn_TextChanged(object sender, EventArgs e)
	{
		FormHelper.SetDifference(txtBiltyFreightGrn, txtBiltyFreightGdn, txtBiltyFreightDiff);
	}

	private void txtBiltyFreight_TextChanged(object sender, EventArgs e)
	{
		BaseFreightForCalculation = 1;
		FormHelper.SetDifference(txtBiltyFreightGrn, txtBiltyFreightGdn, txtBiltyFreightDiff);
		CalculateTotalFreight();
	}

	private void txtOtherChargesGrn_TextChanged(object sender, EventArgs e)
	{
		FormHelper.SetDifference(txtOtherChargesGrn, txtOtherChargesGdn, txtOtherChargesDiff);
	}

	private void txtOtherCharges_TextChanged(object sender, EventArgs e)
	{
		FormHelper.SetDifference(txtOtherChargesGrn, txtOtherChargesGdn, txtOtherChargesDiff);
		CalculateTotalFreight();
	}

	private void txtTotalFreightGrn_TextChanged(object sender, EventArgs e)
	{
		FormHelper.SetDifference(txtTotalFreightGrn, txtTotalFreightGdn, txtTotalFreightDiff);
	}

	private void txtTotalFreightGdn_TextChanged(object sender, EventArgs e)
	{
		BaseFreightForCalculation = 2;
		FormHelper.SetDifference(txtTotalFreightGrn, txtTotalFreightGdn, txtTotalFreightDiff);
		CalculateTotalFreight();
	}

	private void txtVehicleQtyGrn_TextChanged(object sender, EventArgs e)
	{
		FormHelper.SetDifference(txtVehicleQtyGrn, txtVehicleQtyGdn, txtVehicleQtyDiff);
		decimal qty = Conversion.ToDecimal((object)txtVehicleQtyGrn.Text);
		txtAvgFillingPerBagGrn.Text = ((qty > 0m) ? (Conversion.ToDecimal((object)txtWbNetWeightGrn.Text) / qty) : 0m).ToString("#,##0.###");
	}

	private void txtVehicleQtyGdn_TextChanged(object sender, EventArgs e)
	{
		FormHelper.SetDifference(txtVehicleQtyGrn, txtVehicleQtyGdn, txtVehicleQtyDiff);
		decimal qty = Conversion.ToDecimal((object)txtVehicleQtyGdn.Text);
		txtAvgFillingPerBagGdn.Text = ((qty > 0m) ? (Conversion.ToDecimal((object)txtWbNetWeightGdn.Text) / qty) : 0m).ToString("#,##0.###");
	}

	private void txtLoadWeightGrn_TextChanged(object sender, EventArgs e)
	{
		FormHelper.SetDifference(txtLoadWeightGrn, txtLoadWeightGdn, txtLoadWeightDiff);
	}

	private void txtLoadWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			FormHelper.SetDifference(txtLoadWeightGrn, txtLoadWeightGdn, txtLoadWeightDiff);
			CalculateGdnGrossWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtTareWeightGrn_TextChanged(object sender, EventArgs e)
	{
		FormHelper.SetDifference(txtTareWeightGrn, txtTareWeightGdn, txtTareWeightDiff);
	}

	private void txtTareWeight_TextChanged(object sender, EventArgs e)
	{
		FormHelper.SetDifference(txtTareWeightGrn, txtTareWeightGdn, txtTareWeightDiff);
		CalculateGdnGrossWeight();
	}

	private void CalculateGdnGrossWeight()
	{
		try
		{
			decimal num = Conversion.ToDecimal((object)txtLoadWeightGdn.Text);
			decimal TareWeight = Conversion.ToDecimal((object)txtTareWeightGdn.Text);
			decimal NetWeight = Math.Abs(num - TareWeight);
			txtWbNetWeightGdn.Text = NetWeight.ToString("#,##0.###");
			if (string.IsNullOrEmpty(txtWbNetWeightGdn.AccessibleDescription))
			{
				txtWbNetWeightGdn.AccessibleDescription = NetWeight.ToString("#,##0.###");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAvgFillingPerBag_TextChanged(object sender, EventArgs e)
	{
		FormHelper.SetDifference(txtAvgFillingPerBagGrn, txtAvgFillingPerBagGdn, txtAvgFillingPerBagDiff);
	}

	private void txtWbNetWeightGrn_TextChanged(object sender, EventArgs e)
	{
		FormHelper.SetDifference(txtWbNetWeightGrn, txtWbNetWeightGdn, txtWbNetWeightDiff);
		decimal qty = Conversion.ToDecimal((object)txtVehicleQtyGrn.Text);
		txtAvgFillingPerBagGrn.Text = ((qty > 0m) ? (Conversion.ToDecimal((object)txtWbNetWeightGrn.Text) / qty) : 0m).ToString("#,##0.###");
	}

	private void txtWbNetWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			FormHelper.SetDifference(txtWbNetWeightGrn, txtWbNetWeightGdn, txtWbNetWeightDiff);
			double NetWeight = Conversion.ToDouble((object)txtWbNetWeightGdn.Text);
			if (NetWeight != Conversion.ToDouble((object)txtWbNetWeightGdn.AccessibleDescription))
			{
				txtLoadWeightGdn.Text = string.Empty;
				txtTareWeightGdn.Text = string.Empty;
			}
			txtWbNetWeightGdn.Text = NetWeight.ToString("#,##0.###");
			txtWbNetWeightGdn.AccessibleDescription = NetWeight.ToString("#,##0.###");
			double AvgUom = GridEX_Helper.GetColumnAvg(grdDetail, "PackUomEquivalent");
			if (Conversion.ToDouble((object)txtVehicleQtyGdn.Text) == 0.0)
			{
				txtVehicleQtyGdn.Text = ((AvgUom > 0.0) ? (NetWeight / AvgUom) : 0.0).ToString("#,##0.###");
			}
			decimal qty = Conversion.ToDecimal((object)txtVehicleQtyGdn.Text);
			txtAvgFillingPerBagGdn.Text = ((qty > 0m) ? (Conversion.ToDecimal((object)txtWbNetWeightGdn.Text) / qty) : 0m).ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtBillWeightGrn_TextChanged(object sender, EventArgs e)
	{
		FormHelper.SetDifference(txtBillWeightGrn, txtBillWeightGdn, txtBillWeightDiff);
	}

	private void txtBillWeightGdn_TextChanged(object sender, EventArgs e)
	{
		FormHelper.SetDifference(txtBillWeightGrn, txtBillWeightGdn, txtBillWeightDiff);
	}

	private void txtLoadWeight_Leave(object sender, EventArgs e)
	{
		try
		{
			txtLoadWeightGdn.Text = Conversion.ToDecimal((object)txtLoadWeightGdn.Text).ToString("#,##0.####");
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
			txtTareWeightGdn.Text = Conversion.ToDecimal((object)txtTareWeightGdn.Text).ToString("#,##0.####");
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
			txtBiltyFreightGdn.Text = Conversion.ToDecimal((object)txtBiltyFreightGdn.Text).ToString("#,##0.####");
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
			txtOtherChargesGdn.Text = Conversion.ToDecimal((object)txtOtherChargesGdn.Text).ToString("#,##0.####");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtBillWeightGdn_Leave(object sender, EventArgs e)
	{
		txtBillWeightGdn.Text = Conversion.ToDecimal((object)txtBillWeightGdn.Text).ToString("#,##0.####");
	}

	private void txtTotalFreightGdn_Leave(object sender, EventArgs e)
	{
		txtTotalFreightGdn.Text = Conversion.ToDecimal((object)txtTotalFreightGdn.Text).ToString("#,##0.####");
	}

	private void btnAttachments_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				CrystalReportPrint_Helper.GrnSupplierLoadingSlip_1054(RecId);
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
			DataTable dtBase = new DataTable();
			dtBase.Columns.Add("Id", typeof(int));
			dtBase.Columns.Add("Name");
			DataTable dtCA = dtBase.Clone();
			DataTable dtBuyer = dtBase.Clone();
			DataTable dtItem = dtBase.Clone();
			DataTable dtParentCategory = dtBase.Clone();
			DataTable dtMostUsedPC = dtBase.Clone();
			DataTable dtDeliveryParty = dtBase.Clone();
			DataTable dtDeliverAddress = dtBase.Clone();
			Dictionary<string, DataTable> map = new Dictionary<string, DataTable>(StringComparer.OrdinalIgnoreCase)
			{
				{ "CommissionAgent", dtCA },
				{ "BuyerName", dtBuyer },
				{ "Item", dtItem },
				{ "DeliveryToParty", dtDeliveryParty },
				{ "DeliverToAddress", dtDeliverAddress }
			};
			DataTable dtMost = dtBase.Clone();
			if (dtcombo.Rows.Count > 0)
			{
				EnumerableRowCollection<DataRow> mostRow = from r in dtcombo.AsEnumerable()
					where Conversion.ToString(r["Activity"]) == "GetMostUsedParentCategoryId"
					select r;
				dtMost = (mostRow.Any() ? mostRow.CopyToDataTable() : dtBase.Clone());
				if (dtMost.Rows.Count == 1)
				{
					dtMostUsedPC.Rows.Add(dtMost.Rows[0]["Id"], dtMost.Rows[0]["ReferenceName"]);
				}
				int mostParentId = ((dtMost.Rows.Count > 0) ? Conversion.ToInt(dtMost.Rows[0]["Id"]) : (-1));
				foreach (DataRow r2 in dtcombo.Rows)
				{
					string activity = Conversion.ToString(r2["Activity"]);
					int parentId = Conversion.ToInt(r2["ParentCategoryId"]);
					DataTable table;
					if (activity == "ParentCategories")
					{
						dtParentCategory.Rows.Add(r2["Id"], r2["ReferenceName"]);
					}
					else if (parentId == mostParentId && map.TryGetValue(activity, out table))
					{
						table.Rows.Add(r2["Id"], r2["ReferenceName"]);
					}
				}
			}
			Bind(CmbCommissionAgentHistory, dtCA, "Commission Agent");
			Bind(CmbBuyerNameHistory, dtBuyer, "Buyer Name");
			Bind(CmbItemNameHistory, dtItem, "Item Name");
			Bind(CmbDeliveryToPartyHistory, dtDeliveryParty, "Deliver To Party");
			Bind(CmbDeliverToAddressHistory, dtDeliverAddress, "Deliver To Address");
			DDL.BindDDL(dtParentCategory, CmbParentItemHistory, "Id", "Name", "Parent Categories", ZeroIndex: false);
			((UltraGridBase)CmbParentItemHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			UltraGridBand band = ((UltraGridBase)CmbParentItemHistory).DisplayLayout.Bands[0];
			if (!((KeyedSubObjectsCollectionBase)band.Columns).Exists("Selected"))
			{
				UltraGridColumn obj = band.Columns.Add("Selected");
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
			}
			CmbParentItemHistory.LimitToList = false;
			CmbParentItemHistory.CheckedListSettings.CheckStateMember = "Selected";
			((EditorCheckedListSettings)CmbParentItemHistory.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
			((EditorCheckedListSettings)CmbParentItemHistory.CheckedListSettings).ListSeparator = ",";
			((EditorCheckedListSettings)CmbParentItemHistory.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
			if (dtMostUsedPC.Rows.Count > 0)
			{
				((Control)(object)CmbParentItemHistory).Text = Conversion.ToString(dtMostUsedPC.Rows[0]["Name"]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		static void Bind(UltraCombo combo, DataTable dt, string caption)
		{
			InfragisticsHelper.BindAndRetainSelection(combo, dt, "Id", "Name", caption, AllColumns: true, Conversion.ToInt(combo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
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
				["BuyerName"] = delegate(DataRow r)
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
			Bind(CmbBuyerNameHistory, dtBuyer, "Buyer Name");
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
			obj.BuyerId = Conversion.ToInt(CmbBuyerNameHistory.Value);
			obj.DeliverToPartyId = Conversion.ToInt(CmbDeliveryToPartyHistory.Value);
			obj.ShipToAddress = Conversion.ToString((object)((Control)(object)CmbDeliverToAddressHistory).Text);
			DataTable dtHistory = gdnBuyerDispatchMaster.FormHistory(obj);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("CommissionAgent", typeof(string));
				dt.Columns.Add("BuyerName", typeof(string));
				dt.Columns.Add("DeliverToParty", typeof(string));
				dt.Columns.Add("DeliverToAddress", typeof(string));
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
					dr["Id"] = Conversion.ToInt(row["gdnBuyerDispatchMasterId"]);
					dr["DocNo"] = Conversion.ToInt(row["docNo"]);
					dr["DocDate"] = Conversion.ToDateTime(row["docDate"]).ToShortDateString();
					dr["CommissionAgent"] = Conversion.ToString(row["CommissionAgentName"]);
					dr["BuyerName"] = Conversion.ToString(row["BuyerName"]);
					dr["DeliverToParty"] = Conversion.ToString(row["DeliverToPartyName"]);
					dr["DeliverToAddress"] = Conversion.ToString(row["DeliverToAddress"]);
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
			grdHistory.RootTable.Columns["BuyerName"].Width = Constants.InventoryConstants.SupplierName + 20;
			grdHistory.RootTable.Columns["DeliverToParty"].Width = Constants.InventoryConstants.SupplierName + 20;
			grdHistory.RootTable.Columns["DeliverToAddress"].Width = Constants.InventoryConstants.SupplierName + 20;
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
			grdHistory.FrozenColumns = 2;
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
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
					CrystalReportPrint_Helper.GrnSupplierLoadingSlip_1054(Id);
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
			gdnBuyerDispatchMaster Obj = gdnBuyerDispatchMaster.ReadById(Id);
			if (Obj != null && Obj.GdnBuyerDispatchDetailList.Count == 0)
			{
				grdHistoryDetail.DataSource = null;
				return;
			}
			DataTable dtDetailHistory = dtDetail.Clone();
			GdnBuyerDispatch_Helper.FillDetailtableFromListCommonForReadById(Obj, dtDetailHistory);
			grdHistoryDetail.DataSource = dtDetailHistory;
			grdHistoryDetail.RetrieveStructure();
			grddetailhistorySettings();
			DataTable dtEb = dtEmptyBags.Clone();
			dtEb.Columns.Add("PackType", typeof(string));
			dtEb.Columns.Add("EmptyBagItemName", typeof(string));
			GdnBuyerDispatch_Helper.FillEmptyBagTableFromListCommonForReadById(Obj, dtEb, AddExtraColumns: true);
			grdEmptyBagHistory.DataSource = dtEb;
			grdEmptyBagHistory.RetrieveStructure();
			gridEmptyBagsSettings(grdEmptyBagHistory);
			grdEmptyBagHistory.RootTable.Columns["PackingType"].Visible = false;
			grdEmptyBagHistory.RootTable.Columns["EmptyBagItem"].Visible = false;
			DataTable dtEx = dtExpGrid.Clone();
			dtEx.Columns.Add("OtherItemName", typeof(string));
			GdnBuyerDispatch_Helper.FillExpenseTableFromListCommonForReadById(Obj, dtEx, AddExtraColumns: true);
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
					((Control)(object)grdDetail).Focus();
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
					CrystalReportPrint_Helper.GrnSupplierLoadingSlip_1054(CurrId);
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

	private void BtnAllocatePmItems_Click(object sender, EventArgs e)
	{
		try
		{
			using frmPackingMaterialItemsAllocateToTransactionFlow obj = new frmPackingMaterialItemsAllocateToTransactionFlow(UserAccount);
			obj.ShowDialog();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadGrn_Click(object sender, EventArgs e)
	{
		try
		{
			using frmPendingGrnLoadingChallanLoader loadGrn = new frmPendingGrnLoadingChallanLoader(UserAccount);
			loadGrn.BuyerId = Conversion.ToInt(CmbBuyerName.Value);
			loadGrn.ShowDialog();
			loadGrn.BringToFront();
			LoadedDataSetToGetExpenseAndEbDetail = loadGrn.ds;
			LoadInGridDetail(loadGrn.dtLoader);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetail(DataTable dt)
	{
		try
		{
			if (dt.Rows.Count > 0)
			{
				if (grdDetail.GetDataRows().Any())
				{
					GridEXRow existingRow = grdDetail.GetDataRows().FirstOrDefault((GridEXRow r) => Conversion.ToInt(r.Cells["GrnId"].Value) > 0);
					if (existingRow != null)
					{
						int num = Conversion.ToInt(existingRow.Cells["GrnId"].Value);
						int incomingAgreementId = Conversion.ToInt(dt.Rows[0]["grnSupplierLoadingMasterId"]);
						if (num != incomingAgreementId)
						{
							throw new Exception("Already loaded rows belong to a different Grn. You cannot load rows from another Grn.");
						}
					}
					if (Conversion.ToInt(CmbBuyerName.Value) > 0 && Conversion.ToInt(dt.Rows[0]["BuyerId"]) != Conversion.ToInt(CmbBuyerName.Value))
					{
						throw new Exception("Already loaded rows belong to a different buyer. So you cannot load rows from another buyer.");
					}
				}
				string warningRemarks = string.Join(", ", (from row in dt.AsEnumerable()
					select row["warningRemarks"].ToString()).Distinct());
				txtRemarks.Text = warningRemarks;
				CmbBuyerName.Value = Conversion.ToInt(dt.Rows[0]["BuyerId"]);
				((Control)(object)CmbBuyerName).Enabled = false;
				CmbBuyerName_Leave(null, null);
				CmbDeliveryToParty.Value = Conversion.ToInt(dt.Rows[0]["DeliverToPartyId"]);
				CmbDeliveryToParty_Leave(null, null);
				int shipToAddressId = ((Conversion.ToInt(dt.Rows[0]["DeliverToAddressId"]) == 0) ? Conversion.ToInt(dt.Rows[0]["shipToAddressId"]) : Conversion.ToInt(dt.Rows[0]["DeliverToAddressId"]));
				CmbShipToAddress.Value = shipToAddressId;
				CmbShipToAddress_Leave(null, null);
				CmbGrnNoDetail.Value = Conversion.ToInt(dt.Rows[0]["grnSupplierLoadingMasterId"]);
				datGrnDate.Value = Conversion.ToDateTime(dt.Rows[0]["docDate"]);
				int transporterId = Conversion.ToInt(dt.Rows[0]["transporterId"]);
				string transporterName = Conversion.ToString(dt.Rows[0]["transporterName"]);
				if (transporterId > 0)
				{
					CmbTransporter.Value = transporterId;
					txtTransporterName.Text = transporterName;
				}
				else
				{
					txtTransporterName.Text = transporterName;
				}
				CmbLoadingCity.Value = Conversion.ToInt(dt.Rows[0]["loadingCityId"]);
				CmbUnloadingCity.Value = Conversion.ToInt(dt.Rows[0]["unloadingCityId"]);
				cmbVehicleType.Value = Conversion.ToInt(dt.Rows[0]["vehicleTypeId"]);
				txtVehicleNo.Text = Conversion.ToString(dt.Rows[0]["vehicleNo"]);
				txtBiltyNo.Text = Conversion.ToString(dt.Rows[0]["biltyNo"]);
				txtBiltyDate.Value = Conversion.ToDateTime(dt.Rows[0]["biltyDate"]);
				CmbDeliveryTermGdn.Value = Conversion.ToInt(dt.Rows[0]["deliveryTermId"]);
				BindFromGrn(dt);
				HashSet<int> existingDetailIds = new HashSet<int>(from r in grdDetail.GetRows()
					select Conversion.ToInt(r.Cells["GrnDetailId"].Value));
				foreach (DataRow dr in dt.Rows)
				{
					int DetailId = Conversion.ToInt(dr["DetailId"]);
					if (existingDetailIds.Add(DetailId))
					{
						DataRow newRow = dtDetail.NewRow();
						newRow["GrnId"] = Conversion.ToInt(dr["grnSupplierLoadingMasterId"]);
						newRow["GrnDetailId"] = dr["DetailId"];
						newRow["GrnNo"] = dr["docNo"];
						newRow["ParentItemId"] = dr["inventoryParentCategoryId"];
						newRow["ParentItem"] = dr["inventoryParentCategory"];
						int itemId = Conversion.ToInt(dr["itemId"]);
						newRow["ItemId"] = itemId;
						newRow["ItemName"] = ((itemId > 0) ? dr["ItemName"] : string.Empty);
						newRow["ItemCode"] = ((itemId > 0) ? dr["ItemCode"] : string.Empty);
						newRow["CropYearId"] = dr["cropYearId"];
						newRow["CropYear"] = dr["cropYear"];
						newRow["PackingTypeId"] = dr["packingTypeId"];
						newRow["PackingType"] = dr["PackingType"];
						newRow["PackUomId"] = dr["packUomId"];
						newRow["PackUom"] = dr["PackUomCode"];
						newRow["PackUomEquivalent"] = dr["PackUomEquivalent"];
						newRow["Qty"] = dr["BalanceQty"];
						newRow["GrossWeight"] = dr["BalanceGrossWeight"];
						newRow["EbUnit"] = dr["ebwPerUnit"];
						newRow["EbTotal"] = dr["ebwTotal"];
						newRow["AddLessWeight"] = dr["addLessWeight"];
						newRow["NetBillWeight"] = dr["BalanceWeight"];
						newRow["EmptyBagTermId"] = dr["EBWeightDeductionTermId"];
						newRow["EmptyBagTerm"] = dr["EmptyBagDeductionTerm"];
						newRow["PurchaseOrderId"] = dr["purchaseOrderMasterId"];
						newRow["PurchaseOrderDetailId"] = dr["purchaseOrderDetailId"];
						newRow["PurchaseOrderNo"] = dr["PurchaseOrderNo"];
						newRow["SaleOrderId"] = dr["saleOrderMasterId"];
						newRow["SaleOrderDetailId"] = dr["saleOrderDetailId"];
						newRow["SaleOrderNo"] = dr["SaleOrderNo"];
						newRow["SupplierId"] = dr["supplierId"];
						newRow["SupplierName"] = dr["SupplierName"];
						newRow["Remarks"] = "";
						dtDetail.Rows.Add(newRow);
					}
				}
			}
			grdDetail.DataSource = dtDetail;
			grdDetail.RetrieveStructure();
			grdDetailSetting(grdDetail);
			CalculationsCallForDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindFromGrn(DataTable dt)
	{
		try
		{
			CmbDeliveryTermGrn.Value = Conversion.ToInt(dt.Rows[0]["deliveryTermId"]);
			txtVehicleQtyGrn.Text = Conversion.ToDouble(dt.Rows[0]["biltyQty"]).ToString("#,##.###");
			txtLoadWeightGrn.Text = Conversion.ToDouble(dt.Rows[0]["loadWeight"]).ToString("#,##.###");
			txtTareWeightGrn.Text = Conversion.ToDouble(dt.Rows[0]["tareWeight"]).ToString("#,##.###");
			txtWbNetWeightGrn.Text = Conversion.ToDouble(dt.Rows[0]["scaleNetWeight"]).ToString("#,##.###");
			txtBillWeightGrn.Text = DatatableHelper.GetColumnSum(dt, "BalanceWeight").ToString("#,##.###");
			txtBiltyFreightGrn.Text = Conversion.ToDouble(dt.Rows[0]["biltyFreight"]).ToString("#,##.###");
			txtOtherChargesGrn.Text = Conversion.ToDouble(dt.Rows[0]["otherAdLesCharges"]).ToString("#,##.###");
			txtTotalFreightGrn.Text = Conversion.ToDouble(dt.Rows[0]["totalFreight"]).ToString("#,##.###");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CalculationsCallForDetail()
	{
		try
		{
			CalculateEbTotalThenBillWeight();
			GetExpensesOnBaseOfGrnDetailSO();
			GetEmptyBagDetailOnBaseOfGrnDetailSO();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CalculateEbTotalThenBillWeight()
	{
		try
		{
			GridEXRow[] dataRows = grdDetail.GetDataRows();
			foreach (GridEXRow obj in dataRows)
			{
				decimal qty = Conversion.ToDecimal(obj.Cells["Qty"].Value);
				decimal GrossWeight = Conversion.ToDecimal(obj.Cells["GrossWeight"].Value);
				decimal addLessWeight = Conversion.ToDecimal(obj.Cells["AddLessWeight"].Value);
				decimal EbUnit = Conversion.ToDecimal(obj.Cells["EbUnit"].Value);
				decimal ebTotal = qty * EbUnit;
				obj.BeginEdit();
				obj.Cells["EbTotal"].Value = ebTotal;
				obj.Cells["NetBillWeight"].Value = GrossWeight - ebTotal + addLessWeight;
				obj.EndEdit();
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
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Expected O, but got Unknown
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Expected O, but got Unknown
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Expected O, but got Unknown
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Expected O, but got Unknown
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Expected O, but got Unknown
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Expected O, but got Unknown
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Expected O, but got Unknown
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Expected O, but got Unknown
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Expected O, but got Unknown
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Expected O, but got Unknown
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Expected O, but got Unknown
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Expected O, but got Unknown
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Expected O, but got Unknown
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Expected O, but got Unknown
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Expected O, but got Unknown
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Expected O, but got Unknown
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Expected O, but got Unknown
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Expected O, but got Unknown
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Expected O, but got Unknown
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Expected O, but got Unknown
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Expected O, but got Unknown
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Expected O, but got Unknown
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_0507: Expected O, but got Unknown
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Expected O, but got Unknown
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Expected O, but got Unknown
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Expected O, but got Unknown
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Expected O, but got Unknown
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Expected O, but got Unknown
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Expected O, but got Unknown
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Expected O, but got Unknown
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Expected O, but got Unknown
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Expected O, but got Unknown
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Expected O, but got Unknown
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Expected O, but got Unknown
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Expected O, but got Unknown
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Expected O, but got Unknown
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Expected O, but got Unknown
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_0570: Expected O, but got Unknown
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Expected O, but got Unknown
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Expected O, but got Unknown
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Expected O, but got Unknown
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Expected O, but got Unknown
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0593: Expected O, but got Unknown
		//IL_0593: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Expected O, but got Unknown
		//IL_059a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a1: Expected O, but got Unknown
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Expected O, but got Unknown
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Expected O, but got Unknown
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
		//IL_0665: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Expected O, but got Unknown
		//IL_066c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0673: Expected O, but got Unknown
		//IL_0673: Unknown result type (might be due to invalid IL or missing references)
		//IL_067a: Expected O, but got Unknown
		//IL_0691: Unknown result type (might be due to invalid IL or missing references)
		//IL_069b: Expected O, but got Unknown
		//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bc: Expected O, but got Unknown
		//IL_06e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f3: Expected O, but got Unknown
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0756: Expected O, but got Unknown
		//IL_0778: Unknown result type (might be due to invalid IL or missing references)
		//IL_0782: Expected O, but got Unknown
		//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c4: Expected O, but got Unknown
		//IL_07c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cf: Expected O, but got Unknown
		//IL_0875: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Expected O, but got Unknown
		//IL_088b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0895: Expected O, but got Unknown
		//IL_08a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ab: Expected O, but got Unknown
		//IL_08d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e2: Expected O, but got Unknown
		//IL_09ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d4: Expected O, but got Unknown
		//IL_0aa6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab0: Expected O, but got Unknown
		//IL_0ab1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abb: Expected O, but got Unknown
		//IL_0add: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae7: Expected O, but got Unknown
		//IL_0b09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b13: Expected O, but got Unknown
		//IL_0b98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba2: Expected O, but got Unknown
		//IL_0bb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc3: Expected O, but got Unknown
		//IL_0be5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bef: Expected O, but got Unknown
		//IL_0c1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c26: Expected O, but got Unknown
		//IL_0c7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c89: Expected O, but got Unknown
		//IL_0c8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c94: Expected O, but got Unknown
		//IL_0c95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9f: Expected O, but got Unknown
		//IL_0ca0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0caa: Expected O, but got Unknown
		//IL_0cf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d02: Expected O, but got Unknown
		//IL_0d0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d18: Expected O, but got Unknown
		//IL_12b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c3: Expected O, but got Unknown
		//IL_12d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_12da: Expected O, but got Unknown
		//IL_14ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_14f6: Expected O, but got Unknown
		//IL_1503: Unknown result type (might be due to invalid IL or missing references)
		//IL_150d: Expected O, but got Unknown
		//IL_1835: Unknown result type (might be due to invalid IL or missing references)
		//IL_183f: Expected O, but got Unknown
		//IL_9005: Unknown result type (might be due to invalid IL or missing references)
		//IL_900f: Expected O, but got Unknown
		//IL_9033: Unknown result type (might be due to invalid IL or missing references)
		//IL_903d: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInvExp_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdEmptyBags_DesignTimeLayout = new GridEXLayout();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
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
		Appearance appearance206 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
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
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance117 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Cmagt.frmGoodsDispatchingNoteCmagt));
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdExpenseHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdEmptyBagHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance126 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
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
		Appearance appearance163 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
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
		this.BtnAllocatePmItems = new System.Windows.Forms.Button();
		this.panel14 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.CmbDeliveryToParty = new UltraCombo();
		this.label28 = new System.Windows.Forms.Label();
		this.txtFreightRemarks = new System.Windows.Forms.TextBox();
		this.txtTotalFreightGrn = new System.Windows.Forms.TextBox();
		this.CmbTransporter = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.txtOtherChargesGrn = new System.Windows.Forms.TextBox();
		this.label40 = new System.Windows.Forms.Label();
		this.txtBiltyFreightGrn = new System.Windows.Forms.TextBox();
		this.txtTransporterName = new System.Windows.Forms.TextBox();
		this.CmbDeliveryTermGrn = new UltraCombo();
		this.CmbDeliveryTermGdn = new UltraCombo();
		this.label21 = new System.Windows.Forms.Label();
		this.LabelWarningRemarks = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.datGrnDate = new System.Windows.Forms.DateTimePicker();
		this.txtBuyerRefNo = new System.Windows.Forms.TextBox();
		this.label52 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.txtVehicleQtyGrn = new System.Windows.Forms.TextBox();
		this.txtLoadWeightGrn = new System.Windows.Forms.TextBox();
		this.cmbVehicleType = new UltraCombo();
		this.txtTareWeightGrn = new System.Windows.Forms.TextBox();
		this.CmbUnloadingCity = new UltraCombo();
		this.txtWbNetWeightGrn = new System.Windows.Forms.TextBox();
		this.CmbGrnNoDetail = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.btnDefineShipToAddress = new System.Windows.Forms.Button();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbLoadingCity = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.txtTotalFreightDiff = new System.Windows.Forms.TextBox();
		this.txtTotalFreightGdn = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.txtBillWeightGrn = new System.Windows.Forms.TextBox();
		this.txtBillWeightDiff = new System.Windows.Forms.TextBox();
		this.txtBillWeightGdn = new System.Windows.Forms.TextBox();
		this.txtWbNetWeightDiff = new System.Windows.Forms.TextBox();
		this.txtWbNetWeightGdn = new System.Windows.Forms.TextBox();
		this.CmbShipToAddress = new UltraCombo();
		this.txtTareWeightDiff = new System.Windows.Forms.TextBox();
		this.txtTareWeightGdn = new System.Windows.Forms.TextBox();
		this.txtLoadWeightDiff = new System.Windows.Forms.TextBox();
		this.txtLoadWeightGdn = new System.Windows.Forms.TextBox();
		this.txtVehicleQtyDiff = new System.Windows.Forms.TextBox();
		this.txtVehicleQtyGdn = new System.Windows.Forms.TextBox();
		this.txtBiltyNo = new System.Windows.Forms.TextBox();
		this.txtOtherChargesDiff = new System.Windows.Forms.TextBox();
		this.txtOtherChargesGdn = new System.Windows.Forms.TextBox();
		this.txtBiltyFreightDiff = new System.Windows.Forms.TextBox();
		this.txtBiltyFreightGdn = new System.Windows.Forms.TextBox();
		this.txtBiltyDate = new System.Windows.Forms.DateTimePicker();
		this.datDocDate = new System.Windows.Forms.DateTimePicker();
		this.label17 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtWarningRemarks = new System.Windows.Forms.TextBox();
		this.txtDeliveryToAddress = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbCommissionAgent = new UltraCombo();
		this.CmbBuyerName = new UltraCombo();
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
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnLoadGrn = new System.Windows.Forms.ToolStripButton();
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
		this.CmbBuyerNameHistory = new UltraCombo();
		this.CmbCommissionAgentHistory = new UltraCombo();
		this.CmbDeliverToAddressHistory = new UltraCombo();
		this.CmbDeliveryToPartyHistory = new UltraCombo();
		this.label35 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.datValidityToHistory = new System.Windows.Forms.DateTimePicker();
		this.datValidityFromHistory = new System.Windows.Forms.DateTimePicker();
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
		this.label29 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label30 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.txtAvgFillingPerBagGdn = new System.Windows.Forms.TextBox();
		this.txtAvgFillingPerBagDiff = new System.Windows.Forms.TextBox();
		this.label39 = new System.Windows.Forms.Label();
		this.txtAvgFillingPerBagGrn = new System.Windows.Forms.TextBox();
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
		this.panel1.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryToParty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransporter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTermGrn).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTermGdn).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbVehicleType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUnloadingCity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGrnNoDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLoadingCity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbShipToAddress).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionAgent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBuyerName).BeginInit();
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
		((System.ComponentModel.ISupportInitialize)this.CmbBuyerNameHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionAgentHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliverToAddressHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryToPartyHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentItemHistory).BeginInit();
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
		this.tabControl1.Size = new System.Drawing.Size(1314, 611);
		this.tabControl1.TabIndex = 0;
		this.tabForm.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabForm.Controls.Add(this.panel6);
		this.tabForm.Controls.Add(this.panel14);
		this.tabForm.Controls.Add(this.panel1);
		this.tabForm.Controls.Add(this.panel3);
		this.tabForm.Controls.Add(this.toolStrip1);
		this.tabForm.Location = new System.Drawing.Point(4, 4);
		this.tabForm.Name = "tabForm";
		this.tabForm.Padding = new System.Windows.Forms.Padding(3);
		this.tabForm.Size = new System.Drawing.Size(1306, 582);
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
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(3, 311);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1300, 114);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.CellUpdated += new ColumnActionEventHandler(grdDetail_CellUpdated);
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetail_KeyDown);
		this.panel6.Controls.Add(this.panel8);
		this.panel6.Controls.Add(this.panel10);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel6.Location = new System.Drawing.Point(3, 425);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1300, 154);
		this.panel6.TabIndex = 134;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdInvExp);
		this.panel8.Controls.Add(this.panel5);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(515, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(785, 154);
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
		((System.Windows.Forms.Control)(object)this.grdInvExp).Size = new System.Drawing.Size(785, 132);
		((System.Windows.Forms.Control)(object)this.grdInvExp).TabIndex = 0;
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
		this.panel5.Size = new System.Drawing.Size(785, 22);
		this.panel5.TabIndex = 134;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(745, 1);
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
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Tag = "GrdEmptyBags";
		this.grdEmptyBags.TotalRow = (InheritableBoolean)1;
		this.grdEmptyBags.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmptyBags).VisualStyle = (VisualStyle)4;
		this.grdEmptyBags.ColumnButtonClick += new ColumnActionEventHandler(grdEmptyBags_ColumnButtonClick);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label27);
		this.panel7.Controls.Add(this.ctrlGrdBar4);
		this.panel7.Controls.Add(this.BtnAllocatePmItems);
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
		this.ctrlGrdBar4.Location = new System.Drawing.Point(469, 1);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar4.MyGrid = this.grdEmptyBags;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(43, 19);
		this.ctrlGrdBar4.TabIndex = 127;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.BtnAllocatePmItems.BackColor = System.Drawing.Color.FromArgb(192, 192, 0);
		this.BtnAllocatePmItems.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnAllocatePmItems.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAllocatePmItems.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.BtnAllocatePmItems.Location = new System.Drawing.Point(327, 0);
		this.BtnAllocatePmItems.Name = "BtnAllocatePmItems";
		this.BtnAllocatePmItems.Size = new System.Drawing.Size(141, 22);
		this.BtnAllocatePmItems.TabIndex = 9;
		this.BtnAllocatePmItems.TabStop = false;
		this.BtnAllocatePmItems.Text = "Pm Item Allocation To Flow";
		this.BtnAllocatePmItems.UseVisualStyleBackColor = false;
		this.BtnAllocatePmItems.Click += new System.EventHandler(BtnAllocatePmItems_Click);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.ctrlGrdBar2);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(3, 290);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1300, 21);
		this.panel14.TabIndex = 133;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1259, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.grdDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 20);
		this.ctrlGrdBar2.TabIndex = 127;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel1.Controls.Add(this.groupBox2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 61);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1300, 229);
		this.panel1.TabIndex = 3;
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty);
		this.groupBox2.Controls.Add(this.label28);
		this.groupBox2.Controls.Add(this.txtFreightRemarks);
		this.groupBox2.Controls.Add(this.txtTotalFreightGrn);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransporter);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add(this.txtOtherChargesGrn);
		this.groupBox2.Controls.Add(this.label40);
		this.groupBox2.Controls.Add(this.txtBiltyFreightGrn);
		this.groupBox2.Controls.Add(this.txtTransporterName);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryTermGrn);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryTermGdn);
		this.groupBox2.Controls.Add(this.label21);
		this.groupBox2.Controls.Add(this.LabelWarningRemarks);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.txtRemarks);
		this.groupBox2.Controls.Add(this.datGrnDate);
		this.groupBox2.Controls.Add(this.txtBuyerRefNo);
		this.groupBox2.Controls.Add(this.label52);
		this.groupBox2.Controls.Add(this.label25);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add(this.label20);
		this.groupBox2.Controls.Add(this.label19);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Controls.Add(this.label15);
		this.groupBox2.Controls.Add(this.txtVehicleQtyGrn);
		this.groupBox2.Controls.Add(this.txtLoadWeightGrn);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbVehicleType);
		this.groupBox2.Controls.Add(this.txtTareWeightGrn);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbUnloadingCity);
		this.groupBox2.Controls.Add(this.txtAvgFillingPerBagGrn);
		this.groupBox2.Controls.Add(this.txtWbNetWeightGrn);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGrnNoDetail);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.label49);
		this.groupBox2.Controls.Add(this.btnDefineShipToAddress);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLoadingCity);
		this.groupBox2.Controls.Add(this.label18);
		this.groupBox2.Controls.Add(this.txtVehicleNo);
		this.groupBox2.Controls.Add(this.txtTotalFreightDiff);
		this.groupBox2.Controls.Add(this.txtTotalFreightGdn);
		this.groupBox2.Controls.Add(this.label39);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add(this.label44);
		this.groupBox2.Controls.Add(this.label42);
		this.groupBox2.Controls.Add(this.label41);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.label34);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.label60);
		this.groupBox2.Controls.Add(this.txtBillWeightGrn);
		this.groupBox2.Controls.Add(this.txtBillWeightDiff);
		this.groupBox2.Controls.Add(this.txtAvgFillingPerBagDiff);
		this.groupBox2.Controls.Add(this.txtBillWeightGdn);
		this.groupBox2.Controls.Add(this.txtAvgFillingPerBagGdn);
		this.groupBox2.Controls.Add(this.txtWbNetWeightDiff);
		this.groupBox2.Controls.Add(this.txtWbNetWeightGdn);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbShipToAddress);
		this.groupBox2.Controls.Add(this.txtTareWeightDiff);
		this.groupBox2.Controls.Add(this.txtTareWeightGdn);
		this.groupBox2.Controls.Add(this.txtLoadWeightDiff);
		this.groupBox2.Controls.Add(this.txtLoadWeightGdn);
		this.groupBox2.Controls.Add(this.txtVehicleQtyDiff);
		this.groupBox2.Controls.Add(this.txtVehicleQtyGdn);
		this.groupBox2.Controls.Add(this.txtBiltyNo);
		this.groupBox2.Controls.Add(this.txtOtherChargesDiff);
		this.groupBox2.Controls.Add(this.txtOtherChargesGdn);
		this.groupBox2.Controls.Add(this.txtBiltyFreightDiff);
		this.groupBox2.Controls.Add(this.txtBiltyFreightGdn);
		this.groupBox2.Controls.Add(this.txtBiltyDate);
		this.groupBox2.Controls.Add(this.datDocDate);
		this.groupBox2.Controls.Add(this.label17);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.txtWarningRemarks);
		this.groupBox2.Controls.Add(this.txtDeliveryToAddress);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommissionAgent);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBuyerName);
		this.groupBox2.Controls.Add(this.txtDocNo);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(3, 0);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1299, 228);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main Information";
		this.CmbDeliveryToParty.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryToParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryToParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryToParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance95;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance97;
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
		((AppearanceBase)appearance76).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryToParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty).Location = new System.Drawing.Point(90, 144);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty).Name = "CmbDeliveryToParty";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty).Size = new System.Drawing.Size(221, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty).Leave += new System.EventHandler(CmbDeliveryToParty_Leave);
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(2, 145);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(82, 30);
		this.label28.TabIndex = 687;
		this.label28.Text = "Deliver / Ship \r\nTo Party";
		this.txtFreightRemarks.BackColor = System.Drawing.Color.White;
		this.txtFreightRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreightRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreightRemarks.Location = new System.Drawing.Point(1024, 153);
		this.txtFreightRemarks.Multiline = true;
		this.txtFreightRemarks.Name = "txtFreightRemarks";
		this.txtFreightRemarks.Size = new System.Drawing.Size(273, 44);
		this.txtFreightRemarks.TabIndex = 43;
		this.txtTotalFreightGrn.BackColor = System.Drawing.SystemColors.Control;
		this.txtTotalFreightGrn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalFreightGrn.Enabled = false;
		this.txtTotalFreightGrn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalFreightGrn.Location = new System.Drawing.Point(1024, 74);
		this.txtTotalFreightGrn.Name = "txtTotalFreightGrn";
		this.txtTotalFreightGrn.Size = new System.Drawing.Size(90, 23);
		this.txtTotalFreightGrn.TabIndex = 37;
		this.txtTotalFreightGrn.Tag = "double";
		this.txtTotalFreightGrn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTotalFreightGrn.TextChanged += new System.EventHandler(txtTotalFreightGrn_TextChanged);
		this.CmbTransporter.AllowNull = (DefaultableBoolean)1;
		this.CmbTransporter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransporter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransporter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance178).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance178).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransporter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance179).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance179;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransporter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance180).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance180).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance181).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance182).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance184).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance184).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance186).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance200).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance204).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbTransporter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Location = new System.Drawing.Point(1024, 100);
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Name = "CmbTransporter";
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Size = new System.Drawing.Size(273, 26);
		((System.Windows.Forms.Control)(object)this.CmbTransporter).TabIndex = 41;
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Leave += new System.EventHandler(CmbTransporter_Leave);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(943, 106);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(67, 15);
		this.label9.TabIndex = 33;
		this.label9.Text = "Transporter";
		this.txtOtherChargesGrn.BackColor = System.Drawing.SystemColors.Control;
		this.txtOtherChargesGrn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherChargesGrn.Enabled = false;
		this.txtOtherChargesGrn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOtherChargesGrn.Location = new System.Drawing.Point(1024, 49);
		this.txtOtherChargesGrn.Name = "txtOtherChargesGrn";
		this.txtOtherChargesGrn.Size = new System.Drawing.Size(90, 23);
		this.txtOtherChargesGrn.TabIndex = 36;
		this.txtOtherChargesGrn.Tag = "double";
		this.txtOtherChargesGrn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOtherChargesGrn.TextChanged += new System.EventHandler(txtOtherChargesGrn_TextChanged);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(943, 124);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(67, 30);
		this.label40.TabIndex = 33;
		this.label40.Text = "Transporter\r\nName";
		this.txtBiltyFreightGrn.BackColor = System.Drawing.SystemColors.Control;
		this.txtBiltyFreightGrn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBiltyFreightGrn.Enabled = false;
		this.txtBiltyFreightGrn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBiltyFreightGrn.Location = new System.Drawing.Point(1024, 23);
		this.txtBiltyFreightGrn.Name = "txtBiltyFreightGrn";
		this.txtBiltyFreightGrn.Size = new System.Drawing.Size(90, 23);
		this.txtBiltyFreightGrn.TabIndex = 35;
		this.txtBiltyFreightGrn.Tag = "double";
		this.txtBiltyFreightGrn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBiltyFreightGrn.TextChanged += new System.EventHandler(txtBiltyFreightGrn_TextChanged);
		this.txtTransporterName.BackColor = System.Drawing.Color.White;
		this.txtTransporterName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTransporterName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTransporterName.Location = new System.Drawing.Point(1024, 128);
		this.txtTransporterName.Name = "txtTransporterName";
		this.txtTransporterName.Size = new System.Drawing.Size(273, 23);
		this.txtTransporterName.TabIndex = 42;
		this.txtTransporterName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtTransporterName_KeyPress);
		this.CmbDeliveryTermGrn.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryTermGrn.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryTermGrn.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryTermGrn.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryTermGrn).DisplayLayout.Appearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.CmbDeliveryTermGrn).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryTermGrn).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance221).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance221).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance221).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTermGrn).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance221;
		((AppearanceBase)appearance222).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTermGrn).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance222;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTermGrn).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance223).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance223).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTermGrn).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.CmbDeliveryTermGrn).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryTermGrn).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance224).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryTermGrn).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance224;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance225).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryTermGrn).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.CmbDeliveryTermGrn).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryTermGrn).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTermGrn).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance226;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance227).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryTermGrn).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.CmbDeliveryTermGrn).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryTermGrn).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance228).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance228).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance228).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance228).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTermGrn).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance228;
		((AppearanceBase)appearance3).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryTermGrn).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbDeliveryTermGrn).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryTermGrn).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryTermGrn).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbDeliveryTermGrn).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryTermGrn).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbDeliveryTermGrn).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryTermGrn).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryTermGrn).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermGrn).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermGrn).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryTermGrn.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermGrn).Location = new System.Drawing.Point(620, 21);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermGrn).Name = "CmbDeliveryTermGrn";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermGrn).Size = new System.Drawing.Size(112, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermGrn).TabIndex = 18;
		this.CmbDeliveryTermGdn.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryTermGdn.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryTermGdn.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryTermGdn.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryTermGdn).DisplayLayout.Appearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.CmbDeliveryTermGdn).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryTermGdn).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance208).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance208).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance208).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTermGdn).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance208;
		((AppearanceBase)appearance209).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTermGdn).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance209;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTermGdn).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance210).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance210).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance210).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTermGdn).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.CmbDeliveryTermGdn).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryTermGdn).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance211).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryTermGdn).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance211;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryTermGdn).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.CmbDeliveryTermGdn).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryTermGdn).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTermGdn).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance214).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryTermGdn).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.CmbDeliveryTermGdn).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryTermGdn).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance215).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance215).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance215).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTermGdn).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance215;
		((AppearanceBase)appearance216).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryTermGdn).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.CmbDeliveryTermGdn).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryTermGdn).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance217).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryTermGdn).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.CmbDeliveryTermGdn).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryTermGdn).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.CmbDeliveryTermGdn).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryTermGdn).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryTermGdn).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermGdn).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermGdn).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryTermGdn.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermGdn).Location = new System.Drawing.Point(734, 21);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermGdn).Name = "CmbDeliveryTermGdn";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermGdn).Size = new System.Drawing.Size(112, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermGdn).TabIndex = 24;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(943, 159);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(52, 30);
		this.label21.TabIndex = 29;
		this.label21.Text = "Freight\r\nRemarks";
		this.LabelWarningRemarks.AutoSize = true;
		this.LabelWarningRemarks.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.LabelWarningRemarks.ForeColor = System.Drawing.Color.Red;
		this.LabelWarningRemarks.Location = new System.Drawing.Point(943, 195);
		this.LabelWarningRemarks.Name = "LabelWarningRemarks";
		this.LabelWarningRemarks.Size = new System.Drawing.Size(53, 30);
		this.LabelWarningRemarks.TabIndex = 29;
		this.LabelWarningRemarks.Text = "Warning\r\nRemarks";
		this.LabelWarningRemarks.Visible = false;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(315, 203);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(52, 15);
		this.label12.TabIndex = 29;
		this.label12.Text = "Remarks";
		this.txtRemarks.BackColor = System.Drawing.Color.White;
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(397, 199);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(541, 23);
		this.txtRemarks.TabIndex = 17;
		this.datGrnDate.CustomFormat = "dd-MMM-yy";
		this.datGrnDate.Enabled = false;
		this.datGrnDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.datGrnDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datGrnDate.Location = new System.Drawing.Point(181, 95);
		this.datGrnDate.Name = "datGrnDate";
		this.datGrnDate.Size = new System.Drawing.Size(130, 22);
		this.datGrnDate.TabIndex = 5;
		this.txtBuyerRefNo.BackColor = System.Drawing.Color.White;
		this.txtBuyerRefNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBuyerRefNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBuyerRefNo.Location = new System.Drawing.Point(90, 120);
		this.txtBuyerRefNo.Name = "txtBuyerRefNo";
		this.txtBuyerRefNo.Size = new System.Drawing.Size(221, 23);
		this.txtBuyerRefNo.TabIndex = 6;
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(2, 124);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(92, 15);
		this.label52.TabIndex = 685;
		this.label52.Text = "Buyer Ref Doc #";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label25.ForeColor = System.Drawing.Color.Maroon;
		this.label25.Location = new System.Drawing.Point(855, 24);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(77, 17);
		this.label25.TabIndex = 0;
		this.label25.Text = "Difference";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label24.ForeColor = System.Drawing.Color.Maroon;
		this.label24.Location = new System.Drawing.Point(1214, 4);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(77, 17);
		this.label24.TabIndex = 0;
		this.label24.Text = "Difference";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label20.ForeColor = System.Drawing.Color.Maroon;
		this.label20.Location = new System.Drawing.Point(1141, 4);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(39, 17);
		this.label20.TabIndex = 0;
		this.label20.Text = "GDN";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label19.ForeColor = System.Drawing.Color.Maroon;
		this.label19.Location = new System.Drawing.Point(1055, 4);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(39, 17);
		this.label19.TabIndex = 0;
		this.label19.Text = "GRN";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label16.ForeColor = System.Drawing.Color.Maroon;
		this.label16.Location = new System.Drawing.Point(774, 4);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(39, 17);
		this.label16.TabIndex = 0;
		this.label16.Text = "GDN";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label15.ForeColor = System.Drawing.Color.Maroon;
		this.label15.Location = new System.Drawing.Point(638, 4);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(39, 17);
		this.label15.TabIndex = 0;
		this.label15.Text = "GRN";
		this.txtVehicleQtyGrn.BackColor = System.Drawing.SystemColors.Control;
		this.txtVehicleQtyGrn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleQtyGrn.Enabled = false;
		this.txtVehicleQtyGrn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleQtyGrn.Location = new System.Drawing.Point(620, 49);
		this.txtVehicleQtyGrn.Name = "txtVehicleQtyGrn";
		this.txtVehicleQtyGrn.Size = new System.Drawing.Size(112, 23);
		this.txtVehicleQtyGrn.TabIndex = 19;
		this.txtVehicleQtyGrn.Tag = "double";
		this.txtVehicleQtyGrn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtVehicleQtyGrn.TextChanged += new System.EventHandler(txtVehicleQtyGrn_TextChanged);
		this.txtLoadWeightGrn.BackColor = System.Drawing.SystemColors.Control;
		this.txtLoadWeightGrn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLoadWeightGrn.Enabled = false;
		this.txtLoadWeightGrn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLoadWeightGrn.Location = new System.Drawing.Point(620, 74);
		this.txtLoadWeightGrn.Name = "txtLoadWeightGrn";
		this.txtLoadWeightGrn.Size = new System.Drawing.Size(112, 23);
		this.txtLoadWeightGrn.TabIndex = 20;
		this.txtLoadWeightGrn.Tag = "double";
		this.txtLoadWeightGrn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtLoadWeightGrn.TextChanged += new System.EventHandler(txtLoadWeightGrn_TextChanged);
		this.cmbVehicleType.AllowNull = (DefaultableBoolean)1;
		this.cmbVehicleType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbVehicleType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbVehicleType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbVehicleType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance73;
		((SpecialBoxBase)((UltraGridBase)this.cmbVehicleType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance87).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance75).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance90).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbVehicleType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).Location = new System.Drawing.Point(397, 66);
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).Name = "cmbVehicleType";
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).Size = new System.Drawing.Size(140, 26);
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).TabIndex = 13;
		this.txtTareWeightGrn.BackColor = System.Drawing.SystemColors.Control;
		this.txtTareWeightGrn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTareWeightGrn.Enabled = false;
		this.txtTareWeightGrn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTareWeightGrn.Location = new System.Drawing.Point(620, 99);
		this.txtTareWeightGrn.Name = "txtTareWeightGrn";
		this.txtTareWeightGrn.Size = new System.Drawing.Size(112, 23);
		this.txtTareWeightGrn.TabIndex = 21;
		this.txtTareWeightGrn.Tag = "double";
		this.txtTareWeightGrn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTareWeightGrn.TextChanged += new System.EventHandler(txtTareWeightGrn_TextChanged);
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
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance89).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance111).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbUnloadingCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbUnloadingCity).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbUnloadingCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbUnloadingCity).Location = new System.Drawing.Point(397, 38);
		((System.Windows.Forms.Control)(object)this.CmbUnloadingCity).Name = "CmbUnloadingCity";
		((System.Windows.Forms.Control)(object)this.CmbUnloadingCity).Size = new System.Drawing.Size(140, 26);
		((System.Windows.Forms.Control)(object)this.CmbUnloadingCity).TabIndex = 12;
		this.txtWbNetWeightGrn.BackColor = System.Drawing.SystemColors.Control;
		this.txtWbNetWeightGrn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWbNetWeightGrn.Enabled = false;
		this.txtWbNetWeightGrn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWbNetWeightGrn.Location = new System.Drawing.Point(620, 124);
		this.txtWbNetWeightGrn.Name = "txtWbNetWeightGrn";
		this.txtWbNetWeightGrn.Size = new System.Drawing.Size(112, 23);
		this.txtWbNetWeightGrn.TabIndex = 22;
		this.txtWbNetWeightGrn.Tag = "double";
		this.txtWbNetWeightGrn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtWbNetWeightGrn.TextChanged += new System.EventHandler(txtWbNetWeightGrn_TextChanged);
		this.CmbGrnNoDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbGrnNoDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGrnNoDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGrnNoDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGrnNoDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbGrnNoDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGrnNoDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGrnNoDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGrnNoDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance137;
		((SpecialBoxBase)((UltraGridBase)this.CmbGrnNoDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGrnNoDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbGrnNoDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGrnNoDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGrnNoDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGrnNoDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbGrnNoDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGrnNoDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGrnNoDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance155).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGrnNoDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbGrnNoDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGrnNoDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance156).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance156).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance156).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGrnNoDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGrnNoDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbGrnNoDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGrnNoDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGrnNoDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbGrnNoDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGrnNoDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbGrnNoDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGrnNoDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGrnNoDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGrnNoDetail).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbGrnNoDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbGrnNoDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbGrnNoDetail).Location = new System.Drawing.Point(90, 93);
		((System.Windows.Forms.Control)(object)this.CmbGrnNoDetail).Name = "CmbGrnNoDetail";
		((System.Windows.Forms.Control)(object)this.CmbGrnNoDetail).Size = new System.Drawing.Size(90, 26);
		((System.Windows.Forms.Control)(object)this.CmbGrnNoDetail).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbGrnNoDetail).Leave += new System.EventHandler(CmbGrnNoDetail_Leave);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(315, 16);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(73, 15);
		this.label8.TabIndex = 20;
		this.label8.Text = "Loading City";
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(315, 72);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(73, 15);
		this.label49.TabIndex = 675;
		this.label49.Text = "Vehicle Type";
		this.btnDefineShipToAddress.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnDefineShipToAddress.Location = new System.Drawing.Point(286, 171);
		this.btnDefineShipToAddress.Name = "btnDefineShipToAddress";
		this.btnDefineShipToAddress.Size = new System.Drawing.Size(26, 26);
		this.btnDefineShipToAddress.TabIndex = 9;
		this.btnDefineShipToAddress.TabStop = false;
		this.btnDefineShipToAddress.UseVisualStyleBackColor = true;
		this.btnDefineShipToAddress.Click += new System.EventHandler(btnDefineShipToAddress_Click);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(315, 44);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(85, 15);
		this.label6.TabIndex = 20;
		this.label6.Text = "Unloading City";
		this.CmbLoadingCity.AllowNull = (DefaultableBoolean)1;
		this.CmbLoadingCity.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLoadingCity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLoadingCity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLoadingCity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance144;
		((SpecialBoxBase)((UltraGridBase)this.CmbLoadingCity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance145).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance145).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance149).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance150).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance150).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance150).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance152).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLoadingCity).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLoadingCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLoadingCity).Location = new System.Drawing.Point(397, 10);
		((System.Windows.Forms.Control)(object)this.CmbLoadingCity).Name = "CmbLoadingCity";
		((System.Windows.Forms.Control)(object)this.CmbLoadingCity).Size = new System.Drawing.Size(140, 26);
		((System.Windows.Forms.Control)(object)this.CmbLoadingCity).TabIndex = 11;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(541, 27);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(80, 15);
		this.label18.TabIndex = 673;
		this.label18.Text = "Delivery Term";
		this.txtVehicleNo.BackColor = System.Drawing.Color.White;
		this.txtVehicleNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleNo.Enabled = false;
		this.txtVehicleNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleNo.Location = new System.Drawing.Point(397, 94);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.Size = new System.Drawing.Size(140, 23);
		this.txtVehicleNo.TabIndex = 14;
		this.txtVehicleNo.Tag = "";
		this.txtVehicleNo.TextChanged += new System.EventHandler(txtVehicleNo_TextChanged);
		this.txtVehicleNo.KeyDown += new System.Windows.Forms.KeyEventHandler(txtvehicleno_KeyDown);
		this.txtVehicleNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtvehicleno_KeyPress);
		this.txtVehicleNo.KeyUp += new System.Windows.Forms.KeyEventHandler(txtvehicleno_KeyUp);
		this.txtVehicleNo.Leave += new System.EventHandler(txtvehicleno_Leave);
		this.txtTotalFreightDiff.BackColor = System.Drawing.SystemColors.Control;
		this.txtTotalFreightDiff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalFreightDiff.Enabled = false;
		this.txtTotalFreightDiff.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalFreightDiff.Location = new System.Drawing.Point(1207, 74);
		this.txtTotalFreightDiff.Name = "txtTotalFreightDiff";
		this.txtTotalFreightDiff.Size = new System.Drawing.Size(90, 23);
		this.txtTotalFreightDiff.TabIndex = 34;
		this.txtTotalFreightDiff.Tag = "double";
		this.txtTotalFreightDiff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTotalFreightGdn.BackColor = System.Drawing.Color.White;
		this.txtTotalFreightGdn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalFreightGdn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalFreightGdn.Location = new System.Drawing.Point(1115, 74);
		this.txtTotalFreightGdn.Name = "txtTotalFreightGdn";
		this.txtTotalFreightGdn.Size = new System.Drawing.Size(90, 23);
		this.txtTotalFreightGdn.TabIndex = 40;
		this.txtTotalFreightGdn.Tag = "double";
		this.txtTotalFreightGdn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTotalFreightGdn.TextChanged += new System.EventHandler(txtTotalFreightGdn_TextChanged);
		this.txtTotalFreightGdn.Leave += new System.EventHandler(txtTotalFreightGdn_Leave);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.Maroon;
		this.label14.Location = new System.Drawing.Point(541, 178);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(68, 15);
		this.label14.TabIndex = 671;
		this.label14.Text = "Bill Weight";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(541, 128);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(68, 15);
		this.label44.TabIndex = 671;
		this.label44.Text = "Net Weight";
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(541, 78);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(75, 15);
		this.label42.TabIndex = 671;
		this.label42.Text = "Load Weight";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(541, 103);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(71, 15);
		this.label41.TabIndex = 671;
		this.label41.Text = "Tare Weight";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(541, 45);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(67, 30);
		this.label11.TabIndex = 671;
		this.label11.Text = "Vehicle\r\nNo Of Bags";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(315, 150);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(49, 15);
		this.label10.TabIndex = 671;
		this.label10.Text = "Bilty No";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(943, 54);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(81, 13);
		this.label2.TabIndex = 671;
		this.label2.Text = "Other Charges";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(2, 99);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(85, 15);
		this.label34.TabIndex = 661;
		this.label34.Text = "Grn No && Date";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(943, 78);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(73, 15);
		this.label4.TabIndex = 671;
		this.label4.Text = "Total Freight";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(315, 98);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(64, 15);
		this.label5.TabIndex = 671;
		this.label5.Text = "Vehicle No";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(943, 27);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(70, 15);
		this.label1.TabIndex = 671;
		this.label1.Text = "Bilty Freight";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.Location = new System.Drawing.Point(2, 184);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(82, 30);
		this.label60.TabIndex = 683;
		this.label60.Text = "Delivery /Ship\r\nTo Address";
		this.txtBillWeightGrn.BackColor = System.Drawing.SystemColors.Control;
		this.txtBillWeightGrn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBillWeightGrn.Enabled = false;
		this.txtBillWeightGrn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBillWeightGrn.Location = new System.Drawing.Point(620, 174);
		this.txtBillWeightGrn.Name = "txtBillWeightGrn";
		this.txtBillWeightGrn.Size = new System.Drawing.Size(112, 23);
		this.txtBillWeightGrn.TabIndex = 23;
		this.txtBillWeightGrn.Tag = "double";
		this.txtBillWeightGrn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBillWeightGrn.TextChanged += new System.EventHandler(txtBillWeightGrn_TextChanged);
		this.txtBillWeightDiff.BackColor = System.Drawing.SystemColors.Control;
		this.txtBillWeightDiff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBillWeightDiff.Enabled = false;
		this.txtBillWeightDiff.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBillWeightDiff.Location = new System.Drawing.Point(848, 174);
		this.txtBillWeightDiff.Name = "txtBillWeightDiff";
		this.txtBillWeightDiff.Size = new System.Drawing.Size(90, 23);
		this.txtBillWeightDiff.TabIndex = 34;
		this.txtBillWeightDiff.Tag = "double";
		this.txtBillWeightDiff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBillWeightGdn.BackColor = System.Drawing.Color.White;
		this.txtBillWeightGdn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBillWeightGdn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBillWeightGdn.Location = new System.Drawing.Point(734, 174);
		this.txtBillWeightGdn.Name = "txtBillWeightGdn";
		this.txtBillWeightGdn.Size = new System.Drawing.Size(112, 23);
		this.txtBillWeightGdn.TabIndex = 29;
		this.txtBillWeightGdn.Tag = "double";
		this.txtBillWeightGdn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBillWeightGdn.TextChanged += new System.EventHandler(txtBillWeightGdn_TextChanged);
		this.txtBillWeightGdn.Leave += new System.EventHandler(txtBillWeightGdn_Leave);
		this.txtWbNetWeightDiff.BackColor = System.Drawing.SystemColors.Control;
		this.txtWbNetWeightDiff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWbNetWeightDiff.Enabled = false;
		this.txtWbNetWeightDiff.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWbNetWeightDiff.Location = new System.Drawing.Point(848, 124);
		this.txtWbNetWeightDiff.Name = "txtWbNetWeightDiff";
		this.txtWbNetWeightDiff.Size = new System.Drawing.Size(90, 23);
		this.txtWbNetWeightDiff.TabIndex = 33;
		this.txtWbNetWeightDiff.Tag = "double";
		this.txtWbNetWeightDiff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtWbNetWeightGdn.BackColor = System.Drawing.Color.White;
		this.txtWbNetWeightGdn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWbNetWeightGdn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWbNetWeightGdn.Location = new System.Drawing.Point(734, 124);
		this.txtWbNetWeightGdn.Name = "txtWbNetWeightGdn";
		this.txtWbNetWeightGdn.Size = new System.Drawing.Size(112, 23);
		this.txtWbNetWeightGdn.TabIndex = 28;
		this.txtWbNetWeightGdn.Tag = "double";
		this.txtWbNetWeightGdn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtWbNetWeightGdn.Leave += new System.EventHandler(txtWbNetWeight_TextChanged);
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
		((System.Windows.Forms.Control)(object)this.CmbShipToAddress).Location = new System.Drawing.Point(90, 171);
		((System.Windows.Forms.Control)(object)this.CmbShipToAddress).Name = "CmbShipToAddress";
		((System.Windows.Forms.Control)(object)this.CmbShipToAddress).Size = new System.Drawing.Size(195, 26);
		((System.Windows.Forms.Control)(object)this.CmbShipToAddress).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.CmbShipToAddress).Leave += new System.EventHandler(CmbShipToAddress_Leave);
		this.txtTareWeightDiff.BackColor = System.Drawing.SystemColors.Control;
		this.txtTareWeightDiff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTareWeightDiff.Enabled = false;
		this.txtTareWeightDiff.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTareWeightDiff.Location = new System.Drawing.Point(848, 99);
		this.txtTareWeightDiff.Name = "txtTareWeightDiff";
		this.txtTareWeightDiff.Size = new System.Drawing.Size(90, 23);
		this.txtTareWeightDiff.TabIndex = 32;
		this.txtTareWeightDiff.Tag = "double";
		this.txtTareWeightDiff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTareWeightGdn.BackColor = System.Drawing.Color.White;
		this.txtTareWeightGdn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTareWeightGdn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTareWeightGdn.Location = new System.Drawing.Point(734, 99);
		this.txtTareWeightGdn.Name = "txtTareWeightGdn";
		this.txtTareWeightGdn.Size = new System.Drawing.Size(112, 23);
		this.txtTareWeightGdn.TabIndex = 27;
		this.txtTareWeightGdn.Tag = "double";
		this.txtTareWeightGdn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTareWeightGdn.TextChanged += new System.EventHandler(txtTareWeight_TextChanged);
		this.txtTareWeightGdn.Leave += new System.EventHandler(txtTareWeight_Leave);
		this.txtLoadWeightDiff.BackColor = System.Drawing.SystemColors.Control;
		this.txtLoadWeightDiff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLoadWeightDiff.Enabled = false;
		this.txtLoadWeightDiff.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLoadWeightDiff.Location = new System.Drawing.Point(848, 74);
		this.txtLoadWeightDiff.Name = "txtLoadWeightDiff";
		this.txtLoadWeightDiff.Size = new System.Drawing.Size(90, 23);
		this.txtLoadWeightDiff.TabIndex = 31;
		this.txtLoadWeightDiff.Tag = "double";
		this.txtLoadWeightDiff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtLoadWeightGdn.BackColor = System.Drawing.Color.White;
		this.txtLoadWeightGdn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLoadWeightGdn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLoadWeightGdn.Location = new System.Drawing.Point(734, 74);
		this.txtLoadWeightGdn.Name = "txtLoadWeightGdn";
		this.txtLoadWeightGdn.Size = new System.Drawing.Size(112, 23);
		this.txtLoadWeightGdn.TabIndex = 26;
		this.txtLoadWeightGdn.Tag = "double";
		this.txtLoadWeightGdn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtLoadWeightGdn.TextChanged += new System.EventHandler(txtLoadWeight_TextChanged);
		this.txtLoadWeightGdn.Leave += new System.EventHandler(txtLoadWeight_Leave);
		this.txtVehicleQtyDiff.BackColor = System.Drawing.SystemColors.Control;
		this.txtVehicleQtyDiff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleQtyDiff.Enabled = false;
		this.txtVehicleQtyDiff.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleQtyDiff.Location = new System.Drawing.Point(848, 49);
		this.txtVehicleQtyDiff.Name = "txtVehicleQtyDiff";
		this.txtVehicleQtyDiff.Size = new System.Drawing.Size(90, 23);
		this.txtVehicleQtyDiff.TabIndex = 30;
		this.txtVehicleQtyDiff.Tag = "double";
		this.txtVehicleQtyDiff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtVehicleQtyGdn.BackColor = System.Drawing.Color.White;
		this.txtVehicleQtyGdn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleQtyGdn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleQtyGdn.Location = new System.Drawing.Point(734, 49);
		this.txtVehicleQtyGdn.Name = "txtVehicleQtyGdn";
		this.txtVehicleQtyGdn.Size = new System.Drawing.Size(112, 23);
		this.txtVehicleQtyGdn.TabIndex = 25;
		this.txtVehicleQtyGdn.Tag = "double";
		this.txtVehicleQtyGdn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtVehicleQtyGdn.TextChanged += new System.EventHandler(txtVehicleQtyGdn_TextChanged);
		this.txtBiltyNo.BackColor = System.Drawing.Color.White;
		this.txtBiltyNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBiltyNo.Enabled = false;
		this.txtBiltyNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBiltyNo.Location = new System.Drawing.Point(397, 146);
		this.txtBiltyNo.Multiline = true;
		this.txtBiltyNo.Name = "txtBiltyNo";
		this.txtBiltyNo.Size = new System.Drawing.Size(140, 50);
		this.txtBiltyNo.TabIndex = 16;
		this.txtBiltyNo.Tag = "";
		this.txtOtherChargesDiff.BackColor = System.Drawing.SystemColors.Control;
		this.txtOtherChargesDiff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherChargesDiff.Enabled = false;
		this.txtOtherChargesDiff.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOtherChargesDiff.Location = new System.Drawing.Point(1207, 49);
		this.txtOtherChargesDiff.Name = "txtOtherChargesDiff";
		this.txtOtherChargesDiff.Size = new System.Drawing.Size(90, 23);
		this.txtOtherChargesDiff.TabIndex = 33;
		this.txtOtherChargesDiff.Tag = "double";
		this.txtOtherChargesDiff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOtherChargesGdn.BackColor = System.Drawing.Color.White;
		this.txtOtherChargesGdn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherChargesGdn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOtherChargesGdn.Location = new System.Drawing.Point(1115, 49);
		this.txtOtherChargesGdn.Name = "txtOtherChargesGdn";
		this.txtOtherChargesGdn.Size = new System.Drawing.Size(90, 23);
		this.txtOtherChargesGdn.TabIndex = 39;
		this.txtOtherChargesGdn.Tag = "double";
		this.txtOtherChargesGdn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOtherChargesGdn.TextChanged += new System.EventHandler(txtOtherCharges_TextChanged);
		this.txtOtherChargesGdn.Leave += new System.EventHandler(txtOtherCharges_Leave);
		this.txtBiltyFreightDiff.BackColor = System.Drawing.SystemColors.Control;
		this.txtBiltyFreightDiff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBiltyFreightDiff.Enabled = false;
		this.txtBiltyFreightDiff.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBiltyFreightDiff.Location = new System.Drawing.Point(1207, 23);
		this.txtBiltyFreightDiff.Name = "txtBiltyFreightDiff";
		this.txtBiltyFreightDiff.Size = new System.Drawing.Size(90, 23);
		this.txtBiltyFreightDiff.TabIndex = 37;
		this.txtBiltyFreightDiff.Tag = "double";
		this.txtBiltyFreightDiff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBiltyFreightGdn.BackColor = System.Drawing.Color.White;
		this.txtBiltyFreightGdn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBiltyFreightGdn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBiltyFreightGdn.Location = new System.Drawing.Point(1115, 23);
		this.txtBiltyFreightGdn.Name = "txtBiltyFreightGdn";
		this.txtBiltyFreightGdn.Size = new System.Drawing.Size(90, 23);
		this.txtBiltyFreightGdn.TabIndex = 38;
		this.txtBiltyFreightGdn.Tag = "double";
		this.txtBiltyFreightGdn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBiltyFreightGdn.TextChanged += new System.EventHandler(txtBiltyFreight_TextChanged);
		this.txtBiltyFreightGdn.Leave += new System.EventHandler(txtBiltyFreight_Leave);
		this.txtBiltyDate.CustomFormat = "dd-MMM-yy";
		this.txtBiltyDate.Enabled = false;
		this.txtBiltyDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtBiltyDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtBiltyDate.Location = new System.Drawing.Point(397, 120);
		this.txtBiltyDate.Name = "txtBiltyDate";
		this.txtBiltyDate.Size = new System.Drawing.Size(140, 22);
		this.txtBiltyDate.TabIndex = 15;
		this.datDocDate.CustomFormat = "dd-MMM-yy";
		this.datDocDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.datDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datDocDate.Location = new System.Drawing.Point(181, 12);
		this.datDocDate.Name = "datDocDate";
		this.datDocDate.Size = new System.Drawing.Size(130, 22);
		this.datDocDate.TabIndex = 1;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(2, 16);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(76, 15);
		this.label17.TabIndex = 7;
		this.label17.Text = "Doc No,Date";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(315, 124);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(58, 15);
		this.label13.TabIndex = 7;
		this.label13.Text = "Bilty Date";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(2, 36);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(73, 30);
		this.label7.TabIndex = 12;
		this.label7.Text = "Commission\r\nAgent";
		this.txtWarningRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWarningRemarks.Enabled = false;
		this.txtWarningRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWarningRemarks.ForeColor = System.Drawing.Color.Red;
		this.txtWarningRemarks.Location = new System.Drawing.Point(1024, 199);
		this.txtWarningRemarks.Name = "txtWarningRemarks";
		this.txtWarningRemarks.Size = new System.Drawing.Size(273, 23);
		this.txtWarningRemarks.TabIndex = 44;
		this.txtWarningRemarks.Visible = false;
		this.txtWarningRemarks.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDeliveryToAddress_KeyPress);
		this.txtDeliveryToAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDeliveryToAddress.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDeliveryToAddress.Location = new System.Drawing.Point(90, 199);
		this.txtDeliveryToAddress.Name = "txtDeliveryToAddress";
		this.txtDeliveryToAddress.Size = new System.Drawing.Size(221, 23);
		this.txtDeliveryToAddress.TabIndex = 10;
		this.txtDeliveryToAddress.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDeliveryToAddress_KeyPress);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(2, 72);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(72, 15);
		this.label3.TabIndex = 14;
		this.label3.Text = "Buyer Name";
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
		((AppearanceBase)appearance112).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance112).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance113).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance113).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance113).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommissionAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Location = new System.Drawing.Point(90, 38);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Name = "CmbCommissionAgent";
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Size = new System.Drawing.Size(221, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).TabIndex = 2;
		this.CmbBuyerName.AllowNull = (DefaultableBoolean)1;
		this.CmbBuyerName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBuyerName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBuyerName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBuyerName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance38;
		((SpecialBoxBase)((UltraGridBase)this.CmbBuyerName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance43).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance173).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance175).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBuyerName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBuyerName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBuyerName).Location = new System.Drawing.Point(90, 66);
		((System.Windows.Forms.Control)(object)this.CmbBuyerName).Name = "CmbBuyerName";
		((System.Windows.Forms.Control)(object)this.CmbBuyerName).Size = new System.Drawing.Size(221, 26);
		((System.Windows.Forms.Control)(object)this.CmbBuyerName).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbBuyerName).Leave += new System.EventHandler(CmbBuyerName_Leave);
		this.txtDocNo.BackColor = System.Drawing.Color.White;
		this.txtDocNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNo.Location = new System.Drawing.Point(90, 12);
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
		this.panel3.Size = new System.Drawing.Size(1300, 31);
		this.panel3.TabIndex = 2;
		this.label54.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.ForeColor = System.Drawing.Color.White;
		this.label54.Location = new System.Drawing.Point(942, 8);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(46, 15);
		this.label54.TabIndex = 32;
		this.label54.Text = "Branch";
		this.CmbBranch.AllowNull = (DefaultableBoolean)1;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.CmbBranch.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbBranch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance48;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbBranch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance54).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance55).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBranch.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Location = new System.Drawing.Point(995, 2);
		((System.Windows.Forms.Control)(object)this.CmbBranch).Name = "CmbBranch";
		((System.Windows.Forms.Control)(object)this.CmbBranch).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranch).TabIndex = 33;
		this.label26.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.White;
		this.label26.Location = new System.Drawing.Point(656, 8);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(58, 15);
		this.label26.TabIndex = 30;
		this.label26.Text = "Company";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 6);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(203, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "Goods Dispatching Note";
		this.chkPrint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPrint.AutoSize = true;
		this.chkPrint.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPrint.ForeColor = System.Drawing.Color.White;
		this.chkPrint.Location = new System.Drawing.Point(1217, 6);
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
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCompanyName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance59;
		((SpecialBoxBase)((UltraGridBase)this.CmbCompanyName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance65).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCompanyName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCompanyName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCompanyName).Location = new System.Drawing.Point(718, 2);
		((System.Windows.Forms.Control)(object)this.CmbCompanyName).Name = "CmbCompanyName";
		((System.Windows.Forms.Control)(object)this.CmbCompanyName).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbCompanyName).TabIndex = 31;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.btnnew, this.BtnRefresh, this.btnsave, this.btnUpdate, this.btnDelete, this.btnAttachments, this.btnPrint, this.BtnShortCutkeys, this.toolStripSeparator2, this.btnLoadGrn });
		this.toolStrip1.Location = new System.Drawing.Point(3, 3);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1300, 27);
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
		this.btnPrint.Text = "1055-&Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = (System.Drawing.Image)resources.GetObject("BtnShortCutkeys.Image");
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnLoadGrn.Image = Architecture.WinApp.Properties.Resources.download;
		this.btnLoadGrn.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadGrn.Name = "btnLoadGrn";
		this.btnLoadGrn.Size = new System.Drawing.Size(89, 24);
		this.btnLoadGrn.Text = "Load Grn";
		this.btnLoadGrn.Click += new System.EventHandler(btnLoadGrn_Click);
		this.tabHistory.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.tabHistory.Controls.Add(this.tabControl3);
		this.tabHistory.Controls.Add(this.panel15);
		this.tabHistory.Controls.Add(this.panel13);
		this.tabHistory.Controls.Add(this.panel4);
		this.tabHistory.Controls.Add(this.toolStrip2);
		this.tabHistory.Location = new System.Drawing.Point(4, 4);
		this.tabHistory.Name = "tabHistory";
		this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
		this.tabHistory.Size = new System.Drawing.Size(1306, 582);
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
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(3, 153);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1300, 190);
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
		this.tabControl3.Location = new System.Drawing.Point(3, 343);
		this.tabControl3.Name = "tabControl3";
		this.tabControl3.SelectedIndex = 0;
		this.tabControl3.Size = new System.Drawing.Size(1300, 236);
		this.tabControl3.TabIndex = 59;
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryDetail);
		this.tabPage1.Controls.Add(this.panel11);
		this.tabPage1.Location = new System.Drawing.Point(4, 25);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1292, 207);
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
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(1286, 177);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabIndex = 0;
		this.grdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label50);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 3);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1286, 24);
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
		this.tabPage2.Size = new System.Drawing.Size(1292, 207);
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
		((System.Windows.Forms.Control)(object)this.grdExpenseHistory).Size = new System.Drawing.Size(1286, 177);
		((System.Windows.Forms.Control)(object)this.grdExpenseHistory).TabIndex = 59;
		this.grdExpenseHistory.TotalRow = (InheritableBoolean)1;
		this.grdExpenseHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdExpenseHistory).VisualStyle = (VisualStyle)4;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label57);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 3);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1286, 24);
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
		this.tabPage3.Size = new System.Drawing.Size(1292, 207);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "Empty Bags Weight Deduction From Bill Weight";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel23.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmptyBagHistory);
		this.panel23.Controls.Add(this.panel16);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel23.Location = new System.Drawing.Point(0, 0);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(1292, 207);
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
		((System.Windows.Forms.Control)(object)this.grdEmptyBagHistory).Size = new System.Drawing.Size(1292, 183);
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
		this.panel16.Size = new System.Drawing.Size(1292, 24);
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
		this.panel15.Location = new System.Drawing.Point(3, 130);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1300, 23);
		this.panel15.TabIndex = 54;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1258, 1);
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
		this.panel13.Size = new System.Drawing.Size(1300, 71);
		this.panel13.TabIndex = 6;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBuyerNameHistory);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommissionAgentHistory);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliverToAddressHistory);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryToPartyHistory);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.label38);
		this.groupBox1.Controls.Add(this.datValidityToHistory);
		this.groupBox1.Controls.Add(this.datValidityFromHistory);
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
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1295, 68);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.CmbBuyerNameHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbBuyerNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBuyerNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBuyerNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance187).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance187).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance187).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance187;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance127;
		((SpecialBoxBase)((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance190).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance190).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance191).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance191).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance191).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance191;
		((AppearanceBase)appearance192).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBuyerNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameHistory).Location = new System.Drawing.Point(703, 38);
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameHistory).Name = "CmbBuyerNameHistory";
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameHistory).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameHistory).TabIndex = 7;
		this.CmbCommissionAgentHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCommissionAgentHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommissionAgentHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommissionAgentHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance135;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance82).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance83).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommissionAgentHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentHistory).Location = new System.Drawing.Point(703, 11);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentHistory).Name = "CmbCommissionAgentHistory";
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentHistory).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentHistory).TabIndex = 6;
		this.CmbDeliverToAddressHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliverToAddressHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliverToAddressHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliverToAddressHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance99).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance99).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance101;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance106).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliverToAddressHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliverToAddressHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliverToAddressHistory).Location = new System.Drawing.Point(1008, 38);
		((System.Windows.Forms.Control)(object)this.CmbDeliverToAddressHistory).Name = "CmbDeliverToAddressHistory";
		((System.Windows.Forms.Control)(object)this.CmbDeliverToAddressHistory).Size = new System.Drawing.Size(144, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliverToAddressHistory).TabIndex = 9;
		this.CmbDeliveryToPartyHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryToPartyHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryToPartyHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryToPartyHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance51;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance165).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance166).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance168;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance169).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToPartyHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryToPartyHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToPartyHistory).Location = new System.Drawing.Point(1008, 11);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToPartyHistory).Name = "CmbDeliveryToPartyHistory";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToPartyHistory).Size = new System.Drawing.Size(197, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToPartyHistory).TabIndex = 8;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(905, 44);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(105, 15);
		this.label35.TabIndex = 527;
		this.label35.Text = "Deliver To Address\r\n";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(905, 17);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(90, 15);
		this.label38.TabIndex = 526;
		this.label38.Text = "Deliver To Party";
		this.datValidityToHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datValidityToHistory.Checked = false;
		this.datValidityToHistory.CustomFormat = "dd-MMM-yy";
		this.datValidityToHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.datValidityToHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datValidityToHistory.Location = new System.Drawing.Point(226, 40);
		this.datValidityToHistory.Name = "datValidityToHistory";
		this.datValidityToHistory.ShowCheckBox = true;
		this.datValidityToHistory.Size = new System.Drawing.Size(130, 22);
		this.datValidityToHistory.TabIndex = 3;
		this.datValidityFromHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datValidityFromHistory.Checked = false;
		this.datValidityFromHistory.CustomFormat = "dd-MMM-yy";
		this.datValidityFromHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.datValidityFromHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datValidityFromHistory.Location = new System.Drawing.Point(95, 40);
		this.datValidityFromHistory.Name = "datValidityFromHistory";
		this.datValidityFromHistory.ShowCheckBox = true;
		this.datValidityFromHistory.Size = new System.Drawing.Size(130, 22);
		this.datValidityFromHistory.TabIndex = 2;
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(226, 13);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(130, 22);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(95, 13);
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
		this.btnshow.Location = new System.Drawing.Point(1154, 39);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(51, 25);
		this.btnshow.TabIndex = 10;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.CmbItemNameHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance193).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance193;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance194).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance195).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance195;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance197).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance198;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance199).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance123).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance123).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance123).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance201).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance202).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Location = new System.Drawing.Point(429, 38);
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Name = "CmbItemNameHistory";
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).TabIndex = 5;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(360, 44);
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
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance174).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance174).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance185).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance185;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance196).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance196).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance196).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance207).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance207;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance218).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance218;
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
		((AppearanceBase)appearance164).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentItemHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbParentItemHistory).Location = new System.Drawing.Point(429, 11);
		((System.Windows.Forms.Control)(object)this.CmbParentItemHistory).Name = "CmbParentItemHistory";
		((System.Windows.Forms.Control)(object)this.CmbParentItemHistory).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentItemHistory).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbParentItemHistory).Leave += new System.EventHandler(CmbParentItemHistory_Leave);
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(1209, 46);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 13;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(1209, 28);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 12;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(360, 17);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(69, 15);
		this.label36.TabIndex = 187;
		this.label36.Text = "Parent Item";
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(1209, 11);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 11;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(3, 44);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(93, 15);
		this.label32.TabIndex = 185;
		this.label32.Text = "Validity From,To";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(632, 44);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(37, 15);
		this.label29.TabIndex = 182;
		this.label29.Text = "Buyer";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(632, 9);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(73, 30);
		this.label31.TabIndex = 180;
		this.label31.Text = "Commission\r\nAgent";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(3, 17);
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
		this.panel4.Size = new System.Drawing.Size(1300, 29);
		this.panel4.TabIndex = 5;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.White;
		this.label30.Location = new System.Drawing.Point(4, 4);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(266, 19);
		this.label30.TabIndex = 0;
		this.label30.Text = "Goods Dispatching Note History";
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.toolStripSeparator3, this.BtnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(3, 3);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1300, 27);
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
		this.txtAvgFillingPerBagGdn.BackColor = System.Drawing.SystemColors.Control;
		this.txtAvgFillingPerBagGdn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAvgFillingPerBagGdn.Enabled = false;
		this.txtAvgFillingPerBagGdn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAvgFillingPerBagGdn.Location = new System.Drawing.Point(734, 149);
		this.txtAvgFillingPerBagGdn.Name = "txtAvgFillingPerBagGdn";
		this.txtAvgFillingPerBagGdn.Size = new System.Drawing.Size(112, 23);
		this.txtAvgFillingPerBagGdn.TabIndex = 28;
		this.txtAvgFillingPerBagGdn.TabStop = false;
		this.txtAvgFillingPerBagGdn.Tag = "double";
		this.txtAvgFillingPerBagGdn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAvgFillingPerBagGdn.TextChanged += new System.EventHandler(txtAvgFillingPerBag_TextChanged);
		this.txtAvgFillingPerBagDiff.BackColor = System.Drawing.SystemColors.Control;
		this.txtAvgFillingPerBagDiff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAvgFillingPerBagDiff.Enabled = false;
		this.txtAvgFillingPerBagDiff.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAvgFillingPerBagDiff.Location = new System.Drawing.Point(848, 149);
		this.txtAvgFillingPerBagDiff.Name = "txtAvgFillingPerBagDiff";
		this.txtAvgFillingPerBagDiff.Size = new System.Drawing.Size(90, 23);
		this.txtAvgFillingPerBagDiff.TabIndex = 33;
		this.txtAvgFillingPerBagDiff.TabStop = false;
		this.txtAvgFillingPerBagDiff.Tag = "double";
		this.txtAvgFillingPerBagDiff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(541, 145);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(66, 30);
		this.label39.TabIndex = 671;
		this.label39.Text = "Avg Filling \r\nPer Bag";
		this.txtAvgFillingPerBagGrn.BackColor = System.Drawing.SystemColors.Control;
		this.txtAvgFillingPerBagGrn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAvgFillingPerBagGrn.Enabled = false;
		this.txtAvgFillingPerBagGrn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAvgFillingPerBagGrn.Location = new System.Drawing.Point(620, 149);
		this.txtAvgFillingPerBagGrn.Name = "txtAvgFillingPerBagGrn";
		this.txtAvgFillingPerBagGrn.Size = new System.Drawing.Size(112, 23);
		this.txtAvgFillingPerBagGrn.TabIndex = 22;
		this.txtAvgFillingPerBagGrn.TabStop = false;
		this.txtAvgFillingPerBagGrn.Tag = "double";
		this.txtAvgFillingPerBagGrn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAvgFillingPerBagGrn.TextChanged += new System.EventHandler(txtAvgFillingPerBag_TextChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1314, 611);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "frmGoodsDispatchingNoteCmagt";
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
		this.panel1.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryToParty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransporter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTermGrn).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTermGdn).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbVehicleType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUnloadingCity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGrnNoDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLoadingCity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbShipToAddress).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionAgent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBuyerName).EndInit();
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
		((System.ComponentModel.ISupportInitialize)this.CmbBuyerNameHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionAgentHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliverToAddressHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryToPartyHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentItemHistory).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
