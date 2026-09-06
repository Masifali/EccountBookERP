using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.StoreManagement;

public class GrnStore : Form
{
	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtdetail = new DataTable();

	private DataTable dtRegisterFromDb = new DataTable();

	private DataTable dtRegisterGrid = new DataTable();

	private DataTable dtHistoryGrid = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtTransporter = new DataTable();

	private DataTable dtwh = new DataTable();

	private DataTable dtCondition = new DataTable();

	public bool OnlyPending;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private string ScreenName = "";

	private bool UpdateMode;

	private int DocumentTypeId = 48;

	private int Id;

	private int BaseDocumentTypeId;

	private int GPID;

	private int WareHouseConfigId;

	private bool SubsidiaryAccountAllownOnVouchers;

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool FormInitialized;

	public DateTime FromDate;

	public DateTime ToDate;

	private IContainer components;

	private Panel panel1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton BtnPrint;

	private ToolStripButton btnAttachment;

	private Panel panel3;

	private Label label23;

	private Label label24;

	private Label label25;

	private TextBox txtRemarksHeader;

	private Label label21;

	private Label label19;

	private TextBox txtBiltyNo;

	private Label label7;

	private TextBox txtVehicleNo;

	internal UltraCombo CmbTransporter;

	private Label label48;

	private Label label26;

	private TextBox txtFreightAmount;

	internal UltraCombo CmbBillToParty;

	private Label label2;

	private Label label10;

	private TextBox txtReferenceNo;

	private DateTimePicker DocDate;

	private TextBox txtdocno;

	private Label label5;

	private Panel panel7;

	private Panel panel8;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton BtnRefresh;

	private TextBox CmbGpNo;

	private ToolStripButton btnLoadPurchaseOrder;

	private Timer timer1;

	internal GridEX grd;

	private Panel panel6;

	internal GridEX GrdHistory;

	internal UltraCombo CmbVehicleType;

	private Panel panel12;

	private Panel panel13;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label1;

	private ToolStrip toolStrip2;

	private CheckBox ChkPrint;

	private Panel panel16;

	internal GridEX GrdHistoryDetail;

	private Panel panel15;

	private TabPage tabRegister;

	private Panel panel17;

	private Panel panel19;

	private Panel panel18;

	private Button BtnShowRegister;

	private TextBox txtDocNoToRegister;

	private TextBox txtDocNoFromRegister;

	private Label label6;

	internal UltraCombo CmbWarehouseRegister;

	internal UltraCombo CmbItemNameRegister;

	private Label label14;

	internal UltraCombo CmbTransporterRegister;

	private Label label12;

	internal UltraCombo CmbPartyNameRegister;

	private Label label11;

	private Label label3;

	private Label label4;

	private CtrlGrdBar ctrlGrdBar3;

	internal GridEX GrdiRegister;

	private ToolStrip toolStrip3;

	private ToolStripButton BtnRegisterPrint;

	private ToolStripButton BtnRefreshRegister;

	private ToolStripSeparator toolStripSeparator2;

	private GroupBox groupBox3;

	private Button btnshow;

	private Label label49;

	private Label label50;

	private Label label51;

	private Label label52;

	private Label label53;

	internal UltraCombo cmbSupplierNameHistory;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Panel panel9;

	private Panel panel21;

	private Panel panel11;

	private Label label32;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private ToolStripButton btnDelete;

	private ToolStripButton BtnResetRegister;

	private Label label8;

	private ToolStripButton BtnLoadPurchaseDemand;

	private ToolStripButton BtnLoadDeliveryChallan;

	private ToolStripButton BtnShortCutkeys;

	public TabControl tabControl1;

	public DateTimePicker txtDateToRegister;

	public DateTimePicker txtDatefromRegister;

	public GrnStore(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
		InitializeComponentMethod();
	}

	private void InitializeComponentCustom()
	{
		try
		{
			ScreenName = Conversion.ToString((object)base.Name);
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("CompanyName", typeof(string));
			dtSupplier.Columns.Add("PartyCode", typeof(string));
			dtSupplier.Columns.Add("GlAccountId", typeof(int));
			dtSupplier.Columns.Add("CityId", typeof(int));
			dtSupplier.Columns.Add("CityName", typeof(string));
			dtSupplier.Columns.Add("MobileNo", typeof(string));
			dtTransporter.Columns.Add("Id", typeof(int));
			dtTransporter.Columns.Add("AccountTitle", typeof(string));
			dtTransporter.Columns.Add("AccountCode", typeof(string));
			dtTransporter.Columns.Add("SupplierCustomerId", typeof(int));
			dtwh.Columns.Add("Id", typeof(int));
			dtwh.Columns.Add("WareHouseName", typeof(string));
			dtdetail.Columns.Add("Id", typeof(int));
			dtdetail.Columns.Add("OrderId", typeof(int));
			dtdetail.Columns.Add("OrderDetailId", typeof(int));
			dtdetail.Columns.Add("OrderNo", typeof(int));
			dtdetail.Columns.Add("DemandId", typeof(int));
			dtdetail.Columns.Add("DemandDetailId", typeof(int));
			dtdetail.Columns.Add("DemandNo", typeof(int));
			dtdetail.Columns.Add("PurchasePreBillId", typeof(int));
			dtdetail.Columns.Add("PurchasePreBillDetailId", typeof(int));
			dtdetail.Columns.Add("PurchasePreBillNo", typeof(int));
			dtdetail.Columns.Add("DeliveryChallanId", typeof(int));
			dtdetail.Columns.Add("DeliveryChallanDetailId", typeof(int));
			dtdetail.Columns.Add("DeliveryChallanNo", typeof(int));
			dtdetail.Columns.Add("ItemId", typeof(int));
			dtdetail.Columns.Add("ItemCode");
			dtdetail.Columns.Add("ItemName");
			dtdetail.Columns.Add("WarehouseId", typeof(int));
			dtdetail.Columns.Add("WarehouseName", typeof(string));
			dtdetail.Columns.Add("RackId", typeof(int));
			dtdetail.Columns.Add("RackName", typeof(string));
			dtdetail.Columns.Add("ItemConditionId", typeof(int));
			dtdetail.Columns.Add("UOMId", typeof(int));
			dtdetail.Columns.Add("UOM");
			dtdetail.Columns.Add("UomEquivalent", typeof(double));
			dtdetail.Columns.Add("TotalOrderQty", typeof(double));
			dtdetail.Columns.Add("UsedOrderQty", typeof(double));
			dtdetail.Columns.Add("BalOrderQty", typeof(double));
			dtdetail.Columns.Add("TotalDemandQty", typeof(double));
			dtdetail.Columns.Add("UsedDemandQty", typeof(double));
			dtdetail.Columns.Add("BalDemandQty", typeof(double));
			dtdetail.Columns.Add("PurchasePreBillQty", typeof(double));
			dtdetail.Columns.Add("TotalDeliveryChallanQty", typeof(double));
			dtdetail.Columns.Add("UsedDeliveryChallanQty", typeof(double));
			dtdetail.Columns.Add("BalDeliveryChallanQty", typeof(double));
			dtdetail.Columns.Add("ThisQty", typeof(double));
			dtdetail.Columns.Add("AvailableStockQty", typeof(double));
			dtdetail.Columns.Add("Remarks");
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
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			string documentCode = GetDocumentCode();
			DataTable all = VehicleType.GetAll();
			DataTable dataForDropDownFromGrn = InvGrn.GetDataForDropDownFromGrn(UserAccount.OrganizationId, UserAccount.CompanyId, DocumentTypeId.ToString(), (string)null, Conversion.ToString((object)UserAccount.BranchesId), 0);
			return new
			{
				documentNoData = documentCode,
				VehicleTypeData = all,
				HistoryComboData = dataForDropDownFromGrn
			};
		}).ContinueWith(t =>
		{
			if (t.IsFaulted)
			{
				MessageBox.Show("Error occurred during database call.");
			}
			else
			{
				var result = t.Result;
				btnSave.Enabled = formright.DoHaveSaveRight;
				BtnPrint.Enabled = formright.DoHavePrintRights;
				btnUpdate.Enabled = formright.DoHaveUpdateRights;
				btnDelete.Enabled = formright.DoHaveCanDelete;
				btnDelete.Visible = false;
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar1, formright);
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar2, formright);
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar3, formright);
				GetConfigurationsFromGlobal();
				if (Id == 0)
				{
					txtdocno.Text = result.documentNoData;
				}
				SupplierDtFillFromGlobal();
				BindSupplierName();
				VehicleTypesBind(result.VehicleTypeData);
				TransporterDtFillFromGlobal();
				TransporterAcFill();
				GetConfigurationsFromGlobalAndBindValuesInColumns();
				BindGrids(BaseDocumentTypeId);
				HistoryComboBind(result.HistoryComboData);
				FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				ToDateHistory.Value = DateTime.Now;
				if (!Conversion.CheckDateTimeNull(FromDate) && !Conversion.CheckDateTimeNull(ToDate))
				{
					txtDatefromRegister.Value = clsGlobalVariables.ActiveYr.Start_Period;
					txtDateToRegister.Value = ToDate;
					BtnShowRegister_Click(null, null);
				}
				else
				{
					txtDatefromRegister.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
					txtDateToRegister.Value = DateTime.Now;
				}
				FormInitialized = true;
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void InvFrmGRN_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1000, 700);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryComboBind(DataTable dt)
	{
		try
		{
			DataTable dtSupplier = new DataTable();
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("Name", typeof(string));
			DataTable dtTransporter = dtSupplier.Clone();
			DataTable dtWare = dtSupplier.Clone();
			DataTable dtItem = dtSupplier.Clone();
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					switch (Conversion.ToString(dt.Rows[i]["Activity"]))
					{
					case "Supplier":
						dtSupplier.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
						break;
					case "Transporter":
						dtTransporter.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
						break;
					case "Warehouse":
						dtWare.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
						break;
					case "Item":
						dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
						break;
					}
				}
			}
			InfragisticsHelper.BindAndRetainSelection(cmbSupplierNameHistory, dtSupplier, "Id", "Name", "Supplier Name", AllColumns: true, null, insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbPartyNameRegister, dtSupplier, "Id", "Name", "Supplier Name", AllColumns: true, null, insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbTransporterRegister, dtTransporter, "Id", "Name", "Transporter Name", AllColumns: true, null, insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbItemNameRegister, dtItem, "Id", "Name", "Item Name", AllColumns: true, Conversion.ToInt(CmbItemNameRegister.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbWarehouseRegister, dtWare, "Id", "Name", "Warehouse", AllColumns: true, Conversion.ToInt(CmbWarehouseRegister.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetConfigurationsFromGlobalAndBindValuesInColumns()
	{
		try
		{
			if (!SubsidiaryAccountAllownOnVouchers)
			{
				int transporterId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("FreightInwardAc"));
				if (transporterId != 0 && dtTransporter.AsEnumerable().Any((DataRow r) => Conversion.ToInt(r["Id"]) == transporterId))
				{
					CmbTransporter.Value = transporterId;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Configuration Error" + ex.Message);
		}
	}

	private void GetConfigurationsFromGlobal()
	{
		try
		{
			WareHouseConfigId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultWarehouseForStoreFlow"));
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private string GetDocumentCode()
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
			return Conversion.ToInt((object)InvGrn.GenerateInvGrnCode(new InvGrn
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = DocumentTypeId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			})).ToString();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void SupplierDtFillFromGlobal()
	{
		try
		{
			dtSupplier.Clear();
			List<getGlobalSupplierCustomer> obj = clsGlobalVariables.globalSupplierCustomer;
			if (obj != null && obj.Count > 0)
			{
				for (int i = 0; i < obj.Count; i++)
				{
					dtSupplier.Rows.Add(Conversion.ToInt((object)obj[i].Id), Conversion.ToString((object)obj[i].CompanyName), Conversion.ToString((object)obj[i].PartyCode), Conversion.ToInt((object)obj[i].GlAccountId), Conversion.ToInt((object)obj[i].CityId), Conversion.ToString((object)obj[i].CityName), Conversion.ToString((object)obj[i].MobilePersonal));
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindSupplierName()
	{
		try
		{
			int SupId = Conversion.ToInt(CmbBillToParty.Value);
			if (dtSupplier.Rows.Count > 0)
			{
				InfragisticsHelper.BindAndRetainSelection(CmbBillToParty, dtSupplier, "Id", "CompanyName", "SupplierName", AllColumns: true, SupId, insertDefaultRow: false, null, null, ActivateRow: false);
				((UltraGridBase)CmbBillToParty).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbBillToParty).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			}
			else
			{
				((Control)(object)CmbBillToParty).Text = string.Empty;
				((UltraGridBase)CmbBillToParty).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void TransporterDtFillFromGlobal()
	{
		try
		{
			List<getGlobalVendorsAndCustomersForTransporter> lst = clsGlobalVariables.getGlobalVendorsAndCustomersForTransporters;
			dtTransporter.Clear();
			if (SubsidiaryAccountAllownOnVouchers)
			{
				if (lst != null && lst.Count > 0)
				{
					for (int i = 0; i < lst.Count; i++)
					{
						dtTransporter.Rows.Add(lst[i].GlAccountId, lst[i].CompanyName, lst[i].PartyCode, lst[i].Id);
					}
				}
				return;
			}
			int[] excludedIds = new int[11]
			{
				2, 4, 10, 11, 12, 13, 14, 15, 20, 21,
				22
			};
			IEnumerable<getGlobalAllAccountsWithCustomGroup> enumerable = clsGlobalVariables.AllAccountsWithCustomGroupId.Where((getGlobalAllAccountsWithCustomGroup r) => !excludedIds.Contains(r.AccountTypeId));
			HashSet<int> existingIds = new HashSet<int>();
			foreach (getGlobalAllAccountsWithCustomGroup acc in enumerable)
			{
				int id = Conversion.ToInt((object)acc.ChartOfAccountId);
				if (!existingIds.Contains(id))
				{
					dtTransporter.Rows.Add(acc.ChartOfAccountId, acc.AccountTitle, acc.AccountCode, 0);
				}
				existingIds.Add(id);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void TransporterAcFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbTransporter.Value);
			if (dtTransporter.Rows.Count > 0)
			{
				InfragisticsHelper.BindAndRetainSelection(CmbTransporter, dtTransporter, "Id", "AccountTitle", "Transporter Name", AllColumns: true, Id, insertDefaultRow: true);
				if (dtTransporter.Rows.Count > 0 && ((UltraGridBase)CmbTransporter).DataSource != null)
				{
					((UltraGridBase)CmbTransporter).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void VehicleTypesBind(DataTable dtVehicle)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbVehicleType, dtVehicle, "Id", "VehicleDescription", "VehicleDescription", AllColumns: true, Conversion.ToInt(CmbVehicleType.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combgatepass_Leave(object sender, EventArgs e)
	{
	}

	private void GridDtFill()
	{
		try
		{
			List<GlobalItemCondition> lst = clsGlobalVariables.globalItemConditions.Where((GlobalItemCondition r) => r.Id != 4).ToList();
			dtCondition = DatatableHelper.PopulateDataTableAndReturn(lst, (GlobalItemCondition x) => x.Id, (GlobalItemCondition x) => x.ConditionStatus);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGrids(int BaseDocumentTypeId)
	{
		try
		{
			DataTable dataTable = dtdetail;
			if (dataTable != null && dataTable.Columns.Count > 0)
			{
				grd.DataSource = dtdetail;
				grd.RetrieveStructure();
				grdSettings(grd, BaseDocumentTypeId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridComboBind()
	{
		try
		{
			GridDtFill();
			GridEX_Helper.ComboBind(grd.RootTable.Columns["ItemConditionId"], dtCondition, "Id", "Description", "Item Condition", 90, limitToList: true, editType: true, grd.RootTable.Columns["UomEquivalent"].Position + 1);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void grdSettings(GridEX gridEX, int BaseDocumentTypeId)
	{
		try
		{
			List<string> hiddenColumns = new List<string>
			{
				"Id", "ItemId", "UOMId", "OrderId", "OrderDetailId", "DemandId", "DemandDetailId", "DeliveryChallanId", "DeliveryChallanDetailId", "UomEquivalent",
				"PurchasePreBillId", "PurchasePreBillDetailId", "WarehouseId", "RackId"
			};
			switch (BaseDocumentTypeId)
			{
			case 0:
				hiddenColumns.AddRange(new string[14]
				{
					"OrderNo", "DemandNo", "PurchasePreBillNo", "DeliveryChallanNo", "TotalOrderQty", "UsedOrderQty", "BalOrderQty", "TotalDemandQty", "UsedDemandQty", "BalDemandQty",
					"PurchasePreBillQty", "TotalDeliveryChallanQty", "UsedDeliveryChallanQty", "BalDeliveryChallanQty"
				});
				break;
			case 1:
				hiddenColumns.AddRange(new string[10] { "DemandNo", "DeliveryChallanNo", "TotalDemandQty", "UsedDemandQty", "BalDemandQty", "TotalDeliveryChallanQty", "UsedDeliveryChallanQty", "BalDeliveryChallanQty", "PurchasePreBillNo", "PurchasePreBillQty" });
				break;
			case 2:
				hiddenColumns.AddRange(new string[10] { "OrderNo", "DeliveryChallanNo", "TotalOrderQty", "UsedOrderQty", "BalOrderQty", "TotalDeliveryChallanQty", "UsedDeliveryChallanQty", "BalDeliveryChallanQty", "PurchasePreBillNo", "PurchasePreBillQty" });
				break;
			case 3:
				hiddenColumns.AddRange(new string[6] { "OrderNo", "TotalOrderQty", "UsedOrderQty", "BalOrderQty", "UsedDemandQty", "BalDemandQty" });
				break;
			}
			Dictionary<string, EditType> editableColumns = null;
			if (((Control)(object)gridEX).Name == "grd")
			{
				editableColumns = new Dictionary<string, EditType>
				{
					{
						"ThisQty",
						(EditType)1
					},
					{
						"Remarks",
						(EditType)1
					}
				};
			}
			GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 2, 2, hiddenColumns, editableColumns);
			gridEX.RootTable.Columns["PurchasePreBillNo"].Width = 80;
			gridEX.RootTable.Columns["DeliveryChallanNo"].Width = 80;
			gridEX.RootTable.Columns["UsedDeliveryChallanQty"].Width = 130;
			gridEX.RootTable.Columns["TotalDemandQty"].Caption = "Approved Demand Qty";
			gridEX.RootTable.Columns["ThisQty"].Caption = "Received Qty";
			if (((Control)(object)gridEX).Name == "grd")
			{
				gridEX.ColumnAutoResize = false;
				GridComboBind();
				GridEX_Helper.AddButton(grd, "Delete", "X", 20, 0);
				ctrlGrdBar1_Load(null, null);
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
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Column.Key == "Delete")
			{
				item.Delete();
				grd.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space && grd.CurrentColumn.Key == "Delete")
			{
				r.Delete();
				grd.UpdateData();
			}
			if (e.KeyCode == Keys.Delete)
			{
				r.Delete();
				grd.UpdateData();
			}
			if (e.KeyCode != Keys.F1)
			{
				return;
			}
			int WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
			int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
			int RackId = Conversion.ToInt(r.Cells["RackId"].Value);
			if (grd.CurrentColumn.Key == "WarehouseId" || grd.CurrentColumn.Key == "WarehouseName")
			{
				List<RacksWithWarehouseAndItem> list = (from x in clsGlobalVariables.racksWithWarehouseAndItems
					where x.ItemId == ItemId
					group x by x.WarehouseId into g
					select g.First()).ToList();
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("WareHouseName", typeof(string));
				dt.BeginLoadData();
				foreach (RacksWithWarehouseAndItem w in list)
				{
					dt.Rows.Add(w.WarehouseId, w.WareHouseName);
				}
				dt.EndLoadData();
				using (GrdPopUp obj = new GrdPopUp(dt, "Id", "WareHouseName"))
				{
					obj.StartPosition = FormStartPosition.CenterScreen;
					obj.ShowDialog();
					int selectedId2 = Conversion.ToInt((object)obj.ReturnId);
					string selectedName = Conversion.ToString((object)obj.ReturnName);
					r.Cells["WarehouseId"].Value = selectedId2;
					r.Cells["WarehouseName"].Value = selectedName;
					List<RacksWithWarehouseAndItem> racks = clsGlobalVariables.racksWithWarehouseAndItems.Where((RacksWithWarehouseAndItem x) => x.ItemId == ItemId && x.WarehouseId == selectedId2).ToList();
					if (RackId > 0)
					{
						if (!racks.Any((RacksWithWarehouseAndItem x) => x.Id == RackId))
						{
							if (racks.Count > 1)
							{
								r.Cells["RackId"].Value = 0;
								r.Cells["RackName"].Value = "";
							}
							else if (racks.Count == 1)
							{
								RacksWithWarehouseAndItem singleRack = racks.First();
								r.Cells["RackId"].Value = singleRack.Id;
								r.Cells["RackName"].Value = singleRack.RackName;
							}
							else
							{
								r.Cells["RackId"].Value = 0;
								r.Cells["RackName"].Value = "";
							}
						}
					}
					else if (racks.Count == 1)
					{
						RacksWithWarehouseAndItem singleRack2 = racks.First();
						r.Cells["RackId"].Value = singleRack2.Id;
						r.Cells["RackName"].Value = singleRack2.RackName;
					}
				}
				grd.UpdateData();
			}
			else if (grd.CurrentColumn.Key == "RackId" || grd.CurrentColumn.Key == "RackName")
			{
				List<RacksWithWarehouseAndItem> filteredRacks = (from rr in clsGlobalVariables.racksWithWarehouseAndItems
					where rr.ItemId == ItemId && (WarehouseId == 0 || rr.WarehouseId == WarehouseId)
					select rr into row
					group row by row.Id into g
					select g.First()).ToList();
				if (filteredRacks.Count == 0)
				{
					return;
				}
				DataTable dt2 = new DataTable();
				dt2.Columns.Add("Id", typeof(int));
				dt2.Columns.Add("RackName", typeof(string));
				dt2.Columns.Add("WarehouseId", typeof(int));
				dt2.Columns.Add("WarehouseName", typeof(string));
				dt2.BeginLoadData();
				foreach (RacksWithWarehouseAndItem item in filteredRacks)
				{
					dt2.Rows.Add(item.Id, item.RackName, item.WarehouseId, item.WareHouseName);
				}
				dt2.EndLoadData();
				using (GrdPopUp grdPopUp = new GrdPopUp(dt2, "Id", "RackName", "WarehouseName"))
				{
					grdPopUp.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp.ShowDialog();
					int selectedId = Conversion.ToInt((object)grdPopUp.ReturnId);
					string selectedName2 = Conversion.ToString((object)grdPopUp.ReturnName);
					r.Cells["RackId"].Value = selectedId;
					r.Cells["RackName"].Value = selectedName2;
					if (WarehouseId == 0)
					{
						RacksWithWarehouseAndItem selected = filteredRacks.FirstOrDefault((RacksWithWarehouseAndItem x) => x.Id == selectedId);
						if (selected != null)
						{
							r.Cells["WarehouseId"].Value = selected.WarehouseId;
							r.Cells["WarehouseName"].Value = selected.WareHouseName;
						}
					}
				}
				grd.UpdateData();
			}
			r.BeginEdit();
			RowBalanceStockQtyUpdate(r);
			r.EndEdit();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grd.CurrentColumn != null)
			{
				RowBalanceStockQtyUpdate(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void RowBalanceStockQtyUpdate(GridEXRow r)
	{
		int itemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		if (itemId == 0)
		{
			r.Cells["AvailableStockQty"].Value = 0;
			return;
		}
		int itemConditionId = Conversion.ToInt(r.Cells["ItemConditionId"].Value);
		int warehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
		int rackId = Conversion.ToInt(r.Cells["RackId"].Value);
		double itemQty = 0.0;
		DataTable dtStock = CommonServices.GetAvgRateQtyAndStockInHand(itemId, DocDate.Value, itemConditionId, Id, (Id > 0) ? DocumentTypeId : 0, warehouseId, rackId);
		if (dtStock != null && dtStock.Rows.Count > 0 && dtStock.Rows[0]["QtyInHand"] != DBNull.Value)
		{
			itemQty = Math.Round(Conversion.ToDouble(dtStock.Rows[0]["QtyInHand"]), 2);
		}
		r.Cells["AvailableStockQty"].Value = itemQty;
	}

	private void GridStockUpdate()
	{
		GridEXRow[] dataRows = grd.GetDataRows();
		foreach (GridEXRow r in dataRows)
		{
			r.BeginEdit();
			RowBalanceStockQtyUpdate(r);
			r.EndEdit();
		}
	}

	private void DocDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			GridStockUpdate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
	}

	private void btnSave_Click(object sender, EventArgs e)
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

	private void btnDelete_Click(object sender, EventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
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
						DocumentTypeId = DocumentTypeId,
						Id = Id,
						EntryUser = UserAccount.ID
					});
					MessageBox.Show("Delete Record Successfully");
					reset();
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

	private void Insert()
	{
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				throw new Exception("Detail Record Not Found");
			}
			if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
			{
				((Control)(object)CmbBillToParty, "Supplier", FormHelper.ValidType.INT),
				(CmbGpNo, "Gate Pass No", FormHelper.ValidType.String)
			}))
			{
				return;
			}
			if (BaseDocumentTypeId == 3 && GPID == 0)
			{
				MessageBox.Show("No Record found against gp no");
			}
			else
			{
				if (!FormHelper.ConfirmAction((Id == 0) ? "Are you sure to Save" : "Are you sure to Update"))
				{
					return;
				}
				InvGrn GRN = new InvGrn();
				GRN.OrganizationId = UserAccount.OrganizationId;
				GRN.CompanyId = UserAccount.CompanyId;
				GRN.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				GRN.BranchesId = UserAccount.BranchesId;
				GRN.ProjectsId = UserAccount.BranchesId;
				GRN.DocumentTypeId = DocumentTypeId;
				GRN.BaseDocumentTypeId = BaseDocumentTypeId;
				GRN.Id = Id;
				GRN.IsApproved = false;
				GRN.EntryDate = DateTime.Today;
				GRN.EntryUser = UserAccount.ID;
				GRN.ModifyDate = DateTime.Today;
				GRN.ModifyUser = UserAccount.ID;
				GRN.DocNo = Convert.ToInt32(txtdocno.Text.Trim());
				GRN.DocDate = DocDate.Value;
				GRN.SupplierCustomerId = Conversion.ToInt(CmbBillToParty.Value);
				GRN.ReferenceDocNo = txtReferenceNo.Text.Trim();
				GRN.InwardGatePassId = Conversion.ToInt((object)GPID);
				GRN.GpNo = Conversion.ToInt((object)CmbGpNo.Text);
				GRN.VehicleType = Conversion.ToString((object)((Control)(object)CmbVehicleType).Text);
				GRN.VehicleNo = Conversion.ToString((object)txtVehicleNo.Text.Trim());
				GRN.BiltyNo = txtBiltyNo.Text.Trim();
				GRN.TransporterDocRef = string.Empty;
				GRN.CarriageAmount = Conversion.ToDouble((object)txtFreightAmount.Text.Trim());
				if (!SubsidiaryAccountAllownOnVouchers)
				{
					GRN.TransporterId = Conversion.ToInt(CmbTransporter.Value);
				}
				else
				{
					GRN.TransporterSupCustId = Conversion.ToInt(((UltraDropDownBase)CmbTransporter).SelectedRow.Cells[3].Value);
					GRN.TransporterId = Conversion.ToInt(CmbTransporter.Value);
				}
				if (GRN.CarriageAmount > 0.0 && (((UltraGridBase)CmbTransporter).ActiveRow == null || Conversion.ToInt(CmbTransporter.Value) == 0))
				{
					MessageBox.Show("Transporter Account field required");
					CmbTransporter.Focus();
					return;
				}
				GRN.OtherCharges = 0.0;
				GRN.RemarksHeader = txtRemarksHeader.Text.Trim();
				GRN.ScreenName = Conversion.ToString((object)base.Name);
				GRN.invGrnDetail = new List<InvGrnDetail>();
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					InvGrnDetail vd = new InvGrnDetail();
					vd.Id = ((Id != 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
					FillDetailListCommonForInsertAndDelete(vd, r);
					FormHelper.ValidateField(vd.WarehouseId, "WareHouseName", r.RowIndex);
					FormHelper.ValidateField(vd.ItemId, "Item", r.RowIndex);
					FormHelper.ValidateField(vd.ItemUomId, "Uom", r.RowIndex);
					FormHelper.ValidateField(vd.ItemConditionId, "ItemCondition", r.RowIndex);
					FormHelper.ValidateField(vd.RackId, "Rack Name", r.RowIndex);
					FormHelper.ValidateField(vd.ItemQty, "This Qty", r.RowIndex);
					GRN.invGrnDetail.Add(vd);
				}
				GRN.AttachmentsList = new List<DMSAttachments>();
				GRN.DeleteAttachmentsList = new List<DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<InvGrn>(GRN, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				GRN.AttachmentsValues = AttachmentsValues;
				GRN.CustomAttachmentsValues = CustomAttachmentsValues;
				int success = InvGrn.Save(GRN);
				if (Id > 0)
				{
					MessageBox.Show("Record Update Successfully [" + GRN.DocNo + "]");
					FormHelper.DeleteAllAttachmentsFromDb(AT, Id, DocumentTypeId, base.Name);
				}
				else
				{
					MessageBox.Show("Record Save Successfully [" + GRN.DocNo + "]");
				}
				reset();
				if (ChkPrint.Checked)
				{
					ShowPrint(success);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int ID)
	{
		try
		{
			reset();
			Id = ID;
			InvGrn invGrn = InvGrn.GetByID(Id);
			if (invGrn == null || invGrn.invGrnDetail == null)
			{
				MessageBox.Show("No Record Found");
				return;
			}
			tabControl1.SelectedIndex = 0;
			UpdateMode = true;
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			UpdateMode = true;
			btnDelete.Visible = true;
			txtdocno.Text = invGrn.DocNo.ToString();
			DocDate.Value = invGrn.DocDate;
			CmbBillToParty.Value = invGrn.SupplierCustomerId;
			txtReferenceNo.Text = invGrn.ReferenceDocNo;
			GPID = Conversion.ToInt((object)invGrn.InwardGatePassId);
			CmbGpNo.Text = invGrn.GpNo.ToString();
			((Control)(object)CmbVehicleType).Text = invGrn.VehicleType;
			txtVehicleNo.Text = invGrn.VehicleNo;
			txtBiltyNo.Text = invGrn.BiltyNo;
			CmbTransporter.Value = invGrn.TransporterId;
			txtFreightAmount.Text = invGrn.CarriageAmount.ToString();
			txtRemarksHeader.Text = invGrn.RemarksHeader;
			AttachmentsValues = invGrn.AttachmentsValues;
			CustomAttachmentsValues = invGrn.CustomAttachmentsValues;
			BaseDocumentTypeId = invGrn.BaseDocumentTypeId;
			dtdetail.Rows.Clear();
			FillDetailFromListCommonForReadById(invGrn, dtdetail);
			BindGrids(BaseDocumentTypeId);
			FormHelper.LoadAttachmentsForObject(AT, Id, base.Name);
			DocDate.Focus();
			EnabledDisabledFeilds(BaseDocumentTypeId, GPID);
			GridStockUpdate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailFromListCommonForReadById(InvGrn invGrn, DataTable dtdetail, bool AddExtraColumns = false)
	{
		if (((invGrn != null) ? invGrn.invGrnDetail : null) == null || invGrn.invGrnDetail.Count == 0)
		{
			return;
		}
		foreach (InvGrnDetail detail in invGrn.invGrnDetail)
		{
			DataRow dr = dtdetail.NewRow();
			dr["Id"] = detail.Id;
			dr["OrderId"] = detail.PurchaseOrderId;
			dr["OrderDetailId"] = detail.PurchaseOrderDetailId;
			dr["OrderNo"] = detail.PurchaseOrder;
			dr["DemandId"] = detail.InvPurchasedemondId;
			dr["DemandDetailId"] = detail.PurchaseDemondDetailId;
			dr["DemandNo"] = detail.PurchaseDemandNo;
			dr["DeliveryChallanId"] = detail.DeliveryChallanId;
			dr["DeliveryChallanDetailId"] = detail.DeliveryChallanDetailId;
			dr["DeliveryChallanNo"] = detail.DeliveryChallanNo;
			dr["PurchasePreBillId"] = detail.PurchasePreBillId;
			dr["PurchasePreBillDetailId"] = detail.PurchasePreBillDetailId;
			dr["PurchasePreBillNo"] = detail.PurchasePreBillNo;
			dr["PurchasePreBillQty"] = detail.PurchasePreBillQty;
			dr["WarehouseId"] = detail.WarehouseId;
			dr["WarehouseName"] = detail.WareHouseCode;
			if (AddExtraColumns)
			{
				dr["ItemCondition"] = detail.ConditionStatus;
			}
			dr["ItemId"] = detail.ItemId;
			dr["ItemCode"] = detail.ItemCode;
			dr["ItemName"] = detail.Item;
			dr["UOMId"] = detail.ItemUomId;
			dr["UOM"] = detail.UOMCode;
			dr["UomEquivalent"] = Conversion.ToDouble((object)detail.UOM);
			dr["TotalOrderQty"] = detail.PurchaseOrderQty;
			dr["UsedOrderQty"] = detail.TotalUsedOrderQty;
			dr["BalOrderQty"] = detail.BalOrderQty;
			dr["TotalDemandQty"] = detail.PurchaseDemandQty;
			dr["UsedDemandQty"] = detail.TotalUsedDemandQty;
			dr["BalDemandQty"] = detail.BalDemandQty;
			dr["TotalDeliveryChallanQty"] = detail.DeliveryChallanQty;
			dr["UsedDeliveryChallanQty"] = detail.UsedDeliveryChallanQtyGrn;
			dr["BalDeliveryChallanQty"] = detail.BalDeliveryChallanQty;
			dr["ThisQty"] = Conversion.ToDouble((object)detail.ItemQty);
			dr["Remarks"] = detail.CommentsDetail;
			dr["ItemConditionId"] = detail.ItemConditionId;
			dr["RackId"] = detail.RackId;
			dr["RackName"] = detail.rackName;
			dtdetail.Rows.Add(dr);
		}
	}

	private void FillDetailListCommonForInsertAndDelete(InvGrnDetail vd, GridEXRow r)
	{
		vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
		vd.PurchaseOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
		vd.PurchaseOrderDetailId = Conversion.ToInt(r.Cells["OrderDetailId"].Value);
		vd.PurchaseOrder = Conversion.ToInt(r.Cells["OrderNo"].Value);
		vd.InvPurchasedemondId = Conversion.ToInt(r.Cells["DemandId"].Value);
		vd.PurchaseDemondDetailId = Conversion.ToInt(r.Cells["DemandDetailId"].Value);
		vd.PurchaseDemandNo = Conversion.ToInt(r.Cells["DemandNo"].Value);
		vd.PurchasePreBillId = Conversion.ToInt(r.Cells["PurchasePreBillId"].Value);
		vd.PurchasePreBillDetailId = Conversion.ToInt(r.Cells["PurchasePreBillDetailId"].Value);
		vd.PurchasePreBillNo = Conversion.ToInt(r.Cells["PurchasePreBillNo"].Value);
		vd.DeliveryChallanId = Conversion.ToInt(r.Cells["DeliveryChallanId"].Value);
		vd.DeliveryChallanDetailId = Conversion.ToInt(r.Cells["DeliveryChallanDetailId"].Value);
		vd.DeliveryChallanNo = Conversion.ToInt(r.Cells["DeliveryChallanNo"].Value);
		vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		vd.ItemCode = Conversion.ToString(r.Cells["ItemCode"].Value);
		vd.Item = Conversion.ToString(r.Cells["ItemName"].Value);
		vd.ItemUomId = Conversion.ToInt(r.Cells["UOMId"].Value);
		vd.UOMCode = Conversion.ToString(r.Cells["UOM"].Value);
		vd.UOM = Conversion.ToDouble(r.Cells["UomEquivalent"].Value);
		vd.PurchaseOrderQty = Conversion.ToDouble(r.Cells["TotalOrderQty"].Value);
		vd.TotalUsedOrderQty = Conversion.ToDouble(r.Cells["UsedOrderQty"].Value);
		vd.BalOrderQty = Conversion.ToDouble(r.Cells["BalOrderQty"].Value);
		vd.PurchaseDemandQty = Conversion.ToDouble(r.Cells["TotalDemandQty"].Value);
		vd.TotalUsedDemandQty = Conversion.ToDouble(r.Cells["UsedDemandQty"].Value);
		vd.BalDemandQty = Conversion.ToDouble(r.Cells["BalDemandQty"].Value);
		vd.PurchasePreBillQty = Conversion.ToDouble(r.Cells["PurchasePreBillQty"].Value);
		vd.DeliveryChallanQty = Conversion.ToDouble(r.Cells["TotalDeliveryChallanQty"].Value);
		vd.UsedDeliveryChallanQtyGrn = Conversion.ToDouble(r.Cells["UsedDeliveryChallanQty"].Value);
		vd.BalDeliveryChallanQty = Conversion.ToDouble(r.Cells["BalDeliveryChallanQty"].Value);
		vd.ItemQty = Conversion.ToDouble(r.Cells["ThisQty"].Value);
		vd.GrossWeight = vd.ItemQty;
		vd.NetBillWeight = vd.ItemQty;
		vd.StockWeight = vd.NetBillWeight;
		vd.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
		vd.WareHouseCode = Conversion.ToString((object)r.Cells["WarehouseName"].Text);
		vd.ItemConditionId = Conversion.ToInt(r.Cells["ItemConditionId"].Value);
		vd.ConditionStatus = Conversion.ToString((object)r.Cells["ItemConditionId"].Text);
		vd.RackId = Conversion.ToInt(r.Cells["RackId"].Value);
		vd.rackName = Conversion.ToString((object)r.Cells["RackName"].Text);
		vd.CommentsDetail = Conversion.ToString(r.Cells["Remarks"].Value);
	}

	private void reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			Id = 0;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			GPID = 0;
			BaseDocumentTypeId = 0;
			((Control)(object)CmbBillToParty).Text = string.Empty;
			txtReferenceNo.Text = string.Empty;
			txtRemarksHeader.Text = string.Empty;
			CmbGpNo.Text = string.Empty;
			((Control)(object)CmbVehicleType).Text = string.Empty;
			txtVehicleNo.Text = string.Empty;
			txtBiltyNo.Text = string.Empty;
			((Control)(object)CmbTransporter).Text = string.Empty;
			txtFreightAmount.Text = string.Empty;
			dtdetail.Rows.Clear();
			BindGrids(BaseDocumentTypeId);
			DocDate.Focus();
			txtdocno.Text = GetDocumentCode();
			GetConfigurationsFromGlobalAndBindValuesInColumns();
			EnabledDisabledFeilds(BaseDocumentTypeId, GPID);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "BranchesAllocatedToUser", "AccountsWithCustomGroupId", "UomSchedule", "SupplierCustomerLists", "VendorsAndCustomersForTransporters", "WarehousesWithBranches", "AllItems", "RacksWithWarehouseAndItem", "ItemConditions" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			GetConfigurationsFromGlobal();
			SupplierDtFillFromGlobal();
			BindSupplierName();
			TransporterDtFillFromGlobal();
			TransporterAcFill();
			VehicleTypesBind(VehicleType.GetAll());
			GridComboBind();
			GetConfigurationsFromGlobalAndBindValuesInColumns();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrnStore_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			BtnRefresh_Click(null, null);
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
			((Control)(object)cmbSupplierNameHistory).Text = string.Empty;
			GrdHistory.DataSource = null;
			GrdHistoryDetail.DataSource = null;
			FromDateHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name != null && base.ActiveControl.Name == "")
			{
				FromDateHistory.Focus();
			}
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
			HistoryGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters grn = new ReportsParameters();
			grn.OrganizationId = UserAccount.OrganizationId;
			grn.CompanyId = UserAccount.CompanyId;
			grn.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			grn.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			grn.BranchesId = UserAccount.BranchesId;
			grn.DocumentTypeId = DocumentTypeId;
			grn.RefDocumentTypeIds = "1,2,3";
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					grn.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					grn.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					grn.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					grn.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					grn.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					grn.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					grn.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					grn.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			grn.FromDocNo = Conversion.ToInt((object)txtFromDocNoHistory.Text);
			grn.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			grn.SupplierCustomerId = Conversion.ToInt(cmbSupplierNameHistory.Value);
			DataTable dthistory = InvGrn.GetHisoty(grn);
			if (dthistory.Rows.Count == 0)
			{
				GrdHistory.DataSource = null;
				GrdHistoryDetail.DataSource = null;
			}
			DataTable dataTable = dtHistoryGrid;
			if (dataTable != null && dataTable.Columns.Count == 0)
			{
				dtHistoryGrid.Columns.Add("Id");
				dtHistoryGrid.Columns.Add("BaseDocumentTypeId");
				dtHistoryGrid.Columns.Add("BaseDocumentType");
				dtHistoryGrid.Columns.Add("DocDate");
				dtHistoryGrid.Columns.Add("DocNo");
				dtHistoryGrid.Columns.Add("SupplierName");
				dtHistoryGrid.Columns.Add("GpNo");
				dtHistoryGrid.Columns.Add("VehicleNo");
				dtHistoryGrid.Columns.Add("BiltyNo");
				dtHistoryGrid.Columns.Add("Transporter");
				dtHistoryGrid.Columns.Add("FreightAmount", typeof(double));
				dtHistoryGrid.Columns.Add("RemarksHeader");
				dtHistoryGrid.Columns.Add("EntryUser");
				dtHistoryGrid.Columns.Add("EntryDate");
				dtHistoryGrid.Columns.Add("ModifyUser");
				dtHistoryGrid.Columns.Add("ModifyDate");
				dtHistoryGrid.Columns.Add("NoOfAttachments");
			}
			dtHistoryGrid.Clear();
			for (int i = 0; i < dthistory.Rows.Count; i++)
			{
				dtHistoryGrid.Rows.Add(dthistory.Rows[i]["Id"], dthistory.Rows[i]["BaseDocumentTypeId"], dthistory.Rows[i]["BaseDocumentType"], Conversion.ToDateTime(dthistory.Rows[i]["DocDate"]).ToString("dd-MMM-yy"), dthistory.Rows[i]["DocNo"], dthistory.Rows[i]["SupplierName"], dthistory.Rows[i]["GpNo"], dthistory.Rows[i]["VehicleNo"], dthistory.Rows[i]["BiltyNo"], dthistory.Rows[i]["Transporter"], dthistory.Rows[i]["CarriageAmount"], dthistory.Rows[i]["RemarksHeader"], dthistory.Rows[i]["EntryUser"], dthistory.Rows[i]["EntryDate"], dthistory.Rows[i]["ModifyUser"], dthistory.Rows[i]["ModifyDate"], dthistory.Rows[i]["NoOfAttachments"]);
			}
			GrdHistory.DataSource = dtHistoryGrid;
			GrdHistory.RetrieveStructure();
			HistoryGridSettings();
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
			List<string> hiddenColumns = new List<string> { "Id", "BaseDocumentTypeId" };
			GridEX_Helper.GridWrappingAndColumnSettings(GrdHistory, 3, 3, hiddenColumns);
			GrdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GrdHistory.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yy";
			GrdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			GrdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			GrdHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant + 10;
			GrdHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			GrdHistory.RootTable.Columns["SupplierName"].Width = 200;
			GrdHistory.RootTable.Columns["GpNo"].Width = 120;
			GrdHistory.RootTable.Columns["VehicleNo"].Width = 120;
			GrdHistory.RootTable.Columns["BiltyNo"].Width = 120;
			GrdHistory.RootTable.Columns["Transporter"].Width = 150;
			GrdHistory.RootTable.Columns["FreightAmount"].Width = 120;
			GrdHistory.RootTable.Columns["RemarksHeader"].Width = Constants.InventoryConstants.RemarksConstants;
			GrdHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			GrdHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants;
			GridEX_Helper.AddButton(GrdHistory, "Edit", "Edit", 50, GrdHistory.RootTable.Columns["BaseDocumentTypeId"].Position - 1);
			GridEX_Helper.AddButton(GrdHistory, "Print", "Print", 50, GrdHistory.RootTable.Columns["BaseDocumentTypeId"].Position - 1);
			GrdHistory.FrozenColumns = 4;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GrdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int Currid = Conversion.ToInt(item.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					ReadById(Currid);
				}
				if (e.Column.Key == "Print")
				{
					ShowPrint(Currid);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GrdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), DocumentTypeId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			ReadById(Conversion.ToInt((object)GrdHistory.CurrentRow.Cells["Id"].Value.ToString()));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = GrdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int num = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				int BaseDocumentTypeId = Conversion.ToInt((object)r.Cells["BaseDocumentTypeId"].Value.ToString());
				InvGrn invGrn = InvGrn.GetByID(num);
				if (invGrn == null)
				{
					GrdHistoryDetail.DataSource = null;
					return;
				}
				DataTable dtDetailHistory = dtdetail.Clone();
				dtDetailHistory.Columns.Add("WareHouseName");
				dtDetailHistory.Columns.Add("ItemCondition");
				FillDetailFromListCommonForReadById(invGrn, dtDetailHistory, AddExtraColumns: true);
				GrdHistoryDetail.DataSource = dtDetailHistory;
				GrdHistoryDetail.RetrieveStructure();
				grddetailhistorySettings(BaseDocumentTypeId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grddetailhistorySettings(int BaseDocumentTypeId)
	{
		try
		{
			grdSettings(GrdHistoryDetail, BaseDocumentTypeId);
			GrdHistoryDetail.RootTable.Columns["WareHouseId"].Visible = false;
			switch (BaseDocumentTypeId)
			{
			case 1:
				GrdHistoryDetail.RootTable.Columns["UsedOrderQty"].Visible = false;
				break;
			case 2:
				GrdHistoryDetail.RootTable.Columns["UsedDemandQty"].Visible = false;
				break;
			case 3:
				GrdHistoryDetail.RootTable.Columns["UsedDeliveryChallanQty"].Visible = false;
				break;
			}
			GrdHistoryDetail.RootTable.Columns["ItemConditionId"].Visible = false;
			GrdHistoryDetail.RootTable.Columns["AvailableStockQty"].Visible = false;
			GrdHistoryDetail.RootTable.Columns["WareHouseName"].Position = GrdHistoryDetail.RootTable.Columns["WareHouseId"].Position + 1;
			GrdHistoryDetail.RootTable.Columns["ItemCondition"].Position = GrdHistoryDetail.RootTable.Columns["UomEquivalent"].Position + 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = GrdHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || GrdHistory.CurrentColumn == null)
			{
				return;
			}
			int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (GrdHistory.CurrentColumn.Key == "Edit")
				{
					ReadById(CurrId);
				}
				if (GrdHistory.CurrentColumn.Key == "Print")
				{
					ShowPrint(CurrId);
				}
				if (GrdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				ReadById(CurrId);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				ShowPrint(CurrId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvFrmGRN_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		if (e.KeyCode == Keys.Delete && tabControl1.SelectedIndex == 1)
		{
			GridEXRow[] checkedRows = grd.GetCheckedRows();
			for (int i = 0; i < checkedRows.Length; i++)
			{
				int.Parse(checkedRows[i].Cells["Id"].Value.ToString());
			}
		}
		if (btnSave.Visible && btnSave.Enabled && e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0)
		{
			btnSave_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.N && tabControl1.SelectedIndex == 0)
		{
			btnNew_Click(null, null);
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
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
		{
			btnUpdate_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.P)
		{
			printToolStripButton_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.L)
		{
			btnLoadSupplySchedule_Click(null, null);
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
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Item in Detail Box");
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
		MakeShortCutKeys();
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, GrdHistory, dtHistoryGrid);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grd, dtdetail);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void printToolStripButton_Click(object sender, EventArgs e)
	{
		try
		{
			ShowPrint(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ShowPrint(int PrintId)
	{
		try
		{
			CommonServices.GrnSlipReport212(PrintId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EnabledDisabledFeilds(int BaseDocumentTypeId, int GpId)
	{
		try
		{
			((Control)(object)CmbBillToParty).Enabled = BaseDocumentTypeId == 2;
			((Control)(object)CmbVehicleType).Enabled = true;
			CmbGpNo.Enabled = GpId == 0;
			txtVehicleNo.Enabled = GpId == 0;
			txtBiltyNo.Enabled = GpId == 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadSupplySchedule_Click(object sender, EventArgs e)
	{
		try
		{
			if (BaseDocumentTypeId > 0 && BaseDocumentTypeId != 1 && grd.RowCount > 0)
			{
				string formName = string.Empty;
				if (BaseDocumentTypeId == 2)
				{
					formName = "Purchase Demand";
				}
				else if (BaseDocumentTypeId == 3)
				{
					formName = "Delivery Challan";
				}
				MessageBox.Show("Data for " + formName + " already exists in the grid. Please Reset the Form before loading data for Purchase Order.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			using frmPendingPurchaseOrderStoreLoader LoadOrder = new frmPendingPurchaseOrderStoreLoader(UserAccount);
			LoadOrder.ShowDialog();
			LoadOrder.SupplierId = Conversion.ToInt(CmbBillToParty.Value);
			LoadInGridDetailFromPO(LoadOrder.dtLoader);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetailFromPO(DataTable dtLoader)
	{
		try
		{
			if (dtLoader == null || dtLoader.Rows.Count == 0)
			{
				if (grd.GetRows().Count() == 0)
				{
					BaseDocumentTypeId = 0;
				}
				return;
			}
			BaseDocumentTypeId = 1;
			DataRow firstLoaderRow = dtLoader.Rows[0];
			if (grd.GetRows().Count() > 0)
			{
				int currentOrderId = Conversion.ToInt(firstLoaderRow["Id"]);
				GridEXRow firstGridRow = grd.GetDataRows().FirstOrDefault();
				int existingOrderId = ((firstGridRow != null) ? Conversion.ToInt(firstGridRow.Cells["OrderId"].Value) : 0);
				if (existingOrderId != 0 && existingOrderId != currentOrderId)
				{
					MessageBox.Show("Data for another Order already exists in the grid. Please Reset the Form before loading data for a different Order.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				int currentPartyId = Conversion.ToInt(firstLoaderRow["SupplierCustomerId"]);
				int existingPartyId = Conversion.ToInt(CmbBillToParty.Value);
				if (existingPartyId != 0 && existingPartyId != currentPartyId)
				{
					MessageBox.Show("Data for another party already exists in the grid. Please Reset the Form before loading data for a different party.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			CmbBillToParty.Value = Conversion.ToInt(firstLoaderRow["SupplierCustomerId"]);
			int currentGatePassId = Conversion.ToInt(firstLoaderRow["GatePassId"]);
			if (currentGatePassId > 0)
			{
				GPID = currentGatePassId;
				CmbGpNo.Text = Conversion.ToString(firstLoaderRow["GpSrNo"]);
				((Control)(object)CmbVehicleType).Text = Conversion.ToString(firstLoaderRow["VehicleType"]);
				txtVehicleNo.Text = Conversion.ToString(firstLoaderRow["VehicleNo"]);
				txtBiltyNo.Text = Conversion.ToString(firstLoaderRow["BiltyNo"]);
			}
			HashSet<int> existingOrderDetailIds = new HashSet<int>();
			foreach (DataRow r in dtdetail.Rows)
			{
				existingOrderDetailIds.Add(Conversion.ToInt(r["OrderDetailId"]));
			}
			foreach (DataRow row in dtLoader.Rows)
			{
				int orderDetailId = Conversion.ToInt(row["OrderDetailId"]);
				if (existingOrderDetailIds.Contains(orderDetailId))
				{
					continue;
				}
				existingOrderDetailIds.Add(orderDetailId);
				double totalOrderQty = Conversion.ToDouble(row["OrderItemQty"]);
				double usedOrderQty = Conversion.ToDouble(row["UsedQty"]);
				double balOrderQty = Conversion.ToDouble(row["ItemQty"]);
				DataRow dr = dtdetail.NewRow();
				dr["Id"] = 0;
				dr["OrderId"] = row["Id"];
				dr["OrderDetailId"] = orderDetailId;
				dr["OrderNo"] = row["DocNo"];
				dr["DemandId"] = 0;
				dr["DemandDetailId"] = 0;
				dr["DemandNo"] = 0;
				dr["DeliveryChallanId"] = 0;
				dr["DeliveryChallanDetailId"] = 0;
				dr["DeliveryChallanNo"] = 0;
				dr["ItemId"] = row["ItemId"];
				dr["ItemCode"] = row["ItemCode"];
				dr["ItemName"] = row["ItemName"];
				dr["UOMId"] = row["ItemUOMId"];
				dr["UOM"] = row["UomCode"];
				dr["UomEquivalent"] = row["Equivalent"];
				if (dtLoader.Columns.Contains("ItemConditionId"))
				{
					dr["ItemConditionId"] = row["ItemConditionId"];
				}
				dr["TotalOrderQty"] = totalOrderQty;
				dr["UsedOrderQty"] = usedOrderQty;
				dr["BalOrderQty"] = balOrderQty;
				dr["TotalDemandQty"] = 0;
				dr["UsedDemandQty"] = 0;
				dr["BalDemandQty"] = 0;
				dr["TotalDeliveryChallanQty"] = 0;
				dr["UsedDeliveryChallanQty"] = 0;
				dr["BalDeliveryChallanQty"] = 0;
				dr["ThisQty"] = balOrderQty;
				dr["Remarks"] = string.Empty;
				Dictionary<int, List<RacksWithWarehouseAndItem>> dictionary = (from x in clsGlobalVariables.racksWithWarehouseAndItems
					group x by x.ItemId).ToDictionary((IGrouping<int, RacksWithWarehouseAndItem> g) => g.Key, (IGrouping<int, RacksWithWarehouseAndItem> g) => g.ToList());
				int itemId = Conversion.ToInt(row["ItemId"]);
				if (dictionary.TryGetValue(itemId, out var itemData))
				{
					(int, string, int, string) result = FormHelper.ResolveWarehouseAndRack(itemData, WareHouseConfigId);
					if (result.Item1 > 0)
					{
						dr["WarehouseId"] = result.Item1;
						dr["WarehouseName"] = result.Item2;
					}
					if (result.Item3 > 0)
					{
						dr["RackId"] = result.Item3;
						dr["RackName"] = result.Item4;
					}
				}
				dtdetail.Rows.Add(dr);
			}
			BindGrids(BaseDocumentTypeId);
			EnabledDisabledFeilds(BaseDocumentTypeId, GPID);
			GridStockUpdate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnLoadPurchaseDemand_Click(object sender, EventArgs e)
	{
		try
		{
			if (BaseDocumentTypeId > 0 && BaseDocumentTypeId != 2 && grd.RowCount > 0)
			{
				string formName = string.Empty;
				if (BaseDocumentTypeId == 1)
				{
					formName = "Purchase Order";
				}
				else if (BaseDocumentTypeId == 3)
				{
					formName = "Delivery Challan";
				}
				MessageBox.Show("Data for " + formName + " already exists in the grid. Please Reset the Form before loading data for Purchase Demand.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			using frmPendingPurchaseDemand LoadOrder = new frmPendingPurchaseDemand(UserAccount);
			LoadOrder.ShowDialog();
			LoadInGridDetailForDemand(LoadOrder.dtLoader);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetailForDemand(DataTable dtLoader)
	{
		try
		{
			if (dtLoader == null || dtLoader.Rows.Count == 0)
			{
				if (grd.GetRows().Count() == 0)
				{
					BaseDocumentTypeId = 0;
				}
				return;
			}
			DataRow firstLoaderRow = dtLoader.Rows[0];
			int currentGatePassId = Conversion.ToInt(firstLoaderRow["GatePassId"]);
			BaseDocumentTypeId = 2;
			if (grd.GetRows().Count() > 0)
			{
				int currentOrderId = Conversion.ToInt(firstLoaderRow["Id"]);
				GridEXRow firstGridRow = grd.GetDataRows().FirstOrDefault();
				int existingOrderId = ((firstGridRow != null) ? Conversion.ToInt(firstGridRow.Cells["DemandId"].Value) : 0);
				if (GPID == 0 && currentGatePassId > 0)
				{
					MessageBox.Show("Data for another demand (without a Gate Pass) already exists in the grid. You cannot load a new row with a Gate Pass. Please reset the form or select a demand without a Gate Pass.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				if (GPID > 0 && existingOrderId != 0 && existingOrderId != currentOrderId)
				{
					MessageBox.Show("Data for another demand (with a Gate Pass) already exists in the grid. Please reset the form before loading data for a different demand.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			if (currentGatePassId > 0)
			{
				GPID = currentGatePassId;
				CmbGpNo.Text = Conversion.ToString(firstLoaderRow["GpSrNo"]);
				((Control)(object)CmbVehicleType).Text = Conversion.ToString(firstLoaderRow["VehicleType"]);
				txtVehicleNo.Text = Conversion.ToString(firstLoaderRow["VehicleNo"]);
				txtBiltyNo.Text = Conversion.ToString(firstLoaderRow["BiltyNo"]);
			}
			HashSet<int> existingDemandDetailIds = new HashSet<int>(from r in grd.GetRows()
				select Conversion.ToInt(r.Cells["DemandDetailId"].Value));
			foreach (DataRow row in dtLoader.Rows)
			{
				int demandDetailId = Conversion.ToInt(row["DetailId"]);
				if (!existingDemandDetailIds.Add(demandDetailId))
				{
					continue;
				}
				double totalDemandQty = Conversion.ToDouble(row["ApprovedQty"]);
				double usedDemandQty = Conversion.ToDouble(row["QtyUsedInPreBill"]) + Conversion.ToDouble(row["QtyUsedInGrnDirectly"]);
				double balDemandQty = Conversion.ToDouble(row["BalanceQty"]);
				double thisQty = balDemandQty;
				DataRow dr = dtdetail.NewRow();
				dr["Id"] = 0;
				dr["DemandId"] = row["Id"];
				dr["DemandDetailId"] = demandDetailId;
				dr["DemandNo"] = row["DocNo"];
				dr["OrderId"] = 0;
				dr["OrderDetailId"] = 0;
				dr["OrderNo"] = 0;
				dr["DeliveryChallanId"] = 0;
				dr["DeliveryChallanDetailId"] = 0;
				dr["DeliveryChallanNo"] = 0;
				dr["ItemId"] = row["ItemId"];
				dr["ItemCode"] = row["ItemCode"];
				dr["ItemName"] = row["ItemName"];
				dr["UOMId"] = row["ItemSchuomId"];
				dr["UOM"] = row["Uom"];
				dr["UomEquivalent"] = Conversion.ToDouble(row["UomEquivalent"]);
				if (dtLoader.Columns.Contains("ItemConditionId"))
				{
					dr["ItemConditionId"] = row["ItemConditionId"];
				}
				dr["TotalOrderQty"] = 0;
				dr["UsedOrderQty"] = 0;
				dr["BalOrderQty"] = 0;
				dr["TotalDemandQty"] = totalDemandQty;
				dr["UsedDemandQty"] = usedDemandQty;
				dr["BalDemandQty"] = balDemandQty;
				dr["TotalDeliveryChallanQty"] = 0;
				dr["UsedDeliveryChallanQty"] = 0;
				dr["BalDeliveryChallanQty"] = 0;
				dr["ThisQty"] = thisQty;
				dr["Remarks"] = Conversion.ToString(row["RemarksDetail"]);
				Dictionary<int, List<RacksWithWarehouseAndItem>> dictionary = (from x in clsGlobalVariables.racksWithWarehouseAndItems
					group x by x.ItemId).ToDictionary((IGrouping<int, RacksWithWarehouseAndItem> g) => g.Key, (IGrouping<int, RacksWithWarehouseAndItem> g) => g.ToList());
				int itemId = Conversion.ToInt(row["ItemId"]);
				if (dictionary.TryGetValue(itemId, out var itemData))
				{
					(int, string, int, string) result = FormHelper.ResolveWarehouseAndRack(itemData, WareHouseConfigId);
					if (result.Item1 > 0)
					{
						dr["WarehouseId"] = result.Item1;
						dr["WarehouseName"] = result.Item2;
					}
					if (result.Item3 > 0)
					{
						dr["RackId"] = result.Item3;
						dr["RackName"] = result.Item4;
					}
				}
				dtdetail.Rows.Add(dr);
			}
			BindGrids(BaseDocumentTypeId);
			EnabledDisabledFeilds(BaseDocumentTypeId, GPID);
			GridStockUpdate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnLoadDeliveryChallan_Click(object sender, EventArgs e)
	{
		try
		{
			if (BaseDocumentTypeId > 0 && BaseDocumentTypeId != 3 && grd.RowCount > 0)
			{
				string formName = string.Empty;
				if (BaseDocumentTypeId == 2)
				{
					formName = "Purchase Demand";
				}
				else if (BaseDocumentTypeId == 1)
				{
					formName = "Purchase Order";
				}
				MessageBox.Show("Data for " + formName + " already exists in the grid. Please Reset the Form before loading data for Delivery Challan.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			using frmPendingDeliveryChallanLoader LoadOrder = new frmPendingDeliveryChallanLoader(UserAccount);
			LoadOrder.ShowDialog();
			LoadInGridDetailFromDeliveryChallan(LoadOrder.dtLoader);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetailFromDeliveryChallan(DataTable dtLoader)
	{
		try
		{
			if (dtLoader == null || dtLoader.Rows.Count == 0)
			{
				if (grd.GetRows().Count() == 0)
				{
					BaseDocumentTypeId = 0;
				}
				return;
			}
			BaseDocumentTypeId = 3;
			DataRow firstLoaderRow = dtLoader.Rows[0];
			if (grd.GetRows().Count() > 0)
			{
				int currentOrderId = Conversion.ToInt(firstLoaderRow["DeliveryChallanHeaderId"]);
				GridEXRow firstGridRow = grd.GetDataRows().FirstOrDefault();
				int existingOrderId = ((firstGridRow != null) ? Conversion.ToInt(firstGridRow.Cells["DeliveryChallanId"].Value) : 0);
				if (existingOrderId != 0 && existingOrderId != currentOrderId)
				{
					MessageBox.Show("Data for another Delivery-Challan already exists in the grid. Please Reset the Form before loading data for a different Delivery-Challan.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				int currentPartyId = Conversion.ToInt(firstLoaderRow["BillToPartyId"]);
				int existingPartyId = Conversion.ToInt(CmbBillToParty.Value);
				if (existingPartyId != 0 && existingPartyId != currentPartyId)
				{
					MessageBox.Show("Data for another party already exists in the grid. Please Reset the Form before loading data for a different party.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			CmbBillToParty.Value = Conversion.ToInt(firstLoaderRow["BillToPartyId"]);
			GPID = Conversion.ToInt(firstLoaderRow["GatePassId"]);
			CmbGpNo.Text = Conversion.ToString(firstLoaderRow["GpSrNo"]);
			((Control)(object)CmbVehicleType).Text = Conversion.ToString(firstLoaderRow["VehicleType"]);
			txtVehicleNo.Text = Conversion.ToString(firstLoaderRow["VehicleNo"]);
			txtBiltyNo.Text = Conversion.ToString(firstLoaderRow["BiltyNo"]);
			txtRemarksHeader.Text = Conversion.ToString(firstLoaderRow["RemarksHeader"]);
			HashSet<int> existingChallanDetailIds = new HashSet<int>();
			foreach (DataRow r in dtdetail.Rows)
			{
				existingChallanDetailIds.Add(Conversion.ToInt(r["DeliveryChallanDetailId"]));
			}
			foreach (DataRow row in dtLoader.Rows)
			{
				int DetailId = Conversion.ToInt(row["DetailId"]);
				if (existingChallanDetailIds.Contains(DetailId))
				{
					continue;
				}
				existingChallanDetailIds.Add(DetailId);
				double ChallanQty = Conversion.ToDouble(row["ChallanQty"]);
				double ChallanQtyUsedInGrn = Conversion.ToDouble(row["ChallanQtyUsedInGrn"]);
				double BalChallanQty = Conversion.ToDouble(row["BalChallanQty"]);
				DataRow dr = dtdetail.NewRow();
				dr["Id"] = 0;
				dr["OrderId"] = row["PurchaseOrderDetailId"];
				dr["OrderDetailId"] = row["PurchaseOrderHeaderId"];
				dr["OrderNo"] = row["PurchaseOrderDocNo"];
				dr["DemandId"] = row["PurchaseDemandHeaderId"];
				dr["DemandDetailId"] = row["PurchaseDemandDetailId"];
				dr["DemandNo"] = row["PurchaseDemandDocNo"];
				dr["DeliveryChallanId"] = row["DeliveryChallanHeaderId"];
				dr["DeliveryChallanDetailId"] = DetailId;
				dr["DeliveryChallanNo"] = row["DocNo"];
				dr["ItemId"] = row["ItemId"];
				dr["ItemCode"] = row["ItemCode"];
				dr["ItemName"] = row["ItemName"];
				dr["UOMId"] = row["UomId"];
				dr["UOM"] = row["Uom"];
				dr["UomEquivalent"] = row["UomEquivalent"];
				if (dtLoader.Columns.Contains("ItemConditionId"))
				{
					dr["ItemConditionId"] = row["ItemConditionId"];
				}
				dr["TotalOrderQty"] = row["PurchaseOrderQty"];
				dr["UsedOrderQty"] = 0;
				dr["BalOrderQty"] = 0;
				dr["TotalDemandQty"] = row["PurchaseDemandQty"];
				dr["UsedDemandQty"] = 0;
				dr["BalDemandQty"] = 0;
				dr["PurchasePreBillId"] = row["PurchasePreBillHeaderId"];
				dr["PurchasePreBillDetailId"] = row["PurchasePreBillDetailId"];
				dr["PurchasePreBillNo"] = row["PurchasePreBillDocNo"];
				dr["PurchasePreBillQty"] = row["PurchasePreBillQty"];
				dr["TotalDeliveryChallanQty"] = ChallanQty;
				dr["UsedDeliveryChallanQty"] = ChallanQtyUsedInGrn;
				dr["BalDeliveryChallanQty"] = BalChallanQty;
				dr["ThisQty"] = BalChallanQty;
				dr["Remarks"] = string.Empty;
				int WarehouseId = ((Conversion.ToInt(row["WareHouseId"]) > 0) ? Conversion.ToInt(row["WareHouseId"]) : WareHouseConfigId);
				Dictionary<int, List<RacksWithWarehouseAndItem>> dictionary = (from x in clsGlobalVariables.racksWithWarehouseAndItems
					group x by x.ItemId).ToDictionary((IGrouping<int, RacksWithWarehouseAndItem> g) => g.Key, (IGrouping<int, RacksWithWarehouseAndItem> g) => g.ToList());
				int itemId = Conversion.ToInt(row["ItemId"]);
				if (dictionary.TryGetValue(itemId, out var itemData))
				{
					(int, string, int, string) result = FormHelper.ResolveWarehouseAndRack(itemData, WarehouseId);
					if (result.Item1 > 0)
					{
						dr["WarehouseId"] = result.Item1;
						dr["WarehouseName"] = result.Item2;
					}
					if (result.Item3 > 0)
					{
						dr["RackId"] = result.Item3;
						dr["RackName"] = result.Item4;
					}
				}
				dtdetail.Rows.Add(dr);
			}
			BindGrids(BaseDocumentTypeId);
			EnabledDisabledFeilds(BaseDocumentTypeId, GPID);
			GridStockUpdate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnResetRegister_Click(object sender, EventArgs e)
	{
		try
		{
			txtDatefromRegister.Value = DateTime.Now;
			txtDateToRegister.Value = DateTime.Now;
			txtDocNoFromRegister.Text = string.Empty;
			txtDocNoToRegister.Text = string.Empty;
			((Control)(object)CmbPartyNameRegister).Text = string.Empty;
			((Control)(object)CmbItemNameRegister).Text = string.Empty;
			((Control)(object)CmbWarehouseRegister).Text = string.Empty;
			((Control)(object)CmbTransporterRegister).Text = string.Empty;
			GrdiRegister.DataSource = null;
			txtDatefromRegister.Focus();
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
			HistoryComboBind(InvGrn.GetDataForDropDownFromGrn(UserAccount.OrganizationId, UserAccount.CompanyId, DocumentTypeId.ToString(), (string)null, Conversion.ToString((object)UserAccount.BranchesId), 0));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void BtnShowRegister_Click(object sender, EventArgs e)
	{
		try
		{
			RegisterGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RegisterGridFill()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dtRegisterFromDb.Clear();
			ReportsParameters grn = new ReportsParameters();
			grn.OrganizationId = UserAccount.OrganizationId;
			grn.CompanyId = UserAccount.CompanyId;
			grn.BranchesId = UserAccount.BranchesId;
			grn.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			grn.DocumentTypeId = DocumentTypeId;
			grn.RefDocumentTypeIds = "1,2,3";
			grn.FromDocNo = Conversion.ToInt((object)txtDocNoFromRegister.Text.Trim());
			grn.ToDocNo = Conversion.ToInt((object)txtDocNoToRegister.Text.Trim());
			grn.FromDate = Conversion.ToDateTime((object)txtDatefromRegister.Value);
			grn.ToDate = Conversion.ToDateTime((object)txtDateToRegister.Value);
			grn.SupplierCustomerId = Conversion.ToInt(CmbPartyNameRegister.Value);
			grn.WarehouseId = Conversion.ToInt(CmbWarehouseRegister.Value);
			grn.ItemId = Conversion.ToInt(CmbItemNameRegister.Value);
			grn.PendingForView = (OnlyPending ? 1 : 0);
			dtRegisterFromDb = InvGrn.GrnRegisterStore(grn);
			DataTable dataTable = dtRegisterFromDb;
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				DataTable dataTable2 = dtRegisterGrid;
				if (dataTable2 != null && dataTable2.Columns.Count == 0)
				{
					dtRegisterGrid.Columns.Add("Id", typeof(int));
					dtRegisterGrid.Columns.Add("BaseDocumentType", typeof(string));
					dtRegisterGrid.Columns.Add("DocDate", typeof(DateTime));
					dtRegisterGrid.Columns.Add("DocNo", typeof(int));
					dtRegisterGrid.Columns.Add("OrderId", typeof(int));
					dtRegisterGrid.Columns.Add("OrderNo", typeof(int));
					dtRegisterGrid.Columns.Add("OrderDate", typeof(string));
					dtRegisterGrid.Columns.Add("SupplierCustomerId", typeof(int));
					dtRegisterGrid.Columns.Add("PartyName", typeof(string));
					dtRegisterGrid.Columns.Add("ReferenceNo", typeof(string));
					dtRegisterGrid.Columns.Add("VehicleNo", typeof(string));
					dtRegisterGrid.Columns.Add("BiltyNo", typeof(string));
					dtRegisterGrid.Columns.Add("TransporterId", typeof(int));
					dtRegisterGrid.Columns.Add("TransporterName", typeof(string));
					dtRegisterGrid.Columns.Add("FreightAmount", typeof(double));
					dtRegisterGrid.Columns.Add("ItemId", typeof(int));
					dtRegisterGrid.Columns.Add("ItemName", typeof(string));
					dtRegisterGrid.Columns.Add("ItemUOM", typeof(string));
					dtRegisterGrid.Columns.Add("ItemQty", typeof(double));
					dtRegisterGrid.Columns.Add("WareHouseName", typeof(string));
					dtRegisterGrid.Columns.Add("NoOfAttachments", typeof(int));
					dtRegisterGrid.Columns.Add("DocumentTypeId", typeof(int));
				}
				dtRegisterGrid.Rows.Clear();
				foreach (DataRow dr in dtRegisterFromDb.Rows)
				{
					string OrderDate = ((!Conversion.CheckDateTimeNull(Conversion.ToDateTime(dr["PoDate"]))) ? Conversion.ToDateTime(dr["PoDate"]).ToString("d") : "");
					dtRegisterGrid.Rows.Add(dr["Id"], dr["BaseDocumentType"], Conversion.ToDateTime(dr["DocDate"]), dr["DocNo"], dr["OrderId"], dr["PoNo"], OrderDate, dr["SupplierCustomerId"], dr["CompanyName"], dr["ReferenceDocNo"], dr["VehicleNo"], dr["BiltyNo"], dr["TransporterId"], dr["TransporterName"], dr["FreightAmount"], dr["ItemId"], dr["ItemName"], dr["UOMCode"], dr["ItemQty"], dr["WareHouseName"], dr["NoOfAttachments"], dr["DocumentTypeId"]);
				}
				GrdiRegister.DataSource = dtRegisterGrid;
				GrdiRegister.RetrieveStructure();
				RegisterGridSetting();
			}
			else
			{
				GrdiRegister.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RegisterGridSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "OrderId", "SupplierCustomerId", "TransporterId", "ItemId", "DocumentTypeId" };
			GridEX_Helper.GridWrappingAndColumnSettings(GrdiRegister, 3, 3, hiddenColumns);
			GrdiRegister.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GrdiRegister.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yy";
			GrdiRegister.RootTable.Columns["OrderDate"].FormatString = "dd-MMM-yy";
			GrdiRegister.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant + 10;
			GrdiRegister.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			GrdiRegister.RootTable.Columns["OrderNo"].Width = 120;
			GrdiRegister.RootTable.Columns["OrderDate"].Width = Constants.InventoryConstants.DateConstant;
			GrdiRegister.RootTable.Columns["PartyName"].Width = 200;
			GrdiRegister.RootTable.Columns["ReferenceNo"].Width = 120;
			GrdiRegister.RootTable.Columns["VehicleNo"].Width = 120;
			GrdiRegister.RootTable.Columns["BiltyNo"].Width = 120;
			GrdiRegister.RootTable.Columns["TransporterName"].Width = 150;
			GrdiRegister.RootTable.Columns["FreightAmount"].Width = 120;
			GrdiRegister.RootTable.Columns["ItemName"].Width = 180;
			GrdiRegister.RootTable.Columns["ItemUOM"].Width = 80;
			GrdiRegister.RootTable.Columns["ItemQty"].Width = 100;
			GrdiRegister.RootTable.Columns["WareHouseName"].Width = 150;
			GridEX_Helper.AddButton(GrdiRegister, "Print", "Print", 50, GrdiRegister.RootTable.Columns["DocNo"].Position - 1);
			GrdiRegister.FrozenColumns = 4;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdiRegister_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GrdiRegister.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Column.Key == "Print")
			{
				ShowPrint(Conversion.ToInt(GrdiRegister.CurrentRow.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdiRegister_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GrdiRegister.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), DocumentTypeId);
			}
			if (!(e.Column.Key == "OrderNo") || clsGlobalVariables.ScreenViewReights.Count <= 0)
			{
				return;
			}
			List<tblUserRights> newList = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "PurchsaeOrder").ToList();
			if (newList.Count <= 0)
			{
				return;
			}
			if (bool.Parse(newList[0].Value.ToString()))
			{
				DataTable dt = PurchaseOrderReports.PurchaseOrderSlipReport203(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = 42,
					OrderId = Conversion.ToInt(item.Cells["OrderId"].Value),
					ApprovedFilter = "All"
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					throw new Exception("Not Record Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "201-InvRptPurchaseOrderGeneralSlip.rpt");
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

	private void BtnRegisterPrint_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtRegisterFromDb == null || dtRegisterFromDb.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@PrintedBy", UserAccount.UserName.ToString());
			val.ShowReportWithDataTable(dtRegisterFromDb, "336-GrnStoreRegister.rpt");
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
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, GrdiRegister, dtRegisterGrid);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
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
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Expected O, but got Unknown
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Expected O, but got Unknown
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Expected O, but got Unknown
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Expected O, but got Unknown
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Expected O, but got Unknown
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Expected O, but got Unknown
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Expected O, but got Unknown
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Expected O, but got Unknown
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Expected O, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Expected O, but got Unknown
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Expected O, but got Unknown
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Expected O, but got Unknown
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Expected O, but got Unknown
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Expected O, but got Unknown
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Expected O, but got Unknown
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Expected O, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Expected O, but got Unknown
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Expected O, but got Unknown
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Expected O, but got Unknown
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Expected O, but got Unknown
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Expected O, but got Unknown
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Expected O, but got Unknown
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Expected O, but got Unknown
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Expected O, but got Unknown
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Expected O, but got Unknown
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Expected O, but got Unknown
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Expected O, but got Unknown
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Expected O, but got Unknown
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Expected O, but got Unknown
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Expected O, but got Unknown
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Expected O, but got Unknown
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Expected O, but got Unknown
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Expected O, but got Unknown
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Expected O, but got Unknown
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Expected O, but got Unknown
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Expected O, but got Unknown
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Expected O, but got Unknown
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Expected O, but got Unknown
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Expected O, but got Unknown
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Expected O, but got Unknown
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Expected O, but got Unknown
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Expected O, but got Unknown
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Expected O, but got Unknown
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Expected O, but got Unknown
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Expected O, but got Unknown
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Expected O, but got Unknown
		//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fa: Expected O, but got Unknown
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0526: Expected O, but got Unknown
		//IL_05ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Expected O, but got Unknown
		//IL_062f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0639: Expected O, but got Unknown
		//IL_065b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0665: Expected O, but got Unknown
		//IL_06d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06de: Expected O, but got Unknown
		//IL_06df: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e9: Expected O, but got Unknown
		//IL_06f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ff: Expected O, but got Unknown
		//IL_0c38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c42: Expected O, but got Unknown
		//IL_0c4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c59: Expected O, but got Unknown
		//IL_3317: Unknown result type (might be due to invalid IL or missing references)
		//IL_3321: Expected O, but got Unknown
		//IL_3345: Unknown result type (might be due to invalid IL or missing references)
		//IL_334f: Expected O, but got Unknown
		//IL_4797: Unknown result type (might be due to invalid IL or missing references)
		//IL_47a1: Expected O, but got Unknown
		//IL_47ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_47b8: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		GridEXLayout GrdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		GridEXLayout GrdiRegister_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance76 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
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
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.StoreManagement.GrnStore));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label10 = new System.Windows.Forms.Label();
		this.CmbVehicleType = new UltraCombo();
		this.CmbBillToParty = new UltraCombo();
		this.txtFreightAmount = new System.Windows.Forms.TextBox();
		this.txtReferenceNo = new System.Windows.Forms.TextBox();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.txtBiltyNo = new System.Windows.Forms.TextBox();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label26 = new System.Windows.Forms.Label();
		this.txtRemarksHeader = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label48 = new System.Windows.Forms.Label();
		this.CmbTransporter = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.CmbGpNo = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ChkPrint = new System.Windows.Forms.CheckBox();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.BtnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnLoadPurchaseOrder = new System.Windows.Forms.ToolStripButton();
		this.BtnLoadPurchaseDemand = new System.Windows.Forms.ToolStripButton();
		this.BtnLoadDeliveryChallan = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel16 = new System.Windows.Forms.Panel();
		this.panel21 = new System.Windows.Forms.Panel();
		this.GrdHistoryDetail = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label32 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.GrdHistory = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.label49 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.cmbSupplierNameHistory = new UltraCombo();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.tabRegister = new System.Windows.Forms.TabPage();
		this.GrdiRegister = new GridEX();
		this.panel17 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel19 = new System.Windows.Forms.Panel();
		this.CmbPartyNameRegister = new UltraCombo();
		this.txtDocNoToRegister = new System.Windows.Forms.TextBox();
		this.txtDateToRegister = new System.Windows.Forms.DateTimePicker();
		this.txtDatefromRegister = new System.Windows.Forms.DateTimePicker();
		this.txtDocNoFromRegister = new System.Windows.Forms.TextBox();
		this.BtnShowRegister = new System.Windows.Forms.Button();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbTransporterRegister = new UltraCombo();
		this.CmbWarehouseRegister = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.CmbItemNameRegister = new UltraCombo();
		this.panel18 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnResetRegister = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshRegister = new System.Windows.Forms.ToolStripButton();
		this.BtnRegisterPrint = new System.Windows.Forms.ToolStripButton();
		this.label8 = new System.Windows.Forms.Label();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel12.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbVehicleType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBillToParty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransporter).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel16.SuspendLayout();
		this.panel21.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).BeginInit();
		this.panel11.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).BeginInit();
		this.panel15.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierNameHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.tabRegister.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdiRegister).BeginInit();
		this.panel17.SuspendLayout();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyNameRegister).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransporterRegister).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseRegister).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameRegister).BeginInit();
		this.panel18.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(984, 661);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Controls.Add(this.tabRegister);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(984, 661);
		this.tabControl1.TabIndex = 0;
		this.tabPage1.BackColor = System.Drawing.Color.White;
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage1.Controls.Add(this.panel12);
		this.tabPage1.Controls.Add(this.panel13);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(976, 632);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.grd.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		this.grd.ColumnAutoResize = true;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(2, 170);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(972, 460);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.ctrlGrdBar1);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(2, 147);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(972, 23);
		this.panel12.TabIndex = 3;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(926, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(44, 22);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel13.Controls.Add(this.label10);
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.CmbVehicleType);
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBillToParty);
		this.panel13.Controls.Add(this.txtFreightAmount);
		this.panel13.Controls.Add(this.txtReferenceNo);
		this.panel13.Controls.Add(this.txtVehicleNo);
		this.panel13.Controls.Add(this.txtBiltyNo);
		this.panel13.Controls.Add(this.DocDate);
		this.panel13.Controls.Add(this.label26);
		this.panel13.Controls.Add(this.txtRemarksHeader);
		this.panel13.Controls.Add(this.label25);
		this.panel13.Controls.Add(this.label5);
		this.panel13.Controls.Add(this.label21);
		this.panel13.Controls.Add(this.label24);
		this.panel13.Controls.Add(this.txtdocno);
		this.panel13.Controls.Add(this.label48);
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransporter);
		this.panel13.Controls.Add(this.label19);
		this.panel13.Controls.Add(this.CmbGpNo);
		this.panel13.Controls.Add(this.label2);
		this.panel13.Controls.Add(this.label7);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(2, 59);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(972, 88);
		this.panel13.TabIndex = 0;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(6, 64);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(78, 15);
		this.label10.TabIndex = 6;
		this.label10.Text = "Reference No";
		this.CmbVehicleType.AllowNull = (DefaultableBoolean)1;
		this.CmbVehicleType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbVehicleType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbVehicleType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbVehicleType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.CmbVehicleType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance89).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbVehicleType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbVehicleType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbVehicleType).Location = new System.Drawing.Point(379, 31);
		((System.Windows.Forms.Control)(object)this.CmbVehicleType).Name = "CmbVehicleType";
		((System.Windows.Forms.Control)(object)this.CmbVehicleType).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.CmbVehicleType).TabIndex = 11;
		this.CmbBillToParty.AllowNull = (DefaultableBoolean)1;
		this.CmbBillToParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBillToParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBillToParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBillToParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.CmbBillToParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance42).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance43).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBillToParty).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbBillToParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBillToParty).Location = new System.Drawing.Point(95, 31);
		((System.Windows.Forms.Control)(object)this.CmbBillToParty).Name = "CmbBillToParty";
		((System.Windows.Forms.Control)(object)this.CmbBillToParty).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.CmbBillToParty).TabIndex = 5;
		this.txtFreightAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreightAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFreightAmount.Location = new System.Drawing.Point(767, 33);
		this.txtFreightAmount.Name = "txtFreightAmount";
		this.txtFreightAmount.Size = new System.Drawing.Size(201, 23);
		this.txtFreightAmount.TabIndex = 21;
		this.txtReferenceNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtReferenceNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtReferenceNo.Location = new System.Drawing.Point(95, 60);
		this.txtReferenceNo.Name = "txtReferenceNo";
		this.txtReferenceNo.Size = new System.Drawing.Size(207, 23);
		this.txtReferenceNo.TabIndex = 7;
		this.txtVehicleNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtVehicleNo.Location = new System.Drawing.Point(565, 5);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.Size = new System.Drawing.Size(110, 23);
		this.txtVehicleNo.TabIndex = 13;
		this.txtBiltyNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBiltyNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBiltyNo.Location = new System.Drawing.Point(565, 33);
		this.txtBiltyNo.Name = "txtBiltyNo";
		this.txtBiltyNo.Size = new System.Drawing.Size(110, 23);
		this.txtBiltyNo.TabIndex = 15;
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(172, 5);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(130, 22);
		this.DocDate.TabIndex = 3;
		this.DocDate.TabStop = false;
		this.DocDate.ValueChanged += new System.EventHandler(DocDate_ValueChanged);
		this.DocDate.Leave += new System.EventHandler(DocDate_ValueChanged);
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(676, 37);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(91, 15);
		this.label26.TabIndex = 20;
		this.label26.Text = "Freight Amount";
		this.txtRemarksHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksHeader.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRemarksHeader.Location = new System.Drawing.Point(379, 60);
		this.txtRemarksHeader.Name = "txtRemarksHeader";
		this.txtRemarksHeader.Size = new System.Drawing.Size(589, 23);
		this.txtRemarksHeader.TabIndex = 17;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(502, 37);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(49, 15);
		this.label25.TabIndex = 14;
		this.label25.Text = "Bilty No";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(6, 9);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(88, 15);
		this.label5.TabIndex = 0;
		this.label5.Text = "Doc No && Date";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(307, 37);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(73, 15);
		this.label21.TabIndex = 10;
		this.label21.Text = "Vehicle Type";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(307, 64);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(52, 15);
		this.label24.TabIndex = 16;
		this.label24.Text = "Remarks";
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtdocno.Location = new System.Drawing.Point(95, 5);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(75, 23);
		this.txtdocno.TabIndex = 1;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label48.Location = new System.Drawing.Point(676, 9);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(67, 15);
		this.label48.TabIndex = 18;
		this.label48.Text = "Transporter";
		this.CmbTransporter.AllowNull = (DefaultableBoolean)1;
		this.CmbTransporter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransporter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransporter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransporter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance96;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransporter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbTransporter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Location = new System.Drawing.Point(767, 3);
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Name = "CmbTransporter";
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.CmbTransporter).TabIndex = 19;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(502, 9);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(64, 15);
		this.label19.TabIndex = 12;
		this.label19.Text = "Vehicle No";
		this.CmbGpNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.CmbGpNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbGpNo.Location = new System.Drawing.Point(379, 5);
		this.CmbGpNo.Name = "CmbGpNo";
		this.CmbGpNo.Size = new System.Drawing.Size(120, 23);
		this.CmbGpNo.TabIndex = 9;
		this.CmbGpNo.Leave += new System.EventHandler(combgatepass_Leave);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(6, 37);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(86, 15);
		this.label2.TabIndex = 4;
		this.label2.Text = "Supplier Name";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(307, 9);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(57, 15);
		this.label7.TabIndex = 8;
		this.label7.Text = "Gate Pass";
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.ChkPrint);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 33);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(972, 26);
		this.panel3.TabIndex = 0;
		this.ChkPrint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrint.AutoSize = true;
		this.ChkPrint.Checked = true;
		this.ChkPrint.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrint.ForeColor = System.Drawing.Color.White;
		this.ChkPrint.Location = new System.Drawing.Point(845, 4);
		this.ChkPrint.Name = "ChkPrint";
		this.ChkPrint.Size = new System.Drawing.Size(122, 20);
		this.ChkPrint.TabIndex = 25;
		this.ChkPrint.Text = "Print Preview";
		this.ChkPrint.UseVisualStyleBackColor = true;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(4, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(195, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "Goods Receiving Notes";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(972, 31);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[11]
		{
			this.btnNew, this.BtnRefresh, this.btnUpdate, this.btnSave, this.btnDelete, this.BtnPrint, this.btnAttachment, this.btnLoadPurchaseOrder, this.BtnLoadPurchaseDemand, this.BtnLoadDeliveryChallan,
			this.BtnShortCutkeys
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(972, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.BtnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefresh.Name = "BtnRefresh";
		this.BtnRefresh.Size = new System.Drawing.Size(80, 24);
		this.BtnRefresh.Text = "&Refresh";
		this.BtnRefresh.Click += new System.EventHandler(toolStripButton1_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.BtnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint.Name = "BtnPrint";
		this.BtnPrint.Size = new System.Drawing.Size(92, 24);
		this.BtnPrint.Text = "212-Print";
		this.BtnPrint.Click += new System.EventHandler(printToolStripButton_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnLoadPurchaseOrder.Image = Architecture.WinApp.Properties.Resources.download;
		this.btnLoadPurchaseOrder.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadPurchaseOrder.Name = "btnLoadPurchaseOrder";
		this.btnLoadPurchaseOrder.Size = new System.Drawing.Size(85, 24);
		this.btnLoadPurchaseOrder.Text = "Load PO";
		this.btnLoadPurchaseOrder.Click += new System.EventHandler(btnLoadSupplySchedule_Click);
		this.BtnLoadPurchaseDemand.Image = Architecture.WinApp.Properties.Resources.download;
		this.BtnLoadPurchaseDemand.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadPurchaseDemand.Name = "BtnLoadPurchaseDemand";
		this.BtnLoadPurchaseDemand.Size = new System.Drawing.Size(119, 24);
		this.BtnLoadPurchaseDemand.Text = "Load Demand";
		this.BtnLoadPurchaseDemand.Click += new System.EventHandler(BtnLoadPurchaseDemand_Click);
		this.BtnLoadDeliveryChallan.Image = Architecture.WinApp.Properties.Resources.download;
		this.BtnLoadDeliveryChallan.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadDeliveryChallan.Name = "BtnLoadDeliveryChallan";
		this.BtnLoadDeliveryChallan.Size = new System.Drawing.Size(171, 24);
		this.BtnLoadDeliveryChallan.Text = "Load Delivery Challan";
		this.BtnLoadDeliveryChallan.Click += new System.EventHandler(BtnLoadDeliveryChallan_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(976, 632);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add(this.panel6);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(2, 58);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(972, 572);
		this.panel8.TabIndex = 1;
		this.panel6.Controls.Add(this.panel16);
		this.panel6.Controls.Add(this.panel15);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(972, 572);
		this.panel6.TabIndex = 45;
		this.panel16.Controls.Add(this.panel21);
		this.panel16.Controls.Add(this.panel11);
		this.panel16.Controls.Add(this.panel9);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel16.Location = new System.Drawing.Point(0, 68);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(972, 504);
		this.panel16.TabIndex = 47;
		this.panel21.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistoryDetail);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel21.Location = new System.Drawing.Point(0, 351);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(972, 153);
		this.panel21.TabIndex = 49;
		this.GrdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		GrdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistoryDetail.DesignTimeLayout = GrdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdHistoryDetail.DynamicFiltering = true;
		this.GrdHistoryDetail.FilterMode = (FilterMode)2;
		this.GrdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Name = "GrdHistoryDetail";
		((ScrollableBase)this.GrdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Size = new System.Drawing.Size(972, 153);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).TabIndex = 46;
		this.GrdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.GrdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label32);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 330);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(972, 21);
		this.panel11.TabIndex = 48;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label32.ForeColor = System.Drawing.Color.White;
		this.label32.Location = new System.Drawing.Point(6, 2);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(230, 17);
		this.label32.TabIndex = 24;
		this.label32.Text = "Detail of the Selected above row";
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistory);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(972, 330);
		this.panel9.TabIndex = 47;
		this.GrdHistory.AllowDelete = (InheritableBoolean)1;
		GrdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistory.DesignTimeLayout = GrdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdHistory.DynamicFiltering = true;
		this.GrdHistory.FilterMode = (FilterMode)2;
		this.GrdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistory.FrozenColumns = 3;
		this.GrdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GrdHistory).Name = "GrdHistory";
		((ScrollableBase)this.GrdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Size = new System.Drawing.Size(972, 330);
		((System.Windows.Forms.Control)(object)this.GrdHistory).TabIndex = 45;
		this.GrdHistory.TotalRow = (InheritableBoolean)1;
		this.GrdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistory).VisualStyle = (VisualStyle)4;
		this.GrdHistory.ColumnButtonClick += new ColumnActionEventHandler(GrdHistory_ColumnButtonClick);
		this.GrdHistory.SelectionChanged += new System.EventHandler(GrdHistory_SelectionChanged);
		this.GrdHistory.LinkClicked += new ColumnActionEventHandler(GrdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdHistory).DoubleClick += new System.EventHandler(GrdHistory_DoubleClick);
		this.panel15.Controls.Add(this.groupBox3);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(972, 68);
		this.panel15.TabIndex = 46;
		this.groupBox3.Controls.Add(this.rdapproveddate);
		this.groupBox3.Controls.Add(this.rdmodifydate);
		this.groupBox3.Controls.Add(this.drdocdate);
		this.groupBox3.Controls.Add(this.rdentrydate);
		this.groupBox3.Controls.Add(this.btnshow);
		this.groupBox3.Controls.Add(this.label49);
		this.groupBox3.Controls.Add(this.label50);
		this.groupBox3.Controls.Add(this.label51);
		this.groupBox3.Controls.Add(this.label52);
		this.groupBox3.Controls.Add(this.label53);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory);
		this.groupBox3.Controls.Add(this.txtToDocNoHistory);
		this.groupBox3.Controls.Add(this.txtFromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(6, -1);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(973, 66);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(863, 44);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 146;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(863, 22);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 145;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(782, 22);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 143;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(782, 44);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 144;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(721, 36);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(53, 26);
		this.btnshow.TabIndex = 5;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.btnshow.Leave += new System.EventHandler(btnshow_Leave);
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label49.Location = new System.Drawing.Point(479, 19);
		this.label49.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(86, 15);
		this.label49.TabIndex = 142;
		this.label49.Text = "Supplier Name";
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label50.Location = new System.Drawing.Point(378, 19);
		this.label50.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(64, 15);
		this.label50.TabIndex = 141;
		this.label50.Text = "To Doc No";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label51.Location = new System.Drawing.Point(276, 19);
		this.label51.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(79, 15);
		this.label51.TabIndex = 140;
		this.label51.Text = "From Doc No";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label52.Location = new System.Drawing.Point(143, 19);
		this.label52.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(48, 15);
		this.label52.TabIndex = 139;
		this.label52.Text = "To Date";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label53.Location = new System.Drawing.Point(5, 20);
		this.label53.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(63, 15);
		this.label53.TabIndex = 13;
		this.label53.Text = "From Date";
		this.cmbSupplierNameHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplierNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplierNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplierNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance92).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplierNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Location = new System.Drawing.Point(479, 36);
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Name = "cmbSupplierNameHistory";
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Size = new System.Drawing.Size(240, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).TabIndex = 4;
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
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label1);
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Controls.Add(this.toolStrip2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 2);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(972, 56);
		this.panel7.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(6, 32);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(258, 19);
		this.label1.TabIndex = 0;
		this.label1.Text = "Goods Receiving Notes History";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(925, 27);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.GrdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 29);
		this.ctrlGrdBar2.TabIndex = 26;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(972, 27);
		this.toolStrip2.TabIndex = 24;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(68, 24);
		this.btnNewHistory.Text = "&Reset";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "&Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.tabRegister.Controls.Add((System.Windows.Forms.Control)(object)this.GrdiRegister);
		this.tabRegister.Controls.Add(this.panel17);
		this.tabRegister.Controls.Add(this.panel19);
		this.tabRegister.Controls.Add(this.panel18);
		this.tabRegister.Location = new System.Drawing.Point(4, 4);
		this.tabRegister.Name = "tabRegister";
		this.tabRegister.Size = new System.Drawing.Size(976, 632);
		this.tabRegister.TabIndex = 2;
		this.tabRegister.Text = "Register";
		this.tabRegister.UseVisualStyleBackColor = true;
		this.GrdiRegister.AllowEdit = (InheritableBoolean)2;
		GrdiRegister_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdiRegister.DesignTimeLayout = GrdiRegister_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdiRegister).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdiRegister.DynamicFiltering = true;
		this.GrdiRegister.FilterMode = (FilterMode)2;
		this.GrdiRegister.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdiRegister).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdiRegister.FrozenColumns = 1;
		this.GrdiRegister.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GrdiRegister).Location = new System.Drawing.Point(0, 141);
		((System.Windows.Forms.Control)(object)this.GrdiRegister).Name = "GrdiRegister";
		((ScrollableBase)this.GrdiRegister).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdiRegister).Size = new System.Drawing.Size(976, 491);
		((System.Windows.Forms.Control)(object)this.GrdiRegister).TabIndex = 1;
		this.GrdiRegister.TotalRow = (InheritableBoolean)1;
		this.GrdiRegister.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdiRegister).VisualStyle = (VisualStyle)4;
		this.GrdiRegister.ColumnButtonClick += new ColumnActionEventHandler(GrdiRegister_ColumnButtonClick);
		this.GrdiRegister.LinkClicked += new ColumnActionEventHandler(GrdiRegister_LinkClicked);
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.ctrlGrdBar3);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(0, 116);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(976, 25);
		this.panel17.TabIndex = 0;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(932, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.GrdiRegister;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(41, 22);
		this.ctrlGrdBar3.TabIndex = 1;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPartyNameRegister);
		this.panel19.Controls.Add(this.txtDocNoToRegister);
		this.panel19.Controls.Add(this.txtDateToRegister);
		this.panel19.Controls.Add(this.txtDatefromRegister);
		this.panel19.Controls.Add(this.txtDocNoFromRegister);
		this.panel19.Controls.Add(this.BtnShowRegister);
		this.panel19.Controls.Add(this.label4);
		this.panel19.Controls.Add(this.label3);
		this.panel19.Controls.Add(this.label11);
		this.panel19.Controls.Add(this.label12);
		this.panel19.Controls.Add(this.label6);
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransporterRegister);
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouseRegister);
		this.panel19.Controls.Add(this.label14);
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemNameRegister);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel19.Location = new System.Drawing.Point(0, 51);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(976, 65);
		this.panel19.TabIndex = 0;
		this.CmbPartyNameRegister.AllowNull = (DefaultableBoolean)1;
		this.CmbPartyNameRegister.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPartyNameRegister.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPartyNameRegister.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPartyNameRegister).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbPartyNameRegister).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPartyNameRegister).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPartyNameRegister).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPartyNameRegister).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance68;
		((SpecialBoxBase)((UltraGridBase)this.CmbPartyNameRegister).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPartyNameRegister).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbPartyNameRegister).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPartyNameRegister).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPartyNameRegister).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPartyNameRegister).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbPartyNameRegister).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPartyNameRegister).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPartyNameRegister).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPartyNameRegister).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbPartyNameRegister).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPartyNameRegister).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPartyNameRegister).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPartyNameRegister).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbPartyNameRegister).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPartyNameRegister).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPartyNameRegister).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbPartyNameRegister).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPartyNameRegister).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbPartyNameRegister).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPartyNameRegister).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPartyNameRegister).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPartyNameRegister).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPartyNameRegister.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPartyNameRegister).Location = new System.Drawing.Point(444, 3);
		((System.Windows.Forms.Control)(object)this.CmbPartyNameRegister).Name = "CmbPartyNameRegister";
		((System.Windows.Forms.Control)(object)this.CmbPartyNameRegister).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbPartyNameRegister).TabIndex = 4;
		this.txtDocNoToRegister.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoToRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoToRegister.Location = new System.Drawing.Point(235, 33);
		this.txtDocNoToRegister.Name = "txtDocNoToRegister";
		this.txtDocNoToRegister.Size = new System.Drawing.Size(140, 23);
		this.txtDocNoToRegister.TabIndex = 3;
		this.txtDateToRegister.CustomFormat = "dd-MMM-yy";
		this.txtDateToRegister.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtDateToRegister.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDateToRegister.Location = new System.Drawing.Point(235, 5);
		this.txtDateToRegister.Name = "txtDateToRegister";
		this.txtDateToRegister.Size = new System.Drawing.Size(140, 22);
		this.txtDateToRegister.TabIndex = 1;
		this.txtDateToRegister.TabStop = false;
		this.txtDatefromRegister.CustomFormat = "dd-MMM-yy";
		this.txtDatefromRegister.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtDatefromRegister.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDatefromRegister.Location = new System.Drawing.Point(93, 5);
		this.txtDatefromRegister.Name = "txtDatefromRegister";
		this.txtDatefromRegister.Size = new System.Drawing.Size(140, 22);
		this.txtDatefromRegister.TabIndex = 0;
		this.txtDatefromRegister.TabStop = false;
		this.txtDocNoFromRegister.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoFromRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoFromRegister.Location = new System.Drawing.Point(93, 33);
		this.txtDocNoFromRegister.Name = "txtDocNoFromRegister";
		this.txtDocNoFromRegister.Size = new System.Drawing.Size(140, 23);
		this.txtDocNoFromRegister.TabIndex = 2;
		this.BtnShowRegister.BackColor = System.Drawing.Color.Teal;
		this.BtnShowRegister.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.BtnShowRegister.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShowRegister.ForeColor = System.Drawing.Color.White;
		this.BtnShowRegister.Location = new System.Drawing.Point(917, 32);
		this.BtnShowRegister.Name = "BtnShowRegister";
		this.BtnShowRegister.Size = new System.Drawing.Size(54, 24);
		this.BtnShowRegister.TabIndex = 8;
		this.BtnShowRegister.Text = "Show";
		this.BtnShowRegister.UseVisualStyleBackColor = false;
		this.BtnShowRegister.Click += new System.EventHandler(BtnShowRegister_Click);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(3, 9);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(91, 15);
		this.label4.TabIndex = 10;
		this.label4.Text = "From && To Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(376, 9);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(69, 15);
		this.label3.TabIndex = 12;
		this.label3.Text = "Party Name";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(376, 37);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(67, 15);
		this.label11.TabIndex = 12;
		this.label11.Text = "Transporter";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(645, 9);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(67, 15);
		this.label12.TabIndex = 12;
		this.label12.Text = "Item Name";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(3, 37);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(88, 15);
		this.label6.TabIndex = 14;
		this.label6.Text = "From && To Doc";
		this.CmbTransporterRegister.AllowNull = (DefaultableBoolean)1;
		this.CmbTransporterRegister.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransporterRegister.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransporterRegister.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransporterRegister).DisplayLayout.Appearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbTransporterRegister).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransporterRegister).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransporterRegister).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransporterRegister).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance53;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransporterRegister).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransporterRegister).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbTransporterRegister).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransporterRegister).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransporterRegister).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransporterRegister).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbTransporterRegister).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransporterRegister).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransporterRegister).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransporterRegister).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbTransporterRegister).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransporterRegister).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransporterRegister).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransporterRegister).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbTransporterRegister).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransporterRegister).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransporterRegister).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbTransporterRegister).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransporterRegister).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbTransporterRegister).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransporterRegister).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransporterRegister).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransporterRegister).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbTransporterRegister.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransporterRegister).Location = new System.Drawing.Point(444, 31);
		((System.Windows.Forms.Control)(object)this.CmbTransporterRegister).Name = "CmbTransporterRegister";
		((System.Windows.Forms.Control)(object)this.CmbTransporterRegister).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbTransporterRegister).TabIndex = 5;
		this.CmbWarehouseRegister.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouseRegister.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouseRegister.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouseRegister.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouseRegister).DisplayLayout.Appearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbWarehouseRegister).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouseRegister).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseRegister).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseRegister).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance17;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseRegister).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseRegister).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbWarehouseRegister).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouseRegister).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouseRegister).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouseRegister).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbWarehouseRegister).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouseRegister).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseRegister).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouseRegister).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbWarehouseRegister).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouseRegister).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseRegister).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouseRegister).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbWarehouseRegister).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouseRegister).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouseRegister).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbWarehouseRegister).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouseRegister).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbWarehouseRegister).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouseRegister).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouseRegister).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseRegister).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWarehouseRegister.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseRegister).Location = new System.Drawing.Point(714, 31);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseRegister).Name = "CmbWarehouseRegister";
		((System.Windows.Forms.Control)(object)this.CmbWarehouseRegister).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseRegister).TabIndex = 7;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(645, 37);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(67, 15);
		this.label14.TabIndex = 12;
		this.label14.Text = "Warehouse";
		this.CmbItemNameRegister.AllowNull = (DefaultableBoolean)1;
		this.CmbItemNameRegister.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemNameRegister.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemNameRegister.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemNameRegister).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbItemNameRegister).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemNameRegister).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameRegister).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameRegister).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameRegister).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameRegister).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbItemNameRegister).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemNameRegister).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemNameRegister).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemNameRegister).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbItemNameRegister).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemNameRegister).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameRegister).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance41).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemNameRegister).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbItemNameRegister).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemNameRegister).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameRegister).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemNameRegister).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbItemNameRegister).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemNameRegister).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemNameRegister).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbItemNameRegister).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemNameRegister).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbItemNameRegister).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemNameRegister).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemNameRegister).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemNameRegister).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbItemNameRegister.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemNameRegister).Location = new System.Drawing.Point(714, 3);
		((System.Windows.Forms.Control)(object)this.CmbItemNameRegister).Name = "CmbItemNameRegister";
		((System.Windows.Forms.Control)(object)this.CmbItemNameRegister).Size = new System.Drawing.Size(257, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemNameRegister).TabIndex = 6;
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.toolStrip3);
		this.panel18.Controls.Add(this.label8);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 0);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(976, 51);
		this.panel18.TabIndex = 0;
		this.toolStrip3.BackColor = System.Drawing.Color.White;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.toolStripSeparator2, this.BtnResetRegister, this.BtnRefreshRegister, this.BtnRegisterPrint });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(976, 25);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
		this.BtnResetRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnResetRegister.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnResetRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnResetRegister.Name = "BtnResetRegister";
		this.BtnResetRegister.Size = new System.Drawing.Size(64, 22);
		this.BtnResetRegister.Text = "&Reset";
		this.BtnResetRegister.Click += new System.EventHandler(BtnResetRegister_Click);
		this.BtnRefreshRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshRegister.Image = (System.Drawing.Image)resources.GetObject("BtnRefreshRegister.Image");
		this.BtnRefreshRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshRegister.Name = "BtnRefreshRegister";
		this.BtnRefreshRegister.Size = new System.Drawing.Size(76, 22);
		this.BtnRefreshRegister.Text = "Refresh";
		this.BtnRefreshRegister.Click += new System.EventHandler(BtnRefresh_Click);
		this.BtnRegisterPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRegisterPrint.ForeColor = System.Drawing.Color.Black;
		this.BtnRegisterPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnRegisterPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRegisterPrint.Name = "BtnRegisterPrint";
		this.BtnRegisterPrint.Size = new System.Drawing.Size(88, 22);
		this.BtnRegisterPrint.Text = "336-Print";
		this.BtnRegisterPrint.Click += new System.EventHandler(BtnRegisterPrint_Click);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(3, 28);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(269, 19);
		this.label8.TabIndex = 10;
		this.label8.Text = "Goods Receiving Notes Register";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(984, 661);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "GrnStore";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(GrnStore_FormClosing);
		base.Load += new System.EventHandler(InvFrmGRN_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvFrmGRN_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbVehicleType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBillToParty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransporter).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel16.ResumeLayout(false);
		this.panel21.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).EndInit();
		this.panel15.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierNameHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.tabRegister.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdiRegister).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		this.panel19.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyNameRegister).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransporterRegister).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseRegister).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameRegister).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
